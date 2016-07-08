namespace CCI.DesktopClient.Screens
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
      this.grdMaintenance = new System.Windows.Forms.DataGridView();
      ((System.ComponentModel.ISupportInitialize)(this.grdMaintenance)).BeginInit();
      this.SuspendLayout();
      // 
      // grdCodeMaster
      // 
      this.grdMaintenance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdMaintenance.Dock = System.Windows.Forms.DockStyle.Fill;
      this.grdMaintenance.Location = new System.Drawing.Point(0, 0);
      this.grdMaintenance.Name = "grdCodeMaster";
      this.grdMaintenance.Size = new System.Drawing.Size(1013, 469);
      this.grdMaintenance.TabIndex = 0;
      this.grdMaintenance.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdMaintenance_RowValidated);
      // 
      // frmCodeMaster
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1013, 469);
      this.Controls.Add(this.grdMaintenance);
      this.Name = "frmCodeMaster";
      this.Text = "Manage Codes";
      ((System.ComponentModel.ISupportInitialize)(this.grdMaintenance)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView grdMaintenance;
  }
}