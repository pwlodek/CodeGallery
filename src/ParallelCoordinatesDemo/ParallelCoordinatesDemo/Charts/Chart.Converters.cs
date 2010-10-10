/// --------------------------------------------------------------------------------------
/// <copyright file="Chart.Converters.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr Włodek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the converters used by the chart.
/// </summary>
/// --------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Windows.Data;

namespace ParallelCoordinatesDemo.Charts
{
    public class AxisTransformMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var point = (string)parameter;
            var orientation = (AxisOrientation)values[0];
            var translate = (double)values[1];

            if ((point == "X" && orientation == AxisOrientation.Vertical) ||
                (point == "Y" && orientation == AxisOrientation.Horizontal))
                return translate;

            return 0.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class ChartPointInfoValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var point = (ChartPoint) value;
            if (point.Axes.Count == 1)
            {
                return point.Data.Values[point.Axes[0].Dimension].ToString("0.000");
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
