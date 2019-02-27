using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CCI.DesktopClient.Common;
using CCI.Common;
using CCI.Sys.Data;
using TAGBOSS.Common.Model;

namespace CCI.DesktopClient.Screens
{
  public partial class frmNetworkInventory : ScreenBase
  {
    #region private method data
    private const string CUSTOMERID = "customer";
    private const string NETINVID = "CityCareNetInvID";
    private DataSource _dSource = null;
    string[] _hiddenColumns = new string[] { "netinv_id", "user_customerid", "location_id", "productid", "vendorid", "prodclassid", "prodsubclassid" };
    private DataSource _dataSource { get { if (_dSource == null) _dSource = new DataSource(); return _dSource; } }
    private string _carrier = "CityHosted";
    #endregion

    #region public properties
    #endregion

    public frmNetworkInventory()
    {
      InitializeComponent();
      Init();
    }

    #region public methods
    public void Init()
    {
      srchNetworkInventory.Init(CommonData.UnmatchedNameTypes.SearchNetworkInventory, null);
      srchPhysicalInventory.Init("Physical Inventory", null);
      ctlCustomerSearch.BringToFront();
      ctlLocationSearch.BringToFront();
      txtNewCustomer.BringToFront();
      txtNewLocation.BringToFront();
      txtPrimaryCarrier.BringToFront();
    }
    public new void Save()
    {
      saveNewFields();
      srchNetworkInventory.ReLoad();
      MessageBox.Show("Record Saved");
    }
    #endregion

    #region private methods
    #region main form methods
    private void loadByOrder(string orderid)
    {
        loadNewInventoryByOrder(orderid);
    }
    private void loadNewInventoryByOrder(string orderid)
    {
      int netinvid = _dataSource.getNewNetworkInventoryFromOrderID(orderid);
      if (netinvid == -1) // does not exist
      {
        long  id = addToNewDatabaseFromOrder(orderid);
        if (id > 0)
          loadNewFields(id.ToString());
      }
      else
        loadNewFields(netinvid.ToString());
    }
    private void search()
    {
      clearNewFields();
      srchNetworkInventory.Clear();
      string customer = ctlCustomerSearch.Text;
      string location = ctlLocationSearch.Text;
      string orderid = txtOrderSearch.Text;
      // Populate new fields from master customer project
      txtAgent.Text = _dataSource.getAgentForCustomer(customer);
      EntityAttributesCollection eac = _dataSource.getEntity(customer, "Customer", DateTime.Now);
      string entityOwner = CommonFunctions.CString(eac.getValue(customer + ".EntityOwner"));
      txtMasterCustomer.Text = CommonFunctions.CString(_dataSource.getEntity(entityOwner, "MasterCustomer", DateTime.Now).getValue(entityOwner + ".LegalName"));
      string day2YN = CommonFunctions.CString(eac.getValue(customer + ".Entity.Customer.Day2YN")); //  Entities[0].ItemTypes["Entity"].Items["Customer"].Attributes["Day2YN"].
      txtDay2YN.Text = day2YN.Equals("Y", StringComparison.CurrentCultureIgnoreCase) ? "Yes" : "No";
      // end master customer fields
      Dictionary<string, string[]> criteriaNew = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
      if (ckActiveInventoryOnly.Checked)
        criteriaNew.Add("Active", new string[] { ctlSearchGrid.opEQUALS, "Yes" });
      if (!string.IsNullOrEmpty(orderid)) // if there is an order id, we use that
      {
        int citycareorderid = _dataSource.getCityCareOrderFromProdOrderID(orderid);
        if (citycareorderid > 0) // we have a "master" order
        {
          // so we display ALL prodorders for this order
          criteriaNew.Add("CityCareOrder", new string[] { ctlSearchGrid.opEQUALS, citycareorderid.ToString() });
          
          clearNewFields();
        }
        else
        {
          // nope, no master order so we display just the one that matches this prod order
          criteriaNew.Add("OrderID", new string[] { ctlSearchGrid.opEQUALS, orderid });
          clearNewFields();
        }
        loadByOrder(orderid);
      }        
      else
      {
        if (!string.IsNullOrEmpty(location)) // we have a location, so use that
        {
          criteriaNew.Add("Location", new string[] { ctlSearchGrid.opEQUALS, location });
        }
        else
        {
          if (!string.IsNullOrEmpty(customer)) // if there is no location or order, but there is a customer, we use that
          {
            criteriaNew.Add("Customer", new string[] { ctlSearchGrid.opEQUALS, customer });
          }
        }
      }
      srchNetworkInventory.SearchCriteria = criteriaNew;
      if (criteriaNew.Count > 0)
        srchNetworkInventory.ReLoad();

    }
    //private Dictionary<string, string[]> refreshSearchCriteria(string customerID, string netinv_id)
    //{
    //  Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
    //  if (!string.IsNullOrEmpty(srchCityCareNetworkInventory.SelectedID))
    //  {
    //    //string[] commCondition = new string[] { ctlSearchGrid.opEQUALS, srchCityCareNetworkInventory.SelectedID };
    //    //customerID = (CommonFunctions.CInt(customerID) + 20000).ToString();
    //    string[] custCondition = new string[] { ctlSearchGrid.opEQUALS, customerID };
    //    string[] idCondition = new string[] { ctlSearchGrid.opEQUALS, netinv_id };
    //    if (criteria.ContainsKey(CUSTOMERID))
    //      criteria[CUSTOMERID] = custCondition;
    //    else
    //      criteria.Add(CUSTOMERID, custCondition);
    //    if (criteria.ContainsKey(NETINVID))
    //      criteria[NETINVID] = idCondition;
    //    else
    //      criteria.Add(NETINVID, idCondition);
    //  }
    //  return criteria;
    //}

