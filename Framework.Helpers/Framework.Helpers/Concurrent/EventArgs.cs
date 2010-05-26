using System;
using System.Collections.Generic;
using System.Text;

namespace VisionCode.Framework.Helpers.Concurrent
{
    public class WorkCompletedEventArgs : EventArgs
    {
        private Task m_Task;
        private object m_Result;

        public WorkCompletedEventArgs(Task task) : this(task, null) { }

        public WorkCompletedEventArgs(Task task, object val)
        {
            m_Task = task;
            m_Result = val;
        }

        public Task Task
        {
            get { return m_Task; }
        }

        public object Result
        {
            get { return m_Result; }
        }
    }

    public class JobCompletedEventArgs : EventArgs
    {
        private Job m_Job;
        private object[] m_Results;

        public JobCompletedEventArgs(Job job, object[] val)
        {
            m_Job = job;
            m_Results = val;
        }

        public Job Job
        {
            get { return m_Job; }
        }

        public object[] Results
        {
            get { return m_Results; }
        }
    }
}
