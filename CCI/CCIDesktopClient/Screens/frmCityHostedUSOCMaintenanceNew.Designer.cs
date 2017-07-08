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
      this.splitMain = new System.Windows.Forms.SplitContainer();
      this.cboCarrier = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.cboMode = new System.Windows.Forms.ComboBox();
      this.lblMode = new System.Windows.Forms.Label();
      this.srchUSOCList = new CCI.DesktopClient.Common.ctlSearchGrid();
      this.ckRefresh = new System.Windows.Forms.CheckBox();
      this.ckIncludeInactive = new System.Windows.Forms.CheckBox();
      this.ckIncludeNonSaddleback = new System.Windows.Forms.CheckBox();
      this.pnlRetail = new System.Windows.Forms.Panel();
      this.label3 = new System.Windows.Forms.Label();
      this.txtRetailUSOC = new ACG.CommonForms.ctlSearch();
      this.cboTaxCode = new System.Windows.Forms.ComboBox();
      this.label12 = new System.Windows.Forms.Label();
      this.ckRetailInactivate = new System.Windows.Forms.CheckBox();
      this.cboRITCategory = new System.Windows.Forms.ComboBox();
      this.label5 = new System.Windows.Forms.Label();
      this.ckVariableRetailNRC = new System.Windows.Forms.CheckBox();
      this.ckVariableRetailMRC = new System.Windows.Forms.CheckBox();
      this.txtExternalDescription = new System.Windows.Forms.TextBox();
      this.label11 = new System.Windows.Forms.Label();
      this.ckSaddlebackUSOC = new System.Windows.Forms.CheckBox();
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
      this.ckExcludeFromExceptions = new System.Windows.Forms.CheckBox();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnSave = new System.Windows.Forms.Button();
      this.btnNew = new System.Windows.Forms.Button();
      this.pnlMatching = new System.Windows.Forms.Panel();
      this.lblUsocMatching = new System.Windows.Forms.Label();
      this.lstWholesaleUsocs = new System.Windows.Forms.ListBox();
      this.btnUnmatchUsoc = new System.Windows.Forms.Button();
      this.btnMatchUsoc = new System.Windows.Forms.Button();
      this.lstRetailUsocs = new System.Windows.Forms.ListBox();
      this.label4 = new System.Windows.Forms.Label();
      this.pnlWholesale = new System.Windows.Forms.Panel();
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
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      this.splitMain.Panel1.SuspendLayout();
      this.splitMain.Panel2.SuspendLayout();
      this.splitMain.SuspendLayout();
      this.pnlRetail.SuspendLayout();
      this.pnlMatching.SuspendLayout();
      this.pnlWholesale.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitMain
      // 
      this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitMain.Location = new System.Drawing.Point(0, 0);
      this.splitMain.Name = "splitMain";
      this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitMain.Panel1
      // 
      this.splitMain.Panel1.Controls.Add(this.cboCarrier);
      this.splitMain.Panel1.Controls.Add(this.label2);
      this.splitMain.Panel1.Controls.Add(this.cboMode);
      this.splitMain.Panel1.Controls.Add(this.lblMode);
      this.splitMain.Panel1.Controls.Add(this.srchUSOCList);
      this.splitMain.Panel1.Controls.Add(this.ckRefresh);
      this.splitMain.Panel1.Controls.Add(this.ckIncludeInactive);
      this.splitMain.Panel1.Controls.Add(this.ckIncludeNonSaddleback);
      // 
      // splitMain.Panel2
      // 
      this.splitMain.Panel2.Controls.Add(this.pnlMatching);
      this.splitMain.Panel2.Controls.Add(this.pnlWholesale);
      this.splitMain.Size = new System.Drawing.Size(1171, 691);
      this.splitMain.SplitterDistance = 353;
      this.splitMain.TabIndex = 0;
      // 
      // cboCarrier
      // 
      this.cboCarrier.FormattingEnabled = true;
      this.cboCarrier.Location = new System.Drawing.Point(585, 1);
      this.cboCarrier.Name = "cboCarrier";
      this.cboCarrier.Size = new System.Drawing.Size(121, 21);
      this.cboCarrier.TabIndex = 10;
      this.cboCarrier.Text = "Saddleback";
      this.cboCarrier.SelectedIndexChanged += new System.EventHandler(this.cboCarrier_SelectedIndexChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(542, 4);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(37, 13);
      this.label2.TabIndex = 9;
      this.label2.Text = "Carrier";
      // 
      // cboMode
      // 
      this.cboMode.FormattingEnabled = true;
      this.cboMode.Items.AddRange(new object[] {
            "Edit Wholesale",
            "Edit Retail",
            "Match Retail/Wholesale"});
      this.cboMode.Location = new System.Drawing.Point(394, 1);
      this.cboMode.Name = "cboMode";
      this.cboMode.Size = new System.Drawing.Size(132, 21);
      this.cboMode.TabIndex = 8;
      this.cboMode.Text = "Edit Retail";
      this.cboMode.SelectedIndexChanged += new System.EventHandler(this.cboMode_SelectedIndexChanged);
      // 
      // lblMode
      // 
      this.lblMode.AutoSize = true;
      this.lblMode.Location = new System.Drawing.Point(354, 4);
      this.lblMode.Name = "lblMode";
      this.lblMode.Size = new System.Drawing.Size(34, 13);
      this.lblMode.TabIndex = 7;
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
      this.srchUSOCList.Location = new System.Drawing.Point(-3, 25);
      this.srchUSOCList.Name = "srchUSOCList";
      this.srchUSOCList.NameType = CCI.Common.CommonData.UnmatchedNameTypes.Customer;
      this.srchUSOCList.SearchCriteria = null;
      this.srchUSOCList.Size = new System.Drawing.Size(1171, 325);
      this.srchUSOCList.TabIndex = 6;
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
      this.ckRefresh.Location = new System.Drawing.Point(260, 3);
      this.ckRefresh.Name = "ckRefresh";
      this.ckRefresh.Size = new System.Drawing.Size(88, 17);
      this.ckRefresh.TabIndex = 5;
      this.ckRefresh.Text = "Auto Refresh";
      this.ckRefresh.UseVisualStyleBackColor = true;
      this.ckRefresh.CheckedChanged += new System.EventHandler(this.ckRefresh_CheckedChanged);
      // 
      // ckIncludeInactive
      // 
      this.ckIncludeInactive.AutoSize = true;
      this.ckIncludeInactive.Location = new System.Drawing.Point(122, 3);
      this.ckIncludeInactive.Name = "ckIncludeInactive";
      this.ckIncludeInactive.Size = new System.Drawing.Size(132, 17);
      this.ckIncludeInactive.TabIndex = 4;
      this.ckIncludeInactive.Text = "Include Inactive Retail";
      this.ckIncludeInactive.UseVisualStyleBackColor = true;
      this.ckIncludeInactive.CheckedChanged += new System.EventHandler(this.ckIncludeInactive_CheckedChanged);
      // 
      // ckIncludeNonSaddleback
      // 
      this.ckIncludeNonSaddleback.AutoSize = true;
      this.ckIncludeNonSaddleback.Checked = true;
      this.ckIncludeNonSaddleback.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckIncludeNonSaddleback.Location = new System.Drawing.Point(3, 3);
      this.ckIncludeNonSaddleback.Name = "ckIncludeNonSaddleback";
      this.ckIncludeNonSaddleback.Size = new System.Drawing.Size(113, 17);
      this.ckIncludeNonSaddleback.TabIndex = 3;
      this.ckIncludeNonSaddleback.Text = "Include All USOCs";
      this.ckIncludeNonSaddleback.UseVisualStyleBackColor = true;
      // 
      // pnlRetail
      // 
      this.pnlRetail.Controls.Add(this.label3);
      this.pnlRetail.Controls.Add(this.txtRetailUSOC);
      this.pnlRetail.Controls.Add(this.cboTaxCode);
      this.pnlRetail.Controls.Add(this.label12);
      this.pnlRetail.Controls.Add(this.ckRetailInactivate);
      this.pnlRetail.Controls.Add(this.cboRITCategory);
      this.pnlRetail.Controls.Add(this.label5);
      this.pnlRetail.Controls.Add(this.ckVariableRetailNRC);
      this.pnlRetail.Controls.Add(this.ckVariableRetailMRC);
      this.pnlRetail.Controls.Add(this.txtExternalDescription);
      this.pnlRetail.Controls.Add(this.label11);
      this.pnlRetail.Controls.Add(this.ckSaddlebackUSOC);
      this.pnlRetail.Controls.Add(this.txtRetailDescription);
      this.pnlRetail.Controls.Add(this.label6);
      this.pnlRetail.Controls.Add(this.ckRetailOnly);
      this.pnlRetail.Controls.Add(this.dtRetailEndDate);
      this.pnlRetail.Controls.Add(this.label8);
      this.pnlRetail.Controls.Add(this.dtRetailStartDate);
      this.pnlRetail.Controls.Add(this.label9);
      this.pnlRetail.Controls.Add(this.label10);
      this.pnlRetail.Controls.Add(this.txtRetailNRC);
      this.pnlRetail.Controls.Add(this.txtRetailMRC);
      this.pnlRetail.Controls.Add(this.label7);
      this.pnlRetail.Controls.Add(this.cboCHSCategory);
      this.pnlRetail.Controls.Add(this.ckExcludeFromExceptions);
      this.pnlRetail.Controls.Add(this.btnCancel);
      this.pnlRetail.Controls.Add(this.btnSave);
      this.pnlRetail.Controls.Add(this.btnNew);
      this.pnlRetail.Location = new System.Drawing.Point(34, 15);
      this.pnlRetail.Name = "pnlRetail";
      this.pnlRetail.Size = new System.Drawing.Size(610, 325);
      this.pnlRetail.TabIndex = 1;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(13, 9);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(251, 24);
      this.label3.TabIndex = 1;
      this.label3.Text = "Retail USOC Maintenance";
      // 
      // txtRetailUSOC
      // 
      this.txtRetailUSOC.AddNewMode = false;
      this.txtRetailUSOC.AutoAddNewMode = false;
      this.txtRetailUSOC.AutoSelectWhenMatch = false;
      this.txtRetailUSOC.AutoTabToNextControlOnSelect = true;
      this.txtRetailUSOC.ClearSearchWhenComplete = false;
      this.txtRetailUSOC.Collapsed = true;
      this.txtRetailUSOC.CreatedNewItem = false;
      this.txtRetailUSOC.DisplayOnlyDescription = false;
      this.txtRetailUSOC.DisplayOnlyID = false;
      this.txtRetailUSOC.FixKeySpace = "-1";
      this.txtRetailUSOC.ID = "";
      this.txtRetailUSOC.ID_DescSplitter = ":";
      this.txtRetailUSOC.Location = new System.Drawing.Point(154, 36);
      this.txtRetailUSOC.MaxHeight = 228;
      this.txtRetailUSOC.MustExistInList = false;
      this.txtRetailUSOC.MustExistMessage = "You must enter a valid value";
      this.txtRetailUSOC.Name = "txtRetailUSOC";
      this.txtRetailUSOC.SearchExec = null;
      this.txtRetailUSOC.ShowCustomerNameWhenSet = true;
      this.txtRetailUSOC.ShowTermedCheckBox = false;
      this.txtRetailUSOC.Size = new System.Drawing.Size(434, 19);
      this.txtRetailUSOC.TabIndex = 82;
      // 
      // cboTaxCode
      // 
      this.cboTaxCode.FormattingEnabled = true;
      this.cboTaxCode.Location = new System.Drawing.Point(153, 216);
      this.cboTaxCode.Name = "cboTaxCode";
      this.cboTaxCode.Size = new System.Drawing.Size(176, 21);
      this.cboTaxCode.TabIndex = 103;
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(14, 220);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(68, 13);
      this.label12.TabIndex = 104;
      this.label12.Text = "RIT TranTax";
      // 
      // ckRetailInactivate
      // 
      this.ckRetailInactivate.AutoSize = true;
      this.ckRetailInactivate.Location = new System.Drawing.Point(367, 167);
      this.ckRetailInactivate.Name = "ckRetailInactivate";
      this.ckRetailInactivate.Size = new System.Drawing.Size(64, 17);
      this.ckRetailInactivate.TabIndex = 92;
      this.ckRetailInactivate.Text = "Inactive";
      this.ckRetailInactivate.UseVisualStyleBackColor = true;
      // 
      // cboRITCategory
      // 
      this.cboRITCategory.FormattingEnabled = true;
      this.cboRITCategory.Location = new System.Drawing.Point(153, 191);
      this.cboRITCategory.Name = "cboRITCategory";
      this.cboRITCategory.Size = new System.Drawing.Size(176, 21);
      this.cboRITCategory.TabIndex = 94;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(14, 195);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(70, 13);
      this.label5.TabIndex = 102;
      this.label5.Text = "RIT Category";
      // 
      // ckVariableRetailNRC
      // 
      this.ckVariableRetailNRC.AutoSize = true;
      this.ckVariableRetailNRC.Location = new System.Drawing.Point(342, 135);
      this.ckVariableRetailNRC.Name = "ckVariableRetailNRC";
      this.ckVariableRetailNRC.Size = new System.Drawing.Size(64, 17);
      this.ckVariableRetailNRC.TabIndex = 88;
      this.ckVariableRetailNRC.Text = "Variable";
      this.ckVariableRetailNRC.UseVisualStyleBackColor = true;
      // 
      // ckVariableRetailMRC
      // 
      this.ckVariableRetailMRC.AutoSize = true;
      this.ckVariableRetailMRC.Location = new System.Drawing.Point(218, 135);
      this.ckVariableRetailMRC.Name = "ckVariableRetailMRC";
      this.ckVariableRetailMRC.Size = new System.Drawing.Size(64, 17);
      this.ckVariableRetailMRC.TabIndex = 86;
      this.ckVariableRetailMRC.Text = "Variable";
      this.ckVariableRetailMRC.UseVisualStyleBackColor = true;
      // 
      // txtExternalDescription
      // 
      this.txtExternalDescription.Location = new System.Drawing.Point(153, 102);
      this.txtExternalDescription.Name = "txtExternalDescription";
      this.txtExternalDescription.Size = new System.Drawing.Size(436, 20);
      this.txtExternalDescription.TabIndex = 84;
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(14, 105);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(133, 13);
      this.label11.TabIndex = 101;
      this.label11.Text = "RIT Retail Bill Presentation";
      // 
      // ckSaddlebackUSOC
      // 
      this.ckSaddlebackUSOC.AccessibleDescription = " ";
      this.ckSaddlebackUSOC.AutoSize = true;
      this.ckSaddlebackUSOC.Location = new System.Drawing.Point(473, 167);
      this.ckSaddlebackUSOC.Name = "ckSaddlebackUSOC";
      this.ckSaddlebackUSOC.Size = new System.Drawing.Size(116, 17);
      this.ckSaddlebackUSOC.TabIndex = 93;
      this.ckSaddlebackUSOC.Text = "Saddleback USOC";
      this.ckSaddlebackUSOC.UseVisualStyleBackColor = true;
      // 
      // txtRetailDescription
      // 
      this.txtRetailDescription.Location = new System.Drawing.Point(153, 69);
      this.txtRetailDescription.Name = "txtRetailDescription";
      this.txtRetailDescription.Size = new System.Drawing.Size(436, 20);
      this.txtRetailDescription.TabIndex = 83;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(15, 40);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(67, 13);
      this.label6.TabIndex = 97;
      this.label6.Text = "Retail USOC";
      // 
      // ckRetailOnly
      // 
      this.ckRetailOnly.AutoSize = true;
      this.ckRetailOnly.Location = new System.Drawing.Point(512, 135);
      this.ckRetailOnly.Name = "ckRetailOnly";
      this.ckRetailOnly.Size = new System.Drawing.Size(77, 17);
      this.ckRetailOnly.TabIndex = 89;
      this.ckRetailOnly.Text = "Retail Only";
      this.ckRetailOnly.UseVisualStyleBackColor = true;
      // 
      // dtRetailEndDate
      // 
      this.dtRetailEndDate.Checked = false;
      this.dtRetailEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtRetailEndDate.Location = new System.Drawing.Point(259, 165);
      this.dtRetailEndDate.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
      this.dtRetailEndDate.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtRetailEndDate.Name = "dtRetailEndDate";
      this.dtRetailEndDate.RightToLeftLayout = true;
      this.dtRetailEndDate.ShowCheckBox = true;
      this.dtRetailEndDate.Size = new System.Drawing.Size(98, 20);
      this.dtRetailEndDate.TabIndex = 91;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(14, 71);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(60, 13);
      this.label8.TabIndex = 98;
      this.label8.Text = "Description";
      // 
      // dtRetailStartDate
      // 
      this.dtRetailStartDate.Checked = false;
      this.dtRetailStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtRetailStartDate.Location = new System.Drawing.Point(153, 165);
      this.dtRetailStartDate.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
      this.dtRetailStartDate.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtRetailStartDate.Name = "dtRetailStartDate";
      this.dtRetailStartDate.ShowCheckBox = true;
      this.dtRetailStartDate.Size = new System.Drawing.Size(98, 20);
      this.dtRetailStartDate.TabIndex = 90;
      this.dtRetailStartDate.Value = new System.DateTime(2012, 12, 7, 9, 11, 0, 0);
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(14, 137);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(89, 13);
      this.label9.TabIndex = 99;
      this.label9.Text = "Retail MRC/NRC";
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(14, 169);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(79, 13);
      this.label10.TabIndex = 100;
      this.label10.Text = "Start/End Date";
      // 
      // txtRetailNRC
      // 
      this.txtRetailNRC.Location = new System.Drawing.Point(279, 133);
      this.txtRetailNRC.Name = "txtRetailNRC";
      this.txtRetailNRC.Size = new System.Drawing.Size(57, 20);
      this.txtRetailNRC.TabIndex = 87;
      // 
      // txtRetailMRC
      // 
      this.txtRetailMRC.Location = new System.Drawing.Point(153, 133);
      this.txtRetailMRC.Name = "txtRetailMRC";
      this.txtRetailMRC.Size = new System.Drawing.Size(59, 20);
      this.txtRetailMRC.TabIndex = 85;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(332, 195);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(74, 13);
      this.label7.TabIndex = 72;
      this.label7.Text = "CHS Category";
      // 
      // cboCHSCategory
      // 
      this.cboCHSCategory.FormattingEnabled = true;
      this.cboCHSCategory.Location = new System.Drawing.Point(412, 191);
      this.cboCHSCategory.Name = "cboCHSCategory";
      this.cboCHSCategory.Size = new System.Drawing.Size(176, 21);
      this.cboCHSCategory.TabIndex = 95;
      // 
      // ckExcludeFromExceptions
      // 
      this.ckExcludeFromExceptions.AutoSize = true;
      this.ckExcludeFromExceptions.Location = new System.Drawing.Point(417, 218);
      this.ckExcludeFromExceptions.Name = "ckExcludeFromExceptions";
      this.ckExcludeFromExceptions.Size = new System.Drawing.Size(172, 17);
      this.ckExcludeFromExceptions.TabIndex = 69;
      this.ckExcludeFromExceptions.Text = "Exclude from Retail Exceptions";
      this.ckExcludeFromExceptions.UseVisualStyleBackColor = true;
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(100, 248);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(77, 23);
      this.btnCancel.TabIndex = 81;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnSave
      // 
      this.btnSave.Location = new System.Drawing.Point(183, 248);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(77, 23);
      this.btnSave.TabIndex = 70;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      // 
      // btnNew
      // 
      this.btnNew.Location = new System.Drawing.Point(17, 248);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new System.Drawing.Size(77, 23);
      this.btnNew.TabIndex = 71;
      this.btnNew.Text = "Add New";
      this.btnNew.UseVisualStyleBackColor = true;
      // 
      // pnlMatching
      // 
      this.pnlMatching.Controls.Add(this.pnlRetail);
      this.pnlMatching.Controls.Add(this.lblUsocMatching);
      this.pnlMatching.Controls.Add(this.lstWholesaleUsocs);
      this.pnlMatching.Controls.Add(this.btnUnmatchUsoc);
      this.pnlMatching.Controls.Add(this.btnMatchUsoc);
      this.pnlMatching.Controls.Add(this.lstRetailUsocs);
      this.pnlMatching.Controls.Add(this.label4);
      this.pnlMatching.Location = new System.Drawing.Point(0, 0);
      this.pnlMatching.Name = "pnlMatching";
      this.pnlMatching.Size = new System.Drawing.Size(824, 306);
      this.pnlMatching.TabIndex = 2;
      // 
      // lblUsocMatching
      // 
      this.lblUsocMatching.AutoSize = true;
      this.lblUsocMatching.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblUsocMatching.Location = new System.Drawing.Point(298, 15);
      this.lblUsocMatching.Name = "lblUsocMatching";
      this.lblUsocMatching.Size = new System.Drawing.Size(0, 20);
      this.lblUsocMatching.TabIndex = 6;
      // 
      // lstWholesaleUsocs
      // 
      this.lstWholesaleUsocs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstWholesaleUsocs.FormattingEnabled = true;
      this.lstWholesaleUsocs.Location = new System.Drawing.Point(453, 59);
      this.lstWholesaleUsocs.Name = "lstWholesaleUsocs";
      this.lstWholesaleUsocs.Size = new System.Drawing.Size(355, 238);
      this.lstWholesaleUsocs.TabIndex = 5;
      // 
      // btnUnmatchUsoc
      // 
      this.btnUnmatchUsoc.Location = new System.Drawing.Point(354, 106);
      this.btnUnmatchUsoc.Name = "btnUnmatchUsoc";
      this.btnUnmatchUsoc.Size = new System.Drawing.Size(75, 23);
      this.btnUnmatchUsoc.TabIndex = 4;
      this.btnUnmatchUsoc.Text = "Unmatch";
      this.btnUnmatchUsoc.UseVisualStyleBackColor = true;
      // 
      // btnMatchUsoc
      // 
      this.btnMatchUsoc.Location = new System.Drawing.Point(354, 72);
      this.btnMatchUsoc.Name = "btnMatchUsoc";
      this.btnMatchUsoc.Size = new System.Drawing.Size(75, 23);
      this.btnMatchUsoc.TabIndex = 3;
      this.btnMatchUsoc.Text = "Match";
      this.btnMatchUsoc.UseVisualStyleBackColor = true;
      // 
      // lstRetailUsocs
      // 
      this.lstRetailUsocs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.lstRetailUsocs.FormattingEnabled = true;
      this.lstRetailUsocs.Location = new System.Drawing.Point(15, 59);
      this.lstRetailUsocs.Name = "lstRetailUsocs";
      this.lstRetailUsocs.Size = new System.Drawing.Size(326, 238);
      this.lstRetailUsocs.TabIndex = 2;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.Location = new System.Drawing.Point(11, 12);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(262, 24);
      this.label4.TabIndex = 1;
      this.label4.Text = "Wholesale USOC Matching";
      // 
      // pnlWholesale
      // 
      this.pnlWholesale.Controls.Add(this.label1);
      this.pnlWholesale.Controls.Add(this.txtWholesaleUSOC);
      this.pnlWholesale.Controls.Add(this.ckWholesaleInactivate);
      this.pnlWholesale.Controls.Add(this.txtWholesaleDescription);
      this.pnlWholesale.Controls.Add(this.label13);
      this.pnlWholesale.Controls.Add(this.ckWholesaleOnly);
      this.pnlWholesale.Controls.Add(this.dtWholesaleEndDate);
      this.pnlWholesale.Controls.Add(this.label14);
      this.pnlWholesale.Controls.Add(this.dtWholesaleStartDate);
      this.pnlWholesale.Controls.Add(this.label15);
      this.pnlWholesale.Controls.Add(this.label16);
      this.pnlWholesale.Controls.Add(this.txtWholesaleNRC);
      this.pnlWholesale.Controls.Add(this.txtWholesaleMRC);
      this.pnlWholesale.Controls.Add(this.button1);
      this.pnlWholesale.Controls.Add(this.button2);
      this.pnlWholesale.Controls.Add(this.button3);
      this.pnlWholesale.Location = new System.Drawing.Point(302, 6);
      this.pnlWholesale.Name = "pnlWholesale";
      this.pnlWholesale.Size = new System.Drawing.Size(567, 322);
      this.pnlWholesale.TabIndex = 3;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(16, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(297, 24);
      this.label1.TabIndex = 0;
      this.label1.Text = "Wholesale USOC Maintenance";
      // 
      // txtWholesaleUSOC
      // 
      this.txtWholesaleUSOC.AddNewMode = false;
      this.txtWholesaleUSOC.AutoAddNewMode = false;
      this.txtWholesaleUSOC.AutoSelectWhenMatch = false;
      this.txtWholesaleUSOC.AutoTabToNextControlOnSelect = true;
      this.txtWholesaleUSOC.ClearSearchWhenComplete = false;
      this.txtWholesaleUSOC.Collapsed = true;
      this.txtWholesaleUSOC.CreatedNewItem = false;
      this.txtWholesaleUSOC.DisplayOnlyDescription = false;
      this.txtWholesaleUSOC.DisplayOnlyID = false;
      this.txtWholesaleUSOC.FixKeySpace = "-1";
      this.txtWholesaleUSOC.ID = "";
      this.txtWholesaleUSOC.ID_DescSplitter = ":";
      this.txtWholesaleUSOC.Location = new System.Drawing.Point(112, 37);
      this.txtWholesaleUSOC.MaxHeight = 228;
      this.txtWholesaleUSOC.MustExistInList = false;
      this.txtWholesaleUSOC.MustExistMessage = "You must enter a valid value";
      this.txtWholesaleUSOC.Name = "txtWholesaleUSOC";
      this.txtWholesaleUSOC.SearchExec = null;
      this.txtWholesaleUSOC.ShowCustomerNameWhenSet = true;
      this.txtWholesaleUSOC.ShowTermedCheckBox = false;
      this.txtWholesaleUSOC.Size = new System.Drawing.Size(434, 19);
      this.txtWholesaleUSOC.TabIndex = 27;
      // 
      // ckWholesaleInactivate
      // 
      this.ckWholesaleInactivate.AutoSize = true;
      this.ckWholesaleInactivate.Location = new System.Drawing.Point(319, 134);
      this.ckWholesaleInactivate.Name = "ckWholesaleInactivate";
      this.ckWholesaleInactivate.Size = new System.Drawing.Size(64, 17);
      this.ckWholesaleInactivate.TabIndex = 36;
      this.ckWholesaleInactivate.Text = "Inactive";
      this.ckWholesaleInactivate.UseVisualStyleBackColor = true;
      // 
      // txtWholesaleDescription
      // 
      this.txtWholesaleDescription.Location = new System.Drawing.Point(113, 70);
      this.txtWholesaleDescription.Name = "txtWholesaleDescription";
      this.txtWholesaleDescription.Size = new System.Drawing.Size(436, 20);
      this.txtWholesaleDescription.TabIndex = 28;
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(16, 39);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(90, 13);
      this.label13.TabIndex = 32;
      this.label13.Text = "Wholesale USOC";
      // 
      // ckWholesaleOnly
      // 
      this.ckWholesaleOnly.AutoSize = true;
      this.ckWholesaleOnly.Location = new System.Drawing.Point(437, 104);
      this.ckWholesaleOnly.Name = "ckWholesaleOnly";
      this.ckWholesaleOnly.Size = new System.Drawing.Size(100, 17);
      this.ckWholesaleOnly.TabIndex = 31;
      this.ckWholesaleOnly.Text = "Wholesale Only";
      this.ckWholesaleOnly.UseVisualStyleBackColor = true;
      // 
      // dtWholesaleEndDate
      // 
      this.dtWholesaleEndDate.Checked = false;
      this.dtWholesaleEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtWholesaleEndDate.Location = new System.Drawing.Point(215, 134);
      this.dtWholesaleEndDate.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
      this.dtWholesaleEndDate.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtWholesaleEndDate.Name = "dtWholesaleEndDate";
      this.dtWholesaleEndDate.RightToLeftLayout = true;
      this.dtWholesaleEndDate.ShowCheckBox = true;
      this.dtWholesaleEndDate.Size = new System.Drawing.Size(98, 20);
      this.dtWholesaleEndDate.TabIndex = 34;
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(16, 72);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(60, 13);
      this.label14.TabIndex = 37;
      this.label14.Text = "Description";
      // 
      // dtWholesaleStartDate
      // 
      this.dtWholesaleStartDate.Checked = false;
      this.dtWholesaleStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtWholesaleStartDate.Location = new System.Drawing.Point(112, 134);
      this.dtWholesaleStartDate.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
      this.dtWholesaleStartDate.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtWholesaleStartDate.Name = "dtWholesaleStartDate";
      this.dtWholesaleStartDate.ShowCheckBox = true;
      this.dtWholesaleStartDate.Size = new System.Drawing.Size(98, 20);
      this.dtWholesaleStartDate.TabIndex = 33;
      this.dtWholesaleStartDate.Value = new System.DateTime(2012, 12, 7, 9, 11, 0, 0);
      // 
      // label15
      // 
      this.label15.AutoSize = true;
      this.label15.Location = new System.Drawing.Point(15, 101);
      this.label15.Name = "label15";
      this.label15.Size = new System.Drawing.Size(92, 13);
      this.label15.TabIndex = 35;
      this.label15.Text = "Whsle MRC/NRC";
      // 
      // label16
      // 
      this.label16.AutoSize = true;
      this.label16.Location = new System.Drawing.Point(15, 138);
      this.label16.Name = "label16";
      this.label16.Size = new System.Drawing.Size(79, 13);
      this.label16.TabIndex = 38;
      this.label16.Text = "Start/End Date";
      // 
      // txtWholesaleNRC
      // 
      this.txtWholesaleNRC.Location = new System.Drawing.Point(215, 101);
      this.txtWholesaleNRC.Name = "txtWholesaleNRC";
      this.txtWholesaleNRC.Size = new System.Drawing.Size(90, 20);
      this.txtWholesaleNRC.TabIndex = 30;
      // 
      // txtWholesaleMRC
      // 
      this.txtWholesaleMRC.Location = new System.Drawing.Point(112, 101);
      this.txtWholesaleMRC.Name = "txtWholesaleMRC";
      this.txtWholesaleMRC.Size = new System.Drawing.Size(87, 20);
      this.txtWholesaleMRC.TabIndex = 29;
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(158, 169);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(77, 23);
      this.button1.TabIndex = 57;
      this.button1.Text = "Cancel";
      this.button1.UseVisualStyleBackColor = true;
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(254, 169);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(77, 23);
      this.button2.TabIndex = 55;
      this.button2.Text = "Save";
      this.button2.UseVisualStyleBackColor = true;
      // 
      // button3
      // 
      this.button3.Location = new System.Drawing.Point(29, 165);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(77, 23);
      this.button3.TabIndex = 56;
      this.button3.Text = "Add New";
      this.button3.UseVisualStyleBackColor = true;
      // 
      // frmCityHostedUSOCMaintenanceNew
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1171, 691);
      this.Controls.Add(this.splitMain);
      this.Name = "frmCityHostedUSOCMaintenanceNew";
      this.Text = "frmCityHostedUSOCMaintenanceNew";
      this.Load += new System.EventHandler(this.frmCityHostedUSOCMaintenanceNew_Load);
      this.splitMain.Panel1.ResumeLayout(false);
      this.splitMain.Panel1.PerformLayout();
      this.splitMain.Panel2.ResumeLayout(false);
      this.splitMain.ResumeLayout(false);
      this.pnlRetail.ResumeLayout(false);
      this.pnlRetail.PerformLayout();
      this.pnlMatching.ResumeLayout(false);
      this.pnlMatching.PerformLayout();
      this.pnlWholesale.ResumeLayout(false);
      this.pnlWholesale.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitMain;
    private System.Windows.Forms.CheckBox ckRefresh;
    private System.Windows.Forms.CheckBox ckIncludeInactive;
    private System.Windows.Forms.CheckBox ckIncludeNonSaddleback;
    private Common.ctlSearchGrid srchUSOCList;
    private System.Windows.Forms.ComboBox cboMode;
    private System.Windows.Forms.Label lblMode;
    private System.Windows.Forms.ComboBox cboCarrier;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Panel pnlMatching;
    private System.Windows.Forms.Panel pnlRetail;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.CheckBox ckExcludeFromExceptions;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Label label7;
    private ACG.CommonForms.ctlSearch txtRetailUSOC;
    private System.Windows.Forms.ComboBox cboTaxCode;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.CheckBox ckRetailInactivate;
    private System.Windows.Forms.ComboBox cboRITCategory;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.CheckBox ckVariableRetailNRC;
    private System.Windows.Forms.CheckBox ckVariableRetailMRC;
    private System.Windows.Forms.TextBox txtExternalDescription;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.CheckBox ckSaddlebackUSOC;
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
    private System.Windows.Forms.ComboBox cboCHSCategory;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Panel pnlWholesale;
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
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button btnMatchUsoc;
    private System.Windows.Forms.ListBox lstRetailUsocs;
    private System.Windows.Forms.Button btnUnmatchUsoc;
    private System.Windows.Forms.ListBox lstWholesaleUsocs;
    private System.Windows.Forms.Label lblUsocMatching;
  }
}