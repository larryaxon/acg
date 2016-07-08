using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ACG.App.Common;

namespace ACG.DesktopClient.Common
{
  public partial class ctlExceptionsTreeView : ctlTreeBase
  {
    DataSet _ds = null;
    public enum ModeList { PayorCustomer, CustomerPayor, PayCodePayor, PayCodeCustomer, CustomerPayCode };
    public ModeList Mode { get; set; }
    public ctlExceptionsTreeView()
    {
      Mode = ModeList.PayorCustomer;
      InitializeComponent();
    }

    public void LoadTree(DataSet ds)
    {
      ctlExceptionsTreeView tv = this;
      tv.SuspendLayout();
      tv.Nodes.Clear();
      TreeNode root = tv.Nodes.Add("root", "/");
      
      if (root == null)
        return;
      if (ds == null || ds.Tables.Count == 0)
        return;
      _ds = ds;
      DataView view = _ds.Tables[0].DefaultView;
      switch (Mode)
      {
        case ModeList.CustomerPayor:
          view.Sort = "CustomerName, PayorName, PayCode";
          break;
        case ModeList.CustomerPayCode:
          view.Sort = "CustomerName, PayCode, PayorName";
          break;
        case ModeList.PayCodeCustomer:
          view.Sort = "PayCode, CustomerName, PayorName";
          break;
        case ModeList.PayCodePayor:
          view.Sort = "PayCode, PayorName, CustomerName";
          break;
        case ModeList.PayorCustomer:
          view.Sort = "PayorName, CustomerName, PayCode";
          break;
      }
      DataTable table = view.Table;
      string lastPayor = string.Empty;
      string lastCustomer = string.Empty;
      string lastPaycode = string.Empty;
      TreeNode tvwPayor = null;
      TreeNode tvwCustomer = null;
      TreeNode tvwPaycode = null;
      bool hasBreak = true;
      foreach (DataRowView row in view)
      //for (int k1 = 0; k1 < table.Rows.Count; k1++)
      {
        //DataRow row = table.Rows[k1];
        string payor = CommonFunctions.CString(row["Payor"]);
        string payorName = CommonFunctions.CString(row["PayorName"]);
        string payorDesc = string.Format("{0}({1})", payorName, payor);
        string customer = CommonFunctions.CString(row["Customer"]);
        string customerName = CommonFunctions.CString(row["CustomerName"]);
        string customerDesc = string.Format("{0}({1})", customerName, customer);
        string paycode = CommonFunctions.CString(row["PayCode"]);
        decimal actualComm = CommonFunctions.CDecimal(row["ActualCommission"]);
        decimal scheduledComm = CommonFunctions.CDecimal(row["ScheduledCommission"]);
        switch (Mode)
        {
          case ModeList.PayorCustomer:
            if (payor.Equals(lastPayor)) // time for a new payor node
              hasBreak = false;
            else
            {
              tvwPayor = NewNode("Payor", payorDesc);
              root.Nodes.Add(tvwPayor);
              hasBreak = true;
            }            
            if (!customer.Equals(lastCustomer) || hasBreak) // time for a new customer node
            {
              tvwCustomer = NewNode("Customer", customerDesc);
              tvwPayor.Nodes.Add(tvwCustomer);
            }
            tvwPaycode = NewNode("PayCode", paycode, scheduledComm, actualComm);
            tvwCustomer.Nodes.Add(tvwPaycode);
            break;
          case ModeList.CustomerPayor:
            if (customer.Equals(lastCustomer)) // time for a new customer node
              hasBreak = false;
            else
            {
              tvwCustomer = NewNode("Customer", customerDesc);
              root.Nodes.Add(tvwCustomer);
              hasBreak = true;
            }
            if (!payor.Equals(lastPayor) || hasBreak) // time for a new payor node
            {
              tvwPayor = NewNode("Payor", payorDesc);
              tvwCustomer.Nodes.Add(tvwPayor);
            }
            tvwPaycode = NewNode("PayCode", paycode, scheduledComm, actualComm);
            tvwPayor.Nodes.Add(tvwPaycode);
            break;
          case ModeList.CustomerPayCode:
            if (customer.Equals(lastCustomer)) // time for a new customer node
              hasBreak = false;
            else
            {
              tvwCustomer = NewNode("Customer", customerDesc);
              root.Nodes.Add(tvwCustomer);
              hasBreak = true;
            }
            if (!paycode.Equals(lastPaycode) || hasBreak)
            {
              tvwPaycode = NewNode("PayCode", paycode);
              tvwCustomer.Nodes.Add(tvwPaycode);
            }
            tvwPayor = NewNode("Payor", payorDesc, scheduledComm, actualComm);
            tvwPaycode.Nodes.Add(tvwPayor);
            break;
          case ModeList.PayCodeCustomer:
            if (paycode.Equals(lastPaycode))
              hasBreak = false;
            else
            {
              tvwPaycode = NewNode("PayCode", paycode);
              root.Nodes.Add(tvwPaycode);
              hasBreak = true;
            }
            if (!customer.Equals(lastCustomer) || hasBreak)
            {
              tvwCustomer = NewNode("Customer", customerDesc);
              tvwPaycode.Nodes.Add(tvwCustomer);
            }
            tvwPayor = NewNode("Payor", payorDesc, scheduledComm, actualComm);
            tvwCustomer.Nodes.Add(tvwPayor);
            break;
          case ModeList.PayCodePayor:
            if (paycode.Equals(lastPaycode))
              hasBreak = false;
            else
            {
              tvwPaycode = NewNode("PayCode", paycode);
              root.Nodes.Add(tvwPaycode);
              hasBreak = true;
            }
            if (!payor.Equals(lastPayor) || hasBreak)
            {
              tvwPayor = NewNode("Payor", payorDesc);
              tvwPaycode.Nodes.Add(tvwPayor);
            }
            tvwCustomer = NewNode("Customer", customerDesc, scheduledComm, actualComm);
            tvwPayor.Nodes.Add(tvwCustomer);
            break;
        }


        lastPayor = payor;
        lastCustomer = customer;
        lastPaycode = paycode;
      }
      expandPreviouslyExpandedNodes();
      tv.ResumeLayout();
    }
    private TreeNode NewNode(string nodeType, string nodeName)
    {
      return NewNode(nodeType, nodeName, false, 0, 0);
    }
    private TreeNode NewNode(string nodeType, string nodeName, decimal scheduledValue, decimal actualValue)
    {
      return NewNode(nodeType, nodeName, true, scheduledValue, actualValue);
    }
    private TreeNode NewNode(string nodeType, string nodeName, bool isLeaf, decimal scheduledValue, decimal actualValue)
    {
      TreeNode node;
      if (isLeaf)
        node = new TreeNode(string.Format("{0}: {1}, Scheduled: {2}, Actual: {3}",nodeType, nodeName, actualValue.ToString(), scheduledValue.ToString()));
      else
        node = new TreeNode(string.Format("{0}: {1}", nodeType, nodeName));
      return node;
    }
  }
}
