using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CCI.Sys.Data
{
  public partial class DataSource
  {
    public static List<T> getListFromDataset<T>(DataSet ds)
    {
      /*
       * Larry's poor man's Dapper
       * it takes a dataset and populates matching fields into a POCO class
       */
      if (ds == null)
        return null;
      if (ds.Tables.Count == 0)
        return null;
      List<T> list = new List<T>();
      DataTable dt = ds.Tables[0];
      var columns = dt.Columns;
      foreach (DataRow row in dt.Rows)
      {
        object model = Activator.CreateInstance<T>();
        Type type = model.GetType();

        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
          if (columns.Contains(property.Name))
          {
            object val = row[property.Name];
            if (val == System.DBNull.Value)
              val = null;
            property.SetValue(model, val, null);
          }
        }
        list.Add((T)model);
      }
      return list;
    }

  }
}
