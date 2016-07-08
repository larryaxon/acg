using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CCI.Sys.Data;
using CCI.Common;
using CCI.DesktopClient.Common;
using CCI.DesktopClient.Screens;

namespace CCI.DesktopClient.Screens
{
  public partial class frmUnmatchedNames : ScreenBase
  {
    #region module data
    private string _unmatchedFieldName = "CustomerNameRaw";
    private string _similarFieldName = "CustomerName";
    private string _id = "CustomerID";
    private string _entityType = "Customer";
    private string _unmatchedLabelText = "Unmatched Names";
    private string _similarLabelText = "Similar Names";
    private const string LABELFORMAT = "{0} ({1})";
    private string _searchTokens = string.Empty;
    private CommonData.UnmatchedNameTypes _nameType = CommonData.UnmatchedNameTypes.Customer;
    
    DataSet _dsUnmatchedList = null;
    DataSource _dataSource = new DataSource();
    int _selectedUnmatched = -1;
    int _selectedMatched = -1;
    int _lastUnmatchedSelection = -1;
    private Rectangle _dragBoxFromMouseDown;
    private Point _screenOffset;
    private bool _detailPaneCollapsed { get { return splitMain.Panel2Collapsed; } set { collapseDetailPane(value); } }
    private bool _hasExceptions { get { return btnFlagException.Visible; } set { btnFlagException.Visible = value; } }

    #endregion

    #region public properties
    public string ColumnName { get { return _unmatchedFieldName; } set { _unmatchedFieldName = value; } }
    public string ID { get { return _id; } set { _id = value; } }
    public string EntityType { get { return _entityType; } set { _entityType = value; } }
    public string SimilarFieldName { get { return _similarFieldName; } set { _similarFieldName = _id = value; } }
    public CommonData.UnmatchedNameTypes NameType 
    { 
      get { return _nameType; } 
      set 
      { 
        _nameType = value;
        btnGetCityCareCustomers.Visible = false;
        btnUpdateCityHosted.Visible = false;
        string nameType = _nameType.ToString();
        this.Text = string.Format("Unmatched {0}", nameType);
        lblTitle.Text = nameType;
        switch (_nameType)
        {
          case CommonData.UnmatchedNameTypes.Customer:
          case CommonData.UnmatchedNameTypes.ImportCustomer:
          case CommonData.UnmatchedNameTypes.TrueUp:
            collapseDetailPane(true);
            _unmatchedFieldName = "CustomerNameRaw";
            _similarFieldName = "CustomerName";
            _id = "CustomerID";
            EntityType = "Customer";
            btnGetCityCareCustomers.Visible = true;
            break;
          case CommonData.UnmatchedNameTypes.Inventory:
            collapseDetailPane(true);
            _unmatchedFieldName = "ProductNameRaw";
            _similarFieldName = "Name";
            _id = "ItemID";
            EntityType = "Item";
            break;
          case CommonData.UnmatchedNameTypes.Payor:
            collapseDetailPane(true);
            _id = _unmatchedFieldName = _similarFieldName = "VendorName";
            EntityType = "Payor";
            break;
          case CommonData.UnmatchedNameTypes.CityHostedCustomer:
            btnResearch.Visible = false;
            collapseDetailPane(false);
            _unmatchedFieldName = "CustomerName";
            _similarFieldName = "CustomerName";
            _id = "CustomerID";
            EntityType = "Customer";
            btnGetCityCareCustomers.Visible = true;
            btnUpdateCityHosted.Visible = true;
            break;
          case CommonData.UnmatchedNameTypes.CityHostedUnmatchedBTNs:
            collapseDetailPane(false);
            btnResearch.Visible = false;
            ckIncludeNewCustomers.Visible = true;
            btnAddNew.Visible = false;
            _hasExceptions = true;
            _id = "[Wholesale BTN]";
             _unmatchedFieldName = "[Retail BTN]";
            _similarFieldName = "CustomerName + Customer + BTN";
            break;
          case CommonData.UnmatchedNameTypes.CityHostedUnmatchedBTNsWholesale:
            collapseDetailPane(false);
            ckIncludeNewCustomers.Visible = true;
            btnResearch.Visible = false;
            btnAddNew.Visible = false;
            _hasExceptions = true;
            _id = "[Retail BTN]";
            _unmatchedFieldName = "[Wholesale BTN]";
            _similarFieldName = "CustomerName + Customer + BTN";
            break;
        }
      } 
    }
    #endregion
    public frmUnmatchedNames()
    {
      InitializeComponent();
      _unmatchedLabelText = lblUnmatchedNames.Text;
      _similarLabelText = lblSimilarNames.Text;
    }
    #region form and control events
    private void frmUnmatchedNames_Load(object sender, EventArgs e)
    {
      loadUnmatched();
    }

