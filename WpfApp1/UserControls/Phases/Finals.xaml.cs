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
    /// Interaction logic for Finals.xaml
    /// </summary>
    public partial class Finals : UserControl, IFinalsPhase
    {
        private int _idTorneo;
        private int _idDisciplina;
        private bool _loaded = false;
        
        PdfManager pdf;

        private List<AtletaEliminatorie> poolOne;
        private List<AtletaEliminatorie> poolTwo;
        private string goldMedal;
        private string silverMedal;
        private string bronzeMedal;
        private string woodMedal;

        public Finals()
        {
            InitializeComponent();

            pdf = new PdfManager();
        }

        public void LoadFields(int idTorneo, int idDisciplina)
        {
            this._idTorneo = idTorneo;
            this._idDisciplina = idDisciplina;

            poolOne = SqlDal_Pools.GetFinali(idTorneo, idDisciplina, 1);
            poolTwo = SqlDal_Pools.GetFinali(idTorneo, idDisciplina, 2);

            LoadPool(poolOne, dataGridPoolOne);
            LoadPool(poolTwo, dataGridPoolTwo);

            if(poolOne.Select(x => x.PuntiFatti > 0).ToList().Count > 0 || poolTwo.Select(x => x.PuntiFatti > 0).ToList().Count > 0)
            { 
                btnPrintResult.IsEnabled = true;
                SaveFields(idTorneo, idDisciplina);
            }



            _loaded = true;
        }

        private void LoadPool(List<AtletaEliminatorie> pool, DataGrid dataGridPool)
        {
            if (pool.Count == 0)
                return;

            List<MatchEntity> list = new List<MatchEntity>();

            #region THREE match for finals
            list.Add(new MatchEntity()
            {
                IdRosso = pool[0].IdAtleta,
                CognomeRosso = pool[0].Cognome,
                NomeRosso = pool[0].Nome,
                PuntiRosso = pool[0].PuntiFatti,
                IdBlu = pool[3].IdAtleta,
                CognomeBlu = pool[3].Cognome,
                NomeBlu = pool[3].Nome,
                PuntiBlu = pool[3].PuntiFatti,
            }
            );

            list.Add(new MatchEntity()
            {
                IdRosso = pool[1].IdAtleta,
                CognomeRosso = pool[1].Cognome,
                NomeRosso = pool[1].Nome,
                PuntiRosso = pool[1].PuntiFatti,
                IdBlu = pool[4].IdAtleta,
                CognomeBlu = pool[4].Cognome,
                NomeBlu = pool[4].Nome,
                PuntiBlu = pool[4].PuntiFatti,
            }
            );

            list.Add(new MatchEntity()
            {
                IdRosso = pool[2].IdAtleta,
                CognomeRosso = pool[2].Cognome,
                NomeRosso = pool[2].Nome,
                PuntiRosso = pool[2].PuntiFatti,
                IdBlu = pool[5].IdAtleta,
                CognomeBlu = pool[5].Cognome,
                NomeBlu = pool[5].Nome,
                PuntiBlu = pool[5].PuntiFatti,
            }
            );

            #endregion
            
            dataGridPool.ItemsSource = list;
        }

        public void SaveFields(int idTorneo, int idDisciplina)
        {
            var pool1 = AscEngine.SaveFinalPool(idTorneo, idDisciplina, 1, dataGridPoolOne);
            var pool2 = AscEngine.SaveFinalPool(idTorneo, idDisciplina, 2, dataGridPoolTwo);

            goldMedal = pool1.Item1;
            silverMedal = pool1.Item2;

            bronzeMedal = pool2.Item1;
            woodMedal = pool2.Item2;


            btnPrintResult.IsEnabled = true;
        }

        public void PrintBracket()
        {
            //do nothing
        }

        public void PrintPools()
        {
            if(_loaded)
                pdf.StampaFinali(poolOne, poolTwo);
        }

        private void BtnPrintResult_Click(object sender, RoutedEventArgs e)
        {
            TorneoEntity tournament = SqlDal_Tournaments.GetTorneoById(_idTorneo);
            String nomeDisciplina = SqlDal_Tournaments.GetDisciplinaById(_idDisciplina);

            pdf.FineTorneo(goldMedal, silverMedal, bronzeMedal, woodMedal, tournament.Name, nomeDisciplina);
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
