using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mediatek86.controleur;

namespace Mediatek86.vue
{
    public partial class FrmAuthentification : Form
    {
        private Controle controle;
        
        public FrmAuthentification(Controle controle)
        {
            this.controle = controle;
            InitializeComponent();
        }

        private void btnSeconnecter_Click(object sender, EventArgs e)
        {
            if (!txtIdentifiant.Text.Equals("") && !txtMdp.Text.Equals(""))
            {
                if (controle.ControleAuthentification(txtIdentifiant.Text, txtMdp.Text, this) == 0)
                {
                    MessageBox.Show("Authentification incorrecte", "Alerte");
                    txtIdentifiant.Text = "";
                    txtMdp.Text = "";
                    txtIdentifiant.Focus();
                }
                else if(controle.ControleAuthentification(txtIdentifiant.Text, txtMdp.Text, this) == 1)
                {
                    MessageBox.Show("Vous ne disposez pas de droits suffisant pour accéder à l'application", "Alerte");
                }
            }
            else
            {
                MessageBox.Show("Tous les champs doivent être remplis.", "Information");
            }
        }
    }
}
