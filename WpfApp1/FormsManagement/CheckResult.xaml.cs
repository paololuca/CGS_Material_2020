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
        private Int32 idTorneo = 0;
        private Int32 idDisciplina = 0;
        private Int32 atletiAmmessiEliminatorie;

        public bool WindowCheckResult = false;
        private int _currentSelectedItems;

        public CheckResult(Int32 idTorneo, Int32 idDisciplina, Int32 atletiAmmessiEliminatorie)
        {
            this.idTorneo = idTorneo;
            this.idDisciplina = idDisciplina;
            this.atletiAmmessiEliminatorie = atletiAmmessiEliminatorie;

            InitializeComponent();

            CaricaAtletiPostGironi();

            this._currentSelectedItems = atletiAmmessiEliminatorie;
            SetLStatusLabel();
        }

        private void CaricaAtletiPostGironi()
        {
            List<GironiConclusi> gironiConclusi = SqlDal_Pools.GetClassificaGironi(idTorneo, idDisciplina);

            for (int i = 0; i < atletiAmmessiEliminatorie; i++)
                gironiConclusi[i].Qualificato = true;

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
                    e.Column.IsReadOnly = true;
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
            lblStatus.Content = " Selezionati " + _currentSelectedItems + " Atleti per la fase successiva";
        }

        private void BtnSaveResult_Click(object sender, RoutedEventArgs e)
        {
                        
            if (_currentSelectedItems != atletiAmmessiEliminatorie)
            {
                System.Windows.Forms.MessageBox.Show("Il numero di atleti selezionati non è " + atletiAmmessiEliminatorie + ": controllare la lista", "ERRORE", 
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            else
            {
                List<AtletaEliminatorie> listaQualificati = new List<AtletaEliminatorie>();
                int posizione = 1;
                foreach (GironiConclusi a in dataGridResult.Items)
                {
                    if (a.Qualificato)
                    {
                        listaQualificati.Add(new AtletaEliminatorie()
                        {
                            IdAtleta = a.IdAtleta,
                            IdTorneo = a.IdTorneo,
                            idDisciplina = a.IdDisciplina,
                            Posizione = posizione
                        }
                        );
                        posizione++;
                    }
                }

                CreateNextPhase(listaQualificati);

                SqlDal_Pools.ConcludiGironi(idTorneo, idDisciplina);

                WindowCheckResult = true;
                this.Close();
            }
        }

        private void CreateNextPhase(List<AtletaEliminatorie> listaQualificati)
        {
            if (atletiAmmessiEliminatorie == 32)
                SqlDal_Pools.InsertSedicesimi(listaQualificati);
            else if (atletiAmmessiEliminatorie == 16)
                SqlDal_Pools.InsertOttavi(listaQualificati);
            else if (atletiAmmessiEliminatorie == 8)
                SqlDal_Pools.InsertQuarti(listaQualificati);
            else if (atletiAmmessiEliminatorie == 4)
                SqlDal_Pools.InsertSemifinali(SetCampoForSemifinali(listaQualificati));
        }

        private List<AtletaEliminatorie> SetCampoForSemifinali(List<AtletaEliminatorie> listaQualificati)
        {
            listaQualificati[0].Campo = 1;
            listaQualificati[3].Campo = 1;
            listaQualificati[1].Campo = 2;
            listaQualificati[2].Campo = 2;

            return listaQualificati;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            WindowCheckResult = false;
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

        private void dataGridResult_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var column = e.Column as DataGridBoundColumn;
                if (column != null)
                {
                    
                    int rowIndex = e.Row.GetIndex();

                    var row = (GironiConclusi)dataGridResult.Items[rowIndex];

                    if (row.Qualificato)
                        _currentSelectedItems--;    //from true to false
                    else
                        _currentSelectedItems++;    //from false to true

                    SetLStatusLabel();
                }
            }
        }
    }
}
