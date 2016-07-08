using System;
using System.Collections;
using System.Collections.Generic;
using TAGBOSS.Common;

namespace TAGBOSS.Common.Model
{
  [Serializable]
  public class TableHeaderRowCollection : IEnumerable
  {
    #region method data
    // we needed the custom comparer so the _rows collection works propertly and so we can enumerate properly
    private static TableHeaderUniqueKeyEqualityComparer keyComparer = new TableHeaderUniqueKeyEqualityComparer();
    private Dictionary<TableHeaderUniqueKey, TableHeaderRow> _rows = new Dictionary<TableHeaderUniqueKey, TableHeaderRow>(keyComparer);
    private ArrayList _keys = new ArrayList();  // this is the index (sorted list of keys)
    /*
     * if this collection is sorted by a different key than the default (from the Dictionary),
     * then we need two things extra: the list of columns in the alternate key sequence, 
     * and an array of the original keys to point to _rows. In this case, _keys is the list of keys we are sorted by,
     * and _altKeys is the list of original keys (in the same order) that point to _rows.
     * 
     * If we are not sorted by a different key than specified in the Dictionary, then the sorted key list IS the 
     * alternate/original keylist and these two data structures are null
     */
    private string[] _altKeyList = null;    //  this is the list of columns in the alternate sort key-
    private ArrayList _altKeys = null;          // this is the original key list
    #endregion

    #region public properties
    public Dictionary<TableHeaderUniqueKey, TableHeaderRow> Rows { get { return _rows; } }
    public ArrayList Keys { get { return _keys; } }
    #endregion

    public TableHeaderRowCollection()
    {

    }

    #region public methods
    public bool Contains(TableHeaderUniqueKey key)
    {
      if (_altKeys == null)
        return _rows.ContainsKey(key);
      else
        return ContainsKey(key);
    }
    private bool ContainsKey(TableHeaderUniqueKey key)
    {
      if (_altKeys == null)
        return _rows.ContainsKey(key);
      return (IndexOf(key) >= 0);
    }
    private int IndexOf(TableHeaderUniqueKey key)
    {
      ArrayList lookupList;
      if (_altKeys == null)
        lookupList = _keys;
      else
        lookupList = _altKeys;

      for (int i = 0; i < lookupList.Count; i++)
        if (key.Equals((TableHeaderUniqueKey)lookupList[i]))
          return i;
      return -1;
    }
    public TableHeaderRow this[TableHeaderUniqueKey key]
    {
      /*
       * We actually have TWO key indices (sometimes). If someone has sorted the rows by an index (not
       * the standard column List in the dictionary or the first row if none is defined), then
       * the _altKeys list will be defined (not null). In this case, the key we are given
       * could match either list, the primary (_keys and the Key of _rows) or the sorted (_altKeys).
       * 
       * So, whether we are doing a get or set, we first look to see if the _altKeys exists. If not,
       * we just use the _keys and _rows collections. If so, first look to see if there is a match
       * in the primary. If there is, we use that. If not, we look in the sorted list, use the same
       * index to find the primary key in the _keys list, and then use that to find the correct
       * row. 
       * 
       * Note that some of this difficulty is due to the fact that ArrayList does NOT use the Overloaded
       * Equals of the objects it is collecting. Therefore, we cannot rely upon ArrayList.Contains() and
       * related methods to properly indentify the TableHeaderUniqueKey entry.
       * 
       */
      get
      {
        if (Contains(key))
        {
          if (_altKeys == null) // there is no sorted key list
            return _rows[key];
          else
            if (_rows.ContainsKey(key)) // there is one, but the key provided is still in the prilery key list
              return _rows[key];
            else
              return (TableHeaderRow)_rows[(TableHeaderUniqueKey)_keys[IndexOf(key)]]; // the key provided is in the sorted list
        }
        else
          throw new KeyNotFoundException();
      }
      set
      {
        if (_altKeys == null) // no sorted key list
        {
          if (Contains(key))  // we already have one
          {
            _rows.Remove(key);
            _keys.RemoveAt(IndexOf(key));
          }
          Add(key, value);
        }
        else
        {
          int index = -1;
          if (_rows.ContainsKey(key))
          {
            // the key provided is a key in the primary key index (and in the _rows collection)
            for (int i = 0; i < _keys.Count; i++)
              if (key.Equals((TableHeaderUniqueKey)_keys[i]))
              {
                index = i;
                break;
              }
            if (index < 0)
              throw new Exception("Index out of sync");
            _rows.Remove(key);

          }
          else
          {
            // the key provided is in the sorted key index
            index = IndexOf(key);
            if (index < 0)
              throw new Exception("Index out of sync");
            _rows.Remove((TableHeaderUniqueKey)_altKeys[index]);
          }
          _keys.RemoveAt(index);
          _altKeys.RemoveAt(index);
        }
      }
    }
    public TableHeaderRow this[int index]
    {
      get
      {
        if (index < 0 || index > _rows.Count)
          throw new KeyNotFoundException();
        //if (_rows.Count != _keys.Count)
        //  throw new Exception("Corrupt TableHeaderRows collection, keys are out of sync");
        if (_altKeys == null)
          return _rows[(TableHeaderUniqueKey)_keys[index]];
        else
          return _rows[(TableHeaderUniqueKey)_altKeys[index]];
      }
    }
    public int Count { get { return _rows.Count; } }
    public void Add(TableHeaderUniqueKey key, TableHeaderRow item)
    {
      TableHeaderUniqueKey sortedKey;
      if (_altKeyList == null)
        sortedKey = key;
      else
        sortedKey = new TableHeaderUniqueKey(item, _altKeyList);
      AddKey(sortedKey, key);
      _rows.Add(key, item);
    }

