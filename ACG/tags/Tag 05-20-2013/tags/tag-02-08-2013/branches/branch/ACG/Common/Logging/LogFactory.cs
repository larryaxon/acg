using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security;

using log4net;
using log4net.Config;

namespace ACG.Common.Logging
{
  /// <summary>
  /// Singleton LogFactory class 
  /// </summary>
  public class LogFactory
  {
    private static readonly LogFactory logFactoryInstance = new LogFactory();

    //New config file for Logging Factory
    private const string configFileName = "log4net.config";    
       
    /// <summary>
    /// Sealed Constructor 
    /// </summary>
    protected LogFactory()
    {
      try
      {
        String currentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");

        if (!currentPath.EndsWith("\\"))
          currentPath += "\\";

        XmlConfigurator.ConfigureAndWatch(new FileInfo(currentPath + configFileName));
      }
      catch (Exception e)
      {
        //Nothing to do! This is an error that we get when we are using ACG inside SQL Server as managed SP
      }
    }

    /// <summary>
    /// Factory instance accessor method
    /// </summary>
    /// <returns></returns>
    public static LogFactory GetInstance()
    {
      return logFactoryInstance;
    }

    /// <summary>
    /// Returns the current log for the instance of the caller class
    /// </summary>
    /// <param name="className">
    /// The  caller class pass as a fully qualified name string.
    /// </param>
    /// <returns>Returns the ILog Instances</returns>
    public ILog GetLog(string className)
    {
      return new Log(LogManager.GetLogger(className));      
    }

    /// <summary>
    /// Returns the current log for the instance of the caller class
    /// </summary>
    /// <param name="classInstance">
    /// The caller class object pass as This
    /// </param>
    /// <returns>Returns the ILog Instances</returns>
    public ILog GetLog(object classInstance)
    {
      return new Log(LogManager.GetLogger(classInstance.GetType()));
    }

    /// <summary>
    /// Returns the current log for the instance of the caller class
    /// </summary>
    /// <param name="classType">
    /// The caller class type pass as a Type object.
    /// </param>
    /// <returns>Returns the ILog Instances</returns>
    public ILog GetLog(Type classType) 
    {
      return new Log(LogManager.GetLogger(classType));
    }
  }
}
