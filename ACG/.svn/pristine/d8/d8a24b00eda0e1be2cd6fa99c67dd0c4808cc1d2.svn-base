using System;

namespace TAGBOSS.Common.Model
{
  public class TIndexItem
  {
    public Object ItemObj { get; set; }
    public Object ItemIdx { get; set; }
    public bool IsRelatedAttribute { get; set; }
    public bool IsConditionalItem { get; set; }
    
    public string ItemHash
    {
      get
      {
        Type t = ItemIdx.GetType();
        if (t == typeof(TAttribute))
        {
          if(IsRelatedAttribute)
            return ((TAttribute)ItemIdx).Item.ItemHash + "." + ((TAttribute)ItemIdx).AttributeHash;
          else
            return ((TAttribute)ItemIdx).AttributeHash;
        }
        else if (t == typeof(TItem))
        {
          return ((TItem)ItemIdx).ItemHash;
        }
        else if (t == typeof(TItemType))
        {
          return ((TItemType)ItemIdx).ItemTypeHash;
        }
        else if (t == typeof(TEntity))
        {
          return ((TEntity)ItemIdx).EntityHash;
        }
        else if (t == typeof(TIndexItem))
        {
          if (IsConditionalItem)
            return ((TItem)ItemObj).Id + "." + ((TItem)ItemObj).Entity.Id + "." + ((TItem)ItemObj).ItemType.Id + ":" + ((TItem)((TIndexItem)ItemIdx).ItemIdx).ItemHash;
          else
            throw new NotImplementedException("This case is not supported! This is NOT a conditional item");
        }

        throw new InvalidCastException("Item property is not of type TIndexItem, TEntity, TItemType, TItem  or TAttribute");
      }
    }
  }
}
