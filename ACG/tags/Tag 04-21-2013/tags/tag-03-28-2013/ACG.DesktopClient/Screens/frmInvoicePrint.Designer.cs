namespace ACG.DesktopClient.Screens
{
  partial class frmInvoicePrint
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInvoicePrint));
      this.cboProject = new System.Windows.Forms.ComboBox();
      this.cboCustomer = new System.Windows.Forms.ComboBox();
      this.cboResource = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.grpInvoicing = new System.Windows.Forms.GroupBox();
      this.ckExcludeProject = new System.Windows.Forms.CheckBox();
      this.ckDetail = new System.Windows.Forms.CheckBox();
      this.btnUnpost = new System.Windows.Forms.Button();
      this.ckIncludePosted = new System.Windows.Forms.CheckBox();
      this.btnPostInvoice = new System.Windows.Forms.Button();
      this.btnPreviewInvoice = new System.Windows.Forms.Button();
      this.ckInvoiceOnlyThisProject = new System.Windows.Forms.CheckBox();
      this.ckInvoiceOnlyThisCustomer = new System.Windows.Forms.CheckBox();
      this.ckInvoiceOnlyThisResource = new System.Windows.Forms.CheckBox();
      this.dtInvoiceThrough = new System.Windows.Forms.DateTimePicker();
      this.label13 = new System.Windows.Forms.Label();
      this.splitMain = new System.Windows.Forms.SplitContainer();
      this.srchInvoices = new ACG.DesktopClient.Common.ctlSearchGrid();
      this.grpInvoicing.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
      this.splitMain.Panel1.SuspendLayout();
      this.splitMain.Panel2.SuspendLayout();
      this.splitMain.SuspendLayout();
      this.SuspendLayout();
      // 
      // cboProject
      // 
      this.cboProject.FormattingEnabled = true;
      this.cboProject.Location = new System.Drawing.Point(88, 68);
      this.cboProject.Name = "cboProject";
      this.cboProject.Size = new System.Drawing.Size(245, 21);
      this.cboProject.TabIndex = 5;
      // 
      // cboCustomer
      // 
      this.cboCustomer.FormattingEnabled = true;
      this.cboCustomer.Location = new System.Drawing.Point(88, 40);
      this.cboCustomer.Name = "cboCustomer";
      this.cboCustomer.Size = new System.Drawing.Size(245, 21);
      this.cboCustomer.TabIndex = 4;
      this.cboCustomer.SelectedIndexChanged += new System.EventHandler(this.cboCustomer_SelectedIndexChanged);
      // 
      // cboResource
      // 
      this.cboResource.FormattingEnabled = true;
      this.cboResource.Location = new System.Drawing.Point(88, 14);
      this.cboResource.Name = "cboResource";
      this.cboResource.Size = new System.Drawing.Size(245, 21);
      this.cboResource.TabIndex = 3;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(12, 71);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(40, 13);
      this.label3.TabIndex = 26;
      this.label3.Text = "Project";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(12, 44);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(51, 13);
      this.label2.TabIndex = 25;
      this.label2.Text = "Customer";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 17);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(53, 13);
      this.label1.TabIndex = 24;
      this.label1.Text = "Resource";
      // 
      // grpInvoicing
      // 
      this.grpInvoicing.Controls.Add(this.ckExcludeProject);
      this.grpInvoicing.Controls.Add(this.ckDetail);
      this.grpInvoicing.Controls.Add(this.btnUnpost);
      this.grpInvoicing.Controls.Add(this.ckIncludePosted);
      this.grpInvoicing.Controls.Add(this.btnPostInvoice);
      this.grpInvoicing.Controls.Add(this.btnPreviewInvoice);
      this.grpInvoicing.Controls.Add(this.ckInvoiceOnlyThisProject);
      this.grpInvoicing.Controls.Add(this.ckInvoiceOnlyThisCustomer);
      this.grpInvoicing.Controls.Add(this.ckInvoiceOnlyThisResource);
      this.grpInvoicing.Controls.Add(this.dtInvoiceThrough);
      this.grpInvoicing.Controls.Add(this.label13);
      this.grpInvoicing.Location = new System.Drawing.Point(352, 3);
      this.grpInvoicing.Name = "grpInvoicing";
      this.grpInvoicing.Size = new System.Drawing.Size(370, 121);
      this.grpInvoicing.TabIndex = 27;
      this.grpInvoicing.TabStop = false;
      this.grpInvoicing.Text = "Invoicing";
      // 
      // ckExcludeProject
      // 
      this.ckExcludeProject.AutoSize = true;
      this.ckExcludeProject.Location = new System.Drawing.Point(146, 92);
      this.ckExcludeProject.Name = "ckExcludeProject";
      this.ckExcludeProject.Size = new System.Drawing.Size(119, 17);
      this.ckExcludeProject.TabIndex = 10;
      this.ckExcludeProject.Text = "Exclude this Project";
      this.ckExcludeProject.UseVisualStyleBackColor = true;
      this.ckExcludeProject.CheckedChanged += new System.EventHandler(this.ckExcludeProject_CheckedChanged);
      // 
      // ckDetail
      // 
      this.ckDetail.AutoSize = true;
      this.ckDetail.Location = new System.Drawing.Point(146, 69);
      this.ckDetail.Name = "ckDetail";
      this.ckDetail.Size = new System.Drawing.Size(77, 17);
      this.ckDetail.TabIndex = 9;
      this.ckDetail.Text = "Print Detail";
      this.ckDetail.UseVisualStyleBackColor = true;
      // 
      // btnUnpost
      // 
      this.btnUnpost.Location = new System.Drawing.Point(279, 69);
      this.btnUnpost.Name = "btnUnpost";
      this.btnUnpost.Size = new System.Drawing.Size(75, 23);
      this.btnUnpost.TabIndex = 8;
      this.btnUnpost.Text = "Unpost";
      this.btnUnpost.UseVisualStyleBackColor = true;
      this.btnUnpost.Click += new System.EventHandler(this.btnUnpost_Click);
      // 
      // ckIncludePosted
      // 
      this.ckIncludePosted.AutoSize = true;
      this.ckIncludePosted.Location = new System.Drawing.Point(146, 43);
      this.ckIncludePosted.Name = "ckIncludePosted";
      this.ckIncludePosted.Size = new System.Drawing.Size(97, 17);
      this.ckIncludePosted.TabIndex = 7;
      this.ckIncludePosted.Text = "Posted Invoice";
      this.ckIncludePosted.UseVisualStyleBackColor = true;
      // 
      // btnPostInvoice
      // 
      this.btnPostInvoice.Location = new System.Drawing.Point(279, 40);
      this.btnPostInvoice.Name = "btnPostInvoice";
      this.btnPostInvoice.Size = new System.Drawing.Size(75, 23);
      this.btnPostInvoice.TabIndex = 6;
      this.btnPostInvoice.Text = "Post";
      this.btnPostInvoice.UseVisualStyleBackColor = true;
      this.btnPostInvoice.Click += new System.EventHandler(this.btnPostInvoice_Click);
      // 
      // btnPreviewInvoice
      // 
      this.btnPreviewInvoice.Location = new System.Drawing.Point(279, 11);
      this.btnPreviewInvoice.Name = "btnPreviewInvoice";
      this.btnPreviewInvoice.Size = new System.Drawing.Size(75, 23);
      this.btnPreviewInvoice.TabIndex = 5;
      this.btnPreviewInvoice.Text = "Preview";
      this.btnPreviewInvoice.UseVisualStyleBackColor = true;
      this.btnPreviewInvoice.Click += new System.EventHandler(this.btnPreviewInvoice_Click);
      // 
      // ckInvoiceOnlyThisProject
      // 
      this.ckInvoiceOnlyThisProject.AutoSize = true;
      this.ckInvoiceOnlyThisProject.Location = new System.Drawing.Point(13, 92);
      this.ckInvoiceOnlyThisProject.Name = "ckInvoiceOnlyThisProject";
      this.ckInvoiceOnlyThisProject.Size = new System.Drawing.Size(102, 17);
      this.ckInvoiceOnlyThisProject.TabIndex = 4;
      this.ckInvoiceOnlyThisProject.Text = "Only this Project";
      this.ckInvoiceOnlyThisProject.UseVisualStyleBackColor = true;
      this.ckInvoiceOnlyThisProject.CheckedChanged += new System.EventHandler(this.ckInvoiceOnlyThisProject_CheckedChanged);
      // 
      // ckInvoiceOnlyThisCustomer
      // 
      this.ckInvoiceOnlyThisCustomer.AutoSize = true;
      this.ckInvoiceOnlyThisCustomer.Checked = true;
      this.ckInvoiceOnlyThisCustomer.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckInvoiceOnlyThisCustomer.Location = new System.Drawing.Point(13, 67);
      this.ckInvoiceOnlyThisCustomer.Name = "ckInvoiceOnlyThisCustomer";
      this.ckInvoiceOnlyThisCustomer.Size = new System.Drawing.Size(113, 17);
      this.ckInvoiceOnlyThisCustomer.TabIndex = 3;
      this.ckInvoiceOnlyThisCustomer.Text = "Only this Customer";
      this.ckInvoiceOnlyThisCustomer.UseVisualStyleBackColor = true;
      // 
      // ckInvoiceOnlyThisResource
      // 
      this.ckInvoiceOnlyThisResource.AutoSize = true;
      this.ckInvoiceOnlyThisResource.Location = new System.Drawing.Point(13, 44);
      this.ckInvoiceOnlyThisResource.Name = "ckInvoiceOnlyThisResource";
      this.ckInvoiceOnlyThisResource.Size = new System.Drawing.Size(115, 17);
      this.ckInvoiceOnlyThisResource.TabIndex = 2;
      this.ckInvoiceOnlyThisResource.Text = "Only this Resource";
      this.ckInvoiceOnlyThisResource.UseVisualStyleBackColor = true;
      // 
      // dtInvoiceThrough
      // 
      this.dtInvoiceThrough.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtInvoiceThrough.Location = new System.Drawing.Point(154, 17);
      this.dtInvoiceThrough.Name = "dtInvoiceThrough";
      this.dtInvoiceThrough.Size = new System.Drawing.Size(87, 20);
      this.dtInvoiceThrough.TabIndex = 1;
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(9, 21);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(139, 13);
      this.label13.TabIndex = 0;
      this.label13.Text = "Invoice/Worked Thru  Date";
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
      this.splitMain.Panel1.AutoScrollMinSize = new System.Drawing.Size(420, 128);
      this.splitMain.Panel1.Controls.Add(this.grpInvoicing);
      this.splitMain.Panel1.Controls.Add(this.cboResource);
      this.splitMain.Panel1.Controls.Add(this.label3);
      this.splitMain.Panel1.Controls.Add(this.cboCustomer);
      this.splitMain.Panel1.Controls.Add(this.label2);
      this.splitMain.Panel1.Controls.Add(this.cboProject);
      this.splitMain.Panel1.Controls.Add(this.label1);
      // 
      // splitMain.Panel2
      // 
      this.splitMain.Panel2.Controls.Add(this.srchInvoices);
      this.splitMain.Size = new System.Drawing.Size(874, 333);
      this.splitMain.SplitterDistance = 131;
      this.splitMain.TabIndex = 28;
      // 
      // srchInvoices
      // 
      this.srchInvoices.CanSearchLockedColumns = false;
      this.srchInvoices.ColumnName = "CustomerName";
      this.srchInvoices.DisplaySearchCriteria = true;
      this.srchInvoices.Dock = System.Windows.Forms.DockStyle.Fill;
      this.srchInvoices.ExportCriteria = false;
      this.srchInvoices.InnerWhere = "";
      this.srchInvoices.Location = new System.Drawing.Point(0, 0);
      this.srchInvoices.Name = "srchInvoices";
      this.srchInvoices.NameType = ACG.App.Common.CommonData.NameTypes.Customer;
      this.srchInvoices.SearchCriteria = ((System.Collections.Generic.Dictionary<string, string[]>)(resources.GetObject("srchInvoices.SearchCriteria")));
      this.srchInvoices.Size = new System.Drawing.Size(874, 198);
      this.srchInvoices.TabIndex = 0;
      this.srchInvoices.Title = "Search (0 Records Found)";
      this.srchInvoices.UniqueIdentifier = "ID";
      this.srchInvoices.RowSelected += new ACG.DesktopClient.Common.ctlSearchGrid.RowSelectedHandler(this.srchInvoices_RowSelected);
      // 
      // frmInvoicePrint
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(874, 333);
      this.Controls.Add(this.splitMain);
      this.Name = "frmInvoicePrint";
      this.Text = "frmInvoicePrint";
      this.Load += new System.EventHandler(this.frmInvoicePrint_Load);
      this.grpInvoicing.ResumeLayout(false);
      this.grpInvoicing.PerformLayout();
      this.splitMain.Panel1.ResumeLayout(false);
      this.splitMain.Panel1.PerformLayout();
      this.splitMain.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
      this.splitMain.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ComboBox cboProject;
    private System.Windows.Forms.ComboBox cboCustomer;
    private System.Windows.Forms.ComboBox cboResource;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.GroupBox grpInvoicing;
    private System.Windows.Forms.Button btnUnpost;
    private System.Windows.Forms.CheckBox ckIncludePosted;
    private System.Windows.Forms.Button btnPostInvoice;
    private System.Windows.Forms.Button btnPreviewInvoice;
    private System.Windows.Forms.CheckBox ckInvoiceOnlyThisProject;
    private System.Windows.Forms.CheckBox ckInvoiceOnlyThisCustomer;
    private System.Windows.Forms.CheckBox ckInvoiceOnlyThisResource;
    private System.Windows.Forms.DateTimePicker dtInvoiceThrough;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.CheckBox ckDetail;
    private System.Windows.Forms.CheckBox ckExcludeProject;
    private System.Windows.Forms.SplitContainer splitMain;
    private Common.ctlSearchGrid srchInvoices;
  }
}