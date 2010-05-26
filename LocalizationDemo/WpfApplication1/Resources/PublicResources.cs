using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using ResX = WpfApplication1.Properties.Resources;

namespace WpfApplication1.Resources
{
    public class PublicResources : INotifyPropertyChanged
    {
        public static readonly PublicResources Instance = new PublicResources();
        private static readonly ResourceHelper ResourceHelperInstance = new ResourceHelper();

        private PublicResources()
        {
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public object Resource
        {
            get
            {
                return ResourceHelperInstance;
            }
        }

        public void Invalidate()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Resource"));
            }
        }

        public static void SetLanguage(string lang)
        {
            var ci = new CultureInfo(lang);

            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            // Invalidate UI
            Instance.Invalidate();
        }

        internal class ResourceHelper
        {
            public string this[string index]
            {
                get
                {
                    return ResX.ResourceManager.GetString(index);
                }
            }
        }
    }
}