
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'FkHoldLocationId' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_trn'))
 Alter Table tblSalesInvoice_trn Add FkHoldLocationId bigint  