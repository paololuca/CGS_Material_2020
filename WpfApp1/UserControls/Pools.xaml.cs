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
using System.Windows.Input;

namespace HEMATournamentSystem
{
    /// <summary>
    /// Interaction logic for Pools.xaml
    /// </summary>
    public partial class Pools : UserControl
    {
        private int _tournamentId;
        private int _disciplineId;
        private string _tournamentName;
        private string _disciplineName;

        private CaricaGironiDaDisciplina caricaGironi = null;
        private CreaGironiDaDisciplina creaGironi = null;

        List<List<AtletaEntity>> gironi = new List<List<AtletaEntity>>();
        
        //Lista degli scontri per girone
        List<List<MatchEntity>> gironiIncontri = null;

        private int numeroAtletiTorneoDisciplina = 0;
        private Dictionary<string, int> _dicFighter;
        private int numeroGironi = 0;
        private int atletiAmmessiEliminatorie;
        private int startPhase = 0;

        private readonly LoginUser user;
        private readonly ProfileManager profileManager;

        public Pools(LoginUser user)
        {
            this.user = user;
            this.profileManager = new ProfileManager();

            InitializeComponent();

            EnableCreateControls();
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
                        _tournamentName = creaGironi.NomeTorneo;
                        _disciplineName = creaGironi.NomeDisciplina;

                        _tournamentId = creaGironi.IdTorneo;
                        _disciplineId = creaGironi.IdDisciplina;
                                                
                        creaGironiAndLoad(creaGironi.IdTorneo, creaGironi.IdDisciplina);
                        lblTitle.Text = creaGironi.NomeTorneo + " - " + creaGironi.NomeDisciplina;
                    }
                }
            }
            else if ((sender as System.Windows.Forms.Form).DialogResult == System.Windows.Forms.DialogResult.Abort)
            {
                // Then assume that Cancel Button has been clicked and act accordingly.)
            }
        }

        private void creaGironiAndLoad(int idTorneo, int idDisciplina)
        {
            _dicFighter = new Dictionary<string, int>();

            //loadToolStripMenuItem.Enabled = false;      //Non permetto più di caricare i dati (in teoria va fatto meglio)

            //partecipantiTorneo = CaricaAtleti();

            // i dati in lettura vanno fatti caricando la disciplina dal DB (divisione per disciplina)
            //se sono in presenza degli assoluti, per ora, carico i dati ordinati solo per ranking e senza random dei nomi all'interno delle ASD
            //partecipantiTorneo = assoluti == false ?
            //                                    Helper.GetAtletiTorneoVsDisciplina(idTorneo, idDisciplina, categoria) :
            //                                    Helper.GetAtletiTorneoVsDisciplinaAssoluti(idTorneo, idDisciplina, categoria);

            SqlDal_Pools.DeletePoolsAndMatches(idTorneo, idDisciplina);
            
            SqlDal_Pools.DeleteAllPahases(idTorneo, idDisciplina);

            var partecipantiTorneo = SqlDal_Tournaments.GetAtletiTorneoVsDisciplinaAssoluti(idTorneo, idDisciplina);

            //TODO: da parametrizzare

            bool rankingEnabled = partecipantiTorneo.Sum(x => x.Ranking) != 0;

            numeroGironi = SqlDal_Pools.GetNumeroGironiByTorneoDisciplina(idTorneo, idDisciplina);            


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
                    tabControlPool.Items.Add(ElaboraTab(idTorneo, idDisciplina, title, g, matchList, tabControlPool.Items.Count + 1));

                    //TODO l'inserimento va fatto solo se già non è stato fatto, altrimenti vanno eliminati TUTTI i dati
                    SqlDal_Pools.InserisciGironiIncontri(idTorneo, idDisciplina, matchList, idGirone);

                    foreach (AtletaEntity a in g)
                        SqlDal_Pools.InsertAtletaInGirone(creaGironi.IdTorneo, creaGironi.IdDisciplina, idGirone, a.IdAtleta);

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
                        _tournamentName = caricaGironi.NomeTorneo;
                        _disciplineName = caricaGironi.NomeDisciplina;
                        
                        _tournamentId = caricaGironi.IdTorneo;
                        _disciplineId = caricaGironi.IdDisciplina;
                        

                        CaricaGironiCreati(caricaGironi.IdTorneo, caricaGironi.IdDisciplina);
                        lblTitle.Text = caricaGironi.NomeTorneo + " - " + caricaGironi.NomeDisciplina;
                    }
                }
            }
            else if ((sender as System.Windows.Forms.Form).DialogResult == System.Windows.Forms.DialogResult.Abort)
            {
                // Then assume that Cancel Button has been clicked and act accordingly.)
            }
        }

        private void CaricaGironiCreati(int idTorneo, int idDisciplina)
        {
            _dicFighter = new Dictionary<string, int>();

            numeroGironi = SqlDal_Pools.GetNumeroGironiByTorneoDisciplina(idTorneo, idDisciplina);

            if (numeroGironi > 0)
            {
                gironi = new List<List<AtletaEntity>>();
                gironi = SqlDal_Pools.GetGironiSalvati(idTorneo, idDisciplina);

                numeroAtletiTorneoDisciplina = gironi.SelectMany(list => list).Distinct().Count();

                gironiIncontri = new List<List<MatchEntity>>();

                tabControlPool.Items.Clear();

                Int32 idGirone = 1;

                foreach (List<AtletaEntity> poolList in gironi)
                {
                    List<MatchEntity> matchList = null;

                    //TODO eliminabile visto che sono già sul DB
                    if (poolList.Count == 4)
                        matchList = Helper.ElaborateT4(poolList);
                    else if (poolList.Count == 5)
                        matchList = Helper.ElaborateT5(poolList);
                    else if (poolList.Count == 6)
                        matchList = Helper.ElaborateT6(poolList);

                    if (matchList != null)
                    {
                        foreach (MatchEntity i in matchList)
                            SqlDal_Pools.CaricaPunteggiEsistentiGironiIncontri(idTorneo, idDisciplina, i, idGirone);

                        gironiIncontri.Add(matchList);
                        string title = "Girone " + (tabControlPool.Items.Count + 1).ToString();
                        tabControlPool.Items.Add(ElaboraTab(idTorneo, idDisciplina, title, poolList, matchList, tabControlPool.Items.Count + 1));
                        
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


        private void EnableCreateControls()
        {
            if (!profileManager.CanCreatePools(user.Type))
                btnCreateTournamentPools.IsEnabled = false;
            else
                btnCreateTournamentPools.IsEnabled = true;
        }

        private void EnablePageControls()
        {
            btnClosePools.IsEnabled = true;
            btnExportMatch.IsEnabled = true;
            btnExportPools.IsEnabled = true;
            cmbSearchFighter.IsEnabled = true;

            ManagePhasesButtons();

            cmbSearchFighter.ItemsSource = _dicFighter.Keys.ToList().OrderBy(x => x);

            tabControlPool.SelectedIndex = 0;
        }

        private void ManagePhasesButtons()
        {
            btnLoadPhases.IsEnabled = true;

            btnOpen32th.IsEnabled = false; //for nnow is always disabled
            btnOpen16th.IsEnabled = SqlDal_Pools.CountPhasesMatchs(_tournamentId, _disciplineId, PhasesManager.Decode(PhasesType.Finals_16)) == 0 ? false : true;
            btnOpen8th.IsEnabled = SqlDal_Pools.CountPhasesMatchs(_tournamentId, _disciplineId, PhasesManager.Decode(PhasesType.Finals_8)) == 0 ? false : true;
            btnOpen4th.IsEnabled = SqlDal_Pools.CountPhasesMatchs(_tournamentId, _disciplineId, PhasesManager.Decode(PhasesType.Finals_4)) == 0 ? false : true;
            btnOpenSemifinal.IsEnabled = SqlDal_Pools.CountPhasesMatchs(_tournamentId, _disciplineId, PhasesManager.Decode(PhasesType.SemiFinals)) == 0 ? false : true;
            btnOpenFinal.IsEnabled = SqlDal_Pools.CountPhasesMatchs(_tournamentId, _disciplineId, PhasesManager.Decode(PhasesType.Finals)) == 0 ? false : true;
        }

        /// <summary>
        /// Costruisce il tab dello specifico del girone
        /// </summary>
        /// <param name="title">Titolo del TAB</param>
        /// <param name="poolList">lista dei giroi (lista di persone)</param>
        /// <param name="matchList">Lista degli incontri</param>
        /// <returns></returns>
        private TabItem ElaboraTab(int idTorneo, int idDisciplina, string title, List<AtletaEntity> poolList, List<MatchEntity> matchList, Int32 tabIndex)
        {
            foreach (var a in poolList)
                _dicFighter.Add(a.FullName, tabIndex);

            TabItem item = new TabItem();

            item.Header = title;

            item.Content = new Pool(idTorneo, idDisciplina, poolList, matchList, tabIndex);

            return item;
        }

        private void BtnClosePools_Click(object sender, RoutedEventArgs e)
        {
            //TODO PL chiamata alla "validaEliminatorie" in cui deve essere gestito il motore tramite DB
            bool? result = new MessageBoxCustom("Delete all final phases?", 
                MessageType.Warning, MessageButtons.OkCancel).ShowDialog();

            if (result.Value)
            {
                //save all pools for safety
                if (SaveAllPools())
                {
                    SqlDal_Pools.DeleteAllPahases(_tournamentId, _disciplineId);

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
            //TODO atletiAmmessiEliminatorie = QualificationManager.GetAdmittedFightersNumber(numeroAtletiTorneoDisciplina)
            atletiAmmessiEliminatorie = numeroAtletiTorneoDisciplina >= 54 ? 32 :
                        numeroAtletiTorneoDisciplina >= 24 ? 16 :
                        numeroAtletiTorneoDisciplina >= 12 ? 8 : 4;

            startPhase = PhasesManager.GetStartingPhase(atletiAmmessiEliminatorie);
        }

        private bool SaveAllPools()
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
                FinalsTransitions finals = new FinalsTransitions(startPhase, _tournamentId, _disciplineId);
                
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
            pdf.StampaGironi(gironi, _tournamentName, _disciplineName);

            //DynamicsPdfManager dManger = new DynamicsPdfManager();
            //dManger.StampaGironi(gironi, _tournamentName, _disciplineName);
        }

        private void BtnExportMatch_Click(object sender, RoutedEventArgs e)
        {
            PdfManager pdf = new PdfManager();
            pdf.StampaGironiConIncontri(gironi, _tournamentName, _disciplineName);
        }
        #endregion

        #region open intermediate phases

        private void btnOpen32th_Click(object sender, RoutedEventArgs e)
        {

            FinalsTransitions finals = new FinalsTransitions((int)PhasesType.Finals_32, _tournamentId, _disciplineId);

            finals.Show();
        }

        private void BtnOpen16th_Click(object sender, RoutedEventArgs e)
        {
            FinalsTransitions finals = new FinalsTransitions((int)PhasesType.Finals_16, _tournamentId, _disciplineId);

            finals.Show();
        }

        private void BtnOpen8th_Click(object sender, RoutedEventArgs e)
        {
            FinalsTransitions finals = new FinalsTransitions((int)PhasesType.Finals_8, _tournamentId, _disciplineId);

            finals.Show();
        }

        private void BtnOpen4th_Click(object sender, RoutedEventArgs e)
        {
            FinalsTransitions finals = new FinalsTransitions((int)PhasesType.Finals_4, _tournamentId, _disciplineId);

            finals.Show();
        }

        private void BtnOpenSemifinal_Click(object sender, RoutedEventArgs e)
        {
            FinalsTransitions finals = new FinalsTransitions((int)PhasesType.SemiFinals, _tournamentId, _disciplineId);

            finals.Show();
        }

        private void btnOpenFinal_Click(object sender, RoutedEventArgs e)
        {
            FinalsTransitions finals = new FinalsTransitions((int)PhasesType.Finals, _tournamentId, _disciplineId);

            finals.Show();
        }


        #endregion

        private void cmbSearchFighter_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SwitchPoolTab(sender);
            }
        }

        private void cmbSearchFighter_DropDownClosed(object sender, EventArgs e)
        {
            SwitchPoolTab(sender);
        }

        private void SwitchPoolTab(object sender)
        {
            ComboBox comboBox = (ComboBox)sender;

            // Save the selected employee's name, because we will remove
            // the employee's name from the list.
            string selectedFigther = (string)cmbSearchFighter.SelectedItem;

            if (selectedFigther != null && _dicFighter.TryGetValue(selectedFigther, out int tab))
            {
                tabControlPool.SelectedIndex = tab - 1;  //tabControlPool index start from 0
            }
        }

        private void btnLoadPhases_MouseEnter(object sender, MouseEventArgs e)
        {
            ManagePhasesButtons();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            EnableCreateControls();

            tabControlPool.Items.Clear();
        }
    }
}
