using System;
using System.Collections;

using TAGBOSS.Common;
using TAGBOSS.Common.Logging;
using TAGBOSS.Common.Model;

using TAGBOSS.Sys.AttributeEngine2.Dal;

namespace TAGBOSS.Sys.AttributeEngine2.ConvertToEAC
{
  public class AttributeProcessor
  {
    Log log = (Log)LogFactory.GetInstance().GetLog("TAGBOSS.Sys.AttributeEngine2.ConvertToEAC.AttributeProcessor");

    const string EXCLUDE_YN_ITEM = "excludeyn";  //Constant to identify the attibute that excludes an item (Y/N version)
    const string EXCLUDE_ITEM = "exclude";  //Constant to identify the attibute that excludes an item
    const string BLOCKINHERIT = "blockinherityn";

    //Local class used to process the parameters
    class AttributeProcessorParameters
    {
      internal Parameters parameters = null;

      internal string[] entities = null;
      string[] entitiesReq = null;

      internal string[] itemTypes = null;
      string[] itemTypesReq = null;

      internal string[] items = null;
      string[] itemsReq = null;

      internal DateTime effectiveDate;

      string[] charSeparator = new string[] { "," };

      internal Hashtable entitiesRequested = null;
      internal Hashtable itemTypesRequested = null;
      internal Hashtable itemsRequested = null;
      internal bool processBlockInheritYN = true;

      internal bool setParameters(
        string Entities,
        string ItemTypes,
        string Items,
        string ParameterList,
        DateTime EffectiveDate,
        bool IsRawMode,
        bool RetrieveVirtualItems)
      {
        //We first check all the parameters!!
        if (EffectiveDate != null)
          effectiveDate = DateTime.Parse(EffectiveDate.ToShortDateString());
        else
          effectiveDate = DateTime.Parse(DateTime.Now.ToShortDateString());

        if (Entities != null && Entities.Trim() != "")
        {
          entities = Entities.Replace(" ", "").ToLower().Split(charSeparator, StringSplitOptions.RemoveEmptyEntries);
          entitiesReq = Entities.Replace(" ", "").Split(charSeparator, StringSplitOptions.RemoveEmptyEntries);
        }
        else
          return false;

        if (ItemTypes != null && ItemTypes.Trim() != "")
        {
          itemTypes = ItemTypes.Replace(" ", "").ToLower().Split(charSeparator, StringSplitOptions.RemoveEmptyEntries);
          itemTypesReq = ItemTypes.Replace(" ", "").Split(charSeparator, StringSplitOptions.RemoveEmptyEntries);
        }

        if (Items != null && Items.Trim() != "")
        {
          items = Items.Replace(" ", "").ToLower().Split(charSeparator, StringSplitOptions.RemoveEmptyEntries);
          itemsReq = Items.Replace(" ", "").Split(charSeparator, StringSplitOptions.RemoveEmptyEntries);
        }


        if (ParameterList != null && ParameterList.Trim() != "")
          parameters = new Parameters(ParameterList.Trim());   //Parameters argument in xml format...

        if (entities == null || entities.Length == 0)
          return false;

        //Let us check if the entity ItemType was requested!
        if (entities != null)
        {
          entitiesRequested = new Hashtable();
          for (int i = 0; i < entities.Length; i++)
          {
            if (!(entitiesRequested.ContainsKey(entities[i])))
              entitiesRequested.Add(entities[i], entitiesReq[i]);
          }
        }

        //Let us check if the entity ItemType was requested!
        if (itemTypes != null)
        {
          itemTypesRequested = new Hashtable();
          for (int i = 0; i < itemTypes.Length; i++)
          {
            if (!(itemTypesRequested.ContainsKey(itemTypes[i])))
              itemTypesRequested.Add(itemTypes[i], itemTypesReq[i]);
          }
        }

        //This contains the list of items that WHERE requested!
        //with this list we construct the final EAC that we return, 
        //filtering only the requested Items
        //Let's build the hash of requested items!
        if (items != null)
        {
          itemsRequested = new Hashtable();
          for (int i = 0; i < items.Length; i++)
          {
            if (!(itemsRequested.ContainsKey(items[i])))
              itemsRequested.Add(items[i], itemsReq[i]);
          }
        }

        if (parameters != null && parameters.Contains("ProcessBlockInheritYN"))
          processBlockInheritYN = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, parameters["ProcessBlockInheritYN"]);

        return true;
      }
    }

    #region ConvertToEAC methods, this methods return an old EAC structure

