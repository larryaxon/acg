/************************************************************************************

Recreate views and procs with new Fluentstream Customer ID. 

Use old saddleback id if billdate is older than 5/1/2021

*************************************************************************************/



SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- Create view from import file that xrefs entity id
/****** Object:  View [dbo].[LLAFluentStreamCustomerIDs]    Script Date: 5/18/2021 10:59:55 AM ******/
DROP VIEW [dbo].[LLAFluentStreamCustomerIDs]
GO
--select * from [dbo].[LLAFluentStreamCustomerIDs]
Create View [dbo].[LLAFluentStreamCustomerIDs] as
select c.Entity, c.EntityOwner, c.LegalName, x.[Customer Name],c.AlternateName,c.AlternateID, x.[Account ID] FluentStreamID
from LLACustomerAccountDataExportWithNewIDs x
inner join Entity c on c.AlternateID = x.OldExternalID
where entitytype = 'Customer' and getdate() between isnull(startdate, '1/1/1900') and isnull(enddate, '12/31/2100')
GO


/***************************************************************************************************************

Update PROCS and VIEWS to use new table

****************************************************************************************************************/
/****** Object:  StoredProcedure [dbo].[CashReceiptsCustomerList]    Script Date: 5/17/2021 12:05:41 PM ******/
DROP PROCEDURE [dbo].[CashReceiptsCustomerList]
GO

--exec CashReceiptsCustomerList '5/1/2021', '5/30/2021'
CREATE proc [dbo].[CashReceiptsCustomerList] (@FromDate as date, @ToDate as date)
as
Declare @LastBillDate as date
set @LastBillDate = CONVERT(date, Convert(nvarchar(4), DatePart(year, @ToDate)) + '/' + CONVERT(nvarchar(2), datepart(month, @ToDate)) + '/1')
print @LastBillDate
select * from (select convert(bit, case when r.Amount is null then 0 else 1 end) Pay, -r.Amount PaidAmount,  
  case when r.NonSaddlebackAmount = 0 then null else -r.NonSaddlebackAmount end NonSaddlebackAmount, r.Reference CheckNumber, b.Amount [Current Bill], 
	s.Amount [Balance Owed],
	r.TransactionDate, e.LegalName CustomerName, coalesce(aid.ExternalID, e.AlternateID, '') SaddlebackID, s.customerid Customer,  
	Coalesce(r.TransactionSubType, a.Value, 'Check') PaymentType, 
	r.ExportDateTime PostedDate,  case when r.ExportDateTime is null then 'No' else 'Yes' end Exported,
	case when r.Amount is null then 'No' else 'Yes' end Paid, 
	case when r.customerid is null then 'Open' else 'Paid' end Status, 
	coalesce(s.BillDate, r.BillDate) BillDate, 
	coalesce(r.TransactionDate, s.billdate, r.billdate) EffectiveDate,
	r.TransactionDateTime, r.ID, r.LastModifiedBy, r.LastModifiedDateTime 
	from (Select CustomerID, Max(BillDate) BillDate, isnull(MAX(TransactionDate),'') TransactionDate, 
			SUM(amount) Amount from ARTransactions
			where TransactionDate <= @ToDate
			Group by CustomerID) s
	left join (Select CustomerID, BillDate, Max(TransactionDate) TransactionDate, sum(Amount) Amount, sum(NonSaddlebackAmount) NonSaddlebackAmount, max(Reference) Reference, max(TransactionSubType) TransactionSubType, 
				max(ExportDateTime) ExportDateTime, max(TransactionDateTime) TransactionDateTime, 
				max(LastModifiedBy) LastModifiedBy, max(LastModifiedDateTime) LastModifiedDateTime, max(ID) ID
				from ARTransactions where TransactionType = 'Cash'
				 and TransactionDate between @LastBillDate and @ToDate
				group by  CustomerID, BillDate) r
		on r.CustomerID = s.CustomerID and r.BillDate = s.BillDate
	left join (Select CustomerID, BillDate, TransactionDate, Amount, Reference, TransactionSubType 
				from ARTransactions 
				where TransactionType = 'Invoice'
				 and TransactionDate between @FromDate and @ToDate
				) b
		on b.CustomerID = s.CustomerID and b.BillDate = s.BillDate
	inner join Entity e on e.entity = s.customerid
	left join EntityAlternateIDs aid on e.Entity = aid.Entity and  coalesce(s.BillDate, r.BillDate) between aid.StartDate and aid.EndDate
	left join vw_attributenonxml a on a.Entity = s.CustomerID and a.ItemType = 'Entity' and a.Item = 'Customer' and a.Name = 'PaymentType') a 
