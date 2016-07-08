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
  public partial class ctlMasterProducts : ctlMaintenanceBase
  {
    private const string colMASTERITEMID = "MasterItemID";
    private DataGridViewComboBoxColumn _masterItemID = new DataGridViewComboBoxColumn();
    private ItemListCollection _masterItemList = null;
    private const string colITEMSUBCATEGORY = "ItemSubCategory";
    private DataGridViewComboBoxColumn _itemSubCategory = new DataGridViewComboBoxColumn();
    private string[] _itemSubCategoryList = null; 

    public ctlMasterProducts()
    {
      TableName = "MasterProductList";
      DefaultValues.Add("StartDate", new DateTime(1900, 1, 1));
      InitializeComponent();
      ShowExtendedField = false;
    }
    public void Init(string criteria)
    {
      // master item 
      if (_masterItemList == null)
      {
        _masterItemList = _dataSource.getMasterItemList();
      }
      if (_masterItemID.Items.Count == 0)
      {
        foreach (ItemListEntry entry in _masterItemList)
          _masterItemID.Items.Add(entry);
        _masterItemID.ValueMember = "MasterItemID";
        _masterItemID.DisplayMember = "MasterItemID";
        //_itemID.ValueType = typeof(ItemListEntry);
      }
      if (!PickLists.ContainsKey(colMASTERITEMID))
        PickLists.Add(colMASTERITEMID, _masterItemID);

      // sub category   
      if (_itemSubCategoryList == null)
          _itemSubCategoryList = _dataSource.getItemSubCategories(String.Empty);
      if (_itemSubCategory.Items.Count == 0)
      {
          _itemSubCategory.Items.AddRange(_itemSubCategoryList);

      }
      if (!PickLists.ContainsKey(colITEMSUBCATEGORY))
          PickLists.Add(colITEMSUBCATEGORY, _itemSubCategory);

      
      if (string.IsNullOrEmpty(criteria))
        load(null);
      else
      {
        Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
        parameters.Add("Criteria", criteria);
        load(parameters);
      }
    }
    public new void load(Dictionary<string, string> parameters)
    {
      base.load(parameters);
      foreach (DataGridViewRow row in grdMaintenance.Rows)
      {
        DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)_masterItemID.Clone();
        row.Cells[colMASTERITEMID] = cell;
        //cell.ValueType = typeof(ItemListEntry);
        CommonFormFunctions.setComboBoxCell(cell, row.Cells[colMASTERITEMID].Value);
        cell = (DataGridViewComboBoxCell)_masterItemID.Clone();
        row.Cells[colMASTERITEMID] = cell;
        //cell.ValueType = typeof(SearchResult);
        CommonFormFunctions.setComboBoxCell(cell, row.Cells[colMASTERITEMID].Value);
        //CAC... hook up picklist for subcategory
        DataGridViewComboBoxCell cell2 = (DataGridViewComboBoxCell)_itemSubCategory.Clone();
        row.Cells[colITEMSUBCATEGORY] = cell2;
        //cell.ValueType = typeof(ItemListEntry);
        CommonFormFunctions.setComboBoxCell(cell2, row.Cells[colITEMSUBCATEGORY].Value);
        cell2 = (DataGridViewComboBoxCell)_itemSubCategory.Clone();
        row.Cells[colITEMSUBCATEGORY] = cell2;
        //cell.ValueType = typeof(SearchResult);
        CommonFormFunctions.setComboBoxCell(cell2, row.Cells[colITEMSUBCATEGORY].Value);
      }
    }
  }
}
