/*

	Red Rock Import database changes

	Created by: Larry Axon
	Created Date: 8-5-2017

	History:
	Modified by			Modified Date	Description

*/

/***************************************************************************************

	Update products

****************************************************************************************/

ALTER TABLE ProductList 
ADD [PrimaryCarrier] [nvarchar](50) NULL

Update ProductList
SET PrimaryCarrier = 'Saddleback'


INSERT INTO MasterProductList
select * from [cityhostedtest].dbo.MasterProductList where itemid like 'r%' and itemid <> 'router'

INSERT INTO PRODUCTLIST
SELECT * FROM [cityhostedtest].dbo.ProductList where itemid like 'r%' and itemid <> 'router'

/***************************************************************************************

	Add Red Rock Carrier

****************************************************************************************/

insert into Entity
select * from [cityhostedtest].dbo.Entity where Entity = 'RedRock'

Insert into Attribute
select * from [cityhostedtest].dbo.Attribute where Entity = 'RedRock'

ALTER TABLE AcctImportsLog
ADD 	[Source] [nvarchar](50) NULL

Update AcctImportsLog
SET Source = 'Saddleback'

UPDATE [ProcessSteps]
SET FormParameters = 'Saddleback', Step = 'Import Saddleback'
where Step = 'Import'

INSERT INTO [ProcessSteps] (Cycle, Sequence, Step, Description, Form, FormParameters, IsRequired)
VALUES ('CHSFinancials',2,'Import Red Rock','Import Red Rock Files','frmImports','RedRock',1)

CREATE TABLE [dbo].[ImportSources](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Source] [varchar](50) NULL,
	[FtpSiteUrl] [varchar](255) NULL,
	[FtpSiteUsername] [varchar](50) NULL,
	[FtpSitePassword] [varchar](50) NULL
) ON [PRIMARY]

INSERT INTO ImportSources (Source, FtpSiteUrl,FtpSiteUsername, FtpSitePassword)
Select Source, FtpSiteUrl,FtpSiteUsername, FtpSitePassword 
from [CityHostedTest].[dbo].[ImportSources]

CREATE TABLE [dbo].[ImportFileTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Source] [varchar](50) NULL,
	[FileType] [varchar](50) NULL,
	[Prefix] [varchar](50) NULL,
	[Suffix] [varchar](50) NULL,
	[StoredProcedure] [varchar](100) NULL,
	[CreatedDateTime] [datetime] NULL,
	[ModifiedDateTime] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NULL,
	[SkipLines] [int] NULL
) ON [PRIMARY]

INSERT INTO ImportFileTypes (Source, FileType, PreFix, Suffix, StoredProcedure, CreatedDateTime, ModifiedDateTime, CreatedBy, ModifiedBy, SkipLines)
SELECT Source, FileType, PreFix, Suffix, StoredProcedure, CreatedDateTime, ModifiedDateTime, CreatedBy, ModifiedBy, SkipLines
from [CityHostedTest].[dbo].[ImportFileTypes]


/***************************************************************************************
	Update Queries
****************************************************************************************/

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



/***************************************************************************************
	Add new stored procedures for Red Rock Import
****************************************************************************************/

GO
-- =============================================
-- Author:		Larry Axon
-- Create date: 7/29/2017
-- Description:	Import tab delimited format Ledger import to existing table (Red Rock)
-- =============================================
--exec [dbo].[ImportHostedRedRockLedger] 'C:\Data\Red Rock Imports\2017\2017-07\Ledger Red Rock-201707.txt', '7/1/2017'
CREATE PROCEDURE [dbo].[ImportHostedRedRockLedger]	(@FilePath nvarchar (255),@BillDate varchar(10))
	
