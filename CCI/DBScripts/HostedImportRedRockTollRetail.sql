USE [CityHostedTest]
GO

/****** Object:  StoredProcedure [dbo].[ImportHostedTollRetail]    Script Date: 7/22/2017 12:50:38 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		Larry Axon
-- Create date: 7/22/2017
-- Description:	Import tab delimited format toll retail import to existing table (Red Rock)
-- =============================================
--exec [dbo].[ImportHostedRedRockTollRetail] 'C:\Data\Red Rock Imports\2017\2017-07\Retail Toll Red Rock-201707.txt', '2017','7'
alter PROCEDURE [dbo].[ImportHostedRedRockTollRetail]	(@FilePath nvarchar (255),@BillingYear varchar(10),@BillingMonth varchar(10))
	
AS
BEGIN
/*
AcctNum	TN	Billig Year	Billing Month	Billed TN	Call Date	Connect Time	Other TN	Other City	Other State	Rate	Minutes	Charge
R1180	5863158102	2017	5	5863158102	4/3/2017	01:30.3	5867261553	UTICA	MI	0.04	1.4	0
*/

	if object_id (N'"HostedTempRedRockTollRetail"', N'U') is NULL
		CREATE TABLE [dbo].[HostedTempRedRockTollRetail](
			[AcctNum] [varchar](50) NULL,
			[TN] [varchar](50) NULL,
			[Billig Year] [varchar](50) NULL,
			[Billing Month] [varchar](50) NULL,
			[Billed TN] [varchar](50) NULL,
			[Call Date] [varchar](50) NULL,
			[Connect Time] [varchar](50) NULL,
			[Other TN] [varchar](50) NULL,
			[Other City] [varchar](50) NULL,
			[Other State] [varchar](50) NULL,
			[Rate] [varchar](50) NULL,
			[Minutes] [varchar](50) NULL,
			[Charge] [varchar](50) NULL
		) ON [PRIMARY]

	delete from [dbo].[HostedTempRedRockTollRetail];

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempRedRockTollRetail FROM ''' +
		@ImportFileName +
		N''' WITH (FIRSTROW = 2, FIELDTERMINATOR = ''\t'', ROWTERMINATOR = ''\n'')'
	
	EXEC sp_executesql @BulkInsert;
	
    if not exists (select top 1 [Billig Year],[Billing Month] 
                 from HostedTempRedRockTollRetail 
                where [Billig Year] <> @billingyear or [Billing Month] <> @BillingMonth)
       begin
            print 'Good Date'
			insert into hostedimporttollretail (customernumber,billingyear,billingmonth,btn,messagetype,fromnumber,calldate,duration,
					tonumber,tocity,tostate,rate,charge)
			 select [AcctNum] customernumber,
					[Billig Year] BillingYear,
					[Billing Month] billingmonth,
					replace(replace(replace(replace([Billed TN],' ',''),'(',''),')',''),'-','') as btn,
					null messagetype,
					replace(replace(replace(replace([TN],' ',''),'(',''),')',''),'-','') as fromnumber,
					[Call Date] calldate,
					[Minutes] duration,
					replace(replace(replace(replace([Other TN],' ',''),'(',''),')',''),'-','') as tonumber,
					[Other City] tocity,
					[Other State] tostate,
					rate,
					charge
			   from HostedTempRedRockTollRetail;
			   
			delete from HostedTempRedRockTollRetail;   
       end
    else
       begin
            print 'Bad Date'
       end

END





GO