    #region buttons

    private void btnFindSimilarNames_Click(object sender, EventArgs e)
    {
      loadSimilarNames(false);
    }
    private void btnClear_Click(object sender, EventArgs e)
    {
      clearSimilarNames();
    }
    private void btnLink_Click(object sender, EventArgs e)
    {
      linkMatched();
    }
    private void btnShowAllNames_Click(object sender, EventArgs e)
    {
      loadSimilarNames(true);
    }
    private void btnSearch_Click(object sender, EventArgs e)
    {
      loadSimilarNames(false, txtSearchTokens.Text);
    }
    private void txtSearchTokens_Leave(object sender, EventArgs e)
    {
      loadSimilarNames(false, txtSearchTokens.Text);
    }
    private void btnAddNew_Click(object sender, EventArgs e)
    {
      string name = CommonFunctions.CString(grdUnmatchedNames.Rows[_selectedUnmatched].Cells[_unmatchedFieldName].Value);
      openMaintenance(name);
    }
    private void btnGetCityCareCustomers_Click(object sender, EventArgs e)
    {
      DialogResult ans = MessageBox.Show("Are you sure you want to add all recently added customers in the City Care database to this database?", "Refresh Customers from CityCare", MessageBoxButtons.YesNo);
      if (ans == DialogResult.Yes)
      {
        Cursor.Current = Cursors.WaitCursor;
        _dataSource.RefreshCustomersFromCityCare(SecurityContext.User);
        Cursor.Current = Cursors.Default;
      }
    }
    private void btnUpdateCityHosted_Click(object sender, EventArgs e)
    {
      DialogResult ans = MessageBox.Show("Are you sure you want to update all of the hosted tables with newly linked customers?", "Update Hosted Customers", MessageBoxButtons.YesNo);
      if (ans == DialogResult.Yes)
      {
        Cursor.Current = Cursors.WaitCursor;
        _dataSource.updateMRCUnmatchedCustomers();
        Cursor.Current = Cursors.Default;
      }
    }

    #endregion

    #region Unmatched Grid

