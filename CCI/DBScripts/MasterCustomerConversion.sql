USE CityHostedProd

/*
	Script to import Master Customer Spreadsheet to the CityHosted DB
*/
/****** Script for SelectTopNRows command from SSMS  ******
SELECT TOP 1000 [MasterCusID]
      ,[MasterCustomerName]
      ,[CustomerID]
      ,[CustomerName]
      ,[RITID]
      ,[Source]
      ,[SourceName]
      ,[BillStartDate]
      ,[BillEndDate]
  FROM [CityHostedTest].[import].[CustomerMasterList]
*/
/*
	These are one time things (adding the schema and backing up the old attributes for customers

Create Schema import
Create Schema sandbox
select * into import.attributecustomerbackup from attribute where itemtype = 'entity' and item = 'customer'

Do them once, but then don't redo them when we rerun the script
*/ 
/*
	Reset data to initial conditions so we can rerun
*/


DECLARE @MasterCustomerEntityTyoe as varchar(20) = 'MasterCustomer';
DECLARE @CCIENTITY as varchar(20) = 'CCI';
Declare @modifiedByUser as varchar(20) = 'LarryA-User'

DELETE from entity where entity like '9%' and EntityType = 'MasterCustomer';
DELETE FROM Attribute where Entity like '9%' and Item = 'MasterCustomer'

UPDATE ENtity
SET EntityOwner = @CCIENTITY
WHERE Entity like '2%' and EntityType = 'Customer';

UPDATE DataSources
Set FromClause = 'Select * from vw_DealerMargin'
Where DataSource = 'AnalysisDealerMargin'
UPDATE DataSources
Set FromClause = 'Select * from vw_AnalysisMargin'
Where DataSource = 'AnalysisMarginRaw'

DELETE FROM DataSources WHERE DataSource in ('MasterCustomerCustomerList','AgentCustomerList')

INSERT INTO DataSources(DataSource,Description,FromClause,OrderByClause,OverrideWhere,MaxCount,ParameterList,IncludeInAnalysis,LastModifiedBy,LastModifiedDateTime)
VALUES ('MasterCustomerCustomerList','List of Customers for Master Customers','select EntityOwner, Entity Customer, LegalName Name FROM Entity WHERE EntityType = ''Customer''',
null,0,-1,'',1,'Axon-User',getdate())

INSERT INTO DataSources(DataSource,Description,FromClause,OrderByClause,OverrideWhere,MaxCount,ParameterList,IncludeInAnalysis,LastModifiedBy,LastModifiedDateTime)
VALUES ('AgentCustomerList','List of Customers for Agents','select a.Value Agent, c.Entity Customer, c.LegalName Name FROM Entity c INNER JOIN vw_AttributeNonXML a on c.Entity = a.Entity and a.ItemTYpe = ''Entity'' and a.Item = ''Customer'' and a.name = ''Agent'' Where EntityType = ''Customer''',
null,0,-1,'',1,'Axon-User',getdate())

/*
Import master customer list first 
*/
DROP Table import.customermasterlist;

select * into import.CustomerMasterList from CityHostedTest.import.CustomerMasterList;


WITH MasterCustomers (Entity, EntityType, EntityOwner, LegalName, CustomerID)
AS
(
	Select 
		MasterCusID,
		@MasterCustomerEntityTyoe,
		@CCIENTITY,
		MasterCustomerName,
		Min(CustomerID) 
	FROM [import].[CustomerMasterList]
	GROUP BY MasterCusID, MasterCustomerName
)
  INSERT INTO Entity (Entity, EntityType, EntityOwner, LegalName, LastModifiedBy, LastModifiedDateTime, AlternateID, Address1, Address2, City, State, Zip)
  select m.Entity, m.EntityType, m. EntityOwner, m.LegalName, @modifiedByUser  LastModifiedBy, 
	getdate() LastModifiedDateTime,  c.AlternateID AlternateID, c.Address1, c.Address2,
	c.City, c.State, c.Zip  
   from MasterCustomers m
  INNER JOIN Entity c on Convert(varchar(10), m.CustomerID) = c.Entity

Update Entity
SET EntityOwner = Convert(varchar(20), m.MasterCusID)
FROM Entity e
INNER JOIN [import].[CustomerMasterList] m on e.Entity = m.CustomerID
WHERE e.EntityType = 'Customer'

INSERT INTO Attribute (Entity, ItemType, Item, ItemHistory, Attributes, LastModifiedBy, LastModifiedDateTime)
Select Entity, 'Entity', 'MasterCustomer', '<ItemDates />', null, 'Axon-User', getdate()
FROM Entity where entitytype = 'MasterCustomer'

/* Add Agents to Entity and Attribute from import sheet */

DELETE from Entity WHERE EntityType = 'Agent'
DELETE FROM Attribute where ItemType = 'Entity' and Item = 'Agent'

INSERT INTO Entity (Entity, EntityType, EntityOwner, LegalName, LastModifiedBy, LastModifiedDateTime)
select distinct 'AG-' + left(upper(Replace(cm.SourceName, ' ', '')), 22) Entity, 'Agent' EntityType, 'CCI' EntityOwner, cm.SourceName LegalName, 'Axon-User' LastModifiedBy,
 getdate() LastModifiedDateTime
from import.CustomerMasterList cm  
where source = 'agent'

INSERT INTO Attribute (Entity, ItemType, Item, ItemHistory, Attributes, LastModifiedBy, LastModifiedDateTime)
select distinct 'AG-' + left(upper(Replace(cm.SourceName, ' ', '')), 22) Entity, 'Entity' ItemType, 'Agent' Item, '<ItemDates />' ItemHistory, null Attributes, 'Axon-User' LastModifiedBy,
 getdate() LastModifiedDateTime
from import.CustomerMasterList cm  
where source = 'agent'

