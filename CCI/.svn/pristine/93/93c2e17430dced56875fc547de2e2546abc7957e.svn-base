using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;
using TAGBOSS.Common.Interface;

namespace TAGBOSS.Common
{
  /// <summary>
  /// Parameters class is responsible for handling the parameters entered by any calling object. 
  /// Its responsibility is to encapsulate the DOM complexity for a simple interface
  /// </summary>
  [Serializable]
  public class Parameters
  {
    #region module data
    private DataClass<ParameterEntry> parameterList = new DataClass<ParameterEntry>();
    //const string c_XPATH_ATTR_AT_SOLVE_ATTR = "/parameters/{0}";
    const string c_XPATH_ATTR_AT_SOLVE_ATTR_CAPTION = "/{0}/{1}/@caption"; 
    const string c_XPATH_ATTR_AT_SOLVE_ATTR_DATATYPE = "/{0}/{1}/@datatype";

    const string c_ATTR_AT_PARAM_VALUE = "value";
    const string c_ATTR_AT_PARAM_DATATYPE = "datatype";

    string rootName = "parameters";
    #endregion module data

    #region constructors
    /// <summary>
    /// The constructor build the DOM, no parameter passed to this constructor instance...
    /// </summary>
    public Parameters()
    {
    }

    /// <summary>
    /// The constructor build the DOM 
    /// </summary>
    /// <param name="xml">xml parameters </param>
    public Parameters(string xmlParametersDOM)
    {
      LoadXML(xmlParametersDOM);
      //m_parametersDOM = new XmlDocument();
      //m_parametersDOM_lowercaseNames = new XmlDocument();
      //if (xmlParametersDOM.Trim() != string.Empty)
      //{
      //  m_xmlParametersDOM = xmlParametersDOM;
      //  m_parametersDOM.LoadXml(m_xmlParametersDOM);
      //  LoadLowercaseXML();
      //}
    }
    #endregion constructors

    #region public properties
    /// <summary>
    /// returns the number of Parameters in the class
    /// </summary>
    public int Count
    {
      get { return parameterList.Count; }
    }
    public string this[int index]
    {
      get { return parameterList[index].Name; }
    }
    /// <summary>
    /// Property to export the value of the corresponding parameter...
    /// </summary>
    /// <param name="parameterName"></param>
    /// <returns></returns>
    public string this[string parameterName]
    {
      get
      {
        string key = parameterName.ToLower();

        if (parameterList.Contains(key))
          return parameterList[key].Value;
        else
          return string.Empty;
      }
      set
      {
        string key = parameterName.ToLower();

        if (parameterList.Contains(key))
          parameterList[key].Value = value;
        else
        {
          ParameterEntry entry = new ParameterEntry();
          entry.ID = parameterName;
          entry.Value = value;
          parameterList.Add(entry);
        }
      }
    }
    #endregion public propeties

    #region public methods

    /// <summary>
    /// Add a new parameter with its value to the collection
    /// </summary>
    /// <param name="parameterName"></param>
    /// <param name="parameterValue"></param>
    public void Add(string parameterName, string parameterValue)
    {
      Add(parameterName, parameterValue, null);
    }
    /// <summary>
    /// Add a new parameter with its value and datatype to the collection
    /// </summary>
    /// <param name="parameterName"></param>
    /// <param name="parameterValue"></param>
    /// <param name="dataType"></param>
    public void Add(string parameterName, string parameterValue, string dataType)
    {
      ParameterEntry pEntry = new ParameterEntry();
      string key = parameterName.ToLower();
      pEntry.ID = parameterName;
      pEntry.Value = parameterValue;
      pEntry.DataType = dataType;
      if (parameterList.Contains(key))
        parameterList[key] = pEntry;
      else
        parameterList.Add(key, pEntry);
    }
    /// <summary>
    /// Does the list of parameters contain a parameter named paramterName?
    /// </summary>
    /// <param name="parameterName"></param>
    /// <returns></returns>
    public bool Contains(string parameterName)
    {
      return parameterList.Contains(parameterName);
    }
    /// <summary>
    /// Remove the parameter with the name parameterName from the list
    /// </summary>
    /// <param name="parameterName"></param>
    public void Remove(string parameterName)
    {
      parameterList.Remove(parameterName);
    }
    public void Clear()
    {
      parameterList.Clear();
    }
    /// <summary>
    /// return an XML string that defines the current parameter list
    /// </summary>
    /// <returns></returns>
    public string ToXML()
    {
      if (parameterList.Count == 0)
        return "<parameters></parameters>";
      else
      {
          //string newXML = "<parameters>\n";
          string newXML = "<parameters>";
          foreach (ParameterEntry pe in parameterList)
        {
          newXML += pe.ToXML();
        }
        newXML += "</parameters>";
        return newXML;
      }
    }
    /// <summary>
    /// Used by IEnumerable foreach feature to enumerate the names of the parameters in the list
    /// </summary>
    /// <returns></returns>
    public ParameterEnumerator GetEnumerator()
    {
      /*
       * support the standard GetEnumerator() method from IEnumerator interface so we support foreach
       */
      return new ParameterEnumerator(this);
    }
    public ParameterEntry getParameterEntry(string parameterName)
    {
      if (parameterList.Contains(parameterName))
        return parameterList[parameterName];
      else
        return null;
    }
    #endregion  public methods

