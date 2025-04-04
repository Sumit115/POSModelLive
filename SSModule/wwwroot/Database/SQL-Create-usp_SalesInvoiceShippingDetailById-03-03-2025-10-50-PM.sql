 
 Create procedure [dbo].[usp_SalesInvoiceShippingDetailById]
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
			,trn.PkId,trn.FKSeriesId
			,trn.BiltyNo,trn.BiltyDate,trn.TransportName,trn.NoOfCases,trn.FreightType
			,trn.FreightAmt,trn.ShipingAddress,trn.PaymentMode,trn.FKBankThroughBankID
			,trn.DeliveryDate,trn.ShippingMode,trn.PaymentDays 
			  ,(
				Select 
						eway.*  
				from tblEWayDtl_Lnk as eway  
				Where eway.FKID=trn.PkId And eway.FkSeriesId=trn.FKSeriesId
				FOR JSON PATH--,WITHOUT_ARRAY_WRAPPER			
			) as EWayDetails  
	from tblSalesInvoice_trn as trn
	inner join tblSeries_mas as Series on trn.FKSeriesId=Series.PkSeriesId
	left outer join TblCustomer_Mas as vender on trn.FkPartyId=vender.PkCustomerId
	left outer join  tblLocation_mas as loc on Series.FKLocationID=loc.PKLocationID
	Where trn.PkId=@PkId and FKSeriesId=@FkSeriesId	
	FOR JSON PATH
	 );

    -- Get the value from the temporary table into the output parameter
    SELECT @JsonData = JsonData FROM @TempJsonData;

	--Declare @JsonData NVARCHAR(MAX),@ErrMsg NVARCHAR(MAX)
	--exec usp_SalesInvoiceById  @PkId=5,@FkSeriesId=6,@JsonData=@JsonData output,@ErrMsg=@ErrMsg output
	--Select @JsonData
end

