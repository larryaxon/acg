USE [CityHosted]
GO

/****** Object:  StoredProcedure [dbo].[ImportPayments]    Script Date: 4/30/2017 7:57:24 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		Larry Axon
-- Create date: 4/22/2017
-- Description:	Import csv format payments import to existing table
-- =============================================
--exec [dbo].[ImportPayments] 'C:\Data\Saddleback Imports\2017\2017-03\ACH Payments.txt', '4/1/2017'
--exec [dbo].[ImportPayments] 'C:\Data\Saddleback Imports\2017\2017-03\Check Payments.txt', '4/1/2017'
--exec [dbo].[ImportPayments] 'C:\Data\Saddleback Imports\2017\2017-03\CC Payments.txt', '4/1/2017'
-- select * from HostedTempPayments
-- DELETE FROM 
ALTER PROCEDURE [dbo].[ImportPayments]	(@FilePath nvarchar (255),@BillDate varchar(10))
	
AS
BEGIN
/*
Account # ,Billing # ,Billing Name ,Check # ,Payment ,Ledger ,Trans Date ,Trans Time ,User ,Current ,30 Day ,60 Day ,90 Day ,Status
00001669,405-757-7999,Throckmorton Insurance,,$30.50,48,04/20/2017,1115,CITKRASKAM,$0.00,$0.00,$0.00,$0.00,OK

*/
	Print 'Prepare the temp import table'
	if object_id (N'"HostedTempPayments"', N'U') is NULL
		CREATE TABLE [dbo].[HostedTempPayments](
			[Account # ] [nvarchar](25) NULL,
			[Billing # ] [nvarchar](25) NULL,
			[Billing Name ] [nvarchar](255) NULL,
			[Check # ] nvarchar(25) NULL,
			[Payment ] nvarchar(25) NULL,
			[Ledger ] nvarchar(10) NULL,
			[Trans Date ] nvarchar(20) NULL,
			[Trans Time ] nvarchar(20) NULL,
			[User ] nvarchar(25) NULL,
			[Current ] nvarchar(20) NULL,
			[30 Day ] nvarchar(20) NULL,
			[60 Day ] nvarchar(20) NULL,
			[90 Day ] nvarchar(20) NULL,
			[Status] nvarchar(255) NULL
		) ON [PRIMARY];

	delete from [dbo].[HostedTempPayments];
	PRINT 'Import the data'
	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.[HostedTempPayments] FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = 2, FIELDTERMINATOR = ''\t'', ROWTERMINATOR = ''\n'')'
	
	EXEC sp_executesql @BulkInsert;

	PRINT 'Now check to see if the dates are the correct BillDate'

	DECLARE @CalcBillDate as Date
	Select @CalcBillDate = Convert(date, [Trans Date ]) FROM [dbo].[HostedTempPayments]
	SET @CalcBillDate = Convert(date, Convert(nvarchar(2),Month(@CalcBillDate)) + '/01/' + Convert(nvarchar(4),Year(@CalcBillDate)))
	if (Convert(date,@BillDate) = @CalcBillDate) 
		BEGIN
			PRINT 'OK'
		END
	ELSE
		BEGIN
			RAISERROR('Transaction Date does not match Bill Date', 10, 1)
			Select 'FAIL'
			RETURN 1
		END

	Print 'Post the data to ARTransactions'
	
	-- Now cleanup the double quotes created by the Save As/Tab Delimited Excel operation, and the extra characters in the Payment
	UPDATE [dbo].[HostedTempPayments]
	SET [Billing Name ] = REPLACE([Billing Name ],'"',''),
	    [Payment ] = REPLACE(REPLACE(REPLACE([Payment ],'"',''),'$',''),',','')

	
	PRINT 'Import into the HostedImportPayments Table'
	insert into [dbo].[HostedImportPayments] (BillDate, customernumber,PhoneNumber, CustomerName, Payment, PayType, DateTimeImported, UserId, TransactionDate)
		select @BillDate as BillDate,
			[Account # ] as customernumber,
			[Billing # ] as PhoneNumber,
			[Billing Name ] as CustomerName,
			Convert(money, [Payment ]) as Payment,
			CASE WHEN [Ledger ] = '2' THEN 'Check' 
			     WHEN [Ledger ] = '45' THEN 'CreditCard' 
				 WHEN [Ledger ] = '48' THEN 'ACH' 
				 ELSE 'NONE' 
			END as PayType,
			getdate() as DateTimeImported,
			[User ] as UserId,
			Convert(date,[Trans Date ]) as TransactionDate
		from [dbo].[HostedTempPayments];
	-- dont delete the temp data cause we may want to look at it. We will clean it up on the next import
	--delete from [dbo].[HostedTempPayments]; 
	
	PRINT 'Post to the ARTransactions Table' 
-- Now post to the ARTransactions Table

	INSERT INTO ARTransactions (CustomerID, TransactionType, TransactionDate, BillDate, TransactionDateTIme, TransactionSubType,
		Amount, NonSaddlebackAMount, Reference, LastModifiedBy, LastModifiedDateTime, ExportDateTime, Comment)
	select c.Entity CustomerID, 'Cash' TransactionType, t.TransactionDate TransactionDate, t.BillDate BillDate,
	getdate() transactionDateTIme,  t.PayTYpe TransactionSubType, t.Payment Amount, 0 NonSaddlebackAmount, 'Import' Reference, t.UserID, t.DateTimeImported LastModifiedDateTime, 
	null ExportDateTime, 'Import Payments' Comment
	from HostedImportPayments t
	inner join entity c on t.CustomerNumber = c.AlternateID
	
	SELECT 'SUCCESS'
	--select * from HostedImportPayments
END

--select * from artransactions where comment = 'IMport Payments'






GO


