using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using TAGBOSS.Common.Model;
using TAGBOSS.Common.Logging;

namespace TAGBOSS.Common
{
  [SerializableAttribute]
  public static class TAGFunctions
  {
    static Log log = (Log)LogFactory.GetInstance().GetLog("TAGBOSS.Common.TAGFunctions");

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

    public enum EnumFunctions
    {
      CompareTo,
      CDecimal,
      CDouble,
      CDateTime,
      CInt,
      CLng,
      CBoolean,
      CString,
      CTableHeader,
      IsTableHeader,
      toValue,
      defaultValue,
      IsInteger,
      IsNumeric,
      IsBoolean,
      IsDateTime,
      IsNullOrStringEmpty,
      IsEventResource,
      DataType,
      MapSQLToVar,
      MapVarToSQL,
      dsDataType,

      quarter,
      CalculateDate,
      IsBusinessDate,
      GetNoDays,
      getPeriod,
      daysInPeriod,
      daysInMonth,

      formatString,
      properCase,
      replicate,
      stripParens,
      toValidXMLString,
      cleanXMLString,
      eventResourceID,
      payrollEntityResourceID,
      payrollEntityResourceIDLike,
      getEventIDfromResource,
      getCurrentPath,
      toProperCase,
      getApplicationTitle,
      setFullName,
      setClientName,
      makeFullName,
      GUIStatusLine,
      TAGBOSSVersion,
      getFormattedKey,

      canCompare,
      failsTest,
      format,
      parseValidationString,
      passesTest,
      stringEquals,
      objectEquals,
      actionStatusLine,
      parseActionStatusLine,
      parseString,
      isFunction,
      getFunctionName,
      inList,
      stripBadCharacters,
      stripComment,
      isAttributeReference,
      beginsWith,
      processBlockInheritYN,
      processDefaultItem,
      processFunction,
      isValidID,
      propercaseItemKey,

      Sort,
      Row,
      RowSubscript,
      RemoveRow,
      AppendTable,
      AppendRow,
      InsertRow,
      Copy,
      ToList,
      ToTableList,
      ToListFromTable,
      FromList,
      filterList,
      ContainsKey,
      getValue,

      commentDB,
      writeTextFile,
      loadAvailableDatabases,
      reportError,
      getInnerException,
      setFlag,
      ValueHistoriesUnion,
      ValueHistoriesAssignValues,
      filterEACByAttributes
    };

    #region module data
    private const string cQUOTE = "'";
    private const string cCOLON = ":";
    private const string cLISTSEPARATOR = "~";
    private const char cFUNCTIONCHAR = '_';
    private static DateTime cNULLDATETIME = new DateTime(1, 1, 1);

    // XML string conversion constants
    const string c_TO_XML_AMP = "&amp;";
    const string c_TO_XML_LT = "&lt;";
    const string c_TO_XML_GT = "&gt;";
    const string c_TO_XML_QUOT = "&quot;";
    const string c_TO_XML_APOS = "&apos;";

    const string c_FROM_XML_AMP = "&";
    const string c_FROM_XML_LT = "<";
    const string c_FROM_XML_GT = ">";
    const string c_FROM_XML_QUOT = "\"";
    const string c_FROM_XML_APOS = "'";
    // end xml constants

    private const char cLEFT = '(';
    private const char cRIGHT = ')';
    //private const char cLEFTDELIM = '{';
    //private const char cRIGHTDELIM = '}';
    private const char cEQUALS = '=';
    private const char cLESSTHAN = '<';
    private const char cGREATERTHAN = '>';
    private const char cNOT = '!';
    private const char cSPACE = ' ';
    private const string EQ = "==";
    private const string LT = "<";
    private const string GT = ">";
    private const string LE = "<=";
    private const string GE = ">=";
    private const string NE = "!=";
    //changed operatorList so parseValidationString works properly - Leonardo
    //private static string[] operatorList = { EQ, LT, GT, LE, GE, NE };
    public static string[] operatorList = { EQ, LE, GE, LT, GT, NE };
    private static string[] badFieldCharList = { "'", ",", "~", "@", "#", "\"" };
    private static string[] listSeparatorCharacters = { ",", ".", ROWSEPARATORCHAR.ToString(), COLSEPARATORCHAR.ToString() };  // standard separators for toList(), fromList()
    private static string validIdCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890-";
    private const int VALIDIDLENGTH = 50;


    private static DateTime dtPASTDATETIME = new DateTime(1900, 1, 1);
    private static DateTime dtFUTUREDATETIME = new DateTime(2100,12,31);
    private static int intMAXDEPTHSELFCALL = 255;    //Maximum recursive calls for this component
    private static string databaseName = string.Empty;
    //private static AttributeTable at = new AttributeTable();
    /*
     * The following flags are system-wide, mainly for debugging or testing purposes. 
     * By default, they are all turned off
     */
    private static bool bypassFunctionError = false;
    private static bool throwErrorOnDataTypeConversion = false;
    private static bool isDemoDatabase = false;
    private static bool saveDebugItem = false;
    private static bool throwDataConversionException = false;
    private static bool saveZeroTrans = false;
    private static TItem debugTItem = null;
    private static Item debugItem = null;
    private static bool logAttributeChanges = false;
    private static bool useNonXMLCall = false;
    private static bool bypassAsyncActionProcessing = false;
    private static bool displayNonXMLOption = true;
    private static bool useTAGBOSSConfig = true;
    private static bool useEntityPath = false;

    /*
     * End system flags
     */
    #endregion module data

    #region public properties

    public const string SERVERCONFIGFILENAME = "DB.Config";
    public const string SERVERCONFIGHOSTSECTION = "appHost";
    public const string SERVERCONFIGAPPSERVERSECTION = "appServer";
    public const string SERVERCONFIGDBSERVERSECTION = "dbServer";
    public const string SERVERCONFIGAPPSERVERPROPERTY = "BaseURL";
    public const string SERVERCONFIGDBSERVERPROPERTY = "Database";
    public const string SERVERCONFIGPROXYMODEPROPERTY = "ProxyMode";
    public const string SERVERCONFIGREMEMBERLOGINPROPERTY = "RememberLogin";
    public enum ProxyMode { Remote, Inproc, SharedMem };

    public const string VALUE = "value";
    public const string FUNC = "func";
    public const string SOURCEITEM = "sourceitem";

    public const string ATTRIBUTECHAR = "@";
    public const string MINUSCHAR = "-";
    public const string PLUSCHAR = "+";
    public const char COMMENTCHAR = '^';
    public const string TRANSCODEINDEXCHAR = cCOLON;
    public const string LITERALCHAR = "`";
    public const string REF_RAW = "ref";
    public const string REF_INHERIT = "refinherit";

    public const string ATTR_1_AT_CHAR = "@";
    public const string ATTR_2_AT_CHAR = "@@";
    public const string ATTR_3_AT_CHAR = "@@@";
    public const string ATTR_AT_ENTITY = "@entity";
    public const string ATTR_AT_ITEMTYPE = "@itemtype";
    public const string ATTR_AT_ITEM = "@item";
    public const string ATTR_AT_SOURCEITEM = "@sourceitem";
    public const string ATTR_AT_ATTRIBUTE = "@attribute";
    public const string ATTR_AT_ITEM_STARTDATE = "@itemstartdate";
    public const string ATTR_AT_ITEM_ENDDATE = "@itemenddate";
    public const string ATTR_AT_EFFECTIVE_DATE = "@effectivedate";

    public const string ATTR_AT_ENTITYTYPE = "@entitytype";
    public const string ATTR_AT_ENTITYOWNER = "@entityowner";
    public const string ATTR_AT_LEGALNAME = "@legalname";
    public const string ATTR_AT_FIRSTNAME = "@firstname";
    public const string ATTR_AT_MIDDLENAME = "@middlename";
    public const string ATTR_AT_ALTERNATENAME = "@alternatename";
    public const string ATTR_AT_ALTERNATEID = "@alternateid";
    public const string ATTR_AT_FEIN = "@fein";
    public const string ATTR_AT_ENTITYSTARTDATE = "@entitystartdate";
    public const string ATTR_AT_ENTITYENDDATE = "@entityenddate";
    public const string ATTR_AT_SHORTNAME = "@shortname";
    public const string ATTR_AT_FULLNAME = "@fullname";
    public const string ATTR_AT_CLIENT = "@client";

    //END CONSTANTS FROM AttributeProcessor

    //BEGIN CONSTANTS FROM TransactionEngine, TransCode generator
    public const string TRANSCODE_IT = "transcode";
    public const string TC_ITEMTYPE = "itemtype";
    public const string TC_GENERATOR_YN = "transcodegeneratoryn";
    public const string GENERATE_TC = "generatetranscode";
    //END CONSTANTS FROM TransactionEngine, TransCode generator

    // BEGIN CONSTANTS for data types and AttributeTable Parsing
    // END CONSTANTS for data types and AttributeTable Parsing
    //BEGIN CONSTANTS FROM AttributeProcessor
    public const int LOCKSLEEPTIME = 1000;
    public const int MAXSLEEPSECONDS = 3000;
    public static int MAXENTITYSEARCHRESULTS = 5000;

    public const string ENTITY_ITEMTYPE = "entity";
    public const string ALL_ENTITY = "all";
    public const string DEFAULT_ENTITY = "default";
    public const string DEFAULT_ITEM = "default";
    public const string EXCLUDE_ITEM = "exclude";  //Constant to identify the attibute that excludes an item
    public const string EXCLUDE_YN_ITEM = "excludeyn";  //Constant to identify the attibute that excludes an item (Y/N version)
    public const string BLOCK_INHERIT_ITEM = "blockinherityn";  //Constant to identify the attibute that excludes an item
    public const string INCLUDE = "include";
    public const string DICTIONARY = "dictionary";
    public const string FUNCTION = "function";
    public const string ITEMLIST = "itemlist";
    public const string STARTDATE = "startdate";
    public const string ENDDATE = "enddate";
    public const string LASTMODIFIEDBY = "lastmodifiedby";
    public const string LASTMODIFIEDDATETIME = "lastmodifieddatetime";
    public const string TABLEHEADER = "tableheader";
    public const string TABLEMOD = "tablemod";
    public const string TABLENOHEADER = "tablenoheader";
    public const string RESOURCELOCKEVENTENTITYPREFIX = "PayrollEntity";
    public const string RESOURCELOCKEVENTPREFIX = "Event";


    public static string[] entityTypesWithNames = { "Employee", "Contact", "Dependent", "Beneficiary", "User" };
    public static string[] entityTypesOfOrganization = { "SB", "Bank", "Vendor", "Carrier", "Client", "Prospect", "Parent" };
    public const string ACTIONSUCCEEDED = "Success";
    public const string ACTIONFAILED = "Error";
    public const char ENDSTRINGCHAR = cRIGHT;
    public const char COLSEPARATORCHAR = '~';
    public const char BEGINSTRINGCHAR = cLEFT;
    public const char ROWSEPARATORCHAR = '|';
    public const string DATATYPESTRING = "string";
    public const string DATATYPEINTEGER = "int";
    public const string DATATYPELONG = "long";
    public const string DATATYPEMONEY = "money";
    public const string DATATYPEDATETIME = "datetime";
    public const string DATATYPEDOUBLE = "double";
    public const string DATATYPEDECIMAL = "decimal";
    public const string DATATYPEOBJECT = "object";
    public const string DATATYPEBOOLEAN = "bool";
    public const string DATATYPEVARCHAR = "varchar";
    public const string DATATYPETABLEHEADER = "tableheader";
    public const string DATATYPECONDITION = "condition";
    public const string DEFAULTDATATYPE = "string";
    public static string[] ValidDataTypes = { DATATYPESTRING, DATATYPEDATETIME, DATATYPEDECIMAL, DATATYPEBOOLEAN, DATATYPEINTEGER, 
                                              DATATYPETABLEHEADER, DATATYPECONDITION, DATATYPELONG, DATATYPEMONEY, DATATYPEOBJECT };

    public const string HASTRANSACTIONLOCK = "True";
    public const string HASNOTRANSACTIONLOCK = "False";

    public const string SCREENMODESHOWEVENTGRID = "ShowEventGridYN";
    public const string SCREENMODESHOWTRANSGRID = "ShowTransGridYN";
    public const string SCREENMODESHOWTRANSDETAILFIELDS = "ShowTransDetailFieldsYN";
    public const string SCREENMODEFILTERCLEAREDDATE = "ShowTransDetailFieldsYN";
    public const string SCREENMODESEARCHALLENTITIES = "SearchAllEntitiesYN";
    public const string SCREENMODESEARCHALLEVENTS = "SearchAllEventsYN";
    public const string SCREENMODETRANSCODECATEGORY = "TransCodeCategory";
    public static string[] screenModeAttributes = new string[] { TAGFunctions.SCREENMODESHOWEVENTGRID, 
                                                 TAGFunctions.SCREENMODESHOWTRANSGRID,
                                                 TAGFunctions.SCREENMODESHOWTRANSDETAILFIELDS,
                                                 TAGFunctions.SCREENMODEFILTERCLEAREDDATE,
                                                 TAGFunctions.SCREENMODESEARCHALLENTITIES,
                                                 TAGFunctions.SCREENMODESEARCHALLEVENTS,
                                                 TAGFunctions.SCREENMODETRANSCODECATEGORY };
    public static Dictionaries Dictionary
    {
      get
      {
        return DictionaryFactory.getInstance().getDictionary();
      }
    }

    public static string DatabaseName
    {
      get { return TAGFunctions.databaseName; }
      set { TAGFunctions.databaseName = value; }
    }

    public static bool IsDemoDatabase
    {
      get { return TAGFunctions.isDemoDatabase; }
      set { TAGFunctions.isDemoDatabase = value; }
    }

    public static bool SaveDebugItem
    {
      get { return TAGFunctions.saveDebugItem; }
      set { TAGFunctions.saveDebugItem = value; }
    }

    public static TItem DebugTItem
    {
      get { return TAGFunctions.debugTItem; }
      set { TAGFunctions.debugTItem = value; }
    }

    public static Item DebugItem
    {
      get { return TAGFunctions.debugItem; }
      set { TAGFunctions.debugItem = value; }
    }

    public static bool ThrowDataConversionException
    {
      get { return TAGFunctions.throwDataConversionException; }
      set { TAGFunctions.throwDataConversionException = value; }
    }

    public static DateTime NULLDATETIME
    {
      get { return cNULLDATETIME; }
    }

    public static string EQCHAR
    {
      get { return EQ; }
    }

    /// <summary>
    /// Universal value for a date/time arbitrarily far in the past
    /// </summary>
    public static DateTime PastDateTime
    {
      get { return dtPASTDATETIME; }
    }

    /// <summary>
    /// Universal value for a date/time arbitrarily far in the future
    /// </summary>
    public static DateTime FutureDateTime
    {
      get { return dtFUTUREDATETIME; }
    }

    /// <summary>
    /// For routines that can be recursive, this is the maximum number of recursions that
    /// are allowed. This is to prevent "run-away" recursion and stack overflow
    /// </summary>
    public static int MaxDepthSelfCall
    {
      /// TODO: We must review this number! to see it's real purpose
      get { return intMAXDEPTHSELFCALL; }
      set { intMAXDEPTHSELFCALL = value; }
    }

    /// <summary>
    /// Used to bypass the throwing of errors for functions in the attribute object
    /// </summary>
    public static bool BypassFunctionError
    {
      get { return TAGFunctions.bypassFunctionError; }
      set { TAGFunctions.bypassFunctionError = value; }
    }

    public static bool UseNonXMLCall
    {
      get { return TAGFunctions.useNonXMLCall; }
      set { TAGFunctions.useNonXMLCall = value; }
    }

    public static bool DisplayNonXMLOption
    {
      get { return TAGFunctions.displayNonXMLOption; }
    }
    public static bool BypassAsyncActionProcessing
    {
      get { return TAGFunctions.bypassAsyncActionProcessing; }
      set { TAGFunctions.bypassAsyncActionProcessing = value; }
    }

