using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;

using CCI.Common;
using CCI.Sys.Data;
using CCI.Common.Logging;

//using CCI.Common;
//using CCI.Common.Model;


namespace CCI.Sys.SecurityEngine
{
  /// <summary>
  /// Load the Security Class with data from the database based on authentication, and then process and create the security matrix in the Security class
  /// </summary>
  public class LoadSecurity
  {

    private const string cMTYPEGROUP = "Group";
    private const string cENTITYTYPE = "EntityType";
    #region private classes

    private class RightEntry
    {
      public string SecurityObjectGroup;
      public string Type;
      public string AccessRight;
    }
    #endregion
    #region ErrorMessages
    private const string EM_CREATE_ENTITY_LIST_FROM_ET_FAILED = "Create Entity list from EntityType right failed";
    private const string EM_CREATE_INDIVIDUAL_ENTITY_RIGHTS_FAILED = "Create individual Entity rights failed";
    private const string EM_BUILD_WHERE_CLAUSE_FAILED = "Build of where clause failed";
    private const string EM_LOGOUT = "Logout error";  //this occurs in logout, how ever message is for log purpose. May changed it
    private const string EM_RIGHTS_NOT_GRANTED = "Rights not granted";
    private const string EM_SQL_ERROR_ACCESSING_OBJECT_TYPE = "SQL error in accessing ObjectType";
    private const string EM_ERROR_ASSIGNMENT_GROUP = "{0} Error in get Assignments for groups"; 
    private const string EM_ERR_GET_USER_ASSIGMENTS = "{0} Error in get Assignments for the user";
    private const string EM_RIGHTS_ERROR = "{0} Error in get Rights";   
    private const string EM_ERR_POPULATE_RIGHTS_4USER = "{0} Error in get populate rights list for the user";
    private const string EM_OBJECT_RIGHT_ADDITION_FAILED = "Add of object right failed";
    private const string EM_RIGHTS_LOOKUP_FAILED = "Rights lookup failed";
    private const string EM_ENTITY_RIGHT_ADDITION_FAILED ="Add of Entity right failed";   
    #endregion

    #region module data
    /* 
     * State stuff
     */

    private Log log = (Log)LogFactory.GetInstance().GetLog("CCI.Sys.SecurityEngine.LoadSecurity");
    //Security myIdentity = new Security();
    //EntityBase myEntity = new EntityBase();
    SecurityDB mySecurityDB = new SecurityDB();

    private EncryptDecryptString encryptDecrypt = new EncryptDecryptString();
    
    #endregion module data

    #region module constructors/destructors

    #endregion module constructors/destructors

    #region public properties


    ///// <summary>
    ///// SecurityHandle of this instance
    ///// </summary>
    //public int Handle
    //{
    //  get { return myIdentity.Handle; }
    //  set { myIdentity.Handle = value; }
    //}

    //public bool RightsOK
    //{
    //  get { return myIdentity.RightsOK; }
    //  set { myIdentity.RightsOK = value; }
    //}
    ///// <summary>
    ///// Returns the constant that Security uses to signify denied access
    ///// </summary>
    //public string Deny { get { return Security.DENY; } }
    ///// <summary>
    ///// Returns the constant that Security uses to signify readonly access
    ///// </summary>
    //public string ReadOnly { get { return Security.READONLY; } }
    ///// <summary>
    ///// Returns the Entity id of the currently logged in user
    ///// </summary>
    //public string User
    //{
    //  get
    //  {
    //    return myIdentity.User;
    //  }
    //}
    ///// <summary>
    ///// last login in the format loginid@domainname
    ///// </summary>
    //public string UserLogin
    //{
    //  get { return myIdentity.UserLogin; }
    //}


