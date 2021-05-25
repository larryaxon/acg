USE [CityHostedTest]
GO

/****** Object:  StoredProcedure [dbo].[UndoImport]    Script Date: 5/22/2021 10:12:50 AM ******/
DROP PROCEDURE [dbo].[UndoImport]
GO

/****** Object:  StoredProcedure [dbo].[UndoImport]    Script Date: 5/22/2021 10:12:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[UndoImport](@BillDate date)
AS
Declare @BillMonth as int =  Month(@BillDate)
DECLARE @BillYear as int = Year(@BillDate)
--select * from HostedImportLedger WHERE BillDate = @BillDate 
--select * from HostedImportMRCRetail  WHERE BillDate = @BillDate 
--select * from HostedImportMRCWholesale  WHERE BillDate = @BillDate 
--select * from HostedImportOCCRetail  WHERE BillDate = @BillDate 
--select * from HostedImportOCCWholesale  WHERE BillDate = @BillDate 
--select * from HostedImportTollRetail  WHERE BillingMonth  = @BillYear and BillingMOnth = @BillMonth 
--select * from HostedImportTollWholesale  WHERE BillingMonth  = @BillYear and BillingMOnth = @BillMonth 
--select * from HostedImportTaxes  WHERE BillDate = @BillDate 

--select * from HostedProcessCycle where billingdate = @BillDate
--select * from AcctImportsLog where billingdate = @BillDate

--delete from HostedImportLedger WHERE BillDate = @BillDate 

Delete from AcctImportsLog where billingdate = '5/1/2021'
Delete from HostedProcessCycle where billingdate = @BillDate
delete from HostedImportLedger WHERE BillDate = @BillDate 
delete from HostedImportMRCRetail  WHERE BillDate = @BillDate 
delete from HostedImportMRCWholesale  WHERE BillDate = @BillDate 
delete from HostedImportOCCRetail  WHERE BillDate = @BillDate 
delete from HostedImportOCCWholesale  WHERE BillDate = @BillDate 
delete from HostedImportTollRetail  WHERE BillingMonth  = @BillYear and BillingMOnth = @BillMonth 
delete from HostedImportTollWholesale  WHERE BillingMonth  = @BillYear and BillingMOnth = @BillMonth 
delete from HostedImportTaxes  WHERE BillDate = @BillDate 

GO


