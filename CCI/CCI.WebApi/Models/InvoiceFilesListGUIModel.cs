using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CCI.WebApi.Models
{
  public class InvoiceFilesListGUIModel
  {
    [DisplayName("File")]
    public string FileName { get; set; }
  }
}