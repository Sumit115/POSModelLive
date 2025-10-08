IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'CouponDiscount' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_trn'))
 Alter Table tblSalesInvoice_trn Add CouponDiscount decimal(18,2) not null default 0

 