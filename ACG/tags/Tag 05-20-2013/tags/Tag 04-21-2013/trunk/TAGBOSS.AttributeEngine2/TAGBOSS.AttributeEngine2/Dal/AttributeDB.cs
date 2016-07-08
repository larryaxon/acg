using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using TAGBOSS.Common;

namespace TAGBOSS.Sys.AttributeEngine2.Dal
{
  public class AttributeDB
  {
    string[] tables = { "" };
    string cmdText = "";
    const int commandTimeout = 600;

    public const string DICTIONARYENTITY = TAGFunctions.DICTIONARY;
    public const string ITEMTABLE = "Item";
    public const string HISTORYTABLE = "ItemHistory";
    public const string ATTRIBUTETABLE = "AttributeNew";
    public const string DICTIONARYITEMTABLE = "DictionaryItem";
    public const string DICTIONARYATTRIBUTETABLE = "DictionaryAttribute";

    private SqlConnection connection = null;

    private SqlConnection sqlConnection
    {
      get
      {
        if (connection == null)
          connection = SQLConnectionFactory.GetInstance().Connection;

        if (connection.State == ConnectionState.Closed)
          connection.Open();

        return connection;
      }
    }

    public DataSet getData(string[] entities, string[] itemTypes, DateTime effectiveDate, bool IsRawMode)
    {
      SqlConnection c = SQLConnectionFactory.GetInstance().Connection;
      SqlCommand cmd = new SqlCommand(cmdText, c);

      string[] paramEntities = new string[entities.Length];
      string[] paramItemTypes = null;

      if (itemTypes != null)
        paramItemTypes = new string[itemTypes.Length + 1];
      else
        paramItemTypes = null;

      for (int i = 0; i < entities.Length; i++)
      {
        paramEntities[i] = "@e" + i;
        cmd.Parameters.AddWithValue(paramEntities[i], entities[i]);
      }

      if (paramItemTypes != null)
      {
        for (int i = 0; i <= itemTypes.Length; i++)
        {
          paramItemTypes[i] = "@it" + i;
          cmd.Parameters.AddWithValue(paramItemTypes[i], (i < itemTypes.Length ? itemTypes[i] : "entity"));
        }
      }

      cmd.Parameters.Add(new SqlParameter("@effectiveDate", SqlDbType.DateTime));
      cmd.Parameters["@effectiveDate"].Value = effectiveDate.ToShortDateString();

      cmdText =
          @"WITH EntityHierarchy (Entity, EntityOwner, Level)
              AS
              (
              -- Anchor member definition
                SELECT Entity, EntityOwner, 0 As Level
                FROM Entity WHERE Entity In (";
      cmdText += string.Join(",", paramEntities) + ")";

      if (!IsRawMode)
      {
        cmdText +=
          @"
              --  WHERE EntityOwner = @entities
              UNION ALL
              -- Recursive member definition
                SELECT e.Entity, e.EntityOwner, Level+1
                FROM Entity AS e
                INNER JOIN EntityHierarchy AS eh
                    ON eh.EntityOwner = e.Entity";
      }

      cmdText +=
               @")

                SELECT 
                lower(a.Entity) As Entity, a.Entity As EntityOrigId, 
                lower(c.EntityOwner) As EntityOwner, c.EntityOwner As EntityOwnerOrigId, 
                lower(a.ItemType) As ItemType, a.ItemType As ItemTypeOrigId, 
                lower(a.Item) as Item, a.Item As ItemOrigId, 
                a.ItemHistory, a.Attributes, a.LastModifiedBy, a.LastModifiedDateTime, 

                CASE CAST(attributes.query('count(//attribute[@name = ""BirthDate"" or @name = ""Address1""])') as varchar)
                      WHEN '0' THEN NULL
                      ELSE 
                        CAST('<attributes>' + CAST(attributes.query('//attribute[@name = ""BirthDate"" or @name = ""Address1""]') as varchar(max)) + '</attributes>' as xml)	
                END AS filteredAttributes,
                0 as Virtual,
                c.Level
                FROM Attribute a 
                INNER JOIN 
                (Select distinct b.Entity, ItemType, Item, d.EntityOwner, d.Level from(select Entity, ItemType, Item, ih.history.query('.').value('(/ItemHistory/@startdate)[1]', 'datetime') StartDate, 
                ih.history.query('.').value('(/ItemHistory/@enddate)[1]', 'datetime') EndDate from attribute outer apply itemhistory.nodes('/ItemDates/ItemHistory') ih(history) 
                where (ih.history.query('.').value('(/ItemHistory/@startdate)[1]', 'datetime') is null 
                or ih.history.query('.').value('(/ItemHistory/@startdate)[1]', 'datetime') <= @effectiveDate) 
                and (ih.history.query('.').value('(/ItemHistory/@enddate)[1]', 'datetime') is null 
                or ih.history.query('.').value('(/ItemHistory/@enddate)[1]', 'datetime') >= @effectiveDate)";
      
      if (paramItemTypes != null)
      {
        cmdText += @"AND  ItemType IN (";
        cmdText += string.Join(",", paramItemTypes) + ")";
      }
      cmdText += 
            @") b 
                  JOIN (SELECT Entity, EntityOwner, Level FROM EntityHierarchy) d
                  ON (b.Entity = d.Entity)
                ) c 
                ON a.Entity = c.Entity and a.ItemType = c.ItemType and a.Item = c.Item  
                ORDER BY c.Level DESC, a.Entity, a.ItemType, a.Item";

      cmd.CommandType = CommandType.Text;
      cmd.CommandText = cmdText;

      DataSet ds = new DataSet();
      ds.Load(cmd.ExecuteReader(), LoadOption.PreserveChanges, tables);
    
      c.Close();

      return ds;

    }

    public DataSet getEntityFields(string[] entities) 
    {
      string[] paramEntities = new string[entities.Length];
      SqlConnection c = SQLConnectionFactory.GetInstance().Connection;
      SqlCommand cmd = c.CreateCommand();
      DataSet ds = new DataSet();

      for (int i = 0; i < entities.Length; i++)
      {
        paramEntities[i] = "@e" + i;
        cmd.Parameters.AddWithValue(paramEntities[i], entities[i]);
      }

      cmdText = 
          @"WITH EntityHierarchy (Entity, EntityOwner, Level)
              AS
                (
                -- Anchor member definition
                  SELECT Entity, EntityOwner, 0 As Level
                  FROM Entity
                  WHERE Entity In (";
      cmdText += string.Join(",", paramEntities) + ")";
      cmdText +=
             @"--  WHERE EntityOwner = @entities
                  UNION ALL
                -- Recursive member definition
                  SELECT e.Entity, e.EntityOwner, Level+1
                  FROM Entity AS e
                  INNER JOIN EntityHierarchy AS eh
                      ON eh.EntityOwner = e.Entity
                )

                SELECT Distinct e.Entity, e.EntityType, e.EntityOwner, e.LegalName, 
                      e.FirstName, e.AlternateID, e.MiddleName, e.AlternateName, 
                      e.FEIN, e.Address1, e.Address2, e.City, e.State, e.Zip, 
                      e.StartDate, e.EndDate, 
                      e.LastModifiedBy, e.LastModifiedDateTime, eh.Level
                FROM Entity AS e

                JOIN (SELECT Entity, EntityOwner, Level FROM EntityHierarchy) eh
                ON (e.Entity = eh.Entity)

              Order by Level Desc";

      cmd.CommandType = CommandType.Text;
      cmd.CommandText = cmdText;

      ds.Load(cmd.ExecuteReader(), LoadOption.PreserveChanges, tables);
      c.Close();

      return ds;
    }

    public DataSet getSystemData() 
    {
      SqlConnection c = SQLConnectionFactory.GetInstance().ConnectionBigPacket;
      SqlCommand cmd = c.CreateCommand();
      DataSet ds = new DataSet();

      cmdText =
          @"(SELECT 
                  lower(Entity) As Entity, Entity As EntityOrigID, 
                  '' As EntityOwner, 
	                lower(ItemType) As ItemType, ItemType As ItemTypeOrigId, 
	                lower(Item) As Item, Item As ItemOrigId,
                  ItemHistory, Attributes, 
                  LastModifiedBy, LastModifiedDateTime,
	                0 As Virtual, 
	                0 As Level
                 FROM Attribute
                 WHERE Entity = 'Default'
              ) 
              UNION ALL

              (SELECT 
                  lower(Entity) As Entity, Entity As EntityOrigID, 
                  '' As EntityOwner, 
	                lower(ItemType) As ItemType, ItemType As ItemTypeOrigId, 
	                lower(Item) As Item, Item As ItemOrigId,
                  ItemHistory, Attributes, 
                  LastModifiedBy, LastModifiedDateTime,
	                0 As Virtual, 
	                0 As Level
                 FROM Attribute
                 WHERE Entity = 'Dictionary'
              ) 
              ORDER BY Entity, ItemType, Item";

      cmd.CommandType = CommandType.Text;
      cmd.CommandText = cmdText;

      ds.Load(cmd.ExecuteReader(), LoadOption.PreserveChanges, tables);
      c.Close();

      return ds;
    }

    /// <summary>
    /// Delete Item History Records
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    public void DeleteItemHistory(string entity, string itemType, string item)
    {
      if (entity.Equals(DICTIONARYENTITY, StringComparison.CurrentCultureIgnoreCase)) // dictinary tables have no item history
        return;
      string sql = string.Empty;
      int sequence = 0;
      string condition = string.Empty;
      bool hasCondition = item.StartsWith("Default(", StringComparison.CurrentCultureIgnoreCase);
      if (hasCondition)
      {
        sequence = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, item.Substring(8, 0));
        condition = item.Substring(10, item.Length - 11);
      }
      if (hasCondition)
        sql = string.Format("delete from {0} where ItemID in (select distinct ItemID from Item where Entity = '{1}' and ItemType = '{2}' and Item = '{3}' and Sequence = {4} and Condition = '{5}')", 
          HISTORYTABLE, entity, itemType, item, sequence, condition);
      else
        sql = string.Format("delete from {0} where ItemID in (select distinct ItemID from Item where Entity = '{1}' and ItemType = '{2}' and Item = '{3}')",
          HISTORYTABLE, entity, itemType, item);
      updateDataFromSQL(sql);
    }
    /// <summary>
    /// Delete all Attributes for an Item
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    public void DeleteAttributes(string entity, string itemType, string item)
    {
      string sql;
      if (entity.Equals(DICTIONARYENTITY, StringComparison.CurrentCultureIgnoreCase))
        sql = string.Format("delete from {0} where ItemType = '{2}' and Item = '{3}'",
          DICTIONARYATTRIBUTETABLE, entity, itemType, item);
      else
      {
        int sequence = 0;
        string condition = string.Empty;
        bool hasCondition = item.StartsWith("Default(", StringComparison.CurrentCultureIgnoreCase);
        if (hasCondition)
        {
          sequence = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, item.Substring(8, 0));
          condition = item.Substring(10, item.Length - 11);
        }
        if (hasCondition)
          sql = string.Format("delete from {0} where ItemID in (select distinct ItemID from Item where Entity = '{1}' and ItemType = '{2}' and Item = '{3}' and Sequence = {4} and Condition = '{5}')",
            ATTRIBUTETABLE, entity, itemType, item, sequence, condition);
        else
          sql = string.Format("delete from {0} where ItemID in (select distinct ItemID from Item where Entity = '{1}' and ItemType = '{2}' and Item = '{3}')",
            ATTRIBUTETABLE, entity, itemType, item);
      }
      updateDataFromSQL(sql);
    }
    /// <summary>
    /// Delete all attribute value history for an attribute
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    /// <param name="attribute"></param>
    public void DeleteAttributes(string entity, string itemType, string item, string attribute)
    {
      string sql;
      if (entity.Equals(DICTIONARYENTITY, StringComparison.CurrentCultureIgnoreCase))
        sql = string.Format("delete from {0} Where ItemType = '{2}' and Item = '{3}' and Attribute = '{4}'",
          DICTIONARYATTRIBUTETABLE, entity, itemType, item, attribute);
      else
      {
        int sequence = 0;
        string condition = string.Empty;
        bool hasCondition = item.StartsWith("Default(", StringComparison.CurrentCultureIgnoreCase);
        if (hasCondition)
        {
          sequence = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, item.Substring(8, 0));
          condition = item.Substring(10, item.Length - 11);
        }
        if (hasCondition)
          sql = string.Format("delete from {0} where ItemID in (select Distinct ItemID from Item where Entity = '{1}' and ItemType = '{2}' and Item = '{3}' and Sequence = {5} and Condition = '{6}') and Attribute = '{4}'",
            ATTRIBUTETABLE, entity, itemType, item, attribute, sequence, condition);
        else
          sql = string.Format("delete from {0} where ItemID in (select Distinct ItemID from Item where Entity = '{1}' and ItemType = '{2}' and Item = '{3}') and Attribute = '{4}'",
            ATTRIBUTETABLE, entity, itemType, item, attribute);
      }
      updateDataFromSQL(sql);
    }
    /// <summary>
    /// Delete all Item, Attribute, and ItemHistory records for an item
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    public void DeleteItem(string entity, string itemType, string item)
    {
      if (Exists(ITEMTABLE, entity, itemType, item)) // Don't bother if it isn't there. Also, Exists() automatically adjusts the table name if it is for the dictionary
      {
        int sequence = 0;
        string condition = string.Empty;
        bool hasCondition = item.StartsWith("Default(", StringComparison.CurrentCultureIgnoreCase);
        if (hasCondition)
        {
          sequence = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, item.Substring(8, 0));
          condition = item.Substring(10, item.Length - 11);
        }
        string sql;
        string sqlBase;
        string[] tableList;
        if (entity.Equals(DICTIONARYENTITY, StringComparison.CurrentCultureIgnoreCase))
        {
          tableList = new string[] { DICTIONARYITEMTABLE, DICTIONARYATTRIBUTETABLE };
          sqlBase = "Delete from {0} where ItemType = '{2}' and Item = '{3}'";
        }
        else
        {
          tableList = new string[] { HISTORYTABLE, ATTRIBUTETABLE, ITEMTABLE };
          if (hasCondition)
            sqlBase = "Delete from {0} where ItemID in (select distinct ItemID from Item where Entity = '{1}' and ItemType = '{2}' and Item = '{3}')";
          else
            sqlBase = "Delete from {0} where ItemID in (select distinct ItemID from Item where Entity = '{1}' and ItemType = '{2}' and Item = '{3}' and Sequence = {4} and Condition = '{5}')";
        }
        foreach (string table in tableList)
        {
          if (table.Equals(ITEMTABLE) || table.Equals(DICTIONARYITEMTABLE) || Exists(table, entity, itemType, item))  // don't go back to the db to check to see if Item is there, since we already did that
          {
            if (hasCondition)
              sql = string.Format(sqlBase, table, entity, itemType, item, sequence, condition);
            else
              sql = string.Format(sqlBase, table, entity, itemType, item);
            updateDataFromSQL(sql);
          }
        }
      }
    }
    /// <summary>
    /// Save a specific item history record. This assumes the record is not present
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="user"></param>
    /// <param name="lastModifiedDateTime"></param>
    public void SaveItemHistory(string entity, string itemType, string item, DateTime startDate, DateTime endDate, string user, DateTime lastModifiedDateTime)
    {
      if (entity.Equals(DICTIONARYENTITY, StringComparison.CurrentCultureIgnoreCase)) // Dictionary has no ItemHistory
        return;
      string sql;
      int sequence = 0;
      string condition = string.Empty;
      bool hasCondition = item.StartsWith("Default(", StringComparison.CurrentCultureIgnoreCase);
      if (hasCondition)
      {
        sequence = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, item.Substring(8, 0));
        condition = item.Substring(10, item.Length - 11);
      }
      string strStartDate;
      string strEndDate;
      strStartDate = string.Format("'{0}'", startDate.ToShortDateString());
      if (endDate == TAGFunctions.FutureDateTime)
        strEndDate = "null";
      else
        strEndDate = string.Format("'{0}'", endDate.ToShortDateString());
      if (hasCondition)
        sql = string.Format("insert into ItemHistory (ItemID, StartDate, EndDate, LastModifiedBy, LastModifiedDateTime) select ItemID, {3}, {4}, '{5}', '{6}' from Item where Entity = '{0}' and ItemType = '{1}' and Item = '{2}' and Sequence = {7} and Condition = '{8}')",
          entity, itemType, item, strStartDate, strEndDate, user, lastModifiedDateTime.ToString(), sequence, condition);
      else
        sql = string.Format("insert into ItemHistory (ItemID, StartDate, EndDate, LastModifiedBy, LastModifiedDateTime) select ItemID, {3}, {4}, '{5}', '{6}' from Item where Entity = '{0}' and ItemType = '{1}' and Item = '{2}'",
          entity, itemType, item, strStartDate, strEndDate, user, lastModifiedDateTime.ToString());

