using BusinessEntity.Type;
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
using System.Windows.Shapes;
using UserControls.Phases;

namespace HEMATournamentSystem
{
    /// <summary>
    /// Interaction logic for FinalsTransitions.xaml
    /// </summary>
    public partial class FinalsTransitions : Window
    {

        private const int MAX_TRANSITIONER_INDEX = (int)PhasesType.Finals;
        private int firstTransitionValid = (int)PhasesType.Finals_32;
        private int currentTransition = 0;
        
        private int _idTorneo;
        private int _idDisciplina;

        public FinalsTransitions(int initialPhase, int idTorneo, int idDisciplina)  //TODO initiale transitioner, e non posso andare indietro, MAI
        {
            _idTorneo = idTorneo;
            _idDisciplina = idDisciplina;

            InitializeComponent();

            SetInitialIndexs(initialPhase);

            SetButtonVisibility();

        }

        private void SetInitialIndexs(int initialPhase)
        {
            firstTransitionValid = initialPhase;
            currentTransition = initialPhase;
            finalTransition.SelectedIndex = currentTransition;
            ((IFinalsPhase)finalTransition.SelectedItem).LoadFields(_idTorneo, _idDisciplina);

        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            currentTransition--;

            SwitchPhase();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            currentTransition++;

            SwitchPhase();
        }

        private void SwitchPhase()
        {
            var currentPanel = (IFinalsPhase)finalTransition.SelectedItem;
            currentPanel.SaveFields(_idTorneo, _idDisciplina);

            finalTransition.SelectedIndex = currentTransition;

            //TODO check che io possa leggere la fase successiva (i.e. esistono i dati nel DB)
            var destinantionPanel = (IFinalsPhase)finalTransition.SelectedItem;
            destinantionPanel.LoadFields(_idTorneo, _idDisciplina);

            SetButtonVisibility();
        }

        

        private void SetButtonVisibility()
        {
            btnPrevious.IsEnabled = currentTransition == firstTransitionValid ? false : true;
            btnNext.IsEnabled = currentTransition == MAX_TRANSITIONER_INDEX ? false : true;

            btnPrintBracket.IsEnabled = currentTransition > 2 ? false : true;
        }

        private void btnPrintPools_Click(object sender, RoutedEventArgs e)
        {
            var currentPanel = (IFinalsPhase)finalTransition.SelectedItem;
            currentPanel.PrintPools();
        }

        private void btnPrintBracket_Click(object sender, RoutedEventArgs e)
        {
            var currentPanel = (IFinalsPhase)finalTransition.SelectedItem;
            currentPanel.PrintBracket();
        }
    }
}