    ///// <summary>
    ///// If a workflow action (batch program) is logged in, which is it?
    ///// </summary>
    //public string ProgramID
    //{
    //  get { return myIdentity.ProgramID; }
    //  set { myIdentity.ProgramID = value; }
    //}
    ///// <summary>
    ///// If the security is based on an EntitySource instead of a user, what is that Entity?
    ///// </summary>
    //public string EntitySource
    //{
    //  get { return entitySource; }
    //  set { entitySource = value; }
    //}
    ///// <summary>
    ///// Returns the count of the ACL (Object Rights List) entries
    ///// </summary>
    //public int Count
    //{
    //  get { return myIdentity.ObjectRightsList.Count; }
    //}
    ///// <summary>
    ///// Do we include Employees in the Entity Rights List. Normally not, because the list is long, unless we are interested in a specific 
    ///// empoloyee
    ///// </summary>
    //public bool SecureEmployees
    //{
    //  get { return myIdentity.SecureEmployees; }
    //  set { myIdentity.SecureEmployees = value; }
    //}
    ///// <summary>
    ///// ArrayList list of EntityTypes that we do not want included in the rights list.
    ///// </summary>
    //public ArrayList DontIncludeEntityTypes         // allow the calling program to override this list of EntityTypes not to include in the Entities Rights List
    //{
    //  get { return myIdentity.DontIncludeEntityTypes; }
    //  set { myIdentity.DontIncludeEntityTypes = value; }
    //}
    ///// <summary>
    ///// If certain errors occur, there will be an error message here
    ///// </summary>
    //public string Error
    //{
    //  get
    //  {
    //    /*
    //                 * Returns a string if an error has occurred, and empty string if not. If there was an error, it 
    //                 * returns the error message for that error.
    //                 */
    //    return myIdentity.Error;
    //  }
    //}
    ///// <summary>
    ///// DateTime this Security Object was instantiated
    ///// </summary>
    //public DateTime CreateDateTime
    //{
    //  get { return myIdentity.CreateDateTime; }
    //}
    ///// <summary>
    ///// Last DateTime this security object was accessed in any way
    ///// </summary>
    //public DateTime LastAccessedDateTime
    //{
    //  get { return myIdentity.LastAccessedDateTime; }
    //  set { myIdentity.LastAccessedDateTime = value; }
    //}
    ///// <summary>
    ///// Is the user in a logged in status?
    ///// </summary>
    //public bool IsLoggedIn
    //{
    //  get { return myIdentity.IsLoggedIn; }
    //}
    ///// <summary>
    ///// Is this a program instead of a normal user?
    ///// </summary>
    //public bool IsProgram
    //{
    //  get { return myIdentity.IsProgram; }
    //}
    
    ///// <summary>
    ///// List of groups of which the user is a member
    ///// </summary>
    //public ArrayList ParentGroups
    //{
    //  get { return myIdentity.ParentGroups; }
    //}

    ///// <summary>
    ///// Returns a collection of Entities for which rights are defined for this user
    ///// </summary>
    //public EntityAttributesCollection EntityList
    //{
    //  get
    //  {
    //    return myIdentity.EntityList;
    //  }
    //}
    #endregion public properties

