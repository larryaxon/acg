using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Forms;

using CCI.Common;
using CCI.Sys.Data;
using CCI.DesktopClient.Common;

using TAGBOSS.Common;
using TAGBOSS.Common.Model;

namespace CCI.DesktopClient.Screens
{
  public partial class frmCashReceipts : ScreenBase
  {
    private DataSource _dSource = null;
    EntityAttributes _entityAttributes = null;
    protected EntityAttributesCollection _eac = new EntityAttributesCollection();
    private EntityAttributes _ea { get { if (_entityAttributes == null) _entityAttributes = new EntityAttributes(); return _entityAttributes; } }
    protected DataSource _dataSource { get { if (_dSource == null) _dSource = new DataSource(); return _dSource; } }
    private const string COLUMNPAYCHECKBOX = "Pay";
    private const string COLUMNPAYAMOUNT = "PaidAmount";
    private const string COLUMNNONSBAMOUNT = "NonSaddlebackAmount";
    private const string COLUMNSTATUS = "Status";
    private const string COLUMNCHECKNUMBER = "CheckNumber";
    private const string COLUMNDATERECEIVED = "TransactionDate";
    private const string COLUMNAMOUNT = "Balance Owed";
    private const string COLUMNBILL = "Current Bill";
    private const string COLUMNID = "ID";
    private const string STATUSEXPORTED = "Exported";
    private string _rootPath = "C:\\Data\\Saddleback Imports";
    private bool _fromDetail = false;
    private DateTime _startDate = (new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(-1).Month, 1));
    private DateTime _endDate = DateTime.Today;
    public frmCashReceipts()
    {
      InitializeComponent();
      srchCashReceipts.RowCheckColumns.Add(COLUMNPAYCHECKBOX, null);
      srchCashReceipts.RowCheckColumns.Add(COLUMNPAYAMOUNT, null);
      srchCashReceipts.RowCheckColumns.Add(COLUMNCHECKNUMBER, null);
      srchCashReceipts.RowCheckColumns.Add(COLUMNNONSBAMOUNT, null);
      srchCashReceipts.NumericFormattedColumns.Add(COLUMNAMOUNT, null);
      srchCashReceipts.NumericFormattedColumns.Add(COLUMNBILL, null);
      srchCashReceipts.DoNotSortColumns.Add(COLUMNAMOUNT, null);
      srchCashReceipts.DoNotSortColumns.Add(COLUMNBILL, null);
      srchCashReceipts.HiddenColumns.Add("Comment", null);
      srchCashDetail.Init(CommonData.UnmatchedNameTypes.CashReceiptsDetail, "Cash Detail");
      srchCashDetail.HiddenColumns.Add("CustomerID", null);
      cboCustomer.SearchExec = new SearchDataSourceCustomer();
    }
    public void Init()
    {
      //loadCriteria();
      reloadCashReceipts(false);
      refreshTotals();
      loadCustomerID();
      ClearFields(true);
    }

