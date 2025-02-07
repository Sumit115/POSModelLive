ALTER procedure [dbo].[usp_JobWorkDetailAddUpd]
( 
	@JsonData nvarchar(max)  --, 
	--@OutParam bigint Output,   
	--@ErrMsg nvarchar(max)=null Output,  
	--@SeriesNo Bigint=0 Output  
)
as 
begin
	Declare @PkId bigint, @src bigint = 0, @FKUserId bigint = 0,@FKSeriesId bigint=0,@FKLocationID bigint=0,@TranAlias nvarchar (max), @EntryDate Date;   
		Declare @FKOrderID bigint,@FKOrderSrID bigint;		 	
	SELECT @PkId=PkId,@FKSeriesId=FKSeriesId,@src=src,@FKUserId=FKUserId, @EntryDate=EntryDate,@FKLocationID=FKLocationID
			,@FKOrderID=FKOrderID,@FKOrderSrID=FKOrderSrID
				FROM OPENJSON(@JsonData)
				WITH ([PkId] [bigint] , [FKSeriesId] [bigint],[src] [bigint] ,[FKUserId] [bigint],[FKLocationID] [bigint],[FKOrderID] [bigint],[FKOrderSrID] [bigint],[EntryDate] [date]) as jsondata;				 
	
	SELECT @TranAlias=TranAlias from tblSeries_mas where PkSeriesId=@FKSeriesId
			 		 
	Create Table #Detail([FkId] [bigint],[FKSeriesId] [bigint],[SrNo] [Bigint],[ModeForm] [bigint],[FkProductId] [bigint],[FKLocationID] [bigint],[FkLotId] [bigint],[Rate] [decimal](18, 2)
						,[RateUnit] [nvarchar](max) ,[Qty] [decimal](18, 2),[FreeQty] [decimal](18, 2),[SchemeDisc] [decimal](18, 2),[SchemeDiscType] [nvarchar](max) 
						,[SchemeDiscAmt] [decimal](18, 2),[TradeDisc] [decimal](18, 2),[TradeDiscType] [nvarchar](max) ,[TradeDiscAmt] [decimal](18, 2),[LotDisc] [decimal](18, 2)
						,[GrossAmt] [decimal](18, 2),[TaxableAmt] [decimal](18, 2),[ICRate] [decimal](18, 2),[ICAmt] [decimal](18, 2),[SCRate] [decimal](18, 2),[SCAmt] [decimal](18, 2)
						,[NetAmt] [decimal](18, 2),[Remark] [nvarchar](max) ,[Src] [bigint],[FKUserId] [bigint],[Batch] [nvarchar](500) NULL
						,[Color] [nvarchar](500) NULL,[MfgDate] [datetime] NULL,[ExpiryDate] [datetime] NULL,[MRP] [decimal](18, 2)  NULL,[SaleRate] [decimal](18, 2) NULL,[Barcode] [nvarchar](max),
						[FKOrderID] [bigint] NULL,[OrderSrNo] [bigint] NULL,[FKOrderSrID] [bigint] NULL,[LinkSrNo] [bigint] NULL,[PromotionType] [nvarchar](10),[TranType][char](1))
		
			IF OBJECT_ID('tempdb..#UniqIdDetail') IS NOT NULL DROP TABLE #UniqIdDetail;   	 
				Create Table #UniqIdDetail([SrNo] [Bigint],[Barcode] [nvarchar](max),[TranType] [char](1))

			if @TranAlias='PJ_R' --Product To Be Received (IN)
			 Begin
					print 'A'
					Insert into #Detail(FkId,FKSeriesId,ModeForm,SrNo,FkProductId,FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
										,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,src
										,FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,Barcode,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,TranType)
					SELECT @PkId,@FKSeriesId,ModeForm,SrNo,FkProductId,@FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
							,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,@src
							,@FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,Barcode,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,TranType
					FROM OPENJSON(@JsonData, '$.TranDetails')
					WITH ([PkId] [bigint] ,[FkId] [bigint],[FKSeriesId] [bigint],[SrNo] [bigint],[ModeForm] [bigint],
					[FkProductId] [bigint],[FkLotId] [bigint],[Rate] [decimal](18, 2),[RateUnit] [nvarchar](max) ,
					[Qty] [decimal](18, 2),[FreeQty] [decimal](18, 2),[SchemeDisc] [decimal](18, 2),
					[SchemeDiscType] [nvarchar](max) ,[SchemeDiscAmt] [decimal](18, 2),[TradeDisc] [decimal](18, 2),
					[TradeDiscType] [nvarchar](max) ,[TradeDiscAmt] [decimal](18, 2),[LotDisc] [decimal](18, 2),
					[GrossAmt] [decimal](18, 2),[TaxableAmt] [decimal](18, 2),[ICRate] [decimal](18, 2),[ICAmt] [decimal](18, 2),[SCRate] [decimal](18, 2),
					[SCAmt] [decimal](18, 2),[NetAmt] [decimal](18, 2),[Remark] [nvarchar](max) ,[Src] [bigint],[FKUserId] [bigint],[Batch] [nvarchar](500) ,
					[Color] [nvarchar](500) ,[MfgDate] [datetime] ,[ExpiryDate] [datetime] ,[MRP] [decimal](18, 2)  ,[SaleRate] [decimal](18, 2) ,[Barcode] [nvarchar](max),
					[FKOrderID] [bigint],[OrderSrNo] [bigint],[FKOrderSrID] [bigint],[LinkSrNo] [bigint] ,[PromotionType] [nvarchar] (10),[TranType][char](1)
					) as jsondata; 
		 
					print 'A'
					Insert into #UniqIdDetail(SrNo,Barcode,TranType)
					SELECT SrNo,Barcode,'I'
					FROM OPENJSON(@JsonData, '$.UniqIdDetails')
					WITH ([SrNo] [bigint],[Barcode] [nvarchar](max)
					) as jsondata; 
					
					Delete From tblJobWork_dtl
					Where tblJobWork_dtl.FKID = @PkId And FKSeriesID=@FKSeriesID
					And tblJobWork_dtl.SrNo In (Select Dtl.SrNo FROM #Detail As Dtl Where Dtl.ModeForm = 2)
 
					delete tblProdStock_dtl from tblProdStock_dtl 
					 left outer join  #Detail AS Trn  on tblProdStock_dtl.FKProductId=Trn.FkProductId 
					 and tblProdStock_dtl.FKLotID=Trn.FkLotId  and tblProdStock_dtl.FKLocationID=Trn.FKLocationID 
					 Where Trn.ModeForm=2 

					exec uspGenerateNewLot 'P',@EntryDate

					--Return Detail Start
					Insert into #Detail(FkId,FKSeriesId,ModeForm,SrNo,FkProductId,FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
										,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,src
										,FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,Barcode,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,TranType)
					SELECT @PkId,@FKSeriesId,ModeForm,SrNo,FkProductId,@FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
							,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,@src
							,@FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,Barcode,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,TranType
					FROM OPENJSON(@JsonData, '$.TranReturnDetails')
					WITH ([PkId] [bigint] ,[FkId] [bigint],[FKSeriesId] [bigint],[SrNo] [bigint],[ModeForm] [bigint],
					[FkProductId] [bigint],[FkLotId] [bigint],[Rate] [decimal](18, 2),[RateUnit] [nvarchar](max) ,
					[Qty] [decimal](18, 2),[FreeQty] [decimal](18, 2),[SchemeDisc] [decimal](18, 2),
					[SchemeDiscType] [nvarchar](max) ,[SchemeDiscAmt] [decimal](18, 2),[TradeDisc] [decimal](18, 2),
					[TradeDiscType] [nvarchar](max) ,[TradeDiscAmt] [decimal](18, 2),[LotDisc] [decimal](18, 2),
					[GrossAmt] [decimal](18, 2),[TaxableAmt] [decimal](18, 2),[ICRate] [decimal](18, 2),[ICAmt] [decimal](18, 2),[SCRate] [decimal](18, 2),
					[SCAmt] [decimal](18, 2),[NetAmt] [decimal](18, 2),[Remark] [nvarchar](max) ,[Src] [bigint],[FKUserId] [bigint],[Batch] [nvarchar](500) ,
					[Color] [nvarchar](500) ,[MfgDate] [datetime] ,[ExpiryDate] [datetime] ,[MRP] [decimal](18, 2)  ,[SaleRate] [decimal](18, 2) ,[Barcode] [nvarchar](max),
					[FKOrderID] [bigint],[OrderSrNo] [bigint],[FKOrderSrID] [bigint],[LinkSrNo] [bigint] ,[PromotionType] [nvarchar] (10),[TranType][char](1)
					) as jsondata; 
		 
					print 'A'
					Insert into #UniqIdDetail(SrNo,Barcode,TranType)
					SELECT SrNo,Barcode,'O'
					FROM OPENJSON(@JsonData, '$.UniqIdReturnDetails')
					WITH ([SrNo] [bigint],[Barcode] [nvarchar](max)
					) as jsondata; 

					Delete From tblJobWork_dtl
					Where tblJobWork_dtl.FKID = @PkId And FKSeriesID=@FKSeriesID
					And tblJobWork_dtl.SrNo In (Select Dtl.SrNo FROM #Detail As Dtl Where Dtl.ModeForm = 2)
					--Return Detail End

					Insert into tblJobWork_dtl(FkId,FKSeriesId,SrNo,FkProductId,FkLotId,FKLocationID,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
						,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark
						,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,FKUserId,ModifiedDate,FKCreatedByID,CreationDate,Barcode
						,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,DueQty,TranType)
						SELECT @PkId,@FKSeriesId,SrNo,FkProductId,FkLotId,@FKLocationID,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
						,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark
						,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate
						,@FKUserId,Getdate(),@FKUserId,Getdate()
						,(select STUFF((SELECT ',' + t1.Barcode from #UniqIdDetail t1 where t1.SrNo=SrNo and t1.TranType=TranType  FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)') ,1,1,'') ) as Barcode
						,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,(Qty+FreeQty),TranType
						FROM #Detail where ModeForm=0 
					
					print 'ccc'

					UPDATE [dbo].[tblJobWork_dtl]  
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
						,Barcode=(select STUFF((SELECT ',' + t1.Barcode from #UniqIdDetail t1 where t1.SrNo=Dtl.SrNo and t1.TranType=Dtl.TranType FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)') ,1,1,'') )
						,LinkSrNo=Dtl.LinkSrNo
						,PromotionType=Dtl.PromotionType,
						DueQty=(Dtl.Qty+Dtl.FreeQty),
						TranType=Dtl.TranType
						FROM  #Detail AS Dtl  
						Where tblJobWork_dtl.FKID = Dtl.FkId
						And tblJobWork_dtl.FKSeriesID = Dtl.FKSeriesId
						And tblJobWork_dtl.SrNo = Dtl.SrNo And Dtl.ModeForm = 1
					
					Delete From #Detail Where TranType='O'
					Delete From #UniqIdDetail Where TranType='O'

					exec usp_ProductStock  'P' 
			 End
			 Else if @TranAlias='PJ_I' --Product being issued OUT
			 Begin 
					print 'R'
					Insert into #Detail(FkId,FKSeriesId,ModeForm,SrNo,FkProductId,FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
										,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,src
										,FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,Barcode,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,TranType)
					SELECT @PkId,@FKSeriesId,ModeForm,SrNo,FkProductId,@FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
							,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,@src
							,@FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,Barcode,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,TranType
					FROM OPENJSON(@JsonData, '$.TranReturnDetails')
					WITH ([PkId] [bigint] ,[FkId] [bigint],[FKSeriesId] [bigint],[SrNo] [bigint],[ModeForm] [bigint],
					[FkProductId] [bigint],[FkLotId] [bigint],[Rate] [decimal](18, 2),[RateUnit] [nvarchar](max) ,
					[Qty] [decimal](18, 2),[FreeQty] [decimal](18, 2),[SchemeDisc] [decimal](18, 2),
					[SchemeDiscType] [nvarchar](max) ,[SchemeDiscAmt] [decimal](18, 2),[TradeDisc] [decimal](18, 2),
					[TradeDiscType] [nvarchar](max) ,[TradeDiscAmt] [decimal](18, 2),[LotDisc] [decimal](18, 2),
					[GrossAmt] [decimal](18, 2),[TaxableAmt] [decimal](18, 2),[ICRate] [decimal](18, 2),[ICAmt] [decimal](18, 2),[SCRate] [decimal](18, 2),
					[SCAmt] [decimal](18, 2),[NetAmt] [decimal](18, 2),[Remark] [nvarchar](max) ,[Src] [bigint],[FKUserId] [bigint],[Batch] [nvarchar](500) ,
					[Color] [nvarchar](500) ,[MfgDate] [datetime] ,[ExpiryDate] [datetime] ,[MRP] [decimal](18, 2)  ,[SaleRate] [decimal](18, 2) ,[Barcode] [nvarchar](max),
					[FKOrderID] [bigint],[OrderSrNo] [bigint],[FKOrderSrID] [bigint],[LinkSrNo] [bigint] ,[PromotionType] [nvarchar] (10),[TranType][char](1)
					) as jsondata; 
				
					Insert into #UniqIdDetail(SrNo,Barcode)
					SELECT SrNo,Barcode
					FROM OPENJSON(@JsonData, '$.UniqIdReturnDetails')
					WITH ([SrNo] [bigint],[Barcode] [nvarchar](max)
					) as jsondata; 

					--Tran Detail Start
					Insert into #Detail(FkId,FKSeriesId,ModeForm,SrNo,FkProductId,FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
										,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,src
										,FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,Barcode,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,TranType)
					SELECT @PkId,@FKSeriesId,ModeForm,SrNo,FkProductId,@FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
							,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,@src
							,@FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,Barcode,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,TranType
					FROM OPENJSON(@JsonData, '$.TranDetails')
					WITH ([PkId] [bigint] ,[FkId] [bigint],[FKSeriesId] [bigint],[SrNo] [bigint],[ModeForm] [bigint],
					[FkProductId] [bigint],[FkLotId] [bigint],[Rate] [decimal](18, 2),[RateUnit] [nvarchar](max) ,
					[Qty] [decimal](18, 2),[FreeQty] [decimal](18, 2),[SchemeDisc] [decimal](18, 2),
					[SchemeDiscType] [nvarchar](max) ,[SchemeDiscAmt] [decimal](18, 2),[TradeDisc] [decimal](18, 2),
					[TradeDiscType] [nvarchar](max) ,[TradeDiscAmt] [decimal](18, 2),[LotDisc] [decimal](18, 2),
					[GrossAmt] [decimal](18, 2),[TaxableAmt] [decimal](18, 2),[ICRate] [decimal](18, 2),[ICAmt] [decimal](18, 2),[SCRate] [decimal](18, 2),
					[SCAmt] [decimal](18, 2),[NetAmt] [decimal](18, 2),[Remark] [nvarchar](max) ,[Src] [bigint],[FKUserId] [bigint],[Batch] [nvarchar](500) ,
					[Color] [nvarchar](500) ,[MfgDate] [datetime] ,[ExpiryDate] [datetime] ,[MRP] [decimal](18, 2)  ,[SaleRate] [decimal](18, 2) ,[Barcode] [nvarchar](max),
					[FKOrderID] [bigint],[OrderSrNo] [bigint],[FKOrderSrID] [bigint],[LinkSrNo] [bigint] ,[PromotionType] [nvarchar] (10),[TranType][char](1)
					) as jsondata; 
				
					Insert into #UniqIdDetail(SrNo,Barcode)
					SELECT SrNo,Barcode
					FROM OPENJSON(@JsonData, '$.UniqIdDetails')
					WITH ([SrNo] [bigint],[Barcode] [nvarchar](max)
					) as jsondata; 

					--Tran Detail End
					Insert into tblJobWork_dtl(FkId,FKSeriesId,SrNo,FkProductId,FkLotId,FKLocationID,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
						,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark
						,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,FKUserId,ModifiedDate,FKCreatedByID,CreationDate,Barcode
						,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,DueQty,TranType)
					SELECT @PkId,@FKSeriesId,SrNo,FkProductId,FkLotId,@FKLocationID,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
						,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark
						,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate
						,@FKUserId,Getdate(),@FKUserId,Getdate()
						,(select STUFF((SELECT ',' + t1.Barcode from #UniqIdDetail t1 where t1.SrNo=SrNo and t1.TranType=TranType FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)') ,1,1,'') ) as Barcode
						,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,(Qty+FreeQty),TranType
					FROM #Detail where ModeForm=0 
					
					print 'ccc'
					
					UPDATE [dbo].[tblJobWork_dtl]  
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
							,Barcode=(select STUFF((SELECT ',' + t1.Barcode from #UniqIdDetail t1 where t1.SrNo=Dtl.SrNo and t1.TranType=Dtl.TranType FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)') ,1,1,'') )
							,LinkSrNo=Dtl.LinkSrNo
							,PromotionType=Dtl.PromotionType,
							DueQty=(Dtl.Qty+Dtl.FreeQty),
							TranType=Dtl.TranType
					FROM  #Detail AS Dtl  
					Where tblJobWork_dtl.FKID = Dtl.FkId
					And tblJobWork_dtl.FKSeriesID = Dtl.FKSeriesId
					And tblJobWork_dtl.SrNo = Dtl.SrNo And Dtl.ModeForm = 1
								
					Delete From tblJobWork_dtl
					Where tblJobWork_dtl.FKID = @PkId And FKSeriesID=@FKSeriesID
					And tblJobWork_dtl.SrNo In (Select Dtl.SrNo FROM #Detail As Dtl Where Dtl.ModeForm = 2)

					Delete From #Detail Where TranType='I'
					Delete From #UniqIdDetail Where TranType='I'

					 exec usp_ProductStock  'S'  
			 End
			  Else   --Job Order
			 Begin 
					print 'O'
					Insert into #Detail(FkId,FKSeriesId,ModeForm,SrNo,FkProductId,FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
										,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,src
										,FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,Barcode,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,TranType)
					SELECT @PkId,@FKSeriesId,ModeForm,SrNo,FkProductId,@FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
							,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,@src
							,@FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,Barcode,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,TranType
					FROM OPENJSON(@JsonData, '$.TranReturnDetails')
					WITH ([PkId] [bigint] ,[FkId] [bigint],[FKSeriesId] [bigint],[SrNo] [bigint],[ModeForm] [bigint],
					[FkProductId] [bigint],[FkLotId] [bigint],[Rate] [decimal](18, 2),[RateUnit] [nvarchar](max) ,
					[Qty] [decimal](18, 2),[FreeQty] [decimal](18, 2),[SchemeDisc] [decimal](18, 2),
					[SchemeDiscType] [nvarchar](max) ,[SchemeDiscAmt] [decimal](18, 2),[TradeDisc] [decimal](18, 2),
					[TradeDiscType] [nvarchar](max) ,[TradeDiscAmt] [decimal](18, 2),[LotDisc] [decimal](18, 2),
					[GrossAmt] [decimal](18, 2),[TaxableAmt] [decimal](18, 2),[ICRate] [decimal](18, 2),[ICAmt] [decimal](18, 2),[SCRate] [decimal](18, 2),
					[SCAmt] [decimal](18, 2),[NetAmt] [decimal](18, 2),[Remark] [nvarchar](max) ,[Src] [bigint],[FKUserId] [bigint],[Batch] [nvarchar](500) ,
					[Color] [nvarchar](500) ,[MfgDate] [datetime] ,[ExpiryDate] [datetime] ,[MRP] [decimal](18, 2)  ,[SaleRate] [decimal](18, 2) ,[Barcode] [nvarchar](max),
					[FKOrderID] [bigint],[OrderSrNo] [bigint],[FKOrderSrID] [bigint],[LinkSrNo] [bigint] ,[PromotionType] [nvarchar] (10),[TranType][char](1)
					) as jsondata; 
				
					Insert into #UniqIdDetail(SrNo,Barcode)
					SELECT SrNo,Barcode
					FROM OPENJSON(@JsonData, '$.UniqIdReturnDetails')
					WITH ([SrNo] [bigint],[Barcode] [nvarchar](max)
					) as jsondata; 

					--Tran Detail Start
					Insert into #Detail(FkId,FKSeriesId,ModeForm,SrNo,FkProductId,FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
										,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,src
										,FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,Barcode,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,TranType)
					SELECT @PkId,@FKSeriesId,ModeForm,SrNo,FkProductId,@FKLocationID,FkLotId,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
							,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark,@src
							,@FKUserId,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,Barcode,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,TranType
					FROM OPENJSON(@JsonData, '$.TranDetails')
					WITH ([PkId] [bigint] ,[FkId] [bigint],[FKSeriesId] [bigint],[SrNo] [bigint],[ModeForm] [bigint],
					[FkProductId] [bigint],[FkLotId] [bigint],[Rate] [decimal](18, 2),[RateUnit] [nvarchar](max) ,
					[Qty] [decimal](18, 2),[FreeQty] [decimal](18, 2),[SchemeDisc] [decimal](18, 2),
					[SchemeDiscType] [nvarchar](max) ,[SchemeDiscAmt] [decimal](18, 2),[TradeDisc] [decimal](18, 2),
					[TradeDiscType] [nvarchar](max) ,[TradeDiscAmt] [decimal](18, 2),[LotDisc] [decimal](18, 2),
					[GrossAmt] [decimal](18, 2),[TaxableAmt] [decimal](18, 2),[ICRate] [decimal](18, 2),[ICAmt] [decimal](18, 2),[SCRate] [decimal](18, 2),
					[SCAmt] [decimal](18, 2),[NetAmt] [decimal](18, 2),[Remark] [nvarchar](max) ,[Src] [bigint],[FKUserId] [bigint],[Batch] [nvarchar](500) ,
					[Color] [nvarchar](500) ,[MfgDate] [datetime] ,[ExpiryDate] [datetime] ,[MRP] [decimal](18, 2)  ,[SaleRate] [decimal](18, 2) ,[Barcode] [nvarchar](max),
					[FKOrderID] [bigint],[OrderSrNo] [bigint],[FKOrderSrID] [bigint],[LinkSrNo] [bigint] ,[PromotionType] [nvarchar] (10),[TranType][char](1)
					) as jsondata; 
				
					Insert into #UniqIdDetail(SrNo,Barcode)
					SELECT SrNo,Barcode
					FROM OPENJSON(@JsonData, '$.UniqIdDetails')
					WITH ([SrNo] [bigint],[Barcode] [nvarchar](max)
					) as jsondata; 

					--Tran Detail End
					Insert into tblJobWork_dtl(FkId,FKSeriesId,SrNo,FkProductId,FkLotId,FKLocationID,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
						,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark
						,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate,FKUserId,ModifiedDate,FKCreatedByID,CreationDate,Barcode
						,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,DueQty,TranType)
					SELECT @PkId,@FKSeriesId,SrNo,FkProductId,FkLotId,@FKLocationID,Rate,RateUnit,Qty,FreeQty,SchemeDisc,SchemeDiscType,SchemeDiscAmt
						,TradeDisc,TradeDiscType,TradeDiscAmt,LotDisc,GrossAmt,TaxableAmt,ICRate,ICAmt,SCRate,SCAmt,NetAmt,Remark
						,Batch,Color,MfgDate,ExpiryDate,MRP,SaleRate
						,@FKUserId,Getdate(),@FKUserId,Getdate()
						,(select STUFF((SELECT ',' + t1.Barcode from #UniqIdDetail t1 where t1.SrNo=SrNo and t1.TranType=TranType FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)') ,1,1,'') ) as Barcode
						,FKOrderID,OrderSrNo,FKOrderSrID,LinkSrNo,PromotionType,(Qty+FreeQty),TranType
					FROM #Detail where ModeForm=0 
					
					print 'ccc'
					
					UPDATE [dbo].[tblJobWork_dtl]  
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
							,Barcode=(select STUFF((SELECT ',' + t1.Barcode from #UniqIdDetail t1 where t1.SrNo=Dtl.SrNo and t1.TranType=Dtl.TranType  FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)') ,1,1,'') )
							,LinkSrNo=Dtl.LinkSrNo
							,PromotionType=Dtl.PromotionType,
							DueQty=(Dtl.Qty+Dtl.FreeQty),
							TranType=Dtl.TranType
					FROM  #Detail AS Dtl  
					Where tblJobWork_dtl.FKID = Dtl.FkId
					And tblJobWork_dtl.FKSeriesID = Dtl.FKSeriesId
					And tblJobWork_dtl.SrNo = Dtl.SrNo And Dtl.ModeForm = 1
								
					Delete From tblJobWork_dtl
					Where tblJobWork_dtl.FKID = @PkId And FKSeriesID=@FKSeriesID
					And tblJobWork_dtl.SrNo In (Select Dtl.SrNo FROM #Detail As Dtl Where Dtl.ModeForm = 2) 
					 
			 End
  
end