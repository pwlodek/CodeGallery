using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using AdditionalMefServices;
using Microsoft.Practices.Unity;

namespace UnityMefIntegrationDemo
{
    [Export(typeof(IMefService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MefService1 : IMefService
    {
        [ImportingConstructor]
        public MefService1(CompositionContainer compositionContainer)
        {
            Debug.Assert(compositionContainer != null);
            Console.WriteLine("MefService1.ctor()");
        }

        [Import]
        private IUnityContainer Container { get; set; }

        [Import]
        private ICoreService CoreService { get; set; }

        public void Bar()
        {
            Debug.Assert(Container != null);

            Console.WriteLine("MefService1.Bar()");
            CoreService.Foo();
        }
    }

    [Export(typeof(IMefService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MefService2 : IMefService
    {
        private readonly IUnityContainer m_UnityContainer;
        private readonly ICoreService m_CoreService;

        // Interesting thing happens here: MEF injects components created
        // by both MEF and Unity
        [ImportingConstructor]
        public MefService2(IUnityContainer unityContainer,
            [Import("CoreService2")] ICoreService coreService)
        {
            m_UnityContainer = unityContainer;
            m_CoreService = coreService;

            Debug.Assert(m_UnityContainer != null);
            Console.WriteLine("MefService2.ctor()");
        }

        public void Bar()
        {
            Console.WriteLine("MefService2.Bar()");
            m_CoreService.Foo();
        }
    }

    [Export("MefService3", typeof(IMefService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ExportMetadata("MyKey", "MyValue")]
    public class MefService3 : IMefService
    {
        public MefService3()
        {
            Console.WriteLine("MefService3.ctor()");
        }

        public void Bar()
        {
            Console.WriteLine("MefService3.Bar()");
        }
    }
}