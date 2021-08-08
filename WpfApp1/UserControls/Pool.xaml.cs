using BusinessEntity.Entity;
using Resources;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace HEMATournamentSystem
{
    /// <summary>
    /// Interaction logic for Pool.xaml
    /// </summary>
    public partial class Pool : UserControl
    {

        private List<AtletaEntity> listaAtleti;
        private List<MatchEntity> listaIncontri;
        private string Header;
        private readonly int tabIndex;
        private readonly int idTorneo;
        private readonly int idDisciplina;

        public Pool()
        {
            InitializeComponent();
        }

        public Pool(
            int idTorneo,
            int idDisciplina,
            List<AtletaEntity> listaAtleti,
            List<MatchEntity> listaIncontri,
            int tabIndex)
        {
            InitializeComponent();

            this.tabIndex = tabIndex;
            this.idTorneo = idTorneo;
            this.idDisciplina = idDisciplina;

            this.listaAtleti = listaAtleti;
            this.listaIncontri = listaIncontri;

            this.Header = "Girone " + tabIndex;

            SetPool();
        }

        #region Private Methods
        private void SetPool()
        {
            foreach (AtletaEntity a in listaAtleti)
            {
                lblListaAtleti.Content += "[" + a.Asd + "] " + a.Cognome + " " + a.Nome + "\r\n";
                hiddenId.Content += a.IdAtleta + ";";
            }


            hiddenId.Content = hiddenId.Content.ToString().Substring(0, hiddenId.Content.ToString().Length - 1);


            if (listaIncontri != null)
            {
                dataGridPool.ItemsSource = listaIncontri;
            }
        }

        private void saveGirone_Click(object sender, EventArgs e)
        { }

        //private void saveGirone_Click(object sender, EventArgs e)
        //{


        //    //if (debug)
        //    //    textBox1.AppendText("Selezionato il tab " + indexGirone.ToString() + "\r\n");


        //    string labelIdAtleti = hiddenId.Content.ToString();
        //    List<Int32> idAtleti = labelIdAtleti.Split(';').Select(Int32.Parse).ToList();

        //    //if (debug)
        //    //    textBox1.AppendText("Numero incontri : " + dtView.RowCount.ToString() + "\r\n");

        //    /** dtgrid index columns
        //     * [0] IdA
        //     * [1] AsdA
        //     * [2] CognomeA
        //     * [3] NomeA
        //     * [4] PuntiA
        //     * [5] IdB
        //     * [6] AdB
        //     * [7] CognomeB
        //     * [8] NomeB
        //     * [9] PuntiB
        //     * [10] DoppiaMorte 
        //     * 
        //     * */


        //    //if (debug)
        //    //    foreach (DataGridViewRow r in dtView.Rows)
        //    //    {
        //    //        textBox1.AppendText("Incontro : " +
        //    //                            r.Cells[0].Value + " " + r.Cells[2].Value +
        //    //                            " VS " +
        //    //                            r.Cells[5].Value + " " + r.Cells[7].Value +
        //    //                            "[ " + r.Cells[4].Value + " : " + r.Cells[9].Value + " ]" + "\r\n");
        //    //
        //    //    }

        //    List<RisultatiIncontriGironi> risultati = new List<RisultatiIncontriGironi>();

        //    int numeroIncontriAdPersonam = idAtleti.Count - 1;

        //    foreach (Int32 atleta in idAtleti)
        //    {
        //        RisultatiIncontriGironi res = new RisultatiIncontriGironi();

        //        res.idAtleta = atleta;

        //        foreach (DataGridViewRow r in dataGridPool.row)
        //        {
        //            bool doppiaMorte = (bool)r.Cells[10].Value;

        //            if (!Helper.UpdateGironiIncontri(idTorneo, idDisciplina, indexGirone + 1, (int)r.Cells[0].Value, (int)r.Cells[4].Value, (int)r.Cells[5].Value, (int)r.Cells[9].Value, doppiaMorte))
        //                System.Windows.MessageBox.Show("Errore di salvataggio nella tabella GironiIncontri:\n\n non ti preoccupare funziona uguale, ma chiama PL", "ATTENZIONE: PL REQUIRED", MessageBoxButton.OK, MessageBoxImage.Error);

        //            if ((int)r.Cells[0].Value == atleta) //se sono l'atleta a "sinistra"
        //            {
        //                if (doppiaMorte)
        //                {
        //                    res.Sconfitte++;
        //                    res.PuntiSubiti += Math.Abs(3 - (int)r.Cells[4].Value);
        //                }
        //                else
        //                {
        //                    //HO VINTO con 3 o più punti
        //                    if (((int)r.Cells[4].Value >= 3) && (int)r.Cells[4].Value > (int)r.Cells[9].Value)
        //                    {
        //                        res.Vittorie++;
        //                        res.PuntiFatti += (int)r.Cells[4].Value;// > 3 ? 3 : (int)r.Cells[4].Value;
        //                        res.PuntiSubiti += (int)r.Cells[9].Value;
        //                    }

        //                    //HO PERSO (l'avversario ha vinto con più di tre punti
        //                    else if (((int)r.Cells[4].Value < (int)r.Cells[9].Value) && ((int)r.Cells[9].Value >= 3))
        //                    {
        //                        res.PuntiFatti += (int)r.Cells[4].Value;
        //                        res.PuntiSubiti += (int)r.Cells[9].Value;// > 3 ? 3 : (int)r.Cells[9].Value;
        //                        res.Sconfitte++;
        //                    }

        //                    else if (((int)r.Cells[9].Value < 3) && ((int)r.Cells[4].Value < 3))
        //                    {
        //                        if ((int)r.Cells[4].Value > (int)r.Cells[9].Value)
        //                            res.Vittorie++;
        //                        else if ((int)r.Cells[4].Value < (int)r.Cells[9].Value)
        //                            res.Sconfitte++;

        //                        res.PuntiFatti += (int)r.Cells[4].Value;
        //                        res.PuntiSubiti += (int)r.Cells[9].Value;

        //                    }
        //                }
        //            }
        //            else if ((int)r.Cells[5].Value == atleta)   //se sono l'atleta a "destra"
        //            {
        //                if (doppiaMorte)
        //                {
        //                    res.Sconfitte++;
        //                    res.PuntiSubiti += Math.Abs(3 - (int)r.Cells[9].Value);
        //                }

        //                else
        //                {
        //                    //HO vinto con più di 3 punti
        //                    if (((int)r.Cells[9].Value > (int)r.Cells[4].Value) && ((int)r.Cells[9].Value >= 3))
        //                    {
        //                        res.Vittorie++;
        //                        res.PuntiFatti += (int)r.Cells[9].Value;// > 3 ? 3 : (int)r.Cells[9].Value;
        //                        res.PuntiSubiti += (int)r.Cells[4].Value;
        //                    }
        //                    //HO PERSO (l'avversario ha vinto con più di tre punti
        //                    else if (((int)r.Cells[9].Value < (int)r.Cells[4].Value) && ((int)r.Cells[4].Value >= 3))
        //                    {
        //                        res.PuntiFatti += (int)r.Cells[9].Value;
        //                        res.PuntiSubiti += (int)r.Cells[4].Value;// > 3 ? 3 : (int)r.Cells[4].Value;
        //                        res.Sconfitte++;
        //                    }

        //                    else if (((int)r.Cells[4].Value < 3) && ((int)r.Cells[9].Value < 3))
        //                    {
        //                        if ((int)r.Cells[4].Value < (int)r.Cells[9].Value)
        //                            res.Vittorie++;
        //                        else if ((int)r.Cells[4].Value > (int)r.Cells[9].Value)
        //                            res.Sconfitte++;

        //                        res.PuntiFatti += (int)r.Cells[9].Value;
        //                        res.PuntiSubiti += (int)r.Cells[4].Value;
        //                    }
        //                }
        //            }
        //        }

        //        int delpaP = res.PuntiFatti - res.PuntiSubiti;
        //        res.NumeroIncontriDisputati = numeroIncontriAdPersonam;

        //        res.Differenziale = (Double)(delpaP + res.Vittorie) / res.NumeroIncontriDisputati;

        //        //salvare res in Gironi:
        //        //per ogni atleta , torneo e disciplina salvo i punti fatti, subiti, le vittorie ed il differenziale
        //        Helper.UpdateGironi(res, idTorneo, idDisciplina, indexGirone + 1);

        //        risultati.Add(res);
        //    }

        //    System.Windows.MessageBox.Show("Girone " + (indexGirone + 1) + " salvato correttamente", "Completato", MessageBoxButton.OK, MessageBoxImage.Information);
        //}
        #endregion

        private void DataGridPool_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            Style horizontalAlignment = new Style();
            horizontalAlignment.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));


            switch (e.Column.Header.ToString())
            {
                case "IdBlu":
                case "IdRosso":
                case "SatrapiaRosso":
                case "SatrapiaBlu":
                    e.Column.CanUserSort = false;
                    e.Column.Visibility = Visibility.Hidden;
                    break;
                case "PuntiRosso":
                case "PuntiBlu":
                    e.Column.CanUserSort = false;
                    e.Column.Visibility = Visibility.Visible;
                    e.Column.CellStyle = horizontalAlignment;
                    break;
                default:
                    e.Column.CanUserSort = false;
                    e.Column.Visibility = Visibility.Visible;
                    break;
            }
        }

        public bool SavePoolFromFather()
        {
            //MessageBox.Show("Pool " + Header + " Saved");
            return true;
        }

        private void BtnSavePool_Click(object sender, RoutedEventArgs e)
        {
            PopUpBoxes.ShowPopup("Pool " + Header + " Saved");
        }
    }
}
