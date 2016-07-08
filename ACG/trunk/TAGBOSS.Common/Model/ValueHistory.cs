using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TAGBOSS.Common.Interface;

namespace TAGBOSS.Common.Model
{
  /// <summary>
  /// A value history (<valuehistory/>) node of an TAGAttribute object
  /// </summary>
  [SerializableAttribute]
  public class ValueHistory : DateHistory, IDataClassItem
  {
    private string valueType = "value";
    private object myvalue;
    /// <summary>
    /// The value of this attribute for this value range
    /// </summary>
    public object Value
    {
      get { return myvalue; }
      set { myvalue = value; }
    }

    /// <summary>
    /// What property was used in e_Attribute to specify this (value=? func=? Include=? Expression=? etc.)
    /// </summary>
    public string ValueType
    {
      /*
                     * left side of value statement in <valuehistory/> node, so ref= will return "ref", value= will return "value", etc.
                     */
      get { return valueType; }
      set { valueType = value; }
    }
    public bool Dirty
    {
      get { return base.Dirty; }
      set
      {
        base.Dirty = value;
        // if we are clearing the dirty flag and the value in this history is a valid tableheader, then clear its dirty flag, too
        if (!value && myvalue != null && myvalue.GetType() == typeof(TableHeader))
          ((TableHeader)myvalue).Dirty = false;
      }
    }
    public new object Clone()
    {
      ValueHistory vh = new ValueHistory();
      vh.Deleted = Deleted;
      vh.Dirty = Dirty;
      vh.EndDate = EndDate;
      vh.LastModifiedBy = LastModifiedBy;
      vh.LastModifiedDateTime = LastModifiedDateTime;
      vh.StartDate = StartDate;
      vh.ValueType = valueType;
      //TODO: 2010 0906 Confirm this change with Larry!!, but seems to be necessary!
      if (myvalue != null && myvalue.GetType() == typeof(TableHeader))
        vh.Value = ((TableHeader)myvalue).Clone();
      else
        vh.Value = myvalue;
      return vh;
    }
  }
}