    #endregion

    #region new inventory methods
    private void loadNewFields(string newnetinvid)
    {
      DataSet ds = _dataSource.getNewNetworkInventory(newnetinvid);
      if (ds == null)
        return;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
      {
        DataRow row = ds.Tables[0].Rows[0];
        txtNewNetInvID.Text = newnetinvid;
        txtNewOrderID.Text = CommonFunctions.CString(row["orderid"]);
        if (string.IsNullOrEmpty(txtNewOrderID.Text))
          txtNewOrderID.Text = CommonFunctions.CString(row["CityCareProdOrderID"]);
        string customerid = CommonFunctions.CString(row["Customer"]);
        string saddlebackid = getSaddleBackID(customerid);
        txtNewCustomer.Text = customerid;
        ((SearchDataSourceEntity)txtNewLocation.SearchExec).EntityOwner = customerid;
        txtNewLocation.Text = CommonFunctions.CString(row["Location"]);
        string itemid = CommonFunctions.CString(row["ItemID"]);
        string carrier = CommonFunctions.CString(row["Carrier"]);
        string primaryCarrier = CommonFunctions.CString(row["PrimaryCarrier"]);
        txtPrimaryCarrier.Text = primaryCarrier;
        ((SearchDataSourceProductList)txtNewItemID.SearchExec).Carrier = carrier;
        ((SearchDataSourceProductList)txtNewItemID.SearchExec).PrimaryCarrier = primaryCarrier;
        txtNewItemID.Text = itemid;
        txtTransactionQuantity.Text = Math.Round(CommonFunctions.CDecimal(row["TransactionQuantity"]), 2).ToString();
        txtQuantity.Text = Math.Round(CommonFunctions.CDecimal(row["Quantity"]),2).ToString();
        txtNewAccount.Text = saddlebackid;//CommonFunctions.CString(row["vendoraccountid"]);
        txtTransactionMRC.Text = Math.Round(CommonFunctions.CDecimal(row["TransactionMRC"]), 2).ToString();
        txtNewMRC.Text = Math.Round(CommonFunctions.CDecimal(row["mrc"]),2).ToString();
        txtNewNRC.Text = Math.Round(CommonFunctions.CDecimal(row["nrc"]),2).ToString();
        DateTime? dateInstalled = row["installdate"] == System.DBNull.Value ? null : (DateTime?)row["installdate"];
        DateTime? transactionDate = row["TransactionDate"] == System.DBNull.Value ? null : (DateTime?)row["TransactionDate"];
        DateTime? startDate = row["startdate"] == System.DBNull.Value ? null : (DateTime?)row["startdate"];
        DateTime? endDate = row["enddate"] == System.DBNull.Value ? null : (DateTime?)row["enddate"];
        DateTime? expDate = row["expirationdate"] == System.DBNull.Value ? null : (DateTime?)row["expirationdate"];
        dtNewDateInstalled.Text = dateInstalled == null ? null : ((DateTime)dateInstalled).ToShortDateString();
        dtTransactionDate.Text = transactionDate == null ? null : ((DateTime)transactionDate).ToShortDateString();
        dtNewStartDate.Text = startDate == null ? null : ((DateTime)startDate).ToShortDateString();
        dtNewEndDate.Text = endDate == null ? null : ((DateTime)endDate).ToShortDateString();
        dtNewExpDate.Text = expDate == null ? null : ((DateTime)expDate).ToShortDateString();
        bool isActiveByDate = CommonFunctions.CDateTime(row["startdate"], CommonData.PastDateTime) <= DateTime.Today && DateTime.Today <= CommonFunctions.CDateTime(row["enddate"], CommonData.FutureDateTime);
        //ckNewActive.Checked = System.DBNull.Value == row["inactive"] && isActiveByDate ? true : false;
        txtLastModifiedBy.Text = CommonFunctions.CString(row["LastModifiedBy"]);
        // Load Physical Inventory for this item
        Dictionary<string, string[]> criteriaNew = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
        criteriaNew.Add("NetworkInventoryID", new string[] { ctlSearchGrid.opEQUALS, newnetinvid.ToString() });
        srchPhysicalInventory.SearchCriteria = criteriaNew;
        srchPhysicalInventory.ReLoad();
        lblNewNew.Visible = false;
      }
      ds.Clear();
      ds = null;
    }
    private void reloadNewProductCombo()
    {
      string itemid = txtNewItemID.Text;
      string primaryCarrier = txtPrimaryCarrier.Text;
      ((SearchDataSourceProductList)txtNewItemID.SearchExec).Carrier = _carrier;
      ((SearchDataSourceProductList)txtNewItemID.SearchExec).PrimaryCarrier = primaryCarrier;
      txtNewItemID.Text = itemid;
    }
    private void clearNewFields()
    {
      txtNewNetInvID.Text = string.Empty;
      txtNewOrderID.Text = string.Empty;
      // default to search value if any
      if (!string.IsNullOrEmpty(ctlCustomerSearch.Text))
        txtNewCustomer.Text = ctlCustomerSearch.Text;
      else
        txtNewCustomer.Text = null;
      if (!string.IsNullOrEmpty(ctlLocationSearch.Text))
        txtNewLocation.Text = ctlLocationSearch.Text;
      else
        txtNewLocation.Text = null;
      txtNewItemID.Text = null;
      txtTransactionMRC.Text = string.Empty;
      txtNewMRC.Text = string.Empty;
      txtNewNRC.Text = string.Empty;
      txtNewAccount.Text = string.Empty;
      txtTransactionQuantity.Text = string.Empty;
      txtQuantity.Text = "1";
      dtNewDateInstalled.Text = null;
      dtNewEndDate.Text = null;
      dtTransactionDate.Text = null;
      dtNewStartDate.Text = null;
      dtNewExpDate.Text = null;
      lblNewNew.Visible = true;
      clearPhysicalInventory();
    }
    private void saveNewFields()
    {
      string netinv_id = txtNewNetInvID.Text;
      string customer = txtNewCustomer.Text;
      string location = txtNewLocation.Text;
      string itemid = txtNewItemID.Text;
      string vendor = string.Empty;
      decimal mrc = CommonFunctions.CDecimal(txtNewMRC.Text);
      decimal transactionMRC = CommonFunctions.CDecimal(txtTransactionMRC.Text);
      decimal nrc = CommonFunctions.CDecimal(txtNewNRC.Text);
      int transactionQuantity = CommonFunctions.CInt(txtTransactionQuantity.Text);
      int quantity = CommonFunctions.CInt(txtQuantity.Text);
      DateTime? endDate = dtNewEndDate.Value;
      DateTime? startDate = dtNewStartDate.Value;
      DateTime? transactionDate = dtTransactionDate.Value;
      DateTime? installdate = dtNewDateInstalled.Value;
      DateTime? expirationdate = dtNewExpDate.Value;
      int orderid = CommonFunctions.CInt(txtNewOrderID.Text);
      string account = txtNewAccount.Text;
      bool active = startDate <= DateTime.Today && DateTime.Today <= endDate;
      int? id = _dataSource.updateNewNetworkInventory(netinv_id, customer, location, orderid, orderid, _carrier, itemid, string.Empty, account,
        mrc, nrc, null, string.Empty, string.Empty, installdate, startDate, endDate, string.Empty, string.Empty, quantity, active, vendor, expirationdate,
        transactionDate, transactionQuantity, transactionMRC);
      if (id != null && id > 0)
        txtNewNetInvID.Text = id.ToString();
      savePhysicalInventory();
    }
    private void savePhysicalInventory()
    {
      if (!string.IsNullOrEmpty(txtMacAddress.Text) && !string.IsNullOrEmpty(txtPhysicalInventoryNotes.Text))
      {
        int? id = _dataSource.savePhysicalInventory(txtPhysicalInventoryID.Text,  txtNewNetInvID.Text, txtMacAddress.Text, txtPhysicalInventoryNotes.Text, SecurityContext.User);
        if (id != null && string.IsNullOrEmpty(txtPhysicalInventoryID.Text))
          txtPhysicalInventoryID.Text = ((int)id).ToString();  // set the ID in the screen if we need one and have one
        srchPhysicalInventory.ReLoad();
      }
    }
    private void quickEntrySavePhysicalInventory()
    {
      if (!string.IsNullOrEmpty(txtQEMacAddress.Text) && !string.IsNullOrEmpty(txtQENotes.Text) && !string.IsNullOrEmpty(txtQEUsoc.Text))
      {
        string customerid = txtNewCustomer.Text;
        string location = txtNewLocation.Text;
        string usoc = txtQEUsoc.Text;
        int? netInvId = _dataSource.getNetworkInventoryFromPhysicalInventory(customerid, usoc, location, DateTime.Today);
        if (netInvId == null)
          MessageBox.Show("Cannot find NetworkInventory for this customer, location, and usoc");
        {
          int? id = _dataSource.savePhysicalInventory(null, ((int)netInvId).ToString(), txtQEMacAddress.Text, txtQENotes.Text, SecurityContext.User);
          srchPhysicalInventory.ReLoad();
          clearQuickEntry();
          txtQEUsoc.Focus();
        }
      }
    }
    private void clearPhysicalInventory()
    {
      txtPhysicalInventoryID.Text = string.Empty;
      txtMacAddress.Text = string.Empty;
      txtPhysicalInventoryNotes.Text = string.Empty;
      if (string.IsNullOrEmpty(txtNewNetInvID.Text))
        srchPhysicalInventory.Clear();
      else
        srchPhysicalInventory.ReLoad();
    }
    private long addToNewDatabaseFromOrder(string orderid)
    {
      long retVal = -1;
      if (string.IsNullOrEmpty(orderid))
        MessageBox.Show("You must have a valid orderid");
      else
      {
        if (_dataSource.existsNewCustomerIDFromOldOrder(orderid))
        {
          List<string> idList = new List<string>();
          idList.Add(orderid);
          retVal = _dataSource.AddNetworkInventoryFromCityCareData("Orders", idList, SecurityContext.User, string.Empty);
        }
        else
          MessageBox.Show("This customer has not yet been imported from the CityCare database. You must do that before you can create the NetworkInvetory from it");
      }
      return retVal;
    }
    private string getSaddleBackID(string customerid)
    {
      return CommonFunctions.CString(_dataSource.getEntityFieldValue(customerid, "AlternateID"));
    }
    #endregion
    #endregion

