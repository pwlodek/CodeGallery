/// --------------------------------------------------------------------------------------
/// <copyright file="IChartDataProvider.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the IChartDataProvider interface.
/// </summary>
/// --------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace ParallelCoordinatesDemo.Charts.DataSources.Providers
{
    public interface IChartDataProvider
    {
        void AttachAxes(IChart chart, int dimension, string[] axisLabels);
        void AttachDataSets(IChart chart, IEnumerable<MultiDimensionalPoint> points, int pointDimension, object[] tags);
        ChartLine AttachDataSet(IChart chart, MultiDimensionalPoint point, int pointDimension, object tag);
    }
}