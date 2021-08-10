using BusinessEntity.Entity;
using Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsManagement.Settings
{
    public partial class AddTorneo : Form
    {
        public AddTorneo()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBoxNomeTorneo.Text != "")
            {
                DateTime datainizioTorneo = dateTimePickerDataInizioTorneo.Value;
                DateTime dataFineTorneo = dateTimePickerDataFineTorneo.Value;
                String nomeTorneo = textBoxNomeTorneo.Text;
                String luogoTorneo = textBoxLuogo.Text;

                bool swordAndDagger = checkBoxSpagaPugnale.Checked;
                bool swordAndBuckler = checkBoxSpadaBrocchiero.Checked;
                bool swordAndShield = checkBoxSpadaRotella.Checked;
                bool twoHandSword = checkBoxSpadaDueMani.Checked;
                bool singleSword = checkBoxSpadaSola.Checked;

                TorneoEntity t = new TorneoEntity()
                {
                    Name = nomeTorneo,
                    Place = luogoTorneo,
                    StartDate = datainizioTorneo,
                    EndDate = dataFineTorneo
                    //mancano i commenti
                };

                Int32 newTournamentId = SqlDal_Tournaments.InserNewTorneo(t);

                if(newTournamentId > 0)
                {
                    String categoria = radioButtonMale.Checked ? "M" : "F";
                    //TODO
                    SqlDal_Tournaments.AddDisciplineToTorneo(newTournamentId, singleSword, swordAndDagger, swordAndBuckler, swordAndShield, twoHandSword, categoria);
                }
                else
                {
                    MessageBox.Show("Errore durante il salvataggio del nuovo torneo", "ERRORE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
                MessageBox.Show("Inserire un nome per il nuovo Torneo", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
