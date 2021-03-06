﻿using System;
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

    private const string DEFAULTWHOLESALECARRIER = "Saddleback";
    private const string DEFAULTRETAILCARRIER = "CityHosted";

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
    private const string colPrimaryCarrier = "PrimaryCarrier";

    private const string colIsSaddleback = "IsSaddlebackUSOC";
    private const string colRetailActive = "retailactive";
    private const string colWholesaleActive = "wholesaleactive";
    private const string colExcludeFromExceptions = "ExcludeFromExceptions";

    private const string modeRetail = "Edit Retail";
    private const string modeWholesale = "Edit Wholesale";
    private const string modeMatching = "Match Retail/Wholesale";

    private List<int> _dealerCostDirtyRows = new List<int>();

    private string[] _dpLevels = null;
    private string[] _dealerPricingLevels { get { if (_dpLevels == null) _dpLevels = _dataSource.getDealerPricingLevels(); return _dpLevels; } }

    private string[] _modeList = new string[] { modeRetail, modeWholesale, modeMatching };

    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    #endregion
    public frmCityHostedUSOCMaintenanceNew() 
    {
      InitializeComponent();
      //txtRetailUSOC.SearchExec = new SearchDataSourceProductList();
      //txtWholesaleUSOC.SearchExec = new SearchDataSourceProductList();
      srchRetailUsocMatching.SearchExec = new SearchDataSourceProductList();
      srchWholesaleUsocMatching.SearchExec = new SearchDataSourceProductList();
      resetUsocSearch(cboCarrier.Text);
      srchUSOCList.CanChangeDisplayFields = true;
      cboMode.Items.Clear();
      cboMode.Items.AddRange(_modeList);
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
          else if (cType == typeof(TextBox) || cType == typeof(ComboBox) || cType == typeof(ACG.CommonForms.ctlSearch) || cType == typeof(DateTimePicker))
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
      cboRITCategory.Items.Clear();
      cboRITCategory.Items.AddRange(_dataSource.getExternalCategories());
      cboCHSCategory.Items.Clear();
      cboCHSCategory.Items.AddRange(_dataSource.getItemCategories());
      cboTaxCode.Items.Clear();
      cboTaxCode.Items.AddRange(_dataSource.getTaxCodes());
    }
    private void selectMode(string mode)
    {
      switch (mode)
      {
        case modeWholesale:
          selectWholesaleRetail("wholesale");
          tabMaintenance.SelectedTab = tabWholesale;
          break;
        case modeRetail:
          selectWholesaleRetail("retail");
          tabMaintenance.SelectedTab = tabRetail;
          break;
        default:
          tabMaintenance.SelectedTab = tabMatching;
          break;
      }
      loadEditScreens(mode);
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
      resetUsocSearch(cboCarrier.Text);
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
    private void loadRetailFields(DataGridViewRow row, bool clearFirst = true)
    {
      if (row == null)
        return;
      if (clearFirst)
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
      ckSaddlebackUSOC.Checked = !CommonFunctions.CBoolean(row.Cells["IsSaddlebackUSOC"].Value);
      #endregion

      // final fixup (set item search so they require an existing time
      txtRetailUSOC.Enabled = false;  // you cannot change an existing usoc in edit mode
      loadDealerCostGrid(txtRetailUSOC.Text);

    }
    private void loadDealerCostGrid(string retailUsoc)
    {
      DataSet ds = _dataSource.getHostedDealerCosts(retailUsoc);
      List<DataGridViewColumn> columns = new List<DataGridViewColumn>();
      // specify the column types to use int he grid
      DataGridViewComboBoxColumn levels = new DataGridViewComboBoxColumn();
      levels.Items.AddRange(_dealerPricingLevels);
      columns.Add(levels); //Levels
      DataGridViewTextBoxColumn usoc = new DataGridViewTextBoxColumn();
      usoc.Visible = false;
      columns.Add(usoc); //USOCs
      columns.Add(new DataGridViewTextBoxColumn()); //StartDate
      columns.Add(new DataGridViewTextBoxColumn()); //EndDate
      columns.Add(new DataGridViewTextBoxColumn()); //MRC
      columns.Add(new DataGridViewTextBoxColumn()); //NRC
      DataGridViewTextBoxColumn lastModifiedBy = new DataGridViewTextBoxColumn();
      lastModifiedBy.Visible = false;
      columns.Add(lastModifiedBy);
      DataGridViewTextBoxColumn lastModifiedDateTime = new DataGridViewTextBoxColumn();
      lastModifiedDateTime.Visible = false;
      columns.Add(lastModifiedDateTime);
      CommonFormFunctions.convertDataSetToGrid(grdDealerCosts, ds, columns.ToArray());
      //grdDealerCosts.Columns["USOC"].Visible = false; // hide the itemid/usoc column
      //grdDealerCosts.Columns["LastModifiedBy"].Visible = false;
      //grdDealerCosts.Columns["LastModifiedDateTime"].Visible = false;
      //// now make all the level cells a combo
      //foreach (DataGridViewRow row in grdDealerCosts.Rows)
      //{
      //  DataGridViewCell oldCell = row.Cells["Level"];
      //  DataGridViewComboBoxCell newCell = new DataGridViewComboBoxCell();
      //  newCell.Items.AddRange(_dealerPricingLevels);
      //  newCell.Value = oldCell.Value;
      //  int cellIndex = oldCell.ColumnIndex;
      //  row.Cells[cellIndex] = newCell;
      //}
    }
    private void loadWholesaleFields(DataGridViewRow row, bool clearFirst = true)
    {
      if (row == null)
        return;
      if (clearFirst)
        clear();
      string wholesaleUSOC = txtWholesaleUSOC.Text = CommonFunctions.CString(row.Cells[colWholesaleUSOC].Value).Trim();
      #region Wholesale data
      txtWholesaleDescription.Text = CommonFunctions.CString(row.Cells[colWholesaleDescription].Value);
      //if (string.IsNullOrEmpty(wholesaleUSOC))
      //  txtWholesaleUSOC.AddNewMode = true;
      //else
      //  txtWholesaleUSOC.AddNewMode = false;
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
      {
        lstRetailUsocs.ClearSelected();
        srchRetailUsocMatching.Text = string.Empty;
      }
      else
      {
        CommonFormFunctions.setComboBoxCell(lstRetailUsocs, retail);
        srchRetailUsocMatching.Text = retail;
      }
      if (string.IsNullOrEmpty(wholesale))
      {
        lstWholesaleUsocs.ClearSelected();
        srchWholesaleUsocMatching.Text = string.Empty;
      }
      else
      {
        CommonFormFunctions.setComboBoxCell(lstWholesaleUsocs, wholesale);
        srchWholesaleUsocMatching.Text = wholesale;
      }
      matchUsocs(false);
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
    private void loadEditScreens(string mode)
    {
      DataGridViewRow row = srchUSOCList.SelectedRow;

      loadEditScreens(mode, row);
    }
    private void loadEditScreens(string mode, DataGridViewRow row)
    {
      if (row == null)
        return;
      if (string.IsNullOrEmpty(mode))
        return;
      // 2-10-2018 LLA Got rid of looking at mode to decide whether to load tabs. Just load all of them unless there's no data
      string wholesaleUsoc;
      object w = row.Cells[colWholesaleUSOC];
      if (w == null)
        wholesaleUsoc = string.Empty;
      else
        wholesaleUsoc = CommonFunctions.CString(((DataGridViewCell)w).Value);
      string retailUsoc;
      object r = row.Cells[colRetailUSOC];
      if (r == null)
        retailUsoc = string.Empty;
      else
        retailUsoc = CommonFunctions.CString(((DataGridViewCell)r).Value);
      loadWholesaleFields(row);
      loadRetailFields(row, false);
      loadMatchingFields(row); // don't worry about reload of this screen it is fast and simple
    }
    private void matchUsocs(bool save)
    {
      string retail = srchRetailUsocMatching.Text;
      string wholesale = srchWholesaleUsocMatching.Text;
      string primaryCarrier = cboCarrier.Text;
      if (string.IsNullOrEmpty(retail))
        if (string.IsNullOrEmpty(wholesale))
          lblUsocMatching.Text = "No Retail or Wholesale Usoc selected";
        else
          lblUsocMatching.Text = string.Format("Wholesale Usoc {0} is Unmatched", wholesale);
      else
        if (string.IsNullOrEmpty(wholesale))
          lblUsocMatching.Text = string.Format("Retail Usoc {0} is Unmatched", retail);
        else
          lblUsocMatching.Text = string.Format("Retail {0} = Wholesale {1}", retail, wholesale);

      if (save)
      {
        int? ret = _dataSource.reassignWholesaleUSOC(retail, wholesale, primaryCarrier);
        if (ret != null && ret == -1)
          MessageBox.Show("Match Retail to Wholesale USOC was not successfull");
        reloadUsocList(false);
      }
    }
    private void unmatchUsocs(bool save)
    {

      string retail = lstRetailUsocs.Text;
      string primaryCarrier = cboCarrier.Text;
      string wholesale = lstWholesaleUsocs.Text;
      lblUsocMatching.Text = string.Format("Retail {0} and Wholesale {1} are Unmatched", retail, wholesale);
      if (save)
      {
        // disconect retail
        int? ret = _dataSource.reassignWholesaleUSOC(retail, string.Empty, primaryCarrier);
        if (ret != null && ret == -1)
          MessageBox.Show("UnMatch Retail to Wholesale USOC was not successfull");
        // disconnect whoesale
        if (retail.Equals(wholesale, StringComparison.CurrentCultureIgnoreCase))
        {
          ret = _dataSource.reassignWholesaleUSOC(string.Empty, wholesale, primaryCarrier);
          if (ret != null && ret == -1)
            MessageBox.Show("UnMatch Retail to Wholesale USOC was not successfull");
        }
        reloadUsocList(false);
      }
    }
    private void saveRetail()
    {
      save("retail");
    }
    private void saveDealerCostGrid(string usoc)
    {
      foreach (int rowIndex in _dealerCostDirtyRows)
      {
        DataGridViewRow row = grdDealerCosts.Rows[rowIndex];
        string level = CommonFunctions.CString(row.Cells["Level"].Value);
        if (!string.IsNullOrEmpty(level)) // dont save an empty row
        {
          string install = CommonFunctions.CString(CommonFunctions.CDecimal(row.Cells["NRC"]));
          string dealerCost = CommonFunctions.CString(CommonFunctions.CDecimal(row.Cells["MRC"]));
          object oDate = row.Cells["StartDate"].Value;
          DateTime startDate = oDate == null ? CommonData.PastDateTime : CommonFunctions.CDateTime(oDate);
          oDate = row.Cells["EndDate"].Value;
          DateTime endDate = oDate == null ? CommonData.FutureDateTime : CommonFunctions.CDateTime(oDate);
          _dataSource.updateDealerCost(null, usoc, dealerCost, install, level, startDate, endDate, SecurityContext.User);
        }
      }
    }
    private void saveWholesale()
    {
      save("wholesale");
    }
    private void save(string mode)
    {
      if (string.IsNullOrEmpty(mode))
        return;
      #region init fields
      string retailUSOC, chsCategory, wholesaleUSOC, dqcategory,
        retailDescription, wholesaleDescription, externalDescription, externalCategory, taxCode;
      decimal retailMRC, retailNRC, wholesaleMRC, wholesaleNRC;
      DateTime? retailStartDate, retailEndDate, wholesaleStartDate, wholesaleEndDate;
      bool isSaddlebackUSOC, excludeFromException, retailOnly, wholesaleOnly, isRecommended, useMRC;
      wholesaleUSOC = retailUSOC = wholesaleDescription = retailDescription = wholesaleDescription = chsCategory =
        externalDescription = externalCategory = taxCode = dqcategory = string.Empty;
      excludeFromException = retailOnly = wholesaleOnly = isRecommended = useMRC = false;
      isSaddlebackUSOC = true;
      retailStartDate = retailEndDate = wholesaleStartDate = wholesaleEndDate = null;
      retailMRC = retailNRC = wholesaleMRC = wholesaleNRC = 0;
      int? retCode;
      string primaryCarrier = cboCarrier.Text;
      #endregion

      #region set common fields

      retailUSOC = txtRetailUSOC.Text;
      wholesaleUSOC = txtWholesaleUSOC.Text;
      bool isRetail = mode.Equals("retail", StringComparison.CurrentCultureIgnoreCase);
      bool isWholesale = mode.Equals("wholesale", StringComparison.CurrentCultureIgnoreCase);
      retailOnly = ckRetailOnly.Checked;
      wholesaleOnly = ckWholesaleOnly.Checked;
      chsCategory = cboCHSCategory.Text;
      externalDescription = txtExternalDescription.Text;
      excludeFromException = ckExcludeFromExceptions.Checked;
      isSaddlebackUSOC = !ckSaddlebackUSOC.Checked;
      externalCategory = cboRITCategory.Text;
      #endregion

      #region update retail
      if (isRetail)
      {
        // populate retail fields
        if (string.IsNullOrEmpty(retailUSOC))
        {
          MessageBox.Show("You must have a valid USOC to save");
          return;
        }
        if (string.IsNullOrEmpty(txtRetailDescription.Text))
        {
          MessageBox.Show("You must have a Description to save");
          return;
        }
        retailDescription = txtRetailDescription.Text.Replace("'", "");
        if (ckVariableRetailMRC.Checked)
          retailMRC = -1;
        else
          retailMRC = CommonFunctions.CDecimal(txtRetailMRC.Text);
        if (ckVariableRetailNRC.Checked)
          retailNRC = -1;
        else
          retailNRC = CommonFunctions.CDecimal(txtRetailNRC.Text);

        if (dtRetailStartDate.Checked)
          retailStartDate = dtRetailStartDate.Value;
        else
          retailStartDate = null;
        if (dtRetailEndDate.Checked)
          retailEndDate = dtRetailEndDate.Value;
        else
          retailEndDate = null;
        taxCode = cboTaxCode.Text;
        retCode = _dataSource.updateMasterProduct(retailUSOC, retailDescription, chsCategory, retailUSOC, externalCategory, externalDescription, isRetail, isWholesale, isSaddlebackUSOC);
        if (retCode == null || retCode >= 0)
          retCode = _dataSource.updateProduct(DEFAULTRETAILCARRIER, retailUSOC, retailStartDate, retailEndDate, SecurityContext.User, retailMRC, retailNRC,
            retailOnly, excludeFromException, taxCode, primaryCarrier);
      }
      #endregion

      #region update wholesale 
      if (isWholesale)
      {
        // populate wholesale fields
        if (string.IsNullOrEmpty(wholesaleUSOC))
        {
          MessageBox.Show("You do not have a Wholesale USOC");
          return;
        }

        if (string.IsNullOrEmpty(txtWholesaleDescription.Text))
        {
          MessageBox.Show("You must have a Description to save");
          return;
        }

        wholesaleDescription = txtWholesaleDescription.Text.Replace("'", "");
        wholesaleMRC = CommonFunctions.CDecimal(txtWholesaleMRC.Text);
        wholesaleNRC = CommonFunctions.CDecimal(txtWholesaleNRC.Text);

        if (dtWholesaleStartDate.Checked)
          wholesaleStartDate = dtWholesaleStartDate.Value;
        else
          wholesaleStartDate = null;
        if (dtWholesaleEndDate.Checked)
          wholesaleEndDate = dtWholesaleEndDate.Value;
        else
          wholesaleEndDate = null;
        
        retCode = _dataSource.updateMasterProduct(wholesaleUSOC, wholesaleDescription, chsCategory, wholesaleUSOC, externalCategory, externalDescription, isRetail, isWholesale, isSaddlebackUSOC);
        if (retCode == null || retCode >= 0)
          retCode = _dataSource.updateProduct(primaryCarrier, wholesaleUSOC, wholesaleStartDate, wholesaleEndDate, SecurityContext.User, wholesaleMRC, wholesaleNRC,
            wholesaleOnly, excludeFromException, taxCode, primaryCarrier);
      }
      if (ckRefresh.Checked)
        reloadUsocList(false);
      #endregion

    }
    private void resetUsocSearch(string primaryCarrier)
    {
      //((SearchDataSourceProductList)txtRetailUSOC.SearchExec).Carrier = DEFAULTRETAILCARRIER;
      //((SearchDataSourceProductList)txtRetailUSOC.SearchExec).PrimaryCarrier = primaryCarrier;
      ((SearchDataSourceProductList)srchRetailUsocMatching.SearchExec).Carrier = DEFAULTRETAILCARRIER;
      ((SearchDataSourceProductList)srchRetailUsocMatching.SearchExec).PrimaryCarrier = primaryCarrier;
      //((SearchDataSourceProductList)txtWholesaleUSOC.SearchExec).Carrier = primaryCarrier;
      //((SearchDataSourceProductList)txtWholesaleUSOC.SearchExec).PrimaryCarrier = primaryCarrier;
      ((SearchDataSourceProductList)srchWholesaleUsocMatching.SearchExec).Carrier = primaryCarrier;
      ((SearchDataSourceProductList)srchWholesaleUsocMatching.SearchExec).PrimaryCarrier = primaryCarrier;
    }
    #endregion

    #region form events
    private void frmCityHostedUSOCMaintenanceNew_Load(object sender, EventArgs e)
    {
      srchUSOCList.Init(CCI.Common.CommonData.UnmatchedNameTypes.CityHostedUSOCs, null);
      srchUSOCList.ColumnOrderSaveEnabled = false;
      ckIncludeInactive_CheckedChanged(this, new EventArgs()); // force load to use inactive flag
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
      loadEditScreens(cboMode.Text);
    }
    private void srchUSOCList_RowSelected(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      loadEditScreens(cboMode.Text, e.SelectedRow);
    }
    private void btnMatchUsoc_Click(object sender, EventArgs e)
    {
      matchUsocs(true);
    }
    private void btnUnmatchUsoc_Click(object sender, EventArgs e)
    {
      unmatchUsocs(true);
    }
    private void tabMaintenance_SelectedIndexChanged(object sender, EventArgs e)
    {
      TabPage thisTab = tabMaintenance.SelectedTab;
      string thisMode = cboMode.Text;
      if (thisTab.Name.Equals(tabRetail.Name) && !thisMode.Equals(modeRetail))
        CommonFormFunctions.setComboBoxCell(cboMode, modeRetail);
      else if (thisTab.Name.Equals(tabWholesale.Name) && !thisMode.Equals(modeWholesale))
        CommonFormFunctions.setComboBoxCell(cboMode, modeWholesale);
      else if (thisTab.Name.Equals(tabMatching.Name) && !thisMode.Equals(modeMatching))
        CommonFormFunctions.setComboBoxCell(cboMode, modeMatching);
    }
    private void btnRetailNew_Click(object sender, EventArgs e)
    {
      clearTabPage(tabRetail);
      //txtRetailUSOC.AddNewMode = true;
      txtRetailUSOC.Enabled = true;
    }
    private void btnRetailCancel_Click(object sender, EventArgs e)
    {
      if (srchUSOCList.SelectedRow != null)
        loadRetailFields(srchUSOCList.SelectedRow);
      else
        clearTabPage(tabRetail);
    }
    private void btnRetailSave_Click(object sender, EventArgs e)
    {
      saveRetail();
      txtRetailUSOC.Enabled = false;
    }
    private void btnWholesaleNew_Click(object sender, EventArgs e)
    {
      txtWholesaleUSOC.Enabled = true; // Now you can add a new usoc
      //txtWholesaleUSOC.AddNewMode = true;
      clearTabPage(tabWholesale);
    }
    private void btnWholesaleCancel_Click(object sender, EventArgs e)
    {
      if (srchUSOCList.SelectedRow != null)
        loadWholesaleFields(srchUSOCList.SelectedRow);
      else
        clearTabPage(tabWholesale);
    }
    private void srchRetailUsocMatching_OnSelected(object sender, EventArgs e)
    {
      // select the list item to match selected item
      CommonFormFunctions.setComboBoxCell(lstRetailUsocs, srchRetailUsocMatching.Text);
    }
    private void srchWholesaleUsocMatching_OnSelected(object sender, EventArgs e)
    {
      // select the list item to match selected item
      CommonFormFunctions.setComboBoxCell(lstWholesaleUsocs, srchWholesaleUsocMatching.Text);
    }
    private void lstRetailUsocs_SelectedIndexChanged(object sender, EventArgs e)
    {
      srchRetailUsocMatching.Text = lstRetailUsocs.Text;
    }
    private void lstWholesaleUsocs_SelectedIndexChanged(object sender, EventArgs e)
    {
      srchWholesaleUsocMatching.Text = lstWholesaleUsocs.Text;
    }
    private void btnWholesaleSave_Click(object sender, EventArgs e)
    {
      saveWholesale();
    }
    private void tabMaintenance_DrawItem(object sender, DrawItemEventArgs e)
    {
      TabPage page = tabMaintenance.TabPages[e.Index];
      e.Graphics.FillRectangle(new SolidBrush(page.BackColor), e.Bounds);

      Rectangle paddedBounds = e.Bounds;
      int yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
      paddedBounds.Offset(1, yOffset);
      TextRenderer.DrawText(e.Graphics, page.Text, Font, paddedBounds, page.ForeColor);
    }
    private void grdDealerCosts_RowLeave(object sender, DataGridViewCellEventArgs e)
    {
      if (!_dealerCostDirtyRows.Contains(e.RowIndex))
        _dealerCostDirtyRows.Add(e.RowIndex);
    }
    private void grdDealerCosts_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
    {
      DataGridViewRow row = grdDealerCosts.Rows[e.RowIndex];
      DataGridViewCell lastCell = row.Cells["NRC"];
      // valid dates?
      foreach (DataGridViewCell cell in row.Cells)
      {
        string cellName = grdDealerCosts.Columns[cell.ColumnIndex].Name;
        switch (cellName)
        {
          case "StartDate":
          case "EndDate":
            if (cell.Value != null && cell.Value != System.DBNull.Value) // null is valid value
            {
              if (!CommonFunctions.IsDateTime(cell.Value.ToString())) // but if its not a good date
              {
                cell.Selected = true; // select the errored cell
                if (lastCell.ColumnIndex != cell.ColumnIndex) // and this validation routine keeps that last cell selected so we unselect it
                  lastCell.Selected = false;
                MessageBox.Show(cellName + " is not in correct date format");
                e.Cancel = true;
                return;
              }

            }
            break;
          case "USOC":
            if (cell.Value == null || cell.Value == System.DBNull.Value) // there is no USOC
              cell.Value = txtRetailUSOC.Text; // then use the one from the current tab
            break;
          case "MRC":
          case "NRC":
            // make sure they are numeric
            if (cell.Value == null || !CommonFunctions.IsNumeric(cell.Value))
            {
              cell.Selected = true;
              MessageBox.Show(cellName + "Must be a number");
              e.Cancel = true;
              return;
            }
            break;

        }

      }
    }
    #endregion


  }
}
