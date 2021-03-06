using System;
using System.Collections;

namespace TAGBOSS.Common.Model
{
  [Serializable]
  public class TEntity
  {
    private TIndex itemTypeIndex = new TIndex();
    private TIndex itemIndex = new TIndex();

    private ArrayList conditionalList = new ArrayList();
    private TIndex conditionalIndex = new TIndex();

    public string Id { get; set; }
    public string OrigId { get; set; }
    public string EntityType { get; set; }
    public TEntity EntityOwner { get; set; }
    public string FEIN { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime LastModifiedDateTime { get; set; }
    public string AlternateID { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string AlternateName { get; set; }
    public string ShortName { get; set; }
    public string FullName { get; set; }
    public string LegalName { get; set; }
    public string Client { get; set; }
    public string OldEntity { get; set; }
    public int ItemCount { get; set; }
    public int HierarchyPosition { get; set; }

    public string Address1 { get; set; }
    public string Address2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }

    public bool IsDirty { get; set; }

    public DateTime EffectiveDate{ get; set; }
    public TItem[] Items { get; set; }

    public TIndex ItemTypeIndex 
    {
      get
      {
        //conditionalList.Sort();
        return itemTypeIndex;
      } 
    }

    public TIndex ItemIndex
    {
      get
      {
        return itemIndex;
      }
    }

    public TIndex ConditionalIndex
    {
      get { return conditionalIndex; }
    }

    public ArrayList ConditionalList
    {
      get { return conditionalList; }
    }

    public string EntityHash
    {
      get { return Id; }
    }

    /// <summary>
    /// This functions navigates through the itemType hierarchy
    /// until it finds an ItemType that corresponds to the Id being passed
    /// or until it gets to the top of the hierarchy, if it does not find anything
    /// it will return a null ItemType object.
    /// This is one of the navigations that depends on context, and we should review 
    /// through which branch will it navigate
    ///
    /// </summary>
    /// <param name="ItemTypeId"></param>
    /// <returns></returns>
    public TItemType getItemType(string ItemTypeId)
    {
      string entity = this.Id;
      string itemTypeID = ItemTypeId.ToLower();
      string itemTypeHash = "";

      TEntity tmpEntity = this;
      TItemType tmpItemType = null;

      while (tmpEntity != null)
      {
        itemTypeHash = tmpEntity.Id + "." + itemTypeID;

        if (tmpEntity.ItemTypeIndex.Contains(itemTypeHash))
        {
          tmpItemType = (TItemType)((TIndexItem)tmpEntity.ItemTypeIndex[itemTypeHash]).ItemIdx;
          break;
        }

        tmpEntity = tmpEntity.EntityOwner;
      }

      return tmpItemType;
    }

    public ArrayList getItemEnumerable()
    {
      return getItemEnumerable(null);
    }

    public ArrayList getItemEnumerable(Hashtable itemTypesRequested)
    {
      TIndex processedITItems = new TIndex();
      ArrayList itemEnumerable = null;

      TItem itemIdx1 = null;
      TItem itemObj1 = null;

      TItem itemIdx2 = null;
      TItem itemObj2 = null;

      TIndexItem idxItemEntry1 = null;
      TIndexItem idxItemEntry2 = null;

      string itemTypeItemHash = "";

      itemEnumerable = new ArrayList();

      TEntity tmpEntity = this;
      while (tmpEntity != null)
      {
        for (int i = 0; i < tmpEntity.itemIndex.List.Count; i++)
        {
          idxItemEntry1 = tmpEntity.itemIndex[i];
          itemIdx1 = (TItem)idxItemEntry1.ItemIdx;
          itemObj1 = (TItem)idxItemEntry1.ItemObj;
          itemObj2 = new TItem() { Id = itemObj1.Id, Entity = this, ItemType = itemObj1.ItemType };
         
          if (itemIdx1.Id != Constants.DEFAULT_ITEM && !(itemIdx1.Id.StartsWith(Constants.CONDITIONAL_ITEM)))
          {
            if (itemTypesRequested == null || itemTypesRequested.ContainsKey(itemIdx1.ItemType.Id))
            {
              if (!processedITItems.Contains(itemObj2.ItemHash))
              {
                processedITItems.Add(new TIndexItem() { ItemObj = idxItemEntry1, ItemIdx = itemObj2 });
              }
              else
              {
                itemIdx2 = (TItem)((TIndexItem)processedITItems[itemObj2.ItemHash].ItemObj).ItemIdx;
                if (itemIdx1.Entity.Id != Constants.DEFAULT_ENTITY
                    && (itemIdx2.Entity.Id == Constants.DEFAULT_ENTITY || itemIdx1.Entity.HierarchyPosition < itemIdx2.Entity.HierarchyPosition))
                {
                  processedITItems[itemObj2.ItemHash].ItemObj = idxItemEntry1;
                }
              }        
            }
          }
        }

        tmpEntity = tmpEntity.EntityOwner;
      }
      
      for (int i = 0; i < processedITItems.List.Count; i++) 
        itemEnumerable.Add((TIndexItem)processedITItems[i].ItemObj);

      return itemEnumerable;
    }

    public override string ToString()
    {
      string toString = "";

      toString = "Id: " + (OrigId == null ? "(empty)" : OrigId);
      toString += "; EntityType: " + (EntityType == null ? "(empty)" : EntityType);

      return toString;
    }
  }
}
