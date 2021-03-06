/****** Script for SelectTopNRows command from SSMS  ******/
select * from 
(select acctnum, tn, sum(convert(money, [amt billed])) mrc
FROM ImportHostedRedRockmrcretail 
group by acctnum, tn) r
inner join (
SELECT  [AcctNum]
      ,[Billed TN] TN
      ,sum(convert(money,[Charge])) total
  FROM [CityHosted].[dbo].[ImportHostedRedRockTollRetail] 
  where charge <> '0'
  group by AcctNum, [Billed TN]) t on r.AcctNum = t.acctnum and r.tn = t.TN

  /****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [Account Number]
      ,[Customer]
      ,sum(Convert(money,[TAXES])) Tax
  FROM [CityHosted].[dbo].[ImportHostedRedRockTax]
  group by [Account Number], [Customer]

  /****** Script for SelectTopNRows command from SSMS  ******/
SELECT c.legalname, r.usoc, convert(money, r.[amt billed]) rprice, convert(money, w.[amt billed]) wprice, 
	convert(money, r.price) rcost , convert(money, w.price) wcost 
  FROM [CityHosted].[dbo].[ImportHostedRedRockOCCRetail] r
  inner join entity c on r.acctnum = c.AlternateID
  left join [dbo].[ImportHostedRedRockOCCWholesale] w on r.acctnum = w.[Account Number] and r.usoc = w.usoc and r.tn = w.tn

  /****** Script for SelectTopNRows command from SSMS  ******/
select sum(price) price, sum(cost) cost, sum(net) net, LegalName CustomerName FROM (
SELECT Convert(money,r.[amt billed]) price, Convert(money, w.[price]) cost, Convert(money,r.[amt billed]) - Convert(money, w.[price]) net, c.legalname --, r.*, w.*
  FROM [CityHosted].[dbo].[ImportHostedRedRockMRCRetail] r
  inner join entity c on r.acctnum = c.AlternateID
  inner join  [dbo].[ImportHostedRedRockMRCWholesale] w
  on r.AcctNum = w.[Account Number] and r.TN = w.tn and r.USOC = w.usoc) a
  group by LegalName











  