using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;

using CCI.Common;

using TAGBOSS.Common;
using TAGBOSS.Common.Model;
//using TAGBOSS.Sys.Interface;

using TAGBOSS.Sys.AttributeEngine2.ConvertToEAC;


namespace CCI.Sys.Data
{
  /// <summary>
  /// Entities Object gives access to both the Entity table and Attributes object for any program that needs data from these sources
  /// </summary>
  public class EntityBase 
  {
    #region constants

    const int cMAXDEPTH = 20;
    const string cENTITYITEMTYPE = "Entity";
    const string cEMPLOYEEITEMTYPE = "Employee";
    string cETYPE = cEMPLOYEEITEMTYPE.ToLower();

    DateTime pastDateTime;
    DateTime futureDateTime;

    #endregion constants

    #region module data
    ArrayList dontProcessEntityTypes = new ArrayList() { "Bank", "Vendor", "Carrier", "Prospect" };
    private EntityDB myEntityDB = new EntityDB();               // Entity DAL
    private AttributeDB myAttributeDB = new AttributeDB();
    protected string myItemTypes = "Entity";

    /*
     * TAGAttribute object (where most of the Entity Data is stored
     * 
     * if useAttributes flag is false, don't instantiate the Attributes Object for this Entity. 
     */
    AttributeProcessor myAttributes = new AttributeProcessor();

    private string loginID = string.Empty;
    #endregion module data

    #region public properties

    public string EntityTypeEmployee
    {
      get { return cEMPLOYEEITEMTYPE; }
    }

    #endregion public properties

    #region constructors/destructors
    /// <summary>
    /// This is the contructor that takes no parameters. By default, it sets UseAttributes to true
    /// </summary>
    public EntityBase()
    {
      pastDateTime = TAGFunctions.PastDateTime;    // init the default value for "way back in the past"
      futureDateTime = TAGFunctions.FutureDateTime;  // init the defaule value for "way out in the future"
    }

    /// <summary>
    /// Main constructor. It accepts the parameter of UseAttributes to decide if Attributes should be loaded.
    /// </summary>
    /// <param name="bUseAttributes"></param>
    private EntityBase(bool bUseAttributes)
    {
      // note that it is EntityAttributesCollection that actually decides to use Attributes
      // so we pass this into the DOC constructor
      
      //if (bUseAttributes)
      //{
      //  myAttributes = new AttributeProcessor();    // instantiate a new Attributes object if we are going to use it
      //}
      pastDateTime = TAGFunctions.PastDateTime;    // init the default value for "way back in the past"
      futureDateTime = TAGFunctions.FutureDateTime;  // init the defaule value for "way out in the future"
    }

    #endregion constructors/destructors

    #region public methods
    public bool IsEntityField(string fieldName)
    {
      return myEntityDB.IsEntityField(fieldName);
    }
    public string getSchemaForQuery(string mySQL, string dataSetName)
    {
      return myEntityDB.getSchemaForQuery(mySQL, dataSetName);
    }
    public string ConnectionString { get { return myEntityDB.ConnectionString; } }

    public void reloadCache()
    {
      CacheObject cache = CacheObject.getInstance();
      cache.load();
    }

    public EntityAttributesCollection getCacheBranch(string itemTypes, string items)
    {
      CacheObject cacheObject = CacheObject.getInstance();
      return cacheObject.getCacheBranch(itemTypes, items, DateTime.Now);
    }

    public ArrayList getListCacheEffectiveDates()
    {
      CacheObject cacheObject = CacheObject.getInstance();
      return cacheObject.getListCacheEffectiveDates();
    }

    /// <summary>
    /// Pass in a EntityAttributesCollection set of Entity and TAGAttribute data. Any thing that has been changes should have the
    /// Dirty flag set, so that this routines knows what to update or add.
    /// </summary>
    /// <param name="doEntities"></param>
    /// <param name="userID"></param>
    public void Save(EntityAttributesCollection doEntities, string userID)
    {
      /*
       * this routine receives the object back, then does the work to save anything that is dirty
       */

      foreach (Entity e in doEntities.Entities)
      {
        if (!e.ReadOnly && e.Dirty)
          SaveEntityRecord(e, userID);
      }
    }

    /// <summary>
    /// Returns a list of ItemTypes in an Entity object without items or attributes
    /// </summary>
    /// <param name="pEntity">Entity ID to list the item types for</param>
    /// <returns>Entity Object containing the list of ItemTypes</returns>
    //public Entity getItemTypes(string pEntity)
    //{
    //  return myAttributes.getItemTypes(pEntity);
    //}
    //public string[] getItemTypesRaw(string pEntity)
    //{
    //  return myAttributes.getItemTypesRaw(pEntity);
    //}
    //public string[] getItemsRaw(string pEntity, string pItemType)
    //{
    //  return myAttributes.getItemsRaw(pEntity, pItemType);
    //}
    /// <summary>
    /// Takes an Entity id and returns an Entity Object. Its behavior is influenced by the UseAttributes property.
    /// Overload with only the entity as parameter
    /// </summary>
    /// <param name="pEntity"></param>
    /// <returns>Returns an Entity object</returns>
    public Entity Entity(string pEntity, int handle)
    {
      return Entity(pEntity, false, DateTime.Now, "", handle);
    }
    
    /// <summary>
    /// Takes an Entity id and returns an Entity Object. Its behavior is influenced by the UseAttributes property.
    /// </summary>
    /// <param name="pEntity"></param>
    /// <param name="effectiveDate"></param>
    /// <returns>Returns an Entity object</returns>
    public Entity Entity(string pEntity, DateTime effectiveDate, int handle)
    {
      return Entity(pEntity, false, effectiveDate, "", handle);
    }

