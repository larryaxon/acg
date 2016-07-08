               using System;
using System.Text;
using CCI.Common;
using System.Collections;
using System.Collections.Generic;
using CCIWebClient;
using CCIWebClient.Models;
using CCI.Common;
using CCIWebClient.Common;
//using ACG.Common;
using System.Web.Mvc;
using System.Web;
namespace CCIWebClient
{
    /// <summary>
    /// This class is used as an interface between the client and the server back end.
    /// </summary>
    public class Proxy
    {
        /// <summary>
        /// Login the user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static ServerResponse Login(string userName, string password)
        {
            return getResponse(URLHelper.LoginUrl(userName, password));
        }

        /// <summary>
        /// Validate if the security id is valid
        /// </summary>
        /// <param name="securityId"></param>
        /// <returns></returns>
        public static Boolean isValidSecurityID(int securityId)
        {
            string json = HTTPHelper.GetJSON(URLHelper.isValidSecurityId(securityId));
            ServerResponse Response = ServerResponseSerializer.FromJson(json);
             Hashtable ht  = ( Response.Results[0] as Hashtable);
             if (ht.Count > 0)
             {
                 if (ht["isvalid"].ToString() == "true")
                 {
                     return true;
                 }
             }
            return false;            
        }
        /// <summary>
        /// Get the response from the server using an URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static ServerResponse getResponse(string url)
        {
            string json = HTTPHelper.GetJSON(url);
            ServerResponse Response =   ServerResponseSerializer.FromJson(json);
            return Response;
        }
        /// <summary>
        /// Get the response from the server using an URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string getResponseJson(string url)
        {
            string json = HTTPHelper.GetJSON(url);
            return json;
        }

        public static CCITable getOrderScreenCCITable(int securityId, string newTableFormat)
        {
            ServerResponse response = getOrderScreen(securityId, newTableFormat);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return null;
            CCITable orderScreenTable = (CCITable)response.Results[0];
            return orderScreenTable;
        }

        public static CCITable getOrderScreenCCITable(int securityId, string newTableFormat, Boolean isRecommended)
        {
            ServerResponse response = getOrderScreen(securityId, newTableFormat,isRecommended);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return null;
            CCITable orderScreenTable = (CCITable)response.Results[0];
            return orderScreenTable;
        }
        /// <summary>
        /// Get the response from the server using the URL
        /// </summary>
        /// <returns></returns>
        /// 
        public static ServerResponse getOrderScreen(int securityId, string newtableformat)
        {
            return getResponse(URLHelper.getOrderScreenUrl(securityId, newtableformat));
        }

        public static ServerResponse getOrderScreen(int securityId, string newtableformat, Boolean isRecommended)
        {
            return getResponse(URLHelper.getOrderScreenUrl(securityId, newtableformat, isRecommended));
        }
        /// <summary>
        /// Gets the json table
        /// </summary>
        /// <returns></returns>
        public static string getOrderScreenJson(int securityId, string newtableformat, string orderId)
        {
            ServerResponse response = getOrderScreen(securityId, newtableformat);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return "{}";
            CCITable orderScreenTable = (CCITable)response.Results[0];
            string tableJson = "{ " + ServerResponseSerializer.ToJson(orderScreenTable) + " }";
            return tableJson;
        }

        /// <summary>
        ///  Get the response from the server using the URL
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static ServerResponse getOrderDetail(string orderId, bool isRecommended, int securityId)
        {
            return getResponse(URLHelper.getOrderDetailUrl(orderId, isRecommended, securityId));
        }

        /// <summary>
        /// Gets the json table
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static string getOrderDetailJson(string orderId, bool isRecommended, int securityId)
        {
            ServerResponse response = getOrderDetail(orderId,  isRecommended, securityId);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return "{}";
            CCITable orderDetailTable = (CCITable)response.Results[0];
            string tableJson = "{ " + ServerResponseSerializer.ToJson(orderDetailTable) + " }";
            return tableJson;
        }

