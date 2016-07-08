using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using TAGBOSS.Common;
using TAGBOSS.Common.Model;

namespace CCI.DesktopClient.Common
{
  public partial class ctlNetworkInventory : ctlMaintenanceBase
  {
    private const string colCARRIER = "Carrier";
    private const string colPAYOR = "Payor";
    private const string colITEM = "ItemID";
    private const string colCYCLECODE = "CycleCode";
    private const string colNEEDSREVIEW = "NeedsReview";
    private const string colQUANTITY = "Quantity";
    private const string valNOTYETREVIEWED = "NOTYETREVIEWED";
    private string _customerID = null;
    private string _locationID = null;
    private string _itemID = null;
    private SearchResultCollection _customerList = new SearchResultCollection();
    private DataGridViewComboBoxColumn _customer = new DataGridViewComboBoxColumn();
    private SearchResultCollection _payorList = new SearchResultCollection();
    private DataGridViewComboBoxColumn _payor = new DataGridViewComboBoxColumn();
    private DataGridViewComboBoxColumn _carrier = new DataGridViewComboBoxColumn();
    private SearchResultCollection _carrierList = new SearchResultCollection();
    private DataGridViewComboBoxColumn _item = new DataGridViewComboBoxColumn();
    private ItemListCollection _itemList = new ItemListCollection();
    private DataGridViewComboBoxColumn _location = new DataGridViewComboBoxColumn();
    private SearchResultCollection _locationList = null;
    private PickListEntries _needsReviewList = null;
    private DataGridViewComboBoxColumn _needsReview = new DataGridViewComboBoxColumn();

    public const string colCUSTOMERID = "Customer";
    public const string colLOCATIONID = "Location";
    public DataGridViewRowCollection Rows { get { return grdMaintenance.Rows; } }
    public DataGridViewColumnCollection Columns { get { return grdMaintenance.Columns; } }
    public DataGridViewSelectedRowCollection SelectedRows { get { return grdMaintenance.SelectedRows; } }
    public bool AllowUserToOrderColumns { get { return grdMaintenance.AllowUserToOrderColumns; } set { grdMaintenance.AllowUserToOrderColumns = value; } }
    public DataGridView Grid { get { return grdMaintenance; } }
    public DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode { get { return grdMaintenance.ColumnHeadersHeightSizeMode; } set { grdMaintenance.ColumnHeadersHeightSizeMode = value; } }
    public void AddSelectionChanged(System.EventHandler newHandler)
    {
      grdMaintenance.SelectionChanged += newHandler;
    }

