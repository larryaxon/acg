using System;
using System.Collections;

using TAGBOSS.Common;

namespace TAGBOSS.Common.Model
{
  public class TParameterProcessor
  {
    const string END_CHARACTERS = " ,.;:()�?�!|/{}~$%&<>=+-*/"; //\n\t\\\""

    const string ATTR_2_AT_CHAR = "@@";
    const string ATTR_1_AT_CHAR = "@";

    public string getSolvedParameter(string attributeValue, TEntity eObj, TItem itemObjObj, TAttribute aObj)
    {
      //,~)><=+-/
      //first we have a split based on @
      //then each element of the array has a split over the end of token characters (whereever there is one
      //we get rid of the ones that have an initial @, because the @@ will have an @ at the beginning always
      //finally we substitute every one of this with it's corresponding string value
      if (attributeValue == null || attributeValue == "")
        return "";
      else if (!attributeValue.Contains(ATTR_2_AT_CHAR))
        return attributeValue;

      string srcAttributeValue = "";
      string retAttributeValue = "";

      bool inATToken = false;
      string Token = "";
      string currChar = "";
      int maxLoops = 10;
      int cntLoops = 0;
      int i;

      ArrayList splittedFunction = new ArrayList();

      srcAttributeValue = attributeValue;
      while (srcAttributeValue.Contains(ATTR_2_AT_CHAR) && cntLoops < maxLoops) 
      {
        splittedFunction.Clear();
        retAttributeValue = "";
        Token = "";
        i = 0;

        while (i < srcAttributeValue.Length)
        {
          currChar = srcAttributeValue.Substring(i, 1);

          if (currChar == ATTR_1_AT_CHAR)
          {
            inATToken = true;
            if (Token.Length > 0)
              splittedFunction.Add(Token);

            Token = currChar;
            i++;
            while (i < srcAttributeValue.Length)
            {
              currChar = srcAttributeValue.Substring(i, 1);
              if (END_CHARACTERS.Contains(currChar))
              {
                //So we found the END of the @ or @@ token, now let's looks for the token value!
                Token = getTokenValue(Token, eObj, itemObjObj, aObj);

                splittedFunction.Add(Token);
                inATToken = false;
                Token = "";

                //Token = currChar;
                //i++;
                break;
              }
              else
              {
                Token += currChar;
                i++;
              }
            }
          }
          else
          {
            Token += currChar;
            i++;
          }

          if (i >= srcAttributeValue.Length)
          {
            //last token!
            if (inATToken)
            {
              Token = getTokenValue(Token, eObj, itemObjObj, aObj);

              inATToken = false;
            }

            splittedFunction.Add(Token);
          }
        }

        for (i = 0; i < splittedFunction.Count; i++)
        {
          retAttributeValue += (string)splittedFunction[i];
        }

        srcAttributeValue = retAttributeValue;
        cntLoops++;
      }

      return retAttributeValue;
    }

