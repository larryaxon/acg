using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ACG.Common;

namespace CCI.DesktopClient.Screens
{
  public partial class frmPDFtoText : ScreenBase
  {
    private string _basePath = "C:\\Data\\CityCareBills";
    private string _filePath;
    Dictionary<string, PDFtoText> _pdfFiles = new Dictionary<string, PDFtoText>();
    public frmPDFtoText()
    {
      InitializeComponent();
    }

    private void btnSelectFile_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog1 = new OpenFileDialog();

      openFileDialog1.InitialDirectory = _basePath;
      openFileDialog1.Title = "Select the PDF file to be imported";

     
      DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
      if (result == DialogResult.OK) // Test result.
      {
        txtFilePath.Text = _filePath =  openFileDialog1.FileName;

      }
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      PDFtoText thisFile = new PDFtoText(_filePath);
      _pdfFiles.Add(_filePath, thisFile);
      txtFileResult.Text = thisFile.ToString();
    }
  }
}
