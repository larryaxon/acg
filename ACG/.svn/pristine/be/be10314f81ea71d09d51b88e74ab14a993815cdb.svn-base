using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Text;

using TAGBOSS.Common.Interface;

namespace TAGBOSS.Common.Model
{
  /// <summary>
  /// This is the universal data class for Entities and their corresponding Attributes. It is a collection of collections.
  /// The structure is as follows
  /// base: 
  ///     
  /// </summary>
  [SerializableAttribute]
  public class EntityAttributesCollection 
  {
    #region module constants

    //const string c_TO_XML_AMP = "&amp;";
    //const string c_TO_XML_LT = "&lt;";
    //const string c_TO_XML_GT = "&gt;";
    //const string c_TO_XML_QUOT = "&quot;";
    //const string c_TO_XML_APOS = "&apos;";

    //const string c_FROM_XML_AMP = "&";
    //const string c_FROM_XML_LT = "<";
    //const string c_FROM_XML_GT = ">";
    //const string c_FROM_XML_QUOT = "\"";
    //const string c_FROM_XML_APOS = "'";

    #endregion module constants

    #region module data

    /*
     * Main data store. 
     */
    private DateTime pastDateTime;
    private DateTime futureDateTime;
    private EntitiesCollection entities;                      // storage for the master collection
    //private DataSet dsData;
    /*
     * New local data 
     */
    //private bool loadAttributes = true;                       // Does this instance contain TAGAttribute Object data?
    private DateTime effectiveDate;                           // Effective Date using which this object was created
    private bool isRaw = false;                               // Is this collection in Raw Mode (no inheritance, etc.)?
    //private Dictionaries dictionary = null;

    #endregion module data

    #region constructors/destructors

    //public EntityAttributesCollection(Dictionaries dict)
    //{
    //  pastDateTime = TAGFunctions.PastDateTime;                          // init the default value for "way back in the past"
    //  futureDateTime = TAGFunctions.FutureDateTime;                      // init the defaule value for "way out in the future"
    //  entities = new EntitiesCollection();
    //  dictionary = dict;
    //  if (dictionary != null)
    //    entities.Dictionary = dictionary;
    //}
    /// <summary>
    /// Default constructor that initializes the Entities collection. It leaves the value for UseAttributes
    /// with its default value of true (the collection will 
    /// </summary>
    public EntityAttributesCollection()
    {
      pastDateTime = TAGFunctions.PastDateTime;                          // init the default value for "way back in the past"
      futureDateTime = TAGFunctions.FutureDateTime;                      // init the defaule value for "way out in the future"
      entities = new EntitiesCollection();
    }
    ///// <summary>
    ///// Constructor that specifies a value For UseAttributes (should this instance load attributes along with Entities)
    ///// </summary>
    ///// <param name="pLoadAttributes"></param>
    //public EntityAttributesCollection(bool pUseAttributes)                    // constructor that overrides loadAttributes
    //  : this()
    //{
    //  loadAttributes = pUseAttributes;
    //}
    #endregion constructors/destructors

    #region public properties

    public Dictionaries Dictionary
    {
      get { return DictionaryFactory.getInstance().getDictionary(); }
      //set { dictionary = value; }
    }
    /// <summary>
    /// This is a collection of type Entity. There is one of these for each entity in the collection
    /// </summary>
    public EntitiesCollection Entities                                  // Exposes the master (entities) collection
    {
      get { return entities; }
      set { entities = value; }
    }
    //public bool UseAttributes
    //{
    ///// <summary>
    ///// Property that allows the calling program to specify if Attributes are to be loaded along with the entities.
    ///// 
    ///// </summary>    //  get { return loadAttributes; }
    //  set { loadAttributes = value; }
    //}
    /// <summary>
    /// Entities, Items, and Attributes all can have history (multiple entries with date ranges). This date is
    /// used to only return the instances that whose date ranges contain a specific date. If not specified,
    /// the default is the current system date.
    /// </summary>
    public DateTime EffectiveDate
    {
      get { return effectiveDate; }
      set { effectiveDate = value; }
    }
    /// <summary>
    /// Is this data collection in Raw Mode?
    /// </summary>
    public bool IsRaw
    {
      get { return isRaw; }
      set { isRaw = value; }
    }
    #endregion public properties

    #region public methods

    public bool hasItem()
    {
      if (entities.Count == 0)
        return false;
      bool hasData = false;
      foreach (Entity e in entities)
      { 
        if (e.ItemTypes.Count > 0)
        {
          foreach (ItemType it in e.ItemTypes)
          {
            if (it.Items.Count > 0)
            {
              hasData = true;
              break;
            }
          }
          if (hasData)
            break;
        }
      }
      return hasData;
    }

