USE [CityHostedProd]
GO

/****** Object:  View [sandbox].[vw_CityHostedOCCSummary]    Script Date: 7/2/2021 9:50:47 AM ******/
DROP VIEW [sandbox].[vw_CityHostedOCCSummary]
GO

/****** Object:  View [sandbox].[vw_CityHostedOCCSummary]    Script Date: 7/2/2021 9:50:47 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
--select top 100 * from [sandbox].vw_CityHostedOCCSummary

CREATE view [sandbox].[vw_CityHostedOCCSummary]
AS
WITH
	Retail
	(
		CustomerID,
		CustomerName,
		RetailUSOC,
		RetailAmount,
		RetailQty,
		ProRated,
		BillDate,
		USOCWholesale,
		WholesaleUSOC,
		WholesaleAmount,
		WholesaleQty,
		WholesaleProrated,
		WholesaleBillDate,
		ServiceDescription,
		Credit,
		OCCChargeType
	)
	AS
	(
		select
			retail.CustomerID,
			retail.CustomerName,
			retail.RetailUSOC,
			RetailAmount,
			RetailQty,
			retail.ProRated,
			retail.BillDate,
			u.USOCWholesale,
			wholesale.WholesaleUSOC,
			wholesale.WholesaleAmount,
			WholesaleQty,
			wholesale.ProRated WholesaleProrated, 
			wholesale.BillDate WholesaleBillDate,
			retail.ServiceDescription,
			Credit,
			wholesale.OCCChargeType
		from
		(
			select
				h.CustomerID,
				e.LegalName CustomerName,
				RetailUSOC,
				SUM(RetailAmount) RetailAmount,
				Count(*) RetailQty, 
				--dbo.IsProratedOCC(RetailService) ProRated,
				Case when h.ChargeType = 'ProRated' then 'Yes' else 'No' End  ProRated,
				RetailBillDate BillDate,
				MAX(RetailService) ServiceDescription,
				IIF(RetailService like 'Credit%', 'Yes', 'No') Credit,
				h.ChargeType OCCChargeType
			from hostedmatchedocc h
			inner join entity e on h.CustomerID = e.entity
			where  RetailBillDate is not null and h.CustomerID like '2%'
			group by
				h.CustomerID,
				e.LegalName,
				RetailUSOC,
				RetailBillDate, 
				RetailAmount, -- Added by LLA 7-30-2015 to allow for the same item with different prices to be different lines
				Case when h.ChargeType = 'ProRated' then 'Yes' else 'No' End,
				IIF(RetailService like 'Credit%', 'Yes', 'No'),
				h.ChargeType
		) retail
		left join vw_CityHostedUSOCs u on retail.RetailUSOC = u.USOCRetail
		left join
		(
			select
				h.CustomerID, 
				WholesaleUSOC,
				SUM(WholesaleAmount) WholesaleAmount,
				Count(*) WholesaleQty, 
				Case when h.ChargeType = 'ProRated' then 'Yes' else 'No' End  ProRated,
				WholesaleBillDate BillDate,
				MAX(WholesaleService) ServiceDescription,
				h.ChargeType OCCChargeType
			from hostedmatchedocc h
			where WholesaleBillDate is not null 
			group by h.CustomerID, WholesaleUSOC, Case when h.ChargeType = 'ProRated' then 'Yes' else 'No' End, WholesaleBillDate, h.ChargeType
		) wholesale on retail.CustomerID = wholesale.CustomerID
						and u.USOCWholesale = wholesale.WholesaleUSOC
						and retail.ProRated = wholesale.ProRated
						and retail.BillDate = wholesale.BillDate
	),
	OrphanedWholesale
	(
		CustomerID,
		CustomerName,
		RetailUSOC,
		RetailAmount,
		RetailQty,
		ProRated,
		RetailBillDate,
		USOCWholesale,
		WholesaleUSOC,
		WholesaleAmount,
		WholesaleQty,
		WholesaleProRated,
		BillDate,
		ServiceDescription,
		Credit,
		OCCChargeType
	)
	AS
	(
		select
			wholesale.CustomerID,
			wholesale.CustomerName,
			null RetailUSOC,
			null RetailAmount,
			null RetailQty,
			null ProRated,
			null RetailBillDate,
			WholesaleUSOC USOCWholesale, 
			WholesaleUSOC,
			WholesaleAmount,
			WholesaleQty,
			wholesale.ProRated WholesaleProRated,
			BillDate,
			ServiceDescription,
			Credit,
			wholesale.OCCChargeType
		from
		(
			select
				h.CustomerID,
				e.LegalName CustomerName, 
				h.WholesaleUSOC,
				SUM(h.WholesaleAmount) WholesaleAmount,
				Count(*) WholesaleQty,
				--dbo.IsProratedOCC(h.WholesaleService) ProRated, 
				Case when h.ChargeType = 'ProRated' then 'Yes' else 'No' End  ProRated,
				WholesaleBillDate BillDate,
				Max(h.WholesaleService) ServiceDescription,
				IIF(WholesaleService like 'Credit%', 'Yes', 'No') Credit,
				h.ChargeType OCCChargeType
			from hostedmatchedocc h
			left join Entity e on h.CustomerID = e.Entity and e.EntityType = 'Customer'
			where h.Customerid like '2%' 
			group by
				h.CustomerID,
				e.LegalName,
				WholesaleUSOC,
				Case when h.ChargeType = 'ProRated' then 'Yes' else 'No' End,
				WholesaleBillDate,
				IIF(WholesaleService like 'Credit%', 'Yes', 'No'),
				h.ChargeType
		) wholesale
		left join
		(
			select distinct
				h.CustomerID,
				e.LegalName CustomerName,
				RetailUSOC,
				u.USOCWholesale,
				Case when h.ChargeType = 'ProRated' then 'Yes' else 'No' End  ProRated,
				h.ChargeType OCCChargeType
				--dbo.IsProratedOCC(RetailService) ProRated 
			from hostedmatchedocc h
			inner join entity e on h.CustomerID = e.entity
			left join vw_CityHostedUSOCs u on h.RetailUSOC = u.USOCRetail
			where RetailBillDate is not null --and RetailBillDate = '5/1/2013'
		) retail on wholesale.CustomerID = retail.CustomerID
					and wholesale.WholesaleUSOC = retail.UsocWholesale
					and wholesale.ProRated = retail.ProRated
		where  retail.CustomerID is null and wholesale.BillDate is not null 
	)
select
	CustomerID,
	CustomerName,
	Coalesce(BillDate, WholesaleBillDate) BillDate,
	RetailUSOC,
	RetailAmount,
	RetailQty,
	WholesaleUSOC,
	WholesaleAmount,
	WholesaleQty,
	coalesce(ProRated, WholesaleProRated) ProRated,
	Credit,  
	case
		when RetailUSOC IS null then 'WholesaleOnly' 
		when WholesaleUSOC IS null then 'RetailOnly'
		when ISNULL(RetailQty, 0) <> ISNULL(WholesaleQty, 0) then 'QtyMismatch'
		else 'Matched'
	end Exception,
	ServiceDescription,
	OCCChargeType
from
(
	SELECT * FROM Retail
	UNION
	SELECT * FROM OrphanedWholesale
) a


GO


