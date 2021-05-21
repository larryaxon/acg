USE [CityHostedTest]
GO

/****** Object:  StoredProcedure [dbo].[ImportHostedRedRockLedger]    Script Date: 5/20/2021 2:33:01 PM ******/
DROP PROCEDURE [dbo].[ImportHostedLedger]
GO

/****** Object:  StoredProcedure [dbo].[ImportHostedRedRockLedger]    Script Date: 5/20/2021 2:33:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Larry Axon
-- Create date: 7/29/2017
-- Description:	Import tab delimited format Ledger import to existing table (Red Rock)
-- =============================================
--exec [dbo].ImportHostedLedger 'C:\Data\Red Rock Imports\2017\2017-07\Ledger Red Rock-201707.txt', '7/1/2017'
CREATE PROCEDURE [dbo].[ImportHostedLedger]	(@FilePath nvarchar (255),@BillDate varchar(10), @FirstRow int = 2)
	
AS
BEGIN
/*
Account Number_1	Subscriber Name	Invoice Number	Billing Period Start	Billing Period End	Created Date	Notified Date	Due Date	Last Payment Date	Collection Entry Date	Status	Currency	Recurring charges	Onetime charges	Usage charges	Adjustment	Payment	Late Fees	Other Charges   Credits	Taxes	Invoice Amount	Current Due	Seller Name	External_Id	Account Status	Billing Charge From	Billing Charge To
10426	Infinity Financial Mortgage	IN-80008213501	04/01/2021	04/30/2021	05/17/2021	05/17/2021	05/22/2021	-	2021-05-23	Due	USD	111.35	0	0	0	0	0	0	26.49	137.84	137.84	City Hosted Solutions	1855	Active	05/01/2021	05/31/2021
*/

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


	delete from [dbo].[HostedTempLedger];
	Print 'Deleted data from HostedTempLedger'

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempLedger FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = ' + Convert(varchar(5), @FirstRow) + ', FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'
	print @billdate
	Print @BulkInsert
	EXEC sp_executesql @BulkInsert;
	--return

    if not  exists (select top 1 [Billing Charge To]
                 from [HostedTempLedger] 
                where  [Billing Charge To] not between @BillDate and DateAdd(month, 1, @BilLDate))
       begin
            print 'Good Date'
	
			insert into [dbo].[HostedImportLedger] ([Customer],[CustomerName],[TransactionDate],[Title],[Amount],[BillDate],[MatchedBy],[MatchedDateTime])
			select * from (
					SELECT [Account Number_1] as Customer,
					[Subscriber Name] as [CustomerName],
					Convert(datetime, [Created Date]) as TransactionDate,
					'Invoice' as [Title],
						Convert(decimal(10,2), [Recurring charges] ) +
						Convert(decimal(10,2), [Onetime charges] ) +
						Convert(decimal(10,2), [Usage charges] ) +
						Convert(decimal(10,2), [Adjustment] ) +
						Convert(decimal(10,2), [Late Fees] ) +
						Convert(decimal(10,2), [Other Charges   Credits] ) +
						Convert(decimal(10,2), [Taxes] ) 
					as Amount, 
					@BilLDate as BillDate,
					null [MatchedBy],
					null MatchedDateTime
			   from [dbo].HostedTempLedger
			   where Convert(date, [Billing Charge To]) between  @BillDate and DateAdd(month, 1, @BilLDate)
			   ) ledger
			   WHERE ledger.Amount <> 0
			--delete from [dbo].HostedTempRedRockLedger;   
       end
    else
       begin
            print 'Bad Date'
       end

END

GO


