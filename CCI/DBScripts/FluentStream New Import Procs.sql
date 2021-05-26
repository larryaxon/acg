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



/****** Object:  StoredProcedure [dbo].[ImportHostedLedger]    Script Date: 5/25/2021 7:37:45 PM ******/
DROP PROCEDURE [dbo].[ImportHostedLedger]
GO

- =============================================
-- Author:		Larry Axon
-- Create date: 5/20/2021
-- Description:	Import csv format ledger import to existing table
-- =============================================
--exec [dbo].[ImportHostedLedger] 'C:\Data\Saddleback Imports\2021\2021-05\Completed\Ledger-202105.csv', '5/1/2021'
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
	return
	INSERT into [dbo].[HostedImportLedger] ([Customer]
      ,[CustomerName]
      ,[TransactionDate]
      ,[Title]
      ,[Amount]
      ,[BillDate]
      ,[MatchedBy]
      ,[MatchedDateTime])
	SELECT * from (
		Select Coalesce(fs.ParentID, [Account Number_1]) AS Customer,
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
				@BillDate as BillDate,
				--Convert(date,Convert(varchar(2),Month(Convert(date, [Billing Charge To]))) + '/01/' + Convert(varchar(4), Year(Convert(date, [Billing Charge To])))) AS BillDate,
				null MatchedBy,
				null MatchedDateTime
			FROM [dbo].HostedTempLedger g
			LEFT JOIN vw_FluentStreamParentAccounts fs on [dbo].[fnRemoveLeadingZeros](g.[Account Number_1]) = fs.AccountID

		) ledger
		--WHERE ledger.BillDate = @BillDate and Amount <> 0
			

			   
		delete from [dbo].HostedTempLedger;   



END



GO



/****** Object:  StoredProcedure [dbo].[ImportHostedMRCRetail]    Script Date: 5/25/2021 7:36:21 PM ******/
DROP PROCEDURE [dbo].[ImportHostedMRCRetail]
GO



-- =============================================
-- Author:		Larry Axon
-- Create date: 9/19/2015
-- Description:	Import csv format mrc retail import to existing table
-- =============================================
--exec [dbo].[ImportHostedMRCRetail] 'C:\Data\Saddleback Imports\2021\2021-05\Completed\MRC Retail-202105.csv', '5/1/2021'
CREATE PROCEDURE [dbo].[ImportHostedMRCRetail]	(@FilePath nvarchar (255),@BillDate varchar(10), @FirstRow int = 3)
	
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
					Coalesce(fs.ParentID, [dbo].[fnRemoveLeadingZeros](right([Cust Acct Nr],8))) as customernumber,
					[Service Name] as CustomerName,
					replace(replace(replace(replace([Billing Acct Nr],' ',''),'(',''),')',''),'-','') as MasterBTN,
					replace(replace(replace(replace([Working Acct Nr],' ',''),'(',''),')',''),'-','') as BTN,
					USOC,
					[Title] as [Product Description], 
					Qty, 
					Price,
					[Amt Billed] as Amount,
					Convert(datetime, substring([Conn Date], 5,2) + '/' + substring([Conn Date], 7,2) + '/' + substring([Conn Date], 1, 4)) as ConnectionDate,
					@BillDate as BillDate
					--Convert(datetime, substring([Bill Date], 5,2) + '/' + substring([Bill Date], 7,2) + '/' + substring([Bill Date], 1, 4)) as BillDate
			   from [dbo].[HostedTempMRCRetail] r
			   LEFT JOIN vw_FluentStreamParentAccounts fs on [dbo].[fnRemoveLeadingZeros](right(r.[Cust Acct Nr],8)) = fs.AccountID
			   ) mrc

			   
			delete from [dbo].[HostedTempMRCRetail];   


END





GO


/****** Object:  StoredProcedure [dbo].[ImportHostedMRCWholesale]    Script Date: 5/25/2021 7:38:50 PM ******/
DROP PROCEDURE [dbo].[ImportHostedMRCWholesale]
GO

 --=============================================
-- Author:		Larry Axon
-- Create date: 9/19/2015
-- Description:	Import csv format mrc Wholesale import to existing table
-- =============================================
--exec [dbo].[ImportHostedMRCWholesale] 'C:\Data\Saddleback Imports\2021\2021-05\Completed\MRC WHLSL-202105.csv', '5/1/2021'
CREATE PROCEDURE [dbo].[ImportHostedMRCWholesale]	(@FilePath nvarchar (255),@BillDate varchar(10), @FirstRow int = 3)
	
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
	print @BulkInsert
	EXEC sp_executesql @BulkInsert;


            print 'Good Date'
			insert into [dbo].[HostedImportMRCWholesale] (customernumber,CustomerName, MasterBTN, BTN, USOC, [Product Description], Qty, Price, Amount, ConnectionDate, BillDate)
			 select * FROM (
				SELECT
					Coalesce(fs.ParentID, [dbo].[fnRemoveLeadingZeros](right([Cust Acct Nr],8))) as customernumber,
					[Service Name] as CustomerName,
					replace(replace(replace(replace([Billing Acct Nr],' ',''),'(',''),')',''),'-','') as MasterBTN,
					replace(replace(replace(replace([Working Acct Nr],' ',''),'(',''),')',''),'-','') as BTN,
					USOC,
					[Title] as [Product Description], 
					Qty, 
					Price,
					[Amt Billed] as Amount,
					Convert(datetime, substring([Conn Date], 5,2) + '/' + substring([Conn Date], 7,2) + '/' + substring([Conn Date], 1, 4)) as ConnectionDate,
					@BilLDate as BilLDate
					--Convert(datetime, substring([Bill Date], 5,2) + '/' + substring([Bill Date], 7,2) + '/' + substring([Bill Date], 1, 4)) as BillDate
			   from [dbo].[HostedTempMRCWholesale] w
			   LEFT JOIN vw_FluentStreamParentAccounts fs on [dbo].[fnRemoveLeadingZeros](right(w.[Cust Acct Nr],8)) = fs.AccountID
			   ) mrc
			   --WHERE mrc.BillDate = @BillDate
			   
			delete from [dbo].[HostedTempMRCWholesale];   


