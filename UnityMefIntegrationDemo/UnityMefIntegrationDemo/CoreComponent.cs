using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using AdditionalMefServices;
using Microsoft.Practices.Unity;

namespace UnityMefIntegrationDemo
{
    /// <summary>
    /// Simulates some core component that has dependencies injected by MEF
    /// during resolving an instance via Unity.
    /// </summary>
    public class CoreComponent : IPartImportsSatisfiedNotification
    {
        private readonly IMefService m_MefService3;
        private readonly ICoreService m_CoreService;

        // Interesting thing happens here: Unity injects components created
        // by both MEF and Unity
        public CoreComponent(
            [Dependency("MefService3")] IMefService mefService3,
            ICoreService coreService)
        {
            m_MefService3 = mefService3;
            m_CoreService = coreService;
        }

        [Import(AllowDefault = true)]
        private IEnumerable<IMefService> m_MefServices;

        public void FooBar()
        {
            Console.WriteLine("CoreComponent.FooBar()");

            foreach (var mefService in m_MefServices)
            {
                mefService.Bar();
            }

            m_MefService3.Bar();
            m_CoreService.Foo();
        }

        #region IPartImportsSatisfiedNotification Implementation

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            Debug.Assert(m_MefServices.Count() > 0);
        }

        #endregion
    }
}