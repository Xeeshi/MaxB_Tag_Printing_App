Declare @RequestPID int;
set @RequestPID=(
SELECT TOP 01 Productitemid from [mbo].TagRequest WHERE  TagRequestId=@TRID)

SELECT Distinct 
         tr.TagRequestId
		 ,tr.DateTime
		,pi.ProductItemId
		,pi.LongName
		,pi.Barcode
		,(Select SUBSTRING((SELECT ',' + pib.Barcode AS 'data()' FROM ProductItemBarcode pib where pib.ProductItemId = pi.ProductItemId FOR XML PATH('')), 0, 9999)) as AltBarcode
		,(SELECT (select MAX(myval) from (values ([Target1_5]),([Target6_10]),([Target11_15]),([Target16_20]),([Target21_25]),([Target26_31])) as D(myval)) AS 'MaxTarget' FROM [mbo].[PSOrderingTargets] where ProductItemId=pi.ProductItemId)AS 'MaxTarget'
		,omoq.[MOQ]
		,omoq.[MOQUnit]
		,ic.L2
		,pi.SaleRate
		,tr.BCQty as BCQty
		,pid.Facings
		,TagType
		,tr.BranchID
		,tr.ApplyDate
		,tr.ApplyPrice
		,DATEDIFF(hh, tr.[DateTime],tr.ApplyDate) as Hours_Difference

FROM ProductItem pi
LEFT JOIN mbo.PSOrderingMOQ omoq on omoq.ProductItemid=pi.productitemid
LEFT JOIN [mbo].TagRequest tr on tr.ProductItemid=pi.productitemid
LEFT JOIN [mbo].[PlanogramShelfDimensions] pid on pid.ProductItemid=pi.productitemid
LEFT JOIN ProductItemBarcode pib on pib.ProductItemId = pi.ProductItemId 
LEFT JOIN [mbo].itemCategories ic on ic.ProductItemId = pi.ProductItemId 
WHERE pi.ProductItemId=@RequestPID