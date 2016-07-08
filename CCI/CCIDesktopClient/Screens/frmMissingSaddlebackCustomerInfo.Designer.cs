namespace CCI.DesktopClient.Screens
{
  partial class frmMissingSaddlebackCustomerInfo
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMissingSaddlebackCustomerInfo));
      this.srchCustomers = new CCI.DesktopClient.Common.ctlSearchGrid();
      this.SuspendLayout();
      // 
      // srchCustomers
      // 
      this.srchCustomers.AllowSortByColumn = true;
      this.srchCustomers.CanChangeDisplaySearchCriteria = true;
      this.srchCustomers.ColumnName = "CustomerName";
      this.srchCustomers.DisplaySearchCriteria = false;
      this.srchCustomers.Dock = System.Windows.Forms.DockStyle.Fill;
      this.srchCustomers.ForceReloadSearchColumns = false;
      this.srchCustomers.InnerWhere = "";
      this.srchCustomers.Location = new System.Drawing.Point(0, 0);
      this.srchCustomers.Name = "srchCustomers";
      this.srchCustomers.NameType = CCI.Common.CommonData.UnmatchedNameTypes.Customer;
      this.srchCustomers.Size = new System.Drawing.Size(895, 546);
      this.srchCustomers.TabIndex = 0;
      this.srchCustomers.Title = "Search (0 Records Found)";
      this.srchCustomers.UniqueIdentifier = "CustomerID";
      this.srchCustomers.RowSelected += new CCI.DesktopClient.Common.ctlSearchGrid.RowSelectedHandler(this.srchCustomers_RowSelected);
      // 
      // frmMissingSaddlebackCustomerInfo
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(895, 546);
      this.Controls.Add(this.srchCustomers);
      this.Name = "frmMissingSaddlebackCustomerInfo";
      this.Text = "frmMissingSaddlebackCustomerInfo";
      this.Load += new System.EventHandler(this.frmMissingSaddlebackCustomerInfo_Load);
      this.Enter += new System.EventHandler(this.frmMissingSaddlebackCustomerInfo_Enter);
      this.ResumeLayout(false);

    }

    #endregion

    private Common.ctlSearchGrid srchCustomers;
  }
}