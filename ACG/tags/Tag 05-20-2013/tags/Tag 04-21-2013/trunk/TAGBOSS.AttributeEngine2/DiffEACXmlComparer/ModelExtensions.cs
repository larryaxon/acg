using System;

using System.Text;

using TAGBOSS.Common.Model;

namespace DiffEACXmlComparer
{
  public static class ModelExtensions
  {

    #region Compare routines overload routines

    public static CompareResult CompareResults(this Entity entity, Entity entityRight)
    {
      CompareResult compareResult = new CompareResult() { Object1 = entity, Object2 = entityRight };

      string compareResultsDetails = "";
      compareResultsDetails = CompareEntities((Entity)compareResult.Object1, (Entity)compareResult.Object2);
      
      if (compareResultsDetails != "")
      {
        compareResult.Result = ResultFlag.NOT_EQUAL;
        compareResult.ResultDetails = compareResultsDetails;
      }
      else
      {
        compareResult.Result = ResultFlag.EQUAL;
        compareResult.ResultDetails = "";
      }
      compareResult.ValueType = ValueType.ENTITY;

      return compareResult;
    }

    public static CompareResult CompareResults(this ItemType itemType, ItemType itemTypeRight)
    {
      CompareResult compareResult = new CompareResult() { Object1 = itemType, Object2 = itemTypeRight };

      string compareResultsDetails = "";
      compareResultsDetails = CompareItemTypes((ItemType)compareResult.Object1, (ItemType)compareResult.Object2);

      if (compareResultsDetails != "")
      {
        compareResult.Result = ResultFlag.NOT_EQUAL;
        compareResult.ResultDetails = compareResultsDetails;
      }
      else
      {
        compareResult.Result = ResultFlag.EQUAL;
        compareResult.ResultDetails = "";
      }
      compareResult.ValueType = ValueType.ITEMTYPE;

      return compareResult;
    }
    
    public static CompareResult CompareResults(this Item item, Item itemRight)
    {
      CompareResult compareResult = new CompareResult() { Object1 = item, Object2 = itemRight };

      string compareResultsDetails = "";
      compareResultsDetails = CompareItems((Item)compareResult.Object1, (Item)compareResult.Object2);

      if (compareResultsDetails != "")
      {
        compareResult.Result = ResultFlag.NOT_EQUAL;
        compareResult.ResultDetails = compareResultsDetails;
      }
      else
      {
        compareResult.Result = ResultFlag.EQUAL;
        compareResult.ResultDetails = "";
      }
      compareResult.ValueType = ValueType.ITEM;

      return compareResult;
    }
    
    public static CompareResult CompareResults(this TAGAttribute attribute, TAGAttribute attributeRight)
    {
      CompareResult compareResult = new CompareResult() { Object1 = attribute, Object2 = attributeRight };

      string compareResultsDetails = "";
      compareResultsDetails = CompareAttributes((TAGAttribute)compareResult.Object1, (TAGAttribute)compareResult.Object2);
      if (compareResultsDetails != "")
      {
        compareResult.Result = ResultFlag.NOT_EQUAL;
        compareResult.ResultDetails = compareResultsDetails;
      }
      else
      {
        compareResult.Result = ResultFlag.EQUAL;
        compareResult.ResultDetails = "";
      }
      compareResult.ValueType = ValueType.ATTRIBUTE;

      return compareResult;
    }

    public static CompareResult CompareResults(this ValueHistory valueHistory, ValueHistory valueHistoryRight)
    {
      CompareResult compareResult = new CompareResult() { Object1 = valueHistory, Object2 = valueHistoryRight };

      string compareResultsDetails = "";
      compareResultsDetails = CompareValueHistories((ValueHistory)compareResult.Object1, (ValueHistory)compareResult.Object2);

      if (compareResultsDetails != "")
      {
        compareResult.Result = ResultFlag.NOT_EQUAL;
        compareResult.ResultDetails = compareResultsDetails;
      }
      else
      {
        compareResult.Result = ResultFlag.EQUAL;
        compareResult.ResultDetails = "";
      }
      compareResult.ValueType = ValueType.VALUE_HISTORY;

      return compareResult;
    }

    #endregion Compare routines overload routines

    #region Compare routines, this will return the comparison results for any pair of objects

