﻿namespace CCI.DesktopClient.Screens
{
    partial class frmAdminDeveloper
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
            this.btnCreateIBPTempTable = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreateIBPTempTable
            // 
            this.btnCreateIBPTempTable.Location = new System.Drawing.Point(31, 157);
            this.btnCreateIBPTempTable.Name = "btnCreateIBPTempTable";
            this.btnCreateIBPTempTable.Size = new System.Drawing.Size(229, 30);
            this.btnCreateIBPTempTable.TabIndex = 0;
            this.btnCreateIBPTempTable.Text = "Create Dealer temp Table";
            this.btnCreateIBPTempTable.UseVisualStyleBackColor = true;
            this.btnCreateIBPTempTable.Click += new System.EventHandler(this.btnCreateIBPTempTable_Click);
            // 
            // frmAdminDeveloper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 273);
            this.Controls.Add(this.btnCreateIBPTempTable);
            this.Name = "frmAdminDeveloper";
            this.Text = "frmAdminDeveloper";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreateIBPTempTable;
    }
}