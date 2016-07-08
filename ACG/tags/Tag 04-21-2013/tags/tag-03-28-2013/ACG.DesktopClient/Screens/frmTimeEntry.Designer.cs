namespace ACG.DesktopClient.Screens
{
  partial class frmTimeEntry
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTimeEntry));
      this.splitMain = new System.Windows.Forms.SplitContainer();
      this.label13 = new System.Windows.Forms.Label();
      this.txtEstimatedHoursToComplete = new System.Windows.Forms.TextBox();
      this.label15 = new System.Windows.Forms.Label();
      this.ckAutoStartTimer = new System.Windows.Forms.CheckBox();
      this.lblNew = new System.Windows.Forms.Label();
      this.btnDelete = new System.Windows.Forms.Button();
      this.lblTimerRunning = new System.Windows.Forms.Label();
      this.btnEndDecrement = new System.Windows.Forms.Button();
      this.btnEndIncrement = new System.Windows.Forms.Button();
      this.btnStartDecrement = new System.Windows.Forms.Button();
      this.btnStartIncrement = new System.Windows.Forms.Button();
      this.btnStartStop = new System.Windows.Forms.Button();
      this.btnSave = new System.Windows.Forms.Button();
      this.btnNew = new System.Windows.Forms.Button();
      this.txtDescription = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.cboBillingCode = new System.Windows.Forms.ComboBox();
      this.txtBilledMinutes = new System.Windows.Forms.TextBox();
      this.txtEnteredMinutes = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.tmEndTime = new System.Windows.Forms.DateTimePicker();
      this.label7 = new System.Windows.Forms.Label();
      this.tmStartTime = new System.Windows.Forms.DateTimePicker();
      this.dtDate = new System.Windows.Forms.DateTimePicker();
      this.cboSubProject = new System.Windows.Forms.ComboBox();
      this.cboProject = new System.Windows.Forms.ComboBox();
      this.cboCustomer = new System.Windows.Forms.ComboBox();
      this.cboResource = new System.Windows.Forms.ComboBox();
      this.label6 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.tabTime = new System.Windows.Forms.TabControl();
      this.tabDetail = new System.Windows.Forms.TabPage();
      this.srchTimeDetail = new ACG.DesktopClient.Common.ctlSearchGrid();
      this.tabSummary = new System.Windows.Forms.TabPage();
      this.grpCriteria = new System.Windows.Forms.GroupBox();
      this.ckIncludeOnlyUnpaidTime = new System.Windows.Forms.CheckBox();
      this.ckIncludeSubProject = new System.Windows.Forms.CheckBox();
      this.ckSelectByCustomer = new System.Windows.Forms.CheckBox();
      this.ckSelectByResource = new System.Windows.Forms.CheckBox();
      this.ckSummaryBillableOnly = new System.Windows.Forms.CheckBox();
      this.grpFormat = new System.Windows.Forms.GroupBox();
      this.radioWeekly = new System.Windows.Forms.RadioButton();
      this.radioByProject = new System.Windows.Forms.RadioButton();
      this.radioDaily = new System.Windows.Forms.RadioButton();
      this.btnReloadSummary = new System.Windows.Forms.Button();
      this.grdTimeSummary = new System.Windows.Forms.DataGridView();
      this.mnuSummaryContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.toolStripMenuItemExport = new System.Windows.Forms.ToolStripMenuItem();
      this.dtToDate = new System.Windows.Forms.DateTimePicker();
      this.label12 = new System.Windows.Forms.Label();
      this.dtFromDate = new System.Windows.Forms.DateTimePicker();
      this.label11 = new System.Windows.Forms.Label();
      this.tabResourceTime = new System.Windows.Forms.TabPage();
      this.txtPaidResource = new System.Windows.Forms.TextBox();
      this.grpFlagPaidTime = new System.Windows.Forms.GroupBox();
      this.btnResourceTimeRefresh = new System.Windows.Forms.Button();
      this.btnSetFlag = new System.Windows.Forms.Button();
      this.lblPaidOn = new System.Windows.Forms.Label();
      this.grpPaid = new System.Windows.Forms.GroupBox();
      this.radioUnpaid = new System.Windows.Forms.RadioButton();
      this.radioPaid = new System.Windows.Forms.RadioButton();
      this.dtPaidOn = new System.Windows.Forms.DateTimePicker();
      this.ckIncludePaidTime = new System.Windows.Forms.CheckBox();
      this.srchResourceTime = new ACG.DesktopClient.Common.ctlSearchGrid();
      this.txtEnteredHours = new System.Windows.Forms.TextBox();
      this.txtBilledHours = new System.Windows.Forms.TextBox();
      ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
      this.splitMain.Panel1.SuspendLayout();
      this.splitMain.Panel2.SuspendLayout();
      this.splitMain.SuspendLayout();
      this.tabTime.SuspendLayout();
      this.tabDetail.SuspendLayout();
      this.tabSummary.SuspendLayout();
      this.grpCriteria.SuspendLayout();
      this.grpFormat.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdTimeSummary)).BeginInit();
      this.mnuSummaryContextMenu.SuspendLayout();
      this.tabResourceTime.SuspendLayout();
      this.grpFlagPaidTime.SuspendLayout();
      this.grpPaid.SuspendLayout();
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
      this.splitMain.Panel1.AutoScroll = true;
      this.splitMain.Panel1.AutoScrollMinSize = new System.Drawing.Size(600, 217);
      this.splitMain.Panel1.Controls.Add(this.txtBilledHours);
      this.splitMain.Panel1.Controls.Add(this.txtEnteredHours);
      this.splitMain.Panel1.Controls.Add(this.label13);
      this.splitMain.Panel1.Controls.Add(this.txtEstimatedHoursToComplete);
      this.splitMain.Panel1.Controls.Add(this.label15);
      this.splitMain.Panel1.Controls.Add(this.ckAutoStartTimer);
      this.splitMain.Panel1.Controls.Add(this.lblNew);
      this.splitMain.Panel1.Controls.Add(this.btnDelete);
      this.splitMain.Panel1.Controls.Add(this.lblTimerRunning);
      this.splitMain.Panel1.Controls.Add(this.btnEndDecrement);
      this.splitMain.Panel1.Controls.Add(this.btnEndIncrement);
      this.splitMain.Panel1.Controls.Add(this.btnStartDecrement);
      this.splitMain.Panel1.Controls.Add(this.btnStartIncrement);
      this.splitMain.Panel1.Controls.Add(this.btnStartStop);
      this.splitMain.Panel1.Controls.Add(this.btnSave);
      this.splitMain.Panel1.Controls.Add(this.btnNew);
      this.splitMain.Panel1.Controls.Add(this.txtDescription);
      this.splitMain.Panel1.Controls.Add(this.label10);
      this.splitMain.Panel1.Controls.Add(this.label9);
      this.splitMain.Panel1.Controls.Add(this.cboBillingCode);
      this.splitMain.Panel1.Controls.Add(this.txtBilledMinutes);
      this.splitMain.Panel1.Controls.Add(this.txtEnteredMinutes);
      this.splitMain.Panel1.Controls.Add(this.label8);
      this.splitMain.Panel1.Controls.Add(this.tmEndTime);
      this.splitMain.Panel1.Controls.Add(this.label7);
      this.splitMain.Panel1.Controls.Add(this.tmStartTime);
      this.splitMain.Panel1.Controls.Add(this.dtDate);
      this.splitMain.Panel1.Controls.Add(this.cboSubProject);
      this.splitMain.Panel1.Controls.Add(this.cboProject);
      this.splitMain.Panel1.Controls.Add(this.cboCustomer);
      this.splitMain.Panel1.Controls.Add(this.cboResource);
      this.splitMain.Panel1.Controls.Add(this.label6);
      this.splitMain.Panel1.Controls.Add(this.label5);
      this.splitMain.Panel1.Controls.Add(this.label4);
      this.splitMain.Panel1.Controls.Add(this.label3);
      this.splitMain.Panel1.Controls.Add(this.label2);
      this.splitMain.Panel1.Controls.Add(this.label1);
      // 
      // splitMain.Panel2
      // 
      this.splitMain.Panel2.AutoScroll = true;
      this.splitMain.Panel2.AutoScrollMinSize = new System.Drawing.Size(915, 200);
      this.splitMain.Panel2.Controls.Add(this.tabTime);
      this.splitMain.Size = new System.Drawing.Size(1220, 449);
      this.splitMain.SplitterDistance = 218;
      this.splitMain.TabIndex = 21;
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label13.Location = new System.Drawing.Point(338, 145);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(269, 12);
      this.label13.TabIndex = 47;
      this.label13.Text = "**Note: Time shown is local time, but time in grid is Chicago Time";
      // 
      // txtEstimatedHoursToComplete
      // 
      this.txtEstimatedHoursToComplete.Location = new System.Drawing.Point(156, 140);
      this.txtEstimatedHoursToComplete.Name = "txtEstimatedHoursToComplete";
      this.txtEstimatedHoursToComplete.Size = new System.Drawing.Size(167, 20);
      this.txtEstimatedHoursToComplete.TabIndex = 19;
      this.txtEstimatedHoursToComplete.TextChanged += new System.EventHandler(this.txtEstimatedHoursToComplete_TextChanged);
      this.txtEstimatedHoursToComplete.Leave += new System.EventHandler(this.txtEstimatedHoursToComplete_Leave);
      // 
      // label15
      // 
      this.label15.AutoSize = true;
      this.label15.Location = new System.Drawing.Point(8, 141);
      this.label15.Name = "label15";
      this.label15.Size = new System.Drawing.Size(143, 13);
      this.label15.TabIndex = 46;
      this.label15.Text = "Estimated Hours to Complete";
      // 
      // ckAutoStartTimer
      // 
      this.ckAutoStartTimer.AutoSize = true;
      this.ckAutoStartTimer.Checked = true;
      this.ckAutoStartTimer.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckAutoStartTimer.Location = new System.Drawing.Point(523, 60);
      this.ckAutoStartTimer.Name = "ckAutoStartTimer";
      this.ckAutoStartTimer.Size = new System.Drawing.Size(102, 17);
      this.ckAutoStartTimer.TabIndex = 13;
      this.ckAutoStartTimer.Text = "Auto Start Timer";
      this.ckAutoStartTimer.UseVisualStyleBackColor = true;
      // 
      // lblNew
      // 
      this.lblNew.AutoSize = true;
      this.lblNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblNew.ForeColor = System.Drawing.Color.Red;
      this.lblNew.Location = new System.Drawing.Point(633, 9);
      this.lblNew.Name = "lblNew";
      this.lblNew.Size = new System.Drawing.Size(64, 13);
      this.lblNew.TabIndex = 44;
      this.lblNew.Text = "** NEW **";
      this.lblNew.Visible = false;
      // 
      // btnDelete
      // 
      this.btnDelete.Location = new System.Drawing.Point(500, 109);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(75, 23);
      this.btnDelete.TabIndex = 18;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      // 
      // lblTimerRunning
      // 
      this.lblTimerRunning.AutoSize = true;
      this.lblTimerRunning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTimerRunning.ForeColor = System.Drawing.Color.Red;
      this.lblTimerRunning.Location = new System.Drawing.Point(601, 33);
      this.lblTimerRunning.Name = "lblTimerRunning";
      this.lblTimerRunning.Size = new System.Drawing.Size(114, 13);
      this.lblTimerRunning.TabIndex = 42;
      this.lblTimerRunning.Text = "Timer is Running...";
      this.lblTimerRunning.Visible = false;
      // 
      // btnEndDecrement
      // 
      this.btnEndDecrement.Location = new System.Drawing.Point(500, 55);
      this.btnEndDecrement.Name = "btnEndDecrement";
      this.btnEndDecrement.Size = new System.Drawing.Size(14, 23);
      this.btnEndDecrement.TabIndex = 12;
      this.btnEndDecrement.Text = "-";
      this.btnEndDecrement.UseVisualStyleBackColor = true;
      this.btnEndDecrement.Click += new System.EventHandler(this.btnEndDecrement_Click);
      // 
      // btnEndIncrement
      // 
      this.btnEndIncrement.Location = new System.Drawing.Point(480, 55);
      this.btnEndIncrement.Name = "btnEndIncrement";
      this.btnEndIncrement.Size = new System.Drawing.Size(14, 23);
      this.btnEndIncrement.TabIndex = 11;
      this.btnEndIncrement.Text = "+";
      this.btnEndIncrement.UseVisualStyleBackColor = true;
      this.btnEndIncrement.Click += new System.EventHandler(this.btnEndIncrement_Click);
      // 
      // btnStartDecrement
      // 
      this.btnStartDecrement.Location = new System.Drawing.Point(500, 29);
      this.btnStartDecrement.Name = "btnStartDecrement";
      this.btnStartDecrement.Size = new System.Drawing.Size(14, 23);
      this.btnStartDecrement.TabIndex = 8;
      this.btnStartDecrement.Text = "-";
      this.btnStartDecrement.UseVisualStyleBackColor = true;
      this.btnStartDecrement.Click += new System.EventHandler(this.btnStartDecrement_Click);
      // 
      // btnStartIncrement
      // 
      this.btnStartIncrement.Location = new System.Drawing.Point(480, 29);
      this.btnStartIncrement.Name = "btnStartIncrement";
      this.btnStartIncrement.Size = new System.Drawing.Size(14, 23);
      this.btnStartIncrement.TabIndex = 7;
      this.btnStartIncrement.Text = "+";
      this.btnStartIncrement.UseVisualStyleBackColor = true;
      this.btnStartIncrement.Click += new System.EventHandler(this.btnStartIncrement_Click);
      // 
      // btnStartStop
      // 
      this.btnStartStop.Location = new System.Drawing.Point(520, 29);
      this.btnStartStop.Name = "btnStartStop";
      this.btnStartStop.Size = new System.Drawing.Size(75, 23);
      this.btnStartStop.TabIndex = 9;
      this.btnStartStop.Text = "Start";
      this.btnStartStop.UseVisualStyleBackColor = true;
      this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
      // 
      // btnSave
      // 
      this.btnSave.Location = new System.Drawing.Point(419, 109);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(75, 23);
      this.btnSave.TabIndex = 17;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // btnNew
      // 
      this.btnNew.Location = new System.Drawing.Point(338, 109);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new System.Drawing.Size(75, 23);
      this.btnNew.TabIndex = 16;
      this.btnNew.Text = "New";
      this.btnNew.UseVisualStyleBackColor = true;
      this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
      // 
      // txtDescription
      // 
      this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtDescription.Location = new System.Drawing.Point(78, 169);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new System.Drawing.Size(1130, 40);
      this.txtDescription.TabIndex = 20;
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(8, 168);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(60, 13);
      this.label10.TabIndex = 40;
      this.label10.Text = "Description";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(8, 114);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(62, 13);
      this.label9.TabIndex = 39;
      this.label9.Text = "Billing Code";
      // 
      // cboBillingCode
      // 
      this.cboBillingCode.FormattingEnabled = true;
      this.cboBillingCode.Location = new System.Drawing.Point(78, 111);
      this.cboBillingCode.Name = "cboBillingCode";
      this.cboBillingCode.Size = new System.Drawing.Size(245, 21);
      this.cboBillingCode.TabIndex = 4;
      // 
      // txtBilledMinutes
      // 
      this.txtBilledMinutes.Location = new System.Drawing.Point(721, 84);
      this.txtBilledMinutes.Name = "txtBilledMinutes";
      this.txtBilledMinutes.Size = new System.Drawing.Size(90, 20);
      this.txtBilledMinutes.TabIndex = 15;
      this.txtBilledMinutes.Visible = false;
      this.txtBilledMinutes.Leave += new System.EventHandler(this.txtBilledMinutes_Leave);
      // 
      // txtEnteredMinutes
      // 
      this.txtEnteredMinutes.Location = new System.Drawing.Point(625, 84);
      this.txtEnteredMinutes.Name = "txtEnteredMinutes";
      this.txtEnteredMinutes.Size = new System.Drawing.Size(90, 20);
      this.txtEnteredMinutes.TabIndex = 14;
      this.txtEnteredMinutes.Visible = false;
      this.txtEnteredMinutes.Leave += new System.EventHandler(this.txtEnteredMinutes_Leave);
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(335, 88);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(105, 13);
      this.label8.TabIndex = 35;
      this.label8.Text = "Mins (Entered/Billed)";
      // 
      // tmEndTime
      // 
      this.tmEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
      this.tmEndTime.Location = new System.Drawing.Point(376, 57);
      this.tmEndTime.Name = "tmEndTime";
      this.tmEndTime.Size = new System.Drawing.Size(100, 20);
      this.tmEndTime.TabIndex = 10;
      this.tmEndTime.ValueChanged += new System.EventHandler(this.tmEndTime_ValueChanged);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(335, 60);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(34, 13);
      this.label7.TabIndex = 33;
      this.label7.Text = "End**";
      // 
      // tmStartTime
      // 
      this.tmStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
      this.tmStartTime.Location = new System.Drawing.Point(377, 29);
      this.tmStartTime.Name = "tmStartTime";
      this.tmStartTime.Size = new System.Drawing.Size(100, 20);
      this.tmStartTime.TabIndex = 6;
      this.tmStartTime.ValueChanged += new System.EventHandler(this.tmStartTime_ValueChanged);
      // 
      // dtDate
      // 
      this.dtDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtDate.Location = new System.Drawing.Point(377, 6);
      this.dtDate.MaxDate = new System.DateTime(2100, 12, 31, 0, 0, 0, 0);
      this.dtDate.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtDate.Name = "dtDate";
      this.dtDate.Size = new System.Drawing.Size(250, 20);
      this.dtDate.TabIndex = 5;
      // 
      // cboSubProject
      // 
      this.cboSubProject.FormattingEnabled = true;
      this.cboSubProject.Location = new System.Drawing.Point(78, 84);
      this.cboSubProject.Name = "cboSubProject";
      this.cboSubProject.Size = new System.Drawing.Size(245, 21);
      this.cboSubProject.TabIndex = 3;
      // 
      // cboProject
      // 
      this.cboProject.FormattingEnabled = true;
      this.cboProject.Location = new System.Drawing.Point(78, 57);
      this.cboProject.Name = "cboProject";
      this.cboProject.Size = new System.Drawing.Size(245, 21);
      this.cboProject.TabIndex = 2;
      this.cboProject.SelectedIndexChanged += new System.EventHandler(this.cboProject_SelectedIndexChanged);
      // 
      // cboCustomer
      // 
      this.cboCustomer.FormattingEnabled = true;
      this.cboCustomer.Location = new System.Drawing.Point(78, 29);
      this.cboCustomer.Name = "cboCustomer";
      this.cboCustomer.Size = new System.Drawing.Size(245, 21);
      this.cboCustomer.TabIndex = 1;
      this.cboCustomer.SelectedIndexChanged += new System.EventHandler(this.cboCustomer_SelectedIndexChanged);
      // 
      // cboResource
      // 
      this.cboResource.FormattingEnabled = true;
      this.cboResource.Location = new System.Drawing.Point(78, 3);
      this.cboResource.Name = "cboResource";
      this.cboResource.Size = new System.Drawing.Size(245, 21);
      this.cboResource.TabIndex = 0;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(335, 32);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(37, 13);
      this.label6.TabIndex = 26;
      this.label6.Text = "Start**";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(335, 6);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(30, 13);
      this.label5.TabIndex = 25;
      this.label5.Text = "Date";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(8, 87);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(59, 13);
      this.label4.TabIndex = 24;
      this.label4.Text = "SubProject";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(8, 60);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(40, 13);
      this.label3.TabIndex = 23;
      this.label3.Text = "Project";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(8, 33);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(51, 13);
      this.label2.TabIndex = 22;
      this.label2.Text = "Customer";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(8, 6);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(53, 13);
      this.label1.TabIndex = 21;
      this.label1.Text = "Resource";
      // 
      // tabTime
      // 
      this.tabTime.Controls.Add(this.tabDetail);
      this.tabTime.Controls.Add(this.tabSummary);
      this.tabTime.Controls.Add(this.tabResourceTime);
      this.tabTime.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabTime.Location = new System.Drawing.Point(0, 0);
      this.tabTime.Name = "tabTime";
      this.tabTime.SelectedIndex = 0;
      this.tabTime.Size = new System.Drawing.Size(1220, 227);
      this.tabTime.TabIndex = 0;
      // 
      // tabDetail
      // 
      this.tabDetail.Controls.Add(this.srchTimeDetail);
      this.tabDetail.Location = new System.Drawing.Point(4, 22);
      this.tabDetail.Name = "tabDetail";
      this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
      this.tabDetail.Size = new System.Drawing.Size(1212, 201);
      this.tabDetail.TabIndex = 1;
      this.tabDetail.Text = "Detail";
      this.tabDetail.UseVisualStyleBackColor = true;
      // 
      // srchTimeDetail
      // 
      this.srchTimeDetail.CanSearchLockedColumns = false;
      this.srchTimeDetail.ColumnName = "CustomerName";
      this.srchTimeDetail.DisplaySearchCriteria = true;
      this.srchTimeDetail.Dock = System.Windows.Forms.DockStyle.Fill;
      this.srchTimeDetail.ExportCriteria = false;
      this.srchTimeDetail.InnerWhere = "";
      this.srchTimeDetail.Location = new System.Drawing.Point(3, 3);
      this.srchTimeDetail.Name = "srchTimeDetail";
      this.srchTimeDetail.NameType = ACG.App.Common.CommonData.NameTypes.Customer;
      this.srchTimeDetail.SearchCriteria = ((System.Collections.Generic.Dictionary<string, string[]>)(resources.GetObject("srchTimeDetail.SearchCriteria")));
      this.srchTimeDetail.Size = new System.Drawing.Size(1206, 195);
      this.srchTimeDetail.TabIndex = 0;
      this.srchTimeDetail.Title = "Search (0 Records Found)";
      this.srchTimeDetail.UniqueIdentifier = "ID";
      this.srchTimeDetail.RowSelected += new ACG.DesktopClient.Common.ctlSearchGrid.RowSelectedHandler(this.srchTimeDetail_RowSelected);
      // 
      // tabSummary
      // 
      this.tabSummary.Controls.Add(this.grpCriteria);
      this.tabSummary.Controls.Add(this.grpFormat);
      this.tabSummary.Controls.Add(this.btnReloadSummary);
      this.tabSummary.Controls.Add(this.grdTimeSummary);
      this.tabSummary.Controls.Add(this.dtToDate);
      this.tabSummary.Controls.Add(this.label12);
      this.tabSummary.Controls.Add(this.dtFromDate);
      this.tabSummary.Controls.Add(this.label11);
      this.tabSummary.Location = new System.Drawing.Point(4, 22);
      this.tabSummary.Name = "tabSummary";
      this.tabSummary.Padding = new System.Windows.Forms.Padding(3);
      this.tabSummary.Size = new System.Drawing.Size(1212, 201);
      this.tabSummary.TabIndex = 0;
      this.tabSummary.Text = "Summary";
      this.tabSummary.UseVisualStyleBackColor = true;
      // 
      // grpCriteria
      // 
      this.grpCriteria.Controls.Add(this.ckIncludeOnlyUnpaidTime);
      this.grpCriteria.Controls.Add(this.ckIncludeSubProject);
      this.grpCriteria.Controls.Add(this.ckSelectByCustomer);
      this.grpCriteria.Controls.Add(this.ckSelectByResource);
      this.grpCriteria.Controls.Add(this.ckSummaryBillableOnly);
      this.grpCriteria.Location = new System.Drawing.Point(675, 15);
      this.grpCriteria.Name = "grpCriteria";
      this.grpCriteria.Size = new System.Drawing.Size(240, 144);
      this.grpCriteria.TabIndex = 21;
      this.grpCriteria.TabStop = false;
      this.grpCriteria.Text = "Criteria";
      // 
      // ckIncludeOnlyUnpaidTime
      // 
      this.ckIncludeOnlyUnpaidTime.AutoSize = true;
      this.ckIncludeOnlyUnpaidTime.Location = new System.Drawing.Point(10, 20);
      this.ckIncludeOnlyUnpaidTime.Name = "ckIncludeOnlyUnpaidTime";
      this.ckIncludeOnlyUnpaidTime.Size = new System.Drawing.Size(140, 17);
      this.ckIncludeOnlyUnpaidTime.TabIndex = 3;
      this.ckIncludeOnlyUnpaidTime.Text = "Include only unpaid time";
      this.ckIncludeOnlyUnpaidTime.UseVisualStyleBackColor = true;
      this.ckIncludeOnlyUnpaidTime.Visible = false;
      // 
      // ckIncludeSubProject
      // 
      this.ckIncludeSubProject.AutoSize = true;
      this.ckIncludeSubProject.Location = new System.Drawing.Point(10, 112);
      this.ckIncludeSubProject.Name = "ckIncludeSubProject";
      this.ckIncludeSubProject.Size = new System.Drawing.Size(224, 17);
      this.ckIncludeSubProject.TabIndex = 7;
      this.ckIncludeSubProject.Text = "Include Subproject Detail (By Project only)";
      this.ckIncludeSubProject.UseVisualStyleBackColor = true;
      this.ckIncludeSubProject.Visible = false;
      // 
      // ckSelectByCustomer
      // 
      this.ckSelectByCustomer.AutoSize = true;
      this.ckSelectByCustomer.Location = new System.Drawing.Point(10, 89);
      this.ckSelectByCustomer.Name = "ckSelectByCustomer";
      this.ckSelectByCustomer.Size = new System.Drawing.Size(151, 17);
      this.ckSelectByCustomer.TabIndex = 6;
      this.ckSelectByCustomer.Text = "Include Only this Customer";
      this.ckSelectByCustomer.UseVisualStyleBackColor = true;
      this.ckSelectByCustomer.Visible = false;
      // 
      // ckSelectByResource
      // 
      this.ckSelectByResource.AutoSize = true;
      this.ckSelectByResource.Checked = true;
      this.ckSelectByResource.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckSelectByResource.Location = new System.Drawing.Point(10, 66);
      this.ckSelectByResource.Name = "ckSelectByResource";
      this.ckSelectByResource.Size = new System.Drawing.Size(153, 17);
      this.ckSelectByResource.TabIndex = 5;
      this.ckSelectByResource.Text = "Include Only this Resource";
      this.ckSelectByResource.UseVisualStyleBackColor = true;
      this.ckSelectByResource.Visible = false;
      // 
      // ckSummaryBillableOnly
      // 
      this.ckSummaryBillableOnly.AutoSize = true;
      this.ckSummaryBillableOnly.Location = new System.Drawing.Point(10, 43);
      this.ckSummaryBillableOnly.Name = "ckSummaryBillableOnly";
      this.ckSummaryBillableOnly.Size = new System.Drawing.Size(147, 17);
      this.ckSummaryBillableOnly.TabIndex = 4;
      this.ckSummaryBillableOnly.Text = "Include Only Billable Time";
      this.ckSummaryBillableOnly.UseVisualStyleBackColor = true;
      // 
      // grpFormat
      // 
      this.grpFormat.Controls.Add(this.radioWeekly);
      this.grpFormat.Controls.Add(this.radioByProject);
      this.grpFormat.Controls.Add(this.radioDaily);
      this.grpFormat.Location = new System.Drawing.Point(545, 15);
      this.grpFormat.Name = "grpFormat";
      this.grpFormat.Size = new System.Drawing.Size(111, 97);
      this.grpFormat.TabIndex = 17;
      this.grpFormat.TabStop = false;
      this.grpFormat.Text = "Format";
      // 
      // radioWeekly
      // 
      this.radioWeekly.AutoSize = true;
      this.radioWeekly.Location = new System.Drawing.Point(19, 68);
      this.radioWeekly.Name = "radioWeekly";
      this.radioWeekly.Size = new System.Drawing.Size(61, 17);
      this.radioWeekly.TabIndex = 13;
      this.radioWeekly.TabStop = true;
      this.radioWeekly.Text = "Weekly";
      this.radioWeekly.UseVisualStyleBackColor = true;
      // 
      // radioByProject
      // 
      this.radioByProject.AutoSize = true;
      this.radioByProject.Location = new System.Drawing.Point(19, 40);
      this.radioByProject.Name = "radioByProject";
      this.radioByProject.Size = new System.Drawing.Size(73, 17);
      this.radioByProject.TabIndex = 12;
      this.radioByProject.TabStop = true;
      this.radioByProject.Text = "By Project";
      this.radioByProject.UseVisualStyleBackColor = true;
      // 
      // radioDaily
      // 
      this.radioDaily.AutoSize = true;
      this.radioDaily.Checked = true;
      this.radioDaily.Location = new System.Drawing.Point(19, 12);
      this.radioDaily.Name = "radioDaily";
      this.radioDaily.Size = new System.Drawing.Size(48, 17);
      this.radioDaily.TabIndex = 11;
      this.radioDaily.TabStop = true;
      this.radioDaily.Text = "Daily";
      this.radioDaily.UseVisualStyleBackColor = true;
      // 
      // btnReloadSummary
      // 
      this.btnReloadSummary.Location = new System.Drawing.Point(452, 10);
      this.btnReloadSummary.Name = "btnReloadSummary";
      this.btnReloadSummary.Size = new System.Drawing.Size(75, 23);
      this.btnReloadSummary.TabIndex = 2;
      this.btnReloadSummary.Text = "Reload";
      this.btnReloadSummary.UseVisualStyleBackColor = true;
      this.btnReloadSummary.Click += new System.EventHandler(this.btnReloadSummary_Click);
      // 
      // grdTimeSummary
      // 
      this.grdTimeSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.grdTimeSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdTimeSummary.ContextMenuStrip = this.mnuSummaryContextMenu;
      this.grdTimeSummary.Location = new System.Drawing.Point(19, 38);
      this.grdTimeSummary.Name = "grdTimeSummary";
      this.grdTimeSummary.Size = new System.Drawing.Size(508, 302);
      this.grdTimeSummary.TabIndex = 4;
      // 
      // mnuSummaryContextMenu
      // 
      this.mnuSummaryContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemExport});
      this.mnuSummaryContextMenu.Name = "mnuSummaryContextMenu";
      this.mnuSummaryContextMenu.Size = new System.Drawing.Size(108, 26);
      // 
      // toolStripMenuItemExport
      // 
      this.toolStripMenuItemExport.Name = "toolStripMenuItemExport";
      this.toolStripMenuItemExport.Size = new System.Drawing.Size(107, 22);
      this.toolStripMenuItemExport.Text = "Export";
      this.toolStripMenuItemExport.Click += new System.EventHandler(this.toolStripMenuItemExport_Click);
      // 
      // dtToDate
      // 
      this.dtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtToDate.Location = new System.Drawing.Point(281, 14);
      this.dtToDate.Name = "dtToDate";
      this.dtToDate.Size = new System.Drawing.Size(144, 20);
      this.dtToDate.TabIndex = 1;
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(229, 15);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(46, 13);
      this.label12.TabIndex = 2;
      this.label12.Text = "To Date";
      // 
      // dtFromDate
      // 
      this.dtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtFromDate.Location = new System.Drawing.Point(79, 14);
      this.dtFromDate.Name = "dtFromDate";
      this.dtFromDate.Size = new System.Drawing.Size(144, 20);
      this.dtFromDate.TabIndex = 0;
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(12, 15);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(56, 13);
      this.label11.TabIndex = 0;
      this.label11.Text = "From Date";
      // 
      // tabResourceTime
      // 
      this.tabResourceTime.Controls.Add(this.txtPaidResource);
      this.tabResourceTime.Controls.Add(this.grpFlagPaidTime);
      this.tabResourceTime.Controls.Add(this.ckIncludePaidTime);
      this.tabResourceTime.Controls.Add(this.srchResourceTime);
      this.tabResourceTime.Location = new System.Drawing.Point(4, 22);
      this.tabResourceTime.Name = "tabResourceTime";
      this.tabResourceTime.Size = new System.Drawing.Size(1212, 201);
      this.tabResourceTime.TabIndex = 2;
      this.tabResourceTime.Text = "Resource Time";
      this.tabResourceTime.UseVisualStyleBackColor = true;
      // 
      // txtPaidResource
      // 
      this.txtPaidResource.Enabled = false;
      this.txtPaidResource.Location = new System.Drawing.Point(540, 37);
      this.txtPaidResource.Name = "txtPaidResource";
      this.txtPaidResource.Size = new System.Drawing.Size(152, 20);
      this.txtPaidResource.TabIndex = 24;
      // 
      // grpFlagPaidTime
      // 
      this.grpFlagPaidTime.Controls.Add(this.btnResourceTimeRefresh);
      this.grpFlagPaidTime.Controls.Add(this.btnSetFlag);
      this.grpFlagPaidTime.Controls.Add(this.lblPaidOn);
      this.grpFlagPaidTime.Controls.Add(this.grpPaid);
      this.grpFlagPaidTime.Controls.Add(this.dtPaidOn);
      this.grpFlagPaidTime.Location = new System.Drawing.Point(540, 65);
      this.grpFlagPaidTime.Name = "grpFlagPaidTime";
      this.grpFlagPaidTime.Size = new System.Drawing.Size(180, 114);
      this.grpFlagPaidTime.TabIndex = 23;
      this.grpFlagPaidTime.TabStop = false;
      this.grpFlagPaidTime.Visible = false;
      // 
      // btnResourceTimeRefresh
      // 
      this.btnResourceTimeRefresh.Location = new System.Drawing.Point(92, 81);
      this.btnResourceTimeRefresh.Name = "btnResourceTimeRefresh";
      this.btnResourceTimeRefresh.Size = new System.Drawing.Size(75, 23);
      this.btnResourceTimeRefresh.TabIndex = 21;
      this.btnResourceTimeRefresh.Text = "Refresh";
      this.btnResourceTimeRefresh.UseVisualStyleBackColor = true;
      this.btnResourceTimeRefresh.Click += new System.EventHandler(this.btnResourceTimeRefresh_Click);
      // 
      // btnSetFlag
      // 
      this.btnSetFlag.Location = new System.Drawing.Point(13, 81);
      this.btnSetFlag.Name = "btnSetFlag";
      this.btnSetFlag.Size = new System.Drawing.Size(75, 23);
      this.btnSetFlag.TabIndex = 20;
      this.btnSetFlag.Text = "Set Flag";
      this.btnSetFlag.UseVisualStyleBackColor = true;
      this.btnSetFlag.Click += new System.EventHandler(this.btnSetFlag_Click);
      // 
      // lblPaidOn
      // 
      this.lblPaidOn.AutoSize = true;
      this.lblPaidOn.Location = new System.Drawing.Point(10, 58);
      this.lblPaidOn.Name = "lblPaidOn";
      this.lblPaidOn.Size = new System.Drawing.Size(19, 13);
      this.lblPaidOn.TabIndex = 19;
      this.lblPaidOn.Text = "on";
      this.lblPaidOn.Visible = false;
      // 
      // grpPaid
      // 
      this.grpPaid.Controls.Add(this.radioUnpaid);
      this.grpPaid.Controls.Add(this.radioPaid);
      this.grpPaid.Location = new System.Drawing.Point(7, 5);
      this.grpPaid.Name = "grpPaid";
      this.grpPaid.Size = new System.Drawing.Size(141, 42);
      this.grpPaid.TabIndex = 18;
      this.grpPaid.TabStop = false;
      this.grpPaid.Text = "Flag as";
      // 
      // radioUnpaid
      // 
      this.radioUnpaid.AutoSize = true;
      this.radioUnpaid.Location = new System.Drawing.Point(66, 14);
      this.radioUnpaid.Name = "radioUnpaid";
      this.radioUnpaid.Size = new System.Drawing.Size(59, 17);
      this.radioUnpaid.TabIndex = 1;
      this.radioUnpaid.TabStop = true;
      this.radioUnpaid.Text = "Unpaid";
      this.radioUnpaid.UseVisualStyleBackColor = true;
      // 
      // radioPaid
      // 
      this.radioPaid.AutoSize = true;
      this.radioPaid.Checked = true;
      this.radioPaid.Location = new System.Drawing.Point(9, 14);
      this.radioPaid.Name = "radioPaid";
      this.radioPaid.Size = new System.Drawing.Size(46, 17);
      this.radioPaid.TabIndex = 0;
      this.radioPaid.TabStop = true;
      this.radioPaid.Text = "Paid";
      this.radioPaid.UseVisualStyleBackColor = true;
      // 
      // dtPaidOn
      // 
      this.dtPaidOn.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtPaidOn.Location = new System.Drawing.Point(33, 55);
      this.dtPaidOn.Name = "dtPaidOn";
      this.dtPaidOn.Size = new System.Drawing.Size(94, 20);
      this.dtPaidOn.TabIndex = 16;
      // 
      // ckIncludePaidTime
      // 
      this.ckIncludePaidTime.AutoSize = true;
      this.ckIncludePaidTime.Location = new System.Drawing.Point(540, 14);
      this.ckIncludePaidTime.Name = "ckIncludePaidTime";
      this.ckIncludePaidTime.Size = new System.Drawing.Size(111, 17);
      this.ckIncludePaidTime.TabIndex = 1;
      this.ckIncludePaidTime.Text = "Include Paid Time";
      this.ckIncludePaidTime.UseVisualStyleBackColor = true;
      this.ckIncludePaidTime.CheckedChanged += new System.EventHandler(this.ckIncludePaidTime_CheckedChanged);
      // 
      // srchResourceTime
      // 
      this.srchResourceTime.CanSearchLockedColumns = false;
      this.srchResourceTime.ColumnName = "CustomerName";
      this.srchResourceTime.DisplaySearchCriteria = false;
      this.srchResourceTime.ExportCriteria = false;
      this.srchResourceTime.InnerWhere = "";
      this.srchResourceTime.Location = new System.Drawing.Point(9, 14);
      this.srchResourceTime.Name = "srchResourceTime";
      this.srchResourceTime.NameType = ACG.App.Common.CommonData.NameTypes.Customer;
      this.srchResourceTime.SearchCriteria = ((System.Collections.Generic.Dictionary<string, string[]>)(resources.GetObject("srchResourceTime.SearchCriteria")));
      this.srchResourceTime.Size = new System.Drawing.Size(518, 337);
      this.srchResourceTime.TabIndex = 0;
      this.srchResourceTime.Title = "Search (0 Records Found)";
      this.srchResourceTime.UniqueIdentifier = "ID";
      this.srchResourceTime.RowSelected += new ACG.DesktopClient.Common.ctlSearchGrid.RowSelectedHandler(this.srchResourceTime_RowSelected);
      // 
      // txtEnteredHours
      // 
      this.txtEnteredHours.Location = new System.Drawing.Point(446, 83);
      this.txtEnteredHours.Name = "txtEnteredHours";
      this.txtEnteredHours.Size = new System.Drawing.Size(68, 20);
      this.txtEnteredHours.TabIndex = 48;
      this.txtEnteredHours.Leave += new System.EventHandler(this.txtEnteredHours_Leave);
      // 
      // txtBilledHours
      // 
      this.txtBilledHours.Location = new System.Drawing.Point(523, 83);
      this.txtBilledHours.Name = "txtBilledHours";
      this.txtBilledHours.Size = new System.Drawing.Size(68, 20);
      this.txtBilledHours.TabIndex = 49;
      this.txtBilledHours.Leave += new System.EventHandler(this.txtBilledHours_Leave);
      // 
      // frmTimeEntry
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1220, 449);
      this.Controls.Add(this.splitMain);
      this.Name = "frmTimeEntry";
      this.Text = "frmTimeEntry";
      this.Load += new System.EventHandler(this.frmTimeEntry_Load);
      this.splitMain.Panel1.ResumeLayout(false);
      this.splitMain.Panel1.PerformLayout();
      this.splitMain.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
      this.splitMain.ResumeLayout(false);
      this.tabTime.ResumeLayout(false);
      this.tabDetail.ResumeLayout(false);
      this.tabSummary.ResumeLayout(false);
      this.tabSummary.PerformLayout();
      this.grpCriteria.ResumeLayout(false);
      this.grpCriteria.PerformLayout();
      this.grpFormat.ResumeLayout(false);
      this.grpFormat.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdTimeSummary)).EndInit();
      this.mnuSummaryContextMenu.ResumeLayout(false);
      this.tabResourceTime.ResumeLayout(false);
      this.tabResourceTime.PerformLayout();
      this.grpFlagPaidTime.ResumeLayout(false);
      this.grpFlagPaidTime.PerformLayout();
      this.grpPaid.ResumeLayout(false);
      this.grpPaid.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitMain;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.ComboBox cboBillingCode;
    private System.Windows.Forms.TextBox txtBilledMinutes;
    private System.Windows.Forms.TextBox txtEnteredMinutes;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.DateTimePicker tmEndTime;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.DateTimePicker tmStartTime;
    private System.Windows.Forms.DateTimePicker dtDate;
    private System.Windows.Forms.ComboBox cboSubProject;
    private System.Windows.Forms.ComboBox cboProject;
    private System.Windows.Forms.ComboBox cboCustomer;
    private System.Windows.Forms.ComboBox cboResource;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnStartStop;
    private System.Windows.Forms.Button btnEndDecrement;
    private System.Windows.Forms.Button btnEndIncrement;
    private System.Windows.Forms.Button btnStartDecrement;
    private System.Windows.Forms.Button btnStartIncrement;
    private System.Windows.Forms.TabControl tabTime;
    private System.Windows.Forms.TabPage tabSummary;
    private System.Windows.Forms.TabPage tabDetail;
    private Common.ctlSearchGrid srchTimeDetail;
    private System.Windows.Forms.DataGridView grdTimeSummary;
    private System.Windows.Forms.DateTimePicker dtToDate;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.DateTimePicker dtFromDate;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label lblTimerRunning;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Label lblNew;
    private System.Windows.Forms.CheckBox ckAutoStartTimer;
    private System.Windows.Forms.Button btnReloadSummary;
    private System.Windows.Forms.CheckBox ckSummaryBillableOnly;
    private System.Windows.Forms.CheckBox ckSelectByCustomer;
    private System.Windows.Forms.CheckBox ckSelectByResource;
    private System.Windows.Forms.CheckBox ckIncludeSubProject;
    private System.Windows.Forms.ContextMenuStrip mnuSummaryContextMenu;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExport;
    private System.Windows.Forms.TextBox txtEstimatedHoursToComplete;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.CheckBox ckIncludeOnlyUnpaidTime;
    private System.Windows.Forms.GroupBox grpFormat;
    private System.Windows.Forms.RadioButton radioWeekly;
    private System.Windows.Forms.RadioButton radioByProject;
    private System.Windows.Forms.RadioButton radioDaily;
    private System.Windows.Forms.GroupBox grpCriteria;
    private System.Windows.Forms.TabPage tabResourceTime;
    private System.Windows.Forms.CheckBox ckIncludePaidTime;
    private Common.ctlSearchGrid srchResourceTime;
    private System.Windows.Forms.GroupBox grpFlagPaidTime;
    private System.Windows.Forms.Button btnSetFlag;
    private System.Windows.Forms.Label lblPaidOn;
    private System.Windows.Forms.GroupBox grpPaid;
    private System.Windows.Forms.RadioButton radioUnpaid;
    private System.Windows.Forms.RadioButton radioPaid;
    private System.Windows.Forms.DateTimePicker dtPaidOn;
    private System.Windows.Forms.TextBox txtPaidResource;
    private System.Windows.Forms.Button btnResourceTimeRefresh;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.TextBox txtBilledHours;
    private System.Windows.Forms.TextBox txtEnteredHours;

  }
}