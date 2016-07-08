namespace ACG.DesktopClient.Screens
{
  partial class frmMaintenance
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
      this.ctlMaintenanceMain = new ACG.DesktopClient.Common.ctlMaintenanceBase();
      this.splitSub = new System.Windows.Forms.SplitContainer();
      this.ctlMaintenanceSub = new ACG.DesktopClient.Common.ctlMaintenanceBase();
      this.ctlMaintenanceSub2 = new ACG.DesktopClient.Common.ctlMaintenanceBase();
      ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
      this.splitMain.Panel1.SuspendLayout();
      this.splitMain.Panel2.SuspendLayout();
      this.splitMain.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitSub)).BeginInit();
      this.splitSub.Panel1.SuspendLayout();
      this.splitSub.Panel2.SuspendLayout();
      this.splitSub.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitMain
      // 
      this.splitMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.splitMain.Location = new System.Drawing.Point(5, 53);
      this.splitMain.Name = "splitMain";
      this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitMain.Panel1
      // 
      this.splitMain.Panel1.Controls.Add(this.ctlMaintenanceMain);
      // 
      // splitMain.Panel2
      // 
      this.splitMain.Panel2.Controls.Add(this.splitSub);
      this.splitMain.Size = new System.Drawing.Size(1224, 482);
      this.splitMain.SplitterDistance = 120;
      this.splitMain.TabIndex = 1;
      // 
      // ctlMaintenanceMain
      // 
      this.ctlMaintenanceMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ctlMaintenanceMain.ExtendedFieldCaptionColumn = null;
      this.ctlMaintenanceMain.ExtendedFieldName = null;
      this.ctlMaintenanceMain.FireRowSelectedOnSearch = true;
      this.ctlMaintenanceMain.Location = new System.Drawing.Point(0, 0);
      this.ctlMaintenanceMain.Name = "ctlMaintenanceMain";
      this.ctlMaintenanceMain.NoteLabel = "Note for";
      this.ctlMaintenanceMain.ReadOnly = false;
      this.ctlMaintenanceMain.SecurityContext = null;
      this.ctlMaintenanceMain.ShowExtendedField = false;
      this.ctlMaintenanceMain.Size = new System.Drawing.Size(1224, 120);
      this.ctlMaintenanceMain.TabIndex = 1;
      this.ctlMaintenanceMain.TableName = "";
      this.ctlMaintenanceMain.UniqueIdentifier = null;
      this.ctlMaintenanceMain.RowSelected += new ACG.DesktopClient.Common.ctlMaintenanceBase.RowSelectedHandler(this.ctlMaintenanceMain_RowSelected);
      this.ctlMaintenanceMain.Load += new System.EventHandler(this.frmMaintenance_Load);
      // 
      // splitSub
      // 
      this.splitSub.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitSub.Location = new System.Drawing.Point(0, 0);
      this.splitSub.Name = "splitSub";
      this.splitSub.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitSub.Panel1
      // 
      this.splitSub.Panel1.Controls.Add(this.ctlMaintenanceSub);
      // 
      // splitSub.Panel2
      // 
      this.splitSub.Panel2.Controls.Add(this.ctlMaintenanceSub2);
      this.splitSub.Size = new System.Drawing.Size(1224, 358);
      this.splitSub.SplitterDistance = 164;
      this.splitSub.TabIndex = 4;
      // 
      // ctlMaintenanceSub
      // 
      this.ctlMaintenanceSub.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ctlMaintenanceSub.ExtendedFieldCaptionColumn = null;
      this.ctlMaintenanceSub.ExtendedFieldName = null;
      this.ctlMaintenanceSub.FireRowSelectedOnSearch = true;
      this.ctlMaintenanceSub.Location = new System.Drawing.Point(0, 0);
      this.ctlMaintenanceSub.Name = "ctlMaintenanceSub";
      this.ctlMaintenanceSub.NoteLabel = "Note for";
      this.ctlMaintenanceSub.ReadOnly = false;
      this.ctlMaintenanceSub.SecurityContext = null;
      this.ctlMaintenanceSub.ShowExtendedField = false;
      this.ctlMaintenanceSub.Size = new System.Drawing.Size(1224, 164);
      this.ctlMaintenanceSub.TabIndex = 1;
      this.ctlMaintenanceSub.TableName = "";
      this.ctlMaintenanceSub.UniqueIdentifier = null;
      this.ctlMaintenanceSub.RowSelected += new ACG.DesktopClient.Common.ctlMaintenanceBase.RowSelectedHandler(this.ctlMaintenanceSub_RowSelected);
      // 
      // ctlMaintenanceSub2
      // 
      this.ctlMaintenanceSub2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ctlMaintenanceSub2.ExtendedFieldCaptionColumn = null;
      this.ctlMaintenanceSub2.ExtendedFieldName = null;
      this.ctlMaintenanceSub2.FireRowSelectedOnSearch = true;
      this.ctlMaintenanceSub2.Location = new System.Drawing.Point(0, 0);
      this.ctlMaintenanceSub2.Name = "ctlMaintenanceSub2";
      this.ctlMaintenanceSub2.NoteLabel = "Note for";
      this.ctlMaintenanceSub2.ReadOnly = false;
      this.ctlMaintenanceSub2.SecurityContext = null;
      this.ctlMaintenanceSub2.ShowExtendedField = false;
      this.ctlMaintenanceSub2.Size = new System.Drawing.Size(1224, 190);
      this.ctlMaintenanceSub2.TabIndex = 0;
      this.ctlMaintenanceSub2.TableName = "";
      this.ctlMaintenanceSub2.UniqueIdentifier = null;
      // 
      // frmMaintenance
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1231, 539);
      this.Controls.Add(this.splitMain);
      this.Name = "frmMaintenance";
      this.Text = "frmMaintenance";
      this.Load += new System.EventHandler(this.frmMaintenance_Load);
      this.splitMain.Panel1.ResumeLayout(false);
      this.splitMain.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
      this.splitMain.ResumeLayout(false);
      this.splitSub.Panel1.ResumeLayout(false);
      this.splitSub.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitSub)).EndInit();
      this.splitSub.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitMain;
    protected Common.ctlMaintenanceBase ctlMaintenanceMain;
    private System.Windows.Forms.SplitContainer splitSub;
    protected Common.ctlMaintenanceBase ctlMaintenanceSub;
    protected Common.ctlMaintenanceBase ctlMaintenanceSub2;

  }
}