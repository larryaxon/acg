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

using ChargifyNET;

namespace CCI.DesktopClient.Screens
{
  public partial class frmCityHostedProcess : ScreenBase
  {
    #region constants
    private const string STEPNAME = "btnStep";
    private const string PROCESSEDNAME = "ckProcessed";
    private const string PROCESSEDBYNAME = "txtProcessedBy";
    private const string PROCESSEDDATETIMENAME = "txtProcessedDateTime";
    private const string CYCLE = "CHSFinancials";
    private const string PREFIX = "proc";
    private const string PROCESSERROR = "Could not process this step, message = '{0}'";
    private const int _top = 30;
    private const int _left = 40;
    private const int _height = 30;
    private const int _space = 5;

    #endregion
    #region private classes
    private class ProcessStep
    {
      public int Sequence = 0;
      public string Step = string.Empty;
      public string Description = string.Empty;
      public string Form = string.Empty;
      public string FormParameters = string.Empty;
      public DateTime? BillingDate = null;
      public string ProcessedBy = string.Empty;
      public DateTime? ProcessedDateTime = null;
      public bool IsProcessed { get { return (ProcessedDateTime != null); } set { if (value) ProcessedDateTime = DateTime.Now; else ProcessedDateTime = null; } }
    }

    #endregion


    #region private module data
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    private DateTime _billDate { get { return dtBillDate.Value; } set { dtBillDate.Value = value; } }
    private MainForm _main;
    private Dictionary<int, ProcessStep> _steps = new Dictionary<int, ProcessStep>();
    private bool _firstTime = true;
    private bool _refreshingSteps = false;
    private bool _canAccessChargify { get { return SecurityContext.Security.HasObjectAccess("CanAccessChargify"); } }
    #endregion

    public frmCityHostedProcess()
    {
      InitializeComponent();
    }

    #region form events
    private void frmCityHostedProcess_Load(object sender, EventArgs e)
    {
      _main = CommonFormFunctions.getMainForm(this);
      _billDate = _dataSource.getCurrentBillingMonth();
      srchProcessLog.Init(CommonData.UnmatchedNameTypes.ProcessLog, "Process Log");
      if (!srchProcessLog.HiddenColumns.ContainsKey("BillingDate"))
        srchProcessLog.HiddenColumns.Add("BillingDate", null);
      refreshSteps();
      reloadLog();
    }
    private void processed_CheckChanged(object sender, EventArgs e)
    {
      if (!_refreshingSteps)
        checkOffProcess(sender, ((CheckBox)sender).Checked);
    }
    private void step_Click(object sender, EventArgs e)
    {
      processStep(sender);
    }
    private void dtBillDate_ValueChanged(object sender, EventArgs e)
    {

    }
    private void btnRefresh_Click(object sender, EventArgs e)
    {
      refreshSteps();
      reloadLog();
    }
    private void panelProcess_SizeChanged(object sender, EventArgs e)
    {
      setPanelScrollRegion();
    }

    #endregion

