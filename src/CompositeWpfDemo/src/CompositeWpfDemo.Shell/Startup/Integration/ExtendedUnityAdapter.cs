using System;
using System.Collections.Generic;
using Caliburn.Unity;
using Microsoft.Practices.Unity;

namespace CompositeWpfDemo.Shell.Startup.Integration
{
    public class ExtendedUnityAdapter : UnityAdapter
    {
        public ExtendedUnityAdapter(IUnityContainer container) : base(container)
        {
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            object instance = null;
            try
            {
                instance = GetInstance(serviceType);
            }
            catch
            {
            }

            if (instance != null)
                yield return instance;

            foreach (var allInstance in base.DoGetAllInstances(serviceType))
            {
                yield return allInstance;
            }
        }
    }
}