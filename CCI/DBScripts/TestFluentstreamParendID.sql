	Declare @BillDate as date = '5/1/2021'

select * from  vw_FluentStreamParentAccounts 

	
			--insert into [dbo].[HostedImportMRCRetail] (customernumber,CustomerName, MasterBTN, BTN, USOC, [Product Description], Qty, Price, Amount, ConnectionDate, BillDate)
			 select * FROM (
				Select 
					Coalesce(fs.ParentID, [dbo].[fnRemoveLeadingZeros](right([Cust Acct Nr],8))) as customernumber,
					[Service Name] as CustomerName,
					replace(replace(replace(replace([Billing Acct Nr],' ',''),'(',''),')',''),'-','') as MasterBTN,
					replace(replace(replace(replace([Working Acct Nr],' ',''),'(',''),')',''),'-','') as BTN,
					USOC,
					[Title] as [Product Description], 
					Qty, 
					Price,
					[Amt Billed] as Amount,
					Convert(datetime, substring([Conn Date], 5,2) + '/' + substring([Conn Date], 7,2) + '/' + substring([Conn Date], 1, 4)) as ConnectionDate,
					@BillDate as BillDate
					--Convert(datetime, substring([Bill Date], 5,2) + '/' + substring([Bill Date], 7,2) + '/' + substring([Bill Date], 1, 4)) as BillDate
			   from [dbo].[HostedTempMRCRetail]) mrc
			   LEFT JOIN vw_FluentStreamParentAccounts fs on [dbo].[fnRemoveLeadingZeros](right(mrc.[Cust Acct Nr],8)) = fs.AccountNumber