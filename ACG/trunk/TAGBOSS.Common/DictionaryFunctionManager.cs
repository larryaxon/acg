using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Reflection.Emit;
using System.IO;

using TAGBOSS.Common;
using TAGBOSS.Common.Model;

namespace TAGBOSS.Common
{
  public class DictionaryFunctionManager
  {
    const string c_FUNCTION = "function";
    const string c_DICTIONARY = "dictionary";
    const string c_ATTR_1_AT_CHAR = "@";
    const string className = "AttributeLibraryManager";
    const string c_FILE = "TAGBOSS.Functions.dll";
    const string c_CLASS_NAME = "TAGBOSS.Functions.AttributeFunctions";
    const string c_LIBRARY_NAME = "TAGBOSS.Functions.Library";
    private Dictionaries dictionary = DictionaryFactory.getInstance().getDictionary();
    private ItemType m_functionDictionary;
    private Hashtable m_functionList = new Hashtable();
    private DictionaryFunctions attributeFunctions = new DictionaryFunctions();

    //public EntityAttributesCollection functionDictionary
    //{
    //  get { return m_functionDictionary; }
    //  set { m_functionDictionary = value; }
    //}

    public DictionaryFunctionManager()
    {
      m_functionDictionary = dictionary.DictionaryProperties("Function");
      LoadFunctionList();
      //Nohing to do in this case...
    }

    //public DictionaryFunctionManager(EntityAttributesCollection functionDictionary)
    //{
    //  m_functionDictionary = functionDictionary;
    //  if (m_functionDictionary != null)
    //    LoadFunctionList();
    //}

    private void LoadFunctionList()
    {
      //string Parameters = "";
      //string LibraryName = "";
      string File = "";
      string ClassName = "";
      string FunctionName = "";
      TableHeader parameters;
      //string path = "";
      bool PassAttributes = false;
      string AttributeList = string.Empty;

      //if (m_functionDictionary == null)
      //  return;

      //if (m_functionDictionary.Entities = null || m_functionDictionary.Entities.Count = 0 || !(m_functionDictionary.Entities.Contains(c_DICTIONARY)))
      //  return;

      //if (m_functionDictionary == null  
      //  || !(m_functionDictionary.Items.Contains(c_FUNCTION)) || m_functionDictionary.Items[c_FUNCTION] == null)
      //  return;

      //path = getCurrentLibDir();

      //LibraryName = c_LIBRARY_NAME;
      //File = path + c_FILE;
      //ClassName = c_CLASS_NAME;

      for (int k1 = 0; k1 < m_functionDictionary.Items.Count; k1++)
      {
        Item funcItem = m_functionDictionary.Items[k1];
        FunctionName = funcItem.ID;
        parameters = (TableHeader)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CTableHeader, "parameters", funcItem.Attributes["parameters"].Value, dictionary);
        if (funcItem.Attributes.Contains("RequiresAttributeListYN"))
          PassAttributes = Convert.ToBoolean(funcItem.Attributes["RequiresAttributeListYN"].Value.ToString());
        else
          PassAttributes = false;
        m_functionList.Add(FunctionName, new FunctionElement(File, ClassName, FunctionName, parameters, PassAttributes));
      }
    }

    public object LoadFunction(object attrValue, Item i)
    {
      object functionReturnValue = string.Empty;
      /*
       * This was coded a long time ago, and I did not have the generic functions for parsing available when it
       * was written. I have changed this to use the new functions, because it is more consistent, and because
       * the substitution with the pipe ("|") symbol was creating problems with the resulting data
       * (commas were being substituted for pipes even when they were not supposed to be)
       */
      //StringBuilder strValue = new StringBuilder(attrValue.ToString()); // full string: "functionname(parm1,parm2)"
      //StringBuilder sbFunctionName = new StringBuilder(strValue.Length);
      //sbFunctionName.Length = 0;                      // reset to empty just in case
      //int iSub = 0;
      //while (iSub < strValue.Length && strValue[iSub] != '(' )       // function name is token to the left of the Left Paren
      //  sbFunctionName.Append(strValue[iSub++]);
      //string functionString = attrValue.ToString().Substring(iSub);   // this string is the rest of the string: "(parm1, parm2, etc.)"
      //string functionName = sbFunctionName.ToString().ToLower();    // lowercase function name so it always matches
      
