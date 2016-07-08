using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using CCI.Sys.Data;

using TAGBOSS.Common.Model;

namespace CCI.DesktopClient.Common
{
  public partial class ctlEntitySearch : UserControl
  {
    #region module data
    private bool _searching = true;
    private bool _selecting = false;
    private bool _collapsed = true;
    private int _collapsedHeight;
    private int _expandedHeight;
    private int _margin = 5;
    private int _maxHeight = 0;
    string _entity = string.Empty;
    DateTime _effectiveDate = DateTime.Today;
    string _shortName = string.Empty;
    string _legalName = string.Empty;
    string _entityType = string.Empty;
    string _entityOwner = "CCI"; // plug as default value for now
    private DataSource _dSource = null;
    protected DataSource _dataSource { get { if (_dSource == null) _dSource = new DataSource(); return _dSource; } }
    #endregion
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
      get { return _entity; } 
      set 
      { 
        _entity = value;
        if (ShowCustomerNameWhenSet)
          txtSearch.Text = _dataSource.getEntityName(_entity);
      } 
    }
    public string Entity { get { return _entity; } set { _entity = value; } }
    public string EntityType { get { return _entityType; } set { _entityType = value;  } }
    public string EntityOwner { get { return _entityOwner; } set { _entityOwner = value; } }
    public string LegalName { get { return _legalName; }  }
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
          txtSearch.Width = this.Width - txtSearch.Left - margin;
      } 
    }
    public bool ShowCustomerNameWhenSet { get; set; }
    #endregion
    public ctlEntitySearch()
    {
      ClearSearchWhenComplete = true;
      ShowCustomerNameWhenSet = false;
      AutoSelectWhenMatch = false;
      InitializeComponent();
      _collapsedHeight = txtSearch.Top + txtSearch.Height + _margin;
      _maxHeight = _expandedHeight = lstSearchList.Top + lstSearchList.Height + _margin;
      if (this.Parent != null)
        MaxHeight = this.Parent.Height - this.Top;
    }
    #region events

    private void txtSearch_Enter(object sender, EventArgs e)
    {
      _searching = true;
    }
    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
      if (_searching)
        if (!string.IsNullOrEmpty(txtSearch.Text))
          search(txtSearch.Text, AutoSelectWhenMatch);
    }
    private void txtSearch_Leave(object sender, EventArgs e)
    {
      if (_searching)
      {
        if (!string.IsNullOrEmpty(txtSearch.Text))
          search(txtSearch.Text, true);
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
        selectEntity((SearchResult)lstSearchList.SelectedItem);
    }

    public event EventHandler<EventArgs> OnSelected;

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
      if (string.IsNullOrEmpty(_entityType))
        _entityType = null; // let searh know we want ALL entity types
      SearchResultCollection results;
      if (string.IsNullOrEmpty(_entityOwner) || _entityOwner.Equals("CCI", StringComparison.CurrentCultureIgnoreCase))
        results = _dataSource.SearchEntities(criteria, _entityType, ckIncludeTermed.Checked);
      else
        results = _dataSource.SearchEntities(criteria, _entityType, _entityOwner, ckIncludeTermed.Checked);
      lstSearchList.Items.Clear();
      lstSearchList.ValueMember = "EntityID";
      lstSearchList.DisplayMember = "ShortName";
      if (results.Count == 1 && autoSelect) // there are exactly one result
      {
        selectEntity(results[0]);
        return true;
      }
      else
      {
        _searching = true;
        lstSearchList.SuspendLayout();
        foreach (SearchResult s in results)
        {
          lstSearchList.Items.Add(s);
        }
        lstSearchList.ResumeLayout();
        lstSearchList.Refresh();
        return false;
      }
    }
    private void selectEntity(SearchResult s)
    {
      if (!_selecting)
      {
        _selecting = true;
        _entity = s.EntityID;
        _legalName = s.LegalName;
        _shortName = s.ShortName;
        _searching = false;
        if (ClearSearchWhenComplete)
          txtSearch.Text = string.Empty;
        else
          txtSearch.Text = _shortName;
        Collapsed = true;
        if (OnSelected != null)
          OnSelected(this, new EventArgs());
        if (this.Parent != null)
          this.Parent.SelectNextControl(this, true, false, true, true);
        _selecting = false;
      }
    }
    #endregion

    private void lstSearchList_Leave(object sender, EventArgs e)
    {
      if (lstSearchList.SelectedItem != null)
        selectEntity((SearchResult)lstSearchList.SelectedItem);
    }

  }
}
