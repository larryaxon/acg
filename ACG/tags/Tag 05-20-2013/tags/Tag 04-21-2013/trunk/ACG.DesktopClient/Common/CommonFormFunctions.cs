﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

using ACG.App.Common;
using ACG.DesktopClient;
using ACG.DesktopClient.Screens;

namespace ACG.DesktopClient.Common
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
    public static MainForm getMainForm(Control c)
    {
      Control p = c.Parent;
      while (p != null && p.GetType() != typeof(MainForm))
        p = p.Parent;
      return (MainForm)p;
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
    public static void copyCellContents(DataGridView grd)
    {
      Clipboard.SetText(CommonFunctions.CString(grd.CurrentCell.Value));
    }
    public static void copyTableContents(DataGridView grd)
    {
      grd.SelectAll();
      DataObject table = grd.GetClipboardContent();
      Clipboard.SetDataObject(table, true);
    }
    public static void exportToExcel(DataGridView grd)
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
        //cols = ((DataView)grd.DataSource).Count;
        cols = grd.ColumnCount;
        for (int i = 0; i < cols - 1; i++)
        {
          wr.Write(grd.Columns[i].Name.ToString().ToUpper() + ",");
        }
        wr.WriteLine();

        //write rows to excel file
        for (int i = 0; i < (grd.Rows.Count - 1); i++)
        {
          for (int j = 0; j < cols; j++)
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
    public static void fillComboBoxList(ComboBox ctl, ACG.App.Common.PickListEntries list)
    {
      ctl.Items.Clear();
      ctl.ValueMember = "IDupper";
      ctl.DisplayMember = "Description";
      foreach (PickListEntry entry in list)
        ctl.Items.Add(entry);
    }
    public static void populatePickList(ComboBox ctl, ACGTable list, string val)
    {
      populatePickList(ctl, list, val, false);
    }
    public static void populatePickList(ComboBox ctl, ACGTable list, string val, bool selectFirstIfNoValue)
    {
      ctl.Items.Clear();
      for (int i = 0; i < list.NumberRows; i++)
        ctl.Items.Add(CommonFunctions.CString(list[i, 0]));
      if (val == null)
      {
        if (selectFirstIfNoValue && ctl.Items.Count > 0)
          ctl.Text = CommonFunctions.CString(list[0, 0]);
      }
      else
        ctl.Text = val;
    }

  }

  }

