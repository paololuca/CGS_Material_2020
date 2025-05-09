﻿using BusinessEntity.Entity;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HEMATournamentSystem
{
    /// <summary>
    /// Interaction logic for Fighters.xaml
    /// </summary>
    public partial class Fighters : UserControl
    {
        private List<AtletaEntity> atletiPresenti;
        private readonly LoginUser user;

        public Fighters(LoginUser user)
        {
            this.user = user;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadListAtleti();
            LoadAssociation();
        }
        
        private void LoadListAtleti()
        {
            atletiPresenti = SqlDal_Fighters.GetAllAnagraficaAtletiWithRanking();

            dataGridAthletes.ItemsSource = atletiPresenti;
        }

        private void LoadAssociation()
        {
            var associations = SqlDal_Associations.GetAllAsd(false);
            cmbAssociation.ItemsSource = associations;
            cmbAssociation.DisplayMemberPath = "NomeAsd";
            cmbAssociation.SelectedValuePath = "NomeAsd";

        }

        private void DataGridAthletes_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            Style horizontalAlignment = new Style();
            horizontalAlignment.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Center));

            switch (e.Column.Header.ToString())
            {
                case "FullName":
                case "IdAtleta":
                case "IdAsd":
                case "Ranking":
                case "Posizionamento":
                    e.Column.Visibility = Visibility.Hidden;
                    break;
                case "IsEnabled":
                    e.Column.IsReadOnly = true;
                    break;
                default:
                    e.Column.Visibility = Visibility.Visible;
                    //e.Column.IsReadOnly = true;
                    break;

            }
        }

        /// <summary>
        /// Logical Delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteAccount_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDetailsAccount_Click(object sender, RoutedEventArgs e)
        {
            AtletaEntity fighter = ((FrameworkElement)sender).DataContext as AtletaEntity;
            var f = new FighterStats(fighter.IdAtleta);
            f.Show();
        }

        private void TxtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            var filtered = atletiPresenti.Where(fighter => 
            fighter.Nome.ToLower().Contains(txtSearch.Text.ToLower()) 
            || 
            fighter.Cognome.ToLower().Contains(txtSearch.Text.ToLower())).ToList();

            dataGridAthletes.ItemsSource = filtered;
        }

        private void BtnSaveFighter_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckForSaving())
                return;

            var ass = cmbAssociation.SelectedItem as AsdEntity;

            int id = SqlDal_Fighters.InsertNewAtleta(new AtletaEntity
                ()
            {
                IdAsd = ass.Id,
                Cognome = txtSurname.Text.TrimStart().TrimEnd(),
                Nome = txtName.Text.TrimStart().TrimEnd(),
                Sesso = (bool)rbtMaschile.IsChecked ? "M" : "F",
                Email = txtEmail.Text.TrimStart().TrimEnd()
            });

            if (id > 0)
            {
                new MessageBoxCustom("Saved", MessageType.Success, MessageButtons.Ok).ShowDialog();

                ClearField();
                LoadListAtleti();
            }
            else
                new MessageBoxCustom("Error during saving", MessageType.Error, MessageButtons.Ok).ShowDialog();

        }

        private void ClearField()
        {
            txtName.Text = "";
            txtSurname.Text = "";
            txtEmail.Text = "";

            cmbAssociation.SelectedIndex = 0;
        }

        private bool CheckForSaving()
        {
            if (txtName.Text == "")
            {
                new MessageBoxCustom("Name cannot be empty", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                return false;
            }
            else if (txtSurname.Text == "")
            { 
                new MessageBoxCustom("Surname cannot be empty", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                return false;
            }
            else if (!(bool)rbtFemminile.IsChecked && !(bool)rbtMaschile.IsChecked)
            { 
                new MessageBoxCustom("Select a gender", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                return false;
            }
            else if (cmbAssociation.SelectedIndex <= 0)
            { 
                new MessageBoxCustom("Select an Association", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                return false;
            }

            return true;
        }

        private void btnExportFighters_Click(object sender, RoutedEventArgs e)
        {
            // Not developed yet.
            throw new NotImplementedException();
        }
    }
}
