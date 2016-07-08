using System;
using System.Collections.Generic;
using System.Text;

using log4net;

namespace CCI.Common.Logging
{
  /// <summary>
  /// The Log class implements ILog interface from log4net assembly to encapsulate, enhance our methods to prevent external communication from internal operations and protect the integrity of the component.
  /// </summary>
  public class Log : ILog
  {
    /// <summary>
    /// Instances of the implementing class to allow internal communication of log4net assembly.
    /// </summary>
    protected ILog log = null;

    /// <summary>
    /// The LogContext enum helps us to assign a code for each directory and subdirectory code level implemented in the project.
    /// </summary>    
    public enum LogContext { CCI = 1000, Common, Logging, CCICommissions, Dal };

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="log">
    /// Receives an object from the same interface implemented by this class
    /// </param>
    public Log(ILog log) 
    {
      this.log = log;
    }

    protected Log() { }

    /// <summary>
    /// Formats a string from two parameters to help the overload method to make these two parameters into one.
    /// </summary>
    /// <param name="context">
    /// Holds the enum value representing the level of directory in the project.
    /// </param>
    /// <param name="message">
    /// The messages added by the programmer to denote the event occurred.
    /// </param>
    /// <returns></returns>
    private string FormatErrorMessage(LogContext context, object message)
    {
      return ((int)context).ToString() + " - " + message.ToString();
    }
   
    #region ILog Members

    /// <summary>
    /// Overloaded method by the programmer to enhance flexibility
    /// </summary>
    /// <param name="context">Holds the enum value representing the level of directory in the project.</param>
    /// <param name="message">The messages added by the programmer to denote the event occurred.</param>
    /// <param name="error">The exception occurred in the moment</param>
    public void Debug(LogContext context, object message, Exception error)
    {
      Debug(FormatErrorMessage(context, message), error);
    }
    
    /// <summary>
    /// Log a message object with the Debug level including the stack trace of the Exception passed as a parameter.
    /// </summary>
    /// <param name="message">The message object to log.</param>
    /// <param name="exception">The exception to log, including its stack trace.</param>
    /// <remarks>See the Debug form for more detailed information.</remarks>
    public void Debug(object message, Exception exception)
    {
      log.Debug(message, exception);
    }

    /// <summary>
    ///  Log a message object with the Debug level.
    /// </summary>
    /// <param name="message">The message object to log.</param>    
    public void Debug(object message)
    {
      log.Debug(message);
    }

    /// <summary>
    /// Log a formatted string with the Debug level.
    /// </summary>
    /// <param name="provider">An IFormatProvider that supplies culture-specific formatting information</param>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="args">An Object array containing zero or more objects to format</param>
    /// <remarks>The message is formatted using the String.Format method. See Format for details of the syntax of the format string and the behavior of the formatting.
    /// This method does not take an Exception object to include in the log event. To pass an Exception use one of the Debug methods instead.
    /// </remarks>
    public void DebugFormat(IFormatProvider provider, string format, params object[] args)
    {
      log.DebugFormat(provider, format, args);
    }

    /// <summary>
    /// Log a formatted string with the Debug level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="arg0">An Object to format</param>
    /// <param name="arg1">An Object to format</param>
    /// <param name="arg2">An Object to format</param>
    /// <remarks>
    /// The message is formatted using the String.Format method. See Format for details of the syntax of the format string and the behavior of the formatting.
    /// This method does not take an Exception object to include in the log event. To pass an Exception use one of the Debug methods instead.
    /// </remarks>
    public void DebugFormat(string format, object arg0, object arg1, object arg2)
    {
      log.DebugFormat(format, arg0, arg1, arg2);
    }

    /// <summary>
    /// Log a formatted string with the Debug level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="arg0">An Object to format</param>
    /// <param name="arg1">An Object to format</param>    
    /// <remarks>
    /// The message is formatted using the String.Format method. See Format for details of the syntax of the format string and the behavior of the formatting.
    /// This method does not take an Exception object to include in the log event. To pass an Exception use one of the Debug methods instead.
    /// </remarks>
    public void DebugFormat(string format, object arg0, object arg1)
    {
      log.DebugFormat(format, arg0, arg1);
    }

