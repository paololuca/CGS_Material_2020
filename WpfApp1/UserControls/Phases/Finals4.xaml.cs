using BusinessEntity.DAO;
using BusinessEntity.Entity;
using HEMATournamentSystem.Engine;
using Resources;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace UserControls.Phases
{
    /// <summary>
    /// Interaction logic for Finals4.xaml
    /// </summary>
    public partial class Finals4 : UserControl, IFinalsPhase
    {

        private bool _loaded = false;
        PdfManager pdf;

        private List<AtletaEliminatorie> poolOne;
        private List<AtletaEliminatorie> poolTwo;
        private List<AtletaEliminatorie> poolThree;
        private List<AtletaEliminatorie> poolFour;

        public Finals4()
        {
            InitializeComponent();

            pdf = new PdfManager();
        }

        public void LoadFields(int idTorneo, int idDisciplina)
        {
            poolOne = SqlDal_Pools.GetQuarti(idTorneo, idDisciplina, 1);
            poolTwo = SqlDal_Pools.GetQuarti(idTorneo, idDisciplina, 2);
            poolThree = SqlDal_Pools.GetQuarti(idTorneo, idDisciplina, 3);
            poolFour = SqlDal_Pools.GetQuarti(idTorneo, idDisciplina, 4);

            LoadPool(poolOne, dataGridPoolOne);
            LoadPool(poolTwo, dataGridPoolTwo);
            LoadPool(poolThree, dataGridPoolThree);
            LoadPool(poolFour, dataGridPoolFour);

            _loaded = true;
        }

        

        public void SaveFields(int idTorneo, int idDisciplina)
        {
            //DeleteOldValues(idTorneo, idDisciplina);

            AscEngine.SaveFinal4Pool(idTorneo, idDisciplina, 1, 1, dataGridPoolOne);
            AscEngine.SaveFinal4Pool(idTorneo, idDisciplina, 2, 2, dataGridPoolTwo);
            AscEngine.SaveFinal4Pool(idTorneo, idDisciplina, 3, 2, dataGridPoolThree);
            AscEngine.SaveFinal4Pool(idTorneo, idDisciplina, 4, 1, dataGridPoolFour);
        }

        public void PrintBracket()
        {
            //do nothing
        }

        public void PrintPools()
        {
            if(_loaded)
                pdf.StampaQuarti(poolOne, poolTwo, poolThree, poolFour);
        }

        private void LoadPool(List<AtletaEliminatorie> pool, DataGrid dataGridPool)
        {
            if (pool.Count == 0)
                return;

            List<MatchEntity> list = new List<MatchEntity>();

            list.Add(new MatchEntity()
            {
                IdRosso = pool[0].IdAtleta,
                CognomeRosso = pool[0].Cognome,
                NomeRosso = pool[0].Nome,
                PuntiRosso = pool[0].PuntiFatti,
                IdBlu = pool[1].IdAtleta,
                CognomeBlu = pool[1].Cognome,
                NomeBlu = pool[1].Nome,
                PuntiBlu = pool[1].PuntiFatti,
            }
            );

            dataGridPool.ItemsSource = list;

        }
        private static void DeleteOldValues(int idTorneo, int idDisciplina)
        {
            SqlDal_Pools.EliminaQuartiByCampo(1, idTorneo, idDisciplina);
            SqlDal_Pools.EliminaQuartiByCampo(2, idTorneo, idDisciplina);
            SqlDal_Pools.EliminaQuartiByCampo(3, idTorneo, idDisciplina);
            SqlDal_Pools.EliminaQuartiByCampo(4, idTorneo, idDisciplina);
        }

        private void dataGridPool_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            Style horizontalAlignment = new Style();
            horizontalAlignment.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));


            switch (e.Column.Header.ToString())
            {
                case "IdBlu":
                case "IdRosso":
                case "SatrapiaRosso":
                case "SatrapiaBlu":
                case "DoppiaMorte":
                    e.Column.CanUserSort = false;
                    e.Column.Visibility = Visibility.Hidden;
                    break;
                case "PuntiRosso":
                case "PuntiBlu":
                    e.Column.CanUserSort = false;
                    e.Column.Visibility = Visibility.Visible;
                    e.Column.CellStyle = horizontalAlignment;
                    break;
                default:
                    e.Column.CanUserSort = false;
                    e.Column.Visibility = Visibility.Visible;
                    e.Column.IsReadOnly = true;
                    break;
            }
        }
    }
}
