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
            var cfg = new InterceptionConfiguration()
                .AddHandler(new GenericExportHandler());
            var interceptingCatalog = new InterceptingCatalog(catalog, cfg);
            var container = new CompositionContainer(interceptingCatalog);

            container.SatisfyImportsOnce(this);

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