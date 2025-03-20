IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblVendor_mas' AND COLUMN_NAME = 'FkAccountId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin

ALTER TABLE [dbo].[tblVendor_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblVendor_mas_tblAccount_mas] FOREIGN KEY([FkAccountId])
REFERENCES [dbo].[tblAccount_mas] ([PkAccountId])
 

ALTER TABLE [dbo].[tblVendor_mas] CHECK CONSTRAINT [FK_tblVendor_mas_tblAccount_mas]
 

END

