using Caliburn.PresentationFramework.ApplicationModel;

namespace CompositeWpfDemo.Shell.Views.Presenters
{
    [View(typeof(IShellView))]
    public class ShellViewPresentationModel : MultiPresenter
    {
        public ShellViewPresentationModel()
        {
            DisplayName = "Shell View :)";
        }
    }
}