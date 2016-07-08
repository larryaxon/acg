﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using ACG.Common;

namespace CCI.DesktopClient.Common
{
  public partial class ctlGroupMembers : UserControl
  {
    private string _memberDescription = "Member";
    private string _groupDescription = "Group";
    private bool _displaySearch = false;

    protected bool _isGroup = false;
    protected bool _merge = true;
    protected bool _rightClickEnabled = false;

    public bool Merge
    {
      get { return _merge; }
      set { _merge = value; }
    }

    public string SelectedMember { get; set;  }
    public ArrayList Members = new ArrayList();
    public ArrayList NonMembers = new ArrayList();
    public enum MoveDirection { ToMember, FromMember }
    public string GroupDescription
    {
      get { return _groupDescription; }
      set { _groupDescription = value; setCaptions(); }
    }
    public string MemberDescription
    {
      get { return _memberDescription; }
      set { _memberDescription = value; setCaptions(); }
    }
    public bool CanSwap { get { return btnSwap.Visible; } set { btnSwap.Visible = value; } }
    public ISearchDataSource SearchDataSource { get { return srchMember.SearchExec; } set { srchMember.SearchExec = value; if (value != null) DisplaySearch = true; } }
    public bool DisplaySearch 
    { 
      get { return _displaySearch; } 
      set 
      { 
        _displaySearch = value;
        if (_displaySearch)
        {
          if (!srchMember.Visible)
          {
            srchMember.Visible = true;
            lstMembers.Height = splitDealerCustomers.Height - srchMember.Height - srchMember.Top - 10;
            lstMembers.Top = srchMember.Top + srchMember.Height + 5;
          }
        }
        else
        {
          if (srchMember.Visible)
          {
            srchMember.Visible = false;
            lstMembers.Top = lblMembers.Top + lblMembers.Height + 5;
            lstMembers.Height = splitDealerCustomers.Height - lstMembers.Top - 5;
          }
        }
      } 
    }

    public ctlGroupMembers()
    {
      InitializeComponent();
      SelectedMember = null;
    }

    public void Init()
    {
      lstMembers.Items.Clear();
      lstMembers.Items.AddRange(Members.ToArray());
      lstNonMembers.Items.Clear();
      lstNonMembers.Items.AddRange(NonMembers.ToArray());
      setCaptions();
    }
    public void Init(ArrayList lists)
    {
      if (lists != null && lists.Count == 2)
      {
        object oMember = lists[0];
        if (oMember.GetType() == typeof(ArrayList))
          Members = (ArrayList)oMember;
        else
          Members.Clear();
        oMember = lists[1];
        if (oMember.GetType() == typeof(ArrayList))
          NonMembers = (ArrayList)oMember;
        else
          NonMembers.Clear();
      }
      Init();
    }
    public void insertItem(ListBox lst, string item)
    {
      string desc = getDesc(item);
      int iPos = 0;
      foreach (string lstItem in lst.Items)
      {
        // look through list until we find the one AFTER the alpha order
        if (desc.CompareTo(getDesc(lstItem)) <= 0)
          break;
        else
          iPos++;
      }
      lst.Items.Insert(iPos, item);
    }
    public void Clear()
    {
      lstMembers.Items.Clear();
      lstNonMembers.Items.Clear();
    }

    public event EventHandler<EventArgs> OnSelected;

    public static string getID(string member)
    {
      int iPos = member.IndexOf(':');
      if (iPos > 0)
        return member.Substring(0, iPos);
      else
        return member;
    }
    protected string getDesc(string member)
    {
      int iPos = member.IndexOf(':');
      if (iPos > 0 && iPos < member.Length - 2)
        return member.Substring(iPos + 1);
      else
        return member;
    }
    protected virtual void saveMove(string member, MoveDirection direction)
    {
    }
    protected virtual void saveAll() {}
    protected virtual void addGroup()
    {
    }
    protected void swapMembers()
    {
      ArrayList saveList = (ArrayList)Members.Clone();
      Members = NonMembers;
      NonMembers = saveList;
      lstMembers.Items.Clear();
      lstMembers.Items.AddRange((string[])Members.ToArray(typeof(string)));
      lstNonMembers.Items.Clear();
      lstNonMembers.Items.AddRange((string[])NonMembers.ToArray(typeof(string)));
      bool saveMerge = _merge;
      _merge = false; // don't merge, cause we are swapping
      saveAll();
      _merge = saveMerge;
    }