    /// <summary>
    /// Log a formatted string with the Debug level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="arg0">An Object to format</param>    
    /// <remarks>
    /// The message is formatted using the String.Format method. See Format for details of the syntax of the format string and the behavior of the formatting.
    /// This method does not take an Exception object to include in the log event. To pass an Exception use one of the Debug methods instead.
    /// </remarks>
    public void DebugFormat(string format, object arg0)
    {
      log.DebugFormat(format, arg0);
    }   
      
    /// <summary>
    /// Log a formatted string with the Debug level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="args">An Object array containing zero or more objects to format</param>
    public void DebugFormat(string format, params object[] args)
    {
      log.DebugFormat(format, args);
    }

    /// <summary>
    /// Overloaded method by the programmer to enhance flexibility
    /// </summary>
    /// <param name="context">Holds the enum value representing the level of directory in the project.</param>
    /// <param name="message">The messages added by the programmer to denote the event occurred.</param>
    /// <param name="error">The exception occurred in the moment</param>
    public void Error(LogContext context, object message, Exception error)
    {      
      Error(FormatErrorMessage(context, message), error);
    }

    /// <summary>
    /// Log a message object with the Error level.
    /// </summary>
    /// <param name="message">The message object to log.</param>
    /// <param name="exception">The exception to log, including its stack trace.</param>
    public void Error(object message, Exception exception)
    {
      log.Error(message, exception);
    }

    /// <summary>
    /// Log a message object with the Error level.
    /// </summary>
    /// <param name="message">The message object to log.</param>    
    public void Error(object message)
    {
      log.Error(message);
    }

    /// <summary>
    /// Log a formatted message string with the Error level.
    /// </summary>
    /// <param name="provider">An IFormatProvider that supplies culture-specific formatting information</param>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="args">An Object array containing zero or more objects to format</param>
    /// <remarks> The message is formatted using the String.Format method. See Format for details of the syntax of the format string and the behavior of the formatting.
    /// This method does not take an Exception object to include in the log event. To pass an Exception use one of the Error methods instead.
    /// </remarks>
    public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
    {
      log.ErrorFormat(provider, format, args);
    }

    /// <summary>
    /// Log a formatted message string with the Error level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="arg0">An Object to format</param>
    /// <param name="arg1">An Object to format</param>
    /// <param name="arg2">An Object to format</param>
    public void ErrorFormat(string format, object arg0, object arg1, object arg2)
    {
      log.ErrorFormat(format, arg0, arg1, arg2);
    }

    /// <summary>
    /// Log a formatted message string with the Error level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="arg0">An Object to format</param>
    /// <param name="arg1">An Object to format</param>    
    public void ErrorFormat(string format, object arg0, object arg1)
    {
      log.ErrorFormat(format, arg0, arg1);
    }

    /// <summary>
    /// Log a formatted message string with the Error level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="arg0">An Object to format</param>    
    public void ErrorFormat(string format, object arg0)
    {
      log.ErrorFormat(format, arg0);
    }

    /// <summary>
    /// Log a formatted message string with the Error level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="args">An Object array containing zero or more objects to format</param>
    public void ErrorFormat(string format, params object[] args)
    {
      log.ErrorFormat(format, args);
    }

    /// <summary>
    /// Log a message object with the Fatal level.
    /// </summary>
    /// <param name="context">Holds the enum value representing the level of directory in the project.</param>
    /// <param name="message">The messages added by the programmer to denote the event occurred.</param>
    /// <param name="error">The exception occurred in the moment</param>
    public void Fatal(LogContext context, object message, Exception error) 
    {
      Fatal(FormatErrorMessage(context, message), error);
    }

