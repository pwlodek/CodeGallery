using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using Extensible.Dashboard.Views.Presenters;

namespace Extensible.Dashboard.Startup
{
    public class Bootstrapper
    {
        [Import]
        public ShellPresentationModel Main { get; private set; }

        public Bootstrapper()
        {
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var directoryCatalog = new DirectoryCatalog(path);
            var assemblyCatalog = new AssemblyCatalog(typeof(Bootstrapper).Assembly);
            var aggregateCatalog = new AggregateCatalog(directoryCatalog, assemblyCatalog);
            var compositionContainer = new CompositionContainer(aggregateCatalog);

            compositionContainer.ComposeParts(this);

            var fileSystemWatcher = new FileSystemWatcher(path);
            fileSystemWatcher.Filter = "*.dll";
            fileSystemWatcher.Changed += (s, e) => directoryCatalog.Refresh();
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Run()
        {
            Main.Run();
        }
    }
}