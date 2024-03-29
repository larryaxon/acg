﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using ACG.Common.Logging;

namespace ACG.Common
{
  public class CommonFunctions
  {
    static Log log = (Log)LogFactory.GetInstance().GetLog("ACG.Common.CommonFunctions");
    static EncryptDecryptString _encrypt = new EncryptDecryptString();

    public static Exception getInnerException(Exception ex)
    {
      Exception e = ex;
      while (e.InnerException != null)
        e = e.InnerException;
      return e;
    }

    #region local classes
    class SortKey : IComparable
    {
      public object key = null;
      public int index = 0;
      public int CompareTo(object compareKey)
      {
        string typeName = compareKey.GetType().Name.ToString().ToLower();
        if (typeName == "sortkey")
        {
          SortKey o = (SortKey)compareKey;
          return CompareTo(o);
        }
        throw new Exception("Cannot compare type " + typeName + " with type SortKey");
      }
      public int CompareTo(SortKey o)
      {
        if (o == null)
          return +1;
        if (o.key == null)
          return +1;
        if (objectEquals(key, o.key))
          if (index < o.index)
            return -1;
          else
            if (index == o.index)
              return 0;
            else
              return 1;
        if (passesTest(key, ">", o.key))
          return 1;
        else
          return -1;
      }
    }
    #endregion
    #region string functions
    /// <summary>
    /// Function that fill a field n times with a value
    /// </summary>
    /// <param name="field"></param>
    /// <param name="length"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string formatString(string field, int length, string value)
    {
      int size = (length - field.Length) + 1;
      for (int i = 0; i < size; i++)
      {
        field = value + field;
      }

      return field;
    }
    /// <summary>
    /// Function that leaves only the first letter in Upper Case and the rest in lower case.
    /// The words inside brackets are not modified.
    /// </summary>
    /// <param name="strOld"></param>
    /// <returns></returns>    
    public static string properCase(string strOld)
    {
      string strNew = " ";
      string strCurrent = "";
      string strPrevious = "";
      int x = 0;
      int strLen = 0;
      string closeBracket = "";
      string openBracket = "";
      bool sameOpenAndCloseBracket = false;

      strPrevious = strOld.Substring(0, 1);
      strNew = " ";
      x = 0;
      strLen = strOld.Length;
      openBracket = "\"" + "\'" + "(" + "[" + "{";
      closeBracket = "\"" + "\'" + ")" + "]" + "}";

      while (x < strLen)
      {
        strCurrent = strOld.Substring(x, 1);
        if (x == 0 && !strCurrent.Equals(" "))
        {
          strNew = strNew + strCurrent.ToUpper();
        }
        else
        {
          if (strPrevious.Equals(" ") && !(strCurrent.Equals(" ")))
          {
            strNew = strNew + strCurrent.ToUpper();
          }
          else if (openBracket.Contains(strPrevious) && !sameOpenAndCloseBracket)
          {
            sameOpenAndCloseBracket = (strPrevious == "\"");
            strNew = strNew + strCurrent;
            x++;
            strCurrent = strOld.Substring(x, 1);
            while (!closeBracket.Contains(strCurrent))
            {
              strNew = strNew + strCurrent;
              x++;

              if (x < strOld.Length)
                strCurrent = strOld.Substring(x, 1);
              else
              {
                strCurrent = "";
                break;
              }
            }
            strNew = strNew + strCurrent;
          }
          else
          {
            sameOpenAndCloseBracket = false;
            strNew = strNew + strCurrent.ToLower();
          }
        }
        strPrevious = strCurrent;
        x++;
      }

      return strNew;
    }
    /// <summary>
    /// Function that concatenate the same value "0" n times.
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string replicate(int length)
    {
      if (length == 0)
      {
        return "";
      }
      string stringReplicate = "0";
      for (int i = 1; i < length; i++)
      {
        stringReplicate += stringReplicate;
      }

      return stringReplicate;
    }
    /// <summary>
    /// If the string is encased in parentheses, then strip them off and return what is inside. 
    /// Otherwise, return the original string. In either case, trim off leading and trailing spaces
    /// </summary>
    /// <param name="strIn"></param>
    /// <returns></returns>
    public static string stripParens(string strIn)
    {
      if (strIn == null)
        return null;
      strIn = strIn.Trim();   // trim of leading spaces first
      if (strIn.StartsWith(CommonData.LEFTCHAR.ToString()) && strIn.EndsWith(CommonData.RIGHTCHAR.ToString()))
        return strIn.Substring(1, strIn.Length - 2);
      else
        return strIn;
    }
    public static string getJsonName(string s)
    {
      if (string.IsNullOrEmpty(s))
        return string.Empty;
      int firstQuote = s.IndexOf(CommonData.cDOUBLEQUOTE); // find the first quote
      int secondQuote = s.IndexOf(CommonData.cDOUBLEQUOTE, firstQuote + 1); // now find the second one
      if (secondQuote == -1)
        return s.Substring(firstQuote + 1);
      else
        return s.Substring(firstQuote + 1, secondQuote - 1 - firstQuote).Trim();
    }
    public static string getJsonValue(string s)
    {
      if (string.IsNullOrEmpty(s))
        return string.Empty;
      int firstColon = s.IndexOf(':');
      if (firstColon == -1)
        return string.Empty;
      return stripDelims(s.Substring(firstColon + 1).Trim(), CommonData.cDOUBLEQUOTE);
    }

    //TODO: Check this! this supposed to get a TOKEN, based on a passed char delimeter
    public static string getDelimToken(string s, char delim, int tokenIndex)
    {
      char leftdelim = delim;
      char rightdelim = CommonData.cRIGHTCURLY;
      int tokenIndexCnt = 0;
      string tokenValue = string.Empty;

      if (delim == CommonData.cLEFTSQUARE)
        rightdelim = CommonData.cRIGHTSQUARE;
      else if (delim == CommonData.cDOUBLEQUOTE)
        rightdelim = delim;
      else if (delim == CommonData.cLEFT)
        rightdelim = CommonData.cRIGHT;

      bool sameRightAndLeftDelim = (leftdelim == rightdelim);
      string s2 = s;

      int firstDelim = -1;
      int lastDelim = -1;

      while (tokenIndexCnt < tokenIndex && !string.IsNullOrEmpty(s2))
      {
        firstDelim = s2.IndexOf(leftdelim);  // will be -1 if not found, but that is ok cause -1 + 1 = 0 which is first char

        if (firstDelim == -1 || !(firstDelim + 1 < s2.Length))
          return string.Empty;

        tokenIndexCnt++;
        if (tokenIndexCnt < tokenIndex) 
        {
          if (sameRightAndLeftDelim)
          {
            lastDelim = s2.Substring(firstDelim + 1).IndexOf(leftdelim);  // will be -1 if not found, but that is ok cause -1 + 1 = 0 which is first char

            if (lastDelim == -1)
              return string.Empty;

            if (lastDelim + 1 < s2.Substring(firstDelim + 1).Length)
              s2 = s2.Substring(firstDelim + 1).Substring(lastDelim + 1);
            else
              return string.Empty;
          }
          else
            s2 = s2.Substring(firstDelim + 1);
        }
      }

      lastDelim = -1;
      if (!string.IsNullOrEmpty(s2))
      {
        if (firstDelim > -1 && firstDelim + 1 < s2.Length)
        {
          for (int i = firstDelim + 1; i < s2.Length; i++)
            if (s2[i] == rightdelim)
            {
              lastDelim = i;
              break;
            }
        }

        if (lastDelim == -1 || lastDelim <= firstDelim)
          tokenValue = s2.Substring(firstDelim + 1).Trim();
        else
          tokenValue = s2.Substring(firstDelim + 1, lastDelim - firstDelim - 1).Trim();
      }
      else
        tokenValue = string.Empty;

      return tokenValue;
    }
    
