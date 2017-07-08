using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using CCI.Sys.Data;
using CCI.DesktopClient.Common;

namespace CCI.DesktopClient.Screens
{
  public partial class frmCityHostedUSOCMaintenanceNew : ScreenBase
  {
    #region module data

    private const string colWholesaleUSOC = "usocwholesale";
    private const string colWholesaleMRC = "mrcwholesale";
    private const string colWholesaleNRC = "nrcwholesale";
    private const string colWholesaleStartDate = "wholesalestartdate";
    private const string colWholesaleEndDate = "wholesaleenddate";
    private const string colWholesaleOnly = "wholesaleonly";
    private const string colRetailUSOC = "usocretail";
    private const string colRetailMRC = "mrcretail";
    private const string colRetailNRC = "nrcretail";
    private const string colRetailDescription = "descriptionretail";
    private const string colExternalDescription = "RIT Retail Bill Presentation";
    private const string colRetailStartDate = "retailstartdate";
    private const string colRetailEndDate = "retailenddate";
    private const string colRetailOnly = "retailonly";
    private const string colWholesaleDescription = "descriptionwholesale";
    private const string colDQCategory = "DQCategory";
    private const string colDealerQuote = "DealerQuoteYN";
    private const string colIsRecommended = "DQLessScreenOnly";
    private const string colUseMRC = "UseMRCinDQ";
    private const string colPrimaryCarrier = "PrimaryCarrier";

    private const string colIsSaddleback = "IsSaddlebackUSOC";
    private const string colRetailActive = "retailactive";
    private const string colWholesaleActive = "wholesaleactive";
    private const string colExcludeFromExceptions = "ExcludeFromExceptions";

    bool? _lastDealerQuoteYNValue = null;

    private bool _firstTime = true;

    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    #endregion
    public frmCityHostedUSOCMaintenanceNew()
    {
      InitializeComponent();
      srchUSOCList.CanChangeDisplayFields = true;

    }
    #region public methods

    #endregion

    #region private methods