    /// <summary>
    /// Takes an Entity id and returns an Entity Object. Its behavior is influenced by the UseAttributes property.
    /// </summary>
    /// <param name="pEntity"></param>
    /// <param name="useAttributes"></param>
    /// <param name="effectiveDate"></param>
    /// <param name="itemTypesRequested"></param>
    /// <returns></returns>
    /// <returns>Returns an Entity object</returns>
    public Entity Entity(string pEntity, bool useAttributes, DateTime effectiveDate, string itemTypesRequested, int handle)
    {
      EntityAttributesCollection doEntities = new EntityAttributesCollection();
      DataSet dsEntity = myEntityDB.getEntity(pEntity);
      Entity myEntity = new Entity();
      LoadEntities(dsEntity, doEntities, useAttributes, effectiveDate, itemTypesRequested, handle);                   // Load looks at useAttributes and also automatically does a ResetDirty so we don't have to
      myEntity = doEntities.Entities[pEntity];
      return myEntity;
    }
    
    /// <summary>
    /// Accepts an Entity ID and returns that Entity's EntityType.
    /// </summary>
    /// <param name="pEntity"></param>
    /// <returns></returns>
    public string EntityType(string pEntity)
    {
      /*
       * finds and returns the EntityType of pEntity
       */
      DataSet dsEntity = new DataSet();
      dsEntity = myEntityDB.getEntity(pEntity);
      if (dsEntity == null || dsEntity.Tables.Count == 0 || dsEntity.Tables[0].Rows.Count == 0 ||
        !dsEntity.Tables[0].Columns.Contains("EntityType"))
        return string.Empty;
      else
      return dsEntity.Tables[0].Rows[0]["EntityType"].ToString();
    }
    
    /// <summary>
    /// Accepts an Entity ID and returns that Entity's EntityOwner.
    /// </summary>
    /// <param name="pEntity"></param>
    /// <returns>The EntityOwner</returns>
    public string EntityOwner(string pEntity)
    {
      /*
       * finds and returns the EntityID of the Entity that is the owner of pEntity
       */
      DataSet dsEntity = new DataSet();
      dsEntity = myEntityDB.getEntity(pEntity);
      return dsEntity.Tables[0].Rows[0]["EntityOwner"].ToString();
    }
    
    /// <summary>
    /// Overload of EntityOwner that goes up the entity ownership chain and finds the first entity up from this one that
    /// matches the EntityType. If none is found, returns "None". If one is found, return the EntityID for it.
    /// </summary>
    /// <param name="pEntity"></param>
    /// <param name="pEntityType"></param>
    /// <returns></returns>
    public string EntityOwner(string pEntity, string pEntityType)
    {
      /*
       * goes up the entity ownership chain and finds the first entity up from this one that
       * matches the EntityType. If none is found, returns "None". If one is found, return the EntityID for it.
       */
      string testEntityType = pEntityType.ToLower();
      string myEntityType = "";
      string myEntityOwner = pEntity;
      string myEntity = pEntity;
      bool bEOF = false;
      /*
       * we set a counter to compare with cMAXDEPTH. This is just to make sure we don't end up in a data loop. 
       * We could have set up a chain of entities that create a loop of ownership. To avoid this, we only look upward
       * cMAXDEPTH times. If we exceed this, we throw an error
       */
      int iCounter = 0;
      while (myEntityType != testEntityType && bEOF == false && iCounter <= cMAXDEPTH)   // keep looking until we find the reocrd, or run out of records, or exceed cMAXDEPTH
      {
        DataSet dsEntity = new DataSet();
        dsEntity = myEntityDB.getEntity(myEntityOwner); // get this Entity record
        if (dsEntity == null || dsEntity.Tables.Count == 0)               // Empty dataset?
          bEOF = true;                // set the EOF flag
        else
        {                       // otherwise, pick up the data from this record
          if (dsEntity.Tables[0].Rows.Count == 0)
            bEOF = true;
          else
          {
            myEntity = dsEntity.Tables[0].Rows[0]["Entity"].ToString();       // pick up the Entity from this record                         
            myEntityType = dsEntity.Tables[0].Rows[0]["EntityType"].ToString().ToLower();   // and the EntityType for this record
            myEntityOwner = dsEntity.Tables[0].Rows[0]["EntityOwner"].ToString();   // pick up the ID for the owner from this record
          }
        }
        iCounter++;                                 // incprement the depth counter
      }
      if (bEOF)
      {
        return "None";
      }
      else
      {
        if (iCounter > cMAXDEPTH)                           // we exceeded our depth counter
        {
          return "None";
        }
        else
          /*
           * OK, we found a record upward in the ownership chain whose type matches pEntityType. So we return the EntityID
           */
          return myEntity;
      }

    }
    
    /// <summary>
    /// Return a list of entityTypes valid for the current entity (TODO: Need to review this description)
    /// </summary>
    /// <returns></returns>
    public string[] EntityTypes()
    {
      DataSet dsEntityTypes = myEntityDB.getEntityTypes();
      if (dsEntityTypes == null || dsEntityTypes.Tables.Count == 0)
        return new string[0];
      string[] entityTypes = new string[dsEntityTypes.Tables[0].Rows.Count];
      int i = 0;
      foreach (DataRow row in dsEntityTypes.Tables[0].Rows)
        entityTypes[i++] = row["EntityType"].ToString();
      return entityTypes;
    }

    #region entityAttributes getAttributes calls

