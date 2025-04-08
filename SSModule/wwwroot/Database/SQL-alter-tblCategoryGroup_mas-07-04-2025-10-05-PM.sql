IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblCategoryGroup_mas' AND COLUMN_NAME = 'FkCategoryGroupId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin

 Update tblCategoryGroup_mas set FkCategoryGroupId=null where FkCategoryGroupId not in (Select PkCategoryGroupId from tblCategoryGroup_mas)

ALTER TABLE [dbo].[tblCategoryGroup_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblCategoryGroup_mas_tblCategoryGroup_mas] FOREIGN KEY([FkCategoryGroupId])
REFERENCES [dbo].[tblCategoryGroup_mas] ([PkCategoryGroupId])
 

ALTER TABLE [dbo].[tblCategoryGroup_mas] CHECK CONSTRAINT [FK_tblCategoryGroup_mas_tblCategoryGroup_mas]
 

END

