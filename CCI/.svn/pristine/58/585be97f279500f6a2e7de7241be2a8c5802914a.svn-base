using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CCI.NUNITs
{
  
  [TestFixture]
  public class CCIServerTests
  {
    CCI.Common.SecurityContext securityContext = new CCI.Common.SecurityContext();
    CCI.Sys.Data.DataSource dataSource1 = new CCI.Sys.Data.DataSource();

    CCI.Sys.Server.CCIServer server1 = new CCI.Sys.Server.CCIServer();
    CCI.Common.ServerResponse response1 = null;
    
    int securityID = -1;

    [TestFixtureSetUp]
    public void initService()
    {
      securityContext.Login = "LarryA";
      securityContext.Password = "jitl5485";
      securityContext = dataSource1.Login(securityContext);
      securityID = securityContext.SecurityID;
    }

    [Test]
    public void OrderTests()
    {
      /*
      * TODO: 
      */
      string responseUpdate, responseGetHeader, responseGetDetail;

      CCI.Common.ServerRequest request = new Common.ServerRequest();
      request.SecurityID = securityID;

      string orderID = string.Empty;
      string customerID = string.Empty;
      string detailID = string.Empty;

      //Empty Order Header Data, no Detail!!
      //request.Form.Add("Header.ContractTerm", "36");
      //request.Form.Add("Header.Comment", "Added Header Order!!!");
      //request.Form.Add("Header.InstallationCosts", "Yes We have!!");
      //request.Form.Add("Header.Location", "City One Park");
      //request.Form.Add("Header.ShortName", "CarlosE");

      //request.Form.Add("Header.CreditCardName", "VISA");
      //request.Form.Add("Header.CreditCardNumber", "0245 1252 125358");
      //request.Form.Add("Header.ExpirationDate", "01/14");
      //request.Form.Add("Header.SecurityCode", "1825");
      //request.Form.Add("Header.AmountToPay", "12500.58");

      //request.Form.Add("Header.PhonesFrom", "Phones From Create");
      //request.Form.Add("Header.CarrierServices", "Carrier Services Create");

      //request.Form.Add("Header.LegalName", "Carlos Henriquez");
      //request.Form.Add("Header.Address1", "City One Park");
      //request.Form.Add("Header.City", "Los Angeles");
      //request.Form.Add("Header.State", "CA");
      //request.Form.Add("Header.Zip", "999898");

      ////response1 = server1.Execute(CCI.Common.ServerCommands.UpdateOrderDetail, request);

      //orderID = CCI.Common.CommonFunctions.CInt(((CCI.Common.CCITable)response1.Results[0])[0, CCI.Common.CommonData.fieldID]).ToString();
      //customerID = CCI.Common.CommonFunctions.CString(((CCI.Common.CCITable)response1.Results[0])[0, CCI.Common.CommonData.fieldCUSTOMER]);

      //detailID = string.Empty;

      //request = new Common.ServerRequest();
      //request.SecurityID = securityID;

      ////Empty Order Header Data, no Detail!!
      //request.Form.Add("Header.ID", orderID);
      //request.Form.Add("Header.Customer", customerID);
      //request.Form.Add("Header.ContractTerm", "48");
      //request.Form.Add("Header.ShortName", "CarlosE2");

      ////request.Form.Add("Header.CreditCardName", "VISA");
      ////request.Form.Add("Header.CreditCardNumber", "0245 1252 125358");
      ////request.Form.Add("Header.ExpirationDate", "01/14");
      ////request.Form.Add("Header.SecurityCode", "1825");
      ////request.Form.Add("Header.AmountToPay", "12500.58");

      //request.Form.Add("Header.PhonesFrom", "Phones From Update");
      //request.Form.Add("Header.CarrierServices", "Carrier Services Update");

      //request.Form.Add("Header.LegalName", "Carlos2 Henriquez");
      //request.Form.Add("Header.Address1", "City Two Park");
      //request.Form.Add("Header.City", "New York");

      //response1 = server1.Execute(CCI.Common.ServerCommands.UpdateOrderDetail, request);

      ////We DELETE the created/modified record, so as to be able to run again this TEST, and create a UNIQUE record each time!!
      //dataSource1.deleteRecords(
      //  new string[] { CCI.Common.CommonData.fieldID },
      //  new string[] { orderID }, CCI.Common.CommonData.tableORDERS);

      ////We delete the newly created Customer!
      //dataSource1.deleteRecords(
      //  new string[] { CCI.Common.CommonData.fieldINTERNALID },
      //  new string[] { customerID }, CCI.Common.CommonData.tableEXTERNALIDMAPPING);

      //dataSource1.deleteRecords(
      //  new string[] { CCI.Common.CommonData.fieldENTITY },
      //  new string[] { customerID }, CCI.Common.CommonData.tableATTRIBUTE);

      //dataSource1.deleteRecords(
      //  new string[] { CCI.Common.CommonData.fieldENTITY },
      //  new string[] { customerID }, CCI.Common.CommonData.tableENTITY);

      //WholesaleUSOC and RetailUSOC Order Header Data!!
      request = new Common.ServerRequest();
      request.SecurityID = securityID;

      request.Form.Add("Header.ContractTerm", "36");
      request.Form.Add("Header.Comment", "Added Header Order!!!");
      request.Form.Add("Header.InstallationCosts", "Yes We have!!");
      request.Form.Add("Header.Location", "City One Park");
      request.Form.Add("Header.ShortName", "CarlosE");

      request.Form.Add("Header.CreditCardName", "VISA");
      request.Form.Add("Header.CreditCardNumber", "0245 1252 125358");
      request.Form.Add("Header.ExpirationDate", "01/14");
      request.Form.Add("Header.SecurityCode", "1825");
      request.Form.Add("Header.AmountToPay", "12500.58");

      request.Form.Add("Header.PhonesFrom", "Phones From Create");
      request.Form.Add("Header.CarrierServices", "Carrier Services Create");

      request.Form.Add("Header.LegalName", "Carlos Henriquez");
      request.Form.Add("Header.Address1", "City One Park");
      request.Form.Add("Header.City", "Los Angeles");
      request.Form.Add("Header.State", "CA");
      request.Form.Add("Header.Zip", "999898");

      //Detail, test for wholesaleUSOC and retailUSOC...
      request.Form.Add("Phones[0].ItemId", "CHPPolycom");
      request.Form.Add("Phones[0].Quantity", "1");
      request.Form.Add("Phones[0].RetailItemID", "chppolycom-0");
      //request.Form.Add("Phones[0].RetailMRC", "240.00");
      request.Form.Add("Phones[0].CreatedBy", "Dealer-Dealer");

      response1 = server1.Execute(CCI.Common.ServerCommands.UpdateOrderDetail, request);

      orderID = CCI.Common.CommonFunctions.CInt(((CCI.Common.CCITable)response1.Results[0])[0, CCI.Common.CommonData.fieldID]).ToString();
      customerID = CCI.Common.CommonFunctions.CString(((CCI.Common.CCITable)response1.Results[0])[0, CCI.Common.CommonData.fieldCUSTOMER]);

      detailID = string.Empty;

      request = new Common.ServerRequest();
      request.SecurityID = securityID;

      //WholesaleUSOC and RetailUSOC Order Header Data!!
      request.Form.Add("Header.ID", orderID);
      request.Form.Add("Header.Customer", customerID);
      request.Form.Add("Header.ContractTerm", "48");
      request.Form.Add("Header.ShortName", "CarlosE2");

      request.Form.Add("Header.PhonesFrom", "Phones From Update");
      request.Form.Add("Header.CarrierServices", "Carrier Services Update");

      request.Form.Add("Header.LegalName", "Carlos2 Henriquez");
      request.Form.Add("Header.Address1", "City Two Park");
      request.Form.Add("Header.City", "New York");

      //Detail, test for wholesaleUSOC and retailUSOC...
      request.Form.Add("LinesTrunksList[0].ItemId", "W911A");
      request.Form.Add("LinesTrunksList[0].W911A-24", "30.00");
      request.Form.Add("LinesTrunksList[0].W911A", "35.00");
      request.Form.Add("LinesTrunksList[0].Quantity", "4");

      response1 = server1.Execute(CCI.Common.ServerCommands.UpdateOrderDetail, request);

      //We DELETE the created/modified record, so as to be able to run again this TEST, and create a UNIQUE record each time!!
      dataSource1.deleteRecords(
        new string[] { CCI.Common.CommonData.fieldID },
        new string[] { orderID }, CCI.Common.CommonData.tableORDERS);

      //We delete the newly created Customer!
      dataSource1.deleteRecords(
        new string[] { CCI.Common.CommonData.fieldINTERNALID },
        new string[] { customerID }, CCI.Common.CommonData.tableEXTERNALIDMAPPING);

      dataSource1.deleteRecords(
        new string[] { CCI.Common.CommonData.fieldENTITY },
        new string[] { customerID }, CCI.Common.CommonData.tableATTRIBUTE);

      dataSource1.deleteRecords(
        new string[] { CCI.Common.CommonData.fieldENTITY },
        new string[] { customerID }, CCI.Common.CommonData.tableENTITY);      
      
      //Orders Data
      request = new Common.ServerRequest();
      request.SecurityID = securityID;

      request.Form.Add("Header.ContractTerm", "36");
      request.Form.Add("Header.Comment", "Added Header Order!!!");
      request.Form.Add("Header.InstallationCosts", "Yes We have!!");
      request.Form.Add("Header.Location", "City One Park");
      request.Form.Add("Header.ShortName", "CarlosE");

      request.Form.Add("Header.CreditCardName", "VISA");
      request.Form.Add("Header.CreditCardNumber", "0245 1252 125358");
      request.Form.Add("Header.ExpirationDate", "01/14");
      request.Form.Add("Header.SecurityCode", "1825");
      request.Form.Add("Header.AmountToPay", "12500.58");

      request.Form.Add("Header.PhonesFrom", "Phones From Create");
      request.Form.Add("Header.CarrierServices", "Carrier Services Create");

      request.Form.Add("Header.LegalName", "Carlos Henriquez");
      request.Form.Add("Header.Address1", "City One Park");
      request.Form.Add("Header.City", "Los Angeles");
      request.Form.Add("Header.State", "CA");
      request.Form.Add("Header.Zip", "999898");

      //OrderDetail Data
      request.Form.Add("LinesTrunksList[0].ItemId", "WNIPLD");
      request.Form.Add("LinesTrunksList[0].RetailItemID", "WERFS");
      request.Form.Add("LinesTrunksList[0].RetailMRC", "29.53");
      request.Form.Add("LinesTrunksList[0].Quantity", "3");

      request.Form.Add("LinesTrunksList[1].ItemId", "WNIP");
      request.Form.Add("LinesTrunksList[1].RetailItemID", "WICM");
      request.Form.Add("LinesTrunksList[1].Quantity", "1");
      
      request.Form.Add("LinesTrunksList[2].ItemId", "WIPTL1");
      request.Form.Add("LinesTrunksList[2].RetailItemID", "WIFAXN");
      request.Form.Add("LinesTrunksList[2].Quantity", "14");

      request.Form.Add("LinesTrunksList[3].ItemId", "WBIPLD");
      request.Form.Add("LinesTrunksList[3].RetailMRC", "14.5");
      request.Form.Add("LinesTrunksList[3].Quantity", "12");

      #region Extra DATA for PERFORMANCE TESTS
      //request.Form.Add("LinesTrunksList[4].ItemId", "WIPTS1");

      //request.Form.Add("LinesTrunksList[5].ItemId", "WIPTNL");
      //request.Form.Add("LinesTrunksList[5].RetailMRC", "99.95");
      //request.Form.Add("LinesTrunksList[5].Quantity", "4");

      //request.Form.Add("LinesTrunksList[6].ItemId", "WITNLV");
      //request.Form.Add("LinesTrunksList[6].RetailMRC", "0");

      //request.Form.Add("LinesTrunksList[7].ItemId", "WOBIP");
      //request.Form.Add("LinesTrunksList[7].RetailMRC", "85.5");

      //request.Form.Add("LinesTrunksList[8].ItemId", "WOBLD");
      //request.Form.Add("LinesTrunksList[8].RetailMRC", "77.17");

      //request.Form.Add("FeaturesList[0].RetailMRC", "758.695");
      //request.Form.Add("FeaturesList[2].RetailMRC", "526.58");
      //request.Form.Add("FeaturesList[3].RetailMRC", "1258.695");
      //request.Form.Add("FeaturesList[4].RetailMRC", "4568.695");

      //request.Form.Add("OtherEquipmentList[0].ItemId", "WERSS");
      //request.Form.Add("OtherEquipmentList[0].RetailMRC", "569.26");
      //request.Form.Add("OtherEquipmentList[0].Quantity", "1");

      //request.Form.Add("OtherEquipmentList[1].ItemId", "WERSV");
      //request.Form.Add("OtherEquipmentList[1].RetailMRC", "458.256");
      //request.Form.Add("OtherEquipmentList[1].Quantity", "3");

      //request.Form.Add("OtherEquipmentList[2].ItemId", "WERFS");
      //request.Form.Add("OtherEquipmentList[2].RetailMRC", "45.59");
      //request.Form.Add("OtherEquipmentList[2].Quantity", "2");

      //request.Form.Add("OtherEquipmentList[3].ItemId", "WERFV");
      //request.Form.Add("OtherEquipmentList[3].RetailMRC", "85.569");

      #endregion Extra DATA for PERFORMANCE TESTS
      //Extra manual fields!
      request.Form.Add("CurrentCarrier[0].PhoneMakeModel", "PhoneMModel001");
      request.Form.Add("CurrentCarrier[0].VendorDescription", "VendorDescriptionPMM01");
      request.Form.Add("CurrentCarrier[0].VendorEmail", "vendorEmail@testemail.com");
      request.Form.Add("CurrentCarrier[0].VendorPhone", "(401) 999 556688");

      request.Form.Add("CurrentCarrier[0].Quantity", "8");
      request.Form.Add("CurrentCarrier[1].Quantity", "21");
      request.Form.Add("CurrentCarrier[2].Quantity", "18");

      request.Form.Add("CurrentCarrier[0].SalesPrice", "1250.25");
      request.Form.Add("CurrentCarrier[1].Monthly", "475.25");

      request.Form.Add("CurrentCarrier[2].Variable", "1475.85");
      request.Form.Add("CurrentCarrier[2].Install", "759.25");

      request.Form.Add("CurrentCarrier[0].Description", "ExtraField Description");
      request.Form.Add("CurrentCarrier[0].YourCost", "458.55");
      request.Form.Add("CurrentCarrier[1].CurrentCarrier", "ATT Current");
      request.Form.Add("CurrentCarrier[2].CarrierName", "Verizon is CarrierName!");

      request.Form.Add("CurrentCarrier[0].ConnectionType", "ConnectionType");
      request.Form.Add("CurrentCarrier[0].ContractExpirationDate", "2013/12/30");
      request.Form.Add("CurrentCarrier[0].CarrierEmail", "carrieremail@testemail.com");
      request.Form.Add("CurrentCarrier[0].CarrierPhone", "(604) 999 555 6895");
      request.Form.Add("CurrentCarrier[0].ContactName", "My Contact Name");

      response1 = server1.Execute(CCI.Common.ServerCommands.UpdateOrderDetail, request);

      responseUpdate = CCI.Common.ServerResponseSerializer.ToJson(response1);
      responseGetHeader = string.Format("{0}{1}{2}", "{", CCI.Common.ServerResponseSerializer.ToJson(((CCI.Common.CCITable)response1.Results[0])), "}");
      responseGetDetail = CCI.Common.ServerResponseSerializer.ToJson(((CCI.Common.CCITable)response1.Results[1]));

      orderID = CCI.Common.CommonFunctions.CInt(((CCI.Common.CCITable)response1.Results[0])[0, CCI.Common.CommonData.fieldID]).ToString();
      customerID = CCI.Common.CommonFunctions.CString(((CCI.Common.CCITable)response1.Results[0])[0, CCI.Common.CommonData.fieldCUSTOMER]);

      detailID = CCI.Common.CommonFunctions.CString(((CCI.Common.CCITable)response1.Results[1])[0, CCI.Common.CommonData.fieldDETAILID]);

      request = new Common.ServerRequest();
      request.SecurityID = securityID;

      //Orders Data
      request.Form.Add("Header.ID", orderID);
      request.Form.Add("Header.Customer", customerID);

      request.Form.Add("Header.ContractTerm", "36");
      request.Form.Add("Header.Comment", "Added Header Order Modified!!!");
      request.Form.Add("Header.InstallationCosts", "Yes We have TOO!!");

      request.Form.Add("Header.CreditCardName", "VISA");
      request.Form.Add("Header.SecurityCode", "1845");
      request.Form.Add("Header.AmountToPay", "14000");

      request.Form.Add("Header.CarrierServices", "Carrier Services Update");

      request.Form.Add("Header.LegalName", "Carlos Henriquez");
      request.Form.Add("Header.Address1", "City One Park");
      //request.Form.Add("Header.Suite", "Suite 505");
      request.Form.Add("Header.City", "Los Angeles");
      request.Form.Add("Header.State", "CA");
      request.Form.Add("Header.Zip", "999898");

      //OrderDetail Data
      request.Form.Add("LinesTrunksList[0].DetailID", detailID);
      request.Form.Add("LinesTrunksList[0].ItemId", "WNIPLD");
      request.Form.Add("LinesTrunksList[0].RetailItemID", "WITNLV");
      request.Form.Add("LinesTrunksList[0].Quantity", "4");

      response1 = server1.Execute(CCI.Common.ServerCommands.UpdateOrderDetail, request);

      request.Parameters.Add("ID", orderID);
      response1 = server1.Execute(CCI.Common.ServerCommands.GetOrderHeader, request);
      response1 = server1.Execute(CCI.Common.ServerCommands.GetOrderDetail, request);

      //We finally DELETE the created/modified record, so as to be able to run again this TEST, and create a UNIQUE record each time!!

      request = new Common.ServerRequest();
      request.SecurityID = securityID;

      request.Parameters.Add("OrderId", orderID);
      response1 = server1.Execute(CCI.Common.ServerCommands.GetOrderTotals, request);

      dataSource1.deleteRecords(
        new string[] { CCI.Common.CommonData.fieldORDERID },
        new string[] { orderID }, CCI.Common.CommonData.tableORDERDETAIL);

      //We DELETE the created/modified record, so as to be able to run again this TEST, and create a UNIQUE record each time!!
      dataSource1.deleteRecords(
        new string[] { CCI.Common.CommonData.fieldID },
        new string[] { orderID }, CCI.Common.CommonData.tableORDERS);

      //We delete the newly created Customer!
      dataSource1.deleteRecords(
        new string[] { CCI.Common.CommonData.fieldINTERNALID },
        new string[] { customerID }, CCI.Common.CommonData.tableEXTERNALIDMAPPING);

      dataSource1.deleteRecords(
        new string[] { CCI.Common.CommonData.fieldENTITY },
        new string[] { customerID }, CCI.Common.CommonData.tableATTRIBUTE);

      dataSource1.deleteRecords(
        new string[] { CCI.Common.CommonData.fieldENTITY },
        new string[] { customerID }, CCI.Common.CommonData.tableENTITY);

    }
  }
}
