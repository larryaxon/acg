namespace CCI.DesktopClient.Common
{
  partial class ctlContactsLocations
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.tabMain = new System.Windows.Forms.TabControl();
      this.tabContacts = new System.Windows.Forms.TabPage();
      this.tabLocations = new System.Windows.Forms.TabPage();
      this.ctlContacts1 = new CCI.DesktopClient.Common.ctlContacts();
      this.splitLocations = new System.Windows.Forms.SplitContainer();
      this.ctlLocations1 = new CCI.DesktopClient.Common.ctlContacts();
      this.ctlLocationContacts = new CCI.DesktopClient.Common.ctlContacts();
      this.tabMain.SuspendLayout();
      this.tabContacts.SuspendLayout();
      this.tabLocations.SuspendLayout();
      this.splitLocations.Panel1.SuspendLayout();
      this.splitLocations.Panel2.SuspendLayout();
      this.splitLocations.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabMain
      // 
      this.tabMain.Controls.Add(this.tabContacts);
      this.tabMain.Controls.Add(this.tabLocations);
      this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabMain.Location = new System.Drawing.Point(0, 0);
      this.tabMain.Name = "tabMain";
      this.tabMain.SelectedIndex = 0;
      this.tabMain.Size = new System.Drawing.Size(960, 509);
      this.tabMain.TabIndex = 0;
      // 
      // tabContacts
      // 
      this.tabContacts.Controls.Add(this.ctlContacts1);
      this.tabContacts.Location = new System.Drawing.Point(4, 22);
      this.tabContacts.Name = "tabContacts";
      this.tabContacts.Padding = new System.Windows.Forms.Padding(3);
      this.tabContacts.Size = new System.Drawing.Size(952, 483);
      this.tabContacts.TabIndex = 0;
      this.tabContacts.Text = "Contacts";
      this.tabContacts.UseVisualStyleBackColor = true;
      // 
      // tabLocations
      // 
      this.tabLocations.Controls.Add(this.splitLocations);
      this.tabLocations.Location = new System.Drawing.Point(4, 22);
      this.tabLocations.Name = "tabLocations";
      this.tabLocations.Padding = new System.Windows.Forms.Padding(3);
      this.tabLocations.Size = new System.Drawing.Size(952, 483);
      this.tabLocations.TabIndex = 1;
      this.tabLocations.Text = "Locations";
      this.tabLocations.UseVisualStyleBackColor = true;
      // 
      // ctlContacts1
      // 
      this.ctlContacts1.ColumnNames = new string[] {
        "ContactType",
        "FirstName",
        "LegalName",
        "Phone",
        "CellPhone",
        "EmailAddress",
        "Address1",
        "Address2",
        "City",
        "State",
        "Zip",
        "Entity",
        "EntityOwner",
        "EntityType"};
      this.ctlContacts1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ctlContacts1.EntityOwner = null;
      this.ctlContacts1.EntityOwnerType = null;
      this.ctlContacts1.EntityType = "Contact";
      this.ctlContacts1.Location = new System.Drawing.Point(3, 3);
      this.ctlContacts1.Name = "ctlContacts1";
      this.ctlContacts1.SecurityContext = null;
      this.ctlContacts1.Size = new System.Drawing.Size(946, 477);
      this.ctlContacts1.StateList = null;
      this.ctlContacts1.TabIndex = 0;
      // 
      // splitLocations
      // 
      this.splitLocations.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitLocations.Location = new System.Drawing.Point(3, 3);
      this.splitLocations.Name = "splitLocations";
      this.splitLocations.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitLocations.Panel1
      // 
      this.splitLocations.Panel1.Controls.Add(this.ctlLocations1);
      // 
      // splitLocations.Panel2
      // 
      this.splitLocations.Panel2.Controls.Add(this.ctlLocationContacts);
      this.splitLocations.Size = new System.Drawing.Size(946, 477);
      this.splitLocations.SplitterDistance = 260;
      this.splitLocations.TabIndex = 1;
      // 
      // ctlLocations1
      // 
      this.ctlLocations1.ColumnNames = new string[] {
        "ContactType",
        "FirstName",
        "LegalName",
        "Phone",
        "CellPhone",
        "EmailAddress",
        "Address1",
        "Address2",
        "City",
        "State",
        "Zip",
        "Entity",
        "EntityOwner",
        "EntityType"};
      this.ctlLocations1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ctlLocations1.EntityOwner = null;
      this.ctlLocations1.EntityOwnerType = null;
      this.ctlLocations1.EntityType = "Location";
      this.ctlLocations1.Location = new System.Drawing.Point(0, 0);
      this.ctlLocations1.Name = "ctlLocations1";
      this.ctlLocations1.SecurityContext = null;
      this.ctlLocations1.Size = new System.Drawing.Size(946, 260);
      this.ctlLocations1.StateList = null;
      this.ctlLocations1.TabIndex = 0;
      this.ctlLocations1.RowSelected += new CCI.DesktopClient.Common.ctlEntityGridBase.RowSelectedHandler(this.ctlLocations1_RowSelected);
      // 
      // ctlLocationContacts
      // 
      this.ctlLocationContacts.ColumnNames = new string[] {
        "ContactType",
        "FirstName",
        "LegalName",
        "Phone",
        "CellPhone",
        "EmailAddress",
        "Address1",
        "Address2",
        "City",
        "State",
        "Zip",
        "Entity",
        "EntityOwner",
        "EntityType"};
      this.ctlLocationContacts.Dock = System.Windows.Forms.DockStyle.Fill;
      this.ctlLocationContacts.EntityOwner = null;
      this.ctlLocationContacts.EntityOwnerType = null;
      this.ctlLocationContacts.EntityType = "Contact";
      this.ctlLocationContacts.Location = new System.Drawing.Point(0, 0);
      this.ctlLocationContacts.Name = "ctlLocationContacts";
      this.ctlLocationContacts.SecurityContext = null;
      this.ctlLocationContacts.Size = new System.Drawing.Size(946, 213);
      this.ctlLocationContacts.StateList = null;
      this.ctlLocationContacts.TabIndex = 0;
      // 
      // ctlContactsLocations
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabMain);
      this.Name = "ctlContactsLocations";
      this.Size = new System.Drawing.Size(960, 509);
      this.tabMain.ResumeLayout(false);
      this.tabContacts.ResumeLayout(false);
      this.tabLocations.ResumeLayout(false);
      this.splitLocations.Panel1.ResumeLayout(false);
      this.splitLocations.Panel2.ResumeLayout(false);
      this.splitLocations.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabMain;
    private System.Windows.Forms.TabPage tabContacts;
    private System.Windows.Forms.TabPage tabLocations;
    private ctlContacts ctlContacts1;
    private ctlContacts ctlLocations1;
    private System.Windows.Forms.SplitContainer splitLocations;
    private ctlContacts ctlLocationContacts;
  }
}
