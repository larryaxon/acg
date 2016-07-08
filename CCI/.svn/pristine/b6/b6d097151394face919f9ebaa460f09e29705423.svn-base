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
  public partial class frmCityHostedCreditMemoMaintenance : ScreenBase
  {
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    private bool _loading = false;

    public frmCityHostedCreditMemoMaintenance()
    {
      InitializeComponent();

    }

    #region form events

    private void btnSave_Click(object sender, EventArgs e)
    {
      saveFields();
    }
    private void srchCustomer_OnSelected(object sender, EventArgs e)
    {
      reloadGrid(false);
    }
    private void srchCreditMemos_OnSelected(object sender, EventArgs e)
    {
      reloadGrid(false);
    }
    private void srchBillDate_ValueChanged(object sender, EventArgs e)
    {
      reloadGrid(false);
    }
    private void srchCreditMemos_RowSelected(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      DataGridViewRow row = e.SelectedRow;
      loadFields(row);
    }
    private void frmCityHostedCreditMemoMaintenance_Load(object sender, EventArgs e)
    {
      _loading = true;
      srchBillDate.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1); // first of current month (billdate)
      srchCustomer.SearchExec = new SearchDataSourceCustomer();
    //  srchDealer.SearchExec = new SearchDataSourceEntity("Dealer", "CCI");
      txtCustomer.SearchExec = new SearchDataSourceCustomer();
      srchCreditMemos.Init("CityHostedCreditMemos", null); // this needs to be fixed
      srchCreditMemos.ClearSearch();
      _loading = false;
    }
    private void btnNew_Click(object sender, EventArgs e)
    {
      clearFields();
    }
    private void btnDelete_Click(object sender, EventArgs e)
    {
      deleteRecord();
    }
    private void ckAdjustmentsOnly_CheckedChanged(object sender, EventArgs e)
    {
      reloadGrid(false);
    }
    private void btnLoad_Click(object sender, EventArgs e)
    {
      reloadGrid(true);
    }

    #endregion

    #region private methods
    private void reloadGrid(bool forceRefresh)
    {
      if (!_loading && (ckAutoRefresh.Checked || forceRefresh))
      {
        Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
        if (!string.IsNullOrEmpty(srchCustomer.Text))
          criteria.Add("CustomerID", new string[] { ctlSearchGrid.opEQUALS, srchCustomer.Text });
        criteria.Add("BillDate", new string[] { ctlSearchGrid.opEQUALS, srchBillDate.Value.ToShortDateString() });
        srchCreditMemos.SearchCriteria = criteria;
        srchCreditMemos.ReLoad();
        clearFields();
      }
    }
    private void saveFields()
    {
      string ID = txtID.Text;
      string customerID = txtCustomer.Text;
      DateTime billDate = dtBillDate.Value;
      decimal memoAmount = CommonFunctions.CDecimal(txtCreditAmount.Text);
      string description = txtDescription.Text;
      int? ret = _dataSource.updateCreditMemo(ID, customerID, billDate, memoAmount, description, SecurityContext.User, "CreditMemo");
      reloadGrid(true);
      MessageBox.Show("Record saved");
    }
    private void clearFields()
    {
      txtID.Text = string.Empty;
      txtCustomer.Text = string.Empty;
      dtBillDate.Value = srchBillDate.Value;
      txtCreditAmount.Text = string.Empty;
      txtDescription.Text = string.Empty;
    }
    private void loadFields(DataGridViewRow row)
    {
      bool isCredit = CommonFunctions.CBoolean(row.Cells["CreditMemo"].Value);
      if (!isCredit)
      {
        MessageBox.Show("You can only edit credit memos");
        return;
      }
      txtID.Text = CommonFunctions.CString(row.Cells["ID"].Value);
      txtCustomer.Text = CommonFunctions.CString(row.Cells["CustomerID"].Value);
      dtBillDate.Value = CommonFunctions.CDateTime(row.Cells["BillDate"].Value);
      txtCreditAmount.Text = CommonFunctions.CString(row.Cells["Amount"].Value);
      txtDescription.Text = CommonFunctions.CString(row.Cells["Reference"].Value);
    }
    private void deleteRecord()
    {
      string id = txtID.Text;
      if (string.IsNullOrEmpty(id) || !CommonFunctions.IsNumeric(id))
        MessageBox.Show("There is no record to delete");
      else
      {
        _dataSource.deleteCreditMemo(id);
        clearFields();
        reloadGrid(true);
        MessageBox.Show("Record deleted");
      }
    }

    #endregion

  }
}
