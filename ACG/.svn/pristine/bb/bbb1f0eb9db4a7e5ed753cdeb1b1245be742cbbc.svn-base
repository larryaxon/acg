using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ACG.Common;
using ACG.App.Common;
using ACG.DesktopClient.Common;

namespace ACG.DesktopClient.Screens
{
  public partial class frmMaintenance : ScreenBase
  {

    private string _tableName = string.Empty;
    private Dictionary<string, string[]> _tableFields { get { return ACGCommonData.ACGTableFields; } }
    private Dictionary<string, string[]> _tableTables { get { return ACGCommonData.ACGTableTables; } }
    public string TableName 
    { 
      get { return _tableName; } 
      set 
      {
        _tableName = value;
        if (TableName.Equals(ACGCommonData.TABLECUSTOMERRATES))
          ctlMaintenanceMain.TableName = ACGCommonData.TABLECUSTOMERS;
        else
          ctlMaintenanceMain.TableName = _tableName; 
      } 
    }
    private Dictionary<string, string> _parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    public frmMaintenance()
    {
      InitializeComponent();

    }

    private void frmMaintenance_Load(object sender, EventArgs e)
    {
      ctlMaintenanceMain.SecurityContext = ctlMaintenanceSub.SecurityContext = ctlMaintenanceSub2.SecurityContext = SecurityContext;
      if (!string.IsNullOrEmpty(TableName))
        ctlMaintenanceMain.load(filterParameters(ctlMaintenanceMain.TableName, _parameters));
    }

    private void ctlMaintenanceMain_RowSelected(object sender, MaintenanceGridRowSelectedArgs e)
    {
      selectSubData(ctlMaintenanceSub, (ctlMaintenanceBase)sender, e);
    }

    private void ctlMaintenanceSub_RowSelected(object sender, MaintenanceGridRowSelectedArgs e)
    {
      selectSubData(ctlMaintenanceSub2, (ctlMaintenanceBase)sender, e);
    }

    private void selectSubData(ctlMaintenanceBase targetGrid, ctlMaintenanceBase sourceGrid, MaintenanceGridRowSelectedArgs e)
    {
      Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
      string lookuptablename = string.Empty;
      string tableName = sourceGrid.TableName.ToLower();
      if (sourceGrid.Name.Equals("ctlMaintenanceMain"))
        tableName = TableName;
      parameters.Clear();
      if (_tableTables[tableName].GetLength(0) > 0)
      {
        lookuptablename = _tableTables[tableName][0];
        targetGrid.DefaultValues.Clear();
        targetGrid.HiddenColumns.Clear();
        if (this.Controls.ContainsKey("cboResource"))
        {
          ComboBox ctl = (ComboBox)this.Controls["cboResource"];
          if (ctl.SelectedIndex >= 0)
          {
            string val = CommonFunctions.CString(ctl.Items[ctl.SelectedIndex]);
            if (!val.Equals("All", StringComparison.CurrentCultureIgnoreCase))
            {
              parameters.Add(ACGCommonData.fieldRESOURCEID, val);
              targetGrid.DefaultValues.Add(ACGCommonData.fieldRESOURCEID, val);
              targetGrid.HiddenColumns.Add(ACGCommonData.fieldRESOURCEID, null);
            }
          }
        }        
        foreach (string fldName in _tableFields[tableName])
        {
          if (sourceGrid.ContainsColumn(fldName))
          {
            string val = CommonFunctions.CString(e.SelectedRow.Cells[fldName].Value);
            if (parameters.ContainsKey(fldName))
            {
              parameters[fldName] = val;
              targetGrid.DefaultValues[fldName] = val;
            }
            else
            {
              parameters.Add(fldName, val);
              targetGrid.DefaultValues.Add(fldName, val);
              targetGrid.HiddenColumns.Add(fldName, null);
            }
          }
        }


        targetGrid.TableName = lookuptablename;
        targetGrid.load(filterParameters(tableName, parameters));
      }
      else
        targetGrid.Clear();
    }

    protected Dictionary<string, string> filterParameters(string tableName, Dictionary<string, string> parameters)
    {
      // filter parameters to only use ones needed for the table
      Dictionary<string, string> parms = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
      if (_tableFields.ContainsKey(tableName))
      {
        string[] fields = _tableFields[tableName];
        for (int i = 0; i < fields.GetLength(0); i++)
        {
          if (parameters.ContainsKey(fields[i]))
            parms.Add(fields[i], parameters[fields[i]]);
          else
          {

          }
        }

        return parms;
      }
      else
        return parameters;
    }
  }
}
