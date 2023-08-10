using ACG.Common;
using CCI.Sys.Processors;
using CCI.WebApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CCI.WebApi.Controllers
{
  public class HomeController : Controller
  {
    const string FILESPROCESSEDFILETYPEINVOICEIQ = CCI.Common.CommonData.FILESPROCESSEDFILETYPEINVOICEIQ;
    const string FILESPROCESSEDFILETYPEUNIBILL = CCI.Common.CommonData.FILESPROCESSEDFILETYPEUNIBILL;
    public ActionResult Index()
    {
      ViewBag.Title = "Invoice IQ Downloads";

      return View();
    }
    #region zip files
    public ActionResult ProcessZipFiles()
    {
      using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
      {
        List<string> filesProcessed = processor.ProcessFiles(FILESPROCESSEDFILETYPEINVOICEIQ);
        List<InvoiceFilesListGUIModel> model = new List<InvoiceFilesListGUIModel>();
        foreach (string file in filesProcessed)
        {
          model.Add(new InvoiceFilesListGUIModel() { FileName = file });
        }
        ViewBag.FileListTitle = "Files Downloaded and Processed";
        return View("FileList", model); 
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
      using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
      {
        List<string> filesProcessed = processor.ImportUnibills(10);
        List<InvoiceFilesListGUIModel> model = new List<InvoiceFilesListGUIModel>();
        foreach (string file in filesProcessed)
        {
          model.Add(new InvoiceFilesListGUIModel() { FileName = file });
        }
        ViewBag.FileListTitle = "Carvana Files Imported";
        return View("FileList", model);
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
    public ActionResult ImportCreatioFiles()
    {
      using (InvoiceCreationProcessor processor = new InvoiceCreationProcessor())
      {
        List<string> filesProcessed = processor.ImportCreatioNetworkInventory();
        List<InvoiceFilesListGUIModel> model = new List<InvoiceFilesListGUIModel>();
        foreach (string file in filesProcessed)
        {
          model.Add(new InvoiceFilesListGUIModel() { FileName = file });
        }
        ViewBag.FileListTitle = "Creatio Network Inventory Files Imported";
        return View("FileList", model);
      }
    }

    public ActionResult FileUpload()
    {
      return View();
    }
    [HttpPost]
    public ActionResult UploadFiles()
    {
      // Checking no of files injected in Request object  
      if (Request.Files.Count > 0)
      {
        try
        {
          string localfolder;
          using (InvoiceCreationProcessor processor = new InvoiceCreationProcessor())
          {
            localfolder = processor.LocalFolder;
          }
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
            //fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
            fname = Path.Combine(localfolder, fname);
            file.SaveAs(fname);
          }
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
        return Json("No files selected.");
      }
    }
    #endregion
    private string ConvertListToString(List<string> list)
    {
      string txt = string.Empty;
      foreach (string item in list)
      {
        txt += item + "\r\n";
      }
      return txt;
    }
  }
}
