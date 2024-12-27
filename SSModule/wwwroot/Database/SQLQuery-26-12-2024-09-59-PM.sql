IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'Barcode' AND Object_ID = Object_ID(N'dbo.tblPurchaseOrder_dtl'))
 Alter Table tblPurchaseOrder_dtl Add Barcode nvarchar(max)  

IF EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'Barcode' AND Object_ID = Object_ID(N'dbo.tblPurchaseOrder_dtl'))
 Alter Table tblPurchaseOrder_dtl Alter Column Barcode nvarchar(max)  

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'Barcode' AND Object_ID = Object_ID(N'dbo.tblPurchaseInvoice_dtl'))
 Alter Table tblPurchaseInvoice_dtl Add Barcode nvarchar(max)  

IF EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'Barcode' AND Object_ID = Object_ID(N'dbo.tblPurchaseInvoice_dtl'))
 Alter Table tblPurchaseInvoice_dtl Alter Column Barcode nvarchar(max)  

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'Barcode' AND Object_ID = Object_ID(N'dbo.tblSalesOrder_dtl'))
 Alter Table tblSalesOrder_dtl Add Barcode nvarchar(max)  

IF EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'Barcode' AND Object_ID = Object_ID(N'dbo.tblSalesOrder_dtl'))
 Alter Table tblSalesOrder_dtl Alter Column Barcode nvarchar(max)  

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'Barcode' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_dtl'))
 Alter Table tblSalesInvoice_dtl Add Barcode nvarchar(max)  

IF EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'Barcode' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_dtl'))
 Alter Table tblSalesInvoice_dtl Alter Column Barcode nvarchar(max)  
