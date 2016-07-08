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
    public DataSet getResearchData(CommonData.UnmatchedNameTypes type, object oWhere, int maxCount)
    {
      #region setup data
      string fromClause = string.Empty;
      string orderbyClause = string.Empty;
      string where = string.Empty;
      string innerWhere = string.Empty;
      if (oWhere != null)
      {
        if (oWhere.GetType() == typeof(string))
          where = CommonFunctions.CString(oWhere);
        else
          if (oWhere.GetType() == typeof(string[]))
          {
            string[] whereClauses = (string[])oWhere;
            if (whereClauses.GetLength(0) > 0)
              where = whereClauses[0];
            if (whereClauses.GetLength(0) > 1)
              innerWhere = whereClauses[1];
          }
      }
      /*
       * when we build the final sql statement, and add the "WHERE" clause, sometimes the fromClause has a WHERE clause inside
       * it and we have to convert the "WHERE" to and "AND". This is mostly automatic. However, sometimes there is an 
       * embedded WHERE inside the statement that is not the final where, and this confuses the automatic logic.
       * 
       * In this case, the case statement that applies must set this flag to override the automatic logic and
       * explicitly leave the "WHERE" alone, not changing it to an "AND". 
       * 
       * This flag controls this.
       */
      bool overridewhere = false; 
      #endregion
      switch (type)
      {
        case CommonData.UnmatchedNameTypes.CityHostedMRCWholesaleMatch:
          #region CityHostedMRCWholesaleMatch
          fromClause = @" * from (select distinct CustomerID, e.LegalName CustomerName, WholesaleUSOC, m.Name WholesaleUSOCName, WholesaleBillDate, 
Convert(decimal(10,2),Convert(decimal(10,2),WholesaleAmount) / Convert(decimal(10,2),WholesaleQty)) SaddlebackMRC,
p.MRC OurMRC, case when p.MRC <>  Convert(decimal(10,2),WholesaleAmount) / Convert(decimal(10,2),WholesaleQty) then 'No' else 'Yes' end Matched
from hostedmatchedmrc mrc
left join productlist p on mrc.wholesaleusoc = p.itemid and p.carrier = 'saddleback' 
left join masterproductlist m on mrc.wholesaleusoc = m.itemid
inner join Entity e on e.Entity = mrc.CustomerID) a ";
          orderbyClause = " order by WholesaleBillDate, CustomerName, WholesaleUSOC ";
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.CityHostedMRCNetworkInventoryMatch:
          #region CityHostedMRCNetworkInventoryMatch
          fromClause = string.Format(@" * FROM ( Select a.*, case when ex.ExceptionScreen is null then 'No' else 'Yes' end ExceptionLogged, ex.Comment,
f.AssignedTo, f.DueDate, f.DispositionCode
from (Select mrc.CustomerID, 
			c.LegalName CustomerName, 
			mrc.RetailUSOC, p.Name ProductName, 
			Convert(int,mrc.Qty) SaddlebackQty, 
            Convert(int, n.Qty) NetworkInventoryQuantity, 
            Convert(Decimal(10,2),mrc.Price) SaddlebackMRC, 
            Convert(decimal(10,2), n.Price) NetworkInventoryMRC, 
            case 
	            when n.CustomerID IS null then '{0}' 
	            when mrc.Qty <> n.Qty then '{1}' 
	            when mrc.Price <> n.Price then '{2}' 
	            else null end Exception,
	        mrc.RetailBillDate
            from (
            select retailbilldate, CustomerID, RetailUSOC, 
				sum(RetailQty) Qty, Max(Convert(decimal(10,2),retailamount) / Convert(decimal(10,2),RetailQty)) Price
            from hostedmatchedmrc 
            group by retailbilldate, CustomerID, retailusoc) mrc
            left join (select Customer customerID, ItemId RetailUsoc, SUM(quantity) Qty, MAX(MRC) Price
            from NetworkInventory ni
            group by Customer, ItemID) n
            on mrc.CustomerID = n.customerID and mrc.RetailUSOC = n.RetailUsoc
            left join Entity c on mrc.CustomerID = c.entity
            left join MasterProductList p on p.ItemID = mrc.RetailUSOC
            left join ProductList pr on pr.ItemID = mrc.RetailUSOC and pr.Carrier = 'CityHosted'
        where isnull(pr.ExcludeFromException,0) <> 1
union
select customer CustomerID, c.LegalName CustomerName, ni.Itemid RetailUSOC, p.Name ProductName,
null SaddlebackQty, ni.quantity NetworkInventoryQuantity, null SaddlebackMRC, Convert(decimal(10,2), ni.MRC) NetworkInventoryMRC,
'{3}' Exception, bd.RetailBillDate
from NetworkInventory ni
left join (select distinct customerid, retailusoc from hostedmatchedmrc) mrc
on ni.customer = mrc.customerid and ni.itemid = mrc.retailusoc and ni.carrier = 'cityhosted'
    left join Entity c on ni.Customer = c.entity
    left join MasterProductList p on p.ItemID = ni.ItemID
    left join ProductList pr on pr.ItemID = mrc.RetailUSOC and pr.Carrier = 'CityHosted'
inner join (select max(retailbilldate) RetailBillDate from hostedmatchedmrc) bd on 1 = 1
where mrc.customerid is null and ni.carrier = 'cityhosted'
    and isnull(pr.ExcludeFromException,0) <> 1
) a
left join ExceptionList ex on ex.ExceptionScreen = 'CityHostedMRCNetworkInventoryMatch' and ex.ExceptionType = a.Exception
 and ex.CustomerID = a.CustomerID and ((ex.IsOneTime = 1 and a.RetailBillDate = ex.BillDate) or (ex.IsOneTime = 0 and a.RetailBillDate >= ex.BillDate) or ex.BillDate is null)
 and isnull(ex.WholesaleUSOC, '') = ''
 and case when ex.RetailUSOC is null or ex.RetailUSOC = '' then  a.RetailUSOC else ex.RetailUSOC end = a.RetailUSOC
 left join FollowUps f on ex.FollowUpID = f.ID
 ) b ", CommonData.NetworkInventoryExceptionList[0], 
       CommonData.NetworkInventoryExceptionList[1], 
       CommonData.NetworkInventoryExceptionList[2], 
       CommonData.NetworkInventoryExceptionList[3]);
          orderbyClause = " order by CustomerName, RetailUSOC ";
          overridewhere = true;
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.CashReceipts:
          #region CashReceipts
          DateTime fromDate = DateTime.Today.AddDays(-DateTime.Today.Day + 1); // bom
          DateTime toDate = DateTime.Today.AddMonths(1).AddDays(-DateTime.Today.Day); // eom
          DateTime billDate = fromDate;
          if (!string.IsNullOrEmpty(innerWhere))
          {
            string[] parts = innerWhere.Split(new char[] { ',' });
            if (parts != null && parts.GetLength(0) == 2)
            {
              fromDate = CommonFunctions.CDateTime(parts[0], fromDate);
              toDate = CommonFunctions.CDateTime(parts[1], toDate);
            }
          }
          fromClause = string.Format(@" * from (select convert(bit, case when r.Amount is null then 0 else 1 end) Pay, Convert(decimal(12,2), -r.Amount) PaidAmount,  
  case when r.NonSaddlebackAmount = 0 then null else Convert(decimal(12,2),-r.NonSaddlebackAmount) end NonSaddlebackAmount, r.Reference CheckNumber, Convert(decimal(12,2),b.Amount) [Current Bill], 
	Convert(decimal(12,2),s.Amount) [Balance Owed],
	r.TransactionDate, e.LegalName CustomerName, isnull(e.AlternateID, '') SaddlebackID, s.customerid Customer,  
	Coalesce(r.TransactionSubType, a.Value, 'Check') PaymentType, 
	r.ExportDateTime PostedDate,  case when r.ExportDateTime is null then 'No' else 'Yes' end Exported,
	case when r.Amount is null then 'No' else 'Yes' end Paid, 
	case when r.customerid is null then 'Open' else 'Paid' end Status, 
	coalesce(s.BillDate, r.BillDate) BillDate, 
	coalesce(r.TransactionDate, s.billdate, r.billdate) EffectiveDate,
	r.TransactionDateTime, r.ID, r.LastModifiedBy, r.LastModifiedDateTime, null Comment 
	from (Select CustomerID, Max(BillDate) BillDate, isnull(MAX(TransactionDate),'') TransactionDate, 
			SUM(amount) Amount from ARTransactions
			where TransactionDate <= '{1}'
			Group by CustomerID) s
	left join (Select CustomerID, BillDate, Max(TransactionDate) TransactionDate, sum(Amount) Amount, sum(NonSaddlebackAmount) NonSaddlebackAmount, max(Reference) Reference, max(TransactionSubType) TransactionSubType, 
				max(ExportDateTime) ExportDateTime, max(TransactionDateTime) TransactionDateTime, 
				max(LastModifiedBy) LastModifiedBy, max(LastModifiedDateTime) LastModifiedDateTime, max(ID) ID
				from ARTransactions where TransactionType = 'Cash'
				 and TransactionDate between '{0}' and '{1}'
				group by  CustomerID, BillDate) r
		on r.CustomerID = s.CustomerID and r.BillDate = s.BillDate
	left join (Select CustomerID, BillDate, TransactionDate, Amount, Reference, TransactionSubType 
				from ARTransactions 
				where TransactionType = 'Invoice'
				 and TransactionDate between '{2}' and '{1}'
				) b
		on b.CustomerID = s.CustomerID and b.BillDate = s.BillDate
	inner join Entity e on e.entity = s.customerid
	left join vw_attributenonxml a on a.Entity = s.CustomerID and a.ItemType = 'Entity' and a.Item = 'Customer' and a.Name = 'PaymentType') a   ",
            fromDate.ToShortDateString(), toDate.ToShortDateString(), billDate.ToShortDateString());
          orderbyClause = " order by CustomerName, EffectiveDate";
          overridewhere = true;
           break;
        case CommonData.UnmatchedNameTypes.CashReceiptsDetail:
           fromClause = @" * from (Select ID, CustomerID, 
