using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
//using System.Linq;
using System.Text;
using System.Xml;

using TAGBOSS.Common.Interface;
using TAGBOSS.Common.Logging;
using TAGBOSS.Common.Model;

namespace TAGBOSS.Common
{
  [Serializable]
  public class TableHeader : IEnumerable, IComparable, ICloneable
  {
    #region constants
    public const string KEYNONE = "none";
    public const string TYPEUNIQUEKEY = "tableheaderuniquekey";
    public const string VALUETYPETABLEHEADER = TAGFunctions.TABLEHEADER;
    public const string ATTRIBUTEDATATYPE = "datatype";
    public const string ATTRIBUTEMASK = "mask";
    public const string ATTRIBUTEPICKLIST = "picklist";
    public const string ATTRIBUTEREQUIRED = "required";
    public const string ATTRIBUTEKEYLIST = "tablekeylist";
    public const string ATTRIBUTEDESCRIPTION = "description";
    public static string EMPTYGUITABLE = "<rows><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row><row><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/><cell/></row></rows>";

    #endregion

    #region module data
    private bool enforceGoodHeaders = false;
    private bool includeDeletedRecords = false;

    private Dictionaries dictionary { get { return DictionaryFactory.getInstance().getDictionary(); } }
    //private AttributeTable at = new AttributeTable();
    private string tableSource = string.Empty;
    private string dataTypeSource = string.Empty;
    private string header = null;
    //private string[] dataTypes = null;
    private bool hasHeader = false;
    private bool hasDeletedRecords { get { return DeletedKeyList.Count > 0; } }
    private bool hasDictionaryHeader = false;
    private bool validHeader = false;
    private bool hasDuplicates = false;     // true if the key defined for this table has duplicates
    private bool thisReturnsFindFirst = false;
    private int sequence = 0;               // used to create artificial uniqueness if there are duplicate keys
    string attributeName = string.Empty;
    string _source = null;
    string valueType = string.Empty;
    //string[] headerNames = new string[0];
    string keyList = string.Empty;
    string[] keyNames = null;
    bool canMergeNewSource = true;
    bool _markForDelete = false;
    bool _dirty = false;
    // <key, source>
    private Dictionary<TableHeaderUniqueKey, string> deletedKeyList = new Dictionary<TableHeaderUniqueKey, string>();
    TableHeaderColumnsCollection columns = new TableHeaderColumnsCollection();
    protected TableHeaderRowCollection rows = new TableHeaderRowCollection();
    #endregion

    #region public properties
    /// <summary>
    /// array of column names that make the index for this table
    /// </summary>
    public string[] KeyNames
    {
      get { return keyNames; }
      set 
      {
        if (value == null)
        {
          TAGExceptionMessage tm = new TAGExceptionMessage("TableHeader", "set KeyNames", "value cannot be null");
          throw new Exception(tm.ToString());
        }
        keyNames = value; 
      }
    }
    public Dictionary<TableHeaderUniqueKey, string> DeletedKeyList
    {
      get { return deletedKeyList; }
    }
    /// <summary>
    /// Constant operator that says a row is to be added/replaced in a Merge
    /// </summary>
    public const string OPERATORADD = "+";
    /// <summary>
    /// Constant operator that says a row is to be deleted in a Merge
    /// </summary>
    public const string OPERATORDELETE = "-";
    /// <summary>
    /// Constant operator that says the new table is to completely replace the old one in a Merge
    /// </summary>
    public const string OPERATORSTOPMERGE = "=";
    /// <summary>
    /// Can this table have new table values merged into it?
    /// </summary>
    public bool CanMergeNewSource
    {
      get { return canMergeNewSource; }
      set { canMergeNewSource = value; }
    }
    /// <summary>
    /// Number of Rows in this TableHeader
    /// </summary>
    public int Count 
    { 
      get 
      { 
        // If we have no deleted records, or if we include them in the count...
        if (!hasDeletedRecords || includeDeletedRecords)
          return rows.Count;      // just return the number of entries in the collection
        // Otherwise, we have to go count the non-deleted ones
        int count = 0;
        foreach (TableHeaderRow row in rows)
        {
          if (!row.Deleted)
            count++;
        }
        return count;
      } 
    }
    public int Rank { get { return 2; } }
    public int Length { get { return rows.Count * Rank; } }
    /// <summary>
    /// Returns the row that matches key. This supports three key types:
    /// <para>
    /// 1) Key of type UniqueKey
    /// </para>
    /// <para>
    /// 2) Key of type string. This will accept either a single value or comma or ~ separated list of values. The routine will assume the list is in order of 
    /// keys specified for this table, construct a UniqueKey, and then use it
    /// </para>
    /// <para>
    /// 3) Key of type integer. This will return the Nth row.
    /// </para>
    /// If no match is found, it returns null.
    /// <para>
    /// Setter works much the same way, except that nothing happens if the row is not found.
    /// This also uses FindFirst() logic, which means that if no exact match is found, it will return the one closest that is not greater than the key.
    /// See UniqueKey CompareTo for rules on what keys match.
    /// </para>
    /// </summary>
    /// <param name="key">UniqueKey, index, or comma or tilde-delimited list of values in keyNames[] order</param>
    /// <returns>The Row if a match is found, or null if it is not</returns>
    public TableHeaderRow this[object key]
    {
      get
      {
        Type t = key.GetType();
        string typeName = t.Name.ToLower();
        TableHeaderUniqueKey k;
        //KeyValuePair<TableHeaderUniqueKey, TableHeaderRow> entry;

        switch (typeName)
        {
          case "int32":                                 // key is an index
            if (key == null)
              return null;
            int idx = 0;                                // counter of rows that are NOT deleted
            int compareIdx = (int)key;                  // index we are looking for
            /*
             * If this collection has deleted records and we are supposed to ignore them, then
             * we have to start at the beginning and count upward until we reach the correct 
             * "Virtual" index, which is to say, the ordinal position within the non-deleted records.
             * 
             * Note: we only do this if we have deleted records since it is slower than just getting the one
             * we want using ElementOf(). This is also because there are a few "really big" tableheaders,
             * and these are normally read, not updated, so they normally won't have deleted records.
             * 
             */
            if (hasDeletedRecords && !includeDeletedRecords)  
            {
              foreach (TableHeaderRow row in rows) // for each record, deleted or not
              {
                if (!row.Deleted)   // if it is deleted, skip it
                {
                  if (idx == compareIdx)         // if not, does the "virtual" index match the one we ask for?
                    return row;     // if it does, return it
                  else
                    idx++;                       // if not, then increment the virtual index
                }
              }
              return null;                       // if we got here, then our requested index is out of bounds (we reached the end before we got there)
            }
            else
            {
              return rows[compareIdx];
            }
          case TYPEUNIQUEKEY:                           // key is of type UniqueKey 
            k = (TableHeaderUniqueKey)key;
            if (thisReturnsFindFirst)
              return FindFirst(k);
            else
            {
              if (Contains(k))
                return rows[k];
              else
                return null;
            }
          case TAGFunctions.DATATYPESTRING:             // key is a string. This means it is a delimited list (comma or ~) of key values in keynames[] order
            string s = ((string)key).ToLower();
            k = new TableHeaderUniqueKey(columns, s, keyNames);
            if (k.Count == 0)
            {
              return rows.First();  // current row is one with the zero index
            }
            if (thisReturnsFindFirst)
              return FindFirst(k);
            else
            {
              if (Contains(k))
                return rows[k];
              else
                return null;
            }
          default:                                // we only support UniqueKey, int, and string keytypes. 
            return null;
        }
      }
    }
    public object this[int iRow, int iCol]
    {
      get
      {
        if (iRow < 0 || iRow >= rows.Count)
          return null;
        if (iCol < 0 || iCol >= columns.Count)
          return null;
        TableHeaderRow row = this[iRow];
        if (row == null)
          return null;
        return row[iCol].Value;
      }
      set
      {
        if (iRow < 0 || iRow >= rows.Count)
          return;
        if (iCol < 0 || iCol >= columns.Count)
          return;
        TableHeaderRow row = this[iRow];
        if (row == null)
          return;
        row[iCol].Value = value;
      }
    }
    public object this[int iRow, string columnName]
    {
      get
      {
        if (iRow < 0 || iRow >= rows.Count)
          return null;
        if (!columns.Contains(columnName))
          return null;
        TableHeaderRow row = this[iRow];
        if (row == null)
          return null;
        return row[columnName].Value;
      }
      set
      {
        if (iRow < 0 || iRow >= rows.Count)
          return;
        if (!columns.Contains(columnName))
          return;
        TableHeaderRow row = this[iRow];
        if (row == null)
          return;
        row[columnName].Value = value;
      }
    }
    public object this[object key, string columnName]
    {
      get
      {
        if (!columns.Contains(columnName))
          return null;
        TableHeaderRow row = this[key];
        if (row == null)
          return null;
        return row[columnName].Value;
      }
      set
      {
        if (!columns.Contains(columnName))
          return;
        TableHeaderRow row = this[key];
        if (row == null)
          return;
        row[columnName].Value = value;
      }
    }
    /// <summary>
    /// Flag that can be set to include/exclude Rows if the deleted flag is set. 
    /// Default is false (do not include them). All retrieval operations (this[] and enumeration) are affected
    /// </summary>
    public bool IncludeDeletedRows
    {
      get { return includeDeletedRecords; }
      set { includeDeletedRecords = value; }
    }
    /// <summary>
    /// Set this flag if you want rows marked as deleted instead of just removed. The default is false
    /// </summary>
    public bool MarkForDelete
    {
      get { return _markForDelete; }
      set { _markForDelete = value; }
    }
    /// <summary>
    /// If true, use of indexer (this[key]) will use lookuprange/findfirst algorhthm. If false, it will look for an exact match.
    /// Default is false (exact match only).
    /// </summary>
    public bool ThisReturnsFindFirst
    {
      get { return thisReturnsFindFirst; }
      set { thisReturnsFindFirst = value; }
    }
    //public int IndexOf(UniqueKey key)
    //{
    //  if (rows.ContainsKey(key))
    //  {
    //    Dictionary<int, Row> entry = rows[key];
    //    rows.
    //  }
    //  else
    //    return -1;
    //}
    public bool HasDuplicates
    {
      get { return hasDuplicates; }
      set { hasDuplicates = value; }
    }

