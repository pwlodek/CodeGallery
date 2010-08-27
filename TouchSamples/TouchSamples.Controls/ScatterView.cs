using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace TouchSamples.Controls
{
    public class ScatterView : Selector
    {
        private static readonly Random Randomizer = new Random((int) DateTime.Now.Ticks);

        static ScatterView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScatterView),
                new FrameworkPropertyMetadata(typeof(ScatterView)));

            EventManager.RegisterClassHandler(typeof(ScatterView), SelectedEvent, (RoutedEventHandler) SelectedChanged);
        }

        public ScatterView()
        {
            // Enable touch support
            ManipulationStarting += OnManipulationStarting;
            ManipulationDelta += OnManipulationDelta;
            ManipulationInertiaStarting += OnInertiaStarting;

            Loaded += (s, e) => Reset();
        }

        private static void SelectedChanged(object sender, RoutedEventArgs e)
        {
            var scatterView = (ScatterView) sender;
            foreach (var item in scatterView.Items)
            {
                var scatterViewItem = (ScatterViewItem) scatterView.ItemContainerGenerator.ContainerFromItem(item);

                if (scatterViewItem != e.OriginalSource)
                {
                    scatterViewItem.IsSelected = false;
                }
            }
        }

        public void Reset()
        {
            // Scatter the items around
            foreach (var item in Items)
            {
                var scatterViewItem = (ScatterViewItem) ItemContainerGenerator.ContainerFromItem(item);
                var matrix = new Matrix();

                // Rotate the item.
                matrix.RotateAt(Randomizer.NextDouble() * 180, scatterViewItem.RenderSize.Width / 2, scatterViewItem.RenderSize.Height / 2);

                var maxXOffset = RenderSize.Width - scatterViewItem.RenderSize.Width;
                var maxYOffset = RenderSize.Height - scatterViewItem.RenderSize.Height;

                // Move the item.
                matrix.Translate(Randomizer.NextDouble() * maxXOffset, Randomizer.NextDouble() * maxYOffset);

                // Apply the changes to the item.
                scatterViewItem.RenderTransform = new MatrixTransform(matrix);
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ScatterViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ScatterViewItem;
        }
        
        #region Multi Touch Support

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
                var matrixTransform = (MatrixTransform) uiElement.RenderTransform;
                var matrix = matrixTransform.Matrix;

                // Rotate the item.
                matrix.RotateAt(e.DeltaManipulation.Rotation,
                                e.ManipulationOrigin.X,
                                e.ManipulationOrigin.Y);

                // Resize the item.  Keep it square 
                // so use only the X value of Scale.
                matrix.ScaleAt(e.DeltaManipulation.Scale.X,
                               e.DeltaManipulation.Scale.X,
                               e.ManipulationOrigin.X,
                               e.ManipulationOrigin.Y);

                // Move the item.
                matrix.Translate(e.DeltaManipulation.Translation.X,
                                 e.DeltaManipulation.Translation.Y);

                // Apply the changes to the item.
                matrixTransform.Matrix = matrix;

                var manipulationContainer = (FrameworkElement)e.ManipulationContainer;
                var containingRect = new Rect(manipulationContainer.RenderSize);
                var elementBounds = uiElement.RenderTransform.TransformBounds(new Rect(uiElement.RenderSize));

                // Check if the ScatterViewItem is completely in the ScatterView.
                // If it is not and intertia is occuring, stop the manipulation.
                if (e.IsInertial && !containingRect.Contains(elementBounds))
                {
                    //e.ReportBoundaryFeedback(e.DeltaManipulation);
                    e.Complete();
                }
            }

            e.Handled = true;
        }

        private void OnInertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {
            // Decrease the velocity of the Rectangle's movement by 
            // 10 inches per second every second.
            // (10 inches * 96 DIPS per inch / 1000ms^2)
            e.TranslationBehavior = new InertiaTranslationBehavior
            {
                InitialVelocity = e.InitialVelocities.LinearVelocity,
                DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0)
            };

            // Decrease the velocity of the Rectangle's resizing by 
            // 0.1 inches per second every second.
            // (0.1 inches * 96 DIPS per inch / (1000ms^2)
            e.ExpansionBehavior = new InertiaExpansionBehavior
            {
                InitialVelocity = e.InitialVelocities.ExpansionVelocity,
                DesiredDeceleration = 0.1 * 96 / 1000.0 * 1000.0
            };

            // Decrease the velocity of the Rectangle's rotation rate by 
            // 2 rotations per second every second.
            // (2 * 360 degrees / (1000ms^2)
            e.RotationBehavior = new InertiaRotationBehavior
            {
                InitialVelocity = e.InitialVelocities.AngularVelocity,
                DesiredDeceleration = 720 / (1000.0 * 1000.0)
            };

            e.Handled = true;               
        }

        #endregion
    }
}
