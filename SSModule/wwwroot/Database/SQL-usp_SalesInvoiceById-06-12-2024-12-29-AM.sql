 
ALTER procedure [dbo].[usp_SalesInvoiceById]
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
			 Series.Series+''+Cast(trn.EntryNo as nvarchar(20)) as Inum
			,trn.*,
			--vender.Name as PartyName,
			--vender.Mobile as PartyMobile,
			--vender.Gstno as PartyGSTN,
			--vender.Address as PartyAddress,
			0 as PartyCredit,
			0 as PartyBalance,
			vender.StateName as PartyStateName,
			Series.Series as SeriesName,Series.FKLocationID,Series.BillingRate,
			(
				Select 
				dtl.*,
				Product.NameToDisplay as Product 
				,Product.HSNCode ,brand.BrandName 
				,Product.FKProdCatgId,Product.FkBrandId
				,Product.CodingScheme 
				from tblSalesInvoice_dtl as dtl
				inner join
				tblProduct_mas as Product on dtl.FkProductId=Product.PkProductId
				left outer join
				tblBrand_mas as brand on Product.FkBrandId=brand.PkBrandId
				Where FkId=trn.PkId and FKSeriesId=trn.FkSeriesId
				FOR JSON PATH			
			) as TranDetails
			,(
				Select 
						branch.* ,bcity.CityName as City
				from tblBranch_mas as branch 
				left outer join
				tblCity_mas as bcity on branch.FkCityId=bcity.PkCityId
				Where branch.PkBranchId=Series.FkBranchId  
				FOR JSON PATH--,WITHOUT_ARRAY_WRAPPER			
			) as BranchDetails ,
			(
				Select 
						dtl.TranOutSrNo as SrNo,dtl.Barcode 
				from tblProdQTYBarcode as dtl
				 Where TranOutId=trn.PkId and TranOutSeriesId=trn.FkSeriesId
				FOR JSON PATH			
			) as UniqIdDetails
	from tblSalesInvoice_trn as trn
	inner join
	tblSeries_mas as Series on trn.FKSeriesId=Series.PkSeriesId
	left outer join
	TblCustomer_Mas as vender on trn.FkPartyId=vender.PkCustomerId
	Where trn.PkId=@PkId and FKSeriesId=@FkSeriesId	
	FOR JSON PATH
	 );

    -- Get the value from the temporary table into the output parameter
    SELECT @JsonData = JsonData FROM @TempJsonData;

	--Declare @JsonData NVARCHAR(MAX),@ErrMsg NVARCHAR(MAX)
	--exec usp_SalesInvoiceById  @PkId=5,@FkSeriesId=6,@JsonData=@JsonData output,@ErrMsg=@ErrMsg output
	--Select @JsonData
end

