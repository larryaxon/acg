USE [CityHostedProd]
GO

/****** Object:  View [sandbox].[vw_DealerMargin]    Script Date: 7/2/2021 9:45:26 AM ******/
DROP VIEW [sandbox].[vw_DealerMargin]
GO

/****** Object:  View [sandbox].[vw_DealerMargin]    Script Date: 7/2/2021 9:45:26 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [sandbox].[vw_DealerMargin]
AS
SELECT
	am.Dealer,
	am.DealerName,
	am.SaddlebackID,
	am.CustomerID,
	am.CustomerName,
	am.BillDate,
	am.ChargeType,
	am.RetailUSOC,
	am.RetailUSOCDescription,
	am.CHSCategory,
	am.RetailOnly,
	am.RetailActive,
	am.WholesaleUSOC,
	am.WholesaleUSOCDescription,
	am.WholesaleOnly,
	am.WholesaleActive,
	am.Retail,
	am.Wholesale,
	am.RetailQty,
	am.WholesaleQty,
	am.WholesaleEach,
	am.RetailEach,
	am.GrossMargin,
	am.OCCQty,
	am.ProRated,
	am.ProratedDays,
	am.Credit,
	am.OCCChargeType,
	am.InstallChargeEach,
	am.DealerCostEach,
	am.DealerCostFromLookup,
	DealerCostApplied.Value DealerCostApplied,
	DealerMargin.Value DealerMargin,
	/* REVIEW: If the original comments are correct, then the NetMargin calculation
				is broken because it does not take into account RetailUSOC = 'NMF'.
				Recommend replacing current calculation with this new one that
				eliminates the duplication that seems to have led to the error. */
	am.GrossMargin - DealerMargin.Value - -- also subtract the master dealer cost
	CASE 
		WHEN am.MasterDealer = 'None' THEN 0 
		WHEN am.RetailUSOC = 'NMF' then 0 
		ELSE DealerCostApplied.Value - MasterDealerCost.Value * IIF(am.MasterDealerCostEach < 0, -1, 1)
	END
	NetMargin,
	--am.GrossMargin - Convert
	--	(
	--		decimal(12,2), -- gross margin - dealer margin = net margin
	--		case 
	--			when am.WholesaleOnly = 'Yes' then -am.wholesale
	--			when am.OCCChargeType = 'Install' then -am.Wholesale
	--			else am.Retail - Convert(decimal(12,2), am.DealerCostFromLookup) * IIF(am.Retail < 0, -1, 1) -- less dealer margin applied
	--			/* REVIEW: removed unreachable code */
	--			--else am.Retail
	--			--	- -- less dealer margin applied
	--			--	case 
	--			--		when am.WholesaleOnly = 'Yes' then am.Wholesale -- wholesaleonly
	--			--		when am.OCCChargeType = 'Install' then am.Wholesale
	--			--		else Convert(decimal(12,2), am.DealerCostFromLookup) 
	--			--	end
	--			--	* sandbox.udf_SignNZ(am.Retail)
	--		end
	--	) NetMargin,
	am.MasterDealer, 
	am.MasterDealerCostEach,
    CASE 
		WHEN am.MasterDealer = 'None' THEN 0 
		WHEN am.RetailUSOC = 'NMF' then am.Retail 
		ELSE MasterDealerCost.Value * IIF(am.MasterDealerCostEach < 0, -1, 1)
	END AS MasterDealerCost, 
    CASE 
		WHEN am.MasterDealer = 'None' THEN 0 
		WHEN am.RetailUSOC = 'NMF' then 0 
		ELSE DealerCostApplied.Value - MasterDealerCost.Value * IIF(am.MasterDealerCostEach < 0, -1, 1)
	END AS MasterDealerMargin,
	am.MasterCustomer,
	am.MasterCustomerName,
	am.AgentID,
	am.AgentName,
	am.Day2YN
from vw_ARSummary ars
left join sandbox.vw_AnalysisMargin am on am.CustomerID = ars.Customerid 
OUTER APPLY
	(SELECT Convert
			(
				decimal(12,2), 
				case 
					when am.WholesaleOnly = 'Yes' then am.Wholesale -- wholesaleonly
					when am.OCCChargeType = 'Install' then am.Wholesale
					else
						case 
							when am.OCCQty = 0 then isnull(am.RetailQty, 0)
							when am.Prorated = 'Yes' then Convert(decimal(10,4), Convert(decimal(10,4), am.ProratedDays) / 30. * Convert(decimal(10,4), am.OCCqty)) 
							else am.OCCQty 
						end
						* am.MasterDealerCostEach  
						* IIF(am.Retail < 0 OR am.Wholesale < 0, -1, 1)
				end
			) Value
	) MasterDealerCost
OUTER APPLY
	(SELECT Convert
		(
			decimal(12,2), 
			case 
			    WHEN am.Dealer = 'CCIDealer' then 0
				WHEN am.RetailUSOC = 'NMF' then am.Retail 
				when am.WholesaleOnly = 'Yes' then am.Wholesale -- wholesaleonly
				when am.OCCChargeType = 'Install' then am.Wholesale
				else Convert(decimal(12,2), am.DealerCostFromLookup) 
			end
				* IIF(am.Retail < 0, -1, 1)
		) Value
	) DealerCostApplied
OUTER APPLY
	(SELECT Convert
		(
			decimal(12,2), 
			case 
				when am.WholesaleOnly = 'Yes' then -am.wholesale
				when am.OCCChargeType = 'Install' then -am.Wholesale
				when am.RetailUSOC = 'NMF' then 0 
				else am.Retail - Convert(decimal(12,2), am.DealerCostFromLookup) * IIF(am.Retail < 0, -1, 1) -- less dealer margin applied
				/* REVIEW: Removed unreachable code */
				--else am.Retail
				--	- -- less dealer margin applied
				--	case 
				--		when am.WholesaleOnly = 'Yes' then am.Wholesale -- wholesaleonly
				--		when am.OCCChargeType = 'Install' then am.Wholesale
				--		else Convert(decimal(12,2), am.DealerCostFromLookup) 
				--	end
				--	* sandbox.udf_SignNZ(am.Retail)
			end
		) Value
	) DealerMargin
WHERE
	-- filter from original vw_DealerMargin
	(
		(
			am.RetailUSOC <> 'wsafe'
			AND am.RetailUSOC <> 'winst'
			--AND am.RetailUSOC <> 'nmf'
		)
		OR
			am.RetailUSOC IS NULL
	)
	AND
	-- filter from vw_DealerMarginBase
	(
		not
		(
			am.RetailOnly = 'No'
			and isnull(am.RetailQty,0) = 0
			and IIF(am.WholesaleOnly = 'Yes', -am.Wholesale, am.Retail - am.DealerCostFromLookup) = 0
			and am.ChargeType = 'MRC'
		)
		and not
		(
			am.ChargeType = 'OCC'
			and am.RetailUSOC is not null
			and am.Prorated = 'No'
			and am.RetailOnly = 'No'
			and am.OCCChargeType = 'Install'
		) --Non-Prorated Retail NRC -USOC NOT RETAIL ONLY 
	)


GO


