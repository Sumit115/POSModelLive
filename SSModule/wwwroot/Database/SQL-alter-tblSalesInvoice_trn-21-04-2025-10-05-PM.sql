IF COL_LENGTH('dbo.tblSalesInvoice_trn', 'FKReferById') IS  NULL
BEGIN
     Alter Table tblSalesInvoice_trn
	 Add FKReferById bigint
END

IF COL_LENGTH('dbo.tblSalesInvoice_trn', 'FKSalesPerId') IS  NULL
BEGIN
     Alter Table tblSalesInvoice_trn
	 Add FKSalesPerId bigint
END