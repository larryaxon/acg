using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using ACG.Common;

namespace TAGBOSS.Common
{
  [Serializable]
  public class EntryRowBase : SortedItemBase, IEnumerable
  {
    public enum EntryMode { Read, Update, Delete, Add };
    public EntryMode Mode { get; set; }
    protected Hashtable _fields = new Hashtable();
    public Hashtable fieldStore { get { return _fields; } set { _fields = value; } }
    public EntryRowBase Fields { get { return this; } }
    public EntryRowBase()
    {
      Mode = EntryMode.Read;
    }
    public void Add(string key, string value)
    {
      if (key == null)
        return;
      string lcKey = key.ToLower();
      if (_fields.ContainsKey(lcKey))
        _fields[lcKey] = value;
      else
        _fields.Add(key.ToLower(), value);
    }
    public void Add(string key, object value)
    {
      if (key == null)
        return;
      string lcKey = key.ToLower();
      if (_fields.ContainsKey(lcKey))
        _fields[lcKey] = value;
      else
        _fields.Add(key.ToLower(), value);
    }
    public int Count { get { return _fields.Count; } }
    public bool ContainsKey(string key) 
    {
      if (key == null)
        return false;
      return _fields.ContainsKey(key.ToLower()); 
    }
    public bool Contains(string key)
    {
      return ContainsKey(key);
    }
    public string this[string key]
    {
      get 
      { 
        if (key == null)  
         return null;
        string myKey = key.ToLower();
        if (_fields.Contains(myKey))
        {
          object oReturn = _fields[myKey];
          if (oReturn == null)
            return null;
          else
            return oReturn.ToString();
        }
        else
          return null;
      }
      set 
      { 
        if (key == null) 
          throw new KeyNotFoundException(); 
        string myKey = key.ToLower();
        if (_fields.ContainsKey(myKey))
          _fields[myKey] = value;
        else
          _fields.Add(myKey, value);
      }
    }
    public EntryRowBase Clone()
    {
      EntryRowBase newRow = (EntryRowBase)base.Clone();
      newRow.Mode = Mode;
      newRow.fieldStore = (Hashtable)fieldStore.Clone();
      return newRow;
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator)_fields.GetEnumerator();
    }
  }
}
