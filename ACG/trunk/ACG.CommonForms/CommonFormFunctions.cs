using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using ACG.Common;

namespace ACG.CommonForms
{
  public static class CommonFormFunctions
  {
    public static void displayDataSetGrid(DataGridView grid, DataSet ds)
    {
      if (ds != null && ds.Tables.Count > 0)
      {
        DataView view = ds.Tables[0].DefaultView;
        grid.Columns.Clear();
        grid.DataSource = view;
        grid.Visible = true;
        grid.Refresh();
      }
    }
    public static DataGridView[] populateDataSetGrids(DataSet ds)
    {
      ArrayList gridList = new ArrayList();
      if (ds == null || ds.Tables.Count == 0)
        return new DataGridView[0];
      foreach (DataTable dt in ds.Tables)
      {
        DataGridView grid = new DataGridView();
        grid.Name = "grd" + dt.TableName;
        DataView view = dt.DefaultView;
        grid.DataSource = view;
        grid.Visible = true;
        gridList.Add(grid);
      }
      return (DataGridView[])gridList.ToArray(typeof(DataGridView));
    }
    public static void convertDataSetToGrid(DataGridView grid, DataSet ds)
    {
      grid.Rows.Clear();
      grid.Columns.Clear();

      if (ds != null && ds.Tables.Count > 0)
      {
        
        DataTable dt = ds.Tables[0];
        int colCount = ds.Tables[0].Columns.Count;
        for (int iCol = 0; iCol < colCount; iCol++)
        {
          DataColumn colFrom = dt.Columns[iCol];
          DataGridViewColumn col;
          string t = colFrom.DataType.ToString().ToLower();
          switch (t)
          {
            case "system.boolean":
              col = new DataGridViewCheckBoxColumn();
              break;
            default:
              col = new DataGridViewTextBoxColumn();
              break;
          }
          col.Name = colFrom.ColumnName;
          grid.Columns.Add(col);
        }

        if (dt.Rows.Count > 0)
        {
          for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
          {
            object[] oColValues = new object[colCount];
            DataRow row = dt.Rows[iRow];
            for (int iCol = 0; iCol < dt.Columns.Count; iCol++)
              oColValues[iCol] = row[iCol];           
            grid.Rows.Add(oColValues);
         }
        }
      }
    }
    /// <summary>
    /// Searches a main MDI container for an existing child that has the same name as child.
    /// </summary>
    /// <param name="main">MDI Container form</param>
    /// <param name="child">Child Form</param>
    /// <returns>If main contains a MDIChild with the same name as child, return that form. Otherwise, return child</returns>
    public static Form FindMatchingChild(Form main, Form child)
    {
      if (main.IsMdiContainer)
      {
        foreach (Form f in main.MdiChildren)
        {
          if (f.Name.Equals(child.Name))
          {
            if (f.Text.Equals(child.Text))
              return f;
          }
        }
        return child;
      }
      else
        return child;
    }
    /// <summary>
    /// displays a message dialog box that you can cut and paste the message from.
    /// </summary>
    /// <param name="message"></param>
    public static void showMessage(string message)
    {
      frmMessage f = new frmMessage(message);
      f.ShowDialog();
    }
    /// <summary>
    /// Shows the message of the innermost Exception
    /// </summary>
    /// <param name="ex"></param>
    public static void showException(Exception ex)
    {
      Exception inner = ex;
      while (inner.InnerException != null)
        inner = inner.InnerException;
      showMessage(inner.Message);
    }
    // search for a match in an array of objects and return the object that matches, using either string case insensitive or internal equals method
    public static object setPickListValue(DataGridViewComboBoxCell.ObjectCollection list, object value)
    {
      if (list == null)
        return null;
      if (list.Count == 0)
        return null;
      if (value == null)
        return null;
      foreach (object o in list)
      {
        if (o != null)
        {
          if (o.GetType() == typeof(string))
          {
            if (((string)o).Equals(value.ToString(), StringComparison.CurrentCultureIgnoreCase))
              return o;
          }
          else
            if ((bool)o.GetType().GetMethod("Equals").Invoke(o, new object[] { value }))
              return o;
        }
      }
      return list[0];
    }
    public static void setComboBoxCell(object ctl, object val)
    {
      if (val != null && ctl != null)
      {
        try
        {
          bool isGridCombo = ctl.GetType() == typeof(DataGridViewComboBoxCell);
          bool isComboBox = ctl.GetType() == typeof(ComboBox);
          if ((isGridCombo && ((DataGridViewComboBoxCell)ctl).Items.Count > 0)
              || (isComboBox && ((ComboBox)ctl).Items.Count > 0))
          {
            bool foundMatch = false;
            string strValue = val.ToString();
            IList items;
            if (isGridCombo)
              items = ((DataGridViewComboBoxCell)ctl).Items;
            else
              items = ((ComboBox)ctl).Items;
            for (int i = 0; i < items.Count; i++)
            {
              object item = items[i];
              if (item != null)
              {
                object[] pList = new object[] { val };
                Type[] typelist = new Type[] { val.GetType() };
                if ((bool)item.GetType().GetMethod("Equals", typelist).Invoke(item, pList))
                {
                  if (isGridCombo)
                  {
                    //((DataGridViewComboBoxCell)ctl).ValueType = item.GetType();
                    pList = new object[0];
                    ((DataGridViewComboBoxCell)ctl).Value = item.GetType().GetProperty(((DataGridViewComboBoxCell)ctl).ValueMember).GetValue(item, pList);
                  }
                  else
                  {
                    ((ComboBox)ctl).Text = (string)item.GetType().GetProperty(((ComboBox)ctl).DisplayMember).GetValue(item, new object[] { });
                    ((ComboBox)ctl).SelectedIndex = i;
                    ((ComboBox)ctl).Refresh();
                  }
                  foundMatch = true;
                  break;
                }
              }
            }
            if (!foundMatch)
            {
              if (isGridCombo)
                ((DataGridViewComboBoxCell)ctl).Value = items[0];
              else
                ((ComboBox)ctl).SelectedItem = items[0];
            }
          }

        }
        catch (Exception ex) 
        {
          ;
        }
      }
    }
    public static string getComboValue(ComboBox ctl)
    {
      int index = ctl.SelectedIndex;
      if (index == -1)  // no item is selected
      {
        string text = ctl.Text;
        for (int i = 0; i < ctl.Items.Count; i++)
        {
          object entry = ctl.Items[i];
          if (text.Equals(CommonFunctions.CString(entry.GetType().GetProperty(ctl.DisplayMember).GetValue(entry, new object[0]))))
            return CommonFunctions.CString(entry.GetType().GetProperty(ctl.ValueMember).GetValue(entry, new object[0]));
        }
        return string.Empty; // if we didn't find a match we return an empty string
      }
      else
      {
        object o = ctl.Items[index];
        return (string)o.GetType().GetProperty(ctl.ValueMember).GetValue(o, new object[] { });
      }
    }
    public static Form getMainForm(Control c)
    {
      Control p = c.Parent;
      while (p != null && !p.GetType().Name.Equals("MainForm"))
        p = p.Parent;
      return (Form)p;
    }
    public static IScreenBase getParentForm(Control c)
    {
      Control p = c.Parent;
      while (p != null && !(p is IScreenBase))
        p = p.Parent;
      return (IScreenBase)p;
    }
    public static void setScreenEnabled(Form f, bool enabled, string[] exceptions)
    {
      Control.ControlCollection controls = (Control.ControlCollection)f.GetType().GetProperty("Controls").GetValue(f, new object[0]);
      foreach (Control c in controls)
      {
        if (exceptions == null || !CommonFunctions.inList(exceptions, c.Name))
          c.Enabled = enabled;
      }
    }
    public static void clearFields(Control container)
    {
      foreach (Control c in container.Controls)
      {
        Type ctype = c.GetType();
        if (ctype == typeof(Label)) // ignore labels
          return;
        try
        {
          if (ctype == typeof(DataGridView)) // these controls "have children" but clearfields doesn't work on it, so we handle it first
            ((DataGridView)c).Rows.Clear();
          else if (ctype == typeof(ctlSearchGrid))
            ((ctlSearchGrid)c).Clear();
          else if (ctype.Name.Equals("ctlContactsLocations"))
            c.GetType().GetMethod("Clear").Invoke(c, new object[0]);
            //((ctlContactsLocations)c).Clear();
          else if (ctype == typeof(ListBox))
            ((ListBox)c).Items.Clear();

          else if (ctype == typeof(TextBox) || c.GetType() == typeof(ComboBox))
            c.Text = string.Empty;
          else if (ctype == typeof(DateTimePicker))
          {
            if (((DateTimePicker)c).ShowCheckBox)
              ((DateTimePicker)c).Checked = false; //no value
          }
          else if (c.HasChildren)
            clearFields(c);
        }
        catch (Exception ex)
        {
          string msg = ex.Message;
        }
      }
    }
    public static void copyCellContents(DataGridView grd)
    {
      Clipboard.SetText(CommonFunctions.CString(grd.CurrentCell.Value));
    }
    public static void exportToExcel(DataSet ds)
    {
      string workbookPath = getexcelfile();
      if (workbookPath != null)
      {
        if (workbookPath.Substring(workbookPath.Length - 4, 4) != ".csv")
        {
          workbookPath = workbookPath + ".csv";
        }
        StreamWriter wr = new StreamWriter(workbookPath);

        int cols;

        //determine the number of columns and write columns to file 
        //cols = ((DataView)ds.DataSource).Count;
        if (ds == null || ds.Tables.Count == 0)
          return;
        DataTable dt = ds.Tables[0];
        cols = dt.Columns.Count;
        for (int i = 0; i < cols - 1; i++)
        {
          wr.Write(dt.Columns[i].ColumnName.ToString().ToUpper() + ",");
        }
        wr.WriteLine();

        //write rows to excel file
        for (int i = 0; i < (dt.Rows.Count - 1); i++)
        {
          for (int j = 0; j < cols; j++)
          {
            if (dt.Rows[i][j] != null)
            {
              if (IsNumeric(dt.Rows[i][j]))
              {
                wr.Write(dt.Rows[i][j] + ",");
              }
              else
              {
                string StrValue = Convert.ToString(dt.Rows[i][j]);
                string WriteLine = '"' + StrValue + '"' + ",";
                wr.Write(WriteLine);
              }
            }
            else
            {
              wr.Write(",");
            }
          }

          wr.WriteLine();
        }
        //close file
        wr.Close();
        Excel.Application excelApp = new Excel.Application();
        excelApp.Visible = true;
        Excel.Workbook newworkbook = excelApp.Workbooks.Open(workbookPath);
      }
    }
    public static void exportToExcel(DataGridView grd)
    {
      exportToExcel(grd, null);
    }
    public static void exportToExcel(DataGridView grd, Dictionary<string, string> hiddenColumns)
    {
      if (hiddenColumns == null)
        hiddenColumns = new Dictionary<string, string>(); // create an empty list
      string workbookPath = getexcelfile();
      List<int> hiddenColumnIndices = new List<int>();
      if (workbookPath != null)
      {
        if (workbookPath.Substring(workbookPath.Length - 4, 4) != ".csv")
        {
          workbookPath = workbookPath + ".csv";
        }
        StreamWriter wr = new StreamWriter(workbookPath);

        int cols;

        //determine the number of columns and write columns to file 
        //cols = ((DataView)grd.DataSource).Count;
        cols = grd.ColumnCount;
        for (int i = 0; i < cols; i++)
        {
          string colName = grd.Columns[i].Name;
          if (hiddenColumns.ContainsKey(colName)) // don't add a column if it is hidden
            hiddenColumnIndices.Add(i);
          else
            wr.Write(colName.ToUpper() + ((i == cols - 1) ? "" : ",").ToString());
        }
        wr.WriteLine();

        //write rows to excel file
        for (int i = 0; i < (grd.Rows.Count); i++)
        {
          for (int j = 0; j < cols; j++)
          {
            if (!hiddenColumnIndices.Contains(j)) // add this column in the row if its index is not in the hidden list
            {
              if (grd.Rows[i].Cells[j].Value != null)
              {
                if (IsNumeric(grd.Rows[i].Cells[j].Value))
                {
                  wr.Write(grd.Rows[i].Cells[j].Value + ",");
                }
                else
                {
                  string StrValue = Convert.ToString(grd.Rows[i].Cells[j].Value);
                  string WriteLine = '"' + StrValue + '"' + ",";
                  wr.Write(WriteLine);
                }
              }
              else
              {
                wr.Write(",");
              }
            }
          }

          wr.WriteLine();
        }
        //close file
        wr.Close();
        Excel.Application excelApp = new Excel.Application();
        excelApp.Visible = true;
        Excel.Workbook newworkbook = excelApp.Workbooks.Open(workbookPath);
      }
    }
    private static string getexcelfile()
    {
      Stream myStream;
      SaveFileDialog saveFileDialog1 = new SaveFileDialog();

      saveFileDialog1.Filter = "ALL files (*.csv)|*.csv";
      saveFileDialog1.FilterIndex = 2;
      saveFileDialog1.RestoreDirectory = true;
      if (saveFileDialog1.ShowDialog() == DialogResult.OK)
      {
        if ((myStream = saveFileDialog1.OpenFile()) != null)
        {
          myStream.Close();
          return saveFileDialog1.FileName;
        }
        return null;
      }
      return null;
    }
    public static System.Boolean IsNumeric (System.Object Expression)
    {
      if(Expression == null || Expression is DateTime)
          return false;

      if(Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
          return true;
   
      try 
      {
          if(Expression is string)
              Double.Parse(Expression as string);
          else
              Double.Parse(Expression.ToString());
              return true;
          } catch {} // just dismiss errors but return false
          return false;
    }
    public static void fillComboBoxList(ComboBox ctl, PickListEntries list)
    {
      ctl.Items.Clear();
      ctl.ValueMember = "IDupper";
      ctl.DisplayMember = "Description";
      foreach (PickListEntry entry in list)
        ctl.Items.Add(entry);
    }
    public static DialogResult InputBox(string title, string promptText, ref string value)
    {
      return InputBox(title, promptText, ref value, false, null, false);
    }
    public static DialogResult InputBox(string title, string promptText, ref string value, bool useSearch, ISearchDataSource dataSource, bool addAll)
    {
      Form form = new Form();
      Label label = new Label();
      Control textBox;
      if (useSearch)
      {
        textBox = new ctlSearch();
        ctlSearch txt = (ctlSearch)textBox;
        txt.SearchExec = dataSource;
        txt.AutoAddNewMode = false;
        txt.AutoSelectWhenMatch = false;
        txt.AutoTabToNextControlOnSelect = false;
        txt.ClearSearchWhenComplete = false;
        txt.MustExistInList = true;
        txt.ShowCustomerNameWhenSet = true;
        txt.ShowTermedCheckBox = false;
        if (addAll)
          txt.AdditionalValues.Add("All", "All");
      }
      else
        textBox = new TextBox();
      Button buttonOk = new Button();
      Button buttonCancel = new Button();

      form.Text = title;
      label.Text = promptText;
      if (useSearch)
        ((ctlSearch)textBox).Text = value;
      else
        textBox.Text = value;

      buttonOk.Text = "OK";
      buttonCancel.Text = "Cancel";
      buttonOk.DialogResult = DialogResult.OK;
      buttonCancel.DialogResult = DialogResult.Cancel;

      label.SetBounds(9, 20, 372, 13);
      textBox.SetBounds(12, 36, 372, 20);
      buttonOk.SetBounds(228, 72, 75, 23);
      buttonCancel.SetBounds(309, 72, 75, 23);

      label.AutoSize = true;
      textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
      buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

      form.ClientSize = new Size(396, 200);
      form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
      form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
      form.FormBorderStyle = FormBorderStyle.FixedDialog;
      form.StartPosition = FormStartPosition.CenterScreen;
      form.MinimizeBox = false;
      form.MaximizeBox = false;
      form.AcceptButton = buttonOk;
      form.CancelButton = buttonCancel;

      DialogResult dialogResult = form.ShowDialog();
      if (useSearch)
        value = ((ctlSearch)textBox).Text;
      else
        value = textBox.Text;
      return dialogResult;
    }
}

  }

