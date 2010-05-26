/// --------------------------------------------------------------------------------------
/// <copyright file="LinearAxisTransformation.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the LinearAxisTransformation class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ParallelCoordinatesDemo.Charts.Transformations
{
    public class LinearAxisTransformation : AxisTransformationBase
    {
        public override void Transform(IAxis axis, ChartPoint point)
        {
            Debug.Assert(point.Axes.Contains(axis));

            if (axis.Orientation == AxisOrientation.Horizontal)
            {
                point.X = point.Data[axis.Dimension] * axis.Scale + axis.Translate;
                point.Y = axis.Value;
            }
            else
            {
                point.X = axis.Value;
                point.Y = point.Data[axis.Dimension] * axis.Scale + axis.Translate;
            }
        }

        public override void Transform(IAxis axis, IAxis dependentAxis)
        {
            dependentAxis.Value = dependentAxis.OriginalValue * axis.Scale + axis.Translate;
            Transform(dependentAxis, dependentAxis.Points);
        }

        public override void Scale(IAxis axis, int delta)
        {
            axis.Scale *= 1.0 + delta / 2000.0;
        }

        public override void Translate(IAxis axis, double change)
        {
            axis.Translate = change;
        }

        public override void Swap(IAxis axis)
        {
            double max = axis.Max;
            axis.Max = axis.Min;
            axis.Min = max;

            axis.Scale = -axis.Scale;
            axis.Translate = -axis.Translate;
            Transform(axis, axis.Points);
        }

        public override void SwapAxes(IAxis source, IAxis target)
        {
            // Swap points in line segments
            IList<ChartLineSegment> sourceXLineSegments = new List<ChartLineSegment>();
            IList<ChartLineSegment> sourceYLineSegments = new List<ChartLineSegment>();
            IList<ChartLineSegment> targetXLineSegments = new List<ChartLineSegment>();
            IList<ChartLineSegment> targetYLineSegments = new List<ChartLineSegment>();

            foreach (var chartPoint in source.Points)
            {
                foreach (var line in source.Chart.Lines)
                {
                    foreach (var segment in line.Segments)
                    {
                        if (segment.PointX == chartPoint)
                            sourceXLineSegments.Add(segment);

                        if (segment.PointY == chartPoint)
                            sourceYLineSegments.Add(segment);
                    }
                }
            }

            foreach (var chartPoint in target.Points)
            {
                foreach (var line in target.Chart.Lines)
                {
                    foreach (var segment in line.Segments)
                    {
                        if (segment.PointX == chartPoint)
                            targetXLineSegments.Add(segment);

                        if (segment.PointY == chartPoint)
                            targetYLineSegments.Add(segment);
                    }
                }
            }

            for (int i = 0; i < target.Points.Count; i++)
            {
                if (sourceXLineSegments.Count > 0)
                    sourceXLineSegments[i].PointX = target.Points[i];
                if (sourceYLineSegments.Count > 0)
                    sourceYLineSegments[i].PointY = target.Points[i];

                if (targetXLineSegments.Count > 0)
                    targetXLineSegments[i].PointX = source.Points[i];
                if (targetYLineSegments.Count > 0)
                    targetYLineSegments[i].PointY = source.Points[i];
            }

            double originalValue = source.OriginalValue;
            source.OriginalValue = target.OriginalValue;
            target.OriginalValue = originalValue;

            var helperAxis = target.Chart.Axes[target.Chart.Axes.Count - 1];
            helperAxis.Transformation.Transform(helperAxis, helperAxis.DependentAxes);
        }

        public override void FitToView(IChart chart, IAxis axis, double width, double height)
        {
            if (axis.Orientation == AxisOrientation.Vertical)
            {
                const double margin = 50;
                double min = double.MaxValue, max = double.MinValue;

                foreach (var chartPoint in chart.Points)
                {
                    if (chartPoint.Axes.Contains(axis))
                    {
                        if (chartPoint.Data[axis.Dimension] > max)
                            max = chartPoint.Data[axis.Dimension];

                        if (chartPoint.Data[axis.Dimension] < min)
                            min = chartPoint.Data[axis.Dimension];
                    }
                }

                double heightWithMargin = height - margin;
                double scale = 1.0;
                double translate;

                if (min != max)
                {
                    scale = heightWithMargin / (max - min);
                    translate = (heightWithMargin) / 2 + min * scale;
                }
                else
                {
                    translate = min * scale;
                }

                axis.Scale = -scale;
                axis.Translate = translate;

                Transform(axis, axis.Points);
            }
            else if (axis.Orientation == AxisOrientation.Horizontal)
            {
                const double margin = 50;
                double min = double.MaxValue, max = double.MinValue;

                foreach (var dependentAxis in axis.DependentAxes)
                {
                    if (dependentAxis.OriginalValue > max)
                        max = dependentAxis.OriginalValue;

                    if (dependentAxis.OriginalValue < min)
                        min = dependentAxis.OriginalValue;
                }

                double widthWithMargin = width - margin;
                double scale = 1.0;
                double translate = margin / 2;

                if (min != max)
                    scale = widthWithMargin / (max - min);

                axis.Scale = scale;
                axis.Translate = translate;

                foreach (var dependentAxis in axis.DependentAxes)
                {
                    Transform(axis, dependentAxis);
                }
            }
        }
    }
}