/* Add Agent to the customer fields */
Update attribute
set attributes = '<attributes>
  <attribute name="Include">
    <valuehistory include="Default.EntityType.Business" lastmodifiedby="Axon-User" lastmodifieddatetime="6-21-2012 7:58 AM" />
  </attribute>
  <attribute name="StatusEventMasterList">
    <valuehistory tableheader="(StatusFilter~StatusEvent~Description~Status~BenefitEligible~UpdateEntityStartDate~UpdateEntityEndDate|New~Started~Started~Active~True~StartDate~|Active~Termed-PEO~Terminated - went to PEO~Terminated~False~~StartDate|Active~Termed-InHouse~Termed-went inhouse~Terminated~False~~StartDate|Active~Termed-OutofBusiness~Termed-went out of business~Terminated~False~~StartDate)" lastmodifiedby="Authored" lastmodifieddatetime="2009-03-07T14:35:00" />
  </attribute>
  <attribute name="IBP" />
  <attribute name="CustomerType">
    <valuehistory value="Telecom" />
  </attribute>
  <attribute name="PaymentType" />
  <attribute name="CustomerTypeTable">
    <valuehistory tableheader="(Hosted~City Hosted|Telecom~Telecom Products|All~Both Hosted and Telecom products)" />
  </attribute>
  <attribute name="AlternateID">
    <valuehistory value="@@AlternateID" />
  </attribute>
  <attribute name="QBExported" />
	<attribute name="Agent" />
	<attribute name="Day2YN" />
</attributes>'
where entity = 'default' and itemtype = 'entity' and item = 'customer'


Delete from Attribute
where Entity = 'Dictionary' and ItemType = 'Attribute' and Item = 'Day2YN'

