using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using CCI.Common;
using CCI.Sys.SecurityEngine;
using TAGBOSS.Common.Model;


namespace CCI.Sys.Data
{
  public partial class DataSource
  {
    //const string REQUESTROW = "RequestRow";
    private string[] detailBlankRowFields = new string[] {"itemid", "carrierdescription", "itemdescription", "vendordescription", "PhoneMakeModel", 
        "vendoremail", "vendorphone", "description", "connectiontype", "contractexpirationdate", "carrieremail", "carrierphone", "contactname" };

    public CCITable getOrderHeader(string orderID, string orderName)
    {
      string sql =
        @"Select o.ID, substring(o.ShortName, charindex(':',o.ShortName) + 1, len(o.ShortName)) ShortName, o.OrderType, o.Status, o.Customer, 
              c.LegalName, c.Address1, c.Address2, c.City, c.State, c.Zip, 
              o.ContractTerm, o.InstallationCosts, 
              o.ccName As CreditCardName, 
              o.ccNumber As CreditCardNumber, 
              o.ccExpirationDate As ExpirationDate,
              o.ccSecurityCode As SecurityCode,
              o.ccAmountToPay As AmountToPay,
              o.CarrierServices, o.PhonesFrom
        From Orders o Inner Join Entity c On o.Customer = c.Entity 
        Where OrderType = 'Quote' And {0} = '{1}'";
      if (!string.IsNullOrEmpty(orderID))
        sql = string.Format(sql, "ID", orderID);
      else
        if (!string.IsNullOrEmpty(orderName))
          sql = string.Format(sql, "ShortName", CommonFunctions.fixUpStringForSQL(orderName));
        else
          return new CCITable();
      return CommonFunctions.convertDataSetToCCITable(getDataFromSQL(sql));

    }

    public CCITable getOrderDetail(string orderID, string screenSection)
    {
      if (string.IsNullOrEmpty(orderID))
        return new CCITable();
        string sectionWhere = 
          string.IsNullOrEmpty(screenSection) ? string.Empty 
          : string.Format(" AND (d.ScreenSection = '{0}' OR d.ScreenSection = '{0}List')", CommonFunctions.fixUpStringForSQL(screenSection));
        string sql =
          @"Select o.*, coalesce(d.ScreenSection, o.Section) ScreenSection, coalesce(d.Sequence, o.LineNumber) Sequence, p.ExternalName, 0 Dirty,
              case when isnull(d.useMRC,1) = 1 then o.RetailMRC else o.RetailNRC end As SalesPrice, o.RetailMRC As Monthly,  o.RetailMRC As Variable, 
              o.RetailNRC As Install, 
              o.DealerCost As YourCost, 
              o.CarrierDescription As CurrentCarrier, o.CarrierDescription As CarrierName,
              o.VendorDescription As VendorName,
              o.PhoneMakeModel As MakeModel,
              coalesce(d.Screen, 'DealerQuote') Screen, case when isnull(case when isnull(d.useMRC,1) = 1 then retail.MRC else retail.NRC end, 0) = -1 then 1 else 0 end IsVariable,
              isnull(d.useMRC,1) useMRC
            From OrderDetail o 
              left Join ScreenDefinition d On d.ItemID = o.ItemID And d.Screen = 'DealerQuote'
              left Join MasterProductList p On o.ItemID = p.ItemID
              left join ProductList retail on retail.ItemID = o.RetailItemID and retail.Carrier = 'CityHosted'
            Where o.OrderID = {0}{1}";
      sql = string.Format(sql, orderID,sectionWhere);
      return CommonFunctions.convertDataSetToCCITable(getDataFromSQL(sql));
    }

