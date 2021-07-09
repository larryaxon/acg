-- exec ReImportOCCWholesale '6/1/2021'
-- select * FROM hostedmatchedocc where Coalesce(WholesaleBillDate, RetailBillDate) = '6/1/2021'
--<Prorate,>
alter proc ReImportOCCWholesale(@BillDate date, @UserID varchar(50) = 'LarryA')
AS
DECLARE @importFileName as varchar(256)
DECLARE @BillYear as varchar(4)
DECLARE @BillMonth as varchar(2)
set @BillYear = Convert(varchar(4), datepart(year, @BillDate))
set @BillMonth = Right('00' + Convert(varchar(4), datepart(month, @BillDate)),2)
--print @BillYear + '-' + @BillMonth


SET @importFileName = 'C:\Data\Saddleback Imports\' + @BillYear + '\' + @BillYear + '-' + @BillMonth + 
	'\Completed\OCC WHLSL-' + @BillYear + @BillMonth + '.csv'
print 'Import file = ' + @importFileName

Delete from HostedImportOCCWholesale where BillDate = @BillDate
DELETE FROM hostedmatchedocc where Coalesce(WholesaleBillDate, RetailBillDate) = @BillDate
exec [dbo].[ImportHostedOCCWholesale] @importFileName, @BillDate
exec [dbo].[ProcessOCCDetails] @UserID, @BillDate
GO