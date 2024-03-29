﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using TAGBOSS.Common;

namespace CCI.Sys.Data
{
  public class AttributeDB : DataAccessBase
  {
    #region module data
    const string c_MODIFIED_STATUS = "Status";
    const string c_MODIFIED_STATUS_FAILURE = "0";
    public const string p_Entity = "Entity";
    private string p_EntityList = "EntityList";
    private string p_ItemType = "ItemType";
    private string p_ItemTypeList = "ItemTypeList";
    private string p_ItemList = "ItemList";
    private string p_EffectiveDate = "EffectiveDate";
    private string p_Item = "Item";
    private string p_tblAttributes = "e_Attributes";
    private string p_tblEntities = "e_Entities";
    private string p_getAttributesByDateSp = "GetEntityAttributesByDate";
    private string sp_getEntitiesItemTypesFromAttributes = "getEntitiesItemTypesFromAttributes";
    private string sp_getEntitiesItemTypesItemsFromAttributes = "getEntitiesItemTypesItemsFromAttributes";
    public const string p_spEntity = "getEntity";
    private string p_spAttributeUpdate = "UpdateAttribute";
    private string p_spgetAttribute = "getAttribute";
    private const string ATTRIBUTETABLENAME = "AttributeNew";
    #endregion module data

    #region constructors
    /// <summary>
    /// Constructor that creates and then shares a single DB connection
    /// </summary>
    public AttributeDB()
    {
    }
    #endregion constructors

