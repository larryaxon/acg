﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using CCI.Common;

namespace CCI.Sys.Data
{
  public partial class DataSource
  {
    public DataSet getOldNetworkInventory(string netinvid)
    {
      return getDataFromSQL(string.Format(@"select ni.*, coalesce(ni.comments, po.comments ) pocomments from citycare_prod.dbo.i_networkinventory ni
left join citycare_prod.dbo.tblProdOrder po on ni.prodorderid = po.prodorderid where ni.netinv_id = {0}", netinvid));
    }
    public int updateOldNetworkInventory(string netinv_id, string user_customerid, string location_id, string product_id, decimal mrc, 
      DateTime expdate, DateTime installdate, string nm_cityaccmgr, string circuitid, string comments, string prodorder_id, bool active, 
      bool scrubbed, string prodNickName, string account)
    {
      string sql;
      int returnID = CommonFunctions.CInt(netinv_id, -1);;
      string sActive = ( active ? 1 : 0).ToString();
      string sScrubbed = (scrubbed ? 1 : 0).ToString();
      string sOrder = string.IsNullOrEmpty(prodorder_id) ? "null" : prodorder_id;
      string sComments = comments.Replace("'","''");
      if (string.IsNullOrEmpty(netinv_id))
      {
       sql = @"insert into citycare_prod.dbo.i_networkinventory (user_customerid, location_id, product_id, mrc, 
            expdate, installdate, nm_cityaccmgr, circuitid, prodorderid, active, scrubbed, prodnickname, vendoraccno, comments) 
          values ({1}, {2}, {3}, {4}, '{5}', '{6}', '{7}', '{8}', {9}, {10}, {11}, '{12}', '{13}', '{14}')";
      }
      else
      {
        sql = @"update citycare_prod.dbo.i_networkinventory set user_customerid = {1}, location_id = {2}, product_id = {3}, mrc = {4}, 
            expdate = '{5}', installdate = '{6}', nm_cityaccmgr = '{7}', circuitid = '{8}', prodorderid = {9}, 
            active = {10}, scrubbed = {11}, prodnickname = '{12}', vendoraccno = '{13}', comments = '{14}'
            where netinv_id = {0}";
      }
      sql = string.Format(sql, netinv_id, user_customerid, location_id, product_id, mrc,
        expdate, installdate, nm_cityaccmgr, circuitid, sOrder, sActive, sScrubbed, prodNickName, account, comments.Replace("'","''"));
      int? id = updateDataFromSQL(sql);
      if (id != null)
        if (id < 0) // is this an error
          throw new Exception(string.Format("SQL Error on update {0}", sql));
        else
          returnID = (int)id;
      //sql = string.Format("Update citycare_prod.dbo.tblProdOrder set comments = '{0}' where prodorderid = {1}", comments, prodorder_id);
      //id = updateDataFromSQL(sql);
      //if (id != null && id < 0) // error
      //  throw new Exception(string.Format("SQL Error on update {0}", sql));

      //if (string.IsNullOrEmpty(netinv_id)) // if this was an insert, then get the id we just created
      //  returnID = getLastNetInvID();

      return returnID;
    }
    public int getLastNetInvID()
    {
      string sql = "select Max(netinv_id) MaxID from citycare_prod.dbo.i_networkinventory";
      DataSet ds = getDataFromSQL(sql);
      int netinvid = -1;
      if (ds == null)
        return netinvid;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        netinvid = CommonFunctions.CInt(ds.Tables[0].Rows[0]["MaxID"]);
      ds.Clear();
      ds = null;
      return netinvid;
    }
    public string getOldVendorID(string productid)
    {
      string sql = "select vendorname from citycare_prod.dbo.tblProducts where productid = " + productid;
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return string.Empty;
      string vendorid = string.Empty;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
      {
        vendorid = CommonFunctions.CString(ds.Tables[0].Rows[0]["vendorname"]);
      }
      ds.Clear();
      ds = null;
      return vendorid;
    }
    public PickListEntries getOldVendorList()
    {
      string sql = "select vendorid, vendorname from citycare_prod.dbo.tblVendors order by vendorname";
      DataSet ds = getDataFromSQL(sql);
      return CommonFunctions.toPickList(ds, "vendorid", "vendorname");
    }
    public PickListEntries getOldCustomerList()
    {
      string sql = "select customerid, customername from citycare_prod.dbo.tblCustomers order by customername";
      DataSet ds = getDataFromSQL(sql);
      return CommonFunctions.toPickList(ds, "customerid", "customername");
    }
    public PickListEntries getOldProductList(string vendorid)
    {
      string vendorCondition = string.IsNullOrEmpty(vendorid) ? string.Empty : string.Format("where vendorname = '{0}' ", vendorid); // vendor id is a number, but use quotes just incase it isn't
      string sql = string.Format("select productid, productname from citycare_prod.dbo.tblProducts {0} order by productname", vendorCondition);
      return CommonFunctions.toPickList(getDataFromSQL(sql), "productid", "productname");
    }
    public PickListEntries getNewProductList(string carrier)
    {
      string carrierCondition = string.IsNullOrEmpty(carrier) ? string.Empty : string.Format("where carrier = '{0}' ", carrier); // vendor id is a number, but use quotes just incase it isn't
      string sql = string.Format("select p.ItemID, m.Name from ProductList p inner join MasterProductList m on m.ItemId = p.Itemid {0} order by m.Name", carrierCondition);
      return CommonFunctions.toPickList(getDataFromSQL(sql), "itemid", "name");
    }
    public PickListEntries getOldLocationList(string customerid)
    {
      string sql = string.Format("select locid, locationname from citycare_prod.dbo.tblLocations where customerid = {0} order by locationname", customerid);
      return CommonFunctions.toPickList(getDataFromSQL(sql), "locid", "locationname");
    }
    public PickListEntries getNewLocationList(string customerid)
    {
      string sql = string.Format("select Entity locid, legalname locationname from Entity where EntityOwner = '{0}' and EntityType = 'Location' order by legalname", customerid);
      return CommonFunctions.toPickList(getDataFromSQL(sql), "locid", "locationname");
    }
    public int getNewNetworkInventoryID(string oldnetinvid)
    {
      string sql = string.Format("select max(ID) from NetworkInventory where CityCareNetInvID = {0}", oldnetinvid);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return -1;
      int id = -1;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        id = CommonFunctions.CInt(ds.Tables[0].Rows[0][0]);
      ds.Clear();
      ds = null;
      return id;
    }
    public PickListEntries getOldAccountManager()
    {
      string sql = @"SELECT ni.nm_cityaccmgr AcctMgr
FROM citycare_prod.dbo.I_networkinventory ni
WHERE Isnull(ni.nm_cityaccmgr, '') <> ''
GROUP BY ni.nm_cityaccmgr";
      return CommonFunctions.toPickList(getDataFromSQL(sql), "AcctMgr", "AcctMgr");
    }
    public PickListEntries getNewEntityList(string entityType)
    {
      return getNewEntityList(entityType, null);
    }
    public PickListEntries getNewEntityList(string entityType, string userType)
    {
      string userTypeClause = string.Empty;
      if (!string.IsNullOrEmpty(userType))
        userTypeClause = string.Format(
          " inner join vw_attributenonxml x on e.entity = x.entity and x.ItemType = 'Entity' and e.entitytype = x.Item and x.name = 'UserType' and x.Value = '{0}' ",
          userType);
      string sql = string.Format(@"SELECT e.Entity ID, Case When e.EntityType = 'User' then e.FirstName + ' ' + e.LegalName else e.LegalName end Name 
from Entity e {1} Where e.EntityType = '{0}' order by Case When e.EntityType = 'User' then e.FirstName + ' ' + e.LegalName else e.LegalName end", entityType, userTypeClause);
      return CommonFunctions.toPickList(getDataFromSQL(sql), "ID", "Name");
    }
    public DataSet getOldNetworkInventoryOrderData(string orderid)
    {
      if (string.IsNullOrEmpty(orderid) || !CommonFunctions.IsNumeric(orderid))
        return null;
      string sql = @"SELECT po.prodorderid, po.orderid, po.productid, p.productname, l.customerid, c.customername, o.locID, l.locationname, po.ordertypeID, 
  po.comments, po.orderentrydate, po.reqduedate, po.datedue, po.plantdate, po.dateinstalled, p.vendorname AS VendorID, v.vendorname, po.mthlyrecrev AS MRC, 
po.AcctNum AS Account, o.contrexpdate AS ExpDate, po.cktid AS CircuitID, o.ssrid as AccountManagerID, ss.Name AccountManager
FROM citycare_prod.dbo.tblProdOrder po 
INNER JOIN citycare_prod.dbo.tblProducts p ON po.productid = p.productid 
INNER JOIN citycare_prod.dbo.tblOrders o ON po.orderid = o.orderid 
INNER JOIN citycare_prod.dbo.tblLocations l  ON o.locID = l.locID 
INNER JOIN citycare_prod.dbo.tblCustomers c ON l.customerid = c.customerid 
INNER JOIN citycare_prod.dbo.tblVendors v ON p.vendorname = v.vendorid
INNER JOIN citycare_prod.dbo.tblSalesSupport ss on ss.ssrid = o.ssrid
WHERE po.prodorderid = " + orderid;
      return getDataFromSQL(sql);
    }
    public int getOldNetworkInventoryIDFromOrder(string prodorderid)
    {
      string sql = string.Format("select max(netinv_id) from citycare_prod.dbo.i_networkinventory where prodorderid = {0}", prodorderid);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return -1;
      int netinvid = -1;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        netinvid = CommonFunctions.CInt(ds.Tables[0].Rows[0][0]);
      if (netinvid == 0)
        netinvid = -1;
      ds.Clear();
      ds = null;
      return netinvid;
    }
    public void deleteOldNetworkInventory(string netinv_id)
    {
      if (string.IsNullOrEmpty(netinv_id) || !CommonFunctions.IsNumeric(netinv_id))
        return;
      updateDataFromSQL("delete from citycare_prod.dbo.i_networkinventory where netinv_id = " + netinv_id);
    }
    public void deleteNewNetworkInventory(string id)
    {
      if (string.IsNullOrEmpty(id) || !CommonFunctions.IsNumeric(id))
        return;
      updateDataFromSQL("delete from NetworkInventory Where ID = " + id);
    }
    public DataSet getNewNetworkInventory(string id)
    {
      return getDataFromSQL(string.Format(@"select ni.*, case when p.PrimaryCarrier is null then 'N0ne' else p.PrimaryCarrier end PrimaryCarrier
from NetworkInventory ni 
left Join ProductList p on ni.Itemid = p.itemId and p.Carrier = 'CityHosted'
where ni.ID = {0}",id));
    }
    public bool isActiveNetworkInventory(string id)
    {
      bool isActive = false;
      DateTime thisDate = DateTime.Today;
      string sql = string.Format("select case when '{0}' between startDate and EndDate then 1 else 0 end Active from NetworkInventory where id = {1}",
        thisDate.ToShortDateString(), id.ToString());
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return isActive;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        isActive = CommonFunctions.CBoolean(ds.Tables[0].Rows[0][0], false);
      ds.Clear();
      ds = null;
      return isActive;
    }
    public int getCityCareOrderFromProdOrderID(string id)
    {
      string sql = "select orderid from citycare_prod.dbo.tblProdOrder po where prodorderid = " + id.ToString();
      DataSet ds = getDataFromSQL(sql);
      int orderid = -1;
      if (ds == null)
        return orderid;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        orderid = CommonFunctions.CInt(ds.Tables[0].Rows[0][0]);
      ds.Clear();
      ds = null;
      return orderid;
    }
    public int? updateNewNetworkInventory(string id, string customer, string location, int prodorderid, int orderid, string carrier, string itemid, string productNickName,
      string vendorAccount, decimal mrc, decimal nrc, string comments, string accountManager, string dealer, DateTime? installdate, DateTime? startDate,
      DateTime? endDate, string circuitID, string btn, int quantity, bool active, string vendor, DateTime? expirationDate, DateTime? transactionDate, 
      int transactionQuantity, decimal transactionMRC)
    {
      string sql;
      if (!string.IsNullOrEmpty(id) && CommonFunctions.IsNumeric(id) && existsRecord("NetworkInventory", new string[] { "ID" }, new string[] { id }))
      {
        sql = @"update NetworkInventory set Customer = '{0}', Location = '{1}', Carrier = '{2}', ItemID = '{3}', ProductNickName = '{4}',
                  VendorAccountID = '{5}', mrc = {6}, nrc = {7}, comments = '{8}', AccountManager = '{9}', DealerOrSalesPerson = '{10}',
                  InstallDate = {11}, StartDate = {12}, EndDate = {13}, CircuitID = '{14}', BTN = '{15}', OrderID = {16}, Quantity = {17}, CityCareProdOrderID = {18},
                  Payor = '{20}', ExpirationDate = {21}, TransactionDate = {22}, TransactionQuantity = '{23}', TransactionMRC = '{24}'
                WHERE ID = {19}";
      }
      else
        sql = @"Insert into NetworkInventory (Customer, Location, Carrier, ItemID, ProductNickName, VendorAccountID, MRC, NRC, Comments, AccountManager,
                DealerOrSalesPerson, InstallDate, StartDate, EndDate, CircuitID, BTN, OrderID, Quantity, CityCareProdOrderID, Payor, ExpirationDate,
                TransactionDate, TransactionQuantity, TransactionMRC)
                VALUES ('{0}','{1}','{2}', '{3}', '{4}', '{5}', {6}, {7}, '{8}', '{9}', '{10}', {11}, {12}, {13}, '{14}', '{15}', {16}, {17}, {18}, '{20}', {21}, {22},'{23}','{24}')";
        sql = string.Format(sql, customer, location, carrier, itemid, productNickName, vendorAccount, mrc, nrc, CommonFunctions.fixUpStringForSQL(comments), accountManager, dealer,
        printDate(installdate), printDate(startDate), printDate(endDate), circuitID, btn, orderid.ToString(), quantity.ToString(),orderid.ToString(),
         id, vendor, printDate(expirationDate), printDate(transactionDate), transactionQuantity, transactionMRC);
      int? ret = updateDataFromSQL(sql);
      // we need to run the activatioin sproc if the item is inactive and active flag is true, or if the item is active and the active flag is false
      bool setActive = (active && (DateTime.Today < startDate || DateTime.Today > endDate)) || (!active && DateTime.Today >= startDate && DateTime.Today <= endDate);
      if ((ret == null || ret >= 0) && setActive) // no error and we need to activate/inactivate
      {
        sql = string.Format("exec SetActiveNetworkInventory {0}, {1}", id, active ? 1 : 0);
        int? ret2 = updateDataFromSQL(sql);
        if (ret2 != null && ret2 < 0) // we had an error here
          return ret2;
      }
      return ret;
    }
    public int? savePhysicalInventory(string id, string netInvId, string macAddress, string notes, string user)
    {
      string sql;
      if (string.IsNullOrEmpty(id) || id == "-1")
        sql = @"INSERT INTO PhysicalInventory (NetWorkInventoryID, MacAddress, Notes, CreatedBy, CreatedDateTIme, ModifiedBy, ModifiedDateTime)
VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')";
      else
        sql = @"UPDATE PhysicalInventory
SET NetWorkInventoryID = '{0}', MacAddress = '{1}', Notes = '{2}', CreatedBy = '{3}', CreatedDateTime = '{4}', ModifiedBy = '{5}', ModifiedDateTime = '{6}'
WHERE ID = {7}";
      DateTime date = DateTime.Now;
      sql = string.Format(sql, netInvId, macAddress, notes, user, date, user, date, id);
      int? ret = updateDataFromSQL(sql);
      return ret;
    }
    public int? getNetworkInventoryFromPhysicalInventory(string customerid, string usoc, string location, DateTime asOfDate)
    {
      // find all current NetworkInventoryRecords that have available quantity for mac address and that match this customer, usoc, and location.
      // alo try to find one that has a quantity that is higher than the number of existing matching physical inventory records.
      // we just sort with the most available at the top, so if we have consumed ALL of the available "slots", we still get one.
      // That way, we don't keep the user from entering data even if they don't add up.
      string sql = @"select ni.ID, ni.ItemID, ni.Customer, ni.Location, ni.Quantity, isnull(pi.PICount, 0) PICount,
  ni.Quantity - isnull(pi.PICount, 0) QtyRemaining
   from NetworkInventory ni
  left join (select NetworkInventoryID NiId, count(*) PICount FROM PhysicalInventory GROUP BY NetworkInventoryID) pi 
	on ni.ID = pi.NiId
  where Customer = '{0}' and Location = '{1}' and ItemID = '{2}'
  order by ni.Quantity - isnull(pi.PICount, 0) desc";
      sql = string.Format(sql, customerid, location, usoc);
      using (DataSet ds = getDataFromSQL(string.Format(sql, customerid, location, usoc)))
      {
        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
          return null;
        return CommonFunctions.CInt(ds.Tables[0].Rows[0]["ID"]);
      }

    }
    public string[] getUsocsForNetworkInventory(string customerid, string location)
    {
      string sql = @"  select distinct ItemId Usoc from NetworkInventory
  where customer = '{0}' and location = '{1}'
  order by ItemID";
      using (DataSet ds = getDataFromSQL(string.Format(sql, customerid, location)))
      {
        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
          return new string[0];
        List<string> usocs = new List<string>();
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
          usocs.Add(CommonFunctions.CString(ds.Tables[0].Rows[i]["Usoc"]));
        return usocs.ToArray();
      }
    }
    private string printDate(DateTime? dt)
    {
      return printDate(dt, "d");
    }
    private string printDate(DateTime? dt, string format)
    {
      if (dt == null)
        return "null";
      return string.Format("'{0}'",((DateTime)dt).ToString(format));

    }
    public int getNewNetworkInventoryFromOrderID(string orderid)
    {
      string sql = "Select Max(ID) ID from NetworkInventory where Coalesce(OrderID, CityCareProdOrderID) = " + orderid;
      int netinvid = -1;
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return netinvid;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        netinvid = CommonFunctions.CInt(ds.Tables[0].Rows[0][0], -1);
      ds.Clear();
      ds = null;
      return netinvid;
    }
  }
}
