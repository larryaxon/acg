using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace CCI.Common
{
  [Serializable]
  public class PickListEntry : IComparable, IEquatable<PickListEntry>, IComparable<PickListEntry>
  {
    public const string VALUEMEMBER = "OriginalID";
    public const string DISPLAYMEMBER = "Description";
    protected string _id = string.Empty;
    protected string _description = string.Empty;
    public string ID { get { return _id == null ? _id : _id.ToLower(); } set { if (value == null) _id = string.Empty; else _id = value; } }
    public string IDupper { get { return _id == null ? _id : _id.ToUpper(); } set { if (value == null) _id = string.Empty; else _id = value; } }
    public string OriginalID { get { return _id; } set { if (value == null) _id = string.Empty; else _id = value; } }
    public string Description { get { return _description; } set { if (value == null) _description = string.Empty; else _description = value; } }

    public PickListEntry() { }
    public PickListEntry(string id, string description)
    {
      ID = id;
      Description = description;
    }
    public bool MeetsCriteria(string criteria)
    {
      if (criteria == null)
        return false;
      return (criteria == "*" || ID.Contains(criteria.ToLower()) ||
            Description.Contains(criteria.ToLower()) );
    }
    public string ToString()
    {
      return _description;
    }
    public string Value { get { return _id; } set { _id = value; } }
    public string Text { get { return _id; } set { _id = value; } }
    public int HashCode()
    {
      if (_id == null)
        return -1;
      return _id.ToLower().GetHashCode();
    }
    int IComparable.CompareTo(object o)
    {
      if (o == null)
        return 1;
      if (o.GetType() == typeof(PickListEntry))
        return CompareTo((PickListEntry)o);
      throw new Exception("Cannot compare Search Result to type " + o.GetType().Name);
    }
    public int CompareTo(PickListEntry s)
    {
      if (Equals(s))
        return 0;
      if (s == null)
        return 1;
      return ID.CompareTo(s.ID);
    }
    public bool Equals(object o)
    {
      if (o == null)
        return false;
      if (o.GetType() == typeof(PickListEntry))
        return Equals((PickListEntry)o);
      if (o.GetType() == typeof(string))
        return ID.Equals((string)o, StringComparison.CurrentCultureIgnoreCase);
      return false;
    }
    public bool Equals(PickListEntry s)
    {
      if (s == null)
        return false;
      if (s.HashCode() == HashCode())
        return true;
      return false;
    }
  }
}
