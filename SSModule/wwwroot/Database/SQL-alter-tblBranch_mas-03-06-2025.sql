

IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblBranch_mas' AND COLUMN_NAME = 'FkCityId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin

 
ALTER TABLE [dbo].[tblBranch_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblBranch_mas_tblCity_mas] FOREIGN KEY([FkCityId])
REFERENCES [dbo].[tblCity_mas] ([PkCityId])
 

ALTER TABLE [dbo].[tblBranch_mas] CHECK CONSTRAINT [FK_tblBranch_mas_tblCity_mas]
 

END