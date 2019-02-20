using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using Excel=Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Reflection;

using CCI.Common;
using CCI.Sys.Data;
using CCI.DesktopClient.Common;

using TAGBOSS.Common;
using TAGBOSS.Common.Model;

namespace CCI.DesktopClient.Screens
{
  public partial class frmImports : ScreenBase
  {
    DataSource _dataSource = new DataSource();
    Excel.Application appExl;
    Excel.Workbook workbook;
    Excel.Worksheet NwSheet;
    Excel.Range ShtRange;
    private string _basePath = string.Empty;
    bool hasValidFiles = false;
    bool importfileinvaliddate = false;
    private string _sourceFolder = "Saddleback Imports";
    bool _importLedgerOnly = false;
    private string _rootPath
    {
      get
      {
        return "C:\\Data\\" + _sourceFolder;
      }
      set
      {
        ;
      }
    }
    private bool useNewImport = true;
    private enum ImportSource { Saddleback, RedRock }
    private ImportSource _importSource = ImportSource.Saddleback;
    private class SourceFTPInfo
    {
      public string Uri  { get; set; }
      public string Uid  { get; set; }
      public string Pw  { get; set; }
    }
    
    private class ImportSourceConfiguration
    {
      public ImportSource Source { get; set; }
      public List<ImportfileType> FileTypes { get; set; }
      public SourceFTPInfo FTPInfo { get; set; }
    }
    private class ImportfileType
    {
      public ImportFileTypes FileType { get; set; }
      public string Prefix { get; set; }
      public string Suffic { get; set; }
      public string StoredProcedure { get; set; }
      public int skiplines { get; set; }
    }
    private enum ImportFileTypes { MRCWholesale, MRCRetail, OCCWholesale, OCCRetail, TollWholesale, TollRetail, Tax, Ledger };
    private Dictionary<ImportSource, ImportSourceConfiguration> _importSources = new Dictionary<ImportSource, ImportSourceConfiguration>();
    private const string FILENOTFOUNDPREFIX = "NOT FOUND: ";
    private List<Control> _textBoxes = null;

    public string Source
    {
      get { return _importSource.ToString();  }
      set
      {
        if (string.IsNullOrEmpty(value))
        {
          CommonFormFunctions.showMessage("I don't know which source to import from");
          this.Close();
        }
        if (value.Equals("Saddleback", StringComparison.CurrentCultureIgnoreCase))
        {
          _importSource = ImportSource.Saddleback;
          _sourceFolder = "Saddleback Imports";
          //_fileTypePrefixes = _saddlebackFileTypePrefixes;
          //_fileTypeSuffixes = _saddlebackFileTypeSuffixes;
        }
        else if (value.Equals("RedRock", StringComparison.CurrentCultureIgnoreCase))
        {
          _importSource = ImportSource.RedRock;
          _sourceFolder = "Red Rock Imports";
          //_fileTypePrefixes = _redRockFileTypePrefixes;
          //_fileTypeSuffixes = _redRockFileTypeSuffixes;
          //ckDownload.Visible = false;
        }
        else
        {
          CommonFormFunctions.showMessage("I don't know which source to import from");
          this.Close();
        }
      }
    }
    public frmImports()
    {

      InitializeComponent();
      //            CheckForPreviousImports();
      _textBoxes = ACG.CommonForms.CommonFormFunctions.GetAllControls<TextBox>(this);

    }

    #region form events
    private void btnFindFolder_Click(object sender, EventArgs e)
    {
      resetFileColors(false);
      btnImport.Enabled = false;
      btnPostDetails.Enabled = false;
      btnUndoImport.Enabled = false;
      btnUndoPost.Enabled = false;
      FolderBrowserDialog openFileDialog1 = new FolderBrowserDialog();

      openFileDialog1.SelectedPath = _rootPath;
      openFileDialog1.Description = "Select the folder where the import files for this bill date have been placed";
            

      DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
      if (result == DialogResult.OK) // Test result.
      {
        txtBasePath.Text = _rootPath = _basePath = openFileDialog1.SelectedPath;
        btnReloadFiles.Enabled = true;
        loadFileNames(ckDownload.Checked);
      }
    }

    //private void btnFindFileMRCRetail_Click(object sender, EventArgs e)
      //{
      //    OpenFileDialog openFileDialog1 = new OpenFileDialog();

      //    openFileDialog1.InitialDirectory = "Y:\\City Hosted Solutions\\Saddleback Imports";
      //    openFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
      //    openFileDialog1.FilterIndex = 2;
      //    openFileDialog1.RestoreDirectory = true;

      //    int size = -1;
      //    DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
      //    if (result == DialogResult.OK) // Test result.
      //    {
      //      if (isValidFileName(openFileDialog1.FileName, ImportFileTypes.MRCRetail))
      //        this.txtFileName2.Text = openFileDialog1.FileName;
      //    }
      //}
      //private void btnFindFileOCCWholesale_Click(object sender, EventArgs e)
      //{
      //    OpenFileDialog openFileDialog1 = new OpenFileDialog();

      //    openFileDialog1.InitialDirectory = "Y:\\City Hosted Solutions\\Saddleback Imports";
      //    openFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
      //    openFileDialog1.FilterIndex = 2;
      //    openFileDialog1.RestoreDirectory = true;

      //    int size = -1;
      //    DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
      //    if (result == DialogResult.OK) // Test result.
      //    {
      //      if (isValidFileName(openFileDialog1.FileName, ImportFileTypes.OCCWholesale))
      //        this.txtFileName3.Text = openFileDialog1.FileName;
      //    }
      //}
      //private void btnFindFileTax_Click(object sender, EventArgs e)
      //{
      //    OpenFileDialog openFileDialog1 = new OpenFileDialog();

      //    openFileDialog1.InitialDirectory = "Y:\\City Hosted Solutions\\Saddleback Imports";
      //    openFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
      //    openFileDialog1.FilterIndex = 2;
      //    openFileDialog1.RestoreDirectory = true;

      //    int size = -1;
      //    DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
      //    if (result == DialogResult.OK) // Test result.
      //    {
      //      if (isValidFileName(openFileDialog1.FileName, ImportFileTypes.Tax))
      //        this.txtFileName7.Text = openFileDialog1.FileName;
      //    }
      //}
      //private void btnFindFileTollWholesale_Click(object sender, EventArgs e)
      //{
      //    OpenFileDialog openFileDialog1 = new OpenFileDialog();

      //    openFileDialog1.InitialDirectory = "Y:\\City Hosted Solutions\\Saddleback Imports";
      //    openFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
      //    openFileDialog1.FilterIndex = 2;
      //    openFileDialog1.RestoreDirectory = true;

      //    int size = -1;
      //    DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
      //    if (result == DialogResult.OK) // Test result.
      //    {
      //      if (isValidFileName(openFileDialog1.FileName, ImportFileTypes.TollWholesale))
      //        this.txtFileName5.Text = openFileDialog1.FileName;
      //    }
      //}
      //private void btnFindFileTollRetail_Click(object sender, EventArgs e)
      //{
      //    OpenFileDialog openFileDialog1 = new OpenFileDialog();

      //    openFileDialog1.InitialDirectory = "Y:\\City Hosted Solutions\\Saddleback Imports";
      //    openFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
      //    openFileDialog1.FilterIndex = 2;
      //    openFileDialog1.RestoreDirectory = true;

      //    int size = -1;
      //    DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
      //    if (result == DialogResult.OK) // Test result.
      //    {
      //      if (isValidFileName(openFileDialog1.FileName, ImportFileTypes.TollRetail))

      //        this.txtFileName6.Text = openFileDialog1.FileName;
      //    }
      //}
      //private void btnFindFileOCCRetail_Click(object sender, EventArgs e)
      //{
      //    OpenFileDialog openFileDialog1 = new OpenFileDialog();

      //    openFileDialog1.InitialDirectory = "Y:\\City Hosted Solutions\\Saddleback Imports";
      //    openFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
      //    openFileDialog1.FilterIndex = 2;
      //    openFileDialog1.RestoreDirectory = true;

