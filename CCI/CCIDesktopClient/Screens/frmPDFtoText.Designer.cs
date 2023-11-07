namespace CCI.DesktopClient.Screens
{
  partial class frmPDFtoText
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
      this.btnSelectFile = new System.Windows.Forms.Button();
      this.txtFilePath = new System.Windows.Forms.TextBox();
      this.btnImport = new System.Windows.Forms.Button();
      this.txtFileResult = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // btnSelectFile
      // 
      this.btnSelectFile.Location = new System.Drawing.Point(88, 71);
      this.btnSelectFile.Name = "btnSelectFile";
      this.btnSelectFile.Size = new System.Drawing.Size(139, 23);
      this.btnSelectFile.TabIndex = 0;
      this.btnSelectFile.Text = "Select File to Import";
      this.btnSelectFile.UseVisualStyleBackColor = true;
      this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
      // 
      // txtFilePath
      // 
      this.txtFilePath.Location = new System.Drawing.Point(249, 73);
      this.txtFilePath.Name = "txtFilePath";
      this.txtFilePath.Size = new System.Drawing.Size(569, 20);
      this.txtFilePath.TabIndex = 1;
      // 
      // btnImport
      // 
      this.btnImport.Location = new System.Drawing.Point(88, 113);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new System.Drawing.Size(139, 23);
      this.btnImport.TabIndex = 2;
      this.btnImport.Text = "Import PDF File";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
      // 
      // txtFileResult
      // 
      this.txtFileResult.Location = new System.Drawing.Point(249, 115);
      this.txtFileResult.Multiline = true;
      this.txtFileResult.Name = "txtFileResult";
      this.txtFileResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.txtFileResult.Size = new System.Drawing.Size(568, 416);
      this.txtFileResult.TabIndex = 3;
      // 
      // frmPDFtoText
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(852, 572);
      this.Controls.Add(this.txtFileResult);
      this.Controls.Add(this.btnImport);
      this.Controls.Add(this.txtFilePath);
      this.Controls.Add(this.btnSelectFile);
      this.Name = "frmPDFtoText";
      this.Text = "frmPDFtoText";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnSelectFile;
    private System.Windows.Forms.TextBox txtFilePath;
    private System.Windows.Forms.Button btnImport;
    private System.Windows.Forms.TextBox txtFileResult;
  }
}