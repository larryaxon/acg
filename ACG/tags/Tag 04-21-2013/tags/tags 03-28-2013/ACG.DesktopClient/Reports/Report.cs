using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace ACG.DesktopClient.Reports
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

          if (ConfigurationManager.AppSettings["reportsFolder"] != null)
            m_path = ConfigurationManager.AppSettings["reportsFolder"];
        }

        public ReportDocument getReportDocument(string fileName)
        {

            rep = new ReportDocument();
            m_path = string.Concat(m_path, fileName);
            Dal dal = new Dal();
            SqlConnectionStringBuilder cb = dal.Db.ConnectionBuilder();
            rep.Load(m_path);
            rep.DataSourceConnections[0].SetConnection(cb.DataSource, cb.InitialCatalog, cb.UserID, cb.Password);
            rep.SetDatabaseLogon(cb.UserID, cb.Password, cb.DataSource, cb.InitialCatalog, true);
            return rep;

        }

    }

}
