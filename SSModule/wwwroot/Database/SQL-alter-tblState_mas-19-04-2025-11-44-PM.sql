IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblState_mas' AND COLUMN_NAME = 'FkCountryId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin

ALTER TABLE [dbo].[tblState_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblState_mas_tblCountry_mas] FOREIGN KEY([FkCountryId])
REFERENCES [dbo].[tblCountry_mas] ([PkCountryId])
 

ALTER TABLE [dbo].[tblState_mas] CHECK CONSTRAINT [FK_tblState_mas_tblCountry_mas]
 

END