case when TransactionType = 'Invoice' then 'Bill' else TransactionType end TransactionType, 
TransactionDate, BillDate, 
	case when TransactionType = 'CASH' then TransactionSubType else null end PaymentType, 
	Amount, NonSaddlebackAmount, Reference, 
	case when ExportDateTime is null then 'No' else 'Yes' end Exported, Comment
	from artransactions) a";
           orderbyClause = " order by TransactionDate ";
           break;
          #endregion
        case CommonData.UnmatchedNameTypes.CityHostedDealerCost:
           #region CityHostedDealerCost
           fromClause = @" c.Dealer, c.itemid USOC, p.name Description, c.dealercost, c.install
from hosteddealercosts c
inner join masterproductlist p on c.itemid = p.itemid";
          orderbyClause = " order by p.name ";
          break;
           #endregion
        case CommonData.UnmatchedNameTypes.CityHostedCustomerUSOCMatching:
          #region CityHostedCustomerUSOCMatching
          fromClause = @" * from vw_CityHostedCustomerUSOCMatching ";
          orderbyClause = " order by CustomerName, Source, RetailUSOC, WholesaleUSOC";
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.CityHostedOCCUSOCMatching:
        #region CityHostedOCCUSOCMatching
          fromClause = @" * from vw_CityHostedOCCMatching ";
          orderbyClause = " order by CustomerName, Exception, RetailUSOC, WholesaleUSOC ";
          break;
        #endregion
        case CommonData.UnmatchedNameTypes.CityHostedLedgerInvoiceMatch:
          fromClause = @" * from vw_CityHostedLedgerInvoiceMatching ";
          orderbyClause = " order by CustomerName, Exception ";
          break;
        case CommonData.UnmatchedNameTypes.CityHostedLedgerCashMatch:
          fromClause = @" * from vw_CityHostedLedgerCashMatching ";
          orderbyClause = " order by CustomerName, Exception ";
          break;
        case CommonData.UnmatchedNameTypes.Customer:
          #region Customer
        case CommonData.UnmatchedNameTypes.ImportCustomer:
        case CommonData.UnmatchedNameTypes.TrueUp:
          fromClause = " * from vw_MasterCustomerOrders";
          orderbyClause = " order by customername, pcity, pstate, paddressprefix, paddress";
          break;
        case CommonData.UnmatchedNameTypes.CityHostedCustomersWithMissingInfo:
          fromClause = " * from vw_SaddlebackCustomersWithMissingInfo";
          orderbyClause = " order by CustomerName ";
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.ScreenDefinition:
          #region ScreenDefinition
          fromClause = @" s.Screen, sec.IntVal1 SectionSequence, s.ScreenSection ScreenSection, s.Sequence ItemSequence, s.ItemID, m.Name, s.IsRecommended, s.ImagePath, description from ScreenDefinition s
