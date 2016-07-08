using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCI.Common
{
  [Serializable]
  public class ItemListEntry : IComparable, IEquatable<ItemListEntry>
  {
    private string _itemID = string.Empty;
    private string _itemDescription = string.Empty;
    private string _carrier = string.Empty;
    private string _carrierDescription = string.Empty;
    private string _masterItemID = string.Empty;
    private string _masterItemDescription = string.Empty;
    private string _itemSubCategory = string.Empty;
    private char[] _delim = new char[] { ',' };
    public string IDlower { get { return _itemID.ToLower(); } set { _itemID = value; } }
    public string IDupper { get { return _itemID.ToUpper(); } set { _itemID = value; } }
    public string ID { get { return string.Format("{0}, {1}",ItemID, Carrier); }  }
    public string ItemID { get { return _itemID; } set { _itemID = value; } }
    public string ItemDescription
    {
      get { if (string.IsNullOrEmpty(_itemDescription)) return _itemID; else return _itemDescription; }
      set { _itemDescription = value; }
    }    
    public string Carrier
    {
      get { return _carrier; }
      set { _carrier = value; }
    }    
    public string CarrierDescription
    {
      get { if (string.IsNullOrEmpty(_carrierDescription)) return _carrier; else return _carrierDescription; }
      set { _carrierDescription = value; }
    }
    public string ItemIDandDescription { get { return string.Format("{0}: {1}", _itemID, _itemDescription); } }
    public string MasterItemId { get { return _masterItemID; } set { _masterItemID = value; } }
    public string MasterItemDescription { get { return _masterItemDescription; } set { _masterItemDescription = value; } }
    public string ItemSubCategory { get { return _itemSubCategory; } set { _itemSubCategory = value; } }
    public string MasterItemText { get { return string.Format("{0}, {1}: {2}, {3} ({4})", _itemID, _itemDescription, _masterItemID, _masterItemDescription, _itemSubCategory); } }
    public ItemListEntry() { }
    public ItemListEntry(string item, string itemDescription, string carrier, 
        string carrierDescription, string itemSubCatgory)
    {
      ItemID = item;
      Carrier = carrier;
      ItemDescription = itemDescription;
      CarrierDescription = carrierDescription;
      ItemSubCategory = ItemSubCategory;
    }
    public bool MeetsCriteria(string criteria)
    {
      return (criteria == "*" ||
            ItemID.ToLower().Contains(criteria.ToLower()) ||
            Carrier.ToLower().Contains(criteria.ToLower()) ||
            ItemDescription.ToLower().Contains(criteria.ToLower()) ||
            CarrierDescription.ToLower().Contains(criteria.ToLower()));
    }
    public string ToString()
    {
      return string.Format("{0}, {1}",ItemDescription, CarrierDescription);
    }
    public string Value 
    { 
      get { return string.Format("{0}, {1}",ItemID, Carrier); } 
      set 
      {
        if (string.IsNullOrEmpty(value))
        {
          ItemID = string.Empty;
          Carrier = string.Empty;
        }
        else
        {
          string[] parts = value.Split(_delim);
          if (parts.GetLength(0) == 1)
          {
            ItemID = value;
            Carrier = string.Empty;
          }
          else
          {
            ItemID = parts[0];
            Carrier = parts[1];
          }
        }
      } 
    }
    public string Text { get { return Value; } set { Value = value; } }
    public string DisplayText { get { return ToString(); } }
    public int HashCode()
    {
      return (ItemID + Carrier).ToLower().GetHashCode();
    }
    int IComparable.CompareTo(object o)
    {
      if (o == null)
        return 1;
      if (o.GetType() == typeof(ItemListEntry))
        return CompareTo((ItemListEntry)o);
      throw new Exception("Cannot compare Search Result to type " + o.GetType().Name);
    }
    public int CompareTo(ItemListEntry i)
    {
      if (Equals(i))
        return 0;
      if (i == null)
        return 1;
      if (ItemID.CompareTo(i.ItemID) == 0)
        return Carrier.CompareTo(i.Carrier);
      else
        return ItemID.CompareTo(i.ItemID);
    }
    public bool Equals(object o)
    {
      if (o == null)
        return false;
      if (o.GetType() == typeof(string))
        return _itemID.Equals((string)o, StringComparison.CurrentCultureIgnoreCase);
      if (o.GetType() == typeof(ItemListEntry))
        return Equals((ItemListEntry)o);
      return false;
    }
    public bool Equals(ItemListEntry s)
    {
      if (s == null)
        return false;
      if (s.HashCode() == HashCode())
        return true;
      return false;
    }
  }
}