    /// <summary>
    /// Returns the value of a field or attribute using a single string (valuePath) in the format:
    /// <para>{Entity} (returns an Entity Object)</para>
    /// <para>{Entity}.{ItemType}.{Item}.{TAGAttribute} (returns an TAGAttribute Value)or</para>
    /// <para>{Entity}.{ItemType}.{Item} (returns an Item object)or</para>
    /// <para>{Entity}.ItemType.{ItemType} (Middle token is the constant "ItemType". 
    /// This returns an ItemType object)or</para>
    /// <para>{Entity}.{Fieldname} (returns a Field value)</para>
    /// If any of the object references do not exist (i.e. the Entity or TAGAttribute, etc.),
    /// it returns an empty object of the type specified. If valuePath does not match
    /// any of the proper formats above, it returns a null.
    /// </summary>
    /// <param name="valuePath"></param>
    /// <returns>Value of the TAGAttribute or Field or Item</returns>
    public object getValue(string valuePath)
    {
      try
      {
        string[] pathList = pathElements(valuePath);
        switch (pathList.GetLength(0))                        // how many components?
        {
          case 1:
            if (Entities.Contains(pathList[0]))
              return Entities[pathList[0]];
            else
              return new Entity();
          case 2:
            return getFieldValue(pathList[0], pathList[1]);   // entity.fieldname
          case 3:                                             
            // entity.itemtype.item (returns an item) OR
            // entity."ItemType".ItemType (returns an itemType)
            return getValue(pathList[0], pathList[1], pathList[2]);
          case 4:                                             // entity.itemtype.item.attribute
            return getValue(pathList[0], pathList[1], pathList[2], pathList[3]);
          default:
            return null;
        }
      }
      catch
      {
        return null;
      }
    }
    /// <summary>
    /// Sets the value of a field or attribute to newValue using a single string (valuePath) in the format:
    /// <para>{Entity}.{ItemType}.{Item}.{TAGAttribute} or</para>
    /// <para>{Entity}.{Fieldname}</para>
    /// If any of the object references do not exist (i.e. the Entity or TAGAttribute, etc.),
    /// it creates all of the ones necessary to set the value.
    /// </summary>
    /// <param name="valuePath"></param>
    /// <param name="newValue"></param>
    public void setValue(string valuePath, object newValue)
    {
      string[] pathList = pathElements(valuePath);
      if (pathList.GetLength(0) == 4)                                   // entity.itemtype.item.attribute
        setValue(pathList[0], pathList[1], pathList[2], pathList[3], newValue);
      else
        if (pathList.GetLength(0) == 2)
          setFieldValue(pathList[0], pathList[1], newValue);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public object getValue(string entity, string itemType, string item, string attribute)
    {
      return getValue(entity, itemType, item, attribute, null);
    }
    /// <summary>
    /// Checks to make sure all of the objects in the chain (Entity, ItemType, Item, and TAGAttribute)
    /// exist. If so, it returns the value as type object. If not, it returns null.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    /// <param name="attribute"></param>
    /// <returns></returns>
    public object getValue(string entity, string itemType, string item, string attribute, object defaultValue)
    {
      object myObject = defaultValue;
      if (entities.Contains(entity))
      {
        Entity e = entities[entity];
        if (e.ItemTypes.Contains(itemType))
        {
          ItemType it = e.ItemTypes[itemType];
          if (it.Items.Contains(item))
          {
            Item i = it.Items[item];
            if (i.Attributes.Contains(attribute))
              myObject = i.Attributes[attribute].Value;
          }
        }
      }
      return myObject;
    }

    /// <summary>
    /// Checks to make sure all of the objects in the chain (Entity, ItemType, Item)
    /// exist. If so, it returns the object. If not, it returns the new object of that kind (not casted).
    /// This overload passes the toItemTypeIfApplicable flag in true, wich is the default behaviour
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public object getValue(string entity, string itemType, string item)
    {
      return getValue(entity, itemType, item, true);
    }

    /// <summary>
    /// Checks to make sure all of the objects in the chain (Entity, ItemType, Item)
    /// exist. If so, it returns the object. If not, it returns the new object of that kind (not casted).
    /// the last parameter is a flag so as to return an itemType or an item if it does not apply, 
    /// this only in the case when this gets called with a "itemType" itemType parameter value
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    /// <param name="toItemTypeIfApplicable"></param>
    /// <returns></returns>
    public object getValue(string entity, string itemType, string item, bool toItemTypeIfApplicable)
    {
      /*
       * Three parameters may be Entity.ItemType.Item, which returns the item or
       * an empty item if none is found, OR...
       * 
       * Entity."ItemType".ItemType, where the middle parameter is the constant
       * "ItemType", which returns and itemType object.
       */
      object myObject;
      bool isItemType = (itemType.ToLower().Equals("itemtype") && toItemTypeIfApplicable);
      if (isItemType)
        myObject = new ItemType();
      else
        myObject = new Item();
      if (entities.Contains(entity))
      {
        Entity e = entities[entity];
        if (isItemType)
        {
          if (e.ItemTypes.Contains(item))
            myObject = e.ItemTypes[item];
        }
        else
        {
          if (e.ItemTypes.Contains(itemType))
          {
            ItemType it = e.ItemTypes[itemType];
            if (it.Items.Contains(item))
            {
              myObject = it.Items[item];
            }
          }
        }
      }
      return myObject;
    }
    /// <summary>
    /// Sets a value and a valuetype for Entity/ItemType/Item/TAGAttribute. If any of these are missing (do not yet exist)
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    /// <param name="attribute"></param>
    /// <param name="value"></param>
    /// <param name="valueType"></param>
    //public void setValue(string entity, string itemType, string item, string attribute, object value, string valueType)
    //{
    //  this.setValue(entity, itemType, item, attribute, value);
    //  if (entities.Contains(entity)) // it always ensure the existing of all the componentes. despite the previous set.
    //  {
    //    Entity e = entities[entity];
    //    if (e.ItemTypes.Contains(itemType))
    //    {
    //      ItemType it = e.ItemTypes[itemType];
    //      if (it.Items.Contains(item))
    //      {
    //        Item i = it.Items[item];
    //        if (i.Attributes.Contains(attribute))
    //        {
    //          i.Attributes[attribute].ValueType = valueType;    // they all exist, So set the valuetype
    //          i.Attributes[attribute].Values[i.Attributes[attribute].StartDate.ToString()].ValueType = valueType;
    //        }
    //      }
    //    }
    //  }
    //}
    /// <summary>
    /// Sets a value for Entity/ItemType/Item/TAGAttribute. If any of these are missing (do not yet exist),
    /// it adds them. Also overwrites valuehistory according effective date
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    /// <param name="attribute"></param>
    /// <param name="value"></param>
    /// <param name="overwriteValues"></param>
    public void setValue(string entity, string itemType, string item, string attribute, object value, bool overwriteValues)
    {
      this.setValue(entity, itemType, item, attribute, value);
    }
    public void setValue(string entity, string itemType, string item, string attribute, object value)
    {
      setValue(entity, itemType, item, attribute, value, null);
    }

    /// <summary>
    /// Sets a value for Entity/ItemType/Item/TAGAttribute. If any of these are missing (do not yet exist),
    /// it adds them.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="item"></param>
    /// <param name="attribute"></param>
    /// <param name="value"></param>
    public void setValue(string entity, string itemType, string item, string attribute, object value, string dataType)
    {
      object newValue;
      if (dataType == null)
        newValue = value;
      else
      {
        if (dataType == "")
          dataType = TAGFunctions.DATATYPESTRING;
        newValue = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, value, dataType);
      }
      if (entities.Contains(entity))
      {
        Entity e = entities[entity];
        if (e.ItemTypes.Contains(itemType))
        {
          ItemType it = e.ItemTypes[itemType];
          if (it.Items.Contains(item))
          {
            Item i = it.Items[item];
            if (i.Attributes.Contains(attribute))
              i.Attributes[attribute].setValue(newValue, dataType);    // they all exist, just set the value
            else
              i.AddAttribute(attribute, newValue);         // TAGAttribute object does not exist, so create it with the new value
          }
          else
          {
            Item i = new Item();                        // Item does not exist. Add both it and TAGAttribute
            i.ID = item;
            i.AddAttribute(attribute, newValue);
            it.Add(i);
          }
        }
        else
        {
          ItemType it = new ItemType();                 // Item Type does not exist. Add it, Item, and TAGAttribute
          it.ID = itemType;
          Item i = new Item();
          i.ID = item;
          i.AddAttribute(attribute, newValue);
          it.Items.Add(i);
          e.ItemTypes.Add(it);
        }
      }
      else
      {
        Entity e = new Entity();                        // None of them exist. Add everything.
        e.ID = entity;
        ItemType it = new ItemType();
        it.ID = itemType;
        Item i = new Item();
        i.ID = item;
        i.AddAttribute(attribute, newValue);
        it.Add(i);
        e.ItemTypes.Add(it);
        entities.Add(e);
      }
    }

