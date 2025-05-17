IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblDistrict_mas' AND COLUMN_NAME = 'FkStateId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin

ALTER TABLE [dbo].[tblDistrict_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblDistrict_mas_tblState_mas] FOREIGN KEY([FkStateId])
REFERENCES [dbo].[tblState_mas] ([PkStateId])
 

ALTER TABLE [dbo].[tblDistrict_mas] CHECK CONSTRAINT [FK_tblDistrict_mas_tblState_mas]
 

END