      updateDataFromSQL(sql);
    }
    /// <summary>
    /// Save an Item Record. This works whether the item is already there or not
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    /// <param name="user"></param>
    public void SaveItem(string entity, string itemType, string item, string user)
    {
      string sql;
      string sqlBase;
      string tableName;
      int sequence = 0;
      string condition = string.Empty;
      bool hasCondition = item.StartsWith("Default(", StringComparison.CurrentCultureIgnoreCase);
      if (hasCondition)
      {
        sequence = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, item.Substring(8, 0));
        condition = item.Substring(10, item.Length - 11);
      }
      bool isDictionary = entity.Equals(DICTIONARYENTITY, StringComparison.CurrentCultureIgnoreCase);
      if (isDictionary)
        tableName = DICTIONARYITEMTABLE;
      else
        tableName = ITEMTABLE;
      if (Exists(ITEMTABLE, entity, itemType, item))  // Exists() adjusts the tablename if it is for the Dictionary
      {
        if (isDictionary)
          sqlBase = "update {0} set LastModifiedBy = '{1}', LastModifiedDateTime = '{2}' where ItemType = '{4}' and Item = '{5}'";
        else
        {
          if (hasCondition)
            sqlBase = "update {0} set LastModifiedBy = '{1}', LastModifiedDateTime = '{2}', sequence = {6}, condition = '{7}' where Entity = '{3}' and ItemType = '{4}' and Item = '{5}'";
          else
            sqlBase = "update {0} set LastModifiedBy = '{1}', LastModifiedDateTime = '{2}' where Entity = '{3}' and ItemType = '{4}' and Item = '{5}'";
        }
      }
      else
      {
        if (isDictionary)
          sqlBase = "insert into {0} (ItemType, Item, LastModifiedBy, LastModifiedDateTime) Values ('{4}', '{5}', '{1}', '{2}')";
        else
        {
          if (hasCondition)
            sqlBase = "insert into {0} (Entity, ItemType, Item, Sequence, Condition, LastModifiedBy, LastModifiedDateTime) Values ('{3}', '{4}', '{5}', {6}, '{7}', '{1}', '{2}')";
          else
            sqlBase = "insert into {0} (Entity, ItemType, Item, LastModifiedBy, LastModifiedDateTime) Values ('{3}', '{4}', '{5}', '{1}', '{2}')";
        }
      }
      if (hasCondition)
        sql = string.Format(sqlBase, tableName, user, DateTime.Now.ToString(), entity, itemType, item, sequence, condition);
      else
        sql = string.Format(sqlBase, tableName, user, DateTime.Now.ToString(), entity, itemType, item);
      updateDataFromSQL(sql);
    }
    /// <summary>
    /// Save a specific Attribute/ValueHistory record. Unique key is Entity.ItemType.Item.Attribute.StartDatem, except for Dictinary which has no Entity
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    /// <param name="attribute"></param>
    /// <param name="valuetype"></param>
    /// <param name="value"></param>
    /// <param name="startDate"></param>
    /// <param name="oEndDate"></param>
    /// <param name="user"></param>
    /// <param name="lastModifiedDateTime"></param>
    public void SaveAttribute(string entity, string itemType, string item, string attribute, string valuetype, object value, 
      DateTime startDate, object oEndDate, string user, DateTime lastModifiedDateTime)
    {
      string sValueType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, valuetype); ;
      string sValue = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, value);
      string sql = string.Empty;
      string sqlBase;
      string strEndDate;
      int sequence = 0;
      string condition = string.Empty;
      bool hasCondition = item.StartsWith("Default(", StringComparison.CurrentCultureIgnoreCase);
      if (hasCondition)
      {
        sequence = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, item.Substring(8, 0));
        condition = item.Substring(10, item.Length - 11);
      }
      if (oEndDate == null || typeof(DateTime) != oEndDate.GetType())
        strEndDate = "is null";
      else
        strEndDate = string.Format(" = '{0}'", ((DateTime)oEndDate).ToShortDateString());    
      string tableName;
      bool isDictionary = entity.Equals(DICTIONARYENTITY, StringComparison.CurrentCultureIgnoreCase);
      if (isDictionary)
        tableName = DICTIONARYATTRIBUTETABLE;
      else
        tableName = ATTRIBUTETABLE;
      if (ExistsAttribute(entity, itemType, item, attribute, startDate))
      {
        sqlBase = "update {0} set ValueType = '{1}', Value = '{2}', EndDate {3}, LastModifiedBy = '{4}', LastModifiedDateTime = '{5}' ";
        if (isDictionary)
          sqlBase += " Where ItemType = '{7}' and Item = '{8}' and Attribute = '{9}' and StartDate = '{10}'";
        else
        {
          if (hasCondition)
          {
            sqlBase += " Where ItemID in (select distinct ItemID from Item where Entity = '{6}' and ItemType = '{7}' and Item = '{8}' and sequence = {11} and condition = '{12}') and Attribute = '{9}' and StartDate = '{10}'";
            sql = string.Format(sqlBase, tableName, sValueType, sValue, strEndDate, user, lastModifiedDateTime.ToString(), entity, itemType, item, attribute, startDate, sequence, condition);
          }
          else
          {
            sqlBase += " Where ItemID in (select distinct ItemID from Item where Entity = '{6}' and ItemType = '{7}' and Item = '{8}') and Attribute = '{9}' and StartDate = '{10}'";
            sql = string.Format(sqlBase, tableName, sValueType, sValue, strEndDate, user, lastModifiedDateTime.ToString(), entity, itemType, item, attribute, startDate);
          }
        }
      }
      else
      {
        if (strEndDate.Equals("is null"))
          strEndDate = "null";
        if (entity.Equals(DICTIONARYENTITY, StringComparison.CurrentCultureIgnoreCase))
          sqlBase = "insert into {0} (ItemType, Item, Attribute, StartDate, ValueType, Value, EndDate, LastModifiedBy, LastModifiedDateTime) values ('{2}', '{3}', '{4}', '{5}', '{6}', '{7}', {8}, '{9}', '{10}')";
        else
        {
          if (hasCondition)
            sqlBase = "insert into {0} (ItemID, Attribute, StartDate, ValueType, Value, EndDate, LastModifiedBy, LastModifiedDateTime) select ItemID, '{4}', '{5}', '{6}', '{7}', {8}, '{9}', '{10}' from Item where Entity = '{1}' and ItemType = '{2}' and Item = '{3}' abd Sequence = {11} and condition = '{12}')";
          else
            sqlBase = "insert into {0} (ItemID, Attribute, StartDate, ValueType, Value, EndDate, LastModifiedBy, LastModifiedDateTime) select ItemID, '{4}', '{5}', '{6}', '{7}', {8}, '{9}', '{10}' from Item where Entity = '{1}' and ItemType = '{2}' and Item = '{3}'";
        }
        if (hasCondition)
          sql = string.Format(sqlBase, tableName, entity, itemType, item, attribute, startDate.ToShortDateString(), sValueType, sValue, strEndDate, user, lastModifiedDateTime.ToString(), sequence, condition);
        else
          sql = string.Format(sqlBase, tableName, entity, itemType, item, attribute, startDate.ToShortDateString(), sValueType, sValue, strEndDate, user, lastModifiedDateTime.ToString());
      }
      updateDataFromSQL(sql);
    }
    /// <summary>
    /// Do(es) the requested Item record(s) exist? Adjusts tablename and ignores Entity for Dictinary Entries
    /// </summary>
    /// <param name="tablename"></param>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Exists(string tablename, string entity, string itemType, string item)
    {
      string sql;
      int sequence = 0;
      string condition = string.Empty;
      bool hasCondition = item.StartsWith("Default(", StringComparison.CurrentCultureIgnoreCase);
      if (hasCondition)
      {
        sequence = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, item.Substring(8, 0));
        condition = item.Substring(10, item.Length - 11);
      }
      if (entity.Equals(DICTIONARYENTITY, StringComparison.CurrentCultureIgnoreCase))
      {
        if (tablename.Equals(ITEMTABLE, StringComparison.CurrentCultureIgnoreCase))
          tablename = DICTIONARYITEMTABLE;
        else
          if (tablename.Equals(ATTRIBUTETABLE, StringComparison.CurrentCultureIgnoreCase))
            tablename = DICTIONARYATTRIBUTETABLE;
          else
            if (!tablename.Equals(DICTIONARYITEMTABLE, StringComparison.CurrentCultureIgnoreCase) &&
                !tablename.Equals(DICTIONARYATTRIBUTETABLE, StringComparison.CurrentCultureIgnoreCase))
              return false;
        sql = string.Format("Select Item from {0} where ItemType = '{1}' and Item = '{2}'", tablename, itemType, item);
      }
      else
      {
        if (hasCondition)
          sql = string.Format("select Entity from {0} where Entity = '{1}' and ItemType = '{2}' and Item = '{3}' and sequence = {4} and Condition = '{5}'",
            tablename, entity, itemType, item, sequence, condition);
        else
          sql = string.Format("select Entity from {0} where Entity = '{1}' and ItemType = '{2}' and Item = '{3}'",
            tablename, entity, itemType, item);
      }
      DataSet ds = getDataFromSQL(sql);
      bool exists = (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0);
      ds.Clear();
      return exists;
    }
    /// <summary>
    /// Does this attribute exist?
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    /// <param name="attribute"></param>
    /// <param name="startDate"></param>
    /// <returns></returns>
    public bool ExistsAttribute(string entity, string itemType, string item, string attribute, DateTime startDate)
    {
      int sequence = 0;
      string condition = string.Empty;
      bool hasCondition = item.StartsWith("Default(", StringComparison.CurrentCultureIgnoreCase);
      if (hasCondition)
      {
        sequence = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, item.Substring(8, 0));
        condition = item.Substring(10, item.Length - 11);
      }
      string sql;
      if (entity.Equals(DICTIONARYENTITY, StringComparison.CurrentCultureIgnoreCase))
        sql = string.Format("select * from {0} where ItemType = '{1}' and Item = '{2}' and Attribute = '{3}' and StartDate = '{4}'",
          DICTIONARYATTRIBUTETABLE, itemType, item, attribute, startDate);
      else
      {
        if (hasCondition)
          sql = string.Format("select * from {0} where ItemID in (select distinct ItemId from Item where Entity = '{1}' and ItemType = '{2}' and Item = '{3}' and Sequence = {6} and Condition = '{7}') and Attribute = '{4}' and StartDate = '{5}'",
            ATTRIBUTETABLE, entity, itemType, item, attribute, startDate, sequence, condition);
        else
          sql = string.Format("select * from {0} where ItemID in (select distinct ItemId from Item where Entity = '{1}' and ItemType = '{2}' and Item = '{3}') and Attribute = '{4}' and StartDate = '{5}'",
            ATTRIBUTETABLE, entity, itemType, item, attribute, startDate);
      }
      DataSet ds = getDataFromSQL(sql);
      bool exists = (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0);
      ds.Clear();
      return exists;
    }
    private void updateDataFromSQL(string mySQL)
    {
      SqlCommand command = createCommand();
      // Construct the stored procedure command instance
      command.CommandType = CommandType.Text;
      command.CommandText = mySQL;
      command.ExecuteNonQuery();

      sqlConnection.Close();
    }
    public DataSet getDataFromSQL(string mySQL)
    {
      DataSet dsSQL = new DataSet();
      SqlCommand command = createCommand();
      // Construct the stored procedure command instance
      command.CommandType = CommandType.Text;
      command.CommandText = mySQL;
      try
      {
        dsSQL.Load(command.ExecuteReader(), LoadOption.PreserveChanges, tables);
      }
      catch (SqlException e)
      {
        throw e;
      }
      sqlConnection.Close();

      return dsSQL;
    }
    private SqlCommand createCommand()
    {
      SqlCommand command = this.sqlConnection.CreateCommand();
      command.CommandTimeout = commandTimeout;
      return command;
    }
  }
}
