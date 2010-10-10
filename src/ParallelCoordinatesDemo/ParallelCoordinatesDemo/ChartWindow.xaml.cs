/// --------------------------------------------------------------------------------------
/// <copyright file="ChartWindow.cs">
///     Copyright (C) 2009 AGH University of Science and Technology, Krakow.
/// </copyright>
/// <authors>
///     Piotr Włodek mailto:piotr.wlodek@gmail.com
/// </authors>
/// <summary>
///     Defines the ChartWindow class.
/// </summary>
/// --------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Windows;

namespace ParallelCoordinatesDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ChartWindow : IChartWindow
    {
        public ChartWindow()
        {
            InitializeComponent();

            PresentationModel = new ChartWindowPresentationModel(this);
            PresentationModel.DataBind();
        }

        public ChartWindowPresentationModel PresentationModel
        {
            get { return (ChartWindowPresentationModel)DataContext; }
            set { DataContext = value; }
        }

        public void ShowAboutInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Parallel Coordinates Chart Demo\n");
            sb.AppendLine("Author: Piotr Włodek (mailto: piotr.wlodek@gmail.com)");
            sb.AppendLine("(C) 2009 AGH University of Science and Technology\n");
            sb.AppendLine("You can see my blog at http://pwlodek.blogspot.com/\n");
            sb.AppendLine("This code is a part of Mammoth Pattern Miner 2009");
            sb.AppendLine("See it at http://caribou.iisg.agh.edu.pl/proj/mammoth/");
            
            MessageBox.Show(sb.ToString(), "About...");
        }
    }
}
