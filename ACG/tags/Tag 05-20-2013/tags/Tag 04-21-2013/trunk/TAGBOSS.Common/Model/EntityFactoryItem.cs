using System;
using System.Collections;
using System.Reflection;

namespace TAGBOSS.Common.Model
{
  public class EntityFactoryItem
  {
    private Hashtable entityIndex = new Hashtable();
    private DateTime effectiveDate = DateTime.Now;
    private TEntity[] entities = null;

    public DateTime EffectiveDate
    {
      get { return effectiveDate; }
      set { effectiveDate = value; }
    }

    public TEntity[] Entities
    {
      get { return entities; }
      set { entities = value; }
    }

    public Hashtable EntityIndex
    {
      get { return entityIndex; }
      set { entityIndex = value; }
    }

  }
}
