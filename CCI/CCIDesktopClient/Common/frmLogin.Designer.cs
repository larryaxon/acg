namespace CCI.DesktopClient.Common
{
  partial class frmLogin
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
      this.lblUserName = new System.Windows.Forms.Label();
      this.lblPassword = new System.Windows.Forms.Label();
      this.txtPassword = new System.Windows.Forms.TextBox();
      this.btnLogin = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.lblTitle = new System.Windows.Forms.Label();
      this.txtUserName = new System.Windows.Forms.ComboBox();
      this.lnkChangePassword = new System.Windows.Forms.LinkLabel();
      this.lblNewPassword = new System.Windows.Forms.Label();
      this.txtNewPassword = new System.Windows.Forms.TextBox();
      this.lblNewPassword2 = new System.Windows.Forms.Label();
      this.txtNewPassword2 = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // lblUserName
      // 
      this.lblUserName.AutoSize = true;
      this.lblUserName.Location = new System.Drawing.Point(52, 70);
      this.lblUserName.Name = "lblUserName";
      this.lblUserName.Size = new System.Drawing.Size(63, 13);
      this.lblUserName.TabIndex = 1;
      this.lblUserName.Text = "User Name:";
      // 
      // lblPassword
      // 
      this.lblPassword.AutoSize = true;
      this.lblPassword.Location = new System.Drawing.Point(52, 96);
      this.lblPassword.Name = "lblPassword";
      this.lblPassword.Size = new System.Drawing.Size(56, 13);
      this.lblPassword.TabIndex = 3;
      this.lblPassword.Text = "Password:";
      // 
      // txtPassword
      // 
      this.txtPassword.Location = new System.Drawing.Point(139, 93);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.PasswordChar = '•';
      this.txtPassword.Size = new System.Drawing.Size(192, 20);
      this.txtPassword.TabIndex = 4;
      // 
      // btnLogin
      // 
      this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(200)))), ((int)(((byte)(150)))));
      this.btnLogin.Location = new System.Drawing.Point(266, 152);
      this.btnLogin.Name = "btnLogin";
      this.btnLogin.Size = new System.Drawing.Size(75, 23);
      this.btnLogin.TabIndex = 5;
      this.btnLogin.Text = "Login";
      this.btnLogin.UseVisualStyleBackColor = false;
      this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(200)))), ((int)(((byte)(150)))));
      this.btnCancel.Location = new System.Drawing.Point(73, 152);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = false;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // lblTitle
      // 
      this.lblTitle.AutoSize = true;
      this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTitle.Location = new System.Drawing.Point(157, 9);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new System.Drawing.Size(85, 31);
      this.lblTitle.TabIndex = 0;
      this.lblTitle.Text = "Login";
      // 
      // txtUserName
      // 
      this.txtUserName.FormattingEnabled = true;
      this.txtUserName.Location = new System.Drawing.Point(139, 67);
      this.txtUserName.Name = "txtUserName";
      this.txtUserName.Size = new System.Drawing.Size(192, 21);
      this.txtUserName.TabIndex = 2;
      // 
      // lnkChangePassword
      // 
      this.lnkChangePassword.AutoSize = true;
      this.lnkChangePassword.Location = new System.Drawing.Point(339, 96);
      this.lnkChangePassword.Name = "lnkChangePassword";
      this.lnkChangePassword.Size = new System.Drawing.Size(60, 13);
      this.lnkChangePassword.TabIndex = 7;
      this.lnkChangePassword.TabStop = true;
      this.lnkChangePassword.Text = "change pw";
      this.lnkChangePassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkChangePassword_LinkClicked);
      // 
      // lblNewPassword
      // 
      this.lblNewPassword.AutoSize = true;
      this.lblNewPassword.Location = new System.Drawing.Point(52, 100);
      this.lblNewPassword.Name = "lblNewPassword";
      this.lblNewPassword.Size = new System.Drawing.Size(81, 13);
      this.lblNewPassword.TabIndex = 8;
      this.lblNewPassword.Text = "New Password:";
      this.lblNewPassword.Visible = false;
      // 
      // txtNewPassword
      // 
      this.txtNewPassword.Location = new System.Drawing.Point(139, 97);
      this.txtNewPassword.Name = "txtNewPassword";
      this.txtNewPassword.PasswordChar = '•';
      this.txtNewPassword.Size = new System.Drawing.Size(192, 20);
      this.txtNewPassword.TabIndex = 9;
      this.txtNewPassword.Visible = false;
      // 
      // lblNewPassword2
      // 
      this.lblNewPassword2.AutoSize = true;
      this.lblNewPassword2.Location = new System.Drawing.Point(52, 126);
      this.lblNewPassword2.Name = "lblNewPassword2";
      this.lblNewPassword2.Size = new System.Drawing.Size(85, 13);
      this.lblNewPassword2.TabIndex = 10;
      this.lblNewPassword2.Text = "Verify Password:";
      this.lblNewPassword2.Visible = false;
      // 
      // txtNewPassword2
      // 
      this.txtNewPassword2.AcceptsReturn = true;
      this.txtNewPassword2.Location = new System.Drawing.Point(139, 123);
      this.txtNewPassword2.Name = "txtNewPassword2";
      this.txtNewPassword2.PasswordChar = '•';
      this.txtNewPassword2.Size = new System.Drawing.Size(192, 20);
      this.txtNewPassword2.TabIndex = 11;
      this.txtNewPassword2.Visible = false;
      // 
      // frmLogin
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.ControlLightLight;
      this.ClientSize = new System.Drawing.Size(411, 197);
      this.Controls.Add(this.lblNewPassword2);
      this.Controls.Add(this.txtNewPassword2);
      this.Controls.Add(this.lblNewPassword);
      this.Controls.Add(this.txtNewPassword);
      this.Controls.Add(this.lnkChangePassword);
      this.Controls.Add(this.txtUserName);
      this.Controls.Add(this.lblTitle);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnLogin);
      this.Controls.Add(this.lblPassword);
      this.Controls.Add(this.txtPassword);
      this.Controls.Add(this.lblUserName);
      this.Name = "frmLogin";
      this.Text = "frmLogin";
      this.Load += new System.EventHandler(this.frmLogin_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblUserName;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Button btnLogin;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.ComboBox txtUserName;
    private System.Windows.Forms.LinkLabel lnkChangePassword;
    private System.Windows.Forms.Label lblNewPassword;
    private System.Windows.Forms.TextBox txtNewPassword;
    private System.Windows.Forms.Label lblNewPassword2;
    private System.Windows.Forms.TextBox txtNewPassword2;
  }
}