using System;
using System.Collections;
using System.Collections.Generic;

using TAGBOSS.Common;
using TAGBOSS.Common.Logging;
using TAGBOSS.Common.Model;

namespace TAGBOSS.Sys.AttributeEngine2.ConvertToEAC
{
  public sealed class CacheObject
  {
    Log log = (Log)LogFactory.GetInstance().GetLog("TAGBOSS.Sys.AttributeEngine2.ConvertToEAC.CacheObject");

    private static readonly CacheObject cacheObject = new CacheObject();

    public static CacheObject getInstance()
    {
      return cacheObject;
    }

    public void load()
    {
      SystemEntityFactory.Instance.ReLoad();
    }

    #region support methods for EAC cache consultation
    /// <summary>
    /// Returns a subset of the cache resolved object, so we can visualize it's properties
    /// for exmple to review the itemIncludedTree
    /// </summary>
    /// <param name="ItemTypes"></param>
    /// <param name="Items"></param>
    /// <returns></returns>
    //public EntityAttributesCollection getCacheBranch(string ItemTypes, string Items, bool deepCopy, bool rawMode, DateTime effectiveDate)
    public EntityAttributesCollection getCacheBranch(string ItemTypes, string Items, DateTime EffectiveDate)
    {
      Hashtable itemTypesRequested = null;
      Hashtable itemsRequested = null;

      DateTime effectiveDate;
      TItem TDefaultItem = null;

      string[] arrItemTypes = null;
      string[] arrItems = null;
      string[] arrItemTypesReq = null;
      string[] arrItemsReq = null;
      string[] charSeparator = new string[] { "," };

      TEntity eObjDefault = null;
      TEntity[] eSytemList = null;

      if (ItemTypes != null && ItemTypes.Trim() != "")
        ItemTypes = ItemTypes.Replace(" ", "").ToLower();

      if (Items != null && Items.Trim() != "")
        Items = Items.Replace(" ", "").ToLower();

      //We first check all the parameters!!
      if (EffectiveDate != null)
        effectiveDate = DateTime.Parse(EffectiveDate.ToShortDateString());
      else
        effectiveDate = DateTime.Parse(DateTime.Now.ToShortDateString());

      if (ItemTypes != null && ItemTypes.Trim() != "")
      {
        arrItemTypes = ItemTypes.Replace(" ", "").ToLower().Split(charSeparator, StringSplitOptions.RemoveEmptyEntries);
        arrItemTypesReq = ItemTypes.Replace(" ", "").Split(charSeparator, StringSplitOptions.RemoveEmptyEntries);
      }

      if (Items != null && Items.Trim() != "")
      {
        arrItems = Items.Replace(" ", "").ToLower().Split(charSeparator, StringSplitOptions.RemoveEmptyEntries);
        arrItemsReq = Items.Replace(" ", "").Split(charSeparator, StringSplitOptions.RemoveEmptyEntries);
      }

      //Let us check if the entity ItemType was requested!
      if (arrItemTypes != null)
      {
        itemTypesRequested = new Hashtable();
        for (int i = 0; i < arrItemTypes.Length; i++)
        {
          if (!(itemTypesRequested.ContainsKey(arrItemTypes[i])))
            itemTypesRequested.Add(arrItemTypes[i], arrItemTypesReq[i]);
        }
      }

      //Let us check if the entity ItemType was requested!
      if (arrItems != null)
      {
        itemsRequested = new Hashtable();
        for (int i = 0; i < arrItems.Length; i++)
        {
          if (!(itemsRequested.ContainsKey(arrItems[i])))
            itemsRequested.Add(arrItems[i], arrItemsReq[i]);
        }
      }

      //This creates/loads what is now known as the CACHE and DICTIONARY source objects!
      eSytemList = SystemEntityFactory.Instance.Entities(effectiveDate);

      //Now let us find the Default Entity!
      foreach (TEntity eObj in eSytemList)
      {
        if (eObj.Id == Constants.DEFAULT_ENTITY)
        {
          eObjDefault = eObj;
          break;
        }
      }

      EntityAttributesCollection cacheBranchEAC = new EntityAttributesCollection();
      cacheBranchEAC.EffectiveDate = effectiveDate;

      Entity cacheBranchEntity = new Entity();
      cacheBranchEntity.ID = Constants.DEFAULT_ENTITY;
      cacheBranchEAC.Entities.Add(cacheBranchEntity);

      string defaultEntityID = "Default";
      /*
       * Here we do the filling of the EAC for the Default Branch that we have been asked for...
       * For this we will use the same routine as the Dictionary fill method, without solving references or functions
       * but just getting all the requested itemTypes and Attributes, since what we are looking for is only the 
       * IncludeItemTree so as to display it to the user...
       */

      cacheBranchEntity.ID = defaultEntityID;
      foreach (TIndexItem indexItem in eObjDefault.getItemEnumerable(itemTypesRequested))
      {
        TDefaultItem = (TItem)indexItem.ItemObj;

        if (TDefaultItem == null || (itemsRequested != null && !itemsRequested.ContainsKey(TDefaultItem.Id)))
          continue;

        if (!cacheBranchEntity.ItemTypes.Contains(TDefaultItem.ItemType.Id))
        {
          ItemType DictIT = new ItemType();
          DictIT.ID = TDefaultItem.ItemType.OrigId;
          cacheBranchEntity.ItemTypes.Add(DictIT);
        }

        Item cacheBranchItem = new Item();
        cacheBranchItem.ID = TDefaultItem.OrigId;
        cacheBranchItem.StartDate = TDefaultItem.Entity.StartDate;
        cacheBranchItem.EndDate = TDefaultItem.Entity.EndDate;
        cacheBranchItem.EffectiveDate = TDefaultItem.Entity.EffectiveDate;
        cacheBranchItem.IncludedItemTree = getIncludeItemTree(TDefaultItem, eObjDefault);

        cacheBranchEntity.ItemTypes[TDefaultItem.ItemType.Id].Add(cacheBranchItem);
      }

      cacheBranchEAC.Entities.Add(cacheBranchEntity);

      return cacheBranchEAC;
    }