    public EntityAttributesCollection getAttributes(
      String entity, 
      String itemtype, 
      DateTime effective, 
      bool blocked,
      int handle)
    {
      string parameters = "";

      if (blocked)
        parameters = "<parameters><ProcessBlockInheritYN value='False' /></parameters>";
      else
        parameters = "<parameters/>";

      return getAttributes(entity, itemtype, null, parameters, effective, false, false, handle);
    }

    /// <summary>
    /// getAttributes preprocessing routine, here we check the parameters passed, and clean and prepare them up
    /// for them to be used by the main getAttributes routine
    /// </summary>
    /// <param name="Entities"></param>
    /// <param name="ItemTypes"></param>
    /// <param name="Items"></param>
    /// <param name="ParameterList"></param>
    /// <param name="EffectiveDate"></param>
    /// <param name="IsRawMode"></param>
    /// <param name="retrieveVirtualItems"></param>
    /// <returns></returns>
    public EntityAttributesCollection getAttributes(
      string Entities,
      string ItemTypes,
      string Items,
      string ParameterList,
      DateTime EffectiveDate,
      bool IsRawMode,
      bool RetrieveVirtualItems,
      int handle)
    {
      //TODO: Begin: We set a log for any call to the Default entity
      if (Entities.ToLower().Contains(Constants.DEFAULT_ENTITY))
      {
        log.Debug(
          @"public EntityAttributesCollection getAttributes( "
          + "Entities = " + Entities + ","
          + "ItemTypes = " + ItemTypes + ","
          + "Items = " + Items + ","
          + "ParameterList = '" + ParameterList + "',"
          + "EffectiveDate = " + EffectiveDate.ToShortDateString() + ","
          + "IsRawMode = " + (IsRawMode ? "True" : "False") + ","
          + "RetrieveVirtualItems = " + (RetrieveVirtualItems ? "True" : "False") + "))");
      }
      //TODO: End: We set a log for any call to the Default entity
      AttributeProcessorParameters aPP = new AttributeProcessorParameters();
      bool haveGoodParameters = aPP.setParameters(Entities, ItemTypes, Items, ParameterList, EffectiveDate, IsRawMode, RetrieveVirtualItems);

      EntityAttributesCollection eac = new EntityAttributesCollection();
      eac.EffectiveDate = aPP.effectiveDate;

      if (haveGoodParameters)
      {
        AttributeData ad = new AttributeData();
        TEntity[] eList = null;

        //--------------------------------------------------------------------------------
        //This is the getAttributes CALL!

        eList = ad.Entities(aPP.entities, aPP.itemTypes, aPP.entitiesRequested, aPP.itemTypesRequested, aPP.itemsRequested, aPP.effectiveDate, IsRawMode, RetrieveVirtualItems);
        //--------------------------------------------------------------------------------

        eac = populateEAC(eList, aPP.entitiesRequested, aPP.itemTypesRequested, aPP.itemsRequested, aPP.processBlockInheritYN, aPP.effectiveDate, IsRawMode, RetrieveVirtualItems);
      }

      return eac;
    }

    public EntityAttributesCollection getSystemAttributes(
      String entity,
      String itemtype,
      DateTime effective, bool blocked, int handle)
    {
      string parameters = "";

      if (blocked)
        parameters = "<parameters><ProcessBlockInheritYN value='False' /></parameters>";
      else
        parameters = "<parameters/>";

      return getSystemAttributes(entity, itemtype, null, parameters, effective, false, false, handle);
    }

    /// <summary>
    /// getAttributes preprocessing routine, here we check the parameters passed, and clean and prepare them up
    /// for them to be used by the main getAttributes routine
    /// </summary>
    /// <param name="Entities"></param>
    /// <param name="ItemTypes"></param>
    /// <param name="Items"></param>
    /// <param name="ParameterList"></param>
    /// <param name="EffectiveDate"></param>
    /// <param name="IsRawMode"></param>
    /// <param name="retrieveVirtualItems"></param>
    /// <returns></returns>
    public EntityAttributesCollection getSystemAttributes(
      string Entities,
      string ItemTypes,
      string Items,
      string ParameterList,
      DateTime EffectiveDate,
      bool IsRawMode,
      bool RetrieveVirtualItems,
      int handle)
    {
      AttributeProcessorParameters aPP = new AttributeProcessorParameters();
      bool haveGoodParameters = aPP.setParameters(Entities, ItemTypes, Items, ParameterList, EffectiveDate, IsRawMode, RetrieveVirtualItems);

      EntityAttributesCollection eac = new EntityAttributesCollection();
      eac.EffectiveDate = aPP.effectiveDate;

      if (haveGoodParameters)
      {
        AttributeData ad = new AttributeData();

        Hashtable eListReq = new Hashtable();

        TEntity[] eSystemList = null;
        TEntity[] eList = null;

        //This creates what is now known as the CACHE and DICTIONARY source objects!
        eSystemList = SystemEntityFactory.Instance.Entities(aPP.effectiveDate);

        //--------------------------------------------------------------------------------
        //Let us populate the eList with the system entities requested!
        eListReq.Clear();
        for (int k1 = 0; k1 < aPP.entities.GetLength(0); k1++)
        {
          foreach (TEntity se in eSystemList)
          {
            if (se.Id == aPP.entities[k1] && !eListReq.ContainsKey(se.Id))
            {
              eListReq.Add(se.Id, se);
              break;
            }
          }
        }

        eList = new TEntity[eListReq.Count];
        int k2 = 0;

        foreach (TEntity eReq in eListReq.Values)
          eList[k2++] = eReq;
        //--------------------------------------------------------------------------------

        eac = populateEAC(eList, aPP.entitiesRequested, aPP.itemTypesRequested, aPP.itemsRequested, aPP.processBlockInheritYN, aPP.effectiveDate, IsRawMode, RetrieveVirtualItems);
      }

      return eac;
    }

