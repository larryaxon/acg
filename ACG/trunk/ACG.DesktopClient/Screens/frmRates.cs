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
using ACG.Sys.Data;

namespace ACG.DesktopClient.Screens
{
  public partial class frmRates : ScreenBase
  {
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    private string _resource { get { return cboResource.Text; } set { cboResource.Text = value; } }

    public frmRates()
    {
      InitializeComponent();
    }

    private void populatePickList(ComboBox ctl, ACGTable list, string val)
    {
      ctl.Items.Clear();
      for (int i = 0; i < list.NumberRows; i++)
        ctl.Items.Add(CommonFunctions.CString(list[i, 0]));
      if (val != null)
        ctl.Text = val;
    }

    private void cboResource_SelectedIndexChanged(object sender, EventArgs e)
    {
      Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
      parameters.Add(ACGCommonData.fieldRESOURCEID, cboResource.Text);
      ctlRetailRates.load(parameters);
      setValue(ctlRetailRates.DefaultValues, ACGCommonData.fieldRESOURCEID, cboResource.Text);
      ctlWholesaleRate.load(parameters);
      setValue(ctlWholesaleRate.DefaultValues, ACGCommonData.fieldRESOURCEID, cboResource.Text);
    }

    private void frmRates_Load(object sender, EventArgs e)
    {
      ctlRetailRates.SecurityContext = SecurityContext;
      ctlWholesaleRate.SecurityContext = SecurityContext;
      ACGForm picklists = _dataSource.getDSPickLists(new Hashtable(), new ArrayList() { ACGCommonData.USER }, string.Empty, SecurityContext.User, string.Empty);
      populatePickList(cboResource, (ACGTable)((ACGFormItem)picklists[0]).Value, null);
      ctlRetailRates.TableName = "Rates";
      ctlWholesaleRate.TableName = "Costs";
      ctlRetailRates.HiddenColumns.Add(ACGCommonData.fieldRESOURCEID, null);
      ctlWholesaleRate.ReadOnlyColumns.Add(ACGCommonData.fieldRESOURCEID, null);
      ctlRetailRates.UniqueIdentifier = "ID";
      ctlWholesaleRate.UniqueIdentifier = "ID";
      ctlWholesaleRate.EncryptedFieldName = ACGCommonData.fieldCOST;
      ctlRetailRates.HiddenColumns.Add("ID", null);
      ctlWholesaleRate.ReadOnlyColumns.Add("ID", null);
    }

    private void setValue(Dictionary<string, object> dict, string key, string val)
    {
      if (dict.ContainsKey(key))
        dict[key] = val;
      else
        dict.Add(key, val);
    }

  }
}
