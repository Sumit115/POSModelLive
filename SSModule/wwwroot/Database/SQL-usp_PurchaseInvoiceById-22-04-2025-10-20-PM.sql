 
ALTER procedure [dbo].[usp_PurchaseInvoiceById]
( 
	@PkId bigint,
	@FkSeriesId bigint,
	@JsonData nvarchar(max)=null Output,
	@ErrMsg nvarchar(max)=null Output
)
as 
begin
	Set @ErrMsg=''  
	  DECLARE @TempJsonData TABLE (JsonData NVARCHAR(MAX));


	INSERT INTO @TempJsonData (JsonData)
    Select 
        (
		Select 
			trn.*,
			vender.Name as PartyName,
			vender.Mobile as PartyMobile,
			vender.Gstno as PartyGSTN,
			vender.Address as PartyAddress,
			0 as PartyCredit,
			0 as PartyBalance,
			vender.StateName as PartyStateName,
			refby.Name as ReferByName,
			salesper.Name as SalesPerName,
			Series.Series as SeriesName,Series.FKLocationID,Series.BillingRate,
			(
					Select 
					dtl.*,
					Product.NameToDisplay as Product 
					,Product.HSNCode ,brand.BrandName 
					,Product.CodingScheme 
					from tblPurchaseInvoice_dtl as dtl
					inner join tblProduct_mas as Product on dtl.FkProductId=Product.PkProductId
					left outer join tblBrand_mas as brand on Product.FkBrandId=brand.PkBrandId
					Where FkId=trn.PkId and FKSeriesId=trn.FkSeriesId
				FOR JSON PATH			
			) as TranDetails ,
			(
				Select 
						dtl.TranInSrNo as SrNo,dtl.Barcode 
				from tblProdQTYBarcode as dtl
				 Where TranInId=trn.PkId and TranInSeriesId=trn.FkSeriesId
				FOR JSON PATH			
			) as UniqIdDetails,
			(case When  (Select Count(*) from tblProdStock_dtl   stockdtl
							 Inner join  tblProdLot_Dtl AS lotDtl 
							 on stockdtl.FKProductId=lotDtl.FKProductId  and stockdtl.FKLotID=lotDtl.PkLotId  
							Where lotDtl.InTrnID=trn.PkId and lotDtl.InTrnFKSeriesID=trn.FkSeriesId And OutStock>0)>0 then 1 else 0 end) as IsLock
	from tblPurchaseInvoice_trn as trn
	inner join tblSeries_mas as Series on trn.FKSeriesId=Series.PkSeriesId
	inner join TblVendor_Mas as vender on trn.FkPartyId=vender.PkVendorId
	Left join tblReferBy_mas as refby on trn.FKReferById=refby.PkReferById 
	Left join tblEmployee_mas as salesper on trn.FKSalesPerId=salesper.PkEmployeeId 
	Where trn.PkId=@PkId and FKSeriesId=@FkSeriesId	
	FOR JSON PATH
	 );

    -- Get the value from the temporary table into the output parameter
    SELECT @JsonData = JsonData FROM @TempJsonData;

	--Declare @JsonData NVARCHAR(MAX),@ErrMsg NVARCHAR(MAX)
	--exec usp_PurchaseInvoiceById  @PkId=5,@FkSeriesId=6,@JsonData=@JsonData output,@ErrMsg=@ErrMsg output
	--Select @JsonData
end