    private EntityAttributesCollection populateEAC(
      TEntity[] eList,
      Hashtable entitiesRequested,
      Hashtable itemTypesRequested,
      Hashtable itemsRequested,
      bool processBlockInheritYN,
      DateTime effectiveDate,
      bool IsRawMode,
      bool RetrieveVirtualItems)
    {
      EntityAttributesCollection eac = new EntityAttributesCollection() { EffectiveDate = effectiveDate };

      Entity entity = null;
      ItemType it = null;
      Item item = null;
      ItemHistory itemHst = null;

      TAGAttribute attribute = null;
      ValueHistory vh = null;

      TableHeader tableHeader = null;

      TAttribute attrObjIdx = null;
      TAttribute attrObjObj = null;

      TAttribute aObjIdx = null;
      TAttribute aObjObj = null;

      TItem itemObjIdx = null;
      TItem itemObjObj = null;

      TAttributeTHMerge attrObjTHMerge = null;

      object itemIsBlockInheritedFuncValue = null;
      bool itemIsBlockInherited = false;

      #region BEGIN: Creating the new EAC
      if (eList.GetLength(0) > 0)
      {
        foreach (TEntity eObj in eList)
        {

          if (entitiesRequested != null && !(entitiesRequested.ContainsKey(eObj.Id)))
            continue;

          entity = new Entity();
          entity.ID = eObj.OrigId;

          entity.Fields.Add(new Field() { ID = "AlternateID", Value = eObj.AlternateID });
          entity.Fields.Add(new Field() { ID = "AlternateName", Value = eObj.AlternateName });
          entity.Fields.Add(new Field() { ID = "Client", Value = eObj.Client });
          entity.Fields.Add(new Field() { ID = "EndDate", Value = eObj.EndDate });
          entity.Fields.Add(new Field() { ID = "Entity", Value = eObj.OrigId });
          entity.Fields.Add(new Field() { ID = "EntityOwner", Value = eObj.EntityOwner });
          entity.Fields.Add(new Field() { ID = "EntityType", Value = eObj.EntityType });
          entity.Fields.Add(new Field() { ID = "FEIN", Value = eObj.FEIN });
          entity.Fields.Add(new Field() { ID = "FirstName", Value = eObj.FirstName });
          entity.Fields.Add(new Field() { ID = "FullName", Value = eObj.FullName });
          entity.Fields.Add(new Field() { ID = "LastModifiedBy", Value = eObj.LastModifiedBy });
          entity.Fields.Add(new Field() { ID = "LastModifiedDateTime", Value = eObj.LastModifiedDateTime });
          entity.Fields.Add(new Field() { ID = "LegalName", Value = eObj.LegalName });
          entity.Fields.Add(new Field() { ID = "MiddleName", Value = eObj.MiddleName });
          entity.Fields.Add(new Field() { ID = "StartDate", Value = eObj.StartDate });
          entity.Fields.Add(new Field() { ID = "ShortName", Value = eObj.ShortName });

          IEnumerable itemEnumerable = eObj.getItemEnumerable(itemTypesRequested);
          foreach (TIndexItem idxItemEntry in itemEnumerable)
          {
            itemObjIdx = (TItem)idxItemEntry.ItemIdx;
            itemObjObj = (TItem)idxItemEntry.ItemObj;

            //If we requested ONLY a fixed list of items, let us check if the current one is in the requested list of items!
            if (itemsRequested != null && !(itemsRequested.ContainsKey(itemObjIdx.Id)))
              continue;

            itemIsBlockInherited = false;
            if (!(IsRawMode) && itemObjObj.AttributeIndex.Contains(BLOCKINHERIT))
            {
              aObjIdx = (TAttribute)((TIndexItem)itemObjObj.AttributeIndex[BLOCKINHERIT]).ItemIdx;
              if (aObjIdx.ValueType == Constants.VALUE)
                itemIsBlockInherited = (((string)aObjIdx.Value).ToLower() == "true" && itemObjObj.Entity.Id != eObj.Id);
              else
              { 
                itemIsBlockInheritedFuncValue = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.processBlockInheritYN, aObjIdx.Value.ToString(), itemObjObj, eObj, aObjIdx);
                if(itemIsBlockInheritedFuncValue == null)
                  itemIsBlockInherited = false;
                else
                  itemIsBlockInherited =((bool)itemIsBlockInheritedFuncValue && itemObjObj.Entity.Id != eObj.Id);
              }

              if (itemIsBlockInherited && processBlockInheritYN && !RetrieveVirtualItems)
                continue;
            }

            item = new Item();
            if (itemIsBlockInherited)
              item.Source = Item.BLOCKED;

            if (!entity.ItemTypes.Contains(itemObjIdx.ItemType.Id))
            {
              it = new ItemType();
              it.ID = itemObjIdx.ItemType.OrigId;
              entity.ItemTypes.Add(it);
            }

            item.ID = itemObjIdx.OrigId;
            item.StartDate = itemObjIdx.StartDate;
            item.EndDate = itemObjIdx.EndDate;
            item.EffectiveDate = itemObjIdx.Entity.EffectiveDate;
            item.IsInherited = itemObjIdx.IsInherited(eObj);

            foreach (TItemHistory iObjHst in itemObjIdx.ItemHistory.Values) 
            {
              itemHst = new ItemHistory() { StartDate = iObjHst.StartDate, EndDate = iObjHst.EndDate };
              item.ItemHistoryRecords.Add(itemHst);
            }
            item.ItemHistoryRecords.Sort();

            item.StartDate = itemObjIdx.StartDate;
            item.EndDate = itemObjIdx.EndDate;

            foreach (TIndexItem aObjIdxEntry in itemObjObj.getAttributeEnumerable(eObj))
            {
              attrObjIdx = (TAttribute)aObjIdxEntry.ItemIdx;
              attrObjObj = (TAttribute)aObjIdxEntry.ItemObj;

              attribute = new TAGAttribute();

              attribute.ID = attrObjObj.OrigId;
              attribute.StartDate = Constants.PASTDATETIME;
              attribute.EndDate = Constants.FUTUREDATETIME;
              attribute.EffectiveDate = attrObjObj.Item.Entity.EffectiveDate;
              attribute.IsIncluded = Constants.AnyOn(attrObjObj.Flags, EAttributeFlags.IsIncluded);
              attribute.IsRefValue = Constants.AnyOn(attrObjObj.Flags, EAttributeFlags.IsRefValue);
              attribute.Parent = item;

              foreach (TValueHistory tValueHistory in attrObjObj.History)
              {
                vh = new ValueHistory();
                vh.StartDate = tValueHistory.StartDate;
                vh.EndDate = tValueHistory.EndDate;
                vh.ValueType = tValueHistory.ValueType;
                vh.Value = tValueHistory.Value;
                vh.LastModifiedBy = tValueHistory.LastModifiedBy;
                vh.LastModifiedDateTime = tValueHistory.LastModifiedDateTime;

                if (vh.StartDate <= eObj.EffectiveDate && eObj.EffectiveDate <= vh.EndDate)
                {
                  attribute.StartDate = vh.StartDate;
                  attribute.EndDate = vh.EndDate;
                  attribute.ValueType = vh.ValueType;
                  attribute.ReferenceValueSource = tValueHistory.ReferenceValueSource;
                  attribute.LastModifiedBy = vh.LastModifiedBy;
                  attribute.LastModifiedDateTime = vh.LastModifiedDateTime;
                }

                attribute.Values.Add(vh);
              }

              if (!(IsRawMode))
              {
                switch (attribute.ValueType)
                {
                  case Constants.REF_INHERIT:
                    attribute.OverwriteValue = false;
                    attribute.IsInherited = attrObjIdx.IsInherited(eObj);

                    aObjObj = attrObjObj.getSolveAttribute(aObjIdxEntry);
                    if (aObjObj != null)
                    {
                      attribute.ValueType = aObjObj.ValueType;
                      if (attribute.ValueType == Constants.TABLEHEADER && aObjObj.Value != null)
                      {
                        tableHeader = new TableHeader(attribute.ID, (string)aObjObj.Value, aObjObj.Item.ItemHash);
                        attribute.Value = tableHeader;
                      }
                      else
                        attribute.Value = (string)aObjObj.Value;
                    }
                    else
                    {
                      attribute.Value = attrObjIdx.SolveValue(aObjIdxEntry);
                      attribute.ValueType = attrObjIdx.SolveValueType(aObjIdxEntry);
                    }

                    attribute.OverwriteValue = false;
                    break;
                  case Constants.FUNCTION:
                    attribute.OverwriteValue = false;
                    attribute.IsInherited = attrObjIdx.IsInherited(eObj);
                    attribute.Value = attrObjIdx.SolveValue(aObjIdxEntry);
                    attribute.ValueType = Constants.FUNCTION;
                    break;
                  case Constants.TABLEHEADER:
                    attribute.OverwriteValue = true;
                    attrObjTHMerge = itemObjIdx.getTHMergeAttribute(attrObjIdx.Id, eObj);
                    if (attrObjTHMerge != null)
                      attribute.Value = mergeTHs(attrObjTHMerge);
                    else
                      attribute.Value = mergeTHs(attrObjIdx);

                    attribute.OverwriteValue = false;
                    attribute.IsInherited = attrObjIdx.IsInherited(eObj);
                    attribute.ValueType = Constants.TABLEHEADER;
                    break;
                  case Constants.VALUE:
                    if (attrObjIdx.Value != null && ((string)attrObjIdx.Value).Contains("@@"))
                    {
                      attribute.OverwriteValue = false;
                      attribute.IsInherited = attrObjIdx.IsInherited(eObj);
                      attribute.Value = attrObjIdx.SolveValue(aObjIdxEntry);
                    }
                    else
                    {
                      attribute.OverwriteValue = true;
                      attribute.Value = attrObjIdx.Value;
                      attribute.IsInherited = attrObjIdx.IsInherited(eObj);
                      attribute.OverwriteValue = false;
                    }
                    attribute.ValueType = Constants.VALUE;
                    break;
                }
              }
              else
              {
                attribute.OverwriteValue = true;
                attribute.Value = attrObjIdx.Value;
                attribute.IsInherited = attrObjIdx.IsInherited(eObj);
                attribute.ValueType = attrObjIdx.ValueType;
                attribute.OverwriteValue = false;
              }

              item.Add(attribute);
            }

            entity.ItemTypes[itemObjIdx.ItemType.Id].Add(item);
          }

          entity.Dirty = false;
          eac.Entities.Add(entity);
        }
      }

