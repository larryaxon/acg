USE [CityHostedTest]
GO

/****** Object:  StoredProcedure [dbo].[ImportHostedTollWholesale]    Script Date: 7/22/2017 12:50:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		Larry Axon
-- Create date: 7/22/2017
-- Description:	Import tab delimited format toll Wholesale import to existing table (Red Rock)
-- =============================================
--exec [dbo].[ImportHostedRedRockTollWholesale] 'C:\Data\Red Rock Imports\2017\2017-07\Wholesale Toll Red Rock-201707.txt', '2017','7'
create PROCEDURE [dbo].[ImportHostedRedRockTollWholesale]	(@FilePath nvarchar (255),@BillingYear varchar(10),@BillingMonth varchar(10))
	
AS
BEGIN
/*h
AcctNum	TN	Billig Year	Billing Month	Billed TN	Call Date	Connect Time	Other TN	Other City	Other State	Rate	Minutes	Charge
R1180	5863158102	2017	5	5863158102	4/3/2017	01:30.3	5867261553	UTICA	MI	0.04	1.4	0
*/
/*
Account Number Customer id	billed tn	billed description	RedRock Charge	Redrock Reseller	Billed Date	other tn	other description	call Date
*/
	if object_id (N'"HostedTempRedRockTollWholesale"', N'U') is NULL
		CREATE TABLE [dbo].[HostedTempRedRockTollWholesale](
			[Account Number] [varchar](50) NULL,
			[Customer id] [varchar](50) NULL,
			[Billed TN] [varchar](50) NULL,
			[billed description] varchar(200) NULL,
			[RedRock Charge] money NULL,
			[Redrock Reseller] money NULL,
			[Billed Date] datetime null,
			[Other TN] [varchar](50) NULL,
			[Other Description] [varchar](50) NULL,
			[Call Date] datetime null
		) ON [PRIMARY]

	delete from [dbo].[HostedTempRedRockTollWholesale];

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempRedRockTollWholesale FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = 2, FIELDTERMINATOR = ''\t'', ROWTERMINATOR = ''\n'')'
	
	EXEC sp_executesql @BulkInsert;
	return
    if not exists (select top 1 [Billed Date]
                 from HostedTempRedRockTollWholesale 
                where datepart(year, [Billed Date]) <> @billingyear or datepart(month, [Billed Date]) <> @BillingMonth)
       begin
            print 'Good Date'
			insert into hostedimporttollWholesale (customernumber,billingyear,billingmonth,btn,messagetype,fromnumber,calldate,duration,
					tonumber,tocity,tostate,rate,charge)
			 select [Account Number] customernumber,
					datepart(year, [Billed Date]) BillingYear,
					datepart(month, [Billed Date]) billingmonth,
					replace(replace(replace(replace([Billed TN],' ',''),'(',''),')',''),'-','') as btn,
					null messagetype,
					replace(replace(replace(replace([Billed TN],' ',''),'(',''),')',''),'-','') as fromnumber,
					[Call Date] calldate,
					0 duration,
					replace(replace(replace(replace([Other TN],' ',''),'(',''),')',''),'-','') as tonumber,
					[Other Description] tocity,
					null tostate,
					0 rate,
					[RedRock Charge] charge
			   from HostedTempRedRockTollWholesale;
			   
			delete from HostedTempRedRockTollWholesale;   
       end
    else
       begin
            print 'Bad Date'
       end

END





GO


