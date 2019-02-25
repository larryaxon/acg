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

/* Add Agent to the customer fields */
Update attribute
set attributes = '<attributes>
  <attribute name="Include">
    <valuehistory include="Default.EntityType.Business" lastmodifiedby="Axon-User" lastmodifieddatetime="6-21-2012 7:58 AM" />
  </attribute>
  <attribute name="StatusEventMasterList">
    <valuehistory tableheader="(StatusFilter~StatusEvent~Description~Status~BenefitEligible~UpdateEntityStartDate~UpdateEntityEndDate|New~Started~Started~Active~True~StartDate~|Active~Termed-PEO~Terminated - went to PEO~Terminated~False~~StartDate|Active~Termed-InHouse~Termed-went inhouse~Terminated~False~~StartDate|Active~Termed-OutofBusiness~Termed-went out of business~Terminated~False~~StartDate)" lastmodifiedby="Authored" lastmodifieddatetime="2009-03-07T14:35:00" />
  </attribute>
  <attribute name="IBP" />
  <attribute name="CustomerType">
    <valuehistory value="Telecom" />
  </attribute>
  <attribute name="PaymentType" />
  <attribute name="CustomerTypeTable">
    <valuehistory tableheader="(Hosted~City Hosted|Telecom~Telecom Products|All~Both Hosted and Telecom products)" />
  </attribute>
  <attribute name="AlternateID">
    <valuehistory value="@@AlternateID" />
  </attribute>
  <attribute name="QBExported" />
	<attribute name="Agent" />
	<attribute name="Day2YN" />
</attributes>'
where entity = 'default' and itemtype = 'entity' and item = 'customer'


Delete from Attribute
where Entity = 'Dictionary' and ItemType = 'Attribute' and Item = 'Day2YN'

Insert into Attribute (Entity, ItemType, Item, ItemHistory, Attributes, LastModifiedBy, LastModifiedDateTime)
values ('Dictionary', 'Attribute', 'Day2YN', '<ItemDates />', 
'<attributes>
  <attribute name="datatype">
    <valuehistory value="string" />
  </attribute>
</attributes>', 'Axon-User', getdate())





