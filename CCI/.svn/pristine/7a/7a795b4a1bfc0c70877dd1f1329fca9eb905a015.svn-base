using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using CCI.Common;

namespace CCI.Sys.Data
{
  public partial class DataSource
  {
    private const string DISPOSITIONSADDLEBACK = "Assign to Saddleback";
    private const string DISPOSITIONCITY = "Assign to City";
    private const string DISPOSITIONEXCEPTION = "Mark as Permanent Exception";
    private const string DISPOSITIONONETIMEEXCEPTION = "Exception This Time Only";
    private string[] _dispositionList = new string[] { DISPOSITIONSADDLEBACK, DISPOSITIONCITY };
    private string[] followUpFields = new string[] { "ID"
      ,"AssignedTo"
      ,"CustomerID"
      ,"OrderID"
      ,"Description"
      ,"Notes"
      ,"AsOfDate"
      ,"DueDate"
      ,"CreateDateTime"
      ,"CompleteDateTime"
      ,"DispositionCode"
      ,"Source"
      ,"LastModifiedBy"
      ,"LastModifiedDateTime"};
    private const string followUpTableName = "FollowUps";    
    public DataSet getFollowUp(int id)
    {
      string sql = "select * from FollowUps where id = " + id.ToString();
      return getDataFromSQL(sql);
    }
    public int? updateFollowUp(string[] fieldValues)
    {
      // bail if the structure of the data is bad

      int? retVal = null;
      if (followUpFields == null || fieldValues == null || followUpFields.Length == 0 || followUpFields.Length != fieldValues.Length 
          || !followUpFields[0].Equals("ID", StringComparison.CurrentCultureIgnoreCase))
        return retVal;
      string[] keyList = new string[] { "ID" };
      // assumes ID is the first field
      string id = fieldValues[0];
      if (!string.IsNullOrEmpty(id) && existsRecord(followUpTableName, keyList, new string[] { id }))
      {
        updateRecord(followUpFields, fieldValues, keyList, followUpTableName);
        retVal = CommonFunctions.CInt(fieldValues[0]); // return the id
      }
      else
      {
        // strip off ID for insert
        string[] fields = new string[followUpFields.GetLength(0) - 1];
        for (int i = 0; i < fields.GetLength(0); i++) fields[i] = followUpFields[i + 1];
        string[] vals = new string[fieldValues.GetLength(0) - 1];
        for (int i = 0; i < vals.GetLength(0); i++) vals[i] = fieldValues[i + 1];
        retVal = insertRecord(followUpTableName, fields, vals);
      }
      return retVal;
    }
    public void deleteFollowUp(int id)
    {
      string sql = "delete from FollowUps where id = " + id.ToString();
      getDataFromSQL(sql);
    }
    public int? createException(string exceptionScreen, string exceptionType, bool isOneTime, string customerid, DateTime billDate,
      string retailUSOC, string wholesaleUSOC, string comments, int? followUpID, string user)
    {
      string sql = string.Format(@"insert into ExceptionList (ExceptionScreen, ExceptionType, IsOneTime, CustomerID, BillDate,
        WholesaleUSOC, RetailUSOC, Comment, CreatedBy, CreatedDateTime, FollowUpID)
        Values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}', {10})",
         exceptionScreen, exceptionType, isOneTime ? "1" : "0", customerid, billDate.ToShortDateString(),
         wholesaleUSOC, retailUSOC, CommonFunctions.fixUpStringForSQL(comments), user, DateTime.Now.ToString(CommonData.FORMATLONGDATETIME),
         followUpID == null ? "null" : ((int)followUpID).ToString());
      return updateDataFromSQL(sql);
    }
    public string executeUndoException(CommonData.UnmatchedNameTypes screen, string disposition, string customerID, string description,
      string exception, DateTime billDate, string retailUSOC, string wholesaleUSOC, string user, string comment)
    {
      string errormsg = string.Empty;
      string sql = string.Format(@"delete from followups where id = 
                                      (select followupid 
                                          from exceptionlist x inner join
                                               followups f on f.ID = x.FollowUpID
                                          where x.followupid is not null and 
                                                x.CustomerID = '{2}' and x.ExceptionScreen = '{0}' and (x.WholesaleUSOC = '{4}' OR X.WholesaleUSOC = '') and
                                                x.RetailUSOC = '{5}' and x.ExceptionType = '{1}' and x.BillDate = '{3}')",
         screen.ToString(), exception, customerID, billDate.ToShortDateString(),
         wholesaleUSOC, retailUSOC);
      int ? returnid = updateDataFromSQL(sql);
      if (returnid == null)
      {
        sql = string.Format(@"delete from ExceptionList 
                                    where ExceptionScreen = '{0}' and ExceptionType = '{1}' and CustomerID = '{2}' and
                                          BillDate = '{3}' and (WholesaleUSOC = '{4}' or WholesaleUSOC = '') and RetailUSOC = '{5}'",
           screen.ToString(), exception, customerID, billDate.ToShortDateString(),
           wholesaleUSOC, retailUSOC);
        returnid = updateDataFromSQL(sql);
        if (returnid != null)
          errormsg = "Error deleteing Exception";
      }
      else
      {
        errormsg = "Error deleteing FLUP";
      }
      return errormsg;
    }
    public string executeDisposition(CommonData.UnmatchedNameTypes screen, string disposition, string customerID, string description,
      string exception, DateTime billDate, string retailUSOC, string wholesaleUSOC, string user, string comment)
    {
      string errorMsg = null;
      string assignedTo = string.Empty;
      int? flupID = null;
      bool doFollowUp = false;
      bool logException = false;
      bool oneTime = false;
      switch (disposition)
      {
        case DISPOSITIONSADDLEBACK:
          assignedTo = "Saddleback-User";
          doFollowUp = true;
          break;
        case DISPOSITIONEXCEPTION:
          logException = true;
          oneTime = false;
          break;
        case DISPOSITIONONETIMEEXCEPTION:
          logException = true;
          oneTime = true;
          break;
        default:
          if (disposition.IndexOf(DISPOSITIONCITY, StringComparison.CurrentCultureIgnoreCase) >= 0)
          {
            CommonFunctions.getFunctionName(disposition, out assignedTo); // pull assigned to user id out of the parens in the description
            doFollowUp = true;
          }
          break;
      }
      if (doFollowUp)
      {
        string[] followupFieldValues = new string[] { 
            null, // follow up id
            assignedTo, 
            customerID,
            null, // order id
            description, // Description
            comment, // notes
            billDate.ToShortDateString(), // as of date
            DateTime.Today.AddDays(7).ToShortDateString(), // due date
            DateTime.Now.ToString(CommonData.FORMATLONGDATETIME), // createdatetime
            null, // completedatetime
            exception, // disposition code
            screen.ToString(), // source
            user, // last modified by
            DateTime.Now.ToString(CommonData.FORMATLONGDATETIME) // lastmodifieddatetime
          };
        flupID = updateFollowUp(followupFieldValues);
        logException = true;
        oneTime = true;
      }
      if (logException)
      {
        int? returnID = createException( 
          screen.ToString(), // ExceptionScreen
          exception,
          oneTime,
          customerID,
          billDate,
          retailUSOC,
          wholesaleUSOC,
          comment,
          flupID,
          user);
      }
      else
        errorMsg = "That Disposition is not supported";
      return errorMsg;
    }

    public string[] getDispositionList()
    {
      PickListEntries userList = getNewEntityList("User", "Internal");
      ArrayList dispositions = new ArrayList();
      dispositions.Add(DISPOSITIONONETIMEEXCEPTION);
      dispositions.Add(DISPOSITIONEXCEPTION);
      dispositions.Add(DISPOSITIONSADDLEBACK);
      foreach (PickListEntry user in userList)
        if (user.ID.IndexOf("Saddleback", StringComparison.CurrentCultureIgnoreCase) < 0) // don't put saddleback user in
          dispositions.Add(string.Format("{0}: {1}({2})", DISPOSITIONCITY, user.Description, user.ID));
      return (string[])dispositions.ToArray(typeof(string));
    }
    
  }
}
