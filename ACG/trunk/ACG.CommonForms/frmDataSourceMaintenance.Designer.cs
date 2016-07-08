﻿namespace ACG.CommonForms
{
  partial class frmDataSourceMaintenance
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
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.txtDescription = new System.Windows.Forms.TextBox();
      this.txtOrderBy = new System.Windows.Forms.TextBox();
      this.txtMaxCount = new System.Windows.Forms.TextBox();
      this.txtParameterList = new System.Windows.Forms.TextBox();
      this.ckOverrideWhere = new System.Windows.Forms.CheckBox();
      this.srchDataSource = new ACG.CommonForms.ctlSearch();
      this.txtFromClause = new System.Windows.Forms.TextBox();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnNew = new System.Windows.Forms.Button();
      this.btnSave = new System.Windows.Forms.Button();
      this.btnDelete = new System.Windows.Forms.Button();
      this.ctlTestGrid = new ACG.CommonForms.ctlSearchGrid();
      this.btnTest = new System.Windows.Forms.Button();
      this.ckIncludeInAnalysis = new System.Windows.Forms.CheckBox();
      this.lblTestProgress = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(19, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(67, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Data Source";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(19, 41);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(60, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Description";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(19, 190);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(28, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "SQL";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(19, 65);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(72, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Sort/Order By";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(19, 89);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(122, 13);
      this.label5.TabIndex = 5;
      this.label5.Text = "Max Number of Records";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(19, 113);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(118, 13);
      this.label6.TabIndex = 6;
      this.label6.Text = "Substitution Parameters";
      // 
      // txtDescription
      // 
      this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtDescription.Location = new System.Drawing.Point(174, 38);
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new System.Drawing.Size(542, 20);
      this.txtDescription.TabIndex = 1;
      this.txtDescription.TextChanged += new System.EventHandler(this.txtAny_TextChanged);
      // 
      // txtOrderBy
      // 
      this.txtOrderBy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtOrderBy.Location = new System.Drawing.Point(174, 62);
      this.txtOrderBy.Name = "txtOrderBy";
      this.txtOrderBy.Size = new System.Drawing.Size(542, 20);
      this.txtOrderBy.TabIndex = 2;
      this.txtOrderBy.TextChanged += new System.EventHandler(this.txtAny_TextChanged);
      // 
      // txtMaxCount
      // 
      this.txtMaxCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtMaxCount.Location = new System.Drawing.Point(174, 86);
      this.txtMaxCount.Name = "txtMaxCount";
      this.txtMaxCount.Size = new System.Drawing.Size(542, 20);
      this.txtMaxCount.TabIndex = 3;
      this.txtMaxCount.TextChanged += new System.EventHandler(this.txtAny_TextChanged);
      // 
      // txtParameterList
      // 
      this.txtParameterList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtParameterList.Location = new System.Drawing.Point(174, 110);
      this.txtParameterList.Name = "txtParameterList";
      this.txtParameterList.Size = new System.Drawing.Size(542, 20);
      this.txtParameterList.TabIndex = 4;
      this.txtParameterList.TextChanged += new System.EventHandler(this.txtAny_TextChanged);
      // 
      // ckOverrideWhere
      // 
      this.ckOverrideWhere.AutoSize = true;
      this.ckOverrideWhere.Location = new System.Drawing.Point(22, 136);
      this.ckOverrideWhere.Name = "ckOverrideWhere";
      this.ckOverrideWhere.Size = new System.Drawing.Size(204, 17);
      this.ckOverrideWhere.TabIndex = 5;
      this.ckOverrideWhere.Text = "Don\'t substitute \"AND\" for \"WHERE\"";
      this.ckOverrideWhere.UseVisualStyleBackColor = true;
      this.ckOverrideWhere.CheckedChanged += new System.EventHandler(this.txtAny_TextChanged);
      // 
      // srchDataSource
      // 
      this.srchDataSource.AddNewMode = false;
      this.srchDataSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.srchDataSource.AutoAddNewMode = true;
      this.srchDataSource.AutoSelectWhenMatch = false;
      this.srchDataSource.AutoTabToNextControlOnSelect = true;
      this.srchDataSource.ClearSearchWhenComplete = false;
      this.srchDataSource.Collapsed = true;
      this.srchDataSource.CreatedNewItem = false;
      this.srchDataSource.DisplayOnlyID = false;
      this.srchDataSource.ID = "";
      this.srchDataSource.Location = new System.Drawing.Point(174, 12);
      this.srchDataSource.MaxHeight = 228;
      this.srchDataSource.MustExistInList = false;
      this.srchDataSource.MustExistMessage = "You must enter a valid Data Source";
      this.srchDataSource.Name = "srchDataSource";
      this.srchDataSource.ShowCustomerNameWhenSet = true;
      this.srchDataSource.ShowTermedCheckBox = false;
      this.srchDataSource.Size = new System.Drawing.Size(542, 22);
      this.srchDataSource.TabIndex = 0;
      this.srchDataSource.OnSelected += new System.EventHandler<System.EventArgs>(this.srchDataSource_OnSelected);
      // 
      // txtFromClause
      // 
      this.txtFromClause.AcceptsReturn = true;
      this.txtFromClause.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtFromClause.Location = new System.Drawing.Point(59, 187);
      this.txtFromClause.Multiline = true;
      this.txtFromClause.Name = "txtFromClause";
      this.txtFromClause.Size = new System.Drawing.Size(656, 156);
      this.txtFromClause.TabIndex = 7;
      this.txtFromClause.TextChanged += new System.EventHandler(this.txtAny_TextChanged);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.Location = new System.Drawing.Point(319, 158);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // btnNew
      // 
      this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnNew.Location = new System.Drawing.Point(399, 158);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new System.Drawing.Size(75, 23);
      this.btnNew.TabIndex = 9;
      this.btnNew.Text = "New";
      this.btnNew.UseVisualStyleBackColor = true;
      this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
      // 
      // btnSave
      // 
      this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSave.Location = new System.Drawing.Point(559, 158);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(75, 23);
      this.btnSave.TabIndex = 11;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // btnDelete
      // 
      this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnDelete.Location = new System.Drawing.Point(479, 158);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(75, 23);
      this.btnDelete.TabIndex = 10;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      // 
      // ctlTestGrid
      // 
      this.ctlTestGrid.AllowSortByColumn = true;
      this.ctlTestGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ctlTestGrid.AutoRefreshWhenFieldChecked = false;
      this.ctlTestGrid.CanChangeDisplayFields = true;
      this.ctlTestGrid.CanChangeDisplaySearchCriteria = true;
      this.ctlTestGrid.ColumnName = "";
      this.ctlTestGrid.DisplayFields = false;
      this.ctlTestGrid.DisplaySearchCriteria = true;
      this.ctlTestGrid.FieldsDefaultIsChecked = true;
      this.ctlTestGrid.ForceReloadSearchColumns = false;
      this.ctlTestGrid.IDList = null;
      this.ctlTestGrid.IncludeGroupAsCriteria = false;
      this.ctlTestGrid.InnerWhere = "";
      this.ctlTestGrid.Location = new System.Drawing.Point(9, 352);
      this.ctlTestGrid.Name = "ctlTestGrid";
      this.ctlTestGrid.NameType = null;
      this.ctlTestGrid.SearchCriteria = null;
      this.ctlTestGrid.Size = new System.Drawing.Size(706, 150);
      this.ctlTestGrid.TabIndex = 13;
      this.ctlTestGrid.Title = "Search (0 Records Found)";
      this.ctlTestGrid.UniqueIdentifier = "ID";
      this.ctlTestGrid.UseNamedSearches = false;
      // 
      // btnTest
      // 
      this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnTest.Location = new System.Drawing.Point(640, 158);
      this.btnTest.Name = "btnTest";
      this.btnTest.Size = new System.Drawing.Size(75, 23);
      this.btnTest.TabIndex = 12;
      this.btnTest.Text = "Test";
      this.btnTest.UseVisualStyleBackColor = true;
      this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
      // 
      // ckIncludeInAnalysis
      // 
      this.ckIncludeInAnalysis.AutoSize = true;
      this.ckIncludeInAnalysis.Location = new System.Drawing.Point(240, 136);
      this.ckIncludeInAnalysis.Name = "ckIncludeInAnalysis";
      this.ckIncludeInAnalysis.Size = new System.Drawing.Size(148, 17);
      this.ckIncludeInAnalysis.TabIndex = 6;
      this.ckIncludeInAnalysis.Text = "Include in Analysis Report";
      this.ckIncludeInAnalysis.UseVisualStyleBackColor = true;
      // 
      // lblTestProgress
      // 
      this.lblTestProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblTestProgress.AutoSize = true;
      this.lblTestProgress.ForeColor = System.Drawing.Color.Red;
      this.lblTestProgress.Location = new System.Drawing.Point(522, 138);
      this.lblTestProgress.Name = "lblTestProgress";
      this.lblTestProgress.Size = new System.Drawing.Size(0, 13);
      this.lblTestProgress.TabIndex = 14;
      this.lblTestProgress.Visible = false;
      // 
      // frmDataSourceMaintenance
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(730, 509);
      this.Controls.Add(this.lblTestProgress);
      this.Controls.Add(this.srchDataSource);
      this.Controls.Add(this.btnTest);
      this.Controls.Add(this.ctlTestGrid);
      this.Controls.Add(this.btnDelete);
      this.Controls.Add(this.btnSave);
      this.Controls.Add(this.btnNew);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.txtFromClause);
      this.Controls.Add(this.ckOverrideWhere);
      this.Controls.Add(this.txtParameterList);
      this.Controls.Add(this.txtMaxCount);
      this.Controls.Add(this.txtOrderBy);
      this.Controls.Add(this.txtDescription);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.ckIncludeInAnalysis);
      this.Name = "frmDataSourceMaintenance";
      this.Text = "frmDataSourceMaintenance";
      this.Load += new System.EventHandler(this.frmDataSourceMaintenance_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private ctlSearch srchDataSource;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.TextBox txtOrderBy;
    private System.Windows.Forms.TextBox txtMaxCount;
    private System.Windows.Forms.TextBox txtParameterList;
    private System.Windows.Forms.CheckBox ckOverrideWhere;
    private System.Windows.Forms.TextBox txtFromClause;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnNew;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnDelete;
    private ctlSearchGrid ctlTestGrid;
    private System.Windows.Forms.Button btnTest;
    private System.Windows.Forms.CheckBox ckIncludeInAnalysis;
    private System.Windows.Forms.Label lblTestProgress;
  }
}