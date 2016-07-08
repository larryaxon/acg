using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ACG.App.Common;
using ACG.Sys.Data;

namespace ACG.DesktopClient.Common
{
  public partial class frmLogin : Form
  {      
    int _shiftUp = 22;  // nbr pixels we shift the uid/pwd fields up or down to make room for "new password" entry

    SecurityContext _s = null;
    DataSource dataSource = new DataSource();
    public frmLogin(SecurityContext securityContext)
    {
      _s = securityContext;
      InitializeComponent();
    }

    private void frmLogin_Load(object sender, EventArgs e)
    {
      string[] userList = dataSource.getUserList();
      txtUserName.Items.Clear();
      for (int i = 0; i < userList.GetLength(0); i++)
        txtUserName.Items.Add(userList[i]);
    }

    private void btnLogin_Click(object sender, EventArgs e)
    {
      _s.Login = txtUserName.Text;
      _s.Password = txtPassword.Text;
      _s = dataSource.Login(_s);
      if (!_s.IsLoggedIn)
        MessageBox.Show("Login failed");
      else
      {
        if (txtNewPassword.Visible && _s.IsLoggedIn)
        {
          if (txtNewPassword.Text.Equals(txtNewPassword2.Text))
          {
            _s.NewPassword = txtNewPassword.Text;
            dataSource.SavePassword(_s);
            showNewPassword(false);
          }
          else
            MessageBox.Show("New Passwords do not match");
        }
      }
      if (_s.IsLoggedIn)
        this.Close();    
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (txtNewPassword.Visible)
      {
        showNewPassword(false);
      }
      _s.Cancelled = true;
      this.Close();
    }

    private void lnkChangePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      if (lnkChangePassword.Text == "change pw")
        showNewPassword(true);
      else
        showNewPassword(false);
    }
    private void showNewPassword(bool show)
    {
      if (show)
      {
        lblUserName.Top -= _shiftUp;
        txtUserName.Top -= _shiftUp;
        lblPassword.Top -= _shiftUp;
        txtPassword.Top -= _shiftUp;
        lblNewPassword.Visible = true;
        txtNewPassword.Visible = true;
        lblNewPassword2.Visible = true;
        txtNewPassword2.Visible = true;
        btnLogin.Text = "Save PW";
        lnkChangePassword.Text = "cancel";
        txtNewPassword.Focus();
      }
      else
      {
        lblNewPassword.Visible = false;
        txtNewPassword.Visible = false;
        lblNewPassword2.Visible = false;
        txtNewPassword2.Visible = false;
        lblUserName.Top += _shiftUp;
        txtUserName.Top += _shiftUp;
        lblPassword.Top += _shiftUp;
        txtPassword.Top += _shiftUp;
        btnLogin.Text = "Login";
        lnkChangePassword.Text = "change pw";
      }
    }
  }
}
