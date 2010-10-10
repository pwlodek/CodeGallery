/// --------------------------------------------------------------------------------------
/// <copyright file="ChartLine.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the ChartLine class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ParallelCoordinatesDemo.Charts
{
    public class ChartLine : Control, IChartElement
    {
        #region Constructors
        static ChartLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartLine), new FrameworkPropertyMetadata(typeof(ChartLine)));
        }

        public ChartLine()
            : this(new ObservableCollection<ChartLineSegment>())
        {
        }

        public ChartLine(IEnumerable<ChartLineSegment> segments)
            : this(new ObservableCollection<ChartLineSegment>(segments))
        {
        }

        public ChartLine(params ChartLineSegment[] segments)
            : this(new ObservableCollection<ChartLineSegment>(segments))
        {
        }

        protected ChartLine(ObservableCollection<ChartLineSegment> segments)
        {
            Segments = segments;
            Segments.CollectionChanged += SegmentsChangedHandler;
            foreach (var segment in Segments)
            {
                segment.Line = this;
            }
        }

        private void SegmentsChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                foreach (ChartLineSegment segment in e.NewItems)
                {
                    Debug.Assert(segment.Line == null);
                    segment.Line = this;
                }
            }
            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                foreach (ChartLineSegment segment in e.OldItems)
                {
                    Debug.Assert(segment.Line != null);
                    segment.Line = null;
                }
            }
        }

        #endregion

        #region Public Properties
        public ObservableCollection<ChartLineSegment> Segments { get; private set; }
        public IChart Chart { get; set; }

        public IEnumerable<ChartPoint> ChartPoints
        {
            get
            {
                if (Segments.Count > 0)
                    yield return Segments[0].PointX;

                foreach (var segment in Segments)
                {
                    yield return segment.PointY;
                }
            }
        }
        #endregion

        #region Public Dependency Properties
        public bool IsHighlighted
        {
            get { return (bool)GetValue(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }

        public static readonly DependencyProperty IsHighlightedProperty =
            DependencyProperty.Register("IsHighlighted", typeof(bool), typeof(ChartLine), new UIPropertyMetadata(false, OnIsHighlightedChanged));

        private static void OnIsHighlightedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var line = (ChartLine)d;
            foreach (var chartPoint in line.ChartPoints)
            {
                chartPoint.IsHighlighted = line.IsHighlighted;
            }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(ChartLine), new UIPropertyMetadata(false, OnIsSelectedChanged));

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var line = (ChartLine)d;

            // Mark all point as selected
            foreach (var chartPoint in line.ChartPoints)
            {
                chartPoint.IsSelected = line.IsSelected;
            }

            // Update Chart's SelectedLines collection
            if (line.IsSelected && line.Chart.SelectedLines.Contains(line) == false)
            {
                var chart = (Chart) line.Chart;
                chart.InternalSelectedLines.Add(line);
            }
            else
                if (line.IsSelected == false && line.Chart.SelectedLines.Contains(line))
                {
                    var chart = (Chart)line.Chart;
                    chart.InternalSelectedLines.Remove(line);
                }
        }

        #endregion

        #region Protected Overrides
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (Keyboard.IsKeyDown(Key.LeftAlt) == false)
                IsHighlighted = true;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            IsHighlighted = false;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (Keyboard.IsKeyDown(Chart.SelectionKey))
            {
                IsSelected = !IsSelected;
            }
            else
            {
                foreach (var line in Chart.Lines)
                {
                    line.IsSelected = false;
                }
                IsSelected = !IsSelected;
            }

            e.Handled = true;
        }
        #endregion
    }
}