using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCI.Common;
using System.Globalization;

namespace CCIWebClient.Models
{
  //public class QuoteTotals
  //{
  //    public string QuoteId { get; set; }
  //    public string CustomerName { get; set; }
  //    public string MonthlyPay { get; set; }
  //    public string OneTimePay { get; set; }
  //    public string DividedProfit { get; set; }
  //    public string TotalProfit { get; set; }
  //    public string HostedServicesMRC { get; set; }
  //    public string HostedServicesNRC { get; set; }
  //    public string EquipmentMRC { get; set; }
  //    public string EquipmentNRC { get; set; }
  //    public string CarrierServicesMRC { get; set; }
  //    public string CarrierServicesNRC { get; set; }
  //    //public Hashtable HostedServices { get; set; }
  //    //public Hashtable Equipment { get; set; }
  //    //public Hashtable CarrierSerives { get; set; }

  //}
  public class DealerSummary
  {
    public string ScreenSection { get; set; }
    public string TotalMonthly { get; set; }
    public string TotalOneTime { get; set; }
    public string ProfitMonthly { get; set; }
    public string ProfitOneTime { get; set; }
    public string DealerContribution { get; set; }
  }

  public class QuoteTotalsModel
  {
    public string QuoteId { get; set; }
    public string ShortName { get; set; }
    public string TotalMonthly { get; set; }
    public string TotalOneTime { get; set; }
    public string ProfitMonthly { get; set; }
    public string ProfitOneTime { get; set; }
    public string TotalDealerMonthly { get; set; }
    public string TotalDealerOneTime { get; set; }
    public string DealerContribution { get; set; }
    public List<DealerSummary> DealerSummary { get; set; }


    public QuoteTotalsModel()
    {
      this.DealerSummary = new List<DealerSummary>();
    }

    public QuoteTotalsModel(int securityid, string quoteId, string shortName)
    {
      QuoteId = quoteId;
      ShortName = shortName;
      this.DealerSummary = new List<DealerSummary>();
      if (String.IsNullOrEmpty(quoteId) || quoteId == "0")
      {
        this.TotalMonthly = 0.ToString("C2", CultureInfo.CurrentCulture);
        this.TotalOneTime = 0.ToString("C2", CultureInfo.CurrentCulture);
        this.ProfitMonthly = 0.ToString("C2", CultureInfo.CurrentCulture);
        this.ProfitOneTime = 0.ToString("C2", CultureInfo.CurrentCulture);
        this.DealerContribution = 0.ToString("C2", CultureInfo.CurrentCulture);
        this.QuoteId = null;
        this.ShortName = null;
        return;
      }
      ServerResponse response = Proxy.getOrderTotal(securityid, quoteId);
      if (response.Errors.Count > 0)
        throw new Exception(response.Errors[0].ToString());

      //CCITable subtotal = response.Results[0] as CCITable;
      if (response.Results.Count > 0)
      {
        CCITable grandTotal = response.Results[0] as CCITable;
        if (grandTotal.NumberColumns > 0)
        {
          this.TotalMonthly = CommonFunctions.CDecimal(grandTotal[0, "totalmonthly"]).ToString("C2", CultureInfo.CurrentCulture);
          this.TotalOneTime = CommonFunctions.CDecimal(grandTotal[0, "totalonetime"]).ToString("C2", CultureInfo.CurrentCulture);
          this.ProfitMonthly = CommonFunctions.CDecimal(grandTotal[0, "profitmonthly"]).ToString("C2", CultureInfo.CurrentCulture);
          this.ProfitOneTime = CommonFunctions.CDecimal(grandTotal[0, "profitonetime"]).ToString("C2", CultureInfo.CurrentCulture);
          this.TotalDealerMonthly = CommonFunctions.CDecimal(grandTotal[0, "totaldealermonthly"]).ToString("C2", CultureInfo.CurrentCulture);
          this.TotalDealerOneTime = CommonFunctions.CDecimal(grandTotal[0, "totaldealeronetime"]).ToString("C2", CultureInfo.CurrentCulture);
          this.DealerContribution = CommonFunctions.CDecimal(grandTotal[0, "dealercontribution"]).ToString("C2", CultureInfo.CurrentCulture);
        }
        else {
          this.TotalMonthly = "0.00";
          this.TotalOneTime = "0.00";
          this.ProfitMonthly = "0.00";
          this.ProfitOneTime = "0.00";
          this.TotalDealerMonthly = "0.00";
          this.TotalDealerOneTime = "0.00";
          this.DealerContribution = "0.00";
        }
      }
      else 
      {
        this.TotalMonthly = "0.00";
        this.TotalOneTime = "0.00";
        this.ProfitMonthly = "0.00";
        this.ProfitOneTime = "0.00";
        this.TotalDealerMonthly = "0.00";
        this.TotalDealerOneTime = "0.00";
        this.DealerContribution = "0.00";
      }
      //for (int i = 0; i < subtotal.NumberRows; i++) 
      //{
      //    this.DealerSummary.Add(new DealerSummary() {
      //        ScreenSection = CommonFunctions.CString(subtotal[i, "screensection"]),
      //        ProfitMonthly = CommonFunctions.CDecimal(subtotal[i, "profitmonthly"]).ToString("C2", CultureInfo.CurrentCulture),
      //        ProfitOneTime = CommonFunctions.CDecimal(subtotal[i, "profitonetime"]).ToString("C2", CultureInfo.CurrentCulture),
      //        TotalMonthly = CommonFunctions.CDecimal(subtotal[i, "totalmonthly"]).ToString("C2", CultureInfo.CurrentCulture),
      //        TotalOneTime = CommonFunctions.CDecimal(subtotal[i, "totalonetime"]).ToString("C2", CultureInfo.CurrentCulture)
      //    });
      //}
    }

  }
}

