﻿using System;
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
using CCI.DesktopClient.Screens;

using TAGBOSS.Common;
using TAGBOSS.Common.Model;
using TAGBOSS.Sys.AttributeEngine2;
using TAGBOSS.Sys.AttributeEngine2.ConvertToEAC;

namespace CCI.DesktopClient.Screens
{
    public partial class frmAdminDeveloper : ScreenBase
    {
        EntityAttributes _ea = new EntityAttributes();
        DataSource _ds = new DataSource();
        public frmAdminDeveloper()
        {
            InitializeComponent();
        }

        private void btnCreateIBPTempTable_Click(object sender, EventArgs e)
        {
            _ds.CreateTempEntityTable("Customer", "Dealer");

        }
    }
}
