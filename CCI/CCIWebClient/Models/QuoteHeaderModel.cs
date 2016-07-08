using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCI.Common;

namespace CCIWebClient.Models
{
    public class QuoteHeaderModel
    {
        public string Id { get; set; }
        public string ShortName { get; set; }
        public string Customer { get; set; }
        public string LegalName { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string OrderType { get; set; }
        public string ContractTerm { get; set; }
        public string Status { get; set; }
        public string InstallationCosts { get; set; }
        public string CreditCardName { get; set; }
        public string CreditCardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string SecurityCode { get; set; }
       
        public string AmountToPay { get; set; }
        public string PhonesFrom { get; set; }
        public string CarrierServices { get; set; }

        public QuoteHeaderModel()
        {
            Id = "0";
            Customer = "0";
            Name = "";
            Address1 = "";
            Address2 = "";
            City = "";
            State = "";
            Zip = "";
            Status = "";
            OrderType = "";
            LegalName = "";
            PhonesFrom = "cityhosted";
            CarrierServices = "cityhosted";
        }

        public QuoteHeaderModel(int quoteId, string NewCustomerId, string NewName, string NewAddress, string NewSuite, string NewCity, string NewState, string NewZip)
        {
            Id = Convert.ToString(quoteId);
            Customer = NewCustomerId;
            Name = NewName;
            Address1 = NewAddress;
            Address2 = NewSuite;
            City = NewCity;
            State = NewState;
            Zip = NewZip;
        }

        public QuoteHeaderModel(int quoteId, string quoteName)
        {    
            ServerResponse response = Proxy.getOrderHeader(quoteId.ToString(), quoteName);
            if (response.Errors.Count > 0)
                throw new Exception(response.Errors[0].ToString());
            if (response.Results.Count == 0)
                throw new Exception("Quote doesn't exists");
            CCITable table = response.Results[0] as CCITable;
            this.Id = quoteId.ToString();
            this.Customer = CommonFunctions.CString(table[0, "customer"]);
            this.Address1 = CommonFunctions.CString(table[0, "address1"]);
            this.Address2 = CommonFunctions.CString(table[0, "address2"]);
            this.LegalName = CommonFunctions.CString(table[0, "legalname"]);
            this.City = CommonFunctions.CString(table[0, "city"]);
            this.State = CommonFunctions.CString(table[0, "state"]);
            this.Zip = CommonFunctions.CString(table[0, "zip"]);
            this.OrderType = CommonFunctions.CString(table[0, "ordertype"]);
            this.ContractTerm = CommonFunctions.CString(table[0, "contractterm"]);
            this.ShortName = CommonFunctions.CString(table[0, "shortname"]);
            this.InstallationCosts = CommonFunctions.CString(table[0, "installationcosts"]); ;
            this.CreditCardName= CommonFunctions.CString(table[0, "creditcardname"]); ;
            this.CreditCardNumber= CommonFunctions.CString(table[0, "creditcardnumber"]); ;
            this.SecurityCode = CommonFunctions.CString(table[0, "securitycode"]); ;
            this.ExpirationDate= CommonFunctions.CString(table[0, "expirationdate"]); ;
            this.AmountToPay = CommonFunctions.CString(table[0, "amounttopay"]); ;

            this.PhonesFrom = CommonFunctions.CString(table[0, "phonesfrom"]);
            this.CarrierServices = CommonFunctions.CString(table[0, "carrierservices"]);
        }
    }
}