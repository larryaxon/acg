using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ACG.CommonForms;
using CCI.Common;
using CCI.Sys.Data;
using CCI.DesktopClient.Common;

namespace CCI.DesktopClient.Screens
{
  public partial class frmImportUSOCExceptions : ScreenBase
  {
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }

    private List<string> _dontDislayFields = new List<string>()
    {
      "LastModifiedDateTime","LastModifiedBy"
    };

    public frmImportUSOCExceptions()
    {
      InitializeComponent();
      load();
    }
    #region private methods
    private void load()
    {
      loadGrid();
      grdWholesaleExceptions.ReadOnly = true;
      grdWholesaleExceptions.AllowUserToAddRows = false;
      initEditPane(grdWholesaleExceptions.Columns);
    }
    private void loadGrid()
    {
      DataSet ds = _dataSource.getWholesaleExceptions();
      CCI.DesktopClient.Common.CommonFormFunctions.displayDataSetGrid(grdWholesaleExceptions, ds);
    }


    private void initEditPane(DataGridViewColumnCollection columns)
    {
      if (pnlEditPanel.Controls.Count > 0)
        return;
      pnlEditPanel.Controls.Clear();
      int margin = 5;
      int top = 5;
      int width = pnlEditPanel.Width;
      int left = margin;
      int controlDisplayWidth = width / 2;
      int labelWidth = Convert.ToInt32(Convert.ToDouble(controlDisplayWidth) * .35);
      int controlWidth = Convert.ToInt32(Convert.ToDouble(controlDisplayWidth) * .55);
      foreach (DataGridViewColumn col in columns)
      {
        if (_dontDislayFields.Contains(col.Name))
          continue;
        Label lbl = new Label();
        lbl.Name = "lbl" + col.Name;
        lbl.Text = col.Name;
        lbl.Width = labelWidth;
        if (top + lbl.Height + margin > pnlEditPanel.Height)
        {
          top = margin;
          left = left + margin + controlDisplayWidth;
        }
        lbl.Top = top;
        lbl.Left = left;
        pnlEditPanel.Controls.Add(lbl);
        Control ctl;
        switch (col.Name)
        {
          case "ID":
            ctl = new TextBox();
            ctl.Enabled = false;
            break;
          case "WholesaleUSOCToDelete":
          case "RetailUSOCToCopy":
          case "WholesaleUSOCToReplace":
            ctl = new ctlSearch();
            ((ctlSearch)ctl).SearchExec = new SearchDataSourceProductList();
            ((ctlSearch)ctl).Collapsed = true;
            ((ctlSearch)ctl).DisplayOnlyID = true;
            ((ctlSearch)ctl).MustExistInList = true;
            ((ctlSearch)ctl).ShowTermedCheckBox = false;
            ((ctlSearch)ctl).AddNewMode = false;
            ((ctlSearch)ctl).ShowCustomerNameWhenSet = true;
            ((ctlSearch)ctl).Height = lbl.Height;
            break;
          case "WholesaleCost":
            ctl = new NumericUpDown();
            ((NumericUpDown)ctl).DecimalPlaces = 2;
            break;
          case "StartDate":
          case "EndDate":
            ctl = new ctlACGDate();
            break;
          case "CustomerID":
            ctl = new ctlSearch();
            ((ctlSearch)ctl).SearchExec = new SearchDataSourceCustomer();
            ((ctlSearch)ctl).DisplayOnlyID = false;
            ((ctlSearch)ctl).MustExistInList = true;
            ((ctlSearch)ctl).ShowTermedCheckBox = false;
            ((ctlSearch)ctl).AddNewMode = false;
            ((ctlSearch)ctl).ShowCustomerNameWhenSet = true;
            ((ctlSearch)ctl).Collapsed = true;
            ((ctlSearch)ctl).Height = lbl.Height;
            break;
          default:
            ctl = new TextBox();
            break;
        }
        ctl.Name = "ctl" + col.Name;
        ctl.Text = string.Empty;
        ctl.Width = controlWidth;
        ctl.Top = top;
        ctl.Left = left + lbl.Width + margin;
        top = top + ctl.Height + margin; // now move the next control to the next line
        pnlEditPanel.Controls.Add(ctl);
        if (ctl.GetType().Name == "ctlSearch")
        {
          ((ctlSearch)ctl).Collapsed = true;
          ((ctlSearch)ctl).Refresh();
        }
      }
      // add the buttons
      top = margin;
      left = left + margin + controlDisplayWidth;
      Button save = new Button();
      save.Name = "btnSave";
      save.Text = "Save";
      save.Top = top;
      save.Left = left;
      save.Click += Save_Click;
      top = top + save.Height + margin;
      Button newPane = new Button();
      newPane.Name = "bthNew";
      newPane.Text = "New";
      newPane.Top = top;
      newPane.Left = left;
      newPane.Click += NewPane_Click;
      pnlEditPanel.Controls.Add(save);
      save.BringToFront();
      pnlEditPanel.Controls.Add(newPane);
      newPane.BringToFront();
      pnlEditPanel.Refresh();
    }


