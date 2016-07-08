namespace ACG.DesktopClient.Screens
{
  partial class frmRates
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
      this.cboResource = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.ctlWholesaleRate = new ACG.DesktopClient.Common.ctlMaintenanceBase();
      this.ctlRetailRates = new ACG.DesktopClient.Common.ctlMaintenanceBase();
      this.SuspendLayout();
      // 
      // cboResource
      // 
      this.cboResource.FormattingEnabled = true;
      this.cboResource.Location = new System.Drawing.Point(69, 6);
      this.cboResource.Name = "cboResource";
      this.cboResource.Size = new System.Drawing.Size(186, 21);
      this.cboResource.TabIndex = 1;
      this.cboResource.SelectedIndexChanged += new System.EventHandler(this.cboResource_SelectedIndexChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(7, 8);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(53, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Resource";
      // 
      // ctlWholesaleRate
      // 
      this.ctlWholesaleRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ctlWholesaleRate.EncryptedFieldName = null;
      this.ctlWholesaleRate.ExtendedFieldCaptionColumn = null;
      this.ctlWholesaleRate.ExtendedFieldName = null;
      this.ctlWholesaleRate.FireRowSelectedOnSearch = true;
      this.ctlWholesaleRate.Location = new System.Drawing.Point(10, 180);
      this.ctlWholesaleRate.Name = "ctlWholesaleRate";
      this.ctlWholesaleRate.NoteLabel = "Note for";
      this.ctlWholesaleRate.ReadOnly = false;
      this.ctlWholesaleRate.SecurityContext = null;
      this.ctlWholesaleRate.ShowExtendedField = false;
      this.ctlWholesaleRate.Size = new System.Drawing.Size(1157, 320);
      this.ctlWholesaleRate.TabIndex = 3;
      this.ctlWholesaleRate.TableName = "";
      this.ctlWholesaleRate.UniqueIdentifier = null;
      // 
      // ctlRetailRates
      // 
      this.ctlRetailRates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ctlRetailRates.EncryptedFieldName = null;
      this.ctlRetailRates.ExtendedFieldCaptionColumn = null;
      this.ctlRetailRates.ExtendedFieldName = null;
      this.ctlRetailRates.FireRowSelectedOnSearch = true;
      this.ctlRetailRates.Location = new System.Drawing.Point(11, 38);
      this.ctlRetailRates.Name = "ctlRetailRates";
      this.ctlRetailRates.NoteLabel = "Note for";
      this.ctlRetailRates.ReadOnly = false;
      this.ctlRetailRates.SecurityContext = null;
      this.ctlRetailRates.ShowExtendedField = false;
      this.ctlRetailRates.Size = new System.Drawing.Size(1157, 131);
      this.ctlRetailRates.TabIndex = 2;
      this.ctlRetailRates.TableName = "";
      this.ctlRetailRates.UniqueIdentifier = null;
      // 
      // frmRates
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1173, 505);
      this.Controls.Add(this.ctlWholesaleRate);
      this.Controls.Add(this.ctlRetailRates);
      this.Controls.Add(this.cboResource);
      this.Controls.Add(this.label1);
      this.Name = "frmRates";
      this.Text = "frmRates";
      this.Load += new System.EventHandler(this.frmRates_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cboResource;
    private Common.ctlMaintenanceBase ctlRetailRates;
    private Common.ctlMaintenanceBase ctlWholesaleRate;
  }
}