/// --------------------------------------------------------------------------------------
/// <copyright file="Chart.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the Chart class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ParallelCoordinatesDemo.Charts.DataSources;

namespace ParallelCoordinatesDemo.Charts
{
    [TemplatePart(Name = Chart.DragThumb, Type = typeof(Thumb))]
    public class Chart : Control, IChart
    {
        public const string DragThumb = "PART_DragThumb";

        // Required for drag functionality using a thumb
        private Point m_InitialDragPoint;

        static Chart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Chart), new FrameworkPropertyMetadata(typeof(Chart)));
        }

        public Chart()
        {
            var selectedLines = new ObservableCollection<ChartLine>();
            InternalSelectedLines = selectedLines;
            SelectedLines = new ReadOnlyObservableCollection<ChartLine>(selectedLines);

            Axes = new ObservableCollection<IAxis>();
            Lines = new ObservableCollection<ChartLine>();
            Points = new ObservableCollection<ChartPoint>();

            Axes.CollectionChanged += ChartElementsChanged;
            Lines.CollectionChanged += ChartElementsChanged;
            Points.CollectionChanged += ChartElementsChanged;

            CommandBindings.Add(new CommandBinding(SelectTool, SelectToolExecuted, AlwaysCanExecute));
            CommandBindings.Add(new CommandBinding(FitToScreen, FitToViewExecuted, AlwaysCanExecute));
        }

        private void ChartElementsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (IChartElement chartElement in e.NewItems)
                {
                    chartElement.Chart = this;
                }
            }
            else if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (IChartElement chartElement in e.OldItems)
                {
                    chartElement.Chart = null;
                }
            }
        }

        #region Grab Tool
        private void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            var x = m_InitialDragPoint.X + e.HorizontalChange;
            var y = m_InitialDragPoint.Y + e.VerticalChange;
            var p = new Point(x, y);

            TranslateXY = p;
            e.Handled = true;
        }

        private void OnDragStarted(object sender, DragStartedEventArgs e)
        {
            m_InitialDragPoint = TranslateXY;
            e.Handled = true;
        }
        #endregion

        #region Dependency Properties

        #region DataSource Property
        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register("DataSource", typeof(IChartDataSource), typeof(Chart), new UIPropertyMetadata(null, DataSourceChanged));

        public IChartDataSource DataSource
        {
            get { return (IChartDataSource)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }

        private static void DataSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chart = (Chart)d;

            if (e.NewValue != null)
            {
                // Clear rendering collections
                chart.Lines.Clear();
                chart.Points.Clear();

                if (chart.AutoGenerateAxes)
                {
                    chart.Axes.Clear();
                }

                var dataSourceBase = (DataSourceBase)e.NewValue;
                dataSourceBase.Process(chart);
            }
        }
        #endregion

        public ChartType ChartType
        {
            get { return (ChartType)GetValue(ChartTypeProperty); }
            set { SetValue(ChartTypeProperty, value); }
        }

        public static readonly DependencyProperty ChartTypeProperty =
            DependencyProperty.Register("ChartType", typeof(ChartType), typeof(Chart), new UIPropertyMetadata(ChartType.ParallelCoordinates));

        public bool AutoGenerateAxes
        {
            get { return (bool)GetValue(AutoGenerateAxesProperty); }
            set { SetValue(AutoGenerateAxesProperty, value); }
        }

        public static readonly DependencyProperty AutoGenerateAxesProperty =
            DependencyProperty.Register("AutoGenerateAxes", typeof(bool), typeof(Chart), new UIPropertyMetadata(true));

        public Point TranslateXY
        {
            get { return (Point)GetValue(TranslateXYProperty); }
            set { SetValue(TranslateXYProperty, value); }
        }

        public static readonly DependencyProperty TranslateXYProperty =
            DependencyProperty.Register("TranslateXY", typeof(Point), typeof(Chart), new UIPropertyMetadata(new Point()));

        public ObservableCollection<IAxis> Axes
        {
            get { return (ObservableCollection<IAxis>)GetValue(AxesProperty); }
            set { SetValue(AxesProperty, value); }
        }

        public static readonly DependencyProperty AxesProperty =
            DependencyProperty.Register("Axes", typeof(ObservableCollection<IAxis>), typeof(Chart));

        public ObservableCollection<ChartLine> Lines
        {
            get { return (ObservableCollection<ChartLine>)GetValue(LinesProperty); }
            set { SetValue(LinesProperty, value); }
        }

        public static readonly DependencyProperty LinesProperty =
            DependencyProperty.Register("Lines", typeof(ObservableCollection<ChartLine>), typeof(Chart));

        public ObservableCollection<ChartPoint> Points
        {
            get { return (ObservableCollection<ChartPoint>)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(ObservableCollection<ChartPoint>), typeof(Chart));

        public DiagramTools SelectedTool
        {
            get { return (DiagramTools)GetValue(SelectedToolProperty); }
            set { SetValue(SelectedToolProperty, value); }
        }

        public static readonly DependencyProperty SelectedToolProperty =
            DependencyProperty.Register("SelectedTool", typeof(DiagramTools), typeof(Chart), new UIPropertyMetadata(DiagramTools.Select));

        public Key SelectionKey
        {
            get { return (Key)GetValue(SelectionKeyProperty); }
            set { SetValue(SelectionKeyProperty, value); }
        }

        public static readonly DependencyProperty SelectionKeyProperty =
            DependencyProperty.Register("SelectionKey", typeof(Key), typeof(ChartLine), new UIPropertyMetadata(Key.LeftCtrl));

        #endregion

        #region Read Only Dependency Properties

        internal ObservableCollection<ChartLine> InternalSelectedLines
        {
            get { return (ObservableCollection<ChartLine>)GetValue(InternalSelectedLinesProperty); }
            private set { SetValue(InternalSelectedLinesPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey InternalSelectedLinesPropertyKey =
            DependencyProperty.RegisterReadOnly("InternalSelectedLines", typeof(ObservableCollection<ChartLine>), typeof(Chart), new PropertyMetadata());

        internal static readonly DependencyProperty InternalSelectedLinesProperty =
            InternalSelectedLinesPropertyKey.DependencyProperty;

        public ReadOnlyObservableCollection<ChartLine> SelectedLines
        {
            get { return (ReadOnlyObservableCollection<ChartLine>)GetValue(SelectedLinesProperty); }
            private set { SetValue(SelectedLinesPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey SelectedLinesPropertyKey =
            DependencyProperty.RegisterReadOnly("SelectedLines", typeof(ReadOnlyObservableCollection<ChartLine>), typeof(Chart), new UIPropertyMetadata());

        public static readonly DependencyProperty SelectedLinesProperty =
            SelectedLinesPropertyKey.DependencyProperty;

        #endregion

        #region Overrides

        #region Templates
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var dragThumb = GetPart<Thumb>(DragThumb);
            if (dragThumb != null)
            {
                dragThumb.DragStarted += OnDragStarted;
                dragThumb.DragDelta += OnDragDelta;
            }
        }

        private T GetPart<T>(string name) where T : class
        {
            return Template.FindName(name, this) as T;
        }
        #endregion

        #region Mouse
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (SelectedTool == DiagramTools.Select)
            {
                foreach (var line in Lines)
                {
                    line.IsSelected = false;
                }
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                IAxis helperAxis = Axes[Axes.Count - 1];
                helperAxis.Transformation.Scale(helperAxis, e.Delta);
                helperAxis.Transformation.Transform(helperAxis, helperAxis.DependentAxes);
            }
            else
            {
                for (int i = 0; i < Axes.Count - 1; i++)
                {
                    Axes[i].Transformation.Scale(Axes[i], e.Delta);
                    Axes[i].Transformation.Transform(Axes[i], Axes[i].Points);
                }    
            }
        }
        #endregion

        #endregion

        #region Commands

        #region Select Tool

        public static RoutedUICommand SelectTool = new RoutedUICommand("Select Tool", "SelectTool", typeof(Chart));
        public static RoutedUICommand FitToScreen = new RoutedUICommand("Fit To Screen", "FitToScreen", typeof(Chart));

        private static void AlwaysCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SelectToolExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is DiagramTools)
            {
                SelectedTool = (DiagramTools)e.Parameter;
            }
            else
            {
                SelectedTool = DiagramTools.Select;
            }
        }

        #endregion

        #region Select Tool

        //public static RoutedUICommand FitToView = new RoutedUICommand("Fit To View", "FitToView", typeof(Chart));

        private void FitToViewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (var ax in Axes)
            {
                ax.Transformation.FitToView(this, ax, ActualWidth, ActualHeight);
            }
            TranslateXY = new Point(0.0, ActualHeight / 2);
        }

        #endregion

        #endregion
    }
}