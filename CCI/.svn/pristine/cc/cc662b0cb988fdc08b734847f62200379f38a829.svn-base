using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CCI.DesktopClient.Screens
{
  public partial class frmCodeMaster : frmMaintenanceBase
  {
    public string CodeType { get; set; }
    Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

    public frmCodeMaster(string codetype, string description)
    {
      this.Text = description;
      init(codetype);
    }
    public frmCodeMaster(string codetype) 
    {
      init(codetype);
    }
    private void init(string codetype)
    {
      TableName = "CodeMaster";
      CodeType = codetype;
      if (CodeType == null)
        CodeType = "MASTER";
      DefaultValues.Add("StartDate", new DateTime(1900, 1, 1));
      DefaultValues.Add("CodeType", codetype);
      HiddenColumns.Add("CodeType", null);
      parameters.Add("CodeType", codetype);
      load(parameters);
    }
    private void frmCodeMaster_Load(object sender, EventArgs e)
    {
    }
  }
}
