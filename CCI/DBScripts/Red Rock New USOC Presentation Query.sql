USE [CityHostedTest]
GO

/****** Object:  View [dbo].[vw_CityHostedUSOCPresentation]    Script Date: 7/8/2017 9:08:19 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--select * from vw_CityHostedUSOCPresentation order by primarycarrier, usocretail

ALTER view [dbo].[vw_CityHostedUSOCPresentation] as 
select USOCRetail, Description DescriptionRetail, USOCWholesale, DescriptionWholesale, PrimaryCarrier, 
case when MRCRetail = -1 then 'Variable' else Convert(nvarchar(20), MRCRetail) end MRCRetail, MRCWholesale, 
Case when NRCRetail = -1 then 'Variable' else Convert(nvarchar(20), NRCRetail) end NRCRetail, NRCWholesale, 
RetailOnly, WholesaleOnly, RetailActive, WholesaleActive, CHSCategory,
case when DealerQuoteCategory is null then 'N' else 'Y' end DealerQuoteYN,
DealerQuoteCategory DQCategory, ExternalDescription [RIT Retail Bill Presentation],
ExternalCategory [RIT Category], IsExternal IsSaddlebackUSOC, RetailStartDate, RetailEndDate, WholesaleStartDate, WholesaleEndDate,
UseMRCinDQ, DQLessScreenOnly, ExcludeFromExceptions, TaxCode [RIT TranTax]
from vw_CityHostedUSOCs 



GO


