using Mediatek86.metier;
using System.Collections.Generic;
using Mediatek86.bdd;
using System;
using System.Windows.Forms;
using Mediatek86.controleur;
using Serilog;

namespace Mediatek86.modele
{
    public static class Dao
    {

        private static readonly string server = "3.88.49.183";
        private static readonly string userid = "roots";
        private static readonly string password = "password";
        private static readonly string database = "mediatek86";
        private static readonly string connectionString = "server=" + server + ";user id=" + userid + ";password=" + password + ";database=" + database + ";SslMode=none";
        
        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        /// <summary>
        /// Controle si l'utillisateur a le droit de se connecter (login, pwd)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static int ControleAuthentification(string login, string pwd)
        {
            string req = "select * from utilisateur";
            req += " where login=@login and password=SHA2(@pwd, 256);";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@login", login);
            parameters.Add("@pwd", pwd);

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, parameters);
            int idservice = 0;
            if (curs.Read())
            {

               idservice = (int)curs.Field("idservice");
                curs.Close();
                return idservice;
            }
            else
            {
                curs.Close();
                return idservice;
            }
        }
        public static List<Categorie> GetAllGenres()
        {
            List<Categorie> lesGenres = new List<Categorie>();
            string req = "Select * from genre order by libelle";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Genre genre = new Genre((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesGenres.Add(genre);
            }
            curs.Close();
            return lesGenres;
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Collection d'objets Rayon</returns>
        public static List<Categorie> GetAllRayons()
        {
            List<Categorie> lesRayons = new List<Categorie>();
            string req = "Select * from rayon order by libelle";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Rayon rayon = new Rayon((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesRayons.Add(rayon);
            }
            curs.Close();
            return lesRayons;
        }
        /// <summary>
        /// Retourne tous les ids des abonnements qui arrive à expiration selon le trigger present dans la BDD
        /// </summary>
        /// <returns>Collection d'objets Abonnements</returns>
        public static List<string> getLstExpiration()
        {
            List<string> lesIdAbonnements = new List<string>();
            string req = "Call whoExpire(); ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                lesIdAbonnements.Add(id);
                
            }
            curs.Close();
            return lesIdAbonnements;
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Collection d'objets Public</returns>
        public static List<Categorie> GetAllPublics()
        {
            List<Categorie> lesPublics = new List<Categorie>();
            string req = "Select * from public order by libelle";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Public lePublic = new Public((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesPublics.Add(lePublic);
            }
            curs.Close();
            return lesPublics;
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public static List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = new List<Livre>();
            string req = "Select l.id, l.ISBN, l.auteur, d.titre, d.image, l.collection, ";
            req += "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as public, r.libelle as rayon ";
            req += "from livre l join document d on l.id=d.id ";
            req += "join genre g on g.id=d.idGenre ";
            req += "join public p on p.id=d.idPublic ";
            req += "join rayon r on r.id=d.idRayon ";
            req += "order by titre ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                string isbn = (string)curs.Field("ISBN");
                string auteur = (string)curs.Field("auteur");
                string titre = (string)curs.Field("titre");
                string image = (string)curs.Field("image");
                string collection = (string)curs.Field("collection");
                string idgenre = (string)curs.Field("idgenre");
                string idrayon = (string)curs.Field("idrayon");
                string idpublic = (string)curs.Field("idpublic");
                string genre = (string)curs.Field("genre");
                string lepublic = (string)curs.Field("public");
                string rayon = (string)curs.Field("rayon");
                Livre livre = new Livre(id, titre, image, isbn, auteur, collection, idgenre, genre,
                    idpublic, lepublic, idrayon, rayon);
                lesLivres.Add(livre);
            }
            curs.Close();

            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public static List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = new List<Dvd>();
            string req = "Select l.id, l.duree, l.realisateur, d.titre, d.image, l.synopsis, ";
            req += "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as public, r.libelle as rayon ";
            req += "from dvd l join document d on l.id=d.id ";
            req += "join genre g on g.id=d.idGenre ";
            req += "join public p on p.id=d.idPublic ";
            req += "join rayon r on r.id=d.idRayon ";
            req += "order by titre ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                int duree = (int)curs.Field("duree");
                string realisateur = (string)curs.Field("realisateur");
                string titre = (string)curs.Field("titre");
                string image = (string)curs.Field("image");
                string synopsis = (string)curs.Field("synopsis");
                string idgenre = (string)curs.Field("idgenre");
                string idrayon = (string)curs.Field("idrayon");
                string idpublic = (string)curs.Field("idpublic");
                string genre = (string)curs.Field("genre");
                string lepublic = (string)curs.Field("public");
                string rayon = (string)curs.Field("rayon");
                Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idgenre, genre,
                    idpublic, lepublic, idrayon, rayon);
                lesDvd.Add(dvd);
            }
            curs.Close();

            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public static List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = new List<Revue>();
            string req = "Select l.id, l.empruntable, l.periodicite, d.titre, d.image, l.delaiMiseADispo, ";
            req += "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as public, r.libelle as rayon ";
            req += "from revue l join document d on l.id=d.id ";
            req += "join genre g on g.id=d.idGenre ";
            req += "join public p on p.id=d.idPublic ";
            req += "join rayon r on r.id=d.idRayon ";
            req += "order by titre ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                bool empruntable = (bool)curs.Field("empruntable");
                string periodicite = (string)curs.Field("periodicite");
                string titre = (string)curs.Field("titre");
                string image = (string)curs.Field("image");
                int delaiMiseADispo = (int)curs.Field("delaimiseadispo");
                string idgenre = (string)curs.Field("idgenre");
                string idrayon = (string)curs.Field("idrayon");
                string idpublic = (string)curs.Field("idpublic");
                string genre = (string)curs.Field("genre");
                string lepublic = (string)curs.Field("public");
                string rayon = (string)curs.Field("rayon");
                Revue revue = new Revue(id, titre, image, idgenre, genre,
                    idpublic, lepublic, idrayon, rayon, empruntable, periodicite, delaiMiseADispo);
                lesRevues.Add(revue);
            }
            curs.Close();

            return lesRevues;
        }

