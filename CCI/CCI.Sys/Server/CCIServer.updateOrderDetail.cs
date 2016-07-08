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
    public ServerResponse updateOrderDetails(ServerRequest request, ServerResponse response)
    {
      /*
       * Begin Larry's Comments
       * 
       * Data will come inb the following form:
       * 1) it will come as a POST with the fields in the request.Forms collection
       * 2) the fieldsnames will be in the form: sectionname_99_fieldname where 99 is the line number in the section
       * 3) the record will be uniquely identified by fieldname wholesaleusoc (also by section and line number).  They will also deliver the ID we gave them for the record, if that helps
       * 
       * so to use this, we must create thte CCITAble from this data, then send to the update data layer
       * 
       * TODO: lmv66 Added to Larry's comments
       * 2a) We will have header_fieldname kind of fields in the request, this will be he OrderHeader stuff, so the UpdateOrderDetail will do both create/update if necessary on both tables
       * OrderHeader and OrderDetails...
       * End Larry's Comments
       * 
       * TODO: We will build the Order CCITable base on the call the the [vw_DBColumnDetail] call
       * This view gives us all the fields (columns) belonging to this Table, it does not identify the 
       * primary key of the table, so that will be "hardcoded" for now in this code
       * 
       * I will need to create an arraylist that will populate with the 
       */
      //---------------------------------------------------------------------------------------------------------------------------------------------
      int orderID = 0;
      ArrayList orderHeaderColumns = new ArrayList();
      Hashtable orderHeaderRecords = new Hashtable();
      Hashtable orderHeaderFields = null;

      ArrayList orderDetailColumns = new ArrayList();
      Hashtable orderDetailRecords = new Hashtable();
      Hashtable orderDetailFields = null;

      ArrayList orderColumns = null;
      Hashtable orderRecords = null;
      Hashtable orderFields = null;

      string rawKey = string.Empty;
      string[] rawKeyParts = null;
      string sectionName = string.Empty;
      string sectionNamePrefix = string.Empty;
      string sectionNameLineNumber = string.Empty;
      string fieldName = string.Empty;
      string fieldValue = string.Empty;
      foreach (KeyValuePair<string, string> formField in request.Form)
      {
        rawKey = formField.Key;
        if (string.IsNullOrEmpty(rawKey))
          continue;

        rawKeyParts = rawKey.Split(new string[] { "[", "]", "." }, StringSplitOptions.RemoveEmptyEntries);

        switch (rawKeyParts.GetLength(0))
        {
          case 2: //We are in a header record!
            sectionName = rawKeyParts[0].Trim().ToLower(); //This must be ALWAYS "header"
            fieldName = rawKeyParts[1].ToLower();
            fieldValue = formField.Value.Replace("'", "''");

            if (!(orderHeaderRecords.ContainsKey(sectionName)))
              orderHeaderRecords.Add(sectionName, new Hashtable());

            orderRecords = orderHeaderRecords;
            orderColumns = orderHeaderColumns;
            break;
          case 3: //We are in a detail record!
            sectionName = string.Format("{0}_{1}", rawKeyParts[0], rawKeyParts[1]);
            sectionNamePrefix = rawKeyParts[0].Trim();
            sectionNameLineNumber = rawKeyParts[1].Trim();
            fieldName = rawKeyParts[2].ToLower();
            fieldValue = formField.Value.Replace("'", "''");

            if (!(orderDetailRecords.ContainsKey(sectionName)))
            {
              orderDetailRecords.Add(sectionName, new Hashtable());
              orderDetailFields = (Hashtable)orderDetailRecords[sectionName];
              orderDetailFields.Add(CommonData.fieldSECTIONAME.ToLower(), sectionNamePrefix);
              orderDetailFields.Add(CommonData.fieldLINENUMBER.ToLower(), sectionNameLineNumber);
            }

            orderRecords = orderDetailRecords;
            orderColumns = orderDetailColumns;
            break;
        }

        orderFields = (Hashtable)orderRecords[sectionName];
        if (orderFields.ContainsKey(fieldName))
          orderFields[fieldName] = fieldValue;
        else
          orderFields.Add(fieldName, fieldValue);

        //Add the column to the column list if it does not exists yet!
        if (!(orderColumns.Contains(fieldName)))
          orderColumns.Add(fieldName);
      }

      bool newTableFormat = true;
      if (response.Options.ContainsKey(CommonData.NEWTABLEFORMAT))
        newTableFormat = CommonFunctions.CBoolean(response.Options[CommonData.NEWTABLEFORMAT]);

      CCITable orderHeaderRequest = new CCITable();
      CCITable orderDetailsRequest = new CCITable();

      if (!(orderHeaderColumns.Contains(CommonData.fieldORDERID.ToLower())))
        orderHeaderColumns.Add(CommonData.fieldORDERID.ToLower());
      if (!(orderHeaderColumns.Contains(CommonData.fieldID.ToLower())))
        orderHeaderColumns.Add(CommonData.fieldID.ToLower());
      if (!(orderHeaderColumns.Contains(CommonData.fieldQUOTEID.ToLower())))
        orderHeaderColumns.Add(CommonData.fieldQUOTEID.ToLower());
      if (!(orderHeaderColumns.Contains(CommonData.fieldCUSTOMER.ToLower())))
        orderHeaderColumns.Add(CommonData.fieldCUSTOMER.ToLower());
      if (!(orderHeaderColumns.Contains(CommonData.fieldSHORTNAME.ToLower())))
        orderHeaderColumns.Add(CommonData.fieldSHORTNAME.ToLower());

      if (!(orderDetailColumns.Contains(CommonData.fieldORDERID.ToLower())))
        orderDetailColumns.Add(CommonData.fieldORDERID.ToLower());
      if (!(orderDetailColumns.Contains(CommonData.fieldDETAILID.ToLower())))
        orderDetailColumns.Add(CommonData.fieldDETAILID.ToLower());
      if (!(orderDetailColumns.Contains(CommonData.fieldITEMID.ToLower())))
        orderDetailColumns.Add(CommonData.fieldITEMID.ToLower());
      if (!(orderDetailColumns.Contains(CommonData.fieldSECTIONAME.ToLower())))
        orderDetailColumns.Add(CommonData.fieldSECTIONAME.ToLower());
      if (!(orderDetailColumns.Contains(CommonData.fieldLINENUMBER.ToLower())))
        orderDetailColumns.Add(CommonData.fieldLINENUMBER.ToLower());
      if (!(orderDetailColumns.Contains(CommonData.fieldDIRTY.ToLower())))
        orderDetailColumns.Add(CommonData.fieldDIRTY.ToLower());
      if (!(orderDetailColumns.Contains(CommonData.fieldQUANTITY.ToLower())))
        orderDetailColumns.Add(CommonData.fieldQUANTITY.ToLower());
      if (!(orderDetailColumns.Contains(CommonData.ACTION.ToLower())))
        orderDetailColumns.Add(CommonData.ACTION.ToLower());

      string[] orderHeaderColumnList = new string[orderHeaderColumns.Count];
      string[] orderDetailColumnList = new string[orderDetailColumns.Count];

      orderHeaderColumns.CopyTo(orderHeaderColumnList);
      orderDetailColumns.CopyTo(orderDetailColumnList);

      orderHeaderRequest.SetColumns(orderHeaderColumnList);
      orderDetailsRequest.SetColumns(orderDetailColumnList);

      foreach (string orderHeaderSection in orderHeaderRecords.Keys)
      {
        orderHeaderFields = (Hashtable)orderHeaderRecords[orderHeaderSection];
        ArrayList orderHeaderFieldValues = new ArrayList();
        foreach (string headerFieldName in orderHeaderColumns)
        {
          if (orderHeaderFields.ContainsKey(headerFieldName))
            orderHeaderFieldValues.Add(orderHeaderFields[headerFieldName]);
        }
        orderHeaderRequest.AddRow(orderHeaderFieldValues.ToArray());
      }

      if (orderHeaderRequest.NumberRows == 1)
      {
        //Let's see if we have a Header.OrderID if this is the case, and has a value, we put that value in Header.ID, unless Header.ID is already greater than 0!
        if (CommonFunctions.CInt(orderHeaderRequest[0, CommonData.fieldID]) < 1)
          orderHeaderRequest[0, CommonData.fieldID] = orderHeaderRequest[0, CommonData.fieldORDERID];
      }
      else
      {
        response.Errors.Add("Update must have exactly one header record reference!!");
        return response;
      }

      bool addDetailRow = false;
      foreach (string orderDetailSection in orderDetailRecords.Keys)
      {
        addDetailRow = true;
        orderDetailFields = (Hashtable)orderDetailRecords[orderDetailSection];
        ArrayList orderDetailValues = new ArrayList();
        string itemID = CommonFunctions.CString(orderDetailFields["itemid"]);
        foreach (string detailFieldName in orderDetailColumns)
        {
          if (detailFieldName.Equals(CommonData.fieldDIRTY, StringComparison.CurrentCultureIgnoreCase))
            orderDetailValues.Add("Yes");
          else if (detailFieldName.Equals(CommonData.fieldQUANTITY, StringComparison.CurrentCultureIgnoreCase)
            && CommonFunctions.CString(orderDetailFields[detailFieldName.ToLower()]) == string.Empty
            && !string.IsNullOrEmpty(itemID))
          {
            addDetailRow = false;
            break;
          }
          else if (orderDetailFields.ContainsKey(detailFieldName.ToLower()))
            orderDetailValues.Add(orderDetailFields[detailFieldName.ToLower()]);
          else
            orderDetailValues.Add(string.Empty);
        }
        if (addDetailRow)
          orderDetailsRequest.AddRow(orderDetailValues.ToArray());
      }

      for (int i = 0; i < orderDetailsRequest.NumberRows; i++)
      {
        if (CommonFunctions.CInt(orderDetailsRequest[i, CommonData.fieldQUANTITY]) == 0
          && CommonFunctions.CString(orderDetailsRequest[i, "itemid"]) != string.Empty)
          orderDetailsRequest[i, CommonData.ACTION] = CommonData.SQLDELETEROW;
        else
          orderDetailsRequest[i, CommonData.ACTION] = CommonData.SQLUPDATEROW;
      }

      /*
       * We have the two tables and an OrderID, this means we can save the stuff, even to create or update any of the two records
       * in Order Header and Order Detail tables, so first we create/update the Order Header and then we create/update the Order Details
       */
      //The updateOrderHeader adds the updated orderHeaderRequest to the response, so in the end we will need only to add the OrderDetailRequest
      orderID = CommonFunctions.CInt(orderHeaderRequest[0, CommonData.fieldID]);
      if (orderID == 0)
        orderID = CommonFunctions.CInt(orderHeaderRequest[0, CommonData.fieldQUOTEID]);
      if (orderID == 0)
        orderID = CommonFunctions.CInt(orderHeaderRequest[0, CommonData.fieldORDERID]);

      orderHeaderRequest[0, CommonData.fieldID] = orderID;
      response = updateOrderHeader(orderHeaderRequest, response);

      //Let's read the Order ID!
      orderID = CommonFunctions.CInt(orderHeaderRequest[0, CommonData.fieldID]);
      orderHeaderRequest[0, CommonData.fieldQUOTEID] = orderID;
      orderHeaderRequest[0, CommonData.fieldORDERID] = orderID;

      if (orderID < 1 || !(_dataSource.existsRecord(CommonData.tableORDERS, new string[] { CommonData.fieldID }, new string[] { orderID.ToString().Trim() })))
      {
        response.Errors.Add(string.Format("Invalid Order ID: [{0}], Cannot add detail data!", orderID.ToString()));
        return response;
      }
      //---------------------------------------------------------------------------------------------------------------------------------------------

      if (orderDetailsRequest.NumberRows >= 1)
      {
        //In case that we HAVE a DETAIL we now save it!!
        orderDetailsRequest.SetKeyColumns(new string[] { CommonData.fieldORDERID, CommonData.fieldDETAILID });

        CCITable OrderDetailFields = _dataSource.getTableFieldListData(CommonData.tableORDERDETAIL);
        Hashtable OrderFieldsList = new Hashtable();
        for (int i = 0; i < OrderDetailFields.NumberRows; i++)
          OrderFieldsList.Add(
            CommonFunctions.CString(OrderDetailFields[i, CommonData.fieldCOLUMNNAME]).ToLower().Trim(),
            CommonFunctions.CString(OrderDetailFields[i, CommonData.fieldSYSTEMDATATYPE]).ToLower().Trim());

        //This last column is for relating the request row with the update CCITable
        OrderFieldsList.Add(CommonData.REQUESTROW, CommonData.SQLINT);
        OrderFieldsList.Add(CommonData.ACTION, CommonData.SQLNVARCHAR);

        string[] columnNames = new string[OrderFieldsList.Count];
        string[] columnTypes = new string[OrderFieldsList.Count];
        OrderFieldsList.Keys.CopyTo(columnNames, 0);
        OrderFieldsList.Values.CopyTo(columnTypes, 0);

        CCITable OrderDetailsToUpdate = new CCITable();
        OrderDetailsToUpdate.SetColumns(columnNames, columnTypes);
        OrderDetailsToUpdate.SetKeyColumns(new string[] { CommonData.fieldORDERID, CommonData.fieldDETAILID });

        object[] rowDefaultValues = new object[columnNames.GetLength(0)];
        for (int i = 0; i < OrderDetailsToUpdate.NumberColumns; i++)
          rowDefaultValues[i] = CommonData.NOCHANGE;

        fieldName = string.Empty;

        CCITable getPriceAndUseMRC = null;
        bool useMRC = true;
        decimal defaultRetailMRCvalue = 0;
        decimal retailMRCvalue = 0;
        //decimal retailNRCvalue = 0;
        decimal dealerCostValue = 0;
        string retailitemid = string.Empty;
        string carrierDescriptionValue = string.Empty;
        string vendorDescriptionValue = string.Empty;
        string phoneMakeModel = string.Empty;
        string usocRetail = string.Empty;

        int iRow = 0;
        String xBefore = string.Empty;    // testing
        String xAfter = string.Empty;    // testing
        String xRequest = string.Empty;    // testing
        for (int i = 0; i < orderDetailsRequest.NumberRows; i++)
        {
          //We update the OrderDetailsRequest OrderID column! here we do it...
          orderDetailsRequest[i, CommonData.fieldORDERID] = orderID;

          if (CommonFunctions.CBoolean(orderDetailsRequest[i, CommonData.fieldDIRTY]))
          {
            OrderDetailsToUpdate.AddRow(rowDefaultValues);
            OrderDetailsToUpdate[iRow, CommonData.fieldORDERID] = orderID;
            for (int j = 0; j < OrderDetailsToUpdate.NumberColumns; j++)
            {
              fieldName = OrderDetailsToUpdate.Column(j);

              if (!fieldName.Equals(CommonData.fieldORDERID, StringComparison.CurrentCultureIgnoreCase))
              {
                switch (fieldName.ToLower())
                {
                  case "createdatetime":
                  case "lastmodifieddatetime":
                    OrderDetailsToUpdate[iRow, fieldName] = DateTime.Now.ToString(CommonData.FORMATLONGDATETIME);
                    break;
                  case "createdby":
                  case "lastmodifiedby":
                    OrderDetailsToUpdate[iRow, fieldName] = response.SecurityContext.User;
                    break;
                  //Mappings BEGIN! Following cases are mappings!
                  //case "retailitemid":
                  case "retailitemid":
                    retailitemid = CommonFunctions.CString(orderDetailsRequest[i, "retailitemid"]);
                    retailMRCvalue = 0;
                    getPriceAndUseMRC = _dataSource.getPriceAndUseMRC(retailitemid.ToLower());
                    if (getPriceAndUseMRC != null && getPriceAndUseMRC.NumberRows == 1)
                    {
                      useMRC = CommonFunctions.CBoolean(getPriceAndUseMRC[0, "useMRC"]);
                      defaultRetailMRCvalue = CommonFunctions.CDecimal(getPriceAndUseMRC[0, "Price"]);
                      defaultRetailMRCvalue = (defaultRetailMRCvalue < 0 ? 0 : defaultRetailMRCvalue);
                    }
                    else
                    {
                      useMRC = true;
                      defaultRetailMRCvalue = 0;
                    }

                    if (retailMRCvalue == 0 && orderDetailsRequest.ContainsColumn("retailmrc"))
                      retailMRCvalue = CommonFunctions.CDecimal(orderDetailsRequest[i, "retailmrc"]);
                    if (retailMRCvalue == 0 && orderDetailsRequest.ContainsColumn("salesprice"))
                      retailMRCvalue = CommonFunctions.CDecimal(orderDetailsRequest[i, "salesprice"]);
                    if (retailMRCvalue == 0 && orderDetailsRequest.ContainsColumn("monthly"))
                      retailMRCvalue = CommonFunctions.CDecimal(orderDetailsRequest[i, "monthly"]);
                    if (retailMRCvalue == 0 && orderDetailsRequest.ContainsColumn("variable"))
                      retailMRCvalue = CommonFunctions.CDecimal(orderDetailsRequest[i, "variable"]);
                    if (retailMRCvalue == 0)
                      retailMRCvalue = defaultRetailMRCvalue;

                    if (useMRC)
                    {
                      OrderDetailsToUpdate[iRow, "retailmrc"] = retailMRCvalue;
                    }
                    else
                    {
                      OrderDetailsToUpdate[iRow, "retailnrc"] = retailMRCvalue;
                    }

                    // THIS IS THE LINE THAT ERRONEOUSLY SETS RetailItemID, because "fieldName" is "RetailItemID": OrderDetailsToUpdate[iRow, fieldName] = retailMRCvalue;
                    xRequest = CommonFunctions.CString(orderDetailsRequest[i, "retailitemid"]);
                    //xBefore = CommonFunctions.CString(OrderDetailsToUpdate[iRow, fieldName]); // testing
                    OrderDetailsToUpdate[iRow, "retailitemid"] = retailitemid;  // seems to fix issue
                    //xAfter = CommonFunctions.CString(OrderDetailsToUpdate[iRow, fieldName]); // testing
                    break;
                  //case "retailnrc":
                  //  retailNRCvalue = 0;
                  //  if (orderDetailsRequest.ContainsColumn("retailnrc"))
                  //    retailNRCvalue = CommonFunctions.CDecimal(orderDetailsRequest[i, "retailnrc"]);
                  //  if (retailNRCvalue == 0 && orderDetailsRequest.ContainsColumn("salesprice"))
                  //  {
                  //    if (orderDetailsRequest.ContainsColumn("useMRC") && !CommonFunctions.CBoolean(orderDetailsRequest[i, "useMRC"]))
                  //    {
                  //      retailMRCvalue = CommonFunctions.CDecimal(orderDetailsRequest[i, "salesprice"]);
                  //    }
                  //  }
                  //  if (retailNRCvalue == 0 && orderDetailsRequest.ContainsColumn("install"))
                  //    retailNRCvalue = CommonFunctions.CDecimal(orderDetailsRequest[i, "install"]);

                  //  OrderDetailsToUpdate[iRow, fieldName] = retailNRCvalue;
                  //  break;
                  case "dealercost":
                    dealerCostValue = 0;
                    if (orderDetailsRequest.ContainsColumn("dealercost"))
                      dealerCostValue = CommonFunctions.CDecimal(orderDetailsRequest[i, "dealercost"]);
                    if (dealerCostValue == 0 && orderDetailsRequest.ContainsColumn("yourcost"))
                      dealerCostValue = CommonFunctions.CDecimal(orderDetailsRequest[i, "yourcost"]);
                    OrderDetailsToUpdate[iRow, fieldName] = dealerCostValue;
                    break;
                  case "carrierdescription":
                    carrierDescriptionValue = string.Empty;
                    if (orderDetailsRequest.ContainsColumn("carrierdescription"))
                      carrierDescriptionValue = CommonFunctions.CString(orderDetailsRequest[i, "carrierdescription"]);
                    if (string.IsNullOrEmpty(carrierDescriptionValue) && orderDetailsRequest.ContainsColumn("currentcarrier"))
                      carrierDescriptionValue = CommonFunctions.CString(orderDetailsRequest[i, "currentcarrier"]);
                    if (string.IsNullOrEmpty(carrierDescriptionValue) && orderDetailsRequest.ContainsColumn("carriername"))
                      carrierDescriptionValue = CommonFunctions.CString(orderDetailsRequest[i, "carriername"]);

                    OrderDetailsToUpdate[iRow, fieldName] = carrierDescriptionValue;
                    break;
                  case "vendordescription":
                    vendorDescriptionValue = string.Empty;
                    if (orderDetailsRequest.ContainsColumn("vendordescription"))
                      vendorDescriptionValue = CommonFunctions.CString(orderDetailsRequest[i, "vendordescription"]);
                    if (orderDetailsRequest.ContainsColumn("vendorname"))
                      vendorDescriptionValue = CommonFunctions.CString(orderDetailsRequest[i, "vendorname"]);
                    OrderDetailsToUpdate[iRow, fieldName] = vendorDescriptionValue;

                    break;
                  case "phonemakemodel":
                    phoneMakeModel = string.Empty;
                    if (orderDetailsRequest.ContainsColumn("phonemakemodel"))
                      phoneMakeModel = CommonFunctions.CString(orderDetailsRequest[i, "phonemakemodel"]);
                    if (orderDetailsRequest.ContainsColumn("makemodel"))
                      phoneMakeModel = CommonFunctions.CString(orderDetailsRequest[i, "makemodel"]);
                    OrderDetailsToUpdate[iRow, fieldName] = phoneMakeModel;

                    break;
                  //Mappings END! Following cases are mappings!

                  //Special cases! we need to get rid of part of the characters!
                  //for example for the RetailUSOC may come with a "-<sequence>" attached 
                  //we must get rid of it before updating...
                  case "usocretail":
                    usocRetail = string.Empty;
                    if (orderDetailsRequest.ContainsColumn("usocretail"))
                    {
                      usocRetail = CommonFunctions.CString(orderDetailsRequest[i, "usocretail"]);
                      if (usocRetail.Contains("-"))
                        usocRetail = usocRetail.Substring(1, usocRetail.IndexOf("-"));
                    }
                    OrderDetailsToUpdate[iRow, fieldName] = phoneMakeModel;

                    break;
                  default:
                    if (orderDetailsRequest.ContainsColumn(fieldName))
                      OrderDetailsToUpdate[iRow, fieldName] = orderDetailsRequest[i, fieldName];
                    break;
                }
              }
            }
            OrderDetailsToUpdate[iRow, CommonData.REQUESTROW] = i;
            //OrderDetailsToUpdate[iRow, CommonData.ACTION] = orderDetailsRequest[i, CommonData.ACTION];
            iRow++;
          }
        }

        /*
         * TODO! HERE I AM! Need to see the LOGIC for UPDATING DETAILS!
         * TODO: We must check if the Order exists and then we call the appropiate 
         * method for saving the Order Header data, that is, Create or Update the record
         */
        OrderDetailsToUpdate = _dataSource.updateOrderDetail(orderID, OrderDetailsToUpdate);

        int rowDetail = 0;
        for (int i = 0; i < OrderDetailsToUpdate.NumberRows; i++)
        {
          rowDetail = CommonFunctions.CInt(OrderDetailsToUpdate[i, CommonData.REQUESTROW]);
          orderDetailsRequest[rowDetail, CommonData.fieldDETAILID] = CommonFunctions.CInt(OrderDetailsToUpdate[i, CommonData.fieldDETAILID]);
        }


        response.Results.Add(orderDetailsRequest);
      }
      response.Results.Add(orderHeaderRequest);

      return response;
    }
  }
}
