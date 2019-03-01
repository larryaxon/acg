namespace CCI.DesktopClient.Screens
{
  partial class frmEntityMaintenance
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
      this.ctlEntitySearch1 = new ACG.CommonForms.ctlSearch();
      this.txtEndDate = new ACG.CommonForms.ctlACGDate();
      this.txtStartDate = new ACG.CommonForms.ctlACGDate();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.tabMain = new System.Windows.Forms.TabControl();
      this.tabLocations = new System.Windows.Forms.TabPage();
      this.ctlLocations1 = new CCI.DesktopClient.Common.ctlContacts();
      this.txtStatus = new System.Windows.Forms.ComboBox();
      this.lblStatus = new System.Windows.Forms.Label();
      this.grpAddress = new System.Windows.Forms.GroupBox();
      this.btnCreateLocation = new System.Windows.Forms.Button();
      this.txtCellPhone = new System.Windows.Forms.TextBox();
      this.lblCell = new System.Windows.Forms.Label();
      this.txtPhone = new System.Windows.Forms.TextBox();
      this.lblPhone = new System.Windows.Forms.Label();
      this.txtZip = new System.Windows.Forms.TextBox();
      this.txtState = new System.Windows.Forms.ComboBox();
      this.txtCity = new System.Windows.Forms.TextBox();
      this.txtAddress2 = new System.Windows.Forms.TextBox();
      this.txtAddress1 = new System.Windows.Forms.TextBox();
      this.lblCity = new System.Windows.Forms.Label();
      this.lblAddress2 = new System.Windows.Forms.Label();
      this.lblAddress1 = new System.Windows.Forms.Label();
      this.btnCancelNew = new System.Windows.Forms.Button();
      this.lblNewRecord = new System.Windows.Forms.Label();
      this.pnlLoading = new System.Windows.Forms.Panel();
      this.lblLoading = new System.Windows.Forms.Label();
      this.btnNew = new System.Windows.Forms.Button();
      this.btnSave = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.lblEntity = new System.Windows.Forms.Label();
      this.txtEntity = new System.Windows.Forms.TextBox();
      this.txtLegalName = new System.Windows.Forms.TextBox();
      this.tabMain.SuspendLayout();
      this.tabLocations.SuspendLayout();
      this.grpAddress.SuspendLayout();
      this.pnlLoading.SuspendLayout();
      this.SuspendLayout();
      // 
      // ctlEntitySearch1
      // 
      this.ctlEntitySearch1.AddNewMode = false;
      this.ctlEntitySearch1.AutoAddNewMode = false;
      this.ctlEntitySearch1.AutoSelectWhenMatch = false;
      this.ctlEntitySearch1.AutoTabToNextControlOnSelect = true;
      this.ctlEntitySearch1.ClearSearchOnExpand = false;
      this.ctlEntitySearch1.ClearSearchWhenComplete = true;
      this.ctlEntitySearch1.Collapsed = true;
      this.ctlEntitySearch1.CreatedNewItem = false;
      this.ctlEntitySearch1.DisplayOnlyDescription = false;
      this.ctlEntitySearch1.DisplayOnlyID = false;
      this.ctlEntitySearch1.FixKeySpace = "-1";
      this.ctlEntitySearch1.ID = "";
      this.ctlEntitySearch1.ID_DescSplitter = ":";
      this.ctlEntitySearch1.Location = new System.Drawing.Point(62, 3);
      this.ctlEntitySearch1.MaxHeight = 228;
      this.ctlEntitySearch1.MustExistInList = false;
      this.ctlEntitySearch1.MustExistMessage = "You must enter a valid value";
      this.ctlEntitySearch1.Name = "ctlEntitySearch1";
      this.ctlEntitySearch1.SearchExec = null;
      this.ctlEntitySearch1.ShowCustomerNameWhenSet = false;
      this.ctlEntitySearch1.ShowTermedCheckBox = true;
      this.ctlEntitySearch1.Size = new System.Drawing.Size(302, 24);
      this.ctlEntitySearch1.TabIndex = 0;
      this.ctlEntitySearch1.OnSelected += new System.EventHandler<System.EventArgs>(this.ctlEntitySearch1_OnSelected);
      // 
      // txtEndDate
      // 
      this.txtEndDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.txtEndDate.Format = "d";
      this.txtEndDate.Location = new System.Drawing.Point(932, 29);
      this.txtEndDate.Name = "txtEndDate";
      this.txtEndDate.Size = new System.Drawing.Size(125, 17);
      this.txtEndDate.TabIndex = 4;
      this.txtEndDate.Value = null;
      // 
      // txtStartDate
      // 
      this.txtStartDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.txtStartDate.Format = "d";
      this.txtStartDate.Location = new System.Drawing.Point(743, 29);
      this.txtStartDate.Name = "txtStartDate";
      this.txtStartDate.Size = new System.Drawing.Size(125, 17);
      this.txtStartDate.TabIndex = 3;
      this.txtStartDate.Value = null;
      // 
      // label3
      // 
      this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(874, 31);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(52, 13);
      this.label3.TabIndex = 17;
      this.label3.Text = "End Date";
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(684, 32);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(55, 13);
      this.label2.TabIndex = 15;
      this.label2.Text = "Start Date";
      // 
      // tabMain
      // 
      this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabMain.Controls.Add(this.tabLocations);
      this.tabMain.Location = new System.Drawing.Point(382, 52);
      this.tabMain.Name = "tabMain";
      this.tabMain.SelectedIndex = 0;
      this.tabMain.Size = new System.Drawing.Size(809, 142);
      this.tabMain.TabIndex = 10;
      // 
      // tabLocations
      // 
      this.tabLocations.Controls.Add(this.ctlLocations1);
      this.tabLocations.Location = new System.Drawing.Point(4, 22);
      this.tabLocations.Name = "tabLocations";
      this.tabLocations.Padding = new System.Windows.Forms.Padding(3);
      this.tabLocations.Size = new System.Drawing.Size(801, 116);
      this.tabLocations.TabIndex = 1;
      this.tabLocations.Text = "Locations";
      this.tabLocations.UseVisualStyleBackColor = true;
      // 
      // ctlLocations1
      // 
      this.ctlLocations1.ColumnNames = new string[] {
        "ContactType",
        "FirstName",
        "LegalName",
        "Phone",
        "CellPhone",
        "EmailAddress",
        "Address1",
        "Address2",
        "City",
        "State",
        "Zip",
        "Entity",
        "EntityOwner",
        "EntityType"};
      this.ctlLocations1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ctlLocations1.EntityOwner = null;
      this.ctlLocations1.EntityOwnerType = null;
      this.ctlLocations1.EntityType = "Location";
      this.ctlLocations1.IncludeGrandChildren = false;
      this.ctlLocations1.Location = new System.Drawing.Point(3, 3);
      this.ctlLocations1.Name = "ctlLocations1";
      this.ctlLocations1.SecurityContext = null;
      this.ctlLocations1.Size = new System.Drawing.Size(795, 110);
      this.ctlLocations1.StateList = null;
      this.ctlLocations1.TabIndex = 1;
      // 
      // txtStatus
      // 
      this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.txtStatus.FormattingEnabled = true;
      this.txtStatus.Items.AddRange(new object[] {
            "Active",
            "Inactive"});
      this.txtStatus.Location = new System.Drawing.Point(1111, 29);
      this.txtStatus.Name = "txtStatus";
      this.txtStatus.Size = new System.Drawing.Size(81, 21);
      this.txtStatus.TabIndex = 5;
      this.txtStatus.Tag = "Master";
      this.txtStatus.SelectedIndexChanged += new System.EventHandler(this.txtStatus_SelectedIndexChanged);
      this.txtStatus.SelectedValueChanged += new System.EventHandler(this.txtStatus_SelectedValueChanged);
      // 
      // lblStatus
      // 
      this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblStatus.AutoSize = true;
      this.lblStatus.Location = new System.Drawing.Point(1066, 31);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new System.Drawing.Size(40, 13);
      this.lblStatus.TabIndex = 13;
      this.lblStatus.Text = "Status:";
      // 
      // grpAddress
      // 
      this.grpAddress.Controls.Add(this.btnCreateLocation);
      this.grpAddress.Controls.Add(this.txtCellPhone);
      this.grpAddress.Controls.Add(this.lblCell);
      this.grpAddress.Controls.Add(this.txtPhone);
      this.grpAddress.Controls.Add(this.lblPhone);
      this.grpAddress.Controls.Add(this.txtZip);
      this.grpAddress.Controls.Add(this.txtState);
      this.grpAddress.Controls.Add(this.txtCity);
      this.grpAddress.Controls.Add(this.txtAddress2);
      this.grpAddress.Controls.Add(this.txtAddress1);
      this.grpAddress.Controls.Add(this.lblCity);
      this.grpAddress.Controls.Add(this.lblAddress2);
      this.grpAddress.Controls.Add(this.lblAddress1);
      this.grpAddress.Location = new System.Drawing.Point(10, 58);
      this.grpAddress.Name = "grpAddress";
      this.grpAddress.Size = new System.Drawing.Size(364, 136);
      this.grpAddress.TabIndex = 9;
      this.grpAddress.TabStop = false;
      this.grpAddress.Text = "Address";
      // 
      // btnCreateLocation
      // 
      this.btnCreateLocation.Location = new System.Drawing.Point(69, 111);
      this.btnCreateLocation.Name = "btnCreateLocation";
      this.btnCreateLocation.Size = new System.Drawing.Size(284, 24);
      this.btnCreateLocation.TabIndex = 12;
      this.btnCreateLocation.Text = "Create new Location from this Address";
      this.btnCreateLocation.UseVisualStyleBackColor = true;
      this.btnCreateLocation.Click += new System.EventHandler(this.btnCreateLocation_Click);
      // 
      // txtCellPhone
      // 
      this.txtCellPhone.Location = new System.Drawing.Point(230, 92);
      this.txtCellPhone.Name = "txtCellPhone";
      this.txtCellPhone.Size = new System.Drawing.Size(125, 20);
      this.txtCellPhone.TabIndex = 6;
      this.txtCellPhone.TextChanged += new System.EventHandler(this.txtCellPhone_TextChanged);
      // 
      // lblCell
      // 
      this.lblCell.AutoSize = true;
      this.lblCell.Location = new System.Drawing.Point(200, 95);
      this.lblCell.Name = "lblCell";
      this.lblCell.Size = new System.Drawing.Size(24, 13);
      this.lblCell.TabIndex = 11;
      this.lblCell.Text = "Cell";
      // 
      // txtPhone
      // 
      this.txtPhone.Location = new System.Drawing.Point(67, 92);
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new System.Drawing.Size(127, 20);
      this.txtPhone.TabIndex = 5;
      this.txtPhone.TextChanged += new System.EventHandler(this.txtPhone_TextChanged);
      // 
      // lblPhone
      // 
      this.lblPhone.AutoSize = true;
      this.lblPhone.Location = new System.Drawing.Point(8, 95);
      this.lblPhone.Name = "lblPhone";
      this.lblPhone.Size = new System.Drawing.Size(38, 13);
      this.lblPhone.TabIndex = 8;
      this.lblPhone.Text = "Phone";
      // 
      // txtZip
      // 
      this.txtZip.Location = new System.Drawing.Point(310, 66);
      this.txtZip.Name = "txtZip";
      this.txtZip.Size = new System.Drawing.Size(46, 20);
      this.txtZip.TabIndex = 4;
      this.txtZip.TextChanged += new System.EventHandler(this.txtZip_TextChanged);
      // 
      // txtState
      // 
      this.txtState.FormattingEnabled = true;
      this.txtState.Location = new System.Drawing.Point(261, 65);
      this.txtState.Name = "txtState";
      this.txtState.Size = new System.Drawing.Size(42, 21);
      this.txtState.TabIndex = 3;
      this.txtState.SelectedIndexChanged += new System.EventHandler(this.txtState_SelectedIndexChanged);
      // 
      // txtCity
      // 
      this.txtCity.Location = new System.Drawing.Point(67, 66);
      this.txtCity.Name = "txtCity";
      this.txtCity.Size = new System.Drawing.Size(184, 20);
      this.txtCity.TabIndex = 2;
      this.txtCity.TextChanged += new System.EventHandler(this.txtCity_TextChanged);
      // 
      // txtAddress2
      // 
      this.txtAddress2.Location = new System.Drawing.Point(67, 41);
      this.txtAddress2.Name = "txtAddress2";
      this.txtAddress2.Size = new System.Drawing.Size(288, 20);
      this.txtAddress2.TabIndex = 1;
      this.txtAddress2.TextChanged += new System.EventHandler(this.txtAddress2_TextChanged);
      // 
      // txtAddress1
      // 
      this.txtAddress1.Location = new System.Drawing.Point(67, 16);
      this.txtAddress1.Name = "txtAddress1";
      this.txtAddress1.Size = new System.Drawing.Size(288, 20);
      this.txtAddress1.TabIndex = 0;
      this.txtAddress1.TextChanged += new System.EventHandler(this.txtAddress1_TextChanged);
      // 
      // lblCity
      // 
      this.lblCity.AutoSize = true;
      this.lblCity.Location = new System.Drawing.Point(8, 69);
      this.lblCity.Name = "lblCity";
      this.lblCity.Size = new System.Drawing.Size(59, 13);
      this.lblCity.TabIndex = 2;
      this.lblCity.Text = "City/St/Zip";
      // 
      // lblAddress2
      // 
      this.lblAddress2.AutoSize = true;
      this.lblAddress2.Location = new System.Drawing.Point(8, 44);
      this.lblAddress2.Name = "lblAddress2";
      this.lblAddress2.Size = new System.Drawing.Size(54, 13);
      this.lblAddress2.TabIndex = 1;
      this.lblAddress2.Text = "Address 2";
      // 
      // lblAddress1
      // 
      this.lblAddress1.AutoSize = true;
      this.lblAddress1.Location = new System.Drawing.Point(8, 23);
      this.lblAddress1.Name = "lblAddress1";
      this.lblAddress1.Size = new System.Drawing.Size(54, 13);
      this.lblAddress1.TabIndex = 0;
      this.lblAddress1.Text = "Address 1";
      // 
      // btnCancelNew
      // 
      this.btnCancelNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancelNew.Location = new System.Drawing.Point(1009, 3);
      this.btnCancelNew.Name = "btnCancelNew";
      this.btnCancelNew.Size = new System.Drawing.Size(58, 23);
      this.btnCancelNew.TabIndex = 6;
      this.btnCancelNew.Text = "Cancel";
      this.btnCancelNew.UseVisualStyleBackColor = true;
      this.btnCancelNew.Click += new System.EventHandler(this.btnCancelNew_Click);
      // 
      // lblNewRecord
      // 
      this.lblNewRecord.AutoSize = true;
      this.lblNewRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblNewRecord.ForeColor = System.Drawing.Color.Red;
      this.lblNewRecord.Location = new System.Drawing.Point(370, 3);
      this.lblNewRecord.Name = "lblNewRecord";
      this.lblNewRecord.Size = new System.Drawing.Size(158, 20);
      this.lblNewRecord.TabIndex = 10;
      this.lblNewRecord.Text = "*** New Record ***";
      this.lblNewRecord.Visible = false;
      // 
      // pnlLoading
      // 
      this.pnlLoading.Controls.Add(this.lblLoading);
      this.pnlLoading.Location = new System.Drawing.Point(426, 55);
      this.pnlLoading.Name = "pnlLoading";
      this.pnlLoading.Size = new System.Drawing.Size(271, 108);
      this.pnlLoading.TabIndex = 9;
      this.pnlLoading.Visible = false;
      // 
      // lblLoading
      // 
      this.lblLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblLoading.AutoSize = true;
      this.lblLoading.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblLoading.Location = new System.Drawing.Point(92, 57);
      this.lblLoading.Name = "lblLoading";
      this.lblLoading.Size = new System.Drawing.Size(93, 24);
      this.lblLoading.TabIndex = 0;
      this.lblLoading.Text = "Loading...";
      // 
      // btnNew
      // 
      this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnNew.Location = new System.Drawing.Point(1073, 3);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new System.Drawing.Size(58, 23);
      this.btnNew.TabIndex = 7;
      this.btnNew.Text = "New";
      this.btnNew.UseVisualStyleBackColor = true;
      this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
      // 
      // btnSave
      // 
      this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSave.Location = new System.Drawing.Point(1137, 3);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(55, 23);
      this.btnSave.TabIndex = 8;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(4, 8);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(44, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Search:";
      // 
      // lblEntity
      // 
      this.lblEntity.AutoSize = true;
      this.lblEntity.Location = new System.Drawing.Point(4, 31);
      this.lblEntity.Name = "lblEntity";
      this.lblEntity.Size = new System.Drawing.Size(52, 13);
      this.lblEntity.TabIndex = 8;
      this.lblEntity.Text = "Prospect:";
      // 
      // txtEntity
      // 
      this.txtEntity.Enabled = false;
      this.txtEntity.Location = new System.Drawing.Point(62, 29);
      this.txtEntity.Name = "txtEntity";
      this.txtEntity.Size = new System.Drawing.Size(104, 20);
      this.txtEntity.TabIndex = 1;
      // 
      // txtLegalName
      // 
      this.txtLegalName.Location = new System.Drawing.Point(172, 29);
      this.txtLegalName.Name = "txtLegalName";
      this.txtLegalName.Size = new System.Drawing.Size(135, 20);
      this.txtLegalName.TabIndex = 2;
      this.txtLegalName.Tag = "";
      this.txtLegalName.TextChanged += new System.EventHandler(this.txtLegalName_TextChanged);
      // 
      // frmEntityMaintenance
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1196, 569);
      this.Controls.Add(this.ctlEntitySearch1);
      this.Controls.Add(this.txtEndDate);
      this.Controls.Add(this.txtStartDate);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.tabMain);
      this.Controls.Add(this.txtStatus);
      this.Controls.Add(this.lblStatus);
      this.Controls.Add(this.grpAddress);
      this.Controls.Add(this.btnCancelNew);
      this.Controls.Add(this.lblNewRecord);
      this.Controls.Add(this.pnlLoading);
      this.Controls.Add(this.btnNew);
      this.Controls.Add(this.btnSave);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.lblEntity);
      this.Controls.Add(this.txtEntity);
      this.Controls.Add(this.txtLegalName);
      this.Name = "frmEntityMaintenance";
      this.Text = "frmEntityMaintenance";
      this.Load += new System.EventHandler(this.frmEntityMaintenance_Load);
      this.Leave += new System.EventHandler(this.frmEntityMaintenance_Leave);
      this.Validating += new System.ComponentModel.CancelEventHandler(this.frmEntityMaintenance_Validating);
      this.tabMain.ResumeLayout(false);
      this.tabLocations.ResumeLayout(false);
      this.grpAddress.ResumeLayout(false);
      this.grpAddress.PerformLayout();
      this.pnlLoading.ResumeLayout(false);
      this.pnlLoading.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    protected System.Windows.Forms.TextBox txtLegalName;
    private System.Windows.Forms.GroupBox grpAddress;
    protected System.Windows.Forms.TextBox txtAddress1;
    private System.Windows.Forms.Label lblCity;
    private System.Windows.Forms.Label lblAddress2;
    private System.Windows.Forms.Label lblAddress1;
    protected System.Windows.Forms.TextBox txtCity;
    protected System.Windows.Forms.TextBox txtAddress2;
    protected System.Windows.Forms.TextBox txtZip;
    protected System.Windows.Forms.ComboBox txtState;
    private System.Windows.Forms.Label lblPhone;
    protected System.Windows.Forms.TextBox txtPhone;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Panel pnlLoading;
    private System.Windows.Forms.Label lblLoading;
    private System.Windows.Forms.Button btnNew;
    protected System.Windows.Forms.Label lblEntity;
    protected System.Windows.Forms.TextBox txtEntity;
    private System.Windows.Forms.Button btnCancelNew;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.ComboBox txtStatus;
    protected System.Windows.Forms.TextBox txtCellPhone;
    private System.Windows.Forms.Label lblCell;
    protected ACG.CommonForms.ctlSearch ctlEntitySearch1;
    protected System.Windows.Forms.TabControl tabMain;
    protected System.Windows.Forms.TabPage tabLocations;
    protected System.Windows.Forms.Label lblNewRecord;
    protected Common.ctlContacts ctlLocations1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private ACG.CommonForms.ctlACGDate txtStartDate;
    private ACG.CommonForms.ctlACGDate txtEndDate;
    private System.Windows.Forms.Button btnCreateLocation;
  }
}