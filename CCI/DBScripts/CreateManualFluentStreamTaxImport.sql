delete from hostedtemptax;

	delete from [dbo].HostedTempLedger;

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Declare @FirstRow int = 2
	Set @ImportFileName = 'C:\Data\Saddleback Imports\2021\2021-05\Completed\Ledger-202105.csv';

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempLedger FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = ' + Convert(varchar(5), @FirstRow) + ', FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'
	Print @BulkInsert;
	EXEC sp_executesql @BulkInsert;

insert into hostedtemptax(Account_Number,
					Tax_Type ,
					ProductName,
					Tot_Tax ,
					[Bill_Start_Date] )
select [Account Number_1] Account_Number,  '126' TaxType,'DID Numbers-Retail/DID Numbers' ProductName, Taxes Tot_Tax, '05/01/2021' [Bill_Start_Date] 
from HostedTempLedger 
where Convert(date,[Billing Period Start]) between '4/1/2021' and '4/30/2021'
select * from HostedTempTax


