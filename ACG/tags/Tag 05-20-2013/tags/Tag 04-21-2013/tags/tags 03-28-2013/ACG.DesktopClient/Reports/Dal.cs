using System.Data;
namespace ACG.DesktopClient.Reports
{
    public class Dal
    {
        public DbProvider Db;

        public Dal()
        {
            Db = new DbProvider();
        }
       
    }
}
