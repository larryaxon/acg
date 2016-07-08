using System;
using System.Collections;
using System.Collections.Generic;

namespace TAGBOSS.Common.Model
{
  [Serializable]
  public class TItem
  {
    private TIndex attributeIndex = new TIndex();

    private Hashtable itemHistory = new Hashtable();

    private ArrayList includes = new ArrayList();
    private ArrayList includedItems = new ArrayList();
    private TItemType itemType = null;
    private bool hasHierarchy = false;

    public bool HasHierarchy
    {
      get { return hasHierarchy; }
      set { hasHierarchy = value; }
    }

    public TEntity Entity { get; set; }

    public TItemType ItemType 
    { 
      get{ return itemType;}
      set
      {
        itemType = value;
        if (!itemType.ItemIndex.Contains(ItemHash))
        {
          itemType.ItemIndex.Add(new TIndexItem() { ItemObj = itemType, ItemIdx = this });
        }
        else 
        {
          //TODO:THIS SHOULD NOT HAPPEN!
        }
      }
    }
    
    public string Id { get; set; }
    
    public string OrigId { get; set; }

    public string LastModifiedBy { get; set; }

    public bool IsVirtual{ get; set; }
    
    public DateTime StartDate { 
      get
      {
        DateTime startDate = Constants.PASTDATETIME;

        if(itemHistory.Count > 0)
        {
          foreach(TItemHistory tItemHistory in itemHistory.Values)
          {
            if(tItemHistory.StartDate <= this.Entity.EffectiveDate && this.Entity.EffectiveDate <= tItemHistory.EndDate)
            {
              startDate = tItemHistory.StartDate;
              break;
            }
          }
        }

        return startDate;
      }
    }

    public DateTime EndDate
    {
      get
      {
        DateTime endDate = Constants.FUTUREDATETIME;

        if (itemHistory.Count > 0)
        {
          foreach (TItemHistory tItemHistory in itemHistory.Values)
          {
            if (tItemHistory.StartDate <= this.Entity.EffectiveDate && this.Entity.EffectiveDate <= tItemHistory.EndDate)
            {
              endDate = tItemHistory.EndDate;
              break;
            }
          }
        }

        return endDate;
      }
    }

    public DateTime LastModifiedDateTime { get; set; }

    public TAttribute[] Attributes { get; set; }

    public string ItemHash
    {
      get
      {
        return Entity.Id + "." + ItemType.Id + "." + Id;
      }
    }

    public string Description
    {
      get
      {
        TAttribute aObj = getAttribute("description", this.Entity);
        if (aObj != null)
        {
          string desc = "";

          if(aObj.Value != null)
            desc = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, aObj.SolveValue(this));

          if (desc == string.Empty)
            return this.OrigId;
          else
            return desc;
        }
        else
          return this.OrigId;
      }
    }

    public bool IsInherited(TEntity eObj) 
    {
      return (this.Id == Constants.DEFAULT_ITEM || this.Entity.Id != eObj.Id);
    }

    public bool IsExpired(TEntity eObj) 
    {
      bool isExpired = true;
      TEntity tmpEntity = (eObj != null ? eObj : this.Entity);

      if (itemHistory.Count > 0 && tmpEntity != null)
      {
        foreach (TItemHistory tIH in itemHistory) 
        {
          if (tIH.StartDate <= tmpEntity.EffectiveDate && tmpEntity.EffectiveDate <= tIH.EndDate) 
          {
            isExpired = false;
            break;
          }
        }
      }

      return isExpired;
    }

    public ArrayList Includes
    {
      get { return includes; }
    }

    public ArrayList IncludedItems
    {
      get { return includedItems; }
    }

    public TIndex AttributeIndex
    {
      get { return attributeIndex; }
    }

    public Hashtable ItemHistory
    {
      get { return itemHistory; }
      set { itemHistory = value; }
    }
    
