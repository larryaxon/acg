USE [CityHostedTest2]
GO

/****** Object:  View [dbo].[vw_DealerMarginBase]    Script Date: 2/17/2018 10:58:12 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--select * from vw_analysismargin where chargetype = 'occ' order by InstallChargeEach desc

--select * from vw_cityhostedusocs where usocwholesale = 'w911'
ALTER view [dbo].[vw_DealerMarginBase] as
-- exclude bad wholesale 
select *,	
	Convert(decimal(12,2), 
		case 
			when WholesaleOnly = 'Yes' then a.Wholesale -- wholesaleonly
			when OCCChargeType = 'Install' then a.Wholesale
			else
			case 
				when OCCQty = 0 then isnull(RetailQty,0)
				else 
					case 
						when Prorated = 'Yes' then Convert(decimal(10,4), Convert(decimal(10,4), ProratedDays) / 30. * Convert(decimal(10,4),OCCqty)) 
						else OCCQty 
					end 
				end * MasterDealerCostEach  
					* case when (Retail < 0 or Wholesale < 0) then -1 else 1 end
			end) -- adjust for credits
		MasterDealerCost,
	Convert(decimal(12,2), 
		case 
			when WholesaleOnly = 'Yes' then a.Wholesale -- wholesaleonly
			when OCCChargeType = 'Install' then a.Wholesale
			else Convert(decimal(12,2), a.DealerCostFromLookup) 
		end
		* case when Retail < 0 then -1 else 1 end) DealerCostApplied,
Convert(decimal(12,2), 
	case 
		when WholesaleOnly = 'Yes' then -a.wholesale
		when OCCChargeType = 'Install' then -a.Wholesale
		else a.Retail - -- less dealer margin applied
			case 
				when WholesaleOnly = 'Yes' then a.Wholesale -- wholesaleonly
				when OCCChargeType = 'Install' then a.Wholesale
				else Convert(decimal(12,2), a.DealerCostFromLookup) 
			end
			* case when Retail < 0 then -1 else 1 end 
	end
		) DealerMargin,
CONVERT(decimal(12,2), -- gross margin - dealer margin = net margin
	a.GrossMargin -
		--dbo.getMax(0, 
			case 
				when WholesaleOnly = 'Yes' then -a.wholesale
				when OCCChargeType = 'Install' then -a.Wholesale
				else a.Retail - -- less dealer margin applied
					case 
						when WholesaleOnly = 'Yes' then a.Wholesale -- wholesaleonly
						when OCCChargeType = 'Install' then a.Wholesale
						else Convert(decimal(12,2), a.DealerCostFromLookup) 
					end
					* case when Retail < 0 then -1 else 1 end 
			end
			--,1)
		) NetMargin
from vw_AnalysisMargin a
where not (RetailOnly = 'No' and isnull(RetailQty,0) = 0 and 	case 
		when WholesaleOnly = 'Yes' then -a.wholesale else a.Retail - a.DealerCostFromLookup end = 0 and ChargeType = 'MRC')
		and not (ChargeType = 'OCC' and RetailUSOC is not null and Prorated = 'No' and RetailOnly = 'No' and OCCChargeType = 'Install') --Non-Prorated Retail NRC -USOC NOT RETAIL ONLY 

GO


