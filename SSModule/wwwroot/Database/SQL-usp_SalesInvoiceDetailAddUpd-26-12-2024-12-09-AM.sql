ALTER procedure [dbo].[usp_SalesInvoiceDetailAddUpd]
( 
	@JsonData nvarchar(max)  --, 
	--@OutParam bigint Output,   
	--@ErrMsg nvarchar(max)=null Output,  
	--@SeriesNo Bigint=0 Output  
)
as 
begin
	Declare @PkId bigint, @src bigint = 0, @FKUserId bigint = 0,@FKSeriesId bigint=0,@FKLocationID bigint=0,@TranAlias nvarchar (max); 	 
		Declare @FKOrderID bigint,@FKOrderSrID bigint;		 	
	SELECT @PkId=PkId,@FKSeriesId=FKSeriesId,@src=src,@FKUserId=FKUserId,@FKLocationID=FKLocationID
			,@FKOrderID=FKOrderID,@FKOrderSrID=FKOrderSrID
				FROM OPENJSON(@JsonData)
				WITH ([PkId] [bigint] , [FKSeriesId] [bigint],[src] [bigint] ,[FKUserId] [bigint],[FKLocationID] [bigint],[FKOrderID] [bigint],[FKOrderSrID] [bigint]) as jsondata;				 
	
	SELECT @TranAlias=TranAlias from tblSeries_mas where PkSeriesId=@FKSeriesId
			
	
			 	 	
						 
	Create Table #Detail([FkId] [bigint],[FKSeriesId] [bigint],[SrNo] [Bigint],[ModeForm] [bigint],[FkProductId] [bigint],[FKLocationID] [bigint],[FkLotId] [bigint],[Rate] [decimal](18, 2)
						,[RateUnit] [nvarchar](max) ,[Qty] [decimal](18, 2),[FreeQty] [decimal](18, 2),[SchemeDisc] [decimal](18, 2),[SchemeDiscType] [nvarchar](max) 
						,[SchemeDiscAmt] [decimal](18, 2),[TradeDisc] [decimal](18, 2),[TradeDiscType] [nvarchar](max) ,[TradeDiscAmt] [decimal](18, 2),[LotDisc] [decimal](18, 2)
						,[GrossAmt] [decimal](18, 2),[TaxableAmt] [decimal](18, 2),[ICRate] [decimal](18, 2),[ICAmt] [decimal](18, 2),[SCRate] [decimal](18, 2),[SCAmt] [decimal](18, 2)
						,[NetAmt] [decimal](18, 2),[Remark] [nvarchar](max) ,[Src] [bigint],[FKUserId] [bigint],[Batch] [nvarchar](500) NULL
						,[Color] [nvarchar](500) NULL,[MfgDate] [datetime] NULL,[ExpiryDate] [datetime] NULL,[MRP] [decimal](18, 2)  NULL,[SaleRate] [decimal](18, 2) NULL,[Barcode] [nvarchar](max),
						[FKOrderID] [bigint] NULL,[OrderSrNo] [bigint] NULL,[FKOrderSrID] [bigint] NULL,[LinkSrNo] [bigint] NULL,[PromotionType] [nvarchar](10))
				
	print 'A'
	Insert into #Detail(FkId,FKSeriesId,ModeForm,SrNo,FkProductId,FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
						,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,src
						,FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,Barcode,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType)
	SELECT @PkId,@FKSeriesId,ModeForm,SrNo,FkProductId,@FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
			,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,@src
			,@FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,Barcode,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType
	FROM OPENJSON(@JsonData, '$.TranDetails')
	WITH ([PkId] [bigint] ,[FkId] [bigint],[FKSeriesId] [bigint],[SrNo] [bigint],[ModeForm] [bigint],
	[FkProductId] [bigint],[FkLotId] [bigint],[Rate] [decimal](18, 2),[RateUnit] [nvarchar](max) ,
	[Qty] [decimal](18, 2),[FreeQty] [decimal](18, 2),[SchemeDisc] [decimal](18, 2),
	[SchemeDiscType] [nvarchar](max) ,[SchemeDiscAmt] [decimal](18, 2),[TradeDisc] [decimal](18, 2),
	[TradeDiscType] [nvarchar](max) ,[TradeDiscAmt] [decimal](18, 2),[LotDisc] [decimal](18, 2),
	[GrossAmt] [decimal](18, 2),[TaxableAmt] [decimal](18, 2),[ICRate] [decimal](18, 2),[ICAmt] [decimal](18, 2),[SCRate] [decimal](18, 2),
	[SCAmt] [decimal](18, 2),[NetAmt] [decimal](18, 2),[Remark] [nvarchar](max) ,[Src] [bigint],[FKUserId] [bigint],[Batch] [nvarchar](500) ,
	[Color] [nvarchar](500) ,[MfgDate] [datetime] ,[ExpiryDate] [datetime] ,[MRP] [decimal](18, 2)  ,[SaleRate] [decimal](18, 2) ,[Barcode] [nvarchar](max),
	[FKOrderID] [bigint],[OrderSrNo] [bigint],[FKOrderSrID] [bigint],[LinkSrNo] [bigint] ,[PromotionType] [nvarchar] (10)
	) as jsondata; 
										
	--Exec uspCheckExpDate @ErrMsg Output
	------------Generate New Lot---------------------
	--exec uspGenerateNewLot 'P',@EntryDate
	--exec usp_ProductLot  @JsonData 
	--Select * from #Detail

	IF OBJECT_ID('tempdb..#UniqIdDetail') IS NOT NULL DROP TABLE #UniqIdDetail;   	 	 	
						 
	Create Table #UniqIdDetail([SrNo] [Bigint],[Barcode] [nvarchar](max))
				
	print 'A'
	Insert into #UniqIdDetail(SrNo,Barcode)
	SELECT SrNo,Barcode
	FROM OPENJSON(@JsonData, '$.UniqIdDetails')
	WITH ([SrNo] [bigint],[Barcode] [nvarchar](max)
	) as jsondata; 

	Insert into tblSalesInvoice_dtl(FkId,FKSeriesId,SrNo,FkProductId,FkLotId,FKLocationID,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
										,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark
										,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,FKUserId,ModifiedDate,FKCreatedByID,CreationDate,Barcode
										,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,DueQty)
	SELECT @PkId,@FKSeriesId,SrNo,FkProductId,FkLotId,@FKLocationID,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
					,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark
					,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate
					,@FKUserId,Getdate(),@FKUserId,Getdate()
					,(select STUFF((SELECT ',' + t1.Barcode from #UniqIdDetail t1 where t1.SrNo=SrNo FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)') ,1,1,'') ) as Barcode
					,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,(Qty+FreeQty)
	FROM #Detail where ModeForm=0 
	print 'ccc'
	UPDATE [dbo].[tblSalesInvoice_dtl]  
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
			,Barcode=(select STUFF((SELECT ',' + t1.Barcode from #UniqIdDetail t1 where t1.SrNo=Dtl.SrNo FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)') ,1,1,'') )
			,LinkSrNo=Dtl.LinkSrNo
			,PromotionType=Dtl.PromotionType,
			DueQty=(Dtl.Qty+Dtl.FreeQty)
	FROM  #Detail AS Dtl  
	Where tblSalesInvoice_dtl.FKID = Dtl.FkId
	And tblSalesInvoice_dtl.FKSeriesID = Dtl.FKSeriesId
	And tblSalesInvoice_dtl.SrNo = Dtl.SrNo And Dtl.ModeForm = 1
		
						
	Delete From tblSalesInvoice_dtl
			Where tblSalesInvoice_dtl.FKID = @PkId And FKSeriesID=@FKSeriesID
			And tblSalesInvoice_dtl.SrNo In (Select Dtl.SrNo FROM #Detail As Dtl Where Dtl.ModeForm = 2)
 
			if @TranAlias!='LINV'
				 Begin
					exec usp_StockOut
				 End
		
			 exec usp_ProductStock  'S'  
			
			--Exec uspUpdateStockPurchase  @EntryDate
	  
			if @FKOrderID>0 and @FKOrderSrID>0
				Begin 
						UPDATE [dbo].[tblSalesOrder_dtl]  
						SET  DueQty=(ISNULL(a.Qty,0)+ISNULL(a.FreeQty,0))-(ISNULL(Dtl.Qty,0)+ISNULL(Dtl.FreeQty,0))
						FROM  tblSalesOrder_dtl a
						left outer join
						(Select FkProductId,OrderSrNo,SUM(Qty) as Qty,Sum(FreeQty) as FreeQty from tblSalesInvoice_dtl where FKOrderID=@FKOrderID and FKOrderSrID=@FKOrderSrID
						group by FkProductId,OrderSrNo) AS Dtl  on dtl.FkProductId=a.FkProductId and Dtl.OrderSrNo=a.SrNo
						Where  a.FkId = @FKOrderID  And a.FKSeriesId = @FKOrderSrID 

						if Exists (Select * from tblSalesOrder_dtl where  FkId = @FKOrderID  And  FKSeriesId = @FKOrderSrID and DueQty>0)
							Begin
								update tblSalesOrder_trn set TrnStatus='P' where PkId=@FKOrderID  And  FKSeriesId = @FKOrderSrID
							End
						Else 
							Begin
								update tblSalesOrder_trn set TrnStatus='I' where PkId=@FKOrderID  And  FKSeriesId = @FKOrderSrID
							End

				End
		
			if @TranAlias ='LINV'
				 Begin
					  declare @FkHoldLocationId bigint=(Select FkHoldLocationId from tblSalesInvoice_trn where PkId=@PkId);
					  Update #Detail set FKLocationID=@FkHoldLocationId
					  if @FKOrderID>0 and @FKOrderSrID>0
						Begin
							Delete From #Detail where ModeForm=2 
						End
					  exec usp_ProductStock  'P' 
				 End
end