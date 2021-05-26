USE [CityHostedTest]
GO


Declare @billdate as date = '4/1/2021'


DECLARE @BillYear as int = year(@billdate)
DECLARE @BillMonth as int = month(@billdate)

/****************************************************************************************************

	INSERT  FROM Posted Transactions for this Period from the prod db

****************************************************************************************************/
/*
set IDENTITY_INSERT [dbo].[hostedmatchedmrc] ON
INSERT INTO [dbo].[hostedmatchedmrc] ([ID]
      ,[WholesaleBillDate]
      ,[RetailBillDate]
      ,[CustomerID]
      ,[MasterWholesaleBTN]
      ,[OriginalWholesaleBTN]
      ,[WholesaleBTN]
      ,[MasterRetailBTN]
      ,[OriginalRetailBTN]
      ,[RetailBTN]
      ,[WholesaleUSOC]
      ,[RetailUSOC]
      ,[WholesaleQty]
      ,[WholesaleAmount]
      ,[RetailQty]
      ,[RetailAmount]
      ,[MatchedBy]
      ,[LastModifiedDateTime]
      ,[LastModifiedBy]
      ,[AlternateID])
Select * from CityHostedProd.[dbo].[hostedmatchedmrc] where coalesce([RetailBillDate],[WholesaleBillDate]) = @BillDate
GO
*/
---- OCC

SET IDENTITY_INSERT [dbo].[hostedmatchedocc]  OFF
INSERT INTO [dbo].[hostedmatchedocc]  ([ID]
      ,[CustomerID]
      ,[WholesaleMasterBTN]
      ,[RetailMasterBTN]
      ,[WholesaleBTN]
      ,[RetailBTN]
      ,[WholesaleDate]
      ,[RetailDate]
      ,[WholesaleAmount]
      ,[RetailAmount]
      ,[WholesaleService]
      ,[RetailService]
      ,[WholesaleUSOC]
      ,[RetailUSOC]
      ,[WholesaleBillDate]
      ,[RetailBillDate]
      ,[MatchedBy]
      ,[LastModifiedDateTime]
      ,[LastModifiedBy]
      ,[AlternateID])
SELECT * FROM CityHostedProd.dbo.hostedmatchedocc where coalesce([RetailBillDate],[WholesaleBillDate]) = @BillDate
SET IDENTITY_INSERT [dbo].[hostedmatchedocc]  OFF
/*
-- Toll

SET IDENTITY_INSERT [dbo].[HostedMatchedSummaryToll] ON

INSERT INTO [dbo].[HostedMatchedSummaryToll]([ID]
      ,[BillDate]
      ,[MasterWholesaleBTN]
      ,[WholesaleBTN]
      ,[MasterRetailBTN]
      ,[RetailBTN]
      ,[WholesaleCount]
      ,[WholesaleCharge]
      ,[RetailCount]
      ,[RetailCharge]
      ,[MatchedBy]
      ,[LastModifiedBy]
      ,[LastModifiedDateTime]
      ,[AlternateID]
      ,[CustomerID])
SELECT * from CityHostedProd.[dbo].[HostedMatchedSummaryToll] where BillDate = @BillDate
SET IDENTITY_INSERT [dbo].[HostedMatchedSummaryToll] OFF

-- Ledger
SET IDENTITY_INSERT [dbo].[HostedLedgerTransactions] ON
INSERT INTO [dbo].[HostedLedgerTransactions]  ([ID]
      ,[CustomerID]
      ,[BillDate]
      ,[TransactionDate]
      ,[TransactionType]
      ,[Amount]
      ,[AlternateID]
      ,[LastModifiedBy]
      ,[LastModifiedDateTime])
Select * from CityHostedProd.[dbo].[HostedLedgerTransactions] where BillDate = @BillDate
SET IDENTITY_INSERT [dbo].[HostedLedgerTransactions] OFF

-- Tax

SET IDENTITY_INSERT [dbo].[hostedtaxtransactions] ON
INSERT INTO [dbo].[hostedtaxtransactions] ([ID]
      ,[Customer]
      ,[MasterBTN]
      ,[Level]
      ,[LevelType]
      ,[Jurisdiction]
      ,[Rate]
      ,[TaxType]
      ,[Title]
      ,[TaxAmount]
      ,[BillDate]
      ,[Sign]
      ,[LastModifiedBy]
      ,[LastModifiedDateTime])
Select * from CityHostedProd.[dbo].[hostedtaxtransactions] where BillDate = @BillDate
SET IDENTITY_INSERT [dbo].[hostedtaxtransactions] OFF

-- and finally, the invoiced posted
SET IDENTITY_INSERT [dbo].[ARTransactions]  ON
INSERT INTO  [dbo].[ARTransactions] ([ID]
      ,[CustomerID]
      ,[TransactionType]
      ,[TransactionDate]
      ,[BillDate]
      ,[TransactionDateTime]
      ,[TransactionSubType]
      ,[Amount]
      ,[NonSaddlebackAmount]
      ,[Reference]
      ,[LastModifiedBy]
      ,[LastModifiedDateTime]
      ,[ExportDateTime]
      ,[Comment])
SELECT * FROM CityHostedProd.[dbo].[ARTransactions] where billdate = @BillDate and LastModifiedBy = 'Import Post'
SET IDENTITY_INSERT [dbo].[ARTransactions]  OFF

/****************************************************************************************************

	RESTORE FROM Imported Transactions for this period

****************************************************************************************************/