AS
BEGIN
/*
Account Number	Customer ID	Customer Name	Type	Statement ID	Amount	Received Date		Payment Type	Reference Number	Created Date		Created Date
R1007			1007		Glove IT LLC	Bill	2652			270.67															7/5/2017 11:14 AM	PDT

*/

	if object_id (N'"HostedTempRedRockLedger"', N'U') is NULL
		CREATE TABLE [dbo].HostedTempRedRockLedger(
			[Account Number] [varchar](50) NULL,
			[CustomerID] [varchar](50) NULL,
			[Customer Name] [varchar](255) NULL,
			[Type] [varchar](50) NULL,
			[Statement ID] [varchar](50) NULL,
			[Amount] [varchar](50) NULL,
			[Received Date] [varchar](50) NULL,
			[Payment Type] [varchar](50) NULL,
			[Reference Number] [varchar](50) NULL,
			[Created Date] [varchar](50) NULL,
			[Time Zone] [varchar](50) NULL,

		) ON [PRIMARY]

	delete from [dbo].HostedTempRedRockLedger;

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempRedRockLedger FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = 2, FIELDTERMINATOR = ''\t'', ROWTERMINATOR = ''\n'')'
	print @billdate
	EXEC sp_executesql @BulkInsert;
	--return

    if not exists (select top 1 Convert(varchar(2), Datepart(month, [Created Date])) + '/01/' + Convert(varchar(4), datepart(year, [Created Date])) 
                 from HostedTempRedRockLedger 
                where  Convert(varchar(2), Datepart(month, [Created Date])) + '/01/' + Convert(varchar(4), datepart(year, [Created Date])) = @BillDate)
       begin
            print 'Good Date'
	
			insert into [dbo].[HostedImportLedger] ([Customer],[CustomerName],[TransactionDate],[Title],[Amount],[BillDate],[MatchedBy],[MatchedDateTime])
			 select [Account Number] as Customer,
					[Customer Name] as [CustomerName],
					[Created Date] as [TransactionDate],
					[Type] as [Title],
					Amount, 
					@BilLDate as BillDate,
					null [MatchedBy],
					null MatchedDateTime
			   from [dbo].HostedTempRedRockLedger;
			   
			--delete from [dbo].HostedTempRedRockLedger;   
       end
    else
       begin
            print 'Bad Date'
       end

END

GO


-- =============================================
-- Author:		Larry Axon
-- Create date: 7/22/2017
-- Description:	Import tab delimited format mrc retail import to existing table (Red Rock)
-- =============================================
--exec [dbo].[ImportHostedRedRockMRCRetail] 'C:\Data\Red Rock Imports\2017\2017-07\Retail MRC Red Rock-201707.txt', '7/1/2017'
CREATE PROCEDURE [dbo].[ImportHostedRedRockMRCRetail]	(@FilePath nvarchar (255),@BillDate varchar(10))
	
AS
BEGIN
/*
AcctNum	Customer	City Title	RR Title	USOC	Quantity	Amt Billed	Price	Conn Date	Bill Date	TN	Invoice ID	Time Zone
R1007	Glove IT LLC	Administrative Billing Fee	Administrative Billing Fee	R_WBOB	1	5	5	11/1/2016	4/3/2017 16:45	4809682021	6994	PDT

*/

	if object_id (N'"HostedTempRedRockMRCRetail"', N'U') is NULL
		CREATE TABLE [dbo].[HostedTempRedRockMRCRetail](
			[AcctNum] [varchar](50) NULL,
			[Customer] [varchar](50) NULL,
			[City Title] [varchar](50) NULL,
			[RR Title] [varchar](50) NULL,
			[USOC] [varchar](50) NULL,
			[Quantity] [varchar](50) NULL,
			[Amt Billed] [varchar](50) NULL,
			[Price] [varchar](50) NULL,
			[Conn Date] [varchar](50) NULL,
			[Bill Date] [varchar](50) NULL,
			[TN] [varchar](50) NULL,
			[Invoice ID] [varchar](50) NULL,
			[Time Zone] [varchar](50) NULL
		) ON [PRIMARY]



	delete from [dbo].[HostedTempRedRockMRCRetail];

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempRedRockMRCRetail FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = 3, FIELDTERMINATOR = ''\t'', ROWTERMINATOR = ''\n'')'
	print @billdate
	EXEC sp_executesql @BulkInsert;
    if not exists (select top 1 Datepart(month, [Bill Date]) m, datepart(year, [Bill Date]) y
                 from HostedTempRedRockMRCRetail 
                where  Datepart(month, [Bill Date]) <> datepart(month, @BillDate) and datepart(year, [Bill Date]) <> datepart(year, @BillDate))
       begin
            print 'Good Date'
			insert into [dbo].[HostedImportMRCRetail] (customernumber,CustomerName, MasterBTN, BTN, USOC, [Product Description], Qty, Price, Amount, ConnectionDate, BillDate)
			 select [AcctNum] as customernumber,
					[Customer] as CustomerName,
					replace(replace(replace(replace([TN],' ',''),'(',''),')',''),'-','') as MasterBTN,
					replace(replace(replace(replace([TN],' ',''),'(',''),')',''),'-','') as BTN,
					USOC,
					[City Title] as [Product Description], 
					Quantity, 
					Price,
					[Amt Billed] as Amount,
					Convert(datetime, [Conn Date]) as ConnectionDate,
					Convert(varchar(2),Datepart(month, [Bill Date])) + '/01/' + Convert(varchar(4),Datepart(year, [Bill Date])) as BillDate
			   from [dbo].[HostedTempRedRockMRCRetail];
			   
			delete from [dbo].[HostedTempRedRockMRCRetail];   
       end
    else
       begin
            print 'Bad Date'
       end

