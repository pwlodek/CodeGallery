using System;
using System.Windows;
using DependencyPropertyHelper = StronglyTypedDependencyProperties.Helpers.DependencyPropertyHelper;

namespace StaticallyTypedDependencyProperties
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1
    {
        public Window1()
        {
            InitializeComponent();
        }

        public int MyInteger1
        {
            get { return (int)GetValue(MyInteger1Property); }
            set { SetValue(MyInteger1Property, value); }
        }

        public static readonly DependencyProperty MyInteger1Property =
            DependencyProperty.Register("MyInteger1", typeof(int), typeof(Window1));

        public int MyInteger2
        {
            get { return (int)GetValue(MyInteger2Property); }
            set { SetValue(MyInteger2Property, value); }
        }

        public static readonly DependencyProperty MyInteger2Property =
            DependencyPropertyHelper.Register<Window1>(t => t.MyInteger2);
    }
}