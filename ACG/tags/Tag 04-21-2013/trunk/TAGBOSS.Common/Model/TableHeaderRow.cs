using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using TAGBOSS.Common.Interface;

namespace TAGBOSS.Common.Model
{
  /// <summary>
  /// Defines a Row for the TableHeader
  /// </summary>
  [SerializableAttribute]
  public class TableHeaderRow : IEnumerable
  {
    #region module data
    protected TableHeaderCells cols = new TableHeaderCells();
    //protected AttributesCollection cols = new AttributesCollection();
    protected Dictionary<string, string> dataTypes = new Dictionary<string, string>();
    protected Dictionaries dictionary { get { return DictionaryFactory.getInstance().getDictionary(); } }
    private string op = string.Empty;
    private bool _dirty = false;
    #endregion
    #region public properties
    /// <summary>
    /// Returns the value for a column name
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Field this[string columnName]
    {
      get
      {
        if (cols.ContainsKey(columnName))
          return cols[columnName];
        else
          return null;
      }
      set
      {
        if (cols.ContainsKey(columnName))
          cols[columnName] = value;
        else
          cols.Add(columnName, value);
      }
    }
    public Field this[int index]
    {
      get { return cols[index]; }
      set { cols[index] = value; }
    }
    public string Source { get; set; }
    public string ErrorMessage { get; set; }
    public bool HasError { get { return (ErrorMessage != null && ErrorMessage.Length > 0); } }
    public string Op { get { return op; } set { op = value; } }
    public int Count { get { return cols.Count; } }
    public bool Deleted { get; set; }
    /// <summary>
    /// This returns true if any cell is dirty, or if the dirty flag for this row has specifically been set to true. 
    /// Otherwise it returns false.
    /// </summary>
    public bool Dirty 
    {
      get
      {
        if (_dirty)
          return true;
        else
          foreach (Field cell in cols)
            if (cell.Dirty)
              return true;
        return false;
      }
      set
      {
        _dirty = value;
        if (!_dirty)  // we are clearing the flag
          foreach (Field cell in cols)
            cell.Dirty = false;
      }
    }
    public TableHeaderCells Cells { get { return cols; } set { cols = value; } }
    #endregion
    #region constructors
    public TableHeaderRow(TableHeaderColumnsCollection columns, object[] values)
    {
      ErrorMessage = string.Empty;
      if (columns.Count == 0)
      {
        ErrorMessage = "Cannot Create Row... No columns are present";
        return;
      }
      if (values == null || values.GetLength(0) == 0)
      {
        ErrorMessage = "Cannot Create Row... No values are present";
        return;
      }
      bool hasDeleteOneColumn = false;
      if (values.GetLength(0) == 1 && ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, values[0])).StartsWith(TableHeader.OPERATORDELETE))
        hasDeleteOneColumn = true;
      if (columns.Count != values.GetLength(0) && !hasDeleteOneColumn)
      {
        ErrorMessage = "Cannot Create Row... Number of columns does not match number of values";
        return;
      }
      //dictionary = dict;
      bool firstCol = true;
      foreach (TableHeaderColumn col in columns) // use the columns collection as a template to create cells/columns for the row
      {
        // First, create a TAGAttribute, which is the actual value of the column/cell
        Field a = new Field();
        a.ID = col.OriginalID;
        a.DataType = col.DataType;
        // Now, look for an operator. This will be a +, -, or = prefix of the value in the first column
        // Default value for Op is + (OPERATORADD)
        // If there is an operator, then we strip it before we set the value
        int iCol = columns.IndexOf(col.ID);
        object value = null;
        if (values.GetLength(0) > iCol)
          value = values[iCol];
        string strValue = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, value);
        if (strValue.Length > 0)
        {
          if (firstCol)
          {
            Op = strValue.Substring(0, 1);
            switch (Op)
            {
              case TableHeader.OPERATORADD:
              case TableHeader.OPERATORDELETE:
              case TableHeader.OPERATORSTOPMERGE:
                value = strValue.Substring(1);
                break;
              default:
                Op = TableHeader.OPERATORADD;
                break;
            }
          }
        }
        if (firstCol)
          firstCol = false;
        else
          if (hasDeleteOneColumn) // only has first column and this is to specify a delete operator
            break;  // so don't try to process the additional columns        
        // Now set the value to an object of the correct datatype
        try
        {                 
          a.Value = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, value, a.DataType, true);
        }
        catch (Exception ex)
        {
          TAGExceptionMessage tm = new TAGExceptionMessage("TableHeaderRow", "Constructor", ex.Message);
          tm.AddParm(value);
          tm.AddParm(a.DataType);
          throw new Exception(tm.ToString());
        }
        // now add the cell to the row
        cols.Add(a);    // the cols collection for a row is really a collection of TAGAttribute for the columns of the row (which is the same as Cells)
      }
      if (HasError)
        throw new Exception(ErrorMessage);
    }
    protected TableHeaderRow()
    {
    }
    #endregion
    #region public methods
    public string Column(int index)
    {
      return cols[index].OriginalID;
    }
    public object[] Values()
    {
      object[] values = new object[Count];
      int iCol = 0;
      foreach (Field a in Cells)
        values[iCol++] = a.Value;
      return values;

    }
    public object Clone()
    {
      TableHeaderRow r = new TableHeaderRow();
      r.cols = (TableHeaderCells)cols.Clone();
      foreach (KeyValuePair<string, string> entry in dataTypes)
        r.dataTypes.Add(entry.Key, entry.Value);
      //r.dictionary = dictionary;
      r.op = op;
      r.Source = Source;
      return r;
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator)new RowEnumerator(this);
    }
     #endregion
   [SerializableAttribute]
    public class RowEnumerator : IEnumerator
    {
      int nIndex;
      TableHeaderRow collection;
      public RowEnumerator(TableHeaderRow coll)
      {
        collection = coll;
        nIndex = -1;
      }

      public bool MoveNext()
      {
        nIndex++;
        return (nIndex < collection.Count);
      }

      public void Dispose() { collection = null; }
      public void Reset() { collection = null; nIndex = -1; }
      public object Current
      {
        get
        {
          return (collection[nIndex]);
        }
      }
    }
  }
}
