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
    /// Interaction logic for SemiFinals.xaml
    /// </summary>
    public partial class SemiFinals : UserControl, IFinalsPhase
    {

        private bool _loaded = false;
        PdfManager pdf;

        private List<AtletaEliminatorie> poolOne;
        private List<AtletaEliminatorie> poolTwo;


        public SemiFinals()
        {
            InitializeComponent();

            pdf = new PdfManager();
        }

        public void LoadFields(int idTorneo, int idDisciplina)
        {
            poolOne = SqlDal_Pools.GetSemifinali(idTorneo, idDisciplina, 1);
            poolTwo = SqlDal_Pools.GetSemifinali(idTorneo, idDisciplina, 2);

            LoadPool(poolOne, dataGridPoolOne);
            LoadPool(poolTwo, dataGridPoolTwo);

            _loaded = true;
        }

        public void SaveFields(int idTorneo, int idDisciplina)
        {
            AscEngine.SaveSemiFinalPool(idTorneo, idDisciplina, 1, dataGridPoolOne);
            AscEngine.SaveSemiFinalPool(idTorneo, idDisciplina, 2, dataGridPoolTwo);
        }

        public void PrintBracket()
        {
            //do noting
        }

        public void PrintPools()
        {
            if(_loaded)
                pdf.StampaSemifinali(poolOne, poolTwo);
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
