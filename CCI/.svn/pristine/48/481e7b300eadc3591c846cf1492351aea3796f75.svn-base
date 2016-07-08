using System;
using System.Collections;
using System.Reflection;

namespace TAGBOSS.Common.Model
{
  public sealed class SystemEntityFactory
  {
    private static readonly SystemEntityFactory instance = new SystemEntityFactory();

    private string effectiveDateMaxKey = null;
    private Hashtable effectiveDateIndex = new Hashtable();
    private ITAGBOSSData ad;

    private SystemEntityFactory() 
    {
      Assembly assembly = Assembly.Load("TAGBOSS.AttributeEngine2, Culture = neutral");
      ad = (ITAGBOSSData)assembly.CreateInstance("TAGBOSS.Sys.AttributeEngine2.Dal.AttributeData");

      ReLoad();
    }

    public static SystemEntityFactory Instance
    {
      get { return instance; }
    }

    public TEntity[] Entities(DateTime effectiveDate)
    {
      return getEntityFactoryItem(effectiveDate).Entities;
    }

    public TEntity getEntity(string entity, DateTime effectiveDate) 
    {
      return (TEntity)getEntityFactoryItem(effectiveDate).EntityIndex[entity];
    }

    public void ReLoad()
    {
      effectiveDateMaxKey = null;
      effectiveDateIndex.Clear();
      getEntityFactoryItem(DateTime.Now);
    }

    public ArrayList getListCacheEffectiveDates()
    {
      ArrayList listCacheEffectiveDate = new ArrayList();
      if (effectiveDateIndex.Count > 0)
      {
        foreach (string effectiveDateKey in effectiveDateIndex.Keys)
          listCacheEffectiveDate.Add(effectiveDateKey);
      }
      return listCacheEffectiveDate;
    }

    private EntityFactoryItem getEntityFactoryItem(DateTime effectiveDate) 
    {
      lock (effectiveDateIndex.SyncRoot)
      {
        //We must only take the Date part of the effectiveDate being passed on! for this we convert it to string and then back to date!
        bool reloadDictionary = (effectiveDateIndex.Count == 0);
        EntityFactoryItem entityFactoryItem = null;
        string effectiveDateKey = effectiveDate.ToString(Constants.ID_STARTDATE_FORMAT);

        if (effectiveDateMaxKey == null)
          effectiveDateMaxKey = effectiveDateKey;
        else if (effectiveDateMaxKey.CompareTo(effectiveDateKey) < 0)
          effectiveDateKey = effectiveDateMaxKey;

        if (effectiveDateIndex.ContainsKey(effectiveDateKey))
        {
          entityFactoryItem = (EntityFactoryItem)effectiveDateIndex[effectiveDateKey];
        }
        else
        {
          entityFactoryItem = new EntityFactoryItem();
          entityFactoryItem.EffectiveDate = new DateTime(effectiveDate.Year, effectiveDate.Month, effectiveDate.Day);
          entityFactoryItem.Entities = ad.SystemEntities(effectiveDate);

          foreach (TEntity entity in entityFactoryItem.Entities)
            entityFactoryItem.EntityIndex.Add(entity.EntityHash, entity);

          effectiveDateIndex.Add(effectiveDateKey, entityFactoryItem);
        }

        //TODO:Here we load the Dictionary!
        if (reloadDictionary)
          DictionaryFactory.getInstance().setDictionary(new Dictionaries(loadDictionaryFromEAC(entityFactoryItem.Entities)));

        //Finish the load of the Dictionary
        return entityFactoryItem;
      }
    }

    private EntityAttributesCollection loadDictionaryFromEAC(TEntity[] tSysEntities)
    {
      EntityAttributesCollection DictEAC = new EntityAttributesCollection();
      TItem tDictItem = null;
      string DictEntityID = "Dictionary";

      for (int i = 0; i < tSysEntities.GetLength(0); i++)
      {
        if (tSysEntities[i].Id == DictEntityID.ToLower())
        {
          Entity DictEnt = new Entity();
          DictEnt.ID = DictEntityID;
          foreach (TIndexItem indexItem in tSysEntities[i].ItemIndex.List)
          {
            tDictItem = (TItem)indexItem.ItemIdx;

            if (tDictItem == null)
              continue;

            if (!DictEnt.ItemTypes.Contains(tDictItem.ItemType.Id))
            {
              ItemType DictIT = new ItemType();
              DictIT.ID = tDictItem.ItemType.OrigId;
              DictEnt.ItemTypes.Add(DictIT);
            }

            Item DictItem = new Item();
            DictItem.ID = tDictItem.OrigId;
            DictItem.StartDate = tDictItem.Entity.StartDate;
            DictItem.EndDate = tDictItem.Entity.EndDate;
            DictItem.EffectiveDate = tDictItem.Entity.EffectiveDate;

            foreach (TIndexItem attrObjIdx in tDictItem.getAttributeEnumerable())
            {
              TAttribute tAttribute = (TAttribute)attrObjIdx.ItemIdx;
              TAGAttribute attribute = new TAGAttribute();

              attribute.ID = tAttribute.OrigId;
              attribute.StartDate = Constants.PASTDATETIME;
              attribute.EndDate = Constants.FUTUREDATETIME;
              attribute.EffectiveDate = tAttribute.Item.Entity.EffectiveDate;
              attribute.IsIncluded = Constants.AnyOn(tAttribute.Flags, EAttributeFlags.IsIncluded);
              attribute.IsInherited = Constants.AnyOn(tAttribute.Flags, EAttributeFlags.IsInherited);
              attribute.Parent = DictItem;

              foreach (TValueHistory tValueHistory in tAttribute.History)
              {
                ValueHistory vh = new ValueHistory();
                vh.StartDate = tValueHistory.StartDate;
                vh.EndDate = tValueHistory.EndDate;
                vh.Value = tValueHistory.Value;
                vh.LastModifiedBy = tValueHistory.LastModifiedBy;
                vh.LastModifiedDateTime = tValueHistory.LastModifiedDateTime;

                if (vh.StartDate <= tSysEntities[i].EffectiveDate && tSysEntities[i].EffectiveDate <= vh.EndDate)
                {
                  attribute.StartDate = vh.StartDate;
                  attribute.EndDate = vh.EndDate;
                  attribute.ReferenceValueSource = tValueHistory.ReferenceValueSource;

                  attribute.OverwriteValue = true;
                  attribute.Value = vh.Value;
                  attribute.OverwriteValue = false;
                }

                attribute.Values.Add(vh);
              }

              DictItem.Add(attribute);
            }

            DictEnt.ItemTypes[tDictItem.ItemType.Id].Add(DictItem);
          }

          DictEAC.Entities.Add(DictEnt);
        }
      }

      return DictEAC;
    }

  }
}
