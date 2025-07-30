IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'EditBatch' AND Object_ID = Object_ID(N'dbo.tblUser_mas'))
Begin
	Alter Table [tblUser_mas] Add [EditBatch] Bit
End 
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'EditColor' AND Object_ID = Object_ID(N'dbo.tblUser_mas'))
Begin
	Alter Table [tblUser_mas] Add [EditColor] Bit
End 
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'EditDiscount' AND Object_ID = Object_ID(N'dbo.tblUser_mas'))
Begin
	Alter Table [tblUser_mas] Add [EditDiscount] Bit
End 
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'EditRate' AND Object_ID = Object_ID(N'dbo.tblUser_mas'))
Begin
	Alter Table [tblUser_mas] Add [EditRate] Bit
End 
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'EditMRP' AND Object_ID = Object_ID(N'dbo.tblUser_mas'))
Begin
	Alter Table [tblUser_mas] Add [EditMRP] Bit
End 
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'EditPurRate' AND Object_ID = Object_ID(N'dbo.tblUser_mas'))
Begin
	Alter Table [tblUser_mas] Add [EditPurRate] Bit
End 
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'EditPurDiscount' AND Object_ID = Object_ID(N'dbo.tblUser_mas'))
Begin
	Alter Table [tblUser_mas] Add [EditPurDiscount] Bit
End  