using System;
using System.Collections.Generic;
using System.Text;
using TAGBOSS.Common;
using TAGBOSS.Common.Model;

namespace TAGBOSS.Sys.AttributeEngine2.ConvertToEAC
{
  public class FunctionProcessor
  {
    /// <summary>
    /// Constructor, set the default 'constants' values
    /// </summary>
    public FunctionProcessor()
    {
    }

    /// <summary>
    /// Resolve functions in attributes for an item, returns the resolved item...
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public Item Output(Item item, DictionaryFunctions dictFunc)
    {
      if (dictFunc == null)
        return item;  //If there is no functionDictionary list, just return the item as it is!
      //Now we navigate through all the node attributes to see if we need to resolve a function!
      if (item.Attributes.Count > 0)
      {
        /*
         * We pass through all the attributes to process any func defined...
         */
        object funcValue = null;
        string funcToCall = "";
        string func_i_attrID = "";
        string func_i_attrVT = "";

        TAGAttribute func_i_attr = null;
        int getFunctions = 0;
        do
        {
          /*
           * while (getFunctions > -1)   //While we get functions from the function collection
          */
          func_i_attr = item.foreachValueType(TAGFunctions.FUNC, ref getFunctions);   //Get me the "next" function to process
          if (func_i_attr != null
            && func_i_attr.Value != null
            && !func_i_attr.IsNull)   //The included item is a valid one!
          {
            func_i_attrID = func_i_attr.ID;
            func_i_attrVT = func_i_attr.ValueType;
            if (func_i_attrVT != TAGFunctions.FUNC)  //We MUST check if this attribute is really part of this group valueType!!
            {
              func_i_attr.ValueType = TAGFunctions.FUNC;
              item.setValueType(func_i_attrID, func_i_attrVT);    //It is not part of this valueType group anymore! synchronize!
              getFunctions--;
              continue;
            }

            func_i_attr.ReferenceValueSource = TAGFunctions.FUNC + " = " + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, func_i_attr.Value); //Let's save the original value!
            func_i_attr.IsFunctionValue = true;
            
            //funcValue = mgr.LoadFunction(func_i_attr.Value, item);  //We have a function to process!!
            if (func_i_attr.Value != null)
            {
              funcToCall = (string)func_i_attr.Value;
              if (!funcToCall.StartsWith(TAGFunctions.ATTRIBUTECHAR.ToString()) && !funcToCall.StartsWith(TAGFunctions.FUNCTIONCHAR.ToString()))
                funcToCall = TAGFunctions.FUNCTIONCHAR.ToString() + funcToCall;
              try
              {
                funcValue = dictFunc.evaluateExpression(funcToCall, item, 0);  //We have a function to process!!
              }
              catch (Exception ex)
              {
                if (TAGFunctions.BypassFunctionError)
                  funcValue = null;
                else
                {
                  TAGExceptionMessage tException = new TAGExceptionMessage("FunctionProcessor", funcToCall,
                    "Error in LoadFunction: " + (Exception)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getInnerException, ex));
                  throw new Exception(tException.ToString());
                }
              }
            }
            else
              funcValue = null;

            if (funcValue == null)  // don't try to parse if the value is null or too short
            {
              item.setValueType(func_i_attr.ID, TAGFunctions.VALUE);
              func_i_attr.Value = null;
            }
            else
            {
              if (DictionaryFactory.getInstance().getDictionary().ExistsAttributeProperty(func_i_attr.OriginalID, TAGFunctions.TABLEHEADER))
                item.setValueType(func_i_attr.OriginalID, TAGFunctions.TABLEHEADER);
              else
                item.setValueType(func_i_attr.OriginalID, TAGFunctions.VALUE);

              func_i_attr.Value = funcValue;
            }
            getFunctions--; //Since we changed this attribute to VALUE ValueType, we must again look for this attribute position in the new collection
          }
        } while (getFunctions > -1);
      }

      return item;
    }

    /// <summary>
    /// Resolve functions in attributes for an item, returns the resolved item...
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public Item Output(Item item, DictionaryFunctionManager mgr)
    {
      if (mgr == null)
        return item;  //If there is no functionDictionary list, just return the item as it is!

      //Now we navigate through all the node attributes to see if we need to resolve a function!
      if (item.Attributes.Count > 0)
      {
        /*
         * We pass through all the attributes to process any func defined...
         */

        TAGAttribute func_i_attr = null;
        int getFunctions = 0;
        do
        {
          /*
           * while (getFunctions > -1)   //While we get functions from the function collection
          */
          func_i_attr = item.foreachValueType(TAGFunctions.FUNC, ref getFunctions);   //Get me the "next" function to process
          if (func_i_attr != null
            && func_i_attr.Value != null
            && !func_i_attr.IsNull)   //The included item is a valid one!
          {
            string func_i_attrID = func_i_attr.ID;
            string func_i_attrVT = func_i_attr.ValueType;
            if (func_i_attrVT != TAGFunctions.FUNC)  //We MUST check if this attribute is really part of this group valueType!!
            {
              func_i_attr.ValueType = TAGFunctions.FUNC;
              item.setValueType(func_i_attrID, func_i_attrVT);    //It is not part of this valueType group anymore! synchronize!
              getFunctions--;
              continue;
            }

            func_i_attr.ReferenceValueSource = TAGFunctions.FUNC + " = " + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, func_i_attr.Value); //Let's save the original value!
            func_i_attr.IsFunctionValue = true;
            object funcValue = mgr.LoadFunction(func_i_attr.Value, item);  //We have a function to process!!
            if (funcValue == null)  // don't try to parse if the value is null or too short
            {
              item.setValueType(func_i_attr.ID, TAGFunctions.VALUE);
              func_i_attr.Value = null;
            }
            else
            {
              if (DictionaryFactory.getInstance().getDictionary().ExistsAttributeProperty(func_i_attr.OriginalID, TAGFunctions.TABLEHEADER))
                item.setValueType(func_i_attr.OriginalID, TAGFunctions.TABLEHEADER);
              else
                item.setValueType(func_i_attr.OriginalID, TAGFunctions.VALUE);

              func_i_attr.Value = funcValue;
            }
            getFunctions--; //Since we changed this attribute to VALUE ValueType, we must again look for this attribute position in the new collection
          }
        } while (getFunctions > -1);
      }

      return item;
    }

  }
}
