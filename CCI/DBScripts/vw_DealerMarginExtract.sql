USE [CityHostedProd]
GO

/****** Object:  View [sandbox].[vw_DealerMarginExtract]    Script Date: 7/2/2021 9:49:10 AM ******/
DROP VIEW [sandbox].[vw_DealerMarginExtract]
GO

/****** Object:  View [sandbox].[vw_DealerMarginExtract]    Script Date: 7/2/2021 9:49:10 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- select top 100 * from [sandbox].[vw_DealerMarginExtract]


CREATE VIEW [sandbox].[vw_DealerMarginExtract]
as
WITH
	CombinedSummary
	(
		BillDate,
		CustomerID,
		CustomerName,
		Accouunt,
		ChargeType,
		RetailUSOC,
		WholesaleUSOC,
		Retail,
		Wholesale,
		RetailQty,
		WholesaleQty,
		OCCQty,
		Prorated,
		ProratedDays,
		Credit,
		HasExpedite,
		ServiceEndsInDate,
		NRC,
		WholesaleNRC,
		OCCChargeType
	)
	AS
	(
		select
			Coalesce(retailbilldate, wholesalebilldate) BillDate,
			CustomerID,
			CustomerName,
			CONVERT(int,substring(account, 6,3)) Account,
			'MRC' ChargeType,
			RetailUSOC,
			WholesaleUSOC,
			RetailAmount Retail,
			WholesaleAmount Wholesale,
			RetailQty,
			WholesaleQty,
			0 OCCQty,
			null Prorated,
			null ProratedDays,
			null Credit,
			null HasExpedite,
			null ServiceEndsInDate,
			null NRC,
			null WholesaleNRC,
			null OCCChargeType
		from sandbox.vw_CityHostedMarginMRCUSOCSummary
		union
		select
			coalesce(retailbilldate, wholesalebilldate) BillDate,
			CustomerID,
			CustomerName,
			0 account,
			'OCC' ChargeType,
			RetailUSOC,
			WholesaleUSOC,
			Retail,
			Wholesale,
			RetailQty,
			WholesaleQty,
			OCCQty Quantity,
			Prorated Prorated,
			ProratedDays,
			Credit,
			HasExpedite,
			ServiceEndsInDate,
			NRC,
			WholesaleNRC,
			OCCChargeType
		from sandbox.vw_CityHostedMarginOCCUSOCSummary2
	),
	USOC (USOCWholesale, USOCRetail)
	AS
	(
		Select usocwholesale, MAX(usocretail) USOCRetail 
		from vw_CityHostedUSOCs 
		where USOCWholesale is not null and WholesaleOnly = 'No' and RetailOnly = 'No' group by USOCWholesale
	)
select
	coalesce(Dlr.SalesOrDealer, 'CCIDealer') Dealer,
	e.alternateid SaddlebackID,
	CustomerID,
	CustomerName,
	BillDate,
	ChargeType,
	RetailUSOC,
	WholesaleUSOC, 
	round(Retail, 2) Retail,
	Round(Wholesale, 2) Wholesale,
	RetailQty,
	WholesaleQty, 
	coalesce(dc.DealerCost, dc2.DealerCost, 0) * IIF(Retail < 0 OR Wholesale < 0, -1, 1) DealerCostEach,
	a.WholesaleNRC InstallCharge,
	a.NRC RetailInstallCharge,
	Retail - Wholesale Net, 
	OCCQty,
	ProRated,
	ProratedDays,
	Credit,
	HasExpedite,
	ServiceEndsInDate,
	isnull(lvl.Level, 'None') Level,
	OCCChargeType
from CombinedSummary a
left join Entity e on a.customerid = e.entity
left join ProductList p on p.ItemID = a.RetailUSOC and p.Carrier = 'CityHosted'
left join dbo.SalesOrDealerCustomers dlr on dlr.Customer = a.CustomerID and dlr.SalesType = 'Dealer'
left join sandbox.vw_DealerLevel lvl on lvl.Dealer = dlr.SalesOrDealer
left join USOC on a.WholesaleUSOC = usoc.USOCWholesale
left join sandbox.vw_ActiveHostedDealerCosts dc on dc.Dealer = dlr.SalesOrDealer and dc.ItemID = coalesce(a.RetailUSOC, usoc.USOCRetail)
left join sandbox.vw_ActiveHostedDealerCosts dc2 on dc2.Dealer = lvl.Level and dc2.ItemID = coalesce(a.RetailUSOC, usoc.USOCRetail)
where CustomerName is not null 




GO