    /// <summary>
    /// Updates the value history
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="itemType"></param>
    /// <param name="attribute"></param>
    /// <param name="newValueHistory"></param>
    public void updateValueHistory(string entity, string itemType, string item, string attribute, ValueHistory newValueHistory, string valueHistoryId)
    {
      if (entities.Contains(entity))
      {
        Entity e = entities[entity];
        if (e.ItemTypes.Contains(itemType))
        {
          ItemType it = e.ItemTypes[itemType];
          if (it.Items.Contains(item))
          {
            Item i = it.Items[item];
            if (i.Attributes.Contains(attribute))
            {
                i.Attributes[attribute].Values[valueHistoryId] = newValueHistory;

                // updates the dirty flag of the item and attribute
                i.Dirty = true;
                i.Attributes[attribute].Dirty = true;
            }
          }
        }
      }
    }

    //*-----------------------------------------------------------------------------------------------------------------------------------------------------*/
    //methods to convert a dataObjectCollection to a xml string (it can be the whole collection, an entity, an itemType, or an item)
    //*-----------------------------------------------------------------------------------------------------------------------------------------------------*/
    /// <summary>
    /// returns the entityAttributesCollection as an XML string
    /// </summary>
    /// <param name="myEAC"></param>
    /// <returns></returns>
    public string toXML(EntityAttributesCollection myEAC)
    {
      if (myEAC == null)
        return null;

      StringBuilder xmlEAC = new StringBuilder("<entities>");
      myEAC.Entities.Sort();
      foreach (Entity myEntity in myEAC.entities)
        xmlEAC.Append(toXML(myEntity));

      xmlEAC.Append("</entities>");
      return xmlEAC.ToString();
    }

