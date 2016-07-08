using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using ACG.App.Common;

namespace ACG.Sys.Data
{
  public abstract class DataAccessBase : ACG.Common.Data.DataAccessBase
  {
    protected new DataAdapterContainer getDataAdapterFromSQL(string mySQL)
    {
      ACG.Common.Data.DataAdapterContainer c = base.getDataAdapterFromSQL(mySQL);
      DataAdapterContainer da = new DataAdapterContainer();
      da.DataAdapter = c.DataAdapter;
      da.DataSet = c.DataSet;
      return da;
    }
 
  }
}
