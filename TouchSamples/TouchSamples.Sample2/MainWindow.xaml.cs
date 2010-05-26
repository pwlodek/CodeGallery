using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace TouchSamples.Sample2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = this;
            e.Handled = true;
        }

        private void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {

            // Get the UI element and its RenderTransform matrix.
            var uiElement = e.OriginalSource as UIElement;
            
            if (uiElement != null)
            {
                var rectsMatrix = ((MatrixTransform)uiElement.RenderTransform).Matrix;

                // Rotate the Rectangle.
                rectsMatrix.RotateAt(e.DeltaManipulation.Rotation,
                                     e.ManipulationOrigin.X,
                                     e.ManipulationOrigin.Y);

                // Resize the Rectangle.  Keep it square 
                // so use only the X value of Scale.
                rectsMatrix.ScaleAt(e.DeltaManipulation.Scale.X,
                                    e.DeltaManipulation.Scale.X,
                                    e.ManipulationOrigin.X,
                                    e.ManipulationOrigin.Y);

                // Move the Rectangle.
                rectsMatrix.Translate(e.DeltaManipulation.Translation.X,
                                      e.DeltaManipulation.Translation.Y);

                // Apply the changes to the UI element.
                uiElement.RenderTransform = new MatrixTransform(rectsMatrix);

                var containingRect = new Rect(((FrameworkElement)e.ManipulationContainer).RenderSize);

                var shapeBounds = uiElement.RenderTransform.TransformBounds(
                        new Rect(uiElement.RenderSize));

                // Check if the rectangle is completely in the window.
                // If it is not and intertia is occuring, stop the manipulation.
                if (e.IsInertial && !containingRect.Contains(shapeBounds))
                {
                    e.Complete();
                }
            }

            e.Handled = true;
        }

        private void OnInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {

            // Decrease the velocity of the Rectangle's movement by 
            // 10 inches per second every second.
            // (10 inches * 96 pixels per inch / 1000ms^2)
            e.TranslationBehavior.DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's resizing by 
            // 0.1 inches per second every second.
            // (0.1 inches * 96 pixels per inch / (1000ms^2)
            e.ExpansionBehavior.DesiredDeceleration = 0.1 * 96 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's rotation rate by 
            // 2 rotations per second every second.
            // (2 * 360 degrees / (1000ms^2)
            e.RotationBehavior.DesiredDeceleration = 720 / (1000.0 * 1000.0);

            e.Handled = true;
        }
    }
}
