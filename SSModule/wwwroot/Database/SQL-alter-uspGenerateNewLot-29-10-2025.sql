 
ALTER PROCEDURE [dbo].[uspGenerateNewLot]
(
	@TranFlag NChar(1),
	@StockDate date
)
AS
Begin
	Declare @strQry NVarchar(Max)
	Declare @MissingLots NVarchar(Max)
	Declare @PriceDiffID bigint
	Declare @FkProductId bigint
	Declare @FKLocationID bigint	
	Declare @FKLotID bigint
	Declare @BranchNo int
	Declare @BranchLotID bigint
	Declare @SKUDef NVarchar(500)
	Declare @MaxBranchLotID bigint
	Declare @NewBarcodeQuantity bigint= 0	
	Exec uspProdBarCode_Handler @NewBarcodeQuantity OUTPUT,@StockDate
	
	Select top(1)@FKLocationID=FKLocationID from #Detail
	Set @MissingLots= ''
	Set @BranchNo=1
	Select @PriceDiffID = PKReturnTypeID From tblReturnType_Mas Where ReturnTypeDesc='Price Difference'
	
	print 'AX'
	Set @BranchLotID=10000000 * @BranchNo
	Set @MaxBranchLotID=10000000 * (@BranchNo+1)
	Select @SKUDef = SysDefValue From tblSysDefaults Where SysDefKey='SKUDefinition'
	

	Update #Detail Set SKUDef=dbo.fn_GetSKUData(IsNull(IsNull(Prod.SKUDefinition, Catg.SKUDefinition), @SKUDef),
		IsNull(Dtl.Batch,''),IsNull(Dtl.Color,''),IsNull(Dtl.MRP,0),IsNull(Dtl.ExpiryDate,''),IsNull(Dtl.MfgDate,''),0,0,'')	
	From #Detail Dtl 
	Left Join tblProduct_Mas Prod On Dtl.FkProductId=Prod.PkProductId
	Left Join tblCategory_mas Catg On Catg.PkCategoryId=Prod.FKProdCatgID
	
	print 'BX'
	Create Table #ProdBarcodeExist(FkProductId bigint,PKLotID bigint)
	Insert Into #ProdBarcodeExist
	Select Dtl.FkProductId,IsNull(Dtl.FKLotID,0)
	From tblProdLot_Dtl PL
	Inner Join #Detail Dtl On Dtl.Barcode = PL.Barcode
	Left Join tblProduct_Mas Prod On Prod.PkProductId=PL.FkProductId
	Left Join tblCategory_mas Catg On Catg.PkCategoryId=Prod.FKProdCatgID
	Left Join tblProduct_Mas ProdDtl On ProdDtl.PkProductId=Dtl.FkProductId
	Where Dtl.ModeForm != 2 And isnull(Dtl.Barcode,'')<>'' And Dtl.Barcode != IsNull(ProdDtl.Barcode,'')
	And Dtl.FkProductId!=PL.FkProductId
	And IsNull(ProdDtl.Barcode,'')=''

	Union All
	Select Dtl.FkProductId,IsNull(Dtl.FKLotID,0)
	From tblProdLot_Dtl PL
	Inner Join #Detail Dtl On Dtl.Barcode = PL.Barcode
	Left Join tblProduct_Mas Prod On Prod.PkProductId=PL.FkProductId
	Left Join tblCategory_mas Catg On Catg.PkCategoryId=Prod.FKProdCatgID
	Left Join tblProduct_Mas ProdDtl On ProdDtl.PkProductId=Dtl.FkProductId
	Left Join tblSeries_Mas SrMas On SrMas.PKSeriesID=Dtl.FKSeriesId
	Where Dtl.ModeForm != 2 And Dtl.Barcode>0 And Dtl.Barcode != IsNull(ProdDtl.Barcode,0)
	And convert(Char(20),Dtl.FkProductId)+convert(Char(20),IsNUll(Dtl.FKLotID,0))!=convert(Char(20),PL.FkProductId)+convert(Char(20),PL.PKLotID)
	And ((Dtl.SKUDef like '%Lot ID%' And SrMas.DocumentType!='T' And PL.FKMfgGroupID is Not Null And Dtl.FKMfgGroupID != PL.FKMfgGroupID)
		 Or Dtl.SKUDef!=dbo.fn_GetSKUData(IsNull(IsNull(Prod.SKUDefinition, Catg.SKUDefinition), @SKUDef),
			IsNull(PL.Batch,''),IsNull(PL.Color,''),IsNull(PL.MRP,0),IsNull(PL.ExpiryDate,''),IsNull(PL.MfgDate,''),IsNull(PL.FKMfgGroupID,0),IsNull(PL.SaleRate,0),IsNull(PL.LotAlias,'')))
	And IsNull(ProdDtl.Barcode,0)=0	
	Union All
	Select Dtl.FkProductId,IsNUll(Dtl.FKLotID,0)
	From #Detail PL
	Inner Join #Detail Dtl On Dtl.Barcode = PL.Barcode
	Left Join tblProduct_Mas Prod On PL.FkProductId=Prod.PkProductId
	Left Join tblCategory_mas Catg On Catg.PkCategoryId=Prod.FKProdCatgID
	Left Join tblProduct_Mas ProdDtl On ProdDtl.PkProductId=Dtl.FkProductId
	Left Join tblSeries_Mas SrMas On SrMas.PKSeriesID=Dtl.FKSeriesId
	Where Dtl.ModeForm != 2 And Dtl.Barcode>0 And Dtl.Barcode != IsNull(ProdDtl.Barcode,0)
	And convert(Char(20),Dtl.FkProductId)+convert(Char(20),IsNUll(Dtl.SrNo,0))!=convert(Char(20),PL.FkProductId)+convert(Char(20),IsNull(PL.SrNo,0))
	And ((Dtl.SKUDef like '%Lot ID%' And SrMas.DocumentType!='T' And PL.FKMfgGroupID is Not Null And Dtl.FKMfgGroupID != PL.FKMfgGroupID)
		 Or Dtl.SKUDef!=PL.SKUDef)
	And IsNull(ProdDtl.Barcode,0)=0

	Union All

	Select Dtl.FkProductId,IsNull(Dtl.FKLotID,0)
	From tblProdLot_Dtl PL
	Inner Join #Detail Dtl On Dtl.Barcode = PL.Barcode
	Left Join tblProduct_Mas Prod On Prod.PkProductId=PL.FkProductId
	Left Join tblCategory_mas Catg On Catg.PkCategoryId=Prod.FKProdCatgID
	Left Join tblProduct_Mas ProdDtl On ProdDtl.PkProductId=Dtl.FkProductId
	Left Join tblSeries_Mas SrMas On SrMas.PKSeriesID=Dtl.FKSeriesId
	Where Dtl.ModeForm != 2 And Dtl.Barcode>0 And Dtl.Barcode != IsNull(ProdDtl.Barcode,0)
	And convert(Char(20),Dtl.FkProductId)+convert(Char(20),IsNUll(Dtl.FKLotID,0))!=convert(Char(20),PL.FkProductId)+convert(Char(20),PL.PKLotID)
	And ((Dtl.SKUDef like '%Lot ID%' )
		 Or Dtl.SKUDef!=dbo.fn_GetSKUData(IsNull(IsNull(Prod.SKUDefinition, Catg.SKUDefinition), @SKUDef),
			IsNull(PL.Batch,''),IsNull(PL.Color,''),IsNull(PL.MRP,0),IsNull(PL.ExpiryDate,''),IsNull(PL.MfgDate,''),IsNull(PL.FKMfgGroupID,0),IsNull(PL.SaleRate,0),IsNull(PL.LotAlias,'')))
	And IsNull(ProdDtl.Barcode,0)>0 And PL.FkProductId!=Dtl.FkProductId

	Union All

	Select Dtl.FkProductId,IsNUll(Dtl.FKLotID,0)
	From #Detail PL
	Inner Join #Detail Dtl On Dtl.Barcode = PL.Barcode
	Left Join tblProduct_Mas Prod On PL.FkProductId=Prod.PkProductId
	Left Join tblCategory_mas Catg On Catg.PkCategoryId=Prod.FKProdCatgID
	Left Join tblProduct_Mas ProdDtl On ProdDtl.PkProductId=Dtl.FkProductId
	Left Join tblSeries_Mas SrMas On SrMas.PKSeriesID=Dtl.FKSeriesId
	Where Dtl.ModeForm != 2 And Dtl.Barcode>0 And Dtl.Barcode != IsNull(ProdDtl.Barcode,0)
	And convert(Char(20),Dtl.FkProductId)+convert(Char(20),IsNUll(Dtl.SrNo,0))!=convert(Char(20),PL.FkProductId)+convert(Char(20),IsNull(PL.SrNo,0))
	And ((Dtl.SKUDef like '%Lot ID%' )
		 Or Dtl.SKUDef!=PL.SKUDef)
	And IsNull(ProdDtl.Barcode,0)>0 And PL.FkProductId!=Dtl.FkProductId
	
	
	print 'CX'
	Update #Detail Set Barcode=0
	From #ProdBarcodeExist PL
	Where #Detail.FkProductId=PL.FkProductId And IsNull(#Detail.FKLotID,0)=PL.PKLotID
	
	print 'DX'
	Exec uspGenerateBarcode
	
	print 'EX'
	Update #Detail Set LotModeForm=-1
	Where IsNull(#Detail.FKLotID,0)>0 And LotModeForm=0
  
	Update #Detail Set LotModeForm=0,FKLotID=IsNull((Select Max(PKLotID) From tblProdLot_Dtl PL Where PL.FkProductId=#Detail.FkProductId And PKLotID>@BranchLotID And PKLotID<@MaxBranchLotID),@BranchLotID)
	Where IsNull(#Detail.FKLotID,0)=0 And LotModeForm!=2  And ReturnTypeID<>@PriceDiffID
  
	IF OBJECT_ID('tempdb..#ProdLot') IS NOT NULL 
	DROP TABLE #ProdLot;

	print 'FX'
	Create Table #ProdLot(TranID bigint,TranFKSeriesID bigint,SrNo bigint,FkProductId bigint,FKLotID bigint,Batch Nvarchar(50),Color Nvarchar(50),SaleRate decimal(18,4),PurchaseRate decimal(18,4),MRP decimal(18,4),CostRate decimal(18,4),
						FKChallanID bigint,FKChallanSrID bigint,ReturnTypeID bigint,FKLinkedProdID bigint,LT_Extra Decimal(9,4),AddLT bit,FKMfgGroupID bigint,MfgDate DateTime,ExpiryDate DateTime,Barcode NVarchar(50) ,StockDate DateTime,
						TradeRate Decimal(18,4), DistributionRate Decimal(18,4),SuggestedRate Decimal(18,4),PurchaseRateUnit Nchar(1),MRPSaleRateUnit NChar(1),ExciseType Nchar(1),ExciseRate decimal(18,4),
						ProdConv1 decimal(9,4),ModeForm bigint,LotCount bigint, LotModeForm bigint,SKUDef NVarchar(500) ,SKUCount bigint,FKSaleTaxID bigint,FKPurchaseTaxID bigint,Remarks NVarchar(500),LotAlias NVarchar(25), MasterLotID Bigint)
	
	Insert Into #ProdLot
	Select Dtl.FkId,Dtl.FKSeriesId,Dtl.SrNo,Dtl.FkProductId,Dtl.FKLotID,Dtl.Batch,Dtl.Color,Dtl.SaleRate,
	Case When CHARINDEX(' ~Price Diffrence',Dtl.Remarks)>0 Then Lot.PurchaseRate Else Dtl.PurchaseRate End,
	Dtl.MRP,Case When CHARINDEX(' ~Price Diffrence',Dtl.Remarks)>0 Then Lot.CostRate Else Dtl.CostRate End
		  ,IsNull(Dtl.FKChallanID,0) As FKChallanID
		  ,IsNull(Dtl.FKChallanSrID,0) As FKChallanSrID
		  ,IsNull(Dtl.ReturnTypeID,0) As ReturnTypeID
		  ,IsNull(Dtl.FKLinkedProdID,0) As FKLinkedProdID,Dtl.LT_Extra,isnull(Dtl.AddLT,0),Dtl.FKMfgGroupID,Dtl.MfgDate,Dtl.ExpiryDate
		  ,Dtl.Barcode
		  ,Case When Dtl.StockDate Is Null Then @StockDate Else Dtl.StockDate End As StockDate,Dtl.TradeRate
		  ,Dtl.DistributionRate,Dtl.SuggestedRate,Dtl.PurchaseRateUnit,Dtl.MRPSaleRateUnit,Dtl.ExciseType
		  ,Case When Dtl.ExciseType='' Then Dtl.ExciseRate Else (Case When Dtl.ExciseType='%' Then (Dtl.PurchaseRate*Dtl.ExciseRate)/100 Else Case When Dtl.ExciseType='M' Then (Dtl.MRP*Dtl.ExciseRate)/100 Else Dtl.ExciseRate End End) End As ExciseRate
		  ,Dtl.ProdConv1,Dtl.LotModeForm
		  ,Case When Dtl.LotModeForm=0 Then Row_Number() Over(partition by Dtl.FkProductId,Dtl.FKLotID Order By Dtl.FkProductId) Else 0 End As LotCount		  
		  ,Dtl.ModeForm	
		  ,Dtl.SKUDef
		  ,Case When Dtl.Barcode=0 And Dtl.SKUDef not like '%Lot ID%' Then Row_Number() Over(partition by Dtl.FkProductId,Dtl.SKUDef Order By Dtl.FkProductId,Dtl.FKLotID) Else 0 End As SKUCount		  
		  ,Dtl.FKSaleTaxID
		  ,Dtl.FKPurchaseTaxID
		  ,IsNull(Stuff(IsNull(Stuff(IsNull(Stuff(Dtl.Remarks,Case When CharIndex('~PROM',Dtl.Remarks)>0 Then CharIndex('PROM',Dtl.Remarks) Else 1 End,Case When CharIndex('~PROM',Dtl.Remarks)>0 Then (CharIndex('~PROM',Dtl.Remarks)+5)-CharIndex('PROM',Dtl.Remarks) Else 0 End,''),''),
					Case When CharIndex('~DISC',Dtl.Remarks) >0 Then CharIndex('DISC',Dtl.Remarks) Else 1 End,Case When CharIndex('~DISC',Dtl.Remarks) >0 Then (CharIndex('~DISC',Dtl.Remarks)+5)-CharIndex('DISC',Dtl.Remarks) Else 0 End,''),''),
					Case When CharIndex('~INVTOPRINT',Dtl.Remarks) >0 Then CharIndex('INVTOPRINT',Dtl.Remarks) Else 1 End,Case When CharIndex('~INVTOPRINT',Dtl.Remarks) >0 Then (CharIndex('~INVTOPRINT',Dtl.Remarks)+11)-CharIndex('INVTOPRINT',Dtl.Remarks) Else 0 End,''),'')
		  ,Dtl.LotAlias
		  ,Dtl.MasterLotID
	From #Detail Dtl
	Left Join tblPurchaseInvoice_dtl PDtl on Dtl.FkProductId=PDtl.FkProductId and Dtl.FKLotID=PDtl.FKLotID and Dtl.SrNo=PDtl.SrNo
	Left Join tblProdLot_Dtl Lot on Lot.PKLotID=Dtl.FKLotID and Lot.FkProductId=Dtl.FkProductId
	Where IsNull(Dtl.FKLinkedProdID,0)<> -1 And (@TranFlag='P' Or (@TranFlag='S' And IsNull(Dtl.FKInvoiceID,0)=0 And Dtl.ReturnTypeID<>@PriceDiffID))	
	And Dtl.ModeForm!=2

----------------------------------------------------------------------------------------------------------------------------------------------
	
	IF OBJECT_ID('tempdb..#ProdQTYBarcode') IS NOT NULL 
		DROP TABLE #ProdQTYBarcode;   
	 Create Table #ProdQTYBarcode(FkProductId bigInt,FkLotID bigint,Barcode  varchar(16),TranInId bigint,TranInSeriesId bigint,TranInSrNo bigint)
	
	
	--print 'GX'
	Declare @Count bigint 
	Set @Count=(@NewBarcodeQuantity+1 )
		IF OBJECT_ID('tempdb..#TempProdLot') IS NOT NULL 
			DROP TABLE #TempProdLot;   

		SELECT * INTO #TempProdLot FROM #Detail where ModeForm!=2

	--loop Start

	 WHILE EXISTS (SELECT * FROM #TempProdLot)
     BEGIN 
				DECLARE @FkId bigint,@FKSeriesId bigint,@SrNo bigint,@Qty bigint,@FkProducId bigint,@CodingScheme varchar(20)
				declare @lotBarcode nvarchar(50);
		 
				SELECT TOP 1  @FkId =  a.FkId , @FKSeriesId =  a.FKSeriesId , @SrNo =  a.SrNo ,@Qty=(a.Qty+a.FreeQty),@FkProducId=a.FkProductId ,@CodingScheme=ISNULL(b.CodingScheme,'fixed') ,@lotBarcode=b.Barcode
				FROM  #TempProdLot a Inner join tblProduct_mas b on a.FkProductId=b.PkProductId order by a.SrNo
			 
				if @CodingScheme='fixed'
				Begin 
					Update #ProdLot Set Barcode=@lotBarcode Where SKUCount<=1 And SrNo=@SrNo and ModeForm=0  --And ISNULL(Barcode,'')=''
				End
				else if @CodingScheme='Lot'
				Begin 
						 IF OBJECT_ID('tempdb..#UniqIdDetail') IS NOT NULL 
							 Begin 
								   
								  Select top 1 @lotBarcode= Barcode from #UniqIdDetail where SrNo=@SrNo  
								--  Update #ProdLot Set Barcode=@lotBarcode Where SKUCount<=1 And SrNo=@SrNo and ModeForm=0  --And ISNULL(Barcode,'')=''
							 End
						 
						 if ISNULL(@lotBarcode,'')='' 
							Begin
								Exec uspProdBarCode_Check 'L',@StockDate ,@lotBarcode OUTPUT ,@Count OUTPUT
							End
						 Update #ProdLot Set Barcode=@lotBarcode Where SKUCount<=1 And SrNo=@SrNo and ModeForm=0  --And ISNULL(Barcode,'')='' 
				End
				else if @CodingScheme='Unique'
				Begin 
					Exec uspProdBarCode_Check 'L',@StockDate ,@lotBarcode OUTPUT ,@Count OUTPUT 
					Update #ProdLot Set Barcode=@lotBarcode Where SKUCount<=1 And SrNo=@SrNo and ModeForm=0  --And ISNULL(Barcode,'')='' 
					SET @Count=@Count+1  
					declare @LotId bigint=(Select (case when PL.ModeForm=0 then (PL.FKLotID+LotCount) else PL.FKLotID end) From #ProdLot PL Where  PL.ModeForm!=2 And PL.FKChallanID=0 And  PL.SrNo=@SrNo ) 
					
						IF OBJECT_ID('tempdb..#UniqIdDetail') IS NOT NULL 
						 Begin
							 Declare @QtyBarcode int=(Select count(*) from #UniqIdDetail where SrNo=@SrNo);

         					 Set @Qty-=@QtyBarcode;
							  Insert Into #ProdQTYBarcode(FkProductId,FkLotID,Barcode,TranInId,TranInSeriesId,TranInSrNo)
							  Select  @FkProducId,@LotId, Barcode ,@FkId,@FKSeriesId,@SrNo
							  from #UniqIdDetail where SrNo=@SrNo  
						 End
					 DECLARE @i INT = 1;
					 WHILE (@i <= @Qty) 
						BEGIN
							--select @i
							--select @LotId
								declare @PrdQtyBarcode nvarchar(50);
								Exec uspProdBarCode_Check 'P',@StockDate ,@PrdQtyBarcode OUTPUT ,@Count OUTPUT
								--(FORMAT(@StockDate,'yyMM') + REPLACE(STR(@NewBarcodeQuantity+@Count, 5), SPACE(1), '0')) 
						     Insert Into #ProdQTYBarcode(FkProductId,FkLotID,Barcode,TranInId,TranInSeriesId,TranInSrNo)values(@FkProducId,@LotId, @PrdQtyBarcode ,@FkId,@FKSeriesId,@SrNo)	 
							
							set @Count=@Count+1;
							SET @i = @i + 1;
						END;
				End 

				DELETE FROM #TempProdLot WHERE SrNo = @SrNo
     END
	--Barcode=13240600009 --13YYdd0000(@NewBarcode+@Count) --For Lot
	
	
	--TblProdQTYBarcode(FkLotID,Barcode,Flag bit) --New Table  --@NewBarcodeQuantity as Max TblProdQTYBarcode Barcode No.
	-- Forigen Key From tblProdLot_Dtl on PKLotID
	--FkLotId= PL.FKLotID+LotCount
	----Barcode=240600009 --YYdd0000(@NewBarcodeQuantity+@Count) --For Quantity
	
	--Insert Quantity Barcode in  #ProdQTYBarcode
	
 
	--loop End

----------------------------------------------------------------------------------------------------------------------------------------------


	--Case When Barcode=0 Then @NewBarcode+Row_Number() Over(Order By FkProductId) Else Barcode End As Barcode 
	
	Update #Detail Set FKLotID = PL.FKLotID+LotCount,Barcode=PL.Barcode
	From #ProdLot PL
	Where PL.ModeForm=0
	And PL.FKChallanID=0
	And #Detail.SrNo=PL.SrNo
	And #Detail.ReturnTypeID<>@PriceDiffID

	if EXISTS (SELECT * FROM #ProdQTYBarcode where TranInId>0 and TranInSeriesId>0)
	Begin
		Delete from TblProdQTYBarcode where FkProductId in (	Select FkProductId From #ProdQTYBarcode) and FkLotID in (	Select FkLotID  From #ProdQTYBarcode) and TranInId in (	Select TranInId From #ProdQTYBarcode) and TranInSeriesId in (	Select TranInSeriesId From #ProdQTYBarcode)
	 End

Delete TblProdQTYBarcode from TblProdQTYBarcode
							Inner join
					#Detail AS Trn 
					on TblProdQTYBarcode.TranInId=Trn.FkId 
				and TblProdQTYBarcode.TranInSeriesId=Trn.FKSeriesId  
				and TblProdQTYBarcode.TranInSrNo=Trn.SrNo
				Where Trn.ModeForm=2 

	Delete tblProdLot_Dtl from tblProdLot_Dtl
								 Inner join
						 #Detail AS Trn 
						 on tblProdLot_Dtl.InTrnID=Trn.FkId 
						and tblProdLot_Dtl.InTrnFKSeriesID=Trn.FKSeriesId  
						and tblProdLot_Dtl.InTrnsno=Trn.SrNo
						Where Trn.ModeForm=2 

	SET IDENTITY_INSERT tblProdLot_Dtl ON
	INSERT INTO [dbo].[tblProdLot_Dtl]
		   ([FkProductId],[PKLotID],[Batch],[Color],[SaleRate],[PurchaseRate],[MRP],
			[CostRate],[LT_Extra],[AddLT],[FKMfgGroupID],[MfgDate],[ExpiryDate],
			[Barcode],[StockDate],TradeRate,DistributionRate,SuggestedRate,
			PurchaseRateUnit,MRPSaleRateUnit,ExciseRate,ProdConv1,InTrnID,InTrnFKSeriesID,InTrnsno,FKSaleTaxID,FKPurchaseTaxID,Remarks,LotAlias,MasterLotID)
	Select FkProductId,FKLotID+LotCount,Batch,Color,SaleRate,PurchaseRate,MRP,
			CostRate,LT_Extra,isnull(AddLT,0),FKMfgGroupID,Convert(Date,MfgDate),Convert(Date,ExpiryDate)
			,Barcode
			,StockDate,TradeRate,DistributionRate,SuggestedRate,PurchaseRateUnit,MRPSaleRateUnit,
			ExciseRate,ProdConv1,TranID,TranFKSeriesID,SrNo,FKSaleTaxID,FKPurchaseTaxID,Remarks,LotAlias,MasterLotID
	From #ProdLot PL
	Where PL.ModeForm=0 And FKChallanID=0
	SET IDENTITY_INSERT tblProdLot_Dtl OFF 
	
	--Insert Into TblProdQTYBarcode
	
							
	INSERT INTO [dbo].[TblProdQTYBarcode](FkProductId,FkLotID,FKLocationId,Barcode,TranInId,TranInSeriesId,TranInSrNo)
	Select FkProductId,FkLotID,@FKLocationID,Barcode,TranInId,TranInSeriesId,TranInSrNo  From #ProdQTYBarcode PqL
	--

	print 'HX'
	Update tblProdLot_Dtl 
	Set [Batch]=PL.Batch,
		[Color]= PL.Color,
		[SaleRate]=  PL.SaleRate,
		[PurchaseRate]=  PL.PurchaseRate,
		[MRP] = PL.MRP,
		[CostRate] = PL.CostRate,
		[LT_Extra] = PL.LT_Extra,
		[AddLT] = PL.AddLT,
		[FKMfgGroupID] = PL.FKMfgGroupID,
		[MfgDate] = Convert(Date,PL.MfgDate),
		[ExpiryDate] = Convert(Date,PL.ExpiryDate),
		[TradeRate] = PL.TradeRate,
		[DistributionRate] = PL.DistributionRate,
		[SuggestedRate] = PL.SuggestedRate,
		[PurchaseRateUnit] = PL.PurchaseRateUnit,
		[MRPSaleRateUnit] = PL.MRPSaleRateUnit,
		[ExciseRate] = PL.ExciseRate,
		--[Barcode] = PL.Barcode,
		[FKSaleTaxID]=PL.FKSaleTaxID,
		[FKPurchaseTaxID]=PL.FKPurchaseTaxID,
		[Remarks]=PL.Remarks,
		[LotAlias]=PL.LotAlias
	From #ProdLot PL
	Where (PL.ModeForm=1 or PL.FKChallanID>0) And tblProdLot_Dtl.FkProductId=PL.FkProductId And tblProdLot_Dtl.PKLotID = PL.FKLotID
	And (   (tblProdLot_Dtl.InTrnID=PL.TranID And tblProdLot_Dtl.InTrnFKSeriesID=PL.TranFKSeriesID)
		 Or (tblProdLot_Dtl.InTrnID=PL.FKChallanID And tblProdLot_Dtl.InTrnFKSeriesID=PL.FKChallanSrID)
		 Or PL.TranID=-1)
		 
	print 'IX'
	Update tblProdLot_Dtl
	Set [MfgDate] = Case When IsNull(tblProdLot_Dtl.MfgDate,'')='' Then Convert(Date,PL.MfgDate) Else tblProdLot_Dtl.MfgDate End,
		[ExpiryDate] = Case When IsNull(tblProdLot_Dtl.ExpiryDate,'')='' Then Convert(Date,PL.ExpiryDate) Else tblProdLot_Dtl.ExpiryDate End
	From #ProdLot PL
	Where tblProdLot_Dtl.FkProductId=PL.FkProductId And tblProdLot_Dtl.PKLotID = PL.FKLotID
	
	print 'JX'
	Update tblProdLot_Dtl
	Set tblProdLot_Dtl.StockDate = Case When tblProdLot_Dtl.StockDate > @StockDate Then @StockDate Else tblProdLot_Dtl.StockDate End
	From #ProdLot PL
	Where (PL.ModeForm=1 or pl.FKChallanID>0) And tblProdLot_Dtl.FkProductId=PL.FkProductId And tblProdLot_Dtl.PKLotID = PL.FKLotID
	And tblProdLot_Dtl.InTrnID=PL.TranID And tblProdLot_Dtl.InTrnFKSeriesID=PL.TranFKSeriesID

END

