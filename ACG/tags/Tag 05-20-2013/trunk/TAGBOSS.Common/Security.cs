using System;
using System.Collections.Generic;
using System.Collections;

using TAGBOSS.Common.Logging;
using TAGBOSS.Common.Model;

namespace TAGBOSS.Common
{
  [Serializable]
  public class Security
  {

    #region constants

    private const string cGRANT = "Grant";
    private const string cDENY = "Deny";
    private const string cNONE = "None";          // special right that is worse than any right, including Deny
    private const string cREADONLY = "ReadOnly";
    #endregion constants
    public static string GRANT { get { return cGRANT; } }
    public static string DENY { get { return cDENY; } }
    public static string NONE { get { return cNONE; } }
    public static string READONLY { get { return cREADONLY; } }
    private const string EM_SYSTEMOBJECT_LOOKUP_FAILED = "Lookup of SystemObject {0} failed"; //this string is formated depending the system object failed
    private const string EM_ENTITY_LOOKUP_FAILED = "Lookup of Entity {0} failed";


    #region module data
    private ArrayList dontIncludeEntityTypes = new ArrayList() { "Vendor", "Bank", "Prospect", "Contact" };
    bool rightsok = true;
    private DateTime createDateTime = DateTime.Now;
    private DateTime lastAccessedDateTime = DateTime.Now;
    private ArrayList accessRightNames;               // populated by LoadPrecedence
    /*
     * Under normal conditions, a list of Entities that the user has access to is very long if individual Employees are listed. 
     * for this reason, we by default exclude them from the list, assuming that Entity security is at the client level. However, there
     * are some conditions (e.g. individual employee signon or individual client access) where the list we maintain must be
     * at this level. 
     * 
     * For this reason, we maintain the following flag. If false, Employees are excluded from the list. If true, they are included
     */
    private bool secureEmployees = false;                       // do we generate Entity Security lists at the level of Employees?
    ArrayList myParents;                                // list of groups of which I am a member

    /*
     * The following are identifiers that tell if someone is logged in and if so, who.
     */
    private int handle = 0;                                 // this is the unique handle, assigned by the factory to this instance

    private bool isLoggedIn = false;                      // Currently in a logged in status?
    private bool isProgram = false;                     // is this user a batch program?
    private bool isV3User = false;                      // is this a v3 user?
    private string lastUserLogin = string.Empty;        // user@domain that was most recently logged in
    private string userLogin = string.Empty;            // user@domain that is currently logged in
    private string programID = string.Empty;            // if a workflow action (batch program) is logged in, which is it?
    private string userEntity = null;                   // this is the Entity id for the logged in user.
    private string domain = string.Empty;
    private string errorMessage;                        // contains the most recent error message
    /*
 * The following, objectRightsList, is the primary "database" of SystemObject/Right pairs that the Security class uses to lookup a 
 * securityobject and retrieve the accessright associated with it for the logged in user.
 * 
 */
    private Dictionary<string, string> objectRightsList = new Dictionary<string,string>();      // hash lookup of rights by object
    private Dictionary<string, string> entityRightsList = new Dictionary<string,string>();      // hash lookup of rightys by entity
    private Dictionary<string, string> objectTypes = new Dictionary<string, string>();

    private Dictionary<string, bool> entityList = new Dictionary<string, bool>(StringComparer.CurrentCultureIgnoreCase);  // actual list of Entities, since HashTable does not actually store the string key
    private ArrayList objectList = new ArrayList();                             // actual list of objects, since HashTable does not actually store the string key

    #endregion

    #region public properties


    public ArrayList AccessRightNames
    {
      get { return accessRightNames; }
      set { accessRightNames = value; }
    }
    /// <summary>
    /// SecurityHandle of this instance
    /// </summary>
    public int Handle
    {
      get { return handle; }
      set { handle = value; }
    }
    public Dictionary<string, string> ObjectRightsList
    {
      get { return objectRightsList; }
      set { objectRightsList = value; }
    }

    public ArrayList ObjectList
    {
      get { return objectList; }
      set { objectList = value; }
    }

    public Dictionary<string, string> EntityRightsList
    {
      get { return entityRightsList; }
      set { entityRightsList = value; }
    }

