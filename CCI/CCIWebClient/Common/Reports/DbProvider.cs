using System.Data;
using System.Configuration;
using System.Data.SqlClient;
//using MySql.Data.MySqlClient;

namespace CCIWebClient.Reports
{
    public class DbProvider
    {
        private string m_ConnectionString = "";
        private string m_ConnName = "citycommunication";


        public DbProvider()
        {
            m_ConnectionString = getConnectionString();
        }
        public DbProvider(string ConnectionName)
        {
            m_ConnName = ConnectionName;
            m_ConnectionString = getConnectionString();

        }
        public SqlConnectionStringBuilder ConnectionBuilder()
        {
            string conn = getConnectionString();
            SqlConnectionStringBuilder cb = new SqlConnectionStringBuilder(conn);
            return cb;
        }

        public string getConnectionString()
        {
            ConnectionStringSettingsCollection connectionStringCol = ConfigurationManager.ConnectionStrings;
            string connectionString = "";
            if (connectionStringCol.Count != 0)
            {
                if (connectionStringCol[m_ConnName] != null)
                {
                    connectionString = connectionStringCol[m_ConnName].ToString();
                }
            }

            return connectionString;
        }


    }
}