    /// <summary>
    /// Overload that supports a request for Raw Mode. Note that you cannot
    /// request Raw Mode for more than one Entity, ItemType, or Item. This
    /// also means that none of them can be null if rawMode = true.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="itemTypes"></param>
    /// <param name="items"></param>
    /// <param name="parameters"></param>
    /// <param name="efDate"></param>
    /// <param name="useAttributes"></param>
    /// <param name="rawMode"></param>
    /// <returns></returns>
    public EntityAttributesCollection getAttributes(string entities, string itemTypes,
      string items, string parameters, object efDate, bool useAttributes, bool rawMode, int handle)
    {
      if (!useAttributes)
        return new EntityAttributesCollection();

      if (rawMode)
      {
        //if (entities == null || entities.Contains(",")) // can't do raw mode with more than one entity
        //  throw new Exception("Cannot invoke Raw Mode with more than one Entity");
        //else
        //  if (itemTypes == null || itemTypes.Contains(",")) // or item types
        //    throw new Exception("Cannot invoke Raw Mode with more than one ItemType");
        //  else
            //if (items == null || items.Contains(",")) // or items
        //    if (items != null && items.Contains(",")) // or items
        //      throw new Exception("Cannot invoke Raw Mode with more than one Item");
        //    else
            {
              DateTime myEffectiveDate;
              if (efDate == null)
                myEffectiveDate = DateTime.Today;
              else
                myEffectiveDate = Convert.ToDateTime(efDate);

              EntityAttributesCollection d = new EntityAttributesCollection();
              d = LoadEntities(entities, itemTypes, items, parameters, myEffectiveDate, rawMode, false, true, handle);

              return d;
            }
      }
      else
        return getAttributes(entities, itemTypes, items, parameters, efDate, useAttributes, handle);
    }

    /// <summary>
    /// Method that loads the attributes of the passed list of entities, itemTypes, items etc. 
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="itemTypes"></param>
    /// <param name="items"></param>
    /// <param name="parameters"></param>
    /// <param name="efDate"></param>
    /// <param name="useAttributes"></param>
    /// <returns></returns>
    public EntityAttributesCollection getAttributes(string entities, string itemTypes,
      string items, string parameters, object efDate, bool useAttributes, int handle)
    {
      DateTime edate = DateTime.Now;
      if (efDate != null)
        edate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, efDate);
      EntityAttributesCollection d = new EntityAttributesCollection();
      //d = LoadEntities(entities, itemTypes, items, parameters, edate, false, false, useAttributes);
      d = LoadEntities(entities, itemTypes, items, parameters, edate, false, false, true, handle);

      return d;
    }

