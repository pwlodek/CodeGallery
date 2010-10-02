using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using TouchSamples.Controls.Adorners;

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
            ManipulationStarted += OnManipulationStarted;
            ManipulationCompleted += OnManipulationCompleted;
            ManipulationDelta += OnManipulationDelta;
            ManipulationInertiaStarting += OnInertiaStarting;

            Loaded += (s, e) => Reset();
        }

        private void OnManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            var item = (UIElement) e.OriginalSource;
            AddAdorner(item);
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            var item = (UIElement) e.OriginalSource;
            RemoveAdorner(item);
        }

        private static void SelectedChanged(object sender, RoutedEventArgs e)
        {
            var scatterView = (ScatterView) sender;
            var currentItem = (ScatterViewItem) e.OriginalSource;
            var items = (from object item in scatterView.Items
                         select (ScatterViewItem) scatterView.ItemContainerGenerator.ContainerFromItem(item)).ToList();
            
            UpdateIsSelectedProperty(items, currentItem);
            UpdateZIndexProperty(items, currentItem);
        }

        private static void UpdateIsSelectedProperty(IEnumerable<ScatterViewItem> items, ScatterViewItem current)
        {
            foreach (var item in items)
            {
                if (item != current)
                {
                    item.IsSelected = false;
                }
            }
        }

        private static void UpdateZIndexProperty(IEnumerable<ScatterViewItem> items, ScatterViewItem current)
        {
            var allButCurrent = items.Where(t => t != current).OrderBy(t => t.ZIndex).ToList();
            for (int i = 0; i < allButCurrent.Count; i++)
            {
                allButCurrent[i].ZIndex = i + 1;
            }
            current.ZIndex = allButCurrent.Count + 1;
        }

        public void Reset()
        {
            var zIndex = 1;

            // Scatter the items around
            foreach (var item in Items)
            {
                var scatterViewItem = (ScatterViewItem) ItemContainerGenerator.ContainerFromItem(item);
                var matrix = new Matrix();

                // Apply proper z index
                scatterViewItem.ZIndex = zIndex++;

                // Rotate the item.
                matrix.RotateAt(Randomizer.NextDouble() * 180, scatterViewItem.RenderSize.Width / 2, scatterViewItem.RenderSize.Height / 2);

                var maxXOffset = RenderSize.Width - scatterViewItem.RenderSize.Width;
                var maxYOffset = RenderSize.Height - scatterViewItem.RenderSize.Height;

                // Move the item.
                matrix.Translate(Randomizer.NextDouble() * maxXOffset, Randomizer.NextDouble() * maxYOffset);

                // Apply the changes to the item.
                scatterViewItem.RenderTransform = new MatrixTransform(matrix);

                // Update adorner
                UpdateAdorner(scatterViewItem, matrix);
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

                // Update adorner
                UpdateAdorner(uiElement, matrix);

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

        #region Adorners

        private void UpdateAdorner(UIElement element, Matrix matrix)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(element);
            if (adornerLayer != null)
            {
                var adorners = adornerLayer.GetAdorners(element);
                if (adorners != null && adorners.Length > 0)
                {
                    adorners[0].RenderTransform = new MatrixTransform(matrix);
                }
            }
        }

        private void RemoveAdorner(UIElement element)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(element);
            if (adornerLayer != null)
            {
                var adorners = adornerLayer.GetAdorners(element);
                if (adorners != null && adorners.Length > 0)
                {
                    adorners[0].Visibility = Visibility.Collapsed;
                    adornerLayer.Remove(adorners[0]);
                }
            }
        }

        private void AddAdorner(UIElement element)
        {
            var adornerLayer = AdornerLayer.GetAdornerLayer(element);
            if (adornerLayer != null)
            {
                var adorner = new SizeAdorner(element);
                adornerLayer.Add(adorner);
                adorner.Visibility = Visibility.Visible;
            }
        }

        #endregion
    }
}
