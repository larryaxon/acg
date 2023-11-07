using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CCI.Common;
using CCI.Sys.Data;
using CCI.Common.Logging;

namespace CCI.Sys.SecurityEngine
{
  /// <summary>
  /// Singleton Security factory
  /// </summary>
  public class SecurityFactory
  {
    protected static readonly SecurityFactory securityFactoryInstance = new SecurityFactory();
    // Storage for a list of all registered users and their security matrix
    private Dictionary<int, Security> registeredUsers = new Dictionary<int, Security>();
    private double expirationMinutes = 720.0; // stale entries are eliminated after this amount of time untouched
    private int nextHandle = 1;
    private DateTime lastTimeStaleEntriesRemoved = DateTime.Now;
    private LoadSecurity loadSecurity = new LoadSecurity();
    private Log log = (Log)LogFactory.GetInstance().GetLog("CCI.Sys.SecurityEngine");
    protected SecurityFactory()
    {
      //DictionaryFactory.getInstance().setDictionary(loadDictionary());  // load the dictionary the first time
      loadSecurity = new LoadSecurity();
    }
    public static SecurityFactory GetInstance()
    {
      return securityFactoryInstance;
    }
    ///// <summary>
    ///// Request a Security object instance using the login info. If it does not exist, it is
    ///// created. If it does exist, the one that does exist is returned. 
    ///// <para>Note that, if the user has expired, or not yet logged ibn, then this will return a new, not logged
    ///// in copy of the security object. The calling program should therefore always
    ///// check loadSecurity.IsLoggedIn after this call to make sure it is not stale.</para>
    ///// </summary>
    ///// <param name="login">String in the format loginid@domainname</param>
    ///// <returns>A Security object for this login</returns>
    //public Security getSecurity()
    //{
    //  Security newEntry = new Security();           // create a new security object
    //  newEntry.Handle = getHandle();
    //  registeredUsers.Add(newEntry.Handle, newEntry);         // register it and this user
    //  return newEntry;                              // and return the object
    //}
    public Security getSecurity(string domain, string loginid, string password)
    {
      Security newEntry;
      if (loginid == null || domain == null)  // you can't login with a null id
      {
        log.Error(string.Format("getSecurity({0}, {1}, Password), either the login or the domain is null", domain, loginid));
        return new Security();
      }
      newEntry = loadSecurity.newLogin(domain, loginid, password);           // log in
      if (newEntry.IsLoggedIn)
      {
        foreach (KeyValuePair<int, Security> entry in registeredUsers)
        {
          if (entry.Value.UserLogin.Equals(newEntry.UserLogin, StringComparison.CurrentCultureIgnoreCase))
          {
            newEntry.Handle = entry.Value.Handle;
            break;
          }
        }
      }
      if (newEntry.Handle == 0)
      {
        newEntry.Handle = getHandle();                               // reuse the same handle
        while (registeredUsers.ContainsKey(newEntry.Handle))        // don't add a handle that is already there
          newEntry.Handle = getHandle();

        registeredUsers.Add(newEntry.Handle, newEntry);                     // reset the profile
      }
      else
        registeredUsers[newEntry.Handle] = newEntry;
      if (newEntry.IsLoggedIn)
        log.Debug(string.Format("User {0}/{1} logged in successfully as handle {2}", newEntry.UserLogin, newEntry.User, newEntry.Handle.ToString()));
      else
        log.Error(string.Format("User {0}@{1} login failed, returning handle {2}", loginid, domain, newEntry.Handle.ToString()));
      return newEntry;      // and return it
    }
    protected Security getSecurityClone(int myHandle)
    {
      if (myHandle == 0)
      {
        log.Error("getSecurityClone was invoked with a handle of zero");
        return new Security();
      }
      if (registeredUsers.ContainsKey(myHandle))      // if the requested user is already registered
      {
        Security thisUser = (Security)registeredUsers[myHandle].Clone();// then get the security object
        if (!thisUser.IsLoggedIn)
          log.Error(string.Format("getSecurityClone failed, handle is valid, but user is not logged in", myHandle.ToString()));
        //thisUser.Handle = getHandle();
        //registeredUsers.Add(thisUser.Handle, thisUser);         // register it and this user
        return thisUser;                              // the user's session has not expired, so return it
      }
      else
      {
        log.Error(string.Format("getSecurityClone({0}): handle is not valid", myHandle.ToString()));
        return new Security();
      }
    }
    /// <summary>
    /// Overload to get an existing security object from a security handle
    /// </summary>
    /// <param name="myHandle"></param>
    /// <returns></returns>
    protected Security getSecurity(int myHandle)
    {
      if (registeredUsers.ContainsKey(myHandle))      // if the requested user is already registered
      {
        Security thisUser = registeredUsers[myHandle];
        if (!thisUser.IsLoggedIn)
          log.Error(string.Format("getSecurity({0}) failed, handle is valid, but user is not logged in", myHandle.ToString()));
        return thisUser;// then get the security object
      }
      else
      {
        log.Error(string.Format(string.Format("getSecurity({0}): handle is not valid", myHandle.ToString())));
        return new Security();           // create a new security object
      }
    }
    public string getUser(int myHandle)
    {
      if (registeredUsers.ContainsKey(myHandle)) // if the requested user is already registered
        return registeredUsers[myHandle].User;   // then get the user name from that instance
      else
        return "None";
    }
    public bool SavePassword(string login, string domain, string newPassword)
    {
      return loadSecurity.SavePassword(login, domain, newPassword);
    }
    //public void RefreshDictionary()
    //{
    //  DictionaryFactory.getInstance().setDictionary(loadDictionary());  // load the dictionary the first time
    //}
    ///// <summary>
    ///// Creates and Loads an instance of a dictionary
    ///// </summary>
    ///// <returns>A new Dictionary instance</returns>
    //public Dictionaries loadDictionary()
    //{
    //  EntityAttributesCollection doDictionary = null;
    //  AttributeProcessor _processor = new AttributeProcessor();
    //  DictionaryFactory.getInstance().setDictionary(null);
    //  doDictionary = _processor.getAttributes(TAGFunctions.DICTIONARY, null, null, null, DateTime.Today, true, false, 0);
    //  if (doDictionary == null) // if there is no dictionary, just return a blank one
    //  {
    //    doDictionary = new EntityAttributesCollection();         // hard coded to empty for now
    //    Entity e = new Entity();
    //    e.ID = TAGFunctions.DICTIONARY;
    //    doDictionary.Entities.Add(e);
    //  }
    //  Dictionaries dictionary = new Dictionaries(doDictionary);
    //  DictionaryFactory.getInstance().setDictionary(dictionary);  // set the singleton instance with the interim value so tableheader does not bomb below
    //  // Now that we have a dictionary to use, go through the colletion and resolve all TableHeader valuetypes
    //  // we can do this in the original eac since we just passed in a reference to the dicitonary constructor
    //  //doDictionary.Dictionary = dictionary;   // set the dictionary for the whole collection
    //  ItemTypeCollection itemTypes = doDictionary.Entities[TAGFunctions.DICTIONARY].ItemTypes;  // list of different dictionary 'sections' (e.g. Attribute, ItemType, etc.)
    //  foreach (ItemType items in itemTypes)
    //  {
    //    foreach (Item item in items)  // for
    //    {
    //      foreach (TAGAttribute a in item.Attributes)
    //      {
    //        //a.Dictionary = dictionary;   // same thing for each attribute
    //        // now create tableheader if necessary
    //        if (a.ValueType.ToLower() == TAGFunctions.TABLEHEADER && a.Value.GetType() != typeof(TableHeader) && a.ID != "tableheader")
    //          a.Value = new TableHeader(a.OriginalID, (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString,a.Value), dictionary);
    //      }
    //    }
    //  }
    //  return dictionary;  // return the dictionary we created
    //}
    private int getHandle()
    {
      // this can be more complicated later
      return ++nextHandle;
    }
    /// <summary>
    /// Logoff a handle, and also clean up any cuorrupt logins and handles
    /// </summary>
    /// <param name="myHandle"></param>
    public void Logout(int myHandle)
    {
      if (registeredUsers.ContainsKey(myHandle))
      {
        registeredUsers[myHandle].Logout(); // flag the security instance as logged out, since someone may have a reference to it
        registeredUsers.Remove(myHandle);   // and then delete it from the collection
      }
      Cleanup();  // now routinely go and clean up any other instances in the collection that may be expired or logged out
    }
    /// <summary>
    /// cleanup any orphaned or mismatched handles or users
    /// </summary>
    public void Cleanup()
    {
      ArrayList badEntries = new ArrayList();
      foreach (KeyValuePair<int, Security> registeredUser in registeredUsers)
      {
        if (!registeredUser.Value.IsLoggedIn || hasExpired(registeredUser.Value.LastAccessedDateTime))
          badEntries.Add(registeredUser.Key);
      }
      foreach (int handle in badEntries)
        registeredUsers.Remove(handle);
    }
    public string UserState(int handle)
    {
      if (registeredUsers.ContainsKey(handle))
        if (registeredUsers[handle].IsLoggedIn)
          return "OK";
        else
          return "LoggedOut";
      else
        return "InvalidHandle";
    }
    public string UserState(Security s)
    {
      return UserState(s.Handle);
    }
    public bool IsValid(int handle)
    {
      if (registeredUsers.ContainsKey(handle))
        if (registeredUsers[handle].IsLoggedIn)
        {
          log.Debug(string.Format("IsValid({0}) successful", handle.ToString()));
          return true;
        }
        else
        {
          log.Error(string.Format("IsValid({0}) failed, handle is valid but user is not logged in", handle.ToString()));
          return false;
        }
      else
      {
        log.Error(string.Format("IsValid({0}) failed, handle is invalid", handle.ToString()));
        return false;
      }
    }
    public bool IsValid(Security s)
    {
      return IsValid(s.Handle);
    }
    ///// <summary>
    ///// returns a list of all handles and their users in the registeredUsers collection
    ///// </summary>
    ///// <returns></returns>
    //public PickList RegisteredUsers()
    //{
    //  string[] columns = new string[] { "Handle", "Login", "LoggedIn", "LoginDateTime" };
    //  PickList returnList = new PickList(columns);
    //  int iRow = 0;
    //  foreach (KeyValuePair<int, Security> userEntry in registeredUsers)
    //  {
    //    returnList.AddRow();
    //    returnList[iRow, 0] = userEntry.Key;
    //    returnList[iRow, 1] = userEntry.Value.UserLogin;
    //    returnList[iRow, 2] = userEntry.Value.IsLoggedIn;
    //    returnList[iRow++, 3] = userEntry.Value.CreateDateTime;
    //  }
    //  return returnList;
    //}

    private bool hasExpired(DateTime lastAccessed)        // is the lastAccessed time longer ago than expirationMinutes?
    {
      return false;
      DateTime compareDateTime = lastAccessed.AddMinutes(expirationMinutes);
      return (compareDateTime < DateTime.Now);
    }
  }
}
