using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using CCI.Common;
using CCI.Sys.Data;
using CCI.Common.Logging;
using CCI.Sys.SecurityEngine;

namespace CCI.Sys.Server
{
  public partial class CCIServer
  {
    private ServerResponse updateOrderHeader(CCITable OrderHeaderRequest, ServerResponse response)
    {
      /*
       * TODO: We will build the Order CCITable base on the call the the [vw_DBColumnDetail] call
       * This view gives us all the fields (columns) belonging to this Table, it does not identify the 
       * primary key of the table, so that will be "hardcoded" for now in this code
       * 
       * I will need to create an arraylist that will populate with the 
       */
      if (OrderHeaderRequest.NumberRows != 1)
      {
        response.Errors.Add("Update must have exactly one header record reference!!");
        return response;
      }
      OrderHeaderRequest.SetKeyColumns(new string[] { CommonData.fieldID });

      CCITable OrderFields = _dataSource.getTableFieldListData(CommonData.tableORDERS);
      Hashtable OrderFieldsList = new Hashtable();
      for (int i = 0; i < OrderFields.NumberRows; i++)
        OrderFieldsList.Add(
          CommonFunctions.CString(OrderFields[i, CommonData.fieldCOLUMNNAME]).ToLower().Trim(),
          CommonFunctions.CString(OrderFields[i, CommonData.fieldSYSTEMDATATYPE]).ToLower().Trim());

      string[] columnNames = new string[OrderFieldsList.Count];
      string[] columnTypes = new string[OrderFieldsList.Count];
      OrderFieldsList.Keys.CopyTo(columnNames, 0);
      OrderFieldsList.Values.CopyTo(columnTypes, 0);

      CCITable OrderHeaderToUpdate = new CCITable();
      OrderHeaderToUpdate.SetColumns(columnNames, columnTypes);
      OrderHeaderToUpdate.SetKeyColumns(new string[] { CommonData.fieldID });

      object[] rowDefaultValues = new object[columnNames.GetLength(0)];
      for (int i = 0; i < OrderHeaderToUpdate.NumberColumns; i++)
        rowDefaultValues[i] = CommonData.NOCHANGE;
      OrderHeaderToUpdate.AddRow(rowDefaultValues);

      string fieldName = string.Empty;
      string requestFieldName = string.Empty;
      string dealer = _dataSource.getEntityOwner(response.SecurityContext.User);
      int orderID = 0;
      //In this case we ONLY have one row, so we update that row...
      for (int i = 0; i < OrderHeaderToUpdate.NumberColumns; i++)
      {
        fieldName = OrderHeaderToUpdate.Column(i);
        switch(fieldName.ToLower()) 
        {
          case "ordertype":
            OrderHeaderToUpdate[0, fieldName] = CommonData.ORDERTYPEQUOTE;
            break;
          case "status":
            OrderHeaderToUpdate[0, fieldName] = CommonData.ORDERSTATUSOPEN;
            break;
          case "laststatuschangedate":
          case "createdatetime":
          case "lastmodifieddatetime":
            OrderHeaderToUpdate[0, fieldName] = DateTime.Now.ToString(CommonData.FORMATLONGDATETIME);
            break;
          case "dealerorsalesperson":
            OrderHeaderToUpdate[0, fieldName] = dealer;
            break;
          case "createdby":
          case "lastmodifiedby":
            OrderHeaderToUpdate[0, fieldName] = response.SecurityContext.User;
            break;
          case "shortname": // Order Header Short Name
            string shortName = CommonFunctions.CString(OrderHeaderRequest[0, fieldName]);
            if (string.IsNullOrEmpty(shortName) || shortName.Equals(","))
            {
              if(OrderHeaderRequest.ContainsColumn("Name"))
                shortName = CommonFunctions.CString(OrderHeaderRequest[0, "Name"]);
              else
                shortName = string.Empty;
            }

            string dealerName = CommonFunctions.CString(_dataSource.getEntityValue(dealer, CommonData.fieldLEGALNAME)).ToUpper().Replace(" ", "").Replace("'", "").Replace("/", "");
            //CAC- replace unsavory characters...
            shortName = dealerName + (string.IsNullOrEmpty(shortName) ? "" : ":") + shortName.Replace("'", "").Replace("/", "");
            orderID = CommonFunctions.CInt(OrderHeaderRequest[0, CommonData.fieldID]);
            int seq = 0;
            if (orderID < 1 && 
            _dataSource.existsRecord(CommonData.tableORDERS, new string[] { CommonData.fieldSHORTNAME }, new string[] { shortName }))
            {
                while (_dataSource.existsRecord(CommonData.tableORDERS, new string[] { CommonData.fieldSHORTNAME }, new string[] { shortName + (seq == 0 ? "" : seq.ToString()) }))
                    seq++;
            }
            OrderHeaderToUpdate[0, CommonData.fieldSHORTNAME] = shortName + (seq == 0 ? "" : seq.ToString());

            //We update the OrderHeaderRequest too! with the "new" ShortName...
            OrderHeaderRequest[0, CommonData.fieldSHORTNAME] = shortName + (seq == 0 ? "" : seq.ToString());
            break;
          //BEGIN Following CC prefix cases are mappings! need to review this later!!
          case "ccname":
          case "ccnumber":
            requestFieldName = "CreditCard" + fieldName.Substring(2);
            if (OrderHeaderRequest.ContainsColumn(requestFieldName))
              OrderHeaderToUpdate[0, fieldName] = OrderHeaderRequest[0, requestFieldName];
            break;
          case "ccexpirationdate":
          case "ccsecuritycode":
          case "ccamounttopay":
            requestFieldName = fieldName.Substring(2);
            if (OrderHeaderRequest.ContainsColumn(requestFieldName))
              OrderHeaderToUpdate[0, fieldName] = OrderHeaderRequest[0, requestFieldName];
            break;
          //END Following CC prefix cases are mappings! need to review this later!!
          default:
            if (OrderHeaderRequest.ContainsColumn(fieldName))
              OrderHeaderToUpdate[0, fieldName] = OrderHeaderRequest[0, fieldName];
            break;
        }

      }

      /*
       * TODO: We must check if the Order exists and then we call the appropiate 
       * method for saving the Order Header data, that is, Create or Update the record
       */
      orderID = CommonFunctions.CInt(OrderHeaderToUpdate[0, CommonData.fieldID]);
      string customerID = CommonFunctions.CString(OrderHeaderToUpdate[0, CommonData.fieldCUSTOMER]);
      string[] keys = new string[] { CommonData.fieldID };
      string[] values = new string[] { orderID.ToString().Trim() };

      if (orderID > 0 && _dataSource.existsRecord(CommonData.tableORDERS, keys, values))
      {
        if (string.IsNullOrEmpty(customerID)) 
        {
          response.Errors.Add("Passed a NULL or EMPTY Customer ID for an existing Order Header Update operation!");
          return response;
        }
        else
        {
          keys = new string[] { CommonData.fieldENTITY };
          values = new string[] { customerID.Trim() };
          if (!(_dataSource.existsRecord(CommonData.tableENTITY, keys, values)))
          {
            response.Errors.Add(string.Format("CustomerID:'{0}' does NOT exists!!", customerID));
            return response;
          }
        }
        _dataSource.updateOrderHeader(OrderHeaderToUpdate);
      }
      else
      {
        keys = new string[] { CommonData.fieldENTITY };
        values = new string[] { customerID.Trim() };

        if (!(_dataSource.existsRecord(CommonData.tableENTITY, keys, values)))
        {
          if (_dataSource.createCustomer(OrderHeaderRequest, response, dealer))
            OrderHeaderToUpdate[0, CommonData.fieldCUSTOMER] = OrderHeaderRequest[0, CommonData.fieldCUSTOMER];
          else
            return response;
        }

        OrderHeaderRequest[0, CommonData.fieldID] = _dataSource.createOrderHeader(OrderHeaderToUpdate, response);
        // TODO stip off the dealer prefix of the order shortname
      }
      string newOrderShortName = CommonFunctions.CString(OrderHeaderRequest[0, CommonData.fieldSHORTNAME]);
      int colonPos = newOrderShortName.IndexOf(":");
      newOrderShortName = newOrderShortName.Substring(colonPos + 1);
      OrderHeaderRequest[0, CommonData.fieldSHORTNAME] = newOrderShortName;
      //if (response.Results.Count > 0)
      //  response.Results[0] = OrderHeaderRequest;
      //else 
      //  response.Results.Add(OrderHeaderRequest);
      return response;
    }
  }
}
