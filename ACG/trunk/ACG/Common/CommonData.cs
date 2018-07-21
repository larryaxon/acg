using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Globalization;

using ACG.Common.Logging;

namespace ACG.Common
{
  public class CommonData
  {
    public static int MAXIDLEMINUTES = 60 * 24; // one day  
    //public static bool useUSDateCulture = true;
    public static CultureInfo cultureInfoUS = CultureInfo.GetCultureInfo("en-US");
    public static bool useDBConfig = true;
    public static bool canChangeDB = true;
    public const string NEWTABLEFORMAT = "newtableformat";
    #region module data
    public const string cQUOTE = "'";
    public const string cCOLON = ":";
    public const string cLISTSEPARATOR = "~";
    private const char cFUNCTIONCHAR = '_';
    private static DateTime cNULLDATETIME = new DateTime(1, 1, 1);

    // XML string conversion constants
    public const string c_TO_XML_AMP = "&amp;";
    public const string c_TO_XML_LT = "&lt;";
    public const string c_TO_XML_GT = "&gt;";
    public const string c_TO_XML_QUOT = "&quot;";
    public const string c_TO_XML_APOS = "&apos;";

    public const string c_FROM_XML_AMP = "&";
    public const string c_FROM_XML_LT = "<";
    public const string c_FROM_XML_GT = ">";
    public const string c_FROM_XML_QUOT = "\"";
    public const string c_FROM_XML_APOS = "'";
    // end xml constants

    public const char cLEFT = '(';
    public const char cLEFTCURLY = '{';
    public const char cLEFTSQUARE = '[';
    public const char cRIGHT = ')';
    public const char cRIGHTCURLY = '}';
    public const char cRIGHTSQUARE = ']';
    public const char cDOUBLEQUOTE = '"';
    //private const char cLEFTDELIM = '{';
    //private const char cRIGHTDELIM = '}';
    private const char cEQUALS = '=';
    private const char cLESSTHAN = '<';
    private const char cGREATERTHAN = '>';
    private const char cNOT = '!';
    private const char cSPACE = ' ';
    public const string EQ = "==";
    public const string LT = "<";
    public const string GT = ">";
    public const string LE = "<=";
    public const string GE = ">=";
    public const string NE = "!=";
   
    public static string[] operatorList = { EQ, LE, GE, LT, GT, NE };
    public static string[] badFieldCharList = { "'", ",", "~", "@", "#", "\"" };
    public static string[] listSeparatorCharacters = { ",", ".", ROWSEPARATORCHAR.ToString(), COLSEPARATORCHAR.ToString() };  // standard separators for toList(), fromList()
    public static string validIdCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890-";
    public const int VALIDIDLENGTH = 50;
    public const string FORMATLONGDATETIME = "yyyy-MM-dd HH:mm:ss:fff";
    public const string FORMATLONGDATETIME2 = "yyyy-MM-dd HH:mm:ss.fff";

    public const string FORMATLONGDATETIMEFORFILENAME = "yyyyMMddHHmmssfff";
    public const string FORMATSHORTDATE = "yyyy-MM-dd";
    public const string FORMATALTSHORTDATE = "M-d-yyyy";
    public const string FORMATMYSHORTDATE = "M-yy";
    public static string[] ValidDateFormatList = new string[] { CommonData.FORMATLONGDATETIME, CommonData.FORMATLONGDATETIME2, CommonData.FORMATSHORTDATE, CommonData.FORMATALTSHORTDATE };
    public const string FORMATSHORTDATESQL = "23";

    private static DateTime dtPASTDATETIME = new DateTime(1900, 1, 1);
    private static DateTime dtFUTUREDATETIME = new DateTime(2100, 12, 31);
    private static int intMAXDEPTHSELFCALL = 255;    //Maximum recursive calls for this component
    private static string databaseName = string.Empty;

    public static string[] salutationList = new string[] { "Mr", "Ms", "Mrs", "Miss", "Dr", "Rev", "Sir" };
    public static string[] suffixList = new string[] { "Jr", "Sr", "II", "2", "2", "III", "Esq" };

    /*
     * The following flags are system-wide, mainly for debugging or testing purposes. 
     * By default, they are all turned off
     */
    private static bool bypassFunctionError = false;
    private static bool throwErrorOnDataTypeConversion = false;
    //private static bool saveDebugItem = false;
    private static bool throwDataConversionException = false;
    private static bool logAttributeChanges = false;
    private static bool useNonXMLCall = false;
    private static bool bypassAsyncActionProcessing = false;
    private static bool displayNonXMLOption = true;
    //private static bool useCCIConfig = true;

