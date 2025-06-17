

IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblProduct_mas' AND COLUMN_NAME = 'FkBrandId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin

 
ALTER TABLE [dbo].[tblProduct_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblProduct_mas_tblBrand_mas] FOREIGN KEY([FkBrandId])
REFERENCES [dbo].[tblBrand_mas] ([PkBrandId])
 

ALTER TABLE [dbo].[tblProduct_mas] CHECK CONSTRAINT [FK_tblProduct_mas_tblBrand_mas]
 

END



IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblProduct_mas' AND COLUMN_NAME = 'FKProdCatgId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin

 
ALTER TABLE [dbo].[tblProduct_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblProduct_mas_tblCaegory_mas] FOREIGN KEY([FKProdCatgId])
REFERENCES [dbo].[tblCaegory_mas] ([PkCaegoryId])
 

ALTER TABLE [dbo].[tblProduct_mas] CHECK CONSTRAINT [FK_tblProduct_mas_tblCaegory_mas]
 

END



IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblProduct_mas' AND COLUMN_NAME = 'FkUnitId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin

 
ALTER TABLE [dbo].[tblProduct_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblProduct_mas_tblUnit_mas] FOREIGN KEY([FkUnitId])
REFERENCES [dbo].[tblUnit_mas] ([PkUnitId])
 

ALTER TABLE [dbo].[tblProduct_mas] CHECK CONSTRAINT [FK_tblProduct_mas_tblUnit_mas]
 

END