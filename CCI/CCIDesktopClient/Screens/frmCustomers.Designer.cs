namespace CCI.DesktopClient.Screens
{
  partial class frmCustomers
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
      this.btnRefreshFromCitycare = new System.Windows.Forms.Button();
      this.btnMerge = new System.Windows.Forms.Button();
      this.grpBillingAddress = new System.Windows.Forms.GroupBox();
      this.btnSyncBillingAddress = new System.Windows.Forms.Button();
      this.label8 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.txtBillingCellPhone = new System.Windows.Forms.TextBox();
      this.txtBillingPhone = new System.Windows.Forms.TextBox();
      this.txtBillingZip = new System.Windows.Forms.TextBox();
      this.txtBillingState = new System.Windows.Forms.ComboBox();
      this.txtBillingCity = new System.Windows.Forms.TextBox();
      this.txtBillingAddress2 = new System.Windows.Forms.TextBox();
      this.txtBillingAddress1 = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.textBox7 = new System.Windows.Forms.TextBox();
      this.lblDBA = new System.Windows.Forms.Label();
      this.txtDBA = new System.Windows.Forms.TextBox();
      this.tabMain.SuspendLayout();
      //this.tabLocations.SuspendLayout();
      this.grpBillingAddress.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabMain
      // 
      this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabMain.Size = new System.Drawing.Size(814, 344);
      // 
      // tabLocations
      // 
      //this.tabLocations.Size = new System.Drawing.Size(806, 318);
      // 
      // ctlLocations1
      // 
      //this.ctlLocations1.Size = new System.Drawing.Size(800, 312);
      // 
      // btnRefreshFromCitycare
      // 
      this.btnRefreshFromCitycare.Location = new System.Drawing.Point(871, 3);
      this.btnRefreshFromCitycare.Name = "btnRefreshFromCitycare";
      this.btnRefreshFromCitycare.Size = new System.Drawing.Size(132, 23);
      this.btnRefreshFromCitycare.TabIndex = 15;
      this.btnRefreshFromCitycare.Text = "Refresh from CityCare";
      this.btnRefreshFromCitycare.UseVisualStyleBackColor = true;
      this.btnRefreshFromCitycare.Visible = false;
      this.btnRefreshFromCitycare.Click += new System.EventHandler(this.btnRefreshFromCitycare_Click);
      // 
      // btnMerge
      // 
      this.btnMerge.Location = new System.Drawing.Point(754, 4);
      this.btnMerge.Name = "btnMerge";
      this.btnMerge.Size = new System.Drawing.Size(111, 23);
      this.btnMerge.TabIndex = 16;
      this.btnMerge.Text = "Merge Customer";
      this.btnMerge.UseVisualStyleBackColor = true;
      this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
      // 
      // grpBillingAddress
      // 
      this.grpBillingAddress.Controls.Add(this.btnSyncBillingAddress);
      this.grpBillingAddress.Controls.Add(this.label8);
      this.grpBillingAddress.Controls.Add(this.label7);
      this.grpBillingAddress.Controls.Add(this.txtBillingCellPhone);
      this.grpBillingAddress.Controls.Add(this.txtBillingPhone);
      this.grpBillingAddress.Controls.Add(this.txtBillingZip);
      this.grpBillingAddress.Controls.Add(this.txtBillingState);
      this.grpBillingAddress.Controls.Add(this.txtBillingCity);
      this.grpBillingAddress.Controls.Add(this.txtBillingAddress2);
      this.grpBillingAddress.Controls.Add(this.txtBillingAddress1);
      this.grpBillingAddress.Controls.Add(this.label4);
      this.grpBillingAddress.Controls.Add(this.label5);
      this.grpBillingAddress.Controls.Add(this.label6);
      this.grpBillingAddress.Location = new System.Drawing.Point(10, 199);
      this.grpBillingAddress.Name = "grpBillingAddress";
      this.grpBillingAddress.Size = new System.Drawing.Size(364, 143);
      this.grpBillingAddress.TabIndex = 18;
      this.grpBillingAddress.TabStop = false;
      this.grpBillingAddress.Text = "Billing Address";
      // 
      // btnSyncBillingAddress
      // 
      this.btnSyncBillingAddress.Location = new System.Drawing.Point(67, 118);
      this.btnSyncBillingAddress.Name = "btnSyncBillingAddress";
      this.btnSyncBillingAddress.Size = new System.Drawing.Size(286, 25);
      this.btnSyncBillingAddress.TabIndex = 10;
      this.btnSyncBillingAddress.Text = "Copy Billing from this Address";
      this.btnSyncBillingAddress.UseVisualStyleBackColor = true;
      this.btnSyncBillingAddress.Click += new System.EventHandler(this.btnSyncBillingAddress_Click);
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(200, 95);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(24, 13);
      this.label8.TabIndex = 9;
      this.label8.Text = "Cell";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(9, 95);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(38, 13);
      this.label7.TabIndex = 8;
      this.label7.Text = "Phone";
      // 
      // txtBillingCellPhone
      // 
      this.txtBillingCellPhone.Location = new System.Drawing.Point(230, 92);
      this.txtBillingCellPhone.Name = "txtBillingCellPhone";
      this.txtBillingCellPhone.Size = new System.Drawing.Size(125, 20);
      this.txtBillingCellPhone.TabIndex = 6;
      // 
      // txtBillingPhone
      // 
      this.txtBillingPhone.Location = new System.Drawing.Point(67, 92);
      this.txtBillingPhone.Name = "txtBillingPhone";
      this.txtBillingPhone.Size = new System.Drawing.Size(127, 20);
      this.txtBillingPhone.TabIndex = 5;
      // 
      // txtBillingZip
      // 
      this.txtBillingZip.Location = new System.Drawing.Point(310, 66);
      this.txtBillingZip.Name = "txtBillingZip";
      this.txtBillingZip.Size = new System.Drawing.Size(46, 20);
      this.txtBillingZip.TabIndex = 4;
      // 
      // txtBillingState
      // 
      this.txtBillingState.FormattingEnabled = true;
      this.txtBillingState.Location = new System.Drawing.Point(261, 65);
      this.txtBillingState.Name = "txtBillingState";
      this.txtBillingState.Size = new System.Drawing.Size(42, 21);
      this.txtBillingState.TabIndex = 3;
      // 
      // txtBillingCity
      // 
      this.txtBillingCity.Location = new System.Drawing.Point(67, 66);
      this.txtBillingCity.Name = "txtBillingCity";
      this.txtBillingCity.Size = new System.Drawing.Size(184, 20);
      this.txtBillingCity.TabIndex = 2;
      // 
      // txtBillingAddress2
      // 
      this.txtBillingAddress2.Location = new System.Drawing.Point(67, 41);
      this.txtBillingAddress2.Name = "txtBillingAddress2";
      this.txtBillingAddress2.Size = new System.Drawing.Size(288, 20);
      this.txtBillingAddress2.TabIndex = 1;
      // 
      // txtBillingAddress1
      // 
      this.txtBillingAddress1.Location = new System.Drawing.Point(67, 16);
      this.txtBillingAddress1.Name = "txtBillingAddress1";
      this.txtBillingAddress1.Size = new System.Drawing.Size(288, 20);
      this.txtBillingAddress1.TabIndex = 0;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(9, 69);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(59, 13);
      this.label4.TabIndex = 2;
      this.label4.Text = "City/St/Zip";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(9, 44);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(54, 13);
      this.label5.TabIndex = 1;
      this.label5.Text = "Address 2";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(9, 23);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(54, 13);
      this.label6.TabIndex = 0;
      this.label6.Text = "Address 1";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(200, 95);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(24, 13);
      this.label2.TabIndex = 11;
      this.label2.Text = "Cell";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(8, 95);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(38, 13);
      this.label3.TabIndex = 8;
      this.label3.Text = "Phone";
      // 
      // textBox7
      // 
      this.textBox7.Enabled = false;
      this.textBox7.Location = new System.Drawing.Point(468, 131);
      this.textBox7.Name = "textBox7";
      this.textBox7.Size = new System.Drawing.Size(57, 20);
      this.textBox7.TabIndex = 17;
      // 
      // lblDBA
      // 
      this.lblDBA.AutoSize = true;
      this.lblDBA.Location = new System.Drawing.Point(313, 32);
      this.lblDBA.Name = "lblDBA";
      this.lblDBA.Size = new System.Drawing.Size(29, 13);
      this.lblDBA.TabIndex = 19;
      this.lblDBA.Text = "DBA";
      // 
      // txtDBA
      // 
      this.txtDBA.Location = new System.Drawing.Point(351, 28);
      this.txtDBA.Name = "txtDBA";
      this.txtDBA.Size = new System.Drawing.Size(320, 20);
      this.txtDBA.TabIndex = 20;
      // 
      // frmCustomers
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1196, 413);
      this.Controls.Add(this.txtDBA);
      this.Controls.Add(this.lblDBA);
      this.Controls.Add(this.grpBillingAddress);
      this.Controls.Add(this.textBox7);
      this.Controls.Add(this.btnMerge);
      this.Controls.Add(this.btnRefreshFromCitycare);
      this.Name = "frmCustomers";
      this.Text = "frmCustomers";
      this.Controls.SetChildIndex(this.txtLegalName, 0);
      this.Controls.SetChildIndex(this.txtEntity, 0);
      this.Controls.SetChildIndex(this.lblNewRecord, 0);
      this.Controls.SetChildIndex(this.btnRefreshFromCitycare, 0);
      this.Controls.SetChildIndex(this.btnMerge, 0);
      this.Controls.SetChildIndex(this.textBox7, 0);
      this.Controls.SetChildIndex(this.grpBillingAddress, 0);
      this.Controls.SetChildIndex(this.tabMain, 0);
      this.Controls.SetChildIndex(this.lblDBA, 0);
      this.Controls.SetChildIndex(this.txtDBA, 0);
      this.tabMain.ResumeLayout(false);
      //this.tabLocations.ResumeLayout(false);
      this.grpBillingAddress.ResumeLayout(false);
      this.grpBillingAddress.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnRefreshFromCitycare;
    private System.Windows.Forms.Button btnMerge;
    private System.Windows.Forms.GroupBox grpBillingAddress;
    private System.Windows.Forms.TextBox txtBillingCellPhone;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtBillingPhone;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtBillingZip;
    private System.Windows.Forms.ComboBox txtBillingState;
    private System.Windows.Forms.TextBox txtBillingCity;
    private System.Windows.Forms.TextBox txtBillingAddress2;
    private System.Windows.Forms.TextBox txtBillingAddress1;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox textBox7;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Button btnSyncBillingAddress;
    private System.Windows.Forms.Label lblDBA;
    private System.Windows.Forms.TextBox txtDBA;
  }
}