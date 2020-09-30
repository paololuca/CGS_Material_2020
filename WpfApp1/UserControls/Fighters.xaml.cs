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

namespace HEMATournamentSystem
{
    /// <summary>
    /// Interaction logic for Fighters.xaml
    /// </summary>
    public partial class Fighters : UserControl
    {
        private List<AtletaEntity> atletiPresenti;

        public Fighters()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadListAtleti();
            LoadAssociation();
        }

        

        private void LoadListAtleti()
        {
            atletiPresenti = Helper.GetAllAnagraficaAtletiWithRanking();

            dataGridAthletes.ItemsSource = atletiPresenti;
        }

        private void LoadAssociation()
        {
            var associations = Helper.GetAllAsd(false);
            cmbAssociation.ItemsSource = associations;
            cmbAssociation.DisplayMemberPath = "NomeAsd";
            cmbAssociation.SelectedValuePath = "NomeAsd";

        }

        private void DataGridAthletes_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            Style horizontalAlignment = new Style();
            horizontalAlignment.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));

            switch (e.Column.Header.ToString())
            {
                case "FullName":
                case "IdAtleta":
                case "IdAsd":
                case "Ranking":
                case "Posizionamento":
                    e.Column.Visibility = Visibility.Hidden;
                    break;
                default:
                    e.Column.Visibility = Visibility.Visible;
                    //e.Column.IsReadOnly = true;
                    break;

            }
        }

        private void BtnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDetailsAccount_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TxtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            //TODO uppercase dei campi
            var filtered = atletiPresenti.Where(fighter => fighter.Nome.Contains(txtSearch.Text) || fighter.Cognome.Contains(txtSearch.Text)).ToList();

            dataGridAthletes.ItemsSource = filtered;
        }

        private void BtnSaveFighter_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "")
                PopUpBoxes.ShowPopup("Name cannot be empty");
            else if (txtSurnameame.Text == "")
                PopUpBoxes.ShowPopup("Surname cannot be empty");
            else if (!(bool)rbtFemminile.IsChecked && !(bool)rbtMaschile.IsChecked)
                PopUpBoxes.ShowPopup("Select a gender");

            var ass = cmbAssociation.SelectedItem;

        }
    }
}