    private void clear()
    {
      selectMode(cboMode.Text);
      selectCarrier(cboCarrier.Text);
      reloadUsocList(true);
      clearPanel(pnlWholesale);
      clearPanel(pnlRetail);
      clearPanel(pnlMatching);
      initMatchingPanel();
    }
    private void clearPanel(Panel panel)
    {
      foreach (Control c in panel.Controls)
      {
        if (c.GetType() != typeof(Label))
          c.ResetText();
      }
    }
    private void initMatchingPanel()
    {

    }
    private void refreshPickLists()
    {

    }
    private void selectMode(string mode)
    {
      switch (mode)
      {
        case "Edit Wholesale":
          selectWholesaleRetail("wholesale");
          pnlWholesale.Visible = true;
          pnlRetail.Visible = false;
          pnlMatching.Visible = false;
          pnlWholesale.Dock = DockStyle.Fill;
          break;
        case "Edit Retail":
          selectWholesaleRetail("retail");
          pnlWholesale.Visible = false;
          pnlRetail.Visible = true;
          pnlMatching.Visible = false;
          pnlRetail.Dock = DockStyle.Fill;
          break;
        case "Match Retail/ Wholesale":
          pnlWholesale.Visible = false;
          pnlRetail.Visible = false;
          pnlMatching.Visible = true;
          pnlMatching.Dock = DockStyle.Fill;
          break;
      }
    }
    private void selectWholesaleRetail(string wholesaleRetail)
    {
      if (wholesaleRetail.Equals("retail", StringComparison.CurrentCultureIgnoreCase))
      {
        // show aall records with retail usocs
        addCriteria(colRetailUSOC, ctlSearchGrid.opISNOTNULL, null);
      }
      else if (wholesaleRetail.Equals("wholesale", StringComparison.CurrentCultureIgnoreCase))
      {
        // show all records with retail usocs
        addCriteria(colWholesaleUSOC, ctlSearchGrid.opISNOTNULL, null);
      }
      else
      {
        // show both wholesale and retail usocs
        Dictionary<string, string[]> criteria = removeCriteria(colRetailUSOC, false); // since we have two operations save the first and dont refresh
        // now pass in the new criteria
        removeCriteria(colWholesaleUSOC, true, criteria); // and then wait till the last to refresh the screen
      }
    }
    private void selectCarrier(string carrier)
    {
      addCriteria(colPrimaryCarrier, ctlSearchGrid.opEQUALS, carrier);

    }
    private void reloadUsocList(bool resetLastRowSelected)
    {
      if (ckRefresh.Checked)
      {
        srchUSOCList.ReLoad(resetLastRowSelected);
      }
    }
    private Dictionary<string, string[]> addCriteria(string criteriaName, string op, string value, bool refresh = true, Dictionary<string, string[]> criteria = null)
    {
      if (refresh && criteria == null)
      {
        criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
        foreach (KeyValuePair<string, string[]> entry in srchUSOCList.SearchCriteria)
          criteria.Add(entry.Key, entry.Value);
      }
      string[] val = new string[] { op, value };
      if (criteria.ContainsKey(criteriaName))
        criteria[criteriaName] = val;
      else
        criteria.Add(criteriaName, val);
      if (refresh)
      {
        srchUSOCList.SearchCriteria = criteria;
        reloadUsocList(true);
      }
      return criteria;
    }
    private Dictionary<string, string[]> removeCriteria(string criteriaName, bool refresh = true, Dictionary<string, string[]> criteria = null)
    {
      if (refresh && criteria == null)
      {
        criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
        foreach (KeyValuePair<string, string[]> entry in srchUSOCList.SearchCriteria)
          criteria.Add(entry.Key, entry.Value);
      }
      if (criteria.ContainsKey(criteriaName))
        criteria.Remove(criteriaName);
      if (refresh)
      {
        srchUSOCList.SearchCriteria = criteria;
        reloadUsocList(true);
      }
      return criteria;
    }
    private void loadRetailFields(DataGridViewRow row)
    {
      if (row == null)
        return;

      clear();


      #region Retail data
      txtExternalDescription.Text = CommonFunctions.CString(getCellValueFromColumnHeader(row.Cells,colExternalDescription));
      cboRITCategory.Text = CommonFunctions.CString(getCellValueFromColumnHeader(row.Cells,"RIT Category"));
      cboCHSCategory.Text = CommonFunctions.CString(getCellValueFromColumnHeader(row.Cells,"CHSCategory"));
      string amountText = CommonFunctions.CString(getCellValueFromColumnHeader(row.Cells,colRetailMRC));
      if (amountText.Equals("Variable", StringComparison.CurrentCultureIgnoreCase))
      {
        ckVariableRetailMRC.Checked = true;
        txtRetailMRC.Text = string.Empty;
      }
      else
      {
        ckVariableRetailMRC.Checked = false;
        txtRetailMRC.Text = amountText;
      }
      amountText = CommonFunctions.CString(getCellValueFromColumnHeader(row.Cells,colRetailNRC));
      if (amountText.Equals("Variable", StringComparison.CurrentCultureIgnoreCase))
      {
        ckVariableRetailNRC.Checked = true;
        txtRetailNRC.Text = string.Empty;
      }
      else
      {
        ckVariableRetailNRC.Checked = false;
        txtRetailNRC.Text = amountText;
      }
      DateTime? dt = getDateValue(getCellValueFromColumnHeader(row.Cells,colRetailStartDate));
      if (dt == null)
        dtRetailStartDate.Checked = false;
      else
        dtRetailStartDate.Value = (DateTime)dt;
      dt = getDateValue(getCellValueFromColumnHeader(row.Cells,colRetailEndDate));
      if (dt == null)
        dtRetailEndDate.Checked = false;
      else
        dtRetailEndDate.Value = (DateTime)dt;
      ckRetailInactivate.Checked = !((!dtRetailStartDate.Checked || DateTime.Today >= dtRetailStartDate.Value) &&
        (!dtRetailEndDate.Checked || DateTime.Today <= dtRetailEndDate.Value));

      ckRetailOnly.Checked = CommonFunctions.CBoolean(getCellValueFromColumnHeader(row.Cells,colRetailOnly));
      ckExcludeFromExceptions.Checked = CommonFunctions.CBoolean(getCellValueFromColumnHeader(row.Cells,colExcludeFromExceptions));
      cboTaxCode.Text = CommonFunctions.CString(getCellValueFromColumnHeader(row.Cells,"RIT TranTax"));
      ckSaddlebackUSOC.Checked = CommonFunctions.CBoolean(getCellValueFromColumnHeader(row.Cells,"IsSaddlebackUSOC"));
      #endregion

      // final fixup (set item search so they require an existing time
      txtRetailUSOC.AddNewMode = false; // you cannot enter a new nonexisting usoc
      if (!string.IsNullOrEmpty(txtWholesaleUSOC.Text))
        txtWholesaleUSOC.AddNewMode = false; // same
      else
        txtWholesaleUSOC.AddNewMode = true;
    }
    private void loadWholesaleFields(DataGridViewRow row)
    {
      if (row == null)
        return;

      clear();
      string wholesaleUSOC = txtWholesaleUSOC.Text = CommonFunctions.CString(getCellValueFromColumnHeader(row.Cells,colWholesaleUSOC)).Trim();
      #region Wholesale data
      if (string.IsNullOrEmpty(wholesaleUSOC))
        txtWholesaleUSOC.AddNewMode = true;
      else
        txtWholesaleUSOC.AddNewMode = false;
      txtWholesaleMRC.Text = CommonFunctions.CString(getCellValueFromColumnHeader(row.Cells,colWholesaleMRC));
      txtWholesaleNRC.Text = CommonFunctions.CString(getCellValueFromColumnHeader(row.Cells,colWholesaleNRC));
      DateTime? dt = getDateValue(getCellValueFromColumnHeader(row.Cells,colWholesaleStartDate));
      if (dt == null)
        dtWholesaleStartDate.Checked = false;
      else
        dtWholesaleStartDate.Value = (DateTime)dt;
      dt = getDateValue(getCellValueFromColumnHeader(row.Cells,colWholesaleEndDate));
      if (dt == null)
        dtWholesaleEndDate.Checked = false;
      else
        dtWholesaleEndDate.Value = (DateTime)dt;
      ckWholesaleInactivate.Checked = !((!dtWholesaleStartDate.Checked || DateTime.Today >= dtWholesaleStartDate.Value)
        && (!dtWholesaleEndDate.Checked || DateTime.Today <= dtWholesaleEndDate.Value));
      ckWholesaleOnly.Checked = CommonFunctions.CBoolean(getCellValueFromColumnHeader(row.Cells,colWholesaleOnly));
      //ckIsNotWholesaleSaddlebackUSOC.Checked = !CommonFunctions.CBoolean(GetCellValueFromColumnHeader(row.Cells,colIsSaddleback));
      Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
      criteria.Add("USOCWholesale", new string[] { ctlSearchGrid.opEQUALS, wholesaleUSOC });
      #endregion

      // final fixup (set item search so they require an existing time
      txtRetailUSOC.AddNewMode = false; // you cannot enter a new nonexisting usoc
      if (!string.IsNullOrEmpty(txtWholesaleUSOC.Text))
        txtWholesaleUSOC.AddNewMode = false; // same
      else
        txtWholesaleUSOC.AddNewMode = true;
    }
    private DateTime? getDateValue(object o)
    {
      if (o == null)
        return null;
      if (o.Equals(System.DBNull.Value))
        return null;
      if (o.GetType() != typeof(DateTime))
        return null;
      DateTime dt = (DateTime)o;
      if (dt == CommonData.PastDateTime || dt == CommonData.FutureDateTime)
        return null;
      return dt;
    }
    private object getCellValueFromColumnHeader(DataGridViewCellCollection cells, string name)
    {
      IEnumerable<DataGridViewCell> notempty = cells.Cast<DataGridViewCell>().Where(c => c.Value != null);
      DataGridViewCell cell = cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.OwningColumn.Name.Equals(name, StringComparison.CurrentCulture));
      if (cell == null)
        return null;
      return cell.Value;
    }
    #endregion

