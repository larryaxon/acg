using System;
using System.Collections;
using System.Data;
using System.Data.SqlTypes;

using System.Threading;
using System.Xml;

using TAGBOSS.Common;

using TAGBOSS.Common.Model;
using TAGBOSS.Sys.AttributeEngine2.Processor;

namespace TAGBOSS.Sys.AttributeEngine2.Dal
{
  public class AttributeData : ITAGBOSSData
  {
    public TEntity[] Entities(
      string[] entities, 
      string[] itemTypes,
      Hashtable entitiesRequested, 
      Hashtable itemTypesRequested, 
      Hashtable itemsRequested, 
      DateTime effectiveDate, 
      bool IsRawMode, 
      bool RetrieveVirtualItems)
    {
      DataSet ds = null;
      TEntity[] retEntities = null;

      ds = (new AttributeDB()).getData(entities, itemTypes, effectiveDate, IsRawMode);

      if (!IsRawMode && itemsRequested != null && RetrieveVirtualItems)
        ds = addVirtualItems(ds, itemTypesRequested, itemsRequested);

      retEntities = getEntities(ds, entitiesRequested, Constants.MaxEntityItems, effectiveDate, true, IsRawMode);
      ds.Clear();
      ds = null;
      
      return retEntities;
    }

    public TEntity[] SystemEntities(DateTime effectiveDate)
    {
      DataSet ds = null;
      TEntity[] retSysEntities = null;

      ds = (new AttributeDB()).getSystemData();
      retSysEntities = getEntities(ds, null, Constants.MaxSystemItems, effectiveDate, false, false);
      ds.Clear();
      ds = null;

      return retSysEntities;
    }
    public void Save(Entity e, bool rawMode, string user)
    {
      AttributeDB attributeDB = new AttributeDB();

      bool includeCleanAttributes = rawMode;
      string entity = e.OriginalID;
      foreach (ItemType it in e.ItemTypes)
      {
        string itemType = it.OriginalID;
        foreach (Item i in it.Items)
        {
          DateTime effectiveDate = i.EffectiveDate;
          if (i.Dirty)
          {
            if (i.Deleted)
            {
              attributeDB.DeleteItem(entity, itemType, i.ID);
            }
            else
            {
              string item = i.OriginalID;
              if (rawMode)  // if we are in raw mode, we just replace what is out there with what we have
              {
                attributeDB.SaveItem(entity, itemType, item, user);
                if (i.ItemHistoryRecords.Dirty)
                {
                  // if something in the item history has changed, we just wipe out the old and replace with the new
                  // Otherwise it gets complicated checking date ranges and changes
                  attributeDB.DeleteItemHistory(entity, itemType, item);
                  foreach (ItemHistory ih in i.ItemHistoryRecords)
                    if (ih.Dirty)
                      attributeDB.SaveItemHistory(entity, itemType, item, ih.StartDate, ih.EndDate, user, DateTime.Now);
                    else
                    {
                      if (ih.LastModifiedDateTime < TAGFunctions.PastDateTime)  // check for bad date
                        ih.LastModifiedDateTime = DateTime.Now;
                      if (string.IsNullOrEmpty(ih.LastModifiedBy))
                        ih.LastModifiedBy = user;
                      attributeDB.SaveItemHistory(entity, itemType, item, ih.StartDate, ih.EndDate, ih.LastModifiedBy, ih.LastModifiedDateTime);
                    }
                }
                foreach (TAGAttribute a in i.Attributes)
                  SaveAttribute(attributeDB, entity, itemType, item, user, a);
              }
              else
              {
                /*
                 * 9/21/2011 LLA: These notes were copied from the old Item.ToXML() routine. Then i changed the terminology to be
                 * more appropriate for new non-xml structure. However, the logic is the same which is why i include it here.
                 * 
                 * Firstly, if it is raw mode, then we just get rid of the old data and substitute it for new.
                 * 
                 * IF not raw mode, we need to merge the old attribute list with the new one
                 * 
                 * 1) start with the old list of attributes. You will need to retrieve this from the table. We do this in case
                 *      someone has updated it since we retrieved it last
                 *      NOTE: if someone has added an attribute since we retrieved it, we will pick it up.
                 *      However, if they have updated an attribute, we will overwrite it.
                 * 2) Add new Attributes (dirty == true and node does not exist in old XML)
                 * 3) Update Attributes (dirty == true and node does exist in old XML)
                 *      Note: Calling program is responsible for managing Values collection. Just delete the old TAGAttribute
                 *      node with its values collection and replace with the new one
                 * 4) Deleted. Deleted records are the same as updates in valuehistory, because they are flagged already
                 *     in the valuehistory as deleted with end date one day before the start date: 
                 *     
                 * 9/18/2008 LLA: Added logic to ignore attributes if their ReadOnly flag is set
                 * 
                 * 6/9/2011 LLA: We have this one condition that we have not handled properly. It is:
                 * 1) inherited load of the item for item maintenance had an effectivedate that matched a "hole" in
                 *    at least one attribute value history. e.g. effective date after last end date, or between the
                 *    end date of one value history and the start date of the next.
                 * 2) The attribute therefore is NOT included in the result set, per current specifications
                 * 3) The user enters a value for that attribute and saves it. That creates exactly one new value history with
                 *    a start date of the effectivedate and and enddate of TAGFunctions.FutureDateTime
                 * 4) In this case, the old xml has valid history we need to merge with the new history record. We need to:
                 *    a) find the "hole" in the old value history
                 *    b) find the next value history AFTER the new one in the hole
                 *    c) change the end date of the new one to one day before the start date of the next old on
                 *    d) merge the history
                 * So, to do this, we load the old data into an item so we can search and find the conflict and merge it.
                 * Note: we only do this if we need to, so we start with a null OldItem and ItemType object (needed to
                 * run the AttributesColection() import of xml to Item) and only instantiate them if we have an attribute
                 * that meets the requirements (just one vh with start date of effectivedate)
                 */
                if (i.ItemHistoryRecords.Dirty)
                {
                  // if something in the item history has changed, we just wipe out the old and replace with the new
                  // Otherwise it gets complicated checking date ranges and changes
                  attributeDB.DeleteItemHistory(entity, itemType, item);
                  foreach (ItemHistory ih in i.ItemHistoryRecords)
                    if (ih.Dirty)
                      attributeDB.SaveItemHistory(entity, itemType, item, ih.StartDate, ih.EndDate, user, DateTime.Now);
                    else
                      attributeDB.SaveItemHistory(entity, itemType, item, ih.StartDate, ih.EndDate, ih.LastModifiedBy, ih.LastModifiedDateTime);
                }
                Item oldItem = getRawItem(entity, itemType, item);
                foreach (TAGAttribute aNew in i.Attributes)
                {
                  if (aNew.Deleted)
                  {
                    TAGAttribute aOld = null;
                    if (oldItem.Attributes.Contains(aNew.ID))
                      aOld = oldItem.Attributes[aNew.ID];
                    if (aOld != null) // it is in the old xml (if not, we don't do anything cause it is already deleted
                    {
                      ValueHistoryCollection aOldValueHistories
                        = aOld.Values;
                      if (aOldValueHistories.Count > 0) // if it has value history (otherwise we do nothing, since a value is not there to delete
                      {
                        for (int iRow = aOldValueHistories.Count - 1; iRow >= 0; iRow--) // look for the value history that matches the effective date
                        {
                          ValueHistory vhOld = aOldValueHistories[iRow];
                          DateTime attrStartDate = vhOld.StartDate;
                          DateTime attrEndDate = vhOld.EndDate;

                          // if this vh matches the effective date
                          if (attrStartDate <= effectiveDate && effectiveDate <= attrEndDate)
                          {
                            // then terminate the vh by setting the end date to the day before the effective date
                            vhOld.EndDate = effectiveDate.AddDays(-1);
                            // and record who did this and when
                            vhOld.LastModifiedBy = user;
                            vhOld.LastModifiedDateTime = DateTime.Now;
                            attributeDB.SaveAttribute(entity, itemType, item, aOld.OriginalID, vhOld.ValueType, vhOld.Value, vhOld.StartDate, vhOld.EndDate, vhOld.LastModifiedBy, vhOld.LastModifiedDateTime);
                          }
                          // note: if we don't find a match on the effective date, then there are no current vh records to terminate, so we do nothing
                        }
                      }

                    }
                    // 
                  }
                  else
                  {
                    if ((aNew.Dirty && !aNew.ReadOnly) || includeCleanAttributes)   // has this TAGAttribute been changed (LLA: and is not readonly), or are we saving clean attributes?
                    {
                      string aID = aNew.OriginalID;  //name of the TAGAttribute
                      TAGAttribute aOld = null;
                      if (oldItem == null)  // load the item if we have not already done so
                      {
                        oldItem = new Item();
                        oldItem.ID = item;
                      }
                      if (!oldItem.Attributes.Contains(aID))
                        SaveAttribute(attributeDB, entity, itemType, item, user, aNew); 
                      else
                      {
                        aOld = oldItem.Attributes[aID];  // a is new attribute, aOld is the old one
                        //The attribute exists in both old and new, so we just recreate the valuehistory!
                        // now test for our new "merge" condition
                        if (aNew.Values.Count == 1 && aNew.Values[0].StartDate.Equals(aNew.EffectiveDate))
                        {
                          if (aOld.Values.Count > 0)
                          {
                            // now we merge the histories
                            ValueHistory vhNew = aNew.Values[0]; // we've already established that there is exactly one
                            DateTime newStartdate = vhNew.StartDate;
                            bool holeFound = false;
                            DateTime holeEndDate = TAGFunctions.FutureDateTime.AddDays(-1);
                            aOld.Values.Sort(true); // sort the old history newest date first
                            foreach (ValueHistory vhOld in aOld.Values)
                            {

                              if (vhOld.EndDate < newStartdate && newStartdate < holeEndDate) // is this the "hole"?
                              {
                                vhNew.EndDate = holeEndDate;  // terminate the new vh with the last date in the hole
                                holeFound = true;
                                break;
                              }
                              else
                                if (newStartdate >= vhOld.StartDate) // we will not find a hole, cause we are now looking at older histories than the new record
                                  break;
                                else
                                  holeEndDate = vhOld.StartDate.AddDays(-1);
                            }
                            if (holeFound)
                            {
                              // now add all of the old history "around" the new history 
                              aOld.Values.Sort(); // resort from lower to higher dates
                              foreach (ValueHistory vhOld in aOld.Values)
                                aNew.Values.Add(vhOld);
                            }
                          }
                        }
                      }
                      SaveAttribute(attributeDB, entity, itemType, item, user, aNew);
                    }
                  }
                }
              }
            }
          }
        }
      }
    }

