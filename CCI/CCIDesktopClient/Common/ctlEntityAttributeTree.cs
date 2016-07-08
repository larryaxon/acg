using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using CCI.DesktopClient.Common;

using TAGBOSS.Common;
using TAGBOSS.Common.Model;

namespace CCI.DesktopClient.Common
{
  public partial class ctlEntityAttributeTree : ctlTreeBase
  {
    #region module data

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    private EntityAttributesCollection _eac = new EntityAttributesCollection();
    private string _entitySelected = null;
    private string _itemTypeSelected = null;
    private string _itemSelected = null;
    private string _attributeSelected = null;
    private string _itemHistorySelected = null;
    private string _valueHistorySelected = null;
    private string _valueTypeSelected = null;
    private object _valueSelected = null;
    private object _nodeSelected = null;
    private string _treeNodeLevel = string.Empty;
    private bool _displayRightClickMenu = true;

    #endregion

    #region public properties

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public EntityAttributesCollection EAC { get { return _eac; } }
    public string EntitySelected { get { return _entitySelected; } }
    public string ItemTypeSelected { get { return _itemTypeSelected; } }
    public string ItemSelected { get { return _itemSelected; } }
    public string AttributeSelected { get { return _attributeSelected; } }
    public string ItemHistorySelected { get { return _itemHistorySelected; } }
    public string ValueHistorySelected { get { return _valueHistorySelected; } }
    public string ValueTypeSelected { get { return _valueTypeSelected; } }
    public object ValueSelected { get { return _valueSelected; } }
    public object NodeSelected { get { return _nodeSelected; } }
    public bool DisplayRightClickMenu { get { return _displayRightClickMenu; } set { _displayRightClickMenu = value; } }
    #endregion

    #region constructors

    public ctlEntityAttributeTree()
    {
      InitializeComponent();
    }

    #endregion

    #region public methods

    public void LoadTree()
    {
      if (_eac != null)
        LoadTree(_eac);
    }
    public void LoadTree(EntityAttributesCollection d)
    {
      ctlEntityAttributeTree tv = this;
      tv.SuspendLayout();
      _eac = d;
      tv.Nodes.Clear();
      TreeNode root = tv.Nodes.Add("root", "/");
      loadTree(tv, d.Entities, root, false);
      expandPreviouslyExpandedNodes();
      tv.ResumeLayout();
    }
    public void Clear()
    {
      LoadTree(new EntityAttributesCollection());
    }
    #endregion

    #region private methods