Insert into Attribute (Entity, ItemType, Item, ItemHistory, Attributes, LastModifiedBy, LastModifiedDateTime)
values ('Dictionary', 'Attribute', 'Day2YN', '<ItemDates />', 
'<attributes>
  <attribute name="datatype">
    <valuehistory value="string" />
  </attribute>
</attributes>', 'Axon-User', getdate())

/*   add agent id to existing customers  */
-- rollback
Update a 
SET Attributes = bck.Attributes
FROM Attribute a
INNER JOIN import.attributecustomerbackup bck on a.entity = bck.entity and a.ItemType = bck.ItemType and a.Item = bck.Item;
-- and now update with agent
WITH AgentSource (Entity, ItemType, Item, Attributes, AgentNode)
AS
(
SELECT a.Entity, a.ItemType, a.Item, a.Attributes, Convert(xml, '<attribute name="Agent"><valuehistory value="' + ag.Entity + '"/></attribute>' ) agentNode
  FROM attribute a
  left join [import].[CustomerMasterList] cm
    on a.entity = Convert(varchar(50), cm.customerid) and a.item = 'customer' and a.itemtype = 'entity'
  left join Entity ag on cm.SourceName = ag.LegalName and ag.entitytype = 'Agent'
  where cm.source = 'agent' and ag.legalname is not null
  ) --select 'insert ' + convert(varchar(256),AgentNode) + ' into (/attributes)[1]' from agentsource where entity = '23287'
  UPDATE a
  SET Attributes.modify('insert sql:column("AgentNode") into (/attributes)[1]')
   from Attribute a
   INNER JOIN AgentSource s ON a.Entity = s.Entity

/* and also add agent to salesordealercustomers */
/*
	Add the new SalesType column to SalesOrDealerCustomersz
*/
if not exists(Select * from sys.columns c Where c.name = 'SalesType')
	ALTER TABLE SalesOrDealerCustomers Add SalesType varchar(20) null
GO
Delete from SalesOrDealerCustomers Where SalesType = 'Agent' -- rollback... delete the old ones
UPDATE SalesOrDealerCustomers SET SalesType = 'Dealer'  -- all of the existing ones are Dealers

INSERT INTO SalesOrDealerCustomers
Select ag.Entity SalesOrDealer, cm.CustomerID Customer, 'Axon-User' LastModifiedBy, getdate() LastModifiedDateTime, 'Agent' SalesType 
FROM [import].[CustomerMasterList] cm
INNER JOIN Entity ag on cm.SourceName = ag.LegalName and ag.entitytype = 'Agent'

/*
	Now update queries and datasources
*/
UPDATE DataSources
Set FromClause = 'Select * from sandbox.vw_DealerMargin'
Where DataSource = 'AnalysisDealerMargin'
UPDATE DataSources
Set FromClause = 'Select * from sandbox.vw_AnalysisMargin'
Where DataSource = 'AnalysisMarginRaw'


/*
	Now create the new views
	Note these must be in proper sequence

	[sandbox].[vw_ActiveHostedDealerCosts]
	[sandbox].[vw_DealerLevel]
	[sandbox].[vw_CityHostedOCCSummary]
	[sandbox].[vw_CityHostedUSOCByCustomerSummary]
	[sandbox].[vw_CityHostedMarginOCCUSOCSummary2] 
	[sandbox].[vw_CityHostedMarginMRCUSOCSummary] 
	[sandbox].[vw_DealerMarginExtract]
	[sandbox].[vw_AnalysisMargin] 
	[sandbox].[vw_DealerMargin]
*/
DROP VIEW [sandbox].[vw_ActiveHostedDealerCosts]
GO

/****** Object:  View [sandbox].[vw_ActiveHostedDealerCosts]    Script Date: 2/27/2019 11:40:04 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [sandbox].[vw_ActiveHostedDealerCosts]
AS
SELECT *
from HostedDealerCosts
WHERE GETDATE() between ISNULL(StartDate, '1/1/1900') and ISNULL(enddate, '12/31/2100')

GO
/****** Object:  View [sandbox].[vw_DealerLevel]    Script Date: 2/27/2019 11:44:03 AM ******/
DROP VIEW [sandbox].[vw_DealerLevel]
GO

/****** Object:  View [sandbox].[vw_DealerLevel]    Script Date: 2/27/2019 11:44:03 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [sandbox].[vw_DealerLevel]
AS
select
	hdc.Dealer,
	hdc.ItemID Level
from sandbox.vw_ActiveHostedDealerCosts hdc
inner join CodeMaster cm on hdc.ItemID = cm.CodeValue and cm.CodeType ='DealerPricingLevels'

GO

/****** Object:  View [sandbox].[vw_CityHostedOCCSummary]    Script Date: 2/27/2019 11:41:16 AM ******/
DROP VIEW [sandbox].[vw_CityHostedOCCSummary]
GO

/****** Object:  View [sandbox].[vw_CityHostedOCCSummary]    Script Date: 2/27/2019 11:41:16 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [sandbox].[vw_CityHostedOCCSummary]
AS
WITH
	Retail
	(
		CustomerID,
		CustomerName,
		RetailUSOC,
		RetailAmount,
		RetailQty,
		ProRated,
		BillDate,
		USOCWholesale,
		WholesaleUSOC,
		WholesaleAmount,
		WholesaleQty,
		WholesaleProrated,
		WholesaleBillDate,
		ServiceDescription,
		Credit
	)
	AS
	(
		select
			retail.CustomerID,
			retail.CustomerName,
			retail.RetailUSOC,
			RetailAmount,
			RetailQty,
			retail.ProRated,
			retail.BillDate,
			u.USOCWholesale,
			wholesale.WholesaleUSOC,
			wholesale.WholesaleAmount,
			WholesaleQty,
			wholesale.ProRated WholesaleProrated, 
			wholesale.BillDate WholesaleBillDate,
			retail.ServiceDescription,
			Credit
		from
		(
			select
				h.CustomerID,
				e.LegalName CustomerName,
				RetailUSOC,
				SUM(RetailAmount) RetailAmount,
				Count(*) RetailQty, 
				dbo.IsProratedOCC(RetailService) ProRated,
				RetailBillDate BillDate,
				MAX(RetailService) ServiceDescription,
				IIF(RetailService like 'Credit%', 'Yes', 'No') Credit
			from hostedmatchedocc h
			inner join entity e on h.CustomerID = e.entity
			where  RetailBillDate is not null and h.CustomerID like '2%'
			group by
				h.CustomerID,
				e.LegalName,
				RetailUSOC,
				RetailBillDate, 
				RetailAmount, -- Added by LLA 7-30-2015 to allow for the same item with different prices to be different lines
				dbo.IsProratedOCC(RetailService),
				IIF(RetailService like 'Credit%', 'Yes', 'No')
		) retail
		left join vw_CityHostedUSOCs u on retail.RetailUSOC = u.USOCRetail
		left join
		(
			select
				h.CustomerID, 
				WholesaleUSOC,
				SUM(WholesaleAmount) WholesaleAmount,
				Count(*) WholesaleQty, 
				dbo.IsProratedOCC(WholesaleService) ProRated,
				WholesaleBillDate BillDate,
				MAX(WholesaleService) ServiceDescription 
			from hostedmatchedocc h
			where WholesaleBillDate is not null 
			group by h.CustomerID, WholesaleUSOC, dbo.IsProratedOCC(WholesaleService), WholesaleBillDate
		) wholesale on retail.CustomerID = wholesale.CustomerID
						and u.USOCWholesale = wholesale.WholesaleUSOC
						and retail.ProRated = wholesale.ProRated
						and retail.BillDate = wholesale.BillDate
	),
	OrphanedWholesale
	(
		CustomerID,
		CustomerName,
		RetailUSOC,
		RetailAmount,
		RetailQty,
		ProRated,
		RetailBillDate,
		USOCWholesale,
		WholesaleUSOC,
		WholesaleAmount,
		WholesaleQty,
		WholesaleProRated,
		BillDate,
		ServiceDescription,
		Credit
	)
	AS
	(
		select
			wholesale.CustomerID,
			wholesale.CustomerName,
			null RetailUSOC,
			null RetailAmount,
			null RetailQty,
			null ProRated,
			null RetailBillDate,
			WholesaleUSOC USOCWholesale, 
			WholesaleUSOC,
			WholesaleAmount,
			WholesaleQty,
			wholesale.ProRated WholesaleProRated,
			BillDate,
			ServiceDescription,
			Credit
		from
		(
			select
				h.CustomerID,
				e.LegalName CustomerName, 
				h.WholesaleUSOC,
				SUM(h.WholesaleAmount) WholesaleAmount,
				Count(*) WholesaleQty,
				dbo.IsProratedOCC(h.WholesaleService) ProRated, 
				WholesaleBillDate BillDate,
				Max(h.WholesaleService) ServiceDescription,
				IIF(WholesaleService like 'Credit%', 'Yes', 'No') Credit
			from hostedmatchedocc h
			left join Entity e on h.CustomerID = e.Entity and e.EntityType = 'Customer'
			where h.Customerid like '2%' 
			group by
				h.CustomerID,
				e.LegalName,
				WholesaleUSOC,
				dbo.IsProratedOCC(h.WholesaleService),
				WholesaleBillDate,
				IIF(WholesaleService like 'Credit%', 'Yes', 'No')
		) wholesale
		left join
		(
			select distinct
				h.CustomerID,
				e.LegalName CustomerName,
				RetailUSOC,
				u.USOCWholesale,
				dbo.IsProratedOCC(RetailService) ProRated 
			from hostedmatchedocc h
			inner join entity e on h.CustomerID = e.entity
			left join vw_CityHostedUSOCs u on h.RetailUSOC = u.USOCRetail
			where RetailBillDate is not null --and RetailBillDate = '5/1/2013'
		) retail on wholesale.CustomerID = retail.CustomerID
					and wholesale.WholesaleUSOC = retail.UsocWholesale
					and wholesale.ProRated = retail.ProRated
		where  retail.CustomerID is null and wholesale.BillDate is not null 
	)
select
	CustomerID,
	CustomerName,
	Coalesce(BillDate, WholesaleBillDate) BillDate,
	RetailUSOC,
	RetailAmount,
	RetailQty,
	WholesaleUSOC,
	WholesaleAmount,
	WholesaleQty,
	coalesce(ProRated, WholesaleProRated) ProRated,
	Credit,  
	case
		when RetailUSOC IS null then 'WholesaleOnly' 
		when WholesaleUSOC IS null then 'RetailOnly'
		when ISNULL(RetailQty, 0) <> ISNULL(WholesaleQty, 0) then 'QtyMismatch'
		else 'Matched'
	end Exception,
	ServiceDescription
from
(
	SELECT * FROM Retail
	UNION
	SELECT * FROM OrphanedWholesale
) a

GO


/****** Object:  View [sandbox].[vw_CityHostedUSOCByCustomerSummary]    Script Date: 2/27/2019 11:42:55 AM ******/
DROP VIEW [sandbox].[vw_CityHostedUSOCByCustomerSummary]
GO

/****** Object:  View [sandbox].[vw_CityHostedUSOCByCustomerSummary]    Script Date: 2/27/2019 11:42:55 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE view [sandbox].[vw_CityHostedUSOCByCustomerSummary] as
WITH
	RetailWithWholesale (CustomerID, RetailUSOC, WholesaleUSOC, Retail, Wholesale, BillDate, RetailQty, WholesaleQty)
	AS
	(
		select
			customerid CustomerID,
			RetailUSOC,
			WholesaleUSOC,
			sum(RetailAmount) Retail,
			SUM(WholesaleAmount) Wholesale,
			RetailBillDate BillDate,
			sum(RetailQty) RetailQty,
			SUM(WholesaleQty) WholesaleQty 
		from hostedmatchedmrc r 
		where retailbilldate is not null and WholesaleBillDate is not null
		group by customerid, RetailUSOC, WholesaleUSOC, RetailBillDate 
	),

	RetailWithoutWholesale (CustomerID, RetailUSOC, WholesaleUSOC, Retail, Wholesale, BillDate, RetailQty, WholesaleQty)
	AS
	(
		select
			retail.CustomerID,
			retail.RetailUSOC,
			retail.WholesaleUSOC,
			retail.Retail,
			isnull(Wholesale.Wholesale,0) Wholesale,
			retail.BillDate,
			retail.RetailQty,
			isnull(wholesale.WholesaleQTy, 0) WholesaleQty
		from
		(
			select
				customerid CustomerID,
				RetailUSOC,
				mr.USOCWholesale WholesaleUSOC,
				sum(RetailAmount) Retail,
				0 Wholesale,
				RetailBillDate BillDate,
				sum(RetailQty) RetailQty,
				0 WholesaleQty 
			from hostedmatchedmrc r 
			left join vw_cityhostedusocs mr on r.retailusoc = mr.usocretail 
			where retailbilldate is not null and WholesaleBillDate is  null
			group by CustomerID, retailusoc, mr.USOCWholesale, RetailBillDate
		) retail
		left join -- match to wholesale with no retail via usoc matching table
		( 
			select
				customerid CustomerID,
				wholesaleusoc,
				0 Retail,
				sum(WholesaleAmount) Wholesale,
				WholesaleBillDate BillDate,
				0 RetailQty,
				SUM (WholesaleQty) WholesaleQTy 
			from hostedmatchedmrc w 
			where WholesaleBillDate is not null and RetailBillDate is null
			group by customerid, wholesaleusoc, WholesaleBillDate
		) wholesale
			on retail.CustomerID = wholesale.CustomerID
			and retail.BillDate = wholesale.BillDate
			and retail.WholesaleUSOC = wholesale.WholesaleUSOC
	),

	WholesaleWithoutRetail (CustomerID, RetailUSOC, WholesaleUSOC, Retail, Wholesale, BillDate, RetailQty, WholesaleQty)
	AS
	(
		select
			b.CustomerID,
			u2.USOCRetail RetailUSOC,
			wholesaleusoc,
			0 Retail,
			SUM(WholesaleAmount) Wholesale,
			WholesaleBIllDate BillDate,
			0 RetailQty,
			SUM(WHolesaleQty) WholesaleQty
	    from hostedmatchedmrc b
		/* REVIEW: this and RetailWithoutWholesale should match to a shared CTE, but this seems to be broken --
					it does not use the same criteria as RetailWithoutWholesale */
		left join
		(
			-- all of the wholesaleusocs that CAN be matched from the retail without wholesale via usoc matching table
		 	select distinct m.customerid, u.usocwholesale
			from hostedmatchedmrc m 
			inner join vw_CityHostedUSOCs u on m.retailusoc = u.usocretail
			where m.RetailBillDate is not null and m.WholesaleBillDate is null
		) u on b.WholesaleUSOC = u.USOCWholesale and b.CustomerID = u.CustomerID
		left join
		(
			select USOCWholesale, MAX(USOCRetail) USOCRetail 
	        from vw_CityHostedUSOCs 
	        group by USOCWholesale
		) u2 on b.WholesaleUSOC = u2.USOCWholesale
		where
			u.USOCWholesale is  null -- so just pick up rows that do NOT have a wholesales usoc in the retail matching list
			and b.WholesaleBillDate is not null and b.RetailBillDate is null -- any only get wholesale-only rows
		group by b.CustomerID, u2.USOCRetail, WholesaleUSOC, WholesaleBillDate
	)

