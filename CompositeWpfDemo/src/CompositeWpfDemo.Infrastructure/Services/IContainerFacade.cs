using System;
using System.Collections.Generic;

namespace CompositeWpfDemo.Infrastructure.Services
{
    public interface IContainerFacade
    {
        T Resolve<T>();
        T Resolve<T>(string name);

        object Resolve(Type type);
        object Resolve(Type type, string name);

        IEnumerable<T> ResolveAll<T>();
        IEnumerable<object> ResolveAll(Type type);

        IContainerFacade Register<TFrom, TTo>() where TTo : TFrom;
        IContainerFacade Register<TFrom, TTo>(string name) where TTo : TFrom;
        IContainerFacade Register<TFrom, TTo>(string name, LifetimePolicy policy) where TTo : TFrom;
        IContainerFacade Register<TFrom, TTo>(LifetimePolicy policy) where TTo : TFrom;

        IContainerFacade Register(Type from, Type to);
        IContainerFacade Register(Type from, Type to, string name);
        IContainerFacade Register(Type from, Type to, string name, LifetimePolicy policy);
        IContainerFacade Register(Type from, Type to, LifetimePolicy policy);

        IContainerFacade RegisterInstance<TFrom>(TFrom instance);
        IContainerFacade RegisterInstance<TFrom>(string name, TFrom instance);

        IContainerFacade RegisterInstance(Type type, object instance);
        IContainerFacade RegisterInstance(Type type, string name, object instance);

        IContainerFacade CreateChildContainer();
        IContainerFacade Parent { get; }

        IContainerFacade BuildUp(object instance);

        bool IsTypeRegistered<TType>();
        bool IsTypeRegistered(Type type);
    }

    public enum LifetimePolicy
    {
        Singleton,
        Transient,
        PerThread
    }
}