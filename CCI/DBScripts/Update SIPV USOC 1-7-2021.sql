/*
Script to update all the WSLVs in networkinventory to SIPV
*/

UPDATE networkinventory 
SET ProductNickName = 'Update to SIPV'
where itemid = 'WSLV'
and isnull(EndDate, '12/31/2021') > '1/1/2021'

Insert into NetworkInventory ([Customer]
      ,[Location]
      ,[Carrier]
      ,[ItemID]
      ,[StartDate]
      ,[EndDate]
      ,[MRC]
      ,[CommissionMRC]
      ,[EMU]
      ,[NRC]
      ,[Payor]
      ,[CycleCode]
      ,[Quantity]
      ,[CircuitID]
      ,[BTN]
      ,[VendorAccountID]
      ,[VendorInvoiceNumber]
      ,[LastModifiedBy]
      ,[LastModifiedDateTime]
      ,[NeedsReview]
      ,[CityCareNetInvID]
      ,[CityCareProdOrderID]
      ,[CityCareCommProfileID]
      ,[CommissionID]
      ,[Comments]
      ,[Scrubbed]
      ,[NeedsReviewComments]
      ,[InActive]
      ,[AccountManager]
      ,[DealerOrSalesPerson]
      ,[ProductNickName]
      ,[InstallDate]
      ,[OrderID]
      ,[ExpirationDate]
      ,[TransactionDate]
      ,[TransactionQuantity]
      ,[TransactionMRC])
select [Customer]
      ,[Location]
      ,[Carrier]
      ,'SIPV'
      ,[StartDate]
      ,[EndDate]
      ,[MRC]
      ,[CommissionMRC]
      ,[EMU]
      ,[NRC]
      ,[Payor]
      ,[CycleCode]
      ,[Quantity]
      ,[CircuitID]
      ,[BTN]
      ,[VendorAccountID]
      ,[VendorInvoiceNumber]
      ,[LastModifiedBy]
      ,[LastModifiedDateTime]
      ,[NeedsReview]
      ,[CityCareNetInvID]
      ,[CityCareProdOrderID]
      ,[CityCareCommProfileID]
      ,[CommissionID]
      ,[Comments]
      ,[Scrubbed]
      ,[NeedsReviewComments]
      ,[InActive]
      ,[AccountManager]
      ,[DealerOrSalesPerson]
      ,'SIPV'
      ,[InstallDate]
      ,[OrderID]
      ,[ExpirationDate]
      ,[TransactionDate]
      ,[TransactionQuantity]
      ,[TransactionMRC]
FROM NetworkInventory where ProductNickName = 'Update to SIPV'

UPDATE NetworkInventory 
SET EndDate = '12/31/2020'
WHERE ProductNickName = 'Update to SIPV'