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
      txtRetailUSOC.SearchExec = new SearchDataSourceProductList();
      txtWholesaleUSOC.SearchExec = new SearchDataSourceProductList();
      ((SearchDataSourceProductList)txtRetailUSOC.SearchExec).Carrier = "CityHosted";
      ((SearchDataSourceProductList)txtWholesaleUSOC.SearchExec).Carrier = "Saddleback";
      srchUSOCList.CanChangeDisplayFields = true;

    }
    #region public methods

    #endregion

    #region private methods

    private void clear(bool reloadGrid = false)
    {
      if (reloadGrid)
      {
        selectMode(cboMode.Text);
        selectCarrier(cboCarrier.Text);

        reloadUsocList(true);
      }
      clearTabPage(tabWholesale);
      clearTabPage(tabRetail);
      clearTabPage(tabMatching);
      initMatchingTab();
    }
    private void clearTabPage(TabPage tab)
    {
      foreach (Control c in tab.Controls)
      {
        Type cType = c.GetType();
        if (cType != typeof(Label))
        {
          if (cType == typeof(CheckBox))
            ((CheckBox)c).Checked = false;
          else if (cType == typeof(TextBox) || cType == typeof(ComboBox) || cType == typeof(ACG.CommonForms.ctlSearch))
            c.Text = string.Empty;
        }
      }
    }
    private void initMatchingTab()
    {
      lstRetailUsocs.Items.Clear();
      lstRetailUsocs.Items.AddRange(_dataSource.getRetailUsocs(cboCarrier.Text));
      lstWholesaleUsocs.Items.Clear();
      lstWholesaleUsocs.Items.AddRange(_dataSource.getWholesaleUsocs(cboCarrier.Text));
    }
    private void refreshPickLists()
    {
      cboCarrier.Items.Clear();
      cboCarrier.Items.AddRange(_dataSource.getPrimaryCarriers());

    }
    private void selectMode(string mode)
    {
      switch (mode)
      {
        case "Edit Wholesale":
          selectWholesaleRetail("wholesale");
          tabMaintenance.SelectedTab = tabWholesale;
          //pnlRetail.Visible = false;
          //pnlMatching.Visible = false;
          //pnlWholesale.Dock = DockStyle.Fill;
          //pnlWholesale.BringToFront();
          //pnlWholesale.Visible = true;
          break;
        case "Edit Retail":
          selectWholesaleRetail("retail");
          tabMaintenance.SelectedTab = tabRetail;
          //pnlWholesale.Visible = false;
          //pnlMatching.Visible = false;
          //pnlRetail.Dock = DockStyle.Fill;
          //pnlRetail.BringToFront();
          //pnlRetail.Visible = true;
          break;
        default:
          tabMaintenance.SelectedTab = tabMatching;
          //pnlWholesale.Visible = false;
          //pnlRetail.Visible = false;
          //pnlMatching.Dock = DockStyle.Fill;
          //pnlMatching.BringToFront();
          //pnlMatching.Visible = true;
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
    /// <summary>
    /// Add a criteria to the search grid. Has tow modes:
    /// with just the first three arguments, it will add the criteria to the list and then refresh the grid.
    /// With refresh = false it will not refresh the grid but it will return the criteria as a Dictionary
    /// so you can ad serveral in a rown without refreshing in between. This is faster and does not have
    /// the "screen flash" effect
    /// </summary>
    /// <param name="criteriaName">Namne of the criteria</param>
    /// <param name="op">operator for the criteria</param>
    /// <param name="value">value of the criteria. Note some operators, like ISNULL, will ignore this and you 
    /// can just use null. </param>
    /// <param name="refresh">Refresh the grod after adding the criteria</param>
    /// <param name="criteria"Optional: you can pass in a criteria dictionary from a prior
    /// invocation so you can "chain" them without refresh.></param>
    /// <returns></returns>
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
    /// <summary>
    /// REmoves a criteria. See addCriteria not4s for more details on the two modes.
    /// </summary>
    /// <param name="criteriaName"></param>
    /// <param name="refresh"></param>
    /// <param name="criteria"></param>
    /// <returns></returns>
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
      txtRetailUSOC.Text = CommonFunctions.CString(row.Cells[colRetailUSOC].Value);
      txtRetailDescription.Text = CommonFunctions.CString(row.Cells[colRetailDescription].Value);
      txtExternalDescription.Text = CommonFunctions.CString(row.Cells[colExternalDescription].Value);
      cboRITCategory.Text = CommonFunctions.CString(row.Cells["RIT Category"].Value);
      cboCHSCategory.Text = CommonFunctions.CString(row.Cells["CHSCategory"].Value);
      string amountText = CommonFunctions.CString(row.Cells[colRetailMRC].Value);
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
      amountText = CommonFunctions.CString(row.Cells[colRetailNRC].Value);
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
      DateTime? dt = getDateValue(row.Cells[colRetailStartDate].Value);
      if (dt == null)
        dtRetailStartDate.Checked = false;
      else
        dtRetailStartDate.Value = (DateTime)dt;
      dt = getDateValue(row.Cells[colRetailEndDate].Value);
      if (dt == null)
        dtRetailEndDate.Checked = false;
      else
        dtRetailEndDate.Value = (DateTime)dt;
      ckRetailInactivate.Checked = !((!dtRetailStartDate.Checked || DateTime.Today >= dtRetailStartDate.Value) &&
        (!dtRetailEndDate.Checked || DateTime.Today <= dtRetailEndDate.Value));

      ckRetailOnly.Checked = CommonFunctions.CBoolean(row.Cells[colRetailOnly].Value);
      ckExcludeFromExceptions.Checked = CommonFunctions.CBoolean(row.Cells[colExcludeFromExceptions].Value);
      cboTaxCode.Text = CommonFunctions.CString(row.Cells["RIT TranTax"].Value);
      ckSaddlebackUSOC.Checked = CommonFunctions.CBoolean(row.Cells["IsSaddlebackUSOC"].Value);
      #endregion

      // final fixup (set item search so they require an existing time
      txtRetailUSOC.Enabled = false;  // you cannot change an existing usoc in edit mode

    }
    private void loadWholesaleFields(DataGridViewRow row)
    {
      if (row == null)
        return;

      clear();
      string wholesaleUSOC = txtWholesaleUSOC.Text = CommonFunctions.CString(row.Cells[colWholesaleUSOC].Value).Trim();
      #region Wholesale data
      if (string.IsNullOrEmpty(wholesaleUSOC))
        txtWholesaleUSOC.AddNewMode = true;
      else
        txtWholesaleUSOC.AddNewMode = false;
      txtWholesaleMRC.Text = CommonFunctions.CString(row.Cells[colWholesaleMRC].Value);
      txtWholesaleNRC.Text = CommonFunctions.CString(row.Cells[colWholesaleNRC].Value);
      DateTime? dt = getDateValue(row.Cells[colWholesaleStartDate].Value);
      if (dt == null)
        dtWholesaleStartDate.Checked = false;
      else
        dtWholesaleStartDate.Value = (DateTime)dt;
      dt = getDateValue(row.Cells[colWholesaleEndDate].Value);
      if (dt == null)
        dtWholesaleEndDate.Checked = false;
      else
        dtWholesaleEndDate.Value = (DateTime)dt;
      ckWholesaleInactivate.Checked = !((!dtWholesaleStartDate.Checked || DateTime.Today >= dtWholesaleStartDate.Value)
        && (!dtWholesaleEndDate.Checked || DateTime.Today <= dtWholesaleEndDate.Value));
      ckWholesaleOnly.Checked = CommonFunctions.CBoolean(row.Cells[colWholesaleOnly].Value);
      ////ckIsNotWholesaleSaddlebackUSOC.Checked = !CommonFunctions.CBoolean(row.Cells[colIsSaddleback].Value);
      //Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
      //criteria.Add("USOCWholesale", new string[] { ctlSearchGrid.opEQUALS, wholesaleUSOC });
      #endregion

      txtWholesaleUSOC.Enabled = false; // you can't change the usoc in edit mode
    }
    private void loadMatchingFields(DataGridViewRow row)
    {
      string retail = CommonFunctions.CString(row.Cells[colRetailUSOC].Value);
      string wholesale = CommonFunctions.CString(row.Cells[colWholesaleUSOC].Value).Trim();
      if (string.IsNullOrEmpty(retail))
        lstRetailUsocs.ClearSelected();
      else
        CommonFormFunctions.setComboBoxCell(lstRetailUsocs, retail);
      if (string.IsNullOrEmpty(wholesale))
        lstWholesaleUsocs.ClearSelected();
      else
        CommonFormFunctions.setComboBoxCell(lstWholesaleUsocs, wholesale);
      setMatchingLabel();

    }
    private void setMatchingLabel()
    {
      string retail = lstRetailUsocs.Text;
      string wholesale = lstWholesaleUsocs.Text;
      lblUsocMatching.Text = string.Format("Retail {0} = Wholesale {1}", retail, wholesale);
    }
    private void unsetMatchingLabel()
    {
      lblUsocMatching.Text = string.Empty;

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
      clear(true);
    }
    private void ckRefresh_CheckedChanged(object sender, EventArgs e)
    {

    }
    private void ckIncludeInactive_CheckedChanged(object sender, EventArgs e)
    {

    }
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
      else if (cboMode.Text.Equals("Edit Retail"))
        loadRetailFields(e.SelectedRow);
      else
        loadMatchingFields(e.SelectedRow);

      /*
       * 

Match Retail / Wholesale
*/
    }
    private void btnMatchUsoc_Click(object sender, EventArgs e)
    {
      setMatchingLabel();
    }

    private void btnUnmatchUsoc_Click(object sender, EventArgs e)
    {
      unsetMatchingLabel();
    }
    #endregion

  }
}
