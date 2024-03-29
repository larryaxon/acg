﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using CCI.Common;
using CCI.Sys.SecurityEngine;
using TAGBOSS.Common.Model;

namespace CCI.Sys.Data
{
  public partial class DataSource : DataAccessBase
  {
    EntityAttributes _entityAttributes = null;
    // we init to null and separate out the property to instantiate on first use, to avoid design errors on format that try to connect to the config file and db when we are not at run time
    private EntityAttributes _ea { get { if (_entityAttributes == null) _entityAttributes = new EntityAttributes(); return _entityAttributes; } }
    #region general methods
    public DataAdapterContainer getMaintenanceAdapter(string tablename, Dictionary<string, string> parameters)
    {
      if (string.IsNullOrEmpty(tablename))
        throw new Exception("getMaintenanceAdapter must have a valid table name");
      bool updateable = true;
      string sql = null;
      bool validTable = false;
      bool hasAutoID = false;
      string autoIDName = "ID";
      if (parameters.ContainsKey("AutoIDName"))
      {
          autoIDName = parameters["AutoIDName"];
          hasAutoID = true;
      }
      switch (tablename.ToLower())
      {
        case "codemaster":
          if (parameters != null && parameters.Count == 1)
          {
            validTable = true;
            string codetype = parameters["CodeType"];
            sql = string.Format("Select CodeType, CodeValue, Description, StartDate, EndDate, LastModifiedBy, LastModifiedDateTime from CodeMaster where CodeType ='{0}'", codetype);
          }
          break;
        case "importCHPriceList":
          validTable = true;
          sql = "select * from importCHPriceList";
          break;
        case "opportunities":
          if (parameters != null && parameters.Count == 1)
          {
            validTable = true;
            string entity = parameters["Entity"];
            sql = string.Format("Select * from Opportunities where Customer = '{0}'", entity);
          }
          break;
        case "opportunitylineitems":
          if (parameters != null && parameters.Count == 1)
          {
            validTable = true;
            string opportunity = parameters["OpportunityID"];
            sql = string.Format("Select * from OpportunityLineItems where OpportunityID = '{0}'", opportunity);
          }
          break;
        case "products":
          if (parameters != null && parameters.Count == 1)
          {
            validTable = true;
            string criteria = string.Empty;
            if (parameters.ContainsKey("Criteria"))
              criteria = parameters["Criteria"]; sql = string.Format("select Carrier, ItemId, StartDate, EndDate, LastModifiedBy, Cost from ProductList where ItemId in (select itemid from MasterProductList where Name like '%{0}%')", criteria);
          }
          else
          {
            if (parameters == null || parameters.Count == 0)
            {
              validTable = true;
              sql = "select Carrier, ItemId, StartDate, EndDate, LastModifiedBy, Cost from ProductList";
            }
          }       
          break;
        case "masterproductlist":
          if (parameters != null && parameters.Count == 1)
          {
            validTable = true;
            string criteria = string.Empty;
            if (parameters.ContainsKey("Criteria"))
              criteria = parameters["Criteria"];
            //sql = string.Format("select * from MasterProductList where ItemId in (select masteritemid from MasterProductList where Name like '%{0}%')", criteria);
            sql = string.Format("select * from masterproductlist");
          }
          else
          {
            if (parameters == null || parameters.Count == 0)
            {
              validTable = true;
              sql = "select * from MasterProductList";
            }
          }
          break;
        case "networkinventory":
          validTable = true;
          sql = "select ID, NeedsReview, NeedsReviewComments, Location, CityCareProdOrderID,  Carrier, ItemID, CommissionID, CityCareCommProfileID, VendorAccountID, VendorInvoiceNumber, CircuitID, BTN, MRC, EMU, CommissionMRC, Quantity, StartDate, EndDate, Customer, Payor, CycleCode, CityCareNetinvID, Comments, Scrubbed from NetworkInventory";
          if (parameters != null && parameters.Count > 0)
          {
            string whereclause = string.Empty;
            bool firstTime = true;
            foreach (KeyValuePair<string, string> parm in parameters)
            {
              if (!string.IsNullOrEmpty(parm.Value))
              {
                if (firstTime)
                  firstTime = false;
                else
                  whereclause += " AND ";
                whereclause += string.Format("{0} = '{1}'", parm.Key, parm.Value);
              }
            }
            sql = string.Format("{0} where {1}", sql, whereclause);
          }
          break;
        case "searchnetworkinventory":
          if (parameters != null && parameters.Count > 0)
          {
            string whereClause = parameters["WhereClause"]; // get the where clause
            sql = string.Format("Select ID, NeedsReview, NeedsReviewComments, Location, CityCareProdOrderID,  Carrier, ItemID, CommissionID, CityCareCommProfileID, VendorAccountID, VendorInvoiceNumber, CircuitID, BTN, MRC, EMU, CommissionMRC, Quantity, StartDate, EndDate, Customer, Payor, CycleCode, CityCareNetinvID, Comments, Scrubbed from NetworkInventory WHERE {0} order by Customer, Location, ItemID ", whereClause);
            validTable = true;
            tablename = "NetworkInventory"; // reset table name to the correct table since this is a special case
          }
          break;
        case "tmp_mrcwholesaledetail":
          {
              validTable = true;
              sql = "select * from TMP_MRCWholesaleDetail where 1=0";
          }
          break;
        case "tmp_mrcretaildetail":
          {
              validTable = true;
              sql = "select * from TMP_MRCRetailDetail where 1=0";
          }
          break;
        case "tmp_otherchargesdetail":
          {
              validTable = true;
              sql = "select * from TMP_OtherChargesDetail where 1=0";
          }
          break;
          case "tmp_callwholesaledetail":
          {
              validTable = true;
              sql = "select * from TMP_CallWholesaleDetail where 1=0";
          }
          break;
          case "tmp_callwholesale800":
          {
              validTable = true;
              sql = "select * from TMP_CallWholesale800 where 1=0";
          }
          break;
          case "tmp_callretaildetail":
          {
              validTable = true;
              sql = "select * from TMP_CallRetailDetail where 1=0";
          }
          break;
          case "tmp_callretail800":
          {
              validTable = true;
              sql = "select * from TMP_CallRetail800 where 1=0";
          }
          break;
          case "prospectfollowups":
          if (parameters != null)
          {
            validTable = true;
            DateTime startDate = CommonData.PastDateTime;
            DateTime endDate = CommonData.FutureDateTime;
            string rep = string.Empty;
            if (parameters.ContainsKey("StartDate"))
              startDate = CommonFunctions.CDateTime(parameters["StartDate"]);
            if (parameters.ContainsKey("EndDate"))
              endDate = CommonFunctions.CDateTime(parameters["EndDate"]);
            string status = "active";
            if (parameters.ContainsKey("Status"))
              status = parameters["Status"];
            StringBuilder sb = new StringBuilder();
            sb.Append("select e.Entity, e.legalname [Customer Name], o.OpportunityName [Opportunity Name], o.EstimatedMRC MRC, ");
            sb.Append("ibp.LegalName IBP, o.FollowUpDate Due ");
            sb.Append("from Opportunities o inner join Entity e on o.customer = e.Entity ");
            sb.Append("left join Entity ibp on o.IBP = ibp.Entity ");
            sb.Append("where isnull(o.FollowUpDate,'{2}') between '{0}' and '{1}' ");
            sb.Append("and e.EntityType = 'Prospect' ");
            switch (status.ToLower())
            {
              case "active":
                sb.Append("and '{2}' between ISNULL(e.startdate, '{2}') and ISNULL(e.EndDate,'{2}')");
                break;
              case "inactive":
                sb.Append("and '{2}' not between ISNULL(e.startdate, '{2}') and ISNULL(e.EndDate,'{2}')");
                break;
              case "all":
                break;
            } 
            if (parameters.ContainsKey("Rep"))
              rep = parameters["Rep"];            

            sql = string.Format(sb.ToString(), startDate.ToShortDateString(), endDate.ToShortDateString(), 
              DateTime.Today.ToShortDateString());
            // TO DO we seem to be getting rep short name instead of entity id so we need to fix that
            if (!string.IsNullOrEmpty(rep))
              sql += " and o.Rep = '" + rep + "'";
            updateable = false;
            break;
          }
          break;
        case "ibpfollowups":
          if (parameters != null)
          {
            validTable = true;
            DateTime startDate = CommonData.PastDateTime;
            DateTime endDate = CommonData.FutureDateTime;
            string rep = string.Empty;
            if (parameters.ContainsKey("StartDate"))
              startDate = CommonFunctions.CDateTime(parameters["StartDate"]);
            if (parameters.ContainsKey("EndDate"))
              endDate = CommonFunctions.CDateTime(parameters["EndDate"]);
            string status = "active";
            if (parameters.ContainsKey("Status"))
              status = parameters["Status"];
            StringBuilder sb = new StringBuilder();
            sb.Append("select e.Entity, e.legalname [Customer Name], o.OpportunityName [Opportunity Name], o.FollowUpDate Due ");
            sb.Append("from Opportunities o inner join Entity e on o.customer = e.Entity ");
            sb.Append("where isnull(o.FollowUpDate,'{2}') between '{0}' and '{1}' ");
            sb.Append("and e.EntityType = 'IBPProspect' ");
            switch (status.ToLower())
            {
              case "active":
                sb.Append("and '{2}' between ISNULL(e.startdate, '{2}') and ISNULL(e.EndDate,'{2}')");
                break;
              case "inactive":
                sb.Append("and '{2}' not between ISNULL(e.startdate, '{2}') and ISNULL(e.EndDate,'{2}')");
                break;
              case "all":
                break;
            }
            if (parameters.ContainsKey("Rep"))
              rep = parameters["Rep"];

            sql = string.Format(sb.ToString(), startDate.ToShortDateString(), endDate.ToShortDateString(),
              DateTime.Today.ToShortDateString());
            // TO DO we seem to be getting rep short name instead of entity id so we need to fix that
            if (!string.IsNullOrEmpty(rep))
              sql += " and o.Rep = '" + rep + "'";
            updateable = false;
            break;
          }
          break;
        case "funnel":
          if (parameters != null)
          {
            validTable = true;
            DateTime startDate = CommonData.PastDateTime;
            DateTime endDate = CommonData.FutureDateTime;
            string rep = string.Empty;
            if (parameters.ContainsKey("StartDate"))
              startDate = CommonFunctions.CDateTime(parameters["StartDate"]);
            if (parameters.ContainsKey("EndDate"))
              endDate = CommonFunctions.CDateTime(parameters["EndDate"]);
            string status = "active";
            if (parameters.ContainsKey("Status"))
              status = parameters["Status"];
            StringBuilder sb = new StringBuilder();
            sb.Append("select e.Entity, e.legalname [Customer Name], o.OpportunityName [Opportunity Name], o.FollowUpDate Due ");
            sb.Append("from Opportunities o inner join Entity e on o.customer = e.Entity ");
            sb.Append("where isnull(o.FollowUpDate,'{2}') between '{0}' and '{1}' ");
            sb.Append("and e.EntityType in ('IBPProspect','Prospect') ");
            switch (status.ToLower())
            {
              case "active":
                sb.Append("and '{2}' between ISNULL(e.startdate, '{2}') and ISNULL(e.EndDate,'{2}')");
                break;
              case "inactive":
                sb.Append("and '{2}' not between ISNULL(e.startdate, '{2}') and ISNULL(e.EndDate,'{2}')");
                break;
              case "all":
                break;
            } 
            if (parameters.ContainsKey("Rep"))
              rep = parameters["Rep"];

            sql = string.Format(sb.ToString(), startDate.ToShortDateString(), endDate.ToShortDateString(),
              DateTime.Today.ToShortDateString());
            // TO DO we seem to be getting rep short name instead of entity id so we need to fix that
            if (!string.IsNullOrEmpty(rep))
              sql += " and o.Rep = '" + rep + "'";
            updateable = false;
            break;
          }
          break;
      }
      if (validTable)
      {
        DataAdapterContainer da = (DataAdapterContainer)getDataAdapterFromSQL(sql);
        SqlCommandBuilder builder = new SqlCommandBuilder(da.DataAdapter);
        if (updateable)
        {
          builder.GetDeleteCommand(true);
          builder.GetUpdateCommand(true);
          builder.GetInsertCommand(true);
        }
        if (da.DataSet.Tables.Count > 0)
          da.DataSet.Tables[0].TableName = tablename;
        return da;
      }
      else
        throw new Exception("getMaintenanceAdapter must have a valid table name");
    }
    public int getIDFromRowAdded(string tablename, string columnName, string lastModifiedBy, DateTime timeStamp)
    {
      int returnval = -1;
      string sql = string.Format("select MAX({0}) {0} from {1} where LastModifiedBy = '{2}' and LastModifiedDateTime = '{3}'",
        columnName, tablename, lastModifiedBy, timeStamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
      DataSet ds = getDataFromSQL(sql);
      if (ds == null) 
        return returnval;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        returnval = CommonFunctions.CInt(ds.Tables[0].Rows[0][0]);
      ds.Clear();
      ds = null;
      return returnval;
    }
    public CCITable getFieldValidationData()
    {
      /*
alter view vw_DBColumnDetail as
SELECT OBJECT_SCHEMA_NAME(T.[object_id],DB_ID()) AS [Schema],   
        AC.[name] AS [column_name], T.[name] AS [table_name],   
        TY.[name] AS system_data_type, case when ty.name = 'nvarchar' then case when ac.max_length < 0 then ac.Max_Length else AC.[max_length]/2 end when ty.name = 'date' or ty.name = 'int' or ty.name = 'datetime' then ac.precision else ac.max_length end max_length,  
        AC.[precision], AC.[scale], AC.[is_nullable], AC.[is_ansi_padded],
        ac.max_length raw_length 
FROM sys.[tables] AS T   
  INNER JOIN sys.[all_columns] AC ON T.[object_id] = AC.[object_id]  
 INNER JOIN sys.[types] TY ON AC.[system_type_id] = TY.[system_type_id] AND AC.[user_type_id] = TY.[user_type_id]   
WHERE T.[is_ms_shipped] = 0  
GO

alter view vw_DBColumnList as
SELECT     
        column_name Field,   
        system_data_type DataType, 
        Min(Max_Length) Length,
        Min(Precision) Precision,
        Min(Scale) Scale
FROM vw_DBColumnDetail  
group by Column_Name,   
        system_data_type
GO
       */
      string sql = "select * from vw_DBColumnList order by column_name ";
      DataSet ds = getDataFromSQL(sql);
      CCITable returnTable = CommonFunctions.convertDataSetToATKTable(ds);
      ds.Clear();
      ds = null;
      return returnTable;
    }
    #region Entity Data
    public void CreateTempEntityTable(string entityType, string AttributeName)
    {
        SearchResultCollection customers = SearchEntities("*", "Customer");
        StringBuilder cList = new StringBuilder();
        foreach (SearchResult cust in customers)
        {
            cList.Append(cust.EntityID);
            cList.Append(",");
        }
        cList.Length = cList.Length - 1; // elim last comma
        EntityAttributesCollection eac = getEntity(cList.ToString(), entityType, DateTime.Today);
        string sql = "Insert into tmpIBPCustomerList Values ('{0}', '{1}')";
        foreach (Entity en in eac.Entities)
        {
            string customer = en.OriginalID;
            string IBP = CommonFunctions.CString(eac.getValue(string.Format("{0}.Entity.{1}.{2}", customer,entityType, AttributeName)));
            updateDataFromSQL(string.Format(sql, customer, IBP));
        }
    }
    public void terminateEntity(string entity)
    {
      // terminate the entity by setting the EndDate to yesterday
      string sql = string.Format("Update Entity set EndDate = '{0}' Where Entity = '{1}'", 
        DateTime.Today.AddDays(-1).ToShortDateString(), entity);
      updateDataFromSQL(sql);
    }
    public SearchResultCollection SearchEntities(string criteria, string entityType)
    {
      if (criteria != null && criteria.Length > 0)
        return _ea.Search(null, null, criteria, entityType, false, false);
      else
        return new SearchResultCollection();
    }
    public SearchResultCollection SearchEntities(string criteria, string entityType, bool includeTermed)
    {
      if (criteria != null && criteria.Length > 0)
        return _ea.Search(null, null, criteria, entityType, includeTermed, false);
      else
        return new SearchResultCollection();
    }
    public SearchResultCollection SearchEntities(string criteria, string entityType, string entityOwner, bool includeTermed)
    {
      if (criteria != null && criteria.Length > 0)
        return _ea.Search(entityOwner, null, criteria, entityType, includeTermed, false);
      else
        return new SearchResultCollection();
    }
    public SearchResult getSearchResultEntity(string entity)
    {
      Entity e = _ea.Entity(entity, true);
      SearchResult s = new SearchResult();
      s.EntityID = entity;
      if (e != null)
      {
        s.LegalName = CommonFunctions.CString(e.Fields.getValue("LegalName"));
        s.EntityType = CommonFunctions.CString(e.Fields.getValue("EntityType"));
        if (e.ItemTypes.Contains("Entity"))
        {
          ItemType it = e.ItemTypes["Entity"];
          if (it.Items.Contains(s.EntityType))
          {
            Item i = it.Items[s.EntityType];
            s.FullName = CommonFunctions.CString(i.getValue("FullName"));
            s.ShortName = CommonFunctions.CString(i.getValue("ShortName"));
          }
        }
      }
      return s;
    }
    public EntityAttributesCollection getEntity(string entity, string entityType, DateTime effectiveDate)
    {
      return _ea.getAttributes(entity, "Entity", entityType, null, effectiveDate);
    }
    public EntityAttributesCollection getAttributes(string entities, string itemTypes, string items, string parameters, DateTime effectiveDate, bool getVirtualItems)
    {
      if (getVirtualItems)
        return _ea.getAttributes_withVirtualAttributes(entities, itemTypes, items, parameters, effectiveDate);
      else
        return _ea.getAttributes(entities, itemTypes, items, parameters, effectiveDate);
    }
    public EntityAttributesCollection getAttributes(string entities, string itemTypes, string items, string parameters, DateTime effectiveDate)
    {
      return _ea.getAttributes(entities, itemTypes, items, parameters, effectiveDate);
    }
    public void saveEntity(EntityAttributesCollection eac)
    {
      _ea.Save(eac, false);
    }
    public SearchResultCollection getEntityList(string entityOwner, string entityOwnerType, string entityType)
    {
      return _ea.Search(entityOwner, entityOwnerType, "*", entityType, false);
    }
    public bool ExistsEntity(string entity)
    {
      return _ea.existsEntity(entity);
    }
    public bool IsEntityField(string fieldName)
    {
      return _ea.IsEntityField(fieldName);
    }
    public string getNextNumericEntityID(string entityType)
    {
      if (entityType == null)
        return null;
      string sql = string.Format("select max(entity) from entity where entitytype = '{0}' and len(entity) = 5", entityType);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return null;
      string maxEntity = ds.Tables[0].Rows[0][0].ToString();
      if (CommonFunctions.IsInteger(maxEntity))
        return (CommonFunctions.CInt(maxEntity) + 1).ToString();
      else
        return null;
    }
    public EntityAttributesCollection getDummyEntityRecord(string entity, string entityType)
    {
      return _ea.getAttributes_withVirtualAttributes(entity, "Entity", entityType, null, DateTime.Today);
    }
    public DateTime? getClosestFollowUpDate(string entity, DateTime? fuDate)
    {
      string sql = "select o.Customer, MIN(o.FollowUpDate) from Opportunities o inner join Entity e on o.Customer = e.Entity where '{0}' between ISNULL(e.startdate, '{0}') and ISNULL(e.EndDate, '{0}') and o.Customer = '{1}' Group by o.Customer";
      sql = string.Format(sql, DateTime.Today.ToShortDateString(), entity);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return fuDate;
      DateTime? opFUdate = CommonFunctions.CDateTime(ds.Tables[0].Rows[0][1]);
      if (opFUdate == null)
        return fuDate;
      if (fuDate == null)
        return fuDate;
      if (opFUdate < fuDate)
        return opFUdate;
      return fuDate;
    }
    public string getEntityType(string entity)
    {
      string sql = string.Format("Select EntityType from Entity where Entity = '{0}'", entity);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return null;
      return CommonFunctions.CString(ds.Tables[0].Rows[0][0]);
    }
    public bool hasLocation(string entity)
    {
      return numberLocations(entity) > 0;  
    }
    public int numberLocations(string entity)
    {
      int count = 0;
      string sql = string.Format("Select count(*) NumberLocations from Entity where EntityType = 'Location' and EntityOwner = '{0}'",
        entity);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null) 
        return count;
      if ( ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        count = CommonFunctions.CInt(ds.Tables[0].Rows[0][0]);
      ds.Clear();
      ds = null;
      return count;
    }
    public void deleteOpportunityLineItems(int opportunityID)
    {
      string sql = string.Format("Delete from OpportunityLineItems where OpportunityID = {0}", opportunityID.ToString());
      updateDataFromSQL(sql);
    }
    #endregion
    #region Login
    public SecurityContext Login(SecurityContext s)
    {
      s.Security = SecurityFactory.GetInstance().getSecurity("citycare.com", s.Login, s.Password);
      EntityAttributesCollection user = _ea.getAttributes(s.Security.User, "Entity", "User", null, DateTime.Today);
      if (user != null && user.Entities.Contains(s.Security.User))
          s.UserInfo = (Item)user.getValue(string.Format("{0}.Entity.User", s.Security.User));
      logSecurity(s);
      return s;
    }
    public void SavePassword(SecurityContext s)
    {
      EncryptDecryptString encrypt = new EncryptDecryptString();
      string sql = string.Format("Update SecurityUsers set Password = '{0}' Where login = '{1}'",
        encrypt.encryptString(CommonFunctions.CString(s.NewPassword)), s.Login);
      updateDataFromSQL(sql);
      s.Password = s.NewPassword;
      s.NewPassword = null;
    }
    public string[] getUserList()
    {
      string sql = "select Login from SecurityUsers order by Login";
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0)
        return new string[0];
      else
      {
        string[] userList = new string[ds.Tables[0].Rows.Count];
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
          userList[i] = CommonFunctions.CString(ds.Tables[0].Rows[i]["Login"]);
        return userList;
      }
    }
    private void logSecurity(SecurityContext s)
    {
      string dt = DateTime.Now.ToString(CommonData.FORMATLONGDATETIME);
      string[] fields = new string[] { "SecurityID", "Account", "Timekeeper", "LoginDateTime", "LastAccessedDateTime", "SecurityAccess" };
      string[] values = new string[] { "0", s.Account, s.User, dt, dt, s.ToString() };
      long securityID = createRecordReturnID(fields, values, "createSecurityLogReturnID", "SecurityID");
      s.SecurityID = Convert.ToInt32(securityID);

      //After we create the record we need to UPDATE the record so as to UPDATE the string value for the SecurityID!!
      string[] keys = new string[] { "SecurityID" };
      fields = new string[] { "SecurityID", "SecurityAccess" };
      values = new string[] { s.SecurityID.ToString(), s.ToString() };
      updateRecord(fields, values, keys, "securityLog");
    }
    public SecurityContext getSecurity(int securityID)
    {
      string sql = string.Format("Select SecurityAccess from SecurityLog where SecurityId = {0}", securityID);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return new SecurityContext();
      string securityString = CommonFunctions.CString(ds.Tables[0].Rows[0]["SecurityAccess"]);
      SecurityContext s = new SecurityContext(securityString);
      s.SecurityID = securityID;
      return s;
    }
    #endregion

