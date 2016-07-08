using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using ACG.App.Common;
using ACG.Sys.SecurityEngine;

namespace ACG.Sys.Data
{
  public partial class DataSource
  {
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
    public ACGTable getFieldValidationData()
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
      string sql = "select * from vw_DBColumnList order by Field ";
      DataSet ds = getDataFromSQL(sql);
      ACGTable returnTable = CommonFunctions.convertDataSetToCCITable(ds);
      ds.Clear();
      ds = null;
      return returnTable;
    }
    public ACGTable getTableFieldListData(string tableName)
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
      string sql = string.Format("SELECT * FROM vw_DBColumnDetail WHERE table_name = '{0}' ORDER BY column_name", tableName);
      DataSet ds = getDataFromSQL(sql);
      ACGTable returnTable = CommonFunctions.convertDataSetToCCITable(ds);
      ds.Clear();
      ds = null;
      return returnTable;
    }
  
    #region Utilities
    public string getFormattedFieldValue(string fieldName, int rowIndex, ACGTable sourceTable)
    {
      string formattedFieldValue = string.Empty;
      string fieldType = CommonData.SQLNVARCHAR;
      if (sourceTable.ContainsColumn(fieldName))
      {
        fieldType = sourceTable.getFieldType(fieldName);
        switch (fieldType)
        {
          case CommonData.SQLDATE:
            formattedFieldValue = CommonFunctions.CDateTime(sourceTable[rowIndex, fieldName]).ToString(CommonData.FORMATSHORTDATE);
            break;
          case CommonData.SQLDATETIME:
            formattedFieldValue = CommonFunctions.CDateTime(sourceTable[rowIndex, fieldName]).ToString(CommonData.FORMATLONGDATETIME);
            break;
          case CommonData.SQLBIT:
            formattedFieldValue = CommonFunctions.CBoolean(sourceTable[rowIndex, fieldName]) ? CommonData.SQLTRUE : CommonData.SQLFALSE;
            break;
          case CommonData.SQLBIGINT:
          case CommonData.SQLINT:
            formattedFieldValue = CommonFunctions.CInt(sourceTable[rowIndex, fieldName]).ToString().Trim();
            break;
          case CommonData.SQLNVARCHAR:
          case CommonData.SQLVARCHAR:
          default:
            formattedFieldValue = string.Format("{0}", CommonFunctions.CString(sourceTable[rowIndex, fieldName]).ToString().Trim());
            break;
        }
      }

      return formattedFieldValue;
    }
    public bool checkIfRecordExists(string tablename, string[] keyNames, string[] keyValues)
    {
      bool recordExists = false;
      DataSet recordsRead = new DataSet();

      if (string.IsNullOrEmpty(tablename))
        throw new Exception("getRecordExists must have a valid table name");

      if (keyNames == null || keyNames.GetLength(0) == 0)
        throw new Exception("getRecordExists must have a valid list of keyName Fields");

      if (keyValues == null || keyValues.GetLength(0) == 0)
        throw new Exception("getRecordExists must have a valid list of keyValue Fields");

      try
      {
        recordsRead = getDataFromSQL(makeSelectSQL(keyNames, keyValues, tablename));
        if (recordsRead != null && recordsRead.Tables != null && recordsRead.Tables.Count > 0 && recordsRead.Tables[0].Rows != null && recordsRead.Tables[0].Rows.Count > 0)
          recordExists = true;

        recordsRead = null;
      }
      catch (Exception ex)
      {
        //recordExists = false;
        throw new Exception(string.Format("getRecordExists error: {0}::{1}", ex.Message, ex.StackTrace));
      }

      return recordExists;
    }

    #endregion Utilities
  }
}
