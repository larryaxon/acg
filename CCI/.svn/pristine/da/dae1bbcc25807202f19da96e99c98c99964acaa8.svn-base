using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace TAGBOSS.Common
{
  public class DictionaryElements
  {
    #region method data
    private Dictionary<string, DictionaryElement> list = new Dictionary<string, DictionaryElement>(StringComparer.CurrentCultureIgnoreCase);
    private Dictionary<string, SortedList<string, byte>> index = new Dictionary<string, SortedList<string, byte>>(StringComparer.CurrentCultureIgnoreCase);
    private Dictionary<string, SortedList<string, string>> dataTypeIndex = new Dictionary<string, SortedList<string, string>>(StringComparer.CurrentCultureIgnoreCase);
    private Dictionaries Dictionary { get { return DictionaryFactory.getInstance().getDictionary(); } }
    #endregion

    #region public properties
    public DictionaryElement this[ExpressionType type, string name]
    {
      get 
      { 
        string key = DictionaryElement.makeID(type, name); 
        if (list.ContainsKey(key)) 
          return list[key]; 
        else 
          return new DictionaryElement(name, type, TAGFunctions.DATATYPEOBJECT, null); 
      }
    }
    public DictionaryElement this[string type, string name]
    {
      get
      {
        try { return this[(ExpressionType)Enum.Parse(typeof(ExpressionType), type), name]; }
        catch { return null; }
      }
    }
    #endregion
    public DictionaryElements()
    {
      list = Dictionary.getDictionaryElements();
      /* 
       * now populate the index
       * 
       * The index is a list of <Type, DataType> so we can ask for a list of appropriate entries  in the GUI.
       * 
       * We also have a datatype index so we can give ALL things that are of the correct datatype
       * 
       * Note that dataType object matches all other datatypes
       */
      foreach (KeyValuePair<string, DictionaryElement> entry in list) // load the index
      {
        // first the central index
        DictionaryElement element = entry.Value;
        string key = makeDataTypeKey(element.Type, element.DataType);
        SortedList<string, byte> idList;
        if (index.ContainsKey(key))
          idList = index[key];
        else
        {
          idList = new SortedList<string, byte>(StringComparer.CurrentCultureIgnoreCase);
          index.Add(key, idList);
        }
        idList.Add(element.ID, 0);  // "lookup" entry of type byte is dummy, just want a sorted list of IDs 
        // now the datatype index
        SortedList<string, string> dtList;
        if (dataTypeIndex.ContainsKey(element.DataType))
          dtList = dataTypeIndex[element.DataType];
        else
        {
          dtList = new SortedList<string, string>(StringComparer.CurrentCultureIgnoreCase);
          dataTypeIndex.Add(element.DataType, dtList);
        }
        dtList.Add(DictionaryElement.makeID(element.Type, element.Name), element.Name);
      }
    }

    #region public methods
    public List<DictionaryElement> getElement(string type, string dataType)
    {

      List<DictionaryElement> returnList = new List<DictionaryElement>();
      if (type.Equals(ExpressionType.Condition.ToString(), StringComparison.CurrentCultureIgnoreCase))
      {
        returnList.Add(new DictionaryElement("<Condition>", (ExpressionType)Enum.Parse(typeof(ExpressionType), type), dataType, null));
        return returnList;
      }
      if (type.Equals(ExpressionType.List.ToString(), StringComparison.CurrentCultureIgnoreCase))
      {
        returnList.Add(new DictionaryElement("<List>", (ExpressionType)Enum.Parse(typeof(ExpressionType), type), dataType, null));
        return returnList;
      }
      if (dataType.Equals(ExpressionType.Condition.ToString(), StringComparison.CurrentCultureIgnoreCase))
        return getElement(type, TAGFunctions.DATATYPEBOOLEAN);
      string[] dataTypeList;
      if (dataType.Equals(TAGFunctions.DATATYPEOBJECT, StringComparison.CurrentCultureIgnoreCase))
        dataTypeList = TAGFunctions.ValidDataTypes;
      else
        dataTypeList = new string[] { dataType };
      foreach (string dType in dataTypeList)
      {
        string indexKey = makeDataTypeKey(type, dType);
        if (index.ContainsKey(indexKey))
        {
          foreach (KeyValuePair<string, byte> entry in index[indexKey])
          {
            string key = entry.Key;
            returnList.Add(list[key]);
          }
        }
      }
      return returnList;
    }
    public List<DictionaryElement> getElement(string dataType)
    {
      List<DictionaryElement> returnList = new List<DictionaryElement>();
      foreach (KeyValuePair<string, string> entry in dataTypeIndex[dataType])
        returnList.Add(list[entry.Key]);
      return returnList;
    }
    #endregion

    #region private methods
    private string makeDataTypeKey(string type, string dataType)
    {
      return string.Format("{0}:{1}", type, dataType);
    }
    private string makeDataTypeKey(ExpressionType type, string dataType)
    {
      return makeDataTypeKey(type.ToString(), dataType);
    }
    #endregion
  }
}