inner join MasterProductList m on s.ItemID = m.ItemID
inner join ProductList p on p.ItemID = s.ItemID and p.Carrier = 'saddleback'
inner join CodeMaster sec on sec.CodeValue =  s.ScreenSection
 where GETDATE() between s.StartDate and ISNULL(s.EndDate, '{1}') and GETDATE() between ISNULL(p.StartDate, '{0}') and ISNULL(p.EndDate, '{1}')";
          orderbyClause = "order by s.Screen, sec.IntVal1, s.Sequence, s.ItemID";
          fromClause = string.Format(fromClause, CommonData.PastDateTime.ToShortDateString(), CommonData.FutureDateTime.ToShortDateString());
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.Payor:
          #region Payor
          fromClause = "* from vw_Vendors";
          orderbyClause = " order by VendorName";
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.Location:
          #region Location
          fromClause = "* from vw_MasterCustomerList";
          orderbyClause = " order by Customer, Location";
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.NetworkInventory:
          #region NetworkInventory
          fromClause = " * from vw_NetworkInventoryList";
          orderbyClause = " order by Customer, Location, ItemID ";
          break;        
        case CommonData.UnmatchedNameTypes.UnmatchedNetworkInventory:
          // only get records from vw_NetworkInventoryMatchup (a view from CommissionsRaw) which meet the unmatched criteria defined by the where clause
          /* 
           * this one is different since the it has both an inner where and an regular outer where
           * scrubbed	CustomerID	CustomerName	 Vendor	 VendorID	MRC	 emu	MRC_CR	FirstPRDate	LastPRDate	CommissionID	 ProfileID	EMU_CR	 BTN
           */
          fromClause = " scrubbed,	CustomerID,	CustomerName, Vendor, Carrier, MRC, emu, MRC_CR, FirstPRDate, LastPRDate, CommissionID, ProfileID, EMU_CR, VendorID from NetworkInventoryMatchup a";
          if (!string.IsNullOrEmpty(innerWhere))
            fromClause += string.Format(" WHERE {0}", innerWhere);
          //fromClause += ")";
          orderbyClause = " order by customername, commissionid";
          break;
        case CommonData.UnmatchedNameTypes.UnmatchedNetworkInventoryOneRow:
          fromClause = " scrubbed,	CustomerID,	CustomerName, Vendor, Carrier, MRC, emu, MRC_CR, FirstPRDate, LastPRDate, CommissionID, ProfileID, EMU_CR,  VendorID from NetworkInventoryMatchup";
          orderbyClause = " order by customername, commissionid";
          break;
        case CommonData.UnmatchedNameTypes.CustomerNetworkInventory:
          fromClause = @" * from (
