using System;
using System.Windows;
using WpfApplication1.Resources;

namespace WpfApplication1
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PublicResources.SetLanguage("pl-PL");
        }
    }
}
