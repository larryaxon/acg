﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using TAGBOSS.Common;
using TAGBOSS.Common.Model;

namespace CCI.Sys.Data
{
  /// <summary>
  /// 
  /// </summary>
  public class EntityDB : DataAccessBase
  {
    //private string p_tblEntities = "Entities";
    private string p_tblEntities = "Entity";
    private string p_spEntities = "getEntitiesByType";
    private string p_spEntity = "getEntity";
    private string p_spEntitiesWhere = "getEntitiesWhere";
    //private string p_spEntityChildren = "getEntityChildren";
    private string p_spEntityChildrenExcludeTypes = "getEntityChildrenExcludeEntityTypes";
    private string p_spEntityUpdate = "UpdateEntity";
    //private string p_connectionName = "";
    private string p_Entity = "Entity";
    private Dictionary<string, string> entityFieldList = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

    public EntityDB()
    {
      loadEntityFields();
    }
    private void loadEntityFields()
    {
      string sql = "select top 1 * from Entity";
      DataSet ds = getDataFromSQL(sql);
      if (ds != null && ds.Tables.Count > 0)
      {
        foreach (DataColumn col in ds.Tables[0].Columns)
          entityFieldList.Add(col.ColumnName, null);
      }
    }
    public bool IsEntityField(string fieldName)
    {
      return entityFieldList.ContainsKey(fieldName);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    public void SaveEntityRecord(Entity entity)
    {
      UpdateRecord(entity);
      // TODO: convert this to sql and make it more generic. Also add middlename, altid, altname
      //SqlCommand command = this.sqlConnection.CreateCommand();
      //SqlParameter parmEntity = new SqlParameter(p_Entity, SqlDbType.VarChar);
      //SqlParameter parmEntityType = new SqlParameter("EntityType", SqlDbType.VarChar);
      //SqlParameter parmEntityOwner = new SqlParameter("EntityOwner", SqlDbType.VarChar);
      //SqlParameter parmLegalName = new SqlParameter("LegalName", SqlDbType.VarChar);
      //SqlParameter parmFirstName = new SqlParameter("FirstName", SqlDbType.VarChar);
      //SqlParameter parmFEIN = new SqlParameter("FEIN", SqlDbType.VarChar);
      //SqlParameter parmStartDate = new SqlParameter("StartDate", SqlDbType.DateTime);
      //SqlParameter parmEndDate = new SqlParameter("EndDate", SqlDbType.DateTime);
      //SqlParameter parmLastModifiedBy = new SqlParameter("LastModifiedBy", SqlDbType.VarChar);
      //SqlParameter parmLastModifiedDateTime = new SqlParameter("LastModifiedDateTime", SqlDbType.DateTime);
      //// Assign stored procedure parameter values
      //parmEntity.Value = entity.OriginalID;
      //parmEntityType.Value = setField(entity.Fields, "EntityType");
      //parmEntityOwner.Value = setField(entity.Fields, "EntityOwner");
      //parmLegalName.Value = setField(entity.Fields, "LegalName");
      //parmFirstName.Value = setField(entity.Fields, "FirstName");
      //parmFEIN.Value = setField(entity.Fields, "FEIN");
      //parmStartDate.Value = setField(entity.Fields,"StartDate");
      //parmEndDate.Value = setField(entity.Fields, "EndDate");
      ////parmEndDate.Value = DateTime.Now;
      //parmLastModifiedBy.Value = setField(entity.Fields,"LastModifiedBy");
      //object lastModifiedDateTime = entity.Fields["LastModifiedDateTime"].Value;
      //if (lastModifiedDateTime != null)
      //  parmLastModifiedDateTime.Value = lastModifiedDateTime;
      //else
      //  parmLastModifiedDateTime.Value = DateTime.Now;
      //// Construct the stored procedure command instance
      //command.CommandType = CommandType.StoredProcedure;
      //command.CommandText = p_spEntityUpdate;
      //command.Parameters.Add(parmEntity);
      //command.Parameters.Add(parmEntityType);
      //command.Parameters.Add(parmEntityOwner);
      //command.Parameters.Add(parmLegalName);
      //command.Parameters.Add(parmFirstName);
      //command.Parameters.Add(parmFEIN);
      //command.Parameters.Add(parmStartDate);
      //command.Parameters.Add(parmEndDate);
      //command.Parameters.Add(parmLastModifiedBy);
      //command.Parameters.Add(parmLastModifiedDateTime);
      //if (inTransaction)
      //{
      //  command.Transaction = thisTransaction;
      //  command.ExecuteNonQuery();
      //}
      //else
      //{
      //  BeginTransaction();
      //  command.Transaction = thisTransaction;
      //  command.ExecuteNonQuery();
      //  Commit();
      //}
    }
    private object setField(FieldsCollection flds, string fieldName)
    {
      object val = null; // default is the DB null
      if (flds.Contains(fieldName))     // if the fieldname exists in the collection
      {
        val = flds[fieldName].Value;    // then get its value
        if (val == null || val == System.DBNull.Value)                // if it is c# null, convert to dbNull
          val = System.DBNull.Value;
        else
          if (!((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsDateTime, val)))    // if it is a datetime, leave it along
            val = flds[fieldName].Value.ToString(); // otherwise, convert to a string
      }
      else
        val = System.DBNull.Value;
      return val;
    }
    public void UpdateRecord(Entity entity)
    {
      // First, we find out how many "real" fields we have
      int nbrFields = entity.Fields.Count;
      foreach (Field f in entity.Fields)
        if (f.Virtual)
          nbrFields--;
      string[] fields = new string[nbrFields];
      string[] values = new string[nbrFields];
      string[] keyvalues = new string[1];
      string[] keyfields = { "Entity" };
      int i = 0;
      foreach (Field f in entity.Fields)
      {
        if (!f.Virtual)
        {
          fields[i] = f.OriginalID;
          values[i] = "'" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, f.Value, string.Empty) + "'";
          i++;
        }
      }
      keyvalues[0] = entity.OriginalID;
      if (existsEntity(entity.OriginalID))
      {
        updateRecord(fields, values, keyfields, "Entity");
      }
      else
      {
        insertRecord("Entity", fields, values);
      }
    }
    /// <summary>
    /// Returns a list of entities from a comma-delimited list
    /// </summary>
    /// <param name="entityList"></param>
    /// <returns></returns>
    public DataSet getEntities(string entityList)
    {
      string[] arrEntities = FromList(entityList);
      string entitiesIN = toList(arrEntities);
      string whereClause = " Entity IN (" + entitiesIN + ")";
      return getEntitiesWhere(whereClause);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="WhereClause"></param>
    /// <returns></returns>
    public DataSet getEntitiesWhere(string WhereClause)
    {
      return getDataSetUsingOneID(WhereClause, "WhereClause", p_spEntitiesWhere);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Entity"></param>
    /// <returns></returns>
    public DataSet getEntity(string Entity)
    {
      return getDataSetUsingOneID(Entity, "Entity", p_spEntity);
    }
    /// <summary>
    /// returns true if the entity exists in the entity table, and false if it does not
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool existsEntity(string entity)
    {
      string mySQL = "select entity from entity where entity = '"+ entity + "' ";
      DataSet ds = getDataFromSQL(mySQL);
      return (ds.Tables.Count !=0 && ds.Tables[0].Rows.Count != 0);
    }
    /// <summary>
    /// returns true if the FEIN exists in the entity table, and false if it does not
    /// </summary>
    /// <param name="fein"></param>
    /// <returns></returns>
    public bool existsFEIN(string fein)
    {
      string mySQL = "select entity from entity where FEIN = '" + fein + "' ";
      DataSet ds = getDataFromSQL(mySQL);
      return (ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0);
    }
    /// <summary>
    /// returns the entity from the DB if it does not exists, returns empty string
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public string getEntityFromDB(string entity)
    {
      string mySQL = "select entity from entity where entity = '" + entity + "' ";
      DataSet ds = getDataFromSQL(mySQL);
      if (ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0) 
        return ds.Tables[0].Rows[0]["entity"].ToString();
      else
        return "";
    }

    /// <summary>
    /// returns the itemType from the DB if it does not exists, returns empty string
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <returns></returns>
    public string getItemTypeFromDB(string entity, string itemType)
    {
      string mySQL = "select distinct itemType from attribute where entity = '" + entity + "' and itemType = '" + itemType +"'";
      DataSet ds = getDataFromSQL(mySQL);
      if (ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
        return ds.Tables[0].Rows[0]["itemType"].ToString();
      else
        return "";
    }

    /// <summary>
    /// returns the item from the DB if it does not exists, returns empty string
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <returns></returns>
    public string getItemFromDB(string entity, string itemType, string item)
    {
      string mySQL = "select distinct item from attribute where entity = '" + entity + "' and itemType = '" + itemType + "' and item = '" + item + "'";
      DataSet ds = getDataFromSQL(mySQL);
      if (ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
        return ds.Tables[0].Rows[0]["item"].ToString();
      else
        return "";
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="Entity"></param>
    ///// <returns></returns>
    //public DataSet getEntityChildren(string Entity, DateTime effectiveDate)
    //{
    //  return getEntityChildren(Entity, null, effectiveDate, false);
    //}
    public DataSet getDataFromSQL(string sql)
    {
      return base.getDataFromSQL(sql);
    }
    public DataSet searchEntities(string criteria, DateTime effectiveDate, string entityType, string entityOwner, bool includeTermed)
    {
      int maxResults = TAGFunctions.MAXENTITYSEARCHRESULTS;
      if (string.IsNullOrEmpty(criteria))
        maxResults = 0;
      bool hasWhere = false;
      StringBuilder sbSQL = new StringBuilder();
      sbSQL.Append("Select ");
      if (maxResults >= 0)
      {
        sbSQL.Append(" Top ");
        sbSQL.Append(maxResults.ToString());
        sbSQL.Append(" ");
      }
      sbSQL.Append("* FROM Entity ");
      if (!includeTermed)
      {
        sbSQL.Append("WHERE '");
        sbSQL.Append(effectiveDate.ToShortDateString());
        sbSQL.Append("' BETWEEN isnull(StartDate,'");
        sbSQL.Append(effectiveDate.ToShortDateString());
        sbSQL.Append("') and isnull(EndDate,'");
        sbSQL.Append(effectiveDate.ToShortDateString());
        sbSQL.Append("') ");
        hasWhere = true;
      }
      if (!string.IsNullOrEmpty(criteria) && criteria != "*")
      {
        if (hasWhere)
          sbSQL.Append(" AND ");
        else
        {
          sbSQL.Append(" WHERE ");
          hasWhere = true;
        }
        sbSQL.Append(" (Entity like '%");
        sbSQL.Append(criteria);
        sbSQL.Append("%' OR LegalName like '%");
        sbSQL.Append(criteria);
        sbSQL.Append("%' OR FirstName like '%");
        sbSQL.Append(criteria);
        sbSQL.Append("%')");
      }
      if (!string.IsNullOrEmpty(entityType))
      {
        if (hasWhere)
          sbSQL.Append(" AND ");
        else
        {
          sbSQL.Append(" WHERE ");
          hasWhere = true;
        }
        sbSQL.Append(" EntityType = '");
        sbSQL.Append(entityType);
        sbSQL.Append("'");
      }
      if (!string.IsNullOrEmpty(entityOwner))
      {
        if (hasWhere)
          sbSQL.Append(" AND ");
        else
        {
          sbSQL.Append(" WHERE ");
          hasWhere = true;
        }
        sbSQL.Append(" entityOwner = '");
        sbSQL.Append(entityOwner);
        sbSQL.Append("'");
      }
      sbSQL.Append(" ORDER BY LegalName, FirstName");
      return getDataFromSQL(sbSQL.ToString());
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Entity"></param>
    /// <param name="EntityTypesToExclude"></param>
    /// <returns></returns>
    public DataSet getEntityChildren(string Entity, ArrayList EntityTypesToExclude, DateTime effectiveDate, bool includeTerms, bool includePath)
    {
      return getEntityChildren(Entity, EntityTypesToExclude, effectiveDate, includeTerms, includePath, null);
    }
    public DataSet getEntityChildren(string Entity, ArrayList EntityTypesToExclude, DateTime effectiveDate, bool includeTerms, bool includePath, string entityType)
    {
      string EntityTypeList = "";
      int nbrEntityTypes, i;
      //DataSet dsEntities = new DataSet();
      if (EntityTypesToExclude == null)
        nbrEntityTypes = 0;
      else
        nbrEntityTypes = EntityTypesToExclude.Count;
      for (i = 0; i < nbrEntityTypes; i++)
      {
        if (EntityTypeList.Length == 0)
          EntityTypeList = "(";
        else
          EntityTypeList = EntityTypeList + ", ";
        EntityTypeList = EntityTypeList + "'" + EntityTypesToExclude[i].ToString() + "'";
      }
      if (EntityTypeList.Length != 0)
        EntityTypeList = EntityTypeList + ")";

      //Here we build the SQL string to be evaluated...
      string SQLstring = "";
      if (includePath && TAGFunctions.UseEntityPath)
        SQLstring = string.Format("Select TOP {0} *, dbo.fn_EntityPath(entity) EntityPath From Entity e ",
          TAGFunctions.MAXENTITYSEARCHRESULTS.ToString());
      else
        SQLstring = string.Format("Select TOP {0} * from Entity e ",TAGFunctions.MAXENTITYSEARCHRESULTS.ToString());
      if(Entity == null)
      {
        if(EntityTypeList != null && EntityTypeList != "")
          SQLstring += " WHERE e.EntityType not in " + EntityTypeList;
      }
      else
      {
          SQLstring += " WHERE e.EntityOwner = '" + Entity + "'";
          if (EntityTypeList != null && EntityTypeList != "")
            SQLstring += " and e.EntityType not in " + EntityTypeList;
      }
      if (!string.IsNullOrEmpty(entityType))
      {
        if (SQLstring.Contains("WHERE"))
          SQLstring += " and ";
        else
          SQLstring += " WHERE ";
        SQLstring += "  e.EntityType = '" + entityType + "'";
      }
      if (!includeTerms)
      {
        if (SQLstring.Contains("WHERE"))
          SQLstring += " and '";
        else
          SQLstring += " WHERE '";
        SQLstring += effectiveDate.ToShortDateString() + "' Between COALESCE(e.StartDate, '";
        SQLstring += TAGFunctions.PastDateTime.ToShortDateString() + "') and COALESCE(e.EndDate, '";
        SQLstring += TAGFunctions.FutureDateTime.ToShortDateString() + "')";
      }
      SQLstring += " order by LegalName, FirstName";
      return getDataFromSQL(SQLstring);

      //SqlCommand command = this.sqlConnection.CreateCommand();
      //SqlParameter en = new SqlParameter("Entity", SqlDbType.VarChar);
      //SqlParameter etype = new SqlParameter("EntityTypeList", SqlDbType.VarChar);
      //en.Value = Entity;
      //if (EntityTypeList.Length != 0)
      //{
      //  EntityTypeList = EntityTypeList + ")";
      //  etype.Value = EntityTypeList;
      //}
      //else
      //{
      //  etype.Value = global::System.DBNull.Value;
      //}
      //command.CommandType = CommandType.StoredProcedure;
      //command.CommandText = p_spEntityChildrenExcludeTypes;
      //command.Parameters.Add(en);
      //command.Parameters.Add(etype);
      //dsEntities.Load(command.ExecuteReader(), LoadOption.PreserveChanges, tables);
      //return dsEntities;


      /*
      -- sp_getEntityChildrenExcludeEntityTypes 'TAGSB', '(''Bank'', ''Vendor'', ''Prospect'')'
      -- =============================================
      -- Author:		<Author,,Name>
      -- Create date: <Create Date,,>
      -- Description:	<Description,,>
      -- =============================================
      ALTER PROCEDURE [dbo].[getEntityChildrenExcludeEntityTypes]
        -- Add the parameters for the stored procedure here
        @Entity nvarchar(max),
        @EntityTypeList nvarchar(max)
      AS
      BEGIN
        DECLARE @SQLstring nvarchar(max)
          -- Insert statements for procedure here
        if isnull(@Entity, 'NONE') = 'NONE'
          begin
            set @SQLstring = 'Select *  from Entity e'
            if not isnull(@EntityTypeList, 'NONE') = 'NONE'
              set @SQLstring = @SQLstring + ' WHERE e.EntityType not in ' + @EntityTypeList
          end
        else
          begin
            set @SQLstring = 'SELECT *  from Entity e WHERE e.EntityOwner = ''' + @Entity + ''''
            if not isnull(@EntityTypeList, 'NONE') = 'NONE'
              set @SQLstring = @SQLstring + ' and e.EntityType not in ' + @EntityTypeList
          end
        set @SQLstring = @SQLstring + ';'
        exec (@SQLstring);
      END
       */
    
    }
    public DataSet searchEntities(string searchString, string entityType)
    {
      string likestring = " like '%" + searchString + "%'";
      string strWhere = "LegalName " + likestring + " or FEIN " + likestring + " or FirstName " + likestring;
      strWhere += " and EntityType = '" + entityType + "'";
      return getEntitiesWhere(strWhere);
    }
    public DataSet getEntityTypes()
    {
      string sql = "select distinct EntityType from Entity";
      return getDataFromSQL(sql);
    }
    public string Entity
    {
      get { return p_Entity; }
      set { p_Entity = value; }
    }
    public string tblEntities
    {
      get { return p_tblEntities; }
    }
  }
}
