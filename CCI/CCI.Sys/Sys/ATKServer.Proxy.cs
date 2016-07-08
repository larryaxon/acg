using System;
using System.Text;
using ATK.Sys;
using ATK.Common;

namespace ATK.Sys
{
    /// <summary>
    /// This class is used as an interface between the client and the server back end.
    /// </summary>
    public class Proxy
    {
        /// <summary>
        /// This class is responsible to get the data from the server side using HTTP protocol
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static ATKServerResponse getRecentClientMatterList(string securityId, int maxListSize, Boolean includeInactive, string sortBy, string criteria)
        {
            return getResponse(URLHelper.getRecentClientMatterListUrl(securityId, maxListSize, includeInactive, sortBy, criteria));
        }
        /// <summary>
        /// Login method used to login into the database
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns>Return the atkresponse object</returns>
        public static ATKServerResponse Login(string userName, string password, string domain)
        {
            return getResponse(URLHelper.getLoginUrl(userName, password, domain));
        }
        /// <summary>
        /// This class process a url and return an ATKServerResponse
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static ATKServerResponse getResponse(string url)
        {
            string json = HTTPHelper.GetJSON(url);
            ATKServerResponse atkResponse = ATKSerializer.FromJson(json);
            return atkResponse;
        }
        /// <summary>
        /// Get the time detail
        /// </summary>
        /// <param name="securityId"></param>
        /// <param name="account"></param>
        /// <param name="client"></param>
        /// <param name="matter"></param>
        /// <returns></returns>

        public static ATKServerResponse getTimeDetail(string securityId, string account, string client, string matter)
        {
            return getResponse(URLHelper.getTimeDetailUrl(securityId, account, client, matter));

        }

