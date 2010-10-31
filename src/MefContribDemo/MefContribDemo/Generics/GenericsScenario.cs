using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MefContrib.Hosting.Interception;
using MefContrib.Hosting.Interception.Configuration;
using MefContrib.Hosting.Interception.Handlers;

namespace MefContribDemo.Generics
{
    public class GenericsScenario
    {
        [Import]
        public Trampoline Test { get; set; }

        public void Run()
        {
            Console.WriteLine("\n*** Generics Scenario ***");

            var catalog = new TypeCatalog(typeof(Trampoline), typeof(RepositoryOfTExport));
            
            // Create the interception configuration and add support for open generics
            var cfg = new InterceptionConfiguration()
                .AddHandler(new GenericExportHandler());

            // Create the InterceptingCatalog and pass the configuration
            var interceptingCatalog = new InterceptingCatalog(catalog, cfg);

            // Create the container
            var container = new CompositionContainer(interceptingCatalog);

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