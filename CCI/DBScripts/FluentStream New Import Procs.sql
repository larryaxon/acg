/***************************************************************************************

Updated Import Stored Procedures for FluentStream new Imports

***************************************************************************************/

/****** Object:  StoredProcedure [dbo].[ImportHostedMRCRetail]    Script Date: 5/18/2021 12:26:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- tweak HostedImportTaxes for compatibility
ALTER TABLE Hostedimporttaxes
alter column TaxType nvarchar(1024) Null


ALTER TABLE Hostedimporttaxes
alter column [Title] nvarchar(256) Null


/****** Object:  StoredProcedure [dbo].[ImportHostedLedger]    Script Date: 5/21/2021 9:37:44 AM ******/
DROP PROCEDURE [dbo].[ImportHostedLedger]
GO

-- =============================================
-- Author:		Larry Axon
-- Create date: 5/20/2021
-- Description:	Import csv format ledger import to existing table
-- =============================================
--exec [dbo].[ImportHostedTax] 'C:\Data\Saddleback Imports\2021\2021-05\Completed\Tax-202105.csv', '202105'
CREATE PROCEDURE [dbo].[ImportHostedLedger]	(@FilePath nvarchar (255),@BillDate varchar(10), @FirstRow int = 2)
	
AS
BEGIN
/*
Transaction_id	uomID	Tax_Type_Code	Tax_Type	State	County	City	Zip	Sales_Type	Net_Amount	Tot_Tax	Gross_Amount	Account_Number	Invoice_Number	Bill_Number	DocCode	CREATED_DATE	TotalGrossRevenue	TotalTax	TotalRevenue	BundleName	ProductName	External_Id	Country	Plan_Name	Identifier	Charge_From	Charge_To	Bill_Start_Date	Bill_End_Date
4DEF4F7E-AA4E-11EB-942D-D4DF72495A0E	1.37E+18	126	State Universal Service Fund-Toll	AZ	MARICOPA	PHOENIX	85029	1002	0.222704	0.000704	0.2	17867	IN-80008213429	BI23136		May 1, 2021, 12:24 AM	0.2	0.022704	0.222704		DID Numbers-Retail/DID Numbers	1956	UNITED STATES	DID Numbers	SUB7674	5/1/21	5/31/21	4/1/21	4/30/21
*/
--DROP TABLE HostedTempTax
--Truncate Table HostedTempTax
if object_id (N'"HostedTempLedger"', N'U') is NULL
	CREATE TABLE [dbo].[HostedTempLedger](
		[Account Number_1] [varchar](50) NULL,
		[Subscriber Name] [varchar](50) NULL,
		[Invoice Number] [varchar](50) NULL,
		[Billing Period Start] [varchar](50) NULL,
		[Billing Period End] [varchar](50) NULL,
		[Created Date] [varchar](50) NULL,
		[Notified Date] [varchar](50) NULL,
		[Due Date] [varchar](50) NULL,
		[Last Payment Date] [varchar](50) NULL,
		[Collection Entry Date] [varchar](50) NULL,
		[Status] [varchar](50) NULL,
		[Currency] [varchar](50) NULL,
		[Recurring charges] [varchar](50) NULL,
		[Onetime charges] [varchar](50) NULL,
		[Usage charges] [varchar](50) NULL,
		[Adjustment] [varchar](50) NULL,
		[Payment] [varchar](50) NULL,
		[Late Fees] [varchar](50) NULL,
		[Other Charges   Credits] [varchar](50) NULL,
		[Taxes] [varchar](50) NULL,
		[Invoice Amount] [varchar](50) NULL,
		[Current Due] [varchar](50) NULL,
		[Seller Name] [varchar](50) NULL,
		[External_Id] [varchar](50) NULL,
		[Account Status] [varchar](50) NULL,
		[Billing Charge From] [varchar](50) NULL,
		[Billing Charge To] [varchar](50) NULL
	) ON [PRIMARY]

	

	delete from [dbo].HostedTempLedger;

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempLedger FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = ' + Convert(varchar(5), @FirstRow) + ', FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'
	
	EXEC sp_executesql @BulkInsert;

	INSERT into [dbo].[HostedImportLedger] ([Customer]
      ,[CustomerName]
      ,[TransactionDate]
      ,[Title]
      ,[Amount]
      ,[BillDate]
      ,[MatchedBy]
      ,[MatchedDateTime])
	SELECT * from (
		Select [Account Number_1] AS Customer,
			   [Subscriber Name] as CustomrName,
			   Convert(date,[Created Date]) as [TransactionDate],
			   'Invoice' AS [Title],
				   convert(decimal(10,2),[Recurring charges] ) +
					convert(decimal(10,2),[Onetime charges] ) +
					convert(decimal(10,2),[Usage charges] ) +
					convert(decimal(10,2),[Adjustment] ) +
					convert(decimal(10,2),[Late Fees] ) +
					convert(decimal(10,2),[Other Charges   Credits] ) +
					convert(decimal(10,2),[Taxes] ) 
				AS Amount,
				Convert(date,Convert(varchar(2),Month(Convert(date, [Billing Charge To]))) + '/01/' + Convert(varchar(4), Year(Convert(date, [Billing Charge To])))) AS BillDate,
				null MatchedBy,
				null MatchedDateTime
			FROM [dbo].HostedTempLedger
		) ledger
		WHERE ledger.BillDate = @BillDate and Amount <> 0
			

			   
		delete from [dbo].HostedTempLedger;   