    public TAttribute getAttribute(string attributeId, TEntity eObj) 
    {
      TAttribute aObj = null;
      TAttribute aObjTmp = null;

      string itemTypeId = this.itemType.Id;
      string itemId = this.Id;

      string currentEntityItemHash = "";
      string defaultEntityItemHash = "";
      string currentEntityDefaultItemHash = "";
      string defaultEntityDefaultItemHash = "";

      TEntity tmpEntity = (eObj != null ? eObj : this.Entity);
      TItem tmpItem = null;
      TIndexItem idxItem = null;

      while (tmpEntity != null)
      {
        currentEntityItemHash = tmpEntity.Id + "." + itemTypeId + "." + this.Id;
        defaultEntityItemHash = Constants.DEFAULT_ENTITY + "." + itemTypeId + "." + this.Id;
        currentEntityDefaultItemHash = tmpEntity.Id + "." + itemTypeId + "." + Constants.DEFAULT_ITEM;
        defaultEntityDefaultItemHash = Constants.DEFAULT_ENTITY + "." + itemTypeId + "." + Constants.DEFAULT_ITEM;

        if (tmpEntity.ItemIndex.Contains(currentEntityItemHash))
          idxItem = tmpEntity.ItemIndex[currentEntityItemHash];
        else if (tmpEntity.ItemIndex.Contains(defaultEntityItemHash))
          idxItem = tmpEntity.ItemIndex[defaultEntityItemHash];
        else if (tmpEntity.ItemIndex.Contains(currentEntityDefaultItemHash))
          idxItem = tmpEntity.ItemIndex[currentEntityDefaultItemHash];
        else if (tmpEntity.ItemIndex.Contains(defaultEntityDefaultItemHash))
          idxItem = tmpEntity.ItemIndex[defaultEntityDefaultItemHash];
        else
          idxItem = null;

        if (idxItem != null)
        {
          tmpItem = (TItem)idxItem.ItemObj;
          if (tmpItem.attributeIndex.Contains(attributeId))
          {
            if (aObj == null)
            {
              aObj = (TAttribute)tmpItem.attributeIndex[attributeId].ItemObj;
            }
            else
            {
              //The attribute was already found from a previous entity in the inheritance chain, let us see if it comes from the Default entity
              aObjTmp = (TAttribute)tmpItem.attributeIndex[attributeId].ItemObj;

              if (((Constants.AnyOn(aObj.Flags, EAttributeFlags.FromDefaultEntity) && !Constants.AnyOn(aObjTmp.Flags, EAttributeFlags.FromDefaultEntity))
                || (Constants.AnyOn(aObj.Flags, EAttributeFlags.FromDefaultItem) && !Constants.AnyOn(aObjTmp.Flags, EAttributeFlags.FromDefaultItem)))
                && (aObjTmp.Value != null && (string)aObjTmp.Value != ""))
              {
                aObj = aObjTmp;
              }
            }
          }
        }

        tmpEntity = tmpEntity.EntityOwner;
      }

      return aObj;
    }

    public TAttributeTHMerge getTHMergeAttribute(string attributeId, TEntity eObj)
    {
      TAttributeTHMerge attributeTHMerge = new TAttributeTHMerge();
      TIndexItem idxItemEntry = null;

      attributeTHMerge.AttributeId = attributeId;

      string itemTypeId = this.itemType.Id;
      string itemId = this.Id;

      string currentEntityItemHash = "";
      string defaultEntityItemHash = "";
      string currentEntityDefaultItemHash = "";
      string defaultEntityDefaultItemHash = "";

      TEntity tmpEntity = (eObj != null ? eObj : this.Entity);
      TItem tmpItemIdx = null;
      TItem tmpItemObj = null;

      while (tmpEntity != null)
      {
        currentEntityItemHash = tmpEntity.Id + "." + itemTypeId + "." + this.Id;
        defaultEntityItemHash = Constants.DEFAULT_ENTITY + "." + itemTypeId + "." + this.Id;
        currentEntityDefaultItemHash = tmpEntity.Id + "." + itemTypeId + "." + Constants.DEFAULT_ITEM;
        defaultEntityDefaultItemHash = Constants.DEFAULT_ENTITY + "." + itemTypeId + "." + Constants.DEFAULT_ITEM;

        if (tmpEntity.ItemIndex.Contains(currentEntityItemHash))
          idxItemEntry = tmpEntity.ItemIndex[currentEntityItemHash];
        else if (tmpEntity.ItemIndex.Contains(defaultEntityItemHash))
          idxItemEntry = tmpEntity.ItemIndex[defaultEntityItemHash];
        else if (tmpEntity.ItemIndex.Contains(currentEntityDefaultItemHash))
          idxItemEntry = tmpEntity.ItemIndex[currentEntityDefaultItemHash];
        else if (tmpEntity.ItemIndex.Contains(defaultEntityDefaultItemHash))
          idxItemEntry = tmpEntity.ItemIndex[defaultEntityDefaultItemHash];
        else
          idxItemEntry = null;

        if (idxItemEntry != null)
        {
          tmpItemIdx = (TItem)idxItemEntry.ItemObj;
          tmpItemObj = (TItem)idxItemEntry.ItemObj;

          if (tmpItemObj.attributeIndex.Contains(attributeId)) 
          {
            if (!attributeTHMerge.AttributeMergeItemHashRAList.ContainsKey(tmpItemIdx.ItemHash))
            {
              attributeTHMerge.AttributeMergeItemHashRAList.Add(tmpItemIdx.ItemHash, idxItemEntry);
              attributeTHMerge.AttributeMergeItemHashList.Add(tmpItemIdx.ItemHash);
            }
          }
        }

        tmpEntity = tmpEntity.EntityOwner;
      }

      return attributeTHMerge;
    }

