-- add customers from City Hosted list (MRCDetail import temp table)

-- find customers that are not mapped
select distinct d.customer, m.InternalID CustomerID from MRCDetail d
left join ExternalIDMapping m on d.Customer = m.ExternalID and m.EntityType = 'Customer'
where m.InternalID is null

-- see if a customer is already there:
select * from entity where LegalName like '%fitness%' and EntityType = 'customer'

-- see what mappings already exist for a customer, to avoid dup keeps in following insert statement
select * from ExternalIDMapping where EntityType = 'customer' and ExternalID like 'CONSTRUCTION DIVERSIF%'

-- add mapping (after you have added or found a customer number) for all matching customers in MRCDetail
insert into ExternalIDMapping 
select distinct 'Payor', 'Payor', Customer, 'Customer', '22036', 'Axon-User', GETDATE()
 from MRCDetail where Customer like 'm %' and Customer <> 'MOUNTAINSIDE FITNESS CLUB STELLAR PKWY'
 
 -- add a customer to entity, attribute, and externalidmapping:
 exec dbo.addCustomer 'NORTHWINDS CONTACT SOLUTIONS'
 
 -- delete in case you add a customer by accident:
 
 --delete from ExternalIDMapping where InternalID = '22947'
 --delete from attribute where entity = '22947'
 --delete from entity where entity = '22947'