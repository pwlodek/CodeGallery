using System;
using System.Collections.Generic;

namespace CompositeWpfDemo.Infrastructure.Services
{
    public abstract class ContainerFacadeBase : IContainerFacade
    {
        private IContainerFacade m_Parent;

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T), null);
        }

        public T Resolve<T>(string name)
        {
            return (T)Resolve(typeof(T), name);
        }

        public object Resolve(Type type)
        {
            return Resolve(type, null);
        }

        public object Resolve(Type type, string name)
        {
            return DoResolve(type, name);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            var list = new List<T>();

            foreach (T item in ResolveAll(typeof(T)))
            {
                list.Add(item);
            }

            return list;
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            return DoResolveAll(type);
        }

        public IContainerFacade Register<TFrom, TTo>() where TTo : TFrom
        {
            return Register(typeof(TFrom), typeof(TTo), null, LifetimePolicy.Transient);
        }

        public IContainerFacade Register<TFrom, TTo>(string name) where TTo : TFrom
        {
            return Register(typeof(TFrom), typeof(TTo), name, LifetimePolicy.Transient);
        }

        public IContainerFacade Register<TFrom, TTo>(string name, LifetimePolicy policy) where TTo : TFrom
        {
            return Register(typeof(TFrom), typeof(TTo), name, policy);
        }

        public IContainerFacade Register<TFrom, TTo>(LifetimePolicy policy) where TTo : TFrom
        {
            return Register(typeof(TFrom), typeof(TTo), null, policy);
        }

        public IContainerFacade Register(Type from, Type to)
        {
            return Register(from, to, null, LifetimePolicy.Transient);
        }

        public IContainerFacade Register(Type from, Type to, string name)
        {
            return Register(from, to, name, LifetimePolicy.Transient);
        }

        public IContainerFacade Register(Type from, Type to, string name, LifetimePolicy policy)
        {
            return DoRegister(from, to, name, policy);
        }

        public IContainerFacade Register(Type from, Type to, LifetimePolicy policy)
        {
            return Register(from, to, null, policy);
        }

        public IContainerFacade RegisterInstance<TFrom>(TFrom instance)
        {
            return RegisterInstance(typeof(TFrom), null, instance);
        }

        public IContainerFacade RegisterInstance<TFrom>(string name, TFrom instance)
        {
            return RegisterInstance(typeof(TFrom), name, instance);
        }

        public IContainerFacade RegisterInstance(Type type, object instance)
        {
            return RegisterInstance(type, null, instance);
        }

        public IContainerFacade RegisterInstance(Type type, string name, object instance)
        {
            return DoRegisterInstance(type, name, instance);
        }

        public IContainerFacade CreateChildContainer()
        {
            m_Parent = DoCreateChildContainer();
            return m_Parent;
        }

        public IContainerFacade Parent
        {
            get { return m_Parent; }
        }

        public IContainerFacade BuildUp(object instance)
        {
            return DoBuildUp(instance);
        }

        public bool IsTypeRegistered<TType>()
        {
            return IsTypeRegistered(typeof(TType));
        }

        public bool IsTypeRegistered(Type type)
        {
            throw new NotImplementedException();
        }

        protected abstract bool DoIsTypeRegistered(Type type);
        protected abstract IContainerFacade DoBuildUp(object instance);
        protected abstract object DoResolve(Type type, string name);
        protected abstract IEnumerable<object> DoResolveAll(Type type);
        protected abstract IContainerFacade DoRegister(Type from, Type to, string name, LifetimePolicy policy);
        protected abstract IContainerFacade DoCreateChildContainer();
        protected abstract IContainerFacade DoRegisterInstance(Type type, string name, object instance);
    }
}