select
	CustomerID,
	WholesaleUSOC,
	RetailUSOC,
	isnull(Retail, 0) RetailAmount,
	isnull(Wholesale, 0) WholesaleAmount, 
	BillDate,
	isnull(WholesaleQty, 0) WholesaleQty,
	isnull(RetailQty, 0) RetailQty 
from
(
	select * from RetailWithWholesale
	union all
	select * from RetailWithoutWholesale
	union all
	select * from WholesaleWithoutRetail
) a
--Combined a
where Retail <> 0 or Wholesale <> 0

GO


/****** Object:  View [sandbox].[vw_CityHostedMarginOCCUSOCSummary2]    Script Date: 2/27/2019 11:53:09 AM ******/
DROP VIEW [sandbox].[vw_CityHostedMarginOCCUSOCSummary2]
GO

/****** Object:  View [sandbox].[vw_CityHostedMarginOCCUSOCSummary2]    Script Date: 2/27/2019 11:53:09 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [sandbox].[vw_CityHostedMarginOCCUSOCSummary2] 
as 
select
	dlr.SalesOrDealer Dealer,
	occ.CustomerID,
	c.LegalName CustomerName,
	RetailUSOC,
	WHolesaleUSOC,
	occ.BillDate BillDate,
	occ.BillDate RetailBillDate,
	occ.BillDate wholesalebilldate,
	'OCC' ChargeType,
	Sum(isnull(occ.WholesaleAmount,0)) Wholesale, 
	Sum(isnull(occ.RetailAmount,0)) Retail,
	Sum(isnull(occ.RetailAmount,0)) - Sum(isnull(occ.WholesaleAmount,0)) Net,
	0 DealerMargin,
	dbo.isproratedocc(ServiceDescription) ProRated, 
	Credit,
	IIF(ServiceDescription like '%Expedite%', 'Yes', 'No') HasExpedite,
	IIF(ISDATE(RIght(ServiceDescription, 8)) = 1, 'Yes', 'No') ServiceEndsInDate,
	max(isnull(retailqty,0)) OCCQty, 
	max(isnull(RetailQty,0)) RetailQty,
	max(WHolesaleQty) WholesaleQty,
	max(dbo.GetProratedDays(ServiceDescription)) ProratedDays,
	pr.NRC NRC,
	pw.NRC WholesaleNRC
from sandbox.vw_CityHostedOCCSummary occ
left join dbo.entity c on occ.customerid = c.entity
left join dbo.SalesOrDealerCustomers dlr on dlr.Customer = occ.CustomerID
left join ProductList pw on pw.ItemID = occ.WholesaleUSOC and pw.Carrier = 'saddleback'
left join ProductList pr on pr.ItemID = occ.RetailUSOC and pr.Carrier = 'cityhosted'
group by
	dlr.SalesOrDealer,
	occ.CustomerID,
	c.LegalName,
	RetailUSOC,
	WHolesaleUSOC,
	pr.NRC,
	pw.NRC,
	occ.BillDate,
	dbo.isproratedocc(ServiceDescription),
	Credit,
	IIF(ServiceDescription like '%Expedite%', 'Yes', 'No'),
	IIF(ISDATE(RIght(ServiceDescription, 8)) = 1, 'Yes', 'No')

GO

/****** Object:  View [sandbox].[vw_CityHostedMarginMRCUSOCSummary]    Script Date: 2/27/2019 11:54:04 AM ******/
DROP VIEW [sandbox].[vw_CityHostedMarginMRCUSOCSummary]
GO

