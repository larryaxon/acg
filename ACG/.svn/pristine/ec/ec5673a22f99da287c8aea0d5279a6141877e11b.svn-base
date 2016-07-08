using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using TAGBOSS.Common.Interface;

namespace TAGBOSS.Common.Model
{
  /// <summary>
  /// Defines the key for the rows collection. Note that this allows for duplicate keys, but still guarantees
  /// uniqueness via a sequence, so that this key can be used as a key in a Dictionary class
  /// </summary>
  [Serializable]
  public class TableHeaderUniqueKey : IComparable, IEquatable<TableHeaderUniqueKey>
  {
    [Serializable]
    public class KeyEntry
    {
      private string _keyName = null;
      private object _keyValue = null;
      public string KeyName { get { return _keyName; } set { if (value == null) _keyName = null; else _keyName = value.ToLower(); } }
      public object KeyValue 
      { 
        get { return _keyValue; }
        set 
        { 
          if (value != null && value.GetType() == typeof(string)) 
            _keyValue = ((string)value).ToLower().Trim(); 
          else 
            _keyValue = value;  
        } 
      } 
      public string DataType { get; set; }
      public KeyEntry(string keyName, object keyValue, string dataType)
      {
        KeyName = keyName;
        KeyValue = keyValue;
        DataType = dataType;
      }
      public KeyEntry(string keyName, object keyValue)
      {
        KeyName = keyName;
        KeyValue = keyValue;
        DataType = "string";
      }
      public KeyEntry()
      {
        KeyName = null;
        KeyValue = null;
        DataType = "string";
      }
      public override string ToString()
      {
        return string.Format("{0}:{1}", 
          (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, KeyName), 
          (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, KeyValue));
      }
    }
    private List<KeyEntry> keys = new List<KeyEntry>(); // the unique keye

    public List<KeyEntry> Keys
    {
      get { return keys; }
    }
    private int sequence = 0;
    /// <summary>
    /// Normally zero. Used to ensure uniqueness if all of the key parts ar the same, so a dup will create sequence = 1, 2, etc.
    /// </summary>
    public int Sequence
    {
      get { return sequence; }
      set { sequence = value; }
    }
    public int Count
    {
      get { return keys.Count; }
    }
    /// <summary>
    /// Constructs an empty one
    /// </summary>
    public TableHeaderUniqueKey()
    {
    }
    /// <summary>
    /// Construct a UniqueKey based on load values
    /// </summary>
    /// <param name="columns"></param>
    /// <param name="values"></param>
    /// <param name="pKeyNames"></param>
    public TableHeaderUniqueKey(TableHeaderColumnsCollection columns, object[] values, string[] pKeyNames)
    {
      if (pKeyNames == null || columns == null || values == null)
        return;
      if (columns.Count != values.GetLength(0) && values.GetLength(0) != 1)
        return;
      foreach (string keyName in pKeyNames)
        if (keyName != null && columns.Contains(keyName))
        {
          string dataType = columns[keyName].DataType;
          bool isFirstColumn = columns.IndexOf(keyName) == 0;
          object val = stripRowOperator(values[columns.IndexOf(keyName)], dataType, isFirstColumn);
          KeyEntry key = new KeyEntry(keyName, val, dataType);
          keys.Add(key);
        }
    }
    /// <summary>
    /// Construct a unique key based on a row
    /// </summary>
    /// <param name="columns"></param>
    /// <param name="row"></param>
    /// <param name="pKeyNames"></param>
    public TableHeaderUniqueKey(TableHeaderColumnsCollection columns, TableHeaderRow row, string[] pKeyNames)
    {
      if (pKeyNames == null || columns == null || row == null)
        return;
      if (columns.Count != row.Count)
        return;
      foreach (string keyName in pKeyNames)
        if (keyName != null && columns.Contains(keyName))
        {
          string dataType = columns[keyName].DataType;
          bool isFirstColumn = columns.IndexOf(keyName) == 0;
          object val = stripRowOperator(row[keyName].Value, dataType, isFirstColumn);
          KeyEntry key = new KeyEntry(keyName, val, dataType) ;
          keys.Add(key);
        }
    }
    public TableHeaderUniqueKey(TableHeaderRow row, string[] pKeyNames)
    {
      if (pKeyNames == null || row == null || row.Count == 0)
        return;
      foreach (string keyName in pKeyNames)
        if (keyName != null && row.Cells.Contains(keyName))
        {
          string dataType = row[keyName].DataType;
          bool isFirstColumn = row[keyName].ID.Equals(row[0].ID);
          object val = stripRowOperator(row[keyName].Value, dataType, isFirstColumn);
          KeyEntry key = new KeyEntry(keyName, val, dataType);
          keys.Add(key);
        }
    }
    public TableHeaderUniqueKey(TableHeaderColumnsCollection columns, string valueList, string[] pKeyNames)
    {
      int iKey = 0;
      if (pKeyNames == null || pKeyNames.GetLength(0) == 0)
      {
        if (columns == null || columns.Count == 0)
        {
          TAGExceptionMessage tm = new TAGExceptionMessage("TableHeaderUniqueKey", "Constructor(columns, valueList, pKeyNames)", "Key names cannot be null if no columns are specified");
          tm.AddParm(columns);
          tm.AddParm(valueList);
          throw new Exception(tm.ToString());
        }
        else
          pKeyNames = new string[] { columns[0].ID };
      }      
      string s = ((string)valueList).ToLower();
      string[] slist = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, s);
      if (slist == null || slist.GetLength(0) == 0 || slist.GetLength(0) > pKeyNames.GetLength(0))
      {
        return;     // just create a UniqueKey with no keys
      }
      string[] tempKeyList = pKeyNames;
      // if the incoming list of values is shorter than the list of keys, only create a key with the same number of keys as values
      if (slist.GetLength(0) < pKeyNames.GetLength(0))
      {
        tempKeyList = new string[slist.GetLength(0)];
        for (int i = 0; i < slist.GetLength(0); i++)
          tempKeyList[i] = pKeyNames[i];
      }
      foreach (string keyName in tempKeyList)
      {
        string dataType = columns[keyName].DataType;
        bool isFirstColumn = columns.IndexOf(keyName) == 0;
        object val = stripRowOperator(slist[iKey], dataType, isFirstColumn);
        KeyEntry key = new KeyEntry(keyName, val, dataType);
        keys.Add(key);
        iKey++;
      }
    }

