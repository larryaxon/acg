namespace CCI.DesktopClient.Screens
{
  partial class frmAgentMaintenance
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
      this.pnlDetail = new System.Windows.Forms.Panel();
      this.label5 = new System.Windows.Forms.Label();
      this.SrchCustomers = new CCI.DesktopClient.Common.ctlSearchGrid();
      this.tabMain.SuspendLayout();
      this.tabLocations.SuspendLayout();
      this.pnlDetail.SuspendLayout();
      this.SuspendLayout();
      // 
      // ctlEntitySearch1
      // 
      this.ctlEntitySearch1.ShowTermedCheckBox = true;
      // 
      // pnlDetail
      // 
      this.pnlDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlDetail.Controls.Add(this.label5);
      this.pnlDetail.Controls.Add(this.SrchCustomers);
      this.pnlDetail.Location = new System.Drawing.Point(10, 201);
      this.pnlDetail.Name = "pnlDetail";
      this.pnlDetail.Size = new System.Drawing.Size(1181, 368);
      this.pnlDetail.TabIndex = 19;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(3, 4);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(172, 13);
      this.label5.TabIndex = 2;
      this.label5.Text = "Customers for this Master Customer";
      // 
      // SrchCustomers
      // 
      this.SrchCustomers.AllowSortByColumn = true;
      this.SrchCustomers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.SrchCustomers.AutoRefreshWhenFieldChecked = false;
      this.SrchCustomers.AutoSaveUserOptions = false;
      this.SrchCustomers.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.SrchCustomers.CanChangeDisplayFields = false;
      this.SrchCustomers.CanChangeDisplaySearchCriteria = false;
      this.SrchCustomers.ColumnName = "";
      this.SrchCustomers.DisplayFields = false;
      this.SrchCustomers.DisplaySearchCriteria = false;
      this.SrchCustomers.FieldsDefaultIsChecked = true;
      this.SrchCustomers.ForceReloadSearchColumns = false;
      this.SrchCustomers.IDList = null;
      this.SrchCustomers.IncludeGroupAsCriteria = false;
      this.SrchCustomers.InnerWhere = "";
      this.SrchCustomers.Location = new System.Drawing.Point(0, 24);
      this.SrchCustomers.Name = "SrchCustomers";
      this.SrchCustomers.NameType = CCI.Common.CommonData.UnmatchedNameTypes.None;
      this.SrchCustomers.SearchCriteria = null;
      this.SrchCustomers.Size = new System.Drawing.Size(1181, 344);
      this.SrchCustomers.TabIndex = 0;
      this.SrchCustomers.Title = "Search (0 Records Found)";
      this.SrchCustomers.UniqueIdentifier = "ID";
      this.SrchCustomers.UseNamedSearches = false;
      // 
      // frmMasterCustomerMaintenance
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1196, 569);
      this.Controls.Add(this.pnlDetail);
      this.Name = "frmMasterCustomerMaintenance";
      this.Text = "frmCarrierMaintenance";
      this.Controls.SetChildIndex(this.lblEntity, 0);
      this.Controls.SetChildIndex(this.ctlEntitySearch1, 0);
      this.Controls.SetChildIndex(this.txtLegalName, 0);
      this.Controls.SetChildIndex(this.txtEntity, 0);
      this.Controls.SetChildIndex(this.lblNewRecord, 0);
      this.Controls.SetChildIndex(this.tabMain, 0);
      this.Controls.SetChildIndex(this.pnlDetail, 0);
      this.tabMain.ResumeLayout(false);
      this.tabLocations.ResumeLayout(false);
      this.pnlDetail.ResumeLayout(false);
      this.pnlDetail.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel pnlDetail;
    private Common.ctlSearchGrid SrchCustomers;
    private System.Windows.Forms.Label label5;
  }
}