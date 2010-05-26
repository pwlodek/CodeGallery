/// --------------------------------------------------------------------------------------
/// <copyright file="IChartDataSource.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the IChartDataSource interface.
/// </summary>
/// --------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace ParallelCoordinatesDemo.Charts.DataSources
{
    public interface IChartDataSource
    {
        IEnumerable<ChartPoint> Points { get; }
        IEnumerable<ChartLine> Lines { get; }
    }
}