using BusinessEntity.Entity;
using Resources;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace UserControls.Phases
{
    /// <summary>
    /// Interaction logic for Finals4.xaml
    /// </summary>
    public partial class Finals4 : UserControl, IFinalsPhase
    {
        private int _id_torneo;
        private int _idDisciplina;

        public Finals4()
        {
            InitializeComponent();
        }

        public void LoadFields(int idTorneo, int idDisciplina)
        {
            _id_torneo = idTorneo;
            _idDisciplina = idDisciplina;

            //TODO da lettura da DB
            List<MatchEntity> listaIncontri = new List<MatchEntity>
            {
                new MatchEntity { IdRosso = 1, IdBlu = 2, CognomeRosso = "Aaaaaa", CognomeBlu = "Bbbbbbbb", NomeRosso = "N", NomeBlu="N", DoppiaMorte=false, PuntiRosso = 0, PuntiBlu=0, SatrapiaRosso="", SatrapiaBlu=""},
            };

            dataGridPoolOne.ItemsSource = listaIncontri;
            dataGridPoolTwo.ItemsSource = listaIncontri;
            dataGridPoolThree.ItemsSource = listaIncontri;
            dataGridPoolFour.ItemsSource = listaIncontri;

        }

        public void SaveFields(int idTorneo, int idDisciplina)
        {
            
        }
        public void PrintBracket()
        {
            throw new NotImplementedException();
        }

        public void PrintPools()
        {
            throw new NotImplementedException();
        }

        private void btnSavePools_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dataGridPool_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            Style horizontalAlignment = new Style();
            horizontalAlignment.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));


            switch (e.Column.Header.ToString())
            {
                case "IdBlu":
                case "IdRosso":
                case "SatrapiaRosso":
                case "SatrapiaBlu":
                case "DoppiaMorte":
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
                    e.Column.IsReadOnly = true;
                    break;
            }
        }
    }
}
