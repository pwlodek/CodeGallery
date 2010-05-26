using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TouchSamples.Controls
{
    public class ScatterView : ItemsControl
    {
        private static readonly Random Randomizer = new Random((int) DateTime.Now.Ticks);

        static ScatterView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScatterView),
                new FrameworkPropertyMetadata(typeof(ScatterView)));
        }

        public ScatterView()
        {
            // Enable touch support
            ManipulationStarting += OnManipulationStarting;
            ManipulationDelta += OnManipulationDelta;
            ManipulationInertiaStarting += OnInertiaStarting;

            Loaded += ScatterView_Loaded;
        }

        private void ScatterView_Loaded(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        public void Reset()
        {
            // Scatter the items around
            foreach (var item in Items)
            {
                var scatterViewItem = (ScatterViewItem)ItemContainerGenerator.ContainerFromItem(item);
                var rectsMatrix = ((MatrixTransform)scatterViewItem.RenderTransform).Matrix;

                // Rotate the item.
                rectsMatrix.RotateAt(Randomizer.NextDouble() * 180, scatterViewItem.RenderSize.Width / 2, scatterViewItem.RenderSize.Height / 2);

                var maxXOffset = RenderSize.Width - scatterViewItem.RenderSize.Width;
                var maxYOffset = RenderSize.Height - scatterViewItem.RenderSize.Height;

                // Move the item.
                rectsMatrix.Translate(Randomizer.NextDouble() * maxXOffset, Randomizer.NextDouble() * maxYOffset);

                // Apply the changes to the item.
                scatterViewItem.RenderTransform = new MatrixTransform(rectsMatrix);
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
                var rectsMatrix = ((MatrixTransform) uiElement.RenderTransform).Matrix;

                // Rotate the item.
                rectsMatrix.RotateAt(e.DeltaManipulation.Rotation,
                                     e.ManipulationOrigin.X,
                                     e.ManipulationOrigin.Y);

                // Resize the item.  Keep it square 
                // so use only the X value of Scale.
                rectsMatrix.ScaleAt(e.DeltaManipulation.Scale.X,
                                    e.DeltaManipulation.Scale.X,
                                    e.ManipulationOrigin.X,
                                    e.ManipulationOrigin.Y);

                // Move the item.
                rectsMatrix.Translate(e.DeltaManipulation.Translation.X,
                                      e.DeltaManipulation.Translation.Y);

                // Apply the changes to the item.
                uiElement.RenderTransform = new MatrixTransform(rectsMatrix);

                var containingRect = new Rect(((FrameworkElement) e.ManipulationContainer).RenderSize);
                var elementBounds = uiElement.RenderTransform.TransformBounds(
                    new Rect(uiElement.RenderSize));

                // Check if the ScatterViewItem is completely in the ScatterView.
                // If it is not and intertia is occuring, stop the manipulation.
                if (e.IsInertial && !containingRect.Contains(elementBounds))
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

        #endregion
    }
}
