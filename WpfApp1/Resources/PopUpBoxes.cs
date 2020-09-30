using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HEMATournamentSystem;
using MaterialDesignThemes.Wpf;

namespace Resources
{
    static class PopUpBoxes
    {
        public static void ShowPopup(string message)
        {
            var sampleMessageDialog = new SampleMessageDialog
            {
                Message = { Text = (message) }
            };

            DialogHost.Show(sampleMessageDialog, "RootDialog");
        }
    }
}
