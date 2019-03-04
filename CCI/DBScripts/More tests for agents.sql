select * from entity where entitytype = 'agent' and entity like 'ag-house%'
select * from attribute where entity = '23287'

select * from entity e
inner join attribute a on e.entity = a.entity
where a.item = 'customer'
and convert(varchar(max), attributes)  like '%"Agent"%'
and a.Entity in ('23287', '20025')

select * from vw_AttributeNonXML a inner join entity e on a.entity = e.Entity
where itemtype = 'entity' and item = 'customer' and Name = 'agent'
and value <> 'AG-HOUSEACCOUNT'
and a.Entity in ('23287', '20025')

select * from SalesOrDealerCustomers s 
order by customer

Select d.Entity,  isnull(d.LegalName, 'CCI as CHS Dealer') DealerName
          from entity c 
          inner join (
			select distinct Customer from NetworkInventory where Carrier = 'CityHosted'
			) ni on ni.Customer = c.Entity
          left join SalesOrDealerCustomers dc on c.Entity = dc.customer 
		  left join entity d on d.entity = dc.SalesOrDealer
		  Where c.entity = '23287' and dc.SalesType = 'Agent'

Select d.Entity,  isnull(d.LegalName, 'CCI as CHS Dealer') DealerName
          from entity c 
          inner join (
			select distinct Customer from NetworkInventory where Carrier = 'CityHosted'
			) ni on ni.Customer = c.Entity
          left join SalesOrDealerCustomers dc on c.Entity = dc.customer 
		  left join entity d on d.entity = dc.SalesOrDealer
		  Where c.entity = '20025' and dc.SalesType = 'Agent'