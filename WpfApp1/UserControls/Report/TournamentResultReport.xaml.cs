using BusinessEntity.DAO;
using Report;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HEMATournamentSystem
{
    /// <summary>
    /// Interaction logic for TournamentResultReport.xaml
    /// </summary>
    public partial class TournamentResultReport : Window
    {
        private int _idTorneo;
        private string _nomeTorneo;
        private int _idDisciplina;

        BackgroundWorker bgWorkerExport;

        public TournamentResultReport(int idTorneo, string nomeTorneo)
        {
            _idTorneo = idTorneo;
            _nomeTorneo = nomeTorneo;
            
            InitializeComponent();

            LoadAllResults();

            txtBlockTitle.Text = nomeTorneo;

        }

        private void LoadAllResults()
        {
            var disciplines = SqlDal_Tournaments.GetDisciplineByIdTorneo(_idTorneo);

            foreach(var d in disciplines)
            {
                if (d.IdDisciplina == 0)
                    continue;   //skip the empy record

                TabItem item = new TabItem();

                item.Header = d.Nome;

                item.Content = new TournamentReportByDiscipline(_idTorneo, d.IdDisciplina, d.Nome);

                tabControlResults.Items.Add(item);
            }

            tabControlResults.SelectedIndex = 0;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool? result = new MessageBoxCustom("Confirm EXIT ?", MessageType.Warning, MessageButtons.OkCancel).ShowDialog();

            if (result != null && !result.Value)
            {
                e.Cancel = true;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCloseReport_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void btnExportResults_Click(object sender, RoutedEventArgs e)
        {
            btnExportResults.IsEnabled = false;
            tabControlResults.IsEnabled = false;

            LoadingCustom loading = new LoadingCustom();

            loading.Owner = this;
            loading.Show();


            string exportMessage = "";

            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    var tabsNumber = tabControlResults.Items.Count;
                    var addingValue = 100 / tabsNumber;

                    try
                    {                            
                        for (int i = 0; i < tabsNumber; i++)
                        {
                            tabControlResults.SelectedIndex = i;
                            TournamentReportByDiscipline p = (TournamentReportByDiscipline)tabControlResults.SelectedContent;

                            p.GenerateExcel(_nomeTorneo, p.nomeDisciplina, fbd.SelectedPath);

                            loading.IncrementProgressBar(100 / tabsNumber);

                        }
                    }
                    catch (Exception ex)
                    {
                        exportMessage = ex.Message;

                    }
                }
            }
            if(exportMessage == "")
                new MessageBoxCustom("Export completato con successo", MessageType.Success, MessageButtons.Ok);
            else
                new MessageBoxCustom(exportMessage, MessageType.Error, MessageButtons.Ok);

            btnExportResults.IsEnabled = true;
            tabControlResults.IsEnabled = true;

            loading.Close();
        }
    }
}