/****** Object:  View [sandbox].[vw_CityHostedMarginMRCUSOCSummary]    Script Date: 2/27/2019 11:54:04 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE view [sandbox].[vw_CityHostedMarginMRCUSOCSummary] 
as
select
	mrc.BillDate WholesaleBillDate,
	mrc.BillDate RetailBillDate,
	CustomerID,
	c.Legalname CustomerName,
	'NWHS-' + Convert(nvarchar(12), Convert(int, c.alternateid)) Account,
	RetailUSOC,
	r.Name RetailUSOCName,
	WholesaleUSOC,
	w.Name WholesaleUSOCName, 
	dlr.SalesOrDealer Dealer,
	de.legalname DealerName,
	x.value DealerLevel,
	coalesce(dc.DealerCost, dc2.DealerCost, 0) DealerCost,
	sum(isnull(WholesaleQty,0)) WholesaleQTY, 
	sum(round(isnull(wholesaleamount,0),2)) WholesaleAmount, 
	sum(isnull(RetailQty,0)) RetailQty, 
	sum(round(isnull(retailamount,0),2)) RetailAmount, 
	sum(round(isnull(retailamount,0),2)) - sum(round(isnull(wholesaleamount,0),2)) GrossMargin,
	sum(isnull(WholesaleQTY, 0) * coalesce(dc.DealerCost, dc2.DealerCost, 0)) DealerCostTotal,
	sum(isnull(WholesaleQTY, 0) * coalesce(dc.DealerCost, dc2.DealerCost, 0)) - sum(round(isnull(wholesaleamount,0),2)) Net,
	sum(round(isnull(retailamount,0),2)) - sum(isnull(WholesaleQTY, 0) * coalesce(dc.DealerCost, dc2.DealerCost, 0)) DealerMargin
from sandbox.vw_CityHostedUSOCByCustomerSummary mrc
left join dbo.entity c on mrc.customerid = c.entity
left join dbo.masterproductlist w on mrc.wholesaleusoc = w.itemid
left join dbo.masterproductlist r on mrc.retailusoc = r.itemid
left join dbo.SalesOrDealerCustomers dlr on dlr.Customer = mrc.CustomerID
left join dbo.vw_attributenonxml x on x.entity = dlr.SalesOrDealer and x.itemtype = 'entity' and x.item = 'ibp' and x.Name = 'DealerLevel'
left join dbo.HostedDealerCosts dc on dc.Dealer = dlr.SalesOrDealer and dc.ItemID = WholesaleUSOC
left join dbo.HostedDealerCosts dc2 on dc2.Dealer = x.value and dc2.ItemID = WholesaleUSOC
left join entity de on de.entity = dlr.salesordealer
group by
	mrc.BillDate,
	mrc.BillDate,
	customerid,
	c.Legalname,
	RetailUsoc,
	r.Name,
	WholesaleUSOC,
	w.Name,
	dlr.SalesOrDealer,
	de.legalname,
	x.value,
	dc.DealerCost,
	dc2.DealerCost,
	c.alternateid

GO
/****** Object:  View [sandbox].[vw_DealerMarginExtract]    Script Date: 2/27/2019 11:56:50 AM ******/
DROP VIEW [sandbox].[vw_DealerMarginExtract]
GO

