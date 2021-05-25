using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using CCI.Common;
using CCI.Sys.SecurityEngine;
using TAGBOSS.Common.Model;

namespace CCI.Sys.Data
{
  public partial class DataSource
  {
    private DateTime _fluentStreamStartDate = new DateTime(2021, 5, 1);
    private DateTime _fluentStreamEndDate = new DateTime(2100, 12, 31);

    public EntityAlternateID getEntityAlternateIDFromExternalID(string externalID, DateTime? startDate)
    {
      if (string.IsNullOrWhiteSpace(externalID))
        return null;
      string sql = string.Format(@"Select * from EntityAlternateIDs WHERE ExternalID = '{0}' ", externalID);
      if (startDate != null)
        sql += string.Format(" and StartDate = '{1}';", ((DateTime)startDate).Date.ToShortDateString());
      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return null;
      // populate record
      DataTable table = ds.Tables[0];
      DataRow row = table.Rows[0];
      return getModelFromEntityAlternateIDsRow(row);
    }
    public EntityAlternateID getEntityAlternateIDFromEntity(string entity, DateTime effectiveDate)
    {
      if (string.IsNullOrWhiteSpace(entity))
        return null;
      string sql = string.Format(@"Select * from EntityAlternateIDs WHERE Entity = '{0}' and '{1}' between StartDate and EndDate", entity, effectiveDate);

      DataSet ds = getDataFromSQL(sql);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        return null;
      // populate record
      DataTable table = ds.Tables[0];
      DataRow row = table.Rows[0];
      return getModelFromEntityAlternateIDsRow(row);
    }
    private EntityAlternateID getModelFromEntityAlternateIDsRow(DataRow row)
    {
      EntityAlternateID record = new EntityAlternateID()
      {
        Entity = CommonFunctions.CString(row["Entity"]),
        StartDate = CommonFunctions.CDateTime(row["StartDate"]),
        EndDate = CommonFunctions.CDateTime(row["StartDate"]),
        ExternalServiceName = CommonFunctions.CString(row["ExternalServiceName"]),
        ExternalID = CommonFunctions.CString(row["ExternalID"])
      };
      return record;
    }
    public void saveEntityAlternateID(string entity, string externalID, string serviceName = "FluentStream", DateTime? startDate = null, DateTime? endDate = null)
    {
      DateTime dtStart;
      if (startDate == null)
        dtStart = _fluentStreamStartDate;
      else
        dtStart = (DateTime)startDate;
      DateTime dtEnd;
      if (endDate == null)
        dtEnd = _fluentStreamEndDate;
      else
        dtEnd = (DateTime)endDate;
      string sql = string.Format("INSERT INTO EntityAlternateIDs (Entity, StartDate, EndDate, ExternalServiceName, ExternalID) VALUES ('{0}','{1}', '{2}','{3}','{4}' ",
        entity, dtStart.ToShortDateString(), dtEnd.ToShortDateString(), serviceName, externalID);
      updateDataFromSQL(sql);
    }
  }
}