    /// <summary>
    /// Log a message object with the Fatal level.
    /// </summary>
    /// <param name="message">The message object to log.</param>
    /// <param name="exception">The exception to log, including its stack trace.</param>
    public void Fatal(object message, Exception exception)
    {
      log.Fatal(message, exception);
    }

    /// <summary>
    /// Log a message object with the Fatal level.
    /// </summary>
    /// <param name="message">The message object to log.</param>    
    public void Fatal(object message)
    {
      log.Fatal(message);
    }

    /// <summary>
    /// Log a formatted message string with the Fatal level.
    /// </summary>
    /// <param name="provider">An IFormatProvider that supplies culture-specific formatting information</param>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="args">An Object array containing zero or more objects to format</param>
    public void FatalFormat(IFormatProvider provider, string format, params object[] args)
    {
      log.FatalFormat(provider, format, args);
    }

    /// <summary>
    /// Log a formatted message string with the Fatal level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="arg0">An Object to format</param>
    /// <param name="arg1">An Object to format</param>
    /// <param name="arg2">An Object to format</param>
    public void FatalFormat(string format, object arg0, object arg1, object arg2)
    {
      log.FatalFormat(format, arg0, arg1, arg2);
    }

    /// <summary>
    /// Log a formatted message string with the Fatal level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="arg0">An Object to format</param>
    /// <param name="arg1">An Object to format</param>    
    public void FatalFormat(string format, object arg0, object arg1)
    {
      log.FatalFormat(format, arg0, arg1);
    }

    /// <summary>
    /// Log a formatted message string with the Fatal level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="arg0">An Object to format</param>    
    public void FatalFormat(string format, object arg0)
    {
      log.FatalFormat(format, arg0);
    }

    /// <summary>
    /// Log a formatted message string with the Fatal level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="args">An Object array containing zero or more objects to format</param>
    public void FatalFormat(string format, params object[] args)
    {
      log.FatalFormat(format, args);
    }

    /// <summary>
    /// Overloaded method by the programmer to enhance flexibility
    /// </summary>
    /// <param name="context">Holds the enum value representing the level of directory in the project.</param>
    /// <param name="message">The messages added by the programmer to denote the event occurred.</param>
    /// <param name="error">The exception occurred in the moment</param>
    public void Info(LogContext context, object message, Exception error)
    {
      Info(FormatErrorMessage(context, message), error);
    }

    /// <summary>
    /// Logs a message object with the INFO level including the stack trace of the Exception passed as a parameter.
    /// </summary>
    /// <param name="message">The message object to log.</param>
    /// <param name="exception">The exception to log, including its stack trace.</param>
    public void Info(object message, Exception exception)
    {
      log.Info(message, exception);
    }

    /// <summary>
    /// Logs a message object with the INFO level.
    /// </summary>
    /// <param name="message">The message object to log.</param>    
    public void Info(object message)
    {
      log.Info(message);
    }

    /// <summary>
    /// Log a formatted message string with the Info level.
    /// </summary>
    /// <param name="provider">An IFormatProvider that supplies culture-specific formatting information</param>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="args">An Object array containing zero or more objects to format</param>
    public void InfoFormat(IFormatProvider provider, string format, params object[] args)
    {
      log.InfoFormat(provider, format, args);
    }

    /// <summary>
    /// Log a formatted message string with the Info level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="arg0">An Object to format</param>
    /// <param name="arg1">An Object to format</param>
    /// <param name="arg2">An Object to format</param>
    public void InfoFormat(string format, object arg0, object arg1, object arg2)
    {
      log.InfoFormat(format, arg0, arg1, arg2);
    }

    /// <summary>
    /// Log a formatted message string with the Info level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="arg0">An Object to format</param>
    /// <param name="arg1">An Object to format</param>    
    public void InfoFormat(string format, object arg0, object arg1)
    {
      log.InfoFormat(format, arg0, arg1);
    }
    
    /// <summary>
    /// Log a formatted message string with the Info level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="arg0">An Object to format</param>
    public void InfoFormat(string format, object arg0)
    {
      log.InfoFormat(format, arg0);
    }

