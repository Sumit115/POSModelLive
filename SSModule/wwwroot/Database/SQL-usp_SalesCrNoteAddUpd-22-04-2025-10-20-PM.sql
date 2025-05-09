 
ALTER procedure [dbo].[usp_SalesCrNoteAddUpd]
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
	
	ADDRETRY:  
  Begin Try  
   Begin Tran  
   --Exec @rc = sp_getapplock @Resource='usp_SalesCrNoteAddUpd', @LockMode='Exclusive', @LockOwner='Transaction', @LockTimeout=12000  
   --    if @rc >= 0 
		 --  BEGIN
			
				SELECT @OutParam=PkId,@PKSeriesId=FKSeriesId
				FROM OPENJSON(@JsonData)
				WITH ([PkId] [bigint] , [FKSeriesId] [bigint]  ) as jsondata;
				
				if @OutParam=0 
				Begin	
						select @SeriesNo = (SeriesNo+1) from tblSeries_mas where PkSeriesId=@PKSeriesId
						update tblSeries_mas set SeriesNo=@SeriesNo where PkSeriesId=@PKSeriesId
						
						Insert into tblSalesCrNote_trn(FKSeriesId,EntryNo,EntryDate,EntryTime,FkPartyId,GRNo,GRDate,GrossAmt,SgstAmt,TaxAmt,CashDiscount,CashDiscType
											,CashDiscountAmt,TradeDiscAmt,TotalDiscount,RoundOfDiff,Shipping,OtherCharge,NetAmt
											,Cash,CashAmt,Credit,CreditAmt,CreditDate,FKPostAccID,Cheque,ChequeAmt,ChequeNo,ChequeDate,FKBankChequeID
											,CreditCard,CreditCardAmt,CreditCardNo,CreditCardDate,FKBankCreditCardID
											,Remark,InvStatus,DraftMode,TrnStatus
											 ,ModifiedDate,CreationDate
											--,Src
											,FKUserId,FKCreatedByID,FKReferById,FKSalesPerId)
						SELECT FKSeriesId,@SeriesNo,EntryDate,Convert(time,getdate()) EntryTime,FkPartyId,GRNo,GRDate,GrossAmt,SgstAmt,TaxAmt,CashDiscount,CashDiscType
											,CashDiscountAmt,TradeDiscAmt,TotalDiscount,RoundOfDiff,Shipping,OtherCharge,NetAmt
											,Cash,CashAmt,Credit,CreditAmt,CreditDate,FKPostAccID,Cheque,ChequeAmt,ChequeNo,ChequeDate,FKBankChequeID
											,CreditCard,CreditCardAmt,CreditCardNo,CreditCardDate,FKBankCreditCardID
											,Remark,InvStatus,ISNULL(DraftMode, 0),TrnStatus
											--,DateModified,DateCreated
											 ,getdate(),getdate()
											--,ISNULL(Src, 0)
											,FKUserId,FKUserId,FKReferById,FKSalesPerId
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
						[CashAmt] [decimal](18, 4) ,
						[Credit] [bit] ,
						[CreditAmt] [decimal](18, 4) ,
						[CreditDate] [date] ,
						[FKPostAccID] [bigint] ,
						[Cheque] [bit] ,
						[ChequeAmt] [decimal](18, 4) ,
						[ChequeNo] [nvarchar](30) ,
						[ChequeDate] [date] ,
						[FKBankChequeID] [bigint] ,
						[CreditCard] [bit] ,
						[CreditCardAmt] [decimal](18, 4) ,
						[CreditCardNo] [nvarchar](30) ,
						[CreditCardDate] [date] ,
						[FKBankCreditCardID] [bigint] ,
						[Remark] [nvarchar](max),
						[InvStatus] [nchar](1),
						[DraftMode] [bit],
						[TrnStatus] [nchar](10),
						--[DateModified] [datetime2](7),
						--[DateCreated] [datetime2](7),
						[Src] [int],
						[FKUserId] [bigint],
						[FKReferById] [bigint],
						[FKSalesPerId] [bigint]
						) as jsondata;
						
						Select @OutParam=@@identity
						set @JsonData=JSON_MODIFY(@JsonData,'$.PkId', @OutParam);
						 
				End
				Else
				Begin
						Select @src=Src,@FkUserId=FKUserId from tblSalesCrNote_trn where PkId=@OutParam;
						UPDATE [dbo].[tblSalesCrNote_trn]  
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
						,Cash=Trn.Cash
						,CashAmt=Trn.CashAmt
						,Credit=Trn.Credit
						,CreditAmt=Trn.CreditAmt
						,CreditDate=Trn.CreditDate
						,FKPostAccID=Trn.FKPostAccID
						,Cheque=Trn.Cheque
						,ChequeAmt=Trn.ChequeAmt
						,ChequeNo=Trn.ChequeNo
						,ChequeDate=Trn.ChequeDate
						,FKBankChequeID=Trn.FKBankChequeID
						,CreditCard=Trn.CreditCard
						,CreditCardAmt=Trn.CreditCardAmt
						,CreditCardNo=Trn.CreditCardNo
						,CreditCardDate=Trn.CreditCardDate
						,FKBankCreditCardID=Trn.FKBankCreditCardID 
						,Remark=Trn.Remark 
						,FKReferById=Trn.FKReferById 
						,FKSalesPerId=Trn.FKSalesPerId 
						--,CreationDate=Getdate()
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
						[Cash] [bit],
						[CashAmt] [decimal](18, 4) ,
						[Credit] [bit] ,
						[CreditAmt] [decimal](18, 4) ,
						[CreditDate] [date] ,
						[FKPostAccID] [bigint] ,
						[Cheque] [bit] ,
						[ChequeAmt] [decimal](18, 4) ,
						[ChequeNo] [nvarchar](30) ,
						[ChequeDate] [date] ,
						[FKBankChequeID] [bigint] ,
						[CreditCard] [bit] ,
						[CreditCardAmt] [decimal](18, 4) ,
						[CreditCardNo] [nvarchar](30) ,
						[CreditCardDate] [date] ,
						[FKBankCreditCardID] [bigint] ,
						[Remark] [nvarchar](max) ,
						[Statu] [nvarchar](max) ,
						[DATE_MODIFIED] [datetime2](7) ,
						[DATE_CREATED] [datetime2](7) ,
						[src] [int] ,
						[FKUserId] [bigint]  ,
						[FKReferById] [bigint],
						[FKSalesPerId] [bigint]
						) AS Trn  
						Where tblSalesCrNote_trn.PkId=Trn.PkId  
						And tblSalesCrNote_trn.FKSeriesId=Trn.FKSeriesId 
						Set @ErrMsg=''
				End
				
			  exec usp_SalesCrNoteDetailAddUpd  @JsonData 
			   Declare @OutParamVoucher bigint =0;
			   Declare @SeriesNoVoucher bigint =0;
			  exec usp_VoucherAddUpd  @JsonData,@OutParamVoucher Output,@ErrMsg Output,@SeriesNoVoucher Output,@OutParam

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
   set @LogID=@PkId;--uspGetIDOfSeries,,'tblSwilError_Log','PKID'  
   Insert Into tblError_Log Select ISNULL(@LogID,0),ERROR_Message(),GetDate(),GetDate(),@src,@FkUserId 
  END  
  Set @Outparam=0  
 End Catch  
	  
end
