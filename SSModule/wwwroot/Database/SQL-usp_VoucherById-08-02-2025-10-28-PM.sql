ALTER procedure [dbo].[usp_VoucherById]
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
			trn.*,trn.PKVoucherID as PkId,  
			Series.Series as SeriesName,Series.FKLocationID,Series.BillingRate,
			(
				Select 
				dtl.* 
				,(case when dtl.VoucherAmt>0 then ABS(dtl.VoucherAmt) else  0 end) as CreditAmt
				,(case when dtl.VoucherAmt<0 then ABS(dtl.VoucherAmt) else  0 end) as DebitAmt
				,acc.Account  as AccountName_Text
				, ABS(wallet.BalAmount) as CurrentBalance
				,(case when wallet.BalAmount>0 then 'Cr' else  'Dr' end) as AccMode 
				from tblVoucher_Dtl as dtl
				inner join tblAccount_mas as acc on dtl.FKAccountID=acc.PkAccountId
				inner join tblWallet_mas as wallet on wallet.FkAccountId=acc.PkAccountId
				Where dtl.FKVoucherID=trn.PKVoucherID and FKSeriesId=trn.FkSeriesId
				FOR JSON PATH			
			) as VoucherDetails
			,(
				Select 
						branch.* ,bcity.CityName as City
				from tblBranch_mas as branch 
				left outer join
				tblCity_mas as bcity on branch.FkCityId=bcity.PkCityId
				Where branch.PkBranchId=loc.FkBranchId  
				FOR JSON PATH--,WITHOUT_ARRAY_WRAPPER			
			) as BranchDetails 
	from tblVoucher_Trn as trn
	inner join tblSeries_mas as Series on trn.FKSeriesId=Series.PkSeriesId
	left outer join  tblLocation_mas as loc on Series.FKLocationID=loc.PKLocationID
	Where trn.PKVoucherID=@PkId and FKSeriesId=@FkSeriesId	
	FOR JSON PATH
	 );

    -- Get the value from the temporary table into the output parameter
    SELECT @JsonData = JsonData FROM @TempJsonData;

	--Declare @JsonData NVARCHAR(MAX),@ErrMsg NVARCHAR(MAX)
	--exec usp_SalesOrderById  @PkId=5,@FkSeriesId=6,@JsonData=@JsonData output,@ErrMsg=@ErrMsg output
	--Select @JsonData
end

