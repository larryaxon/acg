﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using CCI.Sys.Data;
using CCI.DesktopClient.Screens;
using Microsoft.VisualBasic;

namespace CCI.DesktopClient.Common
{
  public partial class ctlEntityGroupMembers : ctlGroupMembers
  {

    private DataSource _ds = null;
    private DataSource _dataSource { get { if (_ds == null) _ds = new DataSource(); return _ds; } }
    private string _entityType = string.Empty;
    private string _groupEntityType = string.Empty;
    private bool _includeGrandChilren = false;
    private string _entity = string.Empty;

    public ACG.Common.ISecurityContext SecurityContext { get; set; }
    public string Entity
    {
      get { return _entity; }
      set { _entity = value; }
    }
    public bool IsGroup
    {
      get { return _isGroup; }
      set { _isGroup = value; }
    }
    public string GroupEntityType
    {
      get { return _groupEntityType; }
      //set { _groupEntityType = value; MemberDescription = _isGroup ?  "Group" : _groupEntityType; GroupDescription = _isGroup ? _groupEntityType : "Group" ; }
      set { _groupEntityType = value; MemberDescription = _isGroup ? _entityType : _groupEntityType; GroupDescription = _isGroup ? _groupEntityType : _entityType; }
    }
    public string EntityType
    {
      get { return _entityType; }
      //set { _entityType = value; MemberDescription = _isGroup ? "Group" : _entityType; GroupDescription = _isGroup ? _entityType : "Group"; }
      set { _entityType = value; MemberDescription = _isGroup ? _groupEntityType : _entityType; GroupDescription = _isGroup ? _entityType : _groupEntityType; }
    }
    public bool IncludeGrandChilren
    {
      get { return _includeGrandChilren; }
      set { _includeGrandChilren = value; }
    }    
    
    public ctlEntityGroupMembers()
    {
      InitializeComponent();
      btnAddGroup.Visible = false;
      _rightClickEnabled = true;
    }

    public new void Init()
    {
      if (!string.IsNullOrEmpty(_entity))
      {
        ScreenBase frm = (ScreenBase)CommonFormFunctions.getParentForm(this);
        SecurityContext = frm.SecurityContext;
        string memberType = _isGroup ? "Group" : "Entity";
        string groupType = _isGroup ? "Entity" : "Group";
        Members = _dataSource.getMembers(_entity, groupType, _entityType, _includeGrandChilren);
        NonMembers = _dataSource.getMembers(null, groupType, _isGroup ? _entityType : "Group" );
        foreach (string member in Members)
          NonMembers.Remove(member);
        SearchDataSource = new SearchDataSourceEntityList(NonMembers);
        base.Init();
      }
    }
    public string MakeNewEntityID(string newEntityName)
    {
      return _dataSource.MakeNewEntityID(newEntityName);
    }

    protected override void saveMove(string member, MoveDirection direction)
    {
      string mem = _isGroup ? _entity : getID(member);
      string grp = _isGroup ? getID(member) : _entity;
      if (direction == MoveDirection.FromMember)
        _dataSource.terminateGroupMember(mem, grp, SecurityContext.User);
      else
        if (!_dataSource.existsGroupMember(mem, grp)) // if it is not there then add it
        {
          int? id = _dataSource.getMember(mem, grp);
          _dataSource.updateGroupMember(id == null ? null : ((int)id).ToString(), mem, grp, null, SecurityContext.User);
        }
    }
    protected override void saveAll()
    {
      int? ret = _dataSource.updateMembers(Entity, Members, true, _merge, SecurityContext.User);
    }
    protected override void addGroup()
    {
      string grpName = Interaction.InputBox("Enter a new Group Name", "Add Group", "Default Text");
      _dataSource.createGroup(MakeNewEntityID(grpName), grpName, _groupEntityType, SecurityContext.User);
      MessageBox.Show("Group Added");
      Init();
    }
    protected override void toggleSet()
    {
      int i = lstMembers.SelectedIndex;
      if (i >= 0)
      {
        string member = lstMembers.Items[i].ToString();
        bool isPrimary;
        string newmember;
        if (member.Contains("(Primary)")) // this is flagged as primary
        {
          int pos = member.IndexOf("(Primary)");
          newmember = member.Substring(0, pos).Trim();
          isPrimary = false;
        }
        else
        {
          newmember = member + " (Primary)";
          isPrimary = true;
        }
        lstMembers.Items[i] = newmember;
        string memberID = getID(member);
        string msg;
        if (isPrimary)
          msg = "Make {0} the Primary Owner of {1}?";
        else
          msg = "Disconnect {0} from being the Primery Owner of {1}?";
        DialogResult ans = MessageBox.Show(string.Format(msg,_dataSource.getEntityName(Entity), member));
        if (ans == DialogResult.Yes)
        {
          int? ret = _dataSource.memberSetPrimary(Entity, memberID, isPrimary, SecurityContext.User);
          if (ret == -1)
            MessageBox.Show("Error setting Primary flag");
        }
      }
    }

    private void lstMembers_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index > -1)
      {
        string member = CommonFunctions.CString(lstMembers.Items[e.Index]);
        Color newForeColor = e.ForeColor;
        if (member.EndsWith("(Primary)")) // is a primary
        {
          e.Graphics.FillRectangle(Brushes.Red, e.Bounds);
          newForeColor = Color.White;
        }
        else
          e.DrawBackground();

        using (Brush textBrush = new SolidBrush(newForeColor))
        {
          e.Graphics.DrawString(lstMembers.Items[e.Index].ToString(), e.Font, textBrush, e.Bounds.Location);
        }
      }
    }
  }
}
