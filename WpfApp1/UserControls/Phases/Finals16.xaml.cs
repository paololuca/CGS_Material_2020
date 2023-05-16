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
    /// Interaction logic for Finals16.xaml
    /// </summary>
    public partial class Finals16 : UserControl, IFinalsPhase
    {
        private bool _loaded = false;
        private PdfManager pdf;

        private List<AtletaEliminatorie> poolOne = new List<AtletaEliminatorie>();
        private List<AtletaEliminatorie> poolTwo = new List<AtletaEliminatorie>();
        private List<AtletaEliminatorie> poolThree = new List<AtletaEliminatorie>();
        private List<AtletaEliminatorie> poolFour = new List<AtletaEliminatorie>();
        
        public Finals16()
        {
            InitializeComponent();

            pdf = new PdfManager();
        }

        public void LoadFields(int idTorneo, int idDisciplina)
        {
            LoadMatchs(idTorneo, idDisciplina);

            _loaded = true;
        }

        public void SaveFields(int idTorneo, int idDisciplina)
        {
            //DeleteOldValues(idTorneo, idDisciplina);

            AscEngine.SaveFinal16Pool(idTorneo, idDisciplina, 1, dataGridPoolOne);
            AscEngine.SaveFinal16Pool(idTorneo, idDisciplina, 2, dataGridPoolTwo);
            AscEngine.SaveFinal16Pool(idTorneo, idDisciplina, 3, dataGridPoolThree);
            AscEngine.SaveFinal16Pool(idTorneo, idDisciplina, 4, dataGridPoolFour);
        }

        public void PrintBracket()
        {
            if (_loaded)
                pdf.StampaBracketSedicesimi(poolOne, poolTwo, poolThree, poolFour);
        }

        public void PrintPools()
        {
            if (_loaded)
                pdf.StampaSedicesimi(poolOne, poolTwo, poolThree, poolFour);
        }

        #region Private
        private void LoadMatchs(int idTorneo, int idDisciplina)
        {

            List<AtletaEliminatorie> allAtleti = SqlDal_Pools.GetSedicesimi(idTorneo, idDisciplina);

            poolOne = new List<AtletaEliminatorie>();
            poolTwo = new List<AtletaEliminatorie>();
            poolThree = new List<AtletaEliminatorie>();
            poolFour = new List<AtletaEliminatorie>();

            if (allAtleti.Count == 0)
                return;

            LoadMatchesByPool(allAtleti);
        }

        private void LoadMatchesByPool(List<AtletaEliminatorie> allAtleti)
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
            poolOne.Add(allAtleti.ElementAt(31));

            poolOne.Add(allAtleti.ElementAt(15));
            poolOne.Add(allAtleti.ElementAt(16));

            poolOne.Add(allAtleti.ElementAt(11));
            poolOne.Add(allAtleti.ElementAt(20));

            poolOne.Add(allAtleti.ElementAt(7));
            poolOne.Add(allAtleti.ElementAt(24));

            CreateMatches(dataGridPoolOne, poolOne);
        }

        private void LoadSecondPool(List<AtletaEliminatorie> allAtleti)
        {
            poolTwo.Add(allAtleti.ElementAt(1));
            poolTwo.Add(allAtleti.ElementAt(30));

            poolTwo.Add(allAtleti.ElementAt(14));
            poolTwo.Add(allAtleti.ElementAt(17));

            poolTwo.Add(allAtleti.ElementAt(10));
            poolTwo.Add(allAtleti.ElementAt(21));

            poolTwo.Add(allAtleti.ElementAt(6));
            poolTwo.Add(allAtleti.ElementAt(25));

            CreateMatches(dataGridPoolTwo, poolTwo);
        }

        private void LoadThirdPool(List<AtletaEliminatorie> allAtleti)
        {
            poolThree.Add(allAtleti.ElementAt(5));
            poolThree.Add(allAtleti.ElementAt(26));

            poolThree.Add(allAtleti.ElementAt(9));
            poolThree.Add(allAtleti.ElementAt(22));

            poolThree.Add(allAtleti.ElementAt(13));
            poolThree.Add(allAtleti.ElementAt(18));

            poolThree.Add(allAtleti.ElementAt(2));
            poolThree.Add(allAtleti.ElementAt(29));

            CreateMatches(dataGridPoolThree, poolThree);
        }

        private void LoadFourthPool(List<AtletaEliminatorie> allAtleti)
        {
            poolFour.Add(allAtleti.ElementAt(4));
            poolFour.Add(allAtleti.ElementAt(27));

            poolFour.Add(allAtleti.ElementAt(8));
            poolFour.Add(allAtleti.ElementAt(23));

            poolFour.Add(allAtleti.ElementAt(12));
            poolFour.Add(allAtleti.ElementAt(19));

            poolFour.Add(allAtleti.ElementAt(3));
            poolFour.Add(allAtleti.ElementAt(28));

            CreateMatches(dataGridPoolFour, poolFour);
        }

        private void CreateMatches(DataGrid dataGridPool, List<AtletaEliminatorie> pool)
        {
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

            list.Add(new MatchEntity()
            {
                IdRosso = pool[4].IdAtleta,
                CognomeRosso = pool[4].Cognome,
                NomeRosso = pool[4].Nome,
                PuntiRosso = pool[4].PuntiFatti,
                IdBlu = pool[5].IdAtleta,
                CognomeBlu = pool[5].Cognome,
                NomeBlu = pool[5].Nome,
                PuntiBlu = pool[5].PuntiFatti,
            }
            );

            list.Add(new MatchEntity()
            {
                IdRosso = pool[6].IdAtleta,
                CognomeRosso = pool[6].Cognome,
                NomeRosso = pool[6].Nome,
                PuntiRosso = pool[6].PuntiFatti,
                IdBlu = pool[7].IdAtleta,
                CognomeBlu = pool[7].Cognome,
                NomeBlu = pool[7].Nome,
                PuntiBlu = pool[7].PuntiFatti,
            }
            );

            dataGridPool.ItemsSource = list;
        }
      

        private void dataGridPool_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            Style horizontalAlignment = new Style();
            horizontalAlignment.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));


            switch (e.Column.Header.ToString())
            {
                case "M":
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

        #endregion
    }
}
