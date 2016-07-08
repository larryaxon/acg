using System;
using System.Collections;

namespace TAGBOSS.Common.Model
{
  public class TEnumerable : IEnumerable
  {
    private object[] collection = null;

    #region IEnumerable Members

    public IEnumerator GetEnumerator()
    {
      return new TEnumerator(collection);
    }

    #endregion

    public void Add(object obj)
    {
      resizeCollection(1);
      this.collection[collection.GetLength(0) - 1] = obj;
    }

    private void resizeCollection(int size)
    {
      object[] temp = null;

      if (this.collection == null)
        this.collection = new object[size];
      else
      {
        temp = new object[collection.GetLength(0) + size];
        Array.Copy(this.collection, temp, collection.GetLength(0));
        this.collection = temp;
      }
    }
  }
}