        public static ATKServerResponse getTimeDetail2(string securityId, string account, string timeId)
        {
            return getResponse(URLHelper.getTimeDetail2Url(securityId, account, timeId));

        }
        /// <summary>
        /// Get the time detail serialized to Json
        /// </summary>
        /// <param name="securityId"></param>
        /// <param name="account"></param>
        /// <param name="client"></param>
        /// <param name="matter"></param>
        /// <returns></returns>
        public static string getTimeDetailJson(string securityId, string account, string client, string matter)
        {
            ATKServerResponse response = getTimeDetail(securityId, account, client, matter);
            ATKTable matters = (ATKTable)response.Results[0];
            string tableJson = "{ " + ATKSerializer.ToJson(matters) + " }";
            return tableJson;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityId"></param>
        /// <param name="account"></param>
        /// <param name="client"></param>
        /// <param name="matter"></param>
        /// <returns></returns>
        public static string getTimeDetail2Json(string securityId, string account, string timeid)
        {
            ATKServerResponse response = getTimeDetail2(securityId, account, timeid);
            ATKTable matters = (ATKTable)response.Results[0];
            string tableJson = "{ " + ATKSerializer.ToJson(matters) + " }";
            return tableJson;
        }

        /// <summary>
        /// Get the Response object for a particular picklist
        /// </summary>
        /// <param name="securityId"></param>
        /// <param name="context"></param>
        /// <param name="client"></param>
        /// <param name="fieldname"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static ATKServerResponse getClientPickList(string securityId, string context, string fieldname, string criteria)
        {
            return getResponse(URLHelper.getClientPickListUrl(securityId, context, fieldname, criteria));
        }
        /// <summary>
        /// Get the Json serialiezed for a particular pick list table.
        /// </summary>
        /// <param name="securityId"></param>
        /// <param name="context"></param>
        /// <param name="client"></param>
        /// <param name="fieldname"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static string getClientPickListJson(string securityId, string context, string fieldname, string criteria)
        {
            ATKServerResponse response = getClientPickList(securityId, context, fieldname, criteria);
            ATKTable picklist = (ATKTable)((ATKForm)response.Results[0])["client"].Value;
            string tableJson = "{ " + ATKSerializer.ToJson(picklist) + " }";
            return tableJson;
        }
        /// <summary>
        /// Get Pick List
        /// </summary>
        /// <param name="securityId"></param>
        /// <param name="context"></param>
        /// <param name="fieldname"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static ATKServerResponse getPickList(string securityId, string context, string client, string fieldname, string criteria)
        {
            return getResponse(URLHelper.getPickListUrl(securityId, context, client, fieldname, criteria));
        }


        public static string getPickListJson(string securityId, string context, string client, string fieldname, string criteria)
        {
            ATKServerResponse response = getPickList(securityId, context, client, fieldname, criteria);
            ATKTable picklist = (ATKTable)((ATKForm)response.Results[0])[fieldname].Value;
            string tableJson = "{ " + ATKSerializer.ToJson(picklist) + " }";
            return tableJson;
        }

       
        /// <summary>
        /// Post form data 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string UpdateTimeRecord(string timeid, string securityId, string client, string matter,
            string phase, string task, string activity, 
            string description, string internalNotes,
            DateTime startDateTime, DateTime endDateTime, decimal billedtime)
        {
            string data = string.Format("timeid={0}&client={1}&matter={2}&phase={3}&task={4}&activity={5}&description={6}&internalnotes={7}&startdatetime={8}&enddatetime={9}&billedtime={10}",
               timeid, client, matter, phase, task, activity,  description, internalNotes, startDateTime.ToString(), endDateTime.ToString(), billedtime.ToString());
            return HTTPHelper.PostForm(URLHelper.postCreateTimeRecord(securityId), data);
        }
/// <summary>
/// 
/// </summary>
/// <param name="timeid"></param>
/// <param name="securityId"></param>
/// <param name="client"></param>
/// <param name="matter"></param>
/// <param name="phase"></param>
/// <param name="task"></param>
/// <param name="activity"></param>
/// <param name="description"></param>
/// <param name="internalNotes"></param>
/// <param name="startDateTime"></param>
/// <param name="endDateTime"></param>
/// <returns></returns>
/// 
        /*   account
       , client
       , matter
       , description
       , shortName
       , displayStatus
       , lastModifiedBy;

      DateTime
       startDate
       , endDate
       , referenceDate
       , lastModifiedDateTime
       , lastUsedDateTime;*/
        public static string UpdateMatterRecord(
        
        string securityid,
        string account,
        string client,
        string matter,
        string description,
        string shortName,
        string displayStatus,
        string lastModifiedBy,
        DateTime startDate,
        DateTime endDate,
        DateTime referenceDate,
        DateTime lastModifiedDateTime,
        DateTime lastUsedDateTime,
        string dsphase,
        string dstask,
        string dsactivity,
        string dsdescription,
        string dsinternalnotes,
        string dsoverridestandardrate,
        string dsusetimer,
        string dsautostarttimer,
        string dstimeincrement
                         
            )
        {
            string data = string.Format("client={0}&matter={1}&description={2}&shortName={3}&displayStatus={4}&lastModifiedBy={5}&startDate={6}&endDate={7}&referenceDate={8}&lastModifiedDateTime={9}&lastUsedDateTime={10}&account={11}&phase={12}&task={13}&activity={14}&description2={15}&internalnotes={16}&overridestandardrate={17}&usetimer={18}&autostarttimer={19}&timeincrement={20}",
        client, matter, description, shortName, displayStatus, lastModifiedBy, startDate, endDate, referenceDate, lastModifiedDateTime, lastUsedDateTime, account,
        dsphase, dstask, dsactivity, dsdescription, dsinternalnotes, dsoverridestandardrate, dsusetimer, dsautostarttimer, dstimeincrement);
            return HTTPHelper.PostForm(URLHelper.postUpdateMatterRecord(securityid), data);
        }       

        //public static string UpdateAddMatterRecord(string securityId, string client, string matter, string description)
        //{
        //    string data = string.Format("client={0}&matter={1}",client, matter);
        //    return HTTPHelper.PostForm(URLHelper.postUpdateMatterRecord(securityId), data);
        //}
/// <summary>
/// 
/// </summary>
/// <param name="securityid"></param>
        /// <param name="account"></param>                          
/// <param name="user"></param>
/// <param name="timeKeepers"></param>
/// <param name="clients"></param>
/// <param name="clientNames"></param>
/// <param name="matters"></param>
/// <param name="matterDescriptions"></param>
/// <param name="fromDate"></param>
/// <param name="toDate"></param>
/// <param name="phases"></param>
/// <param name="tasks"></param>
/// <param name="activities"></param>
/// <param name="sortBy"></param>
/// <returns></returns>
        public static ATKServerResponse getFilteredTimeJson(
            string securityid,
            string account,
            string user,
            string timeKeepers,
            string clients,
            string clientNames,
            string matters,
            string matterDescriptions,
            DateTime fromDate,
            DateTime toDate,
            string phases,
            string tasks,
            string activities,
            string sortBy)
        {
            return getResponse(URLHelper.getFilteredTimeUrl( securityid, account, user, timeKeepers, clients, clientNames, matters, matterDescriptions, fromDate, toDate, phases, tasks, activities, sortBy));
        }

