IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'PartyName' AND Object_ID = Object_ID(N'dbo.tblSalesOrder_trn'))
 Alter Table tblSalesOrder_trn Add PartyName nvarchar(100) 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'PartyMobile' AND Object_ID = Object_ID(N'dbo.tblSalesOrder_trn'))
 Alter Table tblSalesOrder_trn Add PartyMobile nvarchar(100) 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'PartyAddress' AND Object_ID = Object_ID(N'dbo.tblSalesOrder_trn'))
 Alter Table tblSalesOrder_trn Add PartyAddress nvarchar(max) 
  


