using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using ACG.Common.Logging;

namespace CCI.Common 
{
  public class CommonFunctions : ACG.Common.CommonFunctions
  {
    /*
     * This class exposes the centreal library of functions. 
     * 
     * Please do NOT add any functions to this class if it is possible to add them to ACG.Common.CommonFunctions.
     * This way, we maintain an ever-expaning set of shared functions. However, if there is something that has dependencies
     * to this local projec, then you must add it here. 
     * 
     * In that case, it should be a static method. No non-static methods are allowed in this class.
     */

    #region DataSet and ATKTable manipulation

    public static CCITable convertDataSetToCCITable(DataSet ds)
    {
      CCITable cciTable = new CCITable();

      if (ds != null && ds.Tables.Count > 0)
      {
        int colCount = ds.Tables[0].Columns.Count;
        for (int iCol = 0; iCol < colCount; iCol++)
          cciTable.AddColumn(ds.Tables[0].Columns[iCol].ColumnName.ToLower());


        object[] oColValues = new object[colCount];
        if (ds.Tables[0].Rows.Count > 0)
        {
          for (int iRow = 0; iRow < ds.Tables[0].Rows.Count; iRow++)
          {
            DataRow dr = ds.Tables[0].Rows[iRow];
            for (int iCol = 0; iCol < colCount; iCol++)
            {
              //TODO: Have to take into account the Type of the column!!
              //atkTable.AddColumn(ds.Tables[0].Columns[1].DataType
              oColValues[iCol] = getDBValue(null, dr[iCol]);
            }
            cciTable.AddRow(oColValues);
          }
        }
        ds.Clear();
        ds = null;
      }

      return cciTable;
    }
    public static PickListEntries toPickList(DataSet ds, string idMember, string descMember)
    {
      PickListEntries list = new PickListEntries();
      if (ds == null)
        return list;
      foreach (DataRow row in ds.Tables[0].Rows)
      {
        PickListEntry entry = new PickListEntry();
        entry.ID = CommonFunctions.CString(row[idMember]);
        entry.Description = CommonFunctions.CString(row[descMember]);
        list.Add(entry);
      }
      ds.Clear();
      ds = null;
      return list;
    }
    #endregion DataSet and ATKTable manipulation

    public static string ProductVersion()
    {
      Assembly assembly = Assembly.GetExecutingAssembly();
      FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
      return fvi.ProductVersion;

    }
  }
}
