using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common
{
  public class FunctionExpression
  {

    #region method data
    private string _text = null;
    private string _comment = string.Empty;
    private bool _isCondition = false;
    private bool _isList = false;
    private bool _isOperator = false;
    private bool _isAtAt = false;
    #endregion

    #region public properties
    public bool IsEmpty { get; set; }
    public bool HasError { get { return (ErrorMessage != null); } }
    public string ErrorMessage { get; set; }
    public char Prefix { get; set; }
    public string Name { get; set; }
    public string ID { get { if (Name == null || Name == string.Empty) return "none"; else return Name.ToLower(); } }
    public int Minus { get; set; }
    public string Comment { get { return _comment; } }
    public string[] Tokens { get; set; }
    public string Text { get { return _text; } }
    public ExpressionType Type
    {
      get
      {
        if (Prefix == TAGFunctions.FUNCTIONCHAR)
          return ExpressionType.Function;
        else
          if (Prefix == TAGFunctions.ATTRIBUTECHAR[0])
          {
            if (_isAtAt)
              return ExpressionType.AtAtValue;
            else
              return ExpressionType.Attribute;
          }
          else
            if (_isCondition)
              return ExpressionType.Condition;
            else
              if (_isList)
                return ExpressionType.List;
              else
                if (_isOperator)
                  return ExpressionType.Operator;
                else
                  return ExpressionType.Literal;
      }
    }
    #endregion

    public FunctionExpression(string expression)
    {
      _text = expression;
      loadExpression(expression);
    }
    #region public methods
    public static string[] getTypeList()
    {
      int nbrTypes = (int)ExpressionType.Operator + 1;  // ick! isn't there a better way to find the number of entries?
      string[] returnList = new string[nbrTypes];
      for (int i = 0; i < nbrTypes; i++)
        returnList[i] = ((ExpressionType)i).ToString();
      return returnList;
    }
    public string ToShortString()
    {
      return ToString(false);
    }
    public override string ToString()
    {
      return ToString(true);
    }
    #endregion
    #region private methods
    private void loadExpression(string expression)
    {
      ErrorMessage = null;
      Prefix = ' ';
      Name = string.Empty;
      IsEmpty = true;
      _comment = string.Empty;
      Minus = 1;
      //TODO: OUT Parameter: We must review this!
      //expression = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, expression, true, out _comment));
      expression = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, expression, true, out _comment));
      if (expression == null || expression == string.Empty)
        return;
      foreach (string op in TAGFunctions.operatorList)
        if (expression == op)
        {
          _isOperator = true;
          Name = expression;
          return;
        }
      IsEmpty = false;
      Prefix = expression[0];
      if (Prefix == TAGFunctions.MINUSCHAR[0])
      {
        Minus = -1;
        expression = expression.Substring(1);
        if (expression.Length > 0)
          Prefix = expression[0];
        else
        {
          ErrorMessage = "Minus prefix must have expression after it";
          Prefix = ' ';
          return;
        }
      }
      if (Prefix == TAGFunctions.LITERALCHAR[0])
      {
        if (expression.Length > 1)
          Name = expression.Substring(1).Trim();
      }
      else
      {
        if (Prefix == TAGFunctions.FUNCTIONCHAR)
        {
          string insideString;
          //TODO: OUT Parameter: Need to review this
          //Name = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getFunctionName, expression, out insideString);
          Name = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getFunctionName, expression, out insideString);
          
          Tokens = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, insideString);
        }
        else
        {
          if (Prefix == TAGFunctions.LEFTCHAR)
          {
            _isList = true;
            Prefix = ' ';
            Name = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, expression);
            Tokens = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, Name);
          }
          else
          {
            foreach (string op in TAGFunctions.operatorList)
              if (expression.Contains(op))
              {
                _isCondition = true;
                break;
              }
            if (_isCondition)
            {
              string operand1 = string.Empty;
              string op = string.Empty;
              string operand2 = string.Empty;
              //TODO: REF Parameters: We need to review this
              //TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseValidationString, expression, ref operand1, ref op, ref operand2);
              TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseValidationString, expression, ref operand1, ref op, ref operand2);
              if (op == string.Empty) // single term condition
                Tokens = new string[] { operand1 };
              else
              {
                Tokens = new string[3];   // we have operand1 op operand2 (e.g. A==0)
                Tokens[0] = operand1;
                Tokens[1] = op;
                Tokens[2] = operand2;
              }
              Prefix = ' ';
              Name = expression;
            }
            else
            {
              if (Prefix == TAGFunctions.ATTRIBUTECHAR[0])
              {
                if (expression.Length == 1)
                {
                  ErrorMessage = "Attribute name must be at least one character long";
                  return;
                }
                if (expression[1] == TAGFunctions.ATTRIBUTECHAR[0]) // is there a second @ ?
                {
                  _isAtAt = true;
                  Name = expression.Substring(2, expression.Length - 2);
                }
                else
                  Name = expression.Substring(1);
              }
              else
              {
                Prefix = ' ';
                Name = expression;
              }
            }
          }
        }
      }
    }
    private string ToString(bool verboseMode)
    {
      StringBuilder sb = new StringBuilder();
      bool firstTime = true;
      if (Minus == -1)
        sb.Append('-');
      switch (Type)
      {
        case ExpressionType.Operator:
          sb.Append("Operator: ");
          sb.Append(Name);
          break;
        case ExpressionType.Function:
          sb.Append("Function: ");
          sb.Append(Name);
          sb.Append('(');
          if (verboseMode)
            foreach (string token in Tokens)
            {
              if (firstTime)
                firstTime = false;
              else
                sb.Append(',');
              FunctionExpression exp = new FunctionExpression(token);
              sb.Append(exp.ToString());
            }
          sb.Append(')');

          break;
        case ExpressionType.Attribute:
          sb.Append("Attribute: ");
          sb.Append(Name);
          break;
        case ExpressionType.AtAtValue:
          sb.Append("AtAtValue: ");
          sb.Append(Name);
          break;
        case ExpressionType.Condition:
          sb.Append("Condition: ");
          string operand1 = string.Empty;
          string op = string.Empty;
          string operand2 = string.Empty;
          //TODO: REF Parameters: We need to review this
          //TAGFunctions.parseValidationString(Name, ref operand1, ref op, ref operand2);
          TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseValidationString, Name, ref operand1, ref op, ref operand2);
          sb.Append(operand1);
          if (op != string.Empty)
          {
            sb.Append(' ');
            sb.Append(op);
            sb.Append(' ');
            sb.Append(operand2);
          }
          break;
        case ExpressionType.Literal:
          sb.Append("Value: ");
          if (Name == null || Name == string.Empty)
            sb.Append("<null>");
          sb.Append(Name);
          break;
        case ExpressionType.List:
          sb.Append("List: ");
          if (verboseMode)
          foreach (string token in Tokens)
          {
            if (firstTime)
              firstTime = false;
            else
              sb.Append(',');
            sb.Append(new FunctionExpression(token).ToString());
          }
          break;
      }
      if (_comment != string.Empty)
      {
        sb.Append(" Comment: ");
        sb.Append(_comment);
      }
      if (HasError)
      {
        sb.Append(" Error: ");
        sb.Append(ErrorMessage);
      }
      return sb.ToString();
    }
    #endregion
  }

}
