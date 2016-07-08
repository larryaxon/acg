using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common.Model
{
  [Serializable]
  public class NewEntityReturn
  {
    bool hasError = false;
    string _newID = string.Empty;
    string _errorMessage = string.Empty;
    string _shortName = string.Empty;
    string _legalName = string.Empty;
    string _fullName = string.Empty;
    string _entityType = string.Empty;
    string _owner = string.Empty;
    InvalidEntries _errorList = new InvalidEntries();
    DateTime _startDate = DateTime.Today;
    string _fein = string.Empty;

    public string FEIN
    {
      get { return _fein; }
      set { _fein = value; }
    }

    public string LegalName
    {
      get { return _legalName; }
      set { _legalName = value; }
    }
    public DateTime StartDate
    {
      get { return _startDate; }
      set { _startDate = value; }
    }

    public string Owner
    {
      get { return _owner; }
      set { _owner = value; }
    }

    public InvalidEntries ErrorList
    {
      get { return _errorList; }
    }

    public string EntityType
    {
      get { return _entityType; }
      set { _entityType = value; }
    }

    public bool HasError
    {
      get { return hasError || _errorList.Count > 0 || !string.IsNullOrEmpty(_errorMessage); }
      set { hasError = value; }
    }

    public string ErrorMessage
    {
      get 
      {
        if (!string.IsNullOrEmpty(_errorMessage))
          return _errorMessage;
        else
          if (_errorList.Count > 0)
            return _errorList[0].ErrorMessage;
          else
            return string.Empty;
      }
      set 
      { 
        _errorMessage = value;
        if (!string.IsNullOrEmpty(_errorMessage))
        {
          InvalidEntry entry = new InvalidEntry();
          if (!string.IsNullOrEmpty(NewID))
            entry.AttributeName = NewID;
          else if (!string.IsNullOrEmpty(_fullName))
            entry.AttributeName = _fullName;
          else if (!string.IsNullOrEmpty(_entityType))
            entry.AttributeName = _entityType;
          else
            entry.AttributeName = "NewEntityAdd";
          entry.ErrorMessage = value;
          entry.Context = "Entity";
          _errorList.Add(entry);
        }
      }
    }

    public string NewID
    {
      get { return _newID; }
      set { _newID = value; }
    }

    public string ShortName
    {
      get { return _shortName; }
      set { _shortName = value; }
    }

    public string FullName
    {
      get { return _fullName; }
      set { _fullName = value; }
    }

  }
}
