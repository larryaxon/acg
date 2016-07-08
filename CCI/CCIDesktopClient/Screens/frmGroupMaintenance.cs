﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CCI.DesktopClient.Screens
{
  public partial class frmGroupMaintenance : frmEntityMaintenance
  {
    public frmGroupMaintenance()
    {
      InitializeComponent();
      _showAddress = false;
      _showContacts = false;
      _showGroups = true;
      EntityType = "Group";
      EntityOwner = "CCI";
    }
    public new void Init(string entity)
    {
      base.Init(entity);
    }

    private void frmGroupMaintenance_Load(object sender, EventArgs e)
    {

      Init(Entity);
      loadCombos();
    }

    private void loadCombos()
    {
      string[] entityTypes = _dataSource.getEntityTypes();
      cboMemberType.Items.Clear();
      cboMemberType.Items.AddRange(entityTypes);
    }

    private void cboMemberType_TextChanged(object sender, EventArgs e)
    {

    }
  }
}
