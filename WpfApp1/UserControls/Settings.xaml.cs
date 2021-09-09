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
            DataBaseType db = SqlDal_MasterDB.GetSelectedDB();

            switch (db)
            {
                case DataBaseType.Principal:
                    rbtDB.IsChecked = true;
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
            var accountList = SqlDal_MasterDB.GetAccountList();

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

                if (SqlDal_MasterDB.DeleteUserAccount(userToDelete.UserName))
                {
                    LoadUsersList();
                    new MessageBoxCustom("User " + userToDelete.UserName + " deleted ", MessageType.Success, MessageButtons.Ok).ShowDialog();
                }
                else
                    new MessageBoxCustom("ERROR occurred: no User deleted", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }

        }
        private void BtnSaveAccount_Click(object sender, RoutedEventArgs e)
        {
            if (txtUserName.Text == "")
                new MessageBoxCustom("User Name cannot be empty", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            else if (txtPassword.Password == "")
                new MessageBoxCustom("Password cannot be empty", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            else if ((ProfileType)cmbAccountType.SelectedValue == ProfileType.None)
                new MessageBoxCustom("Choose a valid Account Type", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            else
            {
                if (SqlDal_MasterDB.CheckIfAccountExist(txtUserName.Text) > 0)
                    new MessageBoxCustom("User name already exist", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                else
                {

                    StringBuilder sb = EncryptionHelper.GetEncryptedPassword(txtPassword.Password);

                    var newId = SqlDal_MasterDB.AddNewAccount(txtUserName.Text, sb.ToString(), (ProfileType)cmbAccountType.SelectedValue);

                    if (newId < 0)
                        new MessageBoxCustom("Account not created", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    else
                    {
                        txtUserName.Text = "";
                        txtPassword.Password = "";
                        cmbAccountType.SelectedValue = ProfileType.None;

                        //referesh user lsit
                        LoadUsersList();

                        new MessageBoxCustom("Account correctly created", MessageType.Success, MessageButtons.Ok).ShowDialog();

                    }
                }
            }
        }

        private void BtnSaveDB_Click(object sender, RoutedEventArgs e)
        {
            DataBaseType db = DataBaseType.None;

            if ((bool)rbtDB.IsChecked)
                db = DataBaseType.Principal;
            else if ((bool)rbtTest.IsChecked)
                db = DataBaseType.Test;

            if (db == DataBaseType.None)
                new MessageBoxCustom("Please select a DB", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            else
            {
                if(SqlDal_MasterDB.UpdateSelectedDB(db))
                    new MessageBoxCustom("Saved !!", MessageType.Success, MessageButtons.Ok).ShowDialog();
                else
                    new MessageBoxCustom("Error during saving DB Info", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
        }

        private void BtnSaveFighterByPhase_Click(object sender, RoutedEventArgs e)
        {
            //TODO
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
