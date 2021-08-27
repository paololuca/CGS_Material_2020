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
    /// Interaction logic for SemiFinals.xaml
    /// </summary>
    public partial class SemiFinals : UserControl, IFinalsPhase
    {
        private int _winnerPool = 1;
        private int _looserPool = 2;

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
            SavePool(idTorneo, idDisciplina, 1, dataGridPoolOne);
            SavePool(idTorneo, idDisciplina, 2, dataGridPoolTwo);
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

        private void SavePool(int idTorneo, int idDisciplina, int pool, DataGrid dataGridPool)
        {
            int posizione = 1;

            List<AtletaEliminatorie> listAtleti = new List<AtletaEliminatorie>();

            foreach (MatchEntity match in dataGridPool.Items)
            {
                AtletaEliminatorie winner = new AtletaEliminatorie();
                AtletaEliminatorie looser = new AtletaEliminatorie();

                winner.IdAtleta = (match.PuntiRosso > match.PuntiBlu) ? match.IdRosso : match.IdBlu;
                looser.IdAtleta = (match.PuntiRosso > match.PuntiBlu) ? match.IdBlu : match.IdRosso;

                DeleteOldValues(pool, idTorneo, idDisciplina, match.IdRosso);
                DeleteOldValues(pool, idTorneo, idDisciplina, match.IdBlu);

                SqlDal_Pools.UpdateSemifinali(idTorneo, idDisciplina, pool, posizione, match.IdRosso, match.PuntiRosso, match.PuntiBlu);
                SqlDal_Pools.UpdateSemifinali(idTorneo, idDisciplina, pool, posizione, match.IdBlu, match.PuntiBlu, match.PuntiRosso);

                CreateFinalsRecords(idTorneo, idDisciplina, posizione, listAtleti, winner, looser);
            }

            SqlDal_Pools.InsertSemifinali(listAtleti);
        }

        private void CreateFinalsRecords(int idTorneo, int idDisciplina, int posizione, List<AtletaEliminatorie> listAtleti, AtletaEliminatorie winner, AtletaEliminatorie looser)
        {
            winner.IdTorneo = idTorneo;
            winner.idDisciplina = idDisciplina;
            winner.Posizione = posizione;
            winner.Campo = _winnerPool;
            winner.PuntiFatti = 0;
            winner.PuntiSubiti = 0;

            listAtleti.Add(winner);
            listAtleti.Add(winner);
            listAtleti.Add(winner);

            looser.IdTorneo = idTorneo;
            looser.idDisciplina = idDisciplina;
            looser.Posizione = posizione;
            looser.Campo = _looserPool;
            looser.PuntiFatti = 0;
            looser.PuntiSubiti = 0;

            listAtleti.Add(looser);
            listAtleti.Add(looser);
            listAtleti.Add(looser);
        }

        private static void DeleteOldValues(int pool, int idTorneo, int idDisciplina, int fighterId)
        {
            SqlDal_Pools.EliminaSemifinaliByCampo(pool, idTorneo, idDisciplina, fighterId);
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
