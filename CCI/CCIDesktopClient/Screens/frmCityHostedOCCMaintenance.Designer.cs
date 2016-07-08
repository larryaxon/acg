namespace CCI.DesktopClient.Screens
{
  partial class frmCityHostedOCCMaintenance
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCityHostedOCCMaintenance));
      this.label1 = new System.Windows.Forms.Label();
      this.srchDealer = new ACG.CommonForms.ctlSearch();
      this.srchCustomer = new ACG.CommonForms.ctlSearch();
      this.label2 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.srchBillDate = new System.Windows.Forms.DateTimePicker();
      this.splitMain = new System.Windows.Forms.SplitContainer();
      this.srchOCCAdjustments = new CCI.DesktopClient.Common.ctlSearchGrid();
      this.txtCustomer = new ACG.CommonForms.ctlSearch();
      this.dtBillDate = new System.Windows.Forms.DateTimePicker();
      this.txtID = new System.Windows.Forms.TextBox();
      this.btnDelete = new System.Windows.Forms.Button();
      this.btnSave = new System.Windows.Forms.Button();
      this.btnNew = new System.Windows.Forms.Button();
      this.txtDescription = new System.Windows.Forms.TextBox();
      this.label11 = new System.Windows.Forms.Label();
      this.txtRetailAmount = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.ckAdjustmentsOnly = new System.Windows.Forms.CheckBox();
      this.btnLoad = new System.Windows.Forms.Button();
      this.ckAutoRefresh = new System.Windows.Forms.CheckBox();
      this.splitMain.Panel1.SuspendLayout();
      this.splitMain.Panel2.SuspendLayout();
      this.splitMain.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(8, 7);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(38, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Dealer";
      // 
      // srchDealer
      // 
      this.srchDealer.AddNewMode = false;
      this.srchDealer.AutoSelectWhenMatch = false;
      this.srchDealer.ClearSearchWhenComplete = false;
      this.srchDealer.Collapsed = true;
      this.srchDealer.CreatedNewItem = false;
      this.srchDealer.DisplayOnlyID = false;
      this.srchDealer.ID = "";
      this.srchDealer.Location = new System.Drawing.Point(52, 7);
      this.srchDealer.MaxHeight = 228;
      this.srchDealer.Name = "srchDealer";
      this.srchDealer.ShowCustomerNameWhenSet = true;
      this.srchDealer.ShowTermedCheckBox = false;
      this.srchDealer.Size = new System.Drawing.Size(273, 25);
      this.srchDealer.TabIndex = 0;
      this.srchDealer.OnSelected += new System.EventHandler<System.EventArgs>(this.srchDealer_OnSelected);
      // 
      // srchCustomer
      // 
      this.srchCustomer.AddNewMode = false;
      this.srchCustomer.AutoSelectWhenMatch = false;
      this.srchCustomer.ClearSearchWhenComplete = false;
      this.srchCustomer.Collapsed = true;
      this.srchCustomer.CreatedNewItem = false;
      this.srchCustomer.DisplayOnlyID = false;
      this.srchCustomer.ID = "";
      this.srchCustomer.Location = new System.Drawing.Point(387, 7);
      this.srchCustomer.MaxHeight = 228;
      this.srchCustomer.Name = "srchCustomer";
      this.srchCustomer.ShowCustomerNameWhenSet = true;
      this.srchCustomer.ShowTermedCheckBox = false;
      this.srchCustomer.Size = new System.Drawing.Size(269, 25);
      this.srchCustomer.TabIndex = 1;
      this.srchCustomer.OnSelected += new System.EventHandler<System.EventArgs>(this.srchCustomer_OnSelected);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(330, 7);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(51, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Customer";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(670, 9);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(46, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Bill Date";
      // 
      // srchBillDate
      // 
      this.srchBillDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.srchBillDate.Location = new System.Drawing.Point(722, 7);
      this.srchBillDate.Name = "srchBillDate";
      this.srchBillDate.Size = new System.Drawing.Size(100, 20);
      this.srchBillDate.TabIndex = 2;
      this.srchBillDate.ValueChanged += new System.EventHandler(this.srchBillDate_ValueChanged);
      // 
      // splitMain
      // 
      this.splitMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.splitMain.Location = new System.Drawing.Point(3, 33);
      this.splitMain.Name = "splitMain";
      this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitMain.Panel1
      // 
      this.splitMain.Panel1.Controls.Add(this.srchOCCAdjustments);
      // 
      // splitMain.Panel2
      // 
      this.splitMain.Panel2.Controls.Add(this.txtCustomer);
      this.splitMain.Panel2.Controls.Add(this.dtBillDate);
      this.splitMain.Panel2.Controls.Add(this.txtID);
      this.splitMain.Panel2.Controls.Add(this.btnDelete);
      this.splitMain.Panel2.Controls.Add(this.btnSave);
      this.splitMain.Panel2.Controls.Add(this.btnNew);
      this.splitMain.Panel2.Controls.Add(this.txtDescription);
      this.splitMain.Panel2.Controls.Add(this.label11);
      this.splitMain.Panel2.Controls.Add(this.txtRetailAmount);
      this.splitMain.Panel2.Controls.Add(this.label8);
      this.splitMain.Panel2.Controls.Add(this.label6);
      this.splitMain.Panel2.Controls.Add(this.label5);
      this.splitMain.Size = new System.Drawing.Size(1236, 499);
      this.splitMain.SplitterDistance = 361;
      this.splitMain.TabIndex = 8;
      // 
      // srchOCCAdjustments
      // 
      this.srchOCCAdjustments.AllowSortByColumn = true;
      this.srchOCCAdjustments.AutoRefreshWhenFieldChecked = true;
      this.srchOCCAdjustments.CanChangeDisplayFields = true;
      this.srchOCCAdjustments.CanChangeDisplaySearchCriteria = true;
      this.srchOCCAdjustments.ColumnName = "CustomerID";
      this.srchOCCAdjustments.DisplayFields = false;
      this.srchOCCAdjustments.DisplaySearchCriteria = false;
      this.srchOCCAdjustments.Dock = System.Windows.Forms.DockStyle.Fill;
      this.srchOCCAdjustments.FieldsDefaultIsChecked = true;
      this.srchOCCAdjustments.ForceReloadSearchColumns = false;
      this.srchOCCAdjustments.IncludeGroupAsCriteria = false;
      this.srchOCCAdjustments.InnerWhere = "";
      this.srchOCCAdjustments.Location = new System.Drawing.Point(0, 0);
      this.srchOCCAdjustments.Name = "srchOCCAdjustments";
      this.srchOCCAdjustments.NameType = CCI.Common.CommonData.UnmatchedNameTypes.Customer;
      this.srchOCCAdjustments.Size = new System.Drawing.Size(1236, 361);
      this.srchOCCAdjustments.TabIndex = 0;
      this.srchOCCAdjustments.Title = "Search (0 Records Found)";
      this.srchOCCAdjustments.UniqueIdentifier = "ID";
      this.srchOCCAdjustments.RowSelected += new CCI.DesktopClient.Common.ctlSearchGrid.RowSelectedHandler(this.srchOCCAdjustments_RowSelected);
      // 
      // txtCustomer
      // 
      this.txtCustomer.AddNewMode = false;
      this.txtCustomer.AutoSelectWhenMatch = false;
      this.txtCustomer.ClearSearchWhenComplete = false;
      this.txtCustomer.Collapsed = true;
      this.txtCustomer.CreatedNewItem = false;
      this.txtCustomer.DisplayOnlyID = false;
      this.txtCustomer.ID = "";
      this.txtCustomer.Location = new System.Drawing.Point(90, 14);
      this.txtCustomer.MaxHeight = 228;
      this.txtCustomer.Name = "txtCustomer";
      this.txtCustomer.ShowCustomerNameWhenSet = true;
      this.txtCustomer.ShowTermedCheckBox = false;
      this.txtCustomer.Size = new System.Drawing.Size(321, 25);
      this.txtCustomer.TabIndex = 0;
      // 
      // dtBillDate
      // 
      this.dtBillDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtBillDate.Location = new System.Drawing.Point(90, 45);
      this.dtBillDate.Name = "dtBillDate";
      this.dtBillDate.Size = new System.Drawing.Size(100, 20);
      this.dtBillDate.TabIndex = 1;
      // 
      // txtID
      // 
      this.txtID.Location = new System.Drawing.Point(321, 46);
      this.txtID.Name = "txtID";
      this.txtID.Size = new System.Drawing.Size(63, 20);
      this.txtID.TabIndex = 2;
      this.txtID.Visible = false;
      // 
      // btnDelete
      // 
      this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnDelete.Location = new System.Drawing.Point(1150, 100);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(75, 23);
      this.btnDelete.TabIndex = 7;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      // 
      // btnSave
      // 
      this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSave.Location = new System.Drawing.Point(1069, 100);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(75, 23);
      this.btnSave.TabIndex = 6;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // btnNew
      // 
      this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnNew.Location = new System.Drawing.Point(988, 100);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new System.Drawing.Size(75, 23);
      this.btnNew.TabIndex = 5;
      this.btnNew.Text = "New";
      this.btnNew.UseVisualStyleBackColor = true;
      this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
      // 
      // txtDescription
      // 
      this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtDescription.Location = new System.Drawing.Point(499, 17);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new System.Drawing.Size(726, 78);
      this.txtDescription.TabIndex = 4;
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(427, 18);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(60, 13);
      this.label11.TabIndex = 17;
      this.label11.Text = "Description";
      // 
      // txtRetailAmount
      // 
      this.txtRetailAmount.Location = new System.Drawing.Point(90, 71);
      this.txtRetailAmount.Name = "txtRetailAmount";
      this.txtRetailAmount.Size = new System.Drawing.Size(321, 20);
      this.txtRetailAmount.TabIndex = 3;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(11, 75);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(73, 13);
      this.label8.TabIndex = 11;
      this.label8.Text = "Retail Amount";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(11, 51);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(46, 13);
      this.label6.TabIndex = 2;
      this.label6.Text = "Bill Date";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(11, 19);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(51, 13);
      this.label5.TabIndex = 0;
      this.label5.Text = "Customer";
      // 
      // ckAdjustmentsOnly
      // 
      this.ckAdjustmentsOnly.AutoSize = true;
      this.ckAdjustmentsOnly.Checked = true;
      this.ckAdjustmentsOnly.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckAdjustmentsOnly.Location = new System.Drawing.Point(837, 10);
      this.ckAdjustmentsOnly.Name = "ckAdjustmentsOnly";
      this.ckAdjustmentsOnly.Size = new System.Drawing.Size(137, 17);
      this.ckAdjustmentsOnly.TabIndex = 3;
      this.ckAdjustmentsOnly.Text = "Show Only Adjustments";
      this.ckAdjustmentsOnly.UseVisualStyleBackColor = true;
      this.ckAdjustmentsOnly.CheckedChanged += new System.EventHandler(this.ckAdjustmentsOnly_CheckedChanged);
      // 
      // btnLoad
      // 
      this.btnLoad.Location = new System.Drawing.Point(1164, 4);
      this.btnLoad.Name = "btnLoad";
      this.btnLoad.Size = new System.Drawing.Size(75, 23);
      this.btnLoad.TabIndex = 5;
      this.btnLoad.Text = "Load";
      this.btnLoad.UseVisualStyleBackColor = true;
      this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
      // 
      // ckAutoRefresh
      // 
      this.ckAutoRefresh.AutoSize = true;
      this.ckAutoRefresh.Location = new System.Drawing.Point(980, 11);
      this.ckAutoRefresh.Name = "ckAutoRefresh";
      this.ckAutoRefresh.Size = new System.Drawing.Size(88, 17);
      this.ckAutoRefresh.TabIndex = 4;
      this.ckAutoRefresh.Text = "Auto Refresh";
      this.ckAutoRefresh.UseVisualStyleBackColor = true;
      // 
      // frmCityHostedOCCMaintenance
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1240, 533);
      this.Controls.Add(this.ckAutoRefresh);
      this.Controls.Add(this.btnLoad);
      this.Controls.Add(this.ckAdjustmentsOnly);
      this.Controls.Add(this.srchBillDate);
      this.Controls.Add(this.srchCustomer);
      this.Controls.Add(this.srchDealer);
      this.Controls.Add(this.splitMain);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Name = "frmCityHostedOCCMaintenance";
      this.Text = "frmCityHostedOCCMaintenance";
      this.Load += new System.EventHandler(this.frmCityHostedOCCMaintenance_Load);
      this.splitMain.Panel1.ResumeLayout(false);
      this.splitMain.Panel2.ResumeLayout(false);
      this.splitMain.Panel2.PerformLayout();
      this.splitMain.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private ACG.CommonForms.ctlSearch srchDealer;
    private ACG.CommonForms.ctlSearch srchCustomer;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.DateTimePicker srchBillDate;
    private System.Windows.Forms.SplitContainer splitMain;
    private Common.ctlSearchGrid srchOCCAdjustments;
    private System.Windows.Forms.DateTimePicker dtBillDate;
    private ACG.CommonForms.ctlSearch txtCustomer;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox txtRetailAmount;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.TextBox txtID;
    private System.Windows.Forms.CheckBox ckAdjustmentsOnly;
    private System.Windows.Forms.Button btnLoad;
    private System.Windows.Forms.CheckBox ckAutoRefresh;
  }
}