    #region public methods
    /// <summary>
    /// Get all records from e_Attribute for an Entity and ItemType that match the EffectiveDate
    /// </summary>
    /// <param name="Entities"></param>
    /// <param name="ItemTypes"></param>
    /// <returns></returns>
    public DataSet getEntityAttributesByDate(string entity, string itemType, DateTime effectiveDate)
    {
      DataSet datasetReturn = new DataSet();
      SqlCommand command = createCommand();
      SqlParameter e = new SqlParameter(p_Entity, SqlDbType.VarChar);
      SqlParameter i = new SqlParameter(p_ItemType, SqlDbType.VarChar);
      SqlParameter d = new SqlParameter(p_EffectiveDate, SqlDbType.DateTime);

      // Assign stored procedure parameter values
      e.Value = entity;
      i.Value = itemType;
      d.Value = effectiveDate;

      // Construct the stored procedure command instance
      command.CommandType = CommandType.StoredProcedure;
      command.CommandText = p_getAttributesByDateSp;
      command.Parameters.Add(e);
      command.Parameters.Add(i);
      command.Parameters.Add(d);

      // Execute the command and return the values in a DataSet instance
      datasetReturn.Load(command.ExecuteReader(), LoadOption.PreserveChanges, tables);
      sqlConnection.Close();

      return datasetReturn;
    }
    /// <summary>
    /// Returns a single row from e_Attribute
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    /// <param name="effectiveDate"></param>
    /// <returns></returns>
    public DataSet getEntityAttributesByItem(string entity, string itemType, string item, DateTime effectiveDate)
    {
      return getEntityAttributesByItem(entity, itemType, item, effectiveDate, false);
    }
    /// <summary>
    /// Overload which allows specification of All Items. This will return all items that match Entity/ItemType/Item, regardless
    /// of Start/End Dates
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    /// <param name="effectiveDate"></param>
    /// <param name="allItems"></param>
    /// <returns></returns>
    public DataSet getEntityAttributesByItem(string entity, string itemType, string item, 
      DateTime effectiveDate, bool getHistory)
    {
      /*
       * Neede tup upgrade this to allow for null=all for itemTypes and Items. getEntitiesItemTypesItems already did this, so
       * just reformatted the parms and call it instead
       */
      string[] entities = FromList(entity);
      string[] itemTypes = FromList(itemType);
      string[] items = FromList(item);
      if (getHistory)
        return getEntitiesItemTypesItems(entities, itemTypes, items, null, false);
      else
        return getEntitiesItemTypesItems(entities, itemTypes, items, effectiveDate, false);
    }
    /// <summary>
    /// Get a list of Entity/ItemType combinations from e_Attribute(New)
    /// where the Entity is in EntityList (parameter entities) and the ItemType is in ItemTypeList (parameter itemTypes)
    /// </summary>
    /// <param name="entities">String Array of Entities. null means "*" or "All"</param>
    /// <param name="itemTypes">String Array of ItemTypes. null means "*" or "All"</param>
    /// <returns></returns>
    public DataSet getEntitiesItemTypes(string[] entities, string[] itemTypes)
    {
      return getEntitiesItemTypesItems(entities, itemTypes, null, null, false);
    }
    /// <summary>
    /// Get attribute data using an entity or entity list and an optional list of
    /// ItemTypes and Items. Also optionally select by an EffectiveDate (if none is 
    /// specified, then the dataset it not selected by EffectiveDate).
    /// </summary>
    /// <param name="entities">String array that contains one or more Entities</param>
    /// <param name="itemTypes">String array that contains zero or more ItemTypes</param>
    /// <param name="items">String array that contains zero or more Items</param>
    /// <param name="effectiveDate">An effectiveDate, or null if no date is to be used to restrict the data</param>
    /// <returns></returns>
    public DataSet getEntitiesItemTypesItems(string[] entities, string[] itemTypes, 
      string[] items, object effectiveDate, bool includeConditionalDefaults)
    {
      string entityList = null;
      if (entities != null && entities.GetLength(0) > 0)
        entityList = toList(entities);
      string itemTypeList = null;
      if (itemTypes != null && itemTypes.GetLength(0) > 0)
        itemTypeList = toList(itemTypes);
      string itemList = null;
      if (items != null && items.GetLength(0) > 0)
        itemList = toList(items);
      string efDate = null;
      if (effectiveDate != null)
        efDate = effectiveDate.ToString();
      string mySQL = buildSQL(entityList, itemTypeList, itemList, efDate, includeConditionalDefaults);
      return getDataFromSQL(mySQL);
    }
    public DataSet getEntitiesItemTypesItemsNonXML(string[] entities, string[] itemTypes,
      string[] items, string[] attributes, object effectiveDate, bool includeConditionalDefaults)
    {
      string entityList = null;
      if (entities != null && entities.GetLength(0) > 0)
        entityList = toList(entities);
      string itemTypeList = null;
      if (itemTypes != null && itemTypes.GetLength(0) > 0)
        itemTypeList = toList(itemTypes);
      string itemList = null;
      if (items != null && items.GetLength(0) > 0)
        itemList = toList(items);
      string attributeList = null;
      if (attributes != null && attributes.GetLength(0) > 0)
        attributeList = toList(attributes);
      string mySQL = buildSQLNonXML(entityList, itemTypeList, itemList, attributeList, effectiveDate, includeConditionalDefaults);
      return getDataFromSQL(mySQL);
    }
    public DataSet getDictionaryItemsNonXML(object effectiveDate)
    {
      string mySQL = buildDictionarySQLNonXML(effectiveDate);
      return getDataFromSQL(mySQL);
    }
    private string buildDictionarySQLNonXML(object effectiveDate)
    {
      DateTime efDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, effectiveDate, DateTime.Today);
      string sql = "select 'Dictionary' Entity, i.ItemType, i.item, a.Attribute, a.ValueType, a.Value, null ItemStartDate, null ItemEndDate, ";
      sql += "i.LastModifiedBy ItemLastModifiedBy, i.LastModifiedDateTime ItemLastModifiedDateTime, a.StartDate AttributeStartDate, ";
      sql += "a.EndDate AttributeEndDate, a.LastModifiedBy, a.LastModifiedDateTime ";
      sql += "from DictionaryItem i left join DictionaryAttribute a on i.ItemType = a.ItemType and i.Item = a.Item ";
      sql += "where '" + efDate.ToShortDateString() + "' between isnull(a.startdate, '" + TAGFunctions.PastDateTime.ToShortDateString() + "') and ";
      sql += "isnull(a.enddate, '" + TAGFunctions.FutureDateTime.ToShortDateString() + "')";
      return sql;
    }
    /// <summary>
    /// Retrieves one record from TAGAttribute
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    public DataSet get_Attribute(string entity, string itemType, string item)
    {
      DataSet datasetReturn = new DataSet();
      SqlCommand command = createCommand();
      SqlParameter parmEntity = new SqlParameter(p_Entity, SqlDbType.VarChar);
      SqlParameter parmItemType = new SqlParameter(p_ItemType, SqlDbType.VarChar);
      SqlParameter parmItem = new SqlParameter(p_Item, SqlDbType.VarChar);
      // Assign stored procedure parameter values
      parmEntity.Value = entity;
      parmItemType.Value = itemType;
      parmItem.Value = item;

      // Construct the stored procedure command instance
      command.CommandType = CommandType.StoredProcedure;
      command.CommandText = p_spgetAttribute;
      command.Parameters.Add(parmEntity);
      command.Parameters.Add(parmItemType);
      command.Parameters.Add(parmItem);
      if (inTransaction)
        command.Transaction = thisTransaction;
      // Execute the command and return the values in a DataSet instance
      datasetReturn.Load(command.ExecuteReader(), LoadOption.PreserveChanges, tables);
      sqlConnection.Close();

      return datasetReturn;
    }
    /// <summary>
    /// Update a single record of TAGAttribute. If it exists, update it. If not, add it.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    /// <param name="startdate"></param>
    /// <param name="endDate"></param>
    /// <param name="attributes"></param>
    /// <param name="Code"></param>
    /// <param name="lastModifiedBy"></param>
    /// <param name="lastModifiedDateTime"></param>
    public void UpdateAttribute(string entity, string itemType, string item, string itemHistory,
                                string attributes, string lastModifiedBy, Object lastModifiedDateTime)
    {
      SqlCommand command = createCommand();
      SqlParameter parmEntity = new SqlParameter(p_Entity, SqlDbType.VarChar);
      SqlParameter parmItemType = new SqlParameter(p_ItemType, SqlDbType.VarChar);
      SqlParameter parmItem = new SqlParameter(p_Item, SqlDbType.VarChar);
      SqlParameter parmItemHistory = new SqlParameter("ItemHistory", SqlDbType.VarChar);
      SqlParameter parmAttributes = new SqlParameter("Attributes", SqlDbType.VarChar);
      SqlParameter parmLastModifiedBy = new SqlParameter("LastModifiedBy", SqlDbType.VarChar);
      SqlParameter parmLastModifiedDateTime = new SqlParameter("LastModifiedDateTime", SqlDbType.DateTime);
      // Assign stored procedure parameter values
      parmEntity.Value = entity;
      parmItemType.Value = itemType;
      parmItem.Value = item;
      parmItemHistory.Value = itemHistory;
      parmAttributes.Value = attributes;
      if (lastModifiedBy == null)
        parmLastModifiedBy.Value = string.Empty;
      else
        parmLastModifiedBy.Value = lastModifiedBy;
      parmLastModifiedDateTime.Value = lastModifiedDateTime;

      // Construct the stored procedure command instance
      command.CommandType = CommandType.StoredProcedure;
      command.CommandText = p_spAttributeUpdate;
      command.Parameters.Add(parmEntity);
      command.Parameters.Add(parmItemType);
      command.Parameters.Add(parmItem);
      command.Parameters.Add(parmItemHistory);
      command.Parameters.Add(parmAttributes);
      command.Parameters.Add(parmLastModifiedBy);
      command.Parameters.Add(parmLastModifiedDateTime);
      if (inTransaction)
      {
        command.Transaction = thisTransaction;
        command.ExecuteNonQuery();
      }
      else
      {
        //BeginTransaction();
        command.Transaction = thisTransaction;
        command.ExecuteNonQuery();
        //Commit();
      }
      sqlConnection.Close();
    }
    ///// <summary>
    ///// Get a single Entity record from the Entity table.
    ///// </summary>
    ///// <param name="entity"></param>
    ///// <returns>Dataset with a single entity record</returns>
    //public DataSet getEntity(string entity)
    //{
    //  /* 
    //   * Note that this is a duplicate from EntitiesDB, but the AttributeData cannot
    //   * get this from Entities, so we just put it here too
    //   */
    //  return getDataSetUsingOneID(entity, p_Entity, p_spEntity);
    //}