    /// <summary>
    /// create a new customer from a dealer order
    /// </summary>
    /// <param name="orderHeader"></param>
    /// <param name="response"></param>
    /// <returns></returns>
    public bool createCustomer(CCITable orderHeader, ServerResponse response, string dealer)
    {

      string sql = string.Empty;
      string fieldName = string.Empty;
      string customerName, address1, address2, city, state, zip;
      customerName = address1 = address2 = city = state = zip = string.Empty;

      Hashtable entityFields = new Hashtable();
      Hashtable attributeFields = new Hashtable();
      Hashtable externalidmappingFields = new Hashtable();

      string newCustomerID = getNextNumericEntityID(CommonData.ENTITYTYPECUSTOMER);

      if (CommonFunctions.CInt(newCustomerID) > 0)
        orderHeader[0, CommonData.fieldCUSTOMER] = newCustomerID;
      else
      {
        response.Errors.Add("Could not create new Customer ID!");
        return false;
      }

      string lastModifiedDataTime = DateTime.Now.ToString(CommonData.FORMATLONGDATETIME);

      entityFields.Add(CommonData.fieldENTITY, newCustomerID);
      entityFields.Add(CommonData.fieldENTITYTYPE, CommonData.ENTITYTYPECUSTOMER);
      entityFields.Add(CommonData.fieldENTITYOWNER, CommonData.ENTITYOWNERCCI);
      entityFields.Add(CommonData.LASTMODIFIEDBY, response.SecurityContext.User);
      entityFields.Add(CommonData.LASTMODIFIEDDATETIME, lastModifiedDataTime);

      attributeFields.Add(CommonData.fieldENTITY, newCustomerID);
      attributeFields.Add(CommonData.fieldITEMTYPE, CommonData.ITEMTYPEENTITY);
      attributeFields.Add(CommonData.fieldITEM, CommonData.ENTITYTYPECUSTOMER);
      attributeFields.Add(CommonData.fieldITEMHISTORY, "<ItemDates/>");
      attributeFields.Add(CommonData.fieldATTRIBUTES, "null");
      attributeFields.Add(CommonData.LASTMODIFIEDBY, response.SecurityContext.User);
      attributeFields.Add(CommonData.LASTMODIFIEDDATETIME, lastModifiedDataTime);

      externalidmappingFields.Add(CommonData.fieldEXTERNALSOURCETYPE, CommonData.EXTERNALSOURCETYPEPAYOR);
      externalidmappingFields.Add(CommonData.fieldEXTERNALSOURCE, CommonData.EXTERNALSOURCEPAYOR);
      externalidmappingFields.Add(CommonData.fieldENTITYTYPE, CommonData.ENTITYTYPECUSTOMER);
      externalidmappingFields.Add(CommonData.fieldINTERNALID, newCustomerID);
      externalidmappingFields.Add(CommonData.LASTMODIFIEDBY, response.SecurityContext.User);
      externalidmappingFields.Add(CommonData.LASTMODIFIEDDATETIME, lastModifiedDataTime);
      for (int i = 0; i < orderHeader.NumberColumns; i++)
      {
        fieldName = orderHeader.Column(i);
        switch (fieldName.ToLower()) 
        {
          case "legalname":
            string legalName = CommonFunctions.CString(orderHeader[0, "legalname"]);
            entityFields.Add(CommonData.fieldLEGALNAME, legalName);
            externalidmappingFields.Add(CommonData.fieldEXTERNALID, legalName);
            break;
          case "address1":
            entityFields.Add(CommonData.fieldADDRESS1, CommonFunctions.CString(orderHeader[0, CommonData.fieldADDRESS1]));
            break;
          case "address2":
            entityFields.Add(CommonData.fieldADDRESS2, CommonFunctions.CString(orderHeader[0, CommonData.fieldADDRESS2]));
            break;
          //case "suite":
          //  entityFields.Add(CommonData.fieldSUITE, CommonFunctions.CString(orderHeader[0, CommonData.fieldSUITE]));
          //  break;
          case "city":
            entityFields.Add(CommonData.fieldCITY, CommonFunctions.CString(orderHeader[0, CommonData.fieldCITY]));
            break;
          case "state":
            entityFields.Add(CommonData.fieldSTATE, CommonFunctions.CString(orderHeader[0, CommonData.fieldSTATE]));
            break;
          case "zip":
            entityFields.Add(CommonData.fieldZIP, CommonFunctions.CString(orderHeader[0, CommonData.fieldZIP]));
            break;
        }
      }

      try
      {
        string[] fieldList = new string[entityFields.Count];
        string[] valueList = new string[entityFields.Count];
        int k = 0;
        foreach (string field in entityFields.Keys)
        {
          fieldList[k] = field;
          valueList[k] = (string)entityFields[field];
          k++;
        }
        sql = makeInsertSQL(fieldList, valueList, CommonData.tableENTITY);
        updateDataFromSQL(sql);

        fieldList = new string[attributeFields.Count];
        valueList = new string[attributeFields.Count];
        k = 0;
        foreach (string field in attributeFields.Keys)
        {
          fieldList[k] = field;
          valueList[k] = (string)attributeFields[field];
          k++;
        }
        sql = makeInsertSQL(fieldList, valueList, CommonData.tableATTRIBUTE);
        updateDataFromSQL(sql);

        fieldList = new string[externalidmappingFields.Count];
        valueList = new string[externalidmappingFields.Count];
        k = 0;
        foreach (string field in externalidmappingFields.Keys)
        {
          fieldList[k] = field;
          valueList[k] = (string)externalidmappingFields[field];
          k++;
        }
        sql = makeInsertSQL(fieldList, valueList, CommonData.tableEXTERNALIDMAPPING);
        updateDataFromSQL(sql);
        sql = string.Format("insert into SalesOrDealerCustomers (SalesOrDealer, Customer, LastModifiedBy, LastModifiedDateTime, SalesType) Values ('{0}', '{1}', '{2}', '{3}', 'Dealer')",
          dealer, newCustomerID, response.SecurityContext.User, DateTime.Now.ToString(CommonData.FORMATLONGDATETIME));
        updateDataFromSQL(sql);
      }
      catch (Exception errSQL)
      {
        response.Errors.Add(string.Format("Error: {0}:: StackTrace:{1}", errSQL.Message, errSQL.StackTrace));
        return false;
      }

      return true;
    }

