using System;
using System.Collections;
using System.Collections.Generic;

namespace TAGBOSS.Common.Model
{
  [Serializable]
  public class TAEL
  {
    public Hashtable HashTable { get; set; }
    public ArrayList List { get; set; }

    public TAEL()
    {
      HashTable = new Hashtable();
      List = new ArrayList();
    }

    public bool Contains(string key)
    {
      if (key != null && key != string.Empty)
        return HashTable.ContainsKey(key.ToLower());
      else
        return false;
    }

    public TIndexItem this[string key]
    {
      get
      {
        if (key != null && key != string.Empty)
          return (TIndexItem)HashTable[key.ToLower()];
        else
          return null;
      }
    }

    public TIndexItem this[int index]
    {
      get
      {
        return (TIndexItem)List[index];
      }
    }

    public void Add(TIndexItem item)
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

        HashTable.Add(item.ItemHash, item);
      }
    }

    private int binarySort(ArrayList list, TIndexItem item)
    {
      int min, max, mid, midPrior, iCompareThis, iComparePrior;

      min = 0;
      max = list.Count - 1;

      string xId = item.ItemHash;

      while (min <= max)
      {
        mid = min + ((max - min) / 2);

        iCompareThis = ((TIndexItem)list[mid]).ItemHash.CompareTo(xId);
        if (iCompareThis == 0)
          return mid;

        midPrior = Math.Max(mid - 1, 0);
        iComparePrior = ((TIndexItem)list[midPrior]).ItemHash.CompareTo(xId);

        if (iCompareThis > 0 && iComparePrior < 0) // is our key equal to the test key or between it and the key before it?
          return mid;   // then return this index as the current position
        else if (iCompareThis < 0)  // if not, then is it less than the test key? 
          min = mid + 1;// yes, look in the segment before this
        else
          max = mid - 1;  // no, so look in the segment above me

      }

      //if max is less than zero that means the new item is LESS THAN any of the existing, and most go first
      if (max < 0)
        return 0;
      else
        return -1;
    }
  }

}
