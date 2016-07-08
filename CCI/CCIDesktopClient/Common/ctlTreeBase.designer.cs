namespace CCI.DesktopClient.Common
{
  partial class ctlTreeBase
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
      this.components = new System.ComponentModel.Container();
      this.rightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.rightClickMenuCopy = new System.Windows.Forms.ToolStripMenuItem();
      this.rightClickMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // rightClickMenu
      // 
      this.rightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rightClickMenuCopy});
      this.rightClickMenu.Name = "rightClickMenu";
      this.rightClickMenu.Size = new System.Drawing.Size(103, 26);
      this.rightClickMenu.Text = "RightClick";
      this.rightClickMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.rightClickMenu_ItemClicked);
      // 
      // rightClickMenuCopy
      // 
      this.rightClickMenuCopy.Name = "rightClickMenuCopy";
      this.rightClickMenuCopy.Size = new System.Drawing.Size(102, 22);
      this.rightClickMenuCopy.Text = "Copy";
      // 
      // ctlTreeBase
      // 
      this.ContextMenuStrip = this.rightClickMenu;
      this.FullRowSelect = true;
      this.HideSelection = false;
      this.LineColor = System.Drawing.Color.Black;
      this.rightClickMenu.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    protected System.Windows.Forms.ContextMenuStrip rightClickMenu;
    private System.Windows.Forms.ToolStripMenuItem rightClickMenuCopy;
  }
}
