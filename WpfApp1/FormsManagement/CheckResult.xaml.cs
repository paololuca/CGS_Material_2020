using BusinessEntity.DAO;
using Resources;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace FormsManagement
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CheckResult : Window
    {
        Int32 idTorneo = 0;
        Int32 idDisciplina = 0;
        Int32 atletiAmmessiEliminatorie;
        public CheckResult(Int32 idTorneo, Int32 idDisciplina, Int32 atletiAmmessiEliminatorie)
        {
            this.idTorneo = idTorneo;
            this.idDisciplina = idDisciplina;
            this.atletiAmmessiEliminatorie = atletiAmmessiEliminatorie;

            InitializeComponent();

            CaricaAtletiPostGironi();
        }

        private void CaricaAtletiPostGironi()
        {
            List<GironiConclusi> gironiConclusi = Helper.GetClassificaGironi(idTorneo, idDisciplina);

            for (int i = 0; i < atletiAmmessiEliminatorie; i++)
                gironiConclusi[i].Qualificato = true;

            lblStatus.Content = " Selezionati " + atletiAmmessiEliminatorie + " Atleti per la fase successiva";

            dataGridResult.ItemsSource = gironiConclusi;
        }

        private void DataGridResult_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            Style horizontalAlignment = new Style();
            horizontalAlignment.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));


            switch (e.Column.Header.ToString())
            {
                case "IdTorneo":
                case "IdDisciplina":
                case "IdGirone":
                case "IdAtleta":
                case "Posizionamento":
                    e.Column.Visibility = Visibility.Hidden;
                    break;
                case "Nome":
                case "Cognome":
                    e.Column.Visibility = Visibility.Visible;
                    break;
                case "Qualificato":
                    e.Column.Visibility = Visibility.Visible;
                    e.Column.CellStyle = horizontalAlignment;
                    e.Column.HeaderStyle = horizontalAlignment;
                    e.Column.IsReadOnly = false;
                    break;
                default:
                    e.Column.Visibility = Visibility.Visible;
                    e.Column.CellStyle = horizontalAlignment;
                    e.Column.HeaderStyle = horizontalAlignment;
                    e.Column.IsReadOnly = true;
                    break;
            }            
        }

       

        private void SetLStatusLabel()
        {
            lblStatus.Content = " Selezionati " + CountSelectedRowInDataGrid() + " Atleti per la fase successiva";
        }

        private int CountSelectedRowInDataGrid()
        {
            int i = 0;
            foreach(GironiConclusi girone in dataGridResult.Items)
            {
                i += girone.Qualificato ? 1 : 0;
            }

            return i;
        }

        private void BtnSaveResult_Click(object sender, RoutedEventArgs e)
        {
            if (CountSelectedRowInDataGrid() != atletiAmmessiEliminatorie)
            {
                System.Windows.Forms.MessageBox.Show("Il numero di atleti selezionati non è " + atletiAmmessiEliminatorie + ": controllare la lista", "ERRORE", 
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnPrintResult_Click(object sender, RoutedEventArgs e)
        {
            PdfManager pdf = new PdfManager();
            pdf.StampaRisultatiGironi(
                dataGridResult,
                Helper.GetTorneoById(this.idTorneo).Name,
                Helper.GetDisciplinaById(this.idDisciplina));
        }

       
       
    }
}
