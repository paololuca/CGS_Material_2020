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
        private List<TournamentReportByDiscipline> tournamentsDisciplinesList;
        private string exportMessage = "";
        private string exportPath = "";

        BackgroundWorker bgWorkerExport;

        public TournamentResultReport(int idTorneo, string nomeTorneo)
        {
            _idTorneo = idTorneo;
            _nomeTorneo = nomeTorneo;
            
            InitializeComponent();

            LoadAllResults();

            txtBlockTitle.Text = nomeTorneo;

            bgWorkerExport = new BackgroundWorker();
            // To report progress from the background worker we need to set this property
            bgWorkerExport.WorkerReportsProgress = true;
            // This event will be raised on the worker thread when the worker starts
            bgWorkerExport.DoWork += new DoWorkEventHandler(bgWorkerExport_DoWork);
            // This event will be raised when we call ReportProgress
            bgWorkerExport.ProgressChanged += new ProgressChangedEventHandler(bgWorkerExport_ProgressChanged);
            bgWorkerExport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerExport_Completed);

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
            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {

                    btnExportResults.IsEnabled = false;
                    tabControlResults.IsEnabled = false;
                    progressBarExport.Value = 0;
                    lblPercentage.Content = "0%";
                    exportPath = fbd.SelectedPath;

                    tournamentsDisciplinesList = new List<TournamentReportByDiscipline>();

                    for (int i = 0; i < tabControlResults.Items.Count; i++)
                    {
                        tabControlResults.SelectedIndex = i;
                        TournamentReportByDiscipline p = (TournamentReportByDiscipline)tabControlResults.SelectedContent;
                        tournamentsDisciplinesList.Add(p);
                    }

                    tabControlResults.SelectedIndex = 0;

                    bgWorkerExport.RunWorkerAsync();
                }
            }
        }

        void bgWorkerExport_DoWork(object sender, DoWorkEventArgs e)
        {
            var i  = 0;
            foreach(var t in tournamentsDisciplinesList)
            {
                try
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        t.GenerateExcel(_nomeTorneo, t.nomeDisciplina, exportPath);
                    }), DispatcherPriority.ContextIdle);

                    var percentage = (i + 1) * 100 / tabControlResults.Items.Count;
                    bgWorkerExport.ReportProgress(percentage);
                    Thread.Sleep(2000);                    
                }
                catch (Exception ex)
                {
                    exportMessage += "Error on Export " + i + 1 + " : " + ex.Message + "\n";
                }
                finally
                {
                    i++;
                }
            }
        }

        // Back on the 'UI' thread so we can update the progress bar
        void bgWorkerExport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // The progress percentage is a property of e
            progressBarExport.Value = e.ProgressPercentage;
            lblPercentage.Content = e.ProgressPercentage + "%";
        }

        private void bgWorkerExport_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                btnExportResults.IsEnabled = true;
                tabControlResults.IsEnabled = true;

                if (exportMessage == "")
                    new MessageBoxCustom("Export completato con successo", MessageType.Success, MessageButtons.Ok).ShowDialog();
                else
                    new MessageBoxCustom(exportMessage, MessageType.Error, MessageButtons.Ok).ShowDialog();

            }), DispatcherPriority.ContextIdle);
        }
    }
}
