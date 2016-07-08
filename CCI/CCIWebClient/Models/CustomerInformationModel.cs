using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using CCI.Common;
using CCIWebClient.Common;

namespace CCIWebClient.Models
{
    public class CustomerInformationModel
    {
        public QuoteHeaderModel Header;
        public string UserType { get; set; }
        public string DetailQuoteId { get; set; }
        public string InstallationCost { get; set; }
        public bool IllPay { get; set; }
        public bool DealersPay { get; set; }
        public bool CustomPay { get; set; }
        public List<QuoteItem> EquipmentRental;
        public List<QuoteItem> Faxing;
        public List<QuoteItem> Features;
        public List<QuoteItem> Fees;
        public List<QuoteItem> LinesTrunks;
        public List<QuoteItem> InternetAccess;
        public List<QuoteItem> LongDistance;
        public List<QuoteItem> MoreUsoc;
        public List<QuoteItem> Networking;
        public List<QuoteItem> OtherEquipment;
        public List<QuoteItem> PhoneNumbers;
        public List<QuoteItem> Phones;
        public List<QuoteItem> NonEquipment;
        public List<QuoteItem> DialTone;
        public List<QuoteItem> ManualPhones;
        public List<QuoteItem> CarrierServices;
        public List<QuoteItem> OtherCarrier;

        public CustomerInformationModel()
        {
            EquipmentRental = new List<QuoteItem>();
            Faxing = new List<QuoteItem>();
            Features = new List<QuoteItem>();
            Fees = new List<QuoteItem>();
            LinesTrunks = new List<QuoteItem>();
            InternetAccess = new List<QuoteItem>();
            LongDistance = new List<QuoteItem>();
            MoreUsoc = new List<QuoteItem>();
            Networking = new List<QuoteItem>();
            OtherEquipment = new List<QuoteItem>();
            PhoneNumbers = new List<QuoteItem>();
            Phones = new List<QuoteItem>();
            NonEquipment = new List<QuoteItem>();
            DialTone = new List<QuoteItem>();
            CarrierServices = new List<QuoteItem>();
            OtherCarrier = new List<QuoteItem>();
            ManualPhones = new List<QuoteItem>();
        }