    private TEntity[] getEntities(
      DataSet ds, 
      Hashtable entitiesRequested, 
      int maxItems, 
      DateTime effectiveDate, 
      bool readEntityFields, 
      bool IsRawMode)
    {
      ProcessorCoordinator ProcessorCoordinator = new ProcessorCoordinator();
      Hashtable e = new Hashtable();
      ArrayList ret = new ArrayList();

      TIndexItem iIndexItem = null;

      bool InNewEntity = false;

      TEntity eObj;

      TItemType itemTypeObjIdx;
      TItem itemObjIdx;

      TItemType itemTypeObjObj;
      TItem itemObjObj;

      TEntity tmpEntity = null;
      TItem itemConditionIdx = null;
      TItem itemConditionObj = null;
      TItem conditional = null;

      string itemTypeId = "";

      string entityOwnerId = "";
      int c_i = 0;

      string defaultEntityItemHash = "";
      string currentEntityDefaultItemHash = "";
      string defaultEntityDefaultItemHash = "";

      foreach (DataRow row in ds.Tables[0].Rows)
      {
        if ((string)getValue("", row["Entity"]) == "")
          continue;

        if (e.ContainsKey((string)row["Entity"]))
        {
          InNewEntity = false;
          eObj = (TEntity)e[(string)row["Entity"]];

          c_i++;
        }
        else
        {
          InNewEntity = true;

          eObj = new TEntity();
          eObj.ItemCount = 0;
          eObj.EffectiveDate = effectiveDate;
          eObj.Id = (string)row["Entity"];
          eObj.OrigId = (string)row["EntityOrigId"];
          eObj.HierarchyPosition = (int)row["Level"];
          
          eObj.Items = new TItem[maxItems];
          e.Add(eObj.Id, eObj);
          ret.Add(eObj);

          entityOwnerId = (string)getValue("", row["EntityOwner"]);
          
          if (entityOwnerId != "" && e.ContainsKey(entityOwnerId))
              eObj.EntityOwner = (TEntity)e[entityOwnerId];

          c_i = 0;
        }

        //ADD or Set the ItemType on the ItemTypeIndex for the Entity
        if (eObj.ItemTypeIndex.Contains(eObj.Id + "." + (string)row["ItemType"]))
        {
          itemTypeObjIdx = (TItemType)((TIndexItem)eObj.ItemTypeIndex[eObj.Id + "." + (string)row["ItemType"]]).ItemIdx;
          itemTypeObjObj = (TItemType)((TIndexItem)eObj.ItemTypeIndex[eObj.Id + "." + (string)row["ItemType"]]).ItemObj;
        }
        else
        {
          itemTypeObjIdx = new TItemType();

          itemTypeObjIdx.Id = (string)row["ItemType"];
          itemTypeObjIdx.OrigId = (string)row["ItemTypeOrigId"];
          itemTypeObjIdx.Entity = eObj;

          itemTypeObjObj = new TItemType();

          itemTypeObjObj.Id = (string)row["ItemType"];
          itemTypeObjObj.OrigId = (string)row["ItemTypeOrigId"];
          itemTypeObjObj.Entity = eObj;

          eObj.ItemTypeIndex.Add(new TIndexItem() { ItemObj = itemTypeObjObj, ItemIdx = itemTypeObjIdx });
        }

        itemObjIdx = new TItem();
        itemObjIdx.Id = (string)row["Item"];
        itemObjIdx.Entity = eObj;
        itemObjIdx.ItemType = itemTypeObjIdx;

        //Since in a multiple entity call we can get the same item more than once, we skip duplicates! since the data is already in...
        //This was discovered by an error of duplicates reported by Larry and Michelle
        if (!(eObj.ItemIndex.Contains(itemObjIdx.ItemHash)))
        {
          itemObjIdx.OrigId = (string)row["ItemOrigId"];
          itemObjIdx.IsVirtual = ((int)getValue(0, row["Virtual"]) == 0 ? false : true);
          itemObjIdx.LastModifiedBy = (string)getValue("", row["LastModifiedBy"]);
          itemObjIdx.LastModifiedDateTime = (DateTime)getValue(DateTime.Now, row["LastModifiedDateTime"]);

          getItemHistory((string)getValue(null, row["ItemHistory"]), itemObjIdx);

          itemObjObj = new TItem() { Id = itemObjIdx.Id, OrigId = itemObjIdx.OrigId, ItemHistory = itemObjIdx.ItemHistory, Entity = eObj, ItemType = itemTypeObjObj };
          getXmlAttributes((string)getValue(null, row["Attributes"]), itemObjIdx, itemObjObj, IsRawMode);

          if (!(IsRawMode) && itemObjIdx.Entity.Id != Constants.DEFAULT_ENTITY && itemObjIdx.Entity.Id != Constants.DICTIONARY)
          {
            if (!(itemObjIdx.Id.StartsWith(Constants.CONDITIONAL_ITEM)))
            {
              //Get all the attributes, from Default.(iObj.ItemType.id).(iObj.id) and from Default.(iObj.ItemType.id).Default
              currentEntityDefaultItemHash = eObj.Id + "." + itemObjIdx.ItemType.Id + "." + Constants.DEFAULT_ITEM;
              defaultEntityItemHash = Constants.DEFAULT_ENTITY + "." + itemObjIdx.ItemType.Id + "." + itemObjIdx.Id;
              defaultEntityDefaultItemHash = Constants.DEFAULT_ENTITY + "." + itemObjIdx.ItemType.Id + "." + Constants.DEFAULT_ITEM;

              itemObjIdx.Includes.Add(
                new TIncludeItem() 
                { 
                  IncludeSource = itemObjIdx.ItemHash, 
                  IncludeHash = currentEntityDefaultItemHash, 
                  IncludeDepth = 0,
                  DefaultInclude = true
                }); //Local Include

              itemObjIdx.Includes.Add(
                new TIncludeItem() 
                { 
                  IncludeSource = itemObjIdx.ItemHash, 
                  IncludeHash = defaultEntityItemHash, 
                  IncludeDepth = 0,
                  DefaultInclude = true
                }); //Default Entity
              
              itemObjIdx.Includes.Add(
                new TIncludeItem() 
                { 
                  IncludeSource = itemObjIdx.ItemHash, 
                  IncludeHash = defaultEntityDefaultItemHash, 
                  IncludeDepth = 0,
                  DefaultInclude = true
                }); //Default Item

              itemObjObj.Includes.Add(
                new TIncludeItem() 
                { 
                  IncludeSource = itemObjIdx.ItemHash, 
                  IncludeHash = currentEntityDefaultItemHash, 
                  IncludeDepth = 0,
                  DefaultInclude = true
                }); //Local Include

              itemObjObj.Includes.Add(
                new TIncludeItem() 
                { 
                  IncludeSource = itemObjIdx.ItemHash, 
                  IncludeHash = defaultEntityItemHash, 
                  IncludeDepth = 0,
                  DefaultInclude = true
                }); //Default Entity
              
              itemObjObj.Includes.Add(
                new TIncludeItem() 
                { 
                  IncludeSource = itemObjIdx.ItemHash, 
                  IncludeHash = defaultEntityDefaultItemHash, 
                  IncludeDepth = 0,
                  DefaultInclude = true
                }); //Default Item
            }

            //We see if my parent had any conditionals in itself!
            if (InNewEntity && eObj.EntityOwner != null && eObj.EntityOwner.ConditionalList.Count > 0)
            {
              for (int j = 0; j < eObj.EntityOwner.ConditionalList.Count; j++)
              {
                conditional = (TItem)eObj.EntityOwner.ConditionalList[j];
                if (!eObj.ConditionalList.Contains(conditional))
                  eObj.ConditionalList.Add(conditional);
              }
            }

            //We do not need to process the condition, but we will need to save it for the bottom level hierarchy Entitites
            if (itemObjIdx.Id.StartsWith(Constants.CONDITIONAL_ITEM))
            {
              eObj.ConditionalList.Add(itemObjObj);
              //eObj.ConditionalList.Add(iObjIdx);
            }
          }

          iIndexItem = new TIndexItem() { ItemObj = itemObjObj, ItemIdx = itemObjIdx };
          eObj.ItemIndex.Add(iIndexItem);

          //TODO: Begin: Business Logic! must review if this goes here...
          if (!(IsRawMode) && itemObjIdx.Includes.Count > 0)
          {
            IncludeProcessor ip = new IncludeProcessor();
            ProcessorCoordinator.QueueInclude(ip.CreateDelegate(iIndexItem));
          }

          eObj.ItemCount++;
          eObj.Items[c_i] = itemObjIdx;
        }
      }

      ds.Clear();

      //TODO: Begin: Business Logic! must review if this goes here...
      if (!(IsRawMode))
      {
        foreach (TEntity entity in ret)
        {
          //if (entity.HierarchyPosition == 0 && entity.ConditionalList.Count > 0 && entity.Id != Constants.DEFAULT_ENTITY && entity.Id != Constants.DICTIONARY)
          if (entity.ConditionalList.Count > 0 
            && entity.Id != Constants.DEFAULT_ENTITY 
            && entity.Id != Constants.DICTIONARY 
            && entitiesRequested != null && entitiesRequested.ContainsKey(entity.Id))
          {
            //TODO:We loop through the bottomlevel identified entities to populate the conditionals arraylist, which contains a list of the items that will
            //be affected by the conditionalsList that this entity has inherited, or that has defined by his own
            //NOTE: We need to ask IF the conditional MUST be sorted first, base on the conditional Item Id only! this is NOT what we have here, yet
            for (int i = 0; i < entity.ConditionalList.Count; i++)
            {
              //conditional = (TItem)tmpEntity.ItemIndex[condition];
              conditional = (TItem)entity.ConditionalList[i];
              if (conditional != null)
              {
                itemTypeId = conditional.ItemHash.Substring(conditional.ItemHash.IndexOf(".") + 1);
                itemTypeId = itemTypeId.Substring(0, itemTypeId.IndexOf("."));

                //Walking the hierarchy, to find Items to whom to apply the condition
                tmpEntity = entity;
                while (tmpEntity != null)
                {
                  foreach (TIndexItem idxItem in tmpEntity.ItemIndex)
                  {
                    itemConditionIdx = (TItem)idxItem.ItemIdx;
                    itemConditionObj = (TItem)idxItem.ItemObj;
                    if (itemConditionIdx.ItemType.Id == itemTypeId && !itemConditionIdx.Id.StartsWith(Constants.CONDITIONAL_ITEM) && !itemConditionIdx.Id.StartsWith(Constants.DEFAULT_ITEM))
                    {
                      //We must create a collection like this, (iObj, item) and this will be our new eObj.Conditional collection
                      if (!entity.ConditionalIndex.Contains(conditional.Id + ":" + itemConditionIdx.ItemHash))
                        entity.ConditionalIndex.Add(new TIndexItem { ItemIdx = new TIndexItem() { ItemObj = itemConditionObj, ItemIdx = itemConditionIdx }, ItemObj = conditional, IsConditionalItem = true });
                    }
                  }

                  tmpEntity = tmpEntity.EntityOwner;
                }
              }
            }

            ConditionalProcessor cp = new ConditionalProcessor();
            ProcessorCoordinator.QueueConditional(cp.CreateDelegate(entity));
          }
          //TODO: End: Business Logic! must review if this goes here...
        }

        ProcessorCoordinator.StartInclude(Constants.QueueBlockSize);
        ProcessorCoordinator.StartConditional(Constants.QueueBlockSize);
      }

      //Let's load the fields of each Entity...
      if (readEntityFields && ret.Count > 0)
        ret = getEntityFields(ret);

      return (TEntity[])ret.ToArray(typeof(TEntity));
    }

