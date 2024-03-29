﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCIWebClient.Common;
using CCI.Common;
using CCIWebClient.Models;
using CCIWebClient.Reports;

namespace CCIWebClient.Controllers
{

    [ACGAuthorize]
    public class QuoteController : Controller
    {
        //
        // GET: /Quote/

        private static QuoteTotalsModel TotalModel;
        private static CustomerInformationModel DetailModel;


        public void LoadNewModel(int quoteId, string quoteName, bool IsRecommended)
        {
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;
            DetailModel = new CustomerInformationModel(quoteId, IsRecommended, securityid);
            TotalModel = new QuoteTotalsModel(securityid, quoteId.ToString(), quoteName);
        }

        public ActionResult Index()
        {
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;
            //var model = new CustomerInformationModel(quoteId, securityid);
            var model = new CustomerInformationModel();
            return View(model);
        }

        //public ActionResult LoadQuote(int quoteId, bool isRecommended)
        //{
        //    int securityid = (int)Session["securityid"];
        //    var model = new CustomerInformationModel(quoteId, isRecommended, securityid, QuoteDictionary.DEFAULT_CUSTOMERID
        //        , QuoteDictionary.DEFAULT_CUSTOMERNAME, QuoteDictionary.DEFAULT_ADDRESS, QuoteDictionary.DEFAULT_STATE, QuoteDictionary.DEFAULT_CITY, QuoteDictionary.DEFAULT_ZIPCODE);
        //    return View("Header", model);
        //}


