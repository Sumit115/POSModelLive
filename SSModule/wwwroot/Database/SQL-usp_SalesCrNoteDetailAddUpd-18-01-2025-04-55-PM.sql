ALTER procedure [dbo].[usp_SalesCrNoteDetailAddUpd]
( 
	@JsonData nvarchar(max)  --, 
	--@OutParam bigint Output,   
	--@ErrMsg nvarchar(max)=null Output,  
	--@SeriesNo Bigint=0 Output  
)
as 
begin
	Declare @PkId bigint, @src bigint = 0, @FkUserId bigint = 0,@FKSeriesId bigint=0,@TranAlias nvarchar (max); 	 
				 	
	SELECT @PkId=PkId,@FKSeriesId=FKSeriesId,@src=src,@FkUserId=FkUserId
				FROM OPENJSON(@JsonData)
				WITH ([PkId] [bigint] , [FKSeriesId] [bigint],[src] [bigint] ,[FKUserId] [bigint]) as jsondata;				 
	
	SELECT @TranAlias=TranAlias from tblSeries_mas where PkSeriesId=@FKSeriesId
			
			 	 	
						 
	Create Table #Detail([FkId] [bigint],[FKSeriesId] [bigint],[SrNo] [Bigint],[ModeForm] [bigint],[FkProductId] [bigint],[FkLotId] [bigint],[FKLocationID] [bigint],[Rate] [decimal](18, 2)
						,[RateUnit] [nvarchar](max) ,[Qty] [decimal](18, 2),[FreeQty] [decimal](18, 2),[SchemeDisc] [decimal](18, 2),[SchemeDiscType] [nvarchar](max) 
						,[SchemeDiscAmt] [decimal](18, 2),[TradeDisc] [decimal](18, 2),[TradeDiscType] [nvarchar](max) ,[TradeDiscAmt] [decimal](18, 2),[LotDisc] [decimal](18, 2)
						,[GrossAmt] [decimal](18, 2),[ICRate] [decimal](18, 2),[ICAmt] [decimal](18, 2),[SCRate] [decimal](18, 2),[SCAmt] [decimal](18, 2)
						,[NetAmt] [decimal](18, 2),[Remark] [nvarchar](max) ,[Src] [bigint],[FKUserId] [bigint],[Batch] [nvarchar](500) NULL
						,[Color] [nvarchar](500) NULL,[ExpiryDate] [datetime] NULL,[MRP] [decimal](18, 2)  NULL,[SaleRate] [decimal](18, 2) NULL,[FKInvoiceID] [bigint],[InvoiceSrNo] [bigint],[FKInvoiceSrID] [bigint])
				
	print 'A'
	Insert into #Detail(FkId,FKSeriesId,ModeForm,SrNo,FkProductId,FkLotId,FKLocationID,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
						,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,src
						,FKUserId,Batch,Color,ExpiryDate,MRP,SaleRate,FKInvoiceID,InvoiceSrNo,FKInvoiceSrID)
	SELECT @PkId,@FKSeriesId,ModeForm,SrNo,FkProductId,FkLotId,FKLocationID,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
			,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,@src
			,@FKUserId,Batch,Color,ExpiryDate,MRP,SaleRate,FKInvoiceID,InvoiceSrNo,FKInvoiceSrID
	FROM OPENJSON(@JsonData, '$.TranDetails')
	WITH ([PkId] [bigint] ,[FkId] [bigint],[FKSeriesId] [bigint],[SrNo] [bigint],[ModeForm] [bigint],
	[FkProductId] [bigint],[FkLotId] [bigint],[FKLocationID] [bigint],[Rate] [decimal](18, 2),[RateUnit] [nvarchar](max) ,
	[Qty] [decimal](18, 2),[FreeQty] [decimal](18, 2),[SchemeDisc] [decimal](18, 2),
	[SchemeDiscType] [nvarchar](max) ,[SchemeDiscAmt] [decimal](18, 2),[TradeDisc] [decimal](18, 2),
	[TradeDiscType] [nvarchar](max) ,[TradeDiscAmt] [decimal](18, 2),[LotDisc] [decimal](18, 2),
	[GrossAmt] [decimal](18, 2),[ICRate] [decimal](18, 2),[ICAmt] [decimal](18, 2),[SCRate] [decimal](18, 2),
	[SCAmt] [decimal](18, 2),[NetAmt] [decimal](18, 2),[Remark] [nvarchar](max) ,[Src] [bigint],[FKUserId] [bigint],[Batch] [nvarchar](500) ,
	[Color] [nvarchar](500)  ,[ExpiryDate] [datetime] ,[MRP] [decimal](18, 2)  ,[SaleRate] [decimal](18, 2) ,[FKInvoiceID] [bigint],[InvoiceSrNo] [bigint],[FKInvoiceSrID] [bigint]
	) as jsondata; 
										
	--Exec uspCheckExpDate @ErrMsg Output
	------------Generate New Lot---------------------
	--exec uspGenerateNewLot 'P',@EntryDate
	--exec usp_ProductLot  @JsonData 
	--Select * from #Detail


	Insert into tblSalesCrNote_dtl(FkId,FKSeriesId,SrNo,FkProductId,FkLotId,FKLocationID,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
										,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark
										,Batch,Color,ExpiryDate,MRP,SaleRate,FKInvoiceID,InvoiceSrNo,FKInvoiceSrID,ModifiedDate,FKCreatedByID,CreationDate,FkUserId)
	SELECT @PkId,@FKSeriesId,SrNo,1,(Select top(1)PkLotId from tblProdLot_dtl),FKLocationID,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
					,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark
					,Batch,Color,ExpiryDate,MRP,SaleRate,FKInvoiceID,InvoiceSrNo,FKInvoiceSrID
					,getdate(),FkUserId,getdate(),FkUserId
	FROM #Detail where ModeForm=0 
	print 'ccc'
	UPDATE [dbo].[tblSalesCrNote_dtl]  
			SET   FkLotId=Dtl.FkLotId
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
			,ICRate=Dtl.ICRate
			,ICAmt=Dtl.ICAmt
			,SCRate=Dtl.SCRate
			,SCAmt=Dtl.SCAmt
			,NetAmt=Dtl.NetAmt
			,Remark=Dtl.Remark
			,Batch=Dtl.Batch
			,Color=Dtl.Color
			,ExpiryDate=Dtl.ExpiryDate
			,MRP=Dtl.MRP
			,SaleRate=Dtl.SaleRate
			,FKInvoiceID=Dtl.FKInvoiceID
			,InvoiceSrNo=Dtl.InvoiceSrNo
			,FKInvoiceSrID=Dtl.FKInvoiceSrID
			 
	FROM  #Detail AS Dtl  
	Where tblSalesCrNote_dtl.FKID = Dtl.FkId
	And tblSalesCrNote_dtl.FKSeriesID = Dtl.FKSeriesId
	And tblSalesCrNote_dtl.SrNo = Dtl.SrNo And Dtl.ModeForm = 1
		
						
	Delete From tblSalesCrNote_dtl
			Where tblSalesCrNote_dtl.FKID = @PkId And FKSeriesID=@FKSeriesID
			And tblSalesCrNote_dtl.SrNo In (Select Dtl.SrNo FROM #Detail As Dtl Where Dtl.ModeForm = 2)
 
	
	--exec usp_ProductStock  @JsonData 
	--Exec uspUpdateStockPurchase  @EntryDate
	  
end
