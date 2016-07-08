using System;

namespace TAGBOSS.Common.Model
{
  public class TConditionalItem
  {
    public TItem Conditional { get; set; }
    public TIndexItem IndexItemEntry { get; set; }

    public string ConditionalHash
    {
      get
      {
        return Conditional.Id + "." + Conditional.Entity.Id + "." + Conditional.ItemType.Id + ":" + ((TItem)IndexItemEntry.ItemIdx).ItemHash;
      }
    }
  }
}
