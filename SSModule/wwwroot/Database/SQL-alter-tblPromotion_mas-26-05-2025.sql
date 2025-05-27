 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'PromotionApplyQty2' AND Object_ID = Object_ID(N'dbo.tblPromotion_mas'))
Begin
	Alter Table [tblPromotion_mas] Add [PromotionApplyQty2] [decimal](18,2) 
End 
 
