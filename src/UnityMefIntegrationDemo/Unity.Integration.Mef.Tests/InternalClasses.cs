/// --------------------------------------------------------------------------------------
/// <copyright file="InternalClasses.cs">
///     Copyright (C) 2008-2009 Piotr W³odek.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com, http://pwlodek.blogspot.com
/// </authors>
/// <summary>
///     Contains internal classes usid in all tests.
/// </summary>
/// --------------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Diagnostics;
using Microsoft.Practices.Unity;

namespace Unity.Integration.Mef.Tests
{
    internal interface IMefComponent2
    {
        void Foo();
    }

    [Export(typeof(IMefComponent2))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class MefComponent21 : IMefComponent2
    {
        public void Foo()
        {
        }
    }

    [Export(typeof(IMefComponent2))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class MefComponent22 : IMefComponent2
    {
        public void Foo()
        {
        }
    }

    internal interface IMefComponent1
    {
        void Foo();
    }

    [Export(typeof(IMefComponent1))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class MefComponent1 : IMefComponent1
    {
        public void Foo()
        {
        }
    }

    [Export("MefComponent2", typeof(IMefComponent1))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class MefComponent2 : IMefComponent1
    {
        public void Foo()
        {
        }
    }

    [Export("MefComponent3", typeof(IMefComponent1))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class MefComponent3 : IMefComponent1
    {
        private readonly IUnityService1 m_UnityService1;
        private readonly IMefComponent1 m_MefComponent1;

        [ImportingConstructor]
        public MefComponent3(IUnityService1 unityService1,
            [Import("MefComponent2")] IMefComponent1 mefComponent1)
        {
            Debug.Assert(unityService1 != null);
            Debug.Assert(mefComponent1 != null);
            Debug.Assert(mefComponent1 is MefComponent2);

            m_UnityService1 = unityService1;
            m_MefComponent1 = mefComponent1;
        }

        public void Foo()
        {
        }

        public IMefComponent1 MefComponent1
        {
            get { return m_MefComponent1; }
        }

        public IUnityService1 UnityService1
        {
            get { return m_UnityService1; }
        }

        public IUnityService1 UnityService1_1
        {
            get { return m_UnityService1_1; }
        }

        private IUnityService1 m_UnityService1_1;

        [InjectionMethod]
        public void UnityInvokedMethod([Dependency("UnityService2")] IUnityService1 unityService1)
        {
            m_UnityService1_1 = unityService1;
        }
    }

    internal interface IUnityService1
    {
        void Bar();
    }

    internal class UnityService1 : IUnityService1
    {
        public void Bar()
        {
        }
    }

    internal class UnityService2 : IUnityService1
    {
        public void Bar()
        {
        }
    }

    internal class UnityComponent0
    {
        [Import]
        public IMefComponent1 MefComponent1 { get; set; }
    }

    [PartNotComposable]
    internal class UnityComponent00
    {
        [Import]
        public IMefComponent1 MefComponent1 { get; set; }
    }

    internal class UnityComponent1
    {
        private readonly IMefComponent1 m_MefComponent1;

        public UnityComponent1(IMefComponent1 mefComponent1)
        {
            m_MefComponent1 = mefComponent1;
            Debug.Assert(mefComponent1 != null);
        }

        public IMefComponent1 MefComponent1
        {
            get { return m_MefComponent1; }
        }
    }

    internal class UnityComponent2
    {
        private readonly IMefComponent1 m_MefComponent1;
        private readonly IUnityService1 m_UnityService1;

        public UnityComponent2([Dependency("MefComponent3")] IMefComponent1 mefComponent1,
            IUnityService1 unityService1)
        {
            Debug.Assert(mefComponent1 != null);
            Debug.Assert(unityService1 != null);

            m_MefComponent1 = mefComponent1;
            m_UnityService1 = unityService1;
        }

        public IUnityService1 UnityService1
        {
            get { return m_UnityService1; }
        }

        public IMefComponent1 MefComponent1
        {
            get { return m_MefComponent1; }
        }
    }
}