    public string CustomerID
    { 
      get { return _customerID; }
      set 
      { 
        _customerID = value;
        setDefaultValue(colCUSTOMERID, _customerID, null, null);
      }
    }
    public string LocationID 
    { 
      get { return _locationID; }
      set 
      { 
        _locationID = value;
        setDefaultValue(colLOCATIONID, value, _location, _locationList);
      }
    }
    public string ItemID
    {
      get { return _itemID; }
      set
      {
        _itemID = value;
        setDefaultValue(colITEM, value, _item, _itemList);
      }
    }
    public ctlNetworkInventory()
    {
      TableName = "NetworkInventory";
      UniqueIdentifier = "ID";
      ReadOnlyColumns.Add(UniqueIdentifier, null);
      ReadOnlyColumns.Add(colCYCLECODE, null);
      DefaultValues.Add(colCYCLECODE, "Standard");
      DefaultValues.Add(colNEEDSREVIEW, valNOTYETREVIEWED);
      DefaultValues.Add(colQUANTITY, 1);
      InitializeComponent();
      ShowExtendedField = false;
      this.grdMaintenance.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdMaintenance_CellValueChanged);
      this.grdMaintenance.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdMaintenance_CellFormatting);

    }
    public void Init()
    {
      Init(null);
    }
    public void Init(string whereClause)
    {
      if (string.IsNullOrEmpty(_customerID) && string.IsNullOrEmpty(whereClause))
      {
        //CommonFormFunctions.showMessage("Must have a customer selected to display network inventory");
        return;
      }
      _suspendRowValidation = true;
      Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
      parameters.Add(colCUSTOMERID, _customerID);
      parameters.Add(colLOCATIONID, _locationID);
      if (!string.IsNullOrEmpty(whereClause))
        parameters.Add("WhereClause", whereClause);
      load(parameters);
      int colIndex = 0;
      /*
       *    Location Setup
       *    
       * if this customer has no location, then disable the location column
       */
      if (!_dataSource.hasLocation(_customerID))
      {

        if (!HiddenColumns.ContainsKey(colLOCATIONID))
          HiddenColumns.Add(colLOCATIONID, null);
        // and disable the pick list
        _location = new DataGridViewComboBoxColumn();
        _locationList = null;  
      }
      else
      {
        // otherwise, if we have a location, then use that one and disable it
        if (string.IsNullOrEmpty(_locationID))  // no location
        {
          if (HiddenColumns.ContainsKey(colLOCATIONID)) // so enable the field
            HiddenColumns.Remove(colLOCATIONID);
          // now we populate the location pick list
          _locationList = _dataSource.getEntityList(_customerID, "Customer", "Location");
          _location.Items.Clear();
          _location.ValueType = typeof(SearchResult);
          _location.DisplayMember = "LegalName";
          _location.ValueMember = "EntityID";
          _location.DataPropertyName = colLOCATIONID;
          _location.Name = colLOCATIONID;
          _location.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
          _location.Resizable = DataGridViewTriState.True;
          foreach (SearchResult loc in _locationList)
            _location.Items.Add(loc);
          if (grdMaintenance.Columns.Contains(colLOCATIONID))
          {
            colIndex = grdMaintenance.Columns[colLOCATIONID].Index;
            grdMaintenance.Columns.Remove(colLOCATIONID);
          }
          grdMaintenance.Columns.Insert(colIndex,_location);
        }
        else
        {
          if (!HiddenColumns.ContainsKey(colLOCATIONID)) // we have one, so we disable the field
            HiddenColumns.Add(colLOCATIONID, null);
          // and disable the pick list
          _location = new DataGridViewComboBoxColumn();
          _location.ValueType = typeof(SearchResult);
          _locationList = new SearchResultCollection();
        }
      }
      // now add the location combo to the pick lists if there it has stuff, otherwise remove it 
      if (_locationList != null &&_locationList.Count > 0)
        if (PickLists.ContainsKey(colLOCATIONID))
          PickLists[colLOCATIONID] = _location;
        else
          PickLists.Add(colLOCATIONID, _location);
      else
        if (PickLists.ContainsKey(colLOCATIONID))
          PickLists.Remove(colLOCATIONID);
      /*
       * End Location
       */
      // customer
      if (_customerList.Count == 0)
        _customerList = _dataSource.SearchEntities("*", "Customer");
      _customer = loadColumn(_customer, colCUSTOMERID, _customerList);
      // payor
      if (_payorList.Count == 0)
        _payorList = _dataSource.SearchEntities("*", "Payor");
      _payor = loadColumn(_payor, colPAYOR, _payorList);

      // carrier
      if (_carrierList.Count == 0)
        _carrierList = _dataSource.SearchEntities("*", "Carrier");
      _carrier = loadColumn(_carrier, colCARRIER, _carrierList);
      // Items
      _item = loadColumn(_item, colITEM, _itemList, true);
      //Needs Review
      if (_needsReviewList == null)
        _needsReviewList = _dataSource.getCodeListDescriptions("REVIEWCODE");
      _needsReview = loadColumn(_needsReview, colNEEDSREVIEW, _needsReviewList);
      grdMaintenance.AllowUserToResizeColumns = true;
      grdMaintenance.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
      _suspendRowValidation = false;

    }

    public new void load(Dictionary<string, string> parameters)
    {
      if (HiddenColumns.ContainsKey(colCUSTOMERID))
        HiddenColumns.Remove(colCUSTOMERID);
      base.load(parameters);

      foreach (DataGridViewRow row in grdMaintenance.Rows)
      {
        if (row.Cells[colITEM].Value != null)
        {
          DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)_item.CellTemplate.Clone();
          cell.Items.Clear();
          cell = getItems(cell, CommonFunctions.CString(row.Cells["Carrier"].Value));
          CommonFormFunctions.setComboBoxCell(cell, row.Cells[colITEM].Value);
          //cell.RowIndex = -1;
          //cell.ColumnIndex = -1;
          row.Cells[colITEM] = cell;
          ////cell.ValueType = typeof(ItemListEntry);
          //((DataGridViewComboBoxCell)row.Cells[colITEM]).Items.Clear();
          //((DataGridViewComboBoxCell)row.Cells[colITEM]).Items.AddRange(cell.Items);
          //row.Cells[colITEM] = cell;
          //cell = (DataGridViewComboBoxCell)_payor.Clone();
          //row.Cells[colPAYOR] = cell;
          //CommonFormFunctions.setComboBoxCell(cell, CommonFunctions.CString(row.Cells[colPAYOR].Value));
          //cell = (DataGridViewComboBoxCell)_location.Clone();
          //row.Cells[colLOCATIONID] = cell;
          //CommonFormFunctions.setComboBoxCell(cell, row.Cells[colLOCATIONID].Value);
          //cell = (DataGridViewComboBoxCell)_carrier.Clone();
          //row.Cells[colCARRIER] = cell;
          //CommonFormFunctions.setComboBoxCell(cell, CommonFunctions.CString(row.Cells[colCARRIER].Value));
          //cell = (DataGridViewComboBoxCell)_needsReview.Clone();
          //row.Cells[colNEEDSREVIEW] = cell;
          //CommonFormFunctions.setComboBoxCell(cell, CommonFunctions.CString(row.Cells[colNEEDSREVIEW].Value));
        }
      }
    }
    private DataGridViewComboBoxCell getItems(DataGridViewComboBoxCell cell, string carrier)
    {
      _itemList = _dataSource.getItemList(carrier);
      cell.Items.Clear();
      cell.ValueType = typeof(ItemListEntry);
      cell.DisplayMember = "DisplayText";
      cell.ValueMember = "ItemID";     
      foreach (ItemListEntry item in _itemList)
        cell.Items.Add(item);

      return cell;
    }

    public new void grdMaintenance_CellLeave(object sender, DataGridViewCellEventArgs e)
    {
      ;
    }
    public new void grdMaintenance_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex == grdMaintenance.Columns["Carrier"].Index) // did we just leave the Carrier column?
      {
        // yes, so we need to repopulate the item pick list based on the carrier
        DataGridViewRow row = grdMaintenance.Rows[e.RowIndex];
        string carrier = CommonFunctions.CString(row.Cells[e.ColumnIndex].Value);
        string itemID = CommonFunctions.CString(row.Cells[e.ColumnIndex + 1].Value);// this assumes the Item cell is the one right after carrie
        DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)_item.CellTemplate.Clone();
        cell.Items.Clear();
        cell = getItems(cell, carrier);
        cell.Value = _itemList[itemID, carrier]; // if the new combo of item/carrier doesn't exist this returns a null
        row.Cells[e.ColumnIndex + 1] = cell; // this assumes the Item cell is the one right after carrier
      }
      //base.grdMaintenance_CellLeave(sender, e);
    }
    public new void grdMaintenance_CellValidated(object sender, DataGridViewCellEventArgs e)
    {

    }
    public void grdMaintenance_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      return;
      //DataGridView grid = (DataGridView)sender;
      //string columnName = grid.Columns[e.ColumnIndex].Name;
      //switch (columnName)
      //{
      //  case colCARRIER:
      //  case colLOCATIONID:
      //  case colPAYOR:
      //    if (e.Value == null)
      //      e.Value = string.Empty;
      //    else
      //    {
      //      object oCell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
      //      if (oCell.GetType() == typeof(DataGridViewComboBoxCell))
      //      {
      //        DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)oCell;
      //        if (cell.Items.Count > 0)
      //        {
      //          for (int i = 0; i < cell.Items.Count; i++)
      //          {
      //            if (((SearchResult)cell.Items[i]).Equals(cell.Value))
      //            {
      //              e.Value = ((SearchResult)cell.Items[i]).LegalName;
      //              break;
      //            }
      //          }
      //        }
      //      }
      //    }
      //    break;
      //  case colITEM:
      //    if (e.Value == null)
      //      e.Value = string.Empty;
      //    else
      //    {
      //      object oCell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
      //      if (oCell.GetType() == typeof(DataGridViewComboBoxCell))
      //      {
      //        DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)oCell;
      //        if (cell.Items.Count > 0)
      //        {
      //          for (int i = 0; i < cell.Items.Count; i++)
      //          {
      //            if (((ItemListEntry)cell.Items[i]).Equals(cell.Value))
      //            {
      //              e.Value = ((ItemListEntry)cell.Items[i]).ItemDescription;
      //              break;
      //            }
      //          }
      //        }
      //      }
      //    }
      //    break;
      //}
    }

    private void setDefaultValue(string columnName, object value, DataGridViewComboBoxColumn cell, object list)
    {
      if (!CommonFunctions.CString(value).Equals(string.Empty))
      {
        // if this has a value make it the default one
        if (DefaultValues.ContainsKey(columnName))
          DefaultValues[columnName] = value;
        else
          DefaultValues.Add(columnName, value);
        // and make the column read-only
        if (!HiddenColumns.ContainsKey(columnName))
          HiddenColumns.Add(columnName, null);
        // and disable the pick list
        if (cell != null)
          cell = new DataGridViewComboBoxColumn();
        if (list != null)
          list = new SearchResultCollection();
      }
    }
    public new bool isValidRow()
    {
      if (_suspendRowValidation)
        return true;
      DataGridViewRow row = grdMaintenance.CurrentRow;
      DataSet ds = ((DataView)grdMaintenance.DataSource).DataViewManager.DataSet;
      if (ds.Tables[0].Rows[row.Index].RowState == DataRowState.Unchanged)
        return true;
      if (row.IsNewRow)
        return true;
      string needsReview = CommonFunctions.CString(row.Cells["NeedsReview"].Value).ToLower();
      string itemID = CommonFunctions.CString(row.Cells["ItemID"].Value);
      string customerID = CommonFunctions.CString(row.Cells["Customer"].Value);
      bool hasLocation = false;
      string location = string.Empty;
      if (grdMaintenance.Columns.Contains("Location"))
      {
        location = CommonFunctions.CString(row.Cells["Location"].Value);
        hasLocation = true;
      }
      string commissionID = CommonFunctions.CString(row.Cells["CommissionID"].Value);
      string vendor = CommonFunctions.CString(row.Cells["Payor"].Value);
      string carrier = CommonFunctions.CString(row.Cells["Carrier"].Value);
      DateTime startDate = CommonFunctions.CDateTime(row.Cells["StartDate"].Value, CommonData.PastDateTime);
      long orderid = CommonFunctions.CLng(row.Cells["CityCareProdOrderID"].Value, 0);
      if (string.IsNullOrEmpty(customerID))
      {
        MessageBox.Show("Must have a Customer to save this record");
        return false;
      }
      if (string.IsNullOrEmpty(itemID))
      {
        DialogResult ans = MessageBox.Show("Are you sure you want to save this record with no Item?","Validate Network Inventory Record",MessageBoxButtons.YesNo);
        if (ans == DialogResult.No)
        {
          MessageBox.Show("Save Cancelled");
          return false;
        }
      }
      if (hasLocation && string.IsNullOrEmpty(location))
      {
        if (_dataSource.hasLocation(customerID) && !needsReview.Equals("revieword"))
        {
          MessageBox.Show("This record must have a Location");
          return false;
        }
      }
      if (string.IsNullOrEmpty(commissionID))
      {
        MessageBox.Show("Must have a CommissionID");
        return false;
      }
      if (orderid == 0 && !needsReview.Equals("revieword"))
      {
        MessageBox.Show("Must have an order id");
        return false;
      }
      if (startDate.Equals(CommonData.PastDateTime))
      {
        MessageBox.Show("Must have a StartDate");
        return false;
      }
      if (string.IsNullOrEmpty(vendor))
      {
        MessageBox.Show("Must have a vendor");
        return false;
      }
      if (string.IsNullOrEmpty(carrier))
      {
        MessageBox.Show("Must have a carrier");
        return false;
      }
      return true;
    }
  }
}
