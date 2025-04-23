IF COL_LENGTH('dbo.tblSalesOrder_trn', 'FKReferById') IS  NULL
BEGIN
     Alter Table tblSalesOrder_trn
	 Add FKReferById bigint
END

IF COL_LENGTH('dbo.tblSalesOrder_trn', 'FKSalesPerId') IS  NULL
BEGIN
     Alter Table tblSalesOrder_trn
	 Add FKSalesPerId bigint
END