    private void loadTree(ctlEntityAttributeTree tv, EntitiesCollection en_c, TreeNode tvw, bool clearTreeNodes)
    {
      TreeNode root = null;
      if (clearTreeNodes)
      {
        tv.Nodes.Clear();
        root = tv.Nodes.Add("root", "/");
      }
      else
        root = tvw;

      if (root == null)
        return;

      for (int k1 = 0; k1 < en_c.Count; k1++)
      {
        TreeNode tvw_en = root.Nodes.Add(en_c[k1].ID, "entity = '" + en_c[k1].OriginalID + "' ");
        en_c[k1].ItemTypes.Sort();
        en_c[k1].Fields.Sort();
        loadTree(tv, en_c[k1].ItemTypes, tvw_en, false);
      }

    }
    private void loadTree(ctlEntityAttributeTree tv, ItemTypeCollection it_c, TreeNode tvw_en, bool clearTreeNodes)
    {
      TreeNode root = null;
      if (clearTreeNodes)
      {
        tv.Nodes.Clear();
        root = tv.Nodes.Add("root", "/");
      }
      else
        root = tvw_en;

      if (root == null)
        return;

      for (int k2 = 0; k2 < it_c.Count; k2++)
      {
        TreeNode tvw_it = root.Nodes.Add(it_c[k2].ID, "itemType = '" + it_c[k2].OriginalID + "' ");
        it_c[k2].Items.Sort();
        loadTree(tv, it_c[k2], tvw_it, false);
      }
    }
    private void loadTree(ctlEntityAttributeTree tv, ItemType it, TreeNode tvw_it, bool clearTreeNodes)
    {
      TreeNode root = null;
      if (clearTreeNodes)
      {
        tv.Nodes.Clear();
        root = tv.Nodes.Add("root", "/");
      }
      else
        root = tvw_it;

      if (root == null)
        return;

      for (int k3 = 0; k3 < it.Items.Count; k3++)
      {
        Item i = it[k3];
        i.Attributes.Sort();
        string ii = "";
        if (i.IsInherited)
          ii = "(I)";
        if (i.Dirty)
          ii += "(D)";
        if (i.Source != null && i.Source != string.Empty)
          ii += "(" + i.Source + ")";
        string key = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getFormattedKey, it.Items.getKey(k3), i.OriginalID); // in case this is a merged data pool, we need the indexed version
        TreeNode tvw_i = root.Nodes.Add(key.ToLower(), "item = '" + key + "' " + ii);
        if (i.ItemHistoryRecords != null && i.ItemHistoryRecords.Count > 0)
        {
          TreeNode tvw_iH = tvw_i.Nodes.Add("item Dates");
          i.ItemHistoryRecords.Sort();
          foreach (ItemHistory iH in i.ItemHistoryRecords)
          {
            TreeNode tvw_iHR = tvw_iH.Nodes.Add(iH.ID, "iH StartDate = " + iH.StartDate + " EndDate = " + iH.EndDate);
          }
        }

        foreach (TAGAttribute attr in i.Attributes)
        {
          if (!attr.Deleted)
          {
            ii = "";
            if (attr.IsInherited)
              ii = "(I)";
            if (attr.Dirty)
              ii += "(D)";
            //if (attr.IsExpressionValue)
            //  ii += "(C)";
            if (attr.IsFunctionValue)
              ii += "(F)";
            if (attr.IsIncluded)
              ii += "(L)";
            if (attr.IsRefValue)
              ii += "(R)";
            if (attr.Virtual)
              ii += "(V)";
            if (attr.IsGenerated)
              ii += "(G)";
            if (attr.IsHistory)
              ii += "(H)";


            if (attr.HasHistory)
            {
              TreeNode tvw_a = tvw_i.Nodes.Add(attr.ID, "attr name = '" + attr.OriginalID + "' " + attr.ValueType + " = '" + attr.Value + "' " + ii);

              if (attr.ReferenceValueSource != "")
                tvw_a.Nodes.Add("[" + attr.ReferenceValueSource + "]");

              foreach (ValueHistory vhist in attr.Values)
                tvw_a.Nodes.Add(vhist.ID, "History ID = '" + vhist.ID + "' " + vhist.ValueType + " = '" + vhist.Value + "' Start Date = '" + vhist.ID + "' enddate = '" + Convert.ToString(vhist.EndDate) + "' ");
            }
            else
              tvw_i.Nodes.Add(attr.ID, "attr name = '" + attr.OriginalID + "' value = '" + attr.Value + "' " + ii);
          }
        }
      }
    }
    private void cleanSelectedFields()
    {
      _entitySelected = null;
      _itemTypeSelected = null;
      _itemSelected = null;
      _itemHistorySelected = null;
      _attributeSelected = null;
      _valueHistorySelected = null;
      _valueSelected = null;
      _valueTypeSelected = null;
      _nodeSelected = null;
    }
    /// <summary>
    /// gets the value of NodeSelected based on what of the other selected values are set
    /// </summary>
    /// <returns></returns>
    private object getNode()
    {
      object node = null;
      if (_entitySelected != null)
      {
        if (_itemTypeSelected == null)
          node = _eac.getValue(_entitySelected);
        else
        {
          if (_itemSelected == null)
            node = _eac.getValue(string.Format("{0}.ItemType.{1}", _entitySelected, _itemTypeSelected));
          else
          {
            Item item = (Item)_eac.getValue(string.Format("{0}.{1}.{2}", _entitySelected, _itemTypeSelected, _itemSelected));
            if (_itemHistorySelected == null)
            {
              if (_attributeSelected == null)
                node = item;
              else
              {
                if (item != null)
                {
                  if (item.Attributes.Contains(_attributeSelected))
                  {
                    TAGAttribute a = item.Attributes[_attributeSelected];
                    if (_valueHistorySelected == null)
                      node = a;
                    else
                      if (a.Values.Contains(_valueHistorySelected))
                        node = a.Values[_valueHistorySelected];
                  }
                }
              }
            }
            else
            {
              if (item.ItemHistoryRecords.Contains(_itemHistorySelected))
                node = item.ItemHistoryRecords[_itemHistorySelected];
            }
          }
        }
      }
      return node;
    }
    private void setSelectedFields()
    {
      bool setAttributeValue = false;  //This local flag is set to true only if we click on a attribute or attribute history node

      cleanSelectedFields();

      if (SelectedNode == null)
        return;

      string selectedNodeText = SelectedNode.Text;
      string  selectedName = null;
      if (!selectedNodeText.StartsWith("item Dates") && !selectedNodeText.StartsWith("iH"))
      {
        selectedName = getNodeName(selectedNodeText);
        if (selectedName == null)
          return;
      }
      if (selectedNodeText.StartsWith("entity"))
      {
        #region Entity Selected
        //-----------------------------------
        //Entity node was clicked
        //-----------------------------------        
        _treeNodeLevel = "entity";
        _entitySelected = selectedName;
        //_nodeSelected = (Entity)_eac.getValue(_entitySelected);
        //_entityEnabled = false;
        #endregion
      }
      else 
      {
        if (selectedNodeText.StartsWith("itemType"))
        {
          #region ItemType Selected
          //-----------------------------------
          //ItemType node was clicked
          //-----------------------------------          
          _treeNodeLevel = "itemType";

          _itemTypeSelected = selectedName;

          if (SelectedNode.Parent != null)
          {
              _entitySelected = getNodeName(SelectedNode.Parent.Text);
          }
          //_entityEnabled = false;
          //_itemTypeEnabled = false;
          //_nodeSelected = (ItemType)_eac.getValue(string.Format("{0}.ItemType.{1}", _entitySelected, _itemTypeSelected));
          #endregion
        }
        else 
        {
          if (selectedNodeText.StartsWith("item Dates") || selectedNodeText.StartsWith("iH"))  // same affect really as selecting the item
          {
            #region Item History Selected
            _treeNodeLevel = "itemhistory";
            setAttributeValue = false;
            TreeNode itemNode;
            if (selectedNodeText.StartsWith("iH"))
              itemNode = SelectedNode.Parent.Parent;
            else
              itemNode = SelectedNode.Parent;
            _itemSelected = getNodeName(itemNode.Text);
            if (itemNode.Parent != null)
            {
              _itemTypeSelected = getNodeName(itemNode.Parent.Text);
              if (itemNode.Parent.Parent != null)
                _entitySelected = getNodeName(itemNode.Parent.Parent.Text);
            }
            #endregion
          }
          else
          {
            if (selectedNodeText.StartsWith("item"))
            {
              #region Item Selected
              //-----------------------------------
              //Item node was clicked
              //-----------------------------------
              _treeNodeLevel = "item";
              _itemSelected = selectedName;
              if (SelectedNode.Parent != null)
              {
                _itemTypeSelected = getNodeName(SelectedNode.Parent.Text);
                if (SelectedNode.Parent.Parent != null)
                  _entitySelected = getNodeName(SelectedNode.Parent.Parent.Text);
              }
              //_nodeSelected = (Item)_eac.getValue(this._entitySelected, this._itemTypeSelected, this._itemSelected);   //Current selected Item

              //_entityEnabled = false;
              //_itemTypeEnabled = false;
              //_itemEnabled = false;
              #endregion
            }
            else
            {
              if (selectedNodeText.StartsWith("attr"))
              {
                #region Attribute Selected
                //-----------------------------------
                //Attribute node was clicked
                //-----------------------------------
                _treeNodeLevel = "attr";

                setAttributeValue = true;
                _attributeSelected = getNodeName(selectedNodeText);

                if (SelectedNode.Parent != null)
                {
                  _itemSelected = getNodeName(SelectedNode.Parent.Text);

                  if (SelectedNode.Parent.Parent != null)
                  {
                    _itemTypeSelected = getNodeName(SelectedNode.Parent.Parent.Text);
                    if (SelectedNode.Parent.Parent.Parent != null)
                      _entitySelected = getNodeName(SelectedNode.Parent.Parent.Parent.Text);
                  }
                }
                //We set the currently selected Attribute!!
                //_nodeSelected = ((Item)_eac.getValue(this._entitySelected, this._itemTypeSelected, this._itemSelected)).Attributes[_attributeSelected];    //Current Selected Attribute

                //_entityEnabled = false;
                //_itemTypeEnabled = false;
                //_itemEnabled = false;
                //_attributeEnabled = false;
                #endregion
              }
              else
              {
                if (selectedNodeText.StartsWith("History"))
                {
                  #region Value History Selected
                  //-----------------------------------
                  //Attribute history node was clicked
                  //-----------------------------------
                  _treeNodeLevel = "history";

                  setAttributeValue = true;

                  _valueHistorySelected = getNodeName(selectedNodeText);
                  if (SelectedNode.Parent != null)
                  {
                    _attributeSelected = getNodeName(SelectedNode.Parent.Text);
                    if (SelectedNode.Parent.Parent != null)
                    {
                      _itemSelected = getNodeName(SelectedNode.Parent.Parent.Text);
                      if (SelectedNode.Parent.Parent.Parent != null)
                      {
                        _itemTypeSelected = getNodeName(SelectedNode.Parent.Parent.Parent.Text);
                        if (SelectedNode.Parent.Parent.Parent.Parent != null)
                          _entitySelected = getNodeName(SelectedNode.Parent.Parent.Parent.Parent.Text);
                      }
                    }
                  }
                  //_nodeSelected = ((Item)_eac.getValue(this._entitySelected, this._itemTypeSelected, this._itemSelected)).Attributes[_attributeSelected].Values[_valueHistorySelected];   

                  //_entityEnabled = false;
                  //_itemTypeEnabled = false;
                  //_itemEnabled = false;
                  //_attributeEnabled = false;
                  #endregion
                }
              }
            }
          }
        }
      }
      _nodeSelected = getNode();
      //--------------------------------------------------------------------------------------------------------------------------------------------
      //Particular node click processing finished, general node click tasks now being processed
      //--------------------------------------------------------------------------------------------------------------------------------------------
      if (setAttributeValue)   //This flag is set to true only is we have clicked on a Attribute or Attribute History node!!
      {
        if (_nodeSelected != null)
        {
          if (_valueHistorySelected == null)
          {
            TAGAttribute a = (TAGAttribute)_nodeSelected;
            ValueHistory vh = null;
            if (a.Values.Count == 1)
              vh = a.Values[0];
            else
            {
              foreach (ValueHistory vhTest in a.Values)
              {
                if (vhTest.StartDate <= _eac.EffectiveDate && _eac.EffectiveDate <= vhTest.EndDate)
                {
                  vh = vhTest;
                  break;
                }
              }
              vh = new ValueHistory();
              vh.StartDate = a.StartDate;
              vh.EndDate = a.EndDate;
              vh.ValueType = a.ValueType;
              vh.Value = a.Value;
            }
             _valueTypeSelected = vh.ValueType;
            _valueSelected = vh.Value;
          }
          else
          {
            if (((ValueHistory)_nodeSelected).ValueType != null)
              _valueTypeSelected = ((ValueHistory)_nodeSelected).ValueType;
            _valueSelected = ((ValueHistory)_nodeSelected).Value;
          }
        }
      }
    }
    private bool nodeIsItem(TreeNode node)
    {
      if (node == null)
        return false;
      if (node.Text == null)
        return false;
      return node.Text.StartsWith("item =");
    }
    #endregion

