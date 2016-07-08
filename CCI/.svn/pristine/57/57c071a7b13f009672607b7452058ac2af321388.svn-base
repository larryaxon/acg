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
  public partial class frmProductList : ScreenBase
  {
    #region method data
    private string[] fieldNames = new string[] {"Carrier"
      ,"ItemID"
      ,"StartDate"
      ,"EndDate"
      ,"LastModifiedBy"
      ,"LastModifiedDateTime"
      ,"MRC"
      ,"NRC" };
    private string _selectedItem = string.Empty;
    private string _selectedCarrier = string.Empty;
    private ISearchDataSource _masterItemSearch = new SearchDataSourceItem();
    private ISearchDataSource _carrierItemSearch = new SearchDataSourceProductList();
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    private bool _addNewMode { get { return lblAddNewMessage.Visible; } }
    #endregion
    public frmProductList()
    {
      InitializeComponent();
    }
    #region form events
    private void frmProductList_Load(object sender, EventArgs e)
    {
      srchCarrier.SearchExec = new SearchDataSourceCarrier();
      setNewMode(false);
    }
    private void srchItem_OnSelected(object sender, EventArgs e)
    {
      _selectedItem = srchItem.ID;
      if (!_addNewMode)
        loadFields(_selectedItem, _selectedCarrier);
    }
    private void btnSave_Click(object sender, EventArgs e)
    {
      string[] fieldValues = new string[] { srchCarrier.Text, srchItem.Text, dtStartDate.Value.ToShortDateString(), dtEndDate.Value.ToShortDateString(), 
        SecurityContext.User, DateTime.Now.ToString(CommonData.FORMATLONGDATETIME), 
        CommonFunctions.CString(CommonFunctions.CDecimal(txtMRC.Text)),
        CommonFunctions.CString(CommonFunctions.CDecimal(txtNRC.Text))};
      _dataSource.updateProduct(fieldNames, fieldValues);
      setNewMode(false);
      MessageBox.Show("Record Saved");
    }
    private void btnAddNew_Click(object sender, EventArgs e)
    {
      clearFields(true);
      setNewMode(true);
    }
    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(txtNRC.Text))
        MessageBox.Show("No Item has been selected to delete.");
      else
      {
        _dataSource.deleteProduct(_selectedCarrier, _selectedItem);
        clearFields(true);
        setNewMode(false);
        MessageBox.Show("Record Deleted");
      }
    }
    private void srchCarrier_OnSelected(object sender, EventArgs e)
    {
      if (!_addNewMode)
        ((SearchDataSourceProductList)srchItem.SearchExec).Carrier = srchCarrier.Text;
      _selectedCarrier = srchCarrier.ID;
    }
    #endregion

    #region private methods
    private void loadFields(string itemid, string carrier)
    {
      if (string.IsNullOrEmpty(carrier))
        MessageBox.Show("You must have a Carrier selected");
      else
      {
        clearFields(true);
        DataSet ds = _dataSource.getProduct(itemid, carrier);
        if (ds == null)
          return;
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
          DataRow row = ds.Tables[0].Rows[0];
          srchCarrier.Text = CommonFunctions.CString(row["Carrier"]);
          srchItem.Text = CommonFunctions.CString(row["ItemID"]);
          txtMRC.Text = CommonFunctions.CString(row["MRC"]);
          txtNRC.Text = CommonFunctions.CString(row["NRC"]);
          txtLastModifiedBy.Text = CommonFunctions.CString(row["LastModifiedBy"]);
          txtLastModifiedDateTime.Text = CommonFunctions.CString(row["LastModifiedDateTime"]);
          dtStartDate.Value = CommonFunctions.CDateTime(row["StartDate"]);
          dtEndDate.Value = CommonFunctions.CDateTime(row["EndDate"], CommonData.FutureDateTime);
        }
      }
    }
    private void clearFields(bool clearSearch)
    {
      if (clearSearch)
      {
        srchItem.Text = string.Empty;
        srchCarrier.Text = string.Empty;
      }
      txtMRC.Text = string.Empty;
      txtNRC.Text = string.Empty;
      dtStartDate.Value = DateTime.Today;
      dtEndDate.Value = new DateTime(2100, 12, 31);
      txtLastModifiedBy.Text = string.Empty;
      txtLastModifiedDateTime.Text = string.Empty;
      lblAddNewMessage.Visible = false;
    }
    private void loadCombos()
    {

    }
    private void setNewMode(bool isNew)
    {
      lblAddNewMessage.Visible = isNew;
      if (isNew)
        srchItem.SearchExec = _masterItemSearch;
      else
        srchItem.SearchExec = _carrierItemSearch;
    }
    #endregion

  }
}
