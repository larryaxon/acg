namespace ACG.DesktopClient.Screens
{
  partial class frmProjects
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
      this.label1 = new System.Windows.Forms.Label();
      this.cboCustomer = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.cboResource = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(9, 7);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(54, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Customer:";
      // 
      // cboCustomer
      // 
      this.cboCustomer.FormattingEnabled = true;
      this.cboCustomer.Location = new System.Drawing.Point(73, 6);
      this.cboCustomer.Name = "cboCustomer";
      this.cboCustomer.Size = new System.Drawing.Size(153, 21);
      this.cboCustomer.TabIndex = 3;
      this.cboCustomer.SelectedIndexChanged += new System.EventHandler(this.cboCustomer_SelectedIndexChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(242, 8);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(56, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Resource:";
      // 
      // cboResource
      // 
      this.cboResource.FormattingEnabled = true;
      this.cboResource.Location = new System.Drawing.Point(311, 4);
      this.cboResource.Name = "cboResource";
      this.cboResource.Size = new System.Drawing.Size(148, 21);
      this.cboResource.TabIndex = 5;
      this.cboResource.SelectedIndexChanged += new System.EventHandler(this.cboResource_SelectedIndexChanged);
      // 
      // frmProjects
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1231, 539);
      this.Controls.Add(this.cboResource);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.cboCustomer);
      this.Controls.Add(this.label1);
      this.Name = "frmProjects";
      this.Text = "frmProjects";
      this.Load += new System.EventHandler(this.frmProjects_Load);
      this.Controls.SetChildIndex(this.label1, 0);
      this.Controls.SetChildIndex(this.cboCustomer, 0);
      this.Controls.SetChildIndex(this.label2, 0);
      this.Controls.SetChildIndex(this.cboResource, 0);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cboCustomer;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cboResource;
  }
}