      //TODO:OUT Parameter! We need to review this!
      //string functionName = (string)(string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getFunctionName, (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, TAGFunctions.FUNCTIONCHAR.ToString() + attrValue), out functionString).ToLower();
      string functionString;
      string functionName = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getFunctionName, (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, TAGFunctions.FUNCTIONCHAR.ToString() + attrValue), out functionString)).ToLower();

      if (functionName.Length > 0)                    // was there actually a function name?
      {
        //string valueList = functionString.Replace(",", "|");      // replace commas with pipes (attributetable record delim)
        if (m_functionList.ContainsKey(functionName))           // is the function in the registered list?
        {
          FunctionElement myFunc = (FunctionElement)m_functionList[functionName];  // get an object for this report from the registered list
          string attributeList = string.Empty;
          //string[] delimiters = { "," };
          string[] functionParametersTable = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, functionString);
          //object[,] functionParametersTable = at.ToTable(valueList, myFunc.DataTypes); // take the inner parameter list from the func= and convert it to an array of objects
          int nbrArguments = functionParametersTable.GetLength(0);    // pick up the number of arguments in the list 
          if (myFunc.NumberParameters != nbrArguments)          // make sure the number of parameters match with the data dictionary definition
                                                                    // note that since the dictionary includes the return value, we have to subtract one
            return functionReturnValue;                 // if not, just return the default empty parameter
          nbrArguments = functionParametersTable.GetLength(0) + 2;  // if so, we need one more parameter
          object[] functionParameters = new object[nbrArguments];     // create the argument list (array of objects) required by method.Invoke
          for (int k1 = 0; k1 < functionParametersTable.GetLength(0); k1++)   // populate the arguments with the list in the func=
          {
            object o = functionParametersTable[k1];            // create the object for this argument 
            functionParameters[k1] = o;                  // now add this object to the argument list
          }
          /*
           * modified 8/27/2008 by LLA to pass the Item object containing the TAGAttribute
           * List directly, instead of optionally converting it to an AttributeTable
           * string and passing it in, only to convert again to an object[,] table.
           * 
           * This also means that ALL functions MUST have the 
           * Item attributeTable paramater as their LAST
           * parameter. It is not included in the parameters dictionary list.
           * 
           * It also means that the PassAttributes dictionary attribute is sunsetted
           * and is now ignored.
           */
          functionParameters[nbrArguments - 2] = i;
          int initialDepth = 0;
          functionParameters[nbrArguments - 1] = (object)initialDepth;

          // call the function
          try
          {
            functionReturnValue =
              attributeFunctions.GetType().GetMethod(functionName).Invoke(attributeFunctions,
                                                                          functionParameters);
          }
          catch (Exception ex)
          {
            if (TAGFunctions.BypassFunctionError)
              functionReturnValue = string.Empty;
            else
            {
              string msg;
              if (ex.InnerException == null)
                msg = ex.Message;
              else
                msg = ex.InnerException.Message;
              TAGExceptionMessage tException = new TAGExceptionMessage(className, functionName,
                "Error in LoadFunction: " + msg);
              for (int j = 0; j < functionParameters.GetLength(0); j++)
                tException.AddParm(functionParameters[j]);
              throw new Exception(tException.ToString());
            }
          }
          //Assembly assembly = Assembly.LoadFrom(myFunc.LibraryFile);
          //Type type = assembly.GetType(myFunc.ClassName, false);

          //if (type == null)
          //  throw new Exception("Class was not found in the library");
          //MethodInfo method;
          //try
          //{
          //  method = type.GetMethod(myFunc.FunctionName);
          //}
          //catch
          //{
          //  return functionReturnValue;     // for now, we just return an empty string. Once we have everything debugged, we should throw the exception below
          //  //throw new Exception("Function was not found in the library");
          //}
          //if (method == null)
          //{
          //  return functionReturnValue;     // for now, we just return an empty string. Once we have everything debugged, we should throw the exception below
          //  //throw new Exception("Function was not found in the library");
          //}
          //Object instance = null;
          //if (!method.IsStatic)
          //{
          //  instance = Activator.CreateInstance(type);
          //}
          //functionReturnValue = method.Invoke(instance, functionParameters);   //Call the function!!
        }
        else
        {
          TAGExceptionMessage tException = new TAGExceptionMessage(className, "LoadFunction",
                    "Function not found");
          tException.AddParm(functionString);
          throw new Exception(tException.ToString());
        }
      }
      else
      {
        TAGExceptionMessage tException = new TAGExceptionMessage(className, "LoadFunction",
                  "No function was specified");
        tException.AddParm(functionString);
        throw new Exception(tException.ToString());
      }

      return functionReturnValue;
    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <returns></returns>
    //public string getCurrentLibDir()
    //{
    //  return getCurrentPath() + @"\lib\";
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <returns></returns>
    //private string getCurrentPath()
    //{
    //  String AssemblyPath;
    //  AssemblyPath = global::System.IO.Path.GetDirectoryName(global::System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
    //  return AssemblyPath.Replace("file:\\", "");
    //}
  }
}