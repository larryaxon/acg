using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common.Model
{
  /// <summary>
  /// This serves as the basis for the Fields collection, for both Entity and DataRecord/Data Table
  /// </summary>
  [SerializableAttribute]
  public class FieldsCollection : DataClass<Field>          // class definition for the collection of Fields
  {
    /// <summary>
    /// 
    /// </summary>
    public FieldsCollection Fields
    {
      get { return this; }
    }

    /// <summary>
    /// easy way to get a value of a field, without having to check if the attribute exists
    /// </summary>
    /// <param name="fieldName">Name of the attribute we want</param>
    /// <returns>Attriute.Value if it exists, otherwise null</returns>
    public object getValue(string fieldName)
    {
      if (Contains(fieldName))
        return this[fieldName].Value;
      else
        return null;
    }
    public void setValue(string fieldname, object value)
    {
      if (Contains(fieldname))
        this[fieldname].Value = value;
      else
        AddField(fieldname, value);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pID"></param>
    /// <param name="pValue"></param>
    public void AddField(string pID, object pValue)          // Adds a entry from just the ID and the Value without
    {                                                       // requiring the calling program to create a Field object
      Field f = new Field();
      f.ID = pID;
      f.Value = pValue;
      Add(pID, (Field)f);
    }
    /// <summary>
    /// makes a copy of this fields collection in item class format and returns it
    /// </summary>
    /// <returns></returns>
    public Item ToItem()
    {
      Item item = new Item();
      item.ID = OriginalID;
      foreach (Field f in this)
      {
        TAGAttribute a = new TAGAttribute();
        a.ID = f.OriginalID;
        a.Value = f.Value;
        a.ReadOnly = f.ReadOnly;
        a.DataType = f.DataType;
        a.Description = f.Description;
        item.Attributes.Add(a);
      }
      return item;
    }
    new public object Clone()
    {
      FieldsCollection fc = new FieldsCollection();
      return base.Clone<FieldsCollection>(fc);
    }
  }
}
