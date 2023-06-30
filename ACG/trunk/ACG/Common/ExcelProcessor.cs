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
using System.ComponentModel;

namespace ACG.Common
{
  public class ExcelProcessor : IDisposable
  {
    private ExcelPackage excel;
    private ExcelWorkbook workbook;
    public ExcelProcessor()
    {
      ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
      excel = new ExcelPackage();
      workbook = excel.Workbook;
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
    public ExcelWorksheet CreateWorksheetFromDataset(DataSet ds, string name, string locationcell = "A1")
    {
      if (workbook.Worksheets[name] == null)
        AddSheet(name);
      ExcelWorksheet ws = GetSheet(name);
      ws.Cells[locationcell].LoadFromDataTable(ds.Tables[0], true);
      return ws;
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

