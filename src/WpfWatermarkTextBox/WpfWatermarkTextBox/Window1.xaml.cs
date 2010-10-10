using System;

namespace WpfWatermarkTextBox
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1
    {
        public Window1()
        {
            DataContext = new Window1ViewModel();

            InitializeComponent();
        }
    }
}
