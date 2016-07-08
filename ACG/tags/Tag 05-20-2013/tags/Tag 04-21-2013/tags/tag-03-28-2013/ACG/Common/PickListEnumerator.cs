using System;
using System.Collections;

namespace ACG.Common
{
  [Serializable]
  public class PickListEnumerator : IEnumerator
  {
    int nIndex;
    PickListEntries collection;
    public PickListEnumerator(PickListEntries coll)
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
