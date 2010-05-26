using System.Windows;
using System.Windows.Controls;

namespace TouchSamples.Controls
{
    public class ScatterViewItem : ContentControl
    {
        static ScatterViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScatterViewItem),
                new FrameworkPropertyMetadata(typeof(ScatterViewItem)));
        }
    }
}