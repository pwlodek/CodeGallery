using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using AdditionalMefServices;

namespace UnityMefIntegrationDemo
{
    public interface ICoreService
    {
        void Foo();
    }

    public class CoreService1 : ICoreService
    {
        public CoreService1()
        {
            Console.WriteLine("CoreService1.ctor()");
        }

        public void Foo()
        {
            Console.WriteLine("CoreService1.Foo()");
        }
    }

    public class CoreService2 : ICoreService
    {
        public CoreService2()
        {
            Console.WriteLine("CoreService2.ctor()");
        }

        [Import("MefService3")]
        private IMefService m_MefService3;

        public void Foo()
        {
            Debug.Assert(m_MefService3 != null);
            Console.WriteLine("CoreService2.Foo()");
        }
    }
}