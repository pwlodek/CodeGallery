using System;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Windows;
using Caliburn.Core;
using Caliburn.PresentationFramework;
using Caliburn.PresentationFramework.ApplicationModel;
using Caliburn.Prism;
using CompositeWpfDemo.Infrastructure.Services;
using CompositeWpfDemo.Infrastructure.Startup;
using CompositeWpfDemo.Module1;
using CompositeWpfDemo.Shell.Services;
using CompositeWpfDemo.Shell.Startup.Integration;
using CompositeWpfDemo.Shell.Views;
using CompositeWpfDemo.Shell.Views.Presenters;
using CompositeWpfDemo.Shell.Views.Windows;
using MefContrib.Integration.Unity;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;

namespace CompositeWpfDemo.Shell.Startup
{
    public class Bootstrapper : CaliburnBootstrapper
    {
        private IUnityContainer m_Container;

        public Bootstrapper(IApplication application) : base(application)
        {
        }

        protected override IContainerFacade CreateContainer()
        {
            // Create Unity
            m_Container = new UnityContainer();

            // Add MEF catalogs to the Unity
            m_Container.RegisterCatalog(new AssemblyCatalog(Assembly.GetEntryAssembly()));
            m_Container.RegisterCatalog(new DirectoryCatalog("."));

            return new UnityContainerFacade(m_Container);
        }

        protected override IContainer CreateCaliburnContainer()
        {
            return new ExtendedUnityAdapter(m_Container);
        }

        protected override DependencyObject CreateShell()
        {
            var shellPresenter = Container.Resolve<ShellViewPresentationModel>();
            var windowManager = Container.Resolve<IWindowManager>();
            var viewStrategy = Container.Resolve<IViewStrategy>();
            var shellWindow = (DependencyObject)viewStrategy.GetView(shellPresenter, null, null);

            Application.RootVisual = shellWindow;
            windowManager.Show(shellPresenter, null, ExecuteShutdownModel);

            return shellWindow;
        }

        protected override void ConfigureCaliburn(CoreConfiguration configuration)
        {
            configuration
                .WithPresentationFramework()
                .WithCompositeApplicationLibrary(CreateShell)
                .WithModuleCatalog(new ModuleCatalog().AddModule(typeof (Module1Loader)));
        }

        private void ExecuteShutdownModel(ISubordinate subordinate, Action completed)
        {
            completed();
        }

        protected override void RegisterViews()
        {
            Container.Register<IShellView, ShellView>(LifetimePolicy.Singleton);
        }

        protected override void AfterStart()
        {
            var binder = (DefaultBinder)Container.Resolve<IBinder>();
            binder.EnableMessageConventions();
            binder.EnableBindingConventions();
        }
    }
}