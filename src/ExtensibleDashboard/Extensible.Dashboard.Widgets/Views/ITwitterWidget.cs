using Extensible.Dashboard.Contracts;
using Extensible.Dashboard.Widgets.Views.Presenters;

namespace Extensible.Dashboard.Widgets.Views
{
    public interface ITwitterWidget : IWidget
    {
        TwitterWidgetPresentationModel PresentationModel { get; set; }
    }
}