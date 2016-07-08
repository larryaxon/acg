using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using CCI.Common;
using CCI.Sys.SecurityEngine;

namespace CCI.Sys.Data
{
  public partial class DataSource
  {
    public void exemptUSOC(string usoc, string user, string retailOrWholesale)
    {
      string sql = string.Format(@"Insert into HostedNoMatchUSOC (USOC,Type,ModifiedDateTime,ModifiedBy) values ('{0}','{2}',getdate(),'{1}')", usoc, user, retailOrWholesale);
      updateDataFromSQL(sql);
      sql = string.Format(@"Update HostedMatchedMRC set Matchedby = 'Manual:Exempt',LastModifiedDateTime = getdate(),LastModifiedBy = '{0}' where {2}usoc = '{1}' and matchedby is null",
        user, usoc, retailOrWholesale);
      updateDataFromSQL(sql);
    }
    public void matchBTN(MRCMatchingSelectedItem wholesaleBTN, ArrayList retailBTNs, string user)
    {
      foreach (MRCMatchingSelectedItem retail in retailBTNs)
      {
        string sql = string.Format(@"Insert into HostedBTNMatching (Customer,WholesaleBTN,RetailBTN) values ('{0}','{1}','{2}')",
          wholesaleBTN.Customer, wholesaleBTN.WholesaleBTN, retail.RetailBtn);
        updateDataFromSQL(sql);
      }
      updateMatchingBTNs(user);
    }
    public void matchExceptionAlways(MRCMatchingSelectedItem wholesaleItem, ArrayList retailItems, string reasonCode, string user)
    {
      StringBuilder idList = new StringBuilder();
      retailItems.Add(wholesaleItem);
      // first add the exceptions to the exception table
      string[] keys = new string[] { "CustomerID", "BTN", "USOC" };
      foreach (MRCMatchingSelectedItem exception in retailItems)
      {
        if (idList.Length > 0)
          idList.Append(",");
        idList.Append(exception.ID);
        string tableName = "HostedNoMatchBTNUSOC";
        string[] values = new string[] { exception.Customer, string.IsNullOrEmpty(exception.WholesaleBTN) ? exception.RetailBtn : exception.WholesaleBTN, 
            string.IsNullOrEmpty(exception.WholesaleUSOC) ? exception.RetailUSOC : exception.WholesaleUSOC };
        string sql = string.Empty;
        if (!existsRecord(tableName, keys, values))
          insertRecord(tableName, keys, values);
      }
      // now mark all of the records as matched
      if (idList.Length > 0)
      {
        string sql = string.Format("update HostedMatchedMRC set MatchedBy = 'Manual: {2}', LastModifiedBy = '{0}', LastModifiedDateTime = getdate() where ID in ({1})",
          user, idList.ToString(), reasonCode);
        updateDataFromSQL(sql);
      }
    }
    public void matchOnce(MRCMatchingSelectedItem wholesaleItem, ArrayList retailItems, string reasonCode, string user)
    {
      StringBuilder idList = new StringBuilder();
      if (wholesaleItem != null)
        idList.Append(wholesaleItem.ID);
      foreach (MRCMatchingSelectedItem retail in retailItems)
      {
        if (idList.Length > 0)
          idList.Append(",");
        idList.Append(retail.ID);
      }
      if (idList.Length > 0)
      {
        string sql = string.Format("update HostedMatchedMRC set MatchedBy = 'Manual: {2}', LastModifiedBy = '{0}', LastModifiedDateTime = getdate() where ID in ({1})",
          user, idList.ToString(), reasonCode);
        updateDataFromSQL(sql);
      }
    }
    public void updateMatchingBTNs(string user)
    {
      return;
      /*
       update hostedmatchedmrc set RetailBillDate = r.retailbilldate,MasterRetailBTN = r.masterretailbtn,RetailBTN = r.retailbtn, RetailUSOC = r.retailusoc, RetailQty = RetailQty,RetailAmount = r.RetailAmount, MatchedBy = 'Manual:BTN Match',LastModifiedDateTime = getdate(),LastModifiedBy = operator from hostedmatchedmrc m inner join (select retailbilldate,masterretailbtn,bm.wholesalebtn,mr.retailbtn,retailusoc,Sum(RetailQty) RetailQty, Sum(RetailAmount) RetailAmount from hostedmatchedmrc mr inner join hostedbtnmatching bm on mr.retailbtn = bm.retailbtn where mr.matchedby is null group by mr.retailbilldate, mr.masterretailbtn, bm.wholesalebtn,mr.retailbtn, mr.retailusoc) r on m.wholesalebtn = r.wholesalebtn where m.matchedby is null and r.matchedby is null
       */
      string sql = @"update hostedmatchedmrc 
      set RetailBillDate = r.retailbilldate,
      MasterRetailBTN = r.masterretailbtn,
      RetailBTN = r.retailbtn, 
      RetailUSOC = r.retailusoc, 
      RetailQty = RetailQty,
      RetailAmount = r.RetailAmount, 
      MatchedBy = 'Manual:BTN Match',
      LastModifiedDateTime = getdate(),
      LastModifiedBy = '{0}' 
      from hostedmatchedmrc m 
       inner join (select retailbilldate,masterretailbtn,retailbtn,retailusoc,RetailQty, RetailAmount from hostedmatchedmrc where id = ?) r on m.id = ?";
      sql = string.Format(sql, user);
      updateDataFromSQL(sql);
    }
    public DataSet getCustomerMRCData(string customer)
    {
      string sql = string.Format("select * from HostedMatchedMRC where customerid = '{0}'", customer);
      return getDataFromSQL(sql);
    }

