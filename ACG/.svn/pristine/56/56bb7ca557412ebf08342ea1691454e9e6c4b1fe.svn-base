using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ACG.Common;
using ACG.Common.Data;

namespace ACG.CommonForms
{
  public partial class frmDataSourceMaintenance : Form, IScreenBase
  {
    private bool _dirty = false;
    private DataSourceDataSources _ds = null;
    private DataSourceDataSources _dataSource { get { if (_ds == null) _ds = new DataSourceDataSources(); return _ds; } }
    private bool _displayAnalyticsCheckbox = false;

    public bool DisplayAnalyticsCheckbox
    {
      get { return _displayAnalyticsCheckbox; }
      set { _displayAnalyticsCheckbox = value; }
    }

    public ISecurityContext SecurityContext { get; set; }

    public frmDataSourceMaintenance()
    {
      InitializeComponent();
      srchDataSource.SearchExec = new SearchDataSourceDataSources();
    }

    public void Save() 
    {
      if (!string.IsNullOrEmpty(srchDataSource.Text))
        saveData();
      else
        MessageBox.Show("No data to save");
    }

    #region private methods
    
    private void loadData()
    {
      DataSet ds = _dataSource.getDataSourceRecord(srchDataSource.Text, false);
      if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
      {
        clearData(false);
      }
      else
      {
        DataRow row = ds.Tables[0].Rows[0];
        txtDescription.Text = CommonFunctions.CString(row["Description"]);
        txtFromClause.Text = CommonFunctions.CString(row["FromClause"]);
        txtOrderBy.Text = CommonFunctions.CString(row["OrderByClause"]);
        txtParameterList.Text = CommonFunctions.CString(row["ParameterList"]);
        int maxRecords = CommonFunctions.CInt(row["MaxCount"]);
        if (maxRecords < 1)
          txtMaxCount.Text = string.Empty;
        else
          txtMaxCount.Text = maxRecords.ToString();
        ckOverrideWhere.Checked = CommonFunctions.CBoolean(row["OverrideWhere"]);
        ckIncludeInAnalysis.Checked = CommonFunctions.CBoolean(row["IncludeInAnalysis"]);
        _dirty = false;
      }
    }
    private void clearData(bool clearSearch)
    {
      if (clearSearch)
        srchDataSource.Text = string.Empty;
      txtDescription.Text = string.Empty;
      txtFromClause.Text = string.Empty;
      txtMaxCount.Text = string.Empty;
      txtOrderBy.Text = string.Empty;
      txtParameterList.Text = string.Empty;
      ckOverrideWhere.Checked = false;
      ckIncludeInAnalysis.Checked = true;
      ctlTestGrid.ClearAll();
      _dirty = !clearSearch;
    }
    private void saveData()
    {
      // max records = -1 means no maximum. and zero means the same thing
      string strMaxRecords = txtMaxCount.Text;
      int maxRecords = -1;
      if (!string.IsNullOrEmpty(strMaxRecords) && CommonFunctions.IsNumeric(strMaxRecords))
        maxRecords = CommonFunctions.CInt(strMaxRecords);
      if (maxRecords == 0)
        maxRecords = -1;
      int? ret = _dataSource.updateDataSourceRecord(srchDataSource.Text, CommonFunctions.fixUpStringForSQL(txtDescription.Text), CommonFunctions.fixUpStringForSQL(txtFromClause.Text), CommonFunctions.fixUpStringForSQL(txtOrderBy.Text), 
        CommonFunctions.fixUpStringForSQL(txtParameterList.Text), maxRecords, ckOverrideWhere.Checked, ckIncludeInAnalysis.Checked, SecurityContext.User);
      if (ret == -1)
        MessageBox.Show("Update Record failed");
      else
      {
        MessageBox.Show("Record Updated");
        _dirty = false;
      }
    }
    private void deleteData()
    {
      int? ret = _dataSource.deleteDataSourceRecord(srchDataSource.Text);
      if (ret == -1)
        MessageBox.Show("Delete Record failed");
      else
      {
        MessageBox.Show("Record Deleted");
        clearData(true);
      }
    }
    private bool validData()
    {
      if (!isID(srchDataSource.Text))
      {
        MessageBox.Show("Must have a valid DataSource ID. It must not be blank and must consist of characters that are A-Z,a-z,0-9");
        return false;
      }
      if (string.IsNullOrEmpty(txtDescription.Text))
      {
        MessageBox.Show("You must have a description");
        return false;
      }
      if (!isSQL(txtFromClause.Text))
      {
        MessageBox.Show("Must have valid SQL");
        return false;
      }
      if (!CommonFunctions.IsNumeric(txtMaxCount.Text))
      {
        MessageBox.Show("Max Count must be numeric");
        return false;
      }
      return true;
    }
    private bool isID(string id)
    {
      if (string.IsNullOrEmpty(id))
        return false;
      return CommonFunctions.RemoveSpecialCharacters(id).Equals(id);
    }
    private bool isSQL(string sql)
    {
      return (string.IsNullOrEmpty(sql) || !CommonFunctions.fixUpStringForSQL(sql).Equals(sql));
    }
    private void test()
    {
      lblTestProgress.Visible = true;
      if (_dirty)
      {
        lblTestProgress.Text = "Saving data on screen before test";
        lblTestProgress.Refresh();
        saveData();
      }
      try
      {
        lblTestProgress.Text = "Checking SQL for Errors";
        lblTestProgress.Refresh();
        _dataSource.getResearchFields(srchDataSource.Text, string.Empty);
      }
      catch (Exception ex)
      {
        lblTestProgress.Text = string.Empty;
        lblTestProgress.Visible = false;
        MessageBox.Show(string.Format("SQL is not valid. SQL Exception = <{0}>",CommonFunctions.getInnerException(ex).Message));
        return;
      }
      lblTestProgress.Text = "Initializing grid";
      lblTestProgress.Refresh();
      ctlTestGrid.Init(srchDataSource.Text, null);
      lblTestProgress.Text = "Loading data in grid";
      lblTestProgress.Refresh(); 
      ctlTestGrid.ReLoad();
      lblTestProgress.Text = string.Empty;
      lblTestProgress.Visible = false;
    }
    #endregion

    #region form events

    private void frmDataSourceMaintenance_Load(object sender, EventArgs e)
    {
      clearData(true);
      ctlTestGrid.Init("None", null);
      ckIncludeInAnalysis.Checked = _displayAnalyticsCheckbox;
    }
    private void srchDataSource_OnSelected(object sender, EventArgs e)
    {
      loadData();
    }
    private void btnTest_Click(object sender, EventArgs e)
    {
      test();
    }
    private void btnCancel_Click(object sender, EventArgs e)
    {
      loadData();
    }
    private void btnNew_Click(object sender, EventArgs e)
    {
      clearData(true);
    }
    private void btnDelete_Click(object sender, EventArgs e)
    {
      deleteData();
    }
    private void btnSave_Click(object sender, EventArgs e)
    {
      saveData();
    }
    private void txtAny_TextChanged(object sender, EventArgs e)
    {
      _dirty = true;
    }

    #endregion


  }
}
