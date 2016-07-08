namespace CCI.DesktopClient.Screens
{
  partial class frmUserMaintenance
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
      this.btnSearch = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.txtLogin = new System.Windows.Forms.TextBox();
      this.txtPassword = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.txtFirstName = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.txtLastName = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.txtEmail = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.btnSave = new System.Windows.Forms.Button();
      this.label11 = new System.Windows.Forms.Label();
      this.txtUserType = new System.Windows.Forms.ComboBox();
      this.btnNew = new System.Windows.Forms.Button();
      this.ctlSearch1 = new CCI.DesktopClient.Common.ctlEntitySearch();
      this.lblDealer = new System.Windows.Forms.Label();
      this.txtDealer = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // btnSearch
      // 
      this.btnSearch.Location = new System.Drawing.Point(418, 4);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new System.Drawing.Size(75, 23);
      this.btnSearch.TabIndex = 1;
      this.btnSearch.Text = "Load";
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(10, 52);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(33, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Login";
      // 
      // txtLogin
      // 
      this.txtLogin.Location = new System.Drawing.Point(81, 49);
      this.txtLogin.Name = "txtLogin";
      this.txtLogin.Size = new System.Drawing.Size(331, 20);
      this.txtLogin.TabIndex = 2;
      // 
      // txtPassword
      // 
      this.txtPassword.Location = new System.Drawing.Point(81, 73);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.Size = new System.Drawing.Size(331, 20);
      this.txtPassword.TabIndex = 4;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(10, 76);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(53, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Password";
      // 
      // txtFirstName
      // 
      this.txtFirstName.Location = new System.Drawing.Point(81, 99);
      this.txtFirstName.Name = "txtFirstName";
      this.txtFirstName.Size = new System.Drawing.Size(331, 20);
      this.txtFirstName.TabIndex = 6;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(10, 102);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(54, 13);
      this.label5.TabIndex = 10;
      this.label5.Text = "FirstName";
      // 
      // txtLastName
      // 
      this.txtLastName.Location = new System.Drawing.Point(81, 125);
      this.txtLastName.Name = "txtLastName";
      this.txtLastName.Size = new System.Drawing.Size(331, 20);
      this.txtLastName.TabIndex = 7;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(10, 128);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(58, 13);
      this.label6.TabIndex = 12;
      this.label6.Text = "Last Name";
      // 
      // txtEmail
      // 
      this.txtEmail.Location = new System.Drawing.Point(81, 151);
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new System.Drawing.Size(331, 20);
      this.txtEmail.TabIndex = 8;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(10, 154);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(32, 13);
      this.label7.TabIndex = 14;
      this.label7.Text = "Email";
      // 
      // btnSave
      // 
      this.btnSave.DialogResult = System.Windows.Forms.DialogResult.Retry;
      this.btnSave.Location = new System.Drawing.Point(418, 47);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(75, 23);
      this.btnSave.TabIndex = 10;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(10, 180);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(53, 13);
      this.label11.TabIndex = 24;
      this.label11.Text = "UserType";
      // 
      // txtUserType
      // 
      this.txtUserType.FormattingEnabled = true;
      this.txtUserType.Location = new System.Drawing.Point(81, 177);
      this.txtUserType.Name = "txtUserType";
      this.txtUserType.Size = new System.Drawing.Size(331, 21);
      this.txtUserType.TabIndex = 9;
      this.txtUserType.SelectedIndexChanged += new System.EventHandler(this.txtUserType_SelectedIndexChanged);
      // 
      // btnNew
      // 
      this.btnNew.Location = new System.Drawing.Point(418, 73);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new System.Drawing.Size(75, 23);
      this.btnNew.TabIndex = 11;
      this.btnNew.Text = "New";
      this.btnNew.UseVisualStyleBackColor = true;
      this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
      // 
      // ctlSearch1
      // 
      this.ctlSearch1.AutoSelectWhenMatch = false;
      this.ctlSearch1.ClearSearchWhenComplete = false;
      this.ctlSearch1.Collapsed = true;
      this.ctlSearch1.Entity = "";
      this.ctlSearch1.EntityOwner = "CCI";
      this.ctlSearch1.EntityType = "";
      this.ctlSearch1.Location = new System.Drawing.Point(10, 4);
      this.ctlSearch1.MaxHeight = 204;
      this.ctlSearch1.Name = "ctlSearch1";
      this.ctlSearch1.ShowTermedCheckBox = true;
      this.ctlSearch1.Size = new System.Drawing.Size(401, 22);
      this.ctlSearch1.TabIndex = 25;
      this.ctlSearch1.OnSelected += new System.EventHandler<System.EventArgs>(this.ctlSearch1_OnSelected);
      // 
      // lblDealer
      // 
      this.lblDealer.AutoSize = true;
      this.lblDealer.Location = new System.Drawing.Point(10, 207);
      this.lblDealer.Name = "lblDealer";
      this.lblDealer.Size = new System.Drawing.Size(38, 13);
      this.lblDealer.TabIndex = 26;
      this.lblDealer.Text = "Dealer";
      // 
      // txtDealer
      // 
      this.txtDealer.FormattingEnabled = true;
      this.txtDealer.Location = new System.Drawing.Point(80, 204);
      this.txtDealer.Name = "txtDealer";
      this.txtDealer.Size = new System.Drawing.Size(331, 21);
      this.txtDealer.TabIndex = 27;
      // 
      // frmUserMaintenance
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(502, 267);
      this.Controls.Add(this.txtDealer);
      this.Controls.Add(this.lblDealer);
      this.Controls.Add(this.ctlSearch1);
      this.Controls.Add(this.btnNew);
      this.Controls.Add(this.txtUserType);
      this.Controls.Add(this.label11);
      this.Controls.Add(this.btnSave);
      this.Controls.Add(this.txtEmail);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.txtLastName);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.txtFirstName);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.txtPassword);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.txtLogin);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.btnSearch);
      this.Name = "frmUserMaintenance";
      this.Text = "User Maintenance";
      this.Load += new System.EventHandler(this.frmUserMaintenance_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnSearch;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txtLogin;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox txtFirstName;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox txtLastName;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox txtEmail;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.ComboBox txtUserType;
    private System.Windows.Forms.Button btnNew;
    private Common.ctlEntitySearch ctlSearch1;
    private System.Windows.Forms.Label lblDealer;
    private System.Windows.Forms.ComboBox txtDealer;
  }
}