    #region form events
    #region main form events
    private void frmCityCareNetworkInventory_Load(object sender, EventArgs e)
    {
      ctlCustomerSearch.SearchExec = new SearchDataSourceEntity("Customer");
      ctlLocationSearch.SearchExec = new SearchDataSourceEntity("Location");
      txtNewCustomer.SearchExec = new SearchDataSourceEntity("Customer");
      txtNewLocation.SearchExec = new SearchDataSourceEntity("Location");
      txtPrimaryCarrier.SearchExec = new SearchDataSourceEntity("Carrier");
      txtNewItemID.SearchExec = new SearchDataSourceProductList();
    }
    private void txtCustomerSearch_Leave(object sender, EventArgs e)
    {
      ctlLocationSearch.Text = string.Empty;
      txtOrderSearch.Text = string.Empty;
      if (string.IsNullOrEmpty(ctlCustomerSearch.Text))
        ((SearchDataSourceEntity)ctlLocationSearch.SearchExec).EntityOwner = string.Empty;
      else
      {
        ((SearchDataSourceEntity)ctlLocationSearch.SearchExec).EntityOwner = ctlCustomerSearch.Text;
        search();
        txtNewAccount.Text = getSaddleBackID(txtNewCustomer.Text);
        txtDealer.Text = _dataSource.getDealerForCustomer(txtNewCustomer.Text);

      }

    }
    private void btnSearch_Click(object sender, EventArgs e)
    {
      search();
    }
    private void txtOrderSearch_Leave(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(txtOrderSearch.Text))
      {
        ctlCustomerSearch.Text = string.Empty;
        ctlLocationSearch.Text = string.Empty;
        search();
      }
    }
    private void ctlLocationSearch_Leave(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(ctlLocationSearch.Text))
      {
        txtOrderSearch.Text = string.Empty;
        search();
      }
    }
    private void txtNewItemID_Enter(object sender, EventArgs e)
    {
      txtNewItemID.Text = string.Empty;
      reloadNewProductCombo();
    }
    private void ctlCustomerSearch_Enter(object sender, EventArgs e)
    {
      ctlCustomerSearch.Text = string.Empty;
    }
    private void ctlLocationSearch_Enter(object sender, EventArgs e)
    {
      ctlLocationSearch.Text = string.Empty;
    }
    private void txtNewCustomer_Enter(object sender, EventArgs e)
    {
      txtNewCustomer.Text = string.Empty;
    }
    private void txtNewLocation_Enter(object sender, EventArgs e)
    {
      txtNewLocation.Text = string.Empty;
    }
    private void ckActiveInventoryOnly_CheckedChanged(object sender, EventArgs e)
    {
      //srchNetworkInventory.ReLoad();
      search();
    }
    #endregion
    #region new inventory events
    private void srchNetworkInventory_RowSelected(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      if (e.SelectedRow != null)
      {
        string netinvid = CommonFunctions.CString(e.SelectedRow.Cells["ID"].Value);
        loadNewFields(netinvid);
      }
    }

    private void btnNewSave_Click(object sender, EventArgs e)
    {
      Save();
    }
    private void btnNewNew_Click(object sender, EventArgs e)
    {
      clearNewFields();
    }
    private void btnNewDelete_Click(object sender, EventArgs e)
    {
      _dataSource.deleteNewNetworkInventory(txtNewNetInvID.Text);
      clearNewFields();
      srchNetworkInventory.ReLoad();
      MessageBox.Show("Record deleted");
    }
    private void txtNewCustomer_OnSelected(object sender, EventArgs e)
    {
      string custid = txtNewCustomer.Text;
      if (!string.IsNullOrEmpty(custid))
        ((SearchDataSourceEntity)txtNewLocation.SearchExec).EntityOwner = custid;
    }
    private void btnClone_Click(object sender, EventArgs e)
    {
      txtNewNetInvID.Text = string.Empty;
      lblNewNew.Visible = true;
    }
    /// <summary>
    /// If we are editing a record, we reload from the db, cancelling all user input. 
    /// If we are adding a new record, then clear the fields instead
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnNewCancel_Click(object sender, EventArgs e)
    {
      if (!lblNewNew.Visible && !string.IsNullOrEmpty(txtNewNetInvID.Text))
        loadNewFields(txtNewNetInvID.Text);
      else
        clearNewFields();
    }


    #endregion

    private void txtNewCustomer_Leave(object sender, EventArgs e)
    {
      txtNewAccount.Text = getSaddleBackID(txtNewCustomer.Text);
    }
    private void srchPhysicalInventory_RowSelected(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      if (e.SelectedRow != null)
      {
        string id = CommonFunctions.CString(e.SelectedRow.Cells["ID"].Value);
        string macAddress = CommonFunctions.CString(e.SelectedRow.Cells["MacAddress"].Value);
        string Notes = CommonFunctions.CString(e.SelectedRow.Cells["MacAddress"].Value);
        txtMacAddress.Text = macAddress;
        txtPhysicalInventoryNotes.Text = Notes;
        txtPhysicalInventoryID.Text = id;
      }
    }
    private void btnSavePhysicalInventory_Click(object sender, EventArgs e)
    {
      savePhysicalInventory();
    }



    #endregion

    private void txtPrimaryCarrier_Leave(object sender, EventArgs e)
    {
      ((SearchDataSourceProductList)txtNewItemID.SearchExec).PrimaryCarrier = txtPrimaryCarrier.Text;
      //((SearchDataSourceProductList)txtNewItemID.SearchExec).Carrier = txtPrimaryCarrier.Text;

    }

    private void btnNewPhysicalInventory_Click(object sender, EventArgs e)
    {
      // clear out the fields to enter a new one
      txtPhysicalInventoryID.Text = string.Empty;
      txtMacAddress.Text = string.Empty;
      txtPhysicalInventoryNotes.Text = string.Empty;
    }

    private void btnQuickEntryPhysicalInventory_Click(object sender, EventArgs e)
    {
      if (!string.IsNullOrEmpty(txtNewCustomer.Text) && !string.IsNullOrEmpty(txtNewLocation.Text))
      {
        txtQEUsoc.Items.Clear();
        txtQEUsoc.Items.AddRange(_dataSource.getUsocsForNetworkInventory(txtNewCustomer.Text, txtNewLocation.Text));
        swapQuickEntry(true);
      }
      else
        MessageBox.Show("You cannot enter Quick Entry mode unless you have a customer and location selected.");
    }

    private void btnQENormalEntry_Click(object sender, EventArgs e)
    {
      swapQuickEntry(false);
    }
    private void swapQuickEntry(bool qeMode)
    {
      lblPhysicalInventoryID.Visible = !qeMode;
      lblMacAddress.Visible = !qeMode;
      lblNotes.Visible = !qeMode;
      txtPhysicalInventoryNotes.Visible = !qeMode;
      txtMacAddress.Visible = !qeMode;
      txtPhysicalInventoryID.Visible = !qeMode;
      btnSavePhysicalInventory.Visible = !qeMode;
      btnNewPhysicalInventory.Visible = !qeMode;
      btnDeletehysicalInventory.Visible = !qeMode;
      btnQuickEntryPhysicalInventory.Visible = !qeMode;
      lblQEUsoc.Visible = qeMode;
      txtQEUsoc.Visible = qeMode;
      lblQEMacAddress.Visible = qeMode;
      txtQEMacAddress.Visible = qeMode;
      lblQENotes.Visible = qeMode;
      txtQENotes.Visible = qeMode;
      btnQENormalEntry.Visible = qeMode;
    }
    private void clearQuickEntry()
    {
      txtQEUsoc.Text = string.Empty;
      txtQEMacAddress.Text = string.Empty;
      txtQENotes.Text = string.Empty;
    }

    private void txtQENotes_Leave(object sender, EventArgs e)
    {
      quickEntrySavePhysicalInventory();
    }
  }
}