    public static string stripDelims(string s, char delim)
    {
      char leftdelim = delim;
      char rightdelim = CommonData.cRIGHTCURLY;
      
      if (delim == CommonData.cLEFTSQUARE)
        rightdelim = CommonData.cRIGHTSQUARE;
      else if (delim == CommonData.cDOUBLEQUOTE)
        rightdelim = delim;
      else if (delim == CommonData.cLEFT)
        rightdelim = CommonData.cRIGHT;

      int firstDelim = s.IndexOf(leftdelim);  // will be -1 if not found, but that is ok cause -1 + 1 = 0 which is first char
      int lastDelim = -1;

      for (int i = s.Length - 1; i >= 0; i--)
        if (s[i] == rightdelim)
        {
          lastDelim = i;
          break;
        }
      
      if (lastDelim == -1 || lastDelim <= firstDelim)
        return s.Substring(firstDelim + 1).Trim();
      else
        return s.Substring(firstDelim + 1, lastDelim - firstDelim - 1).Trim();
    }
    /// <summary>
    /// Creates a valid xml string by excaping xml reserved characters
    /// </summary>
    /// <param name="inString"></param>
    /// <returns></returns>
    public static string toValidXMLString(object inString)
    {
      return toValidXMLString(inString, true);
    }
    /// <summary>
    /// Overload. Creates a valid xml string by excaping xml reserved characters. Allows specifying that '<' and '>' not be replaced
    /// </summary>
    /// <param name="inString">string to transform</param>
    /// <param name="replaceGTLT">replace the '<' and '>'? </param>
    /// <returns>transformed version of inString</returns>
    public static string toValidXMLString(object inString, bool replaceGTLT)
    {
      string outString = string.Empty;
      if (inString != null)
      {
        outString = inString.ToString().Replace(CommonData.c_FROM_XML_AMP, CommonData.c_TO_XML_AMP);
        outString = outString.Replace("&amp;amp;", "&amp;");  // just in case there was already an &amp, we fix it up
        outString = outString.Replace(CommonData.c_FROM_XML_APOS, CommonData.c_TO_XML_APOS);
        if (replaceGTLT)
        {
          outString = outString.Replace(CommonData.c_FROM_XML_GT, CommonData.c_TO_XML_GT);
          outString = outString.Replace(CommonData.c_FROM_XML_LT, CommonData.c_TO_XML_LT);
          outString = outString.Replace(CommonData.c_FROM_XML_QUOT, CommonData.c_TO_XML_QUOT);
        }
      }
      return outString;
    }
    public static string cleanXMLString(string inString)
    {
      string outstring = string.Empty;
      if (inString != null)
      {
        outstring = inString.Replace("'", "");
        outstring = outstring.Replace("&", "and");
      }
      return outstring;
    }
    public static string getCurrentPath()
    {
      string AssemblyPath;
      AssemblyPath = global::System.IO.Path.GetDirectoryName(global::System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
      if (AssemblyPath.Contains("VisualStudio"))  // this is design time, not run time
        AssemblyPath = "C:\\Working Folders\\ACG";
      return AssemblyPath.Replace("file:\\", "");
    }
    /// <summary>
    /// Makes the first character of a string upper case and the rest lower case
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string toProperCase(string s)
    {
      if (s == null || s.Length == 0)
        return s;
      StringBuilder sb = new StringBuilder(s.ToLower());
      sb[0] = s[0].ToString().ToUpper().ToCharArray()[0];
      return sb.ToString();
    }
    public static string getApplicationTitle(string connectionString, string appName)
    {
      string testORprod = "Phoenix";
      if (connectionString.Contains("192.168.102") || connectionString.ToLower().Contains("rs1sql"))
        testORprod = "RackSpace";

      return string.Format("TAG V{0} {1} - {2}({3})", CCIVersion(), appName, CommonData.DatabaseName, testORprod);
    }
    public static string GUIStatusLine()
    {
      return string.Format("Copyright (C) 2012 City Communications Integrated. Version={0}, DBName={1}", CCIVersion(), CommonData.DatabaseName);
    }
    public static string CCIVersion()
    {
      Assembly assem = Assembly.GetEntryAssembly();
      AssemblyName assemName = assem.GetName();
      Version ver = assemName.Version;
      return ver.ToString();
    }
    public static string[] MakeWhereClauseFromName(string fieldName, string name, bool showAll)
    {
      return MakeWhereClauseFromName(fieldName, name, showAll, true);
    }
    public static string[] MakeWhereClauseFromName(string fieldName, string name, bool showAll, bool useOR)
    {
      const string GETALLRECORDS = " WHERE 1 = 1 ";
      const string GETNORECORDS = " WHERE 1 = 0 ";
      string[] returnList = new string[2];
      StringBuilder sbTokenList = new StringBuilder();
      StringBuilder sbWhereClause = new StringBuilder();
      sbWhereClause.Length = 0; // first clear it out
      if (showAll)
        sbWhereClause.Append(GETALLRECORDS); // return all records
      else
      {
        if (string.IsNullOrEmpty(name))
          sbWhereClause.Append(GETNORECORDS);
        else
        {
          /*
           * build a set of "LIKE" clauses to look for matches on ANY token in the name
           */
          sbWhereClause.Append(" WHERE ");
          string[] tokens = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
          bool firstTime = true;
          if (tokens != null && tokens.GetLength(0) > 0)
          {
            for (int i = 0; i < tokens.GetLength(0); i++)
            {
              string token = tokens[i];
              switch (token.ToLower())
              {
                // tokens to ignore
                case "llc":
                case "llc.":
                case "inc":
                case "inc.":
                case "pc":
                case "mr":
                case "md":
                case "mrs":
                case "ms":
                case "jr":
                case "dba":
                case "the":
                  break;
                default:
                  if (token.Length > 2) // don't try to match really short tokens
                  {
                    if (firstTime)
                      firstTime = false;
                    else
                    {
                      if (useOR)
                        sbWhereClause.Append(" OR ");
                      else
                        sbWhereClause.Append(" AND ");
                      sbTokenList.Append(" "); ;
                    }
                    sbWhereClause.Append(fieldName);
                    sbWhereClause.Append(" LIKE '%");
                    sbWhereClause.Append(token.Replace("'","''"));
                    sbWhereClause.Append("%'");
                    sbTokenList.Append(token);
                  }
                  break;
              }
            }
          }
        }

      }
      returnList[0] = sbWhereClause.ToString();
      returnList[1] = sbTokenList.ToString();
      return returnList;
    }
    public static string fixUpStringForSQL(string inString)
    {
      string outString = inString.Replace("'", "''");

      //outString = outString.Replace("\"", "\\\"");
      return outString;
    }

    #endregion

    #region DataType Conversion

    /// <summary>
    /// compares two objects of two datatypes, and returns a value compatible with ICompariable CompareTo(). If DataTypes
    /// are not the same, it converst both to string, and does a string compare. If either one is null, they
    /// are also converted to string using CString (which always returns an empty string for a null) and compared.
    /// </summary>
    /// <param name="o1">First value</param>
    /// <param name="dataType1">DataType of First value</param>
    /// <param name="o2">Second Value</param>
    /// <param name="dataType2">DataType of second value</param>
    /// <returns>-1 if o1 < o2, 0 if o1 == o2, +1 if 01 > 02</returns>
    public static int CompareTo(object o1, string dataType1, object o2, string dataType2)
    {
      string sDataType1 = CommonData.DATATYPESTRING;
      if (dataType1 != null)
        sDataType1 = dataType1.ToLower();
      string sDataType2 = CommonData.DATATYPESTRING;
      if (dataType2 != null)
        sDataType2 = dataType2.ToLower();
      if (sDataType1 != sDataType2 || o1 == null || o2 == null)
        return CString(o1).CompareTo(CString(o2));
      switch (sDataType1)
      {
        case CommonData.DATATYPESTRING:
            return CString(o1).ToLower().CompareTo(CString(o2).ToLower());
        case CommonData.DATATYPEBOOLEAN:
          return CBoolean(o1).CompareTo(CBoolean(o2));
        case CommonData.DATATYPEDATETIME:
          // is o1 null (or "null")?
          string strO1 = CString(o1).ToLower();
          bool o1isNull = (strO1 == string.Empty || strO1 == "null");
          if (!o1isNull)
          {
            DateTime dtO1 = CDateTime(o1);
            o1isNull = (dtO1 == CommonData.FutureDateTime || dtO1 == CommonData.PastDateTime);
          }
          string str02 = CString(o2).ToLower();
          bool o2isNull = (str02 == string.Empty || str02 == "null");
          if (!o2isNull)
          {
            DateTime dt02 = CDateTime(o2);
            o2isNull = (dt02 == CommonData.FutureDateTime || dt02 == CommonData.PastDateTime);
          }
          if (o1isNull) // if so..., then check o2
          {
            if (o2isNull)
              return 0;   // both are null, so they match
            else
              return -1;  // we say null is less than any "real" date
          }
          else
            if (o2isNull)
              return +1;  // same thing
            else
              return CDateTime(o1).CompareTo(CDateTime(o2));    // do a regular compare if they are not null
        case CommonData.DATATYPEDECIMAL:
        case "money":
          return CDecimal(o1).CompareTo(CDecimal(o2));
        case CommonData.DATATYPEINTEGER:
          return CInt(o1).CompareTo(CInt(o2));
        case "long":
          return CLng(o1).CompareTo(CLng(o2));
        default:
          return CString(o1).ToLower().CompareTo(CString(o2).ToLower());
      }
    }
    /// <summary>
    /// Standard conversion function that never returns a null.
    /// </summary>
    /// <param name="o">object that can be converted to a Decimal</param>
    /// <returns>Returns a zero if an error occurs or if o is null.</returns>
    public static decimal CDecimal(object o)
    {
      if (o == null || o == System.DBNull.Value || o.ToString().Length == 0)
        return 0;
      if (o.GetType() == typeof(decimal))
        return (decimal)o;
      decimal number = 0;
      string s = o.ToString();
      int iDollar = s.IndexOf('$');
      if (iDollar >= 0)
      {
        s = s.Substring(0, iDollar) + s.Substring(iDollar + 1);
      }
      if (s.StartsWith("(") && s.EndsWith(")"))
        s = "-" + s.Substring(1, s.Length - 2);
      if (decimal.TryParse(s, out number))
        number = Convert.ToDecimal(s);
      else
      {
        if (CommonData.ThrowErrorOnDataTypeConversion)
        {
          ExceptionMessage tm = new ExceptionMessage("CommonFunctions", "CDecimal", "Cannot convert to this datatype");
          tm.AddParm(o);
          throw new Exception(tm.ToString());
        }
      }

      return number;
    }

    /// <summary>
    /// Overload which allows specification of a value to return if conversion is not successful
    /// </summary>
    /// <param name="o">object that can be converted to a Decimal</param>
    /// <param name="defaultValue">Decimal default value</param>
    /// <returns>returns defaultValue if an error occurs or o is null</returns>
    public static decimal CDecimal(object o, decimal defaultValue)
    {
      if (o == null || o == System.DBNull.Value)
        return defaultValue;
      if (o.GetType() == typeof(decimal))
        return (decimal)o;
      decimal number = defaultValue;

      if (decimal.TryParse(o.ToString(), out number))
        number = Convert.ToDecimal(o);

      return number;
    }

    /// <summary>
    /// Standard conversion function that never returns a null.
    /// </summary>
    /// <param name="o">object that can be converted to a double</param>
    /// <returns>Returns a zero if an error occurs or if o is null.</returns>
    public static double CDouble(object o)
    {
      if (o == null || o == System.DBNull.Value || o.ToString().Length == 0)
        return 0;
      if (o.GetType() == typeof(double))
        return (double)o;
      double number = 0;

      if (Double.TryParse(o.ToString(), out number))
        number = Convert.ToDouble(o);
      else
      {
        if (CommonData.ThrowErrorOnDataTypeConversion)
        {
          ExceptionMessage tm = new ExceptionMessage("CommonFunctions", "CDouble", "Cannot convert to this datatype");
          tm.AddParm(o);
          throw new Exception(tm.ToString());
        }
      }
      return number;
    }

    /// <summary>
    /// Overload which allows specification of a value to return if conversion is not successful
    /// </summary>
    /// <param name="o">object that can be converted to a double</param>
    /// <param name="defaultValue">double default value</param>
    /// <returns>returns defaultValue if an error occurs or o is null</returns>
    public static double CDouble(object o, double defaultValue)
    {
      if (o == null || o == System.DBNull.Value)
        return defaultValue;
      if (o.GetType() == typeof(double))
        return (double)o;
      double number = defaultValue;

      if (Double.TryParse(o.ToString(), out number))
        number = Convert.ToDouble(o);

      return number;
    }

    /// <summary>
    /// Standard conversion function that never returns a null.
    /// </summary>
    /// <param name="o">object that can be converted to a CDateTime</param>
    /// <returns>Returns the current datetime if an error occurs or if o is null.</returns>
    public static DateTime CDateTime(object o)
    {
      /*
       * TODO lmv66 2012 0904 1038: We have the LIST of VALID Default formats in the cultureInfo object, but in this moment
       * this does not have the Long Format that we are using, we will discuss in the future how this affects
       * us, and how we can take advantange of this existing list so as for us to be "more" standard
       */
      DateTime defaultValue = CommonData.PastDateTime;
      //string[] validDateFormatList = CommonData.cultureInfoUS.DateTimeFormat.GetAllDateTimePatterns();
      string[] validDateFormatList = new string[] { CommonData.FORMATLONGDATETIME, CommonData.FORMATSHORTDATE };
      return CDateTime(o, defaultValue, validDateFormatList);
    }

    /// <summary>
    /// Standard conversion function that never returns a null.
    /// </summary>
    /// <param name="o">object that can be converted to a CDateTime</param>
    /// <returns>Returns the defaultDateTime if an error occurs or if o is null.</returns>
    public static DateTime CDateTime(object o, DateTime defaultValue)
    {

      string[] validDateFormatList = new string[] { CommonData.FORMATLONGDATETIME, CommonData.FORMATSHORTDATE };
      return CDateTime(o, defaultValue, validDateFormatList);
    }

    /// <summary>
    /// Standard conversion function that never returns a null.
    /// </summary>
    /// <param name="o">object that can be converted to a CDateTime</param>
    /// <returns>Returns the defaultDateTime if an error occurs or if o is null.</returns>
    public static DateTime CDateTime(object o, DateTime defaultValue, string[] validDateFormatList)
    {
      if (o == null || o == System.DBNull.Value || o.ToString() == string.Empty)
        return defaultValue;
      if (o.GetType() == typeof(DateTime))
        return (DateTime)o;
      if (o.GetType() == typeof(TimeSpan))
        return CDateTimeFromTime((TimeSpan)o);
      string sDate = o.ToString();
      if (sDate.Length > 3 && sDate.Substring(2, 1) == ":") // this is a time format
        return CDateTimeFromTime(CTime(sDate));
      else
      {
        string[] bigValidDateFormatList = CommonData.cultureInfoUS.DateTimeFormat.GetAllDateTimePatterns();
        DateTime dateTime = defaultValue;
        if (DateTime.TryParseExact(sDate, validDateFormatList, CommonData.cultureInfoUS, System.Globalization.DateTimeStyles.None, out dateTime))
          return dateTime;
        else if (DateTime.TryParseExact(sDate, bigValidDateFormatList, CommonData.cultureInfoUS, System.Globalization.DateTimeStyles.None, out dateTime))
          return dateTime;
      }
      return defaultValue;
    }
    public static DateTime CDateTimeFromTime(TimeSpan t)
    {
      return new DateTime(CommonData.PastDateTime.Year, CommonData.PastDateTime.Month, CommonData.PastDateTime.Day, t.Hours, t.Minutes, t.Seconds);
    }
    public static TimeSpan CTime(object o)
    {
      DateTime now = DateTime.Now;
      TimeSpan defaultValue = new TimeSpan(now.Hour, now.Minute, now.Second, now.Millisecond);
      return CTime(o, defaultValue);
    }
    public static TimeSpan CTime(object o, TimeSpan defaultValue)
    {
      if (o == null || o == System.DBNull.Value || o.ToString() == string.Empty)
        return defaultValue;
      if (o.GetType() == typeof(TimeSpan))
        return (TimeSpan)o;
      TimeSpan time = defaultValue;
      if (TimeSpan.TryParse(o.ToString(), out time))
        return time;
      return defaultValue;
    }

    /// <summary>
    /// Standard conversion function that never returns a null.
    /// </summary>
    /// <param name="o">object that can be converted to an Int32</param>
    /// <returns>Returns a zero if an error occurs or if o is null.</returns>
    public static int CInt(object o)
    {
      return CInt(o, 0);
    }

    public static long CLng(object o, long defaultValue)
    {
      if (o == null || o == System.DBNull.Value || o.ToString().Length == 0)
        return defaultValue;
      if (o.GetType() == typeof(long))
        return (long)o;
      long number = defaultValue;
      double testNumber;
      if (o == null || o == System.DBNull.Value)
        return defaultValue;
      if (double.TryParse(o.ToString(), out testNumber))
        number = Convert.ToInt64(Math.Round(Convert.ToDouble(o.ToString()), 0));

      return number;
    }

    /// <summary>
    /// Overload with default value = 0, and throws an error if (CommonData.ThrowErrorOnDataTypeConversion)
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public static long CLng(object o)
    {
      long defaultValue = 0;

      if (o == null || o == System.DBNull.Value || o.ToString().Length == 0)
        return defaultValue;
      if (o.GetType() == typeof(long))
        return (long)o;
      long number = defaultValue;
      double testNumber; if (double.TryParse(o.ToString(), out testNumber))
        number = Convert.ToInt64(Math.Round(Convert.ToDouble(o.ToString()), 0));
      else
      {
        if (CommonData.ThrowErrorOnDataTypeConversion)
        {
          ExceptionMessage tm = new ExceptionMessage("CommonFunctions", "CLng", "Cannot convert to this datatype");
          tm.AddParm(o);
          throw new Exception(tm.ToString());
        }
      }
      return number;
    }

    /// <summary>
    /// overload that allows specification of the value to use if conversion is unsuccessful
    /// </summary>
    /// <param name="o"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static int CInt(object o, int defaultValue)
    {
      if (o == null || o == System.DBNull.Value)
        return defaultValue;
      if (o.GetType() == typeof(int))
        return (int)o;
      int number = defaultValue;
      double testNumber;
      if (double.TryParse(o.ToString(), out testNumber))
        number = Convert.ToInt32(Math.Round(Convert.ToDouble(o.ToString()), 0));

      return number;
    }

    /// <summary>
    /// Standard conversion function that never returns a null.
    /// </summary>
    /// <param name="o">object that can be converted to a boolean</param>
    /// <returns>Returns a false if an error occurs or if o is null.</returns>
    public static bool CBoolean(object o)
    {
      return CBoolean(o, false);
    }

    /// <summary>
    /// overload that allows specification of the value to use if conversion is unsuccessful
    /// </summary>
    /// <param name="o"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static bool CBoolean(object o, bool defaultValue)
    {
      if (o == null || o == System.DBNull.Value)
        return defaultValue;
      else
      {
        if (o.GetType() == typeof(bool))
          return (bool)o;
        switch (o.ToString().ToLower())
        {
          case "yes":
          case "true":
          case "y":
          case "t":
            //case "1":
            return true;
          case "no":
          case "false":
          case "n":
          case "f":
            //case "0":
            return false;
          default:
            bool testValue;
            if (Boolean.TryParse(o.ToString(), out testValue))
              return Convert.ToBoolean(o);
            else
              return defaultValue;
        }
      }
    }

    /// <summary>
    /// Standard conversion function that never returns a null.
    /// </summary>
    /// <param name="o">object that can be converted to a boolean</param>
    /// <returns>Returns an empty string if an error occurs or if o is null.</returns>
    public static string CString(object o)
    {
      return CString(o, string.Empty, CommonData.FORMATLONGDATETIME);
    }

    /// <summary>
    /// overload that allows specification of the value to use if conversion is unsuccessful
    /// </summary>
    /// <param name="o"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string CString(object o, string defaultValue)
    {
      return CString(o, defaultValue, CommonData.FORMATLONGDATETIME);
    }

    /// <summary>
    /// overload that allows specification of the value to use if conversion is unsuccessful
    /// </summary>
    /// <param name="o"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static string CString(object o, string defaultValue, string dateTimeFormat)
    {
      if (o == null || o == System.DBNull.Value)
        return defaultValue;
      else
        if (o.GetType() == typeof(DateTime))
        {
          //// check to see if this has a time or only a date
          //DateTime dt = (DateTime)o;
          //string dtShort = dt.ToShortDateString();
          //if (Convert.ToDateTime(dtShort).Equals(dt))
          //  return dtShort; // return just the date
          //else
          //  return dt.ToString(); // return the date and time

          DateTime dt = (DateTime)o;
          string dtShortString = dt.ToString(CommonData.FORMATSHORTDATE);

          DateTime dtShort = DateTime.Now;
          if(DateTime.TryParseExact(dtShortString, new string[]{ CommonData.FORMATSHORTDATE }, CommonData.cultureInfoUS, System.Globalization.DateTimeStyles.None, out dtShort))
          {
            if(dtShort.Equals(dt))
              return dtShortString;
            else
              return dt.ToString(dateTimeFormat); // return the date and time        
          }
          else
            return dt.ToString(dateTimeFormat); // return the date and time        
        }
        else
          return o.ToString();
    }


    /// <summary>
    /// Take a string token, and attempt to convert first to a DateTime, then to a numeric (double).
    /// If neither were successfull, then make it a string. Take the result and return it as an object.
    /// </summary>
    /// <param name="strValue"></param>
    /// <returns></returns>
    public static object toValue(string strValue)
    {
      /*
       * This routine takes a string token, and converts it to a numeric or date data type
       * if possible. This enables objects to be compared using other than string comparisons 
       * if applicable. Try datetime first. If it doesn work, try numeric. If that 
       * doesn't work, just assume it is a string
       */
      DateTime dt;
      decimal dec;
      object retValue = null;
      if (decimal.TryParse(strValue, out dec))
        retValue = dec;
      else
        if (DateTime.TryParse(strValue, out dt))
          retValue = dt;
        else
          if (IsBoolean(strValue))
            retValue = CBoolean(strValue);
          else
            retValue = strValue;
      return retValue;
    }

    /// <summary>
    /// overload of toValue which allows the calling program to specify the type to convert to
    /// </summary>
    /// <param name="strValue"></param>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public static object toValue(object oValue, string typeName)
    {
      return toValue(oValue, typeName, defaultValue(typeName));
    }

    /// <summary>
    /// Overload that allows us to throw an exception if the data type is not compatible
    /// </summary>
    /// <param name="oValue"></param>
    /// <param name="typeName"></param>
    /// <param name="throwError"></param>
    /// <returns></returns>
    public static object toValue(object oValue, string typeName, bool throwError)
    {
      object defaultVal = null;
      if (!throwError)
        defaultVal = defaultValue(typeName);
      return toValue(oValue, typeName, defaultValue(typeName), throwError);
    }

    /// <summary>
    /// Overload of toValue that also allows specification of a default value if conversion is not successfull
    /// </summary>
    /// <param name="oValue"></param>
    /// <param name="typeName"></param>
    /// <param name="pDefaultValue"></param>
    /// <returns></returns>
    public static object toValue(object oValue, string typeName, object pDefaultValue)
    {
      return toValue(oValue, typeName, pDefaultValue, false);
    }

    /// <summary>
    /// Main toValue routine
    /// </summary>
    /// <param name="oValue"></param>
    /// <param name="typeName"></param>
    /// <param name="defaultValue"></param>
    /// <param name="throwError">Should we throw an exception if data is not of a compatible type?</param>
    /// <returns></returns>
    public static object toValue(object oValue, string typeName, object pDefaultValue, bool throwError)
    {
      try
      {
        bool hasError = false;  // if throwError is false, then we don't flag an error, so we set hasError to throwError if a conversion error occurs
        object retValue = string.Empty;
        switch (typeName.ToLower())
        {
          case CommonData.DATATYPESTRING:
            retValue = (object)CString(oValue, (string)pDefaultValue);
            break;
          case CommonData.DATATYPEMONEY:
          case CommonData.DATATYPEDECIMAL:
            if (IsNumeric(oValue))
              retValue = (object)CDecimal(oValue, (decimal)pDefaultValue);
            else
            {
              retValue = oValue;
              hasError = (throwError && CommonData.ThrowErrorOnDataTypeConversion);
            }
            break;
          case CommonData.DATATYPEDOUBLE:
            if (IsNumeric(oValue))
              retValue = (object)CDouble(oValue, (double)pDefaultValue);
            else
            {
              retValue = oValue;
              hasError = (throwError && CommonData.ThrowErrorOnDataTypeConversion);
            }
            break;
          case CommonData.DATATYPEINTEGER:
            if (IsNumeric(oValue))
              retValue = (object)CInt(oValue, (int)pDefaultValue);
            else
            {
              retValue = oValue;
              hasError = (throwError && CommonData.ThrowErrorOnDataTypeConversion);
            }
            break;
          case CommonData.DATATYPELONG:
            if (IsNumeric(oValue))
              retValue = (object)CLng(oValue, (long)pDefaultValue);
            else
            {
              retValue = oValue;
              hasError = (throwError && CommonData.ThrowErrorOnDataTypeConversion);
            }
            break;
          case CommonData.DATATYPEDATETIME:
            if (IsDateTime(oValue))
              retValue = (object)CDateTime(oValue, (DateTime)pDefaultValue);
            else
            {
              retValue = oValue;
              hasError = (throwError && CommonData.ThrowErrorOnDataTypeConversion);
            }
            break;
          case CommonData.DATATYPEBOOLEAN:
            if (IsBoolean(oValue))
              retValue = (object)CBoolean(oValue, (bool)pDefaultValue);
            else
            {
              retValue = oValue;
              hasError = (throwError && CommonData.ThrowErrorOnDataTypeConversion);
            }
            break;
          case CommonData.DATATYPEOBJECT:
            retValue = oValue;
            break;
          default:
            hasError = (throwError && CommonData.ThrowErrorOnDataTypeConversion);
            retValue = (object)CString(oValue, (string)pDefaultValue);
            break;
        }
        if (hasError)
          throw new Exception("DataType conversion error");
        else
          return retValue;
      }
      catch // we will generate a catch if defaultValue cannot be cast to the correct data type
      {
        if (throwError && CommonData.ThrowErrorOnDataTypeConversion)
          throw new Exception("DataType conversion error");
        if (oValue == null)
          return string.Empty;
        else
          return oValue.ToString();
      }
    }

    /// <summary>
    /// returns a default value appropriate for the typename in the correct type, boxed in an object
    /// </summary>
    /// <param name="typeName"></param>
    /// <returns></returns>
    public static object defaultValue(string typeName)
    {
      switch (typeName.ToLower())
      {
        case CommonData.DATATYPESTRING:
          return string.Empty;
        case CommonData.DATATYPEINTEGER:
          return (int)0;
        case CommonData.DATATYPELONG:
          return (long)0;
        case CommonData.DATATYPEDOUBLE:
          return (double)0;
        case CommonData.DATATYPEMONEY:
        case CommonData.DATATYPEDECIMAL:
          return (decimal)0;
        case CommonData.DATATYPEBOOLEAN:
          return false;
        case CommonData.DATATYPEDATETIME:
          return CommonData.PastDateTime;
        case CommonData.DATATYPEOBJECT:
          return null;
        default:
          return null;
      }
    }

    /// <summary>
    /// Does this object contain a value that can be interpreted as a integer value?
    /// </summary>
    /// <param name="o">object that evaluate to convert to an int</param>
    /// <returns>Returns true if the object is a valid int number</returns>
    public static bool IsInteger(object o)
    {
      bool result = false;
      if (o == null || o == System.DBNull.Value)
        return true;
      Type t = o.GetType();
      if (t == typeof(int))
        return true;
      if (t == typeof(long))
        return true;
      if (t == typeof(string) && (string)o == string.Empty)
        return true;
      string original = o.ToString();
      decimal number = 0;
      if (Decimal.TryParse(o.ToString(), out number)) // If the object can parse it, perhaps is a valid Integer
      {
        if (number > Int32.MaxValue)
          result = false;
        else
        {
          number = Decimal.Truncate(number);
          if (string.Format("{0:0.00}", Convert.ToDecimal(original)) == string.Format("{0:0.00}", number))
            result = true;
        }
      }

      return result;
    }

    /// <summary>
    /// Does this object contain a value that can be interpreted as a numeric value?
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public static bool IsNumeric(object o)
    {
      if (o == null || o == System.DBNull.Value)
        return true;
      Type t = o.GetType();
      if (t == typeof(decimal))
        return true;
      if (t == typeof(int))
        return true;
      if (t == typeof(long))
        return true;
      if (t == typeof(double))
        return true;
      decimal d;
      string tryString = CString(o);
      if (tryString == string.Empty)
        return false;
      if (decimal.TryParse(tryString, out d))
        return true;
      else
        return false;
    }
    /// <summary>
    /// Does this object contain a value that can be interpreted as a boolean value?
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public static bool IsBoolean(object o)
    {
      bool defaultValue = false;
      if (o == null || o == System.DBNull.Value)
        return false;
      else
      {
        switch (o.ToString().ToLower())
        {
          case "yes":
          case "true":
          case "y":
          case "t":
          //case "1":
          case "no":
          case "false":
          case "n":
          case "f":
            //case "0":
            return true;
          default:
            if (Boolean.TryParse(o.ToString(), out defaultValue))
              return true;
            else
              return false;
        }
      }
    }
    /// <summary>
    /// Does this object contain a value that can be interpreted as a DateTime value?
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public static bool IsDateTime(object o)
    {
      if (o == null || o == System.DBNull.Value)
        return false;
      string typeName = o.GetType().Name.ToLower();
      if (typeName == CommonData.DATATYPEDATETIME)
        return true;
      if (typeName == CommonData.DATATYPEDECIMAL || typeName == CommonData.DATATYPEINTEGER)
        return false;
      DateTime dtResult;
      if (DateTime.TryParse(o.ToString(), out dtResult))
        return true;
      else
        return false;
    }
    /// <summary>
    /// More efficient way of testing an attribute or field value to see if it is null or empty,
    /// that does NOT call the ToString() method. ToString() can be  slow if the value
    /// is a large tableheader
    /// </summary>
    /// <param name="value"></param>
    /// <returns>True if null or type of string and empty. Otherwise, false</returns>
    public static bool IsNullOrStringEmpty(object value)
    {
      if (value == null)
        return true;
      if (value.GetType() == typeof(string))
        return string.IsNullOrEmpty((string)value);
      else
        return false;
    }

    /// <summary>
    /// is this resource an event resource?
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public static bool IsEventResource(string resource)
    {
      if (resource == null)
        return false;
      return resource.StartsWith(CommonData.RESOURCELOCKEVENTPREFIX);
    }

    public static string DataType(object o)
    {
      if (o == null || o == System.DBNull.Value)
        return CommonData.DATATYPESTRING;
      //TODO: lmv66: need to review this with Larry!!
      if (IsNumeric(o) && CDecimal(o).ToString() == o.ToString())
        return CommonData.DATATYPEDECIMAL;
      if (IsInteger(o) && CInt(o).ToString() == o.ToString())
        return CommonData.DATATYPEINTEGER;
      if (IsDateTime(o))
        return CommonData.DATATYPEDATETIME;
      if (IsBoolean(o))
        return CommonData.DATATYPEBOOLEAN;
      string s = o.ToString();
      return CommonData.DATATYPESTRING;
    }

    /// <summary>
    /// Used to convert SQL Type name to Variables type name
    /// </summary>
    /// <param name="dataType"></param>
    /// <returns></returns>
    public static string MapSQLToVar(SqlDbType dataType)
    {
      string type;
      switch (dataType)
      {
        case SqlDbType.VarChar:
          type = CommonData.DATATYPESTRING;
          break;
        case SqlDbType.Decimal:
          type = CommonData.DATATYPEDECIMAL;
          break;
        case SqlDbType.Float:
          type = CommonData.DATATYPEDOUBLE;
          break;
        case SqlDbType.DateTime:
          type = CommonData.DATATYPEDATETIME;
          break;
        case SqlDbType.Int:
          type = CommonData.DATATYPEINTEGER;
          break;
        case SqlDbType.Bit:
          type = CommonData.DATATYPEBOOLEAN;
          break;
        case SqlDbType.BigInt:
          type = CommonData.DATATYPELONG;
          break;
        case SqlDbType.Money:
          type = CommonData.DATATYPEMONEY;
          break;
        default:
          type = CommonData.DATATYPEVARCHAR;
          break;
      }

      return type;
    }

    /// <summary>
    /// Convert variable type name into SQL Type name
    /// </summary>
    /// <param name="dataType"></param>
    /// <returns></returns>
    public static SqlDbType MapVarToSQL(string dataType)
    {
      SqlDbType type;
      switch (dataType.ToLower())
      {
        case CommonData.DATATYPESTRING:
          type = SqlDbType.VarChar;
          break;
        case CommonData.DATATYPEDECIMAL:
          type = SqlDbType.Decimal;
          break;
        case CommonData.DATATYPEDOUBLE:
          type = SqlDbType.Float;
          break;
        case CommonData.DATATYPEDATETIME:
          type = SqlDbType.DateTime;
          break;
        case CommonData.DATATYPEINTEGER:
          type = SqlDbType.Int;
          break;
        case CommonData.DATATYPEBOOLEAN:
          type = SqlDbType.Bit;
          break;
        case CommonData.DATATYPELONG:
          type = SqlDbType.BigInt;
          break;
        case CommonData.DATATYPEMONEY:
          type = SqlDbType.Money;
          break;
        default:
          type = SqlDbType.VarChar;
          break;
      }
      return type;
    }
    public static Type dsDataType(string dataType)
    {
      Type returnType = System.Type.GetType("System.String");   // default is string
      if (dataType == null)
        return returnType;
      switch (dataType.ToLower())
      {
        case CommonData.DATATYPEINTEGER:
          returnType = System.Type.GetType("System.Int32");
          break;
        case CommonData.DATATYPEDOUBLE:
          returnType = System.Type.GetType("System.Double");
          break;
        case CommonData.DATATYPEDECIMAL:
          returnType = System.Type.GetType("System.Decimal");
          break;
        case CommonData.DATATYPEDATETIME:
          returnType = System.Type.GetType("System.DateTime");
          break;
        case CommonData.DATATYPEBOOLEAN:
          returnType = System.Type.GetType("System.Boolean");
          break;
      }
      return returnType;
    }
    #endregion DataType Conversion

    #region date functions
    /// <summary>
    /// Function that receives the month and returns in wich quarter is a date.
    /// </summary>
    /// <param name="month"></param>
    /// <returns></returns>
    public static string quarter(int month)
    {
      string quarterDate = "";
      if (month >= 1 && month <= 3)
      {
        return quarterDate = "1";
      }
      else if (month >= 4 && month <= 6)
      {
        return quarterDate = "2";
      }
      else if (month >= 7 && month <= 9)
      {
        return quarterDate = "3";
      }
      else if (month >= 10 && month <= 12)
      {
        return quarterDate = "4";
      }

      return "";
    }
    /// <summary>
    /// True if this is a weekday that is not a US holiday. False if it is a weekday or holiday.
    /// </summary>
    /// <param name="dtDate"></param>
    /// <returns></returns>
    public static bool IsBusinessDate(DateTime dateIn)
    {
      bool bTmp = true;

      if ((dateIn.DayOfWeek == DayOfWeek.Sunday) || (dateIn.DayOfWeek == DayOfWeek.Saturday)) // Weekend
        return false;

      if ((dateIn.Month == 1) && (dateIn.Day == 1)) // New Years
        return false;

      if ((dateIn.Month == 1) && (dateIn.Day == 2) && (dateIn.DayOfWeek == DayOfWeek.Monday)) // New Years
        return false;

      if ((dateIn.Month == 1) && (dateIn.DayOfWeek == DayOfWeek.Monday) && ((dateIn.Day >= 15) || (dateIn.Day <= 21)))  // MLK - 3rd Monday in Jan
        return false;

      if ((dateIn.Month == 2) && (dateIn.DayOfWeek == DayOfWeek.Monday) && ((dateIn.Day >= 15) || (dateIn.Day <= 21))) // Presidents Day - 3rd Monday in Feb.
        return false;

      if ((dateIn.Month == 5) && (dateIn.DayOfWeek == DayOfWeek.Monday) && (dateIn.Day > 24)) // Memorial Day Last Monday in May
        return false;

      if ((dateIn.Month == 7) && (dateIn.Day == 4)) // July 4
        return false;

      if ((dateIn.Month == 7) && (dateIn.Day == 5) && (dateIn.DayOfWeek == DayOfWeek.Monday)) // July 4
        return false;

      if ((dateIn.Month == 9) && (dateIn.DayOfWeek == DayOfWeek.Monday) && ((dateIn.Day >= 1) || (dateIn.Day <= 7))) // Labors Day - 1st Monday in Sept.
        return false;

      if ((dateIn.Month == 10) && (dateIn.DayOfWeek == DayOfWeek.Monday) && ((dateIn.Day >= 8) || (dateIn.Day <= 14))) // Columbus Day - 2nd Monday in Oct.
        return false;

      if ((dateIn.Month == 11) && (dateIn.Day == 11)) // Veterans Day.
        return false;

      if ((dateIn.Month == 11) && (dateIn.Day == 12) && (dateIn.DayOfWeek == DayOfWeek.Monday)) // Veterans Day.
        return false;

      if ((dateIn.Month == 11) && (dateIn.DayOfWeek == DayOfWeek.Thursday) && ((dateIn.Day >= 22) && (dateIn.Day <= 28))) // Thanksgiving Day - 4th Thursday in Nov.
        return false;

      if ((dateIn.Month == 12) && (dateIn.Day == 25)) // Chritmass.
        return false;

      if ((dateIn.Month == 12) && (dateIn.Day == 26) && (dateIn.DayOfWeek == DayOfWeek.Monday)) // Chritmass.
        return false;

      return bTmp;
    }
    /// <summary>
    /// Get the number of days to adjust a date.
    /// </summary>
    /// <param name="DayofWeek">What day of week are we?</param>
    /// <param name="iType">What kind of adjustment do we make?
    /// <para>0 = move to the next Wed or Friday
    /// </para>
    /// <para>1 = If this is a weekend, move to the next monday</para>
    /// </param>
    /// <returns></returns>
    public static int GetNoDays(global::System.DayOfWeek DayofWeek, int iType)
    {
      int intTmp = 0;
      if (iType == 0)
      {
        switch (DayofWeek)
        {
          case global::System.DayOfWeek.Sunday: intTmp = 5; break;
          case global::System.DayOfWeek.Monday: intTmp = 4; break;
          case global::System.DayOfWeek.Tuesday: intTmp = 3; break;
          case global::System.DayOfWeek.Wednesday: intTmp = 7; break;
          case global::System.DayOfWeek.Thursday: intTmp = 6; break;
          case global::System.DayOfWeek.Friday: intTmp = 5; break;
          case global::System.DayOfWeek.Saturday: intTmp = 6; break;
        }
      }
      else
      {
        switch (DayofWeek)
        {
          case global::System.DayOfWeek.Saturday: intTmp = 2; break;
          case DayOfWeek.Sunday: intTmp = 1; break;
          default: intTmp = 0; break;
        }
      }

      return intTmp;
    }
    public static int getPeriod(DateTime dtDate, string period)
    {
      switch (period)
      {
        case "m":
          return dtDate.Month;
        case "y":
          return dtDate.Year;
        case "q":
          return Convert.ToInt16(Math.Round((Convert.ToDouble(dtDate.Month) + 1) / 3, 0));
      }
      return 0;
    }
    public static int daysInPeriod(DateTime dtDate, string period)
    {
      int iDaysInPeriod = 0;
      switch (period)
      {
        case "m":
          iDaysInPeriod = daysInMonth(dtDate);
          break;
        case "q":
          double month = Convert.ToDouble(dtDate.Month);
          int lastQMonth = Convert.ToInt16(Math.Round((month + 1) / 3, 0) * 3);
          int Qdays = 0;
          int iYear = dtDate.Year;
          for (int iMonth = 1; iMonth <= lastQMonth; iMonth++)
            Qdays += daysInMonth(new DateTime(iYear, iMonth, 1));
          iDaysInPeriod = Qdays;
          break;
        case "y":
          DateTime lastDayOfyear = new DateTime(dtDate.Year, 12, 31);
          iDaysInPeriod = lastDayOfyear.DayOfYear;
          break;
      }
      return iDaysInPeriod;
    }
    public static int daysInMonth(DateTime dtDate)
    {
      double adjustDay = 32;
      DateTime nextMonth = dtDate.AddDays(adjustDay);
      return Convert.ToInt16(adjustDay) - nextMonth.Day;
    }
    public static TimeSpan Round(TimeSpan timeIn, string increment)
    {
      TimeSpan timeOut;
      switch (increment)
      {
        case CommonData.TIMEINCREMENTQUARTER:
          double hours = timeIn.TotalHours;
          int quarterHours = Convert.ToInt32(Math.Round(hours * 4.0));
          int iHours = quarterHours / 4;
          int iMinutes = (quarterHours - (iHours * 4)) * 15;
          timeOut = new TimeSpan(iHours, iMinutes, 0);
          break;
        case CommonData.TIMEINCREMENTMINUTE:
          timeOut = new TimeSpan(timeIn.Hours, timeIn.Minutes + Convert.ToInt16(Math.Round(Convert.ToDecimal(timeIn.Seconds) / 60)), 0);
          break;
        case CommonData.TIMEINCREMENTTENTH:
          timeOut = new TimeSpan(timeIn.Hours, Convert.ToInt32(Math.Round(Convert.ToDecimal(timeIn.Minutes)/10)) * 10, 0);
          break;
        default:
          timeOut = timeIn;
          break;
      }
      return timeOut;
    }
    #endregion date functions

    #region parsing functions
    /// <summary>
    /// Can these two types be compared (e.g. datetime to datetime or numeric to numeric)?
    /// </summary>
    /// <param name="t1">Type 1</param>
    /// <param name="t2">Type 2</param>
    /// <returns></returns>
    public static bool canCompare(Type t1, Type t2, out string compareType)
    {
      bool bCanCompare = false;
      if (t1.Name == t2.Name)   // if the two are the same type....
      {
        compareType = t1.Name;
        if (compareType == "Decimal" || compareType.Substring(0, 3) == "Int") // normalize to double if they are numeric
        {
          bCanCompare = true;
          compareType = "Double";
        }
        else
          if (compareType == "DateTime")
            bCanCompare = true;          //dates can be compared
          else
          {
            if (compareType == "String")
              bCanCompare = true;
            else
            {
              compareType = "String";     // anything else, we must do a string compare
              bCanCompare = false;
            }
          }
      }
      else
      {
        if (t1.Name == "Double" || t1.Name == "Decimal" || t1.Name.Substring(0, 3) == "Int")
          if (t2.Name == "Double" || t2.Name == "Decimal" || t2.Name.Substring(0, 3) == "Int")
          {
            compareType = "Double";
            bCanCompare = true;
          }
          else
          {
            compareType = "String";     // anything else, we must do a string compare
            bCanCompare = false;
          }
        else
        {
          compareType = "String";     // anything else, we must do a string compare
          bCanCompare = false;
        }
      }
      compareType = compareType.ToLower();  // normalize to lower case for string comparison
      return bCanCompare;
    }

    public static bool failsTest(string operand1, string op, string operand2, object pValue)
    {
      return !passesTest((object)operand1, op, (object)operand2, pValue);
    }
    /// <summary>
    /// Parses the validation string into its components (operand operator operand) 
    /// </summary>
    /// <param name="validationString"></param>
    /// <param name="operand1"></param>
    /// <param name="op"></param>
    /// <param name="operand2"></param>
    /// <returns></returns>
    public static bool parseValidationString(string validationString, ref string operand1, ref string op, ref string operand2)
    {
      bool isValid = true;
      /*
       * Build the evaluation sentence
       * 
       * OK, so right now we specify a very simple structure. We see the string as three tokens:
       *    operand1, operator, operand2
       * If operand1 does not exist, then 'x' is implied (x represents our attribute value).
       * 1) We look for operand 1 until we see an operator.
       * 2) We get the operator, then start looking for operand 2.
       * 3) we ignore spaces, but trim them in all three at the end.
       * 4) operators are ==, !=, >=, <=, <, >. Note that all but > and < are two characters.
       *  so we assume we are done with the operator after the second character.
       */
      validationString = stripParens(validationString);
      if (isFunction(validationString)) // if this is a valid function
      {
        operand1 = validationString;        // then we don't attempt to parse it, but return is as a unary bool to be resolved
        operand2 = string.Empty;
        op = string.Empty;
        return true;
      }
      const string replaceDelim = "`";
      string tmpValidationString = stripParens(validationString);
      foreach (string opEntry in CommonData.operatorList)
      {
        if (tmpValidationString.Contains(opEntry))
        {
          tmpValidationString = tmpValidationString.Replace(opEntry, replaceDelim + opEntry + replaceDelim);
          break; //added by Leonardo
        }
      }
      string[] delimiters = { replaceDelim };
      string[] tokenList = parseString(tmpValidationString, delimiters);
      int nbrTokens = tokenList.GetLength(0);
      if (nbrTokens == 1) // is this a unary condition?
      {
        operand1 = tokenList[0];
        operand2 = string.Empty;
        op = string.Empty;
      }
      else
      {
        if (nbrTokens == 3)
        {
          operand1 = tokenList[0];
          bool validOp = false;
          op = tokenList[1];
          foreach (string s in CommonData.operatorList)
          {
            if (s.CompareTo(op) == 0)
            {
              validOp = true;
              break;
            }
          }
          operand2 = tokenList[2];
          isValid = validOp;
        }
        else
          isValid = false;
      }

      return isValid;
    }

    public static bool passesTest(object operand1, string op, object operand2)
    {
      return passesTest(operand1, op, operand2, null);
    }

    public static bool passesTest(string operand1, string op, string operand2)
    {
      return passesTest(operand1, op, operand2, null);
    }

    public static bool passesTest(string operand1, string op, string operand2, object pValue)
    {
      return passesTest((object)operand1, op, (object)operand2, pValue);
    }

    /// <summary>
    /// Tests to see of the value in the pValue object meets the condition of operand1 operator operand2.
    /// Since the tests are "failure tests" (test fails if the condition is true), then return
    /// a false (test does not fail) if the condition is not met, and a true (test fails)
    /// if the condition is met
    /// </summary>
    /// <param name="operand1">First operand</param>
    /// <param name="op">Operator</param>
    /// <param name="operand2">Second Operand</param>
    /// <param name="pValue">Object containing the value to test</param>
    /// <returns></returns>
    public static bool passesTest(object pOperand1, string op, object pOperand2, object pValue)
    {
      /*
       * This routine does a compare between two operands and returns true if the test passes and false if it does not.
       */
      //if (pOperand1 == null || pOperand1.ToString() == string.Empty || pOperand1.ToString() == "")
      //  return false;
      //else
      // --Omitting the 1st operand is OK for the dictionary validation string - Leonardo
      if ((pOperand2 == null || pOperand2.ToString() == string.Empty || pOperand2.ToString() == "") &&
        (op == null || op.ToString() == string.Empty || op.ToString() == ""))     // if both op and operand2 are null, then maybe this is a unary boolean
        if (IsBoolean(pOperand1))               // is it something we can convert to booelan?
          return CBoolean(pOperand1);
        else
          return false;                    // the test fails

      bool op1IsString = false;
      bool op2IsString = false;

      bool testPasses = true;
      object o1, o2;
      o1 = toValue(CString(pOperand1));
      o2 = toValue(CString(pOperand2));
      bool bCanCompare = false;
      string myType = CommonData.DATATYPESTRING;
      Type t = o1.GetType();
      op1IsString = (t.Name.ToLower() == CommonData.DATATYPESTRING);
      t = o2.GetType();
      op2IsString = (t.Name.ToLower() == CommonData.DATATYPESTRING);
      string operand1 = pOperand1.ToString().ToLower();
      string operand2 = pOperand2.ToString().ToLower();
      if (pOperand1 == null || operand1 == string.Empty)
        operand1 = "x";
      if (operand1 == "x" || operand1 == "X")
      {
        o1 = pValue;
        //if (op2IsString)
        //    o2 = toValue(operand2);
        //else
        //    o2 = pOperand2;
      }
      else
      {
        if (operand2 == "x" || operand2 == "X")
        {
          o2 = pValue;
        }
      }
      if (o1 == null || o2 == null)
        bCanCompare = false;
      else
        // check to see if the two objects can be compared in numeric or date mode, and set the type for the compare
        bCanCompare = canCompare(o1.GetType(), o2.GetType(), out myType);
      /*
       * We can't compare strings to strings except using ==
       * 
       * So... we use the string.CompareTo(string1, string2) method which returns:
       * 
       *   < 0 if string1 < string2
       *   = 0 if string1 == string2
       *   > 0 if string1 > string2
       *   
       * Then, when we can't do a numeric or datetime compare, we use that value
       * to determine the relationship between strings.
       */
      int tCompare;
      if (pValue == null || pValue.ToString() == "")
        tCompare = operand1.CompareTo(operand2);
      else
        tCompare = o1.ToString().ToLower().CompareTo(o2.ToString().ToLower());

      switch (op)
      {
        case CommonData.EQ:
          if (bCanCompare)
            if (myType == CommonData.DATATYPEDOUBLE)
              testPasses = Convert.ToDouble(o1) == Convert.ToDouble(o2);
            else
              if (myType == CommonData.DATATYPEDATETIME)
                testPasses = Convert.ToDateTime(o1) == Convert.ToDateTime(o2);
              else
                testPasses = (tCompare == 0);
          else
            testPasses = (tCompare == 0);
          break;
        case CommonData.LT:
          if (bCanCompare)
            if (myType == CommonData.DATATYPEDOUBLE)
              testPasses = Convert.ToDouble(o1) < Convert.ToDouble(o2);
            else
              if (myType == CommonData.DATATYPEDATETIME)
                testPasses = Convert.ToDateTime(o1) < Convert.ToDateTime(o2);
              else
                testPasses = (tCompare < 0);
          else
            testPasses = (tCompare < 0);
          break;
        case CommonData.LE:
          if (bCanCompare)
            if (myType == CommonData.DATATYPEDOUBLE)
              testPasses = Convert.ToDouble(o1) <= Convert.ToDouble(o2);
            else
              if (myType == CommonData.DATATYPEDATETIME)
                testPasses = Convert.ToDateTime(o1) <= Convert.ToDateTime(o2);
              else
                testPasses = (tCompare <= 0);
          else
            testPasses = (tCompare <= 0);
          break;
        case CommonData.GT:
          if (bCanCompare)
            if (myType == CommonData.DATATYPEDOUBLE)
              testPasses = Convert.ToDouble(o1) > Convert.ToDouble(o2);
            else
              if (myType == CommonData.DATATYPEDATETIME)
                testPasses = Convert.ToDateTime(o1) > Convert.ToDateTime(o2);
              else
                testPasses = (tCompare > 0);
          else
            testPasses = (tCompare > 0);
          break;
        case CommonData.GE:
          if (bCanCompare)
            if (myType == CommonData.DATATYPEDOUBLE)
              testPasses = Convert.ToDouble(o1) >= Convert.ToDouble(o2);
            else
              if (myType == CommonData.DATATYPEDATETIME)
                testPasses = Convert.ToDateTime(o1) >= Convert.ToDateTime(o2);
              else
                testPasses = (tCompare >= 0);
          else
            testPasses = (tCompare >= 0);
          break;
        case CommonData.NE:
          if (bCanCompare)
            if (myType == CommonData.DATATYPEDOUBLE)
              testPasses = Convert.ToDouble(o1) != Convert.ToDouble(o2);
            else
              if (myType == CommonData.DATATYPEDATETIME)
                testPasses = Convert.ToDateTime(o1) != Convert.ToDateTime(o2);
              else
                testPasses = (tCompare != 0);
          else
            testPasses = (tCompare != 0);
          break;
      }
      return testPasses;
    }

    /// <summary>
    /// Compares two strings. If either one of them is enclosed in single quotes, they are stripped
    /// for the comparison
    /// </summary>
    /// <param name="s1"></param>
    /// <param name="s2"></param>
    /// <returns></returns>
    public static bool stringEquals(string s1, string s2)
    {
      bool isEqual = false;
      string s1Test, s2Test;
      if (s1.Substring(0, 1) == "'")
      {
        s1Test = s1.Substring(1);
        if (s1Test.Substring(s1Test.Length - 1, 1) == "'")
          s1Test = s1Test.Substring(0, s1Test.Length - 1);
      }
      else
        s1Test = s1;
      if (s2.Substring(0, 1) == "'")
      {
        s2Test = s2.Substring(1);
        if (s2Test.Substring(s2Test.Length - 1, 1) == "'")
          s2Test = s2Test.Substring(0, s2Test.Length - 1);
      }
      else
        s2Test = s2;
      return (s1Test == s2Test);
    }

    /// <summary>
    /// Normalizes two objects to see if their values are the same
    /// </summary>
    /// <param name="o1"></param>
    /// <param name="o2"></param>
    /// <returns></returns>
    public static bool objectEquals(object o1, object o2)
    {
      /*
       * If they are both null, then we say they are equal.
       * If one is, but one is not, then they are not equal.
       * If neither is null, then we go on and check normal comarison rules
       */
      if (o1 == null)
        if (o2 == null)
          return true;
        else
          return false;
      else
        if (o2 == null)
          return false;
      bool bEquals = false;           // assume not equal until we decide they are
      // first, check to see if they are comparable types
      string myType;
      Type t1 = o1.GetType();
      Type t2 = o2.GetType();
      bool bCompare = canCompare(t1, t2, out myType);
      if (bCompare)                   // if so, we normalize both to the same type and compare
      {
        switch (myType.ToLower())
        {
          case CommonData.DATATYPESTRING:
            bEquals = (o1.ToString().ToLower() == o2.ToString().ToLower());
            break;
          case CommonData.DATATYPEINTEGER:
            bEquals = (CInt(o1) == CInt(o2));
            break;
          case CommonData.DATATYPEDECIMAL:
            bEquals = (CDecimal(o1) == CDecimal(o2));
            break;
          case CommonData.DATATYPEDOUBLE:
            bEquals = (CDouble(o1) == CDouble(o2));
            break;
          case CommonData.DATATYPEDATETIME:
            bEquals = (CDateTime(o1) == CDateTime(o2));
            break;
          case CommonData.DATATYPEBOOLEAN:
            bEquals = (CBoolean(o1) == CBoolean(o2));
            break;
          default:
            bEquals = (o1.ToString().ToLower() == o2.ToString().ToLower());
            break;
        }
      }
      else
        bEquals = (o1.ToString().ToLower() == o2.ToString().ToLower()); // if not, we convert them both to strings and see if they are alike
      return bEquals;
    }

    /// <summary>
    /// creates the standard action status line from the arguments
    /// </summary>
    /// <param name="actionName"></param>
    /// <param name="workflowStepID"></param>
    /// <param name="securityHandle"></param>
    /// <param name="status"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static string actionStatusLine(string actionName, string workflowStepID, int securityHandle,
      string status, string message)
    {
      string returnMessage = string.Empty;
      if (message != null)
        returnMessage = actionName + ":" + workflowStepID + ":Status=" + status + ":Message=" + message;
      else
        if (status != null)
          returnMessage = actionName + ":" + workflowStepID + ":Status=" + status;
        else
          if (workflowStepID != null)
            returnMessage = actionName + ":" + workflowStepID;
          else
            if (actionName != null)
              returnMessage = actionName;

      returnMessage += ":Handle=" + securityHandle;
      return returnMessage;
    }

