IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'FkPromotionId' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_dtl'))
 Alter Table tblSalesInvoice_dtl Add FkPromotionId bigint  