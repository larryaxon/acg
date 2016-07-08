using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common.Model
{
  /// <summary>
  /// This class is used to instantiate the ItemTypes collection in Entity. 
  /// </summary>
  [SerializableAttribute]
  public class ItemTypeCollection : DataClass<ItemType>
  {
    /// <summary>
    /// Returns a copy of the current object. All children are also cloned, so that every copy of every object 
    /// is brand new.      
    /// </summary>
    /// <returns>A new copy of the object</returns>
    public new object Clone()
    {
      ItemTypeCollection ic = new ItemTypeCollection();
      return base.Clone<ItemTypeCollection>(ic);
    }
  }
}
