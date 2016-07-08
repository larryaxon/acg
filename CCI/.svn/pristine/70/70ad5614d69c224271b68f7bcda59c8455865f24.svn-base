using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CCI.Common;
using CCI.DesktopClient.Common;
using CCI.Sys.Data;

namespace CCI.DesktopClient.Screens
{
  public partial class dlgEntityEdit : ScreenBase
  {
    private DataSource _dSource = null;
    private DataSource _dataSource { get { if (_dSource == null) _dSource = new DataSource(); return _dSource; } }
    public dlgEntityEdit()
    {
      InitializeComponent();
      srchEntityOwner.SearchExec = new SearchDataSourceGroups();
      srchEntity.SearchExec = new SearchDataSourceEntity();
    }

    private void dlgEntityEdit_Load(object sender, EventArgs e)
    {
      cboEntityType.Items.AddRange(_dataSource.getEntityTypes());
      ctlEntityEdit1.SecurityContext = SecurityContext;
    }

    private void cboEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
      ((SearchDataSourceEntity)srchEntity.SearchExec).EntityType = cboEntityType.Text;
      load(srchEntity.Text);
    }
    private void load(string entity)
    {
      if (!string.IsNullOrEmpty(cboEntityType.Text) && !string.IsNullOrEmpty(srchEntityOwner.Text) && !string.IsNullOrEmpty(entity))
      {
        ctlEntityEdit1.EntityType = cboEntityType.Text;
        ctlEntityEdit1.EntityOwner = srchEntityOwner.Text;
        ctlEntityEdit1.Init(entity);
      }
    }

    private void srchEntity_OnSelected(object sender, EventArgs e)
    {
      load(srchEntity.Text);
    }

    private void srchEntityOwner_OnSelected(object sender, EventArgs e)
    {
      ((SearchDataSourceEntity)srchEntity.SearchExec).EntityOwner = srchEntityOwner.Text;
      load(srchEntity.Text);
    }
  }
}