      //    int size = -1;
      //    DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
      //    if (result == DialogResult.OK) // Test result.
      //    {
      //      if (isValidFileName(openFileDialog1.FileName, ImportFileTypes.OCCRetail))
      //        this.txtFileName4.Text = openFileDialog1.FileName;
      //    }
      //}
    private void btnImport_Click(object sender, EventArgs e)
    {
      if (hasValidFiles || _importLedgerOnly)
      {
        #region MRC Wholesale
        if (!_importLedgerOnly && !this.txtFileName1.Text.StartsWith(FILENOTFOUNDPREFIX))
        {
          importfileinvaliddate = false;
          Cursor.Current = Cursors.WaitCursor;
          string filetype = "MRC Wholesale";
          DateTime billdate = txtBillDate.Value;
          if (ValidImport(billdate, filetype) == false)
          {
            this.txtFileName1.Text = this.txtFileName1.Text.Replace("Y:\\", "c:\\city hosted solutions\\");
            this.lblMRCWholesaleMsg.Text = "Importing...";
            importfile(ImportFileTypes.MRCWholesale, billdate);
            if (importfileinvaliddate == false)
            {
              string user = SecurityContext.User;
              _dataSource.insertAcctImportLog(user, filetype, this.txtFileName1.Text, billdate, Source);
              RefreshImportLog();
              string step = "MRC Wholesale";
              _dataSource.insertHostedProcessStep(user, step, billdate, filetype, this.txtFileName1.Text);
              this.lblMRCWholesaleMsg.Text = "Completed...";
            }
            else
            {
              this.lblMRCWholesaleMsg.Text = "File has wrong bill date";
            }
            Cursor.Current = Cursors.Default;
          }
          else
          {
            this.lblMRCWholesaleMsg.Text = "Already Imported.";
          }
        }
        #endregion
        #region MRC Retail
        if (!_importLedgerOnly && !this.txtFileName2.Text.StartsWith(FILENOTFOUNDPREFIX))
        {
          importfileinvaliddate = false;
          Cursor.Current = Cursors.WaitCursor;
          string filetype = "MRC Retail";
          DateTime billdate = txtBillDate.Value;
          if (ValidImport(billdate, filetype) == false)
          {
            if (useNewImport)
              this.txtFileName2.Text = this.txtFileName2.Text.Replace("Y:\\", "c:\\city hosted solutions\\");
            else
              openSpreadsheet(this.txtFileName2.Text);
            this.lblMRCRetailMsg.Text = "Importing...";
            importfile(ImportFileTypes.MRCRetail, billdate);
            if (!useNewImport)
              closeExcel();
            if (importfileinvaliddate == false)
            {
              string user = SecurityContext.User;
              _dataSource.insertAcctImportLog(user, filetype, this.txtFileName2.Text, billdate, Source);
              RefreshImportLog();
              string step = "MRC Retail";
              _dataSource.insertHostedProcessStep(user, step, billdate, filetype, this.txtFileName2.Text);
              this.lblMRCRetailMsg.Text = "Completed...";
            }
            else
            {
              this.lblMRCRetailMsg.Text = "File has wrong bill date";
            }
            Cursor.Current = Cursors.Default;
          }
          else
          {
            this.lblMRCRetailMsg.Text = "Already Imported.";
          }
        }
        #endregion
        #region OCC Wholesale
        if (!_importLedgerOnly && !this.txtFileName3.Text.StartsWith(FILENOTFOUNDPREFIX))
        {
          importfileinvaliddate = false;
          Cursor.Current = Cursors.WaitCursor;
          string filetype = "OCC Wholesale";
          DateTime billdate = txtBillDate.Value;
          if (ValidImport(billdate, filetype) == false)
          {
            if (useNewImport)
              this.txtFileName3.Text = this.txtFileName3.Text.Replace("Y:\\", "c:\\city hosted solutions\\");
            else
            openSpreadsheet(this.txtFileName3.Text);
            this.lblOCCWholesaleMsg.Text = "Importing...";
            importfile(ImportFileTypes.OCCWholesale, billdate);
            if (!useNewImport)
              closeExcel();
            if (importfileinvaliddate == false)
            {
              string user = SecurityContext.User;
              _dataSource.insertAcctImportLog(user, filetype, this.txtFileName3.Text, billdate, Source);
              RefreshImportLog();
              string step = "OCC Wholesale";
              _dataSource.insertHostedProcessStep(user, step, billdate, filetype, this.txtFileName3.Text);
              this.lblOCCWholesaleMsg.Text = "Completed...";
            }
            else
            {
              this.lblOCCWholesaleMsg.Text = "File has wrong bill date";
            }
            Cursor.Current = Cursors.Default;
          }
          else
          {
            this.lblOCCWholesaleMsg.Text = "Already Imported.";
          }
        }
        #endregion
        #region OCC Retail
        if (!_importLedgerOnly && !this.txtFileName4.Text.StartsWith(FILENOTFOUNDPREFIX))
        {
          Cursor.Current = Cursors.WaitCursor;
          string filetype = "OCC Retail";
          DateTime billdate = txtBillDate.Value;
          if (ValidImport(billdate, filetype) == false)
          {
            if (useNewImport)
              this.txtFileName4.Text = this.txtFileName4.Text.Replace("Y:\\", "c:\\city hosted solutions\\");
            else
              openSpreadsheet(this.txtFileName4.Text);
            this.lblOCCRetailMsg.Text = "Importing...";
            importfile(ImportFileTypes.OCCRetail, billdate);
            if (!useNewImport)
              closeExcel();
            if (importfileinvaliddate == false)
            {
              string user = SecurityContext.User;
              _dataSource.insertAcctImportLog(user, filetype, this.txtFileName4.Text, billdate, Source);
              RefreshImportLog();
              string step = "OCC Retail";
              _dataSource.insertHostedProcessStep(user, step, billdate, filetype, this.txtFileName4.Text);
              this.lblOCCRetailMsg.Text = "Completed...";
            }
            else
            {
              this.lblOCCRetailMsg.Text = "File has wrong bill date";
            }
            Cursor.Current = Cursors.Default;
          }
          else
          {
            this.lblOCCRetailMsg.Text = "Already Imported.";
          }
        }
        #endregion
        #region Toll Wholesale
        if (!_importLedgerOnly && !this.txtFileName5.Text.StartsWith(FILENOTFOUNDPREFIX))
        {
          Exception returnmsg;
          Boolean importerror;
  //         this.txtFileName5.Text = this.txtFileName5.Text.Replace("y:\\", "d:\\city operations\\");
  //         this.txtFileName5.Text = this.txtFileName5.Text.Replace("Y:\\", "d:\\city operations\\");
          //this.txtFileName5.Text = this.txtFileName5.Text.Replace("y:\\", "c:\\city hosted solutions\\");
          this.txtFileName5.Text = this.txtFileName5.Text.Replace("Y:\\", "c:\\city hosted solutions\\");
          Cursor.Current = Cursors.WaitCursor;
          string filetype = "Calls Wholesale";
          string importtable = "hostedtemptollwholesale";
          DateTime billdate = txtBillDate.Value;
          if (ValidImport(billdate, filetype) == false)
          {
            this.lblCallWholesaleMsg.Text = "Importing...";
            this.lblCallWholesaleMsg.Refresh();
            returnmsg = importfile(ImportFileTypes.TollWholesale, billdate);
            if (returnmsg == null)
            {
              importerror = _dataSource.checkfortollimporterror(importtable, Convert.ToString(billdate.Year), Convert.ToString(billdate.Month));
              if (importerror == false)
              {
                string user = SecurityContext.User;
                returnmsg = _dataSource.insertAcctImportLog(user, filetype, this.txtFileName5.Text, billdate, Source);
                if (returnmsg == null)
                {
                  RefreshImportLog();
                  string step = "Calls Wholesale";
                  returnmsg = _dataSource.insertHostedProcessStep(user, step, billdate, filetype, this.txtFileName5.Text);
                  if (returnmsg == null)
                  {
                    this.lblCallWholesaleMsg.Text = "Completed...";
                    this.lblCallWholesaleMsg.Refresh();
                  }
                  else
                  {
                    this.lblCallWholesaleMsg.Text = "Error updating process cycle";
                    this.lblCallWholesaleMsg.Refresh();
                    this.txtErrorMessage.Text = returnmsg.Message;
                  }
                }
                else
                {
                  this.lblCallWholesaleMsg.Text = "Error updating import log";
                  this.lblCallWholesaleMsg.Refresh();
                  this.txtErrorMessage.Text = returnmsg.Message;
                }
              }
              else
              {
                this.lblCallWholesaleMsg.Text = "File has wrong bill date";
                this.lblCallWholesaleMsg.Refresh();
              }
            }
            else
            {
              this.lblCallWholesaleMsg.Text = "Error Importing Call Wholesale";
              this.lblCallWholesaleMsg.Refresh();
              this.txtErrorMessage.Text = returnmsg.Message;
            }
            Cursor.Current = Cursors.Default;
          }
          else
          {
            this.lblCallWholesaleMsg.Text = "Already Imported.";
          }
        }
        #endregion
        #region Toll Retail
        if (!_importLedgerOnly && !this.txtFileName6.Text.StartsWith(FILENOTFOUNDPREFIX))
        {
          Exception returnmsg;
          Boolean importerror;
//            this.txtFileName6.Text = this.txtFileName6.Text.Replace("y:\\", "d:\\city operations\\");
//            this.txtFileName6.Text = this.txtFileName6.Text.Replace("Y:\\", "d:\\city operations\\");
          //this.txtFileName6.Text = this.txtFileName6.Text.Replace("y:\\", "c:\\city hosted solutions\\");
          this.txtFileName6.Text = this.txtFileName6.Text.Replace("Y:\\", "c:\\city hosted solutions\\");

          Cursor.Current = Cursors.WaitCursor;
          string filetype = "Calls Retail";
          string importtable = "hostedtemptollretail";
          DateTime billdate = txtBillDate.Value;
          if (ValidImport(billdate, filetype) == false)
          {
            this.lblCallRetailMsg.Text = "Importing...";
            returnmsg = importfile(ImportFileTypes.TollRetail, billdate);
            if (returnmsg == null)
            {
              importerror = _dataSource.checkfortollimporterror(importtable, Convert.ToString(billdate.Year), Convert.ToString(billdate.Month));
              if (importerror == false)
              {
                string user = SecurityContext.User;
                returnmsg = _dataSource.insertAcctImportLog(user, filetype, this.txtFileName6.Text, billdate, Source);
                if (returnmsg == null)
                {
                  RefreshImportLog();
                  string step = "Calls Retail";
                  returnmsg = _dataSource.insertHostedProcessStep(user, step, billdate, filetype, this.txtFileName6.Text);
                  if (returnmsg == null)
                  {
                    this.lblCallRetailMsg.Text = "Completed...";
                    this.lblCallRetailMsg.Refresh();
                  }
                  else
                  {
                    this.lblCallRetailMsg.Text = "Error updating process cycle";
                    this.lblCallRetailMsg.Refresh();
                    this.txtErrorMessage.Text = returnmsg.Message;
                  }
                }
                else
                {
                  this.lblCallRetailMsg.Text = "Error updating import log";
                  this.lblCallRetailMsg.Refresh();
                  this.txtErrorMessage.Text = returnmsg.Message;
                }
              }
              else
              {
                this.lblCallRetailMsg.Text = "File has wrong bill date";
                this.lblCallRetailMsg.Refresh();
              }
            }
            else
            {
              this.lblCallRetailMsg.Text = "Error Importing Call Retail";
              this.lblCallRetailMsg.Refresh();
              this.txtErrorMessage.Text = returnmsg.Message;
            }
            Cursor.Current = Cursors.Default;
          }
          else
          {
            this.lblCallRetailMsg.Text = "Already Imported.";
          }
        }
        #endregion
        #region Tax
        if (!_importLedgerOnly && !this.txtFileName7.Text.StartsWith(FILENOTFOUNDPREFIX))
        {
          Cursor.Current = Cursors.WaitCursor;
          string filetype = "Tax Detail";
          DateTime billdate = txtBillDate.Value;
          if (ValidImport(billdate, filetype) == false)
          {
            // if there is not a stored procedure for this file, then use the special spreadsheet logic. 
            // note this will need to change if we get another source that needs spreadsheet input but is not Saddleback
            if (_importSources[_importSource].FileTypes.Where(t => t.FileType == ImportFileTypes.Tax).FirstOrDefault().StoredProcedure.Equals("Spreadsheet", StringComparison.CurrentCultureIgnoreCase))
            {
              openSpreadsheet(this.txtFileName7.Text);
              this.lblTaxesMsg.Text = "Importing...";
              importTaxes(txtBillDate.Value);
              closeExcel();
            }
            else
              importfile(ImportFileTypes.Tax, billdate); // just process normally using the stored procedure
            string user = SecurityContext.User;
            _dataSource.insertAcctImportLog(user, filetype, this.txtFileName7.Text, billdate, Source);
            RefreshImportLog();
            string step = "Tax Detail";
            _dataSource.insertHostedProcessStep(user, step, billdate, filetype, this.txtFileName7.Text);
            this.lblTaxesMsg.Text = "Completed...";
            Cursor.Current = Cursors.Default;
          }
          else
          {
            this.lblTaxesMsg.Text = "Already Imported.";
          }
        }
        #endregion
        #region Ledger
        if (ckRequireLedger.Checked && !this.txtFileName8.Text.StartsWith(FILENOTFOUNDPREFIX))
        {
          Cursor.Current = Cursors.WaitCursor;
          string filetype = "Customer Ledger";
          DateTime billdate = txtBillDate.Value;
          if (ValidImport(billdate, filetype) == false)
          {
            if (_importSources[_importSource].FileTypes.Where(t => t.FileType == ImportFileTypes.Ledger).FirstOrDefault().StoredProcedure.Equals("Spreadsheet", StringComparison.CurrentCultureIgnoreCase))
            {
              openSpreadsheet(this.txtFileName8.Text);
              this.lblLedgerMsg.Text = "Importing...";
              importLedger(txtBillDate.Value);
              closeExcel();
            }
            else
              importfile(ImportFileTypes.Ledger, billdate);
            string user = SecurityContext.User;
            _dataSource.insertAcctImportLog(user, filetype, this.txtFileName8.Text, billdate, Source);
            RefreshImportLog();
            string step = "Customer Ledger";
            _dataSource.insertHostedProcessStep(user, step, billdate, filetype, this.txtFileName8.Text);
            this.lblLedgerMsg.Text = "Completed...";
            Cursor.Current = Cursors.Default;
          }
          else
          {
            this.lblTaxesMsg.Text = "Already Imported.";
          }
        }
        #endregion
        RefreshImportLog();
        btnPostDetails.Enabled = true;
        btnUndoImport.Enabled = true;
        btnImport.Enabled = false;
      }
      else
      {
        this.txtErrorMessage.Text = "Make sure the bill date and all import file names are filled.";
      }
    }
    private void btnPostDetails_Click(object sender, EventArgs e)
    {
      DialogResult ans = MessageBox.Show(@"Have you Imported all sources (e.g. Saddleback, Red Rock, etc.)? If not, it is not advisable to Post yet. 
You cannot import and post another source after you have posted something for this Billing Period. Select Yes if you have imported everything,
No if you have not and wish to be safe, and Cancel if you want to ignore and continue to Post unsafely.", "OK to Post", MessageBoxButtons.YesNoCancel);
      if (ans == DialogResult.No)
      {
        MessageBox.Show("Request to Post is Cancelled");
        return;
      }
      else if (ans == DialogResult.Cancel)
      {
        ans = MessageBox.Show("Are you sure you want to Post even though you have not imported everything and will not be able to import what remains?", "OK to Post", MessageBoxButtons.YesNo);
        if (ans == DialogResult.No)
        {
          MessageBox.Show("Request to Post is Cancelled");
          return;
        }
      }

      Exception returnmsg;
      string filetype = "Posted";
      DateTime billdate = txtBillDate.Value;
      if (ValidPost(billdate, filetype) == false)
      {
        this.lblPostedMsg.Text = "Posting...";
        this.lblPostedMsg.Refresh();
        string user = SecurityContext.User;
        if (!_importLedgerOnly)
        {
          returnmsg = _dataSource.processMRCDetails(txtBillDate.Value, user);
          if (returnmsg == null)
          {
            this.lblMRCWholesaleMsg.Text = "Posted...";
            this.lblMRCWholesaleMsg.Refresh();
            this.lblMRCRetailMsg.Text = "Posted...";
            this.lblMRCRetailMsg.Refresh();
          }
          else
          {
            this.lblMRCWholesaleMsg.Text = "Error Posting";
            this.lblMRCWholesaleMsg.Refresh();
            this.lblMRCRetailMsg.Text = "Error Posting";
            this.lblMRCRetailMsg.Refresh();
            this.txtErrorMessage.Text = returnmsg.Message;
          }
          returnmsg = _dataSource.processOCCDetails(txtBillDate.Value, user);
          if (returnmsg == null)
          {
            this.lblOCCWholesaleMsg.Text = "Posted...";
            this.lblOCCWholesaleMsg.Refresh();
            this.lblOCCRetailMsg.Text = "Posted...";
            this.lblOCCRetailMsg.Refresh();
          }
          else
          {
            this.lblOCCWholesaleMsg.Text = "Error Posting";
            this.lblOCCWholesaleMsg.Refresh();
            this.lblOCCRetailMsg.Text = "Error Posting";
            this.lblOCCRetailMsg.Refresh();
            this.txtErrorMessage.Text = returnmsg.Message;
          }
          returnmsg = _dataSource.processTOLLDetails(txtBillDate.Value, user);
          if (returnmsg == null)
          {
            this.lblCallWholesaleMsg.Text = "Posted...";
            this.lblCallWholesaleMsg.Refresh();
            this.lblCallRetailMsg.Text = "Posted...";
            this.lblCallRetailMsg.Refresh();
          }
          else
          {
            this.lblCallWholesaleMsg.Text = "Error Posting";
            this.lblCallWholesaleMsg.Refresh();
            this.lblCallRetailMsg.Text = "Error Posting";
            this.lblCallRetailMsg.Refresh();
            this.txtErrorMessage.Text = returnmsg.Message;
          }
          returnmsg = _dataSource.insertTaxDetail(txtBillDate.Value, user);
          if (returnmsg == null)
          {
            this.lblTaxesMsg.Text = "Posted...";
            this.lblTaxesMsg.Refresh();
          }
          else
          {
            this.lblTaxesMsg.Text = "Error Posting";
            this.lblTaxesMsg.Refresh();
            this.txtErrorMessage.Text = returnmsg.Message;
          }
        }
        if (ckRequireLedger.Checked)
        {
          returnmsg = _dataSource.processLedgerTransactions(txtBillDate.Value, user);
          if (returnmsg == null)
          {
            lblLedgerMsg.Text = "Posted...";
            lblLedgerMsg.Refresh();
          }
          else
          {
            lblLedgerMsg.Text = "Error Posting";
            lblLedgerMsg.Refresh();
            this.txtErrorMessage.Text = returnmsg.Message;
          }
        }
        returnmsg = _dataSource.insertAcctImportLog(user, filetype, this.txtFileName7.Text, billdate, Source);
        if (returnmsg == null)
        {
          this.lblPostedMsg.Text = "Posted...";
          this.lblPostedMsg.Refresh();
        }
        else
        {
          this.lblPostedMsg.Text = "Error updating import log";
          this.lblPostedMsg.Refresh();
          this.txtErrorMessage.Text = returnmsg.Message;
        }
        string step = "Posted";
        returnmsg = _dataSource.insertHostedProcessStep(user, step, billdate, filetype, this.txtFileName4.Text);
        if (returnmsg == null)
        {
          this.lblPostedMsg.Text = "Posted...";
          this.lblPostedMsg.Refresh();
        }
        else
        {
          this.lblPostedMsg.Text = "Error updating process cycle";
          this.lblPostedMsg.Refresh();
          this.txtErrorMessage.Text = returnmsg.Message;
        }
        RefreshImportLog();
        btnPostDetails.Enabled = false;
        btnUndoPost.Enabled = true;
      }
      else
      {
        this.lblPostedMsg.Text = "Already Posted.";
        btnPostDetails.Enabled = false;
        btnUndoPost.Enabled = true;
      }
    }

    private void btnUndoImport_Click(object sender, EventArgs e)
    {
      //if (this.txtBillingDate.Text != "")
      //{
        string filetype = "Posted";
        string returnmsg;
        DateTime billdate = txtBillDate.Value;
        if (ValidPost(billdate, filetype) == false)
        {
          this.lblUndoPostMsg.Text = "Undoing Import...";
          this.lblUndoPostMsg.Refresh();
          string user = SecurityContext.User;
          returnmsg = _dataSource.processUndoImport(txtBillDate.Value, user);
          if (returnmsg == "")
          {
            RefreshImportLog();
            this.lblUndoPostMsg.Text = "Undo Completed...";
            this.lblUndoPostMsg.Refresh();
          }
          else
          {
            this.lblUndoPostMsg.Text = returnmsg;
            this.lblUndoPostMsg.Refresh();
          }
        }
        else
        {
          lblUndoPostMsg.Text = "Already posted";
        }
      //}
      //else
      //{
      //  this.lblUndoPostMsg.Text = "Missing Bill Date.";
      //}
      RefreshImportLog();
    }
    private void btnUndoPost_Click(object sender, EventArgs e)
    {
      //if (this.txtBillingDate.Text != "")
      //{
        string filetype = "Posted";
        string returnmsg;
        DateTime billdate = txtBillDate.Value;
        if (ValidPost(billdate, filetype) == true)
        {
          this.lblUndoPostMsg.Text = "Undoing Post...";
          this.lblUndoPostMsg.Refresh();
          string user = SecurityContext.User;
          returnmsg = _dataSource.processUndoPost(txtBillDate.Value, user, Source);
          if (returnmsg == "")
          {
            RefreshImportLog();
            this.lblUndoPostMsg.Text = "Undo Completed...";
            this.lblUndoPostMsg.Refresh();
          }
          else
          {
            this.lblUndoPostMsg.Text = returnmsg;
            this.lblUndoPostMsg.Refresh();
          }
        }
        else
        {
          lblUndoPostMsg.Text = "Not yet posted";
        }
      //}
      //else
      //{
      //  this.lblUndoPostMsg.Text = "Missing Bill Date.";
      //}
      RefreshImportLog();
    }
    private void btnQBExport_Click(object sender, EventArgs e)
    {

    }

    private bool ValidImport(DateTime billdate, string filetype)
    {
      bool logexists;
      logexists = _dataSource.checkProcessStepsbeforeimport(billdate, filetype, filetype);
      return logexists;
    }
    private bool ValidPost(DateTime billdate, string step)
    {
      bool logexists;
      logexists = _dataSource.checkProcessStepsbeforeimport(billdate, step, null);
      return logexists;
    }
    private bool ValidUnPost(DateTime billdate, string step)
    {
      bool logexists;
      logexists = _dataSource.checkforUnpost(billdate, step, null);
      return logexists;
    }
    private void btnReloadFiles_Click(object sender, EventArgs e)
    {
      loadFileNames();
    }

    private void ckImportLedgerOnly_CheckedChanged(object sender, EventArgs e)
    {
      _importLedgerOnly = ckImportLedgerOnly.Checked;
    }
    private void frmImports_Load(object sender, EventArgs e)
    {
      this.Text = this.Text + " files for " + Source;
      if (_importSources.Count == 0)
        loadImportFileTypes();
      RefreshImportLog();
      txtBillDate.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
      string filetype = "Posted";
      string importfiletype = "Tax Detail";
      DateTime billdate = txtBillDate.Value;
      if (ValidImport(billdate, importfiletype) == true && ValidPost(billdate, filetype) == false)
      {
        this.btnPostDetails.Enabled = true;
      }
      if (ValidPost(billdate, filetype) == true)
      {
        this.btnUndoPost.Enabled = true;
      }
      if (ValidUnPost(billdate, filetype) == true)
      {
        this.btnPostDetails.Enabled = true;
      }
      //if (!useNewImport)
      //{
      //  ckDownload.Checked = false;
      //  ckDownload.Visible = false;
      //  for (int i = 0; i < 4; i++)
      //    _fileTypeSuffixes[i] = "xls";
      //}
    }

    #endregion

    #region import methods


    private Exception importfile(ImportFileTypes fileTypeEnum, DateTime billdate)
    {
      ImportfileType fileType = _importSources[_importSource].FileTypes.Where(t => t.FileType == fileTypeEnum).FirstOrDefault();
      int fileNumber = (int)fileTypeEnum + 1; // which file name text box i9s this?
      string fileTextBoxName = "txtFileName" + fileNumber.ToString();
      string filepath = _textBoxes.Where(t => t.Name == fileTextBoxName).FirstOrDefault().Text;
      string storedproc = fileType.StoredProcedure;
      string criteria;
      switch (fileTypeEnum)
      {
        case ImportFileTypes.TollRetail:
        case ImportFileTypes.TollWholesale:
          criteria = String.Format(",'{0}', '{1}'", billdate.Year, billdate.Month);
          break;
        default:
          criteria = String.Format(",'{0}'", billdate.ToString("yyyyMMdd"));
          break;
      }
      Exception returnmsg = _dataSource.executestoredproc(storedproc, filepath, criteria);
      return returnmsg;
    }

    private void importTaxes(DateTime importdate)
    {
        string importtable = "hostedimporttaxes";
        DataAdapterContainer da = _dataSource.getMaintenanceAdapter(importtable, new Dictionary<string, string>());
        DataSet ds = da.DataSet;
        DataTable dt = ds.Tables[0];
        int Cnum = 0;
        int Rnum = 0;
        int StartRow = 1;
        int TotRows = 2;
        bool GoodRow = false;
        int BadRowCount = 0;
        int BadColCount = 0;
        for (Rnum = StartRow; Rnum <= ShtRange.Rows.Count - TotRows && BadRowCount < 3; Rnum++)
        {
            DataRow dr = dt.NewRow();
            for (Cnum = 1; Cnum <= 9; Cnum++)
            {
              if ((ShtRange.Cells[Rnum, Cnum] as Excel.Range).Value2 != null)
              {
                dr[Cnum] = TaxCases(Cnum, (ShtRange.Cells[Rnum, Cnum] as Excel.Range).Value2.ToString());
                GoodRow = true;
                BadColCount = 0;
              }
              else
                BadColCount = BadColCount + 1;
            }
            if (BadColCount > 9)
              GoodRow = false;
            if (GoodRow == true)
            {
              dr[10] = importdate;
              dt.Rows.Add(dr);
              GoodRow = false;
              BadRowCount = 0;
            }
            else
            {
              BadRowCount = BadRowCount + 1;
            }
        }
        da.DataAdapter.Update(ds, importtable);
    }
    string TaxCases(int FieldNum, string cellvalue)
    {
      switch (FieldNum)
      {
        case 1: //Customer Number
          {
            string zeropad = "000000";
            cellvalue = cellvalue.Trim();
            int padlength = 8 - cellvalue.Length;
            string strcellvalue = zeropad.Substring(0, padlength) + cellvalue;
            return strcellvalue;
          }
        case 2: //masterbtn
          {
            if (cellvalue.Length > 5)
            {
              string strcellvalue = FixBTNs(cellvalue);
              return strcellvalue;
            }
            else
              return cellvalue;
          }
        case 3: //Level
        case 4: //level description
        case 5: //jurisdiction
        case 6: //Rate
        case 7: //Tax Type
        case 8: //Title
          {
            string strcellvalue = cellvalue.Replace("'", "''");
            return strcellvalue;
          }
        case 9://Tax Amount
          {
            return cellvalue;
          }
      }
      return cellvalue;
    }

    private void importLedger(DateTime importdate)
    {
      string importtable = "hostedimportledger";
      const string balanceLine = "Current Account Balance Due: ";
      DataAdapterContainer da = _dataSource.getMaintenanceAdapter(importtable, new Dictionary<string, string>());
      DataSet ds = da.DataSet;
      DataTable dt = ds.Tables[0];
      int Rnum = 0;
      int StartRow = 5;
      int TotRows = 3;
      DataRow dr = dt.NewRow();
      bool newCust = true;
      string custid = string.Empty;
      string custname = string.Empty;
      DateTime lastTransctionDate = DateTime.Today;
      for (Rnum = StartRow; Rnum <= ShtRange.Rows.Count - TotRows; Rnum++)
      {
        custid = LedgerCases(1, CommonFunctions.CString((ShtRange.Cells[Rnum, 1] as Excel.Range).Value2));
        if (string.IsNullOrEmpty(custid)) //  a blank line
          newCust = true;
        else
        {
          string tmpName = LedgerCases(2, CommonFunctions.CString((ShtRange.Cells[Rnum, 2] as Excel.Range).Value2)).Trim();
          if (newCust)
          {
            newCust = false; // don't write a record, just get the name
            custname = tmpName;
          }
          else
          {
            decimal amount = 0;
            string title = string.Empty;
            DateTime transactionDate = lastTransctionDate;
            if (string.IsNullOrEmpty(tmpName)) // normal line item
            {
              transactionDate = CommonFunctions.CDateTime(LedgerCases(3, CommonFunctions.CString((ShtRange.Cells[Rnum, 3] as Excel.Range).Value2)));
              title = LedgerCases(4, CommonFunctions.CString((ShtRange.Cells[Rnum, 4] as Excel.Range).Value2));
              amount = CommonFunctions.CDecimal(LedgerCases(5, CommonFunctions.CString((ShtRange.Cells[Rnum, 5] as Excel.Range).Value2)));
            }
            else
            {
              // balance line (and last line of customer)
              // Current Account Balance Due: 403.89
              if (tmpName.StartsWith(balanceLine))
              {
                amount = CommonFunctions.CDecimal(tmpName.Substring(balanceLine.Length));
                title = "Balance";
              }
            }
            //dr[0] = System.DBNull.Value;
            dr[1] = custid;
            dr[2] = custname;
            dr[3] = transactionDate;
            dr[4] = title;
            dr[5] = amount;
            dr[6] = txtBillDate.Value;
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            //newCust = true; // looking for the next customer
          }
        }
      }
      da.DataAdapter.Update(ds, importtable);
    }
    string LedgerCases(int FieldNum, string cellvalue)
    {
      string strcellvalue = string.Empty;
      switch (FieldNum)
      {
        case 1: //Customer Number
          {
            if (string.IsNullOrEmpty(cellvalue))
              return strcellvalue;
            string zeropad = "000000";
            cellvalue = cellvalue.Trim();
            int padlength = 8 - cellvalue.Length;
              strcellvalue = zeropad.Substring(0, padlength) + cellvalue;
            return strcellvalue;
          }
        case 3: //TransactionDate
          {
            strcellvalue = ExcelDateConvert(cellvalue).ToString();
            return strcellvalue;
          }
        case 2: //CustomerName
        case 4: //Title
          {
              strcellvalue = cellvalue.Replace("'", "''");
            return strcellvalue;
          }
        case 5://Tax Amount
          {
            return cellvalue;
          }
      }
      return cellvalue;
    }

    #endregion

    #region other private methods
    DateTime ExcelDateConvert(string cellvalue)
    {
      if (CommonFunctions.IsNumeric(cellvalue) && cellvalue.Length != 8)
      {
        int SerialDate = CommonFunctions.CInt(cellvalue);
        if (SerialDate > 59) SerialDate -= 1; //Excel/Lotus 2/29/1900 bug   
        return new DateTime(1899, 12, 31).AddDays(SerialDate);
      }
      else
      {
        string stryear = cellvalue.Substring(0, 4);
        string strmonth = cellvalue.Substring(4, 2);
        string strday = cellvalue.Substring(6, 2);
        DateTime dt = Convert.ToDateTime(strmonth + "/" + strday + "/" + stryear);
        return dt;
      }
    }
    string ExcelDaysConvert(string cellvalue)
    {
        DateTime begdate = new DateTime(1900, 1, 1);
        double xldays = Convert.ToDouble(cellvalue);
        xldays = xldays - 2;
        DateTime xldate = begdate.AddDays(xldays);
        string strxldate = Convert.ToString(xldate);
        return strxldate;
    }
    string ExcelTimeConvert(string cellvalue)
    {
        double dblcellvalue = Convert.ToDouble(cellvalue);
        DateTime time = DateTime.FromOADate(dblcellvalue);
        string strtime = time.ToString("H:mm");
        return strtime;
    }
    string FixBTNs(string cellvalue)
    {
      if (string.IsNullOrEmpty(cellvalue))
        return string.Empty;
      string strcellvalue = cellvalue.Replace(" ", "");
      strcellvalue = strcellvalue.Replace("-", "");
      strcellvalue = strcellvalue.Replace("(", "");
      strcellvalue = strcellvalue.Replace(")", "");
      int len = Math.Max(10, strcellvalue.Length);
      strcellvalue = strcellvalue.Substring(0,len);
      return strcellvalue;
    }
    private bool isValidFileName(string filename, ImportFileTypes fileTypeEnum)
    {
      bool isOK = false;
      ImportSourceConfiguration source = _importSources[_importSource];
      int i = (int)fileTypeEnum;
      ImportfileType fileType = source.FileTypes.Where<ImportfileType>(t => t.FileType == fileTypeEnum).FirstOrDefault();
      string standardPrefix = fileType.Prefix;//_fileTypePrefixes[i];
      //if (CommonFunctions.IsDateTime(txtBillingDate.Text))
      //{
        string comparedate = txtBillDate.Value.ToString("yyyyMM");
        if (!string.IsNullOrEmpty(filename))
        {
          int lastslash = filename.LastIndexOf("\\"); // strip of full path
          if (lastslash > 0)
            filename = filename.Substring(lastslash + 1);
          int dot = filename.LastIndexOf(".");
          if (dot > 0)
            filename = filename.Substring(0, dot);
          int len1 = standardPrefix.Length;
          if (len1 + 6 == filename.Length)
          {
            string prefix = filename.Substring(0, len1);
            if (prefix.Equals(standardPrefix, StringComparison.CurrentCultureIgnoreCase))
            {
              string filerest = filename.Substring(len1);
              if (comparedate.Equals(filerest))
                isOK = true;
            }
          }
        }
      //}
      //else
      //{
      //  MessageBox.Show("Please enter the Bill Date.");
      //  return isOK;
      //}
      if (!isOK)
        MessageBox.Show(string.Format("Incorrect filename format. Must be in the form {0}YYYYMM", standardPrefix));
      return isOK;
          
    }
    private void resetFileColors(bool red)
    {
      for (int iFile = 1; iFile <= 8; iFile++)
      {
        string textbox = "txtFileName" + (iFile + 1).ToString();
        TextBox txt = findTextBox(textbox);
        if (txt != null)
          txt.ForeColor = red ? Color.Red : Color.Black;
      }
    }
    private void openSpreadsheet(string filename)
    {
      //Create COM Objects. Create a COM object for everything that is referenced
      appExl = new Excel.Application();
      workbook = appExl.Workbooks.Open(filename);
      NwSheet = (Excel.Worksheet)workbook.Sheets[1];
      ShtRange = NwSheet.UsedRange;
    }
    private void closeExcel()
    {
        workbook.Close(true, Missing.Value, Missing.Value);
        appExl.Quit();
    }
    private TextBox findTextBox(string name)
    {
      return findTextBox(name, this);
    }
    private TextBox findTextBox(string name, Control c)
    {
      if (c.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
        if (c.GetType() == typeof(TextBox))
          return (TextBox)c;
        else
          throw new Exception(string.Format("Control {0} is not a TextBox", name));
      else
        if (c.HasChildren)
          foreach (Control container in c.Controls)
          {
            TextBox txt = findTextBox(name, container);
            if (txt != null)
              return txt;
          }
      return null;
    }
    private void CheckForPreviousImports()
    {
        DateTime nodate = new DateTime(1980, 1, 1);
        DateTime dt;
        string sdt;
        sdt = _dataSource.selectBillDatefromLog("MRC Wholesale");
        if (sdt != "")
        {
            this.lblMRCWholesaleMsg.Text = String.Format("{0:yyyy/MM}", sdt);
        }
        dt = _dataSource.selectBillingDatefromTemp("HostedImportMRCRetail");
        if (dt > nodate)
        {
            this.lblMRCRetailMsg.Text = String.Format("{0:yyyy/MM/dd}", dt);
        }
        sdt = _dataSource.selectBillDatefromLog("OCC Wholesale");
        if (sdt != "")
        {
            this.lblOCCWholesaleMsg.Text = String.Format("{0:yyyy/MM}", sdt);
        }
        dt = _dataSource.selectBillingDatefromTemp("HostedImportOCCRetail");
        if (dt > nodate)
        {
            this.lblOCCRetailMsg.Text = String.Format("{0:yyyy/MM/dd}", dt);
        }
        sdt = _dataSource.selectBillingDatefromCallTemp("HostedImportTollWholesale");
        if (sdt != "")
        {
            this.lblCallWholesaleMsg.Text = sdt;
        }
        sdt = _dataSource.selectBillingDatefromCallTemp("HostedImportTollRetail");
        if (sdt != "")
        {
            this.lblCallRetailMsg.Text = sdt;
        }
        dt = _dataSource.selectBillingDatefromTemp("HostedImportTaxes");
        if (dt > nodate)
        {
            this.lblTaxesMsg.Text = String.Format("{0:yyyy/MM/dd}", dt);
        }

    }
    private void loadFileNames(bool downloadFiles = false)
    {
      ImportSourceConfiguration source = _importSources[_importSource];

      bool hasError = false;
      string datepart = txtBillDate.Value.ToString("yyyyMM");
      // now populate each foldername and check to see if it is there
      int iFile = 0;
      foreach (ImportfileType fileType in source.FileTypes)
      {
        string prefix = fileType.Prefix;
        string suffix = fileType.Suffic;
        string datepartend = string.Empty;
        //if (useNewImport && iFile < 6)
        //  datepartend = "01";
        string filename = string.Format("{0}{1}{2}.{3}", prefix, datepart, datepartend, suffix);
        string filepath = string.Format("{0}\\{1}", _basePath, filename);
        string textbox = "txtFileName" + (iFile + 1).ToString();
        TextBox txt = findTextBox(textbox);
        if (txt == null)
        {
          MessageBox.Show("Error in getting filenames, bad text box name");
          return;
        }
        if (File.Exists(filepath))
        {
          txt.Text = filepath;
          txt.Refresh();
        }
        else
        {
          if (downloadFiles && source.FTPInfo != null)
          {
            /*
             * TODO: right now, only Saddleback files can be ftp downloaded, and there are some 
             * Saddleback hard coded entries in this section. This works for now, but if
             * we start ftping red rock files or get another source that needs ftp downloads,
             * then some of this section will need to be tweaked
             */
            #region ftp download
            SourceFTPInfo ftpInfo = source.FTPInfo;

            string uri = ftpInfo.Uri;
            string uid = ftpInfo.Uid;
            string pw = ftpInfo.Pw;
            ACG.Common.ACGFtp ftp = new ACG.Common.ACGFtp();
            if (iFile < source.FileTypes.Count - 1)  // last two files are tax and ledger and they are not on the ftp site 10-31-2015 supposedly the tax file will be there so I add it
            {
              txt.Text = "Downloading...";
              txt.ForeColor = Color.Blue;
              txt.Refresh();
              try
              {
                string firstLine = string.Empty;
                if (iFile == 2 || iFile == 3) // OCC files don't have header row
                  firstLine = @"Customer,MasterBTN,BTN,Date,Amount,Code,USOC,Service,BillDate";
                ftp.ftpGetFile(uri, uid, pw, filename, _basePath, fileType.skiplines, firstLine);
                txt.Text = filepath;
                txt.ForeColor = Color.Black;
                txt.Refresh();
              }
              catch (Exception ex)
              {
                MessageBox.Show(ex.Message);
                hasError = true;
                break;
              }
            }
            else
            {
              if (iFile < source.FileTypes.Count - 1 || ckRequireLedger.Checked)
                hasError = true;

              txt.Text = FILENOTFOUNDPREFIX + filepath;
              txt.ForeColor = Color.Red;
              txt.Refresh();
            }
            #endregion
          }
          else
          {
            if (iFile < source.FileTypes.Count - 1 || ckRequireLedger.Checked)
              hasError = true;

            txt.Text = FILENOTFOUNDPREFIX + filepath;
            txt.ForeColor = Color.Red;
            txt.Refresh();
          }
        }
        iFile++;
      }
      if (hasError && !_importLedgerOnly)
        MessageBox.Show("Some filenames were not found");
      else
      {
        btnReloadFiles.Enabled = true;
        btnImport.Enabled = true;
        hasValidFiles = true;
      }
    }
    private void loadImportFileTypes()
    {
      DataSet sources = _dataSource.getImportSources();
      foreach (DataRow src in sources.Tables[0].Rows)
      {
        ImportSourceConfiguration source = new ImportSourceConfiguration();
        source.Source = (ImportSource)Enum.Parse(typeof(ImportSource), src["Source"].ToString());
        DataSet fileTypes = _dataSource.getImportFileTypes(source.Source.ToString());
        source.FileTypes = new List<ImportfileType>();
        foreach (DataRow row in fileTypes.Tables[0].Rows)
        {
          ImportfileType fileType = new ImportfileType();
          fileType.FileType = (ImportFileTypes)Enum.Parse(typeof(ImportFileTypes), CommonFunctions.CString(row["FileType"]));
          fileType.Prefix = CommonFunctions.CString(row["Prefix"]);
          fileType.Suffic = CommonFunctions.CString(row["Suffix"]);
          fileType.StoredProcedure = CommonFunctions.CString(row["StoredProcedure"]);
          fileType.skiplines = CommonFunctions.CInt(row["SkipLines"]);
          source.FileTypes.Add(fileType);
        }
        if (src["FtpSiteUrl"] != null)
        {
          source.FTPInfo = new SourceFTPInfo();
          source.FTPInfo.Uri = CommonFunctions.CString(src["FtpSiteUrl"]);
          source.FTPInfo.Uid = CommonFunctions.CString(src["FtpSiteUsername"]);
          source.FTPInfo.Pw = CommonFunctions.CString(src["FtpSitePassword"]);
        }
        _importSources.Add(source.Source, source);
      }

    }
    private void RefreshImportLog()
      {
        DataSet dsLog = _dataSource.getAcctImportLog();
        ACG.CommonForms.CommonFormFunctions.displayDataSetGrid(grdImportLog, dsLog);
      }
    #endregion

    #region old commented out code
    //string OtherCases(int FieldNum, string cellvalue)
    //{
    //    switch (FieldNum)
    //    {
    //        case 1: //Customer
    //        case 6: //Code
    //        case 7: //USOC
    //        case 8: //Description
    //            {
    //                string strcellvalue = cellvalue.Replace("'", "''");
    //                return strcellvalue;
    //            }
    //        case 2: //Master BTN
    //        case 3: //BTN
    //            {
    //              string strcellvalue = FixBTNs(cellvalue);
    //              return strcellvalue;
    //            }
    //        case 5: //Amount
    //            {
    //                return cellvalue;
    //            }
    //        case 4://DateTime   
    //            {
    //              cellvalue = ExcelDaysConvert(cellvalue).ToString();
    //              return cellvalue;
    //            }
    //        case 9://BillDate
    //            {
    //                cellvalue = ExcelDateConvert(cellvalue).ToString();
    //                if (Convert.ToString(this.txtBillDate.Value) != cellvalue)
    //                {
    //                  cellvalue = "Bad Date";
    //                  importfileinvaliddate = true;
    //                }
    //                return cellvalue;
    //            }
    //    }
    //    return cellvalue;
    //}

    //private string[] _saddlebackFileTypePrefixes = new string[] { "MRC WHLSL-", "MRC Retail-", "OCC WHLSL-", "OCC Retail-", "Toll WHLSL-", "Toll Retail-", "TAX-", "Ledger-" };
    //private string[] _redRockFileTypePrefixes = new string[] { "Wholesale MRC Red Rock-", "Retail MRC Red Rock-", "Wholesale OCC Red Rock-", "Retail OCC Red Rock-", "Wholesale Toll Red Rock-", "Retail Toll Red Rock-", "TAX Red Rock-", "Ledger Red Rock-" };
    //private string[] _fileTypePrefixes = new string[] { };
    //private string[] _fileTypeSuffixes = new string[] { };
    //private string[] _saddlebackFileTypeSuffixes = new string[] { "csv", "csv", "csv", "csv", "csv", "csv", "csv", "xls" };
    //private string[] _redRockFileTypeSuffixes = new string[] { "txt", "txt", "txt", "txt", "txt", "txt", "txt", "txt" };
    //private int[] _skipLines = new int[] { 4, 4, 4, 4, 0, 0, 0, 0 };
    //private string[] _fileTypeSuffixes = new string[] { "xls", "xls", "xls", "xls", "csv", "csv", "csv", "xls" };
    //string MRCCases(int FieldNum, string cellvalue, string MRCType, DateTime billdate)
    //{
    //    switch (FieldNum)
    //        {
    //            case 1: //Customer Acct# 
    //            case 2: //Customer Name
    //            case 5: //USOC
    //            case 6: //Product Description
    //                {
    //                    string strcellvalue = cellvalue.Replace("'", "''");
    //                    return strcellvalue;
    //                }
    //            case 3: //MasterBTN
    //            case 4: // BTN
    //                {
    //                  string strcellvalue = FixBTNs(cellvalue);
    //                  return strcellvalue;
    //                }
    //            case 7: //Qty
    //            case 8: //Price
    //            case 9: //Amount Billed
    //                {
    //                    return cellvalue;
    //                }
    //            case 10://Connection Date                            
    //                {
    //                    cellvalue = ExcelDateConvert(cellvalue).ToString();
    //                    return cellvalue;
    //                }
    //            case 11://Billing Date 
    //                {
    //                  if (cellvalue == "")
    //                  {
    //                      cellvalue = Convert.ToString(billdate);
    //                  }
    //                  else
    //                  {
    //                      cellvalue = ExcelDateConvert(cellvalue).ToString();
    //                      if (Convert.ToString(this.txtBillDate.Value) != cellvalue)
    //                      {
    //                        cellvalue = "Bad Date";
    //                        importfileinvaliddate = true;
    //                      }
    //                  }
    //                  return cellvalue;
    //                }
    //        }
    //    return cellvalue;
    //}
    //    private void importOldOtherWholesale()
    //    {
    //        string importtable;
    //        importtable = "hostedimportoccwholesale";
    //        DataAdapterContainer da = _dataSource.getMaintenanceAdapter(importtable, new Dictionary<string, string>());
    //        DataSet ds = da.DataSet;
    //        DataTable dt = ds.Tables[0];
    //        int Cnum = 0;
    //        int dtCnum = 0;
    //        int Rnum = 0;
    //        bool iscustomerrow = true;
    //        bool issubtotrow = false;
    //        string cellvalue;
    //        string customer;
    //        DataRow dr = dt.NewRow(); 
    //        for (Rnum = 6; Rnum <= ShtRange.Rows.Count - 4; Rnum++)
    //        {
    //            for (Cnum = 1; Cnum <= 7; Cnum++)
    //            {
    //                if ((ShtRange.Cells[Rnum, Cnum] as Excel.Range).Value2 != null)
    //                {
    //                    if (Cnum > 1 && Cnum < 6)
    //                    {
    //                        dtCnum = Cnum + 1;
    //                    }
    //                    else
    //                    {
    //                        dtCnum = Cnum;
    //                    }
    //                    if (Cnum == 1)
    //                    {
    //                        cellvalue = (ShtRange.Cells[Rnum, Cnum] as Excel.Range).Value2.ToString();
    //                        issubtotrow = cellvalue.Contains("Subtotal");
    //                        if (iscustomerrow == true)
    //                        {
    //                            dr[1] = cellvalue;
    //                        }
    //                        if (issubtotrow == false && iscustomerrow == false)
    //                        {
    //                            dr[2] = cellvalue;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (issubtotrow == false && iscustomerrow == false)
    //                        {
    //                        //     dr[dtCnum] = OtherCasesWholesale(Cnum, (ShtRange.Cells[Rnum, Cnum] as Excel.Range).Value2.ToString());
    //                        }
    //                    }
    //                }
    //                else
    //                {
    ////                    Rnum = ShtRange.Rows.Count;
    //                }
    //            }
    //            if (issubtotrow == false && iscustomerrow == false)
    //            {
    //                customer = Convert.ToString(dr[1]);
    //                dt.Rows.Add(dr);
    //                dr = dt.NewRow();
    //                dr[1] = customer;
    //            }
    //            else
    //            {
    //                if (iscustomerrow == true)
    //                {
    //                    iscustomerrow = false;
    //                }
    //                if (issubtotrow == true)
    //                {
    //                    iscustomerrow = true;
    //                    issubtotrow = false;
    //                }
    //            }
    //        }
    //        da.DataAdapter.Update(ds, importtable);
    //    }
    //string OLdOtherCasesWholesale(int FieldNum, string cellvalue)
    //{
    //    switch (FieldNum)
    //    {
    //        case 1: //Master BTN
    //        case 2: //BTN
    //            {
    //              string strcellvalue = FixBTNs(cellvalue);
    //              return strcellvalue;
    //            }
    //        case 5: //Service
    //        case 6: //USOC
    //            {
    //                string strcellvalue = cellvalue.Replace("'", "''");
    //                return strcellvalue;
    //            }
    //        case 4: //Amount
    //            {
    //                return cellvalue;
    //            }
    //        case 3://DateTime   
    //            {
    //                cellvalue = ExcelDateConvert(cellvalue).ToString();
    //                return cellvalue;
    //            }
    //    }
    //    return cellvalue;
    //}
    //private void importCalls(string calltype)
    //{
    //    string importtable;
    //    int Columns;
    //    int RowCount;
    //    if (calltype == "Wholesale")
    //    {
    //        importtable = "hostedimporttollwholesale";
    //    }
    //    else
    //    {
    //        importtable = "hostedimporttollretail";
    //    }
    //    DataAdapterContainer da = _dataSource.getMaintenanceAdapter(importtable, new Dictionary<string, string>());
    //    DataSet ds = da.DataSet;
    //    DataTable dt = ds.Tables[0];
    //    int Cnum = 0;
    //    int Rnum = 0;
    //    int TblCol = 0;
    //    //string cellvalue;
    //    //string holddate;

    //    if (calltype == "Wholesale")
    //    {
    //        Columns = 21;
    //        RowCount = ShtRange.Rows.Count;
    //    }
    //    else
    //    {
    //        Columns = 14;
    //        RowCount = ShtRange.Rows.Count - 1;
    //    }

    //    for (Rnum = 2; Rnum <= RowCount; Rnum++)
    //    {
    //        DataRow dr = dt.NewRow(); 
    //        for (Cnum = 1; Cnum <= Columns; Cnum++)
    //        {
    //            if ((ShtRange.Cells[Rnum, Cnum] as Excel.Range).Value2 != null)
    //                if (calltype == "Wholesale")
    //                {
    //                    dr[Cnum] = CallWholesaleCases(Cnum, (ShtRange.Cells[Rnum, Cnum] as Excel.Range).Value2.ToString());
    //                }
    //                else
    //                {
    //                  if (Cnum > 7)
    //                  {
    //                    TblCol = Cnum - 1;
    //                  }
    //                  else
    //                  {
    //                    TblCol = Cnum;
    //                  }
    //                  if (Cnum != 7 && Cnum != 8)
    //                  {
    //                    dr[TblCol] = CallRetailCases(Cnum, (ShtRange.Cells[Rnum, Cnum] as Excel.Range).Value2.ToString());
    //                  }
    //                  if (Cnum == 8)
    //                  {
    //                    string calldate = CallRetailCases(7,(ShtRange.Cells[Rnum, 7] as Excel.Range).Value2.ToString());
    //                    string calltime = (ShtRange.Cells[Rnum, 8] as Excel.Range).Value2.ToString();
    //                    if (calltime.Length < 6)
    //                    {
    //                      calltime = "00:" + calltime;
    //                    }
    //                    dr[7] = calldate.Substring(0,10) + " " + calltime;
    //                  }
    //                }
    //            else
    //                Rnum = ShtRange.Rows.Count;
    //        }
    //          dt.Rows.Add(dr);
    //    }
    //    da.DataAdapter.Update(ds,importtable);
    //}
    //string CallWholesaleCases(int FieldNum, string cellvalue)
    //{
    //    switch (FieldNum)
    //    {
    //        case 1: //Billing Month
    //        case 2: //Billing Year
    //        case 6: //Customer Number
    //        case 10: //From State
    //        case 11://Carrier ID
    //        case 12://Message Type
    //        case 13://OCPID
    //        case 14://Rate
    //        case 15://Rate Class
    //        case 16://Settlement Code
    //        case 17://To City
    //        case 18://To Reference
    //        case 20://To State
    //            {
    //                string strcellvalue = cellvalue.Replace("'", "''");
    //                return strcellvalue;
    //            }
    //        case 3: //BTN
    //        case 9: //From NUmber
    //        case 19://To Number
    //            {
    //              string strcellvalue = FixBTNs(cellvalue);
    //              return strcellvalue;
    //            }
    //        case 4: //Call Number
    //        case 5: //Charge
    //        case 8: //Duration
    //        case 21://Usage Account Code
    //            {
    //                return cellvalue;
    //            }
    //        case 7: //Date                            
    //            {
    //                cellvalue = ExcelDaysConvert(cellvalue).ToString();
    //                return cellvalue;
    //            }
    //    }
    //    return cellvalue;
    //}
    //string CallRetailCases(int FieldNum, string cellvalue)
    //{
    //    switch (FieldNum)
    //    {
    //        case 1: //Customer Number
    //        case 2: //Billing Year
    //        case 3: //Billing Month
    //        case 5: //Message Type
    //        case 11://To City
    //        case 12://To State
    //        case 13://Rate
    //            {
    //                string strcellvalue = cellvalue.Replace("'", "''");
    //                return strcellvalue;
    //            }
    //        case 4: //BTN
    //        case 6: //From Number
    //        case 10: //To Number
    //            {
    //              string strcellvalue = FixBTNs(cellvalue);
    //              return strcellvalue;
    //            }
    //        case 9: //Duration
    //        case 14://Charge
    //            {
    //                return cellvalue;
    //            }
    //        case 7: //Date                            
    //            {
    //                cellvalue = ExcelDateConvert(cellvalue).ToString();
    //                return cellvalue;
    //            }
    //        case 8: //Time
    //            {
    //                return cellvalue;
    //            }
    //    }
    //    return cellvalue;
    //}

    #endregion
  }
}