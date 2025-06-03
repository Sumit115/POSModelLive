
delete tblPromotionLocation_lnk where FkLocationId not in (Select PkLocationId from tblLocation_mas)
 

IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblPromotionLocation_lnk' AND COLUMN_NAME = 'FkLocationId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin


ALTER TABLE [dbo].[tblPromotionLocation_lnk]  WITH CHECK ADD  CONSTRAINT [FK_tblPromotionLocation_lnk_tblLocation_mas] FOREIGN KEY([FkLocationId])
REFERENCES [dbo].[tblLocation_mas] ([PkLocationId])
 

ALTER TABLE [dbo].[tblPromotionLocation_lnk] CHECK CONSTRAINT [FK_tblPromotionLocation_lnk_tblLocation_mas]
 

END