    #region form events
    private void frmCityHostedUSOCMaintenanceNew_Load(object sender, EventArgs e)
    {
      srchUSOCList.Init(CCI.Common.CommonData.UnmatchedNameTypes.CityHostedUSOCs, null);
      srchUSOCList.ColumnOrderSaveEnabled = false;
      ckIncludeInactive_CheckedChanged(this, new EventArgs()); // force load to use inactive flag
      //reloadUsocList(true);
      refreshPickLists();
      srchUSOCList.ColumnOrderSaveEnabled = true;
      clear();
    }

    private void ckRefresh_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void ckIncludeInactive_CheckedChanged(object sender, EventArgs e)
    {

    }
    #endregion

    private void cboMode_SelectedIndexChanged(object sender, EventArgs e)
    {
      string mode = cboMode.SelectedItem.ToString();
      selectMode(mode);

    }

    private void cboCarrier_SelectedIndexChanged(object sender, EventArgs e)
    {
      string carrier = cboCarrier.SelectedItem.ToString();
      selectCarrier(carrier);
    }

    private void srchUSOCList_RowSelected(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      string mode = cboMode.Text;
      if (cboMode.Text.Equals("Edit Wholesale"))
        loadWholesaleFields(e.SelectedRow);
      else
        if (cboMode.Text.Equals("Edit Retail"))
        loadRetailFields(e.SelectedRow);

      /*
       * 

Match Retail / Wholesale
*/
    }
  }
}
