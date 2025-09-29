IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'IsDelete' AND Object_ID = Object_ID(N'dbo.tblRole_Dtl'))
 Alter Table tblRole_Dtl Add IsDelete bit not null default 0
