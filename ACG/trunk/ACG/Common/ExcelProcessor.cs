using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using EPPlus;

using System.Security.Policy;
using OfficeOpenXml;
using System.Data;
using System.Text.RegularExpressions;
using ACG.Common;
using OfficeOpenXml.Table;
//using System.Web.UI;
using OfficeOpenXml.FormulaParsing;
using System.Security.Cryptography;
using System.Reflection.Emit;
//using CallPoint.Models;

namespace ACG.Common
{
  public class ExcelProcessor : IDisposable
  {
    private ExcelPackage excel;
    private ExcelWorkbook workbook;
    public Dictionary<string, string> NumericFormats = null;
    public List<string> AlignmentFormats = null;
    public ExcelProcessor(Dictionary<string, string> numericformats, List<string> alignmentformats)
    {
      constructall(numericformats, alignmentformats);
    }
    public ExcelProcessor()
    {
      //List<ReportFormatModel> allformats = ReportingProcessor.GetReportFormats();
      //Dictionary<string, string> numericFormats = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
      //List<string> alignmentformats = new List<string>();
      //foreach (ReportFormatModel f in allformats)
      //{
      //  if ((f.FormatOutput ?? "Excel") == "Excel")
      //  {
      //    if (f.FormatType == "Number")
      //      numericFormats.Add(f.FormatName, f.FormatString);
      //    else if (f.FormatType == "Alignment")
      //      alignmentformats.Add(f.FormatName);

      //  }
      //}
      constructall(null, null);
    }
    private void constructall(Dictionary<string, string> numericformats, List<string> alignmentformats)
    {
      ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
      excel = new ExcelPackage();
      workbook = excel.Workbook;
      NumericFormats = numericformats;
      AlignmentFormats = alignmentformats;
    }
    public void Dispose()
    {
      excel.Dispose();
    }
    public void Save()
    {
      excel.Save();
    }
    public ExcelWorksheet AddSheet(string name)
    {
      workbook.Worksheets.Add(name);
      return GetSheet(name);
    }
    public ExcelWorksheet GetSheet(string name)
    {
      try
      {
        return workbook.Worksheets[name];
      }
      catch (Exception ex)
      {
        return null;
      }
    }
    public void RemoveSheet(string name)
    {
      workbook.Worksheets.Delete(name);
    }
    public static ExcelProcessor CreateWorkbookFromDataset(DataSet ds, List<string> tabnames = null, Dictionary<int,
      List<int>> selectmap = null, Dictionary<int, Dictionary<string, List<int>>> formats = null)
    {
      ExcelProcessor workbook = new ExcelProcessor();
      if (ds != null && ds.Tables.Count > 0)
      {
        int tabindex = 0;
        string[] tabarray = null;
        if (tabnames != null && tabnames.Count > 0)
        {
          tabarray = tabnames.ToArray();
        }

        if (selectmap != null && selectmap.Count > 0) // do we have a map of multiple selects/tables to tabs?
        {
          // yes, so navigate through the map and place the selects/tables as directed
          /*
           * Note: the mapping between tab names and selects/tables has is ordered by the order of the tabinfo array in the json block
           * we don't create empty tabs, so if the block "skips" a tab, we don't skip it in the spreadsheet
           */
          foreach (KeyValuePair<int, List<int>> tabmap in selectmap)
          {
            string locationcol = "A"; // for now, we always place in the left most column. Maybe in the future will will add the ability to change this
            int locationrow = 1; // start the first select/table in the first row
            int itab = tabmap.Key - 1; // tab number is json is 1 based, but in excel tabs it is zero based
            string tabname = tabarray[itab];
            bool firsttime = true;
            foreach (int tablenbr in tabmap.Value)
            {
              int iTable = tablenbr - 1; // Table number in json is 1-based, but in ds.Tables it is zero based
              if (iTable < ds.Tables.Count)
              {
                DataTable dt = ds.Tables[iTable];
                if (firsttime) // is this the first select/table in this tab
                {
                  // yes, so set the row to place the table at 1
                  firsttime = false;
                  locationrow = 1; // row number in excel is 1 based (no such thing as cell A0)
                }
                Dictionary<string, List<int>> thisformat;
                if (formats != null && formats.ContainsKey(tablenbr))
                  thisformat = formats[tablenbr];
                else
                  thisformat = null;
                workbook.CreateWorksheetFromDataTable(dt, tabname, CellID(locationcol, locationrow), true, thisformat);
                locationrow += dt.Rows.Count + 2; // now move the location down to the bottom for the next one (if there is one)

              }
            }
          }
        }
        else
        {
          // no map, so we just add a tab for each table
          foreach (DataTable dt in ds.Tables)
          {
            string tabname = dt.TableName; // if we don't specify the tab names in tabinfo, then we use the table name
            if (tabnames != null && tabindex < tabnames.Count) // but if we do have a tab name
              tabname = tabarray[tabindex]; // we use it
            tabindex++;
            workbook.CreateWorksheetFromDataTable(dt, tabname, "A1");
          }
        }
      }
      return workbook;
    }
    public ExcelWorksheet CreateWorksheetFromDataset(DataSet ds, string name, string locationcell = "A1")
    {
      // get the first table in the dataset. If its not there, make it null
      DataTable dt = (ds == null ? null : (ds.Tables.Count == 0 ? null : ds.Tables[0]));
      return CreateWorksheetFromDataTable(ds.Tables[0], name, locationcell);
    }
    public ExcelWorksheet CreateWorksheetFromDataTable(DataTable dt, string name, string locationcell = "A1",
      bool formattables = true, Dictionary<string, List<int>> formats = null)
    {
      if (workbook.Worksheets[name] == null)
        AddSheet(name);
      ExcelWorksheet ws = GetSheet(name);
      if (dt != null)
        ws.Cells[locationcell].LoadFromDataTable(dt, true);
      if (formattables)
      {
        // go back and reformat the header cells
        int nbrcols = dt.Columns.Count;
        int nbrrows = dt.Rows.Count;
        int irow = RowPart(locationcell); // upper left hand of table
        int tablenumber = 1;
        int icol = 1;
        // First, let's "fix up" the column titles
        for (int c = 1; c <= nbrcols; c++)
        {
          string headertext = ws.Cells[irow, c].GetCellValue<string>();
          headertext = ACGTable.PrettyPrintName(headertext);
          ws.Cells[irow, c].SetCellValue(0, 0, headertext);
        }
        ExcelRange range = ws.Cells[irow, icol, irow + nbrrows, icol + nbrcols - 1];
        //add a table to the range
        // Excel table names don't allow special characters except underscore, and don't allow spaces
        string tablename = (name + "_" + locationcell + "_" + tablenumber.ToString()).Replace(" ", "_");
        tablename = Regex.Replace(tablename, "[^0-9A-Za-z_]", string.Empty);
        ExcelTable tab = ws.Tables.Add(range, tablename);
        if (nbrcols > 1)
          range.AutoFitColumns();
        //format the table
        tab.TableStyle = TableStyles.Medium2;
        // now apply formatting
        range = formatCells(range, formats);
      }

      return ws;
    }
    private ExcelRange formatCells(ExcelRange range, Dictionary<string, List<int>> formats = null)
    {
      if (formats == null) // if we don't have any
        return range; // then just return the range unchanged
      // now just go through the list of formats
      foreach (KeyValuePair<string, List<int>> format in formats)
      {
        if (format.Key != "None") // if there is none, then don't do anything
        {
          /*
           * get a list of ranges to format. 
           * I could just go through column by column but I think creating ranges
           * will be faster once the format is applied
           */
          List<ExcelRange> formatRanges = getFormatRanges(range, format.Value);
          foreach (ExcelRange formattingRange in formatRanges)
          {
            if (NumericFormats.ContainsKey(format.Key))
              formattingRange.Style.Numberformat.Format = NumericFormats[format.Key];
            else if (AlignmentFormats.Contains(format.Key, StringComparer.CurrentCultureIgnoreCase))
            {
              string akey = format.Key.ToLower().Trim();
              switch (akey)
              {
                case "left":
                  formattingRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                  break;
                case "right":
                  formattingRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                  break;
                case "center":
                  formattingRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                  break;
              }
            }
          }
        }
      }
      return range; // hopefull, this range now has the formatting the specified areas
    }
    private List<ExcelRange> getFormatRanges(ExcelRange range, List<int> columns)
    {
      /*
       * get a list of columns in the range. Look for contiguous columns and create single ranges
       * for them
       * 
       * So, for example, if the columns are { 5, 7, 8, 9, 10 } 
       * then create ranges:
       *    5-5
       *    7-10
       * 
       * Of course, this is a range, not the whold worksheet, so we just format the cells
       * within the rows for this range
       */
      List<ExcelRange> ranges = new List<ExcelRange>();
      ExcelWorksheet ws = range.Worksheet;
      columns.Sort();
      int rowstart = range.Start.Row;
      int rowend = range.End.Row;
      int colstart = range.Start.Column;
      int colend = range.End.Column;
      List<int> startcols = new List<int>();
      List<int> endcols = new List<int>();
      int lastcol = -1;
      /*
       * creates a list of ranges. startcols is the list of the first cols
       * endcols is the corresponding last column
       */
      foreach (int col in columns)
      {
        if (col > lastcol + 1) // there is a gap
        {
          startcols.Add(col);
          if (lastcol != -1) // not the first one
            endcols.Add(lastcol);
        }
        lastcol = col;
      }
      endcols.Add(lastcol);
      for (int i = 0; i < startcols.Count; i++)
      {
        ExcelRange r = ws.Cells[rowstart, startcols[i], rowend, endcols[i]];
        ranges.Add(r);
      }
      return ranges;
    }
    public static string ColumnPart(string locationcell)
    {
      string col = Regex.Replace(locationcell, @"[\d-]", string.Empty);
      return col;
    }
    public static int RowPart(string locationcell)
    {
      string numbers = Regex.Replace(locationcell, @"^[A-Za-z]+", "");
      return CommonFunctions.CInt(numbers);
    }
    public static string CellID(string col, int row)
    {
      return col + row.ToString();
    }
    public static string CellID(int col, int row)
    {
      return ColumnNumberToLetters(col) + row.ToString();
    }
    public static string ColumnNumberToLetters(int colnbr)
    {
      const int NBRLETTERS = 26;
      int originalcolnbr = colnbr;
      int nbralphabets = colnbr / NBRLETTERS;
      int currentalphabet = 1;
      string returnvalue = string.Empty;
      int currentalphabetnbr = colnbr % NBRLETTERS;
      do
      {
        char c = (char)currentalphabetnbr;
        returnvalue += c;
      } while (++currentalphabet <= nbralphabets);
      return returnvalue;
    }
    public MemoryStream ToStream()
    {
      MemoryStream stream = new MemoryStream();
      var bytes = excel.GetAsByteArray();
      stream.Write(bytes, 0, bytes.Length);
      return stream;
    }
    public Byte[] ToByteArray()
    {
      return excel.GetAsByteArray();
    }
  }
}
