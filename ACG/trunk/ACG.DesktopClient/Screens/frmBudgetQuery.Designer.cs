namespace ACG.DesktopClient.Screens
{
  partial class frmBudgetQuery
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBudgetQuery));
      this.btnFlagBudget = new System.Windows.Forms.Button();
      this.btnSetThroughDate = new System.Windows.Forms.Button();
      this.dtBudgetThroughDate = new System.Windows.Forms.DateTimePicker();
      this.label14 = new System.Windows.Forms.Label();
      this.cboCustomer = new System.Windows.Forms.ComboBox();
      this.label10 = new System.Windows.Forms.Label();
      this.tabMain = new System.Windows.Forms.TabControl();
      this.tabSummary = new System.Windows.Forms.TabPage();
      this.splitBudgetSummary = new System.Windows.Forms.SplitContainer();
      this.srchBudgetSummary = new ACG.DesktopClient.Common.ctlSearchGrid();
      this.tabEditBudgetDetail = new System.Windows.Forms.TabControl();
      this.tabEditBudget = new System.Windows.Forms.TabPage();
      this.splitEditBudget = new System.Windows.Forms.SplitContainer();
      this.srchEditBudget = new ACG.DesktopClient.Common.ctlSearchGrid();
      this.txtBudgetRate = new System.Windows.Forms.TextBox();
      this.label15 = new System.Windows.Forms.Label();
      this.btnBudgetSave = new System.Windows.Forms.Button();
      this.dtEstimatedCompletionDate = new System.Windows.Forms.DateTimePicker();
      this.txtOriginalCompletionDate = new System.Windows.Forms.TextBox();
      this.txtEstimatedHoursToComplete = new System.Windows.Forms.TextBox();
      this.txtRevisedBudgetHours = new System.Windows.Forms.TextBox();
      this.txtOriginalBudgetHours = new System.Windows.Forms.TextBox();
      this.label13 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.label8 = new System.Windows.Forms.Label();
      this.txtBudgetCustomerProject = new System.Windows.Forms.TextBox();
      this.tabEditTime = new System.Windows.Forms.TabPage();
      this.splitEditTime = new System.Windows.Forms.SplitContainer();
      this.srchEditTime = new ACG.DesktopClient.Common.ctlSearchGrid();
      this.btnTimeSave = new System.Windows.Forms.Button();
      this.txtTimeDescription = new System.Windows.Forms.TextBox();
      this.txtInvoiceDate = new System.Windows.Forms.TextBox();
      this.txtInvoiceNumber = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.txtBilledAmount = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.txtTimeRate = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.txtBilledHours = new System.Windows.Forms.TextBox();
      this.txtEnteredHours = new System.Windows.Forms.TextBox();
      this.dtTimeWorkedDate = new System.Windows.Forms.DateTimePicker();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.cboBillingCode = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.txtTimeCustomerProject = new System.Windows.Forms.TextBox();
      this.tabDetail = new System.Windows.Forms.TabPage();
      this.srchBudgetQuery = new ACG.DesktopClient.Common.ctlSearchGrid();
      this.tabMain.SuspendLayout();
      this.tabSummary.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitBudgetSummary)).BeginInit();
      this.splitBudgetSummary.Panel1.SuspendLayout();
      this.splitBudgetSummary.Panel2.SuspendLayout();
      this.splitBudgetSummary.SuspendLayout();
      this.tabEditBudgetDetail.SuspendLayout();
      this.tabEditBudget.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitEditBudget)).BeginInit();
      this.splitEditBudget.Panel1.SuspendLayout();
      this.splitEditBudget.Panel2.SuspendLayout();
      this.splitEditBudget.SuspendLayout();
      this.tabEditTime.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitEditTime)).BeginInit();
      this.splitEditTime.Panel1.SuspendLayout();
      this.splitEditTime.Panel2.SuspendLayout();
      this.splitEditTime.SuspendLayout();
      this.tabDetail.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnFlagBudget
      // 
      this.btnFlagBudget.Location = new System.Drawing.Point(572, 2);
      this.btnFlagBudget.Name = "btnFlagBudget";
      this.btnFlagBudget.Size = new System.Drawing.Size(156, 23);
      this.btnFlagBudget.TabIndex = 7;
      this.btnFlagBudget.Text = "Flag Budget as Distributed";
      this.btnFlagBudget.UseVisualStyleBackColor = true;
      this.btnFlagBudget.Click += new System.EventHandler(this.btnFlagBudget_Click);
      // 
      // btnSetThroughDate
      // 
      this.btnSetThroughDate.Location = new System.Drawing.Point(454, 2);
      this.btnSetThroughDate.Name = "btnSetThroughDate";
      this.btnSetThroughDate.Size = new System.Drawing.Size(112, 23);
      this.btnSetThroughDate.TabIndex = 6;
      this.btnSetThroughDate.Text = "Set Through Date";
      this.btnSetThroughDate.UseVisualStyleBackColor = true;
      this.btnSetThroughDate.Click += new System.EventHandler(this.btnSetThroughDate_Click);
      // 
      // dtBudgetThroughDate
      // 
      this.dtBudgetThroughDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtBudgetThroughDate.Location = new System.Drawing.Point(350, 5);
      this.dtBudgetThroughDate.Name = "dtBudgetThroughDate";
      this.dtBudgetThroughDate.Size = new System.Drawing.Size(85, 20);
      this.dtBudgetThroughDate.TabIndex = 5;
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(263, 7);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(73, 13);
      this.label14.TabIndex = 4;
      this.label14.Text = "Through Date";
      // 
      // cboCustomer
      // 
      this.cboCustomer.FormattingEnabled = true;
      this.cboCustomer.Location = new System.Drawing.Point(65, 5);
      this.cboCustomer.Name = "cboCustomer";
      this.cboCustomer.Size = new System.Drawing.Size(184, 21);
      this.cboCustomer.TabIndex = 3;
      this.cboCustomer.SelectedIndexChanged += new System.EventHandler(this.cboCustomer_SelectedIndexChanged);
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(4, 7);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(51, 13);
      this.label10.TabIndex = 2;
      this.label10.Text = "Customer";
      // 
      // tabMain
      // 
      this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabMain.Controls.Add(this.tabSummary);
      this.tabMain.Controls.Add(this.tabDetail);
      this.tabMain.Location = new System.Drawing.Point(0, 26);
      this.tabMain.Name = "tabMain";
      this.tabMain.SelectedIndex = 0;
      this.tabMain.Size = new System.Drawing.Size(1142, 550);
      this.tabMain.TabIndex = 1;
      // 
      // tabSummary
      // 
      this.tabSummary.Controls.Add(this.splitBudgetSummary);
      this.tabSummary.Location = new System.Drawing.Point(4, 22);
      this.tabSummary.Name = "tabSummary";
      this.tabSummary.Padding = new System.Windows.Forms.Padding(3);
      this.tabSummary.Size = new System.Drawing.Size(1134, 524);
      this.tabSummary.TabIndex = 0;
      this.tabSummary.Text = "Summary";
      this.tabSummary.UseVisualStyleBackColor = true;
      // 
      // splitBudgetSummary
      // 
      this.splitBudgetSummary.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitBudgetSummary.Location = new System.Drawing.Point(3, 3);
      this.splitBudgetSummary.Name = "splitBudgetSummary";
      this.splitBudgetSummary.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitBudgetSummary.Panel1
      // 
      this.splitBudgetSummary.Panel1.Controls.Add(this.srchBudgetSummary);
      // 
      // splitBudgetSummary.Panel2
      // 
      this.splitBudgetSummary.Panel2.Controls.Add(this.tabEditBudgetDetail);
      this.splitBudgetSummary.Size = new System.Drawing.Size(1128, 518);
      this.splitBudgetSummary.SplitterDistance = 212;
      this.splitBudgetSummary.TabIndex = 1;
      // 
      // srchBudgetSummary
      // 
      this.srchBudgetSummary.CanChangeDisplaySearchCriteria = true;
      this.srchBudgetSummary.CanSearchLockedColumns = false;
      this.srchBudgetSummary.ColumnName = "CustomerName";
      this.srchBudgetSummary.DisplaySearchCriteria = false;
      this.srchBudgetSummary.Dock = System.Windows.Forms.DockStyle.Fill;
      this.srchBudgetSummary.ExportCriteria = false;
      this.srchBudgetSummary.InnerWhere = "";
      this.srchBudgetSummary.Location = new System.Drawing.Point(0, 0);
      this.srchBudgetSummary.Name = "srchBudgetSummary";
      this.srchBudgetSummary.NameType = ACG.App.Common.ACGCommonData.NameTypes.Customer;
      this.srchBudgetSummary.SearchCriteria = ((System.Collections.Generic.Dictionary<string, string[]>)(resources.GetObject("srchBudgetSummary.SearchCriteria")));
      this.srchBudgetSummary.Size = new System.Drawing.Size(1128, 212);
      this.srchBudgetSummary.TabIndex = 0;
      this.srchBudgetSummary.Title = "Search (0 Records Found)";
      this.srchBudgetSummary.UniqueIdentifier = "ID";
      this.srchBudgetSummary.RowSelected += new ACG.DesktopClient.Common.ctlSearchGrid.RowSelectedHandler(this.srchBudgetSummary_RowSelected);
      // 
      // tabEditBudgetDetail
      // 
      this.tabEditBudgetDetail.Controls.Add(this.tabEditBudget);
      this.tabEditBudgetDetail.Controls.Add(this.tabEditTime);
      this.tabEditBudgetDetail.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabEditBudgetDetail.Location = new System.Drawing.Point(0, 0);
      this.tabEditBudgetDetail.Name = "tabEditBudgetDetail";
      this.tabEditBudgetDetail.SelectedIndex = 0;
      this.tabEditBudgetDetail.Size = new System.Drawing.Size(1128, 302);
      this.tabEditBudgetDetail.TabIndex = 0;
      // 
      // tabEditBudget
      // 
      this.tabEditBudget.Controls.Add(this.splitEditBudget);
      this.tabEditBudget.Location = new System.Drawing.Point(4, 22);
      this.tabEditBudget.Name = "tabEditBudget";
      this.tabEditBudget.Padding = new System.Windows.Forms.Padding(3);
      this.tabEditBudget.Size = new System.Drawing.Size(1120, 276);
      this.tabEditBudget.TabIndex = 0;
      this.tabEditBudget.Text = "Budget";
      this.tabEditBudget.UseVisualStyleBackColor = true;
      // 
      // splitEditBudget
      // 
      this.splitEditBudget.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitEditBudget.Location = new System.Drawing.Point(3, 3);
      this.splitEditBudget.Name = "splitEditBudget";
      // 
      // splitEditBudget.Panel1
      // 
      this.splitEditBudget.Panel1.Controls.Add(this.srchEditBudget);
      // 
      // splitEditBudget.Panel2
      // 
      this.splitEditBudget.Panel2.Controls.Add(this.txtBudgetRate);
      this.splitEditBudget.Panel2.Controls.Add(this.label15);
      this.splitEditBudget.Panel2.Controls.Add(this.btnBudgetSave);
      this.splitEditBudget.Panel2.Controls.Add(this.dtEstimatedCompletionDate);
      this.splitEditBudget.Panel2.Controls.Add(this.txtOriginalCompletionDate);
      this.splitEditBudget.Panel2.Controls.Add(this.txtEstimatedHoursToComplete);
      this.splitEditBudget.Panel2.Controls.Add(this.txtRevisedBudgetHours);
      this.splitEditBudget.Panel2.Controls.Add(this.txtOriginalBudgetHours);
      this.splitEditBudget.Panel2.Controls.Add(this.label13);
      this.splitEditBudget.Panel2.Controls.Add(this.label12);
      this.splitEditBudget.Panel2.Controls.Add(this.label11);
      this.splitEditBudget.Panel2.Controls.Add(this.label9);
      this.splitEditBudget.Panel2.Controls.Add(this.label8);
      this.splitEditBudget.Panel2.Controls.Add(this.txtBudgetCustomerProject);
      this.splitEditBudget.Size = new System.Drawing.Size(1114, 270);
      this.splitEditBudget.SplitterDistance = 811;
      this.splitEditBudget.TabIndex = 0;
      // 
      // srchEditBudget
      // 
      this.srchEditBudget.CanChangeDisplaySearchCriteria = false;
      this.srchEditBudget.CanSearchLockedColumns = false;
      this.srchEditBudget.ColumnName = "CustomerName";
      this.srchEditBudget.DisplaySearchCriteria = false;
      this.srchEditBudget.Dock = System.Windows.Forms.DockStyle.Fill;
      this.srchEditBudget.ExportCriteria = false;
      this.srchEditBudget.InnerWhere = "";
      this.srchEditBudget.Location = new System.Drawing.Point(0, 0);
      this.srchEditBudget.Name = "srchEditBudget";
      this.srchEditBudget.NameType = ACG.App.Common.ACGCommonData.NameTypes.Customer;
      this.srchEditBudget.SearchCriteria = ((System.Collections.Generic.Dictionary<string, string[]>)(resources.GetObject("srchEditBudget.SearchCriteria")));
      this.srchEditBudget.Size = new System.Drawing.Size(811, 270);
      this.srchEditBudget.TabIndex = 0;
      this.srchEditBudget.Title = "Search (0 Records Found)";
      this.srchEditBudget.UniqueIdentifier = "ID";
      this.srchEditBudget.RowSelected += new ACG.DesktopClient.Common.ctlSearchGrid.RowSelectedHandler(this.srchEditBudget_RowSelected);
      // 
      // txtBudgetRate
      // 
      this.txtBudgetRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtBudgetRate.Location = new System.Drawing.Point(111, 129);
      this.txtBudgetRate.Name = "txtBudgetRate";
      this.txtBudgetRate.Size = new System.Drawing.Size(185, 20);
      this.txtBudgetRate.TabIndex = 17;
      // 
      // label15
      // 
      this.label15.AutoSize = true;
      this.label15.Location = new System.Drawing.Point(3, 132);
      this.label15.Name = "label15";
      this.label15.Size = new System.Drawing.Size(30, 13);
      this.label15.TabIndex = 16;
      this.label15.Text = "Rate";
      // 
      // btnBudgetSave
      // 
      this.btnBudgetSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnBudgetSave.Location = new System.Drawing.Point(220, 166);
      this.btnBudgetSave.Name = "btnBudgetSave";
      this.btnBudgetSave.Size = new System.Drawing.Size(75, 23);
      this.btnBudgetSave.TabIndex = 13;
      this.btnBudgetSave.Text = "Save";
      this.btnBudgetSave.UseVisualStyleBackColor = true;
      this.btnBudgetSave.Click += new System.EventHandler(this.btnBudgetSave_Click);
      // 
      // dtEstimatedCompletionDate
      // 
      this.dtEstimatedCompletionDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dtEstimatedCompletionDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtEstimatedCompletionDate.Location = new System.Drawing.Point(111, 108);
      this.dtEstimatedCompletionDate.Name = "dtEstimatedCompletionDate";
      this.dtEstimatedCompletionDate.Size = new System.Drawing.Size(185, 20);
      this.dtEstimatedCompletionDate.TabIndex = 12;
      // 
      // txtOriginalCompletionDate
      // 
      this.txtOriginalCompletionDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtOriginalCompletionDate.Enabled = false;
      this.txtOriginalCompletionDate.Location = new System.Drawing.Point(111, 87);
      this.txtOriginalCompletionDate.Name = "txtOriginalCompletionDate";
      this.txtOriginalCompletionDate.Size = new System.Drawing.Size(185, 20);
      this.txtOriginalCompletionDate.TabIndex = 11;
      // 
      // txtEstimatedHoursToComplete
      // 
      this.txtEstimatedHoursToComplete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtEstimatedHoursToComplete.Location = new System.Drawing.Point(111, 66);
      this.txtEstimatedHoursToComplete.Name = "txtEstimatedHoursToComplete";
      this.txtEstimatedHoursToComplete.Size = new System.Drawing.Size(185, 20);
      this.txtEstimatedHoursToComplete.TabIndex = 10;
      // 
      // txtRevisedBudgetHours
      // 
      this.txtRevisedBudgetHours.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtRevisedBudgetHours.Location = new System.Drawing.Point(111, 45);
      this.txtRevisedBudgetHours.Name = "txtRevisedBudgetHours";
      this.txtRevisedBudgetHours.Size = new System.Drawing.Size(185, 20);
      this.txtRevisedBudgetHours.TabIndex = 9;
      // 
      // txtOriginalBudgetHours
      // 
      this.txtOriginalBudgetHours.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtOriginalBudgetHours.Enabled = false;
      this.txtOriginalBudgetHours.Location = new System.Drawing.Point(111, 24);
      this.txtOriginalBudgetHours.Name = "txtOriginalBudgetHours";
      this.txtOriginalBudgetHours.Size = new System.Drawing.Size(185, 20);
      this.txtOriginalBudgetHours.TabIndex = 8;
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(3, 111);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(103, 13);
      this.label13.TabIndex = 7;
      this.label13.Text = "Rev Est Compl Date";
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(3, 90);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(102, 13);
      this.label12.TabIndex = 6;
      this.label12.Text = "Orig Est Compl Date";
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(3, 69);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(85, 13);
      this.label11.TabIndex = 5;
      this.label11.Text = "Est Hrs to Compl";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(3, 48);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(74, 13);
      this.label9.TabIndex = 3;
      this.label9.Text = "Rev Budg Hrs";
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(3, 27);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(73, 13);
      this.label8.TabIndex = 2;
      this.label8.Text = "Orig Budg Hrs";
      // 
      // txtBudgetCustomerProject
      // 
      this.txtBudgetCustomerProject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtBudgetCustomerProject.Location = new System.Drawing.Point(1, 3);
      this.txtBudgetCustomerProject.Name = "txtBudgetCustomerProject";
      this.txtBudgetCustomerProject.Size = new System.Drawing.Size(294, 20);
      this.txtBudgetCustomerProject.TabIndex = 1;
      // 
      // tabEditTime
      // 
      this.tabEditTime.Controls.Add(this.splitEditTime);
      this.tabEditTime.Location = new System.Drawing.Point(4, 22);
      this.tabEditTime.Name = "tabEditTime";
      this.tabEditTime.Padding = new System.Windows.Forms.Padding(3);
      this.tabEditTime.Size = new System.Drawing.Size(1120, 276);
      this.tabEditTime.TabIndex = 1;
      this.tabEditTime.Text = "Time";
      this.tabEditTime.UseVisualStyleBackColor = true;
      // 
      // splitEditTime
      // 
      this.splitEditTime.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitEditTime.Location = new System.Drawing.Point(3, 3);
      this.splitEditTime.Name = "splitEditTime";
      // 
      // splitEditTime.Panel1
      // 
      this.splitEditTime.Panel1.Controls.Add(this.srchEditTime);
      // 
      // splitEditTime.Panel2
      // 
      this.splitEditTime.Panel2.Controls.Add(this.btnTimeSave);
      this.splitEditTime.Panel2.Controls.Add(this.txtTimeDescription);
      this.splitEditTime.Panel2.Controls.Add(this.txtInvoiceDate);
      this.splitEditTime.Panel2.Controls.Add(this.txtInvoiceNumber);
      this.splitEditTime.Panel2.Controls.Add(this.label7);
      this.splitEditTime.Panel2.Controls.Add(this.txtBilledAmount);
      this.splitEditTime.Panel2.Controls.Add(this.label6);
      this.splitEditTime.Panel2.Controls.Add(this.txtTimeRate);
      this.splitEditTime.Panel2.Controls.Add(this.label5);
      this.splitEditTime.Panel2.Controls.Add(this.txtBilledHours);
      this.splitEditTime.Panel2.Controls.Add(this.txtEnteredHours);
      this.splitEditTime.Panel2.Controls.Add(this.dtTimeWorkedDate);
      this.splitEditTime.Panel2.Controls.Add(this.label4);
      this.splitEditTime.Panel2.Controls.Add(this.label3);
      this.splitEditTime.Panel2.Controls.Add(this.label2);
      this.splitEditTime.Panel2.Controls.Add(this.cboBillingCode);
      this.splitEditTime.Panel2.Controls.Add(this.label1);
      this.splitEditTime.Panel2.Controls.Add(this.txtTimeCustomerProject);
      this.splitEditTime.Size = new System.Drawing.Size(1114, 270);
      this.splitEditTime.SplitterDistance = 810;
      this.splitEditTime.TabIndex = 0;
      // 
      // srchEditTime
      // 
      this.srchEditTime.CanChangeDisplaySearchCriteria = false;
      this.srchEditTime.CanSearchLockedColumns = false;
      this.srchEditTime.ColumnName = "CustomerName";
      this.srchEditTime.DisplaySearchCriteria = false;
      this.srchEditTime.Dock = System.Windows.Forms.DockStyle.Fill;
      this.srchEditTime.ExportCriteria = false;
      this.srchEditTime.InnerWhere = "";
      this.srchEditTime.Location = new System.Drawing.Point(0, 0);
      this.srchEditTime.Name = "srchEditTime";
      this.srchEditTime.NameType = ACG.App.Common.ACGCommonData.NameTypes.Customer;
      this.srchEditTime.SearchCriteria = ((System.Collections.Generic.Dictionary<string, string[]>)(resources.GetObject("srchEditTime.SearchCriteria")));
      this.srchEditTime.Size = new System.Drawing.Size(810, 270);
      this.srchEditTime.TabIndex = 0;
      this.srchEditTime.Title = "Search (0 Records Found)";
      this.srchEditTime.UniqueIdentifier = "ID";
      this.srchEditTime.RowSelected += new ACG.DesktopClient.Common.ctlSearchGrid.RowSelectedHandler(this.srchEditTime_RowSelected);
      // 
      // btnTimeSave
      // 
      this.btnTimeSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnTimeSave.Location = new System.Drawing.Point(224, 243);
      this.btnTimeSave.Name = "btnTimeSave";
      this.btnTimeSave.Size = new System.Drawing.Size(75, 23);
      this.btnTimeSave.TabIndex = 20;
      this.btnTimeSave.Text = "Save";
      this.btnTimeSave.UseVisualStyleBackColor = true;
      this.btnTimeSave.Click += new System.EventHandler(this.btnTimeSave_Click);
      // 
      // txtTimeDescription
      // 
      this.txtTimeDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtTimeDescription.Enabled = false;
      this.txtTimeDescription.Location = new System.Drawing.Point(10, 211);
      this.txtTimeDescription.Multiline = true;
      this.txtTimeDescription.Name = "txtTimeDescription";
      this.txtTimeDescription.Size = new System.Drawing.Size(286, 26);
      this.txtTimeDescription.TabIndex = 19;
      // 
      // txtInvoiceDate
      // 
      this.txtInvoiceDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtInvoiceDate.Enabled = false;
      this.txtInvoiceDate.Location = new System.Drawing.Point(173, 186);
      this.txtInvoiceDate.Name = "txtInvoiceDate";
      this.txtInvoiceDate.Size = new System.Drawing.Size(124, 20);
      this.txtInvoiceDate.TabIndex = 18;
      // 
      // txtInvoiceNumber
      // 
      this.txtInvoiceNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtInvoiceNumber.Enabled = false;
      this.txtInvoiceNumber.Location = new System.Drawing.Point(87, 186);
      this.txtInvoiceNumber.Name = "txtInvoiceNumber";
      this.txtInvoiceNumber.Size = new System.Drawing.Size(80, 20);
      this.txtInvoiceNumber.TabIndex = 17;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(6, 189);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(42, 13);
      this.label7.TabIndex = 16;
      this.label7.Text = "Invoice";
      // 
      // txtBilledAmount
      // 
      this.txtBilledAmount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtBilledAmount.Enabled = false;
      this.txtBilledAmount.Location = new System.Drawing.Point(87, 160);
      this.txtBilledAmount.Name = "txtBilledAmount";
      this.txtBilledAmount.Size = new System.Drawing.Size(210, 20);
      this.txtBilledAmount.TabIndex = 15;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(6, 163);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(71, 13);
      this.label6.TabIndex = 14;
      this.label6.Text = "Billed Amount";
      // 
      // txtTimeRate
      // 
      this.txtTimeRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtTimeRate.Location = new System.Drawing.Point(87, 134);
      this.txtTimeRate.Name = "txtTimeRate";
      this.txtTimeRate.Size = new System.Drawing.Size(210, 20);
      this.txtTimeRate.TabIndex = 13;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 137);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(30, 13);
      this.label5.TabIndex = 12;
      this.label5.Text = "Rate";
      // 
      // txtBilledHours
      // 
      this.txtBilledHours.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtBilledHours.Location = new System.Drawing.Point(87, 108);
      this.txtBilledHours.Name = "txtBilledHours";
      this.txtBilledHours.Size = new System.Drawing.Size(210, 20);
      this.txtBilledHours.TabIndex = 11;
      // 
      // txtEnteredHours
      // 
      this.txtEnteredHours.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtEnteredHours.Enabled = false;
      this.txtEnteredHours.Location = new System.Drawing.Point(87, 77);
      this.txtEnteredHours.Name = "txtEnteredHours";
      this.txtEnteredHours.Size = new System.Drawing.Size(210, 20);
      this.txtEnteredHours.TabIndex = 10;
      // 
      // dtTimeWorkedDate
      // 
      this.dtTimeWorkedDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dtTimeWorkedDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtTimeWorkedDate.Location = new System.Drawing.Point(87, 27);
      this.dtTimeWorkedDate.Name = "dtTimeWorkedDate";
      this.dtTimeWorkedDate.Size = new System.Drawing.Size(210, 20);
      this.dtTimeWorkedDate.TabIndex = 9;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 33);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(30, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "Date";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 111);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(60, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "BilledHours";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 81);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(75, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "Entered Hours";
      // 
      // cboBillingCode
      // 
      this.cboBillingCode.FormattingEnabled = true;
      this.cboBillingCode.Location = new System.Drawing.Point(87, 49);
      this.cboBillingCode.Name = "cboBillingCode";
      this.cboBillingCode.Size = new System.Drawing.Size(210, 21);
      this.cboBillingCode.TabIndex = 5;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 52);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(62, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Billing Code";
      // 
      // txtTimeCustomerProject
      // 
      this.txtTimeCustomerProject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtTimeCustomerProject.Location = new System.Drawing.Point(5, 5);
      this.txtTimeCustomerProject.Name = "txtTimeCustomerProject";
      this.txtTimeCustomerProject.Size = new System.Drawing.Size(294, 20);
      this.txtTimeCustomerProject.TabIndex = 0;
      // 
      // tabDetail
      // 
      this.tabDetail.Controls.Add(this.srchBudgetQuery);
      this.tabDetail.Location = new System.Drawing.Point(4, 22);
      this.tabDetail.Name = "tabDetail";
      this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
      this.tabDetail.Size = new System.Drawing.Size(1134, 524);
      this.tabDetail.TabIndex = 1;
      this.tabDetail.Text = "Detail";
      this.tabDetail.UseVisualStyleBackColor = true;
      // 
      // srchBudgetQuery
      // 
      this.srchBudgetQuery.CanChangeDisplaySearchCriteria = true;
      this.srchBudgetQuery.CanSearchLockedColumns = false;
      this.srchBudgetQuery.ColumnName = "CustomerName";
      this.srchBudgetQuery.DisplaySearchCriteria = true;
      this.srchBudgetQuery.Dock = System.Windows.Forms.DockStyle.Fill;
      this.srchBudgetQuery.ExportCriteria = false;
      this.srchBudgetQuery.InnerWhere = "";
      this.srchBudgetQuery.Location = new System.Drawing.Point(3, 3);
      this.srchBudgetQuery.Name = "srchBudgetQuery";
      this.srchBudgetQuery.NameType = ACG.App.Common.ACGCommonData.NameTypes.BudgetQuery;
      this.srchBudgetQuery.SearchCriteria = ((System.Collections.Generic.Dictionary<string, string[]>)(resources.GetObject("srchBudgetQuery.SearchCriteria")));
      this.srchBudgetQuery.Size = new System.Drawing.Size(1128, 518);
      this.srchBudgetQuery.TabIndex = 2;
      this.srchBudgetQuery.Title = "Search (0 Records Found)";
      this.srchBudgetQuery.UniqueIdentifier = "ID";
      // 
      // frmBudgetQuery
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1142, 576);
      this.Controls.Add(this.btnFlagBudget);
      this.Controls.Add(this.btnSetThroughDate);
      this.Controls.Add(this.dtBudgetThroughDate);
      this.Controls.Add(this.label14);
      this.Controls.Add(this.cboCustomer);
      this.Controls.Add(this.label10);
      this.Controls.Add(this.tabMain);
      this.Name = "frmBudgetQuery";
      this.Text = "frmBudgetQuery";
      this.Load += new System.EventHandler(this.frmBudgetQuery_Load);
      this.tabMain.ResumeLayout(false);
      this.tabSummary.ResumeLayout(false);
      this.splitBudgetSummary.Panel1.ResumeLayout(false);
      this.splitBudgetSummary.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitBudgetSummary)).EndInit();
      this.splitBudgetSummary.ResumeLayout(false);
      this.tabEditBudgetDetail.ResumeLayout(false);
      this.tabEditBudget.ResumeLayout(false);
      this.splitEditBudget.Panel1.ResumeLayout(false);
      this.splitEditBudget.Panel2.ResumeLayout(false);
      this.splitEditBudget.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitEditBudget)).EndInit();
      this.splitEditBudget.ResumeLayout(false);
      this.tabEditTime.ResumeLayout(false);
      this.splitEditTime.Panel1.ResumeLayout(false);
      this.splitEditTime.Panel2.ResumeLayout(false);
      this.splitEditTime.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitEditTime)).EndInit();
      this.splitEditTime.ResumeLayout(false);
      this.tabDetail.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TabControl tabMain;
    private System.Windows.Forms.TabPage tabSummary;
    private Common.ctlSearchGrid srchBudgetSummary;
    private System.Windows.Forms.TabPage tabDetail;
    private Common.ctlSearchGrid srchBudgetQuery;
    private System.Windows.Forms.SplitContainer splitBudgetSummary;
    private System.Windows.Forms.TabControl tabEditBudgetDetail;
    private System.Windows.Forms.TabPage tabEditBudget;
    private System.Windows.Forms.SplitContainer splitEditBudget;
    private Common.ctlSearchGrid srchEditBudget;
    private System.Windows.Forms.TabPage tabEditTime;
    private System.Windows.Forms.SplitContainer splitEditTime;
    private Common.ctlSearchGrid srchEditTime;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtTimeCustomerProject;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cboBillingCode;
    private System.Windows.Forms.TextBox txtBilledAmount;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox txtTimeRate;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox txtBilledHours;
    private System.Windows.Forms.TextBox txtEnteredHours;
    private System.Windows.Forms.DateTimePicker dtTimeWorkedDate;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button btnTimeSave;
    private System.Windows.Forms.TextBox txtTimeDescription;
    private System.Windows.Forms.TextBox txtInvoiceDate;
    private System.Windows.Forms.TextBox txtInvoiceNumber;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox txtBudgetCustomerProject;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Button btnBudgetSave;
    private System.Windows.Forms.DateTimePicker dtEstimatedCompletionDate;
    private System.Windows.Forms.TextBox txtOriginalCompletionDate;
    private System.Windows.Forms.TextBox txtEstimatedHoursToComplete;
    private System.Windows.Forms.TextBox txtRevisedBudgetHours;
    private System.Windows.Forms.TextBox txtOriginalBudgetHours;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.TextBox txtBudgetRate;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.ComboBox cboCustomer;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.DateTimePicker dtBudgetThroughDate;
    private System.Windows.Forms.Button btnSetThroughDate;
    private System.Windows.Forms.Button btnFlagBudget;

  }
}