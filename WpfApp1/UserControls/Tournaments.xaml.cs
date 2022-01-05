using Resources;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BusinessEntity.Entity;
using System;
using WindowsFormsApplication1;
using HEMATournamentSystem.Report;
using FormsManagement.Menu;
using Report;

namespace HEMATournamentSystem
{
    /// <summary>
    /// Interaction logic for Tournaments.xaml
    /// </summary>
    public partial class Tournaments : UserControl
    {
        private List<TorneoEntity> newTournamentList;
        private List<TorneoEntity> activeTournamentList;
        private List<TorneoEntity> closedTournamentList;
        private CaricaGironiDaDisciplina risultatiTorneo;
        private readonly LoginUser user;

        public Tournaments(LoginUser user)
        {
            this.user = user;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadNewTournaments();

            LoadActiveTournaments();

            LoadClosedTournaments();
        }

        private void LoadClosedTournaments()
        {
            closedTournamentList = SqlDal_Tournaments.GetTorneiConclusi(true);
            dataGridClosedTournament.ItemsSource = closedTournamentList;
        }

        private void LoadActiveTournaments()
        {
            activeTournamentList = SqlDal_Tournaments.GetTorneiAttivi(true);
            dataGridActiveTournament.ItemsSource = activeTournamentList;
        }

        private void LoadNewTournaments()
        {
            newTournamentList = SqlDal_Tournaments.GetTorneiToActivate(true);
            dataGridNewTournament.ItemsSource = newTournamentList;
        }

        private void dataGridTournament_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            SetColumnHeader(e);
        }

        private static void SetColumnHeader(DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "Id":
                    e.Column.Visibility = Visibility.Hidden;
                    break;
                default:
                    e.Column.Visibility = Visibility.Visible;
                    e.Column.IsReadOnly = true;
                    break;
            }
        }
              

        private void btnNewTournament_Click(object sender, RoutedEventArgs e)
        {
            AddTorneo newTorneo = new AddTorneo();
            newTorneo.FormClosing += new System.Windows.Forms.FormClosingEventHandler(creaTorneo_FormClosed);

            newTorneo.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            newTorneo.Show();
        }

        private void creaTorneo_FormClosed(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            LoadNewTournaments();
        }

        private void txtSearchNewTournament_KeyUp(object sender, KeyEventArgs e)
        {
            var filtered = newTournamentList.
                Where(tournament => 
                tournament.Name.ToLower().Contains(txtSearchNewTournament.Text) || 
                tournament.Note.ToLower().Contains(txtSearchNewTournament.Text)).ToList();

            dataGridNewTournament.ItemsSource = filtered;
        }

        private void txtSearchActiveTournament_KeyUp(object sender, KeyEventArgs e)
        {
            var filtered = activeTournamentList.
                Where(tournament =>
                tournament.Name.ToLower().Contains(txtSearchActiveTournament.Text) ||
                tournament.Note.ToLower().Contains(txtSearchActiveTournament.Text)).ToList();

            dataGridActiveTournament.ItemsSource = filtered;
        }

        private void txtSearchClosedTournament_KeyUp(object sender, KeyEventArgs e)
        {
            var filtered = closedTournamentList.
                Where(tournament =>
                tournament.Name.ToLower().Contains(txtSearchClosedTournament.Text) ||
                tournament.Note.ToLower().Contains(txtSearchClosedTournament.Text)).ToList();

            dataGridClosedTournament.ItemsSource = filtered;
        }

        private void btnDeleteTournament_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnActivateTournament_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnmanageSubscription_Click(object sender, RoutedEventArgs e)
        {
            ManageTournament tournament = new ManageTournament();

            tournament.Show();
            tournament.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            tournament.TopMost = false;
        }

        private void btnReportActiveTournament_Click(object sender, RoutedEventArgs e)
        {
            ShowReport(sender);
        }

        private void btnReportNoActiveTournament_Click(object sender, RoutedEventArgs e)
        {
            ShowReport(sender);
        }

        private static void ShowReport(object sender)
        {
            TorneoEntity torunament = ((FrameworkElement)sender).DataContext as TorneoEntity;

            TournamentResultReport report = new TournamentResultReport(torunament.Id, torunament.Name);

            report.Show();
        }

        //private void VisualizzaReport_FormClosed(object sender, System.Windows.Forms.FormClosingEventArgs e)
        //{
        //    if ((sender as System.Windows.Forms.Form).DialogResult == System.Windows.Forms.DialogResult.None)
        //    {
        //        // Then assume that X has been clicked and act accordingly.
        //    }
        //    else if ((sender as System.Windows.Forms.Form).DialogResult == System.Windows.Forms.DialogResult.OK)
        //    {
        //        if (risultatiTorneo != null)
        //        {
        //            if ((risultatiTorneo.IdDisciplina > 0) && (risultatiTorneo.IdTorneo > 0))
        //            {
        //                ReportRisultatiTorneo report = new ReportRisultatiTorneo(risultatiTorneo.IdTorneo, risultatiTorneo.IdDisciplina);
        //                report.Show();
        //                report.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        //            }
        //        }
        //    }
        //    else if ((sender as System.Windows.Forms.Form).DialogResult == System.Windows.Forms.DialogResult.Abort)
        //    {
        //        // Then assume that Cancel Button has been clicked and act accordingly.)
        //    }
        //}
    }
}
