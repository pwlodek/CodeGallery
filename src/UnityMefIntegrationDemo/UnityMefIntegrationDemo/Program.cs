using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Reflection;
using AdditionalMefServices;
using Microsoft.Practices.Unity;
using Unity.Integration.Mef;

namespace UnityMefIntegrationDemo
{
    [PartNotComposable]
    public class Program
    {
        private readonly IUnityContainer m_UnityContainer;
        private readonly CompositionContainer m_CompositionContainer;

        public Program(IUnityContainer unityContainer, CompositionContainer compositionContainer)
        {
            m_UnityContainer = unityContainer;
            m_CompositionContainer = compositionContainer;
        }

        [Import("MefService3")]
        private Export<IMefService> m_MefService;

        [Import]
        private IEnumerable<IMefService> m_MefServices;

        /// <summary>
        /// Main entry point to the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        private static void Main(string[] args)
        {
            #region Initialization

            // Create the Unity container and self-register
            var unity = new UnityContainer();
            unity.RegisterInstance<IUnityContainer>(unity);

            // Register MEF catalogs in Unity
            unity.RegisterCatalog(new AssemblyCatalog(Assembly.GetEntryAssembly()));
            unity.RegisterCatalog(new DirectoryCatalog("."));

            #endregion

            // Register unity components
            unity.RegisterType<CoreComponent>();
            unity.RegisterType<ICoreService, CoreService1>(new ContainerControlledLifetimeManager(), true);
            unity.RegisterType<ICoreService, CoreService2>("CoreService2", new ContainerControlledLifetimeManager(), true);

            // Run the "Application"
            var p = unity.Resolve<Program>();
            p.Run();
        }

        private void Run()
        {
            Debug.Assert(m_UnityContainer != null);
            Debug.Assert(m_CompositionContainer != null);

            // Test MEF service
            if (m_MefService != null)
            {
                var data = (string) m_MefService.Metadata["MyKey"];
                Debug.Assert(data == "MyValue");
                m_MefService.GetExportedObject().Bar();
            }

            if (m_MefServices != null)
            {
                foreach (var mefService in m_MefServices)
                {
                    mefService.Bar();
                }
            }

            var coreComponent = m_UnityContainer.Resolve<CoreComponent>();
            coreComponent.FooBar();
        }
    }
}
