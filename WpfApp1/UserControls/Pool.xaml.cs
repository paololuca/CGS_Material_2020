using BusinessEntity.DAO;
using BusinessEntity.Entity;
using HEMATournamentSystem.Engine;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly int poolIndex;
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
            int poolIndex)
        {
            InitializeComponent();

            this.poolIndex = poolIndex;
            this.idTorneo = idTorneo;
            this.idDisciplina = idDisciplina;

            this.listaAtleti = listaAtleti;
            this.listaIncontri = SetIndex(listaIncontri);

            this.Header = "Girone " + poolIndex;

            SetPool();
        }

        #region Private Methods
        private List<MatchEntity> SetIndex(List<MatchEntity> listaIncontri)
        {
            int index = 1;
            foreach (var i in listaIncontri)
                i.M = index++;

            return listaIncontri;
        }

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

        #endregion


        public List<MatchEntity> GetListaIncontri()
        {
            return listaIncontri;
        }

        public List<AtletaEntity> GetListaAtleti()
        {
            return listaAtleti;
        }

        public bool SavePoolFromFather()
        {
            try
            {
                SavePool();
                return true;
            }
            catch
            {
                return false;
            }
        }

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

        private void BtnSavePool_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SavePool();
                new MessageBoxCustom("Pool " + poolIndex + " saved", MessageType.Success, MessageButtons.Ok).ShowDialog();
            }
            catch
            {
                new MessageBoxCustom("Error during saving pool " + poolIndex, MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
        }

        private void SavePool()
        {
            string labelIdAtleti = hiddenId.Content.ToString();
            List<Int32> idAtleti = labelIdAtleti.Split(';').Select(Int32.Parse).ToList();

            int numeroIncontriAdPersonam = idAtleti.Count - 1;

            AscEngine.SaveTournamentPool(idTorneo, idDisciplina, poolIndex, idAtleti, numeroIncontriAdPersonam, dataGridPool);

            UpdateHemaSite();

        }

        private void UpdateHemaSite()
        {
            SqlDal_HemaSiteMongoDB.UpdateStatistics(idTorneo, idDisciplina, poolIndex);

            SqlDal_HemaSiteMongoDB.UpdatePoolsMatchs(idTorneo, poolIndex, dataGridPool);
        }

    }
}