END


GO

/****** Object:  StoredProcedure [dbo].[ImportHostedMRCRetail]    Script Date: 5/21/2021 10:00:10 AM ******/
DROP PROCEDURE [dbo].[ImportHostedMRCRetail]
GO



-- =============================================
-- Author:		Larry Axon
-- Create date: 9/19/2015
-- Description:	Import csv format mrc retail import to existing table
-- =============================================
--exec [dbo].[ImportHostedMRCRetail] 'C:\Users\Larry\Documents\CCI\Saddleback\MRC Retail-20150901.csv', '20150901'
CREATE PROCEDURE [dbo].[ImportHostedMRCRetail]	(@FilePath nvarchar (255),@BillDate varchar(10), @FirstRow int = 7)
	
AS
BEGIN
/*
Cust Acct Nr	Service Name	Billing Acct Nr	Working Acct Nr	USOC	Title	Qty	Price	Amt Billed	Conn Date	Bill Date	Bus/Res
											
1156	1 WORLD MEDICINE	702 445-7031	702 445-7031	NMF	Network Maintenance Assessment	6	0.5	3	20140819	20150901	B

*/

	if object_id (N'"HostedTempMRCRetail"', N'U') is NULL
		CREATE TABLE [dbo].[HostedTempMRCRetail](
			[Cust Acct Nr] [nvarchar](25) NULL,
			[Service Name] [nvarchar](255) NULL,
			[Billing Acct Nr] [nvarchar](25) NULL,
			[Working Acct Nr] [nvarchar](25) NULL,
			[USOC] [nvarchar](25) NULL,
			[Title] [nvarchar](255) NULL,
			[Qty] int NULL,
			[Price] decimal(12,2) NULL,
			[Amt Billed] decimal(12,2) NULL,
			[Conn Date] [nvarchar](25) NULL,
			[Bill Date] [nvarchar](25) NULL,
			[Bus/Res] [nvarchar](5) NULL
		) ON [PRIMARY];

	delete from [dbo].[HostedTempMRCRetail];

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempMRCRetail FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = ' + Convert(varchar(5), @FirstRow) + ', FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'
	print @BulkInsert
	EXEC sp_executesql @BulkInsert;
    --if not exists (select top 1 [Bill Date]
    --             from HostedTempMRCRetail 
    --            where [Bill Date] <> @BillDate)
    --   begin
            print 'Good Date'
			insert into [dbo].[HostedImportMRCRetail] (customernumber,CustomerName, MasterBTN, BTN, USOC, [Product Description], Qty, Price, Amount, ConnectionDate, BillDate)
			 select * FROM (
				Select 
					[Cust Acct Nr] as customernumber,
					[Service Name] as CustomerName,
					replace(replace(replace(replace([Billing Acct Nr],' ',''),'(',''),')',''),'-','') as MasterBTN,
					replace(replace(replace(replace([Working Acct Nr],' ',''),'(',''),')',''),'-','') as BTN,
					USOC,
					[Title] as [Product Description], 
					Qty, 
					Price,
					[Amt Billed] as Amount,
					Convert(datetime, substring([Conn Date], 5,2) + '/' + substring([Conn Date], 7,2) + '/' + substring([Conn Date], 1, 4)) as ConnectionDate,
					Convert(datetime, substring([Bill Date], 5,2) + '/' + substring([Bill Date], 7,2) + '/' + substring([Bill Date], 1, 4)) as BillDate
			   from [dbo].[HostedTempMRCRetail]) mrc
			WHERE BillDate = @BillDate;
			   
			delete from [dbo].[HostedTempMRCRetail];   


