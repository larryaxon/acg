using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CCI.Common
{
  public static class FieldValidation
  {
    private static CCITable _info = null;
    private static Hashtable _dataType = new Hashtable();
    private static Hashtable _length = new Hashtable();
    private static Hashtable _precision = new Hashtable();
    private static Hashtable _scale = new Hashtable();
    private static string[] _columns = new string[] { "Field", "DataType", "Length", "Precision", "Scale" };


    public static string IsValid(string fieldname, object val)
    {
      if (string.IsNullOrEmpty(fieldname))
        return "Fieldname cannot be null or empty";
      if (val == null)
        return string.Empty;
      string datatype = CommonFunctions.CString(_dataType[fieldname]).ToLower();
      switch (datatype)
      {
        case "nvarchar": 
          string s = val.ToString();
          if (s.Length > CommonFunctions.CInt(_length[fieldname]))
            return string.Format("Value ({0} is too long for field {1}",s, fieldname);
          return string.Empty;
        case "datetime":
        case "date":
          DateTime dt = CommonFunctions.CDateTime(val);
          if (dt < CommonData.PastDateTime || dt > CommonData.FutureDateTime)
            return string.Format("Date/Time {0} is out of range for field {1}", val.ToString(), fieldname);
          return string.Empty;
      }
      return string.Empty;
    }

    public static ArrayList ValidFields(Hashtable fieldValues)
    {
      ArrayList errors = new ArrayList();
      foreach (DictionaryEntry entry in fieldValues)
      {
        string error = IsValid(CommonFunctions.CString(entry.Key), entry.Value);
        if (!string.IsNullOrEmpty(error))
          errors.Add(error);
      }
      return errors;
    }
    public static void load(CCITable info)
    {
      for (int i = 0; i < _columns.GetLength(0); i++)
        if (!info.ContainsColumn(_columns[i]))
          throw new Exception("FieldValidationLoad must have a valid ATKTable list of columns");
      _info = info;
      for (int i = 0; i < _info.NumberRows; i++)
      {
        string fld = CommonFunctions.CString(_info[i,"Field"]);
        if (_dataType.ContainsKey("Field"))
          _dataType.Add(fld,CommonFunctions.CString(_info[i,"DataType"]));
        if (_length.ContainsKey("Field"))
          _length.Add(fld, CommonFunctions.CInt(_info[i, "Length"]));
        if (_precision.ContainsKey("Field"))
          _precision.Add(fld, CommonFunctions.CInt(_info[i, "Precision"]));
        if (_scale.ContainsKey("Field"))
          _scale.Add(fld, CommonFunctions.CInt(_info[i, "Scale"]));
      }
    }
  }
}
