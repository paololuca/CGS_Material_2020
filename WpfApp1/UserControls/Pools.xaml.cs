using BusinessEntity.Entity;
using FormsManagement.Menu;
using FormsManagement;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HEMATournamentSystem
{
    /// <summary>
    /// Interaction logic for Pools.xaml
    /// </summary>
    public partial class Pools : UserControl
    {

        private CaricaGironiDaDisciplina caricaGironi = null;
        List<List<AtletaEntity>> gironi = new List<List<AtletaEntity>>();
        
        //Lista degli scontri per girone
        List<List<MatchEntity>> gironiIncontri = null;

        private int numeroAtletiTorneoDisciplina = 0;

        private int numeroGironi = 0;
        private int tournamentId;
        private string tournamentName;
        private int disciplineId;
        private string disciplineName;
        private int atletiAmmessiEliminatorie;
        private readonly LoginUser user;

        public Pools(LoginUser user)
        {
            this.user = user;
            InitializeComponent();
        }



        private void BtnOpenTournament_Click(object sender, RoutedEventArgs e)
        {

            caricaGironi = new CaricaGironiDaDisciplina(false);
            caricaGironi.FormClosing += new System.Windows.Forms.FormClosingEventHandler(caricaGironi_FormClosed);

            caricaGironi.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            caricaGironi.Show();
        }


        private void caricaGironi_FormClosed(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if ((sender as System.Windows.Forms.Form).DialogResult == System.Windows.Forms.DialogResult.None)
            {
                // Then assume that X has been clicked and act accordingly.
            }
            else if ((sender as System.Windows.Forms.Form).DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (caricaGironi != null)
                {
                    if ((caricaGironi.IdDisciplina > 0) && (caricaGironi.IdTorneo > 0))
                    {
                        //TODO solo se non ci sono già gironi attivi (e cioè in stato 0 per quel torneo e disciplina)
                        
                        CaricaGironiCreati(caricaGironi.IdTorneo, caricaGironi.IdDisciplina, caricaGironi.Categoria);
                        tournamentId = caricaGironi.IdTorneo;
                        tournamentName = caricaGironi.NomeTorneo;
                        disciplineId = caricaGironi.IdDisciplina;
                        disciplineName = caricaGironi.Disciplina;
                        
                    }
                }
            }
            else if ((sender as System.Windows.Forms.Form).DialogResult == System.Windows.Forms.DialogResult.Abort)
            {
                // Then assume that Cancel Button has been clicked and act accordingly.)
            }
        }

        private void CaricaGironiCreati(int idTorneo, int idDisciplina, String categoria)
        {

            numeroGironi = Helper.GetNumeroGironiByTorneoDisciplina(idTorneo, idDisciplina, categoria);

            if (numeroGironi > 0)
            {
                gironi = new List<List<AtletaEntity>>();
                //TODO il problema è qui, carica la lista degli atleti in maniera diversa di come li salva la prima volta
                gironi = Helper.GetGironiSalvati(idTorneo, idDisciplina, categoria);

                numeroAtletiTorneoDisciplina = gironi.SelectMany(list => list).Distinct().Count();

                gironiIncontri = new List<List<MatchEntity>>();

                tabControlPool.Items.Clear();

                Int32 idGirone = 1;

                foreach (List<AtletaEntity> g in gironi)
                {
                    List<MatchEntity> l = null;

                    //TODO eliminabile visto che sono già sul DB
                    if (g.Count == 4)
                        l = Helper.ElaborateT4(g);
                    else if (g.Count == 5)
                        l = Helper.ElaborateT5(g);
                    else if (g.Count == 6)
                        l = Helper.ElaborateT6(g);

                    if (l != null)
                    {
                        foreach (MatchEntity i in l)
                            Helper.CaricaPunteggiEsistentiGironiIncontri(idTorneo, idDisciplina, i, idGirone);

                        gironiIncontri.Add(l);
                        string title = "Girone " + (tabControlPool.Items.Count + 1).ToString();
                        tabControlPool.Items.Add(ElaboraTab(title, g, l, tabControlPool.Items.Count + 1));
                    }
                    idGirone++;
                }

                EnablePageControls();

            }
            else
            {
                MessageBox.Show("Si è verificato un errore durante il recupero delle informazioni sul numero dei gironi \r\nContattare un amministratore",
                                "ERRORE Applicazione", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EnablePageControls()
        {
            btnClosePools.IsEnabled = true;
            btnExportMatch.IsEnabled = true;
            btnExportPools.IsEnabled = true;
            btnLoadPhases.IsEnabled = true;
        }

        /// <summary>
        /// Costruisce il tab dello specifico del girone
        /// </summary>
        /// <param name="title">Titolo del TAB</param>
        /// <param name="g">lista dei giroi (lista di persone)</param>
        /// <param name="l">Lista degli incontri</param>
        /// <returns></returns>
        private TabItem ElaboraTab(string title, List<AtletaEntity> g, List<MatchEntity> l, Int32 tabIndex)
        {
            TabItem item = new TabItem();

            item.Header = title;

            item.Content = new Pool(caricaGironi.IdTorneo, caricaGironi.IdDisciplina, g, l, tabIndex);

            return item;
        }

        private void BtnClosePools_Click(object sender, RoutedEventArgs e)
        {
            //TODO PL chiamata alla "validaEliminatorie" in cui deve essere gestito il motore tramite DB
            if (System.Windows.Forms.MessageBox.Show("If you continue all values about next phases, if present, will be deleted.\n\n Continue?", "Attenzione",
                System.Windows.Forms.MessageBoxButtons.OKCancel,
                System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                //TODO Helper.DeleteAllPahases(tournamentId, disciplineId);

                foreach (var p in tabControlPool.Items)
                    Console.WriteLine(p.GetType());

                //TODO calcolo dei valori presi da DB

                atletiAmmessiEliminatorie = numeroAtletiTorneoDisciplina >= 54 ? 32 :
                    numeroAtletiTorneoDisciplina >= 24 ? 16 :
                    numeroAtletiTorneoDisciplina >= 12 ? 8 : 4;

                //TODO salvare tutti i tab

                Window validaAtleti = new CheckResult(caricaGironi.IdTorneo, caricaGironi.IdDisciplina, atletiAmmessiEliminatorie);
                

                validaAtleti.Closing += new CancelEventHandler(creaEliminatorie_FormClosed);
                
                validaAtleti.Show();
            }

        }

        private void creaEliminatorie_FormClosed(object sender, CancelEventArgs e)
        {
            CheckResult windowResult = (sender as CheckResult);

            if(windowResult.WindowCheckResult)
                MessageBox.Show("User clicked OK");
            else
                MessageBox.Show("User clicked cancel");

        }

        #region Export
        private void BtnExportPools_Click(object sender, RoutedEventArgs e)
        {
            PdfManager pdf = new PdfManager();
            pdf.StampaGironi(gironi, tournamentName, disciplineName);
        }

        private void BtnExportMatch_Click(object sender, RoutedEventArgs e)
        {
            PdfManager pdf = new PdfManager();
            pdf.StampaGironiConIncontri(gironi, tournamentName, disciplineName);
        }
        #endregion

        #region open intermediate phases

        private void BtnOpen16th_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOpen8th_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOpen4th_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOpenSemifinal_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnOpenFinal_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

    }
}
