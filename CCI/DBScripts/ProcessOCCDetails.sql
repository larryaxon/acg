USE [CityHostedProd]
GO

/****** Object:  StoredProcedure [dbo].[ProcessOCCDetails]    Script Date: 7/2/2021 9:57:05 AM ******/
DROP PROCEDURE [dbo].[ProcessOCCDetails]
GO

/****** Object:  StoredProcedure [dbo].[ProcessOCCDetails]    Script Date: 7/2/2021 9:57:05 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Andy Werdeman>
-- Create date: <11/30/2012>
-- Description:	<Process OCC Import files>
-- =============================================
--exec [dbo].[ProcessOCCDetails] 'LarryA', '5/2/2021'
CREATE PROCEDURE [dbo].[ProcessOCCDetails] (@operator nvarchar(25), @billdate date)
AS
BEGIN
	SET NOCOUNT ON;



--Insert Wholesale 
	insert into hostedmatchedocc ([CustomerID],[WholesaleMasterBTN],[WholesaleBTN],[WholesaleDate],[WholesaleAmount],
		[WholesaleService],[WholesaleUSOC],[WholesaleBillDate],[LastModifiedDateTime],[LastModifiedBy], ChargeType)
	select coalesce(aid.entity,'Missing') as CustomerID,w.masterbtn, left(w.btn,10), w.date, w.amount, w.service, w.usoc, 
			w.billdate, getdate(), @operator, ChargeType
	  from hostedimportoccwholesale w 

--	   left join (select top 1 customerid,wholesalebtn from hostedmatchedmrc) m on m.wholesalebtn = left(w.btn,10)
       outer apply (select top 1 customerid,wholesalebtn 
                    from hostedmatchedmrc h
                    where left(wholesalebtn,10) = left(w.btn,10) and
                          id = (select max(id) 
                                  from hostedmatchedmrc h1
                                  where h.wholesalebtn = h1.wholesalebtn)) m 
	  left join EntityAlternateIDs aid on m.CustomerID = aid.ExternalID and w.BillDate between aid.StartDate and aid.EndDate
      where w.matchedby is null and w.billdate = @billdate 
	  order by w.btn,w.usoc	  

--	select coalesce(m.customerid,'Missing') as CustomerID,w.masterbtn, left(w.btn,10), w.date, w.amount, w.service, w.usoc, 
--			w.billdate, getdate(),@operator
--	  from hostedimportoccwholesale w 
--	   left join hostedmatchedmrc m on m.wholesalebtn = left(w.btn,10)
--	   where w.matchedby is null and w.billdate = @billdate
--	  order by w.btn,w.usoc
--	select coalesce(e.entity,w.customer), w.masterbtn, left(w.btn,10), w.date, w.amount, w.service, w.usoc, 
--			w.billdate, getdate(),@operator
--	  from hostedimportoccwholesale w 
--		left join entity e on w.customer = e.alternateid 
--	  where w.matchedby is null and w.billdate = @billdate
--	  order by w.btn,w.usoc

--Insert Retail
	insert into hostedmatchedocc ([CustomerID],[AlternateID],[retailMasterBTN],[retailBTN],[retailDate],[retailAmount],
		[retailService],[retailUSOC],[retailBillDate],[LastModifiedDateTime],[LastModifiedBy])
	select coalesce(aid.entity,r.customer),
	               r.customer, r.masterbtn, r.btn, r.date, r.amount, r.service, r.usoc, r.billdate, 
	       getdate(),@operator
	  from hostedimportoccretail r 
	  left join EntityAlternateIDs aid on aid.ExternalID = r.Customer and r.BillDate between aid.StartDate and aid.EndDate
	  where r.billdate = @billdate
	  order by r.btn,r.usoc
	  
--Update Import Tables
	update hostedimportoccwholesale set Matchedby = 'Auto:OCC Post', matcheddatetime = getdate() 
		where matchedby is null	and billdate = @billdate 

	update hostedimportoccretail set Matchedby = 'Auto:OCC Post', matcheddatetime = getdate() 
		where matchedby is null	and billdate = @billdate 

--Update Wholesale CustomerID's where the BTN matches Wholesale MRC
update hostedmatchedocc
   set customerid = m.customerid
  from hostedmatchedocc o 
   inner join hostedmatchedmrc m on o.wholesalebtn = m.wholesalebtn
 where isnumeric(o.customerid) <> 1 and o.wholesalebilldate = @billdate

--Update Wholesale CustomerID's where the BTN matches Retail MRC
update hostedmatchedocc
   set customerid =  m.customerid
  from hostedmatchedocc o 
   inner join hostedmatchedmrc m on o.wholesalebtn = m.retailbtn
 where isnumeric(o.customerid) <> 1 and o.wholesalebilldate = @billdate
 
--Update CustomerID's where the name matches the entity legalname
update hostedmatchedocc
   set customerid = e.entity
  from hostedmatchedocc o 
   inner join entity e on o.customerid = e.legalname
 where isnumeric(o.customerid) <> 1 and o.wholesalebilldate = @billdate

--Update CustomerID's where the name matches an externalidmapping entry
update hostedmatchedocc
   set customerid = x.internalid
  from hostedmatchedocc o 
   inner join externalidmapping x on o.customerid = x.externalid
 where isnumeric(o.customerid) <> 1 and o.wholesalebilldate = @billdate

END

	  
	  




GO


