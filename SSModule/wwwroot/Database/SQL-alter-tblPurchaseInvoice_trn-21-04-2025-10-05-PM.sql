IF COL_LENGTH('dbo.tblPurchaseInvoice_trn', 'FKReferById') IS  NULL
BEGIN
     Alter Table tblPurchaseInvoice_trn
	 Add FKReferById bigint
END

IF COL_LENGTH('dbo.tblPurchaseInvoice_trn', 'FKSalesPerId') IS  NULL
BEGIN
     Alter Table tblPurchaseInvoice_trn
	 Add FKSalesPerId bigint
END