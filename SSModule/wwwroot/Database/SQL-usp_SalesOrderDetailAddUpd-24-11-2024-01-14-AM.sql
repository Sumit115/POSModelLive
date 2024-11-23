Alter procedure [dbo].[usp_SalesOrderDetailAddUpd]
( 
	@JsonData nvarchar(max)  --, 
	--@OutParam bigint Output,   
	--@ErrMsg nvarchar(max)=null Output,  
	--@SeriesNo Bigint=0 Output  
)
as 
begin
	Declare @PkId bigint, @src bigint = 0, @FkUserId bigint = 0,@FKSeriesId bigint=0,@TranAlias nvarchar (max),@IsUploadExcelFile bigint=0; 	 
				 	
	SELECT @PkId=PkId,@FKSeriesId=FKSeriesId,@src=src,@FkUserId=FkUserId,@IsUploadExcelFile=IsUploadExcelFile
				FROM OPENJSON(@JsonData)
				WITH ([PkId] [bigint] , [FKSeriesId] [bigint],[src] [bigint] ,[FKUserId] [bigint],[IsUploadExcelFile] [int]) as jsondata;				 
	
	SELECT @TranAlias=TranAlias from tblSeries_mas where PkSeriesId=@FKSeriesId
			
			 	 	
						 
	Create Table #Detail([FkId] [bigint],[FKSeriesId] [bigint],[SrNo] [Bigint],[ModeForm] [bigint],[FkProductId] [bigint],[FkLotId] [bigint],[Rate] [decimal](18, 2)
						,[RateUnit] [nvarchar](max) ,[Qty] [decimal](18, 2),[FreeQty] [decimal](18, 2),[SchemeDisc] [decimal](18, 2),[SchemeDiscType] [nvarchar](max) 
						,[SchemeDiscAmt] [decimal](18, 2),[TradeDisc] [decimal](18, 2),[TradeDiscType] [nvarchar](max) ,[TradeDiscAmt] [decimal](18, 2),[LotDisc] [decimal](18, 2)
						,[GrossAmt] [decimal](18, 2),[TaxableAmt] [decimal](18, 2),[ICRate] [decimal](18, 2),[ICAmt] [decimal](18, 2),[SCRate] [decimal](18, 2),[SCAmt] [decimal](18, 2)
						,[NetAmt] [decimal](18, 2),[Remark] [nvarchar](max) ,[Src] [bigint],[FKUserId] [bigint],[Batch] [nvarchar](500) NULL
						,[Color] [nvarchar](500) NULL,[MfgDate] [datetime] NULL,[ExpiryDate] [datetime] NULL,[MRP] [decimal](18, 2)  NULL,[SaleRate] [decimal](18, 2) NULL
						,[LinkSrNo] [bigint] NULL,[PromotionType] [nvarchar](10))
				
	print 'A'
	Insert into #Detail(FkId,FKSeriesId,ModeForm,SrNo,FkProductId,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
						,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,src
						,FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,LinkSrNo,PromotionType)
	SELECT @PkId,@FKSeriesId,ModeForm,SrNo,FkProductId,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
			,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,@src
			,@FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,LinkSrNo,PromotionType
	FROM OPENJSON(@JsonData, '$.TranDetails')
	WITH ([PkId] [bigint] ,[FkId] [bigint],[FKSeriesId] [bigint],[SrNo] [bigint],[ModeForm] [bigint],
	[FkProductId] [bigint],[FkLotId] [bigint],[Rate] [decimal](18, 2),[RateUnit] [nvarchar](max) ,
	[Qty] [decimal](18, 2),[FreeQty] [decimal](18, 2),[SchemeDisc] [decimal](18, 2),
	[SchemeDiscType] [nvarchar](max) ,[SchemeDiscAmt] [decimal](18, 2),[TradeDisc] [decimal](18, 2),
	[TradeDiscType] [nvarchar](max) ,[TradeDiscAmt] [decimal](18, 2),[LotDisc] [decimal](18, 2),
	[GrossAmt] [decimal](18, 2),[TaxableAmt] [decimal](18, 2),[ICRate] [decimal](18, 2),[ICAmt] [decimal](18, 2),[SCRate] [decimal](18, 2),
	[SCAmt] [decimal](18, 2),[NetAmt] [decimal](18, 2),[Remark] [nvarchar](max) ,[Src] [bigint],[FKUserId] [bigint],[Batch] [nvarchar](500) ,
	[Color] [nvarchar](500) ,[MfgDate] [datetime] ,[ExpiryDate] [datetime] ,[MRP] [decimal](18, 2)  ,[SaleRate] [decimal](18, 2) 
	,[LinkSrNo] [bigint],[PromotionType] [nvarchar] (10)
	) as jsondata; 
										
	--Exec uspCheckExpDate @ErrMsg Output
	------------Generate New Lot---------------------
	--exec uspGenerateNewLot 'P',@EntryDate
	--exec usp_ProductLot  @JsonData 
	--Select * from #Detail
	if @IsUploadExcelFile=1
	Begin
		Delete tblSalesOrder_dtl Where FkId=@PkId and FKSeriesId=@FKSeriesId
	End

	Insert into tblSalesOrder_dtl(FkId,FKSeriesId,SrNo,FkProductId,FkLotId,FKLocationID,Rate,RateUnit,Qty,FreeQty,DueQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
										,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark
										,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,FKUserId,ModifiedDate,FKCreatedByID,CreationDate
										,LinkSrNo,PromotionType)
	SELECT @PkId,@FKSeriesId,SrNo,FkProductId,FkLotId,11,Rate,RateUnit,Qty,FreeQty,(Qty+FreeQty),SchemeDisc,SchemeDiscType,SchemeDiscAmt
					,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark
					,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate
					,@FKUserId,Getdate(),@FKUserId,Getdate(),LinkSrNo,PromotionType
	FROM #Detail where ModeForm=0 
	print 'ccc'
	UPDATE [dbo].[tblSalesOrder_dtl]  
			SET  FkLotId=Dtl.FkLotId
			,Rate=Dtl.Rate
			,RateUnit=Dtl.RateUnit
			,Qty=Dtl.Qty
			,FreeQty=Dtl.FreeQty
			,SchemeDisc=Dtl.SchemeDisc
			,SchemeDiscType=Dtl.SchemeDiscType
			,SchemeDiscAmt=Dtl.SchemeDiscAmt
			,TradeDisc=Dtl.TradeDisc
			,TradeDiscType=Dtl.TradeDiscType
			,TradeDiscAmt=Dtl.TradeDiscAmt
			,LotDisc=Dtl.LotDisc
			,GrossAmt=Dtl.GrossAmt
			,TaxableAmt=Dtl.TaxableAmt
			,ICRate=Dtl.ICRate
			,ICAmt=Dtl.ICAmt
			,SCRate=Dtl.SCRate
			,SCAmt=Dtl.SCAmt
			,NetAmt=Dtl.NetAmt
			,Remark=Dtl.Remark
			,Batch=Dtl.Batch
			,Color=Dtl.Color
			,MfgDate=Dtl.MfgDate
			,ExpiryDate=Dtl.ExpiryDate
			,MRP=Dtl.MRP
			,SaleRate=Dtl.SaleRate
			,LinkSrNo=Dtl.LinkSrNo
			,PromotionType=Dtl.PromotionType
	FROM  #Detail AS Dtl  
	Where tblSalesOrder_dtl.FKID = Dtl.FkId
	And tblSalesOrder_dtl.FKSeriesID = Dtl.FKSeriesId
	And tblSalesOrder_dtl.SrNo = Dtl.SrNo And Dtl.ModeForm = 1
		
						
	Delete From tblSalesOrder_dtl
			Where tblSalesOrder_dtl.FKID = @PkId And FKSeriesID=@FKSeriesID
			And tblSalesOrder_dtl.SrNo In (Select Dtl.SrNo FROM #Detail As Dtl Where Dtl.ModeForm = 2)
 
	
	--exec usp_ProductStock  @JsonData 
	--Exec uspUpdateStockPurchase  @EntryDate
	  
end