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

        public MainWindow()
        {
            InitializeComponent();
            initListSelection();
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

        private void listeSelection_SelectedIndexChanged(object sender, EventArgs e)
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


        private bool estDansListeSelection(String path)
        {
            foreach (ListViewItem item in listeSelection.Items)
            {
                if ((string)item.Tag == path) return true;
            }

            return false;
        }
    }
}