END



GO


/****** Object:  StoredProcedure [dbo].[ImportHostedOCCRetail]    Script Date: 5/25/2021 7:39:42 PM ******/
DROP PROCEDURE [dbo].[ImportHostedOCCRetail]
GO


-- =============================================
-- Author:		Larry Axon
-- Create date: 9/19/2015
-- Description:	Import csv format OCC retail import to existing table
-- =============================================
--exec [dbo].[ImportHostedOCCRetail] 'C:\Data\Saddleback Imports\2021\2021-05\Completed\OCC Retail-202105.csv', '5/1/2021'
CREATE PROCEDURE [dbo].[ImportHostedOCCRetail]	(@FilePath nvarchar (255),@BillDate date, @FirstRow int = 2)
	
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
	print @BulkInsert
	EXEC sp_executesql @BulkInsert;

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
				SELECT coalesce(fs.ParentID,[dbo].[fnRemoveLeadingZeros]([Customer])) as Customer,
					replace(replace(replace(replace([MasterBTN],' ',''),'(',''),')',''),'-','') as MasterBTN,
					replace(replace(replace(replace([BTN],' ',''),'(',''),')',''),'-','') as BTN,
					[Date], 
					Amount, 
					Code,
					USOC,
					Service,
					@BillDate as BillDate
					--Convert(date, substring(BillDate,5,2) + '/' + SUBSTRING(BillDate, 7,2) + '/' + SUBSTRING(BillDate, 1, 4)) BillDate
			   from [dbo].[HostedTempOCCRetail] r
			   LEFT JOIN vw_FluentStreamParentAccounts fs on [dbo].[fnRemoveLeadingZeros](r.[Customer]) = fs.AccountID
			   WHERE isnull(USOC, '') != '' -- filter out the FS/CCI interco charges
			   ) occ
			   --WHERE BillDate = @BillDate
			   
			delete from [dbo].[HostedTempOCCRetail];   


END




GO

/****** Object:  StoredProcedure [dbo].[ImportHostedOCCWholesale]    Script Date: 5/25/2021 7:40:48 PM ******/
DROP PROCEDURE [dbo].[ImportHostedOCCWholesale]
GO

-- =============================================
-- Author:		Larry Axon
-- Create date: 9/19/2015
-- Description:	Import csv format OCC Wholesale import to existing table
-- =============================================
--exec [dbo].[ImportHostedOCCWholesale] 'C:\Data\Saddleback Imports\2021\2021-05\Completed\OCC WHLSL-202105.csv', '5/1/2021'
CREATE PROCEDURE [dbo].[ImportHostedOCCWholesale]	(@FilePath nvarchar (255),@BillDate date, @FirstRow int = 2)
	
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
	print @BulkInsert

	EXEC sp_executesql @BulkInsert;
	 

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
				SELECT  coalesce(fs.ParentID,[dbo].[fnRemoveLeadingZeros]([Customer])) as Customer,
					replace(replace(replace(replace([MasterBTN],' ',''),'(',''),')',''),'-','') as MasterBTN,
					replace(replace(replace(replace([BTN],' ',''),'(',''),')',''),'-','') as BTN,
					[Date], 
					Amount, 
					Code as OCCCode,
					USOC,
					Service,
					@BilLDate as BillDate
					--Convert(date, substring(BillDate,5,2) + '/' + SUBSTRING(BillDate, 7,2) + '/' + SUBSTRING(BillDate, 1, 4)) BillDate			   
					from [dbo].[HostedTempOCCWholesale] w
					LEFT JOIN vw_FluentStreamParentAccounts fs on [dbo].[fnRemoveLeadingZeros](w.[Customer]) = fs.AccountID

			   WHERE isnull(USOC, '') != '' -- filter out the FS/CCI interco charges
			   ) occ
			   --WHERE occ.BillDate = @BillDate
			   
			delete from [dbo].[HostedTempOCCWholesale];   


END







GO