Select Customer, 
isnull(l.LegalName, 'Main') Location, 
ni.Carrier, ni.ItemID, m.Name ProductName, ni.Quantity, ni.MRC, ni.EMU, ni.NRC, ni.StartDate, ni.EndDate,
	ni.CircuitID, ni.BTN, ni.VendorAccountID, ni.Comments 
from networkinventory ni
inner join MasterProductList m on ni.itemid = m.itemid
left join entity l on ni.location = l.entity
) a";
          orderbyClause = " order by Location, ProductName, EndDate desc ";
          break;
        case CommonData.UnmatchedNameTypes.SearchNetworkInventory:
          fromClause = @"* from (Select Upper(NeedsReview) NeedsReview, NeedsReviewComments, ni.Customer, c.Legalname CustomerName, ni.Location, l.LegalName LocationName, Upper(ni.Carrier) Carrier, 
                        ni.ItemId, p.Name ProductName, ProductNickName, ni.Quantity, ni.MRC, ni.NRC, ni.Comments, ni.CircuitID, ni.BTN, ni.AccountManager, u.LegalName AccountManagerName, ni.StartDate, ni.EndDate, 
CityCareProdOrderID, CityCareNetInvID,  ni.VendorAccountID, ni.VendorInvoiceNumber, ni.LastModifiedBy, ni.LastModifiedDateTime,   
ni.CycleCode, ni.ID, Case when ni.InActive is null then 'Yes' else 'No' end Active, coalesce(ni.OrderID, ni.CityCareProdOrderID) OrderID, po.orderid CityCareOrder from NetworkInventory ni
inner join Entity c on ni.Customer = c.Entity
left join Entity l on ni.Location = l.Entity
inner join MasterProductList p on ni.Itemid = p.Itemid
left join Entity u on u.Entity = ni.AccountManager
left join citycare_prod.dbo.tblProdOrder po on po.prodorderid = coalesce(ni.OrderID, ni.CityCareProdOrderID)) a  ";
          orderbyClause = "order by CustomerName, LocationName, ItemID";
          break;
        case CommonData.UnmatchedNameTypes.CityCareNetworkInventory:
          fromClause = @" * from (SELECT  ni.netinv_id, ni.user_customerid, c.customername, ni.location_id, l.locationname AS Location, p.productid, 
