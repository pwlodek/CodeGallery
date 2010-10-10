using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using TouchSamples.PhotoViewer.Views.Presenters;

namespace TouchSamples.PhotoViewer.Startup
{
    public class Bootstrapper
    {
        [Import]
        public PhotoViewerPresentationModel PresentationModel { get; set; }

        public Bootstrapper()
        {
            var catalog = new AssemblyCatalog(typeof(Bootstrapper).Assembly);
            var compositionContainer = new CompositionContainer(catalog);
            
            compositionContainer.ComposeParts(this);
        }

        public void Run()
        {
            PresentationModel.Run();
        }
    }
}