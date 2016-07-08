using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using CCI.Sys.Data;

namespace CCI.DesktopClient.Common
{
  public partial class ctlOpportunityLineItems : ctlMaintenanceBase
  {
    #region module data
    private const string colOPPORTUNITYID = "OpportunityID";
    private const string colSEQUENCE = "Sequence";
    private const string colPRODUCT = "OpportunityProduct";
    private const string colNote = "Note";
    private const string CODEPRODUCT = "OPITEMS";
    private DataGridViewComboBoxColumn _productCell = new DataGridViewComboBoxColumn();
    private string[] _productList = null;
    private Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    #endregion
    public string OpportunityID { get; set; }
    public ctlOpportunityLineItems()
    {
      TableName = "OpportunityLineItems";
      DefaultValues.Add("StartDate", new DateTime(1900, 1, 1));
      InitializeComponent();
      ReadOnlyColumns.Add(colOPPORTUNITYID, null);
      HiddenColumns.Add(colSEQUENCE, null);
    }

    public void Init(string opportunityID)
    {
      if (_productList == null)
        _productList = _dataSource.getCodeList(CODEPRODUCT);
      if (_productCell.Items.Count == 0)
        _productCell.Items.AddRange(_productList);
      if (!PickLists.ContainsKey(colPRODUCT))
        PickLists.Add(colPRODUCT, _productCell);
      parameters.Clear();
      OpportunityID = opportunityID;
      parameters.Add(colOPPORTUNITYID, opportunityID);
      if (DefaultValues.ContainsKey(colOPPORTUNITYID))
        DefaultValues[colOPPORTUNITYID] = opportunityID;
      else
        DefaultValues.Add(colOPPORTUNITYID, opportunityID);
      load(parameters);
      int seq = getNextSequence();
      if (DefaultValues.ContainsKey(colSEQUENCE))
        DefaultValues[colSEQUENCE] = seq;
      else
        DefaultValues.Add(colSEQUENCE, seq);
      if (!HiddenColumns.ContainsKey(colSEQUENCE))
        HiddenColumns.Add(colSEQUENCE, null);
      if (grdMaintenance.Columns.Contains("EstimatedMRC"))
      {
        DataGridViewCellStyle style = new DataGridViewCellStyle();
        style.Format = "c";
        grdMaintenance.Columns["EstimatedMRC"].DefaultCellStyle = style;
      }
    }

    private int getNextSequence()
    {
      int nextSequence = 0;
      foreach (DataGridViewRow row in grdMaintenance.Rows)
      {
        int seq = CommonFunctions.CInt(row.Cells[colSEQUENCE].Value, 0);
        nextSequence = Math.Max(seq, nextSequence);
      }
      return nextSequence + 1;
    }

    public new void load(Dictionary<string, string> parameters)
    {
      base.load(parameters);
      foreach (DataGridViewRow row in grdMaintenance.Rows)
      {
        DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)_productCell.Clone();
        cell.Value = CommonFunctions.CString(row.Cells[colPRODUCT].Value);
        row.Cells[colPRODUCT] = cell;
      }
    }

  }
}
