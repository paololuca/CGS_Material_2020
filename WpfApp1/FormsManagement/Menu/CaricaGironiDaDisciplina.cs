﻿using BusinessEntity.Entity;
using Resources;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FormsManagement.Menu
{
    public partial class CaricaGironiDaDisciplina : Form
    {
        public CaricaGironiDaDisciplina(bool export)
        {
            InitializeComponent();
            var dbType = Helper.GetDbType();

            this.StartPosition = FormStartPosition.CenterScreen;

            if(export)
                this.buttonOk.Text = "Carica Risultati";

            List<TorneoEntity> tornei = !export ? SqlDal_Tournaments.GetTorneiAttivi(false) : SqlDal_Tournaments.GetTorneiConclusi(false);
            this.comboBox1.DataSource = tornei.ToArray();
            this.comboBox1.ValueMember = "Id";
            this.comboBox1.DisplayMember = "Name";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);

            //La inizializzo a 0
            List<DisciplinaEntity> discipline = SqlDal_Tournaments.GetDisciplineByIdTorneo(0);
            this.comboBox2.DataSource = discipline.ToArray();
            this.comboBox2.ValueMember = "IdDisciplina";
            this.comboBox2.DisplayMember = "Nome";

            

            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Abort;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if ((int)comboBox1.SelectedValue > 0)
            {
                ComboBox comboBox = (ComboBox)sender;

                List<DisciplinaEntity> discipline = SqlDal_Tournaments.GetDisciplineByIdTorneo((int)comboBox1.SelectedValue);

                this.comboBox2.DataSource = discipline.ToArray();
                this.comboBox2.ValueMember = "IdDisciplina";
                this.comboBox2.DisplayMember = "Nome";
            }

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            IdTorneo = (int)comboBox1.SelectedValue;
            IdDisciplina = (int)comboBox2.SelectedValue;

            if ((IdTorneo > 0) && (IdDisciplina > 0))
            {
                NomeTorneo = comboBox1.Text;
                NomeDisciplina = comboBox2.Text;
                this.Close();
            }
            else
            {
                if (MessageBox.Show("Selezionare un Torneo ed una Disciplina per la creazione dei Gironi",
                                "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                }
            }


        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            IdTorneo = 0;
            IdDisciplina = 0;
            this.Close();
        }
    }
}
