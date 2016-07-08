using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAGBOSS.Common.Interface;

namespace TAGBOSS.Common.Model
{
  [SerializableAttribute]
    public class EntityHierarchy : DataClass<EntityHierarchy>, IDataClassContainer<EntityHierarchy>, IDataClassItem
  {
    private Entity myEntity = null;

    public EntityHierarchy Entities
    {
      get
      {
        return this;
      }
    }
    public Entity Entity
    {
      get { return myEntity; }
      set { myEntity = value; }
    }
    /// <summary>
    /// Returns a copy of the current object. All children are also cloned, so that every copy of every object 
    /// is brand new.      
    /// </summary>
    /// <returns>A new copy of the object</returns>
    new public object Clone()
    {
      EntityHierarchy eh = new EntityHierarchy();
      return base.Clone<EntityHierarchy>(eh);
    }
  }
}