    private static string CompareEntities(Entity entityLeft, Entity entityRight)
    {
      StringBuilder resultDetail = new StringBuilder();
      string resultDetailString = "";

      if (entityLeft.ID != entityRight.ID)
      {
        resultDetail.Append("ID: ");
        resultDetail.Append(entityLeft.ID);
        resultDetail.Append("<>");
        resultDetail.Append(entityRight.ID + "::");
      }

      if (entityLeft.OriginalID != entityRight.OriginalID)
      {
        resultDetail.Append("OriginalID: ");
        resultDetail.Append(entityLeft.OriginalID);
        resultDetail.Append("<>");
        resultDetail.Append(entityRight.OriginalID + "::");
      }

      if (entityLeft.EntityType != entityRight.EntityType)
      {
        resultDetail.Append("EntityType: ");
        resultDetail.Append(entityLeft.EntityType);
        resultDetail.Append("<>");
        resultDetail.Append(entityRight.EntityType + "::");
      }

      resultDetailString = resultDetail.ToString();

      if (resultDetailString.Trim() != "")
        resultDetailString = "Entity[" + entityLeft.ID + "]" + resultDetailString;

      return resultDetailString;

    }

    private static string CompareItemTypes(ItemType itemTypeLeft, ItemType itemTypeRight)
    {
      StringBuilder resultDetail = new StringBuilder();
      string resultDetailString = "";

      if (itemTypeLeft.ID != itemTypeRight.ID)
      {
        resultDetail.Append("ID: ");
        resultDetail.Append(itemTypeLeft.ID);
        resultDetail.Append("<>");
        resultDetail.Append(itemTypeRight.ID + "::");
      }

      if (itemTypeLeft.OriginalID != itemTypeRight.OriginalID)
      {
        resultDetail.Append("OriginalID: ");
        resultDetail.Append(itemTypeLeft.OriginalID);
        resultDetail.Append("<>");
        resultDetail.Append(itemTypeRight.OriginalID + "::");
      }

      if (itemTypeLeft.EffectiveDate.ToShortDateString() != itemTypeRight.EffectiveDate.ToShortDateString())
      {
        resultDetail.Append("EffectiveDate: ");
        resultDetail.Append(itemTypeLeft.EffectiveDate.ToShortDateString());
        resultDetail.Append("<>");
        resultDetail.Append(itemTypeRight.EffectiveDate.ToShortDateString() + "::");
      }

      resultDetailString = resultDetail.ToString();

      if (resultDetailString.Trim() != "")
        resultDetailString = "ItemType[" + itemTypeLeft.ID + "]" + resultDetailString;

      return resultDetailString;
    }

