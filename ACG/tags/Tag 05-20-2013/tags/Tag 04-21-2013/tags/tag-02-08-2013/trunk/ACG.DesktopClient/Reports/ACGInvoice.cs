using CrystalDecisions.Shared;
using System;
using System.IO;

namespace ACG.DesktopClient.Reports
{
  class ACGInvoice : ReportBase
  {
    public string Customer { set { setParameter("Customer", value); } }
    public string Project { set { setParameter("Project", value); } }
    public string Resource { set { setParameter("Resource", value); } }
    public DateTime ThroughDate { set { setParameter("ThroughDate", value); } }
    public bool IncludeUnposted { set { setParameter("IncludeUnposted", value); } }
    public DateTime FromDate { set { setParameter("FromDate", value); } }
    public ACGInvoice()
    {
      Report rep = new Report();
      this.ReportDocument = rep.getReportDocument("rptACGInvoice.rpt");
    }
    public void PrintToFile(string filename)
    {
      this.ReportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, filename);
    }
    //public Stream QuoteReportStream(int Quoteid)
    //{
    //  Stream stream = ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
    //  return stream;
    //}
    public void setParameter(string name, object value)
    {
      ParameterDiscreteValue parm = new ParameterDiscreteValue();
      parm.Value = value;
      string parmName = "@" + name;
      if (ReportDocument.ParameterFields[parmName].HasCurrentValue)
        this.ReportDocument.ParameterFields[parmName].CurrentValues.Clear();
      this.ReportDocument.ParameterFields[parmName].CurrentValues.Add(parm);
    }
  }
}
