IF COL_LENGTH('dbo.tblPurchaseOrder_trn', 'FKReferById') IS  NULL
BEGIN
     Alter Table tblPurchaseOrder_trn
	 Add FKReferById bigint
END

IF COL_LENGTH('dbo.tblPurchaseOrder_trn', 'FKSalesPerId') IS  NULL
BEGIN
     Alter Table tblPurchaseOrder_trn
	 Add FKSalesPerId bigint
END