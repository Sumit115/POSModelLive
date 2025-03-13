 
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'FkLocalityId' AND Object_ID = Object_ID(N'dbo.tblAccount_mas'))
Begin
	Alter Table [tblAccount_mas] Add [FkLocalityId] bigint

ALTER TABLE [dbo].[tblAccount_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblAccount_mas_tblLocality_mas] FOREIGN KEY([FKLocalityId])
REFERENCES [dbo].[tblLocality_mas] ([PkLocalityId])
 

ALTER TABLE [dbo].[tblAccount_mas] CHECK CONSTRAINT [FK_tblAccount_mas_tblLocality_mas]
End 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'FkStationId' AND Object_ID = Object_ID(N'dbo.tblAccount_mas'))
Begin
	Alter Table [tblAccount_mas] Add [FkStationId] bigint

ALTER TABLE [dbo].[tblAccount_mas]  WITH CHECK ADD  CONSTRAINT [FK_tblAccount_mas_tblStation_mas] FOREIGN KEY([FKStationID])
REFERENCES [dbo].[tblStation_mas] ([PkStationId])
 

ALTER TABLE [dbo].[tblAccount_mas] CHECK CONSTRAINT [FK_tblAccount_mas_tblStation_mas]
End  


IF   EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'Locality' AND Object_ID = Object_ID(N'dbo.tblAccount_mas')) 
	Alter Table [tblAccount_mas] Drop Column [Locality] 

IF   EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'Station' AND Object_ID = Object_ID(N'dbo.tblAccount_mas')) 
	Alter Table [tblAccount_mas] Drop Column [Station] 

