namespace CCI.DesktopClient.Screens
{
  partial class frmImportUSOCExceptions
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
      this.grdWholesaleExceptions = new System.Windows.Forms.DataGridView();
      this.pnlEditPanel = new System.Windows.Forms.Panel();
      ((System.ComponentModel.ISupportInitialize)(this.grdWholesaleExceptions)).BeginInit();
      this.SuspendLayout();
      // 
      // grdWholesaleExceptions
      // 
      this.grdWholesaleExceptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grdWholesaleExceptions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdWholesaleExceptions.Location = new System.Drawing.Point(5, 40);
      this.grdWholesaleExceptions.Name = "grdWholesaleExceptions";
      this.grdWholesaleExceptions.Size = new System.Drawing.Size(885, 284);
      this.grdWholesaleExceptions.TabIndex = 0;
      this.grdWholesaleExceptions.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdWholesaleExceptions_RowEnter);
      this.grdWholesaleExceptions.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdWholesaleExceptions_RowHeaderMouseClick);
      // 
      // pnlEditPanel
      // 
      this.pnlEditPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlEditPanel.Location = new System.Drawing.Point(5, 330);
      this.pnlEditPanel.Name = "pnlEditPanel";
      this.pnlEditPanel.Size = new System.Drawing.Size(884, 110);
      this.pnlEditPanel.TabIndex = 1;
      // 
      // frmImportUSOCExceptions
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(892, 447);
      this.Controls.Add(this.pnlEditPanel);
      this.Controls.Add(this.grdWholesaleExceptions);
      this.Name = "frmImportUSOCExceptions";
      this.Text = "Wholesale Usoc Import Exceptions";
      ((System.ComponentModel.ISupportInitialize)(this.grdWholesaleExceptions)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView grdWholesaleExceptions;
    private System.Windows.Forms.Panel pnlEditPanel;
  }
}