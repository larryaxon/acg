using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using ACG.Common.Logging;

namespace ACG.App.Common
{
  public class CommonData : ACG.Common.CommonData
  {
/*
 * This class exposes the centreal library of static data points (mostly constants and some global flags). 
 * 
 * Please do NOT add any data/property to this class if it is possible to add it to ACG.Common.CommonData.
 * This way, we maintain an ever-expaning set of shared data. However, if there is something that has dependencies
 * to this local projecr, then you must add it here. 
 * 
 * In that case, it should be a static or public property. No non-static methods are allowed in this class.
 * 
 * No functions/methods should be added to this class
 */
    public enum NameTypes { Customer, Project, SubProject, TimeDetail, TimeSummary, Item, BudgetQuery, BudgetSummary, ResourceCost,
      BudgetEditProjectBudget, BudgetEditTimeEntry, ResourceTimeSummary, BilledInvoices};
    public enum TimeEntrySummaryMode { Daily, Weekly, ByProject };

    public static int MAXIDLEMINUTES = 15;

    public const string USER = "user";
    public const string CUSTOMER = "customer";

    // Time Entry
    public const string fieldCUSTOMERID = "CustomerID";
    public const string fieldPROJECTID = "ProjectID";
    public const string fieldSUBPROJECTID = "SubProjectID";
    public const string fieldRESOURCEID = "ResourceID";
    public const string fieldBILLINGCODE = "billingcode";
    public const string fieldTIMEID = "ID";
    public const string fieldTIMEDATE = "TimeWorkedDate";
    public const string fieldSTARTTIME = "StartTime";
    public const string fieldENDTIME = "EndTime";
    public const string fieldDESCRIPTION = "Description";
    public const string fieldINTERNALNOTES = "InternalNotes";
    public const string fieldENTEREDMINUTES = "EnteredTime";
    public const string fieldBILLEDMINUTES = "BilledTime";
    public const string fieldRATE = "Rate";
    public const string fieldCOST = "Cost";
    public const string fieldBILLEDAMOUNT = "BilledAmount";
    public const string fieldINVOICENUMBER = "InvoiceNumber";
    public const string fieldINVOICEDATE = "InvoiceDate";
    public const string fieldCREATEDATE = "CreateDate";
    public const string fieldLASTPAYDATE = "LastPayDateTime";
    public const string fieldLASTDATEPAID = "LastDatePaid";

    // ProjectBudget
    public const string fieldORIGINALBUDGETHOURS = "OriginalBudgetHours";
    public const string fieldREVISEDBUDGETHOURS = "RevisedBudgetHours";
    public const string fieldESTIMATEDHOURSTOCOMPLETE = "EstimatedHoursToComplete";
    public const string fieldORIGINALCOMPLETIONDATE = "OriginalCompletionDate";
    public const string fieldREVISEDCOMPLETIONDATE = "RevisedCompletionDate";

    public const string BILLABLE = "Billable";
    public const string TABLECUSTOMERS = "customers";
    public const string TABLEPROJECTS = "projects";
    public const string TABLESUBPROJECTS = "subprojects";
    public const string TABLERESOURCES = "resources";
    public const string TABLEBUDGET = "projectbudget";
    public const string TABLERATES = "rates";
    public const string TABLECUSTOMERRATES = "customerrates";

      /*
       *  the fields that passed from TABLENAME to its sub table
       */
    public static Dictionary<string, string[]> ACGTableFields = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase) 
      { { TABLECUSTOMERS, new string[] { fieldCUSTOMERID } },
        { TABLEPROJECTS, new string[] { fieldCUSTOMERID, fieldPROJECTID } },
        { TABLERESOURCES, new string[] {  } },
        { TABLESUBPROJECTS, new string[] { fieldCUSTOMERID, fieldPROJECTID, fieldSUBPROJECTID, fieldRESOURCEID }},
        { TABLEBUDGET, new string[] { }},
        { TABLERATES, new string[] { }},
        { TABLECUSTOMERRATES, new string[] { fieldCUSTOMERID }}};
    public static Dictionary<string, string[]> ACGTableTables = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase) 
      { { TABLECUSTOMERS, new string[] { TABLEPROJECTS }},
        { TABLEPROJECTS, new string[] { TABLESUBPROJECTS }},
        { TABLESUBPROJECTS, new string[] { TABLEBUDGET }},
        { TABLERESOURCES, new string[] { }},
        { TABLEBUDGET, new string[] { }},
        { TABLERATES, new string[] { }},
        { TABLECUSTOMERRATES, new string[] { TABLERATES }}};

    public const string DEFAULTDOMAIN = "ACG.com";
    public const string NEWTABLEFORMAT = "newtableformat";

    public const string NOCHANGE = "{NO_CHANGE}";
    public const string parmACCOUNT = "Account";
    public const string parmCONTEXT = "Context";
    public const string parmCRITERIA = "Criteria";
    public const string parmDATA = "data";
    public const string parmFIELDNAMES = "FieldNames";


    public const string fieldADDRESS1 = "Address1";
    public const string fieldADDRESS2 = "Address2";
    public const string fieldATTRIBUTES = "Attributes";
    public const string fieldCOLUMNNAME = "column_name";
    public const string fieldCONTRACTTERM = "ContractTerm";
    public const string fieldCITY = "City";
    public const string fieldCREATEDBY = "CreatedBy";
    public const string fieldCREATEDDATETIME = "CreateDate";
    public const string fieldCUSTOMER = "Customer";
    public const string fieldDEALERORSALESPERSON = "DealerOrSalesPerson";
    public const string fieldDETAILID = "DetailId";
    public const string fieldDIRTY = "Dirty";
    public const string fieldENTITY = "Entity";
    public const string fieldENTITYOWNER = "EntityOwner";
    public const string fieldENTITYTYPE = "EntityType";
    public const string fieldEXTERNALID = "ExternalID";
    public const string fieldEXTERNALSOURCE = "ExternalSource";
    public const string fieldEXTERNALSOURCETYPE = "ExternalSourceType";
    public const string fieldID = "ID";
    public const string fieldINSTALLDATE = "InstallDate";
    public const string fieldINTERNALID = "InternalID";
    public const string fieldITEMID = "ItemId";
    public const string fieldITEM = "Item";
    public const string fieldITEMHISTORY = "ItemHistory";
    public const string fieldITEMTYPE = "ItemType";
    public const string fieldLASTMODIFIEDBY = "LastModifiedBy";
    public const string fieldLASTMODIFIEDDATETIME = "LastModifiedDateTime";
    public const string fieldLASTSTATUSCHANGEDATE = "LastStatusChangeDate";
    public const string fieldLEGALNAME = "LegalName";
    public const string fieldLINENUMBER = "LineNumber";
    public const string fieldORDERID = "OrderId";
    public const string fieldORDERNAME = "OrderName";
    public const string fieldORDERTYPE = "OrderType";
    public const string fieldSECTIONAME = "Section";
    public const string fieldSHORTNAME = "ShortName";
    public const string fieldSTATE = "State";
    public const string fieldSTATUS = "Status";
    public const string fieldSUITE = "Suite";
    public const string fieldSYSTEMDATATYPE = "system_data_type";
    public const string fieldZIP = "Zip";
  }
}