    public static bool parseActionStatusLine(string statusLine, out string actionName,
      out string workflowStepID, out string status, out string message, out int securityHandle)
    {
      bool parseOK = false;
      int pos1 = 0;
      int pos2 = 0;
      int pos3 = 0;
      actionName = string.Empty;
      workflowStepID = string.Empty;
      status = string.Empty;
      message = string.Empty;
      securityHandle = 0;
      if (statusLine == null)
        return parseOK;
      pos1 = statusLine.IndexOf(CommonData.cCOLON);
      if (pos1 <= 0)   // no colon was found
        message = "No action name was found";
      else
      {
        actionName = statusLine.Substring(0, pos1);
        pos2 = statusLine.IndexOf(CommonData.cCOLON, pos1 + 1);
        if (pos2 <= 0)
          message = "No workflow step ID was found";
        else
        {
          workflowStepID = statusLine.Substring(pos1 + 1, pos2 - pos1 - 1);
          pos3 = statusLine.IndexOf(CommonData.cCOLON, pos2 + 1);
          if (pos3 <= 0)
          {
            if (pos2 < statusLine.Length - 1) // there is a status but no message
            {
              status = statusLine.Substring(pos2 + 1, statusLine.Length - pos2 - 1);
              int eqPos = status.IndexOf("=");      // parse status from the "Status=xxx" token
              status = status.Substring(eqPos + 1, status.Length - eqPos - 1);
              parseOK = true;
            }
            else
              message = "No status was found";
          }
          else
          {
            status = statusLine.Substring(pos2 + 1, pos3 - pos2 - 1);
            int eqPos = status.IndexOf("=");      // parse status from the "Status=xxx" token
            status = status.Substring(eqPos + 1, status.Length - eqPos - 1);
            message = statusLine.Substring(pos3 + 1, statusLine.Length - pos3 - 1);
            eqPos = message.IndexOf("=");         // parse status from the "Message=xxx" token
            message = message.Substring(eqPos + 1, message.Length - eqPos - 1);
            parseOK = true;
          }
        }
      }
      // TODO: Look for securityhandle here
      string[] args = statusLine.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
      for (int i = 0; i < args.Length; i++)
      {
        if (args[i].Equals("Handle"))
          securityHandle = CInt(args[i].Substring(args[i].IndexOf("=")));
      }
      return parseOK;

    }

