using System;
using System.Collections;
using System.Collections.Generic;

namespace TAGBOSS.Common.Model
{
  public class TRelatedAttributeIndex
  {
    public Hashtable HashTable { get; set; }
    public ArrayList List { get; set; }

    public TRelatedAttributeIndex()
    {
      HashTable = new Hashtable();
      List = new ArrayList();
    }

    public IEnumerator GetEnumerator()
    {
      return this.List.GetEnumerator();
    }

    public bool Contains(string key)
    {
      return HashTable.ContainsKey(key);
    }

    public TRelatedAttributeItem this[string key]
    {
      get
      {
        return (TRelatedAttributeItem)HashTable[key];
      }
    }

    public TRelatedAttributeItem this[int index]
    {
      get
      {
        return (TRelatedAttributeItem)List[index];
      }
    }

    public void Add(TRelatedAttributeItem item)
    {
      lock (HashTable.SyncRoot)
      {
        if (this.List.Count > 0)
        {
          int pos = binarySort(this.List, item);

          if (pos == -1)
            this.List.Add(item);
          else
            this.List.Insert(pos, item);
        }
        else
          this.List.Add(item);

        HashTable.Add(item.RelatedItemHash, item);
      }
    }

    private int binarySort(ArrayList list, TRelatedAttributeItem item)
    {
      int min, max, mid;

      min = 0;
      max = list.Count - 1;

      string xId = item.RelatedItemHash;

      while (min <= max)
      {
        mid = min + ((max - min) / 2);
        if (((TRelatedAttributeItem)list[mid]).RelatedItemHash == xId)
          return mid;
        else if (((TRelatedAttributeItem)list[mid]).RelatedItemHash.CompareTo(xId) < 0)
          min = mid + 1;
        else
          max = mid - 1;
      }

      //if max is less than zero that means the new item is LESS THAN any of the existing, and most go first
      if (max < 0)
        return 0;
      else
        return -1;
    }

  }
}
