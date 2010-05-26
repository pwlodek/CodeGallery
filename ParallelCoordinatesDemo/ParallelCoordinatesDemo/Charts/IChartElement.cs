/// --------------------------------------------------------------------------------------
/// <copyright file="IChartElement.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the IChartElement interface.
/// </summary>
/// --------------------------------------------------------------------------------------

namespace ParallelCoordinatesDemo.Charts
{
    public interface IChartElement
    {
        IChart Chart { get; set; }
    }
}