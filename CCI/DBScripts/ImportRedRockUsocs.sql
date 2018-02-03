USE CityHostedTest
GO

/***************************************************************************

Import Red Rock Usoc File

****************************************************************************/
DECLARE @DidAddPrimaryCarrier as bit = 0
if not exists(select * from sys.columns where name = 'PrimaryCarrier')
	SET @DidAddPrimaryCarrier = 1
	ALTER TABLE ProductList Add PrimaryCarrier nvarchar(50) NULL
GO
if @DidAddPrimaryCarrier = 1
BEGIN
	-- update productlist set primarycarrier = null
	-- first get the wholesales
	UPDATE ProductList 
	SET PrimaryCarrier = Carrier 
	WHERE Carrier in ('Saddleback', 'RedRock')

	UPDATE ProductList 
	SET PrimaryCarrier = 'Saddleback'
	where primarycarrier is null and (itemid not like 'r%' or itemid = 'router')

	UPDATE ProductList 
	SET PrimaryCarrier = 'RedRock'
	WHERE PrimaryCarrier is null and itemid like 'r%' and itemid <> 'router'

	-- query to double check results
	--select p.itemid, p.carrier, p.primarycarrier, w.itemid,  w.carrier wcarrier, w.primarycarrier wpcarrier
	--FROM ProductList p
	--inner join MasterProductList m on p.itemid = m.ItemID -- get this item on the master list
	--inner join ProductList w on m.MasterItemID = w.ItemID -- and get the wholesale item
	--order by p.PrimaryCarrier, p.ItemID

END
-- Pickup data from old file to fill in missing data in the new file

DELETE FROM LLARedRockUsocs where DQCATEGORY = 'First File Record' -- delete the ones from last time

INSERT INTO LLARedRockUsocs
select u1.[Retail USOC] USOCREtail,u1.[Retail Description] DESCRIPTIONRETAIL,  null USOCWHolesale, null DescriptionWholesale,
U1.[Retail Rate] mrcretail, null MrcWholesale, 0 NRCRetail, null NrcWholesale,  'Yes' RetailOnly, 'No' WholesaleOnly, 
'Yes' RetailActive, 'No' WholesaleActive, null CHSCategory, 'N' DetalerQuoteYn, 'First File Record'  DQCategory, u1.[Retail Description] [RIT Retail Bill Presentation],
null [RIT Category], 'Yes' IsSaddlebackUsoc, '7/1/2017' RetailStartDate, null RetailEndDate, '7/1/2017' WholesaleStartDate, null WholesaleEndDate,
'No' UseMRCInDQ, 0 DQLessScreenOnly, 'No' ExcludeFromExceptionsm, null [RIT TranTax]
	from tmpRedRockUsocs u1  
left join llaredrockusocs u2  on u2.usocretail = u1.[retail usoc]
where u2.usocretail is null or u2.usocwholesale is null

--Set up the Red Rock Carrier
DELETE FROM Entity where Entity = 'RedRock'
DELETE FROM Attribute where Entity = 'RedRock'

INSERT INTO Entity (Entity, EntityType, ENtityOwner, LegalName, LastModifiedBy, LastModifiedDateTime)
Select 'RedRock' Entity, EntityType, ENtityOwner, 'Red Rock' LegalName, 'LarryA-User', getdate()
FROM ENTITY where entity = 'saddleback'

INSERT INTO Attribute (Entity, ItemType, Item, ItemHistory, Attributes, LastModifiedBy, LastModifiedDateTime)
Select 'RedRock', ItemType, Item, ItemHistory, Attributes, 'LarryA-User', getdate()
FROM Attribute where entity = 'saddleback'

-- Now the USOCs

-- Delete the last imported batch
Delete 
-- select * 
from [dbo].[MasterProductList] where itemid like 'r%' and itemid <> 'router'

DELETE 
-- select *
FROM [dbo].ProductList where itemid like 'r%' and itemid <> 'router'

/***************************************************************************************************************

MASTER PRODUCT LIST

****************************************************************************************************************/
-- We have no WholesaleOnly so first get retail only

INSERT INTO dbo.MasterProductList 
SELECT USOCRETAIL ItemID, descriptionRetail Name, 'Red Rock' ItemSubCategory, '' MasterItemID, 1 IsCityHostedRetail, 0 isCityHostedWHolesale, 
1 IsSaddlebackUSOC, DescriptionRetail ExternalName, null IsSaddlebackVariable, 'Import From RedRock' ExternalCategory 
from LLARedRockUsocs
where USOCWHOLESALE is null

-- Now get matched where retail usoc is the same as wholesale usoc

INSERT INTO dbo.MasterProductList 
SELECT USOCRETAIL ItemID, descriptionRetail Name, 'Red Rock' ItemSubCategory, USOCRETAIL MasterItemID, 1 IsCityHostedRetail, 1 isCityHostedWHolesale, 
1 IsSaddlebackUSOC, DescriptionRetail ExternalName, CASE WHEN MRCRetail = 'Variable' then 1 else 0 END IsSaddlebackVariable, 
'Import From RedRock' ExternalCategory 
from LLARedRockUsocs
where USOCWHOLESALE = USOCRETAIL

