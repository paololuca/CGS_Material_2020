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
    /// Interaction logic for Tournaments.xaml
    /// </summary>
    public partial class Tournaments : UserControl
    {
        public Tournaments()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridActiveTournament.ItemsSource = Helper.GetTorneiAttivi(true);
            dataGridClosedTournament.ItemsSource = Helper.GetTorneiNonAttivi(true);
        }

        private void DataGridActiveTournament_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "Id":
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

        private void DataGridClosedTournament_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
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
    }
}