GO

/****** Object:  View [dbo].[vw_CashReceipts]    Script Date: 5/17/2021 12:48:55 PM ******/
DROP VIEW [dbo].[vw_CashReceipts]
GO


-- select * from vw_CashReceipts where billdate = '4/1/2013' order by CustomerName
CREATE view [dbo].[vw_CashReceipts] as
select  * from (
select * from (select convert(bit, case when r.Amount is null then 0 else 1 end) Pay, -r.Amount PaidAmount,  
  case when r.NonSaddlebackAmount = 0 then null else -r.NonSaddlebackAmount end NonSaddlebackAmount, r.Reference CheckNumber, Convert(decimal(12,2),b.Amount) [Current Bill], 
	Convert(decimal(12,2),s.Amount) [Balance Owed],
	r.TransactionDate, e.LegalName CustomerName, coalesce(aid.ExternalID, e.AlternateID, '') SaddlebackID, s.customerid Customer,  
	Coalesce(r.TransactionSubType, a.Value, 'Check') PaymentType, 
	r.ExportDateTime PostedDate,  case when r.ExportDateTime is null then 'No' else 'Yes' end Exported,
	case when r.Amount is null then 'No' else 'Yes' end Paid, 
	case when r.customerid is null then 'Open' else 'Paid' end Status, 
	coalesce(s.BillDate, r.BillDate) BillDate, 
	coalesce(r.TransactionDate, s.billdate, r.billdate) EffectiveDate,
	r.TransactionDateTime, r.ID, r.LastModifiedBy, r.LastModifiedDateTime 
	from (Select CustomerID, Max(BillDate) BillDate, isnull(MAX(TransactionDate),'') TransactionDate, 
			SUM(amount) Amount from ARTransactions
			where TransactionDate <= GETDATE()
			Group by CustomerID) s
	left join (Select CustomerID, BillDate, Max(TransactionDate) TransactionDate, sum(Amount) Amount, sum(NonSaddlebackAmount) NonSaddlebackAmount, max(Reference) Reference, max(TransactionSubType) TransactionSubType, 
				max(ExportDateTime) ExportDateTime, max(TransactionDateTime) TransactionDateTime, 
				max(LastModifiedBy) LastModifiedBy, max(LastModifiedDateTime) LastModifiedDateTime, max(ID) ID
				from ARTransactions where TransactionType = 'Cash'
				 and TransactionDate between CONVERT(date, Convert(nvarchar(4), DatePart(year, GETDATE())) + '/' + CONVERT(nvarchar(2), datepart(month, GETDATE())) + '/1') and GETDATE()
				group by  CustomerID, BillDate) r
		on r.CustomerID = s.CustomerID and r.BillDate = s.BillDate
	left join (Select CustomerID, BillDate, TransactionDate, Amount, Reference, TransactionSubType 
				from ARTransactions 
				where TransactionType = 'Invoice'
				 and BillDate = CONVERT(date, Convert(nvarchar(4), DatePart(year, GETDATE())) + '/' + CONVERT(nvarchar(2), datepart(month, GETDATE())) + '/1')
				) b
		on b.CustomerID = s.CustomerID and b.BillDate = s.BillDate
	inner join Entity e on e.entity = s.customerid
	left join EntityAlternateIDs aid on e.entity = aid.entity and coalesce(s.BillDate, r.BillDate)  between aid.StartDate and aid.EndDate
	left join vw_attributenonxml a on a.Entity = s.CustomerID and a.ItemType = 'Entity' and a.Item = 'Customer' and a.Name = 'PaymentType') a 

) charges
--select * from artransactions where CustomerID = '22079' and billdate = '4/1/2013'

GO


/****** Object:  View [dbo].[vw_CityHostedAccountsReceivableReport]    Script Date: 5/17/2021 12:54:55 PM ******/
DROP VIEW [dbo].[vw_CityHostedAccountsReceivableReport]
GO


