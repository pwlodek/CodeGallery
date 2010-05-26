using System.Windows;
using System.Windows.Input;

namespace WpfWatermarkTextBox.Behaviors
{
    public static class KeyboardFocusBehavior
    {
        public static bool GetIsFocused(UIElement obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(UIElement obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached("IsFocused", typeof(bool), typeof(KeyboardFocusBehavior),
                new UIPropertyMetadata(false, OnIsFocusedChanged));

        private static void OnIsFocusedChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var isFocused = (bool) args.NewValue;
            if (isFocused)
            {
                var element = (FrameworkElement) o;
                if (element.IsLoaded)
                {
                    Keyboard.Focus(element);
                }
                else
                {
                    element.Loaded += ((sender, e) => Keyboard.Focus(element));
                }
            }
        }
    }
}