using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    public class Suivi
    {

        public Suivi(string id, string libelle)
        {

            this.Id = id;
            this.Libelle = libelle;


        }

        public string Id { set; get;}
        public string Libelle { set; get; }

        public override string ToString()
        {
            return Libelle;
        }
             
        



    }
}