    public bool RightsOK
    {
      get { return rightsok; }
      set { rightsok = value; }
    }
    /// <summary>
    /// Returns the constant that Security uses to signify denied access
    /// </summary>
    public string Deny { get { return cDENY; } }
    /// <summary>
    /// Returns the constant that Security uses to signify readonly access
    /// </summary>
    public string ReadOnly { get { return cREADONLY; } }
    /// <summary>
    /// Returns the Entity id of the currently logged in user
    /// </summary>
    public string User
    {
      get
      {
        //if (handle == 0)
        //  return "System";
        if (userEntity == null)
          return "Unknown";
        else
        {
          string returnUser = userEntity;
          if (!isLoggedIn)
            returnUser += "*";  // we place an asterisk at the end if the user is not logged in
          return returnUser;
        }
      }
      set { userEntity = value; }
    }
    /// <summary>
    /// last login in the format loginid@domainname
    /// </summary>
    public string UserLogin
    {
      get { return userLogin; }
      set { userLogin = value; }
    }

    public string Domain
    {
      get { return domain; }
      set { domain = value; }
    }

    /// <summary>
    /// If a workflow action (batch program) is logged in, which is it?
    /// </summary>
    public string ProgramID
    {
      get { return programID; }
      set { programID = value; }
    }
    /// <summary>
    /// Returns the count of the ACL (Object Rights List) entries
    /// </summary>
    public int Count
    {
      get { return objectRightsList.Count; }
    }
    /// <summary>
    /// Do we include Employees in the Entity Rights List. Normally not, because the list is long, unless we are interested in a specific 
    /// empoloyee
    /// </summary>
    public bool SecureEmployees
    {
      get { return secureEmployees; }
      set { secureEmployees = value; }
    }
    /// <summary>
    /// ArrayList list of EntityTypes that we do not want included in the rights list.
    /// </summary>
    public ArrayList DontIncludeEntityTypes         // allow the calling program to override this list of EntityTypes not to include in the Entities Rights List
    {
      get { return dontIncludeEntityTypes; }
      set { dontIncludeEntityTypes = value; }
    }
    /// <summary>
    /// If certain errors occur, there will be an error message here
    /// </summary>
    public string Error
    {
      get
      {
        /*
         * Returns a string if an error has occurred, and empty string if not. If there was an error, it 
         * returns the error message for that error.
         */
        return errorMessage;
      }
      set { errorMessage = value; }
    }
    /// <summary>
    /// DateTime this Security Object was instantiated
    /// </summary>
    public DateTime CreateDateTime
    {
      get { return createDateTime; }
    }
    /// <summary>
    /// Last DateTime this security object was accessed in any way
    /// </summary>
    public DateTime LastAccessedDateTime
    {
      get { return lastAccessedDateTime; }
      set { lastAccessedDateTime = value; }
    }
    /// <summary>
    /// Is the user in a logged in status?
    /// </summary>
    public bool IsLoggedIn
    {
      get { return isLoggedIn; }
      set { isLoggedIn = value; }
    }
    /// <summary>
    /// Is this a program instead of a normal user?
    /// </summary>
    public bool IsProgram
    {
      get { return isProgram; }
      set { isProgram = value; }
    }
    /// <summary>
    /// List of groups of which the user is a member
    /// </summary>
    public ArrayList ParentGroups
    {
      get { return myParents; }
      set { myParents = value; }
    }

    public bool IsV3User { get { return isV3User; } set { isV3User = value; } }

    /// <summary>
    /// Returns a collection of Entities for which rights are defined for this user
    /// </summary>
    public Dictionary<string, bool> EntityList
    {
      /*
       * returns a list of entities for which rights are defined for this user
       */
      get
      {
        lastAccessedDateTime = DateTime.Now;
        return entityList;
      }
      set { entityList = value; }
    }

    public Dictionary<string, string> ObjectTypes
    {
      get { return objectTypes; }
      set { objectTypes = value; }
    }
    #endregion

    #region public methods

/// <summary>
    /// Creates a shallow copy of the current security object
    /// </summary>
    /// <returns></returns>
    public Security Clone()
    {
      // clone non static members
      return (Security)this.MemberwiseClone();
    }

    /// <summary>
    /// De-authenticates, and frees up the ACL/Security Matrix
    /// </summary>
    public void Logout()
    {
      lastAccessedDateTime = DateTime.Now;
      isLoggedIn = false;           // log out the user
    }

