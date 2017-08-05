USE [CityHostedTest]
GO

/****** Object:  StoredProcedure [dbo].[ImportHostedMRCWholesale]    Script Date: 7/22/2017 11:39:47 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Larry Axon
-- Create date: 7/22/2017
-- Description:	Import tab delimited format mrc Wholesale import to existing table (Red Rock)
-- =============================================
--exec [dbo].[ImportHostedRedRockMRCWholesale] 'C:\Data\Red Rock Imports\2017\2017-07\Wholesale MRC Red Rock-201707.txt', '7/1/2017'
alter PROCEDURE [dbo].[ImportHostedRedRockMRCWholesale]	(@FilePath nvarchar (255),@BillDate varchar(10))
	
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