END


GO

/****** Object:  StoredProcedure [dbo].[ImportHostedMRCWholesale]    Script Date: 5/21/2021 10:02:40 AM ******/
DROP PROCEDURE [dbo].[ImportHostedMRCWholesale]
GO
 =============================================
-- Author:		Larry Axon
-- Create date: 9/19/2015
-- Description:	Import csv format mrc Wholesale import to existing table
-- =============================================
--exec [dbo].[ImportHostedMRCWholesale] 'C:\Users\Larry\Documents\CCI\Saddleback\MRC WHLSL-20150901.csv', '20150901'
CREATE PROCEDURE [dbo].[ImportHostedMRCWholesale]	(@FilePath nvarchar (255),@BillDate varchar(10), @FirstRow int = 7)
	
AS
BEGIN
/*
Cust Acct Nr	Service Name	Billing Acct Nr	Working Acct Nr	USOC	Title	Qty	Price	Amt Billed	Conn Date	Bill Date	Bus/Res
											
80008213	1 World Medicine	500 000-0014	725 222-4430	WNIP11	IP Station Line w/LD	1	10	10	20140818	20150901	B


*/

	if object_id (N'"HostedTempMRCWholesale"', N'U') is NULL
		CREATE TABLE [dbo].[HostedTempMRCWholesale](
			[Cust Acct Nr] [nvarchar](25) NULL,
			[Service Name] [nvarchar](255) NULL,
			[Billing Acct Nr] [nvarchar](25) NULL,
			[Working Acct Nr] [nvarchar](25) NULL,
			[USOC] [nvarchar](25) NULL,
			[Title] [nvarchar](255) NULL,
			[Qty] int NULL,
			[Price] decimal(12,2) NULL,
			[Amt Billed] decimal(12,2) NULL,
			[Conn Date] [nvarchar](25) NULL,
			[Bill Date] [nvarchar](25) NULL,
			[Bus/Res] [nvarchar](5) NULL
		) ON [PRIMARY];

	delete from [dbo].[HostedTempMRCWholesale];

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempMRCWholesale FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = ' + convert(varchar(5), @FirstRow) + ', FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'
	
	EXEC sp_executesql @BulkInsert;


            print 'Good Date'
			insert into [dbo].[HostedImportMRCWholesale] (customernumber,CustomerName, MasterBTN, BTN, USOC, [Product Description], Qty, Price, Amount, ConnectionDate, BillDate)
			 select * FROM (
				SELECT
					[Cust Acct Nr] as customernumber,
					[Service Name] as CustomerName,
					replace(replace(replace(replace([Billing Acct Nr],' ',''),'(',''),')',''),'-','') as MasterBTN,
					replace(replace(replace(replace([Working Acct Nr],' ',''),'(',''),')',''),'-','') as BTN,
					USOC,
					[Title] as [Product Description], 
					Qty, 
					Price,
					[Amt Billed] as Amount,
					Convert(datetime, substring([Conn Date], 5,2) + '/' + substring([Conn Date], 7,2) + '/' + substring([Conn Date], 1, 4)) as ConnectionDate,
					Convert(datetime, substring([Bill Date], 5,2) + '/' + substring([Bill Date], 7,2) + '/' + substring([Bill Date], 1, 4)) as BillDate
			   from [dbo].[HostedTempMRCWholesale]
			   ) mrc
			   WHERE mrc.BillDate = @BillDate
			   
			delete from [dbo].[HostedTempMRCWholesale];   