    private static string CompareItems(Item itemLeft, Item itemRight)
    {
      StringBuilder resultDetail = new StringBuilder();
      string resultDetailString = "";

      if (itemLeft.ID != itemRight.ID)
      {
        resultDetail.Append("ID: ");
        resultDetail.Append(itemLeft.ID);
        resultDetail.Append("<>");
        resultDetail.Append(itemRight.ID + "::");
      }

      if (itemLeft.OriginalID != itemRight.OriginalID)
      {
        resultDetail.Append("OriginalID: ");
        resultDetail.Append(itemLeft.OriginalID);
        resultDetail.Append("<>");
        resultDetail.Append(itemRight.OriginalID + "::");
      }

      if (itemLeft.StartDate.ToShortDateString() != itemRight.StartDate.ToShortDateString())
      {
        resultDetail.Append("StartDate: ");
        resultDetail.Append(itemLeft.StartDate.ToShortDateString());
        resultDetail.Append("<>");
        resultDetail.Append(itemRight.StartDate.ToShortDateString() + "::");
      }

      if (itemLeft.EndDate.ToShortDateString() != itemRight.EndDate.ToShortDateString())
      {
        resultDetail.Append("EndDate: ");
        resultDetail.Append(itemLeft.EndDate.ToShortDateString());
        resultDetail.Append("<>");
        resultDetail.Append(itemRight.EndDate.ToShortDateString() + "::");
      }

      if (itemLeft.EffectiveDate.ToShortDateString() != itemRight.EffectiveDate.ToShortDateString())
      {
        resultDetail.Append("EffectiveDate: ");
        resultDetail.Append(itemLeft.EffectiveDate.ToShortDateString());
        resultDetail.Append("<>");
        resultDetail.Append(itemRight.EffectiveDate.ToShortDateString() + "::");
      }

      if (itemLeft.LastModifiedBy != itemRight.LastModifiedBy)
      {
        resultDetail.Append("LastModifiedBy: ");
        resultDetail.Append(itemLeft.LastModifiedBy);
        resultDetail.Append("<>");
        resultDetail.Append(itemRight.LastModifiedBy + "::");
      }

      if (itemLeft.LastModifiedDateTime.ToShortDateString() != itemRight.LastModifiedDateTime.ToShortDateString())
      {
        resultDetail.Append("LastModifiedDateTime: ");
        resultDetail.Append(itemLeft.LastModifiedDateTime.ToShortDateString());
        resultDetail.Append("<>");
        resultDetail.Append(itemRight.LastModifiedDateTime.ToShortDateString() + "::");
      }

      if (itemLeft.IsInherited != itemRight.IsInherited)
      {
        resultDetail.Append("IsInherited: ");
        resultDetail.Append(itemLeft.IsInherited.ToString());
        resultDetail.Append("<>");
        resultDetail.Append(itemRight.IsInherited.ToString() + "::");
      }

      if (itemLeft.Source != itemRight.Source)
      {
        resultDetail.Append("Source: ");
        resultDetail.Append(itemLeft.Source);
        resultDetail.Append("<>");
        resultDetail.Append(itemRight.Source + "::");
      }

      resultDetailString = resultDetail.ToString();

      if (resultDetailString.Trim() != "")
        resultDetailString = "Item[" + itemLeft.ID + "]" + resultDetailString;

      return resultDetailString;
    }

    private static string CompareItemHistories(ItemHistory itemHistoryLeft, ItemHistory itemHistoryRight)
    {
      StringBuilder resultDetail = new StringBuilder();
      string resultDetailString = "";

      if (itemHistoryLeft.ID != itemHistoryRight.ID)
      {
        resultDetail.Append("ID: ");
        resultDetail.Append(itemHistoryLeft.ID);
        resultDetail.Append("<>");
        resultDetail.Append(itemHistoryRight.ID + "::");
      }

      if (itemHistoryLeft.StartDate.ToShortDateString() != itemHistoryRight.StartDate.ToShortDateString())
      {
        resultDetail.Append("StartDate: ");
        resultDetail.Append(itemHistoryLeft.StartDate.ToShortDateString());
        resultDetail.Append("<>");
        resultDetail.Append(itemHistoryRight.StartDate.ToShortDateString() + "::");
      }

      if (itemHistoryLeft.EndDate.ToShortDateString() != itemHistoryRight.EndDate.ToShortDateString())
      {
        resultDetail.Append("EndDate: ");
        resultDetail.Append(itemHistoryLeft.EndDate.ToShortDateString());
        resultDetail.Append("<>");
        resultDetail.Append(itemHistoryRight.EndDate.ToShortDateString() + "::");
      }

      if (itemHistoryLeft.EndDate.ToShortDateString() != itemHistoryRight.LastModifiedDateTime.ToShortDateString())
      {
        resultDetail.Append("LastModifiedDateTime: ");
        resultDetail.Append(itemHistoryLeft.LastModifiedDateTime.ToShortDateString());
        resultDetail.Append("<>");
        resultDetail.Append(itemHistoryRight.LastModifiedDateTime.ToShortDateString() + "::");
      }

      if (itemHistoryLeft.LastModifiedBy != itemHistoryRight.LastModifiedBy)
      {
        resultDetail.Append("LastModifiedBy: ");
        resultDetail.Append(itemHistoryLeft.LastModifiedBy);
        resultDetail.Append("<>");
        resultDetail.Append(itemHistoryRight.LastModifiedBy + "::");
      }

      resultDetailString = resultDetail.ToString();

      if (resultDetailString.Trim() != "")
        resultDetailString = "ItemHistory[" + itemHistoryLeft.ID + "]" + resultDetailString;

      return resultDetailString;
    }

    private static string CompareAttributes(TAGAttribute attributeLeft, TAGAttribute attributeRight)
    {
      StringBuilder resultDetail = new StringBuilder();
      string resultDetailString = "";

      if (attributeLeft.ID != attributeRight.ID)
      {
        resultDetail.Append("ID: ");
        resultDetail.Append(attributeLeft.ID);
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.ID + "::");
      }

