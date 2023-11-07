using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronOcr;

namespace ACG.Common
{
  public class PDFtoText
  {
    private AutoOcr _ocr = new AutoOcr();
    private OcrResult _result;
    private string _text;
    public PDFtoText(string filepath)
    {
      var Ocr = new IronOcr.AutoOcr();
      _result = _ocr.Read(filepath);
      _text = _result.Text;
    }
    public override string ToString()
    {
      return _text;
    }
  }
}