    private void getItemHistory(string xml, TItem iObj)
    {
      string localXML = ((xml == null || xml == string.Empty) ? "<ItemDates><ItemHistory startdate='" + Constants.PASTDATETIME.ToShortDateString() + "'/></ItemDates>" : xml);

      TItemHistory tItemHistory = null;

      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.LoadXml(localXML);

      XmlNodeList xmlItemHistoryList = xmlDoc.SelectNodes("//ItemHistory");
      foreach (XmlNode xmlItemDate in xmlItemHistoryList)
      {
        tItemHistory = new TItemHistory();
        tItemHistory.StartDate = Constants.PASTDATETIME;
        tItemHistory.EndDate = Constants.FUTUREDATETIME;

        foreach (XmlAttribute xmlItemHistoryAttr in xmlItemDate.Attributes)
        {
          switch (xmlItemHistoryAttr.Name.ToLower())
          {
            case Constants.STARTDATE:
              if (xmlItemHistoryAttr.Value != null)
                tItemHistory.StartDate = Convert.ToDateTime(xmlItemHistoryAttr.Value);
              break;
            case Constants.ENDDATE:
              if (xmlItemHistoryAttr.Value != null)
                tItemHistory.EndDate = Convert.ToDateTime(xmlItemHistoryAttr.Value);
              break;
          }
        }

        //TODO: ItemHistory Id cannot be duplicated, if this is the case the DB has wrong data!
        if(!(iObj.ItemHistory.Contains(tItemHistory.Id)))
          iObj.ItemHistory.Add(tItemHistory.Id, tItemHistory);
      }
    }

