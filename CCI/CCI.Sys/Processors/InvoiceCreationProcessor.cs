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




    public InvoiceCreationProcessor() : base()
    {
      _creatioImportFolder = getAppSetting(APPSETTINGCREATIOIMPORTFOLDER, _creatioImportFolder);
      _importFileSpecs = new List<ImportFileSpecs>()
      {
        new ImportFileSpecs()
        {
          FileType = "NI",
          TableName = "[dbo].[CreatioNetworkInventory]",
          HeaderLine = "Number,Carrier,Service Location,Parent Account No.,Child Account No.,Product Nickname,MRC ($),Stage,Status",
          RepaceAllRecords = true,
          IsActive = true
        },
        new ImportFileSpecs()
        {
          FileType = "CreatioAudit",
          TableName = "[dbo].[CreatioAudit]",
          HeaderLine = "",
          RepaceAllRecords = false,
          IsActive = false
        }
      };
      _localDirectory = _localDirectory + _creatioImportFolder;
    }
    public List<string> ImportCreatioNetworkInventory()
    {
      List<ACGFileInfo> files = GetFileList();
      return ProcessFiles();
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
    public List<string> ListUnprocessedImportFiles()
    {
      GetFileList(LocalFolder);
      List<ACGFileInfo> filesToProcess = getFilesToProcess();
      List<string> filelist = filesToProcess.Select(f => f.Name).ToList();
      return filelist;
    }
    public List<string> ProcessFiles()
    {
      GetFileList(LocalFolder);
      List<ACGFileInfo> filesToProcess = getFilesToProcess();
      foreach (ACGFileInfo file in filesToProcess)
        ProcessFile(file);
      return filesToProcess.Select(f => f.Name).ToList();
    }
    private void ProcessFile(ACGFileInfo file)
    {
      string fileType;
      ImportFileInfo niFile = ImportFile(file, out fileType);
      ImportFileSpecs spec = _importFileSpecs.Where(s => s.FileType == fileType).FirstOrDefault();
      SaveImportFile(niFile, spec.TableName, spec.RepaceAllRecords);
    }
  }
}
