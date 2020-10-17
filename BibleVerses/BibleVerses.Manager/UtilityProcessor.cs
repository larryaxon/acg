using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BibleVerses.Manager
{
    public static class UtilityProcessor
    {
        public static string heartbeat()
        {
            string dbstatus = DataSource.testDBConnection();
            if (dbstatus == "Success")
                return @"Success: Service is running, DB connection is online.";
            else
                return @"Failure: Service is running, but DB connection is failed, message = " + dbstatus;
        }

    }
}
