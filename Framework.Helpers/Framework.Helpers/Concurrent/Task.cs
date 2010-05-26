using System;
using System.Collections.Generic;
using System.Text;

namespace VisionCode.Framework.Helpers.Concurrent
{
    [Serializable]
    public abstract class Task
    {
        private Guid m_ID;

        public Task()
        {
            m_ID = Guid.NewGuid();
        }

        public Guid ID
        {
            get { return m_ID; }
        }

        public abstract object Run();

        public sealed override bool Equals(object obj)
        {
            if (obj == this) return true;
            Task task = obj as Task;
            if (task != null && ID.Equals(task.ID))
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
