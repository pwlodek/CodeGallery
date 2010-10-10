using System;
using System.Windows;
using System.AddIn.Hosting;
using System.Collections.ObjectModel;
using Plugins.Sdk.Views;

namespace WpfHost
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        public void LoadPlugins()
        {
            // Set the add-ins discovery root directory to be the current directory
            string addinRoot = Environment.CurrentDirectory;

            // Rebuild the add-ins cache and pipeline components cache.
            AddInStore.Rebuild(addinRoot);

            // Get registerd add-ins of type SimpleAddInHostView
            Collection<AddInToken> addins = AddInStore.FindAddIns(typeof(WpfAddinHostView), addinRoot);

            foreach (AddInToken addinToken in addins)
            {

                // Activate the add-in
                WpfAddinHostView addinInstance = addinToken.Activate<WpfAddinHostView>(AddInSecurityLevel.FullTrust);

                FrameworkElement element = addinInstance.RegisterContent();
                m_DockPanel.Children.Add(element);

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(AppDomain.CurrentDomain.FriendlyName);
            LoadPlugins();
        }
    }
}
