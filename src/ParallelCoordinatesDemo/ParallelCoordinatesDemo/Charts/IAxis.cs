/// --------------------------------------------------------------------------------------
/// <copyright file="IAxis.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the IAxis interface.
/// </summary>
/// --------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using ParallelCoordinatesDemo.Charts.Transformations;

namespace ParallelCoordinatesDemo.Charts
{
    public interface IAxis
    {
        string Label { get; set; }
        AxisOrientation Orientation { get; set; }
        int Dimension { get; set; }
        double Min { get; set; }
        double Max { get; set; }
        double Value { get; set; }
        double OriginalValue { get; set; }
        double Scale { get; set; }
        double Translate { get; set; }
        IChart Chart { get; set; }
        IAxisTransformation Transformation { get; set; }
        ObservableCollection<IAxis> DependentAxes { get; }
        ObservableCollection<ChartPoint> Points { get; }
    }
}