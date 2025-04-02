IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'BiltyNo' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_trn'))
Begin
	Alter Table [tblSalesInvoice_trn] Add [BiltyNo] [nvarchar](30)
End 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'BiltyDate' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_trn'))
Begin
	Alter Table [tblSalesInvoice_trn] Add [BiltyDate] [date]
End 
 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'TransportName' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_trn'))
Begin
	Alter Table [tblSalesInvoice_trn] Add [TransportName] [nvarchar](50)
End 
 
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'NoOfCases' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_trn'))
Begin
	Alter Table [tblSalesInvoice_trn] Add [NoOfCases] [bigint]
End 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'FreightType' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_trn'))
Begin
	Alter Table [tblSalesInvoice_trn] Add [FreightType] [nvarchar](20)
End 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'FreightAmt' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_trn'))
Begin
	Alter Table [tblSalesInvoice_trn] Add [FreightAmt] [decimal](18, 4)
End 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'ShipingAddress' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_trn'))
Begin
	Alter Table [tblSalesInvoice_trn] Add [ShipingAddress] [nvarchar](500) NULL
End 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'PaymentMode' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_trn'))
Begin
	Alter Table [tblSalesInvoice_trn] Add [PaymentMode] [nchar](1)  
End 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'FKBankThroughBankID' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_trn'))
Begin
	Alter Table [tblSalesInvoice_trn] Add [FKBankThroughBankID] [bigint]
End 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'DeliveryDate' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_trn'))
Begin
	Alter Table [tblSalesInvoice_trn] Add [DeliveryDate] [date]
End 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'ShippingMode' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_trn'))
Begin
	Alter Table [tblSalesInvoice_trn] Add [ShippingMode] [nchar](1)  
End 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'PaymentDays' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_trn'))
Begin
	Alter Table [tblSalesInvoice_trn] Add [PaymentDays] [int]   
End 