    /*
     * End system flags
     */
    #endregion module data

    #region public properties

    public static string[] badEntityIDChars = new string[] { " ", ",", "-", "'", ".", "/", "&" };
    public const string tableUSEROPTIONS = "UserOptions";
    public const string fieldUSER = "UserID";
    public const string fieldOPTIONTYPE = "OptionType";
    public const string fieldOPTIONNAME = "OptionName";
    public const string USEROPTIONTYPECOLUMNORDER = "SearchGridColumnOrder";
    public const string USEROPTIONDISPLAYFIELDS = "SearchGridDisplayFields";
    public const string USEROPTIONLASTCOLUMNSORTED = "SearchGridLastColumnSorted";
    public const string USEROPTIONSEARCHCRITERI = "SearchGridCriteria";

    public const string parmENCRYPTEDFIELDNAME = "encryptedfieldname";
    // time increments
    public const string TIMEINCREMENTMINUTE = "Minute";
    public const string TIMEINCREMENTTENTH = "Tenth";
    public const string TIMEINCREMENTQUARTER = "Quarter";

    public const string SERVERCONFIGFILEPROD = "DBProd.Config";
    public const string SERVERCONFIGFILEDEFAULT = "DB.Config";
    public static string SERVERCONFIGFILENAME =SERVERCONFIGFILEDEFAULT;
    public const string SERVERCONFIGPRODTOKENFILENAME = "Prod.dat";
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

    public static ArrayList AccessRightNames = new ArrayList { "Grant", "ReadWriteUpdateDelete", "ReadWriteUpdate", "ReadWrite", "UpdateWithApproval", "Create", Security.READONLY, Security.DENY }; 
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

    //SQL Data Types!
    public const string SQLTRUE = "1";
    public const string SQLFALSE = "0";

    public const string SQLBIGINT = "bigint";
    public const string SQLBIT = "bit";
    public const string SQLDATE = "date";
    public const string SQLDATETIME = "datetime";
    public const string SQLFLOAT = "float";
    public const string SQLDECIMAL = "decimal";
    public const string SQLIMAGE = "image";
    public const string SQLINT = "int";
    public const string SQLMONEY = "money";
    public const string SQLNVARCHAR = "nvarchar";
    public const string SQLOBJECT = "object";
    public const string SQLVARCHAR = "varchar";

    //SQL Actions
    public const string SQLDELETEROW = "Delete";
    public const string SQLUPDATEROW = "Update";

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
    public const string DATATYPECONDITION = "condition";
    public const string DEFAULTDATATYPE = "string";
    public static string[] ValidDataTypes = { DATATYPESTRING, DATATYPEDATETIME, DATATYPEDECIMAL, DATATYPEBOOLEAN, DATATYPEINTEGER, 
                                              DATATYPECONDITION, DATATYPELONG, DATATYPEMONEY, DATATYPEOBJECT };

    public const string HASTRANSACTIONLOCK = "True";
    public const string HASNOTRANSACTIONLOCK = "False";

    public static string DatabaseName
    {
      get { return databaseName; }
      set { databaseName = value; }
    }

    public static bool ThrowDataConversionException
    {
      get { return throwDataConversionException; }
      set { throwDataConversionException = value; }
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
      get { return bypassFunctionError; }
      set { bypassFunctionError = value; }
    }

    public static bool UseNonXMLCall
    {
      get { return useNonXMLCall; }
      set { useNonXMLCall = value; }
    }

    public static bool DisplayNonXMLOption
    {
      get { return displayNonXMLOption; }
    }
    public static bool BypassAsyncActionProcessing
    {
      get { return bypassAsyncActionProcessing; }
      set { bypassAsyncActionProcessing = value; }
    }

    public static bool ThrowErrorOnDataTypeConversion
    {
      get { return throwErrorOnDataTypeConversion; }
      set { throwErrorOnDataTypeConversion = value; }
    }

    /// <summary>
    /// Should we log changes to the attribute table?
    /// </summary>
    public static bool LogAttributeChanges
    {
      get { return logAttributeChanges; }
      set { logAttributeChanges = value; }
    }

    public static bool UseDBConfig
    {
      get { return useDBConfig; }
      set { useDBConfig = value; }
    }

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
  }
}
