using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Duplicateur
{
    public partial class Chargement : Form
    {
        //private BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        //private Thread copyThread;
        private Hashtable threadTable = new Hashtable();

        private Hashtable groupList = new Hashtable();
        private List<Usb> usbList = new List<Usb>();

        private Hashtable allFiles = new Hashtable();
        private int clesTraitees = 0;

        private bool errorCopie = false;

        public delegate void changerProgresCles();
        public delegate void notifierCopieFichier(string filepath);
        public delegate void changerEtatBoutonsCopie(bool continuer,bool pause, bool arreter);
        public delegate void changerEtatBoutonsChoix(bool fusionner, bool ignorer, bool remplacer, bool fermer);
        public delegate void changerMessageCopie(int typeAlerte, string messageAlerte, string description);
        
        public Chargement(Hashtable groups,List<Usb> destList)
        {
            InitializeComponent();

            this.groupList = groups;
            this.usbList = destList;

            //Initialisation progress bar cles
            progressBarCles.Minimum = 0;
            progressBarCles.Maximum = usbList.Count;

            //Initialisation label copie cle
            labelCopieCle.Text = "Clés usb traitées ... ("
                + clesTraitees + "/" + usbList.Count + ")";

            //Initialisation boutons de copie
            btnContinueCopie.Enabled = false;
            btnPauseCopie.Enabled = true;
            btnArretCopie.Enabled = true;

            //Initialisation boutons d'options
            btnFusionerDossiers.Hide();
            btnRemplacerFichier.Hide();
            btnNePasCopier.Hide();
            btnFermerCopie.Hide();

            //Initialisation message notif
            pictureMessageCopieNotif.Hide();
            pictureMessageCopieOk.Hide();

            //Initialisation hashtable fichier
            getAllFiles();

            //Initialisation progress bar fichiers
            progressBarFichiers.Minimum = 0;
            progressBarFichiers.Maximum = allFiles.Count;


            //On lance la copie multithread
            for (int i = 0; i < usbList.Count; ++i)
            {
                Usb usb = usbList[i];
                string usbName = usb.driveLetter + ":";
                Thread thread = new Thread(processSelectionCopy);
                threadTable.Add(usbName, thread);
                thread.Start(usb);
            }
        }

        private int getCopyListCount()
        {
            int copyListCount = 0;

            foreach (String group in groupList.Keys)
            {
                List<String> List = (List<String>)groupList[group];
                copyListCount += List.Count;
            }

            return copyListCount;
        }

        public void nCopieFichier(string filepath)
        {
            int nbCopie = (int)allFiles[filepath];
            ++nbCopie;

            if (nbCopie == usbList.Count)
            {
                this.progressBarFichiers.Increment(1);
            }

            //allFiles.Add(filepath, nbCopie);
            allFiles[filepath] = nbCopie;
        }

        public void cProgresCles()
        {
            this.progressBarCles.Increment(1);
            ++clesTraitees;
            this.labelCopieCle.Text = "Clés usb traitées ... (" + clesTraitees + "/" + usbList.Count + ")";

            if (clesTraitees == usbList.Count)
            {
                if (!errorCopie)
                {
                    Invoke((changerMessageCopie)cMessageCopie, 0, "Copie terminée avec succès", "");
                }
                Invoke((changerEtatBoutonsCopie)cEtatBoutonsCopie, false, false, false);
                Invoke((changerEtatBoutonsChoix)cEtatBoutonChoix, false, false, false, true);
            }
        }

        public void cEtatBoutonsCopie(bool continuer, bool pause, bool arreter)
        {
            this.btnContinueCopie.Enabled = continuer;
            this.btnPauseCopie.Enabled = pause;
            this.btnArretCopie.Enabled = arreter;
        }

        public void cEtatBoutonChoix(bool fusionner, bool ignorer, bool remplacer, bool fermer)
        {
            if (fusionner) this.btnFusionerDossiers.Show();
            if (ignorer) this.btnNePasCopier.Show();
            if (remplacer) this.btnRemplacerFichier.Show();
            if (fermer) this.btnFermerCopie.Show();
        }

        public void cMessageCopie(int typeAlerte, string messageAlerte, string description)
        {
            switch (typeAlerte)
            {
                case 0 :
                    pictureMessageCopieOk.Show();
                    pictureMessageCopieNotif.Hide();
                    labelMessageCopie.Text = messageAlerte;
                    textMessageCopie.Text = description;
                    break;
                case 1:
                    pictureMessageCopieNotif.Show();
                    pictureMessageCopieOk.Hide();
                    labelMessageCopie.Text = "Erreur de copie fichier";
                    textMessageCopie.Text += description + "\n";
                    break;
                default:
                    labelMessageCopie.Text = "";
                    pictureMessageCopieOk.Hide();
                    pictureMessageCopieNotif.Hide();
                    textMessageCopie.Text = description;
                    break;
            }
        }

        /*Fonction qui permet de copier toute la liste de fichier et dossier selectionnés
         * */
        //public void processSelectionCopy(BackgroundWorker worker, DoWorkEventArgs e)
        public void processSelectionCopy(object obj)
        {
            Usb usb = (Usb)obj;

            String destPath = "";

            //On parcoure tous les groupes de dossier
            foreach (string group in groupList.Keys)
            {
                //Si copier à la racine
                if (usb.copyToRoot)
                {
                    destPath = usb.driveLetter + ":\\";
                }
                else
                {
                    destPath = usb.getDestinationPath();
                }

                //Si dossier parent à créer
                if (usb.getFolderToCreate().Length > 0)
                {
                    destPath = Path.Combine(destPath, usb.getFolderToCreate());
                }

                //On cree un dossier a la destination avec le nom du groupe
                //String destDirName = usb.driveLetter + ":\\" + group;
                //Directory.CreateDirectory(destDirName);
                destPath = Path.Combine(destPath, group);

                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }


                //On recupere l'ensemble des fichier et dossiers du groupe
                List<String> pathList = (List<String>)groupList[group];

                foreach (String path in pathList)
                {
                    //Si le path est un repertoire
                    if (Directory.Exists(path))
                    {
                        //On construit le chemin du dossier de destination
                        DirectoryInfo di = new DirectoryInfo(path);
                        //On appelle la fonction de copie récursive
                        String destDir = Path.Combine(destPath, di.Name);

                        String message = "Copie de " + path + " vers " + destDir;
                        //Invoke((changerLabelCopie)cLabelCopie, message);
                        copyDirectory(path, destDir, true);
                    }
                    else // C'est un fichier
                    {
                        //On copie le fichier
                        FileInfo fi = new FileInfo(path);
                        String destFile = Path.Combine(destPath, fi.Name);

                        //String message = "Copie de " + path + " vers " + destFile;
                        //Invoke((changerLabelCopie)cLabelCopie, message);
                        try
                        {

                            fi.CopyTo(destFile, true);
                        }
                        catch (Exception e)
                        {
                            //string labeErr = "Copie impossible du fichier " + fi.Name;
                            Invoke((changerMessageCopie)cMessageCopie, 1, "", e.Message);
                            errorCopie = true;
                        }
                        finally
                        {
                            Invoke((notifierCopieFichier)nCopieFichier, path);
                        }
                    }
                }
            }

            //Copie des fichier finie pour une des clés usb (envoyer mail)
            Invoke((changerProgresCles)cProgresCles);
            envoyerMail();
        }

        /*
        public void processSelectionCopy()
        {
            //On parcours la liste des usb destinations
            for (int i = 0; i < usbList.Count; ++i)
            {
                Usb usb = usbList[i];

                //On modifie le label de la progress bar des clés
                string text = "Copie en cours sur le clé " + usb.driveLetter;
                Invoke((changerLabelCles)cLabelCles, text);

                //Compteur de boucle sur fichiers et dossiers
                int fCompteur = 0;

                //On parcoure tous les groupes de dossier
                foreach (string group in groupList.Keys)
                {
                    //On cree un dossier a la destination avec le nom du groupe
                    String destDirName = usb.driveLetter + ":\\" + group;
                    Directory.CreateDirectory(destDirName);

                    //On recupere l'ensemble des fichier et dossiers du groupe
                    List<String> pathList = (List<String>)groupList[group];

                    foreach (String path in pathList)
                    {
                        //Si le path est un repertoire
                        if (Directory.Exists(path))
                        {
                            //On construit le chemin du dossier de destination
                            DirectoryInfo di = new DirectoryInfo(path);
                            //On appelle la fonction de copie récursive
                            String destDir = Path.Combine(destDirName, di.Name);

                            String message = "Copie de " + path + " vers " + destDir;
                            Invoke((changerLabelCopie)cLabelCopie, message);
                            copyDirectory(path, destDir, true);
                        }
                        else // C'est un fichier
                        {
                            //On copie le fichier
                            FileInfo fi = new FileInfo(path);
                            String destFile = Path.Combine(destDirName, fi.Name);

                            String message = "Copie de " + path + " vers " + destFile;
                            Invoke((changerLabelCopie)cLabelCopie, message);
                            fi.CopyTo(destFile, true);
                        }

                        ++fCompteur;
                        Invoke((changerProgresCopie)cProgresCopie, fCompteur);
                    }
                }

                Invoke((changerProgresCles)cProgresCles, i + 1);
            }

            //Copie des fichier fini (notification pour dire que la copie est terminé)
            Invoke((changerMessageCopie)cMessageCopie, 0, "Copie terminée avec succès", "");
            Invoke((changerEtatBoutonsCopie)cEtatBoutonsCopie, false, false, false);
            Invoke((changerEtatBoutonsChoix)cEtatBoutonChoix, false, false, false, true);
            envoyerMail();
        }
         * 
         * */

        /*Fonction qui permet de copier recursivement des dossiers 
         * */
        private void copyDirectory(string sourceDirName, string destDirName, bool copySubDirs)
        {

            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory doesn't exist, create it. 
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                try
                {
                    string temppath = Path.Combine(destDirName, file.Name);                
                    file.CopyTo(temppath, true);
                    Invoke((notifierCopieFichier)nCopieFichier, file.FullName);
                }
                catch (Exception e)
                {
                    //Do nothing
                    Invoke((changerMessageCopie)cMessageCopie, 1, "", e.Message);
                }
            }

            // If copying subdirectories, copy them and their contents to new location. 
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    copyDirectory(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        private void btnPauseCopie_Click(object sender, EventArgs e)
        {
            //copyThread.Suspend();
            foreach (String key in threadTable.Keys)
            {
                Thread thread = (Thread)threadTable[key];
                thread.Suspend();
            }

            btnContinueCopie.Enabled = true;
            btnPauseCopie.Enabled = false;
        }

        private void btnContinueCopie_Click(object sender, EventArgs e)
        {
            //copyThread.Resume();
            foreach (String key in threadTable.Keys)
            {
                Thread thread = (Thread)threadTable[key];
                thread.Resume();
            }
            btnContinueCopie.Enabled = false;
            btnPauseCopie.Enabled = true;
        }

        private void btnArretCopie_Click(object sender, EventArgs e)
        {
            btnPauseCopie.Enabled = false;

            foreach (String key in threadTable.Keys)
            {
                Thread thread = (Thread)threadTable[key];
                thread.Abort();

                while (thread.IsAlive)
                {
                    //Do nothing and wait for the thread to stop
                }
            }
        }

        private void btnFermerCopie_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void envoyerMail()
        {
            foreach (Usb usb in usbList)
            {
                if (usb.sendNotification)
                {
                    string email = usb.getNotifMailAddress();
                    MailMessage mail = new MailMessage("duplicateurmgr@gmail.com", email, "Duplicateur - Copie terminée", "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional //EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>Mailing Duplicator</title>	<!--<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">	-->	<meta http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><style type=\"text/css\">/* Fonts and Content */body, td { font-family: 'Helvetica Neue', Arial, Helvetica, Geneva, sans-serif; font-size:14px; }body { background-color: #ffffff; margin: 0; padding: 0; -webkit-text-size-adjust:none; -ms-text-size-adjust:none; }h2{ padding-top:12px; /* ne fonctionnera pas sous Outlook 2007+ */color:#0E7693; font-size:44px; }</style></head><body style=\"margin:0px; padding:0px; -webkit-text-size-adjust:none;\"><table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"background-color:rgb(42, 55, 78)\" ><tbody><tr><td align=\"center\" bgcolor=\"#4FC1E9\"><table  cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tbody><tr><td class=\"w640\"  width=\"640\" height=\"10\"></td><td class=\"w640\"  width=\"640\" height=\"10\"></td></tr><!-- entete --><tr class=\"pagetoplogo\"><td class=\"w640\"  width=\"640\"><table  class=\"w640\"  width=\"640\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" bgcolor=\"#F2F0F0\"><tbody><tr><td class=\"w30\"  width=\"30\"></td><td  class=\"w580\"  width=\"580\" valign=\"middle\" align=\"left\"><div class=\"pagetoplogo-content\"><!--<img class=\"w580\" style=\"text-decoration: none; display: block; color:#476688; font-size:30px;\" src=\"header_2.png\" alt=\"Duplicator 1.0\" width=\"482\" height=\"108\"/>--><img width=\"70\" height=\"70\" src=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEYAAABGCAYAAABxLuKEAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6NDlGQTgyNUJFMDFBMTFFMzkxNUZDNkE0RTEzMTAxOEIiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6NDlGQTgyNUNFMDFBMTFFMzkxNUZDNkE0RTEzMTAxOEIiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDo0OUZBODI1OUUwMUExMUUzOTE1RkM2QTRFMTMxMDE4QiIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDo0OUZBODI1QUUwMUExMUUzOTE1RkM2QTRFMTMxMDE4QiIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/Ppq9rsQAAAaESURBVHja7JxJbBtl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\" /></div></td></tr><td class=\"w30\"  width=\"30\"></td></tbody></table></td></tr><!-- contenu --><tr class=\"content\"><td class=\"w640\" class=\"w640\"  width=\"640\" bgcolor=\"#ffffff\"><table class=\"w640\"  width=\"640\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tbody><tr><td  class=\"w30\"  width=\"30\"></td><td  class=\"w580\"  width=\"580\"><!-- une zone de contenu --><table class=\"w580\"  width=\"580\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\"><tbody><tr><td class=\"w580\"  width=\"580\"><h2 style=\"color:#476688; font-size:22px; padding-top:12px;\">Votre copie est terminee !  </h2><div align=\"left\" class=\"article-content\"><p>La copie des fichiers s'est terminee avec succes.</a></p><p>Vous pouvez maintenant utiliser votre fichier sur le support de destination.</p></div></td></tr><tr><td class=\"w580\"  width=\"580\" height=\"1\" bgcolor=\"#c7c5c5\"></td></tr></tbody></table><!-- fin zone --></table></table></td></tr></tbody></table></body></html>");
                    mail.IsBodyHtml = true;
                    SmtpClient client = new SmtpClient("smtp.gmail.com");
                    client.Port = 587;
                    client.Credentials = new System.Net.NetworkCredential("duplicateurmgr@gmail.com", "duplicateurg4");
                    client.EnableSsl = true;
                    client.Send(mail);
                    //MessageBox.Show("Votre message a bien été envoyé.", "Message envoyé", MessageBoxButtons.OK);
                }
            }
        }


        /*Fonction qui permet de calculer recursivement la taille d'un dossier
         * */
        private void getFolderChildren(String path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);

                try
                {
                    FileInfo[] files = dir.GetFiles();

                    foreach (FileInfo file in files)
                    {
                        allFiles.Add(file.FullName,0);
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
                        getFolderChildren(directory.FullName);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /* Fonction qui permet de calculer la taille de l'ensemble des fichiers à copier
         * */
        private void getAllFiles()
        {
            foreach (string group in groupList.Keys)
            {
                //On recupere l'ensemble des fichier et dossiers du groupe
                List<String> pathList = (List<String>)groupList[group];

                foreach (String path in pathList)
                {
                    if (Directory.Exists(path))
                    {
                        getFolderChildren(path);
                    }
                    else
                    {
                        allFiles.Add(path,0);
                    }
                }
            }
        }
    }
}
