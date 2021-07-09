USE [CityHostedProdTest]
GO

/****** Object:  StoredProcedure [dbo].[ImportHostedOCCWholesale]    Script Date: 7/6/2021 8:03:02 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		Larry Axon
-- Create date: 9/19/2015
-- Description:	Import csv format OCC Wholesale import to existing table
-- =============================================
--exec [dbo].[ImportHostedOCCWholesale] 'C:\Data\Saddleback Imports\2021\2021-06\Completed\OCC WHLSL-202106.csv', '6/1/2021'
--select * from HostedTempOCCWholesale
alter PROCEDURE [dbo].[ImportHostedOCCWholesale]	(@FilePath nvarchar (255),@BillDate date, @FirstRow int = 2)
	
AS
BEGIN
/*
Cust Acct Nr	Service Name	Billing Acct Nr	Working Acct Nr	USOC	Title	Qty	Price	Amt Billed	Conn Date	Bill Date	Bus/Res
											
1156	1 WORLD MEDICINE	702 445-7031	702 445-7031	NMF	Network Maintenance Assessment	6	0.5	3	20140819	20150901	B

*/

	if object_id (N'"HostedTempOCCWholesale"', N'U') is NULL
		CREATE TABLE [dbo].[HostedTempOCCWholesale](
			[Customer] varchar(25) NULL
      ,[MasterBTN] varchar(25) NULL
      ,[BTN] varchar(25) NULL
      ,[Date] datetime
      ,[Amount] float
      ,[Code]  varchar(25) NULL
      ,[USOC]  varchar(25) NULL
      ,[Service]  varchar(1024) NULL
      ,[BillDate] varchar(25) NULL
      ,[ChargeType] varchar(25) NULL
		) ON [PRIMARY];

	delete from [dbo].[HostedTempOCCWholesale];

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempOCCWholesale FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = ' + convert(varchar(5), @FirstRow) + ', FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'
	print @BulkInsert

	EXEC sp_executesql @BulkInsert;

            print 'Good Date'
			insert into [dbo].[HostedImportOCCWholesale] ([Customer]
			  ,[MasterBTN]
			  ,[BTN]
			  ,[Date]
			  ,[Amount]
			  ,[OCCCode]
			  ,[USOC]
			  ,[Service]
			  ,[BillDate]
			  ,[ChargeType])
			 select * from (
				SELECT  coalesce(fs.ParentID,[dbo].[fnRemoveLeadingZeros]([Customer])) as Customer,
					replace(replace(replace(replace([MasterBTN],' ',''),'(',''),')',''),'-','') as MasterBTN,
					replace(replace(replace(replace([BTN],' ',''),'(',''),')',''),'-','') as BTN,
					[Date], 
					Amount, 
					Code as OCCCode,
					USOC,
					Service,
					@BilLDate as BillDate,
					LEFT(ChargeType, len(chargetype)-1) ChargeType -- strip off trailing comma
					--Convert(date, substring(BillDate,5,2) + '/' + SUBSTRING(BillDate, 7,2) + '/' + SUBSTRING(BillDate, 1, 4)) BillDate			   
					from [dbo].[HostedTempOCCWholesale] w
					LEFT JOIN vw_FluentStreamParentAccounts fs on [dbo].[fnRemoveLeadingZeros](w.[Customer]) = fs.AccountID

			   WHERE isnull(USOC, '') != '' -- filter out the FS/CCI interco charges
			   ) occ
			   --WHERE occ.BillDate = @BillDate
			   
			delete from [dbo].[HostedTempOCCWholesale];   


END









GO


