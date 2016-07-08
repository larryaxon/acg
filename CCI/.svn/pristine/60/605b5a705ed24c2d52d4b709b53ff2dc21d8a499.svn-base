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
  public partial class frmCityHostedCustomerUSOCMatching : ScreenBase
  {
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    private DateTime _billDate { get { return dtBillDate.Value; } set { dtBillDate.Value = value; } }
    private MainForm _main { get { return CommonFormFunctions.getMainForm(this); } }
    private DataGridViewRow _selectedRow = null;
    private bool _autoRefresh = false;
    public Dictionary<string, string> ColumnCaptions { get; set; }

    public bool AutoRefresh
    {
      get { return _autoRefresh; }
      set { _autoRefresh = value; }
    }
    public frmCityHostedCustomerUSOCMatching()
    {
      ColumnCaptions = null;
      InitializeComponent();
      srchUsocMatching.Init(CommonData.UnmatchedNameTypes.CityHostedCustomerUSOCMatching, "USOC Matching");
      ctlCityHostedDetail1.Load(SecurityContext, null);
    }

    #region form events

    private void frmCityHostedCustomerUSOCMatching_Load(object sender, EventArgs e)
    {
      dtBillDate.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
      cboSource.Text = "Unmatched";
      splitMain.Panel2Collapsed = !ckShowResearch.Checked;
      loadCombos();
      txtComment.Visible = false;

    }
    private void dtBillDate_ValueChanged(object sender, EventArgs e)
    {
      loadMatching(false);
    }
    private void cboSource_SelectedIndexChanged(object sender, EventArgs e)
    {
      loadMatching(false);

    }
    private void srchUsocMatching_RowSelected(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      _selectedRow = e.SelectedRow;
      lblRowSelected.Visible = true;
      string name = CommonFunctions.CString(_selectedRow.Cells["CustomerName"].Value);
      name = name.Substring(0, Math.Min(name.Length, 12));
      lblRowSelected.Text = string.Format("{0}.{1}", name,
        CommonFunctions.CString(_selectedRow.Cells["RetailUSOC"].Value));
      refreshDetail(e.SelectedRow);
      if (CommonFunctions.CString(_selectedRow.Cells["ExceptionLogged"].Value) == "Yes")
      {
        this.btnExecute.Visible = false;
        this.btnUndo.Visible = true;
      }
      else
      {
        this.btnExecute.Visible = true;
        this.btnUndo.Visible = false;
      }
    }
    private void ckShowResearch_CheckedChanged(object sender, EventArgs e)
    {
      splitMain.Panel2Collapsed = !ckShowResearch.Checked;
    }
    private void btnExecute_Click(object sender, EventArgs e)
    {
      if (srchUsocMatching.SelectedRow == null)
      {
        MessageBox.Show("You must have a row selected");
        return;
      }
      DataRow dRow = ((DataRowView)srchUsocMatching.SelectedRow.DataBoundItem).Row;
      string rowData = CommonFunctions.dbRowToText(dRow, srchUsocMatching.DataColumns, ColumnCaptions);
      if (cboDisposition.SelectedIndex >= 0) // only process if a disposition is selected
      {
        _dataSource.executeDisposition(srchUsocMatching.NameType,
          cboDisposition.Text,
          CommonFunctions.CString(srchUsocMatching.SelectedRow.Cells["CustomerID"].Value),
          "Wholes vs Retail Matching Exception",
          CommonFunctions.CString(srchUsocMatching.SelectedRow.Cells["Source"].Value),
          _billDate,
          CommonFunctions.CString(srchUsocMatching.SelectedRow.Cells["RetailUSOC"].Value),
          CommonFunctions.CString(srchUsocMatching.SelectedRow.Cells["WholesaleUSOC"].Value),
          SecurityContext.User, string.Format("{0}: \r\n{1}", this.txtComment.Text, rowData));
        loadMatching(true);
      }
    }
    private void btnUndo_Click(object sender, EventArgs e)
    {
      if (srchUsocMatching.SelectedRow == null)
      {
        MessageBox.Show("You must have a row selected");
        return;
      }
      DataRow dRow = ((DataRowView)srchUsocMatching.SelectedRow.DataBoundItem).Row;
      string rowData = CommonFunctions.dbRowToText(dRow, srchUsocMatching.DataColumns, ColumnCaptions);
      _dataSource.executeUndoException(srchUsocMatching.NameType,
        cboDisposition.Text,
        CommonFunctions.CString(srchUsocMatching.SelectedRow.Cells["CustomerID"].Value),
        "Wholes vs Retail Matching Exception",
        CommonFunctions.CString(srchUsocMatching.SelectedRow.Cells["Source"].Value),
        CommonFunctions.CDateTime(srchUsocMatching.SelectedRow.Cells["BillDate"].Value),
        CommonFunctions.CString(srchUsocMatching.SelectedRow.Cells["RetailUSOC"].Value),
        CommonFunctions.CString(srchUsocMatching.SelectedRow.Cells["WholesaleUSOC"].Value),
        SecurityContext.User, string.Format("{0}: \r\n{1}", this.txtComment.Text, rowData));
      loadMatching(true);
      this.btnExecute.Visible = true;
      this.btnUndo.Visible = false;
    }

    #endregion

    #region private methods

    private void loadMatching(bool forceRefresh)
    {
      if (_autoRefresh || forceRefresh)
      {
        Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
        if (cboSource.Text.Equals("Unmatched", StringComparison.CurrentCultureIgnoreCase))
          criteria.Add("Source", new string[] { ctlSearchGrid.opNOTEQUALS, "Matched" });
        else
          if (!cboSource.Text.Equals("All", StringComparison.CurrentCultureIgnoreCase))
            criteria.Add("Source", new string[] { ctlSearchGrid.opEQUALS, cboSource.Text });
        criteria.Add("BillDate", new string[] { ctlSearchGrid.opEQUALS, dtBillDate.Value.ToShortDateString() });
        if (!ckIncludeExceptions.Checked)
          criteria.Add("ExceptionLogged", new string[] { ctlSearchGrid.opEQUALS, "No" });
        srchUsocMatching.SearchCriteria = criteria;
        this.txtComment.Text = String.Empty;
        this.txtCommentButton.Text = String.Empty;
        this.btnExecute.Visible = true;
        this.btnUndo.Visible = false;
        srchUsocMatching.ReLoad();
      }
    }
    private void loadCombos()
    {
      cboDisposition.Items.Clear();
      cboDisposition.Items.AddRange(_dataSource.getDispositionList());
      cboDisposition.SelectedIndex = 0;
    }
    private void refreshDetail(DataGridViewRow row)
    {
      if (ckShowResearch.Checked)
      {
        string customerID = CommonFunctions.CString(row.Cells["CustomerID"].Value);
        ctlCityHostedDetail1.CustomerID = customerID;
        ctlCityHostedDetail1.ReLoad();
      }
    }

    #endregion

    private void ckIncludeExceptions_CheckedChanged(object sender, EventArgs e)
    {
      loadMatching(false);
    }
    private void txtCommentButton_Click(object sender, EventArgs e)
    {
      if (this.txtComment.Visible == true)
      {
        txtComment.Visible = false;
        txtCommentButton.Text = txtComment.Text;
      }
      else
      {
        txtComment.Visible = true;
        txtComment.Focus();
      }
    }
    private void txtComment_Leave(object sender, EventArgs e)
    {
      txtCommentButton.Text = txtComment.Text;
      txtComment.Visible = false;
    }
    private void btnLoad_Click(object sender, EventArgs e)
    {
      loadMatching(true);
    }
  }
}