    public static string[] parseString(string insideString)
    {
      string[] delimiters = { "|", ",", CommonData.cLISTSEPARATOR };
      return parseString(insideString, delimiters);
    }

    public static string[] parseString(string strString, string[] delimiters)
    {
      /*
       * takes a string of delimited tokens and breaks them down into an array.
       * Note that parentheses are used to indicate a token that should not be broken apart.
       */
      if (string.IsNullOrEmpty(strString))
        return new string[0];
      ArrayList arrList = new ArrayList();
      string[] strList = null;
      int i, j;
      char[] cDelimList = new char[delimiters.GetLength(0)];
      for (i = 0; i < delimiters.GetLength(0); i++)
        cDelimList[i] = Convert.ToChar(delimiters[i]);
      StringBuilder strStringList = new StringBuilder(strString);
      StringBuilder token = new StringBuilder(strString.Length);
      int parenDepth = 0;
      bool InsideDOUBLEQUOTE = false;
      //int delimDepth = 0;
      for (i = 0; i < strStringList.Length; i++)      // walk through the incoming string one character at a time
      {
        switch (strStringList[i])
        {
          case CommonData.cLEFT:                                     // left paren?
          case CommonData.cLEFTCURLY:                                 // or left curly brace '{'
          case CommonData.cLEFTSQUARE:                              // or left square
            parenDepth++;
            token.Append(strStringList[i]);
            break;
          case CommonData.cRIGHT:
          case CommonData.cRIGHTCURLY:
          case CommonData.cRIGHTSQUARE:
            token.Append(strStringList[i]);
            parenDepth--;
            break;
          case CommonData.cDOUBLEQUOTE:
            if (InsideDOUBLEQUOTE)
            {
              token.Append(strStringList[i]);
              parenDepth--;
              InsideDOUBLEQUOTE = false;
            }
            else
            {
              parenDepth++;
              token.Append(strStringList[i]);
              InsideDOUBLEQUOTE = true;
            }
            break;
          default:
            bool delimiterFound = false;
            for (j = 0; j < cDelimList.GetLength(0); j++)
            {
              if (strStringList[i] == cDelimList[j])
              {
                delimiterFound = true;
                break;
              }
            }
            if (delimiterFound)
            {
              if (parenDepth == 0)    // are we outside of a parentheses pair?
              {                       // yes we are...
                string s = token.ToString();     // pick up the token we have created
                arrList.Add(s);       // add the string to the list
                token.Length = 0;     // reset the token to zero length
              }
              else
                token.Append(strStringList[i]); // nope, so just treat it as a character in the token
            }
            else
              token.Append(strStringList[i]); // just another character in the token
            break;
        }

      }
      string s1 = token.ToString();
      arrList.Add(s1);
      strList = new string[arrList.Count];
      for (i = 0; i < arrList.Count; i++)
        strList[i] = arrList[i].ToString();   // load the return array
      return strList;
    }

