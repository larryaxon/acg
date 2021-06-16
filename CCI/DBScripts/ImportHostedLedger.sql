USE [CityHostedProd]
GO

/****** Object:  StoredProcedure [dbo].[ImportHostedLedger]    Script Date: 6/16/2021 11:32:08 AM ******/
DROP PROCEDURE [dbo].[ImportHostedLedger]
GO

/****** Object:  StoredProcedure [dbo].[ImportHostedLedger]    Script Date: 6/16/2021 11:32:08 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		Larry Axon
-- Create date: 5/20/2021
-- Description:	Import csv format ledger import to existing table
-- =============================================
--exec [dbo].[ImportHostedLedger] 'C:\Data\Saddleback Imports\2021\2021-06\Completed\Ledger-202106.csv', '6/1/2021'
--select * from hostedimportledger where billdate = '5/1/2021'
CREATE PROCEDURE [dbo].[ImportHostedLedger]	(@FilePath nvarchar (255),@BillDate varchar(10), @FirstRow int = 2)
	
AS
BEGIN
/*
	For now, this routine imports both ledger and tax info
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
	) ON [PRIMARY];

	delete from [dbo].HostedTempLedger;


	if object_id (N'"HostedTempTax"', N'U') is NULL
	CREATE TABLE [dbo].[HostedTempTax](
		[Account_Number] [varchar](50) NULL,
		[Invoice_Number] [varchar](50) NULL,
		[Tax_Type_Code] [varchar](50) NULL,
		[State] [varchar](50) NULL,
		[County] [varchar](50) NULL,
		[City] [varchar](50) NULL,
		[Zip] [varchar](50) NULL,
		[Tax_Type] [varchar](256) NULL,
		[Tot_Tax_Unrounded] [varchar](50) NULL,
		[Tot_Tax_Rounded] [varchar](50) NULL,
		[Gross_Amount] [varchar](50) NULL
	) ON [PRIMARY]

	delete from [dbo].HostedTempTax;


	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempLedger FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = ' + Convert(varchar(5), @FirstRow) + ', FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'
	-- import from the CSV into the temp ledger table
	EXEC sp_executesql @BulkInsert;

/*
	use Ledger Import instead of Tax import file to import taxes. Currently, tax import file does not add up properly
*/			  

		insert into hostedtemptax(Account_Number,
							[Invoice_Number] ,
							Tax_Type_Code ,
							Tax_Type,
							[Tot_Tax_Rounded],
							[Tot_Tax_Unrounded] )
		select [Account Number_1] Account_Number, [Invoice Number], '126' Tax_Type_Code,'DID Numbers-Retail/DID Numbers' Tax_Type, Taxes [Tot_Tax_Rounded], Taxes [Tot_Tax_Unrounded]
		from HostedTempLedger 
		where Convert(date,[Billing Period Start]) between @BillDate and DateAdd(day,-1,DateAdd(month, 1, @BillDate))
/*
	Now import the data into the permanent ledger table
*/
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
			-- this date field can vary but it should always be in the prior month
			WHERE Convert(date, g.[Billing Period Start]) between DateAdd(month, -1, @BillDate) and DateAdd(day, -1, @BillDate)
		) ledger
		WHERE Amount <> 0
			
/*
	and import the tax info into the permanent tax import table
*/

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
			'' as Title,
			Convert(decimal(10,5), [Tot_Tax_Unrounded]) as Amount,
			@BilLDate as BillDate
		from [dbo].HostedTempTax tx
	LEFT JOIN vw_FluentStreamParentAccounts fs on [dbo].[fnRemoveLeadingZeros](tx.Account_Number) = fs.AccountID

		--WHERE case when isdate(Charge_From) = 1 then Convert(date,Charge_From) else '1/1/1900' end between @BillDate and dateadd(month, 1, @BillDate)
		) tax
	--WHERE tax.BillDate = @BillDate


		delete from [dbo].HostedTempLedger;  
		delete from dbo.HostedTempTax;


END





GO


