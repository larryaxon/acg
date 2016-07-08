using System;
using System.Configuration;
using System.Configuration.Assemblies;


namespace CCIWebClient.Common
{
    public static class ConfigHelper
    {
        public static string getDefaultServer()
        {

            if (ConfigurationManager.AppSettings["server"] != null)
                return ConfigurationManager.AppSettings["server"];
            return "http://localhost:3115";
        }

        public static string getReportFolder()
        {

            if (ConfigurationManager.AppSettings["reportsfolder"] != null)
                return ConfigurationManager.AppSettings["reportsfolder"];
            return "";
        }
        public static string getDefaultPage()
        {
            if (ConfigurationManager.AppSettings["defaultpage"] != null)
                return ConfigurationManager.AppSettings["defaultpage"];

            return "default.aspx";
        }
        public static string getDefaultCommand()
        {
            if (ConfigurationManager.AppSettings["defaultcommandword"] != null)
                return ConfigurationManager.AppSettings["defaultcommandword"];

            return "command";

        }

    }
}

