using Resources;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BusinessEntity.Entity;
using FormsManagement.Settings;
using System;

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
    }
}