    private ArrayList getEntityFields(ArrayList ret)
    {
      TEntity eObj = null;
      DataSet entityFields = null;
      Hashtable entityIndex = new Hashtable();

      string[] entities = new string[ret.Count];
      int i = 0;

      foreach (TEntity entity in ret)
      {
        entityIndex.Add(entity.Id, entity);
        entities[i++] = entity.Id;
      }

      entityFields = new AttributeDB().getEntityFields(entities);

      foreach (DataRow row in entityFields.Tables[0].Rows)
      {
        if ((string)getValue("", row["Entity"]) == "")
          continue;

        eObj = (TEntity)entityIndex[((string)row["Entity"]).ToLower()];

        if (eObj != null)
        {
          eObj.AlternateID = (string)getValue("", row["AlternateID"]);
          eObj.AlternateName = (string)getValue("", row["AlternateName"]);
          eObj.EndDate = (DateTime)getValue(DateTime.Now, row["EndDate"]);
          eObj.EntityType = (string)getValue("", row["EntityType"]);
          eObj.FEIN = (string)getValue("", row["FEIN"]);
          eObj.FirstName = (string)getValue("", row["FirstName"]);
          eObj.LastModifiedBy = (string)getValue("", row["LastModifiedBy"]);
          eObj.LastModifiedDateTime = (DateTime)getValue(DateTime.Now, row["LastModifiedDateTime"]);
          eObj.LegalName = (string)getValue("", row["LegalName"]);
          eObj.MiddleName = (string)getValue("", row["MiddleName"]);
          eObj.StartDate = (DateTime)getValue(DateTime.Now, row["StartDate"]);

          eObj.Address1 = (string)getValue("", row["Address1"]);
          eObj.Address2 = (string)getValue("", row["Address2"]);
          eObj.City = (string)getValue("", row["City"]);
          eObj.State = (string)getValue("", row["State"]);
          eObj.Zip = (string)getValue("", row["Zip"]);

          TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.setFullName, eObj);   // set the full name and short name virtual fields
          TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.setClientName, eObj); //set the client name virtual field.

        }
      }

