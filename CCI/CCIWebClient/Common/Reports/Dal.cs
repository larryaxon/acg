using System.Data;
namespace CCIWebClient.Reports
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
