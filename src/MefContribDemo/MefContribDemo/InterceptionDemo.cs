using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Castle.DynamicProxy;
using MefContrib.Hosting.Interception;
using MefContrib.Hosting.Interception.Castle;

namespace MefContribDemo
{
    public class InterceptionDemo
    {
        [Import]
        public ISomeService Service { get; set; }

        public void Run()
        {
            // Create the catalog
            var assemblyCatalog = new AssemblyCatalog(typeof(InterceptionDemo).Assembly);
            
            // Create Castle based interceptor
            var freezableInterceptor = new FreezableInterceptor();

            // Wrap any Castle interceptors with DynamicProxyInterceptor
            var proxyInterceptor = new DynamicProxyInterceptor(freezableInterceptor);
            
            // Create the InterceptingCatalog and pass in the catalog being intercepted
            var interceptingCatalog = new InterceptingCatalog(assemblyCatalog, proxyInterceptor);
            
            // Create the container using intercepting catalog
            var container = new CompositionContainer(interceptingCatalog);

            container.SatisfyImportsOnce(this);

            // Service is now a proxy
            Service.ImportantValue = 123;
            Service.PrintValue();

            // Freeze the interceptor
            freezableInterceptor.Freeze();
            try
            {
                Service.ImportantValue = 1234;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public interface ISomeService
    {
        int ImportantValue { get; set; }
        void PrintValue();
    }

    [Export(typeof(ISomeService))]
    public class SomeService : ISomeService
    {
        public int ImportantValue { get; set; }

        public void PrintValue()
        {
            Console.WriteLine("ImportantValue=" + ImportantValue);
        }
    }

    // Freezable interceptor taken from Krzysztof Kozmic
    public class FreezableInterceptor : IInterceptor, IFreezable
    {
        public void Freeze()
        {
            IsFrozen = true;
        }

        public bool IsFrozen { get; private set; }

        public void Intercept(IInvocation invocation)
        {
            if (IsFrozen && invocation.Method.Name.StartsWith("set_", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Cannot modify a frozen object.");
            }

            invocation.Proceed();
        }
    }

    public interface IFreezable
    {
        bool IsFrozen { get; }
        void Freeze();
    }
}