        public static CCITable getOrderDetailCCITable(string orderId, bool isRecommended, int securityId)
        {
            ServerResponse response = getOrderDetail(orderId,  isRecommended, securityId);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return null;
            CCITable orderScreenTable = (CCITable)response.Results[0];
            return orderScreenTable;
        }

        public static CCITable getOrderDetailCCITable(string orderId, bool isRecommended, int securityId, string seccion)
        {
            ServerResponse response = getOrderDetail(orderId, isRecommended, securityId);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return null;
            CCITable orderScreenTable = (CCITable)response.Results[0];
            return orderScreenTable;
        }

        /// <summary>
        ///  Get the response from the server using the URL
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderName"></param>
        /// <returns></returns>
        public static ServerResponse getOrderHeader(string orderId, string orderName)
        {
            return getResponse(URLHelper.getOrderHeaderUrl(orderId, orderName));
        }

        /// <summary>
        /// Gets the json table
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderName"></param>
        /// <returns></returns>
        public static string getOrderHeaderJson(string orderId, string orderName)
        {
            ServerResponse response = getOrderHeader(orderId, orderName);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return "{}";
            CCITable orderHeaderTable = (CCITable)response.Results[0];
            string tableJson = "{ " + ServerResponseSerializer.ToJson(orderHeaderTable,false) + " }";
            return tableJson;
        }

        /// <summary>
        ///  Get the response from the server using the URL
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static ServerResponse getOrderTotal(int securityid, string orderId)
        {
            return getResponse(URLHelper.getOrderTotalUrl(securityid, orderId));
        }
        
        /// <summary>
        /// Gets the json table
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        //public static CCITable getOrderTotalTable(string orderId)
        //{
        //    ServerResponse response = getOrderTotal(orderId);
        //    if (response.Errors.Count > 1)
        //        throw new Exception(response.Errors[0].ToString());
        //    if (response.Results.Count <= 0)
        //        return null;
        //   return (CCITable)response.Results[0];
        //   // string tableJson = "{ " + ServerResponseSerializer.ToJson(orderTotalTable) + " }";
        //  //  return tableJson;
        //}

        //public static CCITable getOrderTotalTable(string orderId)
        //{
        //    ServerResponse response = getOrderTotal(orderId);
        //    if (response.Errors.Count > 1)
        //        throw new Exception(response.Errors[0].ToString());
        //    if (response.Results.Count <= 0)
        //        return null;
        //    CCITable orderTotalTable = (CCITable)response.Results[0];
        //    return orderTotalTable;
        //}
        
        /// <summary>
        /// Get the response from the server using the URL
        /// </summary>
        /// <returns></returns>
        public static ServerResponse getVersion()
        {
            return getResponse(URLHelper.getVersionUrl());
        }
        
        /// <summary>
        /// gets the json response with version number
        /// </summary>
        /// <returns></returns>
        public static string getVersionJson()
        {
            ServerResponse response = getVersion();
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return "{}";
            CCITable versionTable = (CCITable)response.Results[0];
            string tableJson = "{ " + ServerResponseSerializer.ToJson(versionTable) + " }";
            return tableJson;
        }
        
        /// <summary>
        /// Get the response from the server using the URL
        /// </summary>
        /// <returns></returns>
        public static ServerResponse getFieldValidationInfo()
        {
            return getResponse(URLHelper.getFieldValidationInfoUrl());
        }
        
        /// <summary>
        /// gets json response with field validation info
        /// </summary>
        /// <returns></returns>
        public static string getFieldValidationInfoJson()
        {
            ServerResponse response = getFieldValidationInfo();
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return "{}";
            CCITable fieldValidationTable = (CCITable)response.Results[0];
            string tableJson = "{ " + ServerResponseSerializer.ToJson(fieldValidationTable) + " }";
            return tableJson;
        }
        
