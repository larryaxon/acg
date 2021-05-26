/*
	Script to add new FluentStream AlternateID to Customers and revise the dependencies to use it

	Note: This needs to have the [dbo].[LLACustomerAccountDataExportWithNewIDs] table imported first
*/
--EXEC LLA_TEMP_UpdateNewFLuentStreamCuxtomerIDs
/*
     STEP ONE: CreateView and Proc to change alternateid in Entity and add old value to attribute
*/

GO
/****** Object:  View [dbo].[LLAFluentStreamCustomerIDs]    Script Date: 5/18/2021 10:59:55 AM ******/
DROP VIEW [dbo].[LLAFluentStreamCustomerIDs]
GO
--select * from [dbo].[LLAFluentStreamCustomerIDs]
Create View [dbo].[LLAFluentStreamCustomerIDs] as
select c.Entity, c.EntityOwner, c.LegalName, x.[Customer Name],c.AlternateName,c.AlternateID, x.[Account ID] FluentStreamID
from LLACustomerAccountDataExportWithNewIDs x
inner join Entity c on c.AlternateID = x.OldExternalID
where entitytype = 'Customer' and getdate() between isnull(startdate, '1/1/1900') and isnull(enddate, '12/31/2100')
GO
DROP VIEW vw_FluentStreamParentAccounts
GO
Create View vw_FluentStreamParentAccounts as 
	select b.[Account ID] ParentID, a.[Account ID] AccountID, a.OldExternalID, a.[Billing Account Number] BillingAcct,
		a.[Customer Name] CustomerName, a.[Parent Account] ParentName --, a.[Customer Name] CustomerName , a.{Parent Account] ParentName
	 from ( SElECT * FROM LLACustomerAccountDataExportWithNewIDs 
			where isnull([parent account], '') != '') a
	INNER JOIN LLACustomerAccountDataExportWithNewIDs b on a.[Parent Account] = b.[Customer Name]
GO

-- Create procedure to update Entity and Attribute tables
/****** Object:  StoredProcedure [dbo].[LLA_TEMP_UpdateNewFLuentStreamCuxtomerIDs]    Script Date: 5/18/2021 11:01:17 AM ******/
DROP PROCEDURE [dbo].[LLA_TEMP_UpdateNewFLuentStreamCuxtomerIDs]
GO


CREATE  PROCEDURE [dbo].[LLA_TEMP_UpdateNewFLuentStreamCuxtomerIDs] AS
DECLARE @EntityID as varchar(50) 
DECLARE @OldID as varchar(50)
DECLARE @NewID as varchar(50)

--Select * INTO EntityBackup from Entity

DECLARE OldIds CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR 
SELECT distinct Entity from LLAFluentStreamCustomerIDs

OPEN OldIds
FETCH NEXT FROM OldIds INTO @EntityID
WHILE @@FETCH_STATUS = 0
BEGIN 
    Select @Oldid = AlternateID from Entity Where Entity = @EntityID
	Select @NewID = FluentStreamID from LLAFluentStreamCustomerIDs where Entity = @EntityID
	PRINT 'E:' + @EntityID + ' O:' + @OldID + ' N:' + @NewID
	exec [dbo].CreateAttributeNode @EntityID, 'Entity', 'Customer', 'OldAlternateID',  @Oldid
	Update Entity SET AlternateID = @NewID Where Entity = @EntityID
    FETCH NEXT FROM OldIds INTO @EntityID
END
CLOSE OldIds
DEALLOCATE OldIds
GO
/*
     STEP TWO: Execute the Proc to change alternateid in Entity and add old value to attribute
*/
DECLARE @AttributeStepRun as bit = 0
IF @AttributeStepRun = 0
BEGIN
	EXEC LLA_TEMP_UpdateNewFLuentStreamCuxtomerIDs
END
GO
/*
     STEP THREE: Create the external id table that allows
*/

/****** Object:  Table [dbo].[EntityAlternateIDs]    Script Date: 5/17/2021 11:32:35 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EntityAlternateIDs]') AND type in (N'U'))
DROP TABLE [dbo].[EntityAlternateIDs]
GO

/****** Object:  Table [dbo].[EntityAlternateIDs]    Script Date: 5/17/2021 11:32:35 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EntityAlternateIDs](
	[Entity] [varchar](50) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[ExternalServiceName] varchar(50) NULL,
	[ExternalID] [varchar](50) NULL,
 CONSTRAINT [PK_EntityAlternateIDs] PRIMARY KEY CLUSTERED 
(
	[Entity] ASC,
	[StartDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/*
     STEP FOUR: Populate the EntityExternalID Table
*/
Truncate Table EntityAlternateIDs
-- Add OLd Saddleback IDs
INSERT INTO EntityAlternateIDs (Entity, StartDate, EndDate, ExternalServiceName, ExternalID)
select e.entity, '1/1/1900', '4/30/2021', 'SaddleBack' ExternalServiceName, x.Value ExternalID
FROM entity e
INNER JOIN vw_AttributeNonXML x on e.entity = x.entity and x.itemtype = 'entity' and x.item = 'customer' and x.name = 'OldAlternateID'
where x.Item = 'Customer'

-- Add new FluentStream IDs
INSERT INTO EntityAlternateIDs (Entity, StartDate, EndDate, ExternalServiceName, ExternalID)
select distinct e.entity, '5/1/2021', '12/31/2100', 'FluentStream' ExternalServiceName, e.AlternateID ExternalID
FROM entity e
where e.entitytype = 'Customer'

--select * into LLAEntityAlternateIDsBackup from EntityAlternateIDs

Update aid 
SET ExternalID = p.ParentID
FROM EntityAlternateIDs aid
INNER JOIN vw_FluentStreamParentAccounts p on aid.ExternalID = p.AccountID

update e
SET AlternateID = p.ParentID
FROM Entity e
INNER JOIN  vw_FluentStreamParentAccounts p on e.AlternateID = p.AccountID

--Select e.Entity, e.LegalName, e.AlternateID, p.ParentID
--FROM Entity e
--INNER JOIN  vw_FluentStreamParentAccounts p on e.AlternateID = p.AccountID

--select * From EntityAlternateIDs where ExternalServiceName = 'FluentStream'

--select * from vw_AttributeNonXML where itemtype = 'entity' and item = 'customer' and name = 'AlternateID'

