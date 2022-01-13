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
            associations = SqlDal_Associations.GetAllAsd(true);
            dataGridAssociations.ItemsSource = associations;
        }

        //TODO
        private void BtnDeleteAssociation_Click(object sender, RoutedEventArgs e)
        {

        }

        //TODO
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
                    e.Column.IsReadOnly = true;
                    break;

            }
        }

        private void TxtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            List<AsdEntity> filtered = GetFilteredList();

            dataGridAssociations.ItemsSource = filtered;
        }

        private List<AsdEntity> GetFilteredList()
        {
            return associations.Where(associaition => associaition.NomeAsd.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
        }

        //TODO
        private void BtnSaveAssociation_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            List<AsdEntity> filtered = GetFilteredList().OrderBy(x => x.NomeAsd).ToList();
            // Creating a Excel object. 
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            try
            {

                worksheet = workbook.ActiveSheet;

                worksheet.Name = "Associations";

                int cellRowIndex = 1;

                worksheet.Cells[cellRowIndex, 1] = "Id";
                worksheet.Cells[cellRowIndex, 2] = "Association Name";

                foreach (var a in filtered)
                {
                    cellRowIndex++;
                    worksheet.Cells[cellRowIndex, 1] = a.Id;
                    worksheet.Cells[cellRowIndex, 2] = a.NomeAsd;
                }



                //Getting the location and file name of the excel to save from user. 
                System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveDialog.FilterIndex = 2;

                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    new MessageBoxCustom("Export completato con successo", MessageType.Success, MessageButtons.Ok).ShowDialog();                   
                }
            }
            catch (System.Exception ex)
            {
                new MessageBoxCustom(ex.Message, MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
            finally
            {
                excel.Quit();
                workbook = null;
                excel = null;
            }
        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            //TODO to implement
        }
    }
}
