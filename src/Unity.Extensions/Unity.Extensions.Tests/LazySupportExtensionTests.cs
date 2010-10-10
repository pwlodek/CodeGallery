using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace Unity.Extensions.Tests
{
    [TestFixture]
    public class LazySupportExtensionTests
    {
        #region Fake components

        public interface IComponent { }

        public class Component1 : IComponent
        {
            public static int InstanceCount;

            public Component1()
            {
                InstanceCount++;
            }
        }

        public class Component2 : IComponent
        {
            public static int InstanceCount;

            public Component2()
            {
                InstanceCount++;
            }
        }

        public class Component3 : IComponent { }

        #endregion

        [Test]
        public void UnityCanResolveLazyTypeRegisteredInUnityTest()
        {
            // Setup
            var unityContainer = new UnityContainer();

            // Add composition support for unity
            unityContainer.AddNewExtension<LazySupportExtension>();

            Component1.InstanceCount = 0;
            unityContainer.RegisterType<IComponent, Component1>();

            var lazyUnityComponent = unityContainer.Resolve<Lazy<IComponent>>();
            Assert.That(lazyUnityComponent, Is.Not.Null);
            Assert.That(Component1.InstanceCount, Is.EqualTo(0));

            Assert.That(lazyUnityComponent.Value, Is.Not.Null);
            Assert.That(lazyUnityComponent.Value.GetType(), Is.EqualTo(typeof(Component1)));
            Assert.That(Component1.InstanceCount, Is.EqualTo(1));
        }

        [Test]
        public void UnityCanResolveLazyEnumerableOfTypesRegisteredInUnityTest()
        {
            // Setup
            var unityContainer = new UnityContainer();

            // Add composition support for unity
            unityContainer.AddNewExtension<LazySupportExtension>();

            Component1.InstanceCount = 0;

            unityContainer.RegisterType<IComponent, Component1>("component1");
            unityContainer.RegisterType<IComponent, Component2>("component2");

            var collectionOfLazyUnityComponents = unityContainer.Resolve<Lazy<IEnumerable<IComponent>>>();
            Assert.That(collectionOfLazyUnityComponents, Is.Not.Null);

            Assert.That(Component1.InstanceCount, Is.EqualTo(0));
            var list = new List<IComponent>(collectionOfLazyUnityComponents.Value);
            Assert.That(Component1.InstanceCount, Is.EqualTo(1));
            Assert.That(list.Count, Is.EqualTo(2));
        }

        [Test]
        public void UnityCanResolveEnumerableOfLazyTypesRegisteredInUnityTest()
        {
            // Setup
            var unityContainer = new UnityContainer();

            // Add composition support for unity
            unityContainer.AddNewExtension<LazySupportExtension>();

            Component1.InstanceCount = 0;
            Component2.InstanceCount = 0;
            
            unityContainer.RegisterType<IComponent, Component1>("component1");
            unityContainer.RegisterType<IComponent, Component2>("component2");
            unityContainer.RegisterType<IComponent, Component3>();

            var collectionOfLazyUnityComponents = unityContainer.Resolve<IEnumerable<Lazy<IComponent>>>();
            Assert.That(collectionOfLazyUnityComponents, Is.Not.Null);

            Assert.That(Component1.InstanceCount, Is.EqualTo(0));
            Assert.That(Component2.InstanceCount, Is.EqualTo(0));

            var list = new List<Lazy<IComponent>>(collectionOfLazyUnityComponents);

            Assert.That(Component1.InstanceCount, Is.EqualTo(0));

            Assert.That(list[0].Value, Is.Not.Null);
            Assert.That(list[1].Value, Is.Not.Null);
            Assert.That(list[2].Value, Is.Not.Null);

            Assert.That(Component1.InstanceCount, Is.EqualTo(1));
            Assert.That(Component2.InstanceCount, Is.EqualTo(1));

            Assert.That(list.Count, Is.EqualTo(3));

            Assert.That(list.Select(t => t.Value).OfType<Component1>().Count(), Is.EqualTo(1));
            Assert.That(list.Select(t => t.Value).OfType<Component2>().Count(), Is.EqualTo(1));
            Assert.That(list.Select(t => t.Value).OfType<Component3>().Count(), Is.EqualTo(1));
        }

        [Test]
        public void UnityCanResolveEnumerableOfTypesRegisteredInUnityTest()
        {
            // Setup
            var unityContainer = new UnityContainer();

            // Add composition support for unity
            unityContainer.AddNewExtension<LazySupportExtension>();

            Component1.InstanceCount = 0;
            Component2.InstanceCount = 0;

            unityContainer.RegisterType<IComponent, Component1>("component1");
            unityContainer.RegisterType<IComponent, Component2>("component2");
            unityContainer.RegisterType<IComponent, Component3>();

            var collectionOfLazyUnityComponents = unityContainer.Resolve<IEnumerable<IComponent>>();
            Assert.That(collectionOfLazyUnityComponents, Is.Not.Null);

            Assert.That(Component1.InstanceCount, Is.EqualTo(1));
            Assert.That(Component2.InstanceCount, Is.EqualTo(1));

            var list = new List<IComponent>(collectionOfLazyUnityComponents);
            Assert.That(list.Count, Is.EqualTo(3));

            Assert.That(list.OfType<Component1>().Count(), Is.EqualTo(1));
            Assert.That(list.OfType<Component2>().Count(), Is.EqualTo(1));
            Assert.That(list.OfType<Component3>().Count(), Is.EqualTo(1));
        }
    }
}