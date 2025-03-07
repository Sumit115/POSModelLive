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

	declare @TranAlias nvarchar(10)=(select TranAlias from tblSeries_mas where PkSeriesId=@FkSeriesId)

	IF OBJECT_ID('tempdb..#UniqIdDetail') IS NOT NULL DROP TABLE #UniqIdDetail;    
		Create Table #UniqIdDetail([SrNo] [Bigint],[Barcode] [nvarchar](max))

	IF OBJECT_ID('tempdb..#UniqIdReturnDetail') IS NOT NULL DROP TABLE #UniqIdReturnDetail;    
		Create Table #UniqIdReturnDetail([SrNo] [Bigint],[Barcode] [nvarchar](max))
	
	if @TranAlias='PJ_R' --Product To Be Received (IN)
	Begin
			Insert into #UniqIdDetail(SrNo,Barcode)
			Select dtl.TranInSrNo as SrNo,dtl.Barcode 
			from tblProdQTYBarcode as dtl
			Where TranInId=@PkId and TranInSeriesId=@FkSeriesId

			Insert into #UniqIdReturnDetail(SrNo,Barcode)
			select  SrNo,cs.Value from tblJobWork_dtl
			cross apply STRING_SPLIT (barcode, ',') cs
			where   FkId=@PkId and FKSeriesId=@FkSeriesId and TranType='O'  
	End
	 Else if @TranAlias='PJ_I' --Product being issued OUT
		Begin
			Insert into #UniqIdDetail(SrNo,Barcode)
			select  SrNo,cs.Value from tblJobWork_dtl
			cross apply STRING_SPLIT (barcode, ',') cs
			where   FkId=@PkId and FKSeriesId=@FkSeriesId  and TranType='I'

			Insert into #UniqIdReturnDetail(SrNo,Barcode)
			Select dtl.TranOutSrNo as SrNo,dtl.Barcode  from tblProdQTYBarcode as dtl
			Where TranOutId=@PkId and TranOutSeriesId=@FkSeriesId 
				  
			IF Not EXISTS(Select * from #UniqIdReturnDetail)
			Insert into #UniqIdReturnDetail(SrNo,Barcode)
			select  SrNo,cs.Value from tblJobWork_dtl
			cross apply STRING_SPLIT (barcode, ',') cs
			where   FkId=@PkId and FKSeriesId=@FkSeriesId  and TranType='O'  

		End
	  Else   --Job Order
		Begin 
			Insert into #UniqIdDetail(SrNo,Barcode)
			select  SrNo,cs.Value from tblJobWork_dtl
			cross apply STRING_SPLIT (barcode, ',') cs
			where   FkId=@PkId and FKSeriesId=@FkSeriesId  and TranType='I'

			Insert into #UniqIdReturnDetail(SrNo,Barcode)
			select  SrNo,cs.Value from tblJobWork_dtl
			cross apply STRING_SPLIT (barcode, ',') cs
			where   FkId=@PkId and FKSeriesId=@FkSeriesId and TranType='O'  
		End
				 

	INSERT INTO @TempJsonData (JsonData)
    Select 
        (
		Select 
			 Series.Series+''+Cast(trn.EntryNo as nvarchar(20)) as Inum
			,trn.*, 
			vender.StateName as PartyStateName, 
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
				Select * from #UniqIdDetail 
				FOR JSON PATH			
			) as UniqIdDetails
			,(
				Select * from #UniqIdReturnDetail 
				FOR JSON PATH			
			) as UniqIdReturnDetails
			,(
				Select 
						branch.* ,bcity.CityName as City
				from tblBranch_mas as branch 
				left outer join
				tblCity_mas as bcity on branch.FkCityId=bcity.PkCityId
				Where branch.PkBranchId=loc.FkBranchId  
				FOR JSON PATH--,WITHOUT_ARRAY_WRAPPER			
			) as BranchDetails  
	from tblJobWork_trn as trn
	inner join tblSeries_mas as Series on trn.FKSeriesId=Series.PkSeriesId
	left outer join TblCustomer_Mas as vender on trn.FkPartyId=vender.PkCustomerId
	left outer join  tblLocation_mas as loc on Series.FKLocationID=loc.PKLocationID
	Where trn.PkId=@PkId and FKSeriesId=@FkSeriesId	
	FOR JSON PATH
	 );

    -- Get the value from the temporary table into the output parameter
    SELECT @JsonData = JsonData FROM @TempJsonData;

	--Declare @JsonData NVARCHAR(MAX),@ErrMsg NVARCHAR(MAX)
	--exec usp_JobWorkById  @PkId=5,@FkSeriesId=6,@JsonData=@JsonData output,@ErrMsg=@ErrMsg output
	--Select @JsonData
end

