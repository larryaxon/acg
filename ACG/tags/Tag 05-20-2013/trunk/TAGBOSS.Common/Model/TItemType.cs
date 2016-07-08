using System;
using System.Collections;

namespace TAGBOSS.Common.Model
{
  [Serializable]
  public class TItemType
  {
    private TIndex itemIndex = new TIndex();
    
    public TEntity Entity { get; set; }

    public string Id { get; set; }
    public string OrigId { get; set; }

    public TIndex ItemIndex
    {
      get { return itemIndex; }
    }

    public string ItemTypeHash
    {
      get
      {
        return Entity.Id + "." + Id;
      }
    }

    public TItem getItem(string ItemId)
    {
      string itemType = this.Id;

      string itemTypeHash = "";
      string itemHash = "";

      TEntity tmpEntity = this.Entity;
      TItem tmpItem = null;

      while (tmpEntity != null)
      {
        itemTypeHash = tmpEntity.Id + "." + this.Id;
        itemHash = itemTypeHash + "." + ItemId;

        if (tmpEntity.ItemTypeIndex.Contains(itemTypeHash))
        {
          if (((TItemType)((TIndexItem)tmpEntity.ItemTypeIndex[itemTypeHash]).ItemIdx).itemIndex.Contains(itemHash))
          {
            return tmpItem = (TItem)((TIndexItem)((TItemType)((TIndexItem)tmpEntity.ItemTypeIndex[itemTypeHash]).ItemIdx).itemIndex[itemHash]).ItemIdx;
          }
        }

        tmpEntity = tmpEntity.EntityOwner;
      }

      return null;
    }
  }
}
