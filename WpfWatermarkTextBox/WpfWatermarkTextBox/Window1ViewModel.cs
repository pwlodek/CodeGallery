using System.Collections.Generic;
using System.ComponentModel;

namespace WpfWatermarkTextBox
{
    public class Window1ViewModel : INotifyPropertyChanged
    {
        private IList<Fruit> m_Fruits;
        private Fruit m_SelectedFruit;

        public Window1ViewModel()
        {
            Fruits = new List<Fruit>
                         {
                             new Fruit { Name = "Oranges" },
                             new Fruit { Name = "Apples" },
                             new Fruit { Name = "Bananas" }
                         };
        }

        public IList<Fruit> Fruits
        {
            get { return m_Fruits; }
            set
            {
                m_Fruits = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Fruits"));
            }
        }

        public Fruit SelectedFruit
        {
            get { return m_SelectedFruit; }
            set
            {
                m_SelectedFruit = value;
                PropertyChanged(this, new PropertyChangedEventArgs("SelectedFruit"));
            }
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion
    }

    public class Fruit
    {
        public string Name { get; set; }
    }
}