    public static bool isFunction(string checkString)
    {
      /*
       * this routine checks to see if the string is in the following format:
       * 
       * _funcname(variousnestedtokens, various other nested tokens, etc.)
       * 
       * Some notes:
       * 
       * 1) any number of nested tokens (right/left parenthesis enclosed) are allowed inside
       *    the main tokens.
       * 2) all parens must be paired properly.
       * 3) funcname must be at least one character long
       * 4) funcname must be alpha or numeric characters only
       * 5) white spaces are ignored, except in the funcname, where they are not allowed
       * 6) the main argument section of the function must begin and end with a paired paren
       * 7) no other characters are allowed past the ending paren
       * 8) we trim and convert to lower case so leading and trailing spaces are ignored
       *    and all comparisons are chase insensitive
       * 
       * If all of these criteria are met, this is a valid function formatted string
       */
      if (checkString == null)
        return false;
      bool isF = false;                     // final result: default is not valid
      int parendepth = 0;                   // how many levels are we inside of nested parens
      bool functionNameStarted = false;     // is there a function name
      bool functionNameEnded = false;       // and did it have a proper ending
      bool validFunctionName = true;        // is function name itself valid? assume yes unless we hit a bad char
      bool lastCharIsParen = false;         // is the last character a right paren?
      int numberParenTokens = 0;            // how many "sister" (non-nested) paren-delimited tokens are there
      StringBuilder s = new StringBuilder(checkString.Trim().ToLower());  // prepare the string for comparison
      if (s.Length == 0)
        return false;
      if (s[0] == CommonData.FUNCTIONCHAR)                // does it start with the function prefix?
      {
        for (int i = 1; i < s.Length; i++)
        {
          switch (s[i])
          {
            case CommonData.cLEFT:
              if (functionNameStarted && !functionNameEnded)
                functionNameEnded = true;
              if (parendepth == 0)    // if we are starting a new pair and we are not already in one...
                numberParenTokens++;  // then add to the number of "sister" pairs
              parendepth++;           // we are now one pair deeper
              break;
            case CommonData.cRIGHT:
              if (i == checkString.Length - 1)  // is this the last character?
                lastCharIsParen = true;         // yes, and this is a right paren, so this is true
              parendepth--;           // we are now one pair shallower
              break;
            default:                  // not a paren, so check to see if we are in the function name
              if (!functionNameStarted && !functionNameEnded) // we have not yet started or finished the function name
                functionNameStarted = true;   // so we start it
              // if we are not in the function name, all non-paren chars are ignored
              if (functionNameStarted && !functionNameEnded)  // are we in the function name?
                if (!((s[i] >= '0' && s[i] <= '9') || (s[i] >= 'a' && s[i] <= 'z')))  // yes... not a valid char?
                  validFunctionName = false;                  // then it is a bad function name
              break;
          }
        }
      }
      // else... if we did not start with a function prefix, then we never start a function name
      if (!functionNameStarted)         // if we never got to a functionname
        validFunctionName = false;      // then is is not valid
      if (validFunctionName             // if there was a valid function name
        && numberParenTokens == 1      // and there was exactly one outside paren pair
          && lastCharIsParen            // and there were no chars past the paren pair
            && parendepth == 0)       // and parens were paired evenly
        isF = true;                     // then this is a valid function string
      return isF;
    }