/****** Object:  View [sandbox].[vw_DealerMarginExtract]    Script Date: 2/27/2019 11:56:50 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [sandbox].[vw_DealerMarginExtract]
as
WITH
	CombinedSummary
	(
		BillDate,
		CustomerID,
		CustomerName,
		Accouunt,
		ChargeType,
		RetailUSOC,
		WholesaleUSOC,
		Retail,
		Wholesale,
		RetailQty,
		WholesaleQty,
		OCCQty,
		Prorated,
		ProratedDays,
		Credit,
		HasExpedite,
		ServiceEndsInDate,
		NRC,
		WholesaleNRC
	)
	AS
	(
		select
			Coalesce(retailbilldate, wholesalebilldate) BillDate,
			CustomerID,
			CustomerName,
			CONVERT(int,substring(account, 6,3)) Account,
			'MRC' ChargeType,
			RetailUSOC,
			WholesaleUSOC,
			RetailAmount Retail,
			WholesaleAmount Wholesale,
			RetailQty,
			WholesaleQty,
			0 OCCQty,
			null Prorated,
			null ProratedDays,
			null Credit,
			null HasExpedite,
			null ServiceEndsInDate,
			null NRC,
			null WholesaleNRC
		from sandbox.vw_CityHostedMarginMRCUSOCSummary
		union
		select
			coalesce(retailbilldate, wholesalebilldate) BillDate,
			CustomerID,
			CustomerName,
			0 account,
			'OCC' ChargeType,
			RetailUSOC,
			WholesaleUSOC,
			Retail,
			Wholesale,
			RetailQty,
			WholesaleQty,
			OCCQty Quantity,
			Prorated Prorated,
			ProratedDays,
			Credit,
			HasExpedite,
			ServiceEndsInDate,
			NRC,
			WholesaleNRC
		from sandbox.vw_CityHostedMarginOCCUSOCSummary2
	),
	USOC (USOCWholesale, USOCRetail)
	AS
	(
		Select usocwholesale, MAX(usocretail) USOCRetail 
		from vw_CityHostedUSOCs 
		where USOCWholesale is not null and WholesaleOnly = 'No' and RetailOnly = 'No' group by USOCWholesale
	)
select
	coalesce(Dlr.SalesOrDealer, 'CCIDealer') Dealer,
	e.alternateid SaddlebackID,
	CustomerID,
	CustomerName,
	BillDate,
	ChargeType,
	RetailUSOC,
	WholesaleUSOC, 
	round(Retail, 2) Retail,
	Round(Wholesale, 2) Wholesale,
	RetailQty,
	WholesaleQty, 
	coalesce(dc.DealerCost, dc2.DealerCost, 0) * IIF(Retail < 0 OR Wholesale < 0, -1, 1) DealerCostEach,
	a.WholesaleNRC InstallCharge,
	a.NRC RetailInstallCharge,
	Retail - Wholesale Net, 
	OCCQty,
	ProRated,
	ProratedDays,
	Credit,
	HasExpedite,
	ServiceEndsInDate,
	isnull(lvl.Level, 'None') Level
from CombinedSummary a
left join Entity e on a.customerid = e.entity
left join ProductList p on p.ItemID = a.RetailUSOC and p.Carrier = 'CityHosted'
left join dbo.SalesOrDealerCustomers dlr on dlr.Customer = a.CustomerID
left join sandbox.vw_DealerLevel lvl on lvl.Dealer = dlr.SalesOrDealer
left join USOC on a.WholesaleUSOC = usoc.USOCWholesale
left join sandbox.vw_ActiveHostedDealerCosts dc on dc.Dealer = dlr.SalesOrDealer and dc.ItemID = coalesce(a.RetailUSOC, usoc.USOCRetail)
left join sandbox.vw_ActiveHostedDealerCosts dc2 on dc2.Dealer = lvl.Level and dc2.ItemID = coalesce(a.RetailUSOC, usoc.USOCRetail)
where CustomerName is not null 


GO



/****** Object:  View [sandbox].[vw_AnalysisMargin]    Script Date: 2/27/2019 11:55:10 AM ******/
DROP VIEW [sandbox].[vw_AnalysisMargin]
GO

