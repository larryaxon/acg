using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ACG.Common.Data
{
  public class SecurityDB : DataAccessBase
  {
    #region module data
    private string p_tblSecurityAssignments = "SecurityAssignments";
    private string p_tblSecurityGroups = "SecurityGroups";
    private string p_tblSecurityObjectGroups = "SecurityObjectGroups";
    private string p_tblSecurityObjects = "SecurityObjects";
    private string p_tblSecurityRights = "SecurityRights";
    private string p_tblSecurityUsers = "SecurityUsers";
    private string p_spLogin = "Login";
    private string p_spGroups = "getSecurityGroups";
    private string p_spObjectGroups = "getSecurityObjectGroups";
    private string p_spObjectGroupChildren = "getSecurityObjectGroupChildren";
    private string p_spGroupChildren = "getSecurityGroupChildren";
    private string p_spObjectMemberParents = "getSecurityObjectMemberParents";
    private string p_spMemberParents = "getSecurityMemberParents";
    private string p_spRights = "getSecurityRights";
    private string p_spAssignments = "getSecurityAssignments";
    private string p_spObjectType = "getSecurityObjectType";
    private string p_tblGroups = "SecurityGroups";
    private string p_tblObjectGroups = "SecurityObjectGroups";
    #endregion module data

    #region public properties
    public string tblGroups
    {
      get { return p_tblGroups; }
      //TODO: set { p_tblGroups = value; }
    }

    public string tblObjectGroups
    {
      get { return p_tblObjectGroups; }
      //set { p_tblObjectGroups = value; }
    }
    #endregion public properties

    #region public methods

    public DataSet getUser(string LoginID, string Domain)
    {
        DataSet datasetReturn = new DataSet();
        SqlCommand command = createCommand();
        SqlParameter login = new SqlParameter("LoginID", SqlDbType.VarChar);
        // Assign stored procedure parameter values
        if (LoginID == null || LoginID == string.Empty)
            return datasetReturn;
        else
          login.Value = LoginID;
        SqlParameter domain = new SqlParameter("Domain", SqlDbType.VarChar);
        // Assign stored procedure parameter values
        if (Domain == null || Domain == string.Empty)
          return datasetReturn;
        else
          domain.Value = Domain;
        // Construct the stored procedure command instance
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = p_spLogin;
        command.Parameters.Add(login);
        command.Parameters.Add(domain);

        // Execute the command and return the values in a DataSet instance
        datasetReturn.Load(command.ExecuteReader(), LoadOption.PreserveChanges, tables);
        sqlConnection.Close();

        return datasetReturn;

    }
    public void SavePassword(string login, string domain, string password)
    {
      // public void updateRecord(string[] fieldList, string[] valueList, string[] keyList, string tableName)
      string[] fieldList = { "Login", "Domain", "Password" };
      string[] valueList = new string[3];
      valueList[0] = "'" +  login + "'";
      valueList[1] = "'" + domain + "'";
      valueList[2] = password;
      string[] keyList = { "Login", "Domain" };
      string tablename = "SecurityUsers";
      base.updateRecord(fieldList, valueList, keyList, tablename);
    }
    public DataSet getGroups()
    {
      return getDataSetUsingOneID(null, "Member", p_spGroups);
    }
    public DataSet getResourceList(string domain)
    {
      string sql = "Select Distinct Entity as Resource, 'User' as ResourceType FROM SecurityUsers where Domain = '" + domain;
      sql += "' UNION Select Distinct SecurityGroup as Resource, 'Group' as ResourceType from SecurityGroups";
      return getDataFromSQL(sql);
    }
    public DataSet getGroups(string Member)
    {
      return getDataSetUsingOneID(Member, "Member", p_spGroups);
    }
    public bool isGroup(string groupID)
    {
      string mySQL = "Select distinct SecurityGroup from SecurityGroups where SecurityGroup = '" +
        groupID + "';";
      DataSet dsGroup = getDataFromSQL(mySQL);
      if (dsGroup.Tables.Count > 0 && dsGroup.Tables[0].Rows.Count > 0)
        return true;
      else
        return false;
    }
    public DataSet getGroupList()
    {
      string mySQL = "Select distinct SecurityGroup from SecurityGroups;";
      return getDataFromSQL(mySQL);
    }

    public DataSet getObjectType(string SecurityObject)
    {
      return getDataSetUsingOneID(SecurityObject, "SecurityObject", p_spObjectType);
    }
    public DataSet getAllObjectTypes()
    {
      string sql = "Select * from SecurityObjects";
      return getDataFromSQL(sql);
    }
    public DataSet getObjectGroups()
    {
      return getDataSetUsingOneID(null, "Member", p_spObjectGroups);
    }

    public DataSet getObjectGroups(string Member)
    {
      return getDataSetUsingOneID(Member, "Member", p_spObjectGroups);
    }

    public DataSet getGroupChildren(string Group)
    {
      return getDataSetUsingOneID(Group, "Group", p_spGroupChildren);
    }

    public DataSet getObjectGroupChildren(string Group)
    {
      return getDataSetUsingOneID(Group, "Group", p_spObjectGroupChildren);
    }

    public DataSet getMemberParents(string Member)
    {
      return getDataSetUsingOneID(Member, "Member", p_spMemberParents);
    }

    public DataSet getObjectMemberParents(string Member)
    {
      return getDataSetUsingOneID(Member, "Member", p_spObjectMemberParents);
    }

    public DataSet getAssignments(string SecurityGroup)
    {
      return getDataSetUsingOneID(SecurityGroup, "SecurityGroup", p_spAssignments);
    }

    public DataSet getRights(string SecurityRight)
    {
      return getDataSetUsingOneID(SecurityRight, "SecurityRight", p_spRights);
    }
    #endregion public methods
  }
}
