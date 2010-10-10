using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MefContrib.Hosting;

namespace MefContribDemo
{
    public class FactoryDemo
    {
        [Import]
        public CompositionContainer Container { get; set; }

        public void Run()
        {
            SetupContainer();

            var exp11 = Container.GetExportedValue<Export1>();
            var exp12 = Container.GetExportedValue<Export1>();

            Console.WriteLine("Export1.InstanceCount = " + Export1.InstanceCount);

            var exp21 = Container.GetExportedValue<Export2>();
            var exp22 = Container.GetExportedValue<Export2>();

            Console.WriteLine("Export2.InstanceCount = " + Export2.InstanceCount);

            // will throw ImportCardinalityMismatchException
            //var exp31 = Container.GetExportedValue<Export3>();

            var exp31 = Container.GetExportedValue<Export3>("export3");
            var exp32 = Container.GetExportedValue<Export3>("export3");

            Console.WriteLine("Export3.InstanceCount = " + Export3.InstanceCount);
        }

        private void SetupContainer()
        {
            var factoryExportProvider = new FactoryExportProvider()
                .Register(typeof (Export2), () => new Export2())
                .Register(typeof (Export3), "export3", () => new Export3());

            var catalog = new AssemblyCatalog(typeof(FactoryDemo).Assembly);
            var container = new CompositionContainer(catalog, factoryExportProvider);

            container.ComposeExportedValue(container);
            container.SatisfyImportsOnce(this);
        }
    }

    public class Export1Trampoline
    {
        [Export]
        public Export1 Export1Factory
        {
            get { return new Export1(); }
        }
    }

    public class Export1
    {
        public static int InstanceCount;

        public Export1()
        {
            InstanceCount++;
        }
    }

    public class Export2
    {
        public static int InstanceCount;

        public Export2()
        {
            InstanceCount++;
        }
    }

    public class Export3
    {
        public static int InstanceCount;

        public Export3()
        {
            InstanceCount++;
        }
    }
}