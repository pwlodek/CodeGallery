using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfWatermarkTextBox.Behaviors
{
    public class TextBlockAdorner : Adorner
    {
        private readonly TextBlock m_TextBlock;

        public TextBlockAdorner(UIElement adornedElement, string label, Style labelStyle)
            : base(adornedElement)
        {
            m_TextBlock = new TextBlock { Style = labelStyle, Text = label };
        }

        protected override Size MeasureOverride(Size constraint)
        {
            m_TextBlock.Measure(constraint);
            return constraint;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            m_TextBlock.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return m_TextBlock;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
    }
}