using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using CCI.Common;
using CCI.Sys.SecurityEngine;
using TAGBOSS.Common.Model;


namespace CCI.Sys.Data
{
  public partial class DataSource
  {
    private enum EntityMemberType { Group, Entity }
    private const string DEFAULTGROUPRELATIONSHIP = "Member";
    EntityAttributes _entityAttributes = null;
    // we init to null and separate out the property to instantiate on first use, to avoid design errors on format that try to connect to the config file and db when we are not at run time
    private EntityAttributes _ea { get { if (_entityAttributes == null) _entityAttributes = new EntityAttributes(); return _entityAttributes; } }
    #region Entity Data
    public void CreateTempEntityTable(string entityType, string AttributeName)
    {
      SearchResultCollection customers = SearchEntities("*", "Customer");
      StringBuilder cList = new StringBuilder();
      foreach (SearchResult cust in customers)
      {
        cList.Append(cust.EntityID);
        cList.Append(",");
      }
      cList.Length = cList.Length - 1; // elim last comma
      EntityAttributesCollection eac = getEntity(cList.ToString(), entityType, DateTime.Today);
      string sql = "Insert into tmpIBPCustomerList Values ('{0}', '{1}')";
      foreach (Entity en in eac.Entities)
      {
        string customer = en.OriginalID;
        string Dealer = CommonFunctions.CString(eac.getValue(string.Format("{0}.Entity.{1}.{2}", customer, entityType, AttributeName)));
        updateDataFromSQL(string.Format(sql, customer, Dealer));
      }
    }
    public void terminateEntity(string entity)
    {
      // terminate the entity by setting the EndDate to yesterday
      string sql = string.Format("Update Entity set EndDate = '{0}' Where Entity = '{1}'",
        DateTime.Today.AddDays(-1).ToShortDateString(), entity);
      updateDataFromSQL(sql);
    }
    public SearchResultCollection SearchEntities(string criteria, string entityType)
    {
      if (criteria != null && criteria.Length > 0)
        return _ea.Search(null, null, criteria, entityType, false, false).Sort("LegalName");
      else
        return new SearchResultCollection();
    }
    public SearchResultCollection SearchEntities(string criteria, string entityType, bool includeTermed)
    {
      if (criteria != null && criteria.Length > 0)
        return _ea.Search(null, null, criteria, entityType, includeTermed, false);
      else
        return new SearchResultCollection();
    }
    public SearchResultCollection SearchEntities(string criteria, string entityType, string entityOwner, bool includeTermed)
    {
      if (criteria != null && criteria.Length > 0)
        return _ea.Search(entityOwner, null, criteria, entityType, includeTermed, false);
      else
        return new SearchResultCollection();
    }
    public SearchResult getSearchResultEntity(string entity)
    {
      Entity e = _ea.Entity(entity, true);
      SearchResult s = new SearchResult();
      s.EntityID = entity;
      if (e != null)
      {
        s.LegalName = CommonFunctions.CString(e.Fields.getValue("LegalName"));
        s.EntityType = CommonFunctions.CString(e.Fields.getValue("EntityType"));
        if (e.ItemTypes.Contains("Entity"))
        {
          ItemType it = e.ItemTypes["Entity"];
          if (it.Items.Contains(s.EntityType))
          {
            Item i = it.Items[s.EntityType];
            s.FullName = CommonFunctions.CString(i.getValue("FullName"));
            s.ShortName = CommonFunctions.CString(i.getValue("ShortName"));
          }
        }
      }
      return s;
    }
    public EntityAttributesCollection getEntity(string entity, string entityType, DateTime effectiveDate)
    {
      return _ea.getAttributes(entity, "Entity", entityType, null, effectiveDate);
    }
    public EntityAttributesCollection getAttributes(string entities, string itemTypes, string items, string parameters, DateTime effectiveDate, bool getVirtualItems)
    {
      if (getVirtualItems)
        return _ea.getAttributes_withVirtualAttributes(entities, itemTypes, items, parameters, effectiveDate);
      else
        return _ea.getAttributes(entities, itemTypes, items, parameters, effectiveDate);
    }
    public EntityAttributesCollection getAttributes(string entities, string itemTypes, string items, string parameters, DateTime effectiveDate)
    {
      return _ea.getAttributes(entities, itemTypes, items, parameters, effectiveDate);
    }
    public void saveEntity(EntityAttributesCollection eac)
    {
      _ea.Save(eac, false);
    }
    public SearchResultCollection getEntityList(string entityOwner, string entityOwnerType, string entityType)
    {
      return _ea.Search(entityOwner, entityOwnerType, "*", entityType, false, false);
    }
    public bool ExistsEntity(string entity)
    {
      return _ea.existsEntity(entity);
    }
    public bool IsEntityField(string fieldName)
    {
      return _ea.IsEntityField(fieldName);
    }
    public string getNextNumericEntityID(string entityType)
    {
      return getNextNumericEntityID(entityType, 0);
    }
    public string getNextNumericEntityID(string entityType, int increment)
    {
      if (entityType == null)
        return null;
      string sql = string.Format("select max(entity) from entity where entitytype = '{0}' and len(entity) = 5", entityType);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return null;
      string maxEntity = ds.Tables[0].Rows[0][0].ToString();
      if (CommonFunctions.IsInteger(maxEntity))
        return (CommonFunctions.CInt(maxEntity) + 1 + increment).ToString();
      else
        return null;
    }
    public EntityAttributesCollection getDummyEntityRecord(string entity, string entityType)
    {
      return _ea.getAttributes_withVirtualAttributes(entity, "Entity", entityType, null, DateTime.Today);
    }
    public string getEntityType(string entity)
    {
      string sql = string.Format("Select EntityType from Entity where Entity = '{0}'", entity);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return null;
      return CommonFunctions.CString(ds.Tables[0].Rows[0][0]);
    }
    public string getEntityName(string entity)
    {
      string sql = string.Format("Select LegalName from Entity where Entity = '{0}'", entity);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return null;
      return CommonFunctions.CString(ds.Tables[0].Rows[0][0]);
    }
    public string getEntityOwner(string entity)
    {
      return _ea.EntityOwner(entity);
    }
    public bool hasLocation(string entity)
    {
      return numberLocations(entity) > 0;
    }
    public int numberLocations(string entity)
    {
      int count = 0;
      string sql = string.Format("Select count(*) NumberLocations from Entity where EntityType = 'Location' and EntityOwner = '{0}'",
        entity);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return count;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        count = CommonFunctions.CInt(ds.Tables[0].Rows[0][0]);
      ds.Clear();
      ds = null;
      return count;
    }
    public object getEntityValue(string entity, string attribute)
    {
      string sql = string.Format("select entitytype from entity where entity = '{0}'", entity);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return null;
      object o = null;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
      {
        string entitytype = CommonFunctions.CString(ds.Tables[0].Rows[0]["entitytype"]);
        o = getAttributeValue(entity, "Entity", entitytype, attribute, null, false);
      }
      ds.Clear();
      ds = null;
      return o;

    }
    public object getEntityFieldValue(string entity, string field)
    {
      if (string.IsNullOrEmpty(entity) || string.IsNullOrEmpty(field))
        return null;
      DataSet ds = getEntityRaw(entity);
      if (ds == null)
        return null;
      object o = null;
      try
      {
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
          o = ds.Tables[0].Rows[0][field];
      }
      catch
      {   
        o = null;
      }
      ds.Clear();
      ds = null;
      return o;
    }
    public object getAttributeValue(string entity, string itemType, string item, string attribute, DateTime? efDate, bool includeVirtual)
    {
      DateTime dt;
      if (efDate == null)
        dt = DateTime.Today;
      else
        dt = (DateTime)efDate;
      EntityAttributesCollection eac = _ea.getAttributes(entity, itemType, item, attribute, null, null, dt, includeVirtual);
      object ret = eac.getValue(string.Format("{0}.{1}.{2}.{3}",entity,itemType, item, attribute));
      eac = null;
      return ret;
    }
    /// <summary>
    /// Find near matches on various criteria
    /// </summary>
    /// <param name="name"></param>
    /// <param name="address"></param>
    /// <param name="city"></param>
    /// <param name="state"></param>
    /// <param name="zip"></param>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public DataSet searchEntities(string name, string address, string city, string state, string zip, string entityType, string salesOwner, bool useOR)
    {
      StringBuilder sql = new StringBuilder();
      sql.Append("Select Entity, LegalName, Address1, City, State, Zip, EntityType, EntityOwner from Entity WHERE ");
      string[] whereResults;
      if (!string.IsNullOrEmpty(name))
      {
        sql.Append("(");
        sql.Append("(");
        whereResults = CommonFunctions.MakeWhereClauseFromName("LegalName", name, false, false);
        sql.Append(whereResults[0].Replace(" WHERE ",""));
        sql.Append(")");
        whereResults = CommonFunctions.MakeWhereClauseFromName("AlternateName", name, false, false);
        if (!whereResults[0].Contains("1 = 0"))
        {
          sql.Append(" OR ");
          sql.Append("(");
          sql.Append(whereResults[0].Replace(" WHERE ", " "));
          sql.Append(")");
        }
        sql.Append(") ");
      }
      if (!string.IsNullOrEmpty(address))
      {
        string[] tokens = address.Split(new char[] { ' ' });
        string street = string.Empty;
        string number = string.Empty;
        for (int i = 0; i < tokens.GetLength(0); i++)
        {
          if (tokens[i].Length > street.Length)
            street = tokens[i];
          if (CommonFunctions.IsNumeric(tokens[i]))
            number = tokens[i];
        }
        if (!string.IsNullOrEmpty(street) || !string.IsNullOrEmpty(number))
          sql.Append(string.Format(" AND Address1 like '%{0}%{1}%", number, street));
      }    
      sql = addClause(sql, "City", getBiggestToken(city), false);
      sql = addClause(sql, "State", state, false);
      sql = addClause(sql, "Zip", zip, false);
      sql = addClause(sql, "EntityType", entityType, false);
      if (!string.IsNullOrEmpty(salesOwner))
        sql.Append(string.Format(" and Entity in (select distinct Customer from salesordealercustomers where SalesOrDealer = '{0}' and SalesType = 'Dealer') ",salesOwner));
      sql.Append(" ORDER BY LegalName, Entity");
      return getDataFromSQL(sql.ToString());
    }
    public string MakeNewEntityID(string newEntityName)
    {
      string entityID = CommonFunctions.CString(newEntityName).Trim().ToUpper();
      entityID = entityID.Substring(0, Math.Min(22, entityID.Length));
      foreach (string c in CommonData.badEntityIDChars)
        entityID = entityID.Replace(c, "");
      int seq = 0;
      while (ExistsEntity(entityID))
        entityID = string.Format("{0}{1}", entityID, (seq++).ToString());
      return entityID;
    }
    public ArrayList getEntityNames(ArrayList ids)
    {
      string idList = CommonFunctions.ToList(ids.ToArray(), "'");
      string sql = string.Format("select Entity + ': ' + LegalName Entry from Entity where Entity in ({0})", idList);
      ArrayList list = new ArrayList();
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return list;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        foreach (DataRow row in ds.Tables[0].Rows)
          list.Add(CommonFunctions.CString(row[0]));
      ds.Clear();
      ds = null;
      return list;
    }
    public DataSet getEntityRaw(string entity)
    {
      string sql = string.Format("select * from entity where entity = '{0}'", entity);
      return getDataFromSQL(sql);
    }
    #endregion
    private StringBuilder addClause(StringBuilder sb, string columnName, string value, bool like)
    {
      if (!string.IsNullOrEmpty(value))
      {
        sb.Append(" AND ");
        string format = "{0} = '{1}'";
        if (like)
          format = "{0} LIKE '%{1}%'";
        sb.Append(string.Format(format, columnName, value));
      }
      return sb;
    }
    private string getBiggestToken(string inStr)
    {
      string outStr = string.Empty;
      if (!string.IsNullOrEmpty(inStr))
      {
        string[] tokens = inStr.Split(new char[] { ' ' });
        for (int i = 0; i < tokens.GetLength(0); i++)
          if (tokens[i].Length > outStr.Length)
            outStr = tokens[i];
      }

      return outStr;
    }
    public int? MergeCustomer(string fromCustomer, string toCustomer)
    {
      string sql = string.Format("EXEC MergeCustomer '{0}', '{1}'", fromCustomer, toCustomer);
      return updateDataFromSQL(sql);
    }
    public string[] getEntityTypes()
    {
      string sql = "select distinct EntityType from Entity where EntityType not in ('All', 'SB', 'Default', 'Admin')";
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return new string[0];
      ArrayList list = new ArrayList();
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        foreach (DataRow row in ds.Tables[0].Rows)
          list.Add(CommonFunctions.CString(row[0]));
      ds.Clear();
      ds = null;
      return (string[]) list.ToArray(typeof(string));
    }
    public void updateEntityAttribute(string entity, string attribute, object value)
    {
      EntityAttributesCollection eac = _ea.getAttributes(entity, null, null, null, DateTime.Today);
      string entityType = CommonFunctions.CString(eac.getValue(string.Format("{0}.EntityType", entity)));
      eac.setValue(string.Format("{0}.Entity.{1}.{2}",entity, entityType, attribute),value);
      _ea.Save(eac);
    }
    #region group maintenance
    public void createGroup(string group, string Description, string memberType, string user)
    {
      Entity en = new Entity();
      en.ID = group;
      en.Fields.AddField("Entity", group);
      en.Fields.AddField("LegalName", Description);
      en.Fields.AddField("EntityType", "Group");
      en.Fields.AddField("EntityOwner", "CCI");
      en.Fields.AddField("LastModifiedBy", user);
      ItemType it = new ItemType();
      it.ID = "Entity";
      Item i = new Item();
      i.ID = "Group";
      i.AddAttribute("GroupType", "Entity");
      i.AddAttribute("MemberType", memberType);
      i.LastModifiedBy = user;
      it.Items.Add(i);
      en.ItemTypes.Add(it);
      EntityAttributesCollection eac = new EntityAttributesCollection();
      eac.Entities.Add(en);      
      _ea.Save(eac);
    }
    public int? updateGroupMember(string ID, string groupID, string entity, DateTime? startDate, DateTime? endDate, string relationship, string user)
    {
      return updateGroupMember(ID, groupID, entity, startDate, endDate, relationship, user, true);
    }
    public int? updateGroupMember(string ID, string groupID, string entity, string relationship, string user)
    {
      return updateGroupMember(ID, groupID, entity, null, null, relationship, user, false);
    }
    public int? updateGroupMember(string ID, string groupID, string entity, DateTime? startDate, DateTime? endDate, string relationship, string user, bool overwriteDates)
    {
      string sql;
      string updateRelationship;
      if (!string.IsNullOrEmpty(ID) && existsRecord("GroupMembers", new string[] { "ID" }, new string[] { ID }))
      {
        // if the relationship is null ,then don't change it in the database
        updateRelationship = string.IsNullOrEmpty(relationship) ? string.Empty : string.Format(", Relationship = '{0}'", relationship);
        if (overwriteDates)
          sql = @"update GroupMembers set GroupID = '{0}', Entity = '{1}', StartDate = {2}, EndDate = {3}, 
            LastModifiedBy = '{4}', LastModifiedDateTime = '{5}'{6} where ID = {7}";
        else
          sql = @"update GroupMembers set GroupID = '{0}', Entity = '{1}', LastModifiedBy = '{4}', LastModifiedDateTime = '{5}'{6} where ID = {7}";
      }
      else
      {
        // however for an insert, if relationship is not specified we use the default
        updateRelationship = string.IsNullOrEmpty(relationship) ? DEFAULTGROUPRELATIONSHIP : relationship;
        sql = @"insert into GroupMembers (GroupID, Entity, StartDate, EndDate, LastModifiedBy, LastModifiedDateTime, Relationship)
Values ('{0}', '{1}', {2}, {3}, '{4}', '{5}', '{6}')";
      }
      sql = string.Format(sql, groupID, entity, startDate == null ? "null" : string.Format("'{0}'", ((DateTime)startDate).ToShortDateString()),
        endDate == null ? "null" : string.Format("'{0}'", ((DateTime)endDate).ToShortDateString()), user, DateTime.Now.ToString(CommonData.FORMATLONGDATETIME), updateRelationship, ID);
      return updateDataFromSQL(sql);
    }
    public int? moveMember(string fromGroup, string toGroup, string member, string user)
    {
      int? idFrom = getMember(fromGroup, member);
      int? idTo = getMember(toGroup, member);
      if (idFrom == null || idTo == null)
        return -1;
      int? ret = updateGroupMember(((int)idFrom).ToString(), fromGroup, member, null, user);
      if (ret != null && ret >= 0)
        ret = updateGroupMember(((int)idTo).ToString(), toGroup, member, null, user);
      return ret;
    }
    public int? getMember(string group, string member)
    {
      DataSet ds = getGroupMemberInfo(group, member);
      if (ds == null)
        return null;
      int? id = null;
      if (ds.Tables.Count == 1 && ds.Tables[0].Rows.Count == 1)
        id = CommonFunctions.CInt(ds.Tables[0].Rows[0]["ID"]);
      ds.Clear();
      ds = null;
      return id;
    }
    public int? updateMembers(string group, ArrayList members, bool isGroup, bool merge, string user)
    {
      // takes a list and updates the list in GroupMembers so it is correct
      int? ret = null;
      string sql;
      // we loop through the list, and if it is there, we leave it. If not, we add it
      // we also check each token to see if it is in the form <ID: Description>. If so, we pull out the ID
      string g = group;
      int colonPos = g.IndexOf(":");
      if (colonPos > 0)
        g = g.Substring(0, colonPos);
      // sql insert has a max of 1000 records, so we break up the list if necessary
      ArrayList insertMembers = new ArrayList();
      ArrayList list1000 = new ArrayList();
      int iCount = 0;
      foreach (string member in members)
      {
        if (iCount >= 1000)
        {
          insertMembers.Add(list1000);
          list1000 = new ArrayList();
          iCount = 0;
        }
        string m = member;
        colonPos = m.IndexOf(":");
        if (colonPos > 0)
          m = m.Substring(0, colonPos);
        list1000.Add(m);
        iCount++;
      }
      insertMembers.Add(list1000);
      // OK, so this is super slow. So, we try another tack... Create a list of ids,. identify the inserts and terminates,
      // then do a multiple row update and insert
      string memberList = toList(insertMembers);
      const string GETLISTSQL = "select distinct {2} from GroupMembers where {0} = '{1}' and {2} {6} in ({3}) and getdate() between isnull(StartDate, '{4}') and isnull(EndDate, '{5}')";
      ArrayList currentMembers = new ArrayList();
      DataSet ds;
      if (!string.IsNullOrEmpty(memberList))
      {
        // note parm {6} is for NOT so we can find ones that are and ones that are not
        // first, get a list of members that are already in the group
        sql = string.Format(GETLISTSQL,
          isGroup ? "GroupID" : "Entity", group, isGroup ? "Entity" : "GroupID", memberList, CommonData.PastDateTime.ToString(CommonData.FORMATLONGDATETIME),
          CommonData.FutureDateTime.ToString(CommonData.FORMATLONGDATETIME), string.Empty);
        ds = getDataFromSQL(sql);
        if (ds != null)
        {
          if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            foreach (DataRow row in ds.Tables[0].Rows)
            {
              string member = CommonFunctions.CString(row[0]);
              currentMembers.Add(member);
              removeMember(insertMembers,member);
            }
          ds.Clear();
          ds = null;
        }
      }
      string timestamp = DateTime.Now.ToString(CommonData.FORMATLONGDATETIME);
      // now we have a list of members to terminate (if not merge) and members to insert
      if (!merge)
      {
        ArrayList termMembers = new ArrayList();
        sql = string.Format(GETLISTSQL, 
        isGroup ? "GroupID" : "Entity", group, isGroup ? "Entity" : "GroupID",  memberList, CommonData.PastDateTime.ToString(CommonData.FORMATLONGDATETIME),
        CommonData.FutureDateTime.ToString(CommonData.FORMATLONGDATETIME),"NOT");
        ds = getDataFromSQL(sql);
        if (ds != null)
        {
          if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            foreach (DataRow row in ds.Tables[0].Rows)
            {
              string member = CommonFunctions.CString(row[0]);
              termMembers.Add(member);
            }
          ds.Clear();
          ds = null;
        }
        string termMemberList = CommonFunctions.ToList((string[])termMembers.ToArray(typeof(string)), "'");
        if (!string.IsNullOrEmpty(termMemberList))
        {
          // set end date to yesterday
          sql = string.Format("Update GroupMembers set EndDate = '{0}', LastModifiedBy = '{1}', LastModifiedDateTime = '{2}' where {3} = '{4}' and {5} IN ({6})",
            DateTime.Today.AddDays(-1).ToShortDateString(), user, timestamp, isGroup ? "GroupID" : "Entity", g, isGroup ? "Entity" : "GroupID", termMemberList);
          ret = updateDataFromSQL(sql);
        }
      }      
      // and we insert any new members
      foreach (ArrayList list in insertMembers)
      {
        // create an insert statment and execute it for each 1000 in the list
        // SQL has a restriction that limits this to 1000
        if (ret != -1 && insertMembers.Count > 0)
        {
          string dt = DateTime.Today.ToShortDateString();
          StringBuilder insertSQL = new StringBuilder();
          insertSQL.Append("insert into GroupMembers (GroupID, Entity, StartDate, LastModifiedBy, LastModifiedDateTime) Values ");
          foreach (string mem in list)
          {
            string mbr = isGroup ? mem : g;
            string grp = isGroup ? g : mem;
            insertSQL.Append(string.Format("('{0}','{1}','{2}','{3}','{4}'),", grp, mbr, dt, user, timestamp));
          }
          insertSQL.Length--; // take off the last comma
          sql = insertSQL.ToString();
          ret = updateDataFromSQL(sql);
        }
      }
      return ret;
    }
    private void removeMember(ArrayList members, string member)
    {
      // members is an array of arrays, each one 1000 entries or less
      // so we have to look in each list to find the one to remove. then we exit
      foreach (ArrayList list in members)
        if (list.Contains(member))
        {
          list.Remove(members);
          break;
        }
    }
    private string toList(ArrayList members)
    {
      // members is an array of arrays, each one 1000 entries or less
      StringBuilder sb = new StringBuilder();
      foreach (ArrayList list in members)
        foreach (string member in list)
          sb.Append(string.Format("'{0}',",member));
      if (sb.Length > 1)
        sb.Length--; // remove the last comma
      return sb.ToString();
    }
    public bool existsGroupMember(string group, string member)
    {
      string sql = string.Format("select ID from GroupMembers where groupID = '{0}' and entity = '{1}'", group, member);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return false;
      bool exists = false;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        exists = true;
      ds.Clear();
      ds = null;
      return exists;
    }
    public int? terminateGroupMember(string group, string member, string user)
    {
      int? id = getMember(group, member);
      if (id == null || (int)id < 0)
        return id;
      id = updateGroupMember(((int)id).ToString(), group, member, null, DateTime.Today.AddDays(-1), null, user); // terminate as of yesterday
      return id;
    }
    public ArrayList getMembers(string group, string memberType, string entityType)
    {
      return getMembers(group, memberType, entityType, false);
    }
    public ArrayList getMembers(string group, string memberType, string entityType, bool includeGrandChildren)
    {
      ArrayList members = new ArrayList();
      EntityMemberType mType;
      if (string.IsNullOrEmpty(memberType))
        return members;
      try 
      {
        mType  =  (EntityMemberType)Enum.Parse(typeof(EntityMemberType), memberType, true);
      }
      catch
      {
        return members;
      }
      EntityMemberType gType = (mType == EntityMemberType.Group) ? EntityMemberType.Entity : EntityMemberType.Group;

      string sql = string.Empty;
      string groupClause = string.Empty;
      string entityTypeClause = string.Empty;
      if (string.IsNullOrEmpty(group))
      {
        //if (mType == EntityMemberType.Entity) // we do not have a group and we want a list of ALL entities
        //{
          if (string.IsNullOrEmpty(entityType))
            return members;
          else
            sql = string.Format(@"select Entity + ': ' + LegalName from Entity e where EntityType = '{0}' 
and '{1}' between isnull(e.StartDate, '{2}') and isnull(e.EndDate, '{3}') order by LegalName ",
            entityType, DateTime.Today.ToShortDateString(),
          CommonData.PastDateTime.ToShortDateString(), CommonData.FutureDateTime.ToShortDateString());
        //}
      }
      if (string.IsNullOrEmpty(sql))
      {
        /*
         * 6-6-2013 LLA added filter by start/enddates on both member and group entity records.
         * 
         * And... lest we get confused: All of the substitutions are due to the fact that this query 
         * works for both members of a groups and groups of a member (depending on memberType parameter).
         * Also it is made more complicated by the fact that in GroupMembers, the two keys are
         * GroupID and Entity. This is because Group is an SQL reserved workd and it didn't like
         * the columnname Group, so I had to make it GroupID. 
         * 
         * So.. {0}{2} in the first clause and {0}{5} in the sql is either Entity or GroupID.
         * Likewise the {6}{7} in the second entity join. 
         * 
         * Also, instead of using getdate() for current date, and hard coded dates for the 
         * "forever in the past" and the "forever in the future" dates, we use the values
         * from CommonData. This guarantees consisitency.
         * 
         * This makes this query a little hard to read, but basically we display and join to Entity
         * on GroupID and Entity, but we reverse them depending on memberType. 
         */
        if (!string.IsNullOrEmpty(group))
          groupClause = string.Format(" g.{0}{2} = '{1}' and ", gType.ToString(), group, gType == EntityMemberType.Group ? "ID" : "");
        if (!string.IsNullOrEmpty(entityType))
          entityTypeClause = string.Format(" and e.EntityType = '{0}'", entityType);
        if (includeGrandChildren && mType == EntityMemberType.Entity)  // only if looking for members of a group
          sql = string.Format(@"select e.Entity + ': ' + e.LegalName + case when isnull(g.IsPrimary,0) = 1 then ' (Primary)' else '' end Member
            from dbo.fn_GroupChildren('{0}','{1}') c
            inner join Entity e on c.Entity = e.Entity and '{2}' between isnull(e.StartDate, '{3}') and isnull(e.EndDate, '{4}')
            left join GroupMembers g on g.GroupID = '{0}' and g.Entity = c.Entity and '{2}' between isnull(g.StartDate, '{3}') and isnull(g.EndDate, '{4}')
               order by e.LegalName ", group, entityType, 
            DateTime.Today.ToShortDateString(), CommonData.PastDateTime.ToShortDateString(), CommonData.FutureDateTime.ToShortDateString());
        else
          sql = string.Format(@"select e.Entity + ': ' + e.LegalName + case when isnull(g.IsPrimary,0) = 1 then ' (Primary)' else '' end Member
            from GroupMembers g 
            inner join Entity e on g.{0}{5} = e.Entity and '{2}' between isnull(e.StartDate, '{3}') and isnull(e.EndDate, '{4}')
            left join Entity ge on g.{6}{7} = ge.Entity and '{2}' between isnull(ge.StartDate, '{3}') and isnull(ge.EndDate, '{4}')
            where {1} '{2}' between isnull(g.StartDate, '{3}') and isnull(g.EndDate, '{4}') {8}
               order by e.LegalName ",
              mType.ToString(), groupClause, DateTime.Today.ToShortDateString(), 
              CommonData.PastDateTime.ToShortDateString(), CommonData.FutureDateTime.ToShortDateString(),
              mType == EntityMemberType.Group ? "ID" : "", gType.ToString(), gType == EntityMemberType.Group ? "ID" : "",
              entityTypeClause);
      }
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return members;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        foreach (DataRow row in ds.Tables[0].Rows)
        {
          string entry = CommonFunctions.CString(row[0]);
          if (!string.IsNullOrEmpty(entry))
            members.Add(entry);
        }
      ds.Clear();
      ds = null;
      return members;
    }
    public int? memberSetPrimary(string groupID, string entity, bool isPrimary, string user)
    {
      string sql = string.Format(
@"Update GroupMembers set IsPrimary = {2}, LastModifiedBy = '{3}', LastModifiedDateTime = '{4}' 
where GroupID = '{0}' and Entity = '{1}'", groupID, entity, isPrimary ? "1" : "0", user, DateTime.Now.ToString(CommonData.FORMATLONGDATETIME));
      int? ret = updateDataFromSQL(sql);
      if (ret != -1)
      {
        EntityAttributesCollection eac = getEntity(entity, null, DateTime.Today);
        string entityType = CommonFunctions.CString(eac.getValue(string.Format("{0}.EntityType",entity)));
        eac.setValue(string.Format("{0}.Entity.{1}.ContactType", entity, entityType), isPrimary ? "Primary" : null);
        _ea.Save(eac);
        ret = null;
      }
      return ret;
    }
    public DataSet getGroupMemberInfo(string group, string member)
    {
      string sql = "select * from GroupMembers where groupid = '{0}' and entity = '{1}' and '{2}' between isnull(StartDate, '{3}') and isnull(EndDate, '{4}')";
      sql = string.Format(sql, group, member, DateTime.Today.ToShortDateString(), CommonData.PastDateTime.ToShortDateString(), CommonData.FutureDateTime.ToShortDateString());
      return getDataFromSQL(sql);

    }
    public string getGroupMemberRelationship(string group, string member)
    {
      using (DataSet ds = getGroupMemberInfo(group, member))
      {
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 1)
          return CommonFunctions.CString(ds.Tables[0].Rows[0]["Relationship"]);
      }
      return string.Empty;
    }
    #endregion
  }
}
