

IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblRegion_mas' AND COLUMN_NAME = 'FkZoneId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin

 
ALTER TABLE [dbo].[tblRegion_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblRegion_mas_tblZone_mas] FOREIGN KEY([FkZoneId])
REFERENCES [dbo].[tblZone_mas] ([PkZoneId])
 

ALTER TABLE [dbo].[tblRegion_mas] CHECK CONSTRAINT [FK_tblRegion_mas_tblZone_mas]
 

END