using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media; // Pen
using BusinessEntity.DAO;
using Microsoft.Research.DynamicDataDisplay; // Core functionality
using Microsoft.Research.DynamicDataDisplay.DataSources; // EnumerableDataSource
using Microsoft.Research.DynamicDataDisplay.PointMarkers; // CirclePointMarker
using Resources;

namespace HEMATournamentSystem
{
    /// <summary>
    /// Interaction logic for FighterStats.xaml
    /// </summary>
    public partial class FighterStats : Window
    {
        private int idFighter;

        public FighterStats(int idFighter)
        {
            this.idFighter = idFighter;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillSideSword();
            FillSwordAndDagger();

        } // Window1_Loaded()

        private void FillSideSword()
        {
            List<RankingByYear> rankingList = LoadUserInfo(idFighter, 1);

            CompositeDataSource compositeDataSource = CreateCompositeDataSource(rankingList);

            sideSwordPlotter.AddLineGraph(compositeDataSource,
                new Pen(Brushes.Blue, 2),
                new CirclePointMarker { Size = 10.0, Fill = Brushes.Red },
                new PenDescription("Sidesword"));

            sideSwordPlotter.Viewport.FitToView();
        }

        private void FillSwordAndDagger()
        {
            List<RankingByYear> rankingList = LoadUserInfo(idFighter, 2);   //s&d per pielle

            CompositeDataSource compositeDataSource = CreateCompositeDataSource(rankingList);

            swordAndDaggerPlotter.AddLineGraph(compositeDataSource,
                new Pen(Brushes.Blue, 2),
                new CirclePointMarker { Size = 10.0, Fill = Brushes.Red },
                new PenDescription("Sword and Dagger"));

            swordAndDaggerPlotter.Viewport.FitToView();
        }

        private CompositeDataSource CreateCompositeDataSource(List<RankingByYear> rankingList)
        {
            DateTime[] dates = new DateTime[rankingList.Count];
            double[] punteggi = new double[rankingList.Count];
            double[] numberClosed = new double[rankingList.Count];

            for (int i = 0; i < rankingList.Count; ++i)
            {
                dates[i] = rankingList[i].InsertedDate;
                punteggi[i] = rankingList[i].Punteggio;
            }

            var datesDataSource = new EnumerableDataSource<DateTime>(dates);
            datesDataSource.SetXMapping(x => dateAxis.ConvertToDouble(x));

            var numberOpenDataSource = new EnumerableDataSource<double>(punteggi);
            numberOpenDataSource.SetYMapping(y => y);

            CompositeDataSource compositeDataSource1 = new
              CompositeDataSource(datesDataSource, numberOpenDataSource);
            //CompositeDataSource compositeDataSource2 = new
            //  CompositeDataSource(datesDataSource, numberClosedDataSource);
            return compositeDataSource1;
        }

        private static List<RankingByYear> LoadUserInfo(int idAtleta, int idDisciplina)
        {
            var result = SqlDal_FighterStatistics.GetFighterRankingByCategory(idAtleta, idDisciplina);

            return result;
        }

    } // class Window1

}
