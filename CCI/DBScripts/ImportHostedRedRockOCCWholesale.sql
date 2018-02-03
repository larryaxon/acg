USE [CityHostedTest]
GO

/****** Object:  StoredProcedure [dbo].[ImportHostedOCCWholesale]    Script Date: 7/22/2017 11:39:47 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Larry Axon
-- Create date: 7/22/2017
-- Description:	Import tab delimited format OCC Wholesale import to existing table (Red Rock)
-- =============================================
--exec [dbo].[ImportHostedRedRockOCCWholesale] 'C:\Data\Red Rock Imports\2017\2017-07\Wholesale OCC Red Rock-201707.txt', '7/1/2017'
alter PROCEDURE [dbo].[ImportHostedRedRockOCCWholesale]	(@FilePath nvarchar (255),@BillDate varchar(10))
	
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


