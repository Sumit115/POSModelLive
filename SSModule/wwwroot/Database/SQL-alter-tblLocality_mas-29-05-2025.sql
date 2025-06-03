

IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblLocality_mas' AND COLUMN_NAME = 'FkAreaId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin

 
ALTER TABLE [dbo].[tblLocality_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblLocality_mas_tblArea_mas] FOREIGN KEY([FkAreaId])
REFERENCES [dbo].[tblArea_mas] ([PkAreaId])
 

ALTER TABLE [dbo].[tblLocality_mas] CHECK CONSTRAINT [FK_tblLocality_mas_tblArea_mas]
 

END