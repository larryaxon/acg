using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CCI.Common
{
  [Serializable]
  public class ItemListCollection : IEnumerable
  {
    private Hashtable hashTable = new Hashtable();
    private ArrayList list = new ArrayList();
    public ItemListEntry this[int index]
    {
      get
      {
        if (index < 0 || index >= list.Count)
          return null;
        else
        {
          string key = (string)list[index];
          return this[key];
        }
      }
    }
    public ItemListEntry this[string key]
    {
      get
      {
        if (hashTable.ContainsKey(key))
          return (ItemListEntry)hashTable[key];
        else
          return null;
      }
    }
    public ItemListEntry this[string item, string carrier]
    {
      get
      {
        string key = string.Format("{0}, {1}", item, carrier);
        return this[key];
      }
    }
    public int Count { get { return list.Count; } }
    public bool Contains(string key)
    {
      return list.Contains(key) && hashTable.ContainsKey(key);
    }
    public void Add(ItemListEntry searchResult)
    {
      if (searchResult == null)
        return;
      Add(searchResult.ID, searchResult);
    }
    public void Add(string key, ItemListEntry searchResult)
    {
      if (key == null || searchResult == null)
        return;
      AddKey(key);
      if (hashTable.ContainsKey(key))
        hashTable[key] = searchResult;
      else
        hashTable.Add(key, searchResult);
    }
    public void Remove(string key)
    {
      if (string.IsNullOrEmpty(key))
        return;
      string sKey = key.ToLower();
      if (hashTable.ContainsKey(sKey))
      {
        hashTable.Remove(sKey);
        if (list.Contains(sKey))
          list.Remove(sKey);
      }
    }
    private void AddKey(string key)
    {
      if (this.list.Count > 0)
      {
        int pos = binarySort(this.list, key);

        if (pos == -1)
          this.list.Add(key);
        else
          this.list.Insert(pos, key);
      }
      else
        this.list.Add(key);
    }
    private int binarySort(ArrayList list, string key)
    {
      /*
       * This divides the list into segments to return the position the new key should go in. This is done by cutting each segment
       * in half until we find the position we are looking for. 
       * 
       * Mid is the index of the "test" key we want to compare to. It is always precisely in the middle of the segment
       * Min and Max are the lower and upper boundaries of the segment we are looking inside of. They start as the top and bottom 
       * of the entire list
       * 
       * So, for example, in the sequence 1,3,5,7,9, if we are trying to insert a 4, we divide the list in half, then in half again, etc.
       * untill we find the place we are looking for.
       * 
       * That place is where either they key matches a test key, or is between that test key and the one below it in the sequence.
       * 
       */
      int min, max, mid, midPrior, iCompareThis, iComparePrior;

      min = 0;
      max = list.Count - 1;

      while (min <= max)
      {
        mid = min + ((max - min) / 2);
        iCompareThis = ((string)list[mid]).CompareTo(key);
        if (iCompareThis == 0)  // exact match of the key is found, so insert the new key before it
          return mid;
        midPrior = Math.Max(mid - 1, 0);  // this is the one before.
        iComparePrior = ((string)list[midPrior]).CompareTo(key);
        if (iComparePrior < 0 && iCompareThis > 0) // is our key equal to the test key or between it and the key before it?
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
        return -1;  // -1 means it goes after the END of the list
    }
    public SortedDictionary<string, ItemListEntry> SortedBy(string memberName)
    {
      SortedDictionary<string, ItemListEntry> list = new SortedDictionary<string, ItemListEntry>(StringComparer.CurrentCultureIgnoreCase);
      string key = string.Empty;
      foreach (ItemListEntry s in this)
      {
        switch (memberName.ToLower())
        {
          case "itemdescription":
            key = s.ItemDescription;
            break;
          case "carrierDescription":
            key = s.CarrierDescription;
            break;
          case "carrier":
            key = s.Carrier;
            break;
          default:
            key = s.ItemID;
            break;
        }
        string baseKey = key;

        int counter = 0;
        while (list.ContainsKey(key))
          key = baseKey + (counter++).ToString();
        list.Add(key, s);
      }
      return list;
    }
    #region IEnumerable Members

    /// <summary>
    /// Support enumeration (supports foreach).
    /// Enumerator overload to support LINQ use in Reports and other modules
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator)new ItemListEnumerator(this);
    }

    #endregion
  }
}
