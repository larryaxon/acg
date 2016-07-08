using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAGBOSS.Common;
using TAGBOSS.Common.Model;

namespace TAGBOSS.Common
{
  public class FunctionElement
  {
    TableHeader m_Parameters;
    private string m_DataTypes = "";

    private string m_FunctionName = "";
    private string m_LibraryFile = "";
    private string m_ClassName = "";
    private bool m_PassAttributes = false;


    public FunctionElement(string LibraryFile, string ClassName, string FunctionName, TableHeader parameters, bool PassAttributes)
    {
      m_LibraryFile = LibraryFile;
      m_FunctionName = FunctionName;
      m_ClassName = ClassName;
      m_Parameters = parameters;
      if (parameters.Count > 0)
      {
        int dataTypeSub = 1;
        if (parameters.Columns.Count > 2) // is this the new one with the sequence column?
          dataTypeSub = 2;
        for (int i = 0; i < m_Parameters.GetLength(0); i++)
          m_DataTypes += m_Parameters[i, dataTypeSub].ToString() + "|";
        m_DataTypes = m_DataTypes.Substring(0, m_DataTypes.Length - 1);
        m_PassAttributes = PassAttributes;
      }
    }

    public string FunctionName
    {
      get { return m_FunctionName; }
      set { m_FunctionName = value; }
    }
    public int NumberParameters
    {
      get { return m_Parameters.GetLength(0); }
    }
    public string DataTypes
    {
      get { return m_DataTypes; }
      set { m_DataTypes = value; }
    }

    public bool PassAttributes
    {
      get { return m_PassAttributes; }
      set { m_PassAttributes = value; }
    }
    public string ClassName
    {
      get { return m_ClassName; }
      set { m_ClassName = value; }
    }

    public string LibraryFile
    {
      get { return m_LibraryFile; }
      set { m_LibraryFile = value; }
    }

    //public string Parameters
    //{
    //  get { return m_parameters; }
    //  set { m_parameters = value; }
    //}  
  }
}
