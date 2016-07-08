using CrystalDecisions.Shared;
using System.IO;
using System.Data.SqlClient;

namespace CCIWebClient.Reports
{
    public class QuoteReportHandler : ReportBase
    {
        public QuoteReportHandler(int QuoteId)
        {
            Report rep = new Report();
            this.ReportDocument = rep.getReporDocument("Quote.rpt");
            SqlConnectionStringBuilder cb = new SqlConnectionStringBuilder();

            //throw new Exception(cb.InitialCatalog);
           
            ParameterDiscreteValue quoteId = new ParameterDiscreteValue();
            quoteId.Value = QuoteId;
            /*CAC- for testing quoteId.Value = 108; */
            this.ReportDocument.SetParameterValue("@id", QuoteId);
            //this.ReportDocument.ParameterFields["@id"].CurrentValues.Add(quoteId);
        }

        public Stream QuoteReportStream(int Quoteid)
        {
            Stream stream = ReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);


            return stream;
        }
    }
}
