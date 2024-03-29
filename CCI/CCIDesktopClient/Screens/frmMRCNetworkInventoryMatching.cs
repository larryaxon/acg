﻿using System;
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
  public partial class frmMRCNetworkInventoryMatching : ScreenBase
  {
    #region module data
    private bool _displayRetailWholesale = false;
    private DataGridViewRow _selectedRow = null;
    public Dictionary<string, string> ColumnCaptions { get; set; }


    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    private DateTime _billDate { get { return dtBillDate.Value; } set { dtBillDate.Value = value; } }
    private bool _autoRefresh = false;

    #endregion

    #region public properties
    public bool DisplayRetailWholesale { get { return _displayRetailWholesale; } set { _displayRetailWholesale = value; } }
    public bool IsRetail { get { return !ckWholesale.Checked; } set { ckWholesale.Checked = !value; } }
    public bool AutoRefresh
    {
      get { return _autoRefresh; }
      set { _autoRefresh = value; }
    }
    #endregion

    public frmMRCNetworkInventoryMatching()
    {
      ColumnCaptions = null;
      InitializeComponent();
    }
    #region form events
    private void frmMRCNetworkInventoryMatching_Load(object sender, EventArgs e)
    {
      loadCombos();
      this.txtComment.Text = string.Empty;
      this.txtCommentButton.Text = string.Empty;
      txtComment.Visible = false;
      _billDate = _dataSource.getCurrentBillingMonth();
      if (!_displayRetailWholesale)
        ckWholesale.Visible = false;
      if (!IsRetail)
        cboExceptionSearch.Visible = false;
      reload(true);
    }
    private void ckUnmatchedOnly_CheckedChanged(object sender, EventArgs e)
    {
      filterSearch(false);
    }
    private void ckWholesale_CheckedChanged(object sender, EventArgs e)
    {
      reload(false);
    }
    private void cboExceptionSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
      reload(false);
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

    private void btnExecute_Click(object sender, EventArgs e)
    {
      if (srchMRCNIMatching.SelectedRow == null)
      {
        MessageBox.Show("You must have a row selected");
        return;
      }
      DataRow dRow = ((DataRowView)srchMRCNIMatching.SelectedRow.DataBoundItem).Row;
      string rowData = CommonFunctions.dbRowToText(dRow, srchMRCNIMatching.DataColumns, ColumnCaptions);
      if (cboDisposition.SelectedIndex >= 0) // only process if a disposition is selected
      {
        _dataSource.executeDisposition(srchMRCNIMatching.NameType,
          cboDisposition.Text,
          CommonFunctions.CString(srchMRCNIMatching.SelectedRow.Cells["CustomerID"].Value),
          "Network Inventory Matching Exception",
          CommonFunctions.CString(srchMRCNIMatching.SelectedRow.Cells["Exception"].Value),
          _billDate,
          CommonFunctions.CString(srchMRCNIMatching.SelectedRow.Cells["RetailUSOC"].Value),
           null,
          SecurityContext.User, string.Format("{0}: \r\n{1}", this.txtComment.Text, rowData));
        reload(true);
      }
    }
    private void btnUndo_Click(object sender, EventArgs e)
    {
      if (srchMRCNIMatching.SelectedRow == null)
      {
        MessageBox.Show("You must have a row selected");
        return;
      }
      DataRow dRow = ((DataRowView)srchMRCNIMatching.SelectedRow.DataBoundItem).Row;
      string rowData = CommonFunctions.dbRowToText(dRow, srchMRCNIMatching.DataColumns, ColumnCaptions);
      _dataSource.executeUndoException(srchMRCNIMatching.NameType,
        cboDisposition.Text,
        CommonFunctions.CString(srchMRCNIMatching.SelectedRow.Cells["CustomerID"].Value),
        "Network Inventory Matching Exception",
        CommonFunctions.CString(srchMRCNIMatching.SelectedRow.Cells["Exception"].Value),
        _billDate,
        CommonFunctions.CString(srchMRCNIMatching.SelectedRow.Cells["RetailUSOC"].Value),
         null,
        SecurityContext.User, string.Format("{0}: \r\n{1}", this.txtComment.Text, rowData));
      reload(true);
      this.btnExecute.Visible = true;
      this.btnUndo.Visible = false;
    }

    private void srchMRCNIMatching_RowSelected(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      _selectedRow = e.SelectedRow;
      lblRowSelected.Visible = true;
      string name = CommonFunctions.CString(_selectedRow.Cells["CustomerName"].Value);
      name = name.Substring(0,Math.Min(name.Length, 12));
      lblRowSelected.Text = string.Format("{0}.{1}", name,
        CommonFunctions.CString(_selectedRow.Cells["RetailUSOC"].Value));
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

    #endregion

    #region private methods
    private void filterSearch(bool forceRefresh)
    {
      if (_autoRefresh || forceRefresh)
      {
        bool unMatchedOnly = ckUnmatchedOnly.Checked;
        Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
        if (IsRetail)
        {
          lblTitle.Text = "Retail";
          if (unMatchedOnly)
          {
            if (cboExceptionSearch.Text != "All")
              criteria.Add("Exception", new string[] { ctlSearchGrid.opEQUALS, cboExceptionSearch.Text });
            else
              criteria.Add("Exception", new string[] { ctlSearchGrid.opISNOTNULL, null });
          }
          criteria.Add("RetailBillDate", new string[] { ctlSearchGrid.opEQUALS, _billDate.ToShortDateString() });

        }
        else
        {
          lblTitle.Text = "Wholesale";
          if (unMatchedOnly)
            criteria.Add("Matched", new string[] { ctlSearchGrid.opEQUALS, "No" });
          criteria.Add("WholesaleBillDate", new string[] { ctlSearchGrid.opEQUALS, _billDate.ToShortDateString() });

        }
        if (!ckIncludeExceptions.Checked)
          criteria.Add("ExceptionLogged", new string[] { ctlSearchGrid.opEQUALS, "No" });
        if (!ckInclude500s.Checked)
          criteria.Add("RetailUSOC", new string[] { ctlSearchGrid.opNOTEQUALS, "500" });
        srchMRCNIMatching.SearchCriteria = criteria;
        srchMRCNIMatching.ReLoad();
      }
    }
    private void reload(bool forceRefresh)
    {
      ckWholesale.Visible = DisplayRetailWholesale;
      if (IsRetail)
      {
        cboExceptionSearch.Visible = true;
        lblExceptionSearch.Visible = true;
        srchMRCNIMatching.Init(CommonData.UnmatchedNameTypes.CityHostedMRCNetworkInventoryMatch, null);
      }
      else
      {
        cboExceptionSearch.Visible = false;
        lblExceptionSearch.Visible = false;
        srchMRCNIMatching.Init(CommonData.UnmatchedNameTypes.CityHostedMRCWholesaleMatch, null);
      }
      filterSearch(forceRefresh);
      this.btnUndo.Visible = false;
      this.btnExecute.Visible = true;
    }
    private void loadCombos()
    {
      string[] exceptionList = new string[CommonData.NetworkInventoryExceptionList.GetLength(0) + 1];
      exceptionList[0] = "All";
      for (int i = 0; i < CommonData.NetworkInventoryExceptionList.GetLength(0); i++)
        exceptionList[i + 1] = CommonData.NetworkInventoryExceptionList[i];
      cboExceptionSearch.Items.Clear();
      cboExceptionSearch.Items.AddRange(exceptionList);
      cboExceptionSearch.Text = "All";
      cboDisposition.Items.Clear();
      cboDisposition.Items.AddRange(_dataSource.getDispositionList());
      cboDisposition.SelectedIndex = 0;
    }


    #endregion

    private void ckIncludeExceptions_CheckedChanged(object sender, EventArgs e)
    {
      reload(false);
    }

    private void btnLoad_Click(object sender, EventArgs e)
    {
      reload(true);
    }

  }
}
