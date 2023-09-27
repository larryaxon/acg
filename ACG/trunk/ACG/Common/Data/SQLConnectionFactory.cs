using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using ACG.Common;
using ACG.Common.Logging;

namespace ACG.Common.Data
{
  /// <summary>
  /// Singleton SQLConnection Factory
  /// </summary>
  /// <remarks>
  /// The SQLConnection Factory class contains all the definitions needed in order to make SQL database connections.
  /// The SQLConnection Factory class it has been declared as a sealed class to ensure no body else inherit this definition. 
  /// </remarks>
  public class SQLConnectionFactory
  {

    public static string m_ConfigFileName = CommonData.SERVERCONFIGFILENAME;
    /// <summary>
    /// Thread-Safe instance 
    /// </summary>
    private static readonly SQLConnectionFactory sqlConnectionFactoryInstance = new SQLConnectionFactory();

    /// <summary>
    /// Log class object for logging purpose
    /// </summary>
    private Log log = new Log(LogFactory.GetInstance().GetLog("ACG.Common.Data.SQLConnectionFactory"));

    /// <summary>
    /// Constant holding the definition of the configuration file name.
    /// </summary>
    private const string cCONNECTIONSTRING = "Server={0};Database={1};{2}MultipleActiveResultSets=True;Connection Timeout={3};";//New config for connStr added by Leo
    private const string cINTEGRATEDSEC = "Integrated Security=True;";
    private const string cUSERAUTHENTICATION = "User Id={0};Password={1};";
    public int TIMEOUTSECONDS = 300; // 300 seconds time out to wait for a connection

    /// <summary>
    /// Shared SQL Connection instance.
    /// </summary>
    private string connectionString;
    private SqlConnection _sqlConnection = null;


    #region ErrorMessages
    /// <summary>
    /// Constant to define database connection trouble message
    /// </summary>
    private const string EM_DATABASE_CONNECTION_FAILED = "Database Connection Failed";

    /// <summary>
    /// Constant to define configuration file not found exception message 
    /// </summary>
    private const string EM_CONFIGURATION_FILE_NOT_FOUND = "Configuration File Not Found";

    #endregion

    /// <summary>
    /// SQLConnection Factory Constructor
    /// </summary>
    /// <remarks> 
    /// Sealed constructor to ensure only this instance declared in this class can be created.
    /// </remarks>
    protected SQLConnectionFactory()
    {
    try
    {
        connectionString = getConnectionString();
      }
      catch (Exception ex)
      {
        connectionString = "NoConnectionString";
      };
    }

    /// <summary>
    /// Factory instance accessor method
    /// </summary>
    /// <returns>
    /// Returns the instance for SQL Connection definitions.
    /// </returns>
    public static SQLConnectionFactory GetInstance()
    {
      return sqlConnectionFactoryInstance;
    }
    public string getConnectionString()
    {
      string configFileName = m_ConfigFileName;
      string currentPath = CommonFunctions.getCurrentPath();
      if (!currentPath.EndsWith("\\"))
        currentPath += "\\";
      //if (!currentPath.StartsWith("\\"))
      //  currentPath = "\\" + currentPath;
      string fileLocation = currentPath + configFileName;
      string line, conStr = "";
      string[] db_server;
      string[] delimeters = new string[] { ";" };
      if (CommonData.useDBConfig)
      {
        try
        {
          System.IO.StreamReader CCIConfigFile = new System.IO.StreamReader(fileLocation);

          while ((line = CCIConfigFile.ReadLine()) != null)
          {
            if (!(line.IndexOf("//") >= 0) && (!(line.Equals(""))))
              conStr = line;
          }
          CCIConfigFile.Close();
        }
        catch (FileNotFoundException fnfe)
        {
          log.Error(Log.LogContext.Dal, EM_CONFIGURATION_FILE_NOT_FOUND, fnfe);
          string msg = "Database Configuration file " + configFileName + " could not be found.";
          FileNotFoundException myFnfe = new FileNotFoundException(msg, fnfe);
          throw myFnfe;
        }
      }
     
      //else
      //  conStr = ConfigurationManager.AppSettings.Get("Database");
      db_server = conStr.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
      /* 
         * added by LLA 1/14/2010 to set special demo database flag.
         * Ok, I know this is ugly and a serious hack, but because of de-identifyed data,
         * we needes some minor behavior differences if we are connected to a demo database.
         * We assume here that it is a demo database if it has "demo" in the name.
         * 
         * Note that db_server[0] is the instance/server name (e.g. development, or rackspace server),
         * and [1] is the name of the database
         */

      if (db_server.Length > 1)
      {
        CommonData.DatabaseName = db_server[1];
      }
      string[] connectionParameters = new string[4]; 
      if (db_server.Length > 2)
      {

        connectionParameters[0] = db_server[0];
        connectionParameters[1] = db_server[1];
        string db_sec = "";
        //TODO: OUT Parameter! We need to review this!
        string myToken = CommonFunctions.getFunctionName(CommonData.FUNCTIONCHAR + db_server[2], out db_sec);
        string[] delimiters2 = { "," };
        string[] authWords = CommonFunctions.parseString(db_sec, delimiters2);
        connectionParameters[2] = db_server[2] = string.Format(cUSERAUTHENTICATION, authWords);
        connectionParameters[3] = TIMEOUTSECONDS.ToString();

       
      }
      else if (db_server.Length == 2)

          connectionParameters = new string[] { db_server[0], db_server[1], cINTEGRATEDSEC, TIMEOUTSECONDS.ToString() };

      conStr = string.Format(cCONNECTIONSTRING, connectionParameters);
      return conStr;
    }
    /// <summary>
    /// Shared SQL Connection property
    /// </summary>
    /// <remarks>
    /// Property that allows to get or set the Database Connection.
    /// </remarks>
    public SqlConnection Connection
    {
      get
      {
        try
        {
          if (_sqlConnection == null) 
            _sqlConnection = new SqlConnection(connectionString);

          if (_sqlConnection.State == ConnectionState.Closed)
            _sqlConnection.Open();
        }
        catch (SqlException se)
        {
          log.Error(Log.LogContext.Dal, EM_DATABASE_CONNECTION_FAILED, se);
          throw new Exception(string.Format("Connection String <{0}> not valid",connectionString));
        }

        return _sqlConnection;
      }
    }

    public SqlConnection ConnectionBigPacket
    {
      get
      {
        SqlConnection sqlConnection = null;
        try
        {
          sqlConnection = new SqlConnection(connectionString + ";Packet Size=32767");

          if (sqlConnection.State == ConnectionState.Closed)
            sqlConnection.Open();
        }
        catch (SqlException se)
        {
          log.Error(Log.LogContext.Dal, EM_DATABASE_CONNECTION_FAILED, se);
        }

        return sqlConnection;
      }
    }
  }
}


