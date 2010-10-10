/// --------------------------------------------------------------------------------------
/// <copyright file="ChartPoint.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the ChartPoint class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ParallelCoordinatesDemo.Charts.DataSources;

namespace ParallelCoordinatesDemo.Charts
{
    public class ChartPoint : Control, INotifyPropertyChanged, IChartElement
    {
        private MultiDimensionalPoint m_Data;

        static ChartPoint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChartPoint), new FrameworkPropertyMetadata(typeof(ChartPoint)));
        }

        public ChartPoint() : this(0.0, 0.0)
        {
        }

        public ChartPoint(double x, double y)
        {
            Axes = new List<IAxis>();

            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets or sets original data associated with this chart point.
        /// </summary>
        public MultiDimensionalPoint Data
        {
            get { return m_Data; }
            set
            {
                m_Data = value;
                OnPropertyChanged("Data");
            }
        }

        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("X", typeof(double), typeof(ChartPoint), new UIPropertyMetadata(0.0));
        
        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Y", typeof(double), typeof(ChartPoint), new UIPropertyMetadata(0.0));

        public bool IsHighlighted
        {
            get { return (bool)GetValue(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }

        public static readonly DependencyProperty IsHighlightedProperty =
            DependencyProperty.Register("IsHighlighted", typeof(bool), typeof(ChartPoint), new UIPropertyMetadata(false));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(ChartPoint), new UIPropertyMetadata(false));
        
        #region Internal Implementation

        internal IList<IAxis> Axes { get; private set; }

        #endregion

        #region IChartElement Implementation

        public IChart Chart { get; set; }

        #endregion

        #region INotifyPropertyChanged Implementation

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion

        #region Operators

        public static explicit operator Point(ChartPoint point)
        {
            var p = new Point(point.X, point.Y);
            return p;
        }

        #endregion
    }
}