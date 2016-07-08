using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using CCI.Sys.Data;
using CCI.DesktopClient.Common;

using TAGBOSS.Sys.AttributeEngine2.ConvertToEAC;
using TAGBOSS.Common;
using TAGBOSS.Common.Model;

namespace CCI.DesktopClient.Screens
{
  public partial class frmAttributes : ScreenBase
  {
    bool _rawMode = false;
    bool _editMode = true;
    string _entities = string.Empty;
    string _itemTypes = string.Empty;
    string _items = string.Empty;
    string _entity = string.Empty;
    string _itemType = string.Empty;
    string _item = string.Empty;
    string _attribute = string.Empty;
    object _value = string.Empty;
    string _valueType = "value";
    string _path = string.Empty;
    DateTime _effectiveDate = DateTime.Today;
    AttributeProcessor _processor = new AttributeProcessor();
    EntityAttributes _entityAttributes = new EntityAttributes();
    EntityAttributesCollection _eac = null;
    public bool EditMode { get { return _editMode; } set { _editMode = value; txtValue.EditMode = _editMode; } }
    public bool RawMode { get { return _rawMode; } set { _rawMode = value; txtValue.RawMode = _rawMode; lblRawMode.Visible = _rawMode; } }
    public frmAttributes()
    {
      InitializeComponent();
      txtValue.EditMode = _editMode;
      
    }

    private void btnLoad_Click(object sender, EventArgs e)
    {
      _entities = ctlAttributeEntry1.Entities;
      _itemTypes = ctlAttributeEntry1.ItemTypes;
      _items = ctlAttributeEntry1.Items;
      if (string.IsNullOrEmpty(_entities))
        MessageBox.Show("You must enter an entity");
      else
      {
        if (string.IsNullOrEmpty(_itemTypes))
          MessageBox.Show("You must enter an Item Type");
        else
        {
          if (string.IsNullOrEmpty(_items))
            _items = null;
          _eac = _processor.getAttributes(_entities, _itemTypes, _items, null, _effectiveDate, RawMode, false, 0);
          if (_eac != null)
            ctlEntityAttributeTree1.LoadTree(_eac);
        }

      }
    }

    private void ctlEntityAttributeTree1_AfterSelect(object sender, TreeViewEventArgs e)
    {
      txtValue.EACobject = _eac;
      txtValue.EntityID = _entity = ctlEntityAttributeTree1.EntitySelected;
      txtValue.ItemTypeID = _itemType = ctlEntityAttributeTree1.ItemTypeSelected;
      txtValue.ItemID = _item = ctlEntityAttributeTree1.ItemSelected;
      txtValue.AttributeID = _attribute = ctlEntityAttributeTree1.AttributeSelected;
      txtValue.ValueType = _valueType = ctlEntityAttributeTree1.ValueTypeSelected;
      txtValue.Value = _value = ctlEntityAttributeTree1.ValueSelected;
      txtPath.Text = _path = string.Format("{0}.{1}.{2}.{3}", _entity, _itemType, _item, _attribute);
      txtValue.SetEnabledControls();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      save();
    }

    private void txtValue_AttributeChange(object sender, AttributeChangeEventArgs e)
    {
      _entity = txtValue.EntityID;
      _itemType = txtValue.ItemTypeID;
      _item = txtValue.ItemID;
      _attribute = txtValue.AttributeID;
      _valueType = txtValue.ValueType;
      _value = txtValue.Value;
      _path = getPath();
      txtPath.Text = _path;
      _eac.setValue(_path, _value);
      ctlEntityAttributeTree1.LoadTree(_eac);
    }
    private string getPath()
    {
      return string.Format("{0}.{1}.{2}.{3}", _entity, _itemType, _item, _attribute);
    }
    private void txtValue_AttributeSave(object sender, EventArgs e)
    {
      save();
    }

    private void save()
    {
      _entityAttributes.CurrentUser = SecurityContext.User;
      _entityAttributes.Save(_eac);
    }
  }
}
