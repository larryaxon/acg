using System;
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
  public partial class frmMissingSaddlebackCustomerInfo : ScreenBase
  {
    private const string _title = "Saddleback Customers with Missing Info";
    public frmMissingSaddlebackCustomerInfo()
    {
      InitializeComponent();
      this.Text = _title;
      srchCustomers.Init(CommonData.UnmatchedNameTypes.CityHostedCustomersWithMissingInfo, "Missing Info");

    }

    private void frmMissingSaddlebackCustomerInfo_Load(object sender, EventArgs e)
    {
      srchCustomers.ReLoad();     

    }

    public void srchCustomers_RowSelected(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      DataGridViewRow row = e.SelectedRow;
      string customerid = CommonFunctions.CString(row.Cells["CustomerID"].Value);
      //MainForm parent = CommonFormFunctions.getMainForm(this);
      this.Text = _title + " Loading.... ";
      ScreenBase frm = new frmCustomers();
      ((frmCustomers)frm).EntityType = "Customer";
      ((frmCustomers)frm).SecurityContext = (CCI.Common.SecurityContext)SecurityContext;
      //((frmCustomers)frm).RefreshAll = true;
      this.Cursor = Cursors.WaitCursor;
      ((frmCustomers)frm).Init(customerid);
      this.Cursor = Cursors.Default;
      frm.ShowDialog();
      this.Text = _title;
      srchCustomers.ReLoad();     
      //parent.ShowForm(frm, false);
    }

    private void frmMissingSaddlebackCustomerInfo_Enter(object sender, EventArgs e)
    {


    }
  }
}