    public string Source
    {
      get { return _source; }
      set { _source = value; }
    }
    public TableHeader Rows { get { return this; } }
    public TableHeaderRowCollection rowCollection { get { return rows; } set { rows = value; } }
    public TableHeaderColumnsCollection Columns { get { return columns; } set { columns = value; }  }
    /// <summary>
    /// Returns true if the flag has been explicitly set, or if any row or cell is dirty)
    /// </summary>
    public bool Dirty
    {
      get
      {
        if (_dirty)
          return true;
        foreach (TableHeaderRow row in rows)
          if (row.Dirty)
            return true;
        return false;
      }
      set 
      { 
        _dirty = value;
        if (!_dirty)  // we are clearing the dirty flag
          foreach (TableHeaderRow row in rows)
            row.Dirty = false;
      }
    }
    //public Dictionaries Dictionary
    //{
    //  get { return dictionary; }
    //  set { dictionary = value; }
    //}
    #endregion

    #region constructors

    public TableHeader()
    {

    }
    public TableHeader(string pAttributeName, string pValue, string pSource)
    {
      attributeName = pAttributeName;
      _source = pSource;
      load(attributeName, VALUETYPETABLEHEADER, pValue, true, false, _source);
    }
    public TableHeader(string pAttributeName, TableHeaderColumnsCollection cols, Dictionaries dict)
    {
      attributeName = pAttributeName;
      if (dict == null)
        return;
      columns = cols;
      keyNames = new string[1] { columns[0].ID };
      hasHeader = true;
      valueType = TAGFunctions.TABLEHEADER.ToLower();
    }
    public TableHeader(string pAttributeName, string pValue, Dictionaries dict)
    {
      attributeName = pAttributeName;
      if (dict == null)
        return;
      if (isXML(pValue))
      {
        //dictionary = dict;
        loadXML(pValue);
      }
      else
        load(pAttributeName, VALUETYPETABLEHEADER, pValue, true, true, null);
    }
    public TableHeader(string pAttributeName, string pValue, Dictionaries dict, bool throwError)
    {
      attributeName = pAttributeName;
      if (dict == null)
        return;
      if (isXML(pValue))
      {
        //dictionary = dict;
        loadXML(pValue);
      }
      else
        load(pAttributeName, VALUETYPETABLEHEADER, pValue, true, throwError, null);
    }
    public TableHeader(string pAttributeName, string pValueType, string pValue, Dictionaries dict)
    {
      load(pAttributeName, pValueType, pValue, true, true, null);
    }
    public TableHeader(string pAttributeName, string pValueType, string pValue, Dictionaries dict, bool throwError)
    {
      load(pAttributeName, pValueType, pValue, true, throwError, null);
    }
    //public TableHeader(string pAttributeName, string pValueType, string pValue, Dictionaries dict, bool useDictionaryForHeader)
    //{
    //  load(pAttributeName, pValueType, pValue, dict, useDictionaryForHeader);
    //}

    #endregion

    #region public methods

    #region standard methods
    /// <summary>
    /// Works like this[key], except that sequence is ignored. It finds the first (sequence zero) key that matches.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public TableHeaderRow Row(TableHeaderUniqueKey key)
    {
      TableHeaderUniqueKey compareKey = (TableHeaderUniqueKey)key.Clone();
      compareKey.Sequence = 0;
      return this[compareKey];
    }
    static public int NumberOfColumnsIn(string tableHeaderString)
    {
      if (tableHeaderString == null)
        return 0;
      if (tableHeaderString.Length < 3)
        return 0;
      StringBuilder s = new StringBuilder(tableHeaderString);
      int nbrCols = 1;
      bool endOfFirstRow = false;
      for (int i = 0; i < s.Length; i++)
      {
        switch (s[i])
        {
          case TAGFunctions.COLSEPARATORCHAR:
            nbrCols++;
            break;
          case TAGFunctions.ROWSEPARATORCHAR:
            endOfFirstRow = true;
            break;
        }
        if (endOfFirstRow)
          break;
      }
      return nbrCols;
    }
    static public int NumberOfRowsIn(string tableHeaderString)
    {
      if (tableHeaderString == null)
        return 0;
      if (tableHeaderString.Length < 3)
        return 0;
      StringBuilder s = new StringBuilder(tableHeaderString);
      int nbrRows = 1;
      for (int i = 0; i < s.Length; i++)
      {
        switch (s[i])
        {
          case TAGFunctions.ROWSEPARATORCHAR:
            nbrRows++;
            break;
        }
      }
      return nbrRows;
    }
    public int GetLength(int dimension)
    {
      switch (dimension)
      {
        case 0:
          return rows.Count;
        case 1:
          return columns.Count;
      }
      throw new Exception("GetLength dimension must be zero or one");
    }
    public int GetUpperBound(int dimension)
    {
      return GetLength(dimension) - 1;
    }
    public int GetLowerBound(int dimension)
    {
      return 0;
    }
    /// <summary>
    /// returns a string key suitable for using in this[key,column] or this[key] based on the index
    /// </summary>
    /// <param name="index">int ordinal position of the row</param>
    /// <returns>string key in the format ColValue1,ColValue2,...</returns>
    public string RowKey(int index)
    {
      if (index < 0 || index > Count)
        throw new IndexOutOfRangeException();
      return rows.KeyAt(index).ToKeyString();
    }
    public void AddRow(TableHeaderUniqueKey key, TableHeaderRow row)
    {
      // If this row is flagged with a DELETE command, we need to add it to the DeletedKeyList and that is all.
      if (row.Op == OPERATORDELETE)
      {
        if(!(IsDeleted(key)))
          deletedKeyList.Add(key, _source);
        return;
      }
      if (rows.Contains(key))
        rows[key] = row;
      else
        rows.Add(key, row);
    }
    /// <summary>
    /// Default overload that will replace (using versioning) a row if it already exists.
    /// </summary>
    /// <param name="row"></param>
    public void AddRow(TableHeaderRow row)
    {
      AddRow(row, true);
    }
    /// <summary>
    /// Add a new row, even if the key is a duplicate
    /// </summary>
    /// <param name="row"></param>
    public void AddNewRow(TableHeaderRow row)
    {
      AddRow(row, false);
    }
    public void Remove(object key)
    {
      TableHeaderRow row = this[key];
      TableHeaderUniqueKey uKey = new TableHeaderUniqueKey(columns, row, keyNames);
      Remove(uKey);
    }
    /// <summary>
    /// This Remove requires that both the key values and the sequence be a match before it will delete one. Since
    /// the default sequence value in a UniqueKey is zero, if the calling program simply contructs a key to 
    /// use for the delete, then this delete will delete the first one if there is more than one match.
    /// </summary>
    /// <param name="key"></param>
    public void Remove(TableHeaderUniqueKey key)
    {
      if (deletedKeyList.ContainsKey(key))
        deletedKeyList.Remove(key);
      else
        if (_markForDelete)
        {
          rows[key].Deleted = true;
          deletedKeyList.Add(key, Source);
        }
        else
          rows.Remove(key);
    }
    /// <summary>
    /// Returns the UniqueKey that corresponds to the index. This index behaves differently from 
    /// this[int index] and the enumerator if IncludeDeletedRows = false. It will return a unique key 
    /// even if the row is deleted.
    /// </summary>
    /// <param name="index">index of row to return</param>
    /// <returns></returns>
    public TableHeaderUniqueKey KeyOf(int index)
    {
      if (index < 0 || index >= rows.Count)     // if the index is out of range
        return new TableHeaderUniqueKey();                 // return a blank one
      return rows.KeyAt(index);  // get the key at this index location
    }
    /// <summary>
    /// overload which allows use of a string containing the comma-separated key value(s)
    /// instead of needing a TableHeaderUniqueKey for the lookup. Note this requires values as a string.
    /// </summary>
    /// <param name="keyValues">single value (if key has only one column) or multiple values separated by commas to use as the row key</param>
    /// <returns></returns>
    public bool Contains(string keyValues)
    {
      TableHeaderUniqueKey key = new TableHeaderUniqueKey(columns, keyValues, keyNames);
      return Contains(key);
    }
    /// <summary>
    /// Behaves differently depending upon if IncludedDeletedRows == true. 
    /// If true, then returns true if the collection contains a row, even if it is deleted. 
    /// If false, then returns true only if the row exists AND is not deleted.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool Contains(TableHeaderUniqueKey key)
    {
        return rows.Contains(key);
    }
    /// <summary>
    /// Is the key in the deleted key list?
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool IsDeleted(TableHeaderUniqueKey key)
    {
      // this is required because Dictionary<TableHeaderUniqueKey, object> does not use the custom comparer
      foreach (TableHeaderUniqueKey lookupKey in deletedKeyList.Keys)
        if (key.Equals(lookupKey))
          return true;
      return false;
    }
    /// <summary>
    /// Replaces the old AttributeTable Merge.
    /// </summary>
    /// <param name="sourceTable">Table to Merge with this instance, in (va11~val2|val3~val4...) format</param>
    /// <param name="sourceValueType">"tablemod", "tableheader", or "tablenoheader"</param>
    /// <param name="lastTime">Is this the last time through (so tablemods are made tableheader)?</param>
    private void Merge(string sourceTable, string sourceValueType, bool lastTime)
    {
      TableHeader newTable = new TableHeader(attributeName, sourceValueType, sourceTable, dictionary);
      Merge(newTable);
    }

