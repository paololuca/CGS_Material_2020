using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HEMATournamentSystem
{

    /// <summary>
    /// Interaction logic for MessaggeBoxCustom.xaml
    /// </summary>
    public partial class LoadingCustom : Window
    {
        private int progressBarValue;

        public LoadingCustom()
        {
            InitializeComponent();

            progressBarValue = 0;

            this.progressBar.Value = 0;
        }                

        public void IncrementProgressBar(int addedValue)
        {
            progressBarValue += addedValue;

            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(delegate () {
                progressBar.Value += addedValue;
                // Only sleeping to artificially simulate a long running operation
                Thread.Sleep(100);
            }), DispatcherPriority.Background);
        }

    }
    
}
