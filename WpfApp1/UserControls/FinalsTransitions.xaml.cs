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

        private const int MAX_TRANSITIONER_INDEX = 4;
        private int currentTransition = 0;
        private int firstTransitionValid = 0;

        public FinalsTransitions(int initialPhase)  //TODO initiale transitioner, e non posso andare indietro, MAI
        {
            InitializeComponent();

            SetInitialIndexs(initialPhase);

            SetButtonVisibility();
        }

        private void SetInitialIndexs(int initialPhase)
        {
            firstTransitionValid = initialPhase;
            currentTransition = initialPhase;
            finalTransition.SelectedIndex = currentTransition;
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            currentTransition--;
            finalTransition.SelectedIndex = currentTransition;

            //TODO check che io possa leggere la fase successiva (i.e. esistono i dati nel DB)
            var panel = (IFinalsPhase)finalTransition.SelectedItem;
            panel.LoadField();

            SetButtonVisibility();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            currentTransition++;
            finalTransition.SelectedIndex = currentTransition;

            //TODO check che io possa leggere la fase successiva (i.e. esistono i dati nel DB)
            var panel = (IFinalsPhase)finalTransition.SelectedItem;
            panel.LoadField();

            SetButtonVisibility();

        }

        private void SetButtonVisibility()
        {
            btnPrevious.IsEnabled = currentTransition == firstTransitionValid ? false : true;
            btnNext.IsEnabled = currentTransition == MAX_TRANSITIONER_INDEX ? false : true;
        }
    }
}
