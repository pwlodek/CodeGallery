using TouchSamples.PhotoViewer.Views.Presenters;

namespace TouchSamples.PhotoViewer.Views
{
    public interface IPhotoViewerView
    {
        void ResetScatterView();

        void Show();

        PhotoViewerPresentationModel PresentationModel { get; set; }
    }
}