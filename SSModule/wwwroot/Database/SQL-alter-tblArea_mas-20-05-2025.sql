

IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblArea_mas' AND COLUMN_NAME = 'FkRegionId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin

 
ALTER TABLE [dbo].[tblArea_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblArea_mas_tblRegion_mas] FOREIGN KEY([FkRegionId])
REFERENCES [dbo].[tblRegion_mas] ([PkRegionId])
 

ALTER TABLE [dbo].[tblArea_mas] CHECK CONSTRAINT [FK_tblArea_mas_tblRegion_mas]
 

END