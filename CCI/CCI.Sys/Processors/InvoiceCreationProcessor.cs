using ACG.Common;
using CCI.Sys.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OfficeOpenXml;
using static OfficeOpenXml.ExcelErrorValue;
using static System.Net.WebRequestMethods;

namespace CCI.Sys.Processors
{
  public class InvoiceCreationProcessor : ImportFileProcessorBase
  {
    private const string APPSETTINGCREATIOIMPORTFOLDER = "CreatioImportFolder";
    private const string FILETYPECREATIONETWORKINVENTORY = "CreatioNetworkInventory";
    private string _creatioImportFolder = "\\InvoiceIQ\\Creatio\\";
    public class CreatioNetworkInventoryFile : ImportFileInfo
    {
    }
    public InvoiceCreationProcessor() : base()
    {
      _creatioImportFolder = getAppSetting(APPSETTINGCREATIOIMPORTFOLDER, _creatioImportFolder);
      _localDirectory = _localDirectory + _creatioImportFolder;
    }
    public List<string> ImportCreatioNetworkInventory()
    {
      return ProcessFiles(FILETYPECREATIONETWORKINVENTORY);
    }
    public List<ACGFileInfo> GetFileList(string directory = null)
    {
      if (directory == null)
        directory = LocalFolder;
      if (_fileList == null)
        _fileList = new List<ACGFileInfo>();
      else
        _fileList.Clear();

      foreach (string filepath in Directory.GetFiles(directory))
      {
        _fileList.Add(new ACGFileInfo()
        {
          Name = Path.GetFileName(filepath),
          FullName = filepath,
          IsDirectory = false

        });
      }
      return _fileList;
    }
    public List<string> ListUnprocessedNetworkInventory()
    {
      List<string> filesToProcess = GetFileList(LocalFolder).Select(f => f.Name).ToList();
      return filesToProcess;
    }
    public List<string> ProcessFiles(string fileType)
    {
      List<ACGFileInfo> filesToProcess = getFilesToProcess(fileType);
      foreach (ACGFileInfo file in filesToProcess)
        ProcessFile(file, fileType);
      return filesToProcess.Select(f => f.Name).ToList();
    }
    private void ProcessFile(ACGFileInfo file, string fileType)
    {
      CreatioNetworkInventoryFile niFile = ImportNetworkInventory(file, fileType);
    }
    private CreatioNetworkInventoryFile ImportNetworkInventory(ACGFileInfo file, string fileTyp)
    {
      CreatioNetworkInventoryFile networkInventory = new CreatioNetworkInventoryFile();
      networkInventory.filepath = Path.Combine(LocalFolder, file.Name);
      int fileProcessedID = 0;
      using (DataAccess da = new DataAccess())
      {
        fileProcessedID = da.addFileProcessed(FILETYPECREATIONETWORKINVENTORY, networkInventory.filepath, DateTime.Now, -1, false, "Import Creatio Network Inventory");
      }
      // Read the text file line by line

      using (StreamReader sr = new StreamReader(networkInventory.filepath))
      {
        string line;
        line = sr.ReadLine();
        string[] headers = line.Split(',');
        string[] allheaders = new string[headers.Length + 1]; // add one for fileprocessedid
        Array.Copy(headers, 0, allheaders, 0, headers.Length);
        allheaders[headers.Length] = "FilesProcessedID";

        List<List<object>> theserecords = new List<List<object>>();
        // Load the data from the csv into headers and collections of fieldvalues
        while ((line = sr.ReadLine()) != null)
        {
          string[] values = ACG.Common.CommonFunctions.parseString(line);
          object[] fieldvalues = new string[values.Length + 1];
          Array.Copy(values, 0, fieldvalues, 0, values.Length);
          // take any values that cannot be converted to the correct datatype and make them null
          fieldvalues = adjustFieldsForDataType(fieldvalues, headers);
          int fileprocessedidsub = fieldvalues.Length - 1;

          fieldvalues[fileprocessedidsub] = fileProcessedID; // so use the file processed it to uniquely identify this batch
          List<object> fields = new List<object>();
          fields.AddRange(fieldvalues);
          theserecords.Add(fields); // now add the new record (which is itself a list of fields) to the collection for this record type
        }
        networkInventory.Records.Add("NI", theserecords);
        networkInventory.Headers.Add("NI", allheaders.ToList());
        return networkInventory;

      }
    }
    private void SaveNetworkInventoryFile(CreatioNetworkInventoryFile file)
    {
      {
        foreach (string rectype in file.Headers.Keys)
        {
          List<string> headers = file.Headers[rectype]; // now get the list of headers
          if (file.Records.ContainsKey(rectype)) // does the records collection have this record type?
          {
            // yes, so add the records to the db
            List<List<object>> records = file.Records[rectype]; // next get all the records with matching values
            string tablename = "[dbo].[CreatioNetworkInventory]"; // now get the table name
            saveTableFromFileData(tablename, headers, records, _dataTypes, true);
          }
          // else
          //     No: do nothing
        }
      }
    }
  }
}
