using System;
using System.Windows;
using System.Windows.Controls;

namespace Plugins.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This comes from the WPF plugin :-)");
        }
    }
}
