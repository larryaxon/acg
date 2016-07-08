using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

namespace ACG.Common.Data
{
  public class DataAdapterContainer
  {
    public DataSet DataSet { get; set; }
    public SqlDataAdapter DataAdapter { get; set; }
    public DataAdapterContainer()
    {
      DataSet = null;
      DataAdapter = null;
    }
  }
}
