using ACG.Common;
using CCI.Sys.Processors;
using CCI.WebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using CCI.Common;
using Newtonsoft.Json;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Reflection.Emit;

namespace CCI.WebApi.Controllers
{
  public class HomeController : Controller
  {
    #region constants
    const string FILESPROCESSEDFILETYPEINVOICEIQ = InvoiceCreationProcessor.FILESPROCESSEDFILETYPEINVOICEIQ;
    const string FILESPROCESSEDFILETYPEUNIBILL = InvoiceCreationProcessor.FILESPROCESSEDFILETYPEUNIBILL;
    const string PROCESSSTEPZIP = "ZipUpload";
    const string PROCESSSTEPDOWNLOADAUDIT = "DownloadAudit";
    const string PROCESSSTEPUPLOADEDITED = "UploadEditedAudit";
    const string PROCESSSTEPDOWNLOADCREATIOIMPORT = "DownloadCreatioImport";
    const string PROCESSSTEPUPLOADEXPORT = "UploadCreatioExport";
    const string USER = "Melissa";
    #endregion
    public HomeController() { }
    public ActionResult Index()
    {
      ViewBag.Title = "Carvana Audit Process Cycle";
      DateTime billCycleDate = InvoiceCreationProcessor.CalculateBillCycleDate(DateTime.Today.AddMonths(-1));
      CreatioBillCycleGUIModel model = new CreatioBillCycleGUIModel()
      {
        BillCycleDate = billCycleDate
      };
      using (InvoiceCreationProcessor processor = new InvoiceCreationProcessor())
      {
        List<BillCycleModel> steps = processor.getThisBillCycle(billCycleDate);
        foreach (BillCycleModel step in steps)
        {
          switch (step.Step)
          {
            case PROCESSSTEPZIP:
              model.ZipUploaded = step.ProcessedDateTime;
              break;
            case PROCESSSTEPDOWNLOADAUDIT:
              model.AuditDownloaded = step.ProcessedDateTime;
              break;
            case PROCESSSTEPUPLOADEDITED:
              model.EditedFileUploaded = step.ProcessedDateTime;
              break;
            case PROCESSSTEPDOWNLOADCREATIOIMPORT:
              model.DownloadCreatioImport = step.ProcessedDateTime;
              break;
            case PROCESSSTEPUPLOADEXPORT:
              model.UploadNewCreatioFile = step.ProcessedDateTime;
              break;

          }
        }
      }
      return View(model);
    }
    #region zip files
    public ActionResult ImportAndProcessZipFiles(DateTime billCycleDate)
    {
      List<InvoiceFilesListGUIModel> filelist = processZipFiles(); // process zip files and get the return file list
      filelist.AddRange(importUnibillFiles()); // now import the edi files and add them to the list
      InvoiceCreationProcessor.processStep(PROCESSSTEPZIP, billCycleDate, true, USER);
      ViewBag.FileListTitle = "Files Downloaded and Processed";
      return View("FileList", filelist);
    }
    public ActionResult ProcessZipFiles()
    {
      List<InvoiceFilesListGUIModel> model = processZipFiles();
      ViewBag.FileListTitle = "Files Downloaded and Processed";
      return View("FileList", model); 
    }
    private List<InvoiceFilesListGUIModel> processZipFiles()
    {
      using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
      {
        List<string> filesProcessed = processor.ProcessFiles(FILESPROCESSEDFILETYPEINVOICEIQ);
        List<InvoiceFilesListGUIModel> model = new List<InvoiceFilesListGUIModel>();
        foreach (string file in filesProcessed)
        {
          model.Add(new InvoiceFilesListGUIModel() { FileName = file });
        }
        return model;
      }
    }
    public ActionResult UnprocessedZipFiles()
    {
      using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
      {
        List<ACGFileInfo> filesProcessed = processor.getFilesToProcess(FILESPROCESSEDFILETYPEINVOICEIQ);
        List<InvoiceFilesListGUIModel> model = new List<InvoiceFilesListGUIModel>();
        foreach (ACGFtpFileInfo file in filesProcessed)
        {
          model.Add(new InvoiceFilesListGUIModel() { FileName = file.Name }) ;
        }
        ViewBag.FileListTitle = "Files Not Yet Downloaded";
        return View("FileList", model);
      }
    }
    #endregion
    #region unibill files
    public ActionResult UnprocessedUnibillFiles()
    {
      using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
      {
        List<string> filesProcessed = processor.ListUnprocessedUnibills();
        List<InvoiceFilesListGUIModel> model = new List<InvoiceFilesListGUIModel>();
        foreach (string file in filesProcessed)
        {
          model.Add(new InvoiceFilesListGUIModel() { FileName = file });
        }
        ViewBag.FileListTitle = "Carvana Files Not Yet Imported";
        return View("FileList", model);
      }
    }
    public ActionResult ImportUnibillFiles()
    {
      List<InvoiceFilesListGUIModel> model = importUnibillFiles();
      ViewBag.FileListTitle = "Carvana Files Imported";
      return View("FileList", model);

    }
    private List<InvoiceFilesListGUIModel> importUnibillFiles()
    {
      using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
      {
        List<string> filesProcessed = processor.ImportUnibills();
        List<InvoiceFilesListGUIModel> model = new List<InvoiceFilesListGUIModel>();
        foreach (string file in filesProcessed)
        {
          model.Add(new InvoiceFilesListGUIModel() { FileName = file });
        }
        return model;
      }
    }
    #endregion
    #region generic routines
    public ActionResult FileList(List<InvoiceFilesListGUIModel> model)
    {
      return View(model);
    }
    #endregion
    #region Create Invoice
    public ActionResult UnprocessedCreatioFiles()
    {
      using (InvoiceCreationProcessor processor = new InvoiceCreationProcessor())
      {
        List<string> filesProcessed = processor.ListUnprocessedImportFiles();
        List<InvoiceFilesListGUIModel> model = new List<InvoiceFilesListGUIModel>();
        foreach (string file in filesProcessed)
        {
          model.Add(new InvoiceFilesListGUIModel() { FileName = file });
        }
        ViewBag.FileListTitle = "Creatio Not Yet Imported";
        return View("FileList", model);
      }
    }
    public ActionResult ImportCreatioFiles(DateTime billCycleDate, string fileType = null)
    {
      using (InvoiceCreationProcessor processor = new InvoiceCreationProcessor())
      {
        List<string> filesProcessed = processor.ImportCreatioFiles(fileType);
        List<InvoiceFilesListGUIModel> model = new List<InvoiceFilesListGUIModel>();
        foreach (string file in filesProcessed)
        {
          model.Add(new InvoiceFilesListGUIModel() { FileName = file });
        }
        InvoiceCreationProcessor.processStep(PROCESSSTEPUPLOADEDITED, billCycleDate, true, USER);

        ViewBag.FileListTitle = "Creatio Network Inventory Files Imported";
        return View("FileList", model);
      }
    }


