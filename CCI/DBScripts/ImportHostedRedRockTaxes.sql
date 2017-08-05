USE [CityHostedTest]
GO

/****** Object:  StoredProcedure [dbo].[HostedTempRedRockTaxes]    Script Date: 7/29/2017 7:10:04 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Larry Axon
-- Create date: 7/29/2017
-- Description:	Import tab delimited format Tax import to existing table (Red Rock)
-- =============================================
--exec [dbo].[ImportHostedRedRockTax] 'C:\Data\Red Rock Imports\2017\2017-07\TAX Red Rock-201707.txt', '7/1/2017'
alter PROCEDURE [dbo].[ImportHostedRedRockTax]	(@FilePath nvarchar (255),@BillDate varchar(10))
	
AS
BEGIN
/*
Account Number	Customer ID	Customer	STATE	COUNTY	CITY	TAX AUTHORITY	GEOCODE	TAX TYPE	TAX CATEGORY	DESCRIPTION	REVENUE	TAXES	TAX RATE	LINE COUNT
R1007			1007		Glove IT LLC	AZ	MARICOPA	SCOTTSDALE	FEDERAL	0401365000	035	0	FEDERAL UNIVERSAL SERVICE FUND	1933.59	215.98	0.11	9

*/

	if object_id (N'"HostedTempRedRockTaxes"', N'U') is NULL
		CREATE TABLE [dbo].HostedTempRedRockTaxes(
			[Account Number] [varchar](50) NULL,
			[CustomerID] [varchar](50) NULL,
			[Customer] [varchar](50) NULL,
			[State] [varchar](50) NULL,
			[County] [varchar](50) NULL,
			[Tax Authority] [varchar](50) NULL,
			[Geocode] [varchar](50) NULL,
			[Price] [varchar](50) NULL,
			[Tax Type] [varchar](50) NULL,
			[Tax Category] [varchar](50) NULL,
			[Description] [varchar](255) NULL,
			[Revenue] [varchar](50) NULL,
			[Taxes] [varchar](50) NULL,
			[Tax Rate] varchar(50) NULL,
			[Line Count] varchar(50) NULL
		) ON [PRIMARY]

	delete from [dbo].HostedTempRedRockTaxes;

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempRedRockTaxes FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = 3, FIELDTERMINATOR = ''\t'', ROWTERMINATOR = ''\n'')'
	print @billdate
	EXEC sp_executesql @BulkInsert;

-- The following check to make sure the file we are importing has the correct bill date is commented because this RR file does not have a date.
-- If we get a new file version that has a date, we will reinstate this

    --if not exists (select top 1 Datepart(month, [Bill Date]) m, datepart(year, [Bill Date]) y
    --             from HostedTempRedRockTaxes 
    --            where  Datepart(month, [Bill Date]) <> datepart(month, @BillDate) and datepart(year, [Bill Date]) <> datepart(year, @BillDate))
    --   begin
    --        print 'Good Date'
			insert into [dbo].[HostedImportTaxes] (Customer, MasterBTN, [Level], LevelType, Jurisdiction,  Rate, TaxType, Title, TaxAmount, BillDate, [Sign])
			 select [Account Number] as Customer,
					null as MasterBTN,
					0 as [Level],
					Geocode as LevelType,
					CASE WHEN GEOCODE = 'STATE' then [State] WHEN GEOCODE = 'County' THEN [County] ELSE [Tax Authority] END Jurisdiction, 
					[Tax Rate] Rate, 
					null [Tax Type],
					[Description] Title,
					Taxes as TaxAmount,
					@BillDate as BillDate,
					Null as Sign
			   from [dbo].HostedTempRedRockTaxes;
			   
			--delete from [dbo].HostedTempRedRockTaxes;   
    --   end
    --else
    --   begin
    --        print 'Bad Date'
    --   end

END






GO


