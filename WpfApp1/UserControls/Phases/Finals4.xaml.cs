using BusinessEntity.DAO;
using BusinessEntity.Entity;
using HEMATournamentSystem.Engine;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<AtletaEliminatorie> poolOne = new List<AtletaEliminatorie>();
        private List<AtletaEliminatorie> poolTwo = new List<AtletaEliminatorie>();
        private List<AtletaEliminatorie> poolThree = new List<AtletaEliminatorie>();
        private List<AtletaEliminatorie> poolFour = new List<AtletaEliminatorie>();

        public Finals4()
        {
            InitializeComponent();

            pdf = new PdfManager();
        }

        public void LoadFields(int idTorneo, int idDisciplina)
        {

            var allAtleti = SqlDal_Pools.GetQuarti(idTorneo, idDisciplina);

            if (allAtleti.Select(x => x.Campo > 0).ToList().Count() == 0)
            {
                LoadAsFirstValidPhase(allAtleti);
            }
            else
            {
                poolOne = SqlDal_Pools.GetQuarti(idTorneo, idDisciplina, 1);
                poolTwo = SqlDal_Pools.GetQuarti(idTorneo, idDisciplina, 2);
                poolThree = SqlDal_Pools.GetQuarti(idTorneo, idDisciplina, 3);
                poolFour = SqlDal_Pools.GetQuarti(idTorneo, idDisciplina, 4);

                LoadPool(poolOne, dataGridPoolOne);
                LoadPool(poolTwo, dataGridPoolTwo);
                LoadPool(poolThree, dataGridPoolThree);
                LoadPool(poolFour, dataGridPoolFour);
            }

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

        private void LoadAsFirstValidPhase(List<AtletaEliminatorie> allAtleti)
        {
            #region campo1
            LoadFirstPool(allAtleti);
            #endregion

            #region campo2
            LoadSecondPool(allAtleti);
            #endregion

            #region campo3
            LoadThirdPool(allAtleti);
            #endregion

            #region campo4
            LoadFourthPool(allAtleti);
            #endregion
        }

        private void LoadFirstPool(List<AtletaEliminatorie> allAtleti)
        {
            poolOne.Add(allAtleti.ElementAt(0));
            poolOne.Add(allAtleti.ElementAt(7));

            LoadPool(poolOne, dataGridPoolOne);
        }

        private void LoadSecondPool(List<AtletaEliminatorie> allAtleti)
        {
            poolTwo.Add(allAtleti.ElementAt(1));
            poolTwo.Add(allAtleti.ElementAt(6));

            LoadPool(poolTwo, dataGridPoolTwo);
        }

        private void LoadThirdPool(List<AtletaEliminatorie> allAtleti)
        {
            poolThree.Add(allAtleti.ElementAt(2));
            poolThree.Add(allAtleti.ElementAt(5));

            LoadPool(poolThree, dataGridPoolThree);
        }

        private void LoadFourthPool(List<AtletaEliminatorie> allAtleti)
        {
            poolFour.Add(allAtleti.ElementAt(3));
            poolFour.Add(allAtleti.ElementAt(4));

            LoadPool(poolFour, dataGridPoolFour);
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
