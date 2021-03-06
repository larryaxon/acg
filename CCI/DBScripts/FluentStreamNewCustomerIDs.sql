/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 *
  FROM [CityHostedTest].[dbo].[LLACustomerAccountDataExportWithNewIDs]

Create View LLAFluentStreamCustomerIDs as
select c.Entity, c.EntityOwner, c.LegalName, x.[Customer Name],c.AlternateName,c.AlternateID, x.[Account ID] from LLACustomerAccountDataExportWithNewIDs x
inner join Entity c on c.AlternateID = x.OldExternalID
where entitytype = 'Customer' and getdate() between isnull(startdate, '1/1/1900') and isnull(enddate, '12/31/2100')

select * from attribute where entity = 'default' and itemtype = 'entity' and item = 'customer'

select * from LLAFluentStreamCustomerIDs where entity = '22749'
select * from attribute where entity = '22749'
select * from [dbo].[vw_AttributeNonXML] where entity = '22749'

select e.entity, e.legalname, e.alternateid FSID, x.Value OldID
FROM entity e
INNER JOIN vw_AttributeNonXML x on e.entity = x.entity and x.itemtype = 'entity' and x.item = 'customer' and x.name = 'OldAlternateID'
where x.Item = 'Customer'

exec [dbo].CreateAttributeNode '22749', 'Entity', 'Customer', 'OldAlternateID',  '00000071'

update attribute
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
  <attribute name="OldAlternateID">

  </attribute>
  <attribute name="QBExported" />
</attributes>'
where entity = 'default' and itemtype = 'entity' and item = 'customer'