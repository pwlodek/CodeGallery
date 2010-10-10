using System;
using System.Collections.Generic;
using CompositeWpfDemo.Infrastructure.Services;
using MefContrib.Integration.Unity;
using Microsoft.Practices.Unity;

namespace CompositeWpfDemo.Shell.Services
{
    public class UnityContainerFacade : ContainerFacadeBase
    {
        private readonly IUnityContainer m_Container;

        public UnityContainerFacade(IUnityContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            m_Container = container;
        }

        protected override bool DoIsTypeRegistered(Type type)
        {
            return m_Container.IsTypeRegistered(type);
        }

        protected override IContainerFacade DoBuildUp(object instance)
        {
            m_Container.BuildUp(instance);
            return this;
        }

        protected override object DoResolve(Type type, string name)
        {
            return m_Container.Resolve(type, name);
        }

        protected override IEnumerable<object> DoResolveAll(Type type)
        {
            return m_Container.ResolveAll(type);
        }

        protected override IContainerFacade DoRegister(Type from, Type to, string name, LifetimePolicy policy)
        {
            m_Container.RegisterType(from, to, name, ParseLifetime(policy));
            return this;
        }

        protected override IContainerFacade DoRegisterInstance(Type type, string name, object instance)
        {
            m_Container.RegisterInstance(type, name, instance);
            return this;
        }

        protected override IContainerFacade DoCreateChildContainer()
        {
            var parent = m_Container.CreateChildContainer();
            return new UnityContainerFacade(parent);
        }

        private static LifetimeManager ParseLifetime(LifetimePolicy policy)
        {
            if (policy == LifetimePolicy.Singleton)
                return new ContainerControlledLifetimeManager();

            if (policy == LifetimePolicy.Transient)
                return new TransientLifetimeManager();

            if (policy == LifetimePolicy.PerThread)
                return new PerThreadLifetimeManager();

            throw new InvalidOperationException();
        }
    }
}