    public void matchBTN(string customer, string retailBTN, string WholesaleBTN)
    {
      if (!existsRecord("HostedBTNMatching", new string[] { "Customer", "WholesaleBTN", "RetailBTN" }, new string[] { customer, WholesaleBTN, retailBTN }))
      {
        string sql = string.Format("insert into HostedBTNMatching (Customer, WholesaleBTN, RetailBTN) values ('{0}', '{1}', '{2}')", customer, WholesaleBTN, retailBTN);
        updateDataFromSQL(sql);
      }
    }
    public string getBTNsfromCustomer(string customer)
    {
      string[] btnNames = new string[] { "MasterWholesaleBTN", "OriginalWholesaleBTN", "WholesaleBTN", "MasterRetailBTN", "RetailBTN" };
      StringBuilder btnList = new StringBuilder();
      string sql = string.Format(@"select distinct MasterWholesaleBTN, OriginalWholesaleBTN, WholesaleBTN, MasterRetailBTN, RetailBTN 
        from hostedmatchedmrc where customerid = '{0}'  ", customer);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return btnList.ToString();
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
      {
        Dictionary<string, string> btns = new Dictionary<string,string>(StringComparer.CurrentCultureIgnoreCase);
        foreach (DataRow row in ds.Tables[0].Rows)
        {
          foreach (string btnName in btnNames)
          {
            string btn = CommonFunctions.CString(row[btnName]);
            if (!btns.ContainsKey(btn))
              btns.Add(btn, null);
          }
        }
        foreach (KeyValuePair<string, string> b in btns)
        {
          if (btnList.Length > 0)
            btnList.Append(",");
          btnList.Append("'");
          btnList.Append(b.Key);
          btnList.Append("'");
        }
      }
      ds.Clear();
      ds = null;
      return btnList.ToString();
    }

    public void flagPastHostedTransactions(DateTime billDate)
    {
      string sql = string.Format(@"update hostedmatchedmrc
set matchedby = 'CheckOffPriorMonth'
where retailbilldate is not null and retailbilldate <= '{0}' and matchedby is null", billDate.ToShortDateString());
      updateDataFromSQL(sql);
       sql = string.Format(@"update hostedmatchedmrc
set matchedby = 'CheckOffPriorMonth'
where wholesalebilldate is not null and wholesalebilldate <= '{0}' and matchedby is null", billDate.AddMonths(-1).ToShortDateString());
      updateDataFromSQL(sql);
    }
    public int? updateOCCAdjustment(string id, string customerid, DateTime billdate, string retailUSOC, decimal retailAmount,
      string description, string user, string matchedBy)
    {
      string sql;
      if (id == null || !existsRecord("HostedMatchedOCC", new string[] { "ID" }, new string[] { id }))
        sql = @"insert into HostedMatchedOCC (CustomerID, RetailBillDate, RetailUSOC, RetailAMount, 
          RetailService, LastModifiedBy, LastModifiedDateTime, MatchedBy)
          Values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')";
      else
        sql = @"update HostedMatchedOCC set CustomerID = '{1}', RetailBillDate = '{2}', RetailUSOC = '{3}', 
          RetailAmount = '{4}', RetailService = '{5}', LastModifiedBy = '{6}', LastModifiedDateTime = '{7}', MatchedBy = '{8}' Where ID = {0}";
      sql = string.Format(sql, id, customerid, billdate.ToShortDateString(), retailUSOC, retailAmount.ToString(), description,
        user, DateTime.Now.ToString(CommonData.FORMATLONGDATETIME), matchedBy);
      return updateDataFromSQL(sql);
    }
    public int? deleteOCCAdjustment(string id)
    {
      if (string.IsNullOrEmpty(id))
        return -1;
      string sql = string.Format("Delete from HostedMatchedOCC where ID = {0}", id);
      return updateDataFromSQL(sql);
    }
  }
}
