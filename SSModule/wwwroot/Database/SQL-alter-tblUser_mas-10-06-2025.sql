Update tblUser_mas set FkRoleId=null where FkRoleId not in (Select PkRoleId from tblRole_mas)
 

IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblUser_mas' AND COLUMN_NAME = 'FkRoleId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin


ALTER TABLE [dbo].[tblUser_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblUser_mas_tblRole_mas] FOREIGN KEY([FkRoleId])
REFERENCES [dbo].[tblRole_mas] ([PkRoleId])
 

ALTER TABLE [dbo].[tblUser_mas] CHECK CONSTRAINT [FK_tblUser_mas_tblRole_mas]
 

END


Update tblUser_mas set FkEmployeeId=null where FkEmployeeId not in (Select PkEmployeeId from tblEmployee_mas)
 

IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblUser_mas' AND COLUMN_NAME = 'FkEmployeeId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin


ALTER TABLE [dbo].[tblUser_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblUser_mas_tblEmployee_mas] FOREIGN KEY([FkEmployeeId])
REFERENCES [dbo].[tblEmployee_mas] ([PkEmployeeId])
 

ALTER TABLE [dbo].[tblUser_mas] CHECK CONSTRAINT [FK_tblUser_mas_tblEmployee_mas]
 

END


Update tblUser_mas set FkBranchId=null where FkBranchId not in (Select PkBranchId from tblBranch_mas)
 

IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblUser_mas' AND COLUMN_NAME = 'FkBranchId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin


ALTER TABLE [dbo].[tblUser_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblUser_mas_tblBranch_mas] FOREIGN KEY([FkBranchId])
REFERENCES [dbo].[tblBranch_mas] ([PkBranchId])
 

ALTER TABLE [dbo].[tblUser_mas] CHECK CONSTRAINT [FK_tblUser_mas_tblBranch_mas]
 

END