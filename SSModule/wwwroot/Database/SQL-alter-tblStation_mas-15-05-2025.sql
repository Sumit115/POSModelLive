IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblStation_mas' AND COLUMN_NAME = 'FkDistrictId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin

ALTER TABLE [dbo].[tblStation_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblStation_mas_tblDistrict_mas] FOREIGN KEY([FkDistrictId])
REFERENCES [dbo].[tblDistrict_mas] ([PkDistrictId])
 

ALTER TABLE [dbo].[tblStation_mas] CHECK CONSTRAINT [FK_tblStation_mas_tblDistrict_mas]
 

END