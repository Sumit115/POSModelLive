Update tblLocation_mas set FkBranchId=null where FkBranchId not in (Select PkBranchId from tblBranch_mas)
 

IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblLocation_mas' AND COLUMN_NAME = 'FkBranchId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin


ALTER TABLE [dbo].[tblLocation_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblLocation_mas_tblBranch_mas] FOREIGN KEY([FkBranchId])
REFERENCES [dbo].[tblBranch_mas] ([PkBranchId])
 

ALTER TABLE [dbo].[tblLocation_mas] CHECK CONSTRAINT [FK_tblLocation_mas_tblBranch_mas]
 

END