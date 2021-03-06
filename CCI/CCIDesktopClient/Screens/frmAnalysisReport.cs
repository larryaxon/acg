﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ACG.Common.Data;
using ACG.CommonForms;
using CCI.Common;
using CCI.DesktopClient.Common;
using CCI.Sys.Data;

namespace CCI.DesktopClient.Screens
{
  public partial class frmAnalysisReport : ScreenBase
  {
    private bool _clearing = false;
    private List<string> _idList = null;
    DataSourceDataSources _ds = null;
    DataSourceDataSources _dataSource { get { if (_ds == null) _ds = new DataSourceDataSources(); return _ds; } }

    public frmAnalysisReport()
    {
      InitializeComponent();
      loadCombos();
    }

    #region private methods
    private void loadCombos()
    {
      srchNamedSearch.SearchExec = new SearchDataSourceNamedSearch();
      SearchDataSourceDataSources dataSearch = new SearchDataSourceDataSources();
      dataSearch.IncludeOnlyAnalysis = true;
      srchTopGrid.SearchExec = srchMiddleGrid.SearchExec = srchBottomGrid.SearchExec = dataSearch;    
  
      srchTopGrid.Text = srchMiddleGrid.Text = srchBottomGrid.Text = "None";
    }
    private void selectGrid(ACG.CommonForms.ctlSearch srch)
    {
      if (!_clearing)
      {
        Cursor.Current = Cursors.WaitCursor;
        this.SuspendLayout();
        ACG.CommonForms.ctlSearchGrid grid = topGrid;
        string type = srch.Text;
        switch (srch.Name)
        {
          case "srchTopGrid":
            grid = topGrid;
            break;
          case "srchMiddleGrid":
            grid = middleGrid;
            splitTop.Panel2Collapsed = srch.Text.Equals("None"); // unhide the panel
            break;
          case "srchBottomGrid":
            grid = bottomGrid;
            splitMain.Panel2Collapsed = srch.Text.Equals("None"); // unhide the panel
            break;
        }
        grid.ClearAll();
        grid.Init(type, grid.Name);
        // cash receipts query requires special parameters so we present a dialog which asks for and then populates them
        if (type.Equals("CashReceipts", StringComparison.CurrentCultureIgnoreCase))
        {
          dlgGetCashReceiptsParameters frm = new dlgGetCashReceiptsParameters(grid);
          frm.ShowDialog(this);
        }
        grid.ClearSearch();
        this.ResumeLayout();
        Cursor.Current = Cursors.Default;
      }
    }
    #endregion

    #region form events

    private void frmAnalysisReport_Load(object sender, EventArgs e)
    {
      topGrid.UniqueIdentifier = middleGrid.UniqueIdentifier = bottomGrid.UniqueIdentifier = 
        topGrid.IDListColumn = bottomGrid.IDListColumn = middleGrid.IDListColumn = "CustomerID";
      topGrid.FieldsDefaultIsChecked = middleGrid.FieldsDefaultIsChecked = bottomGrid.FieldsDefaultIsChecked = true;
      topGrid.CanChangeDisplayFields = middleGrid.CanChangeDisplayFields = bottomGrid.CanChangeDisplayFields = true;
      topGrid.IncludeGroupAsCriteria = bottomGrid.IncludeGroupAsCriteria = middleGrid.IncludeGroupAsCriteria = true;
      topGrid.UseNamedSearches = middleGrid.UseNamedSearches = bottomGrid.UseNamedSearches = true;
      ((SearchDataSourceNamedSearch)srchNamedSearch.SearchExec).NameType = "AnalyticsScreen";
      ((SearchDataSourceNamedSearch)srchNamedSearch.SearchExec).User = SecurityContext.User;
    }
    private void topGrid_SearchPressed(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      _idList = e.IDList;
      if (ckFilterByIDList.Checked)
      {
        bottomGrid.IDList = _idList;
        middleGrid.IDList = _idList;
      }
    }
    private void ckFilterByIDList_CheckedChanged(object sender, EventArgs e)
    {
      if (!ckFilterByIDList.Checked)
        middleGrid.IDList = bottomGrid.IDList = null; // get rid of the prior IDList so the search works properly
    }
    private void btnGroups_Click(object sender, EventArgs e)
    {
      ScreenBase frm = new frmGroups();
      frm.SecurityContext = SecurityContext;
      ((frmGroups)frm).Init(new ArrayList(topGrid.IDList));
      frm.ShowDialog();
    }
    private void cboTopGrid_SelectedIndexChanged(object sender, EventArgs e)
    {
      selectGrid((ctlSearch)sender);
    }
    private void cboMiddleGrid_SelectedIndexChanged(object sender, EventArgs e)
    {
      selectGrid((ctlSearch)sender);   
    }
    private void cboBottomGrid_SelectedIndexChanged(object sender, EventArgs e)
    {
      selectGrid((ctlSearch)sender);    
    }
    private void middleGrid_SearchPressed(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      if (ckFilterByIDList.Checked)
      {
        bottomGrid.IDList = e.IDList;
      }
    }
    private void btnClear_Click(object sender, EventArgs e)
    {
      _clearing = true;
      srchTopGrid.Text = srchMiddleGrid.Text = srchBottomGrid.Text = "None";
      topGrid.Init("None", null);
      middleGrid.Init("None", null);
      bottomGrid.Init("None", null);
      if (_idList != null)
        _idList.Clear();
      topGrid.IDList = middleGrid.IDList = bottomGrid.IDList = _idList;
      bottomGrid.Clear();
      middleGrid.Clear();
      topGrid.Clear();
      splitMain.Panel2Collapsed = splitTop.Panel2Collapsed = true;
      _clearing = false;
    }
    #endregion
  }
}
