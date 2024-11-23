Alter procedure [dbo].[usp_PurchaseInvoiceDetailAddUpd]
( 
	@JsonData nvarchar(max),
	@ErrMsg NVarchar(500)=null Output
)
as 
begin
	Declare @PkId bigint, @src bigint = 0, @FkUserId bigint = 0,@FKSeriesId bigint=0,@FKLocationID bigint=0,@TranAlias nvarchar (max), @EntryDate Date; 	 
				 	
	SELECT @PkId=PkId, @FKSeriesId=FKSeriesId, @src=src, @FkUserId=FkUserId, @EntryDate=EntryDate,@FKLocationID=FKLocationID
				FROM OPENJSON(@JsonData)
				WITH ([PkId] [bigint] , [FKSeriesId] [bigint],[src] [bigint] ,[FKUserId] [bigint],[FKLocationID] [bigint],[EntryDate] [date]) as jsondata;				 
	
	SELECT @TranAlias=TranAlias from tblSeries_mas where PkSeriesId=@FKSeriesId
			

	Create Table #Detail([FkId] [bigint],[FKSeriesId] [bigint],[SrNo] [Bigint],[FkProductId] [bigint],[FKLocationID] [bigint],[FkLotId] [bigint],[TradeDiscType] [nvarchar](max)
			,[ICAmt] [decimal](18, 2),[SCRate] [decimal](18, 2),[SCAmt] [decimal](18, 2),src bigint,[FKUserId] [bigint]
						,[NetAmt] [decimal](18, 2),MRP decimal(18,4),MRPToPrint decimal(18,4),
			TradeRate decimal(18,4),DistributionRate decimal(18,4),Rate decimal(18,4),RateUnit Nchar(1),Qty decimal(18,4), TotalQty decimal(18,4),QtyUnit Nchar(1),FreeQty decimal(18,4),FreeQtyUnit Nchar(1),
			TradeDisc decimal(18,4),SchemeDisc decimal(18,4),SchemeDiscType NChAR(1),LotDisc decimal(18,4),GrossAmt decimal(18,4),TaxAmt decimal(18,4),TotalAftDisc decimal(18,4),Remarks nvarchar(500),
			TaxableAmt decimal(18,4),FKTaxID bigint,FKProdCatgID bigint,FKMktGroupID bigint,Color Nvarchar(50),Batch Nvarchar(50),ColorToPrint Nvarchar(50),BatchToPrint Nvarchar(50),FKLinkedProdID bigint,
			ProdConv1 decimal(9,4),ExpiryDate Date,AddLT bit,FKChallanID bigint,FKChallanSrID Bigint,ChallanSrNo bigint,ICRate decimal (18,4),ExciseRate decimal (18,4),ExciseType Nchar(1),Scheme Nvarchar(40),
			FutureScheme decimal(18,4),Deduction decimal(9,4),UniqueID Nvarchar(max),ModeForm int,pTotalQty decimal(18,4),ID bigint,pFKLocationID bigint,pFKLotID bigint,pQtyUnit Nchar(1),pFreeQtyUnit Nchar(1),TotalQtyUnit2 decimal(18,4),
			pTotalQtyUnit2 decimal(18,4),FKReturnID bigint,pProdConv1 decimal(9,4),TranID bigint,TranFKSeriesID bigint,LotModeForm bigint,PurchaseRate decimal(18,4),PurchaseRateUnit Nchar(1),CostRate decimal(18,4),LT_Extra Decimal(9,4),FKMfgGroupID bigint,
			MfgDate Date,Barcode bigint,SaleRate decimal(18,4),SuggestedRate decimal(18,4),MRPSaleRateUnit Nchar(1),ReturnTypeID bigint,FKInvoiceID bigint,FKInvoiceSrID bigint,TaxCalcMethod nchar(1),SKUDef NVarchar(500),
			FKSaleTaxID bigint,FKPurchaseTaxID bigint,SaleCharged decimal(18,4),SaleFree decimal(18,4),SaleSchemeQty Decimal(18,4),SaleSchemeDisc decimal(18,4),TradeDiscAmt Decimal(18,4),SchemeDiscAmt Decimal(18,4),
			pQty Decimal(18,4), pFreeQty Decimal(18,4),StockDate Date,SGSTAmt Decimal(18,4),CGSTAmt Decimal(18,4),CessAmt Decimal(18,4),LotAlias NVarchar(25),MasterLotID Bigint,LotScheme NVarchar(200),MinApplyQty Decimal(18,4),FKHSNID bigint,HSNCODE nvarchar(100),PHSNCODE nvarchar(100)
			,[LinkSrNo] [bigint] NULL,[PromotionType] [nvarchar](10))
			
	Insert into #Detail(FkId,FKSeriesId,ModeForm,SrNo,FkProductId,FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
						,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remarks,src
						,FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,TradeRate,DistributionRate,ReturnTypeID,LotModeForm
						,LinkSrNo,PromotionType)
	SELECT @PkId,@FKSeriesId,ModeForm,SrNo,FkProductId,@FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
			,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,@src
			,@FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,TradeRate,DistributionRate,2,ModeForm
			,LinkSrNo,PromotionType
	FROM OPENJSON(@JsonData, '$.TranDetails')
	WITH ([PkId] [bigint] ,[FkId] [bigint],[FKSeriesId] [bigint],[SrNo] [bigint],[ModeForm] [bigint],
	[FkProductId] [bigint],[FkLotId] [bigint],[Rate] [decimal](18, 2),[RateUnit] [nvarchar](max) ,
	[Qty] [decimal](18, 2),[FreeQty] [decimal](18, 2),[SchemeDisc] [decimal](18, 2),
	[SchemeDiscType] [nvarchar](max) ,[SchemeDiscAmt] [decimal](18, 2),[TradeDisc] [decimal](18, 2),
	[TradeDiscType] [nvarchar](max) ,[TradeDiscAmt] [decimal](18, 2),[LotDisc] [decimal](18, 2),
	[GrossAmt] [decimal](18, 2),[ICRate] [decimal](18, 2),[ICAmt] [decimal](18, 2),[SCRate] [decimal](18, 2),
	[SCAmt] [decimal](18, 2),[NetAmt] [decimal](18, 2),[Remark] [nvarchar](max) ,[Src] [bigint],[FKUserId] [bigint],[Batch] [nvarchar](500) ,
	[Color] [nvarchar](500) ,[MfgDate] [datetime] ,[ExpiryDate] [datetime] ,[MRP] [decimal](18, 2)  ,[SaleRate] [decimal](18, 2) ,[TradeRate] [decimal](18, 2) ,[DistributionRate] [decimal](18, 2)
	,[LinkSrNo] [bigint] ,[PromotionType] [nvarchar] (10)
	) as jsondata; 
					
	--Exec uspCheckExpDate @ErrMsg Output
	IF OBJECT_ID('tempdb..#UniqIdDetail') IS NOT NULL DROP TABLE #UniqIdDetail;   	 	 	
						 
	Create Table #UniqIdDetail([SrNo] [Bigint],[Barcode] [nvarchar](max))
				
	print 'A'
	Insert into #UniqIdDetail(SrNo,Barcode)
	SELECT SrNo,Barcode
	FROM OPENJSON(@JsonData, '$.UniqIdDetails')
	WITH ([SrNo] [bigint],[Barcode] [nvarchar](max)
	) as jsondata; 
	
	print 'AAA'
	
	------------Generate New Lot---------------------
	Delete From tblPurchaseInvoice_dtl
			Where tblPurchaseInvoice_dtl.FKID = @PkId And FKSeriesID=@FKSeriesID
			And tblPurchaseInvoice_dtl.SrNo In (Select Dtl.SrNo FROM #Detail As Dtl Where Dtl.ModeForm = 2)
 
	  delete tblProdStock_dtl from tblProdStock_dtl 
						 left outer join 
						 #Detail AS Trn 
						 on tblProdStock_dtl.FKProductId=Trn.FkProductId 
						and tblProdStock_dtl.FKLotID=Trn.FkLotId  
						and tblProdStock_dtl.FKLocationID=Trn.FKLocationID 
						Where Trn.ModeForm=2 

	exec uspGenerateNewLot 'P',@EntryDate
		
	print 'B'
	--Select * from #Detail

	
	print 'B'
	Insert into tblPurchaseInvoice_dtl(FkId,FKSeriesId,SrNo,FkProductId,FkLotId,FKLocationID,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
										,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark
										,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,TradeRate,DistributionRate
										,FKUserID,ModifiedDate,FKCreatedByID,CreationDate,LinkSrNo,PromotionType)
	SELECT @PkId,@FKSeriesId,SrNo,FkProductId,FkLotId,FKLocationID,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
					,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remarks
					,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,TradeRate,DistributionRate
					,@FkUserId,Getdate(),@FkUserId,Getdate(),LinkSrNo,PromotionType
	FROM #Detail where ModeForm=0 
	print 'ccc'
	UPDATE [dbo].[tblPurchaseInvoice_dtl]  
			SET Rate=Dtl.Rate
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
			,Remark=Dtl.Remarks
			,Batch=Dtl.Batch
			,Color=Dtl.Color
			,MfgDate=Dtl.MfgDate
			,ExpiryDate=Dtl.ExpiryDate
			,MRP=Dtl.MRP
			,SaleRate=Dtl.SaleRate
			,TradeRate=Dtl.TradeRate
			,DistributionRate=Dtl.DistributionRate
			,LinkSrNo=Dtl.LinkSrNo
			,PromotionType=Dtl.PromotionType

	FROM  #Detail AS Dtl  
	Where tblPurchaseInvoice_dtl.FKID = Dtl.FkId
	And tblPurchaseInvoice_dtl.FKSeriesID = Dtl.FKSeriesId
	And tblPurchaseInvoice_dtl.SrNo = Dtl.SrNo And Dtl.ModeForm = 1
		
						
	--Delete From tblPurchaseInvoice_dtl
	--		Where tblPurchaseInvoice_dtl.FKID = @PkId And FKSeriesID=@FKSeriesID
	--		And tblPurchaseInvoice_dtl.SrNo In (Select Dtl.SrNo FROM #Detail As Dtl Where Dtl.ModeForm = 2)
 
	
	exec usp_ProductStock  'P' 
	--Exec uspUpdateStockPurchase  @EntryDate
	  
end