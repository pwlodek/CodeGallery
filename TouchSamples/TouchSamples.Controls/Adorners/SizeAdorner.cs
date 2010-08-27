using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace TouchSamples.Controls.Adorners
{
    public class SizeAdorner : Adorner
    {
        private readonly SizeChrome m_Chrome;
        private readonly VisualCollection m_Visuals;

        protected override int VisualChildrenCount
        {
            get
            {
                return m_Visuals.Count;
            }
        }

        public SizeAdorner(UIElement adornedControl)
            : base(adornedControl)
        {
            SnapsToDevicePixels = true;
            
            m_Chrome = new SizeChrome();
            m_Chrome.DataContext = adornedControl;
            m_Visuals = new VisualCollection(this);
            m_Visuals.Add(m_Chrome);
        }

        protected override Visual GetVisualChild(int index)
        {
            return m_Visuals[index];
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            m_Chrome.Arrange(new Rect(new Point(0.0, 0.0), arrangeBounds));
            return arrangeBounds;
        }
    }
}