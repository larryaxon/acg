using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using CCI.Common;

namespace CCI.Sys.Data
{
  public partial class DataSource
  {
    #region Products
    public string[] getItemCategories()
    {
      string sql = "Select * from ItemCategories order by ItemCategory";
      DataSet ds = getDataFromSQL(sql);
      if (ds.Tables.Count > 0)
      {
        string[] list = new string[ds.Tables[0].Rows.Count];
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
          list[i] = CommonFunctions.CString(ds.Tables[0].Rows[i]["ItemCategory"]);
        return list;
      }
      else
        return new string[0];
    }
    public string[] getItemSubCategories(string itemCategory)
    {
      string sql;
      if (string.IsNullOrEmpty(itemCategory))
        sql = "Select ItemSubCategory from ItemSubCategories order by ItemSubCategory";
      else
        sql = string.Format("Select ItemSubCategory from ItemSubCategories where ItemCategory = '{0}' order by ItemSubCategory", itemCategory);
      DataSet ds = getDataFromSQL(sql);
      if (ds.Tables.Count > 0)
      {
        string[] list = new string[ds.Tables[0].Rows.Count];
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
          list[i] = CommonFunctions.CString(ds.Tables[0].Rows[i]["ItemSubCategory"]);
        return list;
      }
      else
        return new string[0];
    }
    //public ItemListCollection getItemSubCategories(string itemCategory)
    //{
    //    string sql;
    //    ItemListCollection list = new ItemListCollection();
    //    if (string.IsNullOrEmpty(itemCategory))
    //        sql = "Select ItemSubCategory from ItemSubCategories";
    //    else
    //        sql = string.Format("Select ItemSubCategory from ItemSubCategories where ItemCategory = '{0}'", itemCategory);
    //    DataSet ds = getDataFromSQL(sql);
    //    if (ds == null)
    //        return list;
    //    if (ds.Tables.Count > 0)
    //    {
    //        //string[] list = new string[ds.Tables[0].Rows.Count];
    //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //        {
    //            ItemListEntry entry = new ItemListEntry();
    //            entry.ItemSubCategory = CommonFunctions.CString(ds.Tables[0].Rows[i]["ItemSubCategory"]);
    //            list.Add(entry);
    //        }
    //    }
    //    ds.Clear();
    //    ds = null;
    //    return list;
    //}

