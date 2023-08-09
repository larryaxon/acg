using CCI.Sys.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ACG.Common;
using System.Diagnostics;
using System.Data;
using System.IO;

using OfficeOpenXml;
using System.Net.Http.Headers;

namespace CCI.WebApi.Controllers
{
  public class APIController : ApiController
  {
    const string FILESPROCESSEDFILETYPEINVOICEIQ = CCI.Common.CommonData.FILESPROCESSEDFILETYPEINVOICEIQ;
    const string FILESPROCESSEDFILETYPEUNIBILL = CCI.Common.CommonData.FILESPROCESSEDFILETYPEUNIBILL;
    [Route("api/invoiceiq/processfiles")]
    [HttpGet]
    public List<string> ProcessInvoiceIQFiles()
    {
      using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
      {
        List<string> filesProcessed = processor.ProcessFiles(FILESPROCESSEDFILETYPEINVOICEIQ);
        return filesProcessed;
      }
    }
    [Route("api/invoiceiq/listfiles")]
    [HttpGet]
    public List<string> ListFiles()
    {
      using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
      {
        List<string> filesProcessed = processor.getFilesToProcess(FILESPROCESSEDFILETYPEINVOICEIQ).Select(f => f.FullName).ToList();
        return filesProcessed;
      }
    }
    [Route("api/invoiceiq/init")]
    [HttpGet]
    public string InitialLoad()
    {
      using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor())
      {
        processor.InitalFilesProcessedLoad(FILESPROCESSEDFILETYPEINVOICEIQ);
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
    [HttpGet]
    [Route("api/invoiceiq/invoice/excel")]
    public HttpResponseMessage GetInvoice(int id)
    {

        using (InvoiceFtpProcessor processor = new InvoiceFtpProcessor()) // this is a test, so just grab any existing dataset formatted export
        {
          MemoryStream stream = processor.GetInvoice(id);
          return ToStream(stream, "Invoice" + id.ToString() + ".xlsx");
        }
      
    }
    [HttpGet]
    [Route("api/invoiceiq/uploadpath")]
    public string GetUploadPath()
    {
      using (InvoiceCreationProcessor processor = new InvoiceCreationProcessor())
      {
        return processor.LocalFolder;
      }
    }
    private HttpResponseMessage ToStream(MemoryStream stream, string filename)
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

  }
}