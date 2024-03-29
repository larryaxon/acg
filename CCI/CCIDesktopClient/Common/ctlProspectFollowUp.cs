﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using CCI.Sys.Data;

using TAGBOSS.Common.Model;

namespace CCI.DesktopClient.Common
{
  public partial class ctlProspectFollowUp : ctlMaintenanceBase
  {
    public const string FUTYPEPROSPECT = "Prospect";
    public const string FUTYPEIBP = "Dealer";
    public const string FUTYPEFUNNEL = "Funnel";
    private string _fuType = FUTYPEPROSPECT;
    private const string IBPENTITYTYPE = "Payee";
    private const string REPENTITYTYPE = "User";
    private const string colIBP = "Dealer";
    private const string colREP = "Rep";
    private const string colCREATEDBY = "CreatedBy";
    private const string colCREATEDDATETIME = "CreatedDateTime";
    private const string colOPPORTUNITYID = "OpportunityID";
    Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
    public string FUtype { get { return _fuType; } set { _fuType = value; } }
    public string Rep { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }
    public ctlProspectFollowUp()
    {
      InitializeComponent();
      ReadOnlyColumns.Add("Customer Name", null);
      ReadOnlyColumns.Add("Opportunity Name", null);
      ReadOnlyColumns.Add("MRC", null);
      ReadOnlyColumns.Add("Dealer", null);
      ReadOnlyColumns.Add("Due", null);
      Status = "Active";
    }

    public void Init(string rep)
    {
      switch (_fuType)
      {
        case FUTYPEPROSPECT:
          TableName = "ProspectFollowUps";
          break;
        case FUTYPEIBP:
          TableName = "IBPFollowUps";
          break;
        case FUTYPEFUNNEL:
          TableName = "Funnel";
          break;
      }
      parameters.Clear();
      Rep = rep;
      parameters.Add("Rep", rep);
      parameters.Add("StartDate", StartDate.ToShortDateString());
      parameters.Add("EndDate", EndDate.ToShortDateString());
      parameters.Add("Status", Status);
      load(parameters);
      if (grdMaintenance.Columns.Contains("MRC"))
        grdMaintenance.Columns["MRC"].DefaultCellStyle.Format = "c";
    }

  }
}
