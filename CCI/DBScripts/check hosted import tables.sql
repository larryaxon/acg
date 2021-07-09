DECLARE @BillDate as date = '5/1/2021'
Declare @BillMonth as int =  Month(@BillDate)
DECLARE @BillYear as int = Year(@BillDate)
Print 'Month:' + Convert(varchar(2), @BillMonth) + ' ' + 'Year:' + convert(varchar(4),@BillYear)
--select * from HostedImportLedger WHERE BillDate = @BillDate 
--select * from HostedImportMRCRetail  WHERE BillDate = @BillDate 
--select * from HostedImportMRCWholesale  WHERE BillDate = @BillDate 
--select * from HostedImportOCCRetail  WHERE BillDate = @BillDate 
--select * from HostedImportOCCWholesale  WHERE BillDate = @BillDate 
select * from HostedImportTollRetail  WHERE BillingMonth  = @BillMonth  and BillingYear =  @BillYear
select * from HostedImportTollWholesale  WHERE BillingMonth  = @BillMonth  and BillingYear =  @BillYear
--select * from HostedImportTaxes  WHERE BillDate = @BillDate 

--select * from HostedProcessCycle where billingdate = @BillDate
--select * from AcctImportsLog where billingdate = @BillDate

--delete from HostedImportLedger WHERE BillDate = @BillDate 

--select * from HostedTempTollRetail