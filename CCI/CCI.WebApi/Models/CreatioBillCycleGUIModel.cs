using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCI.WebApi.Models
{
  public class CreatioBillCycleGUIModel
  {
    public DateTime BillCycleDate { get; set; }
    public string BillCycleDateDisplay
    {
      get { return BillCycleDate.ToShortDateString();  }
      set { BillCycleDate = DateTime.Parse(value); }
    }
    public DateTime? ZipUploaded {  get; set; }
    public DateTime? AuditDownloaded { get; set; }
    public DateTime? EditedFileUploaded { get; set; }
    public DateTime? DownloadCreatioImport { get; set; }
    public DateTime? UploadNewCreatioFile { get; set; }
  }
}
