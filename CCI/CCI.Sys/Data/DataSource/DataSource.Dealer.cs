using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using CCI.Common;
using CCI.Sys.SecurityEngine;
using TAGBOSS.Common.Model;


namespace CCI.Sys.Data
{
  public partial class DataSource
  {
    public ArrayList getDealerCustomerList(string dealer)
    {
      ArrayList list = new ArrayList();
      ArrayList notDealerList = new ArrayList();
      ArrayList dealerList = new ArrayList();
      list.Add(notDealerList);
      list.Add(dealerList);
      if (string.IsNullOrEmpty(dealer))
        return list;
      // changed 6-6-2013 to exclude customers from other dealers
      string sql = string.Format(@"select coalesce(d.SalesOrDealer, 'CCIDealer') Dealer, c.Entity + ': ' + c.LegalName Customer 
          from entity c 
          left join SalesOrDealerCustomers d on c.Entity = d.customer 
          inner join (
			select distinct Customer from NetworkInventory where Carrier = 'CityHosted'
			--union select distinct Customer from Orders where OrderType = 'Quote'
			) ni on ni.Customer = c.Entity
          where c.entitytype = 'customer' and  coalesce(d.SalesOrDealer, 'CCIDealer') in ('{0}', 'CCIDealer')     
          order by c.LegalName", dealer);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return list;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
      {
        foreach (DataRow row in ds.Tables[0].Rows)
        {
          string customerdealer = CommonFunctions.CString(row["Dealer"]);
          string customer = CommonFunctions.CString(row["Customer"]);
          if (!string.IsNullOrEmpty(customer))
            if (customerdealer.Equals(dealer, StringComparison.CurrentCultureIgnoreCase))
              dealerList.Add(customer);
            else
              notDealerList.Add(customer);
        }
      }
      ds.Clear();
      ds = null;
      return list;
    }
    public string getDealerForCustomer(string customerID)
    {
      return getDealerOrAgentForCustomer(customerID, "Dealer", true);

    }
    public string getAgentForCustomer(string customerID)
    {
      return getDealerOrAgentForCustomer(customerID, "Agent", false);
    }
    public int? moveDealerCustomers(ArrayList customerList, string dealer, string user)
    {
      // first we need to check to see which customers are in the table
      string inClause = CommonFunctions.ToList(customerList.ToArray(), "'");
      string sql = string.Format("select distinct customer from SalesOrDealerCustomers where Customer in ({1})", dealer, inClause);
      DataSet ds = getDataFromSQL(sql);
      ArrayList oldCustomers = new ArrayList();
      if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        foreach (DataRow row in ds.Tables[0].Rows)
          oldCustomers.Add(CommonFunctions.CString(row[0]));
      if (ds != null)
      {
        ds.Clear();
        ds = null;
      }
      ArrayList newCustomers = (ArrayList)customerList.Clone();
      foreach (string cust in oldCustomers)
        newCustomers.Remove(cust);
      // now we have two lists. One is customers that are already in the table (oldCustomers) and the other is ones that are not
      int? ret = null;
      if (oldCustomers.Count > 0)
      {
        inClause = CommonFunctions.ToList(oldCustomers.ToArray(), "'");
        sql = string.Format("update SalesOrDealerCustomers set SalesOrDealer = '{0}', LastModifiedBy = '{1}', LastModifiedDateTime = '{2}' where Customer in ({3})", 
          dealer, user, DateTime.Today.ToShortDateString(), inClause);
        ret = updateDataFromSQL(sql);
      }
      if (ret == null || ret >= 0)
      {
        if (newCustomers.Count > 0)
        {
          foreach (string newCust in newCustomers)
          {
            sql = string.Format("insert into SalesOrDealerCustomers (SalesOrDealer, Customer, LastModifiedBy, LastModifiedDateTime) Values('{0}','{1}','{2}','{3}')",
              dealer, newCust, user, DateTime.Today.ToShortDateString());
            ret = updateDataFromSQL(sql);
            if (ret != null && ret == -1)
              break;
          }
        }
      }
      return ret;
    }
    public DataSet getDealerPriceList(string dealer)
    {
      string sql = string.Format(@"select h.Dealer, h.itemid USOC, isnull(p.Name, h.itemid) Description, DealerCost, Install, StartDate, EndDate, LastModifiedBy, LastModifiedDateTime from hosteddealercosts h
left join masterproductlist p on h.itemid = p.itemid
where dealer = '{0}'", dealer);
      //string sql = string.Format("exec DealerPriceList '{0}'", dealer);
      return getDataFromSQL(sql);
    }
    public DataSet getDealerMarginReport(string dealer, DateTime billDate)
    {
      string dealerWhere = string.Empty;
      if (!string.IsNullOrEmpty(dealer) && !dealer.Equals("All", StringComparison.CurrentCultureIgnoreCase))
        dealerWhere = string.Format(" AND Dealer = '{0}' ", dealer);
      string sql = @"select * from vw_dealermarginreport where date = '{0}' {1} order by [rev code], name, usoc";
      sql = string.Format(sql, billDate.ToShortDateString(), dealerWhere);
      return getDataFromSQL(sql);
    }
    public PickListEntries getDealerOrSalesPersonList()
    {
      string sql = @"select distinct e.Entity, case when e.entitytype = 'Dealer' then legalname else firstname + ' ' + legalname end Name from vw_attributenonxml a
inner join entity e on a.entity = e.entity 
where (item =  'user' and name = 'salesperson' and Value = 'Yes') 
or (item =  'Dealer') order by case when e.entitytype = 'Dealer' then legalname else firstname + ' ' + legalname end ";
      DataSet ds = getDataFromSQL(sql);
      return CommonFunctions.toPickList(ds, "entity", "name");
    }
    public string getDealerOrAgentForCustomer(string customerID, string salesType, bool returnName = true)
    {
      string returnFieldName = returnName ? "DealerName" : "Entity";
      string sql = string.Format(@"Select d.Entity,  isnull(d.LegalName, 'CCI as CHS Dealer') DealerName
          from entity c 
          inner join (
			select distinct Customer from NetworkInventory where Carrier = 'CityHosted'
			) ni on ni.Customer = c.Entity
          left join SalesOrDealerCustomers dc on c.Entity = dc.customer 
		  left join entity d on d.entity = dc.SalesOrDealer
		  Where c.entity = '{0}' and dc.SalesType = '{1}'", customerID, salesType);
      using (DataSet ds = getDataFromSQL(sql))
      {
        if (ds == null)
          return null;
        if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count > 0)
          return CommonFunctions.CString(ds.Tables[0].Rows[0][returnFieldName]);
        else
          return null;
      }
    }

    public PickListEntries getSalesPersonList()
    {
      string sql = @"select distinct e.Entity, firstname + ' ' + legalname Name from vw_attributenonxml a
inner join entity e on a.entity = e.entity 
where (item =  'user' and name = 'salesperson' and Value = 'Yes')  order by firstname + ' ' + legalname ";
      DataSet ds = getDataFromSQL(sql);
      return CommonFunctions.toPickList(ds, "entity", "name");
    }
    /// <summary>
    /// Does this dealer have a different overlapping price level? The current record doesn't count of course
    /// </summary>
    /// <param name="dealer"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <returns></returns>
    public bool hasDealerLevel(string dealer, string level, DateTime startDate, DateTime endDate)
    {
      string levels = CommonFunctions.ToList(getDealerPricingLevels(), "'");
      string sql = string.Format(@"select itemid from HostedDealerCosts
        where dealer = '{0}' 
        and ItemId in ({5}) 
        and dbo.fn_isOverlapped(StartDate, isnull(EndDate, '{3}'), '{1}', '{2}') = 1
        and not (ItemID = '{4}' and startDate = '{1}')",
        dealer, startDate.ToShortDateString(), endDate.ToShortDateString(), CommonData.FutureDateTime.ToShortDateString(), level, levels);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return false;
      bool hasLevel = false;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        hasLevel = true;
      ds.Clear();
      ds = null;
      return hasLevel;
    }
    public bool hasOverlappingDealerUsocPrice(string dealer, string usoc, DateTime startDate, DateTime endDate)
    {
      string sql = string.Format(@"select itemid from HostedDealerCosts
        where dealer = '{0}' 
        and dbo.fn_isOverlapped(StartDate, isnull(EndDate, '{3}'), '{1}', '{2}') = 1
        and ItemID = '{4}' and startDate <> '{1}'",
        dealer, startDate.ToShortDateString(), endDate.ToShortDateString(), CommonData.FutureDateTime.ToShortDateString(), usoc);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return false;
      bool hasLevel = false;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        hasLevel = true;
      ds.Clear();
      ds = null;
      return hasLevel;
    }
    public Dictionary<string, object> getDealerLevel(string dealer, DateTime effectiveDate)
    {
      Dictionary<string, object> returnList = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
      string levels = CommonFunctions.ToList(getDealerPricingLevels(), "'");
      string sql = string.Format(@"select ItemId, StartDate, EndDate from HostedDealerCosts 
        where dealer = '{0}' 
        and ItemId in ({3}) 
        and '{1}' between StartDate and isnull(EndDate, '{2}')",
        dealer, effectiveDate.ToShortDateString(), CommonData.FutureDateTime.ToShortDateString(), levels);
      using (DataSet ds = getDataFromSQL(sql))
      {
        if (ds == null)
          return returnList;
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
          DataRow row = ds.Tables[0].Rows[0];
          returnList.Add("Level", CommonFunctions.CString(row["ItemID"]));
          returnList.Add("StartDate", CommonFunctions.CDateTime(row["StartDate"]));
          returnList.Add("EndDate", CommonFunctions.CDateTime(row["EndDate"]));
        }
      }
      return returnList;
    }
    public string getDealerLevel(string dealer, DateTime effectiveDate, string defaultLevel)
    {
      Dictionary<string, object> data = getDealerLevel(dealer, effectiveDate);
      if (data.Count == 0)
        return null;
      string level = CommonFunctions.CString(data["Level"]);
      if (string.IsNullOrEmpty(level))
        level = defaultLevel;
      return level;
    }
    public string getDealerFromOrder(string orderid)
    {
      if (string.IsNullOrEmpty(orderid))
        return string.Empty;
      string sql = @"select s.salesordealer from orders o
  inner join salesordealercustomers s on o.customer = s.customer where o.id = " + orderid;
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return string.Empty;
      string dealer = string.Empty;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        dealer = CommonFunctions.CString(ds.Tables[0].Rows[0][0]);
      ds.Clear();
      ds = null;
      return dealer;
    }
    #region Dealer Cost
    public CCITable getDealerCostInfo(string dealer, string usoc, DateTime effectiveDate)
    {
      string sql = string.Format(@"select DealerCost, Install from HostedDealerCosts 
        where Dealer = '{0}' and ItemID = '{1}' and '{2}' between StartDate and isnull(EndDate, '{3}')", 
        dealer, usoc, effectiveDate.ToShortDateString(), CommonData.FutureDateTime.ToShortDateString());
      return CommonFunctions.convertDataSetToCCITable(getDataFromSQL(sql));
    }
    public void updateDealerCost(string dealer, string usoc, string dealerCost, string install, string level, DateTime startDate, DateTime endDate, string user)
    {
      const string sqlupdate = 
        @"Update HostedDealerCosts set DealerCost = '{0}', Install = '{1}', EndDate = {5}, LastModifiedBy = '{6}', LastModifiedDateTime = '{7}'
        where Dealer = '{2}' and ItemID = '{3}' and StartDate = '{4}'";
      const string sqlselect = "select * from HostedDealerCosts where Dealer = '{0}' and ItemID = '{1}' and dbo.fn_isOverlapped(startDate, enddate, '{2}', '{3}') = 1";
      const string sqlinsert = 
        @"Insert into HostedDealerCosts (Dealer, ItemID, DealerCost, Install, StartDate, EndDate, LastModifiedBy, LastModifiedDateTime) 
        Values ('{2}', '{3}', '{0}', '{1}', '{4}', {5}, '{6}', '{7}')";
      string sql;
      bool existsLevel = false;
      bool existsDealer = false;
      bool existsrecord = false;
      bool updateDealer = true;
      decimal lCost = 0;
      decimal lInstall = 0;
      decimal dCost = 0;
      decimal dInstall = 0;
      string eDate = "null";
      if (endDate != CommonData.FutureDateTime)
        eDate = string.Format("'{0}'", endDate.ToShortDateString());
      if (existsRecord("HostedDealerCosts", new string[] { "Dealer", "ItemID", "StartDate"}, new string[] { dealer, usoc, startDate.ToShortDateString() }))
      {
        existsrecord = true;
        sql = string.Format(sqlselect, level, usoc, startDate.ToShortDateString(), endDate.ToShortDateString());
        DataSet ds = getDataFromSQL(sql);
        if (ds != null)
        {
          if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
          {
            existsLevel = true;
            lCost = CommonFunctions.CDecimal(ds.Tables[0].Rows[0]["DealerCost"]);
            lInstall = CommonFunctions.CDecimal(ds.Tables[0].Rows[0]["Install"]);
          }
          ds.Clear();
          ds = null;
        }
      }
      if (existsLevel)
      {
        sql = string.Format(sqlselect, dealer, usoc, startDate.ToShortDateString(), endDate.ToShortDateString());
        DataSet ds = getDataFromSQL(sql);
        if (ds != null)
        {
          if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
          {
            existsDealer = true;
            dCost = CommonFunctions.CDecimal(ds.Tables[0].Rows[0]["DealerCost"]);
            dInstall = CommonFunctions.CDecimal(ds.Tables[0].Rows[0]["Install"]);
          }
          ds.Clear();
          ds = null;
        }
        if (existsDealer) // we have both a level and a dealer
        {
          if (lCost == dCost && lInstall == dInstall) // they match, so delete the dealer
          {
            deleteDealerCost(dealer, usoc, null);
            updateDealer = false;
          }
        }
      }
      if (updateDealer)
      {
        if (existsrecord)
          sql = sqlupdate;
        else
          sql = sqlinsert;
        sql = string.Format(sql, dealerCost, install, dealer, usoc, startDate.ToShortDateString(), eDate, user, DateTime.Now.ToString(CommonData.FORMATLONGDATETIME));
        updateDataFromSQL(sql);
      }
    }
    public void deleteDealerCost(string dealer, string usoc, DateTime? startDate)
    {
      if (startDate == null)
      {
        if (existsRecord("HostedDealerCosts", new string[] { "Dealer", "ItemID" }, new string[] { dealer, usoc }))
        {
          string sql = string.Format("delete from HostedDealerCosts where Dealer = '{0}' and ItemID = '{1}'", dealer, usoc);
          updateDataFromSQL(sql);
        }
      }
      else
      {
        if (existsRecord("HostedDealerCosts", new string[] { "Dealer", "ItemID", "StartDate" }, new string[] { dealer, usoc, ((DateTime)startDate).ToShortDateString() }))
        {
          string sql = string.Format("delete from HostedDealerCosts where Dealer = '{0}' and ItemID = '{1}' and startDate = '{2}'", dealer, usoc, ((DateTime)startDate).ToShortDateString());
          updateDataFromSQL(sql);
        }
      }
    }
    public string[] getDealerPricingLevels()
    {
      return getCodeList("DealerPricingLevels");
      //return new string[] { "Gold", "Silver", "Bronze", "MasterGold", "MasterSilver", "MasterBronze" };
    }
    public bool isMasterPricingLevel(string level)
    {
      if (string.IsNullOrEmpty(level))
        return false;
      return level.IndexOf("Master", StringComparison.CurrentCulture) == 0;
    }
    public DataSet getHostedDealerCosts(string itemID)
    {
      string sql = string.Format(@"select dc.[Dealer] [Level]
,dc.[ItemID] USOC
,format(dc.[StartDate], 'M/d/yyyy') StartDate
,format(dc.[EndDate], 'M/d/yyyy') EndDate
,dc.[DealerCost] MRC
,dc.[Install] NRC
,dc.[LastModifiedBy]
,dc.[LastModifiedDateTime] 
from HostedDealerCosts dc 
inner join CodeMaster cm on dc.Dealer = cm.CodeValue and cm.CodeType = 'dealerpricinglevels'
where ItemID= '{0}' order by Dealer", itemID);
      return getDataFromSQL(sql);
    }
    public string cloneDealerCostLevel(string oldLevel, string newLevel, DateTime startDate, string user)
    {
      if (string.IsNullOrEmpty(oldLevel) || string.IsNullOrEmpty(newLevel))
        return "Error: Must have old level and new level";
      // first delete any ones that may already be there for this new level
      string sql = string.Format(@"DELETE FROM [HostedDealerCosts] WHERE Dealer = '{0}'", newLevel);
      updateDataFromSQL(sql);
      // now add the new ones by cloning from an old one
      sql = @"insert into [HostedDealerCosts]
select '{1}' Dealer, ItemID, '{2}' StartDate, null EndDate, DealerCost, Install, '{3}', getdate()
from [HostedDealerCosts]
where dealer = '{0}'
and EndDate is null";
      sql = string.Format(sql, oldLevel, newLevel, startDate.ToShortDateString(), user);
      updateDataFromSQL(sql);
      // now add the code to the list if it is not already there
      addCodeMasterCode("DealerPricingLevels",newLevel, user);
      return "Success";
    }
    public string deleteDealerCostLevel(string level)
    {
      if (string.IsNullOrEmpty(level))
        return "Error: Must have level to delete";
      string sql = string.Format("DELETE FROM [dbo].[HostedDealerCosts] WHERE Dealer = '{0}'", level);
      updateDataFromSQL(sql);
      deleteMasterCode("DealerPricingLevels", level);
      return "Success";
    }
    #endregion
  }
}
