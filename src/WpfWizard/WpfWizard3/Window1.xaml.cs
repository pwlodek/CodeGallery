using System;
using System.Windows;

namespace WpfWizard
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            // Attach simple presentation model
            DataContext = new Window1PresentationModel();
        }

        private void Wizard_OnFinishClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Wizard_OnCancelClick(object sender, RoutedEventArgs e)
        {
        }
    }
}
