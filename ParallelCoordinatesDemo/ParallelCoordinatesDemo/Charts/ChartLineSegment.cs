/// --------------------------------------------------------------------------------------
/// <copyright file="ChartLineSegment.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the ChartLineSegment class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System;
using System.ComponentModel;

namespace ParallelCoordinatesDemo.Charts
{
    public class ChartLineSegment : INotifyPropertyChanged, IChartElement
    {
        private ChartPoint m_PointX;
        private ChartPoint m_PointY;
        private ChartLine m_Line;

        public ChartLineSegment() : this(new ChartPoint(), new ChartPoint())
        {
            
        }

        public ChartLineSegment(ChartPoint x, ChartPoint y)
        {
            PointX = x;
            PointY = y;
        }

        public ChartPoint PointX
        {
            get { return m_PointX; }
            set
            {
                m_PointX = value;
                OnPropertyChanged("PointX");
            }
        }

        public ChartPoint PointY
        {
            get { return m_PointY; }
            set
            {
                m_PointY = value;
                OnPropertyChanged("PointY");
            }
        }

        public ChartLine Line
        {
            get { return m_Line; }
            set
            {
                m_Line = value;
                OnPropertyChanged("Line");
            }
        }

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
    }
}