    /// <summary>
    /// Method that supports a request for attributes with tre etrieveVirtualItems set to 'true'
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="itemTypes"></param>
    /// <param name="items"></param>
    /// <param name="parameters"></param>
    /// <param name="efDate"></param>
    /// <param name="useAttributes"></param>
    /// <returns></returns>
    public EntityAttributesCollection getAttributes_withVirtualItems(string entities, string itemTypes,
      string items, string parameters, object efDate, bool useAttributes, int handle)
    {
      DateTime edate = DateTime.Now;
      if (efDate != null)
        edate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, efDate);
      EntityAttributesCollection d = new EntityAttributesCollection();
      //d = LoadEntities(entities, itemTypes, items, parameters, edate, false, false, useAttributes);
      d = LoadEntities(entities, itemTypes, items, parameters, edate, false, true, true, handle);
      return d;
    }

    /// <summary>
    /// method that will include BlockInheritedYN = true items. Note this does NOT enrich the entity info like the other items
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="itemTypes"></param>
    /// <param name="efDate"></param>
    /// <returns></returns>
    public EntityAttributesCollection getAttributes_withBlockedItems(string entities, string itemTypes, object efDate, int handle)
    {
      DateTime edate = DateTime.Now;
      if (efDate != null)
        edate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, efDate);

      if(entities.ToLower() == Constants.DEFAULT_ENTITY)
        return myAttributes.getSystemAttributes(entities, itemTypes, edate, true, handle);
      else
        return myAttributes.getAttributes(entities, itemTypes, edate, true, handle);

    }

    #endregion entityAttributes getAttributes calls

    /// <summary>
    /// This exposes the entity hierarchy to the calling client...
    /// </summary>
    /// <param name="pEntity"></param>
    /// <param name="inRawMode"></param>
    /// <returns></returns>
    //public EntityAttributesCollection EntityHierarchy(string pEntity, bool inRawMode)
    //{
    //  return myAttributes.EntityHierarchy(pEntity, inRawMode);
    //}

    /// <summary>
    /// Finds and returns a set of Entities that match the where clause. Its behavior is influenced by the UseAttributes property.
    /// <returns>A EntityAttributesCollection set of Entities.</returns>
    /// </summary>
    /// <param name="pWhereClause"></param>
    /// <param name="useAttributes"></param>
    /// <param name="effectiveDate"></param>
    /// <param name="itemTypesRequested"></param>
    /// <returns></returns>
    public EntityAttributesCollection EntitiesWhere(string pWhereClause, bool useAttributes, DateTime effectiveDate, string itemTypesRequested, int handle)
    {
      /*
     * finds a set of Entities that match the where clause
     */
      EntityAttributesCollection doEntities = new EntityAttributesCollection();
      DataSet dsEntity = new DataSet();
      dsEntity = myEntityDB.getEntitiesWhere(pWhereClause);   // get the data from the database
      LoadEntities(dsEntity, doEntities, useAttributes, effectiveDate, itemTypesRequested, handle);           // load it into doEntities
      return doEntities;                    // and return it
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pEntity"></param>
    /// <param name="pEntityType"></param>
    /// <param name="levels"></param>
    /// <param name="useAttributes"></param>
    /// <param name="effectiveDate"></param>
    /// <param name="itemTypesRequested"></param>
    /// <returns></returns>
    public EntityAttributesCollection EntityChildren(string pEntity, string pEntityType, int levels, 
      bool useAttributes, DateTime effectiveDate, string itemTypesRequested, int handle)
    {
      ArrayList OmitEntityTypes = dontProcessEntityTypes;  
      string entityType = pEntityType.ToLower();
      if (levels == 1)
        for (int i = 0; i < dontProcessEntityTypes.Count; i++)
          if (dontProcessEntityTypes[i].ToString().ToLower() == entityType)
            OmitEntityTypes.Remove(dontProcessEntityTypes[i]);

      return EntityChildren(pEntity, pEntityType, OmitEntityTypes, false, itemTypesRequested, effectiveDate, false, false, levels, useAttributes, handle);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pEntity"></param>
    /// <param name="pEntityType"></param>
    /// <param name="DoNotProcessEntityTypes"></param>
    /// <param name="pIncludeOnlyBottomLevel"></param>
    /// <param name="pItemTypesRequested"></param>
    /// <param name="effectiveDate"></param>
    /// <param name="includeParentsChildren"></param>
    /// <param name="includeTerms"></param>
    /// <param name="maxlevels"></param>
    /// <param name="useAttributes"></param>
    /// <returns></returns>
    public EntityAttributesCollection EntityChildren(string pEntity, string pEntityType,
      ArrayList DoNotProcessEntityTypes, bool pIncludeOnlyBottomLevel, string pItemTypesRequested,
      DateTime effectiveDate, bool includeParentsChildren, bool includeTerms, int maxlevels, bool useAttributes, int handle)
    {
      EntityAttributesCollection children = LoadEntityChildren(pEntity, pEntityType,
        DoNotProcessEntityTypes, pIncludeOnlyBottomLevel, effectiveDate, 
        includeParentsChildren, includeTerms, maxlevels, useAttributes, pItemTypesRequested, handle);
      return children;      // Either way, return the collection
    }
    public EntityHierarchy EntityChildren(string pEntity, int levels, ArrayList DoNotProcessEntityTypes, DateTime effectiveDate)
    {
      return EntityHierarchyChildren(pEntity, levels, DoNotProcessEntityTypes, effectiveDate, false);
    }
    /// <summary>
    /// Recursive call that gets all of the children of an entity to a certain level. 
    /// </summary>
    /// <param name="pEntity"></param>
    /// <param name="levels"></param>
    /// <param name="DoNotProcessEntityTypes"></param>
    /// <param name="effectiveDate"></param>
    /// <returns>EntityHierarchy is a collectio of itself. Each one contains the entityID, and a collection of EntityHiea</returns>
    public EntityHierarchy EntityHierarchyChildren(string pEntity, int levels, ArrayList DoNotProcessEntityTypes, 
      DateTime effectiveDate, bool includeTerms)
    {
      Type stringType = string.Empty.GetType();
      string[] dontProcessTypes;
      EntityHierarchy eh = new EntityHierarchy();
      eh.ID = pEntity;
      DataSet dsEntity = myEntityDB.getEntity(pEntity);
      
      if (DoNotProcessEntityTypes != null && DoNotProcessEntityTypes.Count > 0)
        dontProcessTypes = (string[])DoNotProcessEntityTypes.ToArray(stringType);
      else
        dontProcessTypes = new string[0];

      if (dsEntity.Tables.Count > 0 && dsEntity.Tables[0].Rows.Count > 0)
      {
        eh.Entity = loadEntity(dsEntity.Tables[0].Rows[0]);
        if (levels-- > 0)
        {
          DataSet dsEntities = myEntityDB.getEntityChildren(pEntity, null, effectiveDate, includeTerms, false);
          if (dsEntity.Tables.Count > 0)
            foreach (DataRow row in dsEntities.Tables[0].Rows)
              if (!(bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.inList, dontProcessTypes, row["EntityType"].ToString()))
                eh.Add(EntityHierarchyChildren(row["Entity"].ToString(), levels, null, effectiveDate, includeTerms));
        }
      }
      return eh;
    }

    /// <summary>
    /// Checkss if an entity exists in the DB
    /// </summary>
    /// <param name="entityID"></param>
    /// <returns></returns>
    public bool existsEntity(string entityID)
    {
      return myEntityDB.existsEntity(entityID);
    }

    /// <summary>
    /// Checks if the FEIN field exists in the DB
    /// </summary>
    /// <param name="fein"></param>
    /// <returns></returns>
    public bool existsFEIN(string fein)
    {
      return myEntityDB.existsFEIN(fein);
    }

    /// <summary>
    /// returns a picklist with ItemType/Item that matches the itemTypes list from the default entity
    /// </summary>
    /// <param name="itemTypes">List of ItemTypes to select</param>
    /// <param name="attributeFilterList">Optional list of attribute name/value pairs in picklist format. use Null of not filter. If present, all 
    /// Items must contain all of the attributes in the list and have values that match the values specified</param>
    /// <returns></returns>
    public PickList getDefaultItemList(string itemTypes, PickList attributeFilterList)
    {
        return myAttributes.getDefaultItemList(itemTypes, attributeFilterList);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parameterNames"></param>
    /// <param name="parameterValues"></param>
    /// <param name="parameterDateTypes"></param>
    /// <param name="parameterDirection"></param>
    /// <param name="spName"></param>
    /// <returns></returns>
    public DataSet executeQueryStoredProcedure(string[] parameterNames, object[] parameterValues,
      string[] parameterDateTypes, string[] parameterDirection, string spName)
    {
      return myEntityDB.executeQueryStoredProcedure(parameterNames, parameterValues, parameterDateTypes, parameterDirection, spName);
    }

    /// <summary>
    /// Private version which accepts just one Entity object. This version looks to see if the e_Entity record needs to be saved.
    /// If so, it reads the e_Entity record for update, and then updates it.
    /// </summary>
    /// <param name="e"></param>
    /// <param name="userID"></param>
    public void SaveEntityRecord(Entity e, string userID)
    {
      // Note if we are here then we already know e.Dirty is true and e.ReadOnly is false
      if (e.Deleted)
      {
        // delete the record here... This is not supported at this time
      }
      else
      {
        // this check was supposed to be performed before this method was invoked, but just in case...
        if (e.Dirty && e.Fields.Dirty)
        {
          /*
           * first, we set the last modified by to the user we got passed in, so it is saved properly
           */
          if (e.Fields.Contains("LastModifiedBy"))
          {
            e.Fields["LastModifiedBy"].Value = userID;
          }
          else
          {
            Field f = new Field();
            f.ID = "LastModifiedBy";
            f.Value = userID;
            e.Fields.Add(f);
          }
          // This collection may come from the gui and not be a complete list of fields. 
          // If it does not have the enitty id, the sql update will fail. So, we add it
          if (!e.Fields.Contains("Entity"))
          {
            Field f = new Field();
            f.ID = "Entity";
            f.Value = e.OriginalID;
            e.Fields.Add(f);
          }
          // then we update the record
          myEntityDB.SaveEntityRecord(e);             // add or update the record
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="e"></param>
    /// <param name="userID"></param>
    /// <param name="useAttributes"></param>
    ////public void SaveAttributes(Entity e, string userID, bool useAttributes)
    //public void SaveAttributes(Entity e, string userID)
    //{
    //  SaveAttributes(e, userID, false);
    //}
    public void SaveAttributes(Entity e, string userID, bool isRawMode)
    {
      //if (useAttributes && e.Dirty)                 // just making sure before we do the update...
      if (e.Dirty)                 // just making sure before we do the update...
      {
        if (e.Fields.Contains("LastModifiedBy"))    // set the lastmodifiedby in the entity, which is used by the save for the TAGAttribute record
          e.Fields["LastModifiedBy"].Value = userID;
        else
        {
          Field f = new Field();
          f.ID = "LastModifiedBy";
          f.Value = userID;
          e.Fields.Add(f);
        }

        saveAttributes(e, isRawMode, userID);
      }
    }
    private void saveAttributes(Entity entity, bool isRawMode, string userID)
    {
      string lastModifiedBy = userID;
      if (entity.Fields.Contains("LastModifiedBy"))             // this should ALWAYS be true, but just in case...
        lastModifiedBy = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, entity.Fields["LastModifiedBy"].Value); // pick up the lastmodifiedby from the entity
     
      if (entity != null)
      {
        foreach (ItemType it in entity.ItemTypes)
        {
          if (it != null)
          {
            foreach (Item i in it.Items)
            {
              if (i != null)
              {
                foreach (TAGAttribute a in i.Attributes)
                {
                  if (a.Dirty && !a.ReadOnly)
                  {
                    a.LastModifiedBy = lastModifiedBy;
                    foreach (ValueHistory vh in a.Values)
                      if (vh.Dirty)
                        vh.LastModifiedBy = lastModifiedBy;
                    i.Dirty = true;
                    break;
                  }
                }
                if (i.Dirty && !i.ReadOnly)
                {
                  if (i.Deleted)
                  {
                    // delete the record here... Not supported at this time
                    // LLA We need to change itemhistory dates to term the record instead
                    //if (isRawMode) 
                    //{
                    myAttributeDB.DeleteItem(entity.ID, it.ID, i.ID);
                    //}
                  }
                  else
                  {
                    //if (IsTermed(i))        // if the user or someone has terminated ed this item 
                    //  foreach (TAGAttribute a in i.Attributes)  // then terminate all of the attributes
                    //    if (!a.IsInherited || a.Dirty) // if this attribute was not inherited or it was modified
                    //    {
                    //      a.EndDate = i.EndDate;      // then terminate it with the item end date
                    //      a.Dirty = true;             // and set the dirty flag so it gets saved
                    //    }
                    i.LastModifiedBy = lastModifiedBy;  // update the item lastmodified by to the current user
                    string newXML = "";
                    string newXMLItemHistory = "";
                    // Set the End Date
                    //Object tmpEndDate = null; 
                    // TODO: enddate is not correct?
                    //if (i.EndDate != TAGFunctions.FutureDateTime)   // this is the standard "way out" future date/time
                    //  tmpEndDate = i.EndDate;               // otherwise, set to the value we have    

                    //myAttributeDB.BeginTransaction();         // Begin a transaction so we lock the record when we read it
                    // Read a raw record:
                    DataSet dsAttribute = myAttributeDB.get_Attribute(entity.OriginalID, it.OriginalID, i.OriginalID);
                    newXMLItemHistory = i.ToItemHistoryXML();   // load the item history xml before we store it
                    if (isRawMode || dsAttribute.Tables.Count == 0 || dsAttribute.Tables[0].Rows.Count == 0) // did we get a record?
                    {
                      // nope, we need to add it. 
                      if (isRawMode)
                        newXML = i.ToXML("", true);           // First build the XML for RAW MODE
                      else
                        newXML = i.ToXML("");           // First build the XML
                      myAttributeDB.UpdateAttribute(entity.OriginalID, it.OriginalID, i.OriginalID,
                          newXMLItemHistory, newXML, i.LastModifiedBy, DateTime.Now);
                    }
                    else
                    {
                      // otherwise we update the record
                      // Update the XML to use the new values (if any) for attributes for this Item
                      newXML = i.ToXML(dsAttribute.Tables[0].Rows[0]["Attributes"].ToString());

                      // Pass the rest of the values from this item and update the record
                      //    Note: if this record does not exists, this call will insert it
                      myAttributeDB.UpdateAttribute(entity.OriginalID, it.OriginalID, i.OriginalID, newXMLItemHistory,
                                      newXML, i.LastModifiedBy, DateTime.Now);
                    }
                    //if (!myAttributeDB.Commit())     // now commit... and check if it was successfull
                    //  throw new Exception("Commit of TAGAttribute save failed");
                  }
                }
              }
            }
          }
        }
      }
    }

    public SearchResultCollection Search(string criteria, string entityRoot, string entityType, bool includeOnlyBottomLevel, 
      bool includeTerms, DateTime effectiveDate, bool useHiearchy)
    {
      SearchResultCollection result = new SearchResultCollection();
      string strCriteria = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, criteria);
      return Search(result, strCriteria, entityRoot, entityType, includeOnlyBottomLevel, includeTerms, effectiveDate, useHiearchy);
    }
    public PickList getIssueEntityList()
    {
      string sql = "select distinct entity from entity e where e.entitytype in ('User', 'Client')";
      DataSet ds = myEntityDB.getDataFromSQL(sql);
      PickList returnList = new PickList(new string[] { "Entity" });
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return returnList;
      returnList.AddRows(ds.Tables[0].Rows.Count);
      int iRow = 0;
      foreach (DataRow row in ds.Tables[0].Rows)
        returnList[iRow++, 0] = row["Entity"];
      return returnList;
    }
    #endregion public methods

    #region private methods
    private SearchResultCollection Search(SearchResultCollection result, string criteria, string entityRoot, string entityType, 
      bool includeOnlyBottomLevel, bool includeTerms, DateTime effectiveDate, bool useHierarchy)
    {
      if (string.IsNullOrEmpty(entityRoot) && useHierarchy)
        return result;
      DataSet dsChildren;
      if (useHierarchy)
        dsChildren = myEntityDB.getEntityChildren(entityRoot, null, effectiveDate, includeTerms, true);
      else
        dsChildren = myEntityDB.searchEntities(criteria, effectiveDate, entityType, entityRoot, includeTerms);
        //dsChildren = myEntityDB.getEntityChildren(entityRoot, null, effectiveDate, includeTerms, true,entityType);
      if (dsChildren == null || dsChildren.Tables.Count == 0 || dsChildren.Tables[0].Rows.Count == 0)
        return result;
      bool hasEntityPath = dsChildren.Tables[0].Columns.Contains("EntityPath");
      foreach (DataRow row in dsChildren.Tables[0].Rows)
      {
        string childType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row["EntityType"]);
        string childID = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row["Entity"]);
        if (string.IsNullOrEmpty(entityType) || childType.Equals(entityType, StringComparison.CurrentCultureIgnoreCase))
        {
          SearchResult entry = new SearchResult();
          entry.EntityID = childID;        
          string legalName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row["LegalName"]);
          string firstName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row["FirstName"]);
          string middleName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row["MiddleName"]);
          string altName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row["AlternateName"]);
          string altID = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row["AlternateID"]);
          string fein = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row["FEIN"]);
          if (hasEntityPath)
            entry.EntityPath = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row["EntityPath"]);
          string shortName;
          entry.FullName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.makeFullName,
            entry.EntityID, childType, legalName, firstName, middleName, fein, altName, altID, out shortName);
          entry.ShortName = shortName;
          entry.LegalName = legalName;
          if (entry.MeetsCriteria(criteria) && !result.Contains(entry.EntityID))
            result.Add(entry.ID, entry);
        }
        if (useHierarchy && (string.IsNullOrEmpty(entityType) || childType != entityType) && childType != cETYPE)
          result = Search(result, criteria, childID, entityType, includeOnlyBottomLevel, includeTerms, effectiveDate, useHierarchy);
      }
      return result;
    }
    private EntityAttributesCollection LoadEntityChildren(string pEntity, string pEntityType,
      ArrayList DoNotProcessEntityTypes, bool pIncludeOnlyBottomLevel, DateTime pEffectiveDate,
      bool includeParentsChildren, bool includeTerms, int maxLevels, bool useAttributes, string itemTypesRequested, int handle)
    {
      /*
       * Using pEntity as the top of the tree, build a list of all children down to where the bottom tier matches pEntityType. 
       * If no EntityType match is found, then all children will be found and included in the list.
       * 
       * This works by building the list from the immediate children of an Entity, and then calling itself recursively for each child
       * to add whatever children that child has.
       *
       * DoNotProcessEntityTypes is used to process the list of EntityTypes that we want to exclude from the list. If the
       * ArrayList is null or empty, we exclude nothing.
       * 
       * LLA 10/28/2010: Added new support for no entityType specified (pEntityType == null). This means any entity that matches criteria will 
       * be included.
       */
      string entityType = null;
      if (pEntityType != null)
        entityType = pEntityType.ToLower();
      if (entityType == null)
        pIncludeOnlyBottomLevel = false;
      int DoNotProcessCount;
      int i;

      if (DoNotProcessEntityTypes == null)
        DoNotProcessCount = 0;
      else
        DoNotProcessCount = DoNotProcessEntityTypes.Count;
      /*
       * Get the list of children at this level
       */
      string myEntityType;
      string myEntity;
      EntityAttributesCollection children = new EntityAttributesCollection();
      if (pEntity == "None")
        return children;
      DataSet dsEntityChildren = new DataSet();             // the dataset for this level which lists Entity Children
      dsEntityChildren = myEntityDB.getEntityChildren(pEntity, DoNotProcessEntityTypes, pEffectiveDate, includeTerms, false);     // ask for the list of Entity Children from the DAL
      for (i = 0; i < dsEntityChildren.Tables[0].Rows.Count; i++)   // for each record in the dataset
      {
        myEntityType = dsEntityChildren.Tables[0].Rows[i]["EntityType"].ToString().ToLower();
        myEntity = dsEntityChildren.Tables[0].Rows[i]["Entity"].ToString();
        /*
        * load the Entity Object with the values we need
        * However, don't do this if we are only supposed to include only the bottom level
        * and this is not the target entity type.
        */
        bool canRecurse = (maxLevels > 1 || maxLevels < 0);   // we have a positive max levels, or we have infinite (-1)
        if (entityType == null || myEntityType == entityType || (canRecurse && !pIncludeOnlyBottomLevel))
          AddEntity(dsEntityChildren.Tables[0].Rows[i], children, null, null, null, useAttributes, pEffectiveDate, itemTypesRequested, handle);
        // if the EntityType is not the cutoff one specified (and Employees don't have children
        if (  (entityType == null || ( myEntityType != entityType && myEntityType != cETYPE) ) 
                && canRecurse)
        {
          EntityBase tmpEntities = new EntityBase();    // create a new instance so recursion works properly
          EntityAttributesCollection newList;
          // then load any children THIS entity might have
          newList = 
            tmpEntities.EntityChildren(myEntity, pEntityType,
              DoNotProcessEntityTypes, pIncludeOnlyBottomLevel, itemTypesRequested, 
              pEffectiveDate, false, includeTerms, --maxLevels, useAttributes, handle);     // get list list of children
              children.Entities.Append(newList.Entities);                         // and append to our list
        }
      }
      if (includeParentsChildren && pEntityType != null)  // includeParentsChildren does not work if there is no entityType specified (entittType = null)
      {
        children = LoadEntitiesWithParentsDescendents(children, pEntity, pEntityType, DoNotProcessEntityTypes,
          pEffectiveDate, includeTerms, useAttributes, itemTypesRequested, handle);
      }
      children.Entities.Dirty = false;         // newly loaded so set Dirty flags to false
      return children;                              //  Success

    }
    
    /// <summary>
    /// Goes up the entity heirarchy chain and looks for parents at each level. Then adds
    /// any direct descendents of that parent that are of type childEntityType. 
    /// When we reach the top, then return then final collection.
    /// </summary>
    /// <param name="childrenIn"></param>
    /// <param name="pEntity"></param>
    /// <param name="childEntityType"></param>
    /// <param name="doNotProcessEntityTypes"></param>
    /// <param name="pEffectiveDate"></param>
    /// <param name="includeTerms"></param>
    /// <param name="useAttributes"></param>
    /// <param name="itemTypesRequested"></param>
    /// <returns></returns>
    private EntityAttributesCollection LoadEntitiesWithParentsDescendents(
      EntityAttributesCollection childrenIn, string pEntity, 
      string childEntityType, ArrayList doNotProcessEntityTypes,
      DateTime pEffectiveDate, bool includeTerms, bool useAttributes, string itemTypesRequested, int handle)
    {
      string entityOwner = EntityOwner(pEntity);    // find my parent
      if (entityOwner.ToLower() == "none")          // If I don't have one
        return childrenIn;                          // then the recursion is done, so just exit
      else
      {
                                                    // otherwise, find this parent's children, and then recurse
        DataSet dsEntityChildren = new DataSet();             // the dataset for this level which lists Entity Children
        string checkEntityType = childEntityType.ToLower();
        dsEntityChildren = myEntityDB.getEntityChildren(pEntity, doNotProcessEntityTypes, pEffectiveDate, includeTerms, false);
        for (int i = 0; i < dsEntityChildren.Tables[0].Rows.Count; i++)   // for each record in the dataset
        {
          string entityType = dsEntityChildren.Tables[0].Rows[i]["EntityType"].ToString().ToLower();
          if (checkEntityType == entityType)
            AddEntity(dsEntityChildren.Tables[0].Rows[i], childrenIn, null, null, null, useAttributes, pEffectiveDate, itemTypesRequested, handle);
        }
        childrenIn = LoadEntitiesWithParentsDescendents(childrenIn, entityOwner, childEntityType,
          doNotProcessEntityTypes, pEffectiveDate, includeTerms, useAttributes, itemTypesRequested, handle);
        return childrenIn;
      }
    }
  
    /// <summary>
    /// LoadEntities that loads the doList EAS with the passed DataSet
    /// </summary>
    /// <param name="dsSource"></param>
    /// <param name="doList"></param>
    /// <param name="useAttributes"></param>
    /// <param name="effectiveDate"></param>
    /// <param name="itemTypesRequested"></param>
    /// <returns></returns>
    private EntityAttributesCollection LoadEntities(DataSet dsSource, EntityAttributesCollection doList, bool useAttributes, DateTime effectiveDate, string itemTypesRequested, int handle)
    {
      int i;
      if (dsSource == null || dsSource.Tables.Count == 0 || dsSource.Tables[0].Rows.Count == 0)   // do nothing if there are no records
        return doList;
      for (i = 0; i < dsSource.Tables[0].Rows.Count; i++)
      {
        AddEntity(dsSource.Tables[0].Rows[i], doList, null, null, null, useAttributes, effectiveDate, itemTypesRequested, handle);
      }
      doList.Entities.Dirty = false;  // reset dirty flags to off
      return doList;
    }

    /// <summary>
    /// LoadEntities main function that uses raw mode flag and the retrieveVirtualItems flag
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="itemTypes"></param>
    /// <param name="items"></param>
    /// <param name="parameters"></param>
    /// <param name="effectiveDate"></param>
    /// <param name="inRawMode"></param>
    /// <param name="retrieveVirtualItems"></param>
    /// <param name="useAttributes"></param>
    /// <returns></returns>
    private EntityAttributesCollection LoadEntities(string entities, string itemTypes, string items,
      string parameters, object effectiveDate, bool inRawMode, bool retrieveVirtualItems, bool useAttributes, int handle)
    {
      /*
       * A version of Load Entities that calles the TAGAttribute Object first, then loads the individual
       * Entity Fields collections afterward
       */
      DataSet dsEntities = myEntityDB.getEntities(entities);
      DateTime eDate = DateTime.Today;
      if (effectiveDate != null)
        eDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, effectiveDate);
      EntityAttributesCollection doList = new EntityAttributesCollection();
      if (dsEntities == null || dsEntities.Tables.Count == 0 || dsEntities.Tables[0].Rows.Count == 0)
        return doList;    // return an empty collection if there are no records
      
      //Load the entity's attributes...
      if (useAttributes)
      {
        if (!inRawMode && entities.ToLower() == Constants.DEFAULT_ENTITY)
          doList = myAttributes.getSystemAttributes(entities, itemTypes, items, parameters, eDate, inRawMode, retrieveVirtualItems, handle);
        else
          doList = myAttributes.getAttributes(entities, itemTypes, items, parameters, eDate, inRawMode, retrieveVirtualItems, handle);
      }

      else
        doList = new EntityAttributesCollection();
      
      /*
       * Now, go through the rows in the dataset, find the fields, and then load the fields
       * for each entity
       */
      DataColumnCollection cols = dsEntities.Tables[0].Columns;
      foreach (DataRow row in dsEntities.Tables[0].Rows)
      {
        // find the entity this set of fields belongs to in the attribute data object
        Entity e;
        string entityID = row["Entity"].ToString();
        if (doList.Entities.Contains(entityID))
          e = doList.Entities[entityID];
        else
        {
          e = new Entity();
          e.ID = entityID;
          doList.Entities.Add(e);
        }
        // Changed by LLA 1/18/2010 so we do this in a consistant way
        loadEntity(row, e); // populate the entity fields
        //// for each field in the row...
        //foreach (DataColumn col in cols)
        //{
        //  // create the field object and then add it
        //  Field f = new Field();
        //  f.ID = col.ColumnName;
        //  f.Description = col.Caption;
        //  f.DataType = col.DataType.ToString();
        //  object oValue = row[col.ColumnName];
        //  if (f.DataType.ToLower() == TAGFunctions.DATATYPEDATETIME && oValue.Equals(global::System.DBNull.Value))
        //    oValue = null;
        //  f.Value = oValue;
        //  f.Virtual = false;
        //  f.Dirty = false;
        //  f.Deleted = false;
        //  e.Fields.Add(f);
        //}
      }
      doList.Entities.Dirty = false;  // reset dirty flags to off
      return doList;
    }

    private void AddEntity(DataRow myRow, EntityAttributesCollection doIn, string itemTypes, string items, 
      string parameters, bool loadAttributes, DateTime effectiveDate, string itemTypesRequested, int handle)
    {
      Entity myEntity = null;
      string myEntityID = myRow["Entity"].ToString();
      if (!doIn.Entities.Contains(myEntityID))                      // does the collection already have it?
      {
        myEntity = loadEntity(myRow);
        if (loadAttributes)                          // Do we load TAGAttribute Data, too?
        {
          string tmpItemTypeList = myItemTypes;
          string tmpParameterList = "<parameters>";
          tmpParameterList += "<EntityType value='" + myEntity.Fields["EntityType"].Value.ToString() + "'/>";
          tmpParameterList += "</parameters>";
          if (itemTypesRequested == null)
            itemTypesRequested = string.Empty;
          if (itemTypesRequested.Length != 0)
            tmpItemTypeList = tmpItemTypeList + "," + itemTypesRequested;
          EntityAttributesCollection doAttributes;
          doAttributes = myAttributes.getAttributes(myEntity.ID.ToString(), tmpItemTypeList, items, parameters, effectiveDate, false, false, handle);
          if (doAttributes.Entities.Count > 0)  // Did this invocation return any data?
          {
            if (doAttributes.Entities[0].ItemTypes != null)   // yes, so does the first entity have any ItemType objects?
              myEntity.ItemTypes = doAttributes.Entities[myEntity.ID.ToString()].ItemTypes;     // then pick it up
          }
        }
        setAddress(myEntity);
        doIn.Entities.Add(myEntity);                      // now add the entity to the collection
      }

    }

    private Entity loadEntity(DataRow myRow)
    {
      Entity myEntity = new Entity();   // Entity Class holds data elements we need
      return loadEntity(myRow, myEntity);
    }
    private Entity loadEntity(DataRow myRow, Entity myEntity)
    {
      // TODO: why are the values ToString()? what about nulls? other values?
      myEntity.ID = myRow["Entity"].ToString();                   // pick up the Entity ID
      myEntity.Fields.AddField("Entity", myRow["Entity"].ToString());     // nope, so set up the fields
      myEntity.Fields.AddField("EntityType", myRow["EntityType"].ToString());
      myEntity.Fields.AddField("EntityOwner", myRow["EntityOwner"].ToString());
      myEntity.Fields.AddField("LegalName", myRow["LegalName"].ToString());
      myEntity.Fields.AddField("FirstName", myRow["FirstName"]);
      myEntity.Fields.AddField("AlternateName", myRow["AlternateName"]);
      myEntity.Fields.AddField("AlternateID", myRow["AlternateID"]);
      myEntity.Fields.AddField("MiddleName", myRow["MiddleName"]);
      TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.setFullName, myEntity.Fields, myRow);
      myEntity.Fields.AddField("FEIN", myRow["FEIN"]);
      myEntity.Fields.AddField("LastModifiedBy", myRow["LastModifiedBy"].ToString());
      if (!myRow["LastModifiedDateTime"].Equals(global::System.DBNull.Value))
        myEntity.Fields.AddField("LastModifiedDateTime", (DateTime)myRow["LastModifiedDateTime"]);
      else
        myEntity.Fields.AddField("LastModifiedDateTime", null);
      if (myRow["StartDate"].Equals(global::System.DBNull.Value))
        myEntity.Fields.AddField("StartDate", pastDateTime);
      else
        myEntity.Fields.AddField("StartDate", (DateTime)myRow["StartDate"]);
      if (myRow["EndDate"].Equals(global::System.DBNull.Value))
        myEntity.Fields.AddField("EndDate", futureDateTime);
      else
        myEntity.Fields.AddField("EndDate", (DateTime)myRow["EndDate"]);
      TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.setClientName, myEntity.Fields, myRow); //set the client name virtual field.

      return myEntity;
    }

    private void setAddress(Entity e)
    {
      bool okToAdd = false;
      string address = null;
      if (e.ItemTypes.Contains("Entity"))
      {
        string entityType = e.Fields["EntityType"].ToString();
        if (e.ItemTypes["Entity"].Items.Contains(entityType))
        {
          Item i = e.ItemTypes["Entity"].Items[entityType];
          if (i.Attributes.Contains("Address1"))
          {
            address = i.Attributes["Address1"].Value.ToString();
            okToAdd = true;
          }
          if (i.Attributes.Contains("City"))
          {
            address += ", " + i.Attributes["City"].Value.ToString();
            okToAdd = true;
          }
          if (i.Attributes.Contains("State"))
          {
            address += ", " + i.Attributes["State"].Value.ToString();
            okToAdd = true;
          }
          if (i.Attributes.Contains("Zip"))
          {
            address += "  " + i.Attributes["Zip"].Value.ToString();
            okToAdd = true;
          }
        }
      }
      if (okToAdd)
      {
        e.Fields.AddField("Address", address);
        e.Fields["Address"].Virtual = true;
        e.Fields["FullName"].Description = "Address";
      }
    }
    #endregion private methods
  }
}
