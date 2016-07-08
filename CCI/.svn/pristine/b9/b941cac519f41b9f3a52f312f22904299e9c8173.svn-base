using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CCI.Sys.Data;
using CCI.Common;

namespace CCI.DesktopClient.Screens
{
  public partial class dlgMergeCustomer : ScreenBase
  {
    private string _fromCustomer = null;
    private string _toCustomer = null;
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    public string FromCustomer { get { return _fromCustomer; } set { _fromCustomer = value; srchFromCustomer.Text = _fromCustomer; } }
    public string ToCustomer { get { return _toCustomer; } set { _toCustomer = value; srchToCustomer.Text = _toCustomer; } }
    public dlgMergeCustomer()
    {
      InitializeComponent();
      srchFromCustomer.SearchExec = new SearchDataSourceEntity("Customer");
      srchToCustomer.SearchExec = new SearchDataSourceEntity("Customer");
      btnMerge.Enabled = false;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnMerge_Click(object sender, EventArgs e)
    {
      DialogResult res = MessageBox.Show(
        string.Format("Warning!! Merging one customer into another is not reversible. Are you sure you want to merge customer {0} into customer {1}?", srchFromCustomer.Description, srchToCustomer.Description), 
        "Merge Customer", 
        MessageBoxButtons.YesNo);
      if (res == System.Windows.Forms.DialogResult.Yes)
      {
        _dataSource.MergeCustomer(_fromCustomer, _toCustomer);
        MessageBox.Show("Customers have been merged");
        this.Close();
      }

    }

    private void dlgMergeCustomer_Load(object sender, EventArgs e)
    {

    }

    private void srchFromCustomer_OnSelected(object sender, EventArgs e)
    {
      _fromCustomer = srchFromCustomer.Text;
      enableMerge();
    }

    private void srchToCustomer_OnSelected(object sender, EventArgs e)
    {
      _toCustomer = srchToCustomer.Text;
      enableMerge();
    }
    private void enableMerge()
    {
      if (!string.IsNullOrEmpty(_fromCustomer) && !string.IsNullOrEmpty(_toCustomer))
        btnMerge.Enabled = true;
    }
  }
}
