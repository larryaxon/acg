using System;
using CCIWebClient.Models;
using CCI.Common;
using CCIWebClient;
using CCIWebClient.Common;
using System.Collections;

namespace CCI.Common
{

    /// <summary>
    /// This class is used to construct the URL used by the proxy to get the data from the server side.
    /// </summary>
    public static class URLHelper
    {
        /// <summary>
        /// This 
        /// </summary>
        private const string _loginUrl = "{0}/{1}?command=Login&username={2}&password={3}";
        private const string _isValidSecurityIDUrl = "{0}/{1}?command=isvalidsecurityid&securityid={2}";
        private const string _getOrderScreen = @"{0}/{1}?command=getorderscreen&securityid={2}&newtableformat={3}";
        private const string _getOrderScreen2 = @"{0}/{1}?command=getorderscreen&securityid={2}&newtableformat={3}&isrecommended={4}";
        private const string _getOrderDetail = @"{0}/{1}?command=getorderdetail&orderid={2}&isrecommended={3}&securityid={4}";
        private const string _getOrderDetail2 = @"{0}/{1}?command=getorderdetail&orderid={2}&isrecommended={3}&securityid={4}&seccion={5}";
        private const string _getOrderHeader = @"{0}/{1}?command=getorderheader&orderid={2}&ordername={3}&newtableformat=No";
        private const string _getOrderTotal = @"{0}/{1}?command=getordertotals&securityid={2}&orderid={3}";
        private const string _getVersion = @"{0}/{1}?command=getversion";
        private const string _getFieldValidationInfo = @"{0}/{1}?command=getfieldvalidationinfo";
        private const string _getCustomerSuggestions = "{0}/{1}?command=getcustomersuggestions&name={2}&address={3}&city={4}&state={5}&zip={6}&dealer={7}";
        private const string _getCustomerInfo = "{0}/{1}?command=getcustomerinfo&securityid={2}&customerid={3}";
        private const string _getPickList = @"{0}/{1}?command=getpicklists&securityid={2}&context={3}&fieldnames={4}&criteria={5}&newtableformat=No";
        private const string _updateOrderHeader = @"{0}/{1}?command=updateorderheader&securityid={2}&newtableformat=No";
        private const string _updateOrderDetail = @"{0}/{1}?command=updateorderdetail&securityid={2}&newtableformat=No";//added security id

        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityid"></param>
        /// <param name="customerid"></param>
        /// <returns></returns>
        public static string getCustomerInfoUrl(string securityid, string customerid)
        {
            return string.Format(_getCustomerInfo, ConfigHelper.getDefaultServer(), ConfigHelper.getDefaultPage(), securityid, customerid);
        }

        /// <summary>
        /// URL used to log in
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        public static string LoginUrl(string username, string password)
        {

            return string.Format(_loginUrl, ConfigHelper.getDefaultServer(), ConfigHelper.getDefaultPage(), username, password);
        }

        /// <summary>
        /// Returnif a security id is valid or not
        /// </summary>
        /// <param name="securityId"></param>
        /// <returns></returns>
        public static string isValidSecurityId(int securityId)
        {
            return string.Format(_isValidSecurityIDUrl, ConfigHelper.getDefaultServer(), ConfigHelper.getDefaultPage(), securityId);
        }

        /// <summary>
        /// Returns json for orderscreen
        /// </summary>
        /// <returns></returns>
        public static string getOrderScreenUrl(int securityId, string newtableformat)
        {
            return string.Format(_getOrderScreen, ConfigHelper.getDefaultServer(), ConfigHelper.getDefaultPage(), securityId, newtableformat);
        }

        public static string getOrderScreenUrl(int securityId, string newtableformat, Boolean isRecommended)
        {
            return string.Format(_getOrderScreen2, ConfigHelper.getDefaultServer(), ConfigHelper.getDefaultPage(), securityId, newtableformat, isRecommended);
        }
        /// <summary>
        /// returns json for order detail
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderName"></param>
        /// <returns></returns>
        public static string getOrderDetailUrl(string orderId, bool isRecommended, int securityId)
        {
            return string.Format(_getOrderDetail, ConfigHelper.getDefaultServer(), ConfigHelper.getDefaultPage(), orderId, isRecommended, securityId);
        }
        public static string getOrderDetailUrl(string orderId, bool isRecommended, int securityId, string seccion)
        {
            return string.Format(_getOrderDetail2, ConfigHelper.getDefaultServer(), ConfigHelper.getDefaultPage(), orderId, isRecommended, securityId, seccion);
        }
        /// <summary>
        /// returns json for order header
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderName"></param>
        /// <returns></returns>
        public static string getOrderHeaderUrl(string orderId, string orderName)
        {
            return string.Format(_getOrderHeader, ConfigHelper.getDefaultServer(), ConfigHelper.getDefaultPage(), orderId, orderName);
        }

        /// <summary>
        /// returns json for order total
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static string getOrderTotalUrl(int securityid, string orderId)
        {
            return string.Format(_getOrderTotal, ConfigHelper.getDefaultServer(), ConfigHelper.getDefaultPage(),securityid.ToString(), orderId);
        }

        /// <summary>
        /// returns the json with version number
        /// </summary>
        /// <returns></returns>
        public static string getVersionUrl()
        {
            return string.Format(_getVersion, ConfigHelper.getDefaultServer(), ConfigHelper.getDefaultPage());
        }

        /// <summary>
        /// returns json with field validation info
        /// </summary>
        /// <returns></returns>
        public static string getFieldValidationInfoUrl()
        {
            return string.Format(_getFieldValidationInfo, ConfigHelper.getDefaultServer(), ConfigHelper.getDefaultPage());
        }

        /// <summary>
        /// returns json with picklist information
        /// </summary>
        /// <returns></returns>
        public static string getPickListUrl( int securityid, string context, string fieldnames, string criteria)  
        {
            return string.Format(_getPickList, ConfigHelper.getDefaultServer(), ConfigHelper.getDefaultPage(),  securityid.ToString(), context, fieldnames, criteria);
        }

        /// <summary>
        /// creates the order header update url
        /// </summary>
        /// <returns></returns>
        public static string updateOrderHeaderUrl(string securityid)
        {
            return string.Format(_updateOrderHeader, ConfigHelper.getDefaultServer(), ConfigHelper.getDefaultPage(), securityid);
        }

        /// <summary>
        /// creates the order detail update url
        /// </summary>
        /// <returns></returns>
        public static string updateOrderDetailUrl(string securityid)
        {
            return string.Format(_updateOrderDetail, ConfigHelper.getDefaultServer(), ConfigHelper.getDefaultPage(), securityid);
        }

        public static string getCustomersSuggestionsUrl(string name, string address, string city, string state, string zip, string dealer) //needs more work!!
        {
            return string.Format(_getCustomerSuggestions, ConfigHelper.getDefaultServer(), ConfigHelper.getDefaultPage(), name, address, city, state, zip, dealer);
        }
        
    }


}


#region commented code

#endregion
