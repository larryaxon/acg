namespace ACG.CommonForms
{
  partial class ctlACGDate
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
      this.txtDate = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // txtDate
      // 
      this.txtDate.Dock = System.Windows.Forms.DockStyle.Fill;
      this.txtDate.Location = new System.Drawing.Point(0, 0);
      this.txtDate.Name = "txtDate";
      this.txtDate.Size = new System.Drawing.Size(245, 20);
      this.txtDate.TabIndex = 0;
      this.txtDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.txtDate.Leave += new System.EventHandler(this.txtDate_Leave);
      this.txtDate.Validating += new System.ComponentModel.CancelEventHandler(this.txtDate_Validating);
      // 
      // ctlACGDate
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.txtDate);
      this.Name = "ctlACGDate";
      this.Size = new System.Drawing.Size(245, 20);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox txtDate;
  }
}