END

GO


-- =============================================
-- Author:		Larry Axon
-- Create date: 7/22/2017
-- Description:	Import tab delimited format mrc Wholesale import to existing table (Red Rock)
-- =============================================
--exec [dbo].[ImportHostedRedRockMRCWholesale] 'C:\Data\Red Rock Imports\2017\2017-07\Wholesale MRC Red Rock-201707.txt', '7/1/2017'
CREATE PROCEDURE [dbo].[ImportHostedRedRockMRCWholesale]	(@FilePath nvarchar (255),@BillDate varchar(10))
	
AS
BEGIN
/*
Account Number	Customer ID	Customer	City Title	RR Title	USOC	Quantity	Amt Billed	Price	Conn Date	Bill Date	TN	Invoice ID	Time Zone
R1007	1007	Glove IT LLC	Administrative Billing Fee	Bill on Behalf - White Label/Invoice	R_WBOB	1	5	5	11/1/2016	3/6/2017 11:58	4809682021	6290	PDT

*/

	if object_id (N'"HostedTempRedRockMRCWholesale"', N'U') is NULL
		CREATE TABLE [dbo].[HostedTempRedRockMRCWholesale](
			[Account Number] [varchar](50) NULL,
			[Customer ID] [varchar](50) NULL,
			[Customer] [varchar](50) NULL,
			[City Title] [varchar](50) NULL,
			[RR Title] [varchar](50) NULL,
			[USOC] [varchar](50) NULL,
			[Quantity] [varchar](50) NULL,
			[Amt Billed] [varchar](50) NULL,
			[Price] [varchar](50) NULL,
			[Conn Date] [varchar](50) NULL,
			[Bill Date] [varchar](50) NULL,
			[TN] [varchar](50) NULL,
			[Invoice ID] [varchar](50) NULL,
			[Time Zone] [varchar](50) NULL
		) ON [PRIMARY]



	delete from [dbo].[HostedTempRedRockMRCWholesale];

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempRedRockMRCWholesale FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = 3, FIELDTERMINATOR = ''\t'', ROWTERMINATOR = ''\n'')'
	print @billdate
	EXEC sp_executesql @BulkInsert;
    if not exists (select top 1 Datepart(month, [Bill Date]) m, datepart(year, [Bill Date]) y
                 from HostedTempRedRockMRCWholesale 
                where  Datepart(month, [Bill Date]) <> datepart(month, @BillDate) and datepart(year, [Bill Date]) <> datepart(year, @BillDate))
       begin
            print 'Good Date'
			insert into [dbo].[HostedImportMRCWholesale] (customernumber,CustomerName, MasterBTN, BTN, USOC, [Product Description], Qty, Price, Amount, ConnectionDate, BillDate)
			 select [Account Number] as customernumber,
					[Customer] as CustomerName,
					replace(replace(replace(replace([TN],' ',''),'(',''),')',''),'-','') as MasterBTN,
					replace(replace(replace(replace([TN],' ',''),'(',''),')',''),'-','') as BTN,
					USOC,
					[City Title] as [Product Description], 
					Quantity, 
					Price,
					[Amt Billed] as Amount,
					Convert(datetime, [Conn Date]) as ConnectionDate,
					Convert(varchar(2),Datepart(month, [Bill Date])) + '/01/' + Convert(varchar(4),Datepart(year, [Bill Date])) as BillDate
			   from [dbo].[HostedTempRedRockMRCWholesale];
			   
			delete from [dbo].[HostedTempRedRockMRCWholesale];   
       end
    else
       begin
            print 'Bad Date'
       end

