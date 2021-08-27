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
    /// Interaction logic for Finals8.xaml
    /// </summary>
    public partial class Finals8 : UserControl, IFinalsPhase
    {

        private bool _loaded = false;
        PdfManager pdf;

        private List<AtletaEliminatorie> poolOne;
        private List<AtletaEliminatorie> poolTwo;
        private List<AtletaEliminatorie> poolThree;
        private List<AtletaEliminatorie> poolFour;

        public Finals8()
        {
            InitializeComponent();

            pdf = new PdfManager();
        }

        public void LoadFields(int idTorneo, int idDisciplina)
        {
            poolOne = SqlDal_Pools.GetOttavi(idTorneo, idDisciplina, 1);
            poolTwo = SqlDal_Pools.GetOttavi(idTorneo, idDisciplina, 2);
            poolThree = SqlDal_Pools.GetOttavi(idTorneo, idDisciplina, 3);
            poolFour = SqlDal_Pools.GetOttavi(idTorneo, idDisciplina, 4);

            LoadPool(poolOne, dataGridPoolOne);
            LoadPool(poolTwo, dataGridPoolTwo);
            LoadPool(poolThree, dataGridPoolThree);
            LoadPool(poolFour, dataGridPoolFour);

            _loaded = true;
        }

        public void SaveFields(int idTorneo, int idDisciplina)
        {
            DeleteOldValues(idTorneo, idDisciplina);

            SavePool(idTorneo, idDisciplina, 1, dataGridPoolOne);
            SavePool(idTorneo, idDisciplina, 2, dataGridPoolTwo);
            SavePool(idTorneo, idDisciplina, 3, dataGridPoolThree);
            SavePool(idTorneo, idDisciplina, 4, dataGridPoolFour);
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

        private void SavePool(int idTorneo, int idDisciplina, int pool, DataGrid dataGridPool)
        {
            int posizione = 1;

            List<AtletaEliminatorie> listAtleti = new List<AtletaEliminatorie>();

            foreach (MatchEntity match in dataGridPool.Items)
            {
                AtletaEliminatorie a = new AtletaEliminatorie();

                a.IdAtleta = (match.PuntiRosso > match.PuntiBlu) ? match.IdRosso : match.IdBlu;

                SqlDal_Pools.UpdateQualificati16(idTorneo, idDisciplina, pool, posizione, match.IdRosso, match.PuntiRosso, match.PuntiBlu);
                SqlDal_Pools.UpdateQualificati16(idTorneo, idDisciplina, pool, posizione, match.IdBlu, match.PuntiBlu, match.PuntiRosso);

                a.IdTorneo = idTorneo;
                a.idDisciplina = idDisciplina;
                a.Posizione = posizione;
                a.Campo = pool;
                a.PuntiFatti = 0;
                a.PuntiSubiti = 0;

                listAtleti.Add(a);

                posizione++;
            }

            SqlDal_Pools.InsertQuarti(listAtleti);
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
