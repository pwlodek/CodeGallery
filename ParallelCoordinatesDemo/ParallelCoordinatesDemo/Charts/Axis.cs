/// --------------------------------------------------------------------------------------
/// <copyright file="Axis.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the Axis class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ParallelCoordinatesDemo.Charts.Transformations;

namespace ParallelCoordinatesDemo.Charts
{
    [TemplatePart(Name = Axis.SwitchScale, Type = typeof(Button))]
    [TemplatePart(Name = Axis.VerticalThumb, Type = typeof(Thumb))]
    [TemplatePart(Name = Axis.HorizontalThumb, Type = typeof(Thumb))]
    public class Axis : Control, IAxis, IChartElement
    {
        public const string SwitchScale = "PART_SwitchScale";
        public const string VerticalThumb = "PART_VerticalDragThumb";
        public const string HorizontalThumb = "PART_HorizontalDragThumb";

        static Axis()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Axis), new FrameworkPropertyMetadata(typeof(Axis)));
        }

        public Axis()
        {
            Scale = -1.0;
            Transformation = new LinearAxisTransformation();
            DependentAxes = new ObservableCollection<IAxis>();
            Points = new ObservableCollection<ChartPoint>();

            Points.CollectionChanged += PointsChangedHandler;
        }

        private void PointsChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (ChartPoint chartPoint in e.NewItems)
                {
                    chartPoint.Axes.Add(this);
                    double data = chartPoint.Data[Dimension];

                    if (data > Max)
                        Max = data;

                    if (data < Min)
                        Min = data;
                }
            }

            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (ChartPoint chartPoint in e.OldItems)
                {
                    chartPoint.Axes.Remove(this);
                }
            }
        }

        public IAxisTransformation Transformation { get; set; }
        public ObservableCollection<IAxis> DependentAxes { get; private set; }
        public ObservableCollection<ChartPoint> Points { get; private set; }

        #region Dependency Properties

        public bool IsDropTarget
        {
            get { return (bool)GetValue(IsDropTargetProperty); }
            set { SetValue(IsDropTargetProperty, value); }
        }

        public static readonly DependencyProperty IsDropTargetProperty =
            DependencyProperty.Register("IsDropTarget", typeof(bool), typeof(Axis), new UIPropertyMetadata(false));

        public bool IsHelper
        {
            get { return (bool)GetValue(IsHelperProperty); }
            set { SetValue(IsHelperProperty, value); }
        }

        public static readonly DependencyProperty IsHelperProperty =
            DependencyProperty.Register("IsHelper", typeof(bool), typeof(Axis), new UIPropertyMetadata(false));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(Axis));

        public AxisOrientation Orientation
        {
            get { return (AxisOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(AxisOrientation), typeof(Axis));

        public double Min
        {
            get { return (double)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(double), typeof(Axis), new UIPropertyMetadata(double.MaxValue));

        public double Max
        {
            get { return (double)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(double), typeof(Axis), new UIPropertyMetadata(double.MinValue));

        /// <summary>
        /// Gets or sets original axis value.
        /// </summary>
        public double OriginalValue
        {
            get { return (double)GetValue(OriginalValueProperty); }
            set { SetValue(OriginalValueProperty, value); }
        }

        public static readonly DependencyProperty OriginalValueProperty =
            DependencyProperty.Register("OriginalValue", typeof(double), typeof(Axis), new UIPropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the value after transformation. Internal property. Do not use.
        /// </summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(Axis), new UIPropertyMetadata(0.0));

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale", typeof(double), typeof(Axis), new UIPropertyMetadata(1.0));

        public double Translate
        {
            get { return (double)GetValue(TranslateProperty); }
            set { SetValue(TranslateProperty, value); }
        }

        public static readonly DependencyProperty TranslateProperty =
            DependencyProperty.Register("Translate", typeof(double), typeof(Axis), new UIPropertyMetadata(0.0));

        /// <summary>
        /// Gets or sets the dimension the axis is bound to. This is a dependency property.
        /// </summary>
        public int Dimension
        {
            get { return (int)GetValue(DimensionProperty); }
            set { SetValue(DimensionProperty, value); }
        }

        public static readonly DependencyProperty DimensionProperty =
            DependencyProperty.Register("Dimension", typeof(int), typeof(Axis), new UIPropertyMetadata(0));

        public IChart Chart
        {
            get { return (IChart)GetValue(ChartProperty); }
            set { SetValue(ChartProperty, value); }
        }

        public static readonly DependencyProperty ChartProperty =
            DependencyProperty.Register("Chart", typeof(IChart), typeof(Axis));

        #endregion

        #region Axis Scale

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            Transformation.Scale(this, e.Delta);
            Transformation.Transform(this, Points);
            Transformation.Transform(this, DependentAxes);

            e.Handled = true;
        }

        #endregion

        #region Axis Translate

        private double m_InitialTranslate;

        private void OnDragStarted(object sender, DragStartedEventArgs e)
        {
            m_InitialTranslate = Translate;
        }

        private void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (m_IsDragging) return;
            if (Orientation == AxisOrientation.Vertical)
            {
                Transformation.Translate(this, m_InitialTranslate + e.VerticalChange);
                Transformation.Transform(this, Points);
            }
            else
            {
                Transformation.Translate(this, m_InitialTranslate + e.HorizontalChange);
                Transformation.Transform(this, Points);
            }

            if (IsHelper)
            {
                if (Orientation == AxisOrientation.Vertical)
                {
                    Transformation.Translate(this, m_InitialTranslate + e.VerticalChange);
                    Transformation.Transform(this, DependentAxes);
                    Transformation.Transform(this, Points);
                }
                else
                {
                    Transformation.Translate(this, m_InitialTranslate + e.HorizontalChange);
                    Transformation.Transform(this, DependentAxes);
                    Transformation.Transform(this, Points);
                }
            }
        }

        #endregion

        #region Swap Axes (Drag & Drop)

        private bool m_IsDragging;
        private Point m_StartPoint;
        private FrameworkElement m_DragScope;
        private FrameworkElement m_DragSource;
        private bool m_DragHasLeftScope;

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (e.LeftButton == MouseButtonState.Pressed && !m_IsDragging && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Point position = e.GetPosition(null);

                if (Math.Abs(position.X - m_StartPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - m_StartPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    DoDragDrop();
                }
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            m_StartPoint = e.GetPosition(null);
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);

            if (IsHelper)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                return;
            }

            var model = (DragModel)e.Data.GetData("DragModel");
            if (model != null && model.Source != this)
            {
                model.Adorner.Opacity = 0.9;
                IsDropTarget = true;
            }
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            base.OnDragLeave(e);

            if (IsHelper)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                return;
            }

            var model = (DragModel)e.Data.GetData("DragModel");
            if (model != null && model.Source != this)
            {
                model.Adorner.Opacity = 0.5;
                IsDropTarget = false;
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (IsHelper)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                return;
            }

            var model = (DragModel)e.Data.GetData("DragModel");
            if (model != null && model.Source != this)
            {
                // Swap points in line segments
                Transformation.SwapAxes(model.Source, this);
                IsDropTarget = false;
            }
        }

        private void DoDragDrop()
        {
            // Let's define our DragScope .. In this case it is every thing inside our main window .. 
            m_DragScope = (FrameworkElement) Chart;
            m_DragSource = this;
            
            Debug.Assert(m_DragScope != null);
            Debug.Assert(m_DragSource != null);

            // We enable Drag & Drop in our scope ...  We are not implementing Drop, so it is OK, but this allows us to get DragOver 
            bool previousDrop = m_DragScope.AllowDrop;
            m_DragScope.AllowDrop = true;

            // Let's wire our usual events.. 
            // GiveFeedback just tells it to use no standard cursors..
            GiveFeedbackEventHandler feedbackhandler = DragSource_GiveFeedback;
            m_DragScope.GiveFeedback += feedbackhandler;

            // The DragOver event ... 
            DragEventHandler draghandler = DragSource_DragOver;
            m_DragScope.PreviewDragOver += draghandler; 
            

            // Drag Leave is optional, but write up explains why I like it .. 
            DragEventHandler dragleavehandler = DragScope_DragLeave;
            m_DragScope.DragLeave += dragleavehandler;

            // QueryContinue Drag goes with drag leave... 
            QueryContinueDragEventHandler queryhandler = DragScope_QueryContinueDrag;
            m_DragScope.QueryContinueDrag += queryhandler;

            // Here we create our adorner.. 
            var adorner = new DragAdorner(m_DragScope, this, true, 0.5);
            var layer = AdornerLayer.GetAdornerLayer(m_DragScope);
            layer.Add(adorner);

            m_IsDragging = true;
            m_DragHasLeftScope = false;

            // Finally lets drag drop 
            var data = new DataObject("DragModel", new DragModel { Adorner = adorner, Source = this });
            DragDrop.DoDragDrop(m_DragSource, data, DragDropEffects.Move);

            // Clean up mess
            m_DragScope.AllowDrop = previousDrop;
            AdornerLayer.GetAdornerLayer(m_DragScope).Remove(adorner);

            m_DragSource.GiveFeedback -= feedbackhandler;
            m_DragScope.DragLeave -= dragleavehandler;
            m_DragScope.QueryContinueDrag -= queryhandler;
            m_DragScope.PreviewDragOver -= draghandler;

            m_IsDragging = false;
        }

        private static void DragSource_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            e.UseDefaultCursors = true;
            e.Handled = true;
        }
        
        private void DragScope_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (m_DragHasLeftScope)
            {
                e.Action = DragAction.Cancel;
                e.Handled = true;
            }
        }

        private void DragScope_DragLeave(object sender, DragEventArgs e)
        {
            if (e.OriginalSource == m_DragScope)
            {
                Point p = e.GetPosition(m_DragScope);
                Rect r = VisualTreeHelper.GetContentBounds(m_DragScope);
                if (!r.Contains(p))
                {
                    m_DragHasLeftScope = true;
                    e.Handled = true;
                }
            }
        }

        private void DragSource_DragOver(object sender, DragEventArgs e)
        {
            var model = (DragModel)e.Data.GetData("DragModel");
            if (model != null)
            {
                model.Adorner.LeftOffset = e.GetPosition(m_DragScope).X /* - m_StartPoint.X */ ;
                //adorner.TopOffset = args.GetPosition(DragScope).Y /* - m_StartPoint.Y */ ;
            }
        }

        #endregion

        #region Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var switchButton = GetPart<Button>(SwitchScale);
            if (switchButton != null)
            {
                // Swap + and - on this axis
                switchButton.Click += (s, e) => Transformation.Swap(this);
            }

            var verticalDragThumb = GetPart<Thumb>(VerticalThumb);
            if (verticalDragThumb != null)
            {
                verticalDragThumb.DragDelta += OnDragDelta;
                verticalDragThumb.DragStarted += OnDragStarted;
            }

            var horizontalDragThumb = GetPart<Thumb>(HorizontalThumb);
            if (horizontalDragThumb != null)
            {
                horizontalDragThumb.DragDelta += OnDragDelta;
                horizontalDragThumb.DragStarted += OnDragStarted;
            }
        }

        private T GetPart<T>(string name) where T : class
        {
            return Template.FindName(name, this) as T;
        }

        #endregion

        #region Nested Types
        
        private class DragModel
        {
            public DragAdorner Adorner { get; set; }
            public IAxis Source { get; set; }
        }

        #endregion
    }
}