/****** Script for SelectTopNRows command from SSMS  ******/
/*
select * from vw_CityHostedAccountsReceivableReport 
where PeriodBeginDate between '11/1/2012' and '4/1/2013' --and abs(Diff) > .09 
order by CustomerName, TransactionDate
*/
CREATE view [dbo].[vw_CityHostedAccountsReceivableReport] as
SELECT 
CASE
	WHEN aid.ExternalServiceName = 'Saddleback' then 'NWHS-' + right(coalesce(aid.ExternalID, e.AlternateID, ''),3) else aid.ExternalID END CustomerID, 
a.CustomerID CCICustomerID, e.LegalName CustomerName,
	a.TransactionType, a.BillDate PeriodBeginDate, a.TransactionDate, a.Amount, isnull(a.NonSaddlebackAmount, 0) NonSaddlebackAmount, 
	isnull(a.Reference, '') Reference, 
	s.MRC, s.OCC, s.toll, round(a.Amount - s.MRC - s.OCC - s.toll, 2) Tax--, isnull(round(a.Amount - s.InvoiceTotal, 2),0) Diff
  FROM ARTransactions a
  left join Entity e on a.CustomerID = e.Entity
  left join EntityAlternateIDs aid on e.Entity = aid.Entity and a.BillDate Between aid.StartDate and aid.EndDate
  left join vw_CityHostedInvoiceSummary s on a.CustomerID = s.CustomerID and a.BillDate = s.BillDate and a.TransactionType = 'Invoice'
  
  --select * from Entity where LegalName like '%OSIO%' and LegalName not like '%flagstaff%'
  
  --select * from HostedImportMRCRetail where CustomerNumber = '00000656' and BillDate = '4/1/2013'
  --select * from hostedmatchedmrc where alternateid = '00000656' and RetailBillDate = '4/1/2013'
  --select * from Entity where Entity = '21822'
  --update hostedmatchedmrc set CustomerID = '23004' where CustomerID = '21822'
  --select * from hostedmatchedmrc where CustomerID = '21822'
  
  --select * from hostedmatchedmrc where AlternateID in ( '0000071', '00000462' )
  
  --select * from HostedImportMRCRetail where CustomerNumber in ( '0000071', '00000462' )
 
GO

/****** Object:  View [dbo].[vw_CityHostedMarginCustomerList]    Script Date: 5/17/2021 1:10:02 PM ******/
DROP VIEW [dbo].[vw_CityHostedMarginCustomerList]
GO

--Query 1: Customer List
CREATE view [dbo].[vw_CityHostedMarginCustomerList] as
select distinct mrc.customerid CustomerID, c.legalname CustomerName, coalesce(aid.ExternalID, c.AlternateID) SaddlebackID from hostedmatchedmrc mrc
left join entity c on mrc.customerid = c.entity
left join EntityAlternateIDs aid on c.entity = aid.entity and coalesce(mrc.RetailBillDate, mrc.wholesalebilldate) between aid.StartDate and aid.enddate

GO

/****** Object:  View [dbo].[vw_CityHostedMarginMRCUSOCSummary]    Script Date: 5/17/2021 1:18:54 PM ******/
DROP VIEW [dbo].[vw_CityHostedMarginMRCUSOCSummary]
GO

--select top 100 * from [dbo].[vw_CityHostedMarginMRCUSOCSummary]  where retailbilldate is null
CREATE view [dbo].[vw_CityHostedMarginMRCUSOCSummary] 
as
select mrc.BillDate WholesaleBillDate, mrc.BillDate RetailBillDate, CustomerID, c.Legalname CustomerName, 
CASE WHEN aid.ExternalServiceName = 'Saddleback' then 'NWHS-' + Convert(nvarchar(12), Convert(int, c.alternateid))
     ELSE aid.ExternalID
END
	Account,
