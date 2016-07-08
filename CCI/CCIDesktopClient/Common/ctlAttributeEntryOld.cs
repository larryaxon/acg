using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace CCI.DesktopClient.Common
{
  public partial class ctlAttributeEntryOld : UserControl
  {
    private bool rawMode = false;
    public string[] SaveFields = new string[] { "txtEntities", "txtItemTypes", "txtItems", "txtAttributes", "txtAttributeValues" };
    public string Entities { get { return txtEntities.Text; } set { txtEntities.Text = value; } }
    public string ItemTypes { get { return txtItemTypes.Text; } set { txtItemTypes.Text = value; } }
    public string Items { get { return txtItems.Text; } set { txtItems.Text = value; } }
    public string Attributes { get { return txtAttributes.Text; } set { txtAttributes.Text = value; } }
    public string Values { get { return txtAttributeValues.Text; } set { txtAttributeValues.Text = value; } }
    public string ItemCaption { get { return lblItems.Text; } set { lblItems.Text = value; } }
    public bool SingleEntityMode { get { return !txtEntities.AllowMultiSelect; } set { txtEntities.AllowMultiSelect = !value; } }
    public bool ItemTypesEnabled
    {
      get { return txtItemTypes.Enabled; }
      set
      {
        lblItemTypes.Enabled = value;
        txtItemTypes.Enabled = value;
      }
    }
    public bool RawMode
    {
      get { return rawMode; }
      set
      {
        rawMode = value;
        if (rawMode)
        {
          showAttributes(false);
          this.Height = txtItems.Top + txtItems.Height + 4;
        }
        else
        {
          showAttributes(true);
          this.Height = txtAttributeValues.Top + txtAttributeValues.Height + 4;
        }
      }
    }
    public bool EntitiesEnabled { get { return txtEntities.Enabled; } set { txtEntities.Enabled = value; } }

    public ctlAttributeEntryOld()
    {
      InitializeComponent();
    }

    private void showAttributes(bool show)
    {
      txtAttributes.Visible = show;
      lblAttributes.Visible = show;
      txtAttributeValues.Visible = show;
      lblAttributeValues.Visible = show;
    }

    private void txtEntities_Expand(object sender, EventArgs e)
    {
      txtEntities.Height += 20;
      this.Height = Math.Max(this.Height, txtEntities.Top + txtEntities.Height + 4);
    }

    private void txtEntities_Collapse(object sender, EventArgs e)
    {
      int calculatedHeight = txtAttributeValues.Top + txtAttributeValues.Height + 4;
      if (!txtAttributeValues.Visible)
        calculatedHeight = txtItems.Top + txtItems.Height + 4;
      this.Height = Math.Min(this.Height, calculatedHeight);
    }
  }
}
