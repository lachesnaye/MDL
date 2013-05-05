namespace MaisonDesLigues
{
    partial class FrmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            this.CmdOk = new System.Windows.Forms.Button();
            this.TxtMdp = new System.Windows.Forms.TextBox();
            this.LblMdp = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtLogin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CmdOk
            // 
            this.CmdOk.Location = new System.Drawing.Point(136, 83);
            this.CmdOk.Name = "CmdOk";
            this.CmdOk.Size = new System.Drawing.Size(86, 24);
            this.CmdOk.TabIndex = 8;
            this.CmdOk.Text = "Connecter";
            this.CmdOk.Click += new System.EventHandler(this.CmdOk_Click);
            // 
            // TxtMdp
            // 
            this.TxtMdp.Location = new System.Drawing.Point(112, 56);
            this.TxtMdp.Name = "TxtMdp";
            this.TxtMdp.PasswordChar = '*';
            this.TxtMdp.Size = new System.Drawing.Size(168, 20);
            this.TxtMdp.TabIndex = 7;
            this.TxtMdp.Text = "mdl";
            // 
            // LblMdp
            // 
            this.LblMdp.AutoSize = true;
            this.LblMdp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblMdp.Location = new System.Drawing.Point(9, 60);
            this.LblMdp.Name = "LblMdp";
            this.LblMdp.Size = new System.Drawing.Size(97, 16);
            this.LblMdp.TabIndex = 6;
            this.LblMdp.Text = "Mot de Passe :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(59, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Login :";
            // 
            // TxtLogin
            // 
            this.TxtLogin.Location = new System.Drawing.Point(112, 30);
            this.TxtLogin.Name = "TxtLogin";
            this.TxtLogin.Size = new System.Drawing.Size(168, 20);
            this.TxtLogin.TabIndex = 11;
            this.TxtLogin.Text = "mdl";
            this.TxtLogin.TextChanged += new System.EventHandler(this.ControleValide);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(92, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(202, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "Application Assises de l\'escrime";
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 119);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtLogin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CmdOk);
            this.Controls.Add(this.TxtMdp);
            this.Controls.Add(this.LblMdp);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmLogin";
            this.Load += new System.EventHandler(this.FrmLogin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CmdOk;
        internal System.Windows.Forms.TextBox TxtMdp;
        internal System.Windows.Forms.Label LblMdp;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox TxtLogin;
        internal System.Windows.Forms.Label label3;
    }
}