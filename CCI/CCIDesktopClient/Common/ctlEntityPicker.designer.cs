namespace CCI.DesktopClient.Common
{
  partial class ctlEntityPicker
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
      this.txtEntityList = new System.Windows.Forms.TextBox();
      this.tabView = new System.Windows.Forms.TabControl();
      this.tabSearch = new System.Windows.Forms.TabPage();
      this.cmdSearch = new System.Windows.Forms.Button();
      this.cboEntityType = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.lstSearchResults = new System.Windows.Forms.ListBox();
      this.txtSearch = new System.Windows.Forms.TextBox();
      this.tabTree = new System.Windows.Forms.TabPage();
      this.cmdExpandTree = new System.Windows.Forms.Button();
      this.tvwEntityHierarchy = new System.Windows.Forms.TreeView();
      this.tabSelected = new System.Windows.Forms.TabPage();
      this.cmdRemove = new System.Windows.Forms.Button();
      this.lstSelected = new System.Windows.Forms.ListBox();
      this.ckIncludeTerms = new System.Windows.Forms.CheckBox();
      this.ckSelectAll = new System.Windows.Forms.CheckBox();
      this.pictMode = new System.Windows.Forms.PictureBox();
      this.cmdExpand = new System.Windows.Forms.Button();
      this.tabView.SuspendLayout();
      this.tabSearch.SuspendLayout();
      this.tabTree.SuspendLayout();
      this.tabSelected.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictMode)).BeginInit();
      this.SuspendLayout();
      // 
      // txtEntityList
      // 
      this.txtEntityList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.txtEntityList.Location = new System.Drawing.Point(0, 0);
      this.txtEntityList.Name = "txtEntityList";
      this.txtEntityList.Size = new System.Drawing.Size(202, 20);
      this.txtEntityList.TabIndex = 1;
      this.txtEntityList.Leave += new System.EventHandler(this.txtEntityList_Leave);
      this.txtEntityList.Enter += new System.EventHandler(this.txtEntityList_Enter);
      // 
      // tabView
      // 
      this.tabView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tabView.Controls.Add(this.tabSearch);
      this.tabView.Controls.Add(this.tabTree);
      this.tabView.Controls.Add(this.tabSelected);
      this.tabView.Location = new System.Drawing.Point(3, 24);
      this.tabView.Name = "tabView";
      this.tabView.SelectedIndex = 0;
      this.tabView.Size = new System.Drawing.Size(324, 0);
      this.tabView.TabIndex = 4;
      this.tabView.Visible = false;
      this.tabView.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabView_Selected);
      // 
      // tabSearch
      // 
      this.tabSearch.Controls.Add(this.cmdSearch);
      this.tabSearch.Controls.Add(this.cboEntityType);
      this.tabSearch.Controls.Add(this.label1);
      this.tabSearch.Controls.Add(this.lstSearchResults);
      this.tabSearch.Controls.Add(this.txtSearch);
      this.tabSearch.Location = new System.Drawing.Point(4, 22);
      this.tabSearch.Name = "tabSearch";
      this.tabSearch.Padding = new System.Windows.Forms.Padding(3);
      this.tabSearch.Size = new System.Drawing.Size(316, 0);
      this.tabSearch.TabIndex = 1;
      this.tabSearch.Text = "Search View";
      this.tabSearch.UseVisualStyleBackColor = true;
      // 
      // cmdSearch
      // 
      this.cmdSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      //this.cmdSearch.Image = global::CCI.DesktopClient.Common.Properties.Resources.searchDocuments;
      this.cmdSearch.Location = new System.Drawing.Point(294, 0);
      this.cmdSearch.Name = "cmdSearch";
      this.cmdSearch.Size = new System.Drawing.Size(19, 20);
      this.cmdSearch.TabIndex = 23;
      this.cmdSearch.Text = "V";
      this.cmdSearch.UseVisualStyleBackColor = true;
      this.cmdSearch.Click += new System.EventHandler(this.cmdSearch_Click);
      // 
      // cboEntityType
      // 
      this.cboEntityType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.cboEntityType.FormattingEnabled = true;
      this.cboEntityType.Location = new System.Drawing.Point(42, 22);
      this.cboEntityType.Name = "cboEntityType";
      this.cboEntityType.Size = new System.Drawing.Size(145, 21);
      this.cboEntityType.TabIndex = 1;
      this.cboEntityType.SelectedValueChanged += new System.EventHandler(this.cboEntityType_SelectedValueChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(4, 26);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(31, 13);
      this.label1.TabIndex = 22;
      this.label1.Text = "Type";
      // 
      // lstSearchResults
      // 
      this.lstSearchResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.lstSearchResults.FormattingEnabled = true;
      this.lstSearchResults.Location = new System.Drawing.Point(3, 50);
      this.lstSearchResults.Name = "lstSearchResults";
      this.lstSearchResults.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.lstSearchResults.Size = new System.Drawing.Size(307, 4);
      this.lstSearchResults.TabIndex = 2;
      this.lstSearchResults.SelectedIndexChanged += new System.EventHandler(this.lstSearchResults_SelectedIndexChanged);
      // 
      // txtSearch
      // 
      this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSearch.Location = new System.Drawing.Point(1, 0);
      this.txtSearch.Name = "txtSearch";
      this.txtSearch.Size = new System.Drawing.Size(290, 20);
      this.txtSearch.TabIndex = 0;
      this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
      // 
      // tabTree
      // 
      this.tabTree.Controls.Add(this.cmdExpandTree);
      this.tabTree.Controls.Add(this.tvwEntityHierarchy);
      this.tabTree.Location = new System.Drawing.Point(4, 22);
      this.tabTree.Name = "tabTree";
      this.tabTree.Padding = new System.Windows.Forms.Padding(3);
      this.tabTree.Size = new System.Drawing.Size(316, 0);
      this.tabTree.TabIndex = 0;
      this.tabTree.Text = "Tree View";
      this.tabTree.UseVisualStyleBackColor = true;
      // 
      // cmdExpandTree
      // 
      this.cmdExpandTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdExpandTree.Location = new System.Drawing.Point(60, 382);
      this.cmdExpandTree.Name = "cmdExpandTree";
      this.cmdExpandTree.Size = new System.Drawing.Size(173, 23);
      this.cmdExpandTree.TabIndex = 16;
      this.cmdExpandTree.Text = "Expand";
      this.cmdExpandTree.UseVisualStyleBackColor = true;
      this.cmdExpandTree.Click += new System.EventHandler(this.cmdExpandTree_Click);
      // 
      // tvwEntityHierarchy
      // 
      this.tvwEntityHierarchy.CheckBoxes = true;
      this.tvwEntityHierarchy.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tvwEntityHierarchy.Location = new System.Drawing.Point(3, 3);
      this.tvwEntityHierarchy.Name = "tvwEntityHierarchy";
      this.tvwEntityHierarchy.Size = new System.Drawing.Size(310, 0);
      this.tvwEntityHierarchy.TabIndex = 15;
      this.tvwEntityHierarchy.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvwEntityHierarchy_AfterCheck);
      // 
      // tabSelected
      // 
      this.tabSelected.Controls.Add(this.cmdRemove);
      this.tabSelected.Controls.Add(this.lstSelected);
      this.tabSelected.Location = new System.Drawing.Point(4, 22);
      this.tabSelected.Name = "tabSelected";
      this.tabSelected.Size = new System.Drawing.Size(316, 0);
      this.tabSelected.TabIndex = 2;
      this.tabSelected.Text = "Selected";
      this.tabSelected.UseVisualStyleBackColor = true;
      // 
      // cmdRemove
      // 
      this.cmdRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdRemove.Location = new System.Drawing.Point(65, 385);
      this.cmdRemove.Name = "cmdRemove";
      this.cmdRemove.Size = new System.Drawing.Size(175, 23);
      this.cmdRemove.TabIndex = 1;
      this.cmdRemove.Text = "Remove Highlighted";
      this.cmdRemove.UseVisualStyleBackColor = true;
      this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
      // 
      // lstSelected
      // 
      this.lstSelected.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lstSelected.FormattingEnabled = true;
      this.lstSelected.Location = new System.Drawing.Point(0, 0);
      this.lstSelected.Name = "lstSelected";
      this.lstSelected.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
      this.lstSelected.Size = new System.Drawing.Size(316, 4);
      this.lstSelected.TabIndex = 0;
      // 
      // ckIncludeTerms
      // 
      this.ckIncludeTerms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ckIncludeTerms.AutoSize = true;
      this.ckIncludeTerms.Location = new System.Drawing.Point(227, 2);
      this.ckIncludeTerms.Margin = new System.Windows.Forms.Padding(1);
      this.ckIncludeTerms.Name = "ckIncludeTerms";
      this.ckIncludeTerms.Size = new System.Drawing.Size(50, 17);
      this.ckIncludeTerms.TabIndex = 2;
      this.ckIncludeTerms.TabStop = false;
      this.ckIncludeTerms.Text = "Term";
      this.ckIncludeTerms.UseVisualStyleBackColor = true;
      this.ckIncludeTerms.CheckedChanged += new System.EventHandler(this.ckIncludeTerms_CheckedChanged);
      // 
      // ckSelectAll
      // 
      this.ckSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ckSelectAll.AutoSize = true;
      this.ckSelectAll.Location = new System.Drawing.Point(275, 2);
      this.ckSelectAll.Margin = new System.Windows.Forms.Padding(1);
      this.ckSelectAll.Name = "ckSelectAll";
      this.ckSelectAll.Size = new System.Drawing.Size(37, 17);
      this.ckSelectAll.TabIndex = 3;
      this.ckSelectAll.TabStop = false;
      this.ckSelectAll.Text = "All";
      this.ckSelectAll.UseVisualStyleBackColor = true;
      this.ckSelectAll.CheckedChanged += new System.EventHandler(this.ckSelectAll_CheckedChanged);
      // 
      // pictMode
      // 
      this.pictMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      //this.pictMode.Image = global::CCI.DesktopClient.Common.Properties.Resources.Expand_large;
      this.pictMode.Location = new System.Drawing.Point(205, 2);
      this.pictMode.Name = "pictMode";
      this.pictMode.Size = new System.Drawing.Size(13, 13);
      this.pictMode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.pictMode.TabIndex = 21;
      this.pictMode.TabStop = false;
      // 
      // cmdExpand
      // 
      this.cmdExpand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      //this.cmdExpand.Image = global::CCI.DesktopClient.Common.Properties.Resources.Expand_large;
      this.cmdExpand.Location = new System.Drawing.Point(311, 0);
      this.cmdExpand.Name = "cmdExpand";
      this.cmdExpand.Size = new System.Drawing.Size(19, 20);
      this.cmdExpand.TabIndex = 0;
      this.cmdExpand.TabStop = false;
      this.cmdExpand.Text = "V";
      this.cmdExpand.UseVisualStyleBackColor = true;
      this.cmdExpand.Click += new System.EventHandler(this.cmdExpand_Click);
      // 
      // ctlEntityPicker
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add(this.ckSelectAll);
      this.Controls.Add(this.pictMode);
      this.Controls.Add(this.ckIncludeTerms);
      this.Controls.Add(this.tabView);
      this.Controls.Add(this.cmdExpand);
      this.Controls.Add(this.txtEntityList);
      this.Name = "ctlEntityPicker";
      this.Size = new System.Drawing.Size(329, 20);
      this.tabView.ResumeLayout(false);
      this.tabSearch.ResumeLayout(false);
      this.tabSearch.PerformLayout();
      this.tabTree.ResumeLayout(false);
      this.tabSelected.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictMode)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox txtEntityList;
    private System.Windows.Forms.Button cmdExpand;
    private System.Windows.Forms.TabControl tabView;
    private System.Windows.Forms.TabPage tabTree;
    private System.Windows.Forms.TabPage tabSearch;
    private System.Windows.Forms.TextBox txtSearch;
    private System.Windows.Forms.ListBox lstSearchResults;
    public System.Windows.Forms.TreeView tvwEntityHierarchy;
    private System.Windows.Forms.ComboBox cboEntityType;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TabPage tabSelected;
    private System.Windows.Forms.ListBox lstSelected;
    private System.Windows.Forms.PictureBox pictMode;
    private System.Windows.Forms.CheckBox ckIncludeTerms;
    private System.Windows.Forms.CheckBox ckSelectAll;
    private System.Windows.Forms.Button cmdSearch;
    private System.Windows.Forms.Button cmdRemove;
    private System.Windows.Forms.Button cmdExpandTree;

  }
}