    /// <summary>
    /// Used by IEquatable, and returns true if they keys are an exact match
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool Equals(TableHeaderUniqueKey key)
    {
      return HashCode() == key.HashCode();
    }
    public int HashCode()
    {
      return ToString().ToLower().GetHashCode();
    }
    /// <summary>
    /// This determines of the key values of two UniqueKeys are equal. Note this will not always yield the same result
    /// as CompareTo(key) == 0. The Equals(key) comparison does NOT take into account the Sequence property, which is used
    /// to guarantee uniqueness and enumerability, while the CompareTo(key) will only yield equal(zero) if both
    /// the corresponding key values, and the sequence number are the same
    /// </summary>
    /// <param name="key">A UniqueKey to compare with this one</param>
    /// <returns>True if all the key values are equal, false if they are not</returns>
    public bool keyEquals(TableHeaderUniqueKey key)
    {
      /*
       * Modified 3/9/1011 by LLA
       * If incoming key has fewer columns than this key, just compare the columns it contains
       */
      if (key.Count > Count)  // incoming cannot have more columns
        return false;
      else
      {
        int iKey = 0;
        foreach (KeyEntry incomingCol in key.keys)  // iterate through the columns of the incoming key
        {
          KeyEntry thisCol = keys[iKey];
          if ((int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CompareTo, incomingCol.KeyValue, incomingCol.DataType, thisCol.KeyValue, thisCol.DataType) != 0 || 
            !incomingCol.KeyName.Equals(thisCol.KeyName, StringComparison.CurrentCultureIgnoreCase))
            return false;
          iKey++;
        }
        return true;
      }
    }
    int IComparable.CompareTo(object compareKey)
    {
      if (compareKey.GetType() == typeof(TableHeaderUniqueKey))
      {
        return CompareTo((TableHeaderUniqueKey)compareKey);
      }
      throw new Exception("Cannot compare type " + compareKey.GetType().Name + " with type UniqueKey");
    }
    public int CompareTo(TableHeaderUniqueKey key)
    {
      if (Count < key.Count)
        return -1;
      else
        if (Count > key.Count)
          return +1;
        else
        {
          int iKey = 0;
          foreach (KeyEntry thisCol in keys)
          {
            string dataType = thisCol.DataType;
            KeyEntry thatCol = key.keys[iKey++];
            if (!thisCol.KeyName.Equals(thatCol.KeyName, StringComparison.CurrentCultureIgnoreCase))
              return thisCol.KeyName.CompareTo(thatCol.KeyName);
            int iCompare = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CompareTo, thisCol.KeyValue, dataType, thatCol.KeyValue, dataType);
            if (iCompare != 0)
              return iCompare;
          }
          if (sequence < key.sequence) return -1;
          if (sequence == key.sequence) return 0;
          return +1;
        }
    }
    public string ToKeyString()
    {
      StringBuilder sbkeys = new StringBuilder();
      bool firstTime = true;
      foreach (KeyEntry key in keys) 
      {
        if (firstTime)
          firstTime = false;
        else
          sbkeys.Append(",");
        sbkeys.Append((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, key.KeyValue));
      }
      return sbkeys.ToString();
    }
    public string ToRowString(TableHeaderColumnsCollection tableColumns)
    {
      StringBuilder sb = new StringBuilder();
      bool firstTimeFlag = true;
      foreach (TableHeaderColumn col in tableColumns)
      {
        if (firstTimeFlag)
        {
          firstTimeFlag = false;
        }
        else
          sb.Append(TAGFunctions.COLSEPARATORCHAR);
        foreach(KeyEntry key in keys)
          if (key.KeyName.Equals(col.OriginalID, StringComparison.CurrentCultureIgnoreCase))
          {
            sb.Append((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, key.KeyValue));
            break;
          }
      }
      return sb.ToString();
    }

    public object[] ToRowValues(TableHeaderColumnsCollection tableColumns)
    {
      object[] values = new object[tableColumns.Count];
      for (int i = 0; i < tableColumns.Count; i++)
      {
        TableHeaderColumn col = tableColumns[i];
        foreach (KeyEntry key in keys)
          if (key.KeyName.Equals(col.OriginalID, StringComparison.CurrentCultureIgnoreCase))
          {
            values[i] = key.KeyValue;
            break;
          }
      }
      return values;
    }
    public override string ToString()
    {
      StringBuilder returnString = new StringBuilder();
      bool firstTime = true;
      foreach (KeyEntry key in keys)
      {
        if (firstTime)
          firstTime = false;
        else
          returnString.Append(",");
        returnString.Append(key.ToString());
      }
      returnString.Append(":");
      returnString.Append(sequence.ToString());
      return returnString.ToString();
    }

    public object Clone()
    {
      TableHeaderUniqueKey k = new TableHeaderUniqueKey();
      k.sequence = sequence;
      foreach (KeyEntry thisCol in keys)
      {
        KeyEntry thatCol = new KeyEntry(thisCol.KeyName, thisCol.KeyValue, thisCol.DataType);
        k.keys.Add(thatCol);
      }
      return k;
    }
    private object stripRowOperator(object val, string dataType, bool isFirstColumn)
    {
      // Sometimes a field may have a one character prefix that is a TableHeader.Merge() operator. If so, we find it, strip it off,
      // and set the rowOperator property of the key. We then return the value which as been stripped of the operator and converted 
      // to the correct datatype
      if (!isFirstColumn)
        return val;
      if (val != null && val.GetType() == typeof(string)) // don't do anything if the value is null or non-string datatype
      {
        bool strippedOP = false;
        string strVal = (string)val;   
        if (strVal.Length > 0)                            // or if it is an empty string
        {
          string op = strVal.Substring(0, 1);
          switch (op)
          {
            case TableHeader.OPERATORADD:                 // if it is an operator
            case TableHeader.OPERATORDELETE:
            case TableHeader.OPERATORSTOPMERGE:
              strVal = strVal.Substring(1);               // then strip it off
              strippedOP = true;
              break;
          }
          if (dataType != TAGFunctions.DATATYPESTRING && strippedOP)    // whether it had an operator or not, if the target dataType is not a string and we stripped it
            val = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, strVal, dataType); // then convert the value to the correct datatype
        }
      }
      return val;                                         // and return it
    }
  }
}
