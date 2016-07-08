using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TAGBOSS.Common;
using TAGBOSS.Common.Model;
using TAGBOSS.Sys.AttributeEngine2;
using CCI.Sys.Data;
using CCI.Common;
using CCI.DesktopClient.Common;

namespace CCI.DesktopClient.Common
{
  public partial class ctlEntityPicker : UserControl
  {
    #region module data
    private const string cROOTENTITY = "All";
    private bool _allowMultiSelect = true;
    private bool _selectingTree = false;
    private bool _isExpanded = false;
    private bool _isTreeExpanded = false;
    private int _expandedHeight = 400;
    private EntityHierarchy eh = null;
    private Dictionary<string, SelectedEntry> currentEntries = new Dictionary<string, SelectedEntry>();
    private EntityAttributes _ea { get { if (_eAttributes == null) _eAttributes = new EntityAttributes(); return _eAttributes; } }
    private EntityAttributes _eAttributes = null;
    /*
     * The storage for the entity list is a stringbuilder which makes the maintenance more efficient. However, we also 
     * need to keep synchronized with both the text box result (txtEntityList) and the public property,
     * both of which are type string. To keep this automatic and simple, we have this storage variable,
     * a private StringBuilder property which the code here uses and which keeps the text box synchronized, 
     * and the public property (EntityList) which exposes the ToString() of the StringBuilder and uses
     * the private property to set the value.
     */
    private string _selectedEntities = null;
    private string _selectedEntitiesOldValue = null;
    private string entityList
    {
      get { return _selectedEntities; }
      set 
      { 
        _selectedEntities = value;
        txtEntityList.Text = Text;
      }
    }

    #endregion

    #region private classes
    private class SelectedEntry
    {
      private string _entityID = string.Empty;
      private string _longName = string.Empty;
      private string _shortName = string.Empty;
      private string _entityType = string.Empty;
      private string _entityOwner = string.Empty;

      public string EntityID { get { return _entityID; } }

