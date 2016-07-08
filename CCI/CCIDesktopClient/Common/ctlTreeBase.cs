using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CCI.Common;

namespace CCI.DesktopClient.Common
{
  public partial class ctlTreeBase : TreeView
  {
    protected List<string> expandedNodes = new List<string>();
    protected string currentNodePath = null;
    protected string[] nodeDelimiters = new string[] { "." };
    protected bool _expandingTree = false;
    private bool _rightClickMenuVisible = true;
    protected TreeNode _nodeRightClicked = null;
    public bool RightClickMenuIsVisible 
    { 
      get { return _rightClickMenuVisible; } 
      set 
      {
        _rightClickMenuVisible = value;
        if (_rightClickMenuVisible)
          this.ContextMenuStrip = rightClickMenu;
        else
          this.ContextMenuStrip = null; 
      } 
    }

    public ctlTreeBase()
    {
      InitializeComponent();
      this.ContextMenuStrip = null; // this is not visible until a context makes it where it should be
      //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
    }
    protected string getNodeName(string selectedNodeText)
    {
      int iBeginName = selectedNodeText.IndexOf("= '") + 3;
      int iNameLength = selectedNodeText.IndexOf("' ") - iBeginName;

      if (iBeginName > -1 && iNameLength > 0)
        return selectedNodeText.Substring(iBeginName, iNameLength);
      else
        return null;
    }
    protected string nodePath(TreeNode node)
    {
      string path = node.Name;
      TreeNode parent = node.Parent;
      while (parent != null)
      {
        path = string.Format("{0}.{1}", parent.Name, path);
        parent = parent.Parent;
      }
      return path;
    }
    protected TreeNode getNodeFromPath(string nodePath)
    {
      return getNodeFromPath(nodePath, true);
    }
    protected TreeNode getNodeFromPath(string nodePath, bool forceCreateNode)
    {
      if (this.Nodes.Count == 0)
        return null;
      if (nodePath == null || nodePath == string.Empty)
        return null;
      string[] pathParts = CommonFunctions.parseString(nodePath, nodeDelimiters);
      TreeNode node = this.Nodes[0]; // hard code the first node to root
      for (int i = 1; i < pathParts.GetLength(0); i++)  // and start at the second
      {
        string key = pathParts[i];
        string id = key.ToLower();
        if (node.Nodes.ContainsKey(id))
          node = node.Nodes[id];
        else
        {
          if (forceCreateNode)  // do we force creation of a node if it does not exist?
          {
            node.Nodes.Add(id, key);  // yes, then create it
            node = node.Nodes[id];
          }
          else
            return null;              // no, just return a null node
        }
      }
      return node;
    }
    public override void Refresh()
    {
      SuspendLayout();
      expandPreviouslyExpandedNodes();
      base.Refresh();
      ResumeLayout();
      PerformLayout();
    }
    protected void expandPreviouslyExpandedNodes()
    {
      for (int i = expandedNodes.Count - 1; i >= 0; i--)
      {
        string nodePath = expandedNodes[i];
        TreeNode node = getNodeFromPath(nodePath, false);
        if (node != null)  // if the node exists in the new tree
          node.Expand();                // then expand it
        else
          expandedNodes.Remove(nodePath); // otherwise, remove it from the list of expanded nodes
      }
      TreeNode currentNode = getNodeFromPath(currentNodePath, false);
      if (currentNode != null)
      {
        _expandingTree = true;        // disable onAfterSelect cause otherwise we have a circular execution path
        this.SelectedNode = currentNode;
        _expandingTree = false;
      }
    }
    #region events
    protected override void OnAfterSelect(TreeViewEventArgs e)
    {
      if (!_expandingTree)    // our refresh of the tree selects the current node, but we don't want to fire this event in this case
      {
        currentNodePath = nodePath(e.Node); // and remember where we were so we can refresh
        base.OnAfterSelect(e);
      }
    }
    protected override void OnAfterExpand(TreeViewEventArgs e)
    {
      string node = nodePath(e.Node);
      if (!expandedNodes.Contains(node))
        expandedNodes.Add(node);
      base.OnAfterExpand(e);
    }
    protected override void OnAfterCollapse(TreeViewEventArgs e)
    {
      string node = nodePath(e.Node);
      if (expandedNodes.Contains(node))
        expandedNodes.Remove(node);
      base.OnAfterCollapse(e);
    }
    protected void rightClickMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
      if (e.ClickedItem.Text.Equals("Copy", StringComparison.CurrentCultureIgnoreCase))
      {
        _nodeRightClicked = this.SelectedNode;
        OnRightClickCopy(e);
      }
    }
    #endregion

    #region custom events
    public delegate void RightClickCopyEventHandler(object sender, ToolStripItemClickedEventArgs e);
    public event RightClickCopyEventHandler RightClickCopy;
    protected virtual void OnRightClickCopy(ToolStripItemClickedEventArgs e)
    {
      if (RightClickCopy != null)
      {
        RightClickCopy(this, e);
      }
    }
    #endregion


  }
}
