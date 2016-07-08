using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common
{
  /// <summary>
  /// This is the base class for both Field and TAGAttribute class. While this is a concrete class, it is meant to be inherited, not instantiated.
  /// </summary>
  [SerializableAttribute]
  public class FieldBase 
  {
    #region module data
    protected string description; // TODO: description really belongs on the Dictionary Entity 
    protected string id;
    protected string origID;
    protected string dataType;
    protected bool isDirty;
    protected object myvalue;
    protected object oldValue;
    protected bool isDeleted;
    protected bool isVirtual;
    protected bool isReadOnly;
    protected bool isVisible;
    protected bool isNull;

    //private Dictionaries Dictionary;

    #endregion module data

    #region constructors
    /// <summary>
    /// This constructor just initializes default values
    /// </summary>
    public FieldBase()
    {
      description = null;
      myvalue = null;
      oldValue = null;
      id = null;
      origID = null;
      dataType = null;                // default value for DataType is string. Note c# data types are used
      isDirty = false;
      isDeleted = false;
      isVirtual = false;
      isReadOnly = false;
      isVisible = true;
      isNull = true;
    }

    //public FieldBase(Dictionaries dict)
    //  : base()
    //{
    //  Dictionary = dict;
    //}

    //public FieldBase(string pId, object pValue, Dictionaries dict)
    //{
    //  id = pId.ToLower();
    //  origID = pId;
    //  myvalue = pValue;
    //  Dictionary = dict;
    //}
    /// <summary>
    /// This constructor initializes default values and the ID and Value of the field
    /// </summary>
    /// <param name="pId"></param>
    /// <param name="pValue"></param>
    public FieldBase(string pId, object pValue) : base()
    {
      id = pId.ToLower();
      origID = pId;
      myvalue = pValue;
    }
    #endregion constructors

    #region public properties

    public Dictionaries Dictionary
    {
      get { return DictionaryFactory.getInstance().getDictionary(); }
      //set { Dictionary = value; }
    }
    /// <summary>
    /// Returns or sets the Value of the field
    /// </summary>
    public object Value
    {
      get { return myvalue; }
      set
      {
        /*
         * per requirement #790, we should not set this value if the readonly flag is set
         */
        if (!ReadOnly)
        {
          /*
           * per requirement # 493, we need to trim all values. However, we can only do this
           * if they are not null and of type string
           */
          object val;
          
          if (value == System.DBNull.Value)
            val = null;
          else
          {
            if (value != null && value.GetType().Name.ToString().ToLower() == TAGFunctions.DATATYPESTRING)
              val = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, value)).Trim();    // right and left trim the string
            else
              val = value;        // the value is not a string, so it is what it is
          }
          
          isNull = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, val) == string.Empty); // isNull is set if val is null or empty

          if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.objectEquals, myvalue, val))    // if the new value is the same as the existing one
            return;                                       // then do nothing

          oldValue = myvalue;                             // otherwise, save the old value
          myvalue = val;                                  // set the new one
          isDirty = true;                                 // and set the dirty flag
        }
      }
    }
    /// <summary>
    /// Prior value to the current set value. This is only guaranteed to be correct if Dirty = true. This is read only.
    /// </summary>
    public object OldValue
    {
      get { return oldValue; }
      set { oldValue = value; }
    }
    /// <summary>
    /// Returns or sets the DataType of this field
    /// </summary>
    public string DataType
    {
      get 
      {
        if (dataType == null)
          if (Dictionary != null)
            dataType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, Dictionary.AttributeProperty(id, "DataType"));
          else
            dataType = TAGFunctions.DATATYPESTRING;
        return dataType; 
      }
      set
      {
        //isDirty = true;
        dataType = value;
      }
    }
    public string Mask
    {
      get
      {
        if (Dictionary == null)
          return string.Empty;
        else
          return (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, Dictionary.AttributeProperty(id, "Mask"));
      }
    }
    public bool Required
    {
      get
      {
        if (Dictionary == null)
          return false;
        else
          return (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, Dictionary.AttributeProperty(id, "RequiredYN"), false);
      }
    }
    /// <summary>
    /// Returns or sets the ID (unique field or column or attribute name) of this field
    /// </summary>
    public string ID
    {
      get { return id; }
      set
      {
        isDirty = true;
        string origid = value.Trim();
        id = origid.ToLower();
        origID = origid;
      }
    }
    /// <summary>
    /// Original ID (read only) with preserved case to save back to the database
    /// </summary>
    public string OriginalID
    {
      get { return origID; }
    }
    /// <summary>
    /// Returns or sets the Description of this field. If null or empty, get returns the ID.
    /// </summary>
    public string Description
    {
      get
      {
        if (description == null)
          if (Dictionary != null)
            description = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, Dictionary.AttributeProperty(id, "Description"));
          else
            description = origID;
        if (description == string.Empty)
          description = origID;
        return description;
      }
      set
      {
        //isDirty = true;
        description = value;
      }
    }
    /// <summary>
    /// Has this field been modified?
    /// </summary>
    public bool Dirty
    {
      get { return isDirty; }
      set { isDirty = value; }
    }
    /// <summary>
    /// Is this Field Deleted?
    /// </summary>
    public bool Deleted
    {
      get { return isDeleted; }
      set { isDeleted = value; isDirty = true; }
    }
    /// <summary>
    /// Is this a virtual/calculated field?
    /// </summary>
    public bool Virtual
    {
      get { return isVirtual; }
      set { isVirtual = value; }
    }
    /// <summary>
    /// is this field read only?
    /// </summary>
    public bool ReadOnly
    {
      get { return isReadOnly; }
      set { isReadOnly = value; }
    }
    /// <summary>
    /// Is this visible?
    /// </summary>
    public bool Visible
    {
      get { return isVisible; }
      set { isVisible = value; }
    }

    public bool IsNull
    {
      get { return isNull || myvalue == null || (myvalue.GetType() == typeof(string) && string.IsNullOrEmpty((string)myvalue)); }
      set { isNull = value; }
    }
    //public virtual object Clone() 
    //{ 
    //  throw new Exception("Must not execute this FieldBase.Clone() directly"); 
    //}
    #endregion public properties

    #region public methods

    public override string ToString()
    {
      return "{ " + origID + ": " + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, myvalue) + " }";
    }
    #endregion
  }
}
