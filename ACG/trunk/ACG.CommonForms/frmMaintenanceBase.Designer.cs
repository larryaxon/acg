﻿namespace ACG.CommonForms
{
  partial class frmMaintenanceBase
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
      this.splitEdit = new System.Windows.Forms.SplitContainer();
      this.ctlSearchMain = new ACG.CommonForms.ctlSearch();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnNew = new System.Windows.Forms.Button();
      this.btnSave = new System.Windows.Forms.Button();
      this.panelEdit = new System.Windows.Forms.Panel();
      this.lblSearchMain = new System.Windows.Forms.Label();
      this.panelReadOnly = new System.Windows.Forms.Panel();
      this.ctlSearchList = new ACG.CommonForms.ctlSearchGrid();
      this.splitMain.Panel1.SuspendLayout();
      this.splitMain.Panel2.SuspendLayout();
      this.splitMain.SuspendLayout();
      this.splitEdit.Panel1.SuspendLayout();
      this.splitEdit.Panel2.SuspendLayout();
      this.splitEdit.SuspendLayout();
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
      this.splitMain.Panel1.Controls.Add(this.splitEdit);
      // 
      // splitMain.Panel2
      // 
      this.splitMain.Panel2.Controls.Add(this.ctlSearchList);
      this.splitMain.Size = new System.Drawing.Size(1090, 521);
      this.splitMain.SplitterDistance = 363;
      this.splitMain.TabIndex = 0;
      // 
      // splitEdit
      // 
      this.splitEdit.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitEdit.Location = new System.Drawing.Point(0, 0);
      this.splitEdit.Name = "splitEdit";
      // 
      // splitEdit.Panel1
      // 
      this.splitEdit.Panel1.Controls.Add(this.ctlSearchMain);
      this.splitEdit.Panel1.Controls.Add(this.btnCancel);
      this.splitEdit.Panel1.Controls.Add(this.btnNew);
      this.splitEdit.Panel1.Controls.Add(this.btnSave);
      this.splitEdit.Panel1.Controls.Add(this.panelEdit);
      this.splitEdit.Panel1.Controls.Add(this.lblSearchMain);
      // 
      // splitEdit.Panel2
      // 
      this.splitEdit.Panel2.Controls.Add(this.panelReadOnly);
      this.splitEdit.Size = new System.Drawing.Size(1090, 363);
      this.splitEdit.SplitterDistance = 746;
      this.splitEdit.TabIndex = 0;
      // 
      // ctlSearchMain
      // 
      this.ctlSearchMain.AddNewMode = false;
      this.ctlSearchMain.AutoAddNewMode = false;
      this.ctlSearchMain.AutoSelectWhenMatch = false;
      this.ctlSearchMain.AutoTabToNextControlOnSelect = false;
      this.ctlSearchMain.ClearSearchWhenComplete = true;
      this.ctlSearchMain.Collapsed = true;
      this.ctlSearchMain.CreatedNewItem = false;
      this.ctlSearchMain.DisplayOnlyDescription = false;
      this.ctlSearchMain.DisplayOnlyID = false;
      this.ctlSearchMain.FixKeySpace = "-1";
      this.ctlSearchMain.ID = "";
      this.ctlSearchMain.Location = new System.Drawing.Point(74, 5);
      this.ctlSearchMain.MaxHeight = 228;
      this.ctlSearchMain.MustExistInList = true;
      this.ctlSearchMain.MustExistMessage = "You must enter a valid value";
      this.ctlSearchMain.Name = "ctlSearchMain";
      this.ctlSearchMain.SearchExec = null;
      this.ctlSearchMain.ShowCustomerNameWhenSet = false;
      this.ctlSearchMain.ShowTermedCheckBox = false;
      this.ctlSearchMain.Size = new System.Drawing.Size(308, 26);
      this.ctlSearchMain.TabIndex = 0;
      this.ctlSearchMain.OnSelected += new System.EventHandler<System.EventArgs>(this.ctlSearchMain_OnSelected);
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(512, 2);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(73, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // btnNew
      // 
      this.btnNew.Location = new System.Drawing.Point(591, 2);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new System.Drawing.Size(73, 23);
      this.btnNew.TabIndex = 4;
      this.btnNew.Text = "New";
      this.btnNew.UseVisualStyleBackColor = true;
      this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
      // 
      // btnSave
      // 
      this.btnSave.Location = new System.Drawing.Point(670, 2);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(73, 23);
      this.btnSave.TabIndex = 3;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // panelEdit
      // 
      this.panelEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panelEdit.Location = new System.Drawing.Point(3, 31);
      this.panelEdit.Name = "panelEdit";
      this.panelEdit.Size = new System.Drawing.Size(740, 331);
      this.panelEdit.TabIndex = 2;
      // 
      // lblSearchMain
      // 
      this.lblSearchMain.AutoSize = true;
      this.lblSearchMain.Location = new System.Drawing.Point(4, 8);
      this.lblSearchMain.Name = "lblSearchMain";
      this.lblSearchMain.Size = new System.Drawing.Size(41, 13);
      this.lblSearchMain.TabIndex = 1;
      this.lblSearchMain.Text = "Search";
      // 
      // panelReadOnly
      // 
      this.panelReadOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panelReadOnly.Location = new System.Drawing.Point(5, 30);
      this.panelReadOnly.Name = "panelReadOnly";
      this.panelReadOnly.Size = new System.Drawing.Size(334, 331);
      this.panelReadOnly.TabIndex = 0;
      // 
      // ctlSearchList
      // 
      this.ctlSearchList.AllowSortByColumn = true;
      this.ctlSearchList.AutoRefreshWhenFieldChecked = false;
      this.ctlSearchList.CanChangeDisplayFields = true;
      this.ctlSearchList.CanChangeDisplaySearchCriteria = true;
      this.ctlSearchList.ColumnName = "";
      this.ctlSearchList.DisplayFields = false;
      this.ctlSearchList.DisplaySearchCriteria = false;
      this.ctlSearchList.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ctlSearchList.FieldsDefaultIsChecked = true;
      this.ctlSearchList.ForceReloadSearchColumns = false;
      this.ctlSearchList.IDList = null;
      this.ctlSearchList.IncludeGroupAsCriteria = false;
      this.ctlSearchList.InnerWhere = "";
      this.ctlSearchList.Location = new System.Drawing.Point(0, 0);
      this.ctlSearchList.Name = "ctlSearchList";
      this.ctlSearchList.NameType = null;
      this.ctlSearchList.SearchCriteria = null;
      this.ctlSearchList.Size = new System.Drawing.Size(1090, 154);
      this.ctlSearchList.TabIndex = 0;
      this.ctlSearchList.Title = "Search (0 Records Found)";
      this.ctlSearchList.UniqueIdentifier = "ID";
      this.ctlSearchList.UseNamedSearches = false;
      this.ctlSearchList.RowSelected += new ACG.CommonForms.ctlSearchGrid.RowSelectedHandler(this.ctlSearchList_RowSelected);
      // 
      // frmMaintenanceBase
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1090, 521);
      this.Controls.Add(this.splitMain);
      this.Name = "frmMaintenanceBase";
      this.Text = "frmMaintenanceBase";
      this.Load += new System.EventHandler(this.frmMaintenanceBase_Load);
      this.Resize += new System.EventHandler(this.frmMaintenanceBase_Resize);
      this.splitMain.Panel1.ResumeLayout(false);
      this.splitMain.Panel2.ResumeLayout(false);
      this.splitMain.ResumeLayout(false);
      this.splitEdit.Panel1.ResumeLayout(false);
      this.splitEdit.Panel1.PerformLayout();
      this.splitEdit.Panel2.ResumeLayout(false);
      this.splitEdit.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitMain;
    private System.Windows.Forms.SplitContainer splitEdit;
    private ctlSearchGrid ctlSearchList;
    private ctlSearch ctlSearchMain;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Panel panelEdit;
    private System.Windows.Forms.Label lblSearchMain;
    private System.Windows.Forms.Panel panelReadOnly;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnNew;
  }
}