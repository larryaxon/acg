USE [CityHostedTest]
GO

/****** Object:  View [dbo].[vw_CityHostedUSOCs]    Script Date: 7/8/2017 9:08:39 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO









-- select * from productlist where carrier in ('saddleback', 'cityhosted')

-- select * from [vw_CityHostedUSOCs] where wholesaleonly = 'yes' or usocretail is null
--select m.itemid, name, itemsubcategory, carrier from masterproductlist m inner join productlist p on m.itemid = p.itemid where carrier in ('saddleback', 'cityhosted')

ALTER view [dbo].[vw_CityHostedUSOCs] as
select 
	coalesce(m.Name, m2.name) Description, 
	coalesce(m.ExternalName, m2.ExternalName) ExternalDescription, 
	retail.PrimaryCarrier,
	retail.ItemID USOCRetail, 
	retail.mrc MRCRetail, 
	retail.NRC NRCRetail, 
	m2.Name DescriptionWholesale, 
	m2.ExternalName ExternalDescriptionWholesale, 
	wholesale.ItemID USOCWholesale, 
	wholesale.MRC MRCWholesale,
	 wholesale.NRC NRCWholesale, 
	case when isnull(retail.unmatched,0) = 1 then 'Yes' else 'No' end RetailOnly,
	case when isnull(wholesale.unmatched,0) = 1 then 'Yes' else 'No' end WholesaleOnly,
	case when getdate() between isnull(retail.startdate, '1/1/1900') and isnull(retail.enddate, '12/31/2100') then 'Yes' else 'No' end RetailActive,
	case when getdate() between isnull(wholesale.startdate, '1/1/1900') and isnull(wholesale.enddate, '12/31/2100') then 'Yes' else 'No' end WholesaleActive,
	retail.StartDate RetailStartDate, retail.EndDate RetailEndDate, wholesale.StartDate WholesaleStartDate, wholesale.EndDate WholesaleEndDate,
	case when m.issaddlebackusoc = 1  then 'Yes' else 'No' end IsExternal,
	case when sd.UseMRC = 1 then 'Yes' else 'No' end UseMRCinDQ,
	sd.ScreenSection DealerQuoteCategory,
	sd.IsRecommended DQLessScreenOnly,
	m.ExternalCategory,
	m.ItemSubCategory CHSCategory,
	case when isnull(retail.ExcludeFromException,0) = 0 then 'No' else 'Yes' end ExcludeFromExceptions,
	retail.TaxCode
from ProductList retail
left join (select ItemID, Name, ItemSubCategory, MasterItemID, ExternalName, ExternalCategory, IsSaddlebackUSOC from MasterProductList m
	) m on retail.ItemID = m.ItemID and retail.Carrier = 'cityhosted'
left join ProductList wholesale on wholesale.ItemID = m.MasterItemID and wholesale.Carrier <> 'CityHosted'
left join MasterProductList m2 on wholesale.ItemID = m2.ItemID and Wholesale.Carrier <> 'CityHosted'

left join ScreenDefinition sd on sd.Screen = 'DealerQuote' and sd.ItemID = retail.itemid
where isnull(retail.carrier, 'CityHosted') = 'CityHosted' and isnull(wholesale.carrier, 'SaddleBack') <> 'CityHosted' and coalesce(m.Name, m2.name) is not null
union
select 
	null Description,
	m.ExternalName ExternalDescription, 
	wholesale.Carrier PrimaryCarrier,
	null USOCRetail,
	null MRCRetail, 
	null NRCRetail, 
	m.Name DescriptionWholesale, 
	m.externalname ExternalDescriptionWholesale, 
	wholesale.ItemID USOCWholesale, 
	wholesale.MRC MRCWholesale, 
	wholesale.NRC NRCWholesale, 
	'No' RetailOnly,
	case when wOnly.USOC is null then 'No' else 'Yes' end WholesaleOnly,
	'No' RetailActive,
	case when getdate() between isnull(wholesale.startdate, '1/1/1900') and isnull(wholesale.enddate, '12/31/2100') then 'Yes' else 'No' end WholesaleActive,
	null RetailStartDate, null RetailEndDate, wholesale.StartDate WholesaleStartDate, wholesale.EndDate WholesaleEndDate,
	case when m.issaddlebackusoc = 1  then 'Yes' else 'No' end IsExternal,
	case when sd.UseMRC = 1 then 'Yes' else 'No' end UseMRCinDQ,
	sd.ScreenSection DealerQuoteCategory,
	sd.IsRecommended DQLessScreenOnly,
	m.ExternalCategory,
	m.ItemSubCategory CHSCategory,
	case when isnull(retail.ExcludeFromException,0) = 0 then 'No' else 'Yes' end ExcludeFromExceptions,
	wholesale.TaxCode
from ProductList wholesale
inner join MasterProductList m on wholesale.itemid = m.itemid
left join HostedNoMatchUSOCs wOnly on wOnly.usoc = wholesale.ItemId and wOnly.Type = 'Wholesale'
left join ProductList retail on retail.itemid = m.itemid and retail.carrier = 'CityHosted'
left join ScreenDefinition sd on sd.Screen = 'DealerQuote' and sd.ItemID = wholesale.itemid
where m.itemid = m.masteritemid
and wholesale.carrier = 'Saddleback'
and retail.itemid is null
and wholesale.itemid not in (select distinct masteritemid from masterproductlist where itemid <> masteritemid 

)




GO