      public SelectedEntry(string id, string name)
      {
        _entityID = id;
        _longName = name;
        _shortName = name;
        _entityType = "none";
      }
      public SelectedEntry(Entity entity)
      {
        if (entity != null)
        {
        _entityID = entity.OriginalID;
        _longName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, entity.Fields.getValue("FullName")) ;
        _shortName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, entity.Fields.getValue("ShortName"));
        _entityType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, entity.Fields.getValue("EntityType"));
        _entityOwner = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, entity.Fields.getValue("EntityOwner"));
        }
      }
      public SelectedEntry(SearchResult entity)
      {
        _entityID = entity.EntityID;
        _longName = entity.FullName;
        _shortName = entity.ShortName;
        _entityType = entity.EntityType;
        _entityOwner = string.Empty;
      }
      public override string ToString()
      {
        if (_entityType.Equals("Employee", StringComparison.CurrentCultureIgnoreCase))
          return string.Format("{0} - {1} ({2})", _longName, _entityOwner, _entityType);
        else
          if (_entityType.Equals("none", StringComparison.CurrentCultureIgnoreCase))
            return _longName;
          else
            return string.Format("{0} ({1})", _longName, _entityType);
      }
    }
    #endregion

    #region public properties

    public bool IsExpanded 
    { 
      get { return _isExpanded; } 
      set 
      {
        _isExpanded = value;
        if (_isExpanded)
          expand();
        else
          collapse();
      } 
    }
    public int ExpandedHeight
    {
      get { return _expandedHeight; }
      set { _expandedHeight = value; }
    }
    public override string Text { get { return (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, entityList); } set { entityList = value; } }
    public bool AllowMultiSelect 
    { 
      get { return _allowMultiSelect; } 
      set 
      { 
        _allowMultiSelect = value;
        if (_allowMultiSelect)
        {
          lstSearchResults.SelectionMode = SelectionMode.MultiExtended;
          lstSelected.SelectionMode = SelectionMode.MultiExtended;
          ckSelectAll.Visible = true;
          //pictMode.Image = global::CCI.DesktopClient.Common.Properties.Resources.userAccounts;
        }
        else
        {
          if (ckSelectAll.Checked)
            ckSelectAll.Checked = false;
          ckSelectAll.Visible = false;
          lstSearchResults.SelectionMode = SelectionMode.One;
          lstSelected.SelectionMode = SelectionMode.One;
          selectOneEntry();
          //pictMode.Image = global::CCI.DesktopClient.Common.Properties.Resources.user;
        }
      } 
    }
    public bool SelectAll { get { return ckSelectAll.Checked; } set { ckSelectAll.Checked = value; } }

    #endregion

    public ctlEntityPicker()
    {
      InitializeComponent();
      //_expandedHeight = Math.Max(_expandedHeight, Math.Max(this.Height, tabView.Top + tabView.Height + 24));
    }
    public void load()
    {
      loadTree();
      string[] entityTypeList = _ea.getValidEntityTypes();
      cboEntityType.Items.Clear();
      cboEntityType.Items.Add(string.Empty);
      foreach (string entityType in entityTypeList)
        cboEntityType.Items.Add(entityType);
      lstSelected.Items.Clear();
    }

    #region form events

    private void txtEntityList_Leave(object sender, EventArgs e)
    {
      refreshFromEntityList();
      _selectedEntitiesOldValue = _selectedEntities;
    }
    private void tvwEntityHierarchy_AfterCheck(object sender, TreeViewEventArgs e)
    {
      if (!_selectingTree)
      {
        if (!_allowMultiSelect && currentEntries.Count > 0 && e.Node.Checked) // can't check one if already one checked and not multiselect
        {
          e.Node.Checked = false;
          return;
        }
        getCheckedEntites();

      }
    }
    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
      Search();
    }
    private void cmdExpand_Click(object sender, EventArgs e)
    {
      if (IsExpanded)
        collapse();
      else
        expand();
    }
    private void cboEntityType_SelectedValueChanged(object sender, EventArgs e)
    {
      Search();
    }
    private void cmdSearch_Click(object sender, EventArgs e)
    {
      refreshFromSearch();
    }
    private void tabView_Selected(object sender, TabControlEventArgs e)
    {
      if (tabView.SelectedIndex == tabView.TabPages.IndexOfKey(tabSelected.Name))
        refreshSelected();
    }
    private void lstSearchResults_SelectedIndexChanged(object sender, EventArgs e)
    {
      refreshFromSearch();
    }
    private void ckSelectAll_CheckedChanged(object sender, EventArgs e)
    {
      if (!_allowMultiSelect && ckSelectAll.Checked)
      {
        ckSelectAll.Checked = false;
        return;
      }
      if (!ckSelectAll.Checked)
        currentEntries.Clear();
      loadTree();
      refreshSelected();
      loadEntityList();
      txtSearch.Text = string.Empty;
      lstSearchResults.Items.Clear();
    }
    private void ckIncludeTerms_CheckedChanged(object sender, EventArgs e)
    {
      eh = null; // we need to reset the entity hierarchy
      loadTree();
    }
    private void cmdRemove_Click(object sender, EventArgs e)
    {
      // first remove the selected rows from current entries
      foreach (SelectedEntry entry in lstSelected.SelectedItems)
      {
        string id = entry.EntityID.ToLower();
        if (currentEntries.ContainsKey(id))
          currentEntries.Remove(id);
      }
      loadEntityList();  // now reload our text box at the top
      loadTree(); // refresh the tree view
      refreshSelected();  // and refresh me
    }
    private void cmdExpandTree_Click(object sender, EventArgs e)
    {
      if (_isTreeExpanded)
      {
        cmdExpandTree.Text = "Expand";
        tvwEntityHierarchy.CollapseAll();
      }
      else
      {
        cmdExpandTree.Text = "Collapse";
        tvwEntityHierarchy.ExpandAll();
      }
      _isTreeExpanded = !_isTreeExpanded;
    }
    private void txtEntityList_Enter(object sender, EventArgs e)
    {
      _selectedEntitiesOldValue = _selectedEntities;
    }

    #endregion

    #region private methods
    private void getCheckedEntites()
    {
      currentEntries.Clear();     // in the tree view, we just replace the list with what we have checked
      getCheckedEntites(tvwEntityHierarchy.Nodes);
      loadEntityList();
    }
    private void getCheckedEntites(TreeNodeCollection nodes)
    {
      foreach (TreeNode node in nodes)
      {
        if (node.Checked)
        {
          string entity = ((SelectedEntry)node.Tag).EntityID;
          
          if (!string.IsNullOrEmpty(entity))
          {
            string id = entity.ToLower();
            if (okToAdd(id))
              currentEntries.Add(id, (SelectedEntry)node.Tag); // we have SelectedEntity object in node.Tag
          }
        }

        if (node.Nodes.Count > 0)
          this.getCheckedEntites(node.Nodes);
      }

    }
    private void getSearchedEntities()
    {
      foreach (SelectedEntry entry in lstSearchResults.SelectedItems)
      {
        string id = entry.EntityID.ToLower();
        if (okToAdd(id))
          currentEntries.Add(id, entry);
      }
      loadEntityList();
    }
    private void loadTree()
    {
      if (eh == null)
        eh = EntityTree();
      loadTree(tvwEntityHierarchy, eh, null, true, true);
      tvwEntityHierarchy.Refresh();
      if (_isTreeExpanded)
        tvwEntityHierarchy.ExpandAll();
    }
    private void loadTree(TreeView tv, EntityHierarchy eh, TreeNode tvw, bool clearTreeNodes, bool addRoot)
    {
      TreeNode root = null;
      TreeNode tvw_en = null;
      if (clearTreeNodes)
      {
        tv.Nodes.Clear();
        TreeNode adminRoot = tv.Nodes.Add("Admin", "Admin");
        TreeNode defaultNode = adminRoot.Nodes.Add("Default", "Default");
        defaultNode.Tag = getSystemEntity("Default");
        TreeNode dictionaryNode = adminRoot.Nodes.Add("Dictionary", "Dictionary");
        dictionaryNode.Tag = getSystemEntity("Dictionary");
        root = adminRoot.Nodes.Add("All", "All");
        root.Tag = getSystemEntity("All");
        addRoot = false;
        tvw = root;
      }
      else
        root = tvw;

      if (root == null)
        return;

      if (addRoot)
      {
        if (!root.Nodes.ContainsKey(eh.ID))
        {
          // create the node
          SelectedEntry entry = new SelectedEntry(eh.Entity);
          tvw_en = root.Nodes.Add(eh.ID, entry.ToString());
          tvw_en.Tag = entry; // put the selected entry object in the tag so we can use it later
          // if Select All is checked and this is not "All", then add this to the current list
          if (ckSelectAll.Checked && !eh.ID.Equals(cROOTENTITY, StringComparison.CurrentCultureIgnoreCase))
          {
            if (okToAdd(eh.ID)) // should not be dups but just in case
              currentEntries.Add(eh.ID, entry);
          }
          if (currentEntries.ContainsKey(eh.ID))  // whether we just added it, or it was there before, it this is in the list then check it
          {
            _selectingTree = true;  // don't fire the checked event.
            tvw_en.Checked = true;
            _selectingTree = false;
          }
        }
      }
      else
        tvw_en = tvw;
      addRoot = true;
      foreach (EntityHierarchy ehChild in eh)
        loadTree(tv, ehChild, tvw_en, false, addRoot);

    }
    private EntityHierarchy EntityTree()
    {
      return EntityTree(cROOTENTITY, 999);
    }
    private EntityHierarchy EntityTree(string entityID, int levels)
    {
      ArrayList dontProcessTypes = new ArrayList();
      dontProcessTypes.Add("Bank");
      EntityHierarchy eh = _ea.getEntityHierarchyChildren(entityID, levels, dontProcessTypes, ckIncludeTerms.Checked); ;
      //EntityHierarchy eh = _ea.EntityChildren(entityID, levels, dontProcessTypes); ;
      return eh;
    }
    private void expand()
    {
      _isExpanded = true;
      //this.cmdExpand.Image = null;
      //this.cmdExpand.Image = global::CCI.DesktopClient.Common.Properties.Resources.Collapse_large;
      tabView.Visible = true;
      //ckIncludeTerms.Visible = true;
      //ckSelectAll.Visible = true;
      this.Height = _expandedHeight; //ckIncludeTerms.Top + ckIncludeTerms.Height + 4;
      tabView.Height = _expandedHeight - tabView.Top - 4;
      OnExpand(new EventArgs());
    }
    private void collapse()
    {
      _isExpanded = false;
      //_expandedHeight = this.Height;
      //this.cmdExpand.Image = global::CCI.DesktopClient.Common.Properties.Resources.Expand_large;
      tabView.Visible = false;
      //ckIncludeTerms.Visible = false;
      //ckSelectAll.Visible = false;
      this.Height = txtEntityList.Top + txtEntityList.Height + 4;
      OnCollapse(new EventArgs());
    }
    private void Search()
    {
      lstSearchResults.Items.Clear();
      string entityType = cboEntityType.Text;
      if (entityType == string.Empty)
        entityType = null;
      string criteria;
      if (ckSelectAll.Checked)
        criteria = "*";
      else
        criteria = txtSearch.Text;
      SearchResultCollection entityList = _ea.Search("All", "All", criteria, entityType, ckIncludeTerms.Checked);
      if (entityList == null || entityList.Count == 0)
        lstSearchResults.Items.Add(new SelectedEntry("none", "No results found"));
      else
      {
        foreach (SearchResult entity in entityList)
          lstSearchResults.Items.Add(new SelectedEntry(entity));
      }
    }
    private void loadEntityList()
    {
      string oldValue = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, _selectedEntitiesOldValue);
      StringBuilder sb = new StringBuilder();
      bool firstTime = true;
      foreach (KeyValuePair<string, SelectedEntry> entry in currentEntries)
      {
        string entity = entry.Value.EntityID;
        if (firstTime)
          firstTime = false;
        else
          sb.Append(",");
        sb.Append(entity);
      }
      Text = sb.ToString();
      if (!oldValue.Equals(_selectedEntities, StringComparison.CurrentCultureIgnoreCase)) // if the list has changed
        if (OnChanged != null)
          OnChanged(this, new EventArgs());
      txtEntityList.Refresh();
      return;
    }
    private void refreshSelected()
    {
      lstSelected.Items.Clear();
      foreach (KeyValuePair<string, SelectedEntry> entry in currentEntries)
        lstSelected.Items.Add(entry.Value);
      lstSelected.Refresh();
    }
    private void refreshFromSearch()
    {
      getSearchedEntities();  // append selected entities to the master list
      loadTree();             // refresh the entity tree checked status to match
      txtEntityList.Refresh();
    }
    private void refreshFromEntityList()
    {
      _selectedEntities = txtEntityList.Text;
      string[] entities = getEntityList(_selectedEntities);
      if (entities.GetLength(0) != 0)
      {
        // look recursively through the tree list for the node and check it.
        _selectingTree = true;      // disable firing of the check event
        checkUncheckNode(tvwEntityHierarchy.Nodes, entities);
        _selectingTree = false;
      }
      loadEntityList();
      currentEntries.Clear();
      refreshSelected();
      tvwEntityHierarchy.Refresh();
      lstSelected.Refresh();
      Refresh();
    }
    private void checkUncheckNode(TreeNodeCollection nodes, string[] entities)
    {
      foreach (TreeNode node in nodes)
      {
        if (node.Tag != null)
        {
          SelectedEntry entry = (SelectedEntry)node.Tag;
          string id = entry.EntityID.ToLower();
          //if (id != "all")
          //{
            node.Checked = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.inList, entities, id);   // if in the list check it, otherwise don't
            if (node.Checked && okToAdd(id))
              currentEntries.Add(id, entry);
          //}
        }
        if (node.Nodes.Count > 0)
          checkUncheckNode(node.Nodes, entities);
      }
    }
    private string[] getEntityList(string pEntityList)
    {
      if (pEntityList == null)
        return new string[0];
      string[] entities = pEntityList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
      if (entities == null)
        return new string[0];
      for (int i = 0; i < entities.GetLength(0); i++)
        entities[i] = entities[i].Trim().ToLower();
      return entities;
    }
    private void selectOneEntry()
    {
      // remove all but the first one
      for (int i = currentEntries.Count - 1; i > 0; i--)
        currentEntries.Remove(((SelectedEntry)currentEntries.ElementAt(i).Value).EntityID.ToLower());
      loadTree();
      refreshSelected();
      loadEntityList();
    }
    private bool okToAdd(string id)
    {
      bool ok = !currentEntries.ContainsKey(id);  // can't add it if it is already there
      if (!_allowMultiSelect && currentEntries.Count > 0) // also can't add if multiselect is false and there is already one
        ok = false;
      return ok;
    }
    private SelectedEntry getSystemEntity(string entityID)
    {
      Entity e = _ea.Entity(entityID, false);
      return new SelectedEntry(e);
    }
    #endregion

    #region custom events
    public delegate void ExpandEventHandler(object sender, EventArgs e);
    public delegate void CollapseEventHandler(object sender, EventArgs e);
    public event ExpandEventHandler Expand;
    public event CollapseEventHandler Collapse;
    protected virtual void OnExpand(EventArgs e)
    {
      if (Expand != null)
      {
        //Invokes the delegates.
        Expand(this, e);
      }
    }
    protected virtual void OnCollapse(EventArgs e)
    {
      if (Collapse != null)
      {
        //Invokes the delegates.
        Collapse(this, e);
      }
    }
    public event EventHandler<EventArgs> OnChanged;

    #endregion

  }
}
