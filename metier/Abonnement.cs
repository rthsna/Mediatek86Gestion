using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    public class Abonnement
    {




        public Abonnement(string id, DateTime dateCommande, double montant, DateTime dateFinAbonnement, string idRevue)
        {

            this.Id = id;
            this.DateCommande = dateCommande;
            this.Montant = montant;
            this.DateFinAbonnement = dateFinAbonnement;
            this.IdRevue = idRevue;
        }

        public string Id { get; set; }
        public DateTime DateCommande { get; set; }
        public double Montant { get; set; }
        public DateTime DateFinAbonnement {get; set;}
        public string IdRevue { get; set; }

    }
}