RetailUSOC, r.Name RetailUSOCName, WholesaleUSOC, w.Name WholesaleUSOCName, 
dlr.SalesOrDealer Dealer, de.legalname DealerName, x.value DealerLevel, coalesce(dc.DealerCost, dc2.DealerCost, 0) DealerCost,
sum(isnull(WholesaleQty,0)) WholesaleQTY, 
sum(round(isnull(wholesaleamount,0),2)) WholesaleAmount, 
sum(isnull(RetailQty,0)) RetailQty, 
sum(round(isnull(retailamount,0),2)) RetailAmount, 
sum(round(isnull(retailamount,0),2)) - sum(round(isnull(wholesaleamount,0),2)) GrossMargin,
sum(isnull(WholesaleQTY, 0) * coalesce(dc.DealerCost, dc2.DealerCost, 0)) DealerCostTotal,
sum(isnull(WholesaleQTY, 0) * coalesce(dc.DealerCost, dc2.DealerCost, 0)) - sum(round(isnull(wholesaleamount,0),2)) Net,
sum(round(isnull(retailamount,0),2)) - sum(isnull(WholesaleQTY, 0) * coalesce(dc.DealerCost, dc2.DealerCost, 0)) DealerMargin
 from vw_CityHostedUSOCByCustomerSummary mrc
left join dbo.entity c on mrc.customerid = c.entity
left join dbo.masterproductlist w on mrc.wholesaleusoc = w.itemid
left join dbo.masterproductlist r on mrc.retailusoc = r.itemid
left join dbo.SalesOrDealerCustomers dlr on dlr.Customer = mrc.CustomerID and dlr.SalesType = 'Dealer'
left join dbo.vw_attributenonxml x on x.entity = dlr.SalesOrDealer and x.itemtype = 'entity' and x.item = 'ibp' and x.Name = 'DealerLevel'
left join dbo.HostedDealerCosts dc on dc.Dealer = dlr.SalesOrDealer and dc.ItemID = WholesaleUSOC
left join dbo.HostedDealerCosts dc2 on dc2.Dealer = x.value and dc2.ItemID = WholesaleUSOC
left join entity de on de.entity = dlr.salesordealer
left join EntityAlternateIDs aid on c.Entity = aid.Entity and mrc.BillDate between aid.StartDate and aid.EndDate
group by mrc.BillDate, mrc.BillDate, customerid, c.Legalname , RetailUsoc, r.Name, WholesaleUSOC, w.Name, 
dlr.SalesOrDealer, de.legalname, x.value, dc.DealerCost, dc2.DealerCost, CASE WHEN aid.ExternalServiceName = 'Saddleback' then 'NWHS-' + Convert(nvarchar(12), Convert(int, c.alternateid))
     ELSE aid.ExternalID
END

GO


/****** Object:  View [dbo].[vw_CityHostedMarginTaxSummary]    Script Date: 5/17/2021 1:31:32 PM ******/
DROP VIEW [dbo].[vw_CityHostedMarginTaxSummary]
GO

--Query 6: Tax by customer

CREATE view [dbo].[vw_CityHostedMarginTaxSummary] 
 
as
select dlr.SalesOrDealer Dealer, coalesce(c.entity, btn.customer) CustomerID, coalesce(c.legalname, c2.legalname) CustomerName, 
tax.billdate RetailBillDate, 
'Tax' ChargeType,
--sum(case when tax.sign = '+' then tax.taxamount else -tax.taxamount end) 
sum(taxamount) Wholesale, 
--sum(case when tax.sign = '+' then tax.taxamount else -tax.taxamount end) 
sum(taxamount) DealerCost, 
--sum(case when tax.sign = '+' then tax.taxamount else -tax.taxamount end) 
sum(taxamount) Retail, 
0 Net, 0 DealerMargin
from dbo.hostedtaxtransactions tax
LEFT JOIN DBO.EntityAlternateIDs aid on tax.customer = aid.ExternalID and tax.billdate between aid.StartDate and aid.EndDate
left join dbo.entity c on c.entity = aid.entity --c.entitytype = 'customer' and c.alternateid = tax.customer
left join dbo.hostedbtncustomer btn on tax.masterbtn = btn.btn
left join dbo.entity c2 on c2.entitytype = 'customer' and c2.entity = btn.customer
left join dbo.SalesOrDealerCustomers dlr on dlr.Customer =  coalesce(c.entity, btn.customer) and dlr.SalesType = 'Dealer'
group by dlr.SalesOrDealer, coalesce(c.entity, btn.customer), coalesce(c.legalname, c2.legalname), tax.billdate