    public static string getFunctionName(string expressionIn, out string insideString)
    {
      string expression = expressionIn.Trim();

      string functionName = string.Empty;
      bool leftFound = true;
      int index = 0;
      int insideLength = expression.Length;
      StringBuilder sbExpression = new StringBuilder(expression);
      StringBuilder token = new StringBuilder(expression.Length);
      if (sbExpression[0] == CommonData.FUNCTIONCHAR)
      {
        index++;
        while (index < sbExpression.Length && sbExpression[index] != CommonData.cLEFT)
          token.Append(sbExpression[index++]);
        if (index == sbExpression.Length || sbExpression[index] != CommonData.cLEFT)
          leftFound = false;
        functionName = token.ToString();
        insideLength = insideLength - index - 2;
        if (sbExpression[sbExpression.Length - 1] != CommonData.cRIGHT || !leftFound)
        {
          functionName = expression.Substring(1);
          insideString = string.Empty;
        }
        else
        {
          insideString = expression.Substring(index + 1, insideLength);
        }
      }
      else
      {
        functionName = expression;
        insideString = "";
        int parametersIndex = expression.IndexOf("(");
        if (parametersIndex >= 0)
        {
          functionName = expression.Substring(0, parametersIndex);
          insideString = expression.Substring(parametersIndex + 1);
          if (insideString.EndsWith(")"))
            insideString = insideString.Substring(0, insideString.Length - 1);
        }
      }
      return functionName;
    }

    public static bool inList(string[] values, string value)
    {
      if (string.IsNullOrEmpty(value) || values == null)
        return false;
      for (int i = 0; i < values.GetLength(0); i++)
        if (values[i] != null && values[i].Equals(value, StringComparison.CurrentCultureIgnoreCase))
          return true;
      return false;
    }

    public static string stripBadCharacters(string name)
    {
      string newName = name;
      foreach (string badChar in CommonData.badFieldCharList)
      {
        name = name.Replace(badChar, "");
      }
      return name;
    }

    public static object stripComment(object val)
    {
      return stripComment(val, true);
    }

    public static object stripComment(object val, bool trimResults)
    {
      string comment;
      return stripComment(val, trimResults, out comment);
    }
    public static object stripComment(object val, bool trimResults, out string comment)
    {
      comment = string.Empty;
      if (val == null)
        return val;
      if (val.GetType() != typeof(string))
        return val;
      string strReturn = val.ToString();
      if (strReturn.StartsWith(CommonData.cLEFT.ToString()) && // is it enclosed in parens? Then don't parse the inside
          strReturn.EndsWith(CommonData.cRIGHT.ToString()))
        return strReturn;
      int loc = strReturn.IndexOf(CommonData.COMMENTCHAR);
      if (loc == -1)
        return strReturn;
      strReturn = strReturn.Substring(0, loc);
      comment = strReturn.Substring(loc + 1);
      if (trimResults)
        strReturn = strReturn.Trim();
      return strReturn;
    }

    public static bool isAttributeReference(object val)
    {
      return beginsWith(val, CommonData.ATTR_1_AT_CHAR);
    }

    public static bool beginsWith(object val, string s)
    {
      if (s == null)
        return false;
      if (val == null)
        return false;
      if (val.GetType() != typeof(string))
        return false;
      return (((string)val).Trim().StartsWith(s));
    }

    public static bool beginsWith(object val, char c)
    {
      return beginsWith(val, c.ToString());
    }

    /// <summary>
    /// used to validate data entry of any id (Item, Entity, etc.)
    /// </summary>
    /// <param name="id">id to be validated</param>
    /// <returns></returns>
    public static string isValidID(string id)
    {
      return isValidID(id, CommonData.VALIDIDLENGTH);
    }

    /// <summary>
    /// used to validate data entry of any id (Item, Entity, etc.)
    /// </summary>
    /// <param name="id">id to be validated</param>
    /// <param name="len">length to validate to</param>
    /// <returns></returns>
    public static string isValidID(string id, int len)
    {
      /*
       * used to validate data entry of any id (Item, Entity, etc.)
       */
      if (id == null || id == string.Empty)
        return "ID cannot be null or empty";
      if (id.Length > len)
        return string.Format("ID must not be greater than {0} characters", len.ToString());
      StringBuilder idTest = new StringBuilder(id);
      for (int i = 0; i < idTest.Length; i++)
        if (!CommonData.validIdCharacters.Contains(idTest[i]))
          return "ID can only have letters, numbers and a dash(-)";
      return string.Empty;
    }

    public static string propercaseItemKey(string itemKey, string originalID)
    {
      if (itemKey.StartsWith(originalID, StringComparison.CurrentCultureIgnoreCase))
      {
        int colonLoc = itemKey.IndexOf(":");
        return string.Format("{0}{1}", originalID, itemKey.Substring(colonLoc, itemKey.Length - colonLoc));
      }
      else
        return itemKey;
    }
    #endregion parsing functions

    #region Table and List Manipulation

    /// <summary>
    /// Sort a 2d Array of objects without a header row
    /// </summary>
    /// <param name="myTable"></param>
    /// <returns></returns>
    public static object[,] Sort(object[,] myTable)
    {
      return Sort(myTable, false);
    }

    /// <summary>
    /// Sort a 2d Array of objects
    /// </summary>
    /// <param name="myTable">The table to sort</param>
    /// <param name="hasHeader">Does this table have a header? if so, don't sort that record.</param>
    /// <returns></returns>
    public static object[,] Sort(object[,] myTable, bool hasHeader)
    {
      /*
       * This routine takes a 2d array of object (commonly used throughout the TAG system)
       * and sorts it on the first column.
       * 
       * It works by using the first column as a key in an arraylist that can be sorted, 
       * and by saving the index for that key in the original table in a hashtable
       * where it can be easily looked up.
       * 
       * Note that this means the sort does not work on tables where the key is not unique.
       */
      object[,] newTable;
      try
      {
        ArrayList sorted = new ArrayList();
        Hashtable h = new Hashtable();
        int i = 0;      // init to zero here instead of in the for(;;) because we may need to override based on hasHeader
        if (hasHeader)    // if we have a header, we skip the header when we create the list of IDs to sort
          i = 1;
        for (; i < myTable.GetLength(0); i++)
        {
          object myKey = myTable[i, 0];
          h.Add(myKey, i);        // connect the key (1st column) to the index of the original table
          sorted.Add(myKey);      // and add the key to the arraylist so it can be sorted
        }
        sorted.Sort();          // sort the keys
        newTable = new object[myTable.GetLength(0), myTable.GetLength(1)];  // create a new table
        i = 0;        // once again, we init the i here instead of in the for(;;)
        if (hasHeader)    // if we have a header, we prepopulate the first row of the new table with the first row from the old table
        {
          for (int j = 0; j < myTable.GetLength(1); j++)  // copy the first row as is into the new table
            newTable[0, j] = myTable[0, j];
          i = 1;      // set the initial row to populate in the new table up to skip the header
        }
        int iSort = 0;    // init iSort separately since it might not be the same as i
        for (; i < myTable.GetLength(0); i++)      // go through the sorted list
        {
          object key = sorted[iSort++];             // get the key
          int oldIndex = (int)h[key];            // and then get the old index for the key
          for (int j = 0; j < myTable.GetLength(1); j++)  // now for each column
            newTable[i, j] = myTable[oldIndex, j];    // copy the cell from the old table into the new
        }
      }
      catch
      {
        newTable = myTable;   // if an error occurs, just return the original table
      }
      return newTable;
    }

