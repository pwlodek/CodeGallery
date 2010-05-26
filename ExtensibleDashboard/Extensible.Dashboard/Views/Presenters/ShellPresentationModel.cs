using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Extensible.Dashboard.Contracts;
using Extensible.Dashboard.Contracts.Common;

namespace Extensible.Dashboard.Views.Presenters
{
    [Export]
    public class ShellPresentationModel : BindableBase, IPartImportsSatisfiedNotification
    {
        private readonly IShellView m_View;

        [ImportingConstructor]
        public ShellPresentationModel(IShellView view)
        {
            m_View = view;
            m_View.PresentationModel = this;
        }

        [ImportMany(AllowRecomposition = true)]
        public IEnumerable<Lazy<IWidget, IWidgetMetadata>> Widgets { get; private set; }

        public IEnumerable<IWidget> LeftWidgets
        {
            get
            {
                return Widgets
                    .Where(t => t.Metadata.Location == WidgetLocation.Left)
                    .Select(t => t.Value);
            }
        }

        public IEnumerable<IWidget> RightWidgets
        {
            get
            {
                return Widgets
                    .Where(t => t.Metadata.Location == WidgetLocation.Right)
                    .Select(t => t.Value);
            }
        }

        public string DisplayName { get { return "Extensible Dashboard v1.0"; } }

        public void Run()
        {
            m_View.Show();
        }

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            OnPropertyChanged("LeftWidgets");
            OnPropertyChanged("RightWidgets");
        }
    }
}