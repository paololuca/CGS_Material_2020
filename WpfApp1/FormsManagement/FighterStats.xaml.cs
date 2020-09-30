using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media; // Pen
using BusinessEntity.DAO;
using Microsoft.Research.DynamicDataDisplay; // Core functionality
using Microsoft.Research.DynamicDataDisplay.DataSources; // EnumerableDataSource
using Microsoft.Research.DynamicDataDisplay.PointMarkers; // CirclePointMarker
using Resources;

namespace FormsManagement
{
    /// <summary>
    /// Interaction logic for FighterStats.xaml
    /// </summary>
    public partial class FighterStats : Window
    {
        public FighterStats()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<RankingByYear> rankingList = LoadBugInfo(54, 2);   //s&d per pielle

            DateTime[] dates = new DateTime[rankingList.Count];
            double[] punteggi = new double[rankingList.Count];
            double[] numberClosed = new double[rankingList.Count];

            for (int i = 0; i < rankingList.Count; ++i)
            {

                dates[i] = new DateTime(rankingList[i].Anno, i+1, 1);
                punteggi[i] = rankingList[i].Punteggio;
                //numberClosed[i] = rankingList[i].numberClosed;
            }

            var datesDataSource = new EnumerableDataSource<DateTime>(dates);
            datesDataSource.SetXMapping(x => dateAxis.ConvertToDouble(x));

            var numberOpenDataSource = new EnumerableDataSource<double>(punteggi);
            numberOpenDataSource.SetYMapping(y => y);

            //var numberClosedDataSource = new EnumerableDataSource<int>(numberClosed);
            //numberClosedDataSource.SetYMapping(y => y);

            CompositeDataSource compositeDataSource1 = new
              CompositeDataSource(datesDataSource, numberOpenDataSource);
            //CompositeDataSource compositeDataSource2 = new
            //  CompositeDataSource(datesDataSource, numberClosedDataSource);


            plotter.AddLineGraph(compositeDataSource1,
                new Pen(Brushes.Blue, 2),
                new CirclePointMarker { Size = 10.0, Fill = Brushes.Red },
                new PenDescription("Sword and Dagger"));

            //plotter.AddLineGraph(compositeDataSource2, 
            //    new Pen(Brushes.Green, 2),
            //    new TrianglePointMarker
            //    {
            //        Size = 10.0,
            //        Pen = new Pen(Brushes.Black, 2.0),
            //        Fill = Brushes.GreenYellow
            //    },
            //    new PenDescription("Number bugs closed"));

            plotter.Viewport.FitToView();

        } // Window1_Loaded()

        private static List<RankingByYear> LoadBugInfo(int idAtleta, int idDisciplina)
        {
            var result = FighterStatistics_DAL.GetFighterRankingByCategory(idAtleta, idDisciplina);

            return result;
        }

    } // class Window1

}