        public CustomerInformationModel(int quoteId, bool isRecommended, int securityid)
        {
            if (quoteId != 0)
                Header = new QuoteHeaderModel(quoteId, "");
            else
                Header = new QuoteHeaderModel();

            DetailQuoteId = Convert.ToString(quoteId);
            EquipmentRental = new List<QuoteItem>();
            Faxing = new List<QuoteItem>();
            Features = new List<QuoteItem>();
            Fees = new List<QuoteItem>();
            LinesTrunks = new List<QuoteItem>();
            InternetAccess = new List<QuoteItem>();
            LongDistance = new List<QuoteItem>();
            Networking = new List<QuoteItem>();
            MoreUsoc = new List<QuoteItem>();
            OtherEquipment = new List<QuoteItem>();
            PhoneNumbers = new List<QuoteItem>();
            Phones = new List<QuoteItem>();
            NonEquipment = new List<QuoteItem>();
            DialTone = new List<QuoteItem>();
            ManualPhones = new List<QuoteItem>();
            CarrierServices = new List<QuoteItem>();
            OtherCarrier = new List<QuoteItem>();

            QuoteItem EquipmentRentalItems = new QuoteItem();
            QuoteItem FaxingItems = new QuoteItem();
            QuoteItem FeatureItems = new QuoteItem();
            QuoteItem FeesItems = new QuoteItem();
            QuoteItem LinesTrunksItems = new QuoteItem();
            QuoteItem InternetAccessItems = new QuoteItem();
            QuoteItem LongDistanceItems = new QuoteItem();
            QuoteItem MoreUsocItems = new QuoteItem();
            QuoteItem NetworkingItems = new QuoteItem();
            QuoteItem OtherEquipmentItems = new QuoteItem();
            QuoteItem PhoneNumbersItems = new QuoteItem();
            QuoteItem PhonesItems = new QuoteItem();
            QuoteItem NonEquipmentItems = new QuoteItem();
            QuoteItem DialToneItems = new QuoteItem();
            
            CCITable table = null;
            if (quoteId == 0 ) //quoteId is an int, which isn't nullable - 2nd condition will always be false!
                table = Proxy.getOrderScreenCCITable(securityid, "Yes", isRecommended);
            else
                table = Proxy.getOrderDetailCCITable(Convert.ToString(quoteId), isRecommended, securityid);

            for (int i = 0; i < table.NumberRows; i++)
            {
             //   if (Convert.ToString(table[i, 0]) == QuoteDictionary.DEALERQUOTE)
                {
                    switch ((table[i, "screensection"] as string))
                    {
                        case "EquipmentRental":
                            {
                                EquipmentRentalItems = getElementList(table, i);
                                EquipmentRental.Add(EquipmentRentalItems);
                            } break;
                        case "MoreUsoc":
                            {
                                MoreUsocItems = getElementList(table, i);
                                MoreUsoc.Add(MoreUsocItems);
                            } break;
                        case QuoteDictionary.FAXING:
                            {
                                FaxingItems = getElementList(table, i);
                                Faxing.Add(FaxingItems);
                            } break;
                        case QuoteDictionary.FEATURES:
                            {
                                FeatureItems = getElementList(table, i);
                                Features.Add(FeatureItems);
                            } break;
                        case "Fees":
                            {
                                FeesItems = getElementList(table, i);
                                Fees.Add(FeesItems);
                            } break;
                        case QuoteDictionary.LINESTRUNCKS:
                            {
                                LinesTrunksItems = getElementList(table, i);
                                LinesTrunks.Add(LinesTrunksItems);
                            } break;
                        case QuoteDictionary.INTERNETACCESS:
                            {
                                InternetAccessItems = getElementList(table, i);
                                InternetAccess.Add(InternetAccessItems);
                            } break;
                        case QuoteDictionary.LONGDISTANCE:
                            {
                                LongDistanceItems = getElementList(table, i);
                                LongDistance.Add(LongDistanceItems);
                            } break;
                        case QuoteDictionary.NETWORKING:
                            {
                                NetworkingItems = getElementList(table, i);
                                Networking.Add(NetworkingItems);
                            } break;
                        case QuoteDictionary.OTHEREQUIPMENT:
                            {
                                OtherEquipmentItems = getElementList(table, i);
                                OtherEquipment.Add(OtherEquipmentItems);
                            } break;
                        case QuoteDictionary.PHONENUMBERS:
                            {
                                PhoneNumbersItems = getElementList(table, i);
                                PhoneNumbers.Add(PhoneNumbersItems);
                            } break;
                        case QuoteDictionary.PHONES:
                            {
                                PhonesItems = getElementList(table, i);
                                Phones.Add(PhonesItems);
                            } break;
                        case QuoteDictionary.DIALTONE:
                            {
                                DialToneItems = getElementList(table, i);
                                DialTone.Add(DialToneItems);
                            } break;
                        case "NonEquipment":
                            {
                                NonEquipmentItems = getElementList(table, i);
                                NonEquipment.Add(NonEquipmentItems);
                            } break;
                        case "otherphones":
                            ManualPhones.Add(getElementList(table, i));
                            break;
                        case "CurrentCarrier":
                            CarrierServices.Add(getElementList(table, i));

                            break;
                        case "OtherCarriers":
                            OtherCarrier.Add(getElementList(table, i));

                            break;
                        case "ManualPhones":
                            this.ManualPhones.Add(getElementList(table, i));
                            break;
                    } //switch
                } //if
            } //for

            if (NonEquipment.Count == 0)
                NonEquipment.Add(new QuoteItem());
            if (Fees.Count == 0)
                Fees.Add(new QuoteItem());
            if (ManualPhones.Count == 0)
                ManualPhones.Add(new QuoteItem());
            if (OtherCarrier.Count == 0)
                OtherCarrier.Add(new QuoteItem());
            if (CarrierServices.Count == 0)
                CarrierServices.Add(new QuoteItem());
        }
       
        public static class DetailColumns
        {
            public static string SCREEN = "screen";
            public static string SCREENSECTION="screensection";
            public static string SEQUENCE="sequence";
            public static string QUANTITY="quantity";
            public static string ITEMID="itemid";
            public static string DESCRIPTION="description";
            public static string IMAGEPATH="imagepath";
            public static string RETAILITEMID="retailitemid";
            public static string MRCRETAIL="mrcretail";
            public static string DEALERCOST="dealercost";
            public static string ORDERID="orderid";
            public static string DETAILID = "detailid";
            public static string VARIABLE = "variable";
            public static string PHONEMAKEMODEL = "phonemakemodel";
            public static string VENDOREMAIL = "vendoremail";
            public static string VENDORDESCRIPTION = "vendordescription";
            public static string VENDORPHONE = "vendorphone";
        }

