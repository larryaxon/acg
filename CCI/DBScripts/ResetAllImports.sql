Alter Procedure ResetAllImports(@BillDate as date)
AS

DECLARE @BillYear as int = year(@billdate)
DECLARE @BillMonth as int = month(@billdate)

/****************************************************************************************************

	DELETE FROM Posted Transactions for this Period

****************************************************************************************************/

DELETE from [dbo].[hostedmatchedmrc] where coalesce([RetailBillDate],[WholesaleBillDate]) = @BillDate
DELETE from [dbo].[hostedmatchedocc] where coalesce([RetailBillDate],[WholesaleBillDate]) = @BillDate
DELETE from [dbo].[HostedMatchedSummaryToll] where BillDate = @BillDate
DELETE from [dbo].[HostedLedgerTransactions] where BillDate = @BillDate
DELETE from [dbo].[hostedtaxtransactions] where BillDate = @BillDate
-- and finally, the invoiced posted
DELETE FROM [dbo].[ARTransactions] where billdate = @BillDate and LastModifiedBy = 'Import Post'

/****************************************************************************************************

	DELETE FROM Imported Transactions for this period

****************************************************************************************************/

DELETE from [dbo].[HostedImportLedger] where billdate = @BillDate
DELETE from [dbo].[HostedImportMRCRetail] where billdate = @BillDate
DELETE from [dbo].[HostedImportMRCWholesale] where billdate = @BillDate
DELETE from [dbo].[HostedImportOCCRetail] where billdate = @BillDate
DELETE from [dbo].[HostedImportOCCWholesale] where billdate = @BillDate
DELETE from [dbo].[HostedImportTollRetail] where billingyear = @BillYear and billingmonth = @BillMonth
DELETE from [dbo].[HostedImportTollWholesale] where billingyear = @BillYear and billingmonth = @BillMonth
DELETE from [dbo].[HostedImportTaxes] where billdate = @BillDate

/****************************************************************************************************

	DELETE FROM Temp Import Tables for this period

	Note: These would normally be empty anyway but just in case something went wrong

****************************************************************************************************/

-- Saddleback temp tables
DELETE from [dbo].[HostedTempMRCRetail] 
DELETE from [dbo].[HostedTempMRCWholesale] 
DELETE from [dbo].[HostedTempOCCRetail] 
DELETE from [dbo].[HostedTempOCCWholesale] 
DELETE from [dbo].[HostedTempPayments] 
DELETE from [dbo].[HostedTempTollRetail] 
DELETE from [dbo].[HostedTempTollWholesale]

/****************************************************************************************************

	DELETE FROM Log Tables so we have no trace of prior activity for this period

****************************************************************************************************/

DELETE FROM [dbo].[HostedProcessCycle] where BillingDate = @BillDate
DELETE FROM [dbo].[AcctImportsLog] where BillingDate = @BillDate

GO