    public DataSet getDataFromSQL(string mySQL)
    {
      return base.getDataFromSQL(mySQL);
    }
    public void DeleteItem(string pEntity, string pItemType, string pItem) 
    {
      if (pEntity != string.Empty && pItemType != string.Empty && pItem != string.Empty)
      {
        SqlCommand command = createCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = string.Format("DELETE FROM Attribute WHERE entity = '{0}' AND itemType = '{1}' AND item = '{2}'", pEntity, pItemType, pItem);
        command.Transaction = thisTransaction;
        command.ExecuteNonQuery();
        sqlConnection.Close();
      }
    }
    public string[] getItemTypesRaw(string pEntity)
    {
      string sql = string.Format("select distinct ItemType from Attribute where Entity = '{0}'", pEntity);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return new string[0];
      string[] returnList = new string[ds.Tables[0].Rows.Count];
      int iRow = 0;
      foreach (DataRow row in ds.Tables[0].Rows)
        returnList[iRow++] = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row["ItemType"]);
      return returnList;
    }
    public string[] getItemsRaw(string pEntity, string pItemType)
    {
      string sql = string.Format("select distinct Item from Attribute where Entity = '{0}' and ItemType = '{1}'", pEntity, pItemType);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return new string[0];
      string[] returnList = new string[ds.Tables[0].Rows.Count];
      int iRow = 0;
      foreach (DataRow row in ds.Tables[0].Rows)
        returnList[iRow++] = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row["Item"]);
      return returnList;
    }
    #endregion public methods

    #region private methods
    private static string buildSQLNonXML(string entitylist, string itemtypelist, string itemlist, string attributeList, object effectivedate, bool includeConditionalDefault)
    {
      string SQLString = string.Format("select i.Entity, i.ItemType, i.item, a.Attribute, a.ValueType, a.Value, ih.StartDate ItemStartDate, ih.EndDate ItemEndDate, " +
        "ih.LastModifiedBy ItemLastModifiedBy, ih.LastModifiedDateTime ItemLastModifiedDateTime, a.StartDate AttributeStartDate, a.EndDate AttributeEndDate, " +
        "a.LastModifiedBy, a.LastModifiedDateTime " +
        "from Item i " +
        "left join ItemHistory ih on i.Entity = ih.Entity and i.ItemType = ih.ItemType and i.Item = ih.Item " +
        "left join {0} a on i.Entity = a.Entity and i.ItemType = a.ItemType and i.Item = a.Item ", ATTRIBUTETABLENAME);
      string[] ConnectorString = new string[] {"", "", "", "", ""};
      string ORDERBYString = "order by i.Entity, i.ItemType, i.Item, a.Attribute";
      string EntityString = "";
      string ItemTypeString = "";
      string ItemString = "";
      string AttributeString = "";
      string ConditionalItemString = "";
      string effectiveDateString = "";
      int Number = 0;
      const string ANDSTRING = " AND ";
      if (null != entitylist && entitylist.Trim().Length > 0)
      {
        EntityString = " i.Entity IN (" + entitylist + ") ";
        Number++;
      }
      if (null != itemtypelist && itemtypelist.Trim().Length > 0)
      {
        ItemTypeString = " i.ItemType IN (" + itemtypelist + ") ";
        Number++;
      }
      if (null != itemlist && itemlist.Trim().Length > 0)
      {
        ItemString = " i.Item IN (" + itemlist + ") ";
        Number++;
      }
      if (includeConditionalDefault && ItemString.ToLower().Contains("default"))
      {
        ConditionalItemString = " i.Item like 'Default(%' ";
        if (ItemString == "")
        {
          ItemString = ConditionalItemString;
          Number++;
        }
        else
          ItemString = string.Format(" ({0} OR {1}) ", ItemString, ConditionalItemString); 
      }
      if (null != attributeList && attributeList.Trim().Length > 0)
      {
        AttributeString = " a.Attribute IN (" + attributeList + ") ";
        Number++;
      }
      if (null != effectivedate)
      {
        effectiveDateString = string.Format( " ( '{0}' between isnull(ih.StartDate, '{1}') and isnull(ih.EndDate, '{2}') )", 
          ((DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, effectivedate)).ToShortDateString(), 
          TAGFunctions.PastDateTime.ToShortDateString(), TAGFunctions.FutureDateTime.ToShortDateString());
        Number++;
      }
      if (Number > 0)
        ConnectorString[0] = " WHERE ";
      for (int i = 1; i <= Number - 1; i++)
        ConnectorString[i] = ANDSTRING;
      int connectorIndex = 0;
      SQLString = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}", SQLString, ConnectorString[connectorIndex++], 
        EntityString, ConnectorString[connectorIndex++], ItemTypeString, ConnectorString[connectorIndex++], 
        ItemString, ConnectorString[connectorIndex++], AttributeString, ConnectorString[connectorIndex++], 
        effectiveDateString, ORDERBYString);
      return SQLString;
    }
    private static string buildSQL(string entitylist, string itemtypelist, string itemlist, string effectivedate, bool includeConditionalDefault)
    {
      string SQLString = "Select a.* from Attribute a";
      string SQLStringEnd = "";
      string ANDString = "";
      string ANDString2 = "";
      string ORDERBYString = "";
      string EntityString = "";
      string ItemTypeString = "";
      string ItemString = "";
      string ConditionalItemString = "";
      int Number = 0;

      if (null != entitylist && entitylist.Trim().Length > 0)
      {
        EntityString = " Entity IN (" + entitylist + ") ";
        Number++;
      }
      if (null != itemtypelist && itemtypelist.Trim().Length > 0)
      {
        ItemTypeString = " ItemType IN (" + itemtypelist + ") ";
        Number++;
      }
      if (null != itemlist && itemlist.Trim().Length > 0)
      {
        if (includeConditionalDefault)
          ItemString = " (Item IN (" + itemlist + ") OR Item like 'Default(%' )";
        else
        ItemString = " Item IN (" + itemlist + ") ";

        Number++;
      }
      
      if (null != effectivedate && effectivedate.Trim().Length > 0)
      {
        SQLString = "SELECT a.* FROM Attribute a inner join (Select distinct Entity, ItemType, Item from(select Entity, ItemType, Item, ih.history.query('.').value('(/ItemHistory/@startdate)[1]', 'datetime') StartDate, ih.history.query('.').value('(/ItemHistory/@enddate)[1]', 'datetime') EndDate from attribute outer apply itemhistory.nodes('/ItemDates/ItemHistory') ih(history) where (ih.history.query('.').value('(/ItemHistory/@startdate)[1]', 'datetime') is null or ih.history.query('.').value('(/ItemHistory/@startdate)[1]', 'datetime') <= '" + effectivedate + "') and (ih.history.query('.').value('(/ItemHistory/@enddate)[1]', 'datetime') is null or ih.history.query('.').value('(/ItemHistory/@enddate)[1]', 'datetime') >= '" + effectivedate + "' ) ";
        SQLStringEnd = ") b ) c ON a.Entity = c.Entity and a.ItemType = c.ItemType and a.Item = c.Item ";
      }

      if (Number > 1)
        ANDString = " And ";

      if (Number > 2)
        ANDString2 = " And ";

      if (Number > 0)
      {
        if (null == effectivedate || effectivedate.Trim().Length <= 0)
          SQLString = SQLString + " WHERE ";
        else
          SQLString = SQLString + " AND ";
      }
      ORDERBYString = " ORDER BY a.Entity, a.ItemType, a.Item";

      //TODO: Must convert this to a StringBuilder instead!
      SQLString = SQLString + EntityString + ANDString + ItemTypeString + ANDString2 + ItemString + SQLStringEnd + ORDERBYString;

      return SQLString;
    }
    #endregion private methods
  }
}