    public FileStreamResult DownloadCreatioInvoice(DateTime billCycleDate)
    {
      DateTime fromDate = GetPeriodBeginDate(billCycleDate);
      DateTime toDate = GetPeriodEndDate(billCycleDate);

      using (InvoiceCreationProcessor processor = new InvoiceCreationProcessor())
      {
        string filename = "Carvana Audit " + DateTime.Today.ToShortDateString() + ".xlsx";
        MemoryStream stream = processor.GetCreatioInvoice(fromDate, toDate);
        FileStreamResult result = ToExcel(stream, filename);
        InvoiceCreationProcessor.processStep(PROCESSSTEPDOWNLOADAUDIT, billCycleDate, true, USER);

        return result;
      }
    }
    public FileStreamResult DownloadCreatioExport(DateTime billCycleDate)
    {
      using (InvoiceCreationProcessor processor = new InvoiceCreationProcessor())
      {
        string filename = "Carvana Import File " + DateTime.Today.ToShortDateString() + ".xlsx";
        MemoryStream stream = processor.getCreationAuditExport(billCycleDate);
        FileStreamResult result = ToExcel(stream, filename);
        InvoiceCreationProcessor.processStep(PROCESSSTEPDOWNLOADCREATIOIMPORT, billCycleDate, true, USER);

        return result;
      }
    }
    #endregion
    #region upload files

