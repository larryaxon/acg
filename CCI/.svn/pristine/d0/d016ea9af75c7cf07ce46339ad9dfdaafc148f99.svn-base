using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;
using System.IO;
using CCIWebClient.Common;

namespace CCIWebClient.Reports
{

    public class Report
    {
        private string m_path = "";
        public ReportDocument rep;
        public Report(string path)
        {
            if (m_path.EndsWith(@"\"))
                m_path = path;
            else
                m_path = string.Concat(path, @"\");

        }
        public Report()
        {
            //m_path = Directory.GetCurrentDirectory();
            //if (m_path.EndsWith(@"\"))
            //    string.Concat(m_path, @"Reports\");
            //else
            //    m_path = string.Concat(m_path, @"\Reports\");
            m_path = ConfigHelper.getReportFolder();
        }

        public ReportDocument getReporDocument(string fileName)
        {

            rep = new ReportDocument();
            string repPath= string.Concat(m_path, fileName);
            Dal dal = new Dal();
            SqlConnectionStringBuilder cb = dal.Db.ConnectionBuilder();
            rep.Load(repPath);
            rep.DataSourceConnections[0].SetConnection(cb.DataSource, cb.InitialCatalog, cb.UserID, cb.Password);
            rep.SetDatabaseLogon(cb.UserID, cb.Password, cb.DataSource, cb.InitialCatalog, true);

            //CAC- set connection info...
            string test = dal.Db.getConnectionString();

            foreach (Table table in rep.Database.Tables)
            {
              //table.LogOnInfo.ConnectionInfo = ciReportConnection;

              table.LogOnInfo.ConnectionInfo.ServerName = cb.DataSource;
              table.LogOnInfo.ConnectionInfo.DatabaseName = cb.InitialCatalog;
              table.LogOnInfo.ConnectionInfo.UserID = cb.UserID;
              table.LogOnInfo.ConnectionInfo.Password = cb.Password;

              table.ApplyLogOnInfo(table.LogOnInfo);
            }

            foreach (ReportDocument sr in rep.Subreports)
            {
                sr.SetDatabaseLogon(cb.UserID, cb.Password, cb.DataSource, cb.InitialCatalog, true);
            }
            //

            return rep;
        }
    }
}
