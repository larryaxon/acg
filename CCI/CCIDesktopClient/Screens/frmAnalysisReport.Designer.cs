namespace CCI.DesktopClient.Screens
{
  partial class frmAnalysisReport
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
      this.srchBottomGrid = new ACG.CommonForms.ctlSearch();
      this.srchMiddleGrid = new ACG.CommonForms.ctlSearch();
      this.srchTopGrid = new ACG.CommonForms.ctlSearch();
      this.srchNamedSearch = new ACG.CommonForms.ctlSearch();
      this.btnClear = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.btnGroups = new System.Windows.Forms.Button();
      this.ckFilterByIDList = new System.Windows.Forms.CheckBox();
      this.splitMain = new System.Windows.Forms.SplitContainer();
      this.splitTop = new System.Windows.Forms.SplitContainer();
      this.topGrid = new CCI.DesktopClient.Common.ctlSearchGrid();
      this.middleGrid = new CCI.DesktopClient.Common.ctlSearchGrid();
      this.bottomGrid = new CCI.DesktopClient.Common.ctlSearchGrid();
      this.splitMain.Panel1.SuspendLayout();
      this.splitMain.Panel2.SuspendLayout();
      this.splitMain.SuspendLayout();
      this.splitTop.Panel1.SuspendLayout();
      this.splitTop.Panel2.SuspendLayout();
      this.splitTop.SuspendLayout();
      this.SuspendLayout();
      // 
      // srchBottomGrid
      // 
      this.srchBottomGrid.AddNewMode = false;
      this.srchBottomGrid.AutoAddNewMode = false;
      this.srchBottomGrid.AutoSelectWhenMatch = false;
      this.srchBottomGrid.AutoTabToNextControlOnSelect = false;
      this.srchBottomGrid.ClearSearchWhenComplete = false;
      this.srchBottomGrid.Collapsed = true;
      this.srchBottomGrid.CreatedNewItem = false;
      this.srchBottomGrid.DisplayOnlyDescription = true;
      this.srchBottomGrid.DisplayOnlyID = false;
      this.srchBottomGrid.FixKeySpace = "-1";
      this.srchBottomGrid.ID = "";
      this.srchBottomGrid.ID_DescSplitter = ":";
      this.srchBottomGrid.Location = new System.Drawing.Point(878, 0);
      this.srchBottomGrid.MaxHeight = 228;
      this.srchBottomGrid.MustExistInList = true;
      this.srchBottomGrid.MustExistMessage = "You must select a valid query";
      this.srchBottomGrid.Name = "srchBottomGrid";
      this.srchBottomGrid.SearchExec = null;
      this.srchBottomGrid.ShowCustomerNameWhenSet = true;
      this.srchBottomGrid.ShowTermedCheckBox = false;
      this.srchBottomGrid.Size = new System.Drawing.Size(204, 24);
      this.srchBottomGrid.TabIndex = 11;
      this.srchBottomGrid.OnSelected += new System.EventHandler<System.EventArgs>(this.cboBottomGrid_SelectedIndexChanged);
      // 
      // srchMiddleGrid
      // 
      this.srchMiddleGrid.AddNewMode = false;
      this.srchMiddleGrid.AutoAddNewMode = false;
      this.srchMiddleGrid.AutoSelectWhenMatch = false;
      this.srchMiddleGrid.AutoTabToNextControlOnSelect = false;
      this.srchMiddleGrid.ClearSearchWhenComplete = false;
      this.srchMiddleGrid.Collapsed = true;
      this.srchMiddleGrid.CreatedNewItem = false;
      this.srchMiddleGrid.DisplayOnlyDescription = true;
      this.srchMiddleGrid.DisplayOnlyID = false;
      this.srchMiddleGrid.FixKeySpace = "-1";
      this.srchMiddleGrid.ID = "";
      this.srchMiddleGrid.ID_DescSplitter = ":";
      this.srchMiddleGrid.Location = new System.Drawing.Point(669, 0);
      this.srchMiddleGrid.MaxHeight = 228;
      this.srchMiddleGrid.MustExistInList = true;
      this.srchMiddleGrid.MustExistMessage = "You must select a valid query";
      this.srchMiddleGrid.Name = "srchMiddleGrid";
      this.srchMiddleGrid.SearchExec = null;
      this.srchMiddleGrid.ShowCustomerNameWhenSet = true;
      this.srchMiddleGrid.ShowTermedCheckBox = false;
      this.srchMiddleGrid.Size = new System.Drawing.Size(204, 24);
      this.srchMiddleGrid.TabIndex = 10;
      this.srchMiddleGrid.OnSelected += new System.EventHandler<System.EventArgs>(this.cboMiddleGrid_SelectedIndexChanged);
      // 
      // srchTopGrid
      // 
      this.srchTopGrid.AddNewMode = false;
      this.srchTopGrid.AutoAddNewMode = false;
      this.srchTopGrid.AutoSelectWhenMatch = false;
      this.srchTopGrid.AutoTabToNextControlOnSelect = false;
      this.srchTopGrid.ClearSearchWhenComplete = false;
      this.srchTopGrid.Collapsed = true;
      this.srchTopGrid.CreatedNewItem = false;
      this.srchTopGrid.DisplayOnlyDescription = true;
      this.srchTopGrid.DisplayOnlyID = false;
      this.srchTopGrid.FixKeySpace = "-1";
      this.srchTopGrid.ID = "";
      this.srchTopGrid.ID_DescSplitter = ":";
      this.srchTopGrid.Location = new System.Drawing.Point(460, 0);
      this.srchTopGrid.MaxHeight = 228;
      this.srchTopGrid.MustExistInList = true;
      this.srchTopGrid.MustExistMessage = "You must select a valid query";
      this.srchTopGrid.Name = "srchTopGrid";
      this.srchTopGrid.SearchExec = null;
      this.srchTopGrid.ShowCustomerNameWhenSet = true;
      this.srchTopGrid.ShowTermedCheckBox = false;
      this.srchTopGrid.Size = new System.Drawing.Size(204, 24);
      this.srchTopGrid.TabIndex = 9;
      this.srchTopGrid.OnSelected += new System.EventHandler<System.EventArgs>(this.cboTopGrid_SelectedIndexChanged);
      // 
      // srchNamedSearch
      // 
      this.srchNamedSearch.AddNewMode = false;
      this.srchNamedSearch.AutoAddNewMode = false;
      this.srchNamedSearch.AutoSelectWhenMatch = false;
      this.srchNamedSearch.AutoTabToNextControlOnSelect = true;
      this.srchNamedSearch.ClearSearchWhenComplete = false;
      this.srchNamedSearch.Collapsed = true;
      this.srchNamedSearch.CreatedNewItem = false;
      this.srchNamedSearch.DisplayOnlyDescription = false;
      this.srchNamedSearch.DisplayOnlyID = false;
      this.srchNamedSearch.FixKeySpace = "-1";
      this.srchNamedSearch.ID = "";
      this.srchNamedSearch.ID_DescSplitter = ":";
      this.srchNamedSearch.Location = new System.Drawing.Point(1081, 2);
      this.srchNamedSearch.MaxHeight = 228;
      this.srchNamedSearch.MustExistInList = false;
      this.srchNamedSearch.MustExistMessage = "You must enter a valid value";
      this.srchNamedSearch.Name = "srchNamedSearch";
      this.srchNamedSearch.SearchExec = null;
      this.srchNamedSearch.ShowCustomerNameWhenSet = true;
      this.srchNamedSearch.ShowTermedCheckBox = false;
      this.srchNamedSearch.Size = new System.Drawing.Size(199, 19);
      this.srchNamedSearch.TabIndex = 8;
      this.srchNamedSearch.Visible = false;
      // 
      // btnClear
      // 
      this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClear.Location = new System.Drawing.Point(1286, 1);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new System.Drawing.Size(45, 20);
      this.btnClear.TabIndex = 7;
      this.btnClear.Text = " Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(333, 3);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(122, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Grid Top/Middle/Bottom";
      // 
      // btnGroups
      // 
      this.btnGroups.Location = new System.Drawing.Point(238, 1);
      this.btnGroups.Name = "btnGroups";
      this.btnGroups.Size = new System.Drawing.Size(89, 20);
      this.btnGroups.TabIndex = 2;
      this.btnGroups.Text = "Create Group";
      this.btnGroups.UseVisualStyleBackColor = true;
      this.btnGroups.Click += new System.EventHandler(this.btnGroups_Click);
      // 
      // ckFilterByIDList
      // 
      this.ckFilterByIDList.AutoSize = true;
      this.ckFilterByIDList.Checked = true;
      this.ckFilterByIDList.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckFilterByIDList.Location = new System.Drawing.Point(10, 2);
      this.ckFilterByIDList.Name = "ckFilterByIDList";
      this.ckFilterByIDList.Size = new System.Drawing.Size(223, 17);
      this.ckFilterByIDList.TabIndex = 1;
      this.ckFilterByIDList.Text = "Filter bottom grids by upper grid customers";
      this.ckFilterByIDList.UseVisualStyleBackColor = true;
      this.ckFilterByIDList.CheckedChanged += new System.EventHandler(this.ckFilterByIDList_CheckedChanged);
      // 
      // splitMain
      // 
      this.splitMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.splitMain.Location = new System.Drawing.Point(2, 27);
      this.splitMain.Name = "splitMain";
      this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitMain.Panel1
      // 
      this.splitMain.Panel1.Controls.Add(this.splitTop);
      // 
      // splitMain.Panel2
      // 
      this.splitMain.Panel2.Controls.Add(this.bottomGrid);
      this.splitMain.Panel2Collapsed = true;
      this.splitMain.Size = new System.Drawing.Size(1331, 706);
      this.splitMain.SplitterDistance = 443;
      this.splitMain.TabIndex = 0;
      // 
      // splitTop
      // 
      this.splitTop.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitTop.Location = new System.Drawing.Point(0, 0);
      this.splitTop.Name = "splitTop";
      this.splitTop.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitTop.Panel1
      // 
      this.splitTop.Panel1.Controls.Add(this.topGrid);
      // 
      // splitTop.Panel2
      // 
      this.splitTop.Panel2.Controls.Add(this.middleGrid);
      this.splitTop.Panel2Collapsed = true;
      this.splitTop.Size = new System.Drawing.Size(1331, 706);
      this.splitTop.SplitterDistance = 431;
      this.splitTop.TabIndex = 0;
      // 
      // topGrid
      // 
      this.topGrid.AllowSortByColumn = true;
      this.topGrid.AutoRefreshWhenFieldChecked = false;
      this.topGrid.AutoSaveUserOptions = false;
      this.topGrid.CanChangeDisplayFields = true;
      this.topGrid.CanChangeDisplaySearchCriteria = true;
      this.topGrid.ColumnName = "CustomerName";
      this.topGrid.DisplayFields = false;
      this.topGrid.DisplaySearchCriteria = true;
      this.topGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.topGrid.FieldsDefaultIsChecked = true;
      this.topGrid.ForceReloadSearchColumns = false;
      this.topGrid.IDList = null;
      this.topGrid.IncludeGroupAsCriteria = false;
      this.topGrid.InnerWhere = "";
      this.topGrid.Location = new System.Drawing.Point(0, 0);
      this.topGrid.Name = "topGrid";
      this.topGrid.NameType = CCI.Common.CommonData.UnmatchedNameTypes.None;
      this.topGrid.SearchCriteria = null;
      this.topGrid.Size = new System.Drawing.Size(1331, 706);
      this.topGrid.TabIndex = 0;
      this.topGrid.Title = "Search (0 Records Found)";
      this.topGrid.UniqueIdentifier = "ID";
      this.topGrid.UseNamedSearches = false;
      this.topGrid.SearchPressed += new ACG.CommonForms.ctlSearchGrid.SearchPressedHandler(this.topGrid_SearchPressed);
      // 
      // middleGrid
      // 
      this.middleGrid.AllowSortByColumn = true;
      this.middleGrid.AutoRefreshWhenFieldChecked = false;
      this.middleGrid.AutoSaveUserOptions = false;
      this.middleGrid.CanChangeDisplayFields = true;
      this.middleGrid.CanChangeDisplaySearchCriteria = true;
      this.middleGrid.ColumnName = "CustomerName";
      this.middleGrid.DisplayFields = false;
      this.middleGrid.DisplaySearchCriteria = true;
      this.middleGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.middleGrid.FieldsDefaultIsChecked = true;
      this.middleGrid.ForceReloadSearchColumns = false;
      this.middleGrid.IDList = null;
      this.middleGrid.IncludeGroupAsCriteria = false;
      this.middleGrid.InnerWhere = "";
      this.middleGrid.Location = new System.Drawing.Point(0, 0);
      this.middleGrid.Name = "middleGrid";
      this.middleGrid.NameType = CCI.Common.CommonData.UnmatchedNameTypes.None;
      this.middleGrid.SearchCriteria = null;
      this.middleGrid.Size = new System.Drawing.Size(150, 46);
      this.middleGrid.TabIndex = 0;
      this.middleGrid.Title = "Search (0 Records Found)";
      this.middleGrid.UniqueIdentifier = "ID";
      this.middleGrid.UseNamedSearches = false;
      this.middleGrid.SearchPressed += new ACG.CommonForms.ctlSearchGrid.SearchPressedHandler(this.middleGrid_SearchPressed);
      // 
      // bottomGrid
      // 
      this.bottomGrid.AllowSortByColumn = true;
      this.bottomGrid.AutoRefreshWhenFieldChecked = false;
      this.bottomGrid.AutoSaveUserOptions = false;
      this.bottomGrid.CanChangeDisplayFields = true;
      this.bottomGrid.CanChangeDisplaySearchCriteria = true;
      this.bottomGrid.ColumnName = "CustomerName";
      this.bottomGrid.DisplayFields = false;
      this.bottomGrid.DisplaySearchCriteria = true;
      this.bottomGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.bottomGrid.FieldsDefaultIsChecked = true;
      this.bottomGrid.ForceReloadSearchColumns = false;
      this.bottomGrid.IDList = null;
      this.bottomGrid.IncludeGroupAsCriteria = false;
      this.bottomGrid.InnerWhere = "";
      this.bottomGrid.Location = new System.Drawing.Point(0, 0);
      this.bottomGrid.Name = "bottomGrid";
      this.bottomGrid.NameType = CCI.Common.CommonData.UnmatchedNameTypes.None;
      this.bottomGrid.SearchCriteria = null;
      this.bottomGrid.Size = new System.Drawing.Size(150, 46);
      this.bottomGrid.TabIndex = 0;
      this.bottomGrid.Title = "Search (0 Records Found)";
      this.bottomGrid.UniqueIdentifier = "ID";
      this.bottomGrid.UseNamedSearches = false;
      // 
      // frmAnalysisReport
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1331, 733);
      this.Controls.Add(this.srchBottomGrid);
      this.Controls.Add(this.srchMiddleGrid);
      this.Controls.Add(this.srchTopGrid);
      this.Controls.Add(this.srchNamedSearch);
      this.Controls.Add(this.btnClear);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.btnGroups);
      this.Controls.Add(this.ckFilterByIDList);
      this.Controls.Add(this.splitMain);
      this.Name = "frmAnalysisReport";
      this.Text = "frmAnalysisReport";
      this.Load += new System.EventHandler(this.frmAnalysisReport_Load);
      this.splitMain.Panel1.ResumeLayout(false);
      this.splitMain.Panel2.ResumeLayout(false);
      this.splitMain.ResumeLayout(false);
      this.splitTop.Panel1.ResumeLayout(false);
      this.splitTop.Panel2.ResumeLayout(false);
      this.splitTop.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitMain;
    private System.Windows.Forms.SplitContainer splitTop;
    private System.Windows.Forms.CheckBox ckFilterByIDList;
    private System.Windows.Forms.Button btnGroups;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnClear;
    private ACG.CommonForms.ctlSearch srchNamedSearch;
    private ACG.CommonForms.ctlSearch srchTopGrid;
    private ACG.CommonForms.ctlSearch srchMiddleGrid;
    private ACG.CommonForms.ctlSearch srchBottomGrid;
    private Common.ctlSearchGrid topGrid;
    private Common.ctlSearchGrid middleGrid;
    private Common.ctlSearchGrid bottomGrid;
  }
}