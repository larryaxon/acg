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
  public partial class frmCityHostedLedgerCashMatching : frmCityHostedMatchingBase
  {
    public frmCityHostedLedgerCashMatching() : base(CommonData.UnmatchedNameTypes.CityHostedLedgerCashMatch)
    {
      this.Name = "frmCityHostedLedgerCashMatching";
      this.Text = "Saddleback Ledger Cash Matching";
      ExceptionFollowupMessage = "Ledger Cash Matching";
      cboSource.Items.Clear();
      cboSource.Items.AddRange(new object[] {
            "Matched",
            "MissingAR",
            "MissingLedger",
            "All"});
    }
  }
}
