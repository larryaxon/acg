﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ACG.App.Common;
using ACG.DesktopClient.Common;
using ACG.DesktopClient.Screens;

using ACG.Sys.Data;

namespace ACG.DesktopClient.Screens
{
  public partial class frmBudgetQuery : ScreenBase
  {
    #region method data
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    private string _customer = string.Empty;
    private string _project = string.Empty;
    private string _subproject = string.Empty;
    private int _timeID = -1;
    private int _budgetID = -1;
    private bool _populatingCombos = false;
    #endregion
    public frmBudgetQuery()
    {
      InitializeComponent();
    }

    #region form events
    private void frmBudgetQuery_Load(object sender, EventArgs e)
    {
      initScreen();
    }
    private void srchBudgetSummary_RowSelected(object sender, MaintenanceGridRowSelectedArgs e)
    {
      DataGridViewRow row = e.SelectedRow;
      _customer = CommonFunctions.CString(row.Cells[CommonData.fieldCUSTOMERID].Value);
      _project = CommonFunctions.CString(row.Cells[CommonData.fieldPROJECTID].Value);
      _subproject = CommonFunctions.CString(row.Cells[CommonData.fieldSUBPROJECTID].Value);
      Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
      criteria.Add(CommonData.fieldCUSTOMERID, new string[] { ctlSearchGrid.opEQUALS, _customer });
      criteria.Add(CommonData.fieldPROJECTID, new string[] { ctlSearchGrid.opEQUALS, _project });
      criteria.Add(CommonData.fieldSUBPROJECTID, new string[] { ctlSearchGrid.opEQUALS, _subproject });
      srchEditBudget.SearchCriteria = criteria;
      srchEditTime.SearchCriteria = criteria;
      srchEditBudget.ReLoad();
      srchEditTime.ReLoad();

    }
    private void srchEditBudget_RowSelected(object sender, MaintenanceGridRowSelectedArgs e)
    {
      loadBudgetEditFields(e.SelectedRow);
    }
    private void btnBudgetSave_Click(object sender, EventArgs e)
    {
      saveBudgetEditFields();
    }
    private void srchEditTime_RowSelected(object sender, MaintenanceGridRowSelectedArgs e)
    {
      loadTimeEditFields(e.SelectedRow);
    }
    private void btnTimeSave_Click(object sender, EventArgs e)
    {
      saveTimeEditFields();
    }
    private void cboCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!_populatingCombos)
      {
        getThroughDate();
        refreshGrids();
      }
    }
    private void btnSetThroughDate_Click(object sender, EventArgs e)
    {
      setThroughDate();
    }
    #endregion

    #region private methods
    private void initScreen()
    {
      loadCombos();
      getThroughDate();
      srchBudgetQuery.Load(CommonData.NameTypes.BudgetQuery, "BudgetQuery");
      srchBudgetQuery.ReLoad();
      srchBudgetSummary.Load(CommonData.NameTypes.BudgetSummary, "BudgetSummary");
      srchBudgetSummary.ReLoad();
      srchEditBudget.Load(CommonData.NameTypes.BudgetEditProjectBudget, "ProjectBudget");
      srchEditBudget.HiddenColumns.Add(CommonData.fieldCUSTOMERID, null);
      srchEditBudget.HiddenColumns.Add(CommonData.fieldPROJECTID, null);
      srchEditBudget.HiddenColumns.Add(CommonData.fieldSUBPROJECTID, null);
      srchEditTime.Load(CommonData.NameTypes.BudgetEditTimeEntry, "TimeEntry");
      srchEditTime.HiddenColumns.Add(CommonData.fieldCUSTOMERID, null);
      srchEditTime.HiddenColumns.Add(CommonData.fieldPROJECTID, null);
      srchEditTime.HiddenColumns.Add(CommonData.fieldSUBPROJECTID, null);
      enableFields("Time", false);
      enableFields("Budget", false);
    }
    private void loadTimeEditFields(DataGridViewRow row)
    {
      _timeID = CommonFunctions.CInt(row.Cells[CommonData.fieldID].Value, -1);
      if (_timeID > 0)
      {
        enableFields("Time", true);
        txtTimeCustomerProject.Text = string.Format("{0}.{1}.{2}.{3}", _customer, _project, _subproject, CommonFunctions.CString(row.Cells[CommonData.fieldRESOURCEID].Value));
        dtTimeWorkedDate.Value = CommonFunctions.CDateTime(row.Cells[CommonData.fieldTIMEDATE].Value);
        cboBillingCode.Text = CommonFunctions.CString(row.Cells[CommonData.fieldBILLINGCODE].Value);
        txtEnteredHours.Text = (CommonFunctions.CDecimal(row.Cells[CommonData.fieldENTEREDMINUTES].Value, 0) / (decimal)60).ToString();
        txtBilledHours.Text = (CommonFunctions.CDecimal(row.Cells[CommonData.fieldBILLEDMINUTES].Value, 0) / (decimal)60).ToString();
        txtTimeRate.Text = CommonFunctions.CDecimal(row.Cells[CommonData.fieldRATE].Value, 0).ToString();
        txtBilledAmount.Text = CommonFunctions.CDecimal(row.Cells[CommonData.fieldBILLEDAMOUNT].Value, 0).ToString();
        txtInvoiceNumber.Text = CommonFunctions.CString(row.Cells[CommonData.fieldINVOICENUMBER].Value);
        txtInvoiceDate.Text = CommonFunctions.CDateTime(row.Cells[CommonData.fieldINVOICEDATE].Value).ToShortDateString();
        txtTimeDescription.Text = CommonFunctions.CString(row.Cells[CommonData.fieldDESCRIPTION].Value);
      }
      else
        enableFields("Time", false);
    }
    private void saveTimeEditFields()
    {
      Hashtable timeFields = new Hashtable();
      timeFields.Add(CommonData.fieldTIMEID, _timeID);
      timeFields.Add(CommonData.fieldTIMEDATE, dtTimeWorkedDate.Value);
      timeFields.Add(CommonData.fieldBILLINGCODE, cboBillingCode.Text);
      decimal billedHours = CommonFunctions.CDecimal(txtBilledHours.Text, 0);
      timeFields.Add(CommonData.fieldBILLEDMINUTES, CommonFunctions.RoundMinutes(CommonFunctions.CInt(billedHours * 60), "quarter"));
      decimal rate = CommonFunctions.CDecimal(txtTimeRate.Text, 0);
      timeFields.Add(CommonData.fieldRATE, rate);
      decimal amount = rate * billedHours;
      timeFields.Add(CommonData.fieldBILLEDAMOUNT, amount);
      _dataSource.updateTimeEntry(timeFields);
      srchEditTime.ReLoad();
    }
    private void loadBudgetEditFields(DataGridViewRow row)
    {
      _budgetID = CommonFunctions.CInt(row.Cells[CommonData.fieldID].Value, -1);
      if (_budgetID > 0)
      {
        enableFields("Budget", true);
        txtBudgetCustomerProject.Text = string.Format("{0}.{1}.{2}.{3}", _customer, _project, _subproject, CommonFunctions.CString(row.Cells[CommonData.fieldRESOURCEID].Value));
        txtOriginalBudgetHours.Text = CommonFunctions.CString(CommonFunctions.CDecimal(row.Cells[CommonData.fieldORIGINALBUDGETHOURS].Value));
        txtRevisedBudgetHours.Text = CommonFunctions.CString(CommonFunctions.CDecimal(row.Cells[CommonData.fieldREVISEDBUDGETHOURS].Value));
        txtBudgetRate.Text = CommonFunctions.CString(CommonFunctions.CDecimal(row.Cells[CommonData.fieldRATE].Value));
        txtEstimatedHoursToComplete.Text = CommonFunctions.CString(CommonFunctions.CDecimal(row.Cells[CommonData.fieldESTIMATEDHOURSTOCOMPLETE].Value));
        txtOriginalCompletionDate.Text = CommonFunctions.CDateTime(row.Cells[CommonData.fieldORIGINALCOMPLETIONDATE].Value).ToShortDateString();
        dtEstimatedCompletionDate.Value = CommonFunctions.CDateTime(row.Cells[CommonData.fieldREVISEDCOMPLETIONDATE].Value);
        //OriginalBudgetHours, RevisedBudgetHours, Rate, EstimatedHoursToComplete, OriginalCompletionDate, RevisedCompletionDate
      }
      else
        enableFields("Budget", false);
    }
    private void saveBudgetEditFields()
    {
      Hashtable budgetFields = new Hashtable();
      budgetFields.Add(CommonData.fieldID, _budgetID);
      budgetFields.Add(CommonData.fieldREVISEDBUDGETHOURS, CommonFunctions.CDecimal(txtRevisedBudgetHours.Text, 0));
      budgetFields.Add(CommonData.fieldRATE, CommonFunctions.CDecimal(txtBudgetRate.Text, 0));
      budgetFields.Add(CommonData.fieldESTIMATEDHOURSTOCOMPLETE, CommonFunctions.CDecimal(txtEstimatedHoursToComplete.Text));
      budgetFields.Add(CommonData.fieldREVISEDCOMPLETIONDATE, dtEstimatedCompletionDate.Value);
      _dataSource.updateProjectBudget(budgetFields);
      srchEditBudget.ReLoad();
    }
    private void loadCombos()
    {
      if (cboCustomer.Items.Count == 0 || cboBillingCode.Items.Count == 0)
      {
        _populatingCombos = true;        
        ACGForm picklists = _dataSource.getDSPickLists(new Hashtable(), new ArrayList() { CommonData.CUSTOMER, CommonData.fieldBILLINGCODE }, string.Empty, SecurityContext.User, string.Empty);
        foreach (ACGFormItem picklist in picklists)
        {
          ComboBox ctl = cboBillingCode;
          string val = string.Empty;
          switch (picklist.ID)
          {
            //case CommonData.USER:
            //  ctl = cboResource;
            //  val = SecurityContext.User;
            //  break;
            case CommonData.CUSTOMER:
              ctl = cboCustomer;
              val = null;
              break;
            case CommonData.fieldBILLINGCODE:
              ctl = cboBillingCode;
              val = CommonData.BILLABLE;
              break;
          }
          CommonFormFunctions.populatePickList(ctl, (ACGTable)((ACGFormItem)picklist).Value, val, true);
        }
        _populatingCombos = false;
      }
    }

    private void enableFields(string tableName, bool enable)
    {
      if (tableName.Equals("Time", StringComparison.CurrentCultureIgnoreCase))
      {
        dtTimeWorkedDate.Enabled = enable;
        txtBilledHours.Enabled = enable;
        txtTimeRate.Enabled = enable;
        cboBillingCode.Enabled = enable;
        btnTimeSave.Enabled = enable;
      }
      else
      {
        txtRevisedBudgetHours.Enabled = enable;
        txtBudgetRate.Enabled = enable;
        txtEstimatedHoursToComplete.Enabled = enable;
        dtEstimatedCompletionDate.Enabled = enable;
        btnBudgetSave.Enabled = enable;
      }
    }
    private void refreshGrids()
    {
      string customerid = cboCustomer.Text;
      Dictionary<string, string[]> criteria = new Dictionary<string,string[]>(StringComparer.CurrentCultureIgnoreCase);
      criteria.Add(CommonData.fieldCUSTOMERID, new string[] { ctlSearchGrid.opEQUALS, customerid });
      srchBudgetSummary.SearchCriteria = criteria;
      srchBudgetSummary.ReLoad();
      criteria.Clear();
      string customername = _dataSource.getCustomerName(customerid);
      criteria.Add("customername", new string[] { ctlSearchGrid.opEQUALS, customername });
      srchBudgetQuery.SearchCriteria = criteria;
      srchBudgetQuery.ReLoad();
    }
    private void setThroughDate()
    {
      if (cboCustomer.SelectedIndex >= 0 && cboCustomer.SelectedIndex < cboCustomer.Items.Count)
      {
        if (dtBudgetThroughDate.Value != null && dtBudgetThroughDate.Value != CommonData.PastDateTime && dtBudgetThroughDate.Value != CommonData.FutureDateTime)
        {
          _dataSource.setThroughDate(cboCustomer.Text, dtBudgetThroughDate.Value);
        }
      }
    }
    private void getThroughDate()
    {
      if (cboCustomer.SelectedIndex >= 0 && cboCustomer.SelectedIndex < cboCustomer.Items.Count)
      {
        string customerid = cboCustomer.Text;
        DateTime? throughDate = _dataSource.getThroughDate(customerid);
        if (throughDate != null)
          dtBudgetThroughDate.Value = (DateTime)throughDate;
      }
    }
    #endregion

    private void btnFlagBudget_Click(object sender, EventArgs e)
    {
      if (cboCustomer.SelectedIndex >= 0 && cboCustomer.SelectedIndex < cboCustomer.Items.Count &&
        dtBudgetThroughDate.Value != null && dtBudgetThroughDate.Value != CommonData.PastDateTime && dtBudgetThroughDate.Value != CommonData.FutureDateTime)
      {
        _dataSource.flagBudget(cboCustomer.Text, dtBudgetThroughDate.Value);
      }
      else
        MessageBox.Show("Must have valid Customer and Through Date Selected");
    }


  }
}
