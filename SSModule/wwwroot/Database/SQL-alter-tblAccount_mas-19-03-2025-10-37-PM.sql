IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblAccount_mas' AND COLUMN_NAME = 'FkAccountGroupId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin

ALTER TABLE [dbo].[tblAccount_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblAccount_mas_tblAccountGroup_mas] FOREIGN KEY([FkAccountGroupId])
REFERENCES [dbo].[tblAccountGroup_mas] ([PkAccountGroupId])
 

ALTER TABLE [dbo].[tblAccount_mas] CHECK CONSTRAINT [FK_tblAccount_mas_tblAccountGroup_mas]
 

END


IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblAccount_mas' AND COLUMN_NAME = 'FkBankId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin

Update tblAccount_mas set FKBankID=null where FKBankID=0 OR FKBankID not in (select PkBankId from tblBank_mas)

ALTER TABLE [dbo].[tblAccount_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblAccount_mas_tblBank_mas] FOREIGN KEY([FkBankId])
REFERENCES [dbo].[tblBank_mas] ([PkBankId])
 

ALTER TABLE [dbo].[tblAccount_mas] CHECK CONSTRAINT [FK_tblAccount_mas_tblBank_mas]
 

END