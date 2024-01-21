using BusinessEntity.DAO;
using BusinessEntity.Entity;
using Report;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    public partial class HemaRatingsTournamentResultReportByDiscipline : UserControl, ITournamentReportByDiscipline
    {
        private int _idTorneo;
        private int _idDisciplina;
        public string nomeDisciplina;

        public HemaRatingsTournamentResultReportByDiscipline(int idTorneo, int idDisciplina, string nomeDisciplina)
        {
            _idTorneo = idTorneo;
            _idDisciplina = idDisciplina;
            this.nomeDisciplina = nomeDisciplina;

            InitializeComponent();

            FullFillDatagrid();
        }


        private void FullFillDatagrid()
        {
            List<HemaRatingsClubEntity> clubsInvolved = SqlDal_HemaRatings.GetClubsInvolved(_idTorneo, _idDisciplina);
            if (clubsInvolved != null)
            {
                dataGridViewClubs.ItemsSource = clubsInvolved.ToArray();
                //lblTotal.Text = "Totale Incontri Gironi : " + risultatiGironi.Count;
            }

            List<HemaRatingsFighterMatchEntity> fightersInvolved = SqlDal_HemaRatings.GetFightersInvolved(_idTorneo, _idDisciplina);
            if (fightersInvolved != null)
                dataGridViewFighters.ItemsSource = fightersInvolved.ToArray();
            
            //List<OutputRisultatiEliminatorieTorneo> risultatiSedicesimi = SqlDal_Report.GetExportSedicesimiTorneo(_idTorneo, _idDisciplina);
            //if (risultatiSedicesimi != null)
            //    dataGridViewSedicesimi.ItemsSource = risultatiSedicesimi.ToArray();
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

        public void GenerateExcel(string tournamentName, string disciplineName, string path)
        {
            var fileNameWithPath = path + "\\" + tournamentName + "_" + disciplineName + ".xlsx";
            
            if (File.Exists(fileNameWithPath))
                File.Delete(fileNameWithPath);
            
            // Creating a Excel object. 
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            //try
            //{
            //    excel.SheetsInNewWorkbook = 7;

            //    workbook = excel.Workbooks.Add();

            //    worksheet = workbook.Worksheets[1];

            //    worksheet.Name = "Gironi";

            //    int cellRowIndex = 1;
            //    int cellColumnIndex = 1;

            //    #region Gironi
            //    //Loop through gironi

            //    worksheet.Cells[cellRowIndex, 1] = "IdRosso";
            //    worksheet.Cells[cellRowIndex, 2] = "CognomeRosso";
            //    worksheet.Cells[cellRowIndex, 3] = "NomeRosso";
            //    worksheet.Cells[cellRowIndex, 4] = "PuntiRosso";
            //    worksheet.Cells[cellRowIndex, 5] = "IdBlu";
            //    worksheet.Cells[cellRowIndex, 6] = "CognomeBlu";
            //    worksheet.Cells[cellRowIndex, 7] = "NomeBlu";
            //    worksheet.Cells[cellRowIndex, 8] = "PuntiBlu";
            //    worksheet.Cells[cellRowIndex, 9] = "Campo";
            //    worksheet.Cells[cellRowIndex, 10] = "DoppiaMorte";
            //    cellRowIndex++;

            //    foreach (OutputRisultatiTorneo r in dataGridViewGironi.Items)
            //    { 
            //        worksheet.Cells[cellRowIndex, 1] = r.IdRosso;
            //        worksheet.Cells[cellRowIndex, 2] = r.CognomeRosso;
            //        worksheet.Cells[cellRowIndex, 3] = r.NomeRosso;
            //        worksheet.Cells[cellRowIndex, 4] = r.PuntiRosso;

            //        worksheet.Cells[cellRowIndex, 5] = r.IdBlu;
            //        worksheet.Cells[cellRowIndex, 6] = r.CognomeBlu;
            //        worksheet.Cells[cellRowIndex, 7] = r.NomeBlu;
            //        worksheet.Cells[cellRowIndex, 8] = r.PuntiBlu;

            //        worksheet.Cells[cellRowIndex, 9] = r.Campo;
            //        worksheet.Cells[cellRowIndex, 10] = r.DoppiaMorte.ToString();


            //        cellRowIndex++;
            //    }
            //    #endregion

            //    #region Classifica post gironi
            //    worksheet = workbook.Sheets[2];
            //    worksheet.Name = "PostGironi";
            //    cellRowIndex = 1;
            //    cellColumnIndex = 1;
            //    //TODO
            //    //Loop through Sedicesimi

            //    worksheet.Cells[cellRowIndex, 1] = "IdGirone";
            //    worksheet.Cells[cellRowIndex, 2] = "IdAtleta";
            //    worksheet.Cells[cellRowIndex, 3] = "Cognome";
            //    worksheet.Cells[cellRowIndex, 4] = "Nome";
            //    worksheet.Cells[cellRowIndex, 5] = "Vittorie";
            //    worksheet.Cells[cellRowIndex, 6] = "Sconfitte";
            //    worksheet.Cells[cellRowIndex, 7] = "PuntiFatti";
            //    worksheet.Cells[cellRowIndex, 8] = "PuntiSubiti";
            //    worksheet.Cells[cellRowIndex, 9] = "Differenziale";
            //    worksheet.Cells[cellRowIndex, 10] = "Ranking";

            //    cellRowIndex++;
            //    foreach (GironiConclusi r in dataGridViewPostGironi.Items)
            //    {
            //        worksheet.Cells[cellRowIndex, 1] = r.IdGirone;
            //        worksheet.Cells[cellRowIndex, 2] = r.IdAtleta;
            //        worksheet.Cells[cellRowIndex, 3] = r.Cognome;
            //        worksheet.Cells[cellRowIndex, 4] = r.Nome;
            //        worksheet.Cells[cellRowIndex, 5] = r.Vittorie;
            //        worksheet.Cells[cellRowIndex, 6] = r.Sconfitte;
            //        worksheet.Cells[cellRowIndex, 7] = r.PuntiFatti;
            //        worksheet.Cells[cellRowIndex, 8] = r.PuntiSubiti;
            //        worksheet.Cells[cellRowIndex, 9] = r.Differenziale;
            //        worksheet.Cells[cellRowIndex, 10] = r.Ranking;
                    
            //        cellRowIndex++;
            //    }
            //    #endregion

            //    #region sedicesimi
            //    worksheet = workbook.Sheets[3];
            //    worksheet.Name = "Sedicesimi";
            //    cellRowIndex = 1;
            //    cellColumnIndex = 1;

            //    //Loop through Sedicesimi
            //    worksheet.Cells[cellRowIndex, 1] = "Posizione";
            //    worksheet.Cells[cellRowIndex, 2] = "IdAtleta";
            //    worksheet.Cells[cellRowIndex, 3] = "CognomeAtleta";
            //    worksheet.Cells[cellRowIndex, 4] = "NomeAtleta";
            //    worksheet.Cells[cellRowIndex, 5] = "PuntiFatti";
            //    worksheet.Cells[cellRowIndex, 6] = "PuntiSubiti";
            //    worksheet.Cells[cellRowIndex, 7] = "Campo";

            //    cellRowIndex++;

            //    foreach(OutputRisultatiEliminatorieTorneo r in dataGridViewSedicesimi.Items)
            //    {
            //        worksheet.Cells[cellRowIndex, 1] = r.Posizione;
            //        worksheet.Cells[cellRowIndex, 2] = r.IdAtleta;
            //        worksheet.Cells[cellRowIndex, 3] = r.CognomeAtleta;
            //        worksheet.Cells[cellRowIndex, 4] = r.NomeAtleta;
            //        worksheet.Cells[cellRowIndex, 5] = r.PuntiFatti;
            //        worksheet.Cells[cellRowIndex, 6] = r.PuntiSubiti;
            //        worksheet.Cells[cellRowIndex, 7] = r.Campo;
                       
            //        cellRowIndex++;
            //    }
            //    #endregion

            //    #region ottavi
            //    worksheet = workbook.Sheets[4];
            //    worksheet.Name = "Ottavi";
            //    cellRowIndex = 1;
            //    cellColumnIndex = 1;

            //    //Loop through Ottavi
            //    worksheet.Cells[cellRowIndex, 1] = "Posizione";
            //    worksheet.Cells[cellRowIndex, 2] = "IdAtleta";
            //    worksheet.Cells[cellRowIndex, 3] = "CognomeAtleta";
            //    worksheet.Cells[cellRowIndex, 4] = "NomeAtleta";
            //    worksheet.Cells[cellRowIndex, 5] = "PuntiFatti";
            //    worksheet.Cells[cellRowIndex, 6] = "PuntiSubiti";
            //    worksheet.Cells[cellRowIndex, 7] = "Campo";

            //    cellRowIndex++;

            //    foreach (OutputRisultatiEliminatorieTorneo r in dataGridViewOttavi.Items)
            //    {
            //        worksheet.Cells[cellRowIndex, 1] = r.Posizione;
            //        worksheet.Cells[cellRowIndex, 2] = r.IdAtleta;
            //        worksheet.Cells[cellRowIndex, 3] = r.CognomeAtleta;
            //        worksheet.Cells[cellRowIndex, 4] = r.NomeAtleta;
            //        worksheet.Cells[cellRowIndex, 5] = r.PuntiFatti;
            //        worksheet.Cells[cellRowIndex, 6] = r.PuntiSubiti;
            //        worksheet.Cells[cellRowIndex, 7] = r.Campo;

            //        cellRowIndex++;
            //    }
            //    #endregion

            //    #region Quarti
            //    worksheet = workbook.Sheets[5];
            //    worksheet.Name = "Quarti";
            //    cellRowIndex = 1;
            //    cellColumnIndex = 1;

            //    //Loop through Ottavi
            //    worksheet.Cells[cellRowIndex, 1] = "Posizione";
            //    worksheet.Cells[cellRowIndex, 2] = "IdAtleta";
            //    worksheet.Cells[cellRowIndex, 3] = "CognomeAtleta";
            //    worksheet.Cells[cellRowIndex, 4] = "NomeAtleta";
            //    worksheet.Cells[cellRowIndex, 5] = "PuntiFatti";
            //    worksheet.Cells[cellRowIndex, 6] = "PuntiSubiti";
            //    worksheet.Cells[cellRowIndex, 7] = "Campo";

            //    cellRowIndex++;
            //    foreach (OutputRisultatiEliminatorieTorneo r in dataGridViewQuarti.Items)
            //    {
            //        worksheet.Cells[cellRowIndex, 1] = r.Posizione;
            //        worksheet.Cells[cellRowIndex, 2] = r.IdAtleta;
            //        worksheet.Cells[cellRowIndex, 3] = r.CognomeAtleta;
            //        worksheet.Cells[cellRowIndex, 4] = r.NomeAtleta;
            //        worksheet.Cells[cellRowIndex, 5] = r.PuntiFatti;
            //        worksheet.Cells[cellRowIndex, 6] = r.PuntiSubiti;
            //        worksheet.Cells[cellRowIndex, 7] = r.Campo;

            //        cellRowIndex++;
            //    }
            //    #endregion

            //    #region Semifinali
            //    worksheet = workbook.Sheets[6];
            //    worksheet.Name = "Semifinali";
            //    cellRowIndex = 1;
            //    cellColumnIndex = 1;

            //    //Loop through Semifinali
            //    worksheet.Cells[cellRowIndex, 1] = "Posizione";
            //    worksheet.Cells[cellRowIndex, 2] = "IdAtleta";
            //    worksheet.Cells[cellRowIndex, 3] = "CognomeAtleta";
            //    worksheet.Cells[cellRowIndex, 4] = "NomeAtleta";
            //    worksheet.Cells[cellRowIndex, 5] = "PuntiFatti";
            //    worksheet.Cells[cellRowIndex, 6] = "PuntiSubiti";
            //    worksheet.Cells[cellRowIndex, 7] = "Campo";

            //    cellRowIndex++;

            //    foreach (OutputRisultatiEliminatorieTorneo r in dataGridViewSemifinali.Items)
            //    {
            //        worksheet.Cells[cellRowIndex, 1] = r.Posizione;
            //        worksheet.Cells[cellRowIndex, 2] = r.IdAtleta;
            //        worksheet.Cells[cellRowIndex, 3] = r.CognomeAtleta;
            //        worksheet.Cells[cellRowIndex, 4] = r.NomeAtleta;
            //        worksheet.Cells[cellRowIndex, 5] = r.PuntiFatti;
            //        worksheet.Cells[cellRowIndex, 6] = r.PuntiSubiti;
            //        worksheet.Cells[cellRowIndex, 7] = r.Campo;

            //        cellRowIndex++;
            //    }
            //    #endregion

            //    #region Finali
            //    worksheet = workbook.Sheets[7];
            //    worksheet.Name = "Finali";
            //    cellRowIndex = 1;
            //    cellColumnIndex = 1;

            //    //Loop through Finali 
            //    worksheet.Cells[cellRowIndex, 1] = "Posizione";
            //    worksheet.Cells[cellRowIndex, 2] = "IdAtleta";
            //    worksheet.Cells[cellRowIndex, 3] = "CognomeAtleta";
            //    worksheet.Cells[cellRowIndex, 4] = "NomeAtleta";
            //    worksheet.Cells[cellRowIndex, 5] = "PuntiFatti";
            //    worksheet.Cells[cellRowIndex, 6] = "PuntiSubiti";
            //    worksheet.Cells[cellRowIndex, 7] = "Campo";

            //    cellRowIndex++;

            //    foreach (OutputRisultatiEliminatorieTorneo r in dataGridViewFinali.Items)
            //    {
            //        worksheet.Cells[cellRowIndex, 1] = r.Posizione;
            //        worksheet.Cells[cellRowIndex, 2] = r.IdAtleta;
            //        worksheet.Cells[cellRowIndex, 3] = r.CognomeAtleta;
            //        worksheet.Cells[cellRowIndex, 4] = r.NomeAtleta;
            //        worksheet.Cells[cellRowIndex, 5] = r.PuntiFatti;
            //        worksheet.Cells[cellRowIndex, 6] = r.PuntiSubiti;
            //        worksheet.Cells[cellRowIndex, 7] = r.Campo;

            //        cellRowIndex++;
            //    }
            //    #endregion

            //    workbook.SaveAs(fileNameWithPath);
                
            //}
            //catch (System.Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    excel.Quit();
            //    workbook = null;
            //    excel = null;
            //}
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
