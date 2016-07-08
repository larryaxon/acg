using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using ACG.Common;
using ACG.App.Common;
using ACG.Sys.SecurityEngine;

namespace ACG.Sys.Data
{
  public partial class DataSource : DataAccessBase
  {
    public ACGForm getDSPickLists(Hashtable context, ArrayList fieldnames, string securityAccount, string securityUser, string criteria)
    {
      ACGForm returnList = new ACGForm();

      string sql = string.Empty;
      foreach (object f in fieldnames)
      {
        sql = string.Empty;
        string fld = CommonFunctions.CString(f).ToLower();
        string customerid = string.Empty;
        string projectid = string.Empty;
        switch (fld)
        {
          case "dummy":
            sql = string.Empty;
            break;
          case "customer":
            sql = "SELECT CustomerID, Name from Customers order by Name";
            break;
          case "resource":
            sql = "SELECT ResourceID, name from Resources order by Name";
            break;
          case "user":
            sql = "SELECT distinct Login, Login Name from SecurityUsers";
            break;
          case "project":
            sql = "SELECT projectid, Name from Projects";
            if (context.ContainsKey(ACGCommonData.fieldCUSTOMERID))
            {
              customerid = CommonFunctions.CString(context[ACGCommonData.fieldCUSTOMERID]);
              sql += string.Format(" WHERE customerid = '{0}'", customerid);
            }
            break;
          case "subproject":
            sql = "SELECT subprojectid, Name from SubProjects";
            if (context.ContainsKey(ACGCommonData.fieldCUSTOMERID))
            {
              customerid = CommonFunctions.CString(context[ACGCommonData.fieldCUSTOMERID]);
              sql += string.Format(" WHERE customerid = '{0}'", customerid);
              if (context.ContainsKey(ACGCommonData.fieldPROJECTID))
              {
                projectid = CommonFunctions.CString(context[ACGCommonData.fieldPROJECTID]);
                sql += string.Format(" AND projectid = '{0}'", projectid);
              }
            }
            break;
          case "billingcode":
          case "state":
            sql = string.Format(
              @"SELECT Distinct CodeValue, Description
                FROM CodeMaster
                WHERE CodeType = '{0}'
                {1} Order By Description"
              , fld, (string.IsNullOrEmpty(criteria) ? string.Empty : string.Format(" And Description like '%{0}%' ", criteria)));
            break;
        }

        if (sql != string.Empty)
        {
          returnList.Add(
            new ACGFormItem()
            {
              FormItemType = ACG.Common.FormItemTypes.PickList,
              OriginalID = fld,
              Value = ACGCommonFunctions.convertDataSetToCCITable(getDataFromSQL(sql))
            }
          );
        }
      }

      return returnList;
    }
  }
}
