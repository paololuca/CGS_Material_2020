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
using BusinessEntity.Type;

namespace HEMATournamentSystem
{
    /// <summary>
    /// Interaction logic for Pools.xaml
    /// </summary>
    public partial class Pools : UserControl
    {

        private CaricaGironiDaDisciplina caricaGironi = null;
        private CreaGironiDaDisciplina creaGironi = null;

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
        private int startPhase = 0;
        private readonly LoginUser user;

        public Pools(LoginUser user)
        {
            this.user = user;
            InitializeComponent();
        }


        private void btnCreateTournamentPools_Click(object sender, RoutedEventArgs e)
        {
            creaGironi = new CreaGironiDaDisciplina();
            creaGironi.FormClosing += new System.Windows.Forms.FormClosingEventHandler(creaGironi_FormClosed);

            creaGironi.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            creaGironi.Show();
        }

        private void creaGironi_FormClosed(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if ((sender as System.Windows.Forms.Form).DialogResult == System.Windows.Forms.DialogResult.None)
            {
                // Then assume that X has been clicked and act accordingly.
            }
            else if ((sender as System.Windows.Forms.Form).DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                if (creaGironi != null)
                {
                    if ((creaGironi.IdDisciplina > 0) && (creaGironi.IdTorneo > 0))
                    {
                        //TODO solo se non ci sono già gironi attivi (e cioè in stato 0 per quel torneo e disciplina)
                        //problematica rigirata sulla form di caricamento 
                        creaGironiAndLoad(creaGironi.IdTorneo, creaGironi.IdDisciplina, creaGironi.Categoria);
                    }
                }
            }
            else if ((sender as System.Windows.Forms.Form).DialogResult == System.Windows.Forms.DialogResult.Abort)
            {
                // Then assume that Cancel Button has been clicked and act accordingly.)
            }
        }

