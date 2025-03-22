IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblProduct_mas' AND COLUMN_NAME = 'FkBrandId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin

ALTER TABLE [dbo].[tblProduct_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblProduct_mas_tblBrand_mas] FOREIGN KEY([FkBrandId])
REFERENCES [dbo].[tblBrand_mas] ([PkBrandId])
 

ALTER TABLE [dbo].[tblProduct_mas] CHECK CONSTRAINT [FK_tblProduct_mas_tblBrand_mas]
 

END