    public long createOrderHeader(CCITable orderHeader, ServerResponse response)
    {
      long newOrderID = -1;

      string sql = string.Empty;
      string fieldName = string.Empty;
      string fieldValue = string.Empty;
      Hashtable fields = new Hashtable();

      /*
       * TODO: Here is WHERE we NEED the FieldType! to add the correct format to the SQL string being generated!
       */

      //foreach (string keyField in order.KeyColumns.Keys)
      //  keyFields.Add(keyField, getFormattedFieldValue(keyField, 0, order));

      for (int i = 0; i < orderHeader.NumberColumns; i++)
      {
        /*
         * This will work ONLY in the case where the key is an autonumber!
         * so we do not need to pass the ID, since it is generated, for "combined" keys 
         * we will need another logic, since combined keys are NOT autogenerated...
         */
        fieldName = orderHeader.Column(i);
        fieldValue = string.Empty;
        //if (!(orderHeader.KeyColumns.ContainsKey(fieldName.ToLower().Trim()) 
        //  || orderHeader[0, fieldName].ToString().Trim().Equals(CommonData.NOCHANGE,StringComparison.CurrentCultureIgnoreCase)))
        if (!(orderHeader[0, fieldName] == null || orderHeader[0, fieldName].ToString().Trim().Equals(CommonData.NOCHANGE, StringComparison.CurrentCultureIgnoreCase)))
          fieldValue = getFormattedFieldValue(fieldName, 0, orderHeader);

        fields.Add(fieldName.ToLower(), fieldValue);
      }

      //string[] keyList = new string[keyFields.Count];
      if (!(fields.ContainsKey(CommonData.fieldID.ToLower())))
        fields.Add(CommonData.fieldID.ToLower(), string.Empty);

      string[] fieldList = new string[fields.Count];
      string[] valueList = new string[fields.Count];

      //keyFields.CopyTo(keyList, 0);
      fields.Keys.CopyTo(fieldList, 0);
      fields.Values.CopyTo(valueList, 0);

      /*
       * TODO: We now have all the data in string arrays to build the corresponding Insert or Update SQL
       * Pending to write and test it!!
       */

      newOrderID = createRecordReturnID(fieldList, valueList, "CreateOrderHeaderReturnID", CommonData.fieldID.ToLower());
      return newOrderID;
      //sql = makeInsertSQL(fieldList, valueList, CommonData.tableORDERS);
      //updateDataFromSQL(sql);
    }

