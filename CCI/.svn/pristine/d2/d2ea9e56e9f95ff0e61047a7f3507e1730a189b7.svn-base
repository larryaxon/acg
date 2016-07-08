using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using CCI.Sys.Data;
using CCI.DesktopClient.Common;


namespace CCI.DesktopClient.Screens
{
  public partial class frmUserMaintenance : ScreenBase
  {
    DataSource _dataSource = new DataSource();
    UserInfo _info = new UserInfo();
    string[] _userTypes = new string[] { "Dealer", "Agent", "Customer", "User" };

    public frmUserMaintenance()
    {
      InitializeComponent();
      txtUserType.Items.AddRange(_userTypes);
      ctlSearch1.EntityType = "User";
    }

    #region form events
    private void btnSearch_Click(object sender, EventArgs e)
    {
      getFieldData(ctlSearch1.Text);
    }
    private void btnNew_Click(object sender, EventArgs e)
    {
      txtUserType.Text = "Dealer";
      txtLogin.Text = string.Empty;
      txtEmail.Text = string.Empty;
      txtFirstName.Text = string.Empty;
      txtLastName.Text = string.Empty;
    }
    private void ctlSearch1_OnSelected(object sender, EventArgs e)
    {
      getFieldData(ctlSearch1.Text);
    }
    private void btnSave_Click(object sender, EventArgs e)
    {
      saveFieldData();
    }
    #endregion

    #region private methods
    private void getFieldData(string entity)
    {
      _info = _dataSource.getUserInfo(entity, DateTime.Today);
      txtLogin.Text = _info.Login;
      txtPassword.Text = _info.Password;
      txtFirstName.Text = _info.FirstName;
      txtLastName.Text = _info.LastName;
      txtUserType.Text = _info.UserType;
      txtEmail.Text = _info.Email;
    }
    private void saveFieldData()
    {
      string entityFormat = "{0}{1}-{2}";
      _info = new UserInfo();
      _info.Login = txtLogin.Text;
      _info.Domain = "citycare.com";
      _info.Password = txtPassword.Text;
      _info.EntityOwner = "CCI";
      _info.FirstName = txtFirstName.Text;
      _info.LastName = txtLastName.Text;
      if (string.IsNullOrEmpty(_info.Login) || string.IsNullOrEmpty(_info.LastName))
      {
        MessageBox.Show("Both Login and Last Name are required");
        return;
      }
      _info.OldID = null;
      _info.UserType = txtUserType.Text;
      _info.SecurityGroup = _info.UserType;
      _info.Entity = string.Format(entityFormat, _info.LastName, string.Empty, _info.UserType);
      _info.Email = txtEmail.Text;
      _dataSource.UpdateUserInfo(_info);
    }
    #endregion

    private void txtUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (txtUserType.Text.Equals("Dealer"))
      {
        txtDealer.Visible = true;
        lblDealer.Visible = true;
      }
      else
      {
        txtDealer.Visible = false;
        lblDealer.Visible = false;
      }
    }

    private void frmUserMaintenance_Load(object sender, EventArgs e)
    {
      SearchResultCollection dealers = _dataSource.SearchEntities("*", "Dealer");
      foreach (SearchResult dealer in dealers)
      {
        txtDealer.Items.Add(dealer);
      }
      txtDealer.ValueMember = "EntityID";
      txtDealer.DisplayMember = "FullName";
    }

  }
}
