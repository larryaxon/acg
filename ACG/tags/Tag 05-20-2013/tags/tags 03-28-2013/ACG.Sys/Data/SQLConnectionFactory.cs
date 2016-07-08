using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using ACG.App.Common;
using ACG.Common.Logging;

namespace ACG.Sys.Data
{
  /// <summary>
  /// Singleton SQLConnection Factory
  /// </summary>
  /// <remarks>
  /// The SQLConnection Factory class contains all the definitions needed in order to make SQL database connections.
  /// The SQLConnection Factory class it has been declared as a sealed class to ensure no body else inherit this definition. 
  /// </remarks>
  public class SQLConnectionFactory : ACG.Common.Data.SQLConnectionFactory
  {

  }
}