    public void updateOrderHeader(CCITable orderHeader)
    {
      string sql = string.Empty;
      string fieldName = string.Empty;
      //Hashtable keyFields = new Hashtable();
      ArrayList keyFields = new ArrayList();
      Hashtable fields = new Hashtable();

      /*
       * TODO: Here is WHERE we NEED the FieldType! to add the correct format to the SQL string being generated!
       */

      foreach (string keyField in orderHeader.KeyColumns.Keys)
        keyFields.Add(keyField);

      for (int i = 0; i < orderHeader.NumberColumns; i++)
      {
        fieldName = orderHeader.Column(i);
        if (!(orderHeader[0, fieldName] == null || orderHeader[0, fieldName].ToString().Trim().Equals(CommonData.NOCHANGE, StringComparison.CurrentCultureIgnoreCase)))
          fields.Add(fieldName, getFormattedFieldValue(fieldName, 0, orderHeader));
      }

      string[] keyList = new string[keyFields.Count];
      string[] fieldList = new string[fields.Count];
      string[] valueList = new string[fields.Count];

      int k = 0;
      foreach (string keyField in keyFields)
      {
        keyList[k] = keyField;
        k++;
      }

      k = 0;
      foreach (string field in fields.Keys)
      {
        fieldList[k] = field;
        valueList[k] = (string)fields[field];
        k++;
      }

      /*
       * TODO: We now have all the data in string arrays to build the corresponding Insert or Update SQL
       * Pending to write and test it!!
       */
      sql = makeUpdateSQL(fieldList, keyList, valueList, CommonData.tableORDERS);
      updateDataFromSQL(sql);
    }

