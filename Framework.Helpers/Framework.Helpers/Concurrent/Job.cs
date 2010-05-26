using System;
using System.Collections.Generic;

namespace VisionCode.Framework.Helpers.Concurrent
{
    [Serializable]
    public class Job
    {
        private Guid m_ID;
        private IList<Task> m_Tasks;

        public Job()
        {
            m_Tasks = new List<Task>();
            m_ID = Guid.NewGuid();
        }

        public Job(IList<Task> tasks)
        {
            m_Tasks = tasks;
            m_ID = Guid.NewGuid();
        }

        public Guid ID
        {
            get { return m_ID; }
        }

        public IList<Task> Tasks
        {
            get { return m_Tasks; }
            set { m_Tasks = value; }
        }

        public sealed override bool Equals(object obj)
        {
            if (obj == this) return true;
            Job job = obj as Job;
            if (job != null && ID.Equals(job.ID))
            {
                return true;
            }
            return false;
        }

        public sealed override int GetHashCode()
        {
            int hash = 17;
            hash = 37 * hash + ID.GetHashCode();
            return hash;
        }
    }
}