END

GO


/****** Object:  StoredProcedure [dbo].[ImportHostedOCCRetail]    Script Date: 5/21/2021 10:08:01 AM ******/
DROP PROCEDURE [dbo].[ImportHostedOCCRetail]
GO



-- =============================================
-- Author:		Larry Axon
-- Create date: 9/19/2015
-- Description:	Import csv format OCC retail import to existing table
-- =============================================
--exec [dbo].[ImportHostedOCCRetail] 'C:\Users\Larry\Documents\CCI\Saddleback\OCC Retail-20150901.csv', '20150901'
CREATE PROCEDURE [dbo].[ImportHostedOCCRetail]	(@FilePath nvarchar (255),@BillDate date, @FirstRow int = 5)
	
AS
BEGIN
/*
Cust Acct Nr	Service Name	Billing Acct Nr	Working Acct Nr	USOC	Title	Qty	Price	Amt Billed	Conn Date	Bill Date	Bus/Res
											
1156	1 WORLD MEDICINE	702 445-7031	702 445-7031	NMF	Network Maintenance Assessment	6	0.5	3	20140819	20150901	B

*/

	if object_id (N'"HostedTempOCCRetail"', N'U') is NULL
		CREATE TABLE [dbo].[HostedTempOCCRetail](
			[Customer] varchar(25) NULL
      ,[MasterBTN] varchar(25) NULL
      ,[BTN] varchar(25) NULL
      ,[Date] datetime
      ,[Amount] float
      ,[Code]  varchar(25) NULL
      ,[USOC]  varchar(25) NULL
      ,[Service]  varchar(1024) NULL
      ,[BillDate] varchar(25)
		) ON [PRIMARY];

	delete from [dbo].[HostedTempOCCRetail];

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempOCCRetail FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = ' + convert(varchar(5), @FirstRow) + ', FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'
	
	EXEC sp_executesql @BulkInsert;
    if not exists (select top 1 BillDate
                 from HostedTempOCCRetail 
                where BillDate <> @BillDate)
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
			 select * FROM (
				SELECT [Customer] as Customer,
					replace(replace(replace(replace([MasterBTN],' ',''),'(',''),')',''),'-','') as MasterBTN,
					replace(replace(replace(replace([BTN],' ',''),'(',''),')',''),'-','') as BTN,
					[Date], 
					Amount, 
					Code,
					USOC,
					Service,
					Convert(date, substring(BillDate,5,2) + '/' + SUBSTRING(BillDate, 7,2) + '/' + SUBSTRING(BillDate, 1, 4)) BillDate
			   from [dbo].[HostedTempOCCRetail]
			   WHERE isnull(USOC, '') != '' -- filter out the FS/CCI interco charges
			   ) occ
			   WHERE BillDate = @BillDate
			   
			delete from [dbo].[HostedTempOCCRetail];   
       end
    else
       begin
            print 'Bad Date'
       end

END


GO

/****** Object:  StoredProcedure [dbo].[ImportHostedOCCWholesale]    Script Date: 5/21/2021 10:11:39 AM ******/
DROP PROCEDURE [dbo].[ImportHostedOCCWholesale]
GO

-- =============================================
-- Author:		Larry Axon
-- Create date: 9/19/2015
-- Description:	Import csv format OCC Wholesale import to existing table
-- =============================================
--exec [dbo].[ImportHostedOCCWholesale] 'C:\Users\Larry\Documents\CCI\Saddleback\OCC Wholesale-20150901.csv', '20150901'
CREATE PROCEDURE [dbo].[ImportHostedOCCWholesale]	(@FilePath nvarchar (255),@BillDate date, @FirstRow int = 5)
	
