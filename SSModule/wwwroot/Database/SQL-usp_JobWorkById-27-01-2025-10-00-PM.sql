ALTER procedure [dbo].[usp_JobWorkById]
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
			--(case when Series.TranAlias='LORD' then l.Location else vender.Name end) as PartyName,
			--(case when Series.TranAlias='LORD' then l.Phone1 else vender.Mobile end) as PartyMobile,
			--(case when Series.TranAlias='LORD' then '' else vender.Gstno end) as PartyGSTN,
			--(case when Series.TranAlias='LORD' then l.Address else vender.Address end) as PartyAddress,
			(case when Series.TranAlias='LORD' then l.State else vender.StateName end) as PartyStateName, 
			0 as PartyCredit,
			0 as PartyBalance,
			 Series.Series as SeriesName,Series.FKLocationID,Series.BillingRate,
			(
				Select 
				dtl.*,
				Product.NameToDisplay as Product 
				,Product.HSNCode ,brand.BrandName 
				,Product.FKProdCatgId,Product.FkBrandId
				,Product.CodingScheme 
				from tblJobWork_dtl as dtl
				inner join
				tblProduct_mas as Product on dtl.FkProductId=Product.PkProductId
				left outer join
				tblBrand_mas as brand on Product.FkBrandId=brand.PkBrandId
				Where FkId=trn.PkId and FKSeriesId=trn.FkSeriesId
				And dtl.TranType='I'
				FOR JSON PATH			
			) as TranDetails,
			(
				Select 
				dtl.*,
				Product.NameToDisplay as Product 
				,Product.HSNCode ,brand.BrandName 
				,Product.FKProdCatgId,Product.FkBrandId
				,Product.CodingScheme 
				from tblJobWork_dtl as dtl
				inner join
				tblProduct_mas as Product on dtl.FkProductId=Product.PkProductId
				left outer join
				tblBrand_mas as brand on Product.FkBrandId=brand.PkBrandId
				Where FkId=trn.PkId and FKSeriesId=trn.FkSeriesId
				And dtl.TranType='O'
				FOR JSON PATH			
			) as TranReturnDetails
			,(
				Select 
						branch.* ,bcity.CityName as City
				from tblBranch_mas as branch 
				left outer join
				tblCity_mas as bcity on branch.FkCityId=bcity.PkCityId
				Where branch.PkBranchId=Series.FkBranchId  
				FOR JSON PATH--,WITHOUT_ARRAY_WRAPPER			
			) as BranchDetails  
	from tblJobWork_trn as trn
	inner join tblSeries_mas as Series on trn.FKSeriesId=Series.PkSeriesId
	left outer join TblCustomer_Mas as vender on trn.FkPartyId=vender.PkCustomerId
	left outer join  tblLocation_mas as l on trn.FkPartyId=l.PKLocationID
	Where trn.PkId=@PkId and FKSeriesId=@FkSeriesId	
	FOR JSON PATH
	 );

    -- Get the value from the temporary table into the output parameter
    SELECT @JsonData = JsonData FROM @TempJsonData;

	--Declare @JsonData NVARCHAR(MAX),@ErrMsg NVARCHAR(MAX)
	--exec usp_JobWorkById  @PkId=5,@FkSeriesId=6,@JsonData=@JsonData output,@ErrMsg=@ErrMsg output
	--Select @JsonData
end

