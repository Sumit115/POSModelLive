Update tblSeries_mas set FkLocationId=null where FkLocationId not in (Select PkLocationId from tblLocation_mas)
 

IF Not EXISTS(SELECT CONSTRAINT_NAME FROM information_schema.KEY_COLUMN_USAGE 
WHERE TABLE_NAME = 'tblSeries_mas' AND COLUMN_NAME = 'FkLocationId' AND CONSTRAINT_NAME IN ( SELECT CONSTRAINT_NAME FROM information_schema.TABLE_CONSTRAINTS WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'))
Begin


ALTER TABLE [dbo].[tblSeries_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblSeries_mas_tblLocation_mas] FOREIGN KEY([FkLocationId])
REFERENCES [dbo].[tblLocation_mas] ([PkLocationId])
 

ALTER TABLE [dbo].[tblSeries_mas] CHECK CONSTRAINT [FK_tblSeries_mas_tblLocation_mas]
 

END