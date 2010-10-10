/// --------------------------------------------------------------------------------------
/// <copyright file="Mef2UnityTests.cs">
///     Copyright (C) 2008-2009 Piotr W³odek.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com, http://pwlodek.blogspot.com
/// </authors>
/// <summary>
///     Contains MEF 2 Unity tests.
/// </summary>
/// --------------------------------------------------------------------------------------

using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace Unity.Integration.Mef.Tests
{
    [TestFixture]
    public class Mef2UnityTests
    {
        [Test]
        public void UnityCanAccessCompositionContainer()
        {
            var unityContainer = new UnityContainer();

            // Register MEF catalog in Unity
            unityContainer.RegisterCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            var compositionContainer = unityContainer.Resolve<CompositionContainer>();
            Assert.IsNotNull(compositionContainer);

            var mefComponent1 = compositionContainer.GetExportedObject<IMefComponent1>();
            Assert.IsNotNull(mefComponent1);
        }

        [Test]
        public void UnityComponentsHaveMefDependenciesInjected()
        {
            var unityContainer = new UnityContainer();

            // Register MEF catalog in Unity
            unityContainer.RegisterCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            var unityComponent0 = unityContainer.Resolve<UnityComponent0>();
            Assert.IsNotNull(unityComponent0);
            Assert.IsNotNull(unityComponent0.MefComponent1);

            var mefComponent1 = unityContainer.Resolve<IMefComponent1>();
            Assert.IsNotNull(mefComponent1);
            Assert.IsTrue(mefComponent1 is MefComponent1);

            Assert.AreSame(mefComponent1, unityComponent0.MefComponent1);
        }

        [Test]
        public void UnityComponentsDontHaveMefDependenciesInjectedIfMarkedWithPartNotComposableAttribute()
        {
            var unityContainer = new UnityContainer();

            // Register MEF catalog in Unity
            unityContainer.RegisterCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            var unityComponent00 = unityContainer.Resolve<UnityComponent00>();
            Assert.IsNotNull(unityComponent00);

            Assert.IsNull(unityComponent00.MefComponent1);
        }

        [Test]
        public void UnityCanAccessPureMefComponents()
        {
            var unityContainer = new UnityContainer();

            // Register MEF catalog in Unity
            unityContainer.RegisterCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            var mefComponent1 = unityContainer.Resolve<IMefComponent1>();
            Assert.IsNotNull(mefComponent1);
            Assert.IsTrue(mefComponent1 is MefComponent1);

            var unityComponent1 = unityContainer.Resolve<UnityComponent1>();
            Assert.IsNotNull(unityComponent1);

            Assert.AreSame(mefComponent1, unityComponent1.MefComponent1);
        }

        [Test]
        public void UnityCanAccessPureMefComponentsByName()
        {
            var unityContainer = new UnityContainer();

            // Register MEF catalog in Unity
            unityContainer.RegisterCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            var mefComponent1 = unityContainer.Resolve<IMefComponent1>();
            Assert.IsNotNull(mefComponent1);
            Assert.IsTrue(mefComponent1 is MefComponent1);

            var mefComponent2 = unityContainer.Resolve<IMefComponent1>("MefComponent2");
            Assert.IsNotNull(mefComponent2);
            Assert.IsTrue(mefComponent2 is MefComponent2);
        }

        [Test]
        public void UnityCanAccessMixedMefAndUnityComponents()
        {
            var unityContainer = new UnityContainer();

            // Register MEF catalog in Unity
            unityContainer.RegisterCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            unityContainer.RegisterType<IUnityService1, UnityService1>(
                new ContainerControlledLifetimeManager(), true);

            // Most complex test: UnityComponent2 has dependencies on both Unity
            // and MEF components while specified MEF component has further
            // dependencies on both Unity and MEF components
            var unityComponent2 = unityContainer.Resolve<UnityComponent2>();
            Assert.IsNotNull(unityComponent2);

            var mefComponent3 = unityContainer.Resolve<IMefComponent1>("MefComponent3");
            Assert.IsNotNull(mefComponent3);
            Assert.IsTrue(mefComponent3 is MefComponent3);

            var unityService1 = unityContainer.Resolve<IUnityService1>();
            Assert.IsNotNull(unityService1);

            Assert.AreSame(mefComponent3, unityComponent2.MefComponent1);
            Assert.AreSame(unityService1, unityComponent2.UnityService1);

            Assert.IsTrue(unityComponent2.MefComponent1 is MefComponent3);

            var originalMefComponent3 = (MefComponent3)unityComponent2.MefComponent1;
            Assert.IsNotNull(originalMefComponent3.MefComponent1);
            Assert.IsNotNull(originalMefComponent3.UnityService1);
        }

        [Test]
        public void UnityDoesNotSupportMefComponentsCollections()
        {
            var unityContainer = new UnityContainer();

            // Register MEF catalog in Unity
            unityContainer.RegisterCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            var mefComponents = unityContainer.ResolveAll<IMefComponent1>();
            Assert.IsNotNull(mefComponents);
            Assert.IsTrue(mefComponents.Count() == 0);
        }

        [Test]
        public void UnityCannotAccessMultipleMefComponents()
        {
            var unityContainer = new UnityContainer();

            // Register MEF catalog in Unity
            unityContainer.RegisterCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            Assert.That(delegate
            {
                unityContainer.Resolve<IMefComponent2>();
            }, Throws.TypeOf<ResolutionFailedException>());
        }
    }
}