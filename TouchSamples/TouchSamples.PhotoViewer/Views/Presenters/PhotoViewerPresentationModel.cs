using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using TouchSamples.PhotoViewer.Common;
using TouchSamples.PhotoViewer.ViewModels;

namespace TouchSamples.PhotoViewer.Views.Presenters
{
    [Export]
    public class PhotoViewerPresentationModel
    {
        public ObservableCollection<Picture> Pictures { get; private set; }

        public RelayCommand ResetCommand { get; private set; }

        private readonly IPhotoViewerView m_View;

        [ImportingConstructor]
        public PhotoViewerPresentationModel(IPhotoViewerView view)
        {
            Pictures = new ObservableCollection<Picture>();
            ResetCommand = new RelayCommand(o => Reset());

            m_View = view;
            m_View.PresentationModel = this;
        }

        public void Run()
        {
            m_View.Show();

            CollectImages(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Images"));
        }

        private void Reset()
        {
            m_View.ResetScatterView();
        }

        private void CollectImages(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            if (directoryInfo.Exists)
            {
                var files = directoryInfo.GetFiles("*.jpg");

                foreach (var fileInfo in files)
                {
                    Pictures.Add(new Picture(fileInfo.FullName));
                }
            }
        }
    }
}