      return ret;
    }

    private void getXmlAttributes(string xml, TItem itemObjIdx, TItem itemObjObj, bool IsRawMode)
    {
      if (xml == null || xml == string.Empty)
        return; //null;

      //TAttribute[] retA = null;
      ArrayList retIdx = new ArrayList();
      ArrayList retObj = new ArrayList();
      
      TAttribute tAttrIdx = null;
      TValueHistory tAttr_vhIdx = null;

      TAttribute tAttrObj = null;
      TValueHistory tAttr_vhObj = null;
      
      XmlDocument xmlDoc = new XmlDocument();
      string includeValue = "";
      bool expiredAttribute = true;

      int vhIdx_i = 0;
      int vhObj_i = 0;

      try
      {
        xmlDoc.LoadXml(xml);
      }
      catch(Exception ex)
      {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.StackTrace);
        return;
      }

      XmlNodeList xmlAttrList = xmlDoc.SelectNodes("//attribute");
      foreach (XmlNode attr in xmlAttrList)
      {
        if (attr.Attributes["name"] != null)
        {
          expiredAttribute = true;

          //Reference attribute, as it comes from the DB...
          tAttrIdx = new TAttribute();
          tAttrIdx.Item = itemObjIdx;
          tAttrIdx.Id = attr.Attributes["name"].Value.ToLower();
          tAttrIdx.OrigId = attr.Attributes["name"].Value;

          //Context Attribute...
          tAttrObj = new TAttribute();
          tAttrObj.Item = itemObjObj;
          tAttrObj.Id = attr.Attributes["name"].Value.ToLower();
          tAttrObj.OrigId = attr.Attributes["name"].Value;

          vhIdx_i = 0;
          vhObj_i = 0;
          if (attr.ChildNodes.Count != 0)
          {
            tAttrIdx.History = new TValueHistory[attr.ChildNodes.Count];
            tAttrObj.History = new TValueHistory[attr.ChildNodes.Count];
            
            foreach (XmlNode vh in attr.ChildNodes)
            {
              tAttr_vhIdx = new TValueHistory();
              tAttr_vhIdx.ValueType = Constants.VALUE;
              tAttr_vhIdx.StartDate = Constants.PASTDATETIME;
              tAttr_vhIdx.EndDate = Constants.FUTUREDATETIME;

              tAttr_vhObj = new TValueHistory();
              tAttr_vhObj.ValueType = Constants.VALUE;
              tAttr_vhObj.StartDate = Constants.PASTDATETIME;
              tAttr_vhObj.EndDate = Constants.FUTUREDATETIME;

              foreach (XmlAttribute vh_attr in vh.Attributes)
              {
                switch (vh_attr.Name.ToLower())
                {
                  case Constants.STARTDATE:
                    if (vh_attr.Value != null)
                    {
                      tAttr_vhIdx.StartDate = Convert.ToDateTime(vh_attr.Value);
                      tAttr_vhObj.StartDate = Convert.ToDateTime(vh_attr.Value);
                    }
                    break;
                  case Constants.ENDDATE:
                    if (vh_attr.Value != null)
                    {
                      tAttr_vhIdx.EndDate = Convert.ToDateTime(vh_attr.Value);
                      tAttr_vhObj.EndDate = Convert.ToDateTime(vh_attr.Value);
                    }
                    break;
                  case Constants.LASTMODIFIEDBY:
                    tAttr_vhIdx.LastModifiedBy = vh_attr.Value;
                    tAttr_vhObj.LastModifiedBy = vh_attr.Value;
                    break;
                  case Constants.LASTMODIFIEDDATETIME:
                    tAttr_vhIdx.LastModifiedDateTime = Convert.ToDateTime(vh_attr.Value);
                    tAttr_vhObj.LastModifiedDateTime = Convert.ToDateTime(vh_attr.Value);
                    break;
                  case Constants.INCLUDE:
                    tAttrIdx.Flags = Constants.SetOn(tAttrIdx.Flags, EAttributeFlags.IsAnInclude);
                    tAttr_vhIdx.ValueType = vh_attr.Name.ToLower();

                    tAttr_vhIdx.Value =
                      ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString,
                      TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, vh_attr.Value))).ToLower();

                    tAttrObj.Flags = Constants.SetOn(tAttrObj.Flags, EAttributeFlags.IsAnInclude);
                    tAttr_vhObj.ValueType = vh_attr.Name.ToLower();

                    tAttr_vhObj.Value =
                      ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString,
                      TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, vh_attr.Value))).ToLower();
                    
                    break;
                  case Constants.REF_INHERIT:
                    tAttrIdx.Flags = Constants.SetOn(tAttrIdx.Flags, EAttributeFlags.IsRefValue);
                    tAttr_vhIdx.ValueType = vh_attr.Name.ToLower();

                    tAttr_vhIdx.Value =
                      ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString,
                      TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, vh_attr.Value))).ToLower();

                    tAttr_vhIdx.ReferenceValueSource = TAGFunctions.REF_INHERIT + " = "
                      + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, vh_attr.Value));

                    tAttrObj.Flags = Constants.SetOn(tAttrObj.Flags, EAttributeFlags.IsRefValue);
                    tAttr_vhObj.ValueType = vh_attr.Name.ToLower();

                    tAttr_vhObj.Value =
                      ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString,
                      TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, vh_attr.Value))).ToLower();

                    tAttr_vhObj.ReferenceValueSource = TAGFunctions.REF_INHERIT + " = "
                      + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, vh_attr.Value));

                    break;
                  case Constants.FUNCTION:
                    tAttrIdx.Flags = Constants.SetOn(tAttrIdx.Flags, EAttributeFlags.IsFunctionValue);
                    tAttr_vhIdx.ValueType = vh_attr.Name.ToLower();

                    tAttr_vhIdx.Value =
                      ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString,
                      TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, vh_attr.Value)));

                    tAttr_vhIdx.ReferenceValueSource = TAGFunctions.FUNCTION + " = "
                      + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, vh_attr.Value));

                    tAttrObj.Flags = Constants.SetOn(tAttrObj.Flags, EAttributeFlags.IsFunctionValue);
                    tAttr_vhObj.ValueType = vh_attr.Name.ToLower();

                    tAttr_vhObj.Value =
                      ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString,
                      TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, vh_attr.Value)));

                    tAttr_vhObj.ReferenceValueSource = TAGFunctions.FUNCTION + " = "
                      + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, vh_attr.Value));

                    break;
                  default:
                    tAttr_vhIdx.ValueType = vh_attr.Name.ToLower();
                    tAttr_vhIdx.Value = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, vh_attr.Value));

                    tAttr_vhObj.ValueType = vh_attr.Name.ToLower();
                    tAttr_vhObj.Value = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, vh_attr.Value));
                    
                    break;
                }
              }

              switch (tAttr_vhIdx.ValueType)
              {
                //TableNoHeader is deprecated, we change it to Value whenever we find it in the Data
                case TAGFunctions.TABLENOHEADER:
                  tAttr_vhIdx.ValueType = Constants.VALUE;
                  tAttr_vhObj.ValueType = Constants.VALUE;
                  break;
                //TableMod is deprecated, we change it to TableHeader whenever we find it in the Data
                case TAGFunctions.TABLEMOD:
                  tAttr_vhIdx.ValueType = Constants.TABLEHEADER;
                  tAttr_vhObj.ValueType = Constants.TABLEHEADER;
                  tAttrIdx.Flags = Constants.SetOn(tAttrIdx.Flags, EAttributeFlags.IsTableHeader);
                  tAttrObj.Flags = Constants.SetOn(tAttrObj.Flags, EAttributeFlags.IsTableHeader);
                  if (tAttrIdx.Id == TAGFunctions.GENERATE_TC)
                  {
                    tAttrIdx.Flags = Constants.SetOn(tAttrIdx.Flags, EAttributeFlags.IsTransCodeGenerator);
                    tAttrObj.Flags = Constants.SetOn(tAttrObj.Flags, EAttributeFlags.IsTransCodeGenerator);
                  }
                  break;
                case TAGFunctions.TABLEHEADER:
                  tAttrIdx.Flags = Constants.SetOn(tAttrIdx.Flags, EAttributeFlags.IsTableHeader);
                  tAttrObj.Flags = Constants.SetOn(tAttrObj.Flags, EAttributeFlags.IsTableHeader);
                  if (tAttrIdx.Id == TAGFunctions.GENERATE_TC)
                  {
                    tAttrIdx.Flags = Constants.SetOn(tAttrIdx.Flags, EAttributeFlags.IsTransCodeGenerator);
                    tAttrObj.Flags = Constants.SetOn(tAttrObj.Flags, EAttributeFlags.IsTransCodeGenerator);
                  }
                  break;
              }

              tAttrIdx.History[vhIdx_i++] = tAttr_vhIdx;
              tAttrObj.History[vhObj_i++] = tAttr_vhObj;

              if (expiredAttribute && (tAttr_vhIdx.StartDate <= itemObjIdx.Entity.EffectiveDate && itemObjIdx.Entity.EffectiveDate <= tAttr_vhIdx.EndDate))
                expiredAttribute = false;
            }
          }
          else
          {
            //If NO valueHistory comes from the DB we create a new valueHistory with the default values
            tAttrIdx.History = new TValueHistory[1];
            tAttr_vhIdx = new TValueHistory();
            tAttr_vhIdx.ValueType = Constants.VALUE;
            tAttr_vhIdx.Value = String.Empty;
            tAttr_vhIdx.StartDate = Constants.PASTDATETIME;
            tAttr_vhIdx.EndDate = Constants.FUTUREDATETIME;
            tAttr_vhIdx.LastModifiedBy = string.Empty;
            tAttr_vhIdx.LastModifiedDateTime = DateTime.Now;

            tAttrIdx.History[vhIdx_i] = tAttr_vhIdx;

            //If NO valueHistory comes from the DB we create a new valueHistory with the default values
            tAttrObj.History = new TValueHistory[1];
            tAttr_vhObj = new TValueHistory();
            tAttr_vhObj.ValueType = Constants.VALUE;
            tAttr_vhObj.Value = String.Empty;
            tAttr_vhObj.StartDate = Constants.PASTDATETIME;
            tAttr_vhObj.EndDate = Constants.FUTUREDATETIME;
            tAttr_vhObj.LastModifiedBy = string.Empty;
            tAttr_vhObj.LastModifiedDateTime = DateTime.Now;

            tAttrObj.History[vhObj_i] = tAttr_vhObj;

            expiredAttribute = false;
          }

          //TODO: This IF must NOT process entity ALL!
          //if (!expiredAttribute || iObj.Entity.Id == Constants.DEFAULT_ENTITY || iObj.Entity.Id == Constants.DICTIONARY)
          if (!expiredAttribute)
          {
            //Set the valueType to the last valid valueHistory valueType
            //tattr.ValueType = valueType;  
            if (!(IsRawMode) && Constants.AnyOn(tAttrIdx.Flags, EAttributeFlags.IsAnInclude))
            {
              if (tAttrIdx.Value != null)
              {
                includeValue = tAttrIdx.Value.ToString().Trim().ToLower();
                if (includeValue != "")
                {
                  TIncludeItem tinclItem =
                    new TIncludeItem()
                    {
                      IncludeSource = itemObjIdx.ItemHash,
                      IncludeHash = includeValue,
                      IncludeDepth = 0,
                      DefaultInclude = false
                    };

                  itemObjIdx.Includes.Add(tinclItem);
                  itemObjObj.Includes.Add(tinclItem);
                }
              }
            }
            else
            {
              //TODO: We CANNOT allow duplicate Attributes, if this is the case, the DB data is wrong!!
              TIndexItem tindexItem =
                  new TIndexItem()
                  {
                    ItemObj = tAttrObj,
                    ItemIdx = tAttrIdx
                  };

              if (!(itemObjIdx.AttributeIndex.Contains(tAttrIdx.AttributeHash)))
              {
                itemObjIdx.AttributeIndex.Add(tindexItem);
                itemObjObj.AttributeIndex.Add(tindexItem);
              }
            }

            tAttrIdx.Flags = Constants.SetOn(tAttrIdx.Flags, EAttributeFlags.IsLocal);
            if (itemObjIdx.Entity.Id == Constants.DEFAULT_ENTITY)
              tAttrIdx.Flags = Constants.SetOn(tAttrIdx.Flags, EAttributeFlags.FromDefaultEntity);
            if (itemObjIdx.Id == Constants.DEFAULT_ITEM)
              tAttrIdx.Flags = Constants.SetOn(tAttrIdx.Flags, EAttributeFlags.FromDefaultItem);

            tAttrObj.Flags = Constants.SetOn(tAttrObj.Flags, EAttributeFlags.IsLocal);
            if (itemObjObj.Entity.Id == Constants.DEFAULT_ENTITY)
              tAttrObj.Flags = Constants.SetOn(tAttrObj.Flags, EAttributeFlags.FromDefaultEntity);
            if (itemObjObj.Id == Constants.DEFAULT_ITEM)
              tAttrObj.Flags = Constants.SetOn(tAttrObj.Flags, EAttributeFlags.FromDefaultItem);

            retIdx.Add(tAttrIdx);
            retObj.Add(tAttrObj);
          }
        }
      }

      itemObjIdx.Attributes = new TAttribute[Constants.MaxEntityAttributes];
      Array.Copy((TAttribute[])retIdx.ToArray(typeof(TAttribute)), itemObjIdx.Attributes, retIdx.Count);

      itemObjObj.Attributes = new TAttribute[Constants.MaxEntityAttributes];
      Array.Copy((TAttribute[])retObj.ToArray(typeof(TAttribute)), itemObjObj.Attributes, retObj.Count);

      //return retA;
    }

    private DataSet addVirtualItems(DataSet ds, Hashtable itemTypesRequested, Hashtable itemsRequested)
    {
      //For the virtual items, we will add them to the DATASET, this way, all the process will be the same
      //so we will modify the dataset in this method, for this we will create a temporary dataset that will be populated
      //with all the missing items, when it applies, this is, when the itemsRequestesd list is not null and the retreiveVirtualItems is true
      //TODO: I have to create this INDEPENDENT of POSITIONAL problems, so I must preferably create a row object
      //and add the value to it, so as no to have problems when adding columns...
      const string cNO_OWNER = "(NoOwner)";

      Hashtable entitiesRequested = new Hashtable();
      Hashtable requestedEntitiesItemTypesItems = new Hashtable();
      Hashtable obtainedEntitiesItemTypesItems = new Hashtable();
      
      string requestedEntitiesItemTypesItemsKey = String.Empty;
      string obtainedEntitiesItemTypesItemsKey = String.Empty;

      string[] reqEntryKey = null;
      string[] reqEntryValue = null;

      DataSet dsVirtuals = new DataSet();

      DataTable virtualItems = null;
      DataRow[] rows = null;
      DataRow row = null;

      Hashtable processedItems = null;

      string entityID = "";
      string entityOrigID = "";
      string entityOwnerID = "";
      string entityOwnerOrigID = "";
      string itemTypeID = "";
      string itemTypeOrigID = "";
      string itemID = "";
      string itemOrigID = "";
      int IsVirtual = 1;

      int i = 0;

      processedItems = new Hashtable();

      virtualItems = dsVirtuals.Tables.Add();

      virtualItems.Columns.Add("Entity", typeof(string));
      virtualItems.Columns.Add("EntityOrigId", typeof(string));
      virtualItems.Columns.Add("EntityOwner", typeof(string));
      virtualItems.Columns.Add("EntityOwnerOrigId", typeof(string));
      virtualItems.Columns.Add("ItemType", typeof(string));
      virtualItems.Columns.Add("ItemTypeOrigId", typeof(string));
      virtualItems.Columns.Add("Item", typeof(string));
      virtualItems.Columns.Add("ItemOrigId", typeof(string));
      virtualItems.Columns.Add("Virtual", typeof(int));

      rows = ds.Tables[0].Select("Level = 0");
      for (i = 0; i < rows.GetLength(0); i++)
      {
        row = rows[i];

        if ((int)row["Level"] > Constants.BOTTOMLEVEL)
          continue;

        entityID = (string)row["Entity"];
        entityOrigID = (string)row["EntityOrigId"];
        itemTypeID = (string)row["ItemType"];
        itemTypeOrigID = (string)row["ItemTypeOrigId"];
        itemID = (string)row["Item"];
        itemOrigID = (string)row["ItemOrigId"];

        entityOwnerID = (string)getValue(cNO_OWNER, row["EntityOwner"]);
        entityOwnerOrigID = (string)getValue(cNO_OWNER, row["EntityOwnerOrigId"]);

        if (!(entitiesRequested.ContainsKey(entityID)))
          entitiesRequested.Add(entityID, String.Format("{0}.{1}", entityOrigID, entityOwnerOrigID));

        obtainedEntitiesItemTypesItemsKey = String.Format("{0}.{1}.{2}", entityID, itemTypeID, itemID);

        if (!(obtainedEntitiesItemTypesItems.ContainsKey(obtainedEntitiesItemTypesItemsKey)))
          obtainedEntitiesItemTypesItems.Add(obtainedEntitiesItemTypesItemsKey, String.Format("{0}.{1}.{2}.{3}", entityOrigID, itemTypeOrigID, itemOrigID, entityOwnerOrigID));
      }

      foreach (DictionaryEntry dictEntity in entitiesRequested)
      {
        reqEntryValue = ((string)dictEntity.Value).Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

        entityID = (string)dictEntity.Key;
        entityOrigID = reqEntryValue[0];
        entityOwnerOrigID= reqEntryValue[1];

        foreach (DictionaryEntry dictItemType in itemTypesRequested)
        {
          itemTypeID = (string)dictItemType.Key;
          itemTypeOrigID = (string)dictItemType.Value;

          foreach (DictionaryEntry dictItem in itemsRequested)
          {
            itemID = (string)dictItem.Key;
            itemOrigID = (string)dictItem.Value;

            requestedEntitiesItemTypesItemsKey = String.Format("{0}.{1}.{2}", entityID, itemTypeID, itemID);

            if (!(requestedEntitiesItemTypesItems.ContainsKey(requestedEntitiesItemTypesItemsKey)))
              requestedEntitiesItemTypesItems.Add(requestedEntitiesItemTypesItemsKey, String.Format("{0}.{1}.{2}.{3}", entityOrigID, itemTypeOrigID, itemOrigID, entityOwnerOrigID));
          }
        }
      }

      foreach (DictionaryEntry dictReq in requestedEntitiesItemTypesItems)
      {
        requestedEntitiesItemTypesItemsKey = (string)dictReq.Key;
        
        if (obtainedEntitiesItemTypesItems.ContainsKey(requestedEntitiesItemTypesItemsKey))
          continue;

        //So the requested entity.itemtype.item was NOT found in the DB, but we are asking for virtual items, 
        //so we must create this combination and mark it as a virtual item
        reqEntryKey = requestedEntitiesItemTypesItemsKey.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
        reqEntryValue = ((string)dictReq.Value).Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

        entityID = reqEntryKey[0];
        entityOrigID = reqEntryValue[0];

        itemTypeID = reqEntryKey[1];
        itemTypeOrigID = reqEntryValue[1];

        itemID = reqEntryKey[2];
        itemOrigID = reqEntryValue[2];

        entityOwnerID = (reqEntryValue[3] == cNO_OWNER ? "" : reqEntryValue[3]).ToLower();
        entityOwnerOrigID = (reqEntryValue[3] == cNO_OWNER ? "" : reqEntryValue[3]);

        //Here we add the item to the dsVirtuals, using the entityID, itemTypeID and itemID
        virtualItems.Rows.Add(
          entityID, entityOrigID,
          entityOwnerID, entityOwnerOrigID,
          itemTypeID, itemTypeOrigID,
          itemID, itemOrigID, IsVirtual);

      }

      if (virtualItems.Rows.Count > 0)
      {
        //foreach (DataRow row in virtualItems.Rows)
        for (i = 0; i < virtualItems.Rows.Count; i++)
        {
          row = virtualItems.Rows[i];

          entityID = (string)row["Entity"];
          entityOrigID = (string)row["EntityOrigId"];
          entityOwnerID = (string)row["EntityOwner"];
          entityOwnerOrigID = (string)row["EntityOwnerOrigId"];
          itemTypeID = (string)row["ItemType"];
          itemTypeOrigID = (string)row["ItemTypeOrigId"];
          itemID = (string)row["Item"];
          itemOrigID = (string)row["ItemOrigId"];

          ds.Tables[0].Rows.Add(
            entityID, entityOrigID,
            entityOwnerID, entityOwnerOrigID,
            itemTypeID, itemTypeOrigID,
            itemID, itemOrigID,
            null, null, "", DateTime.Now, IsVirtual, Constants.BOTTOMLEVEL);
        }

      }

      return ds;
    }

    private object getValue(object def, object src) { return src is DBNull ? def : src; }

    private Item getRawItem(string entity, string itemType, string item)
    {
      Item i = new Item();
      AttributeDB attributeDB = new AttributeDB();
      string sqlBase ="select * from {0} where entity = '{1}' and ItemType = '{2}' and item = '{3}'";
      string sql = string.Format(sqlBase, "Item", entity, itemType, item);
      DataSet dsItem = attributeDB.getDataFromSQL(sql);
      if (dsItem == null || dsItem.Tables.Count == 0 || dsItem.Tables[0].Rows.Count == 0)
        return null;
      i.ID = item;
      DataRow itemRow = dsItem.Tables[0].Rows[0];
      i.LastModifiedBy = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, itemRow["LastModifiedBy"]);
      i.LastModifiedDateTime = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, itemRow["LastModifiedDateTime"]);
      sql = string.Format(sqlBase, "ItemHistory", entity, itemType, item);
      DataSet dsHistory = attributeDB.getDataFromSQL(sql);
      if (dsHistory != null && dsHistory.Tables.Count > 0 && dsHistory.Tables[0].Rows.Count > 0)
      {
        foreach (DataRow row in dsHistory.Tables[0].Rows)
        {
          ItemHistory ih = new ItemHistory();
          ih.StartDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, row["StartDate"]);
          if (row["EndDate"].Equals(DBNull.Value))
            ih.EndDate = TAGFunctions.FutureDateTime;
          else
            ih.EndDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, row["EndDate"]);
          i.ItemHistoryRecords.Add(ih);
        }
      }
      sql = string.Format(sqlBase, "AttributeNew", entity, itemType, item);
      DataSet dsAttribute = attributeDB.getDataFromSQL(sql);
      Dictionaries dictionary = DictionaryFactory.getInstance().getDictionary();
      if (dsAttribute != null && dsAttribute.Tables.Count > 0 && dsAttribute.Tables[0].Rows.Count > 0)
      {
        TAGAttribute a = null;
        foreach (DataRow row in dsAttribute.Tables[0].Rows)
        {
          string newID = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row["Attribute"]);
          if (a == null || !a.ID.Equals(newID, StringComparison.CurrentCultureIgnoreCase))
          {
            if (a != null)
              i.Attributes.Add(a);
            a = new TAGAttribute();
            a.ID = newID;  
          }
          ValueHistory vh = new ValueHistory();
          vh.StartDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, row["StartDate"]);
          if (row["EndDate"].Equals(DBNull.Value))
            vh.EndDate = TAGFunctions.FutureDateTime;
          else
            vh.EndDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, row["EndDate"]);
          a.ValueType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row["ValueType"]);
          if (string.IsNullOrEmpty(a.ValueType))
            a.ValueType = "value";
          if (!row["Value"].Equals(DBNull.Value))
            vh.Value = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, row["Value"], dictionary.DataType(a.ID));
          vh.LastModifiedBy = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row["LastModifiedBy"]);
          vh.LastModifiedDateTime = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, row["LastModifiedDateTime"]);
          a.Values.Add(vh);
        }
        if (a != null)  // save the last one
          i.Attributes.Add(a);
      }
      return i;
    }

    private void SaveAttribute(AttributeDB attributeDB, string entity, string itemType, string item, string user, TAGAttribute a)
    {
      if (a == null)
        return;
      if (!a.Dirty)
        return;
      // delete the ones that are there
      attributeDB.DeleteAttributes(entity, itemType, item, a.ID);
      if (a.Deleted)
        return; // we deleted it, and that was the flag, so we are done
      // and now save the new versions
      if (a.Values.Count > 0)
      {
        foreach (ValueHistory vh in a.Values)
        {
          if (!vh.Deleted)
          {
            object oEndDate;
            if (vh.EndDate == TAGFunctions.FutureDateTime)
              oEndDate = null;
            else
              oEndDate = vh.EndDate;
            if (vh.Dirty)
              attributeDB.SaveAttribute(entity, itemType, item, a.OriginalID, vh.ValueType, vh.Value, vh.StartDate, oEndDate, user, DateTime.Now);
            else
            {
              if (vh.LastModifiedDateTime < TAGFunctions.PastDateTime)  // null or invalid date
                vh.LastModifiedDateTime = DateTime.Now;
              if (string.IsNullOrEmpty(vh.LastModifiedBy))
                vh.LastModifiedBy = user;
              attributeDB.SaveAttribute(entity, itemType, item, a.OriginalID, vh.ValueType, vh.Value, vh.StartDate, oEndDate, vh.LastModifiedBy, vh.LastModifiedDateTime);
            }
          }
        }
      }
      else
        if (a.Dirty)
          attributeDB.SaveAttribute(entity, itemType, item, a.OriginalID, null, null, TAGFunctions.PastDateTime, null, user, DateTime.Now);
        else
          attributeDB.SaveAttribute(entity, itemType, item, a.OriginalID, null, null, TAGFunctions.PastDateTime, null, a.LastModifiedBy, a.LastModifiedDateTime);
    }

  }

}
