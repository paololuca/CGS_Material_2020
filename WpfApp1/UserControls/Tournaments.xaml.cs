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
using BusinessEntity.Entity;

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

        public Tournaments()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            newTournamentList = Helper.GetTorneiToActivate(true);
            dataGridNewTournament.ItemsSource = newTournamentList;

            activeTournamentList = Helper.GetTorneiAttivi(true);
            dataGridActiveTournament.ItemsSource = activeTournamentList;

            closedTournamentList = Helper.GetTorneiNonAttivi(true);
            dataGridClosedTournament.ItemsSource = closedTournamentList;
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
                tournament.Name.ToLower().Contains(txtSearchNewTournament.Text) ||
                tournament.Note.ToLower().Contains(txtSearchNewTournament.Text)).ToList();

            dataGridActiveTournament.ItemsSource = filtered;
        }

        private void txtSearchClosedTournament_KeyUp(object sender, KeyEventArgs e)
        {
            var filtered = closedTournamentList.
                Where(tournament =>
                tournament.Name.ToLower().Contains(txtSearchNewTournament.Text) ||
                tournament.Note.ToLower().Contains(txtSearchNewTournament.Text)).ToList();

            dataGridClosedTournament.ItemsSource = filtered;
        }
    }
}
