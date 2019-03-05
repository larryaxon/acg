namespace CCI.DesktopClient.Screens
{
  partial class frmCashReceipts
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
      System.Windows.Forms.ToolStripButton tbPost;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCashReceipts));
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.tbOptions = new System.Windows.Forms.ToolStripDropDownButton();
      this.exportedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tbPosted = new System.Windows.Forms.ToolStripMenuItem();
      this.tbUnPosted = new System.Windows.Forms.ToolStripMenuItem();
      this.tbUnpaidOnly = new System.Windows.Forms.ToolStripMenuItem();
      this.activeInventoryOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.noCreditBalancesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.excludeWithInactiveInventoryAndZeroBalanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
      this.tbBillingPeriod = new System.Windows.Forms.ToolStripTextBox();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.tbLoad = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.tbQBExport = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
      this.tbAmountDue = new System.Windows.Forms.ToolStripTextBox();
      this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
      this.tbTotalPaid = new System.Windows.Forms.ToolStripTextBox();
      this.btnImportPayments = new System.Windows.Forms.ToolStripButton();
      this.btnUndoImport = new System.Windows.Forms.ToolStripButton();
      this.lblProcessing = new System.Windows.Forms.ToolStripLabel();
      this.splitMain = new System.Windows.Forms.SplitContainer();
      this.srchCashReceipts = new CCI.DesktopClient.Common.ctlSearchGrid();
      this.radioBill = new System.Windows.Forms.RadioButton();
      this.chkSvcActivation = new System.Windows.Forms.CheckBox();
      this.cboCustomer = new ACG.CommonForms.ctlSearch();
      this.srchCashDetail = new CCI.DesktopClient.Common.ctlSearchGrid();
      this.label8 = new System.Windows.Forms.Label();
      this.btnDelete = new System.Windows.Forms.Button();
      this.txtComment = new System.Windows.Forms.TextBox();
      this.txtAmount = new System.Windows.Forms.TextBox();
      this.btnNew = new System.Windows.Forms.Button();
      this.label4 = new System.Windows.Forms.Label();
      this.lblMessage = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.dtDateReceived = new System.Windows.Forms.DateTimePicker();
      this.radioCC = new System.Windows.Forms.RadioButton();
      this.lblCheckNumber = new System.Windows.Forms.Label();
      this.txtNonSBAmount = new System.Windows.Forms.TextBox();
      this.txtCheckNbr = new System.Windows.Forms.TextBox();
      this.radioCheck = new System.Windows.Forms.RadioButton();
      this.txtID = new System.Windows.Forms.TextBox();
      this.btnUpdate = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.lblQBExportMsg = new System.Windows.Forms.Label();
      this.radioACH = new System.Windows.Forms.RadioButton();
      this.btnCancel = new System.Windows.Forms.Button();
      tbPost = new System.Windows.Forms.ToolStripButton();
      this.toolStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
      this.splitMain.Panel1.SuspendLayout();
      this.splitMain.Panel2.SuspendLayout();
      this.splitMain.SuspendLayout();
      this.SuspendLayout();
      // 
      // tbPost
      // 
      tbPost.BackColor = System.Drawing.SystemColors.ControlLight;
      tbPost.ImageTransparentColor = System.Drawing.Color.Magenta;
      tbPost.Name = "tbPost";
      tbPost.Size = new System.Drawing.Size(83, 22);
      tbPost.Text = "Export to Sage";
      tbPost.Visible = false;
      tbPost.Click += new System.EventHandler(this.btnPost_Click);
      // 
      // toolStrip1
      // 
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbOptions,
            this.toolStripLabel1,
            this.tbBillingPeriod,
            this.toolStripSeparator1,
            this.tbLoad,
            this.toolStripSeparator2,
            tbPost,
            this.toolStripSeparator3,
            this.tbQBExport,
            this.toolStripSeparator4,
            this.toolStripLabel3,
            this.tbAmountDue,
            this.toolStripLabel4,
            this.tbTotalPaid,
            this.btnImportPayments,
            this.btnUndoImport,
            this.lblProcessing});
      this.toolStrip1.Location = new System.Drawing.Point(0, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(1140, 25);
      this.toolStrip1.TabIndex = 24;
      this.toolStrip1.Text = "toolStrip1";
      // 
      // tbOptions
      // 
      this.tbOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.tbOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportedToolStripMenuItem,
            this.tbUnpaidOnly,
            this.activeInventoryOnlyToolStripMenuItem,
            this.noCreditBalancesToolStripMenuItem,
            this.excludeWithInactiveInventoryAndZeroBalanceToolStripMenuItem});
      this.tbOptions.Image = ((System.Drawing.Image)(resources.GetObject("tbOptions.Image")));
      this.tbOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tbOptions.Name = "tbOptions";
      this.tbOptions.Size = new System.Drawing.Size(57, 22);
      this.tbOptions.Text = "Options";
      // 
      // exportedToolStripMenuItem
      // 
      this.exportedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbPosted,
            this.tbUnPosted});
      this.exportedToolStripMenuItem.Name = "exportedToolStripMenuItem";
      this.exportedToolStripMenuItem.Size = new System.Drawing.Size(313, 22);
      this.exportedToolStripMenuItem.Text = "Include Exported...";
      // 
      // tbPosted
      // 
      this.tbPosted.Checked = true;
      this.tbPosted.CheckOnClick = true;
      this.tbPosted.CheckState = System.Windows.Forms.CheckState.Checked;
      this.tbPosted.Name = "tbPosted";
      this.tbPosted.Size = new System.Drawing.Size(138, 22);
      this.tbPosted.Text = "Exported";
      // 
      // tbUnPosted
      // 
      this.tbUnPosted.Checked = true;
      this.tbUnPosted.CheckOnClick = true;
      this.tbUnPosted.CheckState = System.Windows.Forms.CheckState.Checked;
      this.tbUnPosted.Name = "tbUnPosted";
      this.tbUnPosted.Size = new System.Drawing.Size(138, 22);
      this.tbUnPosted.Text = "Not Exported";
      // 
      // tbUnpaidOnly
      // 
      this.tbUnpaidOnly.CheckOnClick = true;
      this.tbUnpaidOnly.Name = "tbUnpaidOnly";
      this.tbUnpaidOnly.Size = new System.Drawing.Size(313, 22);
      this.tbUnpaidOnly.Text = "Unpaid Only";
      // 
      // activeInventoryOnlyToolStripMenuItem
      // 
      this.activeInventoryOnlyToolStripMenuItem.CheckOnClick = true;
      this.activeInventoryOnlyToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.activeInventoryOnlyToolStripMenuItem.Name = "activeInventoryOnlyToolStripMenuItem";
      this.activeInventoryOnlyToolStripMenuItem.Size = new System.Drawing.Size(313, 22);
      this.activeInventoryOnlyToolStripMenuItem.Text = "Active Inventory Only";
      // 
      // noCreditBalancesToolStripMenuItem
      // 
      this.noCreditBalancesToolStripMenuItem.CheckOnClick = true;
      this.noCreditBalancesToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.noCreditBalancesToolStripMenuItem.Name = "noCreditBalancesToolStripMenuItem";
      this.noCreditBalancesToolStripMenuItem.Size = new System.Drawing.Size(313, 22);
      this.noCreditBalancesToolStripMenuItem.Text = "No Credit Balances";
      // 
      // excludeWithInactiveInventoryAndZeroBalanceToolStripMenuItem
      // 
      this.excludeWithInactiveInventoryAndZeroBalanceToolStripMenuItem.CheckOnClick = true;
      this.excludeWithInactiveInventoryAndZeroBalanceToolStripMenuItem.Name = "excludeWithInactiveInventoryAndZeroBalanceToolStripMenuItem";
      this.excludeWithInactiveInventoryAndZeroBalanceToolStripMenuItem.Size = new System.Drawing.Size(313, 22);
      this.excludeWithInactiveInventoryAndZeroBalanceToolStripMenuItem.Text = "Exclude with Inactive Inventory and Zero Balance";
      // 
      // toolStripLabel1
      // 
      this.toolStripLabel1.Name = "toolStripLabel1";
      this.toolStripLabel1.Size = new System.Drawing.Size(59, 22);
      this.toolStripLabel1.Text = "Billing Date";
      // 
      // tbBillingPeriod
      // 
      this.tbBillingPeriod.Name = "tbBillingPeriod";
      this.tbBillingPeriod.Size = new System.Drawing.Size(100, 25);
      this.tbBillingPeriod.Leave += new System.EventHandler(this.tbBillingPeriod_Leave);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // tbLoad
      // 
      this.tbLoad.BackColor = System.Drawing.SystemColors.ControlLight;
      this.tbLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tbLoad.Name = "tbLoad";
      this.tbLoad.Size = new System.Drawing.Size(34, 22);
      this.tbLoad.Text = "Load";
      this.tbLoad.Click += new System.EventHandler(this.btnLoad_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
      // 
      // tbQBExport
      // 
      this.tbQBExport.BackColor = System.Drawing.SystemColors.ControlLight;
      this.tbQBExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.tbQBExport.Image = ((System.Drawing.Image)(resources.GetObject("tbQBExport.Image")));
      this.tbQBExport.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tbQBExport.Name = "tbQBExport";
      this.tbQBExport.Size = new System.Drawing.Size(100, 22);
      this.tbQBExport.Text = "QuickBooks Export";
      this.tbQBExport.Visible = false;
      this.tbQBExport.Click += new System.EventHandler(this.btnQBExport_Click);
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripLabel3
      // 
      this.toolStripLabel3.Name = "toolStripLabel3";
      this.toolStripLabel3.Size = new System.Drawing.Size(48, 22);
      this.toolStripLabel3.Text = "Amt Due";
      // 
      // tbAmountDue
      // 
      this.tbAmountDue.Enabled = false;
      this.tbAmountDue.Name = "tbAmountDue";
      this.tbAmountDue.Size = new System.Drawing.Size(100, 25);
      // 
      // toolStripLabel4
      // 
      this.toolStripLabel4.Name = "toolStripLabel4";
      this.toolStripLabel4.Size = new System.Drawing.Size(54, 22);
      this.toolStripLabel4.Text = "Total Paid";
      // 
      // tbTotalPaid
      // 
      this.tbTotalPaid.Enabled = false;
      this.tbTotalPaid.Name = "tbTotalPaid";
      this.tbTotalPaid.Size = new System.Drawing.Size(100, 25);
      // 
      // btnImportPayments
      // 
      this.btnImportPayments.BackColor = System.Drawing.SystemColors.ControlLight;
      this.btnImportPayments.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.btnImportPayments.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnImportPayments.Name = "btnImportPayments";
      this.btnImportPayments.Size = new System.Drawing.Size(93, 22);
      this.btnImportPayments.Text = "Import Payments";
      this.btnImportPayments.Click += new System.EventHandler(this.btnImportPayments_Click);
      // 
      // btnUndoImport
      // 
      this.btnUndoImport.BackColor = System.Drawing.SystemColors.ControlLight;
      this.btnUndoImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.btnUndoImport.Image = ((System.Drawing.Image)(resources.GetObject("btnUndoImport.Image")));
      this.btnUndoImport.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.btnUndoImport.Name = "btnUndoImport";
      this.btnUndoImport.Size = new System.Drawing.Size(71, 22);
      this.btnUndoImport.Text = "Undo Import";
      this.btnUndoImport.Visible = false;
      this.btnUndoImport.Click += new System.EventHandler(this.btnUndoImport_Click);
      // 
      // lblProcessing
      // 
      this.lblProcessing.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
      this.lblProcessing.ForeColor = System.Drawing.Color.Red;
      this.lblProcessing.Name = "lblProcessing";
      this.lblProcessing.Size = new System.Drawing.Size(75, 22);
      this.lblProcessing.Text = "Processing...";
      this.lblProcessing.Visible = false;
      // 
      // splitMain
      // 
      this.splitMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.splitMain.Location = new System.Drawing.Point(1, 28);
      this.splitMain.Name = "splitMain";
      this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitMain.Panel1
      // 
      this.splitMain.Panel1.Controls.Add(this.srchCashReceipts);
      // 
      // splitMain.Panel2
      // 
      this.splitMain.Panel2.Controls.Add(this.radioBill);
      this.splitMain.Panel2.Controls.Add(this.chkSvcActivation);
      this.splitMain.Panel2.Controls.Add(this.cboCustomer);
      this.splitMain.Panel2.Controls.Add(this.srchCashDetail);
      this.splitMain.Panel2.Controls.Add(this.label8);
      this.splitMain.Panel2.Controls.Add(this.btnDelete);
      this.splitMain.Panel2.Controls.Add(this.txtComment);
      this.splitMain.Panel2.Controls.Add(this.txtAmount);
      this.splitMain.Panel2.Controls.Add(this.btnNew);
      this.splitMain.Panel2.Controls.Add(this.label4);
      this.splitMain.Panel2.Controls.Add(this.lblMessage);
      this.splitMain.Panel2.Controls.Add(this.label5);
      this.splitMain.Panel2.Controls.Add(this.label7);
      this.splitMain.Panel2.Controls.Add(this.dtDateReceived);
      this.splitMain.Panel2.Controls.Add(this.radioCC);
      this.splitMain.Panel2.Controls.Add(this.lblCheckNumber);
      this.splitMain.Panel2.Controls.Add(this.txtNonSBAmount);
      this.splitMain.Panel2.Controls.Add(this.txtCheckNbr);
      this.splitMain.Panel2.Controls.Add(this.radioCheck);
      this.splitMain.Panel2.Controls.Add(this.txtID);
      this.splitMain.Panel2.Controls.Add(this.btnUpdate);
      this.splitMain.Panel2.Controls.Add(this.label2);
      this.splitMain.Panel2.Controls.Add(this.lblQBExportMsg);
      this.splitMain.Panel2.Controls.Add(this.radioACH);
      this.splitMain.Panel2.Controls.Add(this.btnCancel);
      this.splitMain.Size = new System.Drawing.Size(1135, 489);
      this.splitMain.SplitterDistance = 305;
      this.splitMain.TabIndex = 4;
      // 
      // srchCashReceipts
      // 
      this.srchCashReceipts.AllowSortByColumn = true;
      this.srchCashReceipts.AutoRefreshWhenFieldChecked = false;
      this.srchCashReceipts.AutoSaveUserOptions = false;
      this.srchCashReceipts.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.srchCashReceipts.CanChangeDisplayFields = true;
      this.srchCashReceipts.CanChangeDisplaySearchCriteria = true;
      this.srchCashReceipts.ColumnName = "CustomerName";
      this.srchCashReceipts.DisplayFields = false;
      this.srchCashReceipts.DisplaySearchCriteria = false;
      this.srchCashReceipts.Dock = System.Windows.Forms.DockStyle.Fill;
      this.srchCashReceipts.FieldsDefaultIsChecked = true;
      this.srchCashReceipts.ForceReloadSearchColumns = false;
      this.srchCashReceipts.IDList = null;
      this.srchCashReceipts.IncludeGroupAsCriteria = false;
      this.srchCashReceipts.InnerWhere = "";
      this.srchCashReceipts.Location = new System.Drawing.Point(0, 0);
      this.srchCashReceipts.Name = "srchCashReceipts";
      this.srchCashReceipts.NameType = CCI.Common.CommonData.UnmatchedNameTypes.Customer;
      this.srchCashReceipts.SearchCriteria = null;
      this.srchCashReceipts.Size = new System.Drawing.Size(1135, 305);
      this.srchCashReceipts.TabIndex = 0;
      this.srchCashReceipts.Title = "Search (0 Records Found)";
      this.srchCashReceipts.UniqueIdentifier = "ID";
      this.srchCashReceipts.UseNamedSearches = false;
      this.srchCashReceipts.RowSelected += new ACG.CommonForms.ctlSearchGrid.RowSelectedHandler(this.srchCashReceipts_RowSelected);
      this.srchCashReceipts.RowChecked += new ACG.CommonForms.ctlSearchGrid.RowCheckedHandler(this.srchCashReceipts_RowChecked);
      // 
      // radioBill
      // 
      this.radioBill.AutoSize = true;
      this.radioBill.Location = new System.Drawing.Point(246, 5);
      this.radioBill.Name = "radioBill";
      this.radioBill.Size = new System.Drawing.Size(38, 17);
      this.radioBill.TabIndex = 25;
      this.radioBill.TabStop = true;
      this.radioBill.Text = "Bill";
      this.radioBill.UseVisualStyleBackColor = true;
      this.radioBill.Visible = false;
      this.radioBill.CheckedChanged += new System.EventHandler(this.radioBill_CheckedChanged);
      // 
      // chkSvcActivation
      // 
      this.chkSvcActivation.AutoSize = true;
      this.chkSvcActivation.Location = new System.Drawing.Point(909, 5);
      this.chkSvcActivation.Name = "chkSvcActivation";
      this.chkSvcActivation.Size = new System.Drawing.Size(101, 17);
      this.chkSvcActivation.TabIndex = 24;
      this.chkSvcActivation.Text = "Svc Activation?";
      this.chkSvcActivation.UseVisualStyleBackColor = true;
      // 
      // cboCustomer
      // 
      this.cboCustomer.AddNewMode = false;
      this.cboCustomer.AutoAddNewMode = false;
      this.cboCustomer.AutoSelectWhenMatch = false;
      this.cboCustomer.AutoTabToNextControlOnSelect = true;
      this.cboCustomer.ClearSearchOnExpand = false;
      this.cboCustomer.ClearSearchWhenComplete = false;
      this.cboCustomer.Collapsed = true;
      this.cboCustomer.CreatedNewItem = false;
      this.cboCustomer.DisplayOnlyDescription = false;
      this.cboCustomer.DisplayOnlyID = false;
      this.cboCustomer.FixKeySpace = "-1";
      this.cboCustomer.ID = "";
      this.cboCustomer.ID_DescSplitter = ":";
      this.cboCustomer.Location = new System.Drawing.Point(562, 2);
      this.cboCustomer.MaxHeight = 228;
      this.cboCustomer.MustExistInList = true;
      this.cboCustomer.MustExistMessage = "You must enter a valid Customer";
      this.cboCustomer.Name = "cboCustomer";
      this.cboCustomer.SearchExec = null;
      this.cboCustomer.ShowCustomerNameWhenSet = true;
      this.cboCustomer.ShowTermedCheckBox = false;
      this.cboCustomer.Size = new System.Drawing.Size(208, 22);
      this.cboCustomer.TabIndex = 6;
      this.cboCustomer.OnSelected += new System.EventHandler<System.EventArgs>(this.cboCustomer_OnSelected_1);
      // 
      // srchCashDetail
      // 
      this.srchCashDetail.AllowSortByColumn = true;
      this.srchCashDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.srchCashDetail.AutoRefreshWhenFieldChecked = false;
      this.srchCashDetail.AutoSaveUserOptions = false;
      this.srchCashDetail.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.srchCashDetail.CanChangeDisplayFields = false;
      this.srchCashDetail.CanChangeDisplaySearchCriteria = true;
      this.srchCashDetail.ColumnName = "CustomerName";
      this.srchCashDetail.DisplayFields = false;
      this.srchCashDetail.DisplaySearchCriteria = false;
      this.srchCashDetail.FieldsDefaultIsChecked = true;
      this.srchCashDetail.ForceReloadSearchColumns = false;
      this.srchCashDetail.IDList = null;
      this.srchCashDetail.IncludeGroupAsCriteria = false;
      this.srchCashDetail.InnerWhere = "";
      this.srchCashDetail.Location = new System.Drawing.Point(3, 74);
      this.srchCashDetail.Name = "srchCashDetail";
      this.srchCashDetail.NameType = CCI.Common.CommonData.UnmatchedNameTypes.CashReceiptsDetail;
      this.srchCashDetail.SearchCriteria = null;
      this.srchCashDetail.Size = new System.Drawing.Size(1135, 84);
      this.srchCashDetail.TabIndex = 18;
      this.srchCashDetail.Title = "Search (0 Records Found)";
      this.srchCashDetail.UniqueIdentifier = "ID";
      this.srchCashDetail.UseNamedSearches = false;
      this.srchCashDetail.RowSelected += new ACG.CommonForms.ctlSearchGrid.RowSelectedHandler(this.srchCashDetail_RowSelected);
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(434, 33);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(51, 13);
      this.label8.TabIndex = 19;
      this.label8.Text = "Comment";
      // 
      // btnDelete
      // 
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new System.Drawing.Point(84, 36);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(75, 23);
      this.btnDelete.TabIndex = 15;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      // 
      // txtComment
      // 
      this.txtComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtComment.Location = new System.Drawing.Point(496, 30);
      this.txtComment.Multiline = true;
      this.txtComment.Name = "txtComment";
      this.txtComment.Size = new System.Drawing.Size(590, 38);
      this.txtComment.TabIndex = 13;
      // 
      // txtAmount
      // 
      this.txtAmount.Location = new System.Drawing.Point(825, 5);
      this.txtAmount.Name = "txtAmount";
      this.txtAmount.Size = new System.Drawing.Size(67, 20);
      this.txtAmount.TabIndex = 8;
      this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.txtAmount.TextChanged += new System.EventHandler(this.txtAmount_TextChanged);
      // 
      // btnNew
      // 
      this.btnNew.Location = new System.Drawing.Point(246, 36);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new System.Drawing.Size(75, 23);
      this.btnNew.TabIndex = 17;
      this.btnNew.Text = "New";
      this.btnNew.UseVisualStyleBackColor = true;
      this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(329, 5);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(60, 13);
      this.label4.TabIndex = 3;
      this.label4.Text = "Trans Date";
      // 
      // lblMessage
      // 
      this.lblMessage.AutoSize = true;
      this.lblMessage.Location = new System.Drawing.Point(345, 41);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new System.Drawing.Size(0, 13);
      this.lblMessage.TabIndex = 14;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(776, 6);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(43, 13);
      this.label5.TabIndex = 7;
      this.label5.Text = "Amount";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(1031, 6);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(65, 13);
      this.label7.TabIndex = 9;
      this.label7.Text = "Non SB Amt";
      // 
      // dtDateReceived
      // 
      this.dtDateReceived.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtDateReceived.Location = new System.Drawing.Point(395, 2);
      this.dtDateReceived.Name = "dtDateReceived";
      this.dtDateReceived.ShowCheckBox = true;
      this.dtDateReceived.Size = new System.Drawing.Size(100, 20);
      this.dtDateReceived.TabIndex = 4;
      this.dtDateReceived.ValueChanged += new System.EventHandler(this.dtDateReceived_ValueChanged);
      // 
      // radioCC
      // 
      this.radioCC.AutoSize = true;
      this.radioCC.Location = new System.Drawing.Point(47, 5);
      this.radioCC.Name = "radioCC";
      this.radioCC.Size = new System.Drawing.Size(77, 17);
      this.radioCC.TabIndex = 0;
      this.radioCC.TabStop = true;
      this.radioCC.Text = "Credit Card";
      this.radioCC.UseVisualStyleBackColor = true;
      this.radioCC.CheckedChanged += new System.EventHandler(this.radioCC_CheckedChanged);
      this.radioCC.Click += new System.EventHandler(this.radioCash_Click);
      // 
      // lblCheckNumber
      // 
      this.lblCheckNumber.AutoSize = true;
      this.lblCheckNumber.Location = new System.Drawing.Point(1168, 6);
      this.lblCheckNumber.Name = "lblCheckNumber";
      this.lblCheckNumber.Size = new System.Drawing.Size(48, 13);
      this.lblCheckNumber.TabIndex = 11;
      this.lblCheckNumber.Text = "Check #";
      // 
      // txtNonSBAmount
      // 
      this.txtNonSBAmount.Location = new System.Drawing.Point(1102, 5);
      this.txtNonSBAmount.Name = "txtNonSBAmount";
      this.txtNonSBAmount.Size = new System.Drawing.Size(67, 20);
      this.txtNonSBAmount.TabIndex = 10;
      this.txtNonSBAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // txtCheckNbr
      // 
      this.txtCheckNbr.Location = new System.Drawing.Point(1222, 5);
      this.txtCheckNbr.Name = "txtCheckNbr";
      this.txtCheckNbr.Size = new System.Drawing.Size(79, 20);
      this.txtCheckNbr.TabIndex = 12;
      this.txtCheckNbr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.txtCheckNbr.TextChanged += new System.EventHandler(this.txtCheckNbr_TextChanged);
      // 
      // radioCheck
      // 
      this.radioCheck.AutoSize = true;
      this.radioCheck.Location = new System.Drawing.Point(130, 5);
      this.radioCheck.Name = "radioCheck";
      this.radioCheck.Size = new System.Drawing.Size(56, 17);
      this.radioCheck.TabIndex = 1;
      this.radioCheck.TabStop = true;
      this.radioCheck.Text = "Check";
      this.radioCheck.UseVisualStyleBackColor = true;
      this.radioCheck.CheckedChanged += new System.EventHandler(this.radioCheck_CheckedChanged);
      this.radioCheck.Click += new System.EventHandler(this.radioCheck_Click);
      // 
      // txtID
      // 
      this.txtID.Location = new System.Drawing.Point(3, 3);
      this.txtID.Name = "txtID";
      this.txtID.Size = new System.Drawing.Size(38, 20);
      this.txtID.TabIndex = 0;
      this.txtID.Visible = false;
      // 
      // btnUpdate
      // 
      this.btnUpdate.Enabled = false;
      this.btnUpdate.Location = new System.Drawing.Point(3, 36);
      this.btnUpdate.Name = "btnUpdate";
      this.btnUpdate.Size = new System.Drawing.Size(75, 23);
      this.btnUpdate.TabIndex = 14;
      this.btnUpdate.Text = "Update";
      this.btnUpdate.UseVisualStyleBackColor = true;
      this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(501, 6);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(51, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Customer";
      // 
      // lblQBExportMsg
      // 
      this.lblQBExportMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblQBExportMsg.AutoSize = true;
      this.lblQBExportMsg.Location = new System.Drawing.Point(305, 41);
      this.lblQBExportMsg.Name = "lblQBExportMsg";
      this.lblQBExportMsg.Size = new System.Drawing.Size(0, 13);
      this.lblQBExportMsg.TabIndex = 7;
      // 
      // radioACH
      // 
      this.radioACH.AutoSize = true;
      this.radioACH.Location = new System.Drawing.Point(192, 5);
      this.radioACH.Name = "radioACH";
      this.radioACH.Size = new System.Drawing.Size(47, 17);
      this.radioACH.TabIndex = 2;
      this.radioACH.TabStop = true;
      this.radioACH.Text = "ACH";
      this.radioACH.UseVisualStyleBackColor = true;
      this.radioACH.CheckedChanged += new System.EventHandler(this.radioACH_CheckedChanged);
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(165, 36);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 16;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // frmCashReceipts
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1140, 519);
      this.Controls.Add(this.toolStrip1);
      this.Controls.Add(this.splitMain);
      this.Name = "frmCashReceipts";
      this.Text = "frmCashReceipts";
      this.Load += new System.EventHandler(this.frmCashReceipts_Load);
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.splitMain.Panel1.ResumeLayout(false);
      this.splitMain.Panel2.ResumeLayout(false);
      this.splitMain.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
      this.splitMain.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitMain;
    private Common.ctlSearchGrid srchCashReceipts;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox txtAmount;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.DateTimePicker dtDateReceived;
    private System.Windows.Forms.Label lblCheckNumber;
    private System.Windows.Forms.TextBox txtCheckNbr;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnUpdate;
    private System.Windows.Forms.RadioButton radioCheck;
    private System.Windows.Forms.RadioButton radioCC;
    private System.Windows.Forms.Label lblMessage;
    private System.Windows.Forms.TextBox txtID;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Label lblQBExportMsg;
    private System.Windows.Forms.RadioButton radioACH;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox txtNonSBAmount;
    private Common.ctlSearchGrid srchCashDetail;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.TextBox txtComment;
    private ACG.CommonForms.ctlSearch cboCustomer;
    private System.Windows.Forms.CheckBox chkSvcActivation;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripDropDownButton tbOptions;
    private System.Windows.Forms.ToolStripMenuItem exportedToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem tbPosted;
    private System.Windows.Forms.ToolStripMenuItem tbUnPosted;
    private System.Windows.Forms.ToolStripMenuItem tbUnpaidOnly;
    private System.Windows.Forms.ToolStripMenuItem activeInventoryOnlyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem noCreditBalancesToolStripMenuItem;
    private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    private System.Windows.Forms.ToolStripTextBox tbBillingPeriod;
    private System.Windows.Forms.ToolStripButton tbLoad;
    private System.Windows.Forms.ToolStripButton tbQBExport;
    private System.Windows.Forms.ToolStripLabel toolStripLabel3;
    private System.Windows.Forms.ToolStripTextBox tbAmountDue;
    private System.Windows.Forms.ToolStripLabel toolStripLabel4;
    private System.Windows.Forms.ToolStripTextBox tbTotalPaid;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.RadioButton radioBill;
    private System.Windows.Forms.ToolStripButton btnImportPayments;
    private System.Windows.Forms.ToolStripButton btnUndoImport;
    private System.Windows.Forms.ToolStripLabel lblProcessing;
    private System.Windows.Forms.ToolStripMenuItem excludeWithInactiveInventoryAndZeroBalanceToolStripMenuItem;
  }
}