        /// <summary>
        /// Return the account info
        /// </summary>
        /// <param name="securityId"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static ATKServerResponse getAccountInfo(string securityId, string account)
        {
            return getResponse(URLHelper.getAccountInfoUrl(securityId, account));
        }

        /// <summary>
        /// This class returns Json table with account info used in common controller.
        /// </summary>
        public static string getAccountInfoJson(string securityId, string account)
        {
            ATKServerResponse response = getAccountInfo(securityId, account);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return "{}";
            ATKTable accountsTable = (ATKTable)response.Results[0];
            string tableJson = "{ " + ATKSerializer.ToJson(accountsTable) + " }";
            return tableJson;
        }

        /// <summary>
        /// This class returns clients detail information, this will be used fro Json calls.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ATKServerResponse getClientDetail(string securityId, string account, string client)
        {
            return getResponse(URLHelper.getClientDetailUrl(securityId, account, client));
        }

        /// <summary>
        /// This class returns Json table with clientdetails used in common controller.
        /// </summary>
        public static string getClientDetailJson(string securityId, string account, string client)
        {
            ATKServerResponse response = getClientDetail(securityId, account, client);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return "{}";
            ATKTable clientsTable = (ATKTable)response.Results[0];
            string tableJson = "{ " + ATKSerializer.ToJson(clientsTable) + " }";
            return tableJson;
        }

        /// <summary>
        /// This class returns matters detail information, this will be used for Json calls.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ATKServerResponse getMatterDetail(string securityId, string client, string matter)
        {
            return getResponse(URLHelper.getMatterDetailUrl(securityId, client,matter));
        }

        /// <summary>
        /// This class returns Json table with matter details used in common controller
        /// </summary>
        public static string getMatterDetailJson(string securityId, string client, string matter)
        {
            ATKServerResponse response = getMatterDetail(securityId, client, matter);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0 )
                return "{}";
            ATKTable mattersTable = (ATKTable)response.Results[0];
            string tableJson = "{ " + ATKSerializer.ToJson(mattersTable) + " }";
            return tableJson;
        }

        /// <summary>
        /// This class returns filtered clients, this will be used for Json calls
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ATKServerResponse getFilteredClients(string securityId, string account, int maxlistsize, string client)
        {
            return getResponse(URLHelper.getFilteredClientsUrl(securityId, account, maxlistsize,client));
        }
        
        /// <summary>
        /// This class returns Json table with filtered client details used in common controller.
        /// </summary>
        public static string getFilteredClientsJson(string securityId, string account, int maxlistsize, string client)
        {
            ATKServerResponse response = getFilteredClients(securityId, account, maxlistsize, client);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return "{}";
            ATKTable filteredclientsTable = (ATKTable)response.Results[0];
            string tableJson = "{ " + ATKSerializer.ToJson(filteredclientsTable) + " }";
            return tableJson;
        }

        /// <summary>
        /// This class returns filtered matters, this will be used for Json calls.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ATKServerResponse getFilteredMatters(string securityId, string account, int maxlistsize, string matters)
        {
            return getResponse(URLHelper.getFilteredMattersUrl(securityId, account, maxlistsize, matters));
        }
        
        /// <summary>
        /// This class returns Json table with filtered matter details used in common controller.
        /// </summary>
        public static string getFilteredMattersJson(string securityId, string account, int maxlistsize, string matters)
        {
            ATKServerResponse response = getFilteredMatters(securityId, account, maxlistsize, matters);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return "{}";
            ATKTable filteredmattersTable = (ATKTable)response.Results[0];
            string tableJson = "{ " + ATKSerializer.ToJson(filteredmattersTable) + " }";
            return tableJson;
        }

        /// <summary>
        /// This class returns billing code detail information, this will be used for Json calls.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ATKServerResponse getFilteredBillingCode(string securityId, string account, string client, int maxlistsize, string criteria)
        {
            return getResponse(URLHelper.getFilteredBillingCodeUrl(securityId,account,client,maxlistsize,criteria));
        }
        
