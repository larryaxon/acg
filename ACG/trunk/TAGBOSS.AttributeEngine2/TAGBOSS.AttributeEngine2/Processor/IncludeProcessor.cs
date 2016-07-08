using System;
using System.Collections;

using TAGBOSS.Common.Model;

namespace TAGBOSS.Sys.AttributeEngine2.Processor
{
  //TODO: We most call log4net for INVALID includes...
  public class IncludeProcessor
  {
    public ProcessorDelegate CreateDelegate(TIndexItem iIdxItem)
    {
      TItem targetItemIdx = (TItem)iIdxItem.ItemIdx;
      TItem targetItemObj = (TItem)iIdxItem.ItemObj;

      string defaultEntityItemHash = Constants.DEFAULT_ENTITY + "." + targetItemObj.ItemType.Id + "." + targetItemObj.Id;
      string currentEntityDefaultItemHash = targetItemObj.Entity.Id + "." + targetItemObj.ItemType.Id + "." + Constants.DEFAULT_ITEM;
      string defaultEntityDefaultItemHash = Constants.DEFAULT_ENTITY + "." + targetItemObj.ItemType.Id + "." + Constants.DEFAULT_ITEM;

      //TIndexItem currAttrIdxItem = null;
      TIndexItem attrObjIdxItem = null;
      TAttribute attrObjObj = null;

      ProcessorDelegate includes = new ProcessorDelegate();
      includes.Process = delegate
      {
        try
        {
          Hashtable processedIncludes = new Hashtable();
          Hashtable defaultInclude = new Hashtable();

          TIndexItem includeObjIdxItem = null;
          TIncludeItem includeItemObj = null;

          TItem includeItem = null;

          TAttribute includeAttributeObj = null;
          TAttribute targetAttrIdx = null;
          TAttribute targetAttrObj = null;

          string includeAttributeItemHash = "";
          int i = 0;

          while (i < targetItemObj.Includes.Count)
          {
            includeItemObj = (TIncludeItem)targetItemObj.Includes[i];

            //includeObj = resolveItem(includeItemObj.IncludeHash, targetItemIdx);
            includeObjIdxItem = resolveItem(includeItemObj.IncludeHash, targetItemObj);
            if (includeObjIdxItem != null)
              includeItem = (TItem)includeObjIdxItem.ItemIdx;
            else
              includeItem = null;

            //We do not process an include item more than once
            if (includeItem != null && includeItem.Attributes != null && !(processedIncludes.ContainsKey(includeItemObj.IncludeHash)))
            {
              //Now we will process the INCLUDED item attributes
              for (int j = 0; j < Constants.MaxEntityAttributes; j++)
              {
                //this variable is a reference to the INCLUDED item attribute
                includeAttributeObj = includeItem.Attributes[j];

                if (includeAttributeObj == null)
                  break;

                //Let us see if this attribute already exists in the targetItem
                if (targetItemObj.AttributeIndex.Contains(includeAttributeObj.AttributeHash))
                {
                  attrObjIdxItem = targetItemObj.AttributeIndex[includeAttributeObj.AttributeHash];
                  targetAttrIdx = (TAttribute)attrObjIdxItem.ItemIdx;
                  targetAttrObj = (TAttribute)attrObjIdxItem.ItemObj;
                }
                else
                {
                  attrObjIdxItem = null;
                  targetAttrIdx = null;
                  targetAttrObj = null;
                }

                //If the tmpAttribute is an include, let us add it to the Includes collection of the targetItem being processed
                if (Constants.AnyOn(includeAttributeObj.Flags, EAttributeFlags.IsAnInclude))
                {
                  includeAttributeItemHash = includeAttributeObj.Value.ToString().Trim().ToLower();

                  //Each include is only added once
                  if (includeAttributeObj.Value != null && includeAttributeItemHash != "" && !processedIncludes.ContainsKey(includeAttributeItemHash))
                    //So we have another include, we add it at the end of the list! so as to be processed after the ones that we have
                    targetItemObj.Includes.Insert(
                      i + 1,
                      new TIncludeItem()
                      {
                        IncludeSource = includeItem.ItemHash,
                        IncludeHash = includeAttributeItemHash,
                        IncludeDepth = includeItemObj.IncludeDepth + 1,
                        DefaultInclude = false
                      });
                }
                //includeAttributeIdx is NOT an Include, so we add it to the AttributeIndex of targetItem, if it is NOT there yet
                else if (!(targetItemObj.AttributeIndex.Contains(includeAttributeObj.AttributeHash)))
                {

                  /* TODO!!
                   * This is part of the code we need to review carefully! because here we create a context Attribute, at this level we must have a context attribute available
                   * since we are referencing a includeAttribute, that includeAttribute must have already a contextAttribute accompaining it, but maybe we still need
                   * to review, since the context where I am adding this attribute may NOT be the same the includeAttribute comes from! So we need to review this creation
                   * with a microscope and see if it is rightly created...
                   */
                  attrObjObj = (TAttribute)includeAttributeObj.Clone();
                  attrObjObj.IncludeDepth = includeItemObj.IncludeDepth;
                  attrObjObj.Item = targetItemObj;

                  if (includeItemObj.IncludeHash != defaultEntityItemHash
                    && includeItemObj.IncludeHash != currentEntityDefaultItemHash
                    && includeItemObj.IncludeHash != defaultEntityDefaultItemHash)
                    //So this is a valid included attribute, not one from the "includes" added because of Default inheritance
                    attrObjObj.Flags = Constants.SetOn(attrObjObj.Flags, EAttributeFlags.IsIncluded);

                  if (Constants.AnyOn(includeAttributeObj.Flags, EAttributeFlags.FromDefaultEntity))
                    attrObjObj.Flags = Constants.SetOn(attrObjObj.Flags, EAttributeFlags.FromDefaultEntity);

                  if (Constants.AnyOn(includeAttributeObj.Flags, EAttributeFlags.FromDefaultItem))
                    attrObjObj.Flags = Constants.SetOn(attrObjObj.Flags, EAttributeFlags.FromDefaultItem);

                  targetItemObj.AttributeIndex.Add(new TIndexItem() { ItemObj = attrObjObj, ItemIdx = includeAttributeObj });
                }
                //If we get here this means that the AttributeIndex contains the attribute already, 
                //The following "if" will be executed only if tmpAttribute is NOT an Include NOR a TableHeader, 
                //so let's see if we have to replace the targetAttr, 
                //this will be the case IF the current attribute comes from a Default Item, and tmpAttribute doesn't!
                else if (targetAttrObj.ValueHistory != null && targetAttrObj.ValueHistory.ValueType != Constants.TABLEHEADER)
                {
                  //if (targetItem.ItemHash != includeSource && includeAttribute.Value != null && (string)includeAttribute.Value != "")
                  if (includeAttributeObj.Value != null && ((string)includeAttributeObj.Value).Trim() != "" && includeItem.Id != Constants.DEFAULT_ITEM)
                  {
                    if (includeItemObj.IncludeDepth < targetAttrObj.IncludeDepth || targetAttrObj.Value == null || ((string)targetAttrObj.Value).Trim() == "")
                    {
                      attrObjObj = (TAttribute)includeAttributeObj.Clone();
                      attrObjObj.Item = targetItemObj;

                      if (includeItemObj.IncludeHash != defaultEntityItemHash
                        && includeItemObj.IncludeHash != currentEntityDefaultItemHash
                        && includeItemObj.IncludeHash != defaultEntityDefaultItemHash)
                        //So this is a valid included attribute, not one from the "includes" added because of Default inheritance
                        attrObjObj.Flags = Constants.SetOn(attrObjObj.Flags, EAttributeFlags.IsIncluded);

                      if (Constants.AnyOn(includeAttributeObj.Flags, EAttributeFlags.FromDefaultEntity))
                        attrObjObj.Flags = Constants.SetOn(attrObjObj.Flags, EAttributeFlags.FromDefaultEntity);

                      if (Constants.AnyOn(includeAttributeObj.Flags, EAttributeFlags.FromDefaultItem))
                        attrObjObj.Flags = Constants.SetOn(attrObjObj.Flags, EAttributeFlags.FromDefaultItem);

                      attrObjIdxItem.ItemObj = attrObjObj;
                      attrObjIdxItem.ItemIdx = includeAttributeObj;
                    }
                  }
                }
                /*
                 * If we get to this point then:
                 * tmpAttribute is NOT an Include AND already exists in the AttributeIndex of the targetItem
                 * however if this is a TABLEHEADER attribute of the targetItem, 
                 * so the tmpAttribute is added to the collection of related attributes, for tableHeader merge
                 * Since the tableHeader MERGING is CONTEXTUAL, we will do this by CREATING an Attribute
                 * in the Context Item for this entry, so as to have the related attribute list DEPENDING on the
                 * Context, that is, on the PLACE in the TREE of Inheritance where the Attribute is located
                 */
                else if (targetAttrObj.ValueHistory != null && targetAttrObj.ValueHistory.ValueType == Constants.TABLEHEADER)
                {
                  //Since we are here! this context MUST exist in the targetItemObj...
                  attrObjObj = (TAttribute)targetItemObj.AttributeIndex[includeAttributeObj.AttributeHash].ItemObj;

                  TAttribute relatedAttrObj = 
                    new TAttribute()
                    {
                      Id = includeAttributeObj.Id,
                      OrigId = includeAttributeObj.OrigId,
                      IncludeDepth = includeItemObj.IncludeDepth,
                      Item = targetItemObj
                    };

                  if (includeItemObj.IncludeHash != defaultEntityItemHash
                    && includeItemObj.IncludeHash != currentEntityDefaultItemHash
                    && includeItemObj.IncludeHash != defaultEntityDefaultItemHash)
                    //So this is a valid included attribute, not one from the "includes" added because of Default inheritance
                    relatedAttrObj.Flags = Constants.SetOn(relatedAttrObj.Flags, EAttributeFlags.IsIncluded);

                  if (Constants.AnyOn(includeAttributeObj.Flags, EAttributeFlags.FromDefaultEntity))
                    relatedAttrObj.Flags = Constants.SetOn(relatedAttrObj.Flags, EAttributeFlags.FromDefaultEntity);

                  if (Constants.AnyOn(includeAttributeObj.Flags, EAttributeFlags.FromDefaultItem))
                    relatedAttrObj.Flags = Constants.SetOn(relatedAttrObj.Flags, EAttributeFlags.FromDefaultItem);

                  if (!attrObjObj.RelatedAttributes.Contains(includeAttributeObj.Item.ItemHash + "." + includeAttributeObj.AttributeHash))
                    if (includeAttributeObj.Value != null && (string)includeAttributeObj.Value != "")
                      attrObjObj.RelatedAttributes.Add(new TIndexItem { ItemObj = relatedAttrObj, ItemIdx = includeAttributeObj, IsRelatedAttribute = true });

                  //So since the TH already exists, we set the IsIncluded flag to true, if it is not setted yet!
                  targetAttrObj.Flags = Constants.SetOn(targetAttrObj.Flags, EAttributeFlags.IsIncluded);
                }
              }

              //So we have ended with this include, let's add it to the processedIncludes so as NOT to process it more than once!
              processedIncludes.Add(includeItemObj.IncludeHash, null);
            }
            else
            {
              //TODO: Report an INVALID include!!
              //Need to add logging here
            }

            i++;
          }
        }
        catch (Exception e)
        {
          //TODO: Something BAD happened!!
          //Need to add logging here
          Console.WriteLine(e.Message);
          Console.WriteLine(e.StackTrace);
        }
      };

      return includes;
    }

    private TIndexItem resolveItem(string include, TItem targetItem)
    {
      //If the targetItem is NOT in the Default Entity, and the include IS IN the Default entity, we get the resolveItem from the SystemEntity collection
      if (targetItem.Entity.Id != Constants.DEFAULT_ENTITY && include.StartsWith(Constants.DEFAULT_ENTITY))
      {
        if (SystemEntityFactory.Instance.getEntity(Constants.DEFAULT_ENTITY, targetItem.Entity.EffectiveDate).ItemIndex.Contains(include))
          return (TIndexItem)SystemEntityFactory.Instance.getEntity(Constants.DEFAULT_ENTITY, targetItem.Entity.EffectiveDate).ItemIndex[include];
        else
          return null;
      }
      //This resolveItem comes from the Entity in which the TargetItem resides
      else
      {
        if (targetItem.Entity.ItemIndex.Contains(include))
          return (TIndexItem)targetItem.Entity.ItemIndex[include];
        else
          return null;
      }
    }

  }
}
