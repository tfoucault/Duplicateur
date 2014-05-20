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

        public MainWindow()
        {
            InitializeComponent();
            initListSelection();
            initListClesUsb();
        }

      
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


        private void initListClesUsb()
        {
            // Set the view to show details.
            listViewClesUsb.View = View.Details;
            // Allow the user to rearrange columns.
            listViewClesUsb.AllowColumnReorder = true;
            // Display check boxes.
            listViewClesUsb.CheckBoxes = true;
            // Select the item and subitems when selection is made.
            listViewClesUsb.FullRowSelect = true;
            // Display grid lines.
            //listViewClesUsb.GridLines = true;
            // Sort the items in the list in ascending order.
            listViewClesUsb.Sorting = SortOrder.Ascending;

            //Ajout des headers dans la liste view des cles usb
            listViewClesUsb.Columns.Add("Racine", -2, HorizontalAlignment.Left);
            listViewClesUsb.Columns.Add("Espace total", -2, HorizontalAlignment.Left);
            listViewClesUsb.Columns.Add("Espace libre", -2, HorizontalAlignment.Left);
            listViewClesUsb.Columns.Add("Format", -2, HorizontalAlignment.Center);

            List<DriveInfo> usbList = fileMgr.getUsbList();

            foreach (DriveInfo di in usbList)
            {
                //On crée un objet de type usb
                string usbName = di.Name.Substring(0, 1);
                char usbLetter = usbName[0];

                Usb usb = new Usb(usbLetter);
                this.usbList.Add(di.Name,usb);
                
                ListViewItem item = new ListViewItem();
                item.Tag = di.Name;
                item.Text = di.Name;
                item.SubItems.Add(usb.getTotalSizeStr());
                item.SubItems.Add(usb.getFreeSpaceStr());
                item.SubItems.Add(usb.getFormat());

                listViewClesUsb.Items.Add(item);
            }
        }


        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

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
                foreach(String file in selectedFiles){

                    //On crée un objet d'informations sur le fichier
                    FileInfo fi = new FileInfo(file);

                    //On ajoute le fichier si  : 
                    //Son parent n'est pas dans la liste
                    //Le fichier n'est pas dans la liste

                    if(!estDansListeSelection(fi.DirectoryName) 
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


        private bool estDansListeSelection(String path)
        {
            foreach (ListViewItem item in listeSelection.Items)
            {
                if ((string)item.Tag == path) return true;
            }

            return false;
        }

        private void btnSupprSelection_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem item in listeSelection.SelectedItems){

                listeSelection.Items.Remove(item);
            }
        }

        private void checkBoxToutesCles_CheckedChanged(object sender, EventArgs e)
        {
            //Si l'etat pas de non coché à coché
            if (checkBoxToutesCles.CheckState == CheckState.Checked)
            {
                //On coche tous les items de la listview cles usb
                foreach(ListViewItem item in listViewClesUsb.Items){
                    item.Checked = true;
                }
            }
            else
            {
                //On décoche tous les items de la listeview cles usb
                foreach (ListViewItem item in listViewClesUsb.Items)
                {
                    item.Checked = false;
                }
            }
        }

        private void checkBoxDossier_CheckedChanged(object sender, EventArgs e)
        {
            //Si l'état pas de coché à non coché
            if (checkBoxDossier.CheckState == CheckState.Checked)
            {
                //On active le champs de saisie
                textBoxNomDossier.Enabled = true;
                currentUsb.createFolder = true;
            }
            else
            {
                //On desactive le champs de saisie
                textBoxNomDossier.Enabled = false;
                currentUsb.createFolder = false;
            }
        }

        private void radioButtonChooseFolder_CheckedChanged(object sender, EventArgs e)
        {
            //Si le bouton passe à l'état coché
            if (radioButtonChooseFolder.Checked)
            {
                //On active le champs de saisie
                textBoxChooseFolder.Enabled = true;
                //On active le bouton parcourir
                btnChooseFolder.Enabled = true;
                currentUsb.copyToRoot = false;
            }
            else
            {
                //On desactive le champs de saisie
                textBoxChooseFolder.Enabled = false;
                btnChooseFolder.Enabled = false;
                currentUsb.copyToRoot = true;
            }
        }

        private void btnChooseFolder_Click(object sender, EventArgs e)
        {
            //On ouvre un dialog pour le choix d'un dossier de desstination
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                //Une fois la séléction validée, on ajoute le chemin absolu au champ de saisie
                textBoxChooseFolder.Text = fbd.SelectedPath;
                //On ajoute le chemin comme fichier de destination pour le usb courant
                currentUsb.setDestinationPath(fbd.SelectedPath);
            }
        }

        private void listViewClesUsb_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

            if (e.ItemIndex != previousClesUsbIndex)
            {
                previousClesUsbIndex = e.ItemIndex;
                string key = e.Item.Tag.ToString();

                //Récupération de l'usb selectionné dans la liste des usb
                currentUsb = (Usb)usbList[key];

                //MessageBox.Show(usb.getFolderToCreate());
                //On met à jour les champs concernés
                if (currentUsb.copyToRoot)
                {
                    radioButtonRacine.Checked = true;
                    radioButtonChooseFolder.Checked = false;
                }
                else
                {
                    radioButtonRacine.Checked = false;
                    radioButtonChooseFolder.Checked = true;
                    textBoxChooseFolder.Text = currentUsb.getDestinationPath();
                }

                //On met a jour le dossier à créer si existe
                checkBoxDossier.Checked = currentUsb.createFolder;
                textBoxNomDossier.Text = currentUsb.getFolderToCreate();
            }
        }

        private void textBoxNomDossier_TextChanged(object sender, EventArgs e)
        {
            //On sauvegarde le nom de dossier a creer
            currentUsb.setFolderToCreate(textBoxNomDossier.Text);
        }
    }
}