    /// <summary>
    /// Overload that accepts a TableHeader class object
    /// </summary>
    /// <param name="newTable"></param>
    public void Merge(TableHeader newTable)
    {
      Merge(newTable, null);
    }
    public void Merge(TableHeader newTable, string newTableSource)
    {
      if (!canMergeNewSource) // if i cannot have a new source merged into me, then just do nothing
        return;

      if (canMergeNewSource)
      {
        bool firstRow = true;
        foreach (TableHeaderUniqueKey key in newTable.rows.Keys)
        {
          TableHeaderRow row = newTable.rows[key];
          row.Source = newTableSource;
          TableHeaderRow oldRow = Row(key);
          // if the existing operator is delete, override the incoming operator
          string op = row.Op;
          if (firstRow)
          {
            if (op == OPERATORSTOPMERGE)  // if new table coming in has an equal operator
            {
              //ReplaceWith(newTable);      // then ignore the existing data and replace it with the incomeing table
              row.Op = OPERATORADD;       // reset the operator 
              canMergeNewSource = false;  // and set the flag so it will not merge new stuff again
              //return;
            }
            firstRow = false;
          }
          if (oldRow != null)
            if (oldRow.Op == OPERATORDELETE)
              op = oldRow.Op;
            else
              if (oldRow.Op == OPERATORSTOPMERGE)
              {
                canMergeNewSource = false;
                break;
              }

          switch (op)
          {
            case OPERATORADD:
              if (!rows.Contains(key) && !isDeletedKey(key))  // only add this row if the key is not in the deleted key list
                rows.Add(key, row);
              break;
            case OPERATORDELETE:
              if (!IsDeleted(key))
                deletedKeyList.Add(key, row.Source);
              //if (rows.ContainsKey(key))
              //  Remove(key);
              break;
            case OPERATORSTOPMERGE:
              canMergeNewSource = false;
              if (rows.Contains(key))
                AddRow(row, true);
              else
                rows.Add(key, row);
              break;
          }
          //if (!(canMergeNewSource))
          //  break;
        }
        foreach (TableHeaderUniqueKey key in newTable.deletedKeyList.Keys)
          if (!isDeletedKey(key))
            if (!(IsDeleted(key)))
            deletedKeyList.Add(key, newTableSource);
        // deleted rows were not being actually deleted using the final resulting deleted key list
        TableHeaderUniqueKey[] arrKeys = new TableHeaderUniqueKey[deletedKeyList.Count];
        deletedKeyList.Keys.CopyTo(arrKeys, 0);
        foreach (TableHeaderUniqueKey key in arrKeys)
          if (rows.Contains(key))
            Remove(key);
      }
    }
    private bool isDeletedKey(TableHeaderUniqueKey key)
    {
      foreach (TableHeaderUniqueKey testKey in deletedKeyList.Keys)
      {
        if (testKey.keyEquals(key))
          return true;
      }
      return false;
    }
    public void ReplaceWith(TableHeader newTable)
    {
      columns = newTable.columns;
      rows = newTable.rows;
      tableSource = newTable.tableSource;
      dataTypeSource = newTable.dataTypeSource;
      attributeName = newTable.attributeName;
      keyList = newTable.keyList;
      keyNames = newTable.keyNames;
      sequence = newTable.sequence;
      hasDuplicates = newTable.hasDuplicates;
      hasDictionaryHeader = newTable.hasDictionaryHeader;
      hasHeader = newTable.hasHeader;
      tableSource = newTable.tableSource;
      includeDeletedRecords = newTable.includeDeletedRecords;
      enforceGoodHeaders = newTable.enforceGoodHeaders;
      header = newTable.header;
    }    /// <summary>
    /// Converts this into an ItemType. Each Row is an Item. Each cell is a TAGAttribute
    /// </summary>
    /// <returns>This table in an ItemType format</returns>
    public ItemType ToItemType()
    {
      ItemType rowList = new ItemType();
      rowList.ID = attributeName;
      int rownbr = 0;
      foreach (TableHeaderRow row in this)
      {
        Item i = new Item();
        i.ID = (rownbr++).ToString();
        foreach (TAGAttribute a in row)
          i.Add(a);
        rowList.Add(i);
      }
      return rowList;
    }
    /// <summary>
    /// Returns the value of a cell in the row that matches key, and the specified column name
    /// </summary>
    /// <param name="key"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public object Cell(object key, string columnName)
    {
      TableHeaderRow row = this[key];
      return row[columnName];
    }
    /// <summary>
    /// Overload that yields a value based on the zero-=based index (iRow, iCol) of the table. HeaderRow is NOT taken into account, so
    /// first data row is iRow=0.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public object Cell(int row, int column)
    {
      TableHeaderRow thisRow = this[row];
      return thisRow[column];
    }
    /// <summary>
    /// Clears all rows from the instance. Note that Header definition is NOT cleared.
    /// </summary>
    public void Clear()
    {
      rows.Clear();
    }
    /// <summary>
    /// Returns the first row in the table where the key is equal to the specified key. If there is no match, it returns the row 
    /// with they key that is closest to the key but not greater than the key
    /// </summary>
    /// <param name="key">Key to match</param>
    /// <returns>Row that matches the criteria</returns>
    public TableHeaderRow FindFirst(TableHeaderUniqueKey key)
    {
      TableHeaderRow returnRow = null;
      if (rows.Count == 0)
        return returnRow;
      foreach (TableHeaderUniqueKey compareKey in rows.Keys)
      {
        TableHeaderRow row = rows[compareKey];
        bool includeRow = (!(row).Deleted || IncludeDeletedRows);
        if (includeRow)
          if (compareKey.CompareTo(key) > 0)
            break;
          else
            returnRow = row;
      }
      return returnRow;
    }
	
    /// <summary>
    /// True of the incoming TableHeader is the same as this one
    /// </summary>
    /// <param name="th"></param>
    /// <returns></returns>
    public bool Equals(TableHeader th)
    {
      return (CompareTo(th) == 0);
    }
	
