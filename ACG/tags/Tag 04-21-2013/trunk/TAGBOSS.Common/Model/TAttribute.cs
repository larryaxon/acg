using System;
using System.Collections;

//using TAGBOSS.Common;

namespace TAGBOSS.Common.Model
{
  [Serializable]
  public class TAttribute
  {
    int maxCalls = 10;

    private TValueHistory valueHistory;

    //TODO: Change this to a TIndex collection!
    private TIndex relatedAttributes = new TIndex();

    public TIndex RelatedAttributes
    {
      get { return relatedAttributes; }
    }

    public string AttributeHash
    {
      get { return Id; }
    }

    public string Id { get; set; }

    public string OrigId { get; set; }

    public string ValueType
    {
      get
      {
        if (ValueHistory != null)
        {
          if (ValueHistory.ValueType == Constants.REF_INHERIT)
          {
            if (RelatedAttributes.List.Count > 0)
            {
              return ((TAttribute)RelatedAttributes[0].ItemIdx).ValueType;
            }
          }
        }
        else
          return Constants.VALUE;
        
        return ValueHistory.ValueType;
      }
      set
      {
        if (ValueHistory != null)
        {
          switch(value)
          {
            case Constants.REF_INHERIT:
            case Constants.TABLEHEADER:
            case Constants.FUNCTION:
            case Constants.VALUE:
              ValueHistory.ValueType = value;
              break;
          }
        }
      }
    }

    //TODO 2011/01/10: this is still NOT synchronized with the ValueSolve method
    //We must just ask for a solved attribute, instead of 2 methods (ValueSolve and ValueTypeSolve)
    //So when we call the ValueSolve method, returns an attribute that includes the ValueType, too
    public string SolveValueType(TIndexItem attrObjIdx) 
    {
      TItem iObj = ((TAttribute)attrObjIdx.ItemObj).Item;
      TEntity eObj = iObj.Entity;
    
      TAttribute returnAttribute = null;
      string returnAttributeValueType = Constants.VALUE;

      if (ValueHistory != null)
      {
        TParameterProcessor parameterProcessor = new TParameterProcessor();

        switch (valueHistory.ValueType)
        {
          case Constants.REF_INHERIT:
            returnAttribute = this;
            returnAttribute = parameterProcessor.getReference((string)returnAttribute.Value, attrObjIdx);
            if (returnAttribute != null)
              returnAttributeValueType = returnAttribute.ValueType;

            break;
          default:
            returnAttributeValueType = this.ValueType;
            break;
        }
      }

      return returnAttributeValueType;

    }

    public TValueHistory ValueHistory 
    {
      get 
      {
        if (this.valueHistory == null)
        {
          if (Item != null && Item.Entity != null)
          {
            DateTime effectiveDate = Item.Entity.EffectiveDate;

            if (History != null)
            {
              foreach (TValueHistory vh in History)
              {
                if (vh.StartDate <= effectiveDate && effectiveDate <= vh.EndDate)
                {
                  this.valueHistory = vh;
                }
              }
            }
          }
        }

        return this.valueHistory;
      }
    }

    //TODO: When in RAW Mode we DO NOT create a new history value, but overwrite an existing one...
    public object Value
    {
      get
      {
        return getValue();
      }

      set
      {
        setValue(value);
      }
    }

