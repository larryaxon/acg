DECLARE @BillDate as date = '5/1/2021'
Declare @BillMonth as nvarchar(10)  =  convert(varchar(2),Month(@BillDate))
DECLARE @BillYear as nvarchar(10) = convert(varchar(4),Year(@BillDate))
print convert(varchar(50), @Billdate) + ' y:' + @BillYear + ' M:' + @Billmonth	
--select * from HostedImportLedger WHERE BillDate = @BillDate 
--select * from HostedImportMRCRetail  WHERE BillDate = @BillDate 
--select * from HostedImportMRCWholesale  WHERE BillDate =  @BillDate 
--select * from HostedImportOCCRetail  WHERE BillDate = @BillDate  
--select * from HostedImportOCCWholesale  WHERE BillDate = @BillDate 
select * from HostedImportTollRetail  WHERE BillingYear  = @BillYear and BillingMOnth = @BillMonth 
select * from HostedImportTollWholesale  WHERE BillingYear  = @BillYear and BillingMOnth = @BillMonth 
--select * from HostedImportTaxes  WHERE BillDate = @BillDate 

--select * from HostedProcessCycle where billingdate = @BillDate
--select * from AcctImportsLog where billingdate = @BillDate