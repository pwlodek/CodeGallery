using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TouchSamples.Controls
{
    [DefaultEvent("Selected")]
    public class ScatterViewItem : ContentControl
    {
        static ScatterViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScatterViewItem),
                new FrameworkPropertyMetadata(typeof(ScatterViewItem)));
        }

        public static readonly DependencyProperty IsSelectedProperty =
            Selector.IsSelectedProperty.AddOwner(typeof (ScatterViewItem),
                new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.Journal |
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnIsSelectedChanged));

        [Bindable(true), Category("Appearance")]
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var container = (ScatterViewItem) d;
            var newValue = (bool) e.NewValue;

            container.RaiseEvent(newValue
                                     ? new RoutedEventArgs(Selector.SelectedEvent, container)
                                     : new RoutedEventArgs(Selector.UnselectedEvent, container));
        }

        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            IsSelected = true;
            base.OnMouseDown(e);
        }

        protected override void OnTouchDown(System.Windows.Input.TouchEventArgs e)
        {
            IsSelected = true;
            base.OnTouchDown(e);
        }
    }
}