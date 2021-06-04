USE [CityHostedProd]
GO

/****** Object:  StoredProcedure [dbo].[ImportHostedTax]    Script Date: 6/4/2021 12:18:32 PM ******/
DROP PROCEDURE [dbo].[ImportHostedTax]
GO

/****** Object:  StoredProcedure [dbo].[ImportHostedTax]    Script Date: 6/4/2021 12:18:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO








-- =============================================
-- Author:		Larry Axon
-- Create date: 6-45-2021
-- Description:	Import csv format mrc retail import to existing table
-- Modified 6/2021 by LLA for new tax file format
-- =============================================
--exec [dbo].[ImportHostedTax] 'C:\Data\Saddleback Imports\2021\2021-06\Completed\Tax-202106.csv', '6/1/2021'
-- select * from [HostedImportTaxes] where BillDate = '6/1/2021'
CREATE PROCEDURE [dbo].[ImportHostedTax]	(@FilePath nvarchar (255),@BillDate datetime, @FirstRow int = 2, @UseFormatFile bit = 0)
	
AS
BEGIN
/*
Transaction_id	uomID	Tax_Type_Code	Tax_Type	State	County	City	Zip	Sales_Type	Net_Amount	Tot_Tax	Gross_Amount	Account_Number	Invoice_Number	Bill_Number	DocCode	CREATED_DATE	TotalGrossRevenue	TotalTax	TotalRevenue	BundleName	ProductName	External_Id	Country	Plan_Name	Identifier	Charge_From	Charge_To	Bill_Start_Date	Bill_End_Date
4DEF4F7E-AA4E-11EB-942D-D4DF72495A0E	1.37E+18	126	State Universal Service Fund-Toll	AZ	MARICOPA	PHOENIX	85029	1002	0.222704	0.000704	0.2	17867	IN-80008213429	BI23136		May 1, 2021, 12:24 AM	0.2	0.022704	0.222704		DID Numbers-Retail/DID Numbers	1956	UNITED STATES	DID Numbers	SUB7674	5/1/21	5/31/21	4/1/21	4/30/21
*/
--DROP TABLE HostedTempTax
--Truncate Table HostedTempTax
if object_id (N'"HostedTempTax"', N'U') is NULL
CREATE TABLE [dbo].[HostedTempTax](
	[Account_Number] [varchar](50) NULL,
	[Invoice_Number] [varchar](50) NULL,
	[Tax_Type_Code] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[County] [varchar](50) NULL,
	[City] [varchar](50) NULL,
	[Zip] [varchar](50) NULL,
	[Tax_Type] [varchar](256) NULL,
	[Tot_Tax_Unrounded] [varchar](50) NULL,
	[Tot_Tax_Rounded] [varchar](50) NULL,
	[Gross_Amount] [varchar](50) NULL
) ON [PRIMARY]

	delete from [dbo].HostedTempTax;

	Declare @BulkInsert nvarchar(2000);
	Declare @ImportFileName nvarchar(255);
	Set @ImportFileName = @FilePath;

	Set @BulkInsert =
		N'BULK INSERT dbo.HostedTempTax FROM ''' +
		@ImportFileName +
		N''' WITH (';
	if @UseFormatFile = 1		
		Set @BulkInsert = @BulkInsert + N'FORMATFILE=''C:\Data\FluentStreamTaxFormatFile.xml'', '
	Set @BulkInsert = @BulkInsert + N'FIRSTROW = ' + Convert(varchar(5), @FirstRow) + ', FIELDTERMINATOR = '','', ROWTERMINATOR = ''\n'')'
	
	print @BulkInsert
	EXEC sp_executesql @BulkInsert;



	insert into [dbo].[HostedImportTaxes] ([Customer]
      ,[MasterBTN]
      ,[Level]
      ,[LevelType]
      ,[Jurisdiction]
      ,[Rate]
      ,[TaxType]
      ,[Title]
      ,[TaxAmount]
      ,[BillDate])
			 select * FROM
				(Select 
					Coalesce(fs.ParentID, Account_Number) as [Customer],
					'' as MasterBTN,
					'' as Level,
					'' AS [LevelType],
					'' as [Jurisdiction],
					0 as [Rate],
					Tax_Type as TaxType,
					'' as Title,
					Convert(decimal(10,5), [Tot_Tax_Unrounded]) as Amount,
					@BilLDate as BillDate
			   from [dbo].HostedTempTax tx
			LEFT JOIN vw_FluentStreamParentAccounts fs on [dbo].[fnRemoveLeadingZeros](tx.Account_Number) = fs.AccountID

			   --WHERE case when isdate(Charge_From) = 1 then Convert(date,Charge_From) else '1/1/1900' end between @BillDate and dateadd(month, 1, @BillDate)
			   ) tax
			--WHERE tax.BillDate = @BillDate
			   
	delete from [dbo].HostedTempTax;   



END







GO


