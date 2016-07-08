using System;
using System.Collections;

namespace TAGBOSS.Common.Model
{
  public class TEnumerator: IEnumerator
  {
    private object[] collection;
    private int index = -1;

    public TEnumerator(object[] collection) 
    {
      this.collection = collection;
    }

    #region IEnumerator Members
   
    public object Current
    {
      get
      {
        if (index > -1 && index < collection.GetLength(0))
          return collection[index];
        else
          return null;
      }
    }

    public bool MoveNext()
    {
      if (collection == null)
        return false;

      index++;
      if (index < collection.GetLength(0))
        return true;
      else
        return false;
   }

    public void Reset()
    {
      index = -1;
    }

    #endregion
  }
}