p.productname AS Product, v.vendorid, v.vendorname AS Vendor, pc.PRODCLASSID, 
case when o.prodtype is null then 'Unknown'
	 when o.prodtype = 'LV' then 'Local Voice' 
	 when o.prodtype = 'LD' then 'Long Distance' 
	 when o.prodtype = 'DT' then 'Data' 
	 else 'Other' end Type, 
sc.PRODSUBCLASSID, 
sc.PRODUCTSUBCLASS AS [Sub Type], ni.active AS Active, ni.vendoraccno AS Acount, isnull(po.qty, 1) Quantity, ni.mrc AS MRC, ni.expdate AS [Exp Date],
Convert(nvarchar(5), 20000 + c.customerid) Customer, ni.netinv_id ID, ni.prodorderid, po.orderid CityCareOrder
FROM citycare_prod.dbo.I_networkinventory ni 
INNER JOIN citycare_prod.dbo.tblCustomers c ON ni.user_customerid = c.customerid 
INNER JOIN citycare_prod.dbo.tblLocations l ON ni.user_customerid = l.customerid AND ni.location_id = l.locID 
INNER JOIN citycare_prod.dbo.tblProducts p ON ni.product_id = p.productid 
INNER JOIN citycare_prod.dbo.tblVendors v ON p.vendorname = v.vendorid 
INNER JOIN citycare_prod.dbo.tblProdClass pc ON p.productype = pc.PRODCLASSID 
INNER JOIN citycare_prod.dbo.tblProductSubClass sc ON p.PRODUCTSUBCLASS = sc.PRODSUBCLASSID 
LEFT JOIN citycare_prod.dbo.tblProdOrder po on po.prodorderid = ni.prodorderid
LEFT JOIN citycare_prod.dbo.tblOrders o on o.orderid = po.orderid
) ni";
          orderbyClause = " order by ni.Customer, ni.location, ni.product";
          break;
        case CommonData.UnmatchedNameTypes.OldNetworkInventory:
          fromClause = " * from vw_CityCareNetworkInventory";
          orderbyClause = " order by CustomerName, LocationName, ProductName ";
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.Profiles:
          #region Profiles
          fromClause = " * from vw_OrdersProfile";
          orderbyClause = " order by CommissionID ";
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.Orders:
          #region Orders
          fromClause = " * from vw_CityCareOrders";
          orderbyClause = " order by CustomerName, LocationName, ProductName, ID ";
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.CityHostedUSOCs:
          #region CityHostedUSOCs
          fromClause = " * from vw_CityHostedUSOCPresentation";
          orderbyClause = " order by USOCRetail ";
          break;
        case CommonData.UnmatchedNameTypes.CityHostedUSOCsRetail:
          fromClause = "m.Name Description, u.USOCRetail, u.MRCRetail, u.NRCRetail, u.USOCWholesale, u.RetailActive, u.WholesaleActive from vw_CityHostedUSOCs u inner join MasterProductList m on u.USOCRetail = m.ItemId";
          orderbyClause = " order by m.Name";
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.CityHostedMRCMatching:
          #region CityHostedMRCMatching
          string filter = string.IsNullOrEmpty(innerWhere) ? string.Empty : string.Format(" AND {0} ", innerWhere);
          fromClause = string.Format(" * from vw_HostedMatchedMRC Where MatchedBy is null {0}", filter);
          orderbyClause = " order by Customer, [Whsle BTN], [Rtl BTN], [Whsle USOC], [Rtl USOC], [Whsle Bill Date], [Rtl Bill Date]";
          break;
        case CommonData.UnmatchedNameTypes.CityHostedDetailWholesaleMRC:
          fromClause = @" * from (select m.CustomerID, Coalesce(c.LegalName, m.CustomerID) CustomerName, 
	WholesaleBillDate BillDate, WholesaleBTN BTN, WholesaleUSOC USOC, pw.Name USOCName, WholesaleAmount WholesaleMRC, WholesaleQty,
            RetailBTN, RetailUSOC, pr.Name RetailUSOCName, RetailAmount RetailMRC, RetailQty, 
    cast(case when retailUSOC is null then 0 else 1 end as bit) MatchedYN 
