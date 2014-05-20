namespace Duplicateur
{
    partial class MainWindow
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnEjecterCles = new System.Windows.Forms.Button();
            this.btnFormaterCles = new System.Windows.Forms.Button();
            this.labelSelectionFichiers = new System.Windows.Forms.Label();
            this.labelSelectionCible = new System.Windows.Forms.Label();
            this.btnSelectionFichiers = new System.Windows.Forms.Button();
            this.btnSelectionDossier = new System.Windows.Forms.Button();
            this.labelSupportCible = new System.Windows.Forms.Label();
            this.checkBoxToutesCles = new System.Windows.Forms.CheckBox();
            this.labelCreerDossier = new System.Windows.Forms.Label();
            this.labelNotification = new System.Windows.Forms.Label();
            this.radioButtonNotifNon = new System.Windows.Forms.RadioButton();
            this.radioButtonNotifOui = new System.Windows.Forms.RadioButton();
            this.textBoxTel = new System.Windows.Forms.TextBox();
            this.textBoxMail = new System.Windows.Forms.TextBox();
            this.labelTel = new System.Windows.Forms.Label();
            this.labelMail = new System.Windows.Forms.Label();
            this.buttonLancerCopie = new System.Windows.Forms.Button();
            this.labelAlerte = new System.Windows.Forms.Label();
            this.btnAlertOui = new System.Windows.Forms.Button();
            this.btnAlertNon = new System.Windows.Forms.Button();
            this.radioButtonRacine = new System.Windows.Forms.RadioButton();
            this.radioButtonDossier = new System.Windows.Forms.RadioButton();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.labelSelectionDossier = new System.Windows.Forms.Label();
            this.listeSelection = new System.Windows.Forms.ListView();
            this.btnSupprSelection = new System.Windows.Forms.Button();
            this.listViewClesUsb = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // btnEjecterCles
            // 
            this.btnEjecterCles.Location = new System.Drawing.Point(12, 12);
            this.btnEjecterCles.Name = "btnEjecterCles";
            this.btnEjecterCles.Size = new System.Drawing.Size(103, 23);
            this.btnEjecterCles.TabIndex = 0;
            this.btnEjecterCles.Text = "Ejecter les clés";
            this.btnEjecterCles.UseVisualStyleBackColor = true;
            // 
            // btnFormaterCles
            // 
            this.btnFormaterCles.Location = new System.Drawing.Point(120, 12);
            this.btnFormaterCles.Name = "btnFormaterCles";
            this.btnFormaterCles.Size = new System.Drawing.Size(103, 23);
            this.btnFormaterCles.TabIndex = 1;
            this.btnFormaterCles.Text = "Formater les clés";
            this.btnFormaterCles.UseVisualStyleBackColor = true;
            // 
            // labelSelectionFichiers
            // 
            this.labelSelectionFichiers.AutoSize = true;
            this.labelSelectionFichiers.Location = new System.Drawing.Point(12, 64);
            this.labelSelectionFichiers.Name = "labelSelectionFichiers";
            this.labelSelectionFichiers.Size = new System.Drawing.Size(114, 13);
            this.labelSelectionFichiers.TabIndex = 2;
            this.labelSelectionFichiers.Text = "1) Sélection de fichiers";
            this.labelSelectionFichiers.Click += new System.EventHandler(this.label1_Click);
            // 
            // labelSelectionCible
            // 
            this.labelSelectionCible.AutoSize = true;
            this.labelSelectionCible.Location = new System.Drawing.Point(406, 55);
            this.labelSelectionCible.Name = "labelSelectionCible";
            this.labelSelectionCible.Size = new System.Drawing.Size(195, 13);
            this.labelSelectionCible.TabIndex = 3;
            this.labelSelectionCible.Text = "3) Séléction des supports de destination";
            // 
            // btnSelectionFichiers
            // 
            this.btnSelectionFichiers.Location = new System.Drawing.Point(229, 59);
            this.btnSelectionFichiers.Name = "btnSelectionFichiers";
            this.btnSelectionFichiers.Size = new System.Drawing.Size(75, 23);
            this.btnSelectionFichiers.TabIndex = 4;
            this.btnSelectionFichiers.Text = "Parcourir";
            this.btnSelectionFichiers.UseVisualStyleBackColor = true;
            this.btnSelectionFichiers.Click += new System.EventHandler(this.btnSelectionFichiers_Click);
            // 
            // btnSelectionDossier
            // 
            this.btnSelectionDossier.Location = new System.Drawing.Point(229, 88);
            this.btnSelectionDossier.Name = "btnSelectionDossier";
            this.btnSelectionDossier.Size = new System.Drawing.Size(75, 23);
            this.btnSelectionDossier.TabIndex = 6;
            this.btnSelectionDossier.Text = "Parcourir";
            this.btnSelectionDossier.UseVisualStyleBackColor = true;
            this.btnSelectionDossier.Click += new System.EventHandler(this.btnSelectionDossier_Click);
            // 
            // labelSupportCible
            // 
            this.labelSupportCible.AutoSize = true;
            this.labelSupportCible.Location = new System.Drawing.Point(406, 85);
            this.labelSupportCible.Name = "labelSupportCible";
            this.labelSupportCible.Size = new System.Drawing.Size(113, 13);
            this.labelSupportCible.TabIndex = 8;
            this.labelSupportCible.Text = "Support de destination";
            // 
            // checkBoxToutesCles
            // 
            this.checkBoxToutesCles.AutoSize = true;
            this.checkBoxToutesCles.Location = new System.Drawing.Point(409, 231);
            this.checkBoxToutesCles.Name = "checkBoxToutesCles";
            this.checkBoxToutesCles.Size = new System.Drawing.Size(190, 17);
            this.checkBoxToutesCles.TabIndex = 10;
            this.checkBoxToutesCles.Text = "Sélectionner tous les périphériques";
            this.checkBoxToutesCles.UseVisualStyleBackColor = true;
            // 
            // labelCreerDossier
            // 
            this.labelCreerDossier.AutoSize = true;
            this.labelCreerDossier.Location = new System.Drawing.Point(406, 276);
            this.labelCreerDossier.Name = "labelCreerDossier";
            this.labelCreerDossier.Size = new System.Drawing.Size(97, 13);
            this.labelCreerDossier.TabIndex = 11;
            this.labelCreerDossier.Text = "Création du dossier";
            // 
            // labelNotification
            // 
            this.labelNotification.AutoSize = true;
            this.labelNotification.Location = new System.Drawing.Point(406, 423);
            this.labelNotification.Name = "labelNotification";
            this.labelNotification.Size = new System.Drawing.Size(73, 13);
            this.labelNotification.TabIndex = 15;
            this.labelNotification.Text = "Avertissement";
            // 
            // radioButtonNotifNon
            // 
            this.radioButtonNotifNon.AutoSize = true;
            this.radioButtonNotifNon.Location = new System.Drawing.Point(409, 439);
            this.radioButtonNotifNon.Name = "radioButtonNotifNon";
            this.radioButtonNotifNon.Size = new System.Drawing.Size(45, 17);
            this.radioButtonNotifNon.TabIndex = 16;
            this.radioButtonNotifNon.TabStop = true;
            this.radioButtonNotifNon.Text = "Non";
            this.radioButtonNotifNon.UseVisualStyleBackColor = true;
            // 
            // radioButtonNotifOui
            // 
            this.radioButtonNotifOui.AutoSize = true;
            this.radioButtonNotifOui.Location = new System.Drawing.Point(474, 439);
            this.radioButtonNotifOui.Name = "radioButtonNotifOui";
            this.radioButtonNotifOui.Size = new System.Drawing.Size(41, 17);
            this.radioButtonNotifOui.TabIndex = 17;
            this.radioButtonNotifOui.TabStop = true;
            this.radioButtonNotifOui.Text = "Oui";
            this.radioButtonNotifOui.UseVisualStyleBackColor = true;
            // 
            // textBoxTel
            // 
            this.textBoxTel.Enabled = false;
            this.textBoxTel.Location = new System.Drawing.Point(409, 478);
            this.textBoxTel.Name = "textBoxTel";
            this.textBoxTel.Size = new System.Drawing.Size(141, 20);
            this.textBoxTel.TabIndex = 18;
            // 
            // textBoxMail
            // 
            this.textBoxMail.Enabled = false;
            this.textBoxMail.Location = new System.Drawing.Point(558, 478);
            this.textBoxMail.Name = "textBoxMail";
            this.textBoxMail.Size = new System.Drawing.Size(141, 20);
            this.textBoxMail.TabIndex = 19;
            // 
            // labelTel
            // 
            this.labelTel.AutoSize = true;
            this.labelTel.Enabled = false;
            this.labelTel.Location = new System.Drawing.Point(406, 462);
            this.labelTel.Name = "labelTel";
            this.labelTel.Size = new System.Drawing.Size(58, 13);
            this.labelTel.TabIndex = 20;
            this.labelTel.Text = "Téléphone";
            // 
            // labelMail
            // 
            this.labelMail.AutoSize = true;
            this.labelMail.Enabled = false;
            this.labelMail.Location = new System.Drawing.Point(555, 462);
            this.labelMail.Name = "labelMail";
            this.labelMail.Size = new System.Drawing.Size(26, 13);
            this.labelMail.TabIndex = 21;
            this.labelMail.Text = "Mail";
            // 
            // buttonLancerCopie
            // 
            this.buttonLancerCopie.Location = new System.Drawing.Point(409, 511);
            this.buttonLancerCopie.Name = "buttonLancerCopie";
            this.buttonLancerCopie.Size = new System.Drawing.Size(290, 42);
            this.buttonLancerCopie.TabIndex = 22;
            this.buttonLancerCopie.Text = "Lancer la copie";
            this.buttonLancerCopie.UseVisualStyleBackColor = true;
            // 
            // labelAlerte
            // 
            this.labelAlerte.AutoSize = true;
            this.labelAlerte.Location = new System.Drawing.Point(12, 478);
            this.labelAlerte.Name = "labelAlerte";
            this.labelAlerte.Size = new System.Drawing.Size(87, 13);
            this.labelAlerte.TabIndex = 23;
            this.labelAlerte.Text = "Message d\'alerte";
            // 
            // btnAlertOui
            // 
            this.btnAlertOui.Location = new System.Drawing.Point(148, 530);
            this.btnAlertOui.Name = "btnAlertOui";
            this.btnAlertOui.Size = new System.Drawing.Size(75, 23);
            this.btnAlertOui.TabIndex = 24;
            this.btnAlertOui.Text = "Oui";
            this.btnAlertOui.UseVisualStyleBackColor = true;
            // 
            // btnAlertNon
            // 
            this.btnAlertNon.Location = new System.Drawing.Point(229, 530);
            this.btnAlertNon.Name = "btnAlertNon";
            this.btnAlertNon.Size = new System.Drawing.Size(75, 23);
            this.btnAlertNon.TabIndex = 25;
            this.btnAlertNon.Text = "Non";
            this.btnAlertNon.UseVisualStyleBackColor = true;
            // 
            // radioButtonRacine
            // 
            this.radioButtonRacine.AutoSize = true;
            this.radioButtonRacine.Location = new System.Drawing.Point(409, 296);
            this.radioButtonRacine.Name = "radioButtonRacine";
            this.radioButtonRacine.Size = new System.Drawing.Size(107, 17);
            this.radioButtonRacine.TabIndex = 28;
            this.radioButtonRacine.TabStop = true;
            this.radioButtonRacine.Text = "Copier à la racine";
            this.radioButtonRacine.UseVisualStyleBackColor = true;
            // 
            // radioButtonDossier
            // 
            this.radioButtonDossier.AutoSize = true;
            this.radioButtonDossier.Location = new System.Drawing.Point(409, 319);
            this.radioButtonDossier.Name = "radioButtonDossier";
            this.radioButtonDossier.Size = new System.Drawing.Size(145, 17);
            this.radioButtonDossier.TabIndex = 29;
            this.radioButtonDossier.TabStop = true;
            this.radioButtonDossier.Text = "Créer un dossier et copier";
            this.radioButtonDossier.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(429, 342);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(270, 20);
            this.textBox3.TabIndex = 30;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(409, 368);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(119, 17);
            this.radioButton5.TabIndex = 31;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "Chercher un dossier";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(429, 391);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(189, 20);
            this.textBox2.TabIndex = 33;
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(624, 390);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 32;
            this.button5.Text = "Parcourir";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // labelSelectionDossier
            // 
            this.labelSelectionDossier.AutoSize = true;
            this.labelSelectionDossier.Location = new System.Drawing.Point(12, 94);
            this.labelSelectionDossier.Name = "labelSelectionDossier";
            this.labelSelectionDossier.Size = new System.Drawing.Size(114, 13);
            this.labelSelectionDossier.TabIndex = 2;
            this.labelSelectionDossier.Text = "2) Sélection de dossier";
            this.labelSelectionDossier.Click += new System.EventHandler(this.label1_Click);
            // 
            // listeSelection
            // 
            this.listeSelection.Location = new System.Drawing.Point(15, 128);
            this.listeSelection.Name = "listeSelection";
            this.listeSelection.Size = new System.Drawing.Size(289, 283);
            this.listeSelection.TabIndex = 34;
            this.listeSelection.UseCompatibleStateImageBehavior = false;
            this.listeSelection.SelectedIndexChanged += new System.EventHandler(this.listeSelection_SelectedIndexChanged);
            // 
            // btnSupprSelection
            // 
            this.btnSupprSelection.Location = new System.Drawing.Point(15, 417);
            this.btnSupprSelection.Name = "btnSupprSelection";
            this.btnSupprSelection.Size = new System.Drawing.Size(289, 23);
            this.btnSupprSelection.TabIndex = 35;
            this.btnSupprSelection.Text = "Supprimer selection";
            this.btnSupprSelection.UseVisualStyleBackColor = true;
            this.btnSupprSelection.Click += new System.EventHandler(this.btnSupprSelection_Click);
            // 
            // listViewClesUsb
            // 
            this.listViewClesUsb.Location = new System.Drawing.Point(409, 110);
            this.listViewClesUsb.Name = "listViewClesUsb";
            this.listViewClesUsb.Size = new System.Drawing.Size(290, 115);
            this.listViewClesUsb.TabIndex = 36;
            this.listViewClesUsb.UseCompatibleStateImageBehavior = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 560);
            this.Controls.Add(this.listViewClesUsb);
            this.Controls.Add(this.btnSupprSelection);
            this.Controls.Add(this.listeSelection);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.radioButton5);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.radioButtonDossier);
            this.Controls.Add(this.radioButtonRacine);
            this.Controls.Add(this.btnAlertNon);
            this.Controls.Add(this.btnAlertOui);
            this.Controls.Add(this.labelAlerte);
            this.Controls.Add(this.buttonLancerCopie);
            this.Controls.Add(this.labelMail);
            this.Controls.Add(this.labelTel);
            this.Controls.Add(this.textBoxMail);
            this.Controls.Add(this.textBoxTel);
            this.Controls.Add(this.radioButtonNotifOui);
            this.Controls.Add(this.radioButtonNotifNon);
            this.Controls.Add(this.labelNotification);
            this.Controls.Add(this.labelCreerDossier);
            this.Controls.Add(this.checkBoxToutesCles);
            this.Controls.Add(this.labelSupportCible);
            this.Controls.Add(this.btnSelectionDossier);
            this.Controls.Add(this.btnSelectionFichiers);
            this.Controls.Add(this.labelSelectionCible);
            this.Controls.Add(this.labelSelectionDossier);
            this.Controls.Add(this.labelSelectionFichiers);
            this.Controls.Add(this.btnFormaterCles);
            this.Controls.Add(this.btnEjecterCles);
            this.Name = "MainWindow";
            this.Text = "Duplicator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEjecterCles;
        private System.Windows.Forms.Button btnFormaterCles;
        private System.Windows.Forms.Label labelSelectionFichiers;
        private System.Windows.Forms.Label labelSelectionCible;
        private System.Windows.Forms.Button btnSelectionFichiers;
        private System.Windows.Forms.Button btnSelectionDossier;
        private System.Windows.Forms.Label labelSupportCible;
        private System.Windows.Forms.CheckBox checkBoxToutesCles;
        private System.Windows.Forms.Label labelCreerDossier;
        private System.Windows.Forms.Label labelNotification;
        private System.Windows.Forms.RadioButton radioButtonNotifNon;
        private System.Windows.Forms.RadioButton radioButtonNotifOui;
        private System.Windows.Forms.TextBox textBoxTel;
        private System.Windows.Forms.TextBox textBoxMail;
        private System.Windows.Forms.Label labelTel;
        private System.Windows.Forms.Label labelMail;
        private System.Windows.Forms.Button buttonLancerCopie;
        private System.Windows.Forms.Label labelAlerte;
        private System.Windows.Forms.Button btnAlertOui;
        private System.Windows.Forms.Button btnAlertNon;
        private System.Windows.Forms.RadioButton radioButtonRacine;
        private System.Windows.Forms.RadioButton radioButtonDossier;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label labelSelectionDossier;
        private System.Windows.Forms.ListView listeSelection;
        private System.Windows.Forms.Button btnSupprSelection;
        private System.Windows.Forms.ListView listViewClesUsb;
    }
}

