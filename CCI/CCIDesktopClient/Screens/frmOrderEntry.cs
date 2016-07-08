using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ACG.CommonForms;

using CCI.Common;
using CCI.Sys.Data;

namespace CCI.DesktopClient.Screens
{
  public partial class frmOrderEntry : ScreenBase
  {
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }

    public frmOrderEntry()
    {
      InitializeComponent();
      srchOrder.SearchExec = new SearchDataSourceOrder();
      srchCustomer.SearchExec = new SearchDataSourceCustomer();
    }

    private void srchOrder_OnSelected(object sender, EventArgs e)
    {
      loadEntityGrid();
      populateEntityList();
      Dictionary<string, string[]> criteria = new Dictionary<string, string[]>(StringComparer.CurrentCultureIgnoreCase);
      string orderid = srchOrder.ID; // format is Order-9999, so we strip off the Order- to get the id
      orderid = orderid.Substring(orderid.IndexOf("-") + 1);
      criteria.Add("OrderID", new string[] { ctlSearchGrid.opEQUALS, orderid });
      ctlOrderDetail.SearchCriteria = criteria;
      ctlOrderDetail.ReLoad();
      CCITable orderHeader = _dataSource.getOrderHeader(orderid, null);
      string customerID = string.Empty;
      if (orderHeader.NumberRows == 1)
        customerID = CommonFunctions.CString(orderHeader[0, "Customer"]);
      srchCustomer.Text = customerID;
      ctlOrderFollowUps.Init(orderid, customerID);    
    }

    private void cboEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
      loadEntityGrid();
    }

    private void loadEntityGrid()
    {
      string entityType;
      switch (cboEntityType.Text)
      {
        case "Vendor":
          entityType = "Payor";
          break;
        case "CCI Resource":
          entityType = "User";
          break;
        default:
          entityType = cboEntityType.Text;
          break;
      }
      ctlEntities.EntityType = entityType;
      ctlEntities.EntityOwner = srchOrder.ID;
      ctlEntities.Init(srchOrder.ID, "Order");
    }

    private void frmOrderEntry_Load(object sender, EventArgs e)
    {
      cboEntityType.Text = "Contact";
      ctlOrderDetail.Init("OrderDetailScreen", null);
    }

    private void populateEntityList()
    {
      ArrayList allEntities = new ArrayList();
      foreach (string entityType in cboEntityType.Items)
      {
        ArrayList members = _dataSource.getMembers(srchOrder.ID, "Entity", entityType);
        foreach (string member in members)
          allEntities.Add(string.Format("{0}: {1}",entityType, member));
      }
      lstEntities.Items.Clear();
      lstEntities.Items.AddRange((object[])allEntities.ToArray());
    }

    private void ctlEntities_DataChanged(object sender, EventArgs e)
    {
      populateEntityList();
    }

  }
}
