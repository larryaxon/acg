using System;

namespace TAGBOSS.Common.Model
{
  public static class Constants
  {
    public const string ALL_ENTITY = "all";
    public const string DEFAULT_ENTITY = "default";
    public const string DEFAULT_ITEM = "default";
    public const string CONDITIONAL_ITEM = "default(";
    public const string DICTIONARY = "dictionary";
    public const string ENTITY_ITEMTYPE = "entity";

    public const string REF_ENTITY = "@@entity";
    public const string REF_ENTITYTYPE = "@@entitytype";

    public const string STARTDATE = "startdate";
    public const string ENDDATE = "enddate";
    public const string LASTMODIFIEDBY = "lastmodifiedby";
    public const string LASTMODIFIEDDATETIME = "lastmodifieddatetime";
    public const string INCLUDE = "include";
    public const string TABLEHEADER = "tableheader";
    public const string TABLEMOD = "tablemod";
    public const string REF_INHERIT = "refinherit";
    public const string FUNCTION = "func";
    public const string VALUE = "value";
    public const string ID_STARTDATE_FORMAT = "yyyyMMdd";
    public const int BOTTOMLEVEL = 0;

    public const string ATTR_AT_ENTITY = "entity";
    public const string ATTR_AT_ITEMTYPE = "itemtype";
    public const string ATTR_AT_ITEM = "item";
    public const string ATTR_AT_SOURCEITEM = "sourceitem";
    public const string ATTR_AT_ATTRIBUTE = "attribute";
    public const string ATTR_AT_ITEM_STARTDATE = "itemstartdate";
    public const string ATTR_AT_ITEM_ENDDATE = "itemenddate";
    public const string ATTR_AT_EFFECTIVE_DATE = "effectivedate";

    public const string ATTR_AT_ENTITYTYPE = "entitytype";
    public const string ATTR_AT_ENTITYOWNER = "entityowner";
    public const string ATTR_AT_LEGALNAME = "legalname";
    public const string ATTR_AT_FIRSTNAME = "firstname";
    public const string ATTR_AT_MIDDLENAME = "middlename";
    public const string ATTR_AT_ALTERNATENAME = "alternatename";
    public const string ATTR_AT_ALTERNATEID = "alternateid";
    public const string ATTR_AT_FEIN = "fein";
    public const string ATTR_AT_ENTITYSTARTDATE = "entitystartdate";
    public const string ATTR_AT_ENTITYENDDATE = "entityenddate";
    public const string ATTR_AT_SHORTNAME = "shortname";
    public const string ATTR_AT_FULLNAME = "fullname";
    public const string ATTR_AT_CLIENT = "client";
    public const string ATTR_AT_ADDRESS1 = "address1";
    public const string ATTR_AT_ADDRESS2 = "address2";
    public const string ATTR_AT_CITY = "city";
    public const string ATTR_AT_STATE = "state";
    public const string ATTR_AT_ZIP = "zip";

    public const char COMMENTCHAR = '^';
    public static DateTime PASTDATETIME = new DateTime(1900,1,1);
    public static DateTime FUTUREDATETIME = new DateTime(2100,12,31);

    #region This will become parameterized properties of the project
    public static int MaxSystemItems { get { return 10000; } }
    public static int MaxSystemAttributes { get { return 1000; } }
    public static int MaxEntities { get { return 10000; } }
    public static int MaxEntityItems { get { return 10000; } }
    public static int MaxEntityAttributes { get { return 1000; } }
    public static int QueueBlockSize { get { return 1000; } }
    public static int ThreadSleepTime{ get { return 500; } }
    #endregion This will become parameterized properties of the project

    public static EAttributeFlags SetOn(EAttributeFlags value, EAttributeFlags flag) 
    {
      return (EAttributeFlags)(value | flag); 
    }

    public static EAttributeFlags SetOff(EAttributeFlags value, EAttributeFlags flag) 
    {
      return (EAttributeFlags)(value & ~flag);
    }

    public static bool AnyOn(EAttributeFlags value, EAttributeFlags flag) 
    {
      return (value & flag) != 0; 
    }

    public static bool AllOn(EAttributeFlags value, EAttributeFlags flag) 
    { 
      return (value & flag) == flag; 
    }
  }
}
