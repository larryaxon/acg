using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ACG.Common;
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
        ACGForm pickLists = _dataSource.getDSPickLists(new Hashtable(), new ArrayList() { ACGCommonData.CUSTOMER, "resource" }, string.Empty, SecurityContext.User, string.Empty);
        foreach (ACGFormItem item in pickLists)
        {
          switch (item.ID)
          {
            case ACGCommonData.CUSTOMER:
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
      ctlMaintenanceMain.SecurityContext = ctlMaintenanceSub.SecurityContext = ctlMaintenanceSub2.SecurityContext = SecurityContext;
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
        setDefaultCustomer();
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
      setDefaultCustomer();
      Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
      parameters.Add(ACGCommonData.fieldCUSTOMERID, _customer);
      parameters.Add("resourceid", _resource);
      if (ctlMaintenanceMain.DefaultValues.ContainsKey(ACGCommonData.fieldCUSTOMERID))
        ctlMaintenanceMain.DefaultValues[ACGCommonData.fieldCUSTOMERID] = _customer;
      else
        ctlMaintenanceMain.DefaultValues.Add(ACGCommonData.fieldCUSTOMERID, _customer);
      ctlMaintenanceMain.load(filterParameters(TableName, parameters));
    }
    private void setDefaultCustomer()
    {
      if (cboCustomer.SelectedIndex < 0 || cboCustomer.SelectedIndex >= cboCustomer.Items.Count) // does not have valid selected index
        if (cboCustomer.Items.Count > 0)
          cboCustomer.SelectedIndex = 0; // then select the first one
        else
          return;
      setDefaultValue(ctlMaintenanceMain, ACGCommonData.fieldCUSTOMERID, _customer);
      setDefaultValue(ctlMaintenanceSub, ACGCommonData.fieldCUSTOMERID, _customer);
      setDefaultValue(ctlMaintenanceSub2, ACGCommonData.fieldCUSTOMERID, _customer);
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