    /// <summary>
    /// Log a formatted message string with the Info level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="args">An Object array containing zero or more objects to format</param>
    public void InfoFormat(string format, params object[] args)
    {
      log.InfoFormat(format, args);
    }

    /// <summary>
    /// Checks if this logger is enabled for the Debug level.
    /// </summary>
    public bool IsDebugEnabled
    {
      get 
      { 
        return log.IsDebugEnabled; 
      }
    }

    /// <summary>
    /// Checks if this logger is enabled for the Error level.
    /// </summary>
    public bool IsErrorEnabled
    {
      get
      {
        return log.IsErrorEnabled;
      }
    }

    /// <summary>
    /// Checks if this logger is enabled for the Fatal level.
    /// </summary>
    public bool IsFatalEnabled
    {
      get
      {
        return log.IsFatalEnabled;
      }
    }

    /// <summary>
    /// Checks if this logger is enabled for the Info level.
    /// </summary>
    public bool IsInfoEnabled
    {
      get
      {
        return log.IsInfoEnabled;
      }
    }

    /// <summary>
    /// Checks if this logger is enabled for the Warn level.
    /// </summary>
    public bool IsWarnEnabled
    {
      get
      {
        return log.IsWarnEnabled;
      }
    }

    /// <summary>
    /// Overloaded method by the programmer to enhance flexibility
    /// </summary>
    /// <param name="context">Holds the enum value representing the level of directory in the project.</param>
    /// <param name="message">The messages added by the programmer to denote the event occurred.</param>
    /// <param name="error">The exception occurred in the moment</param>
    public void Warn(LogContext context, object message, Exception error)
    {
      Warn(FormatErrorMessage(context, message), error);
    }

    /// <summary>
    /// Log a message object with the Warn level.
    /// </summary>
    /// <param name="message">The message object to log.</param>
    /// <param name="exception">The exception to log, including its stack trace.</param>
    public void Warn(object message, Exception exception)
    {
      log.Warn(message, exception);
    }

    /// <summary>
    /// Log a message object with the Warn level.
    /// </summary>
    /// <param name="message">The message object to log.</param>    
    public void Warn(object message)
    {
      log.Warn(message);
    }

    /// <summary>
    /// Log a formatted message string with the Warn level.
    /// </summary>
    /// <param name="provider">An IFormatProvider that supplies culture-specific formatting information</param>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="args">An Object array containing zero or more objects to format</param>
    public void WarnFormat(IFormatProvider provider, string format, params object[] args)
    {
      log.WarnFormat(provider, format, args);
    }

    /// <summary>
    /// Log a formatted message string with the Warn level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="arg0">An Object to format</param>
    /// <param name="arg1">An Object to format</param>
    /// <param name="arg2">An Object to format</param>
    public void WarnFormat(string format, object arg0, object arg1, object arg2)
    {
      log.WarnFormat(format, arg0, arg1, arg2);
    }

    /// <summary>
    /// Log a formatted message string with the Warn level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="arg0">An Object to format</param>
    /// <param name="arg1">An Object to format</param>    
    public void WarnFormat(string format, object arg0, object arg1)
    {
      log.WarnFormat(format, arg0, arg1);
    }

    /// <summary>
    /// Log a formatted message string with the Warn level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="arg0">An Object to format</param>    
    public void WarnFormat(string format, object arg0)
    {
      log.WarnFormat(format, arg0);
    }

    /// <summary>
    /// Log a formatted message string with the Warn level.
    /// </summary>
    /// <param name="format">A String containing zero or more format items</param>
    /// <param name="args">An Object array containing zero or more objects to format</param>
    public void WarnFormat(string format, params object[] args)
    {
      log.WarnFormat(format, args);
    }

    #endregion

    #region ILoggerWrapper Members

    public log4net.Core.ILogger Logger
    {
      get 
      {
        return log.Logger;
      }
    }

    #endregion
  }
}
