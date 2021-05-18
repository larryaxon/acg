

/****** Object:  StoredProcedure [dbo].[ImportHostedTax]    Script Date: 5/18/2021 4:01:52 PM ******/
DROP PROCEDURE [dbo].[ImportHostedTax]
GO

/****** Object:  StoredProcedure [dbo].[ImportHostedTax]    Script Date: 5/18/2021 4:01:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Larry Axon
-- Create date: 9/19/2015
-- Description:	Import csv format mrc retail import to existing table
-- =============================================
--exec [dbo].[ImportHostedTax] 'C:\Data\Saddleback Imports\2021\2021-05\Completed\Tax-202105.csv', '202105'
CREATE PROCEDURE [dbo].[ImportHostedTax]	(@FilePath nvarchar (255),@BillDate varchar(10), @FirstRow int = 2)
	
AS
BEGIN
/*
Transaction_id	uomID	Tax_Type_Code	Tax_Type	State	County	City	Zip	Sales_Type	Net_Amount	Tot_Tax	Gross_Amount	Account_Number	Invoice_Number	Bill_Number	DocCode	CREATED_DATE	TotalGrossRevenue	TotalTax	TotalRevenue	BundleName	ProductName	External_Id	Country	Plan_Name	Identifier	Charge_From	Charge_To	Bill_Start_Date	Bill_End_Date
4DEF4F7E-AA4E-11EB-942D-D4DF72495A0E	1.37E+18	126	State Universal Service Fund-Toll	AZ	MARICOPA	PHOENIX	85029	1002	0.222704	0.000704	0.2	17867	IN-80008213429	BI23136		May 1, 2021, 12:24 AM	0.2	0.022704	0.222704		DID Numbers-Retail/DID Numbers	1956	UNITED STATES	DID Numbers	SUB7674	5/1/21	5/31/21	4/1/21	4/30/21
*/
--DROP TABLE HostedTempTax
--Truncate Table HostedTempTax
if object_id (N'"HostedTempTax"', N'U') is NULL
Create Table HostedTempTax (
	Transaction_id	nvarchar(50) NULL,
	uomID	nvarchar(50) NULL,
	Tax_Type_Code	nvarchar(50) NULL,
	Tax_Type	nvarchar(1024) NULL,
	[State]	nvarchar(50) NULL,
	County	nvarchar(256) NULL,
	City	nvarchar(50) NULL,
	Zip	nvarchar(50) NULL,
	Sales_Type	nvarchar(50) NULL,
	Net_Amount	nvarchar(50) NULL,
	Tot_Tax	nvarchar(50) NULL,
	Gross_Amount	nvarchar(50) NULL,
	Account_Number	nvarchar(50) NULL,
	Invoice_Number	nvarchar(50) NULL,
	Bill_Number	nvarchar(50) NULL,
	DocCode	nvarchar(50) NULL,
	CREATED_DATE	nvarchar(128) NULL,
	TotalGrossRevenue	nvarchar(50) NULL,
	TotalTax	nvarchar(50) NULL,
	TotalRevenue	nvarchar(50) NULL,
	BundleName	nvarchar(50) NULL,
	ProductName	nvarchar(256) NULL,
	External_Id	nvarchar(50) NULL,
	Country	nvarchar(256) NULL,
	Plan_Name	nvarchar(256) NULL,
	Identifier	nvarchar(50) NULL,
	Charge_From	nvarchar(256) NULL,
	Charge_To	nvarchar(256)  NULL,
	Bill_Start_Date nvarchar(50) NULL,
	Bill_End_Date	nvarchar(256) NULL)
	ON [PRIMARY];
	

	delete from [dbo].HostedTempTax;

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempTax FROM ''' +
		@ImportFileName +
		N''' WITH (FORMATFILE=''C:\Data\FluentStreamTaxFormatFile.xml'', FIRSTROW = ' + Convert(varchar(5), @FirstRow) + ', FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'
	
	EXEC sp_executesql @BulkInsert;

    if not exists (select top 1 [Charge_From]
                 from HostedTempTax 
                where [Charge_From] not between @BillDate and DateAdd(month, 1, @BilLDate))
       begin
            print 'Good Date'
			insert into [dbo].[HostedImportTaxes] ([Customer]
      ,[MasterBTN]
      ,[Level]
      ,[LevelType]
      ,[Jurisdiction]
      ,[Rate]
      ,[TaxType]
      ,[Title]
      ,[TaxAmount]
      ,[BillDate]
      ,[Sign])
			 select 
					Account_Number as [Customer],
					'' as MasterBTN,
					'' as Level,
					'' AS [LevelType],
					'' as [Jurisdiction],
					0 as [Rate],
					Tax_Type as TaxType,
					ProductName as Title,
					Tot_Tax as Amount,
					Charge_From as ConnectionDate,
					Charge_From as BillDate
			   from [dbo].HostedTempTax;
			   
			delete from [dbo].HostedTempTax;   
       end
    else
       begin
            print 'Bad Date'
       end


END
GO