    #endregion
    #region Screens
    #region Commission Exceptions
    public DataSet getScheduledAndActualsWithDifferentTotals()
    {
      string sql = "SELECT * from vw_CustomerMatchup WHERE IsMatch = 0 ORDER BY Payor, Customer, PayCode";
      return getDataFromSQL(sql);
    }
    public DataSet getScheduledWithNoActual()
    {
      string sql = "SELECT * FROM vw_CustomerMatchupScheduledWithNoActual WHERE IsMatch = 0 ORDER BY Payor, Customer, PayCode";
      return getDataFromSQL(sql);
    }
    public DataSet getActualWithNoScheduled()
    {
      string sql = "SELECT * FROM vw_CustomerMatchupActualWithNoScheduled WHERE IsMatch = 0 ORDER BY Payor, Customer, PayCode";
      return getDataFromSQL(sql);
    }
    public DataSet getAllUnmatched()
    {
      string sql = "SELECT * from vw_CustomerMatchupAll WHERE IsMatch = 0 ORDER BY Payor, Customer, PayCode";
      return getDataFromSQL(sql);
    }
    public DataSet getActuals(string customer, string payor, string paycode)
    {
      string sql = "select * from vw_Actual where EntityType = 'Payor' and PayeeOrPayor = '{0}' and Customer = '{1}' and PayCode = '{2}' and PaymentDate between DATEADD(month, -1, getdate()) and GETDATE()";
      sql = string.Format(sql, payor, customer, paycode);
      return getDataFromSQL(sql);
    }
    public DataSet getScheduled(string customer, string payor, string paycode)
    {
      string sql = "select * from vw_Scheduled where PayeeOrPayor = 'Payor' and VendorAccountID = '{0}' and AccountNumber = '{1}' and PayCode = '{2}'";
      sql = string.Format(sql, payor, customer, paycode);
      return getDataFromSQL(sql);
    }
    #endregion
    #region Code Master
    public DataSet getCodeMaster(string codetype)
    {
      string sql = string.Format("Select * from CodeMaster where CodeType ='{0}'", codetype);
      DataSet ds = getDataFromSQL(sql);
      if (ds.Tables.Count > 0)
      {
        ds.Tables[0].TableName = "CodeMaster";
      }
      return ds;
    }
    public string[] getCodeList(string codetype)
    {
      DataSet ds = getCodeMaster(codetype);
      if (ds.Tables.Count > 0)
      {
        string[] list = new string[ds.Tables[0].Rows.Count];
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
          list[i] = CommonFunctions.CString(ds.Tables[0].Rows[i]["CodeValue"]);
        return list;
      }
      else
        return new string[0];
    }
    #endregion
    #region Login
    //public SecurityContext Login(SecurityContext s)
    //{
    //  s.Security = (Security)SecurityFactory.GetInstance().getSecurity("citycare.com", s.Login, s.Password);
    //  if (s.IsLoggedIn)
    //  {
    //    EntityAttributesCollection eac = getEntity(s.User, "User", DateTime.Today);
    //    s.UserInfo = (Item)eac.getValue(string.Format("{0}.Entity.User", s.User));
    //  }
    //  _securityContext = s;  // now save the context so we know who we are
    //  return s;
    //}
    //public void SavePassword(SecurityContext s)
    //{
    //  EncryptDecryptString encrypt = new EncryptDecryptString();
    //  string sql = string.Format("Update SecurityUsers set Password = '{0}' Where login = '{1}'", 
    //    encrypt.encryptString(CommonFunctions.CString(s.NewPassword)), s.Login);
    //  updateDataFromSQL(sql);
    //  s.Password = s.NewPassword;
    //  s.NewPassword = null;
    //}
    //public string[] getUserList()
    //{
    //  string sql = "select Login from SecurityUsers order by Login";
    //  DataSet ds = getDataFromSQL(sql);
    //  if (ds == null || ds.Tables.Count == 0)
    //    return new string[0];
    //  else
    //  {
    //    string[] userList = new string[ds.Tables[0].Rows.Count];
    //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //      userList[i] = CommonFunctions.CString(ds.Tables[0].Rows[i]["Login"]);
    //    return userList;
    //  }
    //}
    #endregion
    #region Unmatched Screens
    public DataSet getUnmatchedNames(CommonData.UnmatchedNameTypes type)
    {
      string sql = string.Empty;
      switch (type)
      {
        case CommonData.UnmatchedNameTypes.Customer:
          sql = "Select * from vw_ActualUnmatchedCustomers order by CustomerName";
          break;
        case CommonData.UnmatchedNameTypes.TrueUp:
          sql = "select distinct customernameraw from citycare_prod.dbo.vw_MasterCommissionTrueUp where customername is null order by customernameraw";
          break;
        case CommonData.UnmatchedNameTypes.ImportCustomer:
          sql = "Select * from vw_ImportUnmatchedCustomers order by CustomerName";
          break;
        case CommonData.UnmatchedNameTypes.Inventory:
          sql = "Select * from vw_UnmatchedProducts order by ProductNameRaw";
          break;
        case CommonData.UnmatchedNameTypes.Payor:
          sql = "Select VendorName from vw_UnmatchedPayors order by VendorName";
          break;
        case CommonData.UnmatchedNameTypes.CityHostedCustomer:
          sql = @"select distinct customer from mrcdetail c
left join ExternalIDMapping map on c.customer = map.ExternalID and map.EntityType = 'customer'
where map.ExternalID is null";
          break;
      }
      if (string.IsNullOrEmpty(sql))
        return null;
      else 
        return getDataFromSQL(sql);

    }
    //public DataSet getUnmatchedCustomers()
    //{
    //  string sql = "Select * from vw_ActualUnmatchedCustomers order by CustomerName";
    //  return getDataFromSQL(sql);
    //}
    public DataSet getSimilarNamesLike(CommonData.UnmatchedNameTypes type, string whereClause)
    {
      string sql = string.Empty;
      switch (type)
      {
        case CommonData.UnmatchedNameTypes.CityHostedCustomer:
        case CommonData.UnmatchedNameTypes.TrueUp:
        case CommonData.UnmatchedNameTypes.Customer:
        case CommonData.UnmatchedNameTypes.ImportCustomer:
          sql = string.Format("Select * from vw_Customers {0} ORDER BY CustomerName", whereClause);
          break;
        case CommonData.UnmatchedNameTypes.Inventory:
          sql = string.Format("Select distinct Name ItemID from Products {0} ORDER BY Name", whereClause);
          break;
        case CommonData.UnmatchedNameTypes.Payor:
          sql = string.Format("Select distinct VendorName from vw_VendorNameList {0} ORDER BY VendorName", whereClause);
          break;
      }
      if (string.IsNullOrEmpty(sql))
        return null;
      else 
        return getDataFromSQL(sql);
    }
    public DataSet getAllRawData(CommonData.UnmatchedNameTypes type, string whereClause)
    {
      if (whereClause == null)
        whereClause = string.Empty;
      string sql = string.Empty;
      switch (type)
      {
        case CommonData.UnmatchedNameTypes.ImportCustomer:
        case CommonData.UnmatchedNameTypes.Customer:
        case CommonData.UnmatchedNameTypes.TrueUp:
          sql = string.Format("select * from CommissionsRaw {0}",whereClause);
          break;
      }
      if (string.IsNullOrEmpty(sql))
        return null;
      else
        return getDataFromSQL(sql);
    }
    public SortedDictionary<string, string> getResearchFields(CommonData.UnmatchedNameTypes type)
    {
      SortedDictionary<string, string> returnList = new SortedDictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
      DataSet ds = getResearchData(type, null, 1);

      if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Columns.Count > 0)
      {
        foreach (DataColumn col in ds.Tables[0].Columns)
        {
          returnList.Add(col.Caption, col.Caption);
        }
      }    
      return returnList;
    }
    public DataSet getResearchData(CommonData.UnmatchedNameTypes type, object oWhere, int maxCount)
    {
      string where = string.Empty;
      string innerWhere = string.Empty;
      if (oWhere != null)
      {
        if (oWhere.GetType() == typeof(string))
          where = CommonFunctions.CString(oWhere);
        else
          if (oWhere.GetType() == typeof(string[]))
          {
            string[] whereClauses = (string[])oWhere;
            if (whereClauses.GetLength(0) > 0)
              where = whereClauses[0];
            if (whereClauses.GetLength(0) > 1)
              innerWhere = whereClauses[1];
          }
      }
      string topClause = string.Empty;
      string fromClause = string.Empty;
      string orderbyClause = string.Empty;
      string whereClause = string.Empty;
      if (maxCount > 0)
        topClause = string.Format(" TOP {0} ", maxCount.ToString());
      string sql = string.Empty;
      switch (type)
      {
        case CommonData.UnmatchedNameTypes.Customer:
        case CommonData.UnmatchedNameTypes.ImportCustomer:
        case CommonData.UnmatchedNameTypes.TrueUp:
          fromClause = "* from vw_MasterCustomerOrders";
          orderbyClause = " order by customername, pcity, pstate, paddressprefix, paddress";
          break;
        case CommonData.UnmatchedNameTypes.Payor:
          fromClause = "* from vw_Vendors";
          orderbyClause = " order by VendorName";
          break;
        case CommonData.UnmatchedNameTypes.Location:
          fromClause = "* from vw_MasterCustomerList";
          orderbyClause = " order by Customer, Location";
          break;
        case CommonData.UnmatchedNameTypes.UnmatchedNetworkInventory:
          // only get records from vw_NetworkInventoryMatchup (a view from CommissionsRaw) which meet the unmatched criteria defined by the where clause
          fromClause = "raw.* from (select distinct commissionid from vw_UnmatchedCommissionsRawVsNetworkInventory where commissionid  in (";
          /* 
           * this one is different since the it has both an inner where and an regular outer where
           */
          fromClause += "select distinct ni.CommissionID from NetworkInventory ni";
          if (!string.IsNullOrEmpty(innerWhere))
            fromClause += string.Format(" WHERE {0}", innerWhere);
          fromClause += ")) u inner join vw_NetworkInventoryMatchup raw on raw.CommissionID = u.CommissionID";
          orderbyClause = " order by customername, commissionid, vendor";
          break;
        case CommonData.UnmatchedNameTypes.NetworkInventory:
          fromClause = " * from vw_NetworkInventoryList";
          orderbyClause = " order by Customer, Location, ItemID ";
          break;
        case CommonData.UnmatchedNameTypes.OldNetworkInventory:
          fromClause = " * from vw_CityCareNetworkInventory";
          orderbyClause = " order by CustomerName, LocationName, ProductName ";
          break;
        case CommonData.UnmatchedNameTypes.Profiles:
          fromClause = " * from vw_OrdersProfile";
          orderbyClause = " order by CommissionID ";
          break;
        case CommonData.UnmatchedNameTypes.Orders:
          fromClause = " * from vw_CityCareOrders";
          orderbyClause = " order by CustomerName, LocationName, ProductName, ID ";
          break;
      }
      if (!string.IsNullOrEmpty(where))
      {
        string qualifier;
        if (fromClause.ToLower().Contains(" where "))
          qualifier = "AND";
        else
          qualifier = "WHERE";
        whereClause = string.Format(" {0} {1} ", qualifier, where);
      }
      if (!string.IsNullOrEmpty(fromClause))
        sql = string.Format("SELECT {0}{1}{2}{3}", topClause, fromClause, whereClause, orderbyClause);
      if (string.IsNullOrEmpty(sql))
        return null;
      else
        return getDataFromSQL(sql);

    }
    public void addExternalID(string entityType, string internalID, string externalID)
    {
      string[] keyNames = new string[] { "ExternalSourceType", "ExternalSource", "ExternalID", "EntityType" };
      string[] keyValues = new string[] { "Payor", "Payor", externalID, entityType };
      string sql;
      if (!existsRecord("ExternalIDMapping", keyNames, keyValues))
        sql = "INSERT INTO ExternalIDMapping (ExternalSourceType, ExternalSource, ExternalID, EntityType, InternalID, LastModifiedBy, LastModifiedDateTime) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
      else
        sql = "UPDATE ExternalIDMapping SET InternalID = '{4}', LastModifiedBy = '{5}', LastModifiedDateTime = '{6}' WHERE ExternalSourceType = '{0}' and ExternalSource = '{1}' and ExternalID = '{2}' and EntityType = '{3}'";
      sql = string.Format(sql, "Payor", "Payor", externalID, entityType, internalID, _securityContext.User, DateTime.Now.ToString());
      updateDataFromSQL(sql);
    }
    public bool existsRecord(string tableName, string[] keyNames, string[] keyValues)
    {
      if (keyNames == null || keyValues == null || keyNames.GetLength(0) != keyValues.GetLength(0))
        return false;
      StringBuilder sb = new StringBuilder();
      sb.Append("Select Count(*) From ");
      sb.Append(tableName);
      sb.Append(" WHERE ");
      bool firstTime = true;
      for (int i = 0; i < keyNames.GetLength(0); i++)
      {
        if (firstTime)
          firstTime = false;
        else
          sb.Append(" AND ");
        sb.Append(keyNames[i]);
        sb.Append(" = '");
        sb.Append(keyValues[i]);
        sb.Append("'");
      }
      DataSet ds = getDataFromSQL(sb.ToString());
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
      {
        ds.Clear();
        ds = null;
        return false;
      }
      int count = CommonFunctions.CInt(ds.Tables[0].Rows[0][0]);
      ds.Clear();
      ds = null;
      if (count > 0)
        return true;
      return false;

    }
    public string getMatchedID(string desc, CommonData.UnmatchedNameTypes type)
    {
      string sql = null;
      string returnID = null;
      switch (type)
      {
        case CommonData.UnmatchedNameTypes.Item:
          sql = string.Format("Select top 1 itemid from products where name = '{0}'", desc);
          break;
        case CommonData.UnmatchedNameTypes.CityHostedCustomer:
        case CommonData.UnmatchedNameTypes.Customer:
        case CommonData.UnmatchedNameTypes.Payor:
          return desc;
          break;
      }
      if (sql != null)
      {
        DataSet ds = getDataFromSQL(sql);
        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
          return null;
        returnID = CommonFunctions.CString(ds.Tables[0].Rows[0][0]);
        ds.Clear();
        ds = null;
      }
      return returnID;    
    }
    public string getCustomerIDFromCommissionID(string commissionID)
    {
      string id = null;
      if (string.IsNullOrEmpty(commissionID))
        return id;
      StringBuilder sql = new StringBuilder();
      sql.Append("select map.InternalID CustomerID from CommissionsRaw raw ");
      sql.Append("inner join ExternalIDMapping map on raw.customernameRAW = map.ExternalID and map.EntityType = 'Customer' ");
      sql.Append(string.Format("where commissionidRAW = '{0}'",commissionID));
      DataSet ds = getDataFromSQL(sql.ToString());
      if (ds == null) 
        return id;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        id = CommonFunctions.CString(ds.Tables[0].Rows[0]["CustomerID"]);
      ds.Clear();
      ds = null;
      return id;
    }
    public void AddNetworkInventoryFromCityCareData(string tableName, List<string> idList, string user, string commissionID)
    {
      string strIDList = CommonFunctions.ToList((string[])idList.ToArray());
      string sql = string.Format("exec CreateNetworkInventory '{0}', '{1}', '{2}', '{3}'",tableName, strIDList, commissionID, user);
      updateDataFromSQL(sql);
    }
    #endregion
    #region UserMaintenance
    public UserInfo getUserInfo(string entity, DateTime effectiveDate)
    {
      EncryptDecryptString encrpt = new EncryptDecryptString();
      UserInfo info = new UserInfo();
      const string aPath = "{0}.Entity.User.{1}";
      
      info.Entity = entity;
      EntityAttributesCollection eac = _ea.getAttributes(entity, "Entity", "User", null, effectiveDate);
      string sql = string.Format("Select Login, Password, Domain, Entity from SecurityUsers where Entity = '{0}'",entity);
      DataSet dsUser = getDataFromSQL(sql);
      if (eac == null || !eac.Entities.Contains(entity) || !eac.Entities[entity].ItemTypes.Contains("Entity") ||
        !eac.Entities[entity].ItemTypes["Entity"].Items.Contains("User")
        || dsUser == null || dsUser.Tables.Count == 0 || dsUser.Tables[0].Rows.Count == 0)
        return info;
      info.Login = CommonFunctions.CString(dsUser.Tables[0].Rows[0]["Login"]);
      info.Password = encrpt.decryptString(CommonFunctions.CString(dsUser.Tables[0].Rows[0]["Password"]));
      info.Domain = CommonFunctions.CString(dsUser.Tables[0].Rows[0]["Domain"]);
      Entity e = eac.Entities[entity];
      info.EntityOwner = CommonFunctions.CString(e.Fields.getValue("EntityOwner"));
      info.EntityType = "User";
      info.FirstName = CommonFunctions.CString(e.Fields.getValue("FirstName"));
      info.LastName = CommonFunctions.CString(e.Fields.getValue("LegalName"));
      info.Email = CommonFunctions.CString(eac.getValue(string.Format(aPath, entity, "EmailAddress")));
      info.OldID = CommonFunctions.CString(eac.getValue(string.Format(aPath, entity, "OldID")));
      dsUser.Clear();
      dsUser = null;
      sql = string.Format("Select top 1 SecurityGroup from SecurityGroups where Member = '{0}'", entity);
      dsUser = getDataFromSQL(sql);
      if (dsUser != null && dsUser.Tables.Count > 0 && dsUser.Tables[0].Rows.Count == 1)
        info.SecurityGroup = CommonFunctions.CString(dsUser.Tables[0].Rows[0]["SecurityGroup"]);
      return info;
    }
    public void UpdateUserInfo(UserInfo info)
    {
      if (!existsRecord("SecurityUsers", new string[] { "Entity" }, new string[] { info.Entity }))
      {
        addUserInfo(info);
        return;
      }
      DateTime efDate = DateTime.Today;
      EncryptDecryptString encrypt = new EncryptDecryptString();
      const string aPath = "{0}.Entity.User.{1}";

      EntityAttributesCollection eac = _ea.getAttributes(info.Entity, "Entity", "User", null, efDate);
      string sql = string.Format("Update SecurityUsers set Login = '{0}', Password = '{1}', Domain = '{2}' where Entity = '{3}'",
        info.Login, encrypt.encryptString(info.Password), info.Domain, info.Entity);
      updateDataFromSQL(sql);

      if (eac == null || !eac.Entities.Contains(info.Entity) || !eac.Entities[info.Entity].ItemTypes.Contains("Entity") ||
        !eac.Entities[info.Entity].ItemTypes["Entity"].Items.Contains("User"))
        return;

      Entity e = eac.Entities[info.Entity];
      e.Fields.setValue("FirstName", info.FirstName);
      e.Fields.setValue("LegalName", info.LastName);
      e.Fields.setValue("EntityOwner", info.EntityOwner);
      eac.setValue("OldID",string.Format(aPath, info.Entity, info.OldID));
      eac.setValue("EmailAddress", string.Format(aPath, info.Entity, info.Email));
      _ea.Save(eac);
    }
    private void addUserInfo(UserInfo info)
    {
      /*
       create procedure addNewUser (@Login varchar(50), @Password varchar(50), @SecurityGroup varchar(50), @Entity varchar(50), @FirstName varchar(50), @LastName varchar(50), 
@Email varchar(128), @OldID varchar(12), @UserType varchar(50), @EntityOwner varchar(50), @LastModifiedBy varchar(50) = 'Axon-User') 
       */
      string sqlFormat = "Exec addNewUser '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}'";
      EncryptDecryptString encrypt = new EncryptDecryptString();
      string sql = string.Format(sqlFormat, info.Login, encrypt.encryptString(info.Password), info.SecurityGroup, info.Entity, info.FirstName, info.LastName,
        info.Email, info.OldID, info.EntityType, info.EntityOwner, _securityContext.User, DateTime.Today.ToString());
      updateDataFromSQL(sql);
    }
    public string[] getSecurityGroups()
    {
      DataSet ds = getDataFromSQL("Select distinct SecurityGroup from SecurityGroups order by SecurityGroup");
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return new string[0];
      DataTable table = ds.Tables[0];
      string[] list = new string[table.Rows.Count];
      for (int i = 0; i < table.Rows.Count; i++)
        list[i] = CommonFunctions.CString(table.Rows[i][0]);
      return list;

    }
    #endregion
    #region Products
    public string[] getItemCategories()
    {
      string sql = "Select * from ItemCategories";
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
            sql = "Select ItemSubCategory from ItemSubCategories";
        else
            sql = string.Format("Select ItemSubCategory from ItemSubCategories where ItemCategory = '{0}'", itemCategory);
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
      string sql = "select ItemID, Name ItemDescription, Carrier, e.LegalName CarrierDescription from Products p left join Entity e on p.carrier = e.entity";
      if (!string.IsNullOrEmpty(carrier))
        sql += string.Format(" where p.carrier = '{0}'",carrier);
      sql += " order by ItemID, Carrier";
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return list;
      if (ds.Tables.Count > 0 || ds.Tables[0].Rows.Count > 0)
      {
        foreach (DataRow row in ds.Tables[0].Rows)
        {
 //         ItemListEntry item = new ItemListEntry(CommonFunctions.CString(row["ItemID"]),
 //                                               CommonFunctions.CString(row["ItemDescription"]),
 //                                               CommonFunctions.CString(row["Carrier"]),
 //                                               CommonFunctions.CString(row["CarrierDescription"]),
 //                                               CommonFunctions.CString(row["ItemSubCategory"]));
 //         list.Add(item);
        }
      }
      ds.Clear();
      ds = null;
      return list;
    }
    #endregion
    #endregion
  }
}
