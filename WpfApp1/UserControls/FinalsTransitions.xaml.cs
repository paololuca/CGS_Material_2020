using BusinessEntity.Type;
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
        private int _firstTransitionValid = (int)PhasesType.Finals_32;
        private int _currentTransition = 0;
        
        private int _idTorneo;
        private int _idDisciplina;

        public FinalsTransitions(int initialPhase, int idTorneo, int idDisciplina)  //TODO initiale transitioner, e non posso andare indietro, MAI
        {   
            InitializeComponent();

            _idTorneo = idTorneo;
            _idDisciplina = idDisciplina;

            _firstTransitionValid = initialPhase;
            _currentTransition = initialPhase;
            finalTransition.SelectedIndex = _currentTransition;

            LoadPhasesData(initialPhase);

            SetButtonVisibility();

        }

        private void LoadPhasesData(int startPhase)
        {
            int index = 0;

            foreach (IFinalsPhase phase in finalTransition.Items)
            {
                if(index >= startPhase)
                    phase.LoadFields(_idTorneo, _idDisciplina);

                index++;
            }
        }

        #region Switch between transion's objects

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            _currentTransition--;

            SwitchPhase();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            _currentTransition++;

            SwitchPhase();
        }

        private void SwitchPhase()
        {
            finalTransition.SelectedIndex = _currentTransition;
            SetButtonVisibility();
        } 
        #endregion



        private void SetButtonVisibility()
        {
            btnPrevious.IsEnabled = _currentTransition == _firstTransitionValid ? false : true;
            btnNext.IsEnabled = _currentTransition == MAX_TRANSITIONER_INDEX ? false : true;

            btnPrintBracket.IsEnabled = _currentTransition > 2 ? false : true;
        }

        private void btnPrintPools_Click(object sender, RoutedEventArgs e)
        {
            ((IFinalsPhase)finalTransition.SelectedItem).PrintPools();
        }

        private void btnPrintBracket_Click(object sender, RoutedEventArgs e)
        {
            ((IFinalsPhase)finalTransition.SelectedItem).PrintBracket();
        }

        private void btnSavePool_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DeleteNextPhases();

                ((IFinalsPhase)finalTransition.SelectedItem).SaveFields(_idTorneo, _idDisciplina);

                LoadPhasesData(_currentTransition + 1);

                new MessageBoxCustom("Pools saved correctly !! ", MessageType.Success, MessageButtons.Ok).ShowDialog();
            }
            catch
            {
                new MessageBoxCustom("Error occurred during saving", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }

        }

        private void DeleteNextPhases()
        {
            switch (_currentTransition)
            {
                case (int)PhasesType.Finals_32:
                    SqlDal_Pools.DeleteAllPahases(_idTorneo, _idDisciplina);
                    break;
                case (int)PhasesType.Finals_16:
                    SqlDal_Pools.DeleteAfterSedicesimi(_idTorneo, _idDisciplina);
                    break;
                case (int)PhasesType.Finals_8:
                    SqlDal_Pools.DeleteAfterOttavi(_idTorneo, _idDisciplina);
                    break;
                case (int)PhasesType.Finals_4:
                    SqlDal_Pools.DeleteAfterQuarti(_idTorneo, _idDisciplina);
                    break;
                case (int)PhasesType.SemiFinals:
                    SqlDal_Pools.DeleteAfterSeimifinali(_idTorneo, _idDisciplina);
                    break;
                default:
                    break;
            }
        }

        private void btnClosePool_Click(object sender, RoutedEventArgs e)
        {
            bool? result = new MessageBoxCustom("Confirm EXIT ?", MessageType.Warning, MessageButtons.OkCancel).ShowDialog();

            if (result.Value)
            {
                this.Close();
            }
        }
    }
}
