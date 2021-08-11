USE [CityHostedProd]
GO

/****** Object:  View [sandbox].[vw_CityHostedMarginOCCUSOCSummary2]    Script Date: 7/23/2021 8:10:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





ALTER view [sandbox].[vw_CityHostedMarginOCCUSOCSummary2] 
as 
select
	dlr.SalesOrDealer Dealer,
	occ.CustomerID,
	c.LegalName CustomerName,
	RetailUSOC,
	WHolesaleUSOC,
	occ.BillDate BillDate,
	occ.BillDate RetailBillDate,
	occ.BillDate wholesalebilldate,
	'OCC' ChargeType,
	Sum(isnull(occ.WholesaleAmount,0)) Wholesale, 
	Sum(isnull(occ.RetailAmount,0)) Retail,
	Sum(isnull(occ.RetailAmount,0)) - Sum(isnull(occ.WholesaleAmount,0)) Net,
	0 DealerMargin,
	occ.Prorated ProRated, 
	Credit,
	IIF(occ.OCCChargeType = 'Expedite', 'Yes', 'No') HasExpedite,
	IIF(ISDATE(RIght(ServiceDescription, 8)) = 1, 'Yes', 'No') ServiceEndsInDate,
	sum(isnull(retailqty,0)) OCCQty, 
	sum(isnull(RetailQty,0)) RetailQty,
	sum(WHolesaleQty) WholesaleQty,
	--max(case when occ.ProRated = 'Yes' then 30 - Day(occ.RetailDate) else 0 end) ProratedDays,
	case when occ.ProRated = 'Yes' then 30 - Day(occ.RetailDate) else 0 end ProratedDays,
	pr.NRC NRC,
	pw.NRC WholesaleNRC,
	occ.OCCChargeType
from sandbox.vw_CityHostedOCCSummary occ
left join dbo.entity c on occ.customerid = c.entity
left join dbo.SalesOrDealerCustomers dlr on dlr.Customer = occ.CustomerID and dlr.SalesType = 'Dealer'
left join ProductList pw on pw.ItemID = occ.WholesaleUSOC and pw.Carrier = 'saddleback'
left join ProductList pr on pr.ItemID = occ.RetailUSOC and pr.Carrier = 'cityhosted'
group by
	dlr.SalesOrDealer,
	occ.CustomerID,
	c.LegalName,
	RetailUSOC,
	WHolesaleUSOC,
	pr.NRC,
	pw.NRC,
	occ.BillDate,
	occ.Prorated,
	Credit,
	IIF(ServiceDescription like '%Expedite%', 'Yes', 'No'),
	IIF(ISDATE(RIght(ServiceDescription, 8)) = 1, 'Yes', 'No'),
	occ.OCCChargeType,
	case when occ.ProRated = 'Yes' then 30 - Day(occ.RetailDate) else 0 end





GO