        /// <summary>
        /// Retourne toutes les Commandes à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Commande</returns>
        public static List<Commande> GetAllCommandes()
        {
            List<Suivi> lesSuivis = getAllSuivis();
            List<Commande> lesCommandes = new List<Commande>();
            string req = "Select C.id, C.dateCommande, C.montant, CD.nbExemplaire, CD.idLivreDvd, CD.idsuivi ";
            req += " from commandedocument CD join commande C on CD.id = C.id";
            req += " order by C.Id;";
            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                DateTime dateCommande = (DateTime)curs.Field("dateCommande");
                double montant = (double)curs.Field("montant");
                int nbExemplaire = (int)curs.Field("nbExemplaire");
                string idLivreDvd = (string)curs.Field("idLivreDvd");
                string idsuivi = (string)curs.Field("idsuivi");

                Suivi unSuivi = lesSuivis.Find(x => x.Id.Equals(idsuivi));
                Commande commande = new Commande(id, dateCommande, montant, nbExemplaire, idLivreDvd, unSuivi);
                lesCommandes.Add(commande);
            }
            curs.Close();

            return lesCommandes;
        }

        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <returns>Liste d'objets Exemplaire</returns>
        public static List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            List<Exemplaire> lesExemplaires = new List<Exemplaire>();
            string req = "Select e.id, e.numero, e.dateAchat, e.photo, e.idEtat ";
            req += "from exemplaire e join document d on e.id=d.id ";
            req += "where e.id = @id ";
            req += "order by e.dateAchat DESC";
            Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", idDocument}
                };

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, parameters);

            while (curs.Read())
            {
                string idDocuement = (string)curs.Field("id");
                int numero = (int)curs.Field("numero");
                DateTime dateAchat = (DateTime)curs.Field("dateAchat");
                string photo = (string)curs.Field("photo");
                string idEtat = (string)curs.Field("idEtat");
                Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocuement);
                lesExemplaires.Add(exemplaire);
            }
            curs.Close();

            return lesExemplaires;
        }


        public static List<Suivi> getAllSuivis()
        {
            List<Suivi> lesSuivis = new List<Suivi>();
            string req = "SELECT * from suivi; ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {

                string id = (string)curs.Field("id");
                string libelle = (string)curs.Field("libelle");
                Suivi suivi = new Suivi(id, libelle);
                lesSuivis.Add(suivi);


            }
            curs.Close();

            return lesSuivis;
        }



        public static string getLastIdCommande()
        {

            string req = "SELECT max(CAST(id as SIGNED INTEGER)) AS id from commande;";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            int id = 0;
            while (curs.Read())
            {

                id = Convert.ToInt32((long)curs.Field("id"));
                id += 1;

            }
            curs.Close();


            return id.ToString();
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire"></param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public static bool CreerExemplaire(Exemplaire exemplaire)
        {
            try
            {
                string req = "insert into exemplaire values (@idDocument,@numero,@dateAchat,@photo,@idEtat);";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@idDocument", exemplaire.IdDocument},
                    { "@numero", exemplaire.Numero},
                    { "@dateAchat", exemplaire.DateAchat},
                    { "@photo", exemplaire.Photo},
                    { "@idEtat",exemplaire.IdEtat}
                };
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
              
                return true;
            }
            catch
            {
                return false;
            }
        }




        /// <summary>
        /// Creer la commande d'un document
        /// </summary>
        /// <param name="commande"></param>
        /// <returns>true si l'insertion a pu se faire</returns>
        public static bool CreerCommmandeDocuemnt(Commande commande)
        {
            try
            {
                string req = "insert into commande values (@id,@dateCommande,@montant);";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", commande.Id},
                    { "@dateCommande", commande.DateCommande},
                    { "@montant", commande.Montant},

                };
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                curs.Close();
                string req2 = "insert into commandedocument values(@id, @nbExemplaire, @idLivreDvd, @idSuivi); ";
                Dictionary<string, object> scParameters = new Dictionary<string, object>
                {

                    { "@id", commande.Id},
                    { "@nbExemplaire", commande.NbExemplaire},
                    { "@idLivreDvd",commande.IdLivreDvd},
                    { "@idSuivi",commande.Suivi.Id}
                };
                BddMySql scCurs = BddMySql.GetInstance(connectionString);
                scCurs.ReqUpdate(req2, scParameters);
               
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <returns>true si l'insertion a pu se faire</returns>
        public static bool updateCommandeDocument(Commande commande)
        {
            try
            {
                string req = "update commandedocument set idsuivi = @idSuivi ";
                req += "where id = @id;";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", commande.Id},
                    { "@nbExemplaire", commande.NbExemplaire},
                    { "@LivreDvd", commande.IdLivreDvd},
                    { "@idSuivi", commande.Suivi.Id}

                };
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                
                return true;


            }
            catch
            {
                return false;
            }
        }
        public static bool deleteCmdLivre(Commande commande)
        {
            try
            {
                string req = "delete from commandedocument where id = @id; ";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", commande.Id}
                    
                };
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
            


                string req2 = "delete from commande where id = @id; ";

                Dictionary<string, object> parameters2 = new Dictionary<string, object>
                {
                    { "@id", commande.Id}

                };
                BddMySql curs2 = BddMySql.GetInstance(connectionString);
                curs2.ReqUpdate(req2, parameters2);
             
                return true;

            }
            catch
            {
                return false;
            }



        }

        public static bool creerCmdRevue(Abonnement abonnement)
        {
            try
            {
                string req = "insert into commande values (@id,@dateCommande,@montant);";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", abonnement.Id},
                    { "@dateCommande", abonnement.DateCommande},
                    { "@montant", abonnement.Montant},

                };
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                string req2 = "insert into abonnement values(@id, @dateFinAbonnement, @idRevue); ";
                Dictionary<string, object> scParameters = new Dictionary<string, object>
                {

                    { "@id", abonnement.Id},
                    { "@dateFinAbonnement", abonnement.DateFinAbonnement},
                    { "@idRevue",abonnement.IdRevue}
                };
                BddMySql scCurs = BddMySql.GetInstance(connectionString);
                scCurs.ReqUpdate(req2, scParameters);

                return true;
            }
            catch
            {
                return false;
            }
        }


        public static bool deleteCmdRevue(Abonnement abonnement)
        {
            try
            {
                string req = "delete from abonnement where id = @id; ";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", abonnement.Id}

                };
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
               


                string req2 = "delete from commande where id = @id; ";

                Dictionary<string, object> parameters2 = new Dictionary<string, object>
                {
                    { "@id", abonnement.Id}

                };
                BddMySql curs2 = BddMySql.GetInstance(connectionString);
                curs2.ReqUpdate(req2, parameters2);
                return true;

            }
            catch
            {
                return false;
            }



        }


        public static bool updateCmdRevue(Abonnement abonnement)
        {
            try
            {
                string req = "update abonnement set dateFinAbonnement = @dateFinAbonnement ";
                req += "where id = @id;";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", abonnement.Id},
                    { "@dateFinAbonnement", abonnement.DateFinAbonnement }

                };
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);

                return true;


            }
            catch
            {
                return false;
            }
        }

        public static List<Abonnement> getAllAbonnements()
        {
            
            List<Abonnement> lesAbonnements = new List<Abonnement>();
            string req = "Select C.id, C.dateCommande, C.montant, A.dateFinAbonnement, A.idRevue ";
            req += " from abonnement A join commande C on A.id = C.id";
            req += " order by C.Id;";
            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                DateTime dateCommande = (DateTime)curs.Field("dateCommande");
                double montant = (double)curs.Field("montant");
                DateTime dateFinCommande = (DateTime)curs.Field("dateFinAbonnement");
                string idRevue = (string)curs.Field("idRevue");
                Abonnement abonnement = new Abonnement(id, dateCommande, montant, dateFinCommande, idRevue);
                lesAbonnements.Add(abonnement);
            }
            curs.Close();

            return lesAbonnements;
        }
    }
}
