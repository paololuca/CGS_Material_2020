using BusinessEntity.DAO;
using BusinessEntity.Entity;
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
        private int _idTorneo;
        private int _idDisciplina;

        private List<AtletaEliminatorie> campo1;
        private List<AtletaEliminatorie> campo2;
        private List<AtletaEliminatorie> campo3;
        private List<AtletaEliminatorie> campo4;

        public Finals16()
        {
            InitializeComponent();
        }

        public void LoadFields(int idTorneo, int idDisciplina)
        {
            _idTorneo = idTorneo;
            _idDisciplina = idDisciplina;

            LoadMatchs();

            btnBracket.IsEnabled = true;
        }
        
        public void SaveFields(int idTorneo, int idDisciplina)
        {
        }

        private void LoadMatchs()
        {
            List<AtletaEliminatorie> allAtleti = SqlDal_Pools.GetSedicesimi(_idTorneo, _idDisciplina);

            campo1 = new List<AtletaEliminatorie>();
            campo2 = new List<AtletaEliminatorie>();
            campo3 = new List<AtletaEliminatorie>();
            campo4 = new List<AtletaEliminatorie>();

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
            campo1.Add(allAtleti.ElementAt(0));
            campo1.Add(allAtleti.ElementAt(31));

            campo1.Add(allAtleti.ElementAt(15));
            campo1.Add(allAtleti.ElementAt(16));

            campo1.Add(allAtleti.ElementAt(11));
            campo1.Add(allAtleti.ElementAt(20));

            campo1.Add(allAtleti.ElementAt(7));
            campo1.Add(allAtleti.ElementAt(24));

            List<MatchEntity> list = new List<MatchEntity>();
            CreateMatches(list, campo1);

            dataGridPoolOne.ItemsSource = list;
        }

        private void LoadSecondPool(List<AtletaEliminatorie> allAtleti)
        {
            campo2.Add(allAtleti.ElementAt(1));
            campo2.Add(allAtleti.ElementAt(30));

            campo2.Add(allAtleti.ElementAt(14));
            campo2.Add(allAtleti.ElementAt(17));

            campo2.Add(allAtleti.ElementAt(10));
            campo2.Add(allAtleti.ElementAt(21));

            campo2.Add(allAtleti.ElementAt(6));
            campo2.Add(allAtleti.ElementAt(25));

            List<MatchEntity> list = new List<MatchEntity>();
            CreateMatches(list, campo2);

            dataGridPoolTwo.ItemsSource = list;
        }

        private void LoadThirdPool(List<AtletaEliminatorie> allAtleti)
        {
            campo3.Add(allAtleti.ElementAt(5));
            campo3.Add(allAtleti.ElementAt(26));

            campo3.Add(allAtleti.ElementAt(9));
            campo3.Add(allAtleti.ElementAt(22));

            campo3.Add(allAtleti.ElementAt(13));
            campo3.Add(allAtleti.ElementAt(18));

            campo3.Add(allAtleti.ElementAt(2));
            campo3.Add(allAtleti.ElementAt(29));

            List<MatchEntity> list = new List<MatchEntity>();
            CreateMatches(list, campo3);

            dataGridPoolThree.ItemsSource = list;
        }

        private void LoadFourthPool(List<AtletaEliminatorie> allAtleti)
        {
            campo4.Add(allAtleti.ElementAt(4));
            campo4.Add(allAtleti.ElementAt(27));

            campo4.Add(allAtleti.ElementAt(8));
            campo4.Add(allAtleti.ElementAt(23));

            campo4.Add(allAtleti.ElementAt(12));
            campo4.Add(allAtleti.ElementAt(19));

            campo4.Add(allAtleti.ElementAt(3));
            campo4.Add(allAtleti.ElementAt(28));

            List<MatchEntity> list = new List<MatchEntity>();
            CreateMatches(list, campo4);

            dataGridPoolFour.ItemsSource = list;
        }

        private void CreateMatches(List<MatchEntity> list, List<AtletaEliminatorie> campo)
        {
            list.Add(new MatchEntity()
            {
                IdRosso = campo[0].IdAtleta,
                CognomeRosso = campo[0].Cognome,
                NomeRosso = campo[0].Nome,
                PuntiRosso = 0,
                IdBlu = campo[1].IdAtleta,
                CognomeBlu = campo[1].Cognome,
                NomeBlu = campo[1].Nome,
                PuntiBlu = 0,
            }
            );

            list.Add(new MatchEntity()
            {
                IdRosso = campo[2].IdAtleta,
                CognomeRosso = campo[2].Cognome,
                NomeRosso = campo[2].Nome,
                PuntiRosso = 0,
                IdBlu = campo[3].IdAtleta,
                CognomeBlu = campo[3].Cognome,
                NomeBlu = campo[3].Nome,
                PuntiBlu = 0,
            }
            );

            list.Add(new MatchEntity()
            {
                IdRosso = campo[4].IdAtleta,
                CognomeRosso = campo[4].Cognome,
                NomeRosso = campo[4].Nome,
                PuntiRosso = 0,
                IdBlu = campo[5].IdAtleta,
                CognomeBlu = campo[5].Cognome,
                NomeBlu = campo[5].Nome,
                PuntiBlu = 0,
            }
            );

            list.Add(new MatchEntity()
            {
                IdRosso = campo[6].IdAtleta,
                CognomeRosso = campo[6].Cognome,
                NomeRosso = campo[6].Nome,
                PuntiRosso = 0,
                IdBlu = campo[7].IdAtleta,
                CognomeBlu = campo[7].Cognome,
                NomeBlu = campo[7].Nome,
                PuntiBlu = 0,
            }
            );
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
