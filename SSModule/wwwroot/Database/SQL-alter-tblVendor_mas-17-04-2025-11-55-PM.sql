Alter Table tblVendor_mas
Alter Column FkCityId bigint

IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblVendor_mas' AND COLUMN_NAME = 'FkCityId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin

ALTER TABLE [dbo].[tblVendor_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblVendor_mas_tblCity_mas] FOREIGN KEY([FkCityId])
REFERENCES [dbo].[tblCity_mas] ([PkCityId])
 

ALTER TABLE [dbo].[tblVendor_mas] CHECK CONSTRAINT [FK_tblVendor_mas_tblCity_mas]
 

END