using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

using TAGBOSS.Common;
using TAGBOSS.Common.Interface;

namespace TAGBOSS.Common.Model
{
  [Serializable]
  public class TableHeaderCells : ICloneable, IEnumerable
  {
    private Dictionary<string, Field> cells = new Dictionary<string, Field>(StringComparer.CurrentCultureIgnoreCase);
    private List<string> indexList = new List<string>();
    public Field this[int index]
    {
      get 
      { 
        if (index >= 0 && index < indexList.Count)
          return cells[indexList[index]]; 
        else
          return new Field();
      }
      set
      {
        if (index >= 0 && index < indexList.Count)
          cells[indexList[index]] = value;
      }
    }
    public Field this[string key]
    {
      get 
      { 
        if (cells.ContainsKey(key))
          return cells[key];
        else
          return default(Field);
      }
      set
      {
        if (cells.ContainsKey(key))
          cells[key] = value;
        else
          Add(key, value);
      }
    }
    public int Count { get { return cells.Count; } }
    public void Add(string key, Field f)
    {
      if (cells.ContainsKey(key))
        cells[key] = f;
      else
      {
        indexList.Add(key);
        cells.Add(key, f);
      }
    }
    public void Add(Field f)
    {
      Add(f.OriginalID, f);
    }
    public void Remove(string key)
    {
      indexList.Remove(key);
      cells.Remove(key);
    }
    public bool ContainsKey(string key)
    {
      return cells.ContainsKey(key);
    }
    public bool Contains(string key)
    {
      return cells.ContainsKey(key);
    }
    public object Clone()
    {
      TableHeaderCells c = new TableHeaderCells();
      foreach (string key in indexList)
        c.Add(key, (Field)cells[key].Clone());
      return c;
    }
    #region class definitions
    /// <summary>
    /// Class definition to support enumeration.
    /// </summary>
    [SerializableAttribute]
    public class DataClassEnumerator : IEnumerator
    {
      int nIndex;
      TableHeaderCells collection;
      public DataClassEnumerator(TableHeaderCells coll)
      {
        collection = coll;
        nIndex = -1;
      }
      public bool MoveNext()
      {
        nIndex++;
        return (nIndex < collection.Count);
      }
      public object Current
      {
        get
        {
          return (collection[nIndex]);
        }
      }
      public void Dispose() { ;}
      public void Reset() { ;}

    }
    #endregion


    #region IEnumerable Members

    /// <summary>
    /// Support enumeration (supports foreach).
    /// Enumerator overload to support LINQ use in Reports and other modules
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator)new DataClassEnumerator(this);
    }


    #endregion
  }
}
