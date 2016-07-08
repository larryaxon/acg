using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace ACG.Common
{
  [Serializable]
  public class SearchResult : PickListEntry
  {
    string _shortName = string.Empty;
    string _entityType = string.Empty;
    string _entityPath = string.Empty;
    string _legalName = string.Empty;
    public string EntityID { get { return ID; } set { ID = value; } }
    public string FullName { get { return Description; } set { Description = value; } }
    public string ShortName { get { return _shortName; } set { if (value == null) _shortName = string.Empty; else _shortName = value; } }
    public string EntityType { get { return _entityType; } set { if (value == null) _entityType = string.Empty; else _entityType = value; } }
    public string EntityPath { get { return _entityPath; } set { if (value == null) _entityPath = string.Empty; else _entityPath = value; } }
    public string LegalName { get { return _legalName; } set { if (value == null) _legalName = string.Empty; else _legalName = value; } }
    public SearchResult() { }
    public SearchResult(string entityID, string fullName, string shortName, string entityType)
    {
      EntityID = entityID;
      FullName = fullName;
      ShortName = shortName;
      EntityType = entityType;
    }
    public new bool MeetsCriteria(string criteria)
    {
      return (base.MeetsCriteria(criteria) ||
            _shortName.ToLower().Contains(criteria.ToLower()));
    }
    public new bool Equals(string id)
    {
      if (string.IsNullOrEmpty(id))
        return false;
      return id.Equals(ID, StringComparison.CurrentCultureIgnoreCase);
    }
    public new bool Equals(SearchResult s)
    {
      if (s == null || string.IsNullOrEmpty(s.ID))
        return false;
      return s.ID.Equals(ID, StringComparison.CurrentCultureIgnoreCase);
    }
    public new string ToString()
    {
      return LegalName;
    }
  }
}