/****** Object:  View [sandbox].[vw_AnalysisMargin]    Script Date: 2/27/2019 11:55:10 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [sandbox].[vw_AnalysisMargin] AS
SELECT
	dme.Dealer,
	dlr.LegalName DealerName,
	dme.SaddlebackID,
	dme.CustomerID,
	dme.CustomerName,
	dme.BillDate,
	dme.ChargeType,
	dme.RetailUSOC,
	mr.Name RetailUSOCDescription,
	mr.ItemSubCategory CHSCategory,
	case when isnull(pr.unmatched,0) = 1 and dme.retailusoc is not null
		then 'Yes'
		else 'No'
	end RetailOnly,
	case when getdate() between isnull(pr.startdate, '1/1/1900') and isnull(pr.enddate, '12/31/2100') and dme.retailusoc is not null
		then 'Yes'
		else 'No'
	end RetailActive,
	dme.WholesaleUSOC,
	mw.Name WholesaleUSOCDescription,
	case when isnull(pw.unmatched,0) = 1 and dme.wholesaleusoc is not null
		then 'Yes'
		else 'No'
	end WholesaleOnly,
	case when getdate() between isnull(pw.startdate, '1/1/1900') and isnull(pw.enddate, '12/31/2100') and dme.wholesaleusoc is not null
		then 'Yes'
		else 'No'
	end WholesaleActive,
	Convert(decimal(12, 2), dme.Retail) Retail,
	Convert(decimal(12, 2), dme.Wholesale) Wholesale,
	Convert(decimal(12, 0), dme.RetailQty) RetailQty,
	Convert(decimal(12, 0), dme.WholesaleQty) WholesaleQty,
	each.WholesaleEach,
	each.RetailEach,
	Convert(decimal(12, 2), dme.Net) GrossMargin,
	Convert(decimal(12, 2), dme.OCCQty) OCCQty,
	dme.ProRated,
	dme.ProratedDays,
	dme.Credit,
	case
		when dme.chargetype = 'MRC' then ''
		when dme.ProRated = 'Yes' then 'ProRated'
		when dme.HasExpedite = 'Yes' then 'Expedited'
		when dme.ServiceEndsInDate = 'Yes' and dme.InstallCharge = each.WholesaleEach then 'Install' 
		else 'Other'
	end OCCChargeType,
	dme.InstallCharge InstallChargeEach,
	CONVERT(decimal(12, 2), dme.DealerCostEach) DealerCostEach,
	CONVERT
	(
		decimal(12, 2),
		(
			case 
				when dme.OCCQty = 0 then isnull(dme.RetailQty,0)
				when dme.Prorated = 'Yes' then Convert(decimal(10,4), Convert(decimal(10,4), dme.ProratedDays) / 30. * Convert(decimal(10,4), dme.OCCqty)) 
				else dme.OCCQty 
			end
			* dme.DealerCostEach  
			* IIF(dme.Retail < 0 OR dme.Wholesale < 0, -1, 1) -- adjust for credits
		)
	) DealerCostFromLookup,
	dme.Level,
	ISNULL(da.Value, 'None') MasterDealer,
	Coalesce(mdc.DealerCost, mdc2.DealerCost, 0) * IIF(dme.Retail < 0 OR dme.Wholesale < 0, -1, 1) MasterDealerCostEach,
	cust.EntityOwner MasterCustomer,
	coalesce(mcust.LegalName, 'CCI') MasterCustomerName,
	agt.SalesOrDealer AgentID,
	agte.LegalName AgentName,
	d2.Value Day2YN
FROM sandbox.vw_DealerMarginExtract dme
left join MasterProductList mr on dme.RetailUSOC = mr.ItemID
left join ProductList pr on dme.RetailUSOC = pr.ItemID and pr.Carrier = 'CityHosted'
left join MasterProductList mw on WholesaleUSOC = mw.ItemID
left join ProductList pw on dme.WholesaleUSOC = pw.ItemID and pw.Carrier = 'Saddleback'
left join Entity dlr on dlr.Entity = dme.Dealer
left join vw_AttributeNonXML da on da.Entity = dme.Dealer and da.ItemType = 'Entity' and da.Item = 'Dealer' and da.Name = 'MasterDealer'
left join vw_AttributeNonXML d2 on d2.Entity = dme.CustomerID and d2.ItemType = 'Entity' and d2.Item = 'Customer' and d2.Name = 'Day2YN'
left join sandbox.vw_DealerLevel lvl on lvl.Dealer = da.Value
left join sandbox.vw_ActiveHostedDealerCosts mdc on mdc.Dealer = da.Value and mdc.ItemID = dme.RetailUSOC
left join sandbox.vw_ActiveHostedDealerCosts mdc2 on mdc2.Dealer = lvl.Level and mdc2.ItemID = dme.RetailUSOC
left join Entity cust on dme.CustomerID = cust.Entity and cust.EntityType = 'Customer' 
left join Entity mcust on cust.EntityOwner = mcust.Entity and mcust.EntityType = 'MasterCustomer'
left join SalesOrDealerCustomers agt on agt.Customer = cust.Entity and agt.SalesType = 'Agent'
left join Entity agte on agt.SalesOrDealer = agte.Entity and agte.EntityType = 'Agent'
outer apply
	(
		select
			convert
			(
				decimal(12, 2),
				case
					when dme.WholesaleQty = 0 then 0
					else dme.Wholesale / dme.WholesaleQty
				end
			) WholesaleEach,
			convert
			(
				decimal(12, 2),
				case
					when dme.RetailQty = 0 then 0
					else dme.Retail / dme.RetailQty
				end
			) RetailEach
	) each
where
	dme.customername is not null
	and dme.customerid <> '20035' 
	and 'MANOCC' not in (isnull(dme.RetailUSOC, ''), isnull(dme.WholesaleUSOC, '')) 
	and not (ISNULL(dme.retailusoc, '') = '' and ISNULL(dme.wholesaleusoc, '') = '')

GO
/****** Object:  View [sandbox].[vw_DealerMargin]    Script Date: 2/27/2019 11:58:11 AM ******/
DROP VIEW [sandbox].[vw_DealerMargin]
GO

