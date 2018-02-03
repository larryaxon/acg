namespace ACG.CommonForms
{
  partial class ctlSearchGrid
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctlSearchGrid));
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      this.splitMain = new System.Windows.Forms.SplitContainer();
      this.splitSelect = new System.Windows.Forms.SplitContainer();
      this.srchNamedSearch = new ACG.CommonForms.ctlSearch();
      this.toolStripSearch = new System.Windows.Forms.ToolStrip();
      this.tsbtnSearch = new System.Windows.Forms.ToolStripButton();
      this.tsbtnExport = new System.Windows.Forms.ToolStripButton();
      this.tsbtnClear = new System.Windows.Forms.ToolStripButton();
      this.tslblSavedSearchLabel = new System.Windows.Forms.ToolStripLabel();
      this.tslblCurrentSavedSearch = new System.Windows.Forms.ToolStripLabel();
      this.tsbthNewSavedSearch = new System.Windows.Forms.ToolStripButton();
      this.tsbtnDeleteSavedSearch = new System.Windows.Forms.ToolStripButton();
      this.tsbtnSaveOptions = new System.Windows.Forms.ToolStripButton();
      this.grdSearch = new System.Windows.Forms.DataGridView();
      this.lblMRCTotal = new System.Windows.Forms.Label();
      this.lblTitle = new System.Windows.Forms.Label();
      this.btnSelectAllFields = new System.Windows.Forms.Button();
      this.btnClearFields = new System.Windows.Forms.Button();
      this.ckAutoRefreshFields = new System.Windows.Forms.CheckBox();
      this.btnRefresh = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.lstFields = new System.Windows.Forms.CheckedListBox();
      this.grdResearch = new System.Windows.Forms.DataGridView();
      this.mnuGridContext = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.copyCellToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.btnCollapseSearch = new System.Windows.Forms.Button();
      this.btnShowFields = new System.Windows.Forms.Button();
      this.splitMain.Panel1.SuspendLayout();
      this.splitMain.Panel2.SuspendLayout();
      this.splitMain.SuspendLayout();
      this.splitSelect.Panel1.SuspendLayout();
      this.splitSelect.Panel2.SuspendLayout();
      this.splitSelect.SuspendLayout();
      this.toolStripSearch.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.grdResearch)).BeginInit();
      this.mnuGridContext.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitMain
      // 
      this.splitMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
      this.splitMain.Location = new System.Drawing.Point(18, 0);
      this.splitMain.Name = "splitMain";
      // 
      // splitMain.Panel1
      // 
      this.splitMain.Panel1.AutoScroll = true;
      this.splitMain.Panel1.Controls.Add(this.splitSelect);
      // 
      // splitMain.Panel2
      // 
      this.splitMain.Panel2.Controls.Add(this.grdResearch);
      this.splitMain.Size = new System.Drawing.Size(1089, 448);
      this.splitMain.SplitterDistance = 452;
      this.splitMain.TabIndex = 0;
      // 
      // splitSelect
      // 
      this.splitSelect.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitSelect.Location = new System.Drawing.Point(0, 0);
      this.splitSelect.Name = "splitSelect";
      // 
      // splitSelect.Panel1
      // 
      this.splitSelect.Panel1.Controls.Add(this.srchNamedSearch);
      this.splitSelect.Panel1.Controls.Add(this.toolStripSearch);
      this.splitSelect.Panel1.Controls.Add(this.grdSearch);
      this.splitSelect.Panel1.Controls.Add(this.lblMRCTotal);
      this.splitSelect.Panel1.Controls.Add(this.lblTitle);
      // 
      // splitSelect.Panel2
      // 
      this.splitSelect.Panel2.Controls.Add(this.btnSelectAllFields);
      this.splitSelect.Panel2.Controls.Add(this.btnClearFields);
      this.splitSelect.Panel2.Controls.Add(this.ckAutoRefreshFields);
      this.splitSelect.Panel2.Controls.Add(this.btnRefresh);
      this.splitSelect.Panel2.Controls.Add(this.label1);
      this.splitSelect.Panel2.Controls.Add(this.lstFields);
      this.splitSelect.Size = new System.Drawing.Size(452, 448);
      this.splitSelect.SplitterDistance = 265;
      this.splitSelect.TabIndex = 11;
      // 
      // srchNamedSearch
      // 
      this.srchNamedSearch.AddNewMode = false;
      this.srchNamedSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.srchNamedSearch.AutoAddNewMode = false;
      this.srchNamedSearch.AutoSelectWhenMatch = false;
      this.srchNamedSearch.AutoTabToNextControlOnSelect = false;
      this.srchNamedSearch.ClearSearchOnExpand = true;
      this.srchNamedSearch.ClearSearchWhenComplete = true;
      this.srchNamedSearch.Collapsed = true;
      this.srchNamedSearch.CreatedNewItem = false;
      this.srchNamedSearch.DisplayOnlyDescription = false;
      this.srchNamedSearch.DisplayOnlyID = true;
      this.srchNamedSearch.FixKeySpace = "-1";
      this.srchNamedSearch.ID = "";
      this.srchNamedSearch.ID_DescSplitter = ":";
      this.srchNamedSearch.Location = new System.Drawing.Point(6, 22);
      this.srchNamedSearch.MaxHeight = 228;
      this.srchNamedSearch.MustExistInList = false;
      this.srchNamedSearch.MustExistMessage = "You must enter a valid value";
      this.srchNamedSearch.Name = "srchNamedSearch";
      this.srchNamedSearch.SearchExec = null;
      this.srchNamedSearch.ShowCustomerNameWhenSet = true;
      this.srchNamedSearch.ShowTermedCheckBox = false;
      this.srchNamedSearch.Size = new System.Drawing.Size(256, 22);
      this.srchNamedSearch.TabIndex = 0;
      this.srchNamedSearch.OnSelected += new System.EventHandler<System.EventArgs>(this.srchNamedSearch_OnSelected);
      // 
      // toolStripSearch
      // 
      this.toolStripSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.toolStripSearch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnSearch,
            this.tsbtnExport,
            this.tsbtnClear,
            this.tslblSavedSearchLabel,
            this.tslblCurrentSavedSearch,
            this.tsbthNewSavedSearch,
            this.tsbtnDeleteSavedSearch,
            this.tsbtnSaveOptions});
      this.toolStripSearch.Location = new System.Drawing.Point(0, 423);
      this.toolStripSearch.Name = "toolStripSearch";
      this.toolStripSearch.Size = new System.Drawing.Size(265, 25);
      this.toolStripSearch.TabIndex = 2;
      this.toolStripSearch.Text = "toolStrip1";
      // 
      // tsbtnSearch
      // 
      this.tsbtnSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.tsbtnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsbtnSearch.Name = "tsbtnSearch";
      this.tsbtnSearch.Size = new System.Drawing.Size(44, 22);
      this.tsbtnSearch.Text = "Search";
      this.tsbtnSearch.Click += new System.EventHandler(this.btnSearch_Click);
      // 
      // tsbtnExport
      // 
      this.tsbtnExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.tsbtnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsbtnExport.Name = "tsbtnExport";
      this.tsbtnExport.Size = new System.Drawing.Size(43, 22);
      this.tsbtnExport.Text = "Export";
      this.tsbtnExport.Click += new System.EventHandler(this.btnExport_Click);
      // 
      // tsbtnClear
      // 
      this.tsbtnClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.tsbtnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsbtnClear.Name = "tsbtnClear";
      this.tsbtnClear.Size = new System.Drawing.Size(36, 22);
      this.tsbtnClear.Text = "Clear";
      this.tsbtnClear.Click += new System.EventHandler(this.btnClear_Click);
      // 
      // tslblSavedSearchLabel
      // 
      this.tslblSavedSearchLabel.Name = "tslblSavedSearchLabel";
      this.tslblSavedSearchLabel.Size = new System.Drawing.Size(77, 22);
      this.tslblSavedSearchLabel.Text = "Saved Search:";
      // 
      // tslblCurrentSavedSearch
      // 
      this.tslblCurrentSavedSearch.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
      this.tslblCurrentSavedSearch.Name = "tslblCurrentSavedSearch";
      this.tslblCurrentSavedSearch.Size = new System.Drawing.Size(0, 22);
      // 
      // tsbthNewSavedSearch
      // 
      this.tsbthNewSavedSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.tsbthNewSavedSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsbthNewSavedSearch.Name = "tsbthNewSavedSearch";
      this.tsbthNewSavedSearch.Size = new System.Drawing.Size(32, 22);
      this.tsbthNewSavedSearch.Text = "New";
      this.tsbthNewSavedSearch.Click += new System.EventHandler(this.tsbthNewSavedSearch_Click);
      // 
      // tsbtnDeleteSavedSearch
      // 
      this.tsbtnDeleteSavedSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.tsbtnDeleteSavedSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.tsbtnDeleteSavedSearch.Name = "tsbtnDeleteSavedSearch";
      this.tsbtnDeleteSavedSearch.Size = new System.Drawing.Size(23, 17);
      this.tsbtnDeleteSavedSearch.Text = "X";
      this.tsbtnDeleteSavedSearch.Click += new System.EventHandler(this.btnDeleteSavedSearch_Click);
      // 
      // tsbtnSaveOptions
      // 
      this.tsbtnSaveOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.tsbtnSaveOptions.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSaveOptions.Image")));
      this.tsbtnSaveOptions.ImageTransparentColor = System.Drawing.Color.Black;
      this.tsbtnSaveOptions.Name = "tsbtnSaveOptions";
      this.tsbtnSaveOptions.Size = new System.Drawing.Size(23, 20);
      this.tsbtnSaveOptions.Text = "Save";
      this.tsbtnSaveOptions.Click += new System.EventHandler(this.tsbtnSaveOptions_Click);
      // 
      // grdSearch
      // 
      this.grdSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grdSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdSearch.Location = new System.Drawing.Point(6, 50);
      this.grdSearch.Name = "grdSearch";
      this.grdSearch.Size = new System.Drawing.Size(259, 370);
      this.grdSearch.TabIndex = 1;
      this.grdSearch.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdSearch_CellValueChanged);
      this.grdSearch.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.grdSearch_DefaultValuesNeeded);
      this.grdSearch.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.grdSearch_RowsAdded);
      this.grdSearch.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.grdSearch_RowsRemoved);
      this.grdSearch.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.grdSearch_RowValidating);
      this.grdSearch.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.grdSearch_UserDeletedRow);
      // 
      // lblMRCTotal
      // 
      this.lblMRCTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblMRCTotal.AutoSize = true;
      this.lblMRCTotal.Location = new System.Drawing.Point(83, 10);
      this.lblMRCTotal.Name = "lblMRCTotal";
      this.lblMRCTotal.Size = new System.Drawing.Size(58, 13);
      this.lblMRCTotal.TabIndex = 9;
      this.lblMRCTotal.Text = "Total MRC";
      this.lblMRCTotal.Visible = false;
      // 
      // lblTitle
      // 
      this.lblTitle.AutoSize = true;
      this.lblTitle.Location = new System.Drawing.Point(3, 10);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new System.Drawing.Size(132, 13);
      this.lblTitle.TabIndex = 7;
      this.lblTitle.Text = "Search (0 Records Found)";
      // 
      // btnSelectAllFields
      // 
      this.btnSelectAllFields.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSelectAllFields.Location = new System.Drawing.Point(6, 422);
      this.btnSelectAllFields.Name = "btnSelectAllFields";
      this.btnSelectAllFields.Size = new System.Drawing.Size(54, 20);
      this.btnSelectAllFields.TabIndex = 2;
      this.btnSelectAllFields.Text = "All";
      this.btnSelectAllFields.UseVisualStyleBackColor = true;
      this.btnSelectAllFields.Click += new System.EventHandler(this.btnSelectAllFields_Click);
      // 
      // btnClearFields
      // 
      this.btnClearFields.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClearFields.Location = new System.Drawing.Point(66, 422);
      this.btnClearFields.Name = "btnClearFields";
      this.btnClearFields.Size = new System.Drawing.Size(54, 20);
      this.btnClearFields.TabIndex = 3;
      this.btnClearFields.Text = "Clear";
      this.btnClearFields.UseVisualStyleBackColor = true;
      this.btnClearFields.Click += new System.EventHandler(this.btnClearFields_Click);
      // 
      // ckAutoRefreshFields
      // 
      this.ckAutoRefreshFields.AutoSize = true;
      this.ckAutoRefreshFields.Location = new System.Drawing.Point(129, 6);
      this.ckAutoRefreshFields.Name = "ckAutoRefreshFields";
      this.ckAutoRefreshFields.Size = new System.Drawing.Size(88, 17);
      this.ckAutoRefreshFields.TabIndex = 0;
      this.ckAutoRefreshFields.Text = "Auto Refresh";
      this.ckAutoRefreshFields.UseVisualStyleBackColor = true;
      this.ckAutoRefreshFields.CheckedChanged += new System.EventHandler(this.ckAutoRefreshFields_CheckedChanged);
      // 
      // btnRefresh
      // 
      this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnRefresh.Location = new System.Drawing.Point(126, 422);
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new System.Drawing.Size(54, 20);
      this.btnRefresh.TabIndex = 4;
      this.btnRefresh.Text = "Refresh";
      this.btnRefresh.UseVisualStyleBackColor = true;
      this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(7, 7);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(116, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Select Fields to Display";
      // 
      // lstFields
      // 
      this.lstFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstFields.CheckOnClick = true;
      this.lstFields.FormattingEnabled = true;
      this.lstFields.Location = new System.Drawing.Point(4, 26);
      this.lstFields.Name = "lstFields";
      this.lstFields.ScrollAlwaysVisible = true;
      this.lstFields.Size = new System.Drawing.Size(176, 394);
      this.lstFields.Sorted = true;
      this.lstFields.TabIndex = 1;
      this.lstFields.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstFields_ItemCheck);
      // 
      // grdResearch
      // 
      this.grdResearch.AllowUserToAddRows = false;
      this.grdResearch.AllowUserToDeleteRows = false;
      this.grdResearch.AllowUserToOrderColumns = true;
      dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
      this.grdResearch.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(133)))), ((int)(((byte)(138)))));
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.Info;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.grdResearch.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
      this.grdResearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdResearch.Dock = System.Windows.Forms.DockStyle.Fill;
      this.grdResearch.EnableHeadersVisualStyles = false;
      this.grdResearch.Location = new System.Drawing.Point(0, 0);
      this.grdResearch.MultiSelect = false;
      this.grdResearch.Name = "grdResearch";
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLightLight;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.grdResearch.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.grdResearch.Size = new System.Drawing.Size(633, 448);
      this.grdResearch.TabIndex = 7;
      this.grdResearch.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.grdResearch_CellBeginEdit);
      this.grdResearch.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdResearch_CellValidated);
      this.grdResearch.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.grdResearch_ColumnAdded);
      this.grdResearch.ColumnDisplayIndexChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.grdResearch_ColumnDisplayIndexChanged);
      this.grdResearch.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdResearch_ColumnHeaderMouseClick);
      this.grdResearch.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdResearch_RowEnter);
      this.grdResearch.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdResearch_RowHeaderMouseClick);
      this.grdResearch.SelectionChanged += new System.EventHandler(this.grdResearch_SelectionChanged);
      this.grdResearch.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.grdResearch_SortCompare);
      // 
      // mnuGridContext
      // 
      this.mnuGridContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.copyCellToolStripMenuItem});
      this.mnuGridContext.Name = "contextMenuStrip1";
      this.mnuGridContext.Size = new System.Drawing.Size(120, 48);
      // 
      // exportToolStripMenuItem
      // 
      this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
      this.exportToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
      this.exportToolStripMenuItem.Text = "Export";
      this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
      // 
      // copyCellToolStripMenuItem
      // 
      this.copyCellToolStripMenuItem.Name = "copyCellToolStripMenuItem";
      this.copyCellToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
      this.copyCellToolStripMenuItem.Text = "Copy Cell";
      this.copyCellToolStripMenuItem.Click += new System.EventHandler(this.copyCellToolStripMenuItem_Click);
      // 
      // btnCollapseSearch
      // 
      this.btnCollapseSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.btnCollapseSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(133)))), ((int)(((byte)(138)))));
      this.btnCollapseSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnCollapseSearch.ForeColor = System.Drawing.SystemColors.ControlLightLight;
      this.btnCollapseSearch.Location = new System.Drawing.Point(1, 0);
      this.btnCollapseSearch.Name = "btnCollapseSearch";
      this.btnCollapseSearch.Size = new System.Drawing.Size(18, 341);
      this.btnCollapseSearch.TabIndex = 0;
      this.btnCollapseSearch.Text = "<";
      this.btnCollapseSearch.UseVisualStyleBackColor = false;
      this.btnCollapseSearch.Click += new System.EventHandler(this.btnCollapseSearch_Click);
      // 
      // btnShowFields
      // 
      this.btnShowFields.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnShowFields.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(133)))), ((int)(((byte)(138)))));
      this.btnShowFields.ForeColor = System.Drawing.SystemColors.ControlLightLight;
      this.btnShowFields.Location = new System.Drawing.Point(0, 349);
      this.btnShowFields.Name = "btnShowFields";
      this.btnShowFields.Size = new System.Drawing.Size(18, 95);
      this.btnShowFields.TabIndex = 1;
      this.btnShowFields.Text = "Fields";
      this.btnShowFields.UseVisualStyleBackColor = false;
      this.btnShowFields.Click += new System.EventHandler(this.btnShowFields_Click);
      // 
      // ctlSearchGrid
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.Controls.Add(this.btnShowFields);
      this.Controls.Add(this.btnCollapseSearch);
      this.Controls.Add(this.splitMain);
      this.Name = "ctlSearchGrid";
      this.Size = new System.Drawing.Size(1107, 448);
      this.Resize += new System.EventHandler(this.ctlSearchGrid_Resize);
      this.splitMain.Panel1.ResumeLayout(false);
      this.splitMain.Panel2.ResumeLayout(false);
      this.splitMain.ResumeLayout(false);
      this.splitSelect.Panel1.ResumeLayout(false);
      this.splitSelect.Panel1.PerformLayout();
      this.splitSelect.Panel2.ResumeLayout(false);
      this.splitSelect.Panel2.PerformLayout();
      this.splitSelect.ResumeLayout(false);
      this.toolStripSearch.ResumeLayout(false);
      this.toolStripSearch.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.grdResearch)).EndInit();
      this.mnuGridContext.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitMain;
    protected System.Windows.Forms.DataGridView grdResearch;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblMRCTotal;
    private System.Windows.Forms.ContextMenuStrip mnuGridContext;
    private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyCellToolStripMenuItem;
    private System.Windows.Forms.Button btnCollapseSearch;
    private System.Windows.Forms.SplitContainer splitSelect;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckedListBox lstFields;
    private System.Windows.Forms.Button btnShowFields;
    private System.Windows.Forms.CheckBox ckAutoRefreshFields;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnSelectAllFields;
    private System.Windows.Forms.Button btnClearFields;
    private System.Windows.Forms.ToolStrip toolStripSearch;
    private System.Windows.Forms.ToolStripButton tsbtnSaveOptions;
    private System.Windows.Forms.ToolStripButton tsbtnSearch;
    private System.Windows.Forms.ToolStripButton tsbtnExport;
    private System.Windows.Forms.ToolStripButton tsbtnClear;
    private System.Windows.Forms.DataGridView grdSearch;
    private ACG.CommonForms.ctlSearch srchNamedSearch;
    private System.Windows.Forms.ToolStripLabel tslblCurrentSavedSearch;
    private System.Windows.Forms.ToolStripLabel tslblSavedSearchLabel;
    private System.Windows.Forms.ToolStripButton tsbthNewSavedSearch;
    private System.Windows.Forms.ToolStripButton tsbtnDeleteSavedSearch;
  }
}