    /// <summary>
    /// Overload that allows they first column to be non-unique
    /// </summary>
    /// <param name="myTable"></param>
    /// <param name="hasHeader"></param>
    /// <param name="hasUniqueKey"></param>
    /// <param name="dataType"></param>
    /// <returns></returns>
    public static object[,] Sort(object[,] myTable, bool hasHeader, bool hasUniqueKey, string dataType)
    {
      return Sort(myTable, hasHeader, hasUniqueKey, dataType, 0);
    }

    /// <summary>
    /// Overload that allows they first column to be non-unique, and that allows specification of the column to sort by
    /// </summary>
    /// <param name="myTable">Table to be sorted</param>
    /// <param name="hasHeader">Does this table have a header row?</param>
    /// <param name="hasUniqueKey">Can the key in the sort column be non-unique?</param>
    /// <param name="dataType">Data Type of the sort column for comparisons</param>
    /// <param name="column">Zero-based column to sort on</param>
    /// <returns></returns>
    public static object[,] Sort(object[,] myTable, bool hasHeader, bool hasUniqueKey, string dataType, int column)
    {
      /*
       * this routine allows a table sort even if there are non-unique values in the first column.
       * 
       * It does this by loading the key in the SortKey class, and indexes duplicate keys with 
       * sequence numbers, as in { (key, 0), (key, 1) }. This guarantees order is preserved from the 
       * original table if there are dups in column 1.
       * 
       * We used the SortedDictionary class to sort this for us.
       */
      if (hasUniqueKey && column == 0)     // if the key is unique and we are sorting on the first column
        return Sort(myTable, hasHeader);    // then just do it the old way... it is quicker
      SortedDictionary<SortKey, object[]> sortStructure = new SortedDictionary<SortKey, object[]>();  // define the structure for us to sort
      SortKey lastKey = new SortKey();                // we remember the prior key so we can index if required
      int index = 0;                        // index is always zero unless there is a dup
      int nbrRows = myTable.GetLength(0);
      int nbrCols = myTable.GetLength(1);
      int iRow = 0;                         // init row subscript
      if (hasHeader)                        // if there is a header
        iRow = 1;                           // skip it for the sorted structure
      for (; iRow < nbrRows; iRow++)        // populate the sorted structure
      {
        SortKey thisKey = new SortKey();    // create the key
        thisKey.key = toValue(myTable[iRow, column], dataType);  // convert to the correct datatype and populate the key part
        if (sortStructure.ContainsKey(thisKey)) // if it is a dup
          thisKey.index = ++index;          // increment the index (note ++index increments BEFORE the assignnment
        else
          index = 0;                        // otherwise reset to zero
        lastKey = thisKey;                  // now set lastKey to compare on next iteration
        object[] thisRow = Row(myTable, iRow);  // get this row to store as a value
        sortStructure.Add(thisKey, thisRow);  // and add this key and row to the sorted structure
      }
      object[,] sorted = new object[nbrRows, nbrCols];    // create a new table for the output
      iRow = 0;                             // reset row and col
      int iCol = 0;
      if (hasHeader)                        // if there is a header
      {
        for (iCol = 0; iCol < nbrCols; iCol++)
          sorted[iRow, iCol] = myTable[iRow, iCol]; // populate the header
        iRow = 1;                           // and skip it when we fill from the sorted list
      }
      foreach (KeyValuePair<SortKey, object[]> row in sortStructure)  // this is the recommend way to navigate a sorted dictionary
      {
        object[] thisRow = row.Value;             // get the row
        for (iCol = 0; iCol < nbrCols; iCol++)    // use it to populate the output list
          sorted[iRow, iCol] = thisRow[iCol];
        iRow++;                                   // next row
      }
      return sorted;                      // now return the result

    }

    /// <summary>
    /// Overload with default for hasHeader of false
    /// </summary>
    /// <param name="tableLookup"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static object[] Row(object[,] tableLookup, object key)
    {
      return Row(tableLookup, key, false);
    }

    /// <summary>
    /// Uses a key to lookup the value in the first column of a table and then returns the entire
    /// row as an array of objects
    /// </summary>
    /// <param name="tableLookup"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static object[] Row(object[,] tableLookup, object key, bool hasHeader)
    {
      object[] row = null;                  // define our row as array of columns
      int i = RowSubscript(tableLookup, key, hasHeader);  // look for a match and return the index
      if (i >= 0)                       // did we find one (-1 means not found)
        row = Row(tableLookup, i);              // yes... so get the row
      return row;                       // and return it, whether null or not
    }

    /// <summary>
    /// Returns a row from a table based on the index
    /// </summary>
    /// <param name="tableLookup"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static object[] Row(object[,] tableLookup, int index)
    {
      if (index >= tableLookup.GetLength(0) || index < 0)   // if index is out of range
        return null;                    // then just return a null
      object[] row = new object[tableLookup.GetLength(1)];  // create an array that has the same dimesions as a row
      for (int j = 0; j < tableLookup.GetLength(1); j++)  // for each column
        row[j] = tableLookup[index, j];
      return row;
    }

    /// <summary>
    /// Returns the subscript of the first row in a table whose first column matches "key"
    /// </summary>
    /// <param name="tableLookup"></param>
    /// <param name="key"></param>
    /// <param name="hasHeader"></param>
    /// <returns></returns>
    public static int RowSubscript(object[,] tableLookup, object key, bool hasHeader)
    {
      int sub = -1;  // subscript of the row with the match. -1 means not found
      int i = 0;
      if (hasHeader)
        i = 1;
      for (; i < tableLookup.GetLength(0); i++)
        if (tableLookup[i, 0].ToString() == key.ToString())  // use string compare, do we have a match?
        {
          sub = i;
          break;
        }
      return sub;
    }

    /// <summary>
    /// Find the first row that matchs the key, and remove it from the table. Return the result.
    /// </summary>
    /// <param name="oldTable"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static object[,] RemoveRow(object[,] oldTable, object key)
    {
      object[,] newTable = null;
      int newLength = oldTable.GetLength(0);
      int newWidth = oldTable.GetLength(1);
      for (int i = 0; i < newLength; i++)   // look in each row for the key
      {
        if (oldTable[i, 0].ToString() == key.ToString())  // if we have a match
        {
          // copy all of the old table to the new one except for the deleted row
          newTable = new object[--newLength, oldTable.GetLength(1)];  // redim the new table to one less row
          for (int j = 0; j < newLength; j++) // through the rows again, but to copy them now (note newlength is one less)
          {
            int jOld;
            if (j >= i)
              jOld = j + 1;
            else
              jOld = j;
            for (int k = 0; k < newWidth; k++)    // for each column
              newTable[j, k] = oldTable[jOld, k];
          }
          break;  // and get out of the for loop
        }
      }
      if (newTable == null) // we didn't find a row
        newTable = oldTable;// so make the new one the same as the old
      return newTable;    // return the new table
    }

    public static object[,] AppendTable(object[,] oldTable, object[,] newTable)
    {
      return AppendTable(oldTable, newTable, false, false);
    }

    public static object[,] AppendTable(object[,] oldTable, object[,] newTable, bool hasHeader)
    {
      return AppendTable(oldTable, newTable, hasHeader, hasHeader);
    }

    /// <summary>
    /// Take two tables, append the second to the first, and return it. The resulting table
    /// is as wide as the widest of the two
    /// </summary>
    /// <param name="oldTable"></param>
    /// <param name="newTable"></param>
    /// <returns></returns>
    public static object[,] AppendTable(object[,] oldTable, object[,] newTable, bool oldHasHeader, bool newHasHeader)
    {
      /*
       * There are four possibilities:
       * 
       * 1) Old has header and new does not- This is a simple append
       * 2) Old has header and new has header- this means we do not append the 2nd header
       * 3) old no header and new has header - on the first row of tnew, set beginning = true, and hasheadxer = true 
       * 4) old no header, new no header - simple append
       * */
      object[,] returnTable = oldTable;
      int oldWidth = oldTable.GetLength(1);
      int oldLength = oldTable.GetLength(0);
      int newWidth = newTable.GetLength(1);
      int newLength = newTable.GetLength(0);
      int targetWidth = Math.Max(oldWidth, newWidth);
      int targetLength = oldLength + newLength;
      int i = 0;
      if (oldHasHeader && newHasHeader)
        i = 1;
      for (; i < newLength; i++)
      {
        object[] newRow = Row(newTable, i);
        if (!oldHasHeader && newHasHeader)
          returnTable = InsertRow(returnTable, newRow, true, true, targetLength, targetWidth);
        else
          returnTable = InsertRow(returnTable, newRow, false, false, targetLength, targetWidth);
      }
      return returnTable;
    }

    /// <summary>
    /// Take one row and append it to the end of a table. Return the result.
    /// The resulting table is as wide as the widest of the two
    /// </summary>
    /// <param name="oldTable"></param>
    /// <param name="newRow"></param>
    /// <returns></returns>
    public static object[,] AppendRow(object[,] oldTable, object[] newRow)
    {
      int oldWidth = oldTable.GetLength(1);
      int oldLength = oldTable.GetLength(0);
      int newWidth = newRow.GetLength(0);
      int targetWidth = Math.Max(oldWidth, newWidth);
      int targetLength = oldLength + 1;
      return InsertRow(oldTable, newRow, false, false, targetLength, targetWidth);
    }

    /// <summary>
    /// Resize a table to the largest of newLength and newWidth, or the combination of
    /// the table and the row. Optionally, insert the row at the beginning. Of course
    /// this means after the header if there is one.
    /// </summary>
    /// <param name="oldTable"></param>
    /// <param name="oldRow"></param>
    /// <param name="atBeginning"></param>
    /// <param name="hasHeader"></param>
    /// <param name="newLength"></param>
    /// <param name="newWidth"></param>
    /// <returns></returns>
    public static object[,] InsertRow(object[,] oldTable, object[] oldRow, bool atBeginning, bool hasHeader,
      int newLength, int newWidth)
    {
      int i = 0;
      object[,] newTable;
      int newRowSub = 0;
      while (newRowSub < oldTable.GetLength(0) && oldTable[newRowSub, 0] != null) // find the first null row or the end
        newRowSub++;
      if (oldTable.GetLength(1) > newWidth)   // make sure the new width is at least as wide as oldTable
        newWidth = oldTable.GetLength(1);
      //if (oldTable.GetLength(0) + 1 > newLength) // and that the new length is long enough to contain oldTzble plus one row
      if (newRowSub + 1 > oldTable.GetLength(0))
        newLength = oldTable.GetLength(0) + 1;
      else
        newLength = oldTable.GetLength(0);
      int copyWidth = 0;
      if (oldRow.GetLength(0) < newWidth)       // and that the new width is wide enough for for the new row, too
        copyWidth = oldRow.GetLength(0);        // and that we only copy enough cells for new row
      else
      {
        newWidth = oldRow.GetLength(0);
        copyWidth = newWidth;
      }
      if (atBeginning)                        // do we insert this row at the beginning?
      {
        newTable = new object[newLength, newWidth]; // yes, so create the new table
        int beginRow = 0;                   // keep track of where we put the header and the new row
        if (hasHeader)                      // if it has a header
        {
          for (i = 0; i < oldTable.GetLength(1); i++)
            newTable[beginRow, i] = oldTable[beginRow, i];  // copy the header first if there is one
        }
        for (i = 0; i < copyWidth; i++)
          newTable[beginRow, i] = oldRow[i];                // then copy the row
        i = 0;
        if (hasHeader)
          i = 1;
        for (; i < oldTable.GetLength(0); i++)         // then the rest of the old table
          for (int j = 0; j < oldTable.GetLength(1); j++)
            newTable[i + beginRow + 1, j] = oldTable[i, j];
      }
      else
      {
        if (newLength > oldTable.GetLength(0) || newWidth > oldTable.GetLength(1))
          newTable = Copy(oldTable, newLength, newWidth); // now copy our table and expand as required 
        else
          newTable = oldTable;

        for (i = 0; i < copyWidth; i++)
          newTable[newRowSub, i] = oldRow[i];                // and then add the row
      }
      return newTable;
    }

    /// <summary>
    /// Overload that assumes the output table is the same size as the input
    /// </summary>
    /// <param name="oldTable"></param>
    /// <returns></returns>
    public static object[,] Copy(object[,] oldTable)
    {
      return Copy(oldTable, oldTable.GetLength(0), oldTable.GetLength(1));
    }

    /// <summary>
    /// Copies a table to another one. They may be of different sizes. 
    /// The entries are null-filled or truncated as required to fit the
    /// size of the target table  
    /// </summary>
    /// <param name="oldTable"></param>
    /// <param name="newLength">Length of target table</param>
    /// <param name="newWidth">Width of target table</param>
    /// <returns></returns>
    public static object[,] Copy(object[,] oldTable, int newLength, int newWidth)
    {
      object[,] newTable = new object[newLength, newWidth]; // make the new table the size of the input dimensions
      int tLength = oldTable.GetLength(0);  // set length of copy dimensions
      int tWidth = oldTable.GetLength(1);   // set width of copy dimensions
      // we reset the dimensions to the min of width and length of both tables
      if (newLength < tLength)
        tLength = newLength;
      if (newWidth < tWidth)
        tWidth = newWidth;
      for (int i = 0; i < tLength; i++)   // now do the copy
        for (int j = 0; j < tWidth; j++)
          newTable[i, j] = oldTable[i, j];
      return newTable;
    }

