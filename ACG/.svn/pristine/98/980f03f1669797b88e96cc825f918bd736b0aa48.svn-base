using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common
{
  [Serializable]
  public class SortedCollectionEnumerator : IEnumerator
  {
    int nIndex;
    SortedCollectionBase collection;
    public SortedCollectionEnumerator(SortedCollectionBase coll)
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
