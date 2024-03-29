﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

//using ACG.Common;
//using ACG.Common.Model;

using ACG.Common;

namespace ACG.Common.Data
{
  public abstract class DataAccessBase
  {
    /*
     * Base class for all DAL objects.
     *      
    */

    #region module data
    protected SecurityContext _securityContext = new SecurityContext();
    string[] idFieldNameList = { "" };
    const string AMOUNTFIELDNAME = "amount";
    protected bool inTransaction = false;
    const string c_QUOTE = "'";
    const string c_MODIFIED_STATUS = "Status";
    const string c_MODIFIED_STATUS_FAILURE = "0";
    protected string[] tables = { "" };
    const int commandTimeout = 600;

    protected SqlTransaction thisTransaction;
    protected string connectionString = string.Empty;
    private SqlConnection connection = null;
    #endregion module data

    #region constructors, sql connection, and begin/end transaction methods
    public DataAccessBase()
    {
      
    }

    /// <summary>
    ///  This exports the SQL conneciton upward
    /// </summary>
    public SqlConnection sqlConnection
    {
      get
      {
        if (connection == null)
            connection = SQLConnectionFactory.GetInstance().Connection;

        if(connection.State == ConnectionState.Closed)
          connection.Open();

        return connection;
      }
    }
    public string ConnectionString 
    { 
      get 
      { 
        return SQLConnectionFactory.GetInstance().getConnectionString(); 
      } 
    }

    #endregion

    protected SqlCommand createCommand()
    {
      SqlCommand command = this.sqlConnection.CreateCommand();
      command.CommandTimeout = commandTimeout;
      return command;
    }

    #region public methods
    /*
     * These public methods of DataAccessBase can be accessed directly by a calling program using any 
     * of the DALs that inherit from it.
     */
    /// <summary>
    /// Update (insert or update or delete) data using an SQL statement
    /// Updated 1-13-2012 by LLA to return a unique id if it is an insert and it happens to create one. Otherwise returns null
    /// </summary>
    /// <param name="mySQL"></param>
    public int? updateDataFromSQL(string mySQL)
    {
      int? returnCode = null;
      SqlCommand command = createCommand();
      // Construct the stored procedure command instance
      command.CommandType = CommandType.Text;
      command.CommandText = mySQL;
      try
      {
        command.ExecuteNonQuery();
        command.CommandText = "Select SCOPE_IDENTITY()";
        var id = command.ExecuteScalar();
        if (id.Equals(System.DBNull.Value))
          returnCode = null;
        else
          returnCode = CommonFunctions.CInt(id);
      }
      catch (SqlException e)      // if we get an error
      {
        Logging.LogFactory.GetInstance().GetLog((this)).Error(string.Format("SQL Update Error sql = <{0}>", mySQL), e);
        return -1;                          // and then rethrow the error so the calling program knows it happened
      }

      sqlConnection.Close();
      return returnCode;
    }
    public void updateRecordEncrypted(string[] fieldsList, string[] valueList, string[] keyList, string tableName, string encryptedFieldName)
    {
      EncryptDecryptString encrypt = new EncryptDecryptString();
      string[] newValues = (string[])valueList.Clone();
      for (int i = 0; i < fieldsList.GetLength(0); i++)
        if (fieldsList[i].Equals(encryptedFieldName, StringComparison.CurrentCultureIgnoreCase))
        {
          newValues[i] = encrypt.encryptString(valueList[i]);
          break;
        }
      updateRecord(fieldsList, newValues, keyList, tableName);

    }
    /// <summary>
    /// Updates a record by constructing an update statement and then running it. See makeUpdateSQL for how this works.
    /// </summary>
    /// <param name="fieldList">List of field/column names. This includes the keys as well.</param>
    /// <param name="valueList">List of values for each field. Note that if the value requires
    /// delimiters (i.e. 'mystring'), they need to be already in the string for that value in this list.</param>    
    /// <param name="keyList">List of field/column names in the unique key</param>
    /// <param name="tableName">Name of the table to update</param>
    /// <returns>An SQL Update statement</returns>
    public void updateRecord(string[] fieldList, string[] valueList, string[] keyList, string tableName)
    {
      string SQL = makeUpdateSQL(fieldList, keyList, valueList, tableName);
      updateDataFromSQL(SQL);
    }

