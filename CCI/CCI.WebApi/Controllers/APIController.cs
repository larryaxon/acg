using CCI.Sys.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ACG.Common;
using System.Diagnostics;

namespace CCI.WebApi.Controllers
{
  public class APIController : ApiController
  {

    [Route("api/invoiceiq/processfiles")]
    [HttpGet]
    public List<string> ProcessInvoiceIQFiles()
    {
      using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
      {
        List<string> filesProcessed = processor.ProcessFiles();
        return filesProcessed;
      }
    }
    [Route("api/invoiceiq/listfiles")]
    [HttpGet]
    public List<string> ListFiles()
    {
      using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
      {
        List<string> filesProcessed = processor.getFilesToProcess().Select(f => f.FullName).ToList();
        return filesProcessed;
      }
    }
    [Route("api/invoiceiq/init")]
    [HttpGet]
    public string InitialLoad()
    {
      using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
      {
        processor.InitalFilesProcessedLoad();
        return "Complete";
      }
    }
    [HttpGet]
    [Route("api/invoiceiq/unibill/import")]
    public string ImportUnibills(int maxFilesToProcess = -1)
    {
      using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
      {
        try
        {
          processor.ImportUnibills(maxFilesToProcess);
        }
        catch (Exception ex)
        {
          return ex.Message + " Stack Trace = " + ex.StackTrace;
        }
        return "Success";
      }
    }
    [HttpGet]
    [Route("api/invoiceiq/unibill/list")]
    public List<string> ListUnprocessedUnibills()
    {
      using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
      {
        try
        {
          List<string> unimportedunibils = processor.ListUnprocessedUnibills();
          return unimportedunibils;
        }
        catch (Exception ex)
        {
          return new List<string>() { ex.Message + " Stack Trace = " + ex.StackTrace };
        }
      }
    }
  }
}