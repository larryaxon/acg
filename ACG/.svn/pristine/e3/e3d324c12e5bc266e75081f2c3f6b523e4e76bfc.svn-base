using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

//TODO: We must get RID of this!!
using System.Globalization;

using TAGBOSS.Common.Model;
using TAGBOSS.Common.Logging;

namespace TAGBOSS.Common
{
  public class DictionaryFunctions
  {
    Log log = (Log)LogFactory.GetInstance().GetLog("TAGBOSS.Common.DictionaryFunctions");

    #region module data

    const string className = "DictionaryFunctions";
    //AttributeTable at = new AttributeTable();
    private int cMAXDEPTH = TAGFunctions.MaxDepthSelfCall;
    private char cFUNCTION = TAGFunctions.FUNCTIONCHAR;
    private string cATTRIBUTE = TAGFunctions.ATTRIBUTECHAR;
    private string cMINUS = TAGFunctions.MINUSCHAR;
    private char cLEFT = TAGFunctions.LEFTCHAR;
    private char cRIGHT = TAGFunctions.RIGHTCHAR;
    private string cLISTSEPARATOR = TAGFunctions.LISTSEPARATORCHAR;

    string[] cDELIMITERS = { "," };

    #endregion module data

    #region public entry points! evaluateExpression definition