/****** Object:  View [sandbox].[vw_DealerMargin]    Script Date: 2/27/2019 11:58:11 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--select top 200 * from sandbox.vw_dealermargin where billdate = '1/1/2018'

CREATE VIEW [sandbox].[vw_DealerMargin]
AS
SELECT
	am.Dealer,
	am.DealerName,
	am.SaddlebackID,
	am.CustomerID,
	am.CustomerName,
	am.BillDate,
	am.ChargeType,
	am.RetailUSOC,
	am.RetailUSOCDescription,
	am.CHSCategory,
	am.RetailOnly,
	am.RetailActive,
	am.WholesaleUSOC,
	am.WholesaleUSOCDescription,
	am.WholesaleOnly,
	am.WholesaleActive,
	am.Retail,
	am.Wholesale,
	am.RetailQty,
	am.WholesaleQty,
	am.WholesaleEach,
	am.RetailEach,
	am.GrossMargin,
	am.OCCQty,
	am.ProRated,
	am.ProratedDays,
	am.Credit,
	am.OCCChargeType,
	am.InstallChargeEach,
	am.DealerCostEach,
	am.DealerCostFromLookup,
	DealerCostApplied.Value DealerCostApplied,
	DealerMargin.Value DealerMargin,
	/* REVIEW: If the original comments are correct, then the NetMargin calculation
				is broken because it does not take into account RetailUSOC = 'NMF'.
				Recommend replacing current calculation with this new one that
				eliminates the duplication that seems to have led to the error. */
	--am.GrossMargin - DealerMargin.Value NetMargin,
	am.GrossMargin - Convert
		(
			decimal(12,2), -- gross margin - dealer margin = net margin
			case 
				when am.WholesaleOnly = 'Yes' then -am.wholesale
				when am.OCCChargeType = 'Install' then -am.Wholesale
				else am.Retail - Convert(decimal(12,2), am.DealerCostFromLookup) * IIF(am.Retail < 0, -1, 1) -- less dealer margin applied
				/* REVIEW: removed unreachable code */
				--else am.Retail
				--	- -- less dealer margin applied
				--	case 
				--		when am.WholesaleOnly = 'Yes' then am.Wholesale -- wholesaleonly
				--		when am.OCCChargeType = 'Install' then am.Wholesale
				--		else Convert(decimal(12,2), am.DealerCostFromLookup) 
				--	end
				--	* sandbox.udf_SignNZ(am.Retail)
			end
		) NetMargin,
	am.MasterDealer, 
	am.MasterDealerCostEach,
    CASE 
		WHEN am.MasterDealer = 'None' THEN 0 
		WHEN am.RetailUSOC = 'NMF' then am.Retail 
		ELSE MasterDealerCost.Value * IIF(am.MasterDealerCostEach < 0, -1, 1)
	END AS MasterDealerCost, 
    CASE 
		WHEN am.MasterDealer = 'None' THEN 0 
		WHEN am.RetailUSOC = 'NMF' then 0 
		ELSE DealerCostApplied.Value - MasterDealerCost.Value * IIF(am.MasterDealerCostEach < 0, -1, 1)
	END AS MasterDealerMargin,
	am.MasterCustomer,
	am.MasterCustomerName,
	am.AgentID,
	am.AgentName,
	am.Day2YN
from vw_ARSummary ars
left join sandbox.vw_AnalysisMargin am on am.CustomerID = ars.Customerid 
OUTER APPLY
	(SELECT Convert
			(
				decimal(12,2), 
				case 
					when am.WholesaleOnly = 'Yes' then am.Wholesale -- wholesaleonly
					when am.OCCChargeType = 'Install' then am.Wholesale
					else
						case 
							when am.OCCQty = 0 then isnull(am.RetailQty, 0)
							when am.Prorated = 'Yes' then Convert(decimal(10,4), Convert(decimal(10,4), am.ProratedDays) / 30. * Convert(decimal(10,4), am.OCCqty)) 
							else am.OCCQty 
						end
						* am.MasterDealerCostEach  
						* IIF(am.Retail < 0 OR am.Wholesale < 0, -1, 1)
				end
			) Value
	) MasterDealerCost
OUTER APPLY
	(SELECT Convert
		(
			decimal(12,2), 
			case 
				WHEN am.RetailUSOC = 'NMF' then am.Retail 
				when am.WholesaleOnly = 'Yes' then am.Wholesale -- wholesaleonly
				when am.OCCChargeType = 'Install' then am.Wholesale
				else Convert(decimal(12,2), am.DealerCostFromLookup) 
			end
				* IIF(am.Retail < 0, -1, 1)
		) Value
	) DealerCostApplied
OUTER APPLY
	(SELECT Convert
		(
			decimal(12,2), 
			case 
				when am.WholesaleOnly = 'Yes' then -am.wholesale
				when am.OCCChargeType = 'Install' then -am.Wholesale
				when am.RetailUSOC = 'NMF' then 0 
				else am.Retail - Convert(decimal(12,2), am.DealerCostFromLookup) * IIF(am.Retail < 0, -1, 1) -- less dealer margin applied
				/* REVIEW: Removed unreachable code */
				--else am.Retail
				--	- -- less dealer margin applied
				--	case 
				--		when am.WholesaleOnly = 'Yes' then am.Wholesale -- wholesaleonly
				--		when am.OCCChargeType = 'Install' then am.Wholesale
				--		else Convert(decimal(12,2), am.DealerCostFromLookup) 
				--	end
				--	* sandbox.udf_SignNZ(am.Retail)
			end
		) Value
	) DealerMargin
WHERE
	-- filter from original vw_DealerMargin
	(
		(
			am.RetailUSOC <> 'wsafe'
			AND am.RetailUSOC <> 'winst'
			--AND am.RetailUSOC <> 'nmf'
		)
		OR
			am.RetailUSOC IS NULL
	)
	AND
	-- filter from vw_DealerMarginBase
	(
		not
		(
			am.RetailOnly = 'No'
			and isnull(am.RetailQty,0) = 0
			and IIF(am.WholesaleOnly = 'Yes', -am.Wholesale, am.Retail - am.DealerCostFromLookup) = 0
			and am.ChargeType = 'MRC'
		)
		and not
		(
			am.ChargeType = 'OCC'
			and am.RetailUSOC is not null
			and am.Prorated = 'No'
			and am.RetailOnly = 'No'
			and am.OCCChargeType = 'Install'
		) --Non-Prorated Retail NRC -USOC NOT RETAIL ONLY 
	)


GO