        //screen","screensection","sequence","quantity","itemid","description","imagepath","retailitemid","mrcretail","dealercost","orderid","detailid

        public QuoteItem getElementList(CCITable newTable, int rowNumber)
        {
          QuoteItem Element = new QuoteItem();
          Element.Screen = newTable[rowNumber, DetailColumns.SCREEN] as string;
          Element.ScreenSection = newTable[rowNumber, DetailColumns.SCREENSECTION] as string;
          Element.Sequence = newTable[rowNumber, DetailColumns.SEQUENCE] as string;
          Element.Quantity = CommonFunctions.CString(newTable[rowNumber, DetailColumns.QUANTITY]);
          if (Element.Quantity == "0") Element.Quantity = "";
          Element.ItemId = newTable[rowNumber, DetailColumns.ITEMID] as string;
          Element.Description = newTable[rowNumber, DetailColumns.DESCRIPTION] as string ?? "N/A";
          Element.ImagePath = newTable[rowNumber, DetailColumns.IMAGEPATH] as string;
          Element.RetailItemId = newTable[rowNumber, DetailColumns.RETAILITEMID] as string;
          Element._RetailItemId = newTable.getPickList(rowNumber, QuoteDictionary.RETAILITEMID);
          if (Element._RetailItemId != null)
            Element.NumberOfPickListValues = Element._RetailItemId.Count;
          Element.MRCRetail = newTable[rowNumber, DetailColumns.MRCRETAIL] as string;
          Element.DealerCost = newTable[rowNumber, DetailColumns.DEALERCOST] as string;
          Element.ElementOrderId = CommonFunctions.CString(newTable[rowNumber, DetailColumns.ORDERID]);
          Element.DetailId = newTable[rowNumber, DetailColumns.DETAILID] as string;
          Element.PhoneMakeModel = newTable[rowNumber, DetailColumns.PHONEMAKEMODEL] as string;
          Element.VendorEmail = newTable[rowNumber, DetailColumns.VENDOREMAIL] as string;
          Element.VendorPhone = newTable[rowNumber, DetailColumns.VENDORPHONE] as string;
          Element.RetailMRC = CommonFunctions.CString(newTable[rowNumber, "retailmrc"]);
          Element.RetailNRC = CommonFunctions.CString(newTable[rowNumber, "retailnrc"]);
          Element.CarrierDescription = CommonFunctions.CString(newTable[rowNumber, "carrierdescription"]);
          Element.ContractExpirationDate = CommonFunctions.CString(newTable[rowNumber, "contractexpirationdate"]);
          Element.ConectionType = CommonFunctions.CString(newTable[rowNumber, "connectiontype"]);
          Element.CarrierEmail = CommonFunctions.CString(newTable[rowNumber, "carrieremail"]);
          Element.CarrierPhone = CommonFunctions.CString(newTable[rowNumber, "carrierphone"]);
          Element.ContactName = CommonFunctions.CString(newTable[rowNumber, "contactname"]);
          Element.VendorDescription = CommonFunctions.CString(newTable[rowNumber, "vendordescription"]);
          //  Element.Description = newTable[rowNumber, DetailColumns.VENDORDESCRIPTION] as string;

          if (CommonFunctions.CDouble(newTable[rowNumber, "retailmrc"]) != 0)
          {
            Element.Variable = CommonFunctions.CString(newTable[rowNumber, "retailmrc"]);
          }
          else 
          {
            Element.Variable = CommonFunctions.CString(newTable[rowNumber, "retailnrc"]);
          }

          Element.IsVariable = (CommonFunctions.CInt(newTable[rowNumber, "isvariable"]) == 1);
          //if (Element.IsVariable)
          //   Element.Variable = CommonFunctions.CString( newTable[rowNumber, "retailmrc"]);

          return Element;
        }

        public QuoteItem getElementList()
        {
            QuoteItem Element = new QuoteItem();
            Element.Screen = "";
            Element.ScreenSection = "";
            Element.Sequence = "";
            Element.Quantity = "";
            Element.ItemId = "";
            Element.Description = "";
            Element.ImagePath = "";
            Element.MRCRetail = "";
            Element.DealerCost = "";
            Element.ElementOrderId = "";
            Element.DetailId = "";
            Element.Vendor = "";
            Element.Monthly = "";
            Element.Install = "";

            return Element;
        }
    }
}

