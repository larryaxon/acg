using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using ACG.App.Common;
using ACG.Sys.SecurityEngine;


namespace ACG.Sys.Data
{
  public partial class DataSource
  {
    public DataAdapterContainer getMaintenanceAdapter(string tablename, Dictionary<string, string> parameters)
    {
      if (string.IsNullOrEmpty(tablename))
        //throw new Exception("getMaintenanceAdapter must have a valid table name");
        return new DataAdapterContainer();
      string encryptedFieldName = string.Empty;
      if (parameters.ContainsKey("EncryptedFieldName"))
      {
        encryptedFieldName = parameters[CommonData.parmENCRYPTEDFIELDNAME];
        parameters.Remove(CommonData.parmENCRYPTEDFIELDNAME);
      }
      bool updateable = true;
      string sql = null;
      bool validTable = false;
      //      bool hasAutoID = false;
      //      string autoIDName = "ID";
      //      if (parameters.ContainsKey("AutoIDName"))
      //      {
      //          autoIDName = parameters["AutoIDName"];
      //          hasAutoID = true;
      //      }
      string updateSQL, insertSQL, deleteSQL;
      SqlCommand cmdUpdate, cmdInsert, cmdDelete;
      updateSQL = insertSQL = deleteSQL = string.Empty;
      cmdUpdate = cmdInsert = cmdDelete = null;
      string customerid = string.Empty;
      string projectid = string.Empty;
      string subprojectid = string.Empty;
      string resourceid = string.Empty;

      switch (tablename.ToLower())
      {
        case "codemaster":
          #region codemaster
          if (parameters != null && parameters.Count == 1)
          {
            validTable = true;
            string codetype = parameters["CodeType"];
            sql = string.Format("Select CodeType, CodeValue, Description, StartDate, EndDate, LastModifiedBy, LastModifiedDateTime from CodeMaster where CodeType ='{0}'", codetype);
          }
          break;
          #endregion
        case "customers":
        case "resources":
        case "projects":
        case "subprojects":
        case "rates":
        case "costs":
        case "projectbudget":
          // note: this assumes that the calling program sent in parameters with valid fieldnames for the table we are accessing
          StringBuilder whereClause = new StringBuilder();
          foreach (KeyValuePair<string, string> parm in parameters)
          {
            if (!parm.Value.Equals("All"))
            {
              if (whereClause.Length > 0)
                whereClause.Append(" AND ");
              else
                whereClause.Append(" WHERE ");
              whereClause.Append(string.Format("{0} = '{1}'", parm.Key, parm.Value));
            }
          }
          validTable = true;
          sql = string.Format("select * from {0} {1}", tablename, whereClause.ToString());
          break;

      }
      if (validTable)
      {
        DataAdapterContainer da = (DataAdapterContainer)getDataAdapterFromSQL(sql);
        if (!string.IsNullOrEmpty(encryptedFieldName))
        {
          da.EncryptedFieldName = encryptedFieldName;
          da.DataSet = CommonFunctions.decryptDataset(da.DataSet, encryptedFieldName);
        }
        SqlCommandBuilder builder = new SqlCommandBuilder(da.DataAdapter);
        if (updateable)
        {
          if (cmdDelete == null)
            builder.GetDeleteCommand(true);
          else
          {
            builder.GetDeleteCommand(true);
            da.DataAdapter.DeleteCommand = cmdDelete;
          }
          if (cmdUpdate == null)
            builder.GetUpdateCommand(true);
          else
          {
            builder.GetUpdateCommand(true);
            da.DataAdapter.UpdateCommand = cmdUpdate;
          }
          if (cmdInsert == null)
            builder.GetInsertCommand(true);
          else
          {
            builder.GetInsertCommand(true);
            da.DataAdapter.InsertCommand = cmdInsert;
          }
        }
        if (da.DataSet.Tables.Count > 0)
          da.DataSet.Tables[0].TableName = tablename;
        return da;
      }
      return new DataAdapterContainer();
      //else
      //  throw new Exception("getMaintenanceAdapter must have a valid table name");
    }
    private SqlCommand AddParameters(SqlCommand cmd, string tablename)
    {
      cmd.Parameters.Add("NeedsReview", System.Data.SqlDbType.NVarChar, 50, "NeedsReview");
      cmd.Parameters.Add("NeedsReviewComments", System.Data.SqlDbType.NVarChar, -1, "NeedsReviewComments");
      cmd.Parameters.Add("Location", System.Data.SqlDbType.NVarChar, 50, "Location");
      cmd.Parameters.Add("CityCareProdOrderID", System.Data.SqlDbType.Int, 8, "CityCareProdOrderID");
      cmd.Parameters.Add("Carrier", System.Data.SqlDbType.NVarChar, 50, "Carrier");
      cmd.Parameters.Add("ItemID", System.Data.SqlDbType.NVarChar, 75, "ItemID");
      cmd.Parameters.Add("CommissionID", System.Data.SqlDbType.NVarChar, 128, "CommissionID");
      cmd.Parameters.Add("CityCareCommProfileID", System.Data.SqlDbType.Int, 8, "CityCareCommProfileID");
      cmd.Parameters.Add("VendorAccountID", System.Data.SqlDbType.NVarChar, 50, "VendorAccountID");
      cmd.Parameters.Add("VendorInvoiceNumber", System.Data.SqlDbType.NVarChar, 50, "VendorInvoiceNumber");
      cmd.Parameters.Add("CircuitID", System.Data.SqlDbType.NVarChar, 50, "CircuitID");
      cmd.Parameters.Add("BTN", System.Data.SqlDbType.NVarChar, 50, "BTN");
      cmd.Parameters.Add("MRC", System.Data.SqlDbType.Decimal, 18, "MRC");
      cmd.Parameters.Add("CommissionMRC", System.Data.SqlDbType.Decimal, 18, "CommissionMRC");
      cmd.Parameters.Add("Quantity", System.Data.SqlDbType.Decimal, 18, "Quantity");
      cmd.Parameters.Add("StartDate", System.Data.SqlDbType.Date, 4, "StartDate");
      cmd.Parameters.Add("EndDate", System.Data.SqlDbType.Date, 4, "EndDate");
      cmd.Parameters.Add("Customer", System.Data.SqlDbType.NVarChar, 50, "Customer");
      cmd.Parameters.Add("Payor", System.Data.SqlDbType.NVarChar, 50, "Payor");
      cmd.Parameters.Add("CycleCode", System.Data.SqlDbType.NVarChar, 50, "CycleCode");
      cmd.Parameters.Add("CityCareNetinvID", System.Data.SqlDbType.Int, 8, "CityCareNetinvID");
      cmd.Parameters.Add("InactiveFlag", System.Data.SqlDbType.NVarChar, 4, "InActiveFlag");
      //cmd.Parameters.Add("Comments", System.Data.SqlDbType.NVarChar, -1, "Comments");
      //cmd.Parameters.Add("Scrubbed", System.Data.SqlDbType.Bit, 1, "Scrubbed");
      cmd.Parameters.Add("ID", System.Data.SqlDbType.Int, 8, "ID");
      return cmd;
    }
  }
}
