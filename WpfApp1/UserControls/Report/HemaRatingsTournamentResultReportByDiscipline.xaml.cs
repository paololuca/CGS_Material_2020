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

            List<HemaRatingsMatchResult> matches = SqlDal_HemaRatings.GetPoolsResults(_idTorneo, _idDisciplina);

            matches.AddRange(SqlDal_HemaRatings.Get16FinalsResults(_idTorneo, _idDisciplina));
            matches.AddRange(SqlDal_HemaRatings.Get8FinalsResults(_idTorneo, _idDisciplina));
            matches.AddRange(SqlDal_HemaRatings.Get4FinalsResults(_idTorneo, _idDisciplina));
            matches.AddRange(SqlDal_HemaRatings.GetSemiFinalsResults(_idTorneo, _idDisciplina));
            matches.AddRange(SqlDal_HemaRatings.GetFinalsResults(_idTorneo, _idDisciplina));

            
            if (matches != null)
                dataGridViewMatchs.ItemsSource = matches.ToArray();
        }

        private void dataGridViewClubs_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            Style horizontalAlignment = new Style();
            horizontalAlignment.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));

            switch (e.Column.Header.ToString())
            {
                case "Id":
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
            var fileNameWithPath = path + "\\HemaRating_" + tournamentName + "_" + disciplineName + ".xlsx";
            
            if (File.Exists(fileNameWithPath))
                File.Delete(fileNameWithPath);
            
            // Creating a Excel object. 
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {
                excel.SheetsInNewWorkbook = 4;

                workbook = excel.Workbooks.Add();

                worksheet = workbook.Worksheets[1];

                worksheet.Name = "Event Info";

                int cellRowIndex = 1;
                int cellColumnIndex = 1;

                #region Event info
                //Loop through gironi

                worksheet.Cells[1, 1] = "Event info";
                worksheet.Cells[1, 2] = "Fill out here";
                worksheet.Cells[1, 3] = "Example";
                worksheet.Cells[1, 4] = "Required?";
                worksheet.Cells[1, 5] = "Description/explanation";

                worksheet.Cells[2, 1] = "Event name";
                worksheet.Cells[2, 2] = "";
                worksheet.Cells[2, 3] = "";
                worksheet.Cells[2, 4] = "Yes";
                worksheet.Cells[2, 5] = "The date of the event. If the event went over several days, the first day of the event";

                worksheet.Cells[3, 1] = "Date";
                worksheet.Cells[3, 2] = "";
                worksheet.Cells[3, 3] = "";
                worksheet.Cells[3, 4] = "Yes";
                worksheet.Cells[3, 5] = "";

                worksheet.Cells[4, 1] = "Country";
                worksheet.Cells[4, 2] = "";
                worksheet.Cells[4, 3] = "";
                worksheet.Cells[4, 4] = "Yes";
                worksheet.Cells[4, 5] = "";

                worksheet.Cells[5, 1] = "State (If applicable)";
                worksheet.Cells[5, 2] = "";
                worksheet.Cells[5, 3] = "";
                worksheet.Cells[5, 4] = "No";
                worksheet.Cells[5, 5] = "";

                worksheet.Cells[6, 1] = "City";
                worksheet.Cells[6, 2] = "";
                worksheet.Cells[6, 3] = "";
                worksheet.Cells[6, 4] = "Yes";
                worksheet.Cells[6, 5] = "";

                worksheet.Cells[7, 1] = "Organizing club (if any)";
                worksheet.Cells[7, 2] = "";
                worksheet.Cells[7, 3] = "";
                worksheet.Cells[7, 4] = "No";
                worksheet.Cells[7, 5] = "";

                worksheet.Cells[8, 1] = "Social media links";
                worksheet.Cells[8, 2] = "";
                worksheet.Cells[8, 3] = "";
                worksheet.Cells[8, 4] = "Yes";
                worksheet.Cells[8, 5] = "One or more links to info about the event on Facebook, VK, Eventbrite, etc. This makes it easier to tag, find photos, etc. If this is missing, please write an explanation why.";

                worksheet.Cells[9, 1] = "Photo links";
                worksheet.Cells[9, 2] = "";
                worksheet.Cells[9, 3] = "";
                worksheet.Cells[9, 4] = "Yes";
                worksheet.Cells[9, 5] = "Links to photos/albums we can use when announcing that we’ve added the results to the ratings. These can also be submitted to us directly via email or Facebook message. If this is missing, please write an explanation why.";

                worksheet.Cells[10, 1] = "";
                worksheet.Cells[10, 2] = "";
                worksheet.Cells[10, 3] = "";
                worksheet.Cells[10, 4] = "";
                worksheet.Cells[10, 5] = "";

                worksheet.Cells[11, 1] = "Submitter info";
                worksheet.Cells[11, 2] = "";
                worksheet.Cells[11, 3] = "";
                worksheet.Cells[11, 4] = "";
                worksheet.Cells[11, 5] = "";

                worksheet.Cells[12, 1] = "Submitter";
                worksheet.Cells[12, 2] = "";
                worksheet.Cells[12, 3] = "";
                worksheet.Cells[12, 4] = "Yes";
                worksheet.Cells[12, 5] = "The person who submitted the data to us";

                worksheet.Cells[13, 1] = "Submitter email address";
                worksheet.Cells[13, 2] = "";
                worksheet.Cells[13, 3] = "";
                worksheet.Cells[13, 4] = "Yes";
                worksheet.Cells[13, 5] = "The email address of the person submitting the results";

                worksheet.Cells[14, 1] = "Organizer (if different from submitter)";
                worksheet.Cells[14, 2] = "";
                worksheet.Cells[14, 3] = "";
                worksheet.Cells[14, 4] = "No";
                worksheet.Cells[14, 5] = "Someone who can help us sort out any mistakes in the data if the original submitter can't";

                worksheet.Cells[15, 1] = "Organizer email address";
                worksheet.Cells[15, 2] = "";
                worksheet.Cells[15, 3] = "";
                worksheet.Cells[15, 4] = "No";
                worksheet.Cells[15, 5] = "The email address of the person who can sort out mistakes";

                worksheet.Cells[16, 1] = "";
                worksheet.Cells[16, 2] = "";
                worksheet.Cells[16, 3] = "";
                worksheet.Cells[16, 4] = "";
                worksheet.Cells[16, 5] = "";

                worksheet.Cells[17, 1] = "Info about the data";
                worksheet.Cells[17, 2] = "";
                worksheet.Cells[17, 3] = "";
                worksheet.Cells[17, 4] = "";
                worksheet.Cells[17, 5] = "";

                worksheet.Cells[18, 1] = "Does the event conform to the HEMA Ratings event criteria?";
                worksheet.Cells[18, 2] = "";
                worksheet.Cells[18, 3] = "";
                worksheet.Cells[18, 4] = "Yes";
                worksheet.Cells[18, 5] = "I have read and understood the criteria laid out on HEMA Ratings \"About\" page, and the event meets these requirements: https://hemaratings.com/about/";

                worksheet.Cells[19, 1] = "Are there any fights in the submitted results that didn't happen?";
                worksheet.Cells[19, 2] = "";
                worksheet.Cells[19, 3] = "";
                worksheet.Cells[19, 4] = "Yes";
                worksheet.Cells[19, 5] = "If someone withdrew due to injury or for some other reason didn't fight all their fights, those fight should not be included. We want to rate people's fighting ability, not their injuries";

                worksheet.Cells[20, 1] = "Are there any missing fights in the data?";
                worksheet.Cells[20, 2] = "";
                worksheet.Cells[20, 3] = "";
                worksheet.Cells[20, 4] = "Yes";
                worksheet.Cells[20, 5] = "Sometimes there are fights missing for various reasons. If there are any fights missing, please let us know why and we'll find out how to proceed.";

                #endregion

                #region Clubs
                worksheet = workbook.Sheets[2];
                worksheet.Name = "Clubs";
                cellRowIndex = 1;
                cellColumnIndex = 1;
                //TODO
                //Loop through Sedicesimi

                worksheet.Cells[cellRowIndex, 1] = "Name";
                worksheet.Cells[cellRowIndex, 2] = "Country";
                worksheet.Cells[cellRowIndex, 3] = "State";
                worksheet.Cells[cellRowIndex, 4] = "City";
                worksheet.Cells[cellRowIndex, 5] = "Website URL";
                worksheet.Cells[cellRowIndex, 6] = "Facebook URL";
                worksheet.Cells[cellRowIndex, 7] = "Parent club (see explanation document)";

                cellRowIndex++;
                foreach (HemaRatingsClubEntity r in dataGridViewClubs.Items)
                {
                    worksheet.Cells[cellRowIndex, 1] = r.Name;
                    worksheet.Cells[cellRowIndex, 2] = r.Country;
                    worksheet.Cells[cellRowIndex, 3] = r.State;
                    worksheet.Cells[cellRowIndex, 4] = r.City;
                    worksheet.Cells[cellRowIndex, 5] = "";
                    worksheet.Cells[cellRowIndex, 6] = "";
                    worksheet.Cells[cellRowIndex, 7] = "";
                  
                    cellRowIndex++;
                }
                #endregion

                #region Fighters
                worksheet = workbook.Sheets[3];
                worksheet.Name = "Fighters";
                cellRowIndex = 1;
                cellColumnIndex = 1;

                //Loop through Sedicesimi
                worksheet.Cells[cellRowIndex, 1] = "Name";
                worksheet.Cells[cellRowIndex, 2] = "Club";
                worksheet.Cells[cellRowIndex, 3] = "Nationality";
                worksheet.Cells[cellRowIndex, 4] = "Gender";
                worksheet.Cells[cellRowIndex, 5] = "HemaRatingsId";

                cellRowIndex++;

                foreach(HemaRatingsFighterMatchEntity r in dataGridViewFighters.Items)
                {
                    worksheet.Cells[cellRowIndex, 1] = r.Name;
                    worksheet.Cells[cellRowIndex, 2] = r.Club;
                    worksheet.Cells[cellRowIndex, 3] = r.Nationality;
                    worksheet.Cells[cellRowIndex, 4] = r.Gender;
                    worksheet.Cells[cellRowIndex, 5] = r.HemaRatingsId;
                     
                    cellRowIndex++;
                }
                #endregion

                #region matches results
                worksheet = workbook.Sheets[4];
                worksheet.Name = nomeDisciplina;
                cellRowIndex = 1;
                cellColumnIndex = 1;

                //Loop through Ottavi
                worksheet.Cells[cellRowIndex, 1] = "Fighter1";
                worksheet.Cells[cellRowIndex, 2] = "Fighter2";
                worksheet.Cells[cellRowIndex, 3] = "Fighter_1_Result";
                worksheet.Cells[cellRowIndex, 4] = "Fighter_2_Result";
                worksheet.Cells[cellRowIndex, 5] = "Round";

                cellRowIndex++;

                foreach (HemaRatingsMatchResult r in dataGridViewMatchs.Items)
                {
                    worksheet.Cells[cellRowIndex, 1] = r.Fighter1;
                    worksheet.Cells[cellRowIndex, 2] = r.Fighter2;
                    worksheet.Cells[cellRowIndex, 3] = r.Fighter_1_Result;
                    worksheet.Cells[cellRowIndex, 4] = r.Fighter_2_Result;
                    worksheet.Cells[cellRowIndex, 5] = r.Round;

                    cellRowIndex++;
                }
                #endregion

                workbook.SaveAs(fileNameWithPath);
              
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