    //TODO: When in RAW Mode we DO NOT create a new history value, but overwrite an existing one...
    /// <summary>
    /// Value2 method resolves value of the attribute JIT, based on the valueType of the Attribute
    /// 
    /// Notes as of December 17, 2010:
    /// Issues to be reviewed as to how this works in general, according to original design notes
    /// This is the value resolution for the Attribute, it works JIT to solve references and functions, 
    /// We are missing functions that call references, references that call functions, functions that call functions
    /// because the final value we need to return must always be a Value or a Tableheader
    /// 
    /// Implementation Note:
    /// This can be done through a while loop that continues until we bump into a tableheader or value attribute
    /// or until we do a maximun of 10 (or any imposed limit) deep call, where we stop and return the value we are
    /// at the moment, or a null value after rasing or logging an appropiate error
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public TAttribute getSolveAttribute(TIndexItem aObjIdxEntry)
    {
      TItem iObjObj = ((TAttribute)aObjIdxEntry.ItemObj).Item;
      TEntity eObj = iObjObj.Entity;

      TAttribute aObjResolved = null;

      if (ValueHistory != null)
      {
        TParameterProcessor parameterProcessor = new TParameterProcessor();

        switch (valueHistory.ValueType)
        {
          case Constants.REF_INHERIT:
            aObjResolved = this;
            aObjResolved = parameterProcessor.getReference((string)aObjResolved.Value, aObjIdxEntry);

            if (aObjResolved != null && aObjResolved.Value != null && aObjResolved.ValueType != Constants.REF_INHERIT && aObjResolved.ValueType != Constants.TABLEHEADER)
            {
              if (!Constants.AnyOn(aObjResolved.Flags, EAttributeFlags.IsAtAtEvaluated) && ((string)aObjResolved.Value).Contains(TAGFunctions.ATTR_2_AT_CHAR))
              {
                aObjResolved.Value = parameterProcessor.getSolvedParameter((string)aObjResolved.Value, eObj, iObjObj, this);
                aObjResolved.Flags = Constants.SetOn(aObjResolved.Flags, EAttributeFlags.IsAtAtEvaluated);
              }
            }

            break;

          //case Constants.FUNCTION:
          ////TODO: 2011 0112 1802: Function evaluation still failing! need to debug, testing refInherit evaluation
          //  returnAttrbuteFuncValue = parameterProcessor.getSolvedParameter((string)getValue(), eObj, iObj);

          //  if (!returnAttrbuteFuncValue.Contains("@@"))
          //    returnAttributeValue = tfunc.processFunction(returnAttrbuteFuncValue, this.Item, eObj);
          //  else
          //    returnAttributeValue = returnAttrbuteFuncValue;
          //  break;
          default:
            aObjResolved = new TAttribute() { Id = this.Id, OrigId = this.OrigId, Item = this.Item, ValueType = this.ValueType };
            aObjResolved.Value = parameterProcessor.getSolvedParameter((string)getValue(), eObj, iObjObj, this);
            break;
        }
      }

      return aObjResolved;
    }

    public object SolveValue(TItem iObj)
    {
      TIndexItem attrObjIdx = new TIndexItem() { ItemObj = new TAttribute(){ Id = this.Id, OrigId = this.OrigId, Flags = this.Flags, Item = iObj }, ItemIdx = this };
      return SolveValue(attrObjIdx);
    }

    //TODO: When in RAW Mode we DO NOT create a new history value, but overwrite an existing one...
    /// <summary>
    /// SolveValue method resolves value of the attribute JIT, based on the valueType of the Attribute
    /// 
    /// Notes as of December 17, 2010:
    /// Issues to be reviewed as to how this works in general, according to original design notes
    /// This is the value resolution for the Attribute, it works JIT to solve references and functions, 
    /// We are missing functions that call references, references that call functions, functions that call functions
    /// because the final value we need to return must always be a Value or a Tableheader
    /// 
    /// Implementation Note:
    /// This can be done through a while loop that continues until we bump into a tableheader or value attribute
    /// or until we do a maximun of 10 (or any imposed limit) deep call, where we stop and return the value we are
    /// at the moment, or a null value after rasing or logging an appropiate error
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public object SolveValue(TIndexItem attrObjIdx)
    {
      TItem iObj = ((TAttribute)attrObjIdx.ItemObj).Item;
      TEntity eObj = iObj.Entity;

      TAttribute returnAttribute = null;
      object returnAttributeValue = null;
      string returnAttrbuteFuncValue = "";

      if (ValueHistory != null)
      {
        TParameterProcessor parameterProcessor = new TParameterProcessor();

        switch (valueHistory.ValueType)
        {
          case Constants.REF_INHERIT:
            returnAttribute = this;
            returnAttribute = parameterProcessor.getReference((string)returnAttribute.Value, attrObjIdx);

            if (returnAttribute != null && returnAttribute.Value != null && returnAttribute.ValueType != Constants.REF_INHERIT)
            {
              returnAttributeValue = returnAttribute.Value;
              if (!Constants.AnyOn(returnAttribute.Flags, EAttributeFlags.IsAtAtEvaluated) && ((string)returnAttributeValue).Contains(TAGFunctions.ATTR_2_AT_CHAR))
              {
                returnAttributeValue = parameterProcessor.getSolvedParameter((string)returnAttribute.Value, eObj, iObj, this);
                returnAttribute.Flags = Constants.SetOn(returnAttribute.Flags, EAttributeFlags.IsAtAtEvaluated);
              }
            }

            break;
          default:
            returnAttributeValue = getValue();
            if (!Constants.AnyOn(this.Flags, EAttributeFlags.IsAtAtEvaluated) && returnAttributeValue != null && ((string)returnAttributeValue).Contains(TAGFunctions.ATTR_2_AT_CHAR))
              returnAttributeValue = parameterProcessor.getSolvedParameter((string)returnAttributeValue, eObj, iObj, this);

            break;
        }
      }

      return returnAttributeValue;
    }

    private object getValue()
    {
      if (ValueHistory == null)
        return null;
      else
        return ValueHistory.Value;
    }

