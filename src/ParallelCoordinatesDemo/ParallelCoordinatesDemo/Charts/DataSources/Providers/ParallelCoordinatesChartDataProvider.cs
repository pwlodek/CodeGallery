/// --------------------------------------------------------------------------------------
/// <copyright file="ParallelCoordinatesChartDataProvider.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the ParallelCoordinatesChartDataProvider class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace ParallelCoordinatesDemo.Charts.DataSources.Providers
{
    public class ParallelCoordinatesChartDataProvider : IChartDataProvider
    {
        public void AttachAxes(IChart chart, int dimension, string[] axisLabels)
        {
            const double margin = 0.0;
            const double spaceBetweenAxes = 200;

            var horizontalHelperAxis = new Axis();
            horizontalHelperAxis.IsHelper = true;
            horizontalHelperAxis.Orientation = AxisOrientation.Horizontal;
            horizontalHelperAxis.Dimension = dimension;
            horizontalHelperAxis.Label = axisLabels[dimension];
            horizontalHelperAxis.Scale = 1.0;

            for (int i = 0; i < dimension; i++)
            {
                var axis = new Axis();
                axis.Dimension = i;
                axis.Label = axisLabels[i];
                axis.Orientation = AxisOrientation.Vertical;
                axis.OriginalValue = i * spaceBetweenAxes + margin;

                horizontalHelperAxis.DependentAxes.Add(axis);

                chart.Axes.Add(axis);
            }

            horizontalHelperAxis.Transformation.Transform(horizontalHelperAxis, horizontalHelperAxis.DependentAxes);
            chart.Axes.Add(horizontalHelperAxis);
        }

        public void AttachDataSets(IChart chart, IEnumerable<MultiDimensionalPoint> points, int pointDimension, object[] tags)
        {
            int i = 0;
            foreach (var point in points)
            {
                AttachDataSet(chart, point, pointDimension, tags[i]);
                i++;
            }
        }

        public ChartLine AttachDataSet(IChart chart, MultiDimensionalPoint point, int pointDimension, object tag)
        {
            var chartPoints = new ChartPoint[pointDimension];
            var chartLineSegments = new ChartLineSegment[pointDimension - 1];

            for (int i = 0; i < pointDimension; i++)
            {
                chartPoints[i] = new ChartPoint { Data = point };
                chart.Axes[i].Points.Add(chartPoints[i]);
                chart.Axes[i].Transformation.Transform(chart.Axes[i], chartPoints[i]);
            }

            for (int i = 0; i < pointDimension - 1; i++)
            {
                chartLineSegments[i] = new ChartLineSegment(chartPoints[i], chartPoints[i + 1]);
            }

            foreach (var chartPoint in chartPoints)
            {
                chart.Points.Add(chartPoint);
            }

            var line = new ChartLine(chartLineSegments) { Tag = tag };
            chart.Lines.Add(line);

            return line;
        }
    }
}