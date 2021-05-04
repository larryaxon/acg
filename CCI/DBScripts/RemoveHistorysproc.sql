-- ================================================
-- Routine to delete all history prior to a certain date
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE RemoveHistory
GO
-- =============================================
-- Author:		Larry Axon
-- Create date: 10/14/2020
-- Description:	Routine to delete all history prior to a certain date
-- =============================================
-- EXEC RemoveHistory '12/1/2018'
CREATE PROCEDURE RemoveHistory
	-- Add the parameters for the stored procedure here
	@BeforeDateTime as datetime
AS
BEGIN
	DELETE FROM [dbo].[AcctImportsLog] where BillingDate < @BeforeDateTime

	--DELETE FROM [dbo].[ARTransactions]   where billdate < @BeforeDateTime

	DELETE FROM [dbo].[HostedDealerCosts]  where isnull(EndDate, getdate()) < @BeforeDateTime

	DELETE  FROM [dbo].[HostedImportLedger] where BillDate < @BeforeDateTime

	DELETE  FROM [dbo].[HostedImportMRCRetail] where BillDate < @BeforeDateTime

	DELETE  FROM [dbo].[HostedImportMRCWholesale] where BillDate < @BeforeDateTime

	DELETE  FROM [dbo].[HostedImportMRCWholesale] where BillDate < @BeforeDateTime

	DELETE  FROM [dbo].[HostedImportOCCRetail]  where BillDate < @BeforeDateTime

	DELETE  FROM [dbo].[HostedImportOCCWholesale]  where BillDate < @BeforeDateTime

	DELETE  FROM [dbo].[HostedImportPayments]  where BillDate < @BeforeDateTime

	DELETE  FROM [dbo].[HostedImportTaxes] where BillDate < @BeforeDateTime

	DELETE  FROM [dbo].[HostedImportTollRetail]  where  BillingMonth + '/1/' + Billingyear < @BeforeDateTime

	DELETE  FROM [dbo].[HostedImportTollWholesale] where  BillingMonth + '/1/' + Billingyear < @BeforeDateTime

	DELETE  FROM [dbo].[HostedLedgerTransactions] where BillDate < @BeforeDateTime

	DELETE  FROM [dbo].[hostedmatchedmrc] where coalesce(RetailBillDate, WholesaleBillDate) < @BeforeDateTime

	DELETE  FROM [dbo].[hostedmatchedocc]  where coalesce(RetailBillDate, WholesaleBillDate) < @BeforeDateTime

	DELETE  FROM [dbo].[HostedMatchedSummaryToll] where BillDate < @BeforeDateTime

	DELETE  FROM [dbo].[HostedProcessCycle] where BillingDate < @BeforeDateTime

	DELETE  FROM [dbo].[hostedtaxtransactions] where BillDate < @BeforeDateTime

END

GO
