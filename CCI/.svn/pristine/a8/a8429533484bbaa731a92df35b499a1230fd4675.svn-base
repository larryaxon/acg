using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common
{
  [SerializableAttribute]
  public class PickList
  {
    #region module data
    SortedDictionary<int, SortedDictionary<int, object>> pickList = new SortedDictionary<int, SortedDictionary<int, object>>();
    Dictionary<string, string> columns = new Dictionary<string, string>();  // ID, Captions
    string[] defaultCaptions = new string[2] { "Code", "Description" };
    bool hasHeader = false;
    #endregion

    #region constructors
    /// <summary>
    /// Creates a pick list from a tableheader string. This uses the header from the first row,
    /// </summary>
    /// <param name="pickListTableString">TableHeader formatted string</param>    
    public PickList(string pickListTableString)
    {
      if (pickListTableString == null || pickListTableString == string.Empty)
        return;   // just make an empty PickList
      hasHeader = true;
      PickList newList = TableHeader.LoadTable(pickListTableString, null, hasHeader);
      columns = newList.columns;
      pickList = newList.pickList;
    }

    public PickList(string pickListTableString, string[] dataTypes, bool pHasHeader)
    {
      if (pickListTableString == null || pickListTableString == string.Empty)
        return;   // just make an empty PickList
      hasHeader = pHasHeader;
      PickList newList = TableHeader.LoadTable(pickListTableString, dataTypes, hasHeader);
      columns = newList.columns;
      pickList = newList.pickList;
    }
    public PickList(string pickListTableString, string[] dataTypes)
    {
      if (pickListTableString == null || pickListTableString == string.Empty)
        return;   // just make an empty PickList
      hasHeader = true;
      PickList newList = TableHeader.LoadTable(pickListTableString, dataTypes, hasHeader);
      columns = newList.columns;
      pickList = newList.pickList;
    }
    public PickList(string pickListTableString, string[] dataTypes, string[] columnNames)
    {
      if (pickListTableString == null)
        return;
      hasHeader = false;
      int nbrCols = TableHeader.NumberOfColumnsIn(pickListTableString);
      if (columnNames == null || dataTypes == null || 
        (nbrCols  > 0 && (nbrCols != columnNames.Length || nbrCols != dataTypes.Length)))
        return;
      PickList newList = TableHeader.LoadTable(pickListTableString, dataTypes, hasHeader);
      addColumns(columnNames);
      pickList = newList.pickList;
    }
    /// <summary>
    /// Creates default columns names (Code, Description) if flag is set to true
    /// </summary>
    /// <param name="createDefaultColumns">Should this create the default columns</param>
    public PickList(bool createDefaultColumns)
    {
      if (createDefaultColumns)
        addColumns(defaultCaptions);
      hasHeader = true;
    }
    /// <summary>
    /// Creates an empty picklist with no rows or columns
    /// </summary>
    public PickList()
    {
      hasHeader = false;
    }
    /// <summary>
    /// Creates a picklist with the given list of captions and no rows
    /// </summary>
    /// <param name="columnCaptions"></param>
    public PickList(string[] columnCaptions)
    {
      hasHeader = true;
      addColumns(columnCaptions);
    }
    /// <summary>
    /// Creates a picklist with the default columns and the requested number of empty rows
    /// </summary>
    /// <param name="nbrRows"></param>
    public PickList(int nbrRows)
    {
      hasHeader = true;
      addColumns(defaultCaptions);
      AddRows(nbrRows);
    }
    /// <summary>
    /// Just create a picklist as a 2D array with no meaningful columns
    /// </summary>
    /// <param name="nbrRows"></param>
    /// <param name="nbrColumns"></param>
    public PickList(int nbrRows, int nbrColumns)
    {
      string[] captions = new string[nbrColumns];
      for (int k = 0; k < nbrColumns; k++)
        captions[k] = "Col" + k.ToString();
      hasHeader = false;
      addColumns(captions);
      AddRows(nbrRows);
    }
    /// <summary>
    /// Creates a picklist with the given list of captions and the requested number of empty rows
    /// </summary>
    /// <param name="columnCaptions"></param>
    /// <param name="nbrRows"></param>
    public PickList(string[] columnCaptions, int nbrRows)
    {
      hasHeader = true;
      addColumns(columnCaptions);
      AddRows(nbrRows);
    }
    #endregion
    #region public properties
    public object this[object key, int col]
    {
      get
      {
        if (col < 0 || col >= columns.Count)
          return null;
        int row = IndexOf(key);
        if (row < 0)
          return null;
        return this[row, col];
      }
      set
      {
        if (col < 0 || col >= columns.Count)
          return;
        int row = IndexOf(key);
        if (row < 0)
          return;
        this[row, col] = value;
      }
      
    }
    public object this[int row, int col]
    {
      get
      {
        if (row < 0 || row > pickList.Count)
          return null;
        if (col < 0 || col > columns.Count)
          return null;
        return pickList[row][col];
      }
      set
      {
        if (row < 0 || row >= pickList.Count)
          return;
        if (col < 0 || col >= columns.Count)
          return;
        pickList[row][col] = value;
      }
    }
    public int Count
    {
      get { return pickList.Count; }
    }
    public int Rank { get { return columns.Count; } }
    public int Length { get { return pickList.Count * Rank; } }

    public int NumberColumns
    {
      get { return columns.Count; }
    }
    #endregion

    #region public methods
    /// <summary>
    /// Returns a string array of column captions
    /// </summary>
    /// <returns></returns>
    public string[] Captions()
    {
      string[] returnList = new string[columns.Count];
      int iCol = 0;
      foreach (KeyValuePair<string, string> entry in columns)
        returnList[iCol++] = entry.Value;
      return returnList;
    }
    /// <summary>
    /// Returns a string array of column ids
    /// </summary>
    /// <returns></returns>
    public string[] Columns()
    {
      string[] returnList = new string[columns.Count];
      int iCol = 0;
      foreach (KeyValuePair<string, string> entry in columns)
        returnList[iCol++] = entry.Key;
      return returnList;
    }
    public void AddRows(int nbrRows)
    {
      for (int i = 0; i < nbrRows; i++)
        AddRow();
    }
    /// <summary>
    /// Adds a blank row
    /// </summary>
    public void AddRow()
    {
      SortedDictionary<int, object> newRow = new SortedDictionary<int, object>();
      for (int i = 0; i < columns.Count; i++)
        newRow.Add(i, null);
      pickList.Add(pickList.Count, newRow);
    }
    public void RemoveRow(int iRow)
    {
      if (iRow >= pickList.Count)
        return;
      bool doCompress = (iRow < pickList.Count - 1);
      pickList.Remove(iRow);
      if (doCompress)
        compressRows();
    }
    public void RemoveRow(string code)
    {
      bool missingRow = false;
      string compareCode = code.ToLower();
      for (int i = pickList.Count - 1; i >= 0; i--)
      {
        object o = this[i, 0];
        if (((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString,o)).ToLower() == compareCode)
        {
          if (i != pickList.Count)
            missingRow = true;
          pickList.Remove(i);
          break;
        }
      }
      if (missingRow)
        compressRows();
    }
    public void AddColumn(string column)
    {
      AddColumn(column, column);
    }
    public object[] Row(int index)
    {
      if (index < 0 || index >= Count)
        return null;
      object[] returnRow = new object[columns.Count];
      for (int j = 0; j < columns.Count; j++)
        returnRow[j] = this[index, j];
      return returnRow;
    }
    public object[] Row(object key)
    {
      int i = IndexOf(key);
      if (i < 0)
        return null;
      return Row(i);
    }
    public int IndexOf(object key)
    {
      string compareKey = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, key)).ToLower();
      for (int i = 0; i < pickList.Count; i++)
        if (((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, this[i, 0])).ToLower() == compareKey)
          return i;
      return -1;
    }
    public int GetUpperBound(int dimension)
    {
      return GetLength(dimension) - 1;
    }
    public int GetLowerBound(int dimension)
    {
      return 0;
    }
    public void AddColumn(string column, string caption)
    {
      if (columns == null)
        return;
      if (caption == null)
        caption = column;
      try
      {
        columns.Add(column, caption); // add the column
      }
      catch
      {
        if (true) ;
      }
      int iCol = columns.Count - 1;
      foreach (KeyValuePair<int, SortedDictionary<int, object>> entry in pickList)  // now add the extra column to all the rows
        entry.Value.Add(iCol, null);
    }
    public int GetLength(int dimension)
    {
      if (dimension == 0)
        return pickList.Count;
      else
        if (dimension == 1)
          return columns.Count;
      throw new Exception("GetLength dimension is out of range");
    }
    public override string ToString()
    {
      if (columns.Count == 0)
        return string.Empty;
      string strResult = TAGFunctions.LEFTCHAR.ToString();
      bool firstColumn = true;
      bool firstRow = true;
      if (hasHeader)
      {
        // first, populate the header
        foreach (KeyValuePair<string, string> col in columns)
        {
          if (firstColumn)
            firstColumn = false;
          else
            strResult += TAGFunctions.COLSEPARATORCHAR;
          strResult += col.Value;
        }
        firstRow = false;
      }
      foreach (KeyValuePair<int, SortedDictionary<int, object>> row in pickList)
      {
        if (firstRow)
          firstRow = false;
        else
          strResult += TAGFunctions.ROWSEPARATORCHAR;
        firstColumn = true;
        foreach (KeyValuePair<int, object> a in row.Value)
        {
          if (firstColumn)
            firstColumn = false;
          else
            strResult += TAGFunctions.COLSEPARATORCHAR;
          strResult += (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, a.Value);
        }
      }
      strResult += TAGFunctions.RIGHTCHAR;
      return strResult;
    }
    public bool Contains(object value, string dataType)
    {
      for (int i = 0; i < Count; i++)
        if ((int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CompareTo, this[i,0], dataType, value, dataType) == 0)
          return true;
      return false;
    }
    public PickList Merge(PickList source, int[] keyColumns)
    {
      if (source.columns.Count != this.columns.Count)
        return this;
      int[] colList;
      if (keyColumns == null || keyColumns.GetLength(0) == 0)
        colList = new int[] { 0 };
      else
        colList = keyColumns;
      Dictionary<string, object[]> combinedList = new Dictionary<string, object[]>(StringComparer.CurrentCultureIgnoreCase);
      PickList[] lists = new PickList[] { this, source };
      foreach (PickList list in lists)
      {
        for (int i = 0; i < list.Count; i++)
        {
          string key = string.Empty;
          for (int j = 0; j < colList.GetLength(0); j++)
          {
            key += list[i, j];
            if (j != 0)
              key += "~";
          }
          if (!combinedList.ContainsKey(key))
            combinedList.Add(key, list.Row(i));
        }
      }
      PickList returnList = new PickList(this.Captions(), combinedList.Count);
      int iRow = 0;
      foreach (KeyValuePair<string, object[]> entry in combinedList)
      {
        for (int i = 0; i < columns.Count; i++)
          returnList[iRow, i] = entry.Value[i];
        iRow++;
      }
      return returnList;
    }
    public void Clear()
    {
      pickList.Clear();
    }
    public PickList Clone()
    {
      PickList newList = new PickList(Captions(), Count);
      for (int iRow = 0; iRow < pickList.Count; iRow++)
        for (int iCol = 0; iCol < columns.Count; iCol++)
          newList[iRow, iCol] = this[iRow, iCol];
      return newList;
    }
    #endregion
    #region private methods
    private void addColumns(string[] columnCaptions)
    {
      if (columnCaptions != null)
        foreach (string columnCaption in columnCaptions)
          AddColumn(columnCaption, columnCaption);
    }
    private void compressRows()
    {
      // we may have corrupted our unique index, because we might have a missing row.
      // wo we create a new copy with the correct index
      PickList newList = new PickList(Columns(), Count);
      int iRow = 0;
      foreach (KeyValuePair<int, SortedDictionary<int, object>> entry in pickList)  // for each row
      {
        int iCol = 0;
        foreach (KeyValuePair<int, object> cell in entry.Value)
          newList[iRow, iCol++] = cell.Value;
        iRow++;
      }
      pickList = newList.pickList;
    }
    #endregion
  }
}
