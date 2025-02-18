
Create procedure [dbo].[usp_PurchaseInvoiceDelete]
( 
	@PkId bigint,
	@FkSeriesId bigint,
	@ErrMsg NVarchar(500)=null Output
)
as 
begin
	ADDRETRY:  
  Begin Try  
	   Begin Tran  
		declare @IsLock int=((case When  (Select Count(*) from tblProdStock_dtl   stockdtl
								 Inner join  tblProdLot_Dtl AS lotDtl 
								 on stockdtl.FKProductId=lotDtl.FKProductId  and stockdtl.FKLotID=lotDtl.PkLotId  
								Where lotDtl.InTrnID=@PkId and lotDtl.InTrnFKSeriesID=@FkSeriesId And OutStock>0)>0 then 1 else 0 end))
		if @IsLock=0
			Begin
				Delete tblProdStock_dtl from tblProdStock_dtl 
										 Inner join 
										 tblProdLot_Dtl AS Trn 
										 on tblProdStock_dtl.FKProductId=Trn.FKProductId 
										and tblProdStock_dtl.FKLotID=Trn.PkLotId  
										Where InTrnID=@PkId and InTrnFKSeriesID=@FkSeriesId

				Delete from TblProdQTYBarcode Where TranInId=@PkId and TranInSeriesId=@FkSeriesId
				Delete  from tblPurchaseinvoice_dtl  Where FkId = @PkId and FKSeriesId=@FkSeriesId
				Delete from tblProdLot_Dtl Where InTrnID=@PkId and InTrnFKSeriesID=@FkSeriesId 
				Delete from tblPurchaseinvoice_trn  Where pkId = @PkId and FKSeriesId=@FkSeriesId

				Set @ErrMsg=''  
			End
		Else
		   Begin
			Set @ErrMsg='Invalid Request'  
		   End
  Commit Tran   
 End Try   
 Begin Catch   
  Declare @LogID bigint   
  Set @ErrMsg=ERROR_Message()  
     
  Rollback Tran  
  
  IF ERROR_NUMBER() = 1205  
  BEGIN  
   WAITFOR DELAY '00:00:00.10'  
   GOTO ADDRETRY  
  END  
  ELSE  
  BEGIN  
   set @LogID=ISNULL(@PkId,0);--uspGetIDOfSeries,,'tblSwilError_Log','PKID'  
   Insert Into tblError_Log Select @LogID,ERROR_Message(),GetDate(),GetDate(),0,0 
  END  
   
End Catch
	  
end 
