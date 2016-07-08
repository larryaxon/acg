using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using TAGBOSS.Common.Interface;

namespace TAGBOSS.Common.Model
{
    /// <summary>
    /// Enumerable class that collects the Columns
    /// </summary>
    [Serializable]
    public class TableHeaderColumnsCollection : IEnumerable
    {
      protected Dictionary<string, TableHeaderColumn> cols = new Dictionary<string, TableHeaderColumn>();
      protected Dictionary<int, string> sortedColumnNames = new Dictionary<int, string>();
      public TableHeaderColumn this[string columnName] { get { return cols[columnName.ToLower()]; } }
      public TableHeaderColumn this[int index] 
      { 
        get 
        {
          if (index < 0 || index >= sortedColumnNames.Count)  // if index is out of range
            return null;                                                                // return a null
          return cols[sortedColumnNames[index]];                                        // otherwise return the correct entry
        } 
      }
      public int Count { get { return cols.Count; } }
      public int IndexOf(string columnName)
      {
        string compareColumnName = columnName.ToLower();
        if (sortedColumnNames.ContainsValue(compareColumnName))
        {
          foreach (KeyValuePair<int, string> colIndex in sortedColumnNames)
            if (colIndex.Value.ToLower() == compareColumnName)
              return colIndex.Key;
          return -1;
        }
        else
          return -1;
      }
      public void Add(string columnName, TableHeaderColumn column)
      {
        cols.Add(columnName.ToLower(), column);
        sortedColumnNames.Add(sortedColumnNames.Count, column.ID);
      }
      public bool Contains(string columnName)
      {
        if (columnName == null)
          return false;
        return cols.ContainsKey(columnName.ToLower());
      }
      public void Clear()
      {
        cols.Clear();
      }
      public string[] DataTypes()
      {
        if (Count == 0)
          return new string[0];
        else
        {
          string[] dataTypes = new string[Count];
          int iCol = 0;
          foreach (KeyValuePair<string, TableHeaderColumn> entry in cols)
            dataTypes[iCol++] = entry.Value.DataType;
          return dataTypes;
        }
      }
      public object Clone()
      {
        TableHeaderColumnsCollection c = new TableHeaderColumnsCollection();
        foreach (KeyValuePair<string, TableHeaderColumn> col in cols)
          c.cols.Add(col.Key, (TableHeaderColumn)col.Value.Clone());
        foreach (KeyValuePair<int, string> s in sortedColumnNames)
          c.sortedColumnNames.Add(s.Key, s.Value);
        return c;
      }

      IEnumerator IEnumerable.GetEnumerator() { return (IEnumerator)new ColumnEnumerator(this); }
    }
}
