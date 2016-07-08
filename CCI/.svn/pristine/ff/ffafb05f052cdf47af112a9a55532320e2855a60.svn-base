using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using CCI.Sys.Data;
using CCI.DesktopClient.Common;

namespace CCI.DesktopClient.Screens
{
  public partial class frmGroups : ScreenBase
  {
    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    private ArrayList _memberList = null;
    private ArrayList _nonMemberList = null;
    private string _entityType = "Customer";

    public string EntityType
    {
      get { return _entityType; }
      set { _entityType = value; ctlManageGroup.MemberDescription = _entityType; }
    }
    public frmGroups()
    {
      InitializeComponent();
      this.Text = "Manage Groups";
      ctlManageGroup.GroupDescription = "Group";
      txtGroup.AddNewMode = true;
    }

    private void frmGroups_Load(object sender, EventArgs e)
    {
      txtGroup.SearchExec = new SearchDataSourceEntity("Group", "CCI");
    }

    public void Init(ArrayList idList)
    {
      if (idList.Count > 0)
        _memberList = _dataSource.getEntityNames(idList);
      else
        _memberList = new ArrayList();
      _nonMemberList = _dataSource.getMembers(null, "Entity", _entityType);
      foreach (string member in _memberList)
        _nonMemberList.Remove(member);
      ctlManageGroup.Members = _memberList;
      ctlManageGroup.NonMembers = _nonMemberList;
      ctlManageGroup.Init();
    }

    private ArrayList getGroupMembers(string group)
    {
      return _dataSource.getMembers(group, "Entity", _entityType);
    }

    private void txtGroup_OnSelected(object sender, EventArgs e)
    {
      txtGroupName.Text = txtGroup.Description;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      string groupID = txtGroup.Text;
      if (txtGroup.CreatedNewItem)
        groupID = _dataSource.MakeNewEntityID(groupID);
       _dataSource.createGroup(groupID, txtGroupName.Text, "Customer", SecurityContext.User);
       int? ret = _dataSource.updateMembers(groupID, _memberList, true, !ckClear.Checked, SecurityContext.User);
       if (ret == -1)
         MessageBox.Show("Error saving group members");
       else
         MessageBox.Show("Group members saved");
    }
  }
}
