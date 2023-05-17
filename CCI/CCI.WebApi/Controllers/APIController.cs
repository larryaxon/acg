using CCI.Sys.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using ACG.Common;

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

  }
}