using System;
using Caliburn.PresentationFramework.ApplicationModel;
using CompositeWpfDemo.Infrastructure.Common;
using CompositeWpfDemo.Module1.Views.Presenters;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.ServiceLocation;

namespace CompositeWpfDemo.Module1
{
    public class Module1Loader : IModule
    {
        private readonly IServiceLocator m_ServiceLocator;
        private readonly IRegionManager m_RegionManager;
        private readonly IBinder m_Binder;
        private readonly IViewStrategy m_ViewStrategy;

        public Module1Loader(
            IServiceLocator serviceLocator,
            IRegionManager regionManager,
            IBinder binder,
            IViewStrategy viewStrategy)
        {
            m_ServiceLocator = serviceLocator;
            m_RegionManager = regionManager;
            m_Binder = binder;
            m_ViewStrategy = viewStrategy;
        }

        public void Initialize()
        {
            RegisterViews();

            var model = m_ServiceLocator.GetInstance<DemoViewPresentationModel>();
            var view = m_ViewStrategy.GetView(model, null, null);

            m_Binder.Bind(model, view, null);
            m_RegionManager.RegisterViewWithRegion(RegionNames.MainRegion, () => view);
        }

        private void RegisterViews()
        {
            
        }
    }
}