-- Now get the wholesale usocs where they are not the same as retail

INSERT INTO dbo.MasterProductList 
SELECT USOCWHOLESALE ItemID, DESCRIPTIONWHOLESALE Name, 'Red Rock' ItemSubCategory, USOCWHOLESALE MasterItemID, 0 IsCityHostedRetail, 1 isCityHostedWHolesale, 
1 IsSaddlebackUSOC, Coalesce(DescriptionRetail, DESCRIPTIONWHOLESALE) ExternalName, CASE WHEN MRCREtail = 'Variable' then 1 else 0 END IsSaddlebackVariable, 
'Import From RedRock' ExternalCategory 
from LLARedRockUsocs
where USOCWHOLESALE <> USOCRETAIL

-- Now get the retail usocs where they are not the same as wholesale

INSERT INTO dbo.MasterProductList 
SELECT USOCRETAIL ItemID, descriptionRetail Name, 'Red Rock' ItemSubCategory, USOCWHOLESALE MasterItemID, 1 IsCityHostedRetail, 1 isCityHostedWHolesale, 
1 IsSaddlebackUSOC, DescriptionRetail ExternalName, CASE WHEN MRCRetail = 'Variable' then 1 else 0 END IsSaddlebackVariable, 
'Import From RedRock' ExternalCategory 
from LLARedRockUsocs
where USOCWHOLESALE <> USOCRETAIL


DECLARE @ModifiedDateTime as datetime = getdate()

---- Now Retail Only ProductList
INSERT INTO ProductList
Select 'CityHosted' Carrier, USOCRetail ItemID, '7/1/2017' StartDate, null EndDate, Convert(decimal(8,2),replace(MRCRETAIL,'$','')) MRC,
	   Convert(decimal(8,2),replace(NRCRETAIL,'$','')) NRC, 0 unmatched,
       0 ExcludeFromException, 'LarryA-User' LastModifiedBy, @ModifiedDateTime lastModifiedDateTime, [RIT TRANTAX] TaxCode, 'RedRock' PrimaryCarrier
FROM [dbo].[LLARedRockUsocs]
WHERE USOCWHOLESALE is null

-- Now Retail USOCS where usocs match

INSERT INTO ProductList
Select 'CityHosted' Carrier, USOCRetail ItemID, '7/1/2017' StartDate, null EndDate, 
case when MRCRetail = 'Variable' then -1 else Convert(decimal(8,2),replace(MRCRETAIL,'$','')) END MRC,
case when NRCRETAIL = 'Variable' then -1 else Convert(decimal(8,2),replace(NRCRETAIL,'$','')) END NRC, 
0 unmatched, 0 ExcludeFromException, 'LarryA-User' LastModifiedBy, @ModifiedDateTime lastModifiedDateTime, [RIT TRANTAX] TaxCode, 'RedRock' PrimaryCarrier
FROM [dbo].[LLARedRockUsocs]
WHERE USOCWHOLESALE = USOCRETAIL

-- Now wholesale USOCS where usocs match

INSERT INTO ProductList
Select 'RedRock' Carrier, USOCWHOLESALE ItemID, '7/1/2017' StartDate, null EndDate, MRCWHOLESALE MRC,
	   NRCWHOLESALE NRC, 0 unmatched,
       0 ExcludeFromException, 'LarryA-User' LastModifiedBy, @ModifiedDateTime lastModifiedDateTime, [RIT TRANTAX] TaxCode, 'RedRock' PrimaryCarrier
FROM [dbo].[LLARedRockUsocs]
WHERE USOCWHOLESALE = USOCRETAIL

-- now retail USOCS where usocs don't match

INSERT INTO ProductList
Select 'CityHosted' Carrier, USOCRetail ItemID, '7/1/2017' StartDate, null EndDate, 
case when MRCRetail = 'Variable' then -1 else Convert(decimal(8,2),replace(MRCRETAIL,'$','')) END MRC,
case when NRCRETAIL = 'Variable' then -1 else Convert(decimal(8,2),replace(NRCRETAIL,'$','')) END NRC, 
0 unmatched, 0 ExcludeFromException, 'LarryA-User' LastModifiedBy, '7/4/2017' lastModifiedDateTime, [RIT TRANTAX] TaxCode, 'RedRock' PrimaryCarrier
FROM [dbo].[LLARedRockUsocs]
WHERE USOCWHOLESALE <> USOCRETAIL

-- Now wholesale USOCS where usocs don't  match

INSERT INTO ProductList
Select 'RedRock' Carrier, USOCWHOLESALE ItemID, '7/1/2017' StartDate, null EndDate, MRCWHOLESALE MRC,
	   NRCWHOLESALE NRC, 0 unmatched,
       0 ExcludeFromException, 'LarryA-User' LastModifiedBy, @ModifiedDateTime lastModifiedDateTime, [RIT TRANTAX] TaxCode, 'RedRock' PrimaryCarrier
FROM [dbo].[LLARedRockUsocs]
WHERE USOCWHOLESALE <> USOCRETAIL



