using System;
using System.ComponentModel.Composition.Hosting;
using MefContrib.Hosting.Interception;
using MefContrib.Hosting.Interception.Castle;
using MefContrib.Hosting.Interception.Configuration;

namespace MefContribDemo.Interception
{
    public class InterceptionScenario
    {
        public void Run()
        {
            Console.WriteLine("\n*** Interception Scenario ***");

            var catalog = new TypeCatalog(typeof(Bar), typeof(Foo));
            var cfg = new InterceptionConfiguration()
                .AddInterceptionCriteria(
                    new LogInterceptionCriteria(
                        new DynamicProxyInterceptor(new LoggingInterceptor())))
                .AddInterceptor(new StartableStrategy());
            var interceptingCatalog = new InterceptingCatalog(catalog, cfg);
            var container = new CompositionContainer(interceptingCatalog);

            var barPart = container.GetExportedValue<IBar>();
            barPart.Foo();

            var fooPart = container.GetExportedValue<IFoo>();
            fooPart.Bar();
        }
    }
}