    public static bool ThrowErrorOnDataTypeConversion
    {
      get { return TAGFunctions.throwErrorOnDataTypeConversion; }
      set { TAGFunctions.throwErrorOnDataTypeConversion = value; }
    }
    /// <summary>
    /// debug flag that will save ALL transactions in a transaction engine calculation, not just ones
    /// with a non-zero balance. Default value is false
    /// </summary>
    public static bool SaveZeroTrans
    {
      get { return TAGFunctions.saveZeroTrans; }
      set { TAGFunctions.saveZeroTrans = value; }
    }

    /// <summary>
    /// Should we log changes to the attribute table?
    /// </summary>
    public static bool LogAttributeChanges
    {
      get { return TAGFunctions.logAttributeChanges; }
      set { TAGFunctions.logAttributeChanges = value; }
    }

    public static bool UseTAGBOSSConfig
    {
      get { return TAGFunctions.useTAGBOSSConfig; }
      set { TAGFunctions.useTAGBOSSConfig = value; }
    }

    public static bool UseEntityPath
    {
      get { return TAGFunctions.useEntityPath; }
      set { TAGFunctions.useEntityPath = value; }
    }
    public static bool AllowEmptyParameterATAT = true;
    //ThrowErrorOnDataTypeConversion

    /// <summary>
    /// standard list separator
    /// </summary>
    public static string LISTSEPARATORCHAR
    {
      get { return cLISTSEPARATOR; }
    }

    /// <summary>
    /// This is the character prefix that indicates a token to be parsed is a function name
    /// </summary>
    public const char FUNCTIONCHAR = '_';

    /// <summary>
    /// The right character of the paired delimiter (right parens)
    /// </summary>
    public static char RIGHTCHAR
    {
      get { return cRIGHT; }
    }

    /// <summary>
    /// The left character of the paired delimiter (left parens)
    /// </summary>
    public static char LEFTCHAR
    {
      get { return cLEFT; }
    }

    /// <summary>
    /// The minus character
    /// </summary>
    /// <summary>
    /// This is the character prefix that indicates a token to be parsed is an attribute name
    /// </summary>

    #endregion public properties

    #region public evaluateFunction unique entry point!

    public static object evaluateFunction(EnumFunctions functionName, string entityID, string entityType, string entityName, string firstName, string middleName, string fein, string altName, string altID, out string shortName)
    {
      string getFullName = "";
      try
      {
        getFullName = makeFullName(entityID, entityType, entityName, firstName, middleName, fein, altName, altID, out shortName);
      }
      catch (Exception ex)
      {
        shortName = "";
        log.Error("Function 'makeFullName' FAILED: ", ex);
      }

      return getFullName;
    }

    public static object evaluateFunction(EnumFunctions functionName, Type t1, Type t2, out string compareType)
    {
      bool getCanCompare = false;
      try
      {
        getCanCompare = canCompare(t1, t2, out compareType);
      }
      catch (Exception ex)
      {
        compareType = "";
        log.Error("Function 'canCompare' FAILED: ", ex);
      }

      return getCanCompare;
    }

    public static object evaluateFunction(EnumFunctions functionName, string statusLine, out string actionName, out string workflowStepID, out string status, out string message, out int securityHandle)
    {
      bool getParseActionStatusLine = false;
      try
      {
        getParseActionStatusLine = parseActionStatusLine(statusLine, out actionName, out workflowStepID, out status, out message, out securityHandle);
      }
      catch (Exception ex)
      {
        actionName = "";
        workflowStepID = "";
        status = "";
        message = "";
        securityHandle = -1;

        log.Error("Function 'parseActionStatusLine': ", ex);
      }

      return getParseActionStatusLine;
    }

    public static object evaluateFunction(EnumFunctions functionName, string expressionIn, out string insideString)
    {
      string FunctionName = "";
      try
      {
        FunctionName = getFunctionName(expressionIn, out insideString);
      }
      catch (Exception ex)
      {
        insideString = "";

        log.Error("Function 'getFunctionName': ", ex);
      }

      return FunctionName;
    }

    public static object evaluateFunction(EnumFunctions functionName, object val, bool trimResults, out string comment)
    {
      object getStripComment = "";
      try
      {
        getStripComment = stripComment(val, trimResults, out comment);
      }
      catch (Exception ex)
      {
        comment = "";

        log.Error("Function 'stripComment' FAILED: ", ex);
      }
      return getStripComment;
    }

    public static object evaluateFunction(EnumFunctions functionName, string validationString, ref string operand1, ref string op, ref string operand2)
    {
      object getParseValidationString = "";
      try
      {
        getParseValidationString = parseValidationString(validationString, ref operand1, ref op, ref operand2);
      }
      catch (Exception ex)
      {
        operand1 = "";
        op = "";
        operand2 = "";

        log.Error("Function 'parseValidationString' FAILED: ", ex);
      }

      return getParseValidationString;
    }

    public static object evaluateFunction(EnumFunctions functionName, params object[] args)
    {
      object resolveValue = null;

      try
      {
        #region main evaluate function switch
        switch (functionName)
        {
          //BEGIN: DataType Conversion
          case EnumFunctions.CompareTo:
            resolveValue = CompareTo(args[0], CString(args[1]), args[2], CString(args[3]));
            break;
          case EnumFunctions.CDecimal:
            switch (args.GetLength(0))
            {
              case 1:
                resolveValue = CDecimal(args[0]);
                break;
              case 2:
                resolveValue = CDecimal(args[0], CDecimal(args[1]));
                break;
            }
            break;
          case EnumFunctions.CDouble:
            switch (args.GetLength(0))
            {
              case 1:
                resolveValue = CDouble(args[0]);
                break;
              case 2:
                resolveValue = CDouble(args[0], CDouble(args[1]));
                break;
            }
            break;
          case EnumFunctions.CDateTime:
            switch (args.GetLength(0))
            {
              case 1:
                resolveValue = CDateTime(args[0]);
                break;
              case 2:
                resolveValue = CDateTime(args[0], CDateTime(args[1]));
                break;
            }
            break;
          case EnumFunctions.CInt:
            switch (args.GetLength(0))
            {
              case 1:
                resolveValue = CInt(args[0]);
                break;
              case 2:
                resolveValue = CInt(args[0], CInt(args[1]));
                break;
            }
            break;
          case EnumFunctions.CLng:
            switch (args.GetLength(0))
            {
              case 1:
                resolveValue = CLng(args[0]);
                break;
              case 2:
                resolveValue = CLng(args[0], CLng(args[1]));
                break;
            }
            break;
          case EnumFunctions.CBoolean:
            switch (args.GetLength(0))
            {
              case 1:
                resolveValue = CBoolean(args[0]);
                break;
              case 2:
                resolveValue = CBoolean(args[0], CBoolean(args[1]));
                break;
            }
            break;
          case EnumFunctions.CString:
            switch (args.GetLength(0))
            {
              case 1:
                resolveValue = CString(args[0]);
                break;
              case 2:
                resolveValue = CString(args[0], CString(args[1]));
                break;
            }
            break;
          case EnumFunctions.CTableHeader:
            switch (args.GetLength(0))
            {
              case 1:
                resolveValue = CTableHeader(args[0]);
                break;
              case 2:
                resolveValue = CTableHeader(CString(args[0]), args[1]);
                break;
              case 3:
                resolveValue = CTableHeader(CString(args[0]), args[1], (Dictionaries)args[2]);
                break;
            }
            break;
          case EnumFunctions.IsTableHeader:
            resolveValue = IsTableHeader(args[0]);
            break;
          case EnumFunctions.toValue:
            switch (args.GetLength(0))
            {
              case 1:
                resolveValue = toValue(CString(args[0]));
                break;
              case 2:
                resolveValue = toValue(args[0], CString(args[1]));
                break;
              case 3:
                if (args[2].GetType() == typeof(bool))
                  resolveValue = toValue(args[0], CString(args[1]), CBoolean(args[2]));
                else
                  resolveValue = toValue(args[0], CString(args[1]), args[2]);
                break;
              case 4:
                resolveValue = toValue(args[0], CString(args[1]), args[2], CBoolean(args[3]));
                break;
            }
            break;
          case EnumFunctions.defaultValue:
            resolveValue = defaultValue(CString(args[0]));
            break;
          case EnumFunctions.IsInteger:
            resolveValue = IsInteger(args[0]);
            break;
          case EnumFunctions.IsNumeric:
            resolveValue = IsNumeric(args[0]);
            break;
          case EnumFunctions.IsBoolean:
            resolveValue = IsBoolean(args[0]);
            break;
          case EnumFunctions.IsDateTime:
            resolveValue = IsDateTime(args[0]);
            break;
          case EnumFunctions.IsNullOrStringEmpty:
            resolveValue = IsNullOrStringEmpty(args[0]);
            break;
          case EnumFunctions.IsEventResource:
            resolveValue = IsEventResource(CString(args[0]));
            break;
          case EnumFunctions.DataType:
            resolveValue = DataType(args[0]);
            break;
          case EnumFunctions.MapSQLToVar:
            resolveValue = MapSQLToVar((SqlDbType)args[0]);
            break;
          case EnumFunctions.MapVarToSQL:
            resolveValue = MapVarToSQL(CString(args[0]));
            break;
          case EnumFunctions.dsDataType:
            resolveValue = dsDataType(CString(args[0]));
            break;
          //END: DataType Conversion

          //BEGIN: date functions
          case EnumFunctions.quarter:
            resolveValue = quarter(CInt(args[0]));
            break;
          case EnumFunctions.CalculateDate:
            resolveValue = CalculateDate((DateTime)args[0], CString(args[1]), CBoolean(args[2]));
            break;
          case EnumFunctions.IsBusinessDate:
            resolveValue = IsBusinessDate((DateTime)args[0]);
            break;
          case EnumFunctions.GetNoDays:
            resolveValue = GetNoDays((global::System.DayOfWeek)args[0], CInt(args[1]));
            break;
          case EnumFunctions.getPeriod:
            resolveValue = getPeriod((DateTime)args[0], CString(args[1]));
            break;
          case EnumFunctions.daysInPeriod:
            resolveValue = daysInPeriod((DateTime)args[0], CString(args[1]));
            break;
          case EnumFunctions.daysInMonth:
            resolveValue = daysInMonth((DateTime)args[0]);
            break;
          //END: date functions

          //BEGIN: string functions
          case EnumFunctions.formatString:
            resolveValue = formatString(CString(args[0]), CInt(args[1]), CString(args[2]));
            break;
          case EnumFunctions.properCase:
            resolveValue = properCase(CString(args[0]));
            break;
          case EnumFunctions.replicate:
            resolveValue = replicate(CInt(args[0]));
            break;
          case EnumFunctions.stripParens:
            resolveValue = stripParens(CString(args[0]));
            break;
          case EnumFunctions.toValidXMLString:
            switch (args.GetLength(0))
            {
              case 1:
                resolveValue = toValidXMLString(args[0]);
                break;
              case 2:
                resolveValue = toValidXMLString(args[0], CBoolean(args[1]));
                break;
            }
            break;
          case EnumFunctions.cleanXMLString:
            resolveValue = cleanXMLString(CString(args[0]));
            break;
          case EnumFunctions.eventResourceID:
            resolveValue = eventResourceID(CString(args[0]));
            break;
          case EnumFunctions.payrollEntityResourceID:
            resolveValue = payrollEntityResourceID(CString(args[0]), CString(args[1]));
            break;
          case EnumFunctions.payrollEntityResourceIDLike:
            resolveValue = payrollEntityResourceIDLike(CString(args[0]));
            break;
          case EnumFunctions.getEventIDfromResource:
            resolveValue = getEventIDfromResource(CString(args[0]));
            break;
          case EnumFunctions.getCurrentPath:
            resolveValue = getCurrentPath();
            break;
          case EnumFunctions.toProperCase:
            resolveValue = toProperCase(CString(args[0]));
            break;
          case EnumFunctions.getApplicationTitle:
            resolveValue = getApplicationTitle(CString(args[0]), CString(args[1]));
            break;
          case EnumFunctions.setFullName:
            switch (args.GetLength(0))
            {
              case 1:
                setFullName((TEntity)args[0]);
                break;
              case 2:
                setFullName((TAGBOSS.Common.Model.FieldsCollection)args[0], (System.Data.DataRow)args[1]);
                break;
            }
            break;
          case EnumFunctions.setClientName:
            switch (args.GetLength(0))
            {
              case 1:
                setClientName((TEntity)args[0]);
                break;
              case 2:
                setClientName((TAGBOSS.Common.Model.FieldsCollection)args[0], (System.Data.DataRow)args[1]);
                break;
            }
            break;
          //case EnumFunctions.makeFullName:
          //  //TODO! How do we manage OUT parameters??
          //  string shorName = "";
          //  resolveValue = makeFullName(CString(args[0], CString(args[1], CString(args[2], CString(args[3], CString(args[4], CString(args[5], CString(args[6], CString(args[7], out shorName);
          //  break;
          case EnumFunctions.GUIStatusLine:
            resolveValue = GUIStatusLine();
            break;
          case EnumFunctions.TAGBOSSVersion:
            resolveValue = TAGBOSSVersion();
            break;
          case EnumFunctions.getFormattedKey:
            resolveValue = getFormattedKey(CString(args[0]), CString(args[1]));
            break;
          //END: string functions

          //BEGIN: parsing functions
          //case EnumFunctions.canCompare:
          //  //TODO! How do we manage OUT parameters??
          //  string compareType = "";
          //  resolveValue = canCompare((Type)args[0], (Type)args[1], out compareType);
          //  break;
          case EnumFunctions.failsTest:
            resolveValue = failsTest(CString(args[0]), CString(args[1]), CString(args[2]), args[3]);
            break;
          case EnumFunctions.format:
            resolveValue = format(CString(args[0]), CInt(args[1]), CString(args[2]));
            break;
          //case EnumFunctions.parseValidationString:
          //  //TODO! How do we manage REF parameters??
          //  string arg1, arg2, arg3;
          //  arg1 = CString(args[1];
          //  arg2 = CString(args[2];
          //  arg3 = CString(args[3];
          //  resolveValue = parseValidationString(CString(args[0], ref arg1, ref arg2, ref arg3);
          //  break;
          case EnumFunctions.passesTest:
            switch (args.GetLength(0))
            {
              case 3:
                if (args[0].GetType() == typeof(string))
                  resolveValue = passesTest(CString(args[0]), CString(args[1]), CString(args[2]));
                else
                  resolveValue = passesTest(args[0], CString(args[1]), args[2]);

                break;
              case 4:
                resolveValue = passesTest(CString(args[0]), CString(args[1]), CString(args[2]), args[3]);
                break;
            }
            break;
          case EnumFunctions.stringEquals:
            resolveValue = stringEquals(CString(args[0]), CString(args[1]));
            break;
          case EnumFunctions.objectEquals:
            resolveValue = objectEquals(args[0], args[1]);
            break;
          case EnumFunctions.actionStatusLine:
            resolveValue = actionStatusLine(CString(args[0]), CString(args[1]), CInt(args[2]), CString(args[3]), CString(args[4]));
            break;
          //case EnumFunctions.parseActionStatusLine:
          //  //TODO! How do we manage OUT parameters??
          //  string actionName = CString(args[1];
          //  string workflowStepID = CString(args[2];
          //  string status = CString(args[3];
          //  string message = CString(args[4];
          //  int securityHandle = CInt(args[5];

          //  resolveValue = parseActionStatusLine(CString(args[0], out actionName, out workflowStepID, out status, out message, out securityHandle);
          //  break;
          case EnumFunctions.parseString:
            switch (args.GetLength(0))
            {
              case 1:
                resolveValue = parseString(CString(args[0]));
                break;
              case 2:
                resolveValue = parseString(CString(args[0]), (string[])args[1]);
                break;
            }
            break;
          case EnumFunctions.isFunction:
            resolveValue = isFunction(CString(args[0]));
            break;
          //case EnumFunctions.getFunctionName:
          //  //TODO! How do we manage OUT parameters??
          //  string insideString = CString(args[1];
          //  resolveValue = getFunctionName(CString(args[0], out insideString);
          //  break;
          case EnumFunctions.inList:
            resolveValue = inList((string[])args[0], CString(args[1]));
            break;
          case EnumFunctions.stripBadCharacters:
            resolveValue = stripBadCharacters(CString(args[0]));
            break;
          case EnumFunctions.stripComment:
            switch (args.GetLength(0))
            {
              case 1:
                resolveValue = stripComment(args[0]);
                break;
              case 2:
                resolveValue = stripComment(args[0], CBoolean(args[1]));
                break;
            }
            break;
          case EnumFunctions.isAttributeReference:
            resolveValue = isAttributeReference(args[0]);
            break;
          case EnumFunctions.beginsWith:
            if (args[1].GetType() == typeof(string))
              resolveValue = beginsWith(args[0], CString(args[1]));
            else
              resolveValue = beginsWith(args[0], (char)args[1]);

            break;
          case EnumFunctions.processBlockInheritYN:
            resolveValue = processBlockInheritYN(CString(args[0]), (TItem)args[1], (TEntity)args[2], (TAttribute)args[3]);
            break;
          case EnumFunctions.processDefaultItem:
            switch (args.GetLength(0))
            {
              case 2:
                resolveValue = processDefaultItem((Item)args[0], CString(args[1]));
                break;
              case 3:
                resolveValue = processDefaultItem((TItem)args[0], (TEntity)args[1], CString(args[2]));
                break;
            }
            break;
          case EnumFunctions.processFunction:
            resolveValue = processFunction(CString(args[0]), (TAttribute)args[1], (TItem)args[2], (TEntity)args[3]);
            break;
          case EnumFunctions.isValidID:
            switch (args.GetLength(0))
            {
              case 1:
                resolveValue = isValidID(CString(args[0]));
                break;
              case 2:
                resolveValue = isValidID(CString(args[0]), CInt(args[1]));
                break;
            }
            break;
          case EnumFunctions.propercaseItemKey:
            resolveValue = propercaseItemKey(CString(args[0]), CString(args[1]));
            break;
          //END: parsing functions

          //BEGIN: Table and List Manipulation
          case EnumFunctions.Sort:
            switch (args.GetLength(0))
            {
              case 1:
                resolveValue = Sort((object[,])args[0]);
                break;
              case 2:
                resolveValue = Sort((object[,])args[0], CBoolean(args[1]));
                break;
              case 4:
                resolveValue = Sort((object[,])args[0], CBoolean(args[1]), CBoolean(args[2]), CString(args[3]));
                break;
              case 5:
                resolveValue = Sort((object[,])args[0], CBoolean(args[1]), CBoolean(args[2]), CString(args[3]), CInt(args[4]));
                break;
            }
            break;
          case EnumFunctions.Row:
            switch (args.GetLength(0))
            {
              case 2:
                if (args[1].GetType() == typeof(int))
                  resolveValue = Row((object[,])args[0], CInt(args[1]));
                else
                  resolveValue = Row((object[,])args[0], args[1]);

                break;
              case 3:
                resolveValue = Row((object[,])args[0], args[1], CBoolean(args[2]));
                break;
            }
            break;
          case EnumFunctions.RowSubscript:
            resolveValue = RowSubscript((object[,])args[0], args[1], CBoolean(args[2]));
            break;
          case EnumFunctions.RemoveRow:
            resolveValue = RemoveRow((object[,])args[0], args[1]);
            break;
          case EnumFunctions.AppendTable:
            switch (args.GetLength(0))
            {
              case 2:
                resolveValue = AppendTable((object[,])args[0], (object[,])args[1]);
                break;
              case 3:
                resolveValue = AppendTable((object[,])args[0], (object[,])args[1], CBoolean(args[2]));
                break;
              case 4:
                resolveValue = AppendTable((object[,])args[0], (object[,])args[1], CBoolean(args[2]), CBoolean(args[3]));
                break;
            }
            break;
          case EnumFunctions.AppendRow:
            resolveValue = AppendRow((object[,])args[0], (object[])args[1]);
            break;
          case EnumFunctions.InsertRow:
            resolveValue = InsertRow((object[,])args[0], (object[])args[1], CBoolean(args[2]), CBoolean(args[3]), CInt(args[4]), CInt(args[5]));
            break;
          case EnumFunctions.Copy:
            switch (args.GetLength(0))
            {
              case 1:
                resolveValue = Copy((object[,])args[0]);
                break;
              case 3:
                resolveValue = Copy((object[,])args[0], CInt(args[1]), CInt(args[2]));
                break;
            }
            break;
          case EnumFunctions.ToList:
            if (args.GetLength(0) > 0)
            {
              Type t; 
              if (args[0] == null)
                t = typeof(object);
              else
                t = args[0].GetType();
              switch (args.GetLength(0))
              {
                case 1:
                  if (t == typeof(string[]))
                    resolveValue = ToList((string[])args[0]);
                  else
                    resolveValue = ToList((object[])args[0]);

                  break;
                case 2:
                  if (t == typeof(string[]))
                    resolveValue = ToList((string[])args[0], CString(args[1]));
                  else
                    resolveValue = ToList((object[])args[0], CString(args[1]));

                  break;
                case 3:
                  if (t == typeof(string[]))
                    resolveValue = ToList((string[])args[0], CString(args[1]), CBoolean(args[2]));
                  else
                    resolveValue = ToList((object[])args[0], CString(args[1]), CBoolean(args[2]));

                  break;
              }
            }
            break;
          case EnumFunctions.ToTableList:
            resolveValue = ToTableList((string[])args[0], CBoolean(args[1]));
            break;
          case EnumFunctions.ToListFromTable:
            resolveValue = ToListFromTable((object[,])args[0]);
            break;
          case EnumFunctions.FromList:
            resolveValue = FromList(CString(args[0]));
            break;
          case EnumFunctions.filterList:
            resolveValue = filterList(CString(args[0]), CString(args[1]));
            break;
          case EnumFunctions.ContainsKey:
            resolveValue = ContainsKey(args[0], (object[,])args[1]);
            break;
          case EnumFunctions.getValue:
            resolveValue = getValue(args[0], CInt(args[1]), (object[,])args[2]);
            break;
          //END: Table and List Manipulation

          //BEGIN: SQL Database Connection Options
          case EnumFunctions.commentDB:
            commentDB(CString(args[0]));
            break;
          case EnumFunctions.writeTextFile:
            writeTextFile(CString(args[0]), CString(args[1]), CString(args[2]), CString(args[3]));
            break;
          case EnumFunctions.loadAvailableDatabases:
            resolveValue = loadAvailableDatabases();
            break;
          //END: SQL Database Connection Options

          //BEGIN: Error processing functions
          case EnumFunctions.reportError:
            resolveValue = reportError(CString(args[0]), CString(args[1]), CString(args[2]));
            break;
          case EnumFunctions.getInnerException:
            resolveValue = getInnerException((Exception)args[0]);
            break;
          case EnumFunctions.setFlag:
            setFlag(CString(args[0]), CBoolean(args[1]));
            break;
          //END: 

          //BEGIN: valueHistory manipulation functions
          case EnumFunctions.ValueHistoriesUnion:
            resolveValue = ValueHistoriesUnion((ValueHistoryCollection)args[0], (ValueHistoryCollection)args[1]);
            break;
          case EnumFunctions.ValueHistoriesAssignValues:
            resolveValue = ValueHistoriesAssignValues((ValueHistoryCollection)args[0], (ValueHistoryCollection)args[1]);
            break;
          //END: valueHistory manipulation functions

          //BEGIN: EntityAttribute transformation functions
          case EnumFunctions.filterEACByAttributes:
            resolveValue = filterEACByAttributes((EntityAttributesCollection)args[0], CString(args[1]), CString(args[2]));
            break;
          //END: 

        };
        #endregion main evaluate function switch
      }
      catch (System.InvalidCastException icex) 
      {
        log.Error("Function FAILED: ", icex);
      }
      catch (Exception ex)
      {
        log.Error("Function FAILED: ", ex);
      }

