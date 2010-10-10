using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Extensible.Dashboard.Widgets.Controllers;
using Extensible.Dashboard.Widgets.ViewModels;

namespace Extensible.Dashboard.Widgets.Views.Presenters
{
    [Export]
    public class TwitterWidgetPresentationModel
    {
        public ObservableCollection<Tweet> Tweets { get; private set; }

        [ImportingConstructor]
        public TwitterWidgetPresentationModel(ITweetsController tweetsController)
        {
            Tweets = new ObservableCollection<Tweet>();

            var tweets = tweetsController.GetTweetsForUser("@pwlodek");
            foreach (var tweet in tweets)
            {
                Tweets.Add(tweet);
            }
        }
    }
}