    /// <summary>
    /// This is the central method of evaluating all parameters. It will take the expression, and recursively evaluate it until
    /// a "base" class (scalar or tableheader or something) is reached
    /// This uses the OLD object model Item
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    public object evaluateExpression(object expression, Item attributeTable, int depth)
    {
      object returnValue = null;

      try 
      {
        #region initialize and prepare to evaluate
        bool isMinus = false;
        if (expression == null)
          return string.Empty;

        Type t = expression.GetType();
        if (t != typeof(string) && t != typeof(String))
          return expression;

        returnValue = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, expression); // added by larry to implement comnments

        string strExpression = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, returnValue);
        if (strExpression == string.Empty)  // expression is empty?
          return strExpression;             // then just return it - don't try to process
        /* 
         * we need to check if it is already in number list format (for sums, contacts, etc)
         * or if we need to evaluate. Since we are evaluating outside in, if it is a number
         * list format, we leave it alone, and wait until we may need to evaluate one of the
         * component parts later.
         */
        if (strExpression.StartsWith(cLEFT.ToString()) && // is it enclosed in parens?
          strExpression.EndsWith(cRIGHT.ToString()))
        {
          strExpression = strExpression.Substring(1, strExpression.Length - 2);
          string[] delimiters = { cLISTSEPARATOR };   // yes, so let's try to parse it as a number string
          string[] numberArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, strExpression, delimiters);
          if (numberArray.GetLength(0) > 0) // yep, it is a list
            return cLEFT.ToString()
              + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.ToTableList, numberArray, true)
              + cRIGHT.ToString();  // so just return it converted back to a list. We do this so that trim() and comments have been stripped out'
        }

        /*
         * we stay in this loop, resolving attribute names or functions inside out, until
         * we get to the most outside resolution and there is nothing left but a scalar value (see logic at the top of this routine)
         * or we have exceeded the max depth. This is not recursion, but still has a maximum
         * depth, in case we have somehow created a circular reference. 
         */
        bool isFinished = false;
        #endregion init
        // Keep processing, outside in, until all is resolved or we have exceeded maxdepth
        while (!isFinished)
        {
          #region Process expression
          string insideExpression = "";
          if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.beginsWith, returnValue, TAGFunctions.LITERALCHAR))
          {
            // go ahead and process comment, but don't trim the results
            returnValue = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, returnValue)).Substring(1);
            isFinished = true;
          }
          else
          {
            returnValue = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, returnValue)).Trim();
            /*
             * Now check for a minus sign...
             */
            if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.beginsWith, returnValue, cMINUS)) // if it starts with a minus sign...
            {
              #region Process minus sign
              strExpression = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, returnValue)).Substring(1); // then strip it off so the rest of the stuff works
              returnValue = (object)strExpression;
              isMinus = true;                            // and remember so we can put it back at the end
              #endregion
            }
            strExpression = returnValue.ToString();

            if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.isAttributeReference, strExpression))
            {
              /* 
               * we check to see if this starts with @@. if so, it is an orphaned paramater substitution, so we replace it with an empthy string
               */
              if (strExpression.StartsWith(TAGFunctions.ATTR_2_AT_CHAR))
              {
                if (strExpression.Equals("@@Item", StringComparison.CurrentCultureIgnoreCase))  // special case: we KNOW how to resolve this
                  returnValue = attributeTable.OriginalID;
                else
                  returnValue = "";
              }
              else
              {
                // if not, we process the attribute substitution
                #region Process attribute resolve
                // if the expression starts with cATTRIBUTE, then the rest of the expression contains an attribute name
                returnValue = attributeValue(attributeTable, strExpression.ToString().Substring(1), depth);
                #endregion
              }
            }
            else
            {
              if (!((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.isFunction, strExpression)))
                isFinished = true;
              else
                returnValue = evaluateFunction(strExpression, attributeTable, depth, out isFinished);
            }
          }

          if (depth > cMAXDEPTH)
          {
            TAGExceptionMessage tException = new TAGExceptionMessage(className, "evaluateExpression", "Recursion exceede max depth");
            tException.AddParm(expression);
            tException.AddParm(attributeTable);
            throw new Exception(tException.ToString());
          }
          if (isMinus)  // if there was a minus sign, we saved it at the beginning, now we process it
          {
            returnValue = processMinus(returnValue, out t);
            isMinus = false;
          }
          else
          {
            if (returnValue == null)
              t = null;
            else
              t = returnValue.GetType();
          }
          // now check to see if we have returned an "atomic" value (null, string.empty, or non-string). If so, we exit the loop.
          if (!isFinished)
            isFinished = isAtomic(returnValue);
          #endregion Process expression
        } // end while (!isFinished)
      }
      catch (Exception ex)
      {
        string errDetails = "";
        errDetails = "Expression: " + (expression == null ? "(empty)" : expression.ToString());
        errDetails += "; ItemId: " + (attributeTable == null ? "(empty)" : attributeTable.OriginalID);
        errDetails += "; Inherited: " + (attributeTable != null && attributeTable.IsInherited ? "TRUE" : "FALSE");

        log.Error("TCDF002: Expression evaluation was unsuccesful: " + ex.Message);
        log.Error("TCDF002: " + errDetails);
        if (log.IsDebugEnabled)
        {
          StackTrace st = new StackTrace(true);
          log.Debug("TCDF002: ", ex);
          log.Debug("TCDF002: " + st.ToString());
        }
        throw ex;
      }

      return returnValue;
    }

    /// <summary>
    /// This is the central method of evaluating all parameters. It will take the expression, and recursively evaluate it until
    /// a "base" class (scalar or tableheader or something) is reached
    /// This uses the NEW object model TItem
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    public object evaluateExpression(object expression, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      object returnValue = null;
     
      try 
      {
        //Exceeded max depth calls?? 
        if (depth > cMAXDEPTH)
        {
          TAGExceptionMessage tException = new TAGExceptionMessage(className, "evaluateExpression", "Recursion exceeded max depth");
          tException.AddParm(expression);
          tException.AddParm(attributeTable);
          throw new Exception(tException.ToString());
        }

        #region initialize and prepare to evaluate
        bool isMinus = false;
        if (expression == null)
          return string.Empty;

        Type t = expression.GetType();
        if (t != typeof(string) && t != typeof(String))
          return expression;

        returnValue = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, expression); // added by larry to implement comnments

        string strExpression = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, returnValue);
        if (strExpression == string.Empty)  // expression is empty?
          return strExpression;             // then just return it - don't try to process

        TParameterProcessor tPProc = new TParameterProcessor();  //For resolving @@ that are NOT references we need this getFunction method in this class

        /* 
         * we need to check if it is already in number list format (for sums, contacts, etc)
         * or if we need to evaluate. Since we are evaluating outside in, if it is a number
         * list format, we leave it alone, and wait until we may need to evaluate one of the
         * component parts later.
         */
        if (strExpression.StartsWith(cLEFT.ToString()) && // is it enclosed in parens?
          strExpression.EndsWith(cRIGHT.ToString()))
        {
          strExpression = strExpression.Substring(1, strExpression.Length - 2);
          string[] delimiters = { cLISTSEPARATOR };   // yes, so let's try to parse it as a number string
          string[] numberArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, strExpression, delimiters);
          if (numberArray.GetLength(0) > 0) // yep, it is a list
            return cLEFT.ToString() + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.ToTableList, numberArray, true) + cRIGHT.ToString();  // so just return it converted back to a list. We do this so that trim() and comments have been stripped out'
        }

        /*
         * we stay in this loop, resolving attribute names or functions inside out, until
         * we get to the most outside resolution and there is nothing left but a scalar value (see logic at the top of this routine)
         * or we have exceeded the max depth. This is not recursion, but still has a maximum
         * depth, in case we have somehow created a circular reference. 
         */
        bool isFinished = false;
        #endregion init
        // Keep processing, outside in, until all is resolved or we have exceeded maxdepth
        while (!isFinished)
        {
          #region Process expression
          string insideExpression = "";
          if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.beginsWith, returnValue, TAGFunctions.LITERALCHAR))
          {
            // go ahead and process comment, but don't trim the results
            returnValue = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, returnValue)).Substring(1);
            isFinished = true;
          }
          else
          {
            returnValue = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, returnValue)).Trim();
            /*
             * Now check for a minus sign...
             */
            if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.beginsWith, returnValue, cMINUS)) // if it starts with a minus sign...
            {
              #region Process minus sign
              strExpression = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, returnValue)).Substring(1); // then strip it off so the rest of the stuff works
              returnValue = (object)strExpression;
              isMinus = true;                            // and remember so we can put it back at the end
              #endregion
            }
            strExpression = returnValue.ToString();

            if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.isAttributeReference, strExpression))
            {
              /* 
               * we check to see if this starts with @@. if so, it is an orphaned paramater substitution, so we replace it with an empthy string
               */
              if (strExpression.StartsWith(TAGFunctions.ATTR_2_AT_CHAR))
              {
                if (strExpression.Contains("."))
                {
                  //If it contains a dot MUST probably is a REFERENCE we deal with it as one...
                  TAttribute returnValueAttr = getRefAttribute(strExpression, attributeTable, eObj);
                  if (returnValueAttr != null && returnValueAttr.Value != null)
                    returnValue = returnValueAttr.Value;
                  else
                    returnValue = "";
                }
                else
                {
                  returnValue = tPProc.getSolvedParameter(strExpression, eObj, attributeTable, aObj);
                }
              }
              else
              {
                // if not, we process the attribute substitution
                #region Process attribute resolve
                // if the expression starts with cATTRIBUTE, then the rest of the expression contains an attribute name
                returnValue = attributeValue(attributeTable, eObj, strExpression.ToString().Substring(1), depth);
                #endregion
              }
            }
            else
            {
              if (!((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.isFunction, strExpression)))
                isFinished = true;
              else
                returnValue = evaluateFunction(strExpression, aObj, attributeTable, eObj, depth, out isFinished);
            }
          }

          //if (depth > cMAXDEPTH)
          //{
          //  TAGExceptionMessage tException = new TAGExceptionMessage(className, "evaluateExpression", "Recursion exceede max depth");
          //  tException.AddParm(expression);
          //  tException.AddParm(attributeTable);
          //  throw new Exception(tException.ToString());
          //}
          if (isMinus)  // if there was a minus sign, we saved it at the beginning, now we process it
          {
            returnValue = processMinus(returnValue, out t);
            isMinus = false;
          }
          else
          {
            if (returnValue == null)
              t = null;
            else
              t = returnValue.GetType();
          }
          // now check to see if we have returned an "atomic" value (null, string.empty, or non-string). If so, we exit the loop.
          if (!isFinished)
            isFinished = isAtomic(returnValue);
          #endregion Process expression
        } // end while (!isFinished)
      }
      catch (Exception ex)
      {
        string errDetails = "";
        errDetails = "Expression: " + (expression == null ? "(empty)" : expression.ToString());
        errDetails += "; Entity" + (eObj == null ? "(empty)" : eObj.ToString());
        errDetails += "; Item" + (attributeTable == null ? "(empty)" : attributeTable.ToString());
        errDetails += "; Attribute" + (aObj == null ? "(empty)" : aObj.ToString());
        errDetails += "; Value: " + (aObj == null || aObj.ValueHistory == null ? "(empty)" : aObj.ValueHistory.ToString());

        log.Error("TCDF001: Expression evaluation was unsuccesful: " + ex.Message);
        log.Error("TCDF001: " + errDetails);

        if (log.IsDebugEnabled)
        {
          StackTrace st = new StackTrace(true);
          log.Debug("TCDF001: ", ex);
          log.Debug("TCDF001: " + st.ToString());
        }

        throw ex;
      }

      return returnValue;
    }

    #endregion public entry points! evaluateExpression definition

    #region Original TAGBOSS overload functions!

    #region  private evaluateExpresion (func) functions
    /// <summary>
    /// Absolute value. Format is abs(operand)
    /// </summary>
    /// <param name="operand">Numeric value</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  abs(object operand, Item attributeTable, int depth)
    {
      try
      {
        decimal dResult;
        object op = evaluateExpression(operand, attributeTable, depth);
        if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsNumeric, op))
          dResult = Math.Abs((decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, op));
        else
        {
          throw new Exception("ABS requires a numeric argument");
        }
        return (object)dResult;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "abs", e.Message);
        t.AddParm(operand);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    /// <summary>
    /// Add two values. Format is add(operand1, operand2)
    /// </summary>
    /// <param name="operand1">Numeric value</param>
    /// <param name="operand2">Numeric value</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  add(object operand1, object operand2, Item attributeTable, int depth)
    {
      try
      {
        decimal op1 = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(operand1, attributeTable, depth), 0);
        decimal op2 = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(operand2, attributeTable, depth), 0);
        return op1 + op2;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "add", e.Message);
        t.AddParm(operand1);
        t.AddParm(operand2);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    /// <summary>
    /// Calculate age based on birthday and effective date. Format is age(birthdate, effectivedate)
    /// </summary>
    /// <param name="birthDate"></param>
    /// <param name="effectiveDate"></param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  age(object birthDate, object effectiveDate, Item attributeTable, int depth)
    {
      try
      {
        DateTime eDate, bDate;
        object oBirthDate = evaluateExpression(birthDate, attributeTable, depth);
        object oEffectiveDate = evaluateExpression(effectiveDate, attributeTable, depth);
        eDate = Convert.ToDateTime(oEffectiveDate);
        bDate = Convert.ToDateTime(oBirthDate);
        return (object)Convert.ToInt32(DateDiff(bDate, eDate, "y", 0));
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "age", e.Message);
        t.AddParm(birthDate);
        t.AddParm(effectiveDate);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    /// <summary>
    /// Boolean function. Returns true if both condition1 and condition2 are true. format is and(condition1, condition2)
    /// </summary>
    /// <param name="conditionList"></param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  and(object conditionList, Item attributeTable, int depth)
    {
      try
      {
        // 5/17/2010 LLA: change so other conditions are not eval'd unless necessary
        string sConditionList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(conditionList, attributeTable, depth));
        sConditionList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, sConditionList);
        string[] delimiters = { cLISTSEPARATOR };
        string[] conditionArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, sConditionList, delimiters);
        for (int i = 0; i < conditionArray.GetLength(0); i++)
        {
          if (!(bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, ifCondition(conditionArray[i], attributeTable, depth))) // if any condition is false
            return (object)false; // then we return false
        }
        return (object)true;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "and", e.Message);
        t.AddParm(conditionList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  attributedate(object attributeName, object dateName, object asOfDate, Item attributeTable, int depth)
    {
      try
      {
        string sAttribute = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(attributeName, attributeTable, depth));
        string sDate = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(dateName, attributeTable, depth))).ToLower();
        DateTime efDate;
        if (asOfDate == null || asOfDate.Equals(string.Empty))
          efDate = attributeTable.EffectiveDate;
        else
          efDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, evaluateExpression(asOfDate, attributeTable, depth));
        if (attributeTable.Attributes.Contains(sAttribute))
        {
          // we want the start or end date as of the effective date
          TAGAttribute a = attributeTable.Attributes[sAttribute];
          if (efDate != a.EffectiveDate)
            a = (TAGAttribute)a.Clone();
          a.EffectiveDate = efDate;
          switch (sDate)
          {
            case "startdate":
            case "start":
              return a.StartDate;
            case "enddate":
            case "end":
              return a.EndDate;
            default:
              TAGExceptionMessage t = new TAGExceptionMessage(className, "attributedate", "Date Requested must be Start or End");
              t.AddParm(attributeName);
              t.AddParm(dateName);
              t.AddParm(asOfDate);
              t.AddParm(attributeTable);
              throw new Exception(t.ToString());
          }
        }
        else
          return string.Empty;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "attributedate", e.Message);
        t.AddParm(attributeName);
        t.AddParm(dateName);
        t.AddParm(asOfDate);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    /// <summary>
    /// Boolean function. returns true of testvalue is between fromvalue and tovalue. this is for string values.
    /// Format is between(testvalue, fromvalue, tovalue)
    /// </summary>
    /// <param name="testValue"></param>
    /// <param name="fromValue"></param>
    /// <param name="toValue"></param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  between(object testValue, object fromValue, object toValue, Item attributeTable, int depth)
    {
      try
      {
        return (object)Between(testValue, fromValue, toValue, TAGFunctions.DATATYPESTRING, attributeTable, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "between", e.Message);
        t.AddParm(testValue);
        t.AddParm(fromValue);
        t.AddParm(toValue);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    /// <summary>
    /// same as between, except for date values
    /// </summary>
    /// <param name="testValue"></param>
    /// <param name="fromValue"></param>
    /// <param name="toValue"></param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  betweendate(object testValue, object fromValue, object toValue, Item attributeTable, int depth)
    {
      try
      {
        return (object)Between(testValue, fromValue, toValue, TAGFunctions.DATATYPEDATETIME, attributeTable, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "betweendate", e.Message);
        t.AddParm(testValue);
        t.AddParm(fromValue);
        t.AddParm(toValue);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    /// <summary>
    /// same as between, except for numeric values
    /// </summary>
    /// <param name="testValue"></param>
    /// <param name="fromValue"></param>
    /// <param name="toValue"></param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  betweennumber(object testValue, object fromValue, object toValue, Item attributeTable, int depth)
    {
      try
      {
        return (object)Between(testValue, fromValue, toValue, TAGFunctions.DATATYPEDECIMAL, attributeTable, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "betweennumber", e.Message);
        t.AddParm(testValue);
        t.AddParm(fromValue);
        t.AddParm(toValue);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    /// <summary>
    /// Implementation of Jack's calculateDate function
    /// </summary>
    /// <param name="dateIn">Date to start with</param>
    /// <param name="modification">Modification strings (see TAGFunction.CalculateDate for details)</param>
    /// <param name="afterToday">Force the resulting date to be on or after today</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  calculatedate(object dateIn, object modification, object afterToday, Item attributeTable, int depth)
    {
      object oDate = evaluateExpression(dateIn, attributeTable, depth);
      if (!(bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsDateTime, oDate))
      {
        TAGExceptionMessage tm = new TAGExceptionMessage(className, "CalculateDate", "DateIn is not a valid date");
        tm.AddParm(dateIn);
        tm.AddParm(modification);
        tm.AddParm(afterToday);
        throw new Exception(tm.ToString());
      }
      DateTime dtDateIn = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, oDate);
      string sMods = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(modification, attributeTable, depth));
      object oAfterToday = evaluateExpression(afterToday, attributeTable, depth);
      if (!(bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsBoolean, oAfterToday))
      {
        TAGExceptionMessage tm = new TAGExceptionMessage(className, "CalculateDate", "AfterToday is not a valid Boolean");
        tm.AddParm(dateIn);
        tm.AddParm(modification);
        tm.AddParm(afterToday);
        throw new Exception(tm.ToString());
      }
      bool bAfterToday = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, oAfterToday);
      DateTime returnDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CalculateDate, dtDateIn, sMods, bAfterToday);
      if (returnDate == TAGFunctions.FutureDateTime)
        return string.Empty;
      else
        return returnDate;
    }
    private object  cap(object priorAmount, object currentAmount, object targetAmount, object mode, Item attributeTable, int depth)
    {
      try
      {
        decimal dCurrent = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(currentAmount, attributeTable, depth));
        object oTarget = evaluateExpression(targetAmount, attributeTable, depth);
        if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, isnull(oTarget, attributeTable, depth)) || (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, oTarget) == 0) //The zero check is temporary until Larry fixed the TDs to not evaluate to 0
          return dCurrent;
        decimal dPrior = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(priorAmount, attributeTable, depth));
        decimal dTarget = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, oTarget);
        string sMode = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(mode, attributeTable, depth))).ToLower();
        if (sMode == "max" || sMode == "max0")
        {
          if (dTarget > 0)
          {
            if (dTarget - dPrior < dCurrent)
              if (dTarget - dPrior < 0 && sMode == "max0")
                return 0;
              else
                return dTarget - dPrior;
            else
              return dCurrent;
          }
          else
          {
            if (dTarget - dPrior > dCurrent)
              if (dTarget - dPrior > 0 && sMode == "max0")
                return 0;
              else
                return dTarget - dPrior;
            else
              return dCurrent;
          }
        }
        else // min
        {
          if (dTarget > 0)
          {
            if (dTarget - dPrior > dCurrent)
              return dTarget - dPrior;
            else
              return dCurrent;
          }
          else
          {
            if (dTarget - dPrior < dCurrent)
              return dTarget - dPrior;
            else
              return dCurrent;
          }
        }
      }
      catch (Exception ex)
      {
        TAGExceptionMessage tm = new TAGExceptionMessage(className, "cap", ex.Message);
        tm.AddParm(priorAmount);
        tm.AddParm(currentAmount);
        tm.AddParm(targetAmount);
        tm.AddParm(mode);
        throw new Exception(tm.ToString());
      }
    }
    /// <summary>
    /// string functions takes the list format (string1~string1~string3...) and returns the concatination of all of the strings in the list.
    /// Format is concatenate((string1~string2~...)
    /// </summary>
    /// <param name="pStringList">List format set of strings (string1~string1~string3...)</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>

    private object  concatenate(object pStringList, Item attributeTable, int depth)
    {
      try
      {
        string returnValue = string.Empty;
        object oList = evaluateExpression(pStringList, attributeTable, depth);
        string stringList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, oList);
        /*
         * first, we strip off parenthesis on the outside if they exist
         */
        stringList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, stringList);
        string[] delimiters = { cLISTSEPARATOR };
        string[] stringArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, stringList, delimiters);
        for (int i = 0; i < stringArray.GetLength(0); i++)
        {
          object myToken = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(stringArray[i], attributeTable, depth));
          returnValue = returnValue + myToken;
        }
        return (object)returnValue;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "concatenate", e.Message);
        t.AddParm(pStringList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    /// <summary>
    /// boolean function which compares testvalue with each entry in the list. Returns true if the test value is the same as any of the values in the list.
    /// Format is containslist(testvalue, (value1~value2~value3...))
    /// </summary>
    /// <param name="testValue"></param>
    /// <param name="pStringList">List in the format (value1~value2~value3...)</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  containslist(object testValue, object pStringList, Item attributeTable, int depth)
    {
      try
      {
        bool inList = false;
        object oList = evaluateExpression(pStringList, attributeTable, depth);
        object oValue = evaluateExpression(testValue, attributeTable, depth);
        if (oValue.GetType().Name.ToLower() == TAGFunctions.DATATYPESTRING)
          oValue = oValue.ToString().Trim();
        string stringList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, oList));
        string[] delimiters = { cLISTSEPARATOR };
        string[] stringArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, stringList, delimiters);
        for (int i = 0; i < stringArray.GetLength(0); i++)
        {
          object myToken = evaluateExpression(stringArray[i], attributeTable, depth);
          if (myToken.GetType().Name.ToLower() == TAGFunctions.DATATYPESTRING)
            myToken = myToken.ToString().Trim();
          if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.passesTest, myToken, TAGFunctions.EQCHAR, oValue))
          {
            inList = true;
            break;
          }
        }
        return (object)inList;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "containslist", e.Message);
        t.AddParm(testValue);
        t.AddParm(pStringList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    /// <summary>
    /// boolean function which compares testvalue with each entry in the first column of the table. 
    /// Returns true if the test value is the same as any of the values in the first column of the table.
    /// Format is containslist(testvalue, (value1~field1|value2~field2|value3~field3|...))    /// </summary>
    /// <param name="testValue"></param>
    /// <param name="tableList">Attribute table with unique values in column 1</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  containstable(object testValue, object tableList, Item attributeTable, int depth)
    {
      try
      {
        bool inList = false;
        TableHeader oTable = (TableHeader)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CTableHeader, evaluateExpression(tableList, attributeTable, depth));
        if (oTable == null || oTable.GetLength(0) == 0)
          return (object)inList;
        object oValue = evaluateExpression(testValue, attributeTable, depth);
        if (oValue.GetType().Name.ToLower() == TAGFunctions.DATATYPESTRING)
          oValue = oValue.ToString().Trim();
        for (int i = 0; i < oTable.GetLength(0); i++)
        {
          object myToken = evaluateExpression(oTable[i, 0], attributeTable, depth);
          if (myToken.GetType().Name.ToLower() == TAGFunctions.DATATYPESTRING)
            myToken = myToken.ToString().Trim();
          if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.passesTest, myToken, TAGFunctions.EQCHAR, oValue))
          {
            inList = true;
            break;
          }
        }
        return (object)inList;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "containstable", e.Message);
        t.AddParm(testValue);
        t.AddParm(tableList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    /// <summary>
    /// format is coalesce((value1~value2~value3...))
    /// accepts a list of values, and returns the first one in the list that is not empty.
    /// </summary>
    /// <param name="pStringList">List in the format (value1~value2~value3...)</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  coalesce(object pStringList, Item attributeTable, int depth)
    {
      try
      {
        /*
         * find the first nonempty string in the list and return an evaluated version
         */
        string stringList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(pStringList, attributeTable, depth));
        /*
         * first, we strip off parenthesis on the outside if they exist
         */
        stringList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, stringList);
        string[] delimiters = { cLISTSEPARATOR };
        string[] stringArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, stringList, delimiters);
        for (int i = 0; i < stringArray.GetLength(0); i++)
        {
          if (stringArray[i] != null && stringArray[i] != string.Empty)
          {
            string myToken = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(stringArray[i], attributeTable, depth));
            if (myToken != string.Empty)
              return (object)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, myToken);
          }
        }
        return (object)string.Empty;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "coalesce", e.Message);
        t.AddParm(pStringList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    /// <summary>
    /// Accepts an attributetable and converts the first column (normaly key values) into a list format (key1~key2~...)
    /// format is converttolist((value1~field1|value2~field2|value3~field3|...))
    /// </summary>
    /// <param name="pTable"></param>
    /// <param name="attributeTable">Attribute table in the format (value1~field1|value2~field2|value3~field3|...)</param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  converttolist(object pTable, Item attributeTable, int depth)
    {
      try
      {
        string returnList = TAGFunctions.LEFTCHAR.ToString();
        TableHeader oTable = (TableHeader)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CTableHeader, evaluateExpression(pTable, attributeTable, depth));
        int nbrRows = oTable.GetLength(0);
        bool firstOne = true;
        for (int i = 0; i < nbrRows; i++)
        {
          if (firstOne)
            firstOne = false;
          else
            returnList += TAGFunctions.COLSEPARATORCHAR;
          returnList += (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, oTable[i, 0]);
        }
        returnList += TAGFunctions.RIGHTCHAR;
        return (object)returnList;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "converttolist", e.Message);
        t.AddParm(pTable);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    /// <summary>
    /// Add addDays days to fromDate and return the result
    /// </summary>
    /// <param name="fromDate">datetime to add the days to</param>
    /// <param name="addDays">Number of days to add</param>
    /// <param name="attributeTable">Attribute table in the format (value1~field1|value2~field2|value3~field3|...)</param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  dateadd(object fromDate, object addDays, Item attributeTable, int depth)
    {
      object oDate = evaluateExpression(fromDate, attributeTable, depth);
      if (!(bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsDateTime, oDate))
      {
        TAGExceptionMessage tm = new TAGExceptionMessage(className, "DateAdd", "FromDate is not a valid date");
        tm.AddParm(fromDate);
        tm.AddParm(addDays);
        throw new Exception(tm.ToString());
      }
      DateTime dtFromDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, oDate);
      object oAddDays = (double)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDouble, evaluateExpression(addDays, attributeTable, depth));
      if (!(bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsNumeric, oAddDays))
      {
        TAGExceptionMessage tm = new TAGExceptionMessage(className, "DateAdd", "AddDays is not a valid number");
        tm.AddParm(fromDate);
        tm.AddParm(addDays);
        throw new Exception(tm.ToString());
      }
      double dblAddDays = (double)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDouble, oAddDays);
      return dtFromDate.AddDays(dblAddDays);
    }
    /// <summary>
    /// calculates the difference in dateType units between startDate and endDate. 
    /// Format is datediff(dateType,startdate,endDate,round)
    /// </summary>
    /// <param name="dateType">DateType is one of "y","m","q","w","d" which is Year,Month,Quarter,Week, or Day</param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="round">Orecision of rounded result. E.g., round 2 will round to nearest 10 units.</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  datediff(object dateType, object startDate, object endDate, object round, Item attributeTable, int depth)
    {
      try
      {
        DateTime sDate, eDate;
        string dType;
        int rnd;
        sDate = Convert.ToDateTime(evaluateExpression(startDate, attributeTable, depth));
        eDate = Convert.ToDateTime(evaluateExpression(endDate, attributeTable, depth));
        dType = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(dateType, attributeTable, depth));
        rnd = Convert.ToInt32(evaluateExpression(round, attributeTable, depth));
        return DateDiff(sDate, eDate, dType, rnd);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "datediff", e.Message);
        t.AddParm(dateType);
        t.AddParm(startDate);
        t.AddParm(endDate);
        t.AddParm(round);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    /// <summary>
    /// Returns 'Monday', 'Tuesday', etc as the day of week of the date that was passed in
    /// </summary>
    /// <param name="dateIn"></param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  dayofweek(object dateIn, Item attributeTable, int depth)
    {
      DateTime thisDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, evaluateExpression(dateIn, attributeTable, depth));
      string dayOfWeek = thisDate.DayOfWeek.ToString();
      return dayOfWeek;
    }
    private object  distinctvalue(object table, object columnname, Item attributeTable, int depth)
    {
      string column = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(columnname, attributeTable, depth));
      TableHeader th = (TableHeader)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CTableHeader, evaluateExpression(table, attributeTable, depth));
      return th.DistinctValue(column);
    }
    /// <summary>
    /// Divide value1 by value2.
    /// </summary>
    /// <param name="Value1">A numeric value</param>
    /// <param name="Value2">A non-zero numeric value</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  divide(object Value1, object Value2, Item attributeTable, int depth)
    {
      try
      {
        return calculate(Value1, Value2, attributeTable, depth, "divide");
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "divide", e.Message);
        t.AddParm(Value1);
        t.AddParm(Value2);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    /// <summary>
    /// Calculates the Employee Months of Service starting in EmployeeStartDate ending on effectiveDate
    /// </summary>
    /// <param name="EmployeeStartDate"></param>
    /// <param name="effectiveDate"></param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  employeemos(object EmployeeStartDate, object effectiveDate, Item attributeTable, int depth)
    {
      try
      {
        DateTime eDate, startDate;
        eDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, evaluateExpression(effectiveDate, attributeTable, depth));
        startDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, evaluateExpression(EmployeeStartDate, attributeTable, depth));
        return (object)Convert.ToInt32(Math.Abs(Convert.ToInt32(DateDiff(eDate, startDate, "m", 0))));
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "employeemos", e.Message);
        t.AddParm(EmployeeStartDate);
        t.AddParm(effectiveDate);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    /// <summary>
    /// Is this date the last event in a period?
    /// </summary>
    /// <param name="eventDate">Date to Test</param>
    /// <param name="numberofDaysInEvent"></param>
    /// <param name="period">"m", "q", or "y" (Month, Quarter, Year)</param>
    /// <param name="businessDayOnlyYN">Does the event have to be on a business day? (default = true)</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  endofperiodyn(object eventDate, object numberofDaysInPayroll, object period, object businessDayOnlyYN, Item attributeTable, int depth)
    {
      // first we evaluate the parms
      DateTime dtEventDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, evaluateExpression(eventDate, attributeTable, depth));
      double dNbrDays = (double)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDouble, evaluateExpression(numberofDaysInPayroll, attributeTable, depth));
      if (dNbrDays == 0)
        dNbrDays = 1;

      string sPeriod = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(period, attributeTable, depth))).ToLower();
      bool bBusinessOnly = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, evaluateExpression(businessDayOnlyYN, attributeTable, depth), true);
      // now we use calulateDate to find the next event date
      string mod = "d" + dNbrDays.ToString();
      //if (bBusinessOnly)
      //  mod = "b" + dNbrDays.ToString();
      DateTime dtTest = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, calculatedate(dtEventDate, mod, false, attributeTable, depth));
      // finally, if the next event is in the same period, this is not the end of period. If it is not, then it is
      bool endOfPeriod = ((int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getPeriod, dtEventDate, sPeriod) != (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getPeriod, dtTest, sPeriod));
      return endOfPeriod;
    }
    private object  exp(object Value1, object Value2, Item attributeTable, int depth)
    {
      try
      {
        return calculate(Value1, Value2, attributeTable, depth, "exponent");
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "exp", e.Message);
        t.AddParm(Value1);
        t.AddParm(Value2);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  format(object type, object Value, object formatString, Item attributeTable, int depth)
    {
      try
      {
        string sType = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(type, attributeTable, depth))).ToLower();
        object oValue = evaluateExpression(Value, attributeTable, depth);
        string sFormat = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(formatString, attributeTable, depth))).ToLower();
        switch (sType)
        {
          case "datetime":
            switch (sFormat)
            {
              case "shortdate":
                return (object)((DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, oValue)).ToShortDateString();
              case "unformatted":
                return (object)(((DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, oValue)).ToString("yyMMdd"));
              default:
                return string.Empty;
            }

          default:
            return string.Empty;
        }
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "format", e.Message);
        t.AddParm(type);
        t.AddParm(Value);
        t.AddParm(formatString);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  iif(object condition, object valueTrue, object valueElse, Item attributeTable, int depth)
    {
      /*
       * This routine mimics the excel If function. It has the following parameters:
       * 
       * Condition:
       *  it parses two kinds of Conditions...
       * 
       *  _Operators: These are intrinsic functions which have two or three parameters, and
       *  whose names always begin with _.
       * 
       *    _if(condition, valueTrue, valueFalse). Recursively calls this function.
       *    _and(condition1, condition2)           Logical AND of both conditions
       *    _or(condition1, condition2)            Logical OR of both conditions
       *    _not(condition)                        Boolean opposite of condition
       *  
       *  AttributeValidationStrings: These are strings that would be valid for the Dictionary
       *  ValidationString, except that they are evaluated as pass instead of fail, and they pull 
       *  their values from an AttributeTable structure of  AttributeName/Value pairs. So any operand
       *  can be a scalar or an attribute name.
       *  
       * TAGAttribute Values can also be the @If function, which allows nesting of conditions
       * 
       *  the attributeList string is an Item formated structure of 
       *  attribute name/value pairs
       */
      try
      {
        return If(condition, valueTrue, valueElse, attributeTable, depth);          // and pass that to the method that actually contains the logic
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "iif", e.Message);
        t.AddParm(condition);
        t.AddParm(valueTrue);
        t.AddParm(valueElse);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  isnull(object variable, Item attributeTable, int depth)
    {
      if (variable == null)
        return (object)true;
      try
      {
        bool checkNullFlag = false;
        string sAttributeName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, variable);
        if (sAttributeName.StartsWith(TAGFunctions.ATTRIBUTECHAR)) // is this an @Attribute reference?
        {
          checkNullFlag = true;
          sAttributeName = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, sAttributeName))).Substring(1);  // now get the attribute name
          // if this is in the form @AttributeName, then we return IsNull = true if it is not there
          // This makes this function useful to determine if an attribute is missing without throwing an error
          if (!attributeTable.Contains(sAttributeName))
            return true;
        }
        // Note, if we are here, then either it is @Atribute, or it is a funciton. If a function, it will NOT
        // tell you if one of the attributes inside is missing. It will fail in that event with a function error
        object oVariable = evaluateExpression(variable, attributeTable, depth);
        if (oVariable == null || oVariable.ToString() == string.Empty)
          return true;
        else
        {
          if (checkNullFlag)  // this was an @Attribute reference, so we check the attribute referenced to see if it is null (useful for strongly typed attributes of non-string types)
          {
            if (attributeTable.Contains(sAttributeName))
              return attributeTable[sAttributeName].IsNull;
          }
          return false;
        }
      }
      catch (Exception e)
      {
        if (e.Message.Contains("Attribute lookup did not find attribute")) // if the error was an attribute was not found
          return true;          // then we say the variable is null
        else
        {
          TAGExceptionMessage t = new TAGExceptionMessage(className, "isnull", e.Message);
          t.AddParm(variable);
          t.AddParm(attributeTable);
          throw new Exception(e.Message);   // otherwise, we rethrow the message
        }
      }
    }
    private object  ifnull(object variable, object defaultValue, Item attributeTable, int depth)
    {
      try
      {
        bool isNull = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, isnull(variable, attributeTable, depth));
        if (isNull)
          return evaluateExpression(defaultValue, attributeTable, depth);
        else
          return evaluateExpression(variable, attributeTable, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "ifnull", e.Message);
        t.AddParm(variable);
        t.AddParm(defaultValue);
        t.AddParm(attributeTable);
        throw new Exception(e.Message);   // otherwise, we rethrow the message
      }
    }
    private object  isinherited(object attributeName, Item attributeTable, int depth)
    {
      string aName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(attributeName, attributeTable, depth));
      if (attributeTable.Contains(aName))
        return attributeTable.Attributes[aName].IsInherited;
      else
      {
        TAGExceptionMessage tm = new TAGExceptionMessage(className, "isinherited", "Attribute does not exist in this scope");
        tm.AddParm(attributeName);
        tm.AddParm(attributeTable);
        throw new Exception(tm.ToString());
      }
    }
    private object  left(object s1, object len, Item attributeTable, int depth)
    {
      try
      {
        string myString = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(s1, attributeTable, depth));
        int length = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, evaluateExpression(len, attributeTable, depth));
        if (length > myString.Length)
          length = myString.Length;
        return myString.Substring(0, length);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "left", e.Message);
        t.AddParm(s1);
        t.AddParm(len);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  len(object s1, Item attributeTable, int depth)
    {
      string s = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(s1, attributeTable, depth));
      return s.Length;
    }
    private object  lookup(object pAttributeName, object pValueTable, object pColumnName, Item attributeTable, int depth)
    {
      try
      {
        return lookupinList(pValueTable, pAttributeName, pColumnName, false, attributeTable, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "lookup", e.Message);
        t.AddParm(pAttributeName);
        t.AddParm(pValueTable);
        t.AddParm(pColumnName);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  lookuptable(object pAttributeName, object pValueTable, object pColumnName, Item attributeTable, int depth)
    {
      try
      {
        return lookupinList(pValueTable, pAttributeName, pColumnName, false, attributeTable, depth, TAGFunctions.DATATYPESTRING, true);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "lookuptable", e.Message);
        t.AddParm(pAttributeName);
        t.AddParm(pValueTable);
        t.AddParm(pColumnName);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  lookuprange(object pLookupValue, object pValueTable, object pColumnName, Item attributeTable, int depth)
    {
      /*
       * OK so this routine takes an attribute table and does a lookup nearest. Some rules are:
       * 
       * - all of the lookup values are ALWAYS in first column
       * - both the value we are looking up and the values in the lookup column must be convertible to numbers
       *   and numeric comparison is used
       * - Table must be TableHeader 
       * - the value we are looking for is in column with the name in pColumnName
       * - the "range" part means we find the row whose value is <= the value we are looking for.
       */
      return lookupinList(pValueTable, pLookupValue, pColumnName, true, attributeTable, depth, TAGFunctions.DATATYPEDECIMAL, false);
    }
    private object  lookuprangedate(object pAttributeName, object pValueTable, object pColumnName, Item attributeTable, int depth)
    {
      try
      {
        return lookupinList(pValueTable, pAttributeName, pColumnName, true, attributeTable, depth, TAGFunctions.DATATYPEDATETIME);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "lookuprangedate", e.Message);
        t.AddParm(pAttributeName);
        t.AddParm(pValueTable);
        t.AddParm(pColumnName);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  lookuptablerange(object pAttributeName, object pValueTable, object pColumnName, Item attributeTable, int depth)
    {
      try
      {
        return lookupinList(pValueTable, pAttributeName, pColumnName, true, attributeTable, depth, TAGFunctions.DATATYPESTRING, true);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "lookuprangedate", e.Message);
        t.AddParm(pAttributeName);
        t.AddParm(pValueTable);
        t.AddParm(pColumnName);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  lookuptablerangedate(object pAttributeName, object pValueTable, object pColumnName, Item attributeTable, int depth)
    {
      try
      {
        return lookupinList(pValueTable, pAttributeName, pColumnName, true, attributeTable, depth, TAGFunctions.DATATYPEDATETIME, true);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "lookuprangedate", e.Message);
        t.AddParm(pAttributeName);
        t.AddParm(pValueTable);
        t.AddParm(pColumnName);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  maxdate(object pDateList, Item attributeTable, int depth)
    {
      try
      {
        return minmax(pDateList, "max", TAGFunctions.DATATYPEDATETIME, attributeTable, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "maxdate", e.Message);
        t.AddParm(pDateList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  maxnumber(object pNumberList, Item attributeTable, int depth)
    {
      try
      {
        return minmax(pNumberList, "max", TAGFunctions.DATATYPEDECIMAL, attributeTable, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "maxnumber", e.Message);
        t.AddParm(pNumberList);
        throw new Exception(t.ToString());
      }
    }
    private object  mid(object s1, object position, object length, Item attributeTable, int depth)
    {
      try
      {
        int pos, len;
        string news1 = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(s1, attributeTable, depth));
        if (news1.Length == 0)
          return string.Empty;
        pos = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, evaluateExpression(position, attributeTable, depth));
        if (pos < 0)
          pos = 0;
        if (pos > news1.Length - 1)
          pos = news1.Length - 1;
        len = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, evaluateExpression(length, attributeTable, depth));
        if (len < 0)
          len = 0;
        if (pos + len > news1.Length)     // if we go past the end of the string
          len = news1.Length - pos;       // then we shorten the len to end at the end of the string
        string sOut = news1.Substring(pos, len);
        return sOut;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "mid", e.Message);
        t.AddParm(s1);
        t.AddParm(position);
        t.AddParm(length);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  mindate(object pDateList, Item attributeTable, int depth)
    {
      try
      {
        return minmax(pDateList, "min", TAGFunctions.DATATYPEDATETIME, attributeTable, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "mindate", e.Message);
        t.AddParm(pDateList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  minnumber(object pNumberList, Item attributeTable, int depth)
    {
      try
      {
        return minmax(pNumberList, "min", TAGFunctions.DATATYPEDECIMAL, attributeTable, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "minnumber", e.Message);
        t.AddParm(pNumberList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  multiply(object pNumberList, Item attributeTable, int depth)
    {
      try
      {
        string numberList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(pNumberList, attributeTable, depth));
        /*
         * first, we strip off parenthesis on the outside if they exist
         */
        numberList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, numberList);
        string[] delimiters = { cLISTSEPARATOR };
        string[] numberArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, numberList, delimiters);
        int arrayLen = numberArray.GetLength(0);
        //string[] numberArray = new string[arrayLen];
        //for (int j = 0; j < arrayLen; j++)
        //  numberArray[j] = evaluateExpression(tokenArray[j], attributeTable, depth).ToString();
        decimal dresult = 1;
        for (int i = 0; i < arrayLen; i++)
        {
          object myToken = evaluateExpression(numberArray[i], attributeTable, depth);
          if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsNumeric, myToken)) // skip null or empty values or non-numeric values
            dresult *= (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, myToken);
          else   //added by Jack
            dresult = 0;  //added by Jack
        }
        return (object)dresult;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "multiply", e.Message);
        t.AddParm(pNumberList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  not(object condition, Item attributeTable, int depth)
    {
      try
      {
        object oCondition = evaluateExpression(condition, attributeTable, depth);
        return !ifCondition(oCondition.ToString(), attributeTable, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "not", e.Message);
        t.AddParm(condition);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  or(object conditionList, Item attributeTable, int depth)
    {
      try
      {
        // 5/17/2010 LLA: change so other conditions are not eval'd unless necessary
        string sConditionList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(conditionList, attributeTable, depth));
        sConditionList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, sConditionList);
        string[] delimiters = { cLISTSEPARATOR };
        string[] conditionArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, sConditionList, delimiters);
        for (int i = 0; i < conditionArray.GetLength(0); i++)
        {
          if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, ifCondition(conditionArray[i], attributeTable, depth))) // if any condition is true
            return (object)true; // then we return false
        }
        return (object)false;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "or", e.Message);
        t.AddParm(conditionList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  payrollnumber(object eventDate, object numberDaysInPayroll, Item attributeTable, int depth)
    {
      DateTime dtEventDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, evaluateExpression(eventDate, attributeTable, depth));
      int iNbrDays = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, evaluateExpression(numberDaysInPayroll, attributeTable, depth));
      if (iNbrDays == 0)
        iNbrDays = 1;
      int iPayrollNumber = 0;
      int iDay = dtEventDate.Day;
      while (iDay > 0)
      {
        iDay -= iNbrDays;
        iPayrollNumber++;
      }
      return (object)iPayrollNumber;
    }
    private object  plandate(object PlanBegin, object EffectiveDate, object Mode, Item attributeTable, int depth)
    {
      try
      {
        return Plandate(PlanBegin, EffectiveDate, Mode, attributeTable, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "plandate", e.Message);
        t.AddParm(PlanBegin);
        t.AddParm(EffectiveDate);
        t.AddParm(Mode);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  replacestring(object sourceString, object findString, object replaceString,
      Item attributeTable, int depth)
    {
      try
      {
        string source = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(sourceString, attributeTable, depth));
        string find = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(findString, attributeTable, depth));
        string replace = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(replaceString, attributeTable, depth));
        return source.Replace(find, replace);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "replacestring", e.Message);
        t.AddParm(sourceString);
        t.AddParm(findString);
        t.AddParm(replaceString);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  right(object s1, object len, Item attributeTable, int depth)
    {
      try
      {
        string myString = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(s1, attributeTable, depth));
        int iStart = myString.Length - (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, evaluateExpression(len, attributeTable, depth));
        if (iStart < 0)
          iStart = 0;
        return myString.Substring(iStart);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "right", e.Message);
        t.AddParm(s1);
        t.AddParm(len);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  round(object pValue, object pPrecision, Item attributeTable, int depth)
    {
      try
      {
        return Round(pValue, pPrecision, "round", attributeTable, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "round", e.Message);
        t.AddParm(pValue);
        t.AddParm(pPrecision);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  roundtruncate(object pValue, object pPrecision, object pMode, Item attributeTable, int depth)
    {
      try
      {
        return Round(pValue, pPrecision, pMode, attributeTable, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "roundtruncate", e.Message);
        t.AddParm(pValue);
        t.AddParm(pPrecision);
        t.AddParm(pMode);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  selectcase(object testValue, object caseList, object defaultValue, Item attributeTable, int depth)
    {
      try
      {
        string sTestValue = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(testValue, attributeTable, depth));  // the value to compare
        string strCaseList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(caseList, attributeTable, depth)); // string format at table of comparevalues/returnvalues
        object oDefault = evaluateExpression(defaultValue, attributeTable, depth); //resolved value of the default if no tests match
        if (strCaseList.Substring(0, 1) == Convert.ToString(cLEFT) && // is it enclosed in parens?
              strCaseList.Substring(strCaseList.Length - 1, 1) == Convert.ToString(cRIGHT))
          strCaseList = strCaseList.Substring(1, strCaseList.Length - 2);
        string[] delimiters = { cLISTSEPARATOR };
        string[] caseTable = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, strCaseList, delimiters);
        int lenCase = caseTable.GetLength(0);
        for (int i = 0; i < lenCase; i = i + 2)  // tests are every other one
        {
          string condition = sTestValue + "==" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, caseTable[i]);
          if (ifCondition(condition, attributeTable, depth))
            if (i + 1 < lenCase)  // is there a return value after this test value?
              return evaluateExpression(caseTable[i + 1], attributeTable, depth);
            else
              return oDefault;
        }
        return oDefault;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "selectcase", e.Message);
        t.AddParm(testValue);
        t.AddParm(caseList);
        t.AddParm(defaultValue);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  selectcondition(object conditionList, object defaultValue, Item attributeTable, int depth)
    {
      try
      {
        string strConditionList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(conditionList, attributeTable, depth)); // string format at table of comparevalues/returnvalues
        object oDefault = evaluateExpression(defaultValue, attributeTable, depth); //resolved value of the default if no tests match
        strConditionList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, strConditionList);
        string[] delimiters = { cLISTSEPARATOR };
        string[] caseTable = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, strConditionList, delimiters);
        int lenCase = caseTable.GetLength(0);
        for (int i = 0; i < lenCase; i = i + 2)  // tests are every other one
        {
          string condition = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(caseTable[i], attributeTable, depth));
          condition = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, condition); ;
          if (ifCondition(condition, attributeTable, depth))
            if (i + 1 < lenCase)  // is there a return value after this test value?
              return evaluateExpression(caseTable[i + 1], attributeTable, depth);
            else
              return oDefault;
        }
        return oDefault;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "selectcondition", e.Message);
        t.AddParm(conditionList);
        t.AddParm(defaultValue);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  subtract(object operand1, object operand2, Item attributeTable, int depth)
    {
      try
      {
        decimal op1 = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(operand1, attributeTable, depth));
        decimal op2 = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(operand2, attributeTable, depth));
        return (object)(op1 - op2);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "subtract", e.Message);
        t.AddParm(operand1);
        t.AddParm(operand2);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  sum(string pNumberList, Item attributeTable, int depth)
    {
      try
      {
        string numberList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(pNumberList, attributeTable, depth));
        /*
         * first, we strip off parenthesis on the outside if they exist
         */
        numberList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, numberList);
        string[] delimiters = { cLISTSEPARATOR };
        string[] numberArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, numberList, delimiters);
        int arrayLen = numberArray.GetLength(0);
        //string[] numberArray = new string[arrayLen];
        //for (int j = 0; j < arrayLen; j++)
        //  numberArray[j] = evaluateExpression(tokenArray[j], attributeTable, depth).ToString();
        decimal dsum = 0;
        for (int i = 0; i < arrayLen; i++)
        {
          object myToken = evaluateExpression(numberArray[i], attributeTable, depth);
          decimal num = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, myToken);
          dsum += num;
        }
        return (object)dsum;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "sum", e.Message);
        t.AddParm(pNumberList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  tablefindreplace(object key, object table, object columnname, object oldvalue, object newvalue, Item attributeTable, int depth)
    {
      try
      {
        if (table != null && table.GetType() == typeof(TableHeader))
        {
          string oKey = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(key, attributeTable, depth));
          string oColumn = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(columnname, attributeTable, depth));
          TableHeader th = ((TableHeader)table).FindReplace(oKey, oColumn, oldvalue, newvalue);
          return th;
        }
        else
        {
          TAGExceptionMessage tm = new TAGExceptionMessage(className, "tablefindreplace", "Table is not a tableheader");
          tm.AddParm(key);
          tm.AddParm((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, table));
          tm.AddParm(columnname);
          tm.AddParm(oldvalue);
          tm.AddParm(newvalue);
          throw new Exception(tm.ToString());
        }
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "tablefindreplace", e.Message);
        t.AddParm(key);
        t.AddParm(table);
        t.AddParm(columnname);
        t.AddParm(oldvalue);
        t.AddParm(newvalue);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  tablemerge(object targetTable, object sourceTable, Item attributeTable, int depth)
    {
      object oTarget = evaluateExpression(targetTable, attributeTable, depth);
      object oSource = evaluateExpression(sourceTable, attributeTable, depth);
      if (oTarget.GetType() != typeof(TableHeader) || oSource.GetType() != typeof(TableHeader))
      {
        TAGExceptionMessage tm = new TAGExceptionMessage(className, "TableMerge", "Both target and source tables must be tableheaders");
        tm.AddParm(targetTable);
        tm.AddParm(sourceTable);
        throw new Exception(tm.ToString());
      }
      TableHeader thTarget = ((TableHeader)oTarget);
      thTarget.Merge((TableHeader)oSource);
      return thTarget;
    }
    private object  today(Item attributeTable, int depth)
    {
      return attributeTable.EffectiveDate;
    }
    private object  trimtable(object pStringTable, object startColumn, object endColumn, Item attributeTable, int depth)
    {
      try
      {
        object oTable = evaluateExpression(pStringTable, attributeTable, depth);
        if (oTable != null && oTable.GetType() == typeof(TableHeader))
        {
          string sStartColumn = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(startColumn, attributeTable, depth))).ToLower();
          string sEndColumn = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(endColumn, attributeTable, depth))).ToLower();
          TableHeader th = (TableHeader)oTable;
          return th.TrimTable(sStartColumn, sEndColumn);
        }
        else
          throw new Exception("Table parameter is not a TableHeader");
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "trimtable", e.Message);
        t.AddParm(pStringTable);
        t.AddParm(startColumn);
        t.AddParm(endColumn);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  truncate(object pValue, object pPrecision, Item attributeTable, int depth)
    {
      try
      {
        return Round(pValue, pPrecision, "truncate", attributeTable, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "truncate", e.Message);
        t.AddParm(pValue);
        t.AddParm(pPrecision);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }
    private object  withholding(object Base, object PayrollPerYear, object Multiple, object FlatAmount, object WithholdingTable,
      object TotalExemptionAmount, object TotalCreditAmount, object Threshold, object StandardDeduction, object WithholdingStatus,
      object Mode, Item attributeTable, int depth)
    {
      /*
       * evaluate parms and create variables to calc withholding
       */
      string sWithholdingStatus = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(WithholdingStatus, attributeTable, depth))).ToLower();
      string sMode = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(Mode, attributeTable, depth))).ToLower();
      string sWithholdingTable = null;
      TableHeader thWithholdingTable = null;
      object oWithholdingTable = evaluateExpression(WithholdingTable, attributeTable, depth);
      // if we got a tableheader, then use that instead of a string
      if (oWithholdingTable != null && oWithholdingTable.GetType() == typeof(TableHeader))
      {
        thWithholdingTable = (TableHeader)oWithholdingTable;
        oWithholdingTable = thWithholdingTable;
      }
      else
      {
        sWithholdingTable = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, oWithholdingTable)).ToLower();
        oWithholdingTable = sWithholdingTable;
      }

      decimal dResult = 0;
      decimal dBase = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(Base, attributeTable, depth));
      decimal dPayrollPerYear = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, evaluateExpression(PayrollPerYear, attributeTable, depth));
      decimal dMultiple = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(Multiple, attributeTable, depth));
      decimal dFlatAmount = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(FlatAmount, attributeTable, depth));
      decimal dTotalExemptionAmount = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(TotalExemptionAmount, attributeTable, depth));
      decimal dTotalCreditAmount = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(TotalCreditAmount, attributeTable, depth));
      decimal dThreshold = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(Threshold, attributeTable, depth));
      decimal dStandardDeduction = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(StandardDeduction, attributeTable, depth));
      decimal dAnnualBase = Math.Max(dBase * dPayrollPerYear - dStandardDeduction - dTotalExemptionAmount, 0);
      decimal dTableAddition = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, lookuprange(dAnnualBase, oWithholdingTable, "flatamount", attributeTable, depth));
      decimal dTableMultiple = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, lookuprange(dAnnualBase, oWithholdingTable, "multiple", attributeTable, depth));
      decimal dBracketFloor = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, lookuprange(dAnnualBase, oWithholdingTable, "beginbase", attributeTable, depth)) - (decimal).01;

      if (sMode == "forward")
      {
        if (dBase <= dThreshold)
          dResult = 0;
        else
          if (sWithholdingStatus != "withhold") // exempt or nowithhold
            dResult = 0;
          else
          {
            //decimal dAnnualBase = dBase * dPayrollPerYear - dTotalExempt - dStandardDeduction;    // calculate annual base
            dResult = ((dAnnualBase - dBracketFloor) * dTableMultiple + dTableAddition - dTotalCreditAmount) / dPayrollPerYear; // calc wh
          }
        dResult = dResult + dFlatAmount + dBase * dMultiple;    //adjust for additional wh
      }
      else
      {
        decimal dAnnualAfterTaxBase = dBase * dPayrollPerYear + dTotalExemptionAmount + dTotalCreditAmount + dStandardDeduction;   // calc base
        dResult = ((dAnnualAfterTaxBase * dPayrollPerYear) + dBracketFloor - dTotalCreditAmount -                         // calc gross??
          (dTotalExemptionAmount + dTableAddition + dStandardDeduction) * dTableMultiple) / (dPayrollPerYear - dPayrollPerYear * dTableMultiple);
        // how do I adjust for additional wh here?
        /*
         * this was from the original OnTime wiki entry
  Result = (Base * Frequency + BracketRevFloor- CreditAmount - (ExemptionAmount + TaxRevAddition + StandardDeduction) * TaxRevMultiplier) / (Frequency - Frequency * TaxRevMultipler

        */
      }
      dResult = Math.Round(dResult, 2);
      return (object)dResult;
    }


    #endregion  private evaluateExpresion (func) functions

    #region private functions

    private object attributeValue(Item item, string attributeName, int depth)
    {
      /*
       * Modified 1/12/2009 by LLA
       * 
       * When an attribute contains an unresolved function, it is not parsing properly in the evaluateExpression 
       * loop, because it does not have the cFUNCTION Prefix. This change is meant to address this problem.
       * Hopefully, it will also make calculation of functions independent of resolution order, so that
       * it calculates in a "natural order" way.
       */
      if (item.Contains(attributeName))
      {
        //TODO lmv66: 2010 0320 Constant Values, could be declared at the class loading
        string c_TABLEHEADER = "tableheader";
        string c_FUNC = "func";
        string c_VALUE = "value";

        TAGAttribute a = item[attributeName];
        object returnValue = item[attributeName].Value;

        string compareValueType = a.ValueType.ToLower();
        if (compareValueType == c_FUNC)
        {
          string strReturn = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, returnValue);
          a.ReferenceValueSource = strReturn;
          if (!strReturn.StartsWith(cFUNCTION.ToString()))
            strReturn = cFUNCTION + strReturn;  // add func prefix if it is one
          returnValue = evaluateExpression(strReturn, item, depth);

          //lmv66 2010 0320: BEGIN: This was copied fromt functionProcessor, since we are changing a attribute value in the item!
          if (returnValue == null)  // don't try to parse if the value is null or too short
          {
            item.setValueType(a.ID, c_VALUE);
            a.ValueType = c_VALUE;
            a.Value = null;
          }
          else
          {
            if (DictionaryFactory.getInstance().getDictionary().ExistsAttributeProperty(a.OriginalID, c_TABLEHEADER))
              item.setValueType(a.OriginalID, c_TABLEHEADER);
            else
              item.setValueType(a.OriginalID, c_VALUE);

            a.Value = returnValue;

          }
        }
        ///*
        // * If this is a tableheader/mod and it is still of type string, then we take this opportunity to convert it to a
        // * TableHeader class object
        // */        
        if (returnValue != null && item.Dictionary != null &&
          (compareValueType == TAGFunctions.TABLEHEADER || compareValueType == TAGFunctions.TABLEMOD)
          && returnValue.GetType().Name.ToLower() == TAGFunctions.DATATYPESTRING
          && (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsTableHeader, returnValue))
          returnValue = new TableHeader(attributeName, returnValue.ToString(), item.Dictionary);

        return returnValue;
      }
      else
      {
        //return string.Empty;
        TAGExceptionMessage t = new TAGExceptionMessage(className, "attributeValue", "Attribute lookup did not find attribute");
        t.AddParm(attributeName);
        //t.AddParm(item);
        throw new Exception(t.ToString());
      }
    }
    private bool Between(object pTestValue, object pFromValue, object pToValue, string compareType,
      Item attributeTable, int depth)
    {
      bool isBetween = false;
      object testValue = evaluateExpression(pTestValue, attributeTable, depth);
      object fromValue = evaluateExpression(pFromValue, attributeTable, depth);
      object toValue = evaluateExpression(pToValue, attributeTable, depth);
      switch (compareType.ToLower())
      {
        case TAGFunctions.DATATYPESTRING:
          isBetween = (((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, fromValue)).CompareTo((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, testValue)) <= 0 &&
              ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, testValue)).CompareTo((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, toValue)) <= 0);
          break;
        case TAGFunctions.DATATYPEDATETIME:
          isBetween = ((DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, fromValue) <= (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, testValue) &&
            (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, testValue) <= (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, toValue));
          break;
        case TAGFunctions.DATATYPEDECIMAL:
          isBetween = ((decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, fromValue) <= (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, testValue) &&
              (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, testValue) <= (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, toValue));
          break;
      }
      return isBetween;
    }
    private object calculate(object Value1, object Value2, Item attributeTable, int depth, string op)
    {
      object oValue1 = evaluateExpression(Value1, attributeTable, depth);
      object oValue2 = evaluateExpression(Value2, attributeTable, depth);
      decimal v1, v2;
      try
      {
        v1 = Convert.ToDecimal(oValue1);
      }
      catch
      {
        if (op == "divide")
          v1 = 0;
        else
          v1 = 1;
      }
      try
      {
        v2 = Convert.ToDecimal(oValue2);
      }
      catch
      {
        if (op == "exponent")
          v2 = 0;
        else
          v2 = 1;
      }
      decimal returnValue = 0;
      if (op == "divide")
        if (v2 == 0)  // can't divide by zero
          returnValue = 0;
        else
          returnValue = v1 / v2;
      else
        if (op == "multiply")
          returnValue = v1 * v2;
        else
          if (op == "exponent")
            returnValue = Convert.ToDecimal(Math.Pow(Convert.ToDouble(v1), Convert.ToDouble(v2)));
      return (object)returnValue;
    }
    private object DateDiff(DateTime sDate, DateTime eDate, string dType, int rnd)
    {
      /*
       * Note: this does not currently take leap years into account. 
       */
      decimal dateDiff = 0;
      decimal[] daysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
      decimal[] daysInQuarters = { 90, 91, 92, 92 };
      Calendar myCalender = CultureInfo.InvariantCulture.Calendar;
      int i = 0;
      decimal monthdays = 0;
      decimal quarterdays = 0;
      decimal days = 0;
      int year = 0;
      TimeSpan ts = eDate.Subtract(sDate);
      dateDiff = ts.Days;
      switch (dType)
      {
        case "y":
          dateDiff = dateDiff / (decimal)365; // not exact, but add 1/4 day to each year to account for leap years
          break;
        case "m":
          i = eDate.Month - 1;
          if (i < 0)
          {
            i = 11;
            year = year - 1;
          }
          year = eDate.Year;
          decimal months = 0;
          days = dateDiff;
          monthdays = daysInMonth[i];
          if (myCalender.IsLeapYear(year) && i == 2)
            monthdays++;
          while (days > monthdays)
          {

            days = days - monthdays;
            months = months + 1;
            i--;
            if (i < 0)
            {
              i = 11;
              year = year - 1;
            }
            monthdays = daysInMonth[i];
            if (myCalender.IsLeapYear(year) && i == 2)
              monthdays++;
          }
          monthdays = daysInMonth[i];
          if (myCalender.IsLeapYear(year) && i == 2)
            monthdays = monthdays + 1;
          dateDiff = months + days * (decimal)1.0 / monthdays;
          break;
        case "q":
          i = eDate.Month / 3 + 1;
          year = eDate.Year;
          decimal quarters = 0;
          days = dateDiff;
          quarterdays = daysInQuarters[i];
          if (myCalender.IsLeapYear(year) && i == 1)
            quarterdays++;
          while (days > daysInQuarters[i])
          {
            quarterdays = daysInQuarters[i];
            if (myCalender.IsLeapYear(year) && i == 1)
              quarterdays++;
            days = days - daysInQuarters[i];

            quarters = quarters + 1;
            i--;
            if (i < 0)
              i = 3;
          }
          quarterdays = daysInQuarters[i];
          if (myCalender.IsLeapYear(year) && i == 1)
            quarterdays++;
          dateDiff = quarters + quarters * (decimal)1.0 / quarterdays;
          break;
        case "w":
          dateDiff = dateDiff / 7;
          break;
        case "d":
          // dateDiff remains the tspan.Days value
          break;
        default:
          dateDiff = 0;
          break;
      }

      decimal factor = (decimal)Math.Pow(10, -rnd);

      dateDiff = (decimal)Convert.ToInt64(dateDiff / factor);
      dateDiff = dateDiff * factor;

      return dateDiff;
    }

    private bool isAtomic(object returnValue)
    {
      if (returnValue == null)
        return true;
      else
        if (returnValue.GetType() == typeof(string))
        {
          if ((string)returnValue == string.Empty)
            return true;
        }
        else
          return true;
      return false;
    }
    private object processMinus(object returnValue, out Type t)
    {
      // Note: I also put these if statements in order of most likely to be true, so there are a minimum of checks for performance reasons
      t = returnValue.GetType();
      // how we handle the minus sign depends on the type of the object
      if (t == typeof(string))
        returnValue = "-" + (string)returnValue;  // prepend the minus sign (put it back on)
      else if (t == typeof(decimal))
        returnValue = (decimal)-1.0 * (decimal)returnValue; // multiply by -1
      else if (t == typeof(bool))
        returnValue = !(bool)returnValue;                 // negate it
      else if (t == typeof(int))
        returnValue = -1 * (int)returnValue;              // multiply by -1
      else if (t == typeof(double))
        returnValue = -1.0 * (double)returnValue;         // multiply by -1
      else if (t == typeof(long))
        returnValue = -1 * (long)returnValue;              // multiply by -1
      //else:    // otherwise, ignore the minus sign, so we leave returnValue like it is
      return returnValue;
    }
    private object evaluateFunction(string strExpression, Item attributeTable, int depth, out bool isFinished)
    {
      string[] tokenList = null;
      bool badFunctionCall = false;
      object returnValue = null;
      string functionName = string.Empty;
      string insideExpression;
      isFinished = false;
      functionName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getFunctionName, strExpression, out insideExpression);  // look for an @function
      if (functionName == string.Empty)   // this expression does not start with a function or an attribute character
        isFinished = true;    // exit the loop
      else
      {
        tokenList = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, insideExpression); // there is an @ function, so parse the string inside the "()" for tokens
        switch (functionName.ToLower())           // and execute the appropriate function
        {
          case "abs":
            if (tokenList.GetLength(0) == 1)
              returnValue = abs((object)tokenList[0], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "add":
            if (tokenList.GetLength(0) == 2)
              returnValue = add((object)tokenList[0], (object)tokenList[1], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "age":
            if (tokenList.GetLength(0) == 2)
              returnValue = age((object)tokenList[0], (object)tokenList[1], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "and":
            if (tokenList.GetLength(0) == 1)
              returnValue = and((object)tokenList[0], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "attributedate":
            if (tokenList.GetLength(0) == 3)
              returnValue = attributedate((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "between":
            if (tokenList.GetLength(0) == 3)
              returnValue = between((object)tokenList[0], (object)tokenList[1], (object)tokenList[2],
                attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "betweendate":
            if (tokenList.GetLength(0) == 3)
              returnValue = betweendate((object)tokenList[0], (object)tokenList[1], (object)tokenList[2],
                attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "betweennumber":
            if (tokenList.GetLength(0) == 3)
              returnValue = betweennumber((object)tokenList[0], (object)tokenList[1], (object)tokenList[2],
                attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "calculatedate":
            if (tokenList.GetLength(0) == 3)
              returnValue = calculatedate((object)tokenList[0], (object)tokenList[1], (object)tokenList[2],
                attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "cap":
            if (tokenList.GetLength(0) == 4)
              returnValue = cap((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], (object)tokenList[3],
                                attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)((decimal)0);
            }
            break;
          case "coalesce":
            if (tokenList.GetLength(0) == 1)
              returnValue = (object)coalesce(tokenList[0], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "concatenate":
            if (tokenList.GetLength(0) == 1)
              returnValue = (object)concatenate(tokenList[0], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "containslist":
            if (tokenList.GetLength(0) == 2)
              returnValue = containslist((object)tokenList[0], (object)tokenList[1], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "containstable":
            if (tokenList.GetLength(0) == 2)
              returnValue = containslist((object)tokenList[0], (object)tokenList[1], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "converttolist":
            if (tokenList.GetLength(0) == 1)
              returnValue = (object)converttolist(tokenList[0], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "dateadd":
            if (tokenList.GetLength(0) == 2)
              returnValue = dateadd((object)tokenList[0], (object)tokenList[1], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "datediff":
            if (tokenList.GetLength(0) == 4)
              returnValue = datediff((object)tokenList[0], (object)tokenList[1],
                (object)tokenList[2], (object)tokenList[3], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "distinctvalue":
            if (tokenList.GetLength(0) == 2)
              returnValue = distinctvalue((object)tokenList[0], (object)tokenList[1], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "divide":
            if (tokenList.GetLength(0) == 2)
              returnValue = divide((object)tokenList[0], (object)tokenList[1], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "employeemos":
            if (tokenList.GetLength(0) == 2)
              returnValue = employeemos((object)tokenList[0], (object)tokenList[1], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "exp":
            if (tokenList.GetLength(0) == 2)
              returnValue = exp((object)tokenList[0], (object)tokenList[1], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "format":
            if (tokenList.GetLength(0) == 3)
              returnValue = format((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "iif":
            if (tokenList.GetLength(0) == 3)      // does this have the correct number of parameters?
              returnValue = (object)If(tokenList[0], tokenList[1], tokenList[2],
                attributeTable, ++depth); // evaluate the IF and return the results
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "isinherited":
            if (tokenList.GetLength(0) == 1)
              returnValue = isinherited((object)tokenList[0], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "ifnull":
            if (tokenList.GetLength(0) == 2)
              returnValue = ifnull((object)tokenList[0], (object)tokenList[1], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "isnull":
            if (tokenList.GetLength(0) == 1)
              returnValue = isnull((object)tokenList[0], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "left":
            if (tokenList.GetLength(0) == 2)
              returnValue = (object)left((object)tokenList[0], (object)tokenList[1], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "len":
            if (tokenList.GetLength(0) == 1)
              returnValue = (object)len((object)tokenList[0], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)0;
            }
            break;
          case "lookup":
            if (tokenList.GetLength(0) == 3)
              returnValue = lookupinList((object)tokenList[1], (object)tokenList[0],
                (object)tokenList[2], false, attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "lookuprange":
            if (tokenList.GetLength(0) == 3)
              returnValue = lookupinList((object)tokenList[1], (object)tokenList[0],
                (object)tokenList[2], true, attributeTable, ++depth, TAGFunctions.DATATYPEDECIMAL);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "lookuprangedate":
            if (tokenList.GetLength(0) == 3)
              returnValue = lookupinList((object)tokenList[1], (object)tokenList[0],
                (object)tokenList[2], true, attributeTable, ++depth, TAGFunctions.DATATYPEDATETIME);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "lookuptable":
            if (tokenList.GetLength(0) == 3)
              returnValue = lookuptable((object)tokenList[0], (object)tokenList[1],
                (object)tokenList[2], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "lookuptablerange":
            if (tokenList.GetLength(0) == 3)
              returnValue = lookuptablerange((object)tokenList[0], (object)tokenList[1],
                (object)tokenList[2], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "lookuptablerangedate":
            if (tokenList.GetLength(0) == 3)
              returnValue = lookuptablerangedate((object)tokenList[0], (object)tokenList[1],
                (object)tokenList[2], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "maxdate":
            if (tokenList.GetLength(0) == 1)
              returnValue = minmax((object)tokenList[0], "max", TAGFunctions.DATATYPEDATETIME, attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "maxnumber":
            if (tokenList.GetLength(0) == 1)
              returnValue = minmax((object)tokenList[0], "max", TAGFunctions.DATATYPEDECIMAL, attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "mid":
            if (tokenList.GetLength(0) == 3)
              returnValue = (object)mid((object)tokenList[0], (object)tokenList[1], (object)tokenList[2],
                attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "mindate":
            if (tokenList.GetLength(0) == 1)
              returnValue = minmax((object)tokenList[0], "min", TAGFunctions.DATATYPEDATETIME, attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "minnumber":
            if (tokenList.GetLength(0) == 1)
              returnValue = minmax((object)tokenList[0], "min", TAGFunctions.DATATYPEDECIMAL, attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "multiply":
            if (tokenList.GetLength(0) == 1)
              returnValue = multiply((object)tokenList[0], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "not":
            if (tokenList.GetLength(0) == 1)
              returnValue = (object)!ifCondition(tokenList[0], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "endofperiodyn":
            if (tokenList.GetLength(0) == 4)
              returnValue = endofperiodyn((object)tokenList[0], tokenList[1], tokenList[2], tokenList[3], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "or":
            if (tokenList.GetLength(0) == 1)
              returnValue = or((object)tokenList[0], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "payrollnumber":
            if (tokenList.GetLength(0) == 2)
              returnValue = (object)(payrollnumber(tokenList[0], tokenList[1], attributeTable, ++depth));
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)0;
            }
            break;
          case "plandate":
            if (tokenList.GetLength(0) == 3)
              returnValue = plandate((object)tokenList[0], (object)tokenList[1],
                (object)tokenList[2], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "replacestring":
            if (tokenList.GetLength(0) == 3)
              returnValue = (object)replacestring((object)tokenList[0], (object)tokenList[1],
                (object)tokenList[2], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "right":
            if (tokenList.GetLength(0) == 2)
              returnValue = (object)right((object)tokenList[0], (object)tokenList[1],
                attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "round":
            if (tokenList.GetLength(0) == 2)
              returnValue = Round((object)tokenList[0], (object)tokenList[1], "round",
                attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "roundtruncate":
            if (tokenList.GetLength(0) == 3)
              returnValue = Round((object)tokenList[0], (object)tokenList[1],
                (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, tokenList[2]), attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "selectcase":
            if (tokenList.GetLength(0) == 3)
              returnValue = selectcase((object)tokenList[0], (object)tokenList[1], (object)tokenList[2],
                attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "selectcondition":
            if (tokenList.GetLength(0) == 2)
              returnValue = selectcondition((object)tokenList[0], (object)tokenList[1],
                attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "subtract":
            if (tokenList.GetLength(0) == 2)
              returnValue = subtract((object)tokenList[0], (object)tokenList[1], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "sum":
            if (tokenList.GetLength(0) == 1)
              returnValue = (object)sum(tokenList[0], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "tablefindreplace":
            if (tokenList.GetLength(0) == 5)
              returnValue = tablefindreplace((object)tokenList[0], (object)tokenList[1],
                (object)tokenList[2], (object)tokenList[3], (object)tokenList[4],
                attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "trimtable":
            if (tokenList.GetLength(0) == 3)
              returnValue = trimtable((object)tokenList[0], (object)tokenList[1],
                (object)tokenList[2], attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "today":
            if (tokenList.GetLength(0) == 0 || tokenList.GetLength(0) == 1 && tokenList[0] == string.Empty)
              returnValue = today(attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)DateTime.Today;
            }
            break;
          case "truncate":
            if (tokenList.GetLength(0) == 2)
              returnValue = Round((object)tokenList[0], (object)tokenList[1], "truncate",
                attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "withholding":
            if (tokenList.GetLength(0) == 11)
              returnValue = withholding((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], (object)tokenList[3],
                (object)tokenList[4], (object)tokenList[5], (object)tokenList[6], (object)tokenList[7], (object)tokenList[8],
                (object)tokenList[9], (object)tokenList[10],
                attributeTable, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          default:
            {
              TAGExceptionMessage t1 = new TAGExceptionMessage(className, "evaluateExpression",
                "Unsupported function <" + functionName + "> was called");
              t1.AddParm(strExpression);
              t1.AddParm(attributeTable);
              throw new Exception(t1.ToString());
            }
        }
      }

      if (badFunctionCall)
      {
        TAGExceptionMessage e = new TAGExceptionMessage(className, functionName, "Incorrect number of arguments");
        for (int iParm = 0; iParm < tokenList.GetLength(0); iParm++)
          e.AddParm(tokenList[iParm]);
        throw new Exception(e.ToString());
      }
      return returnValue;
    }
    private object If(object condition, object valueTrue, object valueElse, Item attributeTable, int depth)
    {
      if (ifCondition((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, condition), attributeTable, depth))      // Call the method that evaluates the condition into True/False
        return evaluateExpression(valueTrue, attributeTable, depth);
      else
        return evaluateExpression(valueElse, attributeTable, depth);
    }
    private bool ifCondition(string condition, Item attributeTable, int depth)
    {
      try
      {
        bool isConditionTrue = false;
        string operand1 = string.Empty;
        string op = string.Empty;
        string operand2 = string.Empty;
        //object expression = evaluateExpression(condition,   attributeTable,  depth);
        //Type t = condition.GetType();
        //if (t.Name == TAGFunctions.DATATYPESTRING)
        if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseValidationString, condition, ref operand1, ref op, ref operand2))
        {
          object opLookup1 = evaluateExpression(operand1, attributeTable, depth);   // lookup a value for operand1 in the attribute list
          if (op == string.Empty && operand2 == string.Empty)
          {
            isConditionTrue = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, opLookup1);
          }
          else
          {
            if (opLookup1 == null)          // if is not found, then it is a scalar
              opLookup1 = operand1;
            object opLookup2 = evaluateExpression(operand2, attributeTable, depth); // same for operand 2
            if (opLookup2 == null)
              opLookup2 = operand2;
            isConditionTrue = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.passesTest, opLookup1, op, opLookup2); // now evaluate the test
          }
        }
        return isConditionTrue;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "ifCondition", e.Message);
        t.AddParm(condition);
        //t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object lookupinList(object lookupList, object attributeName, object columnName, bool pickNearest, Item attributeTable, int depth)
    {
      return lookupinList(lookupList, attributeName, columnName, pickNearest, attributeTable, depth, TAGFunctions.DATATYPESTRING, false);
    }
    private object lookupinList(object lookupList, object attributeName, object columnName, Item attributeTable, int depth, string dataType)
    {
      return lookupinList(lookupList, attributeName, columnName, false, attributeTable, depth, dataType, false);
    }
    private object lookupinList(object lookupList, object attributeName, object columnName, bool pickNearest, Item attributeTable, int depth, string dataType)
    {
      return lookupinList(lookupList, attributeName, columnName, pickNearest, attributeTable, depth, dataType, false);
    }
    private object lookupinList(object lookupList, object attributeName, object columnName, bool pickNearest, Item attributeTable, int depth, string dataType, bool returnTable)
    {
      try
      {
        object oList = evaluateExpression(lookupList, attributeTable, depth);
        object oLookup = evaluateExpression(attributeName, attributeTable, depth);
        object oColumn = evaluateExpression(columnName, attributeTable, depth);
        object dLookup;
        bool goodParms = true;
        string errorMsg = string.Empty;
        // we don't allow any empty parameters. If one is, we throw an error
        if (oList == null || oList.ToString().Length == 0) //() ||)
        {
          errorMsg = "List (lookup table) is missing";
          goodParms = false;
        }
        else
        {
          if (oLookup == null)
          {
            oLookup = string.Empty;
          }
          else
          {
            if (oColumn == null || oColumn.ToString().Length == 0)
            {
              errorMsg = "Column Name is missing";
              goodParms = false;
            }
          }
        }
        if (!goodParms)
        {
          ////return string.Empty;
          TAGExceptionMessage t = new TAGExceptionMessage(className, "LookupXX", errorMsg);
          t.AddParm("List=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, lookupList));
          t.AddParm("Name=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, attributeName));
          t.AddParm("Column=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, columnName));
          t.AddParm("PickNearest=" + pickNearest.ToString());
          t.AddParm("DataType=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, dataType));
          t.AddParm("LookupRange=" + returnTable.ToString());
          t.AddParm(attributeTable);
          throw new Exception(t.ToString());
        }

        // if lookup is a strint tableheader that hasn't been converted to a tableheader class yet, then go ahead and do it
        //if (oList.GetType().Name.ToLower() == TAGFunctions.DATATYPESTRING && (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsTableHeader, oList) && attributeTable.Dictionary != null)
        //  oLookup = new TableHeader(oLookup, oList.ToString(), attributeTable.Dictionary);
        return lookupUsingTableHeader(oLookup, oList, oColumn, pickNearest, returnTable); // skip the rest and use the TableHeader class to perform the lookup
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "LookupXX", e.Message);
        t.AddParm("List=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, lookupList));
        t.AddParm("Name=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, attributeName));
        t.AddParm("Column=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, columnName));
        t.AddParm("PickNearest=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, pickNearest));
        t.AddParm("DataType=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, dataType));
        t.AddParm("LookupRange=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, returnTable));
        //t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }

    }

    private object lookupUsingTableHeader(object oLookup, object oList, object oColumn, bool pickNearest, bool returnTable)
    {
      if (oList != null && oList.GetType() == typeof(TableHeader))
      {
        TableHeader th = (TableHeader)oList;
        string sLookup = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, oLookup);
        string sColumn = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, oColumn);
        if (returnTable)
          return th.LookupTable(sLookup, sColumn);
        if (pickNearest)
          return th.LookupRange(sLookup, sColumn);
        return th.Lookup(sLookup, sColumn);
      }
      return string.Empty;
    }

    private object minmax(object minmaxList, string minormax, string dataType, Item attributeTable, int depth)
    {
      object returnValue = null;
      string strMinMax = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(minmaxList, attributeTable, depth));
      /*
       * first, we strip off parenthesis on the outside of the list if they exist
       */
      strMinMax = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, strMinMax);
      string[] delimiters = { cLISTSEPARATOR };
      string[] minmaxArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, strMinMax, delimiters);
      bool firstValueSet = false;
      for (int i = 0; i < minmaxArray.GetLength(0); i++)
      {
        object oThisEntry = evaluateExpression(minmaxArray[i], attributeTable, depth);
        if (!(oThisEntry == null || oThisEntry.ToString() == string.Empty))  // skip any empty entries
        {
          if (dataType == TAGFunctions.DATATYPEDECIMAL)
          {
            if (!firstValueSet)                   // first time through
            {
              returnValue = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, oThisEntry);   // just set the min/max to the first value in the list
              firstValueSet = true;
            }
            else
            {
              decimal newValue = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, oThisEntry);
              if (minormax == "min")      // min
              {
                if (newValue < (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, returnValue))
                  returnValue = newValue;
              }
              else                        // max
              {
                if (newValue > (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, returnValue))
                  returnValue = newValue;
              }
            }
          }
          else
          {
            if (!firstValueSet)
            {
              returnValue = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, oThisEntry);
              firstValueSet = true;
            }
            else
            {
              DateTime newValue = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, oThisEntry);
              if (minormax == "min")
              {
                if (newValue < (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, returnValue))
                  returnValue = newValue;
              }
              else
              {
                if (newValue > (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, returnValue))
                  returnValue = newValue;
              }
            }
          }

        }
      }
      return returnValue;
    }
    private object Plandate(object PlanBegin, object EffectiveDate, object Mode, Item attributeTable, int depth)
    {
      DateTime resultDate = DateTime.Now;
      DateTime efDate;
      string sMode, sPlanBegin;
      efDate = Convert.ToDateTime(evaluateExpression(EffectiveDate, attributeTable, depth));
      sMode = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(Mode, attributeTable, depth));
      sPlanBegin = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(PlanBegin, attributeTable, depth));
      int Slash = sPlanBegin.IndexOf("/");
      Slash = sPlanBegin.IndexOf("/", Slash + 1);
      if (Slash > 1)
      { sPlanBegin = sPlanBegin.Substring(0, Slash); }
      String sEffectiveYear = efDate.Year.ToString();
      String sLastYear = (efDate.Year - 1).ToString();
      String sNextYear = (efDate.Year + 1).ToString();
      resultDate = DateTime.Parse(sPlanBegin + "/" + sEffectiveYear);

      switch (sMode)
      {
        case "EndPlan":
          if (resultDate < efDate)
          {
            resultDate = DateTime.Parse(sPlanBegin + "/" + sNextYear);
          }
          resultDate = resultDate.AddDays(-1);
          break;

        case "BeginPlan":
          if (resultDate > efDate)
          {
            resultDate = DateTime.Parse(sPlanBegin + "/" + sLastYear);
          }
          break;

        case "BeginPlanEnrollment":
          if (resultDate > efDate)
          {
            resultDate = DateTime.Parse(sPlanBegin + "/" + sLastYear);
          }
          break;

        case "PriorMonth":
          resultDate = DateTime.Parse(efDate.Month.ToString() + "/1/" + sEffectiveYear).AddDays(-1);
          break;

        case "NextMonth":
          resultDate = DateTime.Parse(efDate.Month.ToString() + "/1/" + sEffectiveYear).AddMonths(1);
          break;
      }

      return (object)(DateTime)resultDate;
    }
    private object Round(object pValue, object pPrecision, object mode, Item attributeTable, int depth)
    {
      object returnValue;
      object fromValue = evaluateExpression(pValue, attributeTable, depth);
      object precision = evaluateExpression(pPrecision, attributeTable, depth);
      object oMode = evaluateExpression(mode, attributeTable, depth);
      decimal myValue;
      decimal d;
      int exp;
      d = Convert.ToDecimal(fromValue);
      exp = Convert.ToInt32(precision);
      switch (oMode.ToString().ToLower())
      {
        case "round":
          break;
        case "truncate":
          if (d >= 0) //is the value we want to round positive?
            d = d - Convert.ToDecimal((double).5 * Math.Pow((double)10, Convert.ToDouble(exp * -1)));
          else
            d = d + Convert.ToDecimal((double).5 * Math.Pow((double)10, Convert.ToDouble(exp * -1)));
          break;
        case "roundup":
          if (d >= 0)
            d = d + Convert.ToDecimal((double).5 * Math.Pow((double)10, Convert.ToDouble(exp * -1)));
          else
            d = d - Convert.ToDecimal((double).5 * Math.Pow((double)10, Convert.ToDouble(exp * -1)));
          break;
      }
      /*
       * it turns out Math.Round does not support negative exponents, so we have to specially treat
       * the calculation of the exp is negative
       */
      if (exp < 0)  // is this a negative exponent?
      {
        myValue = Convert.ToDecimal(d * Convert.ToDecimal(Math.Pow((double)10, (double)(exp))));  // then move to the left exp places
        myValue = (decimal)Math.Round(myValue, 0);                    // round to the decimal point
        myValue = myValue * Convert.ToDecimal(Math.Pow((double)10, (double)(exp * -1))); // then move back to the right
      }
      else
        myValue = (decimal)Math.Round(d, exp);
      returnValue = (object)myValue;
      return returnValue;
    }

    #endregion private functions

    #endregion Original TAGBOSS overload functions!

    #region AttributeEngine2 overload functions!

    #region  private evaluateExpresion (func) functions

    /// <summary>
    /// Absolute value. Format is abs(operand)
    /// </summary>
    /// <param name="operand">Numeric value</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  abs(object operand, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        decimal dResult;
        object op = evaluateExpression(operand, aObj, attributeTable, eObj, depth);
        if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsNumeric, op))
          dResult = Math.Abs((decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, op));
        else
        {
          throw new Exception("ABS requires a numeric argument");
        }
        return (object)dResult;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "abs", e.Message + ":" + e.StackTrace);
        t.AddParm(operand);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    /// <summary>
    /// Add two values. Format is add(operand1, operand2)
    /// </summary>
    /// <param name="operand1">Numeric value</param>
    /// <param name="operand2">Numeric value</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  add(object operand1, object operand2, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        string op1 = evaluateExpression(operand1, aObj, attributeTable, eObj, depth).ToString();
        string op2 = evaluateExpression(operand2, aObj, attributeTable, eObj, depth).ToString();
        string numberList = "(" + op1.ToString() + "~" + op2.ToString() + ")";
        return sum(numberList, aObj, attributeTable, eObj, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "add", e.Message + ":" + e.StackTrace);
        t.AddParm(operand1);
        t.AddParm(operand2);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    /// <summary>
    /// Calculate age based on birthday and effective date. Format is age(birthdate, effectivedate)
    /// </summary>
    /// <param name="birthDate"></param>
    /// <param name="effectiveDate"></param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  age(object birthDate, object effectiveDate, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        DateTime eDate, bDate;
        object oBirthDate = evaluateExpression(birthDate, aObj, attributeTable, eObj, depth);
        object oEffectiveDate = evaluateExpression(effectiveDate, aObj, attributeTable, eObj, depth);
        eDate = Convert.ToDateTime(oEffectiveDate);
        bDate = Convert.ToDateTime(oBirthDate);
        return (object)Convert.ToInt32(DateDiff(bDate, eDate, "y", 0));
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "age", e.Message + ":" + e.StackTrace);
        t.AddParm(birthDate);
        t.AddParm(effectiveDate);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    /// <summary>
    /// Boolean function. Returns true if both condition1 and condition2 are true. format is and(condition1, condition2)
    /// </summary>
    /// <param name="conditionList"></param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  and(object conditionList, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        // 5/17/2010 LLA: change so other conditions are not eval'd unless necessary
        string sConditionList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(conditionList, aObj, attributeTable, eObj, depth));
        sConditionList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, sConditionList);
        string[] delimiters = { cLISTSEPARATOR };
        string[] conditionArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, sConditionList, delimiters);
        for (int i = 0; i < conditionArray.GetLength(0); i++)
        {
          if (!(bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, ifCondition(conditionArray[i], aObj, attributeTable, eObj, depth))) // if any condition is false
            return (object)false; // then we return false
        }
        return (object)true;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "and", e.Message + ":" + e.StackTrace);
        t.AddParm(conditionList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    /// <summary>
    /// Boolean function. returns true of testvalue is between fromvalue and tovalue. this is for string values.
    /// Format is between(testvalue, fromvalue, tovalue)
    /// </summary>
    /// <param name="testValue"></param>
    /// <param name="fromValue"></param>
    /// <param name="toValue"></param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  between(object testValue, object fromValue, object toValue, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return (object)Between(testValue, fromValue, toValue, TAGFunctions.DATATYPESTRING, aObj, attributeTable, eObj, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "between", e.Message + ":" + e.StackTrace);
        t.AddParm(testValue);
        t.AddParm(fromValue);
        t.AddParm(toValue);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    /// <summary>
    /// same as between, except for date values
    /// </summary>
    /// <param name="testValue"></param>
    /// <param name="fromValue"></param>
    /// <param name="toValue"></param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  betweendate(object testValue, object fromValue, object toValue, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return (object)Between(testValue, fromValue, toValue, TAGFunctions.DATATYPEDATETIME, aObj, attributeTable, eObj, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "betweendate", e.Message + ":" + e.StackTrace);
        t.AddParm(testValue);
        t.AddParm(fromValue);
        t.AddParm(toValue);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    /// <summary>
    /// same as between, except for numeric values
    /// </summary>
    /// <param name="testValue"></param>
    /// <param name="fromValue"></param>
    /// <param name="toValue"></param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  betweennumber(object testValue, object fromValue, object toValue, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return (object)Between(testValue, fromValue, toValue, TAGFunctions.DATATYPEDECIMAL, aObj, attributeTable, eObj, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "betweennumber", e.Message + ":" + e.StackTrace);
        t.AddParm(testValue);
        t.AddParm(fromValue);
        t.AddParm(toValue);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    /// <summary>
    /// Implementation of Jack's calculateDate function
    /// </summary>
    /// <param name="dateIn">Date to start with</param>
    /// <param name="modification">Modification strings (see TAGFunction.CalculateDate for details)</param>
    /// <param name="afterToday">Force the resulting date to be on or after today</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  calculatedate(object dateIn, object modification, object afterToday, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      object oDate = evaluateExpression(dateIn, aObj, attributeTable, eObj, depth);
      if (!(bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsDateTime, oDate))
      {
        TAGExceptionMessage tm = new TAGExceptionMessage(className, "CalculateDate", "DateIn is not a valid date");
        tm.AddParm(dateIn);
        tm.AddParm(modification);
        tm.AddParm(afterToday);
        throw new Exception(tm.ToString());
      }
      DateTime dtDateIn = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, oDate);
      string sMods = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(modification, aObj, attributeTable, eObj, depth));
      object oAfterToday = evaluateExpression(afterToday, aObj, attributeTable, eObj, depth);
      if (!(bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsBoolean, oAfterToday))
      {
        TAGExceptionMessage tm = new TAGExceptionMessage(className, "CalculateDate", "AfterToday is not a valid Boolean");
        tm.AddParm(dateIn);
        tm.AddParm(modification);
        tm.AddParm(afterToday);
        throw new Exception(tm.ToString());
      }
      bool bAfterToday = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, oAfterToday);
      DateTime returnDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CalculateDate, dtDateIn, sMods, bAfterToday);
      if (returnDate == TAGFunctions.FutureDateTime)
        return string.Empty;
      else
        return returnDate;
    }

    private object  cap(object priorAmount, object currentAmount, object targetAmount, object mode, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        decimal dCurrent = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(currentAmount, aObj, attributeTable, eObj, depth));
        object oTarget = evaluateExpression(targetAmount, aObj, attributeTable, eObj, depth);
        if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, isnull(oTarget, aObj, attributeTable, eObj, depth)) || (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, oTarget) == 0) //The zero check is temporary until Larry fixed the TDs to not evaluate to 0
          return dCurrent;
        decimal dPrior = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(priorAmount, aObj, attributeTable, eObj, depth));
        decimal dTarget = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, oTarget);
        string sMode = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(mode, aObj, attributeTable, eObj, depth))).ToLower();
        if (sMode == "max" || sMode == "max0")
        {
          if (dTarget > 0)
          {
            if (dTarget - dPrior < dCurrent)
              if (dTarget - dPrior < 0 && sMode == "max0")
                return 0;
              else
                return dTarget - dPrior;
            else
              return dCurrent;
          }
          else
          {
            if (dTarget - dPrior > dCurrent)
              if (dTarget - dPrior > 0 && sMode == "max0")
                return 0;
              else
                return dTarget - dPrior;
            else
              return dCurrent;
          }
        }
        else // min
        {
          if (dTarget > 0)
          {
            if (dTarget - dPrior > dCurrent)
              return dTarget - dPrior;
            else
              return dCurrent;
          }
          else
          {
            if (dTarget - dPrior < dCurrent)
              return dTarget - dPrior;
            else
              return dCurrent;
          }
        }
      }
      catch (Exception ex)
      {
        TAGExceptionMessage tm = new TAGExceptionMessage(className, "cap", ex.Message);
        tm.AddParm(priorAmount);
        tm.AddParm(currentAmount);
        tm.AddParm(targetAmount);
        tm.AddParm(mode);
        throw new Exception(tm.ToString());
      }
    }

    /// <summary>
    /// string functions takes the list format (string1~string1~string3...) and returns the concatination of all of the strings in the list.
    /// Format is concatenate((string1~string2~...)
    /// </summary>
    /// <param name="pStringList">List format set of strings (string1~string1~string3...)</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  concatenate(object pStringList, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        string returnValue = string.Empty;
        object oList = evaluateExpression(pStringList, aObj, attributeTable, eObj, depth);
        string stringList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, oList);
        /*
         * first, we strip off parenthesis on the outside if they exist
         */
        stringList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, stringList);
        string[] delimiters = { cLISTSEPARATOR };
        string[] stringArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, stringList, delimiters);
        for (int i = 0; i < stringArray.GetLength(0); i++)
        {
          object myToken = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(stringArray[i], aObj, attributeTable, eObj, depth));
          returnValue = returnValue + myToken;
        }
        return (object)returnValue;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "concatenate", e.Message + ":" + e.StackTrace);
        t.AddParm(pStringList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    /// <summary>
    /// boolean function which compares testvalue with each entry in the list. Returns true if the test value is the same as any of the values in the list.
    /// Format is containslist(testvalue, (value1~value2~value3...))
    /// </summary>
    /// <param name="testValue"></param>
    /// <param name="pStringList">List in the format (value1~value2~value3...)</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  containslist(object testValue, object pStringList, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        bool inList = false;
        object oList = evaluateExpression(pStringList, aObj, attributeTable, eObj, depth);
        object oValue = evaluateExpression(testValue, aObj, attributeTable, eObj, depth);
        if (oValue.GetType().Name.ToLower() == TAGFunctions.DATATYPESTRING)
          oValue = oValue.ToString().Trim();
        string stringList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, oList));
        string[] delimiters = { cLISTSEPARATOR };
        string[] stringArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, stringList, delimiters);
        for (int i = 0; i < stringArray.GetLength(0); i++)
        {
          object myToken = evaluateExpression(stringArray[i], aObj, attributeTable, eObj, depth);
          if (myToken.GetType().Name.ToLower() == TAGFunctions.DATATYPESTRING)
            myToken = myToken.ToString().Trim();
          if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.passesTest, myToken, TAGFunctions.EQCHAR, oValue))
          {
            inList = true;
            break;
          }
        }
        return (object)inList;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "containslist", e.Message + ":" + e.StackTrace);
        t.AddParm(testValue);
        t.AddParm(pStringList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    //TODO: Fix this, so as to be supported when tableHeaders are...
    /// <summary>
    /// boolean function which compares testvalue with each entry in the first column of the table. 
    /// Returns true if the test value is the same as any of the values in the first column of the table.
    /// Format is containslist(testvalue, (value1~field1|value2~field2|value3~field3|...))    /// </summary>
    /// <param name="testValue"></param>
    /// <param name="tableList">Attribute table with unique values in column 1</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  containstable(object testValue, object tableList, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        bool inList = false;
        TableHeader oTable = (TableHeader)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CTableHeader, evaluateExpression(tableList, aObj, attributeTable, eObj, depth));
        if (oTable == null || oTable.GetLength(0) == 0)
          return (object)inList;
        object oValue = evaluateExpression(testValue, aObj, attributeTable, eObj, depth);
        if (oValue.GetType().Name.ToLower() == TAGFunctions.DATATYPESTRING)
          oValue = oValue.ToString().Trim();
        for (int i = 0; i < oTable.GetLength(0); i++)
        {
          object myToken = evaluateExpression(oTable[i, 0], aObj, attributeTable, eObj, depth);
          if (myToken.GetType().Name.ToLower() == TAGFunctions.DATATYPESTRING)
            myToken = myToken.ToString().Trim();
          if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.passesTest, myToken, TAGFunctions.EQCHAR, oValue))
          {
            inList = true;
            break;
          }
        }
        return (object)inList;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "containstable", e.Message + ":" + e.StackTrace);
        t.AddParm(testValue);
        t.AddParm(tableList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    /// <summary>
    /// format is coalesce((value1~value2~value3...))
    /// accepts a list of values, and returns the first one in the list that is not empty.
    /// </summary>
    /// <param name="pStringList">List in the format (value1~value2~value3...)</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  coalesce(object pStringList, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        /*
         * find the first nonempty string in the list and return an evaluated version
         */
        object oList = evaluateExpression(pStringList, aObj, attributeTable, eObj, depth);
        string stringList = oList.ToString();
        /*
         * first, we strip off parenthesis on the outside if they exist
         */
        stringList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, stringList);
        string[] delimiters = { cLISTSEPARATOR };
        string[] stringArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, stringList, delimiters);
        for (int i = 0; i < stringArray.GetLength(0); i++)
        {
          if (stringArray[i] != null && stringArray[i] != string.Empty)
          {
            object myToken = evaluateExpression(stringArray[i], aObj, attributeTable, eObj, depth);
            if (myToken.ToString() != string.Empty)
              return (object)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.toValue, myToken.ToString());
          }
        }
        return (object)string.Empty;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "coalesce", e.Message + ":" + e.StackTrace);
        t.AddParm(pStringList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    //TODO: Fix this when the tableheader is implemented in the new model!
    /// <summary>
    /// Accepts an attributetable and converts the first column (normaly key values) into a list format (key1~key2~...)
    /// format is converttolist((value1~field1|value2~field2|value3~field3|...))
    /// </summary>
    /// <param name="pTable"></param>
    /// <param name="attributeTable">Attribute table in the format (value1~field1|value2~field2|value3~field3|...)</param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  converttolist(object pTable, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        string returnList = TAGFunctions.LEFTCHAR.ToString();
        TableHeader oTable = (TableHeader)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CTableHeader, evaluateExpression(pTable, aObj, attributeTable, eObj, depth));
        int nbrRows = oTable.GetLength(0);
        bool firstOne = true;
        for (int i = 0; i < nbrRows; i++)
        {
          if (firstOne)
            firstOne = false;
          else
            returnList += TAGFunctions.COLSEPARATORCHAR;
          returnList += (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, oTable[i, 0]);
        }
        returnList += TAGFunctions.RIGHTCHAR;
        return (object)returnList;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "converttolist", e.Message + ":" + e.StackTrace);
        t.AddParm(pTable);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    /// <summary>
    /// Add addDays days to fromDate and return the result
    /// </summary>
    /// <param name="fromDate">datetime to add the days to</param>
    /// <param name="addDays">Number of days to add</param>
    /// <param name="attributeTable">Attribute table in the format (value1~field1|value2~field2|value3~field3|...)</param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  dateadd(object fromDate, object addDays, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      object oDate = evaluateExpression(fromDate, aObj, attributeTable, eObj, depth);
      if (!(bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsDateTime, oDate))
      {
        TAGExceptionMessage tm = new TAGExceptionMessage(className, "DateAdd", "FromDate is not a valid date");
        tm.AddParm(fromDate);
        tm.AddParm(addDays);
        throw new Exception(tm.ToString());
      }
      DateTime dtFromDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, oDate);
      object oAddDays = (double)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDouble, evaluateExpression(addDays, aObj, attributeTable, eObj, depth));
      if (!(bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsNumeric, oAddDays))
      {
        TAGExceptionMessage tm = new TAGExceptionMessage(className, "DateAdd", "AddDays is not a valid number");
        tm.AddParm(fromDate);
        tm.AddParm(addDays);
        throw new Exception(tm.ToString());
      }
      double dblAddDays = (double)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDouble, oAddDays);
      return dtFromDate.AddDays(dblAddDays);
    }

    /// <summary>
    /// calculates the difference in dateType units between startDate and endDate. 
    /// Format is datediff(dateType,startdate,endDate,round)
    /// </summary>
    /// <param name="dateType">DateType is one of "y","m","q","w","d" which is Year,Month,Quarter,Week, or Day</param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="round">Orecision of rounded result. E.g., round 2 will round to nearest 10 units.</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  datediff(object dateType, object startDate, object endDate, object round, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        DateTime sDate, eDate;
        string dType;
        int rnd;
        sDate = Convert.ToDateTime(evaluateExpression(startDate, aObj, attributeTable, eObj, depth));
        eDate = Convert.ToDateTime(evaluateExpression(endDate, aObj, attributeTable, eObj, depth));
        dType = evaluateExpression(dateType, aObj, attributeTable, eObj, depth).ToString();
        rnd = Convert.ToInt32(evaluateExpression(round, aObj, attributeTable, eObj, depth));
        return DateDiff(sDate, eDate, dType, rnd);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "datediff", e.Message + ":" + e.StackTrace);
        t.AddParm(dateType);
        t.AddParm(startDate);
        t.AddParm(endDate);
        t.AddParm(round);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    /// <summary>
    /// Returns 'Monday', 'Tuesday', etc as the day of week of the date that was passed in
    /// </summary>
    /// <param name="dateIn"></param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  dayofweek(object dateIn, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      DateTime thisDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, evaluateExpression(dateIn, aObj, attributeTable, eObj, depth));
      string dayOfWeek = thisDate.DayOfWeek.ToString();
      return dayOfWeek;
    }

    //TODO: Fix this when the TableHeader is implemented in the new structure
    private object  distinctvalue(object table, object columnname, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      string column = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(columnname, aObj, attributeTable, eObj, depth));
      TableHeader th = (TableHeader)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CTableHeader, evaluateExpression(table, aObj, attributeTable, eObj, depth));
      return th.DistinctValue(column);
    }

    /// <summary>
    /// Divide value1 by value2.
    /// </summary>
    /// <param name="Value1">A numeric value</param>
    /// <param name="Value2">A non-zero numeric value</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  divide(object Value1, object Value2, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return calculate(Value1, Value2, aObj, attributeTable, eObj, depth, "divide");
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "divide", e.Message + ":" + e.StackTrace);
        t.AddParm(Value1);
        t.AddParm(Value2);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    /// <summary>
    /// Calculates the Employee Months of Service starting in EmployeeStartDate ending on effectiveDate
    /// </summary>
    /// <param name="EmployeeStartDate"></param>
    /// <param name="effectiveDate"></param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  employeemos(object EmployeeStartDate, object effectiveDate, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        DateTime eDate, startDate;
        eDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, evaluateExpression(effectiveDate, aObj, attributeTable, eObj, depth));
        startDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, evaluateExpression(EmployeeStartDate, aObj, attributeTable, eObj, depth));
        return (object)Convert.ToInt32(Math.Abs(Convert.ToInt32(DateDiff(eDate, startDate, "m", 0))));
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "employeemos", e.Message + ":" + e.StackTrace);
        t.AddParm(EmployeeStartDate);
        t.AddParm(effectiveDate);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    /// <summary>
    /// Is this date the last event in a period?
    /// </summary>
    /// <param name="eventDate">Date to Test</param>
    /// <param name="numberofDaysInEvent"></param>
    /// <param name="period">"m", "q", or "y" (Month, Quarter, Year)</param>
    /// <param name="businessDayOnlyYN">Does the event have to be on a business day? (default = true)</param>
    /// <param name="attributeTable"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private object  endofperiodyn(object eventDate, object numberofDaysInPayroll, object period, object businessDayOnlyYN, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      // first we evaluate the parms
      DateTime dtEventDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, evaluateExpression(eventDate, aObj, attributeTable, eObj, depth));
      double dNbrDays = (double)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDouble, evaluateExpression(numberofDaysInPayroll, aObj, attributeTable, eObj, depth));
      if (dNbrDays == 0)
        dNbrDays = 1;

      string sPeriod = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(period, aObj, attributeTable, eObj, depth))).ToLower();
      bool bBusinessOnly = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, evaluateExpression(businessDayOnlyYN, aObj, attributeTable, eObj, depth), true);
      // now we use calulateDate to find the next event date
      string mod = "d" + dNbrDays.ToString();
      //if (bBusinessOnly)
      //  mod = "b" + dNbrDays.ToString();
      DateTime dtTest = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, calculatedate(dtEventDate, mod, true, aObj, attributeTable, eObj, depth));
      // finally, if the next event is in the same period, this is not the end of period. If it is not, then it is
      bool endOfPeriod = ((int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getPeriod, dtEventDate, sPeriod) != (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getPeriod, dtTest, sPeriod));
      return endOfPeriod;
    }

    private object  exp(object Value1, object Value2, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return calculate(Value1, Value2, aObj, attributeTable, eObj, depth, "exponent");
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "exp", e.Message + ":" + e.StackTrace);
        t.AddParm(Value1);
        t.AddParm(Value2);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  format(object type, object Value, object formatString, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        string sType = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(type, aObj, attributeTable, eObj, depth))).ToLower();
        object oValue = evaluateExpression(Value, aObj, attributeTable, eObj, depth);
        string sFormat = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(formatString, aObj, attributeTable, eObj, depth))).ToLower();
        switch (sType)
        {
          case "datetime":
            switch (sFormat)
            {
              case "shortdate":
                return (object)((DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, oValue)).ToShortDateString();
              case "unformatted":
                return (object)(((DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, oValue)).ToString("yyMMdd"));
              default:
                return string.Empty;
            }

          default:
            return string.Empty;
        }
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "format", e.Message + ":" + e.StackTrace);
        t.AddParm(type);
        t.AddParm(Value);
        t.AddParm(formatString);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  iif(object condition, object valueTrue, object valueElse, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      /*
       * This routine mimics the excel If function. It has the following parameters:
       * 
       * Condition:
       *  it parses two kinds of Conditions...
       * 
       *  _Operators: These are intrinsic functions which have two or three parameters, and
       *  whose names always begin with _.
       * 
       *    _if(condition, valueTrue, valueFalse). Recursively calls this function.
       *    _and(condition1, condition2)           Logical AND of both conditions
       *    _or(condition1, condition2)            Logical OR of both conditions
       *    _not(condition)                        Boolean opposite of condition
       *  
       *  AttributeValidationStrings: These are strings that would be valid for the Dictionary
       *  ValidationString, except that they are evaluated as pass instead of fail, and they pull 
       *  their values from an AttributeTable structure of  AttributeName/Value pairs. So any operand
       *  can be a scalar or an attribute name.
       *  
       * TAGAttribute Values can also be the @If function, which allows nesting of conditions
       * 
       *  the attributeList string is an Item formated structure of 
       *  attribute name/value pairs
       */
      try
      {
        return If(condition, valueTrue, valueElse, aObj, attributeTable, eObj, depth);          // and pass that to the method that actually contains the logic
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "iif", e.Message + ":" + e.StackTrace);
        t.AddParm(condition);
        t.AddParm(valueTrue);
        t.AddParm(valueElse);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  isnull(object variable, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      if (variable == null)
        return (object)true;
      try
      {
        bool checkNullFlag = false;
        string sAttributeName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, variable);
        if (sAttributeName.StartsWith(TAGFunctions.ATTRIBUTECHAR)) // is this an @Attribute reference?
        {
          checkNullFlag = true;
          sAttributeName = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripComment, sAttributeName))).Substring(1);  // now get the attribute name
          // if this is in the form @AttributeName, then we return IsNull = true if it is not there
          // This makes this function useful to determine if an attribute is missing without throwing an error
          if (!attributeTable.AttributeIndex.Contains(sAttributeName))
            return true;
        }
        // Note, if we are here, then either it is @Atribute, or it is a funciton. If a function, it will NOT
        // tell you if one of the attributes inside is missing. It will fail in that event with a function error
        object oVariable = evaluateExpression(variable, aObj, attributeTable, eObj, depth);
        if (oVariable == null || oVariable.ToString() == string.Empty)
          return true;
        else
        {
          if (checkNullFlag)  // this was an @Attribute reference, so we check the attribute referenced to see if it is null (useful for strongly typed attributes of non-string types)
          {
            if (attributeTable.AttributeIndex.Contains(sAttributeName))
              return (attributeTable.AttributeIndex[sAttributeName] == null); //attributeTable[sAttributeName].IsNull;
          }
          return false;
        }
      }
      catch (Exception e)
      {
        if (e.Message.Contains("Attribute lookup did not find attribute")) // if the error was an attribute was not found
          return true;          // then we say the variable is null
        else
        {
          TAGExceptionMessage t = new TAGExceptionMessage(className, "isnull", e.Message + ":" + e.StackTrace);
          t.AddParm(variable);
          t.AddParm(attributeTable);
          throw new Exception(e.Message + ":" + e.StackTrace);   // otherwise, we rethrow the message
        }
      }
    }

    private object  ifnull(object variable, object defaultValue, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        bool isNull = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, isnull(variable, aObj, attributeTable, eObj, depth));
        if (isNull)
          return evaluateExpression(defaultValue, aObj, attributeTable, eObj, depth);
        else
          return evaluateExpression(variable, aObj, attributeTable, eObj, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "ifnull", e.Message + ":" + e.StackTrace);
        t.AddParm(variable);
        t.AddParm(defaultValue);
        t.AddParm(attributeTable);
        throw new Exception(e.Message + ":" + e.StackTrace);   // otherwise, we rethrow the message
      }
    }

    private object  isinherited(object attributeName, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      string aName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(attributeName, aObj, attributeTable, eObj, depth));
      if (attributeTable.AttributeIndex.Contains(aName))
        return (Constants.AnyOn(((TAttribute)((TIndexItem)attributeTable.AttributeIndex[aName]).ItemIdx).Flags, EAttributeFlags.IsInherited));
      else
      {
        TAGExceptionMessage tm = new TAGExceptionMessage(className, "isinherited", "Attribute does not exist in this scope");
        tm.AddParm(attributeName);
        tm.AddParm(attributeTable);
        throw new Exception(tm.ToString());
      }
    }

    private object  left(object s1, object len, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        string myString = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(s1, aObj, attributeTable, eObj, depth));
        int length = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, evaluateExpression(len, aObj, attributeTable, eObj, depth));
        if (length > myString.Length)
          length = myString.Length;
        return myString.Substring(0, length);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "left", e.Message + ":" + e.StackTrace);
        t.AddParm(s1);
        t.AddParm(len);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  len(object s1, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      string s = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(s1, aObj, attributeTable, eObj, depth));
      return s.Length;
    }

    //TODO: The following commented routines must be implemented when the tableHeader is working again...
    private object  lookup(object pAttributeName, object pValueTable, object pColumnName, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return lookupinList(pValueTable, pAttributeName, pColumnName, false, aObj, attributeTable, eObj, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "lookup", e.Message + ":" + e.StackTrace);
        t.AddParm(pAttributeName);
        t.AddParm(pValueTable);
        t.AddParm(pColumnName);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  lookuptable(object pAttributeName, object pValueTable, object pColumnName, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return lookupinList(pValueTable, pAttributeName, pColumnName, false, aObj, attributeTable, eObj, depth, TAGFunctions.DATATYPESTRING, true);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "lookuptable", e.Message + ":" + e.StackTrace);
        t.AddParm(pAttributeName);
        t.AddParm(pValueTable);
        t.AddParm(pColumnName);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  lookuprange(object pLookupValue, object pValueTable, object pColumnName, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      /*
       * OK so this routine takes an attribute table and does a lookup nearest. Some rules are:
       * 
       * - all of the lookup values are ALWAYS in first column
       * - both the value we are looking up and the values in the lookup column must be convertible to numbers
       *   and numeric comparison is used
       * - Table must be TableHeader 
       * - the value we are looking for is in column with the name in pColumnName
       * - the "range" part means we find the row whose value is <= the value we are looking for.
       */
      return lookupinList(pValueTable, pLookupValue, pColumnName, true, aObj, attributeTable, eObj, depth, TAGFunctions.DATATYPEDECIMAL, false);
    }

    private object  lookuprangedate(object pAttributeName, object pValueTable, object pColumnName, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return lookupinList(pValueTable, pAttributeName, pColumnName, true, aObj, attributeTable, eObj, depth, TAGFunctions.DATATYPEDATETIME);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "lookuprangedate", e.Message + ":" + e.StackTrace);
        t.AddParm(pAttributeName);
        t.AddParm(pValueTable);
        t.AddParm(pColumnName);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  lookuptablerange(object pAttributeName, object pValueTable, object pColumnName, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return lookupinList(pValueTable, pAttributeName, pColumnName, true, aObj, attributeTable, eObj, depth, TAGFunctions.DATATYPESTRING, true);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "lookuprangedate", e.Message + ":" + e.StackTrace);
        t.AddParm(pAttributeName);
        t.AddParm(pValueTable);
        t.AddParm(pColumnName);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  lookuptablerangedate(object pAttributeName, object pValueTable, object pColumnName, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return lookupinList(pValueTable, pAttributeName, pColumnName, true, aObj, attributeTable, eObj, depth, TAGFunctions.DATATYPEDATETIME, true);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "lookuprangedate", e.Message + ":" + e.StackTrace);
        t.AddParm(pAttributeName);
        t.AddParm(pValueTable);
        t.AddParm(pColumnName);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  maxdate(object pDateList, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return minmax(pDateList, "max", TAGFunctions.DATATYPEDATETIME, aObj, attributeTable, eObj, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "maxdate", e.Message + ":" + e.StackTrace);
        t.AddParm(pDateList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  maxnumber(object pNumberList, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return minmax(pNumberList, "max", TAGFunctions.DATATYPEDECIMAL, aObj, attributeTable, eObj, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "maxnumber", e.Message + ":" + e.StackTrace);
        t.AddParm(pNumberList);
        throw new Exception(t.ToString());
      }
    }

    private object  mid(object s1, object position, object length, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        int pos, len;
        string news1 = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(s1, aObj, attributeTable, eObj, depth));
        if (news1.Length == 0)
          return string.Empty;
        pos = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, evaluateExpression(position, aObj, attributeTable, eObj, depth));
        if (pos < 0)
          pos = 0;
        if (pos > news1.Length - 1)
          pos = news1.Length - 1;
        len = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, evaluateExpression(length, aObj, attributeTable, eObj, depth));
        if (len < 0)
          len = 0;
        if (pos + len > news1.Length)     // if we go past the end of the string
          len = news1.Length - pos;       // then we shorten the len to end at the end of the string
        string sOut = news1.Substring(pos, len);
        return sOut;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "mid", e.Message + ":" + e.StackTrace);
        t.AddParm(s1);
        t.AddParm(position);
        t.AddParm(length);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  mindate(object pDateList, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return minmax(pDateList, "min", TAGFunctions.DATATYPEDATETIME, aObj, attributeTable, eObj, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "mindate", e.Message + ":" + e.StackTrace);
        t.AddParm(pDateList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  minnumber(object pNumberList, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return minmax(pNumberList, "min", TAGFunctions.DATATYPEDECIMAL, aObj, attributeTable, eObj, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "minnumber", e.Message + ":" + e.StackTrace);
        t.AddParm(pNumberList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  multiply(object pNumberList, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        string numberList = evaluateExpression(pNumberList, aObj, attributeTable, eObj, depth).ToString();
        /*
         * first, we strip off parenthesis on the outside if they exist
         */
        numberList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, numberList);
        string[] delimiters = { cLISTSEPARATOR };
        string[] numberArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, numberList, delimiters);
        int arrayLen = numberArray.GetLength(0);
        //string[] numberArray = new string[arrayLen];
        //for (int j = 0; j < arrayLen; j++)
        //  numberArray[j] = evaluateExpression(tokenArray[j], aObj, attributeTable, eObj, depth).ToString();
        decimal dresult = 1;
        for (int i = 0; i < arrayLen; i++)
        {
          object myToken = evaluateExpression(numberArray[i], aObj, attributeTable, eObj, depth);
          if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsNumeric, myToken)) // skip null or empty values or non-numeric values
            dresult *= (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, myToken);
          else   //added by Jack
            dresult = 0;  //added by Jack
        }
        return (object)dresult;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "multiply", e.Message + ":" + e.StackTrace);
        t.AddParm(pNumberList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  not(object condition, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        object oCondition = evaluateExpression(condition, aObj, attributeTable, eObj, depth);
        return !ifCondition(oCondition.ToString(), aObj, attributeTable, eObj, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "not", e.Message + ":" + e.StackTrace);
        t.AddParm(condition);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  or(object conditionList, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        // 5/17/2010 LLA: change so other conditions are not eval'd unless necessary
        string sConditionList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(conditionList, aObj, attributeTable, eObj, depth));
        sConditionList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, sConditionList);
        string[] delimiters = { cLISTSEPARATOR };
        string[] conditionArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, sConditionList, delimiters);
        for (int i = 0; i < conditionArray.GetLength(0); i++)
        {
          if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, ifCondition(conditionArray[i], aObj, attributeTable, eObj, depth))) // if any condition is true
            return (object)true; // then we return false
        }
        return (object)false;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "or", e.Message + ":" + e.StackTrace);
        t.AddParm(conditionList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  payrollnumber(object eventDate, object numberDaysInPayroll, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      DateTime dtEventDate = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, evaluateExpression(eventDate, aObj, attributeTable, eObj, depth));
      int iNbrDays = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, evaluateExpression(numberDaysInPayroll, aObj, attributeTable, eObj, depth));
      if (iNbrDays == 0)
        iNbrDays = 1;
      int iPayrollNumber = 0;
      int iDay = dtEventDate.Day;
      while (iDay > 0)
      {
        iDay -= iNbrDays;
        iPayrollNumber++;
      }
      return (object)iPayrollNumber;
    }

    private object  plandate(object PlanBegin, object EffectiveDate, object Mode, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return Plandate(PlanBegin, EffectiveDate, Mode, aObj, attributeTable, eObj, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "plandate", e.Message + ":" + e.StackTrace);
        t.AddParm(PlanBegin);
        t.AddParm(EffectiveDate);
        t.AddParm(Mode);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  replacestring(object sourceString, object findString, object replaceString, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        string source = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(sourceString, aObj, attributeTable, eObj, depth));
        string find = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(findString, aObj, attributeTable, eObj, depth));
        string replace = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(replaceString, aObj, attributeTable, eObj, depth));
        return source.Replace(find, replace);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "replacestring", e.Message + ":" + e.StackTrace);
        t.AddParm(sourceString);
        t.AddParm(findString);
        t.AddParm(replaceString);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  right(object s1, object len, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        string myString = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(s1, aObj, attributeTable, eObj, depth));
        int iStart = myString.Length - (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, evaluateExpression(len, aObj, attributeTable, eObj, depth));
        if (iStart < 0)
          iStart = 0;
        return myString.Substring(iStart);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "right", e.Message + ":" + e.StackTrace);
        t.AddParm(s1);
        t.AddParm(len);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  round(object pValue, object pPrecision, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return Round(pValue, pPrecision, "round", aObj, attributeTable, eObj, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "round", e.Message + ":" + e.StackTrace);
        t.AddParm(pValue);
        t.AddParm(pPrecision);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  roundtruncate(object pValue, object pPrecision, object pMode, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return Round(pValue, pPrecision, pMode, aObj, attributeTable, eObj, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "roundtruncate", e.Message + ":" + e.StackTrace);
        t.AddParm(pValue);
        t.AddParm(pPrecision);
        t.AddParm(pMode);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  selectcase(object testValue, object caseList, object defaultValue, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        object oTestValue = evaluateExpression(testValue, aObj, attributeTable, eObj, depth);  // the value to compare
        string strCaseList = evaluateExpression(caseList, aObj, attributeTable, eObj, depth).ToString(); // string format at table of comparevalues/returnvalues
        object oDefault = evaluateExpression(defaultValue, aObj, attributeTable, eObj, depth); //resolved value of the default if no tests match
        if (strCaseList.Substring(0, 1) == Convert.ToString(cLEFT) && // is it enclosed in parens?
              strCaseList.Substring(strCaseList.Length - 1, 1) == Convert.ToString(cRIGHT))
          strCaseList = strCaseList.Substring(1, strCaseList.Length - 2);
        string[] delimiters = { cLISTSEPARATOR };
        string[] caseTable = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, strCaseList, delimiters);
        int lenCase = caseTable.GetLength(0);
        for (int i = 0; i < lenCase; i = i + 2)  // tests are every other one
        {
          string condition = oTestValue.ToString() + "==" + caseTable[i].ToString();
          if (ifCondition(condition, aObj, attributeTable, eObj, depth))
            if (i + 1 < lenCase)  // is there a return value after this test value?
              return evaluateExpression(caseTable[i + 1], aObj, attributeTable, eObj, depth);
            else
              return oDefault;
        }
        return oDefault;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "selectcase", e.Message + ":" + e.StackTrace);
        t.AddParm(testValue);
        t.AddParm(caseList);
        t.AddParm(defaultValue);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  selectcondition(object conditionList, object defaultValue, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        string strConditionList = evaluateExpression(conditionList, aObj, attributeTable, eObj, depth).ToString(); // string format at table of comparevalues/returnvalues
        object oDefault = evaluateExpression(defaultValue, aObj, attributeTable, eObj, depth); //resolved value of the default if no tests match
        strConditionList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, strConditionList);
        string[] delimiters = { cLISTSEPARATOR };
        string[] caseTable = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, strConditionList, delimiters);
        int lenCase = caseTable.GetLength(0);
        for (int i = 0; i < lenCase; i = i + 2)  // tests are every other one
        {
          string condition = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(caseTable[i], aObj, attributeTable, eObj, depth));
          condition = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, condition); ;
          if (ifCondition(condition, aObj, attributeTable, eObj, depth))
            if (i + 1 < lenCase)  // is there a return value after this test value?
              return evaluateExpression(caseTable[i + 1], aObj, attributeTable, eObj, depth);
            else
              return oDefault;
        }
        return oDefault;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "selectcondition", e.Message + ":" + e.StackTrace);
        t.AddParm(conditionList);
        t.AddParm(defaultValue);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  subtract(object operand1, object operand2, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        decimal op1 = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(operand1, aObj, attributeTable, eObj, depth));
        decimal op2 = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(operand2, aObj, attributeTable, eObj, depth));
        return (object)(op1 - op2);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "subtract", e.Message + ":" + e.StackTrace);
        t.AddParm(operand1);
        t.AddParm(operand2);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  sum(string pNumberList, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        string numberList = evaluateExpression(pNumberList, aObj, attributeTable, eObj, depth).ToString();
        /*
         * first, we strip off parenthesis on the outside if they exist
         */
        numberList = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.stripParens, numberList);
        string[] delimiters = { cLISTSEPARATOR };
        string[] numberArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, numberList, delimiters);
        int arrayLen = numberArray.GetLength(0);
        //string[] numberArray = new string[arrayLen];
        //for (int j = 0; j < arrayLen; j++)
        //  numberArray[j] = evaluateExpression(tokenArray[j], aObj, attributeTable, eObj, depth).ToString();
        decimal dsum = 0;
        for (int i = 0; i < arrayLen; i++)
        {
          object myToken = evaluateExpression(numberArray[i], aObj, attributeTable, eObj, depth);
          decimal num = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, myToken);
          dsum += num;
        }
        return (object)dsum;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "sum", e.Message + ":" + e.StackTrace);
        t.AddParm(pNumberList);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    //TODO: Fix this when the tableheader is implemented in the new model!
    private object  tablefindreplace(object key, object table, object columnname, object oldvalue, object newvalue, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        if (table != null && table.GetType() == typeof(TableHeader))
        {
          string oKey = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(key, aObj, attributeTable, eObj, depth));
          string oColumn = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(columnname, aObj, attributeTable, eObj, depth));
          TableHeader th = ((TableHeader)table).FindReplace(oKey, oColumn, oldvalue, newvalue);
          return th;
        }
        else
        {
          TAGExceptionMessage tm = new TAGExceptionMessage(className, "tablefindreplace", "Table is not a tableheader");
          tm.AddParm(key);
          tm.AddParm((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, table));
          tm.AddParm(columnname);
          tm.AddParm(oldvalue);
          tm.AddParm(newvalue);
          throw new Exception(tm.ToString());
        }
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "tablefindreplace", e.Message + ":" + e.StackTrace);
        t.AddParm(key);
        t.AddParm(table);
        t.AddParm(columnname);
        t.AddParm(oldvalue);
        t.AddParm(newvalue);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  today(TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      return DateTime.Today;
    }

    //TODO: Fix this when the implementation of the tableHeader is in the new model
    private object  trimtable(object pStringTable, object startColumn, object endColumn, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        object oTable = evaluateExpression(pStringTable, aObj, attributeTable, eObj, depth);
        if (oTable != null && oTable.GetType() == typeof(TableHeader))
        {
          string sStartColumn = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(startColumn, aObj, attributeTable, eObj, depth))).ToLower();
          string sEndColumn = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(endColumn, aObj, attributeTable, eObj, depth))).ToLower();
          TableHeader th = (TableHeader)oTable;
          return th.TrimTable(sStartColumn, sEndColumn);
        }
        else
          throw new Exception("Table parameter is not a TableHeader");
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "trimtable", e.Message + ":" + e.StackTrace);
        t.AddParm(pStringTable);
        t.AddParm(startColumn);
        t.AddParm(endColumn);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object  truncate(object pValue, object pPrecision, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        return Round(pValue, pPrecision, "truncate", aObj, attributeTable, eObj, depth);
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "truncate", e.Message + ":" + e.StackTrace);
        t.AddParm(pValue);
        t.AddParm(pPrecision);
        t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    //TODO: Fix this when the tableheader is implemented in the new model...
    private object  withholding(object Base, object PayrollPerYear, object Multiple, object FlatAmount, object WithholdingTable,
      object TotalExemptionAmount, object TotalCreditAmount, object Threshold, object StandardDeduction, object WithholdingStatus,
      object Mode, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      /*
       * evaluate parms and create variables to calc withholding
       */
      string sWithholdingStatus = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(WithholdingStatus, aObj, attributeTable, eObj, depth))).ToLower();
      string sMode = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(Mode, aObj, attributeTable, eObj, depth))).ToLower();
      string sWithholdingTable = null;
      TableHeader thWithholdingTable = null;
      object oWithholdingTable = evaluateExpression(WithholdingTable, aObj, attributeTable, eObj, depth);
      // if we got a tableheader, then use that instead of a string
      if (oWithholdingTable != null && oWithholdingTable.GetType() == typeof(TableHeader))
      {
        thWithholdingTable = (TableHeader)oWithholdingTable;
        oWithholdingTable = thWithholdingTable;
      }
      else
      {
        sWithholdingTable = ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, oWithholdingTable)).ToLower();
        oWithholdingTable = sWithholdingTable;
      }

      decimal dResult = 0;
      decimal dBase = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(Base, aObj, attributeTable, eObj, depth));
      decimal dPayrollPerYear = (int)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CInt, evaluateExpression(PayrollPerYear, aObj, attributeTable, eObj, depth));
      decimal dMultiple = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(Multiple, aObj, attributeTable, eObj, depth));
      decimal dFlatAmount = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(FlatAmount, aObj, attributeTable, eObj, depth));
      decimal dTotalExemptionAmount = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(TotalExemptionAmount, aObj, attributeTable, eObj, depth));
      decimal dTotalCreditAmount = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(TotalCreditAmount, aObj, attributeTable, eObj, depth));
      decimal dThreshold = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(Threshold, aObj, attributeTable, eObj, depth));
      decimal dStandardDeduction = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, evaluateExpression(StandardDeduction, aObj, attributeTable, eObj, depth));
      decimal dAnnualBase = Math.Max(dBase * dPayrollPerYear - dStandardDeduction - dTotalExemptionAmount, 0);
      decimal dTableAddition = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, lookuprange(dAnnualBase, oWithholdingTable, "flatamount", aObj, attributeTable, eObj, depth));
      decimal dTableMultiple = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, lookuprange(dAnnualBase, oWithholdingTable, "multiple", aObj, attributeTable, eObj, depth));
      decimal dBracketFloor = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, lookuprange(dAnnualBase, oWithholdingTable, "beginbase", aObj, attributeTable, eObj, depth)) - (decimal).01;

      if (sMode == "forward")
      {
        if (dBase <= dThreshold)
          dResult = 0;
        else
          if (sWithholdingStatus != "withhold") // exempt or nowithhold
            dResult = 0;
          else
          {
            //decimal dAnnualBase = dBase * dPayrollPerYear - dTotalExempt - dStandardDeduction;    // calculate annual base
            dResult = ((dAnnualBase - dBracketFloor) * dTableMultiple + dTableAddition - dTotalCreditAmount) / dPayrollPerYear; // calc wh
          }
        dResult = dResult + dFlatAmount + dBase * dMultiple;    //adjust for additional wh
      }
      else
      {
        decimal dAnnualAfterTaxBase = dBase * dPayrollPerYear + dTotalExemptionAmount + dTotalCreditAmount + dStandardDeduction;   // calc base
        dResult = ((dAnnualAfterTaxBase * dPayrollPerYear) + dBracketFloor - dTotalCreditAmount -                         // calc gross??
          (dTotalExemptionAmount + dTableAddition + dStandardDeduction) * dTableMultiple) / (dPayrollPerYear - dPayrollPerYear * dTableMultiple);
        // how do I adjust for additional wh here?
        /*
         * this was from the original OnTime wiki entry
         Result = (Base * Frequency + BracketRevFloor- CreditAmount - (ExemptionAmount + TaxRevAddition + StandardDeduction) * TaxRevMultiplier) / (Frequency - Frequency * TaxRevMultipler
        */
      }
      dResult = Math.Round(dResult, 2);
      return (object)dResult;
    }

    #endregion  private evaluateExpresion (func) functions

    #region private functions

    private object attributeValue(TItem item, TEntity eObj, string attributeName, int depth)
    {
      /*
       * Modified 1/12/2009 by LLA
       * 
       * When an attribute contains an unresolved function, it is not parsing properly in the evaluateExpression 
       * loop, because it does not have the cFUNCTION Prefix. This change is meant to address this problem.
       * Hopefully, it will also make calculation of functions independent of resolution order, so that
       * it calculates in a "natural order" way.
       */
      //TODO lmv66: 2010 0320 Constant Values, could be declared at the class loading
      string c_TABLEHEADER = "tableheader";
      string c_FUNC = "func";
      string c_VALUE = "value";
      object returnValue = null;

      Dictionaries dictionary = TAGFunctions.Dictionary;
      TAttribute attr = item.getAttribute(attributeName, eObj);
      TAttribute attrSolved = null;
      TAttribute aObjContext = null;
      TItem iObjContext = null;

      TIndexItem tmpIdx = null;

      //if (item.AttributeIndex.Contains(attributeName))
      if (attr != null)
      {
        //TAttribute a = item.getAttribute(attributeName);
        if (attr.ValueType == Constants.TABLEHEADER && dictionary != null)
          returnValue = new TableHeader(attributeName, (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, attr.Value), dictionary);
        else if (attr.ValueType == Constants.REF_INHERIT && (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, attr.Value) != "")
        {
          iObjContext = new TItem() { Id = item.Id, OrigId = item.OrigId, Entity = eObj, ItemType = item.ItemType };
          aObjContext = new TAttribute() { Id = attr.Id, OrigId = attr.OrigId, ValueType = attr.ValueType, Item = iObjContext };
          tmpIdx = new TIndexItem() { ItemObj = aObjContext, ItemIdx = attr };

          attrSolved = attr.getSolveAttribute(tmpIdx);

          if (attrSolved != null)
            returnValue = attrSolved.Value;
        }
        else
          returnValue = attr.Value;

        //string compareValueType = a.ValueType.ToLower();
        string compareValueType = attr.ValueType.ToLower();
        if (compareValueType == c_FUNC)
        {
          string strReturn = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, returnValue);
          //a.ReferenceValueSource = strReturn;
          if (!strReturn.StartsWith(cFUNCTION.ToString()))
            strReturn = cFUNCTION + strReturn;  // add func prefix if it is one
          returnValue = evaluateExpression(strReturn, attr, item, eObj, depth);

        }

        return returnValue;
      }
      else
      {
        //return string.Empty;
        TAGExceptionMessage t = new TAGExceptionMessage(className, "attributeValue", "Attribute lookup did not find attribute");
        t.AddParm(attributeName);
        //t.AddParm(item);
        throw new Exception(t.ToString());
      }
    }

    private bool Between(object pTestValue, object pFromValue, object pToValue, string compareType, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      bool isBetween = false;
      object testValue = evaluateExpression(pTestValue, aObj, attributeTable, eObj, depth);
      object fromValue = evaluateExpression(pFromValue, aObj, attributeTable, eObj, depth);
      object toValue = evaluateExpression(pToValue, aObj, attributeTable, eObj, depth);
      switch (compareType.ToLower())
      {
        case TAGFunctions.DATATYPESTRING:
          isBetween = (((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, fromValue)).CompareTo((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, testValue)) <= 0 &&
              ((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, testValue)).CompareTo((string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, toValue)) <= 0);
          break;
        case TAGFunctions.DATATYPEDATETIME:
          isBetween = ((DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, fromValue) <= (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, testValue) &&
            (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, testValue) <= (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, toValue));
          break;
        case TAGFunctions.DATATYPEDECIMAL:
          isBetween = ((decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, fromValue) <= (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, testValue) &&
              (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, testValue) <= (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, toValue));
          break;
      }
      return isBetween;
    }

    private object calculate(object Value1, object Value2, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth, string op)
    {
      object oValue1 = evaluateExpression(Value1, aObj, attributeTable, eObj, depth);
      object oValue2 = evaluateExpression(Value2, aObj, attributeTable, eObj, depth);
      decimal v1, v2;
      try
      {
        v1 = Convert.ToDecimal(oValue1);
      }
      catch
      {
        if (op == "divide")
          v1 = 0;
        else
          v1 = 1;
      }
      try
      {
        v2 = Convert.ToDecimal(oValue2);
      }
      catch
      {
        if (op == "exponent")
          v2 = 0;
        else
          v2 = 1;
      }
      decimal returnValue = 0;
      if (op == "divide")
        if (v2 == 0)  // can't divide by zero
          returnValue = 0;
        else
          returnValue = v1 / v2;
      else
        if (op == "multiply")
          returnValue = v1 * v2;
        else
          if (op == "exponent")
            returnValue = Convert.ToDecimal(Math.Pow(Convert.ToDouble(v1), Convert.ToDouble(v2)));
      return (object)returnValue;
    }

    private TAttribute getRefAttribute(string strRefExpression, TItem attributeTable, TEntity eObj)
    {
      string itemHash = "";
      string attributeHash = "";
      string refIndex2Key = "";

      TEntity refInheritEntity = null;
      TAttribute refAttribute = null;

      try
      {
        string[] reference;
        string[] solvedReference;

        solvedReference = resolveItem(strRefExpression, attributeTable, eObj);

        itemHash = solvedReference[0];
        attributeHash = solvedReference[1];
        refIndex2Key = itemHash + "." + attributeHash;

        reference = itemHash.Split('.');

        refInheritEntity = eObj;

        while (refInheritEntity != null)
        {
          if (refInheritEntity.ItemIndex[itemHash] != null && ((TItem)((TIndexItem)refInheritEntity.ItemIndex[itemHash]).ItemIdx).AttributeIndex[attributeHash] != null)
          {
            refAttribute = (TAttribute)((TIndexItem)((TItem)((TIndexItem)refInheritEntity.ItemIndex[itemHash]).ItemIdx).AttributeIndex[attributeHash]).ItemIdx;
            break;
          }

          refInheritEntity = refInheritEntity.EntityOwner;
          if (refInheritEntity != null)
          {
            itemHash = refInheritEntity.EntityHash + "." + reference[1] + "." + reference[2];
            refIndex2Key = itemHash + "." + attributeHash;
          }
        }

      }
      catch (Exception e)
      {
        //TODO: Something BAD happened!!
        //Need to add logging here
        Console.WriteLine(e.Message);
        Console.WriteLine(e.StackTrace);
      }

      return refAttribute;
    }

    private string[] resolveItem(string reference, TItem iObj, TEntity eObj)
    {
      string defaultItemHash = "";

      string[] ATAT = { "@@" };
      string[] value = new string[2];

      string[] referenceTokens = reference.Split(ATAT, StringSplitOptions.RemoveEmptyEntries);

      for (int i = 0; i < referenceTokens.GetLength(0); i++)
      {
        switch (i)
        {
          case 0:
            referenceTokens[i] = eObj.Id + referenceTokens[i].Substring(Constants.REF_ENTITY.Length - 2);
            break;
          case 1:
            referenceTokens[i] = iObj.Id + referenceTokens[i].Substring(Constants.REF_ENTITYTYPE.Length - 2);
            break;
        }
        value[0] += referenceTokens[i];
      }

      value[1] = value[0].Substring(value[0].LastIndexOf(".") + 1);
      value[0] = value[0].Substring(0, (value[0].Length - value[1].Length - 1));

      //defaultItemHash = Constants.DEFAULT_ENTITY + value[0].Substring(value[0].IndexOf("."));

      //if (((TEntity)SystemEntityFactory.Instance.Entities[0]).ItemIndex.ContainsKey(defaultItemHash))
      //  if (((TItem)((TEntity)SystemEntityFactory.Instance.Entities[0]).ItemIndex[defaultItemHash]).AttributeIndex.ContainsKey(value[1]))
      //    value[0] = defaultItemHash;

      return value;
    }

    private object evaluateFunction(string strExpression, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth, out bool isFinished)
    {
      string[] tokenList = null;
      bool badFunctionCall = false;
      object returnValue = null;
      string functionName = string.Empty;
      string insideExpression;
      isFinished = false;
      functionName = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.getFunctionName, strExpression, out insideExpression);  // look for an @function
      if (functionName == string.Empty)   // this expression does not start with a function or an attribute character
        isFinished = true;    // exit the loop
      else
      {
        tokenList = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, insideExpression); // there is an @ function, so parse the string inside the "()" for tokens
        switch (functionName.ToLower())           // and execute the appropriate function
        {
          case "abs":
            if (tokenList.GetLength(0) == 1)
              returnValue = abs((object)tokenList[0], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "add":
            if (tokenList.GetLength(0) == 2)
              returnValue = add((object)tokenList[0], (object)tokenList[1], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "age":
            if (tokenList.GetLength(0) == 2)
              returnValue = age((object)tokenList[0], (object)tokenList[1], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "and":
            if (tokenList.GetLength(0) == 1)
              returnValue = and((object)tokenList[0], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;

          // TODO: Fix this when the attributedate function is done...
          // TODO: Ask what this function is used for!!
          //case "attributedate":
          //  if (tokenList.GetLength(0) == 3)
          //    returnValue = attributedate((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], aObj, attributeTable, eObj, ++depth);
          //  else
          //  {
          //    badFunctionCall = true;
          //    isFinished = true;
          //    returnValue = (object)string.Empty;
          //  }
          //  break;
          case "between":
            if (tokenList.GetLength(0) == 3)
              returnValue = between((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "betweendate":
            if (tokenList.GetLength(0) == 3)
              returnValue = betweendate((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "betweennumber":
            if (tokenList.GetLength(0) == 3)
              returnValue = betweennumber((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "calculatedate":
            if (tokenList.GetLength(0) == 3)
              returnValue = calculatedate((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "cap":
            if (tokenList.GetLength(0) == 4)
              returnValue = cap((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], (object)tokenList[3], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)((decimal)0);
            }
            break;
          case "coalesce":
            if (tokenList.GetLength(0) == 1)
              returnValue = (object)coalesce(tokenList[0], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "concatenate":
            if (tokenList.GetLength(0) == 1)
              returnValue = (object)concatenate(tokenList[0], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "containslist":
            if (tokenList.GetLength(0) == 2)
              returnValue = containslist((object)tokenList[0], (object)tokenList[1], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "containstable":
            if (tokenList.GetLength(0) == 2)
              returnValue = containslist((object)tokenList[0], (object)tokenList[1], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;

          //TODO: Fix this when the tableheader is implemented...
          case "converttolist":
            if (tokenList.GetLength(0) == 1)
              returnValue = (object)converttolist(tokenList[0], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "dateadd":
            if (tokenList.GetLength(0) == 2)
              returnValue = dateadd((object)tokenList[0], (object)tokenList[1], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "datediff":
            if (tokenList.GetLength(0) == 4)
              returnValue = datediff((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], (object)tokenList[3], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          //TODO: Fix this when the tableheader is implemented...
          case "distinctvalue":
            if (tokenList.GetLength(0) == 2)
              returnValue = distinctvalue((object)tokenList[0], (object)tokenList[1], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "divide":
            if (tokenList.GetLength(0) == 2)
              returnValue = divide((object)tokenList[0], (object)tokenList[1], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "employeemos":
            if (tokenList.GetLength(0) == 2)
              returnValue = employeemos((object)tokenList[0], (object)tokenList[1], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "exp":
            if (tokenList.GetLength(0) == 2)
              returnValue = exp((object)tokenList[0], (object)tokenList[1], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "format":
            if (tokenList.GetLength(0) == 3)
              returnValue = format((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "iif":
            if (tokenList.GetLength(0) == 3)      // does this have the correct number of parameters?
              returnValue = (object)If(tokenList[0], tokenList[1], tokenList[2], aObj, attributeTable, eObj, ++depth); // evaluate the IF and return the results
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "isinherited":
            if (tokenList.GetLength(0) == 1)
              returnValue = isinherited((object)tokenList[0], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "ifnull":
            if (tokenList.GetLength(0) == 2)
              returnValue = ifnull((object)tokenList[0], (object)tokenList[1], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "isnull":
            if (tokenList.GetLength(0) == 1)
              returnValue = isnull((object)tokenList[0], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "left":
            if (tokenList.GetLength(0) == 2)
              returnValue = (object)left((object)tokenList[0], (object)tokenList[1], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "len":
            if (tokenList.GetLength(0) == 1)
              returnValue = (object)len((object)tokenList[0], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)0;
            }
            break;

          //TODO: The lookup routines must be implemented WHEN the tableheader is supported in the new model...
          case "lookup":
            if (tokenList.GetLength(0) == 3)
              returnValue = lookupinList((object)tokenList[1], (object)tokenList[0],
                (object)tokenList[2], false, aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "lookuprange":
            if (tokenList.GetLength(0) == 3)
              returnValue = lookupinList((object)tokenList[1], (object)tokenList[0],
                (object)tokenList[2], true, aObj, attributeTable, eObj, ++depth, TAGFunctions.DATATYPEDECIMAL);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "lookuprangedate":
            if (tokenList.GetLength(0) == 3)
              returnValue = lookupinList((object)tokenList[1], (object)tokenList[0],
                (object)tokenList[2], true, aObj, attributeTable, eObj, ++depth, TAGFunctions.DATATYPEDATETIME);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "lookuptable":
            if (tokenList.GetLength(0) == 3)
              returnValue = lookuptable((object)tokenList[0], (object)tokenList[1],
                (object)tokenList[2], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "lookuptablerange":
            if (tokenList.GetLength(0) == 3)
              returnValue = lookuptablerange((object)tokenList[0], (object)tokenList[1],
                (object)tokenList[2], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "lookuptablerangedate":
            if (tokenList.GetLength(0) == 3)
              returnValue = lookuptablerangedate((object)tokenList[0], (object)tokenList[1],
                (object)tokenList[2], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "maxdate":
            if (tokenList.GetLength(0) == 1)
              returnValue = minmax((object)tokenList[0], "max", TAGFunctions.DATATYPEDATETIME, aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "maxnumber":
            if (tokenList.GetLength(0) == 1)
              returnValue = minmax((object)tokenList[0], "max", TAGFunctions.DATATYPEDECIMAL, aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "mid":
            if (tokenList.GetLength(0) == 3)
              returnValue = (object)mid((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "mindate":
            if (tokenList.GetLength(0) == 1)
              returnValue = minmax((object)tokenList[0], "min", TAGFunctions.DATATYPEDATETIME, aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "minnumber":
            if (tokenList.GetLength(0) == 1)
              returnValue = minmax((object)tokenList[0], "min", TAGFunctions.DATATYPEDECIMAL, aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "multiply":
            if (tokenList.GetLength(0) == 1)
              returnValue = multiply((object)tokenList[0], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "not":
            if (tokenList.GetLength(0) == 1)
              returnValue = (object)!ifCondition(tokenList[0], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "endofperiodyn":
            if (tokenList.GetLength(0) == 4)
              returnValue = endofperiodyn((object)tokenList[0], tokenList[1], tokenList[2], tokenList[3], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "or":
            if (tokenList.GetLength(0) == 1)
              returnValue = or((object)tokenList[0], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)false;
            }
            break;
          case "payrollnumber":
            if (tokenList.GetLength(0) == 2)
              returnValue = (object)(payrollnumber(tokenList[0], tokenList[1], aObj, attributeTable, eObj, ++depth));
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)0;
            }
            break;
          case "plandate":
            if (tokenList.GetLength(0) == 3)
              returnValue = plandate((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "replacestring":
            if (tokenList.GetLength(0) == 3)
              returnValue = (object)replacestring((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "right":
            if (tokenList.GetLength(0) == 2)
              returnValue = (object)right((object)tokenList[0], (object)tokenList[1], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "round":
            if (tokenList.GetLength(0) == 2)
              returnValue = Round((object)tokenList[0], (object)tokenList[1], "round", aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "roundtruncate":
            if (tokenList.GetLength(0) == 3)
              returnValue = Round((object)tokenList[0], (object)tokenList[1],
                tokenList[2].ToString(), aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "selectcase":
            if (tokenList.GetLength(0) == 3)
              returnValue = selectcase((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "selectcondition":
            if (tokenList.GetLength(0) == 2)
              returnValue = selectcondition((object)tokenList[0], (object)tokenList[1], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "subtract":
            if (tokenList.GetLength(0) == 2)
              returnValue = subtract((object)tokenList[0], (object)tokenList[1], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "sum":
            if (tokenList.GetLength(0) == 1)
              returnValue = (object)sum(tokenList[0], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          //TODO: Fix this when the tableheader is implemented...
          case "tablefindreplace":
            if (tokenList.GetLength(0) == 5)
              returnValue = tablefindreplace((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], (object)tokenList[3], (object)tokenList[4], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "trimtable":
            if (tokenList.GetLength(0) == 3)
              returnValue = trimtable((object)tokenList[0], (object)tokenList[1],
                (object)tokenList[2], aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)string.Empty;
            }
            break;
          case "today":
            if (tokenList.GetLength(0) == 0 || tokenList.GetLength(0) == 1 && tokenList[0] == string.Empty)
              returnValue = today(aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)DateTime.Today;
            }
            break;
          case "truncate":
            if (tokenList.GetLength(0) == 2)
              returnValue = Round((object)tokenList[0], (object)tokenList[1], "truncate", aObj, attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          case "withholding":
            if (tokenList.GetLength(0) == 11)
              returnValue = withholding((object)tokenList[0], (object)tokenList[1], (object)tokenList[2], (object)tokenList[3],
                (object)tokenList[4], (object)tokenList[5], (object)tokenList[6], (object)tokenList[7], (object)tokenList[8],
                (object)tokenList[9], (object)tokenList[10], aObj,
                attributeTable, eObj, ++depth);
            else
            {
              badFunctionCall = true;
              isFinished = true;
              returnValue = (object)(decimal)0;
            }
            break;
          default:
            {
              TAGExceptionMessage t1 = new TAGExceptionMessage(className, "evaluateExpression",
                "Unsupported function <" + functionName + "> was called");
              t1.AddParm(strExpression);
              t1.AddParm(attributeTable);
              throw new Exception(t1.ToString());
            }
        }
      }

      if (badFunctionCall)
      {
        TAGExceptionMessage e = new TAGExceptionMessage(className, functionName, "Incorrect number of arguments");
        for (int iParm = 0; iParm < tokenList.GetLength(0); iParm++)
          e.AddParm(tokenList[iParm]);
        throw new Exception(e.ToString());
      }
      return returnValue;
    }

    private object If(object condition, object valueTrue, object valueElse, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      if (ifCondition(condition.ToString(), aObj, attributeTable, eObj, depth))      // Call the method that evaluates the condition into True/False
        return evaluateExpression(valueTrue, aObj, attributeTable, eObj, depth);
      else
        return evaluateExpression(valueElse, aObj, attributeTable, eObj, depth);
    }

    private bool ifCondition(string condition, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      try
      {
        bool isConditionTrue = false;
        string operand1 = string.Empty;
        string op = string.Empty;
        string operand2 = string.Empty;
        //object expression = evaluateExpression(condition,   attributeTable,  depth);
        //Type t = condition.GetType();
        //if (t.Name == TAGFunctions.DATATYPESTRING)
        if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseValidationString, condition, ref operand1, ref op, ref operand2))
        {
          object opLookup1 = evaluateExpression(operand1, aObj, attributeTable, eObj, depth);   // lookup a value for operand1 in the attribute list
          if (op == string.Empty && operand2 == string.Empty)
          {
            isConditionTrue = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CBoolean, opLookup1);
          }
          else
          {
            if (opLookup1 == null)          // if is not found, then it is a scalar
              opLookup1 = operand1;
            object opLookup2 = evaluateExpression(operand2, aObj, attributeTable, eObj, depth); // same for operand 2
            if (opLookup2 == null)
              opLookup2 = operand2;
            isConditionTrue = (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.passesTest, opLookup1, op, opLookup2); // now evaluate the test
          }
        }
        return isConditionTrue;
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "ifCondition", e.Message + ":" + e.StackTrace);
        t.AddParm(condition);
        //t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }
    }

    private object lookupinList(object lookupList, object attributeName, object columnName, bool pickNearest, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      return lookupinList(lookupList, attributeName, columnName, pickNearest, aObj, attributeTable, eObj, depth, TAGFunctions.DATATYPESTRING, false);
    }

    private object lookupinList(object lookupList, object attributeName, object columnName, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth, string dataType)
    {
      return lookupinList(lookupList, attributeName, columnName, false, aObj, attributeTable, eObj, depth, dataType, false);
    }

    private object lookupinList(object lookupList, object attributeName, object columnName, bool pickNearest, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth, string dataType)
    {
      return lookupinList(lookupList, attributeName, columnName, pickNearest, aObj, attributeTable, eObj, depth, dataType, false);
    }

    private object lookupinList(object lookupList, object attributeName, object columnName, bool pickNearest, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth, string dataType, bool returnTable)
    {
      try
      {
        object oList = evaluateExpression(lookupList, aObj, attributeTable, eObj, depth);
        object oLookup = evaluateExpression(attributeName, aObj, attributeTable, eObj, depth);
        object oColumn = evaluateExpression(columnName, aObj, attributeTable, eObj, depth);
        bool goodParms = true;
        string errorMsg = string.Empty;
        // we don't allow any empty parameters. If one is, we throw an error
        if (oList == null || oList.ToString().Length == 0) //() ||)
        {
          errorMsg = "List (lookup table) is missing";
          goodParms = false;
        }
        else
        {
          if (oLookup == null)
          {
            oLookup = string.Empty;
          }
          else
          {
            if (oColumn == null || oColumn.ToString().Length == 0)
            {
              errorMsg = "Column Name is missing";
              goodParms = false;
            }
          }
        }
        if (!goodParms)
        {
          ////return string.Empty;
          TAGExceptionMessage t = new TAGExceptionMessage(className, "LookupXX", errorMsg);
          t.AddParm("List=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, lookupList));
          t.AddParm("Name=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, attributeName));
          t.AddParm("Column=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, columnName));
          t.AddParm("PickNearest=" + pickNearest.ToString());
          t.AddParm("DataType=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, dataType));
          t.AddParm("LookupRange=" + returnTable.ToString());
          t.AddParm(attributeTable);
          throw new Exception(t.ToString());
        }

        // if lookup is a strint tableheader that hasn't been converted to a tableheader class yet, then go ahead and do it
        //if (oList.GetType().Name.ToLower() == TAGFunctions.DATATYPESTRING && (bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.IsTableHeader, oList) && attributeTable.Dictionary != null)
        //  oLookup = new TableHeader(oLookup, oList.ToString(), attributeTable.Dictionary);
        return lookupUsingTableHeader(oLookup, oList, oColumn, pickNearest, returnTable); // skip the rest and use the TableHeader class to perform the lookup
      }
      catch (Exception e)
      {
        TAGExceptionMessage t = new TAGExceptionMessage(className, "LookupXX", e.Message + ":" + e.StackTrace);
        t.AddParm("List=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, lookupList));
        t.AddParm("Name=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, attributeName));
        t.AddParm("Column=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, columnName));
        t.AddParm("PickNearest=" + pickNearest.ToString());
        t.AddParm("DataType=" + (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, dataType));
        t.AddParm("LookupRange=" + returnTable.ToString());
        //t.AddParm(attributeTable);
        throw new Exception(t.ToString());
      }

    }

    private object minmax(object minmaxList, string minormax, string dataType, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      object returnValue = null;
      object oMinMax = evaluateExpression(minmaxList, aObj, attributeTable, eObj, depth);
      string strMinMax = oMinMax.ToString();
      /*
       * first, we strip off parenthesis on the outside of the list if they exist
       */
      if (strMinMax.Substring(0, 1) == Convert.ToString(cLEFT) && strMinMax.Substring(strMinMax.Length - 1, 1) == Convert.ToString(cRIGHT))
        strMinMax = strMinMax.Substring(1, strMinMax.Length - 2);
      string[] delimiters = { cLISTSEPARATOR };
      string[] minmaxArray = (string[])TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.parseString, strMinMax, delimiters);
      bool firstValueSet = false;
      for (int i = 0; i < minmaxArray.GetLength(0); i++)
      {
        object oThisEntry = evaluateExpression(minmaxArray[i], aObj, attributeTable, eObj, depth);
        if (!(oThisEntry == null || oThisEntry.ToString() == string.Empty))  // skip any empty entries
        {
          if (dataType == TAGFunctions.DATATYPEDECIMAL)
          {
            if (!firstValueSet)                   // first time through
            {
              returnValue = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, oThisEntry);   // just set the min/max to the first value in the list
              firstValueSet = true;
            }
            else
            {
              decimal newValue = (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, oThisEntry);
              if (minormax == "min")      // min
              {
                if (newValue < (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, returnValue))
                  returnValue = newValue;
              }
              else                        // max
              {
                if (newValue > (decimal)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDecimal, returnValue))
                  returnValue = newValue;
              }
            }
          }
          else
          {
            if (!firstValueSet)
            {
              returnValue = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, oThisEntry);
              firstValueSet = true;
            }
            else
            {
              DateTime newValue = (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, oThisEntry);
              if (minormax == "min")
              {
                if (newValue < (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, returnValue))
                  returnValue = newValue;
              }
              else
              {
                if (newValue > (DateTime)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CDateTime, returnValue))
                  returnValue = newValue;
              }
            }
          }

        }
      }
      return returnValue;
    }

    private object Plandate(object PlanBegin, object EffectiveDate, object Mode, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      DateTime resultDate = DateTime.Now;
      DateTime efDate;
      string sMode, sPlanBegin;
      efDate = Convert.ToDateTime(evaluateExpression(EffectiveDate, aObj, attributeTable, eObj, depth));
      sMode = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(Mode, aObj, attributeTable, eObj, depth));
      sPlanBegin = (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, evaluateExpression(PlanBegin, aObj, attributeTable, eObj, depth));
      int Slash = sPlanBegin.IndexOf("/");
      Slash = sPlanBegin.IndexOf("/", Slash + 1);
      if (Slash > 1)
      { sPlanBegin = sPlanBegin.Substring(0, Slash); }
      String sEffectiveYear = efDate.Year.ToString();
      String sLastYear = (efDate.Year - 1).ToString();
      String sNextYear = (efDate.Year + 1).ToString();
      resultDate = DateTime.Parse(sPlanBegin + "/" + sEffectiveYear);

      switch (sMode)
      {
        case "EndPlan":
          if (resultDate < efDate)
          {
            resultDate = DateTime.Parse(sPlanBegin + "/" + sNextYear);
          }
          resultDate = resultDate.AddDays(-1);
          break;

        case "BeginPlan":
          if (resultDate > efDate)
          {
            resultDate = DateTime.Parse(sPlanBegin + "/" + sLastYear);
          }
          break;

        case "BeginPlanEnrollment":
          if (resultDate > efDate)
          {
            resultDate = DateTime.Parse(sPlanBegin + "/" + sLastYear);
          }
          break;

        case "PriorMonth":
          resultDate = DateTime.Parse(efDate.Month.ToString() + "/1/" + sEffectiveYear).AddDays(-1);
          break;

        case "NextMonth":
          resultDate = DateTime.Parse(efDate.Month.ToString() + "/1/" + sEffectiveYear).AddMonths(1);
          break;
      }

      return (object)(DateTime)resultDate;
    }

    private object Round(object pValue, object pPrecision, object mode, TAttribute aObj, TItem attributeTable, TEntity eObj, int depth)
    {
      object returnValue;
      object fromValue = evaluateExpression(pValue, aObj, attributeTable, eObj, depth);
      object precision = evaluateExpression(pPrecision, aObj, attributeTable, eObj, depth);
      object oMode = evaluateExpression(mode, aObj, attributeTable, eObj, depth);
      decimal myValue;
      decimal d;
      int exp;
      d = Convert.ToDecimal(fromValue);
      exp = Convert.ToInt32(precision);
      switch (oMode.ToString().ToLower())
      {
        case "round":
          break;
        case "truncate":
          if (d >= 0) //is the value we want to round positive?
            d = d - Convert.ToDecimal((double).5 * Math.Pow((double)10, Convert.ToDouble(exp * -1)));
          else
            d = d + Convert.ToDecimal((double).5 * Math.Pow((double)10, Convert.ToDouble(exp * -1)));
          break;
        case "roundup":
          if (d >= 0)
            d = d + Convert.ToDecimal((double).5 * Math.Pow((double)10, Convert.ToDouble(exp * -1)));
          else
            d = d - Convert.ToDecimal((double).5 * Math.Pow((double)10, Convert.ToDouble(exp * -1)));
          break;
      }
      /*
       * it turns out Math.Round does not support negative exponents, so we have to specially treat
       * the calculation of the exp is negative
       */
      if (exp < 0)  // is this a negative exponent?
      {
        myValue = Convert.ToDecimal(d * Convert.ToDecimal(Math.Pow((double)10, (double)(exp))));  // then move to the left exp places
        myValue = (decimal)Math.Round(myValue, 0);                    // round to the decimal point
        myValue = myValue * Convert.ToDecimal(Math.Pow((double)10, (double)(exp * -1))); // then move back to the right
      }
      else
        myValue = (decimal)Math.Round(d, exp);
      returnValue = (object)myValue;
      return returnValue;
    }

    #endregion private functions

    #endregion AttributeEngine2 overload functions!
  }
}
