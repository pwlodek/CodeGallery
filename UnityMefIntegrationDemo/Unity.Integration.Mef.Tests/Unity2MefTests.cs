/// --------------------------------------------------------------------------------------
/// <copyright file="Unity2MefTests.cs">
///     Copyright (C) 2008-2009 Piotr W³odek.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com, http://pwlodek.blogspot.com
/// </authors>
/// <summary>
///     Contains Unity 2 Mef tests.
/// </summary>
/// --------------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace Unity.Integration.Mef.Tests
{
    [TestFixture]
    public class Unity2MefTests
    {
        [Test]
        public void MefCanAccessUnityContainer()
        {
            var unityContainer = new UnityContainer();

            // Register MEF catalog in Unity
            unityContainer.RegisterCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            var compositionContainer = unityContainer.Resolve<CompositionContainer>();
            Assert.IsNotNull(compositionContainer);

            var unityContainerFromMef1 = compositionContainer.GetExportedObject<IUnityContainer>();
            Assert.IsNotNull(unityContainerFromMef1);

            var unityContainerFromMef2 = compositionContainer.GetExportedObject<IUnityContainer>();
            Assert.IsNotNull(unityContainerFromMef2);

            Assert.AreSame(unityContainerFromMef1, unityContainerFromMef2);
            Assert.AreSame(unityContainer, unityContainerFromMef2);
        }

        [Test]
        public void MefCanNotAccessPureUnityComponentsIfNotProperlyRegistered()
        {
            var unityContainer = new UnityContainer();

            // Register MEF catalog in Unity
            unityContainer.RegisterCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            // This service will not be visible to MEF
            unityContainer.RegisterType<IUnityService1, UnityService1>(
                new ContainerControlledLifetimeManager());

            var compositionContainer = unityContainer.Resolve<CompositionContainer>();
            Assert.IsNotNull(compositionContainer);

            // Exception here
            Assert.That(delegate
            {
                compositionContainer.GetExportedObject<IUnityService1>();
            }, Throws.TypeOf<ImportCardinalityMismatchException>());
        }

        [Test]
        public void MefCanAccessPureUnityComponents()
        {
            var unityContainer = new UnityContainer();

            // Register MEF catalog in Unity
            unityContainer.RegisterCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            // This service will be visible to MEF
            unityContainer.RegisterType<IUnityService1, UnityService1>(
                new ContainerControlledLifetimeManager(), true);

            var compositionContainer = unityContainer.Resolve<CompositionContainer>();
            Assert.IsNotNull(compositionContainer);

            var unityService1 = compositionContainer.GetExportedObject<IUnityService1>();
            var unityService2 = compositionContainer.GetExportedObject<IUnityService1>();

            Assert.IsNotNull(unityService1);
            Assert.IsNotNull(unityService2);
            Assert.AreSame(unityService1, unityService2);
        }

        [Test]
        public void MefCanAccessMixedUnityAndMefComponents()
        {
            var unityContainer = new UnityContainer();

            // Register MEF catalog in Unity
            unityContainer.RegisterCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            // This service will be visible to MEF
            unityContainer.RegisterType<IUnityService1, UnityService1>(
                new ContainerControlledLifetimeManager(), true);

            var compositionContainer = unityContainer.Resolve<CompositionContainer>();
            Assert.IsNotNull(compositionContainer);

            var mefComponent3 = compositionContainer.GetExportedObject<IMefComponent1>("MefComponent3");
            Assert.IsTrue(mefComponent3 is MefComponent3);
            Assert.IsNotNull(mefComponent3);

            var originalMefComponent3 = (MefComponent3) mefComponent3;
            Assert.IsNotNull(originalMefComponent3.MefComponent1);
            Assert.IsNotNull(originalMefComponent3.UnityService1);
        }

        [Test]
        public void MefComponentsDontHaveUnityDependenciesInjectedUsingInjectionMethod()
        {
            var unityContainer = new UnityContainer();

            // Register MEF catalog in Unity
            unityContainer.RegisterCatalog(new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            unityContainer.RegisterType<IUnityService1, UnityService1>(true);
            unityContainer.RegisterType<IUnityService1, UnityService2>("UnityService2", true);

            var compositionContainer = unityContainer.Resolve<CompositionContainer>();
            Assert.IsNotNull(compositionContainer);

            var mefComponent3 = compositionContainer.GetExportedObject<IMefComponent1>("MefComponent3");
            Assert.IsTrue(mefComponent3 is MefComponent3);
            Assert.IsNotNull(mefComponent3);

            var originalMefComponent3 = (MefComponent3)mefComponent3;
            Assert.IsNotNull(originalMefComponent3.MefComponent1);
            Assert.IsNotNull(originalMefComponent3.UnityService1);

            // Limited functionality
            Assert.IsNull(originalMefComponent3.UnityService1_1);
        }
    }
}