AS
BEGIN
/*
Cust Acct Nr	Service Name	Billing Acct Nr	Working Acct Nr	USOC	Title	Qty	Price	Amt Billed	Conn Date	Bill Date	Bus/Res
											
1156	1 WORLD MEDICINE	702 445-7031	702 445-7031	NMF	Network Maintenance Assessment	6	0.5	3	20140819	20150901	B

*/

	if object_id (N'"HostedTempOCCWholesale"', N'U') is NULL
		CREATE TABLE [dbo].[HostedTempOCCWholesale](
			[Customer] varchar(25) NULL
      ,[MasterBTN] varchar(25) NULL
      ,[BTN] varchar(25) NULL
      ,[Date] datetime
      ,[Amount] float
      ,[Code]  varchar(25) NULL
      ,[USOC]  varchar(25) NULL
      ,[Service]  varchar(1024) NULL
      ,[BillDate] varchar(25) NULL
      ,[ChargeType] varchar(25) NULL
		) ON [PRIMARY];

	delete from [dbo].[HostedTempOCCWholesale];

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempOCCWholesale FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = ' + convert(varchar(5), @FirstRow) + ', FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'
	
	EXEC sp_executesql @BulkInsert;
	 
    if not exists (select top 1 BillDate
                 from HostedTempOCCWholesale 
                where BillDate <> @BillDate)
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
			 select * from (
				SELECT [Customer] as Customer,
					replace(replace(replace(replace([MasterBTN],' ',''),'(',''),')',''),'-','') as MasterBTN,
					replace(replace(replace(replace([BTN],' ',''),'(',''),')',''),'-','') as BTN,
					[Date], 
					Amount, 
					Code as OCCCode,
					USOC,
					Service,
					Convert(date, substring(BillDate,5,2) + '/' + SUBSTRING(BillDate, 7,2) + '/' + SUBSTRING(BillDate, 1, 4)) BillDate			   
					from [dbo].[HostedTempOCCWholesale]
			   WHERE isnull(USOC, '') != '' -- filter out the FS/CCI interco charges
			   ) occ
			   WHERE occ.BillDate = @BillDate
			   
			delete from [dbo].[HostedTempOCCWholesale];   
       end
    else
       begin
            print 'Bad Date'
       end

END





GO

/****** Object:  StoredProcedure [dbo].[ImportHostedTollRetail]    Script Date: 5/21/2021 10:16:30 AM ******/
DROP PROCEDURE [dbo].[ImportHostedTollRetail]
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ImportHostedTollRetail]	(@FilePath nvarchar (255),@BillingYear varchar(10),@BillingMonth varchar(10), @FirstRow int = 5)
	
AS
BEGIN


	if object_id (N'"hostedtemptollretail"', N'U') is NULL
		CREATE TABLE [dbo].[HostedTempTollRetail](
			[CustomerNumber] [nvarchar](25) NULL,
			[BTN] [nvarchar](25) NULL,
			[BillingYear] [nvarchar](10) NULL,
			[BillingMonth] [nvarchar](10) NULL,
			[MessageType] [nvarchar](25) NULL,
			[FromNumber] [nvarchar](25) NULL,
			[CallDate] [datetime] NULL,
			[CallTime] [nvarchar] (20) NULL,
			[ToNumber] [nvarchar](25) NULL,
			[ToCity] [nvarchar](25) NULL,
			[ToState] [nvarchar](25) NULL,
			[Rate] [nvarchar](25) NULL,
			[Duration] [float] NULL,
			[Charge] [float] NULL,
		) ON [PRIMARY];

	delete from [dbo].[HostedTempTollRetail];

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempTollRetail FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = ' + convert(varchar(5), @FirstRow) + ', FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'
	
	EXEC sp_executesql @BulkInsert;

	insert into hostedimporttollretail (customernumber,billingyear,billingmonth,btn,messagetype,fromnumber,calldate,duration,
			tonumber,tocity,tostate,rate,charge)
	select * FROM (
		SELECT
			customernumber,billingyear,billingmonth,
			replace(replace(replace(replace(btn,' ',''),'(',''),')',''),'-','') as btn,
			messagetype,replace(replace(replace(replace(fromnumber,' ',''),'(',''),')',''),'-','') as fromnumber,calldate,
			duration,replace(replace(replace(replace(tonumber,' ',''),'(',''),')',''),'-','') as tonumber,tocity,tostate,
			rate,charge
		from HostedTempTollRetail
		) toll
	WHERE toll.BillingMonth = @BillingMonth and toll.BillingYear = @BillingYear
			   
	delete from hostedtemptollretail;   


