 
Create procedure [dbo].[usp_GetProductDetailReturn]
(
	@Barcode  nvarchar(20)='',
	@FKPartyID bigint=0 
	
)
as 
BEGIN 
		declare @ProductId bigint=0,@LotId bigint=0, @TranOutId bigint=0,@TranOutSeriesId bigint=0;
		if @Barcode <> '' 
			Begin
				Select top 1 @LotId =lot.PkLotId,@ProductId=prd.PkProductId
				,@TranOutId=a.TranOutId,@TranOutSeriesId=a.TranOutSeriesId
				from tblProdQTYBarcode a
				INNER JOIN  tblProdLot_dtl lot on a.FkLotID=lot.PkLotId and a.FkProductId=lot.FKProductId
				INNER JOIN  tblProduct_mas prd on lot.FKProductId=prd.PkProductId
				Where a.Barcode=@Barcode and ISNULL(a.TranOutId,'')!='' and ISNULL(a.TranOutSeriesId,'')!='' and ISNULL(a.TranOutSrNo,'')!=''
			End 

		Select top(1) prd.PkProductId,prd.Product,
		ISNULL(lot.MRP,prd.MRP) as MRP,ISNULL(lot.SaleRate,prd.SaleRate) as SaleRate,ISNULL(lot.TradeRate,prd.TradeRate) as TradeRate,
		ISNULL(lot.DistributionRate,prd.DistributionRate) as DistributionRate,ISNULL(lot.PurchaseRate,prd.PurchaseRate) as PurchaseRate,
		ISNULL(lot.PurchaseRateUnit,'1') as PurchaseRateUnit,
		ISNULL(lot.PkLotId,0) as PkLotId,lot.Color,lot.Batch
		,0 as FkOrderId,0 as FKOrderSrID,0 as OrderSrNo
		,ISNULL(trn.FkPartyId,0) as FkPartyId,trn.PartyName,trn.PartyMobile,trn.PartyAddress ,trn.PartyDob ,trn.PartyMarriageDate 
		,ISNULL(prd.FkBrandId,0) as FkBrandId,ISNULL(prd.FKProdCatgId,0) as FKProdCatgId,ISNULL(prd.CodingScheme,'fixed') as CodingScheme
		,ISNULL(trn.PkId,0) as FKInvoiceID,ISNULL(trn.FKSeriesId,0) as FKSeriesId ,c.Series+''+Cast(trn.EntryNo as nvarchar(20)) as FKInvoiceID_Text
		from	tblProduct_mas prd 
		--INNER To Left Join Because Purchase Case Lot Not found then return blank Table
		Left Outer JOIN tblProdLot_dtl lot on lot.FKProductId=prd.PkProductId
		Inner JOIN tblSalesInvoice_dtl invDtl on invDtl.FKProductId=prd.PkProductId and invDtl.FkId=@TranOutId and invDtl.FKSeriesId=@TranOutSeriesId and invDtl.Batch=lot.Batch
		Inner JOIN tblSalesInvoice_trn trn on trn.PkId=invDtl.FkId and trn.PkId=@TranOutId and trn.FKSeriesId=@TranOutSeriesId  
		Inner join  tblSeries_mas as c on trn.FKSeriesId=c.PkSeriesId 
			where prd.PkProductId=@ProductId and (@lotId=0 or lot.PkLotId=@lotId)
		and (@FKPartyID=0 or trn.FkPartyId=@FKPartyID)
			
END






