using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using TAGBOSS.Common;
using TAGBOSS.Common.Model;

using CCI.Common;


namespace CCI.Sys.Data
{
  public class EntityAttributes
  {
    #region module data
    const string cENTITY = "entity";
    const string cLEGALNAME = "legalname";
    const string cFIRSTNAME = "firstname";
    const string cFEIN = "fein";
    const string cALTNAME = "alternatename";
    const string cALTID = "alternateid";
    const string cSHORTNAME = "shortname";
    const string cFULLNAME = "fullname";
    const string cEVENTSTATUS = "event.status";
    int mySecurityHandle = 0;
    //private Security mySecurity 
    //{ 
    //  get 
    //  {
    //    if (mySecurityHandle == 0)
    //      throw new Exception("Security has been accessed with a zero handle");
    //    return SecurityFactory.GetInstance().getSecurity(mySecurityHandle); 
    //  } 
    //}
    EntityBase entityBase;
    bool useSecurity = true;
    public string CurrentUser = null;
    Dictionaries dictionary = DictionaryFactory.getInstance().getDictionary();
    private string[] validEntityTypes = { "all", "admin", "none", "sb", "parent", "client", "customer", 
                                          "prospect", "ibpprospect", "employee", "user", "default", "dictionary", "vendor", "contact",
                                        "payee","payor","location", "carrier"};

    //TODO: Get rid of this temporary solution to the EntityBase refactoring, 
    //the following property variables must be killed from this class too
    //and passed by as parameters to calling functions
    //BEGIN: To be killed parameter variables
    private bool useAttributes = false;             // currently readonly false since TAGAttribute Object is not yet linked in
    private DateTime effectiveDate = DateTime.Now;              // Effective Date with default value of Now()
    protected string itemTypesRequested = "";
    //private EntityAttributesCollection entityList;  // list of Entities that this user has at least read access to
    private Dictionary<string, bool> entityList;
    //private bool useSecurity = false;               // currently readonly false since Security Object is not yet linked in
    //END: To be killed parameter variables

    string[] systemEntities = { "dictionary", "all", "default" };
    string[] entityTypesWithNoSecurity = { "paygroup", "parent" };

    #endregion module data

    #region constructors
    public EntityAttributes()
    {
      load(0);
    }
    private EntityAttributes(int securityHandle)
    {
      load(securityHandle);
    }
    /// <summary>
    /// Normal Constructor 
    /// </summary>
    /// <param name="securityHandle">Security Handle of the logged in user</param>
    /// <param name="hasAttributes">Should this object also retrieve the TAGAttribute data?</param>
    private EntityAttributes(int securityHandle, bool hasAttributes)
    {
      load(securityHandle);
      //useAttributes = hasAttributes; //TODO: Review this! this flag must be deprecated!
    }
    private void load(int securityHandle)
    {
      mySecurityHandle = securityHandle;
      entityBase = new EntityBase();
      //workflow = new Workflows(mySecurityHandle);
      ////entityBase = new EntityBase(hasAttributes);
      ////entityBase.UseSecurity = useSecurity;
      //currentUser = mySecurity.User;
      //entityList = mySecurity.EntityList;

      //eventTransactions = new EventTransactions(mySecurityHandle);
    }
    ///// <summary>
    ///// Overload that does not use Security. This is ONLY to be used by Action Programs, NEVER
    ///// by a GUI program or other object called directly or indirectly by a GUI program
    ///// </summary>
    ///// <param name="hasAttributes"></param>
    //public EntityAttributes(bool UseSecurity, bool hasAttributes)
    //{
    //  /*
    //   * Note: Even though it seems dumb to add the UseSecurity parameter when it always has
    //   * to have the value of false, I did this so that a developer would not accidentally
    //   * instantiate the object without the intent to bypass security. As the HTML comment
    //   * states, this constructor is only to be used by Action/Batch programs, not by 
    //   * any object that is called directly by a GUI
    //   */
    //  if (UseSecurity)
    //    throw new Exception("UseSecurity must be false for use by this Constructor");
    //  useSecurity = UseSecurity;
    //  entityBase = new EntityBase(hasAttributes);
    //  entityBase.UseSecurity = useSecurity;
    //  currentUser = "TAGPAY\\System";
    //}
    #endregion constructors

    #region public properties

    public int Handle
    {
      get { return mySecurityHandle; }
      set
      {
        mySecurityHandle = value;
      }
    }

    #endregion public properties

    #region public methods

    #region utility methods

