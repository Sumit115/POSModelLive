ALTER procedure [dbo].[usp_ProductStock]
( 
	@TranFlag nchar(1)
)
as 
begin 
		 
						UPDATE [dbo].[tblProdStock_dtl]  
						SET  tblProdStock_dtl.FkProductId=Trn.FkProductId 
						,tblProdStock_dtl.FKLocationID=Trn.FKLocationID
						,tblProdStock_dtl.FKLotID=Trn.FKLotID
						--,InStock += case when @TranFlag='P' then (Trn.Qty+Trn.FreeQty) else InStock end
						--,OutStock+= case when @TranFlag='S' then (Trn.Qty+Trn.FreeQty) else OutStock end
						--,OpStock += case when @TranFlag='O' then (Trn.Qty+Trn.FreeQty) else OpStock end
						,InStock += case when @TranFlag='P' then (Trn.Qty+Trn.FreeQty) else 0 end
						,OutStock+= case when @TranFlag='S' then (Trn.Qty+Trn.FreeQty) else 0 end
						,OpStock += case when @TranFlag='O' then (Trn.Qty+Trn.FreeQty) else 0 end
						--,CurStock= ((case when @TranFlag='O' then Trn.Qty else 0 end)+(case when @TranFlag='P' then Trn.Qty else 0 end)-(case when @TranFlag='S' then Trn.Qty else 0 end)-AdjStock)
						FROM  #Detail AS Trn  
						Where tblProdStock_dtl.FKProductId=Trn.FkProductId 
						and tblProdStock_dtl.FKLotID=Trn.FkLotId  
						and tblProdStock_dtl.FKLocationID=Trn.FKLocationID 
		
						UPDATE [dbo].[tblProdStock_dtl]  
						SET   CurStock=(InStock+OpStock-OutStock-AdjStock) 
					 	FROM  #Detail AS Trn  
						Where tblProdStock_dtl.FKProductId=Trn.FkProductId 
						and tblProdStock_dtl.FKLotID=Trn.FkLotId  
						and tblProdStock_dtl.FKLocationID=Trn.FKLocationID 


						if(@TranFlag = 'P')
						begin 
						Insert into tblProdStock_dtl(FKProductId,FKLocationID,FKLotID,OpStock,InStock,OutStock,CurStock,StockDate,FKUserID,ModifiedDate,FKCreatedByID,CreationDate)
						SELECT FkProductId,FKLocationID,FkLotId,0,(Qty+FreeQty),0,(Qty+FreeQty),GETDATE(),1,GETDATE(),1,GETDATE() FROM #Detail where ModeForm=0
						and FkProductId not in (Select FkProductId from tblProdStock_dtl)   
						and FKLocationID not in (Select FKLocationID from tblProdStock_dtl)   
						and FkLotId not in (Select FkLotId from tblProdStock_dtl)   

				 		 delete tblProdStock_dtl from tblProdStock_dtl 
						 left outer join 
						 #Detail AS Trn 
						 on tblProdStock_dtl.FKProductId=Trn.FkProductId 
						and tblProdStock_dtl.FKLotID=Trn.FkLotId  
						and tblProdStock_dtl.FKLocationID=Trn.FKLocationID 
						Where Trn.ModeForm=2 
						end
		
	  
end
