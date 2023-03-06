delete from [dbo].[AcctImportsLog]
delete from [dbo].[ARTransactions]
delete from [dbo].[CustomerIDMismatch]
  delete from Attribute where item = 'entity' and itemtype in ('Customer', 'Location', 'MasterCustomer')
  delete from Entity where entitytype in ('Customer', 'Location', 'MasterCustomer')
delete from [dbo].[EntityAlternateIDs]
delete from [dbo].[ExceptionList]
delete from [dbo].[ExceptionLog]
delete from [dbo].[ExternalIDMapping] where Entitytype = 'cusomter'
delete from [dbo].[FollowUps]
delete from [dbo].[GroupMembers]
delete from [dbo].[HostedBTNCustomer]
delete from [dbo].[HostedBTNMatching]
delete from [dbo].[HostedImportLedger]
delete from [dbo].[HostedImportMRCRetail]
delete from [dbo].[HostedImportMRCWholesale]
delete from [dbo].[HostedImportOCCRetail]
delete from [dbo].[HostedImportOCCWholesale]
delete from [dbo].[HostedImportPayments]
delete from [dbo].[HostedImportTaxes]
delete from [dbo].[HostedImportTollRetail]
delete from [dbo].[HostedImportTollWholesale]
delete from [dbo].[HostedLedgerTransactions]
delete from [dbo].[HostedMatchedExceptions]
delete from [dbo].[hostedmatchedmrc]
delete from [dbo].[hostedmatchedocc]
delete from [dbo].[HostedMatchedSummaryToll]
delete from [dbo].[HostedMRCWholesaleBTNMatching]
delete from [dbo].[HostedNoMatchBTNs]
delete from [dbo].[HostedNoMatchBTNUSOC]
delete from [dbo].[HostedNoMatchUSOCs]
delete from [dbo].[HostedProcessCycle]
delete from [dbo].[hostedtaxtransactions]
truncate table [dbo].[HostedTempLedger]
-- next batch

truncate table [dbo].[HostedTempMRCRetail]
truncate table [dbo].[HostedTempMRCWholesale]
truncate table [dbo].[HostedTempOCCRetail]
truncate table [dbo].[HostedTempOCCWholesale]
truncate table [dbo].[HostedTempPayments]
truncate table [dbo].[HostedTempRedRockOCCWholesale]
truncate table [dbo].[HostedTempTax]
truncate table [dbo].[HostedTempTollRetail]
truncate table [dbo].[HostedTempTollWholesale]
delete from [dbo].[HostedUSOCRetailPricing] where isnull(enddate, '1/1/2020') < '1/1/2023'


drop table [dbo].[LLACustomerAccountDataExportWithNewIDs]
drop table [dbo].[LLAEntityAlternateIDsBackup]
drop table [dbo].[LLAFluentstreamBillingConversionList]
drop table [dbo].[LLATax-202106]


  delete from [dbo].[MRCDetail] where customer not like 'CITY COMMUNICATIONS%'
truncate table [dbo].[NetworkInventory]
truncate table [dbo].[NetworkInventoryMatchup]
truncate table [dbo].[SalesOrDealerCustomers]