#region Commented Code

//PhoneNumberIDs.Add(QuoteDictionary.DESCRIPTIONID, QuoteDictionary.PHONENUMDES);
//PhoneNumberIDs.Add(QuoteDictionary.SALEPRICEID, QuoteDictionary.PHONENUMPRICE);
//PhoneNumberIDs.Add(QuoteDictionary.DEALERCOSTID, QuoteDictionary.PHONENUMCOST);
//PhoneNumberIDs.Add(QuoteDictionary.QUANTITYID, QuoteDictionary.PHONENUMQUANTITY);
//PhoneIDs.Add(QuoteDictionary.DESCRIPTIONID, QuoteDictionary.PHONEDES);
//PhoneIDs.Add(QuoteDictionary.SALEPRICEID, QuoteDictionary.PHONEPRICE);
//PhoneIDs.Add(QuoteDictionary.DEALERCOSTID, QuoteDictionary.PHONECOST);
//PhoneIDs.Add(QuoteDictionary.QUANTITYID, QuoteDictionary.PHONEQUANTITY);
//OtherEquipmentIDs.Add(QuoteDictionary.DESCRIPTIONID, QuoteDictionary.OTHEREQDES);
//OtherEquipmentIDs.Add(QuoteDictionary.SALEPRICEID, QuoteDictionary.OTHEREQPRICE);
//OtherEquipmentIDs.Add(QuoteDictionary.DEALERCOSTID, QuoteDictionary.OTHEREQCOST);
//OtherEquipmentIDs.Add(QuoteDictionary.QUANTITYID, QuoteDictionary.OTHEREQQUANTITY);
//Hashtable PhoneNumberIDs = new Hashtable();
//Hashtable PhoneIDs = new Hashtable();
//Hashtable OtherEquipmentIDs = new Hashtable();

//public CustomerInformationModel(int quoteId, bool isRecommended, int securityid, string section, int NumberOfNewElements)
//{
//    DetailQuoteId = Convert.ToString(quoteId);
//    Fees = new List<QuoteItem>();
//    InternetAccess = new List<QuoteItem>();
//    LongDistance = new List<QuoteItem>();
//    Networking = new List<QuoteItem>();
//    NonEquipment = new List<QuoteItem>();
//    DialTone = new List<QuoteItem>();
//    ManualPhones = new List<QuoteItem>();
//    QuoteItem FeesItems = new QuoteItem();
//    QuoteItem InternetAccessItems = new QuoteItem();
//    QuoteItem LongDistanceItems = new QuoteItem();
//    QuoteItem NetworkingItems = new QuoteItem();
//    QuoteItem NonEquipmentItems = new QuoteItem();
//    QuoteItem DialToneItems = new QuoteItem();
//    CCITable table = Proxy.getOrderDetailCCITable(Convert.ToString(quoteId), isRecommended, securityid, section);
//    for (int i = 0; i < table.NumberRows; i++)
//    {
//        if (Convert.ToString(table[i, 0]) == QuoteDictionary.DEALERQUOTE)
//        {
//            switch (table[i, 1] as string)
//            {
//                case QuoteDictionary.FEES:
//                    {
//                        FeesItems = getElementList(table, i);
//                        Fees.Add(FeesItems);
//                    } break;
//                case QuoteDictionary.INTERNETACCESS:
//                    {
//                        InternetAccessItems = getElementList(table, i);
//                        InternetAccess.Add(InternetAccessItems);
//                    } break;
//                case QuoteDictionary.LONGDISTANCE:
//                    {
//                        LongDistanceItems = getElementList(table, i);
//                        LongDistance.Add(LongDistanceItems);
//                    } break;
//                case QuoteDictionary.NETWORKING:
//                    {
//                        NetworkingItems = getElementList(table, i);
//                        Networking.Add(NetworkingItems);
//                    } break;
//                case QuoteDictionary.DIALTONE:
//                    {
//                        DialToneItems = getElementList(table, i);
//                        DialTone.Add(DialToneItems);
//                    } break;
//                case QuoteDictionary.NONEQUIPMENT:
//                    {
//                        NonEquipmentItems = getElementList(table, i);
//                        NonEquipment.Add(NonEquipmentItems);
//                    } break;
//            }
//        }
//    }
//}

#endregion