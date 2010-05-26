using System;
using System.Collections.Generic;
using System.Threading;
using log4net;

namespace VisionCode.Framework.Helpers.Concurrent
{
    public sealed class ManagedThreadPool
    {
        // Events
        public event EventHandler<WorkCompletedEventArgs> WorkCompleted;

        // Constants
        public const int DefaultPoolSize = 5;
        private const string WorkerThreadLoggerName = "Framework.Helpers.Concurrent.ManagedThreadPool.WorkerThread";

        // Fields
        private readonly Thread m_Thread;
        private readonly Queue<WorkerThread> m_WorkerThreads;
        private readonly Queue<Task> m_Tasks;
        private bool m_Stopped;
        private int m_WorkersCount;

        public ManagedThreadPool() : this(DefaultPoolSize) { }

        public ManagedThreadPool(int poolSize)
        {
            m_WorkerThreads = new Queue<WorkerThread>();
            m_Tasks = new Queue<Task>();
            m_Thread = new Thread(Run) { Name = "ThreadPool Manager Thread", IsBackground = true };
            m_WorkersCount = poolSize;

            // lock statement can be omitted here
            for (int i = 0; i < m_WorkersCount; i++)
            {
                m_WorkerThreads.Enqueue(new WorkerThread(this));
            }

            m_Thread.Start();
        }

        public int WorkersCount
        {
            get { return m_WorkersCount; }
        }

        public void AddWorkers(int num)
        {
            lock (m_WorkerThreads)
            {
                m_WorkersCount += num;
                for (int i = 0; i < num; i++)
                {
                    m_WorkerThreads.Enqueue(new WorkerThread(this));
                }
                Monitor.Pulse(m_WorkerThreads);
            }
        }

        //public void RemoveWorkers(int num)
        //{
        //    lock (m_WorkerThreads)
        //    {
        //        int min = Math.Min(num, m_WorkersCount);
        //        m_WorkersCount -= min;
        //        for (int i = 0; i < min; i++)
        //        {
        //            WorkerThread worker = m_WorkerThreads.Dequeue();
        //            lock (worker)
        //            {
        //                Monitor.Pulse(worker);
        //            }
        //        }
        //        Monitor.Pulse(m_WorkerThreads);
        //    }
        //}

        public void Stop()
        {
            m_Stopped = true;
            foreach (WorkerThread worker in m_WorkerThreads)
            {
                lock (worker)
                {
                    Monitor.Pulse(worker);
                }
            }
        }

        public bool Stopped
        {
            get { return m_Stopped; }
        }

        private void Run()
        {
            while (Stopped == false)
            {
                lock (m_Tasks)
                {
                    if (m_Tasks.Count == 0)
                    {
                        Monitor.Wait(m_Tasks);
                    }
                }

                lock (m_WorkerThreads)
                {
                    if (m_WorkerThreads.Count == 0)
                    {
                        Monitor.Wait(m_WorkerThreads);
                    }
                }

                lock (m_WorkerThreads)
                    lock (m_Tasks)
                    {
                        m_WorkerThreads.Dequeue().SetTask(m_Tasks.Dequeue());
                    }
            }
        }

        public void QueueUserWorkItem(Task task)
        {
            lock (m_Tasks)
            {
                m_Tasks.Enqueue(task);
                Monitor.Pulse(m_Tasks);
            }
        }

        public void QueueUserWorkItem<T>(Action<T> action) where T : class
        {
            QueueUserWorkItem(action, null);
        }

        public void QueueUserWorkItem<T>(Action<T> action, T param)
        {
            lock (m_Tasks)
            {
                m_Tasks.Enqueue(new RunnableTask<T>(action, param));
                Monitor.Pulse(m_Tasks);
            }
        }

        private void OnWorkCompleted(object sender, WorkCompletedEventArgs e)
        {
            if (WorkCompleted != null)
            {
                WorkCompleted(sender, e);
            }
        }

        private void ReturnWorker(WorkerThread worker)
        {
            lock (m_WorkerThreads)
            {
                m_WorkerThreads.Enqueue(worker);
                Monitor.Pulse(m_WorkerThreads);
            }
        }

        private sealed class WorkerThread
        {
            // Constants
            private static readonly ILog m_Logger = LogManager.GetLogger(WorkerThreadLoggerName);
            public const string WorkerThreadName = "ThreadPool Worker Thread";
            
            // Fields
            private readonly Thread m_Thread;
            private readonly ManagedThreadPool m_ThreadPool;
            private Task m_Task;

            public WorkerThread(ManagedThreadPool threadPool)
            {
                m_ThreadPool = threadPool;
                m_Thread = new Thread(Run);
                m_Thread.Name = WorkerThreadName;
                m_Thread.IsBackground = true;
                m_Thread.Start();
            }

            public void SetTask(Task task)
            {
                m_Task = task;
                lock (this)
                {
                    Monitor.Pulse(this);
                }
            }

            private void Run()
            {
                while (m_ThreadPool.Stopped == false)
                {
                    lock (this)
                    {
                        if (m_Task != null)
                        {
                            try
                            {
                                object returnVal = m_Task.Run();
                                m_ThreadPool.OnWorkCompleted(m_ThreadPool, new WorkCompletedEventArgs(m_Task, returnVal));
                                m_Task = null;
                            }
                            catch (Exception ex)
                            {
                                m_Logger.Error("Error while executing task", ex);
                            }
                            m_ThreadPool.ReturnWorker(this);
                        }
                        Monitor.Wait(this);
                    }
                }
            }
        }

        private class RunnableTask<T> : Task
        {
            private Action<T> m_Action;
            private T m_Value;

            public RunnableTask(Action<T> action, T param)
            {
                m_Action = action;
                m_Value = param;
            }

            public override object Run()
            {
                if (m_Action != null)
                {
                    m_Action(m_Value);
                }
                return null;
            }
        }
    }
}