    public ArrayList getListCacheEffectiveDates()
    {
      return SystemEntityFactory.Instance.getListCacheEffectiveDates();
    }

    private IncludedItemTree getIncludeItemTree(TItem iObj, TEntity eObj) 
    {
      Hashtable depthSequence = new Hashtable();
      Hashtable itemHashItemHash = new Hashtable();
      Hashtable itemHashIncludedItemAddress = new Hashtable();

      TItem iObjInclude = null;

      IncludedItemTree includeItemTree = new IncludedItemTree();
      IncludedItemAddress includeItemAddress = null;
      IncludedItemLeaf includeItemLeaf = null;
      string[] includeHashKeys = null;
      string[] splitChar = new string[] { "." };
      int depth = 0;
      int sequence = 0;

       //The first item in the includeTree is ALWAYS the Item I am processing, since it is the "root" of the IncludeTree
       //We must add this to the list before going into the includes for this item
      includeItemAddress = new IncludedItemAddress(0, 0);
      includeItemLeaf =
        new IncludedItemLeaf()
        {
          ItemTypeID = iObj.ItemType.OrigId,
          ItemID = iObj.OrigId,
          EmbeddedIncludesList = null,
          ValidInclude = true
        };

      itemHashIncludedItemAddress.Add(iObj.ItemHash, includeItemAddress);
      itemHashItemHash.Add(iObj.ItemHash, null);
      depthSequence.Add(0, 0);

      includeItemTree.Add(includeItemAddress, includeItemLeaf);

      TIncludeItem includeItem = null;
      try
      {
        for (int i = 0; i < iObj.Includes.Count; i++)
        {
          includeItem = (TIncludeItem)iObj.Includes[i];
          if (includeItem.DefaultInclude)
            continue;

          //First we check the [depth, sequence] pair for the includeItemAddress
          depth = includeItem.IncludeDepth;
          if (depthSequence.ContainsKey(depth))
          {
            sequence = ((int)depthSequence[depth]) + 1;
            depthSequence[depth] = sequence;
          }
          else
          {
            sequence = 0;
            depthSequence.Add(depth, sequence);
          }

          includeHashKeys = includeItem.IncludeHash.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

          if (eObj.ItemIndex.Contains(includeItem.IncludeHash))
            iObjInclude = (TItem)eObj.ItemIndex[includeItem.IncludeHash].ItemIdx;
          else
            iObjInclude = null;

          includeItemAddress = new IncludedItemAddress(depth, sequence);
          includeItemLeaf = new IncludedItemLeaf();

          includeItemLeaf.ItemTypeID = (iObjInclude != null ? iObjInclude.ItemType.OrigId : includeHashKeys[1]);
          includeItemLeaf.ItemID = (iObjInclude != null ? iObjInclude.OrigId : includeHashKeys[2]);
          includeItemLeaf.EmbeddedIncludesList = null;
          includeItemLeaf.ValidInclude = (iObjInclude != null ? true : false);

          itemHashItemHash.Add(includeItem.IncludeHash, includeItem.IncludeSource);
          itemHashIncludedItemAddress.Add(includeItem.IncludeHash, includeItemAddress);
          includeItemTree.Add(includeItemAddress, includeItemLeaf);
        }

        //So we have the completed (but not sorted!) includeItemTree, now we must fill the Embedded includeItemTrees
        string includeSource = "";
        foreach (string includeHash in itemHashItemHash.Keys)
        {
          if (itemHashItemHash[includeHash] == null)
            continue;

          includeSource = (string)itemHashItemHash[includeHash];

          if (itemHashIncludedItemAddress.ContainsKey(includeSource))
          {
            includeItemAddress = (IncludedItemAddress)itemHashIncludedItemAddress[includeSource];
            includeItemLeaf = includeItemTree[includeItemAddress];

            includeItemAddress = (IncludedItemAddress)itemHashIncludedItemAddress[includeHash];

            if (includeItemLeaf.EmbeddedIncludesList == null)
              includeItemLeaf.EmbeddedIncludesList = new List<IncludedItemAddress>();

            includeItemLeaf.EmbeddedIncludesList.Add(includeItemAddress);

          }
        }
      }
      catch (Exception e) 
      {
        log.Error("Error adding include element to: " + iObj.ItemHash + "::" +  includeItem.IncludeHash);
        log.Error(e.Message);
        log.Error(e.StackTrace);
      }

      return includeItemTree;
    }

    #endregion support methods for EAC cache consultation
  }
}
