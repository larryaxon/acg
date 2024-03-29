﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using CrystalDecisions.Shared;

using ACG.Common;
using ACG.App.Common;
using ACG.Sys.Data;
using ACG.DesktopClient.Common;
using ACG.DesktopClient.Reports;

namespace ACG.DesktopClient.Screens
{
  public partial class frmTimeEntry : ScreenBase
  {
    #region private method data and properties
    // Constants
    private const string START = "Start";
    private const string STOP = "Stop";
    private const string TIMERISRUNNING = "Timer is Running";
    private TimeSpan _plus15 = new TimeSpan(0, 15, 0);
    private TimeSpan _minus15 = new TimeSpan(0, -15, 0);
    // field data
    private int _timeID = -1;
    private string _customer { get { return cboCustomer.Text; } set { cboCustomer.Text = value; } }
    private string _resource { get { return cboResource.Text; } set { cboResource.Text = value; } }
    private string _project { get { return cboProject.Text; } set { cboProject.Text = value; } }
    private string _subProject { get { return cboSubProject.Text; } set { cboSubProject.Text = value; } }
    private string _billingCode { get { return cboBillingCode.Text; } set { cboBillingCode.Text = value; } }
    private string _description { get { return txtDescription.Text; } set { txtDescription.Text = value; } }
    private DateTime _timeEntryDate { get { return dtDate.Value.Date; } set { dtDate.Value = value; } }
    private TimeSpan _startTime { get { return offsetTime(tmStartTime.Value.TimeOfDay, false); } set { tmStartTime.Value = CommonFunctions.CDateTime(offsetTime(value, true)); }  }
    private TimeSpan _endTime { get { return offsetTime(tmEndTime.Value.TimeOfDay, false); } set { tmEndTime.Value = CommonFunctions.CDateTime(offsetTime(value, true)); } }
    private int _enteredMinutes 
    { 
      get { return CommonFunctions.CInt(txtEnteredMinutes.Text, 0); } 
      set 
      { 
        txtBilledMinutes.Text = txtEnteredMinutes.Text = value.ToString();
        txtEnteredHours.Text = convertMinutes(txtEnteredMinutes.Text, false);
        txtBilledHours.Text = txtEnteredHours.Text;
      } 
    }
    private int _billedMinutes 
    { 
      get { return CommonFunctions.CInt(txtBilledMinutes.Text, 0); } 
      set 
      { 
        txtBilledMinutes.Text = value.ToString();
        txtBilledHours.Text = convertMinutes(txtBilledMinutes.Text, false);
      } 
    }
    private decimal _lastEstimatedHoursToComplete = 0;
    private decimal _estimatedHoursToComplete { get { return CommonFunctions.CDecimal(txtEstimatedHoursToComplete.Text, 0); } set { txtEstimatedHoursToComplete.Text = value.ToString(); } }
    private ACGCommonData.TimeEntrySummaryMode _summaryMode 
    { 
      get 
      { 
        if (radioDaily.Checked)
          return ACGCommonData.TimeEntrySummaryMode.Daily;
        if (radioWeekly.Checked)
          return ACGCommonData.TimeEntrySummaryMode.Weekly;
        return ACGCommonData.TimeEntrySummaryMode.ByProject;
      }
    }
    private string _internalNotes = string.Empty;
    // other
    private TimeSpan _hoursOffset = new TimeSpan(0, 0, 0);
    private TimeSpan _hoursOffsetMinus = new TimeSpan(0, 0, 0);
    private bool _firstTime = true; // set to init the screen in newTime() on the first time
    private bool _estimateHoursChanged = false;
    private bool _isAdmin { get { return SecurityContext.Security.HasObjectAccess("CanEnterOthersTime"); } }
    private bool _refreshingTimeDetail = false;
    private bool _enteringMinutes = false;  // is the user using start/end time or minutes to entere the time?
    private bool _isTimerRunning { get { return lblTimerRunning.Visible; } set { setTimerRunning(value); } }
    private bool _isNewMode { get { return lblNew.Visible; } set { lblNew.Visible = value; } }
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    #endregion
    public frmTimeEntry()
    {
      InitializeComponent();
      srchTimeDetail.NameType = ACGCommonData.NameTypes.TimeDetail;
      srchTimeDetail.LockedColumns.Add(ACGCommonData.fieldRESOURCEID, null);
      srchTimeDetail.Load(ACGCommonData.NameTypes.TimeDetail, ACGCommonData.fieldCUSTOMERID);

    }

