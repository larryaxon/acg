using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TAGBOSS.Common.Model;

//using TAGBOSS.Common;

namespace TAGBOSS.Common.Model
{
  /// <summary>
  /// The ItemType class. It is a collection of Items.
  /// </summary>
  [SerializableAttribute]
  public class ItemType : DataClass<Item>
  {
    private DateTime effectiveDate = DateTime.Now;
    private List<string> defaultItems = new List<string>();
    private List<string> nonDefaultItems = new List<string>();

    /// <summary>
    /// Collection of Items for this ItemType. Basically renames "this" for convenience of syntax
    /// </summary>
    public ItemType Items
    {
      get { return this; }
    }
    /// <summary>
    /// EffectiveDate is used to select which Item in the Item History is the correct element. Default is Current Date
    /// </summary>
    public DateTime EffectiveDate
    {
      get { return effectiveDate; }
      set { effectiveDate = value; }
    }

    public List<string> DefaultItems
    {
      get { return defaultItems; }
      set { defaultItems = value; }
    }

    public List<string> NonDefaultItems
    {
      get { return nonDefaultItems; }
      set { nonDefaultItems = value; }
    }
    public new void Add(Item i)
    {
      this.Add(i.ID, i);
    }
    public new void Add(string id, Item i)
    {
      base.Add(id, i);
      if (id != null && id != string.Empty)
      {
        string compareID = id.ToLower();
        if (compareID.StartsWith("default"))
        {
          if (!compareID.Equals("default"))
            if (defaultItems.Count == 0 || !(defaultItems.Contains(compareID)))
              defaultItems.Add(compareID);
        }
        else
          if (nonDefaultItems.Count == 0 || !(nonDefaultItems.Contains(compareID)))
            nonDefaultItems.Add(compareID);
      }
    }
    /// <summary>
    /// Returns a copy of the current object. All children are also cloned, so that every copy of every object 
    /// is brand new.
    /// </summary>
    /// <returns>A new copy of the object</returns>
    public new object Clone()
    {
      ItemType it = new ItemType();

      for (int index = 0; index < Count; index++)
      {
        string key = getKey(index);
        it.Add(key, (Item)this[index].Clone());
      }
      it.Deleted = Deleted;
      it.Dirty = base.isDirty;
      it.MarkForDelete = MarkForDelete;
      it.ID = OriginalID;
      it.Description = Description;
      //it = (ItemType)base.Clone<ItemType>(it);
      it.EffectiveDate = effectiveDate;

      //Clone the DefaultItems and NonDefaultItems collections!
      if (defaultItems.Count > 0)
        foreach (string defaultItem in defaultItems)
          it.defaultItems.Add(defaultItem);
      if (nonDefaultItems.Count > 0)
        foreach (string nonDefaultItem in nonDefaultItems)
          it.nonDefaultItems.Add(nonDefaultItem);

      return it;
    }
    /// <summary>
    /// Stateless routine which receives the XML data from e_Attribute Attributes column and populates 
    /// an Item object with the data from it. Effective date is used to select which attributes
    /// to include (if no <valuehistory/> node exists which matches the effective date, then the attribute
    /// is not included
    /// </summary>
    /// <param name="strAttributes"></param>    
    /// <param name="effectiveDate"></param>
    /// <returns>Returns an item object populated from the XML</returns>
    public Item AttributesCollection(string strAttributes, DateTime effectiveDate)
    {
      return AttributesCollection(strAttributes, effectiveDate, false);
    }
    /// <summary>
    /// Overload which supports the request of the data in "Raw mode" (no filtering by effectiveDate)
    /// </summary>
    /// <param name="strAttributes"></param>
    /// <param name="effectiveDate"></param>
    /// <param name="RawMode"></param>
    /// <returns></returns>
    public Item AttributesCollection(string strAttributes, object efDate, bool rawMode)
    {
      DateTime effectiveDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, efDate, DateTime.Now);
      if (strAttributes == null || strAttributes == string.Empty)
      {
        Item i = new Item();
        i.EffectiveDate = effectiveDate;
        return i;
      }
      //const string c_LONG_DATE_FORMAT = "yyyy-MM-ddThh:mm:ss";        //TODO: discuss about this "hard coded" format for dates!!
      //const string c_LONG_DATE_FORMAT = "yyyy-MM-ddThh:mm:ss";        //TODO: discuss about this "hard coded" format for dates!!
      const string c_DEFAULT_ITEM = "Default";        //Default item id...
      try
      {
        Item i = new Item();
        XmlDocument xmlAttributesInput = new XmlDocument();

        xmlAttributesInput.LoadXml(strAttributes);

        bool useDefaultValueHistory = false;
        string defaultValueHistory = "<valuehistory value='' startdate= '" + TAGFunctions.PastDateTime + "'/>";
        string strEffectiveDate = effectiveDate.ToShortDateString();
        XmlNodeList xmlAttributes = xmlAttributesInput.SelectNodes("//attributes/*");

        foreach (XmlNode xmlAttribute in xmlAttributes)
        {
          bool addAttribute = false;
          XmlAttribute xmlAttributeName = xmlAttribute.Attributes["name"];

          if (xmlAttributeName != null)
          {
            //addAttribute = true;
            string attributeName = xmlAttributeName.Value;
            object attributeValueValue = null;
            TAGAttribute ia = new TAGAttribute();
            ia.ID = attributeName;
            //if (Dictionary != null)
            //  ia.Dictionary = Dictionary;

            //Right now the Data has records without attributes (xml attributes) in the valuehistories...
            //So we create this kind of Default value!
            useDefaultValueHistory = (xmlAttribute.ChildNodes.Count == 0 || xmlAttribute.InnerXml.Trim() == "");
            if (useDefaultValueHistory)
              xmlAttribute.InnerXml = defaultValueHistory;

            XmlNodeList xmlValueHistories = xmlAttributesInput.SelectNodes(string.Format("//attributes/attribute[@name='{0}']/valuehistory", attributeName));

            string attrValueType = string.Empty;
            object attrValueValue = null;
            bool attrIsNull = false;

            // first, let's set the default values for everything we might get from an xml attribute
            DateTime attrStartDate = TAGFunctions.PastDateTime;
            DateTime attrEndDate = TAGFunctions.FutureDateTime;
            string attrLastModifiedBy = string.Empty;
            DateTime attrLastModifiedDateTime = DateTime.Now;
            if (xmlValueHistories != null)
            {
              foreach (XmlNode xmlValueHistory in xmlValueHistories)
              {
                attrStartDate = TAGFunctions.PastDateTime;
                attrEndDate = TAGFunctions.FutureDateTime;
                attrLastModifiedBy = string.Empty;
                attrLastModifiedDateTime = DateTime.Now;
                // now, lets go through the list of attributes and override the defaults if there is one
                foreach (XmlAttribute xmlValueHistoryAttr in xmlValueHistory.Attributes)
                {
                  switch (xmlValueHistoryAttr.Name.ToLower())
                  {
                    case TAGFunctions.STARTDATE:
                      attrStartDate = global::System.Convert.ToDateTime(xmlValueHistory.Attributes[TAGFunctions.STARTDATE].Value.ToString());
                      break;
                    case TAGFunctions.ENDDATE:
                      attrEndDate = global::System.Convert.ToDateTime(xmlValueHistory.Attributes[TAGFunctions.ENDDATE].Value.ToString());
                      break;
                    case TAGFunctions.LASTMODIFIEDBY:
                      attrLastModifiedBy = xmlValueHistory.Attributes[TAGFunctions.LASTMODIFIEDBY].Value.ToString();
                      break;
                    case TAGFunctions.LASTMODIFIEDDATETIME:
                      attrLastModifiedDateTime = global::System.Convert.ToDateTime(xmlValueHistory.Attributes[TAGFunctions.LASTMODIFIEDDATETIME].Value.ToString());
                      break;
                    default:
                      attrValueType = xmlValueHistoryAttr.Name.ToString().ToLower().Trim();
                      //IF the valuetype is TABLEMOD we changed it to TABLEHEADER

                      attrValueValue = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, xmlValueHistory.Attributes[xmlValueHistoryAttr.Name].Value);

                      switch (attrValueType)
                      {
                        case TAGFunctions.TABLENOHEADER:
                          attrValueType = TAGFunctions.VALUE;
                          break;
                        case TAGFunctions.TABLEMOD:
                        case TAGFunctions.TABLEHEADER:
                          attrValueType = TAGFunctions.TABLEHEADER;
                          if (Dictionary != null)
                            attrValueValue = new TableHeader(attributeName, (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, attrValueValue), Dictionary, TAGFunctions.ThrowDataConversionException);
                          break;
                      }
                      // the following was modified by LLA 11/24/2009 to strongly type Value and allow for IsNull to be properly set

                      if (attrValueType == TAGFunctions.VALUE)
                      {
                        if (attrValueValue.GetType() == typeof(string))
                        {
                          if ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, attrValueValue) == string.Empty)
                            attrIsNull = true;
                          else
                            if (Dictionary != null)
                              attrValueValue = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, attrValueValue, ia.DataType);
                        }
                      }

                      break;
                  }
                }
                ia.AddValue(attrValueValue, attrValueType, attrStartDate, attrEndDate, true, attrLastModifiedBy, attrLastModifiedDateTime);
              }
            }
            else
            {
              //TODO: Default Entity allows attributes with no valueange! we have to process them!!
              ia.ValueType = "value";
              ia.Value = string.Empty;
              ia.StartDate = attrStartDate;
              ia.EndDate = attrEndDate;
              ia.LastModifiedDateTime = DateTime.Now;
              ia.LastModifiedBy = string.Empty;
              addAttribute = true;
            }
            if (rawMode)
            {
              // note: there is no attribute value or value type in raw mode
              addAttribute = true;
            }
            else
            {
              foreach (ValueHistory vh in ia.Values)
              {
                if (effectiveDate >= vh.StartDate && effectiveDate <= vh.EndDate)
                {
                  if (!rawMode)
                  {
                    ia.OverwriteValue = true;
                    ia.ValueType = vh.ValueType;
                    ia.Value = vh.Value;
                    ia.OverwriteValue = false;
                    ia.LastModifiedBy = vh.LastModifiedBy;
                    ia.LastModifiedDateTime = vh.LastModifiedDateTime;
                  }
                  addAttribute = true;
                }
              }
              //if (useDefaultValueHistory)
              //  addAttribute = true;
            }
            if (addAttribute)
            {
              ia.Values.Sort();
              i.Add(ia);
              // datatype at the attribute level has been deprecated
              //XmlNode xmlAttributeDataType
              //   = xmlAttributesInput.SelectSingleNode(
              //   string.Format("//attributes/attribute[@name = '{0}']/@datatype", attributeName));
              //if (xmlAttributeDataType != null)
              //  ia.DataType = xmlAttributeDataType.Value;
            }
          }
        }

        i.Dirty = false;             // this is newly populated so reset dirty flags to not dirty
        return i;    //Return the item with the attributes structure already filtered...

      }
      catch (Exception exAttrColl)
      {
        throw;
      }
    }
    /// <summary>
    /// Returns an AttributeTable format string which contains a list of the Items in this collection
    /// </summary>
    /// <returns></returns>
    public PickList ToItemList()
    {
      int j = 0;
      int iRow = 0;
      string[] itemListHeaders = new string[] { "Name", "Value" };
      PickList itemList = new PickList(itemListHeaders, this.Items.Count);
      foreach (Item i in this.Items)
      {
        iRow = j;
        itemList[iRow, 0] = i.OriginalID;
        if (i.Contains("Description"))
        {
          string desc = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, i.Attributes["Description"].Value);
          if (desc == null || desc == string.Empty)
            itemList[iRow, 1] = null;
          else
            itemList[iRow, 1] = desc;
        }
        else
          itemList[iRow, 1] = i.OriginalID;

        j++;
      }
      return itemList;
    }
    public void AddItemZeroIndex()
    {
      for (int j = 0; j < items.Count; j++)
      {
        string key = items[j].ToString();
        string newKey = key + ":0";
        //Dictionary<int, Item>  value = itemHash[key];
        Item value = itemHash[key];
        items[j] = newKey;
        itemHash.Remove(key);
        itemHash.Add(newKey, value);
      }
    }
  }
}