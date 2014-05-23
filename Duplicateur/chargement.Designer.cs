namespace Duplicateur
{
    partial class Chargement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Chargement));
            this.pictureBoxTitreChargement = new System.Windows.Forms.PictureBox();
            this.labelTitreChargement = new System.Windows.Forms.Label();
            this.progressBarCles = new System.Windows.Forms.ProgressBar();
            this.progressBarFichiers = new System.Windows.Forms.ProgressBar();
            this.labelCopieCle = new System.Windows.Forms.Label();
            this.labelCopieFichier = new System.Windows.Forms.Label();
            this.btnContinueCopie = new System.Windows.Forms.Button();
            this.btnPauseCopie = new System.Windows.Forms.Button();
            this.btnArretCopie = new System.Windows.Forms.Button();
            this.labelMessageCopie = new System.Windows.Forms.Label();
            this.pictureMessageCopieNotif = new System.Windows.Forms.PictureBox();
            this.btnFermerCopie = new System.Windows.Forms.Button();
            this.btnFusionerDossiers = new System.Windows.Forms.Button();
            this.textMessageCopie = new System.Windows.Forms.RichTextBox();
            this.btnNePasCopier = new System.Windows.Forms.Button();
            this.btnRemplacerFichier = new System.Windows.Forms.Button();
            this.pictureMessageCopieOk = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTitreChargement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureMessageCopieNotif)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureMessageCopieOk)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxTitreChargement
            // 
            this.pictureBoxTitreChargement.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBoxTitreChargement.BackgroundImage")));
            this.pictureBoxTitreChargement.Location = new System.Drawing.Point(7, 10);
            this.pictureBoxTitreChargement.Name = "pictureBoxTitreChargement";
            this.pictureBoxTitreChargement.Size = new System.Drawing.Size(22, 23);
            this.pictureBoxTitreChargement.TabIndex = 100;
            this.pictureBoxTitreChargement.TabStop = false;
            // 
            // labelTitreChargement
            // 
            this.labelTitreChargement.AutoSize = true;
            this.labelTitreChargement.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitreChargement.Location = new System.Drawing.Point(35, 10);
            this.labelTitreChargement.Name = "labelTitreChargement";
            this.labelTitreChargement.Size = new System.Drawing.Size(259, 19);
            this.labelTitreChargement.TabIndex = 99;
            this.labelTitreChargement.Text = "Copie sur les supports de destination";
            // 
            // progressBarCles
            // 
            this.progressBarCles.BackColor = System.Drawing.SystemColors.Highlight;
            this.progressBarCles.Location = new System.Drawing.Point(7, 65);
            this.progressBarCles.Name = "progressBarCles";
            this.progressBarCles.Size = new System.Drawing.Size(559, 25);
            this.progressBarCles.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarCles.TabIndex = 101;
            // 
            // progressBarFichiers
            // 
            this.progressBarFichiers.Location = new System.Drawing.Point(7, 118);
            this.progressBarFichiers.Name = "progressBarFichiers";
            this.progressBarFichiers.Size = new System.Drawing.Size(559, 25);
            this.progressBarFichiers.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarFichiers.TabIndex = 102;
            // 
            // labelCopieCle
            // 
            this.labelCopieCle.AutoSize = true;
            this.labelCopieCle.Location = new System.Drawing.Point(6, 48);
            this.labelCopieCle.Name = "labelCopieCle";
            this.labelCopieCle.Size = new System.Drawing.Size(162, 14);
            this.labelCopieCle.TabIndex = 103;
            this.labelCopieCle.Text = "Pourcentage de clés traitées";
            // 
            // labelCopieFichier
            // 
            this.labelCopieFichier.AutoSize = true;
            this.labelCopieFichier.Location = new System.Drawing.Point(6, 101);
            this.labelCopieFichier.Name = "labelCopieFichier";
            this.labelCopieFichier.Size = new System.Drawing.Size(172, 14);
            this.labelCopieFichier.TabIndex = 104;
            this.labelCopieFichier.Text = "Pourcentage de fichiers copiés";
            // 
            // btnContinueCopie
            // 
            this.btnContinueCopie.BackColor = System.Drawing.Color.Transparent;
            this.btnContinueCopie.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnContinueCopie.BackgroundImage")));
            this.btnContinueCopie.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnContinueCopie.FlatAppearance.BorderSize = 0;
            this.btnContinueCopie.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContinueCopie.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContinueCopie.ForeColor = System.Drawing.Color.White;
            this.btnContinueCopie.Location = new System.Drawing.Point(404, 153);
            this.btnContinueCopie.Margin = new System.Windows.Forms.Padding(0);
            this.btnContinueCopie.Name = "btnContinueCopie";
            this.btnContinueCopie.Size = new System.Drawing.Size(75, 25);
            this.btnContinueCopie.TabIndex = 106;
            this.btnContinueCopie.Text = "Continuer";
            this.btnContinueCopie.UseVisualStyleBackColor = false;
            this.btnContinueCopie.Click += new System.EventHandler(this.btnContinueCopie_Click);
            // 
            // btnPauseCopie
            // 
            this.btnPauseCopie.BackColor = System.Drawing.Color.Transparent;
            this.btnPauseCopie.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPauseCopie.BackgroundImage")));
            this.btnPauseCopie.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPauseCopie.FlatAppearance.BorderSize = 0;
            this.btnPauseCopie.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPauseCopie.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPauseCopie.ForeColor = System.Drawing.Color.White;
            this.btnPauseCopie.Location = new System.Drawing.Point(317, 153);
            this.btnPauseCopie.Margin = new System.Windows.Forms.Padding(0);
            this.btnPauseCopie.Name = "btnPauseCopie";
            this.btnPauseCopie.Size = new System.Drawing.Size(75, 25);
            this.btnPauseCopie.TabIndex = 107;
            this.btnPauseCopie.Text = "Pause";
            this.btnPauseCopie.UseVisualStyleBackColor = false;
            this.btnPauseCopie.Click += new System.EventHandler(this.btnPauseCopie_Click);
            // 
            // btnArretCopie
            // 
            this.btnArretCopie.BackColor = System.Drawing.Color.Transparent;
            this.btnArretCopie.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnArretCopie.BackgroundImage")));
            this.btnArretCopie.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnArretCopie.FlatAppearance.BorderSize = 0;
            this.btnArretCopie.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArretCopie.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnArretCopie.ForeColor = System.Drawing.Color.White;
            this.btnArretCopie.Location = new System.Drawing.Point(491, 153);
            this.btnArretCopie.Margin = new System.Windows.Forms.Padding(0);
            this.btnArretCopie.Name = "btnArretCopie";
            this.btnArretCopie.Size = new System.Drawing.Size(75, 25);
            this.btnArretCopie.TabIndex = 109;
            this.btnArretCopie.Text = "Annuler";
            this.btnArretCopie.UseVisualStyleBackColor = false;
            this.btnArretCopie.Click += new System.EventHandler(this.btnArretCopie_Click);
            // 
            // labelMessageCopie
            // 
            this.labelMessageCopie.AutoSize = true;
            this.labelMessageCopie.ForeColor = System.Drawing.Color.Black;
            this.labelMessageCopie.Location = new System.Drawing.Point(37, 206);
            this.labelMessageCopie.Name = "labelMessageCopie";
            this.labelMessageCopie.Size = new System.Drawing.Size(136, 14);
            this.labelMessageCopie.TabIndex = 117;
            this.labelMessageCopie.Text = "Message de notification";
            // 
            // pictureMessageCopieNotif
            // 
            this.pictureMessageCopieNotif.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureMessageCopieNotif.BackgroundImage")));
            this.pictureMessageCopieNotif.Location = new System.Drawing.Point(9, 202);
            this.pictureMessageCopieNotif.Name = "pictureMessageCopieNotif";
            this.pictureMessageCopieNotif.Size = new System.Drawing.Size(22, 23);
            this.pictureMessageCopieNotif.TabIndex = 116;
            this.pictureMessageCopieNotif.TabStop = false;
            // 
            // btnFermerCopie
            // 
            this.btnFermerCopie.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFermerCopie.BackgroundImage")));
            this.btnFermerCopie.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFermerCopie.FlatAppearance.BorderSize = 0;
            this.btnFermerCopie.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFermerCopie.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFermerCopie.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.btnFermerCopie.Location = new System.Drawing.Point(431, 291);
            this.btnFermerCopie.Name = "btnFermerCopie";
            this.btnFermerCopie.Size = new System.Drawing.Size(135, 32);
            this.btnFermerCopie.TabIndex = 112;
            this.btnFermerCopie.Text = "Fermer";
            this.btnFermerCopie.UseVisualStyleBackColor = true;
            this.btnFermerCopie.Click += new System.EventHandler(this.btnFermerCopie_Click);
            // 
            // btnFusionerDossiers
            // 
            this.btnFusionerDossiers.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFusionerDossiers.BackgroundImage")));
            this.btnFusionerDossiers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFusionerDossiers.FlatAppearance.BorderSize = 0;
            this.btnFusionerDossiers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFusionerDossiers.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFusionerDossiers.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.btnFusionerDossiers.Location = new System.Drawing.Point(7, 291);
            this.btnFusionerDossiers.Name = "btnFusionerDossiers";
            this.btnFusionerDossiers.Size = new System.Drawing.Size(135, 32);
            this.btnFusionerDossiers.TabIndex = 111;
            this.btnFusionerDossiers.Text = "Fusionner les dossiers";
            this.btnFusionerDossiers.UseVisualStyleBackColor = true;
            // 
            // textMessageCopie
            // 
            this.textMessageCopie.Location = new System.Drawing.Point(7, 235);
            this.textMessageCopie.Name = "textMessageCopie";
            this.textMessageCopie.ReadOnly = true;
            this.textMessageCopie.Size = new System.Drawing.Size(559, 50);
            this.textMessageCopie.TabIndex = 118;
            this.textMessageCopie.Text = "";
            // 
            // btnNePasCopier
            // 
            this.btnNePasCopier.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNePasCopier.BackgroundImage")));
            this.btnNePasCopier.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNePasCopier.FlatAppearance.BorderSize = 0;
            this.btnNePasCopier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNePasCopier.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNePasCopier.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.btnNePasCopier.Location = new System.Drawing.Point(148, 291);
            this.btnNePasCopier.Name = "btnNePasCopier";
            this.btnNePasCopier.Size = new System.Drawing.Size(135, 32);
            this.btnNePasCopier.TabIndex = 120;
            this.btnNePasCopier.Text = "Ne pas copier";
            this.btnNePasCopier.UseVisualStyleBackColor = true;
            // 
            // btnRemplacerFichier
            // 
            this.btnRemplacerFichier.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRemplacerFichier.BackgroundImage")));
            this.btnRemplacerFichier.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemplacerFichier.FlatAppearance.BorderSize = 0;
            this.btnRemplacerFichier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemplacerFichier.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemplacerFichier.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.btnRemplacerFichier.Location = new System.Drawing.Point(290, 291);
            this.btnRemplacerFichier.Name = "btnRemplacerFichier";
            this.btnRemplacerFichier.Size = new System.Drawing.Size(135, 32);
            this.btnRemplacerFichier.TabIndex = 119;
            this.btnRemplacerFichier.Text = "Remplacer le fichier";
            this.btnRemplacerFichier.UseVisualStyleBackColor = true;
            // 
            // pictureMessageCopieOk
            // 
            this.pictureMessageCopieOk.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureMessageCopieOk.BackgroundImage")));
            this.pictureMessageCopieOk.Location = new System.Drawing.Point(9, 202);
            this.pictureMessageCopieOk.Name = "pictureMessageCopieOk";
            this.pictureMessageCopieOk.Size = new System.Drawing.Size(22, 23);
            this.pictureMessageCopieOk.TabIndex = 121;
            this.pictureMessageCopieOk.TabStop = false;
            // 
            // Chargement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(576, 343);
            this.ControlBox = false;
            this.Controls.Add(this.pictureMessageCopieOk);
            this.Controls.Add(this.btnNePasCopier);
            this.Controls.Add(this.btnRemplacerFichier);
            this.Controls.Add(this.textMessageCopie);
            this.Controls.Add(this.labelMessageCopie);
            this.Controls.Add(this.pictureMessageCopieNotif);
            this.Controls.Add(this.btnFermerCopie);
            this.Controls.Add(this.btnFusionerDossiers);
            this.Controls.Add(this.btnArretCopie);
            this.Controls.Add(this.btnPauseCopie);
            this.Controls.Add(this.btnContinueCopie);
            this.Controls.Add(this.labelCopieFichier);
            this.Controls.Add(this.labelCopieCle);
            this.Controls.Add(this.progressBarFichiers);
            this.Controls.Add(this.progressBarCles);
            this.Controls.Add(this.pictureBoxTitreChargement);
            this.Controls.Add(this.labelTitreChargement);
            this.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Chargement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Copie";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTitreChargement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureMessageCopieNotif)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureMessageCopieOk)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxTitreChargement;
        private System.Windows.Forms.Label labelTitreChargement;
        private System.Windows.Forms.ProgressBar progressBarCles;
        private System.Windows.Forms.ProgressBar progressBarFichiers;
        private System.Windows.Forms.Label labelCopieCle;
        private System.Windows.Forms.Label labelCopieFichier;
        private System.Windows.Forms.Button btnContinueCopie;
        private System.Windows.Forms.Button btnPauseCopie;
        private System.Windows.Forms.Button btnArretCopie;
        private System.Windows.Forms.Label labelMessageCopie;
        private System.Windows.Forms.PictureBox pictureMessageCopieNotif;
        private System.Windows.Forms.Button btnFermerCopie;
        private System.Windows.Forms.Button btnFusionerDossiers;
        private System.Windows.Forms.RichTextBox textMessageCopie;
        private System.Windows.Forms.Button btnNePasCopier;
        private System.Windows.Forms.Button btnRemplacerFichier;
        private System.Windows.Forms.PictureBox pictureMessageCopieOk;
    }
}