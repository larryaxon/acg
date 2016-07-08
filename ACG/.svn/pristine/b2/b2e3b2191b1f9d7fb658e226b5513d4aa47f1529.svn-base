using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using ACG.Common;
using ACG.App.Common;
using ACG.Sys.SecurityEngine;

namespace ACG.Sys.Data
{
  public partial class DataSource : DataAccessBase
  {

    #region general methods

    public void deleteOpportunityLineItems(int opportunityID)
    {
      string sql = string.Format("DELETE FROM OpportunityLineItems WHERE OpportunityID = {0}", opportunityID.ToString());
      updateDataFromSQL(sql);
    }
    public DateTime? getClosestFollowUpDate(string entity, DateTime? fuDate)
    {
      string sql = 
        @"SELECT o.Customer, MIN(o.FollowUpDate) 
            FROM Opportunities o INNER JOIN Entity e 
            ON o.Customer = e.Entity 
            WHERE '{0}' BETWEEN IsNull(e.startdate, '{0}') 
              AND ISNULL(e.EndDate, '{0}') AND o.Customer = '{1}' 
            GROUP BY o.Customer";
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
    public PickListEntries getCodeListDescriptions(string codetype)
    {
      PickListEntries list = new PickListEntries();
      DataSet ds = getCodeMaster(codetype);
      if (ds.Tables.Count > 0)
      {
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
          string description = CommonFunctions.CString(ds.Tables[0].Rows[i]["Description"]);          
          string code = CommonFunctions.CString(ds.Tables[0].Rows[i]["CodeValue"]);
          list.Add(new PickListEntry(code, description));
        }
        return list;
      }
      else
        return list;
    }
    #endregion