from hostedmatchedmrc m left join entity c on m.customerid = c.entity 
left join MasterProductList pw on pw.itemid = m.WholesaleUSOC 
left join MasterProductList pr on pr.itemid = m.RetailUSOC ) d 
 WHERE BillDate is not null";
          orderbyClause = string.Empty;
          break;
        case CommonData.UnmatchedNameTypes.CityHostedDetailWholesaleOCC:
          fromClause = @" * from (select m.CustomerID, Coalesce(c.LegalName, m.CustomerID) CustomerName, WholesaleBillDate BillDate, WholesaleBTN BTN,
WholesaleService, WholesaleUSOC USOC, WholesaleAmount 
from hostedmatchedocc m left join entity c on m.customerid = c.entity) d WHERE BillDate is not null";
          orderbyClause = string.Empty;
          break;
        case CommonData.UnmatchedNameTypes.CityHostedDetailWholesaleToll:
          fromClause = @" * from (select c.Entity CustomerID, c.LegalName CustomerName, Convert(date, WholesaleBillingMonth + '/01/' + WholesaleBillingYear) BillDate, WholesaleBTN BTN,
FromNumber, ToNumber, WholesaleCount, WholesaleCharge 
from hostedmatchedtoll m left join entity c on m.alternateid = c.alternateid) d  WHERE BillDate is not null";
          orderbyClause = string.Empty;
          break;
        case CommonData.UnmatchedNameTypes.CityHostedDetailRetailMRC:
          fromClause = @" * from (select m.CustomerID, Coalesce(c.LegalName, m.CustomerID) CustomerName, 
	RetailBillDate BillDate, RetailBTN BTN, RetailUSOC USOC, pr.Name RetailUSOCName, RetailAmount RetailMRC, RetailQty,
    WholesaleBTN, WholesaleUSOC, pw.Name WholesaleUSOCName, WholesaleAmount WholesaleMRC, WholesaleQty, 
    cast(case when wholesaleusoc is null then 0 else 1 end as bit) MatchedYN 
from hostedmatchedmrc m 
left join entity c on m.customerid = c.entity
left join MasterProductList pw on pw.itemid = m.WholesaleUSOC 
left join MasterProductList pr on pr.itemid = m.RetailUSOC ) d 
 WHERE BillDate is not null";
          orderbyClause = string.Empty;
          break;
        case CommonData.UnmatchedNameTypes.CityHostedDetailRetailOCC:
          fromClause = @" * from (select m.CustomerID, Coalesce(c.LegalName, m.CustomerID) CustomerName, RetailBillDate BillDate, RetailBTN BTN,
