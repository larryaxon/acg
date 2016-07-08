using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACG.App.Common
{
  [Serializable]
  public class ItemListEnumerator : IEnumerator
  {
    int nIndex;
    ItemListCollection collection;
    public ItemListEnumerator(ItemListCollection coll)
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
