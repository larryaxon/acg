USE [CityHostedProd]
GO

/****** Object:  View [sandbox].[vw_AnalysisMargin]    Script Date: 7/2/2021 9:46:43 AM ******/
DROP VIEW [sandbox].[vw_AnalysisMargin]
GO

/****** Object:  View [sandbox].[vw_AnalysisMargin]    Script Date: 7/2/2021 9:46:43 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [sandbox].[vw_AnalysisMargin] AS
SELECT
	dme.Dealer,
	dlr.LegalName DealerName,
	dme.SaddlebackID,
	dme.CustomerID,
	dme.CustomerName,
	dme.BillDate,
	dme.ChargeType,
	dme.RetailUSOC,
	mr.Name RetailUSOCDescription,
	mr.ItemSubCategory CHSCategory,
	case when isnull(pr.unmatched,0) = 1 and dme.retailusoc is not null
		then 'Yes'
		else 'No'
	end RetailOnly,
	case when getdate() between isnull(pr.startdate, '1/1/1900') and isnull(pr.enddate, '12/31/2100') and dme.retailusoc is not null
		then 'Yes'
		else 'No'
	end RetailActive,
	dme.WholesaleUSOC,
	mw.Name WholesaleUSOCDescription,
	case when isnull(pw.unmatched,0) = 1 and dme.wholesaleusoc is not null
		then 'Yes'
		else 'No'
	end WholesaleOnly,
	case when getdate() between isnull(pw.startdate, '1/1/1900') and isnull(pw.enddate, '12/31/2100') and dme.wholesaleusoc is not null
		then 'Yes'
		else 'No'
	end WholesaleActive,
	Convert(decimal(12, 2), dme.Retail) Retail,
	Convert(decimal(12, 2), dme.Wholesale) Wholesale,
	Convert(decimal(12, 0), dme.RetailQty) RetailQty,
	Convert(decimal(12, 0), dme.WholesaleQty) WholesaleQty,
	each.WholesaleEach,
	each.RetailEach,
	Convert(decimal(12, 2), dme.Net) GrossMargin,
	Convert(decimal(12, 2), dme.OCCQty) OCCQty,
	dme.ProRated,
	dme.ProratedDays,
	dme.Credit,
	--case
	--	when dme.chargetype = 'MRC' then ''
	--	when dme.ProRated = 'Yes' then 'ProRated'
	--	when dme.HasExpedite = 'Yes' then 'Expedited'
	--	when dme.ServiceEndsInDate = 'Yes' and dme.InstallCharge = each.WholesaleEach then 'Install' 
	--	else 'Other'
	--end OCCChargeType,
	dme.OCCChargeType,
	dme.InstallCharge InstallChargeEach,
	CONVERT(decimal(12, 2), dme.DealerCostEach) DealerCostEach,
	CONVERT
	(
		decimal(12, 2),
		(
			case 
				when dme.OCCQty = 0 then isnull(dme.RetailQty,0)
				when dme.Prorated = 'Yes' then Convert(decimal(10,4), Convert(decimal(10,4), dme.ProratedDays) / 30. * Convert(decimal(10,4), dme.OCCqty)) 
				else dme.OCCQty 
			end
			* dme.DealerCostEach  
			* IIF(dme.Retail < 0 OR dme.Wholesale < 0, -1, 1) -- adjust for credits
		)
	) DealerCostFromLookup,
	dme.Level,
	ISNULL(da.Value, 'None') MasterDealer,
	Coalesce(mdc.DealerCost, mdc2.DealerCost, 0) * IIF(dme.Retail < 0 OR dme.Wholesale < 0, -1, 1) MasterDealerCostEach,
	cust.EntityOwner MasterCustomer,
	coalesce(mcust.LegalName, 'CCI') MasterCustomerName,
	agt.SalesOrDealer AgentID,
	agte.LegalName AgentName,
	d2.Value Day2YN
FROM sandbox.vw_DealerMarginExtract dme
left join MasterProductList mr on dme.RetailUSOC = mr.ItemID
left join ProductList pr on dme.RetailUSOC = pr.ItemID and pr.Carrier = 'CityHosted'
left join MasterProductList mw on WholesaleUSOC = mw.ItemID
left join ProductList pw on dme.WholesaleUSOC = pw.ItemID and pw.Carrier = 'Saddleback'
left join Entity dlr on dlr.Entity = dme.Dealer
left join vw_AttributeNonXML da on da.Entity = dme.Dealer and da.ItemType = 'Entity' and da.Item = 'Dealer' and da.Name = 'MasterDealer'
left join vw_AttributeNonXML d2 on d2.Entity = dme.CustomerID and d2.ItemType = 'Entity' and d2.Item = 'Customer' and d2.Name = 'Day2YN'
left join sandbox.vw_DealerLevel lvl on lvl.Dealer = da.Value
left join sandbox.vw_ActiveHostedDealerCosts mdc on mdc.Dealer = da.Value and mdc.ItemID = dme.RetailUSOC
left join sandbox.vw_ActiveHostedDealerCosts mdc2 on mdc2.Dealer = lvl.Level and mdc2.ItemID = dme.RetailUSOC
left join Entity cust on dme.CustomerID = cust.Entity and cust.EntityType = 'Customer' 
left join Entity mcust on cust.EntityOwner = mcust.Entity and mcust.EntityType = 'MasterCustomer'
left join SalesOrDealerCustomers agt on agt.Customer = cust.Entity and agt.SalesType = 'Agent'
left join Entity agte on agt.SalesOrDealer = agte.Entity and agte.EntityType = 'Agent'
outer apply
	(
		select
			convert
			(
				decimal(12, 2),
				case
					when dme.WholesaleQty = 0 then 0
					else dme.Wholesale / dme.WholesaleQty
				end
			) WholesaleEach,
			convert
			(
				decimal(12, 2),
				case
					when dme.RetailQty = 0 then 0
					else dme.Retail / dme.RetailQty
				end
			) RetailEach
	) each
where
	dme.customername is not null
	and dme.customerid <> '20035' 
	and 'MANOCC' not in (isnull(dme.RetailUSOC, ''), isnull(dme.WholesaleUSOC, '')) 
	and not (ISNULL(dme.retailusoc, '') = '' and ISNULL(dme.wholesaleusoc, '') = '')


GO


