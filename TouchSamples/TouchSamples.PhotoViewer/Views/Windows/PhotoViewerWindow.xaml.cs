using System;
using System.ComponentModel.Composition;
using TouchSamples.PhotoViewer.Views.Presenters;

namespace TouchSamples.PhotoViewer.Views.Windows
{
    /// <summary>
    /// Interaction logic for PhotoViewerWindow.xaml
    /// </summary>
    [Export(typeof(IPhotoViewerView))]
    public partial class PhotoViewerWindow : IPhotoViewerView
    {
        public PhotoViewerWindow()
        {
            InitializeComponent();
        }

        public void ResetScatterView()
        {
            m_ScatterView.Reset();
        }

        public PhotoViewerPresentationModel PresentationModel
        {
            get { return (PhotoViewerPresentationModel) DataContext; }
            set { DataContext = value; }
        }
    }
}