    /// <summary>
    /// Deletes a record by constructing a delete statement. See makeDeleteSQL for how this works. 
    /// </summary>
    /// <param name="fieldList">Ordered list of fieldnames for the where clause</param>
    /// <param name="valueList">Matching ordered list of values for each field in fieldlist</param>
    /// <param name="tableName">Name of the table to delete from</param>
    public void deleteRecords(string[] fieldList, string[] valueList, string tableName)
    {
      string SQL = makeDeleteSQL(fieldList, valueList, tableName);
      updateDataFromSQL(SQL);
    }
    /// <summary>
    /// This routine takes the query and the dataset name and produces a scheme definition, returned as a string
    /// </summary>
    /// <param name="mySQL"></param>
    /// <param name="dataSetName"></param>
    /// <returns></returns>
    public string getSchemaForQuery(string mySQL, string dataSetName)
    {
      DataSet ds = getDataFromSQL(mySQL);
      ds.DataSetName = dataSetName;
      StringWriter sw = new StringWriter();
      ds.WriteXml(sw, XmlWriteMode.WriteSchema);
      return sw.ToString();
    }
    ///// <summary>
    ///// Takes Table[0] from a dataset (if there is one), and loads rows and cols, including the header row
    ///// into a 2D AttributeTable format object array
    ///// </summary>
    ///// <param name="ds">Any open DataSet</param>
    ///// <returns>object [,] 2D Array</returns>
    //public TableHeader ToTable(DataSet ds, Dictionaries dictionary)
    //{
    //  TableHeader returnTable;
    //  if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
    //    return new TableHeader();
    //  DataColumnCollection cols = ds.Tables[0].Columns;
    //  DataRowCollection rows = ds.Tables[0].Rows;
    //  int nbrCols = cols.Count;
    //  int nbrRows = rows.Count + 1;
    //  returnTable = new TableHeader();

    //  TableHeaderColumnsCollection returnTableCols = new TableHeaderColumnsCollection();

    //  foreach (DataColumn col in cols)
    //  {
    //    TableHeaderColumn returnTableCol = new TableHeaderColumn();
    //    returnTableCol.Caption = col.Caption;
    //    returnTableCol.DataType = col.DataType.Name;
    //    returnTableCol.ID = col.ColumnName;
    //    returnTableCols.Add(col.ColumnName, returnTableCol);
    //  }
    //  returnTable.Columns = returnTableCols;
    //  returnTable.KeyNames = new string[] { returnTableCols[0].OriginalID };
    //  foreach (DataRow row in rows)
    //  {
    //    int iCol = 0;
    //    object[] colValues = new object[nbrCols];
    //    foreach (DataColumn col in cols)
    //    {
    //      colValues[iCol] = row[cols[iCol].ColumnName];
    //      iCol++;
    //    }
    //    TableHeaderRow returnTableRow = new TableHeaderRow(returnTableCols, colValues);
    //    returnTable.AddNewRow(returnTableRow);
    //  }
    //  return returnTable;
    //}

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
        if (keyValues[i] != null)
        {
          sb.Append(" = '");
          sb.Append(keyValues[i].Replace("'", "''"));
          sb.Append("'");
        }
        else
          sb.Append(" Is Null");
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
 
    #endregion

    #region protected methods

