﻿namespace CCI.DesktopClient.Screens
{
  partial class frmCityHostedOCCUSOCMatching
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
      this.label1 = new System.Windows.Forms.Label();
      this.dtBillDate = new System.Windows.Forms.DateTimePicker();
      this.splitMain = new System.Windows.Forms.SplitContainer();
      this.txtComment = new System.Windows.Forms.RichTextBox();
      this.srchUsocMatching = new CCI.DesktopClient.Common.ctlSearchGrid();
      this.ctlCityHostedDetail1 = new CCI.DesktopClient.Common.ctlCityHostedDetail();
      this.cboSource = new System.Windows.Forms.ComboBox();
      this.ckShowResearch = new System.Windows.Forms.CheckBox();
      this.btnExecute = new System.Windows.Forms.Button();
      this.cboDisposition = new System.Windows.Forms.ComboBox();
      this.ckIncludeExceptions = new System.Windows.Forms.CheckBox();
      this.lblTitle = new System.Windows.Forms.Label();
      this.lblRowSelected = new System.Windows.Forms.Label();
      this.btnLoad = new System.Windows.Forms.Button();
      this.txtCommentButton = new System.Windows.Forms.TextBox();
      this.btnUndo = new System.Windows.Forms.Button();
      this.splitMain.Panel1.SuspendLayout();
      this.splitMain.Panel2.SuspendLayout();
      this.splitMain.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(46, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Bill Date";
      // 
      // dtBillDate
      // 
      this.dtBillDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtBillDate.Location = new System.Drawing.Point(74, 8);
      this.dtBillDate.Name = "dtBillDate";
      this.dtBillDate.Size = new System.Drawing.Size(106, 20);
      this.dtBillDate.TabIndex = 1;
      this.dtBillDate.ValueChanged += new System.EventHandler(this.dtBillDate_ValueChanged);
      // 
      // splitMain
      // 
      this.splitMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.splitMain.Location = new System.Drawing.Point(2, 31);
      this.splitMain.Name = "splitMain";
      this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitMain.Panel1
      // 
      this.splitMain.Panel1.Controls.Add(this.txtComment);
      this.splitMain.Panel1.Controls.Add(this.srchUsocMatching);
      // 
      // splitMain.Panel2
      // 
      this.splitMain.Panel2.Controls.Add(this.ctlCityHostedDetail1);
      this.splitMain.Size = new System.Drawing.Size(1359, 597);
      this.splitMain.SplitterDistance = 287;
      this.splitMain.TabIndex = 2;
      // 
      // txtComment
      // 
      this.txtComment.Location = new System.Drawing.Point(944, 1);
      this.txtComment.Name = "txtComment";
      this.txtComment.Size = new System.Drawing.Size(261, 126);
      this.txtComment.TabIndex = 2;
      this.txtComment.Text = "";
      this.txtComment.Leave += new System.EventHandler(this.txtComment_Leave);
      // 
      // srchUsocMatching
      // 
      this.srchUsocMatching.AllowSortByColumn = true;
      this.srchUsocMatching.AutoRefreshWhenFieldChecked = false;
      this.srchUsocMatching.CanChangeDisplayFields = true;
      this.srchUsocMatching.CanChangeDisplaySearchCriteria = true;
      this.srchUsocMatching.ColumnName = "CustomerName";
      this.srchUsocMatching.DisplayFields = false;
      this.srchUsocMatching.DisplaySearchCriteria = false;
      this.srchUsocMatching.Dock = System.Windows.Forms.DockStyle.Fill;
      this.srchUsocMatching.FieldsDefaultIsChecked = true;
      this.srchUsocMatching.ForceReloadSearchColumns = false;
      this.srchUsocMatching.IDList = null;
      this.srchUsocMatching.IncludeGroupAsCriteria = false;
      this.srchUsocMatching.InnerWhere = "";
      this.srchUsocMatching.Location = new System.Drawing.Point(0, 0);
      this.srchUsocMatching.Name = "srchUsocMatching";
      this.srchUsocMatching.NameType = CCI.Common.CommonData.UnmatchedNameTypes.Customer;
      this.srchUsocMatching.SearchCriteria = null;
      this.srchUsocMatching.Size = new System.Drawing.Size(1359, 287);
      this.srchUsocMatching.TabIndex = 0;
      this.srchUsocMatching.Title = "Search (0 Records Found)";
      this.srchUsocMatching.UniqueIdentifier = "ID";
      this.srchUsocMatching.UseNamedSearches = false;
      this.srchUsocMatching.RowSelected += new ACG.CommonForms.ctlSearchGrid.RowSelectedHandler(this.srchUsocMatching_RowSelected);
      // 
      // ctlCityHostedDetail1
      // 
      this.ctlCityHostedDetail1.BillDate = null;
      this.ctlCityHostedDetail1.BTN = null;
      this.ctlCityHostedDetail1.CustomerID = null;
      this.ctlCityHostedDetail1.CustomerName = null;
      this.ctlCityHostedDetail1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ctlCityHostedDetail1.Location = new System.Drawing.Point(0, 0);
      this.ctlCityHostedDetail1.Name = "ctlCityHostedDetail1";
      this.ctlCityHostedDetail1.SecurityContext = null;
      this.ctlCityHostedDetail1.Size = new System.Drawing.Size(1359, 306);
      this.ctlCityHostedDetail1.TabIndex = 0;
      this.ctlCityHostedDetail1.USOC = null;
      // 
      // cboSource
      // 
      this.cboSource.FormattingEnabled = true;
      this.cboSource.Items.AddRange(new object[] {
            "Unmatched",
            "QtyMismatch",
            "WholesaleMismatch",
            "RetailOnly",
            "WholesaleOnly",
            "Matched",
            "All"});
      this.cboSource.Location = new System.Drawing.Point(186, 8);
      this.cboSource.Name = "cboSource";
      this.cboSource.Size = new System.Drawing.Size(130, 21);
      this.cboSource.TabIndex = 3;
      this.cboSource.SelectedIndexChanged += new System.EventHandler(this.cboSource_SelectedIndexChanged);
      // 
      // ckShowResearch
      // 
      this.ckShowResearch.AutoSize = true;
      this.ckShowResearch.Location = new System.Drawing.Point(331, 10);
      this.ckShowResearch.Name = "ckShowResearch";
      this.ckShowResearch.Size = new System.Drawing.Size(83, 17);
      this.ckShowResearch.TabIndex = 5;
      this.ckShowResearch.Text = "Show Detail";
      this.ckShowResearch.UseVisualStyleBackColor = true;
      this.ckShowResearch.CheckedChanged += new System.EventHandler(this.ckShowResearch_CheckedChanged);
      // 
      // btnExecute
      // 
      this.btnExecute.Location = new System.Drawing.Point(1153, 4);
      this.btnExecute.Name = "btnExecute";
      this.btnExecute.Size = new System.Drawing.Size(93, 23);
      this.btnExecute.TabIndex = 10;
      this.btnExecute.Text = "Fl&ag Exception";
      this.btnExecute.UseVisualStyleBackColor = true;
      this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
      // 
      // cboDisposition
      // 
      this.cboDisposition.DropDownWidth = 250;
      this.cboDisposition.FormattingEnabled = true;
      this.cboDisposition.Location = new System.Drawing.Point(876, 6);
      this.cboDisposition.Name = "cboDisposition";
      this.cboDisposition.Size = new System.Drawing.Size(120, 21);
      this.cboDisposition.TabIndex = 9;
      // 
      // ckIncludeExceptions
      // 
      this.ckIncludeExceptions.AutoSize = true;
      this.ckIncludeExceptions.Location = new System.Drawing.Point(420, 10);
      this.ckIncludeExceptions.Name = "ckIncludeExceptions";
      this.ckIncludeExceptions.Size = new System.Drawing.Size(116, 17);
      this.ckIncludeExceptions.TabIndex = 11;
      this.ckIncludeExceptions.Text = "Include Exceptions";
      this.ckIncludeExceptions.UseVisualStyleBackColor = true;
      this.ckIncludeExceptions.CheckedChanged += new System.EventHandler(this.ckIncludeExceptions_CheckedChanged);
      // 
      // lblTitle
      // 
      this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblTitle.AutoSize = true;
      this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTitle.Location = new System.Drawing.Point(1253, 6);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new System.Drawing.Size(108, 24);
      this.lblTitle.TabIndex = 12;
      this.lblTitle.Text = "Wholesale";
      // 
      // lblRowSelected
      // 
      this.lblRowSelected.AutoSize = true;
      this.lblRowSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblRowSelected.ForeColor = System.Drawing.Color.Red;
      this.lblRowSelected.Location = new System.Drawing.Point(542, 12);
      this.lblRowSelected.Name = "lblRowSelected";
      this.lblRowSelected.Size = new System.Drawing.Size(86, 13);
      this.lblRowSelected.TabIndex = 13;
      this.lblRowSelected.Text = "Row Selected";
      this.lblRowSelected.Visible = false;
      // 
      // btnLoad
      // 
      this.btnLoad.Location = new System.Drawing.Point(786, 4);
      this.btnLoad.Name = "btnLoad";
      this.btnLoad.Size = new System.Drawing.Size(75, 23);
      this.btnLoad.TabIndex = 14;
      this.btnLoad.Text = "Load";
      this.btnLoad.UseVisualStyleBackColor = true;
      this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
      // 
      // txtCommentButton
      // 
      this.txtCommentButton.Location = new System.Drawing.Point(1002, 6);
      this.txtCommentButton.Name = "txtCommentButton";
      this.txtCommentButton.Size = new System.Drawing.Size(143, 20);
      this.txtCommentButton.TabIndex = 17;
      this.txtCommentButton.Click += new System.EventHandler(this.txtCommentButton_Click);
      // 
      // btnUndo
      // 
      this.btnUndo.Location = new System.Drawing.Point(1154, 4);
      this.btnUndo.Name = "btnUndo";
      this.btnUndo.Size = new System.Drawing.Size(93, 23);
      this.btnUndo.TabIndex = 21;
      this.btnUndo.Text = "Undo Exception";
      this.btnUndo.UseVisualStyleBackColor = true;
      this.btnUndo.Visible = false;
      this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
      // 
      // frmCityHostedOCCUSOCMatching
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1363, 630);
      this.Controls.Add(this.btnUndo);
      this.Controls.Add(this.txtCommentButton);
      this.Controls.Add(this.btnLoad);
      this.Controls.Add(this.lblRowSelected);
      this.Controls.Add(this.lblTitle);
      this.Controls.Add(this.ckIncludeExceptions);
      this.Controls.Add(this.btnExecute);
      this.Controls.Add(this.cboDisposition);
      this.Controls.Add(this.ckShowResearch);
      this.Controls.Add(this.cboSource);
      this.Controls.Add(this.splitMain);
      this.Controls.Add(this.dtBillDate);
      this.Controls.Add(this.label1);
      this.Name = "frmCityHostedOCCUSOCMatching";
      this.Text = "frmCityHostedOCCUSOCMatching";
      this.Load += new System.EventHandler(this.frmCityHostedCustomerUSOCMatching_Load);
      this.splitMain.Panel1.ResumeLayout(false);
      this.splitMain.Panel2.ResumeLayout(false);
      this.splitMain.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.DateTimePicker dtBillDate;
    private System.Windows.Forms.SplitContainer splitMain;
    private Common.ctlSearchGrid srchUsocMatching;
    private System.Windows.Forms.ComboBox cboSource;
    private Common.ctlCityHostedDetail ctlCityHostedDetail1;
    private System.Windows.Forms.CheckBox ckShowResearch;
    private System.Windows.Forms.Button btnExecute;
    private System.Windows.Forms.ComboBox cboDisposition;
    private System.Windows.Forms.CheckBox ckIncludeExceptions;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblRowSelected;
    private System.Windows.Forms.Button btnLoad;
    private System.Windows.Forms.RichTextBox txtComment;
    private System.Windows.Forms.TextBox txtCommentButton;
    private System.Windows.Forms.Button btnUndo;
  }
}