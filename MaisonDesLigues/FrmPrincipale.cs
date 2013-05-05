using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.ObjectModel;
using ComposantNuite;
using BaseDeDonnees;
using aipsys.qrcode.encoder;
namespace MaisonDesLigues
{
    public partial class FrmPrincipale : Form
    {
        

        /// <summary>
        /// constructeur du formulaire
        /// </summary>
        public FrmPrincipale()
        {
            InitializeComponent();
        }
        private Bdd UneConnexion;
        private String TitreApplication;
        private String IdStatutSelectionne = "";
        /// <summary>
        /// création et ouverture d'une connexion vers la base de données sur le chargement du formulaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPrincipale_Load(object sender, EventArgs e)
        {
            UneConnexion = ((FrmLogin)Owner).UneConnexion;
            TitreApplication = ((FrmLogin)Owner).TitreApplication;
            this.Text = TitreApplication;
            UneConnexion.RemplirComboBoxAtelier(UneConnexion, comboBox_id_atelier_ajout_vac, "ATELIER");
            UneConnexion.RemplirComboBoxAtelier(UneConnexion, comboBox_id_atelier_modif_vac, "ATELIER");

            comboBox_enregistrement_participant.Text = "Veuillez choisir votre participant.";
            UneConnexion.RemplirComboBoxParcicipant(UneConnexion, comboBox_enregistrement_participant, "PARTICIPANT");
            
            

        }
        /// <summary>
        /// gestion de l'événement click du bouton quitter.
        /// Demande de confirmation avant de quitetr l'application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmdQuitter_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Voulez-vous quitter l'application ?", ConfigurationManager.AppSettings["TitreApplication"], MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                UneConnexion.FermerConnexion();
                Application.Exit();
            }
        }

        private void RadTypeParticipant_Changed(object sender, EventArgs e)
        {
            switch (((RadioButton)sender).Name)
            {
                case "RadBenevole":
                    this.GererInscriptionBenevole();
                    break;
                case "RadLicencie":
                    //this.GererInscriptionLicencie();
                    break;
                case "RadIntervenant":
                    this.GererInscriptionIntervenant();
                    break;
                default:
                    throw new Exception("Erreur interne à l'application");
            }
        }

        /// <summary>     
        /// procédure permettant d'afficher l'interface de saisie du complément d'inscription d'un intervenant.
        /// </summary>
        private void GererInscriptionIntervenant()
        {

            GrpBenevole.Visible = false;
            GrpIntervenant.Visible = true;
            PanFonctionIntervenant.Visible = true;
            GrpIntervenant.Left = 23;
            GrpIntervenant.Top = 264;
            Utilitaire.CreerDesControles(this, UneConnexion, "VSTATUT01", "Rad_", PanFonctionIntervenant, "RadioButton", this.rdbStatutIntervenant_StateChanged);
            Utilitaire.RemplirComboBox(UneConnexion, CmbAtelierIntervenant, "VATELIER01");

            CmbAtelierIntervenant.Text = "Choisir";

        }

        /// <summary>     
        /// procédure permettant d'afficher l'interface de saisie des disponibilités des bénévoles.
        /// </summary>
        private void GererInscriptionBenevole()
        {

            GrpBenevole.Visible = true;
            GrpBenevole.Left = 23;
            GrpBenevole.Top = 264;
            GrpIntervenant.Visible = false;

            Utilitaire.CreerDesControles(this, UneConnexion, "VDATEBENEVOLAT01", "ChkDateB_", PanelDispoBenevole, "CheckBox", this.rdbStatutIntervenant_StateChanged);
            // on va tester si le controle à placer est de type CheckBox afin de lui placer un événement checked_changed
            // Ceci afin de désactiver les boutons si aucune case à cocher du container n'est cochée
            foreach (Control UnControle in PanelDispoBenevole.Controls)
            {
                if (UnControle.GetType().Name == "CheckBox")
                {
                    CheckBox UneCheckBox = (CheckBox)UnControle;
                    UneCheckBox.CheckedChanged += new System.EventHandler(this.ChkDateBenevole_CheckedChanged);
                }
            }


        }
        /// <summary>
        /// permet d'appeler la méthode VerifBtnEnregistreIntervenant qui déterminera le statu du bouton BtnEnregistrerIntervenant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbStatutIntervenant_StateChanged(object sender, EventArgs e)
        {
            // stocke dans un membre de niveau form l'identifiant du statut sélectionné (voir règle de nommage des noms des controles : prefixe_Id)
            this.IdStatutSelectionne = ((RadioButton)sender).Name.Split('_')[1];
            BtnEnregistrerIntervenant.Enabled = VerifBtnEnregistreIntervenant();
        }
        /// <summary>
        /// Permet d'intercepter le click sur le bouton d'enregistrement d'un bénévole.
        /// Cetteméthode va appeler la méthode InscrireBenevole de la Bdd, après avoir mis en forme certains paramètres à envoyer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEnregistreBenevole_Click(object sender, EventArgs e)
        {
            Collection<Int16> IdDatesSelectionnees = new Collection<Int16>();
            Int64? NumeroLicence;
            if (TxtLicenceBenevole.MaskCompleted)
            {
                NumeroLicence = System.Convert.ToInt64(TxtLicenceBenevole.Text);
            }
            else
            {
                NumeroLicence = null;
            }


            foreach (Control UnControle in PanelDispoBenevole.Controls)
            {
                if (UnControle.GetType().Name == "CheckBox" && ((CheckBox)UnControle).Checked)
                {
                    /* Un name de controle est toujours formé come ceci : xxx_Id où id représente l'id dans la table
                     * Donc on splite la chaine et on récupére le deuxième élément qui correspond à l'id de l'élément sélectionné.
                     * on rajoute cet id dans la collection des id des dates sélectionnées
                        
                    */
                    IdDatesSelectionnees.Add(System.Convert.ToInt16((UnControle.Name.Split('_'))[1]));
                }
            }
            UneConnexion.InscrireBenevole(TxtNom.Text, TxtPrenom.Text, TxtAdr1.Text, TxtAdr2.Text != "" ? TxtAdr2.Text : null, TxtCp.Text, TxtVille.Text, txtTel.MaskCompleted ? txtTel.Text : null, TxtMail.Text != "" ? TxtMail.Text : null, System.Convert.ToDateTime(TxtDateNaissance.Text), NumeroLicence, IdDatesSelectionnees);

        }
        /// <summary>
        /// Cetet méthode teste les données saisies afin d'activer ou désactiver le bouton d'enregistrement d'un bénévole
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChkDateBenevole_CheckedChanged(object sender, EventArgs e)
        {
            BtnEnregistreBenevole.Enabled = (TxtLicenceBenevole.Text == "" || TxtLicenceBenevole.MaskCompleted) && TxtDateNaissance.MaskCompleted && Utilitaire.CompteChecked(PanelDispoBenevole) > 0;
        }
        /// <summary>
        /// Méthode qui permet d'afficher ou masquer le controle panel permettant la saisie des nuités d'un intervenant.
        /// S'il faut rendre visible le panel, on teste si les nuités possibles ont été chargés dans ce panel. Si non, on les charges 
        /// On charge ici autant de contrôles ResaNuit qu'il y a de nuits possibles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RdbNuiteIntervenant_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Name == "RdbNuiteIntervenantOui")
            {
                PanNuiteIntervenant.Visible = true;
                if (PanNuiteIntervenant.Controls.Count == 0) // on charge les nuites possibles possibles et on les affiche
                {
                    //DataTable LesDateNuites = UneConnexion.ObtenirDonnesOracle("VDATENUITE01");
                    //foreach(Dat
                    Dictionary<Int16, String> LesNuites = UneConnexion.ObtenirDatesNuites();
                    int i = 0;
                    foreach (KeyValuePair<Int16, String> UneNuite in LesNuites)
                    {
                        ComposantNuite.ResaNuite unResaNuit = new ResaNuite(UneConnexion.ObtenirDonnesOracle("VHOTEL01"), (UneConnexion.ObtenirDonnesOracle("VCATEGORIECHAMBRE01")), UneNuite.Value, UneNuite.Key);
                        unResaNuit.Left = 5;
                        unResaNuit.Top = 5 + (24 * i++);
                        unResaNuit.Visible = true;
                        //unResaNuit.click += new System.EventHandler(ComposantNuite_StateChanged);
                        PanNuiteIntervenant.Controls.Add(unResaNuit);
                    }

                }

            }
            else
            {
                PanNuiteIntervenant.Visible = false;

            }
            BtnEnregistrerIntervenant.Enabled = VerifBtnEnregistreIntervenant();

        }

        /// <summary>
        /// Cette procédure va appeler la procédure .... qui aura pour but d'enregistrer les éléments 
        /// de l'inscription d'un intervenant, avec éventuellment les nuités à prendre en compte        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEnregistrerIntervenant_Click(object sender, EventArgs e)
        {
            try
            {
                if (RdbNuiteIntervenantOui.Checked)
                {
                    // inscription avec les nuitées
                    Collection<Int16> NuitsSelectionnes = new Collection<Int16>();
                    Collection<String> HotelsSelectionnes = new Collection<String>();
                    Collection<String> CategoriesSelectionnees = new Collection<string>();
                    foreach (Control UnControle in PanNuiteIntervenant.Controls)
                    {
                        if (UnControle.GetType().Name == "ResaNuite" && ((ResaNuite)UnControle).GetNuitSelectionnee())
                        {
                            // la nuité a été cochée, il faut donc envoyer l'hotel et la type de chambre à la procédure de la base qui va enregistrer le contenu hébergement 
                            //ContenuUnHebergement UnContenuUnHebergement= new ContenuUnHebergement();
                            CategoriesSelectionnees.Add(((ResaNuite)UnControle).GetTypeChambreSelectionnee());
                            HotelsSelectionnes.Add(((ResaNuite)UnControle).GetHotelSelectionne());
                            NuitsSelectionnes.Add(((ResaNuite)UnControle).IdNuite);
                         }

                    }
                    if (NuitsSelectionnes.Count == 0)
                    {
                        MessageBox.Show("Si vous avez sélectionné que l'intervenant avait des nuités\n in faut qu'au moins une nuit soit sélectionnée");
                    }
                    else
                    {
                        UneConnexion.InscrireIntervenant(TxtNom.Text, TxtPrenom.Text, TxtAdr1.Text, TxtAdr2.Text != "" ? TxtAdr2.Text : null, TxtCp.Text, TxtVille.Text, txtTel.MaskCompleted ? txtTel.Text : null, TxtMail.Text != "" ? TxtMail.Text : null, System.Convert.ToInt16(CmbAtelierIntervenant.SelectedValue), this.IdStatutSelectionne, CategoriesSelectionnees, HotelsSelectionnes, NuitsSelectionnes);
                        MessageBox.Show("Inscription intervenant effectuée");
                    }
                }
                else
                { // inscription sans les nuitées
                    UneConnexion.InscrireIntervenant(TxtNom.Text, TxtPrenom.Text, TxtAdr1.Text, TxtAdr2.Text != "" ? TxtAdr2.Text : null, TxtCp.Text, TxtVille.Text, txtTel.MaskCompleted ? txtTel.Text : null, TxtMail.Text != "" ? TxtMail.Text : null, System.Convert.ToInt16(CmbAtelierIntervenant.SelectedValue), this.IdStatutSelectionne);
                    MessageBox.Show("Inscription intervenant effectuée");
                    
                }

                
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        /// <summary>
        /// Méthode privée testant le contrôle combo et la variable IdStatutSelectionne qui contient une valeur
        /// Cette méthode permetra ensuite de définir l'état du bouton BtnEnregistrerIntervenant
        /// </summary>
        /// <returns></returns>
        private Boolean VerifBtnEnregistreIntervenant()
        {
            return CmbAtelierIntervenant.Text !="Choisir" && this.IdStatutSelectionne.Length > 0;
        }
        /// <summary>
        /// Méthode permettant de définir le statut activé/désactivé du bouton BtnEnregistrerIntervenant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbAtelierIntervenant_TextChanged(object sender, EventArgs e)
        {
            BtnEnregistrerIntervenant.Enabled = VerifBtnEnregistreIntervenant();
        }

        private void groupBox_Atelier_Enter(object sender, EventArgs e)
        {

        }

        private void radioBtn_Atelier_CheckedChanged(object sender, EventArgs e)
        {
            groupBox_Atelier.Visible = true;
            groupBox_Vacation.Visible = false;
            groupBox_Theme.Visible = false;

        }

        private void radioBtn_Vacation_CheckedChanged(object sender, EventArgs e)
        {
            groupBox_Atelier.Visible = false;
            groupBox_Theme.Visible = false;
            groupBox_Vacation.Visible = true;

            UneConnexion.RemplirComboBoxAtelier(UneConnexion, comboBox_id_atelier_ajout_vac, "ATELIER");
            comboBox_id_atelier_ajout_vac.Text = "Veuillez choisir votre atelier :";

            UneConnexion.RemplirComboBoxAtelier(UneConnexion, comboBox_id_atelier_modif_vac, "ATELIER");
            comboBox_id_atelier_modif_vac.Text = "Veuillez choisir votre atelier :";

            dateTimePicker_heureDebut_ajout_vac.Format = DateTimePickerFormat.Custom;
            dateTimePicker_heureDebut_ajout_vac.CustomFormat = "dddd dd MMMM yyyy -- HH:mm:ss";

            dateTimePicker_heureFin_ajout_vac.Format = DateTimePickerFormat.Custom;
            dateTimePicker_heureFin_ajout_vac.CustomFormat = "dddd dd MMMM yyyy -- HH:mm:ss";

            dateTimePicker_heureDebut_modif_vac.Format = DateTimePickerFormat.Custom;
            dateTimePicker_heureDebut_modif_vac.CustomFormat = "dddd dd MMMM yyyy -- HH:mm:ss";

            dateTimePicker_heureFin_modif_vac.Format = DateTimePickerFormat.Custom;
            dateTimePicker_heureFin_modif_vac.CustomFormat = "dddd dd MMMM yyyy -- HH:mm:ss";


        }

        private void radioBtn_theme_CheckedChanged(object sender, EventArgs e)
        {
            groupBox_Theme.Visible = true;
            groupBox_Atelier.Visible = false;
            groupBox_Vacation.Visible = false;
            UneConnexion.RemplirComboBoxAtelier(UneConnexion, comboBox_Id_Atelier, "ATELIER");
            comboBox_Id_Atelier.Text = "Veuillez choisir votre atelier :";
            
        }

        private void groupBox_Vacation_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox_Theme_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            UneConnexion.majVacation(Convert.ToString(comboBox_id_atelier_modif_vac.SelectedValue), Convert.ToInt32(comboBox_num_modif_vac.SelectedValue), dateTimePicker_heureDebut_modif_vac.Value, dateTimePicker_heureFin_modif_vac.Value);
        }

        private void btn_Creer_Atelier_Click(object sender, EventArgs e)
        {
            UneConnexion.creerAtelier(textBox_libelle_atelier.Text, Convert.ToInt16(textBox_nbPlaces.Text));
        }

        private void btn_Creer_Theme_Click(object sender, EventArgs e)
        {
            UneConnexion.creerTheme(Convert.ToString(comboBox_Id_Atelier.SelectedValue), textBox_Libelle_Theme.Text);
        }

        private void comboBox_Id_Atelier_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker_heureDebut_ajout_vac_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btn_ajout_vac_Click(object sender, EventArgs e)
        {
            UneConnexion.creerVacation(Convert.ToString(comboBox_id_atelier_ajout_vac.SelectedValue), dateTimePicker_heureDebut_ajout_vac.Value, dateTimePicker_heureFin_ajout_vac.Value);
        }

        private void comboBox_id_atelier_modif_vac_SelectionChangeCommitted(object sender, EventArgs e)
        {
            UneConnexion.RemplirComboBoxVacation(UneConnexion, comboBox_num_modif_vac, Convert.ToString(comboBox_id_atelier_modif_vac.SelectedValue));
            comboBox_num_modif_vac.Text = "Veuillez choisir votre vacation :";
        }

        private void textBox_num_ajout_vac_TextChanged(object sender, EventArgs e)
        {

        }

        private void label_libelleAtelier_Click(object sender, EventArgs e)
        {

        }

        private void textBox_libelle_atelier_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker_heureDebut_modif_vac_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox_enregistrement_participant_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Méthode privée permettant d'enregistrer un nouveau participant en générant une clé wifi si voulu et en générant
        /// un QR code contenant l'id du participant
        /// </summary>
        private void btn_enregistre_participant_Click(object sender, EventArgs e)
        {
            UneConnexion.EnregistreParticipant(Convert.ToString(comboBox_enregistrement_participant.SelectedValue), DateTime.Now);

            if (radioBtn_oui.Checked == true)
            {
                UneConnexion.EnregistreParticipantCleWifi(Convert.ToString(comboBox_enregistrement_participant.SelectedValue), genererCleWifi());
            }

            //Génération du QR code
            ImageEncoder iee = new ImageEncoder();
            iee.AutoConfigurate = true;
            iee.ECI = Int32.Parse("-1");
            iee.Encoding = 1;
            iee.Fnc1Mode = 1;
            iee.ErrorCorrectionLevel = 1;
            iee.Version = 1;
            iee.ProcessTilde = false;
            iee.MarginSize = Int32.Parse("10");
            iee.ModuleSize = Int32.Parse("5");
            iee.StructuredAppend = false;
            iee.StructuredAppendCounter = Int32.Parse("0");
            iee.StructuredAppendIndex = Int32.Parse("0");
            iee.TextData = "Participant " + Convert.ToString(comboBox_enregistrement_participant.SelectedValue);
            pictureBox_qr_code.Image = iee.Encode2Image();

        }

        /// <summary>
        /// Méthode privée permettant de générer une clé wifi aléatoire sur 24 caractères
        /// </summary>
        ///<returns>Retourne la clé wifi générée</returns>
        private string genererCleWifi()
        {
            string carac = "ABCDEF0123456789";
            int nbcarac = 24;
            string clef = "";
            Random aRand = new Random ();

            for (int i = 0; i < nbcarac; i++)
            {
                clef += carac.Substring(aRand.Next(16),1);
            }
            return clef;
        }

    }
}
