using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WpfWatermarkTextBox.Behaviors
{
    public sealed class WatermarkTextBoxBehavior
    {
        private readonly TextBox m_TextBox;
        private TextBlockAdorner m_TextBlockAdorner;

        private WatermarkTextBoxBehavior(TextBox textBox)
        {
            if (textBox == null)
                throw new ArgumentNullException("textBox");

            m_TextBox = textBox;
        }

        #region Behavior Internals

        private static WatermarkTextBoxBehavior GetWatermarkTextBoxBehavior(DependencyObject obj)
        {
            return (WatermarkTextBoxBehavior)obj.GetValue(WatermarkTextBoxBehaviorProperty);
        }

        private static void SetWatermarkTextBoxBehavior(DependencyObject obj, WatermarkTextBoxBehavior value)
        {
            obj.SetValue(WatermarkTextBoxBehaviorProperty, value);
        }

        private static readonly DependencyProperty WatermarkTextBoxBehaviorProperty =
            DependencyProperty.RegisterAttached("WatermarkTextBoxBehavior",
                typeof(WatermarkTextBoxBehavior), typeof(WatermarkTextBoxBehavior), new UIPropertyMetadata(null));

        public static bool GetEnableWatermark(TextBox obj)
        {
            return (bool)obj.GetValue(EnableWatermarkProperty);
        }

        public static void SetEnableWatermark(TextBox obj, bool value)
        {
            obj.SetValue(EnableWatermarkProperty, value);
        }

        public static readonly DependencyProperty EnableWatermarkProperty =
            DependencyProperty.RegisterAttached("EnableWatermark", typeof(bool),
                typeof(WatermarkTextBoxBehavior), new UIPropertyMetadata(false, OnEnableWatermarkChanged));

        private static void OnEnableWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                var enabled = (bool)e.OldValue;

                if (enabled)
                {
                    var textBox = (TextBox)d;
                    var behavior = GetWatermarkTextBoxBehavior(textBox);
                    behavior.Detach();

                    SetWatermarkTextBoxBehavior(textBox, null);
                }
            }

            if (e.NewValue != null)
            {
                var enabled = (bool)e.NewValue;

                if (enabled)
                {
                    var textBox = (TextBox)d;
                    var behavior = new WatermarkTextBoxBehavior(textBox);
                    behavior.Attach();

                    SetWatermarkTextBoxBehavior(textBox, behavior);
                }
            }
        }

        private void Attach()
        {
            m_TextBox.Loaded += TextBoxLoaded;
            m_TextBox.TextChanged += TextBoxTextChanged;
            m_TextBox.DragEnter += TextBoxDragEnter;
            m_TextBox.DragLeave += TextBoxDragLeave;
        }

        private void Detach()
        {
            m_TextBox.Loaded -= TextBoxLoaded;
            m_TextBox.TextChanged -= TextBoxTextChanged;
            m_TextBox.DragEnter -= TextBoxDragEnter;
            m_TextBox.DragLeave -= TextBoxDragLeave;
        }

        private void TextBoxDragLeave(object sender, DragEventArgs e)
        {
            UpdateAdorner();
        }

        private void TextBoxDragEnter(object sender, DragEventArgs e)
        {
            m_TextBox.TryRemoveAdorners<TextBlockAdorner>();
        }

        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var hasText = !string.IsNullOrEmpty(m_TextBox.Text);
            SetHasText(m_TextBox, hasText);
        }

        private void TextBoxLoaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        #endregion

        #region Attached Properties

        public static string GetLabel(TextBox obj)
        {
            return (string)obj.GetValue(LabelProperty);
        }

        public static void SetLabel(TextBox obj, string value)
        {
            obj.SetValue(LabelProperty, value);
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.RegisterAttached("Label", typeof(string), typeof(WatermarkTextBoxBehavior));

        public static Style GetLabelStyle(TextBox obj)
        {
            return (Style)obj.GetValue(LabelStyleProperty);
        }

        public static void SetLabelStyle(TextBox obj, Style value)
        {
            obj.SetValue(LabelStyleProperty, value);
        }

        public static readonly DependencyProperty LabelStyleProperty =
            DependencyProperty.RegisterAttached("LabelStyle", typeof(Style),
                typeof(WatermarkTextBoxBehavior));

        public static bool GetHasText(TextBox obj)
        {
            return (bool)obj.GetValue(HasTextProperty);
        }

        private static void SetHasText(TextBox obj, bool value)
        {
            obj.SetValue(HasTextPropertyKey, value);
        }

        private static readonly DependencyPropertyKey HasTextPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("HasText", typeof(bool),
                typeof(WatermarkTextBoxBehavior), new UIPropertyMetadata(false));

        public static readonly DependencyProperty HasTextProperty =
            HasTextPropertyKey.DependencyProperty;

        #endregion

        private void Init()
        {
            m_TextBlockAdorner = new TextBlockAdorner(m_TextBox, GetLabel(m_TextBox), GetLabelStyle(m_TextBox));
            UpdateAdorner();

            DependencyPropertyDescriptor focusProp = DependencyPropertyDescriptor.FromProperty(UIElement.IsFocusedProperty, typeof(FrameworkElement));
            if (focusProp != null)
            {
                focusProp.AddValueChanged(m_TextBox, (sender, args) => UpdateAdorner());
            }

            DependencyPropertyDescriptor containsTextProp = DependencyPropertyDescriptor.FromProperty(HasTextProperty, typeof(TextBox));
            if (containsTextProp != null)
            {
                containsTextProp.AddValueChanged(m_TextBox, (sender, args) => UpdateAdorner());
            }
        }

        private void UpdateAdorner()
        {
            if (GetHasText(m_TextBox) || m_TextBox.IsFocused)
            {
                // Hide the Watermark Label if the adorner layer is visible
                m_TextBox.ToolTip = GetLabel(m_TextBox);
                m_TextBox.TryRemoveAdorners<TextBlockAdorner>();
            }
            else
            {
                // Show the Watermark Label if the adorner layer is visible
                m_TextBox.ToolTip = null;
                m_TextBox.TryAddAdorner<TextBlockAdorner>(m_TextBlockAdorner);
            }
        }
    }
}