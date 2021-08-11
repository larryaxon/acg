select * from vw_cityhostedusocs where usocretail = 'wdidov'

select * from hostedimportoccretail
where usoc = 'wdidov' and billdate between '5/1/2021' and '7/1/2021' 
and customer = '21889'

select * from HostedImportOCCWholesale
where usoc = 'WDIDNN' and billdate between '5/1/2021' and '7/1/2021' 
and customer = '21889'

select * from hostedmatchedocc
where coalesce (WholesaleBillDate, RetailBillDate)
between '5/1/2021' and '7/1/2021' 
 and CustomerID in ('24828')  
and  (RetailUSOC in ('WDIDOV') or wholesaleusoc in ('WDIDNN'))
order by CustomerID, RetailUSOC, chargetype

--select * from entity where alternateid = '14489' --24776
--select * from entity where alternateid =  '14488' --24777

select  * from [sandbox].vw_CityHostedOCCSummary where billdate between '5/1/2021' and '7/1/2021' 
 and CustomerID in ('24828')  
and  RetailUSOC in ('WDIDOV')

select  * from [sandbox].[vw_CityHostedMarginOCCUSOCSummary2]  where billdate between '5/1/2021' and '7/1/2021' 
 and CustomerID in ('24828')  
and  RetailUSOC in ('WDIDOV')


select * from  sandbox.vw_DealerMarginExtract where billdate between '5/1/2021' and '7/1/2021' 
 and CustomerID in ('24828')  
and  RetailUSOC in ('WDIDOV')
and chargetype = 'OCC'
--and  OCCChargeType = 'install'
order by CustomerName, RetailUSOC

select * from  [sandbox].vw_AnalysisMargin where billdate between '5/1/2021' and '7/1/2021' 
 and CustomerID in ('24828')  
and  RetailUSOC in ('WDIDOV')
and chargetype = 'OCC'
--and  OCCChargeType = 'install'
order by CustomerName, RetailUSOC

select * from  [sandbox].vw_DealerMargin where billdate between '5/1/2021' and '7/1/2021' 
 and CustomerID in ('24828')  
and  RetailUSOC in ('WDIDOV')
and chargetype = 'OCC'
--and  OCCChargeType = 'install'
order by CustomerName, RetailUSOC