    public bool AddRight(string SecurityObject, string Right)
    {
      try
      {
        string compareKey = SecurityObject.ToLower();
        if (objectRightsList.ContainsKey(compareKey))
        {
          objectRightsList[compareKey] = Right;
        }
        else
        {
          objectRightsList.Add(compareKey, Right);   // add this if it does not already exist
          objectList.Add(compareKey);
        }
      }
      catch (ArgumentNullException ane)
      {
        errorMessage = "Add of object right failed";
        return false;
      }
      catch (NotSupportedException nse)
      {
        errorMessage = "Add of object right failed";
        return false;
      }

      return true;
    }
    public bool UpdateRight(string SecurityObject, string Right)
    {
      try
      {
        objectRightsList[SecurityObject] = Right;
        return true;
      }
      catch (NullReferenceException nre)
      {
        errorMessage = "Rights lookup failed";
        return false;
      }
    }
    public bool UpdateEntity(Entity myEntity, string Right)
    {
      /*
       * Update an Entity Right
       */
      try
      {
        entityRightsList[myEntity.ID] = Right;
        return true;
      }
      catch (NullReferenceException nre)
      {
        errorMessage = "Entitys lookup failed";
        return false;
      }
    }
    public int AccessRight(string strRight)
    {
      /*
       * Returns the subscript for the Right in AccessRightsNames
       */
      int r;
      try
      {
        for (r = 0; r < AccessRightNames.Count; r++)
        {
          if (AccessRightNames[r].ToString() == strRight)
          {
            return r;
          }
        }
        return AccessRightNames.Count - 1;       // if the string is invalid, this returns the lowest access right (highest subscript)
      }
      catch (Exception e)
      {
        try
        {
          errorMessage = "Right was not found in rights heirarchy";
          return AccessRightNames.Count - 1;       // if the string is invalid, this returns the lowest access right (highest subscript)
        }
        catch (Exception ee)
        {
          errorMessage = "Error in lookup in AccessRightsNames";
          return 0;
        }
      }
    }
    /// <summary>
    /// Returns a where clause (without the "WHERE" keyword) of entities that this user has rights to. This overload returns entities
    /// where the right is better than cDENY
    /// </summary>
    /// <returns>A where clause</returns>
    public string EntityWhereClause()
    {
      lastAccessedDateTime = DateTime.Now;
      return EntityWhereClause(cDENY);
    }
    /// <summary>
    /// Returns a where clause (without the "WHERE" keyword) of entities that this user has rights to. This overload returns entities
    /// where the right is better than then right passed in via BetherThanRight.
    /// </summary>
    /// <param name="BetterThanRight"></param>
    /// <returns>A where clause</returns>
    public string EntityWhereClause(string BetterThanRight)
    {
      /*
       * Returns an Entity Where Clause appropriate for the this user, which would allow a 
       * the calling program to filter any data stream by entity
       * 
       * Note that whereclause does NOT include the WHERE keyword, in case the calling program wants to construct a compound clause
       * 
      */
      //TODO: ./fix bug there are no commas.
      string whereclause = "";
      if (isProgram)            // batch programs have no restrictions
        return whereclause;     // so where clause is blank (no filter on entity)
      int i;
      //try
      //{
      List<string> entityIDs = new List<string>(entityList.Keys);
      for (i = 0; i < entityIDs.Count; i++)
      {
        string myEntityID =   entityIDs[i];                            // get the next Entity object in the list
        if (IsBetterThan(EntityRight(entityIDs[i].ToString()), BetterThanRight))  // so does this object have a better right than the minimum right that was passed in?
        {
          if (whereclause.Length == 0)                                        // If this is the beginning of the where clause
          {
            whereclause = " Entity IN (";                                   // then create the start of the string
          }
          whereclause = whereclause + " '" + myEntityID + "'";     // now add the next item
          if (i < entityIDs.Count - 1)
            whereclause = whereclause + ",";
        }
      }
      if (whereclause.Length > 1)
      {
        whereclause = whereclause + " )";
      }
      //}
      //catch(Exception e)
      //{
      //  whereclause = "";
      //  errorMessage = "Build of where clause failed";
      //  log.Error(Log.LogContext.Common, EM_BUILD_WHERE_CLAUSE_FAILED, e);
      //}
      lastAccessedDateTime = DateTime.Now;
      return whereclause;
    }
    /// <summary>
    /// Returns the Right associated with the global::System Object specified for this user
    /// </summary>
    /// <param name="SystemObjectName"></param>
    /// <returns></returns>
    public string Right(string SystemObjectName)
    {
      /*
       * Returns the Right for a particular system object for this user. Note, if there are more 
       * than one accessrights assigned to this system object for this user, it returns the right with 
       * the highest access (least restrictive). 
      */
      string retVal = cDENY;

      try
      {
        lastAccessedDateTime = DateTime.Now;
        if (isProgram)
          retVal = cGRANT;    // batch programs have access to everything
        else
          retVal = objectRightsList[SystemObjectName.ToLower()].ToString();       // return the right for this object
      }
      catch (KeyNotFoundException knfe)
      {
        return cDENY;              // if not found, return Deny. We do not throw an error
      }

      return retVal;
      //try
      //{
      //  return objectRightsList[SystemObjectName].ToString();       // return the right for this object
      //}
      //catch (Exception e) //TODO: logs reports that line 486 throws System.Collections.Generic.KeyNotFoundException 2 times every run
      //{
      //  log.Error(Log.LogContext.Common, EM_RIGHTS_NOT_GRANTED, e);
      //  return cDENY;              // if not found, return Deny. We do not throw an error
      //}
    }

