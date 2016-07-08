using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common.Model.Clr
{
  [Serializable]
  public class EntityChildrenRow
  {
    #region member variables
    public string entity = "";
    public string entityType = "";
    public string entityOwner = "";
    public string legalName = "";
    public string firstName = "";
    public string middleName = "";
    public string alternateName = "";
    public string alternateID = "";
    public string fein = "";
    public DateTime startDate;
    public DateTime endDate;
    public string lastModifiedBy = "";
    public DateTime lastModifiedDateTime;
    public string itemType = "";
    public string item = "";
    public string attribute = "";
    public string value = "";
    public string type = "";
    #endregion

    /// <summary>
    /// Constructor creates and initializes a Row instance
    /// </summary>
    public EntityChildrenRow(
      string entity, 
      string entityType,
      string entityOwner,
      string legalName,
      string firstName,
      string middleName,
      string alternateName,
      string alternateID,
      string fein,
      DateTime startDate,
      DateTime endDate,
      string lastModifiedBy,
      DateTime lastModifiedDateTime,
      string itemType, 
      string item, 
      string attribute, 
      string value, 
      string type)
    {
      this.entity = entity;
      this.entityType = entityType;
      this.entityOwner = entityOwner;
      this.legalName = legalName;
      this.firstName = firstName;
      this.middleName = middleName;
      this.alternateName = alternateName;
      this.alternateID = alternateID;
      this.fein = fein;
      this.startDate = startDate;
      this.endDate = endDate;
      this.lastModifiedBy = lastModifiedBy;
      this.lastModifiedDateTime = lastModifiedDateTime;
      this.itemType = itemType;
      this.item = item;
      this.attribute = attribute;
      this.value = value;
      this.type = type;
    }
  }
}
