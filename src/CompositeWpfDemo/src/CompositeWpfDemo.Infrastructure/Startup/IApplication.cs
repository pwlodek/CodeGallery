using System.Windows;
using System.Windows.Threading;

namespace CompositeWpfDemo.Infrastructure.Startup
{
    public interface IApplication
    {
        event ExitEventHandler Exit;

        event DispatcherUnhandledExceptionEventHandler DispatcherUnhandledException;

        DependencyObject RootVisual { get; set; }

        ShutdownMode ShutdownMode { get; set; }
    }
}