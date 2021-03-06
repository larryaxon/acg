/****** Script for SelectTopNRows command from SSMS  ******/
SELECT i.*
  FROM [CityHosted].[dbo].[vw_CityHostedInvoiceSummary] i
  where BillDate = '8/1/2017'
  and CustomerID in ( '20645', '24412', '24516', '23268')

  --select * from entity where legalname like '%Trusted Guardian%'

  DECLARE @CUSTOMERID as varchar(30)= '20645'
  DECLARE @extid as varchar(20) 
  select @extid =  alternateid from entity where entity = @CUSTOMERID
  select * from hostedmatchedmrc where customerid = @CUSTOMERID and coalesce(wholesalebilldate, retailbilldate) = '8/1/2017'
  select * from hostedmatchedocc where customerid = @CUSTOMERID and coalesce(wholesalebilldate, retailbilldate) = '8/1/2017'
  select Billdate, customer, sum(taxamount) totaltax from hostedtaxtransactions where  customer = @extid and BillDate = '8/1/2017' group by Billdate, customer
  select customerid, coalesce(WholesaleBillingYear, retailbillingyear), coalesce(WholesaleBillingMonth, retailbillingmonth), sum(RetailCharge) charge 
  from HostedMatchedToll where customerid = @CUSTOMERID and coalesce(WholesaleBillingYear, retailbillingyear) = 2017 and coalesce(WholesaleBillingMonth, retailbillingmonth) = 8
  group by customerid, coalesce(WholesaleBillingYear, retailbillingyear), coalesce(WholesaleBillingMonth, retailbillingmonth)
