 
ALTER procedure [dbo].[usp_SalesInvoiceAddUpd]
( 
	@JsonData nvarchar(max)  , 
	@OutParam bigint Output,   
	@ErrMsg nvarchar(max)=null Output,  
	@SeriesNo Bigint=0 Output  
)
as 
begin	
	 Declare @rc int  ;
	 Declare @PkId bigint, @src int = 0, @FKUserId bigint = 0,@PKSeriesId bigint=0,@TranAlias nvarchar (max); 
	declare @DateModified  datetime2(7) ,@DateCreated datetime2(7) ,@FDate datetime,@TDate datetime ;
	 Declare  @FkPartyId bigint= 0;
	 Declare @FKOrderID bigint,@FKOrderSrID bigint;
	ADDRETRY:  
  Begin Try  
   Begin Tran  
   --Exec @rc = sp_getapplock @Resource='usp_SalesInvoiceAddUpd', @LockMode='Exclusive', @LockOwner='Transaction', @LockTimeout=12000  
   --    if @rc >= 0 
		 --  BEGIN
			SELECT  @SeriesNo=0,@OutParam=PkId,@PKSeriesId=FKSeriesId,@TranAlias=TranAlias ,@FkPartyId=FkPartyId
				,@FKOrderID=FKOrderID,@FKOrderSrID=FKOrderSrID
				FROM OPENJSON(@JsonData)
				WITH ([PkId] [bigint] , [FKSeriesId] [bigint] ,[TranAlias] [nvarchar](100),[FkPartyId] [bigint],[FKOrderID] [bigint],[FKOrderSrID] [bigint]  ) as jsondata;
			
			declare @FkHoldLocationId bigint=(Select cast(Isnull((Select SysDefValue from tblSysDefaults where SysDefKey='FkHoldLocationId'),0) as bigint));
		 
		 if (@TranAlias!='LINV' OR (@TranAlias='LINV' AND @FkHoldLocationId>0))
			Begin 
				if @OutParam=0 
				Begin	
						
						select @SeriesNo = (SeriesNo+1) from tblSeries_mas where PkSeriesId=@PKSeriesId
						update tblSeries_mas set SeriesNo=@SeriesNo where PkSeriesId=@PKSeriesId
						
						Insert into tblSalesInvoice_trn(FKSeriesId,EntryNo,EntryDate,EntryTime,FkPartyId,GRNo,GRDate,GrossAmt,SgstAmt,TaxAmt,CashDiscount,CashDiscType
											,CashDiscountAmt,TradeDiscAmt,TotalDiscount,RoundOfDiff,Shipping,OtherCharge,NetAmt
											,Cash,CashAmt,Credit,CreditAmt,CreditDate,FKPostAccID,Cheque,ChequeAmt,ChequeNo,ChequeDate,FKBankChequeID
											,CreditCard,CreditCardAmt,CreditCardNo,CreditCardDate,FKBankCreditCardID
											,Remark,InvStatus,DraftMode,TrnStatus
											 ,FKUserId,ModifiedDate,FKCreatedByID,CreationDate
											 ,PartyName,PartyMobile,PartyAddress,PartyDob,PartyMarriageDate
											 ,FreePoint,FKReferById,FKSalesPerId)
						SELECT FKSeriesId,@SeriesNo,EntryDate,Convert(time,getdate()) EntryTime,FkPartyId,GRNo,GRDate,GrossAmt,SgstAmt,TaxAmt,CashDiscount,CashDiscType
											,CashDiscountAmt,TradeDiscAmt,TotalDiscount,RoundOfDiff,Shipping,OtherCharge,NetAmt
											,Cash,CashAmt,Credit,CreditAmt,CreditDate,FKPostAccID,Cheque,ChequeAmt,ChequeNo,ChequeDate,FKBankChequeID
											,CreditCard,CreditCardAmt,CreditCardNo,CreditCardDate,FKBankCreditCardID
											,Remark,InvStatus,ISNULL(DraftMode, 0),TrnStatus
											 ,FKUserId,getdate(),FKUserId,getdate()
											 ,PartyName,PartyMobile,PartyAddress,PartyDob,PartyMarriageDate
											 ,FreePoint,FKReferById,FKSalesPerId
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
						[Src] [int],
						[FKUserId] [bigint],
						[PartyName] [nvarchar](50),
						[PartyMobile] [nvarchar](50),
						[PartyAddress] [nvarchar](max),
						[PartyDob] [nvarchar](50),
						[PartyMarriageDate] [nvarchar](50),
						[FreePoint] [decimal](18, 2)  ,
						[FKReferById] [bigint],
						[FKSalesPerId] [bigint]
						) as jsondata;
						
						Select @OutParam=@@identity
						set @JsonData=JSON_MODIFY(@JsonData,'$.PkId', @OutParam);
						 
				End
				Else
				Begin
						update TblCustomer_mas set FreePoint-=Trn.FreePoint 
						From tblSalesInvoice_trn Trn Inner Join TblCustomer_mas cust on Trn.FkPartyId=cust.PkCustomerId
						Where  Trn.PkId =@OutParam  And  Trn.FKSeriesId =@PKSeriesId
						And cust.PkCustomerId=@FkPartyId

						Select --@src=Src,
						@FKUserId=FKUserId from tblSalesInvoice_trn where PkId=@OutParam;
						UPDATE [dbo].[tblSalesInvoice_trn]  
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
						,PartyName=Trn.PartyName
						,PartyMobile=Trn.PartyMobile
						,PartyAddress=Trn.PartyAddress
						,PartyDob=Trn.PartyDob
						,PartyMarriageDate=Trn.PartyMarriageDate 
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
						,FreePoint=Trn.FreePoint 
						,FKReferById=Trn.FKReferById 
						,FKSalesPerId=Trn.FKSalesPerId 
						--,DateCreated=Getdate()
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
						[FKUserId] [bigint],  
						[PartyName] [nvarchar](50),
						[PartyMobile] [nvarchar](50),
						[PartyAddress] [nvarchar](max),
						[PartyDob] [nvarchar](50),
						[PartyMarriageDate] [nvarchar](50),
						[FreePoint] [decimal](18, 2)  ,
						[FKReferById] [bigint],
						[FKSalesPerId] [bigint]
						) AS Trn  
						Where tblSalesInvoice_trn.PkId=Trn.PkId  
						And tblSalesInvoice_trn.FKSeriesId=Trn.FKSeriesId 
						Set @ErrMsg=''

						--Stock Minus For Edit Mode
						UPDATE [dbo].[tblProdStock_dtl]  
						SET  OutStock -= (dtl.Qty+dtl.FreeQty) 
						FROM  tblSalesInvoice_dtl AS dtl 
						Inner join tblSalesInvoice_trn trn on dtl.FkId=trn.PkId
						Where tblProdStock_dtl.FKProductId=dtl.FkProductId 
						and tblProdStock_dtl.FKLotID=dtl.FkLotId  
						and tblProdStock_dtl.FKLocationID=dtl.FKLocationID 
						And dtl.FkId=@OutParam

						if @TranAlias='LINV' and Isnull(@FKOrderID,0)<=0 and Isnull(@FKOrderSrID,0)<=0
						 Begin
								UPDATE [dbo].[tblProdStock_dtl]  
								SET  InStock -= (dtl.Qty+dtl.FreeQty) 
								FROM  tblSalesInvoice_dtl AS dtl 
								Inner join tblSalesInvoice_trn trn on dtl.FkId=trn.PkId
								Where tblProdStock_dtl.FKProductId=dtl.FkProductId 
								and tblProdStock_dtl.FKLotID=dtl.FkLotId  
								and tblProdStock_dtl.FKLocationID=@FkHoldLocationId 
								And dtl.FkId=@OutParam
						  End
				End
				
				if @TranAlias='LINV'
				 Begin
					 UPDATE [dbo].[tblSalesInvoice_trn]  
						SET  FkHoldLocationId=@FkHoldLocationId where PkId=@OutParam
				  End

				 exec usp_SalesInvoiceDetailAddUpd  @JsonData 
				 if @TranAlias='SINV'
				 Begin
					Declare @OutParamVoucher bigint =0;
					Declare @SeriesNoVoucher bigint =0;
					exec usp_VoucherAddUpd  @JsonData,@OutParamVoucher Output,@ErrMsg Output,@SeriesNoVoucher Output,@OutParam
				  End
				
				update TblCustomer_mas set FreePoint+=Trn.FreePoint 
						From tblSalesInvoice_trn Trn Inner Join TblCustomer_mas cust on Trn.FkPartyId=cust.PkCustomerId
						Where  Trn.PkId =@OutParam  And  Trn.FKSeriesId =@PKSeriesId
						And cust.PkCustomerId=@FkPartyId
				
				exec usp_SalesInvoiceShippingDetailAddUpd  @JsonData 
			
			End
		Else
			Begin
			Set @ErrMsg='Please Update Hold Location For Transfer'
			End
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
   Insert Into tblError_Log Select ISNULL(@LogID,0),ERROR_Message(),GetDate(),GetDate(),@src,@FKUserId 
  END  
  Set @Outparam=0  
End Catch
	  
end