using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using System.Data;
using BaseDeDonnees;
using System.Reflection;


namespace MaisonDesLigues
{
    internal abstract class Utilitaire
    {
        
        /// <summary>
        /// Cette méthode permet de renseigner les propriétés des contrôles à créer. C'est une partie commune aux 
        /// 3 types de participants : intervenant, licencié, bénévole
        /// </summary>
        /// <param name="UneForme">le formulaire concerné</param>  
        /// <param name="UnContainer">le panel ou le groupbox dans lequel on va placer les controles</param> 
        /// <param name="UnControleAPlacer"> le controle en cours de création</param>
        /// <param name="UnPrefixe">les noms des controles sont standard : NomControle_XX
        ///                                         ou XX estl'id de l'enregistrement récupéré dans la vue qui
        ///                                         sert de siurce de données</param> 
        /// <param name="UneLigne">un enregistrement de la vue, celle pour laquelle on crée le contrôle</param> 
        /// <param name="i"> Un compteur qui sert à positionner en hauteur le controle</param>   
        /// <param name="callback"> Le pointeur de fonction. En fait le pointeur sur la fonction qui réagira à l'événement déclencheur </param>
        private static void AffecterControle(Form UneForme, ScrollableControl UnContainer, ButtonBase UnControleAPlacer, String UnPrefixe, DataRow UneLigne, Int16 i, Action<object, EventArgs> callback)
        {
            UnControleAPlacer.Name = UnPrefixe + UneLigne[0];
            UnControleAPlacer.Width = 320;
            UnControleAPlacer.Text = UneLigne[1].ToString();
            UnControleAPlacer.Left = 13;
            UnControleAPlacer.Top = 5 +(10 * i);
            UnControleAPlacer.Visible = true;                  
            System.Type UnType = UneForme.GetType();
            //((UnType)UneForme).
            UnContainer.Controls.Add(UnControleAPlacer);
            
        }
        /// <summary>
        /// Créé une combobox dans un container avec le nom passé en paramètre
        /// </summary>
        /// <param name="UnContainer">panel ou groupbox</param> 
        /// <param name="unNom">nom de la groupbox à créer</param> 
        /// <param name="UnTop">positionnement haut dans le container  </param> 
        /// <param name="UnLeft">positionnement bas dans le container </param> 
        public static void CreerCombo(ScrollableControl UnContainer, String unNom, Int16 UnTop, Int16 UnLeft)
        {
            CheckBox UneCheckBox= new CheckBox();
            UneCheckBox.Name=unNom;
            UneCheckBox.Top=UnTop;
            UneCheckBox.Left=UnLeft;
            UneCheckBox.Visible=true;
            UnContainer.Controls.Add(UneCheckBox);
        }
        /// <summary>
        /// Cette méthode crée des controles de type chckbox ou radio button dans un controle de type panel.
        /// Elle va chercher les données dans la base de données et crée autant de controles (les uns au dessous des autres
        /// qu'il y a de lignes renvoyées par la base de données.
        /// </summary>
        /// <param name="UneForme">Le formulaire concerné</param> 
        /// <param name="UneConnexion">L'objet connexion à utiliser pour la connexion à la BD</param> 
        /// <param name="pUneTable">Le nom de la source de données qui va fournir les données. Il s'agit en fait d'une vue de type
        /// VXXXXOn ou XXXX représente le nom de la tabl à partir de laquelle la vue est créée. n représente un numéro de séquence</param>  
        /// <param name="pPrefixe">les noms des controles sont standard : NomControle_XX
        ///                                         ou XX estl'id de l'enregistrement récupéré dans la vue qui
        ///                                         sert de source de données</param>
        /// <param name="UnPanel">panel ou groupbox dans lequel on va créer les controles</param>
        /// <param name="unTypeControle">type de controle à créer : checkbox ou radiobutton</param>
        /// <param name="callback"> Le pointeur de fonction. En fait le pointeur sur la fonction qui réagira à l'événement déclencheur </param>
        public static void CreerDesControles(Form UneForme, Bdd UneConnexion, String pUneTable, String pPrefixe, ScrollableControl UnPanel, String unTypeControle, Action<object, EventArgs> callback)
        {
            DataTable UneTable = UneConnexion.ObtenirDonnesOracle(pUneTable);
            // on va récupérer les statuts dans un datatable puis on va parcourir les lignes(rows) de ce datatable pour 
            // construire dynamiquement les boutons radio pour le statut de l'intervenant dans son atelier


            Int16 i = 0;
            foreach (DataRow UneLigne in UneTable.Rows)
            {
                //object UnControle = Activator.CreateInstance(object unobjet, unTypeControle);
                //UnControle=Convert.ChangeType(UnControle, TypeC);
                  
                if (unTypeControle == "CheckBox")
                {
                    CheckBox UnControle = new CheckBox();
                    AffecterControle(UneForme, UnPanel, UnControle, pPrefixe, UneLigne, i++, callback);     
              
                }
                else if (unTypeControle == "RadioButton")
                {
                    RadioButton UnControle = new RadioButton();
                    AffecterControle(UneForme, UnPanel, UnControle, pPrefixe, UneLigne, i++, callback);
                    UnControle.CheckedChanged += new System.EventHandler(callback);
                }
                i++;
            }
            UnPanel.Height = 20 * i + 5;
        }
        /// <summary>
        /// méthode permettant de remplir une combobox à partir d'une source de données
        /// </summary>
        /// <param name="UneConnexion">L'objet connexion à utiliser pour la connexion à la BD</param>
        /// <param name="UneCombo"> La combobox que l'on doit remplir</param>
        /// <param name="UneSource">Le nom de la source de données qui va fournir les données. Il s'agit en fait d'une vue de type
        /// VXXXXOn ou XXXX représente le nom de la tabl à partir de laquelle la vue est créée. n représente un numéro de séquence</param>
        public static void RemplirComboBox(Bdd UneConnexion, ComboBox UneCombo, String UneSource)
        {

            UneCombo.DataSource = UneConnexion.ObtenirDonnesOracle(UneSource);
            UneCombo.DisplayMember = "libelle";
            UneCombo.ValueMember = "id";
        }
        /// <summary>
        /// Cette fonction va compter le nombre de controles types CheckBox qui sont cochées contenus dans la collection controls
        /// du container passé en paramètre
        /// </summary>
        /// <param name="UnControl"> le container sur lequel on va compter les controles de type checkbox qui sont checked</param>
        /// <returns>nombre  de checkbox cochées</returns>
        internal static int CompteChecked(ScrollableControl UnContainer)
        {
            Int16 i = 0;
            foreach (Control UnControle in UnContainer.Controls)
            {
                if (UnControle.GetType().Name == "CheckBox" && ((CheckBox)UnControle).Checked)
                {
                    i++;
                }
            }
            return i;
        }
    }
}