    /// <summary>
    /// returns the entity as an XML string
    /// </summary>
    /// <param name="myEACEntity"></param>
    /// <returns></returns>
    public string toXML(Entity myEACEntity)
    {
      if (myEACEntity == null)
        return null;

      //StringBuilder xmlEACEntity = new StringBuilder(string.Format("<entity id='{0}'><itemtypes>", myEACEntity.ID));
      StringBuilder xmlEACEntity = new StringBuilder(string.Format("<entity id='{0}'><fields>", myEACEntity.ID));
      myEACEntity.Fields.Sort();
      myEACEntity.ItemTypes.Sort();
      foreach (Field myField in myEACEntity.Fields) 
        xmlEACEntity.Append(string.Format("<field name = '{0}' value = '{1}' />", myField.ID, (myField.Value == null?"(NULL)":myField.Value.ToString())));

      xmlEACEntity.Append("</fields><itemtypes>");

      foreach (ItemType myItemType in myEACEntity.ItemTypes)
        xmlEACEntity.Append(toXML(myItemType));

      xmlEACEntity.Append("</itemtypes></entity>");
      return xmlEACEntity.ToString();

    }

    /// <summary>
    /// returns the itemType as an XML string
    /// </summary>
    /// <param name="myEACItemType"></param>
    /// <returns></returns>
    public string toXML(ItemType myEACItemType)
    {
      if (myEACItemType == null)
        return null;

      StringBuilder xmlEACItemType = new StringBuilder(string.Format("<itemtype id='{0}'><items>", myEACItemType.ID));
      myEACItemType.Items.Sort();
      foreach (Item myItem in myEACItemType)
        xmlEACItemType.Append(toXML(myItem));

      xmlEACItemType.Append("</items></itemtype>");
      return xmlEACItemType.ToString();
    }

    /// <summary>
    /// returns the Item as an XML string
    /// </summary>
    /// <param name="myEACItem"></param>
    /// <returns></returns>
    public string toXML(Item myEACItem)
    {
      if (myEACItem == null)
        return null;

      StringBuilder xmlEACItem = new StringBuilder(string.Format("<item id='{0}'><attributes>", myEACItem.ID));
      string sMyValue = "";
      myEACItem.Attributes.Sort();
      foreach (TAGAttribute myAttribute in myEACItem.Attributes)
      {
        //This characters must be replaced in the value of the attribute
        xmlEACItem.Append(toXML(myAttribute));
        //sMyValue = TAGFunctions.toValidXMLString(myAttribute.Value);
        //string myValueType = myAttribute.ValueType;
        //if (myValueType == null || myValueType == string.Empty)
        //  myValueType = "value";
        //xmlEACItem.Append(string.Format("<attribute name='{0}' {1} ='{2}'></attribute>", myAttribute.ID, myValueType, sMyValue));
      }

      xmlEACItem.Append("</attributes></item>");
      return xmlEACItem.ToString();
    }