    private void setValue(object value) 
    {
      /* lmv66 comment
       * TODO 2011 0616 1801:
       * We must review the date we use to split this, since this call is associated with a effectiveDate 
       * we should review is we must call this using the effectiveDate in place of the dateTime.Now that we are using
       * this is important to clarify, since the effectiveDate is the one that identifies when is this data structure valid!
       */
      if (ValueHistory != null)
      {
        string newValue = (value != null ? value.ToString().Trim() : string.Empty);

        if(valueHistory.Value == newValue) //Nothing to do! we are assigning the same value more than once!!
          return;

        TValueHistory vhNew = new TValueHistory();
        vhNew.StartDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, DateTime.Now.ToShortDateString());
        vhNew.EndDate = valueHistory.EndDate;
        vhNew.ValueType = valueHistory.ValueType;
        vhNew.Value = newValue;

        //Now we change the EndDate of the active valueHistory, since we are splitting it to include the new valueHistory!
        valueHistory.EndDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, DateTime.Now.AddDays(-1).ToShortDateString());

        int hstLength = History.GetLength(0);
        TValueHistory[] newHistory = new TValueHistory[hstLength + 1];
        Array.Copy(History, newHistory, hstLength);
        newHistory[hstLength] = vhNew;

        valueHistory = vhNew;
        History = newHistory;
      }
      else
      {
        //TODO: Create a totally new VH with this value! but need to find proper StartDate and EndDate for it...
      }
    }

    public TItem Item { get; set; }

    public TValueHistory[] History { get; set; }

    public EAttributeFlags Flags { get; set; }

    public int IncludeDepth { get; set; }

    public bool IsInherited(TEntity eObj) 
    {
      bool isInherited = false;

      if (this.ValueType == Constants.TABLEHEADER
        && eObj.ItemIndex.Contains(this.Item.ItemHash)
        && eObj.ItemIndex[this.Item.ItemHash].ItemObj != null
        && ((TItem)eObj.ItemIndex[this.Item.ItemHash].ItemObj).AttributeIndex.Contains(this.Id)
        && ((TAttribute)((TItem)eObj.ItemIndex[this.Item.ItemHash].ItemObj).AttributeIndex[this.Id].ItemIdx).relatedAttributes.List.Count > 0)
      {
        isInherited = true;
      }
      else 
      {
        isInherited = this.Item.IsInherited(eObj);
      }

      return isInherited;
    }

    public bool IsExpired(TEntity eObj)
    {
      bool isExpired = true;
      TEntity tmpEntity = (eObj != null ? eObj : this.Item.Entity);

      if (History.GetLength(0) > 0 && tmpEntity != null)
      {
        foreach (TValueHistory vh in History)
        {
          if (vh != null && vh.StartDate <= tmpEntity.EffectiveDate && tmpEntity.EffectiveDate <= vh.EndDate)
          {
            isExpired = false;
            break;
          }
        }
      }

      return isExpired;
    }

    public void overwriteValueHistory(object value, DateTime startDate) 
    {
      TValueHistory vh;
      for (int i = 0; i < History.GetLength(0); i++)
      {
        vh = History[i];
        if (vh.StartDate == startDate) 
        {
          vh.Value = value.ToString();
          break;
        }
      }
    }

    public object Clone() 
    {
      TAttribute attrObj = new TAttribute();
      attrObj.Id = this.Id;
      attrObj.OrigId = this.OrigId;
      attrObj.Flags = this.Flags;
      attrObj.ValueType = this.ValueType;
      attrObj.IncludeDepth = this.IncludeDepth;
      attrObj.Item = this.Item;

      //We CREATE a NEW history collection for this Attribute...
      if (this.History.GetLength(0) > 0)
      {
        attrObj.History = new TValueHistory[this.History.GetLength(0)];
        for (int i = 0; i < this.History.GetLength(0); i++) 
          if(this.History[i] != null)
            attrObj.History[i] = (TValueHistory)this.History[i].Clone();
      }

      //TODO! review this, because the CLONE attribute will POINT to the SAME attributes as the original one...
      if (this.RelatedAttributes.List.Count > 0)
      {
        foreach (TIndexItem trAttrItem in this.RelatedAttributes)
          if(trAttrItem != null)
            attrObj.RelatedAttributes.Add(new TIndexItem{ ItemObj = trAttrItem.ItemObj, ItemIdx= trAttrItem.ItemIdx, IsRelatedAttribute = true });
      }

      return attrObj;
    }

    public string ToString()
    {
      string toString = "";

      //TODO: We need to define the case when NON VALID data
      //We need to define reserved words to be used only inside the System
      toString = "Id: " + (Id == null ? "(empty)" : Id);
      toString += "; Flags: " + Flags;

      return toString;
    }

  }
}
