using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TouchSamples.Sample1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Dictionary<int, Shape> m_Shapes = new Dictionary<int, Shape>();
        private readonly Random m_Rnd = new Random((int) DateTime.Now.Ticks);

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create shape of given size, random fill and translate it to X,Y.
        /// </summary>
        /// <param name="x">X.</param>
        /// <param name="y">Y.</param>
        /// <param name="size">Desired size.</param>
        /// <returns>New shape.</returns>
        private Shape GetShape(double  x, double y, Size size)
        {
            var elipse = new Ellipse();
            var transform = new TranslateTransform(x - size.Width / 2, y - size.Height / 2);

            elipse.Fill = new SolidColorBrush(Color.FromRgb((byte)m_Rnd.Next(256), (byte)m_Rnd.Next(256), (byte)m_Rnd.Next(256)));
            elipse.StrokeThickness = 2;
            elipse.Stroke = Brushes.Black;
            elipse.Width = size.Width;
            elipse.Height = size.Height;
            elipse.RenderTransform = transform;

            return elipse;
        }

        private void OnTouchDown(object sender, TouchEventArgs e)
        {
            // Get the touch point
            var touchPoint = e.GetTouchPoint(m_Canvas);

            // Get the touch size
            var touchSize = touchPoint.Size;
            
            // If the touch device does not return proper size of the touch, create the shape with a default size
            var shapeSize = (touchSize.IsEmpty || touchSize.Height == 0.0 || touchSize.Width == 0.0)
                           ? new Size(100, 100)
                           : touchSize;

            // Create the shape
            var shape = GetShape(touchPoint.Position.X, touchPoint.Position.Y, shapeSize);
            
            // Stash the shape and add it to the canvas
            m_Shapes.Add(touchPoint.TouchDevice.Id, shape);
            m_Canvas.Children.Add(shape);

            e.Handled = true;
        }

        private void OnTouchUp(object sender, TouchEventArgs e)
        {
            // Get the touch point
            var touchPoint = e.GetTouchPoint(m_Canvas);

            // Retrieve the shape associated with this touch
            var shape = m_Shapes[touchPoint.TouchDevice.Id];
            
            // Release touch capture and remove the shape
            m_Canvas.ReleaseTouchCapture(touchPoint.TouchDevice);
            m_Canvas.Children.Remove(shape);
            m_Shapes.Remove(touchPoint.TouchDevice.Id);

            e.Handled = true;
        }

        private void OnTouchMove(object sender, TouchEventArgs e)
        {
            // Get the touch point
            var touchPoint = e.GetTouchPoint(m_Canvas);

            // Retrieve the shape associated with this touch
            var shape = m_Shapes[touchPoint.TouchDevice.Id];
            var translateTransform = (TranslateTransform) shape.RenderTransform;

            // Update shape's position
            translateTransform.X = touchPoint.Position.X - shape.Width / 2;
            translateTransform.Y = touchPoint.Position.Y - shape.Height / 2;

            e.Handled = true;
        }
    }
}