    #region private methods
    private void checkOffProcess(object sender, bool isChecked)
    {
      ProcessStep step = getStep(sender);
      if (_billDate == null)
        MessageBox.Show("You must have a valid Bill Date");
      else
      {
        _dataSource.processStep(step.Step, _billDate, isChecked, SecurityContext.User);
        refreshSteps();
        reloadLog();
      }
    }
    private void refreshSteps()
    {
      #region get process steps data
      _refreshingSteps = true;
      DataSet dsSteps = _dataSource.getProcessSteps(CYCLE, _billDate, _canAccessChargify); // temporarily include cycle with an "x", too, which are test steps
      int lastNumberSteps = _steps.Count;
      _steps.Clear();
      if (dsSteps != null && dsSteps.Tables.Count > 0)
      {
        int iRow = 0;
        if (!_firstTime && lastNumberSteps != dsSteps.Tables[0].Rows.Count) // somebody changed the steps since the last time we refreshed
          _firstTime = true;
        foreach (DataRow row in dsSteps.Tables[0].Rows)
        {
          ProcessStep step = new ProcessStep();
          step.Sequence = CommonFunctions.CInt(row["Sequence"]);
          step.Step = CommonFunctions.CString(row["Step"]);
          step.Description = CommonFunctions.CString(row["Description"]);
          step.Form = CommonFunctions.CString(row["Form"]);
          step.FormParameters = CommonFunctions.CString(row["FormParameters"]);
          object oDate = row["ProcessedDateTime"];
          step.ProcessedDateTime = (oDate == null || oDate.Equals(System.DBNull.Value)) ? (DateTime?)null : CommonFunctions.CDateTime(oDate);
          step.ProcessedBy = CommonFunctions.CString(row["ProcessedBy"]);
          oDate = row["BillingDate"];
          step.BillingDate = (oDate == null || oDate.Equals(System.DBNull.Value)) ? (DateTime?)null : CommonFunctions.CDateTime(oDate);
          _steps.Add(iRow++, step);
        }
      }
      dsSteps.Clear();
      dsSteps = null;
      #endregion
      setPanelScrollRegion();
      if (_firstTime)
        panelProcess.Controls.Clear(); // just in case this is not the first time
      for (int line = 0; line < _steps.Count; line++ ) // for each step
      {
        string ckName = controlName(PROCESSEDNAME, line);
        string t1Name = controlName(PROCESSEDBYNAME, line);
        string t2Name = controlName(PROCESSEDDATETIMENAME, line);
        ProcessStep step = _steps[line];
        if (_firstTime)
        {
          #region create controls
          Button btn = new Button();
          btn.Name = controlName(STEPNAME, line);
          btn.Left = _left;
          btn.Top = _top + (line * _height);
          btn.Text = step.Description;
          btn.Width = 250;
          btn.Click += step_Click;
          panelProcess.Controls.Add(btn);
          CheckBox ck = new CheckBox();
          ck.Name = ckName;
          ck.Left = btn.Left + btn.Width + _space;
          ck.Top = _top + line * _height;
          ck.Text = "Processed by";
          ck.CheckedChanged += processed_CheckChanged;
          panelProcess.Controls.Add(ck);
          TextBox t1 = new TextBox();
          t1.Name = t1Name;
          t1.Top = _top + (line * _height);
          t1.Left = ck.Left + ck.Width + _space;
          t1.BorderStyle = BorderStyle.None;
          t1.Width = 75;
          t1.Enabled = false;
          panelProcess.Controls.Add(t1);
          TextBox t2 = new TextBox();
          t2.Name = t2Name;
          t2.Top = _top + (line * _height);
          t2.Left = t1.Left + t1.Width + _space;
          t2.BorderStyle = BorderStyle.None;
          t2.Width = 150;
          t2.Enabled = false;
          panelProcess.Controls.Add(t2);
          #endregion
        }
        // set values
        ((CheckBox)panelProcess.Controls[ckName]).Checked = step.IsProcessed;
        ((TextBox)panelProcess.Controls[t1Name]).Text = step.ProcessedBy;
        ((TextBox)panelProcess.Controls[t2Name]).Text = step.ProcessedDateTime.ToString();

      }
      _firstTime = false;
      _refreshingSteps = false;
    }
    private string controlName(string name, int seq)
    {
      return string.Format("{0}{1}_{2}", PREFIX, name, seq.ToString());
    }    
    private ProcessStep getStep(object sender)
    {
      string name;
      int seq;
      if (sender == null)
        return null;
      if (sender.GetType() == typeof(string))
      {
        name = sender.ToString();
        seq = -1;
        for (int i = 0; i < _steps.Count; i++)
          if (_steps[i].Form.Equals(name, StringComparison.CurrentCultureIgnoreCase))
          {
            seq = i;
            break;
          }
      }
      else
      {
        name = ((Control)sender).Name;
        int seqLoc = name.IndexOf("_") + 1;
        seq = CommonFunctions.CInt(name.Substring(seqLoc));
      }
      if (seq == -1)
        return null;
      else
        return _steps[seq];
    }
    private ScreenBase makeForm(string formName, string folder)
    {
      try
      {
        Assembly asm = Assembly.GetEntryAssembly();
        string path = string.Format("CCI.DesktopClient.{0}.", folder);
        string formname = path + formName;
        Type formtype = asm.GetType(formname);
        return (ScreenBase)Activator.CreateInstance(formtype);
      }
      catch (Exception e)
      {
        return null;
      }
    }
    private void processStep(object sender)
    {
      ProcessStep step = getStep(sender);
      string folder = "Screens";
      if (step.Form.StartsWith("frm")) // step is a form
      {

        ScreenBase frm = makeForm(step.Form, folder);
        if (frm == null)
          MessageBox.Show(step.Form + " does not exist");
        else
        {
          frm.SecurityContext = SecurityContext; // even tho ShowForm will set this, some of the inits in some of the forms need it before the form is displayed
          bool showForm = true;
          bool singleInstance = true;
          if (frm.Name.Equals("frmUnmatchedNames", StringComparison.CurrentCultureIgnoreCase))
          {
            CommonData.UnmatchedNameTypes nameType = getNameType(step.FormParameters);
            switch (nameType)
            {
              case CommonData.UnmatchedNameTypes.None:
                showForm = false;
                MessageBox.Show(string.Format("{0} is not a valid UnmatchedNames type", step.FormParameters));
                break;
              case CommonData.UnmatchedNameTypes.CityHostedCustomer:
                ((frmUnmatchedNames)frm).NameType = nameType;
                ((frmUnmatchedNames)frm).ColumnName = "Customer";
                break;
              case CommonData.UnmatchedNameTypes.CityHostedUnmatchedBTNs:
                ((frmUnmatchedNames)frm).NameType = nameType;
                ((frmUnmatchedNames)frm).ColumnName = "[Retail BTN]";
                break;
              case CommonData.UnmatchedNameTypes.CityHostedUnmatchedBTNsWholesale:
                ((frmUnmatchedNames)frm).NameType = nameType;
                ((frmUnmatchedNames)frm).ColumnName = "[Wholesale BTN]";
                break;
            }
          }
          else if (frm.Name.Equals("frmMRCNetworkInventoryMatching", StringComparison.CurrentCultureIgnoreCase))
          {
            singleInstance = false;
            bool isRetail = step.FormParameters.Equals("Retail", StringComparison.CurrentCultureIgnoreCase);
            ((frmMRCNetworkInventoryMatching)frm).IsRetail = isRetail;
          }
          else if (frm.Name.Equals("frmImports", StringComparison.CurrentCultureIgnoreCase))
          {
            singleInstance = true;
            ((frmImports)frm).Source = step.FormParameters; // parameters tell where to import from
          }
          if (showForm)
            _main.ShowForm(frm, singleInstance);
        }
      }
      else
      {
        // step is a process
        string stat = string.Empty;
        DialogResult ans = MessageBox.Show(string.Format("You are about to process the {0} Step. Are you sure you want to proceed?", step.Description), "Process a Step", MessageBoxButtons.YesNo);
        if (ans == DialogResult.Yes)
        {
          switch (step.Form.ToLower())
          {
            case "qbcustomersexport":
              stat = processQBCustomers();
              if (stat.Equals("Ready"))
                checkOffProcess(step.Form, true);
              else
                MessageBox.Show(string.Format(PROCESSERROR, stat));
              break;
            case "qbinvoiceexport":
              stat = processQBInvoiceExport();
              if (stat.Equals("Ready"))
                checkOffProcess(step.Form, true);
              else
                MessageBox.Show(string.Format(PROCESSERROR, stat));
              break;
            case "qbcashexport":
              stat = processQBCashExport();
              if (stat.Equals("Ready"))
                checkOffProcess(step.Form, true);
              else
                MessageBox.Show(string.Format(PROCESSERROR, stat));
              break;
            case "qbtaxexport":
              stat = processQBTaxes();
              if (stat.Equals("Ready"))
                checkOffProcess(step.Form, true);
              else
                MessageBox.Show(string.Format(PROCESSERROR, stat));
              break;
            case "checkoffpriormonth":
              stat = processCheckOffPriorMonth();
              if (stat.Equals("Ready"))
                checkOffProcess(step.Form, true);
              else
                MessageBox.Show(string.Format(PROCESSERROR, stat));
              break;
            case "exportdealermargin":
              stat = processExportDealerMargin();
              if (stat.Equals("Ready"))
                checkOffProcess(step.Form, true);
              else
                MessageBox.Show(string.Format(PROCESSERROR, stat));
              break;
            case "postinvoices":
              stat = processPostInvoices();
              if (stat.Equals("Ready"))
                checkOffProcess(step.Form, true);
              else
                MessageBox.Show(string.Format(PROCESSERROR, stat));
              break;
            case "unpostinvoices":
              stat = processUnpostInvoices();
              if (stat.Equals("Ready"))
                checkOffProcess(step.Form, true);
              else
                MessageBox.Show(string.Format(PROCESSERROR, stat));
              break;
            case "resetallimports":
              stat = processResetAll();
              if (!stat.Equals("Ready"))
                MessageBox.Show(string.Format(PROCESSERROR, stat));
              break;
            case "postcashreceipts":
              stat = processPostCashReceipts();
              if (stat.Equals("Ready"))
                checkOffProcess(step.Form, true);
              else
                MessageBox.Show(string.Format(PROCESSERROR, stat));
              break;
            case "freezedealerinvoice":
              stat = processFreezeDealerInvoice();
              if (stat.Equals("Ready"))
                checkOffProcess(step.Form, true);
              else
                MessageBox.Show(string.Format(PROCESSERROR, stat));
              break;
            case "unfreezedealerinvoice":
              stat = unprocessFreezeDealerInvoice();
              if (stat.Equals("Ready"))
                checkOffProcess(step.Form, true);
              else
                MessageBox.Show(string.Format(PROCESSERROR, stat));
              break;
            case "chargifypost":
              stat = processChargifyPost();
              if (stat.Equals("Ready"))
                checkOffProcess(step.Form, true);
              else
                MessageBox.Show(string.Format(PROCESSERROR, stat));
              break;
            case "chargifyunpost":
              stat = processChargifyUnPost();
              if (stat.Equals("Ready"))
                checkOffProcess(step.Form, true);
              else
                MessageBox.Show(string.Format(PROCESSERROR, stat));
              break;
            case "chargifyupdatestatus":
              stat = chargifyUpdateCustomerStatus();
              if (stat.Equals("Ready"))
                checkOffProcess(step.Form, true);
              else
                MessageBox.Show(string.Format(PROCESSERROR, stat));
              break;
            case "clearinvoicesforperiod":
              stat = processClearInvoices();
              if (stat.Equals("Ready"))
                checkOffProcess(step.Form, true);
              else
                MessageBox.Show(string.Format(PROCESSERROR, stat));
              break;
          }
        }
      }
    }
    private CommonData.UnmatchedNameTypes getNameType(string type)
    {
      try
      {
        return (CommonData.UnmatchedNameTypes)Enum.Parse(typeof(CommonData.UnmatchedNameTypes), type, true);
      }
      catch
      {
        return CommonData.UnmatchedNameTypes.None;
      }
    }
    private void setPanelScrollRegion()
    {
      panelProcess.AutoScrollMinSize = new System.Drawing.Size(panelProcess.Size.Width, _top + (_steps.Count * _height));
    }
    #region process QuickBooks steps
    private string processQBCustomers()
    {
      lblProcessStatus.Text = "Processing Quickboks New Customers Export...";
      lblProcessStatus.Visible = true;
      DataSet ds = _dataSource.getNewCustomersDataset();
      _dataSource.exportNewCustomerstoCSV(ds);
      _dataSource.updateNewCustomersExported(ds);
      lblProcessStatus.Visible = false;
      return "Ready";
    }
    private string processQBTaxes()
    {
      lblProcessStatus.Text = "Processing Quickboks Taxes Export...";
      lblProcessStatus.Visible = true;
      DataSet ds = _dataSource.getTaxesDataset(_billDate);
      _dataSource.exportTaxestoCSV(ds);
 //     _dataSource.updateTaxesExported(_billDate);
      lblProcessStatus.Visible = false;
      return "Ready";
    }
    private string processQBCashExport()
    {
      lblProcessStatus.Text = "Processing Quickboks Cash Receipts Export...";
      lblProcessStatus.Visible = true;
      DataSet ds = _dataSource.getCashReceiptsDataset(_billDate);
      _dataSource.exportCashReceiptsToCSV(ds);
      _dataSource.updateCashReceiptsExported(_billDate);
      lblProcessStatus.Visible = false;
      return "Ready";
    }
    private string processQBInvoiceExport()
    {
      lblProcessStatus.Text = "Processing Quickboks Invoice Export...";
      lblProcessStatus.Visible = true;
      string filetype = "QB Export";
      string user = SecurityContext.User;
      string validexportmsg = ValidExport(_billDate, filetype);
      if (validexportmsg == "Ready")
      {

          Cursor.Current = Cursors.WaitCursor;
          DataSet ds = _dataSource.getBillingDataset(_billDate);
          string fileName = _dataSource.exportToExcel(ds);
          if (string.IsNullOrEmpty(fileName))
            MessageBox.Show("Export was not successful");
          else
          _dataSource.insertAcctImportLog(user, filetype, fileName, _billDate);
          lblProcessStatus.Visible = false;
      }
      

      Cursor.Current = Cursors.Default;
      return validexportmsg;
    }
    #endregion
    #region process Chargify Steps

