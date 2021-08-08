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
    /// Interaction logic for Associations.xaml
    /// </summary>
    public partial class Associations : UserControl
    {
        private List<AsdEntity> associations;
        private readonly LoginUser user;

        public Associations(LoginUser user)
        {
            this.user = user;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAssociations();
        }

        private void LoadAssociations()
        {
            associations = Helper.GetAllAsd(true);
            dataGridAssociations.ItemsSource = associations;
        }

        private void BtnDeleteAssociation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnMigrateAssociation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGridAssociations_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "Id":
                case "AtletiAssociativi":
                    e.Column.Visibility = Visibility.Hidden;
                    break;
                default:
                    e.Column.Visibility = Visibility.Visible;
                    //e.Column.IsReadOnly = true;
                    break;

            }
        }

        private void TxtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            var filtered = associations.Where(associaition => associaition.NomeAsd.ToLower().Contains(txtSearch.Text.ToLower())).ToList();

            dataGridAssociations.ItemsSource = filtered;
        }

        private void BtnSaveAssociation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            //TODO to implement
        }

        private void BtnExport_Copy_Click(object sender, RoutedEventArgs e)
        {
            //TODO to implement
        }
    }
}