    public CCITable updateOrderDetail(int orderID, CCITable orderDetail)
    {
      string sql = string.Empty;
      string fieldName = string.Empty;
      string fieldValue = string.Empty;
      string fieldValueType = string.Empty;
      ArrayList keyFields = new ArrayList();
      Hashtable fields = new Hashtable();
      Hashtable fieldTypes = new Hashtable();

      /*
       * TODO: Here is WHERE we NEED the FieldType! to add the correct format to the SQL string being generated!
       */
      foreach (string keyField in orderDetail.KeyColumns.Keys)
        keyFields.Add(keyField);

      //string itemID = string.Empty;
      string sectionName = string.Empty;
      string lineNumber = string.Empty;
      string itemID = string.Empty;
      string retailItemID = string.Empty;   // for debugging
      long detailID = 0;
      DataSet dsGetRetailDATA = null;

      //END If RetailItemID is NOT empty AND the RetailMRC is empty or Zero, we search it in the PricesList table and get the value for it

      for (int iRow = 0; iRow < orderDetail.NumberRows; iRow++) 
      {
        fields = new Hashtable();
        fieldTypes = new Hashtable();
        // add bool passthru here, and check to see if the orderDetail columnns contain it, then use CommonFunctions.CBoolean(val, default) to set with default = false
        //  if passthru = true and itemid is empty then itemid = "WINST"
        sectionName = CommonFunctions.CString(orderDetail[iRow, CommonData.fieldSECTIONAME]);
        lineNumber = CommonFunctions.CString(orderDetail[iRow, CommonData.fieldLINENUMBER]);
        itemID = CommonFunctions.CString(orderDetail[iRow, CommonData.fieldITEMID]);
        retailItemID = CommonFunctions.CString(orderDetail[iRow, CommonData.fieldRETAILITEMID]);   // was commented

        detailID = 0;
        //If the ItemID is NOT Empty, we first search for it by ItemID
        if (!string.IsNullOrEmpty(itemID)
          && (existsRecord(CommonData.tableORDERDETAIL,
          new string[] { CommonData.fieldORDERID, CommonData.fieldITEMID },
          new string[] { orderID.ToString(), itemID }))) 
        {
          dsGetRetailDATA = getDataFromSQL(string.Format(
            @"SELECT DetailID FROM OrderDetail 
                WHERE OrderID = {0} AND ItemID = '{1}'"
            , orderID.ToString(), itemID));
          if (dsGetRetailDATA != null && dsGetRetailDATA.Tables != null && dsGetRetailDATA.Tables[0].Rows != null && dsGetRetailDATA.Tables[0].Rows.Count == 1)
            detailID = CommonFunctions.CLng(CommonFunctions.getDBValue(null, dsGetRetailDATA.Tables[0].Rows[0][0]));
        }

        //If ItemID was empty or it was NOT empty but was NOT found, we search by Section and LineNumber
        if(detailID == 0 && existsRecord(CommonData.tableORDERDETAIL, 
          new string[] { CommonData.fieldORDERID, CommonData.fieldSECTIONAME, CommonData.fieldLINENUMBER}, 
          new string[] { orderID.ToString(), sectionName, lineNumber}))
        {
          dsGetRetailDATA = getDataFromSQL(string.Format(
            @"SELECT DetailID FROM OrderDetail 
                WHERE OrderID = {0} AND Section = '{1}'  AND LineNumber = {2}"
            , orderID.ToString(), sectionName, lineNumber));
          if (dsGetRetailDATA != null && dsGetRetailDATA.Tables != null && dsGetRetailDATA.Tables[0].Rows != null && dsGetRetailDATA.Tables[0].Rows.Count == 1)
            detailID = CommonFunctions.CLng(CommonFunctions.getDBValue(null, dsGetRetailDATA.Tables[0].Rows[0][0]));
        }

        //If DetailID is still zero, then this is a NEW record, we INSERT it...
        if (detailID == 0) 
        {
          //We are in an insert! let's see if this is a VALID save, that means, let's see if it is an insert based on the ACTION column
          if (CommonFunctions.CString(orderDetail[iRow, CommonData.ACTION]).ToLower() == CommonData.SQLDELETEROW.ToLower())
            continue;
          // note: we only check for blank records on the add, not the update. This means the user can blank out an old record if they want to
          bool isBlankRecord = true; // look for blank records... so we don't save them
          for (int j = 0; j < orderDetail.NumberColumns; j++)
          {
            fieldName = orderDetail.Column(j);
            if (fieldName == CommonData.REQUESTROW)
              continue;
            if (fieldName == CommonData.ACTION)
              continue;

            fieldValue = string.Empty;
            
            if (!(orderDetail[iRow, fieldName] == null || orderDetail[iRow, fieldName].ToString().Trim().Equals(CommonData.NOCHANGE, StringComparison.CurrentCultureIgnoreCase)))
               fieldValue = getFormattedFieldValue(fieldName, iRow, orderDetail);
            // if this looks like a blank record, but one of the test fields has data, then we flag it as NOT blank
            if (isBlankRecord // we test this first, so we don't keep looking in the list if we know the record is not blank
                              // we also test the Item field first, cause it is the most likely not to be blank
                && CommonFunctions.inList(detailBlankRowFields, fieldName) && !string.IsNullOrEmpty(fieldValue))
              isBlankRecord = false;
            fields.Add(fieldName, fieldValue);
          }
          if (isBlankRecord)
            continue; // skip this record if it is blank
          string[] fieldList = new string[fields.Count];
          string[] valueList = new string[fields.Count];

          fields.Keys.CopyTo(fieldList, 0);
          fields.Values.CopyTo(valueList, 0);

          detailID = createRecordReturnID(fieldList, valueList, "CreateOrderDetailReturnID", CommonData.fieldDETAILID.ToLower());
          orderDetail[iRow, CommonData.fieldDETAILID] = detailID;
        }
        else 
        {
          //Since it is an update/delete we first set the DetailID to the one we are updating/deletinng!
          orderDetail[iRow, CommonData.fieldDETAILID] = detailID;

          //This seems to be an update, let's see if the record exists!
          if (existsRecord(CommonData.tableORDERDETAIL
            , new string[] { CommonData.fieldORDERID, CommonData.fieldDETAILID }
            , new string[] { orderID.ToString(), detailID.ToString() })) 
          {
            //Let's see if we are in an update or a delete!
            if (CommonFunctions.CString(orderDetail[iRow, CommonData.ACTION]).ToLower() == CommonData.SQLUPDATEROW.ToLower())
            {                
              for (int j = 0; j < orderDetail.NumberColumns; j++)
              {
                fieldName = orderDetail.Column(j);
                if (fieldName == CommonData.REQUESTROW 
                  || fieldName == CommonData.ACTION)
                  continue;

                if (!(orderDetail[iRow, fieldName] == null || orderDetail[iRow, fieldName].ToString().Trim().Equals(CommonData.NOCHANGE, StringComparison.CurrentCultureIgnoreCase)))
                {
                  fields.Add(fieldName, getFormattedFieldValue(fieldName, iRow, orderDetail));
                  fieldTypes.Add(fieldName, orderDetail.getFieldType(fieldName));
                }
              }
            }
            else
            {
              fields.Add(CommonData.fieldORDERID, getFormattedFieldValue(CommonData.fieldORDERID, iRow, orderDetail));
              fieldTypes.Add(CommonData.fieldORDERID, orderDetail.getFieldType(CommonData.fieldORDERID));

              fields.Add(CommonData.fieldDETAILID, getFormattedFieldValue(CommonData.fieldDETAILID, iRow, orderDetail));
              fieldTypes.Add(CommonData.fieldDETAILID, orderDetail.getFieldType(CommonData.fieldDETAILID));
            }

            string[] keyList = new string[keyFields.Count];
            string[] fieldList = new string[fields.Count];
            string[] valueList = new string[fields.Count];
            string[] valueTypeList = new string[fieldTypes.Count];

            int k = 0;
            foreach (string keyField in keyFields)
              keyList[k++] = keyField;

            k = 0;
            foreach (string field in fieldTypes.Keys)
              valueTypeList[k++] = (string)fieldTypes[field];
 
            k = 0;
            foreach (string field in fields.Keys)
            {
              fieldList[k] = field;
              valueList[k++] = (string)fields[field];
            }

            //Let's see if we are in an update or a delete!
            if (CommonFunctions.CString(orderDetail[iRow, CommonData.ACTION]).ToLower() == CommonData.SQLUPDATEROW.ToLower())
              sql = makeUpdateSQL(fieldList, keyList, valueList, valueTypeList, CommonData.tableORDERDETAIL);
            else
              sql = makeDeleteSQL(fieldList, valueList, CommonData.tableORDERDETAIL);

            updateDataFromSQL(sql);
          }
        }
      }
      postUpdateOrderDetail(orderID);
      return orderDetail;
    }
    