      return resolveValue;
    }

    #endregion public evaluateFunction unique entry point!

    #region private evaluateFunction function calls

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
    private static int CompareTo(object o1, string dataType1, object o2, string dataType2)
    {
      string sDataType1 = DATATYPESTRING;
      if (dataType1 != null)
        sDataType1 = dataType1.ToLower();
      string sDataType2 = DATATYPESTRING;
      if (dataType2 != null)
        sDataType2 = dataType2.ToLower();
      if (sDataType1 != sDataType2 || o1 == null || o2 == null)
        return CString(o1).CompareTo(CString(o2));
      switch (sDataType1)
      {
        case DATATYPESTRING:
          if (o1.GetType() == typeof(TableHeader) && o2.GetType() == typeof(TableHeader))
            return ((TableHeader)o1).CompareTo((TableHeader)o2);
          else
            return CString(o1).ToLower().CompareTo(CString(o2).ToLower());
        case DATATYPEBOOLEAN:
          return CBoolean(o1).CompareTo(CBoolean(o2));
        case DATATYPEDATETIME:
          // is o1 null (or "null")?
          string strO1 = CString(o1).ToLower();
          bool o1isNull = (strO1 == string.Empty || strO1 == "null");
          if (!o1isNull)
          {
            DateTime dtO1 = CDateTime(o1);
            o1isNull = (dtO1 == FutureDateTime || dtO1 == PastDateTime);
          }
          string str02 = CString(o2).ToLower();
          bool o2isNull = (str02 == string.Empty || str02 == "null");
          if (!o2isNull)
          {
            DateTime dt02 = CDateTime(o2);
            o2isNull = (dt02 == FutureDateTime || dt02 == PastDateTime);
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
        case DATATYPEDECIMAL:
        case "money":
          return CDecimal(o1).CompareTo(CDecimal(o2));
        case DATATYPEINTEGER:
          return CInt(o1).CompareTo(CInt(o2));
        case "long":
          return CLng(o1).CompareTo(CLng(o2));
        case DATATYPETABLEHEADER:
          if (o1.GetType() == typeof(TableHeader) && o2.GetType() == typeof(TableHeader))
            return ((TableHeader)o1).CompareTo((TableHeader)o2);
          else
            return CString(o1).ToLower().CompareTo(CString(o2).ToLower());
        default:
          return CString(o1).ToLower().CompareTo(CString(o2).ToLower());
      }
    }
    /// <summary>
    /// Standard conversion function that never returns a null.
    /// </summary>
    /// <param name="o">object that can be converted to a Decimal</param>
    /// <returns>Returns a zero if an error occurs or if o is null.</returns>
    private static decimal CDecimal(object o)
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
        if (throwErrorOnDataTypeConversion)
        {
          TAGExceptionMessage tm = new TAGExceptionMessage("TAGFunctions", "CDecimal", "Cannot convert to this datatype");
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
    private static decimal CDecimal(object o, decimal defaultValue)
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
    private static double CDouble(object o)
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
        if (throwErrorOnDataTypeConversion)
        {
          TAGExceptionMessage tm = new TAGExceptionMessage("TAGFunctions", "CDouble", "Cannot convert to this datatype");
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
    private static double CDouble(object o, double defaultValue)
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
    private static DateTime CDateTime(object o)
    {
      return CDateTime(o, PastDateTime);
    }

    /// <summary>
    /// Standard conversion function that never returns a null.
    /// </summary>
    /// <param name="o">object that can be converted to a CDateTime</param>
    /// <returns>Returns the defaultDateTime if an error occurs or if o is null.</returns>
    private static DateTime CDateTime(object o, DateTime defaultValue)
    {
      if (o == null || o == System.DBNull.Value || o.ToString() == string.Empty)
        return defaultValue;
      if (o.GetType() == typeof(DateTime))
        return (DateTime)o;
      DateTime dateTime = defaultValue;

      if (DateTime.TryParse(o.ToString(), out dateTime))
        return dateTime;

      return defaultValue;
    }

    /// <summary>
    /// Standard conversion function that never returns a null.
    /// </summary>
    /// <param name="o">object that can be converted to an Int32</param>
    /// <returns>Returns a zero if an error occurs or if o is null.</returns>
    private static int CInt(object o)
    {
      return CInt(o, 0);
    }

    private static long CLng(object o, long defaultValue)
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
    /// Overload with default value = 0, and throws an error if (throwErrorOnDataTypeConversion)
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    private static long CLng(object o)
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
        if (throwErrorOnDataTypeConversion)
        {
          TAGExceptionMessage tm = new TAGExceptionMessage("TAGFunctions", "CLng", "Cannot convert to this datatype");
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
    private static int CInt(object o, int defaultValue)
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
    private static bool CBoolean(object o)
    {
      return CBoolean(o, false);
    }

    /// <summary>
    /// overload that allows specification of the value to use if conversion is unsuccessful
    /// </summary>
    /// <param name="o"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    private static bool CBoolean(object o, bool defaultValue)
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
    private static string CString(object o)
    {
      return CString(o, string.Empty);
    }

    /// <summary>
    /// overload that allows specification of the value to use if conversion is unsuccessful
    /// </summary>
    /// <param name="o"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    private static string CString(object o, string defaultValue)
    {
      if (o == null || o == System.DBNull.Value)
        return defaultValue;
      else
        if (o.GetType() == typeof(DateTime))
        {
          // check to see if this has a time or only a date
          DateTime dt = (DateTime)o;
          string dtShort = dt.ToShortDateString();
          if (Convert.ToDateTime(dtShort).Equals(dt))
            return dtShort; // return just the date
          else
            return dt.ToString(); // return the date and time
        }
        else
          return o.ToString();
    }
    /// <summary>
    /// Returns a TableHeader typed version of the object if it is one. Otherwise throws an error
    /// </summary>
    /// <param name="oTable"></param>
    /// <returns></returns>
    private static TableHeader CTableHeader(object oTable)
    {
      string errorMessage = CString(oTable) + " is not a valid TableHeader";
      if (oTable == null)
        errorMessage = "Table is null";
      else
      {
        Type t = oTable.GetType();
        if (t == typeof(TableHeader))
          return (TableHeader)oTable;
      }
      TAGExceptionMessage tm = new TAGExceptionMessage("TAGFunctions", "CTableHeader", errorMessage);
      tm.AddParm(oTable);
      throw new Exception(tm.ToString());
    }
    /// <summary>
    /// Returns a TableHeader typed version of the object if it is one. If it is not, it tries to convert it to a table header
    /// </summary>
    /// <param name="attributeName">name of the Attribute. This is not required if the object is already a tableheader</param>
    /// <param name="oTable">value which should contain the table</param>
    /// <param name="dictionary">Instance of the dictionary. This is not required if the object is already a tableheader</param>
    /// <returns></returns>
    private static TableHeader CTableHeader(string attributeName, object oTable, Dictionaries dictionary)
    {
      // for backward compatbility. We no longer need the dictionary argument
      return CTableHeader(attributeName, oTable);
    }
    private static TableHeader CTableHeader(string attributeName, object oTable)
    {
      string errorMessage = CString(attributeName) + " is not a valid TableHeader";
      if (oTable == null)
        errorMessage = CString(attributeName) + " is null";
      else
      {
        Type t = oTable.GetType();
        if (t == typeof(TableHeader))
          return (TableHeader)oTable;
        //return (TableHeader)((TableHeader)oTable).Clone();
        if (IsTableHeader(oTable))
          return new TableHeader(attributeName, oTable.ToString(), DictionaryFactory.getInstance().getDictionary());
      }
      TAGExceptionMessage tm = new TAGExceptionMessage("TAGFunctions", "CTableHeader", errorMessage);
      tm.AddParm(oTable);
      tm.AddParm(attributeName);
      throw new Exception(tm.ToString());
    }
    private static bool IsTableHeader(object o)
    {
      if (o == null)
        return false;
      string tName = o.GetType().Name.ToLower();
      switch (tName)
      {
        case DATATYPETABLEHEADER:
          return true;
        case DATATYPESTRING:
          string oString = o.ToString().Trim();
          if (oString.Length < 2)
            return false;
          return (oString.StartsWith(cLEFT.ToString())
                  && oString.EndsWith(cRIGHT.ToString()));
        default:
          return false;
      }
    }

    /// <summary>
    /// Take a string token, and attempt to convert first to a DateTime, then to a numeric (double).
    /// If neither were successfull, then make it a string. Take the result and return it as an object.
    /// </summary>
    /// <param name="strValue"></param>
    /// <returns></returns>
    private static object toValue(string strValue)
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
    private static object toValue(object oValue, string typeName)
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
    private static object toValue(object oValue, string typeName, bool throwError)
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
    private static object toValue(object oValue, string typeName, object pDefaultValue)
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
    private static object toValue(object oValue, string typeName, object pDefaultValue, bool throwError)
    {
      try
      {
        bool hasError = false;  // if throwError is false, then we don't flag an error, so we set hasError to throwError if a conversion error occurs
        object retValue = string.Empty;
        switch (typeName.ToLower())
        {
          case DATATYPESTRING:
            retValue = (object)CString(oValue, (string)pDefaultValue);
            break;
          case DATATYPEMONEY:
          case DATATYPEDECIMAL:
            if (IsNumeric(oValue))
              retValue = (object)CDecimal(oValue, (decimal)pDefaultValue);
            else
            {
              retValue = oValue;
              hasError = (throwError && throwErrorOnDataTypeConversion);
            }
            break;
          case DATATYPEDOUBLE:
            if (IsNumeric(oValue))
              retValue = (object)CDouble(oValue, (double)pDefaultValue);
            else
            {
              retValue = oValue;
              hasError = (throwError && throwErrorOnDataTypeConversion);
            }
            break;
          case DATATYPEINTEGER:
            if (IsNumeric(oValue))
              retValue = (object)CInt(oValue, (int)pDefaultValue);
            else
            {
              retValue = oValue;
              hasError = (throwError && throwErrorOnDataTypeConversion);
            }
            break;
          case DATATYPELONG:
            if (IsNumeric(oValue))
              retValue = (object)CLng(oValue, (long)pDefaultValue);
            else
            {
              retValue = oValue;
              hasError = (throwError && throwErrorOnDataTypeConversion);
            }
            break;
          case DATATYPEDATETIME:
            if (IsDateTime(oValue))
              retValue = (object)CDateTime(oValue, (DateTime)pDefaultValue);
            else
            {
              retValue = oValue;
              hasError = (throwError && throwErrorOnDataTypeConversion);
            }
            break;
          case DATATYPEBOOLEAN:
            if (IsBoolean(oValue))
              retValue = (object)CBoolean(oValue, (bool)pDefaultValue);
            else
            {
              retValue = oValue;
              hasError = (throwError && throwErrorOnDataTypeConversion);
            }
            break;
          case DATATYPEOBJECT:
            retValue = oValue;
            break;
          default:
            hasError = (throwError && throwErrorOnDataTypeConversion);
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
        if (throwError && throwErrorOnDataTypeConversion)
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
    private static object defaultValue(string typeName)
    {
      switch (typeName.ToLower())
      {
        case DATATYPESTRING:
          return string.Empty;
        case DATATYPEINTEGER:
          return (int)0;
        case DATATYPELONG:
          return (long)0;
        case DATATYPEDOUBLE:
          return (double)0;
        case DATATYPEMONEY:
        case DATATYPEDECIMAL:
          return (decimal)0;
        case DATATYPEBOOLEAN:
          return false;
        case DATATYPEDATETIME:
          return PastDateTime;
        case DATATYPEOBJECT:
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
    private static bool IsInteger(object o)
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
    private static bool IsNumeric(object o)
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
    private static bool IsBoolean(object o)
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
    private static bool IsDateTime(object o)
    {
      if (o == null || o == System.DBNull.Value)
        return false;
      string typeName = o.GetType().Name.ToLower();
      if (typeName == DATATYPEDATETIME)
        return true;
      if (typeName == DATATYPEDECIMAL || typeName == DATATYPEINTEGER)
        return false;
      //if (typeName == DATATYPESTRING && o.ToString().IndexOf("/") == -1 && CLng(o) > 0)
      //  return false;
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
    private static bool IsNullOrStringEmpty(object value)
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
    private static bool IsEventResource(string resource)
    {
      if (resource == null)
        return false;
      return resource.StartsWith(RESOURCELOCKEVENTPREFIX);
    }

    private static string DataType(object o)
    {
      if (o == null || o == System.DBNull.Value)
        return DATATYPESTRING;
      //TODO: lmv66: need to review this with Larry!!
      if (IsNumeric(o) && CDecimal(o).ToString() == o.ToString())
        return DATATYPEDECIMAL;
      if (IsInteger(o) && CInt(o).ToString() == o.ToString())
        return DATATYPEINTEGER;
      if (IsDateTime(o))
        return DATATYPEDATETIME;
      if (IsBoolean(o))
        return DATATYPEBOOLEAN;
      if (o.GetType().Name.ToLower().EndsWith(DATATYPETABLEHEADER))
        return DATATYPETABLEHEADER;
      string s = o.ToString();
      if (s.Length > 3 && s.StartsWith(cLEFT.ToString()) && s.EndsWith(cRIGHT.ToString()) && (s.Contains(ROWSEPARATORCHAR) || s.Contains(COLSEPARATORCHAR)))
        return DATATYPETABLEHEADER;
      return DATATYPESTRING;
    }

    /// <summary>
    /// Used to convert SQL Type name to Variables type name
    /// </summary>
    /// <param name="dataType"></param>
    /// <returns></returns>
    private static string MapSQLToVar(SqlDbType dataType)
    {
      string type;
      switch (dataType)
      {
        case SqlDbType.VarChar:
          type = DATATYPESTRING;
          break;
        case SqlDbType.Decimal:
          type = DATATYPEDECIMAL;
          break;
        case SqlDbType.Float:
          type = DATATYPEDOUBLE;
          break;
        case SqlDbType.DateTime:
          type = DATATYPEDATETIME;
          break;
        case SqlDbType.Int:
          type = DATATYPEINTEGER;
          break;
        case SqlDbType.Bit:
          type = DATATYPEBOOLEAN;
          break;
        case SqlDbType.BigInt:
          type = DATATYPELONG;
          break;
        case SqlDbType.Money:
          type = DATATYPEMONEY;
          break;
        default:
          type = DATATYPEVARCHAR;
          break;
      }

      return type;
    }

    /// <summary>
    /// Convert variable type name into SQL Type name
    /// </summary>
    /// <param name="dataType"></param>
    /// <returns></returns>
    private static SqlDbType MapVarToSQL(string dataType)
    {
      SqlDbType type;
      switch (dataType.ToLower())
      {
        case DATATYPESTRING:
          type = SqlDbType.VarChar;
          break;
        case DATATYPEDECIMAL:
          type = SqlDbType.Decimal;
          break;
        case DATATYPEDOUBLE:
          type = SqlDbType.Float;
          break;
        case DATATYPEDATETIME:
          type = SqlDbType.DateTime;
          break;
        case DATATYPEINTEGER:
          type = SqlDbType.Int;
          break;
        case DATATYPEBOOLEAN:
          type = SqlDbType.Bit;
          break;
        case DATATYPELONG:
          type = SqlDbType.BigInt;
          break;
        case DATATYPEMONEY:
          type = SqlDbType.Money;
          break;
        default:
          type = SqlDbType.VarChar;
          break;
      }
      return type;
    }
    private static Type dsDataType(string dataType)
    {
      Type returnType = System.Type.GetType("System.String");   // default is string
      if (dataType == null)
        return returnType;
      switch (dataType.ToLower())
      {
        case TAGFunctions.DATATYPEINTEGER:
          returnType = System.Type.GetType("System.Int32");
          break;
        case TAGFunctions.DATATYPEDOUBLE:
          returnType = System.Type.GetType("System.Double");
          break;
        case TAGFunctions.DATATYPEDECIMAL:
          returnType = System.Type.GetType("System.Decimal");
          break;
        case TAGFunctions.DATATYPEDATETIME:
          returnType = System.Type.GetType("System.DateTime");
          break;
        case TAGFunctions.DATATYPEBOOLEAN:
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
    private static string quarter(int month)
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
    /// Take a date, and modify it per the modification code. Optionally, ensure
    /// it is the next business day on or after today
    /// </summary>
    /// <param name="dateIn">The date you start with</param>
    /// <param name="Modifications">
    /// This code is one of the following:
    /// <para>
    /// BXX:  Add XX business days</para><para>
    /// MXX:  Next first of the month on or after XX months from the date</para><para>
    /// DXX:  Add XX days</para><para>
    /// WXX:  move to XX (1-7 where 1=Monday) of this week. Don't go to next or prior week.</para><para>
    /// SW:   IRS special semi weekly: Move forward to the next Wed or Fri</para><para>
    /// FSW:  IRS begining of SW period Wed or Sat
    /// LSW:  IRS end of SW period Fri or Tues
    /// MT:   IRS special move to the 15th of the following month</para><para>
    /// QT:   IRS special move to the end of the month following the current quarter</para><para>
    /// SA:   IRS special semi-annual. Of it is not 1/1 or 7/1, move forward to the next 1/1 or 7/1.
    /// FM:   The first of the current month</para><para>
    /// LM:   The last of the current month</para><para>
    /// FW:   If this is a weekend, move to the first of the next week.</para><para>
    /// LW:   If this is a weekend, move the the Friday immediately before (last of the week)</para><para>
    /// LQ:   Last day of the current quarter</para><para>
    /// FQ:   First day of the current quarter</para><para>
    /// any number: change number to XX and then do a BXX (see above)
    /// any other value: the date it unchanged by the modifier
    /// </para>
    /// </param>
    /// <param name="bAfterToday">If true, the resulting date will be the first
    /// valid business day on or after today</param>
    /// <returns></returns>
    private static DateTime CalculateDate(DateTime dateIn, string Modification, bool bAfterToday)
    {
      DateTime dtToday = DateTime.Today;
      DateTime pDate = dateIn;
      int iMonth;
      int intDayAdjust = 0;
      double weekendDay;
      string mods;
      if (Modification != null && Modification != string.Empty)
        mods = Modification.ToUpper();
      else
        throw new Exception("Modification in Calculate date cannot be empty");
      string Options = mods.Substring(0, 1); // get the first character
      if ((Options != "B" && Options != "M" && Options != "D" && Options != "W") || mods == "MT")
        Options = mods;        // but use the full string unless it is a B or M or D
      if (CInt(mods) != 0)       // if it just a number with no prefix
        Options = "B" + mods;    // then assume B as the prefix
      switch (Options)
      {
        case "NONE":
          return TAGFunctions.FutureDateTime;
        case "SW":  // Semi Weekly - Special IRS rule that means go forward to the next Wed or Fri
          DateTime dtSemiWeeklyDate = pDate.AddDays(GetNoDays(pDate.DayOfWeek, 0));
          pDate = pDate.AddDays(GetNoDays(pDate.DayOfWeek, 1));
          pDate = CalculateDate(pDate, "B3", false);
          if (pDate > dtSemiWeeklyDate)
            pDate = CalculateDate(pDate, "B0", false);
          else
            pDate = CalculateDate(dtSemiWeeklyDate, "B0", false);
          break;
        case "FSW":  // First Day of Semi Weekly Period - Special IRS rule that means go to the prior Wed or Sat
          switch (pDate.DayOfWeek)
          {
            case global::System.DayOfWeek.Sunday: intDayAdjust = -1; break;
            case global::System.DayOfWeek.Monday: intDayAdjust = -2; break;
            case global::System.DayOfWeek.Tuesday: intDayAdjust = -3; break;
            case global::System.DayOfWeek.Wednesday: intDayAdjust = 0; break;
            case global::System.DayOfWeek.Thursday: intDayAdjust = -1; break;
            case global::System.DayOfWeek.Friday: intDayAdjust = -2; break;
            case global::System.DayOfWeek.Saturday: intDayAdjust = 0; break;
          }
          dtSemiWeeklyDate = pDate.AddDays(intDayAdjust);

          //If a holdiday causes the period to "split" to 2 due dates, use a later end date to properly split the period
          if (CalculateDate(dtSemiWeeklyDate, "SW", false) < CalculateDate(pDate, "SW", false))
            dtSemiWeeklyDate = pDate;
          //else
          //dtSemiWeeklyDate = dtSemiWeeklyDate;

          if (dtSemiWeeklyDate < CalculateDate(pDate, "FQ", false))
            pDate = CalculateDate(pDate, "FQ", false);
          else
            pDate = dtSemiWeeklyDate;
          break;
        case "LSW":  // Last Day of Semi Weekly Period - Special IRS rule that means go forward to the next Fri or Tues
          switch (pDate.DayOfWeek)
          {
            case global::System.DayOfWeek.Sunday: intDayAdjust = 2; break;
            case global::System.DayOfWeek.Monday: intDayAdjust = 1; break;
            case global::System.DayOfWeek.Tuesday: intDayAdjust = 0; break;
            case global::System.DayOfWeek.Wednesday: intDayAdjust = 2; break;
            case global::System.DayOfWeek.Thursday: intDayAdjust = 1; break;
            case global::System.DayOfWeek.Friday: intDayAdjust = 0; break;
            case global::System.DayOfWeek.Saturday: intDayAdjust = 3; break;
          }
          dtSemiWeeklyDate = pDate.AddDays(intDayAdjust);

          //If a holdiday causes the period to "split" to 2 due dates, use an earlier end date to properly split the period
          if (CalculateDate(dtSemiWeeklyDate, "SW", false) > CalculateDate(pDate, "SW", false))
            dtSemiWeeklyDate = dtSemiWeeklyDate - (CalculateDate(dtSemiWeeklyDate, "SW", false) - CalculateDate(pDate, "SW", false));

          if (dtSemiWeeklyDate > CalculateDate(pDate, "LQ", false))
            pDate = CalculateDate(pDate, "LQ", false);
          else
            pDate = dtSemiWeeklyDate;
          break;
        case "MT":  // special IRS 15th of the month calculation, which is 15th of the following month
          if (pDate.Month == 12)
            pDate = CalculateDate(DateTime.Parse("1/15/" + (pDate.Year + 1).ToString()), "B0", false);
          else
            pDate = CalculateDate(DateTime.Parse((pDate.Month + 1).ToString() + "/15/" + pDate.Year.ToString()), "B0", false);
          break;
        case "QT":  // IRS specially defined, last day of the month following the quarter we are in
          if (pDate.Month >= 1 && pDate.Month <= 3)
            pDate = CalculateDate(DateTime.Parse("4/30/" + pDate.Year.ToString()), "B0", false);
          else if (pDate.Month >= 4 && pDate.Month <= 6)
            pDate = CalculateDate(DateTime.Parse("7/31/" + pDate.Year.ToString()), "B0", false);
          else if (pDate.Month >= 7 && pDate.Month <= 9)
            pDate = CalculateDate(DateTime.Parse("10/31/" + pDate.Year.ToString()), "B0", false);
          else if (pDate.Month >= 10 && pDate.Month <= 12)
            pDate = CalculateDate(DateTime.Parse("1/31/" + (pDate.Year + 1).ToString()), "B0", false);
          break;
        case "FM": // first day of the month our date is in
          pDate = DateTime.Parse(pDate.Month.ToString() + "/1/" + pDate.Year.ToString());
          break;
        case "LM": // last day of the month our date is in
          pDate = DateTime.Parse(pDate.Month.ToString() + "/1/" + pDate.Year.ToString()).AddMonths(1).AddDays(-1);
          break;
        case "FW": // first weekday of the week if it is a weekend
          weekendDay = 0;
          if (pDate.DayOfWeek == DayOfWeek.Saturday)
            weekendDay = 1;
          if (pDate.DayOfWeek == DayOfWeek.Sunday)
            weekendDay = 2;
          if (weekendDay != 0)
            pDate = pDate.AddDays((3.0 - weekendDay)); // move forward to Monday
          break;
        case "LW":  // last weekday of the week if it is a weekend
          weekendDay = 0;
          if (pDate.DayOfWeek == DayOfWeek.Saturday)
            weekendDay = 1;
          if (pDate.DayOfWeek == DayOfWeek.Sunday)
            weekendDay = 2;
          if (weekendDay != 0)
            pDate = pDate.AddDays(-(weekendDay)); // move back to Friday
          break;
        case "LQ":  // Last day of the quarter our date is in
          if (pDate.Month >= 1 && pDate.Month <= 3)
            pDate = DateTime.Parse("3/31/" + pDate.Year.ToString());
          else if (pDate.Month >= 4 && pDate.Month <= 6)
            pDate = DateTime.Parse("6/30/" + pDate.Year.ToString());
          else if (pDate.Month >= 7 && pDate.Month <= 9)
            pDate = DateTime.Parse("9/30/" + pDate.Year.ToString());
          else if (pDate.Month >= 10 && pDate.Month <= 12)
            pDate = DateTime.Parse("12/31/" + pDate.Year.ToString());

          break;
        case "FQ":    // first day of the quarter our date is in
          if (pDate.Month >= 1 && pDate.Month <= 3)
            pDate = DateTime.Parse("1/1/" + pDate.Year.ToString());
          else if (pDate.Month >= 4 && pDate.Month <= 6)
            pDate = DateTime.Parse("4/1/" + pDate.Year.ToString());
          else if (pDate.Month >= 7 && pDate.Month <= 9)
            pDate = DateTime.Parse("7/1/" + pDate.Year.ToString());
          else if (pDate.Month >= 10 && pDate.Month <= 12)
            pDate = DateTime.Parse("10/1/" + pDate.Year.ToString());
          break;
        case "SA":    // IRS defined calendar semi-annually: must be either 1/1 or 7/1
          if (!((pDate.Day == 1) && ((pDate.Month == 1 || pDate.Month == 7)))) // if 1/1 or 7/1 leave it alone
            if (pDate.Month < 7)  // otherwise, if it is in the first half the year
              pDate = DateTime.Parse("7/1/" + pDate.Year.ToString()); // make the date tne next 7/1
            else
              pDate = DateTime.Parse("1/1/" + (pDate.Year + 1).ToString()); // else make it the first of the next year
          break;
        case "M": // MXX where XX is the number of Months to move
          iMonth = pDate.Month + int.Parse(mods.Substring(1, mods.Length - 1)) + 1;
          int iYear = pDate.Year;
          int loopCounter = 0;
          if (pDate.Day == 1)
            iMonth -= 1;
          while (iMonth > 12 && loopCounter < 50)   // if the resulting month > 12
          {
            iYear++;            // add on to the year
            iMonth -= 12;       // subtract 12 from the month, and try again
            loopCounter++;      // just so we dont' go into some huge loop. Surly 50 years is enough
          }
          if (loopCounter >= 50)
            throw new Exception("month calculation cannot be more than 50 years in the future");
          pDate = DateTime.Parse((iMonth).ToString() + "/1/" + (iYear).ToString());
          break;
        case "D": // DXX where XX is the number of days to move
          pDate = pDate.AddDays(double.Parse(mods.Substring(1, mods.Length - 1)));
          break;
        case "W": // WX where X is the day of the week. Rules are: stay in the same week, and move to the correct day Monday = 1
          int dayOf = CInt(mods.Substring(1, mods.Length - 1));  // chg XX to dow
          if (dayOf > 7)
            Math.DivRem(dayOf + 1, 7, out dayOf);           // reduce back to mod 7
          int weekDay = (int)pDate.DayOfWeek + 1;
          // calculate days to adjust to make return date match weekday and still stay in the same week
          int daysToAdd = (dayOf - weekDay);
          if (daysToAdd != 0)             // we have to move it
            pDate = pDate.AddDays(daysToAdd); // then do it            
          break;
        case "B":   // BXX where XX is the number of business days to move the date (+/-)
          bool bIsBusinessDate = false;
          int iDaysToMove = CInt(mods.Substring(1, mods.Length - 1)); // chg the XX to an int
          int iIncrement = 1; // we move one day at a time
          if (iDaysToMove < 0)  // but if we are moving backward
            iIncrement = -1;    // we need to move backward one day at a time
          // use do...while to guarantee we go through the loop at least once
          int i = 1;
          do
          {
            if (iDaysToMove != 0)                   // move one day unless we are moving zero days
              pDate = pDate.AddDays(iIncrement);

            bIsBusinessDate = IsBusinessDate(pDate);  // now, is the day a business day?
            // if not, and we are supposed to move zero days, we still move one forward until it IS a business day
            if ((!bIsBusinessDate) && (iDaysToMove == 0))
              while (!IsBusinessDate(pDate))
                pDate = pDate.AddDays(iIncrement);

            if (bIsBusinessDate)    // we only count business days when we move up/down the counter
              i++;
          } while (i < Math.Abs(iDaysToMove));
          break;
      }
      /*
       * bAfterToday flag guarantees the date will not be less than the first business day
       * on or after the resulting date
       */
      if ((bAfterToday) && pDate < dtToday) // 
        pDate = CalculateDate(dtToday, "B0", false);

      pDate = DateTime.Parse(pDate.ToShortDateString()); // ensure we only have a date with no time

      return pDate;
    }
    /// <summary>
    /// True if this is a weekday that is not a US holiday. False if it is a weekday or holiday.
    /// </summary>
    /// <param name="dtDate"></param>
    /// <returns></returns>
    private static bool IsBusinessDate(DateTime dateIn)
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
    private static int GetNoDays(global::System.DayOfWeek DayofWeek, int iType)
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
    private static int getPeriod(DateTime dtDate, string period)
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
    private static int daysInPeriod(DateTime dtDate, string period)
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
    private static int daysInMonth(DateTime dtDate)
    {
      double adjustDay = 32;
      DateTime nextMonth = dtDate.AddDays(adjustDay);
      return Convert.ToInt16(adjustDay) - nextMonth.Day;
    }
    #endregion date functions

    #region string functions
    /// <summary>
    /// Function that fill a field n times with a value
    /// </summary>
    /// <param name="field"></param>
    /// <param name="length"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private static string formatString(string field, int length, string value)
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
    private static string properCase(string strOld)
    {
      string strNew = " ";
      string strCurrent = "";
      string strPrevious = "";
      int x = 0;
      int strLen = 0;
      string closeBracket = "";
      string openBracket = "";

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
          else if (openBracket.Contains(strPrevious))
          {
            strNew = strNew + strCurrent;
            x++;
            strCurrent = strOld.Substring(x, 1);
            while (!closeBracket.Contains(strCurrent))
            {
              strNew = strNew + strCurrent;
              x++;
              strCurrent = strOld.Substring(x, 1);

            }
            strNew = strNew + strCurrent;
          }
          else
          {
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
    private static string replicate(int length)
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
    private static string stripParens(string strIn)
    {
      if (strIn == null)
        return null;
      strIn = strIn.Trim();   // trim of leading spaces first
      if ( (strIn.StartsWith(cLEFT.ToString()) && strIn.EndsWith(cRIGHT.ToString())) || 
          (strIn.StartsWith("{") && strIn.EndsWith("}")) )
        return strIn.Substring(1, strIn.Length - 2);
      else
        return strIn;
    }
    /// <summary>
    /// Creates a valid xml string by excaping xml reserved characters
    /// </summary>
    /// <param name="inString"></param>
    /// <returns></returns>
    private static string toValidXMLString(object inString)
    {
      return toValidXMLString(inString, true);
    }
    /// <summary>
    /// Overload. Creates a valid xml string by excaping xml reserved characters. Allows specifying that '<' and '>' not be replaced
    /// </summary>
    /// <param name="inString">string to transform</param>
    /// <param name="replaceGTLT">replace the '<' and '>'? </param>
    /// <returns>transformed version of inString</returns>
    private static string toValidXMLString(object inString, bool replaceGTLT)
    {
      string outString = string.Empty;
      if (inString != null)
      {
        outString = inString.ToString().Replace(c_FROM_XML_AMP, c_TO_XML_AMP);
        outString = outString.Replace("&amp;amp;", "&amp;");  // just in case there was already an &amp, we fix it up
        outString = outString.Replace(c_FROM_XML_APOS, c_TO_XML_APOS);
        if (replaceGTLT)
        {
          outString = outString.Replace(c_FROM_XML_GT, c_TO_XML_GT);
          outString = outString.Replace(c_FROM_XML_LT, c_TO_XML_LT);
          outString = outString.Replace(c_FROM_XML_QUOT, c_TO_XML_QUOT);
        }
      }
      return outString;
    }
    private static string cleanXMLString(string inString)
    {
      string outstring = string.Empty;
      if (inString != null)
      {
        outstring = inString.Replace("'", "");
        outstring = outstring.Replace("&", "and");
      }
      return outstring;
    }
    private static string eventResourceID(string eventID)
    {
      if (eventID == null)
        eventID = "Unknown";
      return string.Format("Event.{0}", eventID);
    }
    private static string payrollEntityResourceID(string eventID, string entity)
    {
      if (eventID == null)
        eventID = "Unknown";
      if (entity == null)
        entity = "Unknown";
      return string.Format("PayrollEntity.{0}.{1}", eventID, entity);
    }
    private static string payrollEntityResourceIDLike(string eventID)
    {
      if (eventID == null)
        eventID = "Unknown";
      return string.Format("PayrollEntity.{0}.%", eventID);
    }
    private static string getEventIDfromResource(string resource)
    {
      if (resource == null || resource == string.Empty)
        return string.Empty;
      string[] parts = FromList(resource);
      if (parts.GetLength(0) < 2)
        return string.Empty;
      switch (parts[0])
      {
        case RESOURCELOCKEVENTENTITYPREFIX:
        case RESOURCELOCKEVENTPREFIX:
          return parts[1];
        default:
          return string.Empty;
      }
    }
    private static string getCurrentPath()
    {
      string AssemblyPath;
      AssemblyPath = global::System.IO.Path.GetDirectoryName(global::System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
      return AssemblyPath.Replace("file:\\", "");
    }
    /// <summary>
    /// Makes the first character of a string upper case and the rest lower case
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private static string toProperCase(string s)
    {
      if (s == null || s.Length == 0)
        return s;
      StringBuilder sb = new StringBuilder(s.ToLower());
      sb[0] = s[0].ToString().ToUpper().ToCharArray()[0];
      return sb.ToString();
    }
    private static string getApplicationTitle(string connectionString, string appName)
    {
      string testORprod = "Phoenix";
      if (connectionString.Contains("192.168.102") || connectionString.ToLower().Contains("rs1sql"))
        testORprod = "RackSpace";

      return string.Format("TAG V{0} {1} - {2}({3})", TAGBOSSVersion(), appName, TAGFunctions.DatabaseName, testORprod);
    }

    /// <summary>
    /// Accepts a DataRow from the EntityTable and adds the virtual fields FullName and ShortName to the Entity.Fields Collection
    /// </summary>
    /// <param name="fields">Entity.Fields collection to add the ShortName and FullName to</param>
    /// <param name="row">DataRow from the Entity table for this entity</param>
    private static void setFullName(TAGBOSS.Common.Model.FieldsCollection fields, System.Data.DataRow row)
    {
      string fullName = null;
      string shortName = null;
      string entityID = row["Entity"].ToString();
      string entityType = row["EntityType"].ToString();
      string entityName = row["LegalName"].ToString();
      string altName = TAGFunctions.CString(row["AlternateName"]);
      if (altName.Length > 0)
        entityName = altName;
      string fein = row["FEIN"].ToString();
      string altID = TAGFunctions.CString(row["AlternateID"]);
      if (altID.Length > 0)
        fein = altID;
      string firstName = TAGFunctions.CString(row["FirstName"]);
      string middleName = TAGFunctions.CString(row["MiddleName"]);
      fullName = TAGFunctions.makeFullName(entityID, entityType, entityName, firstName, middleName, fein, altName, altID, out shortName);
      fields.AddField("FullName", fullName);
      fields["FullName"].Virtual = true;
      fields["FullName"].Description = "Name";
      fields.AddField("ShortName", shortName);
      fields["ShortName"].Virtual = true;
      fields["ShortName"].Description = "Short Name";
    }

    /// <summary>
    /// Accepts a DataRow from the EntityTable and adds the virtual fields FullName and ShortName to the Entity.Fields Collection
    /// </summary>
    /// <param name="fields">Entity.Fields collection to add the ShortName and FullName to</param>
    /// <param name="row">DataRow from the Entity table for this entity</param>
    private static void setFullName(TEntity eObj)
    {
      string fullName = null;
      string shortName = null;
      string entityID = eObj.OrigId;
      string entityType = eObj.EntityType;
      string entityName = eObj.LegalName;
      string altName = TAGFunctions.CString(eObj.AlternateName);
      if (altName.Length > 0)
        entityName = altName;
      string fein = eObj.FEIN;
      string altID = TAGFunctions.CString(eObj.AlternateID);
      if (altID.Length > 0)
        fein = altID;
      string firstName = TAGFunctions.CString(eObj.FirstName);
      string middleName = TAGFunctions.CString(eObj.MiddleName);
      fullName = TAGFunctions.makeFullName(entityID, entityType, entityName, firstName, middleName, fein, altName, altID, out shortName);
      eObj.FullName = fullName;
      eObj.ShortName = shortName;
    }

    /// <summary>
    /// Accepts a DataRow from the EntityTable and adds the Client virtual field to the Entity.Fields Collection
    /// </summary>
    /// <param name="fields"></param>
    /// <param name="row"></param>
    private static void setClientName(TAGBOSS.Common.Model.FieldsCollection fields, System.Data.DataRow row)
    {
      if (row["EntityType"].ToString().ToLower().Equals("client"))     //set the client virtual field.
        fields.AddField("Client", TAGFunctions.CString(row["Entity"]));
      else
        fields.AddField("Client", TAGFunctions.CString(row["EntityOwner"]));
      fields["Client"].Virtual = true;
      fields["Client"].Description = "Client";
    }

    /// <summary>
    /// Accepts a DataRow from the EntityTable and adds the Client virtual field to the Entity.Fields Collection
    /// </summary>
    /// <param name="fields"></param>
    /// <param name="row"></param>
    private static void setClientName(TEntity eObj)
    {
      if (eObj.EntityType.ToLower().Equals("client"))     //set the client virtual field.
        eObj.Client = TAGFunctions.CString(eObj.OrigId);
      else
      {
        if (eObj.EntityOwner != null)
          eObj.Client = TAGFunctions.CString(eObj.EntityOwner.OrigId);
      }
    }

    /// <summary>
    /// takes fields associated with an entity in the entity table and constructs both the FullName and Shortname virtual fields. 
    /// Returns the full name as the function value, and the short name as an output parameter
    /// </summary>
    /// <param name="entityID">Entity.Entity</param>
    /// <param name="entityType">Entity.EntityType</param>
    /// <param name="entityName">Entity.Legalname</param>
    /// <param name="firstName">Entity.FirstName</param>
    /// <param name="middleName">Entity.MiddleName</param>
    /// <param name="fein">Entity.FEIN</param>
    /// <param name="altName">Entity.AlternateName</param>
    /// <param name="altID">Entity.AlternateID</param>
    /// <param name="shortName">Output parameter to return the short name</param>
    /// <returns></returns>
    private static string makeFullName(string entityID, string entityType, string entityName, string firstName, string middleName,
      string fein, string altName, string altID, out string shortName)
    {
      string fullName = string.Empty;
      if (TAGFunctions.inList(entityTypesWithNames, entityType))
      {
        if (entityName == null)
          fullName = firstName;
        else
          if (firstName == null)
            fullName = entityName;
          else
            fullName = entityName + ", " + firstName;
        shortName = fullName;
        if (middleName.Length > 0)
          fullName += " " + middleName;
        if (altName.Length > 0)
          fullName += " (" + altName + ")";
        if (fein.Length > 0)
          fullName += " - " + fein;
      }
      else
        if (TAGFunctions.inList(entityTypesOfOrganization, entityType))
        {
          shortName = entityName;
          if (altName.Length > 0)
            shortName = altName;
          fullName = shortName;
          string companyID = fein;
          if (altID.Length > 0)
            companyID = altID;
          if (companyID.Length > 0)
            fullName += " (" + companyID + ")";
          if (entityID != entityName)
            if (!TAGFunctions.IsDemoDatabase)
              fullName += " {" + entityID + "}";
        }
        else
        {
          fullName = entityName;
          shortName = entityName;
        }
      return fullName;
    }
    private static string GUIStatusLine()
    {
      return string.Format("Copyright (C) 2009, 2010 TAG Backoffice, LLC. Version={0}, DBName={1}", TAGBOSSVersion(), databaseName);
    }
    private static string TAGBOSSVersion()
    {
      Assembly assem = Assembly.GetEntryAssembly();
      AssemblyName assemName = assem.GetName();
      Version ver = assemName.Version;
      return ver.ToString();
    }
    private static string getFormattedKey(string key, string itemID)
    {
      char colon = ':';
      if (key == null)
        return itemID;
      if (key.Contains(colon))  // has an index
      {
        int loc = key.IndexOf(colon);
        string index = key.Substring(loc);
        return string.Format("{0}{1}", itemID, index);
      }
      else
        if (key.Equals(itemID, StringComparison.CurrentCultureIgnoreCase))  // they are the same case-insensitively
          return itemID;  // itemId is propercased version, return that
        else
          return key;
    }
    #endregion string functions

    #region parsing functions
    /// <summary>
    /// Can these two types be compared (e.g. datetime to datetime or numeric to numeric)?
    /// </summary>
    /// <param name="t1">Type 1</param>
    /// <param name="t2">Type 2</param>
    /// <returns></returns>
    private static bool canCompare(Type t1, Type t2, out string compareType)
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

    private static bool failsTest(string operand1, string op, string operand2, object pValue)
    {
      return !passesTest((object)operand1, op, (object)operand2, pValue);
    }

    /// <summary>
    /// Function that gives some specific format to a number, date, money
    /// </summary>
    /// <param name="field"></param>
    /// <param name="length"></param>
    /// <param name="fill"></param>
    /// <returns></returns>
    private static string format(string field, int length, string fill)
    {
      //--@Fill can be '0' or '00' filled (for numbers) or character/space (for left justified) or date (eg. mmddyyyyy, yymd, m/d/yy, mm-dd-yyyy) or 'worddollar'
      string format = "";
      if (field == null)
      {
        field = "";
      }
      field = field.TrimEnd();
      field = field.TrimStart();

      if (fill.Contains("m") || fill.Contains("y") || fill.Contains("q") || fill.Contains("h"))
      {
        if (field.Equals(""))
        {
          formatString(field, length, " ");
        }
        else
        {
          DateTime date = DateTime.Parse(field);

          format = fill.Replace("mm", replicate(2 - (date.Month.ToString().Length)) + date.Month.ToString());
          format = format.Replace("m", date.Month.ToString());
          format = format.Replace("qq", replicate(2 - (quarter(date.Month).Length)) + quarter(date.Month));
          format = format.Replace("q", quarter(date.Month));
          format = format.Replace("yyyy", replicate(4 - (date.Year.ToString().Length)) + date.Year.ToString());
          format = format.Replace("yy", date.Year.ToString().Substring(date.Year.ToString().Length - 2, 2));
          format = format.Replace("dd", replicate(2 - (date.Day.ToString().Length)) + date.Day.ToString());
          format = format.Replace("d", date.Day.ToString());
          format = format.Replace("hh", replicate(2 - (date.Hour.ToString().Length)) + date.Hour.ToString());
          format = format.Replace("nn", replicate(2 - (date.Minute.ToString().Length)) + date.Minute.ToString());
        }
      }
      else
      {
        if (field.Length > length)
        {
          field = field.Substring(0, length - 1);
          field = field.TrimEnd();

          bool negativeYN = false;
          if (field.IndexOf("%-%") != 0 && (fill.Equals("0") || fill.Equals("00")))
          {
            negativeYN = true;
            field = field.Replace("-", "");
          }

          bool noDecimalYN = false;
          if (fill.Equals("00") || fill.Equals(".."))
          {
            noDecimalYN = true;
            if (fill.Equals("00"))
            {
              fill = "0";
            }
            else
            {
              fill = "";
            }
            double fieldMoney = Double.Parse(field) * 100;
            Math.Round(fieldMoney, 0);
            field = fieldMoney.ToString().Replace(".00", "");
            field = field.Replace(",", "");
          }

          string fillResult = "";
          if (fill.Equals("0"))
          {
            fillResult = "";
          }

          string rightFill = fill.Substring(fill.Length - 1, 1);
          for (int i = 1; i < length - field.Length; i++)
          {
            rightFill += rightFill;
          }

          string fillResult2 = "";
          if (fill != "0")
          {
            fillResult2 = "";
          }
          format = fillResult + rightFill + fillResult2;

          if (negativeYN)
          {
            format = "-" + format.Substring(2);
          }
        }

      }

      if (fill.Equals("WordDollar"))
      {
        ArrayList parts = new ArrayList();
        string fixedAmount = "";
        double moneyAmount = 0.0;

        fixedAmount = formatString(field, 20, "0");
        moneyAmount = Double.Parse(field);

        parts temp = new parts();

        temp.Part = "Tens";
        if (moneyAmount >= 1)
        {
          temp.Amount = fixedAmount.Substring(16, 2);
          parts.Add(temp);
        }
        temp = new parts();
        temp.Part = "Hundreds";
        if (moneyAmount >= 100)
        {
          temp.Amount = fixedAmount.Substring(15, 1);
          parts.Add(temp);
        }
        temp = new parts();
        temp.Part = "TenThousands";
        if (moneyAmount >= 1000)
        {
          temp.Amount = fixedAmount.Substring(13, 2);
          parts.Add(temp);
        }
        temp = new parts();
        temp.Part = "HundredThousands";
        if (moneyAmount >= 100000)
        {
          temp.Amount = fixedAmount.Substring(12, 1);
          parts.Add(temp);
        }
        temp = new parts();
        temp.Part = "TenMillions";
        if (moneyAmount >= 1000000)
        {
          temp.Amount = fixedAmount.Substring(10, 2);
          parts.Add(temp);
        }
        temp = new parts();
        temp.Part = "HundredMillions";
        if (moneyAmount >= 100000000)
        {
          temp.Amount = fixedAmount.Substring(9, 1);
          parts.Add(temp);
        }

        temp = null;
        foreach (parts p in parts)
        {

          string amountStr = p.Amount.ToString().Substring(0, 1);
          if (p.Amount.Length == 2)
          {
            switch (amountStr)
            {
              case "2": p.FirstWord = "Twenty";
                break;
              case "3": p.FirstWord = "Thirty";
                break;
              case "4": p.FirstWord = "Fourty";
                break;
              case "5": p.FirstWord = "Fifty";
                break;
              case "6": p.FirstWord = "Sixty";
                break;
              case "7": p.FirstWord = "Seventy";
                break;
              case "8": p.FirstWord = "Eighty";
                break;
              case "9": p.FirstWord = "Ninety";
                break;
              default: break;
            }
          }
        }

        foreach (parts p in parts)
        {

          string amountStr = p.Amount.ToString().Substring(p.Amount.ToString().Length - 1, 1);
          if (p.Amount.Length == 1 || !(amountStr.Substring(0, 1).Equals("1")))
          {
            switch (amountStr)
            {
              case "1": p.SecondWord = "one";
                break;
              case "2": p.SecondWord = "two";
                break;
              case "3": p.SecondWord = "three";
                break;
              case "4": p.SecondWord = "four";
                break;
              case "5": p.SecondWord = "five";
                break;
              case "6": p.SecondWord = "six";
                break;
              case "7": p.SecondWord = "seven";
                break;
              case "8": p.SecondWord = "eight";
                break;
              case "9": p.SecondWord = "nine";
                break;
              default: break;
            }
          }
        }

        foreach (parts p in parts)
        {
          if (p.FirstWord == null)
          {
            if (p.SecondWord != null)
            {
              string secondWordLeft = p.SecondWord.Substring(0, 1);
              p.SecondWord = secondWordLeft.ToUpper() + p.SecondWord.Substring(1);
            }
          }
        }

        foreach (parts p in parts)
        {
          if (p.SecondWord != null)
          {
            if (p.FirstWord != null)
            {
              p.FirstWord = p.FirstWord + "-";
            }
          }
        }

        foreach (parts p in parts)
        {

          string amountStr = p.Amount.ToString();
          if (p.Amount.Length == 2 && !(amountStr.Substring(0, 1).Equals("1")))
          {
            switch (amountStr)
            {
              case "10": p.SecondWord = "Ten";
                break;
              case "11": p.SecondWord = "Eleven";
                break;
              case "12": p.SecondWord = "Twelve";
                break;
              case "13": p.SecondWord = "Thirteen";
                break;
              case "14": p.SecondWord = "Fourteen";
                break;
              case "15": p.SecondWord = "Fifteen";
                break;
              case "16": p.SecondWord = "Sixteen";
                break;
              case "17": p.SecondWord = "Seventeen";
                break;
              case "18": p.SecondWord = "Eighteen";
                break;
              case "19": p.SecondWord = "Nineteen";
                break;
              default: break;
            }
          }
        }

        foreach (parts p in parts)
        {
          if (p.FirstWord != null || p.SecondWord != null)
          {
            if (p.FirstWord == null)
            {
              p.FirstWord = "";
            }
            if (p.SecondWord == null)
            {
              p.SecondWord = "";
            }
            p.CombineWord = p.FirstWord + p.SecondWord;
          }
        }

        string result1 = "", result2 = "", result3 = "", result4 = "", result5 = "", result6 = "";
        foreach (parts p in parts)
        {

          if (p.Part.Equals("HundredMillions"))
          {
            if (p.CombineWord != null)
            {
              result1 = p.CombineWord + " Hundred ";
            }
          }

          if (p.Part.Equals("TenMillions"))
          {
            if (p.CombineWord != null)
            {
              result2 = p.CombineWord + " Million ";
            }
          }

          if (p.Part.Equals("HundredThousands"))
          {
            if (p.CombineWord != null)
            {
              result3 = p.CombineWord + " Hundred ";
            }
          }


          if (p.Part.Equals("TenThousands"))
          {
            if (p.CombineWord != null)
            {
              result4 = p.CombineWord + " Thousand ";
            }
          }


          if (p.Part.Equals("Hundreds"))
          {
            if (p.CombineWord != null)
            {
              result5 = p.CombineWord + " Hundred ";
            }
          }

          if (p.Part.Equals("Tens"))
          {
            if (p.CombineWord != null)
            {
              result6 = p.CombineWord;
            }
          }

        }

        format = result1 + result2 + result3 + result4 + result5 + result6 + " Dollars and " + fixedAmount.Substring(19, 2) + " cents";
      }

      char Char1 = (char)146;
      char Char2 = (char)184;
      format = format.Replace(Char1.ToString(), "\'");
      format = format.Replace(Char2.ToString(), ",");

      return format;
    }

    /// <summary>
    /// Parses the validation string into its components (operand operator operand) 
    /// </summary>
    /// <param name="validationString"></param>
    /// <param name="operand1"></param>
    /// <param name="op"></param>
    /// <param name="operand2"></param>
    /// <returns></returns>
    private static bool parseValidationString(string validationString, ref string operand1, ref string op, ref string operand2)
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
      foreach (string opEntry in operatorList)
      {
        if (tmpValidationString.Contains(opEntry))
        {
          tmpValidationString = tmpValidationString.Replace(opEntry, replaceDelim + opEntry + replaceDelim);
          break; //added by Leonardo
        }
      }
      string[] delimiters = { replaceDelim };
      string[] tokenList = TAGFunctions.parseString(tmpValidationString, delimiters);
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
          foreach (string s in operatorList)
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

    private static bool passesTest(object operand1, string op, object operand2)
    {
      return passesTest(operand1, op, operand2, null);
    }

    private static bool passesTest(string operand1, string op, string operand2)
    {
      return passesTest(operand1, op, operand2, null);
    }

    private static bool passesTest(string operand1, string op, string operand2, object pValue)
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
    private static bool passesTest(object pOperand1, string op, object pOperand2, object pValue)
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
      string myType = DATATYPESTRING;
      Type t = o1.GetType();
      op1IsString = (t.Name.ToLower() == DATATYPESTRING);
      t = o2.GetType();
      op2IsString = (t.Name.ToLower() == DATATYPESTRING);
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
        case EQ:
          if (bCanCompare)
            if (myType == DATATYPEDOUBLE)
              testPasses = Convert.ToDouble(o1) == Convert.ToDouble(o2);
            else
              if (myType == DATATYPEDATETIME)
                testPasses = Convert.ToDateTime(o1) == Convert.ToDateTime(o2);
              else
                testPasses = (tCompare == 0);
          else
            testPasses = (tCompare == 0);
          break;
        case LT:
          if (bCanCompare)
            if (myType == DATATYPEDOUBLE)
              testPasses = Convert.ToDouble(o1) < Convert.ToDouble(o2);
            else
              if (myType == DATATYPEDATETIME)
                testPasses = Convert.ToDateTime(o1) < Convert.ToDateTime(o2);
              else
                testPasses = (tCompare < 0);
          else
            testPasses = (tCompare < 0);
          break;
        case LE:
          if (bCanCompare)
            if (myType == DATATYPEDOUBLE)
              testPasses = Convert.ToDouble(o1) <= Convert.ToDouble(o2);
            else
              if (myType == DATATYPEDATETIME)
                testPasses = Convert.ToDateTime(o1) <= Convert.ToDateTime(o2);
              else
                testPasses = (tCompare <= 0);
          else
            testPasses = (tCompare <= 0);
          break;
        case GT:
          if (bCanCompare)
            if (myType == DATATYPEDOUBLE)
              testPasses = Convert.ToDouble(o1) > Convert.ToDouble(o2);
            else
              if (myType == DATATYPEDATETIME)
                testPasses = Convert.ToDateTime(o1) > Convert.ToDateTime(o2);
              else
                testPasses = (tCompare > 0);
          else
            testPasses = (tCompare > 0);
          break;
        case GE:
          if (bCanCompare)
            if (myType == DATATYPEDOUBLE)
              testPasses = Convert.ToDouble(o1) >= Convert.ToDouble(o2);
            else
              if (myType == DATATYPEDATETIME)
                testPasses = Convert.ToDateTime(o1) >= Convert.ToDateTime(o2);
              else
                testPasses = (tCompare >= 0);
          else
            testPasses = (tCompare >= 0);
          break;
        case NE:
          if (bCanCompare)
            if (myType == DATATYPEDOUBLE)
              testPasses = Convert.ToDouble(o1) != Convert.ToDouble(o2);
            else
              if (myType == DATATYPEDATETIME)
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
    private static bool stringEquals(string s1, string s2)
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
    private static bool objectEquals(object o1, object o2)
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
          case DATATYPESTRING:
            bEquals = (o1.ToString().ToLower() == o2.ToString().ToLower());
            break;
          case DATATYPEINTEGER:
            bEquals = (CInt(o1) == CInt(o2));
            break;
          case DATATYPEDECIMAL:
            bEquals = (CDecimal(o1) == CDecimal(o2));
            break;
          case DATATYPEDOUBLE:
            bEquals = (CDouble(o1) == CDouble(o2));
            break;
          case DATATYPEDATETIME:
            bEquals = (CDateTime(o1) == CDateTime(o2));
            break;
          case DATATYPEBOOLEAN:
            bEquals = (CBoolean(o1) == CBoolean(o2));
            break;
          case DATATYPETABLEHEADER:
            bEquals = (((TableHeader)o1).CompareTo((TableHeader)o2)) == 0;
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
    private static string actionStatusLine(string actionName, string workflowStepID, int securityHandle,
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

    private static bool parseActionStatusLine(string statusLine, out string actionName,
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
      pos1 = statusLine.IndexOf(cCOLON);
      if (pos1 <= 0)   // no colon was found
        message = "No action name was found";
      else
      {
        actionName = statusLine.Substring(0, pos1);
        pos2 = statusLine.IndexOf(cCOLON, pos1 + 1);
        if (pos2 <= 0)
          message = "No workflow step ID was found";
        else
        {
          workflowStepID = statusLine.Substring(pos1 + 1, pos2 - pos1 - 1);
          pos3 = statusLine.IndexOf(cCOLON, pos2 + 1);
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

    private static string[] parseString(string insideString)
    {
      string[] delimiters = { "|", ",", cLISTSEPARATOR };
      return parseString(insideString, delimiters);
    }

    private static string[] parseString(string strString, string[] delimiters)
    {
      /*
       * takes a string of delimited tokens and breaks them down into an array.
       * Note that parentheses are used to indicate a token that should not be broken apart.
       */
      ArrayList arrList = new ArrayList();
      string[] strList = null;
      int i, j;
      char[] cDelimList = new char[delimiters.GetLength(0)];
      for (i = 0; i < delimiters.GetLength(0); i++)
        cDelimList[i] = Convert.ToChar(delimiters[i]);
      StringBuilder strStringList = new StringBuilder(strString);
      StringBuilder token = new StringBuilder(strString.Length);
      int parenDepth = 0;
      int delimDepth = 0;
      for (i = 0; i < strStringList.Length; i++)      // walk through the incoming string one character at a time
      {
        switch (strStringList[i])
        {
          case cLEFT:                                     // left paren?
            parenDepth++;
            //if (parenDepth > 0)                 // if we are in the middle of a paren-delimited token, we keep the paren
            token.Append(strStringList[i]);
            break;
          case cRIGHT:
            //if (parenDepth > 0)                 // same with ending paren
            token.Append(strStringList[i]);
            parenDepth--;
            break;
          //case cLEFTDELIM:
          //  delimDepth++;
          //  token.Append(strStringList[i]);
          //  break;
          //case cRIGHTDELIM:
          //  delimDepth--;
          //  token.Append(strStringList[i]);
          //  break;
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
                string s = token.ToString();     // pick up the token we have creat
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

    private static bool isFunction(string checkString)
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
      if (s[0] == FUNCTIONCHAR)                // does it start with the function prefix?
      {
        for (int i = 1; i < s.Length; i++)
        {
          switch (s[i])
          {
            case cLEFT:
              if (functionNameStarted && !functionNameEnded)
                functionNameEnded = true;
              if (parendepth == 0)    // if we are starting a new pair and we are not already in one...
                numberParenTokens++;  // then add to the number of "sister" pairs
              parendepth++;           // we are now one pair deeper
              break;
            case cRIGHT:
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

    private static string getFunctionName(string expressionIn, out string insideString)
    {
      string expression = expressionIn.Trim();

      string functionName = string.Empty;
      bool leftFound = true;
      int index = 0;
      int insideLength = expression.Length;
      StringBuilder sbExpression = new StringBuilder(expression);
      StringBuilder token = new StringBuilder(expression.Length);
      if (sbExpression[0] == FUNCTIONCHAR)
      {
        index++;
        while (index < sbExpression.Length && sbExpression[index] != cLEFT)
          token.Append(sbExpression[index++]);
        if (index == sbExpression.Length || sbExpression[index] != cLEFT)
          leftFound = false;
        functionName = token.ToString();
        insideLength = insideLength - index - 2;
        if (sbExpression[sbExpression.Length - 1] != cRIGHT || !leftFound)
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

    private static bool inList(string[] values, string value)
    {
      if (string.IsNullOrEmpty(value) || values == null)
        return false;
      for (int i = 0; i < values.GetLength(0); i++)
        if (values[i] != null && values[i].Equals(value, StringComparison.CurrentCultureIgnoreCase))
          return true;
      return false;
    }

    private static string stripBadCharacters(string name)
    {
      string newName = name;
      foreach (string badChar in badFieldCharList)
      {
        name = name.Replace(badChar, "");
      }
      return name;
    }

    private static object stripComment(object val)
    {
      return stripComment(val, true);
    }

    private static object stripComment(object val, bool trimResults)
    {
      string comment;
      return stripComment(val, trimResults, out comment);
    }
    private static object stripComment(object val, bool trimResults, out string comment)
    {
      comment = string.Empty;
      if (val == null)
        return val;
      if (val.GetType() != typeof(string))
        return val;
      string strReturn = val.ToString();
      if (strReturn.StartsWith(cLEFT.ToString()) && // is it enclosed in parens? Then don't parse the inside
          strReturn.EndsWith(cRIGHT.ToString()))
        return strReturn;
      int loc = strReturn.IndexOf(COMMENTCHAR);
      if (loc == -1)
        return strReturn;
      strReturn = strReturn.Substring(0, loc);
      comment = strReturn.Substring(loc + 1);
      if (trimResults)
        strReturn = strReturn.Trim();
      return strReturn;
    }

    private static bool isAttributeReference(object val)
    {
      return beginsWith(val, ATTR_1_AT_CHAR);
    }

    private static bool beginsWith(object val, string s)
    {
      if (s == null)
        return false;
      if (val == null)
        return false;
      if (val.GetType() != typeof(string))
        return false;
      return (((string)val).Trim().StartsWith(s));
    }

    private static bool beginsWith(object val, char c)
    {
      return beginsWith(val, c.ToString());
    }

    /// <summary>
    /// decides whether to process a conditional item
    /// </summary>
    /// <param name="targetItem"></param>
    /// <param name="conditionalDefaultItemID"></param>
    /// <returns></returns>
    private static bool processDefaultItem(Item targetItem, string conditionalDefaultItemID)
    {
      DictionaryFunctions funcs = new DictionaryFunctions();
      string conditionString;
      string defaultID = getFunctionName(conditionalDefaultItemID, out conditionString);
      string[] parts = parseString(conditionString, new string[] { ",", "~" });
      string myCondition = parts[1];

      // now, if this is a function and there are no prefix characters, then add the function prefix
      if (myCondition == null)
        myCondition = string.Empty;
      if (!myCondition.StartsWith(TAGFunctions.ATTRIBUTECHAR.ToString()) && !myCondition.StartsWith(TAGFunctions.FUNCTIONCHAR.ToString()))
        myCondition = TAGFunctions.FUNCTIONCHAR.ToString() + myCondition;
      // and now make the call
      //      return CBoolean(funcs.evaluateExpression(myCondition, targetItem, 0), false);          

      bool returnValue = false;
      try
      {
        returnValue = CBoolean(funcs.evaluateExpression(myCondition, targetItem, 0), false);
      }
      catch (Exception ex)
      {
        if (bypassFunctionError)
          return false;
        else
        {
          TAGExceptionMessage tm = new TAGExceptionMessage("Default Processing", "processDefaultItem", "Error processing Conditional Default <" + ex.Message + ">");
          tm.AddParm(targetItem.OriginalID);
          tm.AddParm(conditionalDefaultItemID);
          throw new Exception(tm.ToString());
        }
      }
      return returnValue;
    }

    private static bool processDefaultItem(TItem targetItem, TEntity eObj, string conditionalDefaultItemID)
    {
      DictionaryFunctions funcs = new DictionaryFunctions();
      //funcs.SourceEntity = eObj;

      string conditionString;
      string defaultID = getFunctionName(conditionalDefaultItemID, out conditionString);
      string[] parts = parseString(conditionString, new string[] { ",", "~" });
      string myCondition = parts[1];

      // now, if this is a function and there are no prefix characters, then add the function prefix
      if (myCondition == null)
        myCondition = string.Empty;
      if (!myCondition.StartsWith(TAGFunctions.ATTRIBUTECHAR.ToString()) && !myCondition.StartsWith(TAGFunctions.FUNCTIONCHAR.ToString()))
        myCondition = TAGFunctions.FUNCTIONCHAR.ToString() + myCondition;

      bool returnValue = false;
      try
      {
        returnValue = CBoolean(funcs.evaluateExpression(myCondition, null, targetItem, eObj, 0), false);
      }
      catch (Exception ex)
      {
        if (bypassFunctionError)
        {
          //Console.WriteLine(ex.Message);
          //Console.WriteLine(ex.StackTrace);
          return false;
        }
        else
        {
          TAGExceptionMessage tm = new TAGExceptionMessage("Default Processing", "processDefaultItem", "Error processing Conditional Default <" + ex.Message + ">" + "<" + ex.StackTrace + ">");
          tm.AddParm(targetItem.OrigId);
          tm.AddParm(conditionalDefaultItemID);
          throw new Exception(tm.ToString());
        }
      }
      return returnValue;
    }

    private static bool processBlockInheritYN(string myCondition, TItem targetItem, TEntity eObj, TAttribute aObj)
    {
      DictionaryFunctions funcs = new DictionaryFunctions();
      TParameterProcessor tparam = new TParameterProcessor();

      // now, if this is a function and there are no prefix characters, then add the function prefix
      if (myCondition == null)
        myCondition = string.Empty;
      if (!myCondition.StartsWith(TAGFunctions.ATTRIBUTECHAR.ToString()) && !myCondition.StartsWith(TAGFunctions.FUNCTIONCHAR.ToString()))
        myCondition = TAGFunctions.FUNCTIONCHAR.ToString() + myCondition;

      bool returnValue = false;
      try
      {
        if(myCondition.Contains(TAGFunctions.ATTR_2_AT_CHAR))
          myCondition = tparam.getSolvedParameter(myCondition, eObj, targetItem, aObj);

        returnValue = CBoolean(funcs.evaluateExpression(myCondition, null, targetItem, eObj, 0), false);
      }
      catch (Exception ex)
      {
        if (bypassFunctionError)
        {
          //Console.WriteLine(ex.Message);
          //Console.WriteLine(ex.StackTrace);
          return false;
        }
        else
        {
          TAGExceptionMessage tm = new TAGExceptionMessage("BlockInheritYN processing", "processBlockInheritYN", "Error processing BlockInherit <" + ex.Message + ">" + "<" + ex.StackTrace + ">");
          tm.AddParm(myCondition);
          tm.AddParm(targetItem.OrigId);
          tm.AddParm(eObj.OrigId);
          throw new Exception(tm.ToString());
        }
      }
      return returnValue;
    }

    private static object processFunction(string functionToProcess, TAttribute aObj, TItem attributeTable, TEntity eObj)
    {
      DictionaryFunctions funcs = new DictionaryFunctions();
      //funcs.SourceEntity = eObj;

      //string myCondition;
      //string defaultID = getFunctionName(functionToProcess, out myCondition);

      // now, if this is a function and there are no prefix characters, then add the function prefix
      if (functionToProcess == null)
        functionToProcess = string.Empty;
      if (!functionToProcess.StartsWith(TAGFunctions.ATTRIBUTECHAR.ToString()) && !functionToProcess.StartsWith(TAGFunctions.FUNCTIONCHAR.ToString()))
        functionToProcess = TAGFunctions.FUNCTIONCHAR.ToString() + functionToProcess;

      object returnValue = null;
      try
      {
        returnValue = funcs.evaluateExpression(functionToProcess, aObj, attributeTable, eObj, 0);
      }
      catch (Exception ex)
      {
        if (bypassFunctionError)
          return false;
        else
        {
          Console.WriteLine(ex.Message);
          Console.WriteLine(ex.StackTrace);
          //TAGExceptionMessage tm = new TAGExceptionMessage("Function Processing", "processFunction", "Error processing Function <" + ex.Message + ">" + "<" + ex.StackTrace + ">");
          //tm.AddParm(targetItem.OrigId);
          //tm.AddParm(functionToProcess);
          //throw new Exception(tm.ToString());
        }
      }
      return returnValue;
    }

    /// <summary>
    /// used to validate data entry of any id (Item, Entity, etc.)
    /// </summary>
    /// <param name="id">id to be validated</param>
    /// <returns></returns>
    private static string isValidID(string id)
    {
      return isValidID(id, VALIDIDLENGTH);
    }

    /// <summary>
    /// used to validate data entry of any id (Item, Entity, etc.)
    /// </summary>
    /// <param name="id">id to be validated</param>
    /// <param name="len">length to validate to</param>
    /// <returns></returns>
    private static string isValidID(string id, int len)
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
        if (!validIdCharacters.Contains(idTest[i]))
          return "ID can only have letters, numbers and a dash(-)";
      return string.Empty;
    }

    private static string propercaseItemKey(string itemKey, string originalID)
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
    private static object[,] Sort(object[,] myTable)
    {
      return Sort(myTable, false);
    }

    /// <summary>
    /// Sort a 2d Array of objects
    /// </summary>
    /// <param name="myTable">The table to sort</param>
    /// <param name="hasHeader">Does this table have a header? if so, don't sort that record.</param>
    /// <returns></returns>
    private static object[,] Sort(object[,] myTable, bool hasHeader)
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
    private static object[,] Sort(object[,] myTable, bool hasHeader, bool hasUniqueKey, string dataType)
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
    private static object[,] Sort(object[,] myTable, bool hasHeader, bool hasUniqueKey, string dataType, int column)
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
    private static object[] Row(object[,] tableLookup, object key)
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
    private static object[] Row(object[,] tableLookup, object key, bool hasHeader)
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
    private static object[] Row(object[,] tableLookup, int index)
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
    private static int RowSubscript(object[,] tableLookup, object key, bool hasHeader)
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
    private static object[,] RemoveRow(object[,] oldTable, object key)
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

    private static object[,] AppendTable(object[,] oldTable, object[,] newTable)
    {
      return AppendTable(oldTable, newTable, false, false);
    }

    private static object[,] AppendTable(object[,] oldTable, object[,] newTable, bool hasHeader)
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
    private static object[,] AppendTable(object[,] oldTable, object[,] newTable, bool oldHasHeader, bool newHasHeader)
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
    private static object[,] AppendRow(object[,] oldTable, object[] newRow)
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
    private static object[,] InsertRow(object[,] oldTable, object[] oldRow, bool atBeginning, bool hasHeader,
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
    private static object[,] Copy(object[,] oldTable)
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
    private static object[,] Copy(object[,] oldTable, int newLength, int newWidth)
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
    private static string ToList(object[] oList)
    {
      return ToList(oList, "");
    }

    /// <summary>
    /// Overload that accepts array of object instead of array of string
    /// </summary>
    /// <param name="oList"></param>
    /// <param name="delimiter"></param>
    /// <returns></returns>
    private static string ToList(object[] oList, string delimiter)
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
    private static string ToList(object[] oList, string delimiter, bool includeEmptyItems)
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
    private static string ToList(string[] strList)
    {
      return ToList(strList, "");
    }

    /// <summary>
    /// ToList method with strList, delimeter parameter
    /// </summary>
    /// <param name="strList"></param>
    /// <param name="delimiter"></param>
    /// <returns></returns>
    private static string ToList(string[] strList, string delimiter)
    {
      return ToList(strList, delimiter, true);
    }

    /// <summary>
    /// Convert an array of string to a commas separated list, with delimeters
    /// like: ('item1','item2',...) or (item1,item2,item3,...) or ("item1","item2","item3",...) etc
    /// </summary>
    /// <param name="strList"></param>
    /// <returns></returns>
    private static string ToList(string[] strList, string delimiter, bool includeEmptyItems)
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

    private static string ToTableList(string[] strList, bool includeEmptyItems)
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
            strResult = strResult + COLSEPARATORCHAR;
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
    private static string ToListFromTable(object[,] oList)
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
    private static string[] FromList(string strString)
    {
      /* 
       * Take a comma delimited string and turn it into an array of string
       */
      string[] arrList = null;
      string strStringList = string.Empty;

      if (!(strString == null || strString == string.Empty))    // if the list of itemTypes is not null or empty
      {                               // populate array of strString
        strStringList = strString.Replace(" ", "");
        strStringList = listSeparatorCharacters[0] + strStringList + listSeparatorCharacters[0];
        arrList = strStringList.Split(listSeparatorCharacters, StringSplitOptions.RemoveEmptyEntries);
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
    private static string filterList(string filter, string list)
    {
      string[] arrFilter = FromList(filter);
      string[] arrList = FromList(list);
      ArrayList newList = new ArrayList();
      foreach (string listToken in arrList)
        foreach (string filterToken in arrFilter)
          if (stringEquals(listToken, filterToken))
            newList.Add(listToken);
      string[] result = (string[])newList.ToArray();
      string resultString = ToList(result, cQUOTE);
      return resultString;
    }

    /// <summary>
    /// Tests to see if an AttributeTable contains the key in one of the rows in the first column
    /// </summary>
    /// <param name="key">Value to match against the first column</param>
    /// <param name="myTable">AttrbuteTable format table</param>
    /// <returns>True if the key is found, false if it is not</returns>
    private static bool ContainsKey(object key, object[,] myTable)
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
    private static object getValue(object key, int col, object[,] myTable)
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
    private static void commentDB(string db)
    {
      const string m_ConfigFileName = TAGFunctions.SERVERCONFIGFILENAME;
      string currentPath = TAGFunctions.getCurrentPath();

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
    private static void writeTextFile(string path, string fileName, string fileType, string contents)
    {
      if (fileName == null && fileName == string.Empty)
      {
        TAGExceptionMessage tm = new TAGExceptionMessage("TAGFunctions", "writeTextFile", "File name is empty");
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
        TAGExceptionMessage tm = new TAGExceptionMessage("TAGFunctions", "writeTextFile", ioe.Message + ":" + ioe.StackTrace);
        tm.AddParm(fileName);
        tm.AddParm(path);
        tm.AddParm(fileType);
        throw new Exception(tm.ToString());
      }
    }

    private static List<string[]> loadAvailableDatabases()
    {
      List<string[]> availableDBs = new List<string[]>();
      const string m_ConfigFileName = TAGFunctions.SERVERCONFIGFILENAME;

      string currentPath = TAGFunctions.getCurrentPath();
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

    #region Error processing functions
    /// <summary>
    /// Returns an error message if an error occured. 
    /// </summary>
    /// <param name="actionResult">Either a success message (TAGFunctions.ACTIONSUCCEEDED) or and error message</param>
    /// <param name="errorID">Module, name of button, or name of report that failed</param>
    /// <returns>Returns an empty InvalidEntries collection if no error. Otherwise returns the collection the error that occurred</returns>
    private static InvalidEntries reportError(string module, string actionResult, string errorID)
    {
      InvalidEntries returnResult = new InvalidEntries();
      if (actionResult != TAGFunctions.ACTIONSUCCEEDED)
      {
        InvalidEntry myError = new InvalidEntry();
        myError.ID = errorID;
        myError.Context = module;
        myError.ErrorMessage = actionResult;
        returnResult.Add(myError);
      }
      return returnResult;
    }
    private static Exception getInnerException(Exception ex)
    {
      Exception e = ex;
      while (e.InnerException != null)
        e = e.InnerException;
      return e;
    }
    private static void setFlag(string flagName, bool value)
    {
      if (flagName == null || flagName == string.Empty)
        return;
      switch (flagName.ToLower())
      {
        case "bypassfunctionerror":
          bypassFunctionError = value;
          break;
        case "throwdataconversionexception":
          ThrowDataConversionException = value;
          break;
        case "usenonxmlcall":
          UseNonXMLCall = value;
          break;
        case "bypassasyncactionprocessing":
          BypassAsyncActionProcessing = value;
          break;
        case "throwerrorondatatypeconversion":
          ThrowErrorOnDataTypeConversion = value;
          break;
        case "savezerotrans":
          SaveZeroTrans = value;
          break;
        case "usetagbossconfig":
          UseTAGBOSSConfig = value;
          break;
      }
    }
    #endregion

    #region valueHistory manipulation functions

    private static ValueHistoryCollection ValueHistoriesUnion(ValueHistoryCollection values1, ValueHistoryCollection values2)
    {
      /*
       * This function receives two value history collections, and creates one more finer based on them, with this
       * we can substitute the valuehistorycollection of both attributes, and assign the corresponding values depending on the attribute
       * and then merge tableheaders for each the new valuehistorycollection
       */

      ValueHistoryCollection vhUnion = new ValueHistoryCollection();
      vhUnion.MarkForDelete = false;

      if ((values1 == null || values1.Count == 0) && (values2 == null || values2.Count == 0))
        return vhUnion;
      else if (values1 == null || values1.Count == 0)
      {
        vhUnion = (ValueHistoryCollection)values2.Clone();
        vhUnion.MarkForDelete = false;
        return vhUnion;
      }
      else if (values2 == null || values2.Count == 0)
      {
        vhUnion = (ValueHistoryCollection)values1.Clone();
        vhUnion.MarkForDelete = false;
        return vhUnion;
      }

      //So we have non null and non empty values lists!
      //Let us then create the union list
      SortedDictionary<string, DateTime> vhUnionList = new SortedDictionary<string, DateTime>();

      //Let us add all the elements of the values1 list to the sorted dictionary
      foreach (ValueHistory vh in values1)
      {
        if (vh != null)
        {
          if (!(vhUnionList.ContainsKey(vh.StartDate.ToString("yyyyMMdd") + ":1")))
            vhUnionList.Add(vh.StartDate.ToString("yyyyMMdd") + ":1", vh.StartDate);

          if (!(vhUnionList.ContainsKey(vh.EndDate.ToString("yyyyMMdd") + ":0")))
            vhUnionList.Add(vh.EndDate.ToString("yyyyMMdd") + ":0", vh.EndDate);
        }
      }

      //Let us add all the elements of the values2 list to the sorted dictionary
      foreach (ValueHistory vh in values2)
      {
        if (vh != null)
        {
          if (!(vhUnionList.ContainsKey(vh.StartDate.ToString("yyyyMMdd") + ":1")))
            vhUnionList.Add(vh.StartDate.ToString("yyyyMMdd") + ":1", vh.StartDate);

          if (!(vhUnionList.ContainsKey(vh.EndDate.ToString("yyyyMMdd") + ":0")))
            vhUnionList.Add(vh.EndDate.ToString("yyyyMMdd") + ":0", vh.EndDate);
        }
      }

      List<string> keys = vhUnionList.Keys.ToList<string>();
      ValueHistory vhLast = null;

      DateTime startDate = TAGFunctions.PastDateTime;
      DateTime endDate = TAGFunctions.FutureDateTime;
      bool startDateIsEndOfInterval = false;
      bool addValueHistoryUnionElement = true;

      keys.Sort();
      for (int i = 0; i < keys.Count - 1; i++)
      {
        DateTime vhUnionElementBeginDate = vhUnionList[keys[i]];
        DateTime vhUnionElementEndDate = TAGFunctions.FutureDateTime;
        addValueHistoryUnionElement = true;

        startDateIsEndOfInterval = keys[i].EndsWith(":0");
        if (startDateIsEndOfInterval)
          startDate = vhUnionElementBeginDate.AddDays(1);
        else
          startDate = vhUnionElementBeginDate;

        if (i + 1 < keys.Count)
        {
          vhUnionElementEndDate = vhUnionList[keys[i + 1]];
          if (keys[i + 1].EndsWith(":1"))
          {
            if (startDateIsEndOfInterval)
              addValueHistoryUnionElement = false;
            else
              endDate = vhUnionElementEndDate.AddDays(-1);
          }
          else
            endDate = vhUnionElementEndDate;
        }

        //We have a complete time frame for a value, let's check if it is valid and add it to the valuesUnion collection
        if (addValueHistoryUnionElement)
          if (startDate <= endDate)
          {
            ValueHistory vh = new ValueHistory();
            vh.StartDate = startDate;
            vh.EndDate = endDate;
            if (vhLast != null)
              if (vhLast.EndDate == vh.StartDate)
              {
                if (vhLast.EndDate > vhLast.StartDate)
                {
                  ValueHistory vhOneDay = new ValueHistory();
                  vhOneDay.StartDate = vhLast.EndDate;
                  vhOneDay.EndDate = vhLast.EndDate;

                  vhLast.EndDate = vhLast.EndDate.AddDays(-1);
                  vhUnion.Add(vhOneDay);
                }
                vh.StartDate = vh.StartDate.AddDays(1);
              }

            if (vh.StartDate <= vh.EndDate)
            {
              vhUnion.Add(vh);
              vhLast = vh;
            }
          }
      }
      //Return the merged valueHistoryCollection
      return vhUnion;
    }

    private static ValueHistoryCollection ValueHistoriesAssignValues(ValueHistoryCollection valuesSrc, ValueHistoryCollection valuesTrg)
    {
      ValueHistoryCollection valuesUnionTrgValues = new ValueHistoryCollection();
      foreach (ValueHistory vhTrg in valuesTrg)
      {
        vhTrg.ValueType = TAGFunctions.VALUE;
        vhTrg.Value = null;
        foreach (ValueHistory vhSrc in valuesSrc)
          if (vhSrc.StartDate <= vhTrg.StartDate && vhTrg.EndDate <= vhSrc.EndDate)
          {
            vhTrg.ValueType = vhSrc.ValueType;
            vhTrg.Value = vhSrc.Value;
            break;
          }
        ValueHistory vhTrgNew = (ValueHistory)vhTrg.Clone();
        valuesUnionTrgValues.Add(vhTrgNew);
      }
      return valuesUnionTrgValues;
    }
    #endregion valueHistory manipulation functions

    #region EntityAttribute transformation functions
    private static EntityAttributesCollection filterEACByAttributes(EntityAttributesCollection eac, string attributes, string attributeValues)
    {
      if (attributes == null || attributes == string.Empty)
        return eac;
      string[] arrAttributes = TAGFunctions.FromList(attributes.ToLower());
      if (arrAttributes == null || arrAttributes.GetLength(0) == 0)
        return eac;
      bool checkValues = true;
      string[] arrValues = null;
      if (attributeValues == null || attributeValues == string.Empty)
        checkValues = false;
      else
      {
        arrValues = TAGFunctions.FromList(attributeValues);
        if (arrValues == null || arrValues.GetLength(0) == 0 || arrValues.GetLength(0) != arrAttributes.GetLength(0))
          checkValues = false;
      }
      foreach (Entity e in eac.Entities)
        for (int k = e.ItemTypes.Count - 1; k >= 0; k--)
        {
          ItemType it = e.ItemTypes[k];
          if (checkValues)
          {
            bool oldMarkForDelete = it.Items.MarkForDelete;
            it.Items.MarkForDelete = false;
            for (int k0 = it.Items.Count - 1; k0 >= 0; k0--)  // top to bottom since we may remove some
            {
              Item i = it.Items[k0];
              string removeKey = it.getKey(k0);
              bool removeItem = false;
              for (int k1 = 0; k1 < arrAttributes.GetLength(0); k1++)
              {
                string aID = arrAttributes[k1];
                if (i.Attributes.Contains(aID))
                {
                  TAGAttribute a = i.Attributes[aID];
                  if (!(TAGFunctions.CompareTo(a.Value, a.DataType, arrValues[k1], a.DataType) == 0))  // are the values equal?
                  {
                    removeItem = true;
                    break;
                  }
                }
                else
                {
                  removeItem = true;
                  break;
                }
              }
              if (removeItem)
                it.Items.Remove(removeKey);
            }
            it.MarkForDelete = oldMarkForDelete;
          }
          else
          {
            ItemType newIT = new ItemType();
            for (int i = 0; i < it.Items.Count; i++)  // changed from foreach to for so we can get oldkey
            {
              Item oldItem = it.Items[i];
              Item newItem = new Item();
              string oldKey = it.getKey(i); // LLA 9/9/2010: for compatibility with data pool with item indexes (salary:0)
              newItem.ID = oldItem.OriginalID;
              newItem.ItemHistoryRecords = oldItem.ItemHistoryRecords;
              newItem.Source = oldItem.Source;
              newItem.Description = oldItem.Description;
              newItem.Source = oldItem.Source;
              newItem.IsInherited = oldItem.IsInherited;
              newItem.LastModifiedBy = oldItem.LastModifiedBy;
              newItem.LastModifiedDateTime = oldItem.LastModifiedDateTime;
              foreach (string aID in arrAttributes)
              {
                if (oldItem.Attributes.Contains(aID))
                  newItem.Attributes.Add(oldItem.Attributes[aID]);
              }
              if (newItem.Attributes.Count > 0)
                newIT.Items.Add(oldKey, newItem);
            }
            string oldITID = it.OriginalID;
            bool markSave = e.ItemTypes.MarkForDelete;
            e.ItemTypes.MarkForDelete = false;
            e.ItemTypes.Remove(it.ID);
            e.ItemTypes.MarkForDelete = markSave;
            if (newIT.Items.Count > 0)
            {
              newIT.ID = oldITID;
              e.ItemTypes.Add(newIT);
            }
          }
        }
      return eac;
    }

    #endregion

    #endregion private evaluateFunction function calls

    #region private functions

    private static EntityAttributesCollection loadDictionaryEAC(TEntity[] tSysEntities)
    {
      EntityAttributesCollection DictEAC = new EntityAttributesCollection();
      TItem tDictItem = null;
      string DictEntityID = "dictionary";

      //TEntity[] tSysEntities = ad.SystemEntities();
      for (int i = 0; i < tSysEntities.GetLength(0); i++)
      {
        if (tSysEntities[i].Id == DictEntityID)
        {
          Entity DictEnt = new Entity();
          DictEnt.ID = DictEntityID;
          foreach (TIndexItem indexItem in tSysEntities[i].ItemIndex.List)
          {
            tDictItem = (TItem)indexItem.ItemIdx;

            if (tDictItem == null)
              continue;

            if (!DictEnt.ItemTypes.Contains(tDictItem.ItemType.Id))
            {
              ItemType DictIT = new ItemType();
              DictIT.ID = tDictItem.ItemType.Id;
              DictEnt.ItemTypes.Add(DictIT);
            }

            Item DictItem = new Item();
            DictItem.ID = tDictItem.Id;
            DictItem.StartDate = tDictItem.Entity.StartDate;
            DictItem.EndDate = tDictItem.Entity.EndDate;
            DictItem.EffectiveDate = tDictItem.Entity.EffectiveDate;

            foreach (TIndexItem attrObjIdx in tDictItem.getAttributeEnumerable())
            {
              TAttribute tAttribute = (TAttribute)attrObjIdx.ItemIdx;
              TAGAttribute attribute = new TAGAttribute();

              attribute.ID = tAttribute.Id;
              attribute.StartDate = Constants.PASTDATETIME;
              attribute.EndDate = Constants.FUTUREDATETIME;
              attribute.EffectiveDate = tAttribute.Item.Entity.EffectiveDate;
              attribute.IsIncluded = Constants.AnyOn(tAttribute.Flags, EAttributeFlags.IsIncluded);
              attribute.IsInherited = Constants.AnyOn(tAttribute.Flags, EAttributeFlags.IsInherited);
              attribute.Parent = DictItem;

              foreach (TValueHistory tValueHistory in tAttribute.History)
              {
                ValueHistory vh = new ValueHistory();
                vh.StartDate = tValueHistory.StartDate;
                vh.EndDate = tValueHistory.EndDate;
                vh.Value = tValueHistory.Value;
                vh.LastModifiedBy = tValueHistory.LastModifiedBy;
                vh.LastModifiedDateTime = tValueHistory.LastModifiedDateTime;

                if (vh.StartDate <= tSysEntities[i].EffectiveDate && tSysEntities[i].EffectiveDate <= vh.EndDate)
                {
                  attribute.StartDate = vh.StartDate;
                  attribute.EndDate = vh.EndDate;

                  attribute.OverwriteValue = true;
                  attribute.Value = vh.Value;
                  attribute.OverwriteValue = false;
                }

                attribute.Values.Add(vh);
              }

              DictItem.Add(attribute);
            }

            DictEnt.ItemTypes[tDictItem.ItemType.Id].Add(DictItem);
          }

          DictEAC.Entities.Add(DictEnt);
        }
      }

      return DictEAC;
    }

    #endregion private functions

    #region class definitions
    public class parts
    {
      string part;
      string amount;
      string firstWord;
      string secondWord;
      string combineWord;
      public string Part
      {
        get { return part; }
        set { part = value; }
      }
      public string Amount
      {
        get { return amount; }
        set { amount = value; }
      }
      public string FirstWord
      {
        get { return firstWord; }
        set { firstWord = value; }
      }
      public string SecondWord
      {
        get { return secondWord; }
        set { secondWord = value; }
      }
      public string CombineWord
      {
        get { return combineWord; }
        set { combineWord = value; }
      }
    }

    #endregion class definitions
  }
}