    #region private methods
    private void loadCriteria()
    {
      Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
   
      if (!(tbPosted.Checked && tbUnPosted.Checked))  // if both, we don't filter
        criteria.Add(STATUSEXPORTED, new string[] { ctlSearchGrid.opEQUALS, tbPosted.Checked ? "Yes" : "No" });
      if (tbUnpaidOnly.Checked)
        criteria.Add("Pay", new string[] { ctlSearchGrid.opEQUALS, "0" });
      if (activeInventoryOnlyToolStripMenuItem.Checked)
        criteria.Add("HasActiveInventory", new string[] { ctlSearchGrid.opEQUALS, "Yes" });
      if (noCreditBalancesToolStripMenuItem.Checked)
        criteria.Add("Balance Owed", new string[] { ctlSearchGrid.opGREATEROREQUAL, "0" });
      DateTime endDate = _endDate;
      DateTime billDate = endDate.AddDays(1 - endDate.Day);
      srchCashReceipts.Parameters.Clear();
      srchCashReceipts.Parameters.Add("FromDate", _startDate.ToShortDateString());
      srchCashReceipts.Parameters.Add("ToDate", _endDate.ToShortDateString());
      srchCashReceipts.Parameters.Add("BillDate", billDate.ToShortDateString());
      srchCashReceipts.SearchCriteria = criteria;
    }
    private void refreshTotals()
    {
      Dictionary<string, decimal> totals = srchCashReceipts.getTotals(new string[] { "paidamount" });
      tbTotalPaid.Text = totals["paidamount"].ToString();
      totals = srchCashReceipts.getTotals(new string[] { "balance owed" });
      tbAmountDue.Text = totals["balance owed"].ToString();
    }
    private void ClearFields(bool clearDetail)
    {
      this.txtID.Text = null;
      this.txtAmount.Text = null;
      this.txtCheckNbr.Text = null;
      this.txtCheckNbr.Enabled = true;
      this.cboCustomer.Text = null;
      txtNonSBAmount.Text = null;
      this.chkSvcActivation.Checked = false;
//      this.dtDateReceived.Text = null;
      this.radioCC.Checked = true;
      this.lblMessage.Text = "";
      this.btnUpdate.Text = "Add";
      this.btnUpdate.Enabled = false;
      this.btnDelete.Enabled = false;
      dtDateReceived.Value = DateTime.Today;
      radioCC.Focus();
      if (clearDetail)
        srchCashDetail.Clear();
      _fromDetail = !clearDetail;
    }
    private void loadFields(Dictionary<string, object> row, bool fromDetail)
    {
      ClearFields(!fromDetail);
      double sign = fromDetail ? -1.0 : 1.0;
      string fldCustomerID, fldAmount, fldID, fldCheckNumber, fldTransactionDate, fldPaymentType, fldNonSaddlebackAmount, fldExported, fldComment;
      fldPaymentType = "PaymentType";
      fldID = "ID";
      fldTransactionDate = "TransactionDate";
      fldComment = "Comment";
      fldNonSaddlebackAmount = COLUMNNONSBAMOUNT;
      if (fromDetail)
      {
        fldCustomerID = "CustomerID";
        fldAmount = "Amount";
        fldCheckNumber = "Reference";
        fldExported = "Exported";
      }
      else
      {
        fldCustomerID = "Customer";
        fldAmount = "Balance Owed";
        fldCheckNumber = "CheckNumber";
        fldExported = STATUSEXPORTED;
      }
      string exported = CommonFunctions.CString(row[fldExported]);
      string customerID = CommonFunctions.CString(row[fldCustomerID]);

      this.txtID.Text = CommonFunctions.CString(row[fldID]);
      this.txtAmount.Text = CommonFunctions.CString(CommonFunctions.CDouble(row[fldAmount]) * sign);
      this.txtCheckNbr.Text = CommonFunctions.CString(row[fldCheckNumber]);
      this.cboCustomer.Text = customerID;

      this.txtNonSBAmount.Text = CommonFunctions.CString(CommonFunctions.CDouble(row[fldNonSaddlebackAmount]) * sign);
      txtComment.Text = CommonFunctions.CString(row[fldComment]);
      cboCustomer.Collapsed = true;
      object oDate = row[fldTransactionDate];
      if (oDate == null || oDate.Equals(System.DBNull.Value))
      {
        //dtDateReceived.Checked = false;
        dtDateReceived.Value = DateTime.Today;
      }
      else
        this.dtDateReceived.Value = CommonFunctions.CDateTime(oDate);
      string paymenttype = CommonFunctions.CString(row[fldPaymentType]);
      string transactiontype = row.ContainsKey("TransactionType") ? CommonFunctions.CString(row["TransactionType"]) : string.Empty;
      this.radioBill.Visible = false;
      if (paymenttype.Equals("CreditCard", StringComparison.CurrentCultureIgnoreCase))
      {
        this.radioCC.Checked = true;
      }
      else if (paymenttype.Equals("Check", StringComparison.CurrentCultureIgnoreCase))
      {
        this.radioCheck.Checked = true;
      }
      else if (paymenttype.Equals("ACH", StringComparison.CurrentCultureIgnoreCase)
        || paymenttype.Equals("Cash", StringComparison.CurrentCultureIgnoreCase))
      {
        this.radioACH.Checked = true;
      }
      else if (transactiontype.Equals("Bill", StringComparison.CurrentCultureIgnoreCase))
      {
        this.radioBill.Visible = true;
        this.radioBill.Checked = true;
      }
      this.btnUpdate.Text = "Save";
      this.btnUpdate.Enabled = true;
      this.btnDelete.Enabled = true;
      if (string.IsNullOrEmpty(exported) && exported.Equals("No"))  //  posted
      {
        this.btnUpdate.Text = "Locked";
        this.btnUpdate.Enabled = false;
        this.btnDelete.Enabled = false;
        MessageBox.Show("This cash transaction has been exported to Quick Books, you cannot edit it");
      }
      if (!fromDetail)
        loadDetail(customerID);
    }
    private void loadDetail(string customerID)
    {
      Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
      criteria.Add("CustomerID", new string[] { ctlSearchGrid.opEQUALS, customerID });
      criteria.Add("TransactionDate", new string[] { ctlSearchGrid.opBETWEEN, string.Format("'{0}' AND '{1}'", _startDate.ToShortDateString(), _endDate.ToShortDateString()) });
      srchCashDetail.SearchCriteria = criteria;
      srchCashDetail.ReLoad();
    }
    private void saveFields()
    {

      string customerid = Convert.ToString(this.cboCustomer.Text.Substring(0,5));
      if (string.IsNullOrEmpty(customerid))
      {
        MessageBox.Show("You cannot save a cash receipt without a valid customer");
        return;
      }
      DateTime datereceived;
      if (dtDateReceived.Checked)
        datereceived = Convert.ToDateTime(dtDateReceived.Value);
      else
      {
        MessageBox.Show("You cannot save a cash receipt without a valid Transaction Date");
        return;
      }
      string paymenttype = "";
      if (this.radioCC.Checked)
        paymenttype = "CreditCard";
      else if (this.radioCheck.Checked)
        paymenttype = "Check";
      else if (this.radioACH.Checked)
        paymenttype = "ACH";
      else if (this.radioBill.Checked)
        paymenttype = "Invoice";
      string checknbr = Convert.ToString(this.txtCheckNbr.Text);
      double amount = Convert.ToDouble(this.txtAmount.Text);
      if (paymenttype.Equals("Invoice", StringComparison.CurrentCultureIgnoreCase))
      {
        if (amount >= 0)
        {
          MessageBox.Show("You cannot save a Bill without a valid (negative) amount");
          return;
        }
      }
      else
        if (amount <= 0.0)
        {
          MessageBox.Show("You cannot save a cash receipt without a valid amount");
          return;
        }
      double nonSBamount = CommonFunctions.CDouble(this.txtNonSBAmount.Text);
      double debitamount = amount - nonSBamount;
      string comment = txtComment.Text;
      DateTime billDate = datereceived.AddDays(-datereceived.Day + 1); // first of the month of the trans date
      string User = SecurityContext.User;
      int? ret;
      if (this.btnUpdate.Text == "Add" || string.IsNullOrEmpty(txtID.Text))
      {
        ret = _dataSource.insertCashReceipt(customerid, datereceived, billDate, paymenttype, checknbr, amount, nonSBamount, User, comment);
        if (this.chkSvcActivation.Checked)
        {
          // Enter offsetting debit transaction
          ret = _dataSource.insertDebitMemo(customerid, datereceived, billDate, debitamount, User, comment);
        }
      }
      else
      {
        string rowID = this.txtID.Text;
        ret = _dataSource.updateCashReceipt(rowID, customerid, datereceived, paymenttype, checknbr, amount, nonSBamount, User, comment);
      }
      if (ret != null && ret == -1)
        MessageBox.Show("Save failed");
      else
        MessageBox.Show("Record saved");
    }
    private void deleteRecord()
    {
      string rowid = this.txtID.Text;
      _dataSource.deleteCashReceipt(rowid);
      if (this.chkSvcActivation.Checked)
      {
        // Enter offsetting debit transaction
        //ret = _dataSource.deleteDebitMemo(customerid, datereceived, billDate, amount, User, comment);
      }

    }
    private bool validateFields()
    {
      if (!this.radioCC.Checked  && !radioCheck.Checked && !radioACH.Checked && !radioBill.Checked)
      {
        return false;
      }
      if (this.cboCustomer.Text == "")
      {
        return false;
      }
      if (this.radioCheck.Checked == true && this.txtCheckNbr.Text == "")
      {
        return false;
      }
      if (this.txtAmount.Text == "")
      {
        return false;
      }
      if (radioBill.Checked)
        return Convert.ToDouble(this.txtAmount.Text) <= 0;
      
      if (Convert.ToDouble(this.txtAmount.Text) <= 0)
        return false;
      return true;
    }
    private void loadCustomerID()
    {
      
      //DataSet ds = _dataSource.GetQBCustomerList(); 
      //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
      //{
      //  this.cboCustomer.Items.Clear();
      //  foreach (DataRow row in ds.Tables[0].Rows)
      //  {
      //    this.cboCustomer.Items.Add(row[0].ToString() + " " + row[1].ToString());
      //  }
      //}
    }
    private void applyPostCashReceipts()
    {
      DateTime? throughDate = _dataSource.getMostRecentCashReceiptDate();
      if (throughDate == null)
        MessageBox.Show("There are no cash receipts to post");
      else
      {
        _dataSource.applyCashPost((DateTime)throughDate);
        MessageBox.Show("Cash Receipts have been Posted");
      }
    }
    private void testPaymentType()
    {
      if (radioACH.Checked)
      {
        lblCheckNumber.Visible = false;
        txtCheckNbr.Visible = false;
        lblCheckNumber.Text = "";
      }
      else if (radioBill.Checked)
      {
        lblCheckNumber.Visible = false;
        txtCheckNbr.Visible = false;
      }
      else
      {
        lblCheckNumber.Visible = true;
        txtCheckNbr.Visible = true;
        txtCheckNbr.Enabled = true;
        if (radioCC.Checked)
          lblCheckNumber.Text = "CC #";
        else
          lblCheckNumber.Text = "Check #";
      }
    }
    private void payThisRow(DataGridViewRow row, string columnName)
    {
      string id = CommonFunctions.CString(row.Cells["ID"].Value);
      int? iid = null;
      double amount = CommonFunctions.CDouble(row.Cells[COLUMNAMOUNT].Value);
      double paidamount = CommonFunctions.CDouble(row.Cells[COLUMNPAYAMOUNT].Value);
      double nonSBamount = CommonFunctions.CDouble(row.Cells[COLUMNNONSBAMOUNT].Value);
      string checkNumber = CommonFunctions.CString(row.Cells[COLUMNCHECKNUMBER].Value);
      string customer = CommonFunctions.CString(row.Cells["Customer"].Value);
      DateTime dateReceived = CommonFunctions.CDateTime(row.Cells["TransactionDate"].Value, DateTime.Today);
      string paymenttype = CommonFunctions.CString(row.Cells["PaymentType"].Value);
      if (columnName.Equals(COLUMNCHECKNUMBER))
      {
        if (!string.IsNullOrEmpty(id))
          _dataSource.updateCashReceipt(id, customer, dateReceived, paymenttype, checkNumber, paidamount, nonSBamount, SecurityContext.User, null);
      }
      else if (columnName.Equals(COLUMNPAYCHECKBOX)) // check to see if they paid it
      {
        bool toPay = CommonFunctions.CBoolean(row.Cells[COLUMNPAYCHECKBOX].Value);
        if (toPay) // they checked it
        {
          dateReceived = DateTime.Today;
          if (string.IsNullOrEmpty(id)) // new record
          {
            DateTime billDate = dateReceived.AddDays(-dateReceived.Day + 1); // first of the month of the trans date
            iid = _dataSource.insertCashReceipt(customer, dateReceived, billDate, paymenttype, checkNumber, amount, nonSBamount, SecurityContext.User, null);
          }
          else
            _dataSource.updateCashReceipt(id, customer, dateReceived, paymenttype, checkNumber, amount, nonSBamount, SecurityContext.User, null);

          row.Cells[COLUMNPAYAMOUNT].Value = amount;
          row.Cells[COLUMNDATERECEIVED].Value = dateReceived;
        }
        else
          if (string.IsNullOrEmpty(id))
            MessageBox.Show("You cannot unpay this item, it has not been paid");
          else
          {
            _dataSource.deleteCashReceipt(id); //delete the cash receipt
            // and blank out the cash columns so it is back to normal
            row.Cells[COLUMNPAYAMOUNT].Value = null; 
            row.Cells[COLUMNCHECKNUMBER].Value = null;
            row.Cells[COLUMNID].Value = null;
            row.Cells[COLUMNDATERECEIVED].Value = null;
          }
      }
      else if (columnName.Equals(COLUMNPAYAMOUNT))
        {
          if (string.IsNullOrEmpty(id)) // new record
          {
            DateTime billDate = dateReceived.AddDays(-dateReceived.Day + 1); // first of the month of the trans date
            iid = _dataSource.insertCashReceipt(customer, dateReceived, billDate, paymenttype, checkNumber, paidamount, nonSBamount, SecurityContext.User, null);
          }
          else
            _dataSource.updateCashReceipt(id, customer, dateReceived, paymenttype, checkNumber, paidamount, nonSBamount, SecurityContext.User, null);
          row.Cells[COLUMNPAYCHECKBOX].Value = true;
        }
      else if (COLUMNNONSBAMOUNT.Equals(COLUMNNONSBAMOUNT))
      {
        if (string.IsNullOrEmpty(id)) // new record
        {
          DateTime billDate = dateReceived.AddDays(-dateReceived.Day + 1); // first of the month of the trans date
          iid = _dataSource.insertCashReceipt(customer, dateReceived, billDate, paymenttype, checkNumber, paidamount, nonSBamount, SecurityContext.User, null);
        }
        else
          _dataSource.updateCashReceipt(id, customer, dateReceived, paymenttype, checkNumber, paidamount, nonSBamount, SecurityContext.User, null);
        row.Cells[COLUMNPAYCHECKBOX].Value = true;
      }
      if (iid != null && iid > 0)
        row.Cells[COLUMNID].Value = (int)iid;
      refreshTotals();
    }
    private void reloadCashReceipts(bool clearFields)
    {
      loadCriteria();
      srchCashReceipts.ReLoad();
      if (clearFields)
        ClearFields(true);
    }
    private string getInnerWhere(DateTime billDate)
    {
      string postClause;
      string unpaidClause;
      if (tbPosted.Checked)
        postClause = "'Posted'";
      else
        if (tbPosted.Checked)
          postClause = "'Unposted'";
        else
          postClause = "'All'";
      if (tbUnpaidOnly.Checked)
        unpaidClause = "'Unpaid'";
      else
        unpaidClause = "'All'";
      return string.Format("'{0}', {1}, {2}", billDate.ToShortDateString(), postClause, unpaidClause);
    }
    private string getImportFileName()
    {
      OpenFileDialog openFileDialog1 = new OpenFileDialog();
      openFileDialog1.Title = "Open Payments Import File";
      openFileDialog1.Filter = "TXT files|*.txt";
      openFileDialog1.InitialDirectory = _rootPath;

      DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
      if (result == DialogResult.OK) // Test result.
      {
        return openFileDialog1.FileName;
      }
      else
        return null;
    }
    private void importPaymentFile()
    {
      string fileName = getImportFileName();
      if (string.IsNullOrEmpty(fileName))
        CommonFormFunctions.showMessage("No file was chosen, Import was not run.");
      else
      {
        string storedproc = "ImportPayments";
        DateTime billDate = CommonFunctions.CDateTime(tbBillingPeriod.Text);
        string criteria = String.Format(",'{0}'", billDate.ToString("yyyyMMdd"));
        Exception returnmsg = _dataSource.executestoredproc(storedproc, fileName, criteria);
        if (returnmsg != null)
          CommonFormFunctions.showException(returnmsg);
        else
          Init();
      }
    }
    private void undoImportPayments()
    {
      DateTime billDate = CommonFunctions.CDateTime(tbBillingPeriod.Text);
      _dataSource.undoImportPayments(billDate);
      Init();
    }
    private Dictionary<string, object> getValues(DataGridViewRow row)
    {
      Dictionary<string, object> fields = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
      foreach (DataGridViewCell cell in row.Cells)
        fields.Add(cell.OwningColumn.Name, cell.Value);
      return fields;
    }