        public ActionResult Header()
        {
            //int quoteId = 0;
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;
            //bool isRecommended = true;
            //var model = new CustomerInformationModel(QuoteDictionary.DEFAULT_QUOTEID, QuoteDictionary.DEFAULT_ISRECOMMENDEDVALUE, securityid, QuoteDictionary.DEFAULT_CUSTOMERID
            //    , QuoteDictionary.DEFAULT_CUSTOMERNAME, QuoteDictionary.DEFAULT_ADDRESS, QuoteDictionary.DEFAULT_STATE, QuoteDictionary.DEFAULT_CITY, QuoteDictionary.DEFAULT_ZIPCODE);
            // var model = new CustomerInformationModel();
           // var model = new CustomerInformationModel(1, true, securityid);
            var model = new QuoteHeaderModel();
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult Detail()
        {
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;
            CustomerInformationModel model = new CustomerInformationModel(0, true, securityid);  
            return PartialView("Detail", model);
        }

        public ActionResult DetailPage( int Quoteid, Boolean isRecommended)
        {
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;
             CustomerInformationModel model = new CustomerInformationModel(Quoteid, isRecommended, securityid);
            return PartialView("Detail", model);
        }

        public ActionResult HeaderPage(int Quoteid, Boolean isRecommended)
        {
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;
            CustomerInformationModel model = new CustomerInformationModel(Quoteid, isRecommended, securityid);
            return PartialView("Header", model);
        }
       

        [ChildActionOnly]
        public ActionResult LinesAndTrunks(CustomerInformationModel model)
        {
            return PartialView("LinesAndTrunks", model);
        }

        [ChildActionOnly]
        public ActionResult Features(CustomerInformationModel model)
        {
            return PartialView("Features", model);
        }

        [ChildActionOnly]
        public ActionResult Faxing(CustomerInformationModel model)
        {
            return PartialView("Faxing", model);
        }

        [ChildActionOnly]
        public ActionResult PhoneNumbers(CustomerInformationModel model)
        {
            return PartialView("PhoneNumbers", model);
        }

        [ChildActionOnly]
        public ActionResult WhereYouGetYourPhone(CustomerInformationModel model)
        {
            return PartialView("WhereYouGetYourPhone", model);
        }

        [ChildActionOnly]
        public ActionResult Phones(CustomerInformationModel model)
        {
            return PartialView("Phones", model);
        }

        [ChildActionOnly]
        public ActionResult EquipmentRental(CustomerInformationModel model)
        {
            return PartialView("EquipmentRental", model);
        }

        [ChildActionOnly]
        public ActionResult MoreUsoc(CustomerInformationModel model)
        {
            return PartialView("MoreUsoc", model);
        }


        [ChildActionOnly]
        public ActionResult OtherEquipment(CustomerInformationModel model)
        {
            return PartialView("OtherEquipment", model);
        }


        [ChildActionOnly]
        public ActionResult NonCityEquipment(CustomerInformationModel model)
        {
            return PartialView("NonCityEquipment", model);
        }
        [ChildActionOnly]
        public ActionResult CarrierServices(CustomerInformationModel model)
        {
            return PartialView("CarrierServices", model);
        }

        [ChildActionOnly]
        public ActionResult DialTone(CustomerInformationModel model)
        {
            return PartialView("DialTone", model);
        }

        [ChildActionOnly]
        public ActionResult InternetAccess(CustomerInformationModel model)
        {
            return PartialView("InternetAccess", model);
        }

        [ChildActionOnly]
        public ActionResult Networking(CustomerInformationModel model)
        {
            return PartialView("Networking", model);
        }

        

        [ChildActionOnly]
        public ActionResult AdditionalServices(CustomerInformationModel model)
        {
            return PartialView("AdditionalServices", model);
        }

        [ChildActionOnly]
        public ActionResult Fees(CustomerInformationModel model)
        {
            return PartialView("Fees", model);
        }

        [ChildActionOnly]
        public ActionResult InstallationCost(CustomerInformationModel model)
        {
            return PartialView("InstallationCost", model);
        }

        public ActionResult InstallationCost(int Quoteid)
        {
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;
            CustomerInformationModel model = new CustomerInformationModel(Quoteid, true, securityid);
          
            return PartialView("InstallationCost", model);
        }



        public PartialViewResult Total(string quoteId, string quoteName)
        {
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;
            var model = new QuoteTotalsModel(securityid, quoteId, quoteName);
            return PartialView("Total", model);
        }

        [ChildActionOnly]
        public ActionResult Total2(string quoteId, string quoteName)
        {
            return Total(quoteId, quoteName);
        }
        //public ActionResult PartialDetail(int quoteId, bool isRecommended, string customerId, string name, string address, string state, string city, string zip)
        //{

        //    int securityid = (int)Session["securityid"];
        //    var model = new CustomerInformationModel(quoteId,  isRecommended, securityid, customerId, name, address, state, city, zip);                                          
        //    return PartialView("Detail", model);
        //}

        public ActionResult GetSuggestedCustomers(string name, string address, string suite, string city, string state, string zip) //string dealer
        {
            CustomerSuggestedModel model = new CustomerSuggestedModel(name, address, suite, city, state, zip); //dealer
            return PartialView("SelectCustomer", model);
        }

        [HttpPost]
        public ActionResult SaveData(FormCollection data)
        {


            return Json("{}");
        }


        public PartialViewResult LoadPartial(string partialName, int quoteId, Boolean isRecommended)
        {
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;
            CustomerInformationModel model = new CustomerInformationModel(quoteId, isRecommended, securityid);
            return PartialView(partialName, model);
        }


        public ActionResult PhoneNumberSection(int Quoteid, Boolean isRecommended, Boolean IsLinkAction)
        {
            CustomerInformationModel model;
            if (IsLinkAction)
            {
                CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
                int securityid = user.SecurityId;
                model = new CustomerInformationModel(Quoteid, isRecommended, securityid);
            }
            else
                model = DetailModel;
            return PartialView("PhoneNumbers", model);
        }

        public ActionResult PhoneSection(int Quoteid, Boolean isRecommended, Boolean IsLinkAction)
        {
            CustomerInformationModel model;
            if (IsLinkAction)
            {
                CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
                int securityid = user.SecurityId;
                model = new CustomerInformationModel(Quoteid, isRecommended, securityid);
            }
            else
                model = DetailModel;
            return PartialView("Phones", model);
        }

        public ActionResult EquipmentRentalSection(int Quoteid, Boolean isRecommended, Boolean IsLinkAction)
        {
            CustomerInformationModel model;
            if (IsLinkAction)
            {
                CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
                int securityid = user.SecurityId;
                model = new CustomerInformationModel(Quoteid, isRecommended, securityid);
            }
            else
                model = DetailModel;
            return PartialView("EquipmentRental", model);
        }


        public ActionResult MoreUsocSection(int Quoteid, Boolean isRecommended, Boolean IsLinkAction)
        {
            CustomerInformationModel model;
            if (IsLinkAction)
            {
                CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
                int securityid = user.SecurityId;
                model = new CustomerInformationModel(Quoteid, isRecommended, securityid);
            }
            else
                model = DetailModel;
            return PartialView("MoreUsoc", model);
        }

        public ActionResult OtherEquipmentSection(int Quoteid, Boolean isRecommended, Boolean IsLinkAction)
        {
            CustomerInformationModel model;
            if (IsLinkAction)
            {
                CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
                int securityid = user.SecurityId;
                model = new CustomerInformationModel(Quoteid, isRecommended, securityid);
            }
            else
                model = DetailModel;
            return PartialView("OtherEquipment", model);
        }

        //public ActionResult NonCityEquipmentSection(int Quoteid, Boolean isRecommended, Boolean IsLinkAction, string section, int NumberOfElements)
        //{
        //    CustomerInformationModel model;
        //    if (IsLinkAction)
        //    {
        //        CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
        //        int securityid = user.SecurityId;
        //        model = new CustomerInformationModel(Quoteid, isRecommended, securityid, section, NumberOfElements);
        //    }
        //    else
        //        model = DetailModel;
        //    return PartialView("NonCityEquipment", model);
        //}


        public ActionResult DialToneSection(int Quoteid, Boolean isRecommended, Boolean IsLinkAction, string section, int NumberOfElements)
        {
            CustomerInformationModel model;
            if (IsLinkAction)
            {
                CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
                int securityid = user.SecurityId;
                //model = new CustomerInformationModel(Quoteid, isRecommended, securityid, section, NumberOfElements);
                model = new CustomerInformationModel(Quoteid, isRecommended, securityid);
            }
            else
                model = DetailModel;
            return PartialView("DialTone", model);
        }

        //public ActionResult InternetAccessSection(int Quoteid, Boolean isRecommended, Boolean IsLinkAction, string section, int NumberOfElements)
        //{
        //    CustomerInformationModel model;
        //    if (IsLinkAction)
        //    {
        //        CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
        //        int securityid = user.SecurityId;
        //        model = new CustomerInformationModel(Quoteid, isRecommended, securityid, section, NumberOfElements);
        //    }
        //    else
        //        model = DetailModel;
        //    return PartialView("InternetAccess", model);
        //}

        //public ActionResult NetworkingSection(int Quoteid, Boolean isRecommended, Boolean IsLinkAction, string section, int NumberOfElements)
        //{
        //    CustomerInformationModel model;
        //    if (IsLinkAction)
        //    {
        //        CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
        //        int securityid = user.SecurityId;
        //        model = new CustomerInformationModel(Quoteid, isRecommended, securityid, section, NumberOfElements);
        //    }
        //    else
        //        model = DetailModel;
        //    return PartialView("Networking", model);
        //}

        //public ActionResult FeesSection(int Quoteid, Boolean isRecommended, Boolean IsLinkAction, string section, int NumberOfElements)
        //{
        //    CustomerInformationModel model;
        //    if (IsLinkAction)
        //    {
        //        CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
        //        int securityid = user.SecurityId;
        //        model = new CustomerInformationModel(Quoteid, isRecommended, securityid, section, NumberOfElements);
        //    }
        //    else
        //        model = DetailModel;
        //    return PartialView("Fees", model);
        //}

        //public ActionResult NonCityEquipmentSection(int Quoteid, Boolean isRecommended, Boolean IsLinkAction, string section, int NumberOfElements)
        //{
        //    CustomerInformationModel model;
        //    if (IsLinkAction)
        //    {
        //        CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
        //        int securityid = user.SecurityId;
        //        model = new CustomerInformationModel(Quoteid, isRecommended, securityid, section, NumberOfElements);
        //    }
        //    else
        //        model = DetailModel;
        //    return PartialView("NonCityEquipment", model);
        //}

    

        [ChildActionOnly]
        public ActionResult CustomerData(QuoteHeaderModel model)
        {
            return PartialView("CustomerData", model);
        }

     
        public PartialViewResult getQuoteHeader(int quoteId, string quoteName)
        {
            QuoteHeaderModel model;
            if(quoteId == 0 && quoteName == "")
                model = new QuoteHeaderModel();
            else            
                model = new QuoteHeaderModel(quoteId, quoteName);
            return PartialView("CustomerData", model);
        }

        //To load quote detail from quotename autocomplete
        public PartialViewResult getQuoteDetail(int quoteId, string quoteName)
        {
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;
            CustomerInformationModel model;
            //if (quoteId==0)
                 model = new CustomerInformationModel(quoteId, true, securityid);
            //else
            //     model = new CustomerInformationModel(quoteId, false, securityid);
            return PartialView("Detail", model);
        }

        public PartialViewResult getOrderTotals(int quoteId, string quoteName)
        {
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;
            QuoteTotalsModel model = new QuoteTotalsModel(securityid, quoteId.ToString(), quoteName);
            return PartialView("Total", model);
        }

        [ChildActionOnly]
        public ActionResult DealerSummary(QuoteTotalsModel model)
        {
            return PartialView("DealerSummary", model);
        }

       
        [HttpGet]
        [ChildActionOnly]
        public ActionResult OtherTotalsData(QuoteTotalsModel model)
        {
            return PartialView("OtherTotalsData", model);
        }
/// <summary>
/// 
/// </summary>
/// <param name="model"></param>
        [HttpPost]
        public void SendQuoteByMail(EmailToModel model)
        {
            int quoteid = CommonFunctions.CInt(model.QuoteId);
            QuoteReportHandler quoteRepor = new QuoteReportHandler(quoteid);
            Mailer.SendMail(model.toEmail, model.Subject, model.Content, quoteRepor.QuoteReportStream(quoteid), "Quote.pdf", "application/pdf");
        }
/// <summary>
/// 
/// </summary>
/// <param name="QuoteId"></param>
/// <returns></returns>
        public ActionResult MailTo(string QuoteId)
        {
            //CAC-
            ViewBag.ShouldClose = true;
            //

            EmailToModel model = new EmailToModel();
            model.QuoteId = QuoteId;
            return PartialView("MailTo", model);
        }
/// <summary>
/// 
/// </summary>
/// <param name="model"></param>
/// <returns></returns>

      
        public ActionResult TotalValueArea(QuoteTotalsModel model)
        {
            return PartialView("TotalValueArea", model);
        }

        [ChildActionOnly]
        public ActionResult TotalsQuoteIdSection(QuoteTotalsModel model)
        {
            return PartialView("TotalsQuoteIdSection", model);
        }

        public ActionResult LoadQuoteIdSection(bool UseMain)
        {
            //CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            //int securityid = user.SecurityId;
            //QuoteTotalsModel model = new QuoteTotalsModel(securityid, quoteId.ToString(), quoteName);
            return PartialView("TotalsQuoteIdSection", TotalModel);
        }

        public ActionResult LoadTotalValueArea(bool UseMain)
        {
            //CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            //int securityid = user.SecurityId;
            //QuoteTotalsModel model = new QuoteTotalsModel(securityid, quoteId.ToString(), quoteName);
            return PartialView("TotalValueArea", TotalModel);
        }

        public ActionResult LoadOtherTotalsData(bool UseMain)
        {
            //CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            //int securityid = user.SecurityId;
            //QuoteTotalsModel model = new QuoteTotalsModel(securityid, quoteId.ToString(), quoteName);
            return PartialView("OtherTotalsData", TotalModel);
        }

        public ActionResult FaxingSection(bool UseMain)
        {
            //CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            //int securityid = user.SecurityId;
            //CustomerInformationModel model = new CustomerInformationModel(Quoteid, isRecommended, securityid);
            return PartialView("Faxing", DetailModel);
        }

        public ActionResult FeatureSection(bool UseMain)
        {
            //CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            //int securityid = user.SecurityId;
            //CustomerInformationModel model = new CustomerInformationModel(Quoteid, isRecommended, securityid);
            return PartialView("Features", DetailModel);
        }

        public ActionResult LinesAndTrunksSection(bool UseMain)
        {
            //CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            //int securityid = user.SecurityId;
            //CustomerInformationModel model = new CustomerInformationModel(Quoteid, isRecommended, securityid);
            return PartialView("LinesAndTrunks", DetailModel);
        }
    }
}




#region commentedcode

//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Linq;
//using CCIWebClient.Common;
//using CCI.Common;
//using CCIWebClient.Models;

//namespace CCIWebClient.Controllers



//{
    
//     [ACGAuthorize]
//    public class QuoteController : Controller
//    {
//        //
//        // GET: /Quote/

//        public ActionResult Index(CustomerInformationModel model)
//        {
//            string quoteId = model.QuoteId;
//            int securityid = (int)Session["securityid"];
//            List<QuoteItem> FaxingList = new List<QuoteItem>();
//            List<QuoteItem> Features = new List<QuoteItem>();
//            List<QuoteItem> Fees = new List<QuoteItem>();
//            List<QuoteItem> LinesTrunks = new List<QuoteItem>();
//            List<QuoteItem> InternetAccess = new List<QuoteItem>();
//            List<QuoteItem> LongDistanceList = new List<QuoteItem>();
//            List<QuoteItem> Networking = new List<QuoteItem>();
//            List<QuoteItem> OtherEquipment = new List<QuoteItem>();
//            List<QuoteItem> PhoneNumbers = new List<QuoteItem>();
//            List<QuoteItem> Phones = new List<QuoteItem>();
//            List<QuoteItem> NonEquipment = new List<QuoteItem>();
//            List<QuoteItem> DialTone = new List<QuoteItem>();
//            QuoteItem FaxingItems = new QuoteItem();
//            QuoteItem FeatureItems = new QuoteItem();
//            QuoteItem FeesItems = new QuoteItem();
//            QuoteItem LinesTrunksItems = new QuoteItem();
//            QuoteItem InternetAccessItems = new QuoteItem();
//            QuoteItem LongDistanceItems = new QuoteItem();
//            QuoteItem NetworkingItems = new QuoteItem();
//            QuoteItem OtherEquipmentItems = new QuoteItem();
//            QuoteItem PhoneNumbersItems = new QuoteItem();
//            QuoteItem PhonesItems = new QuoteItem();
//            QuoteItem NonEquipmentItems = new QuoteItem();
//            QuoteItem DialToneItems = new QuoteItem();
//            Hashtable PhoneNumberIDs = new Hashtable();
//            Hashtable PhoneIDs = new Hashtable();
//            Hashtable OtherEquipmentIDs = new Hashtable();
//            CCITable table  = null;
//            if (quoteId == "1"|| quoteId == null)
       
//                  table = Proxy.getOrderScreenCCITable(securityid, "Yes", "1");
          
//            else
//                  table = Proxy.getOrderDetailCCITable(quoteId, securityid);

            
//            for (int i = 0; i < table.NumberRows; i++)
//            {
//                if (Convert.ToString(table[i, 0]) == QuoteDictionary.DEALERQUOTE)
//                {
//                    switch (table[i, 1] as string)
//                    {
//                        case QuoteDictionary.FAXING:
//                            {
//                                FaxingItems = getElementList(table, i);
//                                FaxingList.Add(FaxingItems);
//                            } break;
//                        case QuoteDictionary.FEATURES:
//                            {
//                                FeatureItems = getElementList(table, i);
//                                Features.Add(FeatureItems);
//                            }break;
//                        case QuoteDictionary.FEES:
//                            {
//                                FeesItems = getElementList(table, i);
//                                Fees.Add(FeesItems);
//                            }break;
//                        case QuoteDictionary.LINESTRUNCKS:
//                            {
//                                LinesTrunksItems = getElementList(table, i);
//                                LinesTrunks.Add(LinesTrunksItems);
//                            }break;
//                        case QuoteDictionary.INTERNETACCESS:
//                            {
//                                InternetAccessItems = getElementList(table, i);
//                                InternetAccess.Add(InternetAccessItems);
//                            }break;
//                        case QuoteDictionary.LONGDISTANCE:
//                            {
//                                LongDistanceItems = getElementList(table, i);
//                                LongDistanceList.Add(LongDistanceItems);
//                            }break;
//                        case QuoteDictionary.NETWORKING:
//                            {
//                                NetworkingItems = getElementList(table, i);
//                                Networking.Add(NetworkingItems);
//                            }break;
//                        case QuoteDictionary.OTHEREQUIPMENT:
//                            {
//                                OtherEquipmentItems = getElementList(table, i);
//                                OtherEquipment.Add(OtherEquipmentItems);
//                            }break;
//                        case QuoteDictionary.PHONENUMBERS:
//                            {
//                                PhoneNumbersItems = getElementList(table, i);
//                                PhoneNumbers.Add(PhoneNumbersItems);
//                            }break;
//                        case QuoteDictionary.PHONES:
//                            {
//                                PhonesItems = getElementList(table, i);
//                                Phones.Add(PhonesItems);
//                            }break;
//                        case QuoteDictionary.DIALTONE:
//                            {
//                                DialToneItems = getElementList(table, i);
//                                DialTone.Add(DialToneItems);
//                            }break;
//                        case QuoteDictionary.NONEQUIPMENT:
//                            {
//                                NonEquipmentItems = getElementList(table, i);
//                                NonEquipment.Add(NonEquipmentItems);
//                            }break;
//                    }
//                }
//            }
//            PhoneNumberIDs.Add(QuoteDictionary.DESCRIPTIONID, QuoteDictionary.PHONENUMDES);
//            PhoneNumberIDs.Add(QuoteDictionary.SALEPRICEID, QuoteDictionary.PHONENUMPRICE);
//            PhoneNumberIDs.Add(QuoteDictionary.DEALERCOSTID, QuoteDictionary.PHONENUMCOST);
//            PhoneNumberIDs.Add(QuoteDictionary.QUANTITYID, QuoteDictionary.PHONENUMQUANTITY);
//            PhoneIDs.Add(QuoteDictionary.DESCRIPTIONID, QuoteDictionary.PHONEDES);
//            PhoneIDs.Add(QuoteDictionary.SALEPRICEID, QuoteDictionary.PHONEPRICE);
//            PhoneIDs.Add(QuoteDictionary.DEALERCOSTID, QuoteDictionary.PHONECOST);
//            PhoneIDs.Add(QuoteDictionary.QUANTITYID, QuoteDictionary.PHONEQUANTITY);
//            OtherEquipmentIDs.Add(QuoteDictionary.DESCRIPTIONID, QuoteDictionary.OTHEREQDES);
//            OtherEquipmentIDs.Add(QuoteDictionary.SALEPRICEID, QuoteDictionary.OTHEREQPRICE);
//            OtherEquipmentIDs.Add(QuoteDictionary.DEALERCOSTID, QuoteDictionary.OTHEREQCOST);
//            OtherEquipmentIDs.Add(QuoteDictionary.QUANTITYID, QuoteDictionary.OTHEREQQUANTITY);

//            ViewBag.Faxing = FaxingList;
//            ViewBag.Feature = Features;
//            ViewBag.Fees = Fees;
//            ViewBag.LinesTrunks = LinesTrunks;
//            ViewBag.InternetAccess = InternetAccess;
//            ViewBag.LongDistance = LongDistanceList;
//            ViewBag.Networking = Networking;
//            ViewBag.OtherEquipment = OtherEquipment;
//            ViewBag.PhoneNumbers = PhoneNumbers;
//            ViewBag.Phones = Phones;
//            ViewBag.PhoneNumIDs = PhoneNumberIDs;
//            ViewBag.PhonesIDs = PhoneIDs;
//            ViewBag.OtherEquipmentIDs = OtherEquipmentIDs;
//            ViewBag.DialTone = DialTone;
//            ViewBag.NonEquipment = NonEquipment;
//            return View();
//        }


//        public QuoteItem getElementList(CCITable newTable, int rowNumber)
//        {
//            QuoteItem Element = new QuoteItem();
//            for (int j = 0; j < newTable.NumberColumns; j++)
//            {
//                switch (j)
//                {
//                    case 0:
//                        Element.Screen = newTable[rowNumber, j] as string; break;
//                    case 1:
//                        Element.ScreenSection = newTable[rowNumber, j] as string; break;
//                    case 2:
//                        Element.Sequence = newTable[rowNumber, j] as string; break;
//                    case 3:
//                        Element.Quantity = newTable[rowNumber, j] as string; break;
//                    case 4:
//                        Element.ItemId = newTable[rowNumber, j] as string; break;
//                    case 5:
//                        {
//                            string DescriptionString = newTable[rowNumber, j] as string;
//                            if(DescriptionString == "")
//                                DescriptionString = "N/A";
//                            Element.Description = DescriptionString;
//                        }
//                        break;
//                    case 6:
//                        Element.ImagePath = newTable[rowNumber, j] as string; break;
//                    case 7:
//                        {
//                            Element.RetailItemId = newTable.getPickList(rowNumber, QuoteDictionary.RETAILITEMID);
//                            //var dictionary = new Dictionary<string, decimal>();
//                            //foreach (DictionaryEntry c in Element.RetailItemId)
//                            //{
//                            //    dictionary.Add(Convert.ToString(c.Key), Convert.ToDecimal(c.Value));
//                            //}
//                            //var misitems = from mipar in dictionary orderby mipar.Value ascending select mipar;
//                            //Hashtable u = new Hashtable();
//                            //foreach (KeyValuePair<string, decimal> otherpair in misitems)
//                            //{
//                            //    u.Add(Convert.ToString(otherpair.Key), Convert.ToString(otherpair.Value));
//                            //}
//                            //Element.test = misitems;
//                            ////Element.RetailItemId = u;
//                            Element.NumberOfPickListValues = Element.RetailItemId.Count;
//                        } break;
                        
//                    case 8:
//                        Element.MRCRetail = Convert.ToDecimal(newTable[rowNumber, j]); break;
//                    case 9:
//                        Element.DealerCost = Convert.ToDecimal(newTable[rowNumber, j]); break;
//                    case 10:
//                        Element.ElementOrderId = newTable[rowNumber, j] as string; break;
//                    case 11:
//                        Element.DetailId = newTable[rowNumber, j] as string; break;
//                }
//            }

//            return Element;
//        }


//        public ActionResult PartialQuoteView(CustomerInformationModel model)
//        {
//            return PartialView("Index", model);
//        }

//    }
//}
#endregion
