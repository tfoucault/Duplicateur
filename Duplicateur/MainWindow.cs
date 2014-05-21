using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Duplicateur
{
    public partial class MainWindow : Form
    {
        //HashTable pour les groupes de liste
        Hashtable groups = new Hashtable();

        //Acces au file manager
        FileManager fileMgr = new FileManager();

        //Liste des clés usb
        Hashtable usbList = new Hashtable();

        //Variable indice listview cles usb
        int previousClesUsbIndex = -1;

        //Usb courant selectionné
        Usb currentUsb = null;

        //Variable modif check list or global
        bool modifCheckFromList = false;
        bool modifCheckGlobal = false;

        public MainWindow()
        {
            InitializeComponent();
            initListSelection();
            initListClesUsb();
        }

        /*Fonction d'initialisation de la liste de selection
         * Crée les headers et parametre le comportement de
         * la Listview (Selection, Click, ....)
         * */
        private void initListSelection()
        {
            //Initialisation de la listview

            // Set the view to show details.
            listeSelection.View = View.Details;

            // Create and initialize column headers for myListView.
            ColumnHeader columnHeader0 = new ColumnHeader();
            columnHeader0.Text = "Nom";
            columnHeader0.Width = -2;
            ColumnHeader columnHeader1 = new ColumnHeader();
            columnHeader1.Text = "Taille";
            columnHeader1.Width = -2;
            ColumnHeader columnHeader2 = new ColumnHeader();
            columnHeader2.Text = "Date de modification";
            columnHeader2.Width = -2;

            //Ajout des headers a la liste de selection
            listeSelection.Columns.AddRange(new ColumnHeader[] 
            { columnHeader0, columnHeader1, columnHeader2 }
            );
        }

        /*Fonction d'initialisation de la liste des clés usb
         * branchées sur le hub
         * */
        private void initListClesUsb()
        {
            // Set the view to show details.
            listeClesUsb.View = View.Details;
            // Allow the user to rearrange columns.
            listeClesUsb.AllowColumnReorder = true;
            // Display check boxes.
            listeClesUsb.CheckBoxes = true;
            // Select the item and subitems when selection is made.
            listeClesUsb.FullRowSelect = true;
            // Sort the items in the list in ascending order.
            listeClesUsb.Sorting = SortOrder.Ascending;

            //Ajout des headers dans la liste view des cles usb
            listeClesUsb.Columns.Add("Racine", -2, HorizontalAlignment.Left);
            listeClesUsb.Columns.Add("Espace total", -2, HorizontalAlignment.Left);
            listeClesUsb.Columns.Add("Espace libre", -2, HorizontalAlignment.Left);
            listeClesUsb.Columns.Add("Format", -2, HorizontalAlignment.Center);

            List<DriveInfo> usbList = fileMgr.getUsbList();

            foreach (DriveInfo di in usbList)
            {
                //On crée un objet de type usb
                string usbName = di.Name.Substring(0, 1);
                char usbLetter = usbName[0];

                Usb usb = new Usb(usbLetter);
                this.usbList.Add(di.Name, usb);

                ListViewItem item = new ListViewItem();
                item.Tag = di.Name;
                item.Text = di.Name;
                item.SubItems.Add(usb.getTotalSizeStr());
                item.SubItems.Add(usb.getFreeSpaceStr());
                item.SubItems.Add(usb.getFormat());

                listeClesUsb.Items.Add(item);
            }
        }

        /*Fonction qui permet de selectionner un dossier et d'ajout 
         * son contenu à la liste de selection
         * */
        private void btnSelectionDossier_Click(object sender, EventArgs e)
        {
            //On ouvre une boite de dialogue pour la selection de dossier
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                //On récupere le chemin absolu du dossier selectionne
                String folderPath = fbd.SelectedPath;

                //On cree un objet d'information sur le repertoire
                DirectoryInfo root = new DirectoryInfo(folderPath);

                //Si le dossier est déja présent dans la liste
                if (estDansListeSelection(root.FullName)) return;

                //On prépare un group d'item pour la selection
                ListViewGroup lvg;

                //Si le group n'existe pas déja
                if (!groups.Contains(root.FullName))
                {
                    //On ajoute le groupe à la liste view
                    lvg = new ListViewGroup(root.Name);
                    listeSelection.Groups.Add(lvg);

                    //On ajoute le groupe au hashtable
                    groups.Add(root.FullName, lvg);
                }
                else
                {
                    lvg = (ListViewGroup)groups[root.FullName];
                }

                //On recupère les sous dossiers
                DirectoryInfo[] dil = root.GetDirectories();

                foreach (DirectoryInfo di in dil)
                {
                    if(!estDansListeSelection(di.FullName)){

                        //On prépare un item à ajouter a la liste
                        ListViewItem item = new ListViewItem();

                        //On renseigne l'item avec les infos du Dossier
                        item.Tag = di.FullName;
                        item.Text = di.Name;
                        item.SubItems.Add("");
                        item.SubItems.Add(di.LastAccessTime.ToString());
                        listeSelection.Items.Add(item);

                        //On ajoute l'item crée au groupe parent
                        item.Group = lvg;
                    }
                }

                FileInfo[] fil = root.GetFiles();

                foreach(FileInfo fi in fil)
                {
                    if(!estDansListeSelection(fi.FullName)){

                        //On prépare un item à ajouter a la liste
                        ListViewItem item = new ListViewItem();

                        //On renseigne l'item avec les infos du fichier
                        item.Tag = fi.FullName;
                        item.Text = fi.Name;
                        item.SubItems.Add(fi.Length.ToString());
                        item.SubItems.Add(fi.LastAccessTime.ToString());
                        listeSelection.Items.Add(item);

                        //On ajoute l'item crée au groupe parent
                        item.Group = lvg;
                    }
                }                
            }
        }

        /*Fonction qui permet de selection un ou plusieurs fichier et de
         * les ajouter à la liste de selection des fichier et dossiers
         * */
        private void btnSelectionFichiers_Click(object sender, EventArgs e)
        {
            //On ouvre une boite de dialogue pour la selection de fichier
            OpenFileDialog fd = new OpenFileDialog();
            fd.Multiselect = true;

            //Si l'utilisateur clique sur ok
            if (fd.ShowDialog() == DialogResult.OK)
            {
                //On récupére les fichiers selectionnés
                String[] selectedFiles = fd.FileNames;

                //Pour chaque element retourne
                foreach (String file in selectedFiles)
                {

                    //On crée un objet d'informations sur le fichier
                    FileInfo fi = new FileInfo(file);

                    //On ajoute le fichier si  : 
                    //Son parent n'est pas dans la liste
                    //Le fichier n'est pas dans la liste

                    if (!estDansListeSelection(fi.DirectoryName)
                        && !estDansListeSelection(fi.FullName))
                    {

                        //On prépare un group d'item pour la selection
                        ListViewGroup lvg;

                        //Si le group n'existe pas déja
                        if (!groups.Contains(fi.Directory.FullName))
                        {
                            //On ajoute le groupe à la liste view
                            lvg = new ListViewGroup(fi.Directory.Name);
                            listeSelection.Groups.Add(lvg);

                            //On ajoute le groupe au hashtable
                            groups.Add(fi.Directory.FullName, lvg);
                        }
                        else
                        {
                            lvg = (ListViewGroup)groups[fi.Directory.FullName];
                        }


                        //On crée un item de type fichier
                        ListViewItem item = new ListViewItem();
                        item.Tag = fi.FullName;
                        item.Text = fi.Name;
                        item.SubItems.Add(fi.Length.ToString());
                        item.SubItems.Add(fi.LastAccessTime.ToString());

                        //Ajout de l'item à liste de selection
                        listeSelection.Items.Add(item);

                        //On ajoute l'item crée au groupe parent
                        item.Group = lvg;
                    }
                }
            }
        }

        /*Fonction qui verifie si un item est présent dans la liste de 
         * selection à l'aide du chemin absolu correspondant au fichier
         * à ajouter (item.Tag contient le chemin absolu dans l'item)
         * */
        private bool estDansListeSelection(String path)
        {
            foreach (ListViewItem item in listeSelection.Items)
            {
                if ((string)item.Tag == path) return true;
            }

            return false;
        }

        /*Fonction qui permet de supprimer l'ensemble des items 
         * sélectionnés dans la liste de selections de fichiers
         * */
        private void btnSupprSelection_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem item in listeSelection.SelectedItems){

                listeSelection.Items.Remove(item);
            }
        }

        /*Fonction qui gère l'action "Selectionner toutes les cles usb"
         * */
        private void checkBoxToutesCles_CheckedChanged(object sender, EventArgs e)
        {
            //Si l'etat passe de non coché à coché
            if (checkBoxToutesCles.CheckState == CheckState.Checked)
            {
                //On coche tous les items de la listview cles usb
                foreach (ListViewItem item in listeClesUsb.Items)
                {
                    item.Checked = true;
                }
            }
            else
            {
                //On décoche tous les items de la listeview cles usb
                foreach (ListViewItem item in listeClesUsb.Items)
                {
                    item.Checked = false;
                }
            }
        }

        /*Fonction qui permet de charger les paramètres propres à chaque clés
         * lors du changement de selection d'un item dans la liste des cles usb
         * */
        private void listeClesUsb_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            //Si première selection ou selection change
            if (e.ItemIndex != previousClesUsbIndex)
            {                
                //On mémorise le nouvel index de clé
                previousClesUsbIndex = e.ItemIndex;
                string key = e.Item.Tag.ToString();

                //Récupération de l'usb selectionné
                currentUsb = (Usb)usbList[key];

                //Chargement paramétrage
                if (currentUsb.copyToRoot)
                {
                    radioButtonChoixRacine.Checked = true;
                    radioButtonChoixDossier.Checked = false;
                }
                else
                {
                    radioButtonChoixRacine.Checked = false;
                    radioButtonChoixDossier.Checked = true;
                }

                if (currentUsb.sendNotification)
                {
                    radioButtonNotifOui.Checked = true;
                    radioButtonNotifNon.Checked = false;
                }
                else
                {
                    radioButtonNotifOui.Checked = false;
                    radioButtonNotifNon.Checked = true;
                }

                //Affiche le chemin du dossier de destination
                textBoxChoixDossier.Text = currentUsb.getDestinationPath();

                //Mise à jour du dossier à créer si paramétré pour la clé
                textBoxCreerDossier.Text = currentUsb.getFolderToCreate();
                checkBoxCreerDossier.Checked = currentUsb.createFolder;

                //Affiche l'adresse mail renseigné pour les notifications
                textBoxMail.Text = currentUsb.getNotifMailAddress();

                //On active/desactive le groupe de parametrage en fonction
                //de l'état coché ou non de l'item représentant la clé usb
                groupBoxParamCle.Enabled = e.Item.Checked;
            }
        }

        /*Fonction qui gere l'action "Copier à la racine" et met a jour le 
         * paramétrage pour l'objet usb correspondant dans la liste des usb
         * */
        private void radioButtonChoixRacine_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonChoixRacine.Checked)
            {
                currentUsb.copyToRoot = true;
            }
            else
            {
                currentUsb.copyToRoot = false;
            }
        }

        /*Fonction qui gere l'action "Choisir un dossier" pour la destination du
         * support à paramétrer
         * */
        private void radioButtonChoixDossier_CheckedChanged(object sender, EventArgs e)
        {
            //Si le bouton passe à l'état coché
            if (radioButtonChoixDossier.Checked)
            {
                //On active le champ de saisie
                textBoxChoixDossier.Enabled = true;
                //On active le bouton parcourir
                btnChoixDossier.Enabled = true;
            }
            else
            {
                //On desactive le champs de saisie
                textBoxChoixDossier.Enabled = false;
                //On desactive le bouton parcourir
                btnChoixDossier.Enabled = false;
            }
        }

        /*Fonction qui gère l'action "Cocher une clé usb
         * */
        private void listeClesUsb_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //Si l'etat coché de la clé selectionnée change on active/desative
            //le groupe de paramétrage pour la clé selectionnée
            if(e.Item.Selected) groupBoxParamCle.Enabled = e.Item.Checked;

            /*
            if (!modifCheckGlobal || modifCheckFromList)
            {
                //On regarde le nombre de cles selectionnées
                int nbCles = nombreClesSelectionnes();

                if (nbCles == 0) checkBoxToutesCles.CheckState = CheckState.Unchecked;
                else if (nbCles > 0 && nbCles < listeClesUsb.Items.Count) checkBoxToutesCles.CheckState = CheckState.Indeterminate;
                else checkBoxToutesCles.CheckState = CheckState.Checked;

                modifCheckGlobal = true;
            }
             * */
        }

        /*Fonction qui permet de choisir un dossier de destination
         * */
        private void btnChoixDossier_Click(object sender, EventArgs e)
        {
            //On ouvre un dialog pour le choix d'un dossier de desstination
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                //Une fois la séléction validée, on ajoute le chemin absolu au champ de saisie
                textBoxChoixDossier.Text = fbd.SelectedPath;
                //On ajoute le chemin comme fichier de destination pour le usb courant
                currentUsb.setDestinationPath(fbd.SelectedPath);
            }
        }

        /*Fonction qui gère l'action "Crer un dossier"
         * */
        private void checkBoxCreerDossier_CheckedChanged(object sender, EventArgs e)
        {
            //Si l'état passe de coché à non coché
            if (checkBoxCreerDossier.CheckState == CheckState.Checked)
            {
                //On active le champs de saisie
                textBoxCreerDossier.Enabled = true;

                //Mise à jour paramétrage clé usb
                currentUsb.createFolder = true;
            }
            else
            {
                //On desactive le champs de saisie
                textBoxCreerDossier.Enabled = false;

                //Mise à jour paramétrage clé usb
                currentUsb.createFolder = false;
            }
        }

        /*Fonction qui gere le changement du nom de dossier a creer
         * */
        private void textBoxCreerDossier_TextChanged(object sender, EventArgs e)
        {
            //mise à jour paramétrage clé usb selectionnée
            currentUsb.setFolderToCreate(textBoxCreerDossier.Text);
        }
        
        /*Fonction qui retourne le nombre de clés usb selectionnés
         * */
        private int nombreClesSelectionnes()
        {
            int nombreCles = 0;

            foreach (ListViewItem item in listeClesUsb.Items)
            {
                if (item.Checked) ++nombreCles;
            }

            return nombreCles;
        }

        /*Desactivation des notifications
         * */
        private void radioButtonNotifNon_CheckedChanged(object sender, EventArgs e)
        {
            //Application des modifications
            if (radioButtonNotifNon.Checked)
            {
                textBoxMail.Enabled = false;
                currentUsb.sendNotification = false;
            }
        }

        /*Activation des notifications
         * */
        private void radioButtonNotifOui_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonNotifOui.Checked)
            {
                textBoxMail.Enabled = true;
                currentUsb.sendNotification = true;
            }
        }

        /*Fonction de sauvegarde automatique de l'adresse mail
         * */
        private void textBoxMail_TextChanged(object sender, EventArgs e)
        {
            //mise à jour paramétrage clé usb selectionnée
            currentUsb.setNotifMailAddress(textBoxMail.Text);
        }
    }
}
