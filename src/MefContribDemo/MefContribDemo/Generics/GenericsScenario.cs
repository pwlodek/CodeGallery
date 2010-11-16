using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MefContrib.Hosting.Generics;
using MefContrib.Hosting.Interception;
using MefContrib.Hosting.Interception.Configuration;

namespace MefContribDemo.Generics
{
    /// <summary>
    /// Shows how to use open-generics support. <see cref="CreateContainer1"/>
    /// and <see cref="CreateContainer2"/> mothods are equivalents.
    /// </summary>
    public class GenericsScenario
    {
        [Import]
        public Trampoline Test { get; set; }

        private static CompositionContainer CreateContainer1()
        {
            // Create source catalog
            var typeCatalog = new TypeCatalog(typeof(Trampoline), typeof(MyGenericContractRegistry));

            // Create the interception configuration and add support for open generics
            var cfg = new InterceptionConfiguration()
                .AddHandler(new GenericExportHandler());

            // Create the InterceptingCatalog and pass the configuration
            var interceptingCatalog = new InterceptingCatalog(typeCatalog, cfg);

            // Create the container
            return new CompositionContainer(interceptingCatalog);
        }

        private static CompositionContainer CreateContainer2()
        {
            // Create source catalog
            var typeCatalog = new TypeCatalog(typeof(Trampoline));

            // Create catalog which supports open-generics, pass in the registry
            var genericCatalog = new GenericCatalog(new MyGenericContractRegistry());

            // Aggregate the both catalogs
            var aggregateCatalog = new AggregateCatalog(typeCatalog, genericCatalog);

            // Create the container
            return new CompositionContainer(aggregateCatalog);
        }

        public void Run()
        {
            Console.WriteLine("\n*** Generics Scenario ***");

            // Create the container
            var container = CreateContainer2();

            container.SatisfyImportsOnce(this);

            // Test the open generics support
            Test.Repository.Save(new Customer());
        }

        [Export]
        public class Trampoline
        {
            [Import]
            public IRepository<Customer> Repository { get; set; }
        }
    }
}