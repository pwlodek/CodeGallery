using System;
using MefContrib.Hosting.Interception;

namespace MefContribDemo.Interception
{
    public class StartableStrategy : IExportedValueInterceptor
    {
        public object Intercept(object value)
        {
            var startable = value as IStartable;
            if (startable != null && !startable.IsStarted)
            {
                startable.Start();
            }

            return value;
        }
    }
}