    #region classes
    /// <summary>
    /// Class required by IEnumerable foreach GetEnumerator()
    /// </summary>
    public class ParameterEnumerator
    {
      int nIndex;
      Parameters collection;
      public ParameterEnumerator(Parameters coll)
      {
        collection = coll;
        nIndex = -1;
      }
      public bool MoveNext()
      {
        nIndex++;
        return (nIndex < collection.Count);
      }
      public object Current
      {
        get
        {
          return (collection[nIndex]);
        }
      }
      public void Dispose() { ;}
      public void Reset() { ;}
    }
    /// <summary>
    /// A single parameter in the list with all of its attributes
    /// </summary>
    [Serializable]
    public class ParameterEntry : IDataClassItem
    {
      private bool dirty = false;
      private bool deleted = false;
      private bool readOnly = false;
      private string name = null;
      private string caption = null;

      private string myValue = null;
      private string dataType = null;
      private Dictionaries dictionary = null;

      public Dictionaries Dictionary
      {
        get { return dictionary; }
        set { dictionary = value; }
      }

      public string Caption
      {
        get 
        {
          if (caption != null && caption != string.Empty)
            return caption;
          else
            return name;
        }
        set { caption = value; }
      }
      public string Name
      {
        get { return name; }
        set { name = value; }
      }
      public string Value
      {
        get { return myValue; }
        set { myValue = value; }
      }
      public bool Dirty
      {
        get { return dirty; }
        set { dirty = value; }
      }
      public bool Deleted
      {
        get { return deleted; }
        set { deleted = value; }
      }
      public bool ReadOnly
      {
        get { return readOnly; }
        set { readOnly = value; }
      }
      public string DataType
      {
        get { return dataType; }
        set { dataType = value; }
      }
      public string ID
      {
        get { return Name; }
        set { Name = value; }
      }
      public object Clone()
      {
        ParameterEntry p = new ParameterEntry();
        p.Name = Name;
        p.Value = Value;
        p.Dirty = Dirty;
        p.Deleted = Deleted;
        p.ReadOnly = ReadOnly;
        p.DataType = DataType;
        return (object)p;
      }
      public string ToXML()
      {
        string newXML = "<" + Name + " value=\"" + Value + "\"";
        if (DataType != null)
          newXML += " datatype=\"" + DataType + "\"";
        newXML += "/>";
        //newXML += "/>\n";
        return (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.cleanXMLString, newXML);
      }

    }

    #endregion classes

    #region private methods
    private void LoadXML(string xmlParameters)
    {
      XmlDocument xDoc = new XmlDocument();
      xDoc.LoadXml(xmlParameters);
      XmlNode rootNode = xDoc.ChildNodes[0];
      XmlNodeList xmlParmList = rootNode.SelectNodes("*");
      if (xmlParmList != null)
      {
        foreach (XmlNode xmlParameter in xmlParmList)
        {
          ParameterEntry pEntry = new ParameterEntry();
          string key = xmlParameter.Name.ToLower();
          pEntry.ID = xmlParameter.Name;
          pEntry.Value = xmlParameter.Attributes[c_ATTR_AT_PARAM_VALUE].Value;
          XmlAttribute xmlValueDatatype = xmlParameter.Attributes["datatype"];
          if (xmlValueDatatype != null && (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, xmlValueDatatype.Value) != string.Empty)
            pEntry.DataType = xmlValueDatatype.Value;
          XmlAttribute caption = xmlParameter.Attributes["caption"];
          if (caption != null && (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, caption.Value) != string.Empty)
            pEntry.Caption = caption.Value;
          parameterList.Add(key, pEntry);
        }
      }
    }
    /// <summary>
    /// This converts all the names of the loaded xml to lowercase! in a different xml document
    /// </summary>
    //private void LoadLowercaseXML()
    //{
    //  string strParametersLowercase = "<parameters>";
    //  XmlNodeList xmlParameters = m_parametersDOM.SelectNodes("//parameters/*");
    //  if (xmlParameters != null)
    //  {
    //    foreach (XmlNode xmlParameter in xmlParameters)
    //    {
    //      string strParameter =
    //        "<" + xmlParameter.Name.ToLower().Trim()
    //        + " " + c_ATTR_AT_PARAM_VALUE
    //        + "='" + xmlParameter.Attributes[c_ATTR_AT_PARAM_VALUE].Value + "'";
    //      XmlNode xmlValueDatatype = m_parametersDOM.SelectSingleNode(string.Format(c_XPATH_ATTR_AT_SOLVE_ATTR_DATATYPE, xmlParameter.Name));
    //      if (xmlValueDatatype != null)
    //        strParameter += " datatype='" + xmlValueDatatype.Value + "'/>";
    //      else
    //        strParameter += "/>";
    //      strParametersLowercase += strParameter;
    //    }
    //  }
    //  strParametersLowercase += "</parameters>";
    //  m_parametersDOM_lowercaseNames.LoadXml(strParametersLowercase);
    //}
    #endregion private methods
  }
}
