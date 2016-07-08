using System;
using System.Web.Mvc;
using CCI.Common;
using CCIWebClient.Common;


namespace CCIWebClient.Controllers
{
    public class CCICommonController : Controller
    {
        //
        // GET: /CCICommon/

        /// <summary>
        /// Gets the order screen list
        /// </summary>
        /// <returns></returns>
        [ACGAuthorize]
        public ActionResult getOrderScreen(int securityId, string newtableformat, string orderId)
        {

            string orderScreenJson = Proxy.getOrderScreenJson(securityId, newtableformat, orderId);
            return Json(orderScreenJson, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the order detail list
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [ACGAuthorize]
        public ActionResult getOrderDetail(string orderId, bool isRecommended, int securityId)
        { 
            string orderDetailJson = Proxy.getOrderDetailJson(orderId, isRecommended, securityId);
            return Json(orderDetailJson, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the order header list
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderName"></param>
        /// <returns></returns>
        [ACGAuthorize]
        public ActionResult getOrderHeader(string orderId, string orderName)
        {
            string orderHeaderJson = Proxy.getOrderHeaderJson(orderId, orderName);
            return Json(orderHeaderJson, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get version
        /// </summary>
        /// <returns></returns>
        [ACGAuthorize]
        public ActionResult getVersion()
        {
    
            string versionJson = Proxy.getVersionJson();
            return Json(versionJson, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// gets field validation info
        /// </summary>
        /// <returns></returns>
        [ACGAuthorize]
        public ActionResult getFieldValidation()
        {
          
            string fieldValidationJson = Proxy.getFieldValidationInfoJson();
            return Json(fieldValidationJson, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// gets pick list info
        /// </summary>
        /// <returns></returns>
        [ACGAuthorize]
        public ActionResult getPickList(string context, string fieldnames, string criteria)
        {
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;

            string pickListJson = Proxy.getPickListJson(securityid, context, fieldnames, criteria);
            return Json(pickListJson, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// saves detail header record
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        [HttpPost]
        [ACGAuthorize]
        public ActionResult updateOrderDetailRecord(CCITable detail)
        {
            //string detailStrJson = Proxy.updateOrderDetailRecord(detail);
            //dont need to serialize
            return Json("");
        }

        [HttpPost]
        [ACGAuthorize]
        public ActionResult updateOrderDetailRecordJson(FormCollection json)
        {
            //ServerResponse response = ServerResponseSerializer.FromJson(json);
            //CCITable detail = response.Results[0] as CCITable;
            //dont need to serialize
            
            string detailStrJson = ""; // Proxy.updateOrderDetailRecord(securityid, json);
            return Json(detailStrJson);
        }

        [ACGAuthorize]
        public ActionResult getCustomerSuggestions(string name, string address, string city, string state, string zip, string dealer)
        {
           
            string orderSuggestionJson = Proxy.getCustomerSuggestionsJson(name, address, city, state, zip, dealer);
            return Json(orderSuggestionJson, JsonRequestBehavior.AllowGet);
        }

        [ACGAuthorize]
        public string getUserType()
        {
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;
            CCITable table = Proxy.getOrderScreenCCITable(securityid, "Yes", true);
            for (int i = 0; i < table.NumberRows; i++)
            {
                if (Convert.ToString(table[i, 0]) == QuoteDictionary.DEALERQUOTE)
                {
                    return QuoteDictionary.DEALER;
                }
            }
            return QuoteDictionary.AGENT;
        }

        [ACGAuthorize]
        public ActionResult getQuoteName(string criteria)
        {
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;
            return Json(Proxy.getPickListJson(securityid, "", "orders", criteria), JsonRequestBehavior.AllowGet);

        }

        [ACGAuthorize]
        public ActionResult getClient(string criteria)
        {
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;
            return Json(Proxy.getPickListJson(securityid, "", "dealercustomer", criteria), JsonRequestBehavior.AllowGet);

        }
        [ACGAuthorize]
        public ActionResult getCustomerInfo(string customerid)
        {
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;
            return Json(Proxy.getCustomerInfoJson(securityid.ToString(), customerid), JsonRequestBehavior.AllowGet);

        }

        [ACGAuthorize]
        [HttpPost]
        public ActionResult SaveOrder(FormCollection form)
        {
            CustomPrincipal user = CookieHelper.getUser(this.HttpContext);
            int securityid = user.SecurityId;
            string json = Proxy.SaveOrder(securityid, form);
            return Json(json, JsonRequestBehavior.AllowGet);
        }


    }
}


#region commentedcode
/// <summary>
/// saves order header record
/// </summary>
/// <param name="order"></param>
/// <returns></returns>
//[HttpPost]
//[ACGAuthorize]
//public ActionResult updateOrderHeaderRecord(CCITable order)
//{
//    string orderStrJson = Proxy.updateOrderHeaderRecord(order);
//    return Json(orderStrJson);
//}
/*
 * 
 *  [ACGAuthorize]
        //public ActionResult getOrderTotal(string orderId)
        //{
        //    string orderTotalJson = Proxy.getOrderTotalJson(orderId);
        //    return Json(orderTotalJson, JsonRequestBehavior.AllowGet);

 * //  return Json("{\"table\":{\"rows\":[[\"1\",\"Qname1\"],[\"1\",\"Qname2\"],[\"1\",\"Qname3\"]]}}", JsonRequestBehavior.AllowGet);
 * //}
 * */

#endregion