-- LEDGER
SET IDENTITY_INSERT [dbo].[HostedImportLedger]  ON
INSERT INTO [dbo].[HostedImportLedger] ([ID]
      ,[Customer]
      ,[CustomerName]
      ,[TransactionDate]
      ,[Title]
      ,[Amount]
      ,[BillDate]
      ,[MatchedBy]
      ,[MatchedDateTime])
Select * from CityHostedProd.[dbo].[HostedImportLedger] where billdate = @BillDate
SET IDENTITY_INSERT [dbo].[HostedImportLedger]  OFF

-- MRC Retail
SET IDENTITY_INSERT [dbo].[HostedImportMRCRetail]  ON
INSERT INTO [dbo].[HostedImportMRCRetail]  ( [ID]
      ,[CustomerNumber]
      ,[CustomerName]
      ,[MasterBTN]
      ,[BTN]
      ,[USOC]
      ,[Product Description]
      ,[Qty]
      ,[Price]
      ,[Amount]
      ,[ConnectionDate]
      ,[BillDate]
      ,[MatchedBy]
      ,[MatchedDateTime])
Select * from CityHostedProd.[dbo].[HostedImportMRCRetail] where billdate = @BillDate
SET IDENTITY_INSERT [dbo].[HostedImportMRCRetail]  OFF

--MRC WHolesale

SET IDENTITY_INSERT [dbo].[HostedImportMRCWholesale]  ON
INSERT INTO [dbo].[HostedImportMRCWholesale]( [ID]
      ,[CustomerNumber]
      ,[CustomerName]
      ,[MasterBTN]
      ,[BTN]
      ,[USOC]
      ,[Product Description]
      ,[Qty]
      ,[Price]
      ,[Amount]
      ,[ConnectionDate]
      ,[BillDate]
      ,[MatchedBy]
      ,[MatchedDateTime])
SELECT * FROM CityHostedProd.[dbo].[HostedImportMRCWholesale] where billdate = @BillDate
SET IDENTITY_INSERT [dbo].[HostedImportMRCWholesale]  OFF

-- OCC Retail
SET IDENTITY_INSERT [dbo].[HostedImportOCCRetail]   ON
INSERT INTO [dbo].[HostedImportOCCRetail]([ID]
      ,[Customer]
      ,[MasterBTN]
      ,[BTN]
      ,[Date]
      ,[Amount]
      ,[Code]
      ,[USOC]
      ,[Service]
      ,[BillDate]
      ,[MatchedBy]
      ,[MatchedDateTime])
Select * from CityHostedProd.[dbo].[HostedImportOCCRetail] where billdate = @BillDate
SET IDENTITY_INSERT [dbo].[HostedImportOCCRetail]   OFF

-- OCC Wholesale
SET IDENTITY_INSERT [dbo].[HostedImportOCCWholesale]   ON
INSERT INTO [dbo].[HostedImportOCCWholesale] ([ID]
      ,[Customer]
      ,[MasterBTN]
      ,[BTN]
      ,[Date]
      ,[Amount]
      ,[OCCCode]
      ,[USOC]
      ,[Service]
      ,[BillDate]
      ,[MatchedBy]
      ,[MatchedDateTime])