    public TableHeaderRow First()
    {
      if (Count > 0)
        if (_altKeys == null)
          return _rows[(TableHeaderUniqueKey)Keys[0]];
        else
        {
          int index = IndexOf((TableHeaderUniqueKey)_altKeys[0]);
          return _rows[(TableHeaderUniqueKey)_keys[index]];
        }
      else
        return null;
    }
    public TableHeaderUniqueKey KeyAt(int index)
    {
      if (index < 0 || index > _rows.Count)
        throw new KeyNotFoundException();
      if (_rows.Count != _keys.Count || (_altKeys != null && _rows.Count != _altKeys.Count))
        throw new Exception("Corrupt TableHeaderRows collection, keys are out of sync");
      if (_altKeys == null) // if we are not sorted by an alternate key
        return (TableHeaderUniqueKey)_keys[index];  // then the _keys is the only list
      else
        return (TableHeaderUniqueKey)_altKeys[index]; // otherwise, we need to look in _altkeys to find the "original" key
    }
    public void Remove(TableHeaderUniqueKey key)
    {
      if (_altKeys == null)
      {
        _rows.Remove(key);
        _keys.RemoveAt(IndexOf(key));
      }
      else
      {
        int index = IndexOf(key);
        if (_rows.ContainsKey(key))   // key is in primary key list
        {
          _rows.Remove(key);
        }
        else
        {
          TableHeaderUniqueKey origKey = (TableHeaderUniqueKey)_altKeys[index];
          _rows.Remove(origKey);
        }
        _keys.RemoveAt(index);
        _altKeys.RemoveAt(index);
      }   
    }
    public void Clear()
    {
      _rows.Clear();
      _keys.Clear();
      _altKeys = null;
      _altKeyList = null;
    }
    /// <summary>
    /// Sorts the row collection by the specified column list, using the internal indexed key list
    /// </summary>
    /// <param name="keys"></param>
    public void Sort(string[] keys)
    {
      _keys.Clear();
      if (_altKeys == null)
        _altKeys = new ArrayList();
      else
        _altKeys.Clear();
      _altKeyList = keys;
      foreach (KeyValuePair<TableHeaderUniqueKey, TableHeaderRow> entry in _rows)
      {
        // get the row and original key from the row collection
        TableHeaderUniqueKey sortedKey = new TableHeaderUniqueKey(entry.Value, keys); // create a bew key based on the sorted keylist
        AddKey(sortedKey, entry.Key);
      }
    }
    #endregion

    #region private methods
    private void AddKey(TableHeaderUniqueKey sortedKey, TableHeaderUniqueKey origKey)
    {
      if (this.Keys.Count > 0)
      {
        int pos = binarySort(this.Keys, sortedKey);

        if (pos == -1)
        {
          if (_altKeys != null && origKey != null)
            _altKeys.Add(origKey);
          this.Keys.Add(sortedKey);
        }
        else
        {
          if (_altKeys != null && origKey != null)
            _altKeys.Insert(pos, origKey);
          this.Keys.Insert(pos, sortedKey);
        }
      }
      else
      {
        if (_altKeys != null && origKey != null)
          _altKeys.Add(origKey);
        this.Keys.Add(sortedKey);
      }
    }
    private int binarySort(ArrayList list, TableHeaderUniqueKey key)
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
        iCompareThis = ((TableHeaderUniqueKey)list[mid]).CompareTo(key);
        if (iCompareThis == 0)  // exact match of the key is found, so insert the new key before it
          return mid;
        midPrior = Math.Max(mid - 1, 0);  // this is the one before.
        iComparePrior = ((TableHeaderUniqueKey)list[midPrior]).CompareTo(key);
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
    #endregion

    #region IEnumerable Members

    /// <summary>
    /// Support enumeration (supports foreach).
    /// Enumerator overload to support LINQ use in Reports and other modules
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator)new TableHeaderRowEnumerator(this);
    }

    #endregion

  }

}