    public string getTokenValue(string TokenName, TEntity eObj, TItem itemObj, TAttribute attrObj)
    {
      TAttribute tmpAttr = null;
      string solvedValue = "";
      string solvedName = "";
      bool allowEmptyTokenValue = TAGFunctions.AllowEmptyParameterATAT;

      if (TokenName.StartsWith(ATTR_2_AT_CHAR))
      {
        //Here we process the @@ function parameters
        solvedName = TokenName.Substring(2).ToLower();

        if (solvedName.Equals(Constants.ATTR_AT_ENTITY))
        {
          solvedValue = eObj.OrigId;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_ENTITYTYPE))
        {
          solvedValue = eObj.EntityType;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_ENTITYOWNER))
        {
          solvedValue = eObj.EntityOwner.OrigId;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_LEGALNAME))
        {
          solvedValue = eObj.LegalName;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_FULLNAME))
        {
          solvedValue = eObj.FullName;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_SHORTNAME))
        {
          allowEmptyTokenValue = true; 
          solvedValue = eObj.ShortName;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_FIRSTNAME))
        {
          allowEmptyTokenValue = true;
          solvedValue = eObj.FirstName;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_MIDDLENAME))
        {
          allowEmptyTokenValue = true;
          solvedValue = eObj.MiddleName;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_ALTERNATENAME))
        {
          allowEmptyTokenValue = true;
          solvedValue = eObj.AlternateName;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_ALTERNATEID))
        {
          allowEmptyTokenValue = true;
          solvedValue = eObj.AlternateID;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_FEIN))
        {
          allowEmptyTokenValue = true;
          solvedValue = eObj.FEIN;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_ENTITYSTARTDATE))
        {
          solvedValue = eObj.StartDate.ToShortDateString();
        }
        else if (solvedName.Equals(Constants.ATTR_AT_ENTITYENDDATE))
        {
          solvedValue = eObj.EndDate.ToShortDateString();
        }
        else if (solvedName.Equals(Constants.ATTR_AT_ITEMTYPE))
        {
          if (itemObj != null)
            solvedValue = itemObj.ItemType.OrigId;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_ITEM))
        {
          if (itemObj != null)
            solvedValue = itemObj.OrigId;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_ATTRIBUTE))
        {
          if (attrObj != null)
            solvedValue = attrObj.OrigId;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_ITEM_STARTDATE))
        {
          if (itemObj != null)
            solvedValue = itemObj.StartDate.ToShortDateString();
        }
        else if (solvedName.Equals(Constants.ATTR_AT_ITEM_ENDDATE))
        {
          if (itemObj != null)
            solvedValue = itemObj.EndDate.ToShortDateString();
        }
        else if (solvedName.Equals(Constants.ATTR_AT_EFFECTIVE_DATE))
        {
          solvedValue = eObj.EffectiveDate.ToShortDateString();
        }
        else if (solvedName.Equals(Constants.ATTR_AT_CLIENT))
        {
          solvedValue = eObj.Client;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_ADDRESS1))
        {
          solvedValue = eObj.Address1;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_ADDRESS2))
        {
          solvedValue = eObj.Address2;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_CITY))
        {
          solvedValue = eObj.City;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_STATE))
        {
          solvedValue = eObj.State;
        }
        else if (solvedName.Equals(Constants.ATTR_AT_ZIP))
        {
          solvedValue = eObj.Zip;
        }
        /*
            public const string ATTR_AT_ADDRESS1 = "address1";
            public const string ATTR_AT_CITY = "city";
            public const string ATTR_AT_STATE = "state";
            public const string ATTR_AT_ZIP = "zip";
         */
        if (solvedValue == string.Empty && !(allowEmptyTokenValue))
          solvedValue = TokenName;
      }
      else
        solvedValue = TokenName;

      return solvedValue;
    }

    /// <summary>
    /// Implementation Notes:
    /// This method receives a string refInherit that points to a SPECIFIC attribute, totally identified by the 4 part refInherit
    /// the two possible parts to be solved are the Entity and the EntityItemType in the Item 
    /// that are tagged with a @@Entity and @@EntityItemType, after solving this two in the context o which
    /// the refInherit is defined we go and use the generated address to look for the attribute and return the value
    /// 
    /// TODO: We need to add logic so if we get a refInherit or function we continue with the proper evaluation for it
    /// so in the end we need to return a Value or TableHeader. We will first add a loop for chained refInherits
    /// then we need to add the logic for solving functions in their corresponding context, this context 
    /// is the Entity on which the function has been found.
    /// 
    /// </summary>
    /// <param name="referenceValue"></param>
    /// <param name="attrObjIdx"></param>
    /// <returns></returns>
    public TAttribute getReference(string referenceValue, TIndexItem attrObjIdxEntry)
    {
      TItem itemObjObj = ((TAttribute)attrObjIdxEntry.ItemObj).Item;
      TEntity eObj = itemObjObj.Entity;

      TIndexItem refAttrSolvedIdxItem = null;
      TAttribute refAttrDefaultSolved = null;
      TAttribute refAttrSolved = null;

      TEntity refInheritEntity = null;

      TItem refItemSolved = null;

      string itemHash = "";
      string attributeHash = "";
      string refIndex2Key = "";
      string refSolvedValueType = "";

      try
      {
        //TODO: this must become a loop, as long as the returned value is a refinherit 
        //we need to loop until we find a Function, Value or TableHeader
        string[] referenceItemHash;
        string[] solvedReferenceItemHash;
        string refFuncToSolve = "";

        TAttribute refFuncSolved = null;
        object refFuncValueSolved = null;

        TAttribute refSolvedATAT = null;
        object refSolvedATATValue = null;
        bool IsRefAttr = false;
        int maxTries = 10;
        int cntTries = 1;

        //We will loop UNTIL we get out of ANY refInherit chain!
        TIndexItem refAttrObjIdxItem = new TIndexItem() { ItemObj = attrObjIdxEntry.ItemObj, ItemIdx = attrObjIdxEntry.ItemIdx };
        string refAttrObjValue = referenceValue;
        do
        {
          //BEGIN: This blocks solves the reference for the current context
          solvedReferenceItemHash = resolveItem(refAttrObjValue, refAttrObjIdxItem);

          itemHash = solvedReferenceItemHash[0];
          attributeHash = solvedReferenceItemHash[1];
          refIndex2Key = itemHash + "." + attributeHash;

          referenceItemHash = itemHash.Split('.');

          if (eObj.ItemIndex.Contains(itemHash))
            refInheritEntity = eObj;
          else
            refInheritEntity = resolveEntity(itemHash, itemObjObj);

          while (refInheritEntity != null)
          {
            refAttrSolvedIdxItem = null;
            refItemSolved = null;
            IsRefAttr = false;

            if (refInheritEntity.ItemIndex[itemHash] != null)
              refItemSolved = (TItem)((TIndexItem)refInheritEntity.ItemIndex[itemHash]).ItemObj;

            if (refItemSolved != null && refItemSolved.AttributeIndex[attributeHash] != null)
            {
              refAttrSolvedIdxItem = (TIndexItem)refItemSolved.AttributeIndex[attributeHash];

              if (refItemSolved.Entity.Id == Constants.DEFAULT_ENTITY)
              {
                if (refAttrDefaultSolved == null)
                  refAttrDefaultSolved = (TAttribute)refAttrSolvedIdxItem.ItemIdx;

                refAttrSolved = null;

                IsRefAttr = (refAttrDefaultSolved.ValueType == Constants.REF_INHERIT);
              }
              else
              {
                refAttrSolved = (TAttribute)refAttrSolvedIdxItem.ItemIdx;
                refAttrDefaultSolved = null;
                
                IsRefAttr = (refAttrSolved.ValueType == Constants.REF_INHERIT);
                break;
              }
            }

            refInheritEntity = refInheritEntity.EntityOwner;
            if (refInheritEntity != null)
            {
              itemHash = refInheritEntity.EntityHash + "." + referenceItemHash[1] + "." + referenceItemHash[2];
              refIndex2Key = itemHash + "." + attributeHash;
            }
          }
          //END: This blocks solves the reference for the current context

          if (IsRefAttr)
          {
            /*
             * lmv66: 06/15/2011 7:42 pm 
             * TODO:
             * We need to change this! now we have access to the idxItemEntry for the attribute that we resolve, if it is pointing to a
             * refInherit attribute too, we need to use the key just recovered to prepare the next iteration! we will review this, since this
             * does not seem to be working properly but for now, I need to solve it when it is NOT a refInherit chain call, like in the nunit
             * that is failing
             */

            //We are in the refInherit chain, let us prepare the next loop call
            //we need to find the CONTEXT for the new reference!
            //for this we locate the Entity that is is pointing too and solved it in that context...

            refAttrObjIdxItem = new TIndexItem() { ItemObj = refAttrSolvedIdxItem.ItemObj, ItemIdx = refAttrSolvedIdxItem.ItemIdx };

            //Then we create the context, unless the refAttrObjValue is empty
            refAttrSolvedIdxItem = null;
            refAttrDefaultSolved = null;
            refAttrSolved = null;

          }

          cntTries++;
        }
        while (IsRefAttr && refAttrObjValue != "" && cntTries < maxTries);

        /*
         * At this point I have my refAttrSolvedIdxItem for the reference
         * This has the aObjIdx and the aObjObj, the reference attribute and context attribute
         * We will be returning the context attribute resolve if neccesary, 
         * and the reference will ALWAYS be used as the seed for solving the value of the context
         */

        if (refAttrSolved == null)
          refAttrSolved = refAttrDefaultSolved;

        //If we got out of the loop and found something valid, then let us 
        //unify the return value into the refSolved
        //if the refSolved is null, this measn than in the inheritance chain 
        //the refDefaultSolved is the only one that existed

        if (refAttrSolved != null && refAttrSolved.Value != null)
        {
          //Since the refAttr is not null then we have the TIndexItem where it came from! so we set the attribute values for reference and context attributes
          TAttribute attrObjObj = (TAttribute)refAttrSolvedIdxItem.ItemObj;
          TAttribute attrObjIdx = (TAttribute)refAttrSolvedIdxItem.ItemIdx;

          //We must solved any @@parameters in the context of the resolved reference!
          /*
           * TODO: 2011 0605 2120: BUG FOUND! This is setting the reference attribute and 
           * THIS IS WRONG for the System Entities, so, since we really just want to resolve 
           * this Attribute reference, we are going to create a FRESH ONE, 
           * we were supposed to use the Context Attribute but that change would take 
           * too much time so we will create a new attribute, as with the function, 
           * and we will return this new attribute.
           * 
           * TODO: Will need to review this again! so as to REALLY update the CONTEXT attribute and not create a fresh new one!!
           */
          if (refAttrSolved.ValueType != Constants.TABLEHEADER && !Constants.AnyOn(attrObjObj.Flags, EAttributeFlags.IsAtAtEvaluated) && ((string)refAttrSolved.Value).Contains(ATTR_2_AT_CHAR)) 
          {
            attrObjObj.Value = getSolvedParameter((string)refAttrSolved.Value, eObj, refItemSolved, refAttrSolved);
            attrObjObj.Flags = Constants.SetOn(attrObjObj.Flags, EAttributeFlags.IsAtAtEvaluated);
          }

          refAttrSolved = attrObjObj;

          if (Constants.AnyOn(refAttrSolved.Flags, EAttributeFlags.IsFunctionValue) && !Constants.AnyOn(refAttrSolved.Flags, EAttributeFlags.IsFunctionEvaluated))
          {
            //TODO:Here we must see if it is a function! if it is we must evaluate it in the context of the attribute.Item
            //We also need to check IF the final returned value IS a Value or a TableHeader
            //For this we need to LOAD a dictionary and look for this attribute in the corresponding dictionary
            //for now we will return this as a VALUE and the program caliing this must check if it is a valid Value
            //or if it most convert it to a TableHeader...

            refFuncToSolve = (string)refAttrSolved.Value;

            if (refFuncToSolve != null && refFuncToSolve != string.Empty && !(refFuncToSolve.Contains(ATTR_2_AT_CHAR)))
            {
              refFuncValueSolved = TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.processFunction, refFuncToSolve, refAttrSolved, refAttrSolved.Item, eObj);
              refFuncValueSolved = (refFuncValueSolved != null ? refFuncValueSolved : string.Empty);
              refAttrSolved.Value = refFuncValueSolved;
              refAttrSolved.ValueType = (refFuncValueSolved.ToString().StartsWith("(") && refFuncValueSolved.ToString().EndsWith(")") ? Constants.TABLEHEADER : Constants.VALUE);
              refAttrSolved.Flags = Constants.SetOn(refAttrSolved.Flags, EAttributeFlags.IsFunctionEvaluated);
            }
            else
            {
              refFuncValueSolved = (refFuncToSolve != null ? refFuncToSolve : string.Empty);
              refAttrSolved.Value = refFuncValueSolved;
              refAttrSolved.ValueType = Constants.FUNCTION;
            }
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

      return refAttrSolved;
    }

    private string[] resolveItem(string reference, TIndexItem attrObjIdxEntry)
    {
      TItem iObjObj = ((TAttribute)attrObjIdxEntry.ItemObj).Item;
      TEntity eObj = iObjObj.Entity;

      string defaultItemHash = "";
      string contextItemHash = "";

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
            referenceTokens[i] = eObj.EntityType.ToLower() + referenceTokens[i].Substring(Constants.REF_ENTITYTYPE.Length - 2);
            break;
        }
        value[0] += referenceTokens[i];
      }

      value[1] = value[0].Substring(value[0].LastIndexOf(".") + 1);
      value[0] = value[0].Substring(0, (value[0].Length - value[1].Length - 1));

      contextItemHash = eObj.Id + value[0].Substring(value[0].IndexOf("."));
      if (eObj.ItemIndex.Contains(contextItemHash))
      {
        if (((TItem)((TIndexItem)eObj.ItemIndex[contextItemHash]).ItemIdx).AttributeIndex.Contains(value[1]))
          value[0] = contextItemHash;
      }
      else
      {
        defaultItemHash = Constants.DEFAULT_ENTITY + value[0].Substring(value[0].IndexOf("."));
        if (((TEntity)SystemEntityFactory.Instance.getEntity(Constants.DEFAULT_ENTITY, eObj.EffectiveDate)).ItemIndex.Contains(defaultItemHash))
          if (((TItem)((TIndexItem)((TEntity)SystemEntityFactory.Instance.getEntity(Constants.DEFAULT_ENTITY, eObj.EffectiveDate)).ItemIndex[defaultItemHash]).ItemIdx).AttributeIndex.Contains(value[1]))
            value[0] = defaultItemHash;
      }

      return value;
    }

    private TEntity resolveEntity(string reference, TItem iObj)
    {
      if (reference.StartsWith(Constants.DEFAULT_ENTITY))
        return SystemEntityFactory.Instance.getEntity(Constants.DEFAULT_ENTITY, iObj.Entity.EffectiveDate);
      else
      {
        if (iObj == null)
          return null;
        else
          return iObj.Entity;
      }
    }
  }
}
