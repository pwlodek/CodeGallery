/// --------------------------------------------------------------------------------------
/// <copyright file="ChartWindowPresentationModel.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr W³odek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the ChartWindowPresentationModel class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using ParallelCoordinatesDemo.Charts.DataSources;

namespace ParallelCoordinatesDemo
{
    public class ChartWindowPresentationModel : INotifyPropertyChanged
    {
        private readonly IChartWindow m_View;
        private readonly Random m_Random = new Random();
        private IChartDataSource m_DataSource;
        private int m_ObjectsCount;

        public RelayCommand<string> GenerateDiagramDataCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }
        public RelayCommand AboutCommand { get; private set; }

        public ChartWindowPresentationModel(IChartWindow view)
        {
            m_View = view;

            GenerateDiagramDataCommand = new RelayCommand<string>(ExecuteGenerateDiagramDataCommand);
            CloseCommand = new RelayCommand(ExecuteCloseCommand);
            AboutCommand = new RelayCommand(ExecuteAboutCommand);

            // Setup defaults
            ObjectsCount = 30;
        }

        public IChartDataSource DataSource
        {
            get { return m_DataSource; }
            private set
            {
                m_DataSource = value;
                OnPropertyChanged("DataSource");
            }
        }

        public int ObjectsCount
        {
            get { return m_ObjectsCount; }
            set
            {
                m_ObjectsCount = value;
                OnPropertyChanged("ObjectsCount");
            }
        }

        public void DataBind()
        {
            IList<DemoInfo> infos = new List<DemoInfo>();

            for (int i = 0; i < ObjectsCount; i++)
            {
                var x = new DemoInfo();
                x.X = m_Random.NextDouble() * 400 - 100;
                x.Y = m_Random.NextDouble() * 500 - 100;
                x.Z = m_Random.NextDouble() * 600 - 300;
                x.V = m_Random.NextDouble() * 800 - 100;
                x.K = 1.0;
                //x.M = i % 2 == 0 ? 1.0 : -20.0;
                x.M = i;
                x.Tag = i + 1;

                infos.Add(x);
            }
            //infos.Add(new DemoInfo { X = 45, Y = 67, Z = 56, V = 8000, K = 1, M = -50});

            var dataSource = new MultiDimensionalDataSource<DemoInfo>(infos, 6);
            dataSource.MapDimension(0, info => info.X);
            dataSource.MapDimension(1, info => info.Y);
            dataSource.MapDimension(2, info => info.Z);
            dataSource.MapDimension(3, info => info.V);
            dataSource.MapDimension(4, info => info.K);
            dataSource.MapDimension(5, info => info.M);

            //dataSource.MapDimensionToOpacity(0, 0.5);
            dataSource.MapTag(info => info.Tag);

            dataSource.Labels[0] = "X";
            dataSource.Labels[1] = "Y";
            dataSource.Labels[2] = "Z";
            dataSource.Labels[3] = "V";
            dataSource.Labels[4] = "K";
            dataSource.Labels[5] = "M";
            dataSource.HelperAxisLabel = "Helper axis";

            DataSource = dataSource;
        }

        #region Commands

        private void ExecuteGenerateDiagramDataCommand(string objects)
        {
            ObjectsCount = int.Parse(objects);
            DataBind();
        }

        private static void ExecuteCloseCommand(object arg)
        {
            Application.Current.Shutdown();
        }

        private void ExecuteAboutCommand(object arg)
        {
            m_View.ShowAboutInfo();
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
    }

    public class DemoInfo
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double V { get; set; }
        public double K { get; set; }
        public double M { get; set; }
        public int Tag { get; set; }
    }
}