    #region form events
    private void frmTimeEntry_Load(object sender, EventArgs e)
    {
      _resource = SecurityContext.User;
      int hour = _dataSource.getTimeZoneOffset(_resource);
      _hoursOffset = new TimeSpan(hour, 0, 0);
      _hoursOffsetMinus = new TimeSpan(-hour, 0, 0);
      if (_isAdmin)
      {
        ckAutoStartTimer.Checked = false;
        ckSelectByCustomer.Visible = true;
        ckSelectByResource.Visible = true;
        ckIncludeSubProject.Visible = true;
        ckIncludeOnlyUnpaidTime.Visible = true;
        grpFlagPaidTime.Visible = true;
        srchTimeDetail.CanSearchLockedColumns = true;
        srchResourceTime.NameType = ACGCommonData.NameTypes.ResourceTimeSummary;
        srchResourceTime.Load(ACGCommonData.NameTypes.ResourceTimeSummary, "ResourceTime");
        loadResourceTime(true);
      }
      else
      {
        srchTimeDetail.CanSearchLockedColumns = false;
        ckIncludePaidTime.Visible = false;
        srchResourceTime.Visible = false;
      }
      if (SecurityContext.Security.HasObjectAccess("DontAutoCreateNew"))
        ckAutoCreateNew.Checked = false;
      else
        ckAutoCreateNew.Checked = true;
      newTime();
    }
    private void btnSave_Click(object sender, EventArgs e)
    {
      saveTime();
      if (ckAutoCreateNew.Checked)
        newTime();
    }
    private void cboCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
      refreshProject();
    }
    private void cboProject_SelectedIndexChanged(object sender, EventArgs e)
    {
      refreshSubProject();
    }
    private void btnStartStop_Click(object sender, EventArgs e)
    {
      if (_isTimerRunning)
        stopTime();
      else
        startTime();

    }
    private void btnStartIncrement_Click(object sender, EventArgs e)
    {
      _startTime = _startTime.Add(_plus15);
      if (_endTime <= _startTime)
        _endTime = _startTime.Add(_plus15);
      resetEnteredMinutes();
    }
    private void btnStartDecrement_Click(object sender, EventArgs e)
    {
      _startTime = _startTime.Add(_minus15);
      resetEnteredMinutes();
    }
    private void btnEndIncrement_Click(object sender, EventArgs e)
    {
      _endTime = _endTime.Add(_plus15);
      resetEnteredMinutes();
    }
    private void btnEndDecrement_Click(object sender, EventArgs e)
    {
      _endTime = _endTime.Add(_minus15);
      if (_startTime >= _endTime)
        _startTime = _endTime.Add(_minus15);
      resetEnteredMinutes();
    }
    private void btnNew_Click(object sender, EventArgs e)
    {
      newTime();
    }
    private void srchTimeDetail_RowSelected(object sender, MaintenanceGridRowSelectedArgs e)
    {
      if (!_refreshingTimeDetail)
      {
        if (_isTimerRunning)
        {
          DialogResult ans = MessageBox.Show("Your timer is still running, do you want to stop the timer and save your current record before you go on?", TIMERISRUNNING, MessageBoxButtons.YesNoCancel);
          if (ans == DialogResult.Yes)
          {
            stopTime();
            saveTime();
          }
          else
          {
            if (ans == DialogResult.Cancel)
            {
              MessageBox.Show("Row select cancelled", TIMERISRUNNING);
              return;
            }
          }
        }
        int id = CommonFunctions.CInt(srchTimeDetail.SelectedID);
        loadTime(id);
      }
    }
    private void btnDelete_Click(object sender, EventArgs e)
    {
      deleteTime();
    }
    private void tmStartTime_ValueChanged(object sender, EventArgs e)
    {
      _enteringMinutes = false;
      resetEnteredMinutes();
    }
    private void tmEndTime_ValueChanged(object sender, EventArgs e)
    {
      _enteringMinutes = false;
      resetEnteredMinutes();
    }
    private void txtEnteredMinutes_Leave(object sender, EventArgs e)
    {
      _enteringMinutes = true;
      roundEnteredMinutes(false);
    }
    private void txtBilledMinutes_Leave(object sender, EventArgs e)
    {
      _enteringMinutes = true;
      roundEnteredMinutes(true);
    }
    private void btnReloadSummary_Click(object sender, EventArgs e)
    {
      loadSummary();
    }
    private void toolStripMenuItemExport_Click(object sender, EventArgs e)
    {
      CommonFormFunctions.exportToExcel(grdTimeSummary);
    }
    private void txtEstimatedHoursToComplete_Leave(object sender, EventArgs e)
    {
      updateEstimatedHours();
    }
    private void txtEstimatedHoursToComplete_TextChanged(object sender, EventArgs e)
    {
      _estimateHoursChanged = true;
    }
    private void btnSetFlag_Click(object sender, EventArgs e)
    {
      flagTime(radioPaid.Checked);
    }
    private void ckIncludePaidTime_CheckedChanged(object sender, EventArgs e)
    {
      loadResourceTime(false);
    }
    private void srchResourceTime_RowSelected(object sender, MaintenanceGridRowSelectedArgs e)
    {
      string resource = CommonFunctions.CString(e.SelectedRow.Cells["Resource"].Value);
      txtPaidResource.Text = resource;
    }
    private void btnResourceTimeRefresh_Click(object sender, EventArgs e)
    {
      loadResourceTime(false);
    }
    private void txtEnteredHours_Leave(object sender, EventArgs e)
    {
      _enteredMinutes = CommonFunctions.CInt(convertMinutes(txtEnteredHours.Text, true));
      txtBilledHours.Text = txtEnteredHours.Text;
      _billedMinutes = CommonFunctions.CInt(convertMinutes(txtBilledHours.Text, true));
    }
    private void txtBilledHours_Leave(object sender, EventArgs e)
    {
      _billedMinutes = CommonFunctions.CInt(convertMinutes(txtBilledHours.Text, true));
    }
    private void btnToday_Click(object sender, EventArgs e)
    {
      dtDate.Value = DateTime.Today;
    }
    private void btnStartNow_Click(object sender, EventArgs e)
    {
      tmStartTime.Value = CommonFunctions.CDateTime(offsetTime(CommonFunctions.Round(DateTime.Now.TimeOfDay, CommonData.TIMEINCREMENTQUARTER), true));
    }
    private void btnEndNow_Click(object sender, EventArgs e)
    {
      tmEndTime.Value = CommonFunctions.CDateTime(offsetTime(CommonFunctions.Round(DateTime.Now.TimeOfDay, CommonData.TIMEINCREMENTQUARTER), true)); ;
    }
    private void cboResource_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (cboResource.Text.Equals("Expenses", StringComparison.CurrentCultureIgnoreCase))
      {
        lblDirectCharge.Visible = true;
        txtDirectCharge.Visible = true;
      }
      else
      {
        lblDirectCharge.Visible = false;
        txtDirectCharge.Visible = false;
      }
    }

    #endregion

    #region private methods
    private void newTime()
    {
      // populate combo boxes
      loadCombos();
      setTimerRunning(false);
      _isNewMode = true;
      _enteringMinutes = false;
      txtEstimatedHoursToComplete.Text = string.Empty;
      _timeID = -1;
      // calculate begin and end dates of this week for filters
      DateTime dt;
      if (ckAutoStartTimer.Checked)
        dt = DateTime.Today;
      else
        dt = _timeEntryDate; // just default to the last one picked. This is good when we are entering time for last week
      int day = (int)dt.DayOfWeek;        
      DateTime dtBeginWeek = dt.AddDays(-day);
      DateTime dtEndWeek = dtBeginWeek.AddDays(6);
      if (ckAutoStartTimer.Checked || _firstTime) // only initialize the default data if we have autostart on
      {
        _firstTime = false;
        dtFromDate.Value = dtBeginWeek;
        dtToDate.Value = dtEndWeek;
        if (_startTime == new TimeSpan(0, 0, 0)) // if this is initialized to 12:0 midnight (no initial value), then
        {
          // then just set the clock to the current time (rounded down to the nearest 15 mins
          _startTime = roundTime(DateTime.Now.TimeOfDay);
          _endTime = _startTime.Add(_plus15);
        }
        else
        {
          // otherwise, set the time to reflect the end of the last time slice
          _startTime = roundTime(_endTime);
          _endTime = _startTime.Add(_plus15);
        }
        resetEnteredMinutes();
        // prefil data with defaults from most recent time slice
        int lastTimeID = _dataSource.getLastTimeID(SecurityContext);
        if (lastTimeID > 0)
        {
          // get default data from last time entered
          ACGTable timeEntry = _dataSource.getTimeData(lastTimeID);
          if (timeEntry != null && timeEntry.NumberRows > 0)
          {
            _resource = CommonFunctions.CString(timeEntry[0, ACGCommonData.fieldRESOURCEID]);
            _customer = CommonFunctions.CString(timeEntry[0, ACGCommonData.fieldCUSTOMERID]);
            refreshProject();
            _project = CommonFunctions.CString(timeEntry[0, ACGCommonData.fieldPROJECTID]);
            refreshSubProject();
            _subProject = CommonFunctions.CString(timeEntry[0, ACGCommonData.fieldSUBPROJECTID]);
            _billingCode = CommonFunctions.CString(timeEntry[0, ACGCommonData.fieldBILLINGCODE]);
            if (ckAutoStartTimer.Checked)
              startTime();
            _description = CommonFunctions.CString(timeEntry[0, ACGCommonData.fieldDESCRIPTION]);
            _lastEstimatedHoursToComplete = _estimatedHoursToComplete = _dataSource.getEstimatedHoursToComplete(_resource, _customer, _project, _subProject);
            
            Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
            criteria.Add(ACGCommonData.fieldRESOURCEID, new string[] { ctlSearchGrid.opEQUALS, _resource });
            criteria.Add("Date", new string[] { ctlSearchGrid.opGREATEROREQUAL, dtBeginWeek.ToShortDateString() });
            srchTimeDetail.SearchCriteria = criteria;
          }
        }
      }
      else
      {
        _startTime = roundTime(_endTime);
        _endTime = _startTime.Add(_plus15);
      }
      // popolate the time detail grid

      refreshTimeDetail();
      // populate the summary screen
      loadSummary();
    }
    private void loadCombos()
    {
      if (cboCustomer.Items.Count == 0) // only do this the first time
      {
        ACGForm picklists = _dataSource.getDSPickLists(new Hashtable(), new ArrayList() { ACGCommonData.USER, ACGCommonData.CUSTOMER, ACGCommonData.fieldBILLINGCODE }, string.Empty, SecurityContext.User, string.Empty);
        foreach (ACGFormItem picklist in picklists)
        {
          ComboBox ctl = cboResource;
          string val = string.Empty;
          switch (picklist.ID)
          {
            case ACGCommonData.USER:
              ctl = cboResource;
              val = SecurityContext.User;
              break;
            case ACGCommonData.CUSTOMER:
              ctl = cboCustomer;
              val = null;
              break;
            case ACGCommonData.fieldBILLINGCODE:
              ctl = cboBillingCode;
              val = ACGCommonData.BILLABLE;
              break;
          }
          CommonFormFunctions.populatePickList(ctl, (ACGTable)((ACGFormItem)picklist).Value, val);
        }
        if (!SecurityContext.Security.HasObjectAccess("CanEnterOthersTime"))
          cboResource.Enabled = false;
      }
    }
    private void saveTime()
    {
      Hashtable fields = new Hashtable();
      if (string.IsNullOrEmpty(_customer))
        MessageBox.Show("You must have a customer selected");
      else
      {
        if (_isTimerRunning)
        {
          DialogResult ans = MessageBox.Show("You have started the timer. Do you want to stop and use the current time as the end time?", TIMERISRUNNING, MessageBoxButtons.YesNoCancel);
          if (ans == DialogResult.Yes)
            stopTime(); // stop the timer and go one...
          else
            if (ans == DialogResult.Cancel)
            {
              MessageBox.Show("Cancel was pressed, record not saved...", TIMERISRUNNING);
              return; // exit and don't save
            }
            else
              setTimerRunning(false); // we will save the record, so change the toggle back to "Start"
        }
        fields.Add(ACGCommonData.fieldCUSTOMERID, _customer);
        fields.Add("resourceid", _resource);
        if (string.IsNullOrEmpty(_project))
          MessageBox.Show("You must have a project selected");
        else
        {
          if (_timeID > 0)
            fields.Add(ACGCommonData.fieldTIMEID, _timeID);
          fields.Add(ACGCommonData.fieldPROJECTID, _project);
          if (!string.IsNullOrEmpty(_subProject))
            fields.Add(ACGCommonData.fieldSUBPROJECTID, _subProject);
          if (string.IsNullOrEmpty(_billingCode))
            fields.Add(ACGCommonData.fieldBILLINGCODE, ACGCommonData.BILLABLE);
          else
            fields.Add(ACGCommonData.fieldBILLINGCODE, _billingCode);
          if (!string.IsNullOrEmpty(_description))
            fields.Add(ACGCommonData.fieldDESCRIPTION, _description);
          if (!string.IsNullOrEmpty(_internalNotes))
            fields.Add(ACGCommonData.fieldINTERNALNOTES, _internalNotes);
          fields.Add(ACGCommonData.fieldTIMEDATE, _timeEntryDate);
          if (lblDirectCharge.Visible) // this is an expense
            fields.Add(ACGCommonData.fieldBILLEDAMOUNT, CommonFunctions.CDecimal(txtDirectCharge.Text));
          else
          {
            fields.Add(ACGCommonData.fieldSTARTTIME, _startTime);
            fields.Add(ACGCommonData.fieldENDTIME, _endTime);
            if (_enteringMinutes)
              roundEnteredMinutes(false);
            else
              resetEnteredMinutes();

            fields.Add(ACGCommonData.fieldENTEREDMINUTES, _enteredMinutes);
            fields.Add(ACGCommonData.fieldBILLEDMINUTES, _billedMinutes);
            decimal rate = _dataSource.getTimeRate(_customer, _project, _resource, _timeEntryDate);
            fields.Add(ACGCommonData.fieldRATE, rate);
            decimal amount = (Convert.ToDecimal(_billedMinutes) / 60) * rate;
            fields.Add(ACGCommonData.fieldBILLEDAMOUNT, amount);
            decimal billedhours = (Convert.ToDecimal(_billedMinutes) / (decimal)60);
            if (billedhours > 0)
              _estimatedHoursToComplete = Math.Max((decimal)0, _lastEstimatedHoursToComplete - billedhours);
            updateEstimatedHours();
          }
          fields.Add(ACGCommonData.fieldLASTMODIFIEDDATETIME, DateTime.Now.ToString(CommonData.FORMATLONGDATETIME));
          fields.Add(CommonData.LASTMODIFIEDBY, SecurityContext.User);
          int? timeID  = _dataSource.updateTimeEntry(fields);
          if (timeID != null && _timeID == -1)  // this was an insert, so pick up the new timeid
            _timeID = (int)timeID;
          if (_resource.Equals(SecurityContext.User, StringComparison.CurrentCultureIgnoreCase)) // don't update the loast time id if we are editing someone else's time
            _dataSource.updateUserState(SecurityContext, null, _timeID.ToString());
        }
        refreshTimeDetail();
      }
      _isNewMode = false;
    }
    private void deleteTime()
    {
      if (_timeID == -1)
        MessageBox.Show("This is a new, unsaved time slice. You cannot delete it.", "Time Entry");
      else
        _dataSource.deleteTimeEntry(_timeID);
      int id = _dataSource.getLastTimeID(SecurityContext);
      if (id == _timeID) // we just deleted the last time id 
        _dataSource.updateUserState(SecurityContext, null, _dataSource.getMostRecentTimeID(SecurityContext).ToString()); // so just save the max(timeid) for this user
      newTime();
    }
    private void setTimerRunning(bool isRunning)
    {
      if (isRunning)
      {
        btnStartStop.Text = STOP;
        lblTimerRunning.Visible = true;
      }
      else
      {
        btnStartStop.Text = START;
        lblTimerRunning.Visible = false;
      }
    }
    private void loadTime(int id)
    {
      _isTimerRunning = false;
      _isNewMode = false;
      ACGTable timeEntry = _dataSource.getTimeData(id);
      if (timeEntry != null && timeEntry.NumberRows > 0)
      {
        _timeID = id;
        _resource = CommonFunctions.CString(timeEntry[0, ACGCommonData.fieldRESOURCEID]);
        if (_resource.Equals("Expenses", StringComparison.CurrentCultureIgnoreCase))
        {
          lblDirectCharge.Visible = true;
          txtDirectCharge.Visible = true;
          txtDirectCharge.Text = CommonFunctions.CString(timeEntry[0, ACGCommonData.fieldBILLEDAMOUNT]);
        }
        else
        {
          lblDirectCharge.Visible = false;
          txtDirectCharge.Visible = false;
        }
        _customer = CommonFunctions.CString(timeEntry[0, ACGCommonData.fieldCUSTOMERID]);
        refreshProject();
        _project = CommonFunctions.CString(timeEntry[0, ACGCommonData.fieldPROJECTID]);
        refreshSubProject();
        _subProject = CommonFunctions.CString(timeEntry[0, ACGCommonData.fieldSUBPROJECTID]);
        _billingCode = CommonFunctions.CString(timeEntry[0, ACGCommonData.fieldBILLINGCODE]);
        _timeEntryDate = CommonFunctions.CDateTime(timeEntry[0, ACGCommonData.fieldTIMEDATE]);
        _endTime = CommonFunctions.CTime(timeEntry[0, ACGCommonData.fieldENDTIME]);
        _startTime = CommonFunctions.CTime(timeEntry[0, ACGCommonData.fieldSTARTTIME]);
        _enteredMinutes = CommonFunctions.CInt(timeEntry[0, ACGCommonData.fieldENTEREDMINUTES]);
        _billedMinutes = CommonFunctions.CInt(timeEntry[0, ACGCommonData.fieldBILLEDMINUTES]);
        _internalNotes = CommonFunctions.CString(timeEntry[0, ACGCommonData.fieldINTERNALNOTES]);
        _description = CommonFunctions.CString(timeEntry[0, ACGCommonData.fieldDESCRIPTION]);
      }
    }
    private void loadSummary()
    {
      DataSet ds = _dataSource.getTimeSummary(_resource, _customer, ckSelectByResource.Checked, ckSelectByCustomer.Checked, 
        ckSummaryBillableOnly.Checked, dtFromDate.Value, dtToDate.Value, _summaryMode, ckIncludeSubProject.Checked, ckIncludeOnlyUnpaidTime.Checked);
      CommonFormFunctions.displayDataSetGrid(grdTimeSummary, ds);
    }
    private void startTime()
    {
      setTimerRunning(true);
      _timeEntryDate = DateTime.Today.Date;
      _endTime = _startTime = CommonFunctions.Round(DateTime.Now.TimeOfDay, CommonData.TIMEINCREMENTQUARTER);
      _enteredMinutes = _billedMinutes = 0;
    }
    private void stopTime()
    {
      setTimerRunning(false);
      _endTime = CommonFunctions.Round(DateTime.Now.TimeOfDay, CommonData.TIMEINCREMENTQUARTER);
      resetEnteredMinutes();
    }
    private void resetEnteredMinutes()
    {
      _enteredMinutes = Convert.ToInt32(_endTime.TotalMinutes) - Convert.ToInt32(_startTime.TotalMinutes);
      if (_billingCode.Equals("Billable", StringComparison.CurrentCultureIgnoreCase))
        _billedMinutes = _enteredMinutes;
      else
        _billedMinutes = 0;
      _enteringMinutes = false;
    }
    private void roundEnteredMinutes(bool billedOnly)
    {
      if (!billedOnly)
      {
        if (_enteredMinutes == 0) // cannot have less than 15 mins
          _enteredMinutes = 15;
        else
          _enteredMinutes = ACGCommonFunctions.RoundMinutes(_enteredMinutes, "quarter");
      }
      if (_billingCode.Equals("Billable", StringComparison.CurrentCultureIgnoreCase))
        _billedMinutes = ACGCommonFunctions.RoundMinutes(_billedMinutes, "quarter");
      else
        _billedMinutes = 0;
    }
    private void refreshProject()
    {
      Hashtable context = new Hashtable();
      context.Add(ACGCommonData.fieldCUSTOMERID, _customer);
      ACGForm picklists = _dataSource.getDSPickLists(context, new ArrayList() { "project" }, string.Empty, SecurityContext.User, string.Empty);
      CommonFormFunctions.populatePickList(cboProject, (ACGTable)((ACGFormItem)picklists[0]).Value, null);
    }
    private void refreshSubProject()
    {
      Hashtable context = new Hashtable();
      context.Add(ACGCommonData.fieldCUSTOMERID, _customer);
      context.Add(ACGCommonData.fieldPROJECTID, _project);
      ACGForm picklists = _dataSource.getDSPickLists(context, new ArrayList() { "subproject" }, string.Empty, SecurityContext.User, string.Empty);
      CommonFormFunctions.populatePickList(cboSubProject, (ACGTable)((ACGFormItem)picklists[0]).Value, null);
    }
    private void refreshTimeDetail()
    {
      _refreshingTimeDetail = true;
      srchTimeDetail.ReLoad();
      _refreshingTimeDetail = false;
    }
    private TimeSpan roundTime(TimeSpan t)
    {
      TimeSpan newTime = DateTime.Now.TimeOfDay;
      int mins = newTime.Minutes;
      int quarters = mins / 15;
      mins = quarters * 15;
      return new TimeSpan(newTime.Hours, mins, 0);
    }
    private void updateEstimatedHours()
    {
      if (_estimateHoursChanged)
        if (!string.IsNullOrEmpty(_resource) && !string.IsNullOrEmpty(_customer) && !string.IsNullOrEmpty(_project) 
          && !string.IsNullOrEmpty(_subProject) && !string.IsNullOrEmpty(txtEstimatedHoursToComplete.Text))
            _dataSource.updateEstimatedHoursToComplete(_resource, _customer, _project, _subProject, 
              CommonFunctions.CDecimal(txtEstimatedHoursToComplete.Text, 0), SecurityContext.User);
      _estimateHoursChanged = false;
    }
    private void flagTime(bool paid)
    {
      if (!string.IsNullOrEmpty(txtPaidResource.Text))
        _dataSource.flagTime(txtPaidResource.Text, string.Empty, true, false, false, CommonData.PastDateTime, dtPaidOn.Value, false, paid, paid);
      else
        MessageBox.Show("Resource has not been selected");
    }
    private void loadResourceTime(bool resetDate)
    {
      if (resetDate)
        dtPaidOn.Value = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek - 1); // last pay period ended the week before
      srchResourceTime.InnerWhere = string.Format("'{0}', {1}", dtPaidOn.Value.ToShortDateString(), ckIncludePaidTime.Checked ? "1" : "0");
      srchResourceTime.ReLoad();
    }
    private TimeSpan offsetTime(TimeSpan time, bool toLocal)
    {
      if (time.Equals(new TimeSpan(0,0,0)))
        return time;
      if (toLocal)
        return time.Add(_hoursOffset);
      else
        return time.Add(_hoursOffsetMinus);
    }
    private string convertMinutes(string sTime, bool toMinutes)
    {
      if (toMinutes) // convert to minutes
      {
        string sHours = string.Empty;
        string sMinutes = string.Empty;
        string sDecimal = string.Empty;
        int pos = sTime.IndexOf(":");
        if (pos >= 0)   // if it has a colon
        {
          sHours = sTime.Substring(0, pos); // then divide into hours a minutes
          sMinutes = sTime.Substring(pos + 1, sTime.Length - pos - 1);
        }
        else
        {
          pos = sTime.IndexOf(".");
          if (pos >= 0) // if it has a decimal
          {
            sHours = sTime.Substring(0, pos); // then divide into hours and decimals
            sDecimal = sTime.Substring(pos + 1, sTime.Length - pos - 1);
          }
          else
            sMinutes = sTime; // otherwise we assume it is minutes like it used to be
        }
        int minutes = CommonFunctions.CInt(sHours) * 60 + CommonFunctions.CInt(sMinutes) + CommonFunctions.CInt(Math.Round(CommonFunctions.CDecimal(sDecimal) * (decimal)60));
        return minutes.ToString();
      }
      else
      {
        // in this case we have minutes coming in
        decimal hours = Math.Round(CommonFunctions.CDecimal(sTime) / (decimal)60, 2);
        string s = hours.ToString();
        if (s.StartsWith("."))
          s = "0" + s;
        int pos = s.IndexOf(".");
        string sHours = string.Empty;
        string sMinutes = string.Empty;
        if (pos >= 0) // if it has a decimal
        {
          sHours = s.Substring(0, pos); // then divide into hours and decimals
          string sDecimal = s.Substring(pos + 1, s.Length - pos - 1);
          if (sDecimal.Length == 1)
            sDecimal = sDecimal + "0";
          sMinutes = (Convert.ToDecimal(sDecimal) / (decimal)100 * (decimal)60).ToString();
          
        }
        else
          sMinutes = (CommonFunctions.CDecimal(sTime) / (decimal)60 * (decimal)100).ToString(); // otherwise we assume it is minutes so we convert to hours
        int iHours = CommonFunctions.CInt(sHours);
        int iMinutes = CommonFunctions.CInt(sMinutes);
        string sResult = string.Format("{0}:{1}", iHours.ToString(), iMinutes.ToString("00"));
        return sResult;
      }
    }
    #endregion


  }
}