END

GO

-- =============================================
-- Author:		Larry Axon
-- Create date: 7/22/2017
-- Description:	Import tab delimited format OCC retail import to existing table (Red Rock)
-- =============================================
--exec [dbo].[ImportHostedRedRockOCCRetail] 'C:\Data\Red Rock Imports\2017\2017-07\Retail OCC Red Rock-201707.txt', '7/1/2017'
CREATE PROCEDURE [dbo].[ImportHostedRedRockOCCRetail]	(@FilePath nvarchar (255),@BillDate varchar(10))
	
AS
BEGIN
/*
AcctNum	Customer	City Title	RR Title	USOC	Quantity	Amt Billed	Price	Conn Date	Bill Date	TN	Invoice ID	Time Zone
R1180	Four Seasons Roofing	Telax Contact Center - Gold	Telax Contact Center - Gold	R_MCCCG	1	102.14	130	2/7/2017	3/6/2017 15:14	5865660308	6400	PDT

*/

	if object_id (N'"HostedTempRedRockOCCRetail"', N'U') is NULL
		CREATE TABLE [dbo].[HostedTempRedRockOCCRetail](
			[AcctNum] [varchar](50) NULL,
			[Customer] [varchar](50) NULL,
			[City Title] [varchar](50) NULL,
			[RR Title] [varchar](50) NULL,
			[USOC] [varchar](50) NULL,
			[Quantity] [varchar](50) NULL,
			[Amt Billed] [varchar](50) NULL,
			[Price] [varchar](50) NULL,
			[Conn Date] [varchar](50) NULL,
			[Bill Date] [varchar](50) NULL,
			[TN] [varchar](50) NULL,
			[Invoice ID] [varchar](50) NULL,
			[Time Zone] [varchar](50) NULL
		) ON [PRIMARY]



	delete from [dbo].[HostedTempRedRockOCCRetail];

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempRedRockOCCRetail FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = 3, FIELDTERMINATOR = ''\t'', ROWTERMINATOR = ''\n'')'
	print @billdate
	EXEC sp_executesql @BulkInsert;
    if not exists (select top 1 Datepart(month, [Bill Date]) m, datepart(year, [Bill Date]) y
                 from HostedTempRedRockOCCRetail 
                where  Datepart(month, [Bill Date]) <> datepart(month, @BillDate) and datepart(year, [Bill Date]) <> datepart(year, @BillDate))
       begin
            print 'Good Date'
			insert into [dbo].[HostedImportOCCRetail] ([Customer]
			  ,[MasterBTN]
			  ,[BTN]
			  ,[Date]
			  ,[Amount]
			  ,[Code]
			  ,[USOC]
			  ,[Service]
			  ,[BillDate])
			 select [AcctNum] as customernumber,
					replace(replace(replace(replace([TN],' ',''),'(',''),')',''),'-','') as MasterBTN,
					replace(replace(replace(replace([TN],' ',''),'(',''),')',''),'-','') as BTN,
					[Bill Date] as [Date],
					[Amt Billed] as Amount,
					null as Code,
					USOC,
					[City Title] as [Service], 
					Convert(varchar(2),Datepart(month, [Bill Date])) + '/01/' + Convert(varchar(4),Datepart(year, [Bill Date])) as BillDate
			   from [dbo].[HostedTempRedRockOCCRetail];
			   
			delete from [dbo].[HostedTempRedRockOCCRetail];   
       end
    else
       begin
            print 'Bad Date'
       end

END

GO
-- =============================================
-- Author:		Larry Axon
-- Create date: 7/22/2017
-- Description:	Import tab delimited format OCC Wholesale import to existing table (Red Rock)
-- =============================================
--exec [dbo].[ImportHostedRedRockOCCWholesale] 'C:\Data\Red Rock Imports\2017\2017-07\Wholesale OCC Red Rock-201707.txt', '7/1/2017'
CREATE PROCEDURE [dbo].[ImportHostedRedRockOCCWholesale]	(@FilePath nvarchar (255),@BillDate varchar(10))
	
