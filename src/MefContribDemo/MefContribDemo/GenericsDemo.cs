using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using MefContrib.Hosting.Interception;
using MefContrib.Hosting.Interception.Generics;

namespace MefContribDemo
{
    public class GenericsDemo
    {
        [Import(AllowDefault = true)]
        public IRepository<Foo> FooRepo { get; set; }

        [Import(AllowDefault = true)]
        public IRepository<Bar> BarRepo { get; set; }

        public void Run()
        {
            var catalog1 = new AssemblyCatalog(typeof(GenericsDemo).Assembly);
            var interceptingCatalog = new InterceptingCatalog(catalog1, new GenericExportHandler());
            var container = new CompositionContainer(interceptingCatalog);

            container.SatisfyImportsOnce(this);

            Console.WriteLine(FooRepo.Get(0).GetType().Name);
            Console.WriteLine(BarRepo.Get(0).GetType().Name);
        }
    }

    [InheritedExport]
    public interface IRepository<T>
    {
        T Get(int id);
    }

    public class Repository<T> : IRepository<T>
        where T : new()
    {
        public T Get(int id)
        {
            return new T();
        }
    }

    public class GenericRepositoryExport : GenericContractTypeMapping
    {
        public GenericRepositoryExport() : base(typeof(IRepository<>), typeof(Repository<>)) { }
    }

    public class Foo { }
    public class Bar { }
}