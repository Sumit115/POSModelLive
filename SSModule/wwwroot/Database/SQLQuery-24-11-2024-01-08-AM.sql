
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'LinkSrNo' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_dtl'))
 Alter Table tblSalesInvoice_dtl Add LinkSrNo bigint 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'PromotionType' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_dtl'))
 Alter Table tblSalesInvoice_dtl Add PromotionType nvarchar(20) 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'LinkSrNo' AND Object_ID = Object_ID(N'dbo.tblSalesOrder_dtl'))
 Alter Table tblSalesOrder_dtl Add LinkSrNo bigint 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'PromotionType' AND Object_ID = Object_ID(N'dbo.tblSalesOrder_dtl'))
 Alter Table tblSalesOrder_dtl Add PromotionType nvarchar(20) 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'LinkSrNo' AND Object_ID = Object_ID(N'dbo.tblPurchaseInvoice_dtl'))
 Alter Table tblPurchaseInvoice_dtl Add LinkSrNo bigint 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'PromotionType' AND Object_ID = Object_ID(N'dbo.tblPurchaseInvoice_dtl'))
 Alter Table tblPurchaseInvoice_dtl Add PromotionType nvarchar(20) 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'LinkSrNo' AND Object_ID = Object_ID(N'dbo.tblPurchaseOrder_dtl'))
 Alter Table tblPurchaseOrder_dtl Add LinkSrNo bigint 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'PromotionType' AND Object_ID = Object_ID(N'dbo.tblPurchaseOrder_dtl'))
 Alter Table tblPurchaseOrder_dtl Add PromotionType nvarchar(20) 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'FreePoint' AND Object_ID = Object_ID(N'dbo.tblSalesInvoice_Trn'))
 Alter Table tblSalesInvoice_Trn Add FreePoint decimal(18,2) not null default 0
 
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'FreePoint' AND Object_ID = Object_ID(N'dbo.tblSalesOrder_Trn'))
 Alter Table tblSalesOrder_Trn Add FreePoint decimal(18,2) not null default 0
 
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'FreePoint' AND Object_ID = Object_ID(N'dbo.tblPurchaseInvoice_Trn'))
 Alter Table tblPurchaseInvoice_Trn Add FreePoint decimal(18,2) not null default 0
 
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'FreePoint' AND Object_ID = Object_ID(N'dbo.tblPurchaseOrder_Trn'))
 Alter Table tblPurchaseOrder_Trn Add FreePoint decimal(18,2) not null default 0
 
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'FreePoint' AND Object_ID = Object_ID(N'dbo.TblCustomer_mas'))
 Alter Table TblCustomer_mas Add FreePoint decimal(18,2) not null default 0
 
IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'FreePoint' AND Object_ID = Object_ID(N'dbo.TblVendor_mas'))
 Alter Table TblVendor_mas Add FreePoint decimal(18,2) not null default 0

 IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'OrderScheduleDate' AND Object_ID = Object_ID(N'dbo.tblSalesOrder_trn'))
 Alter Table tblSalesOrder_trn Add OrderScheduleDate Datetime 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'ConcernPersonName' AND Object_ID = Object_ID(N'dbo.tblSalesOrder_trn'))
 Alter Table tblSalesOrder_trn Add ConcernPersonName nvarchar(100) 

IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'ConcernPersonMobile' AND Object_ID = Object_ID(N'dbo.tblSalesOrder_trn'))
 Alter Table tblSalesOrder_trn Add ConcernPersonMobile nvarchar(20) 
 
