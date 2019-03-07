namespace CCI.DesktopClient.Screens
{
  partial class frmNetworkInventory
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
      this.ckActiveInventoryOnly = new System.Windows.Forms.CheckBox();
      this.ctlLocationSearch = new ACG.CommonForms.ctlSearch();
      this.ctlCustomerSearch = new ACG.CommonForms.ctlSearch();
      this.btnSearch = new System.Windows.Forms.Button();
      this.txtOrderSearch = new System.Windows.Forms.TextBox();
      this.label15 = new System.Windows.Forms.Label();
      this.label13 = new System.Windows.Forms.Label();
      this.label14 = new System.Windows.Forms.Label();
      this.splitMain = new System.Windows.Forms.SplitContainer();
      this.pnlNewNetworkInventory = new System.Windows.Forms.Panel();
      this.txtDay2YN = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.txtAgent = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.txtMasterCustomer = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.txtDealer = new System.Windows.Forms.TextBox();
      this.lblDealer = new System.Windows.Forms.Label();
      this.txtNewItemID = new ACG.CommonForms.ctlSearch();
      this.grpPhysInventory = new System.Windows.Forms.GroupBox();
      this.txtQEUsoc = new System.Windows.Forms.ComboBox();
      this.btnQENormalEntry = new System.Windows.Forms.Button();
      this.txtQENotes = new System.Windows.Forms.TextBox();
      this.lblQENotes = new System.Windows.Forms.Label();
      this.txtQEMacAddress = new System.Windows.Forms.TextBox();
      this.lblQEMacAddress = new System.Windows.Forms.Label();
      this.lblQEUsoc = new System.Windows.Forms.Label();
      this.btnQuickEntryPhysicalInventory = new System.Windows.Forms.Button();
      this.lblPhysicalInventoryID = new System.Windows.Forms.Label();
      this.btnNewPhysicalInventory = new System.Windows.Forms.Button();
      this.txtPhysicalInventoryID = new System.Windows.Forms.TextBox();
      this.btnDeletehysicalInventory = new System.Windows.Forms.Button();
      this.btnSavePhysicalInventory = new System.Windows.Forms.Button();
      this.txtPhysicalInventoryNotes = new System.Windows.Forms.TextBox();
      this.lblNotes = new System.Windows.Forms.Label();
      this.txtMacAddress = new System.Windows.Forms.TextBox();
      this.lblMacAddress = new System.Windows.Forms.Label();
      this.srchPhysicalInventory = new CCI.DesktopClient.Common.ctlSearchGrid();
      this.txtPrimaryCarrier = new ACG.CommonForms.ctlSearch();
      this.txtNewLocation = new ACG.CommonForms.ctlSearch();
      this.txtTransactionMRC = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.txtTransactionQuantity = new System.Windows.Forms.TextBox();
      this.dtTransactionDate = new ACG.CommonForms.ctlACGDate();
      this.label2 = new System.Windows.Forms.Label();
      this.dtNewExpDate = new ACG.CommonForms.ctlACGDate();
      this.dtNewEndDate = new ACG.CommonForms.ctlACGDate();
      this.dtNewStartDate = new ACG.CommonForms.ctlACGDate();
      this.dtNewDateInstalled = new ACG.CommonForms.ctlACGDate();
      this.txtNewCustomer = new ACG.CommonForms.ctlSearch();
      this.label36 = new System.Windows.Forms.Label();
      this.txtNewAccount = new System.Windows.Forms.TextBox();
      this.btnNewCancel = new System.Windows.Forms.Button();
      this.btnClone = new System.Windows.Forms.Button();
      this.label34 = new System.Windows.Forms.Label();
      this.txtQuantity = new System.Windows.Forms.TextBox();
      this.label31 = new System.Windows.Forms.Label();
      this.txtNewNRC = new System.Windows.Forms.TextBox();
      this.label30 = new System.Windows.Forms.Label();
      this.lblNewNew = new System.Windows.Forms.Label();
      this.btnNewDelete = new System.Windows.Forms.Button();
      this.label18 = new System.Windows.Forms.Label();
      this.txtNewNetInvID = new System.Windows.Forms.TextBox();
      this.btnNewNew = new System.Windows.Forms.Button();
      this.label19 = new System.Windows.Forms.Label();
      this.btnNewSave = new System.Windows.Forms.Button();
      this.label21 = new System.Windows.Forms.Label();
      this.label23 = new System.Windows.Forms.Label();
      this.label24 = new System.Windows.Forms.Label();
      this.label25 = new System.Windows.Forms.Label();
      this.txtNewMRC = new System.Windows.Forms.TextBox();
      this.label26 = new System.Windows.Forms.Label();
      this.txtNewOrderID = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.txtLastModifiedBy = new System.Windows.Forms.TextBox();
      this.srchNetworkInventory = new CCI.DesktopClient.Common.ctlSearchGrid();
      ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
      this.splitMain.Panel1.SuspendLayout();
      this.splitMain.Panel2.SuspendLayout();
      this.splitMain.SuspendLayout();
      this.pnlNewNetworkInventory.SuspendLayout();
      this.grpPhysInventory.SuspendLayout();
      this.SuspendLayout();
      // 
      // ckActiveInventoryOnly
      // 
      this.ckActiveInventoryOnly.AutoSize = true;
      this.ckActiveInventoryOnly.Location = new System.Drawing.Point(978, 8);
      this.ckActiveInventoryOnly.Name = "ckActiveInventoryOnly";
      this.ckActiveInventoryOnly.Size = new System.Drawing.Size(127, 17);
      this.ckActiveInventoryOnly.TabIndex = 4;
      this.ckActiveInventoryOnly.Text = "Active Inventory Only";
      this.ckActiveInventoryOnly.UseVisualStyleBackColor = true;
      this.ckActiveInventoryOnly.CheckedChanged += new System.EventHandler(this.ckActiveInventoryOnly_CheckedChanged);
      // 
      // ctlLocationSearch
      // 
      this.ctlLocationSearch.AddNewMode = false;
      this.ctlLocationSearch.AutoAddNewMode = false;
      this.ctlLocationSearch.AutoSelectWhenMatch = false;
      this.ctlLocationSearch.AutoTabToNextControlOnSelect = true;
      this.ctlLocationSearch.ClearSearchOnExpand = false;
      this.ctlLocationSearch.ClearSearchWhenComplete = false;
      this.ctlLocationSearch.Collapsed = true;
      this.ctlLocationSearch.CreatedNewItem = false;
      this.ctlLocationSearch.DisplayOnlyDescription = false;
      this.ctlLocationSearch.DisplayOnlyID = false;
      this.ctlLocationSearch.FixKeySpace = "-1";
      this.ctlLocationSearch.ID = "";
      this.ctlLocationSearch.ID_DescSplitter = ":";
      this.ctlLocationSearch.Location = new System.Drawing.Point(434, 6);
      this.ctlLocationSearch.MaxHeight = 228;
      this.ctlLocationSearch.MustExistInList = false;
      this.ctlLocationSearch.MustExistMessage = "You must enter a valid value";
      this.ctlLocationSearch.Name = "ctlLocationSearch";
      this.ctlLocationSearch.SearchExec = null;
      this.ctlLocationSearch.ShowCustomerNameWhenSet = true;
      this.ctlLocationSearch.ShowTermedCheckBox = false;
      this.ctlLocationSearch.Size = new System.Drawing.Size(295, 24);
      this.ctlLocationSearch.TabIndex = 1;
      this.ctlLocationSearch.OnSelected += new System.EventHandler<System.EventArgs>(this.ctlLocationSearch_Leave);
      this.ctlLocationSearch.Enter += new System.EventHandler(this.ctlLocationSearch_Enter);
      // 
      // ctlCustomerSearch
      // 
      this.ctlCustomerSearch.AddNewMode = false;
      this.ctlCustomerSearch.AutoAddNewMode = false;
      this.ctlCustomerSearch.AutoSelectWhenMatch = false;
      this.ctlCustomerSearch.AutoTabToNextControlOnSelect = true;
      this.ctlCustomerSearch.ClearSearchOnExpand = false;
      this.ctlCustomerSearch.ClearSearchWhenComplete = false;
      this.ctlCustomerSearch.Collapsed = true;
      this.ctlCustomerSearch.CreatedNewItem = false;
      this.ctlCustomerSearch.DisplayOnlyDescription = false;
      this.ctlCustomerSearch.DisplayOnlyID = false;
      this.ctlCustomerSearch.FixKeySpace = "-1";
      this.ctlCustomerSearch.ID = "";
      this.ctlCustomerSearch.ID_DescSplitter = ":";
      this.ctlCustomerSearch.Location = new System.Drawing.Point(76, 6);
      this.ctlCustomerSearch.MaxHeight = 228;
      this.ctlCustomerSearch.MustExistInList = false;
      this.ctlCustomerSearch.MustExistMessage = "You must enter a valid value";
      this.ctlCustomerSearch.Name = "ctlCustomerSearch";
      this.ctlCustomerSearch.SearchExec = null;
      this.ctlCustomerSearch.ShowCustomerNameWhenSet = true;
      this.ctlCustomerSearch.ShowTermedCheckBox = false;
      this.ctlCustomerSearch.Size = new System.Drawing.Size(295, 24);
      this.ctlCustomerSearch.TabIndex = 0;
      this.ctlCustomerSearch.OnSelected += new System.EventHandler<System.EventArgs>(this.txtCustomerSearch_Leave);
      this.ctlCustomerSearch.Enter += new System.EventHandler(this.ctlCustomerSearch_Enter);
      // 
      // btnSearch
      // 
      this.btnSearch.Location = new System.Drawing.Point(883, 4);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new System.Drawing.Size(75, 23);
      this.btnSearch.TabIndex = 3;
      this.btnSearch.Text = "Search";
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
      // 
      // txtOrderSearch
      // 
      this.txtOrderSearch.Location = new System.Drawing.Point(777, 6);
      this.txtOrderSearch.Name = "txtOrderSearch";
      this.txtOrderSearch.Size = new System.Drawing.Size(100, 20);
      this.txtOrderSearch.TabIndex = 2;
      this.txtOrderSearch.Leave += new System.EventHandler(this.txtOrderSearch_Leave);
      // 
      // label15
      // 
      this.label15.AutoSize = true;
      this.label15.Location = new System.Drawing.Point(738, 10);
      this.label15.Name = "label15";
      this.label15.Size = new System.Drawing.Size(33, 13);
      this.label15.TabIndex = 8;
      this.label15.Text = "Order";
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(377, 10);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(51, 13);
      this.label13.TabIndex = 7;
      this.label13.Text = "Location:";
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(16, 10);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(54, 13);
      this.label14.TabIndex = 6;
      this.label14.Text = "Customer:";
      // 
      // splitMain
      // 
      this.splitMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
      this.splitMain.Location = new System.Drawing.Point(12, 36);
      this.splitMain.Name = "splitMain";
      this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitMain.Panel1
      // 
      this.splitMain.Panel1.AutoScroll = true;
      this.splitMain.Panel1.Controls.Add(this.pnlNewNetworkInventory);
      // 
      // splitMain.Panel2
      // 
      this.splitMain.Panel2.Controls.Add(this.srchNetworkInventory);
      this.splitMain.Size = new System.Drawing.Size(1127, 638);
      this.splitMain.SplitterDistance = 228;
      this.splitMain.TabIndex = 2;
      // 
      // pnlNewNetworkInventory
      // 
      this.pnlNewNetworkInventory.Controls.Add(this.txtDay2YN);
      this.pnlNewNetworkInventory.Controls.Add(this.label7);
      this.pnlNewNetworkInventory.Controls.Add(this.txtAgent);
      this.pnlNewNetworkInventory.Controls.Add(this.label6);
      this.pnlNewNetworkInventory.Controls.Add(this.txtMasterCustomer);
      this.pnlNewNetworkInventory.Controls.Add(this.label5);
      this.pnlNewNetworkInventory.Controls.Add(this.txtDealer);
      this.pnlNewNetworkInventory.Controls.Add(this.lblDealer);
      this.pnlNewNetworkInventory.Controls.Add(this.txtNewItemID);
      this.pnlNewNetworkInventory.Controls.Add(this.grpPhysInventory);
      this.pnlNewNetworkInventory.Controls.Add(this.txtPrimaryCarrier);
      this.pnlNewNetworkInventory.Controls.Add(this.txtNewLocation);
      this.pnlNewNetworkInventory.Controls.Add(this.txtTransactionMRC);
      this.pnlNewNetworkInventory.Controls.Add(this.label4);
      this.pnlNewNetworkInventory.Controls.Add(this.label3);
      this.pnlNewNetworkInventory.Controls.Add(this.txtTransactionQuantity);
      this.pnlNewNetworkInventory.Controls.Add(this.dtTransactionDate);
      this.pnlNewNetworkInventory.Controls.Add(this.label2);
      this.pnlNewNetworkInventory.Controls.Add(this.dtNewExpDate);
      this.pnlNewNetworkInventory.Controls.Add(this.dtNewEndDate);
      this.pnlNewNetworkInventory.Controls.Add(this.dtNewStartDate);
      this.pnlNewNetworkInventory.Controls.Add(this.dtNewDateInstalled);
      this.pnlNewNetworkInventory.Controls.Add(this.txtNewCustomer);
      this.pnlNewNetworkInventory.Controls.Add(this.label36);
      this.pnlNewNetworkInventory.Controls.Add(this.txtNewAccount);
      this.pnlNewNetworkInventory.Controls.Add(this.btnNewCancel);
      this.pnlNewNetworkInventory.Controls.Add(this.btnClone);
      this.pnlNewNetworkInventory.Controls.Add(this.label34);
      this.pnlNewNetworkInventory.Controls.Add(this.txtQuantity);
      this.pnlNewNetworkInventory.Controls.Add(this.label31);
      this.pnlNewNetworkInventory.Controls.Add(this.txtNewNRC);
      this.pnlNewNetworkInventory.Controls.Add(this.label30);
      this.pnlNewNetworkInventory.Controls.Add(this.lblNewNew);
      this.pnlNewNetworkInventory.Controls.Add(this.btnNewDelete);
      this.pnlNewNetworkInventory.Controls.Add(this.label18);
      this.pnlNewNetworkInventory.Controls.Add(this.txtNewNetInvID);
      this.pnlNewNetworkInventory.Controls.Add(this.btnNewNew);
      this.pnlNewNetworkInventory.Controls.Add(this.label19);
      this.pnlNewNetworkInventory.Controls.Add(this.btnNewSave);
      this.pnlNewNetworkInventory.Controls.Add(this.label21);
      this.pnlNewNetworkInventory.Controls.Add(this.label23);
      this.pnlNewNetworkInventory.Controls.Add(this.label24);
      this.pnlNewNetworkInventory.Controls.Add(this.label25);
      this.pnlNewNetworkInventory.Controls.Add(this.txtNewMRC);
      this.pnlNewNetworkInventory.Controls.Add(this.label26);
      this.pnlNewNetworkInventory.Controls.Add(this.txtNewOrderID);
      this.pnlNewNetworkInventory.Controls.Add(this.label1);
      this.pnlNewNetworkInventory.Controls.Add(this.txtLastModifiedBy);
      this.pnlNewNetworkInventory.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlNewNetworkInventory.Location = new System.Drawing.Point(0, 0);
      this.pnlNewNetworkInventory.Name = "pnlNewNetworkInventory";
      this.pnlNewNetworkInventory.Size = new System.Drawing.Size(1127, 228);
      this.pnlNewNetworkInventory.TabIndex = 36;
      // 
      // txtDay2YN
      // 
      this.txtDay2YN.Enabled = false;
      this.txtDay2YN.Location = new System.Drawing.Point(107, 190);
      this.txtDay2YN.Name = "txtDay2YN";
      this.txtDay2YN.Size = new System.Drawing.Size(100, 20);
      this.txtDay2YN.TabIndex = 87;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(8, 193);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(35, 13);
      this.label7.TabIndex = 86;
      this.label7.Text = "Day 2";
      // 
      // txtAgent
      // 
      this.txtAgent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtAgent.Enabled = false;
      this.txtAgent.Location = new System.Drawing.Point(107, 167);
      this.txtAgent.Name = "txtAgent";
      this.txtAgent.Size = new System.Drawing.Size(712, 20);
      this.txtAgent.TabIndex = 85;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(8, 170);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(35, 13);
      this.label6.TabIndex = 84;
      this.label6.Text = "Agent";
      // 
      // txtMasterCustomer
      // 
      this.txtMasterCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtMasterCustomer.Enabled = false;
      this.txtMasterCustomer.Location = new System.Drawing.Point(108, 142);
      this.txtMasterCustomer.Name = "txtMasterCustomer";
      this.txtMasterCustomer.Size = new System.Drawing.Size(711, 20);
      this.txtMasterCustomer.TabIndex = 83;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(8, 145);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(86, 13);
      this.label5.TabIndex = 82;
      this.label5.Text = "Master Customer";
      // 
      // txtDealer
      // 
      this.txtDealer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtDealer.Enabled = false;
      this.txtDealer.Location = new System.Drawing.Point(603, 83);
      this.txtDealer.Name = "txtDealer";
      this.txtDealer.Size = new System.Drawing.Size(217, 20);
      this.txtDealer.TabIndex = 81;
      // 
      // lblDealer
      // 
      this.lblDealer.AutoSize = true;
      this.lblDealer.Location = new System.Drawing.Point(559, 86);
      this.lblDealer.Name = "lblDealer";
      this.lblDealer.Size = new System.Drawing.Size(38, 13);
      this.lblDealer.TabIndex = 80;
      this.lblDealer.Text = "Dealer";
      // 
      // txtNewItemID
      // 
      this.txtNewItemID.AddNewMode = false;
      this.txtNewItemID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtNewItemID.AutoAddNewMode = false;
      this.txtNewItemID.AutoSelectWhenMatch = false;
      this.txtNewItemID.AutoTabToNextControlOnSelect = true;
      this.txtNewItemID.ClearSearchOnExpand = false;
      this.txtNewItemID.ClearSearchWhenComplete = false;
      this.txtNewItemID.Collapsed = true;
      this.txtNewItemID.CreatedNewItem = false;
      this.txtNewItemID.DisplayOnlyDescription = false;
      this.txtNewItemID.DisplayOnlyID = false;
      this.txtNewItemID.FixKeySpace = "-1";
      this.txtNewItemID.ID = "";
      this.txtNewItemID.ID_DescSplitter = ":";
      this.txtNewItemID.Location = new System.Drawing.Point(368, 57);
      this.txtNewItemID.MaxHeight = 228;
      this.txtNewItemID.MustExistInList = false;
      this.txtNewItemID.MustExistMessage = "You must enter a valid value";
      this.txtNewItemID.Name = "txtNewItemID";
      this.txtNewItemID.SearchExec = null;
      this.txtNewItemID.ShowCustomerNameWhenSet = true;
      this.txtNewItemID.ShowTermedCheckBox = false;
      this.txtNewItemID.Size = new System.Drawing.Size(458, 24);
      this.txtNewItemID.TabIndex = 4;
      // 
      // grpPhysInventory
      // 
      this.grpPhysInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grpPhysInventory.Controls.Add(this.txtQEUsoc);
      this.grpPhysInventory.Controls.Add(this.btnQENormalEntry);
      this.grpPhysInventory.Controls.Add(this.txtQENotes);
      this.grpPhysInventory.Controls.Add(this.lblQENotes);
      this.grpPhysInventory.Controls.Add(this.txtQEMacAddress);
      this.grpPhysInventory.Controls.Add(this.lblQEMacAddress);
      this.grpPhysInventory.Controls.Add(this.lblQEUsoc);
      this.grpPhysInventory.Controls.Add(this.btnQuickEntryPhysicalInventory);
      this.grpPhysInventory.Controls.Add(this.lblPhysicalInventoryID);
      this.grpPhysInventory.Controls.Add(this.btnNewPhysicalInventory);
      this.grpPhysInventory.Controls.Add(this.txtPhysicalInventoryID);
      this.grpPhysInventory.Controls.Add(this.btnDeletehysicalInventory);
      this.grpPhysInventory.Controls.Add(this.btnSavePhysicalInventory);
      this.grpPhysInventory.Controls.Add(this.txtPhysicalInventoryNotes);
      this.grpPhysInventory.Controls.Add(this.lblNotes);
      this.grpPhysInventory.Controls.Add(this.txtMacAddress);
      this.grpPhysInventory.Controls.Add(this.lblMacAddress);
      this.grpPhysInventory.Controls.Add(this.srchPhysicalInventory);
      this.grpPhysInventory.Location = new System.Drawing.Point(5, 143);
      this.grpPhysInventory.Name = "grpPhysInventory";
      this.grpPhysInventory.Size = new System.Drawing.Size(824, 0);
      this.grpPhysInventory.TabIndex = 78;
      this.grpPhysInventory.TabStop = false;
      this.grpPhysInventory.Text = "Physical Inventory";
      this.grpPhysInventory.Visible = false;
      // 
      // txtQEUsoc
      // 
      this.txtQEUsoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtQEUsoc.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.txtQEUsoc.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
      this.txtQEUsoc.FormattingEnabled = true;
      this.txtQEUsoc.Location = new System.Drawing.Point(50, -7);
      this.txtQEUsoc.Name = "txtQEUsoc";
      this.txtQEUsoc.Size = new System.Drawing.Size(88, 21);
      this.txtQEUsoc.TabIndex = 0;
      // 
      // btnQENormalEntry
      // 
      this.btnQENormalEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnQENormalEntry.Location = new System.Drawing.Point(6, -35);
      this.btnQENormalEntry.Name = "btnQENormalEntry";
      this.btnQENormalEntry.Size = new System.Drawing.Size(78, 26);
      this.btnQENormalEntry.TabIndex = 94;
      this.btnQENormalEntry.Text = "Normal Entry";
      this.btnQENormalEntry.UseVisualStyleBackColor = true;
      this.btnQENormalEntry.Visible = false;
      this.btnQENormalEntry.Click += new System.EventHandler(this.btnQENormalEntry_Click);
      // 
      // txtQENotes
      // 
      this.txtQENotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtQENotes.Location = new System.Drawing.Point(416, -9);
      this.txtQENotes.Name = "txtQENotes";
      this.txtQENotes.Size = new System.Drawing.Size(399, 20);
      this.txtQENotes.TabIndex = 2;
      this.txtQENotes.Visible = false;
      this.txtQENotes.Leave += new System.EventHandler(this.txtQENotes_Leave);
      // 
      // lblQENotes
      // 
      this.lblQENotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblQENotes.AutoSize = true;
      this.lblQENotes.Location = new System.Drawing.Point(368, -4);
      this.lblQENotes.Name = "lblQENotes";
      this.lblQENotes.Size = new System.Drawing.Size(35, 13);
      this.lblQENotes.TabIndex = 92;
      this.lblQENotes.Text = "Notes";
      this.lblQENotes.Visible = false;
      // 
      // txtQEMacAddress
      // 
      this.txtQEMacAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtQEMacAddress.Location = new System.Drawing.Point(219, -6);
      this.txtQEMacAddress.Name = "txtQEMacAddress";
      this.txtQEMacAddress.Size = new System.Drawing.Size(145, 20);
      this.txtQEMacAddress.TabIndex = 1;
      this.txtQEMacAddress.Visible = false;
      // 
      // lblQEMacAddress
      // 
      this.lblQEMacAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblQEMacAddress.AutoSize = true;
      this.lblQEMacAddress.Location = new System.Drawing.Point(144, -3);
      this.lblQEMacAddress.Name = "lblQEMacAddress";
      this.lblQEMacAddress.Size = new System.Drawing.Size(69, 13);
      this.lblQEMacAddress.TabIndex = 90;
      this.lblQEMacAddress.Text = "Mac Address";
      this.lblQEMacAddress.Visible = false;
      // 
      // lblQEUsoc
      // 
      this.lblQEUsoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblQEUsoc.AutoSize = true;
      this.lblQEUsoc.Location = new System.Drawing.Point(7, -2);
      this.lblQEUsoc.Name = "lblQEUsoc";
      this.lblQEUsoc.Size = new System.Drawing.Size(37, 13);
      this.lblQEUsoc.TabIndex = 88;
      this.lblQEUsoc.Text = "USOC";
      this.lblQEUsoc.Visible = false;
      // 
      // btnQuickEntryPhysicalInventory
      // 
      this.btnQuickEntryPhysicalInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnQuickEntryPhysicalInventory.Location = new System.Drawing.Point(508, -35);
      this.btnQuickEntryPhysicalInventory.Name = "btnQuickEntryPhysicalInventory";
      this.btnQuickEntryPhysicalInventory.Size = new System.Drawing.Size(78, 26);
      this.btnQuickEntryPhysicalInventory.TabIndex = 87;
      this.btnQuickEntryPhysicalInventory.Text = "Quick Entry";
      this.btnQuickEntryPhysicalInventory.UseVisualStyleBackColor = true;
      this.btnQuickEntryPhysicalInventory.Click += new System.EventHandler(this.btnQuickEntryPhysicalInventory_Click);
      // 
      // lblPhysicalInventoryID
      // 
      this.lblPhysicalInventoryID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.lblPhysicalInventoryID.AutoSize = true;
      this.lblPhysicalInventoryID.Location = new System.Drawing.Point(733, -29);
      this.lblPhysicalInventoryID.Name = "lblPhysicalInventoryID";
      this.lblPhysicalInventoryID.Size = new System.Drawing.Size(21, 13);
      this.lblPhysicalInventoryID.TabIndex = 86;
      this.lblPhysicalInventoryID.Text = "ID:";
      // 
      // btnNewPhysicalInventory
      // 
      this.btnNewPhysicalInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnNewPhysicalInventory.Location = new System.Drawing.Point(256, -35);
      this.btnNewPhysicalInventory.Name = "btnNewPhysicalInventory";
      this.btnNewPhysicalInventory.Size = new System.Drawing.Size(78, 26);
      this.btnNewPhysicalInventory.TabIndex = 85;
      this.btnNewPhysicalInventory.Text = "New";
      this.btnNewPhysicalInventory.UseVisualStyleBackColor = true;
      this.btnNewPhysicalInventory.Click += new System.EventHandler(this.btnNewPhysicalInventory_Click);
      // 
      // txtPhysicalInventoryID
      // 
      this.txtPhysicalInventoryID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.txtPhysicalInventoryID.Enabled = false;
      this.txtPhysicalInventoryID.Location = new System.Drawing.Point(760, -31);
      this.txtPhysicalInventoryID.Name = "txtPhysicalInventoryID";
      this.txtPhysicalInventoryID.Size = new System.Drawing.Size(55, 20);
      this.txtPhysicalInventoryID.TabIndex = 84;
      this.txtPhysicalInventoryID.WordWrap = false;
      // 
      // btnDeletehysicalInventory
      // 
      this.btnDeletehysicalInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnDeletehysicalInventory.Location = new System.Drawing.Point(424, -35);
      this.btnDeletehysicalInventory.Name = "btnDeletehysicalInventory";
      this.btnDeletehysicalInventory.Size = new System.Drawing.Size(78, 26);
      this.btnDeletehysicalInventory.TabIndex = 83;
      this.btnDeletehysicalInventory.Text = "Delete";
      this.btnDeletehysicalInventory.UseVisualStyleBackColor = true;
      // 
      // btnSavePhysicalInventory
      // 
      this.btnSavePhysicalInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnSavePhysicalInventory.Location = new System.Drawing.Point(340, -35);
      this.btnSavePhysicalInventory.Name = "btnSavePhysicalInventory";
      this.btnSavePhysicalInventory.Size = new System.Drawing.Size(78, 26);
      this.btnSavePhysicalInventory.TabIndex = 82;
      this.btnSavePhysicalInventory.Text = "Save";
      this.btnSavePhysicalInventory.UseVisualStyleBackColor = true;
      this.btnSavePhysicalInventory.Click += new System.EventHandler(this.btnSavePhysicalInventory_Click);
      // 
      // txtPhysicalInventoryNotes
      // 
      this.txtPhysicalInventoryNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtPhysicalInventoryNotes.Location = new System.Drawing.Point(76, -6);
      this.txtPhysicalInventoryNotes.Name = "txtPhysicalInventoryNotes";
      this.txtPhysicalInventoryNotes.Size = new System.Drawing.Size(742, 20);
      this.txtPhysicalInventoryNotes.TabIndex = 81;
      // 
      // lblNotes
      // 
      this.lblNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblNotes.AutoSize = true;
      this.lblNotes.Location = new System.Drawing.Point(35, -28);
      this.lblNotes.Name = "lblNotes";
      this.lblNotes.Size = new System.Drawing.Size(35, 13);
      this.lblNotes.TabIndex = 80;
      this.lblNotes.Text = "Notes";
      // 
      // txtMacAddress
      // 
      this.txtMacAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtMacAddress.Location = new System.Drawing.Point(76, -32);
      this.txtMacAddress.Name = "txtMacAddress";
      this.txtMacAddress.Size = new System.Drawing.Size(167, 20);
      this.txtMacAddress.TabIndex = 79;
      // 
      // lblMacAddress
      // 
      this.lblMacAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblMacAddress.AutoSize = true;
      this.lblMacAddress.Location = new System.Drawing.Point(1, -2);
      this.lblMacAddress.Name = "lblMacAddress";
      this.lblMacAddress.Size = new System.Drawing.Size(69, 13);
      this.lblMacAddress.TabIndex = 78;
      this.lblMacAddress.Text = "Mac Address";
      // 
      // srchPhysicalInventory
      // 
      this.srchPhysicalInventory.AllowSortByColumn = true;
      this.srchPhysicalInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.srchPhysicalInventory.AutoRefreshWhenFieldChecked = false;
      this.srchPhysicalInventory.AutoSaveUserOptions = false;
      this.srchPhysicalInventory.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.srchPhysicalInventory.CanChangeDisplayFields = true;
      this.srchPhysicalInventory.CanChangeDisplaySearchCriteria = true;
      this.srchPhysicalInventory.ColumnName = "";
      this.srchPhysicalInventory.DisplayFields = false;
      this.srchPhysicalInventory.DisplaySearchCriteria = false;
      this.srchPhysicalInventory.FieldsDefaultIsChecked = true;
      this.srchPhysicalInventory.ForceReloadSearchColumns = false;
      this.srchPhysicalInventory.IDList = null;
      this.srchPhysicalInventory.IncludeGroupAsCriteria = false;
      this.srchPhysicalInventory.InnerWhere = "";
      this.srchPhysicalInventory.Location = new System.Drawing.Point(6, 19);
      this.srchPhysicalInventory.Name = "srchPhysicalInventory";
      this.srchPhysicalInventory.NameType = CCI.Common.CommonData.UnmatchedNameTypes.None;
      this.srchPhysicalInventory.SearchCriteria = null;
      this.srchPhysicalInventory.Size = new System.Drawing.Size(809, 0);
      this.srchPhysicalInventory.TabIndex = 77;
      this.srchPhysicalInventory.Title = "Search (0 Records Found)";
      this.srchPhysicalInventory.UniqueIdentifier = "ID";
      this.srchPhysicalInventory.UseNamedSearches = false;
      this.srchPhysicalInventory.RowSelected += new ACG.CommonForms.ctlSearchGrid.RowSelectedHandler(this.srchPhysicalInventory_RowSelected);
      // 
      // txtPrimaryCarrier
      // 
      this.txtPrimaryCarrier.AddNewMode = false;
      this.txtPrimaryCarrier.AutoAddNewMode = false;
      this.txtPrimaryCarrier.AutoSelectWhenMatch = false;
      this.txtPrimaryCarrier.AutoTabToNextControlOnSelect = true;
      this.txtPrimaryCarrier.ClearSearchOnExpand = false;
      this.txtPrimaryCarrier.ClearSearchWhenComplete = false;
      this.txtPrimaryCarrier.Collapsed = true;
      this.txtPrimaryCarrier.CreatedNewItem = false;
      this.txtPrimaryCarrier.DisplayOnlyDescription = false;
      this.txtPrimaryCarrier.DisplayOnlyID = false;
      this.txtPrimaryCarrier.FixKeySpace = "-1";
      this.txtPrimaryCarrier.ID = "";
      this.txtPrimaryCarrier.ID_DescSplitter = ":";
      this.txtPrimaryCarrier.Location = new System.Drawing.Point(107, 55);
      this.txtPrimaryCarrier.MaxHeight = 228;
      this.txtPrimaryCarrier.MustExistInList = false;
      this.txtPrimaryCarrier.MustExistMessage = "You must enter a valid value";
      this.txtPrimaryCarrier.Name = "txtPrimaryCarrier";
      this.txtPrimaryCarrier.SearchExec = null;
      this.txtPrimaryCarrier.ShowCustomerNameWhenSet = true;
      this.txtPrimaryCarrier.ShowTermedCheckBox = false;
      this.txtPrimaryCarrier.Size = new System.Drawing.Size(262, 20);
      this.txtPrimaryCarrier.TabIndex = 3;
      this.txtPrimaryCarrier.Leave += new System.EventHandler(this.txtPrimaryCarrier_Leave);
      // 
      // txtNewLocation
      // 
      this.txtNewLocation.AddNewMode = false;
      this.txtNewLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtNewLocation.AutoAddNewMode = false;
      this.txtNewLocation.AutoSelectWhenMatch = false;
      this.txtNewLocation.AutoTabToNextControlOnSelect = true;
      this.txtNewLocation.ClearSearchOnExpand = false;
      this.txtNewLocation.ClearSearchWhenComplete = false;
      this.txtNewLocation.Collapsed = true;
      this.txtNewLocation.CreatedNewItem = false;
      this.txtNewLocation.DisplayOnlyDescription = false;
      this.txtNewLocation.DisplayOnlyID = false;
      this.txtNewLocation.FixKeySpace = "-1";
      this.txtNewLocation.ID = "";
      this.txtNewLocation.ID_DescSplitter = ":";
      this.txtNewLocation.Location = new System.Drawing.Point(298, 31);
      this.txtNewLocation.MaxHeight = 228;
      this.txtNewLocation.MustExistInList = false;
      this.txtNewLocation.MustExistMessage = "You must enter a valid value";
      this.txtNewLocation.Name = "txtNewLocation";
      this.txtNewLocation.SearchExec = null;
      this.txtNewLocation.ShowCustomerNameWhenSet = true;
      this.txtNewLocation.ShowTermedCheckBox = false;
      this.txtNewLocation.Size = new System.Drawing.Size(823, 18);
      this.txtNewLocation.TabIndex = 2;
      // 
      // txtTransactionMRC
      // 
      this.txtTransactionMRC.Location = new System.Drawing.Point(345, 112);
      this.txtTransactionMRC.Name = "txtTransactionMRC";
      this.txtTransactionMRC.Size = new System.Drawing.Size(43, 20);
      this.txtTransactionMRC.TabIndex = 9;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(221, 116);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(118, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "Trans MRC/MRC/NRC";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(156, 116);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(12, 13);
      this.label3.TabIndex = 76;
      this.label3.Text = "/";
      // 
      // txtTransactionQuantity
      // 
      this.txtTransactionQuantity.Location = new System.Drawing.Point(110, 112);
      this.txtTransactionQuantity.Name = "txtTransactionQuantity";
      this.txtTransactionQuantity.Size = new System.Drawing.Size(40, 20);
      this.txtTransactionQuantity.TabIndex = 7;
      // 
      // dtTransactionDate
      // 
      this.dtTransactionDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.dtTransactionDate.Format = "d";
      this.dtTransactionDate.Location = new System.Drawing.Point(944, 78);
      this.dtTransactionDate.Name = "dtTransactionDate";
      this.dtTransactionDate.Size = new System.Drawing.Size(177, 20);
      this.dtTransactionDate.TabIndex = 14;
      this.dtTransactionDate.Value = null;
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(834, 82);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(89, 13);
      this.label2.TabIndex = 74;
      this.label2.Text = "Transaction Date";
      // 
      // dtNewExpDate
      // 
      this.dtNewExpDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.dtNewExpDate.Format = "d";
      this.dtNewExpDate.Location = new System.Drawing.Point(944, 147);
      this.dtNewExpDate.Name = "dtNewExpDate";
      this.dtNewExpDate.Size = new System.Drawing.Size(177, 20);
      this.dtNewExpDate.TabIndex = 17;
      this.dtNewExpDate.Value = null;
      // 
      // dtNewEndDate
      // 
      this.dtNewEndDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.dtNewEndDate.Format = "d";
      this.dtNewEndDate.Location = new System.Drawing.Point(944, 124);
      this.dtNewEndDate.Name = "dtNewEndDate";
      this.dtNewEndDate.Size = new System.Drawing.Size(177, 20);
      this.dtNewEndDate.TabIndex = 16;
      this.dtNewEndDate.Value = null;
      // 
      // dtNewStartDate
      // 
      this.dtNewStartDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.dtNewStartDate.Format = "d";
      this.dtNewStartDate.Location = new System.Drawing.Point(944, 101);
      this.dtNewStartDate.Name = "dtNewStartDate";
      this.dtNewStartDate.Size = new System.Drawing.Size(177, 20);
      this.dtNewStartDate.TabIndex = 15;
      this.dtNewStartDate.Value = null;
      // 
      // dtNewDateInstalled
      // 
      this.dtNewDateInstalled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.dtNewDateInstalled.Format = "d";
      this.dtNewDateInstalled.Location = new System.Drawing.Point(944, 55);
      this.dtNewDateInstalled.Name = "dtNewDateInstalled";
      this.dtNewDateInstalled.Size = new System.Drawing.Size(177, 20);
      this.dtNewDateInstalled.TabIndex = 13;
      this.dtNewDateInstalled.Value = null;
      // 
      // txtNewCustomer
      // 
      this.txtNewCustomer.AddNewMode = false;
      this.txtNewCustomer.AutoAddNewMode = false;
      this.txtNewCustomer.AutoSelectWhenMatch = false;
      this.txtNewCustomer.AutoTabToNextControlOnSelect = true;
      this.txtNewCustomer.ClearSearchOnExpand = false;
      this.txtNewCustomer.ClearSearchWhenComplete = false;
      this.txtNewCustomer.Collapsed = true;
      this.txtNewCustomer.CreatedNewItem = false;
      this.txtNewCustomer.DisplayOnlyDescription = false;
      this.txtNewCustomer.DisplayOnlyID = false;
      this.txtNewCustomer.FixKeySpace = "-1";
      this.txtNewCustomer.ID = "";
      this.txtNewCustomer.ID_DescSplitter = ":";
      this.txtNewCustomer.Location = new System.Drawing.Point(107, 31);
      this.txtNewCustomer.MaxHeight = 228;
      this.txtNewCustomer.MustExistInList = false;
      this.txtNewCustomer.MustExistMessage = "You must enter a valid value";
      this.txtNewCustomer.Name = "txtNewCustomer";
      this.txtNewCustomer.SearchExec = null;
      this.txtNewCustomer.ShowCustomerNameWhenSet = true;
      this.txtNewCustomer.ShowTermedCheckBox = false;
      this.txtNewCustomer.Size = new System.Drawing.Size(191, 18);
      this.txtNewCustomer.TabIndex = 1;
      this.txtNewCustomer.Leave += new System.EventHandler(this.txtNewCustomer_Leave);
      // 
      // label36
      // 
      this.label36.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label36.AutoSize = true;
      this.label36.Location = new System.Drawing.Point(834, 151);
      this.label36.Name = "label36";
      this.label36.Size = new System.Drawing.Size(94, 13);
      this.label36.TabIndex = 72;
      this.label36.Text = "Contract Exp Date";
      // 
      // txtNewAccount
      // 
      this.txtNewAccount.Enabled = false;
      this.txtNewAccount.Location = new System.Drawing.Point(110, 83);
      this.txtNewAccount.Name = "txtNewAccount";
      this.txtNewAccount.Size = new System.Drawing.Size(443, 20);
      this.txtNewAccount.TabIndex = 5;
      // 
      // btnNewCancel
      // 
      this.btnNewCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnNewCancel.Location = new System.Drawing.Point(907, 172);
      this.btnNewCancel.Name = "btnNewCancel";
      this.btnNewCancel.Size = new System.Drawing.Size(65, 23);
      this.btnNewCancel.TabIndex = 19;
      this.btnNewCancel.Text = "Cancel";
      this.btnNewCancel.UseVisualStyleBackColor = true;
      this.btnNewCancel.Click += new System.EventHandler(this.btnNewCancel_Click);
      // 
      // btnClone
      // 
      this.btnClone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClone.Location = new System.Drawing.Point(981, 173);
      this.btnClone.Name = "btnClone";
      this.btnClone.Size = new System.Drawing.Size(65, 20);
      this.btnClone.TabIndex = 20;
      this.btnClone.Text = "Clone";
      this.btnClone.UseVisualStyleBackColor = true;
      this.btnClone.Click += new System.EventHandler(this.btnClone_Click);
      // 
      // label34
      // 
      this.label34.AutoSize = true;
      this.label34.Location = new System.Drawing.Point(394, 116);
      this.label34.Name = "label34";
      this.label34.Size = new System.Drawing.Size(12, 13);
      this.label34.TabIndex = 6;
      this.label34.Text = "/";
      // 
      // txtQuantity
      // 
      this.txtQuantity.Location = new System.Drawing.Point(174, 112);
      this.txtQuantity.Name = "txtQuantity";
      this.txtQuantity.Size = new System.Drawing.Size(40, 20);
      this.txtQuantity.TabIndex = 5;
      // 
      // label31
      // 
      this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label31.AutoSize = true;
      this.label31.Location = new System.Drawing.Point(834, 105);
      this.label31.Name = "label31";
      this.label31.Size = new System.Drawing.Size(98, 13);
      this.label31.TabIndex = 65;
      this.label31.Text = "Contract Start Date";
      // 
      // txtNewNRC
      // 
      this.txtNewNRC.Location = new System.Drawing.Point(479, 112);
      this.txtNewNRC.Name = "txtNewNRC";
      this.txtNewNRC.Size = new System.Drawing.Size(57, 20);
      this.txtNewNRC.TabIndex = 11;
      // 
      // label30
      // 
      this.label30.AutoSize = true;
      this.label30.Location = new System.Drawing.Point(461, 116);
      this.label30.Name = "label30";
      this.label30.Size = new System.Drawing.Size(12, 13);
      this.label30.TabIndex = 8;
      this.label30.Text = "/";
      // 
      // lblNewNew
      // 
      this.lblNewNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblNewNew.AutoSize = true;
      this.lblNewNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblNewNew.ForeColor = System.Drawing.Color.Red;
      this.lblNewNew.Location = new System.Drawing.Point(1049, 10);
      this.lblNewNew.Name = "lblNewNew";
      this.lblNewNew.Size = new System.Drawing.Size(64, 13);
      this.lblNewNew.TabIndex = 62;
      this.lblNewNew.Text = "** NEW **";
      this.lblNewNew.Visible = false;
      // 
      // btnNewDelete
      // 
      this.btnNewDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnNewDelete.Location = new System.Drawing.Point(1055, 173);
      this.btnNewDelete.Name = "btnNewDelete";
      this.btnNewDelete.Size = new System.Drawing.Size(65, 20);
      this.btnNewDelete.TabIndex = 23;
      this.btnNewDelete.Text = "Delete";
      this.btnNewDelete.UseVisualStyleBackColor = true;
      this.btnNewDelete.Click += new System.EventHandler(this.btnNewDelete_Click);
      // 
      // label18
      // 
      this.label18.AutoSize = true;
      this.label18.Location = new System.Drawing.Point(8, 36);
      this.label18.Name = "label18";
      this.label18.Size = new System.Drawing.Size(97, 13);
      this.label18.TabIndex = 61;
      this.label18.Text = "Customer/Location";
      // 
      // txtNewNetInvID
      // 
      this.txtNewNetInvID.Enabled = false;
      this.txtNewNetInvID.Location = new System.Drawing.Point(5, 3);
      this.txtNewNetInvID.Name = "txtNewNetInvID";
      this.txtNewNetInvID.Size = new System.Drawing.Size(55, 20);
      this.txtNewNetInvID.TabIndex = 0;
      // 
      // btnNewNew
      // 
      this.btnNewNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnNewNew.Location = new System.Drawing.Point(981, 199);
      this.btnNewNew.Name = "btnNewNew";
      this.btnNewNew.Size = new System.Drawing.Size(65, 22);
      this.btnNewNew.TabIndex = 22;
      this.btnNewNew.Text = "Add New";
      this.btnNewNew.UseVisualStyleBackColor = true;
      this.btnNewNew.Click += new System.EventHandler(this.btnNewNew_Click);
      // 
      // label19
      // 
      this.label19.AutoSize = true;
      this.label19.Location = new System.Drawing.Point(61, 6);
      this.label19.Name = "label19";
      this.label19.Size = new System.Drawing.Size(36, 13);
      this.label19.TabIndex = 1;
      this.label19.Text = "Order:";
      // 
      // btnNewSave
      // 
      this.btnNewSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnNewSave.Location = new System.Drawing.Point(907, 199);
      this.btnNewSave.Name = "btnNewSave";
      this.btnNewSave.Size = new System.Drawing.Size(65, 22);
      this.btnNewSave.TabIndex = 21;
      this.btnNewSave.Text = "Save";
      this.btnNewSave.UseVisualStyleBackColor = true;
      this.btnNewSave.Click += new System.EventHandler(this.btnNewSave_Click);
      // 
      // label21
      // 
      this.label21.AutoSize = true;
      this.label21.Location = new System.Drawing.Point(8, 59);
      this.label21.Name = "label21";
      this.label21.Size = new System.Drawing.Size(79, 13);
      this.label21.TabIndex = 38;
      this.label21.Text = "Carrier/Product";
      // 
      // label23
      // 
      this.label23.AutoSize = true;
      this.label23.Location = new System.Drawing.Point(8, 86);
      this.label23.Name = "label23";
      this.label23.Size = new System.Drawing.Size(47, 13);
      this.label23.TabIndex = 43;
      this.label23.Text = "Account";
      // 
      // label24
      // 
      this.label24.AutoSize = true;
      this.label24.Location = new System.Drawing.Point(8, 116);
      this.label24.Name = "label24";
      this.label24.Size = new System.Drawing.Size(103, 13);
      this.label24.TabIndex = 45;
      this.label24.Text = "Transaction Qty/Qty";
      // 
      // label25
      // 
      this.label25.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label25.AutoSize = true;
      this.label25.Location = new System.Drawing.Point(834, 59);
      this.label25.Name = "label25";
      this.label25.Size = new System.Drawing.Size(72, 13);
      this.label25.TabIndex = 47;
      this.label25.Text = "Date Installed";
      // 
      // txtNewMRC
      // 
      this.txtNewMRC.Location = new System.Drawing.Point(412, 112);
      this.txtNewMRC.Name = "txtNewMRC";
      this.txtNewMRC.Size = new System.Drawing.Size(43, 20);
      this.txtNewMRC.TabIndex = 10;
      // 
      // label26
      // 
      this.label26.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label26.AutoSize = true;
      this.label26.Location = new System.Drawing.Point(834, 128);
      this.label26.Name = "label26";
      this.label26.Size = new System.Drawing.Size(91, 13);
      this.label26.TabIndex = 49;
      this.label26.Text = "Service End Date";
      // 
      // txtNewOrderID
      // 
      this.txtNewOrderID.Location = new System.Drawing.Point(110, 3);
      this.txtNewOrderID.Name = "txtNewOrderID";
      this.txtNewOrderID.Size = new System.Drawing.Size(182, 20);
      this.txtNewOrderID.TabIndex = 0;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(542, 116);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(85, 13);
      this.label1.TabIndex = 10;
      this.label1.Text = "Last Modified By";
      // 
      // txtLastModifiedBy
      // 
      this.txtLastModifiedBy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtLastModifiedBy.Enabled = false;
      this.txtLastModifiedBy.Location = new System.Drawing.Point(633, 112);
      this.txtLastModifiedBy.Name = "txtLastModifiedBy";
      this.txtLastModifiedBy.Size = new System.Drawing.Size(187, 20);
      this.txtLastModifiedBy.TabIndex = 12;
      // 
      // srchNetworkInventory
      // 
      this.srchNetworkInventory.AllowSortByColumn = true;
      this.srchNetworkInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.srchNetworkInventory.AutoRefreshWhenFieldChecked = false;
      this.srchNetworkInventory.AutoSaveUserOptions = false;
      this.srchNetworkInventory.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.srchNetworkInventory.CanChangeDisplayFields = true;
      this.srchNetworkInventory.CanChangeDisplaySearchCriteria = true;
      this.srchNetworkInventory.ColumnName = "CustomerName";
      this.srchNetworkInventory.DisplayFields = false;
      this.srchNetworkInventory.DisplaySearchCriteria = false;
      this.srchNetworkInventory.FieldsDefaultIsChecked = true;
      this.srchNetworkInventory.ForceReloadSearchColumns = false;
      this.srchNetworkInventory.IDList = null;
      this.srchNetworkInventory.IncludeGroupAsCriteria = false;
      this.srchNetworkInventory.InnerWhere = "";
      this.srchNetworkInventory.Location = new System.Drawing.Point(0, 14);
      this.srchNetworkInventory.Name = "srchNetworkInventory";
      this.srchNetworkInventory.NameType = CCI.Common.CommonData.UnmatchedNameTypes.Customer;
      this.srchNetworkInventory.SearchCriteria = null;
      this.srchNetworkInventory.Size = new System.Drawing.Size(1127, 392);
      this.srchNetworkInventory.TabIndex = 0;
      this.srchNetworkInventory.Title = "Search (0 Records Found)";
      this.srchNetworkInventory.UniqueIdentifier = "ID";
      this.srchNetworkInventory.UseNamedSearches = false;
      this.srchNetworkInventory.RowSelected += new ACG.CommonForms.ctlSearchGrid.RowSelectedHandler(this.srchNetworkInventory_RowSelected);
      // 
      // frmNetworkInventory
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1140, 673);
      this.Controls.Add(this.ckActiveInventoryOnly);
      this.Controls.Add(this.ctlLocationSearch);
      this.Controls.Add(this.ctlCustomerSearch);
      this.Controls.Add(this.btnSearch);
      this.Controls.Add(this.txtOrderSearch);
      this.Controls.Add(this.label15);
      this.Controls.Add(this.label13);
      this.Controls.Add(this.label14);
      this.Controls.Add(this.splitMain);
      this.Name = "frmNetworkInventory";
      this.Text = "frmCityCareNetworkInventory";
      this.Load += new System.EventHandler(this.frmCityCareNetworkInventory_Load);
      this.splitMain.Panel1.ResumeLayout(false);
      this.splitMain.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
      this.splitMain.ResumeLayout(false);
      this.pnlNewNetworkInventory.ResumeLayout(false);
      this.pnlNewNetworkInventory.PerformLayout();
      this.grpPhysInventory.ResumeLayout(false);
      this.grpPhysInventory.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitMain;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.TextBox txtOrderSearch;
    private System.Windows.Forms.Button btnSearch;
    private Common.ctlSearchGrid srchNetworkInventory;
    private ACG.CommonForms.ctlSearch ctlCustomerSearch;
    private ACG.CommonForms.ctlSearch ctlLocationSearch;
    private System.Windows.Forms.Panel pnlNewNetworkInventory;
    private ACG.CommonForms.ctlSearch txtNewLocation;
    private ACG.CommonForms.ctlSearch txtNewCustomer;
    private ACG.CommonForms.ctlSearch txtNewItemID;
    private System.Windows.Forms.Label label36;
    private System.Windows.Forms.TextBox txtNewAccount;
    private System.Windows.Forms.Button btnNewCancel;
    private System.Windows.Forms.Button btnClone;
    private System.Windows.Forms.Label label34;
    private System.Windows.Forms.TextBox txtQuantity;
    private System.Windows.Forms.Label label31;
    private System.Windows.Forms.TextBox txtNewNRC;
    private System.Windows.Forms.Label label30;
    private System.Windows.Forms.Label lblNewNew;
    private System.Windows.Forms.Button btnNewDelete;
    private System.Windows.Forms.Label label18;
    private System.Windows.Forms.TextBox txtNewNetInvID;
    private System.Windows.Forms.Button btnNewNew;
    private System.Windows.Forms.Label label19;
    private System.Windows.Forms.Button btnNewSave;
    private System.Windows.Forms.Label label21;
    private System.Windows.Forms.Label label23;
    private System.Windows.Forms.Label label24;
    private System.Windows.Forms.Label label25;
    private System.Windows.Forms.TextBox txtNewMRC;
    private System.Windows.Forms.Label label26;
    private System.Windows.Forms.TextBox txtNewOrderID;
    private System.Windows.Forms.CheckBox ckActiveInventoryOnly;
    private System.Windows.Forms.TextBox txtLastModifiedBy;
    private System.Windows.Forms.Label label1;
    private ACG.CommonForms.ctlACGDate dtNewDateInstalled;
    private ACG.CommonForms.ctlACGDate dtNewStartDate;
    private ACG.CommonForms.ctlACGDate dtNewExpDate;
    private ACG.CommonForms.ctlACGDate dtNewEndDate;
    private ACG.CommonForms.ctlACGDate dtTransactionDate;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtTransactionMRC;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtTransactionQuantity;
    private ACG.CommonForms.ctlSearch txtPrimaryCarrier;
    private Common.ctlSearchGrid srchPhysicalInventory;
    private System.Windows.Forms.GroupBox grpPhysInventory;
    private System.Windows.Forms.Button btnDeletehysicalInventory;
    private System.Windows.Forms.Button btnSavePhysicalInventory;
    private System.Windows.Forms.TextBox txtPhysicalInventoryNotes;
    private System.Windows.Forms.Label lblNotes;
    private System.Windows.Forms.TextBox txtMacAddress;
    private System.Windows.Forms.Label lblMacAddress;
    private System.Windows.Forms.TextBox txtPhysicalInventoryID;
    private System.Windows.Forms.Label lblDealer;
    private System.Windows.Forms.TextBox txtDealer;
    private System.Windows.Forms.Button btnNewPhysicalInventory;
    private System.Windows.Forms.Label lblPhysicalInventoryID;
    private System.Windows.Forms.Button btnQuickEntryPhysicalInventory;
    private System.Windows.Forms.Button btnQENormalEntry;
    private System.Windows.Forms.TextBox txtQENotes;
    private System.Windows.Forms.Label lblQENotes;
    private System.Windows.Forms.TextBox txtQEMacAddress;
    private System.Windows.Forms.Label lblQEMacAddress;
    private System.Windows.Forms.Label lblQEUsoc;
    private System.Windows.Forms.ComboBox txtQEUsoc;
    private System.Windows.Forms.TextBox txtDay2YN;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox txtAgent;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox txtMasterCustomer;
    private System.Windows.Forms.Label label5;
  }
}