      if (attributeLeft.LastModifiedBy != attributeRight.LastModifiedBy)
      {
        resultDetail.Append("LastModifiedBy: ");
        resultDetail.Append(attributeLeft.LastModifiedBy);
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.LastModifiedBy + "::");
      }

      if (attributeLeft.LastModifiedDateTime.ToShortDateString() != attributeRight.LastModifiedDateTime.ToShortDateString())
      {
        resultDetail.Append("LastModifiedDateTime: ");
        resultDetail.Append(attributeLeft.LastModifiedDateTime.ToShortDateString());
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.LastModifiedDateTime.ToShortDateString() + "::");
      }

      if (attributeLeft.StartDate.ToShortDateString() != attributeRight.StartDate.ToShortDateString())
      {
        resultDetail.Append("StartDate: ");
        resultDetail.Append(attributeLeft.StartDate.ToShortDateString());
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.StartDate.ToShortDateString() + "::");
      }

      if (attributeLeft.EndDate.ToShortDateString() != attributeRight.EndDate.ToShortDateString())
      {
        resultDetail.Append("EndDate: ");
        resultDetail.Append(attributeLeft.EndDate.ToShortDateString());
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.EndDate.ToShortDateString() + "::");
      }

      if (attributeLeft.EffectiveDate.ToShortDateString() != attributeRight.EffectiveDate.ToShortDateString())
      {
        resultDetail.Append("EffectiveDate: ");
        resultDetail.Append(attributeLeft.EffectiveDate.ToShortDateString());
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.EffectiveDate.ToShortDateString() + "::");
      }

      if (attributeLeft.DataType != attributeRight.DataType)
      {
        resultDetail.Append("DataType: ");
        resultDetail.Append(attributeLeft.DataType);
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.DataType + "::");
      }

      if (attributeLeft.Description != attributeRight.Description)
      {
        resultDetail.Append("Description: ");
        resultDetail.Append(attributeLeft.Description);
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.Description + "::");
      }

      if (attributeLeft.OriginalID != attributeRight.OriginalID)
      {
        resultDetail.Append("OriginalID: ");
        resultDetail.Append(attributeLeft.OriginalID);
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.OriginalID + "::");
      }

      if (attributeLeft.HasHistory != attributeRight.HasHistory)
      {
        resultDetail.Append("HasHistory: ");
        resultDetail.Append(attributeLeft.HasHistory.ToString());
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.HasHistory.ToString() + "::");
      }

      if (attributeLeft.IsFunctionValue != attributeRight.IsFunctionValue)
      {
        resultDetail.Append("IsFunctionValue: ");
        resultDetail.Append(attributeLeft.IsFunctionValue.ToString());
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.IsFunctionValue.ToString() + "::");
      }

      if (attributeLeft.IsGenerated != attributeRight.IsGenerated)
      {
        resultDetail.Append("IsGenerated: ");
        resultDetail.Append(attributeLeft.IsGenerated.ToString());
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.IsGenerated.ToString() + "::");
      }

      if (attributeLeft.IsHistory != attributeRight.IsHistory)
      {
        resultDetail.Append("IsHistory: ");
        resultDetail.Append(attributeLeft.IsHistory.ToString());
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.IsHistory.ToString() + "::");
      }

      if (attributeLeft.IsIncluded != attributeRight.IsIncluded)
      {
        resultDetail.Append("IsIncluded: ");
        resultDetail.Append(attributeLeft.IsIncluded.ToString());
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.IsIncluded.ToString() + "::");
      }

      if (attributeLeft.IsInherited != attributeRight.IsInherited)
      {
        resultDetail.Append("IsInherited: ");
        resultDetail.Append(attributeLeft.IsInherited.ToString());
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.IsInherited.ToString() + "::");
      }

      if (attributeLeft.IsNull != attributeRight.IsNull)
      {
        resultDetail.Append("IsNull: ");
        resultDetail.Append(attributeLeft.IsNull.ToString());
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.IsNull.ToString() + "::");
      }

      if (attributeLeft.IsRefValue != attributeRight.IsRefValue)
      {
        resultDetail.Append("IsRefValue: ");
        resultDetail.Append(attributeLeft.IsRefValue.ToString());
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.IsRefValue.ToString() + "::");
      }

      if (attributeLeft.ValueType != attributeRight.ValueType)
      {
        resultDetail.Append("ValueType: ");
        resultDetail.Append(attributeLeft.ValueType);
        resultDetail.Append("<>");
        resultDetail.Append(attributeRight.ValueType + "::");
      }

      if (!attributeLeft.Value.Equals(attributeRight.Value))
      {
        if (attributeLeft.ValueType != "tableheader")
        {
          resultDetail.Append("Value: ");
          resultDetail.Append(attributeLeft.Value.ToString());
          resultDetail.Append("<>");
          resultDetail.Append(attributeRight.Value.ToString() + "::");
        }
        else
        {
          resultDetail.Append("Value: ");
          resultDetail.Append("tableheaderLeft");
          resultDetail.Append("<>");
          resultDetail.Append("tableheaderRight" + "::");
        }
      }

      resultDetailString = resultDetail.ToString();

      if (resultDetailString.Trim() != "")
        resultDetailString = "Attribute[" + attributeRight.ID + "]" + resultDetailString;

      return resultDetailString;
    }

    private static string CompareValueHistories(ValueHistory valueLeft, ValueHistory valueRight) 
    { 
      StringBuilder resultDetail = new StringBuilder();
      string resultDetailString = "";

      if (valueLeft.ID != valueRight.ID)
      {
        resultDetail.Append("ID: ");
        resultDetail.Append(valueLeft.ID);
        resultDetail.Append("<>");
        resultDetail.Append(valueRight.ID);
      }
      //if (valueLeft.LastModifiedBy != valueRight.LastModifiedBy)
      //{
      //  resultDetail.Append("LastModifiedBy: ");
      //  resultDetail.Append(valueLeft.LastModifiedBy);
      //  resultDetail.Append("<>");
      //  resultDetail.Append(valueRight.LastModifiedBy + "::");
      //}
      //if (valueLeft.LastModifiedDateTime.ToShortDateString() != valueRight.LastModifiedDateTime.ToShortDateString())
      //{
      //  resultDetail.Append("LastModifiedDateTime: ");
      //  resultDetail.Append(valueLeft.LastModifiedDateTime.ToShortDateString());
      //  resultDetail.Append("<>");
      //  resultDetail.Append(valueRight.LastModifiedDateTime.ToShortDateString() + "::");
      //}
      if (valueLeft.StartDate.ToShortDateString() != valueRight.StartDate.ToShortDateString())
      {
        resultDetail.Append("StartDate: ");
        resultDetail.Append(valueLeft.StartDate.ToShortDateString());
        resultDetail.Append("<>");
        resultDetail.Append(valueRight.StartDate.ToShortDateString() + "::");
      }
      if (valueLeft.EndDate.ToShortDateString() != valueRight.EndDate.ToShortDateString())
      {
        resultDetail.Append("EndDate: ");
        resultDetail.Append(valueLeft.EndDate.ToShortDateString());
        resultDetail.Append("<>");
        resultDetail.Append(valueRight.EndDate.ToShortDateString() + "::");
      }
      if (valueLeft.ValueType != valueRight.ValueType)
      {
        resultDetail.Append("ValueType: ");
        resultDetail.Append(valueLeft.ValueType);
        resultDetail.Append("<>");
        resultDetail.Append(valueRight.ValueType + "::");
      }
      if (!valueLeft.Value.Equals(valueRight.Value))
      {
        if (valueLeft.ValueType != "tableheader")
        {
          resultDetail.Append("Value: ");
          resultDetail.Append(valueLeft.Value.ToString());
          resultDetail.Append("<>");
          resultDetail.Append(valueRight.Value.ToString() + "::");
        }
        else 
        {
          resultDetail.Append("Value: ");
          resultDetail.Append("tableheaderLeft");
          resultDetail.Append("<>");
          resultDetail.Append("" + "::");
        }
      }

      resultDetailString = resultDetail.ToString();

      if (resultDetailString.Trim() != "") 
        resultDetailString = "ValueHistory[" + valueLeft.ID + "]"+ resultDetailString;

      return resultDetailString;
    }
    
    #endregion Compare routines, this will return the comparison results for any pair of objects
  }
}
