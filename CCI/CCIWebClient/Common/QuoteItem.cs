using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CCI.Common;

namespace CCIWebClient.Common
{
    public class QuoteItem
    {
        public string Screen { get; set; }
        public string ScreenSection { get; set; }
        public string Sequence { get; set; }
        public string Quantity { get; set; }
        public string ItemId { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string RetailItemId { get; set; }
        public PickListEntries _RetailItemId { get; set; }
        public string MRCRetail { get; set; }
        public string RetailMRC { get; set; }
        public string RetailNRC { get; set; }
        public string DealerCost { get; set; }
        public string YourCost { get; set; }
        public string ElementOrderId { get; set; }
        public string DetailId { get; set; }
        public string Price { get; set; }
        public string Quote { get; set; }
        public int NumberOfPickListValues { get; set; }
        public List<QuotePickList> PickList { get; set; }
        public string ItemDescription { get; set; }
        public string CarrierDescription { get; set; }
       
        public string Vendor { get; set; }
        public string Monthly { get; set; }
        public string Install { get; set; }
        public string Variable { get; set; }
        
        public string PhoneMakeModel { get; set; }
        public string VendorEmail { get; set; }
        public string VendorDescription { get; set; }
        public string VendorPhone { get; set; }
        public string ConectionType { get; set; }
        public string ContractExpirationDate { get; set; }
        public string ContactName { get; set; }
        public string CarrierEmail { get; set; }
        public string CarrierPhone { get; set; }
        public Boolean IsVariable { get; set; }

        public SelectList MRCRetailList
        {
            get
            {
                //CAC- default to highest price
                int i = 0;
                String defaultSelectedItemID=null;

                List<QuotePickList> list = new List<QuotePickList>();
                if (_RetailItemId != null)
                {
                  //CAC - false
                    _RetailItemId.SortedByAscending = false;
                    SortedDictionary<string, PickListEntry> pickList = _RetailItemId.SortedBy("Description");
                    List<QuotePickList> secondList = new List<QuotePickList>();
                    foreach (KeyValuePair<string, PickListEntry> item in pickList)
                    {
                      if (i == 0)
                      {
                        defaultSelectedItemID = item.Value.ID;
                        i++;
                      }
                      QuotePickList q = new QuotePickList { Text = CommonFunctions.fixupDollarFormat(item.Value.Description, true), Value = item.Value.ID };
                      if (item.Value.Description.Equals("Variable", StringComparison.CurrentCultureIgnoreCase))
                        secondList.Add(q); // we put "Variable" prices at the bottom of the list, so we save them here
                      else
                        list.Add(q);
                    }
                    foreach (QuotePickList q in secondList) // then we add any Variable items here
                      list.Add(q);
                }
                //return new SelectList(list, "Value", "Text", RetailItemId);
                return new SelectList(list, "Value", "Text", defaultSelectedItemID);
            }
        }
    }

    public class QuotePickList
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}