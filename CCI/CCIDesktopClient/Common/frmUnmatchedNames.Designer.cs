namespace CCI.DesktopClient.Screens
{
  partial class frmUnmatchedNames
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
      this.grdUnmatchedNames = new System.Windows.Forms.DataGridView();
      this.grdSimilarNames = new System.Windows.Forms.DataGridView();
      this.btnFindSimilarNames = new System.Windows.Forms.Button();
      this.btnClear = new System.Windows.Forms.Button();
      this.btnLink = new System.Windows.Forms.Button();
      this.lblUnmatchedNames = new System.Windows.Forms.Label();
      this.lblSimilarNames = new System.Windows.Forms.Label();
      this.btnShowAllNames = new System.Windows.Forms.Button();
      this.ckAutoFind = new System.Windows.Forms.CheckBox();
      this.lblTitle = new System.Windows.Forms.Label();
      this.txtSearchTokens = new System.Windows.Forms.TextBox();
      this.btnSearch = new System.Windows.Forms.Button();
      this.btnResearch = new System.Windows.Forms.Button();
      this.btnAddNew = new System.Windows.Forms.Button();
      this.btnGetCityCareCustomers = new System.Windows.Forms.Button();
      this.btnUpdateCityHosted = new System.Windows.Forms.Button();
      this.splitMain = new System.Windows.Forms.SplitContainer();
      this.btnFlagException = new System.Windows.Forms.Button();
      this.ckIncludeNewCustomers = new System.Windows.Forms.CheckBox();
      this.ctlCityHostedDetail1 = new CCI.DesktopClient.Common.ctlCityHostedDetail();
      this.ckAutoResearch = new System.Windows.Forms.CheckBox();
      ((System.ComponentModel.ISupportInitialize)(this.grdUnmatchedNames)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.grdSimilarNames)).BeginInit();
      this.splitMain.Panel1.SuspendLayout();
      this.splitMain.Panel2.SuspendLayout();
      this.splitMain.SuspendLayout();
      this.SuspendLayout();
      // 
      // grdUnmatchedNames
      // 
      this.grdUnmatchedNames.AllowUserToAddRows = false;
      this.grdUnmatchedNames.AllowUserToDeleteRows = false;
      this.grdUnmatchedNames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.grdUnmatchedNames.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.grdUnmatchedNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdUnmatchedNames.Location = new System.Drawing.Point(3, 26);
      this.grdUnmatchedNames.MultiSelect = false;
      this.grdUnmatchedNames.Name = "grdUnmatchedNames";
      this.grdUnmatchedNames.Size = new System.Drawing.Size(443, 389);
      this.grdUnmatchedNames.TabIndex = 0;
      this.grdUnmatchedNames.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.grdUnmatchedNames_QueryContinueDrag);
      this.grdUnmatchedNames.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdUnmatchedNames_MouseDown);
      this.grdUnmatchedNames.MouseMove += new System.Windows.Forms.MouseEventHandler(this.grdUnmatchedNames_MouseMove);
      this.grdUnmatchedNames.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdUnmatchedNames_MouseUp);
      // 
      // grdSimilarNames
      // 
      this.grdSimilarNames.AllowDrop = true;
      this.grdSimilarNames.AllowUserToAddRows = false;
      this.grdSimilarNames.AllowUserToDeleteRows = false;
      this.grdSimilarNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grdSimilarNames.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.grdSimilarNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdSimilarNames.Location = new System.Drawing.Point(609, 48);
      this.grdSimilarNames.MultiSelect = false;
      this.grdSimilarNames.Name = "grdSimilarNames";
      this.grdSimilarNames.Size = new System.Drawing.Size(511, 367);
      this.grdSimilarNames.TabIndex = 1;
      this.grdSimilarNames.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmSimilarNames_DragDrop);
      this.grdSimilarNames.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmSimilarNames_DragEnter);
      this.grdSimilarNames.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmSimilarNames_MouseUp);
      // 
      // btnFindSimilarNames
      // 
      this.btnFindSimilarNames.Location = new System.Drawing.Point(452, 67);
      this.btnFindSimilarNames.MinimumSize = new System.Drawing.Size(100, 0);
      this.btnFindSimilarNames.Name = "btnFindSimilarNames";
      this.btnFindSimilarNames.Size = new System.Drawing.Size(151, 34);
      this.btnFindSimilarNames.TabIndex = 2;
      this.btnFindSimilarNames.Text = "Find Similar Names";
      this.btnFindSimilarNames.UseVisualStyleBackColor = true;
      this.btnFindSimilarNames.Click += new System.EventHandler(this.btnFindSimilarNames_Click);
      // 
      // btnClear
      // 
      this.btnClear.Location = new System.Drawing.Point(452, 108);
      this.btnClear.MinimumSize = new System.Drawing.Size(100, 0);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new System.Drawing.Size(151, 34);
      this.btnClear.TabIndex = 3;
      this.btnClear.Text = "Clear Similar Names";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
      // 
      // btnLink
      // 
      this.btnLink.Location = new System.Drawing.Point(452, 149);
      this.btnLink.MinimumSize = new System.Drawing.Size(100, 0);
      this.btnLink.Name = "btnLink";
      this.btnLink.Size = new System.Drawing.Size(151, 34);
      this.btnLink.TabIndex = 4;
      this.btnLink.Text = "Link";
      this.btnLink.UseVisualStyleBackColor = true;
      this.btnLink.Click += new System.EventHandler(this.btnLink_Click);
      // 
      // lblUnmatchedNames
      // 
      this.lblUnmatchedNames.AutoSize = true;
      this.lblUnmatchedNames.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblUnmatchedNames.Location = new System.Drawing.Point(3, 2);
      this.lblUnmatchedNames.Name = "lblUnmatchedNames";
      this.lblUnmatchedNames.Size = new System.Drawing.Size(161, 20);
      this.lblUnmatchedNames.TabIndex = 5;
      this.lblUnmatchedNames.Text = "Unmatched Names";
      // 
      // lblSimilarNames
      // 
      this.lblSimilarNames.AutoSize = true;
      this.lblSimilarNames.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblSimilarNames.Location = new System.Drawing.Point(609, 3);
      this.lblSimilarNames.Name = "lblSimilarNames";
      this.lblSimilarNames.Size = new System.Drawing.Size(123, 20);
      this.lblSimilarNames.TabIndex = 6;
      this.lblSimilarNames.Text = "Similar Names";
      // 
      // btnShowAllNames
      // 
      this.btnShowAllNames.Location = new System.Drawing.Point(452, 26);
      this.btnShowAllNames.MinimumSize = new System.Drawing.Size(100, 0);
      this.btnShowAllNames.Name = "btnShowAllNames";
      this.btnShowAllNames.Size = new System.Drawing.Size(151, 34);
      this.btnShowAllNames.TabIndex = 7;
      this.btnShowAllNames.Text = "Show All Names";
      this.btnShowAllNames.UseVisualStyleBackColor = true;
      this.btnShowAllNames.Click += new System.EventHandler(this.btnShowAllNames_Click);
      // 
      // ckAutoFind
      // 
      this.ckAutoFind.AutoSize = true;
      this.ckAutoFind.Checked = true;
      this.ckAutoFind.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckAutoFind.Location = new System.Drawing.Point(306, 7);
      this.ckAutoFind.Name = "ckAutoFind";
      this.ckAutoFind.Size = new System.Drawing.Size(140, 17);
      this.ckAutoFind.TabIndex = 8;
      this.ckAutoFind.Text = "Auto Find Similar Names";
      this.ckAutoFind.UseVisualStyleBackColor = true;
      // 
      // lblTitle
      // 
      this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTitle.Location = new System.Drawing.Point(452, 3);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new System.Drawing.Size(151, 20);
      this.lblTitle.TabIndex = 9;
      this.lblTitle.Text = "Similar Names";
      this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // txtSearchTokens
      // 
      this.txtSearchTokens.AcceptsReturn = true;
      this.txtSearchTokens.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSearchTokens.Location = new System.Drawing.Point(609, 26);
      this.txtSearchTokens.Name = "txtSearchTokens";
      this.txtSearchTokens.Size = new System.Drawing.Size(511, 20);
      this.txtSearchTokens.TabIndex = 10;
      this.txtSearchTokens.Leave += new System.EventHandler(this.txtSearchTokens_Leave);
      // 
      // btnSearch
      // 
      this.btnSearch.Location = new System.Drawing.Point(1056, 5);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new System.Drawing.Size(70, 19);
      this.btnSearch.TabIndex = 11;
      this.btnSearch.Text = "Search";
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
      // 
      // btnAddNew
      // 
      this.btnAddNew.Location = new System.Drawing.Point(452, 189);
      this.btnAddNew.Name = "btnAddNew";
      this.btnAddNew.Size = new System.Drawing.Size(151, 34);
      this.btnAddNew.TabIndex = 13;
      this.btnAddNew.Text = "Add New";
      this.btnAddNew.UseVisualStyleBackColor = true;
      this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
      // 
      // btnGetCityCareCustomers
      // 
      this.btnGetCityCareCustomers.Location = new System.Drawing.Point(452, 230);
      this.btnGetCityCareCustomers.Name = "btnGetCityCareCustomers";
      this.btnGetCityCareCustomers.Size = new System.Drawing.Size(151, 34);
      this.btnGetCityCareCustomers.TabIndex = 14;
      this.btnGetCityCareCustomers.Text = "Get New CityCare Customers";
      this.btnGetCityCareCustomers.UseVisualStyleBackColor = true;
      this.btnGetCityCareCustomers.Click += new System.EventHandler(this.btnGetCityCareCustomers_Click);
      // 
      // btnUpdateCityHosted
      // 
      this.btnUpdateCityHosted.Location = new System.Drawing.Point(452, 270);
      this.btnUpdateCityHosted.Name = "btnUpdateCityHosted";
      this.btnUpdateCityHosted.Size = new System.Drawing.Size(151, 34);
      this.btnUpdateCityHosted.TabIndex = 15;
      this.btnUpdateCityHosted.Text = "Update Hosted Detail";
      this.btnUpdateCityHosted.UseVisualStyleBackColor = true;
      this.btnUpdateCityHosted.Visible = false;
      this.btnUpdateCityHosted.Click += new System.EventHandler(this.btnUpdateCityHosted_Click);
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
      this.splitMain.Panel1.AutoScrollMinSize = new System.Drawing.Size(0, 350);
      this.splitMain.Panel1.Controls.Add(this.ckAutoResearch);
      this.splitMain.Panel1.Controls.Add(this.btnFlagException);
      this.splitMain.Panel1.Controls.Add(this.ckIncludeNewCustomers);
      this.splitMain.Panel1.Controls.Add(this.lblSimilarNames);
      this.splitMain.Panel1.Controls.Add(this.ckAutoFind);
      this.splitMain.Panel1.Controls.Add(this.grdSimilarNames);
      this.splitMain.Panel1.Controls.Add(this.btnUpdateCityHosted);
      this.splitMain.Panel1.Controls.Add(this.btnShowAllNames);
      this.splitMain.Panel1.Controls.Add(this.grdUnmatchedNames);
      this.splitMain.Panel1.Controls.Add(this.lblTitle);
      this.splitMain.Panel1.Controls.Add(this.btnGetCityCareCustomers);
      this.splitMain.Panel1.Controls.Add(this.lblUnmatchedNames);
      this.splitMain.Panel1.Controls.Add(this.txtSearchTokens);
      this.splitMain.Panel1.Controls.Add(this.btnAddNew);
      this.splitMain.Panel1.Controls.Add(this.btnLink);
      this.splitMain.Panel1.Controls.Add(this.btnFindSimilarNames);
      this.splitMain.Panel1.Controls.Add(this.btnSearch);
      this.splitMain.Panel1.Controls.Add(this.btnResearch);
      this.splitMain.Panel1.Controls.Add(this.btnClear);
      // 
      // splitMain.Panel2
      // 
      this.splitMain.Panel2.Controls.Add(this.ctlCityHostedDetail1);
      this.splitMain.Size = new System.Drawing.Size(1129, 735);
      this.splitMain.SplitterDistance = 418;
      this.splitMain.TabIndex = 16;
      // 
      // btnFlagException
      // 
      this.btnFlagException.Location = new System.Drawing.Point(452, 350);
      this.btnFlagException.Name = "btnFlagException";
      this.btnFlagException.Size = new System.Drawing.Size(151, 34);
      this.btnFlagException.TabIndex = 17;
      this.btnFlagException.Text = "Flag as Exception";
      this.btnFlagException.UseVisualStyleBackColor = true;
      this.btnFlagException.Visible = false;
      this.btnFlagException.Click += new System.EventHandler(this.btnFlagException_Click);
      // 
      // ckIncludeNewCustomers
      // 
      this.ckIncludeNewCustomers.AutoSize = true;
      this.ckIncludeNewCustomers.Location = new System.Drawing.Point(738, 6);
      this.ckIncludeNewCustomers.Name = "ckIncludeNewCustomers";
      this.ckIncludeNewCustomers.Size = new System.Drawing.Size(138, 17);
      this.ckIncludeNewCustomers.TabIndex = 16;
      this.ckIncludeNewCustomers.Text = "Include New Customers";
      this.ckIncludeNewCustomers.UseVisualStyleBackColor = true;
      this.ckIncludeNewCustomers.Visible = false;
      this.ckIncludeNewCustomers.CheckedChanged += new System.EventHandler(this.ckIncludeNewCustomers_CheckedChanged);
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
      this.ctlCityHostedDetail1.Size = new System.Drawing.Size(1129, 313);
      this.ctlCityHostedDetail1.TabIndex = 0;
      this.ctlCityHostedDetail1.USOC = null;
      // 
      // ckAutoResearch
      // 
      this.ckAutoResearch.AutoSize = true;
      this.ckAutoResearch.Checked = true;
      this.ckAutoResearch.CheckState = System.Windows.Forms.CheckState.Checked;
      this.ckAutoResearch.Location = new System.Drawing.Point(456, 390);
      this.ckAutoResearch.Name = "ckAutoResearch";
      this.ckAutoResearch.Size = new System.Drawing.Size(137, 17);
      this.ckAutoResearch.TabIndex = 18;
      this.ckAutoResearch.Text = "Auto Refresh Research";
      this.ckAutoResearch.UseVisualStyleBackColor = true;
      // 
      // frmUnmatchedNames
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1129, 735);
      this.Controls.Add(this.splitMain);
      this.Name = "frmUnmatchedNames";
      this.Text = "Unmatched Customers";
      this.Load += new System.EventHandler(this.frmUnmatchedNames_Load);
      ((System.ComponentModel.ISupportInitialize)(this.grdUnmatchedNames)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.grdSimilarNames)).EndInit();
      this.splitMain.Panel1.ResumeLayout(false);
      this.splitMain.Panel1.PerformLayout();
      this.splitMain.Panel2.ResumeLayout(false);
      this.splitMain.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView grdUnmatchedNames;
    private System.Windows.Forms.DataGridView grdSimilarNames;
    private System.Windows.Forms.Button btnFindSimilarNames;
    private System.Windows.Forms.Button btnClear;
    private System.Windows.Forms.Button btnLink;
    private System.Windows.Forms.Label lblUnmatchedNames;
    private System.Windows.Forms.Label lblSimilarNames;
    private System.Windows.Forms.Button btnShowAllNames;
    private System.Windows.Forms.CheckBox ckAutoFind;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.TextBox txtSearchTokens;
    private System.Windows.Forms.Button btnSearch;
    private System.Windows.Forms.Button btnResearch;
    private System.Windows.Forms.Button btnAddNew;
    private System.Windows.Forms.Button btnGetCityCareCustomers;
    private System.Windows.Forms.Button btnUpdateCityHosted;
    private System.Windows.Forms.SplitContainer splitMain;
    private Common.ctlCityHostedDetail ctlCityHostedDetail1;
    private System.Windows.Forms.CheckBox ckIncludeNewCustomers;
    private System.Windows.Forms.Button btnFlagException;
    private System.Windows.Forms.CheckBox ckAutoResearch;
  }
}