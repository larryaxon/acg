using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAGBOSS.Common.Model;

namespace TAGBOSS.Common
{
  public class DictionaryElement
  {
    private Dictionary<int, string> _parameters = new Dictionary<int, string>();
    private string _listDataType = null;
    public string Name { get; set; }
    public ExpressionType Type { get; set; }
    public string DataType { get; set; }
    public string ListDataType { get { return _listDataType; } set { _listDataType = value; } }
    public string AlternateListDataType { get; set; }
    public Dictionary<int, string> Parameters { get { return _parameters; } }
    public string ID { get { return makeID(Type, Name); } }
    public DictionaryElement(string name, ExpressionType type, string dataType, TableHeader functionDefinition)
    {
      Name = name;
      if (name != null && name.Length > 2 && name.StartsWith("<"))  // this is a "special" name (<List> or <Condition>)
        Name = name.Substring(1, name.Length - 2);
      Type = type;
      AlternateListDataType = null;

        if (dataType.StartsWith("list", StringComparison.CurrentCultureIgnoreCase))
        {
          //TODO: OUT parameter! We must review this!
          //DataType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getFunctionName, dataType, out _listDataType);
          DataType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getFunctionName, dataType, out _listDataType);
          if (_listDataType.Contains(TAGFunctions.COLSEPARATORCHAR))
          {
            string[] parts = _listDataType.Split(new char[] { TAGFunctions.COLSEPARATORCHAR });
            ListDataType = parts[0];
            AlternateListDataType = parts[1];
          }
        }
        else
        {
          DataType = dataType;
          ListDataType = null;
        }
      if (type == ExpressionType.Condition)
      {
        dataType = TAGFunctions.DATATYPECONDITION;
        _parameters.Add(0, "operand1:object");
        _parameters.Add(1, "operator:string");
        _parameters.Add(2, "operand2:object");
      }
      else
      {
        if (Type == ExpressionType.Function && functionDefinition != null)
        {
          foreach (TableHeaderRow row in functionDefinition)
          {
            // calculate the parameter sequence. If a valid value is not stored in the table, then find the next available one
            int seq = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, row["Sequence"].Value, 0);
            while (_parameters.ContainsKey(seq)) // this seq has already been used
              seq++;
            string parmName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row["Parameter"].Value);
            string parmType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, row["DataType"].Value);
            _parameters.Add(seq, string.Format("{0}:{1}", parmName, parmType));
          }
        }
      }
    }
    public static string makeID(string type, string name)
    {
      return string.Format("{0}:{1}", type, name);
    }
    public static string makeID(ExpressionType type, string name)
    {
      return makeID(type.ToString(), name);
    }

  }
}