    /// <summary>
    /// Overload that accepts array of object instead of array of string
    /// </summary>
    /// <param name="oList"></param>
    /// <returns></returns>
    public static string ToList(object[] oList)
    {
      return ToList(oList, "");
    }

    /// <summary>
    /// Overload that accepts array of object instead of array of string
    /// </summary>
    /// <param name="oList"></param>
    /// <param name="delimiter"></param>
    /// <returns></returns>
    public static string ToList(object[] oList, string delimiter)
    {
      return ToList(oList, delimiter, true);
    }

    /// <summary>
    /// Overload that accepts array of object instead of array of string
    /// </summary>
    /// <param name="oList"></param>
    /// <param name="delimiter"></param>
    /// <param name="includeEmptyItems"></param>
    /// <returns></returns>
    public static string ToList(object[] oList, string delimiter, bool includeEmptyItems)
    {
      if (oList == null)
        return null;
      string[] strList = new string[oList.GetLength(0)];
      for (int i = 0; i < oList.GetLength(0); i++)
        strList[i] = CString(oList[i]);
      return ToList(strList, delimiter, includeEmptyItems);
    }

    /// <summary>
    /// Overload! it calls the ToList with an empty delimeter
    /// Convert an array of string to a commas separated list, with delimeters
    /// like: ('item1','item2',...) or (item1,item2,item3,...) or ("item1","item2","item3",...) etc
    /// </summary>
    /// <param name="strList"></param>
    /// <returns></returns>
    public static string ToList(string[] strList)
    {
      return ToList(strList, "");
    }

    /// <summary>
    /// ToList method with strList, delimeter parameter
    /// </summary>
    /// <param name="strList"></param>
    /// <param name="delimiter"></param>
    /// <returns></returns>
    public static string ToList(string[] strList, string delimiter)
    {
      return ToList(strList, delimiter, true);
    }

    /// <summary>
    /// Convert an array of string to a commas separated list, with delimeters
    /// like: ('item1','item2',...) or (item1,item2,item3,...) or ("item1","item2","item3",...) etc
    /// </summary>
    /// <param name="strList"></param>
    /// <returns></returns>
    public static string ToList(string[] strList, string delimiter, bool includeEmptyItems)
    {
      if (strList == null)
        return null;

      string strResult = null;
      bool hasEntry = false;
      if (strList == null)
        return null;

      //Let's check the delimiter
      if (delimiter == null)
        delimiter = "";

      foreach (string s in strList)
      {
        if (includeEmptyItems || (s != null && s.Length > 0))
        {
          if (strResult == null)
            strResult = delimiter;
          if (!hasEntry)
            hasEntry = true;
          else
            strResult = strResult + delimiter + ", " + delimiter;
          strResult = strResult + s;
        }
      }
      if (strResult != null)
        strResult = strResult + delimiter;
      return strResult;
    }

    public static string ToTableList(string[] strList, bool includeEmptyItems)
    {
      if (strList == null)
        return null;

      string strResult = null;
      bool hasEntry = false;
      if (strList == null)
        return null;

      foreach (string s in strList)
      {
        if (includeEmptyItems || (s != null && s.Length > 0))
        {
          if (strResult == null)
            strResult = string.Empty;
          if (!hasEntry)
            hasEntry = true;
          else
            strResult = strResult + CommonData.COLSEPARATORCHAR;
          strResult = strResult + s;
        }
      }
      return strResult;
    }

    /// <summary>
    /// ToListFromTable with object oList parameter
    /// </summary>
    /// <param name="oList"></param>
    /// <returns></returns>
    public static string ToListFromTable(object[,] oList)
    {
      if (oList == null)
        return null;
      string strResult = null;
      bool hasEntry = false;
      if (oList == null)
        return null;

      for (int i = 0; i < oList.GetLength(0); i++)
      {
        string s = oList[i, 0].ToString();
        if (strResult == null)
          strResult = "'";
        if (!hasEntry)
          hasEntry = true;
        else
          strResult = strResult + "', '";
        strResult = strResult + s;
      }
      if (strResult != null)
        strResult = strResult + "'";
      return strResult;
    }

    /// <summary>
    /// Converts a comma-delimited set of tokens into a string array
    /// </summary>
    /// <param name="strString"></param>
    /// <returns></returns>
    public static string[] FromList(string strString)
    {
      /* 
       * Take a comma delimited string and turn it into an array of string
       */
      string[] arrList = null;
      string strStringList = string.Empty;

      if (!(strString == null || strString == string.Empty))    // if the list of itemTypes is not null or empty
      {                               // populate array of strString
        strStringList = strString.Replace(" ", "");
        strStringList = CommonData.listSeparatorCharacters[0] + strStringList + CommonData.listSeparatorCharacters[0];
        arrList = strStringList.Split(CommonData.listSeparatorCharacters, StringSplitOptions.RemoveEmptyEntries);
      }
      return arrList;
    }

    /// <summary>
    /// Takes the comma-delimited list of tokens in filter and does a kind of "AND" 
    /// operation against the comma-delimited list of tokens in list. 
    /// Returns a list in the same format that only has those tokens that
    /// are both in filter and in list.
    /// </summary>
    /// <param name="filter">Comma-delimited list of items to apply as a filter against list</param>
    /// <param name="list">Comma-delimiated list of items to filter</param>
    /// <returns></returns>
    public static string filterList(string filter, string list)
    {
      string[] arrFilter = FromList(filter);
      string[] arrList = FromList(list);
      ArrayList newList = new ArrayList();
      foreach (string listToken in arrList)
        foreach (string filterToken in arrFilter)
          if (stringEquals(listToken, filterToken))
            newList.Add(listToken);
      string[] result = (string[])newList.ToArray();
      string resultString = ToList(result, CommonData.cQUOTE);
      return resultString;
    }

    /// <summary>
    /// Tests to see if an AttributeTable contains the key in one of the rows in the first column
    /// </summary>
    /// <param name="key">Value to match against the first column</param>
    /// <param name="myTable">AttrbuteTable format table</param>
    /// <returns>True if the key is found, false if it is not</returns>
    public static bool ContainsKey(object key, object[,] myTable)
    {
      if (myTable == null)
        return false;
      if (myTable.GetLength(0) == 0 || myTable.GetLength(1) == 0)
        return false;
      if (key == null)
        return false;
      if (key.ToString().Length == 0)
        return false;
      for (int i = 0; i < myTable.GetLength(0); i++)
        if (myTable[i, 0] != null && objectEquals(key, myTable[i, 0]))
          return true;
      return false;
    }

    /// <summary>
    /// Looks up a value in an AttributeTable by finding the first match on the key, and then
    /// returning the cell in column col (zero based).
    /// </summary>
    /// <param name="key">Value to match against the first column</param>
    /// <param name="col">Zero based subscript of the column that contains the value we want</param>
    /// <param name="myTable"></param>
    /// <returns>The value if found, or null if it is not</returns>
    public static object getValue(object key, int col, object[,] myTable)
    {
      if (myTable == null)
        return null;
      if (myTable.GetLength(0) == 0 || myTable.GetLength(1) == 0)
        return null;
      if (key == null)
        return null;
      if (key.ToString().Length == 0)
        return null;
      for (int i = 0; i < myTable.GetLength(0); i++)
        if (myTable[i, 0] != null && objectEquals(key, myTable[i, 0]))
          return myTable[i, col];
      return null;
    }

    #endregion Table and List Manipulation

    #region SQL Database Connection Options
    public static void commentDB(string db)
    {
      string m_ConfigFileName = CommonData.SERVERCONFIGFILENAME;
      string currentPath = getCurrentPath();

      if (!currentPath.EndsWith("\\"))
        currentPath += "\\";

      string fileLocation = currentPath + m_ConfigFileName;
      string line, conStr = "";
      string tofile = "";

      try
      {
        StreamReader tagBossConfig_file = new StreamReader(fileLocation);

        while ((line = tagBossConfig_file.ReadLine()) != null)
        {
          if ((!(line.Equals(""))))
          {
            conStr = line;
            if (conStr.Contains("//"))
              conStr = conStr.Substring(conStr.LastIndexOf("/") + 1);
            if (conStr.Equals(db))
              tofile += conStr + "\n";
            else
              tofile += "//" + conStr + "\n";
          }
        }
        tagBossConfig_file.Close();
      }
      catch (FileNotFoundException fnfe)
      {
        //log.Error(Log.LogContext.Dal, EM_CONFIGURATION_FILE_NOT_FOUND, fnfe);

        string msg = "Database Configuration file " + m_ConfigFileName + " could not be found.";
        FileNotFoundException myFnfe = new FileNotFoundException(msg, fnfe);
        throw myFnfe;
      }

      try
      {
        StreamWriter dbWriter = new StreamWriter(fileLocation, false);
        dbWriter.Write(tofile);
        dbWriter.Flush();
        dbWriter.Close();
      }
      catch (IOException ioe)
      {

      }
    }
    public static void writeTextFile(string path, string fileName, string fileType, string contents)
    {
      if (fileName == null && fileName == string.Empty)
      {
        ExceptionMessage tm = new ExceptionMessage("TAGFunctions", "writeTextFile", "File name is empty");
        tm.AddParm(fileName);
        throw new Exception(tm.ToString());
      }
      if (path == null || path == string.Empty)
        path = getCurrentPath();
      if (!path.EndsWith("\\"))
        path += "\\";
      if (fileType != null && fileType != string.Empty && !fileType.StartsWith("."))
        fileType = "." + fileType;
      string fileLocation = path + fileName + fileType;
      try
      {
        StreamWriter txtWriter = new StreamWriter(fileLocation, false);
        txtWriter.Write(contents);
        txtWriter.Flush();
        txtWriter.Close();
      }
      catch (IOException ioe)
      {
        ExceptionMessage tm = new ExceptionMessage("TAGFunctions", "writeTextFile", ioe.Message + ":" + ioe.StackTrace);
        tm.AddParm(fileName);
        tm.AddParm(path);
        tm.AddParm(fileType);
        throw new Exception(tm.ToString());
      }
    }

    public static List<string[]> loadAvailableDatabases()
    {
      List<string[]> availableDBs = new List<string[]>();
      string m_ConfigFileName = CommonData.SERVERCONFIGFILENAME;

      string currentPath = getCurrentPath();
      if (!currentPath.EndsWith("\\"))
        currentPath += "\\";

      string fileLocation = currentPath + m_ConfigFileName;
      string line, conStr = "";
      string[] db_server;
      string[] fileSplitter = new string[] { ";" };

      try
      {
        StreamReader tagBossConfig_file = new StreamReader(fileLocation);

        while ((line = tagBossConfig_file.ReadLine()) != null)
        {
          if ((!(line.Equals(""))))
          {
            conStr = line;
            if (conStr.Contains("//"))
              conStr = conStr.Substring(conStr.LastIndexOf("/") + 1);
            db_server = conStr.Split(fileSplitter, StringSplitOptions.RemoveEmptyEntries);
            availableDBs.Add(db_server);
          }
        }
        tagBossConfig_file.Close();
      }
      catch (FileNotFoundException fnfe)
      {
        string msg = "Database Configuration file " + m_ConfigFileName + " could not be found.";
        FileNotFoundException myFnfe = new FileNotFoundException(msg, fnfe);
        throw myFnfe;
      }
      return availableDBs;
    }
    #endregion SQL Database Connection Options
    #region Auxiliary functions
    public static object getDBValue(object def, object src) { return src is DBNull ? def : src; }
    public static DataSet decryptDataset(DataSet ds, string encryptedFieldName)
    {
      if (ds != null && ds.Tables.Count > 0)
      {
        DataSet dsNew = ds.Copy();
        DataTable dt = dsNew.Tables[0];
        if (dt.Columns.Contains(encryptedFieldName) && dt.Columns[encryptedFieldName].DataType == typeof(string))
        {
          ds.Clear();
          ds = null;
          foreach (DataRow row in dt.Rows)
          {
            row[encryptedFieldName] = _encrypt.decryptString(CommonFunctions.CString(row[encryptedFieldName]));
          }
          return dsNew;
        }
        else
          return ds;
      }
      else
        return ds;
    }
    public static DataSet encryptDataset(DataSet ds, string encryptedFieldName)
    {
      if (ds != null && ds.Tables.Count > 0)
      {

        DataSet dsNew = ds.Copy();
        DataTable dt = ds.Tables[0];
        if (dt.Columns.Contains(encryptedFieldName) && dt.Columns[encryptedFieldName].DataType == typeof(string))
        {
          foreach (DataRow row in dt.Rows)
          {
            row[encryptedFieldName] = _encrypt.encryptString(CommonFunctions.CString(row[encryptedFieldName]));
          }
          return dsNew;
        }
        else
          return ds;
      }
      else
        return ds;
    }
    #endregion Auxiliary functions
  }
}