    public bool IsEntityField(string fieldname)
    {
      return entityBase.IsEntityField(fieldname);
    }
    public string[] getValidEntityTypes() { return validEntityTypes; }
    public string getConnectionString()
    {
      return entityBase.ConnectionString;
    }
    public string getSchemaForQuery(string mySQL, string dataSetName)
    {
      return entityBase.getSchemaForQuery(mySQL, dataSetName);
    }
    /// <summary>
    /// Accepts an Entity ID and returns that Entity's EntityOwner.
    /// </summary>
    /// <param name="pEntity"></param>
    /// <returns>The EntityOwner</returns>
    public string EntityOwner(string pEntity)
    {
      //bool saveUseAttributes = entityBase.UseAttributes;
      //entityBase.UseAttributes = false;
      //Entity e = entityBase.Entity(pEntity);
      //entityBase.UseAttributes = saveUseAttributes;
      Entity e = entityBase.Entity(pEntity, false, effectiveDate, "", Handle);
      string entityType = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, e.Fields.getValue("EntityType"))).ToLower();
      string entityOwner = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, e.Fields.getValue("EntityOwner"));
      if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.inList, entityTypesWithNoSecurity, entityType))
        return entityOwner;
      if (validEntity(pEntity))
        return entityOwner;
      else
        return null;
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
      if (pEntityType == null)
        return EntityOwner(pEntity);
      else
        return entityBase.EntityOwner(pEntity, pEntityType);
    }
    /// <summary>
    /// Returns an Entity with a list of empty itemtypes for this entity. This does call the AttributeProcessor,
    /// but does not inherit items or itemtypes
    /// </summary>
    /// <param name="pEntity"></param>
    /// <returns></returns>
    //public Entity getItemTypes(string pEntity)
    //{
    //  return entityBase.getItemTypes(pEntity);
    //}
    //public string[] getItemTypesRaw(string pEntity)
    //{
    //  return entityBase.getItemTypesRaw(pEntity);
    //}
    //public string[] getItemsRaw(string pEntity, string pItemType)
    //{
    //  return entityBase.getItemsRaw(pEntity, pItemType);
    //}
    /// <summary>
    /// Looks up the short name for an entity, and returns it.
    /// </summary>
    /// <param name="entity">Entity you want the name of</param>
    /// <returns>Entity Name, or "No Results Found" if not found</returns>
    public string getEntityShortName(string entity)
    {
      string badName = "No Results Found";
      //bool saveUseAttributes = entityBase.UseAttributes;
      //entityBase.UseAttributes = false;
      //Entity thisEntity = Entity(entity);
      //entityBase.UseAttributes = saveUseAttributes;
      Entity thisEntity = Entity(entity, false);
      if (thisEntity == null)
        return badName;
      string shortName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, thisEntity.Fields.getValue("ShortName"));
      if (shortName == null || shortName == string.Empty)
        return badName;
      return shortName;
    }
    public string getBankName(string item)
    {
      string badName = "No Results Found";
      EntityAttributesCollection BankEac = getAttributes("Default", "Bank", item, null, DateTime.Now);
      if (BankEac == null)
        return badName;
      object bankName = BankEac.getValue("Default.Bank." + item + ".LegalName");
      if (bankName == null || bankName.ToString() == string.Empty)
        return badName;
      return bankName.ToString();
    }
    public PickList getBankPickList(string entityRoot)
    {
      return getEntityPickList(entityRoot, "Bank");
    }
    public PickList getVendorPickList(string entityRoot)
    {
      return getEntityPickList(entityRoot, "Vendor");
    }
    public PickList getEntityPickList(string entityRoot, string entityType)
    {
      //bool saveUseAttributes = entityBase.UseAttributes;
      //entityBase.UseAttributes = false;
      //ArrayList dontProcess = new ArrayList();
      //EntityAttributesCollection entityList = entityBase.EntityChildren(entityRoot, entityType, dontProcess, true, null, EffectiveDate, true, false, -1);
      //entityBase.UseAttributes = saveUseAttributes;
      ArrayList dontProcess = new ArrayList();
      EntityAttributesCollection entityList = entityBase.EntityChildren(entityRoot, entityType, dontProcess, true, null, DateTime.Today, true, false, -1, useAttributes, Handle);
      if (entityList == null || entityList.Entities.Count == 0)
        return new PickList();

      int nbrEntities = entityList.Entities.Count;
      PickList pickList = new PickList(new string[] { "Code", "Description" }, nbrEntities);

      int i = 0;
      foreach (Entity e in entityList.Entities)
      {
        pickList[i, 0] = e.OriginalID;
        pickList[i++, 1] = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, e.Fields.getValue("ShortName"));
        i++;
      }
      return pickList;
    }
    /// <summary>
    /// returns a picklist with ItemType/Item that matches the itemTypes list from the default entity
    /// </summary>
    /// <param name="itemTypes">List of ItemTypes to select</param>
    /// <param name="attributeFilterList">Optional list of attribute name/value pairs in picklist format. use Null of not filter. If present, all 
    /// Items must contain all of the attributes in the list and have values that match the values specified</param>
    /// <returns>PickList of Items and descriptions</returns>
    public PickList getDefaultItemList(string itemTypes, PickList attributeFilterList)
    {
      return entityBase.getDefaultItemList(itemTypes, attributeFilterList);
    }

    ///// <summary>
    ///// Overload that defaults to showParentsChildren=false and showTermed=false
    ///// </summary>
    ///// <param name="entityRoot"></param>
    ///// <param name="criteria"></param>
    ///// <param name="entityType"></param>
    ///// <returns></returns>
    //public EntityAttributesCollection Search(string entityRoot, string criteria, string entityType)
    //{
    //  return Search(entityRoot, null, criteria, entityType, EffectiveDate, false, false);
    //}
    public SearchResultCollection Search(string entityRoot, string rootEntityType, string criteria, string entityType, bool showTermed)
    {
      return Search(entityRoot, rootEntityType, criteria, entityType, showTermed, true);
    }
    public SearchResultCollection Search(string entityRoot, string rootEntityType, string criteria, string entityType, bool showTermed, bool useHierarchy)
    {
      bool showParentsChildren = false;
      if (entityType != null && entityType.Equals("Employee", StringComparison.CurrentCultureIgnoreCase))
        showParentsChildren = true;
      // LLA 10/28/2010: Added support for null entityType (none specified)
      string[] delimiters = { "." };
      SearchResultCollection eac = new SearchResultCollection();
      if ((entityRoot == null || entityRoot.Length == 0) && useHierarchy)
        return eac;
      if (entityType != null && !((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.inList, validEntityTypes, entityType.ToLower())))
        return eac;
      string entityRootType = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, rootEntityType)).ToLower();
      if (entityRoot != null && entityRoot.Equals("None", StringComparison.CurrentCultureIgnoreCase))
        entityRoot = "all";

        //if (string.IsNullOrEmpty(entityRootType))
          eac = entityBase.Search(criteria, entityRoot, entityType, true, showTermed, effectiveDate, useHierarchy);

        return eac;
        //return checkSecurity(eac);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eacIn"></param>
    /// <param name="criteria"></param>
    /// <returns></returns>
    public EntityAttributesCollection Search(EntityAttributesCollection eacIn, string criteria)
    {
      if (criteria == "*")
        return eacIn;
      EntityAttributesCollection eacNew = new EntityAttributesCollection();
      foreach (Entity e in eacIn.Entities)
      {
        if (meetsCriteria(e, criteria))
          eacNew.Entities.Add(e);
      }
      return eacNew;
    }

    /// <summary>
    /// Searches various fields in the entity
    /// </summary>
    /// <param name="e"></param>
    /// <param name="criteria">String to match anywhere inside one of the target fields</param>
    /// <returns>True if a match is found, false if not</returns>
    private bool meetsCriteria(Entity e, string criteria)
    {
      bool matchFound = false;
      if (criteria == null)
        return matchFound;
      string entityType = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, e.EntityType)).ToLower();
      foreach (Field f in e.Fields)
      {
        switch (f.ID)
        {
          case cENTITY:   // Don't look for a match in the entityId if this is an employee per Jack
            if (entityType != "employee")
              matchFound = (((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, f.Value)).ToLower().IndexOf(criteria.ToLower()) >= 0);
            break;
          case cLEGALNAME:
          case cFIRSTNAME:
          case cFEIN:
          case cALTID:
          case cALTNAME:
          case cSHORTNAME:
          case cFULLNAME:
          case cEVENTSTATUS:
            matchFound = (((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, f.Value)).ToLower().IndexOf(criteria.ToLower()) >= 0);
            break;
        }
        if (matchFound)
          break;
      }
      return matchFound;
    }

    /// <summary>
    /// Take the current user, and the entityType we want access to, and return the
    /// entityowner of the entities with that entityType that the user
    /// has access to.
    /// </summary>
    /// <param name="userEntity"></param>
    /// <param name="childEntityType"></param>
    /// <returns></returns>
    public string EntityRoot(string userEntity, string childEntityType, out string rootEntityType)
    {
      switch (childEntityType.ToLower())
      {
        case "payroll":
          rootEntityType = "client";
          break;
        case "employee":
          rootEntityType = "client";
          break;
        case "client":
          rootEntityType = "sb";
          break;
        case "sb":
          rootEntityType = "all";
          break;
        case "paygroup":
          rootEntityType = "client";
          break;
        case "parent":
          rootEntityType = "sb";
          break;
        default:
          rootEntityType = "";
          break;
      }
      if (rootEntityType == "")
        return "";
      string entityRoot = entityBase.EntityOwner(userEntity, rootEntityType);
      if (entityRoot == "none")
        entityRoot = "";
      return entityRoot;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pEntity"></param>
    /// <returns></returns>
    public bool existsEntity(string pEntity)
    {
      return entityBase.existsEntity(pEntity);
    }

    public bool existsChildrenOfType(string parentEntity, string entityType)
    {
      EntityAttributesCollection eacChildren =
        EntityChildren(parentEntity, entityType, null, true, null,
          effectiveDate, false, false, -1, false);
      if (eacChildren != null && eacChildren.Entities.Count > 0)
        return true;
      else
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entityOwner"></param>
    /// <param name="lastName"></param>
    /// <param name="firstName"></param>
    /// <param name="FEIN"></param>
    /// <returns></returns>
    private NewEntityReturn newEntityID(string newID, string entityOwner, string entityType, string lastName, string firstName, string FEIN, string frequency)
    {
      NewEntityReturn returnEntity = new NewEntityReturn();
      returnEntity.Owner = entityOwner;
      returnEntity.NewID = newID;
      returnEntity.LegalName = lastName;
      returnEntity.EntityType = entityType;
      returnEntity.FEIN = FEIN;

      Entity owner = entityBase.Entity(entityOwner, useAttributes, effectiveDate, "", Handle);  // get the entity record for the entity owner
      if (owner == null || owner.ID == null)
      {
        returnEntity.ErrorMessage = "Entity Owner is not a valid entity";
        return returnEntity;
      }

      string ownerType = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, owner.getValue("EntityType"))).ToLower();
      if (entityType.Equals("Employee", StringComparison.CurrentCultureIgnoreCase) ||
        entityType.Equals("PayGroup", StringComparison.CurrentCultureIgnoreCase))
      {
      	if (ownerType != "client" && ownerType != "parent")    // is this owner not a client or a parent?
	      {
          entityOwner = entityBase.EntityOwner(entityOwner, "Client");  // then find the client that owns it
          if (entityOwner.ToLower() == "none")    // none was found
  	      {
    	      returnEntity.ErrorMessage = "EntityOwner is not a client and does not belong to a client";
      	    return returnEntity;
          }
        }
      }
      string strEntityID;
      string shortName;
      if (string.IsNullOrEmpty(entityType))
      {
        returnEntity.ErrorMessage = "EntityType is required";
      }
      else
      {
        if (entityType.Equals("PayGroup", StringComparison.CurrentCultureIgnoreCase))
        {
          strEntityID = entityOwner + "-" + frequency;
          returnEntity.LegalName = returnEntity.ShortName = returnEntity.FullName = "Paygroup " + frequency;
        }
        else
        {
		      if (FEIN == null || FEIN.Length != 9 || !((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsInteger, FEIN)))
		      {
		        returnEntity.ErrorMessage = "FEIN must exist and be 9 numeric characters in length";
		        return returnEntity;
		      }
		      if (entityBase.existsFEIN(FEIN))
    		  {
        		returnEntity.ErrorMessage = "That FEIN already exists in the entity table";
		        return returnEntity;
    		  }
          if (entityType.Equals("Employee", StringComparison.CurrentCultureIgnoreCase))
          {

            strEntityID = entityOwner + "-" + lastName; // construct an entity id
            shortName = string.Empty;
	          returnEntity.FullName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.makeFullName, strEntityID, "Employee", lastName, firstName, "", FEIN, "", "", out shortName);
  	        returnEntity.ShortName = shortName;
            returnEntity.LegalName = lastName;
    	    }
      	  else
          {
            // all other entity types
            strEntityID = newID;
            returnEntity.FullName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.makeFullName, strEntityID, "Employee", lastName, firstName, "", FEIN, "", "", out shortName);
            returnEntity.ShortName = shortName;
            returnEntity.LegalName = lastName;
          }
        }
        if (!string.IsNullOrEmpty(strEntityID) && entityBase.existsEntity(strEntityID))  // does it already exist?
	      {
  	      int sequence = 0;     // then append a sequence number
          while (entityBase.existsEntity(strEntityID + "-" + (++sequence).ToString())) ; // keep looking for one that does not exist
          strEntityID += "-" + sequence.ToString();
    	  }
        returnEntity.NewID = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripBadCharacters, strEntityID)).Replace(" ", "");  // take out bad characters and spaces from the id
      }
      return returnEntity;
    }
    public bool existsFEIN(string fein)
    {
      return entityBase.existsFEIN(fein);
    }


    public EntityAttributesCollection getCacheBranch(string itemTypes, string items)
    {
      return entityBase.getCacheBranch(itemTypes, items);
    }

    public ArrayList getListCacheEffectiveDates()
    {
      return entityBase.getListCacheEffectiveDates();
    }

    public void reloadCache()
    {
      entityBase.reloadCache();
      reloadDictionary();
    }
    public void reloadDictionary()
    {
      //SecurityFactory.GetInstance().RefreshDictionary();
    }
    //public void runAdminUtility(string utilityName)
    //{
    //  if (utilityName.Equals("refactorDictionaryFunctionData", StringComparison.CurrentCultureIgnoreCase))
    //    refactorDictionaryFunctionData();
    //  else if (utilityName.Equals("refreshTranscodeCategory", StringComparison.CurrentCultureIgnoreCase))
    //    entityBase.refreshTranscodeCategory();
    //}

    #endregion utility methods

    #region get methods

    /// <summary>
    /// Takes an Entity id and returns an Entity Object. Its behavior is influenced by the UseAttributes property.
    /// </summary>
    /// <param name="pEntity"></param>
    /// <returns>Returns an Entity object</returns>
    public Entity Entity(string pEntity, bool useAttributes)
    {
      Entity e = entityBase.Entity(pEntity, useAttributes, effectiveDate, "", Handle);
      if (e == null)
        return null;
      string entityType = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, e.Fields.getValue("EntityType"))).ToLower();
      if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.inList, entityTypesWithNoSecurity, entityType))
        return e;
      if (validEntity(pEntity))
        return e;
      else
        return null;
    }

    #region entityAttributes getAttributes calls

    /// <summary>
    /// Invocation of Attributes.getAttributes that also enriches the result set with Entity table Fields data
    /// </summary>
    /// <param name="entities">List of one or more Entities to select</param>
    /// <param name="itemTypes">List of zero or more ItemTypes to select</param>
    /// <param name="items">List of zero or more Items to select</param>
    /// <param name="parameters">TAGAttribute Parameters to optionally pass</param>
    /// <param name="efDate">Optional effective date for the Attributes call (default is Now)</param>
    /// <returns></returns>
    public EntityAttributesCollection getAttributes(string entities, string itemTypes,
      string items, string parameters, object efDate)
    {
      if (entities != null && entities != "")
      {
        EntityAttributesCollection e = entityBase.getAttributes(entities, itemTypes, items, parameters, efDate, true, Handle);
        e = checkSecurity(e);
        return e;
      }
      else
        return new EntityAttributesCollection();
    }
    private EntityAttributesCollection checkSecurity(EntityAttributesCollection e)
    {
      return e ;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="itemTypes"></param>
    /// <param name="items"></param>
    /// <param name="parameters"></param>
    /// <param name="efDate"></param>
    /// <param name="rawMode"></param>
    /// <returns></returns>
    public EntityAttributesCollection getAttributes(string entities, string itemTypes,
      string items, string parameters, object efDate, bool rawMode)
    {
      if (entities != null && entities != "")
      {
        EntityAttributesCollection e = entityBase.getAttributes(entities, itemTypes, items,
          parameters, efDate, true, rawMode, Handle);
        e = checkSecurity(e);
        return e;
      }
      else
        return new EntityAttributesCollection();
    }

    public EntityAttributesCollection getAttributes(string entities, string itemTypes,
      string items, string attributes, string attributeValues, string parameters, object efDate, bool includeVirtualItems)
    {
      if (entities != null && entities != "")
      {
        EntityAttributesCollection e;
        if (includeVirtualItems)
          e = entityBase.getAttributes_withVirtualItems(entities, itemTypes, items, parameters, efDate, true, Handle);
        else
          e = entityBase.getAttributes(entities, itemTypes, items, parameters, efDate, true, Handle);
        e = checkSecurity(e);

        return (EntityAttributesCollection)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.filterEACByAttributes, e, attributes, attributeValues);
      }
      else
        return new EntityAttributesCollection();
    }
    /// <summary>
    /// Invocation of Attributes.getAttributes that also enriches the result set with Entity table Fields data
    /// </summary>
    /// <param name="entities">List of one or more Entities to select</param>
    /// <param name="itemTypes">List of zero or more ItemTypes to select</param>
    /// <param name="items">List of zero or more Items to select</param>
    /// <param name="parameters">TAGAttribute Parameters to optionally pass</param>
    /// <param name="efDate">Optional effective date for the Attributes call (default is Now)</param>
    /// <returns></returns>
    public EntityAttributesCollection getAttributes_withVirtualAttributes(string entities, string itemTypes,
      string items, string parameters, object efDate)
    {
      if (entities != null && entities != "")
      {
        EntityAttributesCollection e = entityBase.getAttributes_withVirtualItems(entities, itemTypes, items, parameters, efDate, useAttributes, Handle);
        e = checkSecurity(e);
        return e;
      }
      else
        return new EntityAttributesCollection(); ;
    }

    /// <summary>
    /// method that will include BlockInheritedYN = true items. Note this does NOT enrich the entity info like the other items
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="itemTypes"></param>
    /// <param name="efDate"></param>
    /// <returns></returns>
    public EntityAttributesCollection getAttributes_withBlockedItems(string entities, string itemTypes, object efDate)
    {
      if (entities != null && entities != "")
      {
        EntityAttributesCollection e = entityBase.getAttributes_withBlockedItems(entities, itemTypes, efDate, Handle);
        e = checkSecurity(e);
        return e;
      }
      else
        return new EntityAttributesCollection();
    }

    #endregion entityAttributes getAttributes calls

    /// <summary>
    /// Returns the list of owners this entity might have
    /// </summary>
    /// <param name="pEntity"></param>
    /// <returns></returns>
    //public PickList EntityOwnerList(string pEntity)
    //{
    //  string[] validowners = { "Parent", "Client", "PayGroup" };
    //  return EntityOwnerList(pEntity, validowners);
    //}
    ///// <summary>
    ///// Overload that allows the calling program to specify what entityTypes the owners should be
    ///// </summary>
    ///// <param name="pEntity"></param>
    ///// <param name="validowners"></param>
    ///// <returns></returns>
    //private PickList EntityOwnerList(string pEntity, string[] validowners)
    //{
    //  EntityAttributesCollection entityHierarchy = EntityHierarchy(pEntity, false, true);  // first get the entity hiearchy, which chains the ownership
    //  // all the way up to 'all'
    //  int nbrOwners = 0;
    //  foreach (Entity e in entityHierarchy.Entities)
    //    if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.inList, validowners, (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, e.getValue("EntityType"))))
    //      nbrOwners++;
    //  PickList ownerList = new PickList(new string[] { "Code", "Description" }, nbrOwners);
    //  int i = 0;
    //  foreach (Entity e in entityHierarchy.Entities)
    //    if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.inList, validowners, (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, e.getValue("EntityType"))) && i < nbrOwners)
    //    {
    //      ownerList[i, 0] = e.OriginalID;
    //      ownerList[i, 1] = e.getValue("ShortName");
    //      i++;
    //    }
    //  return ownerList;
    //}

    /// <summary>
    /// This exposes the entity hierarchy to the calling client...
    /// </summary>
    /// <param name="pEntity"></param>
    /// <returns></returns>
    //private EntityAttributesCollection EntityHierarchy(string pEntity)
    //{
    //  if (pEntity != null && pEntity != "")
    //    return EntityHierarchy(pEntity, false);
    //  else
    //    return new EntityAttributesCollection();

    //}
    /// <summary>
    /// This exposes the entity hierarchy to the calling client...
    /// </summary>
    /// <param name="pEntity"></param>
    /// <returns></returns>
    //private EntityAttributesCollection EntityHierarchy(string pEntity, bool inRawMode, bool fromEntityList)
    //{
    //  if (pEntity != null && pEntity != "")
    //  {
    //    EntityAttributesCollection e = entityBase.EntityHierarchy(pEntity, inRawMode);
    //    if (!fromEntityList || !mySecurity.HasObjectAccess("EntityListCanViewAllEntityOwners"))
    //      e = checkSecurity(e);
    //    return e;
    //  }
    //  else
    //    return new EntityAttributesCollection();
    //}
    //public EntityAttributesCollection EntityHierarchy(string pEntity, bool inRawMode)
    //{
    //  if (pEntity != null && pEntity != "")
    //  {
    //    EntityAttributesCollection e = entityBase.EntityHierarchy(pEntity, inRawMode);
    //    e = checkSecurity(e);
    //    return e;
    //  }
    //  else
    //    return new EntityAttributesCollection();
    //}
    /// <summary>
    /// Finds and returns a set of Entities that match the where clause. Its behavior is influenced by the UseAttributes property.
    /// </summary>
    /// <param name="pWhereClause"></param>
    /// <returns>A EntityAttributesCollection set of Entities.</returns>
    public EntityAttributesCollection EntitiesWhere(string pWhereClause)
    {
      EntityAttributesCollection e = entityBase.EntitiesWhere(pWhereClause, useAttributes, effectiveDate, "", Handle);
      e = checkSecurity(e);
      return e;
    }
    ///// <summary>
    ///// Overloaded version of EntityChildren with 'Client' as the default EntityType
    ///// </summary>
    ///// <param name="pEntity"></param>
    ///// <returns></returns>
    //private EntityAttributesCollection EntityChildren(string pEntity)
    //{
    //  if (pEntity != null && pEntity != "")
    //  {
    //    EntityAttributesCollection e = entityBase.EntityChildren(pEntity, "Client", null, false, "", effectiveDate, false, false, -1, useAttributes);
    //    e = checkSecurity(e);
    //    return e;
    //  }
    //  else
    //    return null;
    //}
    ///// <summary>
    ///// Overloaded version of EntityChildren(pEntity, pEntityType, DoNotProcessEntityTypes) that does not specify 
    ///// a list of DoNotProcessEntityTypes.
    ///// </summary>
    ///// <param name="pEntity"></param>
    ///// <param name="pEntityType"></param>
    ///// <returns></returns>
    //private EntityAttributesCollection EntityChildren(string pEntity, string pEntityType)
    //{
    //  if (pEntity != null && pEntity != "")
    //  {
    //    EntityAttributesCollection e = entityBase.EntityChildren(pEntity, pEntityType, null, false, "", effectiveDate, false, false,-1, useAttributes);
    //    e = checkSecurity(e);
    //    return e;
    //  }
    //  else
    //    return null;
    //}
    ///// <summary>
    ///// Using pEntity as the top of the tree, build a list of all children down to where the bottom tier matches pEntityType. 
    ///// If no EntityType match is found, then all children will be found and included in the list.
    ///// 
    ///// This works by building the list from the immediate children of an Entity, and then calling itself recursively for each child
    ///// to add whatever children that child has.
    ///// 
    ///// DoNotProcessEntityTypes is used to process the list of EntityTypes that we want to exclude from the list. If the
    ///// ArrayList is null or empty, we exclude nothing.
    ///// </summary>
    ///// <param name="pEntity"></param>
    ///// <param name="pEntityType"></param>
    ///// <param name="DoNotProcessEntityTypes"></param>
    ///// <returns>a collection of Entities in EntityAttributesCollection form</returns>
    //private EntityAttributesCollection EntityChildren(string pEntity, string pEntityType,
    //  ArrayList DoNotProcessEntityTypes)
    //{
    //  if (pEntity != null && pEntity != "")
    //  {
    //    EntityAttributesCollection e = entityBase.EntityChildren(pEntity, pEntityType, 
    //      DoNotProcessEntityTypes, false, "", effectiveDate, false, false, -1, useAttributes);
    //    e = checkSecurity(e);
    //    return e;
    //  }
    //  else
    //    return null;
    //}
    ///// <summary>
    ///// Overload that allows the calling program to specify that only entities with EntityType
    ///// matching pEntityType are returned. This meets the "Entity Loop" requirement (Req # 360).
    ///// </summary>
    ///// <param name="pEntity"></param>
    ///// <param name="pEntityType"></param>
    ///// <param name="DoNotProcessEntityTypes"></param>
    ///// <param name="pIncludeOnlyBottomLevel"></param>
    ///// <returns></returns>
    //private EntityAttributesCollection EntityChildren(string pEntity, string pEntityType,
    //  ArrayList DoNotProcessEntityTypes, bool pIncludeOnlyBottomLevel)
    //{
    //  if (pEntity != null && pEntity != "")
    //  {
    //    EntityAttributesCollection e = entityBase.EntityChildren(pEntity, pEntityType,
    //      DoNotProcessEntityTypes, pIncludeOnlyBottomLevel, "", effectiveDate, false, false, -1, useAttributes);
    //    e = checkSecurity(e);
    //    return e;
    //  }
    //  else
    //    return null;
    //}

    ///// <summary>
    ///// Overload which allows the calling program to specify the ItemTypes needed for attribute calls
    ///// without a separate setting of the public property
    ///// </summary>
    ///// <param name="pEntity"></param>
    ///// <param name="pEntityType"></param>
    ///// <param name="DoNotProcessEntityTypes"></param>
    ///// <param name="pIncludeOnlyBottomLevel"></param>
    ///// <param name="pItemTypesRequested"></param>
    ///// <returns></returns>
    //private EntityAttributesCollection EntityChildren(string pEntity, string pEntityType,
    //  ArrayList DoNotProcessEntityTypes, bool pIncludeOnlyBottomLevel, string pItemTypesRequested)
    //{
    //  if (pEntity != null && pEntity != "")
    //  {
    //    EntityAttributesCollection e = entityBase.EntityChildren(pEntity, pEntityType,
    //      DoNotProcessEntityTypes, pIncludeOnlyBottomLevel, pItemTypesRequested, effectiveDate, false, false, -1, useAttributes);
    //    e = checkSecurity(e);
    //    return e;
    //  }
    //  else
    //    return null;
    //}

    public EntityAttributesCollection EntityChildren(string pEntity, string pEntityType,
              ArrayList DoNotProcessEntityTypes, bool pIncludeOnlyBottomLevel, string pItemTypesRequested,
              DateTime effectiveDate, bool includeParentsChildren, bool includeTerms, int maxlevels, bool pUseAttributes)
    {
      if (pEntity != null && pEntity != "")
      {
        EntityAttributesCollection e = entityBase.EntityChildren(pEntity, pEntityType, DoNotProcessEntityTypes,
          pIncludeOnlyBottomLevel, pItemTypesRequested, effectiveDate, includeParentsChildren, includeTerms, maxlevels, pUseAttributes, Handle);
        e = checkSecurity(e);
        return e;
      }
      else
        return new EntityAttributesCollection();
    }
    /// <summary>
    /// Overload which allows the calling program to specify the levels
    /// </summary>
    /// <param name="pEntity"></param>
    /// <param name="levels"></param>
    /// <returns></returns>
    private EntityHierarchy EntityChildren(string pEntity, int levels)
    {
      if (pEntity != null && pEntity != "")
        return entityBase.EntityChildren(pEntity, levels, null, effectiveDate);
      else
        return null;
    }
    /// <summary>
    /// replaces the old overload of EntityChildren() that returns an EntityHierarchy object
    /// </summary>
    /// <param name="pEntity"></param>
    /// <param name="levels"></param>
    /// <param name="DoNotProcessEntityTypes"></param>
    /// <returns></returns>
    public EntityHierarchy getEntityHierarchyChildren(string pEntity, int levels, ArrayList DoNotProcessEntityTypes, bool includeTerms)
    {
      return entityBase.EntityHierarchyChildren(pEntity, levels, DoNotProcessEntityTypes, DateTime.Today, includeTerms);
    }
    /// <summary>
    /// Deprecated. Please use getEntityHierarchyChildren()
    /// </summary>
    /// <param name="pEntity"></param>
    /// <param name="levels"></param>
    /// <param name="DoNotProcessEntityTypes"></param>
    /// <returns></returns>
    public EntityHierarchy EntityChildren(string pEntity, int levels, ArrayList DoNotProcessEntityTypes)
    {
      if (pEntity != null && pEntity != "")
        return entityBase.EntityChildren(pEntity, levels, DoNotProcessEntityTypes, effectiveDate);
      else
        return null;
    }

    /// <summary>
    /// returns a list of valid entitytypes from the entity table
    /// </summary>
    /// <returns>string array of Entity Types</returns>
    public string[] getEntityTypes()
    {
      return entityBase.EntityTypes();
    }

    /// <summary>
    /// Returns a dataset created with the parameters. This dataset is passed through CheckSecurity.
    /// Note that there are some rules on bow this works. Plese see CheckSecurity() comments for details.
    /// </summary>
    /// <param name="parameterNames">List of sproc parameters</param>
    /// <param name="parameterValues">List of values for each parameter. They MUST be in the same order as the names</param>
    /// <param name="parameterDateTypes">List of TAGBOSS (mostly C#) datatypes</param>
    /// <param name="parameterDirection">Optional list of direction (In/Out). Default is all In</param>
    /// <param name="spName">Name of Stored procedure</param>
    /// <returns>Security-filtered dataset. I could not flag this as read-only, but you MUST ONLY READ THIS dataset</returns>
    public DataSet executeQueryStoredProcedure(string[] parameterNames, object[] parameterValues,
      string[] parameterDateTypes, string[] parameterDirection, string spName)
    {
      DataSet dsData = entityBase.executeQueryStoredProcedure(parameterNames, parameterValues, parameterDateTypes, parameterDirection, spName);
      //dsData = checkSecurity(dsData);
      return dsData;
    }
    public PickList getIssueEntityList()
    {
      return entityBase.getIssueEntityList();
    }
    #endregion get methods

    #region save methods
    /// <summary>
    /// Pass in a EntityAttributesCollection set of Entity and TAGAttribute data. Any thing that has been changes should have the
    /// Dirty flag set, so that this routines knows what to update or add.
    /// </summary>
    /// <param name="doEntities"></param>
    public void Save(EntityAttributesCollection doEntities)
    {
      Save(doEntities, false);
    }
    public void Save(EntityAttributesCollection doEntities, bool isRawMode)
    {
      if (doEntities != null)
      {
        doEntities = checkSecurity(doEntities);
        foreach (Entity e in doEntities.Entities)
        {
          if (!e.ReadOnly && e.Dirty)
          {
            string entityType = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, e.Fields.getValue("EntityType"))).ToLower();
            if (entityType != "payroll")  // this is a virtual payroll entity, a gui artifact, so don't do anything
            {
              adjustStartEndDate(e);                    // adjust entity start or end dates
              if (!isRawMode)                           // if we are saving and not in raw mode
                adjustTableHeaderToSaveDirtyOnly(e); // make sure tableheaders only save dirty records
              entityBase.SaveEntityRecord(e, CurrentUser);
              if (isRawMode)
                entityBase.SaveAttributes(e, CurrentUser, true);
              else
                entityBase.SaveAttributes(e, CurrentUser, false);
            }
          }
        }
        //DataTriggerNotify(doEntities);
        // now reset the dirty flag so the consuming program can still use the collection. Note this is recursive, so it resets ALL dirty flags
        foreach (Entity e in doEntities.Entities)
        {
          if (e.Dirty)
            e.Dirty = false;
        }
      }
    }
    private void adjustTableHeaderToSaveDirtyOnly(Entity e)
    {
      foreach (ItemType it in e.ItemTypes)
        if (it.Dirty)
          foreach (Item i in it.Items)
            if (i.Dirty)
            {
              string itemSource = string.Format("{0}.{1}.{2}", e.ID, it.ID, i.ID);
              int seedIndex = 0;
              while (seedIndex >= 0)  // -1 means EOF
              {
                TAGAttribute a = i.foreachValueType(TAGFunctions.TABLEHEADER, ref seedIndex);
                if (a != null && a.Dirty && a.Value != null &&
                      a.Value.GetType() == typeof(TableHeader) && ((TableHeader)a.Value).Dirty)
                  a.Value = ((TableHeader)a.Value).getDirtyAndLocalRows(itemSource);
              }
            }
    }
    #endregion save methods

    #endregion public methods

    #region private methods
    /// <summary>
    /// Adjust the start and end date in the entity record based on the StatusEvent. Accepts the entity as a parameter, and returns
    /// the entity adjusted as required
    /// </summary>
    /// <param name="e">Entity object which we want to save</param>
    /// <returns>The adjusted Entity</returns>
    public Entity adjustStartEndDate(Entity e)
    {
      bool throwError = false;

      // So we go get the entity data from the db to fill the fields collection
      // since ItemMaintenance only returns fields that are on the screen, we need to read the db
      // to enrich with fields we don't have
      Entity eNew = entityBase.Entity(e.ID, false, effectiveDate, "", Handle);              // go get the record
      if (eNew != null)
      {
	      foreach (Field f in eNew.Fields)                    // and add the fields to the entity
  	      if (!e.Fields.Contains(f.ID))
    	      e.Fields.Add(f);
      }
      string entityType = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, e.getValue("EntityType"))).ToLower(); // 
      if (entityType == "payroll")      // payroll entities are virtual, don't do anything with them here
        return e;
      if (e.ItemTypes.Contains("Entity") && e.ItemTypes["Entity"].Items.Contains(entityType)) // look for the entitytype item in the attributes
      {
        /*
         * ok, we need to use the StatusEvent attribute to update the start and end dates on the entity record.
         * We only allow these dates to be changed indirectly from this attribute. They only exist on the entity record
         * for performance purposes. The StatusEvent changes the status of the entity, which in turn affects the 
         * start and end dates. 
         * 
         * We also use the StatusEventMasterList to decide how the status event should affect these dates
         * 
         * The following is an example of the contents of this attribute table. the rules are: find the first row that
         * matches the value of the EventStatus, then look in the UpdateEntityStartDate and UpdateEntityEndDate columns. 
         * These columns will have one of four values:
         *    StartDate       use the StartDate in the EventStatus valuehistory to update the column date
         *    EndDate         use the EndDate in the EventStatus valuehistory to update the column date
         *    Blank           make the column date null
         *    {no entry}      Do not change the column date
         *    
StatusFilter StatusEvent        Description                 Status     BenefitEligible UpdateEntityStartDate UpdateEntityEndDate 
New          Hired              Hired                       Active     True            StartDate                                 
Active       Termed-Misc        Terminated for Misc Reason  Terminated False                                 StartDate           
Leave        Termed-Misc        Terminated for Misc Reason  Terminated False                                 StartDate           
Active       Termed-Attendance  Termed for attendance       Active     False                                 StartDate           
Active       Termed-Insub       Termed for insubordination  Active     False                                 StartDate           
Leave        Returned           Returned from Leave         Active     True                                                      
Terminated   ReHired            Re-Hired                    Active     True            StartDate             Blank
         * 
         * If this collection does not have the Entity.EntityType item, then we assume there was not change to StatusEvent so
         * we don't do anything
         */
        Item entityTypeItem = e.ItemTypes["Entity"].Items[entityType];
        if (entityTypeItem.Attributes.Contains(Item.ATTRIBUTESTATUSEVENT))
        {
          bool hasMasterListError = false;
          TAGAttribute statusEventAttr = entityTypeItem.Attributes[Item.ATTRIBUTESTATUSEVENT];
          if (statusEventAttr.Dirty)  // was this changed?
          {
            string newStatus = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, statusEventAttr.Value)).ToLower(); // then get the status  
            if (entityTypeItem.Contains(Item.ATTRIBUTESTATUSEVENTMASTERLIST))       // and look it up in the master table
            {
              TableHeader masterListTable =
                  (TableHeader)TAGFunctions.evaluateFunction(
                  TAGFunctions.EnumFunctions.CTableHeader,
                  Item.ATTRIBUTESTATUSEVENTMASTERLIST,
                  entityTypeItem.Attributes.getValue("StatusEventMasterList"), dictionary);

              if (masterListTable == null || masterListTable.GetLength(0) <= 1 || masterListTable.GetLength(1) != 7)   // bad format?
                hasMasterListError = true;
              else
              {
                // now look for the correct row
                for (int i = 1; i < masterListTable.GetLength(0); i++)  // skip the first row, it contains the header
                {
                  if (newStatus == ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, masterListTable[i, 1])).ToLower())  // is this a match (second column)?
                  {
                    foreach (ValueHistory vh in statusEventAttr.Values)
                    {
                      if (vh.Dirty && ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, vh.Value)).ToLower() == newStatus) // is this the one that was changed?
                      {
                        // yes, we now have the valuehistory record that was changed. So, we update the entity start/end dates
                        // we have a match, so use the last to columns to decide how to modify the entity start/end dates
                        string[] whichDates = { "startdate", "enddate" };   // one for each column
                        string[] dateSource = new string[2];                // values for each column from the table
                        dateSource[0] = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, masterListTable[i, 5])).ToLower();
                        dateSource[1] = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, masterListTable[i, 6])).ToLower();
                        for (int j = 0; j < 2; j++)                         // for each column
                        {
                          switch (dateSource[j])    // what is the contents of the start/enddate column value in the table?
                          {
                            case "startdate":     // override this column (entity start or end date) with the value history start date
                              if (vh.StartDate == TAGFunctions.PastDateTime)    // we never serialize the past or future dates
                                e.Fields[whichDates[j]].Value = null;
                              else
                                e.Fields[whichDates[j]].Value = vh.StartDate;
                              break;
                            case "enddate":       // override this column (entity start or end date) with the value history end date
                              if (vh.EndDate == TAGFunctions.FutureDateTime)    // we never serialize the past or future dates
                                e.Fields[whichDates[j]].Value = null;
                              else
                                e.Fields[whichDates[j]].Value = vh.EndDate;
                              break;
                            case "blank":         // override this column (entity start or end date) with null
                              e.Fields[whichDates[j]].Value = null;
                              break;
                            default:
                              break;    // no value in the column, so do nothing
                          }
                        }
                        break;    // we found the vh record, so we do not look further
                      }
                    }
                    break;  // we had a match, so don't look further in the table
                  } // end if match section
                } // end lookup loop
              }
            }
            else
              hasMasterListError = true;
            if (hasMasterListError && throwError)
            {
              TAGExceptionMessage tm = new TAGExceptionMessage("EntityAttributes", "adjustStartEndDate", "StatusEventMasterList does not exist, is empty, or is poorly formatted");
              tm.AddParm(e.OriginalID);
              throw new Exception(tm.ToString());
            }
          }
        }
      }
      return e;     // whether we changed it or not, we return the entity
    }
    //private void DataTriggerNotify(EntityAttributesCollection doEntities)
    //{
    //  DirtyNotifications dns = new DirtyNotifications();
    //  foreach (Entity e in doEntities.Entities)
    //  {
    //    if (!(bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.inList, systemEntities, e.ID) && !e.ReadOnly && e.Dirty)
    //    {
    //      if (e.Fields.Dirty)
    //      {
    //        // We only process a trigger for entityfields if there is an entity item to use as scope attributes
    //        Item entityItem = null;
    //        string entityType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, e.Fields.getValue("EntityType"));
    //        if (entityType != string.Empty
    //          && e.ItemTypes.Contains("Entity")
    //          && e.ItemTypes["Entity"].Items.Contains(entityType)) // do we have the entityitem?
    //        {
    //          entityItem = e.ItemTypes["Entity"].Items[entityType]; // yes, then pick it up and process
    //          foreach (Field f in e.Fields)
    //          {
    //            if (!f.ReadOnly && f.Dirty)
    //            {
    //              DirtyNotification dn = new DirtyNotification();
    //              dn.ID = e.OriginalID + "." + f.OriginalID;  // id is Entity.FieldName
    //              dn.OldValue = f.OldValue;
    //              dn.NewValue = f.Value;
    //              dn.AttributesInScope = entityItem;
    //              dn.EffectiveDate = doEntities.EffectiveDate;
    //              if (f.ID == "entityid") // entityId is a special case
    //              {
    //                if (e.Deleted)
    //                  dn.TriggerType = "delete"; // field was deleted
    //                else
    //                  if (f.OldValue == null) // field is new since it did not have an old value 
    //                    dn.TriggerType = "add"; // so this is an add
    //              }
    //              else
    //                dn.TriggerType = "update";  // otherwise it is just an update
    //              dn.LastModifiedBy = mySecurity.User;
    //              dn.LastModifiedDateTime = DateTime.Now;
    //              dns.Add(dn);
    //            }
    //          }
    //        }
    //      }
    //      if (e.ItemTypes.Dirty)
    //      {
    //        foreach (ItemType it in e.ItemTypes)
    //        {
    //          if (!it.ReadOnly && it.Dirty)
    //          {
    //            foreach (Item i in it.Items)
    //            {
    //              /*
    //               *Item: (pre req # 535

    //              Add: Add a new ItemHistory record that has a startDate, 
    //                   or, Item does not exist, Item not Inherited & add item

    //              Update: Item inherited & change or Item not inherited & change, StartDate null -> not null

    //                  or: ANy changet to not most recent history is an update, any change to not-null is update

    //              Terminate: EndDate not null to end date null (in the most recent itemhistory record)

    //               */
    //              if (!i.ReadOnly && i.Dirty)
    //              {
    //                if (i.Deleted)  // I am not sure we really should support this but here it is for now
    //                {
    //                  DirtyNotification dn = new DirtyNotification();
    //                  dn.ID = e.OriginalID + "." + it.OriginalID + ".Item." + i.OriginalID + ".Deleted";  // id is Entity.ItemType.Item
    //                  dn.NewValue = i.OriginalID;
    //                  dn.TriggerType = "delete"; // Item was deleted
    //                  dn.LastModifiedBy = mySecurity.User;
    //                  dn.LastModifiedDateTime = DateTime.Now;
    //                  dn.EffectiveDate = doEntities.EffectiveDate;
    //                  dns.Add(dn);
    //                }
    //                else
    //                {
    //                  if (i.ItemHistoryRecords.Dirty)
    //                  {
    //                    string triggerType = "none";
    //                    i.ItemHistoryRecords.Sort();  // sort them in order of start date
    //                    int iNbrHistories = i.ItemHistoryRecords.Count;
    //                    int iHistorySub = 0;
    //                    object oldValue = null;
    //                    object newValue = null;
    //                    string itemDateName = string.Empty;
    //                    foreach (ItemHistory ih in i.ItemHistoryRecords)
    //                    {
    //                      if (ih.Dirty)
    //                      {
    //                        triggerType = "update";
    //                        if (ih.OldEndDate != ih.EndDate)
    //                          itemDateName = "EndDate";
    //                        else
    //                          if (ih.OldStartDate != ih.StartDate)
    //                            itemDateName = "StartDate";
    //                        if (ih.StartDate > TAGFunctions.PastDateTime
    //                          && (ih.OldStartDate != null && ih.OldStartDate != ih.StartDate)
    //                          && iHistorySub == 0) // start date was added and this is the first one
    //                        {
    //                          newValue = ih.StartDate;
    //                          triggerType = "add";
    //                        }
    //                        else
    //                        {
    //                          // ok this is the case where we have put in a new end date
    //                          if (ih.EndDate != TAGFunctions.FutureDateTime   // end date is "deleted"
    //                            && iHistorySub == iNbrHistories - 1           // and ths is the last history record
    //                            && ih.OldEndDate != null && ih.EndDate != ih.OldEndDate)  // and the value has changed
    //                          {
    //                            oldValue = ih.OldEndDate;
    //                            triggerType = "terminate";
    //                          }
    //                        }
    //                        break;  // only report the first one found
    //                      }
    //                      iHistorySub++;
    //                    }
    //                    if (triggerType != "none")
    //                    {
    //                      DirtyNotification dn = new DirtyNotification();
    //                      dn.ID = e.OriginalID + "." + it.OriginalID + ".Item." + i.OriginalID + "." + itemDateName;  // id is Entity.ItemType.Item
    //                      dn.NewValue = newValue;
    //                      dn.OldValue = oldValue;
    //                      dn.AttributesInScope = i;
    //                      dn.TriggerType = triggerType;
    //                      dn.LastModifiedBy = mySecurity.User;
    //                      dn.LastModifiedDateTime = DateTime.Now;
    //                      dn.EffectiveDate = doEntities.EffectiveDate;
    //                      dns.Add(dn);
    //                    }
    //                  }
    //                }
    //                foreach (TAGAttribute a in i.Attributes)
    //                {
    //                  /*
    //                   * Attributes:

    //                    We are adding a new property of the TAGAttriute class which has WouldInheritValue, which contains the value most recently overridden by the current value through inheritance or includes. This means we can know what value this attribute would have if it were deleted and then inherted from above.

    //                    Add: OldVaue is null and new Value is Not Null and WouldInheritValue is null

    //                    Update:  (OldValue not null or WouldInheritValue not null) and new Value is not null

    //                    or...      New Value is null and WouldInheritValue not null

    //                    Delete: OldValue is not null, WouldInheritValue is null, New Value is null

    //                   */
    //                  if (!a.ReadOnly && a.Dirty)
    //                  {
    //                    DirtyNotification dn = new DirtyNotification();
    //                    dn.ID = e.OriginalID + "." + it.OriginalID + "." + i.OriginalID + "." + a.OriginalID;
    //                    dn.OldValue = a.OldValue;
    //                    dn.NewValue = a.Value;
    //                    dn.TriggerType = "update";
    //                    dn.AttributesInScope = i;
    //                    if (
    //                        (
    //                          (a.OldValue == null || (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, a.OldValue) == string.Empty) &&
    //                          (a.WouldInheritValue == null || (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, a.WouldInheritValue) == string.Empty)
    //                        )
    //                      && !a.IsNull)
    //                      dn.TriggerType = "add";
    //                    else
    //                      if (a.IsNull && (a.OldValue != null && (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, a.OldValue) != string.Empty) &&
    //                         (a.WouldInheritValue == null || (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, a.WouldInheritValue) == string.Empty)
    //                        )
    //                        dn.TriggerType = "terminate";
    //                    dn.LastModifiedBy = mySecurity.User;
    //                    dn.LastModifiedDateTime = DateTime.Now;
    //                    dn.EffectiveDate = doEntities.EffectiveDate;
    //                    dns.Add(dn);
    //                  }
    //                }
    //              }
    //            }
    //          }
    //        }
    //      }
    //    }
    //  }
    //  if (dns.Count > 0)  // we did have a notification
    //    workflow.ReportDirtyNotifications(dns, mySecurityHandle); // myWorkflow.
    //}
    //private void refactorDictionaryFunctionData()
    //{
    //  ItemType itFunction = dictionary.DictionaryProperties("Function");
    //  foreach (Item iFunction in itFunction.Items)
    //  {
    //    iFunction.MarkForDelete = false;
    //    iFunction.Attributes.Remove("RequiresAttributeListYN"); // first we remove this deprecated attribute
    //    TAGAttribute a = iFunction.Attributes["Parameters"];
    //    string strParms = null;
    //    foreach (ValueHistory vh in a.Values)
    //      if (vh.Value.GetType() == typeof(string))
    //        strParms = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, vh.Value);
    //    if (strParms == null)
    //      strParms = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, a.Value);
    //    if (!isOldFunctionParameters(strParms))
    //    {
    //      TAGExceptionMessage tm = new TAGExceptionMessage("entityAttributes", "refactorDictionaryFunctionData",
    //        "Functions have already been refactored");
    //      throw new Exception(tm.ToString());
    //    }
    //    string[] dataTypes = new string[] { "string", "string" };
    //    string[] columnNames = new string[] { "Parameter", "DataType" };
    //    PickList oldParms = new PickList(strParms, dataTypes, columnNames);
    //    TableHeader newParms = new TableHeader();
    //    TableHeaderColumn col1 = new TableHeaderColumn("Sequence");
    //    TableHeaderColumn col2 = new TableHeaderColumn("Parameter");
    //    TableHeaderColumn col3 = new TableHeaderColumn("DataType");
    //    newParms.Columns.Add(col1.OriginalID, col1);
    //    newParms.Columns.Add(col2.OriginalID, col2);
    //    newParms.Columns.Add(col3.OriginalID, col3);
    //    newParms.KeyNames = new string[] { "Sequence" };
    //    int seq = 0;
    //    string datatype = "string";
    //    for (int iRow = 0; iRow < oldParms.Count; iRow++)
    //    {
    //      object[] values = new object[3];
    //      values[0] = seq;
    //      values[1] = oldParms[iRow, 0];
    //      values[2] = oldParms[iRow, 1];
    //      if (((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, values[1])).ToLower() == "returntype")
    //        datatype = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, values[2]);
    //      else
    //      {
    //        TableHeaderRow newRow = new TableHeaderRow(newParms.Columns, values);
    //        newParms.AddRow(newRow);
    //        seq++;
    //      }
    //    }
    //    iFunction.Remove(a.ID);
    //    TAGAttribute newAttr = new TAGAttribute();
    //    newAttr.ID = a.OriginalID;
    //    newAttr.ValueType = TAGFunctions.TABLEHEADER;
    //    ValueHistory vhNew = new ValueHistory();
    //    vhNew.ValueType = TAGFunctions.TABLEHEADER;
    //    vhNew.Value = newParms;
    //    newAttr.Values.Add(vhNew);
    //    iFunction.Add(newAttr);
    //    iFunction.AddAttribute("DataType", datatype);
    //  }
    //  ItemType itAttributes = dictionary.DictionaryProperties("Attribute");
    //  Item iParameters = itAttributes["Parameters"];
    //  TAGAttribute aTableHeader = iParameters.Attributes["TableHeader"];
    //  aTableHeader.OverwriteValue = true;
    //  aTableHeader.Value = "(Sequence~Parameter~DataType)";
    //  EntityAttributesCollection eac = new EntityAttributesCollection();
    //  Entity e = new Entity();
    //  e.ID = "Dictionary";
    //  e.ItemTypes.Add(itFunction);
    //  ItemType newItAttributes = new ItemType();
    //  newItAttributes.ID = itAttributes.OriginalID;
    //  newItAttributes.Items.Add(iParameters);
    //  e.ItemTypes.Add(newItAttributes);
    //  eac.Entities.Add(e);
    //  Save(eac, true);
    //}
    //private bool isOldFunctionParameters(string parms)
    //{
    //  if (parms == null || parms == string.Empty)
    //    return false;
    //  string[] parmRows = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, parms), new string[] { "|" });
    //  if (parmRows == null || parmRows.GetLength(0) == 0)
    //    return false;
    //  string[] parmCols = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, parmRows[0], new string[] { "~" });
    //  if (parmCols == null || parmCols.GetLength(0) == 0 || parmCols.GetLength(0) == 2)
    //    return true;
    //  return false;
    //}
    private bool validEntity(string pEntity)
    {
      return true;
      //if (!useSecurity)         // always pass if we are not checking security
      //  return true;
      //bool entityOk = false;                  // assume failure until we find one
      //string checkEntity = pEntity.ToLower(); // id to check against
      //if (!mySecurity.SecureEmployees)        // if employees are not included in the entity list
      //{
      //  Entity thisEntity = entityBase.Entity(checkEntity, useAttributes, effectiveDate, "", Handle);  // read the entity record
      //  if (thisEntity.Fields["EntityType"].Value.ToString().Equals(entityBase.EntityTypeEmployee, StringComparison.CurrentCultureIgnoreCase))  // and check if this is an employee
      //    checkEntity = entityBase.EntityOwner(checkEntity, "Client");  // if so, then we want to check the access to the parent Client
      //}
      //if (entityList.ContainsKey(checkEntity)) // if the entity we want to check is in the Entity List
      //  entityOk = true;                      // then we pass
      //return entityOk;
    }
    //private SearchResultCollection checkSecurity(SearchResultCollection searchResults)
    //{
    //  if (useSecurity)
    //  {
    //    if (mySecurity == null)
    //      throw new Exception("Use Security is true but no security object exists");
    //    else
    //    {
    //      if (mySecurity.IsProgram)   // programs have no security filter
    //        return searchResults;
    //      else
    //      {
    //        if (!mySecurity.IsLoggedIn)
    //          throw new Exception("User is not logged in");
    //      }
    //    }
    //  }
    //  else
    //    return searchResults;
    //  Dictionary<string, int> deletedEntities = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
    //  foreach (SearchResult entity in searchResults)
    //  {
    //    string myEntity = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, entity.ID);
    //    if (myEntity != string.Empty)
    //    {
    //      string myEntityType = entityBase.EntityType(myEntity);
    //      bool readOnly = false;
    //      bool hasAccess = entityPassesSecurity(myEntity, myEntityType, out readOnly);
    //      if (!hasAccess && !deletedEntities.ContainsKey(myEntity))
    //        deletedEntities.Add(myEntity, 0);
    //    }
    //  }
    //  foreach (string entityID in deletedEntities.Keys)
    //    searchResults.Remove(entityID);
    //  return searchResults;
    //}
    /// <summary>
    /// This routine will filter a dataset through security, and delete any rows
    /// that have an entity that the user does not have access to.
    /// IMPORTANT NOTE: Do not call this routine if you are going to try and save
    /// the DataSet. If you do, any rows that do not pass security will be
    /// deleted from the database!
    /// Also note: this routine is looking for column names, left to right, 
    /// that match the list of know entity columns. The list is in this routine,
    /// but at the time of this comment is { entity, client, employee }.
    /// The FIRST column it finds will be the one it uses to filter. it will ignore
    /// any subsequent columns
    /// </summary>
    /// <param name="dsEntities">A DataSet to filter on entity security</param>
    /// <returns></returns>
    //private DataSet checkSecurity(DataSet dsEntities)
    //{
    //  /*
    //   * First, we check for security bypass and errors...
    //   */
    //  if (useSecurity)
    //  {
    //    if (mySecurity == null)
    //      throw new Exception("Use Security is true but no security object exists");
    //    else
    //    {
    //      if (mySecurity.IsProgram)   // programs have no security filter
    //        return dsEntities;        // then return the collection without change
    //      else
    //      {
    //        if (!mySecurity.IsLoggedIn)
    //          throw new Exception("User is not logged in");
    //      }
    //    }
    //  }
    //  else // if we are not using security
    //    return dsEntities;  // then return the collection without change
    //  // Now we check to see if this dataset has an entity columns
    //  //string[] entityColumnNames = { "entity", "client", "employee" };
    //  string[] entityColumnNames = { "entity", "evententity", "sb", "parent", "client", "paygroup", "employee" };
    //  if (dsEntities == null)
    //    return dsEntities;
    //  foreach (DataTable table in dsEntities.Tables)
    //  {
    //    bool hasEntityColumn = false;
    //    string myEntityColumn = string.Empty;
    //    foreach (string columnName in entityColumnNames)
    //      if (table.Columns.Contains(columnName))
    //      {
    //        myEntityColumn = columnName;
    //        hasEntityColumn = true;
    //        break;
    //      }
    //    if (hasEntityColumn)
    //    {
    //      foreach (DataRow row in table.Rows)
    //      {
    //        string myEntity = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row[myEntityColumn]);
    //        if (myEntity != string.Empty)
    //        {
    //          string myEntityType = entityBase.EntityType(myEntity);
    //          bool readOnly = false;
    //          bool hasAccess = entityPassesSecurity(myEntity, myEntityType, out readOnly);
    //          if (!hasAccess)
    //            row.Delete();
    //        }
    //      }
    //    }
    //  }

    //  return dsEntities;
    //}
    //private EntityAttributesCollection checkSecurity(EntityAttributesCollection doEntities)
    //{
    //  /*
    //   * First, we check for security bypass and errors...
    //   */
    //  if (useSecurity)
    //  {
    //    if (mySecurity == null)
    //      throw new Exception("Use Security is true but no security object exists");
    //    else
    //    {
    //      if (mySecurity.IsProgram)   // programs have no security filter
    //        return doEntities;        // then return the collection without change
    //      else
    //      {
    //        if (!mySecurity.IsLoggedIn)
    //          throw new Exception("User is not logged in");
    //      }
    //    }
    //  }
    //  else // if we are not using security
    //    return doEntities;  // then return the collection without change
    //  /* 
    //   * Now it checks security entity list against the data object.
    //   * 
    //   * If the access is read-only, then set the flag.
    //   * 
    //   * If no access for this entity, then delete it from the collection
    //   */

    //  //Defect reported by Marel, we must first check if we have a NOT null entities object
    //  if (doEntities == null)
    //    return null;

    //  for (int k1 = doEntities.Entities.Count - 1; k1 >= 0; k1--)  // otherwise check each entity in the collection
    //  {

    //    Entity e = doEntities.Entities[k1];

    //    if (e.IsNew)                                  // if it is new
    //      mySecurity.EntityList.Add(e.ID.ToString(), e.ReadOnly);      // add it to the valid list of entities, don't check security on a new entity
    //    else
    //    {
    //      if (e.Fields.Count == 0)  // This collection does not have any entity fields
    //      {
    //        // So we go get the entity data from the db to fill the fields collection
    //        //bool saveUseAttributes = entityBase.UseAttributes;  // save the state of UseAttributes 
    //        //entityBase.UseAttributes = false;                   // and turn it off so all we get is the entity record with its fields
    //        //Entity eNew = entityBase.Entity(e.ID);              // go get the record
    //        //entityBase.UseAttributes = saveUseAttributes;       // restore the UseAttributes state
    //        Entity eNew = entityBase.Entity(e.ID, false, effectiveDate, "", Handle);              // go get the record
    //        foreach (Field f in eNew.Fields)                    // and add the fields to the entity
    //          e.Fields.Add(f);
    //      }
    //      string entityType = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, e.getValue("EntityType"))).ToLower(); // 
    //      if (string.IsNullOrEmpty(entityType)) // ok, we had fields, but entitytype was not one of them
    //      {
    //        // so we go get it cause we need it
    //        Entity eNew = entityBase.Entity(e.ID, false, effectiveDate, "", Handle);              // go get the record
    //        entityType = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, eNew.getValue("EntityType"))).ToLower();
    //      }
    //      bool readOnly = false;
    //      if (entityPassesSecurity(e.ID, entityType, out readOnly))
    //        e.ReadOnly = readOnly;
    //      else
    //      {
    //        if (e.ID.Equals("default") && mySecurity.HasObjectAccess("CanViewDefaultEntity"))
    //          e.ReadOnly = true;
    //        else
    //        {
    //          bool saveMarkForDelete = doEntities.Entities.MarkForDelete;               // save the "mark for delete" status
    //          doEntities.Entities.MarkForDelete = false;    // dont mark this for delete, but really delete it
    //          doEntities.Entities.Remove(e.ID);             // now delete the entry
    //          doEntities.Entities.MarkForDelete = saveMarkForDelete;  // and then restore the status
    //        }
    //      }
    //    }
    //  }
    //  return doEntities;
    //}
    private bool entityPassesSecurity(string entityID, string entityType, out bool readOnly)
    {
      readOnly = false;
      return true;
      //entityType = entityType.ToLower();

      //bool passesSecurity = true;
      //readOnly = false;
      ///*
      // * First, we check for security bypass and errors...
      // */
      //if (useSecurity)
      //{
      //  if (mySecurity == null)
      //    throw new Exception("Use Security is true but no security object exists");
      //  else
      //  {
      //    if (mySecurity.IsProgram)   // programs have no security filter
      //      return passesSecurity;        // then return the collection without change
      //    else
      //    {
      //      if (!mySecurity.IsLoggedIn)
      //        throw new Exception("User is not logged in");
      //    }
      //  }
      //}
      //else // if we are not using security
      //  return passesSecurity;  // then return the collection without change
      ///* 
      // * Now it checks security entity list against the data object.
      // * 
      // * If the access is read-only, then set the flag.
      // * 
      // * If no access for this entity, then delete it from the collection
      // */
      //if (!(bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.inList, entityTypesWithNoSecurity, entityType))
      //{
      //  if (entityType != "payroll")                // payroll is a special virtual entity type we don't check security for
      //  {
      //    string entityToCheck = entityID;              // normally, the entity we want to check for is this one
      //    if (entityToCheck.ToLower() == mySecurity.User.ToLower()) // if the entity i got is me, then i always have access
      //      passesSecurity = true;
      //    else
      //    {
      //      // but first, check to see if this is an employee, which may require special handling
      //      // if employees are not in the list, and if this is an employee
      //      if (entityType == "employee" && !mySecurity.SecureEmployees)
      //        entityToCheck = entityBase.EntityOwner(entityID, "Client"); // then we check for the client they belong to instead
      //      if (entityType == "employee"
      //          && mySecurity.HasEntity(entityID)
      //          && !mySecurity.HasEntityAccess(entityID)) // now, we see if there is a special case "Deny" for this employee
      //        passesSecurity = false;
      //      else
      //        if (mySecurity.EntityList.ContainsKey(entityToCheck)) // if the entity we check for is in the list
      //          readOnly = mySecurity.EntityList[entityToCheck]; // then pick up the readonly flag from it
      //        else
      //          passesSecurity = false;
      //    }
      //  }
      //}
      //return passesSecurity;
    }
    //private EntityAttributesCollection filterEACByAttributes(EntityAttributesCollection eac, string attributes, string attributeValues)
    //{
    //  if (attributes == null || attributes == string.Empty)
    //    return eac;
    //  string[] arrAttributes = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.FromList, attributes.ToLower());
    //  if (arrAttributes == null || arrAttributes.GetLength(0) == 0)
    //    return eac;
    //  bool checkValues = true;
    //  string[] arrValues = null;
    //  if (attributeValues == null || attributeValues == string.Empty)
    //    checkValues = false;
    //  else
    //  {
    //    arrValues = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.FromList, attributeValues);
    //    if (arrValues == null || arrValues.GetLength(0) == 0 || arrValues.GetLength(0) != arrAttributes.GetLength(0))
    //      checkValues = false;
    //  }
    //  foreach (Entity e in eac.Entities)
    //    for (int k = e.ItemTypes.Count - 1; k >= 0; k--)
    //    {
    //      ItemType it = e.ItemTypes[k];
    //      if (checkValues)
    //      {
    //        bool oldMarkForDelete = it.Items.MarkForDelete;
    //        it.Items.MarkForDelete = false ;
    //        for (int k0 = it.Items.Count - 1; k0 >= 0; k0--)  // top to bottom since we may remove some
    //        {
    //          Item i = it.Items[k0];
    //          bool removeItem = false;
    //          for (int k1 = 0; k1 < arrAttributes.GetLength(0); k1++)
    //          {
    //            string aID = arrAttributes[k1];
    //            if (i.Attributes.Contains(aID))
    //            {
    //              TAGAttribute a = i.Attributes[aID];
    //              if (!((int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CompareTo, a.Value, a.DataType, arrValues[k1], a.DataType) == 0))  // are the values equal?
    //              {
    //                removeItem = true;
    //                break;
    //              }
    //            }
    //            else
    //            {
    //              removeItem = true;
    //              break;
    //            }
    //          }
    //          if (removeItem)
    //            it.Items.Remove(i.ID);
    //        }
    //        it.MarkForDelete = oldMarkForDelete;
    //      }
    //      else
    //      {
    //        ItemType newIT = new ItemType();
    //        foreach (Item oldItem in it.Items)
    //        {
    //          Item newItem = new Item();
    //          newItem.ID = oldItem.OriginalID;
    //          newItem.ItemHistoryRecords = oldItem.ItemHistoryRecords;
    //          newItem.Source = oldItem.Source;
    //          newItem.Description = oldItem.Description;
    //          newItem.Source = oldItem.Source;
    //          newItem.IsInherited = oldItem.IsInherited;
    //          newItem.LastModifiedBy = oldItem.LastModifiedBy;
    //          newItem.LastModifiedDateTime = oldItem.LastModifiedDateTime;
    //          foreach (string aID in arrAttributes)
    //          {
    //            if (oldItem.Attributes.Contains(aID))
    //              newItem.Attributes.Add(oldItem.Attributes[aID]);
    //          }
    //          if (newItem.Attributes.Count > 0)
    //            newIT.Items.Add(newItem);
    //        }
    //        string oldITID = it.OriginalID;
    //        e.ItemTypes.Remove(it.ID);
    //        if (newIT.Items.Count > 0)
    //        {
    //          newIT.ID = oldITID;
    //          e.ItemTypes.Add(newIT);
    //        }
    //      }
    //    }
    //  return eac;
    //}

    #endregion private methods
  }
}
