using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TAGBOSS.Common.Interface;

namespace TAGBOSS.Common.Model
{
  /// <summary>
  /// Instantiates an <attribute/> node in e_Attribute
  /// </summary>
  [SerializableAttribute]
  public class TAGAttribute : FieldBase, IDataClassItem
  {
    #region member data

    private bool isInherited = false;     // inherited from a level above in the hierarchy?
    private bool isFunctionValue = false;  // Created as the result of a func?
    private bool isRefValue = false;      // Created as the result of a ref or refInherit?
    private bool isIncluded = false;       // brought in via an Include=?
    private bool isHistory = false;       // for the transaction processor, flag this attribute as coming from the history call for the transaction
    private bool isGenerated = false;       // for the transaciont processor, flag this attribute as being generated
    private object wouldInheritValue = null;
    private ValueHistoryCollection values = new ValueHistoryCollection();
    bool valuesExist = true;       // is values instantiated?
    string valueType = "value";
    private string oldValueType = string.Empty;
    private string referenceValueSource = string.Empty;
    DateTime effectiveDate = DateTime.Now;
    DateTime lastModifiedDateTime = DateTime.Now;
    string lastModifiedBy = null;
    string status = null;
    bool overwriteValue = false;
    /* lmv66 2008 0519, Test for parent property setting...
     * This is a test for the parent property, I am putting the property this at the class definition level, 
     * but will be updated just at the parent level, when the "child" is created...
     */
    private Item parent = null;
    const string cCALCULATED = "Calculated";
    const string cPOSTED = "Posted";
    const string cCLOSED = "Closed";    
    const string cCREATED = "Created";
    const string cENTERED = "Entered";
    const string cAUTOPAY = "Autopay";

    #endregion member data

    #region constructors
    public TAGAttribute()
      : base()
    {
      values.ID = "vh";
    }

    public TAGAttribute(bool pOverwriteValue)
      : base()
    {
      values.ID = "vh";
      overwriteValue = pOverwriteValue;
    }
    //public TAGAttribute(Dictionaries dict)
    //  : base()
    //{
    //  values.ID = "vh";
    //  Dictionary = dict;
    //}
    #endregion constructors
    #region static constants
    public static string CREATED
    {
      get { return cCREATED; }
    }
    public static string ENTERED
    {
      get { return cENTERED; }
    }
    public static string AUTOPAY
    {
      get { return cAUTOPAY; }
    }
    public static string CALCULATED
    {
      get { return cCALCULATED; }
    }
    public static string POSTED
    {
      get { return cPOSTED; }
    }
    public static string CLOSED
    {
      get { return cCLOSED; }
    } 
    #endregion

    #region public properties

    public Item Parent
    {
      get { return parent; }
      set { parent = value; }
    }
    public byte Depth { get; set; }
    //public new Dictionaries Dictionary
    //{
    //  get { return base.Dictionary; }
    //  set
    //  {
    //    //if (values.Dictionary == null || value == null)
    //    //  values.Dictionary = null;
    //    //base.Dictionary = value;
    //  }
    //}
    //lmv66 2008 0519 End parent code...

    /// <summary>
    /// The effective date that was used to create this intance
    /// </summary>
    public DateTime EffectiveDate
    {
      get { return effectiveDate; }
      set { effectiveDate = value; }
    }

    /// <summary>
    /// Was this TAGAttribute inherited from a level higher up the e_Entity inheritance chain?
    /// </summary>
    public bool IsInherited
    {
      get { return isInherited; }
      set { isInherited = value; }
    }

    /// <summary>
    /// Was this TAGAttribute brought in by an Include= attribute?
    /// </summary>
    public bool IsIncluded
    {
      get { return isIncluded; }
      set { isIncluded = value; }
    }

    /// <summary>
    /// For the transaction engine. Did this attribute comes from the transaction history call?
    /// </summary>
    public bool IsHistory
    {
      get { return isHistory; }
      set { isHistory = value; }
    }

    /// <summary>
    /// For the transaction engine. Did this attribute is a generated attribute?
    /// </summary>
    public bool IsGenerated
    {
      get { return isGenerated; }
      set { isGenerated = value; }
    }
    
