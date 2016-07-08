using System;
using System.Collections;
using System.Collections.Generic;

namespace TAGBOSS.Common
{
  [Serializable]
  public class SortedCollectionBase : IEnumerable
  {
    public bool SortByID { get; set; }
    public const string SORTORDERFORMAT = "000000";
    public Hashtable HashTable { get; set; }  // item.ID, item
    public ArrayList List { get; set; }   // item in Item.ID order
    public int Count { get { if (HashTable == null) return 0; else return HashTable.Count; } }
    public SortedCollectionBase()
    {
      SortByID = false;
      init();
    }
    public SortedCollectionBase(bool sortByID)
    {
      SortByID = sortByID;
      init();
    }
    private void init()
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

    public ISortedItemBase this[string key]
    {
      get
      {
        if (key != null && key != string.Empty)
          return (ISortedItemBase)HashTable[key.ToLower()];
        else
          return null;
      }
      set
      {
        if (string.IsNullOrEmpty(key))
          return;
        string k = key.ToLower();
        if (HashTable.Contains(k))
        {
          for (int i = 0; i < List.Count; i++)
          {
            if (((SortedItemBase)List[i]).ID.Equals(k))
            {
              List[i] = value;
              break;
            }
          }
          HashTable[k] = value;
        }
      }
    }

    public ISortedItemBase this[int index]
    {
      get
      {
        if (index < List.Count && index >= 0)
          return (ISortedItemBase)List[index];
        else
          throw new KeyNotFoundException();
      }
      set
      {
        if (index < List.Count && index >= 0)
        {
          string key = ((SortedItemBase)List[index]).ID;
          if (HashTable.Contains(key))
          {
            List[index] = value;
            HashTable[key] = value;
            return;
          }
        }
        throw new KeyNotFoundException();
      }
    }
    public void Remove(string key)
    {
      if (HashTable.ContainsKey(key))
      {
        int pos = binaryFind(List, (ISortedItemBase)HashTable[key]);
        if (pos == -1 || pos > List.Count - 1) // this should not happen, since we already know the item is there)
          return;
        else
        {
          List.RemoveAt(pos);
          HashTable.Remove(key);
        }
      }
    }
    public void Remove(int index)
    {
      if (index >= 0 && index < List.Count)
      {
        string key = ((ISortedItemBase)List[index]).ID;
        if (HashTable.ContainsKey(key))
        {
          HashTable.Remove(key);
          List.RemoveAt(index);
        }
      }
    }
    public void Add(ISortedItemBase item)
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
        HashTable.Add(item.ID, item);
      }
    }
    public void Clear()
    {
      List.Clear();
      HashTable.Clear();
    }
    public string NextSortOrder(string sortOrder)
    {
        int iSort = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, sortOrder, 0);
        while (List.Contains((iSort).ToString(SORTORDERFORMAT))) { iSort++; }
        return iSort.ToString(SORTORDERFORMAT);
    }
    public int NextSortOrder(int sortOrder)
    {
      while (List.Contains((sortOrder).ToString(SORTORDERFORMAT))) { sortOrder++; }
      return sortOrder;
    }
    private int binaryFind(ArrayList list, ISortedItemBase item)
    {
      int min, max, mid, iCompareThis;
      min = 0;
      max = list.Count - 1;

      string xId;
      if (SortByID)
        xId = item.ID;
      else
        xId = item.SortOrder.ToString(SORTORDERFORMAT);

      while (min <= max)
      {
        mid = min + ((max - min) / 2);
        string midID;
        if (SortByID)
          midID = ((ISortedItemBase)List[mid]).ID;
        else
          midID = ((ISortedItemBase)List[mid]).SortOrder.ToString(SORTORDERFORMAT); iCompareThis = midID.CompareTo(xId);
        if (iCompareThis == 0)
          return mid;
        else if (iCompareThis < 0)  // if not, then is it less than the test key? 
          min = mid + 1;// yes, look in the segment before this
        else
          max = mid - 1;  // no, so look in the segment above me
      }
      return -1;  // we did not find it, so return -1
    }
    private int binarySort(ArrayList list, ISortedItemBase item)
    {
      int min, max, mid, midPrior, iCompareThis, iComparePrior;

      min = 0;
      max = list.Count - 1;

      string xId;
      if (SortByID)
        xId = item.ID;
      else
        xId = item.SortOrder.ToString(SORTORDERFORMAT);

      while (min <= max)
      {
        mid = min + ((max - min) / 2);
        string midID;
        if (SortByID)
          midID = ((ISortedItemBase)List[mid]).ID;
        else
          midID = ((ISortedItemBase)List[mid]).SortOrder.ToString(SORTORDERFORMAT);
        iCompareThis = midID.CompareTo(xId);
        if (iCompareThis == 0)
          return mid;

        midPrior = Math.Max(mid - 1, 0);
        string midPriorID;
        if (SortByID)
          midPriorID = ((ISortedItemBase)List[midPrior]).ID;
        else 
          midPriorID = ((ISortedItemBase)List[midPrior]).SortOrder.ToString(SORTORDERFORMAT);
        iComparePrior = midPriorID.CompareTo(xId);

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
    IEnumerator IEnumerable.GetEnumerator() { return (IEnumerator)new SortedCollectionEnumerator(this); }

  }

}