END


GO

/****** Object:  StoredProcedure [dbo].[ImportHostedTollWholesale]    Script Date: 5/21/2021 10:19:49 AM ******/
DROP PROCEDURE [dbo].[ImportHostedTollWholesale]
GO

--=========================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- DROP TABLE [dbo].[HostedTempTollWholesale]
CREATE PROCEDURE [dbo].[ImportHostedTollWholesale] (@FileName nvarchar (255),@BillingYear varchar (10),@BillingMonth varchar (10), @FirstRow int = 2)
AS
BEGIN
	
	if object_id (N'"hostedtemptollwholesale"', N'U') is NULL
		CREATE TABLE [dbo].[HostedTempTollWholesale](
			[BillingMonth] [nvarchar](10) NULL,
			[BillingYear] [nvarchar](10) NULL,
			[BTN] [nvarchar](25) NULL,
			[CallNumber] [float]  NULL,
			[Charge] [float] NULL,
			[CustomerNumber] [nvarchar](25) NULL,
			[CallDate] [datetime] NULL,
			[Duration] [float] NULL,
			[FromNumber] [nvarchar](25) NULL,
			[FromState] [nvarchar](25) NULL,
			[CarrierID] [nvarchar](25) NULL,
			[MessageType] [nvarchar](25) NULL,
			[OCPID] [nvarchar] (25) NULL,
			[Rate] [nvarchar](25) NULL,
			[RateClass] [nvarchar](25) NULL,
			[SettlementCode] [nvarchar](255) NULL,
			[ToCity] [nvarchar](25) NULL,
			[ToReference] [nvarchar](25) NULL,
			[ToNumber] [nvarchar](25) NULL,
			[ToState] [nvarchar](25) NULL,
			[UsageAccountCode] [nvarchar] (20) NULL
		) ON [PRIMARY];

	delete from [HostedTempTollWholesale];

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FileName;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempTollWholesale FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = ' + convert(varchar(5), @FirstRow) + ', FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'
	
	EXEC sp_executesql @BulkInsert;

    --if not exists (select top 1 billingyear,billingmonth 
    --             from hostedtemptollwholesale 
    --            where billingyear <> @BillingYear or billingmonth <> @BillingMonth)
    --   begin
  	insert into hostedimporttollwholesale (billingmonth,billingyear,btn,callnumber,charge,customernumber,calldate,duration,
			fromnumber,fromstate,carrierid,messagetype,ocpid,rate,rateclass,settlementcode,tocity,toreference,tonumber,tostate,
			usageaccountcode)
	select * FROM (
		SELECT billingmonth,billingyear,replace(replace(replace(replace(btn,' ',''),'(',''),')',''),'-','') as btn,callnumber,charge,
				customernumber,calldate,duration,replace(replace(replace(replace(fromnumber,' ',''),'(',''),')',''),'-','') as fromnumber,
				fromstate,carrierid,messagetype,ocpid,rate,rateclass,settlementcode,tocity,toreference,
				replace(replace(replace(replace(tonumber,' ',''),'(',''),')',''),'-','') as tonumber,tostate,usageaccountcode
		from HostedTempTollWholesale
		) toll
	WHERE toll.BillingMonth = @BillingMonth and toll.BillingYear = @BillingYear;

  	delete from dbo.hostedtemptollwholesale;   


END



GO





/****** Object:  StoredProcedure [dbo].[ImportHostedTax]    Script Date: 5/21/2021 10:13:41 AM ******/
DROP PROCEDURE [dbo].[ImportHostedTax]
GO

-- =============================================
-- Author:		Larry Axon
-- Create date: 9/19/2015
-- Description:	Import csv format mrc retail import to existing table
-- =============================================
--exec [dbo].[ImportHostedTax] 'C:\Data\Saddleback Imports\2021\2021-05\Completed\Tax-202105.csv', '202105'
CREATE PROCEDURE [dbo].[ImportHostedTax]	(@FilePath nvarchar (255),@BillDate varchar(10), @FirstRow int = 2)
	
