using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common.Model
{
  /// <summary>
  /// Class that is used to instantiate the Entities collection
  /// </summary>
  [SerializableAttribute]
  public class EntitiesCollection : DataClass<Entity> // class definition for the collection of Entity
  {
    /// <summary>
    /// Returns a copy of the current object. All children are also cloned, so that every copy of every object 
    /// is brand new.      
    /// </summary>
    /// <returns>A new copy of the object</returns>
    new public object Clone()
    {
      EntitiesCollection ec = new EntitiesCollection();
      return base.Clone<EntitiesCollection>(ec);
    }
  }
}
