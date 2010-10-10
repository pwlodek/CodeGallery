using System;
using System.Windows;
using CompositeWpfDemo.Infrastructure.Startup;

namespace CompositeWpfDemo.Shell.Startup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : IApplication
    {
        public App()
        {
            var bootstrapper = new Bootstrapper(this);
            bootstrapper.Run();
        }

        public DependencyObject RootVisual
        {
            get { return MainWindow; }
            set { MainWindow = value as Window; }
        }
    }
}