AS
BEGIN
/*
Transaction_id	uomID	Tax_Type_Code	Tax_Type	State	County	City	Zip	Sales_Type	Net_Amount	Tot_Tax	Gross_Amount	Account_Number	Invoice_Number	Bill_Number	DocCode	CREATED_DATE	TotalGrossRevenue	TotalTax	TotalRevenue	BundleName	ProductName	External_Id	Country	Plan_Name	Identifier	Charge_From	Charge_To	Bill_Start_Date	Bill_End_Date
4DEF4F7E-AA4E-11EB-942D-D4DF72495A0E	1.37E+18	126	State Universal Service Fund-Toll	AZ	MARICOPA	PHOENIX	85029	1002	0.222704	0.000704	0.2	17867	IN-80008213429	BI23136		May 1, 2021, 12:24 AM	0.2	0.022704	0.222704		DID Numbers-Retail/DID Numbers	1956	UNITED STATES	DID Numbers	SUB7674	5/1/21	5/31/21	4/1/21	4/30/21
*/
--DROP TABLE HostedTempTax
--Truncate Table HostedTempTax
if object_id (N'"HostedTempTax"', N'U') is NULL
Create Table HostedTempTax (
	Transaction_id	nvarchar(50) NULL,
	uomID	nvarchar(50) NULL,
	Tax_Type_Code	nvarchar(50) NULL,
	Tax_Type	nvarchar(1024) NULL,
	[State]	nvarchar(50) NULL,
	County	nvarchar(256) NULL,
	City	nvarchar(50) NULL,
	Zip	nvarchar(50) NULL,
	Sales_Type	nvarchar(50) NULL,
	Net_Amount	nvarchar(50) NULL,
	Tot_Tax	nvarchar(50) NULL,
	Gross_Amount	nvarchar(50) NULL,
	Account_Number	nvarchar(50) NULL,
	Invoice_Number	nvarchar(50) NULL,
	Bill_Number	nvarchar(50) NULL,
	DocCode	nvarchar(50) NULL,
	CREATED_DATE	nvarchar(128) NULL,
	TotalGrossRevenue	nvarchar(50) NULL,
	TotalTax	nvarchar(50) NULL,
	TotalRevenue	nvarchar(50) NULL,
	BundleName	nvarchar(50) NULL,
	ProductName	nvarchar(256) NULL,
	External_Id	nvarchar(50) NULL,
	Country	nvarchar(256) NULL,
	Plan_Name	nvarchar(256) NULL,
	Identifier	nvarchar(50) NULL,
	Charge_From	nvarchar(256) NULL,
	Charge_To	nvarchar(256)  NULL,
	Bill_Start_Date nvarchar(50) NULL,
	Bill_End_Date	nvarchar(256) NULL)
	ON [PRIMARY];
	

	delete from [dbo].HostedTempTax;

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempTax FROM ''' +
		@ImportFileName +
		N''' WITH (FORMATFILE=''C:\Data\FluentStreamTaxFormatFile.xml'', FIRSTROW = ' + Convert(varchar(5), @FirstRow) + ', FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'
	
	EXEC sp_executesql @BulkInsert;



	insert into [dbo].[HostedImportTaxes] ([Customer]
      ,[MasterBTN]
      ,[Level]
      ,[LevelType]
      ,[Jurisdiction]
      ,[Rate]
      ,[TaxType]
      ,[Title]
      ,[TaxAmount]
      ,[BillDate]
      ,[Sign])
			 select * FROM
				(Select 
					Account_Number as [Customer],
					'' as MasterBTN,
					'' as Level,
					'' AS [LevelType],
					'' as [Jurisdiction],
					0 as [Rate],
					Tax_Type as TaxType,
					ProductName as Title,
					Tot_Tax as Amount,
					case when isdate(Charge_From) = 1 then Convert(date,Charge_From) else null end as ConnectionDate ,
					case when isdate(Charge_From) = 1 then Convert(date,Charge_From) else null end as BillDate
			   from [dbo].HostedTempTax
			   WHERE case when isdate(Charge_From) = 1 then Convert(date,Charge_From) else '1/1/1900' end between @BillDate and dateadd(month, 1, @BillDate)
			   ) tax
			WHERE tax.BillDate = @BillDate
			   
	delete from [dbo].HostedTempTax;   



END


GO