RetailService, RetailUSOC USOC, RetailAmount 
from hostedmatchedocc m left join entity c on m.customerid = c.entity) d WHERE BillDate is not null";
          orderbyClause = string.Empty;
          break;
        case CommonData.UnmatchedNameTypes.CityHostedDetailRetailToll:
          fromClause = @" * from (select c.Entity CustomerID, c.LegalName CustomerName, Convert(date, RetailBillingMonth + '/01/' + RetailBillingYear) BillDate, RetailBTN BTN,
FromNumber, ToNumber, RetailCount, RetailCharge 
from hostedmatchedtoll m left join entity c on m.alternateid = c.alternateid) d WHERE BillDate is not null";
          orderbyClause = string.Empty;
          break;
        case CommonData.UnmatchedNameTypes.CityHostedDetailTax:
          fromClause = @" * from (select c.Entity CustomerID, Coalesce(c.Legalname, m.Customer) CustomerName, BillDate, MasterBTN BTN, Title, 
Convert(decimal(18,2), case when Sign = '+' then TaxAmount else -TaxAmount end) Tax
from hostedtaxtransactions m left join entity c on m.Customer = c.AlternateID) d";
          orderbyClause = string.Empty;
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.CityHostedQueryMain:
          #region City Hosted Margin
          if (string.IsNullOrEmpty(innerWhere))
            innerWhere = string.Empty;
          else
            innerWhere = "'" + innerWhere + "'";
          fromClause = string.Format("exec CityHostedMarginReport {0} ", innerWhere); // where in this case is just the date
          orderbyClause = string.Empty;
          break;
        case CommonData.UnmatchedNameTypes.CityHostedMarginCustomerList:
          fromClause = @" * from vw_CityHostedMarginSummary ";
          orderbyClause = string.Empty;
          break;
        case CommonData.UnmatchedNameTypes.CityHostedMarginMRCSummary:
          fromClause = @" * from vw_CityHostedMarginMRCSummary ";
          orderbyClause = string.Empty;
          break;
        case CommonData.UnmatchedNameTypes.CityHostedMarginOCCSummary:
          fromClause = @" * from vw_CityHostedMarginOCCSummary ";
          orderbyClause = string.Empty;
          break;
        case CommonData.UnmatchedNameTypes.CityHostedMarginTollSummary:
          fromClause = @" * from vw_CityHostedMarginTollSummary ";
          orderbyClause = string.Empty;
          break;
        case CommonData.UnmatchedNameTypes.CityHostedMarginTaxSummary:
          fromClause = @"* from vw_CityHostedMarginTaxSummary ";
          orderbyClause = string.Empty;
          break;
        case CommonData.UnmatchedNameTypes.CityHostedMarginAllSummary:
          fromClause =string.Format(@"* from vw_CityHostedMarginAllSummary {0}", innerWhere);
          orderbyClause = string.Empty;
          break;
        case CommonData.UnmatchedNameTypes.CityHostedMarginMRCUSOCDetail:
          fromClause = @" * from vw_CityHostedMarginMRCUSOCList ";
          orderbyClause = string.Empty;
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.MasterProductList:
          #region Products
          fromClause = " * from MasterProductList";
          orderbyClause = " order by Name ";
          break;
        case CommonData.UnmatchedNameTypes.ProductList:
          fromClause = " m.Name, p.* from ProductList p inner join MasterProductList m on p.itemid = m.itemid";
          orderbyClause = " order by m.Name ";
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.FollowUps:
          #region FollowUps
          fromClause = @" * from (Select f.ID, f.AssignedTo, u.LegalName AssignedToName, f.CustomerID, c.LegalName CustomerName, 
