IF Not EXISTS(SELECT 1 FROM sys.columns  WHERE Name = N'Barcode' AND Object_ID = Object_ID(N'dbo.tblSalesCrNote_dtl'))
 Alter Table tblSalesCrNote_dtl Add Barcode nvarchar(max)