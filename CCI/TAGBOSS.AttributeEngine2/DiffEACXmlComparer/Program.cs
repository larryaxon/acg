using System;
using System.Collections;
using System.Xml;

using TAGBOSS.Common.Model;

namespace DiffEACXmlComparer
{
  class Program
  {
    static void Main(string[] args)
    {
      Hashtable eacCompareResultHash = new Hashtable();

      XmlDocument eacLeftXmlFile = new XmlDocument();
      XmlDocument eacRightXmlFile = new XmlDocument();
      
      EntityAttributesCollection eacLeft = new EntityAttributesCollection();
      EntityAttributesCollection eacRight = new EntityAttributesCollection();

      eacLeftXmlFile.Load(@"C:\Users\lmvallejo\TAGPay\DavaAssocTranscodefromNewAttrEng4_2_3.xml");
      eacRightXmlFile.Load(@"C:\Users\lmvallejo\TAGPay\DavaAssocTranscodefromTestApp4_2_3.xml");

      eacLeft = eacLeft.fromXMLtoEAC(eacLeftXmlFile.OuterXml);
      eacRight = eacRight.fromXMLtoEAC(eacRightXmlFile.OuterXml);

      int[] cntEACLeft = { 0, 0, 0, 0, 0, 0 };
      int[] cntEACRight = { 0, 0, 0, 0, 0, 0 };

      foreach (Entity entity in eacLeft.Entities)
      {
        eacCompareResultHash.Add(entity.ID, new CompareResult(){Object1 = entity, Object2 = null, Result = ResultFlag.LEFT, ValueType = ValueType.ENTITY});
        cntEACLeft[0]++;
        cntEACLeft[1]++;
        foreach (ItemType itemType in entity.ItemTypes)
        {
          if (itemType.ID == "entity")
            continue;

          eacCompareResultHash.Add(string.Format("{0}.{1}", entity.ID, itemType.ID), new CompareResult() { Object1 = itemType, Object2 = null, Result = ResultFlag.LEFT, ValueType = ValueType.ITEMTYPE });
          cntEACLeft[0]++;
          cntEACLeft[2]++;
          foreach (Item item in itemType.Items)
          {
            eacCompareResultHash.Add(string.Format("{0}.{1}.{2}", entity.ID, itemType.ID, item.ID), new CompareResult() { Object1 = item, Object2 = null, Result = ResultFlag.LEFT, ValueType = ValueType.ITEM });
            cntEACLeft[0]++;
            cntEACLeft[3]++;
            foreach (TAGAttribute attribute in item.Attributes)
            {
              eacCompareResultHash.Add(string.Format("{0}.{1}.{2}.{3}", entity.ID, itemType.ID, item.ID, attribute.ID), new CompareResult() { Object1 = attribute, Object2 = null, Result = ResultFlag.LEFT, ValueType = ValueType.ATTRIBUTE });
              cntEACLeft[0]++;
              cntEACLeft[4]++;
              foreach (ValueHistory valueHistory in attribute.Values)
              {
                eacCompareResultHash.Add(string.Format("{0}.{1}.{2}.{3}.{4}", entity.ID, itemType.ID, item.ID, attribute.ID, valueHistory.ID), new CompareResult() { Object1 = valueHistory, Object2 = null, Result = ResultFlag.LEFT, ValueType = ValueType.VALUE_HISTORY });
                cntEACLeft[0]++;
                cntEACLeft[5]++;
              }
            }
          }
        }
      }

      foreach (Entity entity in eacRight.Entities)
      {
        if(eacCompareResultHash.ContainsKey(entity.ID))
          eacCompareResultHash[entity.ID] = ((Entity)((CompareResult)eacCompareResultHash[entity.ID]).Object1).CompareResults(entity);
        else
          eacCompareResultHash.Add(entity.ID, new CompareResult(){Object1 = null, Object2 = entity, Result = ResultFlag.RIGHT, ValueType = ValueType.ENTITY, ResultDetails = ""});

        cntEACRight[0]++;
        cntEACRight[1]++;

        foreach (ItemType itemType in entity.ItemTypes)
        {
          if (eacCompareResultHash.ContainsKey(string.Format("{0}.{1}", entity.ID, itemType.ID)))
            eacCompareResultHash[string.Format("{0}.{1}", entity.ID, itemType.ID)] = ((ItemType)((CompareResult)eacCompareResultHash[string.Format("{0}.{1}", entity.ID, itemType.ID)]).Object1).CompareResults(itemType);
          else
            eacCompareResultHash.Add(string.Format("{0}.{1}", entity.ID, itemType.ID), new CompareResult() {Object1 = null, Object2 = itemType, Result = ResultFlag.RIGHT, ValueType = ValueType.ITEMTYPE, ResultDetails = "" });

          cntEACRight[0]++;
          cntEACRight[2]++;

          foreach (Item item in itemType.Items)
          {
            if (eacCompareResultHash.ContainsKey(string.Format("{0}.{1}.{2}", entity.ID, itemType.ID, item.ID)))
              eacCompareResultHash[string.Format("{0}.{1}.{2}", entity.ID, itemType.ID, item.ID)] = ((Item)((CompareResult)eacCompareResultHash[string.Format("{0}.{1}.{2}", entity.ID, itemType.ID, item.ID)]).Object1).CompareResults(item);
            else
              eacCompareResultHash.Add(string.Format("{0}.{1}.{2}", entity.ID, itemType.ID, item.ID), new CompareResult() { Object1 = null, Object2 = item, Result = ResultFlag.RIGHT, ValueType = ValueType.ITEM, ResultDetails = "" });

            cntEACRight[0]++;
            cntEACRight[3]++;

            foreach (TAGAttribute attribute in item.Attributes)
            {
              if (eacCompareResultHash.ContainsKey(string.Format("{0}.{1}.{2}.{3}", entity.ID, itemType.ID, item.ID, attribute.ID)))
                eacCompareResultHash[string.Format("{0}.{1}.{2}.{3}", entity.ID, itemType.ID, item.ID, attribute.ID)] = ((TAGAttribute)((CompareResult)eacCompareResultHash[string.Format("{0}.{1}.{2}.{3}", entity.ID, itemType.ID, item.ID, attribute.ID)]).Object1).CompareResults(attribute);
              else
                eacCompareResultHash.Add(string.Format("{0}.{1}.{2}.{3}", entity.ID, itemType.ID, item.ID, attribute.ID), new CompareResult() { Object1 = null, Object2 = attribute, Result = ResultFlag.RIGHT, ValueType = ValueType.ATTRIBUTE, ResultDetails = "" });

              cntEACRight[0]++;
              cntEACRight[4]++;

              foreach (ValueHistory valueHistory in attribute.Values)
              {
                if (eacCompareResultHash.ContainsKey(string.Format("{0}.{1}.{2}.{3}.{4}", entity.ID, itemType.ID, item.ID, attribute.ID, valueHistory.ID)))
                  eacCompareResultHash[string.Format("{0}.{1}.{2}.{3}.{4}", entity.ID, itemType.ID, item.ID, attribute.ID, valueHistory.ID)] = ((ValueHistory)((CompareResult)eacCompareResultHash[string.Format("{0}.{1}.{2}.{3}.{4}", entity.ID, itemType.ID, item.ID, attribute.ID, valueHistory.ID)]).Object1).CompareResults(valueHistory);
                else
                  eacCompareResultHash.Add(string.Format("{0}.{1}.{2}.{3}.{4}", entity.ID, itemType.ID, item.ID, attribute.ID, valueHistory.ID), new CompareResult() { Object1 = null, Object2 = valueHistory, Result = ResultFlag.RIGHT, ValueType = ValueType.VALUE_HISTORY, ResultDetails = "" });

                cntEACRight[0]++;
                cntEACRight[5]++;
              }
            }
          }
        }
      }

      int[] cntLeftOnly = { 0, 0, 0, 0, 0, 0 };
      int[] cntRightOnly = { 0, 0, 0, 0, 0, 0 };
      int[] cntDiff = { 0, 0, 0, 0, 0, 0 };
      int[] cntEqual = { 0, 0, 0, 0, 0, 0 };

      CompareResult tmpCompareResult = null;
      foreach (DictionaryEntry eacCompareResult in eacCompareResultHash) 
      {
        tmpCompareResult = (CompareResult)eacCompareResult.Value;
        switch (tmpCompareResult.Result)
        {
          case ResultFlag.LEFT:
            Console.Write(" <<== |" + eacCompareResult.Key);
            cntLeftOnly[0]++;
            switch (tmpCompareResult.ValueType)
            {
              case ValueType.ENTITY:
                Console.WriteLine("{" + tmpCompareResult.ResultDetails + "}");
                cntLeftOnly[1]++;
                break;
              case ValueType.ITEMTYPE:
                Console.WriteLine("{" + tmpCompareResult.ResultDetails + "}");
                cntLeftOnly[2]++;
                break;
              case ValueType.ITEM:
                Console.WriteLine("{" + tmpCompareResult.ResultDetails + "}");
                cntLeftOnly[3]++;
                break;
              case ValueType.ATTRIBUTE:
                Console.Write("{" + tmpCompareResult.ResultDetails + "}");
                if (((TAGAttribute)tmpCompareResult.Object1).ValueType == "tableheader")
                {
                  Console.WriteLine("|" + ((TAGAttribute)tmpCompareResult.Object1).ValueType);
                }
                else
                {
                  Console.Write("|" + ((TAGAttribute)tmpCompareResult.Object1).ValueType);
                  Console.Write("|(" + ((TAGAttribute)tmpCompareResult.Object1).Value.ToString());
                  Console.WriteLine(", NULL)");
                }
                cntLeftOnly[4]++;
                break;
              case ValueType.VALUE_HISTORY:
                Console.Write("{" + tmpCompareResult.ResultDetails + "}");
                if (((ValueHistory)tmpCompareResult.Object1).ValueType == "tableheader")
                {
                  Console.WriteLine("|" + ((ValueHistory)tmpCompareResult.Object1).ValueType);
                }
                else
                {
                  Console.Write("|" + ((ValueHistory)tmpCompareResult.Object1).ValueType);
                  Console.Write("|(" + ((ValueHistory)tmpCompareResult.Object1).Value.ToString());
                  Console.WriteLine(", NULL)");
                }
                cntLeftOnly[5]++;
                break;
            }

            break;
          case ResultFlag.RIGHT:
            Console.Write(" ==>> |" + eacCompareResult.Key);
            cntRightOnly[0]++;
            switch (tmpCompareResult.ValueType)
            {
              case ValueType.ENTITY:
                Console.WriteLine("{" + tmpCompareResult.ResultDetails + "}");
                cntRightOnly[1]++;
                break;
              case ValueType.ITEMTYPE:
                Console.WriteLine("{" + tmpCompareResult.ResultDetails + "}");
                cntRightOnly[2]++;
                break;
              case ValueType.ITEM:
                Console.WriteLine("{" + tmpCompareResult.ResultDetails + "}");
                cntRightOnly[3]++;
                break;
              case ValueType.ATTRIBUTE:
                Console.Write("{" + tmpCompareResult.ResultDetails + "}");
                if (((TAGAttribute)tmpCompareResult.Object2).ValueType == "tableheader")
                {
                  Console.WriteLine("|" + ((TAGAttribute)tmpCompareResult.Object2).ValueType);
                }
                else
                {
                  Console.Write("|" + ((TAGAttribute)tmpCompareResult.Object2).ValueType);
                  Console.Write("|(NULL, ");
                  Console.WriteLine(((TAGAttribute)tmpCompareResult.Object2).Value.ToString() + ")");
                }

                cntRightOnly[4]++;
                break;
              case ValueType.VALUE_HISTORY:
                Console.Write("{" + tmpCompareResult.ResultDetails + "}");
                if (((ValueHistory)tmpCompareResult.Object2).ValueType == "tableheader")
                {
                  Console.WriteLine("|" + ((ValueHistory)tmpCompareResult.Object2).ValueType);
                }
                else
                {
                  Console.Write("|" + ((ValueHistory)tmpCompareResult.Object2).ValueType);
                  Console.Write("|(NULL, ");
                  Console.WriteLine(((ValueHistory)tmpCompareResult.Object2).Value.ToString() + ")");
                }
                cntRightOnly[5]++;
                break;
            }
            break;
          case ResultFlag.NOT_EQUAL:
            Console.Write(" <<>> |" + eacCompareResult.Key);
            cntDiff[0]++;
            switch (tmpCompareResult.ValueType)
            {
              case ValueType.ENTITY:
                Console.WriteLine("{" + tmpCompareResult.ResultDetails + "}");
                cntDiff[1]++;
                break;
              case ValueType.ITEMTYPE:
                Console.WriteLine("{" + tmpCompareResult.ResultDetails + "}");
                cntDiff[2]++;
                break;
              case ValueType.ITEM:
                Console.WriteLine("{" + tmpCompareResult.ResultDetails + "}");
                cntDiff[3]++;
                break;
              case ValueType.ATTRIBUTE:
                Console.Write("{" + tmpCompareResult.ResultDetails + "}");
                if (((TAGAttribute)tmpCompareResult.Object1).ValueType == "tableheader")
                {
                  Console.WriteLine("|" + ((TAGAttribute)tmpCompareResult.Object1).ValueType);
                }
                else
                {
                  Console.Write("|" + ((TAGAttribute)tmpCompareResult.Object1).ValueType);
                  Console.Write("|(" + ((TAGAttribute)tmpCompareResult.Object1).Value.ToString());
                  Console.WriteLine(", " + ((TAGAttribute)tmpCompareResult.Object2).Value.ToString() + ")");
                }
                cntDiff[4]++;
                break;
              case ValueType.VALUE_HISTORY:
                Console.Write("{" + tmpCompareResult.ResultDetails + "}");
                if (((ValueHistory)tmpCompareResult.Object1).ValueType == "tableheader")
                {
                  Console.WriteLine("|" + ((ValueHistory)tmpCompareResult.Object1).ValueType);
                }
                else
                {
                  Console.Write("|" + ((ValueHistory)tmpCompareResult.Object1).ValueType);
                  Console.Write("|(" + ((ValueHistory)tmpCompareResult.Object1).Value.ToString());
                  Console.WriteLine(", " + ((ValueHistory)tmpCompareResult.Object2).Value.ToString() + ")");
                }
                cntDiff[5]++;
                break;
            }
            break;
          //case ResultFlag.EQUAL: //The detail is failing!!
          //  cntEqual[0]++;
          //  switch (tmpCompareResult.ValueType)
          //  {
          //    case ValueType.ENTITY:
          //      Console.WriteLine("");
          //      cntEqual[1]++;
          //      break;
          //    case ValueType.ITEMTYPE:
          //      Console.WriteLine("");
          //      cntEqual[2]++;
          //      break;
          //    case ValueType.ITEM:
          //      Console.WriteLine("");
          //      cntEqual[3]++;
          //      break;
          //    case ValueType.ATTRIBUTE:
          //      Console.WriteLine("");
          //      cntEqual[4]++;
          //      break;
          //    case ValueType.VALUE_HISTORY:
          //      Console.WriteLine("");
          //      cntEqual[5]++;
          //      break;
          //  }
          //  break;
        }
      }

      Console.WriteLine("---------------------------------------------------------------------");
      Console.WriteLine("---------------------------------------------------------------------");
      Console.WriteLine("Total objects in EAC Left:" + cntEACLeft[0].ToString());
      Console.WriteLine("---------------------------------------------------------------------");
      Console.WriteLine("Total Entitites in EAC Left:" + cntEACLeft[1].ToString());
      Console.WriteLine("Total ItemTypes in EAC Left:" + cntEACLeft[2].ToString());
      Console.WriteLine("Total Items in EAC Left:" + cntEACLeft[3].ToString());
      Console.WriteLine("Total Attributes in EAC Left:" + cntEACLeft[4].ToString());
      Console.WriteLine("Total ValueHistories in EAC Left:" + cntEACLeft[5].ToString());
      Console.WriteLine("---------------------------------------------------------------------");
      Console.WriteLine("Total objects in EAC Right:" + cntEACRight[0].ToString());
      Console.WriteLine("---------------------------------------------------------------------");
      Console.WriteLine("Total Entitites in EAC Right:" + cntEACRight[1].ToString());
      Console.WriteLine("Total ItemTypes in EAC Right:" + cntEACRight[2].ToString());
      Console.WriteLine("Total Items in EAC Right:" + cntEACRight[3].ToString());
      Console.WriteLine("Total Attributes in EAC Right:" + cntEACRight[4].ToString());
      Console.WriteLine("Total ValueHistories in EAC Right:" + cntEACRight[5].ToString());
      Console.WriteLine("---------------------------------------------------------------------");
      Console.WriteLine("---------------------------------------------------------------------");
      Console.WriteLine("In both and equal:" + cntEqual[0].ToString());
      Console.WriteLine("---------------------------------------------------------------------");
      Console.WriteLine("In both and equal entities:" + cntEqual[1].ToString());
      Console.WriteLine("In both and equal itemtypes:" + cntEqual[2].ToString());
      Console.WriteLine("In both and equal items:" + cntEqual[3].ToString());
      Console.WriteLine("In both and equal attributes:" + cntEqual[4].ToString());
      Console.WriteLine("In both and equal valuehistories:" + cntEqual[5].ToString());
      Console.WriteLine("---------------------------------------------------------------------");
      Console.WriteLine("Only on Left:" + cntLeftOnly[0].ToString());
      Console.WriteLine("---------------------------------------------------------------------");
      Console.WriteLine("Only on Left entities:" + cntLeftOnly[1].ToString());
      Console.WriteLine("Only on Left itemtypes:" + cntLeftOnly[2].ToString());
      Console.WriteLine("Only on Left items:" + cntLeftOnly[3].ToString());
      Console.WriteLine("Only on Left attributes:" + cntLeftOnly[4].ToString());
      Console.WriteLine("Only on Left valuehistories:" + cntLeftOnly[5].ToString());
      Console.WriteLine("---------------------------------------------------------------------");
      Console.WriteLine("Only on Right:" + cntRightOnly[0].ToString());
      Console.WriteLine("---------------------------------------------------------------------");
      Console.WriteLine("Only on Right entities:" + cntRightOnly[1].ToString());
      Console.WriteLine("Only on Right itemtypes:" + cntRightOnly[2].ToString());
      Console.WriteLine("Only on Right items:" + cntRightOnly[3].ToString());
      Console.WriteLine("Only on Right attributes:" + cntRightOnly[4].ToString());
      Console.WriteLine("Only on Right valuehistories:" + cntRightOnly[5].ToString());
      Console.WriteLine("---------------------------------------------------------------------");
      Console.WriteLine("In both but diff:" + cntDiff[0].ToString());
      Console.WriteLine("---------------------------------------------------------------------");
      Console.WriteLine("In both but diff entities:" + cntDiff[1].ToString());
      Console.WriteLine("In both but diff itemtypes:" + cntDiff[2].ToString());
      Console.WriteLine("In both but diff items:" + cntDiff[3].ToString());
      Console.WriteLine("In both but diff attributes:" + cntDiff[4].ToString());
      Console.WriteLine("In both but diff valuehistories:" + cntDiff[5].ToString());
      Console.WriteLine("---------------------------------------------------------------------");
    }
  }
}
