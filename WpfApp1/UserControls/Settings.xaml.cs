using BusinessEntity.Entity;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using Resources;
using System.Data;
using System.Collections;
using System;
using System.Linq;
using System.Text;
using BusinessEntity.Type;

namespace HEMATournamentSystem
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        private readonly LoginUser user;

        public Settings(LoginUser user)
        {
            this.user = user;

            InitializeComponent();

            LoadUsersList();
            LoadSelectedDatabase();
        }

        private void LoadSelectedDatabase()
        {
            DataBaseType db = HelperMasterDB.GetSelectedDB();

            switch (db)
            {
                case DataBaseType.Maschile:
                    rbtMaschile.IsChecked = true;
                    break;
                case DataBaseType.Femminile:
                    rbtFemminile.IsChecked = true;
                    break;
                case DataBaseType.Test:
                    rbtTest.IsChecked = true;
                    break;
                default:
                    break;
            }
        }

        private void LoadUsersList()
        {
            var accountList = HelperMasterDB.GetAccountList();

            dataGridAccount.ItemsSource = accountList;                       
        }

        private void DataGridAccount_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "UserId":
                case "Password":
                case "IsAdmin":
                    e.Column.CanUserSort = false;
                    e.Column.Visibility = Visibility.Hidden;
                    break;
            }            
        }
        

        private void DataGridAccount_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            LoginUser userToModify = ((FrameworkElement)sender).DataContext as LoginUser;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbAccountType.ItemsSource = Enum.GetValues(typeof(ProfileType)).Cast<ProfileType>();
            cmbAccountType.SelectedValue = ProfileType.None;
        }


        #region Buttons
        private void BtnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            LoginUser userToDelete = ((FrameworkElement)sender).DataContext as LoginUser;

            if (System.Windows.Forms.MessageBox.Show("Are you sure you want to delete '" + userToDelete.UserName + "' user account?",
                               "Warning",
                               System.Windows.Forms.MessageBoxButtons.OKCancel,
                               System.Windows.Forms.MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
            {

                if (HelperMasterDB.DeleteUserAccount(userToDelete.UserName))
                {
                    LoadUsersList();
                    PopUpBoxes.ShowPopup("User " + userToDelete.UserName + " deleted ");
                }
                else
                    PopUpBoxes.ShowPopup("ERROR occurred: no User deleted");
            }

        }
        private void BtnSaveAccount_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserName.Text == "")
                PopUpBoxes.ShowPopup("User Name cannot be empty");
            else if (txtPassword.Password == "")
                PopUpBoxes.ShowPopup("Password cannot be empty");
            else if ((ProfileType)cmbAccountType.SelectedValue == ProfileType.None)
                PopUpBoxes.ShowPopup("Choose a valid Account Type");
            else
            {
                if (HelperMasterDB.CheckIfAccountExist(txtUserName.Text) > 0)
                    PopUpBoxes.ShowPopup("User name already exist");
                else
                {

                    StringBuilder sb = EncryptionHelper.GetEncryptedPassword(txtPassword.Password);

                    var newId = HelperMasterDB.AddNewAccount(txtUserName.Text, sb.ToString(), (ProfileType)cmbAccountType.SelectedValue);

                    if (newId < 0)
                        PopUpBoxes.ShowPopup("Account not created");
                    else
                    {
                        txtUserName.Text = "";
                        txtPassword.Password = "";
                        cmbAccountType.SelectedValue = ProfileType.None;

                        //referesh user lsit
                        LoadUsersList();

                        PopUpBoxes.ShowPopup("Account correctly created");

                    }
                }
            }
        }

        private void BtnSaveDB_Click(object sender, RoutedEventArgs e)
        {
            DataBaseType db = DataBaseType.None;

            if ((bool)rbtMaschile.IsChecked)
                db = DataBaseType.Maschile;
            else if ((bool)rbtFemminile.IsChecked)
                db = DataBaseType.Femminile;
            else if ((bool)rbtTest.IsChecked)
                db = DataBaseType.Test;

            if (db == DataBaseType.None)
                PopUpBoxes.ShowPopup("Please select a DB");
            else
            {
                if(HelperMasterDB.UpdateSelectedDB(db))
                    PopUpBoxes.ShowPopup("Saved !!");
                else
                    PopUpBoxes.ShowPopup("Error during saving DB Info");
            }
        }

        private void BtnSaveFighterByPhase_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Slider
        private void SldMinFor16_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtBoxFor16th.Text = sldMinFor16.Value.ToString();
        }

        private void SldMinFor8_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtBoxFor8th.Text = sldMinFor8.Value.ToString();
        }

        private void SldMinFor4_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtBoxFor4th.Text = sldMinFor4.Value.ToString();
        }
        #endregion
    }
}