        private void creaGironiAndLoad(int idTorneo, int idDisciplina, String categoria)
        {

            //loadToolStripMenuItem.Enabled = false;      //Non permetto più di caricare i dati (in teoria va fatto meglio)

            //partecipantiTorneo = CaricaAtleti();

            // i dati in lettura vanno fatti caricando la disciplina dal DB (divisione per disciplina)
            //se sono in presenza degli assoluti, per ora, carico i dati ordinati solo per ranking e senza random dei nomi all'interno delle ASD
            //partecipantiTorneo = assoluti == false ?
            //                                    Helper.GetAtletiTorneoVsDisciplina(idTorneo, idDisciplina, categoria) :
            //                                    Helper.GetAtletiTorneoVsDisciplinaAssoluti(idTorneo, idDisciplina, categoria);

            var partecipantiTorneo = Helper.GetAtletiTorneoVsDisciplinaAssoluti(idTorneo, idDisciplina, categoria);

            //TODO: da parametrizzare

            bool rankingEnabled = partecipantiTorneo.Sum(x => x.Ranking) != 0;

            numeroGironi = Helper.GetNumeroGironiByTorneoDisciplina(idTorneo, idDisciplina, categoria);
            


            if (numeroGironi > 0)
            {
                gironi = new List<List<AtletaEntity>>();

                //Setting delle strutture dati
                for (int i = 0; i < numeroGironi; i++)
                {
                    //Per ogni girone creo la lista, vuota al momento, dei partecipanti al girone stesso
                    gironi.Add(new List<AtletaEntity>());                    
                }

                //Inserisco ogni partecipante del torneo dentro la struttura dati dei gironi
                //e dell'albero nella posizione corrispondente
                //(l'abero deve essere visualizzato via WEB : quando ci clicchi ti deve far vedere la lista e lo stato degli incontri al suo interno
                int count = 0;

                if (!rankingEnabled)
                {
                    foreach (AtletaEntity a in partecipantiTorneo)    //ciclo sui gironi, e sulla lista atleti partecipanti al torneo, inserendo ogni atleta in un girone diverso, e poi rifacendo il giro
                    {
                        gironi[count].Add(a);

                        if (count == numeroGironi - 1)
                            count = 0;
                        else
                            count++;
                    }
                }
                else
                {
                    //qui ci va il codice per il calcolo dei gironi con gli assoluti
                    int fasceAssoluti = 4;
                    int atletiPerfascia = partecipantiTorneo.Count / fasceAssoluti;

                    #region inizializziani 4 fasce
                    List<AtletaEntity> primaFascia = new List<AtletaEntity>();
                    List<AtletaEntity> secondaFascia = new List<AtletaEntity>();
                    List<AtletaEntity> terzaFascia = new List<AtletaEntity>();
                    List<AtletaEntity> quartaFascia = new List<AtletaEntity>();
                    #endregion 

                    //primo quarto
                    primaFascia.AddRange(partecipantiTorneo.GetRange(0, atletiPerfascia));
                    //secondo quarto     
                    secondaFascia.AddRange(partecipantiTorneo.GetRange(atletiPerfascia, atletiPerfascia));
                    //terzo quarto
                    terzaFascia.AddRange(partecipantiTorneo.GetRange(2 * atletiPerfascia, atletiPerfascia));
                    //tutti i restanti
                    quartaFascia.AddRange(partecipantiTorneo.GetRange(3 * atletiPerfascia, atletiPerfascia + (partecipantiTorneo.Count - (4 * atletiPerfascia))));

                    foreach (List<AtletaEntity> g in gironi)
                    {
                        //inserisco il primo atleta di ogni fascia nel girone i-esimo
                        if (primaFascia.Count > 0)
                            g.Add(primaFascia.ElementAt(0));
                        if (secondaFascia.Count > 0)
                            g.Add(secondaFascia.ElementAt(0));
                        if (terzaFascia.Count > 0)
                            g.Add(terzaFascia.ElementAt(0));
                        if (quartaFascia.Count > 0)
                            g.Add(quartaFascia.ElementAt(0));

                        //elimino quell'atleta dalla lista dei papabili
                        if (primaFascia.Count > 0)
                            primaFascia.RemoveAt(0);
                        if (secondaFascia.Count > 0)
                            secondaFascia.RemoveAt(0);
                        if (terzaFascia.Count > 0)
                            terzaFascia.RemoveAt(0);
                        if (quartaFascia.Count > 0)
                            quartaFascia.RemoveAt(0);
                    }

                    //gestisco eventuali orfani : GIRONI da 5
                    //per sicurezza controllo tutte le fasce ma sarà solo la quarta ad avere degli orfani
                    //che andaranno inseriti nei gironi già popolati a partire dal primo
                    if (primaFascia.Count > 0 || secondaFascia.Count > 0 || terzaFascia.Count > 0 || quartaFascia.Count > 0)
                    {
                        foreach (List<AtletaEntity> g in gironi)
                        {
                            if (primaFascia.Count > 0)
                            {
                                g.Add(primaFascia.ElementAt(0));
                                primaFascia.RemoveAt(0);
                            }
                            else if (secondaFascia.Count > 0)
                            {
                                g.Add(secondaFascia.ElementAt(0));
                                secondaFascia.RemoveAt(0);
                            }
                            else if (terzaFascia.Count > 0)
                            {
                                g.Add(terzaFascia.ElementAt(0));
                                terzaFascia.RemoveAt(0);
                            }
                            else if (quartaFascia.Count > 0)
                            {
                                g.Add(quartaFascia.ElementAt(0));
                                quartaFascia.RemoveAt(0);
                            }
                            else
                                break;
                        }
                    }
                }

                ///TODO
                ///per ogni girone devo creare la lsita corrispondente degli scontri
                ///- nuova struttura dati dei gironi/scontri che ogni posizione contiene una lista di incontri
                ///- tali incontri vanno salvati sul DB
                ///- tali incontri dovranno, in futuro, essere anche visualizzati via WEB
                ///
                gironiIncontri = new List<List<MatchEntity>>();

                tabControlPool.Items.Clear();

                Int32 idGirone = 1;

                foreach (List<AtletaEntity> g in gironi)
                {
                    List<MatchEntity> matchList = null;

                    if (g.Count == 4)
                        matchList = Helper.ElaborateT4(g);
                    else if (g.Count == 5)
                        matchList = Helper.ElaborateT5(g);
                    else if (g.Count == 6)
                        matchList = Helper.ElaborateT6(g);

                    gironiIncontri.Add(matchList);
                    string title = "Girone " + (tabControlPool.Items.Count + 1).ToString();
                    tabControlPool.Items.Add(ElaboraTab(title, g, matchList, tabControlPool.Items.Count + 1));

                    //TODO l'inserimento va fatto solo se già non è stato fatto, altrimenti vanno eliminati TUTTI i dati
                    Helper.InserisciGironiIncontri(idTorneo, idDisciplina, matchList, idGirone);

                    foreach (AtletaEntity a in g)
                        Helper.InsertAtletaInGirone(creaGironi.IdTorneo, creaGironi.IdDisciplina, idGirone, a.IdAtleta);

                    idGirone++;
                }

            }
            else
            {
                MessageBox.Show("Si è verificato un errore durante il recupero delle informazioni sul numero dei gironi \r\nContattare un amministratore",
                                "ERRORE Applicazione", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                //save all pools for safety
                if (SaveAll())
                {
                    Helper.DeleteAllPahases(tournamentId, disciplineId);

                    SetPhasesIndex();

                    Window validaAtleti = new CheckResult(caricaGironi.IdTorneo, caricaGironi.IdDisciplina, atletiAmmessiEliminatorie);
                    validaAtleti.Closing += new CancelEventHandler(creaEliminatorie_FormClosed);
                    validaAtleti.Show();
                }
                else
                {
                    MessageBox.Show("Si è verificato un errore durante il salvataggio dei gironi \r\nContattare un amministratore",
                                "ERRORE Applicazione", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void SetPhasesIndex()
        {

            //TODO calcolo dei valori presi da DB
            atletiAmmessiEliminatorie = numeroAtletiTorneoDisciplina >= 54 ? 32 :
                        numeroAtletiTorneoDisciplina >= 24 ? 16 :
                        numeroAtletiTorneoDisciplina >= 12 ? 8 : 4;


            /*
             * 0 = 1/16
             * 1 = 1/8
             * 2 = 1/4
             * 3 = semifinals
             * 4 = finals
             */
            //TODO calcolo dei valori presi da DB
            startPhase = atletiAmmessiEliminatorie == 32 ? (int)PhasesType.Finals_16 :
                        atletiAmmessiEliminatorie == 16 ? (int)PhasesType.Finals_8 :
                        atletiAmmessiEliminatorie == 8 ? (int)PhasesType.Finals_4 :
                        atletiAmmessiEliminatorie == 4 ? (int)PhasesType.SemiFinals : (int)PhasesType.Finals;
        }

        private bool SaveAll()
        {
            bool saveAllResult = true;
            for (int i = 0; i < tabControlPool.Items.Count; i++)
            {
                tabControlPool.SelectedIndex = i;
                Pool p = (Pool)tabControlPool.SelectedContent;
                saveAllResult = saveAllResult && p.SavePoolFromFather();
            }

            return saveAllResult;
        }

        private void creaEliminatorie_FormClosed(object sender, CancelEventArgs e)
        {
            CheckResult result = (sender as CheckResult);

            if (result.WindowCheckResult)
            {
                FinalsTransitions finals = new FinalsTransitions(startPhase);
                
                finals.Show();
            }
            else
            {

            }
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

        private void btnOpen32th_Click(object sender, RoutedEventArgs e)
        {
            FinalsTransitions finals = new FinalsTransitions((int)PhasesType.Finals_32);

            finals.Show();
        }

        private void BtnOpen16th_Click(object sender, RoutedEventArgs e)
        {
            FinalsTransitions finals = new FinalsTransitions((int)PhasesType.Finals_16);

            finals.Show();
        }

        private void BtnOpen8th_Click(object sender, RoutedEventArgs e)
        {
            FinalsTransitions finals = new FinalsTransitions((int)PhasesType.Finals_8);

            finals.Show();
        }

        private void BtnOpen4th_Click(object sender, RoutedEventArgs e)
        {
            FinalsTransitions finals = new FinalsTransitions((int)PhasesType.Finals_4);

            finals.Show();
        }

        private void BtnOpenSemifinal_Click(object sender, RoutedEventArgs e)
        {
            FinalsTransitions finals = new FinalsTransitions((int)PhasesType.SemiFinals);

            finals.Show();
        }

        private void btnOpenFinal_Click(object sender, RoutedEventArgs e)
        {
            FinalsTransitions finals = new FinalsTransitions((int)PhasesType.Finals);

            finals.Show();
        }

        #endregion

        
    }
}
