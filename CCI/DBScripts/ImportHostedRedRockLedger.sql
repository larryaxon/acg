USE [CityHostedTest]
GO

/****** Object:  StoredProcedure [dbo].[HostedTempRedRockLedger]    Script Date: 7/29/2017 7:10:04 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Larry Axon
-- Create date: 7/29/2017
-- Description:	Import tab delimited format Ledger import to existing table (Red Rock)
-- =============================================
--exec [dbo].[ImportHostedRedRockLedger] 'C:\Data\Red Rock Imports\2017\2017-07\Ledger Red Rock-201707.txt', '7/1/2017'
Alter PROCEDURE [dbo].[ImportHostedRedRockLedger]	(@FilePath nvarchar (255),@BillDate varchar(10))
	
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