AS
BEGIN
/*
AcctNum	Customer	City Title	RR Title	USOC	Quantity	Amt Billed	Price	Conn Date	Bill Date	TN	Invoice ID	Time Zone
R1180	Four Seasons Roofing	Telax Contact Center - Gold	Telax Contact Center - Gold	R_MCCCG	1	102.14	130	2/7/2017	3/6/2017 15:14	5865660308	6400	PDT

*/

	if object_id (N'"HostedTempRedRockOCCWholesale"', N'U') is NULL
		CREATE TABLE [dbo].[HostedTempRedRockOCCWholesale](
			[Account Number] [varchar](50) NULL,
			[Customer ID] [varchar](50) NULL,
			[Customer] [varchar](50) NULL,
			[City Title] [varchar](50) NULL,
			[RR Title] [varchar](50) NULL,
			[USOC] [varchar](50) NULL,
			[Quantity] [varchar](50) NULL,
			[Amt Billed] [varchar](50) NULL,
			[Price] [varchar](50) NULL,
			[Conn Date] [varchar](50) NULL,
			[Bill Date] [varchar](50) NULL,
			[TN] [varchar](50) NULL,
			[Invoice ID] [varchar](50) NULL,
			[Time Zone] [varchar](50) NULL
		) ON [PRIMARY]

	delete from [dbo].[HostedTempRedRockOCCWholesale];

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempRedRockOCCWholesale FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = 3, FIELDTERMINATOR = ''\t'', ROWTERMINATOR = ''\n'')'
	print @billdate
	EXEC sp_executesql @BulkInsert;
    if not exists (select top 1 Datepart(month, [Bill Date]) m, datepart(year, [Bill Date]) y
                 from HostedTempRedRockOCCWholesale 
                where  Datepart(month, [Bill Date]) <> datepart(month, @BillDate) and datepart(year, [Bill Date]) <> datepart(year, @BillDate))
       begin
            print 'Good Date'
			insert into [dbo].[HostedImportOCCWholesale] ([Customer]
			  ,[MasterBTN]
			  ,[BTN]
			  ,[Date]
			  ,[Amount]
			  ,[OCCCode]
			  ,[USOC]
			  ,[Service]
			  ,[BillDate])
			 select [Account Number] as customernumber,
					replace(replace(replace(replace([TN],' ',''),'(',''),')',''),'-','') as MasterBTN,
					replace(replace(replace(replace([TN],' ',''),'(',''),')',''),'-','') as BTN,
					[Bill Date] as [Date],
					[Amt Billed] as Amount,
					null as Code,
					USOC,
					[City Title] as [Service], 
					Convert(varchar(2),Datepart(month, [Bill Date])) + '/01/' + Convert(varchar(4),Datepart(year, [Bill Date])) as BillDate
			   from [dbo].[HostedTempRedRockOCCWholesale];
			   
			delete from [dbo].[HostedTempRedRockOCCWholesale];   
       end
    else
       begin
            print 'Bad Date'
       end

END
GO
-- =============================================
-- Author:		Larry Axon
-- Create date: 7/29/2017
-- Description:	Import tab delimited format Tax import to existing table (Red Rock)
-- =============================================
--exec [dbo].[ImportHostedRedRockTax] 'C:\Data\Red Rock Imports\2017\2017-07\TAX Red Rock-201707.txt', '7/1/2017'
CREATE PROCEDURE [dbo].[ImportHostedRedRockTax]	(@FilePath nvarchar (255),@BillDate varchar(10))
	
AS
BEGIN
/*
Account Number	Customer ID	Customer	STATE	COUNTY	CITY	TAX AUTHORITY	GEOCODE	TAX TYPE	TAX CATEGORY	DESCRIPTION	REVENUE	TAXES	TAX RATE	LINE COUNT
R1007			1007		Glove IT LLC	AZ	MARICOPA	SCOTTSDALE	FEDERAL	0401365000	035	0	FEDERAL UNIVERSAL SERVICE FUND	1933.59	215.98	0.11	9

*/

	if object_id (N'"HostedTempRedRockTaxes"', N'U') is NULL
		CREATE TABLE [dbo].HostedTempRedRockTaxes(
			[Account Number] [varchar](50) NULL,
			[CustomerID] [varchar](50) NULL,
			[Customer] [varchar](50) NULL,
			[State] [varchar](50) NULL,
			[County] [varchar](50) NULL,
			[Tax Authority] [varchar](50) NULL,
			[Geocode] [varchar](50) NULL,
			[Price] [varchar](50) NULL,
			[Tax Type] [varchar](50) NULL,
			[Tax Category] [varchar](50) NULL,
			[Description] [varchar](255) NULL,
			[Revenue] [varchar](50) NULL,
			[Taxes] [varchar](50) NULL,
			[Tax Rate] varchar(50) NULL,
			[Line Count] varchar(50) NULL
		) ON [PRIMARY]

	delete from [dbo].HostedTempRedRockTaxes;

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempRedRockTaxes FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = 2, FIELDTERMINATOR = ''\t'', ROWTERMINATOR = ''\n'')'
	print @billdate
	EXEC sp_executesql @BulkInsert;