    private void grdUnmatchedNames_MouseDown(object sender, MouseEventArgs e)
    {
      DataGridView.HitTestInfo hit = grdUnmatchedNames.HitTest(e.X, e.Y);
      _selectedUnmatched = hit.RowIndex;
      if (_selectedUnmatched >= 0 && _selectedUnmatched < grdUnmatchedNames.Rows.Count)
      {

        // Remember the point where the mouse down occurred. The DragSize indicates
        // the size that the mouse can move before a drag event should be started.                
        Size dragSize = SystemInformation.DragSize;

        // Create a rectangle using the DragSize, with the mouse position being
        // at the center of the rectangle.
        _dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                       e.Y - (dragSize.Height / 2)), dragSize);
      }
      else
        // Reset the rectangle if the mouse is not over an item in the ListBox.
        _dragBoxFromMouseDown = Rectangle.Empty;

    }

    private void grdUnmatchedNames_MouseUp(object sender, MouseEventArgs e)
    {
      // Reset the drag rectangle when the mouse button is raised.
      _dragBoxFromMouseDown = Rectangle.Empty;
      if (_selectedUnmatched >= 0 && _selectedUnmatched < grdUnmatchedNames.Rows.Count)
      {
        if (ckAutoFind.Checked)
          loadSimilarNames(false);
        syncDetail();
      }
    }
    private void syncDetail()
    {
      if (ckAutoResearch.Checked)
      {
        string name = CommonFunctions.CString(grdUnmatchedNames.Rows[_selectedUnmatched].Cells[0].Value);
        if (!string.IsNullOrEmpty(name) && name.Length >= 5)
        {
          name = name.Substring(0, 5);
          if (!_detailPaneCollapsed)
          {
            // load data for detail pane
            switch (NameType)
            {
              case CommonData.UnmatchedNameTypes.CityHostedCustomer:
                ctlCityHostedDetail1.BillDate = _dataSource.getCurrentBillingMonth();
                ctlCityHostedDetail1.BTN = _dataSource.getBTNsfromCustomer(name);
                ctlCityHostedDetail1.ReLoad();
                break;
              case CommonData.UnmatchedNameTypes.CityHostedUnmatchedBTNs:
                MRCMatchingSelectedItem item = new MRCMatchingSelectedItem(name);
                ctlCityHostedDetail1.BillDate = _dataSource.getCurrentBillingMonth();
                ctlCityHostedDetail1.CustomerID = item.ID;
                ctlCityHostedDetail1.ReLoad();
                break;
            }
          }
        }
      }
    }
    private void grdUnmatchedNames_MouseMove(object sender, MouseEventArgs e)
    {
      if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
      {
        // If the mouse moves outside the rectangle, start the drag.
        if (_dragBoxFromMouseDown != Rectangle.Empty &&
            !_dragBoxFromMouseDown.Contains(e.X, e.Y))
        {
          DragDropEffects effects = grdUnmatchedNames.DoDragDrop("Drag", DragDropEffects.Copy);
          //MessageBox.Show(effects.ToString());
        }
      }

    }

    private void grdUnmatchedNames_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      DataGridView grd = sender as DataGridView;
      if (grd != null)
      {

        Form f = grd.FindForm();

        // Cancel the drag if the mouse moves off the form. The screenOffset
        // takes into account any desktop bands that may be at the top or left
        // side of the screen.
        if (((Control.MousePosition.X - _screenOffset.X) < f.DesktopBounds.Left) ||
            ((Control.MousePosition.X - _screenOffset.X) > f.DesktopBounds.Right) ||
            ((Control.MousePosition.Y - _screenOffset.Y) < f.DesktopBounds.Top) ||
            ((Control.MousePosition.Y - _screenOffset.Y) > f.DesktopBounds.Bottom))
        {

          e.Action = DragAction.Cancel;
          //MessageBox.Show("Left the screen");
        }
      }

    }
    #endregion

    #region SimilarNames Grid

    private void frmSimilarNames_DragDrop(object sender, DragEventArgs e)
    {
      //MessageBox.Show("Dropped");
      e.Effect = DragDropEffects.Copy;
      string data = (string)e.Data.GetData(typeof(string));
      DataGridView.HitTestInfo hit = grdSimilarNames.HitTest(e.X, e.Y);
      _selectedMatched = hit.RowIndex;
      linkMatched();
    }

    private void frmSimilarNames_DragEnter(object sender, DragEventArgs e)
    {
      //if (e.Data.GetDataPresent(typeof(string)))
      //  MessageBox.Show(e.Data.GetData(typeof(string)).ToString());
      //else
      //  MessageBox.Show("No data");
    }

    private void frmSimilarNames_MouseUp(object sender, MouseEventArgs e)
    {
      DataGridView.HitTestInfo hit = grdSimilarNames.HitTest(e.X, e.Y);
      _selectedMatched = hit.RowIndex;
      //MessageBox.Show(string.Format("Row selected = {0}", selectedMatched.ToString()));
    }
    #endregion

    #endregion
    #region private methods
    private void loadSimilarNames(bool showAll)
    {
      loadSimilarNames(showAll, null);
    }
    private void loadSimilarNames(bool showAll, string tokenList)
    {
      /*
       * build a set of "LIKE" clauses to look for matches on ANY token in the name
       */
      string name;
      if (tokenList == null)
        name = CommonFunctions.CString(grdUnmatchedNames.Rows[_selectedUnmatched].Cells[0].Value);
      else
        name = tokenList;

      string[] returnList = CommonFunctions.MakeWhereClauseFromName(_similarFieldName, name, showAll);  // default is to return no records
      string sWhereClause = returnList[0];
      txtSearchTokens.Text = returnList[1];
      DataSet dsSimilarNames = _dataSource.getSimilarNamesLike(NameType, sWhereClause);
      CommonFormFunctions.displayDataSetGrid(grdSimilarNames, dsSimilarNames);
      if (grdSimilarNames.Rows.Count == 0)
      {
        _selectedMatched = -1; // can't select a row cause there isn't one
        lblSimilarNames.Text = _similarLabelText;
      }
      else
      {
        _selectedMatched = 0;  // auto select the first row since it is highlighted and the user expects it to be selected
        lblSimilarNames.Text = string.Format(LABELFORMAT, _similarLabelText, grdSimilarNames.Rows.Count.ToString());
      }

    }
    private void clearSimilarNames()
    {
      grdSimilarNames.DataSource = null;
      grdSimilarNames.Refresh();
      _selectedMatched = -1;  // mark NO rows as selected
      lblSimilarNames.Text = _similarLabelText;
    }

    private void loadUnmatched()
    {
      Dictionary<string, string> parameters = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
      if (ckIncludeNewCustomers.Checked)
        parameters.Add("includenewcustomers", "true");
      _dsUnmatchedList = _dataSource.getUnmatchedNames(NameType, parameters);
      CommonFormFunctions.displayDataSetGrid(grdUnmatchedNames, _dsUnmatchedList);
      if (grdUnmatchedNames.Rows.Count > 0)
      {
        lblUnmatchedNames.Text = string.Format(LABELFORMAT, _unmatchedLabelText, grdUnmatchedNames.Rows.Count.ToString());
        _selectedUnmatched = 0;  // auto select the first row since it is highlighted and the user expects it to be selected
        loadSimilarNames(false);
      }
      else
      {
        lblUnmatchedNames.Text = _unmatchedLabelText;
        _selectedMatched = -1;
        clearSimilarNames();
      }
    }
    private void flagException()
    {

      if (_selectedMatched >= 0 && _selectedMatched < grdSimilarNames.Rows.Count
        && _selectedUnmatched >= 0 && _selectedUnmatched < grdUnmatchedNames.Rows.Count)
      {
        frmException dlg = new frmException();
        //dlg.Show(this);
        //dlg.Activate();
        //dlg.Visible = true;
        dlg.SecurityContext = SecurityContext;
        _lastUnmatchedSelection = _selectedUnmatched;
        string unmatchedName = CommonFunctions.CString(grdUnmatchedNames.Rows[_selectedUnmatched].Cells[CommonFunctions.stripDelims(_unmatchedFieldName, CommonData.cLEFTSQUARE)].Value);
        string internalName = CommonFunctions.CString(grdSimilarNames.Rows[_selectedMatched].Cells[CommonFunctions.stripDelims(_id, CommonData.cLEFTSQUARE)].Value);
        if (NameType == CommonData.UnmatchedNameTypes.CityHostedUnmatchedBTNs)
        {
          char[] splitChar = new char[] { ':' };
          string[] retailParts = unmatchedName.Split(splitChar);
          string[] wholesaleParts = internalName.Split(splitChar);
          if (retailParts != null && retailParts.GetLength(0) == 3 && wholesaleParts != null && wholesaleParts.GetLength(0) == 3)
          {
            string customer = retailParts[0].Trim();
            string retailBTN = retailParts[2].Trim();
            string wholesaleBTN = wholesaleParts[2].Trim();
            loadException(dlg, customer, retailBTN, wholesaleBTN);

          }
        }
        else
          if (NameType == CommonData.UnmatchedNameTypes.CityHostedUnmatchedBTNsWholesale)
          {
            char[] splitChar = new char[] { ':' };
            string[] retailParts = internalName.Split(splitChar);
            string[] wholesaleParts = unmatchedName.Split(splitChar);
            if (retailParts != null && retailParts.GetLength(0) == 3 && wholesaleParts != null && wholesaleParts.GetLength(0) == 3)
            {
              string customer = retailParts[0].Trim();
              string retailBTN = retailParts[2].Trim();
              string wholesaleBTN = wholesaleParts[2].Trim();
              loadException(dlg, customer, retailBTN, wholesaleBTN);
            }
          }
        dlg.ShowDialog();

      }
    }
    private void loadException(frmException dlg, string customer, string from, string to)
    {
      dlg.ExceptionType = "SaddleBack";
      dlg.CustomerID = customer;
      dlg.CustomerIDEnabled = false;
      dlg.From = from;
      dlg.FromEnabled = false;
      dlg.To = to;
      dlg.ToEnabled = false;
      dlg.ReasonCode = "UnmatchedBTN";
      dlg.Source = "Unmatched BTNs";
      dlg.Destination = "Saddleback";
    }
    private void linkMatched()
    {

      if (_selectedMatched >= 0 && _selectedMatched < grdSimilarNames.Rows.Count 
        && _selectedUnmatched >= 0 && _selectedUnmatched < grdUnmatchedNames.Rows.Count)
      {
         DialogResult ans = MessageBox.Show("Are you sure you want to link these?", "Confirm Link", MessageBoxButtons.YesNo);
         if (ans == DialogResult.Yes)
         {
           _lastUnmatchedSelection = _selectedUnmatched;
           string unmatchedName = CommonFunctions.CString(grdUnmatchedNames.Rows[_selectedUnmatched].Cells[CommonFunctions.stripDelims(_unmatchedFieldName, CommonData.cLEFTSQUARE)].Value);
           string internalName = CommonFunctions.CString(grdSimilarNames.Rows[_selectedMatched].Cells[CommonFunctions.stripDelims(_id, CommonData.cLEFTSQUARE)].Value);
           if (NameType == CommonData.UnmatchedNameTypes.CityHostedUnmatchedBTNs)
           {
             char[] splitChar = new char[] { ':' };
             string[] retailParts = unmatchedName.Split(splitChar);
             string[] wholesaleParts = internalName.Split(splitChar);
             if (retailParts != null && retailParts.GetLength(0) == 3 && wholesaleParts != null && wholesaleParts.GetLength(0) == 3)
             {
               string customer = retailParts[0].Trim();
               string retailBTN = retailParts[2].Trim();
               string wholesaleBTN = wholesaleParts[2].Trim();
               _dataSource.matchBTN(customer, retailBTN, wholesaleBTN);
             }
           }
           else
             if (NameType == CommonData.UnmatchedNameTypes.CityHostedUnmatchedBTNsWholesale)
             {
               char[] splitChar = new char[] { ':' };
               string[] retailParts = internalName.Split(splitChar);
               string[] wholesaleParts = unmatchedName.Split(splitChar);
               if (retailParts != null && retailParts.GetLength(0) == 3 && wholesaleParts != null && wholesaleParts.GetLength(0) == 3)
               {
                 string customer = retailParts[0].Trim();
                 string retailBTN = retailParts[2].Trim();
                 string wholesaleBTN = wholesaleParts[2].Trim();
                 _dataSource.matchBTN(customer, retailBTN, wholesaleBTN);
               }
             }
             else
             {
               string internalID = _dataSource.getMatchedID(internalName, _nameType);
               if (!string.IsNullOrEmpty(internalID))
               {
                 _dataSource.addExternalID(_entityType, internalID, unmatchedName);
                 loadUnmatched();  // reload the unmatched list... it should get rif of the one we just linked
                 if (grdUnmatchedNames.Rows.Count > 0)
                 {
                   if (grdUnmatchedNames.Rows.Count <= _lastUnmatchedSelection)
                     _lastUnmatchedSelection = grdUnmatchedNames.Rows.Count - 1;
                   //else
                   //  _lastUnmatchedSelection = 0;
                   grdUnmatchedNames.Rows[_lastUnmatchedSelection].Selected = true;
                   DataGridViewCell cell = grdUnmatchedNames.Rows[_lastUnmatchedSelection].Cells[0]; // get the first cell in that row
                   grdUnmatchedNames.FirstDisplayedCell = cell; // and then set the grid to go there
                   _selectedUnmatched = _lastUnmatchedSelection;
                   loadSimilarNames(false);
                 }
               }
             }
         }
      }
    }

    /// <summary>
    /// Open the Entity Maintenance screen (or inherited version) to add a new unmatched name
    /// </summary>
    /// <param name="unmatchedName"></param>
    private void openMaintenance(string unmatchedName)
    {
      MainForm main = CommonFormFunctions.getMainForm(this);
      frmEntityMaintenance frm = null;
      switch (EntityType.ToLower())
      {
        case "customer":
          frm = new frmCustomers();
          break;
        case "payor":
          frm = new frmEntityMaintenance();
          frm.EntityType = EntityType;
          break;
        case "carrier":
          frm = new frmCarrierMaintenance();
          break;
      }
      if (frm != null)
      {
        frm.NewEntityName = unmatchedName;
        frm.SecurityContext = (CCI.Common.SecurityContext)SecurityContext;
        frm.New();
        main.ShowForm(frm, true);
      }
    }
    private void collapseDetailPane(bool collapse)
    {
      splitMain.Panel2Collapsed = collapse;
      if (collapse)
        ctlCityHostedDetail1.Clear();
      else
        ctlCityHostedDetail1.Load(SecurityContext, null);
    }
    #endregion

    private void ckIncludeNewCustomers_CheckedChanged(object sender, EventArgs e)
    {
      loadUnmatched();
    }

    private void btnFlagException_Click(object sender, EventArgs e)
    {
      flagException();
    }

  }
}
