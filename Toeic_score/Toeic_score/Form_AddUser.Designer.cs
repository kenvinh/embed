namespace Toeic_score
{
    partial class Form_AddUser
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
            this.label1 = new System.Windows.Forms.Label();
            this.btn_NewUser = new System.Windows.Forms.Button();
            this.txb_NewUserName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "UserName";
            // 
            // btn_NewUser
            // 
            this.btn_NewUser.Location = new System.Drawing.Point(191, 35);
            this.btn_NewUser.Name = "btn_NewUser";
            this.btn_NewUser.Size = new System.Drawing.Size(80, 25);
            this.btn_NewUser.TabIndex = 1;
            this.btn_NewUser.Text = "Create";
            this.btn_NewUser.UseVisualStyleBackColor = true;
            this.btn_NewUser.Click += new System.EventHandler(this.btn_NewUser_Click);
            // 
            // txb_NewUserName
            // 
            this.txb_NewUserName.Location = new System.Drawing.Point(92, 9);
            this.txb_NewUserName.Name = "txb_NewUserName";
            this.txb_NewUserName.Size = new System.Drawing.Size(179, 20);
            this.txb_NewUserName.TabIndex = 2;
            this.txb_NewUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txb_NewUserName_KeyDown);
            // 
            // Form_AddUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.txb_NewUserName);
            this.Controls.Add(this.btn_NewUser);
            this.Controls.Add(this.label1);
            this.Name = "Form_AddUser";
            this.Text = "Form_AddUser";
            this.Load += new System.EventHandler(this.Form_AddUser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_NewUser;
        private System.Windows.Forms.TextBox txb_NewUserName;
    }
}