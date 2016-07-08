using System;
using System.Collections.Generic;
using System.Text;

namespace CCI.Common
{
  public class MRCMatchingSelectedItem
  {    
      /*
ID	Customer	Whsle BTN	Rtl BTN	Whsle Bill Date	Rtl Bill Date	Whsle USOC	Whsle USOC Name	Rtl USOC	Rtl USOC Name	Whsle Qty	Wholesale Amt	Rtl Qty	Rtl Amt	MatchedBy	LastModifiedBy	LastModifiedDateTime
283459	CITY COMMUNICATIONS INTEGRATED, LLC.	6025150306	NULL	2012-10-01	NULL	WDIDN	DID Numbers	NULL	NULL	1	0.1	NULL	NULL	Auto:Wholesale Except	Andy-10	2013-02-18 20:48:54.560
       */
      public const int MAXFIELD = 12;
      public string ID = string.Empty;
      public string Customer = string.Empty;
      public string WholesaleBTN = string.Empty;
      public string RetailBtn = string.Empty;
      public DateTime? BillDate = null;
      public string WholesaleUSOC = string.Empty;
      public string RetailUSOC = string.Empty;
      public int WholesaleQuantity = 0;
      public decimal WholesaleAmount = 0;
      public int RetailQuantity = 0;
      public decimal RetailAmount = 0;
      public MRCMatchingSelectedItem(string record)
      {
        setFromRecord(record);
      }
      private void setFromRecord(string record)
      {
        if (string.IsNullOrEmpty(record))
          return;
        string[] fields = record.Split(new char[] { ':' });
        int iFields = MAXFIELD;
        if (fields.GetLength(0) < MAXFIELD)
          iFields = fields.GetLength(0);
        for (int iField = 0; iField < iFields; iField++)
          setValue(iField, fields[iField]);
      }
      private void setValue(int index, string value)
      {
        switch (index)
        {
          case 0:
            ID = value;
            break;
          case 1:
            Customer = value;
            break;
          case 2:
            WholesaleBTN = value;
            break;
          case 3:
            RetailBtn = value;
            break;
          case 4:
            BillDate = CommonFunctions.CDateTime(value);
            break;
          case 5:
            WholesaleUSOC = value;
            break;
          case 7:
            RetailUSOC = value;
            break;
          case 9:
            WholesaleQuantity = CommonFunctions.CInt(value);
            break;
          case 10:
            WholesaleAmount = CommonFunctions.CDecimal(value);
            break;
          case 11:
            RetailQuantity = CommonFunctions.CInt(value);
            break;
          case 12:
            RetailAmount = CommonFunctions.CDecimal(value);
            break;
        }
      }
  }
}
