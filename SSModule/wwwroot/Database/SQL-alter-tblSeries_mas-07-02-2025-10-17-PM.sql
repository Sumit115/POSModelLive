IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'TaxType' AND Object_ID = Object_ID(N'dbo.tblSeries_mas'))
 Alter table tblSeries_mas Add TaxType char default 'I'

IF EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'FkBranchId' AND Object_ID = Object_ID(N'dbo.tblSeries_mas'))
 Alter table tblSeries_mas Drop Column FkBranchId
 