    private string processChargifyPost()
    {
      return chargifyPost(false);
    }
    private string processChargifyUnPost()
    {
      return chargifyPost(true);
    }
    private string chargifyPost(bool reverseCharge)
    {
      Chargify chargify = new Chargify();
      string prefix = reverseCharge ? "Un" : string.Empty;
      string validexportmsg = "Ready";
      string errormessage = string.Format("Error Processing {0}Post to Chargify: <{{0}}>", prefix);
      if (!_dataSource.invoicesArePosted(_billDate))
        validexportmsg = "Invoices have not yet been posted for this period - cannot post to Chargify";
      else
      {
        lblProcessStatus.Text = "Processing Post to Chargify...";
        lblProcessStatus.Visible = true;

        Cursor.Current = Cursors.WaitCursor;
        try
        {
          DateTime transactionDateTime = DateTime.Now;
          DataSet ds = _dataSource.getPostedInvoicesForChargify(_billDate);
          if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            validexportmsg = string.Format(errormessage, string.Format("No invoices were found to {0}Post", prefix));
          else
          {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
              int subscriptionid = CommonFunctions.CInt(row["ChargifySubscription"]);
              if (subscriptionid > 0)
              {
                decimal amount = CommonFunctions.CDecimal(row["AmountDue"]);
                if (amount != 0)
                {
                  string customerID = CommonFunctions.CString(row["CustomerID"]);
                  if (reverseCharge)
                    amount = -amount;
                  chargify.createMonthlyCharge(subscriptionid, amount, "Balance Due");
                  validexportmsg = _dataSource.automaticChargifyCashPayment(_billDate, customerID, amount, transactionDateTime, SecurityContext.Security.UserLogin, "Chargify Auto Payment");
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          validexportmsg = string.Format(errormessage, CommonFunctions.getInnerException(ex).Message);
        }
        lblProcessStatus.Visible = false;

        Cursor.Current = Cursors.Default;
      }
      return validexportmsg;
    }
    private string chargifyUpdateCustomerStatus()
    {
      Chargify chargify = new Chargify();
      string validexportmsg = "Ready";
      string errormessage = string.Format("Error Processing Update Chargify Customer Status");
      lblProcessStatus.Text = "Processing Post to Chargify...";
      lblProcessStatus.Visible = true;

      Cursor.Current = Cursors.WaitCursor;
      try
      {
        using (DataSet ds = _dataSource.getChargifyCustomerInfo())
        {
          if (ds == null || ds.Tables.Count == 0 | ds.Tables[0].Rows.Count == 0)
            validexportmsg = "No Chargify Customers were found to update";
          else
          {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
              int subscriptionID = CommonFunctions.CInt(row["ChargifySubscription"]);
              string customerID = CommonFunctions.CString(row["CustomerID"]);
              ISubscription subscription = chargify.getSubscription(subscriptionID);
              SubscriptionState status = subscription.State;
              decimal balance = subscription.Balance;
              switch (status)
              {
                case SubscriptionState.Canceled:
                case SubscriptionState.Suspended:
                case SubscriptionState.Active:
                  break; // we assume success, so if they are not past due or unpaid we do nothing
                case SubscriptionState.Past_Due:
                  break; // for now, we do not do anything until the dunning process is complete and they become unpaid
                case SubscriptionState.Unpaid:
                  // this one means the payment did not go through and the dunning failed.
                  validexportmsg = reverseUnpaidInvoiceCash(customerID, balance);
                  break;
              }

            }
          }
        }
      }
      catch (Exception ex)
      {
        validexportmsg = string.Format(errormessage, CommonFunctions.getInnerException(ex).Message);
      }
      lblProcessStatus.Visible = false;

      Cursor.Current = Cursors.Default;
      return validexportmsg;
    }
    private string reverseUnpaidInvoiceCash(string customerID, decimal balance)
    {
      // find the last payment generated by the chargify post and create a reversnig entry.
      string validexportmsg = "Ready";
      using (DataSet ds = _dataSource.getLastChargifyCashReceipt(customerID))
      {
        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
          return "No prior invoice exists for " + customerID;
        DataRow row = ds.Tables[0].Rows[0];
        DateTime billDate = CommonFunctions.CDateTime(row["BillDate"]);
        decimal amount = CommonFunctions.CDecimal(row["Amount"]);
        if (balance != amount) // subscription balance does not match last auto payment
          return string.Format("Unpaid Subscription Balance {1} does not match last Chargify Autopay {2} for this customer {0}", customerID, balance, amount);
        // same as payment but amount is reversed and comment is different
        validexportmsg = _dataSource.automaticChargifyCashPayment(_billDate, customerID, -amount, DateTime.Now, SecurityContext.Security.UserLogin, "Reverse Unpaid Chargify Payment");
      }
      return validexportmsg;
    }

    #endregion
    private string processExportDealerMargin()
    {
      lblProcessStatus.Text = "Processing Export Dealer Margin...";
      lblProcessStatus.Visible = true;
      string validexportmsg = "Ready";

      Cursor.Current = Cursors.WaitCursor;
      DataSet ds = _dataSource.getDealerMarginReport("All", _billDate);
      CommonFormFunctions.exportToExcel(ds);
      ds.Clear();
      ds = null;
      lblProcessStatus.Visible = false;

      Cursor.Current = Cursors.Default;
      return validexportmsg;
    }
    private string processPostCashReceipts()
    {
      lblProcessStatus.Text = "Processing Post Cash Receipts...";
      lblProcessStatus.Visible = true;
      string validexportmsg = "Ready";

      Cursor.Current = Cursors.WaitCursor;
      DateTime? throughDate = _dataSource.getMostRecentCashReceiptDate();
      if (throughDate != null)
        _dataSource.applyCashPost((DateTime)throughDate);

      lblProcessStatus.Visible = false;

      Cursor.Current = Cursors.Default;
      return validexportmsg;
    }
    private string processPostInvoices()
    {
      string validexportmsg = "Ready";
      if (_dataSource.invoicesArePosted(_billDate))
        validexportmsg = "Invoices have already been posted for this period";
      else
      {
        lblProcessStatus.Text = "Processing Post Invoices...";
        lblProcessStatus.Visible = true;

        Cursor.Current = Cursors.WaitCursor;
        Exception returnmsg = _dataSource.execPostInvoices(_billDate);
        if (returnmsg != null)
          validexportmsg = string.Format("Error Processing Invoice Post: <{0}>", CommonFunctions.getInnerException(returnmsg).Message);
        lblProcessStatus.Visible = false;

        Cursor.Current = Cursors.Default;
      }
      return validexportmsg;
    }
    private string processUnpostInvoices()
    {
      string validexportmsg = "Ready";
      if (!_dataSource.invoicesArePosted(_billDate))
        validexportmsg = "There are no invocies to unpost for this period";
      else
      {
        lblProcessStatus.Text = "Processing Unpost Invoices...";
        lblProcessStatus.Visible = true;

        Cursor.Current = Cursors.WaitCursor;
        Exception returnmsg = _dataSource.execUnpostInvoices(_billDate);
        if (returnmsg != null)
          validexportmsg = string.Format("Error Processing Invoice Unpost: <{0}>", CommonFunctions.getInnerException(returnmsg).Message);
        lblProcessStatus.Visible = false;

        Cursor.Current = Cursors.Default;
      }
      return validexportmsg;
    }
    
    private string processClearInvoices()
    {
      string validexportmsg = "Ready";
      DialogResult ans = MessageBox.Show("Reset Invoice data for this period. Are you sure you want to do this? This will delete all imported and modified invoice data for this period.",
        "Reset All Invoice Data for this Period", MessageBoxButtons.YesNo);
      if (ans == DialogResult.No)
        validexportmsg = "Reset was not performed";
      else
      {
        lblProcessStatus.Text = "Processing Reset Invoice Data...";
        lblProcessStatus.Visible = true;

        Cursor.Current = Cursors.WaitCursor;
        Exception returnmsg = _dataSource.execResetInvoices(_billDate);
        if (returnmsg != null)
          validexportmsg = string.Format("Error Processing Reset All: <{0}>", CommonFunctions.getInnerException(returnmsg).Message);
        lblProcessStatus.Visible = false;

        Cursor.Current = Cursors.Default;
      }
      return validexportmsg;
    }
    private string processResetAll()
    {
      string validexportmsg = "Ready";
      DialogResult ans = MessageBox.Show("Reset ALL data for this period. Are you sure you want to do this? This will delete all imported and posted data for this period and all activity logs.",
        "Reset All Data for this Period", MessageBoxButtons.YesNo);
      if (ans == DialogResult.No)
        validexportmsg = "Reset was not performed";
      else
      {
        lblProcessStatus.Text = "Processing Reset All...";
        lblProcessStatus.Visible = true;

        Cursor.Current = Cursors.WaitCursor;
        Exception returnmsg = _dataSource.execResetAll(_billDate);
        if (returnmsg != null)
          validexportmsg = string.Format("Error Processing Reset All: <{0}>", CommonFunctions.getInnerException(returnmsg).Message);
        lblProcessStatus.Visible = false;

        Cursor.Current = Cursors.Default;
      }
      return validexportmsg;
    }
    private string processFreezeDealerInvoice()
    {
      string validexportmsg = "Ready";
      string dealer = string.Empty;
      DialogResult ans = ACG.CommonForms.CommonFormFunctions.InputBox("Enter Dealer", "Enter Dealer", ref dealer, true, new SearchDataSourceEntity("Dealer"), true);
      if (ans == System.Windows.Forms.DialogResult.Cancel)
        return "Cancelled";
      if (_dataSource.hasDealerFrozenInvoice(dealer, _billDate))
        validexportmsg = "This dealer already has a frozen invoice for this period";
      else
      {
        lblProcessStatus.Text = "Processing Freeze Dealer Invoice...";
        lblProcessStatus.Visible = true;

        Cursor.Current = Cursors.WaitCursor;
        Exception returnmsg = _dataSource.processFreezeDealerInvoice(dealer, _billDate);
        if (returnmsg != null)
          validexportmsg = string.Format("Error Processing Freeze Dealer Invoice: <{0}>", CommonFunctions.getInnerException(returnmsg).Message);
        else if (!_dataSource.getDealerFrozenInvoiceState(_billDate).Equals("Posted",StringComparison.CurrentCultureIgnoreCase)) 
          validexportmsg = "Not All Dealers have been Posted";
        lblProcessStatus.Visible = false;

        Cursor.Current = Cursors.Default;
      }
      return validexportmsg;
    }
    private string unprocessFreezeDealerInvoice()
    {
      string validexportmsg = "Ready";
      string dealer = string.Empty;
      DialogResult ans = ACG.CommonForms.CommonFormFunctions.InputBox("Enter Dealer", "Enter Dealer", ref dealer, true, new SearchDataSourceEntity("Dealer"),true);
      if (ans == System.Windows.Forms.DialogResult.Cancel)
        return "Cancelled";
      if (!_dataSource.hasDealerFrozenInvoice(dealer, _billDate))
        validexportmsg = "This dealer does not have a frozen invoice for this period";
      else
      {
        lblProcessStatus.Text = "Processing Unfreeze Dealer Invoice...";
        lblProcessStatus.Visible = true;

        Cursor.Current = Cursors.WaitCursor;
        Exception returnmsg = _dataSource.unprocessFreezeDealerInvoice(dealer, _billDate);
        if (returnmsg != null)
          validexportmsg = string.Format("Error Processing Unfreeze Dealer Invoice: <{0}>", CommonFunctions.getInnerException(returnmsg).Message);
        else if (!_dataSource.getDealerFrozenInvoiceState(_billDate).Equals("UnPosted",StringComparison.CurrentCultureIgnoreCase)) 
          validexportmsg = "Not All Dealers have been unposted";
        lblProcessStatus.Visible = false;

        Cursor.Current = Cursors.Default;
      }
      return validexportmsg;
    }
    private string processCheckOffPriorMonth()
    {
      lblProcessStatus.Text = "Processing Check Off Prior Month...";
      lblProcessStatus.Visible = true;
      string validexportmsg = "Ready";

      Cursor.Current = Cursors.WaitCursor;
      _dataSource.flagPastHostedTransactions(_billDate);
      lblProcessStatus.Visible = false;

      Cursor.Current = Cursors.Default;
      return validexportmsg;
    }
    private string ValidExport(DateTime billdate, string step)
    {
      bool logexists;
      logexists = _dataSource.checkProcessStepsbeforeimport(billdate, step, null);
      if (logexists == true)
      {
        return "Already Exported.";
      }
      logexists = _dataSource.checkProcessStepsbeforeimport(billdate, "Posted", null);
      if (logexists == false)
      {
        return "Need to Post first.";
      }
      return "Ready";
    }
    private void reloadLog()
    {
      Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
      criteria.Add("BillingDate", new string[] { ctlSearchGrid.opEQUALS, _billDate.ToShortDateString() });
      srchProcessLog.SearchCriteria = criteria;
      srchProcessLog.ReLoad();
    }
    #endregion
  }
}
