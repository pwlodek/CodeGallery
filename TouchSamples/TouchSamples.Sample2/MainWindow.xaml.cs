using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TouchSamples.Sample2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly Random m_Rnd = new Random((int)DateTime.Now.Ticks);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                var shape = GenerateRandomShape();
                
                // Add shape to the canvas
                m_Canvas.Children.Add(shape);
            }
        }

        #region Shape Generation

        /// <summary>
        /// Generate random ellipse or rectangle.
        /// </summary>
        /// <returns>Random shape.</returns>
        private Shape GenerateRandomShape()
        {
            const double minSize = 50;
            const double maxSize = 250;

            bool regular = m_Rnd.Next(0, 2) == 0;
            bool isRectangle = m_Rnd.Next(0, 2) == 0;
            double width, height;

            if (regular)
            {
                width = height = m_Rnd.NextDouble() * maxSize + minSize;
            }
            else
            {
                width = m_Rnd.NextDouble() * maxSize + minSize;
                height = m_Rnd.NextDouble() * maxSize + minSize;
            }

            var size = new Size(width, height);
            var shape = isRectangle ? new Rectangle() : (Shape) new Ellipse();

            SetupShape(shape, size,
                       Math.Max(m_Rnd.NextDouble() * ActualWidth - size.Width / 2, size.Width / 2),
                       Math.Max(m_Rnd.NextDouble() * ActualHeight - size.Height / 2, size.Height / 2));

            return shape;
        }

        /// <summary>
        /// Give a shape proper size, random fill, translate it to X,Y and enable manipulation.
        /// </summary>
        /// <param name="shape">Shape to be set up.</param>
        /// <param name="size">Desired size.</param>
        /// <param name="x">X.</param>
        /// <param name="y">Y.</param>
        private void SetupShape(Shape shape, Size size, double x, double y)
        {
            var matrix = new Matrix();
            matrix.OffsetX = x - size.Width / 2;
            matrix.OffsetY = y - size.Height / 2;

            shape.Fill = new SolidColorBrush(Color.FromRgb((byte)m_Rnd.Next(256), (byte)m_Rnd.Next(256), (byte)m_Rnd.Next(256)));
            shape.StrokeThickness = 2;
            shape.Stroke = Brushes.Black;
            shape.Width = size.Width;
            shape.Height = size.Height;
            shape.RenderTransform = new MatrixTransform(matrix);
            shape.IsManipulationEnabled = true; // this enables manipulation on this element
        }

        #endregion

        private void OnManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = this;
            e.Handled = true;
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            // Get the UI element and its RenderTransform matrix
            var uiElement = e.OriginalSource as UIElement;
            if (uiElement != null)
            {
                var matrixTransform = (MatrixTransform) uiElement.RenderTransform;
                var matrix = matrixTransform.Matrix;

                // Rotate the element
                matrix.RotateAt(e.DeltaManipulation.Rotation,
                                e.ManipulationOrigin.X,
                                e.ManipulationOrigin.Y);

                // Resize the element
                matrix.ScaleAt(e.DeltaManipulation.Scale.X,
                               e.DeltaManipulation.Scale.Y,
                               e.ManipulationOrigin.X,
                               e.ManipulationOrigin.Y);

                // Move the element
                matrix.Translate(e.DeltaManipulation.Translation.X,
                                 e.DeltaManipulation.Translation.Y);

                // Apply the changes to the element
                matrixTransform.Matrix = matrix;

                var manipulationContainer = (FrameworkElement) e.ManipulationContainer;
                var containingRect = new Rect(manipulationContainer.RenderSize);
                var shapeBounds = uiElement.RenderTransform.TransformBounds(new Rect(uiElement.RenderSize));

                // Check if the element is completely in the window,
                // if it is not and intertia is occuring, stop the manipulation
                if (e.IsInertial && !containingRect.Contains(shapeBounds))
                {
                    e.Complete();
                }
            }

            e.Handled = true;
        }

        private void OnInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            // Decrease the velocity of the Rectangle's resizing by 
            // 0.1 inches per second every second.
            // (0.1 inches * 96 pixels per inch / (1000ms^2)
            e.ExpansionBehavior.DesiredDeceleration = 0.1 * 96 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's rotation rate by 
            // 2 rotations per second every second.
            // (2 * 360 degrees / (1000ms^2)
            e.RotationBehavior.DesiredDeceleration = 720 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's movement by 
            // 10 inches per second every second.
            // (10 inches * 96 pixels per inch / 1000ms^2)
            e.TranslationBehavior.DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);

            e.Handled = true;
        }
    }
}