    public ActionResult FileUpload(DateTime billCycleDate, string fileType = null)
    {
      ViewBag.BillCycleDate = billCycleDate;
      ViewBag.FileType = fileType;
      return View();
    }
    [HttpPost]
    public ActionResult UploadFiles(DateTime billCycleDate, string fileType = null)
    {
      string uploadFolder = InvoiceCreationProcessor.LocalFolder;
      // Checking no of files injected in Request object  
      if (Request.Files.Count > 0)
      {
        try
        {
          //  Get all files from Request object  
          HttpFileCollectionBase files = Request.Files;
          for (int i = 0; i < files.Count; i++)
          {
            //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
            //string filename = Path.GetFileName(Request.Files[i].FileName);  

            HttpPostedFileBase file = files[i];
            string fname;

            // Checking for Internet Explorer  
            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
            {
              string[] testfiles = file.FileName.Split(new char[] { '\\' });
              fname = testfiles[testfiles.Length - 1];
            }
            else
            {
              fname = file.FileName;
            }

            // Get the complete folder path and store the file inside it.  
            //fname = Path.Combine(Server.MapPath(uploadFolder), fname);
            fname = Path.Combine(uploadFolder, fname);
            file.SaveAs(fname);
          }
          ImportCreatioFiles(billCycleDate, fileType);

          // Returns message that successfully uploaded  
          return Json("File Uploaded Successfully!");
        }
        catch (Exception ex)
        {
          return Json("Error occurred. Error details: " + ex.Message);
        }
      }
      else
      {
        ImportCreatioFiles(billCycleDate, fileType);

        return Json("No files selected.");
      }
    }
    //[HttpPost]
    //public ActionResult UploadFiles(DateTime billCycleDate)
    //{
    //  // Checking no of files injected in Request object  
    //  if (Request.Files.Count > 0)
    //  {
    //    try
    //    {
    //      string localfolder = InvoiceCreationProcessor.LocalFolder;
    //      //  Get all files from Request object  
    //      HttpFileCollectionBase files = Request.Files;
    //      for (int i = 0; i < files.Count; i++)
    //      {
    //        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
    //        //string filename = Path.GetFileName(Request.Files[i].FileName);  

    //        HttpPostedFileBase file = files[i];
    //        string fname;

    //        // Checking for Internet Explorer  
    //        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
    //        {
    //          string[] testfiles = file.FileName.Split(new char[] { '\\' });
    //          fname = testfiles[testfiles.Length - 1];
    //        }
    //        else
    //        {
    //          fname = file.FileName;
    //        }

    //        // Get the complete folder path and store the file inside it.  
    //        //fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
    //        fname = Path.Combine(localfolder, fname);
    //        file.SaveAs(fname);
    //      }
    //      ImportCreatioFiles(billCycleDate);
    //      // Returns message that successfully uploaded  
    //      return Json("File Uploaded Successfully!");
    //    }
    //    catch (Exception ex)
    //    {
    //      return Json("Error occurred. Error details: " + ex.Message);
    //    }
    //  }
    //  else
    //  {
    //    return Json("No files selected.");
    //  }
    //}
    #endregion
    #region Process Steps
    public string getProcessSteps()
    {
      using (InvoiceCreationProcessor processor = new InvoiceCreationProcessor())
      {
        List<ProcessStepsModel> list =  processor.getProcessStepsList();
        return JsonConvert.SerializeObject(list, Formatting.Indented);
      }
    }
    public string getThisBillCycle(DateTime billCycleDate)
    {
      using (InvoiceCreationProcessor processor = new InvoiceCreationProcessor())
      {
        List<BillCycleModel> list = processor.getThisBillCycle(billCycleDate);
        return JsonConvert.SerializeObject(list, Formatting.Indented);
      }
    }
    #endregion
    #region private methods
    private string ConvertListToString(List<string> list)
    {
      string txt = string.Empty;
      foreach (string item in list)
      {
        txt += item + "\r\n";
      }
      return txt;
    }
    private HttpResponseMessage ToResponse(MemoryStream stream, string filename)
    {
      var result = new HttpResponseMessage(HttpStatusCode.OK)
      {
        Content = new ByteArrayContent(stream.ToArray())
      };
      result.Content.Headers.ContentDisposition =
          new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
          {
            FileName = filename
          };
      result.Content.Headers.ContentType =
          new MediaTypeHeaderValue("application/octet-stream");

      return result;
    }
    private FileStreamResult ToExcel(MemoryStream stream, string filename = null)
    {
      if (filename == null)
        filename = "ExcelReport" + "-" + DateTime.Today.ToString() + ".xlsx";
      stream.Seek(0, 0);
      FileStreamResult result = new FileStreamResult(stream, "application/vnd.ms-excel") { FileDownloadName = filename }; // and return the text file
      return result;
    }
    private DateTime GetPeriodBeginDate(DateTime billCycleDate)
    {
      const int beginday = 16;
      int month = billCycleDate.Month - 1;
      int year = billCycleDate.Year;
      if (month < 0)
      {
        month = 12;
        year--;
      }
      return new DateTime(year, month, beginday);
    }
    private DateTime GetPeriodEndDate(DateTime billCycleDate)
    {
      const int endday = 15;
      int month = billCycleDate.Month;
      int year = billCycleDate.Year;
      return new DateTime(year, month, endday);
    }
    #endregion
  }
}
