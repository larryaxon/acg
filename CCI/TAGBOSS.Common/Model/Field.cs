using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TAGBOSS.Common.Interface;

namespace TAGBOSS.Common.Model
{
  /// <summary>
  /// This is the Field class, which inherits from FieldBase, which is also the base for TAGAttribute.
  /// </summary>
  [SerializableAttribute]
  public class Field : FieldBase, IDataClassItem
  {
    /// <summary>
    /// Clone Method to make copies of fields objects
    /// </summary>
    /// <returns>A copy of the field object</returns>
    public object Clone()
    {
      Field f = new Field();
      f.ID = origID;
      f.Value = myvalue;
      f.OldValue = oldValue;
      f.ReadOnly = isReadOnly;
      f.Virtual = isVirtual;
      f.Deleted = isDeleted;
      f.Description = description;
      f.Dirty = isDirty;
      f.Visible = isVisible;
      return f;
    }
  }
}
