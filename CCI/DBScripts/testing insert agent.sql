/****** Script for SelectTopNRows command from SSMS  ******/

drop table import.attributecustomerbackup

select * into import.attributecustomerbackup from attribute where itemtype = 'entity' and item = 'customer'

WITH AgentSource (Entity, ItemType, Item, Attributes, AgentNode)
AS
(
SELECT a.Entity, a.ItemType, a.Item, a.Attributes, Convert(xml, '<attribute name="Agent"><valuehistory value="' + ag.Entity + '"/></attribute>' ) agentNode
  FROM attribute a
  left join [import].[CustomerMasterList] cm
    on a.entity = Convert(varchar(50), cm.customerid) and a.item = 'customer' and a.itemtype = 'entity'
  left join Entity ag on cm.SourceName = ag.LegalName and ag.entitytype = 'Agent'
  where cm.source = 'agent' and ag.legalname is not null
  ) --select 'insert ' + convert(varchar(256),AgentNode) + ' into (/attributes)[1]' from agentsource where entity = '23287'
  UPDATE a
  SET Attributes.modify('insert sql:column("AgentNode") into (/attributes)[1]')
   from Attribute a
   INNER JOIN AgentSource s ON a.Entity = s.Entity
   where s.Entity = '23287'

select * from attribute where entity = '23287'

update attribute
set attributes = '<attributes>
  <attribute name="BillingAddress1">
    <valuehistory value="101 Richfield Ave" startdate="10/1/2014" lastmodifiedby="MarkK-User" lastmodifieddatetime="10/1/2014 10:25:22 AM" />
  </attribute>
  <attribute name="BillingAddress2" />
  <attribute name="BillingCity">
    <valuehistory value="El Cajon" startdate="10/1/2014" lastmodifieddatetime="10/1/2014 10:25:22 AM" />
  </attribute>
  <attribute name="BillingState">
    <valuehistory value="CA" startdate="10/1/2014" lastmodifieddatetime="10/1/2014 10:25:22 AM" />
  </attribute>
  <attribute name="BillingZip">
    <valuehistory value="92020-" startdate="10/1/2014" lastmodifieddatetime="10/1/2014 10:25:22 AM" />
  </attribute>
  <attribute name="PaymentType">
    <valuehistory value="CreditCard" startdate="10/17/2014" lastmodifiedby="MarkK-User" lastmodifieddatetime="10/17/2014 9:38:50 AM" />
  </attribute>
  <attribute name="ContactType" />
  <attribute name="EmailAddress" />
  <attribute name="BillingCellPhone" />
  <attribute name="BillingPhone" />
</attributes>'
WHERE entity = '23287' and itemtype = 'Entity' and item = 'customer'

