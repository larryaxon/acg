USE [CityHostedProd]
GO

/****** Object:  View [sandbox].[vw_CityHostedMarginOCCUSOCSummary2]    Script Date: 7/2/2021 9:50:06 AM ******/
DROP VIEW [sandbox].[vw_CityHostedMarginOCCUSOCSummary2]
GO

/****** Object:  View [sandbox].[vw_CityHostedMarginOCCUSOCSummary2]    Script Date: 7/2/2021 9:50:06 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE view [sandbox].[vw_CityHostedMarginOCCUSOCSummary2] 
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
	dbo.isproratedocc(ServiceDescription) ProRated, 
	Credit,
	IIF(occ.OCCChargeType = 'Expedite', 'Yes', 'No') HasExpedite,
	IIF(ISDATE(RIght(ServiceDescription, 8)) = 1, 'Yes', 'No') ServiceEndsInDate,
	max(isnull(retailqty,0)) OCCQty, 
	max(isnull(RetailQty,0)) RetailQty,
	max(WHolesaleQty) WholesaleQty,
	max(dbo.GetProratedDays(ServiceDescription)) ProratedDays,
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
	dbo.isproratedocc(ServiceDescription),
	Credit,
	IIF(ServiceDescription like '%Expedite%', 'Yes', 'No'),
	IIF(ISDATE(RIght(ServiceDescription, 8)) = 1, 'Yes', 'No'),
	occ.OCCChargeType



GO


