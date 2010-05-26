using System;
using System.Collections.Generic;
using VisionCode.Framework.Helpers.Events;

namespace VisionCode.Framework.Helpers.Concurrent
{
    public sealed class JobExecutor
    {
        // Events
        public event EventHandler<JobCompletedEventArgs> JobCompleted;

        // Constants
        public const int DefaultPoolSize = 5;

        // Fields
        private ManagedThreadPool m_ThreadPool;
        private IList<Job> m_Jobs;
        private IDictionary<Job, int> m_ResultsCount;
        private IDictionary<Task, object> m_Results;

        public JobExecutor() : this(DefaultPoolSize) { }

        public JobExecutor(int poolSize)
        {
            m_ThreadPool = new ManagedThreadPool(poolSize);
            m_Jobs = new List<Job>();
            m_ResultsCount = new Dictionary<Job, int>();
            m_Results = new Dictionary<Task, object>();
            m_ThreadPool.WorkCompleted += ThreadPool_OnWorkCompleted;
        }

        private void ThreadPool_OnWorkCompleted(object sender, WorkCompletedEventArgs e)
        {
            Task task = e.Task;

            lock (m_Results)
            {
                m_Results[task] = e.Result;
            }

            // Find job that owns this task
            Job owner = null;
            foreach (Job job in m_Jobs)
            {
                foreach (Task jobTask in job.Tasks)
                {
                    if (task.Equals(jobTask))
                    {
                        owner = job;
                        break;
                    }
                }
                if (owner != null)
                {
                    break;
                }
            }

            lock (m_Results)
            lock (m_ResultsCount)
            {
                m_ResultsCount[owner]++;

                // Check whether this task was last in this job
                if (m_ResultsCount[owner] == owner.Tasks.Count)
                {
                    List<object> results = new List<object>(owner.Tasks.Count);
                    foreach (Task ownerTask in owner.Tasks)
                    {
                        object res = m_Results[ownerTask];
                        m_Results.Remove(ownerTask);
                        results.Add(res);
                    }
                    m_ResultsCount.Remove(owner);
                    OnJobCompleted(this, new JobCompletedEventArgs(owner, results.ToArray()));
                } 
            }
        }

        public int WorkersCount
        {
            get { return m_ThreadPool.WorkersCount; }
        }

        public void AddWorkers(int num)
        {
            m_ThreadPool.AddWorkers(num);
        }

        public void RemoveWorkers(int num)
        {
            throw new Exception("This method or operation is not implemented");
        }

        /// <summary>
        /// Adds new job to be executed. Returns immediately.
        /// </summary>
        /// <param name="job"></param>
        public void EnqueueJob(Job job)
        {
            lock (m_Jobs)
            {
                m_Jobs.Add(job);
                foreach (Task task in job.Tasks)
                {
                    m_ThreadPool.QueueUserWorkItem(task);
                }
                lock (m_ResultsCount)
                {
                    m_ResultsCount[job] = 0;
                }
            }
        }

        private void OnJobCompleted(object sender, JobCompletedEventArgs e)
        {
            EventsHelper.Fire<JobCompletedEventArgs>(JobCompleted, this, e);
        }
    }
}
