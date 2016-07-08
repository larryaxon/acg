using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using TAGBOSS.Common.Model;

//using TAGBOSS.Sys.Dal;
//using TAGBOSS.Sys.EntityAttribute;

namespace TAGBOSS.Common
{
  /// <summary>
  /// The Validation class is instantiated as a singleton at system initialization, and is used 
  /// by all routines to validate entry of an TAGAttribute value.
  /// </summary>
  [Serializable]
  public class Dictionaries
  {
    /*
     * This class uses several data structures in e_Attribute to store data dictionary and validation info
     * for ItemTypes, Items, and Attributes. These structures are:
     * 
     * Dictionary For Attributes:      A global list of attributes with validation "properties" for each one
     * 
     * Entity     ItemType    ITEM          TAGAttribute
     * Dictionary   TAGAttribute     {the attribute name}  {name of "property" of the attributes. Among these
     *                            will be:
     *                              Description (description of this attribute, to be used
     *                                in GUI)
     *                              List  {Value is the name of an attribute in the attributes local
     *                                ItemType/Item whose List= points to a list of valid 
     *                                values for validation and picklists
     *                              ItemList (value is the ItemType from which the validation 
     *                                list of items is constructed for this TAGAttribute
     *                              Mask  (display/edit mask of this attribute)
     *                              ValidationRule (simple stateless validation rule) This may be
     *                                implemented as the Item code snippet for this record
     *                              RequiredYN (True/false value which, if true, specifies
     *                                that this attribute is required entry when a new item 
     *                                that contains it is created
     *  Default   {thisItemType}  {ThisItem}        {this attribute}
     *                            This defines the list of attributes that are valid for "add attribute"
     *                            for this Item/ItemType. Note that, unlike all other e_Attribute attributes,
     *                            attributes in this list may NOT have any <valuerange/> nodes. This will
     *                            support the use of XMLAttr entries so that common lists of attributes
     *                            may be included in several itemtype/items
     */
    #region module data
    public const string ISVALIDFUNC = "OK";
    public const string PICKLIST = "PickList";
    public const string DATATYPE = "DataType";
    public const string MASK = "Mask";
    public const string DESCRIPTION = "Description";
    public const string REQUIREDYN = "RequiredYN";
    const string cDICTIONARY = "Dictionary";      // TAGAttribute Entity for dictionary entries
    //const string cDEFAULT = "Default";          // TAGAttribute Entity for valid lists of Items/Attributes
    const string cATTRIBUTE = "Attribute";        // TAGAttribute ItemType for Dictionary list of Attributes
    const string cITEMTYPE = "ItemType";        // ItemType ItemType for Dictionary list of ItemTypes
    const string cITEM = "Item";            // Item ItemType for Dictionary list of Items
    const string cENTITY = "Entity";
    const string cFUNCTION = "Function";
    const string cTRIGGER = "Trigger";
    const string cWORKFLOW = "WorkflowStepCode";
    const string className = "Dictionary";
    EntityAttributesCollection doDictionary = null;
    private bool useAttribute = true;
    string[] AtParameterList = new string[] { TAGFunctions.ATTR_AT_ALTERNATEID, 
                                              TAGFunctions.ATTR_AT_ALTERNATENAME,
                                              TAGFunctions.ATTR_AT_ATTRIBUTE,
                                              TAGFunctions.ATTR_AT_CLIENT,
                                              TAGFunctions.ATTR_AT_EFFECTIVE_DATE,
                                              TAGFunctions.ATTR_AT_ENTITY,
                                              TAGFunctions.ATTR_AT_ENTITYENDDATE,
                                              TAGFunctions.ATTR_AT_ENTITYOWNER,
                                              TAGFunctions.ATTR_AT_ENTITYSTARTDATE,
                                              TAGFunctions.ATTR_AT_ENTITYTYPE,
                                              TAGFunctions.ATTR_AT_FEIN,
                                              TAGFunctions.ATTR_AT_FIRSTNAME,
                                              TAGFunctions.ATTR_AT_FULLNAME,
                                              TAGFunctions.ATTR_AT_ITEM,
                                              TAGFunctions.ATTR_AT_ITEM_ENDDATE,
                                              TAGFunctions.ATTR_AT_ITEM_STARTDATE,
                                              TAGFunctions.ATTR_AT_ITEMTYPE,
                                              TAGFunctions.ATTR_AT_LEGALNAME,
                                              TAGFunctions.ATTR_AT_MIDDLENAME,
                                              TAGFunctions.ATTR_AT_SHORTNAME,
                                              TAGFunctions.ATTR_AT_SOURCEITEM};
    #endregion module data

    #region class definitions


    #endregion class definitions

    #region constructors
    /// <summary>
    /// Constructor for Dictionary. Retrieves all data in the Dictionary and Default Entities in e_Attribute, which contains
    /// both the list of all valid attributes for each ItemType/Item and the validation rules and datatypes
    /// for each attribute
    /// </summary>
    public Dictionaries(EntityAttributesCollection eacDictionary)
    {
      doDictionary = eacDictionary;
    }
    #endregion constructors

    #region public methods
    //public void populateAttributeDictionary()
    //{
    //  AttributeDB attributeDB = new AttributeDB();
      
