using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using ACG.Common;

namespace ACG.CommonForms
{
  public partial class ctlSearch : UserControl
  {

    #region module data
    private bool _searching = true;
    private bool _selecting = false;
    private bool _collapsed = true;
    private bool _uparrowing = false;
    private bool _navigatingFromList = false;
    private int _collapsedHeight;
    private int _expandedHeight;
    private int _margin = 5;
    private int _maxHeight = 0;
    string _id = string.Empty;
    DateTime _effectiveDate = DateTime.Today;
    string _Name = string.Empty;
   
    #endregion
    public ISearchDataSource SearchExec = null;
    #region public properties
    public int MaxHeight 
    { 
      get { return _maxHeight; } 
      set 
      { 
        _maxHeight = value; 
        _expandedHeight = Math.Min(_expandedHeight, _maxHeight);
          lstSearchList.Height = _expandedHeight - lstSearchList.Top - _margin;
      } 
    }
    public bool Collapsed
    {
      get { return _collapsed; }
      set
      {
        _collapsed = value;
        if (_collapsed)
          this.Height = _collapsedHeight;
        else
        {
          if (_maxHeight > 0)
            _expandedHeight = Math.Min(_expandedHeight, _maxHeight);
          this.Height = _expandedHeight;
          lstSearchList.Height = _expandedHeight - lstSearchList.Top - _margin;
        }
      }
    }
    public new string Text 
    { 
      get { return _id; } 
      set 
      { 
        _id = value;
        _searching = false; // disable text changed event so we dont generate circular events
        if (string.IsNullOrEmpty(_id))
          txtSearch.Text = string.Empty;
        else
        {
          string desc = getFromID(_id);
          setIDandDescription(desc);

          if (ShowCustomerNameWhenSet)
            txtSearch.Text = desc;
        }
      } 
    }
    public string ID { get { return _id; } set { _id = value; } }
    public string Description { get { return _Name; }  }
    public bool ClearSearchWhenComplete { get; set; }
    public bool AutoSelectWhenMatch { get; set; }
    public bool ShowTermedCheckBox 
    { 
      get { return ckIncludeTermed.Visible; } 
      set 
      { 
        ckIncludeTermed.Visible = value; 
        int margin = this.Width - (lstSearchList.Left + lstSearchList.Width);
        if (ckIncludeTermed.Visible)
          txtSearch.Width = ckIncludeTermed.Left - margin - txtSearch.Left;
        else
          txtSearch.Width = btnExpand.Left - txtSearch.Left - margin ;
      } 
    }
    public bool ShowCustomerNameWhenSet { get; set; }
    public bool AddNewMode { get; set; }
    #endregion
    public ctlSearch()
    {
      InitializeComponent();
      AddNewMode = false;
      _collapsedHeight = txtSearch.Top + txtSearch.Height + _margin - 2;
      _maxHeight = _expandedHeight = lstSearchList.Top + lstSearchList.Height + _margin;
      if (this.Parent != null)
        MaxHeight = this.Parent.Height - this.Top;
    }
    #region events

    private void txtSearch_Enter(object sender, EventArgs e)
    {
      _searching = !AddNewMode;
      _navigatingFromList = false;
    }
    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
      if (_searching)
        if (!string.IsNullOrEmpty(txtSearch.Text))
          search(txtSearch.Text, AutoSelectWhenMatch);
    }
    private void txtSearch_Leave(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(txtSearch.Text))
      {
        _id = string.Empty;
        _Name = string.Empty;
      }
      bool extendSearchList = true;
      if (_searching)
      {
        if (!string.IsNullOrEmpty(txtSearch.Text))
        {
          bool gotOne = search(txtSearch.Text, true);
          if (gotOne)
            extendSearchList = false;
        }
      }


