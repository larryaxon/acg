using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ACG.App.Common;
using ACG.DesktopClient.Common;
using ACG.Sys.Data;

namespace ACG.DesktopClient.Screens
{
  public partial class frmProjects : frmMaintenance
  {
    private bool _populatingCombos = false;
    private const string VALUEALL = "All";
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    private string _customer { get { return CommonFunctions.CString(cboCustomer.Items[cboCustomer.SelectedIndex]); } }
    private string _resource { get { return CommonFunctions.CString(cboResource.Items[cboResource.SelectedIndex]); } }
    public frmProjects()
    {
      TableName = "Projects";
      InitializeComponent();
    }
    private void loadCombos()
    {
      if (cboCustomer.Items.Count == 0 && cboResource.Items.Count == 0)
      {
        _populatingCombos = true;
        ACGForm pickLists = _dataSource.getDSPickLists(new Hashtable(), new ArrayList() { CommonData.CUSTOMER, "resource" }, string.Empty, SecurityContext.User, string.Empty);
        foreach (ACGFormItem item in pickLists)
        {
          switch (item.ID)
          {
            case CommonData.CUSTOMER:
              ACGTable customerList = (ACGTable)item.Value;
              fillCombo(cboCustomer, customerList);
              cboCustomer.SelectedIndex = 0;
              break;
            case "resource":
              ACGTable resourceList = (ACGTable)item.Value;
              fillCombo(cboResource, resourceList);
              cboResource.SelectedIndex = 0;
              break;
          }
        }
        _populatingCombos = false;
      }
    }

    private void frmProjects_Load(object sender, EventArgs e)
    {
      loadProjects();
    }

    private void fillCombo(ComboBox ctl, ACGTable list)
    {
      ctl.Items.Clear();
      ctl.Items.Add(VALUEALL);
      for (int iRow = 0; iRow < list.NumberRows; iRow++)
        ctl.Items.Add(list[iRow, 0]);

    }

    private void cboCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!_populatingCombos)
      {
        setDefaultValue(ctlMaintenanceMain, CommonData.fieldCUSTOMERID, _customer);
        setDefaultValue(ctlMaintenanceSub, CommonData.fieldCUSTOMERID, _customer);
        setDefaultValue(ctlMaintenanceSub2, CommonData.fieldCUSTOMERID, _customer);
        loadProjects();
      }
    }
    private void setDefaultValue(ctlMaintenanceBase ctl, string fld, string val)
    {
      if (val.Equals(VALUEALL, StringComparison.CurrentCultureIgnoreCase))
      {
        if (ctl.HiddenColumns.ContainsKey(fld))
          ctl.HiddenColumns.Remove(fld);
      }
      else
        if (ctl.HiddenColumns.ContainsKey(fld))
          ctl.HiddenColumns[fld] = val;
        else
          ctl.HiddenColumns.Add(fld, val);
    }
    private void loadProjects()
    {
      loadCombos();
      Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
      parameters.Add(CommonData.fieldCUSTOMERID, _customer);
      parameters.Add("resourceid", _resource);
      ctlMaintenanceMain.load(filterParameters(TableName, parameters));
    }

    private void cboResource_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!_populatingCombos)
      {
        setDefaultValue(ctlMaintenanceMain, "resourceid", _resource);
        setDefaultValue(ctlMaintenanceSub, "resourceid", _resource);
        setDefaultValue(ctlMaintenanceSub2, "resourceid", _resource);
        loadProjects();
      }
    }

  }
}
