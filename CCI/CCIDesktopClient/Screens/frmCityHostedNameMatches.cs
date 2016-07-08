using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using CCI.Sys.Data;
using CCI.DesktopClient.Common;

namespace CCI.DesktopClient.Screens
{
  public partial class frmCityHostedNameMatches : ScreenBase
  {
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    private bool _loading = false;

    public frmCityHostedNameMatches()
    {
      InitializeComponent();
    }

    #region form events

    private void btnSave_Click(object sender, EventArgs e)
    {
      saveFields();
    }
    private void srchNameMatches_RowSelected(object sender, ACG.CommonForms.MaintenanceGridRowSelectedArgs e)
    {
      DataGridViewRow row = e.SelectedRow;
      loadFields(row);
    }

    private void frmCityHostedNameMatches_Load(object sender, EventArgs e)
    {
      _loading = true;
      srchNameMatches.Init("CityHostedNameMatches", null); 
      srchNameMatches.ClearSearch();
      _loading = false;
      reloadGrid(true);
    }
    private void btnDelete_Click(object sender, EventArgs e)
    {
      deleteRecord();
    }
    #endregion

    #region private methods
    private void reloadGrid(bool forceRefresh)
    {
      Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
      srchNameMatches.SearchCriteria = criteria;
      srchNameMatches.ReLoad();
      clearFields();
    }
    private void saveFields()
    {
      string saddlebackName = txtsaddlebackName.Text;
      string oldcustomer = txtcustomerID.Text;
      string newcustomer = txtnewcustomerID.Text;
      if (string.IsNullOrEmpty(oldcustomer) || string.IsNullOrEmpty(newcustomer))
        MessageBox.Show("There is no record to delete");
      else
      {
        _dataSource.saveNameMatch(oldcustomer, newcustomer, saddlebackName);
        reloadGrid(true);
        MessageBox.Show("Record saved");
      }
    }
    private void clearFields()
    {
      txtsaddlebackName.Text = string.Empty;
      txtcustomerID.Text = string.Empty;
      txtnewcustomerID.Text = string.Empty;
    }
    private void loadFields(DataGridViewRow row)
    {
      txtsaddlebackName.Text = CommonFunctions.CString(row.Cells["SaddlebackName"].Value);
      txtcustomerID.Text = CommonFunctions.CString(row.Cells["LinkedTo"].Value);
    }
    private void deleteRecord()
    {
      string saddlebackName = txtsaddlebackName.Text;
      string linkedto = txtcustomerID.Text;
      if (string.IsNullOrEmpty(saddlebackName) || string.IsNullOrEmpty(linkedto))
        MessageBox.Show("There is no record to delete");
      else
      {
        _dataSource.deleteNameMatch(linkedto, saddlebackName);
        clearFields();
        reloadGrid(true);
        MessageBox.Show("Record deleted");
      }
    }

    #endregion
  }
}

