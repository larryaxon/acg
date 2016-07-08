using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common.Model.Clr
{
  [Serializable]
  public class AttributeHistoryRow
  {
    #region member variables
    public string entity = "";
    public string itemType = "";
    public string itemOrigId = "";
    public string attribute = "";
    public DateTime startDate = TAGFunctions.PastDateTime;
    public DateTime endDate = TAGFunctions.FutureDateTime;
    public string value = "";
    public string type = "";
    #endregion

    /// <summary>
    /// Constructor creates and initializes a Row instance
    /// </summary>
    public AttributeHistoryRow(string entity, string itemType, string itemOrigId, string attribute, DateTime startDate, DateTime endDate, string value, string type)
    {
      this.entity = entity;
      this.itemType = itemType;
      this.itemOrigId = itemOrigId;
      this.attribute = attribute;
      this.startDate= startDate;
      this.endDate = endDate;
      this.value = value;
      this.type = type;
    }

  }
}
