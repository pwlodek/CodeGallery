using System;
using System.ComponentModel.Composition;
using MefContrib.Hosting.Interception.Handlers;

namespace MefContribDemo.Generics
{
    public class RepositoryOfTExport : GenericContractTypeMapping
    {
        public RepositoryOfTExport() : base(typeof(IRepository<>), typeof(Repository<>)) { }
    }

    [InheritedExport]
    public interface IRepository<T>
    {
        T Get(int id);

        void Save(T instance);
    }

    public class Repository<T> : IRepository<T> where T : new()
    {
        public T Get(int id)
        {
            return new T();
        }

        public void Save(T instance)
        {
            Console.WriteLine("Saving {0} instance.", instance.GetType().Name);
        }
    }

    public class Customer { }
}