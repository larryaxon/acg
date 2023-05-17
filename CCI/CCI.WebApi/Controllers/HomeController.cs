﻿using ACG.Common;
using CCI.Sys.Processors;
using CCI.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CCI.WebApi.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      ViewBag.Title = "Invoice IQ Downloads";

      return View();
    }
    public ActionResult ProcessFiles()
    {
      using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
      {
        List<string> filesProcessed = processor.ProcessFiles();
        List<InvoiceFilesListGUIModel> model = new List<InvoiceFilesListGUIModel>();
        foreach (string file in filesProcessed)
        {
          model.Add(new InvoiceFilesListGUIModel() { FileName = file });
        }
        ViewBag.FileListTitle = "Files Downloaded and Processed";
        return View("FileList", model); 
      }
    }
    public ActionResult FileList(List<InvoiceFilesListGUIModel> model)
    {
      return View(model);
    }
    public ActionResult UnprocessedFiles()
    {
      using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
      {
        List<ACGFtpFileInfo> filesProcessed = processor.getFilesToProcess();
        List<InvoiceFilesListGUIModel> model = new List<InvoiceFilesListGUIModel>();
        foreach (ACGFtpFileInfo file in filesProcessed)
        {
          model.Add(new InvoiceFilesListGUIModel() { FileName = file.Name }) ;
        }
        ViewBag.FileListTitle = "Files Not Yet Downloaded";
        return View("FileList", model);
      }
    }
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