    /// <summary>
    /// Returns the entity right associated with the requested Entity for this user
    /// </summary>
    /// <param name="pEntity"></param>
    /// <returns></returns>
    public string EntityRight(string pEntity)
    {
      /*
       * Returns the Right for a particular system object for this user. Note, if there are more 
       * than one accessrights assigned to this system object for this user, it returns the right with 
       * the highest access (least restrictive). 
      */
      string retVal = cDENY;
      lastAccessedDateTime = DateTime.Now;

      try
      {
        if (isProgram)
          retVal = cGRANT;    // batch programs have access to everything
        else
          retVal = entityRightsList[pEntity.ToLower()].ToString();       // return the right for this object
      }
      catch (KeyNotFoundException knfe)
      {
        return cDENY;              // if not found, return Deny. We do not throw an error
      }

      return retVal;
      //catch(Exception e)
      //{
      //  log.Error(Log.LogContext.Common, EM_RIGHTS_NOT_GRANTED, e);
      //  return cDENY;              // if not found, return Deny. We do not throw an error
      //}
    }
    /// <summary>
    /// Does the current list of system objects for which rights are defined for this user contain the requested SystemObject?
    /// </summary>
    /// <param name="SystemObject"></param>
    /// <returns></returns>
    public bool HasObject(string SystemObject)
    {
      /*
       * Returns true if the object is in the list of objects with some right assigned 
       * (even if it is deny) to this user
       */
      bool retVal = false;
      lastAccessedDateTime = DateTime.Now;

      try
      {
        if (isProgram)
          retVal = true;    // batch programs have rights to everything
        else
          retVal = objectRightsList.ContainsKey(SystemObject.ToLower());
      }
      catch (ArgumentNullException ane)
      {
        errorMessage = string.Format(EM_SYSTEMOBJECT_LOOKUP_FAILED, SystemObject);
        return false;
      }
      return retVal;
    }
    /// <summary>
    /// Does the current user have any access to this SystemObject?
    /// </summary>
    /// <param name="SystemObject"></param>
    /// <returns></returns>
    public bool HasObjectAccess(string SystemObject)
    {
      bool retVal = false;

      if (isProgram)
        retVal = true;      // batch programs have rights to everything
      if (HasObject(SystemObject))
        retVal = IsBetterThan(Right(SystemObject), cDENY);

      return retVal;
    }
    /// <summary>
    /// Does the current list of entities for which rigts are defined for this user contain the requested entity?
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool HasEntity(string entity)
    {
      /*
       * Returns true if the entity is in the list of objects. Note that, unlike the Object list, if an Entity has deny,
       * it is not in the list at all
       */
      bool retVal = false;
      lastAccessedDateTime = DateTime.Now;

      try
      {
        if (isProgram)
          retVal = true;      // batch programs have rights to all entities
        else
          retVal = entityRightsList.ContainsKey(entity.ToLower());
      }
      catch (ArgumentNullException ane)
      {
        //errorMessage = "Lookup of Entity " + entity + " failed";
        errorMessage = string.Format(EM_ENTITY_LOOKUP_FAILED, entity);
        return false;
      }
      return retVal;
    }
    /// <summary>
    /// Does the current user have any access to this Entity?
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool HasEntityAccess(string entity)
    {
      bool retVal = false;

      if (isProgram)
        retVal = true;    // batch programs have rights to all entities
      if (HasEntity(entity))
        retVal = IsBetterThan(EntityRight(entity), cDENY);

      return retVal;
    }
    /// <summary>
    /// Overload with uses a default for BetterThanRight of cDENY
    /// </summary>
    /// <param name="pObjectType"></param>
    /// <returns></returns>
    public ArrayList ObjectsOfType(string pObjectType)
    {
      /* 
       * overloaded version of objectsoftype which defaults to the BetterThanRight of cDENY
       */
      lastAccessedDateTime = DateTime.Now;
      return ObjectsOfType(pObjectType, cDENY);
    }
    /// <summary>
    /// Returns an ArrayList of sytem objects in the ACL of type Object Type 
    /// (see e_SystemObjects table), and with a right better than
    /// BetterThanRight. 
    /// <para>Batch programs should NOT call this, as their list will be empty</para>
    /// </summary>
    /// <param name="pObjectType"></param>
    /// <param name="BetterThanRight"></param>
    /// <returns></returns>
    public ArrayList ObjectsOfType(string pObjectType, string BetterThanRight)
    {
      /*
       * returns a list of all of the objects of type ObjectType which 
       * have a right better than the BetterThanRight in the users rights list
       * 
       * Note: batch programs should not call this routine. If they do, no object
       * will be in their list.
       */
      ArrayList myList = new ArrayList();
      int i;
      string myObject;
      string objectTypeCompare = pObjectType.ToLower();
      for (i = 0; i < objectList.Count; i++)
      {
        myObject = objectList[i].ToString().ToLower();
        if (ObjectTypes.ContainsKey(myObject) && ObjectTypes[myObject] == objectTypeCompare)
        {
          if (IsBetterThan(Right(myObject), BetterThanRight))
          {
            myList.Add(objectList[i].ToString());
          }
        }
      }
      lastAccessedDateTime = DateTime.Now;
      return myList;
    }

