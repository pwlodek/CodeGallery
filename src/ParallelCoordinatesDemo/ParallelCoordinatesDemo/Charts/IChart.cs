/// --------------------------------------------------------------------------------------
/// <copyright file="IChart.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the IChart interface.
/// </summary>
/// --------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Windows.Input;
using ParallelCoordinatesDemo.Charts.DataSources;

namespace ParallelCoordinatesDemo.Charts
{
    public interface IChart
    {
        double ActualWidth { get; }
        double ActualHeight { get; }
        Key SelectionKey { get; set; }

        bool AutoGenerateAxes { get; set; }
        ObservableCollection<IAxis> Axes { get; }

        IChartDataSource DataSource { get; set; }

        ObservableCollection<ChartPoint> Points { get; }
        ObservableCollection<ChartLine> Lines { get; }
        ReadOnlyObservableCollection<ChartLine> SelectedLines { get; }
    }
}