﻿namespace CCI.DesktopClient.Screens
{
  partial class frmMRCNetworkInventoryMatching
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
      this.srchMRCNIMatching = new CCI.DesktopClient.Common.ctlSearchGrid();
      this.ckUnmatchedOnly = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.dtBillDate = new System.Windows.Forms.DateTimePicker();
      this.ckWholesale = new System.Windows.Forms.CheckBox();
      this.lblExceptionSearch = new System.Windows.Forms.Label();
      this.cboExceptionSearch = new System.Windows.Forms.ComboBox();
      this.lblTitle = new System.Windows.Forms.Label();
      this.cboDisposition = new System.Windows.Forms.ComboBox();
      this.btnExecute = new System.Windows.Forms.Button();
      this.lblRowSelected = new System.Windows.Forms.Label();
      this.ckIncludeExceptions = new System.Windows.Forms.CheckBox();
      this.btnLoad = new System.Windows.Forms.Button();
      this.txtComment = new System.Windows.Forms.RichTextBox();
      this.txtCommentButton = new System.Windows.Forms.TextBox();
      this.btnUndo = new System.Windows.Forms.Button();
      this.ckInclude500s = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // srchMRCNIMatching
      // 
      this.srchMRCNIMatching.AllowSortByColumn = true;
      this.srchMRCNIMatching.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.srchMRCNIMatching.AutoRefreshWhenFieldChecked = false;
      this.srchMRCNIMatching.AutoSaveUserOptions = false;
      this.srchMRCNIMatching.CanChangeDisplayFields = true;
      this.srchMRCNIMatching.CanChangeDisplaySearchCriteria = true;
      this.srchMRCNIMatching.ColumnName = "CustomerName";
      this.srchMRCNIMatching.DisplayFields = false;
      this.srchMRCNIMatching.DisplaySearchCriteria = false;
      this.srchMRCNIMatching.FieldsDefaultIsChecked = true;
      this.srchMRCNIMatching.ForceReloadSearchColumns = true;
      this.srchMRCNIMatching.IDList = null;
      this.srchMRCNIMatching.IncludeGroupAsCriteria = false;
      this.srchMRCNIMatching.InnerWhere = "";
      this.srchMRCNIMatching.Location = new System.Drawing.Point(2, 36);
      this.srchMRCNIMatching.Name = "srchMRCNIMatching";
      this.srchMRCNIMatching.NameType = CCI.Common.CommonData.UnmatchedNameTypes.Customer;
      this.srchMRCNIMatching.SearchCriteria = null;
      this.srchMRCNIMatching.Size = new System.Drawing.Size(1351, 576);
      this.srchMRCNIMatching.TabIndex = 12;
      this.srchMRCNIMatching.Title = "Search (0 Records Found)";
      this.srchMRCNIMatching.UniqueIdentifier = "ID";
      this.srchMRCNIMatching.UseNamedSearches = false;
      this.srchMRCNIMatching.RowSelected += new ACG.CommonForms.ctlSearchGrid.RowSelectedHandler(this.srchMRCNIMatching_RowSelected);
      // 
      // ckUnmatchedOnly
      // 
      this.ckUnmatchedOnly.AutoSize = true;
      this.ckUnmatchedOnly.Checked = true;
      this.ckUnmatchedOnly.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckUnmatchedOnly.Location = new System.Drawing.Point(399, 5);
      this.ckUnmatchedOnly.Name = "ckUnmatchedOnly";
      this.ckUnmatchedOnly.Size = new System.Drawing.Size(143, 17);
      this.ckUnmatchedOnly.TabIndex = 2;
      this.ckUnmatchedOnly.Text = "Include Only Unmatched";
      this.ckUnmatchedOnly.UseVisualStyleBackColor = true;
      this.ckUnmatchedOnly.CheckedChanged += new System.EventHandler(this.ckUnmatchedOnly_CheckedChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 7);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(46, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Bill Date";
      // 
      // dtBillDate
      // 
      this.dtBillDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dtBillDate.Location = new System.Drawing.Point(58, 3);
      this.dtBillDate.Name = "dtBillDate";
      this.dtBillDate.Size = new System.Drawing.Size(106, 20);
      this.dtBillDate.TabIndex = 0;
      // 
      // ckWholesale
      // 
      this.ckWholesale.AutoSize = true;
      this.ckWholesale.Location = new System.Drawing.Point(1274, 8);
      this.ckWholesale.Name = "ckWholesale";
      this.ckWholesale.Size = new System.Drawing.Size(15, 14);
      this.ckWholesale.TabIndex = 10;
      this.ckWholesale.UseVisualStyleBackColor = true;
      this.ckWholesale.CheckedChanged += new System.EventHandler(this.ckWholesale_CheckedChanged);
      // 
      // lblExceptionSearch
      // 
      this.lblExceptionSearch.AutoSize = true;
      this.lblExceptionSearch.Location = new System.Drawing.Point(170, 7);
      this.lblExceptionSearch.Name = "lblExceptionSearch";
      this.lblExceptionSearch.Size = new System.Drawing.Size(54, 13);
      this.lblExceptionSearch.TabIndex = 5;
      this.lblExceptionSearch.Text = "Exception";
      // 
      // cboExceptionSearch
      // 
      this.cboExceptionSearch.FormattingEnabled = true;
      this.cboExceptionSearch.Location = new System.Drawing.Point(225, 3);
      this.cboExceptionSearch.Name = "cboExceptionSearch";
      this.cboExceptionSearch.Size = new System.Drawing.Size(170, 21);
      this.cboExceptionSearch.TabIndex = 1;
      this.cboExceptionSearch.SelectedIndexChanged += new System.EventHandler(this.cboExceptionSearch_SelectedIndexChanged);
      // 
      // lblTitle
      // 
      this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblTitle.AutoSize = true;
      this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTitle.Location = new System.Drawing.Point(1291, 1);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new System.Drawing.Size(62, 24);
      this.lblTitle.TabIndex = 11;
      this.lblTitle.Text = "Retail";
      // 
      // cboDisposition
      // 
      this.cboDisposition.DropDownWidth = 250;
      this.cboDisposition.FormattingEnabled = true;
      this.cboDisposition.Location = new System.Drawing.Point(896, 6);
      this.cboDisposition.Name = "cboDisposition";
      this.cboDisposition.Size = new System.Drawing.Size(124, 21);
      this.cboDisposition.TabIndex = 7;
      // 
      // btnExecute
      // 
      this.btnExecute.Location = new System.Drawing.Point(1175, 4);
      this.btnExecute.Name = "btnExecute";
      this.btnExecute.Size = new System.Drawing.Size(93, 23);
      this.btnExecute.TabIndex = 6;
      this.btnExecute.Text = "Flag Exception";
      this.btnExecute.UseVisualStyleBackColor = true;
      this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
      // 
      // lblRowSelected
      // 
      this.lblRowSelected.AutoSize = true;
      this.lblRowSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblRowSelected.ForeColor = System.Drawing.Color.Red;
      this.lblRowSelected.Location = new System.Drawing.Point(738, 7);
      this.lblRowSelected.Name = "lblRowSelected";
      this.lblRowSelected.Size = new System.Drawing.Size(86, 13);
      this.lblRowSelected.TabIndex = 5;
      this.lblRowSelected.Text = "Row Selected";
      this.lblRowSelected.Visible = false;
      // 
      // ckIncludeExceptions
      // 
      this.ckIncludeExceptions.AutoSize = true;
      this.ckIncludeExceptions.Location = new System.Drawing.Point(544, 5);
      this.ckIncludeExceptions.Name = "ckIncludeExceptions";
      this.ckIncludeExceptions.Size = new System.Drawing.Size(116, 17);
      this.ckIncludeExceptions.TabIndex = 3;
      this.ckIncludeExceptions.Text = "Include Exceptions";
      this.ckIncludeExceptions.UseVisualStyleBackColor = true;
      this.ckIncludeExceptions.CheckedChanged += new System.EventHandler(this.ckIncludeExceptions_CheckedChanged);
      // 
      // btnLoad
      // 
      this.btnLoad.Location = new System.Drawing.Point(830, 4);
      this.btnLoad.Name = "btnLoad";
      this.btnLoad.Size = new System.Drawing.Size(60, 23);
      this.btnLoad.TabIndex = 6;
      this.btnLoad.Text = "Load";
      this.btnLoad.UseVisualStyleBackColor = true;
      this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
      // 
      // txtComment
      // 
      this.txtComment.Location = new System.Drawing.Point(969, 45);
      this.txtComment.Name = "txtComment";
      this.txtComment.Size = new System.Drawing.Size(261, 126);
      this.txtComment.TabIndex = 13;
      this.txtComment.Text = "";
      this.txtComment.Leave += new System.EventHandler(this.txtComment_Leave);
      // 
      // txtCommentButton
      // 
      this.txtCommentButton.Location = new System.Drawing.Point(1026, 6);
      this.txtCommentButton.Name = "txtCommentButton";
      this.txtCommentButton.Size = new System.Drawing.Size(143, 20);
      this.txtCommentButton.TabIndex = 8;
      this.txtCommentButton.Click += new System.EventHandler(this.txtCommentButton_Click);
      // 
      // btnUndo
      // 
      this.btnUndo.Location = new System.Drawing.Point(1175, 4);
      this.btnUndo.Name = "btnUndo";
      this.btnUndo.Size = new System.Drawing.Size(93, 23);
      this.btnUndo.TabIndex = 9;
      this.btnUndo.Text = "Undo Exception";
      this.btnUndo.UseVisualStyleBackColor = true;
      this.btnUndo.Visible = false;
      this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
      // 
      // ckInclude500s
      // 
      this.ckInclude500s.AutoSize = true;
      this.ckInclude500s.Location = new System.Drawing.Point(660, 5);
      this.ckInclude500s.Name = "ckInclude500s";
      this.ckInclude500s.Size = new System.Drawing.Size(87, 17);
      this.ckInclude500s.TabIndex = 4;
      this.ckInclude500s.Text = "Include 500s";
      this.ckInclude500s.UseVisualStyleBackColor = true;
      // 
      // frmMRCNetworkInventoryMatching
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1354, 611);
      this.Controls.Add(this.ckInclude500s);
      this.Controls.Add(this.btnUndo);
      this.Controls.Add(this.txtCommentButton);
      this.Controls.Add(this.txtComment);
      this.Controls.Add(this.btnLoad);
      this.Controls.Add(this.ckIncludeExceptions);
      this.Controls.Add(this.lblRowSelected);
      this.Controls.Add(this.btnExecute);
      this.Controls.Add(this.cboDisposition);
      this.Controls.Add(this.lblTitle);
      this.Controls.Add(this.cboExceptionSearch);
      this.Controls.Add(this.lblExceptionSearch);
      this.Controls.Add(this.ckWholesale);
      this.Controls.Add(this.dtBillDate);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.ckUnmatchedOnly);
      this.Controls.Add(this.srchMRCNIMatching);
      this.Name = "frmMRCNetworkInventoryMatching";
      this.Text = "frmMRCNetworkInventoryMatching";
      this.Load += new System.EventHandler(this.frmMRCNetworkInventoryMatching_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private Common.ctlSearchGrid srchMRCNIMatching;
    private System.Windows.Forms.CheckBox ckUnmatchedOnly;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.DateTimePicker dtBillDate;
    private System.Windows.Forms.CheckBox ckWholesale;
    private System.Windows.Forms.Label lblExceptionSearch;
    private System.Windows.Forms.ComboBox cboExceptionSearch;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.ComboBox cboDisposition;
    private System.Windows.Forms.Button btnExecute;
    private System.Windows.Forms.Label lblRowSelected;
    private System.Windows.Forms.CheckBox ckIncludeExceptions;
    private System.Windows.Forms.Button btnLoad;
    private System.Windows.Forms.RichTextBox txtComment;
    private System.Windows.Forms.TextBox txtCommentButton;
    private System.Windows.Forms.Button btnUndo;
    private System.Windows.Forms.CheckBox ckInclude500s;
  }
}