    public ArrayList getAttributeEnumerable()
    {
      return getAttributeEnumerable(null);
    }

    public ArrayList getAttributeEnumerable(TEntity eObj)
    {
      TIndex processedAttributes = new TIndex();
      ArrayList attributeEnumerable = null;

      TItemType itObjObj = null;
      TIndexItem idxItemEntry = null;

      TItem iObjIdx1 = null;
      TItem iObjObj1 = null;
      TItem iObjObj = null;
      
      TAttribute aObjIdx = null;
      TAttribute aObjObj = null;
      TAttribute aObjIdxPrv = null;
      TAttribute aObjObjPrv = null;

      TEntity tmpEntity = null;
      TIndexItem tmpIdxAttrEntry = null;

      string itemTypeId = "";
      string entityBaseId = "";
      int entityLevel = 0;

      //There can be four sources of attrbutes for this Item, once resolved, the Default entity corresponding Item of Default Item
      //And any of the Entities in the Hierarchy chain corresponding Item of Default Item
      //As we read from the bottom up, we will always keep the most down attribute, UNLESS it comes from the Default Entity, or from the Default Item
      //in which case we will overwrite it with the hierarchy found one, corresponding to it
      string currentEntityItemHash = "";
      string defaultEntityItemHash = "";
      string currentEntityDefaultItemHash = "";
      string defaultEntityDefaultItemHash = "";

      //Entity to begin the hierarchy navigation with...
      tmpEntity = (eObj != null ? eObj : this.Entity);
      entityBaseId = tmpEntity.Id;
      entityLevel = tmpEntity.HierarchyPosition;

      //BEGIN:Let us save the context for ALL this attributes
      itemTypeId = this.itemType.Id;
      itObjObj = new TItemType() { Id = itemTypeId, OrigId = this.itemType.OrigId, Entity = tmpEntity };
      iObjObj = new TItem() { Id = this.Id, OrigId = this.OrigId, itemHistory = this.itemHistory, Entity = tmpEntity, ItemType = itObjObj };
      //END:Let us save the context for ALL this attributes

      while (tmpEntity != null)
      {
        currentEntityItemHash = tmpEntity.Id + "." + itemTypeId  + "." + this.Id;
        defaultEntityItemHash = tmpEntity.Id + "." + itemTypeId + "." + Constants.DEFAULT_ITEM;
        currentEntityDefaultItemHash = Constants.DEFAULT_ENTITY + "." + itemTypeId + "." + this.Id;
        defaultEntityDefaultItemHash = Constants.DEFAULT_ENTITY + "." + itemTypeId + "." + Constants.DEFAULT_ITEM;

        if (tmpEntity.ItemIndex.Contains(currentEntityItemHash))
          idxItemEntry = tmpEntity.ItemIndex[currentEntityItemHash];
        else if (tmpEntity.ItemIndex.Contains(defaultEntityItemHash))
          idxItemEntry = tmpEntity.ItemIndex[defaultEntityItemHash];
        else if (tmpEntity.ItemIndex.Contains(currentEntityDefaultItemHash))
          idxItemEntry = tmpEntity.ItemIndex[currentEntityDefaultItemHash];
        else if (tmpEntity.ItemIndex.Contains(defaultEntityDefaultItemHash))
          idxItemEntry = tmpEntity.ItemIndex[defaultEntityDefaultItemHash];
        else
          idxItemEntry = null;

        if (idxItemEntry != null)
        {
          iObjIdx1 = (TItem)idxItemEntry.ItemIdx;
          iObjObj1 = (TItem)idxItemEntry.ItemObj;

          for (int i = 0; i < iObjObj1.AttributeIndex.List.Count; i++) 
          {
            tmpIdxAttrEntry = iObjObj1.AttributeIndex[i];

            aObjIdx = (TAttribute)tmpIdxAttrEntry.ItemIdx;
            aObjObj = (TAttribute)tmpIdxAttrEntry.ItemObj;

            //TODO: I must work with the CONTEXT where I am, not the attribute itself, so as to compare entity hierarchies correctly!
            if (tmpEntity.Id != entityBaseId)
              aObjObj.Flags = Constants.SetOn(aObjObj.Flags, EAttributeFlags.IsInherited);
            else
              aObjObj.Flags = Constants.SetOff(aObjObj.Flags, EAttributeFlags.IsInherited);

            if (processedAttributes.Contains(aObjIdx.Id))
            {
              if (aObjIdx.Value != null && (string)aObjIdx.Value != "")
              {
                //The attribute already exists from a previous entity in the inheritance chain, let us see if it comes from the Default entity
                aObjIdxPrv = (TAttribute)((TIndexItem)(processedAttributes[aObjIdx.Id]).ItemObj).ItemObj;
                aObjObjPrv = (TAttribute)((TIndexItem)(processedAttributes[aObjIdx.Id]).ItemObj).ItemIdx;

                if (Constants.AnyOn(aObjIdxPrv.Flags, EAttributeFlags.FromDefaultEntity) && !Constants.AnyOn(aObjIdx.Flags, EAttributeFlags.FromDefaultEntity))
                {
                  aObjObj.Item = iObjObj;
                  processedAttributes[aObjIdx.Id].ItemObj = new TIndexItem() { ItemObj = aObjObj, ItemIdx = aObjIdx };
                }
                else if (Constants.AnyOn(aObjIdxPrv.Flags, EAttributeFlags.FromDefaultItem) && !Constants.AnyOn(aObjIdx.Flags, EAttributeFlags.FromDefaultItem))
                {
                  aObjObj.Item = iObjObj;
                  processedAttributes[aObjIdx.Id].ItemObj = new TIndexItem() { ItemObj = aObjObj, ItemIdx = aObjIdx };
                }
                else if (!Constants.AnyOn(aObjIdxPrv.Flags, EAttributeFlags.FromDefaultEntity) && !Constants.AnyOn(aObjIdx.Flags, EAttributeFlags.FromDefaultEntity))
                {
                  if (aObjObj.Item.Entity.HierarchyPosition < aObjObjPrv.Item.Entity.HierarchyPosition)
                  {
                    aObjObj.Item = iObjObj;
                    processedAttributes[aObjIdx.Id].ItemObj = new TIndexItem() { ItemObj = aObjObj, ItemIdx = aObjIdx };
                  }
                  }
                }
              }
            else
            {
              aObjObj.Item = iObjObj;
              processedAttributes.Add(new TIndexItem(){ ItemObj = new TIndexItem() { ItemObj = aObjObj, ItemIdx = aObjIdx }, ItemIdx = aObjIdx });
            }
          }
        }

        tmpEntity = tmpEntity.EntityOwner;
      }

      attributeEnumerable = new ArrayList();
      for (int i = 0; i < processedAttributes.List.Count; i++)
        attributeEnumerable.Add((TIndexItem)processedAttributes[i].ItemObj);

      return attributeEnumerable;
    }

    public new string ToString()
    {
      string toString = "";

      toString = "Id: " + (OrigId == null ? "(empty)" : OrigId);

      return toString;
    }

    public EItemFlags Flags { get; set; }

  }

}