    /// <summary>
    /// returns the Attribute as an XML string
    /// </summary>
    /// <param name="myEACItem"></param>
    /// <returns></returns>
    public string toXML(TAGAttribute myEACAttribute)
    {
      if (myEACAttribute == null)
        return null;

      string sMyValue = 
        (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValidXMLString, 
        (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, myEACAttribute.Value));

      string myValueType = myEACAttribute.ValueType;
      if (myValueType == null || myValueType == string.Empty)
        myValueType = "value";
      StringBuilder xmlEACAttribute = new StringBuilder(string.Format("<attribute name='{0}' {1} ='{2}'>", myEACAttribute.ID, myValueType, sMyValue));

      string sMyValueHst = "";
      foreach (ValueHistory myAttrVH in myEACAttribute.Values)
      {
        //This characters must be replaced in the value of the attribute
        sMyValueHst = 
          (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValidXMLString, 
          (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, myAttrVH.Value));

        myValueType = myAttrVH.ValueType;
        if (myValueType == null || myValueType == string.Empty)
          myValueType = "value";
        string myVHStartDate = myAttrVH.StartDate.ToShortDateString();
        string myVHEndDate = myAttrVH.EndDate.ToShortDateString();
        xmlEACAttribute.Append(
          string.Format("<valueHistory ID = '{0}' StartDate='{1}' EndDate = '{2}'  {3} ='{4}'/>", myAttrVH.ID, myVHStartDate, myVHEndDate, myValueType, sMyValueHst));
      }

      xmlEACAttribute.Append("</attribute>");
      return xmlEACAttribute.ToString();
    }


    //methods to convert a entityAttributesCollection to a XML string 
    //(it can be the whole collection, an entity, an itemType, or an item)
    //including xtra data like flags, valueTypes etc.
    /// <summary>
    /// returns the entityAttributesCollection as an XML string with extra details
    /// </summary>
    /// <param name="myEAC"></param>
    /// <returns></returns>
    public string toXMLEnriched(EntityAttributesCollection myEAC)
    {
      if (myEAC == null)
        return null;

      StringBuilder xmlEAC = new StringBuilder("<entities>");
      foreach (Entity myEntity in myEAC.entities)
        xmlEAC.Append(toXMLEnriched(myEntity));

      xmlEAC.Append("</entities>");
      return xmlEAC.ToString();
    }

    /// <summary>
    /// returns the entity as an XML string with extra details
    /// </summary>
    /// <param name="myEAC"></param>
    /// <returns></returns>
    public string toXMLEnriched(Entity myEACEntity)
    {
      if (myEACEntity == null)
        return null;

      StringBuilder xmlEACEntity = new StringBuilder(string.Format("<entity id='{0}'><fields>", myEACEntity.ID));

      foreach (Field myField in myEACEntity.Fields)
        xmlEACEntity.Append(string.Format("<field name = '{0}' value = '{1}' />", myField.ID, (myField.Value == null?"(NULL)":myField.Value.ToString())));

      xmlEACEntity.Append("</fields><itemtypes>");
      foreach (ItemType myItemType in myEACEntity.ItemTypes)
        xmlEACEntity.Append( toXMLEnriched(myItemType));

      xmlEACEntity.Append("</itemtypes></entity>");
      return xmlEACEntity.ToString();
    }

    /// <summary>
    /// returns the itemType as an XML string with extra details
    /// </summary>
    /// <param name="myEACItemType"></param>
    /// <returns></returns>
    public string toXMLEnriched(ItemType myEACItemType)
    {
      if (myEACItemType == null)
        return null;

      StringBuilder xmlEACItemType = new StringBuilder(string.Format("<itemtype id='{0}'><items>", myEACItemType.ID));
      foreach (Item myItem in myEACItemType)
        xmlEACItemType.Append(toXMLEnriched(myItem));

      xmlEACItemType.Append("</items></itemtype>");
      return xmlEACItemType.ToString();
    }

