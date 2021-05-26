 select * from [dbo].[vw_CityHostedMarginAllSummary]   where customerid = '24570' and retailbilldate = '5/1/2021'
 select * from [dbo].[hostedtaxtransactions] tax
LEFT JOIN DBO.EntityAlternateIDs aid on tax.customer = aid.ExternalID and tax.billdate between aid.StartDate and aid.EndDate
 where tax.billdate = '5/1/2021'and aid.entity = '24370'

 select * from entity where entity = '24570'

 select * from hostedimporttaxes where customer = '6002'
 select * from hostedmatchedmrc where customerid = '24370' and coalesce(retailbilldate, wholesalebilldate) = '5/1/2021'
 select * from hostedmatchedtoll where coalesce(retailbillingyear, wholesalebillingyear) = 2021 and  
	coalesce(retailbillingmonth,wholesalebillingmonth) = 5 and customerid = '24370'

select  * from hostedimporttollretail where customernumber = '6002' and billingyear = 2021 and billingmonth = 5

select * from entityalternateids where entity = '24684'
select * from cityhostedprod.dbo.entity where entity = '24684'

update entityalternateids
set externalid = '9387'
where entity = '24684'

insert into EntityAlternateIDs
Values ('24684','1/1/1900', '4/30/2021', 'Saddleback', '00001833')

update entity
set alternateid = '9387' where entity = '24684'