        /// <summary>
        /// This class returns Json table with filtered billing code information used in common controller.
        /// </summary>
        public static string getFilteredBillingCodeJson(string securityId, string account, string client, int maxlistsize, string criteria)
        {
            ATKServerResponse response = getFilteredBillingCode(securityId, account, client, maxlistsize, criteria);
            if (response.Errors.Count > 1)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count <= 0)
                return "{}";
            ATKTable filteredbillingcodesTable = (ATKTable)response.Results[0];
            string tableJson = "{ " + ATKSerializer.ToJson(filteredbillingcodesTable) + " }";
            return tableJson;
        }

        /// <summary>
        /// Using same fields from Accout Table on DB
        /// </summary>
        public static string UpdateAccountRecord(string securityid, string account, string accountname, string licensetype, string address1, string address2, string city, string state, string zip, string country, string phone, string website, string billingid, string billingnotes,
                                                string createdby, DateTime createddatetime, string lastmodifiedby, DateTime lastmodifieddate, string longpath, DateTime startdate, DateTime enddate)
        {
            string data = string.Format("account={0}&accountName={1}&licenseType={2}&address1={3}&address2={4}&city={5}&state={6}&zip={7}&country={8}&phone={9}&website={10}&billingId={11}&billingNotes={12}&createdBy={13}&createdDatetime={14}&lastModifiedby={15}&lastModifiedDate={16}&longPath={17}&startDate={18}&endDate={19}",
                account, accountname, licensetype, address1, address2, city, state, zip, country, phone, website, billingid, billingnotes, createdby, createddatetime, lastmodifiedby, lastmodifieddate, longpath, startdate, enddate);
            return HTTPHelper.PostForm(URLHelper.postUpdateAccountRecord(securityid), data);
        }
        /// <summary>
        /// 
        /// </summary>
        public static string UpdateActivityRecord(string securityid, string account, string codeset, string code, string description, DateTime lastmodifieddatetime, DateTime startdate, DateTime enddate)
        {
            string data = string.Format("account={0}&codeset={1}&code={2}&description={3}&lastModifiedDatetime={4}&startDate={5}&endDate={6}", account, codeset, code, description, lastmodifieddatetime, startdate, enddate);
            return HTTPHelper.PostForm(URLHelper.postUpdateActivityRecord(securityid), data);
        }
        
        /// <summary>
        /// using same fields from Client Table on DB
        /// </summary>
        public static string UpdateClientRecord(string securityid, string account, string client, bool isindiviual, string name, string firstname, string shortname, string salutation, string suffix, string address1, string address2, string city, string state, string zip, string country,
                                                string phone, string email, string website, string fein, DateTime startdate, DateTime enddate)
        {
            string data = string.Format("account={0}&client={1}&isIndividual={2}&name={3}&firstName={4}&shortName={5}&salutation={6}&suffix={7}&address1={8}&address2={9}&city={10}&state={11}&zip={12}&country={13}&phone={14}&email={15}&website={16}&fein={17}&startDate={18}&endDate={19}",
                                        account, client, isindiviual, name, firstname, shortname, salutation, suffix, address1, address2, city, state, zip, country, phone, email, website, fein, startdate, enddate);
            return HTTPHelper.PostForm(URLHelper.postUpdateClientRecord(securityid), data);
        }
        /// <summary>
        /// 
        /// </summary>
        public static string UpdatePhaseRecord(string securityid, string account, string codeset, string code, string description, DateTime lastmodifieddatetime, DateTime startdate, DateTime enddate)
        {
            string data = string.Format("account={0}&codeset={1}&code={2}&description={3}&lastModifiedDatetime={4}&startDate={5}&endDate={6}",account, codeset, code, description, lastmodifieddatetime,startdate, enddate);
            return HTTPHelper.PostForm(URLHelper.postUpdatePhaseRecord(securityid), data);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static string UpdateTaskRecord(string securityid, string account, string codeset, string code, string description, DateTime lastmodifieddatetime, DateTime startdate, DateTime enddate)
        {
            string data = string.Format("account={0}&codeset={1}&code={2}&description={3}&lastModifiedDatetime={4}&startDate={5}&endDate={6}", account, codeset, code, description, lastmodifieddatetime, startdate, enddate);
            return HTTPHelper.PostForm(URLHelper.postUpdateTaskRecord(securityid), data);
        }

    }
       
}