    /// <summary>
    /// Was this TAGAttribute calculated via a func= value type?
    /// </summary>
    public bool IsFunctionValue
    {
      get { return isFunctionValue; }
      set { isFunctionValue = value; }
    }
    
    /// <summary>
    /// Was this TAGAttribute value brought in by a ref or refInherit value type?
    /// </summary>
    public bool IsRefValue
    {
      get { return isRefValue; }
      set { isRefValue = value; }
    }
    
    /// <summary>
    /// If true, then the Value property does not create a new ValueHistory record, but overwrites 
    /// the value of the ValueHistory that matches the effective date
    /// </summary>
    public bool OverwriteValue
    {
      get { return overwriteValue; }
      set { overwriteValue = value; }
    }

    /// <summary>
    /// overload of base Dirty property that also resets OverwriteValue to false if
    /// the dirty flag is being unset.
    /// </summary>
    public new bool Dirty
    {
      get { return base.Dirty; }
      set
      {
        base.Dirty = value;
        if (!isDirty)
        {
          overwriteValue = false;
          values.Dirty = value;
        }        
      }
    }
    
    /// <summary>
    /// Does this record have <valuehistory/> history?
    /// </summary>
    public bool HasHistory
    {
      get { return valuesExist; }
      set { valuesExist = value; }
    }
    
    /// <summary>
    /// The value type of the the currently effective value range value
    /// </summary>
    public string ValueType
    {
      get { return valueType; }
      set 
      {
        oldValueType = valueType;
        if (value == null || value == string.Empty)
          valueType = "value";
        else
          valueType = value; 
      }
    }
    
    /// <summary>
    /// The old value for the value type. This is used to revert result for an invalid attributes
    /// </summary>
    public string OldValueType
    {
      get { return oldValueType; }
    }

    /// <summary>
    /// This is where we keep where the value came from (like func or ref or refinherit)
    /// </summary>
    public string ReferenceValueSource
    {
      get { return referenceValueSource; }
      set { referenceValueSource = value; }
    }

