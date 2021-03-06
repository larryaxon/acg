USE [CityHosted]
GO
/****** Object:  StoredProcedure [dbo].[ProcessMRCDetails]    Script Date: 1/7/2017 7:38:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Andy Werdeman>
-- Create date: <11/28/2012>
-- Description:	<Process MRC Details>
-- Modified 2/7/2015 by Larry to use faster version of vw_CityHostedUsocs (vw_CHUsocs)
-- =============================================
-- exec ProcessMRCDetails 'LarryA', '1/1/2017'
-- 
ALTER PROCEDURE [dbo].[ProcessMRCDetails] (@operator nvarchar(25), @billdate date)
AS
BEGIN
	SET NOCOUNT ON;
--	Declare @previousmonth date
--	set @previousmonth = Dateadd(m,-1,@billdate)
	RAISERROR ('begin ProcessMRCDetails', 0, 1) WITH NOWAIT
	RAISERROR ('auto match city tolls', 0, 1) WITH NOWAIT
    update hostedimportmrcwholesale set matchedby = 'Auto:City', matcheddatetime = getdate()
     where customernumber = '80008154' and billdate = @billdate
	
	RAISERROR ('post from wholesale hostednomatchbtns', 0, 1) WITH NOWAIT
	update hostedimportmrcwholesale 
	set matchedby = 'Auto:Ex-BTN-USOC', matcheddatetime = getdate() 
     where matchedby is null and billdate = @billdate and (btn in (select btn from hostednomatchbtns) or
		   usoc in (select usoc from hostednomatchusocs where type = 'Wholesale'))
	RAISERROR ('post from no match usocs', 0, 1) WITH NOWAIT

	update hostedimportmrcwholesale set matchedby = 'Auto:Ex-BTN-USOC', matcheddatetime = getdate() 
	 from hostedimportmrcwholesale w WITH (NOLOCK)
     inner join hostednomatchbtnusoc nbu WITH (NOLOCK) on w.btn = nbu.btn and w.usoc = nbu.usoc
     where w.matchedby is null and w.billdate = @billdate

	RAISERROR ('post from retail nomatchbtns', 0, 1) WITH NOWAIT
	update hostedimportmrcretail
	set matchedby = 'Auto:Ex-BTN-USOC', matcheddatetime = getdate() 
     where matchedby is null and billdate = @billdate and (btn in (select btn from hostednomatchbtns) or
		   usoc in (select usoc from hostednomatchusocs where type = 'Retail'))

	RAISERROR ('update retail import', 0, 1) WITH NOWAIT
	update hostedimportmrcretail set matchedby = 'Auto:Ex-BTN-USOC ', matcheddatetime = getdate() 
	 from hostedimportmrcwholesale r WITH (NOLOCK)
     inner join hostednomatchbtnusoc nbu WITH (NOLOCK) on r.btn = nbu.btn and r.usoc = nbu.usoc
     where r.matchedby is null and r.billdate = @billdate
     
     
	RAISERROR ('Post Excluded to Matched table', 0, 1) WITH NOWAIT
	RAISERROR ('Wholesale', 0, 1) WITH NOWAIT
	insert into hostedMatchedMRC (wholesalebilldate, customerid, masterwholesalebtn, originalwholesalebtn,
				wholesalebtn, wholesaleusoc, wholesaleqty, wholesaleamount, 
				matchedby, lastmodifiedby, lastmodifieddatetime)
	(select w.billdate as wholesalebilldate, coalesce(x.internalid, w.customername) as customerid, 
			w.masterbtn as masterwholesalebtn, w.btn as originalwholesalebtn, w.btn as wholesalebtn, 
			w.usoc wholesaleusoc, Sum(w.Qty), Sum(w.Amount), 'Auto:Wholesale Except', @Operator, getdate()
		from hostedimportmrcwholesale w WITH (NOLOCK)
		  --ABW Determine how to change this
		  left join externalidmapping x WITH (NOLOCK) on w.customername = x.externalid and x.entitytype = 'Customer'
		where w.matchedby like 'Auto:Ex%' and w.billdate = @billdate
	    group by w.billdate, coalesce(x.internalid, w.customername), w.masterbtn, w.btn, w.btn, w.usoc)

	RAISERROR ('Retail', 0, 1) WITH NOWAIT
	insert into hostedMatchedMRC (retailbilldate, customerid, masterretailbtn, 
				retailbtn, retailusoc, alternateid, retailqty, retailamount, 
				matchedby, lastmodifiedby, lastmodifieddatetime)
	(select r.billdate as retailbilldate,
	        coalesce((select e.entity from entity e where r.customernumber = e.alternateid),r.customername) as customerid,
--	        coalesce((select min(x.internalid) from externalidmapping x where r.customername = x.externalid and x.entitytype = 'Customer'), r.customername) as customerid, 
		    r.masterbtn as masterretailbtn, r.btn as retailbtn, 
			r.usoc wholesaleusoc, r.customernumber, Sum(r.Qty), Sum(r.Amount), 'Auto:Retail Except', @Operator, getdate()
		from hostedimportmrcretail r WITH (NOLOCK)
		where r.matchedby like 'Auto:Ex%' and r.billdate = @billdate 
	    group by r.billdate, r.customername,r.masterbtn, r.btn, r.usoc,r.customernumber)
				
	RAISERROR ('Post non-excluded Wholesale', 0, 1) WITH NOWAIT
	insert into hostedMatchedMRC (wholesalebilldate, customerid, masterwholesalebtn, originalwholesalebtn,
				wholesalebtn, wholesaleusoc, wholesaleqty, wholesaleamount, 
				lastmodifiedby, lastmodifieddatetime)
	(select w.billdate as wholesalebilldate, coalesce(x.internalid, w.customername) as customerid, 
			w.masterbtn as masterwholesalebtn, w.btn as originalwholesalebtn, coalesce(s.newbtn,w.btn) as wholesalebtn, 
			w.usoc wholesaleusoc, Sum(w.Qty), Sum(w.Amount), @Operator, getdate()
		from hostedimportmrcwholesale w WITH (NOLOCK)
		  -- ABW Determine how to change this
		  left join externalidmapping x WITH (NOLOCK) on w.customername = x.externalid and x.entitytype = 'Customer'
		  left join hostedmrcwholesalebtnmatching s WITH (NOLOCK) on w.btn = s.originalbtn
		where w.matchedby is null and w.billdate = @billdate
	    group by w.billdate, coalesce(x.internalid, w.customername), w.masterbtn, w.btn, coalesce(s.newbtn,w.btn), w.usoc)

	RAISERROR ('Set Matched and Matchedby flags in Import tables', 0, 1) WITH NOWAIT
	update hostedimportmrcwholesale set matchedby = 'Auto:Wholesale Post', matcheddatetime = getdate() 
		where matchedby is null	and billdate = @billdate	

	RAISERROR ('Post Retail against Unmatched Wholesale using matching BTN and USOC', 0, 1) WITH NOWAIT
	RAISERROR ('PREVIOUS MONTH MATCHING', 0, 1) WITH NOWAIT
	RAISERROR ('Populate temp table', 0, 1) WITH NOWAIT

	SELECT m.ID, r.* INTO #tmpPreviousMonth1
      from hostedmatchedmrc m WITH (NOLOCK) 
	  inner join 
	(select billdate as retailbilldate,masterbtn as masterretailbtn,mr.btn as retailbtn,mr.usoc retailusoc, 
		    customernumber, sum(Qty) QSum, Sum(Amount) RetailAmt
	  from hostedimportmrcretail mr WITH (NOLOCK)
	  where mr.matchedby is null and mr.billdate = @billdate
		  group by mr.billdate, mr.customername, mr.masterbtn, mr.btn, mr.usoc,mr.customernumber) r
	on m.wholesalebtn = r.retailbtn 
	left join hostedmrcwholesalebtnmatching s WITH (NOLOCK) on m.wholesalebtn = s.originalbtn
	inner join vw_CHUsocs c WITH (NOLOCK) on r.retailusoc = c.usocretail and m.wholesaleusoc = c.usocwholesale
	where m.wholesaleqty = r.qsum and m.matchedby is null 
	  and m.wholesalebilldate = @billdate

	RAISERROR ('Update from temp table', 0, 1) WITH NOWAIT
	update hostedmatchedmrc 
	set RetailBillDate = r.retailbilldate,MasterRetailBTN = r.masterretailbtn,RetailBTN = r.retailbtn,
	RetailUSOC = r.retailusoc,RetailQty = r.QSum,RetailAmount = r.RetailAmt, alternateid = r.customernumber,
	MatchedBy = 'Auto:BTN/USOC Match',LastModifiedDateTime = getdate(),LastModifiedBy = @Operator
      from hostedmatchedmrc m WITH (NOLOCK) 
	  inner join #tmpPreviousMonth1 r on m.id = r.id

	RAISERROR ('Set Matched and Matchedby flags in Retail Import table', 0, 1) WITH NOWAIT
	update hostedimportmrcretail set matchedby = 'Auto:BTN/USOC Match', matcheddatetime = getdate() 
	 where matchedby is null and billdate = @billdate and id in
	(select distinct t.id 
	   from hostedimportmrcretail t WITH (NOLOCK)
	     inner join hostedmatchedmrc h WITH (NOLOCK)
		   on t.billdate = @billdate and h.retailbilldate = @billdate and t.btn = h.retailbtn and 
		      t.usoc = h.retailusoc)

	RAISERROR ('Post Retail against Unmatched Wholesale using hostedbtnmatching', 0, 1) WITH NOWAIT
	RAISERROR ('PREVIOUS MONTH MATCHING', 0, 1) WITH NOWAIT
	RAISERROR ('Populate temp table', 0, 1) WITH NOWAIT
	select m.id, r.* into #tmpPreviousMonth
	from hostedmatchedmrc m WITH (NOLOCK) inner join 
	(select billdate as retailbilldate,masterbtn as masterretailbtn,mr.btn as originalbtn,bm.wholesalebtn as retailbtn,mr.usoc retailusoc, 
			customernumber,sum(Qty) QSum, Sum(Amount) RetailAmt 
	  from hostedimportmrcretail mr WITH (NOLOCK) 
	  inner join hostedbtnmatching bm WITH (NOLOCK) on mr.btn = bm.retailbtn
	  where mr.matchedby is null and mr.billdate = @billdate
		  group by mr.billdate, mr.customername, mr.masterbtn, mr.btn,bm.wholesalebtn, mr.usoc,mr.customernumber) r
	on m.wholesalebtn = r.retailbtn 
	inner join vw_CHUsocs c WITH (NOLOCK) on r.retailusoc = c.usocretail and m.wholesaleusoc = c.usocwholesale
	where m.wholesaleqty = r.qsum and m.matchedby is null and 
	      m.wholesalebilldate = @billdate

	RAISERROR ('Update from temp table', 0, 1) WITH NOWAIT
	update hostedmatchedmrc set RetailBillDate = r.retailbilldate,MasterRetailBTN = r.masterretailbtn,
	    Originalretailbtn = r.originalbtn, RetailBTN = r.retailbtn,
		RetailUSOC = r.retailusoc,alternateid = r.customernumber, RetailQty = r.QSum,RetailAmount = r.RetailAmt,
		MatchedBy = 'Auto:BTN Matching',LastModifiedDateTime = getdate(),LastModifiedBy = @operator
	from hostedmatchedmrc m WITH (NOLOCK) 
	INNER JOIN #tmpPreviousMonth r on m.id = r.id
	
	--inner join 
	--(select billdate as retailbilldate,masterbtn as masterretailbtn,mr.btn as originalbtn,bm.wholesalebtn as retailbtn,mr.usoc retailusoc, 
	--		customernumber,sum(Qty) QSum, Sum(Amount) RetailAmt 
	--  from hostedimportmrcretail mr WITH (NOLOCK) 
	--  inner join hostedbtnmatching bm WITH (NOLOCK) on mr.btn = bm.retailbtn
	--  where mr.matchedby is null and mr.billdate = @billdate
	--	  group by mr.billdate, mr.customername, mr.masterbtn, mr.btn,bm.wholesalebtn, mr.usoc,mr.customernumber) r
	--on m.wholesalebtn = r.retailbtn 
	--inner join vw_CHUsocs c WITH (NOLOCK) on r.retailusoc = c.usocretail and m.wholesaleusoc = c.usocwholesale
	--where m.wholesaleqty = r.qsum and m.matchedby is null and 
	--      m.wholesalebilldate = @billdate

	RAISERROR ('Set Matched and Matchedby flags in Retail Import table', 0, 1) WITH NOWAIT
	update hostedimportmrcretail set matchedby = 'Auto:BTN Matching', matcheddatetime = getdate() 
	 where matchedby is null and billdate = @billdate and id in
	(select distinct t.id from hostedimportmrcretail t WITH (NOLOCK)
	                       inner join hostedmatchedmrc h WITH (NOLOCK)
								on t.billdate = @billdate and h.retailbilldate = @billdate 
								    and t.usoc = h.retailusoc and t.btn = h.originalretailbtn
	                       inner join hostedbtnmatching m WITH (NOLOCK)
	                            on t.btn = m.retailbtn and h.wholesalebtn = m.wholesalebtn)
	
	RAISERROR ('Post Remaining Retail records as unmatched', 0, 1) WITH NOWAIT
	insert into hostedMatchedMRC (retailbilldate, customerid, masterretailbtn, 
				retailbtn, retailusoc,alternateid, retailqty, retailamount, 
				lastmodifiedby, lastmodifieddatetime)
	(select r.billdate as retailbilldate, coalesce(e.entity, r.customername) as customerid, 
			r.masterbtn as masterretailbtn, r.btn as retailbtn,  
			r.usoc retailusoc, r.customernumber,Sum(r.Qty), Sum(r.Amount), @Operator, getdate()
		from hostedimportmrcretail r WITH (NOLOCK)
          left join entity e WITH (NOLOCK) on r.customernumber = e.alternateid 
		where r.matchedby is null and r.billdate = @billdate
	    group by r.billdate, coalesce(e.entity, r.customername), r.masterbtn, r.btn, r.usoc,r.customernumber)
	    
	RAISERROR ('Update Retail Import Table', 0, 1) WITH NOWAIT
	update hostedimportmrcretail set matchedby = 'Auto:Retail Post', matcheddatetime = getdate() 
	 where matchedby is null and billdate = @billdate

	RAISERROR ('Update CustomerIDs where alternate ID is valid in the entity table and wrong because of externalidmapping', 0, 1) WITH NOWAIT
update hostedmatchedmrc
   set customerid = e.entity
  from hostedmatchedmrc mrc WITH (NOLOCK)
  inner join entity e on mrc.alternateid = e.alternateid
  where mrc.customerid <> e.entity
  
	RAISERROR ('Update CustomerIDs where we have a valid saddleback ID', 0, 1) WITH NOWAIT
update hostedmatchedmrc
   set customerid = e.entity
  from hostedmatchedmrc m  WITH (NOLOCK)
   inner join entity e WITH (NOLOCK) on m.alternateid = e.alternateid
 where m.alternateid is not null and isnumeric(m.customerid) <> 1

	RAISERROR ('Update CustomerIDs where the name matches the entity legalname', 0, 1) WITH NOWAIT
update hostedmatchedmrc
   set customerid = e.entity
  from hostedmatchedmrc m WITH (NOLOCK)
   inner join entity e WITH (NOLOCK) on m.customerid = e.legalname
 where isnumeric(m.customerid) <> 1

	RAISERROR ('Update CustomerIDs where the name matches an externalidmapping entry', 0, 1) WITH NOWAIT
update hostedmatchedmrc
   set customerid = x.internalid
  from hostedmatchedmrc m WITH (NOLOCK)
   inner join externalidmapping x WITH (NOLOCK) on m.customerid = x.externalid
 where isnumeric(m.customerid) <> 1

	RAISERROR ('Post MRC Details Complete', 0, 1) WITH NOWAIT

END