    /*
     * Thise methods can be accessed by any DAL that inherits from this base class, but should not be
     * used by any program that uses one of those DALs
     */
    /// <summary>
    /// Returns a dataset from a SELECT statement
    /// </summary>
    /// <param name="mySQL">A valid SELECT statement</param>
    /// <returns></returns>
    protected DataSet getDataFromSQL(string mySQL)
    {
      DataSet dsSQL = new DataSet();
      SqlCommand command = createCommand();
      // Construct the stored procedure command instance
      command.CommandType = CommandType.Text;
      command.CommandText = mySQL;
      // Execute the command and return the values in a DataSet instance
      //if (inTransaction)
      //  command.Transaction = thisTransaction;
      try
      {
        dsSQL.Load(command.ExecuteReader(), LoadOption.PreserveChanges, tables);
      }
      catch (SqlException e)
      {
        //if (inTransaction)
        //  command.Transaction.Rollback();
        throw e;
      }
      sqlConnection.Close();

      return dsSQL;
    }
    protected DataAdapterContainer getDataAdapterFromSQL(string mySQL)
    {
      SqlDataAdapter daSQL = new SqlDataAdapter();
      DataSet dsSQL = new DataSet();
      SqlCommand command = createCommand();
      // Construct the stored procedure command instance
      command.CommandType = CommandType.Text;
      command.CommandText = mySQL;
      daSQL.SelectCommand = command;
      // Execute the command and return the values in a DataSet instance

      try
      {
        daSQL.Fill(dsSQL, mySQL);
        //dsSQL.Load(command.ExecuteReader(), LoadOption.PreserveChanges, tables);
      }
      catch (SqlException e)
      {
        throw e;
      }
      sqlConnection.Close();
      DataAdapterContainer da = new DataAdapterContainer();
      da.DataSet = dsSQL;
      da.DataAdapter = daSQL;
      return da;
    }
    /// <summary>
    /// Generic routine to insert a record in a table and return the auto-assigned unique id. This uses a field list,
    /// a list of corresponding values in the same order (value[1] is for field[1]), and the name of the
    /// field that will contain the returning id. This field name must be in the field list.
    /// </summary>
    /// <param name="fieldList">List of field names to insert</param>
    /// <param name="valueList">List of values for each field name</param>
    /// <param name="spName">Name of the stored procedure to execute</param>
    /// <param name="returnName">Name from the field list which contains the auto id</param>
    /// <returns></returns>
    protected long createRecordReturnID(string[] fieldList, string[] valueList, string spName, string returnName)
    {
      long newID = 0;                      // the new ID to return
      const string cAMOUNT = "Amount";
      int nbrParms = fieldList.GetLength(0); // add one for the return value
      string[] parmNames = fieldList;
      object[] parmValues = new object[nbrParms];
      string[] parmDataTypes = new string[nbrParms];
      string[] parmDirection = new string[nbrParms];
      bool returnNameIsInList = false;
      for (int i = 0; i < nbrParms; i++)
      {
        if (parmNames[i] == returnName)
        {
          parmDirection[i] = "output";
          parmValues[i] = null;
          parmDataTypes[i] = "long";
          returnNameIsInList = true;
        }
        else
        {
          if (parmNames[i] == cAMOUNT)
            parmDataTypes[i] = "money";
          else
            parmDataTypes[i] = CommonData.DATATYPESTRING;
          parmValues[i] = CommonFunctions.toValue(valueList[i], parmDataTypes[i]);
          parmDirection[i] = "input";
        }
      }
      if (!returnNameIsInList)
      {
        ExceptionMessage tm = new ExceptionMessage("DataAccessBase", "createRecordReturnID", "ReturnName is not in fieldList");
        tm.AddParm(returnName);
        tm.AddParm(spName);
        foreach (string parmName in parmNames)
          tm.AddParm(parmName);
        throw new Exception(tm.ToString());
      }
      executeNonQueryStoredProcedure(parmNames, parmValues, parmDataTypes, parmDirection, spName);
      for (int k = 0; k < nbrParms; k++)
      {
        if (parmNames[k] == returnName)
        {
          newID = CommonFunctions.CLng(parmValues[k]); // now pick up the return id
          break;
        }
      }
      return newID;
    }
    public void insertRecord(string tableName, string[] fieldList, string[] valueList)
    {
      string mySQL = makeInsertSQL(fieldList, valueList, tableName);
      updateDataFromSQL(mySQL);
    }
    protected DataSet getDataSetUsingOneID(string myID, string myIDName, string spName)
    {
      return getDataSetUsingOneID(myID, myIDName, spName, CommonData.DATATYPESTRING);
    }
    public DataSet getDataSetUsingOneID(string myID, string myIDName, string spName, string idDataType)
    {
      string[] parmList = { myIDName };
      string[] parmValues = { myID };
      string[] parmDataTypes = { idDataType };
      return executeQueryStoredProcedure(parmList, parmValues, parmDataTypes, null, spName);
    }
    /// <summary>
    /// Convert an array of string to an "IN" phrase of a WHERE clause 
    /// like: ('item1','item2'...)
    /// </summary>
    /// <param name="strList"></param>
    /// <returns></returns>
    protected string toList(string[] strList)
    {
      return CommonFunctions.ToList(strList, c_QUOTE, false);
    }
    protected string[] FromList(string strString)
    {
      /* 
       * Take a comma delimited string and turn it into an array of string
       */
      return CommonFunctions.FromList(strString);
    }
    protected string dbValue(string fieldname, object value, string valueType)
    {
      string valueOut;
      bool needsQuote = true;

      if (value == null || value.ToString() == "''")
        valueOut = "null";
      else
        switch (valueType.ToLower()) 
        {
          case CommonData.DATATYPEDATETIME:
          case CommonData.SQLDATE:
            string formatDate = (valueType.ToLower() == CommonData.SQLDATE ? CommonData.FORMATSHORTDATE : CommonData.FORMATLONGDATETIME);
            DateTime dt = (DateTime)CommonFunctions.CDateTime(value);
            if (dt == CommonData.PastDateTime || dt == CommonData.FutureDateTime)
              valueOut = "null";
            else
              valueOut = dt.ToString(formatDate);
            break;
          case CommonData.DATATYPESTRING:
          case CommonData.SQLVARCHAR:
          case CommonData.SQLNVARCHAR:
            valueOut = (string)value;
            break;
          case CommonData.DATATYPEBOOLEAN:
          case CommonData.SQLBIT:
            needsQuote = false;
            valueOut = CommonFunctions.CBoolean(value) ? CommonData.SQLTRUE : CommonData.SQLFALSE;
            break;
          default:
            needsQuote = false;
            valueOut = value.ToString();
            break;
        }

      if (needsQuote && (valueOut.Length == 0 || (valueOut.Length > 0 && valueOut.Substring(0, 1) != c_QUOTE)) && valueOut != "null")
        valueOut = c_QUOTE + valueOut + c_QUOTE;
      
      return valueOut;
    }
    protected string dbValue(string fieldname, object value)
    {
      string valueOut;
      if ((bool)CommonFunctions.IsDateTime(value))
      {
        DateTime dt = (DateTime)CommonFunctions.CDateTime(value);
        if (dt == CommonData.PastDateTime || dt == CommonData.FutureDateTime)
          value = null;
      }
      if (value == null || value.ToString() == "''")
        valueOut = "null";
      else
        if (value.GetType().Name.ToLower() == CommonData.DATATYPESTRING)
          valueOut = (string)value;
        else
          valueOut = value.ToString();
      if (needsQuote(fieldname) && (valueOut.Length == 0 || (valueOut.Length > 0 && valueOut.Substring(0, 1) != c_QUOTE)) && valueOut != "null")
        valueOut = c_QUOTE + valueOut + c_QUOTE;
      return valueOut;
    }
    protected bool needsQuote(string fieldname)
    {
      return (!isUniqueID(fieldname) && fieldname.ToLower() != AMOUNTFIELDNAME);
    }
    protected bool isUniqueID(string fieldname)
    {
      bool isID = false;
      if (fieldname == null)
        return isID;
      string fname = fieldname.ToLower();
      for (int i = 0; i < idFieldNameList.GetLength(0); i++)
        if (fname == idFieldNameList[i].ToLower())
        {
          isID = true;
          break;
        }
      return isID;
    }
    /// <summary>
    /// Create a SQL Update statement from a list of fields and value
    /// </summary>
    /// <param name="fieldList">List of field/column names. This includes the keys as well.</param>
    /// <param name="keyList">List of field/column names in the unique key</param>
    /// <param name="valueList">List of values for each field. Note that if the value requires
    /// delimiters (i.e. 'mystring'), they need to be already in the string for that value in this list.</param>
    /// <param name="tableName">Name of the table to update</param>
    /// <returns>An SQL Update statement</returns>
    protected string makeUpdateSQL(string[] fieldList, string[] keyList, string[] valueList, string[] valueTypeList, string tableName)
    {
      bool firstToken = true;                             // note the first token because of commas
      StringBuilder returnSQL = new StringBuilder(4096);  // use stringbuilder (more efficient)
      returnSQL.Append("Update ");
      returnSQL.Append(tableName);
      returnSQL.Append(" Set ");                           // Update {tablename} Set 
      int nbrFields = fieldList.GetLength(0);             // nbr fields and values
      int nbrKeys = keyList.GetLength(0);                 // nbr fields in the unique index
      for (int iField = 0; iField < nbrFields; iField++)                 // foreach field
      {
        bool fieldIsKey = false;                          // check to see if this field is one of the keys
        for (int iKey = 0; iKey < nbrKeys; iKey++)
        {
          if (fieldList[iField].Equals(keyList[iKey], StringComparison.CurrentCultureIgnoreCase))
          {
            fieldIsKey = true;                            // yes it is
            break;
          }
        }
        if (!fieldIsKey)                                  // only put the Set {fieldname} = {value}
        {                                                 // if this is NOT a key field
          if (firstToken)                                 // only put a comma before this item if 
            firstToken = false;                           //     it is not the first one
          else
            returnSQL.Append(", ");
          returnSQL.Append(fieldList[iField]);                 // append the fieldname
          returnSQL.Append(" = ");                        // and an '='
          returnSQL.Append(dbValue(fieldList[iField], valueList[iField], valueTypeList[iField]));                      // and the value
        }
      }
      returnSQL.Append(" Where ");                        // add the Where clause
      firstToken = true;                                  // reset the first token flag to use for "AND"s
      for (int iKey = 0; iKey < nbrKeys; iKey++)                   // and put qualifiers for each unique key
      {
        int keySub = -1;
        for (int iField = 0; iField < nbrFields; iField++)               // find the key in the field list
        {
          if (fieldList[iField].Equals(keyList[iKey], StringComparison.CurrentCultureIgnoreCase))
          {
            keySub = iField;
            break;
          }
        }
        if (keySub >= 0)
        {
          if (firstToken)                                 // only put an "And" before this item 
            firstToken = false;                           //    if it is not the first one
          else
            returnSQL.Append(" And ");
          returnSQL.Append(fieldList[keySub]);            // {keyfieldname} = {value}  
          returnSQL.Append(" = ");
          returnSQL.Append(dbValue(fieldList[keySub], valueList[keySub], valueTypeList[keySub]));
          //returnSQL.Append(valueList[keySub]);
        }
        else
          throw new Exception("Unique key column name not found in the list of fields");
      }
      returnSQL.Append(";");
      return returnSQL.ToString();
    }

