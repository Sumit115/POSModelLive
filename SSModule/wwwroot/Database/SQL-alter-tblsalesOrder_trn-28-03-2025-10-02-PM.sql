 
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'BookingStation' AND Object_ID = Object_ID(N'dbo.tblSalesOrder_trn'))
Begin
	Alter Table [tblSalesOrder_trn] Add [BookingStation] nvarchar(50)
 
End 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'TransportName' AND Object_ID = Object_ID(N'dbo.tblSalesOrder_trn'))
Begin
	Alter Table [tblSalesOrder_trn] Add [TransportName] nvarchar(50)
 
End 
