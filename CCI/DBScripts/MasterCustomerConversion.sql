USE CityHostedTest

/*
	Sript to import Master Customer Spreadsheet to the CityHosted DB
*/
/****** Script for SelectTopNRows command from SSMS  ******/
--SELECT TOP 1000 [MasterCusID]
--      ,[MasterCustomerName]
--      ,[CustomerID]
--      ,[CustomerName]
--      ,[RITID]
--      ,[Source]
--      ,[SourceName]
--      ,[BillStartDate]
--      ,[BillEndDate]
--  FROM [CityHostedTest].[import].[CustomerMasterList]
/*
select distinct sourceName Agent from [CityHostedTest].[import].[CustomerMasterList]
where source = 'agent'
--select * from sys.tables where name like '%dealer%'
--select * from dbo.SalesOrDealerCustomers
select * from entity where entity like '%chromis%'--= 'ACCRAM'
select * from attribute where entity = 'ACCRAM'
*/

DECLARE @MasterCustomerEntityTyoe as varchar(20) = 'MasterCustomer';
DECLARE @CCIENTITY as varchar(20) = 'CCI';
Declare @modifiedByUser as varchar(20) = 'LarryA-User'
DECLARE @ModifiedDate as datetime = getdate()

/**************************************************************************************************

	UNDO/ROLLBACK any previous runs of this script so we can rerun it

***************************************************************************************************/

DELETE from entity where entity like '9%' and EntityType = 'MasterCustomer';
delete from attribute where entity like '9%' and item = 'MasterCustomer';
DELETE FROM Attribute where entity = 'default' and ItemType = 'Entity' and Item = 'Dealer'
DELETE FROM entity where entitytype = 'Agent'

UPDATE ENtity
SET EntityOwner = @CCIENTITY
WHERE Entity like '2%' and EntityType = 'Customer';


/**************************************************************************************************

	Insert Master Customer Records from import file

***************************************************************************************************/

WITH MasterCustomers (Entity, EntityType, EntityOwner, LegalName, CustomerID)
AS
(
	Select 
		MasterCusID,
		@MasterCustomerEntityTyoe,
		@CCIENTITY,
		MasterCustomerName,
		Min(CustomerID) 
	FROM [import].[CustomerMasterList]
	GROUP BY MasterCusID, MasterCustomerName
)
  INSERT INTO Entity (Entity, EntityType, EntityOwner, LegalName, LastModifiedBy, LastModifiedDateTime, AlternateID, Address1, Address2, City, State, Zip)
  select m.Entity, m.EntityType, m. EntityOwner, m.LegalName, @modifiedByUser  LastModifiedBy, 
	@ModifiedDate LastModifiedDateTime,  c.AlternateID AlternateID, c.Address1, c.Address2,
	c.City, c.State, c.Zip  
   from MasterCustomers m
  INNER JOIN Entity c on Convert(varchar(10), m.CustomerID) = c.Entity

Update Entity
SET EntityOwner = Convert(varchar(20), m.MasterCusID)
FROM Entity e
INNER JOIN [import].[CustomerMasterList] m on e.Entity = m.CustomerID
WHERE e.EntityType = 'Customer'

/***********************************************************************************************************

	Insert the Attribute records for the master customers

***********************************************************************************************************/

INSERT INTO Attribute (Entity,ItemType, Item, ItemHistory, Attributes, LastModifiedBy, LastModifiedDateTime)
select distinct e.Entity, 'Entity', 'MasterCustomer', '<ItemDates />', 
'<attributes>
  <attribute name="Address1">
    <valuehistory value="' + e.Address1+ '" />
  </attribute>
  <attribute name="City">
    <valuehistory value="' + e.City + '" />
  </attribute>
  <attribute name="State">
    <valuehistory value="' + e.State+ '" />
  </attribute>
  <attribute name="Zip">
    <valuehistory value="' + e.Zip + '" />
  </attribute>
  <attribute name="Country">
    <valuehistory value="USA" />
  </attribute>
</attributes>', 'LarryA-User', @ModifiedDate
FROM  entity e 
where e.entitytype = 'mastercustomer'

/**************************************************************************************************

	Drop (if exists) and then add new column SalesType to SalesOrDealerCustomers

***************************************************************************************************/
IF EXISTS(SELECT 1 FROM sys.columns 
          WHERE Name = N'SalesType'
          AND Object_ID = Object_ID(N'dbo.SalesOrDealerCustomers'))
ALTER TABLE dbo.SalesOrDealerCustomers DROP COLUMN SalesType

ALTER TABLE dbo.SalesOrDealerCustomers ADD SalesType varchar(20) NULL


DELETE FROM dbo.SalesOrDealerCustomers
WHERE SalesOrDealer in (select Entity FROM ENTITY where EntityType = 'Agebt')

UPDATE dbo.SalesOrDealerCustomers
SET SalesType = 'Dealer'

/***********************************************************************************************************

	Insert the Entity records for the Agents
	Then insert the attribute records 

***********************************************************************************************************/

-- Entities for Agents
INSERT INTO Entity (Entity, EntityType, EntityOwner, LegalName, LastModifiedBy, LastModifiedDateTime)
select Substring(Upper(Replace('AG-' + ag.Agent, ' ', '')), 1, 25) Entity, 'Agent' EntityTye, @CCIENTITY EntityOwner, ag.Agent LegalName, 
@modifiedByUser LastModifiedBy, @ModifiedDate LastModifiedDateTIme from 
(Select distinct SourceName Agent from [import].[CustomerMasterList]
WHERE Source = 'Agent') ag

-- Master Attribute for agents
INSERT INTO Attribute (Entity, ItemType, Item, ItemHistory, Attributes, LastModifiedBy, LastModifiedDateTime)
select Entity, ItemType, 'Agent', ItemHistory, Attributes, @ModifiedByUser, @ModifiedDate
 from attribute where entity = 'default' and ItemType = 'Entity' and Item = 'Dealer'

-- Attributes for Agents
Insert into Attribute (Entity, ItemType, Item, ItemHistory, Attributes, LastModifiedBy, LastModifiedDateTime)
Select Entity, 'Entity' ItemType, EntityType Item, '<ItemDates />', null, @modifiedByUser LastModifiedBy, @ModifiedDate LasModifiedDateTime
FROM entity
where entitytype = 'Agent'

-- Now add the salesordealer records for the agents
-- Note: per JAC email a customer can have both a dealer and an agent, so i do not disturb the existing dealer records but just add the agent ones

INSERT INTO SalesOrDealerCustomers (SalesOrDealer, Customer, LastModifiedBy, LastModifiedDateTime, SalesType)
Select distinct Substring(Upper(Replace('AG-' + imp.SourceName, ' ', '')), 1, 25) SalesOrDelear,  customerid Customer,
@modifiedByUser LastModifiedDateTime, @ModifiedDate LastMododifiedDateTime, 'Agent' SalesType
from import.CustomerMasterList imp 
where imp.source = 'agent'