    /// <summary>
    /// returns the Item as an XML string with extra details
    /// </summary>
    /// <param name="myEACItem"></param>
    /// <returns></returns>
    public string toXMLEnriched(Item myEACItem)
    {
      if (myEACItem == null)
        return null;

      StringBuilder xmlEACItem = new StringBuilder(string.Format("<item id='{0}' inherited='{1}' ><attributes>", myEACItem.ID, (myEACItem.IsInherited ? "true" : "false")));
      string sMyValue = "";
      foreach (TAGAttribute myAttribute in myEACItem.Attributes)
      {
        //This characters must be replaced in the value of the attribute
        sMyValue = (myAttribute.Value == null?"(NULL)":myAttribute.Value.ToString());
        sMyValue = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValidXMLString, sMyValue);

        xmlEACItem.Append(
          string.Format(
            "<attribute name='{0}' {1} ='{2}' inherited='{3}' included='{4}' refvalue='{5}' functionvalue='{6}' calculated='{7}' expressionvalue='{8}'>",
              myAttribute.ID, myAttribute.ValueType, sMyValue, 
              (myAttribute.IsInherited ? "true" : "false"),
              (myAttribute.IsIncluded ? "true" : "false"), 
              (myAttribute.IsRefValue ? "true" : "false"), 
              (myAttribute.IsFunctionValue ? "true" : "false"), 
              ("false"), 
              ("false")));
              //(myAttribute.IsExpressionValue ? "true" : "false") + "'>"));

        foreach (ValueHistory myValueHistory in myAttribute.Values)
        {
          sMyValue = (myValueHistory.Value == null?"(NULL)":myValueHistory.Value.ToString());
          sMyValue = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValidXMLString, sMyValue);

          xmlEACItem.Append(
            string.Format(
              "<valuehistory {0} ='{1}' startdate='{2}' enddate='{3}' lastmodifiedby='{4}' lastmodifieddatetime='{5}'/>",
                (myValueHistory.ValueType.Trim() == "" ? "value" : myValueHistory.ValueType), sMyValue,
                myValueHistory.StartDate.ToShortDateString(),
                myValueHistory.EndDate.ToShortDateString(),
                myValueHistory.LastModifiedBy,
                myValueHistory.LastModifiedDateTime.ToShortDateString()));
        }
        xmlEACItem.Append("</attribute>");
      }
      xmlEACItem.Append("</attributes></item>");
      return xmlEACItem.ToString();
    }

    //*--------------------------------------------------------------------------------------*/
    //methods to convert a XML string to a  entityAttributeCollection object 
    //(it can be the whole collection, an entity, an itemType, or an item)
    //including xtra data like flags, valueTypes etc.
    //*--------------------------------------------------------------------------------------*/

    /// <summary>
    /// This method converts a valid xml EAC string to a EAC entity object
    /// </summary>
    /// <param name="xmlEACEntity"></param>
    /// <returns></returns>
    public EntityAttributesCollection fromXMLtoEAC(string xmlEAC)
    {
      if (xmlEAC == null || xmlEAC == "")
        return null;

      XmlDocument xmlDocEAC = new XmlDocument();

      try
      {
        xmlDocEAC.LoadXml(xmlEAC);
      }
      catch
      {
        return null;   //Could not load the xml string, so we return a null reference...
      }

      //So we could load the xml string into the document, now we proceed...
      //we will not check to see if the loaded document has an itemType as it's root element
      //but it most, if not then it is an invalid call and we will return a null...

      //BEGIN:The following lines check for a valida xml doc to process!!.
      XmlNode xmlDoc = xmlDocEAC.SelectSingleNode("/entities");
      if (xmlDoc == null)
        return null;       //This document is not an item xml document
      //END:The following lines check for a valid xml doc to process!!

      //We have a valid item, let's read the data...
      EntityAttributesCollection myEAC = new EntityAttributesCollection();

      XmlNodeList xmlDocEntities = xmlDocEAC.SelectNodes("//entity");
      if (xmlDocEntities != null)
        foreach (XmlNode xmlDocEntity in xmlDocEntities)
        {
          Entity myEACEntity = fromXMLtoEntity(xmlDocEntity.OuterXml);
          if (myEACEntity != null)
            myEAC.Entities.Add(myEACEntity);
        }

      //-------------------------------------------------------
      //And here we return the entity just processed!
      return myEAC;
      //-------------------------------------------------------
    }

    /// <summary>
    /// This method converts a valid xml entity string to a EAC entity object
    /// </summary>
    /// <param name="xmlEACEntity"></param>
    /// <returns></returns>
    public Entity fromXMLtoEntity(string xmlEACEntity)
    {
      if (xmlEACEntity == null || xmlEACEntity == "")
        return null;

      XmlDocument xmlDocEACEntity = new XmlDocument();

      try
      {
        xmlDocEACEntity.LoadXml(xmlEACEntity);
      }
      catch
      {
        return null;   //Could not load the xml string, so we return a null reference...
      }

      //So we could load the xml string into the document, now we proceed...
      //we will not check to see if the loaded document has an itemType as it's root element
      //but it most, if not then it is an invalid call and we will return a null...

      //BEGIN:The following lines check for a valida xml doc to process!!.
      XmlNode xmlDocEntity = xmlDocEACEntity.SelectSingleNode("/entity");
      if (xmlDocEntity == null)
        return null;       //This document is not an item xml document

      XmlNode xmlDocEntityID = xmlDocEACEntity.SelectSingleNode("/entity/@id");
      if (xmlDocEntityID == null)
        return null;       //We don not have a valid Id so we return null
      //END:The following lines check for a valid xml doc to process!!

      //We have a valid item, let's read the data...
      Entity myEACEntity = new Entity();
      string entityID = xmlDocEntity.Attributes["id"].Value;
      myEACEntity.ID = entityID;

      XmlNodeList xmlDocItemTypes = xmlDocEntity.SelectNodes("//itemtype");
      if (xmlDocItemTypes != null)
        foreach (XmlNode xmlDocItemType in xmlDocItemTypes)
        {
          ItemType myEACItemType = fromXMLtoItemType(xmlDocItemType.OuterXml);
          if (myEACItemType != null)
            myEACEntity.ItemTypes.Add(myEACItemType);
        }

      //-------------------------------------------------------
      //And here we return the entity just processed!
      return myEACEntity;
      //-------------------------------------------------------
    }

    /// <summary>
    /// This method converts a valid xml itemType string to a EAC itemType object
    /// </summary>
    /// <param name="xmlEACItemType"></param>
    /// <returns></returns>
    public ItemType fromXMLtoItemType(string xmlEACItemType)
    {
      if (xmlEACItemType == null || xmlEACItemType == "")
        return null;

      XmlDocument xmlDocEACItemType = new XmlDocument();

      try
      {
        xmlDocEACItemType.LoadXml(xmlEACItemType);
      }
      catch
      {
        return null;   //Could not load the xml string, so we return a null reference...
      }

      //So we could load the xml string into the document, now we proceed...
      //we will not check to see if the loaded document has an itemType as it's root element
      //but it most, if not then it is an invalid call and we will return a null...

      //BEGIN:The following lines check for a valida xml doc to process!!.
      XmlNode xmlDocItemType = xmlDocEACItemType.SelectSingleNode("//itemtype");
      if (xmlDocItemType == null)
        return null;       //This document is not an item xml document

      XmlNode xmlDocItemTypeID = xmlDocEACItemType.SelectSingleNode("//itemtype/@id");
      if (xmlDocItemTypeID == null)
        return null;       //We don not have a valid Id so we return null
      //END:The following lines check for a valid xml doc to process!!

      //We have a valid item, let's read the data...
      ItemType myEACItemType = new ItemType();
      string itemTypeID = xmlDocItemType.Attributes["id"].Value;
      myEACItemType.ID = itemTypeID;

      XmlNodeList xmlDocItems = xmlDocItemType.SelectNodes("//item");
      if (xmlDocItems != null)
        foreach (XmlNode xmlDocItem in xmlDocItems)
        {
          Item myEACItem = fromXMLtoItem(xmlDocItem.OuterXml);
          if (myEACItem != null)
            myEACItemType.Add(myEACItem);
        }

      //-----------------------------------------------------------
      //And here we return the itemType just processed!
      return myEACItemType;
      //-----------------------------------------------------------
    }

    /// <summary>
    /// This method converts a valid xml item string to a EAC item object
    /// </summary>
    /// <param name="xmlEACItem"></param>
    /// <returns></returns>
    public Item fromXMLtoItem(string xmlEACItem)
    {
      if (xmlEACItem == null || xmlEACItem == "")
        return null;

      XmlDocument xmlDocEACItem = new XmlDocument();

      try
      {
        xmlDocEACItem.LoadXml(xmlEACItem);
      }
      catch
      {
        return null;   //Could not load the xml string, so we return a null reference...
      }

      //So we could load the xml string into the document, now we proceed...
      //we will not check to see if the loaded document has an item as it's root element
      //but it most, if not then it is an invalid call and we will return a null...

      //BEGIN:The following lines check for a valida xml doc to process!!.
      XmlNode xmlDocItem = xmlDocEACItem.SelectSingleNode("//item");
      if (xmlDocItem == null)
        return null;       //This document is not an item xml document

      XmlNode xmlDocItemID = xmlDocEACItem.SelectSingleNode("//item/@id");
      if (xmlDocItemID == null)
        return null;       //We don not have a valid Id so we return null
      //END:The following lines check for a valid xml doc to process!!

      //We have a valid item, let's read the data...
      Item myEACItem = new Item();
      string itemID = xmlDocItem.Attributes["id"].Value;
      myEACItem.ID = itemID;

      //BEGIN:inherited flag resolution
      XmlNode xmlDocData = xmlDocEACItem.SelectSingleNode(string.Format("//item[@id = '{0}']/@inherited", itemID));
      if (xmlDocData != null)
      {
        if (xmlDocData.Value == "true")
          myEACItem.IsInherited = true;
        else
          myEACItem.IsInherited = false;
      }
      else
        myEACItem.IsInherited = false;
      //END:inherited flag resolution
      XmlNodeList xmlDocItemAttributes = xmlDocEACItem.SelectNodes(string.Format("//item[@id='{0}']/attributes/*", itemID));

      foreach (XmlNode xmlDocItemAttr in xmlDocItemAttributes)
      {
        TAGAttribute myEACItemAttr = new TAGAttribute();
        string attrName = xmlDocItemAttr.Attributes["name"].Value;
        string attrValueType = xmlDocItemAttr.Attributes[1].Name.ToString().ToLower();  //second element is always the valueType
        object attrValueValue = xmlDocItemAttr.Attributes[1].Value.ToString();
        bool isInherited = (xmlDocItemAttr.Attributes["inherited"].Value == "true" ? true : false);
        bool isIncluded = (xmlDocItemAttr.Attributes["included"].Value == "true" ? true : false);
        bool isRefValue = (xmlDocItemAttr.Attributes["refvalue"].Value == "true" ? true : false);
        bool isFunctionValue = (xmlDocItemAttr.Attributes["functionvalue"].Value == "true" ? true : false);
        bool isCalculated = (xmlDocItemAttr.Attributes["calculated"].Value == "true" ? true : false);
        bool isExpressionValue = (xmlDocItemAttr.Attributes["expressionvalue"].Value == "true" ? true : false);

        XmlNodeList xmlValueHistories
          = xmlDocEACItem.SelectNodes(string.Format("//item[@id='{0}']/attributes/attribute[@name='{1}']/valuehistory", itemID, attrName));

        foreach (XmlNode xmlDocItemAttrVH in xmlValueHistories)
        {
          string attrValueTypeVH = null;
          object attrValueValueVH = null;
          DateTime attrStartDate = TAGFunctions.PastDateTime;
          DateTime attrEndDate = TAGFunctions.FutureDateTime;
          string attrLastModifiedBy = "";
          DateTime attrLastModifiedDateTime = DateTime.Now;
          foreach (XmlAttribute xmlDocItemAttrVHAttr in xmlDocItemAttrVH.Attributes)
          {
            switch (xmlDocItemAttrVHAttr.Name)
            {
              case "startdate":
                attrStartDate = global::System.Convert.ToDateTime(xmlDocItemAttrVH.Attributes["startdate"].Value.ToString());
                break;
              case "enddate":
                attrEndDate = global::System.Convert.ToDateTime(xmlDocItemAttrVH.Attributes["enddate"].Value.ToString());
                break;
              case "lastmodifiedby":
                attrLastModifiedBy = xmlDocItemAttrVH.Attributes["lastmodifiedby"].Value.ToString();
                break;
              case "lastmodifieddatetime":
                attrLastModifiedDateTime = global::System.Convert.ToDateTime(xmlDocItemAttrVH.Attributes["lastmodifieddatetime"].Value.ToString());
                break;
              default:
                attrValueValueVH = xmlDocItemAttrVH.Attributes[xmlDocItemAttrVHAttr.Name].Value.ToString();
                attrValueTypeVH = xmlDocItemAttrVHAttr.Name.ToString().ToLower();
                break;
            }
          }

          //Here we add the valueHistory to the attribute object we are processing!!
          myEACItemAttr.AddValue(attrValueValueVH, attrValueTypeVH, attrStartDate, attrEndDate);
        }
        myEACItemAttr.ID = attrName;
        myEACItemAttr.OverwriteValue = true;
        myEACItemAttr.Value = attrValueValue;
        myEACItemAttr.ValueType = attrValueType;
        myEACItemAttr.IsInherited = isInherited;
        myEACItemAttr.IsIncluded = isIncluded;
        myEACItemAttr.IsRefValue = isRefValue;
        myEACItemAttr.IsFunctionValue = isFunctionValue;

        //Here we add the attribute object to the item we are processing!!
        myEACItem.Add(myEACItemAttr);
      }

      //-----------------------------------------------------
      //And here we return the item just processed!
      return myEACItem;
      //-----------------------------------------------------
    }
    
    /// <summary>
    /// Clone methods, it creates a clon
    /// </summary>
    /// <returns></returns>
    public EntityAttributesCollection Clone()
    {
      EntityAttributesCollection e = new EntityAttributesCollection();
      e.Entities = (EntitiesCollection)entities.Clone();
      e.EffectiveDate = effectiveDate;
      e.IsRaw = isRaw;
      return e;
    }

    #endregion public methods

    #region private methods

    private string[] pathElements(string valuePath)
    {
      string[] myElements = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, valuePath, new string[] { "." });
      return myElements;
    }
    private object getFieldValue(string entity, string fieldName)
    {
      if (entities.Contains(entity))                              // does the entity exist?
      {
        Entity e;
        e = entities[entity];                                     // yep, pick it up
        if (e.Contains(fieldName))                                // does the field exist?
          return e.Fields[fieldName].Value;               // yep, get the value
        else
          return null;
      }
      return null;
    }
    private void setFieldValue(string entity, string fieldName, object newValue)
    {
      Entity e;
      if (entities.Contains(entity))                              // does the entity exist?
      {
        e = entities[entity];                                     // yep, pick it up
      }
      else
      {
        e = new Entity();
        e.ID = entity;
        entities.Add(e);
      }
      if (e.Contains(fieldName))                                // does the field exist?
        e.Fields[fieldName].Value = newValue;                   // yep, set the value
      else
        e.Fields.AddField(fieldName, newValue);                 // nope, create a new field
    }
    #endregion private methods
  }
}