-- The following check to make sure the file we are importing has the correct bill date is commented because this RR file does not have a date.
-- If we get a new file version that has a date, we will reinstate this

    --if not exists (select top 1 Datepart(month, [Bill Date]) m, datepart(year, [Bill Date]) y
    --             from HostedTempRedRockTaxes 
    --            where  Datepart(month, [Bill Date]) <> datepart(month, @BillDate) and datepart(year, [Bill Date]) <> datepart(year, @BillDate))
    --   begin
    --        print 'Good Date'
			insert into [dbo].[HostedImportTaxes] (Customer, MasterBTN, [Level], LevelType, Jurisdiction,  Rate, TaxType, Title, TaxAmount, BillDate, [Sign])
			 select [Account Number] as Customer,
					null as MasterBTN,
					0 as [Level],
					Geocode as LevelType,
					CASE WHEN GEOCODE = 'STATE' then [State] WHEN GEOCODE = 'County' THEN [County] ELSE [Tax Authority] END Jurisdiction, 
					[Tax Rate] Rate, 
					null [Tax Type],
					[Description] Title,
					Taxes as TaxAmount,
					@BillDate as BillDate,
					Null as Sign
			   from [dbo].HostedTempRedRockTaxes;
			   
			--delete from [dbo].HostedTempRedRockTaxes;   
    --   end
    --else
    --   begin
    --        print 'Bad Date'
    --   end

END
GO
-- =============================================
-- Author:		Larry Axon
-- Create date: 7/22/2017
-- Description:	Import tab delimited format toll retail import to existing table (Red Rock)
-- =============================================
--exec [dbo].[ImportHostedRedRockTollRetail] 'C:\Data\Red Rock Imports\2017\2017-07\Retail Toll Red Rock-201707.txt', '2017','7'
CREATE PROCEDURE [dbo].[ImportHostedRedRockTollRetail]	(@FilePath nvarchar (255),@BillingYear varchar(10),@BillingMonth varchar(10))
	
AS
BEGIN
/*
AcctNum	TN	Billig Year	Billing Month	Billed TN	Call Date	Connect Time	Other TN	Other City	Other State	Rate	Minutes	Charge
R1180	5863158102	2017	5	5863158102	4/3/2017	01:30.3	5867261553	UTICA	MI	0.04	1.4	0
*/

	if object_id (N'"HostedTempRedRockTollRetail"', N'U') is NULL
		CREATE TABLE [dbo].[HostedTempRedRockTollRetail](
			[AcctNum] [varchar](50) NULL,
			[TN] [varchar](50) NULL,
			[Billig Year] [varchar](50) NULL,
			[Billing Month] [varchar](50) NULL,
			[Billed TN] [varchar](50) NULL,
			[Call Date] [varchar](50) NULL,
			[Connect Time] [varchar](50) NULL,
			[Other TN] [varchar](50) NULL,
			[Other City] [varchar](50) NULL,
			[Other State] [varchar](50) NULL,
			[Rate] [varchar](50) NULL,
			[Minutes] [varchar](50) NULL,
			[Charge] [varchar](50) NULL
		) ON [PRIMARY]

	delete from [dbo].[HostedTempRedRockTollRetail];

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempRedRockTollRetail FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = 2, FIELDTERMINATOR = ''\t'', ROWTERMINATOR = ''\n'')'
	
	EXEC sp_executesql @BulkInsert;
	
    if not exists (select top 1 [Billig Year],[Billing Month] 
                 from HostedTempRedRockTollRetail 
                where [Billig Year] <> @billingyear or [Billing Month] <> @BillingMonth)
       begin
            print 'Good Date'
			insert into hostedimporttollretail (customernumber,billingyear,billingmonth,btn,messagetype,fromnumber,calldate,duration,
					tonumber,tocity,tostate,rate,charge)
			 select [AcctNum] customernumber,
					[Billig Year] BillingYear,
					[Billing Month] billingmonth,
					replace(replace(replace(replace([Billed TN],' ',''),'(',''),')',''),'-','') as btn,
					null messagetype,
					replace(replace(replace(replace([TN],' ',''),'(',''),')',''),'-','') as fromnumber,
					[Call Date] calldate,
					[Minutes] duration,
					replace(replace(replace(replace([Other TN],' ',''),'(',''),')',''),'-','') as tonumber,
					[Other City] tocity,
					[Other State] tostate,
					rate,
					charge
			   from HostedTempRedRockTollRetail;
			   
			delete from HostedTempRedRockTollRetail;   
       end
    else
       begin
            print 'Bad Date'
       end

