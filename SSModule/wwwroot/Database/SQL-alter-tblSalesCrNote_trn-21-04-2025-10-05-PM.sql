IF COL_LENGTH('dbo.tblSalesCrNote_trn', 'FKReferById') IS  NULL
BEGIN
     Alter Table tblSalesCrNote_trn
	 Add FKReferById bigint
END

IF COL_LENGTH('dbo.tblSalesCrNote_trn', 'FKSalesPerId') IS  NULL
BEGIN
     Alter Table tblSalesCrNote_trn
	 Add FKSalesPerId bigint
END