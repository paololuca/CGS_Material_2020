using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserControls.Phases
{
    /// <summary>
    /// Interaction logic for Finals32.xaml
    /// </summary>
    public partial class Finals32 : UserControl, IFinalsPhase
    {
        private int _id_torneo;
        private int _idDisciplina;

        public Finals32()
        {
            InitializeComponent();
        }

        public void LoadFields(int idTorneo, int idDisciplina)
        {
            _id_torneo = idTorneo;
            _idDisciplina = idDisciplina;
        }

        public void SaveFields(int idTorneo, int idDisciplina)
        {
            
        }

        private void btnSavePools_Click(object sender, RoutedEventArgs e)
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
