/// --------------------------------------------------------------------------------------
/// <copyright file="MultiDimensionalDataSource.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the MultiDimensionalDataSource class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using ParallelCoordinatesDemo.Charts.DataSources.Providers;

namespace ParallelCoordinatesDemo.Charts.DataSources
{
    public class MultiDimensionalDataSource<T> : DataSourceBase
    {
        public IEnumerable<T> SourceCollection { get; private set; }
        public ICollection<MultiDimensionalPoint> Data { get; private set; }
        public int Dimension { get; private set; }
        public IDictionary<int, string> Labels { get; private set; }
        public string HelperAxisLabel { get; set; }

        // Dimension to opacity mapping
        private int? m_OpacityDimension;
        private double m_MinimumOpacity;

        // Value mapping
        private readonly Func<T, double>[] m_Mappings;

        // Tag mapping
        private Func<T, object> m_TagMapping;

        public MultiDimensionalDataSource(IEnumerable<T> dataSource, int dimension)
        {
            SourceCollection = dataSource;
            Dimension = dimension;
            Labels = new Dictionary<int, string>();
            HelperAxisLabel = "Helper Axis";

            m_Mappings = new Func<T, double>[dimension];
        }

        public void MapDimension(int dimension, Func<T, double> filter)
        {
            m_Mappings[dimension] = filter;
        }

        public void MapDimensionToOpacity(int dimension, double minimumOpacity)
        {
            m_OpacityDimension = dimension;
            m_MinimumOpacity = minimumOpacity;
        }

        public void MapTag(Func<T, object> tagMapping)
        {
            m_TagMapping = tagMapping;
        }

        internal override void Process(IChart chart)
        {
            Data = new List<MultiDimensionalPoint>();
            var tags = new List<object>();
            var min = double.MaxValue;
            var max = double.MinValue;

            foreach (var obj in SourceCollection)
            {
                var point = new MultiDimensionalPoint(Dimension);

                for (int i = 0; i < Dimension; i++)
                {
                    point[i] = m_Mappings[i](obj);
                }

                if (m_TagMapping != null)
                    tags.Add(m_TagMapping(obj));
                else
                    tags.Add(null);

                Data.Add(point);
            }

            if (m_OpacityDimension.HasValue)
            {
                foreach (var multiDimensionalPoint in Data)
                {
                    if (multiDimensionalPoint[m_OpacityDimension.Value] < min)
                        min = multiDimensionalPoint[m_OpacityDimension.Value];

                    if (multiDimensionalPoint[m_OpacityDimension.Value] > max)
                        max = multiDimensionalPoint[m_OpacityDimension.Value];
                }
            }

            var chartDataProvider = new ParallelCoordinatesChartDataProvider();

            if (chart.AutoGenerateAxes)
            {
                var labels = new string[Dimension + 1];
                for (int i = 0; i < Dimension; i++)
                {
                    labels[i] = "Axis " + i;
                    if (Labels.ContainsKey(i))
                        labels[i] = Labels[i];
                }
                labels[Dimension] = HelperAxisLabel;
                chartDataProvider.AttachAxes(chart, Dimension, labels);
            }

            if (m_OpacityDimension.HasValue)
            {
                int i = 0;
                foreach (var multiDimensionalPoint in Data)
                {
                    ChartLine line = chartDataProvider.AttachDataSet(chart, multiDimensionalPoint, Dimension, tags[i]);
                    line.Opacity = m_MinimumOpacity + (1.0 - m_MinimumOpacity) *
                                                      ((multiDimensionalPoint[m_OpacityDimension.Value] - min) / (max - min));
                    i++;
                }
            }
            else
            {
                chartDataProvider.AttachDataSets(chart, Data, Dimension, tags.ToArray());
            }
        }
    }
}