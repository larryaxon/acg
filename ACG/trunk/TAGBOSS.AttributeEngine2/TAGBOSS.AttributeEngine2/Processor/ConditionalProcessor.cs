using System;
using System.Collections;

using TAGBOSS.Common;
using TAGBOSS.Common.Model;

namespace TAGBOSS.Sys.AttributeEngine2.Processor
{
  class ConditionalProcessor
  {
    public ProcessorDelegate CreateDelegate(TEntity targetEntity)
    {
      ProcessorDelegate conditional = new ProcessorDelegate();
      conditional.Process = delegate
      {
        TItem targetItemIdx = null;
        TItem targetItemObj = null;
        TItem condition = null;

        TAttribute cAttrIdx = null;
        TAttribute cAttrObj = null;

        string cAttrHash = "";
        int index = -1;

        try
        {
          foreach (TIndexItem idxItem in targetEntity.ConditionalIndex) 
          {
            targetItemIdx = (TItem)((TIndexItem)idxItem.ItemIdx).ItemIdx;
            targetItemObj = (TItem)((TIndexItem)idxItem.ItemIdx).ItemObj;
            condition = (TItem)idxItem.ItemObj;

            if (!targetEntity.ItemIndex.Contains(targetItemIdx.ItemHash))
            {
              if (index == -1)
                index = getLastIndex(targetEntity.Items);
              else
                index++;

              targetEntity.Items[index] = targetItemIdx;
              targetEntity.ItemIndex.Add(new TIndexItem() { ItemObj = targetItemObj, ItemIdx = targetItemIdx });
            }

            //TODO: Here is where we evaluate the condition and, if TRUE, add the corresponding attributes to the attributeIndex of the tmpItem
            //When true the attributes of the conditional item must be passed to the targetItem as long as they don't already exist there
            //since the include processor has been processed before this, if the item already exists we check to see if it comes from the Default Item
            //if this is the case, we replace the Default Item Attribute with the one in the Conditional Item, because this takes precedence of  any 
            //Attribute with the same Id in the Default Item.
            if ((bool)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.processDefaultItem, targetItemObj, targetEntity, condition.Id))
            {
              foreach (TIndexItem cattrIdx in condition.AttributeIndex)
              {
                cAttrHash = cattrIdx.ItemHash;
                
                cAttrIdx = (TAttribute)cattrIdx.ItemIdx;
                cAttrObj = (TAttribute)((TAttribute)cattrIdx.ItemObj).Clone();
                cAttrObj.Item = targetItemObj;

                if (!targetItemObj.AttributeIndex.Contains(cAttrHash))
                  targetItemObj.AttributeIndex.Add(new TIndexItem() { ItemObj = cAttrObj, ItemIdx = cAttrIdx });
                else
                {
                  if (((TAttribute)((TIndexItem)targetItemObj.AttributeIndex[cAttrHash]).ItemIdx).Item.ItemHash.EndsWith(Constants.DEFAULT_ITEM)) 
                  {
                    targetItemObj.AttributeIndex[cAttrHash].ItemObj = cAttrObj;
                    targetItemObj.AttributeIndex[cAttrHash].ItemIdx = cAttrIdx;
                  }
                }
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
      };

      return conditional;
    }

    private int getLastIndex(TItem[] items)
    {
      int value = 0;
      for (; value < Constants.MaxEntityItems; value++)
        if (items[value] == null)
          break;

      return value;
    }
  }
}
