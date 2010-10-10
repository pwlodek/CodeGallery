using Extensible.Dashboard.Views.Presenters;

namespace Extensible.Dashboard.Views
{
    public interface IShellView
    {
        ShellPresentationModel PresentationModel { get; set; }

        void Show();
    }
}