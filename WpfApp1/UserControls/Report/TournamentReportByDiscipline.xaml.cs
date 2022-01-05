using BusinessEntity.DAO;
using Report;
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

namespace HEMATournamentSystem
{
    /// <summary>
    /// Interaction logic for TournamentReportByDiscipline.xaml
    /// </summary>
    public partial class TournamentReportByDiscipline : UserControl, ITournamentReportByDiscipline
    {
        private int _idTorneo;
        private int _idDisciplina;
        private string _nomeDisciplina;

        public TournamentReportByDiscipline(int idTorneo, int idDisciplina, string nomeDisciplina)
        {
            _idTorneo = idTorneo;
            _idDisciplina = idDisciplina;
            _nomeDisciplina = nomeDisciplina;

            InitializeComponent();

            FullFillDatagrid();
        }


        private void FullFillDatagrid()
        {
            List<OutputRisultatiTorneo> risultatiGironi = SqlDal_Report.GetExportGironiTorneo(_idTorneo, _idDisciplina);
            if (risultatiGironi != null)
            {
                dataGridViewGironi.ItemsSource = risultatiGironi.ToArray();
                //lblTotal.Text = "Totale Incontri Gironi : " + risultatiGironi.Count;
            }

            List<GironiConclusi> gironiConclusi = SqlDal_Pools.GetClassificaGironi(_idTorneo, _idDisciplina);
            if (gironiConclusi != null)
                dataGridViewPostGironi.ItemsSource = gironiConclusi.ToArray();

            List<OutputRisultatiEliminatorieTorneo> risultatiSedicesimi = SqlDal_Report.GetExportSedicesimiTorneo(_idTorneo, _idDisciplina);
            if (risultatiSedicesimi != null)
                dataGridViewSedicesimi.ItemsSource = risultatiSedicesimi.ToArray();

            List<OutputRisultatiEliminatorieTorneo> risultatiOttavi = SqlDal_Report.GetExportOttaviTorneo(_idTorneo, _idDisciplina);
            if (risultatiOttavi != null)
                dataGridViewOttavi.ItemsSource = risultatiOttavi.ToArray();

            List<OutputRisultatiEliminatorieTorneo> risultatiQuarti = SqlDal_Report.GetExportQuartiTorneo(_idTorneo, _idDisciplina);
            if (risultatiQuarti != null)
                dataGridViewQuarti.ItemsSource = risultatiQuarti.ToArray();

            List<OutputRisultatiEliminatorieTorneo> risultatiSemifinali = SqlDal_Report.GetExportSemifinaliTorneo(_idTorneo, _idDisciplina);
            if (risultatiSemifinali != null)
                dataGridViewSemifinali.ItemsSource = risultatiSemifinali.ToArray();

            List<OutputRisultatiEliminatorieTorneo> risultatiFinali = SqlDal_Report.GetExportFinaliTorneo(_idTorneo, _idDisciplina);
            if (risultatiFinali != null)
                dataGridViewFinali.ItemsSource = risultatiFinali.ToArray();

        }

        private void dataGridViewPostGironi_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            Style horizontalAlignment = new Style();
            horizontalAlignment.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));

            switch (e.Column.Header.ToString())
            {
                case "Qualificato":
                case "IdTorneo":
                case "IdDisciplina":
                case "Posizionamento":
                    e.Column.Visibility = Visibility.Hidden;
                    break;
                default:
                    e.Column.Visibility = Visibility.Visible;
                    //e.Column.IsReadOnly = true;
                    break;

            }
        }

        public int GenerateExcel(string tournamentName)
        {
            throw new NotImplementedException();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