    public ArrayList getOrderTotals(int orderID)
    {
      ArrayList getOrderTotals = new ArrayList();
      string dealerLevel = getDealerLevel(getDealerFromOrder(orderID.ToString()), DateTime.Today, "Bronze");
      string sqlGrandTotals = string.Format(
        @"SELECT 'GrandTotal' AS ScreenSection
	            ,SUM(IsNull(o.retailMRC,0) * IsNull(o.Quantity, 0)) AS TotalMonthly
	            ,SUM(IsNull(o.RetailNRC,0) * IsNull(o.Quantity, 0)) AS TotalOneTime
				,SUM(Coalesce(o.DealerCost, dlr.DealerCost, lvl.DealerCost, 0)) TotalDealerMonthly
				,SUM(Coalesce(dlr.Install, lvl.Install, o.RetailNRC)) TotalDealerOneTime				
            	,SUM(IsNull(o.retailMRC,0) * IsNull(o.Quantity, 0)) - SUM(Coalesce(o.DealerCost, dlr.DealerCost, lvl.DealerCost, 0) * IsNull(o.Quantity, 0)) AS ProfitMonthly
            	,SUM(IsNull(o.RetailNRC,0) * IsNull(o.Quantity, 0)) - SUM(Coalesce(dlr.Install, lvl.Install, o.RetailNRC) * IsNull(o.Quantity, 0)) AS ProfitOneTime
	            ,h.ccamounttopay * -1 DealerContribution
	        FROM OrderDetail o 
	        LEFT JOIN Orders h on o.orderid = h.id
	        LEFT JOIN SalesOrDealerCustomers dc on dc.Customer = h.customer and dc.SalesType = 'Dealer'
	        LEFT JOIN HostedDealerCosts dlr on dlr.Dealer = dc.SalesOrDealer and dlr.itemid = o.ItemID
	        LEFT Join HostedDealerCosts lvl on lvl.Dealer = '{1}' and lvl.ItemID = o.ItemID
	        WHERE o.OrderID = {0} 
          group by ccamounttopay", orderID.ToString(), dealerLevel);

      //getOrderTotals.Add(CommonFunctions.convertDataSetToCCITable(getDataFromSQL(sqlSubTotals)));
      getOrderTotals.Add(CommonFunctions.convertDataSetToCCITable(getDataFromSQL(sqlGrandTotals)));

      return getOrderTotals;
    }

    public CCITable getPriceAndUseMRC(string fieldName) 
    {
      if(!fieldName.Contains("-")) {
        return null;
      }
      string[] whereFields = fieldName.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
      if (whereFields.GetLength(0) != 2) { 
        return null;
      }
      
      string sqlGetPriceAndUseMRC =
        string.Format(@"SELECT p.RetailUsoc, p.Price, IsNull(s.useMRC, 1) useMRC
            FROM [HostedUSOCRetailPricing] p
            INNER JOIN ScreenDefinition s ON p.RetailUsoc = s.ItemID
            WHERE p.RetailUsoc = '{0}' AND p.Price = {1}"
        , whereFields[0], whereFields[1]);

      return CommonFunctions.convertDataSetToCCITable(getDataFromSQL(sqlGetPriceAndUseMRC));
    }

    /// <summary>
    /// lookup wholesale and retail charges for the order and update the order detail
    /// </summary>
    /// <param name="orderID"></param>
    private void postUpdateOrderDetail(int orderID)
    {
      string sql = string.Format(@"update orderdetail
        set wholesalemrc = w.mrc, 
        wholesalenrc = w.nrc, 
        retailmrc = case when r.mrc = -1 then d.retailmrc else r.mrc end, 
        retailnrc = r.nrc
        from orderdetail d
        inner join productlist r on d.retailitemid = r.itemid and r.carrier = 'cityhosted'
        inner join productlist w on d.itemid = w.itemid and w.carrier = 'saddleback'
        where orderid = {0}", orderID.ToString());
      updateDataFromSQL(sql);
    }
    
  }
}
