﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using ACG.App.Common;

namespace ACG.Sys.Data
{
  public partial class DataSource
  {
    public ACGTable getTimeData(int timeID)
    {
      if (timeID == null)
        return new ACGTable();
      string sql = string.Format("Select * from TimeEntry where ID = {0}", timeID.ToString());
      return CommonFunctions.convertDataSetToCCITable(getDataFromSQL(sql));
    }
    public decimal getTimeRate(string customerid, string projectid, string resourceid, DateTime effectiveDate)
    {
      decimal rate = 0;
      string sql = @"select top 1 r.rate from rates r 
where resourceid in ('All', '{0}') and Customerid in ('All', '{1}') and ProjectID in ('All', '{2}') 
and '{3}' between isnull(startdate, '{4}') and isnull(enddate, '{5}')
order by case when ResourceID = 'All' then 1 else 0 end,
case when CustomerID = 'All' then 1 else 0 end,
case when ProjectID = 'All' then 1 else 0 end";
      sql = string.Format(sql, resourceid, customerid, projectid, effectiveDate.ToShortDateString(),
        CommonData.PastDateTime.ToShortDateString(), CommonData.FutureDateTime.ToShortDateString());
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return rate;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        rate = CommonFunctions.CDecimal(ds.Tables[0].Rows[0][0]);
      ds.Clear();
      ds = null;
      return rate;
    }
    public DataSet getTimeSummary(string resourceid, string customerid, 
      bool selectByResource, bool selectByCustomer, bool billableOnly, 
      DateTime startDate, DateTime endDate, CommonData.TimeEntrySummaryMode mode, bool includeSubProject,
      bool includeOnlyUnpaidTime)
    {
      string whereClause = getTimeWhereClause(resourceid, customerid, string.Empty, selectByResource, selectByCustomer, false, billableOnly, startDate, endDate,
        includeSubProject, includeOnlyUnpaidTime, false, false, false);
      string listResource = selectByResource ? string.Empty : "ResourceID, ";
      string listSubProject = includeSubProject ? "s.Name SubProject, " : string.Empty;
      string groupBySubProject = includeSubProject ? ", s.Name" : string.Empty;
      string sql = string.Empty;
      switch (mode)
      {
        case CommonData.TimeEntrySummaryMode.Daily:
          sql = @"select {0} timeworkeddate Date, sum(Convert(decimal(18, 2), EnteredTime)) / 60  Hours from timeentry t
    {1} group by {0} timeworkeddate order by timeworkeddate";
          break;
        case CommonData.TimeEntrySummaryMode.Weekly:
          sql = @"select {0} datepart(year, timeworkeddate) Year, datepart(week, timeworkeddate) Week, sum(Convert(decimal(18, 2), EnteredTime)) / 60  Hours from timeentry t
    {1} group by {0} datepart(year, timeworkeddate), datepart(week, timeworkeddate) 
    order by datepart(year, timeworkeddate), datepart(week, timeworkeddate) ";
          break;
        case CommonData.TimeEntrySummaryMode.ByProject:
          sql = @"select {0} c.name Customer, p.Name Project, {2} sum(Convert(decimal(18, 2), EnteredTime)) / 60  Hours 
    from timeentry t
    inner join Customers c on t.customerid = c.customerid
    inner join Projects p on t.customerid = p.customerid and t.projectid = p.projectid
    left join SubProjects s on t.customerid = s.customerid and t.projectid = s.projectid and t.subprojectid = s.subprojectid
    {1} group by {0} c.Name, p.Name{3} 
    order by c.Name, p.Name ";
          break;
      }
      if (string.IsNullOrEmpty(sql))
        return new DataSet();
      sql = string.Format(sql, listResource, whereClause, listSubProject, groupBySubProject);
      return getDataFromSQL(sql);
    }
    /// <summary>
    /// posts (or unposts) an invoice
    /// </summary>
    /// <param name="resourceid"></param>
    /// <param name="customerid"></param>
    /// <param name="projectid"></param>
    /// <param name="invoiceOnlyThisResource"></param>
    /// <param name="invoiceOnlyThisCustomer"></param>
    /// <param name="invoiceOnlyThisProject"></param>
    /// <param name="fromDate"></param>
    /// <param name="invoiceDate"></param>
    /// <param name="post">Posts if true, unposts if false</param>
    /// <param name="invoiceNumber"></param>
    public void postTimeInvoice(string resourceid, string customerid, string projectid,
      bool invoiceOnlyThisResource, bool invoiceOnlyThisCustomer, bool invoiceOnlyThisProject,
      DateTime fromDate, DateTime invoiceDate, bool post, int invoiceNumber)
    {
      string whereClause = getTimeWhereClause(resourceid, customerid, projectid, invoiceOnlyThisResource, invoiceOnlyThisCustomer, invoiceOnlyThisProject,
        true, fromDate, invoiceDate, false, false, true, post, true);
      string sql;
      // TODO: this will not update properly with multiple customers
      if (post) // post the invoice
        sql = "UPDATE timeentry set InvoiceNumber = {1}, InvoiceDate = '{2}' {0}";
      else
        sql = "UPDATE timeentry SET InvoiceNumber = null, InvoiceDate = null {0}";
      sql = string.Format(sql, whereClause, invoiceNumber.ToString(), invoiceDate.ToShortDateString());
      updateDataFromSQL(sql);
      if (post)
      {
        sql = string.Format("update systemsettings set LastInvoiceNumber = {0}, LastInvoiceDate = '{1}'", invoiceNumber.ToString(), invoiceDate.ToShortDateString());
        updateDataFromSQL(sql);
        sql = string.Format("update customers set LastInvoiceNumber = {0} {1}", invoiceNumber.ToString(),
            invoiceOnlyThisCustomer ? string.Format(" WHERE CustomerID = '{0}'", customerid) : string.Empty);
        updateDataFromSQL(sql);
      }
      else
      {
        sql = @"update systemsettings
                set LastInvoiceNumber = t.LastNumber,
                    LastInvoiceDate = t.LastDate
                from systemsettings inner join (select max(InvoiceDate) LastDate, max(InvoiceNumber) LastNumber from timeentry) t on 1 = 1";
        updateDataFromSQL(sql);
        sql = string.Format(@"update customers
	              set LastInvoiceNumber = t.LastNumber
		              from customers c inner join (
              select customerid, max(InvoiceDate) LastDate, max(InvoiceNumber) LastNumber from timeentry where InvoiceNumber is not null group by CustomerID) t
              on c.customerid = t.customerid where c.CustomerID = '{0}'", customerid);
        updateDataFromSQL(sql);
      }
    }
    public int getLastInvoiceNumber()
    {
      string sql = "Select LastInvoiceNumber from SystemSettings";
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return -1;
      int lastInvoice = -1;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        lastInvoice = CommonFunctions.CInt(ds.Tables[0].Rows[0][0]);
      ds.Clear();
      ds = null;
      return lastInvoice;
    }
    /// <summary>
    /// Returns the timeId
    /// </summary>
    /// <param name="fields"></param>
    /// <returns></returns>
    public int updateTimeEntry(Hashtable fields)
    {
      string sql = string.Empty;
      int id = fields.ContainsKey(CommonData.fieldTIMEID) ? CommonFunctions.CInt(fields[CommonData.fieldTIMEID], -1) : -1;

      if (id == -1) // new record
      {
        StringBuilder fieldList = new StringBuilder();
        StringBuilder valueList = new StringBuilder();
        foreach (DictionaryEntry entry in fields)
        {
          string fldName = CommonFunctions.CString(entry.Key);
          if (!fldName.Equals("id", StringComparison.CurrentCultureIgnoreCase))
          {
            if (fieldList.Length > 0)
              fieldList.Append(", ");
            if (valueList.Length > 0)
              valueList.Append(", ");
            fieldList.Append(fldName);
            valueList.Append("'");
            if (fldName.Equals("createdatetime"))
              valueList.Append(DateTime.Now.ToString(CommonData.FORMATLONGDATETIME));
            else
            {
              if (CommonFunctions.IsDateTime(entry.Value))
                valueList.Append(CommonFunctions.CDateTime(entry.Value).ToString(CommonData.FORMATLONGDATETIME));
              else
                valueList.Append(CommonFunctions.CString(entry.Value).Replace("'", "''"));
            }
            valueList.Append("'");
          }
        }
        if (!fields.ContainsKey(CommonData.fieldCREATEDDATETIME.ToLower()))
        {
          fieldList.Append(", ");
          fieldList.Append(CommonData.fieldCREATEDDATETIME);
          valueList.Append(", '");
          valueList.Append(DateTime.Now.ToString(CommonData.FORMATLONGDATETIME));
          valueList.Append("'");
        }

        sql = string.Format("insert into TimeEntry ({0}) Values ({1})", fieldList.ToString(), valueList.ToString());
      }
      else // updaterecord  
      {
        StringBuilder updateSQL = new StringBuilder();
        string updateStart = "Update TimeEntry Set ";
        updateSQL.Append(updateStart);
        foreach (DictionaryEntry entry in fields)
        {
          string fldName = CommonFunctions.CString(entry.Key);
          if (!fldName.Equals("id", StringComparison.CurrentCultureIgnoreCase))
          {
            if (updateSQL.Length > updateStart.Length)
              updateSQL.Append(", ");
            updateSQL.Append(string.Format("{0} = '{1}'", fldName, CommonFunctions.CString(entry.Value)));
          }
        }
        updateSQL.Append(string.Format(" WHERE ID = {0}", id.ToString()));
        sql = updateSQL.ToString();
      }
      int? ret = updateDataFromSQL(sql);
      if (ret == null)
        return -1;
      else
        return (int)ret;
      
    }
    public int updateProjectBudget(Hashtable fields)
    {
      string sql = string.Empty;
      int id = fields.ContainsKey(CommonData.fieldTIMEID) ? CommonFunctions.CInt(fields[CommonData.fieldTIMEID], -1) : -1;

      if (id == -1) // new record
      {
        // not yet supported
      }
      else
      {
        StringBuilder updateSQL = new StringBuilder();
        string updateStart = "Update ProjectBudget Set ";
        updateSQL.Append(updateStart);
        foreach (DictionaryEntry entry in fields)
        {
          string fldName = CommonFunctions.CString(entry.Key);
          if (!fldName.Equals("id", StringComparison.CurrentCultureIgnoreCase))
          {
            if (updateSQL.Length > updateStart.Length)
              updateSQL.Append(", ");
            updateSQL.Append(string.Format("{0} = '{1}'", fldName, CommonFunctions.CString(entry.Value)));
          }
        }
        updateSQL.Append(string.Format(" WHERE ID = {0}", id.ToString()));
        sql = updateSQL.ToString();
      }
      int? ret = updateDataFromSQL(sql);
      if (ret == null)
        return -1;
      else
        return (int)ret;
    }
    public void deleteTimeEntry(int id)
    {
      if (id > 0)
      {
        string sql = string.Format("Delete from TimeEntry where ID = {0}", id.ToString());
        updateDataFromSQL(sql);
      }
    }
    public void updateEstimatedHoursToComplete(string resourceid, string customerid, string projectid, string subprojectid, decimal hours, string user)
    {
      string sql;
      if (existsRecord("ProjectBudget", new string[] { "CustomerID", "ProjectId", "SubprojectID","ResourceId" }, new string[] { customerid, projectid, subprojectid, resourceid }))
        sql = @"update ProjectBudget set DeveloperEstimatedHours = {4}, LastModifiedBy = '{5}', LastModifiedDateTime = getdate()
          where customerid = '{0}' and projectid = '{1}' and subprojectid = '{2}' and resourceid = '{3}'";
      else
        sql = @"insert into ProjectBudget (CustomerID, ProjectId, SubprojectID, ResourceId, DeveloperEstimatedHours, LastModifiedBy, LastModifiedDateTime)
            Values ('{0}', '{1}', '{2}', '{3}', {4}, '{5}', getdate())";
      sql = string.Format(sql, customerid, projectid, subprojectid, resourceid, hours.ToString(), user);
      updateDataFromSQL(sql);
    }
    public decimal getEstimatedHoursToComplete(string resourceid, string customerid, string projectid, string subprojectid)
    {
      string sql = "Select DeveloperEstimatedHours from ProjectBudget where customerid = '{0}' and projectid = '{1}' and subprojectid = '{2}' and resourceid = '{3}'";
      sql = string.Format(sql, customerid, projectid, subprojectid, resourceid);
      DataSet ds = getDataFromSQL(sql);
      decimal estimatedHours = (decimal)0;
      if (ds == null)
        return estimatedHours;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        estimatedHours = CommonFunctions.CDecimal(ds.Tables[0].Rows[0]["DeveloperEstimatedHours"]);
      ds.Clear();
      ds = null;
      return estimatedHours;
    }
    public string getCustomerName(string customerid)
    {
      string sql = string.Format("select Name from Customers where CustomerID = '{0}'", customerid);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return string.Empty;
      string name = string.Empty;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        name = CommonFunctions.CString(ds.Tables[0].Rows[0]["Name"]);
      ds.Clear();
      ds = null;
      return name;
    }
    public DateTime? getThroughDate(string customerid)
    {
      string sql = string.Format("select LastWorkDate from Customers where CustomerID = '{0}'", customerid);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return null;
      DateTime? throughDate = null;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        throughDate = CommonFunctions.CDateTime(ds.Tables[0].Rows[0]["LastWorkDate"]);
      ds.Clear();
      ds = null;
      return throughDate;
    }
    public void setThroughDate(string customerid, DateTime throughDate)
    {
      string sql = string.Format("update Customers set LastWorkDate = '{0}' where customerid = '{1}'", throughDate, customerid);
      updateDataFromSQL(sql);
    }
    public void flagBudget(string customerid, DateTime throughDate)
    {
      string sql = string.Format(
        @"update timeentry
          set lastbudgetprintdate = '{1}'
          where customerid = '{0}' and timeworkeddate <= '{1}'
          and lastbudgetprintdate is null",
          customerid, throughDate.ToShortDateString());
      updateDataFromSQL(sql);
    }
    public void flagTime(string resourceid, string customerid,
      bool selectByResource, bool selectByCustomer, bool billableOnly,
      DateTime startDate, DateTime endDate, bool includeSubProject,
      bool includeOnlyUnpaidTime, bool paid)
    {
      string whereClause = getTimeWhereClause(resourceid, customerid, string.Empty, selectByResource, selectByCustomer, false, billableOnly, startDate, endDate,
        includeSubProject, includeOnlyUnpaidTime, false, false, true);
      string paidDateTimeValue = paid ? "'" + endDate.ToShortDateString() + "'" : "null";
      string sql = "update timeentry set LastPayDateTime = {0} {1} {2}";
      string lastWhere = paid ? string.Empty : string.Format(" AND LastPayDateTime = '{0}'", endDate.ToShortDateString());
      sql = string.Format(sql, paidDateTimeValue, whereClause, lastWhere);
      updateDataFromSQL(sql);
    }
    private string getTimeWhereClause(string resourceid, string customerid, string projectid, 
      bool selectByResource, bool selectByCustomer, bool selectByProject, bool billableOnly,
      DateTime startDate, DateTime endDate, bool includeSubProject,
      bool includeOnlyUnpaidTime, bool filterByInvoiceDate, bool unPosted, bool forUpdate)
    {
      string prefix = forUpdate ? string.Empty : "t."; // if we are doing an update, we cannot use the t. qualifier for timeentry fields
      string billableWhere = billableOnly ? string.Format(" AND {0}BillingCode = 'Billable' ", prefix) : string.Empty;
      string resourceWhere = selectByResource && !string.IsNullOrEmpty(resourceid) ? string.Format(" AND {1}ResourceID = '{0}' ", resourceid, prefix) : string.Empty;
      string customerWhere = selectByCustomer && !string.IsNullOrEmpty(customerid) ? string.Format(" AND {1}CustomerID = '{0}' ", customerid, prefix) : string.Empty;
      string unpaidWhere = includeOnlyUnpaidTime ? " AND LastPayDateTime is null " : string.Empty;
      string projectWhere = selectByProject && !string.IsNullOrEmpty(projectid) ? string.Format(" AND {1}ProjectID = '{0}' ", projectid, prefix) : string.Empty;
      string invoicePostedWhere =  filterByInvoiceDate ? unPosted ? " AND InvoiceDate is null " : string.Format(" AND InvoiceDate = '{0}'", endDate.ToShortDateString()) : string.Empty;
      string whereClause = string.Format("where timeworkeddate between '{3}' and '{4}' {0} {1} {2} {5} {6} {7}",
        resourceWhere, customerWhere, billableWhere, startDate.ToShortDateString(), endDate.ToShortDateString(), unpaidWhere, projectWhere, invoicePostedWhere);
      return whereClause;
    }
    public int getTimeZoneOffset(string resourceid)
    {
      string sql = string.Format("Select TimeZoneOffsetFromServer from Resources where Resourceid = '{0}'", resourceid);
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return 0;
      int tz = 0;
      if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        tz = CommonFunctions.CInt(ds.Tables[0].Rows[0]["TimeZoneOffsetFromServer"]);
      ds.Clear();
      ds = null;
      return tz;
    }
  }
}
