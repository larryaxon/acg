USE [CityHostedTest2]
GO

/****** Object:  View [dbo].[vw_DealerMargin]    Script Date: 2/17/2018 7:48:07 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
select top 10 * from vw_DealerMargin where dealer = 'accram' and billdate = '12/1/2017' and retailusoc = 'nmf'
select * from hostedmatchedocc where CustomerID = '23029' and WholesaleUSOC = 'woipld' and wholesalebilldate = '5/1/2013'
*/
ALTER VIEW [dbo].[vw_DealerMargin]
AS
SELECT     TOP (100) PERCENT Dealer, DealerName, SaddlebackID, CustomerID, CustomerName, BillDate, ChargeType, RetailUSOC, RetailUSOCDescription, CHSCategory, 
                      RetailOnly, RetailActive, WholesaleUSOC, WholesaleUSOCDescription, WholesaleOnly, WholesaleActive, Retail, Wholesale, RetailQty, WholesaleQty, 
                      WholesaleEach, RetailEach, GrossMargin, OCCQty, ProRated, ProratedDays, Credit, OCCChargeType, InstallChargeEach, DealerCostEach, DealerCostFromLookup, 
                      DealerCostApplied, DealerMargin, NetMargin, MasterDealer, 
					  CASE 
						  WHEN MasterDealer = 'None' THEN 0 
						  ELSE MasterDealerCostEach END AS MasterDealerCostEach,
                      CASE 
						  WHEN MasterDealer = 'None' THEN 0 
						  WHEN RetailUSOC = 'NMF' then Retail 
						  ELSE CASE 
									WHEN MasterDealerCostEach < 0 then MasterDealerCost * -1
									ELSE MasterDealerCost
							   END
					  END AS MasterDealerCost, 
                      CASE 
						  WHEN MasterDealer = 'None' THEN 0 
						  WHEN RetailUSOC = 'NMF' then 0 
						  ELSE DealerCostApplied - 
								CASE 
									WHEN MasterDealerCostEach < 0 then MasterDealerCost * -1
									ELSE MasterDealerCost
								END  
					  END AS MasterDealerMargin
FROM         dbo.vw_DealerMarginBase
WHERE     ((RetailUSOC <> 'wsafe') AND (RetailUSOC <> 'winst') 
			--AND (RetailUSOC <> 'nmf')
		  ) OR (RetailUSOC IS NULL)


GO