    /// <summary>
    /// Overload of Value property that contains logic to update the value in ValueHistory
    /// </summary>
    public new object Value
    {
      get { return base.Value; }
      set
      {
        object val = null;
        if (value != null)
        {
          string typeName = value.GetType().Name.ToLower();
          switch (typeName)
          {
            case TAGFunctions.TABLEHEADER:
              val = value;
              isNull = false;
              break;
            case TAGFunctions.DATATYPESTRING:
              string checkValueType = valueType.ToLower();
              if (
                  (checkValueType == TAGFunctions.TABLEHEADER || checkValueType == TAGFunctions.TABLEMOD) &&
                  (Dictionary != null && (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, Dictionary.AttributeProperty(id, TAGFunctions.TABLEHEADER)) != string.Empty)
                 )
              {
                val = new TableHeader(origID, valueType, (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, value), Dictionary, TAGFunctions.ThrowDataConversionException);
                valueType = TAGFunctions.TABLEHEADER;
                isNull = false;
              }
              else
              {
                val = value;
                if (val != string.Empty)
                  isNull = false;
                else
                  isNull = true;
              }
              break;
            case TAGFunctions.DATATYPEDATETIME:
              if ((DateTime)value == TAGFunctions.NULLDATETIME)
              {
                val = "";
                isNull = true;
              }
              else
              {
                val = value;
                isNull = false;
              }
              break;
            default:
              val = value;
              isNull = false;
              break;
          }
        }
        else
        {
          val = value;
          isNull = true;
        }
        /*
         * if this is coming in as a string, and we have a dictionary, and the value is not null or empty, and it is a "value" valuetype,
         * then we convert to the proper datatype
         */
        if (Dictionary != null && 
            !((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsNullOrStringEmpty, val)) && 
            valueType.Equals("value", StringComparison.CurrentCultureIgnoreCase) && 
            val.GetType() == typeof(string))    
        {
          val = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, val, DataType);
        }
        if (val != null && myvalue != null && 
          val.GetType() == typeof(TableHeader) && myvalue.GetType() == typeof(TableHeader))
        {
          if (((TableHeader)val).Equals((TableHeader)myvalue))
            return;
          else
            base.Value = val;
        }
        else
        {
          if (((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, val)).Equals(
            (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, myvalue), 
            StringComparison.CurrentCultureIgnoreCase))    // if the new value is the same as the existing one
            return;                                       // then don't override it
          else
            base.Value = val;
        }

        // TODO: Test if this is working!!: 2009 0730 2214
        if (isInherited && !overwriteValue)        // Clean of value history here if isInherited is set to true...
          values.Clear();
        
        bool addRecord = true;
        DateTime tmpEndDate = TAGFunctions.FutureDateTime;
        if (valuesExist)
        {
          /*
           * Now, we look through the value range for the conflicts over start/end date with the new current value. 
           * If we find one, We terminate this one (set the end date to one day prior to today), 
           * and add a new one. We set the end date of the new one = future date.
           * Then we check for future overlaps. If we find one, per Heather Smith, "the future one wins". So we set the end 
           * date of our new one to one day prior to the start date for the future one.
           * 
           * Note: to prevent duplication of history record upon initial load (we add the history record, then we set the
           * value, which adds another one, splitting the dates), if we find a match (history record matches effective
           * date and value is the same as the one passed in), then we decide nevermind, don't add the record or change
           * the value.
           */

          //marel: if the attribute is function, reference or inherited the history will be deleted
          //larry: this cannot happen here, because we expect this to work in non-gui contexts
          //if (this.IsFunctionValue || this.isRefValue || this.isInherited)
          //  this.Values.Clear();

          foreach (ValueHistory vh in values)
          {

            if (vh.StartDate <= effectiveDate)
            {
              if (effectiveDate <= vh.EndDate)            // this is the one that matches
              {
                if (vh.Value == val)                    // Are we not changing the value?
                {
                  addRecord = false;                      // then don't add a new history record
                  break;                                  // and exit our foreach loop
                }
                else
                {
                  if (overwriteValue)                     // this flag means we don't create a new value history
                    vh.Value = val;                     // so just update the value in the vh record
                  else
                    vh.EndDate = effectiveDate.AddDays(-1);// end date of current value history is set to one day before the effectivedate
                }
              }
            }
            else
              if (vh.StartDate > effectiveDate)            // does this history record start in the future? if so...
                if (tmpEndDate > vh.StartDate.AddDays(-1)) // set to min of current value of tmpEndDate or one date before the start date
                  tmpEndDate = vh.StartDate.AddDays(-1);   // end date of new value history is one day before the future value we found
          }
        }
        if (addRecord)                                     // Are we adding a new valuehistory?
        {
          // TODO: if the start date is the same as the old one, we need to split and add, setting the old one to deleted (end date 1 day less than start date)
          if (!overwriteValue)
            AddValue(val, valueType, effectiveDate, tmpEndDate, false);  // Add the new value history, but don't automatically update the value (which would cause recursion)
          this.Dirty = true;                                // and set the dirty flag
        }

      }
    }
    
    /// <summary>
    /// returns the index in the value history of the record that provided the current value of the attribute
    /// </summary>
    public int ValueHistoryIndex
    {
      get
      {
        int retValue = -1;
        if (HasHistory)
        {
          for (int i = 0; i < values.Count; i++)
          {
            if (values[i].StartDate <= effectiveDate && effectiveDate <= values[i].EndDate)
            {
              retValue = i;
              break;
            }
          }
        }
        return retValue;
      }
    }

    public object WouldInheritValue
    {
      get { return wouldInheritValue; }
      set { wouldInheritValue = value; }
    }

    /// <summary>
    /// The most recent status of this Attribute
    /// </summary>
    public string Status
    {
      get { return status; }
      set { status = value; }
    }
    
