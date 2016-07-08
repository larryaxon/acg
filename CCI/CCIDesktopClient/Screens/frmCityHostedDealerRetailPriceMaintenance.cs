using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CCI.DesktopClient.Common;
using CCI.Sys.Data;
using ACG.Common;

namespace CCI.DesktopClient.Screens
{
  public partial class frmCityHostedDealerRetailPriceMaintenance : ScreenBase
  {
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }

    public frmCityHostedDealerRetailPriceMaintenance()
    {
      InitializeComponent();
    }

    private void frmCityHostedDealerRetailPriceMaintenance_Load(object sender, EventArgs e)
    {
      srchRetailUSOC.SearchExec = new SearchDataSourceProductList();
      ((SearchDataSourceProductList)srchRetailUSOC.SearchExec).Carrier = DataSource.HOSTEDRETAILCARRIER;
    }

    private void srchRetailUSOC_OnSelected(object sender, EventArgs e)
    {
      loadGrid();
    }

    private void grdPrices_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
    {
      e.Row.Cells["StartDate"].Value = DateTime.Today;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      save();
      loadGrid();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      loadGrid();
    }

    private void loadGrid()
    {
      DataSet ds = _dataSource.getHostedDealerPriceList(srchRetailUSOC.Text);
      CommonFormFunctions.convertDataSetToGrid(grdPrices, ds);
      grdPrices.Columns["ID"].Visible = false; // hide the id column
    }
    private void save()
    {
      string itemid = srchRetailUSOC.Text;
      int? ret = null;
      foreach (DataGridViewRow row in grdPrices.Rows)
      {
        object val = row.Cells["ID"].Value;
        int? id;
        DateTime? startDate, endDate;
        decimal price = 0;
        if (val == null || val == System.DBNull.Value)
          id = null;
        else
          id = CommonFunctions.CInt(val);
        val = row.Cells["StartDate"].Value;
        if (val == null || val == System.DBNull.Value)
          startDate = null;
        else
          startDate = CommonFunctions.CDateTime(val);
        val = row.Cells["EndDate"].Value;
        if (val == null || val == System.DBNull.Value)
          endDate = null;
        else
          endDate = CommonFunctions.CDateTime(val);
        val = row.Cells["Price"].Value;
        price = CommonFunctions.CDecimal(val);
        if (id != null || price != 0) // only update if it is not a blank row
          ret = _dataSource.updateHostedDealerPrice(id, itemid, price, startDate, endDate, SecurityContext.User);
        if (ret != null && ret < 0)
        {
          MessageBox.Show("Database error in update price");
          break;
        }
      }
      if (ret == null || ret >= 0)
        MessageBox.Show("Records saved");
    }
  }
}
