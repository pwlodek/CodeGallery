/// --------------------------------------------------------------------------------------
/// <copyright file="AxisTransformationBase.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the AxisTransformationBase class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace ParallelCoordinatesDemo.Charts.Transformations
{
    public abstract class AxisTransformationBase : IAxisTransformation
    {
        public virtual void Transform(IAxis axis, ChartLineSegment segment)
        {
            Transform(axis, segment.PointX);
            Transform(axis, segment.PointY);
        }

        public virtual void Transform(IAxis axis, ChartLine line)
        {
            if (line.Segments.Count > 0)
                Transform(axis, line.Segments[0].PointX);

            foreach (var segment in line.Segments)
            {
                Transform(axis, segment.PointY);
            }
        }

        public virtual void Transform(IAxis axis, IEnumerable<ChartLine> lines)
        {
            foreach (var line in lines)
            {
                Transform(axis, line);
            }
        }

        public virtual void Transform(IAxis axis, IEnumerable<ChartPoint> points)
        {
            foreach (var point in points)
            {
                Transform(axis, point);
            }
        }

        public virtual void Transform(IAxis axis, IEnumerable<IAxis> dependentAxes)
        {
            foreach (var ax in dependentAxes)
            {
                Transform(axis, ax);
            }
        }

        public abstract void Transform(IAxis axis, ChartPoint point);
        public abstract void Transform(IAxis axis, IAxis dependentAxis);

        public abstract void Scale(IAxis axis, int delta);
        public abstract void Translate(IAxis axis, double change);
        public abstract void Swap(IAxis axis);
        public abstract void SwapAxes(IAxis source, IAxis target);

        public abstract void FitToView(IChart chart, IAxis axis, double width, double height);
    }
}