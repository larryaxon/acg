/*
	queries to find John's "disappearing items" from 3 month report
*/
--select billdate, count(*) from sandbox.vw_DealerMargin 
--where billdate >=  '5/1/2021'
--group by billdate

select * from sandbox.vw_DealerMargin dm
where customerid in ('24779',
'24796',
'24716',
'24788',
'24825',
'24828',
'24716',
'24828',
'24829')
and billdate >= '5/1/2021'
and chargetype = 'occ'
and 
(isnull(retailusoc,'') in ('','SIPV',
'W911D',
'WACMDV',
'WDIDOV',
'WDIDV',
'WIPV',
'WVTN2V')
or
isnull(wholesaleusoc,'') in ('','WACMD',
'WDIDNN',
'WIP600',
'WNIP11',
'WVTN2')
)
order by billdate, CustomerName, RetailUSOC, WholesaleUSOC

select Customer, [Date], USOC,  sum(Amount) from HostedImportOCCWholesale w
where billdate = '5/1/2021' 
and customer in ('19929','15122')
and w.USOC in ('WACMD',
'WDIDNN',
'WIP600',
'WNIP11',
'WVTN2')
Group by 
Customer, [Date], USOC

select  wholesalebilldate, customerid, wholesaleusoc, retailusoc, sum(wholesaleamount)  from hostedmatchedocc o
where Coalesce(Retailbilldate, wholesalebilldate) = '5/1/2021' 
and customerid in ('24716','24788')
and coalesce(wholesaleusoc, retailusoc) in ('WACMD',
'WDIDNN',
'WIP600',
'WNIP11',
'WVTN2')
Group by 
wholesalebilldate, customerid, wholesaleusoc, retailusoc

select * from [sandbox].vw_CityHostedOCCSummary
where billdate= '7/1/2021' 
and customerid in ('24716','24788')
and coalesce(wholesaleusoc, retailusoc) in ('WACMD',
'WDIDNN',
'WIP600',
'WNIP11',
'WVTN2')