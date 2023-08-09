using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACG.Common;
using CCI.Sys.Data;
using OfficeOpenXml;
using static System.Net.WebRequestMethods;


namespace CCI.Sys.Processors
{
  public class ImportFileProcessorBase : IDisposable
  {

    const string APPSETTINGLOCALBASEFOLDER = "LocalBaseFolder";
    internal List<ACGFileInfo> _fileList = null;
    internal Dictionary<string, string> _dataTypes = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
    {
      { "ACCT_LEVEL", "int" },
      { "ACTIVITY_COMP_DATE", "date" },
      { "AIR_CHG_AMT", "decimal(8,2)" },
      { "BAL_FWD", "decimal(8,2)" },
      { "BAL_FWD_ADJ", "decimal(8,2)" },
      { "BEG_CHG_DATE", "date" },
      { "BILL_PERIOD_END", "date" },
      { "BILL_PERIOD_START", "date" },
      { "CHG_AMT", "decimal(8,2)" },
      { "CHG_BASIS", "decimal(8,2)" },
      { "CHG_QTY1_BILLED", "decimal(8,2)" },
      { "CHG_QTY1_USED", "decimal(8,2)" },
      { "CHG_QTY2_BILLED", "decimal(8,2)" },
      { "CHG_QTY2_USED", "decimal(8,2)" },
      { "CHG_RATE", "decimal(8,2)" },
      { "CONTRACT_EFF_DATE", "date" },
      { "CONTRACT_END_DATE", "date" },
      { "DATA_CHG_AMT", "decimal(8,2)" },
      { "DATE_ISSUED", "date" },
      { "DATE_RECEIVED_FROM_SP", "date" },
      { "DISC_CHG_AMT", "decimal(8,2)" },
      { "DISC_PCT", "decimal(8,2)" },
      { "DUE_DATE" , "date" },
      { "END_CHG_DATE", "date" },
      { "FEAT_CHG_AMT", "decimal(8,2)" },
      { "INV_DATE", "date" },
      { "LD_CHG_AMT", "decimal(8,2)" },
      { "MSG_CHG_AMT", "decimal(8,2)" },
      { "ORIG_INV_DATE", "date" },
      { "PMTS_APP_THRU_DATE", "date" },
      { "PMTS_RCVD", "decimal(8,2)" },
      { "PREV_BILL_AMT", "decimal(8,2)" },
      { "PRORATE_FACTOR", "decimal(8,2)" },
      { "ROAM_CHG_AMT", "decimal(8,2)" },
      { "ROAM_TAX_CHG_AMT", "decimal(8,2)" },
      { "SP_BAL_FWD", "decimal(8,2)" },
      { "SP_INV_LINE_NUM", "int" },
      { "SP_TOT_AMT_DUE", "decimal(8,2)" },
      { "SP_TOT_NEW_CHGS", "decimal(8,2)" },
      { "SVC_ESTABLISH_DATE", "date" },
      { "TAX_SUR_CHG_AMT", "decimal(8,2)" },
      { "TOT_AMT_DUE", "decimal(8,2)" },
      { "TOT_AMT_DUE_ADJ", "decimal(8,2)" },
      { "TOT_DISC_AMT", "decimal(8,2)" },
      { "TOT_MRC_CHGS", "decimal(8,2)" },
      { "TOT_NEW_CHG_ADJ", "decimal(8,2)" },
      { "TOT_NEW_CHGS", "decimal(8,2)" },
      { "TOT_OCC_CHGS", "decimal(8,2)" },
      { "TOT_TAXSUR", "decimal(8,2)" },
      { "TOT_USAGE_CHGS", "decimal(8,2)" },
      { "USG_BAND", "decimal(8,2)" },
      { "FilesProcessedID", "int" }

    };
    internal string _localDirectory = "U:\\Data";
    #region public properties
    public List<ACGFileInfo> FileList
    {
      get
      {
        if (_fileList == null)
          _fileList = new List<ACGFileInfo>();
        return _fileList;
      }
    }
    public List<string> FileNameList
    {
      get
      {
        return FileList.Select(f => f.Name).ToList();
      }
    }
    public string LocalFolder
    {
      get { return _localDirectory; }
      set { _localDirectory = value; }
    }


