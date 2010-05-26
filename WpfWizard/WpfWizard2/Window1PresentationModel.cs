using System.ComponentModel;

namespace WpfWizard
{
    public class Window1PresentationModel : INotifyPropertyChanged
    {
        private bool m_OptionSelected;

        public bool OptionSelected
        {
            get { return m_OptionSelected; }
            set
            {
                m_OptionSelected = value;
                OnPropertyChanged("OptionChanged");
            }
        }

        protected virtual void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}