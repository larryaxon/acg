namespace ACG.DesktopClient.Common
{
  partial class ctlMaintenanceBase
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
      this.grdMaintenance = new System.Windows.Forms.DataGridView();
      this.splitMain = new System.Windows.Forms.SplitContainer();
      this.txtNoteLabel = new System.Windows.Forms.TextBox();
      this.txtValue = new System.Windows.Forms.TextBox();
      this.mnuGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.grdMaintenance)).BeginInit();
      this.splitMain.Panel1.SuspendLayout();
      this.splitMain.Panel2.SuspendLayout();
      this.splitMain.SuspendLayout();
      this.mnuGrid.SuspendLayout();
      this.SuspendLayout();
      // 
      // grdMaintenance
      // 
      this.grdMaintenance.AllowUserToOrderColumns = true;
      this.grdMaintenance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.grdMaintenance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdMaintenance.Dock = System.Windows.Forms.DockStyle.Fill;
      this.grdMaintenance.Location = new System.Drawing.Point(0, 0);
      this.grdMaintenance.Name = "grdMaintenance";
      this.grdMaintenance.Size = new System.Drawing.Size(1013, 355);
      this.grdMaintenance.TabIndex = 0;
      this.grdMaintenance.VirtualMode = true;
      this.grdMaintenance.CancelRowEdit += new System.Windows.Forms.QuestionEventHandler(this.grdMaintenance_CancelRowEdit);
      this.grdMaintenance.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdMaintenance_CellEnter);
      this.grdMaintenance.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdMaintenance_CellLeave);
      this.grdMaintenance.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdMaintenance_CellValidated);
      this.grdMaintenance.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdMaintenance_CellValueChanged);
      this.grdMaintenance.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grdMaintenance_DataError);
      this.grdMaintenance.NewRowNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.grdMaintenance_NewRowNeeded);
      this.grdMaintenance.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdMaintenance_RowEnter);
      this.grdMaintenance.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.grdMaintenance_RowsRemoved);
      this.grdMaintenance.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdMaintenance_RowValidated);
      this.grdMaintenance.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.grdMaintenance_RowValidating);
      this.grdMaintenance.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.grdMaintenance_UserAddedRow);
      this.grdMaintenance.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.grdMaintenance_MouseDoubleClick);
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
      this.splitMain.Panel1.Controls.Add(this.grdMaintenance);
      // 
      // splitMain.Panel2
      // 
      this.splitMain.Panel2.Controls.Add(this.txtNoteLabel);
      this.splitMain.Panel2.Controls.Add(this.txtValue);
      this.splitMain.Size = new System.Drawing.Size(1013, 469);
      this.splitMain.SplitterDistance = 355;
      this.splitMain.TabIndex = 1;
      // 
      // txtNoteLabel
      // 
      this.txtNoteLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.txtNoteLabel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
      this.txtNoteLabel.BackColor = System.Drawing.SystemColors.ControlLight;
      this.txtNoteLabel.Enabled = false;
      this.txtNoteLabel.Location = new System.Drawing.Point(2, 1);
      this.txtNoteLabel.Multiline = true;
      this.txtNoteLabel.Name = "txtNoteLabel";
      this.txtNoteLabel.Size = new System.Drawing.Size(146, 108);
      this.txtNoteLabel.TabIndex = 1;
      this.txtNoteLabel.Text = "Note for Opportunty";
      // 
      // txtValue
      // 
      this.txtValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtValue.Location = new System.Drawing.Point(148, 0);
      this.txtValue.Multiline = true;
      this.txtValue.Name = "txtValue";
      this.txtValue.Size = new System.Drawing.Size(865, 110);
      this.txtValue.TabIndex = 0;
      this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
      this.txtValue.Leave += new System.EventHandler(this.txtValue_Leave);
      // 
      // mnuGrid
      // 
      this.mnuGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem});
      this.mnuGrid.Name = "mnuGrid";
      this.mnuGrid.Size = new System.Drawing.Size(153, 48);
      // 
      // exportToolStripMenuItem
      // 
      this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
      this.exportToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.exportToolStripMenuItem.Text = "Export";
      this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
      // 
      // ctlMaintenanceBase
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.splitMain);
      this.Name = "ctlMaintenanceBase";
      this.Size = new System.Drawing.Size(1013, 469);
      ((System.ComponentModel.ISupportInitialize)(this.grdMaintenance)).EndInit();
      this.splitMain.Panel1.ResumeLayout(false);
      this.splitMain.Panel2.ResumeLayout(false);
      this.splitMain.Panel2.PerformLayout();
      this.splitMain.ResumeLayout(false);
      this.mnuGrid.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    protected System.Windows.Forms.DataGridView grdMaintenance;
    private System.Windows.Forms.SplitContainer splitMain;
    private System.Windows.Forms.TextBox txtValue;
    private System.Windows.Forms.TextBox txtNoteLabel;
    private System.Windows.Forms.ContextMenuStrip mnuGrid;
    private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
  }
}