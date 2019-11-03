SELECT
      TR.TagRequestId
     ,[DateTime]
	 ,pi.[ProductItemID]
	 ,pi.LongName AS [Item Description]
	 
	 ,CASE
      WHEN [TagType]=1  THEN 'Shelf'
      WHEN [TagType]=2  THEN 'Warehouse'
      END as TagType
	  ,CASE
      WHEN [Status]=0  THEN 'Pending'
      WHEN [Status]=1  THEN 'Printed'
      END as [Status]
	 ,tr.BCQty
	 ,tr.[User]
	 ,ic.L2
	 ,tr.ApplyDate
	 ,TR.ApplyPrice
FROM
	 [mbo].[TagRequest] tr 
	
LEFT JOIN ProductITEM pi on pi.[ProductItemID]=tr.ProductItemID
LEFT JOIN [mbo].itemCategories ic on ic.ProductItemId = pi.ProductItemId 
where
	CONVERT(date,DateTime)=CONVERT(date,GETDATE())
and 
	BranchID=@BranchID

 order by [DateTime] desc