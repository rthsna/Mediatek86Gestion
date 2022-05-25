using Mediatek86.metier;
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
    public partial class Alerte : Form
    {

        private BindingSource bdgLstAbonnements = new BindingSource();
        private List<Abonnement> lesAbonnements = new List<Abonnement>();
        private List<string> lesIdAbonnements = new List<string>();
        private List<Revue> lesRevues = new List<Revue>();
        private Controle controle;
        public Alerte(Controle controle)
        {
            this.controle = controle;
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            remplirdgvlstAbonnement();

        }

        public void remplirdgvlstAbonnement()
        {
            lesAbonnements = controle.getAllAbonnements();
            lesIdAbonnements = controle.getLstExpirations();
            lesRevues = controle.GetAllRevues();
            List<Expiration> lesExpirations = new List<Expiration>();
            foreach (string unId in lesIdAbonnements)
            {
                Abonnement unAbonnement = lesAbonnements.Find(x => x.Id.Equals(unId));
                Revue uneRevue = lesRevues.Find(x => x.Id.Equals(unAbonnement.IdRevue));
                lesExpirations.Add(new Expiration(uneRevue.Titre, unAbonnement.DateFinAbonnement));
            }

            lesExpirations  = lesExpirations.OrderBy(o => o.DateFin).ToList();
            bdgLstAbonnements.DataSource = lesExpirations;
            dgvLstExpiration.DataSource = bdgLstAbonnements;
     

        }

         
    }
}