    #endregion
    public class ImportFileInfo
    {
      public string filepath { get; set; }
      public Dictionary<string, List<string>> Headers { get; set; } = new Dictionary<string, List<string>>();
      public Dictionary<string, List<List<object>>> Records { get; set; } = new Dictionary<string, List<List<object>>>();
    }
    public ImportFileProcessorBase()
    {
      _localDirectory = getAppSetting(APPSETTINGLOCALBASEFOLDER, null);
      CommonData.SERVERCONFIGFILENAME = CommonData.SERVERCONFIGFILEDEFAULT; // db.config for now
    }
    public void Dispose()
    {
    }
    public List<ACGFileInfo> getFilesToProcess(string fileType)
    {
      List<string> processedFiles = getFilesProcessed(fileType);
      List<ACGFileInfo> files = FileList.ToList();
      List<ACGFileInfo> filesToProcess = files.Where(ftp => !processedFiles.Contains(ftp.FullName)).ToList();
      return filesToProcess;
    }

    #region other protected methods
    internal object[] adjustFieldsForDataType(object[] fields, string[] columnNames)
    {
      // fixes up fields to be correct for data types
      object[] fldarray = fields.ToArray();
      string[] colarray = columnNames.ToArray();
      for (int i = 0; i < fldarray.Length; i++)
      {
        string col = colarray[i];
        string datatype = "string";
        if (_dataTypes.ContainsKey(col))
        {
          datatype = _dataTypes[col];
          if (datatype.StartsWith("decimal"))
            datatype = "decimal";
        }
        switch (datatype)
        {
          case "string":
            break;
          case "date":
            if (!CommonFunctions.IsDateTime(fldarray[i]))
              fldarray[i] = null;
            else
              fldarray[i] = CommonFunctions.CDateTime(fldarray[i]);
            break;
          case "decimal":
            if (!CommonFunctions.IsNumeric(fldarray[i]))
              fldarray[i] = null;
            else
              fldarray[i] = CommonFunctions.CDecimal(fldarray[i]);
            break;
          case "int":
            if (!CommonFunctions.IsInteger(fldarray[i]))
              fldarray[i] = null;
            else
              fldarray[i] = CommonFunctions.CInt(fldarray[i]);
            break;
        }
      }
      return fldarray;
    }
    internal void saveTableFromFileData(string tablename, List<string> headers, List<List<object>> records, Dictionary<string, string> datatypes, bool hasID)
    {
      using (DataAccess da = new DataAccess())
      {
        // all of these UniBill files have an ID so we set the last parm to true
        DataSet ds = da.getDatasetFromDictionaryData(tablename, headers, records, _dataTypes, true);
        if (ds != null && ds.Tables.Count > 0)
        {
          DataTable dt = ds.Tables[0];
          string fileProcessedID = dt.Rows[0]["FilesProcessedID"].ToString();
          if (!CommonFunctions.IsInteger(fileProcessedID))
            fileProcessedID = "-1";
          Exception ex = da.insertDataTabletoSQL(tablename, dt);
          if (ex != null)
          {
            // update the files processed record with the error
            string sql = "Update FilesProcessed SET ErrorMessage = 'Table:" + dt.TableName + " Error=" + ex.Message + "', StackTrace = '" + ex.StackTrace + "' WHERE ID = " + fileProcessedID;
            da.updateDataFromSQL(sql);
            throw ex;
          }
        }
      }

    }

    protected string getAppSetting(string name, string defaultValue = "")
    {
      string val = ConfigurationManager.AppSettings[name];
      if (string.IsNullOrWhiteSpace(val))
        val = defaultValue;
      return val;
    }
    protected List<string> getFilesProcessed(string filetype)
    {
      List<string> fileNames = new List<string>();
      using (DataAccess db = new DataAccess())
      {
        DataSet ds = db.getFilesProcessed(filetype);
        DataTable dt = ds.Tables[0];
        foreach (DataRow row in dt.Rows)
          fileNames.Add(row["FileName"].ToString());
        return fileNames;
      }
    }
    #endregion protected
  }
}