    /// <summary>
    /// Is Right1 better than (higher access level according to established rights precedence) Right2?
    /// </summary>
    /// <param name="Right1"></param>
    /// <param name="Right2"></param>
    /// <returns>True if Right1 is better than Right2, false if it is not</returns>
    public bool IsBetterThan(string Right1, string Right2)
    {
      /*
       * Used instead of invoking this in each routine, so that our scheme (currently the best right has the lowest index)
       * is encapsulated here
       */
      lastAccessedDateTime = DateTime.Now;
      if (Right2 == cNONE)                 // None is special case right that is worse than any other right, even "Deny"
        return true;
      return AccessRight(Right1) < AccessRight(Right2);
    }

    /// <summary>
    /// Where clause format (without the Where) of EventCodes the user has some access to
    /// </summary>
    /// <returns></returns>    
    public string EventWhereClause()
    {
      return EventWhereClause(cDENY, false);
    }

    /// <summary>
    /// Event List
    /// </summary>
    /// <returns></returns>
    public string EventList() //TODO: summarize this function
    {
      return EventWhereClause(cDENY, true);
    }

    /// <summary>
    /// Overload that allows specification of a betterThanRight
    /// </summary>
    /// <param name="BetterThanRight"></param>
    /// <param name="commasOnly"></param>
    /// <returns></returns>
    public string EventWhereClause(string BetterThanRight, bool commasOnly)
    {
      string eventWhereClause = string.Empty;
      bool firstOne = true;
      ArrayList eventList = ObjectsOfType("Event", BetterThanRight);
      foreach (object eventCode in eventList)
      {
        if (firstOne)
          firstOne = false;
        else
          eventWhereClause += ", ";
        eventWhereClause += eventCode.ToString();
      }
      if (eventWhereClause != string.Empty && !commasOnly)
        eventWhereClause = " Event in (" + eventWhereClause + ")";
      return eventWhereClause;
    }
    public bool AddEntity(Entity myEntity, string Right)
    {
      /*
       * Add an Entity to the entityRightsList and entityList collections
       */
      Log log = (Log)LogFactory.GetInstance().GetLog("TAGBOSS.Common.Security");

      try
      {
        if (entityRightsList.ContainsKey(myEntity.ID.ToString()))
          entityRightsList[myEntity.ID.ToString()] = Right;
        else
          entityRightsList.Add(myEntity.ID.ToString(), Right);   // add this if it does not already exist
        if (IsBetterThan(Right, cDENY))
          if (!entityList.ContainsKey(myEntity.ID.ToString()))
            entityList.Add(myEntity.ID.ToString(), myEntity.ReadOnly);    // and add it to the entityList if it is not Denied       
      }
      catch (NullReferenceException nre) // TODO: logs reports that line 1114 throws System.NullReferenceException   
      {
        log.Error("Add of Entity right failed", nre);
        return false;
      }
      catch (ArgumentNullException ane)
      {
        log.Error("Add of Entity right failed", ane);
        return false;
      }
      return true;
   }
    #endregion
  }
}
