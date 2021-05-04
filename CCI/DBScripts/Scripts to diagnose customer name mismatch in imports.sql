select * from [dbo].[hostedmatchedmrc] mrc
where coalesce(mrc.retailbilldate, mrc.wholesalebilldate) = '1/1/2021'
and 'W911D' in (retailusoc, wholesaleusoc) and customerid in ('24425','90605')

select top 10 * from vw_CHUsocs u where u.USOCRetail like '%911%'

select * from entity where legalname like '%sooner%'

select * from hostedmatchedmrc mrc  where mrc.CustomerID in ('24425','90605') and coalesce(mrc.retailbilldate, mrc.wholesalebilldate) = '1/1/2021'

select * from hostedimportmrcretail r where billdate = '1/1/2021' and customername like '%sooner%' and usoc  like '%911%'

select * from hostedimportmrcwholesale r where billdate = '1/1/2021' and customername like '%sooner%' and usoc  like '%911%'

select * from [dbo].[ExternalIDMapping] c where c.InternalID = '24425'

update [dbo].[ExternalIDMapping] 
set internalid = '24800'
where externalid = 'BazeCorp Investments' and internalid = '24425'

select * from entity where legalname like '%bazecorp%'


select * from hostedimportmrcwholesale where billdate = '1/1/2021' and customername = 'BazeCorp Investments'
