IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'FKLastUserId' AND Object_ID = Object_ID(N'dbo.tblMasterLog_Dtl'))
	Alter Table tblMasterLog_Dtl
	Add FKLastUserId bigint
 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'LastModifyDate' AND Object_ID = Object_ID(N'dbo.tblMasterLog_Dtl'))
	Alter Table tblMasterLog_Dtl
	Add LastModifyDate DateTime
  