    #region events
    protected override void OnAfterSelect(TreeViewEventArgs e)
    {
      if (!_expandingTree)    // our refresh of the tree selects the current node, but we don't want to fire this event in this case
      {
        setSelectedFields();    // set the field values for this node
        RightClickMenuIsVisible = _displayRightClickMenu && nodeIsItem(this.SelectedNode);
        base.OnAfterSelect(e);
      }
    }  
    private void ctlEntityAttributeTree_RightClickCopy(object sender, ToolStripItemClickedEventArgs e)
    {
      if (nodeIsItem(_nodeRightClicked))  // was this an item?
      {
        string itemID = getNodeName(_nodeRightClicked.Text);
        string entityID = string.Empty;
        string itemTypeID = string.Empty;
        if (_nodeRightClicked.Parent != null)
        {
          itemTypeID = getNodeName(_nodeRightClicked.Parent.Text);
          if (_nodeRightClicked.Parent.Parent != null)
            entityID = getNodeName(_nodeRightClicked.Parent.Parent.Text);
        }
        
        Item itemSource = (Item)_eac.getValue(entityID, itemTypeID, itemID);
        if (itemSource != null)
        {
          Item itemTarget = (Item)itemSource.Clone();
          itemTarget.ID = itemTarget.OriginalID + "_1";
          _eac.Entities[entityID].ItemTypes[itemTypeID].Items.Add(itemTarget);
          _nodeRightClicked = null; //reset so we don't do it twice
          LoadTree();
        }
      }
      else
        MessageBox.Show("You cannot copy anything but an item at this time");
    }
    #endregion
  }
}