    #endregion

    #region form events
    private void frmCashReceipts_Load(object sender, EventArgs e)
    {
      srchCashReceipts.NameType = CommonData.UnmatchedNameTypes.CashReceipts;
      _startDate = (new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)).AddMonths(-1);
      DateTime endDate = DateTime.Today;
      tbBillingPeriod.Text = _startDate.AddMonths(1).ToShortDateString();
      srchCashReceipts.Init(CCI.Common.CommonData.UnmatchedNameTypes.CashReceipts, null);
      Init();
    }
    private void dtStartDate_ValueChanged(object sender, EventArgs e)
    {
      //Init();
    }
    private void radioPosted_CheckedChanged(object sender, EventArgs e)
    {
      //Init();
    }
    private void radioUnPosted_CheckedChanged(object sender, EventArgs e)
    {
      //Init();
    }
    private void srchCashReceipts_RowSelected(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      if (e.SelectedRow == null)
        MessageBox.Show("No row is selected");
      else
        loadFields(getValues(e.SelectedRow), false);
    }
    private void btnUpdate_Click(object sender, EventArgs e)
    {
      if (btnUpdate.Text.Equals("Save") || btnUpdate.Text.Equals("Add"))
      {
        if (validateFields())
        {
          string customerID = cboCustomer.Text;
          bool fromDetail = _fromDetail;
          saveFields();
          reloadCashReceipts(false);
          refreshTotals();
          //if (fromDetail)
            loadDetail(customerID);
          //else
          //  ClearFields(true);
        }
        else
        {
          this.lblMessage.Text = "Entry Error.";
        }
      }
      else
        ClearFields(!_fromDetail);

    }
    private void btnDelete_Click(object sender, EventArgs e)
    {
      string customerID = cboCustomer.Text;
      deleteRecord();
      reloadCashReceipts(true);
      refreshTotals();
      if (_fromDetail)
        loadDetail(customerID);
    }
    private void btnCancel_Click(object sender, EventArgs e)
    {
      ClearFields(!_fromDetail);
    }
    private void btnLoad_Click(object sender, EventArgs e)
    {
      Init();
    }
    private void radioCash_Click(object sender, EventArgs e)
    {
      this.btnUpdate.Enabled = true;
    }
    private void radioCheck_Click(object sender, EventArgs e)
    {
      this.btnUpdate.Enabled = true;
      this.txtCheckNbr.Enabled = true;
    }
    private void btnQBExport_Click(object sender, EventArgs e)
    {
      DialogResult ans = MessageBox.Show("You have chosen to create the Quick Books Cash Receipts export file. This will flag all transactions in the file as exported and you will not be able to edit them. Are you sure this is what you want to do? Press Yes to Continue, No to create the file but not flag the transactions as exported, and Cancel to cancel",
        "QuickBooks Export of Cash Receipts", MessageBoxButtons.YesNoCancel);
      if (ans != DialogResult.Cancel)
      {
        this.lblQBExportMsg.Text = "Exporting...";
        DateTime endDate = _endDate;
        DateTime billDate = new DateTime(endDate.Year, endDate.Month, 1); // first of the month of the end date
        DataSet ds = _dataSource.getCashReceiptsDataset(billDate);
        _dataSource.exportCashReceiptsToCSV(ds);
        string msg = "Completed...";
        if (ans == DialogResult.Yes)
        {
          _dataSource.updateCashReceiptsPosted();
          _dataSource.processStep("QBCashExport", billDate, true, SecurityContext.User);
          msg = "Completed, and transactions have been flagged as exported...";
        }
        this.lblQBExportMsg.Text = msg;
        Init();
      }
    }
    private void radioCC_CheckedChanged(object sender, EventArgs e)
    {
      testPaymentType();
    }
    private void btnPost_Click(object sender, EventArgs e)
    {
      applyPostCashReceipts();
    }
    private void radioACH_CheckedChanged(object sender, EventArgs e)
    {
      testPaymentType();
    }
    private void radioCheck_CheckedChanged(object sender, EventArgs e)
    {
      testPaymentType();
    }
    private void srchCashReceipts_RowChecked(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      payThisRow(e.SelectedRow, e.SelectedColumn);
    }
    private void btnNew_Click(object sender, EventArgs e)
    {
      radioBill.Visible = false;
      ClearFields(true);
    }
    private void dtDateReceived_ValueChanged(object sender, EventArgs e)
    {
      btnUpdate.Enabled = true;
    }
    private void cboCustomer_OnSelected(object sender, EventArgs e)
    {
      btnUpdate.Enabled = true;
    }
    private void txtCheckNbr_TextChanged(object sender, EventArgs e)
    {
      btnUpdate.Enabled = true;
    }
    private void txtAmount_TextChanged(object sender, EventArgs e)
    {
      btnUpdate.Enabled = true;
    }
    private void srchCashDetail_RowSelected(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      if (e.SelectedRow == null)
        MessageBox.Show("No row is selected");
      else 
      {
        string transType = CommonFunctions.CString(e.SelectedRow.Cells["TransactionType"].Value);
        string reference = CommonFunctions.CString(e.SelectedRow.Cells["Reference"].Value);
        if (transType.Equals("Cash", StringComparison.CurrentCultureIgnoreCase) ||
          reference.Equals("Service Activation", StringComparison.CurrentCultureIgnoreCase))
          loadFields(getValues(e.SelectedRow), true);
        else
        {
          if (transType.Equals("Bill", StringComparison.CurrentCultureIgnoreCase))
          {
            DialogResult ans = MessageBox.Show("Are you sure you want to edit this Bill/Invoice?", "Edit a Bill", MessageBoxButtons.YesNo);
            if (ans == DialogResult.Yes)
              loadFields(getValues(e.SelectedRow), true);
            else
              MessageBox.Show("Edit of Bill/Invoice Cancelled");
          }
          else
            MessageBox.Show(string.Format("You cannot edit a tranaaction of type {0} in this screen", transType));

        }
      }
    }
    private void cboCustomer_OnSelected_1(object sender, EventArgs e)
    {
      EntityAttributesCollection eac = _ea.getAttributes(cboCustomer.Text, "Entity", "Customer", null, DateTime.Today);
      Entity ent = eac.Entities[cboCustomer.Text];
      String paymenttype = CommonFunctions.CString(eac.getValue(cboCustomer.Text + ".Entity.Customer.PaymentType"));
      if (paymenttype.Equals("CreditCard", StringComparison.CurrentCultureIgnoreCase))
      {
        this.radioCC.Checked = true;
      }
      else if (paymenttype.Equals("Check", StringComparison.CurrentCultureIgnoreCase))
      {
        this.radioCheck.Checked = true;
      }
      else if (paymenttype.Equals("ACH", StringComparison.CurrentCultureIgnoreCase)
        || paymenttype.Equals("Cash", StringComparison.CurrentCultureIgnoreCase))
      {
        this.radioACH.Checked = true;
      }

    }
    private void tbBillingPeriod_Leave(object sender, EventArgs e)
    {
      DateTime billingDate = Convert.ToDateTime(tbBillingPeriod.Text);
      _startDate = new DateTime(billingDate.Year, billingDate.Month - 1, 1);
    }
    private void radioBill_CheckedChanged(object sender, EventArgs e)
    {
      testPaymentType();
    }
    private void btnImportPayments_Click(object sender, EventArgs e)
    {
      importPaymentFile();
    }
    #endregion

    private void btnUndoImport_Click(object sender, EventArgs e)
    {
      undoImportPayments();
    }
  }
}