    #region Unmatched Screens
    public DataSet getUnmatchedNames(ACGCommonData.NameTypes type)
    {
      string sql = string.Empty;
      switch (type)
      {
        case ACGCommonData.NameTypes.Customer:
          sql = "Select * from Customers order by Name";
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
    public DataSet getSimilarNamesLike(ACGCommonData.NameTypes type, string whereClause)
    {
      string sql = string.Empty;
      switch (type)
      {
        case ACGCommonData.NameTypes.Customer:
          sql = string.Format("Select * from Customers {0} ORDER BY Name", whereClause);
          break;
      }
      if (string.IsNullOrEmpty(sql))
        return null;
      else 
        return getDataFromSQL(sql);
    }
    public DataSet getAllRawData(ACGCommonData.NameTypes type, string whereClause)
    {
      if (whereClause == null)
        whereClause = string.Empty;
      string sql = string.Empty;
      switch (type)
      {
        case ACGCommonData.NameTypes.Customer:
          sql = string.Format("select * from Customer {0}",whereClause);
          break;
      }
      if (string.IsNullOrEmpty(sql))
        return null;
      else
        return getDataFromSQL(sql);
    }
    public SortedDictionary<string, string> getResearchFields(ACGCommonData.NameTypes type)
    {
      SortedDictionary<string, string> returnList = new SortedDictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
      DataSet ds;
        ds = getResearchData(type, null, 1);

      if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Columns.Count > 0)
      {
        foreach (DataColumn col in ds.Tables[0].Columns)
        {
          returnList.Add(col.Caption, col.Caption);
        }
      }    
      return returnList;
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
      sql = string.Format(sql, "Payor", "Payor", externalID.Replace("'","''"), entityType, internalID, _securityContext.User, DateTime.Now.ToString());
      updateDataFromSQL(sql);
    }
    public string getMatchedID(string desc, ACGCommonData.NameTypes type)
    {
      string sql = null;
      string returnID = null;
      switch (type)
      {
        case ACGCommonData.NameTypes.Customer:
          return desc;
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
    #endregion
    #region UserMaintenance
    public UserInfo getUserInfo(string entity, DateTime effectiveDate)
    {
      EncryptDecryptString encrpt = new EncryptDecryptString();
      UserInfo info = new UserInfo();
      
      info.Entity = entity;
      string sql = string.Format("Select Login, Password, Domain, Entity from SecurityUsers where Entity = '{0}'",entity);
      DataSet dsUser = getDataFromSQL(sql);
      //if (eac == null || !eac.Entities.Contains(entity) || !eac.Entities[entity].ItemTypes.Contains("Entity") ||
      //  !eac.Entities[entity].ItemTypes["Entity"].Items.Contains("User")
      //  || dsUser == null || dsUser.Tables.Count == 0 || dsUser.Tables[0].Rows.Count == 0)
      //  return info;
      info.Login = CommonFunctions.CString(dsUser.Tables[0].Rows[0]["Login"]);
      info.Password = encrpt.decryptString(CommonFunctions.CString(dsUser.Tables[0].Rows[0]["Password"]));
      info.Domain = CommonFunctions.CString(dsUser.Tables[0].Rows[0]["Domain"]);
      //info.EntityOwner = CommonFunctions.CString(e.Fields.getValue("EntityOwner"));
      info.EntityType = "User";
      //info.FirstName = CommonFunctions.CString(e.Fields.getValue("FirstName"));
      //info.LastName = CommonFunctions.CString(e.Fields.getValue("LegalName"));
      //info.Email = CommonFunctions.CString(eac.getValue(string.Format(aPath, entity, "EmailAddress")));
      //info.OldID = CommonFunctions.CString(eac.getValue(string.Format(aPath, entity, "OldID")));
      //info.UserType = CommonFunctions.CString(eac.getValue(string.Format(aPath, entity, "UserType")));
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
      string sql = string.Format("Update SecurityUsers set Login = '{0}', Password = '{1}', Domain = '{2}' where Entity = '{3}'",
        info.Login, encrypt.encryptString(info.Password), info.Domain, info.Entity);
      updateDataFromSQL(sql);

      //const string aPath = "{0}.Entity.User.{1}";
      //EntityAttributesCollection eac = _ea.getAttributes(info.Entity, "Entity", "User", null, efDate);
      //if (eac == null || !eac.Entities.Contains(info.Entity) || !eac.Entities[info.Entity].ItemTypes.Contains("Entity") ||
      //  !eac.Entities[info.Entity].ItemTypes["Entity"].Items.Contains("User"))
      //  return;

      //Entity e = eac.Entities[info.Entity];
      //e.Fields.setValue("FirstName", info.FirstName);
      //e.Fields.setValue("LegalName", info.LastName);
      //e.Fields.setValue("EntityOwner", info.EntityOwner);
      //eac.setValue(info.Email, string.Format(aPath, info.Entity, "EmailAddress"));
      //eac.setValue(info.UserType, string.Format(aPath, info.Entity, "UserType"));
      //_ea.Save(eac);
    }
    private void addUserInfo(UserInfo info)
    {
      /*
       create procedure addNewUser (@Login varchar(50), @Password varchar(50), @SecurityGroup varchar(50), @Entity varchar(50), @FirstName varchar(50), @LastName varchar(50), 
@Email varchar(128), @OldID varchar(12), @UserType varchar(50), @EntityOwner varchar(50), @LastModifiedBy varchar(50) = 'Axon-User') 
       */
      //string entityFormat = "{0}{1}-{2}";
      //int seq = 1;
      //while (ExistsEntity(info.Entity)) // make sure the entity id is unique
      //{
      //  info.Entity = string.Format(entityFormat, info.LastName, seq.ToString(), info.UserType);
      //  seq++;
      //}
      //string sqlFormat = "Exec addNewUser '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}'";
      //EncryptDecryptString encrypt = new EncryptDecryptString();
      //string sql = string.Format(sqlFormat, info.Login, encrypt.encryptString(info.Password), info.SecurityGroup, info.Entity, info.FirstName, info.LastName,
      //  info.Email, info.OldID, info.EntityType, info.EntityOwner, _securityContext.User, DateTime.Today.ToString());
      //updateDataFromSQL(sql);
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

    #endregion
  }
}
