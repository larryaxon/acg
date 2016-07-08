using System;
using System.Collections;
using System.Xml;

using TAGBOSS.Common.Model;

namespace TAGBOSS.Sys.AttributeEngine2.ConvertToEAC
{
  class Program
  {
    static void Main(string[] args)
    {
      //generateEAC();
      generateIncludeTree();
    }

    private static void generateEAC() 
    {
      AttributeProcessor attrProc = new AttributeProcessor();
      EntityAttributesCollection eac = null;

      //string entityList = "tagsb-spalding-user";
      //string itemTypeList = "menuoption";
      ////string itemList = "401k-ee, luis";
      //string itemList = null;
      //DateTime effectiveDate = DateTime.Now;

      ////string entityList = "DavaAssoc-Lopez-00013, DavaAssoc-Bowers-00005";
      //string entityList = "DavaAssoc-Bowers-00005";
      //string itemTypeList = "TransCode";
      //string itemList = "401k-ee, luis";
      ////string itemList = null;
      //DateTime effectiveDate = DateTime.Now;

      ////string entityList = "DavaAssoc-Lopez-00013, DavaAssoc-Bowers-00005";
      //string entityList = "Default";
      //string itemTypeList = "Vendor";
      //string itemList = null;
      //DateTime effectiveDate = DateTime.Now;

      //NUNIT:BEGIN Calls testing
      //string itemTypeList = null;
      //string itemList = null;
      //string itemList = "life-united1x50";
      //DateTime effectiveDate = DateTime.Now;

      //string entityList = "Test-RefInherit";
      //string itemTypeList = "Entity";
      //string itemList = "RefInherits";
      //DateTime effectiveDate = DateTime.Now;


      //TransCodeGenerator Error
      string entityList = "Cosmetolog-Cox-00013";
      string itemTypeList = "account, benefit, pcpolicy, region, service, state, taxcredit, union, wc";
      string itemList = null;
      DateTime effectiveDate = DateTime.Now;
      //TransCodeGenerator Error

      Console.WriteLine("BEGIN: EAC generation program: " + DateTime.Now);

      TAGBOSS.Common.TAGFunctions.BypassFunctionError = true;

      if (entityList.ToLower() == Constants.DEFAULT_ENTITY)
        eac = attrProc.getSystemAttributes(entityList, itemTypeList, itemList, null, effectiveDate, false, false);
      else
        eac = attrProc.getAttributes(entityList, itemTypeList, itemList, null, effectiveDate, false, false);

      //Console.WriteLine("BEGIN: EAC XML File Generation: " + DateTime.Now);

      //XmlDocument eacXmlFile = new XmlDocument();
      //eacXmlFile.LoadXml(eac.toXMLEnriched(eac));
      //eacXmlFile.Save(@"C:\Users\luis.TAGPAY\TAGPay\DavaAssocTranscodefromNewAttrEng4_2_3.xml");

      //Console.WriteLine("END: EAC generation program: " + DateTime.Now);
    }

    private static void generateIncludeTree() 
    {
      AttributeProcessor attrProc = new AttributeProcessor();

      //TransCodeGenerator Error
      string entityList = "CBSTech-Barrier-00032";
      string itemTypeList = "TransCode";
      string itemList = null;
      DateTime effectiveDate = DateTime.Now;
      //TransCodeGenerator Error

      Console.WriteLine("BEGIN: EAC generation program: " + DateTime.Now);

      TAGBOSS.Common.TAGFunctions.BypassFunctionError = true;

      TEntity[] eList = attrProc.getItemsIncludeTree(entityList, itemTypeList, itemList, effectiveDate);
    }
  }
}
