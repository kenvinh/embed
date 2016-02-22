namespace Toeic_score
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.txb_TestSelect = new System.Windows.Forms.TextBox();
            this.btn_BrowseTestItem = new System.Windows.Forms.Button();
            this.cmb_UsersList = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_UserHello = new System.Windows.Forms.Label();
            this.lbl_Status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(201, 124);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 29);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txb_TestSelect
            // 
            this.txb_TestSelect.Location = new System.Drawing.Point(201, 73);
            this.txb_TestSelect.Name = "txb_TestSelect";
            this.txb_TestSelect.Size = new System.Drawing.Size(350, 20);
            this.txb_TestSelect.TabIndex = 1;
            // 
            // btn_BrowseTestItem
            // 
            this.btn_BrowseTestItem.Location = new System.Drawing.Point(578, 68);
            this.btn_BrowseTestItem.Name = "btn_BrowseTestItem";
            this.btn_BrowseTestItem.Size = new System.Drawing.Size(101, 29);
            this.btn_BrowseTestItem.TabIndex = 2;
            this.btn_BrowseTestItem.Text = "Browse...";
            this.btn_BrowseTestItem.UseVisualStyleBackColor = true;
            this.btn_BrowseTestItem.Click += new System.EventHandler(this.button2_Click);
            // 
            // cmb_UsersList
            // 
            this.cmb_UsersList.FormattingEnabled = true;
            this.cmb_UsersList.Location = new System.Drawing.Point(201, 10);
            this.cmb_UsersList.Name = "cmb_UsersList";
            this.cmb_UsersList.Size = new System.Drawing.Size(125, 21);
            this.cmb_UsersList.TabIndex = 3;
            this.cmb_UsersList.DropDown += new System.EventHandler(this.cmb_UsersList_DropDown);
            this.cmb_UsersList.SelectionChangeCommitted += new System.EventHandler(this.cmb_UsersList_SelectionChangeCommitted);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(346, 9);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(101, 21);
            this.button3.TabIndex = 4;
            this.button3.Text = "New User";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select User Profile or Create new:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Select test item: ";
            // 
            // lbl_UserHello
            // 
            this.lbl_UserHello.AutoSize = true;
            this.lbl_UserHello.Location = new System.Drawing.Point(469, 10);
            this.lbl_UserHello.Name = "lbl_UserHello";
            this.lbl_UserHello.Size = new System.Drawing.Size(0, 13);
            this.lbl_UserHello.TabIndex = 7;
            // 
            // lbl_Status
            // 
            this.lbl_Status.AutoSize = true;
            this.lbl_Status.Location = new System.Drawing.Point(198, 45);
            this.lbl_Status.Name = "lbl_Status";
            this.lbl_Status.Size = new System.Drawing.Size(0, 13);
            this.lbl_Status.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 427);
            this.Controls.Add(this.lbl_Status);
            this.Controls.Add(this.lbl_UserHello);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.cmb_UsersList);
            this.Controls.Add(this.btn_BrowseTestItem);
            this.Controls.Add(this.txb_TestSelect);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Click += new System.EventHandler(this.Form1_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txb_TestSelect;
        private System.Windows.Forms.Button btn_BrowseTestItem;
        private System.Windows.Forms.ComboBox cmb_UsersList;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_UserHello;
        private System.Windows.Forms.Label lbl_Status;
    }
}

