using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common.Model.Clr
{
  [Serializable]
  public class MergeDataPoolRow
  {
    #region member variables
    public string entity = "";
    public string itemType = "";
    public string itemKey = "";
    public string itemOrigId = "";
    public string attribute = "";
    public string value = "";
    public string type = "";
    #endregion

    /// <summary>
    /// Constructor creates and initializes a Row instance
    /// </summary>
    public MergeDataPoolRow(string entity, string itemType, string itemKey, string itemOrigId, string attribute, string value, string type)
    {
      this.entity = entity;
      this.itemType = itemType;
      this.itemKey = itemKey;
      this.itemOrigId = itemOrigId;
      this.attribute = attribute;
      this.value = value;
      this.type = type;
    }
  }
}
