ALTER procedure [dbo].[usp_LocationRequestById]
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

	  IF OBJECT_ID('tempdb..#tmptblSalesOrder_trn') IS NOT NULL  
	  DROP TABLE #tmptblSalesOrder_trn;   

	  SELECT * INTO #tmptblSalesOrder_trn  
	  FROM tblSalesOrder_trn where PkId=@PkId and FKSeriesId=@FkSeriesId	
		
	  ALTER TABLE #tmptblSalesOrder_trn 
	  DROP COLUMN FkPartyId

	INSERT INTO @TempJsonData (JsonData)
    Select 
        (
		Select 
			loc.PKLocationID as FkPartyId
			,trn.*
			,loc.Location  as PartyName
			,loc.Phone1  as PartyMobile
			,loc.Address   as PartyAddress
			,0 as PartyCredit,
			0 as PartyBalance,
			'' as PartyStateName, 
			Series.Series as SeriesName,Series.FKLocationID,Series.BillingRate,
			(
				Select 
				dtl.*,
				Product.NameToDisplay as Product 
				,Product.HSNCode ,brand.BrandName 
				,Product.FKProdCatgId,Product.FkBrandId
				,Product.CodingScheme 
				from tblSalesOrder_dtl as dtl
				inner join tblProduct_mas as Product on dtl.FkProductId=Product.PkProductId
				left outer join tblBrand_mas as brand on Product.FkBrandId=brand.PkBrandId
				Where FkId=trn.PkId and FKSeriesId=trn.FkSeriesId
				FOR JSON PATH			
			) as TranDetails
			,(
				Select 
						branch.* ,bcity.CityName as City
				from tblBranch_mas as branch 
				left outer join
				tblCity_mas as bcity on branch.FkCityId=bcity.PkCityId
				Where branch.PkBranchId=loc.FkBranchId  
				FOR JSON PATH--,WITHOUT_ARRAY_WRAPPER			
			) as BranchDetails 
	from #tmptblSalesOrder_trn as trn
	inner join  tblSeries_mas as Series on trn.FKSeriesId=Series.PkSeriesId 
	left outer join  tblLocation_mas as loc on Series.FKLocationID=loc.PKLocationID
			Where trn.PkId=@PkId and FKSeriesId=@FkSeriesId	
	FOR JSON PATH
	 );

    -- Get the value from the temporary table into the output parameter
    SELECT @JsonData = JsonData FROM @TempJsonData;

	--Declare @JsonData NVARCHAR(MAX),@ErrMsg NVARCHAR(MAX)
	--exec usp_SalesOrderById  @PkId=5,@FkSeriesId=6,@JsonData=@JsonData output,@ErrMsg=@ErrMsg output
	--Select @JsonData
end

