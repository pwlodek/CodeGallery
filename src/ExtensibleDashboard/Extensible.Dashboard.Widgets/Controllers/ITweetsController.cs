using System.Collections.Generic;
using Extensible.Dashboard.Widgets.ViewModels;

namespace Extensible.Dashboard.Widgets.Controllers
{
    public interface ITweetsController
    {
        IEnumerable<Tweet> GetTweetsForUser(string userName);
    }
}