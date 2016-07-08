using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using CCI.Sys.Data;
using CCI.DesktopClient.Common;

namespace CCI.DesktopClient.Screens
{
  public partial class frmCityHostedLedgerInvoiceMatching : frmCityHostedMatchingBase
  {

    public frmCityHostedLedgerInvoiceMatching() : base(CommonData.UnmatchedNameTypes.CityHostedLedgerInvoiceMatch)
    {
      //NameType = CommonData.UnmatchedNameTypes.CityHostedLedgerMatch;
      this.Name = "frmCityHostedLedgerInvoiceMatching";
      this.Text = "Saddleback Ledger Invoice Matching";
      ExceptionFollowupMessage = "Ledger Invoice Matching";
      cboSource.Items.Clear();
      cboSource.Items.AddRange(new object[] {
            "Matched",
            "InvoiceMismatch",
            "All"});
    }


  }
}