      if (!lstSearchList.Focused)
      {
        lstSearchList.Visible = false;
        Collapsed = true;
      }
    }
    private void lstSearchList_DoubleClick(object sender, EventArgs e)
    {
      if (lstSearchList.SelectedItem != null)
        selectItem((string)lstSearchList.SelectedItem);
    }
    private void lstSearchList_Leave(object sender, EventArgs e)
    {
      if (_uparrowing)
        _uparrowing = false;
      else
        if (lstSearchList.SelectedItem != null)
          selectItem((string)lstSearchList.SelectedItem);
      _navigatingFromList = true;
    }
    public event EventHandler<EventArgs> OnSelected;
    private void btnExpand_Click(object sender, EventArgs e)
    {
      /*
       * the problem is that when we press the expand button, if we are coming from the list, we have already selected the item and collapsed, so we don't want to do it again
       */
      if (!_navigatingFromList) 
        expandList(false);
      _navigatingFromList = false;
      if (Collapsed)
        txtSearch.Focus();
    }
    private void txtSearch_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Down)
        expandList(true);
    }
    private void lstSearchList_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Up)
      {
        if (lstSearchList.Items.Count == 0 || lstSearchList.SelectedIndex == 0)
        {
          e.SuppressKeyPress = true;
          txtSearch.Focus();
          txtSearch.SelectionStart = txtSearch.Text.Length;
          _uparrowing = true;
        }
      }

    }
    #endregion
    #region private methods
    /// <summary>
    /// Returns true if it found and selected an entity
    /// </summary>
    /// <param name="criteria"></param>
    /// <param name="autoSelect"></param>
    /// <returns></returns>
    private bool search(string criteria, bool autoSelect)
    {
      Collapsed = false;
      lstSearchList.Visible = true;
      string[] results = SearchExec.Search(criteria); // results = get data here
      lstSearchList.Items.Clear();
      if (results.GetLength(0) == 1 && autoSelect) // there are exactly one result
      {
        _selecting = false;
        selectItem((string)results[0]);
        return true;
      }
      else
      {
        _searching = true;
        lstSearchList.SuspendLayout();
        lstSearchList.Items.AddRange(results);
        lstSearchList.ResumeLayout();
        lstSearchList.Refresh();
        return false;
      }
    }
    private string getFromID(string id)
    {
      string[] list = SearchExec.Search(id);
      if (list != null && list.GetLength(0) > 0)
        return list[0];
      return string.Empty;
    }
    private void selectItem(string s)
    {
      if (!_selecting)
      {
        setIDandDescription(s);
        if (ClearSearchWhenComplete)
          txtSearch.Text = string.Empty;
        else
          if (ShowCustomerNameWhenSet)
            txtSearch.Text = s;
        Collapsed = true;
        if (OnSelected != null)
          OnSelected(this, new EventArgs());
        if (this.Parent != null)
          this.Parent.SelectNextControl(this, true, false, true, true);
        _selecting = false;
      }
    }
    private void setIDandDescription(string s)
    {
      _selecting = true;

      if (s.Contains(":"))// we have a delimiter
      {
        string[] parts = s.Split(new char[] { ':' });
        if (parts == null)
          _id = _Name = s;
        else
        {
          _id = parts[0];
          _Name = parts[1];
        }
      }
      else
        _id = _Name = s;
      _searching = false;

    }
    private void expandList(bool forceExpand)
    {
      if (forceExpand)
        Collapsed = false;
      else
        if (Collapsed)
          search("*", AutoSelectWhenMatch);
        else
          Collapsed = true;
      if (Collapsed)
        txtSearch.Focus();
      else
      {
        lstSearchList.Focus();
        if (!string.IsNullOrEmpty(txtSearch.Text) && lstSearchList.Items.Contains(txtSearch.Text)) // we already had an item selected
          lstSearchList.Text = txtSearch.Text; // then set the item in the list
        else
          if (lstSearchList.Items.Count > 0)
            lstSearchList.SelectedIndex = 0;
      }
    }
    #endregion

    private void lstSearchList_Enter(object sender, EventArgs e)
    {
      _navigatingFromList = true;
    }

  }
}
