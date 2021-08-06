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

        /*
         * 0 = 1/16
         * 1 = 1/8
         * 2 = 1/4
         * 3 = semifinals
         * 4 = finals
         */
        private const int MAX_TRANSITIONER_INDEX = 4;
        private int _currentTransition = 0;
        private int _firstTransitionValid = 0;

        public FinalsTransitions(int initialPhase)  //TODO initiale transitioner, e non posso andare indietro, MAI
        {
            InitializeComponent();

            SetInitialIndexs(initialPhase);

            SetButton();
        }

        private void SetInitialIndexs(int initialPhase)
        {
            _firstTransitionValid = initialPhase;
            _currentTransition = initialPhase;
            finalTransition.SelectedIndex = _currentTransition;
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            _currentTransition--;
            finalTransition.SelectedIndex = _currentTransition;


            //TODO check che io possa leggere la fase successiva (i.e. esistono i dati nel DB)
            var panel = (IFinalsPhase)finalTransition.SelectedItem;
            panel.LoadField();

            SetButton();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            _currentTransition++;
            finalTransition.SelectedIndex = _currentTransition;

            //TODO check che io possa leggere la fase successiva (i.e. esistono i dati nel DB)
            var panel = (IFinalsPhase)finalTransition.SelectedItem;
            panel.LoadField();

            SetButton();

        }

        private void SetButton()
        {
            btnPrevious.IsEnabled = _currentTransition == 0 ? false : true;
            btnNext.IsEnabled = _currentTransition == MAX_TRANSITIONER_INDEX ? false : true;
        }
    }
}
