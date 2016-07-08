using System;
using System.Text;

namespace DiffEACXmlComparer
{
  public enum ResultFlag { EQUAL, NOT_EQUAL, LEFT, RIGHT }
  public enum ValueType { ENTITY, ITEMTYPE, ITEM, ATTRIBUTE, VALUE_HISTORY}

  public class CompareResult
  {
    public object Object1 {get; set;}
    public object Object2 { get; set; }
    public ResultFlag Result { get; set; }
    public ValueType ValueType { get; set; }
    public string ResultDetails { get; set; }
  }
}