/****** Object:  StoredProcedure [dbo].[ImportHostedTollRetail]    Script Date: 5/25/2021 7:41:51 PM ******/
DROP PROCEDURE [dbo].[ImportHostedTollRetail]
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
--exec [dbo].[ImportHostedTollRetail]	'C:\Data\Saddleback Imports\2021\2021-05\Completed\Toll Retail-202105.csv', 2021, 5
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
	print @BulkInsert
	EXEC sp_executesql @BulkInsert;

	insert into hostedimporttollretail (customernumber,billingyear,billingmonth,btn,messagetype,fromnumber,calldate,duration,
			tonumber,tocity,tostate,rate,charge)
	select * FROM (
		SELECT
			Coalesce(fs.parentid, customernumber) as customernumber,
			@billingyear as BillingYear,@billingmonth as BillingMonth,
			replace(replace(replace(replace(btn,' ',''),'(',''),')',''),'-','') as btn,
			messagetype,replace(replace(replace(replace(fromnumber,' ',''),'(',''),')',''),'-','') as fromnumber,calldate,
			duration,replace(replace(replace(replace(tonumber,' ',''),'(',''),')',''),'-','') as tonumber,tocity,tostate,
			rate,charge
		from HostedTempTollRetail r
		LEFT JOIN vw_FluentStreamParentAccounts fs on [dbo].[fnRemoveLeadingZeros](r.customernumber) = fs.AccountID

		) toll
	--WHERE toll.BillingMonth = @BillingMonth and toll.BillingYear = @BillingYear
			   
	delete from hostedtemptollretail;   


END




GO

/****** Object:  StoredProcedure [dbo].[ImportHostedTollWholesale]    Script Date: 5/25/2021 7:42:45 PM ******/
DROP PROCEDURE [dbo].[ImportHostedTollWholesale]
GO


--=========================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
-- DROP TABLE [dbo].[HostedTempTollWholesale]
--exec [dbo].[ImportHostedTollWholesale] 'C:\Data\Saddleback Imports\2021\2021-05\Completed\Toll WHLSL-202105.csv', 2021, 5
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
	print @BulkInsert
	EXEC sp_executesql @BulkInsert;

  	insert into hostedimporttollwholesale (billingmonth,billingyear,btn,callnumber,charge,customernumber,calldate,duration,
			fromnumber,fromstate,carrierid,messagetype,ocpid,rate,rateclass,settlementcode,tocity,toreference,tonumber,tostate,
			usageaccountcode)
	select * FROM (
		SELECT @BillingMOnth as billingmonth,@BillingYear as billingyear,replace(replace(replace(replace(btn,' ',''),'(',''),')',''),'-','') as btn,callnumber,charge,
				coalesce(fs.ParentID, customernumber) as customernumber,
				calldate,duration,replace(replace(replace(replace(fromnumber,' ',''),'(',''),')',''),'-','') as fromnumber,
				fromstate,carrierid,messagetype,ocpid,rate,rateclass,settlementcode,tocity,toreference,
				replace(replace(replace(replace(tonumber,' ',''),'(',''),')',''),'-','') as tonumber,tostate,usageaccountcode
		from HostedTempTollWholesale tw
		LEFT JOIN vw_FluentStreamParentAccounts fs on [dbo].[fnRemoveLeadingZeros](tw.customernumber) = fs.AccountID

		) toll
	--WHERE toll.BillingMonth = @BillingMonth and toll.BillingYear = @BillingYear;

  	delete from dbo.hostedtemptollwholesale;   


END





GO



/****** Object:  StoredProcedure [dbo].[ImportHostedTax]    Script Date: 5/25/2021 7:43:22 PM ******/
DROP PROCEDURE [dbo].[ImportHostedTax]
GO