Select * from CityHostedProd.[dbo].[HostedImportOCCWholesale] where billdate = @BillDate
SET IDENTITY_INSERT [dbo].[HostedImportOCCWholesale]   OFF

-- Toll Retail
SET IDENTITY_INSERT  [dbo].[HostedImportTollRetail]  ON
INSERT INTO [dbo].[HostedImportTollRetail]( [ID]
      ,[CustomerNumber]
      ,[BillingYear]
      ,[BillingMonth]
      ,[BTN]
      ,[MessageType]
      ,[FromNumber]
      ,[CallDate]
      ,[Duration]
      ,[ToNumber]
      ,[ToCity]
      ,[ToState]
      ,[Rate]
      ,[Charge]
      ,[MatchedBy]
      ,[MatchedDateTime])
SELECT * from CityHostedProd.[dbo].[HostedImportTollRetail] where billingyear = @BillYear and billingmonth = @BillMonth
SET IDENTITY_INSERT  [dbo].[HostedImportTollRetail]  OFF

--Toll Wholesale
SET IDENTITY_INSERT [dbo].[HostedImportTollWholesale]  ON
INSERT INTO [dbo].[HostedImportTollWholesale] ([ID]
      ,[BillingMonth]
      ,[BillingYear]
      ,[BTN]
      ,[CallNumber]
      ,[Charge]
      ,[CustomerNumber]
      ,[CallDate]
      ,[Duration]
      ,[FromNumber]
      ,[FromState]
      ,[CarrierID]
      ,[MessageType]
      ,[OCPID]
      ,[Rate]
      ,[RateClass]
      ,[SettlementCode]
      ,[ToCity]
      ,[ToReference]
      ,[ToNumber]
      ,[ToState]
      ,[UsageAccountCode]
      ,[MatchedBy]
      ,[MatchedDateTime])
SELECT * from CityHostedProd.[dbo].[HostedImportTollWholesale] where billingyear = @BillYear and billingmonth = @BillMonth
SET IDENTITY_INSERT [dbo].[HostedImportTollWholesale]  OFF

--Tax
SET IDENTITY_INSERT [dbo].[HostedImportTaxes]  ON
INSERT INTO [dbo].[HostedImportTaxes] ([ID]
      ,[Customer]
      ,[MasterBTN]
      ,[Level]
      ,[LevelType]
      ,[Jurisdiction]
      ,[Rate]
      ,[TaxType]
      ,[Title]
      ,[TaxAmount]
      ,[BillDate]
      ,[Sign])
SELECT  * FROM CityHostedProd.[dbo].[HostedImportTaxes] where billdate = @BillDate
SET IDENTITY_INSERT [dbo].[HostedImportTaxes]  OFF


/****************************************************************************************************

	Restore FROM Log Tables so we have  prior activity for this period

****************************************************************************************************/

-- Process Cycle

SET IDENTITY_INSERT [dbo].[HostedProcessCycle]   ON
INSERT INTO [dbo].[HostedProcessCycle] ([ID]
      ,[BillingDate]
      ,[Step]
      ,[SubStep]
      ,[ProcessedBy]
      ,[ProcessedDateTime]
      ,[LastModifiedDateTime]
      ,[LastModifiedBy]
      ,[FileType]
      ,[FileName]
      ,[UnProcessedDateTime])
SELECT * FROM CityHostedProd.[dbo].[HostedProcessCycle] where BillingDate = @BillDate
SET IDENTITY_INSERT [dbo].[HostedProcessCycle]   OFF

-- Acct Log

SET IDENTITY_INSERT [dbo].[AcctImportsLog]  ON
INSERT INTO [dbo].[AcctImportsLog] ([ID]
      ,[User]
      ,[DateTime]
      ,[BillingDate]
      ,[FileType]
      ,[File]
      ,[Source])
SELECT * FROM CityHostedProd.[dbo].[AcctImportsLog] where BillingDate = @BillDate
SET IDENTITY_INSERT [dbo].[AcctImportsLog]  OFF






*/