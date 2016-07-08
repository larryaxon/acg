using System;
using System.Collections;

namespace TAGBOSS.Common.Model
{
  public class TEntityCollection
  {
    private Hashtable entitiesHierarchy = new Hashtable();

    private Hashtable entitiesRequested = null;
    private Hashtable itemTypesRequested = null;
    private Hashtable itemsRequested = null;

    public Hashtable EntitiesRequested
    {
      get { return entitiesRequested; }
    }

    public Hashtable ItemTypesRequested
    {
      get { return itemTypesRequested; }
    }

    public Hashtable ItemsRequested
    {
      get { return itemsRequested; }
    }

    public TEntityCollection(TEntity[] entities, Hashtable entitiesRequested, Hashtable itemTypesRequested, Hashtable itemsRequested) 
    {
      foreach (TEntity entity in entities)
      {
        entitiesHierarchy.Add(entity.Id, entity);
      }

      this.entitiesRequested = entitiesRequested;
      this.itemTypesRequested = itemTypesRequested;
      this.itemsRequested = itemTypesRequested;
    }

    public TEntity this[string entityID]
    {
      get
      {
        TEntity entity = null;
        string entityId = entityID.Trim().ToLower();

        if (entitiesRequested.ContainsKey(entityId) && entitiesHierarchy.ContainsKey(entityId))
          entity = (TEntity)entitiesHierarchy[entityId];

        return entity;
      }
    }

    public Hashtable getEntities()
    {
      Hashtable entities = new Hashtable();

      foreach (string entityId in entitiesRequested.Keys)
      {
        if (entitiesHierarchy.ContainsKey(entityId))
          entities.Add(entityId, entitiesHierarchy[entityId]);
      }

      return entities;
    }

    public int getItemTypeCount(string entityID)
    {
      int cntItemTypes = 0;
      string entityId = entityID.Trim().ToLower();

      TEntity eObj = this[entityId];
      if (eObj != null)
      {
        ArrayList itemTypeList = new ArrayList();
        Hashtable processedItemTypes = new Hashtable();

        TEntity tmpEntity = eObj;

        while (tmpEntity != null)
        {
          itemTypeList.Add(tmpEntity.ItemTypeIndex);
          tmpEntity = tmpEntity.EntityOwner;
        }

        //TODO; This is simply WRONG!!
        //Review this logic for "counting" ItemTypes!
        if (itemTypesRequested != null) 
        {
          foreach (string itemTypeId in itemTypesRequested.Keys)
          {
            if (!processedItemTypes.ContainsKey(itemTypeId))
            {
              foreach (TIndex itemTypeIndex in itemTypeList)
              {
                if (itemTypeIndex.Contains(itemTypeId))
                {
                  cntItemTypes++;
                  break;
                }
              }

              processedItemTypes.Add(itemTypeId, null);
            }
          }
        }
        else 
        {
          string itemTypeId= "";
          foreach (TIndex itemTypeIndex in itemTypeList)
          {
            foreach(TIndexItem tmpIdx in itemTypeIndex)
            {
              itemTypeId = ((TItemType)tmpIdx.ItemIdx).Id;
              if (!processedItemTypes.ContainsKey(itemTypeId))
              {
                cntItemTypes++;
                processedItemTypes.Add(itemTypeId, null);
              }
            }
          }
        }

      }

      return cntItemTypes;
    }

  }
}
