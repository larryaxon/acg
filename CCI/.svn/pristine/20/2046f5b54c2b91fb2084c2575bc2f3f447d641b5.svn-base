using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCI.DesktopClient.Common
{
  [Serializable]
  public class AttributeChangeEventArgs : EventArgs
  {
    private string _entity;
    private string _itemType;
    private string _item;
    private string _attribute;
    private string _valueType;
    private DateTime? _startDate;
    private DateTime? _endDate;
    private ChangeType _type;
    private object _value;

    public enum ChangeType { Entity, ItemType, Item, ItemHistory, Attribute, ValueHistory, None };

    public string Entity { get { return _entity; } }
    public string ItemType { get { return _itemType; } }
    public string Item { get { return _item; } }
    public string Attribute { get { return _attribute; } }
    public string ValueType { get { return _valueType; } }
    public object Value { get { return _value; }  }
    public DateTime? StartDate { get { return _startDate; } }
    public DateTime? EndDate { get { return _endDate; } }
    public ChangeType Type { get { return _type; } }
    public AttributeChangeEventArgs(string entity, string itemtype, string item, string attribute, 
      string valueType, DateTime? startDate, DateTime? endDate, object value, ChangeType type)
    {
      _entity = entity;
      _itemType = itemtype;
      _item = item;
      _attribute = attribute;
      if (valueType == null || valueType == string.Empty)
        _valueType = "value";
      else
        _valueType = valueType;
      _startDate = startDate;
      _endDate = endDate;
      _type = type;
      _value = value;
    }
  }
}
