using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ACG.Common;
using CCI.Sys.Data;

namespace CCI.DesktopClient.Screens
{
  public partial class frmMasterProductList : ScreenBase
  {
    #region method data
    string[] fieldNames = new string[] {"ItemID"
      ,"Name"
      ,"ItemSubCategory"
      ,"MasterItemID"
      ,"IsCityHostedRetail"
      ,"IsCityHostedWholesale"
      ,"IsSaddlebackUSOC"
      ,"ExternalName"
      ,"IsSaddlebackVariable"
      ,"ExternalCategory" };
    string _selectedItem = string.Empty;
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    #endregion
    public frmMasterProductList()
    {
      InitializeComponent();
    }

    #region form events
    private void frmMasterProductList_Load(object sender, EventArgs e)
    {
      srchItem.SearchExec = new SearchDataSourceItem();
      srchMasterItem.SearchExec = new SearchDataSourceItem();
    }
    private void srchItem_OnSelected(object sender, EventArgs e)
    {
      _selectedItem = srchItem.ID;
      loadFields(_selectedItem);
    }
    private void btnSave_Click(object sender, EventArgs e)
    {
      string[] fieldValues = new string[] { txtItemID.Text, txtName.Text, txtItemSubCategory.Text, srchMasterItem.Text, ckIsCityHostedRetail.Checked ? "1" : "0",
      ckIsCityHostedWholesale.Checked ? "1" : "0", ckIsSaddlebackUSOC.Checked ? "1" : "0", txtExternalName.Text, "0", txtExternalCategory.Text};
      _dataSource.updateMasterProduct(fieldNames, fieldValues);
      MessageBox.Show("Record Saved");
    }
    private void btnAddNew_Click(object sender, EventArgs e)
    {
      clearFields();
      srchItem.Text = string.Empty;
    }
    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(txtItemID.Text))
        MessageBox.Show("No Item has been selected to delete.");
      else
      {
        _dataSource.deleteMasterProduct(txtItemID.Text);
        clearFields();
        MessageBox.Show("Record Deleted");
      }
    }
    #endregion

    #region private methods
    private void loadFields(string itemid)
    {
      clearFields();
      txtItemID.Text = itemid;
      txtName.Text = srchItem.Description;          
      DataSet ds = _dataSource.getMasterProduct(itemid);
      if (ds == null)
        return;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
      {
        DataRow row = ds.Tables[0].Rows[0];
        txtItemSubCategory.Text = CommonFunctions.CString(row["ItemSubCategory"]);
        txtExternalCategory.Text = CommonFunctions.CString(row["ExternalCategory"]);
        txtExternalName.Text = CommonFunctions.CString(row["ExternalName"]);
        ckIsCityHostedRetail.Checked = CommonFunctions.CBoolean(row["IsCityHostedRetail"]);
        ckIsCityHostedWholesale.Checked = CommonFunctions.CBoolean(row["IsCityHostedWholesale"]);
        ckIsSaddlebackUSOC.Checked = CommonFunctions.CBoolean(row["IsSaddlebackUSOC"]);
        srchMasterItem.Text = CommonFunctions.CString(row["MasterItemID"]);
      }
    }
    private void clearFields()
    {
      txtItemID.Text = string.Empty;
      txtName.Text = string.Empty;
      txtItemSubCategory.Text = string.Empty;
      txtExternalCategory.Text = string.Empty;
      txtExternalName.Text = string.Empty;
      ckIsCityHostedRetail.Checked = false;
      ckIsCityHostedWholesale.Checked = false;
      ckIsSaddlebackUSOC.Checked = false;
      srchMasterItem.Text = string.Empty;
    }
    private void loadCombos()
    {
      txtItemSubCategory.Items.Clear();
      string[] categories = _dataSource.getItemSubCategories(null);
      txtItemSubCategory.Items.AddRange(categories);
    }
    #endregion
  }
}
