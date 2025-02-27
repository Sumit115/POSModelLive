 
ALTER procedure [dbo].[usp_GetProductDetail]
(
	@Barcode  nvarchar(20)='',
	@ProductId bigint=0,
	@LotId bigint=0,
	@ProductName  nvarchar(50)='',
	@FKOrderID bigint=0,
	@FKOrderSrID bigint=0
	
)
as 
BEGIN 
		if @Barcode <> '' 
			Begin
				Select top 1 @LotId =lot.PkLotId,@ProductId=prd.PkProductId  from tblProdQTYBarcode a
				INNER JOIN  tblProdLot_dtl lot on a.FkLotID=lot.PkLotId and a.FkProductId=lot.FKProductId
				INNER JOIN  tblProduct_mas prd on lot.FKProductId=prd.PkProductId
				Where a.Barcode=@Barcode and ISNULL(a.TranOutId,'')='' and ISNULL(a.TranOutSeriesId,'')='' and ISNULL(a.TranOutSrNo,'')=''

				if @ProductId=0
					begin
						Select @LotId =lot.PkLotId,@ProductId=prd.PkProductId  from	tblProdLot_dtl lot  
						INNER JOIN  tblProduct_mas prd on lot.FKProductId=prd.PkProductId	Where lot.Barcode=@Barcode 
					end
				if @ProductId=0
					begin
						Select @ProductId=PkProductId  from	tblProduct_mas 	Where Barcode=@barcode
					End
			End
		
		if @ProductName <> '' 
			Begin
				Select top 1 @ProductId=a.PkProductId  from tblProduct_mas a  
				Where a.Product=@ProductName 
			End

		Select top(1) prd.PkProductId,prd.Product,
		ISNULL(lot.MRP,prd.MRP) as MRP,ISNULL(lot.SaleRate,prd.SaleRate) as SaleRate,ISNULL(lot.TradeRate,prd.TradeRate) as TradeRate,
		ISNULL(lot.DistributionRate,prd.DistributionRate) as DistributionRate,ISNULL(lot.PurchaseRate,prd.PurchaseRate) as PurchaseRate,
		ISNULL(lot.PurchaseRateUnit,'1') as PurchaseRateUnit,
		ISNULL(lot.PkLotId,0) as PkLotId,lot.Color,lot.Batch
		,ISNULL(odrDtl.FkId,0) as FkOrderId,ISNULL(odrDtl.FKSeriesId,0) as FKOrderSrID,ISNULL(odrDtl.SrNo,0) as OrderSrNo
		,ISNULL(prd.FkBrandId,0) as FkBrandId,ISNULL(prd.FKProdCatgId,0) as FKProdCatgId,ISNULL(prd.CodingScheme,'fixed') as CodingScheme
		from	tblProduct_mas prd 
		--INNER To Left Join Because Purchase Case Lot Not found then return blank Table
		Left Outer JOIN tblProdLot_dtl lot on lot.FKProductId=prd.PkProductId
		Left Outer JOIN tblSalesOrder_dtl odrDtl on odrDtl.FKProductId=prd.PkProductId and odrDtl.FkId=@FKOrderID and odrDtl.FKSeriesId=@FKOrderSrID and odrDtl.Batch=lot.Batch
		where prd.PkProductId=@ProductId and (@lotId=0 or lot.PkLotId=@lotId)
			
END






