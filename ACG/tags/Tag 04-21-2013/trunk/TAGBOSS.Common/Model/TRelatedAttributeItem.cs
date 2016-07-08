using System;

namespace TAGBOSS.Common.Model
{
  public class TRelatedAttributeItem
  {
    public TAttribute AttributeIdx { get; set; }
    public TAttribute AttributeObj { get; set; }

    public string RelatedItemHash
    {
      get
      {
        return AttributeIdx.Item.ItemHash + "." + AttributeIdx.AttributeHash;
      }
    }
  }
}
