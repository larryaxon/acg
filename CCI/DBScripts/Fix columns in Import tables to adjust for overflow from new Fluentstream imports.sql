select * from HostedMatchedSummaryToll where billdate = '5/1/2021'

--select * from HostedImportTollWholesale where billingyear = 2021 and billingmonth = 5
--select * from HostedImportTollRetail where billingyear = 2021 and billingmonth = 5
--delete from HostedImportTollWholesale where billingyear = 2021 and billingmonth = 5
--delete from HostedImportTollRetail where billingyear = 2021 and billingmonth = 5

select * from HostedTempTollRetail
select * from HostedTempTollWholesale


select * from HostedMatchedSummaryToll where billdate = '5/1/2021'
--delete from HostedMatchedSummaryToll where billdate = '5/1/2021'

alter table hostedmatchedsummarytoll
alter column masterwholesalebtn nvarchar(128) null

alter table hostedmatchedsummarytoll
alter column wholesalebtn nvarchar(128) null 

alter table hostedmatchedsummarytoll
alter column wholesalebtn nvarchar(128) null 

alter table hostedtaxtransactions
alter column taxType nvarchar(1024) null
