using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using MefContrib.Hosting.Interception;
using MefContrib.Hosting.Interception.Configuration;

namespace MefContribDemo.Filtering
{
    /// <summary>
    /// Typical web scenario.
    /// </summary>
    public class FilteringScenario
    {
        public void Run()
        {
            Console.WriteLine("*** Filtering Scenario ***");

            using (var webApp = new WebApplication())
            {
                using (var request = new Request(webApp.Catalog, webApp.Container))
                {
                    request.RequestContainer.GetExportedValue<INonSharedPart>();
                }

                using (var request = new AnotherRequest(webApp.Catalog, webApp.Container))
                {
                    request.RequestContainer.GetExportedValue<RequestSpecificPart>();
                }
            }
        }
    }

    #region Request

    public class Request : IDisposable
    {
        private CompositionContainer requestContainer;

        public Request(ComposablePartCatalog parentCatalog, CompositionContainer parentContainer)
        {
            Console.WriteLine("/* Request */");

            var cfg = new InterceptionConfiguration()
                .AddHandler(new NonSharedPartsFilter());
            var interceptingCatalog = new InterceptingCatalog(parentCatalog, cfg);
            this.requestContainer = new CompositionContainer(interceptingCatalog, parentContainer);
        }

        public CompositionContainer RequestContainer
        {
            get { return requestContainer; }
        }

        public void Dispose()
        {
            this.requestContainer.Dispose();

            Console.WriteLine("/* Request End */");
        }
    }

    #endregion

    #region AnotherRequest

    public class AnotherRequest : IDisposable
    {
        private CompositionContainer requestContainer;

        public AnotherRequest(ComposablePartCatalog parentCatalog, CompositionContainer parentContainer)
        {
            Console.WriteLine("/* AnotherRequest */");

            var cfg = new InterceptionConfiguration()
                .AddHandler(new NonSharedPartsFilter());
            var interceptingCatalog = new InterceptingCatalog(parentCatalog, cfg);
            var typeCatalog = new TypeCatalog(typeof(RequestSpecificPart));
            var aggregateCatalog = new AggregateCatalog(interceptingCatalog, typeCatalog);
            this.requestContainer = new CompositionContainer(aggregateCatalog, parentContainer);
        }

        public CompositionContainer RequestContainer
        {
            get { return requestContainer; }
        }

        public void Dispose()
        {
            this.RequestContainer.Dispose();

            Console.WriteLine("/* AnotherRequest End */");
        }
    }

    #endregion

    #region WebApplication

    public class WebApplication : IDisposable
    {
        private ComposablePartCatalog catalog;
        private CompositionContainer container;

        public WebApplication()
        {
            this.catalog = new TypeCatalog(typeof(SharedPart), typeof(NonSharedPart));
            this.container = new CompositionContainer(this.catalog);
            Console.WriteLine("/* Web App started */");
        }

        public CompositionContainer Container
        {
            get { return container; }
        }

        public ComposablePartCatalog Catalog
        {
            get { return catalog; }
        }

        public void Dispose()
        {
            this.container.Dispose();
            Console.WriteLine("/* Web App stopped */");
        }
    }

    #endregion
}