using System;
using System.Collections;

namespace TAGBOSS.Common
{
  /// <summary>
  /// Class definition to support enumeration.
  /// </summary>
  [Serializable]
  public class TableHeaderEnumerator : IEnumerator
  {
    int nIndex;
    TableHeader collection;
    bool includeDeletedRecords = false;

    public TableHeaderEnumerator(TableHeader coll)
    {
      collection = coll;
      nIndex = -1;
      includeDeletedRecords = collection.IncludeDeletedRows;
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
    public void Dispose() { collection = null; }
    public void Reset() { collection = null; nIndex = -1; includeDeletedRecords = false; }

  }
}