-- =============================================
-- Author:		Larry Axon
-- Create date: 9/19/2015
-- Description:	Import csv format mrc retail import to existing table
-- =============================================
--exec [dbo].[ImportHostedTax] 'C:\Data\Saddleback Imports\2021\2021-05\Completed\Tax-202105.csv', '5/1/2021'
-- select * from [HostedImportTaxes] where BillDate = '5/1/2021'
CREATE PROCEDURE [dbo].[ImportHostedTax]	(@FilePath nvarchar (255),@BillDate datetime, @FirstRow int = 2, @UseFormatFile bit = 0)
	
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
		N''' WITH (';
	if @UseFormatFile = 1		
		Set @BulkInsert = @BulkInsert + N'FORMATFILE=''C:\Data\FluentStreamTaxFormatFile.xml'', '
	Set @BulkInsert = @BulkInsert + N'FIRSTROW = ' + Convert(varchar(5), @FirstRow) + ', FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'
	
	print @BulkInsert
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
      ,[BillDate])
			 select * FROM
				(Select 
					Coalesce(fs.ParentID, Account_Number) as [Customer],
					'' as MasterBTN,
					'' as Level,
					'' AS [LevelType],
					'' as [Jurisdiction],
					0 as [Rate],
					Tax_Type as TaxType,
					ProductName as Title,
					Tot_Tax as Amount,
					@BilLDate as BillDate
			   from [dbo].HostedTempTax tx
			LEFT JOIN vw_FluentStreamParentAccounts fs on [dbo].[fnRemoveLeadingZeros](tx.Account_Number) = fs.AccountID

			   --WHERE case when isdate(Charge_From) = 1 then Convert(date,Charge_From) else '1/1/1900' end between @BillDate and dateadd(month, 1, @BillDate)
			   ) tax
			--WHERE tax.BillDate = @BillDate
			   
	delete from [dbo].HostedTempTax;   



END






GO






/****** Object:  StoredProcedure [dbo].[ProcessMRCDetails]    Script Date: 5/24/2021 12:32:25 PM ******/
DROP PROCEDURE [dbo].[ProcessMRCDetails]
GO

-- =============================================
-- Author:		<Andy Werdeman>
-- Create date: <11/28/2012>
-- Description:	<Process MRC Details>
-- Modified 2/7/2015 by Larry to use faster version of vw_CityHostedUsocs (vw_CHUsocs)
-- =============================================
-- exec ProcessMRCDetails 'LarryA', '5/1/2021'
-- 
CREATE PROCEDURE [dbo].[ProcessMRCDetails] (@operator nvarchar(25), @billdate date)
AS
BEGIN
	SET NOCOUNT ON;
--	Declare @previousmonth date
--	set @previousmonth = Dateadd(m,-1,@billdate)
	RAISERROR ('begin ProcessMRCDetails', 0, 1) WITH NOWAIT
	RAISERROR ('auto match city tolls', 0, 1) WITH NOWAIT
    update hostedimportmrcwholesale set matchedby = 'Auto:City', matcheddatetime = getdate()
     where customernumber = '80008154' and billdate = @billdate
	
	RAISERROR ('post from wholesale hostednomatchbtns', 0, 1) WITH NOWAIT
	update hostedimportmrcwholesale 
	set matchedby = 'Auto:Ex-BTN-USOC', matcheddatetime = getdate() 
     where matchedby is null and billdate = @billdate and (btn in (select btn from hostednomatchbtns) or
		   usoc in (select usoc from hostednomatchusocs where type = 'Wholesale'))
	RAISERROR ('post from no match usocs', 0, 1) WITH NOWAIT

	update hostedimportmrcwholesale set matchedby = 'Auto:Ex-BTN-USOC', matcheddatetime = getdate() 
	 from hostedimportmrcwholesale w WITH (NOLOCK)
     inner join hostednomatchbtnusoc nbu WITH (NOLOCK) on w.btn = nbu.btn and w.usoc = nbu.usoc
     where w.matchedby is null and w.billdate = @billdate

	RAISERROR ('post from retail nomatchbtns', 0, 1) WITH NOWAIT
	update hostedimportmrcretail
	set matchedby = 'Auto:Ex-BTN-USOC', matcheddatetime = getdate() 
     where matchedby is null and billdate = @billdate and (btn in (select btn from hostednomatchbtns) or
		   usoc in (select usoc from hostednomatchusocs where type = 'Retail'))

	RAISERROR ('update retail import', 0, 1) WITH NOWAIT
	update hostedimportmrcretail set matchedby = 'Auto:Ex-BTN-USOC ', matcheddatetime = getdate() 
	 from hostedimportmrcwholesale r WITH (NOLOCK)
     inner join hostednomatchbtnusoc nbu WITH (NOLOCK) on r.btn = nbu.btn and r.usoc = nbu.usoc
     where r.matchedby is null and r.billdate = @billdate
     
     
	RAISERROR ('Post Excluded to Matched table', 0, 1) WITH NOWAIT
	RAISERROR ('Wholesale', 0, 1) WITH NOWAIT
	insert into hostedMatchedMRC (wholesalebilldate, customerid, masterwholesalebtn, originalwholesalebtn,
				wholesalebtn, wholesaleusoc, wholesaleqty, wholesaleamount, 
				matchedby, lastmodifiedby, lastmodifieddatetime)
	(select w.billdate as wholesalebilldate, coalesce(x.internalid, w.customername) as customerid, 
			w.masterbtn as masterwholesalebtn, w.btn as originalwholesalebtn, w.btn as wholesalebtn, 
			w.usoc wholesaleusoc, Sum(w.Qty), Sum(w.Amount), 'Auto:Wholesale Except', @Operator, getdate()
		from hostedimportmrcwholesale w WITH (NOLOCK)
		  --ABW Determine how to change this
		  left join externalidmapping x WITH (NOLOCK) on w.customername = x.externalid and x.entitytype = 'Customer'
		where w.matchedby like 'Auto:Ex%' and w.billdate = @billdate
	    group by w.billdate, coalesce(x.internalid, w.customername), w.masterbtn, w.btn, w.btn, w.usoc)

	RAISERROR ('Retail', 0, 1) WITH NOWAIT
	insert into hostedMatchedMRC (retailbilldate, customerid, masterretailbtn, 
				retailbtn, retailusoc, alternateid, retailqty, retailamount, 
				matchedby, lastmodifiedby, lastmodifieddatetime)
	(select r.billdate as retailbilldate,
	        coalesce((
			select aid.entity 
			from EntityAlternateIDs aid
			 where r.customernumber = aid.ExternalID and r.BillDate between aid.startDate and aid.EndDate
			
			),r.customername) as customerid,
--	        coalesce((select min(x.internalid) from externalidmapping x where r.customername = x.externalid and x.entitytype = 'Customer'), r.customername) as customerid, 
		    r.masterbtn as masterretailbtn, r.btn as retailbtn, 
			r.usoc wholesaleusoc, r.customernumber, Sum(r.Qty), Sum(r.Amount), 'Auto:Retail Except', @Operator, getdate()
		from hostedimportmrcretail r WITH (NOLOCK)
		where r.matchedby like 'Auto:Ex%' and r.billdate = @billdate 
	    group by r.billdate, r.customername,r.masterbtn, r.btn, r.usoc,r.customernumber)
				
	RAISERROR ('Post non-excluded Wholesale', 0, 1) WITH NOWAIT
	insert into hostedMatchedMRC (wholesalebilldate, customerid, masterwholesalebtn, originalwholesalebtn,
				wholesalebtn, wholesaleusoc, wholesaleqty, wholesaleamount, 
				lastmodifiedby, lastmodifieddatetime)
	(select w.billdate as wholesalebilldate, coalesce(x.internalid, w.customername) as customerid, 
			w.masterbtn as masterwholesalebtn, w.btn as originalwholesalebtn, coalesce(s.newbtn,w.btn) as wholesalebtn, 
			w.usoc wholesaleusoc, Sum(w.Qty), Sum(w.Amount), @Operator, getdate()
		from hostedimportmrcwholesale w WITH (NOLOCK)
		  -- ABW Determine how to change this
		  left join externalidmapping x WITH (NOLOCK) on w.customername = x.externalid and x.entitytype = 'Customer'
		  left join hostedmrcwholesalebtnmatching s WITH (NOLOCK) on w.btn = s.originalbtn
		where w.matchedby is null and w.billdate = @billdate
	    group by w.billdate, coalesce(x.internalid, w.customername), w.masterbtn, w.btn, coalesce(s.newbtn,w.btn), w.usoc)

	RAISERROR ('Set Matched and Matchedby flags in Import tables', 0, 1) WITH NOWAIT
	update hostedimportmrcwholesale set matchedby = 'Auto:Wholesale Post', matcheddatetime = getdate() 
		where matchedby is null	and billdate = @billdate	

	RAISERROR ('Post Retail against Unmatched Wholesale using matching BTN and USOC', 0, 1) WITH NOWAIT
	RAISERROR ('PREVIOUS MONTH MATCHING', 0, 1) WITH NOWAIT
	RAISERROR ('Populate temp table', 0, 1) WITH NOWAIT

	SELECT m.ID, r.* INTO #tmpPreviousMonth1
      from hostedmatchedmrc m WITH (NOLOCK) 
	  inner join 
	(select billdate as retailbilldate,masterbtn as masterretailbtn,mr.btn as retailbtn,mr.usoc retailusoc, 
		    customernumber, sum(Qty) QSum, Sum(Amount) RetailAmt
	  from hostedimportmrcretail mr WITH (NOLOCK)
	  where mr.matchedby is null and mr.billdate = @billdate
		  group by mr.billdate, mr.customername, mr.masterbtn, mr.btn, mr.usoc,mr.customernumber) r
	on m.wholesalebtn = r.retailbtn 
	left join hostedmrcwholesalebtnmatching s WITH (NOLOCK) on m.wholesalebtn = s.originalbtn
	inner join vw_CHUsocs c WITH (NOLOCK) on r.retailusoc = c.usocretail and m.wholesaleusoc = c.usocwholesale
	where m.wholesaleqty = r.qsum and m.matchedby is null 
	  and m.wholesalebilldate = @billdate

	RAISERROR ('Update from temp table', 0, 1) WITH NOWAIT
	update hostedmatchedmrc 
	set RetailBillDate = r.retailbilldate,MasterRetailBTN = r.masterretailbtn,RetailBTN = r.retailbtn,
	RetailUSOC = r.retailusoc,RetailQty = r.QSum,RetailAmount = r.RetailAmt, alternateid = r.customernumber,
	MatchedBy = 'Auto:BTN/USOC Match',LastModifiedDateTime = getdate(),LastModifiedBy = @Operator
      from hostedmatchedmrc m WITH (NOLOCK) 
	  inner join #tmpPreviousMonth1 r on m.id = r.id

	RAISERROR ('Set Matched and Matchedby flags in Retail Import table', 0, 1) WITH NOWAIT
	update hostedimportmrcretail set matchedby = 'Auto:BTN/USOC Match', matcheddatetime = getdate() 
	 where matchedby is null and billdate = @billdate and id in
	(select distinct t.id 
	   from hostedimportmrcretail t WITH (NOLOCK)
	     inner join hostedmatchedmrc h WITH (NOLOCK)
		   on t.billdate = @billdate and h.retailbilldate = @billdate and t.btn = h.retailbtn and 
		      t.usoc = h.retailusoc)

	RAISERROR ('Post Retail against Unmatched Wholesale using hostedbtnmatching', 0, 1) WITH NOWAIT
	RAISERROR ('PREVIOUS MONTH MATCHING', 0, 1) WITH NOWAIT
	RAISERROR ('Populate temp table', 0, 1) WITH NOWAIT
	select m.id, r.* into #tmpPreviousMonth
	from hostedmatchedmrc m WITH (NOLOCK) inner join 
	(select billdate as retailbilldate,masterbtn as masterretailbtn,mr.btn as originalbtn,bm.wholesalebtn as retailbtn,mr.usoc retailusoc, 
			customernumber,sum(Qty) QSum, Sum(Amount) RetailAmt 
	  from hostedimportmrcretail mr WITH (NOLOCK) 
	  inner join hostedbtnmatching bm WITH (NOLOCK) on mr.btn = bm.retailbtn
	  where mr.matchedby is null and mr.billdate = @billdate
		  group by mr.billdate, mr.customername, mr.masterbtn, mr.btn,bm.wholesalebtn, mr.usoc,mr.customernumber) r
	on m.wholesalebtn = r.retailbtn 
	inner join vw_CHUsocs c WITH (NOLOCK) on r.retailusoc = c.usocretail and m.wholesaleusoc = c.usocwholesale
	where m.wholesaleqty = r.qsum and m.matchedby is null and 
	      m.wholesalebilldate = @billdate

	RAISERROR ('Update from temp table', 0, 1) WITH NOWAIT
	update hostedmatchedmrc set RetailBillDate = r.retailbilldate,MasterRetailBTN = r.masterretailbtn,
	    Originalretailbtn = r.originalbtn, RetailBTN = r.retailbtn,
		RetailUSOC = r.retailusoc,alternateid = r.customernumber, RetailQty = r.QSum,RetailAmount = r.RetailAmt,
		MatchedBy = 'Auto:BTN Matching',LastModifiedDateTime = getdate(),LastModifiedBy = @operator
	from hostedmatchedmrc m WITH (NOLOCK) 
	INNER JOIN #tmpPreviousMonth r on m.id = r.id
	
	--inner join 
	--(select billdate as retailbilldate,masterbtn as masterretailbtn,mr.btn as originalbtn,bm.wholesalebtn as retailbtn,mr.usoc retailusoc, 
	--		customernumber,sum(Qty) QSum, Sum(Amount) RetailAmt 
	--  from hostedimportmrcretail mr WITH (NOLOCK) 
	--  inner join hostedbtnmatching bm WITH (NOLOCK) on mr.btn = bm.retailbtn
	--  where mr.matchedby is null and mr.billdate = @billdate
	--	  group by mr.billdate, mr.customername, mr.masterbtn, mr.btn,bm.wholesalebtn, mr.usoc,mr.customernumber) r
	--on m.wholesalebtn = r.retailbtn 
	--inner join vw_CHUsocs c WITH (NOLOCK) on r.retailusoc = c.usocretail and m.wholesaleusoc = c.usocwholesale
	--where m.wholesaleqty = r.qsum and m.matchedby is null and 
	--      m.wholesalebilldate = @billdate

	RAISERROR ('Set Matched and Matchedby flags in Retail Import table', 0, 1) WITH NOWAIT
	update hostedimportmrcretail set matchedby = 'Auto:BTN Matching', matcheddatetime = getdate() 
	 where matchedby is null and billdate = @billdate and id in
	(select distinct t.id from hostedimportmrcretail t WITH (NOLOCK)
	                       inner join hostedmatchedmrc h WITH (NOLOCK)
								on t.billdate = @billdate and h.retailbilldate = @billdate 
								    and t.usoc = h.retailusoc and t.btn = h.originalretailbtn
	                       inner join hostedbtnmatching m WITH (NOLOCK)
	                            on t.btn = m.retailbtn and h.wholesalebtn = m.wholesalebtn)
	
	RAISERROR ('Post Remaining Retail records as unmatched', 0, 1) WITH NOWAIT
	insert into hostedMatchedMRC (retailbilldate, customerid, masterretailbtn, 
				retailbtn, retailusoc,alternateid, retailqty, retailamount, 
				lastmodifiedby, lastmodifieddatetime)
	(select r.billdate as retailbilldate, coalesce(e.entity, r.customername) as customerid, 
			r.masterbtn as masterretailbtn, r.btn as retailbtn,  
			r.usoc retailusoc, r.customernumber,Sum(r.Qty), Sum(r.Amount), @Operator, getdate()
		from hostedimportmrcretail r WITH (NOLOCK)
          left join EntityAlternateIDs e WITH (NOLOCK) on r.customernumber = e.ExternalID and r.BillDate between e.StartDate and e.EndDate
		where r.matchedby is null and r.billdate = @billdate
	    group by r.billdate, coalesce(e.entity, r.customername), r.masterbtn, r.btn, r.usoc,r.customernumber)
	    
	RAISERROR ('Update Retail Import Table', 0, 1) WITH NOWAIT
	update hostedimportmrcretail set matchedby = 'Auto:Retail Post', matcheddatetime = getdate() 
	 where matchedby is null and billdate = @billdate

	RAISERROR ('Update CustomerIDs where alternate ID is valid in the entity table and wrong because of externalidmapping', 0, 1) WITH NOWAIT
update hostedmatchedmrc
   set customerid = e.entity
  from hostedmatchedmrc mrc WITH (NOLOCK)
  inner join entity e on mrc.alternateid = e.alternateid
  where mrc.customerid <> e.entity
  
	RAISERROR ('Update CustomerIDs where we have a valid saddleback ID', 0, 1) WITH NOWAIT
update hostedmatchedmrc
   set customerid = e.entity
  from hostedmatchedmrc m  WITH (NOLOCK)
   inner join entity e WITH (NOLOCK) on m.alternateid = e.alternateid
 where m.alternateid is not null and isnumeric(m.customerid) <> 1

	RAISERROR ('Update CustomerIDs where the name matches the entity legalname', 0, 1) WITH NOWAIT
update hostedmatchedmrc
   set customerid = e.entity
  from hostedmatchedmrc m WITH (NOLOCK)
   inner join entity e WITH (NOLOCK) on m.customerid = e.legalname
 where isnumeric(m.customerid) <> 1

	RAISERROR ('Update CustomerIDs where the name matches an externalidmapping entry', 0, 1) WITH NOWAIT
update hostedmatchedmrc
   set customerid = x.internalid
  from hostedmatchedmrc m WITH (NOLOCK)
   inner join externalidmapping x WITH (NOLOCK) on m.customerid = x.externalid
 where isnumeric(m.customerid) <> 1


 	RAISERROR ('Perform wholesale usoc exception fixups from ImportUSOCMatchingExceptions table', 0, 1) WITH NOWAIT
	-- delete the unmatched wholesale usocs that are in the exception list
Delete 
from  hostedmatchedmrc 
WHERE Coalesce(WholesaleBillDate, RetailBillDate) = @BillDate
  AND WholesaleUSOC in (Select distinct WholesaleUSOCToDelete from ImportUSOCMatchingExceptions)
  and RetailUSOC is null

-- now find the retail usocs in the exception list and update the wholesale usoc and cost from the table
UPDATE HostedMatchedMRC  
   SET WholesaleUSOC = Coalesce(e.WholesaleUSOCToReplace, e.WholesaleUSOCToDelete),
       WholesaleAmount = e.WholesaleCost,
	   WholesaleQty = RetailQty,
	   WholesaleBillDate = RetailBillDate
  FROM HostedMatchedMRC m
  INNER JOIN ImportUSOCMatchingExceptions e on m.RetailUSOC = e.RetailUSOCToCopy and getdate() between isnull(e.StartDate, '1/1/1900') and isnull(e.EndDate,'12/31/2100')

	RAISERROR ('Post MRC Details Complete', 0, 1) WITH NOWAIT

END



GO

/****** Object:  StoredProcedure [dbo].[ProcessTOLLDetails]    Script Date: 5/25/2021 8:36:45 AM ******/
DROP PROCEDURE [dbo].[ProcessTOLLDetails]
GO

-- =========================================================================
-- Author:		<Andy Werdeman>
-- Create date: <11/30/2012>
-- Description:	Post import toll tables (retail and wholesale) to the matched toll table
-- Modified:Author		Date		Description	
--			Larry Axon	8/28/2016	Use new Summary Toll table instead
-- =========================================================================
-- ProcessTollDetails 'LLA', 2016, 8
-- select * from hostedmatchedsummarytoll where billdate = '8/1/2016' and customerid = '20645'
-- select * from hostedmatchedsummarytollLLA where billdate = '8/1/2016' and customerid = '20645'
-- select * from hostedimporttollRetail where billingyear = 2016 and billingmonth = 8 and customernumber = 527 and 
-- select * from hostedmatchedsummarytollLLA where billdate = '8/1/2016'
CREATE PROCEDURE [dbo].[ProcessTOLLDetails] (@Operator nvarchar(25), @BillingYear nvarchar (4), @BillingMonth nvarchar (2))
AS
BEGIN
	SET NOCOUNT ON;

/*******************************************************************************************************************

	For testing purposes, let's back out the post for a billdate

Declare @TestBillDate as date = '8/1/2016'
DELETE FROM HostedMatchedSummaryToll 
where BillDate = @TestBillDate

UPDATE HostedImportTollRetail
SET MatchedBy = null, MatchedDateTime = null
Where BillingYear = DatePart(year, @TestBillDate) AND BillingMonth = DatePart(month, @TestBillDate)

UPDATE HostedImportTollWholesale
SET MatchedBy = null, MatchedDateTime = null
Where BillingYear = DatePart(year, @TestBillDate) AND BillingMonth = DatePart(month, @TestBillDate)

select distinct matchedby from hostedimporttollretail where billingmonth = 8 and billingyear = 2016

*******************************************************************************************************************/

SELECT billingyear,billingmonth,btn,fromnumber,tonumber, customernumber, matchedby, count(id) QCount ,sum(charge) ASum  
INTO #Wholesale 
			  from hostedimporttollwholesale 
			  where billingyear = @BillingYear and billingmonth = @BillingMonth
			 group by billingyear,billingmonth,btn,fromnumber,tonumber, customernumber, matchedby

SELECT r.billingyear,r.billingmonth,r.btn,r.fromnumber,r.tonumber,r.customernumber,matchedby, count(id) QCount, sum(charge) ASum 
INTO #Retail
			   from hostedimporttollretail r 
			   where r.billingyear = @BillingYear and r.billingmonth = @BillingMonth
			  group by r.billingyear,r.billingmonth,r.btn,r.fromnumber,r.tonumber,r.customernumber,matchedby

	-------------------------------------------------------------------------------------------------------------
	-- INSERT matched values
	insert into hostedmatchedSummarytoll (BillDate,
			wholesalebtn,retailbtn,wholesalecount,wholesalecharge,retailcount,retailcharge,
			alternateid,matchedby,lastmodifiedby,lastmodifieddatetime,customerid)
	select Convert(date, coalesce(w.billingmonth, r.billingmonth) + '/01/' + coalesce(w.billingyear, r.billingyear)) BillDate,
			w.btn,r.btn,sum(w.QCount) wholesalecount,sum(w.ASum) wholesalecharge,sum(r.QCount) retailcount,sum(r.ASum) retailcharge,
			 r.customernumber alternateid,'Auto:From/To Match',@Operator,getdate(),
			aid.entity customerid
	from #Wholesale w
	left join #retail r
		on (w.fromnumber = r.fromnumber or w.btn = r.fromnumber) and w.tonumber = r.tonumber and w.QCount = r.QCount
	left join EntityAlternateIDs aid on aid.ExternalID =  r.customernumber and Convert(date,Convert(varchar(2),r.billingmonth) + '/01/' + convert(varchar(4),r.billingyear)) between aid.StartDate and aid.EndDate
	group by w.BillingMonth, w.BillingYear, r.BillingMonth, r.BillingYear, w.btn, r.btn, r.CustomerNumber, aid.Entity

	-- NOW Update the Wholesale import records that matched
	update hostedimporttollwholesale
	   set MatchedBy = 'Auto:From/To Match',MatchedDateTime = getdate() 
	FROM hostedimporttollwholesale w  
	INNER JOIN #Wholesale wsum 
		on w.BillingYear = wsum.BillingYear and w.BillingMonth = wsum.BillingMonth 
			and w.btn = wsum.btn and w.FromNumber = wsum.FromNumber and w.ToNumber = wsum.ToNumber
	INNER JOIN #Retail r
		on r.BillingYear = w.BillingYear and r.BillingMonth = w.BillingMonth and 
		(w.fromnumber = r.fromnumber or w.btn = r.fromnumber) and w.tonumber = r.tonumber and wsum.QCount = r.QCount

	-- now update the Retail import records that matched
	update hostedimporttollretail
	   set MatchedBy = 'Auto:From/To Match',MatchedDateTime = getdate() 
	   FROM HostedImportTollRetail r
	   INNER JOIN #Retail rsum
			on r.BillingYear = rsum.BillingYear and r.BillingMonth = rsum.BillingMonth and 
			   r.fromnumber = rsum.fromnumber and r.btn = rsum.btn and r.tonumber = rsum.tonumber
	   INNER JOIN #Wholesale w 
			on r.BillingYear = w.BillingYear and r.BillingMonth = w.BillingMonth 
				and (w.fromnumber = r.fromnumber or w.btn = r.fromnumber) and r.ToNumber = w.ToNumber 
						and w.QCount = rsum.QCount
	   --left join EntityAlternateIDs aid on aid.externalid =  r.customernumber and Convert(date,convert(varchar(2),r.billingmonth) + '/01/' + convert(varchar(4), r.BillingYear)) between aid.StartDate and aid.EndDate


	--Post remainder of the Wholesale rows as unmatched
	insert into HostedMatchedSummaryToll (BillDate,
			wholesalecount,wholesalecharge,	matchedby,lastmodifiedby,lastmodifieddatetime,customerID)
	select Convert(date, w.billingmonth + '/01/' + w.billingyear) BillDate,Count(QCount) as Count,Sum(w.ASum) as Charge,
			'Auto:UnMatched',@Operator,getdate(),aid.entity customerid
	from #Wholesale w
    left join EntityAlternateIDs aid on aid.ExternalID =  w.customernumber and 
		Convert(date,Convert(varchar(2),w.billingmonth) + '/01/' + convert(varchar(4),w.billingyear)) between aid.StartDate and aid.EndDate
	where w.matchedby is null and w.billingyear = @BillingYear and w.billingmonth = @BillingMonth
	group by w.billingyear,w.billingmonth,w.btn,aid.entity

	--Post remainder of the Retail rows as unmatched
	insert into HostedMatchedSummaryToll (BillDate,retailbtn,alternateid,
				retailcount,retailcharge,matchedby,lastmodifiedby,lastmodifieddatetime,customerid)
	select Convert(date, r.billingmonth + '/01/' + r.billingyear) BillDate,btn,customernumber,Count(id) as Count,Sum(Charge) as Charge,
			'Auto:UnMatched',@Operator,getdate(),aid.entity customerid
	from hostedimporttollretail r
    left join EntityAlternateIDs aid on aid.ExternalID =  r.customernumber and 
		Convert(date,Convert(varchar(2),r.billingmonth) + '/01/' + convert(varchar(4),r.billingyear)) between aid.StartDate and aid.EndDate
	where r.matchedby is null and r.billingyear = @BillingYear and r.billingmonth = @BillingMonth
	group by r.billingyear,r.billingmonth,r.btn,r.customernumber,aid.entity
		
	--Update unmatched rows in the import tables
	update hostedimporttollwholesale 
	   set MatchedBy = 'Auto:UnMatched',MatchedDateTime = getdate() 
	 where matchedby is null and billingyear = @BillingYear and billingmonth = @BillingMonth
	 
	update hostedimporttollretail
	   set MatchedBy = 'Auto:UnMatched',MatchedDateTime = getdate() 
	 where matchedby is null and billingyear = @BillingYear and billingmonth = @BillingMonth
	---------------------------------------------------------------------------------------------------

--Update Wholesale CustomerID's where we have a Saddleback or Fluentstream ID
update HostedMatchedSummaryToll
   set customerid = aid.entity
  from HostedMatchedSummaryToll t 
    INNER join EntityAlternateIDs aid on aid.ExternalID =  t.CustomerID and
		t.billdate between aid.StartDate and aid.EndDate
 where isnumeric(t.customerid) <> 1

--Update CustomerID's where the BTN is in MRC
update HostedMatchedSummaryToll
   set customerid = m.customerid
  from HostedMatchedSummaryToll t 
   inner join (Select distinct retailbtn, customerid FROM hostedmatchedmrc) m on t.retailbtn = m.retailbtn
 where isnumeric(t.customerid) <> 1

 --Now delete any summary records with no charge
DELETE
FROM HostedMatchedSummaryToll
Where isnull(WholesaleCharge, 0) + isnull(RetailCharge,0)  = 0

END


GO
















