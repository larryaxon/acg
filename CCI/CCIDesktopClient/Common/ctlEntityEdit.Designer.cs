namespace CCI.DesktopClient.Common
{
  partial class ctlEntityEdit
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
      this.flowEntityMaintenance = new System.Windows.Forms.FlowLayoutPanel();
      this.SuspendLayout();
      // 
      // flowEntityMaintenance
      // 
      this.flowEntityMaintenance.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.flowEntityMaintenance.AutoScroll = true;
      this.flowEntityMaintenance.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.flowEntityMaintenance.Location = new System.Drawing.Point(3, 39);
      this.flowEntityMaintenance.Name = "flowEntityMaintenance";
      this.flowEntityMaintenance.Size = new System.Drawing.Size(454, 455);
      this.flowEntityMaintenance.TabIndex = 1;
      this.flowEntityMaintenance.Enter += new System.EventHandler(this.flowEntityMaintenance_Enter);
      // 
      // ctlEntityEdit
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.flowEntityMaintenance);
      this.Name = "ctlEntityEdit";
      this.Size = new System.Drawing.Size(460, 497);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.FlowLayoutPanel flowEntityMaintenance;
  }
}