        /// <summary>
        /// Get the response from the server using the URL
        /// </summary>
        /// <returns></returns>
        public static ServerResponse getPickList(int securityid, string context, string fieldnames, string criteria)  
        {
            return getResponse(URLHelper.getPickListUrl(securityid,context, fieldnames, criteria));
        }
        
        /// <summary>
        /// gets json response with pick list info
        /// </summary>
        /// <returns></returns>
        public static string getPickListJson(int securityid, string context, string fieldnames, string criteria) 
        {
            ServerResponse response = getPickList(securityid, context, fieldnames, criteria);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return "{}";
            CCITable pickListTable = (CCITable)(response.Results[0] as CCIForm)[fieldnames].Value;
            string tableJson = "{ " + ServerResponseSerializer.ToJson(pickListTable,false) + " }";
            return tableJson;
        }
        
        /// <summary>
        /// Post form data with order detail information
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public static string updateOrderDetailRecord(string securityid, string jsondata)//modified to accept security id and string formed json
        {
          
            return HTTPHelper.PostForm(URLHelper.updateOrderDetailUrl(securityid), jsondata);
        }

        public static ServerResponse getCustomerSuggestions(string name, string address, string city, string state, string zip, string dealer)
        {
            return getResponse(URLHelper.getCustomersSuggestionsUrl(name, address, city, state, zip, dealer));
        }

        public static string getCustomerSuggestionsJson(string name, string address, string city, string state, string zip, string dealer)
        {
            ServerResponse response = getCustomerSuggestions(name, address, city, state, zip, dealer);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return "{}";
            CCITable suggestionsTable = (CCITable)response.Results[0];
            string tableJson = "{ " + ServerResponseSerializer.ToJson(suggestionsTable) + " }";
            return tableJson;
        }

        public static CCITable getCustomerSuggestionsTable(string name, string address, string city, string state, string zip, string dealer)
        {
            ServerResponse response = getCustomerSuggestions(name, address, city, state, zip, dealer);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return null;
            CCITable suggestionsTable = (CCITable)response.Results[0];
            return suggestionsTable;
        }
        
        /// <summary>
        /// Return the information as a server response
        /// </summary>
        /// <param name="securityid"></param>
        /// <param name="customerid"></param>
        /// <returns></returns>
        public static ServerResponse getCustomerInfo(string securityid, string customerid)
        {
            return getResponse(URLHelper.getCustomerInfoUrl(securityid, customerid));
        }
        
        /// <summary>
        /// Return the customerinformation as json
        /// </summary>
        /// <param name="securityid"></param>
        /// <param name="customerid"></param>
        /// <returns></returns>
        public static string getCustomerInfoJson(string securityid, string customerid)
        {
            ServerResponse response = getCustomerInfo(securityid, customerid);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return null;
            CCITable clientInfo = (CCITable)response.Results[0];
            return string.Concat("{", ServerResponseSerializer.ToJson(clientInfo, false), "}");
        }
        
        /// <summary>
        /// Return the order header as a table
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderName"></param>
        /// <returns></returns>
        public static CCITable getOrderHeaderTable(string orderId, string orderName)
        {
            ServerResponse response = getOrderHeader(orderId, orderName);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return null;
            CCITable orderHeaderTable = (CCITable)response.Results[0];
            return orderHeaderTable;
        }
        
        /// <summary>
        /// Save the order
        /// </summary>
        /// <param name="securityid"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public static string SaveOrder(int securityid, FormCollection form)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < form.Count; i++)
            {
                if (!(form.GetKey(i) == "" || form.GetKey(i) == "null"))
                {
                    if (!(form.GetKey(i).ToLower().Contains("quantity") && form.Get(i) == ""))
                    {
                        if (form.Get(i) != null && form.Get(i) != "")
                            sb.Append(String.Concat(i > 0 ? "&" : "", form.GetKey(i), "=", form.Get(i).Replace('&', '|')));
                    }
                }
            }
            string detailjson = HTTPHelper.PostForm(URLHelper.updateOrderDetailUrl(securityid.ToString()), sb.ToString());
            return detailjson;
        }
    }

}
