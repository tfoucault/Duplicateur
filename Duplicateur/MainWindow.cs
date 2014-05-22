using Dolinay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Duplicateur
{
    public partial class MainWindow : Form
    {
        //HashTable pour les groupes de liste
        private Hashtable groups = new Hashtable();

        //Acces au file manager
        private FileManager fileMgr = new FileManager();

        //Liste des clés usb
        private Hashtable usbList = new Hashtable();

        //Variable indice listview cles usb
        private int previousClesUsbIndex = -1;

        //Usb courant selectionné
        private Usb currentUsb = null;

        //Variable qui contient la taille de la liste
        //de selection exprimée en octets
        private long selectionSize = -1;
        private List<String> selectionPaths = new List<String>();

        //private Thread selectionSizeThread = null;
        private volatile bool selectionListeUpdated = false;

        private DriveDetector driveDetector = null;

        public MainWindow()
        {
            InitializeComponent();
            initListSelection();
            initListClesUsb();
            driveDetector = new DriveDetector();
            driveDetector.DeviceArrived += new DriveDetectorEventHandler(OnDriveArrived);
            driveDetector.DeviceRemoved += new DriveDetectorEventHandler(OnDriveRemoved);
            driveDetector.QueryRemove += new DriveDetectorEventHandler(OnQueryRemove);
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
            listeClesUsbFormatage.View = View.Details;
            listViewEjection.View = View.Details;
            // Allow the user to rearrange columns.
            listeClesUsb.AllowColumnReorder = true;
            listeClesUsbFormatage.AllowColumnReorder = true;
            listViewEjection.AllowColumnReorder = true;
            // Display check boxes.
            listeClesUsb.CheckBoxes = true;
            listeClesUsbFormatage.CheckBoxes = true;
            listViewEjection.CheckBoxes = true;
            // Select the item and subitems when selection is made.
            listeClesUsb.FullRowSelect = true;
            listeClesUsbFormatage.FullRowSelect = true;
            listViewEjection.FullRowSelect = true;
            // Sort the items in the list in ascending order.
            listeClesUsb.Sorting = SortOrder.Ascending;
            listeClesUsbFormatage.Sorting = SortOrder.Ascending;
            listViewEjection.Sorting = SortOrder.Ascending;

            //Ajout des headers dans la liste view des cles usb
            listeClesUsb.Columns.Add("Racine", -2, HorizontalAlignment.Left);
            listeClesUsb.Columns.Add("Espace total", -2, HorizontalAlignment.Left);
            listeClesUsb.Columns.Add("Espace libre", -2, HorizontalAlignment.Left);
            listeClesUsb.Columns.Add("Format", -2, HorizontalAlignment.Center);

            //Ajout des headers dans la liste view des cles usb formatage
            listeClesUsbFormatage.Columns.Add("Racine", -2, HorizontalAlignment.Left);
            listeClesUsbFormatage.Columns.Add("Espace total", -2, HorizontalAlignment.Left);
            listeClesUsbFormatage.Columns.Add("Espace libre", -2, HorizontalAlignment.Left);
            listeClesUsbFormatage.Columns.Add("Format", -2, HorizontalAlignment.Center);

            //Ajout des headers dans la liste view des cles usb ejection
            listViewEjection.Columns.Add("Racine", -2, HorizontalAlignment.Left);
            listViewEjection.Columns.Add("Espace total", -2, HorizontalAlignment.Left);
            listViewEjection.Columns.Add("Espace libre", -2, HorizontalAlignment.Left);
            listViewEjection.Columns.Add("Format", -2, HorizontalAlignment.Center);

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
                item = new ListViewItem();
                item.Tag = di.Name;
                item.Text = di.Name;
                item.SubItems.Add(usb.getTotalSizeStr());
                item.SubItems.Add(usb.getFreeSpaceStr());
                item.SubItems.Add(usb.getFormat());
                listeClesUsbFormatage.Items.Add(item);

                item = new ListViewItem();
                item.Tag = di.Name;
                item.Text = di.Name;
                item.SubItems.Add(usb.getTotalSizeStr());
                item.SubItems.Add(usb.getFreeSpaceStr());
                item.SubItems.Add(usb.getFormat());
                listViewEjection.Items.Add(item);
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

                //On tague le group comme dossier
                lvg.Tag = "Folder";

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

                /*
                //On met à jour la nouvelle taille de la liste de selection
                updateSelectionPaths();

                //Si un thread est en cours
                if (selectionSizeThread != null && selectionSizeThread.IsAlive)
                {
                    //On arrete le thread en cours
                    selectionListeUpdated = true;
                    MessageBox.Show("Thread en cours. Annulation ...");

                    //On attend que le thread se termine
                    while (selectionSizeThread.ThreadState != ThreadState.Stopped)
                    {
                        //Do nothing
                    }

                    MessageBox.Show("Thread arrété");
                }
                else
                {
                    MessageBox.Show("Demarrage nouveau thread");
                    selectionSizeThread = new Thread(updateSelectionSize);
                    selectionSizeThread.Start();
                }
                //string selectSizeStr = Math.Round(((decimal)selectionSize) / (1024*1024)) + " Mo";
                //string selectSizeStr = selectionSize + " octets";
                //MessageBox.Show(selectSizeStr);
                 * */
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

                        //Si le groupe n'existe pas déja
                        if (!groups.Contains(fi.Directory.FullName))
                        {
                            //On ajoute le groupe à la liste view
                            lvg = new ListViewGroup(fi.Directory.Name);
                            lvg.Tag = "Files";
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

                //On calcule la nouvelle taille de la liste de selection
                //long selectionSize = getSelectionSize();
                //string selectSizeStr = selectionSize + " Octets";

                //MessageBox.Show(selectSizeStr);
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

            //On met à jour le statut is selected de la clé coche/décochée
            //MessageBox.Show("Activation / desactivation de " + e.Item.Tag.ToString());
            Usb usb = (Usb)usbList[e.Item.Tag.ToString()];
            usb.isSelected = e.Item.Checked;
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
   
        /*Fonction pour actualiser la taille de la liste de selection
         * */
        public void updateSelectionSize()
        {
            selectionSize = -1;

            string[] tempList = new string[selectionPaths.Count];
            selectionPaths.CopyTo(tempList);

            foreach (string item in tempList)
            {
                if (selectionListeUpdated) break;

                if (Directory.Exists(item))
                {
                    if (selectionListeUpdated) break;
                    selectionSize += getFolderSize(item);
                }
                else
                {
                    if (selectionListeUpdated) break;
                    FileInfo fi = new FileInfo(item);
                    selectionSize += fi.Length;
                }
            }

            MessageBox.Show("Size of selection have been update : " + selectionSize + " octets");
        }

        /*Fonction pour actualiser la taille de la liste des chemins de fichier de la liste
         * */
        private void updateSelectionPaths()
        {
            selectionPaths.Clear();

            foreach (ListViewItem item in listeSelection.Items)
            {
                selectionPaths.Add(item.Tag.ToString());
            }
        }

        /*Fonction qui permet de calculer recursivement la taille d'un dossier
         * */
        private long getFolderSize(String path)
        {
            long folderSize = -1;

            if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);

                try
                {
                    FileInfo[] files = dir.GetFiles();

                    foreach (FileInfo file in files)
                    {
                        folderSize += file.Length;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                try
                {
                    DirectoryInfo[] directories = dir.GetDirectories();

                    foreach (DirectoryInfo directory in directories)
                    {
                        folderSize += getFolderSize(directory.FullName);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return folderSize;
        }

        /* Fonction qui permet de calculer la taille de l'ensemble des fichiers à copier
         * */
        private long getSelectionSize()
        {
            long size = -1;

            foreach (ListViewItem item in listeSelection.Items)
            {
                if (selectionListeUpdated) break;

                if (Directory.Exists(item.Tag.ToString()))
                {
                    size += getFolderSize(item.Tag.ToString());
                }
                else
                {
                    FileInfo fi = new FileInfo(item.Tag.ToString());
                    size += fi.Length;
                }
            }

            //MessageBox.Show("Size of selection have been update : " + size + " octets");

            return size;
        }

        /* Fonction qui permet de vérifier que toutes les contraintes sont
         * remplies avant de pouvoir commencer la copie des fichiers
         * */
        private void buttonLancerCopie_Click(object sender, EventArgs e)
        {
            //Initialisation d'un message d'erreur dans le cas ou la copie n'est pas possible
            string errorMessage = "";

            /**********************************************************************/
            if (listeSelection.Items.Count == 0)
            {
                errorMessage = "Aucun fichier à copier";
                MessageBox.Show(errorMessage);
                return;
            }

            /**********************************************************************/
            //On vérifie qu'au moins une clé est selectionnée

            int nbCles = 0;

            foreach (string key in usbList.Keys)
            {
                Usb usb = (Usb)usbList[key];
                if (usb.isSelected) nbCles++;
            }

            if (nbCles == 0)
            {
                errorMessage = "Au moins une clé doit être séléctionnée";
                MessageBox.Show(errorMessage);
                return;
            }
            /**********************************************************************/
            //Verification de l'espace libre sur les cles de destination selectionnées

            bool enoughSpace = true;

            //On calcule la taille de la selection à copier
            long selectionSize = getSelectionSize();

            //Pour chaque clé, on vérifie qu'assez d'espace est dispo
            foreach (string key in usbList.Keys)
            {
                Usb usb = (Usb)usbList[key];

                if (usb.isSelected && usb.getFreeSpace() <= selectionSize)
                {
                    errorMessage += "La clé " + usb.driveLetter + " ne contient pas assez d'espace pour la copie\n";
                    enoughSpace = false;
                }
            }

            if (!enoughSpace)
            {
                MessageBox.Show(errorMessage);
                return;
            }

            /**********************************************************************/
            //Verification des adresses email renseignées pour les clés selectionnées

            bool validEmails = true;

            //Pour chaque clé, on vérifie le mail
            foreach (string key in usbList.Keys)
            {
                Usb usb = (Usb)usbList[key];

                //Si envoi de mail activé
                if (usb.sendNotification)
                {
                    string email = usb.getNotifMailAddress();

                    if((email.Length == 0) || !isValidEmail(email))
                    {
                        validEmails = false;
                        errorMessage += "Email invalide pour la clé " + usb.driveLetter + "\n";
                    }
                }
            }

            if (!validEmails)
            {
                MessageBox.Show(errorMessage);
                return;
            }

            /**********************************************************************/

            //Si la copie est possible, on crée une nouvelle fenetre de chargement
            //et on initialise avec la liste des fichiers et des destinations
            MessageBox.Show("Demarrage de la copie");

            //On récupere la liste des chemin à copier
            Hashtable groupList = new Hashtable();

            foreach (ListViewGroup group in listeSelection.Groups)
            {
                String groupFolder = group.Header;

                List<String> pathList = new List<String>();

                foreach (ListViewItem item in group.Items)
                {
                    pathList.Add(item.Tag.ToString());
                }

                groupList.Add(groupFolder, pathList);
            }

            //On recupère la liste des usb cochés
            List<Usb> usbChecked = new List<Usb>();

            foreach (string key in usbList.Keys)
            {
                //On récupere le usb en cours
                Usb usb = (Usb)usbList[key];

                //Si l'usb est coché
                if (usb.isSelected)
                {
                    //On l'ajoute à la liste
                    usbChecked.Add(usb);
                }
            }

            //On transmet la liste des fichier et clés à la fenetre de copy
            Chargement copyWindow = new Chargement(groupList, usbChecked);

            //On affiche la fenetre modale
            copyWindow.ShowDialog();
        }

        // Called by DriveDetector when removable device in inserted 
        private void OnDriveArrived(object sender, DriveDetectorEventArgs e)
        {

            DriveInfo di = new DriveInfo(e.Drive);
            string usbName = e.Drive.Substring(0, 1);
            char usbLetter = usbName[0];
            Usb usb = new Usb(usbLetter);
            usbList.Add(e.Drive, usb);
            ListViewItem item = new ListViewItem();
            item.Tag = di.Name;
            item.Text = di.Name;
            item.SubItems.Add(usb.getTotalSizeStr());
            item.SubItems.Add(usb.getFreeSpaceStr());
            item.SubItems.Add(usb.getFormat());
            listeClesUsb.Items.Add(item);
            
        }

        // Called by DriveDetector after removable device has been unpluged 
        private void OnDriveRemoved(object sender, DriveDetectorEventArgs e)
        {
            // TODO: do clean up here, etc. Letter of the removed drive is in e.Drive;
            //On supprime la cle de la hashtable
            usbList.Remove(e.Drive);

            int index = 0;

            for (int i = 0; i < listeClesUsb.Items.Count; ++i)
            {
                if (listeClesUsb.Items[i].Tag.ToString() == e.Drive) index = i;
            }

            listeClesUsb.Items.RemoveAt(index);
        }

        // Called by DriveDetector when removable drive is about to be removed
        private void OnQueryRemove(object sender, DriveDetectorEventArgs e)
        {
        }

        private void checkboxToutUsbFormatage_CheckedChanged(object sender, EventArgs e)
        {
            Boolean selectionne;
            if (this.checkboxToutUsbFormatage.Checked)
            {
                selectionne = true;
            }
            else
            {
                selectionne = false;
            }

            foreach (ListViewItem i in this.listeClesUsbFormatage.Items)
            {
                i.Checked = selectionne;
            }
        }

        private void clickFormatage_Click(object sender, EventArgs e)
        {
            String format = this.comboBoxFormatUsb.SelectedItem.ToString();
            Usb temp;
            int compteur = 0;
            Boolean erreur = false, tempErreur = false;
            String messageErreur = "";
            foreach (ListViewItem i in this.listeClesUsbFormatage.Items)
            {
                if (i.Checked)
                {
                    temp = new Usb((Char)i.SubItems[0].Text.ToString()[0]);
                    tempErreur = temp.FormatDrive("test", format.ToUpper());
                    if (!tempErreur)
                    {
                        erreur = true;
                        if(messageErreur != "") {
                            messageErreur += " / ";
                        }
                        messageErreur += "Une erreur est survenu lors du formatage sur la clé " + temp.driveLetter;
                    }
                    i.SubItems[3].Text = format;
                    compteur++;
                }
            }

            if (erreur)
            {
                affichageMessageFormatage(0, messageErreur);
            }
            else if (compteur == 0)
            {
                affichageMessageFormatage(1, "Veuillez sélectionner un périphérique");
            }
            else
            {
                affichageMessageFormatage(2, "Formatage effectué avec succès");
            }
        }
        private void affichageMessageFormatage(int statut, String message)
        {
            //Erreur
            if(statut != 0) {
                this.formatageErreurImg.Location = new System.Drawing.Point(16, 263);
                this.formatageErreurMessage.Location = new System.Drawing.Point(44, 266);
                this.formatageErreurMessage.Visible = false;
                this.formatageErreurImg.Visible = false;
            }
            else {
                this.formatageErreurMessage.Visible = true;
                this.formatageErreurImg.Visible = true;
                this.formatageErreurMessage.Text = message;
            }
            //Avertissement
            if (statut == 1)
            {
                this.formatageAvertissementImg.Location = new System.Drawing.Point(16, 263);
                this.formatageAvertissementMessage.Location = new System.Drawing.Point(44, 266);
                this.formatageAvertissementMessage.Visible = true;
                this.formatageAvertissementImg.Visible = true;
                this.formatageAvertissementMessage.Text = message;
            }
            else
            {
                this.formatageAvertissementMessage.Visible = false;
                this.formatageAvertissementImg.Visible = false;
            }
            //Succes
            if (statut == 2)
            {
                this.formatageSuccesImg.Location = new System.Drawing.Point(16, 263);
                this.formatageSuccesMessage.Location = new System.Drawing.Point(44, 266);
                this.formatageSuccesMessage.Visible = true;
                this.formatageSuccesImg.Visible = true;
                this.formatageSuccesMessage.Text = message;
            }
            else
            {
                this.formatageSuccesMessage.Visible = false;
                this.formatageSuccesImg.Visible = false;
            }
        }

        private void checkboxToutUsbEjection_CheckedChanged(object sender, EventArgs e)
        {
            Boolean selectionne;
            if (this.checkboxToutUsbEjection.Checked)
            {
                selectionne = true;
            }
            else
            {
                selectionne = false;
            }

            foreach (ListViewItem i in this.listViewEjection.Items)
            {
                i.Checked = selectionne;
            }
        }

        private void clickEjection_Click(object sender, EventArgs e)
        {
            Usb temp;
            int compteur = 0;
            Boolean erreur = false, tempErreur = false;
            String messageErreur = "";
            foreach (ListViewItem i in this.listViewEjection.Items)
            {
                if (i.Checked)
                {
                    temp = new Usb((Char)i.SubItems[0].Text.ToString()[0]);
                    tempErreur = temp.EjectDrive();
                    if (!tempErreur)
                    {
                        erreur = true;
                        if (messageErreur != "")
                        {
                            messageErreur += " / ";
                        }
                        messageErreur += "Une erreur est survenu lors de l'éjection de la clé " + temp.driveLetter;
                    }
                    compteur++;
                }
            }
            if (erreur)
            {
                affichageMessageEjection(0, messageErreur);
            }
            else if (compteur == 0)
            {
                affichageMessageEjection(1, "Veuillez sélectionner un périphérique");
            }
            else
            {
                affichageMessageEjection(2, "Ejection effectué avec succès");
            }
        }

        private void affichageMessageEjection(int statut, String message)
        {
            //Erreur
            if (statut != 0)
            {
                this.ejectionErreurImg.Location = new System.Drawing.Point(15, 264);
                this.ejectionErreurMessage.Location = new System.Drawing.Point(43, 267);
                this.ejectionErreurMessage.Visible = false;
                this.ejectionErreurImg.Visible = false;
            }
            else
            {
                this.ejectionErreurMessage.Visible = true;
                this.ejectionErreurImg.Visible = true;
                this.ejectionErreurMessage.Text = message;
            }
            //Avertissement
            if (statut == 1)
            {
                this.ejectionAvertissementImg.Location = new System.Drawing.Point(15, 264);
                this.ejectionAvertissementMessage.Location = new System.Drawing.Point(43, 267);
                this.ejectionAvertissementMessage.Visible = true;
                this.ejectionAvertissementImg.Visible = true;
                this.ejectionAvertissementMessage.Text = message;
            }
            else
            {
                this.ejectionAvertissementMessage.Visible = false;
                this.ejectionAvertissementImg.Visible = false;
            }
            //Succes
            if (statut == 2)
            {
                this.ejectionSuccesImg.Location = new System.Drawing.Point(15, 264);
                this.ejectionSuccesMessage.Location = new System.Drawing.Point(43, 267);
                this.ejectionSuccesMessage.Visible = true;
                this.ejectionSuccesImg.Visible = true;
                this.ejectionSuccesMessage.Text = message;
            }
            else
            {
                this.ejectionSuccesMessage.Visible = false;
                this.ejectionSuccesImg.Visible = false;
            }
        }

        private bool isValidEmail(string email)
        {
            //Verifier l'adresse mail valide
            string strPattern = "^([0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
            
            return System.Text.RegularExpressions.Regex.IsMatch(email, strPattern);
        }
    }
}
