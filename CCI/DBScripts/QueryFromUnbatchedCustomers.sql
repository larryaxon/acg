select distinct customername customer 
                    from hostedimportmrcwholesale mw 
                      left join ExternalIDMapping map on mw.customername = map.externalid and map.EntityType = 'customer'
                    where  map.internalid is null
                  union
                  select distinct customername customer 
                    from hostedimportmrcretail mr 
					  left join EntityAlternateIDs aid on aid.ExternalID = mr.CustomerNumber and mr.billdate between aid.StartDate and aid.EndDate
                      left join entity e on e.Entity = aid.Entity
                      left join ExternalIDMapping map on mr.customername = map.externalid and map.EntityType = 'customer'
                    where  e.entity is null and map.internalid is null
                  --union
                  --select distinct customer customer 
                  --  from hostedimportoccwholesale ow 
                  --    left join ExternalIDMapping map on ow.customer = map.externalid and map.EntityType = 'customer'
                  --  where  map.internalid is null and ow.customer not in ('80008154','80008213')

select top 100  * from hostedimportoccwholesale where customer <> '80008213' 


select top 100 * from ExternalIDMapping where entitytype = 'customer'

                  select distinct customername customer, mr.CustomerNumber, mr.BillDate 
                    from hostedimportmrcretail mr 
					  left join [dbo].[EntityAlternateIDs] aid on aid.ExternalID = mr.CustomerNumber and mr.BillDate between aid.startdate and aid.enddate
                      left join entity e on e.alternateid = mr.customernumber
                      left join ExternalIDMapping map on mr.customername = map.externalid and map.EntityType = 'customer'
                    where  e.entity is null and map.internalid is null

					select * from EntityAlternateIDs where ExternalID = '00000808'

select distinct CustomerNumber from HostedImportMRCRetail where billdate = '5/1/2021' 