GO

/****** Object:  View [dbo].[vw_DealerMarginExtract]    Script Date: 5/17/2021 1:47:15 PM ******/
DROP VIEW [dbo].[vw_DealerMarginExtract]
GO


--select * from vw_DealerMarginExtract where 'WNIPLD' in (retailusoc, wholesaleusoc) and chargetype = 'occ' and Dealer = 'RODENINTEGRATION'
-- select * from vw_aranalysis where BillDate = '5/1/2013' and CustomerID = '21025'
--select * from artransactions where CustomerID = '21025' order by transactiondate desc
--select sum(Amount) from artransactions where CustomerID = '21025' order by transactiondate desc
--select * from vw_analysismargin where chargetype = 'occ' and Dealer <> 'CCIDealer' and dealercostfromlookup is null
-- select * from vw_analysismargin where occchargetype = 'other' and installchargeeach is not null and Dealer <> 'CCIDealer'
--select * from [vw_AnalysisMargin2] where billdate = '5/1/2013' and customerid = '21243'
--select * from hostedimportoccwholesale where service not like '% from %' and service like '%expedite%' order by service

-- if not prorated and ends in a date and amoiunt = install charge 
-- type = prorate, install, expedite, other
-- net margin, seems wrong
--select * from hosteddealercosts where dealer = 'SethScardefield'
--select * from vw_CityHostedUSOCs where USOCwholesale = 'WDPCE'
--update HostedDealerCosts set EndDate = '6/30/2014' where Dealer = 'SethScardefield' and ItemID = 'gold'
/*
select CustomerName, BilLDate, RetailUSOC, WholesaleUSOC, RetailOnly, WholesaleOnly, Retail, Wholesale, RetailQty, WholesaleQty, DealerCostEach, DealerCostFromLookup, DealerCostApplied, DealerMargin from vw_AnalysisMargin where chargetype = 'mrc' --and OCCChargeType in ('other') and installchargeeach < 0
select * from hostedimportoccwholesale where usoc = 'WDIDN' and billdate = '5/1/2013'
select * from hosteddealercosts where itemid = 'WDIDN'
*/
-- select * from [vw_DealerMarginExtract] where Dealer like 'seth%'
--select distinct customernumber, customername from hostedimportmrcretail order by customernumber

CREATE view [dbo].[vw_DealerMarginExtract] as
select coalesce(Dlr.SalesOrDealer, 'CCIDealer') Dealer, coalesce(aid.ExternalID, e.alternateid) SaddlebackID, CustomerID, CustomerName, BillDate, ChargeType, RetailUSOC, WholesaleUSOC, 
round(Retail, 2) Retail, Round(Wholesale, 2) Wholesale, RetailQty, WholesaleQty, 
coalesce(dc.DealerCost, dc2.DealerCost, 0) * case when Retail < 0 or Wholesale < 0 then -1 else 1 end DealerCostEach,
a.WholesaleNRC InstallCharge,
a.NRC RetailInstallCharge,
Retail - Wholesale Net, 
OCCQty, ProRated,
ProratedDays, Credit, HasExpedite, ServiceEndsInDate, isnull(lvl.Level, 'None') Level
from (
select Coalesce(retailbilldate, wholesalebilldate) BillDate, CustomerID, CustomerName, CONVERT(int,substring(account, 6,3)) Account, 'MRC' ChargeType,
RetailUSOC, WholesaleUSOC, RetailAmount Retail, WholesaleAmount Wholesale,  RetailQty, WholesaleQty, 0 OCCQty,
null Prorated, null ProratedDays, null Credit, null HasExpedite, null ServiceEndsInDate, null NRC, null WholesaleNRC from vw_CityHostedMarginMRCUSOCSummary
union
select coalesce(retailbilldate, wholesalebilldate) BillDate, CustomerID, CustomerName, 0 account, 'OCC' ChargeType,
RetailUSOC, WholesaleUSOC, Retail, WHolesale,  RetailQty,  WholesaleQty, OCCQty Quantity, Prorated Prorated, ProratedDays, Credit, HasExpedite, ServiceEndsInDate,
NRC, WholesaleNRC
  from vw_CityHostedMarginOCCUSOCSummary2 ) a
