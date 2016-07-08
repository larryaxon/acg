namespace CCI.DesktopClient.Screens
{
  partial class dlgMergeCustomer
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
      this.srchFromCustomer = new ACG.CommonForms.ctlSearch();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.srchToCustomer = new ACG.CommonForms.ctlSearch();
      this.label3 = new System.Windows.Forms.Label();
      this.btnMerge = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // srchFromCustomer
      // 
      this.srchFromCustomer.AddNewMode = false;
      this.srchFromCustomer.AutoSelectWhenMatch = false;
      this.srchFromCustomer.ClearSearchWhenComplete = false;
      this.srchFromCustomer.Collapsed = true;
      this.srchFromCustomer.ID = "";
      this.srchFromCustomer.Location = new System.Drawing.Point(161, 91);
      this.srchFromCustomer.MaxHeight = 228;
      this.srchFromCustomer.Name = "srchFromCustomer";
      this.srchFromCustomer.ShowCustomerNameWhenSet = true;
      this.srchFromCustomer.ShowTermedCheckBox = false;
      this.srchFromCustomer.Size = new System.Drawing.Size(371, 25);
      this.srchFromCustomer.TabIndex = 0;
      this.srchFromCustomer.OnSelected += new System.EventHandler<System.EventArgs>(this.srchFromCustomer_OnSelected);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(78, 97);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(77, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "From Customer";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(78, 140);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(67, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "To Customer";
      // 
      // srchToCustomer
      // 
      this.srchToCustomer.AddNewMode = false;
      this.srchToCustomer.AutoSelectWhenMatch = false;
      this.srchToCustomer.ClearSearchWhenComplete = false;
      this.srchToCustomer.Collapsed = true;
      this.srchToCustomer.ID = "";
      this.srchToCustomer.Location = new System.Drawing.Point(161, 134);
      this.srchToCustomer.MaxHeight = 228;
      this.srchToCustomer.Name = "srchToCustomer";
      this.srchToCustomer.ShowCustomerNameWhenSet = true;
      this.srchToCustomer.ShowTermedCheckBox = false;
      this.srchToCustomer.Size = new System.Drawing.Size(371, 25);
      this.srchToCustomer.TabIndex = 2;
      this.srchToCustomer.OnSelected += new System.EventHandler<System.EventArgs>(this.srchToCustomer_OnSelected);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(223, 19);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(188, 26);
      this.label3.TabIndex = 4;
      this.label3.Text = "Merge Customer";
      // 
      // btnMerge
      // 
      this.btnMerge.Location = new System.Drawing.Point(435, 177);
      this.btnMerge.Name = "btnMerge";
      this.btnMerge.Size = new System.Drawing.Size(96, 32);
      this.btnMerge.TabIndex = 5;
      this.btnMerge.Text = "Merge";
      this.btnMerge.UseVisualStyleBackColor = true;
      this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(81, 177);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(96, 32);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // dlgMergeCustomer
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(637, 258);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnMerge);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.srchToCustomer);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.srchFromCustomer);
      this.Name = "dlgMergeCustomer";
      this.Text = "dlgMergeCustomer";
      this.Load += new System.EventHandler(this.dlgMergeCustomer_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private ACG.CommonForms.ctlSearch srchFromCustomer;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private ACG.CommonForms.ctlSearch srchToCustomer;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button btnMerge;
    private System.Windows.Forms.Button btnCancel;
  }
}