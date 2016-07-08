using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using TAGBOSS.Common.Model;

namespace TAGBOSS.Common
{
  public class TAGExceptionMessage
  {
    #region private data
    private string className = string.Empty;
    private string methodName = string.Empty;
    private string errorMessage = string.Empty;
    private ArrayList parmList = new ArrayList();
    private const string MSGBEGIN = "Error<";
    private const string MSGEND = "> in ";
    #endregion private data
    #region constructors
    public TAGExceptionMessage() { }

    public TAGExceptionMessage(string pClassName, string pMethodName, string pError)
    {
      className = pClassName;
      methodName = pMethodName;
      errorMessage = pError;
    }
    #endregion constructors
    #region public properties
    public string ClassName
    {
      get { return className; }
      set { className = value; }
    }
    public string ErrorMessage
    {
      get { return errorMessage; }
      set { errorMessage = value; }
    }
    public string MethodName
    {
      get { return methodName; }
      set { methodName = value; }
    }
    #endregion public properties
    #region public methods
    public void AddParm(object parm)
    {
      if (parm == null)
        parmList.Add("null");
      else
        if (parm.GetType().Name.ToString() == "Item")
        {
          Item i = (Item)parm;
          parmList.Add("Item=" + i.ID);
          //parmList.Add(i.ToAttributeTable().ToString());
        }
        else
          parmList.Add(parm);
    }
    public new string ToString()
    {
      return string.Format("{0}{1}{2}{3}.{4}({5})", MSGBEGIN, errorMessage, MSGEND, className, methodName, ToParmList());
    }
    private string ToParmList()
    {
      bool listHasBegun = false;
      string s = string.Empty;
      foreach (object o in parmList)
      {
        if (listHasBegun)
          s += ", ";
        else
          listHasBegun = true;
        s += (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, o);
      }
      return s;
    }
    /// <summary>
    /// takes a tm-created message and delivers just the inside message
    /// </summary>
    /// <param name="tmMessage"></param>
    public static string InnerMessage(string tmMessage)
    {
      if (tmMessage == null || !tmMessage.StartsWith(MSGBEGIN) || !tmMessage.Contains(MSGEND))
        return tmMessage;
      int iLeft = tmMessage.IndexOf(MSGBEGIN, StringComparison.CurrentCultureIgnoreCase) + MSGBEGIN.Length;
      int iRight = tmMessage.LastIndexOf(MSGEND, StringComparison.CurrentCultureIgnoreCase);  // first look for phrase that ends the msg
      if (iRight == -1)  // if it is not there, then look for an ending ">"
        if (tmMessage.EndsWith(">"))
          iRight = tmMessage.Length - 2;
      if (iRight == -1)  // if neither were found, then just make it the last character of the string
        iRight = tmMessage.Length - 1;
      int iLen = iRight - iLeft + 1;
      if (iLen <= 0 || iLeft + 1 + iLen > tmMessage.Length)
        return tmMessage;
      string insideString = tmMessage.Substring(iLeft, iLen);
      if (insideString.StartsWith(MSGBEGIN))
        return InnerMessage(insideString);
      else
        if (insideString.EndsWith(">"))
          return insideString.Substring(0, insideString.Length - 2);
        else
          return insideString;
    }
    #endregion public methods
  }
}
