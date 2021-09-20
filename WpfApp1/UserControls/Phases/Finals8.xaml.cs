using BusinessEntity.DAO;
using BusinessEntity.Entity;
using HEMATournamentSystem.Engine;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserControls.Phases
{
    /// <summary>
    /// Interaction logic for Finals8.xaml
    /// </summary>
    public partial class Finals8 : UserControl, IFinalsPhase
    {

        private bool _loaded = false;
        PdfManager pdf;

        private List<AtletaEliminatorie> poolOne = new List<AtletaEliminatorie>();
        private List<AtletaEliminatorie> poolTwo = new List<AtletaEliminatorie>();
        private List<AtletaEliminatorie> poolThree = new List<AtletaEliminatorie>();
        private List<AtletaEliminatorie> poolFour = new List<AtletaEliminatorie>();

        public Finals8()
        {
            InitializeComponent();

            pdf = new PdfManager();
        }

        public void LoadFields(int idTorneo, int idDisciplina)
        {
            var allAtleti = SqlDal_Pools.GetOttavi(idTorneo, idDisciplina);

            if (allAtleti.Select(x => x.Campo > 0).ToList().Count() == 0)
            {
                LoadAsFirstValidPhase(allAtleti);
            }
            else
            {
                poolOne = SqlDal_Pools.GetOttavi(idTorneo, idDisciplina, 1);
                poolTwo = SqlDal_Pools.GetOttavi(idTorneo, idDisciplina, 2);
                poolThree = SqlDal_Pools.GetOttavi(idTorneo, idDisciplina, 3);
                poolFour = SqlDal_Pools.GetOttavi(idTorneo, idDisciplina, 4);

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

            AscEngine.SaveFinal8Pool(idTorneo, idDisciplina, 1, dataGridPoolOne);
            AscEngine.SaveFinal8Pool(idTorneo, idDisciplina, 2, dataGridPoolTwo);
            AscEngine.SaveFinal8Pool(idTorneo, idDisciplina, 3, dataGridPoolThree);
            AscEngine.SaveFinal8Pool(idTorneo, idDisciplina, 4, dataGridPoolFour);
        }

        public void PrintBracket()
        {
            if (_loaded)
                pdf.StampaBracketOttavi(poolOne, poolTwo, poolThree, poolFour);
        }

        public void PrintPools()
        {
            if (_loaded)
                pdf.StampaOttavi(poolOne, poolTwo, poolThree, poolFour);
        }

        private void LoadAsFirstValidPhase(List<AtletaEliminatorie> allAtleti)
        {
            if (allAtleti.Count == 0)
                return;

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
            poolOne.Add(allAtleti.ElementAt(15));

            poolOne.Add(allAtleti.ElementAt(7));
            poolOne.Add(allAtleti.ElementAt(8));

            LoadPool(poolOne, dataGridPoolOne);
        }

        private void LoadSecondPool(List<AtletaEliminatorie> allAtleti)
        {
            poolTwo.Add(allAtleti.ElementAt(1));
            poolTwo.Add(allAtleti.ElementAt(14));

            poolTwo.Add(allAtleti.ElementAt(6));
            poolTwo.Add(allAtleti.ElementAt(9));

            LoadPool(poolTwo, dataGridPoolTwo);
        }

        private void LoadThirdPool(List<AtletaEliminatorie> allAtleti)
        {
            poolThree.Add(allAtleti.ElementAt(5));
            poolThree.Add(allAtleti.ElementAt(10));

            poolThree.Add(allAtleti.ElementAt(2));
            poolThree.Add(allAtleti.ElementAt(13));

            LoadPool(poolThree, dataGridPoolThree);
        }

        private void LoadFourthPool(List<AtletaEliminatorie> allAtleti)
        {
            poolFour.Add(allAtleti.ElementAt(4));
            poolFour.Add(allAtleti.ElementAt(11));

            poolFour.Add(allAtleti.ElementAt(3));
            poolFour.Add(allAtleti.ElementAt(12));

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

            list.Add(new MatchEntity()
            {
                IdRosso = pool[2].IdAtleta,
                CognomeRosso = pool[2].Cognome,
                NomeRosso = pool[2].Nome,
                PuntiRosso = pool[2].PuntiFatti,
                IdBlu = pool[3].IdAtleta,
                CognomeBlu = pool[3].Cognome,
                NomeBlu = pool[3].Nome,
                PuntiBlu = pool[3].PuntiFatti,
            }
            );

            dataGridPool.ItemsSource = list;

        }

        private static void DeleteOldValues(int idTorneo, int idDisciplina)
        {
            SqlDal_Pools.EliminaOttaviByCampo(1, idTorneo, idDisciplina);
            SqlDal_Pools.EliminaOttaviByCampo(2, idTorneo, idDisciplina);
            SqlDal_Pools.EliminaOttaviByCampo(3, idTorneo, idDisciplina);
            SqlDal_Pools.EliminaOttaviByCampo(4, idTorneo, idDisciplina);
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

        private void btnBracket_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
