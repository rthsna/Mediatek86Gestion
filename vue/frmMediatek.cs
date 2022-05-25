using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Mediatek86.metier;
using Mediatek86.controleur;
using System.Drawing;
using System.Linq;
using Serilog;

namespace Mediatek86.vue
{
    public partial class FrmMediatek : Form
    {

        #region Variables globales

        private readonly Controle controle;
        const string ETATNEUF = "00001";

        private readonly BindingSource bdgLivresListe = new BindingSource();
        private readonly BindingSource bdgLivresListe2 = new BindingSource();
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private readonly BindingSource bdgCommandes = new BindingSource();
        private readonly BindingSource bdgSuivis = new BindingSource();
        private readonly BindingSource bdgCommandesDvd = new BindingSource();
        private readonly BindingSource bdgAbonnements = new BindingSource();
        private readonly BindingSource bdgSuivisDvd = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();
        private List<Dvd> lesDvd = new List<Dvd>();
        private List<Revue> lesRevues = new List<Revue>();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        private List<Commande> lesCommandes = new List<Commande>();
        private List<Abonnement> lesAbonnements = new List<Abonnement>();
        private List<Suivi> lesSuivis = new List<Suivi>();
        private int niveau = 0;


        #endregion


        public FrmMediatek(Controle controle, int niveau)
        {
            this.controle = controle;
            this.niveau = niveau;
            InitializeComponent();
           
            if (niveau == 3)
            {
     
                TabCtrl.TabPages.Remove(tabCmdLivres);
                TabCtrl.TabPages.Remove(tabCommandeDvd);
                TabCtrl.TabPages.Remove(tabCmdRevue);

            }
          
        }
       


        #region modules communs

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories"></param>
        /// <param name="bdg"></param>
        /// <param name="cbx"></param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        #endregion


