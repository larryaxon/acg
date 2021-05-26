delete from hostedtemptax
insert into hostedtemptax(Account_Number,
					Tax_Type ,
					ProductName,
					Tot_Tax ,
					[Bill_Start_Date] )
select [Account Number_1] Account_Number,  '126' TaxType,'DID Numbers-Retail/DID Numbers' ProductName, Taxes Tot_Tax, '05/01/2021' [Bill_Start_Date] from HostedTempLedger 
select * from HostedTempTax