    //  string authoredAttributeSQL = "Select * from Authored.dbo.AttributeAttribute";
    //  DataSet dsAuthoredAttributes = attributeDB.getDataFromSQL(authoredAttributeSQL);
    //  if (dsAuthoredAttributes == null || dsAuthoredAttributes.Tables.Count == 0 || dsAuthoredAttributes.Tables[0].Rows.Count == 0)
    //    return;
    //  EntityAttributesCollection eac = new EntityAttributesCollection();
    //  Entity e = new Entity();
    //  e.ID = cDICTIONARY;
    //  e.Dirty = false;
    //  ItemType it = new ItemType();
    //  it.ID = cATTRIBUTE;
    //  it.Dirty = false;
    //  DataColumnCollection cols = dsAuthoredAttributes.Tables[0].Columns;
    //  foreach (DataRow row in dsAuthoredAttributes.Tables[0].Rows)
    //  {
    //    Item i = new Item();
    //    foreach (DataColumn col in cols)
    //    {
    //      string fieldName = col.ColumnName.ToLower();
    //      switch (fieldName)
    //      {
    //        case "attribute":
    //          i.ID = row[fieldName].ToString();
    //          break;
    //        case "tempyn":
    //        case "discussion":
    //        case "incontexthelp":
    //        case "securityright":
    //        case "trigger":
    //        case "visibleyn":
    //          break;
    //        case "datatype":
    //          if (row[fieldName] != System.DBNull.Value)
    //            i.AddAttribute(col.ColumnName, row[fieldName]);
    //          else
    //            i.AddAttribute(col.ColumnName, "string");
    //          break;
    //        default:
    //          if (row[fieldName] != System.DBNull.Value)
    //            i.AddAttribute(col.ColumnName, row[fieldName]);
    //          break;
    //      }
    //    }
    //    if (!ValidAttribute(i.ID))  // only add this to the collection if it is not already in the dictionary
    //      it.Add(i);
    //  }
    //  e.ItemTypes.Add(it);
    //  eac.Entities.Add(e);
    //  int securityHandle = SecurityFactory.GetInstance().getSecurity("Dictionary").Handle;
    //  EntityAttributes entities = new EntityAttributes(securityHandle, true);
    //  entities.Save(eac);
    //}
    public bool ValidAttribute(string attributeName)
    {
      return (doDictionary.Entities.Contains(cDICTIONARY)  &&
              doDictionary.Entities[cDICTIONARY].ItemTypes.Contains(cATTRIBUTE) &&
              doDictionary.Entities[cDICTIONARY].ItemTypes[cATTRIBUTE].Items.Contains(attributeName)) ;
    }
    public bool ExistsAttribute(string attributeName)
    {
      return ValidAttribute(attributeName);
    }
    public bool ExistsAttributeProperty(string attributeName, string propertyName)
    {
      return ExistsDictionaryProperty(cATTRIBUTE, attributeName, propertyName);
    }
    public bool ExistsDictionaryProperty(string dictionaryType, string dictionaryItem, string propertyName)
    {
      return (doDictionary.Entities.Contains(cDICTIONARY) &&
              doDictionary.Entities[cDICTIONARY].ItemTypes.Contains(dictionaryType) &&
              doDictionary.Entities[cDICTIONARY].ItemTypes[dictionaryType].Items.Contains(dictionaryItem) &&
              doDictionary.Entities[cDICTIONARY].ItemTypes[dictionaryType].Items[dictionaryItem].Attributes.Contains(propertyName));
    }
    public ItemType AttributeProperties()
    {
      return DictionaryProperties(cATTRIBUTE);
    }
    public EntityAttributesCollection getDictionaryCollection(string dictionaryItemType)
    {
      EntityAttributesCollection eac = new EntityAttributesCollection();
      Entity e = new Entity();
      e.ID = cDICTIONARY;
      e.ItemTypes.Add(DictionaryProperties(dictionaryItemType));
      eac.Entities.Add(e);
      return eac;
    }
    public ItemType DictionaryProperties(string dictionaryItemType)
    {
      if (!doDictionary.Entities.Contains(cDICTIONARY) || !doDictionary.Entities[cDICTIONARY].ItemTypes.Contains(dictionaryItemType))
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "DictionaryProperties", "Dictionary does not contain an ItemType of '" + dictionaryItemType +"'");
        throw new Exception(t.ToString());
      }
      return (ItemType)doDictionary.Entities[cDICTIONARY].ItemTypes[dictionaryItemType];
    }
    ///// <summary>
    ///// Returns a list of Attributes that are valid for a given ItemType and Item
    ///// </summary>
    ///// <param name="itemType"></param>
    ///// <param name="item"></param>
    ///// <returns></returns>
    //public Item ValidAttributes(Entity e)
    //{
    //  Item attributeList = new Item();
    //  // if the entity does not have exactly one itemtype and item
    //  if (e.ItemTypes.Count != 1 || e.ItemTypes[0].Items.Count != 1)
    //    return attributeList;       // then return an empty collection
    //  string myEntity = e.OriginalID;
    //  string myItemType = e.ItemTypes[0].OriginalID;
    //  Item oldItem = e.ItemTypes[0].Items[0];
    //  string myItem = oldItem.ID;
    //  Item newItem;
    //  if (doDictionary.Entities != null
    //    && doDictionary.Entities.Contains(cDEFAULT)
    //    && doDictionary.Entities[cDEFAULT].ItemTypes != null
    //    && doDictionary.Entities[cDEFAULT].ItemTypes.Contains(myItemType)
    //    && doDictionary.Entities[cDEFAULT].ItemTypes[myItemType].Items.Contains(myItem))
    //  {
    //    newItem = doDictionary.Entities[cDEFAULT].ItemTypes[myItemType].Items[myItem];
    //    if (newItem != null)
    //      attributeList = (Item)newItem.Clone();  // make a copy of attributes in the dictionary for this item
    //  }
    //  // Now get the list of attributes for this entity/itemtype's default item
    //  AttributeProcessor myAttributes = new AttributeProcessor();
    //  EntityAttributesCollection defaultItemCollection =
    //    myAttributes.getAttributes(myEntity, myItemType, cDEFAULT,DateTime.Now);
    //  // If we got some, then merge them in with our list
    //  if (defaultItemCollection.Entities.Count > 0
    //  && defaultItemCollection.Entities[0].ItemTypes.Count > 0
    //  && defaultItemCollection.Entities[0].ItemTypes[0].Items.Count > 0)
    //  {
    //    newItem = defaultItemCollection.Entities[0].ItemTypes[0].Items[0];
    //    newItem.ID = oldItem.OriginalID;  // make the Default item id to be the same as our item id
    //    attributeList.Append(newItem);  // and merge the lists
    //  }
    //  // Now we subtract the entries in our incoming list from this resultset
    //  bool saveMarkForDelete = attributeList.MarkForDelete;   // save the markfordelete status
    //  foreach (TAGAttribute a in oldItem) // for each attribute in the old list
    //    if (attributeList.Attributes.Contains(a.ID))      // if the new list has it
    //      attributeList.Attributes.Remove(a.ID);        // then get rid of it in the new list
    //  attributeList.MarkForDelete = saveMarkForDelete;    // restore the status

    //  return attributeList;
    //}
    /// <summary>
    /// Returns an arraylist of Items that are valid for a given Item Type
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    public ArrayList ValidItems(string itemType)
    {
      ArrayList itemList = new ArrayList();
      if (doDictionary.Entities != null)
      {
        if (doDictionary.Entities[cDICTIONARY] != null && doDictionary.Entities[cDICTIONARY].ItemTypes.Contains(itemType))
        {
          ItemType it = doDictionary.Entities[cDICTIONARY].ItemTypes[itemType];
          foreach (Item i in it)
          {
            itemList.Add(i.ID);
          }
        }
      }
      return itemList;
    }
    /// <summary>
    /// Returns an ArrayList of all system valid ItemTypes
    /// </summary>
    /// <returns></returns>
    public ArrayList ValidItemTypes()
    {
      ArrayList itemTypeList = new ArrayList();
      if (doDictionary.Entities != null)
      {
        if (doDictionary.Entities[cDICTIONARY].ItemTypes != null)
        {
          foreach (ItemType it in doDictionary.Entities[cDICTIONARY].ItemTypes)
          {
            itemTypeList.Add(it.ID);
          }
        }
      }
      return itemTypeList;
    }
    /// <summary>
    /// takes a list of Attributes or Items or ItemTypes and returns an ArrayList of their descriptions in the same order. For uses
    /// like populating combo boxes and such
    /// </summary>
    /// <param name="attributeList"></param>
    /// <param name="dictionaryType"></param>
    /// <returns></returns>
    public ArrayList DescriptionList(ArrayList attributeList, string dictionaryType)
    {
      ArrayList descriptionList = new ArrayList();
      foreach (string attributeName in attributeList)
      {
        descriptionList.Add(DictionaryProperty(dictionaryType, attributeName, "Description"));
      }
      return descriptionList;
    }
    /// <summary>
    /// DataType method returns the data type for an TAGAttribute
    /// </summary>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public string DataType(string attribute)
    {
      try
      {
        return AttributeProperty(attribute, "DataType").ToString();
      }
      catch
      {
        return TAGFunctions.DATATYPESTRING;
      }
    }
    /// <summary>
    /// Looks up a property for an TAGAttribute in the dictionary and returns its value
    /// </summary>
    /// <param name="attribute"></param>
    /// <param name="property"></param>
    /// <returns>The Value object of the property or string.Empty if not found</returns>
    public object AttributeProperty(string attribute, string property)
    {
      return DictionaryProperty(cATTRIBUTE, attribute, property);
    }
    public object WorkflowProperty(string workflowStep, string property)
    {
      return DictionaryProperty(cWORKFLOW, workflowStep, property);
    }
    /// <summary>
    /// Looks up a property for an ItemType in the dictionary and returns its value
    /// </summary>
    /// <param name="itemType"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    public object ItemTypeProperty(string itemType, string property)
    {
      return DictionaryProperty(cITEMTYPE, itemType, property);
    }
    /// <summary>
    /// returns a list of properties for a dictionary item
    /// </summary>
    /// <param name="dictionaryType"></param>
    /// <param name="dictionaryItem"></param>
    /// <returns></returns>-*9
    public Item DictionaryProperties(string dictionaryType, string dictionaryItem)
    {
      try
      {
        if (doDictionary.Entities[cDICTIONARY].ItemTypes[dictionaryType].Items.Contains(dictionaryItem))
        {
          Item i = (Item)doDictionary.Entities[cDICTIONARY].ItemTypes[dictionaryType].Items[dictionaryItem].Clone();
          return i;
        }
        else
          return null;
      }
      catch
      {
        return null;
      }
    }
    /// <summary>
    /// returns an Item containing all of the properties (attributes) of a dictionary Item
    /// </summary>
    /// <param name="dictionaryType"></param>
    /// <param name="dictionaryItem"></param>
    /// <returns></returns>
    public Item DictionaryItem(string dictionaryType, string dictionaryItem)
    {
      try
      {
        if (doDictionary.Entities[cDICTIONARY].ItemTypes[dictionaryType].Items.Contains(dictionaryItem))
        {
          return doDictionary.Entities[cDICTIONARY].ItemTypes[dictionaryType].Items[dictionaryItem];
        }
        else
        {
          return null;
        }
      }
      catch
      {
        return null;
      }
    }
    /// <summary>
    /// Looks up a property for a specific dictionary object (e.g. attribute) of type dictionaryType and returns its value
    /// </summary>
    /// <param name="dictionaryType"></param>
    /// <param name="dictionaryItem"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    public object DictionaryProperty(string dictionaryType, string dictionaryItem, string property)
    {
      try
      {
        if (doDictionary.Entities[cDICTIONARY].ItemTypes[dictionaryType].Items.Contains(dictionaryItem))
          if (doDictionary.Entities[cDICTIONARY].ItemTypes[dictionaryType].Items[dictionaryItem].Attributes.Contains(property))
            return doDictionary.Entities[cDICTIONARY].ItemTypes[dictionaryType].Items[dictionaryItem].Attributes[property].Value;
        return string.Empty;
      }
      catch
      {
        return string.Empty;
      }
    }
    /// <summary>
    /// Pass in a entity object, get back a list of attributes/fields that don't pass validation.
    /// </summary>
    /// <param name="entity">Entity object containing ItemTypes/Items/Attributes and Fields</param>
    /// <returns>DataClass(T) compliant Collection of Entries that did not pass validation</returns>
    public InvalidEntries InvalidEntryList(Entity entity)
    {
      InvalidEntries invalidEntries = new InvalidEntries();
      foreach (ItemType it in entity.ItemTypes)
        foreach (Item i in it)
          foreach (TAGAttribute a in i)
          {
            string errorMessage;
            if (
                //a.Dirty && 
                !IsValid(i, a.ID, out errorMessage))
            {
              InvalidEntry invalidEntry = new InvalidEntry();
              invalidEntry.AttributeName = a.OriginalID;
              invalidEntry.ErrorMessage = errorMessage;
              invalidEntry.Context = entity.ID + "." + it.ID + "." + i.ID;
              invalidEntries.Add(invalidEntry);
            }
          }
      foreach (Field a in entity.Fields)
      {
        string errorMessage;
        if (!IsValid(entity.Fields, a.ID, out errorMessage))
        {
          InvalidEntry invalidEntry = new InvalidEntry();
          invalidEntry.AttributeName = a.OriginalID;
          invalidEntry.ErrorMessage = errorMessage;
          invalidEntry.Context = entity.ID + ".Fields";
          invalidEntries.Add(invalidEntry);
        }
      }
      return invalidEntries;
    }

    /// <summary>
    /// Do all of the fields in the Fields collection have values that pass validation?
    /// </summary>
    /// <param name="fields"></param>
    /// <param name="fieldName"></param>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public bool IsValid(FieldsCollection fields, string fieldName, out string errorMessage)
    {
      /*
       * This routine will access the dictionary, process the value using the PickList, Validation, and 
       * DataType dictionary attributes, and then return true if valid and false if invalid. If invalid,
       * pErrorMessage is filled with the error that has been generated.
       * 
       * To use, pass in the FieldsCollection object (which contains the local 
       * fields), and the name of the field that is being tested. 
       * 
       * Note: if the field does not exist in the FieldsCollection collection, we assume it is a null value,
       * and the validation always passes.
       * 
       * TODO: currently, if a picklist or Validation attribute exists, it will not be used, because
       * we have no mechanism to include these in the fields lists themselves (unlike attributes).
       * Suggestion: something in the inheritance chain in TAGAttribute table that acts as a collector for
       * fields validations, like an ItemType called Fields, Item = All. We would have to look that up
       * using Attributes object here, though. I wonder if the calling program could do that and then
       * pass it in?
       */
      DictionaryFunctions func = new DictionaryFunctions();
      bool isValid = true;
      bool warn = false;
      errorMessage = string.Empty;
      if (fields.Contains(fieldName))      // is this attribute in the collection?
      {
        if (!fields[fieldName].Visible)   // if this field is not visible, then don't validate it. Just assume isvalid = true;
          return isValid;
        object pValue = fields[fieldName].Value; // yes, pick up its value
        if (fields[fieldName].IsNull && !((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, AttributeProperty(fieldName, "RequireYN"))))
          return isValid;
        if (pValue.GetType().Name.ToLower() == TAGFunctions.DATATYPESTRING)     // if this is a string
          pValue = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, pValue.ToString());         // try to convert it to numeric or datetime
        string dataType = DataType(fieldName);       // get the data type
        /*
         * Check the data type
         */
        if (dataType != string.Empty)             // only perform the data type test if the data type exists
        {
          isValid = isValidDataType(dataType, pValue);
          if (!isValid)
            errorMessage = pValue.ToString() + " is not of data type " + dataType;
        }
        /*
         * Check the pick list (not currently implemented. This needs the @@Entity.Entity.@@EntityType item to work
         */
        //if (isValid)                      // only perform the next check if the last one passed
        //{
        //  string pickListName = AttributeProperty(fieldName, PICKLIST).ToString(); // get the picklist attribute name
        //  if (fields.Contains(pickListName))             // does the picklist attribute exist in this scope?
        //  {
        //    PickList pickList = (TableHeader)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CTableHeader, pickListName, item.Attributes[pickListName].Value, this).ToPickList();     // get the pick list string
        //    isValid = passesPickList(pickList, pValue);    // is in the the pick list?
        //    if (!isValid)
        //      errorMessage = pValue.ToString() + " is not in the list of valid values"; // nope, set the error message
        //  }
        //}
        /*
         * check the validation string
         */
        if (isValid)                      // only perform the next check if the last one passed
        {
          /*
           * now check the validation function
           */
          string validationFunction = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, AttributeProperty(fieldName, "Validation"));
          if (validationFunction != string.Empty)
          {
            string fChar = TAGFunctions.FUNCTIONCHAR.ToString();
            if (!validationFunction.StartsWith(fChar) && !validationFunction.StartsWith(TAGFunctions.ATTRIBUTECHAR))  // if it does not have a "_" or an "@"
              validationFunction = fChar + validationFunction;                            // then assume it is a function and add the function prefix
            Item item = fields.ToItem();
            try
            {
              errorMessage = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, func.evaluateExpression(validationFunction, item, 0));
            }
            catch (Exception ex)
            {
              if (TAGFunctions.BypassFunctionError)
                errorMessage = string.Format("Validation <{0}> failed with error: {1}", 
                  validationFunction, ((Exception)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getInnerException, ex)).Message);
              else
              {
                TAGExceptionMessage tException = new TAGExceptionMessage("IsValid", validationFunction,
                  "Error in LoadFunction: " + (Exception)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getInnerException, ex));
                throw new Exception(tException.ToString());
              }
            }
            if (errorMessage != string.Empty)
              isValid = false;
          }
        }
      }
      return isValid;
    }
   
    /// <summary>
    /// This routine checks the entered value (pValue) against the datatype and validation rules for 
    /// this Item/TAGAttribute and returns true if it passes validation, and false if no.
    /// If it does not pass validation, it also returns an error message for display to the user
    /// (e.g. "Must have a numeric value").
    /// </summary>
    /// <param name="item">Object of type Item which contains all of the local attributes</param>
    /// <param name="attributeName">Name of the TAGAttribute we want to check</param>
    /// <param name="errorMessage">Error message if validation failed</param>
    /// <returns>True if the value is valid, false if it is not</returns>
    public bool IsValid(Item item, string attributeName, out string errorMessage)
    {
      /*
       * This routine will access the dictionary, process the value using the PickList, Validation, and 
       * DataType dictionary attributes, and then return true if valid and false if invalid. If invalid,
       * pErrorMessage is filled with the error that has been generated.
       * 
       * To use, pass in the item object (which contains the item name and all of the local
       * attributes), and the name of the attribute that is being tested. 
       * 
       * Note: if the attribute does not exist in the item collection, we assume it is a null value,
       * and the validation always passes.
       */
      DictionaryFunctions func = new DictionaryFunctions();
      bool isValid = true;
      errorMessage = string.Empty;
      bool isRequired = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, AttributeProperty(attributeName,REQUIREDYN));
      if (item.Attributes.Contains(attributeName))      // is this attribute in the collection?
      {
        TAGAttribute a = item.Attributes[attributeName];
        if (!a.Visible)    // if this field is not visible, then don't validate it. Just assume isvalid = true;
          return isValid;
        object pValue = a.Value; // pick up its value
        if (pValue == null || pValue.ToString() == string.Empty)
          if (isRequired)
          {
            errorMessage = DictionaryProperty("Attribute", attributeName, "Description") + " is required"; // a required attribute was not set!
            return false;
          }
          else
            return true;
        if (!a.Dirty) // except for required fields, non-dirty fields are always valid
          return true;
        // the following is a block to localize the value of valueType:
        {
          //Pick list are NOT validated whatever GARBAGE comes in them!!
          string valueType = a.ValueType.ToLower();
          if (valueType == TAGFunctions.DATATYPETABLEHEADER || valueType == "tablenoheader")
            return true;
        }

        string dataType = DataType(attributeName);       // get the data type
        /*
         * Check the data type
         */
        if (dataType != string.Empty)             // only perform the data type test if the data type exists
        {
          isValid = isValidDataType(dataType, pValue);
          if (!isValid)
            errorMessage = pValue.ToString() + " is not of data type " + dataType;
        }        
        if (pValue.GetType() == typeof(string))     // if this is a string
          pValue = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, pValue, dataType);         // try to convert it to numeric or datetime

        /*
         * Check the pick list
         */
        if (isValid)                      // only perform the next check if the last one passed
        {
          string pickListName = AttributeProperty(attributeName, "PickList").ToString(); // get the picklist attribute name
          if (item.Attributes.Contains(pickListName) && !string.IsNullOrEmpty(pickListName))             // does the picklist attribute exist in this scope?
          {
            PickList pickList = ((TableHeader)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CTableHeader, pickListName, item.Attributes[pickListName].Value, this)).ToPickList();     // get the pick list string
            isValid = passesPickList(pickList, pValue);    // is in the the pick list?
            if (!isValid)
              errorMessage = pValue.ToString() + " is not in the list of valid values"; // nope, set the error message
          }
        }
        /*
         * check the validation string
         */
        if (isValid)                      // only perform the next check if the last one passed
        {
          /*
           * now check the validation function
           */
          string validationFunction = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, AttributeProperty(attributeName, "Validation"));
          if (validationFunction != string.Empty)
          {
            string fChar = TAGFunctions.FUNCTIONCHAR.ToString();
            if (!validationFunction.StartsWith(fChar) && !validationFunction.StartsWith(TAGFunctions.ATTRIBUTECHAR))  // if it does not have a "_" or an "@"
              validationFunction = fChar + validationFunction;                            // then assume it is a function and add the function prefix
            try
            {
              errorMessage = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, func.evaluateExpression(validationFunction, item, 0));
            }
            catch (Exception ex)
            {
              if (TAGFunctions.BypassFunctionError)
                errorMessage = string.Format("Validation <{0}> failed with error: {1}", 
                  validationFunction, ((Exception)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getInnerException, ex)).Message);
              else
              {
                TAGExceptionMessage tException = new TAGExceptionMessage("IsValid", validationFunction,
                  "Error in LoadFunction: " + (Exception)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getInnerException, ex));
                throw new Exception(tException.ToString());
              }
            } 
            if (errorMessage != string.Empty)
              isValid = false;
          }
        }
      }
      return isValid;
    }
    public bool IsValid(string attributeName, object Value, PickList picklist, string valueType, out bool warn,
      out string errorMessage)
    {
      return IsValid(
        Value, picklist, valueType, 
        (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString,AttributeProperty(attributeName, DATATYPE)),
        (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, AttributeProperty(attributeName, "ValidationString")), 
        out warn, out errorMessage);
    }
    public bool IsValid(object Value, PickList picklist, string valueType, string dataType, string validationString,
      out bool warn, out string errorMessage)
    {
      bool isValid = isValidDataType(dataType, Value);          // check data type
      if (isValid)
      {

        isValid = passesPickList(picklist,  Value); // check picklist
        //isValid = passesPickList(picklist, TAGFunctions.TABLEHEADER, dataType + "~string", Value); // check picklist

        if (isValid)
          isValid = passesValidation(validationString, valueType, Value, out errorMessage, out warn); // check validation string
        else
        {
          warn = false;
          errorMessage = string.Format("Value({0}) does not match an item in the pick list", TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, Value));
        }
      }
      else
      {
        warn = false;
        errorMessage = "Does not match data type";
      }

      return isValid;
    }
    ///// <summary>
    ///// This routine checks the entered value (pValue) against the datatype and validation rules for 
    ///// this Item/TAGAttribute and returns true if it passes validation, and false if no.
    ///// If it does not pass validation, it also returns an error message for display to the user
    ///// (e.g. "Must have a numeric value").
    ///// </summary>
    ///// <param name="Value"></param>
    ///// <param name="picklist"></param>
    ///// <param name="valueType"></param>
    ///// <param name="dataType"></param>
    ///// <param name="validationString"></param>
    ///// <param name="warn"></param>
    ///// <param name="errorMessage"></param>
    ///// <returns></returns>
    //public bool IsValid(object Value, string picklist, 
    //  string valueType, string dataType, string validationString,
    //  out bool warn, out string errorMessage)
    //{
    //  bool isValid = isValidDataType(dataType, Value);          // check data type
    //  if (isValid)
    //  {
    //    string[] dataTypeList = new string[] { dataType, "string" };
    //    PickList list = new PickList(picklist, dataTypeList);
    //    isValid = passesPickList(list, Value);
    //    if (isValid)
    //      isValid = passesValidation(validationString, valueType, Value, out errorMessage, out warn); // check validation string
    //    else
    //    {
    //      warn = false;
    //      errorMessage = "Does not match match an item in the pick list";
    //    }      
    //  }
    //  else
    //  {
    //    warn = false;
    //    errorMessage = "Does not match data type";
    //  }

    //  return isValid;
    //}
    /// <summary>
    /// Parses a raw mode function, and returns a comma-delimited list of errors that are found, or "OK" if no errors were found
    /// </summary>
    /// <param name="functionSpec">Function specification, like _divide(@divisor, @dividend)</param>
    /// <param name="attributeName">Optional name of the attribute that has this func=, used to validate the return data type</param>
    /// <param name="returnType">Or, if this is specified, uses it for a return data type</param>
    /// <returns></returns>
    public string IsValidFunction(string functionSpec, string attributeName)
    {
      List<string> msgList = new List<string>();
      string[] ConditionDelimiters = new string[] { "==", "<", ">", "<=", ">=", "!=" };
      string strFunction = functionSpec;
      if (!strFunction.StartsWith(TAGFunctions.FUNCTIONCHAR.ToString()))
        functionSpec = TAGFunctions.FUNCTIONCHAR.ToString() + functionSpec; // make sure it is in function format
      string attrDataType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, AttributeProperty(attributeName, "DataType")); // get the data type of the attribute that has the func
      msgList = checkToken(functionSpec, attrDataType, msgList); 
      if (msgList.Count == 0)   // if no errors were found
        msgList.Add(ISVALIDFUNC); // then we just have one message: Success
      return (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.ToList, (object)msgList.ToArray());  // return a comma delimted list
    }
    /// <summary>
    /// This is the private parser used by IsValidFunction. It takes a token of any type, and validates it. If an error is found, it adds it to the lost
    /// </summary>
    /// <param name="token">Token to be evaluated</param>
    /// <param name="returnType">Data type to compare the resulting expression with. Does it match?</param>
    /// <param name="msgList">List of errors</param>
    /// <returns>Same list of errors we passed in, plus any additions we made</returns>
    private List<string> checkToken(string token, string returnType, List<string> msgList)
    {
      string[] ConditionDelimiters = new string[] { "==", "<", ">", "<=", ">=", "!=" };
      const string LEFTPARENS = "(";
      const string RIGHTPARENS = ")";
      const string FUNC = "_";
      string[] tokenList = null;
      string functionName = string.Empty;
      string insideExpression;
      string attrDataType;
      string funcDataType;

      if (token == null && token.Length < 3)    // nothing to parse or check
        return msgList;                         // so just return the list unchanged

      string strExpression = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, token));
      
      if (strExpression.Length == 0)    // empty parameters pass. We don't have a mechanism for "Required" parameters yet
        return msgList;
      switch (strExpression.Substring(0,1))
      {
        case TAGFunctions.ATTRIBUTECHAR:    // this is an @Attribute
          string attrName = strExpression.Substring(1);
          if (!ExistsAttribute(attrName))
            msgList.Add(string.Format("{0}: Attribute Name does not exist", strExpression));
          else
          {
            if (returnType != null) // somtimes we don't know what the return type is, so we had null passed in. If so, we just ignore the datatype test
            {
              attrDataType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, AttributeProperty(attrName, "DataType"));
              if (!isCompatibleDataType(attrDataType, returnType))
                msgList.Add(string.Format("{0}: Attribute data type {1} must be the same as the function parameter data type {2}",
                  attrName, attrDataType, returnType));
            }
          }
          break;
        case LEFTPARENS:   // this is a list (token1~token2~...)
          if (strExpression.StartsWith(LEFTPARENS) && strExpression.EndsWith(RIGHTPARENS))
          {
            string[] parmParts = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, strExpression.Substring(1, strExpression.Length - 2));
            foreach (string parmPart in parmParts)
              msgList = checkToken(parmPart, null, msgList);
          }
          else
            msgList.Add(string.Format("{0} has mis-matched parentheses", strExpression));
          break;
        case FUNC:
          //TODO: OUT Parameters: We need to review this!
          //functionName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getFunctionName, strExpression, out insideExpression);  // look for an @function
          functionName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getFunctionName, strExpression, out insideExpression);  // look for an @function
          
          if (functionName == string.Empty)   // this expression does not start with a function or an attribute character
            msgList.Add(string.Format("{0}: invalid function name: {1}", strExpression, functionName));    // exit the loop
          else
          {
            Item myFunction = (Item)doDictionary.getValue(string.Format("Dictionary.Function.{0}", functionName));
            if (!myFunction.Attributes.Contains("Parameters"))
              msgList.Add(string.Format("{0}({1}): Function Name is not registered", functionName, insideExpression));
            else
            {
              TableHeader parameters = (TableHeader)myFunction.Attributes.getValue("Parameters");
              tokenList = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, insideExpression); // there is an @ function, so parse the string inside the "()" for tokens
              int nbrParms = parameters.Count - 1;  // parameters also include the return type, so we adjust for that
              if (nbrParms != tokenList.GetLength(0)) // do the number of parameters match?
                msgList.Add(string.Format("{0}({1}): number of parameters should be {2}", functionName, insideExpression, nbrParms.ToString()));
              else
              {
                /*
                 * let's check each parameter.
                 * 
                 * if it is an @Attribute, look that up in the dictionary to check datatype against datatype of the parameter.
                 * 
                 * If it is a function, then call this routine recursively to see if THAT function is properly formatted.
                 * 
                 * Otherwise, we assume all is well
                 */
                for (int iParm = 0; iParm < tokenList.GetLength(0); iParm++)
                {
                  funcDataType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, parameters[iParm, "DataType"]); // datatype is positional in the parameter list
                  if (functionName.ToLower() == "iif") // check for special parm, condition in an iif statement
                  {
                    if (iParm == 0)  // this is the condition
                    {
                      string strCondition = tokenList[iParm];
                      bool hasCondition = false;
                      foreach (string condition in ConditionDelimiters)
                      {
                        if (strExpression.Contains(condition))
                        {
                          hasCondition = true;
                          string[] conditionParts = new string[2];
                          int condLoc = strCondition.IndexOf(condition);
                          int condLen = condition.Length;
                          conditionParts[0] = strCondition.Substring(0, condLoc);
                          conditionParts[1] = strCondition.Substring(condLoc + condLen);
                          foreach (string conditionPart in conditionParts)
                            msgList = checkToken(conditionPart, null, msgList);
                        }
                        break;
                      }
                    }
                    else
                      msgList = checkToken(tokenList[iParm], funcDataType, msgList);
                  }
                  else
                    msgList = checkToken(tokenList[iParm], funcDataType, msgList);
                }
                // now check for the function return type
                if (returnType != null)   // sometimes we don't know what data type to compare with. so we don't try
                {
                  for (int i = 0; i < parameters.Count; i++)
                  {
                    if (((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, parameters[i, "Parameter"])).Equals("ReturnType", StringComparison.CurrentCultureIgnoreCase))
                    {
                      // first get the data type and name of the attribute that will receive the function value
                      funcDataType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, parameters[i, "DataType"]);
                      // now compare that with returnvalue data type
                      if (!isCompatibleDataType(funcDataType, returnType))
                        msgList.Add(string.Format("Function: {0}: Return data type {1} must be the same as receiving data type {2}", functionName, funcDataType, returnType));
                      break;
                    }
                  }
                }
              }
            }
          }
          break;
      }
      return msgList;
    }
    private bool isCompatibleDataType(string datatype1, string datatype2)
    {
      if (datatype1.Equals(datatype2, StringComparison.CurrentCultureIgnoreCase)) // good if they are equal
        return true;
      if (datatype1.Equals("object", StringComparison.CurrentCultureIgnoreCase) ||  // or if either one is object
          datatype2.Equals("object", StringComparison.CurrentCultureIgnoreCase))
        return true;
      if (isNumericDataType(datatype1) && isNumericDataType(datatype2))   // or if both are numeric
        return true;
      return false;     // otherwise, they are not compatible
    }
    private bool isNumericDataType(string datatype)
    {
      return ( datatype.Equals("int", StringComparison.CurrentCultureIgnoreCase) ||
               datatype.Equals("decimal", StringComparison.CurrentCultureIgnoreCase) ||
               datatype.Equals("long", StringComparison.CurrentCultureIgnoreCase) ||
               datatype.Equals("money", StringComparison.CurrentCultureIgnoreCase) );
    }
    /// <summary>
    /// Returns a complete list of valid ItemTypes from the Dictionary
    /// </summary>
    /// <returns>Entity object with ID of "None" and a collection of empty 
    /// ItemTypes with only their IDs as the list</returns>
    public Entity ItemTypes()
    {
      Entity e = new Entity();
      e.ID = "None";
      ItemType itemTypeList = null;
      if (doDictionary.Entities.Contains(cDICTIONARY))
      {
        if (doDictionary.Entities[cDICTIONARY].ItemTypes.Contains(cITEMTYPE))
        {
          itemTypeList = doDictionary.Entities[cDICTIONARY].ItemTypes[cITEMTYPE];
          ItemType it = new ItemType();
          foreach (Item i in itemTypeList)
          {
            ItemType itNew = new ItemType();
            itNew.ID = i.ID;
            e.ItemTypes.Add(itNew);
          }
        }
      }
      return e;
    }
    /// <summary>
    /// Takes a collection of TAGAttributes and populates any attribute that has certain valid dictionary attributes with
    /// those attributes. Currently this is just populating PickList
    /// </summary>
    /// <typeparam name="Attributes">Any valid DataClass collection of Attributes. This includes Items and AttributeCollections</typeparam>
    /// <param name="attributes">The instance of the collection of attributes</param>
    /// <returns>Reference to the same instance that was passed in</returns>
    public Attributes LoadDictionaryProperties<Attributes>(Attributes attributes) where Attributes : DataClass<TAGAttribute>
    {
      ItemType attributeProperties = AttributeProperties();
      foreach (TAGAttribute a in attributes)
      {
        if (attributeProperties.Contains(a.ID)) // does the dictionary have an entry for this attribute?
        {
          Item thisAttribute = attributeProperties[a.ID];
          if (thisAttribute.Attributes.Contains(PICKLIST))
          {
            string pickListName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, thisAttribute[PICKLIST].Value);
            if (attributes.Contains(pickListName))
            {
              string pickList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, attributes[pickListName].Value);
              a.PickList = new PickList(pickList, null, false);
            }
          }
        }
      }
      return attributes;
    }
    public Dictionary<string, DictionaryElement> getDictionaryElements()
    {
      Dictionary<string, DictionaryElement> elementList = new Dictionary<string, DictionaryElement>(StringComparer.CurrentCultureIgnoreCase);
      foreach (Item function in doDictionary.Entities[cDICTIONARY].ItemTypes[cFUNCTION].Items)
      {
        TableHeader parmDefinition = (TableHeader)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CTableHeader, "Parameters", function.getValue("Parameters"));
        string returnType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, function.getValue("DataType"), "object");
        DictionaryElement element = new DictionaryElement(function.OriginalID, ExpressionType.Function, returnType, parmDefinition);
        elementList.Add(element.ID, element);
      }
      foreach (Item attribute in doDictionary.Entities[cDICTIONARY].ItemTypes[cATTRIBUTE].Items)
      {
        DictionaryElement element = 
          new DictionaryElement(attribute.OriginalID, ExpressionType.Attribute, 
            (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, attribute.getValue("DataType")), null);

        elementList.Add(element.ID, element);
      }
      foreach (string atParm in AtParameterList)
      {
        DictionaryElement element = new DictionaryElement(atParm.Substring(1), ExpressionType.AtAtValue, "object", null);
        elementList.Add(element.ID, element);
      }
      foreach (string op in TAGFunctions.operatorList)
      {
        DictionaryElement element = new DictionaryElement(op, ExpressionType.Operator, "string", null);
        elementList.Add(element.ID, element);
      }
      return elementList;
    }


    #endregion public methods

    #region private methods
    /// <summary>
    /// Does the object value have the same datatype as it is supposed to?
    /// </summary>
    /// <param name="dataType">Name of the data type the object is supposed to have</param>
    /// <param name="pValue">The object we are checking</param>
    /// <returns>True if passes, test, false if not.</returns>
    private bool isValidDataType(string dataType, object pValue)
    {
      bool isValid = false;
      if (pValue == null || (pValue.GetType() == typeof(string) && string.IsNullOrEmpty((string)pValue))) // if this is null or empty, then we consider that to be valid
        isValid = true;
      else
      {
        switch (dataType.ToLower())
        {
          case TAGFunctions.DATATYPESTRING:
            isValid = true;
            break;
          case TAGFunctions.DATATYPEINTEGER:
            isValid = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsInteger, pValue);
            break;
          case TAGFunctions.DATATYPEDATETIME:
          case "date":
            isValid = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsDateTime, pValue);
            break;
          case TAGFunctions.DATATYPEDECIMAL:
          case TAGFunctions.DATATYPEDOUBLE:
            isValid = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsNumeric, pValue);
            break;
          case "bool":
            isValid = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsBoolean, pValue);
            break;
        }
      }
      
      return isValid;
    }
    /// <summary>
    /// Does the object value pass the validation criteria>
    /// </summary>
    /// <param name="validationString">AttributeTable format table with a list of tests</param>
    /// <param name="pValue">The object we are checking</param>
    /// <param name="pErrorMessage">The error message if the test failed</param>
    /// <param name="warn">Is this a warning only (true) or is it a "hard error" (false)</param>
    /// <returns>True if passes, test, false if not.</returns>
    private bool passesValidation(string validationString, string valueType, object pValue, out string pErrorMessage, out bool warn)
    {
      /*
       * Validation string is of type AttributeTable ListSource. it is a 2D table with column separators
       * of ~ and row separators of |. It has three columns. Column1 is the validation string. Column2
       * is the error message to present if it fails the validation string, Column2 is a bool
       * which is set to true if failing the validation is only a warning, and is false if
       * failing the validation causes a "hard" fail.
       * 
       * Note that the test is a failure test. That is, if the expression is true, the test fails.
       * This combined with the implied "and" between lines in the table (each a single test) creates
       * the easiest to author.
       * 
       * We use the AttributeTable object to parse the table.
       * 
       * The validation string is currently incredibly simple. It is in the form
       *   operand1 operator operand2
       *   where the attribute value is represented by "x".
       *   as in: x > 40       (numeric comparison)
       *   or   x == "END"     (string comparison)
       *   or   x > '01/12/2007' (date comparison)
       *   
       *  if there is no first operand, then x is implied, as in:
       *      > 12
       *  Operators supported are c# style and are:
       *      ==, >, <, >=, <=, !=
       */
      bool isValid = true;      // default is pass unless we fail
      warn = false;           // default for warn is false (not just a warning)
      pErrorMessage = string.Empty;   // default error message is empty
      string operand1 = string.Empty; // operand 1 
      string operand2 = string.Empty; // operand 2
      string op = string.Empty;     // operator (which is a reserved word, so we use "op")
      int i = 0;
      string[] validationDataTypes = new string[] { "string", "string", "bool" };
      PickList validationTable = new PickList(validationString, validationDataTypes, false);
      /*
       * we can have multiple validation rows (like x < 20 and x > 40). There is an implied "and" between them.
       * The tests are all fail tests (the validation fails if the condition is true.
       */

      for (; i < validationTable.GetLength(0) && isValid; i++) // for each row or until a validation fails
      {
        pErrorMessage = string.Empty;
        //TODO: REF Parameters: We need to review this!
        //if (TAGFunctions.parseValidationString(validationTable[i, 0].ToString(), ref operand1, ref op, ref operand2))// does this parse?
        if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseValidationString, validationTable[i, 0].ToString(), ref operand1, ref op, ref operand2))// does this parse?
        //isValid = !TAGFunctions.failsTest(operand1, op, operand2, pValue);    // then do the comparison
          //We don't need to negate failsTest: If the test fails it means the value IS valid -Leonardo 
          isValid = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.failsTest, operand1, op, operand2, pValue); 
        // if it does not parse, then the validation passes (does not change the default value of isValid)
        if (!isValid)                           // if it did not pass
        {
          //TODO: Has an error because it's trying to get the following coordinates [i,1]
          // and it only has one dimension [0,0]
          pErrorMessage = validationTable[i, 1].ToString();        // then pick up the validation mesage from the table
          try
          {
            warn = Convert.ToBoolean(validationTable[i, 2]);        // and pick up the warn flag
          }
          catch
          {
            warn = false;
          }
          break;                            // then exit the loop, don't try any more compares
        }
      }
      return isValid;
    }
    public bool passesPickList(PickList pickList, object pValue)
    {
      if (pValue == null || pValue.ToString() == string.Empty)
        return true;
      if (pickList == null)   // if there is no pickist then the validation passes
        return true;
      for (int i = 0; i < pickList.Count; i++)
        if (((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, pValue)).ToLower() 
          == ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, pickList[i, 0])).ToLower()) // compare to our one value
          return true;                   // we found it... validation passes and we stop looking

      return false;
    }
    /// <summary>
    /// Takes the AttributeTable format PickList, and tries to compare the value in pValue
    /// with one of the entries. If a match is found, the test passes.
    /// </summary>
    private EntityAttributesCollection Attributes   // don't want to expose this for noww
    {
      get { return doDictionary; }
    }
    #endregion private methods

  }
}