    #region public methods
    public Security newLogin(string domain, string loginid, string password)
    {
      Security identity = new Security();
      identity.SecureEmployees = false;
      identity.DontIncludeEntityTypes = new ArrayList() { "Vendor", "Bank", "Prospect", "Contact" };
      LoadPrecedence(identity); 
      return newLogin(domain, loginid, password, identity);
    }
    public Security newLogin(string domain, string loginid, string password, Security identity)
    {
      /*
       * Returns True if authentication was successful, False if not. If successful, it processes the 
       * security matrix and loads the access rights for the user.
       * 
       * Note: since we added logic to expire logins, we can have a situation in which we are logging 
       * in again, but just because the last on expired. If this is the case, and we are logging
       * in as the same user as the last expired one, then we reauthenticate, but we don't
       * reload the rights matix if we succeed. It is still good from the last time. This
       * is a performance benefit.
       */
      string encryptedPassword;

      identity.IsLoggedIn = false;
      encryptedPassword = encryptDecrypt.encryptString(password);
      
      string newLogin = loginid + "@" + domain;
      if (identity.IsLoggedIn)                                      // are we already logged in?
        if (newLogin == identity.UserLogin)                       // yes we are, is it as the same user?
          return identity;                                   // yep just return success
        else
          identity = doLogin(domain, loginid, encryptedPassword, false, identity); // nope we are logging in as someone else
      else
        if (newLogin == identity.UserLogin)                       // no, but we are logging in as the same person
          identity = doLogin(domain, loginid, encryptedPassword, true, identity);  // authenticate, but don't reload
        else
          identity = doLogin(domain, loginid, encryptedPassword, false, identity); // nope we are logging in for the first time
      if (identity.IsLoggedIn)
        identity.UserLogin = newLogin;                            // if we succeeded, save the user login
      identity.Domain = domain;
      return identity;
    }
    public Security newLogin(string programName)
    {
      Security identity = new Security();
      identity.ProgramID = programName;
      identity.IsLoggedIn = true;
      identity.IsProgram = true;
      identity.User = programName;
      return identity;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="login"></param>
    /// <param name="domain"></param>
    /// <param name="newPassword"></param>
    /// <returns></returns>
    public bool SavePassword(string login, string domain, string newPassword)
    {
      try
      {
        string encrypted = encryptDecrypt.encryptString(newPassword);
        mySecurityDB.SavePassword(login, domain, encrypted);
        return true;
      }
      catch 
      {
        return false;
      }
    }
    
    #endregion public methods

    #region private methods
    private Dictionary<string, string> getObjectTypes()
    {
      DataSet dsTypes = mySecurityDB.getAllObjectTypes();
      Dictionary<string, string> objectTypeList = new Dictionary<string, string>();
      if (dsTypes != null && dsTypes.Tables.Count == 1)
        foreach (DataRow row in dsTypes.Tables[0].Rows)
        {
          string objectName = CommonFunctions.CString(row["SecurityObject"]).ToLower();
          string objectType = CommonFunctions.CString(row["ObjectType"]).ToLower();
          if (objectTypeList.ContainsKey(objectName))
            objectTypeList[objectName] = objectType;
          else 
            objectTypeList.Add(objectName, objectType);
        }
      return objectTypeList;
    }
    private bool UserIsInActiveDirectory(string domain, string loginid)
    {
      /*
               * checks to see if this user authenticates in Active Directory. 
               * 
               * Until this is implemented, this always returns false
               */
      return false;
    }
    //private bool LoadPrecedence()
    //{
    //  return LoadPrecedence(myIdentity);
    //}
    private bool LoadPrecedence(Security identity)
    {
      /* 
               * this routine will load the rights names from the TAGAttribute Object instead of using the constants
               */
      identity.AccessRightNames = new ArrayList { "Grant", "ReadWriteUpdateDelete", "ReadWriteUpdate", "ReadWrite", "UpdateWithApproval", "Create", Security.READONLY, Security.DENY };
      return true;
    }

    private Security LoadRights(Security identity)
    {
      identity = LoadObjectRights(identity);
      //if (identity.RightsOK)
      //{
      //  identity = LoadEntityRights(identity);
      //}
      if (identity.RightsOK)
        identity.IsLoggedIn = true;
      else
        identity.IsLoggedIn = false;
      return identity;
    }
    //private TableHeader getMasterResourceList(string domain)
    //{
    //  DataSet dsResourceList = mySecurityDB.getResourceList(domain);
    //  TableHeader thResourceList = mySecurityDB.ToTable(dsResourceList, DictionaryFactory.getInstance().getDictionary());
    //  return thResourceList;
    //}
    //private Security LoadEntityRights(Security identity)
    //{
    //  identity.RightsOK = true;
    //  ArrayList myEntityList;
    //  Entity myEntityEntry = new Entity();
    //  string tmpEntity = "";
    //  string tmpEntityOwner = "";
    //  string tmpEntityType = "";
    //  int i, j;
    //  try
    //  {
    //    /*
    //     * first, collect the groups defined by EntityType and populate the EntityList with
    //     * their children
    //     */
    //    myEntityList = identity.ObjectsOfType(cENTITYTYPE);         // get the list of objects of type EntityType
    //    if (!identity.SecureEmployees)
    //      if (!identity.DontIncludeEntityTypes.Contains(myEntity.EntityTypeEmployee))
    //        identity.DontIncludeEntityTypes.Add(myEntity.EntityTypeEmployee);         // and don't include employees if the flag is set to false
    //    for (i = 0; i < myEntityList.Count; i++)            // for each one
    //    {
    //      tmpEntityType = myEntityList[i].ToString();       // get the next in the list of EntityTypes from the rights list
    //      tmpEntityOwner = myEntity.EntityOwner(identity.User, tmpEntityType); // find the entity owner of our user at the EntityType level specified
    //      EntityAttributesCollection tmpEntityList = new EntityAttributesCollection();  // create a list to capture the list of children
    //      // get the list of children of this entity, down to the Client level, and excluding Banks and Vendors
          
    //      tmpEntityList = myEntity.EntityChildren(tmpEntityOwner, "Client", identity.DontIncludeEntityTypes, false,"", DateTime.Now, false, false, -1, false, identity.Handle);
          
    //      for (j = 0; j < tmpEntityList.Entities.Count; j++)// for each one of those...
    //      {
    //        myEntityEntry = (Entity)tmpEntityList.Entities[j];  // set the Entries.Entity object to this entry
    //        tmpEntity = myEntityEntry.ID.ToString();         // get the entity id
    //        if (identity.HasEntity(tmpEntity))                        // does this Entity already exist in the EntityRights list?
    //        {
    //          if (identity.IsBetterThan(identity.Right(tmpEntityType), Security.DENY))// yes, so is the new right better than Deny? Note all of the rights of the children are the same as the right of the parent
    //          {
    //            if (identity.IsBetterThan(identity.EntityRight(tmpEntityType), identity.EntityRightsList[tmpEntity].ToString()))  // is this right is better than one already in the list from a group
    //            {
    //              identity.EntityRightsList[tmpEntity] = identity.Right(tmpEntityType);// yes, so replace it
    //              // and then set the readonly flag in the identity.EntityList appropriately
    //              identity.EntityList[tmpEntity] =
    //                !identity.IsBetterThan(identity.EntityRight(tmpEntity), Security.READONLY);
    //            }
    //          }
    //          else
    //          {
    //            // If the access is deny, we delete it from the EntityList
    //            if (identity.EntityList.ContainsKey(tmpEntity))
    //            {
    //              //bool saveMarkForDelete = identity.EntityList.Entities.MarkForDelete; // save the markfordelete status
    //              identity.EntityList.Remove(tmpEntity);   // and reset ReadOnly to the appropriate value
    //              //identity.EntityList.Entities.MarkForDelete = saveMarkForDelete;      // restore the status
    //            }
    //          }
    //        }
    //        else
    //        {
    //          // our new entity does not yet exist in the rights list
    //          if (!identity.IsBetterThan(identity.Right(tmpEntityType), Security.READONLY))
    //            myEntityEntry.ReadOnly = true; // and set readonly if needed
    //          identity.AddEntity(myEntityEntry, identity.Right(tmpEntityType));      // Add to Entity Rights
    //        }
    //      }
    //    }
    //  }
    //  catch(NotSupportedException nse)
    //  {
    //    identity.Error = "Create Entity list from EntityType right failed";
    //    log.Error(Log.LogContext.Common, EM_CREATE_ENTITY_LIST_FROM_ET_FAILED, nse);
    //    identity.RightsOK = false;
    //    return identity;
    //  }
    //  try
    //  {
    //    /*
    //     * This was added by LLA 1/5/2010 to allow us to specify a right for users that have access just to their client data. 
    //     * The EntityType/Client right gives them access to all of the daughters, but not to the Client itself. This is
    //     * especially troublesome for employee rights, because we don't keep track of them individually, and essentially,
    //     * their right IS the right of the client, so the EntityType/Client really doesn't work like we would expect. 
    //     * This is another SecurityObject of Type Entity, but it is a special case called "EntityParentClient". This is not
    //     * a "real" entity right, but a special entity that means the user has access to any client that owns them.
    //     */
    //    if (identity.ObjectList.Contains("entityparentclient"))
    //    {
    //      string myEntityClient = myEntity.EntityOwner(identity.User, "Client").ToLower();
    //      if (myEntityClient != null && myEntityClient != string.Empty)
    //      {
    //        identity.ObjectList.Add(myEntityClient);
    //        identity.ObjectTypes.Add(myEntityClient, "entity");
    //        identity.ObjectRightsList.Add(myEntityClient, identity.Right("entityparentclient"));
    //      }
    //    }        
    //    /*
    //     * Now add the individual Entity Rights. Note: we do that last because of the special case where
    //     * an individual Security.DENY to an Entity will override a group access at a better level
    //     */
    //    myEntityList = identity.ObjectsOfType("Entity", Security.NONE); //  list of individual Entities that Rights are defined for

    //    for (i = 0; i < myEntityList.Count; i++)
    //    {
    //      tmpEntity = myEntityList[i].ToString().ToLower();                // get the entity id
    //      if (identity.HasEntity(tmpEntity))                              // is this already in the list?
    //      {
    //        if (identity.IsBetterThan(identity.Right(tmpEntity), Security.DENY))    // if so, is the new right better than Deny?
    //        {
    //          if (identity.IsBetterThan(identity.EntityRight(tmpEntity), identity.EntityRightsList[tmpEntity].ToString()))  // This right is better than one already in the list from a group
    //          {
    //            identity.EntityRightsList[tmpEntity] = identity.EntityRight(tmpEntity);   // so replace it
    //            identity.EntityList[tmpEntity] =               // and reset ReadOnly to the appropriate value
    //              !identity.IsBetterThan(identity.EntityRight(tmpEntity), Security.READONLY);
    //          }
    //        }
    //        else
    //        {
    //          /*
    //           * This is a special case. If an individual pEntityType is given the right of Deny, then
    //           * it will override the better right gained from a group.
    //           */
    //          identity.EntityRightsList[tmpEntity] = Security.DENY;
    //          // Also, if the access is deny, we delete it from the EntityList
    //          //bool saveMarkForDelete = identity.EntityList.Entities.MarkForDelete; // save the markfordelete status
    //          identity.EntityList.Remove(tmpEntity);   // and reset ReadOnly to the appropriate value
    //          //identity.EntityList.Entities.MarkForDelete = saveMarkForDelete;      // restore the status
    //        }
    //      }
    //      else
    //      {
    //        myEntityEntry = myEntity.Entity(tmpEntity, identity.Handle);  // get an Entity object for this Entity
    //        if (myEntityEntry != null)  // only if we got one
    //          identity.AddEntity(myEntityEntry, identity.Right(tmpEntity));       // and add it
    //      }

    //    }
    //  }
    //  catch(Exception e)
    //  {
    //    identity.Error = "Create individual Entity rights failed";
    //    log.Error(Log.LogContext.Common, EM_CREATE_INDIVIDUAL_ENTITY_RIGHTS_FAILED, e);
    //    identity.RightsOK = false;
    //    return identity;
    //  }
    //  return identity;
    //}
    
    private Security LoadObjectRights(Security identity)
    {
      const string modulename = "LoadObjectRights";
      identity.RightsOK = true;                                                           // Assume success unless something goes wrong
      SecurityGroups mySecurityGroups = new SecurityGroups(mySecurityDB.tblGroups);   // Set up Groups to give me what group the m_Entity(user) belongs to
      DataSet dsAssignments;                                                          // the Assignments will give a list of SecurityRights for each group
      DataSet dsRights;                                                               // this table assigns rights (Grant, Deny, etc.) to SecurityObject Groups
      ArrayList mySecurityRights = new ArrayList();                                   // This will be the list of my Security Rights   
      ArrayList myRightsEntries = new ArrayList();                                    // list of SecurityObjectGroups/Access rights
      identity.ParentGroups = mySecurityGroups.Parents(identity.User);                   // identity.ParentGroups is the list of Groups to which the m_Entity belongs
      SecurityGroups myObjectGroups = new SecurityGroups(mySecurityDB.tblObjectGroups); // set up a group object for this group tpye
      int i, j;                                                                       // we need a bunch of subscripts
      try
      {
        /*
         * Get the groups the Entity/user belongs to
         */
        for (i = 0; i < identity.ParentGroups.Count; i++)                                           // for each of the groups I belong to
        {
          dsAssignments = mySecurityDB.getAssignments(identity.ParentGroups[i].ToString());       // get the Assignments for that group (list of Security Rights
          for (j = 0; j < dsAssignments.Tables[0].Rows.Count; j++)                    // For each Assignment/Security Right
          {
            if (mySecurityRights.IndexOf(dsAssignments.Tables[0].Rows[j]["SecurityRight"].ToString()) == -1)   // Is this SecurityRight in the list?
            {
              mySecurityRights.Add(dsAssignments.Tables[0].Rows[j]["SecurityRight"].ToString()); // if not, then add it
            }
          }
          dsAssignments = null;                                                       // reset dataset
        }
      }
      catch(Exception e)
      {
        //identity.Error = modulename + " Error in get Assignments for groups";
        identity.Error = string.Format(EM_ERROR_ASSIGNMENT_GROUP, modulename);
        log.Error(Log.LogContext.Common, string.Format(EM_ERROR_ASSIGNMENT_GROUP, modulename), e);
        identity.RightsOK = false;
        return identity;
      }
      try
      {
        /*
         * Get the Assignments for the user
         */
        dsAssignments = mySecurityDB.getAssignments(identity.User);                            // now see if there are any assignments directly to the user
        for (j = 0; j < dsAssignments.Tables[0].Rows.Count; j++)                        // For each Assignment/Security Right
        {
          if (mySecurityRights.IndexOf(dsAssignments.Tables[0].Rows[j]["SecurityRight"].ToString()) == -1)   // Is this SecurityRight in the list?
          {
            mySecurityRights.Add(dsAssignments.Tables[0].Rows[j]["SecurityRight"].ToString()); // if not, then add it
          }
        }
      }
      catch(Exception e)
      {
        //identity.Error = modulename + " Error in get Assignments for the user";
        identity.Error = string.Format(EM_ERR_GET_USER_ASSIGMENTS, modulename);
        log.Error(Log.LogContext.Common, string.Format(EM_ERR_GET_USER_ASSIGMENTS, modulename), e);
        identity.RightsOK = false;
        return identity;
      }
      try
      {
        /*
         * Populate the list of security rights entries (pairs of SecurityObject/Groups and Access Rights
         */
        dsAssignments = null;
        for (i = 0; i < mySecurityRights.Count; i++)                                    // for each assignment/right
        {
          dsRights = mySecurityDB.getRights(mySecurityRights[i].ToString());          // get a list of rights for for this Security Right
          for (j = 0; j < dsRights.Tables[0].Rows.Count; j++)                         // for each one...
          {
            RightEntry myRightEntry = new RightEntry();                             // create an entry struct and populate
            myRightEntry.SecurityObjectGroup = dsRights.Tables[0].Rows[j]["SecurityObjectGroup"].ToString();
            myRightEntry.Type = dsRights.Tables[0].Rows[j]["Type"].ToString();
            myRightEntry.AccessRight = dsRights.Tables[0].Rows[j]["AccessRight"].ToString();
            if (myRightsEntries.IndexOf(myRightEntry) == -1)                        // if it does not exist in the list
            {
              myRightsEntries.Add(myRightEntry);                                  // add it
            }
          }
          dsRights = null;
        }
      }
      catch(Exception e)
      {
        identity.Error = modulename + " Error in get Rights";
        log.Error(Log.LogContext.Common, string.Format(EM_RIGHTS_ERROR, modulename), e);        
        identity.RightsOK = false;
        return identity;
      }
      try
      {
        /*
         * now populate the final rights list for each Security Group and Security Object in the list
         */
        for (i = 0; i < myRightsEntries.Count; i++)
        {
          RightEntry myRightsEntry = (RightEntry)myRightsEntries[i];  // note that we apply the same AccessRight to every SystemObject in the group
          if (myRightsEntry.Type.ToString() == cMTYPEGROUP)           // is this right assigned to a group?  
          {
            /*
             * This is a group. Find all of the SystemObjects in the group, and then add the same AccessRight for each one
             */
            ArrayList myObjectList = myObjectGroups.Children(myRightsEntry.SecurityObjectGroup.ToString());    // and find its members
            for (j = 0; j < myObjectList.Count; j++)                  // for each member
            {
              if (identity.HasObject(myObjectList[j].ToString()))              // is this one already there?
              {
                /*
                 * The SystemObject already exists. Check the Rights Heirarchy and see if we need to override. 
                 * 
                 * If this right is higher up the heirarchy (has a lower index in AccessRightNames), then we override,
                 * because the rule is that the logged in user has the highest access available to them on any given
                 * SystemObject. Otherwise, we skip this entry
                 */
                if (identity.IsBetterThan(myRightsEntry.AccessRight.ToString(), identity.Right(myObjectList[j].ToString())))  // Is the right we tried to add better than the one that is already in the list?
                {
                  identity.UpdateRight(myObjectList[j].ToString(), myRightsEntry.AccessRight.ToString());                 // update the right
                }
              }
              else
              {
                identity.AddRight(myObjectList[j].ToString(), myRightsEntry.AccessRight.ToString());
              }
            }
          }
          else
          {
            /*
             * This is not a group, but an individual object, so we just add it individually
             */
            string thisObject = myRightsEntry.SecurityObjectGroup.ToString();
            string thisRight = myRightsEntry.AccessRight.ToString();
            if (identity.HasObject(thisObject))         // is this one already there?
            {
              /*
               * The SystemObject already exists. Check the Rights Heirarchy and see if we need to override. 
               * 
               * If this right is higher up the heirarchy (has a lower enum value in AccessRights), then we override,
               * because the rule is that the logged in user has the highest access available to them on any given
               * SystemObject
               */
              string lastRight = identity.Right(thisObject);
              if (identity.IsBetterThan(thisRight, lastRight) ||   // Is the right we tried to add better than the one that is already in the list?
                thisRight == Security.DENY)   // deny is a special case. If we have a deny, then this overrides the normal rights chain (for now)
              {
                identity.UpdateRight(thisObject, thisRight);                 // update the right
              }
            }
            else
            {
              identity.AddRight(thisObject, myRightsEntry.AccessRight.ToString());
            }
          }
        }
      }
      catch(Exception e)
      {
        identity.Error = modulename + " Error in get populate rights list for the user";
        log.Error(Log.LogContext.Common, string.Format(EM_ERR_POPULATE_RIGHTS_4USER, modulename), e);
        identity.RightsOK = false;
        return identity;
      }
      return identity;
    }

    private Security doLogin(string domain, string loginid, string password, bool sameUser, Security identity)
    {
      identity.ObjectTypes = getObjectTypes();
      DataSet dsUser = new DataSet();
      
      dsUser = mySecurityDB.getUser(loginid, domain);
      if (dsUser == null || dsUser.Tables.Count == 0 || dsUser.Tables[0].Rows.Count == 0)  // does this user exist in the SecurityUsers table?
      {
        identity.Error = "User does not exist";
        identity.IsLoggedIn = false;       // nope, login fails.
      }
      else
      {
        /*
                     * Yes, user exists in the table. Now check to see if they are in ActiveDiretory
                     */
        if (UserIsInActiveDirectory(domain, loginid))       // if user is in AD, we do not need a password
        {
          if (sameUser)
            identity.IsLoggedIn = true;                                // we are just reauthenticating, so don't load the rights
          else
          {
            identity.User = dsUser.Tables[0].Rows[0]["Entity"].ToString();       // Get the entity for this user
            identity = LoadRights(identity);                // load the rights and objects for this user
            if (!identity.IsLoggedIn)
              identity.Error = "Load Rights failed";
          }
        }
        else
        {
          /*
                           * Otherwise, we need to see if the password matches
                           */
          if (dsUser.Tables[0].Rows[0]["Password"].ToString() == password)   // since this user is not in AD, we need to check their password
          {
            identity.IsLoggedIn = true;  // we passed authentication, so we are logged in
            if (!sameUser)       // are we logging in as the same user as last time?
            {
              // no, so load the rights
              identity.User = dsUser.Tables[0].Rows[0]["Entity"].ToString();    // Get the entity for this user
              identity = LoadRights(identity);             // load the rights and objects for this user
              if (!identity.IsLoggedIn)
                identity.Error = "Load Rights failed";
            }
          }
          else
          {
            identity.IsLoggedIn = false;                            // login failed
            identity.Error = "Authentication failed";
          }
        }
      }
      identity.LastAccessedDateTime = DateTime.Now;

      return identity;
    }
    //private bool doLogin(string domain, string loginid, string password, bool sameUser)
    //{
    //  DataSet dsUser = new DataSet();

    //  dsUser = mySecurityDB.getUser(loginid, domain);
    //  if (dsUser == null || dsUser.Tables.Count == 0 || dsUser.Tables[0].Rows.Count == 0)  // does this user exist in the SecurityUsers table?
    //  {
    //    identity.Error = "User does not exist";
    //    loggedin = false;       // nope, login fails.
    //  }
    //  else
    //  {
    //    /*
    //                 * Yes, user exists in the table. Now check to see if they are in ActiveDiretory
    //                 */
    //    if (UserIsInActiveDirectory(domain, loginid))       // if user is in AD, we do not need a password
    //    {
    //      if (sameUser)
    //        loggedin = true;                                // we are just reauthenticating, so don't load the rights
    //      else
    //      {
    //        userEntity = dsUser.Tables[0].Rows[0]["Entity"].ToString();       // Get the entity for this user
    //        loggedin = LoadRights();                // load the rights and objects for this user
    //        if (!loggedin)
    //          identity.Error = "Load Rights failed";
    //      }
    //    }
    //    else
    //    {
    //      /*
    //                       * Otherwise, we need to see if the password matches
    //                       */
    //      if (dsUser.Tables[0].Rows[0]["Password"].ToString() == password)   // since this user is not in AD, we need to check their password
    //      {
    //        if (sameUser)       // it does, but are we logging in as the same user as last time?
    //          loggedin = true;  // yes, so don't reload the rights
    //        else
    //        {
    //          // no, so load the rights
    //          userEntity = dsUser.Tables[0].Rows[0]["Entity"].ToString();    // Get the entity for this user
    //          loggedin = LoadRights();             // load the rights and objects for this user
    //          if (!loggedin)
    //            identity.Error = "Load Rights failed";
    //        }
    //      }
    //      else
    //      {
    //        loggedin = false;                            // login failed
    //        identity.Error = "Authentication failed";
    //      }
    //    }
    //  }
    //  lastAccessedDateTime = DateTime.Now;

    //  return loggedin;
    //}

    #endregion private methods
  }
}
