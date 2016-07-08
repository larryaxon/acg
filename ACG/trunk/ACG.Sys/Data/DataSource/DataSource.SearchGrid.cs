using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using ACG.App.Common;
using ACG.Common;
using ACG.Sys.SecurityEngine;

namespace ACG.Sys.Data
{
  public partial class DataSource
  {
    public DataSet getResearchData(ACGCommonData.NameTypes type, object oWhere, int maxCount)
    {
      string where = string.Empty;
      string innerWhere = string.Empty;
      if (oWhere != null)
      {
        if (oWhere.GetType() == typeof(string))
          where = CommonFunctions.CString(oWhere);
        else
          if (oWhere.GetType() == typeof(string[]))
          {
            string[] whereClauses = (string[])oWhere;
            if (whereClauses.GetLength(0) > 0)
              where = whereClauses[0];
            if (whereClauses.GetLength(0) > 1)
              innerWhere = whereClauses[1];
          }
      }
      string topClause = string.Empty;
      string fromClause = string.Empty;
      string orderbyClause = string.Empty;
      string whereClause = string.Empty;
      if (maxCount > 0)
        topClause = string.Format(" TOP {0} ", maxCount.ToString());
      string sql = string.Empty;
      switch (type)
      {
        case ACGCommonData.NameTypes.TimeDetail:
          fromClause = @" * from (select t.ID, t.ResourceID, c.Name Customer, p.Name Project, isnull(s.Name, 'None') SubProject, TimeWorkedDate Date, 
StartTime, EndTime, Convert(decimal(18, 2), EnteredTime) / 60 Hours, BillingCode, Description from timeentry t
left join customers c on t.customerid = c.customerid
left join projects p on t.customerid = p.customerid and t.projectid = p.projectid
left join subprojects s on t.customerid = s.customerid and t.projectid = s.projectid and t.subprojectid = s.subprojectid) time";
          orderbyClause = " order by time.ResourceID, time.Date, time.Customer, time.Project, time.SubProject";
          break;
        case ACGCommonData.NameTypes.BudgetQuery:
          fromClause = " * from vw_budgetreport";
          orderbyClause = " order by CustomerName, ProjectName, SubProjectName, Resource";
          break;
        case ACGCommonData.NameTypes.ResourceCost:
          fromClause = " * from Costs";
          orderbyClause = " order by ResourceID, CustomerID ";
          break;
        case ACGCommonData.NameTypes.BudgetSummary:
          fromClause = @" * from vw_BudgetSummary";
          orderbyClause = " order by CustomerName, Priority, ProjectName, SubProjectName";
          break;
        case ACGCommonData.NameTypes.BudgetEditProjectBudget:
          fromClause = " ID, customerid, projectid, subprojectid, ResourceID, OriginalBudgetHours, RevisedBudgetHours, Rate, EstimatedHoursToComplete, OriginalCompletionDate, RevisedCompletionDate from ProjectBudget ";
          orderbyClause = " order by CustomerId, ProjectID, subProjectId, ResourceID ";
          break;
        case ACGCommonData.NameTypes.BudgetEditTimeEntry:
          fromClause = " ID, customerid, projectid, subprojectid, ResourceID, TimeWorkedDate, EnteredTime, BilledTime, BillingCode, Rate, BilledAmount, InvoiceNumber, InvoiceDate, Description from TimeEntry ";
          orderbyClause = " order by CustomerID, ProjectID, SubProjectID, ResourceID, TimeWorkedDate ";
          break;
        case ACGCommonData.NameTypes.ResourceTimeSummary:
          if (string.IsNullOrEmpty(innerWhere))
          {
            DateTime dt = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek - 1);
            innerWhere = string.Format("'{0}', 0", dt.ToShortDateString());
          }
          fromClause = string.Format("EXEC getResourceTime {0}", innerWhere); // innerWhere clause for this nametype is the parameter list
          orderbyClause = string.Empty;
          break;
        case ACGCommonData.NameTypes.BilledInvoices:
          fromClause = @" * from (select customerid, invoicedate, invoicenumber, Convert(money,sum(billedamount)) InvoiceTotal from timeentry         
            group by customerid, invoicenumber, invoicedate) i where invoicenumber is not null";
          orderbyClause = " order by customerid, invoicedate ";
          break;
        default:
          break;
      }
      if (!string.IsNullOrEmpty(where))
      {
        string qualifier;
        if (fromClause.ToLower().Contains(" where "))
          qualifier = "AND";
        else
          qualifier = "WHERE";
        whereClause = string.Format(" {0} {1} ", qualifier, where);
      }
      if (!string.IsNullOrEmpty(fromClause))
        if (fromClause.StartsWith("EXEC", StringComparison.CurrentCultureIgnoreCase))
          sql = fromClause;
        else
          sql = string.Format("SELECT {0}{1}{2}{3}", topClause, fromClause, whereClause, orderbyClause);
      if (string.IsNullOrEmpty(sql))
        return null;
      else
        return getDataFromSQL(sql);

    }

  }
}
