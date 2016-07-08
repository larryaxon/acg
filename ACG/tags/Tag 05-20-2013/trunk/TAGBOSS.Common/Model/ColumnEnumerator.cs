using System;
using System.Collections;

namespace TAGBOSS.Common.Model
{
  [Serializable]
  public class ColumnEnumerator : IEnumerator
  {
    int nIndex;
    TableHeaderColumnsCollection collection;
    public ColumnEnumerator(TableHeaderColumnsCollection coll)
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
}