    /// <summary>
    /// Takes a dataset and reloads the row data. Note. The DataSet must have the same number of columns. 
    /// Also, the columns names must match (case insensitive)
    /// </summary>
    /// <param name="ds">DataSet to use to reload the table</param>
    public void LoadDataSet(DataSet ds)
    {
      if (ds == null || ds.Tables.Count == 0)
        return;
      DataTable tbl = ds.Tables[0];
      DataColumnCollection dataColumns = tbl.Columns;
      if (tbl.Columns.Count == columns.Count)
      {
        rows.Clear();
        foreach (DataRow dataRow in tbl.Rows)
        {
          object[] values = new object[tbl.Columns.Count];
          for (int iCol = 0; iCol < tbl.Columns.Count; iCol++)
          {
            string colName = dataColumns[iCol].ColumnName.ToLower();
            if (columns.Contains(colName))
              values[iCol] = dataRow[colName];
            else
            {
              TAGExceptionMessage tm = new TAGExceptionMessage("TableHeader", "LoadDataSet", "DataSet column name does not exist in the Table");
              tm.AddParm(colName);
              throw new Exception(tm.ToString());
            }
          }
          TableHeaderRow row = new TableHeaderRow(columns, values);
          TableHeaderUniqueKey key = new TableHeaderUniqueKey(columns, values, keyNames);
          rows.Add(key, row);
        }
      }
      else
      {
        TAGExceptionMessage tm = new TAGExceptionMessage("TableHeader", "LoadDataSet", "DataSetColumns do not match TableHeader Columns");
        tm.AddParm("DataSet Columns=" + tbl.Columns.Count.ToString());
        tm.AddParm("TableHeader Columns=" + columns.Count.ToString());
        throw new Exception(tm.ToString());
      }
      
    }
    /// <summary>
    /// Overload that makes this object IComparable
    /// </summary>
    /// <param name="tableHeaderString"></param>
    /// <returns></returns>
    public int CompareTo(string tableHeaderString)
    {
      TableHeader compareTable = new TableHeader(attributeName, valueType, tableHeaderString, dictionary);
      return CompareTo(compareTable);
    }
    /// <summary>
    /// Returns -1 if incoming table is less than the current table, 0 if it is equal,
    /// -1 if it is less. Note the following rules:
    /// If compareTable has fewer columns, it is automatically less than, and
    /// if it has more columns, it is automatically greater than.
    /// Likewise, if it has fewer rows, it is less than.
    /// If number of rows and columns match, then we go left to right and then up to down, 
    /// and compare individual cells, using the datatype specified by the column
    /// header attribute. the first one that it finds not equal, it returns
    /// the CompareTo() of that object (-1 = <, 0 = ==, +1 = >).
    /// If it gets to the end and all was the same, it returns a zero (equal to).
    /// </summary>
    /// <param name="compareTable"></param>
    /// <returns></returns>
    public int CompareTo(TableHeader compareTable)
    {
      if (Columns.Count < compareTable.columns.Count)
        return -1;
      if (Columns.Count > compareTable.columns.Count)
        return +1;
      if (Count < compareTable.Count)
        return -1;
      if (Count > compareTable.Count)
        return +1;
      // if headers have the same names and order, they are equal
      foreach (TableHeaderColumn col in compareTable.Columns)
      {
        int gtlt = columns[compareTable.columns.IndexOf(col.ID)].ID.CompareTo(col.ID);  // is the comparetable column id GreaterThan or LessThan (GTLT) this one? 
        if (gtlt != 0)      // if not equalto (ie, is GT or LT)
          return gtlt;      // than return -1 or +1
      }
      foreach (TableHeaderUniqueKey compareKey in compareTable.rows.Keys)
      {
        TableHeaderRow compareRow = compareTable.rows[compareKey];
        if (!Contains(compareKey))
          return -1;
        TableHeaderRow row = rows[compareKey];
        for (int iCol = 0; iCol < row.Cells.Count; iCol++)
        {
          string colID = row.Cells[iCol].ID;
          Field cell = row[iCol];
          Field compareCell = compareRow.Cells[colID];
          int gtlt = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CompareTo, cell.Value, cell.DataType, compareCell.Value, compareCell.DataType);
          if (gtlt != 0)
            return gtlt;
        }
      }
      return 0;       // all tests passed, so they are equal!
    }
    public int CompareTo(object o)
    {
      string type = o.GetType().Name.ToLower();
      if (type == TAGFunctions.DATATYPESTRING)
        return CompareTo((string)o);
      else
        if (type.EndsWith(TAGFunctions.DATATYPETABLEHEADER))
          return CompareTo((TableHeader)o);
        else
          return -1;
    }
    public InvalidEntries InvalidEntryList(TableHeaderRow row) 
    {
      InvalidEntries returnList = new InvalidEntries();
      foreach (Field cell in row.Cells)
      {
        bool warn;
        string errorMsg;
        if (!dictionary.IsValid(cell.OriginalID, cell.Value, columns[cell.ID].PickList, "Value", out warn, out errorMsg))
        {
          InvalidEntry error = new InvalidEntry();
          error.AttributeName = cell.OriginalID;
          error.Context = "Table Header Row";
          error.ErrorMessage = errorMsg;
          returnList.Add(error);
        }
      }
      return null; 
    }
    public object Clone()
    {
      TableHeader th = cloneWithNoRows();
      foreach (TableHeaderUniqueKey key in rows.Keys)
      {
        TableHeaderRow row = rows[key];
        th.rowCollection.Add((TableHeaderUniqueKey)key.Clone(), (TableHeaderRow)row.Clone());
      }
      foreach (KeyValuePair<TableHeaderUniqueKey, string> dk in deletedKeyList)
        th.deletedKeyList.Add((TableHeaderUniqueKey)dk.Key.Clone(), dk.Value);
      return th;
    }
    /// <summary>
    /// Support enumeration (supports foreach).
    /// Enumerator overload to support LINQ use in Reports and other modules
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator)new TableHeaderEnumerator(this);
    }
    #endregion

    #region output methods
    public PickList ToPickList()
    {
      PickList listReturn = new PickList();
      foreach (TableHeaderColumn col in columns)
        listReturn.AddColumn(col.OriginalID, col.Caption);
      int iRow = 0;
      foreach (TableHeaderRow row in this)
      {
        listReturn.AddRow();
        for (int iCol = 0; iCol < columns.Count; iCol++)
          listReturn[iRow, iCol] = row[iCol].Value;
        iRow++;
      }
      return listReturn;
    }
    /// <summary>
    /// Returns the TableHeader in storage format, as a string with row and column delimiters, enclosed in parentheses.
    /// Note that this also resets the DataType string and array to conform to what is stored in the Columns collection
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      if (columns.Count == 0)
        return string.Empty;
      StringBuilder strBuilder = new StringBuilder(GetLength(0) * GetLength(1) * 5);
      strBuilder.Append(TAGFunctions.LEFTCHAR);
      // first, populate the header
      bool firstColumn = true;
      bool firstRow = true;
      foreach (TableHeaderColumn col in columns)
      {
        if (firstColumn)
        {

          firstColumn = false;
        }
        else
          strBuilder.Append(TAGFunctions.COLSEPARATORCHAR);
        strBuilder.Append(col.OriginalID);
      }
      foreach (TableHeaderRow row in this)
      {
        if (!row.Deleted)
        {
          strBuilder.Append(TAGFunctions.ROWSEPARATORCHAR);
          firstColumn = true;
          foreach (Field a in row.Cells)
          {
            if (firstColumn)
            {
              firstColumn = false;
              if (firstRow)
              {
                if (!canMergeNewSource)
                  strBuilder.Append(OPERATORSTOPMERGE); // if there was an equal sign in my past, then add it here to the string
                firstRow = false;
              }
              if (row.Op == OPERATORDELETE)
                strBuilder.Append(row.Op);
            }
            else
              strBuilder.Append(TAGFunctions.COLSEPARATORCHAR);
            if (columns[a.ID].DataType.ToLower() == TAGFunctions.DATATYPEDATETIME)
              strBuilder.Append(((DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, a.Value)).ToShortDateString());
            else
              if (!a.IsNull)
              {
                string strValue = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, a.Value);
                if (!canMergeNewSource && strBuilder[strBuilder.Length - 1].ToString() == OPERATORSTOPMERGE && strValue.StartsWith(OPERATORSTOPMERGE))
                  strBuilder.Append(strValue.Substring(1));
                else
                  strBuilder.Append(strValue);
              }
          }
        }
      }
      foreach (TableHeaderUniqueKey key in deletedKeyList.Keys)
      {
        strBuilder.Append(TAGFunctions.ROWSEPARATORCHAR);
        strBuilder.Append(OPERATORDELETE);
        strBuilder.Append(key.ToRowString(columns));
      }
      strBuilder.Append(TAGFunctions.RIGHTCHAR);
      return strBuilder.ToString();
    }
    ///// <summary>
    ///// Returns an object array version of teh table. The header is included
    ///// </summary>
    ///// <returns></returns>
    //private object[,] ToArray()
    //{
    //  object[,] returnTable = new object[rows.Count + 1,columns.Count];    // create the array. Add a row for the header
    //  int iRow = 0;
    //  int iCol = 0;
    //  // First, populate the header row of the array
    //  for (iCol = 0; iCol < columns.Count; iCol++)
    //    returnTable[iRow,iCol] = columns[iCol].OriginalID;
    //  // Then, populate the data rows, adjusting the Row subscript in the array for the header row
    //  for (iRow = 0; iRow < rows.Count; iRow++)
    //    for (iCol = 0; iCol < columns.Count; iCol++)
    //    {
    //      KeyValuePair<TableHeaderUniqueKey, SortedDictionary<int, TableHeaderRow>> keyValue = rows.ElementAt(iRow);
    //      SortedDictionary<int, TableHeaderRow> entry = keyValue.Value;
    //      TableHeaderRow row = entry[0];
    //      returnTable[iRow,iCol] = row[iCol].Value; // get the TAGAttribute.Value of the zeroth version of the cell at { iRow, iCol }
    //    }
    //  return returnTable;
    //}
    /// <summary>
    /// returns a dataset Equivalent to the table header
    /// </summary>
    /// <returns></returns>
    public DataSet ToDataSet()
    {
      DataSet ds = new DataSet();
      DataTable tbl = new DataTable(attributeName);
      foreach (TableHeaderColumn col in columns)
      {
        // first, normalize the datatype to C# standards
        string dataType = col.DataType;
        Type t = toValidType(dataType);
        DataColumn dataCol = new DataColumn(col.OriginalID, t);
        tbl.Columns.Add(dataCol);
      }
      foreach (TableHeaderRow row in rows)
      {
        DataRow dataRow = tbl.NewRow();
        foreach (Field a in row)
        {
          DataColumn col = tbl.Columns[a.OriginalID];
          if (a.Value == null || a.IsNull)
            dataRow[col] = System.DBNull.Value;
          else
            dataRow[col] = a.Value;
        }
        tbl.Rows.Add(dataRow);
      }
      ds.Tables.Add(tbl);
      return ds;
    }
    ///// <summary>
    ///// This converts a TAG table string into a XML formatted string (supportes embedded tables...)
    ///// </summary>
    ///// <param name="tableString"></param>
    ///// <param name="dataTypes"></param>
    ///// <param name="bHasHeader"></param>
    ///// <returns></returns>
    //public string ToXML()
    //{
    //  int i, j;
    //  string myXML = null;
    //  PickList myTable = ToPickList();
    //  int nbrColumns = myTable.GetLength(1);

    //  //Here we beign our transformation from table format to XML format...
    //  myXML = "<attributetable hasheader='" + (hasHeader ? "True" : "False") + "' >";
    //  if (hasHeader)
    //  {
    //    myXML += "<header>";
    //    for (j = 0; j < myTable.GetLength(1); j++)
    //      myXML += "<column name='" + myTable[0, j] + "'/>";
    //    myXML += "</header>";
    //  }
    //  for (i = (hasHeader ? 1 : 0); i < myTable.GetLength(0); i++)
    //  {
    //    myXML += "<row>";
    //    for (j = 0; j < myTable.GetLength(1); j++)
    //    {
    //      string myValue = "(null)";
    //      if (myTable[i, j] != null)
    //      {
    //        myValue = myTable[i, j].ToString();
    //        if (myValue.StartsWith(TAGFunctions.BEGINSTRINGCHAR.ToString()))
    //        {
    //          TableHeader th = new TableHeader(columns[i].OriginalID, myValue, dictionary);
    //          myValue = th.ToXML();
    //          //myValue = ToXML(myValue,dataTypes,hasHeader);
    //        }
    //      }
    //      myXML += "<cell name='"
    //        + "' datatype='" + dataTypes[j] + "' >"
    //        + myValue
    //        + "</cell>";
    //    }
    //    myXML += "</row>";
    //  }
    //  myXML += "</attributetable>";
    //  return myXML;
    //}
    /// <summary>
    /// This converts a TAG table string into a XML jqGrid formatted string (doesn't supportes embedded tables yet...)
    /// </summary>
    /// <param name="maxCols">Maximum number of columns to return</param>
    /// <param name="maxRows">Maximum number of rows to return</param>
    /// <returns></returns>
    public string ToXMLGUI(int maxCols, int maxRows)
    {
      PickList table = ToPickList();

      if (table == null)
        return EMPTYGUITABLE;

      StringBuilder sb = new StringBuilder();
      XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
      xmlWriterSettings.OmitXmlDeclaration = true;
      XmlWriter xmlWriter = XmlWriter.Create(sb, xmlWriterSettings);

      xmlWriter.WriteStartDocument();
      xmlWriter.WriteStartElement("rows");
      int nbrRows = (rows.Count < maxRows ? rows.Count : maxRows);
      int nbrCols = (columns.Count < maxCols ? columns.Count : maxCols);
      for (int i = 0; i < nbrRows; i++)
      {
        xmlWriter.WriteStartElement("row");
        for (int j = 0; j < nbrCols; j++)
        {
          string value = "";
          if (i < table.GetLength(0) && j < table.GetLength(1))
            if (table[i, j] != null)
              value = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, table[i, j]);

          xmlWriter.WriteElementString("cell", value);
        }
        xmlWriter.WriteEndElement(); // row
      }

      xmlWriter.WriteEndElement(); // rows
      xmlWriter.WriteEndDocument();

      xmlWriter.Flush();
      xmlWriter.Close();

      return sb.ToString();
    }
    /// <summary>
    /// Returns a version of this tableheader sorted according to the supplied list of columns
    /// </summary>
    /// <param name="columnList">Comma-delimited list of column names in order of which the Sort is required</param>
    /// <returns></returns>
    public TableHeader Sort(string columnList)
    {
      return Sort(columnList, false);
    }
    /// <summary>
    /// Overload that allows you to specify whether you want a cloned copy, or just to sort in place
    /// </summary>
    /// <param name="columnList"></param>
    /// <param name="cloneFirst"></param>
    /// <returns></returns>
    public TableHeader Sort(string columnList, bool cloneFirst)
    {
      string[] keys = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.FromList, columnList);
      foreach (string keyName in keys)
        if (!columns.Contains(keyName))
          throw new Exception("TableHeader.Sort(columnList): columnList<" + columnList + "> contains a column not in this TableHeader");
      TableHeader th;
      if (cloneFirst)
        th = (TableHeader)this.Clone();
      else
        th = this;
      th.rows.Sort(keys);
      return th;
    }

    /// <summary>
    /// Converts this tableheader into a text block, where all cells are strings, columns line up, and cells wrap
    /// as necessary
    /// </summary>
    /// <param name="colWidth">How wide should each column be?</param>
    /// <returns></returns>
    public string ToText(int colWidth)
    {
      int[] colWidths = new int[this.GetLength(1)];
      for (int i = 0; i < colWidths.GetLength(0); i++)
        colWidths[i] = colWidth;
      return ToText(colWidths);
    }
    public string ToText()
    {
      if (this.GetLength(0) > 0 && this.GetLength(1) > 0)
      {
        int nbrCols = this.GetLength(1);
        int[] colWidths = new int[nbrCols];
        string cleanedHeader = string.Empty;

        string[] headerCols = new string[columns.Count];
        for (int i = 0; i < columns.Count; i++)
          headerCols[i] = columns[i].Caption;

        for (int j = 0; j < nbrCols; j++)   // for each column
        {
          colWidths[j] = headerCols[j].Length;
          for (int k = 0; k < this.GetLength(0); k++)     // find the widest one
          {
            int newLen = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, this[k, j])).Length;
            if (newLen > colWidths[j])
            {
              colWidths[j] = newLen;                        // and make this column that wide
            }
          }
        }

        return ToText(colWidths);
      }
      return string.Empty;
    }
    /// <summary>
    /// Converts an AT into a text block, where all cells are strings, columns line up, and cells wrap
    /// as necessary
    /// </summary>
    /// <param name="myTable">attribute table in array format</param>
    /// <param name="colWidth">How wide should each column be?</param>
    /// <returns></returns>
    public string ToText(int[] colWidths)
    {
      int nbrCols = this.GetLength(1);
      int nbrRows = this.GetLength(0);
      const int maxLines = 512;           // Ok, we cannot have a column wrap more than this nbr of lines
      const char sp = ' ';                // a space
      const string nl = "\r\n";               // new line
      int iLine = 0;                      // iLine is a print line. There could be many of these for each row
      int iCol = 0;                       // for each colmn
      int iRow = 0;                       // for each row in the table
      int maxColWidth = 0;
      for (int i = 0; i < colWidths.GetLength(0); i++)
        if (colWidths[i] > maxColWidth)
          maxColWidth = colWidths[i];
      /*
       * for each cell, we can wrap up to maxlines. So, we populate this array, one for each column,
       * and as many lines as we need to display with wrapping
       */
      string[,] rowPrint = new string[nbrCols, maxLines];
      /*
       * This is our final string. It is one long text string, with padding to line up
       * the columns, and with a newline at the end of each line. So, we populate
       * our rowPrint array for each row, then we go through it and append one
       * line at a time, with contents for each cell in that line.
       * We finish it up at the end of the row with a new line
       */
    
      
      StringBuilder sbText = new StringBuilder(nbrRows * (nbrCols * maxColWidth + 2));
      
      // add table header manually
      string cleanedHeader = string.Empty;
      if (header != null)
        cleanedHeader = this.header.Replace("(", "").Replace(")", "");
      string[] headerCols = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, cleanedHeader, new string[] { "~" });
      for (int i = 0; i < headerCols.Length; i++)
      {
        sbText.Append(((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, headerCols[i])).PadRight(colWidths[i])); // add the contents, or just spaces
        sbText.Append(sp);
      }
      sbText.Append(nl);            // end of the line, so we add a new line

      for (iRow = 0; iRow < nbrRows; iRow++)    // for each row in our table.
      {
        int topNbrLines = 0;                    // how many lines for this row do we print
        /*
         * first, we go through each column and populate rowPrint by doing the wrapping as needed
         */
        for (iCol = 0; iCol < nbrCols; iCol++)
        {
          string thisCell = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, this[iRow, iCol]); // start with the entire contents of the cell
          int nbrLines = 0;                     // and no print lines
          bool cellDone = false;                // keep wrapping until we are done
          while (!cellDone)                     // while there is still more contents to wrap
          {
            rowPrint[iCol, nbrLines++] = thisCell.Substring(0, Math.Min(colWidths[iCol], thisCell.Length));  // cut it up and put in the current piece
            if (thisCell.Length <= colWidths[iCol] || nbrLines >= maxLines)     // if we are done, or have exceeded the max
              cellDone = true;                  // then quit
            else
              thisCell = thisCell.Substring(colWidths[iCol]);  // otherwise take off what we have printed and keep going
          }
          topNbrLines = Math.Max(topNbrLines, nbrLines);  // lines to print is for the tallest column this time around
        }
        for (iLine = 0; iLine < topNbrLines; iLine++)   // for each line to print
        {
          for (iCol = 0; iCol < nbrCols; iCol++)        // for each column (whether it has anything or not in this row
          {
            sbText.Append(((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, rowPrint[iCol, iLine])).PadRight(colWidths[iCol])); // add the contents, or just spaces
            sbText.Append(sp);
          }
          sbText.Append(nl);            // end of the line, so we add a new line
        }
      }
      return sbText.ToString();         // return the "big string" when we are done
    }
    /// <summary>
    /// Gets the rows in the tableheader that EntityAttributes.Save() needs to save if not in raw mode:
    /// These are any row that is dirty or that came from the "local" item
    /// </summary>
    /// <param name="compareSource">Entity.ItemType.Item string of the "local" item</param>
    /// <returns></returns>
    public TableHeader getDirtyAndLocalRows(string compareSource)
    {
      TableHeader th = cloneWithNoRows(); // get shell
      foreach (TableHeaderUniqueKey key in rows.Keys)
      {
        TableHeaderRow row = rows[key];
        if (row.Deleted)
        {
          if (!th.IsDeleted(key))
            th.deletedKeyList.Add(key, row.Source);
        }
        else
          if (row.Dirty || compareSource.Equals(row.Source, StringComparison.CurrentCultureIgnoreCase))
            th.rowCollection.Add((TableHeaderUniqueKey)key.Clone(), (TableHeaderRow)row.Clone());
      }
      foreach (KeyValuePair<TableHeaderUniqueKey, string> dk in deletedKeyList)
        if (!th.IsDeleted(dk.Key))
          th.deletedKeyList.Add((TableHeaderUniqueKey)dk.Key.Clone(), dk.Value);
      return th;
    }
    #endregion

    #region lookup methods
    /// <summary>
    /// Returns the value found by using keyList to find a row, and then getting the value in the column of that row that is named columnName.
    /// If no match is found and a Default row exists, use that
    /// </summary>
    /// <param name="keyList">Comma separated values that match thisAttribute.Dictionary.TableKeyList in order and number</param>
    /// <param name="columnName">Column name to get the value from once we find the row</param>
    /// <returns></returns>
    public object Lookup(string keyList, string columnName)
    {
      string functionName = "Lookup";
      checkLookupParms(functionName, keyList, columnName);
      if (!columns.Contains(columnName))
        throwColumnNotFound(functionName, columnName);
      TableHeaderRow row = this[keyList];
      if (row == null)
      {
        row = this["default"];
        if (row == null)
          return string.Empty;
      }
      Field a = row[columnName];
      if (a == null)
        return string.Empty;
      else
        return a.Value;
    }
    /// <summary>
    /// Like Lookup, but finds the last row where the key is less than or equal to the keyList key. Default does not affect the range lookup
    /// </summary>
    /// <param name="keyList">Comma separated values that match thisAttribute.Dictionary.TableKeyList in order and number</param>
    /// <param name="columnName">Column name to get the value from once we find the row</param>
    /// <returns></returns>
    public object LookupRange(string keyList, string columnName)
    {
      string functionName = "LookupRange";
      checkLookupParms(functionName, keyList, columnName);
      TableHeaderUniqueKey key = new TableHeaderUniqueKey(columns, keyList, keyNames);
      TableHeaderRow row = FindFirst(key);
      if (row == null)
        return string.Empty;
      Field a = row[columnName];
      if (a == null)
        return string.Empty;
      else
        return a.Value;
    }
    /// <summary>
    /// Returns a sub-table constructed by finding all rows that match the keyList value (Default logic works like Lookup). Then use
    /// columnName as the first column in the new table. All columns before columnName are not included
    /// </summary>
    /// <param name="keyList">Comma separated values that match thisAttribute.Dictionary.TableKeyList in order and number</param>
    /// <param name="columnName">Column name which is the first column in the new table</param>
    /// <returns></returns>
    public TableHeader LookupTable(string keyList, string columnName)
    {
      string functionName = "LookupTable";
      checkLookupParms(functionName, keyList, columnName);        // check for empty parameters
      TableHeaderUniqueKey key = new TableHeaderUniqueKey(columns, keyList, keyNames);  // build a key to lookup with
      // Now construct the columns for the new table
      TableHeaderColumnsCollection newCols = new TableHeaderColumnsCollection();
      string colCompare = columnName.ToLower();
      bool columnFound = false;
      int iStartCol = 0;
      foreach (TableHeaderColumn col in columns)
      {
        if (col.ID == colCompare)
          columnFound = true;
        if (columnFound)
          newCols.Add(col.OriginalID, col);
        else
          iStartCol++;   // get the column number
      }
      if (!columnFound)
        throwColumnNotFound(functionName, columnName);
      TableHeader th = new TableHeader(attributeName, newCols, dictionary);
      th = lookupTable(th, key, newCols, iStartCol);  // get rows with matching keys
      if (th.Count == 0)          // if none were found
      {
        key = new TableHeaderUniqueKey(columns, "default", keyNames);
        th = lookupTable(th, key, newCols, iStartCol); // then get the default rows
      }
      return th;
    }
    /// <summary>
    /// Overload where default values in the table always match
    /// </summary>
    /// <param name="columnList">string array of column names</param>
    /// <param name="valueList">object array of values, in the same order as columnnames</param>
    /// <returns></returns>
    public TableHeader Filter(string[] columnList, object[] valueList)
    {
      return Filter(columnList, valueList, true, false);
    }
    /// <summary>
    /// Returns a table where all rows match the criteria set by columnlist and valuelist, such that:
    /// <para>column[i] = value[i]
    /// </para>
    /// Note that For now, only exact non-casesenstive matches are supported
    /// </summary>
    /// <param name="columnList">Array of column names</param>
    /// <param name="valueList">object array of values, in the same order as columnnames</param>
    /// <param name="includeDefault">Should rows be included if they have "default" as their value?</param>
    /// <param name="startsWith">Do we use StartsWith to match values in the column list?</param>
    /// <returns></returns>
    public TableHeader Filter(string[] columnList, object[] valueList, bool includeDefault, bool startsWith)
    {
      if (columnList == null || valueList == null || columnList.GetLength(0) != valueList.GetLength(0))
      {
        TAGExceptionMessage tm = new TAGExceptionMessage("TableHeader", "Filter", "Number of values must match number of columns specified");
        tm.AddParm(attributeName);
        if (columnList == null)
          tm.AddParm("null");
        else
          tm.AddParm((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.ToList, (object)columnList));
        if (valueList == null)
          tm.AddParm("null");
        else
          tm.AddParm((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.ToList, (object)valueList));
        throw new Exception(tm.ToString());
      }
      TableHeader th = new TableHeader(attributeName, columns, dictionary);
      foreach (TableHeaderRow row in this)
      {
        if (matchesFilter(row, columnList, valueList, includeDefault, startsWith))
          th.AddNewRow(row);
      }
      return th;
    }
    /// <summary>
    /// returns a copy of the table, but only includes columns starting with startColumn and ending with endColumn
    /// </summary>
    /// <param name="startColumn">First Column to include</param>
    /// <param name="endColumn">Last column to include</param>
    /// <returns></returns>
    public TableHeader TrimTable(string startColumn, string endColumn)
    {
      string compareStart = startColumn.ToLower();
      string compareEnd = endColumn.ToLower();
      TableHeaderColumnsCollection newCols = new TableHeaderColumnsCollection();
      bool copyColumn = false;
      bool firstkeyName = true;
      string newKeyList = string.Empty;
      foreach (TableHeaderColumn newCol in columns)
      {
        if (!copyColumn && newCol.ID == compareStart) // if we have not found the start column yet, and if this is it
          copyColumn = true;                          // then start copying columns
        if (copyColumn)                               // if we are copying columns
        {
          newCols.Add(newCol.OriginalID, newCol);                        // then add this one to the new list
          if (newCol.ID == compareEnd)                // and if this is the end column
            copyColumn = false;                       // then make it the last one
          if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.inList, keyNames, newCol.ID)) // is this column a key?
          {
            if (firstkeyName)
              firstkeyName = false;
            else
              newKeyList += ", ";
            newKeyList += newCol.OriginalID;          // then add it to the list
          }
        }
      }
      if (newKeyList == string.Empty)  // if there was no key in the new set of columns
        newKeyList = newCols[0].OriginalID;  // then make the new key = the first column
      TableHeader th = new TableHeader(attributeName, newCols, dictionary);
      th.keyList = newKeyList;
      string[] newKeyNames = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.FromList, newKeyList);
      th.keyNames = newKeyNames;
      foreach (TableHeaderRow oldRow in rows)
      {
        object[] newValues = new object[newCols.Count];
        int iCol = 0;
        foreach (TableHeaderColumn newCol in newCols)
        {
          newValues[iCol++] = oldRow[newCol.ID].Value;
        }
        TableHeaderRow newRow = new TableHeaderRow(newCols, newValues);
        th.AddNewRow(newRow);
      }
      return th;
    }
    public TableHeader FindReplace(string strKey, string columnName, object oldValue, object newValue)
    {
      TableHeader th = (TableHeader)this.Clone(); // make a copy
      TableHeaderUniqueKey key = null;
      bool matchAll = false;
      if (strKey == null || strKey == string.Empty)
        matchAll = true;
      else 
        key = new TableHeaderUniqueKey(columns, strKey, keyNames); // make the key compare
      // for each entry in the rows collection
      foreach (TableHeaderUniqueKey oldKey in th.rows.Keys)
      {
        bool keyMatches = matchAll;
        if (!matchAll)
          if (key.keyEquals(oldKey))                   // if they are equal (not including the sequence)
            keyMatches = true;
        if (keyMatches)
        {
          TableHeaderRow row = rows[oldKey];  // get the most recent copy of the row
          if (!row.Deleted)                     // if it is not deleted
          {
            Field a = row[columnName];   // get the attribute that is the cell
            if ((int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CompareTo, a.Value, a.DataType, oldValue, a.DataType) == 0) // perform a strongly typed compare
              a.Value = newValue; // if they match, replace the old value with the new one
          }
        }
      }
      return th;  // now return the table
    }
    /// <summary>
    /// Creates a new TableHeader with one column from this TableHeader, using columnName, and only returns distinct values
    /// </summary>
    /// <param name="columnName">Name of the column to create the new table from</param>
    /// <returns></returns>
    public TableHeader DistinctValue(string columnName)
    {
      if (!columns.Contains(columnName))
      {
        TAGExceptionMessage tm = new TAGExceptionMessage("TableHeader", "DistinctValue", "Column name does not exist");
        tm.AddParm(columnName);
        throw new Exception(tm.ToString());
      }
      TableHeader th = new TableHeader();
      TableHeaderColumn col = new TableHeaderColumn(columnName);
      th.Columns.Add(columnName, col);
      th.KeyNames = new string[] { columnName }; 
      foreach (TableHeaderRow oldRow in Rows)
      {
        object[] values = new object[] { oldRow[columnName].Value };
        TableHeaderUniqueKey key = new TableHeaderUniqueKey(th.Columns, values, th.KeyNames);
        if (!th.Contains(key))
        {
          TableHeaderRow newRow = new TableHeaderRow(th.Columns, values);
          th.AddRow(newRow);
        }
      }
      return th;
    }
    #endregion
    #endregion

    #region private methods
    private TableHeader cloneWithNoRows()
    {
      TableHeader th = new TableHeader();
      foreach (TableHeaderColumn col in columns)
        th.columns.Add(col.OriginalID, (TableHeaderColumn)col.Clone());      
      th.tableSource = tableSource;
      th.dataTypeSource = dataTypeSource;
      th.header = header;
      th.CanMergeNewSource = canMergeNewSource;
      //th.dataTypes = dataTypes;
      th.hasHeader = hasHeader;
      th.hasDictionaryHeader = hasDictionaryHeader;
      th.validHeader = validHeader;
      th.hasDuplicates = hasDuplicates;     // true if the key defined for this table has duplicates
      th.thisReturnsFindFirst = thisReturnsFindFirst;
      th.sequence = sequence;               // used to create artificial uniqueness if there are duplicate keys
      th.attributeName = attributeName;
      th.valueType = valueType;
      th.keyList = keyList;
      th.keyNames = keyNames;
      return th;
    }
    private TableHeader lookupTable(TableHeader th, TableHeaderUniqueKey key, TableHeaderColumnsCollection newCols, int iStartCol)
    {
      foreach (TableHeaderUniqueKey oldKey in rows.Keys)
      {
        if (oldKey.keyEquals(key))
        {
          TableHeaderRow row = rows[oldKey];
          object[] values = new object[newCols.Count];
          for (int i = iStartCol; i < columns.Count; i++)
          {
            Field a = row[i];   // get the zeroth version of this row, then use "i" as column subscript to get the cell/TAGAttribute there
            object oVal = null;             // value is null if not found
            if (a != null)
              oVal = a.Value;
            values[i - iStartCol] = oVal;          // set the value for the new row
          }
          TableHeaderRow newRow = new TableHeaderRow(newCols, values);
          th.AddNewRow(newRow);             // use AddNewRow so we get duplicates if there are any
        }
      }
      return th;
    }
    private void parseXMLHeader(XmlNodeList xmlAttrTblHdrCols)
    {
      columns.Clear();
      for (int j = 0; j < xmlAttrTblHdrCols.Count; j++)
      {
        string headerName = xmlAttrTblHdrCols[j].Attributes["name"].Value;
        loadOneColumn(headerName);
      }
    }
    /// <summary>
    /// takes the header and turns it into valid column definitions, while consulting the dictionary
    /// </summary>
    private void parseHeader()
    {
      string[] emptyHeaders = new string[0];
      validHeader = true;
      if (header.Length == 0)
      {
        validHeader = false;
        if (enforceGoodHeaders)
          return;
      }
      string[] delims = { TAGFunctions.COLSEPARATORCHAR.ToString() };
      
      string[] hNames = 
        (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, 
        (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, header), delims);
      
      if (hNames.GetLength(0) == 0)
      {
        validHeader = false;
        if (enforceGoodHeaders)
          return;
      }
      foreach (string headerName in hNames)
        loadOneColumn(headerName);
      hasHeader = true;
      return;
    }
    private void loadOneColumn(string headerName)
    {
      TableHeaderColumn newCol = new TableHeaderColumn();
      newCol.ID = headerName;
      string headerDataType = TAGFunctions.DATATYPESTRING;
      if (dictionary.ValidAttribute(headerName))
      {
        newCol.DataType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, dictionary.AttributeProperty(headerName, ATTRIBUTEDATATYPE));
        newCol.Mask = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, dictionary.AttributeProperty(headerName, ATTRIBUTEMASK));
        newCol.Required = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, dictionary.AttributeProperty(headerName, ATTRIBUTEREQUIRED));
        newCol.PickListAttribute = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, dictionary.AttributeProperty(headerName, ATTRIBUTEPICKLIST));
        newCol.Caption = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, dictionary.AttributeProperty(headerName, ATTRIBUTEDESCRIPTION));
      }
      else
      {
        newCol.Caption = headerName;
        validHeader = false;
      }
      columns.Add(headerName, newCol);                          // add this column definition to the collection
    }
    /// <summary>
    /// Takes a TAG datatype (lowercased string) and returns a C# Type equivalent
    /// </summary>
    /// <param name="dataType">string TAG Data Type</param>
    /// <returns>Equivalent C# Data Type</returns>
    private Type toValidType(string dataType)
    {
      string goodType = TAGFunctions.DATATYPESTRING;
      switch (dataType.ToLower())
      {
        case TAGFunctions.DATATYPESTRING:
          goodType = "String";
          break;
        case TAGFunctions.DATATYPEDATETIME:
          goodType = "DateTime";
          break;
        case TAGFunctions.DATATYPEDECIMAL:
        case "money":
          goodType = "Decimal";
          break;
        case "bool":
          goodType = "Boolean";
          break;
        case TAGFunctions.DATATYPEINTEGER:
          goodType = "Int32";
          break;
        case "long":
          goodType = "Int64";
          break;
        default:
          goodType = dataType;
          break;
      }
      if (goodType == TAGFunctions.DATATYPETABLEHEADER)
        goodType = "TAGBOSS.Common.TableHeader";
      else
        goodType = "System." + goodType;
      Type t = Type.GetType(goodType);
      return t;
    }
    private void AddRow(TableHeaderRow row, bool newVersion)
    {
      TableHeaderUniqueKey key = new TableHeaderUniqueKey(columns, row, keyNames);
      // If this row is flagged with a DELETE command, we need to add it to the DeletedKeyList and that is all.
      if (row.Op == OPERATORDELETE)
      {
        if (!(IsDeleted(key)))
          deletedKeyList.Add(key, row.Source);
        return;
      }
      // Otherwise, we process it normally
      SortedDictionary<int, TableHeaderRow> entry = null;
      if (newVersion) // if the row has a key that is already in the collection, keep the existing entry and add a new version
      {
        if (rows.Contains(key))
          rows[key] = row;   // and now replace the existing version collection with the one we just created
        else
          rows.Add(key, row);
      }
      else     // don't create a new version, so each row, even if it has a dup key, must create a new row
      {
        if (rows.Contains(key))
        {
          if (!hasDuplicates)    // we did not already know this table has dups
            hasDuplicates = true;           // set the flag
          key.Sequence = ++sequence;    // add a new key with a sequence number
        }
        rows.Add(key, row);                   // now add this row to the collection
      }
    }
    //static public PickList LoadTable(string listSource, string[] pDataTypes)
    //{
    //  return LoadTable(listSource, pDataTypes, false);
    //}
    static public PickList LoadTable(string listSource, string[] pDataTypes, bool pHasHeader)
    {
      // if the table is empty, then return an empty array;
      PickList table = new PickList(false);
      if (listSource == null || listSource == string.Empty || listSource == "" || listSource == "()")
      {
        table = new PickList(false);
        listSource = "()";
        //return;
        return table;
      }
      StringBuilder token = new StringBuilder(listSource.Length);
      StringBuilder str = new StringBuilder(listSource);
      // first pass to find the number of rows and columns
      int irows = 0;
      int icols = 0;
      int parenDepth = -1;
      int maxNbrCols = 0;
      /*
       * see notes below. We added parendepth to allow for tokens that contain nested tables. 
       * We don't include columns in these in the column count
       */
      for (int iSub = 0; iSub < listSource.Length; iSub++)
      {
        switch (str[iSub])
        {
          case TAGFunctions.BEGINSTRINGCHAR:
            parenDepth++;
            break;
          case TAGFunctions.COLSEPARATORCHAR:
            if (parenDepth <= 0)
              icols++;
            break;
          case TAGFunctions.ROWSEPARATORCHAR:
            if (parenDepth <= 0)
            {
              irows++;
              if (icols > maxNbrCols)
                maxNbrCols = icols;
              icols = 0;
            }
            break;
          case TAGFunctions.ENDSTRINGCHAR:
            if (parenDepth <= 0)
              if (icols > maxNbrCols)
                maxNbrCols = icols;
            parenDepth--;
            break;
        }
      }
      icols = maxNbrCols;
      icols++;                       // number of columns is one more than fielddelims
      if (!pHasHeader)
        irows++;                       // number of rows is one more than rowdelims unless the first row is a header
      if (pDataTypes == null || pDataTypes.GetLength(0) != icols)
      {
        // we don't have any data types, so we create them.
        pDataTypes = new string[icols];
        for (int k = 0; k < icols; k++)
          pDataTypes[k] = TAGFunctions.DATATYPESTRING;
      }
      if (pHasHeader)
        table = new PickList();
      else
        table = new PickList(irows, icols);
      // Second pass to parse the string
      bool inString = false;                                             // ignore everything but between the parentheses
      int col = 0;                                                           // reset everything for the main table
      int row = 0;
      token.Length = 0;
      int toksub = 0;
      /*
       * Modified 8/4/2008 LLA. Sometimes we have to parse a table that has a table is one of the cells. Before,
       * this would cause the parsing to crash, since there are nested parenthesis. I had to add parenDepth, and check
       * it when we begin or end a pair of parenthesis. The outer set defines "inString". Anything outside of this pair is ignored.
       * Inside, if another pair of parens is found, it is all taken as one token. All we do is keep adding characters to the 
       * token. If we encounter further pairs, we use parenDepth to keep track, so that we pop out of the outside
       * pair (the one that is not the outer "inString" pair), and go back to regular parsing.
       */
      parenDepth = 0;
      for (int iSub = 0; iSub < listSource.Length; iSub++)               // walk through string one char at a time
      {
        switch (str[iSub])
        {
          case TAGFunctions.BEGINSTRINGCHAR:                                             // beginning of the string
            if (!inString)
              inString = true;
            else
            {
              parenDepth++;
              token.Length++;
              token[toksub++] = str[iSub];
            }
            break;
          case TAGFunctions.COLSEPARATORCHAR:                                             // next column
            if (inString)                                                 // don't do anything if we are before cBEGINSTRING or after cENDSTRING
              if (parenDepth <= 0)
              {
                setTableValue(table, row, col, token.ToString(), pHasHeader, pDataTypes);
                token.Length = 0;
                toksub = 0;
                col++;
              }
              else
              {
                token.Length++;
                token[toksub++] = str[iSub];
              }
            break;
          case TAGFunctions.ROWSEPARATORCHAR:                                             // next row
            if (inString)                                                // don't do anything if we are before cBEGINSTRING or after cENDSTRING
              if (parenDepth <= 0)
              {
                if (pHasHeader && table.Count == 0 && irows > 0)
                  table.AddRows(irows);
                setTableValue(table, row, col, token.ToString(), pHasHeader, pDataTypes);
                token.Length = 0;
                toksub = 0;
                col = 0;
                row++;
              }
              else
              {
                token.Length++;
                token[toksub++] = str[iSub];
              }
            break;
          case TAGFunctions.ENDSTRINGCHAR:                                               // end of string
            if (inString)                                                // don't do anything if we are before cBEGINSTRING or after cENDSTRING
              if (parenDepth <= 0)
              {
                setTableValue(table, row, col, token.ToString(), pHasHeader, pDataTypes);
                inString = false;
              }
              else
              {
                parenDepth--;
                token.Length++;
                token[toksub++] = str[iSub];
              }
            break;
          default:                                              // any other character gets appended to the current cell of the table
            if (inString)
            {
              token.Length++;
              token[toksub++] = str[iSub];
            }
            break;
        }
      }
      /*
       * If the incoming string is really a value="SingleString" format, then we have not picked up any cell values.
       * We therefore convert to a single row, single column table
       */
      if (icols == 1 && irows == 1 && table[0, 0] == null)
      {
        table[0, 0] = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, listSource, TAGFunctions.DATATYPESTRING);
        listSource = "(" + listSource + ")";
      }
      return table;
    }
    static private void setTableValue(PickList table, int rawRow, int col, string myValue, bool pHasHeader, string[] pDataTypes)
    {
      int row;
      /*
       * special case: if the first column has a prefix, don't convert to the correct data type. Leave it a string.
       */
      if (table == null)
        return;
      if (rawRow == 0 && pHasHeader)                                 // if the first row contains headers
      {
        table.AddColumn(myValue);
        return;
      }
      if (col >= table.Columns().GetLength(0) || col < 0)
        return;


      //table[row, col] = myValue;                      // then this value is a header and is always a string
      else
      {
        // adjust for header row (PickList does not include this as a row
        if (pHasHeader)
          row = rawRow - 1;
        else
          row = rawRow;
        if (row < 0 || row > table.Count)
          return;        
        if (col == 0 && myValue != null && myValue.Length > 1)
        {
          string op = myValue.Substring(0, 1);
          switch (op)
          {
            case OPERATORADD:
            case OPERATORSTOPMERGE:
            case OPERATORDELETE:
              table[row, col] = myValue;
              break;
            default:
              if (row >= table.Count)
                table.AddRow();
              table[row, col] = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, myValue, pDataTypes[col]);   // otherwise convert to the proper datatype before adding
              break;
          }
        }
        else
        {
          if (row >= table.Count)
            table.AddRow();
          table[row, col] = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, myValue, pDataTypes[col]);   // otherwise convert to the proper datatype before adding
        }
      }
    }
    private bool isXML(string testXML)
    {
      if (testXML == null || testXML == string.Empty)
        return false;
      string testStartString = EMPTYGUITABLE.Substring(0, 6);
      string testEndString = EMPTYGUITABLE.Substring(6);
      return (testXML.StartsWith(testStartString) && testXML.EndsWith(testEndString));
    }
    private void load(string pAttributeName, string pValueType, string pValue, bool useDictionaryForHeader, bool throwError, string source)
    {
      try
      {
        // TODO: need to populate the pick lists for all the columns
        attributeName = pAttributeName;
        valueType = VALUETYPETABLEHEADER;
        tableSource = pValue;
        string[] stringDataTypeList = new string[1] { TAGFunctions.DATATYPESTRING };
        if (useDictionaryForHeader)
        {
          header = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, dictionary.AttributeProperty(attributeName, VALUETYPETABLEHEADER));
          // look for header definition in form =attributename
          if (header != null && header.StartsWith("=") && header.Length > 1) // new feature: several attributes can share the same TableHeader definition in the dictionary
          {
            string baseAttribute = header.Substring(1);
            header = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, dictionary.AttributeProperty(baseAttribute, VALUETYPETABLEHEADER));
          }
          hasDictionaryHeader = true;
        }
        if (header == string.Empty || !(pValueType.ToLower() == VALUETYPETABLEHEADER || pValueType.ToLower() == TAGFunctions.TABLEMOD))
        {
          if (throwError)
          {
            TAGExceptionMessage tm = new TAGExceptionMessage("TableHeader", "load", "Table must have dictionary header and correct valuetype");
            tm.AddParm(pAttributeName);
            tm.AddParm(pValueType);
            tm.AddParm(pValue);
            throw new Exception(tm.ToString());
          }
          else
          {
            LogFactory.GetInstance().GetLog("TAGBOSS.Sys.AttributeEngine.AttributeData").Error(string.Format("({0}:{1}:{2})::TableHeader.load:Table must have dictionary header and correct valuetype", pAttributeName, pValueType, pValue));
            return;
          }
        }
        parseHeader();
        if (tableSource == "(=)")
        {
          canMergeNewSource = false;
          tableSource = "()";
        }
        PickList table = LoadTable(tableSource, stringDataTypeList, false);   // convert the table into PickList format
        if (table == null || (table.GetLength(1) != columns.Count && table.GetLength(1) > 1))  // one column tablemods are supported to use the "-" operator, zero columns is also allowed
        {
          if (throwError)
          {
            TAGExceptionMessage tm = new TAGExceptionMessage("TableHeader", "load", "Table must match number of columns with dictionary header");
            tm.AddParm(pAttributeName);
            tm.AddParm(pValueType);
            tm.AddParm(pValue);
            throw new Exception(tm.ToString());
          }
          else
          {
            LogFactory.GetInstance().GetLog("TAGBOSS.Sys.AttributeEngine.AttributeData").Error(string.Format("({0}:{1}:{2})::TableHeader.load:Table must match number of columns with dictionary header", pAttributeName, pValueType, pValue));
            return;
          }
        }
        if (table != null)                                          // if we got any rows
        {
          string strKeyNames = string.Empty;
          if (hasHeader)
            strKeyNames = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, dictionary.AttributeProperty(attributeName, ATTRIBUTEKEYLIST));

          if (strKeyNames == string.Empty)
          {
            keyNames = new string[1];
            if (columns.Count > 0)
              keyNames[0] = columns[0].ID; // default key is the first column, if none are specified
            else
              keyNames[0] = KEYNONE;
          }
          else
          {
            keyNames = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, strKeyNames);
            if (keyNames == null || keyNames.GetLength(0) == 0)
            {
              keyNames = new string[1];
              keyNames[0] = columns[0].ID; // default key is the first column, if none are specified
            }
          }
          if (keyNames == null)
          {
            TAGExceptionMessage tm = new TAGExceptionMessage("TableHeader", "load", "KeyNames is null");
            tm.AddParm(pAttributeName);
            tm.AddParm(pValueType);
            tm.AddParm(pValue);
            tm.AddParm(useDictionaryForHeader);
            tm.AddParm(throwError);
            throw new Exception(tm.ToString());
          }
          for (int iRow = 0; iRow < table.GetLength(0); iRow++)     // take each row in table and construct a row class
          {
            bool skipRow = false;
            bool firstRow = true;
            object[] values = table.Row(iRow);        // get a copy of the row as an array of objects
            if (values != null)
            {
              if (columns.Count == 0) // if there was no header in the dictionary, or the constructor said to ignore it
              {
                string[] colList = table.Columns();
                // then we fill our headers from the first row for backward compatibility
                for (int iCol = 0; iCol < table.GetLength(1); iCol++)
                {
                  string columnName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, colList[iCol]);
                  columns.Add(columnName, new TableHeaderColumn(columnName));
                }
              }

            if (columns.Count == values.GetLength(0))
            {
              // check to see if ths first row ALSo contains table headers
              string header1 = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, values[0])).ToLower();
              if (header1 == columns[0].ID)
                skipRow = true;
            }

            if (!skipRow)
            {
              TableHeaderRow row = new TableHeaderRow(columns, values);
              row.Source = source;
              if (row.ErrorMessage != string.Empty)
              {
                TAGExceptionMessage tm = new TAGExceptionMessage("TableHeader", "load", "New row failed: " + row.ErrorMessage);
                tm.AddParm(attributeName);
                tm.AddParm(pValue);
                if (source != null)
                  tm.AddParm(source);
                throw new Exception(tm.ToString());
              }
              if (firstRow)
              {
                if (row.Op == OPERATORSTOPMERGE)    // process the equal sign: this makes this instance the last one to accept merged records int he inheritance chain
                  canMergeNewSource = false;
                firstRow = false;
              }
              if (row.Op == OPERATORDELETE)         // process the delete row operator. If this row contains it, then we don't add to the list of rows, but we do add to the list of delete row instructions
              {
                TableHeaderUniqueKey key = new TableHeaderUniqueKey(columns, row.Values(), keyNames);
                if (!IsDeleted(key))
                  deletedKeyList.Add(key, row.Source);
              }
              else
                AddRow(row, false);
            }
          }
          }
        }
      }
      catch (Exception ex)
      {
        TAGExceptionMessage tm = new TAGExceptionMessage("TableHeader", "load", "Error in tableheader load");
        tm.AddParm(pAttributeName);
        tm.AddParm(pValueType);
        tm.AddParm(pValue);
        tm.AddParm(useDictionaryForHeader);
        tm.AddParm(throwError);
        tm.AddParm(source);
        throw new Exception(tm.ToString(), ex);
      }
    }
    private void loadXML(string xmlIn)
    {

      XmlDocument xmlTableStringDoc = new XmlDocument();
      try
      {
        xmlTableStringDoc.LoadXml(xmlIn);
      }
      catch
      {
        throwXMLLoadError(xmlIn, "Load of xml failed because xml is in bad format");
      }
      XmlNode xmlAttrTbl = xmlTableStringDoc.SelectSingleNode("/attributetable");
      if (xmlAttrTbl == null)
        return;
      XmlNodeList xmlAttrTblHdrCols = xmlTableStringDoc.SelectNodes("/attributetable/header/column");
      XmlNodeList xmlAttrTblRows = xmlTableStringDoc.SelectNodes("/attributetable/row");
      int nbrRows = 0;
      int nbrColumns = 0;

      if (xmlAttrTblRows != null)
      {
        if (xmlAttrTblHdrCols != null)
          nbrRows = xmlAttrTblRows.Count + 1;
        else
          nbrRows = xmlAttrTblRows.Count;

        nbrColumns = xmlAttrTblRows[0].ChildNodes.Count;


        //So now we populate the table!!
        int i, j;
        i = j = 0;

        if (xmlAttrTblHdrCols != null)
        {
          if (columns.Count == xmlAttrTblHdrCols.Count)
          {
            // check to see the columns match
            bool headersOK = true;
            for (j = 0; j < xmlAttrTblHdrCols.Count; j++)
            {
              string colName = xmlAttrTblHdrCols[j].Attributes["name"].Value.ToLower();
              if (colName != columns[j].ID)
              {
                headersOK = false;
                break;
              }
            }
            if (!headersOK)
              throwXMLLoadError(xmlIn, "XML Headers do not match TableHeader headers");
          }
          else
            parseXMLHeader(xmlAttrTblHdrCols);
        }
        keyNames = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.FromList, 
          (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, dictionary.AttributeProperty(attributeName, ATTRIBUTEKEYLIST)));

        if (keyNames == null || keyNames.GetLength(0) == 0)
          if (columns.Count == 0)
            keyNames[0] = KEYNONE;
          else
            keyNames[0] = columns[0].OriginalID;

        rows.Clear();

        for (i = 0; i < xmlAttrTblRows.Count; i++)    // need to add a row instead
        {
          if (xmlAttrTblRows[i].ChildNodes.Count > rows.Count)
            throwXMLLoadError(xmlIn, "XML has different number of columns than header");
          object[] values = new object[rows.Count];
          for (j = 0; j < xmlAttrTblRows[i].ChildNodes.Count; j++)
          {
            string myName = xmlAttrTblRows[i].ChildNodes[j].Attributes["name"].Value;
            if (!columns.Contains(myName))
              throwXMLLoadError(xmlIn, "xml contains an attribute that is not in this TableHeader");
            int colIndex = columns.IndexOf(myName);
            string myDataType = xmlAttrTblRows[i].ChildNodes[j].Attributes["datatype"].Value;
            object myValue = "";
            if (myDataType != "table")
              myValue = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, xmlAttrTblRows[i].ChildNodes[j].Value, columns[myName].DataType);
            else
              myValue = new TableHeader(attributeName, xmlAttrTblRows[i].ChildNodes[j].InnerXml, dictionary);
            values[colIndex] = myValue;
          }
          TableHeaderRow row = new TableHeaderRow(columns, values);
          AddNewRow(row);
        }
      }
    }
    private void throwXMLLoadError(string xmlIn, string errorMsg)
    {
      TAGExceptionMessage tm = new TAGExceptionMessage("TableHeader", "XML Constructor", errorMsg);
      tm.AddParm(attributeName);
      tm.AddParm(xmlIn);
      throw new Exception(tm.ToString());
    }
    private void throwColumnNotFound(string functionName, string columnName)
    {
      TAGExceptionMessage t = new TAGExceptionMessage("TableHeader", functionName, "Column name specified is not in the table");
      t.AddParm("Attribute=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, attributeName));
      t.AddParm("Column=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, columnName));
      throw new Exception(t.ToString());
    }
    private void checkLookupParms(string functionName, string keyList, string columnName)
    {
      bool goodParms = true;
      string errorMsg = string.Empty;
      // we don't allow any empty parameters. If one is, we throw an error
      if (keyList == null || keyList == string.Empty)
      {
        errorMsg = "Lookup key values are missing";
        goodParms = false;
      }
      else
      {
        if (columnName == null || columnName == string.Empty)
        {
          errorMsg = "Column Name is missing";
          goodParms = false;
        }
      }
      if (!goodParms)
      {
        TAGExceptionMessage t = new TAGExceptionMessage("TableHeader", functionName, errorMsg);
        t.AddParm("Attribute=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, attributeName));
        t.AddParm("Lookup value(s)=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, keyList));
        t.AddParm("Column=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, columnName));
        throw new Exception(t.ToString());
      }
    }
    private bool matchesFilter(TableHeaderRow row, string[] columnList, object[] valueList, bool includeDefault, bool startsWith)
    {
      int index = 0;
      bool isMatch = true;  // assum match unless we find a column that does not match
      foreach (string columnName in columnList)
      {
        string dataType = columns[columnName].DataType;
        object rowValue = row[columnName].Value;
        bool isString = rowValue.GetType() == typeof(string);
        string compareRowString = string.Empty;
        string compareValueString = string.Empty;
        bool valueMatches;
        if (isString)
        {
          compareRowString = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, rowValue)).ToLower();
          compareValueString = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, valueList[index]);
          if (startsWith)
            valueMatches = compareRowString.StartsWith(compareValueString, StringComparison.CurrentCultureIgnoreCase);
          else
            valueMatches = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CompareTo, valueList[index], dataType, rowValue, dataType) == 0;
        }
        else
          valueMatches = ((int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CompareTo, valueList[index], dataType, rowValue, dataType) == 0);
        if (!valueMatches && // values dont match and
            !(includeDefault && isString && compareRowString == "default")) // not default
        {
          isMatch = false;
          break;
        }
        index++;
      }
      return isMatch;
    }
    #endregion
  }
}