END
GO
-- =============================================
-- Author:		Larry Axon
-- Create date: 7/22/2017
-- Description:	Import tab delimited format toll Wholesale import to existing table (Red Rock)
-- =============================================
--exec [dbo].[ImportHostedRedRockTollWholesale] 'C:\Data\Red Rock Imports\2017\2017-07\Wholesale Toll Red Rock-201707.txt', '2017','7'
create PROCEDURE [dbo].[ImportHostedRedRockTollWholesale]	(@FilePath nvarchar (255),@BillingYear varchar(10),@BillingMonth varchar(10))
	
AS
BEGIN
/*h
AcctNum	TN	Billig Year	Billing Month	Billed TN	Call Date	Connect Time	Other TN	Other City	Other State	Rate	Minutes	Charge
R1180	5863158102	2017	5	5863158102	4/3/2017	01:30.3	5867261553	UTICA	MI	0.04	1.4	0
*/
/*
Account Number Customer id	billed tn	billed description	RedRock Charge	Redrock Reseller	Billed Date	other tn	other description	call Date
*/
	if object_id (N'"HostedTempRedRockTollWholesale"', N'U') is NULL
		CREATE TABLE [dbo].[HostedTempRedRockTollWholesale](
			[Account Number] [varchar](50) NULL,
			[Customer id] [varchar](50) NULL,
			[Billed TN] [varchar](50) NULL,
			[billed description] varchar(200) NULL,
			[RedRock Charge] money NULL,
			[Redrock Reseller] money NULL,
			[Billed Date] datetime null,
			[Other TN] [varchar](50) NULL,
			[Other Description] [varchar](50) NULL,
			[Call Date] datetime null
		) ON [PRIMARY]

	delete from [dbo].[HostedTempRedRockTollWholesale];

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempRedRockTollWholesale FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = 2, FIELDTERMINATOR = ''\t'', ROWTERMINATOR = ''\n'')'
	
	EXEC sp_executesql @BulkInsert;
	return
    if not exists (select top 1 [Billed Date]
                 from HostedTempRedRockTollWholesale 
                where datepart(year, [Billed Date]) <> @billingyear or datepart(month, [Billed Date]) <> @BillingMonth)
       begin
            print 'Good Date'
			insert into hostedimporttollWholesale (customernumber,billingyear,billingmonth,btn,messagetype,fromnumber,calldate,duration,
					tonumber,tocity,tostate,rate,charge)
			 select [Account Number] customernumber,
					datepart(year, [Billed Date]) BillingYear,
					datepart(month, [Billed Date]) billingmonth,
					replace(replace(replace(replace([Billed TN],' ',''),'(',''),')',''),'-','') as btn,
					null messagetype,
					replace(replace(replace(replace([Billed TN],' ',''),'(',''),')',''),'-','') as fromnumber,
					[Call Date] calldate,
					0 duration,
					replace(replace(replace(replace([Other TN],' ',''),'(',''),')',''),'-','') as tonumber,
					[Other Description] tocity,
					null tostate,
					0 rate,
					[RedRock Charge] charge
			   from HostedTempRedRockTollWholesale;
			   
			delete from HostedTempRedRockTollWholesale;   
       end
    else
       begin
            print 'Bad Date'
       end

END
GO
