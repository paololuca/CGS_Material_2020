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

            //TODO legare i bottone di attivazione e disattivazione al ruolo utente

            if (!user.IsAdmin && user.Type != BusinessEntity.Type.ProfileType.Editor)
                btnNewTournament.IsEnabled = false;
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
                case "Note":
                case "StartDate":
                case "EndDate":
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

            var nT = new AddTournament();
            nT.Show();
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

        private static void ShowHemaRatingsReport(object sender)
        {
            TorneoEntity torunament = ((FrameworkElement)sender).DataContext as TorneoEntity;

            TournamentResultReport report = new TournamentResultReport(torunament.Id, torunament.Name);

            report.Show();
        }

        private void btnDeleteTournament_Click(object sender, RoutedEventArgs e)
        {
            bool? result = new MessageBoxCustom("Tournament will be DELETED! Continue?",
                MessageType.Warning, MessageButtons.OkCancel).ShowDialog();

            if(result.Value)
            {
                TorneoEntity torunament = ((FrameworkElement)sender).DataContext as TorneoEntity;

                DeleteMatches(torunament);

                SqlDal_Tournaments.EliminaTorneo(torunament.Id);
            }
        }

        private static void DeleteMatches(TorneoEntity torunament)
        {
            var disciplines = SqlDal_Tournaments.GetDisciplineByIdTorneo(torunament.Id);

            foreach (var d in disciplines)
            {
                SqlDal_Pools.DeletePoolsAndMatches(torunament.Id, d.IdDisciplina);

                SqlDal_Pools.DeleteAllPahases(torunament.Id, d.IdDisciplina);
            }
        }

        private void btnDeleteTournamentMatchs_Click(object sender, RoutedEventArgs e)
        {
            bool? result = new MessageBoxCustom("Tournament will be DELETED! Continue?",
                MessageType.Warning, MessageButtons.OkCancel).ShowDialog();

            if (result.Value)
            {
                TorneoEntity torunament = ((FrameworkElement)sender).DataContext as TorneoEntity;
                DeleteMatches(torunament);
            }
        }
    }
}
