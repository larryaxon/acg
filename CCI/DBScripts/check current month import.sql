select * from [dbo].[HostedImportLedger] where billdate = '09/1/2017'
select * from [dbo].[HostedImportMRCRetail] where billdate = '09/1/2017'
select * from [dbo].[HostedImportMRCWholesale] where billdate = '09/1/2017'
select * from [dbo].[HostedImportOCCRetail] where billdate = '09/1/2017'
select * from [dbo].[HostedImportOCCWholesale] where billdate = '09/1/2017'
select * from [dbo].[HostedImportTollRetail] where billingyear = 2017 and billingmonth = 09
select * from [dbo].[HostedImportTollWholesale] where billingyear = 2017 and billingmonth = 09
select * from [dbo].[HostedImportTaxes] where billdate = '09/1/2017'

select *
  FROM [CityHosted].[dbo].[HostedProcessCycle]
  where BillingDate = '9/1/2017'

select *
  FROM [CityHosted].[dbo].[AcctImportsLog]
  where BillingDate = '9/1/2017'

select * from [dbo].[hostedmatchedmrc] where coalesce([RetailBillDate],[WholesaleBillDate]) = '09/1/2017'
select * from [dbo].[hostedmatchedocc] where coalesce([RetailBillDate],[WholesaleBillDate]) = '09/1/2017'