using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ACG.Common.Data;
using ACG.CommonForms;
using CCI.Common;
using CCI.DesktopClient.Common;
using CCI.Sys.Data;

namespace CCI.DesktopClient.Screens
{
  public partial class dlgGetCashReceiptsParameters : Form
  {
    ACG.CommonForms.ctlSearchGrid _grid;
    public dlgGetCashReceiptsParameters(ACG.CommonForms.ctlSearchGrid grid)
    {
      _grid = grid;
      InitializeComponent();
      dtStartDate.Value = (new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(-1).Month, 1));;    
      dtEndDate.Value = DateTime.Today;
      dtBillDate.Value = dtStartDate.Value.AddMonths(1);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      _grid.Parameters.Clear();
      _grid.Parameters.Add("FromDate", dtStartDate.Value.ToShortDateString());
      _grid.Parameters.Add("ToDate", dtEndDate.Value.ToShortDateString());
      _grid.Parameters.Add("BillDate", dtBillDate.Value.ToShortDateString());
      this.Close();
    }
  }
}