    private void clearEditPane()
    {
      foreach (Control ctl in pnlEditPanel.Controls)
      {
        if (ctl.Name.StartsWith("ctl")) // don't mess with the labels
        {
          if (ctl.GetType().Name == "ctlSearch")
          {
            ((ctlSearch)ctl).Text = string.Empty;
            ((ctlSearch)ctl).AddNewMode = false;
          }
          else
            ctl.Text = string.Empty;
        }
      }
    }
    private void leadEditPane(DataGridViewRow row)
    {
      if (row == null)
        return;
      clearEditPane();
      foreach (Control ctl in pnlEditPanel.Controls)
      {
        if (ctl.Name.StartsWith("ctl")) // don't mess with the labels
        {
          string cellname = ctl.Name.Substring(3);
          DataGridViewCell cell = row.Cells[cellname];
          object val = cell.Value;
          if (val == null)
            ctl.Text = string.Empty;
          else
          {
            if (ctl.GetType().Name.Equals("ctlSearch"))
              ((ctlSearch)ctl).Text = val.ToString();
            //else if (ctl.Name.Equals("ctlWholesaleCost"))
            //  ctl.Text = ((decimal)val).ToString("0.00");
            else
              ctl.Text = val.ToString();
          }
        }
      }
    }
    private void saveEditPane()
    {
      string sTest = pnlEditPanel.Controls["ctlID"].Text;
      int? id;
      string wholesaleUsoc, retailUsoc, wholesaleReplaceUsoc, customerId;
      decimal cost;
      DateTime? startDate, endDate;
      if (string.IsNullOrEmpty(sTest))
        id = null;
      else
        id = Convert.ToInt32(sTest);
      wholesaleUsoc = ((ctlSearch)pnlEditPanel.Controls["ctlWholesaleUSOCToDelete"]).Text;
      retailUsoc = ((ctlSearch)pnlEditPanel.Controls["ctlRetailUSOCToCopy"]).Text;
      wholesaleReplaceUsoc = ((ctlSearch)pnlEditPanel.Controls["ctlWholesaleUSOCToReplace"]).Text;
      sTest = pnlEditPanel.Controls["ctlStartDate"].Text;
      if (string.IsNullOrEmpty(sTest))
        startDate = null;
      else
        startDate = Convert.ToDateTime(sTest);
      sTest = pnlEditPanel.Controls["ctlEndDate"].Text;
      if (string.IsNullOrEmpty(sTest))
        endDate = null;
      else
        endDate = Convert.ToDateTime(sTest);
      sTest = ((ctlSearch)pnlEditPanel.Controls["ctlCustomerID"]).Text;
      if (string.IsNullOrEmpty(sTest))
        customerId = null;
      else
        customerId = sTest;
      sTest = pnlEditPanel.Controls["ctlWholesaleCost"].Text;
      if (string.IsNullOrEmpty(sTest))
        cost = 0;
      else
        cost = Convert.ToDecimal(sTest);
      if (cost == 0 || string.IsNullOrEmpty(wholesaleUsoc) || string.IsNullOrEmpty(retailUsoc))
      {
        MessageBox.Show("You must have a whosale usoc, a retail usoc, and a wholesale cost to save");
        return;
      }
      _dataSource.saveWholesaleExceptions(id, wholesaleUsoc, retailUsoc, wholesaleReplaceUsoc, cost, startDate, endDate, customerId, SecurityContext.User);
      clearEditPane();
      loadGrid();
    }
    #endregion
    #region form events
    private void grdWholesaleExceptions_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      DataGridViewRow row = grdWholesaleExceptions.CurrentRow;
      leadEditPane(row);
    }
    private void grdWholesaleExceptions_RowEnter(object sender, DataGridViewCellEventArgs e)
    {

    }

    private void Clear_Click(object sender, EventArgs e)
    {
      clearEditPane();
    }

    private void Save_Click(object sender, EventArgs e)
    {
      saveEditPane();
    }
    private void NewPane_Click(object sender, EventArgs e)
    {
      clearEditPane();
      //ctlSearch srch = (ctlSearch)pnlEditPanel.Controls["ctlWholesaleUSOCToDelete"];
      //srch.MustExistInList = true;
      //srch = (ctlSearch)pnlEditPanel.Controls["ctlRetailUSOCToCopy"];
      //srch.AddNewMode = true;
      //srch = (ctlSearch)pnlEditPanel.Controls["ctlWholesaleUSOCToReplace"];
      //srch.AddNewMode = true;
      //srch = (ctlSearch)pnlEditPanel.Controls["ctlCustomerID"];
      //srch.AddNewMode = true;
    }

    #endregion
  }
}
