using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ACG.App.Common;
using ACG.DesktopClient.Common;
using ACG.DesktopClient.Reports;

using ACG.Common;
using ACG.Sys.Data;

namespace ACG.DesktopClient.Screens
{
  public partial class frmInvoicePrint : ScreenBase
  {
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    private string _customer { get { return cboCustomer.Text; } set { cboCustomer.Text = value; } }
    private string _resource { get { return cboResource.Text; } set { cboResource.Text = value; } }
    private string _project { get { return cboProject.Text; } set { cboProject.Text = value; } }
    public frmInvoicePrint()
    {
      InitializeComponent();
    }

    #region form events
    private void frmInvoicePrint_Load(object sender, EventArgs e)
    {
      loadCombos();
      refreshProject();
      srchInvoices.NameType = ACGCommonData.NameTypes.BilledInvoices;
      srchInvoices.Load(ACGCommonData.NameTypes.BilledInvoices, "Billed Invoices");
      srchInvoices.ReLoad();
    }
    private void cboCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
      refreshProject();
      Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
      criteria.Add(ACGCommonData.fieldCUSTOMERID, new string[] { ctlSearchGrid.opEQUALS, cboCustomer.Text });
      srchInvoices.SearchCriteria = criteria;
      srchInvoices.ReLoad();
    }
    private void btnPreviewInvoice_Click(object sender, EventArgs e)
    {
      printReport();
    }
    private void btnPostInvoice_Click(object sender, EventArgs e)
    {
      printReport();
      postReport(true);
    }
    private void btnUnpost_Click(object sender, EventArgs e)
    {
      postReport(false);
      ckIncludePosted.Checked = false;
      printReport();
    }
    private void ckInvoiceOnlyThisProject_CheckedChanged(object sender, EventArgs e)
    {
      // you can't have both of these checked at the same time
      if (ckInvoiceOnlyThisCustomer.Checked)
        ckExcludeProject.Checked = false;
    }
    private void ckExcludeProject_CheckedChanged(object sender, EventArgs e)
    {
      // you can't have both of these checked at the same time
      if (ckExcludeProject.Checked)
        ckInvoiceOnlyThisProject.Checked = false;
    }
    #endregion

    #region private methods
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
          }
          CommonFormFunctions.populatePickList(ctl, (ACGTable)((ACGFormItem)picklist).Value, val);
        }
        if (!SecurityContext.Security.HasObjectAccess("CanEnterOthersTime"))
          cboResource.Enabled = false;
      }
    }
    private void refreshProject()
    {
      Hashtable context = new Hashtable();
      context.Add(ACGCommonData.fieldCUSTOMERID, cboCustomer.Text);
      ACGForm picklists = _dataSource.getDSPickLists(context, new ArrayList() { "project" }, string.Empty, SecurityContext.User, string.Empty);
      CommonFormFunctions.populatePickList(cboProject, (ACGTable)((ACGFormItem)picklists[0]).Value, null);
    }
    private void printReport()
    {
      string printfilename = string.Empty;
      OpenFileDialog file = new OpenFileDialog();
      file.CheckFileExists = false; // this can be a new file so don't enforce that it exists
      file.AddExtension = true;
      file.Filter = "pdf files (*.pdf)|*.pdf";
      file.DefaultExt = "pdf";
      if (file.ShowDialog() == DialogResult.OK)
      {
        printfilename = file.FileName;

        ACGInvoice rpt = new ACGInvoice(ckDetail.Checked);
        rpt.ThroughDate = dtInvoiceThrough.Value;
        if (ckInvoiceOnlyThisCustomer.Checked)
          rpt.Customer = _customer;
        else
          rpt.Customer = null;
        if (ckInvoiceOnlyThisProject.Checked)
          rpt.Project = _project;
        else
          rpt.Project = null;
        if (ckInvoiceOnlyThisResource.Checked)
          rpt.Resource = _resource;
        else
          rpt.Resource = null;
        rpt.FromDate = CommonData.PastDateTime;
        rpt.IncludeUnposted = ckIncludePosted.Checked;
        rpt.ExcludeProject = ckExcludeProject.Checked;
        rpt.PrintToFile(printfilename);
        Process.Start(printfilename);
      }

    }

    private void printDetailReport()
    {
        string printfilename = string.Empty;
        OpenFileDialog file = new OpenFileDialog();
        file.CheckFileExists = false; // this can be a new file so don't enforce that it exists
        file.AddExtension = true;
        file.Filter = "pdf files (*.pdf)|*.pdf";
        file.DefaultExt = "pdf";
        if (file.ShowDialog() == DialogResult.OK)
        {
            printfilename = file.FileName;

            ACGInvoice rpt = new ACGInvoice(ckDetail.Checked);
            rpt.ThroughDate = dtInvoiceThrough.Value;
            if (ckInvoiceOnlyThisCustomer.Checked)
                rpt.Customer = _customer;
            else
                rpt.Customer = null;
            if (ckInvoiceOnlyThisProject.Checked)
                rpt.Project = _project;
            else
                rpt.Project = null;
            if (ckInvoiceOnlyThisResource.Checked)
                rpt.Resource = _resource;
            else
                rpt.Resource = null;
            rpt.FromDate = CommonData.PastDateTime;
            rpt.IncludeUnposted = ckIncludePosted.Checked;
            rpt.ExcludeProject = ckExcludeProject.Checked;
            rpt.PrintToFile(printfilename);
            Process.Start(printfilename);
        }
    }

    private void postReport(bool post)
    {
      if (!ckInvoiceOnlyThisCustomer.Checked) // for now, this does not work
        MessageBox.Show("We do not support posting of more than one invoice at a time at this time");
      else
      {
        int invoiceNumber = _dataSource.getLastInvoiceNumber() + 1;
        _dataSource.postTimeInvoice(_resource, _customer, _project, ckInvoiceOnlyThisResource.Checked, ckInvoiceOnlyThisCustomer.Checked, ckInvoiceOnlyThisProject.Checked,
          CommonData.PastDateTime, dtInvoiceThrough.Value, post, invoiceNumber);
        srchInvoices.ReLoad();
      }
    }
    private void setScreenValuesFromRow(DataGridViewRow row)
    {
      DateTime invoiceDate = CommonFunctions.CDateTime(row.Cells["InvoiceDate"].Value);
      string customerid = CommonFunctions.CString(row.Cells[ACGCommonData.fieldCUSTOMERID].Value);
      cboCustomer.Text = customerid;
      dtInvoiceThrough.Value = invoiceDate;
      ckInvoiceOnlyThisCustomer.Checked = true;
      ckIncludePosted.Checked = true;
      ckInvoiceOnlyThisProject.Checked = false;
      ckInvoiceOnlyThisResource.Checked = false;
      ckExcludeProject.Checked = false;
    }
    #endregion

    private void srchInvoices_RowSelected(object sender, MaintenanceGridRowSelectedArgs e)
    {
      setScreenValuesFromRow(e.SelectedRow);
    }

    private void btnPreviewDetail_Click(object sender, EventArgs e)
    {
        printDetailReport();
    }

  }
}