    /*
                // not implemented at this time
                public string InheritedFrom
                {
                    get { return inheritedFrom; }
                    set { inheritedFrom = value; }
                }
    */
    /// <summary>
    /// Yields Convert.ToDateTime(0) if no value history exists for this attribute, 
    /// else max(LastModifiedDateTime) of all of the ValueHistory items.
    /// Set sets an internal value, which is used by Valuehistory to set a new value
    /// </summary>
    public DateTime LastModifiedDateTime
    {
      get
      {
        if (values == null)
          return lastModifiedDateTime;    //  there is none if there is not a value history
        else
        {
          if (values.Count < 1)   // also returns null if values exists but is empty
            return lastModifiedDateTime;
          DateTime lastDateTime = TAGFunctions.PastDateTime;
          foreach (ValueHistory v in values)
          {
            if (v.LastModifiedDateTime > lastDateTime)
              lastDateTime = v.LastModifiedDateTime;
          }
          lastModifiedDateTime = lastDateTime;
          return lastDateTime;
        }
      }
      set { lastModifiedDateTime = value; }
    }
    public string LastModifiedBy
    {
      get { return lastModifiedBy; }
      set { lastModifiedBy = value; }
    }
    
    /// <summary>
    /// Used to reference the collection of ValueHistory
    /// </summary>
    public ValueHistoryCollection Values
    {
      get { return values; }
      set
      {
        values = value;
        Dirty = true;

        //lmv66: so we set the HasHistory in case this has a history...
        if (values.Count > 0)
          valuesExist = true;
      }
    }
    
    /// <summary>
    /// Returns the StartDate of the valuehistory that matches the EffectiveDate
    /// </summary>
    public DateTime StartDate
    {
      get
      {
        DateTime lastStartDate = TAGFunctions.PastDateTime;
        values.Sort();  // sort by startdate
        foreach (ValueHistory vh in values)
        {
          lastStartDate = vh.StartDate;
          if (vh.StartDate <= effectiveDate && effectiveDate <= vh.EndDate)   // if this valuehistory is a match
            return vh.StartDate;                                              // then return its startdate
        }
        return lastStartDate; // if no match, then return the start date from the most recent time slice
      }
      set
      {
        foreach (ValueHistory vh in values)
          if (vh.StartDate <= effectiveDate && effectiveDate <= vh.EndDate)   // if this valuehistory is a match
          {
            vh.StartDate = value;
            break;
          }
      }
    }

    public PickList PickList { get; set; }
    /// <summary>
    /// Returns the EndDate of the valuehistory that matches the EffectiveDate
    /// </summary>
    public DateTime EndDate
    {
      get
      {
        DateTime lastEndDate = TAGFunctions.FutureDateTime;
        values.Sort();  // sort by startdate
        foreach (ValueHistory vh in values)
        {
          lastEndDate = vh.EndDate;
          if (vh.StartDate <= effectiveDate && effectiveDate <= vh.EndDate)   // if this valuehistory is a match
            return vh.EndDate;                                                // then return its enddate
        }
        return lastEndDate; // if no match, then return the end date from the most recent time slice
      }
      set
      {
        foreach (ValueHistory vh in values)
          if (vh.StartDate <= effectiveDate && effectiveDate <= vh.EndDate)   // if this valuehistory is a match
          {
            vh.EndDate = value;
            break;
          }
      }
    }
    #endregion public properties

