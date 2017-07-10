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
    public const string HOSTEDSUBCATEGORY = "Hosted Voice";
    private const string HOSTEDWHOLESALEMASTERITEM = "HOSTEDBUSINESSLINE";
    public const string HOSTEDWHOLESALECARRIER = "Saddleback";
    public const string HOSTEDRETAILCARRIER = "CityHosted";
    private const string SETUNMATCHEDSQL = "update ProductList set unmatched = {0} where ItemID = '{1}' and Carrier = '{2}'";

    public CCITable getUSOCInfoFromUSOC(string usoc, bool isWholesale)
    {
      string fldName;
      if (isWholesale)
        fldName = "USOCWholesale";
      else
        fldName = "USOCRetail";
      string sql = string.Format("Select * from vw_CityHostedUSOCs where {0} = '{1}'", fldName, usoc);
      DataSet ds = getDataFromSQL(sql);
      return CommonFunctions.convertDataSetToCCITable(ds);
    }
    public ItemListCollection getUSOCList(bool isWholesale)
    {
      string carrier;
      if (isWholesale)
        carrier = HOSTEDWHOLESALECARRIER;
      else
        carrier = HOSTEDRETAILCARRIER;
      string sql = string.Format("select distinct p.ItemID, m.Name from ProductList p inner join MasterProductList m on p.Itemid = m.ItemiD where p.Carrier = '{0}'", carrier);
      DataSet ds = getDataFromSQL(sql);
      ItemListCollection list = new ItemListCollection();
      if (ds == null)
        return list;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
      {
        foreach (DataRow row in ds.Tables[0].Rows)
        {
          ItemListEntry item = new ItemListEntry();
          item.ItemID = CommonFunctions.CString(row["ItemID"]);
          item.ItemDescription = CommonFunctions.CString(row["Name"]);
          list.Add(item);
        }
      }
      ds.Clear();
      ds = null;
      return list;
    }
    /// <summary>
    /// takes the matching usoc line, and does the appropriate action. If an error occurs,
    /// returns the error message in the string return;
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="retailUSOC"></param>
    /// <param name="oldRetailUSOC"></param>
    /// <param name="wholesaleUSOC"></param>
    /// <param name="oldWholesaleUSOC"></param>
    /// <param name="retailDescription"></param>
    /// <param name="wholesaleDescription"></param>
    /// <param name="externalDescription"></param>
    /// <param name="retailMRC"></param>
    /// <param name="retailNRC"></param>
    /// <param name="wholesaleMRC"></param>
    /// <param name="wholesaleNRC"></param>
    /// <param name="chsCategory"></param>
    /// <param name="retailOnly"></param>
    /// <param name="wholesaleOnly"></param>
    /// <param name="retailStartDate"></param>
    /// <param name="retailEndDate"></param>
    /// <param name="wholesaleStartDate"></param>
    /// <param name="wholesaleEndDate"></param>
    /// <param name="isSaddlebackUSOC"></param>
    /// <param name="excludeFromException"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public string updateMatchedUSOC(CommonData.USOCMaintenanceOperation operation, string retailUSOC, 
      string wholesaleUSOC, string retailDescription, string wholesaleDescription,
      string externalDescription, decimal retailMRC, decimal retailNRC, decimal wholesaleMRC, decimal wholesaleNRC,
      string chsCategory, bool retailOnly, bool wholesaleOnly, DateTime? retailStartDate, DateTime? retailEndDate,
      DateTime? wholesaleStartDate, DateTime? wholesaleEndDate, bool isSaddlebackUSOC, bool excludeFromException,
      string externalCategory, string taxCode, string user)
    {
      string returnMsg = null;
      string wusoc = wholesaleUSOC;
      string rusoc = retailUSOC;
      string oldWholesaleUSOC;
      int? retCode = 0;
      if (wholesaleUSOC.Contains(":"))
        wusoc = wholesaleUSOC.Substring(0, wholesaleUSOC.IndexOf(":"));
      if (retailUSOC.Contains(":"))
        rusoc = retailUSOC.Substring(0, retailUSOC.IndexOf(":"));
      // set flags to update MasterProductList
      bool isRetail, isWholesale;
      if (retailOnly)
      {
        isRetail = true;
        isWholesale = wholesaleOnly;
      }
      else
      {
        isWholesale = true;
        isRetail = !wholesaleOnly;
      }
      /*
       * Cases:
       // DELETES:
       // - wholesale exists, need to delete it // we will not allow delete. you can terminate with an enddate
       // - both exist, need to delete them // we will not allow delete. you can terminate with an enddate       
       // - retaily only and need to delete it  // we will not allow delete. you can terminate with an enddate
       // UPDATES:
       * CommonData.USOCMaintenanceOperation.UpdateExisting:
       // - both exist, just need to update them
       // - wholesale exists, need to add retail (and connect it)
       // - retail exists, need to add wholesale (and connect it)
       // - neither exists, need to add both and connect them 
          *Conditions:
       // - both exist, but they have flagged wholesale only: 
       // - both exist, but they have flagged retail only
       // - both exist, and they are BOTH flagged as "only" // flagged as error
       // - they are unflagging wholesaleonly but there is no retail
       // - they are unflagging retail only but there is no wholesale   
       * case CommonData.USOCMaintenanceOperation.SwitchWholesale:
       // - both exist, but need to change retail (old retail becomes retail only)
       // - wholesaleexists, need to change retail (old wholesale becomes wholesaleonly)
       // - retail exists, need to change wholesale

       */
      // - both exist, just need to update them
      switch (operation)
      {
        case CommonData.USOCMaintenanceOperation.UpdateExisting:
          if (wholesaleOnly == retailOnly) // matched or both unmatched
          {
            retCode = updateMasterProduct(wusoc, wholesaleDescription, chsCategory, wusoc, externalCategory, externalDescription, isRetail, isWholesale, isSaddlebackUSOC);
            if (retCode == null || retCode >= 0)
              retCode = updateProduct(HOSTEDWHOLESALECARRIER, wusoc, wholesaleStartDate, wholesaleEndDate, user, wholesaleMRC, wholesaleNRC,
                wholesaleOnly, excludeFromException, taxCode);
            if (retCode == null || retCode >= 0)
              retCode = updateMasterProduct(rusoc, retailDescription, chsCategory, wusoc, externalCategory, externalDescription, isRetail, isWholesale, isSaddlebackUSOC);
            if (retCode == null || retCode >= 0)
              retCode = updateProduct(HOSTEDRETAILCARRIER, rusoc, retailStartDate, retailEndDate, user, retailMRC, retailNRC,
                retailOnly, excludeFromException, taxCode);
            if (retCode == null || retCode >= 0)
              retCode = reassignWholesaleUSOC(rusoc, wusoc);
          }            
          else if (wholesaleOnly)
          {
            retCode = updateMasterProduct(wusoc, wholesaleDescription, chsCategory, wusoc, externalCategory, externalDescription, isRetail, isWholesale, isSaddlebackUSOC);
            if (retCode == null || retCode >= 0)
              retCode = updateProduct(HOSTEDWHOLESALECARRIER, wusoc, wholesaleStartDate, wholesaleEndDate, user, wholesaleMRC, wholesaleNRC,
              wholesaleOnly, excludeFromException, taxCode);
            if (!string.IsNullOrEmpty(rusoc)) // if there is a retail usoc, then update that data, too
            {
              if (retCode == null || retCode >= 0)
                retCode = updateMasterProduct(rusoc, retailDescription, chsCategory, wusoc, externalCategory, externalDescription, isRetail, isWholesale, isSaddlebackUSOC);
              if (retCode == null || retCode >= 0)
                retCode = updateProduct(HOSTEDRETAILCARRIER, rusoc, retailStartDate, retailEndDate, user, retailMRC, retailNRC,
                retailOnly, excludeFromException, taxCode);
              if (retCode == null || retCode >= 0)
                retCode = setAllRetailToRetailOnly(wusoc);
            }
          }
          else if (retailOnly)
          {
            retCode = updateMasterProduct(rusoc, retailDescription, chsCategory, wusoc, externalCategory, externalDescription, isRetail, isWholesale, isSaddlebackUSOC);
            if (retCode == null || retCode >= 0)
              retCode = updateProduct(HOSTEDRETAILCARRIER, rusoc, retailStartDate, retailEndDate, user, retailMRC, retailNRC,
              retailOnly, excludeFromException, taxCode);
            oldWholesaleUSOC = getWholesaleUSOC(rusoc);
            if (!string.IsNullOrEmpty(wusoc)) // if there is a wholesale usoc, then update that data, too
            {
              retCode = updateMasterProduct(wusoc, wholesaleDescription, chsCategory, wusoc, externalCategory, externalDescription, isRetail, isWholesale, isSaddlebackUSOC);
              if (retCode == null || retCode >= 0)
                retCode = updateProduct(HOSTEDWHOLESALECARRIER, wusoc, wholesaleStartDate, wholesaleEndDate, user, wholesaleMRC, wholesaleNRC,
                wholesaleOnly, excludeFromException, taxCode);
            }
          }
          break;
        case CommonData.USOCMaintenanceOperation.AddNewRetail:
          retCode = updateMasterProduct(rusoc, retailDescription, chsCategory, wusoc, externalCategory, externalDescription, isRetail, isWholesale, isSaddlebackUSOC);
          if (retCode == null || retCode >= 0)
            retCode = updateProduct(HOSTEDRETAILCARRIER, rusoc, retailStartDate, retailEndDate, user, retailMRC, retailNRC,
            retailOnly, excludeFromException, taxCode);
          break;
        case CommonData.USOCMaintenanceOperation.AddNewWholesale:
          retCode = updateMasterProduct(wusoc, wholesaleDescription, chsCategory, wusoc, externalCategory, externalDescription, isRetail, isWholesale, isSaddlebackUSOC);
          if (retCode == null || retCode >= 0)
            retCode = updateProduct(HOSTEDWHOLESALECARRIER, wusoc, wholesaleStartDate, wholesaleEndDate, user, wholesaleMRC, wholesaleNRC,
            wholesaleOnly, excludeFromException, taxCode);          
          break;
        case CommonData.USOCMaintenanceOperation.SwitchWholesale:
          retCode = reassignWholesaleUSOC(rusoc, wusoc);
          break;
      }
      if (string.IsNullOrEmpty(returnMsg) && (retCode != null && retCode < 0))
        returnMsg = "Database error in update USOC";
      return returnMsg;
    }
    public void updateWholesaleUSOC(string usoc, string description, decimal mrc, decimal nrc, bool isRetail, DateTime? startDate, DateTime? endDate, 
      string category, string user, bool wholesaleonly, bool isSaddlebackUSOC, string externalDescription, bool excludeFromException, string taxCode)
    {
      bool hasRetail = existsProduct(usoc); 
      isRetail = isRetail || isUSOCRetailAndWholesale(usoc) || !hasRetail;
      string externalCategory = null;
      updateMasterProduct(usoc, description, HOSTEDSUBCATEGORY, usoc, externalCategory, externalDescription, isRetail, true, isSaddlebackUSOC);
      updateProduct(HOSTEDWHOLESALECARRIER, usoc, startDate, endDate, user, mrc, nrc, wholesaleonly, excludeFromException, taxCode);
      setUnmatchedUSOC(usoc, "Wholesale", wholesaleonly);
      if (!hasRetail) // there isn't a retail usoc yet, so we add one
        updateProduct(HOSTEDRETAILCARRIER, usoc, startDate, endDate, user, 0, 0, wholesaleonly, excludeFromException, taxCode);
    }
    public void updateRetailUSOC(string usoc, string wholesaleUSOC, string description, decimal mrc, decimal nrc, DateTime? startDate, DateTime? endDate,
      string category, string user, bool retailOnly, bool isSaddlebackUSOC, string externalDescription, bool excludeFromException, string taxCode)
    {
      if (string.IsNullOrEmpty(category))
        category = HOSTEDSUBCATEGORY;
      updateMasterProduct(usoc, description, category,  wholesaleUSOC, null, 
        externalDescription, true, (usoc.Equals(wholesaleUSOC, StringComparison.CurrentCultureIgnoreCase)), isSaddlebackUSOC);
      updateProduct(HOSTEDRETAILCARRIER, usoc, startDate, endDate, user, mrc, nrc, retailOnly, excludeFromException, taxCode);
      setUnmatchedUSOC(usoc, "Retail", retailOnly);
    }
    public int?  setUnmatchedUSOC(string usoc, string type, bool isUnmatched)
    {
      string carrier = "CityHosted"; // retail
      if (!string.IsNullOrEmpty(type) && type.Equals("Wholesale", StringComparison.CurrentCultureIgnoreCase))
        carrier = "Saddleback";
      string sql = string.Format("update ProductList set Unmatched = {0} where itemid = '{1}' and carrier = '{2}'", isUnmatched ? "1" : "0", usoc, carrier);
      return updateDataFromSQL(sql);
      //bool exists = existsNoMatchedUSOC(usoc, type);
      //if (!exists && isUnmatched)
      //{
      //  sql = string.Format("insert into HostedNoMatchUSOCs (Usoc, Type) Values ('{0}', '{1}')", usoc, type);
      //  updateDataFromSQL(sql);
      //}
      //else
      //  if (exists && !isUnmatched)
      //  {
      //    sql = string.Format("delete from HostedNoMatchUSOCs WHERE usoc = '{0}' and type = '{1}'", usoc, type);
      //    updateDataFromSQL(sql);
      //  }
    }
       
    //public bool existsNoMatchedUSOC(string usoc, string type)
    //{
    //  string sql = string.Format("select usoc from HostedNoMatchUSOCs where usoc = '{0}' and type = '{1}'", usoc, type);
    //  DataSet ds = getDataFromSQL(sql);
    //  if (ds == null)
    //    return false;
    //  bool retValue = false;
    //  if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //    retValue = true;
    //  ds.Clear();
    //  ds = null;
    //  return retValue;
    //}
    public void deleteRetailUSOC(string usoc)
    {
      deleteProduct(HOSTEDRETAILCARRIER, usoc);
      deleteMasterProduct(usoc);
    }
    public void deleteWholesaleUSOC(string usoc)
    {
      string sql = string.Format("delete from ProductList where Carrier = '{0}' and MasterItemID = '{1}'",HOSTEDWHOLESALECARRIER, usoc);
      updateDataFromSQL(sql);
      deleteMasterProduct(usoc);
    }
    public bool isUSOCRetailAndWholesale(string usoc)
    {
      string sql = string.Format("select isCityHostedRetail, isCityHostedWholesale from MasterProductList where Itemid = '{0}'", usoc);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return false;
      bool ret = false;
      if (ds.Tables.Count == 0)
        ret = false;
      else
      {
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count == 0)
          ret = false;
        else
          ret = CommonFunctions.CBoolean(dt.Rows[0]["isCityHostedRetail"]) && CommonFunctions.CBoolean(dt.Rows[0]["isCityHostedWholesale"]);
      }
      ds.Clear();
      ds = null;
      return ret;
    }
    public int? setAllRetailToRetailOnly(string wholesaleusoc)
    {
      return 0;
      /*
       --update ProductList
--set unmmatched = 1
select p.unmatched RetailOnly, p.carrier, p.itemid, m.name, w.itemid WholesaleUSOC, w.unmatched WholesaleOnly
from MasterProductList m
inner join ProductList p on m.itemid = p.itemid and p.Carrier = 'CityHosted'
inner join ProductList w on m.MasterItemId = w.Itemid and w.carrier = 'Saddleback'
where 
--m.MasterItemID = 'WNIPLD' and 
w.UnMatched = 1
       */
    }
    public int? reassignWholesaleUSOC(string retailusoc, string wholesaleusoc)
    {
      string wusoc = wholesaleusoc;
      string rusoc = retailusoc;
      int? retCode = 0;
      if (wholesaleusoc.Contains(":"))
        wusoc = wholesaleusoc.Substring(0, wholesaleusoc.IndexOf(":"));
      if (retailusoc.Contains(":"))
        rusoc = retailusoc.Substring(0, retailusoc.IndexOf(":"));
      // now check to see if the previous wholesale master is now orphaned
      string oldWholesaleUSOC = getWholesaleUSOC(rusoc);
      string sql = string.Format("update MasterProductList set MasterItemID = '{0}' where ItemID = '{1}'", wusoc,  retailusoc);
      updateDataFromSQL(sql);
      // now update the retailonly and wholesaleonly flags
      string unmatched = "0";
      if (string.IsNullOrEmpty(wusoc))
        unmatched = "1";
      sql = "update ProductList set unmatched = {0} where ItemID = '{1}' and Carrier = '{2}'";

      retCode = updateDataFromSQL(string.Format(sql, unmatched, rusoc, HOSTEDRETAILCARRIER)); // not retailonly
      //if (string.IsNullOrEmpty(rusoc))
      //  unmatched = "1";
      //else
      unmatched = "0";
      if (retCode >= 0 && !string.IsNullOrEmpty(wusoc))
        retCode = updateDataFromSQL(string.Format(sql, unmatched, wusoc, HOSTEDWHOLESALECARRIER)); // make sure new wholesale is NOT wholesaleonly 
      if (retCode >= 0 && numberRetailUSOCs(oldWholesaleUSOC, false) == 0) // there are no more "daughters"  
        retCode = updateDataFromSQL(string.Format(sql, "1", oldWholesaleUSOC, HOSTEDWHOLESALECARRIER)); // reset wholesale to wholesaleonly
      return retCode;
    }
    public string getWholesaleUSOC(string usoc)
    {
      string sql = string.Format("select masteritemid from masterproductlist where itemid = '{0}'",usoc);
      DataSet ds = getDataFromSQL(sql);
      string wholesaleUSOC = string.Empty;
      if (ds == null)
        return wholesaleUSOC;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        wholesaleUSOC = CommonFunctions.CString(ds.Tables[0].Rows[0]["masteritemid"]);
      ds.Clear();
      ds = null;
      return wholesaleUSOC;
    }
    public int numberRetailUSOCs(string wholesaleUSOC, bool includeInactive)
    {
      string activeClause = includeInactive ? string.Empty :
        string.Format("and convert(date, getdate()) between isnull(retail.startdate, '{0}') and isnull(retail.enddate, '{1}')",
          CommonData.PastDateTime.ToShortDateString(), CommonData.FutureDateTime.ToShortDateString());
      string sql = @"select m.ItemID, isnull(c.Count, 0) NumberRetailUSOCs from masterproductlist m
inner join productlist p on m.itemid = p.itemid and p.carrier = 'saddleback'
left join (
	select m2.masteritemid, count(*) Count 
	from masterproductlist m2
	left join productlist retail on m2.itemid = retail.itemid and retail.carrier = 'CityHosted'
	where m2.itemid <> m2.masteritemid 
	{1}
	group by m2.masteritemid
	) c 
on c.masteritemid = m.masteritemid 
where m.itemid = '{0}'";
      sql = string.Format(sql, wholesaleUSOC, activeClause);
      DataSet ds = getDataFromSQL(sql);
      int nbrRetail = 0;
      if (ds == null)
        return nbrRetail;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        nbrRetail = CommonFunctions.CInt(ds.Tables[0].Rows[0]["NumberRetailUSOCs"]);
      ds.Clear();
      ds = null;
      return nbrRetail;
    }
    public string[] getExternalCategories()
    {
      string sql = @"select CodeValue ExternalCategory from CodeMaster where CodeType = 'RITUSOCCategories' order by CodeValue";
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return new string[0];
      string[] categories = new string[0];
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
      {
        categories = new string[ds.Tables[0].Rows.Count];
        int i = 0;
        foreach (DataRow row in ds.Tables[0].Rows)
          categories[i++] = CommonFunctions.CString(row["ExternalCategory"]);
      }
      ds.Clear();
      ds = null;
      return categories;
    }
    public int? updateDealerQuoteScreenData(string usoc, string section, bool isRecommended, bool useMRC, string user)
    {
      return updateScreenDefinition(CommonData.DEALERQUOTESCREEN, section, usoc, null, "99", isRecommended.ToString(), useMRC.ToString(), null, user);
    }
    public string[] getPrimaryCarriers()
    {
      string sql = @"select distinct PrimaryCarrier from ProductList WHERE PrimaryCarrier is not null";
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return new string[0];
      string[] carriers = new string[0];
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
      {
        carriers = new string[ds.Tables[0].Rows.Count];
        int i = 0;
        foreach (DataRow row in ds.Tables[0].Rows)
          carriers[i++] = CommonFunctions.CString(row["PrimaryCarrier"]);
      }
      ds.Clear();
      ds = null;
      return carriers;
    }
    public string[] getRetailUsocs(string carrier)
    {
      if (string.IsNullOrEmpty(carrier))
        return new string[0];
      const string basesql = "SELECT distinct ItemID USOC from ProductList where Carrier = 'CityHosted' and PrimaryCarrier = '{0}'";
      string sql = string.Format(basesql, carrier);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return new string[0];
      string[] carriers = new string[0];
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
      {
        carriers = new string[ds.Tables[0].Rows.Count];
        int i = 0;
        foreach (DataRow row in ds.Tables[0].Rows)
          carriers[i++] = CommonFunctions.CString(row["USOC"]);
      }
      ds.Clear();
      ds = null;
      return carriers;
    }
    public string[] getWholesaleUsocs(string carrier)
    {
      if (string.IsNullOrEmpty(carrier))
        return new string[0];
      const string basesql = "SELECT distinct ItemID USOC from ProductList where Carrier = '{0}'";
      string sql = string.Format(basesql, carrier);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return new string[0];
      string[] carriers = new string[0];
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
      {
        carriers = new string[ds.Tables[0].Rows.Count];
        int i = 0;
        foreach (DataRow row in ds.Tables[0].Rows)
          carriers[i++] = CommonFunctions.CString(row["USOC"]);
      }
      ds.Clear();
      ds = null;
      return carriers;
    }
  }
}