    public ItemListCollection getItemMasterList()
    {
      ItemListCollection list = new ItemListCollection();
      string sql = @"select m.ItemID, m.Name, m.ItemSubCategory, m2.ItemID MasterItemId, m2.Name MasterItemName 
from MasterProductList m 
inner join MasterProductList m2 on m.MasterItemID = m2.ItemID order by m2.Name, m.Name";
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return list;
      if (ds.Tables.Count > 0)
      {
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
          ItemListEntry entry = new ItemListEntry();
          entry.ItemID = CommonFunctions.CString(ds.Tables[0].Rows[i]["ItemID"]);
          entry.ItemDescription = CommonFunctions.CString(ds.Tables[0].Rows[i]["Name"]);
          entry.ItemSubCategory = CommonFunctions.CString(ds.Tables[0].Rows[i]["ItemSubCategory"]);
          entry.MasterItemId = CommonFunctions.CString(ds.Tables[0].Rows[i]["MasterItemID"]);
          entry.MasterItemDescription = CommonFunctions.CString(ds.Tables[0].Rows[i]["MasterItemName"]);
          list.Add(entry);
        }
      }
      ds.Clear();
      ds = null;
      return list;
    }
    // CAC
    public ItemListCollection getMasterItemList()
    {
      ItemListCollection list = new ItemListCollection();
      //        string sql = 
      //            @"select m.ItemID, m.Name, m.ItemSubCategory, m2.ItemID MasterItemId, m2.Name MasterItemName 
      //            from MasterProductList m 
      //            inner join MasterProductList m2 on m.MasterItemID = m2.ItemID order by m2.Name, m.Name";
      string sql = @"select * from masterproductlist where itemid = masteritemid";

      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return list;
      if (ds.Tables.Count > 0)
      {
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
          ItemListEntry entry = new ItemListEntry();
          entry.ItemID = CommonFunctions.CString(ds.Tables[0].Rows[i]["ItemID"]);
          entry.ItemDescription = CommonFunctions.CString(ds.Tables[0].Rows[i]["Name"]);
          entry.ItemSubCategory = CommonFunctions.CString(ds.Tables[0].Rows[i]["ItemSubCategory"]);
          entry.MasterItemId = CommonFunctions.CString(ds.Tables[0].Rows[i]["MasterItemID"]);
          //CAC
          //entry.MasterItemDescription = CommonFunctions.CString(ds.Tables[0].Rows[i]["MasterItemName"]);
          list.Add(entry);
        }
      }
      ds.Clear();
      ds = null;
      return list;
    }
    // end
    public ItemListCollection getItemList(string carrier)
    {
      ItemListCollection list = new ItemListCollection();
      string sql = @"select distinct p.ItemID, m.Name ItemDescription, p.Carrier, e.LegalName CarrierDescription, m.ItemSubCategory 
      from ProductList p inner join MasterProductList m on p.ItemId = m.ItemId left join Entity e on p.carrier = e.entity ";
      if (!string.IsNullOrEmpty(carrier))
        sql += string.Format(" where p.carrier = '{0}'", carrier);
      sql += " order by m.name, p.carrier, p.itemid";
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return list;
      if (ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
      {
        foreach (DataRow row in ds.Tables[0].Rows)
        {
          ItemListEntry item = new ItemListEntry(CommonFunctions.CString(row["ItemID"]),
                                                CommonFunctions.CString(row["ItemDescription"]),
                                                CommonFunctions.CString(row["Carrier"]),
                                                CommonFunctions.CString(row["CarrierDescription"]),
                                                CommonFunctions.CString(row["ItemSubCategory"]));
          list.Add(item);
        }
      }
      ds.Clear();
      ds = null;
      return list;
    }
    public bool existsProduct(string itemID)
    {
      string sql = string.Format("select ItemId from MasterProductList where ItemID = '{0}'", itemID);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null) 
        return false;
      bool recordExists = !(ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0);
      ds.Clear();
      ds = null;
      return recordExists;
    }
    public bool existsProduct(string carrier, string itemID)
    {
      string sql = string.Format("select ItemId from ProductList where ItemID = '{0}' and Carrier = '{1}'", itemID, carrier);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return false;
      bool recordExists = !(ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0);
      ds.Clear();
      ds = null;
      return recordExists;
    }
    public void updateMasterProduct(string[] fieldNames, string[] fieldValues)
    {
      string tableName = "MasterProductList";
      string[] keyList = new string[] { "ItemID" };
      if (existsRecord(tableName, keyList, new string[] { fieldValues[0] })) // exists
        updateRecord(fieldNames, fieldValues, keyList, tableName);
      else
        insertRecord(tableName, fieldNames, fieldValues);
    }
    public void updateProduct(string[] fieldNames, string[] fieldValues)
    {
      string tableName = "ProductList";
      string[] keyList = new string[] { "Carrier", "ItemID" };
      if (existsRecord(tableName, keyList, new string[] { fieldValues[0], fieldValues[1] })) // exists
        updateRecord(fieldNames, fieldValues, keyList, tableName);
      else
        insertRecord(tableName, fieldNames, fieldValues);
    }
    public int? updateMasterProduct(string itemID, string name, string subCategory, string masterItemID, string externalCategory, string externalDescription,
      bool isRetail, bool isWholesale, bool isSaddleback)
    {
      string sql;
      if (existsProduct(itemID))
        sql = @"update MasterProductList set Name = '{1}', ItemSubCategory = '{2}', MasterItemID = '{3}', ExternalCategory = '{4}', IsCityHostedRetail = {5}, 
            IsCityHostedWholesale = {6}, IsSaddlebackUSOC = {7}, ExternalName = '{8}'  where ItemID = '{0}'";
      else
        sql = @"insert into MasterProductList (ItemID, Name, ItemSubCategory, MasterItemID, ExternalCategory, IsCityHostedRetail, IsCityHostedWholesale, IsSaddlebackUSOC, ExternalName)
            Values ('{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, {7}, '{8}')";
      sql = string.Format(sql, itemID, CommonFunctions.fixUpStringForSQL(name), subCategory, masterItemID, externalCategory, 
        isRetail ? 1 : 0, isWholesale ? 1 : 0, isSaddleback ? 1 : 0, CommonFunctions.fixUpStringForSQL(externalDescription));
      return updateDataFromSQL(sql);
    }
    public int? updateProduct(string carrier, string itemID, DateTime? startDate, DateTime? endDate, string lastModifiedBy, decimal mrc, decimal nrc,
      bool unmatched, bool excludeFromException, string taxCode)
    {
      string sql;
      if (existsProduct(carrier, itemID))
        sql = @"update ProductList set StartDate = {2}, EndDate = {3}, LastModifiedBy = '{4}', LastModifiedDateTime = '{5}', MRC = {6}, NRC = {7},
          Unmatched = '{8}', ExcludeFromException = '{9}', TaxCode = '{10}' where carrier = '{0}' and ItemID = '{1}'";
      else
        sql = @"insert into ProductList (Carrier, ItemID, StartDate, EndDate, LastModifiedBy, LastModifiedDateTime,MRC,NRC,Unmatched,ExcludeFromException,TaxCode) 
              Values ('{0}', '{1}', {2}, {3}, '{4}', '{5}', {6}, {7}, '{8}', '{9}', '{10}')";
      sql = string.Format(sql, carrier, itemID, startDate == null ? "null" : string.Format("'{0}'", ((DateTime)startDate).ToShortDateString()), endDate == null ? "null" : string.Format("'{0}'", ((DateTime)endDate).ToShortDateString()),
          lastModifiedBy, DateTime.Now.ToString(CommonData.FORMATLONGDATETIME), mrc.ToString(), nrc.ToString(), 
          unmatched ? "1" : "0", excludeFromException ? "1" : "0", taxCode);
      return updateDataFromSQL(sql);
    }
    public int?  deleteProduct(string carrier, string itemid)
    {
      if (existsRecord("ProductList", new string[] { "Carrier", "ItemID" }, new string[] { carrier, itemid }))
      {
        string sql = string.Format("Delete from ProductList where Carrier = '{0}' and ItemID = '{1}'", carrier, itemid);
        return updateDataFromSQL(sql);
      }
      return -1 ;
    }
    public int? deleteMasterProduct(string itemid)
    {
      string sql = string.Format("Delete from MasterProductList where ItemID = '{0}'", itemid);
      return updateDataFromSQL(sql);
    }
    public string[] searchMasterProducts(string criteria)
    {
      if (string.IsNullOrEmpty(criteria))
        return new string[0];
      string sql = "select ItemID, Name from MasterProductList where itemid like '%{0}%' or name like '%{0}%' order by Name";
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return new string[0];
      string[] list = new string[0];
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
      {
        list = new string[ds.Tables[0].Rows.Count];
        int iRow = 0;
        foreach (DataRow row in ds.Tables[0].Rows)
          list[iRow++] = string.Format("{0}: {1}", CommonFunctions.CString(row["Name"]), CommonFunctions.CString(row["ItemID"]));
      }
      ds.Clear();
      ds = null;
      return list;

    }
    public DataSet getMasterProduct(string itemid)
    {
      string sql = string.Format("select * from MasterProductList where itemid = '{0}'", itemid);
      return getDataFromSQL(sql);
    }
    public DataSet getProduct(string itemid, string carrier)
    {
      string sql = string.Format("select * from ProductList where itemid = '{0}' and carrier = '{1}'", itemid, carrier);
      return getDataFromSQL(sql);
    }
    public string[] getTaxCodes()
    {
      string sql = "Select CodeValue TaxCode from CodeMaster where CodeType = 'RITTaxCode' order by CodeValue";
      //if (!string.IsNullOrEmpty(carrier))
      //  sql = string.Format("{0} and carrier = '{1}'", sql, carrier);
      DataSet ds = getDataFromSQL(sql);
      string[] returnList = new string[0];
      if (ds == null)
        return returnList;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
      {
        returnList = new string[ds.Tables[0].Rows.Count];
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
          returnList[i] = CommonFunctions.CString(ds.Tables[0].Rows[i][0]);
      }
      ds.Clear();
      ds = null;
      return returnList;
    }
    public DataSet getHostedDealerPriceList(string itemID)
    {
      string sql = string.Format("select ID, Price, StartDate, EndDate from HostedUSOCRetailPricing where RetailUSOC = '{0}' order by Price, StartDate", itemID);
      return getDataFromSQL(sql);
    }
    public int? updateHostedDealerPrice(int? id, string itemID, decimal price, DateTime? startDate, DateTime? endDate, string user)
    {
      string strStartDate, strEndDate, sql;
      if (startDate == null)
        strStartDate = "null";
      else
        strStartDate = string.Format("'{0}'",((DateTime)startDate).ToShortDateString());
      if (endDate == null)
        strEndDate = "null";
      else
        strEndDate = string.Format("'{0}'",((DateTime)endDate).ToShortDateString());
      if (id == null)
        sql = @"insert into HostedUSOCRetailPricing (Price, StartDate, EndDate, LastModifiedBy, LastModifiedDateTime, RetailUSOC) Values ({0},{1},{2},'{3}','{4}','{5}')";        
      else
        sql = @"update HostedUSOCRetailPricing set Price = {0}, StartDate = {1}, EndDate = {2}, LastModifiedBy = '{3}', LastModifiedDateTime = '{4}', RetailUSOC = '{5}'
            where ID = {6}";
      sql = string.Format(sql, price, strStartDate, strEndDate, user, DateTime.Now.ToString(CommonData.FORMATLONGDATETIME), itemID, id);
      return updateDataFromSQL(sql);
    }
    #endregion

  }
}
