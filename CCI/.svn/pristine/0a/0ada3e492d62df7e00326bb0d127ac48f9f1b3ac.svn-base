using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common.Model
{

  /// <summary>
  /// This serves as the basis for the Attributes collection, for both Entity and DataRecord/Data Table
  /// </summary>
  [SerializableAttribute]
  public class AttributesCollection : DataClass<TAGAttribute>        // class definition for the collection of Attributes
  {
    public AttributesCollection Attributes
    {
      get { return this; }
    }
    /// <summary>
    /// easy way to get a value of an attribute, without having to check if the attribute exists
    /// </summary>
    /// <param name="attributeName">Name of the attribute we want</param>
    /// <returns>Attriute.Value if it exists, otherwise null</returns>
    public object getValue(string attributeName)
    {
      if (Contains(attributeName))
        return this[attributeName].Value;
      else
        return null;
    }
    public void setValue(string attributeName, object value)
    {
      if (Contains(attributeName))
        this[attributeName].Value = value;
      else
        AddAttribute(attributeName, value);
    }
    public new void Add(TAGBOSS.Common.Model.TAGAttribute pValue)
    {
      TAGAttribute a = new TAGAttribute();
      a.ID = pValue.ID;
      a.Value = pValue.Value;
      a.ReadOnly = pValue.ReadOnly;
      a.Deleted = pValue.Deleted;
      a.LastModifiedBy = pValue.LastModifiedBy;
      a.LastModifiedDateTime = pValue.LastModifiedDateTime;
      a.ValueType = pValue.ValueType;
      a.Dirty = pValue.Dirty;
      base.Add(a);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pID"></param>
    /// <param name="pValue"></param>
    public void AddAttribute(string pID, object pValue)      // Adds a entry from just the ID and the Value without
    {                                                        // requiring the calling program to create a TAGAttribute object
      TAGAttribute a = new TAGAttribute();
      a.ID = pID;
      a.Value = pValue;
      Add(pID, (TAGAttribute)a);
    }
    /// <summary>
    /// Returns the collection of attributes in Item form
    /// </summary>
    /// <returns>An Item that contains all of the attributes of this collection</returns>
    public Item ToItem()
    {
      Item i = new Item();
      i.ID = ID;
      foreach (TAGAttribute a in this)
        i.Add(a);
      return i;
    }
    public new object Clone()
    {
      AttributesCollection ac = new AttributesCollection();
      return base.Clone<AttributesCollection>(ac);
    }
  }
}
