using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCI.Common;

namespace CCIWebClient.Models
{
    public class customerSuggested
    {
        public string QuoteId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Suite { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    public class CustomerSuggestedModel
    {
        public List<customerSuggested> Customers;
        public CustomerSuggestedModel(string name, string address, string suite, string city, string state, string zip) //string dealer
        {
            Customers = new List<customerSuggested>();
            CCITable table = Proxy.getCustomerSuggestionsTable(name, address, suite, city, state, zip); //dealer
            //CCITable HeaderTable = Proxy.getOrderHeaderTable(orderId, name);
            for (int i = 0; i < table.NumberRows; i++) {

                Customers.Add(new customerSuggested()
                {
                    QuoteId = table[i, "entity"] as string,
                    CustomerName = table[i, "legalname"] as string,
                    Address = table[i, "address1"] as string,
                    //Suite = table[i, "entitytype"] as string,
                    City = table[i, "city"] as string,
                    State = table[i, "state"] as string,
                    ZipCode = table[i, "zip"] as string

                    //TO TEST:
                    //QuoteId = "qid001",
                    //CustomerName = "Leonardo",
                    //Address = "hisAddress",
                    ////Suite = table[i, "entitytype"] as string,
                    //City = "Teguz",
                    //State = "FCOMOR",
                    //ZipCode = "12345"
                });
            }
        }
    }
}