    protected string makeUpdateSQL(string[] fieldList, string[] keyList, string[] valueList, string tableName)
    {
      bool firstToken = true;                             // note the first token because of commas
      StringBuilder returnSQL = new StringBuilder(4096);  // use stringbuilder (more efficient)
      returnSQL.Append("Update ");
      returnSQL.Append(tableName);
      returnSQL.Append(" Set ");                           // Update {tablename} Set 
      int nbrFields = fieldList.GetLength(0);             // nbr fields and values
      int nbrKeys = keyList.GetLength(0);                 // nbr fields in the unique index
      for (int iField = 0; iField < nbrFields; iField++)                 // foreach field
      {
        bool fieldIsKey = false;                          // check to see if this field is one of the keys
        for (int iKey = 0; iKey < nbrKeys; iKey++)
        {
          if (fieldList[iField].Equals(keyList[iKey], StringComparison.CurrentCultureIgnoreCase))
          {
            fieldIsKey = true;                            // yes it is
            break;
          }
        }
        if (!fieldIsKey)                                  // only put the Set {fieldname} = {value}
        {                                                 // if this is NOT a key field
          if (firstToken)                                 // only put a comma before this item if 
            firstToken = false;                           //     it is not the first one
          else
            returnSQL.Append(", ");
          returnSQL.Append(fieldList[iField]);                 // append the fieldname
          returnSQL.Append(" = ");                        // and an '='
          returnSQL.Append(dbValue(fieldList[iField], valueList[iField]));                      // and the value
        }
      }
      returnSQL.Append(" Where ");                        // add the Where clause
      firstToken = true;                                  // reset the first token flag to use for "AND"s
      for (int iKey = 0; iKey < nbrKeys; iKey++)                   // and put qualifiers for each unique key
      {
        int keySub = -1;
        for (int iField = 0; iField < nbrFields; iField++)               // find the key in the field list
        {
          if (fieldList[iField].Equals(keyList[iKey], StringComparison.CurrentCultureIgnoreCase))
          {
            keySub = iField;
            break;
          }
        }
        if (keySub >= 0)
        {
          if (firstToken)                                 // only put an "And" before this item 
            firstToken = false;                           //    if it is not the first one
          else
            returnSQL.Append(" And ");
          returnSQL.Append(fieldList[keySub]);            // {keyfieldname} = {value}  
          returnSQL.Append(" = ");
          returnSQL.Append(valueList[keySub]);
        }
        else
          throw new Exception("Unique key column name not found in the list of fields");
      }
      returnSQL.Append(";");
      return returnSQL.ToString();
    }
    protected string makeDeleteSQL(string[] fieldList, string[] valueList, string tableName)
    {
      StringBuilder returnSQL = new StringBuilder(4096);  // use stringbuilder (more efficient)
      if (fieldList.Length != valueList.Length)
      {
        ExceptionMessage tMessage = new ExceptionMessage("DataAccessBase", "makeDeleteSQL",
            "Field name list does not match value list");
        tMessage.AddParm(tableName);
      }
      else
      {
        returnSQL.Append("DELETE FROM ");
        returnSQL.Append(tableName);
        returnSQL.Append(" WHERE ");

        for (int i = 0; i < fieldList.Length; i++)
        {
          returnSQL.Append(fieldList[i]);
          returnSQL.Append(" = ");
          returnSQL.Append(dbValue(fieldList[i], valueList[i]));
          if (!(i == (fieldList.Length - 1)))
            returnSQL.Append(" AND ");
        }
      }
      return returnSQL.ToString();
    }
    protected string makeInsertSQL(string[] fieldList, string[] valueList, string tableName)
    {
      int nbrFlds = fieldList.GetLength(0);
      if (nbrFlds != valueList.GetLength(0))
      {
        ExceptionMessage tMessage = new ExceptionMessage("DataAccessBase", "createRecordSQL",
          "Field name list does not match value list");
        tMessage.AddParm(tableName);
        int nbrVals = valueList.GetLength(0);
        int maxNbr = Math.Max(nbrFlds, nbrVals);
        for (int i = 0; i < maxNbr; i++)
        {
          string myParm = string.Empty;
          if (i < nbrFlds)
            myParm += fieldList[i];
          myParm += ":";
          if (i < nbrVals)
            myParm += valueList[i];
          tMessage.AddParm(myParm);
        }
        throw new Exception(tMessage.ToString());
      }
      bool firstTime = true;
      StringBuilder mySQL = new StringBuilder("INSERT INTO ", 1024);
      mySQL.Append(tableName);
      mySQL.Append("(");
      for (int i = 0; i < nbrFlds; i++)
      {
        if (firstTime)
          firstTime = false;
        else
          mySQL.Append(", ");
        mySQL.Append(fieldList[i]);
      }
      firstTime = true;
      mySQL.Append(") VALUES (");
      for (int i = 0; i < nbrFlds; i++)
      {
        if (firstTime)
          firstTime = false;
        else
          mySQL.Append(", ");
        mySQL.Append(dbValue(fieldList[i], valueList[i]));
      }
      mySQL.Append(")");
      return mySQL.ToString();
    }
    protected string makeSelectSQL(string[] fieldList, string[] valueList, string tableName)
    {
      StringBuilder returnSQL = new StringBuilder(4096);  // use stringbuilder (more efficient)
      if (fieldList.Length != valueList.Length)
      {
        ExceptionMessage tMessage = new ExceptionMessage("DataAccessBase", "makeDeleteSQL",
            "Field name list does not match value list");
        tMessage.AddParm(tableName);
      }
      else
      {
        returnSQL.Append("SELECT * FROM ");
        returnSQL.Append(tableName);
        returnSQL.Append(" WHERE ");

        for (int i = 0; i < fieldList.Length; i++)
        {
          returnSQL.Append(fieldList[i]);
          returnSQL.Append(" = ");
          returnSQL.Append(dbValue(fieldList[i], valueList[i]));

          if (!(i == (fieldList.Length - 1)))
            returnSQL.Append(" AND ");
        }
      }
      return returnSQL.ToString();
    }
    public void executeNonQueryStoredProcedure(string[] parameterNames, object[] parameterValues,
      string[] parameterDateTypes, string[] parameterDirection, string spName)
    {
      DataSet dsQuery;
      executeStoredProcedure(parameterNames, parameterValues, parameterDateTypes, parameterDirection, spName, false, out dsQuery);
    }
    public DataSet executeQueryStoredProcedure(string[] parameterNames, object[] parameterValues,
      string[] parameterDateTypes, string[] parameterDirection, string spName)
    {
      DataSet dsQuery;
      executeStoredProcedure(parameterNames, parameterValues, parameterDateTypes, parameterDirection, spName, true, out dsQuery);
      return dsQuery;
    }
    private void executeStoredProcedure(string[] parameterNames, object[] parameterValues,
      string[] parameterDateTypes, string[] parameterDirection, string spName, bool isQuery, out DataSet dsQuery)
    {
      DataSet ds = new DataSet(spName);
      int nbrParameters = parameterNames.GetLength(0);
      if (nbrParameters != parameterValues.GetLength(0)
        || nbrParameters != parameterDateTypes.GetLength(0))
      {
        ExceptionMessage tm = new ExceptionMessage("DataAccessBase", "executeQueryStoredProcedure", "Number of Parameter Names does not match number of values or datatypes");
        for (int k = 0; k < nbrParameters; k++)
          tm.AddParm(parameterNames[k]);
        tm.AddParm("NumberValues:" + parameterValues.GetLength(0));
        tm.AddParm("NumberDataTypes:" + parameterDateTypes.GetLength(0));
        throw new Exception(tm.ToString());
      }
      int parmDirectionLength = 0;
      if (parameterDirection != null)
        parmDirectionLength = parameterDirection.GetLength(0);
      SqlCommand command = createCommand();
      command.CommandType = CommandType.StoredProcedure;
      command.CommandText = spName;
      // Load parameters
      int i = 0;                                // sub for valueList
      foreach (string parmName in parameterNames)
      {
        SqlDbType type;
        string dataType = parameterDateTypes[i].ToLower();
        type = (SqlDbType)CommonFunctions.MapVarToSQL(dataType);

        SqlParameter newParm = new SqlParameter(parmName, type);
        if (dataType == CommonData.DATATYPESTRING)
            newParm.Size = int.MaxValue;
        if (parameterValues[i] == null)
          newParm.Value = System.DBNull.Value;
        else
        {
          object val = CommonFunctions.toValue(parameterValues[i], dataType);
          newParm.Value = val;
        }
        if (parameterDirection != null && parmDirectionLength >= i && parameterDirection[i] != null)
        {
          string parmDirection = parameterDirection[i].ToLower();
          switch (parmDirection)
          {
            case "out":
            case "output":
              newParm.Direction = ParameterDirection.Output;
              break;
            case "input":
              newParm.Direction = ParameterDirection.Input;
                break;
            case "inputoutput":
              newParm.Direction = ParameterDirection.InputOutput;
              break;
            case "returnvalue":
              newParm.Direction = ParameterDirection.ReturnValue;
              break;
          }
        }
        command.Parameters.Add(newParm);
        i++;
      }
      if (inTransaction)
        command.Transaction = thisTransaction;
      if (isQuery)
      {
        const int maxtables = 6;
        string[] tables = new string[maxtables];
        for (int j = 0; j < maxtables; j++)
          if (j == 0)
            tables[j] = spName;
          else
            tables[j] = string.Format("{0}_{1}", spName, j.ToString());
        ds.Load(command.ExecuteReader(), LoadOption.PreserveChanges, tables);
        for (int j = ds.Tables.Count - 1; j >= 1; j--)
        {
          DataTable dt = ds.Tables[j];
          if (dt.Columns.Count == 0)
            ds.Tables.Remove(dt);
        }
      }
      else
      {
        command.ExecuteNonQuery();    // run the query
        for (int k = 0; k < nbrParameters; k++) // now look for output parameters
        {
          string parmName = parameterNames[k];
          SqlParameter parm = command.Parameters[parmName];
          if (parm.Direction == ParameterDirection.Output || parm.Direction == ParameterDirection.InputOutput) // if we find one
            parameterValues[k] = parm.Value;  // the put the value back in the list so the calling program can have it
        }
      }
      dsQuery = ds;
      sqlConnection.Close();
    }
     
    #endregion protected methods
  }
}
