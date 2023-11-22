using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using CCI.Common;
using CCI.Sys.SecurityEngine;

namespace CCI.Sys.Data
{
  public partial class DataSource
  {
    public List<ProcessStepsModel> getProcessStepsList(string cycle)
    {
      if (string.IsNullOrWhiteSpace(cycle))
        return new List<ProcessStepsModel>();
      string sql = "Select * from ProcessSteps where cycle = '" + cycle + "'";
      DataSet ds = getDataFromSQL(sql);
      //DataSet ds = getProcessSteps(cycle, billDate);
      return getListFromDataset<ProcessStepsModel>(ds);
    }
    public DataSet getProcessSteps(string cycle, DateTime billDate)
    {
      return getProcessSteps(cycle, billDate, false);
      
    }
    public List<BillCycleModel> getThisBillCycle( DateTime billCycleDate)
    {
      string sql = "SELECT * FROM BillingCycleDetail WHERE BillingDate = '" + billCycleDate.ToShortDateString() +  "'";
      DataSet ds = getDataFromSQL(sql);
      return getListFromDataset<BillCycleModel>(ds);
    }

    public DataSet getProcessSteps(string cycle, DateTime billDate, bool testMode)
    {
      string cycleFilter = string.Format("(Cycle = '{0}'{1})", cycle, testMode ? string.Format(" OR Cycle = '{0}x'",cycle) : string.Empty);
      string sql = string.Format(@"select c.BillingDate, s.Sequence, s.Step, s.Description, s.Form, s.FormParameters, c.ProcessedBy, max(c.ProcessedDateTime)  ProcessedDateTime
from processSteps s 
left join HostedProcessCycle c on s.Step = c.Step and c.BillingDate = '{0}' and c.UnprocessedDateTime is null
where {1}
group by c.BillingDate, s.Sequence, s.Step, s.Description, s.Form, s.FormParameters, c.ProcessedBy
order by Sequence", billDate.ToShortDateString(), cycleFilter);
      return getDataFromSQL(sql);
    }

    #region Process Step Log

    public int? processStep(string step, DateTime billDate, bool processed, string user)
    {
      string tableName = "HostedProcessCycle";
      string sDateTime = DateTime.Now.ToString(CommonData.FORMATLONGDATETIME);
      string sBillDate = billDate.ToShortDateString();
      string sql = string.Empty;
      if (existsProcessStep(step, sBillDate))
      {
        if (processed)
          sql = string.Format(@"update {0} set ProcessedBy = '{1}', ProcessedDateTime = '{2}', LastModifiedBy = '{1}', LastModifiedDateTime = '{2}'
            WHERE  Step = '{3}' AND BillingDate = '{4}'",
            tableName, user, sDateTime,null, step, sBillDate);
        else
          sql = string.Format("Update {0} set UnprocessedDateTime = getdate() where Step = '{1}' and BillingDate = '{2}'", tableName, step, sBillDate);
          //sql = string.Format("delete from {0} where Step = '{2}' and BillingDate = '{3}'", tableName, cycle, step, sBillDate);
      }
      else
        if (processed)
          sql = string.Format(@"insert into {0} (Step, BillingDate, ProcessedBy, ProcessedDateTime, LastModifiedBy, LastModifiedDateTime)
            Values ('{1}', '{2}', '{3}', '{4}', '{3}', '{4}')", tableName, step, sBillDate, user, sDateTime);
      if (!string.IsNullOrEmpty(sql))
        return updateDataFromSQL(sql);
      else
        return null;
    }
    public bool existsProcessStep(string step, string sBillDate)
    {
      bool ret = false;
      string sql = string.Format("Select Id from HostedProcessCycle where step = '{0}' and billingdate = '{1}' and UnprocessedDateTime is null", step, sBillDate);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return ret;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        ret = true;
      ds.Clear();
      ds = null;
      return ret;

    }
    public Exception insertHostedProcessStep(string User, string Step, DateTime BillDate, string ImportType, string ImportName)
    {
      Exception returnmsg;
      string LogDateTime = Convert.ToString(DateTime.Now);
      string sql = string.Format(@"Insert into HostedProcessCycle (
      BillingDate
      ,Step
      ,SubStep
      ,ProcessedBy
      ,ProcessedDateTime
      ,LastModifiedDateTime
      ,LastModifiedBy
      ,FileType
      ,FileName) Values ('{0}', 'Import', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')", BillDate, Step, User, LogDateTime, LogDateTime, User, ImportType, ImportName);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      return returnmsg;
    }
    public bool checkProcessStepsbeforeimport(DateTime billdate, string step, string substep)
    {
      string subStepClause = string.Empty;
      if (!string.IsNullOrEmpty(substep))
        subStepClause = string.Format("and filetype = '{0}'", substep);
      string sql = string.Format(@"select top 1 id from hostedprocesscycle 
        where billingdate = '{0}' and step = 'Import' and substep = '{1}' {2} and UnProcessedDateTime is null", billdate, step, subStepClause);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
      {
        return false;
      }
      else
      {
        return true;
      }
    }
    public bool checkforUnpost(DateTime billdate, string step, string substep)
    {
      string subStepClause = string.Empty;
      if (!string.IsNullOrEmpty(substep))
        subStepClause = string.Format("and filetype = '{0}'", substep);
      string sql = string.Format(@"select top 1 id from hostedprocesscycle 
        where billingdate = '{0}' and step = 'Import' and substep = '{1}' {2} and UnProcessedDateTime is not null", billdate, step, subStepClause);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
      {
        return false;
      }
      else
      {
        return true;
      }
    }
    public Exception unProcessStep(string step, DateTime billdate)
    {
      // LLA change from delete to flag as unprocessed
      string sql = string.Format(@"update HostedProcessCycle Set UnprocessedDateTime = getdate() where BillingDate = '{0}' and Step = 'Import' and substep = '{1}' and UnProcessedDateTime is null", billdate, step);
      //sql = string.Format(@"delete from hostedprocesscycle where billingdate = '{0}'", billdate);
      return updateDataFromSQLReturnErrorDescription(sql);
    }
    public DateTime getCurrentBillingMonth()
    {
      DateTime dt = DateTime.Today;
      dt = dt.AddMonths(-1);
      dt = dt.AddDays(-dt.Day + 1);
      string sql = "select max(billingdate) from HostedProcessCycle";
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return dt;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        dt = CommonFunctions.CDateTime(ds.Tables[0].Rows[0][0]);
      ds.Clear();
      ds = null;
      return dt;
    }
    #endregion

    #region AcctImportLog
    public Exception insertAcctImportLog(string User, string ImportType, string ImportName, DateTime BillDate, string source = null)
    {
      Exception returnmsg;
      string LogDateTime = DateTime.Now.ToString(CommonData.FORMATLONGDATETIME);
      string sql = string.Format(@"Insert into AcctImportsLog ([User]
      ,[DateTime]
      ,[BillingDate]
      ,[FileType]
      ,[File]
      ,[Source]) Values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", User, LogDateTime, BillDate, ImportType, ImportName, source);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      return returnmsg;
    }
    public DataSet getAcctImportLog()
    {
      string sql = "select DateTime,BillingDate,Source,Filetype,[User],[File] from acctimportslog order by Datetime desc";
      return getDataFromSQL(sql);
    }
    public string selectBillDatefromLog(string Type)
    {
      string nodate = "";
      string sql = string.Format("Select Top 1 [BillingDate] from AcctImportsLog where FileType = '{0}' order by [BillingDate] desc", Type);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return nodate;
      return CommonFunctions.CString(ds.Tables[0].Rows[0][0]);
    }
    public Exception updateAcctImportsLog(string user, DateTime billdate, string filetype, string source=null)
    {
      Exception returnmsg;
      string sql;
      sql = string.Format(@"insert into acctimportslog ([user],[datetime],billingdate,filetype,source)
                             values ('{0}',getdate(),'{1}','{2}','{3}')", user, billdate, filetype, source);
      returnmsg = updateDataFromSQLReturnErrorDescription(string.Format(sql));
      return returnmsg;
    }
    #endregion

  }
}