left join Entity e on a.customerid = e.entity
left join EntityAlternateIDs aid on e.Entity = aid.Entity and a.BillDate between aid.StartDate and aid.EndDate
left join ProductList p on p.ItemID = a.RetailUSOC and p.Carrier = 'CityHosted'
left join dbo.SalesOrDealerCustomers dlr on dlr.Customer = a.CustomerID
left join (select Dealer, ItemID Level from HostedDealerCosts  where ItemID in (select Distinct CodeValue from CodeMaster where CodeType = 'DealerPricingLevels') and GETDATE() between ISNULL(StartDate, '1/1/1900') and ISNULL(enddate, '12/31/2100')) lvl on lvl.Dealer = dlr.SalesOrDealer
--left join dbo.vw_attributenonxml x on x.entity = dlr.SalesOrDealer and x.itemtype = 'entity' and x.item = 'ibp' and x.Name = 'DealerLevel'
left join (
	Select usocwholesale, MAX(usocretail) USOCRetail 
	from vw_CityHostedUSOCs 
	where USOCWholesale is not null and WholesaleOnly = 'No' and RetailOnly = 'No' group by USOCWholesale
	) usoc on a.WholesaleUSOC = usoc.USOCWholesale
left join dbo.HostedDealerCosts dc on dc.Dealer = dlr.SalesOrDealer and dc.ItemID = coalesce(a.RetailUSOC, usoc.USOCRetail) and convert(date, getdate()) between isnull(dc.startdate, '1/1/1900') and isnull(dc.enddate, '12/31/2100')
left join dbo.HostedDealerCosts dc2 on dc2.Dealer = lvl.Level and dc2.ItemID = coalesce(a.RetailUSOC, usoc.USOCRetail) and convert(date, getdate()) between isnull(dc2.startdate, '1/1/1900') and isnull(dc2.enddate, '12/31/2100')
where CustomerName is not null 

GO

/****** Object:  View [dbo].[vw_SaddlebackImportOCCRaw]    Script Date: 5/17/2021 2:37:30 PM ******/
DROP VIEW [dbo].[vw_SaddlebackImportOCCRaw]
GO

-- select distinct customer from hostedimportoccwholesale where masterbtn  in ( '5000000014', '6023436288')
--select * from hostedimportoccwholesale where btn in ('4802787300', '4803979127', '4803979128', '4806997685', '4806998410', '4806099012')
--select * from vw_SaddlebackImportOCCRaw where service = 'Charge for I-line Managed IP Lines from 04/01/13 to 04/30/13'
-- select * from vw_SaddlebackImportOCCRaw where btn in ('4802787300', '4803979127', '4803979128', '4806997685', '4806998410', '4806099012')
-- select * from vw_SaddlebackImportOCCRaw where customername is null and masterbtn  in ( '5000000014', '6023436288')
CREATE view [dbo].[vw_SaddlebackImportOCCRaw] as
select 'Wholesale' Source, coalesce(mrc.customerid, e.Entity, emap.entity) CustomerID, btn.Customer SaddlebackID, 
coalesce(mrc.LegalName, e.LegalName, emap.legalname) CustomerName, w.MasterBTN,
w.BTN, w.Date TransactionDate, w.BillDate, w.Amount, w.OCCCode, w.USOC, w.Service from HostedImportOCCWholesale w
left join (select distinct customer, btn from HostedImportOCCRetail) btn on w.btn = btn.btn
left join EntityAlternateIDs aid on btn.Customer = aid.ExternalID and w.BillDate between aid.StartDate and aid.EndDate
left join Entity e on e.Entity = aid.Entity-- e.AlternateID = btn.customer
left join ExternalIDMapping map on map.externalid = w.customer and map.entitytype = 'customer'
left join Entity emap on emap.Entity = map.internalid and emap.entitytype = 'customer'
left join (select distinct coalesce(OriginalWholesaleBTN, OriginalRetailBTN, RetailBTN) btn, m.CustomerID, e.legalname 
	from hostedmatchedmrc m 
	left join Entity e on e.entity = m.customerid
	where e.entity is not null --and m.CustomerID not in ('24173','20035')
	) mrc on mrc.btn = w.btn
