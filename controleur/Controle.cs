using System.Collections.Generic;
using Mediatek86.modele;
using Mediatek86.metier;
using Mediatek86.vue;


namespace Mediatek86.controleur
{
    public class Controle
    {
        private readonly List<Livre> lesLivres;
        private readonly List<Dvd> lesDvd;
        private readonly List<Revue> lesRevues;
        private readonly List<Categorie> lesRayons;
        private readonly List<Categorie> lesPublics;
        private readonly List<Categorie> lesGenres;
        private List<Commande> lesCommandes;
        private readonly List<Suivi> lesSuivis;
        private readonly List<Abonnement> lesAbonnements;
        private FrmMediatek frameMediatek;
        private int instance = 0;


        /// <summary>
        /// Ouverture de la fenêtre
        /// </summary>
        public Controle()
        {
            lesLivres = Dao.GetAllLivres();
            lesDvd = Dao.GetAllDvd();
            lesRevues = Dao.GetAllRevues();
            lesGenres = Dao.GetAllGenres();
            lesRayons = Dao.GetAllRayons();
            lesPublics = Dao.GetAllPublics();
            lesCommandes = Dao.GetAllCommandes();
            lesSuivis = Dao.getAllSuivis();
            lesAbonnements = Dao.getAllAbonnements();

        }

        /// <summary>
        /// Demande de controler l'authentification 
        /// Si oui alors : ouverture de la fenêtre principale.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public int ControleAuthentification(string login, string pwd, FrmAuthentification frmAuthentification)
        {
            int service = 0;
            if ((service = Dao.ControleAuthentification(login, pwd)) != 0)
            {

                if (service == 3 || service == 2 || service == 1) {
                    if (instance != 1)
                    {
                        frmAuthentification.Hide();
                        instance = 1;
                        frameMediatek = new FrmMediatek(this, service);
                        frameMediatek.ShowDialog();

                    }
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 0;
            }
            return 2;

        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Collection d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return lesGenres;
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Collection d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return lesLivres;
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Collection d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return lesDvd;
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Collection d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return lesRevues;
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Collection d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return lesRayons;
        }

        public List<Commande> GetAllCommandes()
        {

            return Dao.GetAllCommandes();
        }


        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Collection d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return lesPublics;
        }

        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <returns>Collection d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return Dao.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return Dao.CreerExemplaire(exemplaire);
        }

        public bool CreerCommandeDocument(Commande commande)
        {
            return Dao.CreerCommmandeDocuemnt(commande);
            this.lesCommandes = GetAllCommandes();
        }

        public string getLastIdCommande()
        {
            return Dao.getLastIdCommande();

        }

        public List<Suivi> getAllSuivis()
        {
            return lesSuivis;


        }
        public bool updateCommandeDocument(Commande commande)
        {

            return Dao.updateCommandeDocument(commande);
            this.lesCommandes = GetAllCommandes();
        }
        public bool deleteCmdLivre(Commande commande)
        {
            return Dao.deleteCmdLivre(commande);
            this.lesCommandes = GetAllCommandes();


        }
        public List<Abonnement> getAllAbonnements()
        {
            return Dao.getAllAbonnements();
        }

        public bool creerCmdRevue(Abonnement abonnement)
        {
            return Dao.creerCmdRevue(abonnement);
        }
        public bool updateCmdRevue(Abonnement abonnement)
        {
            return Dao.updateCmdRevue(abonnement);
        }
        public bool deleteCmdRevue(Abonnement abonnement)
        {
            return Dao.deleteCmdRevue(abonnement);
        }

        public List<string> getLstExpirations()
        {
            return Dao.getLstExpiration();
        }

    }

}

