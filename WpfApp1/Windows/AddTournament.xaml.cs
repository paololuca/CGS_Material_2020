using System;
using System.Collections;
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

namespace HEMATournamentSystem
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AddTournament : Window
    {
        private ArrayList myDataList;

        public AddTournament()
        {
            InitializeComponent();

            // Get data from somewhere and fill in my local ArrayList  
            myDataList = LoadListBoxData();
            // Bind ArrayList with the ListBox  
            remainingDiscipline.ItemsSource = myDataList;
        }

        private ArrayList LoadListBoxData()
        {
            ArrayList itemsList = new ArrayList();
            itemsList.Add("Coffie");
            itemsList.Add("Tea");
            itemsList.Add("Orange Juice");
            itemsList.Add("Milk");
            itemsList.Add("Mango Shake");
            itemsList.Add("Iced Tea");
            itemsList.Add("Soda");
            itemsList.Add("Water");
            return itemsList;
        }

        #region closing
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool? result = new MessageBoxCustom("Confirm Closing ?", MessageType.Warning, MessageButtons.OkCancel).ShowDialog();

            if (result != null && !result.Value)
            {
                e.Cancel = true;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region buttons
        private void AddDiscipline_Click(object sender, RoutedEventArgs e)
        {

            if (remainingDiscipline.SelectedValue == null)
                return;

            // Find the right item and it's value and index  
            var currentItemText = remainingDiscipline.SelectedValue.ToString();
            var currentItemIndex = remainingDiscipline.SelectedIndex;
            selectedDiscipline.Items.Add(currentItemText);
            if (myDataList != null)
            {
                myDataList.RemoveAt(currentItemIndex);
            }
            // Refresh data binding  
            ApplyDataBinding();

        }

        private void RemoveDiscipline_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDiscipline.SelectedValue == null)
                return;

            // Find the right item and it's value and index  
            var currentItemText = selectedDiscipline.SelectedValue.ToString();
            var currentItemIndex = selectedDiscipline.SelectedIndex;
            // Add RightListBox item to the ArrayList  
            myDataList.Add(currentItemText);
            selectedDiscipline.Items.RemoveAt(selectedDiscipline.Items.IndexOf(selectedDiscipline.SelectedItem));
            // Refresh data binding  
            ApplyDataBinding();
        }

        private void ApplyDataBinding()
        {
            remainingDiscipline.ItemsSource = null;
            // Bind ArrayList with the ListBox  
            remainingDiscipline.ItemsSource = myDataList;
        }

        private void btnSaveTournament_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }
}
