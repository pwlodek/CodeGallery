/// --------------------------------------------------------------------------------------
/// <copyright file="IAxisTransformation.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the IAxisTransformation interface.
/// </summary>
/// --------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace ParallelCoordinatesDemo.Charts.Transformations
{
    public interface IAxisTransformation
    {
        void Transform(IAxis axis, ChartPoint point);
        void Transform(IAxis axis, IEnumerable<ChartPoint> points);
        void Transform(IAxis axis, ChartLineSegment segment);
        void Transform(IAxis axis, ChartLine line);
        void Transform(IAxis axis, IEnumerable<ChartLine> lines);
        void Transform(IAxis axis, IAxis dependentAxis);
        void Transform(IAxis axis, IEnumerable<IAxis> dependentAxes);

        void Scale(IAxis axis, int delta);
        void Translate(IAxis axis, double change);
        void Swap(IAxis axis);
        void SwapAxes(IAxis source, IAxis target);
        void FitToView(IChart chart, IAxis axis, double width, double height);
    }
}