Union
select 'Retail' Source, e.Entity CustomerID, r.Customer SaddlebackID, e.LegalName CustomerName, r.MasterBTN,
r.BTN, r.Date TransactionDate, r.BillDate, r.Amount, r.Code, r.USOC, r.Service 
from HostedImportOCCRetail r
left join EntityAlternateIDs aid on r.Customer = aid.ExternalID and r.Billdate between aid.StartDate and aid.EndDate
left join Entity e on e.entity = aid.Entity--e.AlternateID = r.Customer and e.EntityType = 'customer'

GO

DROP VIEW [dbo].[vw_TaxDetailHistory]
GO

/****** Object:  View [dbo].[vw_TaxDetailHistory]    Script Date: 5/17/2021 2:42:59 PM ******/

CREATE VIEW [dbo].[vw_TaxDetailHistory]
AS
SELECT  c.Entity CustomerID, c.LegalName CustomerName, t.Customer SaddlebackID, t.masterbtn, 
        t.LEVEL, t.TaxType, t.leveltype, t.Jurisdiction, t.title, t.TaxAmount, t.billdate
FROM         Hostedtaxtransactions t 
	LEFT JOIN EntityAlternateIDs aid on t.Customer = aid.ExternalID and t.BillDate between aid.StartDate and aid.EndDate
	LEFT JOIN
                      Entity c ON  c.Entity = aid.Entity--t.Customer = c.AlternateID
GO

/**********************************************************************************************************

Data Sources

***********************************************************************************************************/

UPDATE DataSources
Set FromClause =
' select * from 
 (   
	select convert(bit, case when r.Amount is null then 0 else 1 end) Pay, 
			Convert(decimal(12,2), -r.Amount) PaidAmount,      

			case when r.NonSaddlebackAmount = 0 then null else Convert(decimal(12,2),-r.NonSaddlebackAmount) end NonSaddlebackAmount,      
			r.Reference CheckNumber, Convert(decimal(12,2),b.Amount) [Current Bill], Convert(decimal(12,2),s.Amount) [Balance Owed],     
			r.TransactionDate, e.LegalName CustomerName, coalesce(aid.ExternalID, e.AlternateID, '''') SaddlebackID, s.customerid Customer,      
			Coalesce(a.Value, ''Check'') PaymentType, r.ExportDateTime PostedDate,      
			case when r.ExportDateTime is null then ''No'' else ''Yes'' end Exported,        
			case when r.Amount is null then ''No'' else ''Yes'' end Paid,      
			case when r.customerid is null then ''Open'' else ''Paid'' end Status,     
			coalesce(s.BillDate, r.BillDate) BillDate, coalesce(r.TransactionDate, s.billdate, r.billdate) EffectiveDate,        
			r.TransactionDateTime, r.ID, r.LastModifiedBy, r.LastModifiedDateTime, null Comment,     
			isnull(ai.HasActiveInventory, ''No'') HasActiveInventory    
	from (    Select CustomerID, Max(BillDate) BillDate, isnull(MAX(TransactionDate),'''') TransactionDate,      SUM(amount) - SUM(Isnull(NonSaddlebackAmount, 0)) Amount     
				from ARTransactions     where TransactionDate <= ''{1}''     Group by CustomerID    ) s     
	left join (    Select CustomerID, BillDate, Max(TransactionDate) TransactionDate,      sum(Amount)  - SUM(Isnull(NonSaddlebackAmount, 0)) Amount,      
				sum(NonSaddlebackAmount) NonSaddlebackAmount, max(Reference) Reference, max(TransactionSubType) TransactionSubType,     
				max(ExportDateTime) ExportDateTime, max(TransactionDateTime) TransactionDateTime,  max(LastModifiedBy) LastModifiedBy,      
				max(LastModifiedDateTime) LastModifiedDateTime, max(ID) ID      
				from ARTransactions     where TransactionType = ''Cash'' and TransactionDate between ''{0}'' and ''{1}''     group by  CustomerID, BillDate   ) r    on r.CustomerID = s.CustomerID and r.BillDate = s.BillDate   
	left join (    Select CustomerID, BillDate, TransactionDate, Amount, Reference, TransactionSubType     
					from ARTransactions           where TransactionType = ''Invoice'' and TransactionDate between ''{2}'' and ''{1}''   ) b on b.CustomerID = s.CustomerID and b.BillDate = s.BillDate      
	inner join Entity e on e.entity = s.customerid  
	left join EntityAlternateIDs aid on e.entity = aid.entity and b.BillDate between aid.StartDate and aid.EndDate
	left join vw_attributenonxml a on a.Entity = s.CustomerID and a.ItemType = ''Entity'' and a.Item = ''Customer'' and a.Name = ''PaymentType''   
	left join (Select Customer, case when count(*) > 0 then ''Yes'' else ''No'' end HasActiveInventory from networkinventory      
	where getdate() between isnull(startDate, ''1/1/1900'')  and isnull(enddate, ''12/31/2100'')     
 Group by Customer) ai on s.customerid = ai.Customer  ) a   '
 WHERE DataSource = 'CashReceipts'

 Update DataSources
 SET FromClause = 'select a.ID,a.CustomerID,coalesce(aid.ExternalID, e.alternateid) as AlternateID,e.legalname as CustomerName,a.billdate as BillDate, a.TransactionType,a.TransactionDate,a.Amount,a.Reference,a.Comment,a.lastmodifiedby as CreatedBy, ''Yes'' as CreditMemo    from 
