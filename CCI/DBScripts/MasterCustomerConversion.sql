USE CityHostedTest

/*
	Sript to import Master Customer Spreadsheet to the CityHosted DB
*/
/****** Script for SelectTopNRows command from SSMS  ******
SELECT TOP 1000 [MasterCusID]
      ,[MasterCustomerName]
      ,[CustomerID]
      ,[CustomerName]
      ,[RITID]
      ,[Source]
      ,[SourceName]
      ,[BillStartDate]
      ,[BillEndDate]
  FROM [CityHostedTest].[import].[CustomerMasterList]
*/

DECLARE @MasterCustomerEntityTyoe as varchar(20) = 'MasterCustomer';
DECLARE @CCIENTITY as varchar(20) = 'CCI';
Declare @modifiedByUser as varchar(20) = 'LarryA-User'

DELETE from entity where entity like '9%' and EntityType = 'MasterCustomer';
DELETE FROM Attribute where Entity like '9%' and Item = 'MasterCustomer'

UPDATE ENtity
SET EntityOwner = @CCIENTITY
WHERE Entity like '2%' and EntityType = 'Customer';


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
	getdate() LastModifiedDateTime,  c.AlternateID AlternateID, c.Address1, c.Address2,
	c.City, c.State, c.Zip  
   from MasterCustomers m
  INNER JOIN Entity c on Convert(varchar(10), m.CustomerID) = c.Entity

Update Entity
SET EntityOwner = Convert(varchar(20), m.MasterCusID)
FROM Entity e
INNER JOIN [import].[CustomerMasterList] m on e.Entity = m.CustomerID
WHERE e.EntityType = 'Customer'

INSERT INTO Attribute (Entity, ItemType, Item, ItemHistory, Attributes, LastModifiedBy, LastModifiedDateTime)
Select Entity, 'Entity', 'MasterCustomer', '<ItemDates />', null, 'Axon-User', getdate()
FROM Entity where entitytype = 'MasterCustomer'