    private void moveMember(MoveDirection direction)
    {
      ListBox fromList;
      ListBox toList;
      ArrayList fromArray;
      ArrayList toArray;
      switch (direction)
      {
        case MoveDirection.FromMember:
          fromList = lstMembers;
          toList = lstNonMembers;
          fromArray = Members;
          toArray = NonMembers;
          break;
        default:
          fromList = lstNonMembers;
          toList = lstMembers;
          fromArray = NonMembers;
          toArray = Members;
          break;
      }
      if (fromList.SelectedItems != null && fromList.SelectedItems.Count > 0)
      {
        ArrayList list = new ArrayList(); // first make a copy of the list since we can't modify it inside its own foreach
        foreach (string item in fromList.SelectedItems)
        {
          list.Add(item);
        }
        foreach (string item in list)
        {
          fromList.Items.Remove(item);
          insertItem(toList, item);
          //toList.Items.Add(item);
          fromArray.Remove(item);
          toArray.Add(item);
          // if we have just a few items, we move them one at a time
          if (list.Count < 20)
            saveMove(getID(item), direction);
        }
        // otherwise, we move them in bulk
        if (list.Count >= 20)
          saveAll();
      }
      else
        MessageBox.Show("No items were selected");
    }
    private void setCaptions()
    {
      string belongsTo =  _isGroup ? "in" : "tied to";
      const string caption = "{0}s that are {3}{2} this {1}";
      string memberDescription = string.IsNullOrEmpty(_groupDescription) ? _isGroup ? "Group" : "Member" : _groupDescription;
      string groupDescription = string.IsNullOrEmpty(_memberDescription) ? _isGroup ? "Group" : "Member" : _memberDescription;

      lblMembers.Text = string.Format(caption, memberDescription, groupDescription, belongsTo, string.Empty);
      lblNonMembers.Text = string.Format(caption, memberDescription, groupDescription, belongsTo, "NOT ");
    }

    private void btnAddMember_Click(object sender, EventArgs e)
    {
      moveMember(MoveDirection.ToMember); 
    }
    private void btnSubtractMember_Click(object sender, EventArgs e)
    {
      moveMember(MoveDirection.FromMember);
    }
    private void btnAddGroup_Click(object sender, EventArgs e)
    {
      addGroup();
    }
    private void btnSwap_Click(object sender, EventArgs e)
    {
      swapMembers();
    }
    private void memberTogglePrimary_Click(object sender, EventArgs e)
    {
      toggleSet();
    }

    private void lstMembers_MouseDown(object sender, MouseEventArgs e)
    {
      lstMembers.SelectedIndex = lstMembers.IndexFromPoint(e.Location);
      if (e.Button == MouseButtons.Right && _rightClickEnabled)
      {
        //select the item under the mouse pointer

        if (lstMembers.SelectedIndex != -1)
        {
          memberRightClickMenu.Show(this, e.Location);
        }
      }
      else if (e.Button == MouseButtons.Left)
      {
        if (OnSelected != null)
        {
          SelectedMember = ACG.Common.CommonFunctions.CString(lstMembers.Items[lstMembers.SelectedIndex]);
          OnSelected(this, new EventArgs());
        }
      }
    }
    protected virtual void toggleSet() { }

    private void srchMember_OnSelected(object sender, EventArgs e)
    {
      string selectedItem = string.Format("{0}: {1}",srchMember.Text.Trim(), srchMember.Description.Trim());
      lstNonMembers.SelectedItem = selectedItem; // select the item in the nommembers list so move knows which one to move
      moveMember(MoveDirection.ToMember);
      //lstMembers.Items.Add(selectedItem);
      //NonMembers.Remove(selectedItem); // this SHOULD remove the item from the search control's list, too, since it is a reference
      //lstNonMembers.Items.Remove(selectedItem);
    }
  }
}
