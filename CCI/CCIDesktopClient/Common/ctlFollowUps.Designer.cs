namespace CCI.DesktopClient.Common
{
  partial class ctlFollowUps
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctlFollowUps));
      this.srchFollowUps = new CCI.DesktopClient.Common.ctlSearchGrid();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.txtDescription = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.txtSource = new System.Windows.Forms.TextBox();
      this.cboDispositionCode = new System.Windows.Forms.ComboBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.dtDueDate = new System.Windows.Forms.DateTimePicker();
      this.label7 = new System.Windows.Forms.Label();
      this.dtEffectiveDate = new System.Windows.Forms.DateTimePicker();
      this.label8 = new System.Windows.Forms.Label();
      this.txtCreatedDateTime = new System.Windows.Forms.TextBox();
      this.label9 = new System.Windows.Forms.Label();
      this.txtLastModifiedDateTime = new System.Windows.Forms.TextBox();
      this.ckCompleted = new System.Windows.Forms.CheckBox();
      this.txtCompletedDateTime = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.Label();
      this.txtNotes = new System.Windows.Forms.TextBox();
      this.btnSave = new System.Windows.Forms.Button();
      this.label11 = new System.Windows.Forms.Label();
      this.txtOrder = new System.Windows.Forms.TextBox();
      this.btnAddNew = new System.Windows.Forms.Button();
      this.btnDelete = new System.Windows.Forms.Button();
      this.txtID = new System.Windows.Forms.TextBox();
      this.srchAssignedTo = new ACG.CommonForms.ctlSearch();
      this.srchCustomer = new ACG.CommonForms.ctlSearch();
      this.SuspendLayout();
      // 
      // srchFollowUps
      // 
      this.srchFollowUps.AllowSortByColumn = true;
      this.srchFollowUps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.srchFollowUps.AutoRefreshWhenFieldChecked = true;
      this.srchFollowUps.CanChangeDisplayFields = false;
      this.srchFollowUps.CanChangeDisplaySearchCriteria = true;
      this.srchFollowUps.ColumnName = "CustomerName";
      this.srchFollowUps.DisplayFields = false;
      this.srchFollowUps.DisplaySearchCriteria = false;
      this.srchFollowUps.FieldsDefaultIsChecked = true;
      this.srchFollowUps.ForceReloadSearchColumns = false;
      this.srchFollowUps.InnerWhere = "";
      this.srchFollowUps.Location = new System.Drawing.Point(0, 147);
      this.srchFollowUps.Name = "srchFollowUps";
      this.srchFollowUps.NameType = CCI.Common.CommonData.UnmatchedNameTypes.FollowUps;
      this.srchFollowUps.Size = new System.Drawing.Size(1118, 320);
      this.srchFollowUps.TabIndex = 15;
      this.srchFollowUps.Title = "Search (0 Records Found)";
      this.srchFollowUps.UniqueIdentifier = "ID";
      this.srchFollowUps.RowSelected += new CCI.DesktopClient.Common.ctlSearchGrid.RowSelectedHandler(this.srchFollowUps_RowSelected);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(14, 8);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(66, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Assigned To";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(14, 35);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(51, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Customer";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(537, 35);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(60, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Description";
      // 
      // txtDescription
      // 
      this.txtDescription.Location = new System.Drawing.Point(623, 33);
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new System.Drawing.Size(484, 20);
      this.txtDescription.TabIndex = 8;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(314, 8);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(41, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "Source";
      // 
      // txtSource
      // 
      this.txtSource.Location = new System.Drawing.Point(379, 5);
      this.txtSource.Name = "txtSource";
      this.txtSource.Size = new System.Drawing.Size(141, 20);
      this.txtSource.TabIndex = 3;
      // 
      // cboDispositionCode
      // 
      this.cboDispositionCode.FormattingEnabled = true;
      this.cboDispositionCode.Location = new System.Drawing.Point(379, 33);
      this.cboDispositionCode.Name = "cboDispositionCode";
      this.cboDispositionCode.Size = new System.Drawing.Size(141, 21);
      this.cboDispositionCode.TabIndex = 4;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(314, 35);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(58, 13);
      this.label5.TabIndex = 10;
      this.label5.Text = "Disposition";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(314, 60);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(53, 13);
      this.label6.TabIndex = 11;
      this.label6.Text = "Due Date";
      // 
      // dtDueDate
      // 
      this.dtDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtDueDate.Location = new System.Drawing.Point(381, 60);
      this.dtDueDate.Name = "dtDueDate";
      this.dtDueDate.Size = new System.Drawing.Size(139, 20);
      this.dtDueDate.TabIndex = 5;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(538, 64);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(72, 13);
      this.label7.TabIndex = 13;
      this.label7.Text = "EffectiveDate";
      // 
      // dtEffectiveDate
      // 
      this.dtEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtEffectiveDate.Location = new System.Drawing.Point(616, 60);
      this.dtEffectiveDate.Name = "dtEffectiveDate";
      this.dtEffectiveDate.Size = new System.Drawing.Size(139, 20);
      this.dtEffectiveDate.TabIndex = 9;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(756, 64);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(44, 13);
      this.label8.TabIndex = 15;
      this.label8.Text = "Created";
      // 
      // txtCreatedDateTime
      // 
      this.txtCreatedDateTime.Enabled = false;
      this.txtCreatedDateTime.Location = new System.Drawing.Point(806, 60);
      this.txtCreatedDateTime.Name = "txtCreatedDateTime";
      this.txtCreatedDateTime.Size = new System.Drawing.Size(119, 20);
      this.txtCreatedDateTime.TabIndex = 10;
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(936, 61);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(47, 13);
      this.label9.TabIndex = 17;
      this.label9.Text = "Modified";
      // 
      // txtLastModifiedDateTime
      // 
      this.txtLastModifiedDateTime.Enabled = false;
      this.txtLastModifiedDateTime.Location = new System.Drawing.Point(989, 60);
      this.txtLastModifiedDateTime.Name = "txtLastModifiedDateTime";
      this.txtLastModifiedDateTime.Size = new System.Drawing.Size(119, 20);
      this.txtLastModifiedDateTime.TabIndex = 11;
      // 
      // ckCompleted
      // 
      this.ckCompleted.AutoSize = true;
      this.ckCompleted.Location = new System.Drawing.Point(540, 7);
      this.ckCompleted.Name = "ckCompleted";
      this.ckCompleted.Size = new System.Drawing.Size(76, 17);
      this.ckCompleted.TabIndex = 19;
      this.ckCompleted.Text = "Completed";
      this.ckCompleted.UseVisualStyleBackColor = true;
      this.ckCompleted.CheckedChanged += new System.EventHandler(this.ckCompleted_CheckedChanged);
      // 
      // txtCompletedDateTime
      // 
      this.txtCompletedDateTime.Enabled = false;
      this.txtCompletedDateTime.Location = new System.Drawing.Point(623, 3);
      this.txtCompletedDateTime.Name = "txtCompletedDateTime";
      this.txtCompletedDateTime.Size = new System.Drawing.Size(149, 20);
      this.txtCompletedDateTime.TabIndex = 6;
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(14, 86);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(35, 13);
      this.label10.TabIndex = 21;
      this.label10.Text = "Notes";
      // 
      // txtNotes
      // 
      this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtNotes.Location = new System.Drawing.Point(81, 85);
      this.txtNotes.Multiline = true;
      this.txtNotes.Name = "txtNotes";
      this.txtNotes.Size = new System.Drawing.Size(1026, 62);
      this.txtNotes.TabIndex = 12;
      // 
      // btnSave
      // 
      this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSave.Location = new System.Drawing.Point(1033, 4);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(75, 23);
      this.btnSave.TabIndex = 15;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(16, 64);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(33, 13);
      this.label11.TabIndex = 24;
      this.label11.Text = "Order";
      // 
      // txtOrder
      // 
      this.txtOrder.Location = new System.Drawing.Point(81, 58);
      this.txtOrder.Name = "txtOrder";
      this.txtOrder.Size = new System.Drawing.Size(93, 20);
      this.txtOrder.TabIndex = 2;
      this.txtOrder.Leave += new System.EventHandler(this.txtOrder_Leave);
      // 
      // btnAddNew
      // 
      this.btnAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAddNew.Location = new System.Drawing.Point(871, 4);
      this.btnAddNew.Name = "btnAddNew";
      this.btnAddNew.Size = new System.Drawing.Size(75, 23);
      this.btnAddNew.TabIndex = 13;
      this.btnAddNew.Text = "Add New";
      this.btnAddNew.UseVisualStyleBackColor = true;
      this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
      // 
      // btnDelete
      // 
      this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnDelete.Location = new System.Drawing.Point(952, 4);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(75, 23);
      this.btnDelete.TabIndex = 14;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      // 
      // txtID
      // 
      this.txtID.Enabled = false;
      this.txtID.Location = new System.Drawing.Point(787, 5);
      this.txtID.Name = "txtID";
      this.txtID.Size = new System.Drawing.Size(72, 20);
      this.txtID.TabIndex = 7;
      // 
      // srchAssignedTo
      // 
      this.srchAssignedTo.AddNewMode = false;
      this.srchAssignedTo.AutoSelectWhenMatch = false;
      this.srchAssignedTo.ClearSearchWhenComplete = false;
      this.srchAssignedTo.Collapsed = true;
      this.srchAssignedTo.DisplayOnlyID = false;
      this.srchAssignedTo.ID = "";
      this.srchAssignedTo.Location = new System.Drawing.Point(78, 6);
      this.srchAssignedTo.MaxHeight = 228;
      this.srchAssignedTo.Name = "srchAssignedTo";
      this.srchAssignedTo.ShowCustomerNameWhenSet = true;
      this.srchAssignedTo.ShowTermedCheckBox = false;
      this.srchAssignedTo.Size = new System.Drawing.Size(217, 21);
      this.srchAssignedTo.TabIndex = 0;
      // 
      // srchCustomer
      // 
      this.srchCustomer.AddNewMode = false;
      this.srchCustomer.AutoSelectWhenMatch = false;
      this.srchCustomer.ClearSearchWhenComplete = false;
      this.srchCustomer.Collapsed = true;
      this.srchCustomer.DisplayOnlyID = false;
      this.srchCustomer.ID = "";
      this.srchCustomer.Location = new System.Drawing.Point(78, 32);
      this.srchCustomer.MaxHeight = 228;
      this.srchCustomer.Name = "srchCustomer";
      this.srchCustomer.ShowCustomerNameWhenSet = true;
      this.srchCustomer.ShowTermedCheckBox = false;
      this.srchCustomer.Size = new System.Drawing.Size(217, 21);
      this.srchCustomer.TabIndex = 1;
      // 
      // ctlFollowUps
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.srchAssignedTo);
      this.Controls.Add(this.srchCustomer);
      this.Controls.Add(this.txtID);
      this.Controls.Add(this.btnDelete);
      this.Controls.Add(this.btnAddNew);
      this.Controls.Add(this.txtOrder);
      this.Controls.Add(this.label11);
      this.Controls.Add(this.btnSave);
      this.Controls.Add(this.txtNotes);
      this.Controls.Add(this.label10);
      this.Controls.Add(this.txtCompletedDateTime);
      this.Controls.Add(this.ckCompleted);
      this.Controls.Add(this.txtLastModifiedDateTime);
      this.Controls.Add(this.label9);
      this.Controls.Add(this.txtCreatedDateTime);
      this.Controls.Add(this.label8);
      this.Controls.Add(this.dtEffectiveDate);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.dtDueDate);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.cboDispositionCode);
      this.Controls.Add(this.txtSource);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.txtDescription);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.srchFollowUps);
      this.Name = "ctlFollowUps";
      this.Size = new System.Drawing.Size(1119, 468);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private ctlSearchGrid srchFollowUps;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox txtSource;
    private System.Windows.Forms.ComboBox cboDispositionCode;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.DateTimePicker dtDueDate;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.DateTimePicker dtEffectiveDate;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.TextBox txtCreatedDateTime;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.TextBox txtLastModifiedDateTime;
    private System.Windows.Forms.CheckBox ckCompleted;
    private System.Windows.Forms.TextBox txtCompletedDateTime;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox txtNotes;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.TextBox txtOrder;
    private System.Windows.Forms.Button btnAddNew;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.TextBox txtID;
    private ACG.CommonForms.ctlSearch srchAssignedTo;
    private ACG.CommonForms.ctlSearch srchCustomer;
  }
}
