namespace CCI.DesktopClient.Screens
{
  partial class frmCityHostedUSOCMaintenanceNew
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
      this.cboCarrier = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.cboMode = new System.Windows.Forms.ComboBox();
      this.lblMode = new System.Windows.Forms.Label();
      this.srchUSOCList = new CCI.DesktopClient.Common.ctlSearchGrid();
      this.ckRefresh = new System.Windows.Forms.CheckBox();
      this.ckIncludeInactive = new System.Windows.Forms.CheckBox();
      this.ckIncludeNonSaddleback = new System.Windows.Forms.CheckBox();
      this.tabMaintenance = new System.Windows.Forms.TabControl();
      this.tabRetail = new System.Windows.Forms.TabPage();
      this.grdDealerCosts = new System.Windows.Forms.DataGridView();
      this.btnRetailNew = new System.Windows.Forms.Button();
      this.btnRetailCancel = new System.Windows.Forms.Button();
      this.btnRetailSave = new System.Windows.Forms.Button();
      this.ckExcludeFromExceptions = new System.Windows.Forms.CheckBox();
      this.ckSaddlebackUSOC = new System.Windows.Forms.CheckBox();
      this.ckRetailInactivate = new System.Windows.Forms.CheckBox();
      this.ckVariableRetailNRC = new System.Windows.Forms.CheckBox();
      this.ckVariableRetailMRC = new System.Windows.Forms.CheckBox();
      this.label3 = new System.Windows.Forms.Label();
      this.txtRetailUSOC = new ACG.CommonForms.ctlSearch();
      this.cboTaxCode = new System.Windows.Forms.ComboBox();
      this.label12 = new System.Windows.Forms.Label();
      this.cboRITCategory = new System.Windows.Forms.ComboBox();
      this.label5 = new System.Windows.Forms.Label();
      this.txtExternalDescription = new System.Windows.Forms.TextBox();
      this.label11 = new System.Windows.Forms.Label();
      this.txtRetailDescription = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.ckRetailOnly = new System.Windows.Forms.CheckBox();
      this.dtRetailEndDate = new System.Windows.Forms.DateTimePicker();
      this.label8 = new System.Windows.Forms.Label();
      this.dtRetailStartDate = new System.Windows.Forms.DateTimePicker();
      this.label9 = new System.Windows.Forms.Label();
      this.label10 = new System.Windows.Forms.Label();
      this.txtRetailNRC = new System.Windows.Forms.TextBox();
      this.txtRetailMRC = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.cboCHSCategory = new System.Windows.Forms.ComboBox();
      this.tabWholesale = new System.Windows.Forms.TabPage();
      this.label1 = new System.Windows.Forms.Label();
      this.txtWholesaleUSOC = new ACG.CommonForms.ctlSearch();
      this.ckWholesaleInactivate = new System.Windows.Forms.CheckBox();
      this.txtWholesaleDescription = new System.Windows.Forms.TextBox();
      this.label13 = new System.Windows.Forms.Label();
      this.ckWholesaleOnly = new System.Windows.Forms.CheckBox();
      this.dtWholesaleEndDate = new System.Windows.Forms.DateTimePicker();
      this.label14 = new System.Windows.Forms.Label();
      this.dtWholesaleStartDate = new System.Windows.Forms.DateTimePicker();
      this.label15 = new System.Windows.Forms.Label();
      this.label16 = new System.Windows.Forms.Label();
      this.txtWholesaleNRC = new System.Windows.Forms.TextBox();
      this.txtWholesaleMRC = new System.Windows.Forms.TextBox();
      this.btnWholesaleCancel = new System.Windows.Forms.Button();
      this.btnWholesaleSave = new System.Windows.Forms.Button();
      this.btnWholesaleNew = new System.Windows.Forms.Button();
      this.tabMatching = new System.Windows.Forms.TabPage();
      this.label18 = new System.Windows.Forms.Label();
      this.label17 = new System.Windows.Forms.Label();
      this.srchWholesaleUsocMatching = new ACG.CommonForms.ctlSearch();
      this.srchRetailUsocMatching = new ACG.CommonForms.ctlSearch();
      this.lblUsocMatching = new System.Windows.Forms.Label();
      this.lstWholesaleUsocs = new System.Windows.Forms.ListBox();
      this.btnUnmatchUsoc = new System.Windows.Forms.Button();
      this.btnMatchUsoc = new System.Windows.Forms.Button();
      this.lstRetailUsocs = new System.Windows.Forms.ListBox();
      this.label4 = new System.Windows.Forms.Label();
      this.tabMaintenance.SuspendLayout();
      this.tabRetail.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdDealerCosts)).BeginInit();
      this.tabWholesale.SuspendLayout();
      this.tabMatching.SuspendLayout();
      this.SuspendLayout();
      // 
      // cboCarrier
      // 
      this.cboCarrier.FormattingEnabled = true;
      this.cboCarrier.Location = new System.Drawing.Point(589, 3);
      this.cboCarrier.Name = "cboCarrier";
      this.cboCarrier.Size = new System.Drawing.Size(121, 21);
      this.cboCarrier.TabIndex = 18;
      this.cboCarrier.Text = "Saddleback";
      this.cboCarrier.SelectedIndexChanged += new System.EventHandler(this.cboCarrier_SelectedIndexChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(546, 6);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(37, 13);
      this.label2.TabIndex = 17;
      this.label2.Text = "Carrier";
      // 
      // cboMode
      // 
      this.cboMode.FormattingEnabled = true;
      this.cboMode.Items.AddRange(new object[] {
            "Edit Wholesale",
            "Edit Retail",
            "Match Retail/Wholesale"});
      this.cboMode.Location = new System.Drawing.Point(398, 3);
      this.cboMode.Name = "cboMode";
      this.cboMode.Size = new System.Drawing.Size(132, 21);
      this.cboMode.TabIndex = 16;
      this.cboMode.Text = "Edit Retail";
      this.cboMode.SelectedIndexChanged += new System.EventHandler(this.cboMode_SelectedIndexChanged);
      // 
      // lblMode
      // 
      this.lblMode.AutoSize = true;
      this.lblMode.Location = new System.Drawing.Point(358, 6);
      this.lblMode.Name = "lblMode";
      this.lblMode.Size = new System.Drawing.Size(34, 13);
      this.lblMode.TabIndex = 15;
      this.lblMode.Text = "Mode";
      // 
      // srchUSOCList
      // 
      this.srchUSOCList.AllowSortByColumn = true;
      this.srchUSOCList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.srchUSOCList.AutoRefreshWhenFieldChecked = false;
      this.srchUSOCList.AutoSaveUserOptions = false;
      this.srchUSOCList.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.srchUSOCList.CanChangeDisplayFields = true;
      this.srchUSOCList.CanChangeDisplaySearchCriteria = true;
      this.srchUSOCList.ColumnName = "CustomerName";
      this.srchUSOCList.DisplayFields = false;
      this.srchUSOCList.DisplaySearchCriteria = false;
      this.srchUSOCList.FieldsDefaultIsChecked = true;
      this.srchUSOCList.ForceReloadSearchColumns = false;
      this.srchUSOCList.IDList = null;
      this.srchUSOCList.IncludeGroupAsCriteria = false;
      this.srchUSOCList.InnerWhere = "";
      this.srchUSOCList.Location = new System.Drawing.Point(1, 27);
      this.srchUSOCList.Name = "srchUSOCList";
      this.srchUSOCList.NameType = CCI.Common.CommonData.UnmatchedNameTypes.Customer;
      this.srchUSOCList.SearchCriteria = null;
      this.srchUSOCList.Size = new System.Drawing.Size(1418, 297);
      this.srchUSOCList.TabIndex = 14;
      this.srchUSOCList.Title = "Search (0 Records Found)";
      this.srchUSOCList.UniqueIdentifier = "ID";
      this.srchUSOCList.UseNamedSearches = false;
      this.srchUSOCList.RowSelected += new ACG.CommonForms.ctlSearchGrid.RowSelectedHandler(this.srchUSOCList_RowSelected);
      // 
      // ckRefresh
      // 
      this.ckRefresh.AutoSize = true;
      this.ckRefresh.Checked = true;
      this.ckRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckRefresh.Location = new System.Drawing.Point(264, 5);
      this.ckRefresh.Name = "ckRefresh";
      this.ckRefresh.Size = new System.Drawing.Size(88, 17);
      this.ckRefresh.TabIndex = 13;
      this.ckRefresh.Text = "Auto Refresh";
      this.ckRefresh.UseVisualStyleBackColor = true;
      this.ckRefresh.CheckedChanged += new System.EventHandler(this.ckRefresh_CheckedChanged);
      // 
      // ckIncludeInactive
      // 
      this.ckIncludeInactive.AutoSize = true;
      this.ckIncludeInactive.Location = new System.Drawing.Point(126, 5);
      this.ckIncludeInactive.Name = "ckIncludeInactive";
      this.ckIncludeInactive.Size = new System.Drawing.Size(132, 17);
      this.ckIncludeInactive.TabIndex = 12;
      this.ckIncludeInactive.Text = "Include Inactive Retail";
      this.ckIncludeInactive.UseVisualStyleBackColor = true;
      this.ckIncludeInactive.CheckedChanged += new System.EventHandler(this.ckIncludeInactive_CheckedChanged);
      // 
      // ckIncludeNonSaddleback
      // 
      this.ckIncludeNonSaddleback.AutoSize = true;
      this.ckIncludeNonSaddleback.Checked = true;
      this.ckIncludeNonSaddleback.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckIncludeNonSaddleback.Location = new System.Drawing.Point(7, 5);
      this.ckIncludeNonSaddleback.Name = "ckIncludeNonSaddleback";
      this.ckIncludeNonSaddleback.Size = new System.Drawing.Size(113, 17);
      this.ckIncludeNonSaddleback.TabIndex = 11;
      this.ckIncludeNonSaddleback.Text = "Include All USOCs";
      this.ckIncludeNonSaddleback.UseVisualStyleBackColor = true;
      // 
      // tabMaintenance
      // 
      this.tabMaintenance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabMaintenance.Controls.Add(this.tabRetail);
      this.tabMaintenance.Controls.Add(this.tabWholesale);
      this.tabMaintenance.Controls.Add(this.tabMatching);
      this.tabMaintenance.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
      this.tabMaintenance.Location = new System.Drawing.Point(1, 330);
      this.tabMaintenance.Name = "tabMaintenance";
      this.tabMaintenance.SelectedIndex = 0;
      this.tabMaintenance.Size = new System.Drawing.Size(1418, 320);
      this.tabMaintenance.TabIndex = 19;
      this.tabMaintenance.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabMaintenance_DrawItem);
      // 
      // tabRetail
      // 
      this.tabRetail.AutoScroll = true;
      this.tabRetail.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.tabRetail.Controls.Add(this.grdDealerCosts);
      this.tabRetail.Controls.Add(this.btnRetailNew);
      this.tabRetail.Controls.Add(this.btnRetailCancel);
      this.tabRetail.Controls.Add(this.btnRetailSave);
      this.tabRetail.Controls.Add(this.ckExcludeFromExceptions);
      this.tabRetail.Controls.Add(this.ckSaddlebackUSOC);
      this.tabRetail.Controls.Add(this.ckRetailInactivate);
      this.tabRetail.Controls.Add(this.ckVariableRetailNRC);
      this.tabRetail.Controls.Add(this.ckVariableRetailMRC);
      this.tabRetail.Controls.Add(this.label3);
      this.tabRetail.Controls.Add(this.txtRetailUSOC);
      this.tabRetail.Controls.Add(this.cboTaxCode);
      this.tabRetail.Controls.Add(this.label12);
      this.tabRetail.Controls.Add(this.cboRITCategory);
      this.tabRetail.Controls.Add(this.label5);
      this.tabRetail.Controls.Add(this.txtExternalDescription);
      this.tabRetail.Controls.Add(this.label11);
      this.tabRetail.Controls.Add(this.txtRetailDescription);
      this.tabRetail.Controls.Add(this.label6);
      this.tabRetail.Controls.Add(this.ckRetailOnly);
      this.tabRetail.Controls.Add(this.dtRetailEndDate);
      this.tabRetail.Controls.Add(this.label8);
      this.tabRetail.Controls.Add(this.dtRetailStartDate);
      this.tabRetail.Controls.Add(this.label9);
      this.tabRetail.Controls.Add(this.label10);
      this.tabRetail.Controls.Add(this.txtRetailNRC);
      this.tabRetail.Controls.Add(this.txtRetailMRC);
      this.tabRetail.Controls.Add(this.label7);
      this.tabRetail.Controls.Add(this.cboCHSCategory);
      this.tabRetail.Location = new System.Drawing.Point(4, 22);
      this.tabRetail.Name = "tabRetail";
      this.tabRetail.Padding = new System.Windows.Forms.Padding(3);
      this.tabRetail.Size = new System.Drawing.Size(1410, 294);
      this.tabRetail.TabIndex = 0;
      this.tabRetail.Text = "Retail Usoc Maintenance";
      // 
      // grdDealerCosts
      // 
      this.grdDealerCosts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grdDealerCosts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdDealerCosts.Location = new System.Drawing.Point(611, 31);
      this.grdDealerCosts.Name = "grdDealerCosts";
      this.grdDealerCosts.Size = new System.Drawing.Size(783, 244);
      this.grdDealerCosts.TabIndex = 133;
      this.grdDealerCosts.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdDealerCosts_RowLeave);
      this.grdDealerCosts.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.grdDealerCosts_RowValidating);
      // 
      // btnRetailNew
      // 
      this.btnRetailNew.Location = new System.Drawing.Point(14, 242);
      this.btnRetailNew.Name = "btnRetailNew";
      this.btnRetailNew.Size = new System.Drawing.Size(77, 23);
      this.btnRetailNew.TabIndex = 108;
      this.btnRetailNew.Text = "Add New";
      this.btnRetailNew.UseVisualStyleBackColor = true;
      this.btnRetailNew.Click += new System.EventHandler(this.btnRetailNew_Click);
      // 
      // btnRetailCancel
      // 
      this.btnRetailCancel.Location = new System.Drawing.Point(97, 242);
      this.btnRetailCancel.Name = "btnRetailCancel";
      this.btnRetailCancel.Size = new System.Drawing.Size(77, 23);
      this.btnRetailCancel.TabIndex = 110;
      this.btnRetailCancel.Text = "Cancel";
      this.btnRetailCancel.UseVisualStyleBackColor = true;
      this.btnRetailCancel.Click += new System.EventHandler(this.btnRetailCancel_Click);
      // 
      // btnRetailSave
      // 
      this.btnRetailSave.Location = new System.Drawing.Point(180, 242);
      this.btnRetailSave.Name = "btnRetailSave";
      this.btnRetailSave.Size = new System.Drawing.Size(77, 23);
      this.btnRetailSave.TabIndex = 107;
      this.btnRetailSave.Text = "Save";
      this.btnRetailSave.UseVisualStyleBackColor = true;
      this.btnRetailSave.Click += new System.EventHandler(this.btnRetailSave_Click);
      // 
      // ckExcludeFromExceptions
      // 
      this.ckExcludeFromExceptions.AutoSize = true;
      this.ckExcludeFromExceptions.Location = new System.Drawing.Point(414, 212);
      this.ckExcludeFromExceptions.Name = "ckExcludeFromExceptions";
      this.ckExcludeFromExceptions.Size = new System.Drawing.Size(172, 17);
      this.ckExcludeFromExceptions.TabIndex = 106;
      this.ckExcludeFromExceptions.Text = "Exclude from Retail Exceptions";
      this.ckExcludeFromExceptions.UseVisualStyleBackColor = true;
      // 
      // ckSaddlebackUSOC
      // 
      this.ckSaddlebackUSOC.AccessibleDescription = " ";
      this.ckSaddlebackUSOC.AutoSize = true;
      this.ckSaddlebackUSOC.Location = new System.Drawing.Point(470, 161);
      this.ckSaddlebackUSOC.Name = "ckSaddlebackUSOC";
      this.ckSaddlebackUSOC.Size = new System.Drawing.Size(101, 17);
      this.ckSaddlebackUSOC.TabIndex = 122;
      this.ckSaddlebackUSOC.Text = "CityHosted Only";
      this.ckSaddlebackUSOC.UseVisualStyleBackColor = true;
      // 
      // ckRetailInactivate
      // 
      this.ckRetailInactivate.AutoSize = true;
      this.ckRetailInactivate.Location = new System.Drawing.Point(364, 161);
      this.ckRetailInactivate.Name = "ckRetailInactivate";
      this.ckRetailInactivate.Size = new System.Drawing.Size(64, 17);
      this.ckRetailInactivate.TabIndex = 121;
      this.ckRetailInactivate.Text = "Inactive";
      this.ckRetailInactivate.UseVisualStyleBackColor = true;
      // 
      // ckVariableRetailNRC
      // 
      this.ckVariableRetailNRC.AutoSize = true;
      this.ckVariableRetailNRC.Location = new System.Drawing.Point(339, 129);
      this.ckVariableRetailNRC.Name = "ckVariableRetailNRC";
      this.ckVariableRetailNRC.Size = new System.Drawing.Size(64, 17);
      this.ckVariableRetailNRC.TabIndex = 117;
      this.ckVariableRetailNRC.Text = "Variable";
      this.ckVariableRetailNRC.UseVisualStyleBackColor = true;
      // 
      // ckVariableRetailMRC
      // 
      this.ckVariableRetailMRC.AutoSize = true;
      this.ckVariableRetailMRC.Location = new System.Drawing.Point(215, 129);
      this.ckVariableRetailMRC.Name = "ckVariableRetailMRC";
      this.ckVariableRetailMRC.Size = new System.Drawing.Size(64, 17);
      this.ckVariableRetailMRC.TabIndex = 115;
      this.ckVariableRetailMRC.Text = "Variable";
      this.ckVariableRetailMRC.UseVisualStyleBackColor = true;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(10, 3);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(251, 24);
      this.label3.TabIndex = 105;
      this.label3.Text = "Retail USOC Maintenance";
      // 
      // txtRetailUSOC
      // 
      this.txtRetailUSOC.AddNewMode = false;
      this.txtRetailUSOC.AutoAddNewMode = false;
      this.txtRetailUSOC.AutoSelectWhenMatch = false;
      this.txtRetailUSOC.AutoTabToNextControlOnSelect = true;
      this.txtRetailUSOC.ClearSearchOnExpand = false;
      this.txtRetailUSOC.ClearSearchWhenComplete = false;
      this.txtRetailUSOC.Collapsed = true;
      this.txtRetailUSOC.CreatedNewItem = false;
      this.txtRetailUSOC.DisplayOnlyDescription = false;
      this.txtRetailUSOC.DisplayOnlyID = false;
      this.txtRetailUSOC.FixKeySpace = "-1";
      this.txtRetailUSOC.ID = "";
      this.txtRetailUSOC.ID_DescSplitter = ":";
      this.txtRetailUSOC.Location = new System.Drawing.Point(151, 30);
      this.txtRetailUSOC.MaxHeight = 228;
      this.txtRetailUSOC.MustExistInList = false;
      this.txtRetailUSOC.MustExistMessage = "You must enter a valid value";
      this.txtRetailUSOC.Name = "txtRetailUSOC";
      this.txtRetailUSOC.SearchExec = null;
      this.txtRetailUSOC.ShowCustomerNameWhenSet = true;
      this.txtRetailUSOC.ShowTermedCheckBox = false;
      this.txtRetailUSOC.Size = new System.Drawing.Size(434, 27);
      this.txtRetailUSOC.TabIndex = 111;
      // 
      // cboTaxCode
      // 
      this.cboTaxCode.FormattingEnabled = true;
      this.cboTaxCode.Location = new System.Drawing.Point(150, 210);
      this.cboTaxCode.Name = "cboTaxCode";
      this.cboTaxCode.Size = new System.Drawing.Size(176, 21);
      this.cboTaxCode.TabIndex = 131;
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(11, 214);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(68, 13);
      this.label12.TabIndex = 132;
      this.label12.Text = "RIT TranTax";
      // 
      // cboRITCategory
      // 
      this.cboRITCategory.FormattingEnabled = true;
      this.cboRITCategory.Location = new System.Drawing.Point(150, 185);
      this.cboRITCategory.Name = "cboRITCategory";
      this.cboRITCategory.Size = new System.Drawing.Size(176, 21);
      this.cboRITCategory.TabIndex = 123;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(11, 189);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(70, 13);
      this.label5.TabIndex = 130;
      this.label5.Text = "RIT Category";
      // 
      // txtExternalDescription
      // 
      this.txtExternalDescription.Location = new System.Drawing.Point(150, 96);
      this.txtExternalDescription.Name = "txtExternalDescription";
      this.txtExternalDescription.Size = new System.Drawing.Size(436, 20);
      this.txtExternalDescription.TabIndex = 113;
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(11, 99);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(133, 13);
      this.label11.TabIndex = 129;
      this.label11.Text = "RIT Retail Bill Presentation";
      // 
      // txtRetailDescription
      // 
      this.txtRetailDescription.Location = new System.Drawing.Point(150, 63);
      this.txtRetailDescription.Name = "txtRetailDescription";
      this.txtRetailDescription.Size = new System.Drawing.Size(436, 20);
      this.txtRetailDescription.TabIndex = 112;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(12, 34);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(67, 13);
      this.label6.TabIndex = 125;
      this.label6.Text = "Retail USOC";
      // 
      // ckRetailOnly
      // 
      this.ckRetailOnly.AutoSize = true;
      this.ckRetailOnly.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ckRetailOnly.Location = new System.Drawing.Point(509, 129);
      this.ckRetailOnly.Name = "ckRetailOnly";
      this.ckRetailOnly.Size = new System.Drawing.Size(77, 17);
      this.ckRetailOnly.TabIndex = 118;
      this.ckRetailOnly.Text = "Retail Only";
      this.ckRetailOnly.UseVisualStyleBackColor = true;
      // 
      // dtRetailEndDate
      // 
      this.dtRetailEndDate.Checked = false;
      this.dtRetailEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtRetailEndDate.Location = new System.Drawing.Point(256, 159);
      this.dtRetailEndDate.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
      this.dtRetailEndDate.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtRetailEndDate.Name = "dtRetailEndDate";
      this.dtRetailEndDate.RightToLeftLayout = true;
      this.dtRetailEndDate.ShowCheckBox = true;
      this.dtRetailEndDate.Size = new System.Drawing.Size(98, 20);
      this.dtRetailEndDate.TabIndex = 120;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(11, 65);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(60, 13);
      this.label8.TabIndex = 126;
      this.label8.Text = "Description";
      // 
      // dtRetailStartDate
      // 
      this.dtRetailStartDate.Checked = false;
      this.dtRetailStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtRetailStartDate.Location = new System.Drawing.Point(150, 159);
      this.dtRetailStartDate.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
      this.dtRetailStartDate.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtRetailStartDate.Name = "dtRetailStartDate";
      this.dtRetailStartDate.ShowCheckBox = true;
      this.dtRetailStartDate.Size = new System.Drawing.Size(98, 20);
      this.dtRetailStartDate.TabIndex = 119;
      this.dtRetailStartDate.Value = new System.DateTime(2012, 12, 7, 9, 11, 0, 0);
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(11, 131);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(89, 13);
      this.label9.TabIndex = 127;
      this.label9.Text = "Retail MRC/NRC";
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(11, 163);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(79, 13);
      this.label10.TabIndex = 128;
      this.label10.Text = "Start/End Date";
      // 
      // txtRetailNRC
      // 
      this.txtRetailNRC.Location = new System.Drawing.Point(276, 127);
      this.txtRetailNRC.Name = "txtRetailNRC";
      this.txtRetailNRC.Size = new System.Drawing.Size(57, 20);
      this.txtRetailNRC.TabIndex = 116;
      // 
      // txtRetailMRC
      // 
      this.txtRetailMRC.Location = new System.Drawing.Point(150, 127);
      this.txtRetailMRC.Name = "txtRetailMRC";
      this.txtRetailMRC.Size = new System.Drawing.Size(59, 20);
      this.txtRetailMRC.TabIndex = 114;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(329, 189);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(74, 13);
      this.label7.TabIndex = 109;
      this.label7.Text = "CHS Category";
      // 
      // cboCHSCategory
      // 
      this.cboCHSCategory.FormattingEnabled = true;
      this.cboCHSCategory.Location = new System.Drawing.Point(409, 185);
      this.cboCHSCategory.Name = "cboCHSCategory";
      this.cboCHSCategory.Size = new System.Drawing.Size(176, 21);
      this.cboCHSCategory.TabIndex = 124;
      // 
      // tabWholesale
      // 
      this.tabWholesale.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.tabWholesale.Controls.Add(this.label1);
      this.tabWholesale.Controls.Add(this.txtWholesaleUSOC);
      this.tabWholesale.Controls.Add(this.ckWholesaleInactivate);
      this.tabWholesale.Controls.Add(this.txtWholesaleDescription);
      this.tabWholesale.Controls.Add(this.label13);
      this.tabWholesale.Controls.Add(this.ckWholesaleOnly);
      this.tabWholesale.Controls.Add(this.dtWholesaleEndDate);
      this.tabWholesale.Controls.Add(this.label14);
      this.tabWholesale.Controls.Add(this.dtWholesaleStartDate);
      this.tabWholesale.Controls.Add(this.label15);
      this.tabWholesale.Controls.Add(this.label16);
      this.tabWholesale.Controls.Add(this.txtWholesaleNRC);
      this.tabWholesale.Controls.Add(this.txtWholesaleMRC);
      this.tabWholesale.Controls.Add(this.btnWholesaleCancel);
      this.tabWholesale.Controls.Add(this.btnWholesaleSave);
      this.tabWholesale.Controls.Add(this.btnWholesaleNew);
      this.tabWholesale.Location = new System.Drawing.Point(4, 22);
      this.tabWholesale.Name = "tabWholesale";
      this.tabWholesale.Padding = new System.Windows.Forms.Padding(3);
      this.tabWholesale.Size = new System.Drawing.Size(1410, 294);
      this.tabWholesale.TabIndex = 1;
      this.tabWholesale.Text = "Wholesale Usoc Maintenance";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(8, 12);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(297, 24);
      this.label1.TabIndex = 58;
      this.label1.Text = "Wholesale USOC Maintenance";
      // 
      // txtWholesaleUSOC
      // 
      this.txtWholesaleUSOC.AddNewMode = false;
      this.txtWholesaleUSOC.AutoAddNewMode = false;
      this.txtWholesaleUSOC.AutoSelectWhenMatch = false;
      this.txtWholesaleUSOC.AutoTabToNextControlOnSelect = true;
      this.txtWholesaleUSOC.ClearSearchOnExpand = false;
      this.txtWholesaleUSOC.ClearSearchWhenComplete = false;
      this.txtWholesaleUSOC.Collapsed = true;
      this.txtWholesaleUSOC.CreatedNewItem = false;
      this.txtWholesaleUSOC.DisplayOnlyDescription = false;
      this.txtWholesaleUSOC.DisplayOnlyID = false;
      this.txtWholesaleUSOC.FixKeySpace = "-1";
      this.txtWholesaleUSOC.ID = "";
      this.txtWholesaleUSOC.ID_DescSplitter = ":";
      this.txtWholesaleUSOC.Location = new System.Drawing.Point(104, 40);
      this.txtWholesaleUSOC.MaxHeight = 228;
      this.txtWholesaleUSOC.MustExistInList = false;
      this.txtWholesaleUSOC.MustExistMessage = "You must enter a valid value";
      this.txtWholesaleUSOC.Name = "txtWholesaleUSOC";
      this.txtWholesaleUSOC.SearchExec = null;
      this.txtWholesaleUSOC.ShowCustomerNameWhenSet = true;
      this.txtWholesaleUSOC.ShowTermedCheckBox = false;
      this.txtWholesaleUSOC.Size = new System.Drawing.Size(434, 27);
      this.txtWholesaleUSOC.TabIndex = 59;
      // 
      // ckWholesaleInactivate
      // 
      this.ckWholesaleInactivate.AutoSize = true;
      this.ckWholesaleInactivate.Location = new System.Drawing.Point(311, 137);
      this.ckWholesaleInactivate.Name = "ckWholesaleInactivate";
      this.ckWholesaleInactivate.Size = new System.Drawing.Size(64, 17);
      this.ckWholesaleInactivate.TabIndex = 68;
      this.ckWholesaleInactivate.Text = "Inactive";
      this.ckWholesaleInactivate.UseVisualStyleBackColor = true;
      // 
      // txtWholesaleDescription
      // 
      this.txtWholesaleDescription.Location = new System.Drawing.Point(105, 73);
      this.txtWholesaleDescription.Name = "txtWholesaleDescription";
      this.txtWholesaleDescription.Size = new System.Drawing.Size(436, 20);
      this.txtWholesaleDescription.TabIndex = 60;
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(8, 42);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(90, 13);
      this.label13.TabIndex = 64;
      this.label13.Text = "Wholesale USOC";
      // 
      // ckWholesaleOnly
      // 
      this.ckWholesaleOnly.AutoSize = true;
      this.ckWholesaleOnly.Location = new System.Drawing.Point(429, 107);
      this.ckWholesaleOnly.Name = "ckWholesaleOnly";
      this.ckWholesaleOnly.Size = new System.Drawing.Size(100, 17);
      this.ckWholesaleOnly.TabIndex = 63;
      this.ckWholesaleOnly.Text = "Wholesale Only";
      this.ckWholesaleOnly.UseVisualStyleBackColor = true;
      // 
      // dtWholesaleEndDate
      // 
      this.dtWholesaleEndDate.Checked = false;
      this.dtWholesaleEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtWholesaleEndDate.Location = new System.Drawing.Point(207, 137);
      this.dtWholesaleEndDate.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
      this.dtWholesaleEndDate.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtWholesaleEndDate.Name = "dtWholesaleEndDate";
      this.dtWholesaleEndDate.RightToLeftLayout = true;
      this.dtWholesaleEndDate.ShowCheckBox = true;
      this.dtWholesaleEndDate.Size = new System.Drawing.Size(98, 20);
      this.dtWholesaleEndDate.TabIndex = 66;
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(8, 75);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(60, 13);
      this.label14.TabIndex = 69;
      this.label14.Text = "Description";
      // 
      // dtWholesaleStartDate
      // 
      this.dtWholesaleStartDate.Checked = false;
      this.dtWholesaleStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtWholesaleStartDate.Location = new System.Drawing.Point(104, 137);
      this.dtWholesaleStartDate.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
      this.dtWholesaleStartDate.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtWholesaleStartDate.Name = "dtWholesaleStartDate";
      this.dtWholesaleStartDate.ShowCheckBox = true;
      this.dtWholesaleStartDate.Size = new System.Drawing.Size(98, 20);
      this.dtWholesaleStartDate.TabIndex = 65;
      this.dtWholesaleStartDate.Value = new System.DateTime(2012, 12, 7, 9, 11, 0, 0);
      // 
      // label15
      // 
      this.label15.AutoSize = true;
      this.label15.Location = new System.Drawing.Point(7, 104);
      this.label15.Name = "label15";
      this.label15.Size = new System.Drawing.Size(92, 13);
      this.label15.TabIndex = 67;
      this.label15.Text = "Whsle MRC/NRC";
      // 
      // label16
      // 
      this.label16.AutoSize = true;
      this.label16.Location = new System.Drawing.Point(7, 141);
      this.label16.Name = "label16";
      this.label16.Size = new System.Drawing.Size(79, 13);
      this.label16.TabIndex = 70;
      this.label16.Text = "Start/End Date";
      // 
      // txtWholesaleNRC
      // 
      this.txtWholesaleNRC.Location = new System.Drawing.Point(207, 104);
      this.txtWholesaleNRC.Name = "txtWholesaleNRC";
      this.txtWholesaleNRC.Size = new System.Drawing.Size(90, 20);
      this.txtWholesaleNRC.TabIndex = 62;
      // 
      // txtWholesaleMRC
      // 
      this.txtWholesaleMRC.Location = new System.Drawing.Point(104, 104);
      this.txtWholesaleMRC.Name = "txtWholesaleMRC";
      this.txtWholesaleMRC.Size = new System.Drawing.Size(87, 20);
      this.txtWholesaleMRC.TabIndex = 61;
      // 
      // btnWholesaleCancel
      // 
      this.btnWholesaleCancel.Location = new System.Drawing.Point(120, 168);
      this.btnWholesaleCancel.Name = "btnWholesaleCancel";
      this.btnWholesaleCancel.Size = new System.Drawing.Size(77, 23);
      this.btnWholesaleCancel.TabIndex = 73;
      this.btnWholesaleCancel.Text = "Cancel";
      this.btnWholesaleCancel.UseVisualStyleBackColor = true;
      this.btnWholesaleCancel.Click += new System.EventHandler(this.btnWholesaleCancel_Click);
      // 
      // btnWholesaleSave
      // 
      this.btnWholesaleSave.Location = new System.Drawing.Point(219, 168);
      this.btnWholesaleSave.Name = "btnWholesaleSave";
      this.btnWholesaleSave.Size = new System.Drawing.Size(77, 23);
      this.btnWholesaleSave.TabIndex = 71;
      this.btnWholesaleSave.Text = "Save";
      this.btnWholesaleSave.UseVisualStyleBackColor = true;
      this.btnWholesaleSave.Click += new System.EventHandler(this.btnWholesaleSave_Click);
      // 
      // btnWholesaleNew
      // 
      this.btnWholesaleNew.Location = new System.Drawing.Point(21, 168);
      this.btnWholesaleNew.Name = "btnWholesaleNew";
      this.btnWholesaleNew.Size = new System.Drawing.Size(77, 23);
      this.btnWholesaleNew.TabIndex = 72;
      this.btnWholesaleNew.Text = "Add New";
      this.btnWholesaleNew.UseVisualStyleBackColor = true;
      this.btnWholesaleNew.Click += new System.EventHandler(this.btnWholesaleNew_Click);
      // 
      // tabMatching
      // 
      this.tabMatching.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.tabMatching.Controls.Add(this.label18);
      this.tabMatching.Controls.Add(this.label17);
      this.tabMatching.Controls.Add(this.srchWholesaleUsocMatching);
      this.tabMatching.Controls.Add(this.srchRetailUsocMatching);
      this.tabMatching.Controls.Add(this.lblUsocMatching);
      this.tabMatching.Controls.Add(this.lstWholesaleUsocs);
      this.tabMatching.Controls.Add(this.btnUnmatchUsoc);
      this.tabMatching.Controls.Add(this.btnMatchUsoc);
      this.tabMatching.Controls.Add(this.lstRetailUsocs);
      this.tabMatching.Controls.Add(this.label4);
      this.tabMatching.Location = new System.Drawing.Point(4, 22);
      this.tabMatching.Name = "tabMatching";
      this.tabMatching.Size = new System.Drawing.Size(1410, 294);
      this.tabMatching.TabIndex = 2;
      this.tabMatching.Text = "Retail/Wholesale Matching";
      // 
      // label18
      // 
      this.label18.AutoSize = true;
      this.label18.Location = new System.Drawing.Point(452, 49);
      this.label18.Name = "label18";
      this.label18.Size = new System.Drawing.Size(90, 13);
      this.label18.TabIndex = 115;
      this.label18.Text = "Wholesale USOC";
      // 
      // label17
      // 
      this.label17.AutoSize = true;
      this.label17.Location = new System.Drawing.Point(14, 49);
      this.label17.Name = "label17";
      this.label17.Size = new System.Drawing.Size(67, 13);
      this.label17.TabIndex = 114;
      this.label17.Text = "Retail USOC";
      // 
      // srchWholesaleUsocMatching
      // 
      this.srchWholesaleUsocMatching.AddNewMode = false;
      this.srchWholesaleUsocMatching.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.srchWholesaleUsocMatching.AutoAddNewMode = false;
      this.srchWholesaleUsocMatching.AutoSelectWhenMatch = false;
      this.srchWholesaleUsocMatching.AutoTabToNextControlOnSelect = true;
      this.srchWholesaleUsocMatching.ClearSearchOnExpand = false;
      this.srchWholesaleUsocMatching.ClearSearchWhenComplete = false;
      this.srchWholesaleUsocMatching.Collapsed = true;
      this.srchWholesaleUsocMatching.CreatedNewItem = false;
      this.srchWholesaleUsocMatching.DisplayOnlyDescription = false;
      this.srchWholesaleUsocMatching.DisplayOnlyID = false;
      this.srchWholesaleUsocMatching.FixKeySpace = "-1";
      this.srchWholesaleUsocMatching.ID = "";
      this.srchWholesaleUsocMatching.ID_DescSplitter = ":";
      this.srchWholesaleUsocMatching.Location = new System.Drawing.Point(455, 65);
      this.srchWholesaleUsocMatching.MaxHeight = 228;
      this.srchWholesaleUsocMatching.MustExistInList = false;
      this.srchWholesaleUsocMatching.MustExistMessage = "You must enter a valid value";
      this.srchWholesaleUsocMatching.Name = "srchWholesaleUsocMatching";
      this.srchWholesaleUsocMatching.SearchExec = null;
      this.srchWholesaleUsocMatching.ShowCustomerNameWhenSet = true;
      this.srchWholesaleUsocMatching.ShowTermedCheckBox = false;
      this.srchWholesaleUsocMatching.Size = new System.Drawing.Size(939, 27);
      this.srchWholesaleUsocMatching.TabIndex = 113;
      // 
      // srchRetailUsocMatching
      // 
      this.srchRetailUsocMatching.AddNewMode = false;
      this.srchRetailUsocMatching.AutoAddNewMode = false;
      this.srchRetailUsocMatching.AutoSelectWhenMatch = false;
      this.srchRetailUsocMatching.AutoTabToNextControlOnSelect = true;
      this.srchRetailUsocMatching.ClearSearchOnExpand = false;
      this.srchRetailUsocMatching.ClearSearchWhenComplete = false;
      this.srchRetailUsocMatching.Collapsed = true;
      this.srchRetailUsocMatching.CreatedNewItem = false;
      this.srchRetailUsocMatching.DisplayOnlyDescription = false;
      this.srchRetailUsocMatching.DisplayOnlyID = false;
      this.srchRetailUsocMatching.FixKeySpace = "-1";
      this.srchRetailUsocMatching.ID = "";
      this.srchRetailUsocMatching.ID_DescSplitter = ":";
      this.srchRetailUsocMatching.Location = new System.Drawing.Point(17, 65);
      this.srchRetailUsocMatching.MaxHeight = 228;
      this.srchRetailUsocMatching.MustExistInList = false;
      this.srchRetailUsocMatching.MustExistMessage = "You must enter a valid value";
      this.srchRetailUsocMatching.Name = "srchRetailUsocMatching";
      this.srchRetailUsocMatching.SearchExec = null;
      this.srchRetailUsocMatching.ShowCustomerNameWhenSet = true;
      this.srchRetailUsocMatching.ShowTermedCheckBox = false;
      this.srchRetailUsocMatching.Size = new System.Drawing.Size(327, 27);
      this.srchRetailUsocMatching.TabIndex = 112;
      // 
      // lblUsocMatching
      // 
      this.lblUsocMatching.AutoSize = true;
      this.lblUsocMatching.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblUsocMatching.Location = new System.Drawing.Point(300, 15);
      this.lblUsocMatching.Name = "lblUsocMatching";
      this.lblUsocMatching.Size = new System.Drawing.Size(0, 20);
      this.lblUsocMatching.TabIndex = 12;
      // 
      // lstWholesaleUsocs
      // 
      this.lstWholesaleUsocs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstWholesaleUsocs.FormattingEnabled = true;
      this.lstWholesaleUsocs.Location = new System.Drawing.Point(455, 98);
      this.lstWholesaleUsocs.Name = "lstWholesaleUsocs";
      this.lstWholesaleUsocs.Size = new System.Drawing.Size(939, 173);
      this.lstWholesaleUsocs.TabIndex = 11;
      // 
      // btnUnmatchUsoc
      // 
      this.btnUnmatchUsoc.Location = new System.Drawing.Point(356, 106);
      this.btnUnmatchUsoc.Name = "btnUnmatchUsoc";
      this.btnUnmatchUsoc.Size = new System.Drawing.Size(75, 23);
      this.btnUnmatchUsoc.TabIndex = 10;
      this.btnUnmatchUsoc.Text = "Unmatch";
      this.btnUnmatchUsoc.UseVisualStyleBackColor = true;
      // 
      // btnMatchUsoc
      // 
      this.btnMatchUsoc.Location = new System.Drawing.Point(356, 72);
      this.btnMatchUsoc.Name = "btnMatchUsoc";
      this.btnMatchUsoc.Size = new System.Drawing.Size(75, 23);
      this.btnMatchUsoc.TabIndex = 9;
      this.btnMatchUsoc.Text = "Match";
      this.btnMatchUsoc.UseVisualStyleBackColor = true;
      // 
      // lstRetailUsocs
      // 
      this.lstRetailUsocs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.lstRetailUsocs.FormattingEnabled = true;
      this.lstRetailUsocs.Location = new System.Drawing.Point(17, 98);
      this.lstRetailUsocs.Name = "lstRetailUsocs";
      this.lstRetailUsocs.Size = new System.Drawing.Size(326, 173);
      this.lstRetailUsocs.TabIndex = 8;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.Location = new System.Drawing.Point(13, 12);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(262, 24);
      this.label4.TabIndex = 7;
      this.label4.Text = "Wholesale USOC Matching";
      // 
      // frmCityHostedUSOCMaintenanceNew
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(200)))), ((int)(((byte)(150)))));
      this.ClientSize = new System.Drawing.Size(1420, 653);
      this.Controls.Add(this.tabMaintenance);
      this.Controls.Add(this.cboCarrier);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.cboMode);
      this.Controls.Add(this.lblMode);
      this.Controls.Add(this.srchUSOCList);
      this.Controls.Add(this.ckRefresh);
      this.Controls.Add(this.ckIncludeInactive);
      this.Controls.Add(this.ckIncludeNonSaddleback);
      this.Name = "frmCityHostedUSOCMaintenanceNew";
      this.Text = "frmCityHostedUSOCMaintenanceNew";
      this.Load += new System.EventHandler(this.frmCityHostedUSOCMaintenanceNew_Load);
      this.tabMaintenance.ResumeLayout(false);
      this.tabRetail.ResumeLayout(false);
      this.tabRetail.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdDealerCosts)).EndInit();
      this.tabWholesale.ResumeLayout(false);
      this.tabWholesale.PerformLayout();
      this.tabMatching.ResumeLayout(false);
      this.tabMatching.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox cboCarrier;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cboMode;
    private System.Windows.Forms.Label lblMode;
    private Common.ctlSearchGrid srchUSOCList;
    private System.Windows.Forms.CheckBox ckRefresh;
    private System.Windows.Forms.CheckBox ckIncludeInactive;
    private System.Windows.Forms.CheckBox ckIncludeNonSaddleback;
    private System.Windows.Forms.TabControl tabMaintenance;
    private System.Windows.Forms.TabPage tabRetail;
    private System.Windows.Forms.Button btnRetailNew;
    private System.Windows.Forms.Button btnRetailCancel;
    private System.Windows.Forms.Button btnRetailSave;
    private System.Windows.Forms.CheckBox ckExcludeFromExceptions;
    private System.Windows.Forms.CheckBox ckSaddlebackUSOC;
    private System.Windows.Forms.CheckBox ckRetailInactivate;
    private System.Windows.Forms.CheckBox ckVariableRetailNRC;
    private System.Windows.Forms.CheckBox ckVariableRetailMRC;
    private System.Windows.Forms.Label label3;
    private ACG.CommonForms.ctlSearch txtRetailUSOC;
    private System.Windows.Forms.ComboBox cboTaxCode;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.ComboBox cboRITCategory;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox txtExternalDescription;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.TextBox txtRetailDescription;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.CheckBox ckRetailOnly;
    private System.Windows.Forms.DateTimePicker dtRetailEndDate;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.DateTimePicker dtRetailStartDate;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox txtRetailNRC;
    private System.Windows.Forms.TextBox txtRetailMRC;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.ComboBox cboCHSCategory;
    private System.Windows.Forms.TabPage tabWholesale;
    private System.Windows.Forms.Label label1;
    private ACG.CommonForms.ctlSearch txtWholesaleUSOC;
    private System.Windows.Forms.CheckBox ckWholesaleInactivate;
    private System.Windows.Forms.TextBox txtWholesaleDescription;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.CheckBox ckWholesaleOnly;
    private System.Windows.Forms.DateTimePicker dtWholesaleEndDate;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.DateTimePicker dtWholesaleStartDate;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.TextBox txtWholesaleNRC;
    private System.Windows.Forms.TextBox txtWholesaleMRC;
    private System.Windows.Forms.Button btnWholesaleCancel;
    private System.Windows.Forms.Button btnWholesaleSave;
    private System.Windows.Forms.Button btnWholesaleNew;
    private System.Windows.Forms.TabPage tabMatching;
    private ACG.CommonForms.ctlSearch srchWholesaleUsocMatching;
    private ACG.CommonForms.ctlSearch srchRetailUsocMatching;
    private System.Windows.Forms.Label lblUsocMatching;
    private System.Windows.Forms.ListBox lstWholesaleUsocs;
    private System.Windows.Forms.Button btnUnmatchUsoc;
    private System.Windows.Forms.Button btnMatchUsoc;
    private System.Windows.Forms.ListBox lstRetailUsocs;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label18;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.DataGridView grdDealerCosts;
  }
}