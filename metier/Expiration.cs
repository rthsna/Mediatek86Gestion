using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    class Expiration
    {
        public Expiration(string titre, DateTime dateFin)
        {
            this.DateFin = dateFin;
            this.Titre = titre;
        }

        public DateTime DateFin { get; set; }
        public string Titre { get; set; }

    }
}
