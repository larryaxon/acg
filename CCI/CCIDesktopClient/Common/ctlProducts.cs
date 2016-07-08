using System;
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
  public partial class ctlProducts : ctlMaintenanceBase
  {
    private const string colCARRIER = "Carrier";
    private DataGridViewComboBoxColumn _carrier = new DataGridViewComboBoxColumn();
    private SearchResultCollection _carrierList = null;
    private const string colITEMID = "ItemID";
    private DataGridViewComboBoxColumn _itemID = new DataGridViewComboBoxColumn();
    private ItemListCollection _itemMasterList = null; 
    
	public ctlProducts()
    {
      TableName = "Products";
      DefaultValues.Add("StartDate", new DateTime(1900, 1, 1));
      InitializeComponent();
      ShowExtendedField = false;
    }
    public void Init(string criteria, string carrierid)
    {
      // sub category
      if (_itemMasterList == null)
      {
        _itemMasterList = _dataSource.getItemMasterList();
      }
      if (_itemID.Items.Count == 0)
      {
        foreach (ItemListEntry entry in _itemMasterList)
          _itemID.Items.Add(entry);
        _itemID.ValueMember = "ItemID";
        _itemID.DisplayMember = "MasterItemText";
        //_itemID.ValueType = typeof(ItemListEntry);
      }
      if (!PickLists.ContainsKey(colITEMID))
        PickLists.Add(colITEMID, _itemID);
      // carrier
      if (_carrierList == null)
        _carrierList = _dataSource.SearchEntities("*", "Carrier");
      if (_carrier.Items.Count == 0)
      {
        foreach (SearchResult carrier in _carrierList)
          _carrier.Items.Add(carrier);
        _carrier.DisplayMember = "LegalName";
        _carrier.ValueMember = "EntityID";
        //_carrier.ValueType = typeof(SearchResult);
      }
      if (!PickLists.ContainsKey(colCARRIER))
        PickLists.Add(colCARRIER, _carrier);
      Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
      if (!string.IsNullOrEmpty(criteria))
        parameters.Add("Criteria", criteria);
      if (!string.IsNullOrEmpty(carrierid))
        parameters.Add("Carrier", carrierid);
      load(parameters);
    }
    public new void load(Dictionary<string, string> parameters)
    {
      base.load(parameters);
      foreach (DataGridViewRow row in grdMaintenance.Rows)
      {
        DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)_itemID.Clone();
        row.Cells[colITEMID] = cell;
        //cell.ValueType = typeof(ItemListEntry);
        CommonFormFunctions.setComboBoxCell(cell, row.Cells[colITEMID].Value);
        cell = (DataGridViewComboBoxCell)_carrier.Clone();
        row.Cells[colCARRIER] = cell;
        //cell.ValueType = typeof(SearchResult);
        CommonFormFunctions.setComboBoxCell(cell, row.Cells[colCARRIER].Value);
      }
    }
  }
}