f.OrderId, o.ShortName OrderShortName, f.Description, f.Notes, f.AsOfDate, f.DueDate, f.Source,
f.CreateDateTime, f.DispositionCode, f.CompleteDateTime, f.LastModifiedBy, f.LastModifiedDateTime,
Convert(bit, case when CompleteDateTime is null then 0 else 1 end) Completed 
from followups f
inner join entity u on f.assignedto = u.entity 
inner join entity c on f.customerid = c.entity
left join orders o on f.orderid = o.ID) a ";
          orderbyClause = " order by AssignedToName, DueDate, CustomerName, OrderShortName ";
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.ProcessLog:
          #region process log
          fromClause = @" * from (Select BillingDate, Step, 
      convert(bit, case when unprocesseddatetime is null then 0 else 1 end) UnProcessed, 
      ProcessedBy,  ProcessedDateTime, FileType, [FileName], UnProcessedDateTime 
FROM HostedProcessCycle) s ";
          orderbyClause = " order by s.ProcessedDateTime desc ";
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.AnalysisCustomerEntity:
          #region Analysis queries
          fromClause = " * from vw_CustomerEntityAnalysis ";
          orderbyClause = " order by CustomerName ";
          break;
        case CommonData.UnmatchedNameTypes.AnalysisAR:
          fromClause = " * from vw_ARAnalysis ";
          orderbyClause = " order by CustomerName ";
          break;
        case CommonData.UnmatchedNameTypes.AnalysisMarginRaw:
          fromClause = " * from vw_AnalysisMargin";
          orderbyClause = " order by CustomerName, BillDate";
          break;
        case CommonData.UnmatchedNameTypes.AnalysisDealerMargin:
          fromClause = " * from vw_DealerMargin";
          orderbyClause = " order by CustomerName, BillDate";
          break;
        case CommonData.UnmatchedNameTypes.AnalysisLocation:
          fromClause = " * from vw_CustomerLocationAnalysis ";
          orderbyClause = " order by Location ";
          break;
        case CommonData.UnmatchedNameTypes.AnalysisContact:
          fromClause = " * from vw_AnalysisContacts ";
          orderbyClause = " order by CustomerName, ContactName ";
          break;
        case CommonData.UnmatchedNameTypes.AnalysisNetworkInventory:
          fromClause = " * from vw_AnalysisNetworkInventory ";
          orderbyClause = " order by CustomerName, LocationName, ProductName ";
          break;
        case CommonData.UnmatchedNameTypes.AnalysisTaxDetailHistory:
          fromClause = " * from vw_TaxDetailHistory ";
          orderbyClause = " order by CustomerName, BillDate ";
          break;
        case CommonData.UnmatchedNameTypes.AnalysisARDetail:
          fromClause = " * from vw_AnalysisARDetail ";
          orderbyClause = " order by CustomerName, TransactionDate, BillDate ";
          break;
        case CommonData.UnmatchedNameTypes.AnalysisOCCImportRaw:
          fromClause = " * from vw_SaddlebackImportOCCRaw ";
          orderbyClause = " order by CustomerName, TransactionDate, BillDate ";
          break;
          #endregion
        case CommonData.UnmatchedNameTypes.CityHostedOCCEntry:
          fromClause = @" * from (select ID, Coalesce(dlr.SalesOrDealer, 'CCIDealer') Dealer, occ.CustomerID, e.AlternateID SaddlebackID, 
    e.LegalName CustomerName, occ.RetailBillDate BillDate, occ.RetailUSOC, occ.RetailAmount, 
    occ.WholesaleUSOC, occ.WholesaleAmount, Coalesce(occ.RetailService, occ.WholesaleService) Description, 
    case when occ.MatchedBy = 'ManualAdjustment' then 'Yes' else 'No' end ManualAdjustment
    from hostedmatchedocc occ
    inner join Entity e on e.Entity = occ.CustomerID
    left join SalesOrDealerCustomers dlr on occ.CustomerID = dlr.Customer) a";
          orderbyClause = " order by CustomerName, BillDate, RetailUSOC ";
          overridewhere = true;
          break;
        case CommonData.UnmatchedNameTypes.None:
          fromClause = " 'none' as None";
          orderbyClause = string.Empty;
          where = string.Empty;
          maxCount = 0;
          break;
      }
      return getSearchData(fromClause, orderbyClause, where, maxCount, overridewhere);

    }
  }
}