      if (!(IsRawMode))
        eac = processEAC(eac, RetrieveVirtualItems, processBlockInheritYN);

      #endregion END: Creating the new EAC

      //---------------------------------------------------------------------
      //Return to populated and sorted EntityAttributeCollection
      return eac;
      //---------------------------------------------------------------------
    }

    private EntityAttributesCollection processEAC(EntityAttributesCollection eac, bool retrieveVirtualItems, bool processBlockInheritYN)
    {
      TAGBOSS.Common.TAGFunctions.BypassFunctionError = true;

      string itemTypeID = "";
      string itemID = "";

      FunctionProcessor functionProcessor = new FunctionProcessor();
      DictionaryFunctions dictionaryFunctions = new DictionaryFunctions();

      foreach (Entity e in eac.Entities)
      {
        foreach (ItemType e_it in e.ItemTypes)
        {
          itemTypeID = e_it.ID;   //itemType that we are processing...

          for (int k3 = e_it.Items.Count - 1; k3 > -1; k3--)
          {
            Item e_it_i = e_it.Items[k3];
            itemID = e_it_i.ID;   //This is the item we are processing...

            //Function processing!! this resolves any call to a function in the functions dictionary
            e_it_i = functionProcessor.Output(e_it_i, dictionaryFunctions);

            processBlockInheritAndExclude(e_it_i, e_it, false, retrieveVirtualItems, processBlockInheritYN);
          }

        }
        e.Dirty = false;
      }

      return eac;
    }

    private TableHeader mergeTHs(TAttribute tAttr) 
    {
      TableHeader tmpTrgTH = null;
      TableHeader tmpSrcTH = null;
      string TH = (string)tAttr.Value;
      string THTrgSrc = "";
      string THSrcSrc = "";

      THTrgSrc = tAttr.Item.ItemHash;
      tmpTrgTH = new TableHeader(tAttr.Id, TH, THTrgSrc);
      foreach (TIndexItem TRAttr in tAttr.RelatedAttributes)
      {
        if (((TAttribute)TRAttr.ItemIdx).ValueType == Constants.TABLEHEADER)
        {
          TH = (string)((TAttribute)TRAttr.ItemIdx).Value;
          THSrcSrc = ((TAttribute)TRAttr.ItemIdx).Item.ItemHash;
          tmpSrcTH = new TableHeader(tAttr.Id, TH, THSrcSrc);
          tmpTrgTH.Merge(tmpSrcTH, THSrcSrc);
        }
      }

      return tmpTrgTH;
    }

    private TableHeader mergeTHs(TAttributeTHMerge tAttrTHMerge)
    {
      ArrayList tmpMergedTHKeys = new ArrayList();
      ArrayList tmpMergedDefaultEntityTHKeys = new ArrayList();
      ArrayList tmpMergedDefaultEntityDefaultItemTHKeys = new ArrayList();
      Hashtable tmpMergedTH = new Hashtable();
      Hashtable processedItemHashList = new Hashtable();

      TIndexItem tmpIdx = null;
      TableHeader tmpTrgTH = null;
      TableHeader tmpSrcTH = null;

      TItem tmpItemIdx = null;
      TItem tmpItemObj = null;
      TAttribute tmpAttrIdx = null;
      TAttribute tmpAttrObj = null;

      int myIncludeDepth = 0;

      string TH = "";
      string THTrgSrc = "";
      string THSrcSrc = "";

      string attributeId = tAttrTHMerge.AttributeId;

      foreach (string attrMergeItemHash in tAttrTHMerge.AttributeMergeItemHashList) 
      {
        tmpIdx = (TIndexItem)tAttrTHMerge.AttributeMergeItemHashRAList[attrMergeItemHash];

        tmpItemIdx = (TItem)tmpIdx.ItemIdx;
        tmpItemObj = (TItem)tmpIdx.ItemObj;

        tmpAttrIdx = (TAttribute)tmpItemObj.AttributeIndex[attributeId].ItemIdx;
        tmpAttrObj = (TAttribute)tmpItemObj.AttributeIndex[attributeId].ItemObj;

        myIncludeDepth = tmpAttrObj.IncludeDepth;

        if (!processedItemHashList.ContainsKey(tmpAttrIdx.Item.ItemHash))
        {
          TH = (string)tmpAttrObj.Value;
          THTrgSrc = tmpAttrIdx.Item.ItemHash;
          tmpTrgTH = new TableHeader(attributeId, TH, THTrgSrc);

          if (tmpAttrObj.RelatedAttributes.List.Count > 0)
          {
            foreach (TIndexItem TRAttr in tmpAttrObj.RelatedAttributes)
            {
              if (((TAttribute)TRAttr.ItemIdx).ValueType == Constants.TABLEHEADER)
              {
                TH = (string)((TAttribute)TRAttr.ItemIdx).Value;
                THSrcSrc = ((TAttribute)TRAttr.ItemIdx).Item.ItemHash;
                tmpSrcTH = new TableHeader(attributeId, TH, THSrcSrc);

                if (myIncludeDepth <= ((TAttribute)TRAttr.ItemObj).IncludeDepth)
                {
                  tmpTrgTH.Merge(tmpSrcTH, THSrcSrc);
                  myIncludeDepth = ((TAttribute)TRAttr.ItemObj).IncludeDepth;
                }
                else
                {
                  tmpSrcTH.Merge(tmpTrgTH, THTrgSrc);
                  tmpTrgTH = tmpSrcTH;
                }
              }
            }
          }

          if (tmpAttrIdx.Item.ItemHash.StartsWith(Constants.DEFAULT_ENTITY) && tmpAttrIdx.Item.ItemHash.EndsWith(Constants.DEFAULT_ITEM))
            tmpMergedDefaultEntityDefaultItemTHKeys.Add(tmpAttrIdx.Item.ItemHash);
          else if (tmpAttrIdx.Item.ItemHash.StartsWith(Constants.DEFAULT_ENTITY))
            tmpMergedDefaultEntityTHKeys.Add(tmpAttrIdx.Item.ItemHash);
          else
            tmpMergedTHKeys.Add(tmpAttrIdx.Item.ItemHash);

          tmpMergedTH.Add(tmpAttrIdx.Item.ItemHash, tmpTrgTH);

          processedItemHashList.Add(tmpAttrIdx.Item.ItemHash, null);
        }
      }

      //We add the default entity, current item to the end of the list
      foreach (string itemHashKey in tmpMergedDefaultEntityTHKeys)
        tmpMergedTHKeys.Add(itemHashKey);

      //We add the default entity, default item to the end of the list
      foreach (string itemHashKey in tmpMergedDefaultEntityDefaultItemTHKeys)
        tmpMergedTHKeys.Add(itemHashKey);

      tmpTrgTH = null;
      foreach (string attrMergedTH in tmpMergedTHKeys)
      {
        if (tmpTrgTH == null)
          tmpTrgTH = (TableHeader)tmpMergedTH[attrMergedTH];
        else
        {
          tmpSrcTH = (TableHeader)tmpMergedTH[attrMergedTH];
          tmpTrgTH.Merge(tmpSrcTH, attrMergedTH);
        }
      }

      return tmpTrgTH;
    }

    private void processBlockInheritAndExclude(Item e_it_i, ItemType e_it, bool IsOnlyDEFAULTEntity, bool retrieveVirtualItems, bool processBlockInheritYN)
    {
      /*
     * Before we return we must get rid of all the items with a Exclude attribute set to TRUE, YES or 1
     * We will check too if the item has a BlockInheritYN set to true, and the isInherit of the item is set to true, then we will remove the item!
     */
      bool bRemoveItem = false;  //This flag will be used to see if we need to remove this item...
      string itemID = e_it_i.ID;   //This is the item we are processing...
      if (!(IsOnlyDEFAULTEntity))
      {
        if (e_it_i.Attributes.Contains(EXCLUDE_ITEM))
          if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, e_it_i.Attributes[EXCLUDE_ITEM].Value))
            bRemoveItem = true;
        if (e_it_i.Attributes.Contains(EXCLUDE_YN_ITEM))
          if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, e_it_i.Attributes[EXCLUDE_YN_ITEM].Value))
            bRemoveItem = true;
        if (e_it_i.IsInherited && e_it_i.Attributes.Contains(BLOCKINHERIT))
        {
          if (retrieveVirtualItems || !(processBlockInheritYN))
          {
            if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, e_it_i.Attributes[BLOCKINHERIT].Value))
              e_it_i.Source = Item.BLOCKED;
          }
          else
            if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, e_it_i.Attributes[BLOCKINHERIT].Value))
              bRemoveItem = true;
        }
      }
      if (bRemoveItem)   //so we set the flag to remove this item, so we remove it...
      {
        bool bOldMarkForDelete = e_it.Items.MarkForDelete;
        e_it.Items.MarkForDelete = false;
        e_it.Items.Remove(itemID);
        e_it.Items.MarkForDelete = bOldMarkForDelete;
      }
    }

    #region Auxiliary and extra processes not directly related to the AttributeProcessor loading

    public PickList getDefaultItemList(string strItemTypeList, PickList attributeFilter)
    {
      TEntity m_defaultEntity = SystemEntityFactory.Instance.getEntity(Constants.DEFAULT_ENTITY, DateTime.Now);
      PickList returnList = new PickList(new string[] { "ItemType", "Item" }, 0);
      TItem tItem = null;
      TAttribute a = null;
      string dataType = "";
      string aName = "";
      object aValue = null;
      bool selectItem = false;

      Hashtable itemTypeHashList = new Hashtable();
      string[] itemTypeList = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, strItemTypeList);
      for (int i = 0; i < itemTypeList.GetLength(0); i++)
        if(itemTypeList[i] != null && itemTypeList[i] != "")
          itemTypeHashList.Add(itemTypeList[i].Trim().ToLower(), null);

      if (m_defaultEntity != null)
      {
        int iRow = 0;
        foreach (TIndexItem tIdxItem in m_defaultEntity.getItemEnumerable())
        {
          tItem = (TItem)tIdxItem.ItemIdx;
          if (!(itemTypeHashList.ContainsKey(tItem.ItemType.Id)))
            continue;

          selectItem = true; // assume we are going to select the item
          if (attributeFilter != null)  // if there is a filter
          {
            for (int i = 0; i < attributeFilter.Count; i++) // foreach filter
            {
              aName = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, attributeFilter[i, 0])).ToLower(); // get the attribute name
              if (!aName.Equals("attribute"))
              {
                a = tItem.getAttribute(aName, m_defaultEntity);
                if (a != null)                        // if the item contains it
                {
                  aValue = attributeFilter[i, 1];
                  dataType = DictionaryFactory.getInstance().getDictionary().DataType(aName);
                  selectItem = ((int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CompareTo, aValue, dataType, a.Value, dataType) == 0);  // then check the values
                  if (!selectItem)
                    break;
                }
                else
                {
                  selectItem = false;                                       // otherwise this item does not have the attribute so we don't include it
                  break;
                }
              }
            }
          }
          if (selectItem)                                                 // add the item to the list if it passes the test (or if there is no test)
          {
            returnList.AddRow();
            returnList[iRow, 0] = tItem.ItemType.OrigId;
            returnList[iRow++, 1] = tItem.OrigId;
          }
        }
      }
      return returnList;
    }
    
    #endregion Auxiliary and extra processes not directly related to the AttributeProcessor loading

    #endregion ConvertToEAC methods, this methods return an old EAC structure

    #region ConvertToEAC methods, this are new methods that return the new structure, an array of the solicited entities!

    public TEntityCollection getSystemAttributesNew(
      string Entities,
      string ItemTypes,
      string Items,
      string ParameterList,
      DateTime EffectiveDate,
      bool IsRawMode,
      bool RetrieveVirtualItems)
    {
      TEntityCollection entityCollection = null;

      AttributeProcessorParameters aPP = new AttributeProcessorParameters();
      bool haveGoodParameters = aPP.setParameters(Entities, ItemTypes, Items, ParameterList, EffectiveDate, IsRawMode, RetrieveVirtualItems);

      if (haveGoodParameters)
      {
        TEntity[] eSytemList = null;
        //TEntity[] eList = null;
        AttributeData ad = new AttributeData();

        //This creates what is now known as the CACHE and DICTIONARY source objects!
        eSytemList = SystemEntityFactory.Instance.Entities(aPP.effectiveDate);

        ////--------------------------------------------------------------------------------
        ////Let us populate the eList with the system entities requested!
        //for (int k1 = 0; k1 < aPP.entities.GetLength(0); k1++)
        //{
        //  foreach (TEntity se in eSytemList)
        //  {
        //    if (se.Id == aPP.entities[k1] && !eListReq.ContainsKey(se.Id))
        //    {
        //      eListReq.Add(se.Id, se);
        //      break;
        //    }
        //  }
        //}

        //eList = new TEntity[eListReq.Count];
        //int k2 = 0;

        //foreach (TEntity eReq in eListReq.Values)
        //  eList[k2++] = eReq;
        ////--------------------------------------------------------------------------------

        //if (eList != null && eList.GetLength(0) > 0)
        if (eSytemList != null && eSytemList.GetLength(0) > 0)
          entityCollection = new TEntityCollection(eSytemList, aPP.entitiesRequested, aPP.itemTypesRequested, aPP.itemsRequested);
      }

      return entityCollection;
    }

    public TEntityCollection getAttributesNew(
      string Entities,
      string ItemTypes,
      string Items,
      string ParameterList,
      DateTime EffectiveDate,
      bool IsRawMode,
      bool RetrieveVirtualItems)
    {
      //TODO: Begin: We set a log for any call to the Default entity
      if (Entities.ToLower().Contains(Constants.DEFAULT_ENTITY))
      {
        log.Debug(
          @"public TEntity[] getAttributesNew( "
          + "Entities = " + Entities + ","
          + "ItemTypes = " + ItemTypes + ","
          + "Items = " + Items + ","
          + "ParameterList = '" + ParameterList + "',"
          + "EffectiveDate = " + EffectiveDate.ToShortDateString() + ","
          + "IsRawMode = " + (IsRawMode ? "True" : "False") + ","
          + "RetrieveVirtualItems = " + (RetrieveVirtualItems ? "True" : "False") + "))");
      }
      //TODO: End: We set a log for any call to the Default entity
      TEntityCollection entityCollection = null;

      AttributeProcessorParameters aPP = new AttributeProcessorParameters();
      bool haveGoodParameters = aPP.setParameters(Entities, ItemTypes, Items, ParameterList, EffectiveDate, IsRawMode, RetrieveVirtualItems);

      if (haveGoodParameters)
      {
        TEntity[] eSytemList = null;
        TEntity[] eList = null;
        AttributeData ad = new AttributeData();

        //This creates what is now known as the CACHE and DICTIONARY source objects!
        eSytemList = SystemEntityFactory.Instance.Entities(aPP.effectiveDate);

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //This is the getAttributes CALL!
        eList = ad.Entities(aPP.entities, aPP.itemTypes, aPP.entitiesRequested, aPP.itemTypesRequested, aPP.itemsRequested, aPP.effectiveDate, IsRawMode, RetrieveVirtualItems);
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        if (eList != null && eList.GetLength(0) > 0)
          entityCollection = new TEntityCollection(eList, aPP.entitiesRequested, aPP.itemTypesRequested, aPP.itemsRequested);
      }

      return entityCollection;
    }

    #endregion ConvertToEAC methods, this are new methods that return the new structure, an array of the solicited entities!

    #region New NonXML Save methods
    public void SaveNonXML(Entity e, bool isRaw, string user)
    {
      AttributeData attributeData = new AttributeData();
      attributeData.Save(e, isRaw, user);
    }
    #endregion

  }
}
