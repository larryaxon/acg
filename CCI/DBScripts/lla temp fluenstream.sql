/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [ID]
      ,[CustomerID]
      ,[BillDate]
      ,[TransactionDate]
      ,[TransactionType]
      ,[Amount]
      ,[AlternateID]
      ,[LastModifiedBy]
      ,[LastModifiedDateTime]
  FROM [CityHostedTest].[dbo].[HostedLedgerTransactions]
  where --customerid = '24824' and 
  billdate = '5/1/2021'

  select * from entity where entity = '24824'
  select top 100 * from artransactions where customerid = '24824' and billdate = '5/1/2021'

  select * from hostedimportledger where billdate = '5/1/2021' and customer = '24824'
--408.27 should be 208.5