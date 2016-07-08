using System;
using System.Collections;

namespace TAGBOSS.Common.Model
{
  public class TConditionalIndex
  {
    public Hashtable HashTable { get; set; }
    public ArrayList List { get; set; }

    public TConditionalIndex()
    {
      HashTable = new Hashtable();
      List = new ArrayList();
    }

    public bool Contains(string key)
    {
      return HashTable.ContainsKey(key);
    }

    public TConditionalItem this[string key]
    {
      get
      {
        return (TConditionalItem)HashTable[key];
      }
    }

    public TConditionalItem this[int index]
    {
      get
      {
        return (TConditionalItem)List[index];
      }
    }

    public void Add(TConditionalItem item)
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

      HashTable.Add(item.ConditionalHash, item);
    }

    private int binarySort(ArrayList list, TConditionalItem item)
    {
      int min, max, mid, midPrior, iCompareThis, iComparePrior; ;

      min = 0;
      max = list.Count - 1;

      string xId = item.ConditionalHash;

      while (min <= max)
      {
        mid = min + ((max - min) / 2);

        iCompareThis = ((TConditionalItem)list[mid]).ConditionalHash.CompareTo(xId);
        if (iCompareThis == 0)
          return mid;

        midPrior = Math.Max(mid - 1, 0);
        iComparePrior = ((TConditionalItem)list[midPrior]).ConditionalHash.CompareTo(xId);

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

    public IEnumerator GetEnumerator()
    {
      return this.List.GetEnumerator();
    }
  }
}
