using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VisionCode.Framework.Helpers.Disk
{
    public class DiskPersistor : IDisposable
    {
        private FileStream m_FileStream;

        public DiskPersistor(string filename)
        {
            m_FileStream = File.Open(filename, FileMode.OpenOrCreate);
        }

        public void Save<T>(T obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(m_FileStream, obj);
        }

        public void Save<T>(T obj, int pos)
        {
            Position = pos;
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(m_FileStream, obj);
        }

        public T Load<T>()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            object obj = formatter.Deserialize(m_FileStream);
            return (T)obj;
        }

        public T Load<T>(int pos)
        {
            Position = pos;
            BinaryFormatter formatter = new BinaryFormatter();
            object obj = formatter.Deserialize(m_FileStream);
            return (T)obj;
        }

        public IList<T> LoadAll<T>()
        {
            IList<T> objects = new List<T>();
            Position = 0;
            BinaryFormatter formatter = new BinaryFormatter();
            while (m_FileStream.Length > m_FileStream.Position)
            {
                object obj = formatter.Deserialize(m_FileStream);
                objects.Add((T)obj);
            }
            return objects;
        }

        public void Close()
        {
            if (m_FileStream != null)
            {
                m_FileStream.Close();
            }
        }

        public long Position
        {
            get { return m_FileStream.Position; }
            set { m_FileStream.Position = value; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion
    }
}
