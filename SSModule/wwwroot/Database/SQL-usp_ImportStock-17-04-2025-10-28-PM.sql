 
 ALTER procedure [dbo].[usp_ImportStock]
( 
	@JsonData nvarchar(max)  , 
	@ErrMsg nvarchar(max)=null Output   
)
as 
begin	
	 
	ADDRETRY:  
  Begin Try  
   Begin Tran  
   --Exec @rc = sp_getapplock @Resource='usp_SalesCrNoteAddUpd', @LockMode='Exclusive', @LockOwner='Transaction', @LockTimeout=12000  
   --    if @rc >= 0 
		 --  BEGIN
			
			IF OBJECT_ID('tempdb..#Detail') IS NOT NULL 
		DROP TABLE #Detail;   

	IF OBJECT_ID('tempdb..#ImportExcel') IS NOT NULL 
		DROP TABLE #ImportExcel;   
		
 	SELECT 0 as FkProductId,0 as FKProdCatgId,Qty,Batch,Color,MRP,TRIM(Product) as Product,TRIM(SubCategoryName) as SubCategoryName,Barcode
	INTO #ImportExcel
	FROM OPENJSON(@JsonData)
	WITH ([Qty] [decimal](18, 2),[Batch] [nvarchar](500) ,[Color] [nvarchar](500) ,
	[MRP] [decimal](18, 2) ,[Product] [nvarchar](100) ,[SubCategoryName] [nvarchar](100),Barcode [nvarchar](100)
	) as jsondata; 


	update #ImportExcel  set FkProductId=prd.PkProductId  
	from #ImportExcel dtl Inner join  tblProduct_mas prd on dtl.Product =prd.Product

	update #ImportExcel  set FKProdCatgId=subcate.PkCategoryId  
	from #ImportExcel dtl Inner join  tblCategory_mas subcate on dtl.SubCategoryName =subcate.CategoryName



	--declare @NotFound nvarchar(max)=  STUFF( (SELECT DISTINCT ','+Product FROM #ImportExcel  where FkProductId=0 FOR XML PATH('')), 1, 1, '') 
	 
	 --Select  * from #ImportExcel 
	--Select @NotFound
	if exists (Select * from TblProdQTYBarcode where Barcode in (Select Barcode from #ImportExcel))
	Begin
			  set @ErrMsg= 'Error : Barcode already exists : '+ STUFF( (SELECT DISTINCT ','+Barcode FROM  TblProdQTYBarcode where Barcode in (Select Barcode from #ImportExcel) FOR XML PATH('')), 1, 1, '') 
			 --Select @ErrMsg
	End
	Else
		 Begin
		if exists (Select * from #ImportExcel where FKProdCatgId=0)
				Begin
					  set @ErrMsg= 'Error : Section Not Found : '+ STUFF( (SELECT DISTINCT ','+SubCategoryName FROM  #ImportExcel where FKProdCatgId=0 FOR XML PATH('')), 1, 1, '') 
	 			End
		Else
				Begin
						declare @CodingScheme nvarchar(50)=(Isnull((Select SysDefValue from tblSysDefaults where SysDefKey  ='CodingScheme'),'Unique'));
						--Insert New Product
						Insert into tblProduct_mas(PkProductId,Alias,Product,Brand,Strength,Unit1,ProdConv1,Unit2,SellLoose,ProdConv2,Unit3,CaseLot,NameToDisplay,NameToPrint,IsExpiryApplied,IsMfgDateApplied
						,IsUniqueIDApplied,IsColorApplied,IsBarCodeApplied,IsBatchApplied,IsVirtual,Description,FKProdCatgId,FKMktGroupID,FKMfgGroupId,ShelfID,MinStock,MaxStock,MRP,SaleRate,TradeRate
						,DistributionRate,SuggestedRate,MRPSaleRateUnit,PurchaseRate,CostRate,PurchaseRateUnit,AddLT,Barcode,Weight,Height,Width,Length,WeightUnit,HeightUnit,Status,DiscDate,IncreaseSaleRateBy
						,KeepStock,AllowDecimal,BestBefore,BestBeforeUnit,SKUDefinition,TradeDisc,MinDays,MaxDays,FKGenericID1,FKGenericID2,BoxSize,FkUnitId,Genration,CodingScheme,FkBrandId,Image,HSNCode
						,FKTaxID,ModifiedDate,FKCreatedByID,CreationDate,FKUserID)
						SELECT ((Isnull((SELECT MAX(PkProductId) FROM tblProduct_mas),0))+ROW_NUMBER() over(order by FKProdCatgId)),null,Product,null,null,'',0,'',null,0,'','',Product,Product,null,null,null,null,null,null,0,null
						,FKProdCatgId,null,null,'',null,null,max(MRP),max(MRP),max(MRP),max(MRP),max(MRP),'',max(MRP),max(MRP)
						,'',null,null,null,null,null,null,null,null,'A',null,null,1,0,null,null,null,0,null,null,null,null,0,1,'Manual',@CodingScheme,null,null,null,0,Getdate(),1,Getdate(),1
						  FROM #ImportExcel  where FkProductId=0 and FKProdCatgId>0
						  group by FkProductId,Product,FKProdCatgId
						order by FKProdCatgId

						update #ImportExcel  set FkProductId=prd.PkProductId  from #ImportExcel dtl Inner join  tblProduct_mas prd on dtl.Product =prd.Product

				  if exists (Select * from #ImportExcel where FkProductId=0)
						Begin
							  set @ErrMsg= 'Error : Artical Not Found Or Insert : '+ STUFF( (SELECT DISTINCT ','+Product FROM #ImportExcel  where FkProductId=0 FOR XML PATH('')), 1, 1, '') 
	 					End
					Else
						Begin

								IF OBJECT_ID('tempdb..#Detail') IS NOT NULL 
									DROP TABLE #Detail;   

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
									,[Product] [nvarchar](100))
	
								print 'A'
	

								Insert into #Detail(FkId,FKSeriesId,ModeForm,SrNo,FkProductId,FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
													,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remarks,src
													,FKUserId,Batch,Color,MRP,SaleRate,TradeRate,DistributionRate,ReturnTypeID,LotModeForm,Product)
								SELECT 0,0,0,ROW_NUMBER() over(order by FkProductId),FkProductId,1,0,MRP,'1',Sum(Qty),0,0,'R',0
										,0,'R',0,0,0,0,0,0,0,0,'',0
										,1,Batch,Color,MRP,MRP,MRP,MRP,2,0,Product
								FROM  #ImportExcel 
								group by FkProductId,Product,Batch,MRP,Color

								IF OBJECT_ID('tempdb..#UniqIdDetail') IS NOT NULL DROP TABLE #UniqIdDetail;   	 	 	
						 
								Create Table #UniqIdDetail([SrNo] [Bigint],[Barcode] [nvarchar](max))
				
								print 'A'
								Insert into #UniqIdDetail(SrNo,Barcode)
								SELECT dtl.SrNo,a.Barcode
								FROM #ImportExcel a 
								left outer join #Detail dtl on a.FkProductId=dtl.FkProductId and a.Product=dtl.Product
								And a.Batch=dtl.Batch And a.Color=dtl.Color And a.MRP=dtl.MRP 
								Order by dtl.SrNo

								--Select * from #Detail
								--Select * from #ImportExcel
								--Select * from #UniqIdDetail  Order by SrNo
								declare @EntryDate date=(select Getdate())
								exec uspGenerateNewLot 'P',@EntryDate;
								 exec usp_ProductStock  'O' ;
	
								declare @NotFound nvarchar(max)=  STUFF( (SELECT DISTINCT ','+Barcode FROM #ImportExcel  where Barcode not in (Select Barcode from tblProdQTYBarcode) and Barcode not in (Select Barcode from tblProdLot_dtl)   FOR XML PATH('')), 1, 1, '') 
								if ISNULL(@NotFound,'')!=''
									Begin
									 Set @ErrMsg= 'Data Upload Successfully. But Barcode Not Insert : '+@NotFound
									End
								Else
									Begin
									 Set @ErrMsg= 'Data Upload Successfully..'
									End
						ENd
				End

	 END 

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
   set @LogID=0;--uspGetIDOfSeries,,'tblSwilError_Log','PKID'  
   Insert Into tblError_Log Select ISNULL(@LogID,0),ERROR_Message(),GetDate(),GetDate(),0,1 
  END  
  --Set @Outparam=0  
 End Catch  
	  
end