    #region public methods
    /// <summary>
    /// This is a special case, where we want to set the value as of a specific date, or
    /// we want to change the start date of the "current" (as of the effectiveDate) ValueHistory record
    /// </summary>
    /// <param name="newStartDate"></param>
    /// <param name="newValue"></param>
    /// <returns></returns>
    public void SetStartDateValue(DateTime newStartDate, object newValue)
    {
      if ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, newValue) != "" && newValue != Value) // is this a new value?
      {
        DateTime saveEffectiveDate = effectiveDate;
        effectiveDate = newStartDate;
        Value = newValue;
        effectiveDate = saveEffectiveDate;
      }
      else
      {
        int valIndex = ValueHistoryIndex;
        ValueHistory vh = values[valIndex];
        vh.StartDate = newStartDate;
        Dirty = true;
      }
    }
    /// <summary>
    /// sets the value of this Attribute using an explicit data type
    /// </summary>
    /// <param name="value"></param>
    /// <param name="dataType"></param>
    public void setValue(object value, string pDataType)
    {
 
      if (pDataType == null)
        if (dataType == null)
          pDataType = TAGFunctions.DATATYPESTRING;
        else
          pDataType = dataType;
      // If this is a string, and both old and new values are either null or empty, then don't change the value
      if (pDataType.ToLower() == TAGFunctions.DATATYPESTRING)
      {
        bool newValueIsEmpty = (value == null || value == string.Empty);
        bool oldValueIsEmpty = (Value == null || Value == string.Empty);
        if (newValueIsEmpty && oldValueIsEmpty)
          return;
      }
      if (value == null)    // don't try to convert if the value is null
        Value = value;
      else
      {
        if (pDataType == null || pDataType == string.Empty)
          pDataType = TAGFunctions.DATATYPESTRING;
        string compareDataType = pDataType.ToLower();    // convert data type to lower since everything is case insensitive
        if (myvalue != null && myvalue.GetType().Name.ToLower() != compareDataType)    // is the type of the current value already the same as pDataType?
          myvalue = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, myvalue, pDataType);        // no, so normalize it first, so the comparison inside of Value uses the correct type
        if (value.GetType().Name.ToLower() != compareDataType)      // is the incoming value the correct data type?
          Value = (object)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, value, pDataType);            // nope, so convert it as we set
        else
          Value = value;                                            // yep, so don't take the trouble to convert it before we set
      }
    }

    public void resetValue()
    {
      myvalue = null;
    }

    public static bool IsEntered(string status)
    {
      return Item.IsEntered(status);
    }
    
    /// <summary>
    /// overload of AddValue that defaults the valueType to "value"
    /// </summary>
    /// <param name="pValue"></param>
    /// <param name="pStartDate"></param>
    /// <param name="pEndDate"></param>
    public void AddValue(Object pValue, DateTime pStartDate, DateTime pEndDate)
    {
      AddValue(pValue, "value", pStartDate, pEndDate);
    }
    
    /// <summary>
    /// Add a value history node for this TAGAttribute
    /// </summary>
    /// <param name="pValue"></param>
    /// <param name="pValueType"></param>
    /// <param name="pStartDate"></param>
    /// <param name="pEndDate"></param>
    public void AddValue(Object pValue, string pValueType, DateTime pStartDate, DateTime pEndDate)
    {
      AddValue(pValue, pValueType, pStartDate, pEndDate, true);
    }
    
    /// <summary>
    /// Overload of AddValue that passed in the lastModified by and datetime instead of getting them from the attribute
    /// </summary>
    /// <param name="pValue"></param>
    /// <param name="pValueType"></param>
    /// <param name="pStartDate"></param>
    /// <param name="pEndDate"></param>
    /// <param name="pLastModifiedBy"></param>
    /// <param name="pLastModifiedDateTime"></param>
    public void AddValue(Object pValue, string pValueType, DateTime pStartDate, DateTime pEndDate,
      string pLastModifiedBy, DateTime pLastModifiedDateTime)
    {
      // TODO: need to consolidate the two overloads of AddValue to only have one copy of the logic
      AddValue(pValue, pValueType, pStartDate, pEndDate, true, pLastModifiedBy, pLastModifiedDateTime);
    }
    
    /// <summary>
    /// Overload that allows the calling routine to specify that the Value property in the TAGAttribute not be updated automatically.
    /// To do this, set pUpdateAttributeValue to false.
    /// </summary>
    /// <param name="pValue"></param>
    /// <param name="pValueType"></param>
    /// <param name="pStartDate"></param>
    /// <param name="pEndDate"></param>
    /// <param name="pUpdateAttributeValue"></param>
    public void AddValue(Object pValue, string pValueType, DateTime pStartDate, DateTime pEndDate, bool pUpdateAttributeValue)
    {
      if (!valuesExist)
      {
        values = new ValueHistoryCollection();
        valuesExist = true;
      }
      ValueHistory vh = new ValueHistory();
      vh.Value = pValue;
      if (pUpdateAttributeValue)            // only set the TAGAttribute Value if the flag is set (to prevent recursion)
        base.myvalue = pValue;              // if we are adding a new history record, then set the value of this TAGAttribute to the same
      vh.ValueType = pValueType;
      vh.StartDate = pStartDate;
      vh.EndDate = pEndDate;
      vh.LastModifiedBy = lastModifiedBy;
      vh.LastModifiedDateTime = DateTime.Now;
      values.Add(vh);
      Dirty = true;
    }
    
    /// <summary>
    /// Overload of AddValue that passed in the lastModified by and datetime instead of getting them from the attribute
    /// </summary>
    /// <param name="pValue"></param>
    /// <param name="pValueType"></param>
    /// <param name="pStartDate"></param>
    /// <param name="pEndDate"></param>
    /// <param name="pUpdateAttributeValue"></param>
    /// <param name="pLastModifiedBy"></param>
    /// <param name="pLastModifiedDateTime"></param>
    public void AddValue(Object pValue, string pValueType, DateTime pStartDate, DateTime pEndDate, bool pUpdateAttributeValue,
      string pLastModifiedBy, DateTime pLastModifiedDateTime)
    {
      if (!valuesExist)
      {
        values = new ValueHistoryCollection();
        valuesExist = true;
      }
      ValueHistory vh = new ValueHistory();
      vh.Value = pValue;
      if (pUpdateAttributeValue)            // only set the TAGAttribute Value if the flag is set (to prevent recursion)
        base.myvalue = pValue;              // if we are adding a new history record, then set the value of this TAGAttribute to the same
      vh.ValueType = pValueType;
      vh.StartDate = pStartDate;
      vh.EndDate = pEndDate;
      vh.LastModifiedBy = pLastModifiedBy;
      vh.LastModifiedDateTime = pLastModifiedDateTime;
      values.Add(vh);
      Dirty = true;
    }

    /// <summary>
    /// Creates a copy of the attribute into a different attribute object, returns the object attribute created...
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
      return CloneInternal(true);
    }

    /// <summary>
    /// Creates a copy of the attribute into a different attribute object, returns the object attribute created...
    /// </summary>
    /// <returns></returns>
    public object CloneInternal(bool deepCopy)
    {
      TAGAttribute a = new TAGAttribute();
      a.ID = origID;
      a.DataType = dataType;
      a.Deleted = isDeleted;
      a.Depth = Depth;
      a.Description = description;
      a.Dirty = isDirty;
      a.EffectiveDate = effectiveDate;
      a.HasHistory = valuesExist;
      a.IsFunctionValue = isFunctionValue;
      a.IsGenerated = isGenerated;
      a.IsIncluded = isIncluded;
      a.IsInherited = isInherited;
      a.IsHistory = isHistory;
      a.IsNull = isNull;
      a.IsRefValue = isRefValue;
      a.LastModifiedBy = lastModifiedBy;
      a.LastModifiedDateTime = lastModifiedDateTime;
      a.Parent = parent;
      a.ReadOnly = isReadOnly;
      a.ReferenceValueSource = referenceValueSource;
      a.Status = status;
      a.Virtual = isVirtual;
      a.Visible = isVisible;
      //BEGIN: ValueType must be set before Value, because it is used by Value to infer the Type of this Value
      a.ValueType = valueType;
      if (deepCopy)
      {
        if (myvalue != null && myvalue.GetType() == typeof(TableHeader))
          a.Value = ((TableHeader)myvalue).Clone();
        else
          a.Value = myvalue;

        a.values = (ValueHistoryCollection)values.Clone();
      }
      //END: ValueType must be set before Value, because it is used by Value to infer the Type of this Value

      return a;
    }

    public bool EqualVHC(TAGAttribute attrVHC2) 
    {
      bool areEqual = true;

      if (id == attrVHC2.ID  && values.Count == attrVHC2.values.Count)
        foreach (ValueHistory vh in attrVHC2.values)
        {
          string vh_ID = vh.ID;
          if(!(values.Contains(vh_ID)))
          {
            areEqual = false;
            break;
          }
          else if (vh.StartDate != values[vh_ID].StartDate || vh.EndDate != values[vh_ID].EndDate || vh.ValueType != values[vh_ID].ValueType)
          {
            areEqual = false;
            break;
          }
          else 
          {
            if (vh.ValueType.ToLower() == TAGFunctions.TABLEHEADER)
            {
              if (!((TableHeader)values[vh_ID].Value).Equals((TableHeader)vh.Value))
              {
                areEqual = false;
                break;
              }
            }
            else
            {
              if (!(values[vh_ID].Value.Equals(vh.Value)))
              {
                areEqual = false;
                break;
              }
            }
          }
        }
      else
        areEqual = false;

      return areEqual;
    }
    #endregion public methods
  }
}