        #region Revues
        //-----------------------------------------------------------
        // ONGLET "Revues"
        //------------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controle.GetAllRevues();
            RemplirComboCategorie(controle.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(controle.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(controle.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
        }
        
        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirRevuesListe(List<Revue> revues, DataGridView dgvRevuesLst)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesLst.DataSource = bdgRevuesListe;
            dgvRevuesLst.Columns["empruntable"].Visible = false;
            dgvRevuesLst.Columns["idRayon"].Visible = false;
            dgvRevuesLst.Columns["idGenre"].Visible = false;
            dgvRevuesLst.Columns["idPublic"].Visible = false;
            dgvRevuesLst.Columns["image"].Visible = false;
            dgvRevuesLst.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesLst.Columns["id"].DisplayIndex = 0;
            dgvRevuesLst.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>();
                    revues.Add(revue);
                    RemplirRevuesListe(revues, dgvRevuesListe);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre, dgvRevuesListe);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue"></param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            chkRevuesEmpruntable.Checked = revue.Empruntable;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch(Exception e)
            {
                pcbRevuesImage.Image = null;
                Log.Information(e, "pcpRevuesImage null dans la fonction AfficheRevuesInfos");
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            chkRevuesEmpruntable.Checked = false;
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues, dgvRevuesListe);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues, dgvRevuesListe);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues, dgvRevuesListe);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch(Exception ex)
                {
                    VideRevuesZones();
                    Log.Fatal(ex, "DgvRevuesListe sur l'evenement selectionChanged à échoué");
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues, dgvRevuesListe);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList, dgvRevuesListe);
        }

        #endregion


        #region Livres

        //-----------------------------------------------------------
        // ONGLET "LIVRES"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controle.GetAllLivres();
            RemplirComboCategorie(controle.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(controle.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(controle.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirLivresListe(List<Livre> livres, DataGridView dgvLivresListe)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>();
                    livres.Add(livre);
                    RemplirLivresListe(livres, dgvLivresListe);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre, dgvLivresListe);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre"></param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
               
            }
            catch(Exception e)
            {
                pcbLivresImage.Image = null;
                Log.Information(e, "probleme dans la fonction AfficheLivresInfos");
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres, dgvLivresListe);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres, dgvLivresListe);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres, dgvLivresListe);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }             
                catch (Exception ex)
                {
                    VideLivresZones();
                    Log.Information(ex, "probleme dans la fonction DgvLivresListe sur l'evenement Selection Changed");
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres, dgvLivresListe);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList, dgvLivresListe);
        }

        #endregion


        #region Dvd
        //-----------------------------------------------------------
        // ONGLET "DVD"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controle.GetAllDvd();
            RemplirComboCategorie(controle.GetAllGenres(), bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(controle.GetAllPublics(), bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(controle.GetAllRayons(), bdgRayons, cbxDvdRayons);
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirDvdListe(List<Dvd> Dvds, DataGridView dgvDvdLst)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdLst.DataSource = bdgDvdListe;
            dgvDvdLst.Columns["idRayon"].Visible = false;
            dgvDvdLst.Columns["idGenre"].Visible = false;
            dgvDvdLst.Columns["idPublic"].Visible = false;
            dgvDvdLst.Columns["image"].Visible = false;
            dgvDvdLst.Columns["synopsis"].Visible = false;
            dgvDvdLst.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdLst.Columns["id"].DisplayIndex = 0;
            dgvDvdLst.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>();
                    Dvd.Add(dvd);
                    RemplirDvdListe(Dvd, dgvDvdListe);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre, dgvDvdListe);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd"></param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
                Log.Information("pcbDvdImage ne contient pas d'image");
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd, dgvDvdListe);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd, dgvDvdListe);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd, dgvDvdListe);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                }
                catch
                {
                    VideDvdZones();
                    Log.Information("l'evenement SelectionChanged sur dgvDvdList à échoué ");
                }
            }
            else
            { 
                VideDvdInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd, dgvDvdListe);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList, dgvDvdListe);
        }

        #endregion

        
        #region Réception Exemplaire de presse
        //-----------------------------------------------------------
        // ONGLET "RECEPTION DE REVUES"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet : blocage en saisie des champs de saisie des infos de l'exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controle.GetAllRevues();
            accesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            bdgExemplairesListe.DataSource = exemplaires;
            dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
            dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
            dgvReceptionExemplairesListe.Columns["idDocument"].Visible = false;
            dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
            dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    VideReceptionRevueInfos();
                }
            }
            else
            {
                VideReceptionRevueInfos();
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            accesReceptionExemplaireGroupBox(false);
            VideReceptionRevueInfos();
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue"></param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            chkReceptionRevueEmpruntable.Checked = revue.Empruntable;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            afficheReceptionExemplairesRevue();
            // accès à la zone d'ajout d'un exemplaire
            accesReceptionExemplaireGroupBox(true);
        }

        private void afficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controle.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
        }

        /// <summary>
        /// Vide les zones d'affchage des informations de la revue
        /// </summary>
        private void VideReceptionRevueInfos()
        {
            txbReceptionRevuePeriodicite.Text = "";
            chkReceptionRevueEmpruntable.Checked = false;
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            lesExemplaires = new List<Exemplaire>();
            RemplirReceptionExemplairesListe(lesExemplaires);
            accesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de l'exemplaire
        /// </summary>
        private void VideReceptionExemplaireInfos()
        {
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces"></param>
        private void accesReceptionExemplaireGroupBox(bool acces)
        {
            VideReceptionExemplaireInfos();
            grpReceptionExemplaire.Enabled = acces;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controle.CreerExemplaire(exemplaire))
                    {
                        VideReceptionExemplaireInfos();
                        afficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// Sélection d'une ligne complète et affichage de l'image sz l'exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }

        #endregion

        /// <summary>
        /// Recherche un Livres , l'affiche et affiche ses commandes si il est disponnible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercherLivres_Click(object sender, EventArgs e)
        {
            lesSuivis = controle.getAllSuivis();
            lesCommandes = controle.GetAllCommandes();
            if (!txtNumeroLivreRecherche.Text.Equals(""))
            {

                List<Livre> livres = new List<Livre>();
                Livre livre = lesLivres.Find(x => x.Id.Equals(txtNumeroLivreRecherche.Text));
                if (livre != null)
                {
                    if (livre.Id != txtNumLivreCmd.Text)
                    {
                        livres.Add(livre);
                        txtNumLivreCmd.Text = livre.Id;
                        RemplirLivresListe(livres, dgvSearchLivreCmd);
                        remplirLstCmdLivres(controle.GetAllCommandes(), livre.Id);
                        zoneNewCmdEnable(false);
                    }
                    else
                    {
                        MessageBox.Show("Le numero entrée est identique à l'ancien");
                    }

                }
                else
                {
                    MessageBox.Show("Le numéro entré ne correspond à aucun livre");
                }


            }
            else
            {
                MessageBox.Show("Pas de numero de livre entrée");
            }


        }

        /// <summary>
        /// Remplis la datagriedview de commande de livres
        /// </summary>
        /// <param name="id"></param>
        private void remplirLstCmdLivres(List<Commande> Commandes, string id)
        {


            List<Commande> CommandesOfId = Commandes.FindAll(x => x.IdLivreDvd.Equals(id));
            if (CommandesOfId.Count <= 0)
            {
                MessageBox.Show("Il n'existe pas ou plus de commande pour cette revue");
            }
            bdgCommandes.DataSource = CommandesOfId;
            dgvCommandeLivres.DataSource = bdgCommandes;
            dgvCommandeLivres.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;



        }
        /// <summary>
        /// Defini quelles zones acceptent les actions utilisateurs
        /// </summary>
        /// <param name="saisie"></param>
        private void zoneNewCmdEnable(bool saisie)
        {
            if(txtMontantCmdLivre.Enabled)
            {
                viderNouvelleCommande();
            }
            TxtNbExemplaireCmdLivre.Enabled = saisie;
            txtMontantCmdLivre.Enabled = saisie;
            cbxSuiviCmdLivre.Enabled = !saisie;
            btnModifier.Enabled = !saisie;
            btnSupprimer.Enabled = !saisie;
            btnAnnuler.Enabled = saisie;
            btnValiderCmdLivre.Enabled = saisie;
            btnNewCmd.Enabled = !saisie;
            if (dgvCommandeLivres.CurrentCell == null)
            {
                btnModifier.Enabled = false;
                cbxSuiviCmdLivre.Enabled = false;
                btnSupprimer.Enabled = false;
            }
            if (txtNumLivreCmd.Text == "")
            {
                
                btnNewCmd.Enabled = false;
            }


        }

        /// <summary>
        /// A chaque chargement de l'onglet , initialise certaines données. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void tabCmdLivres_Enter(object sender, EventArgs e)
        {
            lesCommandes = controle.GetAllCommandes();
            lesSuivis = controle.getAllSuivis();
            txtNumLivreCmd.Enabled = false;
            zoneNewCmdEnable(false);



        }

        /// <summary>
        /// Créer une  nouvelle commande de livres  si les conditions sont satisfaisantes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValiderCmdLivre_Click(object sender, EventArgs e)
        {
            if (txtMontantCmdLivre.Text != "" && TxtNbExemplaireCmdLivre.Text != "")
            {
                try
                {
                    if (MessageBox.Show("Souhaitez-vous creer une nouvelle commande ?  ", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Double montant = double.Parse(txtMontantCmdLivre.Text);
                        int nbExemplaire = int.Parse(TxtNbExemplaireCmdLivre.Text);
                        String idLivreDvd = txtNumLivreCmd.Text;
                        string id = controle.getLastIdCommande();
                        Suivi unSuivi = lesSuivis.Find(x => x.Id.Equals("EC"));
                        Commande commande = new Commande(id, DateTime.Now, montant, nbExemplaire, idLivreDvd, unSuivi);
                        controle.CreerCommandeDocument(commande);
                        remplirLstCmdLivres(controle.GetAllCommandes(), txtNumLivreCmd.Text);
                        zoneNewCmdEnable(false);


                    }
                }
                catch(Exception exc)
                {
                    Log.Information(exc, "l'evenement valider cmdLivre à échoué ");
                    MessageBox.Show("Le nombre d'exemplaire doit etre numerique ", "Information");
                }
            }
            else
            {
                MessageBox.Show("Certains champs sont vides", "Information");
            }




        }

        /// <summary>
        /// Vide les champs pour créer une nouvelle commande
        /// </summary>
        private void viderNouvelleCommande()
        {
            TxtNbExemplaireCmdLivre.Text = "";
            txtMontantCmdLivre.Text = "";
            cbxSuiviCmdLivre.Text = "";
        }
        /// <summary>
        /// Evenement sur changement de ligne sur la dgv : valorise les champs .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandeLivres_SelectionChanged(object sender, EventArgs e)
        {

            if (dgvCommandeLivres.CurrentCell != null)
            {
                try
                {
                    remplirInfoCommande();
                    
                }
                catch
                {
                    MessageBox.Show("Le remplissage des informations à échoué");
                }
            }
            zoneNewCmdEnable(false);


        }

        /// <summary>
        /// Valorise les champs
        /// </summary>
        private void remplirInfoCommande()
        {

            Commande commande = (Commande)bdgCommandes.List[bdgCommandes.Position];
            List<Suivi> lstSuivisModif = new List<Suivi>();
            foreach (Suivi suivi in lesSuivis)
            {
                lstSuivisModif.Add(suivi);
                
            }
            Suivi unSuivi = lstSuivisModif.Find(x => x.Id.Equals(commande.Suivi.Id));

            if (unSuivi.Id == "EC" || unSuivi.Id == "REL")
            {
                lstSuivisModif.Remove(lesSuivis.Find(x => x.Id.Equals("REG")));

            }
            else if (unSuivi.Id == "REG" || unSuivi.Id == "LI")
            {
                lstSuivisModif.Remove(lesSuivis.Find(x => x.Id.Equals("REL")));
                lstSuivisModif.Remove(lesSuivis.Find(x => x.Id.Equals("EC")));
                if (unSuivi.Id == "REG")
                {
                    lstSuivisModif.Remove(lstSuivisModif.Find(x => x.Id.Equals("LI")));
                }

            }
            remplirCbxSuiviCmd(lstSuivisModif);
            txtMontantCmdLivre.Text = commande.Montant.ToString();
            TxtNbExemplaireCmdLivre.Text = commande.NbExemplaire.ToString();



        }

        /// <summary>
        /// Valorise la combobox suivi
        /// </summary>
        /// <param name="lstSuivisModif"></param>
        private void remplirCbxSuiviCmd(List<Suivi> lstSuivisModif)
        {

            Commande commande = (Commande)bdgCommandes.List[bdgCommandes.Position];
            bdgSuivis.DataSource = lstSuivisModif;
            cbxSuiviCmdLivre.DataSource = bdgSuivis;
            cbxSuiviCmdLivre.SelectedIndex = cbxSuiviCmdLivre.FindStringExact(commande.Suivi.Libelle);
        }
        /// <summary>
        /// Evenements sur la modification d'un champs 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Souhaitez-vous vraiment modifier l'etape de suivi de la commande ?  ", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Commande commande = (Commande)bdgCommandes.List[bdgCommandes.Position];
                Suivi unSuivi = lesSuivis.Find(x => x.Id.Equals(commande.Suivi.Id));
                if (unSuivi.Libelle != cbxSuiviCmdLivre.Text)
                {
                    commande.Suivi = (Suivi)cbxSuiviCmdLivre.SelectedItem;
                    controle.updateCommandeDocument(commande);
                    remplirLstCmdLivres(controle.GetAllCommandes(), txtNumLivreCmd.Text);

                }

            }


        }
        /// <summary>
        /// Evenement sur le bouton annuler commande livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnuler_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Souhaitez-vous annuler la saisie d'une commande ?  ", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                viderNouvelleCommande();
                remplirInfoCommande();
                zoneNewCmdEnable(false);
            }

        }

        /// <summary>
        /// Evenement sur le bouton creer une nouvelle commande livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewCmd_Click(object sender, EventArgs e)
        {
            viderNouvelleCommande();
            zoneNewCmdEnable(true);

        }
        /// <summary>
        /// Evenement sur le bouton supprimer commmande livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Souhaitez-vous supprimer cette commande ?  ", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Commande commande = (Commande)bdgCommandes.List[bdgCommandes.Position];

                if (commande.Suivi.Id != "LI")
                {
                    zoneNewCmdEnable(false);
                    controle.deleteCmdLivre(commande);
                    remplirLstCmdLivres(controle.GetAllCommandes(), txtNumLivreCmd.Text);
                }
                else
                {
                    MessageBox.Show("Impossible de supprimer une commande déjà livrée");
                }

            }
        }
        //DVD-------------------------------------------------------------------------------------------------------------------
        //DVD--------------------------------------------------------------------------------------------------------------------
        //DVD--------------------------------------------------------------------------------------------------------------------
        //DVD-----------------------------------------------------------------------------------------------------------------
        //DVD-------------------------------------------------------------------------------------------------------------------
        //DVD-------------------------------------------------------------------------------------------------------------------
        //DVD--------------------------------------------------------------------------------------------------------------------
        //DVD-------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Recherche numero dvd et valorise les dgv 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercherDvd_Click(object sender, EventArgs e)
        {

            lesSuivis = controle.getAllSuivis();
            lesCommandes = controle.GetAllCommandes();
            lesDvd = controle.GetAllDvd();
            if (!txtNumDvd.Text.Equals(""))
            {


                List<Dvd> Dvds = new List<Dvd>();
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txtNumDvd.Text));
                if (dvd != null)
                {
                    if (txtNumDvd.Text != txtNumDvdCmd.Text)
                    {
                        Dvds.Add(dvd);
                        RemplirDvdListe(Dvds, dgvRechercheDvd);
                        txtNumDvdCmd.Text = dvd.Id;
                        txtNumDvdCmd.Enabled = false;
                        remplirLstCmdDvd(lesCommandes, dvd.Id);
                       zoneNewCmdDvdEnable(false);

                    }
                    else
                    {
                        MessageBox.Show("Le numero entrée est identique");
                    }

                }
                else
                {
                    MessageBox.Show("Numero introuvable");
                }


            }
        }
        /// <summary>
        /// Valorise les champs
        /// </summary>
        private void remplirInfoCommandeDvd()
        {

            Commande commande = (Commande)bdgCommandesDvd.List[bdgCommandesDvd.Position];
            List<Suivi> lstSuivisModif = new List<Suivi>();
            foreach (Suivi suivi in lesSuivis)
            {
                lstSuivisModif.Add(suivi);

            }
            Suivi unSuivi = lstSuivisModif.Find(x => x.Id.Equals(commande.Suivi.Id));

            if (unSuivi.Id == "EC" || unSuivi.Id == "REL")
            {
                lstSuivisModif.Remove(lesSuivis.Find(x => x.Id.Equals("REG")));

            }
            else if (unSuivi.Id == "REG" || unSuivi.Id == "LI")
            {
                lstSuivisModif.Remove(lesSuivis.Find(x => x.Id.Equals("REL")));
                lstSuivisModif.Remove(lesSuivis.Find(x => x.Id.Equals("EC")));
                if (unSuivi.Id == "REG")
                {
                    lstSuivisModif.Remove(lstSuivisModif.Find(x => x.Id.Equals("LI")));
                }

            }
            remplirCbxSuiviCmdDvd(lstSuivisModif);
            txtMontantDvd.Text = commande.Montant.ToString();
            txtNbExemplaireDvd.Text = commande.NbExemplaire.ToString();



        }
        /// <summary>
        /// Valorise la combo box suivi de dvd
        /// </summary>
        /// <param name="lstSuivisModif"></param>
        private void remplirCbxSuiviCmdDvd(List<Suivi> lstSuivisModif)
        {

            Commande commande = (Commande)bdgCommandesDvd.List[bdgCommandesDvd.Position];
            bdgSuivis.DataSource = lstSuivisModif;
            cbxSuiviDvd.DataSource = bdgSuivis;
            cbxSuiviDvd.SelectedIndex = cbxSuiviDvd.FindStringExact(commande.Suivi.Libelle);

        }
        /// <summary>
        /// Evenement sur creer nouvelle commande dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewCmdDvd_Click(object sender, EventArgs e)
        {
            remplirLstCmdDvd(controle.GetAllCommandes(), txtNumDvdCmd.Text);
            zoneNewCmdDvdEnable(true);
            viderNouvelleCommandeDvd();

        }

        /// <summary>
        /// Evenement sur btn annuler commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerCmdDvd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Souhaitez-vous annuler la saisie d'une commande ?  ", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                viderNouvelleCommandeDvd();
                zoneNewCmdDvdEnable(false);
            }

        }
        private void remplirLstCmdDvd(List<Commande> Commandes, string id)
        {

            List<Commande> CommandesOfId = Commandes.FindAll(x => x.IdLivreDvd.Equals(id));
            if (CommandesOfId.Count <= 0)
            {

                MessageBox.Show("Il n'existe pas ou plus de commande pour cette revue");


            }
            bdgCommandesDvd.DataSource = CommandesOfId;
            dgvCmdDvd.DataSource = bdgCommandesDvd;
            dgvCmdDvd.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


        }
        /// <summary>
        /// Determine les Zones qui accepte les actions utilisateurs 
        /// </summary>
        /// <param name="saisie"></param>
        private void zoneNewCmdDvdEnable(bool saisie)
        {

            if(txtMontantCmdLivre.Enabled)
            {
                viderNouvelleCommandeDvd();
            }
            txtNbExemplaireDvd.Enabled = saisie;
            txtMontantDvd.Enabled = saisie;
            cbxSuiviDvd.Enabled = !saisie;
            btnModifierCmdDvd.Enabled = !saisie;
            btnSupprimerBtnDvd.Enabled = !saisie;
            btnAnnulerCmdDvd.Enabled = saisie;
            btnValiderCmdDvd.Enabled = saisie;
            btnNewCmdDvd.Enabled = !saisie;
            if (dgvCmdDvd.CurrentCell == null)
            {
                btnModifierCmdDvd.Enabled = false;
                btnSupprimerBtnDvd.Enabled = false;
                cbxSuiviDvd.Enabled = false;

            }
            if (txtNumDvdCmd.Text == "")
            {
                btnNewCmdDvd.Enabled = false;
            }

        }

        /// <summary>
        /// VIde les chmpas pour une nouvelle commande de dvd
        /// </summary>
        private void viderNouvelleCommandeDvd()
        {
            txtNbExemplaireDvd.Text = "";
            txtMontantDvd.Text = "";
            cbxSuiviDvd.Text = "";
        }

        /// <summary>
        /// Evenement sur bouton valider commande dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValiderCmdDvd_Click(object sender, EventArgs e)
        {
            if (txtMontantDvd.Text != "" && txtNbExemplaireDvd.Text != "")
            {
                try
                {
                    if (MessageBox.Show("Souhaitez-vous creer une nouvelle commande ?  ", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Double montant = double.Parse(txtMontantDvd.Text);
                        int nbExemplaire = int.Parse(txtNbExemplaireDvd.Text);
                        String idLivreDvd = txtNumDvdCmd.Text;
                        string id = controle.getLastIdCommande();
                        Suivi unSuivi = lesSuivis.Find(x => x.Id.Equals("EC"));
                        Commande commande = new Commande(id, DateTime.Now, montant, nbExemplaire, idLivreDvd, unSuivi);
                        controle.CreerCommandeDocument(commande);
                        viderNouvelleCommandeDvd();
                        remplirLstCmdDvd(controle.GetAllCommandes(), txtNumDvdCmd.Text);

                        zoneNewCmdDvdEnable(false);


                    }
                }
                catch
                {

                    MessageBox.Show("Le nombre d'exemplaire doit etre numerique ", "Information");
                    Log.Information(" Le nombre d'exemplaire saisi n'etait pas numérique");
                }
            }
            else
            {
                MessageBox.Show("Certains champs sont vides", "Information");
            }

        }
        /// <summary>
        /// Evenement sur le bouton modifier commande dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifierCmdDvd_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Souhaitez-vous vraiment modifier l'etape de suivi de la commande ?  ", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Commande commande = (Commande)bdgCommandesDvd.List[bdgCommandesDvd.Position];
                Suivi unSuivi = lesSuivis.Find(x => x.Id.Equals(commande.Suivi.Id));
                if (unSuivi.Libelle != cbxSuiviDvd.Text)
                {
                    commande.Suivi = (Suivi)cbxSuiviDvd.SelectedItem;
                    controle.updateCommandeDocument(commande);
                    remplirLstCmdDvd(controle.GetAllCommandes(), txtNumDvdCmd.Text);
                }

            }

        }

        /// <summary>
        /// Evenement sur bouton supprimer commmande dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimerBtnDvd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Souhaitez-vous supprimer cette commande ?  ", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Commande commande = (Commande)bdgCommandesDvd.List[bdgCommandesDvd.Position];
                if (commande.Suivi.Id != "LI")
                {
                    zoneNewCmdDvdEnable(false);
                    controle.deleteCmdLivre(commande);
                    remplirLstCmdDvd(controle.GetAllCommandes(), txtNumDvdCmd.Text);
                }
                else
                {
                    MessageBox.Show("Impossible de supprimer une commande déjà livrée");
                }

            }
        }
        /// <summary>
        /// Evenement sur changement page dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandeDvd_Enter(object sender, EventArgs e)
        {
            txtNumDvdCmd.Enabled = false;
            zoneNewCmdDvdEnable(false);

        }
        /// <summary>
        /// Evenement sur changement de selection sur la dgv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCmdDvd_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCmdDvd.CurrentCell != null)
            {
                try
                {
                    remplirInfoCommandeDvd();
                }
                catch
                {
                    MessageBox.Show("Le remplissage des informations à échoué");
                    Log.Information("Le remplissage des information à échoué");
                }
            }
            zoneNewCmdDvdEnable(false);
        }


        /// <summary>
        /// Remplire liste commande revue
        /// </summary>
        /// <param name="id"></param>
        private void remplirLstCmdRevue(List<Abonnement> Abonnements, string id)
        {

            List<Abonnement> AbonnementsOfId = Abonnements.FindAll(x => x.IdRevue.Equals(id));
            if (AbonnementsOfId.Count <= 0)
            {

                MessageBox.Show("Il n'existe pas ou plus de commande pour cette revue");

            }
            bdgAbonnements.DataSource = AbonnementsOfId;
            dgvCmdRevues.DataSource = bdgAbonnements;

            dgvCmdRevues.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            zoneNewCmdRevueEnable(false);
        }
        /// <summary>
        /// Evenement sur le bouton recherche
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRechercherRevue_Click(object sender, EventArgs e)
        {

            lesAbonnements = controle.getAllAbonnements();
            lesRevues = controle.GetAllRevues();

            if (txtNumRevue.Text != "")
            {
                List<Revue> revues = new List<Revue>();
                Revue revue = lesRevues.Find(x => x.Id.Equals(txtNumRevue.Text));
                if (revue != null)
                {
                    if (revue.Id != txtNumCmdRevue.Text)
                    {

                        txtNumCmdRevue.Text = revue.Id;
                        revues.Add(revue);
                        RemplirRevuesListe(revues, dgvRechercheRevue);
                        remplirLstCmdRevue(controle.getAllAbonnements(), revue.Id);
                        zoneNewCmdRevueEnable(false);
                    }
                    else
                    {
                        MessageBox.Show("Le numero entrée est identique");
                    }

                }
                else
                {
                    MessageBox.Show("Le numero de revue saisi est invalide");
                }

            }
            else
            {
                MessageBox.Show("Le numero de revue saisi est nul");

            }


            if (!txtNumeroLivreRecherche.Text.Equals(""))
            {

                List<Livre> livres = new List<Livre>();
                Livre livre = lesLivres.Find(x => x.Id.Equals(txtNumeroLivreRecherche.Text));
                if (livre != null)
                {

                    livres.Add(livre);
                    RemplirLivresListe(livres, dgvSearchLivreCmd);
                    txtNumLivreCmd.Text = livre.Id;
                    txtNumLivreCmd.Enabled = false;
                    lesSuivis = controle.getAllSuivis();
                    List<Commande> CommandesOfId = lesCommandes.FindAll(x => x.IdLivreDvd.Equals(livre.Id));
                    bdgCommandes.DataSource = CommandesOfId;
                    dgvCmdRevues.DataSource = bdgCommandes;
                    dgvCmdRevues.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    if (dgvCmdRevues.CurrentCell != null)
                    {
                        remplirInfoCommande();
                    }
                    else
                    {
                        MessageBox.Show("numéro introuvable");

                    }

                }


            }

        }
        /// <summary>
        /// Evenement sur bouton valider revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCmdValiderCmdRevue_Click(object sender, EventArgs e)
        {
            if (txtCmdMontantRevue.Text != "")
            {
                try
                {

                    if (dateDebCmdRevue.Value.CompareTo(dateFinCmdRevue.Value) < 0)
                    {
                        if (MessageBox.Show("Souhaitez-vous creer une nouvelle commande ?  ", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Double montant = double.Parse(txtCmdMontantRevue.Text);
                            String idRevue = txtNumCmdRevue.Text;
                            string id = controle.getLastIdCommande();
                            DateTime dateFin = dateFinCmdRevue.Value;
                            Abonnement abonnement = new Abonnement(id, DateTime.Now, montant, dateFin, idRevue);
                            controle.creerCmdRevue(abonnement);
                            remplirLstCmdRevue(controle.getAllAbonnements(), txtNumCmdRevue.Text);
                            viderNouvelleAbonnement();
                            zoneNewCmdRevueEnable(false);
                        }
                    }
                    else
                    {
                        MessageBox.Show("La date doit être supérieur à la date de debut");
                    }




                }
                catch
                {

                    MessageBox.Show("Le montant doit etre numerique ", "Information");
                    Log.Information("Le montant saisi n'etait pas numérique ");
                }
            }
            else
            {
                MessageBox.Show("Certains champs sont vides", "Information");
            }
        }
        /// <summary>
        /// Evennement sur bouton creer nouvelle abonnement revue
        /// </summary>
        /// <param name="saisie"></param>
        private void zoneNewCmdRevueEnable(bool saisie)
        {
            if(txtCmdMontantRevue.Enabled)
            {
                viderNouvelleAbonnement();
            }
            txtCmdMontantRevue.Enabled = saisie;
            txtNumCmdRevue.Enabled = false;
            dateDebutCmdRevue.Enabled = false;
            btnAnnulerCmdRevue.Enabled = saisie;
            btnValiderCmdRevue.Enabled = saisie;
            btnRenouvellerCmdRevue.Enabled = !saisie;
            btnSupprimerCmdRevue.Enabled = !saisie;
            btnNewCmdRevue.Enabled = !saisie;
            dateFinCmdRevue.Enabled = true;
            if (dgvCmdRevues.CurrentCell == null)
            {
                btnRenouvellerCmdRevue.Enabled = false;
                dateFinCmdRevue.Enabled = saisie;
            }
            if(txtNumCmdRevue.Text == "")
            {
                btnNewCmdRevue.Enabled = false;
            }


        }
        /// <summary>
        /// Vide champs nouvelle abonnement
        /// </summary>
        private void viderNouvelleAbonnement()
        {
            txtCmdMontantRevue.Text = "";

        }
        /// <summary>
        /// Creer nouvelle commande revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewCmdRevue_Click(object sender, EventArgs e)
        {
            viderNouvelleAbonnement();
            zoneNewCmdRevueEnable(true);
        }
        /// <summary>
        /// Evenement sur annuler commande revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnulerCmdRevue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Souhaitez-vous annuler la saisie d'une commande ?  ", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                viderNouvelleAbonnement();
                zoneNewCmdRevueEnable(false);
                
            }
        }
        /// <summary>
        /// Evenement sur bouton renouveller abonnnement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRenouvellerCmdRevue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Souhaitez-vous vraiment renouveller cet abonnement?  ", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Abonnement abonnement = (Abonnement)bdgAbonnements.List[bdgAbonnements.Position];


                if (dateFinCmdRevue.Value.CompareTo(abonnement.DateFinAbonnement) > 0)
                {
                    abonnement.DateFinAbonnement = dateFinCmdRevue.Value;
                    controle.updateCmdRevue(abonnement);
                    remplirLstCmdRevue(controle.getAllAbonnements(), txtNumCmdRevue.Text);
                }
                else
                {
                    MessageBox.Show("La date de fin ne peut pas être inferieur ou égal à la date de fin initial");
                }



            }
        }
        /// <summary>
        /// Fonction qui determine is la date d'un exemplaire d'une revue est comprise dans la date de l'abonnement
        /// Renvoie true si vrai
        /// </summary>
        /// <param name="dateDebut"></param>
        /// <param name="dateFin"></param>
        /// <param name="dateParution"></param>
        /// <returns></returns>
        public bool isDeleteRevuePossible(DateTime dateDebut, DateTime dateFin, DateTime dateParution)
        {
            if (dateDebut.CompareTo(dateParution) <= 0 && dateFin.CompareTo(dateParution) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        /// <summary>
        /// Evenement sur bouton supprimer commande revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimerCmdRevue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Souhaitez-vous supprimer cette abonnement ?  ", "Confirmation de suppression", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Abonnement abonnement = (Abonnement)bdgAbonnements.List[bdgAbonnements.Position];
                List<Exemplaire> ExemplairesOfId = lesExemplaires.FindAll(x => x.IdDocument.Equals(abonnement.IdRevue));
                bool isDeletePossible = true;
                foreach (Exemplaire unExemplaire in ExemplairesOfId)
                {
                    if (isDeleteRevuePossible(dateDebCmdRevue.Value, dateFinCmdRevue.Value, unExemplaire.DateAchat))
                    {
                        isDeletePossible = false;
                    }

                }



                if (isDeletePossible)
                {
                    controle.deleteCmdRevue(abonnement);
                    remplirLstCmdRevue(controle.getAllAbonnements(), txtNumCmdRevue.Text);

                }
            }
            else
            {
                MessageBox.Show("Impossible de supprimer une commande de revue rattaché à des exemplaires de revues");
            }

        }
        /// <summary>
        /// Evenement sur changement de selection sur la dgv (valorise les champs)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCmdRevues_SelectionChanged(object sender, EventArgs e)
        {
            zoneNewCmdEnable(false);
            remplirZoneCmdRevue();



        }
        /// <summary>
        /// Remplir zone Commande revue
        /// </summary>
        private void remplirZoneCmdRevue()
        {
            if (bdgAbonnements.Count != 0)
            {
                Abonnement abonnement = (Abonnement)bdgAbonnements.List[bdgAbonnements.Position];
                txtCmdMontantRevue.Text = abonnement.Montant.ToString();
                dateDebCmdRevue.Value = abonnement.DateCommande;
                dateFinCmdRevue.Value = abonnement.DateFinAbonnement;
            }
        }
        /// <summary>
        /// Initialise certaines valeurs sur l'evenement de changement d'onglet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCmdRevue_Enter(object sender, EventArgs e)
        {
            lesAbonnements = controle.getAllAbonnements();
            zoneNewCmdRevueEnable(false);
            dateDebCmdRevue.Enabled = false;
            txtNumCmdRevue.Enabled = false;
          
        }
        /// <summary>
        /// Evenement sur click de colonne - tri .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCmdRevues_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            lesAbonnements = controle.getAllAbonnements();
            string titreColonne = dgvCmdRevues.Columns[e.ColumnIndex].HeaderText;

            List<Abonnement> sortedList = new List<Abonnement>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesAbonnements.OrderBy(o => o.Id).ToList();
                    break;
                case "DateCommande":
                    sortedList = lesAbonnements.OrderBy(o => o.DateCommande).ToList();
                    break;

                case "Montant":
                    sortedList = lesAbonnements.OrderBy(o => o.Montant).ToList();
                    break;
                case "DateFinAbonnement":
                    sortedList = lesAbonnements.OrderBy(o => o.DateFinAbonnement).ToList();
                    break;
                case "IdRevue":
                    sortedList = controle.getAllAbonnements();
                    break;

            }

            remplirLstCmdRevue(sortedList, txtNumCmdRevue.Text);
        }

        /// <summary>
        /// Tri colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCmdDvd_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {


            string titreColonne = dgvCmdDvd.Columns[e.ColumnIndex].HeaderText;
            List<Commande> sortedList = new List<Commande>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesCommandes.OrderBy(o => o.Id).ToList();
                    break;
                case "DateCommande":
                    sortedList = lesCommandes.OrderBy(o => o.DateCommande).ToList();
                    break;

                case "Montant":
                    sortedList = lesCommandes.OrderBy(o => o.Montant).ToList();
                    break;
                case "NbExemplaire":
                    sortedList = lesCommandes.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "IdLivreDvd":
                    sortedList = controle.GetAllCommandes();
                    break;
                case "Suivi":
                    sortedList = controle.GetAllCommandes();
                    break;

            }

            remplirLstCmdDvd(sortedList, txtNumDvdCmd.Text);

        }
        /// <summary>
        /// Tri colonnnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void dgvCommandeLivres_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {


            string titreColonne = dgvCommandeLivres.Columns[e.ColumnIndex].HeaderText;
            List<Commande> sortedList = new List<Commande>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesCommandes.OrderBy(o => o.Id).ToList();
                    break;
                case "DateCommande":
                    sortedList = lesCommandes.OrderBy(o => o.DateCommande).ToList();
                    break;

                case "Montant":
                    sortedList = lesCommandes.OrderBy(o => o.Montant).ToList();
                    break;
                case "NbExemplaire":
                    sortedList = lesCommandes.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "IdLivreDvd":
                    sortedList = controle.GetAllCommandes();
                    break;
                case "Suivi":
                    sortedList = controle.GetAllCommandes();
                    break;

            }


            remplirLstCmdLivres(sortedList, txtNumLivreCmd.Text);

        }

        private void TabCtrl_Enter(object sender, EventArgs e)
        {
            if(niveau != 3)
            {
                (new Alerte(controle)).Show();
            }

        }
    }
    
}
