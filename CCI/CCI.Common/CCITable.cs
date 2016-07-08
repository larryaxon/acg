using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCI.Common
{
  public class CCITable : CCIFormItem, IComparable, IEquatable<CCITable>, IComparable<CCITable>
  {
    private Hashtable keyColumns = new Hashtable();
    private Hashtable columnsList = new Hashtable();
    //private Hashtable columnsTypeList = new Hashtable();
    private Dictionary<int, string> columns = new Dictionary<int, string>();
    // datatypes may only hold "exceptions". they are <column, datatype> and if not present for a column, we assume string
    private Dictionary<string, string> dataTypes = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    // pick lists hold optional picklists for a given column and row. row -1 is for all rows
    private Dictionary<int, Dictionary<string, PickListEntries>> pickLists = new Dictionary<int, Dictionary<string, PickListEntries>>();
    private Dictionary<int, Hashtable> rows = new Dictionary<int, Hashtable>();
    public object this[int row, int col]
    {
      get
      {
        if (row < 0 || row >= rows.Count || col < 0 || col >= columns.Count)
          throw new IndexOutOfRangeException();
        string colID = columns[col];
        Hashtable r = rows[row];
        return r[colID];
      }
      set
      {
        if (row < 0 || row >= rows.Count || col < 0 || col >= columns.Count)
          throw new IndexOutOfRangeException();
        string colID = columns[col];
        Hashtable r = rows[row];
        r[colID] = value;
      }
    }
    public object this[int row, string colName]
    {
      get
      {
        if (string.IsNullOrEmpty(colName))
          return null;

        int col = -1;
        foreach(KeyValuePair<int, string> column in columns)
        {
          if (column.Value.Trim().ToLower() == colName.Trim().ToLower()) 
          {
            col = column.Key;
            break;
          }
        }
        if (row < 0 || row >= rows.Count || col < 0 || col >= columns.Count)
          throw new IndexOutOfRangeException();
        
        string colID = columns[col];
        Hashtable r = rows[row];
        return r[colID];
      }
      set
      {
        int col = -1;
        foreach (KeyValuePair<int, string> column in columns)
        {
          if (column.Value.Trim().ToLower() == colName.Trim().ToLower())
          {
            col = column.Key;
            break;
          }
        }
        if (row < 0 || row >= rows.Count || col < 0 || col >= columns.Count)
          throw new IndexOutOfRangeException();
        string colID = columns[col];
        Hashtable r = rows[row];
        r[colID] = value;
      }
    }

    public Hashtable KeyColumns { get { return keyColumns; } }
    public string Column(int index)
    {
      if (index < 0 || index >= columns.Count)
        throw new IndexOutOfRangeException();
      return columns[index];
    }
    public int NumberKeyColumns { get { return keyColumns.Count; } }
    public int NumberColumns { get { return columns.Count; } }
    public int NumberRows { get { return rows.Count; } }
    public CCITable()
    {
      //FormItemType = FormItemTypes.Table;
    }
    public void AddRow(object[] values)
    {
      //if (values.GetLength(0) != columns.Count)
      //  throw new Exception(string.Format("Number of values ({0}) does not equal number of columns ({1})",values.GetLength(0),columns.Count));
      Hashtable row = new Hashtable();
      for (int i = 0; i < columns.Count; i++)
          if (i < values.GetLength(0))
                row.Add(columns[i],values[i]);
      rows.Add(rows.Count, row);
    }
    public void RemoveRow(int index)
    {
      if (index < 0 || index >= rows.Count)
        throw new IndexOutOfRangeException();
      rows.Remove(index);
    }
    public void AddColumn(string columnName)
    {
      AddColumn(columnName, CommonData.SQLOBJECT);
    }
    public void AddColumn(string columnName, string columnType)
    {
      columns.Add(columns.Count, columnName);
      columnsList.Add(columnName.ToLower().Trim(), columnName.ToLower().Trim());
      //columnsList.Add(columnName.ToLower().Trim(), columns.Count);
      //columnsTypeList.Add(columnType.ToLower().Trim(), columns.Count);
    }
    public string getDataType(string column)
    {
      if (dataTypes.ContainsKey(column))
        return dataTypes[column];
      else
        return CommonData.DATATYPESTRING;
    }
    public void setDataType(string column, string datatype)
    {
      if (dataTypes.ContainsKey(column))
        dataTypes[column] = datatype;
      else
        dataTypes.Add(column, datatype);
    }
    public PickListEntries getPickList(int row, string column)
    {
      if (row < -1 || row >= rows.Count) // it is not a valid row
        return null;
      if (pickLists.ContainsKey(row))
      {
        if (pickLists[row].ContainsKey(column))
          return pickLists[row][column];
        else
          if (pickLists.ContainsKey(-1)) // generic "header" row
            if (pickLists[-1].ContainsKey(column))
              return pickLists[-1][column];
            else
              return null;
          else
            return null;
      }
      else
      {
        if (pickLists.ContainsKey(-1)) // generic "header" row
          if (pickLists[-1].ContainsKey(column))
            return pickLists[-1][column];
          else
            return null;
        else
          return null;
      }
    }
    public void setPickList(int row, string column, PickListEntries table)
    {
      if (row < -1 || row >= rows.Count || table == null || string.IsNullOrEmpty(column)) // it is not a valid row
        return;
      if (pickLists.ContainsKey(row))
      {
        if (pickLists[row].ContainsKey(column))
          pickLists[row][column] = table;
        else
          pickLists[row].Add(column, table);
      }
      else
      {
        Dictionary<string, PickListEntries> newRow = new Dictionary<string, PickListEntries>(StringComparer.CurrentCultureIgnoreCase);
        newRow.Add(column, table);
        pickLists.Add(row, newRow);
      }
    }
    /// <summary>
    /// Uses a string array to set the columns. Also clears all prior data
    /// </summary>
    /// <param name="columnNames"></param>
    public void SetColumns(string[] columnNames)
    {
      string[] columnTypes = new string[columnNames.GetLength(0)];
      for (int i = 0; i < columnTypes.GetLength(0); i++)
        columnTypes[i] = CommonData.SQLOBJECT;

      SetColumns(columnNames, columnTypes);
    }
    public void SetColumns(string[] columnNames, string[] columnTypes)
    {
      columns.Clear();
      columnsList.Clear();
      //columnsTypeList.Clear();
      for (int i = 0; i < columnNames.GetLength(0); i++)
      {
        columns.Add(i, columnNames[i]);
        columnsList.Add(columnNames[i].ToLower().Trim(), columnTypes[i].ToLower().Trim());
        //columnsTypeList.Add(columnTypes[i].ToLower().Trim(), columnTypes[i]);
      }
      rows.Clear();
    }
    public void SetKeyColumns(string[] columnNames)
    {
      keyColumns.Clear();
      for (int i = 0; i < columnNames.GetLength(0); i++)
        keyColumns.Add(columnNames[i].ToLower().Trim(), columnNames[i]);
      //rows.Clear();
    }
    public bool ContainsColumn(string columnName)
    {
      if (!(string.IsNullOrEmpty(columnName) || columnsList.Count == 0) && columnsList.ContainsKey(columnName.ToLower().Trim()))
        return true;
      return false;
    }
    public string getFieldType(string fieldName) 
    {
      if(columnsList.ContainsKey(fieldName.ToLower().Trim()))
        return (string)columnsList[fieldName.ToLower().Trim()];
      return CommonData.SQLOBJECT;
    }
    public int CompareTo(object o)
    {
      if (o == null)
        return 1;
      if (o.GetType() == typeof(CCITable))
        return CompareTo((CCITable)o);
      throw new Exception("Must compare to an ATKTable object");
    }
    public int CompareTo(CCITable table)
    {
      if (table == null)
        return 1;
      if (table.ID == null)
        return 1;
      if (ID == null)
        return -1;
      int cmp = table.ID.ToLower().CompareTo(ID.ToLower());
      if (cmp != 0)
        return cmp;
      if (NumberColumns < table.NumberColumns)
        return -1;
      if (NumberColumns > table.NumberColumns)
        return 1;
      if (NumberRows < table.NumberRows)
        return -1;
      if (NumberRows > table.NumberRows)
        return 1;
      for (int iCol = 0; iCol < NumberColumns; iCol++)
      {
        cmp = CommonFunctions.CString(columns[iCol]).ToLower().CompareTo(CommonFunctions.CString(table.Column(iCol)));
        if (cmp != 0)
          return cmp;
      }
      for (int iRow = 0; iRow < NumberRows; iRow++)
      {
        for (int iCol = 0; iCol < NumberColumns; iCol++)
        {
          cmp = CommonFunctions.CString(this[iRow, iCol]).ToLower().CompareTo(CommonFunctions.CString(table[iRow, iCol]).ToLower());
          if (cmp != 0)
            return cmp;
        }
      }
      return 0;
    }
    public bool Equals(CCITable table)
    {
      return (CompareTo(table) == 0);
    }
    /// <summary>
    /// Find the first row that matches the value specified.
    /// Null or empty values will always return NotFound
    /// </summary>
    /// <param name="columnName"></param>
    /// <param name="value"></param>
    /// <returns>Row number, or -1 if not found</returns>
    public int FindFirstRow(string columnName, string value)
    {
      if (!ContainsColumn(columnName))
        return -1;
      if (string.IsNullOrEmpty(value))
        return -1;
      for (int iRow = 0; iRow < rows.Count; iRow++)
      {
        if (value.Equals(CommonFunctions.CString(this[iRow, columnName]), StringComparison.CurrentCultureIgnoreCase))
          return iRow;
      }
      return -1;
    }
  }
}
