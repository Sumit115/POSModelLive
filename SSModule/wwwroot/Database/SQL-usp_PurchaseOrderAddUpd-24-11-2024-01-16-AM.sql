Alter procedure [dbo].[usp_PurchaseOrderAddUpd]
( 
	@JsonData nvarchar(max)  , 
	@OutParam bigint Output,   
	@ErrMsg nvarchar(max)=null Output,  
	@SeriesNo Bigint=0 Output  
)
as 
begin	
	 Declare @rc int  ;
	 Declare @PkId bigint, @src int = 0, @FkUserId bigint = 0,@PKSeriesId bigint=0,@TranAlias nvarchar (max); 
	declare @DateModified  datetime2(7) ,@DateCreated datetime2(7) ,@FDate datetime,@TDate datetime ;
	 Declare  @FkPartyId bigint= 0;
	
	ADDRETRY:  
  Begin Try  
   Begin Tran  
   --Exec @rc = sp_getapplock @Resource='usp_PurchaseOrderAddUpd', @LockMode='Exclusive', @LockOwner='Transaction', @LockTimeout=12000  
   --    if @rc >= 0 
		 --  BEGIN
			
				SELECT @OutParam=PkId,@PKSeriesId=FKSeriesId,@FkPartyId=FkPartyId
				FROM OPENJSON(@JsonData)
				WITH ([PkId] [bigint] , [FKSeriesId] [bigint],[FkPartyId] [bigint]  ) as jsondata;
				
				if @OutParam=0 
				Begin	
						select @SeriesNo = (SeriesNo+1) from tblSeries_mas where PkSeriesId=@PKSeriesId
						update tblSeries_mas set SeriesNo=@SeriesNo where PkSeriesId=@PKSeriesId
						
						Insert into tblPurchaseOrder_trn(FKSeriesId,EntryNo,EntryDate,EntryTime,FkPartyId,GRNo,GRDate,GrossAmt,SgstAmt,TaxAmt,CashDiscount,CashDiscType
											,CashDiscountAmt,TradeDiscAmt,TotalDiscount,RoundOfDiff,Shipping,OtherCharge,NetAmt,Cash,CashAmt,Credit,CreditAmt,CreditDate,Cheque,ChequeAmt
											,ChequeNo,ChequeDate,FKBankChequeID,Remark,InvStatus,DraftMode,TrnStatus
											,FKUserId,ModifiedDate,FKCreatedByID,CreationDate
											 ,FreePoint)
						SELECT FKSeriesId,@SeriesNo,EntryDate,Convert(time,getdate()) EntryTime,FkPartyId,GRNo,GRDate,GrossAmt,SgstAmt,TaxAmt,CashDiscount,CashDiscType
											,CashDiscountAmt,TradeDiscAmt,TotalDiscount,RoundOfDiff,Shipping,OtherCharge,NetAmt,Cash,CashAmt,Credit,CreditAmt,CreditDate,Cheque,ChequeAmt
											,ChequeNo,ChequeDate,FKBankChequeID,Remark,InvStatus,ISNULL(DraftMode, 0),TrnStatus
											,FKUserId,getdate(),FKUserId,getdate()
											 ,FreePoint
						FROM OPENJSON(@JsonData)
						WITH (
						[FKSeriesId] [bigint],
						[EntryNo] [bigint],
						[EntryDate] [date],
						[EntryTime] [time](7),
						[FkPartyId] [bigint],
						[GRNo] [nvarchar](100),
						[GRDate] [datetime],
						[GrossAmt] [decimal](18, 2),
						[SgstAmt] [decimal](18, 2),
						[TaxAmt] [decimal](18, 2),
						[CashDiscount] [decimal](18, 2),
						[CashDiscType] [nchar](1),
						[CashDiscountAmt] [decimal](18, 2),
						[TradeDiscAmt] [decimal](18, 2),
						[TotalDiscount] [decimal](18, 2),
						[RoundOfDiff] [decimal](18, 2),
						[Shipping] [decimal](18, 2),
						[OtherCharge] [decimal](18, 2),
						[NetAmt] [decimal](18, 2),
						[Cash] [bit],
						[CashAmt] [decimal](18, 2),
						[Credit] [bit],
						[CreditAmt] [decimal](18, 2),
						[CreditDate] [date],
						[Cheque] [bit] ,
						[ChequeAmt] [decimal](18, 2) ,
						[ChequeNo] [nvarchar](30),
						[ChequeDate] [date],
						[FKBankChequeID] [bigint],
						[Remark] [nvarchar](max),
						[InvStatus] [nchar](1),
						[DraftMode] [bit],
						[TrnStatus] [nchar](10),
						--[DateModified] [datetime2](7),
						--[DateCreated] [datetime2](7),
						[Src] [int],
						[FKUserId] [bigint],
						[FreePoint] [decimal](18, 2) 
						) as jsondata;
						
						Select @OutParam=@@identity
						set @JsonData=JSON_MODIFY(@JsonData,'$.PkId', @OutParam);
						 
				End
				Else
				Begin

						update tblVendor_mas set FreePoint-=Trn.FreePoint 
						From tblPurchaseInvoice_trn Trn Inner Join tblVendor_mas vendor on Trn.FkPartyId=vendor.PkVendorId
						Where  Trn.PkId =@OutParam  And  Trn.FKSeriesId =@PKSeriesId
						And vendor.PkVendorId=@FkPartyId

						UPDATE [dbo].[tblPurchaseOrder_trn]  
						SET  EntryDate=Trn.EntryDate
						,GRNo=Trn.GRNo
						,GRDate=Trn.GRDate
						,GrossAmt=Trn.GrossAmt
						,SgstAmt=Trn.SgstAmt
						,TaxAmt=Trn.TaxAmt
						,CashDiscount=Trn.CashDiscount
						,CashDiscType=Trn.CashDiscType
						,CashDiscountAmt=Trn.CashDiscountAmt
						,TradeDiscAmt=Trn.TradeDiscAmt
						,TotalDiscount=Trn.TotalDiscount
						,RoundOfDiff=Trn.RoundOfDiff
						,Shipping=Trn.Shipping
						,OtherCharge=Trn.OtherCharge
						,NetAmt=Trn.NetAmt
						,FKUserId=Trn.FKUserId
						,ModifiedDate=Getdate()
						,Remark=Trn.Remark 
						,FreePoint=Trn.FreePoint 
						--,Cash,CashAmt,Credit,CreditAmt,CreditDate,Cheque,ChequeAmt,ChequeNo,ChequeDate,FKBankChequeID,Statu,DateModified,DateCreated,src,FKUserId
						FROM OPENJSON(@JsonData)
						WITH (  
						[PkId] [bigint] ,
						[FKSeriesId] [bigint] ,
						[EntryNo] [bigint] ,
						[EntryDate] [datetime2](7) ,
						[TranAlias] [nvarchar](max) ,
						[FkPartyId] [int] ,
						[GRNo] [nvarchar](max) ,
						[GRDate] [datetime2](7) ,
						[GrossAmt] [decimal](18, 2) ,
						[SgstAmt] [decimal](18, 2) ,
						[TaxAmt] [decimal](18, 2) ,
						[CashDiscount] [decimal](18, 2) ,
						[CashDiscType] [nvarchar](max) ,
						[CashDiscountAmt] [decimal](18, 2) ,
						[TradeDiscAmt] [decimal](18, 2),
						[TotalDiscount] [decimal](18, 2) ,
						[RoundOfDiff] [decimal](18, 2) ,
						[Shipping] [decimal](18, 2) ,
						[OtherCharge] [decimal](18, 2) ,
						[NetAmt] [decimal](18, 2) ,
						[Cash] [bit] ,
						[CashAmt] [decimal](18, 2) ,
						[Credit] [bit] ,
						[CreditAmt] [decimal](18, 2) ,
						[CreditDate] [datetime2](7) ,
						[Cheque] [bit] ,
						[ChequeAmt] [decimal](18, 2) ,
						[ChequeNo] [nvarchar](max) ,
						[ChequeDate] [datetime2](7) ,
						[FKBankChequeID] [bigint] ,
						[Remark] [nvarchar](max) ,
						[Statu] [nvarchar](max) ,
						[DATE_MODIFIED] [datetime2](7) ,
						[DATE_CREATED] [datetime2](7) ,
						[src] [int] ,
						[FKUserId] [bigint] ,  
						[FreePoint] [decimal](18, 2)    
						) AS Trn  
						Where tblPurchaseOrder_trn.PkId=Trn.PkId  
						And tblPurchaseOrder_trn.FKSeriesId=Trn.FKSeriesId 
						Set @ErrMsg=''
				End
				print 'AAA'
			  exec usp_PurchaseOrderDetailAddUpd  @JsonData 
				print 'CCC'
				
			  	update TblCustomer_mas set FreePoint+=Trn.FreePoint 
						From tblSalesInvoice_trn Trn Inner Join TblCustomer_mas cust on Trn.FkPartyId=cust.PkCustomerId
						Where  Trn.PkId =@OutParam  And  Trn.FKSeriesId =@PKSeriesId
						And cust.PkCustomerId=@FkPartyId
			  
		   --END
	   --ELSE
		  -- BEGIN
				--Set @Outparam=0  
			 --  Set @ErrMsg='ServerBusy'  
		  -- END
 
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
   Insert Into tblError_Log Select @LogID,ERROR_Message(),GetDate(),GetDate(),@src,@FkUserId 
  END  
  Set @Outparam=0  
End Catch
	  
end