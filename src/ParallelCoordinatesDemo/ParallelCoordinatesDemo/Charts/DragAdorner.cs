/// --------------------------------------------------------------------------------------
/// <copyright file="DragAdorner.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the DragAdorner class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ParallelCoordinatesDemo.Charts
{
    public class DragAdorner : Adorner
    {
        protected UIElement m_Child;
        protected VisualBrush m_VisualBrush;
        protected UIElement m_Owner;
        protected double m_XCenter;
        protected double m_YCenter;
        private double m_LeftOffset;
        private double m_TopOffset;

        public DragAdorner(UIElement owner, UIElement adornElement, bool useVisualBrush, double opacity)
            : base(owner)
        {
            Debug.Assert(owner != null);
            Debug.Assert(adornElement != null);

            Opacity = opacity;
            m_Owner = owner;

            if (useVisualBrush)
            {
                m_VisualBrush = new VisualBrush(adornElement);

                var r = new Rectangle
                {
                    RadiusX = 3,
                    RadiusY = 3,
                    Width = adornElement.DesiredSize.Width,
                    Height = adornElement.DesiredSize.Height
                };

                m_XCenter = adornElement.DesiredSize.Width / 2;
                m_YCenter = adornElement.DesiredSize.Height / 2;

                r.Fill = m_VisualBrush;
                m_Child = r;

            }
            else
            {
                m_Child = adornElement;
            }
        }

        public double LeftOffset
        {
            get { return m_LeftOffset; }
            set
            {
                m_LeftOffset = value - m_XCenter;
                UpdatePosition();
            }
        }

        public double TopOffset
        {
            get { return m_TopOffset; }
            set
            {
                m_TopOffset = value - m_YCenter;
                UpdatePosition();
            }
        }

        private void UpdatePosition()
        {
            var adorner = (AdornerLayer)Parent;
            if (adorner != null)
            {
                adorner.Update(AdornedElement);
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            return m_Child;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Size MeasureOverride(Size finalSize)
        {
            m_Child.Measure(finalSize);
            return m_Child.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            m_Child.Arrange(new Rect(m_Child.DesiredSize));
            return finalSize;
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            var result = new GeneralTransformGroup();

            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(m_LeftOffset, m_TopOffset));

            return result;
        }
    }
}