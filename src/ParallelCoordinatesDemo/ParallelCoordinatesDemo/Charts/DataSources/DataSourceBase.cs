/// --------------------------------------------------------------------------------------
/// <copyright file="DataSourceBase.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the DataSourceBase class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace ParallelCoordinatesDemo.Charts.DataSources
{
    public abstract class DataSourceBase : IChartDataSource
    {
        protected readonly List<ChartPoint> m_Points;
        protected readonly List<ChartLine> m_Lines;

        protected DataSourceBase()
        {
            m_Points = new List<ChartPoint>();
            m_Lines = new List<ChartLine>();
        }

        public IEnumerable<ChartPoint> Points
        {
            get { return m_Points; }
        }

        public IEnumerable<ChartLine> Lines
        {
            get { return m_Lines; }
        }

        internal abstract void Process(IChart chart);
    }
}