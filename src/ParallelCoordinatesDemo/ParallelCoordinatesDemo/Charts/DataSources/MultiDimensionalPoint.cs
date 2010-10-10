/// --------------------------------------------------------------------------------------
/// <copyright file="MultiDimensionalPoint.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the MultiDimensionalPoint class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ParallelCoordinatesDemo.Charts.DataSources
{
    [Serializable]
    public class MultiDimensionalPoint : INotifyPropertyChanged
    {
        private int m_Dimension;
        private readonly ObservableCollection<double> m_Values;
        private readonly ReadOnlyObservableCollection<double> m_ReadOnlyValues;

        public MultiDimensionalPoint(int dim)
        {
            Dimension = dim;
            m_Values = new ObservableCollection<double>();
            m_ReadOnlyValues = new ReadOnlyObservableCollection<double>(m_Values);

            for (int i = 0; i < Dimension; i++)
            {
                m_Values.Add(0.0);
            }
        }
        
        public int Dimension
        {
            get { return m_Dimension; }
            private set
            {
                m_Dimension = value;
                OnPropertyChanged("Dimension");
            }
        }

        public ReadOnlyObservableCollection<double> Values
        {
            get { return m_ReadOnlyValues; }
        }

        public double this[int index]
        {
            get { return m_Values[index]; }
            set
            {
                m_Values[index] = value;
                OnPropertyChanged("Values");
            }
        }

        #region INotifyPropertyChanged Implementation
        
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        } 

        #endregion
    }
}