artransactions a 
inner join         entity e on a.customerid = e.entity and e.entitytype = ''customer''    
left join EntityAlternateIDs aid on e.entity = aid.Entity and a.BillDate between aid.StartDate and aid.EndDate
where a.transactiontype = ''Credit'''
WHERE DataSource = 'CityHostedCreditMemos'

 Update DataSources
 SET FromClause = 'SELECT * from (
 select c.Entity CustomerID, Coalesce(c.Legalname, m.Customer) CustomerName, BillDate, MasterBTN BTN, Title,   Convert(decimal(18,2), case when Sign = '+' then TaxAmount else -TaxAmount end) Tax  
 from hostedtaxtransactions m 
 left join EntityAlternateIDs aid on m.customer = aid.ExternalID and m.BillDate between aid.StartDate and aid.EndDate
 left join entity c on aid.entity = c.entity	
 ) d'
WHERE DataSource = 'CityHostedDetailTax'

 Update DataSources
 SET FromClause = 'SELECT * from 
(
	select c.Entity CustomerID, c.LegalName CustomerName, 
	Convert(date, Convert(varchar(2), WholesaleBillingMonth) + ''/01/'' +   Convert(varchar(4), WholesaleBillingYear)) BillDate, 
	WholesaleBTN BTN,  FromNumber, ToNumber, WholesaleCount, WholesaleCharge   
	from hostedmatchedtoll m 
	left join EntityAlternateIDs aid on aid.ExternalID = m.AlternateID and 
		Convert(date, Convert(varchar(2), WholesaleBillingMonth) + ''/01/'' +   Convert(varchar(4), WholesaleBillingYear)) between aid.StartDate and aid.EndDate
	left join entity c on c.Entity = aid.entity
) d 
WHERE BillDate is not null
order by billdate desc'
WHERE DataSource = 'CityHostedDetailWholesaleToll'

 Update DataSources
 SET FromClause = ' SELECT * from (
 select ID, Coalesce(dlr.SalesOrDealer, ''CCIDealer'') Dealer, occ.CustomerID, e.AlternateID SaddlebackID,       
 e.LegalName CustomerName, occ.RetailBillDate BillDate, occ.RetailUSOC, occ.RetailAmount,       
 occ.WholesaleUSOC, occ.WholesaleAmount, Coalesce(occ.RetailService, occ.WholesaleService) Description,       
 case when occ.MatchedBy = ''ManualAdjustment'' then ''Yes'' else ''No'' end ManualAdjustment      
 from hostedmatchedocc occ      
 inner join Entity e on e.Entity = occ.CustomerID  
 Left join EntityAlternateIDs aid on aid.Entity = e.Entity and Coalesce(occ.RetailBillDate, occ.WholesaleBillDate) between aid.StartDate and aid.EndDate
 left join SalesOrDealerCustomers dlr on occ.CustomerID = dlr.Customer and SalesType = ''Dealer''
 ) a'
WHERE DataSource = 'CityHostedOCCEntry'

