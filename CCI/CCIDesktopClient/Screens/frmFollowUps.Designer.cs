namespace CCI.DesktopClient.Screens
{
  partial class frmFollowUps
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
      this.ctlFollowUps1 = new CCI.DesktopClient.Common.ctlFollowUps();
      this.SuspendLayout();
      // 
      // ctlFollowUps1
      // 
      this.ctlFollowUps1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ctlFollowUps1.Location = new System.Drawing.Point(0, 0);
      this.ctlFollowUps1.Name = "ctlFollowUps1";
      this.ctlFollowUps1.Size = new System.Drawing.Size(1113, 474);
      this.ctlFollowUps1.TabIndex = 0;
      // 
      // frmFollowUps
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1113, 474);
      this.Controls.Add(this.ctlFollowUps1);
      this.Name = "frmFollowUps";
      this.Text = "frmFollowUps";
      this.Load += new System.EventHandler(this.frmFollowUps_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private Common.ctlFollowUps ctlFollowUps1;
  }
}